using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admin
{
    public class clsRoleMapping
    {
        public int UnitId { get; set; }
        public int WingId { get; set; }
        public int LocationDivId { get; set; }
        public int SectionId { get; set; }
        public int DistrictId { get; set; }
        public int TalukId { get; set; }
        public int InstituteId { get; set; }
        public int RoleId { get; set; }
        public int rmId { get; set; }

        public string UnitName { get; set; }
        public string WingName { get; set; }
        public string LocationDivName { get; set; }
        public string SectionName { get; set; }
        public string RoleName { get; set; }

        public List<clsMenuList> lstMenuList { get; set; }
        public bool IsActive { get; set; }
        public string message { get; set; }
    }

    public class clsMenuList
    {
        public int MenuId { get; set; }
        public int SubMenuId { get; set; }
        public string MenuName { get; set; }
        public string SubMenuName { get; set; }
        public string SubSubMenuName { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
    }
}
