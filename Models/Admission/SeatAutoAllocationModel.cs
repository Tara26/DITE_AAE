using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class SeatAutoAllocationModel
    {
        public int Slno { get; set; }
        public int? Rank { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string MISCode { get; set; }
        public string ITIType { get; set; }
        public string ITIName { get; set; }
        public string TradeName { get; set; }        
        public string AllottedCategory { get; set; }        
        public string AllottedGroup { get; set; }
        public string LocalStatus { get; set; }
        public int? PreferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string Remarks { get; set; }
        public int seatAllocDetailId { get; set; }
        public int? FlowId { get; set; }
        public string CourseType { get; set; }
        public string ApplicantType { get; set; }
        public int AcademicYear { get; set; }
        public string AcademicYearSm { get; set; }
        public int Round { get; set; }
        public string RoundSm { get; set; }
        public string Status { get; set; }
        public int AllocationId { get; set; }
        public int RankNumber { get; set; }
        public int division_id { get; set; }
        public int district_id { get; set; }
        public int TradeCode { get; set; }
        public string division_name { get; set; }
        public string district_ename { get; set; }
        public string InstituteType { get; set; }
        public string InstituteName { get; set; }
        public string UnitsDet { get; set; }
        public string ShiftsDet { get; set; }
        public string userRole { get; set; }
        public int? UnitId { get; set; }
        public int? ShiftId { get; set; }
        public int? TalukId { get; set; }
        public string TalukName { get; set; }
        public string DateOfBirth { get; set; }
    }
}
