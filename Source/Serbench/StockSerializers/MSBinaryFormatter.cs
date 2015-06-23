using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


using NFX.Environment;

namespace Serbench.StockSerializers
{
    /// <summary>
    /// Represents Microsoft's BinaryFormatter technology
    /// </summary>
  [SerializerInfo( 
     Family = SerializerFamily.Binary,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "Microsoft",
     VendorLicense = "Windows",
     VendorURL = "https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter(v=vs.110).aspx",
     VendorPackageAddress = "mscorlib.dll",
     FormatName = "BinaryFormatter",
     LinesOfCodeK = 0,                     
     DataTypes = 0,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 0
  )]
    public class MSBinaryFormatter : Serializer
    {
        private BinaryFormatter m_Formatter;

        public MSBinaryFormatter(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_Formatter = new BinaryFormatter();

        }

        public override void Serialize(object root, Stream stream)
        {
            m_Formatter.Serialize(stream, root);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Formatter.Deserialize(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Formatter.Serialize(stream, root);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Formatter.Deserialize(stream);
        }
    }
}
