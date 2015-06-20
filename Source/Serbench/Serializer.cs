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
    public const string CONFIG_KNOWN_TYPE_SECTION = "known-type";


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
    /// Override to perform logic before initiation of runs of the specified test against this serializer
    /// </summary>
    public virtual void BeforeRuns(Test test)
    {

    }


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


    /// <summary>
    /// QUICKLY asserts the equality of what has deserialized with the original payload.
    /// Warning! This test is done as a part of time measurement, it must be very fast.
    /// DO NOT perform very detailed tests, just test that you have received logcally equal object back.
    /// No need for detailed comparison, as this is not a unit-testing framework
    /// </summary>
    /// <param name="test">Test that the assertion is made for</param>
    /// <param name="original">Original serialization data root</param>
    /// <param name="deserialized">Deserialized data root object</param>
    /// <param name="abort">If true then the method should abort the test befoe returning</param>
    /// <returns>
    /// True when both payloads are LOGICALLY equal, i.e. you can serialize TypedPerson but get Dictionary(string, object) of the same data
    /// which may be considered correct response for some serializers (that's why this method is in the serializer class)
    /// </returns>
    public virtual bool AssertPayloadEquality(Test test, object original, object deserialized, bool abort = true)
    {
      if (deserialized==null)
      {
        if (original==null) return true;
        if (abort) test.Abort(this, "Deserialized null from non-null original");
        return false;
      }

      if (original==null)
      {
        if (abort) test.Abort(this, "Original was null but deserialized into non-null");
        return false;
      }

      if (original is System.Collections.ICollection)
      {
        var orgCol = original as System.Collections.ICollection;
        var gotCol = deserialized as System.Collections.ICollection;

        if (gotCol==null || gotCol.Count!=orgCol.Count)
        {
          if (abort) test.Abort(this, "Original and deserized collections size or type mismatch");
          return false;
        }
      }

      return true; 
    }


    /// <summary>
    /// Reads config sections into Type[]
    /// </summary>
    protected virtual Type[] ReadKnownTypes(IConfigSectionNode conf)
    {
      try
      {
        return conf.Children.Where(cn => cn.IsSameName(CONFIG_KNOWN_TYPE_SECTION))
                            .Select( cn => Type.GetType( cn.AttrByName(Configuration.CONFIG_NAME_ATTR).Value, true ))
                            .ToArray();   //force execution now
      }
      catch(Exception error)
      {
        throw new SerbenchException("{0} serializer config error in '{1}' section: {2}".Args(GetType().FullName,
                                                                                             conf.ToLaconicString(),
                                                                                             error.ToMessageWithType()), error);
      }
    }

  }
}
