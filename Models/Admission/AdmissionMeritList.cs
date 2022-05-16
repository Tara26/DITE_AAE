using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class AdmissionMeritList
    {

        public string tentativeDate { get; set; }
        public string finalDate { get; set; }
        public bool IsPPP { get; set; }
        public int slno { get; set; }
        public int age { get; set; }
        //User Role
        public int loginId { get; set; }
        public int user_id { get; set; }
        public string user_role { get; set; }
        public int RoleId { get; set; }
        public int SelectedRoleId { get; set; }
        public string user_name { get; set; }
        public string ApplicantTypes { get; set; }
        public string TentativeDescription { get; set; }
        //Applicant Table
        public int ApplicationId { get; set; }
        public string ApplicantNumber { get; set; }
        public string ApplicantName { get; set; }
        public string FathersName { get; set; }
        public string ParentsOccupation { get; set; }
        public string Photo { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public int? GenderId { get; set; }
        public string MothersName { get; set; }
        public int Religion { get; set; }
        public int? Category { get; set; }
        public int MinorityCategory { get; set; }
        public int Caste { get; set; }
        public decimal FamilyAnnIncome { get; set; }
        public int ApplicantType { get; set; }
        //public int ReservationsId { get; set; }
        public int Qualification { get; set; }
        public int ApplicantBelongTo { get; set; }
        public int AppliedBasic { get; set; }
        public int TenthBoard { get; set; }
        public string InstituteStudiedQual { get; set; }
        public decimal? MaxMarks { get; set; }

        public decimal? Percentage { get; set; }
        public int ResultQual { get; set; }
        public string CommunicationAddress { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public int Pincode { get; set; }
        public bool SameAdd { get; set; }
        public string PermanentAddress { get; set; }
        public int? PDistrict { get; set; }
        public int? PTaluk { get; set; }
        public int? PPinCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string EmailId { get; set; }
        // public int DocumentId { get; set; }
        public int? DocVerificationCentre { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CredatedBy { get; set; }
        public int? CreatedBy { get; set; }
        //Gradation Rank Trans
        public int Gradation_trans_Id { get; set; }
        public int ApplicantIdTrans { get; set; }
        public bool ParticipateNextRound { get; set; }
        public DateTime TransDate { get; set; }
        public int Rank { get; set; }
        public bool Tentative { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
        public bool Final { get; set; }

        //Application Type
        public int ApplicantTypeId { get; set; }
        public string ApplicantTypeDdl { get; set; }
        //Application Mode
        public int ApplicationModeId { get; set; }
        public string ApplicationMode { get; set; }


        //GradationType
        public int GradationTypeId { get; set; }
        public string GradationTypes { get; set; }

        //District Master
        public int district_id { get; set; }
        public string district_ename { get; set; }
        public string district_kname { get; set; }
        public int district_lgd_code { get; set; }
        public int? state_code { get; set; }
        public bool? dis_is_active { get; set; }
        public DateTime? dis_creation_datetime { get; set; }
        public DateTime? dis_updation_datetime { get; set; }
        public int? dis_created_by { get; set; }
        public int? dis_updated_by { get; set; }

        //Division Master
        public int division_id { get; set; }
        public string division_name { get; set; }
        public bool? division_is_active { get; set; }
        public DateTime? division_created_on { get; set; }
        public int? division_created_by { get; set; }
        public DateTime? division_updated_on { get; set; }
        public int? division_updated_by { get; set; }
        public string division_kannada_name { get; set; }
        public decimal? MarksObtained_1 { get; set; }

        //Gradation Result
        public int ResultId { get; set; }
        public string Result { get; set; }

        //Location
        public int locationId { get; set; }
        public string locationName { get; set; }

        //Round
        public int roundId { get; set; }
        public string roundName { get; set; }

        //Gradation History Table
        public int GradationtransHisId { get; set; }

        //Year Table
        public int YearID { get; set; }
        public string Year { get; set; }

        //Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        //view
        public int generateId { get; set; }
        public int applicantviewId { get; set; }
        public int divisionId { get; set; }
        public int districtIdview { get; set; }
        public int academicId { get; set; }

        //tbl_Applicant_Reservation
        public int ResApplId { get; set; }
        public int ApplicantId { get; set; }
        public int ReservationId { get; set; }


        public int? ApplyYear { get; set; }
        public string yearshow { get; set; }
        public string Qual { get; set; }
        //public int Exserviceman { get; set; }
        public bool? DiffAbled { get; set; }
        public string DiffrentAbled { get; set; }
        public bool? ExServiceMan { get; set; }
        public string ExService { get; set; }
        public bool? EcoWeakSection { get; set; }
        public string EconomicWeekerSec { get; set; }
        //public string EWS { get; set; }
        public decimal? Weitage { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal? Total { get; set; }
        public bool? HydKarRegion { get; set; }
        public bool? KMedium { get; set; }
        public string HyderabadKarnatakaRegion { get; set; }
        public string KanndaMedium { get; set; }
        public string comments { get; set; }
        public int login_id { get; set; }
        public string To { get; set; }
        //public string StatusName { get; set; }
        public string createdatetime { get; set; }
        public string FromUser { get; set; }

        public string dateofbirth { get; set; }
        public string roleName { get; set; }
        public DateTime? Dt_DisplayTentativeGradation { get; set; }
        public string Dt_DisplayTentativeGradationRE1 { get; set; }
    }

    public class nestedMeritList
    {
        public List<AdmissionMeritList> lists { get; set; }
    }
}
