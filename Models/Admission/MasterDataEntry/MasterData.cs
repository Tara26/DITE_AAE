using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission.MasterDataEntry
{
    public class MasterData
    {
        public string MISCode { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string Talukname { get; set; }
        public int TalukCode { get; set; }
        public string InstituteType { get; set; }
        public int InstituteId { get; set; }
        public string InstituteName { get; set; }
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        public int SeatsPerUnit { get; set; }
        public int? Unit { get; set; }
        public int Shifts { get; set; }
        public int PPPModel { get; set; }
        public int? GovtGIASeats { get; set; }
        public int? PPPSeats { get; set; }
        public int DualSystemTraining { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string ISPPP { get; set; }
        public string PrivateSeats { get; set; }
    }

    public class CourseMaster
    {
        public int CourseId { get; set; }
        public string CourseTypeName { get; set; }
    }
}
