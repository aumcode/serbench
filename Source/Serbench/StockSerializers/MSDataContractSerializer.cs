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
    /// Add: a line: using System.Runtime.Serialization
    /// </summary>
  [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Microsoft",
     VendorLicense = "Windows",
     VendorURL = "https://msdn.microsoft.com/en-us/library/system.runtime.serialization.json.datacontractjsonserializer(v=vs.110).aspx",
     VendorPackageAddress = "System.Runtime.Serialization.dll",
     FormatName = "DataContractSerializer",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 0
  )]
    public class MSDataContractSerializer : Serializer
    {
        private Type[] m_KnownTypes;
        private DataContractSerializer m_Serializer;

        public MSDataContractSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            var primaryType = test.GetPayloadRootType();

            try
            {
                m_Serializer = m_KnownTypes.Any() ?
                                new DataContractSerializer(primaryType, m_KnownTypes) :
                                new DataContractSerializer(primaryType);
            }
            catch (Exception error)
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