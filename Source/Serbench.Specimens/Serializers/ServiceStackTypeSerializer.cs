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
    [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Service Stack",
     VendorLicense = "All OSS Licenses + Commercial",
     VendorURL = "https://github.com/ServiceStack/ServiceStack.Text",
     VendorPackageAddress = "Install-Package ServiceStack.Text",
     FormatName = "JSV",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 307
    )]
    public class ServiceStackTypeSerializer : Serializer
    {
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public ServiceStackTypeSerializer(TestingSystem context, IConfigSectionNode conf)
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
                test.Abort(this, "Error making ServiceStackTypeSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }

        public override void Serialize(object root, Stream stream)
        {
            JsonSerializer.SerializeToStream(root, m_primaryType, stream);
        }

        public override object Deserialize(Stream stream)
        {
            return JsonSerializer.DeserializeFromStream(m_primaryType, stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            JsonSerializer.SerializeToStream(root, m_primaryType, stream);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return JsonSerializer.DeserializeFromStream(m_primaryType, stream);
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