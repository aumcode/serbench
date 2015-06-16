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

                  public enum MaritalStatus {Married, Divorced, HatesAll}

                  [Serializable]
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

                    public static TypicalPersonData MakeRandom()
                    {
                      return new TypicalPersonData
                      {
                        FirstName = NaturalTextGenerator.GenerateFirstName(),
                        MiddleName = ExternalRandomGenerator.Instance.NextRandomInteger>500000000 ? NaturalTextGenerator.GenerateFirstName() : null,
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

                    }
                  }



  
  public class TypicalPerson : Test
  {
    public TypicalPerson(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    { 
      if (m_Count<1) m_Count = 1;

      for(var i=0; i<m_Count; i++)
       m_Data.Add( TypicalPersonData.MakeRandom());
    }
    

    [Config]
    private int m_Count;

    [Config]
    private bool m_List;

    private List<TypicalPersonData> m_Data = new List<TypicalPersonData>();

    /// <summary>
    /// How many records in the list
    /// </summary>
    public int Count{ get{ return m_Count;}}


    /// <summary>
    /// Determines whether list of objects is serialized isntead of a single object
    /// </summary>
    public bool List{ get{ return m_List;}}


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
      if (m_List)
      {
        var got = serializer.Deserialize(target) as List<TypicalPersonData>;
        if (got==null){ Abort(serializer, "Did not get list back"); return;}
        if (got.Count!=m_Data.Count){ Abort(serializer, "Did not get same count"); return; };
      }
      else
      {
        var got = serializer.Deserialize(target) as TypicalPersonData;
        if (got==null){ Abort(serializer, "Did not get a person back"); return; }
      }

    }

  }
}
