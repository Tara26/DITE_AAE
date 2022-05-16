using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_Detail
    {
        [Key]
        public int ApplicationId { get; set; }

        public string ApplicantNumber { get; set; }

        public string ApplicantName { get; set; }

        public string FathersName { get; set; }

        public string ParentsOccupation { get; set; }

        public string Photo { get; set; }

        public DateTime? DOB { get; set; }

        public int? Gender { get; set; }

        public string MothersName { get; set; }

        public int? Religion { get; set; }

        public int? Category { get; set; }

        public int? MinorityCategory { get; set; }

        public int? Caste { get; set; }

        public decimal? FamilyAnnIncome { get; set; }

        public int? ApplicantType { get; set; }

        public int? Qualification { get; set; }

        public int? ApplicantBelongTo { get; set; }

        public int? AppliedBasic { get; set; }

        public int? TenthBoard { get; set; }

        public string InstituteStudiedQual { get; set; }

        public decimal? MaxMarks { get; set; }

        public decimal? MarksObtained { get; set; }

        public decimal? Percentage { get; set; }

        public int? ResultQual { get; set; }

        public string CommunicationAddress { get; set; }

        public int? DistrictId { get; set; }

        public int? TalukaId { get; set; }

        public int? Pincode { get; set; }

        public bool? SameAdd { get; set; }

        public string PermanentAddress { get; set; }

        public int? PDistrict { get; set; }

        public int? PTaluk { get; set; }

        public int? PPinCode { get; set; }

        public string PhoneNumber { get; set; }

        public string FatherPhoneNumber { get; set; }

        public string EmailId { get; set; }

        public int? DocVerificationCentre { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? CredatedBy { get; set; }

        public int? DocVeriInstituteFilter { get; set; }

        public string DocVeriInstituteDistrict { get; set; }

        public int? ApplyMonth { get; set; }

        public int? ApplyYear { get; set; }

        public int? ApplStatus { get; set; }

        public string ApplRemarks { get; set; }

        public bool PaymentOptionval { get; set; }

        public string DocumentFeeReceiptDetails { get; set; }

        public bool RStateBoardType { get; set; }

        public int? RAppBasics { get; set; }

        public string RollNumber { get; set; }

        public bool PhysicallyHanidcapInd { get; set; }

        public int? PhysicallyHanidcapType { get; set; }

        public string RationCard { get; set; }

        public string AadhaarNumber { get; set; }

        public int? TenthCOBSEBoard { get; set; }

        public string EducationGrade { get; set; }

        public decimal? EducationCGPA { get; set; }

        public bool? HoraNadu_GadiNadu_Kannidagas { get; set; }

        public bool? ExemptedFromStudyCertificate { get; set; }

        public bool? HyderabadKarnatakaRegion { get; set; }

        public bool? KanndaMedium { get; set; }

        public bool studiedMathsScience { get; set; }

        public bool ParticipateNextRound { get; set; }

        public int? ApplDescStatus { get; set; }

        public int? ReVerficationStatus { get; set; }

        public int? FlowId { get; set; }

        public int? AssignedVO { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string IFSCCode { get; set; }

        public decimal? MinMarks { get; set; }

        public bool? ExServiceMan { get; set; }

        public bool? EconomyWeakerSection { get; set; }

        public int? AgainstVacancyInd { get; set; }

        public string CasteDetail { get; set; }

        public string CategoryName { get; set; }

        public int? BoardId { get; set; }

        public string ApplicantRegNo { get; set; }

        public string Caste_Categaory_Income_RD { get; set; }

        public string Economic_Weaker_Section_RD { get; set; }

        public string Hyderabada_Karnataka_Region_RD { get; set; }

        public string UDID_Number { get; set; }

        public int? ApplicationMode { get; set; }

    }
}
