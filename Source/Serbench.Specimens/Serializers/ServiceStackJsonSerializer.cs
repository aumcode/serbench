using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using ServiceStack.Text;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents ServiceStack:
    /// See here https://github.com/ServiceStack/ServiceStack.Text
    /// >PM Install-Package ServiceStack.Text 
    /// </summary>
    public class ServiceStackJsonSerializer : Serializer
    {
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public ServiceStackJsonSerializer(TestingSystem context, IConfigSectionNode conf)
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
                test.Abort(this, "Error making ServiceStackJsonSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }

        public override void Serialize(object root, Stream stream)
        {
            TypeSerializer.SerializeToStream(root, m_primaryType, stream);
        }

        public override object Deserialize(Stream stream)
        {
            return TypeSerializer.DeserializeFromStream(m_primaryType, stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            TypeSerializer.SerializeToStream(root, m_primaryType, stream);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return TypeSerializer.DeserializeFromStream(m_primaryType, stream);
        }
    }
}