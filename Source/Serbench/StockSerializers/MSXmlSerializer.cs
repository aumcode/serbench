using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
    public class MSXmlSerializer : Serializer
    {
        private Type[] m_KnownTypes;
        private XmlSerializer m_Serializer;

        public MSXmlSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            var primaryType = test.GetPayloadRootType();

            try
            {
                m_Serializer = m_KnownTypes.Any() ?
                                new XmlSerializer(primaryType, m_KnownTypes) :
                                new XmlSerializer(primaryType);
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making XmlSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }


        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.Serialize(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.Serialize(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }
    }
}