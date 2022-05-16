using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class SeatMatrix
    {
        public int AcademicYear { get; set; }       
        public int ApplicantType { get; set; }
        public int Round { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }       
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int CourseTypeId { get; set; }
        public int? FlowId { get; set; }
    }
}
