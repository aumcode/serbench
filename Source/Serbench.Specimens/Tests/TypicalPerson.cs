using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;
using NFX.Parsing;
using ProtoBuf;

namespace Serbench.Specimens.Tests
{
    [DataContract]
    public enum MaritalStatus
    {
        [EnumMember]
        Married,
        [EnumMember]
        Divorced,
        [EnumMember]
        HatesAll
    }

    [ProtoContract]
    [CollectionDataContract]
    [DataContract]
    [Serializable]
    public class TypicalPersonData
    {
        /// <summary>
        /// Required by some serilizers (i.e. XML)
        /// </summary>
        public TypicalPersonData() { }


        [ProtoMember(1)]
        [DataMember]
        public string Address1;
        [ProtoMember(2)]
        [DataMember]
        public string Address2;
        [ProtoMember(3)]
        [DataMember]
        public string AddressCity;
        [ProtoMember(4)]
        [DataMember]
        public string AddressState;
        [ProtoMember(5)]
        [DataMember]
        public string AddressZip;
        [ProtoMember(6)]
        [DataMember]
        public double CreditScore;
        [ProtoMember(7)]
        [DataMember]
        public DateTime DOB;
        [ProtoMember(8)]
        [DataMember]
        public string EMail;
        [ProtoMember(9)]
        [DataMember]
        public string FirstName;
        [ProtoMember(10)]
        [DataMember]
        public string HomePhone;
        [ProtoMember(11)]
        [DataMember]
        public string LastName;
        [ProtoMember(12)]
        [DataMember]
        public MaritalStatus MaritalStatus;
        [ProtoMember(13)]
        [DataMember]
        public string MiddleName;
        [ProtoMember(14)]
        [DataMember]
        public string MobilePhone;
        [ProtoMember(15)]
        [DataMember]
        public bool RegisteredToVote;
        [ProtoMember(16)]
        [DataMember]
        public decimal Salary;
        [ProtoMember(17)]
        [DataMember]
        public int YearsOfService;

        public static TypicalPersonData MakeRandom()
        {
            return new TypicalPersonData
            {
                FirstName = NaturalTextGenerator.GenerateFirstName(),
                MiddleName =
                    ExternalRandomGenerator.Instance.NextRandomInteger > 500000000
                        ? NaturalTextGenerator.GenerateFirstName()
                        : null,
                LastName = NaturalTextGenerator.GenerateLastName(),
                DOB = DateTime.Now.AddYears(ExternalRandomGenerator.Instance.NextScaledRandomInteger(-90, -1)),
                Salary = ExternalRandomGenerator.Instance.NextScaledRandomInteger(30, 250) * 1000m,
                YearsOfService = 25,
                CreditScore = 0.7562,
                RegisteredToVote = (DateTime.UtcNow.Ticks & 1) == 0,
                MaritalStatus = MaritalStatus.HatesAll,
                Address1 = NaturalTextGenerator.GenerateAddressLine(),
                Address2 = NaturalTextGenerator.GenerateAddressLine(),
                AddressCity = NaturalTextGenerator.GenerateCityName(),
                AddressState = "CA",
                AddressZip = "91606",
                HomePhone = (DateTime.UtcNow.Ticks & 1) == 0 ? "(555) 123-4567" : null,
                EMail = NaturalTextGenerator.GenerateEMail()
            };
        }
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TypicalPerson : Test
    {
        [Config]
        private int m_Count;

        private List<TypicalPersonData> m_Data = new List<TypicalPersonData>();

        [Config]
        private bool m_List;

        public TypicalPerson(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            if (m_Count < 1) m_Count = 1;

            for (var i = 0; i < m_Count; i++)
                m_Data.Add(TypicalPersonData.MakeRandom());
        }

        /// <summary>
        /// How many records in the list
        /// </summary>
        [ProtoMember(1)]
        [DataMember]
        public int Count
        {
            get { return m_Count; }
        }

        /// <summary>
        /// Determines whether list of objects is serialized isntead of a single object
        /// </summary>
        [ProtoMember(2)]
        [DataMember]
        public bool List
        {
            get { return m_List; }
        }

        public override Type GetPayloadRootType()
        {
            var root = m_List ? (object)m_Data : m_Data[0];
            return root.GetType();
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            var root = m_List ? (object)m_Data : m_Data[0];
            serializer.Serialize(root, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
     var got = serializer.Deserialize(target);
      
      var originalRoot = m_List ? (object)m_Data : m_Data[0];
      serializer.AssertPayloadEquality(this, originalRoot, got);
        }
    }
}