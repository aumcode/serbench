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
           using(new ServiceBaseApplication(args, null))
             using(var testing = FactoryUtils.MakeAndConfigure<Serbench.TestingSystem>(
                                      App.ConfigRoot[CONFIG_TESTING_SYSTEM_SECTION],
                                      typeof(Serbench.TestingSystem) )
                  )
             {
                testing.Start();
                testing.WaitForCompleteStop();
             }
        }
        catch(Exception error)
        {
          Console.WriteLine("Exception leaked");
          Console.WriteLine("----------------");
          Console.WriteLine(error.ToMessageWithType());
          System.Environment.ExitCode = -1;
        }

    }
  }
}
