using System;
using System.IO;
using Bond;
using Bond.IO.Unsafe;
using Bond.Protocols;
using NFX.Environment;
using Serbench.Specimens.Tests;

namespace Serbench.Specimens.Serializers
{
    [SerializerInfo(
        Family = SerializerFamily.Binary,
        MetadataRequirement = MetadataRequirement.Attributes,
        VendorName = "Microsoft",
        VendorLicense = "The MIT License (MIT)",
        VendorURL = "https://github.com/Microsoft/bond/",
        VendorPackageAddress = "Install-Package Bond.CSharp",
        FormatName = "JSON",
        LinesOfCodeK = 0,
        DataTypes = 0,
        Assemblies = 4,
        ExternalReferences = 1,
        PackageSizeKb = 350
        )]
    public class BondSerializer : Serializer
    {
        //private Deserializer<CompactBinaryReader<InputBuffer>> _deserializer;
        //private Serializer<CompactBinaryWriter<OutputBuffer>> _serializer;
        private Type m_RootType;

        public BondSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        public override void BeforeRuns(Test test)
        {
            m_RootType = test.GetPayloadRootType();
            //_serializer = new Serializer<CompactBinaryWriter<OutputBuffer>>(m_RootType);
            //_deserializer = new Deserializer<CompactBinaryReader<InputBuffer>>(m_RootType);
        }

        public override void Serialize(object root, Stream stream)
        {
            var output = new OutputStream(stream);
            var writer = new CompactBinaryWriter<OutputStream>(output);
            Bond.Serialize.To(writer, root);
            output.Flush();
            //stream.Position = 0;
            //_serializer.Serialize(root, writer);
        }

        public override object Deserialize(Stream stream)
        {
            var input = new InputStream(stream);
            var reader = new CompactBinaryReader<InputStream>(input);
            return Bond.Deserialize<object>.From(reader);
            //return _deserializer.Deserialize(reader);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            var output = new OutputStream(stream);
            var writer = new CompactBinaryWriter<OutputStream>(output);
            Bond.Serialize.To(writer, root);
            output.Flush();
            //stream.Position = 0;
           //_serializer.Serialize(root, writer);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            var input = new InputStream(stream);
            var reader = new CompactBinaryReader<InputStream>(input);
            return Bond.Deserialize<object>.From(reader);
            //return _deserializer.Deserialize(reader);
        }

        public override bool AssertPayloadEquality(Test test, object original, object deserialized, bool abort = true)
        {
            string serError = null;
            if (test.Name.Contains("Telemetry"))
            {
                if (!TelemetryData.AssertPayloadEquality(original, deserialized, out serError))
                {
                    if (abort) test.Abort(this, serError);
                    return false;
                }
            }
            else if (test.Name.Contains("EDI_X12_835"))
            {
                if (!EDI_X12_835Data.AssertPayloadEquality(original, deserialized, out serError))
                {
                    if (abort) test.Abort(this, serError);
                    return false;
                }
            }
            return base.AssertPayloadEquality(test, original, deserialized, abort);
        }
    }
}