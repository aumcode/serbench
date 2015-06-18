using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;  
using NFX.Environment;

using MsgPack.Serialization;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    /// Represents  MsgPack technology
    /// See here https://github.com/msgpack/msgpack-cli
    /// >PM Install-Package MsgPack.Cli
    /// </summary>
    public class MsgPackSerializer : Serializer
    {
        private IMessagePackSerializer m_serializer = null;
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public MsgPackSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);     
        }


        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();
            m_serializer = MessagePackSerializer.Get(m_primaryType);
        }


        public override void Serialize(object root, Stream stream)
        {
            m_serializer.Pack(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_serializer.Unpack(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_serializer.Pack(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_serializer.Unpack(stream);
        }
    }
}
