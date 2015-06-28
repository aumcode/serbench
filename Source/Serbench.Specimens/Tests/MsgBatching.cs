using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ProtoBuf;

using NFX;
using NFX.Parsing;
using NFX.Environment;

namespace Serbench.Specimens.Tests
{
    [ProtoContract]
    [DataContract]
    [Serializable] public class AddressMessage
    {
         [ProtoMember(1)][DataMember] public string Address1;
         [ProtoMember(2)][DataMember] public string Address2;
         [ProtoMember(3)][DataMember] public string City;
         [ProtoMember(4)][DataMember] public string State;
         [ProtoMember(5)][DataMember] public string PostalCode;
         [ProtoMember(6)][DataMember] public bool CanAcceptSecureShipments;

         [ProtoMember(7)][DataMember] public string EMail;
         [ProtoMember(8)][DataMember] public string HomePhone;
         [ProtoMember(9)][DataMember] public string WorkPhone;
         [ProtoMember(10)][DataMember] public string CellPhone;
         [ProtoMember(11)][DataMember] public string Fax;

         public static AddressMessage Build()
         {
           var rnd = ExternalRandomGenerator.Instance.NextRandomInteger;
           return new AddressMessage
           {
            Address1 = NaturalTextGenerator.GenerateAddressLine(),
            Address2 = (0!=(rnd & (1<<15))) ? NaturalTextGenerator.GenerateAddressLine() : null,
            City = NaturalTextGenerator.GenerateCityName(),
            State = NaturalTextGenerator.GenerateCityName(),
            PostalCode = rnd.ToString(),
            CanAcceptSecureShipments = rnd > 0,
            EMail = rnd < -500000000 ? NaturalTextGenerator.GenerateEMail() : null,
            HomePhone = (0!=(rnd & (1<<32)))  ? "(555) 111-22234" : null,
            CellPhone = (0!=(rnd & (1<<31)))  ? "(555) 234-22234" : null,
            Fax = (0!=(rnd & (1<<30)))  ? "(555) 111-22239" : null,
           };
         }

    } 

    [ProtoContract()]
    [DataContract()]
    [Serializable] public class SomePersonalDataMessage
    {
         [ProtoMember(1)][DataMember] public Guid ID;
         [ProtoMember(2)][DataMember] public HumanName LegalName;
         [ProtoMember(3)][DataMember] public HumanName RegistrationName;
         [ProtoMember(4)][DataMember] public DateTime RegistrationDate;
         [ProtoMember(5)][DataMember] public AddressMessage Residence;
         [ProtoMember(6)][DataMember] public AddressMessage Shipping;
         [ProtoMember(7)][DataMember] public AddressMessage Billing;

         [ProtoMember(8)][DataMember]  public bool?     Reserved_BoolFlag1;
         [ProtoMember(9)][DataMember]  public bool?     Reserved_BoolFlag2;
         [ProtoMember(10)][DataMember] public int?      Reserved_IntFlag1;
         [ProtoMember(11)][DataMember] public int?      Reserved_IntFlag2;
         [ProtoMember(12)][DataMember] public double?   Reserved_DblFlag1;
         [ProtoMember(13)][DataMember] public double?   Reserved_DblFlag2;

         [ProtoMember(14)][DataMember] public byte[]   StageAccessCode;
         [ProtoMember(15)][DataMember] public byte[]   SpeakerAccessCode;

         [ProtoMember(16)][DataMember] public bool? RegisteredToVote;
         [ProtoMember(17)][DataMember] public bool? FirearmLicense;
         [ProtoMember(18)][DataMember] public bool? FishermanLicense;
         [ProtoMember(19)][DataMember] public int? YearsInTheMilitary;
         [ProtoMember(20)][DataMember] public int? YearsInSchool;
         [ProtoMember(21)][DataMember] public int? EducationGrade;
         [ProtoMember(22)][DataMember] public double? PossibleRiskFactor;
         [ProtoMember(23)][DataMember] public decimal AssetsAtHand;
         [ProtoMember(24)][DataMember] public decimal PotentialAssets;
         [ProtoMember(25)][DataMember] public decimal TotalDebt;
         [ProtoMember(26)][DataMember] public double CreditScale;

         

         public static SomePersonalDataMessage Build()
         {
           var rnd = ExternalRandomGenerator.Instance.NextRandomInteger;
           var primaryAddr = AddressMessage.Build();
           var data = new SomePersonalDataMessage
           {
            ID = Guid.NewGuid(),
            LegalName = HumanName.Build(),
            RegistrationName = HumanName.Build(),
            RegistrationDate = DateTime.Now.AddDays(-23),
            Residence = primaryAddr,
            Shipping = primaryAddr,
            Billing = primaryAddr,
            StageAccessCode = new byte[32],
            SpeakerAccessCode = new byte[32],

            YearsInSchool = (0!=(rnd & (1<<29))) ? 10 : (int?)null,
            EducationGrade = (0!=(rnd & (1<<28))) ? 230 : (int?)null,

            AssetsAtHand = 567000m,
            TotalDebt = 2345m,
            CreditScale = 0.02323d 
           };

           return data;
         }
    }



    /// <summary>
    /// This Test shows a batching scenario i.e. a full-duplex socket connection
    /// when a party needs to send X consequitive atomic messages one after another in batches
    /// </summary>
    public class MsgBatching : Test
    {
        

        public MsgBatching(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            if (m_MsgCount < 1) m_MsgCount = 1;

            m_Data = new List<SomePersonalDataMessage>(m_MsgCount);

            for (var i = 0; i < m_MsgCount; i++)
              m_Data.Add( SomePersonalDataMessage.Build() );
        }

        [Config]
        private int m_MsgCount;

        private List<SomePersonalDataMessage> m_Data = new List<SomePersonalDataMessage>();


        public override Type GetPayloadRootType()
        {                      
            return typeof(SomePersonalDataMessage);
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            foreach(var msg in m_Data)//<------ The whole point of this test is THIS LOOP that calls serialize MANY times
             serializer.Serialize(msg, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
           foreach(var msg in m_Data)
           {
              var got = serializer.Deserialize(target);
              serializer.AssertPayloadEquality(this, msg, got);
           }
        }
    }
}
