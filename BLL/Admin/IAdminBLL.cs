using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Admin;
using System.Web.Mvc;

namespace BLL.Admin
{
    public interface IAdminBLL
    {
        List<SelectListItem> GetUnitsListBLL();
        string GetUnitsListBLL(clsAddRolesUnits roles);
        string AddUserMappingBLL(List<clsAddUserMapping> lstUserMap);
        List<clsAddRolesUnits> GetRolesDataBLL(string userId, string value, int unitId, int sectionId);
        List<clsEditProfile> GetEditProfileDataBLL(int userId);
        string GetEditProfileDataBLL(clsRoleMapping clsRole, List<clsEditProfile> lstEditProfile, string path);
        List<string> GetEnteredRoleListBLL(string prefix);
        List<clsMenuList> GetRoleMapDataBLL(int val);
        string GetRoleMapDataBLL(List<clsRoleMapping> lstRoleMapping);
        List<clsHolidayList> GetHolidayListBLL(int id, string value);
        string GetHolidayListBLL(clsHolidayList clsHoliday);
        List<clsRoleMapping> GetRoleMapDetailsDataBLL(int id, string value, clsRoleMapping clsRole);
        List<clsAddUserMapping> GetUserDataByKGIDNumberBLL(dynamic KGIDNumber);
        List<clsAddUserMapping> GetUserMappingDataBLL(int userId, string value);
        List<SelectListItem> GetSectionsListBLL(int UnitId);
        List<SelectListItem> GetWingsListBLL();
        List<SelectListItem> GetRolesListBLL(int unitId, int sectionId);
    }
}
