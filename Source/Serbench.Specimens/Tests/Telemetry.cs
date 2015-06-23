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
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class Telemetry : Test
    {
        public Telemetry(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            if (m_MeasurementsNumber < 1) m_MeasurementsNumber = 1;
            m_Data = new TelemetryData(m_MeasurementsNumber);
        }


        public override Type GetPayloadRootType()
        {
            return m_Data.GetType();
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            var m_Data = new TelemetryData(m_MeasurementsNumber);
            serializer.Serialize(m_Data, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
            var deserialized = serializer.Deserialize(target);

            // short test to make sure the Measurements array has the same size before serialization and after deserialization:
            if (deserialized == null)
            {
                this.Abort(serializer, "Deserialized null from non-null original Telemetry");
                return;
            }

            var deserializedTyped = deserialized as TelemetryData;
            if (deserializedTyped.Measurements == null
                || deserializedTyped.Measurements.Length != m_Data.Measurements.Length)
            {
                this.Abort(serializer, "Original and deserized Measurements are mismatch");
                return;
            }
        }

        [Config]
        private int m_MeasurementsNumber;
        public int MeasurementsNumber
        {
            get { return m_MeasurementsNumber; }
        }

        private TelemetryData m_Data;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TelemetryData
    {
        /// <summary>
        /// Required by some serilizers (i.e. XML)
        /// </summary>
        public TelemetryData() : this(1) { }
        public TelemetryData(int measurementsNumber)
        {
            Id = Guid.NewGuid().ToString();
            TimeStamp = DateTime.Now;
            Param1 = ExternalRandomGenerator.Instance.NextRandomInteger;
            Param2 = (uint)ExternalRandomGenerator.Instance.NextRandomInteger;
            Measurements = new double[measurementsNumber];
            for (var i = 0; i < measurementsNumber; i++)
                Measurements[i] = ExternalRandomGenerator.Instance.NextRandomDouble;
        }

        [ProtoMember(1)]
        [DataMember]
        public string Id;
        [ProtoMember(2)]
        [DataMember]
        public DateTime TimeStamp;
        [ProtoMember(3)]
        [DataMember]
        public int Param1;
        [ProtoMember(4)]
        [DataMember]
        public uint Param2;
        [ProtoMember(5)]
        [DataMember]
        public double[] Measurements;
    }
}
