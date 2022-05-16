using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_ITI_Institute_Detail
    {
        [Key]
        public int ApplicantITIInstituteId { get; set; }
        public int ApplicationId { get; set; }
        public int? ColumnCheck { get; set; }
        public int AdmittedStatus { get; set; }
        public string Remarks { get; set; }
        public int? DualType { get; set; }
        public int? TraineeType { get; set; }
        public int? ITIUnderPPP { get; set; }
        public DateTime? AdmisionTime { get; set; }
        public int? AdmisionFee { get; set; }
        public int? PaymentInd { get; set; }
        public int? AdmFeePaidStatus { get; set; }
        public bool IsActive { get; set; }
        
        public string ReceiptNumber { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public string AdmissionRegistrationNumber { get; set; }
        public string StateRegistrationNumber { get; set; }
        public string RollNumber { get; set; }
        public DateTime? PaymentDate { get; set; }

        public int? FlowId { get; set; }
        public int? ApplInstiStatus { get; set; }
        public int? CreatedBy { get; set; }
        public int? AllocationId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? AdmissionTypeID { get; set; }
        public int? Shiftid { get; set; }
        public int? Unitid { get; set; }
        public string TreasuryReceiptNumber { get; set; }

    }
}
