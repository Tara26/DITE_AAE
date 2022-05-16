using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Admission
{
    public class NewApplicant
    {
        public int ID { get; set; }
        public string RegistrationNumber { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter the mobile no")]
        [StringLength(10, ErrorMessage = "Mobile number is invalid", MinimumLength = 10)]
        [Remote("IsMobileNumberExist", "Home", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists in database.")]

        public string MobileNumber { get; set; }


       

        [Display(Name = "Applicant Name")]
        [Required(ErrorMessage = "Please Enter the Applicant Name")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter correct Email address")]
        [Remote("IsEmailExist", "Home", HttpMethod = "POST", ErrorMessage = "EmailID already exists in database.")]

        public string Email { get; set; }


        [Display(Name = "Mobile OTP")]
        [Required(ErrorMessage = "Please enter Mobile OTP")]
        public int MobileOTP { get; set; }

        [Display(Name = "Email OTP")]
        [Required(ErrorMessage = "Please enter Email OTP")]
        public int EmailOTP { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(15)]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please enter Confirm Password")]
        [StringLength(15)]
        public string ConfirmNewPassword { get; set; }
        public string hdnValue { get; set; }
        public string pwdMessage { get; set; }
        public string pwdPattern { get; set; }

    }
    public class Response
    {
        public string message_id { get; set; }
        public int message_count { get; set; }
        public double price { get; set; }
    }
    public class RootObject
    {
        public Response Response { get; set; }
        public string ErrorMessage { get; set; }
        public int Status { get; set; }
    }
  
}
