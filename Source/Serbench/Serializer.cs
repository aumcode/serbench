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
  /// Provides abstract base for all serializers that get tested in a suite
  /// </summary>
  public abstract class Serializer : TestArtifact
  {
    protected Serializer(TestingSystem context, IConfigSectionNode conf) : base(context, conf) {}

    /// <summary>
    /// Invoked by single-threaded tests to serialize payload
    /// </summary>
    public abstract void Serialize(object root, Stream stream);


    /// <summary>
    /// Invoked by single-threaded tests to deserialize payload into an object
    /// </summary>
    public abstract object Deserialize(Stream stream);


    /// <summary>
    /// Invoked by parallel tests, the implementation must be thread-safe
    /// </summary>
    public abstract void ParallelSerialize(object root, Stream stream);

    /// <summary>
    /// Invoked by parallel tests, the implementation must be thread-safe
    /// </summary>
    public abstract object ParallelDeserialize(Stream stream);

    /// <summary>
    /// Override to perform logic right befroe the iterations batch starts
    /// </summary>
    public virtual void BeforeSerializationIterationBatch(Test test)
    {

    }

     /// <summary>
    /// Override to perform logic right befroe the iterations batch starts
    /// </summary>
    public virtual void BeforeDeserializationIterationBatch(Test test)
    {

    }

  }
}
