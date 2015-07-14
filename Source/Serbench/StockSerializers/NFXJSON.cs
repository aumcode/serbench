using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



using NFX;
using NFX.Environment;
using NFX.Serialization.JSON;

namespace Serbench.StockSerializers
{
  
  /// <summary>
  /// Represents NFX.Serialization.JSON technology
  /// </summary>
    [SerializerInfo( 
     Family = SerializerFamily.Textual,    
     MetadataRequirement = MetadataRequirement.None,
     VendorName = "IT Adapter LLC",
     VendorLicense = "Apache 2.0 + Commercial",
     VendorURL = "http://itadapter.com",
     VendorPackageAddress = "https://github.com/aumcode/nfx",
     FormatName = "JSON",
     LinesOfCodeK = 2,                     
     DataTypes = 10,
     Assemblies = 1,
     ExternalReferences = 0,
     PackageSizeKb = 1777
  )]
 public class NFXJson : Serializer
  {
    public const string CONFIG_OPTIONS_SECTION = "options";


    public NFXJson(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    {
      var nopt = conf[CONFIG_OPTIONS_SECTION];
      if (nopt.Exists)
      {
         m_Options = new JSONWritingOptions();
         m_Options.Configure(nopt);
      }  
      else
        m_Options = JSONWritingOptions.Compact;
    }
    
    private JSONWritingOptions m_Options;

    /// <summary>
    /// Returns options used for writing as configured
    /// </summary>
    public JSONWritingOptions Options { get{ return m_Options;}}


    public override void Serialize(object root, Stream stream)
    {
     JSONWriter.Write(root, stream, m_Options); 
    }

    public override object Deserialize(Stream stream)
    {
      return JSONReader.DeserializeDataObject(stream);
    }

    public override void ParallelSerialize(object root, Stream stream)
    {
      JSONWriter.Write(root, stream, m_Options); 
    }

    public override object ParallelDeserialize(Stream stream)
    {
      return JSONReader.DeserializeDataObject(stream);
    }


  }

}
