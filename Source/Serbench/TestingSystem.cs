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

        public const string CONFIG_TEST_SECTION = "test";
        public const string CONFIG_SER_SECTION = "serializer";

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

          foreach(var tnode in node.Children.Where(cn => cn.IsSameName(CONFIG_TEST_SECTION)))
          {
            var item = FactoryUtils.Make<Test>(tnode, args: new object[]{this, tnode});
            m_Tests.Register( item );
            log(MessageType.Info, "conf tests", "Added test {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }

          foreach(var snode in node.Children.Where(cn => cn.IsSameName(CONFIG_SER_SECTION)))
          {
            var item = FactoryUtils.Make<Serializer>(snode, args: new object[]{this, snode});
            m_Serializers.Register( item  );
            log(MessageType.Info, "conf sers", "Added serializer {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }
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

                      for(var i=0; Running && i<test.Runs; i++)
                      {
                          if (test.DoGc) GC.Collect(2);
                          var data = doTestRun(i, serializer, test, targetStream);
                          DataStore.SaveTestData( data );
                      }//runs
                  }//tests
               }//sers
           }

           if (!Running)
            log(MessageType.Warning, "threadBody()", "Service stopping but test has not finished yet");

           log(MessageType.Info, "threadBody()", "Thread exiting");
           
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
           var streamWrap = new NFX.IO.NonClosingStreamWrap( targetStream );

           var sw = Stopwatch.StartNew();

           for(var i=0; i<test.SerIterations; i++)
           {
             targetStream.Position = 0;
             test.PerformSerializationTest( serializer, streamWrap );
           }

           sw.Stop();
           result.SerIterations = test.SerIterations;
           result.SerDurationMs = sw.ElapsedMilliseconds;
           if ((result.SerDurationTicks = sw.ElapsedTicks) > 0)
             result.SerOpsSec = (int)( test.SerIterations / ((double)result.SerDurationTicks / (double)TimeSpan.TicksPerSecond) );

           var readingStreamSegment = new NFX.IO.BufferSegmentReadingStream();
           readingStreamSegment.BindBuffer(targetStream.GetBuffer(), 0, targetStream.Position);
           sw.Restart();
           for(var i=0; i<test.DeserIterations; i++)
           {
             targetStream.Position = 0;
             test.PerformDeserializationTest( serializer, readingStreamSegment );
           }

           sw.Stop();
           result.DeserIterations = test.DeserIterations;
           result.DeserDurationMs = sw.ElapsedMilliseconds;
           if ((result.DeserDurationTicks = sw.ElapsedTicks) > 0)
             result.SerOpsSec = (int)( test.DeserIterations / ((double)result.DeserDurationTicks / (double)TimeSpan.TicksPerSecond) );
        }


      #endregion


    }
}
