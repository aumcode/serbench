using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Serbench.Specimens.Tests
{
    public abstract class Segment
    {
        public Segment(string segmentTag)
        { SegmentTag = segmentTag;}
        public  string SegmentTag;
    }
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class EDI_X12_835
    {
        [ProtoMember(1)]
        [DataMember]
        public BPR_FinancialInformation BPR_FinancialInformation;
        [ProtoMember(2)]
        [DataMember]
        public TRN_ReassociationTraceNumber TRN_ReassociationTraceNumber;
        public CUR_ForeignCurrencyInformation CUR_ForeignCurrencyInformation;
        [ProtoMember(2)]
        [DataMember]
        public List<REF_SubLoop> REF_SubLoop;
        [ProtoMember(3)]
        [DataMember]
        public DTM_Date DTM_ProductionDate;
        public N1_SubLoop N1_SubLoop;
        public TS835_2000_Loop TS835_2000_Loop;
        public List<PLB_ProviderAdjustment> PLB_ProviderAdjustmentList;
    }

    public class AccoungInfo 
    {
        public string DFI_Number_Qualifier;
        public string DFI_Identification_Number;
        public string Account_Number_Qualifier;
        public string Account_Number;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class BPR_FinancialInformation : Segment
    {
        public BPR_FinancialInformation() : base("BPR") { }
        [ProtoMember(1)]
        [DataMember]
        public int TransactionHandlingCode;
        [ProtoMember(2)]
        [DataMember]
        public int CreditDebit_Flag_Code;
        [ProtoMember(3)]
        [DataMember]
        public decimal Payment_Method_Code;
        [ProtoMember(4)]
        [DataMember]
        public string Payment_Format_Code;
        [ProtoMember(5)]
        [DataMember]
        public AccoungInfo AccoungInfo1;
        [ProtoMember(6)]
        [DataMember]
         public string Originating_Company_Identifier;
        public string Originating_Company_Supplemental_Code;
        public AccoungInfo AccoungInfo2;
        public string Date;
        public AccoungInfo AccoungInfo3;
    }

    public class  TRN_ReassociationTraceNumber : Segment
    {
        public TRN_ReassociationTraceNumber() : base("TRN") { }
        public string Trace_Type_Code;
        public string Reference_Identification;
        public string Originatin_Company_Identifier;
        public string Reference_Identification2;
    }

   public class  CUR_DateTime 
   {
        public string Qualifier;
        public string Date;
        public string Time;
   }

     public class  CUR_ForeignCurrencyInformation : Segment
     {
        public CUR_ForeignCurrencyInformation() : base("CUR") { }
       public string Entity_Identifier_Code;
        public string Currency_Code;
        public string Exchange_Rate;
        public string Entity_Identifier_Code2;
        public string Currency_Code2;
        public string Currency_Market_Exchange_Code;
        public List<CUR_DateTime> CUR_DateTimes;
     }

    public class  REF_SubLoop
    {
        public REF_Identification REF_ReceiverIdentification;
        public REF_Identification REF_VersionIdentification ;
    }
    public class  REF_Identification : Segment
    {
        public REF_Identification() : base("REF") { }
       public string Reference_Identification_Qualifier;
        public string Reference_Identification;
        public string Description;
        public string Description2;
    }
   
   
    public class DTM_Date : Segment
    {
        public DTM_Date() : base("DTM") { }
        public string Date_Time_Qualifier;
        public string Date;
        public string Time;
        public string Time_Code;
        public string Date_Time_Period_Format_Qualifier;
     public string Date_Time_Period;
    }

    public class   N1_SubLoop
    {
         public TS835_1000A_Loop TS835_1000A_Loop;
         public TS835_1000B_Loop TS835_1000B_Loop;
    }

    public class TS835_1000A_Loop
    {
          public  N1_PartyIdentification N1_PayerIdentification;
        public  N3_PartyAddress N3_PayerAddress;
        public  N4_PartyCity_State_ZIPCode N4_PayerCity_State_ZIPCode;
        public   List<REF_AdditionalPartyIdentification> REF_AdditionalPayerIdentification_Loop ;
            public PER_SubLoop PER_SubLoop;
    }
   public class TS835_1000B_Loop
    {
          public  N1_PartyIdentification N1_PayeeIdentification;
        public  N3_PartyAddress N3_PayeeAddress;
        public  N4_PartyCity_State_ZIPCode N4_PayeeCity_State_ZIPCode;
        public   List<REF_AdditionalPartyIdentification> REF_PayeeAdditionalIdentification ;
            public RDM_RemittanceDeliveryMethod RDM_RemittanceDeliveryMethod;
    }

    public class N1_PartyIdentification : Segment
    {
        public N1_PartyIdentification() : base("N1") { }
        public string Entity_Identifier_Code;
        public string Name;
        public string Identification_Code_Qualifier;
        public string Identification_Code;
        public string Entity_Relationship_Code;
        public string Entity_Identifier_Code2;
    }

    public class N3_PartyAddress : Segment
    {
         public N3_PartyAddress() : base("N3") { }
       public string AddressInformation1;
        public string AddressInformation2;
    }

    public class N4_PartyCity_State_ZIPCode  : Segment
    {
        public N4_PartyCity_State_ZIPCode() : base("N4") { }
    public string City_Name;
        public string State_or_Province_Code;
        public string Postal_Code;
        public string Country_Code;
        public string Location_Qualifier;
        public string Location_Identifier;
        public string Country_Subdivision_Code;
    }

    public class REF_AdditionalPartyIdentification : Segment
    {
        public REF_AdditionalPartyIdentification( ) : base("REF") { }
        public string Reference_Identification_Qualifier;
        public string Reference_Identification;
        public string Description;
        public string Description2;
    }

    public class PER_SubLoop 
    {
        public PER_PartyContactInformation PER_PayerBusinessContactInformation;
        public List<PER_PartyContactInformation>   PER_PayerTechnicalContactInformation ;
        public PER_PartyContactInformation PER_PayerWEBSite;
    }

     public class PER_PartyContactInformation : Segment
    {
         public PER_PartyContactInformation( ) : base("PER") { }
       public string Contact_Function_Code;
        public string Name;
        public CommunicationNumber CommunicationNumber1;
        public CommunicationNumber CommunicationNumber2;
        public CommunicationNumber CommunicationNumber3;
        public string Contact_Inquiry_Reference;
    }
   public class CommunicationNumber
    {
        public string Communication_Number_Qualifier;
        public string Communication_Number;
    }

    public class RDM_RemittanceDeliveryMethod  : Segment
    {
         public RDM_RemittanceDeliveryMethod() : base("RDM") { }
        public string Report_Transmission_Code;
        public string Name;
        public string Communication_Number;
        public string Info1;
        public string Info2;
    }
    
    public class TS835_2000_Loop
    {
         public  LX_HeaderNumber LX_HeaderNumber;
        public TS3_ProviderSummaryInformation TS3_ProviderSummaryInformation;
        public TS2_ProviderSupplementalSummaryInformation TS2_ProviderSupplementalSummaryInformation;
        public  TS835_2100_Loop TS835_2100_Loop;
        public MIA_InpatientAdjudicationInformation MIA_InpatientAdjudicationInformation;
        public MOA_OutpatientAdjudicationInformation MOA_OutpatientAdjudicationInformation;
        public DTM_SubLoop DTM_SubLoop;
        public List<PER_ClaimContactInformation> PER_ClaimContactInformation;
        public List<AMT_ClaimSupplementalInformation> AMT_ClaimSupplementalInformation;
        public List<QTY_ClaimSupplementalInformationQuantity> QTY_ClaimSupplementalInformationQuantity;
        public TS835_2110_Loop TS835_2110_Loop;
    }

    public class LX_HeaderNumber : Segment
    {
         public LX_HeaderNumber() : base("LX") { }
        public string Assigned_Number;
    }

    public class TS3_ProviderSummaryInformation : Segment
    {
         public TS3_ProviderSummaryInformation() : base("TS3") { }
        public string Reference_Identification;
        public string Facility_Code_Value;
        public string Date;
        public string Quantity;
        public decimal Monetary_Amount;
        public List<decimal> MonetaryAmountList;
        public decimal? Quantity2;
        public decimal? Monetary_Amount2;
    }

    public class TS2_ProviderSupplementalSummaryInformation : Segment
    {
          public TS2_ProviderSupplementalSummaryInformation() : base("TS2") { }
       public List<decimal>   Monetary_AmountList;
         public decimal Quantity;
        public decimal? Monetary_Amount2;
        public decimal? Monetary_Amount3;
   }

    public class TS835_2100_Loop
    {
           public CLP_ClaimPaymentInformation CLP_ClaimPaymentInformation;
        public CAS_Adjustment CAS_ClaimsAdjustment;
        public NM1_SubLoop NM1_SubLoop;
        public MIA_InpatientAdjudicationInformation MIA_InpatientAdjudicationInformation;
    }

    public class  CLP_ClaimPaymentInformation : Segment
    {
          public CLP_ClaimPaymentInformation() : base("CLP") { }
        public string Claim_Submitters_Identifier;
        public string Claim_Status_Code;
        public decimal Monetary_Amount;
        public decimal Monetary_Amount2;
        public decimal Monetary_Amount3;
        public string Claim_Filing_Indicator_Code;
        public string Reference_Identification;
        public string Facility_Code_Value;
        public string Claim_Frequency_Type_Code;
        public string Patient_Status_Code;
        public string Diagnosis_Related_Group_DRG_Code;
        public string Quantity;
        public decimal Percentage_as_Decimal;
        public string Yes_No_Condition_or_Response_Code;
    }
 
       public class CAS_Adjustment : Segment
   {
           public CAS_Adjustment() : base("CAS") { }
       public string Claim_Adjustment_Group_Code;
        public  ClaimAdjustment ClaimAdjustment;
        public List<ClaimAdjustment>  ClaimAdjustments;
    }

    public class ClaimAdjustment
    {
        public string Claim_Adjustment_Reason_Code;
        public decimal Monetary_Amount;
        public decimal? Quantity ;
    }

    public class NM1_SubLoop
    {
           public NM1_PartyName NM1_PatientName;
        public NM1_PartyName NM1_InsuredName;
        public NM1_PartyName NM1_CorrectedPatient_InsuredName;
        public NM1_PartyName NM1_ServiceProviderName;
        public NM1_PartyName NM1_CrossoverCarrierName;
        public NM1_PartyName NM1_CorrectedPriorityPayerName;
        public NM1_PartyName NM1_OtherSubscriberName;
    }

    public class NM1_PartyName : Segment
    {
           public NM1_PartyName() : base("NM1") { }
        public string Entity_Identifier_Code;
        public string Entity_Type_Qualifier;
        public string Name_Last_or_Organization_Name;
        public string Name_First;
        public string Name_Middle;
        public string Name_Prefix;
        public string Name_Suffix;
        public string Identification_Code_Qualifier;
        public string Identification_Code;
        public string Entity_Relationship_Code;
        public string Entity_Identifier_Code2;
        public string Name_Last_or_Organization_Name2;
    }

   public class MIA_InpatientAdjudicationInformation : Segment
   {
            public MIA_InpatientAdjudicationInformation() : base("MIA") { }
       public decimal Quantity;
        public decimal? Monetary_Amount;
        public decimal? Quantity2;
        public decimal? Monetary_Amount2;
        public string Reference_Identification;
         public List<decimal> Monetary_Amount3;
        public decimal? Quantity3;
         public List<decimal> Monetary_Amount4;
         public List<string> Reference_Identification2;
        public decimal? Monetary_Amount5;
  }
 
   public class MOA_OutpatientAdjudicationInformation  : Segment
   {
            public MOA_OutpatientAdjudicationInformation() : base("MOA") { }
       public int? Percentage_as_Decimal;
        public decimal? Monetary_Amount;
        public List<decimal> Reference_Identification;
        public List<decimal> Monetary_Amounts;
   }
 
    public class DTM_SubLoop
    {
          public List<DTM_Date> DTM_StatementFromorToDates ;
        public DTM_Date DTM_CoverageExpirationDate;
        public DTM_Date DTM_ClaimReceivedDate;
    }

     public class PER_ClaimContactInformation : Segment
     {
            public PER_ClaimContactInformation() : base("PER") { }
        public string Contact_Function_Code;
        public string Name;
        public string Communication_Number_Qualifier;
        public List<string> Communication_Number;
        public string Contact_Inquiry_Reference;
     }

    public class AMT_ClaimSupplementalInformation : Segment
    {
             public AMT_ClaimSupplementalInformation() : base("AMT") { }
       public string Amount_Qualifier_Code;
        public decimal Monetary_Amount;
        public string Credit_Debit_Flag_Code;
    }

    public class QTY_ClaimSupplementalInformationQuantity : Segment
    {
            public QTY_ClaimSupplementalInformationQuantity() : base("QTY") { }
        public string Quantity_Qualifier;
        public decimal Quantity;
        public string Description;
        public string Free_form_Information;
    }
 
    public class TS835_2110_Loop
    {
        public SVC_ServicePaymentInformation SVC_ServicePaymentInformation; 
        public List<DTM_Date> DTM_ServiceDate ;
        public List<CAS_Adjustment> CAS_ServiceAdjustment;
        public REF_SubLoop REF_SubLoop_3;
        public List<AMT_ServiceSupplementalAmount> AMT_ServiceSupplementalAmount;
        public List<QTY_ServiceSupplementalQuantity> QTY_ServiceSupplementalQuantity;
        public List<LQ_HealthCareRemarkCodes> LQ_HealthCareRemarkCodes;
    }
  
    public class SVC_ServicePaymentInformation  : Segment
    {
             public SVC_ServicePaymentInformation() : base("SVC") { }
        public string Qualifier;
        public decimal Monetary_Amount;
        public decimal Monetary_Amount2;
        public string Product_Service_ID;
        public decimal? Quantity;
        public string Description;
        public decimal? Quantity2;
    }
  
    public class AMT_ServiceSupplementalAmount : Segment
    {
             public AMT_ServiceSupplementalAmount() : base("AMT") { }
        public string Amount_Qualifier_Code;
        public decimal Monetary_Amount;
        public string Credit_Debit_Flag_Code;
    }
 
    public class QTY_ServiceSupplementalQuantity : Segment
    {
             public QTY_ServiceSupplementalQuantity() : base("QTY") { }
        public string Quantity_Qualifier;
        public decimal Quantity;
        public string Info;
        public string Description;
    }
 
    public class LQ_HealthCareRemarkCodes : Segment
    {
             public LQ_HealthCareRemarkCodes() : base("LQ") { }
        public string Code_List_Qualifier_Code;
        public string Industry_Code;
    }

    public class PLB_ProviderAdjustment : Segment
    {
              public PLB_ProviderAdjustment() : base("PLB") { }
               public string Reference_Identification;
        public string Date;
        public MonetaryAmountAdjustment MonetaryAmountAdjustment;
        public List<MonetaryAmountAdjustment> MonetaryAmountAdjustments ;
    }

    public class MonetaryAmountAdjustment
    {
        public string Qualifier;
        public decimal MonetaryAmount;
    }
    
}
