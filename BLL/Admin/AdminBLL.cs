using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Admin;
using Models.Admin;
using System.Web.Mvc;

namespace BLL.Admin
{
    public class AdminBLL : IAdminBLL
    {
        private readonly IAdminDLL _AdminDll;

        public AdminBLL()
        {
            this._AdminDll = new AdminDLL();
        }

        public List<SelectListItem> GetUnitsListBLL()
        {
            return _AdminDll.GetUnitsListDLL();
        }
        public string GetUnitsListBLL(clsAddRolesUnits roles)
        {
            return _AdminDll.GetUnitsListDLL(roles);
        }
        public string AddUserMappingBLL(List<clsAddUserMapping> lstUserMap)
        {
            return _AdminDll.AddUserMappingDLL(lstUserMap);
        }
        public List<clsAddRolesUnits> GetRolesDataBLL(string userId, string value, int unitId, int sectionId)
        {
            return _AdminDll.GetRolesDataDLL(userId, value, unitId, sectionId);
        }
        public List<clsEditProfile> GetEditProfileDataBLL(int userId)
        {
            
           var data = _AdminDll.GetEditProfileDataDLL(userId);
            foreach (var item in data)
            {
                if (item.EmployeeDOB != null)
                {
                    item.DOB = item.EmployeeDOB.Value.ToString("dd/MM/yyyy");
                }
            }
            return data;

        }
        public string GetEditProfileDataBLL(clsRoleMapping clsRole, List<clsEditProfile> lstEditProfile, string path)
        {
            return _AdminDll.GetEditProfileDataDLL(clsRole, lstEditProfile, path);
        }
        public List<string> GetEnteredRoleListBLL(string prefix)
        {
            return _AdminDll.GetEnteredRoleListDLL(prefix);
        }
        public List<clsMenuList> GetRoleMapDataBLL(int val)
        {
            return _AdminDll.GetRoleMapDataDLL(val);
        }
        public string GetRoleMapDataBLL(List<clsRoleMapping> lstRoleMapping)
        {
            return _AdminDll.GetRoleMapDataDLL(lstRoleMapping);
        }
        public List<clsHolidayList> GetHolidayListBLL(int id, string value)
        {
            return _AdminDll.GetHolidayListDLL(id, value);
        }
        public string GetHolidayListBLL(clsHolidayList clsHoliday)
        {
            return _AdminDll.GetHolidayListDLL(clsHoliday);
        }

        public List<clsRoleMapping> GetRoleMapDetailsDataBLL(int id, string value, clsRoleMapping clsRole)
        {
            return _AdminDll.GetRoleMapDetailsDataDLL(id, value, clsRole);
        }
        public List<clsAddUserMapping> GetUserDataByKGIDNumberBLL(dynamic KGIDNumber)
        {
            return _AdminDll.GetUserDataByKGIDNumberBLL(KGIDNumber);
        }
        public List<clsAddUserMapping> GetUserMappingDataBLL(int userId, string value)
        {
            return _AdminDll.GetUserMappingDataDLL(userId, value);
        }
        public List<SelectListItem> GetSectionsListBLL(int UnitId)
        {
            return _AdminDll.GetSectionsListDLL(UnitId);
        }
        public List<SelectListItem> GetWingsListBLL()
        {
            return _AdminDll.GetWingsListDLL();
        }
        public List<SelectListItem> GetRolesListBLL(int unitId, int sectionId )
        {
            return _AdminDll.GetRolesListDLL(unitId, sectionId);
        }
    }
}
