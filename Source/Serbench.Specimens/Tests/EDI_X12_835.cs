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
    public abstract class Segment
    {
        public Segment(string segmentTag)
        { SegmentTag = segmentTag; }
        [ProtoMember(1)]
        [DataMember]
        public string SegmentTag;
    }


    [ProtoContract]
    [DataContract]
    [Serializable]
    public class EDI_X12_835 : Test
    {
        public EDI_X12_835(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        private EDI_X12_835Data m_Data;

        public override Type GetPayloadRootType()
        {
            return m_Data.GetType();
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            var m_Data = new EDI_X12_835Data();
            serializer.Serialize(m_Data, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
            var deserialized = serializer.Deserialize(target);
            serializer.AssertPayloadEquality(this, m_Data, deserialized);
        }
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class EDI_X12_835Data
    {

        public EDI_X12_835Data()
        {
        }

        [ProtoMember(1)]
        [DataMember]
        public BPR_FinancialInformation BPR_FinancialInformation;
        [ProtoMember(2)]
        [DataMember]
        public TRN_ReassociationTraceNumber TRN_ReassociationTraceNumber;
        [ProtoMember(3)]
        [DataMember]
        public CUR_ForeignCurrencyInformation CUR_ForeignCurrencyInformation;
        [ProtoMember(4)]
        [DataMember]
        public List<REF_SubLoop> REF_SubLoop;
        [ProtoMember(6)]
        [DataMember]
        public DTM_Date DTM_ProductionDate;
        [ProtoMember(7)]
        [DataMember]
        public N1_SubLoop N1_SubLoop;
        [ProtoMember(8)]
        [DataMember]
        public TS835_2000_Loop TS835_2000_Loop;
        [ProtoMember(9)]
        [DataMember]
        public List<PLB_ProviderAdjustment> PLB_ProviderAdjustmentList;


    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class AccoungInfo
    {
        public AccoungInfo()
        {
            Account_Number = NaturalTextGenerator.GenerateWord();
            Account_Number_Qualifier = "Q";
            DFI_Identification_Number = ExternalRandomGenerator.Instance.NextRandomInteger.ToString();
            DFI_Number_Qualifier = "Q";
        }

        [ProtoMember(1)]
        [DataMember]
        public string DFI_Number_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string DFI_Identification_Number;
        [ProtoMember(3)]
        [DataMember]
        public string Account_Number_Qualifier;
        [ProtoMember(4)]
        [DataMember]
        public string Account_Number;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class BPR_FinancialInformation : Segment
    {

        public BPR_FinancialInformation()
            : base("BPR")
        {
            TransactionHandlingCode = ExternalRandomGenerator.Instance.NextRandomInteger;
            CreditDebit_Flag_Code = ExternalRandomGenerator.Instance.NextRandomInteger;
            CreditDebit_Flag_Code = ExternalRandomGenerator.Instance.AsInt();
            Payment_Format_Code = "CA";
            AccoungInfo1 = new AccoungInfo();
            Originating_Company_Identifier =
            Originating_Company_Supplemental_Code = "CSC";
            AccoungInfo2 = new AccoungInfo();
            Date = DateTime.Now.ToShortDateString();
            AccoungInfo3 = new AccoungInfo();
        }

        [ProtoMember(2)]
        [DataMember]
        public int TransactionHandlingCode;
        [ProtoMember(3)]
        [DataMember]
        public int CreditDebit_Flag_Code;
        [ProtoMember(4)]
        [DataMember]
        public decimal Payment_Method_Code;
        [ProtoMember(5)]
        [DataMember]
        public string Payment_Format_Code;
        [ProtoMember(6)]
        [DataMember]
        public AccoungInfo AccoungInfo1;
        [ProtoMember(7)]
        [DataMember]
        public string Originating_Company_Identifier;
        [ProtoMember(8)]
        [DataMember]
        public string Originating_Company_Supplemental_Code;
        [ProtoMember(9)]
        [DataMember]
        public AccoungInfo AccoungInfo2;
        [ProtoMember(10)]
        [DataMember]
        public string Date;
        [ProtoMember(11)]
        [DataMember]
        public AccoungInfo AccoungInfo3;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TRN_ReassociationTraceNumber : Segment
    {

        public TRN_ReassociationTraceNumber()
            : base("TRN")
        {
            Trace_Type_Code = "TT";
            Reference_Identification = NaturalTextGenerator.GenerateWord();
            Originatin_Company_Identifier = NaturalTextGenerator.GenerateWord();
            Reference_Identification2 = NaturalTextGenerator.GenerateWord();
        }
        [ProtoMember(2)]
        [DataMember]
        public string Trace_Type_Code;
        [ProtoMember(3)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(4)]
        [DataMember]
        public string Originatin_Company_Identifier;
        [ProtoMember(5)]
        [DataMember]
        public string Reference_Identification2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CUR_DateTime
    {
        public CUR_DateTime()
        {
            Qualifier = "Q";
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToShortTimeString();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string Date;
        [ProtoMember(3)]
        [DataMember]
        public string Time;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CUR_ForeignCurrencyInformation : Segment
    {
        public CUR_ForeignCurrencyInformation() : base("CUR") { }
        [ProtoMember(1)]
        [DataMember]
        public string Entity_Identifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Currency_Code;
        [ProtoMember(3)]
        [DataMember]
        public string Exchange_Rate;
        [ProtoMember(4)]
        [DataMember]
        public string Entity_Identifier_Code2;
        [ProtoMember(5)]
        [DataMember]
        public string Currency_Code2;
        [ProtoMember(6)]
        [DataMember]
        public string Currency_Market_Exchange_Code;
        [ProtoMember(7)]
        [DataMember]
        public List<CUR_DateTime> CUR_DateTimes;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class REF_SubLoop
    {
        [ProtoMember(1)]
        [DataMember]
        public REF_Identification REF_ReceiverIdentification;
        [ProtoMember(2)]
        [DataMember]
        public REF_Identification REF_VersionIdentification;
    }
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class REF_Identification : Segment
    {
        public REF_Identification() : base("REF") { }
        [ProtoMember(1)]
        [DataMember]
        public string Reference_Identification_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(3)]
        [DataMember]
        public string Description;
        [ProtoMember(4)]
        [DataMember]
        public string Description2;
    }


    [ProtoContract]
    [DataContract]
    [Serializable]
    public class DTM_Date : Segment
    {
        public DTM_Date() : base("DTM") { }
        [ProtoMember(1)]
        [DataMember]
        public string Date_Time_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string Date;
        [ProtoMember(3)]
        [DataMember]
        public string Time;
        [ProtoMember(4)]
        [DataMember]
        public string Time_Code;
        [ProtoMember(5)]
        [DataMember]
        public string Date_Time_Period_Format_Qualifier;
        [ProtoMember(6)]
        [DataMember]
        public string Date_Time_Period;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class N1_SubLoop
    {
        [ProtoMember(1)]
        [DataMember]
        public TS835_1000A_Loop TS835_1000A_Loop;
        [ProtoMember(2)]
        [DataMember]
        public TS835_1000B_Loop TS835_1000B_Loop;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS835_1000A_Loop
    {
        [ProtoMember(1)]
        [DataMember]
        public N1_PartyIdentification N1_PayerIdentification;
        [ProtoMember(2)]
        [DataMember]
        public N3_PartyAddress N3_PayerAddress;
        [ProtoMember(3)]
        [DataMember]
        public N4_PartyCity_State_ZIPCode N4_PayerCity_State_ZIPCode;
        [ProtoMember(4)]
        [DataMember]
        public List<REF_AdditionalPartyIdentification> REF_AdditionalPayerIdentification_Loop;
        [ProtoMember(5)]
        [DataMember]
        public PER_SubLoop PER_SubLoop;
    }
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS835_1000B_Loop
    {
        [ProtoMember(1)]
        [DataMember]
        public N1_PartyIdentification N1_PayeeIdentification;
        [ProtoMember(2)]
        [DataMember]
        public N3_PartyAddress N3_PayeeAddress;
        [ProtoMember(3)]
        [DataMember]
        public N4_PartyCity_State_ZIPCode N4_PayeeCity_State_ZIPCode;
        [ProtoMember(4)]
        [DataMember]
        public List<REF_AdditionalPartyIdentification> REF_PayeeAdditionalIdentification;
        [ProtoMember(5)]
        [DataMember]
        public RDM_RemittanceDeliveryMethod RDM_RemittanceDeliveryMethod;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class N1_PartyIdentification : Segment
    {
        public N1_PartyIdentification() : base("N1") { }
        [ProtoMember(1)]
        [DataMember]
        public string Entity_Identifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Name;
        [ProtoMember(3)]
        [DataMember]
        public string Identification_Code_Qualifier;
        [ProtoMember(4)]
        [DataMember]
        public string Identification_Code;
        [ProtoMember(5)]
        [DataMember]
        public string Entity_Relationship_Code;
        [ProtoMember(6)]
        [DataMember]
        public string Entity_Identifier_Code2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class N3_PartyAddress : Segment
    {
        public N3_PartyAddress() : base("N3") { }
        [ProtoMember(1)]
        [DataMember]
        public string AddressInformation1;
        [ProtoMember(2)]
        [DataMember]
        public string AddressInformation2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class N4_PartyCity_State_ZIPCode : Segment
    {
        public N4_PartyCity_State_ZIPCode() : base("N4") { }
        [ProtoMember(1)]
        [DataMember]
        public string City_Name;
        [ProtoMember(2)]
        [DataMember]
        public string State_or_Province_Code;
        [ProtoMember(3)]
        [DataMember]
        public string Postal_Code;
        [ProtoMember(4)]
        [DataMember]
        public string Country_Code;
        [ProtoMember(5)]
        [DataMember]
        public string Location_Qualifier;
        [ProtoMember(6)]
        [DataMember]
        public string Location_Identifier;
        [ProtoMember(7)]
        [DataMember]
        public string Country_Subdivision_Code;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class REF_AdditionalPartyIdentification : Segment
    {
        public REF_AdditionalPartyIdentification() : base("REF") { }
        [ProtoMember(1)]
        [DataMember]
        public string Reference_Identification_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(3)]
        [DataMember]
        public string Description;
        [ProtoMember(4)]
        [DataMember]
        public string Description2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class PER_SubLoop
    {
        [ProtoMember(1)]
        [DataMember]
        public PER_PartyContactInformation PER_PayerBusinessContactInformation;
        [ProtoMember(2)]
        [DataMember]
        public List<PER_PartyContactInformation> PER_PayerTechnicalContactInformation;
        [ProtoMember(3)]
        [DataMember]
        public PER_PartyContactInformation PER_PayerWEBSite;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class PER_PartyContactInformation : Segment
    {
        public PER_PartyContactInformation() : base("PER") { }
        [ProtoMember(1)]
        [DataMember]
        public string Contact_Function_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Name;
        [ProtoMember(3)]
        [DataMember]
        public CommunicationNumber CommunicationNumber1;
        [ProtoMember(4)]
        [DataMember]
        public CommunicationNumber CommunicationNumber2;
        [ProtoMember(5)]
        [DataMember]
        public CommunicationNumber CommunicationNumber3;
        [ProtoMember(6)]
        [DataMember]
        public string Contact_Inquiry_Reference;
    }
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CommunicationNumber
    {
        [ProtoMember(1)]
        [DataMember]
        public string Communication_Number_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public string Communication_Number;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class RDM_RemittanceDeliveryMethod : Segment
    {
        public RDM_RemittanceDeliveryMethod() : base("RDM") { }
        [ProtoMember(1)]
        [DataMember]
        public string Report_Transmission_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Name;
        [ProtoMember(3)]
        [DataMember]
        public string Communication_Number;
        [ProtoMember(4)]
        [DataMember]
        public string Info1;
        [ProtoMember(5)]
        [DataMember]
        public string Info2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS835_2000_Loop
    {
        public TS835_2000_Loop()
        {
            LX_HeaderNumber = new LX_HeaderNumber();
            TS3_ProviderSummaryInformation = new TS3_ProviderSummaryInformation();
            TS2_ProviderSupplementalSummaryInformation = new TS2_ProviderSupplementalSummaryInformation();
            TS835_2100_Loop = new TS835_2100_Loop();
            MIA_InpatientAdjudicationInformation = new MIA_InpatientAdjudicationInformation();
            MOA_OutpatientAdjudicationInformation = new MOA_OutpatientAdjudicationInformation();
            DTM_SubLoop = new DTM_SubLoop();
            PER_ClaimContactInformations = new List<PER_ClaimContactInformation>() { new PER_ClaimContactInformation(), new PER_ClaimContactInformation(), new PER_ClaimContactInformation(), new PER_ClaimContactInformation(), };
            AMT_ClaimSupplementalInformations = new List<AMT_ClaimSupplementalInformation>() { new AMT_ClaimSupplementalInformation(), new AMT_ClaimSupplementalInformation(), new AMT_ClaimSupplementalInformation(), };
            QTY_ClaimSupplementalInformationQuantities = new List<QTY_ClaimSupplementalInformationQuantity>() { new QTY_ClaimSupplementalInformationQuantity(), new QTY_ClaimSupplementalInformationQuantity(), new QTY_ClaimSupplementalInformationQuantity(), new QTY_ClaimSupplementalInformationQuantity(), };
            TS835_2110_Loop = new TS835_2110_Loop();
        }
        [ProtoMember(1)]
        [DataMember]
        public LX_HeaderNumber LX_HeaderNumber;
        [ProtoMember(2)]
        [DataMember]
        public TS3_ProviderSummaryInformation TS3_ProviderSummaryInformation;
        [ProtoMember(3)]
        [DataMember]
        public TS2_ProviderSupplementalSummaryInformation TS2_ProviderSupplementalSummaryInformation;
        [ProtoMember(4)]
        [DataMember]
        public TS835_2100_Loop TS835_2100_Loop;
        [ProtoMember(5)]
        [DataMember]
        public MIA_InpatientAdjudicationInformation MIA_InpatientAdjudicationInformation;
        [ProtoMember(6)]
        [DataMember]
        public MOA_OutpatientAdjudicationInformation MOA_OutpatientAdjudicationInformation;
        [ProtoMember(7)]
        [DataMember]
        public DTM_SubLoop DTM_SubLoop;
        [ProtoMember(8)]
        [DataMember]
        public List<PER_ClaimContactInformation> PER_ClaimContactInformations;
        [ProtoMember(9)]
        [DataMember]
        public List<AMT_ClaimSupplementalInformation> AMT_ClaimSupplementalInformations;
        [ProtoMember(10)]
        [DataMember]
        public List<QTY_ClaimSupplementalInformationQuantity> QTY_ClaimSupplementalInformationQuantities;
        [ProtoMember(11)]
        [DataMember]
        public TS835_2110_Loop TS835_2110_Loop;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class LX_HeaderNumber : Segment
    {
        public LX_HeaderNumber()
            : base("LX")
        {
            Assigned_Number = ExternalRandomGenerator.Instance.AsDecimal().ToString();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Assigned_Number;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS3_ProviderSummaryInformation : Segment
    {
        public TS3_ProviderSummaryInformation()
            : base("TS3")
        {
            Reference_Identification = "OR";
            Facility_Code_Value = "Factory Code Value";
            Date = DateTime.Now.ToShortDateString();
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            MonetaryAmountList = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal() };
            Quantity2 = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount2 = ExternalRandomGenerator.Instance.AsDecimal();

        }
        [ProtoMember(1)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(2)]
        [DataMember]
        public string Facility_Code_Value;
        [ProtoMember(3)]
        [DataMember]
        public string Date;
        [ProtoMember(4)]
        [DataMember]
        public decimal Quantity;
        [ProtoMember(5)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(6)]
        [DataMember]
        public List<decimal> MonetaryAmountList;
        [ProtoMember(7)]
        [DataMember]
        public decimal? Quantity2;
        [ProtoMember(8)]
        [DataMember]
        public decimal? Monetary_Amount2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS2_ProviderSupplementalSummaryInformation : Segment
    {
        public TS2_ProviderSupplementalSummaryInformation()
            : base("TS2")
        {
            Monetary_AmountList = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal() };
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount2 = ExternalRandomGenerator.Instance.AsDecimal();
        }
        [ProtoMember(1)]
        [DataMember]
        public List<decimal> Monetary_AmountList;
        [ProtoMember(2)]
        [DataMember]
        public decimal Quantity;
        [ProtoMember(3)]
        [DataMember]
        public decimal? Monetary_Amount2;
        [ProtoMember(4)]
        [DataMember]
        public decimal? Monetary_Amount3;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS835_2100_Loop
    {
        public TS835_2100_Loop()
        {
            CLP_ClaimPaymentInformation = new CLP_ClaimPaymentInformation();
            CAS_ClaimsAdjustment = new CAS_Adjustment();
            NM1_SubLoop = new NM1_SubLoop();
            MIA_InpatientAdjudicationInformation = new MIA_InpatientAdjudicationInformation();
        }
        [ProtoMember(1)]
        [DataMember]
        public CLP_ClaimPaymentInformation CLP_ClaimPaymentInformation;
        [ProtoMember(2)]
        [DataMember]
        public CAS_Adjustment CAS_ClaimsAdjustment;
        [ProtoMember(3)]
        [DataMember]
        public NM1_SubLoop NM1_SubLoop;
        [ProtoMember(4)]
        [DataMember]
        public MIA_InpatientAdjudicationInformation MIA_InpatientAdjudicationInformation;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CLP_ClaimPaymentInformation : Segment
    {
        public CLP_ClaimPaymentInformation()
            : base("CLP")
        {
            Claim_Submitters_Identifier = NaturalTextGenerator.GenerateWord();
            Claim_Status_Code = NaturalTextGenerator.GenerateWord();
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal(); ;
            Monetary_Amount2 = ExternalRandomGenerator.Instance.AsDecimal(); ;
            Monetary_Amount3 = ExternalRandomGenerator.Instance.AsDecimal(); ;
            Claim_Filing_Indicator_Code = NaturalTextGenerator.GenerateWord();
            Reference_Identification = NaturalTextGenerator.GenerateWord();
            Facility_Code_Value = NaturalTextGenerator.GenerateWord();
            Claim_Frequency_Type_Code = NaturalTextGenerator.GenerateWord();
            Patient_Status_Code = NaturalTextGenerator.GenerateWord();
            Diagnosis_Related_Group_DRG_Code = NaturalTextGenerator.GenerateWord();
            Quantity = ExternalRandomGenerator.Instance.AsInt().ToString();
            Percentage_as_Decimal = ExternalRandomGenerator.Instance.AsDecimal();
            Yes_No_Condition_or_Response_Code = NaturalTextGenerator.GenerateWord();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Claim_Submitters_Identifier;
        [ProtoMember(2)]
        [DataMember]
        public string Claim_Status_Code;
        [ProtoMember(3)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(4)]
        [DataMember]
        public decimal Monetary_Amount2;
        [ProtoMember(5)]
        [DataMember]
        public decimal Monetary_Amount3;
        [ProtoMember(6)]
        [DataMember]
        public string Claim_Filing_Indicator_Code;
        [ProtoMember(7)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(8)]
        [DataMember]
        public string Facility_Code_Value;
        [ProtoMember(9)]
        [DataMember]
        public string Claim_Frequency_Type_Code;
        [ProtoMember(10)]
        [DataMember]
        public string Patient_Status_Code;
        [ProtoMember(11)]
        [DataMember]
        public string Diagnosis_Related_Group_DRG_Code;
        [ProtoMember(12)]
        [DataMember]
        public string Quantity;
        [ProtoMember(13)]
        [DataMember]
        public decimal Percentage_as_Decimal;
        [ProtoMember(14)]
        [DataMember]
        public string Yes_No_Condition_or_Response_Code;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CAS_Adjustment : Segment
    {
        public CAS_Adjustment()
            : base("CAS")
        {
            Claim_Adjustment_Group_Code = "CJ3";
            ClaimAdjustment = new ClaimAdjustment();
            ClaimAdjustments = new List<ClaimAdjustment>() { new ClaimAdjustment(), new ClaimAdjustment(), new ClaimAdjustment(), new ClaimAdjustment(), new ClaimAdjustment() };
        }
        [ProtoMember(1)]
        [DataMember]
        public string Claim_Adjustment_Group_Code;
        [ProtoMember(2)]
        [DataMember]
        public ClaimAdjustment ClaimAdjustment;
        [ProtoMember(3)]
        [DataMember]
        public List<ClaimAdjustment> ClaimAdjustments;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class ClaimAdjustment
    {
        public ClaimAdjustment()
        {
            Claim_Adjustment_Reason_Code = "CJ";
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Claim_Adjustment_Reason_Code;
        [ProtoMember(2)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public decimal? Quantity;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class NM1_SubLoop
    {
        public NM1_SubLoop()
        {
            NM1_PatientName = new NM1_PartyName();
            NM1_InsuredName = new NM1_PartyName();
            NM1_CorrectedPatient_InsuredName = new NM1_PartyName();
            NM1_ServiceProviderName = new NM1_PartyName();
            NM1_CrossoverCarrierName = new NM1_PartyName();
            NM1_CorrectedPriorityPayerName = new NM1_PartyName();
            NM1_OtherSubscriberName = new NM1_PartyName();
        }
        [ProtoMember(1)]
        [DataMember]
        public NM1_PartyName NM1_PatientName;
        [ProtoMember(2)]
        [DataMember]
        public NM1_PartyName NM1_InsuredName;
        [ProtoMember(3)]
        [DataMember]
        public NM1_PartyName NM1_CorrectedPatient_InsuredName;
        [ProtoMember(4)]
        [DataMember]
        public NM1_PartyName NM1_ServiceProviderName;
        [ProtoMember(5)]
        [DataMember]
        public NM1_PartyName NM1_CrossoverCarrierName;
        [ProtoMember(6)]
        [DataMember]
        public NM1_PartyName NM1_CorrectedPriorityPayerName;
        [ProtoMember(7)]
        [DataMember]
        public NM1_PartyName NM1_OtherSubscriberName;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class NM1_PartyName : Segment
    {
        public NM1_PartyName()
            : base("NM1")
        {
            Entity_Identifier_Code = "EI";
            Entity_Type_Qualifier = "QQ";
            Name_Last_or_Organization_Name = NaturalTextGenerator.GenerateFullName();
            Name_First = NaturalTextGenerator.GenerateFirstName();
            Name_Middle = NaturalTextGenerator.GenerateFirstName();
            Name_Prefix = "Mrs.";
            Name_Suffix = "Jr";
            Identification_Code_Qualifier = "CQ";
            Identification_Code = NaturalTextGenerator.GenerateWord();
            Entity_Relationship_Code = NaturalTextGenerator.GenerateWord();
            Entity_Identifier_Code2 = NaturalTextGenerator.GenerateWord();
            Name_Last_or_Organization_Name2 = NaturalTextGenerator.GenerateLastName();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Entity_Identifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Entity_Type_Qualifier;
        [ProtoMember(3)]
        [DataMember]
        public string Name_Last_or_Organization_Name;
        [ProtoMember(4)]
        [DataMember]
        public string Name_First;
        [ProtoMember(5)]
        [DataMember]
        public string Name_Middle;
        [ProtoMember(6)]
        [DataMember]
        public string Name_Prefix;
        [ProtoMember(7)]
        [DataMember]
        public string Name_Suffix;
        [ProtoMember(8)]
        [DataMember]
        public string Identification_Code_Qualifier;
        [ProtoMember(9)]
        [DataMember]
        public string Identification_Code;
        [ProtoMember(10)]
        [DataMember]
        public string Entity_Relationship_Code;
        [ProtoMember(11)]
        [DataMember]
        public string Entity_Identifier_Code2;
        [ProtoMember(12)]
        [DataMember]
        public string Name_Last_or_Organization_Name2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class MIA_InpatientAdjudicationInformation : Segment
    {
        public MIA_InpatientAdjudicationInformation()
            : base("MIA")
        {
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Quantity2 = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount2 = ExternalRandomGenerator.Instance.AsDecimal();
            Reference_Identification = "Ref";
            Monetary_Amounts3 = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), };
            Quantity3 = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amounts4 = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), };
            Reference_Identifications2 = new List<string>() { "RF2", "RF4", "RG1" };
            Monetary_Amount5 = ExternalRandomGenerator.Instance.AsDecimal();
        }
        [ProtoMember(1)]
        [DataMember]
        public decimal Quantity;
        [ProtoMember(2)]
        [DataMember]
        public decimal? Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public decimal? Quantity2;
        [ProtoMember(4)]
        [DataMember]
        public decimal? Monetary_Amount2;
        [ProtoMember(5)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(6)]
        [DataMember]
        public List<decimal> Monetary_Amounts3;
        [ProtoMember(7)]
        [DataMember]
        public decimal? Quantity3;
        [ProtoMember(8)]
        [DataMember]
        public List<decimal> Monetary_Amounts4;
        [ProtoMember(9)]
        [DataMember]
        public List<string> Reference_Identifications2;
        [ProtoMember(10)]
        [DataMember]
        public decimal? Monetary_Amount5;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class MOA_OutpatientAdjudicationInformation : Segment
    {
        public MOA_OutpatientAdjudicationInformation()
            : base("MOA")
        {
            Percentage_as_Decimal = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Reference_Identifications = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal() };
            Monetary_Amounts = new List<decimal>() { ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal(), ExternalRandomGenerator.Instance.AsDecimal() };
        }
        [ProtoMember(1)]
        [DataMember]
        public decimal? Percentage_as_Decimal;
        [ProtoMember(2)]
        [DataMember]
        public decimal? Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public List<decimal> Reference_Identifications;
        [ProtoMember(4)]
        [DataMember]
        public List<decimal> Monetary_Amounts;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class DTM_SubLoop
    {
        public DTM_SubLoop()
        {
            DTM_StatementFromorToDates = new List<DTM_Date>() { new DTM_Date(), new DTM_Date(), new DTM_Date(), new DTM_Date(), new DTM_Date(), };
            DTM_CoverageExpirationDate = new DTM_Date();
            DTM_ClaimReceivedDate = new DTM_Date();
        }
        [ProtoMember(1)]
        [DataMember]
        public List<DTM_Date> DTM_StatementFromorToDates;
        [ProtoMember(2)]
        [DataMember]
        public DTM_Date DTM_CoverageExpirationDate;
        [ProtoMember(3)]
        [DataMember]
        public DTM_Date DTM_ClaimReceivedDate;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class PER_ClaimContactInformation : Segment
    {
        public PER_ClaimContactInformation()
            : base("PER")
        {
            Contact_Function_Code = "CFC";
            Name = NaturalTextGenerator.GenerateFullName();
            Communication_Number_Qualifier = "DL";
            Communication_Numbers = new List<string>() { NaturalTextGenerator.GenerateEMail(), NaturalTextGenerator.GenerateEMail(), NaturalTextGenerator.GenerateEMail(), };
            Contact_Inquiry_Reference = NaturalTextGenerator.GenerateWord();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Contact_Function_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Name;
        [ProtoMember(3)]
        [DataMember]
        public string Communication_Number_Qualifier;
        [ProtoMember(4)]
        [DataMember]
        public List<string> Communication_Numbers;
        [ProtoMember(5)]
        [DataMember]
        public string Contact_Inquiry_Reference;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class AMT_ClaimSupplementalInformation : Segment
    {
        public AMT_ClaimSupplementalInformation()
            : base("AMT")
        {
            Amount_Qualifier_Code = "QC";
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Credit_Debit_Flag_Code = "USD";
        }
        [ProtoMember(1)]
        [DataMember]
        public string Amount_Qualifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public string Credit_Debit_Flag_Code;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class QTY_ClaimSupplementalInformationQuantity : Segment
    {
        public QTY_ClaimSupplementalInformationQuantity()
            : base("QTY")
        {
            Quantity_Qualifier = "QQ";
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
            Description = NaturalTextGenerator.Generate();
            Free_form_Information = NaturalTextGenerator.Generate();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Quantity_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public decimal Quantity;
        [ProtoMember(3)]
        [DataMember]
        public string Description;
        [ProtoMember(4)]
        [DataMember]
        public string Free_form_Information;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS835_2110_Loop
    {
        public TS835_2110_Loop()
        {
            SVC_ServicePaymentInformation = new SVC_ServicePaymentInformation();
            DTM_ServiceDates = new List<DTM_Date>() { new DTM_Date(), new DTM_Date(), new DTM_Date() };
            CAS_ServiceAdjustments = new List<CAS_Adjustment>() { new CAS_Adjustment(), new CAS_Adjustment(), new CAS_Adjustment(), new CAS_Adjustment() };
            REF_SubLoop_3 = new REF_SubLoop();
            AMT_ServiceSupplementalAmounts = new List<AMT_ServiceSupplementalAmount>()
                {
                    new AMT_ServiceSupplementalAmount() ,
                    new AMT_ServiceSupplementalAmount() ,
                    new AMT_ServiceSupplementalAmount() ,
                };
            QTY_ServiceSupplementalQuantities = new List<QTY_ServiceSupplementalQuantity>(){
                    new  QTY_ServiceSupplementalQuantity(),
                    new  QTY_ServiceSupplementalQuantity(),
                    new  QTY_ServiceSupplementalQuantity(),
                };
            LQ_HealthCareRemarkCodes = new List<LQ_HealthCareRemarkCodes>(){
                new  LQ_HealthCareRemarkCodes(),
                new  LQ_HealthCareRemarkCodes(),
                new  LQ_HealthCareRemarkCodes(),
                new  LQ_HealthCareRemarkCodes(),
            };
        }
        [ProtoMember(1)]
        [DataMember]
        public SVC_ServicePaymentInformation SVC_ServicePaymentInformation;
        [ProtoMember(2)]
        [DataMember]
        public List<DTM_Date> DTM_ServiceDates;
        [ProtoMember(3)]
        [DataMember]
        public List<CAS_Adjustment> CAS_ServiceAdjustments;
        [ProtoMember(4)]
        [DataMember]
        public REF_SubLoop REF_SubLoop_3;
        [ProtoMember(5)]
        [DataMember]
        public List<AMT_ServiceSupplementalAmount> AMT_ServiceSupplementalAmounts;
        [ProtoMember(6)]
        [DataMember]
        public List<QTY_ServiceSupplementalQuantity> QTY_ServiceSupplementalQuantities;
        [ProtoMember(7)]
        [DataMember]
        public List<LQ_HealthCareRemarkCodes> LQ_HealthCareRemarkCodes;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class SVC_ServicePaymentInformation : Segment
    {
        public SVC_ServicePaymentInformation()
            : base("SVC")
        {
            Qualifier = "SC";
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Monetary_Amount2 = ExternalRandomGenerator.Instance.AsDecimal();
            Product_Service_ID = NaturalTextGenerator.GenerateWord();
            Quantity = ExternalRandomGenerator.Instance.AsDecimal();
            Description = NaturalTextGenerator.Generate();
            Quantity2 = ExternalRandomGenerator.Instance.AsDecimal();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public decimal Monetary_Amount2;
        [ProtoMember(4)]
        [DataMember]
        public string Product_Service_ID;
        [ProtoMember(5)]
        [DataMember]
        public decimal? Quantity;
        [ProtoMember(6)]
        [DataMember]
        public string Description;
        [ProtoMember(7)]
        [DataMember]
        public decimal? Quantity2;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class AMT_ServiceSupplementalAmount : Segment
    {
        public AMT_ServiceSupplementalAmount()
            : base("AMT")
        {
            Amount_Qualifier_Code = "SD";
            Monetary_Amount = ExternalRandomGenerator.Instance.AsDecimal();
            Credit_Debit_Flag_Code = NaturalTextGenerator.GenerateWord();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Amount_Qualifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public decimal Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public string Credit_Debit_Flag_Code;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class QTY_ServiceSupplementalQuantity : Segment
    {
        public QTY_ServiceSupplementalQuantity()
            : base("QTY")
        {
            Quantity_Qualifier = "DG";
            Quantity = ExternalRandomGenerator.Instance.AsInt();
            Info = NaturalTextGenerator.Generate(15);
            Description = NaturalTextGenerator.Generate(35);
        }
        [ProtoMember(1)]
        [DataMember]
        public string Quantity_Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public decimal Quantity;
        [ProtoMember(3)]
        [DataMember]
        public string Info;
        [ProtoMember(4)]
        [DataMember]
        public string Description;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class LQ_HealthCareRemarkCodes : Segment
    {
        public LQ_HealthCareRemarkCodes()
            : base("LQ")
        {
            Code_List_Qualifier_Code = "QK";
            Industry_Code = NaturalTextGenerator.GenerateWord();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Code_List_Qualifier_Code;
        [ProtoMember(2)]
        [DataMember]
        public string Industry_Code;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class PLB_ProviderAdjustment : Segment
    {
        public PLB_ProviderAdjustment()
            : base("PLB")
        {
            Reference_Identification = NaturalTextGenerator.GenerateWord();
            Date = DateTime.Now.ToShortDateString();
            MonetaryAmountAdjustment = new MonetaryAmountAdjustment();
            MonetaryAmountAdjustments = new List<MonetaryAmountAdjustment>();
            var count = ExternalRandomGenerator.Instance.NextScaledRandomInteger(2, 10);
            for (var i = 0; i < count; i++)
                MonetaryAmountAdjustments.Add(new MonetaryAmountAdjustment());
        }
        [ProtoMember(1)]
        [DataMember]
        public string Reference_Identification;
        [ProtoMember(2)]
        [DataMember]
        public string Date;
        [ProtoMember(3)]
        [DataMember]
        public MonetaryAmountAdjustment MonetaryAmountAdjustment;
        [ProtoMember(4)]
        [DataMember]
        public List<MonetaryAmountAdjustment> MonetaryAmountAdjustments;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class MonetaryAmountAdjustment
    {
        public MonetaryAmountAdjustment()
        {
            Qualifier = "QA";
            MonetaryAmount = ExternalRandomGenerator.Instance.AsDecimal();
        }
        [ProtoMember(1)]
        [DataMember]
        public string Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public decimal MonetaryAmount;
    }
}
