using DLL.DBConnection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Admin;
using System.Web.Mvc;
using Models.Master;
using Models;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace DLL.Admin
{
    public class AdminDLL : IAdminDLL
    {
        private readonly DbConnection _db = new DbConnection();

        //Get from role Level table for Units multiselect drop down
        public List<SelectListItem> GetUnitsListDLL()
        {
            var res = (from a in _db.tbl_LevelMaster
                       where a.IsActive == true
                       select new SelectListItem
                       {
                           Text = a.Name,
                           Value = a.LevelId.ToString()
                       }).AsQueryable().ToList();
            return res;
        }

        //Update to role master table
        public string GetUnitsListDLL(clsAddRolesUnits roles)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var roleMasterList = new List<int>();
                    var chkExistingRole = GetRoles(roles.RoleName);
                    if (chkExistingRole != null)
                    {
                        roleMasterList = roles.MultiSelectUnitList.Where(a => !chkExistingRole.Any(item2 => item2.role_Level == a)).ToList();
                    }
                    else
                    {
                        roleMasterList = roles.MultiSelectUnitList;
                    }
                    if (roleMasterList.Count > 0)
                    {
                        tbl_role_master role_Master = new tbl_role_master();
                        for (int i = 0; i <= roleMasterList.Count - 1; i++)
                        {
                            role_Master.role_description = roles.RoleName;
                            role_Master.role_seniority_no = GetSeniorityRoleValue(roles.RoleName);
                            role_Master.role_is_active = true;
                            role_Master.role_Level = roleMasterList[i];
                            role_Master.role_creation_datetime = DateTime.Now;
                            _db.tbl_role_master.Add(role_Master);
                            _db.SaveChanges();
                        }
                        transaction.Commit();
                        return "Success";
                    }
                    else
                    {
                        if (chkExistingRole.Count != roles.MultiSelectUnitList.Count)
                        {
                            transaction.Rollback();
                            chkExistingRole = chkExistingRole.Where(a => !roles.MultiSelectUnitList.Any(item2 => item2 == a.role_Level)).ToList();
                            foreach (var item in chkExistingRole)
                            {
                                GetRolesDataDLL("", "Delete", item.role_id, 0);
                            }
                            return "Updated";
                        }
                        return "Exists";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }
            }
        }

        public string AddUserMappingDLL(List<clsAddUserMapping> lstUserMap)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (lstUserMap.Count > 0)
                    {
                        int KGIDNum;
                        tbl_user_master user;
                        var userMap = lstUserMap[0];
                        int roleId = userMap.lstRoleMapping.Select(b => b.RoleId).FirstOrDefault();
                        if (int.TryParse(userMap.KGIDUserID, out KGIDNum))
                            user = _db.tbl_user_master.Where(a => a.um_kgid_number.ToString() == userMap.KGIDUserID).FirstOrDefault();
                        else
                            user = _db.tbl_user_master.Where(a => a.um_name.ToString() == userMap.KGIDUserID).FirstOrDefault();
                        user.um_div_id = userMap.lstRoleMapping.Select(a => a.LocationDivId).FirstOrDefault();
                        if (user != null)
                        {
                            int umId = user.um_id;
                            if (userMap.lstRoleMapping[0].UnitId == 3)
                            {
                                var sid = _db.Staff_Institute_Detail.Where(a => a.UserId == umId).FirstOrDefault();
                                if (userMap.lstRoleMapping.Select(a => a.UnitId).First() == 3)
                                {
                                    if (sid == null)
                                        sid = new Staff_Institute_Detail();
                                    sid.InstituteId = userMap.lstRoleMapping.Select(a => a.InstituteId).FirstOrDefault();
                                    sid.UserId = umId;
                                    sid.CreatedOn = DateTime.Now;
                                    sid.CreatedBy = roleId;
                                    if (sid.StaffId == 0)
                                        _db.Staff_Institute_Detail.Add(sid);
                                }
                            }

                            int sectionId = userMap.lstRoleMapping[0].SectionId;
                            int usrMapId = 0;
                            if (userMap.btnValue != "Update")
                            {
                                usrMapId = (from tum in _db.tbl_user_master
                                                join tumap in _db.tbl_user_mapping on tum.um_id equals tumap.um_id
                                                join trmap in _db.tbl_role_mapping on tumap.role_id equals trmap.role_id
                                                join tmr in _db.MenuRoles on trmap.dept_id equals tmr.MenuId

                                                where tumap.um_id == umId && tumap.role_id == roleId
                                                //&& tmr.MenuId == sectionId
                                                select tumap.user_map_id).Take(1).FirstOrDefault();
                            }
                            else
                            {
                                usrMapId = userMap.userMapId;
                            }
                            var user_Mapping = _db.tbl_user_mapping.Where(a => a.user_map_id == usrMapId).FirstOrDefault();
                            if (user_Mapping == null)
                                user_Mapping = new tbl_user_mapping();
                            user_Mapping.role_id = userMap.lstRoleMapping[0].RoleId;
                            user_Mapping.um_id = umId;
                            //user_Mapping.dept_id = userMap.lstRoleMapping.Select(a => a.SectionId).FirstOrDefault();
                            user_Mapping.is_active = true;
                            user_Mapping.creation_datetime = DateTime.Now;
                            user_Mapping.created_by = roleId;
                            if (user_Mapping.user_map_id == 0)
                                _db.tbl_user_mapping.Add(user_Mapping);
                            _db.SaveChanges();

                            int uMapId = (from tum in _db.tbl_user_mapping
                                          orderby tum.creation_datetime descending
                                          //where tum.um_id == user_master.um_id
                                          select tum.user_map_id).Take(1).FirstOrDefault();

                            var role_Mapping = _db.tbl_role_mapping.Where(a => a.role_id == roleId && a.dept_id == sectionId).ToList();

                            if (role_Mapping.Count > 0)
                            {
                                SetMenuRoles(userMap.lstRoleMapping.Select(a => a.SectionId).FirstOrDefault(), uMapId);
                                foreach (var itm in role_Mapping)
                                {
                                    SetMenuRoles(itm.sub_dept_id, uMapId);
                                }
                                _db.SaveChanges();
                            }
                            transaction.Commit();
                            return "Success";
                        }
                        else
                        {
                        }
                    }
                    transaction.Rollback();
                    return "Exists";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }
            }
        }

        private void SetMenuRoles(int MenuId, int uMapId)
        {
            var menuRoles = _db.MenuRoles.Where(a => a.MenuId == MenuId && a.UserMap_Id == uMapId).FirstOrDefault();
            if (menuRoles == null)
                menuRoles = new MenuRoles();
            menuRoles.MenuId = MenuId;
            menuRoles.UserMap_Id = uMapId;
            menuRoles.Created_On = DateTime.Now;
            menuRoles.IsActive = true;
            if (menuRoles.Id == 0)
                _db.MenuRoles.Add(menuRoles);
        }


        public List<clsAddRolesUnits> GetRolesDataDLL(string userId, string value, int unitId, int sectionId)
        {
            #region commented code to group by in Linq
            //var roles = (from trm in _db.tbl_role_master
            //             where trm.role_is_active == true && trm.role_Level.ToString() != null
            //             orderby trm.role_seniority_no ascending
            //             group trm by trm.role_seniority_no into trm1
            //             select new AddRolesUnits
            //             {
            //                 RoleName = trm1.Select(a => a.role_description).FirstOrDefault(),
            //                 RoleId = trm1.Select(a => a.role_id).FirstOrDefault(),
            //                 RoleLevel = trm1.Select(a => a.role_Level).FirstOrDefault(),
            //                 lstRoleLevel = new List<int>() { trm1.Select(a => a.role_Level).FirstOrDefault() } //GetRolesLevel(trm1.Select(a => a.role_seniority_no).FirstOrDefault())
            //             }).AsQueryable().ToList();

            //return roles;
            //var roles = (from trm in _db.tbl_role_master
            //             where trm.role_is_active == true && trm.role_Level.ToString() != null
            //             && trm.role_seniority_no == 80
            //             select new AddRolesUnits
            //             {
            //                 RoleId = trm.role_id,
            //                 RoleName = trm.role_description,
            //                 RoleLevel = trm.role_Level,
            //                 RoleSeniorityLevel = trm.role_seniority_no
            //             }).AsQueryable().OrderBy(a => a.RoleName).ToList();

            //return roles;
            //var roles1 = new List<AddRolesUnits>();
            //AddRolesUnits addRolesUnits = new AddRolesUnits()
            //{
            //    RoleId = roles.Select(a => a.RoleId).FirstOrDefault(),
            //    RoleName = roles.Select(a => a.RoleName).FirstOrDefault(),
            //    RoleSeniorityLevel = roles.Select(a => a.RoleSeniorityLevel).FirstOrDefault(),
            //    lstRoleLevel = new List<int>()
            //    {
            //        new int(),
            //    },
            //};
            ////addRolesUnits.lstRoleLevel lstRole = new addRolesUnits.lstRoleLevel();
            //foreach (var item in roles)
            //{
            //    addRolesUnits.RoleId = item.RoleId;
            //    addRolesUnits.RoleName = item.RoleName;
            //    addRolesUnits.RoleSeniorityLevel = item.RoleSeniorityLevel;

            //    if (!roles1.Select(a => a.RoleSeniorityLevel).Contains(item.RoleSeniorityLevel))
            //    {
            //        addRolesUnits.lstRoleLevel.Add(item.RoleLevel);
            //    }
            //    roles1.Add(addRolesUnits);
            //}



            //        List<Vehicle> vehicles = new List<Vehicle>();

            //        Vehicle vehicle = new Vehicle()
            //        {
            //            Id = "XPT",
            //            Description = "Average Car",
            //            Steps = new List<Step>()
            //{
            //    new Step() {
            //        Name = "move car",
            //        Movements = new List<Movement>()
            //        {
            //            new Movement("engage 1st gear", 1, 1),
            //            new Movement("reach 10kph", 10, 5),
            //            new Movement("maintain 10kph", 10, 12),
            //        }
            //    },
            //    new Step() {
            //        Name = "stop car",
            //        Movements = new List<Movement>()
            //        {
            //            new Movement("reach 0kph", 10, 4),
            //            new Movement("put in neutral", 0, 1),
            //            new Movement("turn off vehicle", 0, 0),
            //        }
            //    }
            //}
            //        };
            //        vehicles.Add(vehicle);



            //return roles1;
            #endregion

            if (value != "Delete")
            {
                var roles1 = (from trm in _db.tbl_role_master
                              join trmap in _db.tbl_role_mapping on trm.role_id equals trmap.role_id into trmap1
                              from trmap in trmap1.DefaultIfEmpty()

                              where trm.role_is_active == true
                         && (userId != "" ? trm.role_description.Replace(" ", "").Trim().ToUpper() == userId.Replace(" ", "").Trim().ToUpper() : true) &&
                         trm.role_Level.ToString() != null && (unitId != 0 ? trm.role_Level == unitId : true) && (sectionId != 0 ? trmap.dept_id == sectionId : true)
                              select new clsAddRolesUnits
                          {
                             RoleId = trm.role_id,
                             RoleName = trm.role_description,
                             RoleLevel = trm.role_Level == null ? 0 : (int)trm.role_Level,
                             RoleSeniorityLevel = trm.role_seniority_no,
                             //lstRoleLevel = new List<int>() { trm.role_Level}
                         }).AsQueryable().OrderBy(a => a.RoleSeniorityLevel).OrderBy(a=> a.RoleLevel).ToList();

                List<clsAddRolesUnits> roles = new List<clsAddRolesUnits>();

                foreach (var item in roles1)
                {
                    if (!roles.Select(a => a.RoleName).Contains(item.RoleName))
                    {
                        clsAddRolesUnits addRolesUnits = new clsAddRolesUnits()
                        {
                            RoleId = item.RoleId,
                            RoleName = item.RoleName,
                            RoleSeniorityLevel = item.RoleSeniorityLevel,
                            message = "",
                            //lstRoleLevel = (!roles.Select(a => a.RoleSeniorityLevel).Contains(item.RoleSeniorityLevel) ? new List<int>() { item.RoleLevel} : )
                            lstRoleLevel = new List<int>()
                        {
                            item.RoleLevel
                        }
                        };
                        roles.Add(addRolesUnits);
                    }
                    else
                    {
                        var b = roles.Select(a => a.lstRoleLevel).ToList();
                        b[roles.FindIndex(a => a.RoleName == item.RoleName)].Add(item.RoleLevel);
                    }
                }
                return roles.OrderBy(a=> a.RoleName).ToList();
            }
            else
            {
                List<clsAddRolesUnits> lstclsAdd = new List<clsAddRolesUnits>();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var res = new List<tbl_role_master>();
                    if (unitId == 0)
                        res = _db.tbl_role_master.Where(a => a.role_description.Replace(" ", "").Trim().ToUpper() == userId.Replace(" ", "").Trim().ToUpper()).ToList();
                    else
                        res = _db.tbl_role_master.Where(a => a.role_id == unitId).ToList();
                    clsAddRolesUnits clsAdd = new clsAddRolesUnits();
                    if (res.Count > 0)
                    {
                        foreach (var item in res)
                        {
                            if (item.role_id > 19)
                            {
                                _db.tbl_role_master.Remove(item);
                            }
                            else
                                clsAdd.message = "One of the role being removed is mapped. with users. It cannot be removed/deleted.";
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                        clsAdd.message = "Success";
                        lstclsAdd.Add(clsAdd);
                    }
                    return lstclsAdd;
                }
            }
        }
        
        public List<clsEditProfile> GetEditProfileDataDLL(int userId)
        {
            var res = (from a in _db.tbl_user_master
                       join g in _db.tbl_Gender on a.um_gender equals g.Gender_Id into g1
                       from g in g1.DefaultIfEmpty()

                       where userId != 0 ? a.um_id == userId : true
                       orderby a.um_creation_datetime descending
                       select new clsEditProfile
                       {
                           userId = a.um_id,
                           KGIDNumber = a.um_kgid_number,
                           UserName = a.um_name,
                           FatherName = a.um_father_name,
                           Mobile = a.um_mobile_no,
                           Email = a.um_email_id,
                           Gender = g.Gender,
                           Designation = a.um_designation,
                           EmployeeDOB = a.um_dob,
                            status = a.um_is_active,
                           Photo = a.um_photo
                       }).AsQueryable().ToList();

            return res;
        }
        
        public string GetEditProfileDataDLL(clsRoleMapping clsRole, List<clsEditProfile> lstEditProfile, string path)
        {
            var user_master = _db.tbl_user_master.Where(a => a.um_id == clsRole.UnitId).FirstOrDefault();

            var res = "";
            bool IsPermissionGranted = false;
            if (lstEditProfile.Count > 0)
            {
                if (lstEditProfile[0].ImageFile != null)
                {
                    if (lstEditProfile[0].ImageFile.ContentLength == 0)
                    {
                        res = "Please upload File";
                    }
                    else if (lstEditProfile[0].ImageFile.ContentLength > 0)
                    {
                        string FileNameToCreate = "_UploadPhoto";
                        string fileName = lstEditProfile[0].ImageFile.FileName; // getting File Name

                        fileName = Path.GetFileNameWithoutExtension(lstEditProfile[0].ImageFile.FileName);

                        string fileContentType = lstEditProfile[0].ImageFile.ContentType; // getting ContentType
                        byte[] tempFileBytes = new byte[lstEditProfile[0].ImageFile.ContentLength]; // getting filebytes
                        var data = lstEditProfile[0].ImageFile.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(lstEditProfile[0].ImageFile.ContentLength));
                        var types = Common.FileUtility.FileType.Image;  // Setting Image type
                                                                        //  ImageFileExtension imageFileExtension;
                        string imageFile1, imageFile11;
                        bool isvalid1, isvalid11;
                        Common.FileUtility.isValidFile(tempFileBytes, types, fileContentType, out imageFile1, out isvalid1); // Validate Header

                        Common.FileUtility.isValidImageFile(tempFileBytes, fileContentType, out imageFile11, out isvalid11);
                        isvalid1 = true; //Added by Annaray on 06/05/2022
                        if (isvalid1 == true)
                        {
                            long fileSizeibKbs = lstEditProfile[0].ImageFile.ContentLength / 1024;

                            //int maxFileLength = 1024 * 3000; //FileLength 3 MB 
                            //if (lstEditProfile[0].ImageFile.ContentLength > maxFileLength)
                            //{
                            //    return res = "Size of pdf Upload file size exceeded max file upload size(3 MB)!";
                            //}
                            if (fileSizeibKbs > 200)
                            {
                                return res = "Size of JPG/PNG Upload file size exceeded max file upload size(200 KB)!";
                            }
                            else if (!Enum.IsDefined(typeof(Common.FileUtility.ImageFileExtension), imageFile1))
                            {
                                res = "Please upload only image file!";
                            }
                            else if (!Enum.IsDefined(typeof(Common.FileUtility.ImageFileExtension), imageFile11))
                            {
                                res = "Please upload only image file!";
                            }
                            else if (imageFile11 == "")
                            {
                                res = "Please upload only image file!";
                            }
                            else
                            { }
                            Match regex = Regex.Match(fileName, @"[\[\]{}!@#.]");

                            if (regex.Success)
                            {
                                res = "Please check uploaded file name!";
                            }
                            char[] invalidFileChars = Path.GetInvalidFileNameChars();
                            Common.FileUtility.ShowChars(invalidFileChars);

                            string UniqueFileName = null;
                            UniqueFileName = FileNameToCreate + "." + imageFile11;

                            string _path = Path.Combine(path, UniqueFileName);
                            string _pathCreate = Path.Combine(path);

                            FileInfo CheckFileName = new FileInfo(_path);

                            if (!Directory.Exists(_pathCreate))
                            {
                                Directory.CreateDirectory(_pathCreate);
                            }
                            if (CheckFileName.Exists)
                            {
                                FileNameToCreate = FileNameToCreate + "_" + DateTime.Now.Ticks;
                                UniqueFileName = FileNameToCreate + "." + imageFile11;
                            }

                            //IsPermissionGranted = Common.FileUtility.GrantFilePermission();
                            IsPermissionGranted = true;

                            lstEditProfile[0].ImageFile.SaveAs(_path);
                            lstEditProfile[0].Photo = "Content/Uploads/" + UniqueFileName;
                        }
                        else
                        {
                            res = "Please check Uploaded file";
                        }
                    }
                }
                else
                {
                    if (clsRole.UnitId != 0 && lstEditProfile[0].status != null) { 
                        IsPermissionGranted = true;
                    }
                    else if (clsRole.UnitId == 0 && lstEditProfile[0].status == null) { 
                    }
                    else {
                        res = "Please check Uploaded file";
                        return res;
                    }   
                }
                if (IsPermissionGranted)
                {
                    if (path.Contains("Mapping"))
                    {
                        if (path.Contains("Role"))
                        {
                            if (clsRole.UnitId != 0)
                            {
                                var role_Map = _db.tbl_role_mapping.Where(a => a.rm_id == clsRole.UnitId).FirstOrDefault();
                                role_Map.rm_is_active = !(bool)lstEditProfile[0].status;
                                role_Map.rm_creation_time = DateTime.Now;
                                var Menu_Roles = _db.MenuRoles.Where(a => a.MenuId == role_Map.sub_dept_id).ToList();
                                if (Menu_Roles.Count > 0)
                                {
                                    foreach (var item in Menu_Roles)
                                    {
                                        item.IsActive = !(bool)lstEditProfile[0].status;
                                        item.Created_On = DateTime.Now;
                                    }
                                }

                            }
                        }
                        if (path.Contains("User"))
                        {
                            if (clsRole.UnitId != 0)
                            {
                                int userMapId = _db.tbl_user_mapping.Where(a => a.um_id == clsRole.UnitId && a.role_id == clsRole.RoleId).Select(a => a.user_map_id).FirstOrDefault();
                                var menu_Map = _db.MenuRoles.Where(a => a.UserMap_Id == userMapId && a.MenuId == clsRole.SectionId).FirstOrDefault();
                                menu_Map.IsActive = !(bool)lstEditProfile[0].status;
                                menu_Map.Created_On = DateTime.Now;
                            }
                        }
                        _db.SaveChanges();
                        res = "Success";
                    }
                    else
                    {
                        if (user_master != null)
                        {
                            if (lstEditProfile[0].UserName != null)
                            {
                                user_master.um_name = lstEditProfile[0].UserName;
                                user_master.um_father_name = lstEditProfile[0].FatherName;
                                user_master.um_email_id = lstEditProfile[0].Email;
                                user_master.um_mobile_no = lstEditProfile[0].Mobile;
                                user_master.um_photo = lstEditProfile[0].Photo;
                            }
                            else
                            {
                                if (lstEditProfile[0].status != null)
                                    user_master.um_is_active = !(bool)lstEditProfile[0].status;
                            }
                            user_master.um_creation_datetime = DateTime.Now;
                        }

                    }
                    _db.SaveChanges();
                    res = "Success";
                }
                else if (lstEditProfile[0].ImageFile == null && user_master == null) {
                    {
                        tbl_user_master user_master1 = new tbl_user_master();
                        user_master1.um_name = lstEditProfile[0].UserName;
                        user_master1.um_father_name = lstEditProfile[0].FatherName;
                        user_master1.um_email_id = lstEditProfile[0].Email;
                        user_master1.um_mobile_no = lstEditProfile[0].Mobile;
                        user_master1.um_dob = lstEditProfile[0].EmployeeDOB;
                        user_master1.um_gender = Convert.ToInt32(lstEditProfile[0].Gender);
                        user_master1.um_designation = lstEditProfile[0].Designation;
                        if (lstEditProfile[0].EmployeeKGIDNumber != null)
                        {
                            user_master1.um_kgid_number = Convert.ToInt32(lstEditProfile[0].EmployeeKGIDNumber);
                        }
                        else
                        {
                            user_master1.um_kgid_number = null;
                        }
                        user_master1.um_is_active = true;
                        user_master1.um_creation_datetime = DateTime.Now;

                        //user_master1.um_photo = lstEditProfile[0].Photo;
                        _db.tbl_user_master.Add(user_master1);
                        _db.SaveChanges();
                        res = "Success";

                    }

                }
                else
                {
                    res = "Uploaded file Folder Permissions are not granted!";
                }
            }
            return res;
        }

        public List<string> GetEnteredRoleListDLL(string prefix)
        {
            var res = _db.tbl_role_master.Where(a => a.role_description.StartsWith(prefix.Trim())).Select(a => a.role_description).ToList();
            return res.Distinct().ToList();
        }


        public List<clsMenuList> GetRoleMapDataDLL(int val)
        {
            //var lstMenu = _db.MenuDetails.Where(a => a.IsActive == true && a.ParentMenu == 8).Select(a=> new clsMenuList { MenuName = a.MenuName }).ToList();
            
            var lstMenu = (from tdm in _db.tbl_department_master
                           join tsdm in _db.tbl_sub_department_master on tdm.dept_id equals tsdm.dept_id
                           where val != 0 ? tsdm.dept_id == val && tsdm.is_active == true : true && tsdm.is_active == true
                           //&& id != 0 ? trm.role_seniority_no == id : true && trm.role_Level.ToString() != null
                           select new clsMenuList
                           {
                               MenuId = tdm.dept_id,
                               SubMenuId = tsdm.sub_dept_id,
                               MenuName = tdm.dept_description,
                               SubMenuName = tsdm.sub_dept_description,

                           }).AsQueryable().ToList();

            return lstMenu;
        }

        public string GetRoleMapDataDLL(List<clsRoleMapping> lstRoleMapping)
        {
            string msg = "";
            var role = lstRoleMapping[0];
            var roleData = _db.tbl_role_master.Where(a => a.role_id == role.RoleId && a.role_Level == role.UnitId).Select(a => new { a.role_id, a.role_description }).Take(1).FirstOrDefault();

            using (var transaction = _db.Database.BeginTransaction())
            { 
                try
                {
                    if (lstRoleMapping.Count > 0)
                    {
                        foreach (var item in role.lstMenuList)
                        {
                            var role_Mapping = _db.tbl_role_mapping.Where(a => a.role_id == role.RoleId && a.dept_id == item.MenuId && a.sub_dept_id == item.SubMenuId).FirstOrDefault();
                            if (role_Mapping == null)
                                role_Mapping = new tbl_role_mapping();
                            role_Mapping.role_id = role.RoleId;
                            role_Mapping.division_id = role.LocationDivId;
                            role_Mapping.rm_wing_id = role.WingId;
                            role_Mapping.rm_is_active = true;
                            role_Mapping.rm_creation_time = DateTime.Now;


                            role_Mapping.dept_id = item.MenuId;
                            role_Mapping.sub_dept_id = item.SubMenuId;
                            role_Mapping.rm_create = item.Create;
                            role_Mapping.rm_edit = item.Edit;
                            role_Mapping.rm_delete = item.Delete;
                            role_Mapping.rm_view = item.View;
                            if (role_Mapping.rm_id == 0)
                                _db.tbl_role_mapping.Add(role_Mapping);
                            _db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    msg = "Success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    msg = "Failed";
                }
                
            }
            return msg;
        }

        public List<clsHolidayList> GetHolidayListDLL(int id, string value)
        {
            var res = new List<clsHolidayList>();
            if (!value.Contains("Delete"))
            {
                res = _db.tbl_Holiday_mast.Where(a => a.h_is_active == true && (id != 0 ? a.h_id == id : true))
                    .Select(a => new clsHolidayList()
                    {
                        HolidayId = a.h_id,
                        HolidayDate = SqlFunctions.DatePart("day", a.h_date) + "-" + SqlFunctions.DatePart("m", a.h_date) + "-" + SqlFunctions.DatePart("year", a.h_date),
                        HolidayDay = a.h_day,
                        HolidayName = a.h_name
                    }
                    ).OrderBy(a=> a.HolidayDate).ToList();
            }
            else
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    clsHolidayList clsHoliday = new clsHolidayList();
                    try
                    {
                        tbl_Holiday_mast tbl_Holiday = _db.tbl_Holiday_mast.Where(a => a.h_id == id).FirstOrDefault();
                        if (clsHoliday != null)
                        {
                            _db.tbl_Holiday_mast.Remove(tbl_Holiday);
                            _db.SaveChanges();
                            transaction.Commit();
                            clsHoliday.message = "Success";
                            res.Add(clsHoliday);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        clsHoliday.message = "Failed";

                    }
                }
            }
            return res;
        }

        public string GetHolidayListDLL(clsHolidayList clsHoliday)
        {
            string msg = "";
            DateTime dtHolidayDate = DateTime.Parse(clsHoliday.HolidayDate);
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var hol_data = _db.tbl_Holiday_mast.Where(a => clsHoliday.HolidayId == 0 ? a.h_date == dtHolidayDate : a.h_id == clsHoliday.HolidayId).FirstOrDefault();
                    if (hol_data == null || clsHoliday.HolidayId != 0)
                    {
                        if (hol_data == null)
                            hol_data = new tbl_Holiday_mast();
                        hol_data.h_date = dtHolidayDate;
                        hol_data.h_day = clsHoliday.HolidayDay;
                        hol_data.h_name = clsHoliday.HolidayName;
                        hol_data.h_is_active = true;
                        hol_data.h_creation_datetime = DateTime.Now;

                        if (hol_data.h_id == 0)
                        {
                            _db.tbl_Holiday_mast.Add(hol_data);
                            msg = "Success";
                        }
                        else
                            msg = "Updated";
                        _db.SaveChanges();
                        transaction.Commit();
                        
                    }
                    else
                        msg = "Exists";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    msg = "Failed";

                }
            }
            return msg;
        }

        public List<clsRoleMapping> GetRoleMapDetailsDataDLL(int id, string value, clsRoleMapping clsRole)
        {
            //var lstMenu = _db.MenuDetails.Where(a => a.IsActive == true && a.ParentMenu == 8).Select(a=> new clsMenuList { MenuName = a.MenuName }).ToList();

            //var lstMenu = (from tumap in _db.tbl_user_mapping
            //               join tdm in _db.tbl_department_master on tumap.dept_id equals tdm.dept_id
            //               join tsdm in _db.tbl_sub_department_master on tumap.sub_dept_id equals tsdm.sub_dept_id
            //               join trm in _db.tbl_role_master on tumap.role_id equals trm.role_id
            //               join trl in _db.tbl_LevelMaster on trm.role_Level equals trl.LevelId
            //               join tum in _db.tbl_user_master on tumap.um_id equals tum.um_id
            //               join tdivm in _db.tbl_division_master on tum.um_div_id equals tdivm.division_id

            //               where tsdm.is_active == true
            //               orderby trl.LevelId ascending
            //               //&& id != 0 ? trm.role_seniority_no == id : true && trm.role_Level.ToString() != null
            //               select new clsRoleMapping
            //               {
            //                   //UnitId = trl.LevelId,
            //                   UnitName = trl.Name,
            //                   LocationDivName = tdivm.division_name,
            //                   RoleName = trm.role_description,
            //                   IsActive = tumap.is_active,
            //                   //umId = tumap.user_map_id,
            //                   lstMenuList = new List<clsMenuList>() {
            //                     new clsMenuList
            //                     {
            //                        MenuName = tdm.dept_description,
            //                        SubMenuName = tsdm.sub_dept_description
            //                     }
            //                 },
            //               }).AsQueryable().ToList();

            if (value != "Delete")
            {
                var lstMenu = (from trmap in _db.tbl_role_mapping
                               join tdm in _db.MenuDetails on trmap.dept_id equals tdm.Id
                               join tsdm in _db.MenuDetails on trmap.sub_dept_id equals tsdm.Id
                               join trm in _db.tbl_role_master on trmap.role_id equals trm.role_id
                               join trl in _db.tbl_LevelMaster on trm.role_Level equals trl.LevelId
                               //join tdivm in _db.tbl_division_master on trmap.division_id equals tdivm.division_id
                               join twm in _db.tbl_wing_master on trmap.rm_wing_id equals twm.Id into twm1
                               from twm in twm1.DefaultIfEmpty()


                               where /*trmap.rm_is_active == true &&*/
                               (id != 0 ? trmap.rm_id == id : true) &&
                               (clsRole.UnitId != 0 ? trm.role_Level == clsRole.UnitId : true) &&
                               (clsRole.WingId != 0 ? trmap.rm_wing_id == clsRole.WingId : true) &&
                               (clsRole.SectionId != 0 ? trmap.dept_id == clsRole.SectionId : true) &&
                               (clsRole.RoleId != 0 ? trmap.role_id == clsRole.RoleId : true)
                               

                               orderby trl.LevelId ascending orderby trmap.rm_creation_time descending
                               //&& id != 0 ? trm.role_seniority_no == id : true && trm.role_Level.ToString() != null
                               select new clsRoleMapping
                               {
                                   //UnitId = trl.LevelId,
                                   UnitId = (int)trm.role_Level,
                                   UnitName = trl.Name,
                                   //LocationDivId = trmap.division_id,
                                   //LocationDivName = tdivm.division_name,
                                   SectionId = trmap.dept_id,
                                   RoleId = trmap.role_id,
                                   RoleName = trm.role_description,
                                   IsActive = trmap.rm_is_active,
                                   rmId = trmap.rm_id,
                                   lstMenuList = new List<clsMenuList>() {
                                 new clsMenuList
                                 {
                                    MenuName = tdm.MenuName,
                                    SubMenuName = tsdm.MenuName,
                                    Create = trmap.rm_create,
                                    Edit = trmap.rm_edit,
                                    Delete = trmap.rm_delete,
                                    View = trmap.rm_view
                                 }
                             },
                               }).AsQueryable().ToList();


                return lstMenu;
            }
            else
            {
                List<clsRoleMapping> lstRoleMap = new List<clsRoleMapping>();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var res = _db.tbl_role_mapping.Where(a => a.rm_id == id).ToList();
                    if (res.Count > 0)
                    {
                        foreach (var item in res)
                        {
                            _db.tbl_role_mapping.Remove(item);
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                        clsRole.message = "Success";
                        lstRoleMap.Add(clsRole);
                    }
                }
                return lstRoleMap;
            }
        }

        public List<clsAddUserMapping> GetUserDataByKGIDNumberBLL(dynamic KGIDNumber)
        {
            tbl_user_master ExistKGIDNumber = new tbl_user_master();
            int KGIDNum = 0; string KGIDStr = ""; var res = new List<clsAddUserMapping>();
            KGIDStr = KGIDNumber[0];
            if (KGIDStr.Length <= 7)
            {
                int? KGIDStrexistdata = Convert.ToInt32(KGIDStr);
                 ExistKGIDNumber = _db.tbl_user_master.Where(a => a.um_kgid_number == KGIDStrexistdata ).FirstOrDefault();
            }
            else {
                 ExistKGIDNumber = _db.tbl_user_master.Where(a => a.um_mobile_no == KGIDStr).FirstOrDefault();
            }
            if (ExistKGIDNumber != null && ExistKGIDNumber.um_is_active == true)
            {
                if (int.TryParse(KGIDNumber[0], out KGIDNum) && KGIDStr.Length <= 7)
                {

                    KGIDNum = Convert.ToInt32(KGIDNumber[0]);// KGIDNumber is int ? tum.um_kgid_number == KGIDNum :
                    res = (from tum in _db.tbl_user_master
                           where tum.um_kgid_number == KGIDNum && tum.um_is_active == true
                           select new clsAddUserMapping
                           {
                               KGIDUserID = tum.um_kgid_number.ToString(),
                               EEName = tum.um_name,
                               EEFatherName = tum.um_father_name,
                               phoneNum = tum.um_mobile_no,
                               email = tum.um_email_id,
                               userId = tum.um_id,
                               pwd = tum.um_password,
                               um_gender = tum.um_gender,
                               DateOfBirth = SqlFunctions.DatePart("day", tum.um_dob) + "-" + SqlFunctions.DatePart("m", tum.um_dob) + "-" + SqlFunctions.DatePart("year", tum.um_dob),
                               message ="0",
                           }).AsQueryable().ToList();
                }
                else
                {
                    res = (from tum in _db.tbl_user_master
                           where tum.um_mobile_no == KGIDStr && tum.um_is_active == true
                           select new clsAddUserMapping
                           {
                               KGIDUserID = tum.um_mobile_no,
                               EEName = tum.um_name,
                               EEFatherName = tum.um_father_name,
                               phoneNum = tum.um_mobile_no,
                               email = tum.um_email_id,
                               userId = tum.um_id,
                               pwd = tum.um_password,
                               DateOfBirth = SqlFunctions.DatePart("day", tum.um_dob) + "-" + SqlFunctions.DatePart("m", tum.um_dob) + "-" + SqlFunctions.DatePart("year", tum.um_dob),
                               message = "0",

                           }).AsQueryable().ToList();
                }
                return res;
            }
            else if (ExistKGIDNumber != null && ExistKGIDNumber.um_is_active == false)
            {
                clsAddUserMapping clsAddUser = new clsAddUserMapping();
                clsAddUser.message = "1";
                res.Add(clsAddUser);
                return res;
            }
            else
            {
                clsAddUserMapping clsAddUser = new clsAddUserMapping();
                clsAddUser.message = "2";
                res.Add(clsAddUser);
                return res;
            }
        }


        public List<clsAddUserMapping> GetUserMappingDataDLL(int userId, string value)
        {
            if (value != "DeActive")
            {
                var roles = (from tum in _db.tbl_user_master
                             join tumap in _db.tbl_user_mapping on tum.um_id equals tumap.um_id
                             join trm in _db.tbl_role_master on tumap.role_id equals trm.role_id
                             //join trmap in _db.tbl_role_mapping on trm.role_id equals trmap.role_id
                             join tmr in _db.MenuRoles on tumap.user_map_id equals tmr.UserMap_Id
                             join tsm in _db.MenuDetails on tmr.MenuId equals tsm.Id
                             join tdm in _db.tbl_division_master on tum.um_div_id equals tdm.division_id
                             join tlm in _db.tbl_LevelMaster on trm.role_Level equals tlm.LevelId
                             //join twm in _db.tbl_wing_master on trmap.rm_wing_id equals twm.Id
                             //join twm in _db.tbl_wing_master on trmap.rm_wing_id equals twm.Id into twm1
                             //from twm in twm1.DefaultIfEmpty()

                             where (userId != 0 ? tum.um_id == userId : true) && tsm.ParentMenu == 8
                             orderby tum.um_creation_datetime descending
                             select new clsAddUserMapping
                             {
                                 KGIDUserID = tum.um_kgid_number != null ? tum.um_kgid_number.ToString() : tum.um_name,
                                 EEName = tum.um_name,
                                 EEFatherName = tum.um_father_name,
                                 phoneNum = tum.um_mobile_no,
                                 email = tum.um_email_id,
                                 userId = tum.um_id,
                                 userMapId = tumap.user_map_id,
                                 pwd = tum.um_password,
                                 DateOfBirth = SqlFunctions.DatePart("day", tum.um_dob) + "-" + SqlFunctions.DatePart("m", tum.um_dob) + "-" + SqlFunctions.DatePart("year", tum.um_dob),

                                 lstRoleMapping = new List<clsRoleMapping>() {
                                 new clsRoleMapping
                                 {
                                     UnitId = (int)trm.role_Level,
                                     //WingId = trmap.rm_wing_id,
                                     SectionId = (int)tsm.Id,
                                     RoleId = tumap.role_id,
                                     LocationDivId = (int)tum.um_div_id,

                                     UnitName = tlm.Name,
                                     //WingName = (trmap.rm_wing_id != 0 ? twm.WingName : "NA"),
                                     SectionName = tsm.MenuName,
                                     RoleName = trm.role_description,
                                     LocationDivName = tdm.division_name
                                 }
                             },
                                 IsActive = tmr.IsActive
                             }).AsQueryable().ToList();

                foreach (var item in roles)
                {
                    int roleId = item.lstRoleMapping[0].RoleId;
                    int sectionId = item.lstRoleMapping[0].SectionId;
                    var wingData = _db.tbl_role_mapping.Where(a => a.role_id == roleId && a.dept_id == sectionId).Join(_db.tbl_wing_master, x => x.rm_wing_id, y => y.Id, (x, y) => new { y.Id, y.WingName }).Select(a => new { a.Id, a.WingName }).FirstOrDefault();
                    if (wingData != null)
                    {
                        item.lstRoleMapping[0].WingId = wingData.Id;
                        item.lstRoleMapping[0].WingName = wingData.WingName;
                    }
                }



                //roles = roles.GroupBy(a => new { a.lstRoleMapping[0].RoleId }).Select(z => new clsAddUserMapping
                //{
                //    //userId = z.Key.userId,
                //    lstRoleMapping = new List<clsRoleMapping>
                //    {
                //        new clsRoleMapping
                //        {
                //            RoleId = z.Key.RoleId,
                //            //SectionId = z.Key.SectionId,
                            
                //            SectionId = z.Select(a=> a.lstRoleMapping.Select(c=> c.SectionId).FirstOrDefault()).FirstOrDefault(),
                //            UnitId = z.Select(a=> a.lstRoleMapping.Select(c=> c.UnitId).FirstOrDefault()).FirstOrDefault(),
                //            WingId = z.Select(a=> a.lstRoleMapping.Select(c=> c.WingId).FirstOrDefault()).FirstOrDefault(),
                //            LocationDivId = z.Select(a=> a.lstRoleMapping.Select(c=> c.LocationDivId).FirstOrDefault()).FirstOrDefault(),
                            
                //            UnitName = z.Select(a=> a.lstRoleMapping.Select(c=> c.UnitName).FirstOrDefault()).FirstOrDefault(),
                //            WingName = z.Select(a=> a.lstRoleMapping.Select(c=> c.WingName).FirstOrDefault()).FirstOrDefault(),
                //            SectionName = z.Select(a=> a.lstRoleMapping.Select(c=> c.SectionName).FirstOrDefault()).FirstOrDefault(),
                //            RoleName = z.Select(a=> a.lstRoleMapping.Select(c=> c.RoleName).FirstOrDefault()).FirstOrDefault(),
                //            LocationDivName = z.Select(a=> a.lstRoleMapping.Select(c=> c.LocationDivName).FirstOrDefault()).FirstOrDefault(),
                //        }
                //    },

                //    KGIDUserID = z.Select(b => b.KGIDUserID).FirstOrDefault(),
                //    EEName = z.Select(b => b.EEName).FirstOrDefault(),
                //    phoneNum = z.Select(b => b.phoneNum).FirstOrDefault(),
                //    email = z.Select(b => b.email).FirstOrDefault(),

                //    pwd = z.Select(b => b.pwd).FirstOrDefault(),
                //    DateOfBirth = z.Select(b => b.DateOfBirth).FirstOrDefault(),
                //    //lstRoleMapping = z.Select(b => b.lstRoleMapping).FirstOrDefault(),
                //    userId = z.Select(b => b.userId).FirstOrDefault(),
                //    userMapId = z.Select(b => b.userMapId).FirstOrDefault(),
                //    IsActive = z.Select(b => b.IsActive).FirstOrDefault(),

                //}).Distinct().ToList();
                //var roles1 = new List<clsAddUserMapping>();
                //foreach (var item in roles)
                //{
                //    if (item.lstRoleMapping.Select(a=> a.RoleId).FirstOrDefault() != roles1.Select(b=> b.lstRoleMapping.Select(a=> a.RoleId).FirstOrDefault()).FirstOrDefault()
                //        && item.lstRoleMapping.Select(a => a.SectionId).FirstOrDefault() != roles1.Select(b => b.lstRoleMapping.Select(a => a.SectionId).FirstOrDefault()).FirstOrDefault())
                //    {
                //        roles1.Add(item);
                //    }
                //}

                //roles = roles.GroupBy(a => a.userId).Select(z => new clsAddUserMapping
                //{
                //    //lstRoleMapping = new List<clsRoleMapping>
                //    //{
                //    //    new clsRoleMapping
                //    //    {
                //    //        UnitId = z.Select(a=> a.lstRoleMapping.Select(c=> c.UnitId).FirstOrDefault()).FirstOrDefault(),
                //    //        RoleId = z.Key.Select(a=> a.RoleId).FirstOrDefault(),
                //    //        SectionId = z.Key.Select(a=> a.SectionId).FirstOrDefault(),
                //    //        LocationDivId = z.Select(a=> a.lstRoleMapping.Select(c=> c.LocationDivId).FirstOrDefault()).FirstOrDefault(),

                //    //        UnitName = z.Select(a=> a.lstRoleMapping.Select(c=> c.UnitName).FirstOrDefault()).FirstOrDefault(),
                //    //        SectionName = z.Select(a=> a.lstRoleMapping.Select(c=> c.SectionName).FirstOrDefault()).FirstOrDefault(),
                //    //        RoleName = z.Select(a=> a.lstRoleMapping.Select(c=> c.RoleName).FirstOrDefault()).FirstOrDefault(),
                //    //        LocationDivName = z.Select(a=> a.lstRoleMapping.Select(c=> c.LocationDivName).FirstOrDefault()).FirstOrDefault(),
                //    //    }

                //    //},
                //    userId = z.Key,
                //    KGIDUserID = z.Select(b => b.KGIDUserID).FirstOrDefault(),
                //    EEName = z.Select(b => b.EEName).FirstOrDefault(),
                //    phoneNum = z.Select(b => b.phoneNum).FirstOrDefault(),
                //    email = z.Select(b => b.email).FirstOrDefault(),

                //    pwd = z.Select(b => b.pwd).FirstOrDefault(),
                //    DateOfBirth = z.Select(b => b.DateOfBirth).FirstOrDefault(),
                //    lstRoleMapping = z.Select(b => b.lstRoleMapping).FirstOrDefault(),
                //    IsActive = z.Select(b => b.IsActive).FirstOrDefault(),
                //}).ToList();

                return roles;
            }
            else
            {
                List<clsAddUserMapping> lstclsRole = new List<clsAddUserMapping>();
                using (var transaction = _db.Database.BeginTransaction())
                {
                    var res = _db.tbl_user_mapping.Where(a => a.um_id == userId).ToList();
                    if (res.Count > 0)
                    {
                        foreach (var item in res)
                        {
                            _db.tbl_user_mapping.Remove(item);
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                        clsAddUserMapping clsRole = new clsAddUserMapping();
                        clsRole.message = "Success";
                        lstclsRole.Add(clsRole);
                    }
                    return lstclsRole;
                }
            }
        }


        public List<SelectListItem> GetSectionsListDLL(int UnitId)
        {
            var res = new List<SelectListItem>();
            if (UnitId == 0)
            {
                res = (from a in _db.MenuDetails
                           where a.IsActive == true && a.ParentMenu == (int)CmnClass.MenuList.Modules
                           select new SelectListItem
                           {
                               Text = a.MenuName,
                               Value = a.Id.ToString()
                           }).AsQueryable().OrderBy(a => a.Text).ToList();

            }
            else
            {
                res = (from a in _db.MenuDetails
                           join trmap in _db.tbl_role_mapping on a.Id equals trmap.dept_id into trmap1
                           from trmap in trmap1.DefaultIfEmpty()
                           join trm in _db.tbl_role_master on trmap.role_id equals trm.role_id

                           where a.IsActive == true && a.ParentMenu == (int)CmnClass.MenuList.Modules
                           && (UnitId != 0 ? trm.role_Level == UnitId : true)
                           select new SelectListItem
                           {
                               Text = a.MenuName,
                               Value = a.Id.ToString()
                           }).AsQueryable().Distinct().OrderBy(a => a.Text).ToList();
            }

            return res;
        }

        public List<SelectListItem> GetWingsListDLL()
        {
            var res = (from a in _db.tbl_wing_master
                       where a.IsActive == true
                       select new SelectListItem
                       {
                           Text = a.WingName,
                           Value = a.Id.ToString()
                       }).AsQueryable().OrderBy(a => a.Text).ToList();
            return res;
        }


        public List<SelectListItem> GetRolesListDLL(int unitId, int sectionId)
        {
            var res = GetRolesDataDLL("", "", unitId, sectionId);
            return res.Select(a => new SelectListItem { Text = a.RoleName, Value = a.RoleId.ToString() }).ToList();
        }

        public List<tbl_role_master> GetRoles(string roleName)
        {
            var role = _db.tbl_role_master.Where(a => (roleName != "" ? a.role_description.Replace(" ", "").Trim().ToUpper().StartsWith(roleName.Replace(" ", "").Trim().ToUpper()) || roleName.Replace(" ", "").Trim().ToUpper().StartsWith(a.role_description.Replace(" ", "").Trim().ToUpper()) : true)).Select(a => new { a.role_seniority_no, a.role_description}).FirstOrDefault();
            if (role != null)
            {
                return _db.tbl_role_master.Where(a => a.role_seniority_no == role.role_seniority_no && (a.role_description.Replace(" ", "").Trim().ToUpper().StartsWith(roleName.Replace(" ", "").Trim().ToUpper()) || roleName.Replace(" ", "").Trim().ToUpper().StartsWith(a.role_description.Replace(" ", "").Trim().ToUpper())) && a.role_is_active == true && a.role_Level.ToString() != null).ToList();
            }
            return null;
        }

        public Tuple<string, bool> UploadImageFile(clsRoleMapping clsRole, List<clsEditProfile> lstEditProfile, string path)
        {
            var res = "";
            bool IsPermissionGranted = false;
            if (lstEditProfile[0].ImageFile != null)
            {
                if (lstEditProfile[0].ImageFile.ContentLength == 0)
                {
                    res = "Please upload File";
                }
                else if (lstEditProfile[0].ImageFile.ContentLength > 0)
                {
                    string FileNameToCreate = "_UploadPhoto";
                    string fileName = lstEditProfile[0].ImageFile.FileName; // getting File Name

                    fileName = Path.GetFileNameWithoutExtension(lstEditProfile[0].ImageFile.FileName);

                    string fileContentType = lstEditProfile[0].ImageFile.ContentType; // getting ContentType
                    byte[] tempFileBytes = new byte[lstEditProfile[0].ImageFile.ContentLength]; // getting filebytes
                    var data = lstEditProfile[0].ImageFile.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(lstEditProfile[0].ImageFile.ContentLength));
                    var types = Common.FileUtility.FileType.Image;  // Setting Image type
                                                                    //  ImageFileExtension imageFileExtension;
                    string imageFile1, imageFile11;
                    bool isvalid1, isvalid11;
                    Common.FileUtility.isValidFile(tempFileBytes, types, fileContentType, out imageFile1, out isvalid1); // Validate Header

                    Common.FileUtility.isValidImageFile(tempFileBytes, fileContentType, out imageFile11, out isvalid11);
                    if (isvalid1 == true)
                    {
                        long fileSizeibKbs = lstEditProfile[0].ImageFile.ContentLength / 1024;

                        //int maxFileLength = 1024 * 3000; //FileLength 3 MB 
                        //if (lstEditProfile[0].ImageFile.ContentLength > maxFileLength)
                        //{
                        //    return res = "Size of pdf Upload file size exceeded max file upload size(3 MB)!";
                        //}
                        if (fileSizeibKbs > 200)
                        {
                            res = "Size of JPG/PNG Upload file size exceeded max file upload size(200 KB)!";
                        }
                        else if (!Enum.IsDefined(typeof(Common.FileUtility.ImageFileExtension), imageFile1))
                        {
                            res = "Please upload only image file!";
                        }
                        else if (!Enum.IsDefined(typeof(Common.FileUtility.ImageFileExtension), imageFile11))
                        {
                            res = "Please upload only image file!";
                        }
                        else if (imageFile11 == "")
                        {
                            res = "Please upload only image file!";
                        }
                        else
                        { }
                        Match regex = Regex.Match(fileName, @"[\[\]{}!@#.]");

                        if (regex.Success)
                        {
                            res = "Please check uploaded file name!";
                        }
                        char[] invalidFileChars = Path.GetInvalidFileNameChars();
                        Common.FileUtility.ShowChars(invalidFileChars);

                        string UniqueFileName = null;
                        UniqueFileName = FileNameToCreate + "." + imageFile11;

                        string _path = Path.Combine(path, UniqueFileName);
                        string _pathCreate = Path.Combine(path);

                        FileInfo CheckFileName = new FileInfo(_path);

                        if (!Directory.Exists(_pathCreate))
                        {
                            Directory.CreateDirectory(_pathCreate);
                        }
                        if (CheckFileName.Exists)
                        {
                            FileNameToCreate = FileNameToCreate + "_" + DateTime.Now.Ticks;
                            UniqueFileName = FileNameToCreate + "." + imageFile11;
                        }

                        //IsPermissionGranted = Common.FileUtility.GrantFilePermission();
                        IsPermissionGranted = true;

                        lstEditProfile[0].ImageFile.SaveAs(_path);
                        lstEditProfile[0].Photo = "Content/Uploads/" + UniqueFileName;
                    }
                    else
                    {
                        res = "Please check Uploaded file";
                    }
                }
            }
            else
            {
                if (clsRole.UnitId != 0 && lstEditProfile[0].status != null)
                {
                    IsPermissionGranted = true;
                }
                else if (clsRole.UnitId == 0 && lstEditProfile[0].status == null)
                {
                }
                else
                {
                    res = "Please check Uploaded file";
                }
            }
            return new Tuple<string, bool>(res, IsPermissionGranted);
        }



        private int? GetSeniorityRoleValue(string roleName)
        {
            int? role_sen_level = 0;
            if (roleName != null)
            {
                role_sen_level = _db.tbl_role_master.Where(a => a.role_description.Replace(" ", "").Trim().ToUpper() == roleName.Replace(" ", "").Trim().ToUpper()).Select(a => a.role_seniority_no).FirstOrDefault();
            }
            return role_sen_level;

            //switch (str.Replace(" ", "").Trim().ToUpper())
            //{
            //    case "COMMISSIONER":    // Make all cases in Capital letters for appropriate string comparison.
            //        return (int)CmnClass.seniorityRole.Commissioner;
            //    case "DIRECTOR":
            //        return (int)CmnClass.seniorityRole.Director;
            //    case "ADDITIONALDIRECTOR":
            //        return (int)CmnClass.seniorityRole.AdditionalDirector;
            //    case "JOINTDIRECTOR":
            //        return (int)CmnClass.seniorityRole.JointDirector;
            //    case "ASSISTANTDIRECTOR":
            //        return (int)CmnClass.seniorityRole.AssistantDirector;
            //    case "DEPUTYDIRECTOR":
            //        return (int)CmnClass.seniorityRole.DeputyDirector;
            //    case "OFFICESUPERITENDENT":
            //        return (int)CmnClass.seniorityRole.OfficeSuperitendent;
            //    case "CASEWORKER":
            //        return (int)CmnClass.seniorityRole.CaseWorker;
            //    case "ITIADMIN":
            //        return (int)CmnClass.seniorityRole.ITIAdmin;
            //    case "APPLICANT":
            //        return (int)CmnClass.seniorityRole.Applicant;
            //    case "DIVISIONOFFICER":
            //        return (int)CmnClass.seniorityRole.DivisionOfficer;
            //    case "VERIFICATIONOFFICER":
            //        return (int)CmnClass.seniorityRole.VerificationOfficer;
            //    case "ADMISSIONOFFICER":
            //        return (int)CmnClass.seniorityRole.AdmissionOfficer;
            //    default:
            //        return 0;
            //}
        }

    }
}