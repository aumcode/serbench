using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess.CRUD;

namespace Serbench.Data
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
    public int RunNumber{get; set;}

    /// <summary>
    /// Populated when test run crashed with exception
    /// </summary>
    [Field]
    public string RunException{get; set;}


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
    public string FirstSerAbortMsg {get; set;}

    [Field]
    public long SerDurationMs {get; set;}

    [Field]
    public long SerDurationTicks {get; set;}

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
    public string FirstDeserAbortMsg {get; set;}

    [Field]
    public long DeserDurationMs {get; set;}

    [Field]
    public long DeserDurationTicks {get; set;}

    [Field]
    public int DeserOpsSec {get; set;}

  }



}
