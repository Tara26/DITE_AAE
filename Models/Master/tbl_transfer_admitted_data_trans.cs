using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_transfer_admitted_data_trans
    {
        [Key]
        public int Trans_Seat_Id { get; set; }
        public int Admit_Id { get; set; }
        public int ApplicantITIInstituteId { get; set; }
        public int ApplicantId { get; set; } 
        public DateTime Trans_Date { get; set; }
        public int FlowId { get; set; }
        public int Status { get; set; }
        public string AdmissionRegistrationNumber { get; set; }
        public bool DualSystemTraining { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Trade { get; set; }
        public int Unit { get; set; }
        public int Shift { get; set; }
    }
}
