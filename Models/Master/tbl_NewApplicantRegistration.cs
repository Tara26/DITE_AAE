using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
public class tbl_NewApplicantRegistration
	{   
    [Key]
    public int ID { get; set; }
    public string CandidateID { get; set; }
   
    public string ApplicantMobileNumber { get; set; }

    public string ApplicantName { get; set; }

    public string ApplicantEmail { get; set; }

    public int ApplicantOTP { get; set; }

    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
  }
}
