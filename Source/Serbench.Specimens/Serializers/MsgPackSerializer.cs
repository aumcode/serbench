﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;  
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

        private IMessagePackSerializer m_Serializer = null;

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
    }
}
