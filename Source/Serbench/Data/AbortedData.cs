using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess.CRUD;

namespace Serbench.Data
{

  /// <summary>
  /// Denotes types of abort sources - from where abort was thrown
  /// </summary>
  public enum AbortedFrom{ ExceptionLeak, ConfigOther, ConfigSer, ConfigTest, BeforeRunsTest, BeforeRunsSer, Serialization, Deserialization  }


  /// <summary>
  /// Represents data about aborts, gathered during test runs.
  /// This data is saved into App.DataStore
  /// </summary>
  public class AbortedData : TypedRow
  {
    public AbortedData() {}

    public AbortedData(Serializer serializer, Test test, AbortedFrom from, string msg)
    {
       SerializerType = serializer.GetType().FullName;
       SerializerName = serializer.Name;
       TestType = test.GetType().FullName;
       TestName = test.Name;

       From = from;
       Message = msg;
    }


    [Field]
    public string TestType {get; set;}

    [Field]
    public string TestName {get; set;}

    [Field]
    public string SerializerType {get; set;}

    [Field]
    public string SerializerName {get; set;}

    [Field]
    public AbortedFrom From{get; set;}

    [Field]
    public string Message{get; set;}
  }

}
