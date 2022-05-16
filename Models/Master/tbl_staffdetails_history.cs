using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_staffdetails_history
    {
        [Key]
        public int ID { get; set; }
        public int Institute_id { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApprovalFlowId { get; set; }
        public string Remarks { get; set; }
        public DateTime? Created_On { get; set; }
        public int CreatedBy { get; set; }
        public int? Yearid { get; set; }
        public int? Quarter { get; set; }
    }
}
