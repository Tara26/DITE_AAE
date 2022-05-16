using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_transfer_admitted_data
    {
        [Key]
        public int Admit_Id { get; set; }        
        public int ApplicantITIInstituteId { get; set; }
        public int ApplicantId { get; set; }
        public int FlowId { get; set; }
        public int Status { get; set; }   
        public string AdmissionRegistrationNumber { get; set; }       
        public string DualSystemTraining { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int Trade { get; set; }
        public int Unit { get; set; }
        public int Shift { get; set; }
    }
}
