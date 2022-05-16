using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Admission
{
    public class VerificationOfficer
    {
        public int slno { get; set; }
        public int OfficerId { get; set; }
        public string OfficerName { get; set; }
        public string OfficerLoginUserName { get; set; }
        public string OfficerLoginPwd { get; set; }
        public string Designation { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string InstituteName { get; set; }
        public int InstituteId { get; set; }
        public bool Status { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string GrievanceRefNumber { get; set; }
        public int? Year { get; set; }
        public string CourseType { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantNumber { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public int TotalOfficers { get; set; }
        public int TotalApplicants { get; set; }
        public int InactiveOfficerApplicants { get; set; }
        public int InactiveOfficers { get; set; }

        public DateTime SubmitDate { get; set; }
        public string Remarks { get; set; }

        public string FatherName { get; set; }
        public DateTime? DOB { get; set; }
        public string Category { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal? MaxMarks { get; set; }
        public string Result { get; set; }
        public string RuralUrban { get; set; }
        public int? Rank { get; set; }
        public int docTypeId { get; set; }
        public string docType { get; set; }
        public string DocUploaded { get; set; }
        public int GrievanceId { get; set; }
        public int RoleId { get; set; }
        public List<string> FileNames { get; set; }
        public List<string> Files { get; set; }
        public List<int> FileTypes { get; set; }
        public List<Filetypes> Doctypes { get; set; }
        public List<string> DocNames { get; set; }
        public int ApplicationId { get; set; }
        public int? CredatedBy { get; set; }
        public int? ApplStatus { get; set; }
        public bool PaymentOptionval { get; set; }
        public List<string> FileStatus { get; set; }
        public bool? DiffAbled { get; set; }
        public string DiffrentAbled { get; set; }
        public string ExService { get; set; }
        public decimal? Percentage { get; set; }
        public string Qualification { get; set; }
        public decimal? Weightage { get; set; }
        public string EconomicWeekerSec { get; set; }
        public string DocumentFeeReceiptDetails { get; set; }
        public decimal? TotalMarks { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Datestring { get; set; }
        public int? ReVerficationStatus { get; set; }
        public int? AssignedVO { get; set; }
        public string HydKar { get; set; }
        public DateTime Applydate { get; set; }
        public string Apdate { get; set; }
        public int? DocVeriFees { get; set; }
        public string DocVeriFeeReceiptNumbers { get; set; }
        public DateTime? DocumentVeriFeePymtDate { get; set; }
        public int DocumentVeriFeePymtStatus { get; set; }
        public int? FlowId { get; set; }
        public string userRole { get; set; }
        public int? ApplyYear { get; set; }
        public int? ApplicantType { get; set; }
        public string Districtname { get; set; }
        public string Talukname { get; set; }

        public string Divisionname { get; set; }
        public int? KGIDNo { get; set; }
        //NEw field for AUG-2021
        public string Session { get; set; }
        // for mobile no in grid 
        public string MobileNumber { get; set; }
        public string MisCode { get; set; }

        public string Treasury_Receipt_No { get; set; }
        public int? ApplicationModeId { get; set; }
        public string ApplicationMode { get; set; }
        public int? district_lgd_code { get; set; }

        public int? taluk_lgd_code { get; set; }
        public int? division_id { get; set; }
        public string KannadaMedium { get; set; }
        
     public int? ApplDescStatus { get; set; }

    }

    public class Applicants
    {
        public int officer { get; set; }
        public int applicant { get; set; }
    } 
    
    public class Filetypes
    {
        public int DocTypeId { get; set; }
        public string DoctypeName { get; set; }
    }


}
