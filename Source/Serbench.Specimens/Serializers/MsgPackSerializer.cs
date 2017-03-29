using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;


using MsgPack.Serialization;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    /// Represents  MsgPack technology
    /// See here https://github.com/msgpack/msgpack-cli
    /// >PM Install-Package MsgPack.Cli
    /// </summary>
    [SerializerInfo(
     Family = SerializerFamily.Binary,
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Sadayuki Furuhashi",
     VendorLicense = "Apache 2.0",
     VendorURL = "https://github.com/msgpack/msgpack-cli",
     VendorPackageAddress = "Install-Package MsgPack.Cli",
     FormatName = "MsgPack",
     LinesOfCodeK = 0,
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 429
    )]
    public class MsgPackSerializer : Serializer
    {
        public MsgPackSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {

        }

        private MessagePackSerializer m_Serializer = null;

        public override void BeforeRuns(Test test)
        {
            m_Serializer = MessagePackSerializer.Get(test.GetPayloadRootType());
        }


        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.Pack(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.Unpack(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.Pack(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.Unpack(stream);
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
