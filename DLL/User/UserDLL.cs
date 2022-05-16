using DLL.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Models.User;
using Models.MenuItem;
using Models.Admission;
using Models.Master;

namespace DLL.User
{
    public class UserDLL : IUserDLL
    {
        private readonly DbConnection _db = new DbConnection();
        public UserLogin LoginDll(string userName, string Password)
        {
            try
            {
                var UserDtls = new UserLogin();
                long KGIDNum = 0;
                if (long.TryParse(userName, out KGIDNum))
                    KGIDNum = Convert.ToInt64(userName);
                UserDtls = (from n in _db.tbl_user_master
                            join cc in _db.tbl_user_mapping on n.um_id equals cc.um_id
                            join role in _db.tbl_role_master on cc.role_id equals role.role_id
                            where (n.um_kgid_number == KGIDNum || n.um_mobile_no == userName) //&& n.u_password == encryptedPassword
                            select new UserLogin
                            {
                                RoleName = role.role_description,
                                RoleId = cc.role_id,
                                userId = n.um_id,
                                userName = n.um_name,
                                userFatherName = n.um_father_name,
                                Password = n.um_password,
                                SubDeptId = cc.sub_dept_id,
                                KGIDNum = n.um_kgid_number,
                                user_is_active = n.um_is_active
                            }).FirstOrDefault();
                if (UserDtls != null)
                {
                    if (UserDtls.Password != null && UserDtls.Password.Length < 50)
                    {
                        var res = _db.tbl_user_master.Where(x => (x.um_kgid_number != null ? x.um_kgid_number.ToString() == userName : x.um_mobile_no == userName)).FirstOrDefault();

                        res.um_password = Password;
                        res.um_updation_datetime = DateTime.Now;
                        _db.SaveChanges();

                    }
                    if (UserDtls.SubDeptId != null)
                    {
                        var dd = _db.tbl_user_mapping.Where(x => x.um_id == UserDtls.userId).Select(y => y.sub_dept_id).ToList();
                        if (dd.Count == 1)
                        {
                            UserDtls.SubDeptCount = 1;
                        }
                        else
                        {
                            UserDtls.SubDeptCount = 2;
                        }
                    }
                    var ll = _db.tbl_user_mapping.Where(x => x.um_id == UserDtls.userId).Select(y => y.role_id).Distinct().ToList();
                    if (ll.Count == 1)
                    {
                        UserDtls.RoleCount = 1;
                    }
                    else
                    {
                        UserDtls.RoleCount = 2;
                    }
                    var user_history = _db.tbl_user_history_mast.Where(x => x.um_id == UserDtls.userId).FirstOrDefault();
                    var currentpath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

                    if ((user_history != null))
                    {
                        int getloginattempt = user_history.um_loginattempt;
                        if (getloginattempt >= 0)
                        {
                            user_history.um_loginattempt = getloginattempt + 1;
                            user_history.um_updated_by = UserDtls.userId;
                            user_history.um_updation_datetime = DateTime.Now;
                            if (currentpath == "/Home/Logout")
                            {
                                user_history.LoggedIn = false;
                            }
                            else
                            {
                                user_history.LoggedIn = true;
                            }

                            user_history.SessionId = System.Web.HttpContext.Current.Session.SessionID;
                            _db.SaveChanges();

                        }
                    }
                    return UserDtls;
                }
                else
                {
                    var UserDtls1 = new UserLogin();
                    UserDtls1 = (from n in _db.tbl_user_master                                
                                where (n.um_kgid_number == KGIDNum || n.um_mobile_no == userName) 
                                select new UserLogin
                                {                                    
                                    userId = n.um_id,
                                    //userName = n.um_name,
                                    userFatherName = n.um_father_name,
                                    Password = n.um_password,                                    
                                    //KGIDNum = n.um_kgid_number,
                                    user_is_active = n.um_is_active
                                }).FirstOrDefault();
                    if (UserDtls1 != null)
                    {
                        return UserDtls1;                        
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // This method (MenuList) is being used for Login Menus as well as Mapping of Menus in Super Admin Login.
        // Be careful while making any changes to this logic.Any changes to this Menu needs to be cross checked with Super Admin Login.
        public List<MenuItems> MenuList(int userMapId)
        {
            try
            {
                var res = new List<MenuItems>();
                if (userMapId != 0)
                {
                    res = (from menDet in _db.MenuDetails
                           join b in _db.MenuRoles on menDet.Id equals b.MenuId
                           where menDet.IsActive == true && b.UserMap_Id == userMapId && b.IsActive == true
                           
                           select new MenuItems
                           {
                               Url = menDet.Url,
                               MenuName = menDet.MenuName,
                               MenuId = menDet.Id,
                               ParentMenu = menDet.ParentMenu,
                               //MenuClass = i.MenuClass
                           }).ToList();
                }
                else
                {
                    res = (from menDet in _db.MenuDetails
                           where menDet.IsActive == true
                           orderby menDet.MenuOrder
                           select new MenuItems
                           {
                               Url = menDet.Url,
                               MenuName = menDet.MenuName,
                               MenuId = menDet.Id,
                               ParentMenu = menDet.ParentMenu,
                               //MenuClass = i.MenuClass
                           }).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Note: Here Sub Dept Id is always set to 0 when Role selection Modal popup is displayed.
        // subDeptId is being used as role id in some instances
        public List<UserDetails> GetLoginRoles(int loginId, int subDeptId)
        {
            try
            {
                var res = (from aa in _db.tbl_user_mapping
                           join bb in _db.tbl_role_master on aa.role_id equals bb.role_id
                           where aa.um_id == loginId && aa.is_active == true && (subDeptId != 0 ? aa.role_id == subDeptId : true)
                           select new UserDetails
                           {
                               RoleID = bb.role_id,
                               RoleName = bb.role_description
                           }
                    ).Distinct().ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserDetails GetMappingId(int roleId, int loginId, int? subDeptId)
        {
            try
            {
                var res = (from aa in _db.tbl_user_mapping
                           where aa.um_id == loginId && aa.is_active == true && aa.role_id == roleId && (subDeptId !=0 ? aa.sub_dept_id == subDeptId : true)
                           select new UserDetails
                           {
                               MenuMappingId = aa.user_map_id
                           }
                    ).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Department> GetDepartments(int loginId)
        {
            try
            {
                var res = (from aa in _db.tbl_sub_department_master
                               //join bb in _db.tbl_user_mapping on aa.sub_dept_id  equals bb.sub_dept_id 
                           join bb in _db.tbl_user_mapping on aa.sub_dept_id equals bb.sub_dept_id
                           where aa.is_active == true && bb.um_id == loginId
                           select new Department
                           {
                               DeptId = aa.sub_dept_id,
                               DeptName = aa.sub_dept_description

                           }).Distinct().ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public bool IsYourLoginStillTrue(string userId, string sid)
        //{

        //  IEnumerable<tbl_user_history> logins = (from i in _db.tbl_user_history_mast
        //                                          where i.LoggedIn == true && i.um_name == userId && i.SessionId == sid
        //                                          select i).AsEnumerable();
        //  return logins.Any();
        //}

        //public bool IsUserLoggedOnElsewhere(string userId, string sid)
        //{

        //  IEnumerable<tbl_user_history> logins = (from i in _db.tbl_user_history_mast
        //                                          where i.LoggedIn == true &&
        //                                i.um_name == userId && i.SessionId != sid
        //                                          select i).AsEnumerable();
        //  return logins.Any();
        //}
        public void UpdatePassword(string userName, string EncPassword)
        {
            try
            {

                using (var transaction = _db.Database.BeginTransaction())
                {

                    var querya =
             (from n in _db.tbl_user_master
              join cc in _db.tbl_user_mapping on n.um_id equals cc.um_id
              join role in _db.tbl_role_master on cc.role_id equals role.role_id
              where n.um_name == userName
              select new UserLogin
              {
                  RoleName = role.role_description,
                  RoleId = cc.role_id,
                  userId = n.um_id,
                  userName = n.um_name,
                  Password = n.um_password,
                  SubDeptId = cc.sub_dept_id

              }).FirstOrDefault();
                    var res = _db.tbl_user_master.Where(x => x.um_is_active == true).ToList();
                    foreach (var n in res)
                    {
                        n.um_password = EncPassword;
                        n.um_updation_datetime = DateTime.Now;
                        _db.SaveChanges();
                    }

                    //var updateuserhistory = _db.tbl_user_history_mast.Where(x => x.um_id == querya.userId).FirstOrDefault();
                    //if (updateuserhistory != null)
                    //{
                    //	updateuserhistory.um_id = querya.userId;
                    //	res.um_password = EncPassword;
                    //	res.um_updation_datetime = DateTime.Now;
                    //	_db.SaveChanges();
                    //}
                    // InsertUserHistory(res.um_name, EncPassword);




                    transaction.Commit();


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertUserHistory(string userName, string NewPassword)
        {
            try
            {
                var querya = (from n in _db.tbl_user_master
                              join cc in _db.tbl_user_mapping on n.um_id equals cc.um_id
                              join role in _db.tbl_role_master on cc.role_id equals role.role_id
                              where (n.um_kgid_number != null ? n.um_kgid_number.ToString() == userName : n.um_mobile_no == userName)
                              select new UserLogin
                              {
                                  RoleName = role.role_description,
                                  RoleId = cc.role_id,
                                  userId = n.um_id,
                                  userName = n.um_name,
                                  Password = n.um_password,
                                  SubDeptId = cc.sub_dept_id,
                                  KGIDNum = n.um_kgid_number
                              }).FirstOrDefault();

                try
                {
                    tbl_user_history tbluserhistory = new tbl_user_history();
                    tbluserhistory.um_id = querya.userId;
                    tbluserhistory.um_name = querya.KGIDNum.ToString();
                    tbluserhistory.um_email_id = "";
                    tbluserhistory.um_mobile_no = "";
                    tbluserhistory.um_password = NewPassword;
                    tbluserhistory.um_previouspassword = "";
                    tbluserhistory.um_is_active = true;
                    tbluserhistory.um_loginattempt = 1;
                    tbluserhistory.um_created_by = querya.loginId;
                    tbluserhistory.um_creation_datetime = DateTime.Now;
                    tbluserhistory.um_updated_by = querya.loginId;
                    tbluserhistory.um_updation_datetime = DateTime.Now;
                    _db.tbl_user_history_mast.Add(tbluserhistory);
                    _db.SaveChanges();
                    // log.Info("User Informations Stored Successfully");
                }
                catch (Exception ex)
                {
                    // log.Error(ex.Message.ToString());
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ChangePassword(string userName, string OldPassword, string NewPassword)
        {
            try
            {

                using (var transaction = _db.Database.BeginTransaction())
                {

                    var querya = (from n in _db.tbl_user_master
                                  join cc in _db.tbl_user_mapping on n.um_id equals cc.um_id
                                  join role in _db.tbl_role_master on cc.role_id equals role.role_id
                                  where n.um_name == userName
                                  select new UserLogin
                                  {
                                      RoleName = role.role_description,
                                      RoleId = cc.role_id,
                                      userId = n.um_id,
                                      userName = n.um_name,
                                      Password = n.um_password,
                                      SubDeptId = cc.sub_dept_id

                                  }).FirstOrDefault();
                    var changepassword = _db.tbl_user_master.Where(x => x.um_id == querya.userId).FirstOrDefault();
                    var user_history = _db.tbl_user_history_mast.Where(x => x.um_id == querya.userId).FirstOrDefault();

                    if ((user_history != null))
                    {
                        if (OldPassword == changepassword.um_password)
                        {
                            user_history.um_password = NewPassword;
                            user_history.um_previouspassword = OldPassword;
                            user_history.um_updated_by = querya.userId;
                            user_history.um_updation_datetime = DateTime.Now;
                            _db.SaveChanges();
                            // log.Info("User Informations Updated Successfully");
                        }
                        else
                        {
                            //   log.Info("Entered Old Password does not match with Database Old Password");
                            throw new Exception("Entered Old Password does not match with Database Old Password");
                        }

                    }
                    if ((changepassword != null))
                    {
                        if (changepassword.um_password != NewPassword)
                        {
                            changepassword.um_password = NewPassword;
                            changepassword.um_updation_datetime = DateTime.Now;
                            _db.SaveChanges();
                        }
                        else
                        {
                            // log.Info("New Password does match with Previous Password");
                            throw new Exception("New Password does match with Previous Password");
                        }
                    }
                    else
                    {
                        // log.Info("Faild to catch the data from database");
                        throw new Exception("Faild to catch the data from database");
                    }

                    transaction.Commit();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int? GetUserHistory(string userName)
        {
            try
            {
                var user_history = (from i in _db.tbl_user_history_mast
                                    where i.LoggedIn == true && i.um_name == userName
                                    select i.um_loginattempt).FirstOrDefault();
                //var user_history = _db.tbl_user_history_mast.Where(x => x.um_name == userName).Select x.um_loginattempt.FirstOrDefault();

                return user_history;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatewhileLogOut(string userId)
        {

            var logins = (from i in _db.tbl_user_history_mast
                          where i.LoggedIn == true &&
                          i.um_name == userId // need to filter by user ID
                          select i.LoggedIn).FirstOrDefault();

            logins = false;
            _db.SaveChanges();
        }
    }
}
