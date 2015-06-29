using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using HaveBoxJSON;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents HaveBoxJSON:
    /// See here https://www.nuget.org/packages/HaveBoxJSON/
    /// >PM Install-Package HaveBoxJSON 
    /// </summary>
     [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Mehdi Gholam",
     VendorLicense = "The Code Project Open License (CPOL)",
     VendorURL = "https://www.nuget.org/packages/HaveBoxJSON/",
     VendorPackageAddress = "Install-Package HaveBoxJSON",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 40
  )]
    public class HaveBoxJSON : Serializer
    {
        private readonly JsonConverter m_Serializer = new JsonConverter();
        private Type m_primaryType;

        public HaveBoxJSON(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            // m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();           
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(m_primaryType, sr.ReadToEnd());
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(m_primaryType, sr.ReadToEnd());
            }
        }
      public override bool AssertPayloadEquality(Test test, object original, object deserialized, bool abort = true)
        {
            string serError = null;
            if (test.Name.Contains("Telemetry"))
            { 
                if (!Serbench.Specimens.Tests.TelemetryData.AssertPayloadEquality(original, deserialized, out serError))
                {
                    if (abort) test.Abort(this, serError);
                    return false;
                }
            }
           else if (test.Name.Contains("EDI_X12_835"))
            { 
                if (!Serbench.Specimens.Tests.EDI_X12_835Data.AssertPayloadEquality(original, deserialized, out serError))
                {
                    if (abort) test.Abort(this, serError);
                    return false;
                }
           }
            return base.AssertPayloadEquality(test, original, deserialized, abort);
        }
   }
}