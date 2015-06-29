using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using JsonFx.Json;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents JsonFx by Stephen M. McKamey:
    /// See here https://github.com/jsonfx/jsonfx
    /// >PM Install-Package JsonFx 
    /// </summary>
     [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Stephen M. McKamey",
     VendorLicense = "The MIT License (MIT)",
     VendorURL = "https://github.com/jsonfx/jsonfx",
     VendorPackageAddress = "Install-Package JsonFx",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 214
  )]
    public class JsonFxSerializer : Serializer
    {
        static readonly JsonWriter m_jw = new JsonWriter();
        static readonly JsonReader m_jr = new JsonReader();
        private Type m_primaryType;

        public JsonFxSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_jw.Write(root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_jr.Read(sr.ReadToEnd(), m_primaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_jw.Write(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_jr.Read(sr.ReadToEnd(), m_primaryType);
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