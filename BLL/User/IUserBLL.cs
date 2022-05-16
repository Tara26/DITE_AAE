using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Admission;
using Models.MenuItem;
using Models.User;

namespace BLL.User
{
    public interface IUserBLL
    {
        UserLogin LoginBll(string userName, string Password);
        List<MenuItems> MenuList(int roleId);
        List<UserDetails> GetLoginRoles(int loginId, int subDeptId);
        UserDetails GetMappingId(int roleId, int loginId, int? subDeptId);
        List<Department> GetDepartments(int loginId);
        void UpdatePassword(string userName, string EncPassword);
        void InsertUserHistory(string userName, string NewPassword);
        void ChangePassword(string userName, string OldPassword, string NewPassword);
        int? GetUserHistory(string userName);
        void UpdatewhileLogOut(string userId);
    }
}
