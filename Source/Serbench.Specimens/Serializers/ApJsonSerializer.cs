using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Apolyton.FastJson;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Apolyton.FastJson.Json:
    /// See here http://www.codeproject.com/Articles/491742/APJSON
    /// Manually download a dll from mentioned site and add a reference to it. 
    /// </summary>
    [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Aron Kovacs",
     VendorLicense = "The Code Project Open License (CPOL)",
     VendorURL = "http://www.codeproject.com/Articles/491742/APJSON",
     VendorPackageAddress = "Apolyton.FastJson.dll",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 70
  )]
   public class ApJsonSerializer : Serializer
    {
        //private Type[] m_KnownTypes;
        private Type m_PrimaryType;

        public ApJsonSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            //m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            try
            {
                m_PrimaryType = test.GetPayloadRootType();
            }
            catch (Exception error)
            {
                test.Abort(this, "Error making ApJsonSerializer instance in serializer BeforeRun() {0}. \n Did you decorate the primary known type correctly?".Args(error.ToMessageWithType()));
            }
        }

  
        public override void Serialize(object root, Stream stream)
        {
            var buf = Json.Current.ToJsonBytes(root);
            stream.Write(buf, 0, buf.Length);
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return Json.Current.ReadObject(sr.ReadToEnd(), m_PrimaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            var buf = Json.Current.ToJsonBytes(root);
            stream.Write(buf, 0, buf.Length);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return Json.Current.ReadObject(sr.ReadToEnd(), m_PrimaryType);
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