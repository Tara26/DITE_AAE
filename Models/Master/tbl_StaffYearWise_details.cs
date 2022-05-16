using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_StaffYearWise_details
    {
        [Key]
        public long Sno { get; set; }
        public int InstituteId { get; set; }
        public int? Year_ID { get; set; }
        public int Staff_ID { get; set; }
        public long? Created_By { get; set; }
        public long? Modified_By { get; set; }
        public int? ApprovalStatus { get; set; }
        public int? ApprovalFlowId { get; set; }
        public string Remarks { get; set; }
        public DateTime? Created_On { get; set; }
        public DateTime? Modified_On { get; set; }
        public int? Quarter { get; set; }
    }
}
