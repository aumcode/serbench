using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using NetJSON;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents NetJSON:
    /// See here https://github.com/rpgmaker/NetJSON
    /// >PM Install-Package NetJSON 
    /// </summary>
    public class NetJSONSerializer : Serializer
    {
        public NetJSONSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {

        }

        public override void BeforeRuns(Test test)
        {
            m_RootType = test.GetPayloadRootType();
        }


        private Type m_RootType;

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(NetJSON.NetJSON.Serialize(m_RootType, root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return NetJSON.NetJSON.Deserialize(m_RootType, sr.ReadToEnd());
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
             using (var sw = new StreamWriter(stream))
            {
                sw.Write(NetJSON.NetJSON.Serialize(m_RootType, root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return NetJSON.NetJSON.Deserialize(m_RootType, sr.ReadToEnd());
            }
        }
    }
}