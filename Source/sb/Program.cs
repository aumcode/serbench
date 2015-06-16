using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.Log;
using NFX.Log.Destinations;
using NFX.Environment;
using NFX.ApplicationModel;

namespace sb
{
  class Program
  {
    static void Main(string[] args)
    {
       const string CONFIG_TESTING_SYSTEM_SECTION = "testing-system";
       try
       {
           ConfigSectionNode appConfig = null;
           if (args.Length>0)
           {
             var cname = args[0];
             Console.WriteLine("Trying to load config file: '{0}'...".Args(cname));
             appConfig = Configuration.ProviderLoadFromFile(cname).Root;
             Console.WriteLine("... loaded.");
           }
           else
             Console.WriteLine("Using the default config file");
           
           using(new ServiceBaseApplication(args, appConfig))
             try
             {
                 App.ConfigRoot.ProcessIncludePragmas(true);

                 using(var testing = FactoryUtils.MakeAndConfigure<Serbench.TestingSystem>(
                                          App.ConfigRoot[CONFIG_TESTING_SYSTEM_SECTION],
                                          typeof(Serbench.TestingSystem) )
                      )
                 {
                    testing.Start();
                    Console.WriteLine("Press <ENTER> to abort test execution");
                    while(testing.Running)
                    {
                      if (Console.KeyAvailable)
                       if (Console.ReadKey().Key==ConsoleKey.Enter) break;

                      System.Threading.Thread.Sleep(250);
                    }
                 }
             }
             catch(Exception loggableError)//the exception under the APP container scope can be logged
             {
               App.Log.Write( new Message
               {
                 Type = MessageType.CatastrophicError,
                 Topic = "App",
                 From = "Program",
                 Text = loggableError.ToMessageWithType(),
                 Exception = loggableError 
               });

               throw;
             }
        }
        catch(Exception error)
        {
          Console.WriteLine();
          Console.WriteLine("Exception in main()");
          Console.WriteLine("-------------------");
          Console.WriteLine(error.ToMessageWithType());
          System.Environment.ExitCode = -1;
        }

    }
  }
}
