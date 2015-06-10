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

    public abstract void Serialize(object root, Stream stream);

    public abstract object Deserialize(Stream stream);


  }
}
