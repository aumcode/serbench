using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFX;
using NFX.Environment;
using NFX.Parsing;

namespace Serbench.StockTests
{

  public abstract class ArrayTestBase : Test
  {
    public const string CONFIG_DIMENSIONS_ATTR = "dimensions";

    protected ArrayTestBase(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      var dims = conf.AttrByName(CONFIG_DIMENSIONS_ATTR).Value;
      
      if (dims.IsNullOrWhiteSpace())
        throw new SerbenchException("Array dimensions attribute '{0}' is not specified".Args(CONFIG_DIMENSIONS_ATTR));

      m_Dimensions = dims.Split(',',';').Where(s => s.IsNotNullOrWhiteSpace())
                                        .Select(s => s.AsInt())
                                        .ToArray();  
      
      if (m_Dimensions.Length<1 || m_Dimensions.Any(d => d<=0))
       throw new SerbenchException("Invalid array dimensions attribute '{0}' = '{1}'".Args(CONFIG_DIMENSIONS_ATTR, dims));                                        
    }
    

    private int[] m_Dimensions;

    protected Array m_Data;

    /// <summary>
    /// Returns array dimensions
    /// </summary>
    public int[] Dimensions{ get{ return m_Dimensions;}}



    public override Type GetPayloadRootType()
    {
      return m_Data.GetType();
    }


    public override void PerformSerializationTest(Serializer serializer, Stream target)
    {
      serializer.Serialize(m_Data, target);
    }

    public override void PerformDeserializationTest(Serializer serializer, Stream target)
    {
       var got = serializer.Deserialize(target) as Array;
       if (got==null){ Abort(serializer, "Did not get an array back"); return;}
       if (got.Length!=m_Data.Length){ Abort(serializer, "Length is different"); return; }
    }

  }
}
