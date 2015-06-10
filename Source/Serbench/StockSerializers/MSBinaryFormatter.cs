using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


using NFX.Environment;

namespace Serbench.StockSerializers
{
  /// <summary>
  /// Represents Microsoft's BinaryFormatter technology
  /// </summary>
  public class MSBinaryFormatter : Serializer
  {
    public MSBinaryFormatter(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
    {
      m_Formatter = new BinaryFormatter();

    }

    private BinaryFormatter m_Formatter;


    public override void Serialize(object root, Stream stream)
    {
      m_Formatter.Serialize(stream, root);
    }

    public override object Deserialize(Stream stream)
    {
      return m_Formatter.Deserialize(stream);
    }

    public override void ParallelSerialize(object root, Stream stream)
    {
      m_Formatter.Serialize(stream, root);
    }

    public override object ParallelDeserialize(Stream stream)
    {
      return m_Formatter.Deserialize(stream);
    }
  }
}
