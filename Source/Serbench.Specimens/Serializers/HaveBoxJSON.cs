using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using HaveBoxJSON;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents HaveBoxJSON:
    /// See here https://www.nuget.org/packages/HaveBoxJSON/
    /// >PM Install-Package HaveBoxJSON 
    /// </summary>
    public class HaveBoxJSON : Serializer
    {
        private readonly JsonConverter m_Serializer = new JsonConverter();
        private Type m_primaryType;

        public HaveBoxJSON(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            // m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();           
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(m_primaryType, sr.ReadToEnd());
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(m_primaryType, sr.ReadToEnd());
            }
        }
    }
}