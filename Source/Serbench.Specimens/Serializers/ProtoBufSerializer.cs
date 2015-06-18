using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.Environment;

using ProtoBuf;
using ProtoBuf.Meta;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents NetSerializer:
    /// See here: https://github.com/mgravell/protobuf-net 
    /// Add: PM> Install-Package protobuf-net
    ///     NOTE: I use the protobuf-net NuGet package because of
    ///     [http://stackoverflow.com/questions/2522376/how-to-choose-between-protobuf-csharp-port-and-protobuf-net]
    /// </summary>

    public class ProtoBufSerializer : Serializer
    {   
        private RuntimeTypeModel m_model = RuntimeTypeModel.Create();
        private Type[] m_KnownTypes;
        private Type m_primaryType;


        public ProtoBufSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            //var primaryType = test.GetPayloadRootType();

            try
            {
                m_primaryType = test.GetPayloadRootType();
                foreach(var knownType in m_KnownTypes)
                    m_model.Add(knownType, true);
                m_model.CompileInPlace();
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making ProtoBuf instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }


        public override void Serialize(object root, Stream stream)
        {
            m_model.Serialize(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_model.Deserialize(stream, null, m_primaryType);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_model.Serialize(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_model.Deserialize(stream, null, m_primaryType);
        }
    }
}
