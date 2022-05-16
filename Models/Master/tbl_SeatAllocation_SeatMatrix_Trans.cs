using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_SeatAllocation_SeatMatrix_Trans
    {
        [Key]
        public int AllocationTransId { get; set; }
        public int AllocationId { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public int FlowId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Remarks { get; set; }
    }
}
