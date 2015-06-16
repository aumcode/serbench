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


  public class ArrayOfInt : ArrayTestBase
  {
    public ArrayOfInt(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      m_Data = Array.CreateInstance(typeof(int), Dimensions);

      NFX.Serialization.SerializationUtils.WalkArrayRead(m_Data, 
       ()=> NFX.ExternalRandomGenerator.Instance.NextScaledRandomInteger(m_Min, m_Max)
      );
    }

    [Config(Default=-1000)] private int m_Min;
    [Config(Default=+1000)] private int m_Max;

  }


  public class ArrayOfNullableInt : ArrayTestBase
  {
    public ArrayOfNullableInt(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      m_Data = Array.CreateInstance(typeof(int?), Dimensions);

      NFX.Serialization.SerializationUtils.WalkArrayRead(m_Data, 
       ()=> NFX.ExternalRandomGenerator.Instance.NextRandomInteger >0 ? (int?)null : NFX.ExternalRandomGenerator.Instance.NextScaledRandomInteger(m_Min, m_Max)
      );
    }

    [Config(Default=-1000)] private int m_Min;
    [Config(Default=+1000)] private int m_Max;

  }


  public class ArrayOfString : ArrayTestBase
  {
    public ArrayOfString(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      m_Data = Array.CreateInstance(typeof(string), Dimensions);

      NFX.Serialization.SerializationUtils.WalkArrayRead(m_Data, 
       ()=> NFX.Parsing.NaturalTextGenerator.Generate( m_StringSize )
      );
    }

    [Config]
    private int m_StringSize;

  }


  public class ArrayOfDouble : ArrayTestBase
  {
    public ArrayOfDouble(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      m_Data = Array.CreateInstance(typeof(double), Dimensions);

      NFX.Serialization.SerializationUtils.WalkArrayRead(m_Data, 
       ()=> NFX.ExternalRandomGenerator.Instance.NextRandomDouble
      );
    }
  }


  public class ArrayOfDecimal : ArrayTestBase
  {
    public ArrayOfDecimal(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      m_Data = Array.CreateInstance(typeof(decimal), Dimensions);

      NFX.Serialization.SerializationUtils.WalkArrayRead(m_Data, 
       ()=> (decimal)NFX.ExternalRandomGenerator.Instance.NextRandomDouble
      );
    }
      
  }


}
