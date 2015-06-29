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
    public class Telemetry : Test
    {
        public Telemetry(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            if (m_MeasurementsNumber < 1) m_MeasurementsNumber = 1;
            m_Data = TelemetryData.Make(m_MeasurementsNumber);
        }


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
            var deserialized = serializer.Deserialize(target);
            serializer.AssertPayloadEquality(this, m_Data, deserialized);
        }

        [Config(Default = 64)]
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
        public TelemetryData() { }       

 
        [ProtoMember(1)]
        [DataMember]
        public string Id;

        [ProtoMember(2)]
        [DataMember]
        public string DataSource;

        [ProtoMember(3)]
        [DataMember]
        public DateTime TimeStamp;

        [ProtoMember(4)]
        [DataMember]
        public int Param1;

        [ProtoMember(5)]
        [DataMember]
        public uint Param2;

        [ProtoMember(6)]
        [DataMember]
        public double[] Measurements;

        [ProtoMember(7)]
        [DataMember]
        public long AssociatedProblemID;

        [ProtoMember(8)]
        [DataMember]
        public long AssociatedLogID;

        [ProtoMember(9)]
        [DataMember]
        public bool WasProcessed;

        public static TelemetryData Make(int measurementsNumber)
        {
            TelemetryData data = new TelemetryData()
            {
                Id = Guid.NewGuid().ToString(),
                DataSource = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now,
                Param1 = ExternalRandomGenerator.Instance.NextRandomInteger,
                Param2 = (uint)ExternalRandomGenerator.Instance.NextRandomInteger,
                Measurements = new double[measurementsNumber],
                AssociatedProblemID = 123,
                AssociatedLogID = 89032,
                WasProcessed = true
            };
            for (var i = 0; i < measurementsNumber; i++)
                data.Measurements[i] = ExternalRandomGenerator.Instance.NextRandomDouble;
            return data;
        }

        public static bool AssertPayloadEquality(object original, object deserialized, out string errorString)
        {
            errorString = null;

            var originalTyped = original as Serbench.Specimens.Tests.TelemetryData;
            var deserializedTyped = deserialized as Serbench.Specimens.Tests.TelemetryData;

            if (originalTyped == null || deserializedTyped == null)
                errorString = "Error: originalTyped or deserializedTyped == null";
            else if (originalTyped.Measurements == null || deserializedTyped.Measurements == null)
                errorString = "Error: originalTyped or deserializedTyped == null";
            else if (originalTyped.Measurements.Length != deserializedTyped.Measurements.Length)
                errorString = "Error: originalTyped.Measurements.Length == deserializedTyped.Measurements.Length ({0} != {1}".Args(originalTyped.Measurements.Length, deserializedTyped.Measurements.Length);
            else if (originalTyped.AssociatedProblemID != deserializedTyped.AssociatedProblemID || originalTyped.AssociatedLogID != deserializedTyped.AssociatedLogID)
                errorString = "Error: originalTyped.AssociatedProblemID != deserializedTyped.AssociatedProblemID ({0} != {1})  || originalTyped.AssociatedLogID != deserializedTyped.AssociatedLogID ({2} != {3})".Args(
                    originalTyped.AssociatedProblemID, deserializedTyped.AssociatedProblemID,
                     originalTyped.AssociatedLogID, deserializedTyped.AssociatedLogID
                    );
            return (errorString == null) ? true : false;
        }
    }
}
