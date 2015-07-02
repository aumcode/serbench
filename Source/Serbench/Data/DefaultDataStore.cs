﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using NFX;
using NFX.Environment;
using NFX.ApplicationModel;
using NFX.ServiceModel;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;

namespace Serbench.Data
{

  /// <summary>
  /// Represents a data store that saves data as JSON records on disk
  /// </summary>
  public class DefaultDataStore : Service, IDataStoreImplementation, ITestDataStore
  {
    
   
      [Config]
      private string m_RootPath;

      private string m_RunSessionPath;

      private Dictionary<string, List<Row>> m_Data;
   
    #region Properties
     
      public string TargetName
      {
        get { return this.GetType().Namespace; }
      }

      [Config]
      public string RootPath
      {
        get{ return m_RootPath;}
        set
        {
          CheckServiceInactive();
          m_RootPath = value;
        }
      }


      [Config(Default=true)] public bool OutputWeb{get; set;}
      [Config] public bool OutputJSON{get; set;}
      [Config] public bool OutputCSV{get; set;}


    #endregion

    #region Pub
      public void TestConnection()
      {
        throw new NotImplementedException();
      }
   

      public void SaveTestData(Row data)
      {
        if (!Running) return;

        var key = data.GetType().Name;
        List<Row> lst;
        if (!m_Data.TryGetValue(key, out lst))
        {
          lst = new List<Row>();
          m_Data[key] = lst;
        }
        lst.Add( data );
      }

      public void SaveTestPayloadDump(Serializer serializer, Test test, Stream dumpData)
      {
        var dir = DoCreatePayloadDumpFolder(serializer, test);

        var fname = Path.Combine(dir, SanitizeName( serializer.Name ) + "." + serializer.GetType().Name);

        using(var fs = new FileStream(fname, FileMode.Create))
          dumpData.CopyTo(fs, 512 * 1024);
      }


    #endregion

    #region IDataStoreImplementation 
      public StoreLogLevel LogLevel { get { return StoreLogLevel.None;} set {}}

      public bool InstrumentationEnabled { get{ return false;} set{}}

      public bool ExternalGetParameter(string name, out object value, params string[] groups)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<KeyValuePair<string, Type>> ExternalParameters
      {
        get { throw new NotImplementedException(); }
      }

      public IEnumerable<KeyValuePair<string, Type>> ExternalParametersForGroups(params string[] groups)
      {
        throw new NotImplementedException();
      }

      public bool ExternalSetParameter(string name, object value, params string[] groups)
      {
        throw new NotImplementedException();
      }
    #endregion

    #region Protected

      protected override void DoStart()
      {
        if (m_RootPath.IsNullOrWhiteSpace() ||  !Directory.Exists(m_RootPath))
           throw new SerbenchException("Data store directory [{0}] not found".Args(m_RootPath));

        if (!OutputWeb && !OutputJSON && !OutputCSV)
           throw new SerbenchException("None of 'Output-*' flags are set. Data store is not going to save anything. Set at least one of 'Output-[Web|JSON|CSV]' to true ");

        m_Data = new Dictionary<string,List<Row>>(StringComparer.OrdinalIgnoreCase);

        m_RunSessionPath = DoCreateRunSessionFolder();
      }


      protected override void DoWaitForCompleteStop()
      {

        foreach(var kvp in m_Data.Where(kvp => kvp.Value.Count>0))
        {
          if (OutputWeb)  writeWeb(kvp);
          if (OutputJSON) writeJSON(kvp);
          if (OutputCSV)  writeCSV(kvp);
        }
      }


     protected string SanitizeName(string name)
     {
       return new String( name.Select( c => !Char.IsLetterOrDigit(c) ? '_' : c).ToArray() );
     }

     protected virtual string DoCreateRunSessionFolder()
     {
        var appName = App.Name;
        if (appName.IsNullOrWhiteSpace()) appName = "Serbench";

        //sanitize app name so it can be used for directory name
        appName = SanitizeName( appName );
        
        var name = Path.Combine(m_RootPath, "{0}-{1:yyyyMMddHHmm}".Args(appName, App.LocalizedTime));
        
        var dname = name;
        for(var i=0; Directory.Exists(dname); i++) dname = name + i.ToString();

        Directory.CreateDirectory(dname);

        return dname;
     }

     protected virtual string DoCreatePayloadDumpFolder(Serializer serializer, Test test)
     {
        var name = test.Name;
        

        //sanitize app name so it can be used for directory name
        name = SanitizeName( name ); 
        
        name = Path.Combine( m_RunSessionPath , name );
        
        var dname = name;

        if (!Directory.Exists(dname))
          Directory.CreateDirectory(dname);

        return dname;
     }

    #endregion


    #region .pvt

      private void writeWeb(KeyValuePair<string, List<Row>> table)
      {
        var packager = new WebViewer.DefaultWebPackager(this, null);
        var targetDir = packager.Build(m_RunSessionPath);
        using(var fs = new FileStream(Path.Combine(targetDir, "scripts", "data-{0}.js".Args(table.Key)), FileMode.Create, FileAccess.Write, FileShare.None, 256*1024))
         using(var wri = new StreamWriter(fs, Encoding.UTF8))
         {
          wri.WriteLine("//Java script file for Serbench Web output. Table '{0}'".Args(table.Key));
          wri.WriteLine("var data_{0} = ".Args(table.Key));
            JSONWriter.Write(table.Value, wri, JSONWritingOptions.PrettyPrintRowsAsMap);
          wri.WriteLine(";");
         }
      }


      private void writeJSON(KeyValuePair<string, List<Row>> table)
      {
        using(var fs = new FileStream(Path.Combine(m_RunSessionPath, table.Key+".json"), FileMode.Create, FileAccess.Write, FileShare.None, 256*1024))
          JSONWriter.Write(table.Value, fs, JSONWritingOptions.PrettyPrintRowsAsMap);
      }


      private void writeCSV(KeyValuePair<string, List<Row>> table)
      {
         using(var fs = new FileStream(Path.Combine(m_RunSessionPath, table.Key+".csv"), FileMode.Create, FileAccess.Write, FileShare.None, 256*1024))
          using(var sw = new StreamWriter(fs, Encoding.UTF8))
          {
            var firstRow = table.Value[0];
            sw.WriteLine( string.Join(",", firstRow.Schema.Select(fd => fd.Name))); 

            foreach(var row in table.Value.Where( lst => lst!=null))
              sw.WriteLine( string.Join(",", row.Select(v => (v==null) ? string.Empty : "\"{0}\"".Args(v.ToString().Replace("\"",@""""))  ))); 
          }
      }
      

    #endregion



    
  }


}
