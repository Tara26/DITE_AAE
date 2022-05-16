using DLL.DBConnection;
using Models;
using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.OfficerMapping
{
    public class OfficerMapping : IOfficerMapping
    {
        private readonly DbConnection _db = new DbConnection();

        public List<VerificationOfficer> GetOfficers(int loginId, int roleId)
        {
            try
            {
                var res = (from aa in _db.tbl_VerificationOfficer_Master
                           join um in _db.tbl_user_master on aa.UserMasterId equals um.um_id
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join cc in _db.Staff_Institute_Detail on bb.iti_college_id equals cc.InstituteId
                           where cc.UserId == loginId && aa.OfficerRoleId == roleId
                           select new VerificationOfficer
                           {
                               OfficerId = aa.Officer_Id,
                               OfficerName = aa.Name,
                               Designation = aa.Designation,
                               PhoneNo = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Status = aa.IsActive,
                               KGIDNo=um.um_kgid_number
                           }
                           ).Distinct().ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VerificationOfficer> GetInstituteId(int loginId)
        {
            try
            {
                var res = (from bb in _db.tbl_iti_college_details
                               //where bb.Officer_Id == id
                           select new VerificationOfficer
                           {
                               InstituteId = bb.iti_college_id,
                               InstituteName = bb.iti_college_name
                           }
                      ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddOfficer(VerificationOfficer officer, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_VerificationOfficer_Master.Where(x => x.MobileNum == officer.PhoneNo).FirstOrDefault();
                    int collegeId = GetCollegeId(loginId);
                    var update_query = _db.tbl_user_master.Where(s => s.um_kgid_number == officer.KGIDNo).FirstOrDefault();
                    if (res == null)
                    {
                        
                            if (_db.tbl_user_master.Where(a => a.um_kgid_number == officer.KGIDNo).ToList().Count == 0)
                            {
                                tbl_user_master objtbl_user_master = new tbl_user_master();
                           // objtbl_user_master.um_name = officer.OfficerLoginUserName;

                            objtbl_user_master.um_name = officer.OfficerName;
                                objtbl_user_master.um_password = officer.OfficerLoginPwd;
                                objtbl_user_master.um_mobile_no = officer.PhoneNo;
                                objtbl_user_master.um_email_id = officer.EmailId;
                                objtbl_user_master.um_is_active = true;
                                objtbl_user_master.um_creation_datetime = DateTime.Now;
                                objtbl_user_master.um_created_by = loginId;
                                objtbl_user_master.um_kgid_number = officer.KGIDNo;

                                _db.tbl_user_master.Add(objtbl_user_master);
                                _db.SaveChanges();
                            }
                        else
                        {
                            //update_query.um_name = officer.OfficerLoginUserName;

                            update_query.um_name = officer.OfficerName;
                            update_query.um_mobile_no = officer.PhoneNo;
                            update_query.um_email_id = officer.EmailId;
                            
                            update_query.um_creation_datetime = DateTime.Now;
                            update_query.um_created_by = loginId;
                            update_query.um_kgid_number = officer.KGIDNo;

                           
                            _db.SaveChanges();
                        }
                            int ExistingRecordForUpdate = (from tum in _db.tbl_user_master
                                                           orderby tum.um_creation_datetime descending
                                                           select tum.um_id).Take(1).FirstOrDefault();

                            tbl_VerificationOfficer_Master offic = new tbl_VerificationOfficer_Master();
                            offic.Name = officer.OfficerName;
                            offic.Designation = officer.Designation;
                            offic.OfficerLoginUserName = officer.OfficerLoginUserName;
                            //offic.OfficerLoginPwd = officer.OfficerLoginPwd;
                            offic.MobileNum = officer.PhoneNo;
                            offic.EmailId = officer.EmailId;
                            offic.InstituteId = collegeId;
                            offic.IsActive = true;
                            offic.CreatedBy = loginId;
                            offic.UserMasterId = ExistingRecordForUpdate;
                            offic.OfficerRoleId = officer.RoleId;
                            offic.CreatedOn = DateTime.Now;
                            _db.tbl_VerificationOfficer_Master.Add(offic);
                            _db.SaveChanges();

                            tbl_user_mapping objtbl_user_mapping = new tbl_user_mapping();
                            objtbl_user_mapping.role_id = officer.RoleId;
                            objtbl_user_mapping.um_id = ExistingRecordForUpdate;
                            objtbl_user_mapping.dept_id = 1;
                            objtbl_user_mapping.sub_dept_id = 1;
                            objtbl_user_mapping.is_active = true;
                            objtbl_user_mapping.created_by = 1;
                            objtbl_user_mapping.creation_datetime = DateTime.Now;
                            _db.tbl_user_mapping.Add(objtbl_user_mapping);

                            _db.SaveChanges();

                            int UserMappingIDForRoles = (from tum in _db.tbl_user_mapping
                                                         orderby tum.creation_datetime descending
                                                         select tum.user_map_id).Take(1).FirstOrDefault();

                            int[] menuIDToInsertMenuRoles = { /*8,*/ 10, /*40,*/ 47 }; //Adding Menus id for user mapping
                            if (officer.RoleId == (int)CmnClass.Role.AdmOff)
                            {
                                menuIDToInsertMenuRoles = new int[] {/* 8,*/ 10, 60 };
                            }

                            foreach (var id in menuIDToInsertMenuRoles)
                            {
                                MenuRoles objMenuRoles = new MenuRoles();
                                objMenuRoles.MenuId = id;
                                objMenuRoles.UserMap_Id = UserMappingIDForRoles;
                                objMenuRoles.Created_On = DateTime.Now;
                                objMenuRoles.Created_By = loginId;
                                objMenuRoles.IsActive = true;
                                _db.MenuRoles.Add(objMenuRoles);
                                _db.SaveChanges();
                            }

                        
                        transaction.Commit();
                        return "success";
                    }
                    
                    else
                    {
                        return "exist";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public VerificationOfficer EditOfficer(int id)
        {
            try
            {
                var res = (from aa in _db.tbl_VerificationOfficer_Master
                           join bb in _db.tbl_user_master on aa.UserMasterId equals bb.um_id
                           where aa.Officer_Id == id
                           select new VerificationOfficer
                           {
                               OfficerId = aa.Officer_Id,
                               OfficerName = aa.Name,
                               OfficerLoginUserName = aa.OfficerLoginUserName,
                               //OfficerLoginPwd = aa.OfficerLoginPwd,
                               Designation = aa.Designation,
                               PhoneNo = aa.MobileNum,
                               EmailId = aa.EmailId,
                               Status = aa.IsActive,
                               KGIDNo=bb.um_kgid_number
                           }
                      ).ToList();
                res[0].OfficerLoginPwd = _db.tbl_VerificationOfficer_Master.Where(a => a.Officer_Id == id)
                    .Join(_db.tbl_user_master, x => x.UserMasterId, y => y.um_id, (x, y) => new { y.um_password }).Select(a=> a.um_password).FirstOrDefault();
                return res[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateOfficer(VerificationOfficer officer, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_VerificationOfficer_Master.Where(x => x.Officer_Id == officer.OfficerId).SingleOrDefault();
                    var phon = _db.tbl_VerificationOfficer_Master.Where(x => x.MobileNum == officer.PhoneNo).FirstOrDefault();
                    if (res != null)
                    {
                        res.Name = officer.OfficerName;
                        res.Designation = officer.Designation;
                        res.OfficerLoginUserName = officer.OfficerLoginUserName;
                        //res.OfficerLoginPwd = officer.OfficerLoginPwd;
                        if (phon == null)
                            res.MobileNum = officer.PhoneNo;
                        res.EmailId = officer.EmailId;
                        res.IsActive = officer.Status;
                        res.OfficerRoleId = officer.RoleId;
                       
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public bool DeleteOfficer(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_VerificationOfficer_Master.Where(x => x.Officer_Id == id).SingleOrDefault();
                    if (res != null)
                    {
                        _db.tbl_VerificationOfficer_Master.Remove(res);
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<VerificationOfficer> GetApplicants(int loginId, int applicantId, int year)
        {
            if (year != 0)
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                year = Convert.ToInt32(yr);
            }
            try
            {
                int collegeId = GetCollegeId(loginId);
                if (applicantId==0)
                {
                    var res = (from aa in _db.tbl_Applicant_Detail
                               join bb in _db.tbl_Gender on aa.Gender equals bb.Gender_Id into gendermast
                               from GenderMaster in gendermast.DefaultIfEmpty()
                               //join bb in _db.tbl_Gender on aa.Gender equals bb.Gender_Id
                               //join cc in _db.tbl_iti_college_details on aa.DocVerificationCentre equals cc.iti_college_id
                               join cc in _db.tbl_iti_college_details on aa.DocVerificationCentre equals cc.iti_college_id into ITIColDetails
                               from iticol in ITIColDetails.DefaultIfEmpty()
                               join dd in _db.tbl_course_type_mast on
                               iticol.CourseCode equals dd.course_id into courcetype
                               from cttypedetails in courcetype.DefaultIfEmpty()
                               join AM in _db.tbl_ApplicationMode on aa.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               where aa.DocVerificationCentre == collegeId  && aa.IsActive==true && aa.ApplicationMode != 3  // ApplicationMode 3 means Private
                               && aa.ApplDescStatus == 1
                               select new VerificationOfficer
                               {
                                   ApplicationId = aa.ApplicationId,
                                   ApplicantNumber = aa.ApplicantNumber,
                                   ApplicantName = aa.ApplicantName,
                                   //change to display year name
                                   Session=_db.tbl_Year.Where(a => a.Year.Contains(aa.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   //Year = aa.ApplyYear,
                                   CourseType = cttypedetails.course_type_name,
                                   EmailId = aa.EmailId,
                                   Gender = GenderMaster.Gender,
                                   Applydate = aa.CreatedOn,
                                   StatusName = aa.ApplStatus == 10 ? "Verified" : "Not Verified",
                                   MobileNumber=aa.PhoneNumber,
                                   PaymentOptionval=aa.PaymentOptionval,
                                   ApplicationMode = AMDetails.ApplicationMode
                               }).OrderByDescending(x => x.ApplicationId).ToList();
                    foreach (var p in res)
                    {
                        if (!string.IsNullOrEmpty(p.ApplicantNumber) && p.ApplicantNumber != "NULL")
                        {
                            DateTime date = (DateTime)p.Applydate;
                            p.Apdate = date.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            p.Apdate = " Pending";
                        }
                    }
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_Applicant_Detail
                               join bb in _db.tbl_Gender on aa.Gender equals bb.Gender_Id into gendermast
                               from GenderMaster in gendermast.DefaultIfEmpty()
                                   //join bb in _db.tbl_Gender on aa.Gender equals bb.Gender_Id
                                   //join cc in _db.tbl_iti_college_details on aa.DocVerificationCentre equals cc.iti_college_id
                               join cc in _db.tbl_iti_college_details on aa.DocVerificationCentre equals cc.iti_college_id into ITIColDetails
                               from iticol in ITIColDetails.DefaultIfEmpty()
                               join dd in _db.tbl_course_type_mast on
                               iticol.CourseCode equals dd.course_id into courcetype
                               from cttypedetails in courcetype.DefaultIfEmpty()
                               join AM in _db.tbl_ApplicationMode on aa.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               where aa.DocVerificationCentre == collegeId && aa.IsActive == true && aa.ApplicationMode != 3  // ApplicationMode 3 means Private
                                && aa.ApplDescStatus == 1
                               select new VerificationOfficer
                               {
                                   ApplicationId = aa.ApplicationId,
                                   ApplicantNumber = aa.ApplicantNumber,
                                   ApplicantName = aa.ApplicantName,
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(aa.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   MobileNumber = aa.PhoneNumber,
                                   //Year = aa.ApplyYear,
                                   CourseType = cttypedetails.course_type_name,
                                   EmailId = aa.EmailId,
                                   Gender = GenderMaster.Gender,
                                   Applydate = aa.CreatedOn,
                                   StatusName = aa.ApplStatus == 10 ? "Verified" : "Not Verified",
                                   ApplicationMode = AMDetails.ApplicationMode
                               }).OrderByDescending(x => x.ApplicationId).ToList();
                    foreach (var p in res)
                    {
                        if (!string.IsNullOrEmpty(p.ApplicantNumber) && p.ApplicantNumber != "NULL")
                        {
                            DateTime date = (DateTime)p.Applydate;
                            p.Apdate = date.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            p.Apdate = " Pending ";
                        }
                    }
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VerificationOfficer> GetVerificationOfficerDetails(int loginId, int year, int courseType, int roleId)
        {
            try
            {
                if (year != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                    year = Convert.ToInt32(yr);
                }
                if (year == 0)
                {
                    int collegeId = GetCollegeId(loginId);
                    var res = (from aa in _db.tbl_VerOfficer_Applicant_Mapping
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_VerificationOfficer_Master on aa.VerifiedOfficer equals cc.Officer_Id
                               join dd in _db.tbl_iti_college_details on bb.DocVerificationCentre equals dd.iti_college_id
                               join ee in _db.tbl_course_type_mast on dd.CourseCode equals ee.course_id
                               from ff in _db.tbl_SeatAllocationDetail_Seatmatrix.Where(z => z.ApplicantId == bb.ApplicationId).DefaultIfEmpty()
                               join AM in _db.tbl_ApplicationMode on bb.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               join tsm in _db.tbl_ApplicationFormDescStatus on bb.ApplDescStatus equals tsm.ApplicationFormDescStatus_id into gj
                               from tsm in gj.DefaultIfEmpty()
                               where (roleId == (int)CmnClass.Role.AdmOff ? ff.InstituteId == collegeId : bb.DocVerificationCentre == collegeId && bb.ApplStatus != 10)
                               && cc.Officer_Id == aa.VerifiedOfficer && cc.OfficerRoleId == roleId && bb.ApplicantNumber !="" && bb.DocumentFeeReceiptDetails!="" && bb.IsActive == true
                               select new VerificationOfficer
                               {
                                   ApplicationId = bb.ApplicationId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   PhoneNo = bb.PhoneNumber,
                                   OfficerName = cc.Name,
                                   Designation = cc.Designation,
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),//bb.ApplyYear,
                                   CourseType = ee.course_type_name,
                                   //StatusName = "Not verified",
                                   StatusName = tsm.ApplDescription,
                                   ApplicationMode = AMDetails.ApplicationMode

                               }
                               ).ToList();
                    return res;
                }
                else
                {
                    int collegeId = GetCollegeId(loginId);
                    var res = (from aa in _db.tbl_VerOfficer_Applicant_Mapping
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_VerificationOfficer_Master on aa.VerifiedOfficer equals cc.Officer_Id
                               join dd in _db.tbl_iti_college_details on bb.DocVerificationCentre equals dd.iti_college_id
                               join ee in _db.tbl_course_type_mast on dd.CourseCode equals ee.course_id
                               join AM in _db.tbl_ApplicationMode on bb.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               join tsm in _db.tbl_ApplicationFormDescStatus on bb.ApplDescStatus equals tsm.ApplicationFormDescStatus_id into gj
                               from tsm in gj.DefaultIfEmpty()
                               where bb.DocVerificationCentre == collegeId && bb.ApplyYear==year && dd.CourseCode==courseType && bb.ApplicantNumber != "" && bb.DocumentFeeReceiptDetails != "" && bb.IsActive == true
                               select new VerificationOfficer
                               {
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   PhoneNo = bb.PhoneNumber,
                                   OfficerName = cc.Name,
                                   Designation = cc.Designation,
                                   Year = bb.ApplyYear,
                                   CourseType = ee.course_type_name,
                                   StatusName = tsm.ApplDescription,
                                   //StatusName = bb.ApplStatus == 10 ? "Verified" : "Not verified",
                                   PaymentOptionval =bb.PaymentOptionval,
                                   ApplicationMode = AMDetails.ApplicationMode


                               }
                               ).ToList();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VerificationOfficer GetTotalApplicantOfficer(int id, int roleId)
        {
            try
            {
                var res = new VerificationOfficer();
                int collegeId = GetCollegeId(id);
                res.TotalOfficers = _db.tbl_VerificationOfficer_Master.Where(x => x.IsActive == true && x.InstituteId == collegeId && x.OfficerRoleId == roleId).Count();
                //res.TotalApplicants = _db.tbl_Applicant_Detail.Where(x => x.IsActive == true && x.DocVerificationCentre == collegeId).Count();
                var nap = new List<int>();
                if (roleId == (int)CmnClass.Role.AdmOff)
                {
                    var verOff = _db.tbl_VerificationOfficer_Master.Where(a => a.OfficerRoleId == roleId && a.InstituteId == collegeId).Select(a => a.Officer_Id).ToList();
                    nap = _db.tbl_VerOfficer_Applicant_Mapping.Where(a => verOff.Contains(a.VerifiedOfficer)).Select(x => x.ApplicantId).ToList();
                    res.TotalApplicants = _db.tbl_Applicant_ITI_Institute_Detail.Where(a => a.AdmittedStatus == (int)CmnClass.AdmissionStatus.Pending)
                        .Join(_db.tbl_SeatAllocationDetail_Seatmatrix.Where(a => a.InstituteId == collegeId), x => x.ApplicationId, y => y.ApplicantId, (x, y) => new { y.ApplicantId }).Where(a => !(nap.Contains(a.ApplicantId))).Count();
                }
                else
                {
                    nap = _db.tbl_VerOfficer_Applicant_Mapping.Select(x => x.ApplicantId).ToList();
                    res.TotalApplicants = _db.tbl_Applicant_Detail.Where(x => !nap.Contains(x.ApplicationId) && x.DocVerificationCentre == collegeId && x.IsActive == true && x.DocumentFeeReceiptDetails != "" && x.ApplicantNumber != "" && x.AgainstVacancyInd != 1 && x.ApplDescStatus == 1).Count();
                }
                var s = _db.tbl_VerificationOfficer_Master.Where(x => x.IsActive == false && x.InstituteId == collegeId && x.OfficerRoleId == roleId).Select(y => y.Officer_Id).ToList();
                var instit = _db.tbl_VerificationOfficer_Master.Where(x => x.IsActive == false && x.InstituteId == collegeId && x.OfficerRoleId == roleId).Select(y => y.InstituteId).FirstOrDefault();
                var officerIds = s.Select(i => i).ToArray();
                var Ids = string.Join(", ", officerIds);
                res.InactiveOfficerApplicants = (from aa in _db.tbl_VerOfficer_Applicant_Mapping
                                                 join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                                                 where (from ptp in _db.tbl_VerificationOfficer_Master
                                                        where ptp.IsActive == false && bb.AgainstVacancyInd!=1
                                                        select ptp.Officer_Id).Contains(aa.VerifiedOfficer) && bb.DocVerificationCentre == instit && bb.ApplStatus != 10 && bb.IsActive==true
                                                 select new VerificationOfficer
                                                 {
                                                     InactiveOfficerApplicants = aa.ApplicantId
                                                 }).Count();
                return res;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        int GetCollegeId(int loginId)
        {
            try
            {
                int id = _db.Staff_Institute_Detail.Where(x => x.UserId == loginId).Select(y => y.InstituteId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        Applicants GetOfficerApplicantsCount(int collegeId, int roleId)
        {
            var officers = _db.tbl_VerificationOfficer_Master.Where(x => x.IsActive == true && x.InstituteId == collegeId && x.OfficerRoleId == roleId).ToList();
            List<Applicants> list = new List<Applicants>();
            foreach (var off in officers)
            {
                Applicants ss = new Applicants();
                int appcount = _db.tbl_VerOfficer_Applicant_Mapping.Where(x => x.VerifiedOfficer == off.Officer_Id).Count();
                ss.officer = off.Officer_Id;
                ss.applicant = appcount;
                list.Add(ss);
            }            
            list.Sort(delegate (Applicants x, Applicants y) {
                return x.applicant.CompareTo(y.applicant);
            });
            return list[0];
        }
        public bool MapApplicantToOfficer(int id, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int collegeId = GetCollegeId(id);
                    var officers = _db.tbl_VerificationOfficer_Master.Where(x => x.IsActive == true && x.InstituteId == collegeId && x.OfficerRoleId == roleId).ToList();
                    var app = new List<int>();
                    var applicants = new List<tbl_Applicant_Detail>();
                    if (roleId == (int)CmnClass.Role.AdmOff)
                    {
                        app = _db.tbl_Applicant_ITI_Institute_Detail.Where(a => a.AdmittedStatus == (int)CmnClass.AdmissionStatus.Pending)
                        .Join(_db.tbl_SeatAllocationDetail_Seatmatrix.Where(a => a.InstituteId == collegeId), x => x.ApplicationId, y => y.ApplicantId, (x, y) => new { x.ApplicationId }).Select(a => a.ApplicationId).ToList();
                        applicants = _db.tbl_Applicant_Detail.Where(x => app.Contains(x.ApplicationId) && x.IsActive == true ).ToList();
                    }
                    else
                    {
                        app = _db.tbl_VerOfficer_Applicant_Mapping.Select(x => x.ApplicantId).ToList();
                        applicants = _db.tbl_Applicant_Detail.Where(x => !app.Contains(x.ApplicationId) && x.IsActive == true && x.ApplicantNumber != "" && x.DocVerificationCentre == collegeId && x.DocumentFeeReceiptDetails != "" && x.AgainstVacancyInd != 1 && x.ApplDescStatus==1).ToList();
                    }
                    if (applicants.Count != 0)
                    {   
                        for (int i = 0; i < applicants.Count; i++)
                        {
                            var list = GetOfficerApplicantsCount(collegeId, roleId);
                            tbl_VerOfficer_Applicant_Mapping inv = new tbl_VerOfficer_Applicant_Mapping();
                            inv.ApplicantId = applicants[i].ApplicationId;
                            inv.VerifiedOfficer = list.officer;
                            inv.Status = 101;
                            inv.CreatedBy = id;
                            inv.CreatedOn = DateTime.Now;
                            inv.Remarks = "assigned officer";
                            _db.tbl_VerOfficer_Applicant_Mapping.Add(inv);
                            _db.SaveChanges();

                            #region .. Updating in tbl_Applicant_Detail table .. 

                            int tadapplicationId = applicants[i].ApplicationId;

                            var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
                                                  where tad.ApplicationId == tadapplicationId && tad.IsActive == true
                                                  select new ApplicationStatusUpdate { CredatedBy = tad.CredatedBy, ApplDescStatus = tad.ApplDescStatus }).ToList();

                            var resUserMasterId = (from aa in _db.tbl_VerificationOfficer_Master
                                                   where aa.Officer_Id == list.officer && aa.IsActive == true
                                                   select aa.UserMasterId).FirstOrDefault();

                            var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == tadapplicationId && s.IsActive == true).FirstOrDefault();
                            update_query.AssignedVO = resUserMasterId;        //Mapping VO to Applicant table
                            update_query.FlowId = resUserMasterId;
                            update_query.ApplDescStatus = 2; // 2 means  Document verification fee Pending
                            _db.SaveChanges();

                            tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                            objtbl_ApplicantTrans.ApplicantId = tadapplicationId;                             
                            objtbl_ApplicantTrans.FlowId = resUserMasterId;
                            objtbl_ApplicantTrans.Remark = "Verification Officer assigned to the applicant to verify the documents";
                            objtbl_ApplicantTrans.Status = 5;
                            objtbl_ApplicantTrans.ApplDescStatus = 2;
                            objtbl_ApplicantTrans.AssignedVO = resUserMasterId;
                            objtbl_ApplicantTrans.FinalSubmitInd = 1;
                            if (FlowIDExisting.Count > 0)
                            {
                                foreach (var ExistingAssignedVOval in FlowIDExisting)
                                {
                                    //objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                                    objtbl_ApplicantTrans.ApplDescStatus = 2;

                                }
                            }
                            objtbl_ApplicantTrans.CreatedBy = id;
                            objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                            objtbl_ApplicantTrans.TransDate = DateTime.Now;
                            objtbl_ApplicantTrans.IsActive = 1;
                            _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                            _db.SaveChanges();


                            #endregion
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<VerificationOfficer> GetInactiveOfficerApplicants(int loginId, int year, int courseType)
        {
            try
            {
                if (year != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                    year = Convert.ToInt32(yr);
                }
                if (year==0)
                {
                    int collegeId = GetCollegeId(loginId);
                    var res = (from aa in _db.tbl_VerOfficer_Applicant_Mapping
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_VerificationOfficer_Master on aa.VerifiedOfficer equals cc.Officer_Id
                               join dd in _db.tbl_iti_college_details on bb.DocVerificationCentre equals dd.iti_college_id
                               join ee in _db.tbl_course_type_mast on dd.CourseCode equals ee.course_id
                               join AM in _db.tbl_ApplicationMode on bb.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               where bb.DocVerificationCentre == collegeId && cc.IsActive == false && bb.ApplDescStatus != 4 && bb.ApplicantNumber != "" && bb.DocumentFeeReceiptDetails != "" && bb.IsActive==true && bb.AgainstVacancyInd!=1
                               select new VerificationOfficer
                               {
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   OfficerName = cc.Name,
                                   Designation = cc.Designation,
                                   //nrew field to display AUG-2021
                                   Session= _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   //Year = bb.ApplyYear,
                                   CourseType = ee.course_type_name,
                                   MobileNumber=bb.PhoneNumber,
                                   ApplicationMode = AMDetails.ApplicationMode
                               }
                               ).ToList();
                    return res;
                }
                else
                {
                    int collegeId = GetCollegeId(loginId);
                    var res = (from aa in _db.tbl_VerOfficer_Applicant_Mapping
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_VerificationOfficer_Master on aa.VerifiedOfficer equals cc.Officer_Id
                               join dd in _db.tbl_iti_college_details on bb.DocVerificationCentre equals dd.iti_college_id
                               join ee in _db.tbl_course_type_mast on dd.CourseCode equals ee.course_id
                               join AM in _db.tbl_ApplicationMode on bb.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()

                               where bb.DocVerificationCentre == collegeId && cc.IsActive == false && bb.ApplDescStatus != 4 && bb.ApplyYear==year && dd.CourseCode==courseType && bb.ApplicantNumber != "" && bb.DocumentFeeReceiptDetails != "" && bb.IsActive == true
                               && bb.AgainstVacancyInd != 1
                               select new VerificationOfficer
                               {
                                   ApplicantNumber = bb.ApplicantNumber,
                                   ApplicantName = bb.ApplicantName,
                                   OfficerName = cc.Name,
                                   Designation = cc.Designation,
                                   //nrew field to display AUG-2021
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   //Year = bb.ApplyYear,
                                   CourseType = ee.course_type_name,
                                   MobileNumber = bb.PhoneNumber,
                                   ApplicationMode = AMDetails.ApplicationMode
                               }
                               ).ToList();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ReMapApplicantToOfficer(int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int collegeId = GetCollegeId(loginId);                    
                    var applicants = (from a in _db.tbl_VerOfficer_Applicant_Mapping
                                      join b in _db.tbl_Applicant_Detail on a.ApplicantId equals b.ApplicationId
                                      where (from d in _db.tbl_VerificationOfficer_Master
                                             where d.IsActive == false && b.AgainstVacancyInd!=1
                                             select d.Officer_Id).Contains(a.VerifiedOfficer) && b.DocVerificationCentre == collegeId && b.ApplStatus != 10 && b.DocumentFeeReceiptDetails != "" && b.ApplicantNumber != "" && b.IsActive==true
                                      select a.ApplicantId).ToList();
                    if (applicants.Count != 0)
                    {                        
                        for (int i = 0; i < applicants.Count; i++)
                        {
                            var list = GetOfficerApplicantsCount(collegeId, roleId);
                            int appId = applicants[i];
                            var inv = _db.tbl_VerOfficer_Applicant_Mapping.Where(x => x.ApplicantId == appId).FirstOrDefault();
                            inv.VerifiedOfficer = list.officer;
                            inv.Remarks = "Re-assigned officer";
                            _db.SaveChanges();

                            #region .. Updating in tbl_Applicant_Detail table .. 

                            var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
                                                  where tad.ApplicationId == appId && tad.IsActive == true && tad.AgainstVacancyInd != 1
                                                  select new ApplicationStatusUpdate { CredatedBy = tad.CredatedBy, ApplDescStatus = tad.ApplDescStatus }).ToList();

                            var resUserMasterId = (from aa in _db.tbl_VerificationOfficer_Master
                                                   where aa.Officer_Id == list.officer && aa.IsActive == true
                                                   select aa.UserMasterId).FirstOrDefault();

                            var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == appId && s.IsActive == true).FirstOrDefault();
                            update_query.AssignedVO = resUserMasterId;        //Remapping VO to Applicant table
                            update_query.FlowId = resUserMasterId;        //Remapping VO to Applicant table
                            _db.SaveChanges();

                            tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                            objtbl_ApplicantTrans.ApplicantId = appId;
                            objtbl_ApplicantTrans.CreatedBy = loginId;   //Re-Mapping VO to Applicant trans table 
                            if (FlowIDExisting.Count > 0)
                            {
                                foreach (var ExistingAssignedVOval in FlowIDExisting)
                                {
                                    objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                                }
                            }                            
                            objtbl_ApplicantTrans.FlowId = resUserMasterId;
                            objtbl_ApplicantTrans.AssignedVO = resUserMasterId;
                            objtbl_ApplicantTrans.Remark = "Re-mapping the Verification Officer assigned to the applicant to verify the documents";
                            objtbl_ApplicantTrans.FinalSubmitInd = 1;
                            objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                            objtbl_ApplicantTrans.TransDate = DateTime.Now;
                            objtbl_ApplicantTrans.IsActive = 1;
                            _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);

                            _db.SaveChanges();

                            #endregion
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public List<VerificationOfficer> GetActiveOfficers(int loginId, int roleId)
        {
            try
            {
                int collegeId = GetCollegeId(loginId);
                var res = (from bb in _db.tbl_VerificationOfficer_Master
                           where bb.IsActive == true && bb.InstituteId == collegeId && bb.OfficerRoleId == roleId
                           select new VerificationOfficer
                           {
                               OfficerId = bb.Officer_Id,
                               OfficerName = bb.Name
                           }
                      ).ToList();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ReMapApplicantIndividualOff(int loginId, List<Applicants> list)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int collegeId = GetCollegeId(loginId);
                    foreach (var item in list)
                    {
                        var applicants1 = (from a in _db.tbl_VerOfficer_Applicant_Mapping
                                           join b in _db.tbl_Applicant_Detail on a.ApplicantId equals b.ApplicationId
                                           where (from d in _db.tbl_VerificationOfficer_Master
                                                  where d.IsActive == false
                                                  select d.Officer_Id).Contains(a.VerifiedOfficer) && b.DocVerificationCentre == collegeId && b.ApplStatus != 10 && b.DocumentFeeReceiptDetails != "" && b.ApplicantNumber != "" && b.IsActive==true
                                           select a.ApplicantId).Take(item.applicant).ToList();
                        if (item.officer != 0)
                        {
                            if (item.applicant != 0)
                            {
                                foreach (var aa in applicants1)
                                {
                                    int appId = aa;
                                    var inv = _db.tbl_VerOfficer_Applicant_Mapping.Where(x => x.ApplicantId == appId).FirstOrDefault();
                                    inv.VerifiedOfficer = item.officer;
                                    inv.Remarks = "Re-assigned officer";
                                    _db.SaveChanges();

                                    #region .. Updating in tbl_Applicant_Detail table .. 

                                    var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
                                                          where tad.ApplicationId == appId && tad.IsActive == true
                                                          select new ApplicationStatusUpdate { CredatedBy = tad.CredatedBy, ApplDescStatus = tad.ApplDescStatus }).ToList();

                                    var resUserMasterId = (from vom in _db.tbl_VerificationOfficer_Master
                                                           where vom.Officer_Id == item.officer && vom.IsActive == true
                                                           select vom.UserMasterId).FirstOrDefault();

                                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == appId && s.IsActive == true).FirstOrDefault();
                                    update_query.AssignedVO = resUserMasterId;        //Individual Remapping VO to Applicant table
                                    update_query.FlowId = resUserMasterId;        //Individual Remapping VO to Applicant table
                                    _db.SaveChanges();

                                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                                    objtbl_ApplicantTrans.ApplicantId = appId;
                                    objtbl_ApplicantTrans.CreatedBy = loginId;   //Re-Mapping VO to Applicant trans table 
                                    if (FlowIDExisting.Count > 0)
                                    {
                                        foreach (var ExistingAssignedVOval in FlowIDExisting)
                                        {
                                            
                                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                                        }
                                    }
                                    objtbl_ApplicantTrans.FlowId = resUserMasterId;
                                    objtbl_ApplicantTrans.AssignedVO = resUserMasterId;
                                    objtbl_ApplicantTrans.Remark = "Individual Re-mapping the Verification Officer assigned to the applicant to verify the documents";
                                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                                    objtbl_ApplicantTrans.IsActive = 1;
                                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);

                                    _db.SaveChanges();

                                    #endregion
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
