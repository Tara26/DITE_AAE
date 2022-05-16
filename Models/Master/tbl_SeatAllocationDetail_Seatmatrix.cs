using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_SeatAllocationDetail_Seatmatrix
    {
        [Key]
        public int AllocationDetailId { get; set; }
        public int AllocationId { get; set; }
        public int RankNumber { get; set; }
        public int ApplicantId { get; set; }
        public int InstituteId { get; set; }
        public int TradeId { get; set; }
        public int? HorizontalId { get; set; }
        public int? VerticalId { get; set; }
        public int? HyrNonHydrId { get; set; }
        public int? ShiftId { get; set; }
        public int? UnitId { get; set; }
        public int? PreferenceNum { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? FlowId { get; set; }
        public int? InstitutePreferenceId { get; set; }
        public string AllocByCategory { get; set; }
    }
}
