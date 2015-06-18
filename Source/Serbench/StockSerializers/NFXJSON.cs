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
