using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.User;
using DLL;
using DLL.User;
using Models.MenuItem;
using Models.Admission;

namespace BLL.User
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserDLL _user;

        public UserBLL()
        {
            this._user = new UserDLL();
        }
        public UserLogin LoginBll(string userName, string Password)
        {
            return _user.LoginDll(userName, Password);
        }
        public List<MenuItems> MenuList(int roleId)
        {
            return _user.MenuList(roleId);
        }
        public List<UserDetails> GetLoginRoles(int loginId,int subDeptId)
        {
            return _user.GetLoginRoles(loginId, subDeptId);
        }
        public UserDetails GetMappingId(int roleId, int loginId, int? subDeptId)
        {
            return _user.GetMappingId(roleId, loginId, subDeptId);
        }
        public List<Department> GetDepartments(int loginId)
        {
            return _user.GetDepartments(loginId);
        }
    public void UpdatePassword(string userName, string EncPassword)
    {
      _user.UpdatePassword(userName, EncPassword);
    }
    public void InsertUserHistory(string userName, string NewPassword)
    {
      _user.InsertUserHistory(userName, NewPassword);
    }
    public void ChangePassword(string userName, string OldPassword, string NewPassword)
    {
      _user.ChangePassword(userName, OldPassword, NewPassword);
    }
    public int? GetUserHistory(string userName)
    {
      return _user.GetUserHistory(userName);
    }
    public void UpdatewhileLogOut(string userId)
    {
      _user.UpdatewhileLogOut(userId);
    }
  }
}
