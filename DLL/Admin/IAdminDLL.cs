using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.Admin;

namespace DLL.Admin
{
    public interface IAdminDLL
    {
        List<SelectListItem> GetUnitsListDLL();
        string GetUnitsListDLL(clsAddRolesUnits roles);
        string AddUserMappingDLL(List<clsAddUserMapping> lstUserMap);
        List<clsAddRolesUnits> GetRolesDataDLL(string userId, string value, int unitId, int sectionId);
        List<clsEditProfile> GetEditProfileDataDLL(int userId);
        string GetEditProfileDataDLL(clsRoleMapping clsRole, List<clsEditProfile> lstEditProfile, string path);
        List<string> GetEnteredRoleListDLL(string prefix);
        List<clsMenuList> GetRoleMapDataDLL(int val);
        string GetRoleMapDataDLL(List<clsRoleMapping> lstRoleMapping);
        List<clsHolidayList> GetHolidayListDLL(int id, string value);
        string GetHolidayListDLL(clsHolidayList clsHoliday);
        List<clsRoleMapping> GetRoleMapDetailsDataDLL(int id, string value, clsRoleMapping clsRole);
        List<clsAddUserMapping> GetUserDataByKGIDNumberBLL(dynamic KGIDNumber);
        List<clsAddUserMapping> GetUserMappingDataDLL(int userId, string value);
        List<SelectListItem> GetSectionsListDLL(int UnitId);
        List<SelectListItem> GetWingsListDLL();
        List<SelectListItem> GetRolesListDLL(int unitId, int sectionId);
    }
}
