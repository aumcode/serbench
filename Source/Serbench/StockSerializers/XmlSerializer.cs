using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;

namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents Microsoft's XmlSerializer:
    /// Add: a reference: System.Xml.Serialization.dll  
    /// Add: a line: using System.Xml.Serialization.dll 
    /// </summary>
    public class XmlSerializer : Serializer
    {
        private readonly System.Xml.Serialization.XmlSerializer m_Serializer;

        public XmlSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            Type[] known = ReadKnownTypes(conf);


            Type[] knownSubtypes = new Type[known.Length - 1];
            if (known.Length > 1) Array.ConstrainedCopy(known, 1, knownSubtypes, 0, known.Length - 1);
            m_Serializer = new System.Xml.Serialization.XmlSerializer(known[0], knownSubtypes);
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                m_Serializer.Serialize(sw, root);
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(sr);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                m_Serializer.Serialize(sw, root);
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(sr);
            }
        }
    }
}