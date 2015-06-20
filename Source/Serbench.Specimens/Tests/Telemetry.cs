using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Serbench.Specimens.Tests
{
    [ProtoContract] [DataContract] [Serializable] public class Telemetry
    {
         [ProtoMember(1)][DataMember] public string Id;
         [ProtoMember(2)][DataMember] public DateTime TimeStamp;
         [ProtoMember(3)][DataMember] public int Param1;
         [ProtoMember(4)][DataMember] public int Param2;
         [ProtoMember(5)][DataMember] public double[] Measurements;
    }
}
