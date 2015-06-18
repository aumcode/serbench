using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Jil;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Jil serializer:
    /// See here https://github.com/kevin-montrose/Jil
    /// >PM Install-Package  
    /// </summary>
    public class JilSerializer : Serializer
    {
        //private readonly JilSerializer m_Serializer;
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public JilSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                JSON.Serialize(root, sw);
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return JSON.Deserialize(sr.ReadToEnd(), m_primaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                JSON.Serialize(root, sw);
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return JSON.Deserialize(sr.ReadToEnd(), m_primaryType);
            }
        }
    }
}