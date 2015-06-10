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
    protected Test(TestingSystem context, IConfigSectionNode conf) : base(context, conf) { }


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
