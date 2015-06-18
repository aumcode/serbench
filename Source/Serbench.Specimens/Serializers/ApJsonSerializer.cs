using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Apolyton.FastJson;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Apolyton.FastJson.Json:
    /// See here http://www.codeproject.com/Articles/491742/APJSON
    /// Manually download a dll from mentioned site and add a reference to it. 
    /// </summary>
    public class ApJsonSerializer : Serializer
    {
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public ApJsonSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            try
            {
                m_primaryType = test.GetPayloadRootType();
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making ApJsonSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }

        public override void Serialize(object root, Stream stream)
        {
            var buf = Json.Current.ToJsonBytes(root);
            stream.Write(buf, 0, buf.Length);
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return Json.Current.ReadObject(sr.ReadToEnd(), m_primaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            var buf = Json.Current.ToJsonBytes(root);
            stream.Write(buf, 0, buf.Length);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return Json.Current.ReadObject(sr.ReadToEnd(), m_primaryType);
            }
        }
    }
}