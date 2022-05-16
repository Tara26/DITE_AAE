using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_Reservation
    {
        [Key]
        public int ResApplId { get; set; }
        public int ApplicantId { get; set; }
        public int ReservationId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }
}
