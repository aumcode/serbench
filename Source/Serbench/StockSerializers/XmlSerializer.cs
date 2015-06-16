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

            var primaryType = known.FirstOrDefault();

            if (primaryType==null)
              throw new SerbenchException("{0} serializer config error. Must define at least 1 primary known-type".Args(GetType().FullName));

            var extraTypes = known.Skip(1).ToArray();
            
            try
            {
              m_Serializer = extraTypes.Any() ? 
                              new System.Xml.Serialization.XmlSerializer(primaryType, extraTypes) :
                              new System.Xml.Serialization.XmlSerializer(primaryType);
            }
            catch(Exception error)
            {
              throw new SerbenchException("Error making XmlSerializer instance in serializer .ctor: {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()),error);
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