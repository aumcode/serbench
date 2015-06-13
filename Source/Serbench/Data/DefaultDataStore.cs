using System;
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

  public enum OutputFormat {JSON, CSV}

  /// <summary>
  /// Represents a data store that saves data as JSON records on disk
  /// </summary>
  public class DefaultDataStore : Service, IDataStoreImplementation, ITestDataStore
  {
    
   
      private string m_RootPath;
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


      [Config]
      public OutputFormat Output{get; set;}


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

    #endregion

    #region Protected

      protected override void DoStart()
      {
        if (m_RootPath.IsNullOrWhiteSpace() ||  !Directory.Exists(m_RootPath))
           throw new SerbenchException("Data store directory not found");

        m_Data = new Dictionary<string,List<Row>>(StringComparer.OrdinalIgnoreCase);
      }


      protected override void DoWaitForCompleteStop()
      {
        foreach(var kvp in m_Data.Where(kvp => kvp.Value.Count>0))
          if (Output== OutputFormat.JSON)
          {
             using(var fs = new FileStream(Path.Combine(m_RootPath, kvp.Key+".json"), FileMode.Create, FileAccess.Write, FileShare.None, 256*1024))
               JSONWriter.Write(kvp.Value, fs, JSONWritingOptions.PrettyPrintRowsAsMap);
          }
          else//CSV
          {
             using(var fs = new FileStream(Path.Combine(m_RootPath, kvp.Key+".csv"), FileMode.Create, FileAccess.Write, FileShare.None, 256*1024))
               using(var sw = new StreamWriter(fs, Encoding.UTF8))
               {
                 var firstRow = kvp.Value[0];
                 sw.WriteLine( string.Join(",", firstRow.Schema.Select(fd => fd.Name))); 

                 foreach(var row in kvp.Value.Where( lst => lst!=null))
                   sw.WriteLine( string.Join(",", row.Select(v => (v==null) ? string.Empty : "\"{0}\"".Args(v.ToString().Replace("\"",@""""))  ))); 
               }
          }
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
  }


}
