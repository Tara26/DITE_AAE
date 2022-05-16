using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_ITI_Institute_Detail_Trans
    {
        [Key]
        public int ApplicantITIInstituteTransId { get; set; }
        public int ApplicantITIInstituteId { get; set; }
        public int ApplicationId { get; set; }
        public int AdmittedStatus { get; set; }
        public int? FlowId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ApplInstiStatus { get; set; }
        public int? AllocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}
