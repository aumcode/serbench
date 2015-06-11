using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess.CRUD;

namespace Serbench
{

  /// <summary>
  /// Represents data gathered during test runs.
  /// This data is saved into App.DataStore
  /// </summary>
  public class TestRunData : TypedRow
  {
    [Field]
    public string TestType {get; set;}

    [Field]
    public string TestName {get; set;}

    [Field]
    public string SerializerType {get; set;}

    [Field]
    public string SerializerName {get; set;}

    [Field]
    public bool DoGc{get; set;}


    
    [Field]
    public bool SerSupported {get; set;}

    [Field]
    public int SerIterations {get; set;}

    [Field]
    public int SerExceptions {get; set;}

    [Field]
    public int SerAborts {get; set;}

    [Field]
    public int SerDurationMs {get; set;}

    [Field]
    public int SerDurationTicks {get; set;}

    [Field]
    public int SerOpsSec {get; set;}



    [Field]
    public int PayloadSize {get; set;}



    [Field]
    public bool DeserSupported {get; set;}

    [Field]
    public int DeserIterations {get; set;}

    [Field]
    public int DeserExceptions {get; set;}

    [Field]
    public int DeserAborts {get; set;}

    [Field]
    public int DeserDurationMs {get; set;}

    [Field]
    public int DeserDurationTicks {get; set;}

    [Field]
    public int DeserOpsSec {get; set;}

  }



}
