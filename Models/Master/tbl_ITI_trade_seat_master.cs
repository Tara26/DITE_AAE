using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ITI_trade_seat_master
    {
        [Key]
        public int Trade_ITI_seat_Id { get; set;}
        public int Trade_ITI_Id { get; set; }        
        public int UnitId { get; set; }
        public int ShiftId { get; set; }
        public int SeatsPerUnit { get; set; }
        public int SeatsTypeId { get; set; }
        public bool IsPPP { get; set; }
        public bool DualSystemTraining { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Govt_Gia_seats { get; set; }
        public int? PPP_seats { get; set; }
        public int? Management_seats { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
        public int CourseType { get; set; }
        public DateTime AcademicYear { get; set; }
        public bool IsActive { get; set; }
    }
}
