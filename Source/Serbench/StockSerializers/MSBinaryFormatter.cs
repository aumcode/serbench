using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFX.Environment;

namespace Serbench.StockSerializers
{
  /// <summary>
  /// Represents Microsoft's BinaryFormatter technology
  /// </summary>
  public class MSBinaryFormatter : Serializer
  {
    public MSBinaryFormatter(TestingSystem context, IConfigSectionNode conf) : base(context, conf) {}

    public override void Serialize(object root, Stream stream)
    {
      throw new NotImplementedException();
    }

    public override object Deserialize(Stream stream)
    {
      throw new NotImplementedException();
    }
  }
}
