using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
   public class tbl_ITI_Trade_ActiveStatus_History
    {
        [Key]
        public int ActiveITI_HisId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int ITI_Trade_Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string ActDeActRemarks { get; set; }
    }
}
