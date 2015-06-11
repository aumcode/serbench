using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Serbench
{
  /// <summary>
  /// Base exception thrown by the tool
  /// </summary>
  [Serializable]
  public class SerbenchException : Exception
  {
    public SerbenchException()
    {
    }

    public SerbenchException(string message)
      : base(message)
    {
    }

    public SerbenchException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected SerbenchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
      : base(info, context)
    {

    }


  }
}
