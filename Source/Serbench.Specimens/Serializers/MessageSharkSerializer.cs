using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using MessageShark;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents MessageShark by TJ Bakre:
    /// See here https://github.com/rpgmaker/MessageShark
    /// >PM Install-Package MessageShark 
    /// </summary>
    [SerializerInfo(
   Family = SerializerFamily.Binary,
   MetadataRequirement = MetadataRequirement.None,
   VendorName = "TJ Bakre",
   VendorLicense = "The GNU Library General Public License (LGPL)",
   VendorURL = "https://github.com/rpgmaker/MessageShark",
   VendorPackageAddress = "Install-Package MessageShark",
   FormatName = "MessageShark",
   LinesOfCodeK = 0,
   DataTypes = 0,
   Assemblies = 1,
   ExternalReferences = 0,
   PackageSizeKb = 49
)]
    public class MessageSharkSerializer : Serializer
    {

   //     private Type m_primaryType;

        public MessageSharkSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        public override void BeforeRuns(Test test)
        {
            m_RootType = test.GetPayloadRootType();
        }


        private Type m_RootType;

        public override void Serialize(object root, Stream stream)
        {
            MessageShark.MessageSharkSerializer.Serialize(root, stream);
        }

        public override object Deserialize(Stream stream)
        {
            return MessageShark.MessageSharkSerializer.Deserialize(m_RootType, stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            MessageShark.MessageSharkSerializer.Serialize(root, stream);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return MessageShark.MessageSharkSerializer.Deserialize(m_RootType, stream);
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