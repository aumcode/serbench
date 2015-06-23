using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;
using NFX.Parsing;
using ProtoBuf;

namespace Serbench.Specimens.Tests
{
    [ProtoContract] [DataContract] [Serializable] public class Telemetry : Test
    {
              public Telemetry(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
                  Initialize(context, conf);
        } 

         [ProtoMember(1)][DataMember] public string Id;
         [ProtoMember(2)][DataMember] public DateTime TimeStamp;
         [ProtoMember(3)][DataMember] public int Param1;
         [ProtoMember(4)][DataMember] public uint Param2;
         [ProtoMember(5)][DataMember] public double[] Measurements;

       private static Telemetry Initialize(TestingSystem context, IConfigSectionNode conf)
        {
           Telemetry current = new Telemetry(context, conf);
            current.Id = Guid.NewGuid().ToString();
           current.TimeStamp = DateTime.Now;
           current.Param1 =  ExternalRandomGenerator.Instance.NextScaledRandomInteger(30, 250);
           current.Param2 =  (uint)ExternalRandomGenerator.Instance.NextScaledRandomInteger(251, 25000);
           return current;
        }
        
        public override Type GetPayloadRootType()
        {
            return this.GetType();
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            //serializer.Serialize(root, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
        //   var deserialized = serializer.Deserialize(target);
      
        //// short test to make sure the Measurements array has the same size before serialization and after deserialization:
        // if (deserialized==null)
        //  {
        //    if (original==null) return true;
        //    this.Abort(serializer, "Deserialized null from non-null Measurements");
        //    return false;
        //  }

        //  if (this.Measurements==null)
        //  {
        //    if (abort) test.Abort(serializer, "Original Measurements were null but deserialized into non-null");
        //    return false;
        //  }

        //    var deserializedTyped = deserialized as Telemetry;
        //    if (deserializedTyped.Measurements == null 
        //        || deserializedTyped.Measurements.Length != this.Measurements.Length) 
        //    {
        //      this.Abort(serializer, "Original and deserized Measurements are mismatch");
        //      return false;
        //    }
         }

        [Config]
        private int m_MeasurementsNumber;  
        public int MeasurementsNumber
        {
            get { return m_MeasurementsNumber; }
        } 

 
    }
}
