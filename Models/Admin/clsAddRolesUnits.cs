using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admin
{
    public class clsAddRolesUnits
    {
        public int RoleId { get; set; }
        public int RoleLevel { get; set; }
        public int? RoleSeniorityLevel { get; set; }
        public List<int> lstRoleLevel { get; set; }
        public string RoleName { get; set; }
        public List<int> MultiSelectUnitList { get; set; }
        public string message { get; set; }
    }
}
