using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class InstituteWiseAdmission
    {
        public int ApplicantITIInstituteId { get; set; }
        public int? ColumnCheck { get; set; }
        
        public int ApplicationId { get; set; }
        public int AdmittedStatus { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public string RoundList { get; set; }
        public int ApplicantAdmissionRoundsId { get; set; }   
        
        public string ApplicantNumber { get; set; }
        public string ApplicantName { get; set; }
        public string DivisionName { get; set; }
        public string InstituteName { get; set; }
        public string MISCode { get; set; }
        public int ApplicantRank { get; set; }
        public int AdmissionStatus { get; set; }
        public string AdmittedStatusEx { get; set; }
        public int? ApplInstiStatus { get; set; }
        public string ApplInstiStatusEx { get; set; }
        public string AddRemark { get; set; }
        public string checkboxes_value { get; set; }
        public int CourseType { get; set; }
        public string CourseTypeName { get; set; }
        public int slno { get; set; }
        public string DistrictName { get; set; }
        public string TalukName { get; set; }
        public string InstituteType { get; set; }
        public string VerticalCategory { get; set; }
        public string HorizontalCategory { get; set; }
        public int TradeId { get; set; }
        public string TradeCode { get; set; }
        public string TradeName { get; set; }
        public string Units { get; set; }
        public string Shifts { get; set; }
        public int? DualType { get; set; }
        public int? TraineeTypeId { get; set; }
        public string ApplicantType { get; set; }
        public string TraineeType { get; set; }
        public int? ITIUnderPPP { get; set; }
        public DateTime? AdmisionTime { get; set; }
        public int? AdmisionFee { get; set; }
        public int? PaymentInd { get; set; }
        public int? AdmFeePaidStatus { get; set; }
        public int PaymentAmount { get; set; }
        public int ClickToPay { get; set; }
        public int DivisionId { get; set; }
        public int DistrictId { get; set; }
        public int? Gender { get; set; }
        public DateTime? DOB { get; set; }

        public string ReceiptNumber { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string AdmissionRegistrationNumber { get; set; }
        public string StateRegistrationNumber { get; set; }
        public string UpdateMsg { get; set; }
        public int? TalukId { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string RollNumber { get; set; }
        public int? FlowId { get; set; }

        public string ForwardedTo { get; set; }
        public string userRole { get; set; }
        public string CreatedOn { get; set; }
        public string StatusName { get; set; }
        public int UserLoginId { get; set; }
        public int? division_id { get; set; }
        public int? Insitute_TypeId { get; set; }
        public int? AdmissionTypeID { get; set; }
        public int? Shiftid { get; set; }
        public int? Unitid { get; set; }
        public int? AllocationId { get; set; }
        public string GenderName { get; set; }
        public string DateOfBirth { get; set; }
        public string AdmTime { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string AadharNumber { get; set; }
        public string ReligionName { get; set; }
        public int? MinorityCategory { get; set; }
        public string CategoryName { get; set; }
        public string Qualification { get; set; }
        public string RationCardNo { get; set; }
        public string IncomeCertificateNo { get; set; }
        public string CasteCertNum { get; set; }
        public string AccountNumber { get; set; }  
        public string TradeDuration { get; set; }
        public int? ApplyYear { get; set; }
        public int? ApplyMonth { get; set; }
        //newly added field for display AUG-Yearname
        public string Session { get; set; }
        public string ApplicationMode { get; set; }
        public string OfficerName { get; set; }

    }
}
