using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NFX;
using NFX.Environment;

using NetSerializer;


namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents NetSerializer:
    /// See here: https://github.com/tomba/netserializer  
    /// Add: PM> Install-Package NetSerializer
    /// </summary>
    [SerializerInfo( 
     Family = SerializerFamily.Binary,    
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Tomi Valkeinen",
     VendorLicense = "The Mozilla Public License 2.0 (MPL)",
     VendorURL = "https://github.com/tomba/netserializer",
     VendorPackageAddress = "Install-Package NetSerializer",
     FormatName = "NetSerializer",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 30
    )]
    public class NetSerializerSer : Serializer
    {
        public NetSerializerSer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        private NetSerializer.Serializer m_Serializer;
        private Type[] m_KnownTypes;


        public override void BeforeRuns(Test test)
        {
            try
            {
                m_Serializer = m_KnownTypes==null ? new NetSerializer.Serializer( new Type[]{test.GetPayloadRootType() }) 
                                                  : new NetSerializer.Serializer( new Type[]{test.GetPayloadRootType() }.Concat(m_KnownTypes));
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making NetSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }


        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.Serialize(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.Serialize(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
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
