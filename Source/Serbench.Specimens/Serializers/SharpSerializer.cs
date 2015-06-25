using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NFX;
using NFX.Environment;

using Polenter.Serialization;


namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents SharpSerializer:
    /// See here: http://www.sharpserializer.com/en/index.html  
    /// Add: PM> Install-Package SharpSerializer
    /// </summary>
    [SerializerInfo( 
     Family = SerializerFamily.Binary,    
     MetadataRequirement = MetadataRequirement.Attributes,
     VendorName = "Pawel Idzikowski / Polenter-Software Solutions",
     VendorLicense = "New BSD License (BSD)",
     VendorURL = "http://www.sharpserializer.com/en/index.html",
     VendorPackageAddress = "Install-Package SharpSerializer",
     FormatName = "SharpSerializer",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 244
    )]
    public class SharpSerializer : Serializer
    {
        public SharpSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            var settings = new Polenter.Serialization.SharpSerializerBinarySettings
              {
                  Mode = BinarySerializationMode.Burst
              };
            m_Serializer = new Polenter.Serialization.SharpSerializer(settings);   
        }

        private Polenter.Serialization.SharpSerializer m_Serializer;


        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.Serialize(root, stream);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.Serialize(root, stream);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }
    }
}