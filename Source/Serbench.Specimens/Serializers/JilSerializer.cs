using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Jil;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Jil serializer:
    /// See here https://github.com/kevin-montrose/Jil
    /// >PM Install-Package Jil 
    /// </summary>
     [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Kevin Montrose",
     VendorLicense = "The MIT License (MIT)",
     VendorURL = "https://github.com/kevin-montrose/Jil",
     VendorPackageAddress = "Install-Package Jil",
     FormatName = "JSON",
     LinesOfCodeK = 60,                     
     DataTypes = 320,     //https://github.com/kevin-montrose/Jil/issues/141#issuecomment-121346441
     Assemblies = 2,      //JIL + SIGIL
     ExternalReferences = 1,//SIGIL
     PackageSizeKb = 393
  )]
    public class JilSerializer : Serializer
    {
        //private readonly JilSerializer m_Serializer;
        private Type[] m_KnownTypes;
        private Type m_PrimaryType;

        public JilSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            m_PrimaryType = test.GetPayloadRootType();
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                JSON.Serialize(root, sw);
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return JSON.Deserialize(sr, m_PrimaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                JSON.Serialize(root, sw);
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return JSON.Deserialize(sr, m_PrimaryType);
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