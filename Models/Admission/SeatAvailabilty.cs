using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class SeatAvailabilty
    {
        public int Slno { get; set; }
        public string MISCode { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? TalukId { get; set; }
        public string TalukName { get; set; }
        public int? ITIType { get; set; }
        public string ITITypeName { get; set; }
        public string ITIName{ get; set; }
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int Shift { get; set; }
        public int Unit { get; set; }
        public int EligibleForCurrentAdmission { get; set; }
        public int SeatType { get; set; }
        public int? GovtSeatAvailability { get; set; }
        public int? ManagementSeatAvailability { get; set; }
        public string DualSystemTraining { get; set; }
        public string Duration { get; set; }
        public bool DualSystem { get; set; }
        public int UploadDocuments { get; set; }
        public int StatusId { get; set; }
        public int SeatTypeId { get; set; }
        public string SeatTypeName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int RoleId { get; set; }
        public int Id { get; set; }
        public int FlowId { get; set; }
        public int CourseType { get; set; }
        public string CourseTypeName { get; set; }
        public DateTime AcademicYear { get; set; }
        public string AcademicYearString { get; set; }
        public string SeatName { get; set; }
        public DateTime? AffilationDate { get; set; }
        public int? batchsize { get; set; }
        public bool IsActive { get; set; }
        public int TradeItiId { get; set; }     
        public int Govt_seats { get; set; }    
        public int Management_seats { get; set; }    

    }

    public class SeatDetails
    {
        public string MISCode { get; set; }
        public int Shift { get; set; }
        public int Unit { get; set; }
        public int SeatType { get; set; }
        public int GovtSeatAvailability { get; set; }
        public int ManagementSeatAvailability { get; set; }
        public string DualSystemTraining { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public int RoleId { get; set; }
        public int CourseType { get; set; }
        public DateTime AcademicYear { get; set; }
        public int TradeId { get; set; }
        public bool IsChecked { get; set; }
        public string Designation { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? FlowId { get; set; }
        public int seatId { get; set; }

    }
}
