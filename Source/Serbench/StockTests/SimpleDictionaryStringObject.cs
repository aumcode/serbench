using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFX;
using NFX.Environment;

namespace Serbench.StockTests
{
  
  public class SimpleDictionaryStringObject : Test
  {
    public SimpleDictionaryStringObject(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      for(var i=0; i<m_Count; i++)
       m_Dict.Add("{0}.{1}".Args(NFX.Parsing.NaturalTextGenerator.Generate(m_KeyLength), i), i);
    }
    

    [Config]
    private int m_KeyLength;

    [Config]
    private int m_Count;

    private Dictionary<string, object> m_Dict = new Dictionary<string,object>();

    /// <summary>
    /// How many items to keep in the dictionary
    /// </summary>
    public int Count{ get{ return m_Count;}}

    /// <summary>
    /// How long should the key be
    /// </summary>
    public int KeyLength{ get{ return m_KeyLength;}}


    public override Type GetPayloadRootType()
    {
      return m_Dict.GetType();
    }

    public override void PerformSerializationTest(Serializer serializer, Stream target)
    {
      serializer.Serialize(m_Dict, target);
    }

    public override void PerformDeserializationTest(Serializer serializer, Stream target)
    {
      var got = serializer.Deserialize(target) as Dictionary<string, object>;
      if (got==null){ Abort(serializer, "Did not get dict back"); return; }
      if (got.Count!=m_Dict.Count){ Abort(serializer, "Did not get same count"); return;}

    }

  }
}
