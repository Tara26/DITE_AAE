using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_SeatMatrix_Trans
    {
        [Key]
        public int SeatMaxTransId { get; set; }
        public int SeatMaxId { get; set; }
        public int AcademicYear { get; set; }
        public int InstituteId { get; set; }
        public int ApplicantType { get; set; }
        public int Round { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int CourseTypeId { get; set; }
        public int? FlowId { get; set; }
    }
}
