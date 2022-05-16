using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models.Admin
{
    public class clsEditProfile
    {
        public clsEditProfile()
        {
            EmployeeGenderList = new List<SelectListItem>();
        }
        public int sino { get; set; }
        public int userId { get; set; }

        [Display(Name = "KGID Number")]
        [Required(ErrorMessage = "Please enter the KGID no")]
        [Remote("IsKGIDNumberExist", "Admin", HttpMethod = "POST", ErrorMessage = "KGID Number already exists in database.")]
        
        public string EmployeeKGIDNumber { get; set; }
        public bool DITENONDITE { get; set; }


        public int? KGIDNumber { get; set; }

        public string Name { get; set; }
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please Enter the User Name")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Please Enter the Father Name")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(50)]

        public string FatherName { get; set; }
        [Remote("IsEmailExist", "Admin", HttpMethod = "POST", ErrorMessage = "Email ID already exists in database.")]

        [Display(Name = "User Email")]
        [Required(ErrorMessage = "Please enter the Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter correct Email address")]
        public string Email { get; set; }



        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Please Enter the Designation")]
        [RegularExpression(@"[.a-zA-Z ]*$", ErrorMessage = "Please enter alphabets only.")]
        [StringLength(50)]


        public string Designation { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please Select the Gender")]

        public string Gender { get; set; }
        [Remote("IsMobileNumberExist", "Admin", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists in database.")]

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter the mobile no")]
        [StringLength(10, ErrorMessage = "Mobile number is invalid", MinimumLength = 10)]

        public string Mobile { get; set; }
        public string DOB { get; set; }
        public bool? status { get; set; }
        public string Photo { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        [DateRange("01/01/1960", ErrorMessage = "{0} Must be between {1} and Current Date")]
        //[MinimumAge(18)]
        [Required(ErrorMessage = "Date Of Birth is required")]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EmployeeDOB { get; set; }

        public IEnumerable<SelectListItem> EmployeeGenderList { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files1 { get; set; }

    }
}

//public class MinimumAgeAttribute : ValidationAttribute
//{
//    int _minimumAge;

//    public MinimumAgeAttribute(int minimumAge)
//    {
//        _minimumAge = minimumAge;
//    }

//    public override bool IsValid(object value)
//    {
//        DateTime date;
//        if (DateTime.TryParse(value.ToString(), out date))
//        {
//            return date.AddYears(_minimumAge) < DateTime.Now;
//        }

//        return false;
//    }
//}