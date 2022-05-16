using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class Staff_Institute_Detail
    {
        [Key]
        public int StaffId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public int InstituteId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? Designation { get; set; }
        public string Qualification { get; set; }
        public int? StaffType { get; set; }
        //public int? TechingSubject { get; set; }
        //public int? Trade { get; set; }
        public string EmailId { get; set; }
        public string MobileNum { get; set; }
        //public int? Approvalstatus { get; set; }
        //public int? ApprovalFlowId { get; set; }
        //public string Remarks { get; set; }
        //public int? Year_id { get; set; }
        public int? Coursetype_id { get; set; }
        public int? Gender_id { get; set; }
        public string TotalExperience { get; set; }
        public string Other { get; set; }
        public string Photo { get; set; }
        public bool? CITS { get; set; }
    }
}
