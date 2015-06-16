using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;

namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents Microsoft's DataContract:
    /// Add: a reference: System.Runtime.Serialization.dll  
    /// Add: a line: using System.Runtime.Serialization.dll 
    /// </summary>
    public class MSDataContractSerializer : Serializer
    {
        public const string CONFIG_KNOWN_TYPE_SECTION = "known-type";
        private readonly DataContractSerializer m_Serializer;

        public MSDataContractSerializer(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
        {
            Type[] known;

            try
            {
                known = conf.Children.Where(cn => cn.IsSameName(CONFIG_KNOWN_TYPE_SECTION))
                    .Select(cn => Type.GetType(cn.AttrByName(Configuration.CONFIG_NAME_ATTR).Value, true))
                    .ToArray(); //force execution now
            }
            catch (Exception error)
            {
                throw new SerbenchException(
                    "Slim serializer config error in '{0}' section: {1}".Args(conf.ToLaconicString(),
                        error.ToMessageWithType()), error);
            }

            Type[] knownSubtypes = new Type[known.Length - 1];
            if (known.Length > 1) Array.ConstrainedCopy(known, 1, knownSubtypes, 0, known.Length - 1);
            m_Serializer = new DataContractSerializer(known[0], knownSubtypes);
        }

        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.WriteObject(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.ReadObject(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.WriteObject(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.ReadObject(stream);
        }
    }
}