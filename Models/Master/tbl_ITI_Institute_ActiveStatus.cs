using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
  public  class tbl_ITI_Institute_ActiveStatus
    {
        [Key]
        public int ActiveITIId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int ITI_Institute_Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovalFlowId { get; set; }
        public string ActDeActRemarks { get; set; }
        public string AffiliateFilePath { get; set; }
        public string AffiliateFileName { get; set; }


    }
}
