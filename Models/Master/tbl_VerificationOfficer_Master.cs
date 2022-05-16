using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_VerificationOfficer_Master
    {
        [Key]
        public int Officer_Id { get; set;}
        public string Name { get; set; }
        public string OfficerLoginUserName { get; set; }
        public string OfficerLoginPwd { get; set; }
        public string Designation { get; set; }
        public string MobileNum { get; set; }
        public string EmailId { get; set; }
        public int InstituteId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? UserMasterId { get; set; }
        public int? OfficerRoleId { get; set; }
    }
}
