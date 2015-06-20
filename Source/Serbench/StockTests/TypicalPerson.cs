using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFX;
using NFX.Environment;
using NFX.Parsing;

namespace Serbench.StockTests
{

    public enum MaritalStatus { Married, Divorced, HatesAll }

                  public class TypicalPersonData
                  {
                    public string FirstName;
                    public string MiddleName;
                    public string LastName;
                    public DateTime DOB;
                    public decimal Salary;
                    public int YearsOfService;
                    public double CreditScore;
                    public bool RegisteredToVote;
                    public MaritalStatus MaritalStatus;

                    public string Address1;
                    public string Address2;
                    public string AddressCity;
                    public string AddressState;
                    public string AddressZip;

                    public string HomePhone;
                    public string MobilePhone;

                    public string EMail;

                    public string SkypeID;
                    public string YahooID;
                    public string GoogleID;

                    public string Notes;
                    
                    public bool? IsSmoker;
                    public bool? IsLoving;
                    public bool? IsLoved;
                    public bool? IsDangerous;
                    public bool? IsEducated;
                    public DateTime? LastSmokingDate;

                    public decimal? DesiredSalary;
                    public double? ProbabilityOfSpaceFlight;

                    public int? CurrentFriendCount;
                    public int? DesiredFriendCount;

                 //   public object SomeObject;

                    public static TypicalPersonData MakeRandom(bool extraData = false)
                    {
                      var rnd = ExternalRandomGenerator.Instance.NextRandomInteger;

                      var data =  new TypicalPersonData
                      {
                        FirstName = NaturalTextGenerator.GenerateFirstName(),
                MiddleName = ExternalRandomGenerator.Instance.NextRandomInteger > 500000000 ? NaturalTextGenerator.GenerateFirstName() : null,
                        LastName = NaturalTextGenerator.GenerateLastName(),
                        DOB = DateTime.Now.AddYears(ExternalRandomGenerator.Instance.NextScaledRandomInteger(-90, -1)),
                        Salary = ExternalRandomGenerator.Instance.NextScaledRandomInteger(30,250)*1000m,
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

                      //if (extraData)
                      //  data.SomeObject = new Dictionary<object, object>
                      //  {
                      //    { "1aaaaa", TypicalPersonData.MakeRandom(false)}, { 2212, -234123}, {13000,100}, { Tuple.Create(2, false,true), "yes"}, {"4aaa",'a'},
                      //    { "a2aaaa", TypicalPersonData.MakeRandom(false)}, { 132, TypicalPersonData.MakeRandom(false)}, {130400,100}, { Tuple.Create(3, false,true), "yes"}, {"a234aa",'a'},
                      //    { "aa4aaa", TypicalPersonData.MakeRandom(false)}, { 412, -123}, {2100d,100L}, { Tuple.Create(4, false,TypicalPersonData.MakeRandom(false)), "yes"}, {"a5aa",'a'},
                      //    { "aa3aaa", TypicalPersonData.MakeRandom(false)}, { 212, 0}, {1222200m,100}, { Tuple.Create(5, false,true), "yes"}, {"a43aa1",'a'},
                      //    { "a5aaaa", TypicalPersonData.MakeRandom(false)}, { 512, new int[]{1,2,-3,4,5,-6,-1,-2,-3,-4,5,6,1,2,3,4,5,6,1,2,3,4,5,6,1,2,3,4,5,60}},
                      //    {102200,100}, { Tuple.Create(-213232d, false,false), "yes"}, {"222222222222aaa",'a'},
                      //  };

                      if (0!=(rnd & (1<<32))) data.Notes = NaturalTextGenerator.Generate(45);
                      if (0!=(rnd & (1<<31))) data.SkypeID = NaturalTextGenerator.GenerateEMail();
                      if (0!=(rnd & (1<<30))) data.YahooID = NaturalTextGenerator.GenerateEMail();

                      if (0!=(rnd & (1<<29))) data.IsSmoker = 0!=(rnd & (1<<17)); 
                      if (0!=(rnd & (1<<28))) data.IsLoving = 0!=(rnd & (1<<16)); 
                      if (0!=(rnd & (1<<27))) data.IsLoved = 0!=(rnd & (1<<15)); 
                      if (0!=(rnd & (1<<26))) data.IsDangerous = 0!=(rnd & (1<<14)); 
                      if (0!=(rnd & (1<<25))) data.IsEducated = 0!=(rnd & (1<<13)); 

                      if (0!=(rnd & (1<<24))) data.LastSmokingDate = DateTime.Now.AddYears(-10);


                      if (0!=(rnd & (1<<23))) data.DesiredSalary = rnd / 1000m;
                      if (0!=(rnd & (1<<22))) data.ProbabilityOfSpaceFlight = rnd / (double)int.MaxValue;

                      if (0!=(rnd & (1<<21))) 
                      {
                        data.CurrentFriendCount = rnd % 123;
                        data.DesiredFriendCount = rnd % 121000;
                      }
                       

                      return data;
                    }
                  }


  public class TypicalPerson : Test
  {
        public TypicalPerson(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
    { 
            if (m_Count < 1) m_Count = 1;

            for (var i = 0; i < m_Count; i++)
                m_Data.Add(TypicalPersonData.MakeRandom());
    }
    

    [Config]
    private int m_Count;

    [Config]
    private bool m_List;

    private List<TypicalPersonData> m_Data = new List<TypicalPersonData>();

    /// <summary>
    /// How many records in the list
    /// </summary>
        public int Count { get { return m_Count; } }


    /// <summary>
    /// Determines whether list of objects is serialized isntead of a single object
    /// </summary>
        public bool List { get { return m_List; } }


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
