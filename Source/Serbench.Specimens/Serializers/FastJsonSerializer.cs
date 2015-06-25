using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;
using fastJSON;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents fastJSON:
    /// See here https://github.com/mgholam/fastJSON
    /// >PM Install-Package fastJSON 
    /// </summary>
     [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Mehdi Gholam",
     VendorLicense = "The Code Project Open License (CPOL)",
     VendorURL = "https://github.com/mgholam/fastJSON",
     VendorPackageAddress = "Install-Package fastJSON",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 40
  )]
   public class FastJsonSerializer : Serializer
    {
        private readonly FastJsonSerializer m_Serializer;
        private Type m_PrimaryType;

        public FastJsonSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

       public override void BeforeRuns(Test test)
        {
            m_PrimaryType = test.GetPayloadRootType();           
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(JSON.ToJSON(root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return fastJSON.JSON.ToObject(sr.ReadToEnd(), m_PrimaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(JSON.ToJSON(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return fastJSON.JSON.ToObject(sr.ReadToEnd(), m_PrimaryType);
            }
        }
    }
}