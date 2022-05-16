using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admin
{
    public class clsAddUserMapping
    {
        public int DiteNonDiteEE { get; set; }
        public int userId { get; set; }
        public int userMapId { get; set; }
        public string KGIDUserID { get; set; }
        public string EEName { get; set; }
        public string EEFatherName { get; set; }
        public string phoneNum { get; set; }
        public string email { get; set; }
        public string pwd { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DOB { get; set; }
        public string DateOfBirth { get; set; }
        public List<clsRoleMapping> lstRoleMapping { get; set; }
        public string message { get; set; }
        public string btnValue { get; set; }


        public int um_gender { get; set; }

    }
}
