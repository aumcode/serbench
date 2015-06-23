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
        public override Type GetPayloadRootType()
        {
            return this.GetType();
        }

        public override void PerformSerializationTest(Serializer serializer, Stream target)
        {
            //serializer.Serialize(root, target);
        }

        public override void PerformDeserializationTest(Serializer serializer, Stream target)
        {
            var deserialized = serializer.Deserialize(target);

            //// short test to make sure the Measurements array has the same size before serialization and after deserialization:
            // if (deserialized==null)
            //  {
            //    if (original==null) return true;
            //    this.Abort(serializer, "Deserialized null from non-null Measurements");
            //    return false;
            //  }

            //  if (this.Measurements==null)
            //  {
            //    if (abort) test.Abort(serializer, "Original Measurements were null but deserialized into non-null");
            //    return false;
            //  }

            //    var deserializedTyped = deserialized as EDI_X12_835;
            //    if (deserializedTyped.Measurements == null 
            //        || deserializedTyped.Measurements.Length != this.Measurements.Length)
            //    {
            //      this.Abort(serializer, "Original and deserized Measurements are mismatch");
            //      return false;
            //    }
        }
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class EDI_X12_835Data
    {

        public EDI_X12_835Data() { }

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
        public BPR_FinancialInformation() : base("BPR") { }
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
        public TRN_ReassociationTraceNumber() : base("TRN") { }
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
        public List<PER_ClaimContactInformation> PER_ClaimContactInformation;
        [ProtoMember(9)]
        [DataMember]
        public List<AMT_ClaimSupplementalInformation> AMT_ClaimSupplementalInformation;
        [ProtoMember(10)]
        [DataMember]
        public List<QTY_ClaimSupplementalInformationQuantity> QTY_ClaimSupplementalInformationQuantity;
        [ProtoMember(11)]
        [DataMember]
        public TS835_2110_Loop TS835_2110_Loop;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class LX_HeaderNumber : Segment
    {
        public LX_HeaderNumber() : base("LX") { }
        [ProtoMember(1)]
        [DataMember]
        public string Assigned_Number;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class TS3_ProviderSummaryInformation : Segment
    {
        public TS3_ProviderSummaryInformation() : base("TS3") { }
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
        public string Quantity;
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
        public TS2_ProviderSupplementalSummaryInformation() : base("TS2") { }
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
        public CLP_ClaimPaymentInformation() : base("CLP") { }
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
        public CAS_Adjustment() : base("CAS") { }
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
        public NM1_PartyName() : base("NM1") { }
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
        public MIA_InpatientAdjudicationInformation() : base("MIA") { }
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
        public List<decimal> Monetary_Amount3;
        [ProtoMember(7)]
        [DataMember]
        public decimal? Quantity3;
        [ProtoMember(8)]
        [DataMember]
        public List<decimal> Monetary_Amount4;
        [ProtoMember(9)]
        [DataMember]
        public List<string> Reference_Identification2;
        [ProtoMember(10)]
        [DataMember]
        public decimal? Monetary_Amount5;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class MOA_OutpatientAdjudicationInformation : Segment
    {
        public MOA_OutpatientAdjudicationInformation() : base("MOA") { }
        [ProtoMember(1)]
        [DataMember]
        public int? Percentage_as_Decimal;
        [ProtoMember(2)]
        [DataMember]
        public decimal? Monetary_Amount;
        [ProtoMember(3)]
        [DataMember]
        public List<decimal> Reference_Identification;
        [ProtoMember(4)]
        [DataMember]
        public List<decimal> Monetary_Amounts;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class DTM_SubLoop
    {
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
        public PER_ClaimContactInformation() : base("PER") { }
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
        public List<string> Communication_Number;
        [ProtoMember(5)]
        [DataMember]
        public string Contact_Inquiry_Reference;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class AMT_ClaimSupplementalInformation : Segment
    {
        public AMT_ClaimSupplementalInformation() : base("AMT") { }
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
        public QTY_ClaimSupplementalInformationQuantity() : base("QTY") { }
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
        [ProtoMember(1)]
        [DataMember]
        public SVC_ServicePaymentInformation SVC_ServicePaymentInformation;
        [ProtoMember(2)]
        [DataMember]
        public List<DTM_Date> DTM_ServiceDate;
        [ProtoMember(3)]
        [DataMember]
        public List<CAS_Adjustment> CAS_ServiceAdjustment;
        [ProtoMember(4)]
        [DataMember]
        public REF_SubLoop REF_SubLoop_3;
        [ProtoMember(5)]
        [DataMember]
        public List<AMT_ServiceSupplementalAmount> AMT_ServiceSupplementalAmount;
        [ProtoMember(6)]
        [DataMember]
        public List<QTY_ServiceSupplementalQuantity> QTY_ServiceSupplementalQuantity;
        [ProtoMember(7)]
        [DataMember]
        public List<LQ_HealthCareRemarkCodes> LQ_HealthCareRemarkCodes;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class SVC_ServicePaymentInformation : Segment
    {
        public SVC_ServicePaymentInformation() : base("SVC") { }
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
        public AMT_ServiceSupplementalAmount() : base("AMT") { }
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
        public QTY_ServiceSupplementalQuantity() : base("QTY") { }
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
        public LQ_HealthCareRemarkCodes() : base("LQ") { }
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
        public PLB_ProviderAdjustment() : base("PLB") { }
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
        [ProtoMember(1)]
        [DataMember]
        public string Qualifier;
        [ProtoMember(2)]
        [DataMember]
        public decimal MonetaryAmount;
    }


}
