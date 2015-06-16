using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;

namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents Microsoft's DataContract:
    /// Add: a reference: System.Runtime.Serialization.dll  
    /// Add: a line: using System.Runtime.Serialization.dll 
    /// </summary>
    public class MSDataContractSerializer : Serializer
    {
        

        public MSDataContractSerializer(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        private Type[] m_KnownTypes;
        private DataContractSerializer m_Serializer;



        public override void BeforeRuns(Test test)
        {
            var primaryType = test.GetPayloadRootType();

            try
            {
              m_Serializer = m_KnownTypes.Any() ? 
                              new DataContractSerializer(primaryType, m_KnownTypes) :
                              new DataContractSerializer(primaryType);
            }
            catch(Exception error)
            {
              test.Abort(this, "Error making DataContractSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }


        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.WriteObject(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.ReadObject(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.WriteObject(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.ReadObject(stream);
        }
    }
}