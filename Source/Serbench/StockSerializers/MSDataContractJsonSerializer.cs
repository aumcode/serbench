using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using NFX;
using NFX.Environment;

namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents Microsoft's System.Runtime.Serialization.Json.DataContractJsonSerializer:
    /// Add: a reference: System.Runtime.Serialization.dll  
    /// Add: a line: using System.Runtime.Serialization.Json 
    /// </summary>
  [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Microsoft",
     VendorLicense = "Windows",
     VendorURL = "https://msdn.microsoft.com/en-us/library/system.runtime.serialization.json.datacontractjsonserializer(v=vs.110).aspx",
     VendorPackageAddress = "System.Runtime.Serialization.dll",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 0
  )]
    public class MSDataContractJsonSerializer : Serializer
    {
        private DataContractJsonSerializer m_Serializer;
        private Type[] m_KnownTypes;

        public MSDataContractJsonSerializer(TestingSystem context, IConfigSectionNode conf)
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
                                new DataContractJsonSerializer(primaryType, m_KnownTypes) :
                                new DataContractJsonSerializer(primaryType);
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making DataContractJsonSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
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