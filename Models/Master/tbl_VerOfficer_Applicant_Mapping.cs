using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_VerOfficer_Applicant_Mapping
    {
        [Key]
        public int VOApplicantMapID { get; set; }
        public int ApplicantId { get; set; }
        public int VerifiedOfficer { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? DocVeriFee { get; set; } 
        public int? DocVeriFeePymtStatus { get; set; }
        public string DocVeriFeeReceiptNumber { get; set; }
        public DateTime? DocVeriFeePymtDate { get; set; }
        public string Treasury_Receipt_No { get; set; }

    }
}
