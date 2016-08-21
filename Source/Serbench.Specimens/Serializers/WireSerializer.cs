using System;
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

    MetadataRequirement = MetadataRequirement.None,
    VendorName = "Akka.NET Team",
    VendorLicense = "Apache 2.0",
    VendorURL = "http://getakka.net",
    VendorPackageAddress = "http://github.com/akkadotnet/wire",
    FormatName = "Wire",
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
      var opt = new Wire.SerializerOptions(false, true);
      m_Serializer = new Wire.Serializer(opt);
    }

    private Wire.Serializer m_Serializer;

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
