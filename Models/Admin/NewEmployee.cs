using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Admin
{
    public class NewEmployee
    {
        public NewEmployee()
        {
            EmployeeGenderList = new List<SelectListItem>();
        }
        public int EmployeeID { get; set; }    // Do not change the name of the property as it is used for validation

        public string EmpRegistrationNumber { get; set; }

        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Please Enter the Employee Name")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(30)]
        public string Name { get; set; }

        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Please Enter the Father Name")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(30)]
        public string EmployeeFatherName { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please Select the Gender")]
        public int EmployeeGender { get; set; }
        public IEnumerable<SelectListItem> EmployeeGenderList { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please Select the Date Of Birth")]
        [DateRange("1960/01/01", ErrorMessage = "{0} Must be between {1} and Current Date")]
        public DateTime EmployeeDOB { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter the mobile no")]
        [StringLength(10, ErrorMessage = "Mobile number is invalid", MinimumLength = 10)]
        //[Remote("IsMobileNumberExist", "Home", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists in database.")]

        public string MobileNumber { get; set; }

        [Display(Name = "Employee Email")]
        [Required(ErrorMessage = "Please enter the Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter correct Email address")]
        //[Remote("IsEmailExist", "Home", HttpMethod = "POST", ErrorMessage = "EmailID already exists in database.")]

        public string Email { get; set; }


        [Display(Name = "KGID Number")]
        [Required(ErrorMessage = "Please enter the KGID no")]
        public int EmployeeKGIDNumber { get; set; }

        [Display(Name = "MobileOTP")]
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
    public enum GenderType
    {
        Male = 1, Female = 2
    }

    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string minimumValue)
            : base(typeof(DateTime), minimumValue, DateTime.Now.ToShortDateString())
        {
        }
    }


    //public class ActionModel
    //{
    //    public ActionModel()
    //    {
    //        ActionsList = new List<SelectListItem>();
    //    }
    //    [Display(Name = "Gender")]
    //    public int ActionId { get; set; }
    //    public IEnumerable<SelectListItem> ActionsList { get; set; }
    //}
}
