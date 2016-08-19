﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX.Environment;

namespace Serbench.Specimens.Serializers
{
  /// <summary>
  /// Represents Wire Binary serializer for POCO objects
  /// See here https://github.com/akkadotnet/Wire
  /// >PM Install-Package Wire
  /// </summary>
  [SerializerInfo(
    Family = SerializerFamily.Binary,
    MetadataRequirement = MetadataRequirement.Attributes,
    VendorName = "Roger Johansson",
    VendorLicense = "The Apache License 2.0",
    VendorURL = "https://github.com/akkadotnet/Wire",
    VendorPackageAddress = "Install-Package wire",
    FormatName = "wire",
    LinesOfCodeK = 0,
    DataTypes = 0,
    Assemblies = 1,
    ExternalReferences = 0,
    PackageSizeKb = 193
  )]
  public class WireSerializer : Serializer
  {
    public WireSerializer(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
    {
    }
    
    private Wire.Serializer m_Serializer = new Wire.Serializer();

    public override object Deserialize(Stream stream)
    {
      return m_Serializer.Deserialize(stream);
    }

    public override object ParallelDeserialize(Stream stream)
    {
      return m_Serializer.Deserialize(stream);
    }

    public override void ParallelSerialize(object root, Stream stream)
    {
      m_Serializer.Serialize(root, stream);
    }

    public override void Serialize(object root, Stream stream)
    {
      m_Serializer.Serialize(root, stream);
    }
  }
}
