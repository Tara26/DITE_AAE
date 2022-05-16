using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class tbl_ITI_Trade_History
    {
        [Key]
        public int Trade_ITI_His_id { get; set; }
        public int Trade_ITI_id { get; set; }
        public int TradeCode { get; set; }
        public int? ITICode { get; set; }
        public int? StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? Unit { get; set; }
        public int? FlowId { get; set; }
        public bool IsActive { get; set; }
        public string FileUploadPath { get; set; }
    }
}
