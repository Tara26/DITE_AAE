using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class AdmissionRegister
    {
        public int slno { get; set; }

        //tbl_Applicant_details table
        public int ApplicationId { get; set; }
        public bool PhysicallyHanidcapInd { get; set; }
        public int? PhysicallyHanidcapType { get; set; }
        public bool RStateBoardType { get; set; }
        public int RAppBasics { get; set; }
        public string RollNumber { get; set; }
        public string ApplicantNumber { get; set; }
        public string ApplicantName { get; set; }
        public string FathersName { get; set; }
        public string ParentsOccupation { get; set; }
        public string Photo { get; set; }
        public DateTime? DOB { get; set; }
        public int? GenderId { get; set; }
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
        public decimal MarksObtained { get; set; }
        public decimal Percentage { get; set; }
        public int? ResultQual { get; set; }
        public string CommunicationAddress { get; set; }
        public int? DistrictId { get; set; }
        public int? TalukaId { get; set; }
        public int? Pincode { get; set; }
        public bool SameAdd { get; set; }
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
        public int CredatedBy { get; set; }
        public int? ApplyMonth { get; set; }
        public int? ApplyYear { get; set; }
        public int? ApplStatus { get; set; }
        public string ApplRemarks { get; set; }
        public int? DocVeriInstituteFilter { get; set; }
        public string DocVeriInstituteDistrict { get; set; }
        public bool PaymentOptionval { get; set; }
        public bool studiedMathsScience { get; set; }
        public bool ParticipateNextRound { get; set; }
        public string DocumentFeeReceiptDetails { get; set; }
        public string RationCard { get; set; }
        public string AadhaarNumber { get; set; }
        public string EducationGrade { get; set; }




        public bool? HoraNadu_GadiNadu_Kannidagas { get; set; }
        public bool? ExemptedFromStudyCertificate { get; set; }
        public bool? HyderabadKarnatakaRegion { get; set; }
        public bool? KanndaMedium { get; set; }

        //tbl_GradationRank_Trans
        public int Gradation_trans_Id { get; set; }
        public int ApplicantId { get; set; }
        public DateTime TransDate { get; set; }
        public int Rank { get; set; }
        public bool Tentative { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }      
        public bool Final { get; set; }       
        public int CreatedBy { get; set; }
        public decimal? Weitage { get; set; }
        public decimal? Total { get; set; }

        //tbl_district_master
        public int district_lgd_code { get; set; }
        public string districtename { get; set; }
        public int? division_id { get; set; }
        public string dist_name { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }

        //tbl_iti_college_details
        public string MISCode { get; set; }
        public string iticollegename { get; set; }
        //tbl_division_mast       
        public string divisionname { get; set; }


        //tbl_taluk_master
        public int taluk_lgd_code { get; set; }
        public int taluk_id { get; set; }
        public string talukename { get; set; }
        public string taluk_kname { get; set; }

        //tbl_Institute_type
        public int Institute_type_id { get; set; }
        public int? Institutetypeid { get; set; }
        public string Institutetype { get; set; }

        public string Gendername { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string Qualify { get; set; }

        public string StateRegistrationNumber { get; set; }
        public string dateofbirth { get; set; }
        public bool? DiffAbled { get; set; }
        public string DiffrentAbled { get; set; }
        public string ExService { get; set; }
        public string EconomicWeekerSec { get; set; }
        public string TraineePhoto { get; set; }
        public string TraineeName { get; set; }
        public DateTime? AdmisionTime { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string AdmisionDateTime { get; set; }
        public string PaymentDateReceipt { get; set; }
        public int? AdmisionFee { get; set; }
        public string ReceiptNumber { get; set; }
        public int? ITIUnderPPP { get; set; }
        public int? TraineeType { get; set; }
        public int? DualType { get; set; }
        public int? AdmFeePaidStatus { get; set; }
        public string Feestatus { get; set; }
        public int Seat_type_id { get; set; }
        public string SeatType { get; set; }
        public string tradename { get; set; }
        public string shifts { get; set; }
        public string units { get; set; }
        public string DisabilityName { get; set; }
        public string Minority { get; set; }

        public string district_ename { get; set; }
        public int? district_id { get; set; }
        public int CasteId { get; set; }
        public string CasteCategory { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public int ApplicantAdmissionRoundsId { get; set; }
        public string RoundList { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; }
        public decimal? MinMarks { get; set; }
        public string Trainee { get; set; }
        public string Traineeisindual  { get; set; }
        public string AcademicYearString { get; set; }
        public DateTime AcademicYear { get; set; }
        public string DocTypeIncome { get; set; }
    }
}
