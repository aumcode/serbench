using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using Newtonsoft.Json;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents Newtonsoft's Json.Net xerializer:
    /// See here http://www.newtonsoft.com/json
    /// >PM Install-Package Newtonsoft.Json 
    /// </summary>
     [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Stephen M. McKamey",
     VendorLicense = "The MIT License (MIT)",
     VendorURL = "http://www.newtonsoft.com/json",
     VendorPackageAddress = "Install-Package Newtonsoft.Json",
     FormatName = "JSON",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 502
  )]
    public class JsonNet : Serializer
    {
        private readonly JsonSerializer m_Serializer  = new JsonSerializer();
        private Type[] m_KnownTypes;
        private Type m_PrimaryType;

        public JsonNet(TestingSystem context, IConfigSectionNode conf)
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
            using (var jw = new JsonTextWriter(sw))
            {
                m_Serializer.Serialize(jw, root);
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                return m_Serializer.Deserialize(jr, m_PrimaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            using (var jw = new JsonTextWriter(sw))
            {
                m_Serializer.Serialize(jw, root);
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var jr = new JsonTextReader(sr))
            {
                return m_Serializer.Deserialize(jr, m_PrimaryType);
            }
        }
    }
}