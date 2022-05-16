using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class ApplicantTransferModel
    {
        public int Slno { get; set; }
        public int? Session { get; set; }
        public string ApplicantNumber { get; set; }
        public int AdmitId { get; set; }
        public int ApplicantId { get; set; }
        public string AdmisRegiNumber { get; set; }
        public string ApplicantName { get; set; }
        public string MISCode { get; set; }
        public int InstituteTypeId { get; set; }
        public string InstituteType { get; set; }
        public string InstituteName{ get; set; }
        public string CourseType { get; set; }
        public string TradeName { get; set; }
        public int Unit { get; set; }
        public int Shift { get; set; }
        public int TradeId { get; set; }
        public string DualSystem { get; set; }
        public int AdmissionStatus { get; set; }
        public string AdmissionStatusName { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public string From { get; set; }
        public string Date { get; set; }
        public int FlowId { get; set; }
        public string To { get; set; }
        public int StatusId { get; set; }
        public int RoleId { get; set; }
        public int CourseTypeId { get; set; }
        public int ApplicantInstituId { get; set; }
        public int DivisionId{ get; set; }
        public string DivisionName { get; set; }
        public int DistrictId { get; set; }
        public int DivisionIdEdit { get; set; }
        public string DistrictName { get; set; }
        public int TalukId { get; set; }
        public string TalukName { get; set; }
        public int TalukLgdCode { get; set; }
        public int DistrictLgdCode { get; set; }
        public bool Select { get; set; }
        public string TradeDuration { get; set; }
        public int? ApplyMonth { get; set; }
        public int statuswith { get; set; }
        public string YearSession { get; set; }
    }

    public class Trades
    {
        public int TradeId { get; set; }
        public string TradeName { get; set; }
        
    }
    public class TradeCodes
    {
        public int? TradeCode { get; set; }
        public int Trade_ITI_Id { get; set; }
    }
}
