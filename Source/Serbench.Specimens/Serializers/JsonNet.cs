using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Newtonsoft.Json;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Newtonsoft's Json.Net xerializer:
    /// See here http://www.newtonsoft.com/json
    /// >PM Install-Package Newtonsoft.Json 
    /// </summary>
    public class JsonNet : Serializer
    {
        private readonly JsonSerializer m_Serializer  = new JsonSerializer();
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public JsonNet(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            var m_primaryType = test.GetPayloadRootType();
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            using (var jw = new JsonTextWriter(sw))
            {
                m_Serializer.Serialize(jw, root);
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                return m_Serializer.Deserialize(jr, m_primaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            using (var jw = new JsonTextWriter(sw))
            {
                m_Serializer.Serialize(jw, root);
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                return m_Serializer.Deserialize(jr, m_primaryType);
            }
        }
    }
}