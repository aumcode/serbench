using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.Environment;

using ProtoBuf;
using ProtoBuf.Meta;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents ProtoBuf:
    /// See here: https://github.com/mgravell/protobuf-net 
    /// Add: PM> Install-Package protobuf-net
    ///     NOTE: I use the protobuf-net NuGet package because of
    ///     [http://stackoverflow.com/questions/2522376/how-to-choose-between-protobuf-csharp-port-and-protobuf-net]
    /// </summary>
    [SerializerInfo(
     Family = SerializerFamily.Binary,
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Marc Gravell",
     VendorLicense = "The Apache License 2.0",
     VendorURL = "https://github.com/tomba/netserializer",
     VendorPackageAddress = "Install-Package protobuf-net",
     FormatName = "protobuf",
     LinesOfCodeK = 0,
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 193
    )]
    public class ProtoBufSerializer : Serializer
    {
        public ProtoBufSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
            foreach (var knownType in m_KnownTypes)
                m_Model.Add(knownType, true);

            m_Model.CompileInPlace();
        }


        private RuntimeTypeModel m_Model = RuntimeTypeModel.Create();
        private Type[] m_KnownTypes;
        private Type m_PrimaryType;


        public override void BeforeRuns(Test test)
        {
            m_PrimaryType = test.GetPayloadRootType();
        }


        public override void Serialize(object root, Stream stream)
        {
            m_Model.Serialize(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Model.Deserialize(stream, null, m_PrimaryType);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Model.Serialize(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Model.Deserialize(stream, null, m_PrimaryType);
        }
        public override bool AssertPayloadEquality(Test test, object original, object deserialized, bool abort = true)
        {
            string serError = null;
            if (test.Name.Contains("Telemetry"))
                if (!Serbench.Specimens.Tests.TelemetryData.AssertPayloadEquality(original, deserialized, out serError))
                {
                    if (abort) test.Abort(this, serError);
                    return false;
                }
            return base.AssertPayloadEquality(test, original, deserialized, abort);
        }
    }
}
