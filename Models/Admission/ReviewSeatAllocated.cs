using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
   public class ReviewSeatAllocated
    {
        public string FormwardFrom { get; set; }
        public int slno { get; set; }
        //Application Type
        public int ApplicantTypeId { get; set; }
        public string ApplicantTypeDdl { get; set; }

        //Institute Type
        public int Institute_typeId { get; set; }
        public string InstituteType { get; set; }

        // Status 

        public int StatusId { get; set; }
        public string StatusName { get; set; }


        public int ExamYear { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Rules_allocation_masterid { get; set; }

        //tbl_SeatAllocation_SeatMatrix
        public int AllocationId { get; set; }

        public int AcademicYear { get; set; }

        public int ApplicantType { get; set; }

        public int Round { get; set; }

        public int Status { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CourseTypeId { get; set; }

      

    }
}
