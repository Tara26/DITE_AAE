using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ApplicantTrans
    {
        [Key]
        public int ApplicantTransId { get; set; }
        public int ApplicantId { get; set; }
        public int? VerfOfficer { get; set; }
        public DateTime TransDate { get; set; }
        public int IsActive { get; set; }
        public int? Status { get; set; }
        public int DocumentWiseStatus { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
        public int DocumentName { get; set; }
        public string DocumentWiseRemarks { get; set; }
        public int? CreatedBy { get; set; }
        public int? FlowId { get; set; }
        public int? AssignedVO { get; set; }
        public int? ApplDescStatus { get; set; }
        public bool ReVerficationStatus { get; set; } 
        public int? FinalSubmitInd { get; set; } 
    }
}
