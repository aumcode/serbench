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
    public enum MaritalStatus
    {
        Married,
        Divorced,
        HatesAll
    }

    [DataContract]
    [Serializable]
    public class TypicalPersonData
    {
        public string Address1;
        public string Address2;
        public string AddressCity;
        public string AddressState;
        public string AddressZip;
        public double CreditScore;
        public DateTime DOB;
        public string EMail;
        public string FirstName;
        public string HomePhone;
        public string LastName;
        public MaritalStatus MaritalStatus;
        public string MiddleName;
        public string MobilePhone;
        public bool RegisteredToVote;
        public decimal Salary;
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


    public class TypicalPerson : Test
    {
        [Config] private readonly int m_Count;

        private readonly List<TypicalPersonData> m_Data = new List<TypicalPersonData>();

        [Config] private bool m_List;

        public TypicalPerson(TestingSystem context, IConfigSectionNode conf) : base(context, conf)
        {
            if (m_Count < 1) m_Count = 1;

            for (var i = 0; i < m_Count; i++)
                m_Data.Add(TypicalPersonData.MakeRandom());
        }

        /// <summary>
        ///     How many records in the list
        /// </summary>
        public int Count
        {
            get { return m_Count; }
        }

        /// <summary>
        ///     Determines whether list of objects is serialized isntead of a single object
        /// </summary>
        public bool List
        {
            get { return m_List; }
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
                    Abort("Did not get list back");
                    return;
                }
                if (got.Count != m_Data.Count)
                {
                    Abort("Did not get same count");
                }
                ;
            }
            else
            {
                var got = serializer.Deserialize(target) as TypicalPersonData;
                if (got == null)
                {
                    Abort("Did not get a person back");
                }
            }
        }
    }
}