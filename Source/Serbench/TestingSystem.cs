using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

using NFX;
using NFX.ServiceModel;
using NFX.Environment;
using NFX.Log;
using NFX.DataAccess.CRUD;

using Serbench.Data;

namespace Serbench
{
    /// <summary>
    /// Provides a base fo test execution environment
    /// </summary>
    public class TestingSystem : Service
    {
      #region CONSTS

        public const string CONFIG_TESTS_SECTION = "tests";
        public const string CONFIG_SERIALIZERS_SECTION = "serializers";
        public const string CONFIG_TEST_SECTION = "test";
        public const string CONFIG_SERIALIZER_SECTION = "serializer";

        public const int DEFAULT_STREAM_CAPACITY = 64 * 1024 * 1024;
        public const int MIN_STREAM_CAPACITY = 32 * 1024;

      #endregion

      #region .ctor/.dctor
        public TestingSystem() : base() {}

        protected override void Destructor()
        {
          base.Destructor();
        }

      #endregion

      #region Fields

        private OrderedRegistry<Test> m_Tests = new OrderedRegistry<Test>();
        private OrderedRegistry<Serializer> m_Serializers = new OrderedRegistry<Serializer>();

        private Thread m_Thread;

        private int m_StreamCapacity = DEFAULT_STREAM_CAPACITY;
      #endregion


      #region Properties

        /// <summary>
        /// Returns tests configured to run 
        /// </summary>
        public IRegistry<Test> Tests { get{ return m_Tests;}} 

        /// <summary>
        /// Returns serializers configured to run 
        /// </summary>
        public IRegistry<Serializer> Serililizers { get{ return m_Serializers;}} 


        [Config(Default=DEFAULT_STREAM_CAPACITY)]
        public int StreamCapacity
        {
          get { return m_StreamCapacity;}
          set
          {
            CheckServiceInactive();
            m_StreamCapacity = value < MIN_STREAM_CAPACITY ? MIN_STREAM_CAPACITY : value;  
          }
        }

        /// <summary>
        /// References data store used to store the test run results
        /// </summary>
        public ITestDataStore DataStore
        {
          get
          {
             var ds = App.DataStore as ITestDataStore;
             if (ds==null)
               throw new SerbenchException("The configured app data store is not ITestDataStore. The tool has no output to save into. Revise data-store config element");

             return ds;
          }
        }

      #endregion


      #region Svc Protected

        protected override void DoConfigure(IConfigSectionNode node)
        {
          base.DoConfigure(node);

          foreach(var tnode in node[CONFIG_TESTS_SECTION].Children.Where(cn => cn.IsSameName(CONFIG_TEST_SECTION)))
          {
            var item = FactoryUtils.Make<Test>(tnode, args: new object[]{this, tnode});
            m_Tests.Register( item );
            log(MessageType.Info, "conf tests", "Added test {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }

          if (m_Tests.Count==0)
           throw new SerbenchException("No test specified in config. Nothing to do");


          foreach(var snode in node[CONFIG_SERIALIZERS_SECTION].Children.Where(cn => cn.IsSameName(CONFIG_SERIALIZER_SECTION)))
          {
            var item = FactoryUtils.Make<Serializer>(snode, args: new object[]{this, snode});
            m_Serializers.Register( item  );
            log(MessageType.Info, "conf sers", "Added serializer {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }

          if (m_Serializers.Count==0)
           throw new SerbenchException("No serializers specified in config. Nothing to do");
        }

        protected override void DoStart()
        {
          var ds = DataStore;//this will thorw if DataStore is not configured properly
          
          m_Thread = new Thread(threadBody);
          m_Thread.IsBackground = false;
          m_Thread.Name = "{0} Main Thread".Args(GetType().FullName);
          m_Thread.Start();
        }

        protected override void DoWaitForCompleteStop()
        {
          if (m_Thread!=null)
          {
            m_Thread.Join();
            m_Thread = null;
          }
        }


      #endregion

      #region .pvt

       private void log(MessageType tp, string from, string text, Exception err = null)
       {
         App.Log.Write(
           new Message
           {
             Type = tp,
              Topic = "TestingSystem",
               From = "{0}.{1}".Args(GetType().FullName, from),
                Text = text,
                 Exception = err
           }

         );
       }


        private void threadBody()
        {
          try
          {
            threadBodyCore();
          }
          catch(Exception error)
          {
            log(MessageType.CatastrophicError, "threadBody()", "Thread crashed as it leaked exception: "+error.ToMessageWithType(), error);
          }
        }

        private void threadBodyCore()
        {
           
           using(var targetStream = new MemoryStream( StreamCapacity ))//pre-allocate large space so it is not likely to resize and skew test results
           {
               foreach(var serializer in m_Serializers.OrderedValues)
               {
                  if (!Running) break;
                  log(MessageType.Info, serializer.Name, "Starting Serializer Tests");
              
                  foreach(var test in m_Tests.OrderedValues)
                  {
                      if (!Running) break;
                      
                      log(MessageType.Info, test.Name, "Starting Test with {0} runs".Args(test.Runs));

                      test.ResetAbort();
                      serializer.BeforeRuns(test);
                      if (test.Aborted)
                      {
                        continue;//todo return result into aborted tests dataset
                      }
                      test.BeforeRuns(serializer);
                      if (test.Aborted)
                      {
                        continue;//todo return result into aborted tests dataset
                      }

                      for(var i=0; Running && i<test.Runs; i++)
                      {
                          if (test.DoGc) GC.Collect(2);
                          test.ResetAbort();
                          var data = doTestRun(i, serializer, test, targetStream);
                          DataStore.SaveTestData( data );
                      }//runs
                  }//tests
               }//sers
           }

           if (!Running)
            log(MessageType.Warning, "threadBodyCore()", "Service stopping but test has not finished yet");

           log(MessageType.Info, "threadBodyCore()", "Thread exiting");
           
           SignalStop();
        }



        private TestRunData doTestRun(int runNumber, Serializer serializer, Test test, MemoryStream targetStream)
        {
           var nonClosingStreamWrap = new NFX.IO.NonClosingStreamWrap( targetStream );
           
           var result = new TestRunData
           {
              TestName = test.Name,
              TestType = test.GetType().FullName,
              SerializerName = serializer.Name,
              SerializerType = serializer.GetType().FullName,
              RunNumber = runNumber,
              DoGc = test.DoGc,
           };

           try
           {
             invokeTests(result, serializer, test, targetStream);
           }
           catch(Exception error)
           {
             result.RunException = error.ToMessageWithType();
             log(MessageType.Error, "'{0}'-'{1}'".Args(serializer.Name, test.Name), error.ToMessageWithType(), error);
           }

           return result;
        }


        private void invokeTests(TestRunData result, Serializer serializer, Test test, MemoryStream targetStream)
        {
           const int ERROR_CUTOFF = 3;

           const int ABORT_CUTOFF = 16;

           var streamWrap = new NFX.IO.NonClosingStreamWrap( targetStream );


           var serExceptions = 0;
           var wasOk = false;
           test.BeforeSerializationIterationBatch( serializer );
           serializer.BeforeSerializationIterationBatch( test );
         
           var sw = Stopwatch.StartNew();

           for(var i=0; i<test.SerIterations; i++)
           {
             targetStream.Position = 0;

             try
             {
               test.PerformSerializationTest( serializer, streamWrap );
               if (test.Aborted)
               {
                 result.SerAborts++;
                 result.FirstSerAbortMsg = test.AbortMessage;
                 test.ResetAbort();
                 if (result.SerAborts==ABORT_CUTOFF)
                 {
                  i = test.SerIterations;
                  throw new SerbenchException("Too many aborts {0}. Iterations run interrupted".Args(result.SerAborts));
                 }

                 continue;
               }
               wasOk = true;
             }
             catch(Exception error)
             {
               serExceptions++;
               log(MessageType.Error, "Serilizing '{0}'-'{1}'".Args(serializer.Name, test.Name), error.ToMessageWithType(), error);
               if (!wasOk && serExceptions==ERROR_CUTOFF)
               {
                 result.SerExceptions = serExceptions;
                 result.SerSupported = false;
                 log(MessageType.Error, "Serilizing '{0}'-'{1}'".Args(serializer.Name, test.Name), "Ser test aborted in ser phase. Too many consequitive exceptions");
                 return;
               }
             }
           }

           sw.Stop();
           result.SerSupported = wasOk;
           result.SerExceptions = serExceptions;
           result.PayloadSize = (int)targetStream.Position;
           result.SerIterations = test.SerIterations;
           result.SerDurationMs = sw.ElapsedMilliseconds;
           if ((result.SerDurationTicks = sw.ElapsedTicks) > 0)
             result.SerOpsSec = (int)( test.SerIterations / ((double)result.SerDurationTicks / (double)TimeSpan.TicksPerSecond) );

           if (!result.SerSupported)
            throw new SerbenchException("Test run failed as serialization not supported");

           if (result.SerIterations==0)
            throw new SerbenchException("Test run failed as nothing was serialized. Test must be configured with at least 1 serialization iteration to succeed");

           if (result.PayloadSize==0)
            throw new SerbenchException("Test run failed as no payload generated by serialization");


           var readingStreamSegment = new NFX.IO.BufferSegmentReadingStream();
           
           var deserExceptions = 0;
           wasOk = false;
           test.BeforeDeserializationIterationBatch( serializer );
           serializer.BeforeDeserializationIterationBatch( test );
           
           sw.Restart();

           for(var i=0; i<test.DeserIterations; i++)
           {
             targetStream.Position = 0;
             readingStreamSegment.BindBuffer(targetStream.GetBuffer(), 0, result.PayloadSize);
             try
             {
               test.PerformDeserializationTest( serializer, readingStreamSegment );
               if (test.Aborted)
               {
                 result.DeserAborts++;
                 result.FirstDeserAbortMsg = test.AbortMessage;
                 test.ResetAbort();
                 if (result.DeserAborts==ABORT_CUTOFF)
                 {
                  i = test.DeserIterations;
                  throw new SerbenchException("Too many aborts {0}. Iterations run interrupted".Args(result.DeserAborts));
                 }
                 continue;
               }
               wasOk = true;
             }
             catch(Exception error)
             {
               deserExceptions++;
               log(MessageType.Error, "Deserilizing '{0}'-'{1}'".Args(serializer.Name, test.Name), error.ToMessageWithType(), error);
               if (!wasOk && deserExceptions==ERROR_CUTOFF)
               {
                 result.DeserExceptions = deserExceptions;
                 result.DeserSupported = false;
                 log(MessageType.Error, "Deserilizing '{0}'-'{1}'".Args(serializer.Name, test.Name), "Test aborted in deser phase. Too many consequitive exceptions");
                 return;
               }
             }
           }
                
           sw.Stop();
           result.DeserSupported = wasOk;
           result.DeserIterations = test.DeserIterations;
           result.DeserDurationMs = sw.ElapsedMilliseconds;
           if ((result.DeserDurationTicks = sw.ElapsedTicks) > 0)
             result.DeserOpsSec = (int)( test.DeserIterations / ((double)result.DeserDurationTicks / (double)TimeSpan.TicksPerSecond) );
        }


      #endregion


    }
}
