using Models.Affiliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models.Affiliation
{
   public  class StaffInstituteDetails
    {
        public int? StaffId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public int InstituteId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? Designation { get; set; }
        public string Qualification { get; set; }
        public int? StaffType { get; set; }
        public int? TechingSubject { get; set; }
        public int? Trade { get; set; }
        public string EmailId { get; set; }
        public string MobileNum { get; set; }
        public int slno { get; set; }
        public string Type { get; set; }
        public string subject { get; set; }
        public string Tradename { get; set; }
        public string InstituteName { get; set; }
        public string DesignationName { get; set; }
        public int? Appeovalstatus { get; set; }
        public int? ApprovalFlowId { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public string Coursetype { get; set; }
        public string MIScode { get; set; }
        public string ITIinstitute { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Taluk { get; set; }
        public string location_type { get; set; }
        public string Year { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? StaffCount { get; set; }
        public int? Courseid { get; set; }
        public int? DivisionId { get; set; }
        public int? DistrictId { get; set; }
        public int? taluk_id { get; set; }
        public int? Insttype_Id { get; set; }
        public int? location_Id { get; set; }
        public int? scheme_Id { get; set; }
        public string staffsub { get; set; }
        public string selectstaffsub { get; set; }
        public string Date { get; set; }
        public string stafftrade { get; set; }
        public string selectstafftrade { get; set; }

        public int? YearId { get; set; }
        public int? GenderId { get; set; }
        public string Consoildatesubject { get; set; }
        public List<StaffInstituteDetails> stafflist { get; set; }
        public List<SelectListItem> DesignationList { get; set; }
        public List<SelectListItem> StafftypeList { get; set; }
        public List<SelectListItem> SubjectList { get; set; }
        public List<SelectListItem> TradeList { get; set; }
        public List<SelectListItem> GenderList { get; set; }
        public List<AffiliationCollegeCourseType> CourseList { get; set; }
        public List<int> MultiSelectSubjectList { get; set; }
        public List<int> MultiSelectTradeList { get; set; }
        public bool IsAction { get; set; }
        public string GenderName { get; set; }
        public string Scheme { get; set; }
        public string TotalExperience { get; set; }
        public string Other { get; set; }
        public string Photo { get; set; }
        public bool? CITS { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public int? Quarter { get; set; }
        public string InstitutionType { get; set; }

        public string TrainingMode { get; set; }

    }
}
