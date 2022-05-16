using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class ApplicationForm
    {
        public int iti_college_code { get; set; }
        public int UnitsId { get; set; }
        public string UnitsValue { get; set; }
        public int ShiftId { get; set; }
        public string ShiftValue { get; set; }
        public string iti_college_name { get; set; }
        public string iti_MISCode { get; set; }
        public string course_type_name { get; set; }
        public int trade_id { get; set; }
        public string trade_name { get; set; }
        public int Religion_Id { get; set; }
        public int division_id { get; set; }
        public int division_name { get; set; }
        public string Religion { get; set; }
        public int Gender_Id { get; set; }
        public string Gender { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int CasteId { get; set; }
        public string Caste { get; set; }
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public int ApplicantTypeId { get; set; }
        public string ApplicantType { get; set; }
        public int QualificationId { get; set; }
        public string Qualification { get; set; }
        public int ReservationId { get; set; }
        public string Reservations { get; set; }
        public int Syllabus_type_id { get; set; }
        public string Syllabus_type { get; set; }
        public int district_lgd_code { get; set; }
        public string district_ename { get; set; }
        public int taluk_lgd_code { get; set; }
        public string taluk_ename { get; set; }
        public int location_id { get; set; }
        public string location_name { get; set; }
        public int course_Id { get; set; }
        //public List<tbl_oth>

        public string SelectedReservationId { get; set; }
        public List<ApplicationForm> TalukListDet { get; set; }
        public List<ApplicationForm> DocVerfiInstDet { get; set; }

        public int ApplDocVerifiID { get; set; }
        public string VerificationStatus { get; set; }
        public bool IsActive { get; set; }
        public int stateId { get; set; }
        public string NameOfState { get; set; }
    }

    public class UploadPreferenceType
    {
        public int flag { get; set; }
        public string status { get; set; }
    }

    public class PersonWithDisabilityCategory
    {
        public int PersonWithDisabilityCategoryId { get; set; }
        public string DisabilityName { get; set; }
    }
}
