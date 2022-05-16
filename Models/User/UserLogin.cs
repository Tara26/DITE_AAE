using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
    public class UserLogin
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string userFatherName { get; set; }
        public string Password { get; set; }
        public string userRole { get; set; }
        public int loginId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int SubDeptCount { get; set; }
        public int? SubDeptId { get; set; }
        public int? KGIDNum { get; set; }
        public int RoleCount { get; set; }
        public string HDNpwd { get; set; }
        public bool user_is_active { get; set; }
    }
}
