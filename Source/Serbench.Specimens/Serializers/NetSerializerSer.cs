using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NFX;
using NFX.Environment;

using NetSerializer;


namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents NetSerializer:
    /// See here: https://github.com/tomba/netserializer  
    /// Add: PM> Install-Package NetSerializer
    /// </summary>
    public class NetSerializerSer : Serializer
    {
        public NetSerializerSer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        private NetSerializer.Serializer m_Serializer;
        private Type[] m_KnownTypes;


        public override void BeforeRuns(Test test)
        {
            try
            {
                m_Serializer = m_KnownTypes==null ? new NetSerializer.Serializer( new Type[]{test.GetPayloadRootType() }) 
                                                  : new NetSerializer.Serializer( new Type[]{test.GetPayloadRootType() }.Concat(m_KnownTypes));
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making NetSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
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