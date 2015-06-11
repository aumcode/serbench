using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using NFX;
using NFX.Environment;

namespace Serbench
{
  /// <summary>
  /// Provides abstract base for all tests that get executed against the serializers
  /// </summary>
  public abstract class Test : TestArtifact
  {
    protected Test(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    {

    }


    [Config(Default=1)]
    private int m_SerIterations;

    [Config(Default=1)]
    private int m_DeserIterations;

    [Config(Default=1)]
    private int m_Runs;

    [Config]
    private bool m_DoGc;

    /// <summary>
    /// Returns how many serialization iterations per run the test performs
    /// </summary>
    public int SerIterations{ get{ return m_SerIterations;}}

    /// <summary>
    /// Returns how many deserialization iterations per run the test performs
    /// </summary>
    public int DeserIterations{ get{ return m_DeserIterations;}}

    /// <summary>
    /// Returns how many runs the test executes
    /// </summary>
    public int Runs{ get{ return m_Runs;}}


    /// <summary>
    /// True to do a full GC before test run execution
    /// </summary>
    public bool DoGc{ get{ return m_DoGc;}}


    /// <summary>
    /// Override to perform the test logic
    /// </summary>
    public abstract void PerformSerializationTest(Serializer serializer, Stream target);

    /// <summary>
    /// Override to perform the test logic
    /// </summary>
    public abstract void PerformDeserializationTest(Serializer serializer, Stream target);

    /// <summary>
    /// Reports abort of the test due to error. This is MUCH faster than using exceptions
    /// </summary>
    public void Abort(string msg)
    {
      //todo add this to aborted/error count in context
    }

  }
}
