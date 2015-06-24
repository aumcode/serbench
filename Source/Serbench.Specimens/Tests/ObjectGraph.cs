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
    
    [ProtoContract]     [DataContract(IsReference=true)] [Serializable] public struct HumanName
    {
         [ProtoMember(1)][DataMember] public string FirstName;
         [ProtoMember(2)][DataMember] public string MiddleName;
         [ProtoMember(3)][DataMember] public string LastName;

         public static HumanName Build()
         {
           return new HumanName
           {
            FirstName = NaturalTextGenerator.GenerateFirstName(),
            MiddleName = ExternalRandomGenerator.Instance.NextRandomInteger>500000000 ? NaturalTextGenerator.GenerateFirstName() : null,
            LastName = NaturalTextGenerator.GenerateLastName()
           };
         }
    }

    [ProtoContract(AsReferenceDefault=true)]
    [DataContract(IsReference=true)]
    [Serializable] public class Address
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

         public static Address Build()
         {
           var rnd = ExternalRandomGenerator.Instance.NextRandomInteger;
           return new Address
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
    
    [ProtoContract]     [DataContract(IsReference=true)] [Serializable] public struct Relationship
    {
         [ProtoMember(1)][DataMember] public string RelationshipName;
         [ProtoMember(2, AsReference=true)][DataMember] public Participant Other;
    }
    
    [ProtoContract(AsReferenceDefault=true)]
    [DataContract(IsReference=true)]
    [Serializable] public class Participant
    {
         [ProtoMember(1)][DataMember] public Guid ID;
         [ProtoMember(2)][DataMember] public HumanName LegalName;
         [ProtoMember(3)][DataMember] public HumanName RegistrationName;
         [ProtoMember(4)][DataMember] public DateTime RegistrationDate;
         [ProtoMember(5, AsReference=true)][DataMember] public Address Residence;
         [ProtoMember(6, AsReference=true)][DataMember] public Address Shipping;
         [ProtoMember(7, AsReference=true)][DataMember] public Address Billing;
         [ProtoMember(8)][DataMember] public List<Relationship> Relationships;

         [ProtoMember(9)][DataMember]  public bool?     Reserved_BoolFlag1;
         [ProtoMember(10)][DataMember] public bool?     Reserved_BoolFlag2;
         [ProtoMember(11)][DataMember] public int?      Reserved_IntFlag1;
         [ProtoMember(12)][DataMember] public int?      Reserved_IntFlag2;
         [ProtoMember(13)][DataMember] public double?   Reserved_DblFlag1;
         [ProtoMember(14)][DataMember] public double?   Reserved_DblFlag2;

         [ProtoMember(15)][DataMember] public byte[]   StageAccessCode;
         [ProtoMember(16)][DataMember] public byte[]   SpeakerAccessCode;
         

         public static Participant Build()
         {
           var rnd = ExternalRandomGenerator.Instance.NextRandomInteger;
           var primaryAddr = Address.Build();
           return new Participant
           {
            ID = Guid.NewGuid(),
            LegalName = HumanName.Build(),
            RegistrationName = HumanName.Build(),
            RegistrationDate = DateTime.Now.AddDays(-23),
            Residence = primaryAddr,
            Shipping = (0!=(rnd & (1<<32))) ? primaryAddr : Address.Build(),
            Billing = (0!=(rnd & (1<<31))) ? primaryAddr : Address.Build(),
            StageAccessCode = (0!=(rnd & (1<<30))) ? new byte[128] : null,
            SpeakerAccessCode = (0!=(rnd & (1<<30))) ? new byte[128] : null
           };
         }
    }


    [ProtoContract(AsReferenceDefault=true)] [DataContract(IsReference=true)] [Serializable] public class ConferenceTopic
    {
         [ProtoMember(1)][DataMember] public Guid ID;
         [ProtoMember(2)][DataMember] public string Name;
         [ProtoMember(3)][DataMember] public string Description;
         [ProtoMember(4)][DataMember] public bool? IsPhysics;
         [ProtoMember(5)][DataMember] public bool? IsMathematics;
         [ProtoMember(6)][DataMember] public bool? IsBiology;
         [ProtoMember(7)][DataMember] public int? PlannedAttendance;
         [ProtoMember(8)][DataMember] public int[] AttendanceHistory;

    }

    [ProtoContract(AsReferenceDefault=true)] [DataContract(IsReference=true)] [Serializable] public class Event
    {
         [ProtoMember(1)][DataMember] public Guid ID;
         [ProtoMember(2)][DataMember] public DateTime StartTime;
         [ProtoMember(3)][DataMember] public DateTime EndTime;
         [ProtoMember(5, AsReference=true)][DataMember] public List<Participant> Participants;
         [ProtoMember(6, AsReference=true)][DataMember] public List<ConferenceTopic> Topics;
    }


    [ProtoContract(AsReferenceDefault=true)] [DataContract(IsReference=true)] [Serializable] public class Conference
    {
         [ProtoMember(1)][DataMember] public Guid ID;
         [ProtoMember(2)][DataMember] public DateTime StartDate;
         [ProtoMember(3)][DataMember] public DateTime? EndDate;
         [ProtoMember(4, AsReference=true)][DataMember] public Address Location;

         [ProtoMember(5, AsReference=true)][DataMember] public List<Event> Events;
    }


    public static class ConferenceBuilder
    {
       public static Conference Build(int participantCount, int eventCount)
       {
         var topics = new ConferenceTopic[]
         {
            new ConferenceTopic{ ID = Guid.NewGuid(), 
                                 Name = "Is There a life on Mars?", 
                                 Description="We will discuss how aliens eat donuts with honey sitting at a marsian lake shore",
                                 PlannedAttendance = 80,
                                 IsPhysics = true,
                                 AttendanceHistory = new int[]{24, 27, 39, 50, 75, 234, 200, 198, 245, 188, 120, 90, 80, 24, 24, 55, 23, 45, 33} } ,
            new ConferenceTopic{ ID = Guid.NewGuid(), 
                                 Name = "Solder-Free Welding", 
                                 Description="Soldering with sugar syrop",
                                 PlannedAttendance = 120,
                                 IsBiology=true },
            new ConferenceTopic{ ID = Guid.NewGuid(), 
                                 Name = "2+2=5", 
                                 Description="What do we know about logic?",
                                 PlannedAttendance = 4000,
                                 IsMathematics=true,
                                 AttendanceHistory = new int[]{3000, 3245, 2343, 2344, 4332, 23434, 23434, 2343, 545, 2322, 3453, 2332, 2323, 3234}  },
            new ConferenceTopic{ ID = Guid.NewGuid(), 
                                 Name = "Growing Corn", 
                                 Description="Corn starches and calories?",
                                 PlannedAttendance = 233,
                                 IsBiology=true }
         };
         
         var people = new Participant[participantCount];
         for(var i=0; i<participantCount; i++) people[i] = Participant.Build();

         foreach(var person in people)
         {
           if (ExternalRandomGenerator.Instance.NextRandomInteger<750000000) continue;
           person.Relationships = new List<Relationship>();
           for (var i=0; i<ExternalRandomGenerator.Instance.NextScaledRandomInteger(0, 4); i++)
           {
             var friend = people.FirstOrDefault( p => p!=person && p.Relationships==null);
             person.Relationships.Add( new Relationship{ Other = friend, RelationshipName = "Good Friend #"+i.ToString()} );
           }
         }


         var confStartDate = DateTime.Now.AddDays(30);

         var events = new Event[eventCount];
         var sd = confStartDate;
         for(var i=0; i<eventCount; i++) 
         {
           var evt = new Event();
           evt.ID = Guid.NewGuid();
           evt.StartTime = sd;
           evt.EndTime = sd.AddMinutes( ExternalRandomGenerator.Instance.NextScaledRandomInteger(30, 480));
           sd = evt.EndTime.AddMinutes( 1 );
           evt.Participants = new List<Participant>();
           for(var j=ExternalRandomGenerator.Instance.NextScaledRandomInteger(0, people.Length); j<people.Length; j++)
            evt.Participants.Add( people[j] );
           
           evt.Topics = new List<ConferenceTopic>();
           for(var j=ExternalRandomGenerator.Instance.NextScaledRandomInteger(0, topics.Length); j<topics.Length; j++)
            evt.Topics.Add( topics[j] ); 
             
           events[i] = evt;
         }

         

         var result = new Conference
         {
           ID = Guid.NewGuid(),
           StartDate = confStartDate,
           Location = Address.Build(),
           Events = new List<Event>(events)
         };
         return result;
       }
      
    }

    /// <summary>
    /// NOTE: ProtBuf DOES NOT support references, the ProtoBuf.Net serializer does because it is an "extra feature" from Marc Gravell.
    /// So technically we are favoring not ProtoBuf but ProtoBuf.NET as Google's format does not care about object normalization.
    /// This crosses out ProtoBuff portability between platforms as other readers will not be able to "read-in" the original object graph.
    /// see: http://stackoverflow.com/questions/6063729/does-protocol-buffers-support-serialization-of-object-graphs-with-shared-referen
    /// </summary>
    public class ObjectGraph : Test
    {
        

        public ObjectGraph(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            if (m_ConferenceCount < 1) m_ConferenceCount = 1;
            if (m_ParticipantCount < 1) m_ParticipantCount = 1;
            if (m_EventCount < 1) m_EventCount = 1;

            for (var i = 0; i < m_ConferenceCount; i++)
                m_Data.Add( ConferenceBuilder.Build(m_ParticipantCount, m_EventCount ) );
        }

        [Config]
        private int m_ConferenceCount;


        [Config]
        private int m_ParticipantCount;

        [Config]
        private int m_EventCount;

        private List<Conference> m_Data = new List<Conference>();

        


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
           var got = serializer.Deserialize(target);

           serializer.AssertPayloadEquality(this, m_Data, got);
        }
    }
}
