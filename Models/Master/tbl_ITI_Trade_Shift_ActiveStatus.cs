using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
  public  class tbl_ITI_Trade_Shift_ActiveStatus
    {
        [Key]
        public int ITI_Trade_Shift_Active_Id { get; set; }
        public int ITI_Trade_Shift_Id { get; set; }
        public int ITI_Trade_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public int ApprovalStatus { get; set; }
        public int ApprovalFlowId { get; set; }
        public string ActDeActRemarks { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public string DeactivateOrderNo { get; set; }
        public Nullable<DateTime> DeactivateDate { get; set; }

        public Nullable<DateTime> ActivateDate { get; set; }
        public string ActivateOrderNo { get; set; }
        public string ActivateFileName { get; set; }
        public string ActivateFilePath { get; set; }
    }
}
