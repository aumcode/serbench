using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using NFX;
using NFX.Environment;
using NFX.Parsing;

namespace Serbench.Specimens.Tests
{
    [DataContract]
    [Serializable]
    public enum MaritalStatus
    {
        Married,
        Divorced,
        HatesAll
    }

    [CollectionDataContract]
    [DataContract]
    [Serializable]
    public class TypicalPersonData
    {
        /// <summary>
        /// Required by some serilizers (i.e. XML)
        /// </summary>
        public TypicalPersonData() {}


        [DataMember]
        public string Address1;
        [DataMember]
        public string Address2;
        [DataMember]
        public string AddressCity;
        [DataMember]
        public string AddressState;
        [DataMember]
        public string AddressZip;
        [DataMember]
        public double CreditScore;
        [DataMember]
        public DateTime DOB;
        [DataMember]
        public string EMail;
        [DataMember]
        public string FirstName;
        [DataMember]
        public string HomePhone;
        [DataMember]
        public string LastName;
        [DataMember]
        public MaritalStatus MaritalStatus;
        [DataMember]
        public string MiddleName;
        [DataMember]
        public string MobilePhone;
        [DataMember]
        public bool RegisteredToVote;
        [DataMember]
        public decimal Salary;
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
                Salary = ExternalRandomGenerator.Instance.NextScaledRandomInteger(30, 250)*1000m,
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

    [DataContract]
    [Serializable]
    public class TypicalPerson : Test
    {
        [Config] 
        private  int m_Count;

        private  List<TypicalPersonData> m_Data = new List<TypicalPersonData>();

        [Config] 
        private bool m_List;

        public TypicalPerson(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
        {
            if (m_Count < 1) m_Count = 1;

            for (var i = 0; i < m_Count; i++)
                m_Data.Add(TypicalPersonData.MakeRandom());
        }

        /// <summary>
        /// How many records in the list
        /// </summary>
        public int Count
        {
            get { return m_Count; }
        }

        /// <summary>
        /// Determines whether list of objects is serialized isntead of a single object
        /// </summary>
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
            var root = m_List ? (object) m_Data : m_Data[0];
            serializer.Serialize(root, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
            if (m_List)
            {
                var got = serializer.Deserialize(target) as List<TypicalPersonData>;
                if (got == null)
                {
                    Abort(serializer, "Did not get list back");
                    return;
                }
                if (got.Count != m_Data.Count)
                {
                    Abort(serializer, "Did not get same count");
                }
                ;
            }
            else
            {
                var got = serializer.Deserialize(target) as TypicalPersonData;
                if (got == null)
                {
                    Abort(serializer, "Did not get a person back");
                }
            }
        }
    }
}