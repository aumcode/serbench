using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NFX;
using NFX.Environment;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Newtonsoft's JsonSerializer:
    /// See here http://www.newtonsoft.com/json
    /// >PM Install-Package NetJSON 
    /// </summary>
    public class JsonNet : Serializer
    {
        private readonly JsonSerializer m_Serializer;

        public JsonNet(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
             m_Serializer = new JsonSerializer();
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
                return m_Serializer.Deserialize(jr);
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
                return m_Serializer.Deserialize(jr);
            }
        }
    }
}