using DLL.DBConnection;
using Models.Admission;
using Models.ExamNotification;
using Models.Master;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Spire.Doc;
using DLL.Common;

namespace DLL.Admission.AdmissionNotifications
{
    public class AdmissionNotifications : IAdmissionNotifications
    {
        private readonly DbConnection _db = new DbConnection();
        private readonly CommonDLL   _commonDLL = new CommonDLL();

        
        public string CreateAdmissionNotificationDetailsDLL(AdmissionNotification model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                try
                {
                    tbl_admsn_ntf_details tbl_Admission_Notification = _db.tbl_admsn_ntf_details.Where(x => x.AdmsnNtfNum == model.Exam_Notif_Number || (x.applicantType == model.applicantTypeId && x.CourseId == model.CourseTypeId)).FirstOrDefault();
                    if (tbl_Admission_Notification != null && model.StatusId == 0)
                    {
                        return model.Remarks = "exist";
                    }
                    if (tbl_Admission_Notification == null)
                    {
                        tbl_Admission_Notification = new tbl_admsn_ntf_details();
                        tbl_Admission_Notification.AdmsnNtfNum = model.Exam_Notif_Number;
                        tbl_Admission_Notification.Notif_desc = model.NotifDesc;
                        tbl_Admission_Notification.CourseId = model.CourseTypeId;
                        tbl_Admission_Notification.applicantType = model.applicantTypeId.Value;
                        tbl_Admission_Notification.DeptId = model.DeptId;
                        tbl_Admission_Notification.Admsn_ntf_Date = model.Exam_notif_date;
                        tbl_Admission_Notification.Appli_Subm_LastDate = DateTime.Now;
                        tbl_Admission_Notification.Admsn_notif_Doc = model.SavePath;
                        tbl_Admission_Notification.CreatedOn = DateTime.Now;
                        tbl_Admission_Notification.CreatedBy = model.user_id;
                        if(model.DeptId == (int)CmnClass.Dept.Admission)
                            tbl_Admission_Notification.StatusId = 101;
                        else
                            tbl_Admission_Notification.StatusId = 110;
                        tbl_Admission_Notification.FlowId = model.role_id;
                        _db.tbl_admsn_ntf_details.Add(tbl_Admission_Notification);
                        model.Remarks = "Saved";
                    }
                    else if(model.DeptId == 1)
                    {
                        tbl_Admission_Notification.Notif_desc = model.NotifDesc;
                        tbl_Admission_Notification.Admsn_ntf_Date = model.Exam_notif_date;
                        
                        if (tbl_Admission_Notification.StatusId == 110)
                            tbl_Admission_Notification.StatusId = 110;
                        else
                            tbl_Admission_Notification.StatusId = 101;
                        tbl_Admission_Notification.FlowId = model.role_id;
                        tbl_Admission_Notification.Appli_Subm_LastDate = DateTime.Now;
                        tbl_Admission_Notification.Admsn_notif_Doc = model.SavePath;
                        if (model.StatusId == 107)
                            model.Remarks = "Modified";
                        else
                            model.Remarks = "Updated";
                    }
                    else if(tbl_Admission_Notification.StatusId==110 && model.DeptId == 1)
                    {
                        tbl_Admission_Notification.Notif_desc = model.NotifDesc;
                        tbl_Admission_Notification.Admsn_ntf_Date = model.Exam_notif_date;                        
                        tbl_Admission_Notification.StatusId = 101;                       
                        tbl_Admission_Notification.FlowId = model.role_id;
                        tbl_Admission_Notification.Appli_Subm_LastDate = DateTime.Now;
                        tbl_Admission_Notification.Admsn_notif_Doc = model.SavePath;
                    }
                    else
                    {
                        model.Remarks = "exist";
                    }
                    _db.SaveChanges();
                    tbl_admsn_ntf_trans exam_Notification_Trans = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == model.Admission_Notif_Id && x.FlowId == model.role_id).OrderByDescending(a=> a.CreatedOn).FirstOrDefault();

                    if (exam_Notification_Trans == null)
                    {
                        exam_Notification_Trans = new tbl_admsn_ntf_trans();
                        exam_Notification_Trans.Admsn_notif_Id = tbl_Admission_Notification.Admsn_notif_Id;
                        if (model.DeptId == 1 || model.DeptId == 3)
                            exam_Notification_Trans.StatusId = 101;
                        else
                            exam_Notification_Trans.StatusId = 110;
                        exam_Notification_Trans.Trans_date = model.Exam_notif_date; //DateTime.Now;
                        exam_Notification_Trans.FlowId = model.role_id;
                        exam_Notification_Trans.CreatedOn = DateTime.Now;
                        exam_Notification_Trans.CreatedBy = model.user_id;
                        exam_Notification_Trans.Remarks = "inserted successfully";
                        exam_Notification_Trans.Admsn_notif_Doc = Path.ChangeExtension(model.SavePath, ".docx");
                        _db.tbl_admsn_ntf_trans.Add(exam_Notification_Trans);
                    }
                    else
                    {
                        model.Remarks = "Updated";
                        exam_Notification_Trans.CreatedOn = DateTime.Now;
                        exam_Notification_Trans.Admsn_notif_Doc = Path.ChangeExtension(model.SavePath, ".docx");
                    }

                    _db.SaveChanges();
                    transaction.Commit();
                    return model.Remarks;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }
            }

        }

        public string ConvertUploadedAdmsnNotifToPDFDLL(AdmissionNotification model, string PdfFileNameFormat, string DocFileNameFormat, string DocumentsFolder)
        {
            if (!Directory.Exists(DocumentsFolder))
            {
                Directory.CreateDirectory(DocumentsFolder);
            }

            string filename = Path.GetFileNameWithoutExtension(model.file.FileName).Replace("/", "_");
            filename = filename + "-" + GetRoles(model.role_id, 0).Where(a => a.RoleID == model.role_id).Select(a => a.RoleDescShort).FirstOrDefault();
            filename = filename.Replace("-", "_");
            string pdfpath = string.Format(DocumentsFolder + PdfFileNameFormat, filename);
            string wordpath = string.Format(DocumentsFolder + DocFileNameFormat, filename);
            model.file.SaveAs(wordpath);
            ConvertWordToPDF(model.toPDF, wordpath, pdfpath);
            return filename;
        }

        public string ConvertWordToPDF(bool toPDF, string wordpath, string pdfpath)
        {
            if (toPDF == true)
            {
                Document document = new Document();
                document.LoadFromFile(wordpath);

                //Convert Word to PDF
                document.SaveToFile(pdfpath, Spire.Doc.FileFormat.PDF);
            }
            return pdfpath;
        }

        public List<AdmissionNotification> GetUpdateNotificationDLL(int id, int? notificationId)
        {
            List<AdmissionNotification> Notifs = null;

            try
            {
                Notifs = (from n in _db.tbl_admsn_ntf_details //.tbl_exam_notification_trans
                          join d in _db.tbl_admsn_ntf_trans on n.Admsn_notif_Id equals d.Admsn_notif_Id
                          join p in _db.tbl_admission_notif_status_mast on n.StatusId equals p.statusId
                          join c in _db.tbl_course_type_mast on n.CourseId equals c.course_id
                          join m in _db.tbl_department_master on n.DeptId equals m.dept_id
                          join o in _db.tbl_role_master on d.FlowId equals o.role_id
                          where d.Admsn_notif_Id == notificationId && (d.StatusId != 103 && d.StatusId != 106 && d.StatusId != 109)
                          orderby d.CreatedOn descending
                          select new AdmissionNotification
                          {
                              Admission_Notif_Id = n.Admsn_notif_Id,
                              admissionNotificationId = d.Admsn_notif_trans_Id,
                              Exam_Notif_Number = n.AdmsnNtfNum,
                              NotifDesc = n.Notif_desc,
                              Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                              exam_notif_status_desc = p.statusDesc,
                              exam_notif_status_id = n.StatusId,
                              CourseTypeId = n.CourseId,
                              CourseTypeName = c.course_type_name,
                              DeptId = n.DeptId,
                              Admsn_notif_doc = n.Admsn_notif_Doc,
                              applicantTypeId = n.applicantType,
                              RoleId = d.FlowId,
                              Role = o.role_description,
                              createdatetime = d.CreatedOn.ToString(),
                              SavePath = d.Admsn_notif_Doc
                          }).ToList();


                return Notifs;
            }
            catch (Exception e)
            {
                throw e;
            }              
        }
        public int GetNotificationStatus(int? notificationId)
        {
            try
            {
                int res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == notificationId).Select(y => y.StatusId).FirstOrDefault();
                return res;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public List<AdmissionNotification> GetAdmissionNotificationBox()
        {
            try
            {
                var Notifs = (
                              from d in _db.tbl_admsn_ntf_details
                              join p in _db.tbl_admission_notif_status_mast on d.StatusId equals p.statusId
                              join c in _db.tbl_course_type_mast on d.CourseId equals c.course_id
                              join m in _db.tbl_department_master on d.DeptId equals m.dept_id
                              where d.StatusId == 106
                              select new Models.Admission.AdmissionNotification
                              {
                                  Admission_Notif_Id = d.Admsn_notif_Id,
                                  Exam_Notif_Number = d.AdmsnNtfNum,
                                  Exam_Notif_Desc = d.Notif_desc,
                                  Exam_notif_date = (DateTime)d.Admsn_ntf_Date,
                                  exam_notif_status_desc = p.statusDesc,
                                  exam_notif_status_id = d.StatusId,
                                  CourseTypeId = d.CourseId,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = d.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = d.Admsn_notif_Doc_Publish,
                                  applicantTypeId = d.applicantType
                              }
                              ).OrderByDescending(x => x.Admission_Notif_Id).ToList();
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionNotification> GetAdmissionNotification(int id)
        {
            try
            {
                var notify = new List<AdmissionNotification>();
                if (id == 8)
                {
                    notify = (from n in _db.tbl_admsn_ntf_details
                                  from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                                  from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.CourseId).DefaultIfEmpty()
                                  from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                                  from v in _db.tbl_role_master.Where(f =>f.role_id == n.FlowId).DefaultIfEmpty()
                                  select new AdmissionNotification
                                  {
                                      Admission_Notif_Id = n.Admsn_notif_Id,
                                      Exam_Notif_Number = n.AdmsnNtfNum,
                                      Exam_Notif_Desc = n.Notif_desc,
                                      Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                                      exam_notif_status_desc = n.StatusId==106 ? p.statusDesc : p.statusDesc+" - "+v.role_DescShortForm,
                                      StatusId = n.StatusId,
                                      CourseTypeId = n.CourseId,
                                      CourseTypeName = c.course_type_name,
                                      DeptId = n.DeptId,
                                      DeptName = m.dept_description,
                                      SavePath = n.Admsn_notif_Doc,
                                      FlowId = n.FlowId,
                                      RoleDesc=v.role_DescShortForm,
                                      role_id = id,
                                      applicantTypeId=n.applicantType
                                  }).OrderByDescending(x => x.Admission_Notif_Id).ToList();

                }
                else
                {
                    notify = (from n in _db.tbl_admsn_ntf_details
                                  from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                                  from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.CourseId).DefaultIfEmpty()
                                  from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                                  from v in _db.tbl_role_master.Where(f => f.role_id == n.FlowId).DefaultIfEmpty()
                                  where (n.StatusId != 101)
                                  select new AdmissionNotification
                                  {
                                      Admission_Notif_Id = n.Admsn_notif_Id,
                                      Exam_Notif_Number = n.AdmsnNtfNum,
                                      Exam_Notif_Desc = n.Notif_desc,
                                      Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                                      exam_notif_status_desc = n.StatusId == 106 ? p.statusDesc : p.statusDesc + " - " + v.role_DescShortForm,
                                      StatusId = n.StatusId,
                                      CourseTypeId = n.CourseId,
                                      CourseTypeName = c.course_type_name,
                                      DeptId = n.DeptId,
                                      DeptName = m.dept_description,
                                      SavePath = n.Admsn_notif_Doc,
                                      FlowId = n.FlowId,
                                      RoleDesc = v.role_DescShortForm,
                                      role_id = id
                                  }).OrderByDescending(x => x.Admission_Notif_Id).ToList();
                }
                foreach (var res in notify)
                {
                    if (res.StatusId == 109)
                        res.exam_notif_status_desc = "Approved - Pending for Publish -" + res.RoleDesc;

                }
                return notify;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Notification> GetComments(int id)
        {
            try
            {
                var notify = (from n in _db.tbl_admission_comments_transaction
                              join e in _db.tbl_admsn_ntf_details on n.notification_id equals e.Admsn_notif_Id
                              join u in _db.tbl_role_master on n.login_id equals u.role_id
                              join bb in _db.tbl_admission_notif_status_mast on n.status_id equals bb.statusId
                              where n.notification_id == id && n.module_id == 2
                              orderby n.ct_created_on descending
                              select new Notification
                              {
                                  comments = n.comments_transaction_desc,
                                  login_id = n.login_id,
                                  To = u.role_description,
                                  Exam_Notif_Number = e.AdmsnNtfNum,
                                  created_by = n.ct_created_by,
                                  Status=bb.statusDesc,
                                  createdatetime = n.ct_created_on.ToString(),
                              }).ToList();

                var rss= (from aa in notify
                          join uu in _db.tbl_role_master on aa.created_by equals uu.role_id                          
                          select new Notification
                          {
                              comments=aa.comments,
                              Status = aa.Status,
                              To = aa.To,
                              Exam_Notif_Number= aa.Exam_Notif_Number,
                              FromUser = uu.role_description,
                              createdatetime= aa.createdatetime
                          }).ToList();

                return rss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Notification GetAdmissionNotificationDetails(int id, int roleId)
        {
            try
            {
                var res = (from d in _db.tbl_admsn_ntf_details 
                           join p in _db.tbl_admission_notif_status_mast on d.StatusId equals p.statusId
                           join c in _db.tbl_course_type_mast on d.CourseId equals c.course_id
                           join m in _db.tbl_department_master on d.DeptId equals m.dept_id
                           join rl in _db.tbl_role_master on d.FlowId equals rl.role_id
                           join tal in _db.tbl_ApplicantType on d.applicantType equals tal.ApplicantTypeId
                           where d.Admsn_notif_Id == id
                           select new Notification
                           {
                               //Admsn_notif_trans_Id = n.Admsn_notif_trans_Id,
                               Admsn_notif_Id = d.Admsn_notif_Id,
                               CourseType = c.course_type_name,
                               DepartmentName = m.dept_description,
                               NotificationNumber = d.AdmsnNtfNum,
                               Description = d.Notif_desc,
                               AddDate = d.Admsn_ntf_Date,
                               Status = p.statusDesc,
                               StatusId = d.StatusId,
                               ForwardId = d.FlowId,
                               //TransDate = n.Trans_date,
                               Role = rl.role_description,
                               applicantTypeId=d.applicantType,
                               NotifDescName = tal.ApplicantType
                           }
                    ).FirstOrDefault();
                //if (roleId <= 7)
                //{
                //    if (res.ForwardId < 7 )
                //    {
                //        if (res.StatusId == 102)
                //            res.Status = "Reviewed and recommended";
                //    }

                //}
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserDetails> GetRoles(int id, int level)
        {
            int? cur_role_seniority_no = 0;
            try
            {
                cur_role_seniority_no = (from aa in _db.tbl_role_master
                                         where aa.role_id == id && aa.role_is_active == true && aa.role_Level == 1
                                         select aa.role_seniority_no).FirstOrDefault();
                List<UserDetails> res = new List<UserDetails>();

                if (level == 1)
                {
                    res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && (aa.role_seniority_no < cur_role_seniority_no)
                               orderby aa.role_seniority_no descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description,
                                   RoleDescShort = aa.role_DescShortForm
                               }).ToList();                    
                }
                else if (level == 3)
                {
                    res = (from aa in _db.tbl_role_master
                               where aa.role_id == (int)CmnClass.Role.CaseWorker || aa.role_id == (int)CmnClass.Role.DeputyDirector && aa.role_is_active == true && aa.role_Level == 1
                               orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description,
                                   RoleDescShort = aa.role_DescShortForm
                               }).ToList();
                }
                else if (level == 4)
                {
                    //admission send back
                    res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no > cur_role_seniority_no && aa.role_id!=10 && aa.role_id!=11 && aa.role_id!=12 && aa.role_id!=9
                               orderby aa.role_seniority_no 
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description,
                                   RoleDescShort = aa.role_DescShortForm
                               }).ToList();

                }
                else if (level == 0)
                {
                    res = (from aa in _db.tbl_role_master
                               where aa.role_id == id && aa.role_is_active == true && aa.role_Level == 1
                               orderby aa.role_seniority_no
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description,
                                   RoleDescShort = aa.role_DescShortForm
                               }).ToList();
                }
                else
                {
                    res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 2
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description,
                                   RoleDescShort = aa.role_DescShortForm
                               }).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ForwardAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string filePathName)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == admiNotifId).OrderByDescending(y => y.Admsn_notif_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                    if (rs != null)
                    {
                        tbl_admsn_ntf_trans_history inn = new tbl_admsn_ntf_trans_history();
                        inn.Admsn_notif_trans_Id = rs.Admsn_notif_trans_Id;
                        inn.Admsn_notif_Id = rs.Admsn_notif_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.StatusId = rs.StatusId;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_admsn_ntf_trans_history.Add(inn);

                        tbl_admsn_ntf_trans iss = new tbl_admsn_ntf_trans();
                        iss.Admsn_notif_Id = rs.Admsn_notif_Id;
                        iss.Trans_date = rs.Trans_date;
                        if(loginRole < 8)
                            iss.StatusId = 111;
                        else
                            iss.StatusId = 102;
                        iss.Remarks = remarks;
                        iss.FlowId = roleId;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        iss.Admsn_notif_Doc = filePathName;
                        _db.tbl_admsn_ntf_trans.Add(iss);

                        res.FlowId = roleId;
                        if (loginRole < 8)
                            res.StatusId = 111;
                        else
                            res.StatusId = 102;
                        
                        res.Remarks = remarks;

                        tbl_admission_comments_transaction comments = new tbl_admission_comments_transaction();
                        comments.notification_id = rs.Admsn_notif_Id;
                        comments.module_id = 2;
                        if (loginRole < 8)
                            comments.status_id = 111;
                        else
                            comments.status_id = 102;
                        comments.login_id = roleId;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }
        public bool SendbackAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            using (var trasaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                    var res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                    var sendbackid = _db.tbl_admsn_ntf_trans_history.Where(x => x.Admsn_notif_Id == admiNotifId).OrderByDescending(a => a.Admsn_notif_trans_his_Id).Select(y => y.FlowId).First();
                    if (rs != null)
                    {
                        tbl_admsn_ntf_trans_history inn = new tbl_admsn_ntf_trans_history();
                        inn.Admsn_notif_trans_Id = rs.Admsn_notif_trans_Id;
                        inn.Admsn_notif_Id = rs.Admsn_notif_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.StatusId = rs.StatusId;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_admsn_ntf_trans_history.Add(inn);
                        rs.FlowId = roleId;
                        rs.StatusId = 108;
                        res.StatusId = 108;
                        res.FlowId = roleId;
                        tbl_admission_comments_transaction comments = new tbl_admission_comments_transaction();
                        comments.notification_id = rs.Admsn_notif_Id;
                        comments.module_id = 2;
                        comments.status_id = 108;
                        comments.login_id = roleId;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        trasaction.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    trasaction.Rollback();
                    throw ex;
                }
            }

        }
        public bool ApproveAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole, string SavePath)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == admiNotifId).OrderByDescending(y => y.Admsn_notif_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                    if (res != null)
                    {
                        tbl_admsn_ntf_details tbl_Admission_Notification = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                        tbl_Admission_Notification.Admsn_notif_Doc_Publish = SavePath;
                    }
                    if (rs != null)
                    {
                        tbl_admsn_ntf_trans_history inn = new tbl_admsn_ntf_trans_history();
                        inn.Admsn_notif_trans_Id = rs.Admsn_notif_trans_Id;
                        inn.Admsn_notif_Id = rs.Admsn_notif_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.StatusId = rs.StatusId;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_admsn_ntf_trans_history.Add(inn);

                        tbl_admsn_ntf_trans iss = new tbl_admsn_ntf_trans();
                        iss.Admsn_notif_Id = rs.Admsn_notif_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.StatusId = 109;
                        iss.Remarks = remarks;
                        iss.FlowId = 8;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        _db.tbl_admsn_ntf_trans.Add(iss);

                        res.StatusId = 109;
                        res.Remarks = remarks;
                        res.FlowId = 8;

                        tbl_admission_comments_transaction comments = new tbl_admission_comments_transaction();
                        comments.notification_id = rs.Admsn_notif_Id;
                        comments.module_id = 2;
                        comments.status_id = 109;
                        comments.login_id = 8;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }
        public bool ChangesAdmissionNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == admiNotifId).OrderByDescending(y => y.Admsn_notif_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == admiNotifId).FirstOrDefault();
                    if (rs != null)
                    {
                        tbl_admsn_ntf_trans_history inn = new tbl_admsn_ntf_trans_history();
                        inn.Admsn_notif_trans_Id = rs.Admsn_notif_trans_Id;
                        inn.Admsn_notif_Id = rs.Admsn_notif_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.StatusId = rs.StatusId;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_admsn_ntf_trans_history.Add(inn);

                        tbl_admsn_ntf_trans iss = new tbl_admsn_ntf_trans();
                        iss.Admsn_notif_Id = rs.Admsn_notif_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.StatusId = 103;
                        iss.Remarks = remarks;
                        iss.FlowId = roleId;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        _db.tbl_admsn_ntf_trans.Add(iss);

                        if (res.StatusId == 106)
                            res.StatusId = 107;
                        else
                            res.StatusId = 103;

                        res.Remarks = remarks;
                        res.FlowId = roleId;
                        tbl_admission_comments_transaction comments = new tbl_admission_comments_transaction();
                        comments.notification_id = rs.Admsn_notif_Id;
                        comments.module_id = 2;
                        comments.status_id = 103;
                        comments.login_id = 8;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }
        public string PublishNotification(int notificationId, int loginId, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == notificationId).FirstOrDefault();
                    var rs = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == notificationId).OrderByDescending(y => y.Admsn_notif_trans_Id).FirstOrDefault();
                    if (res != null)
                    {
                        tbl_admsn_ntf_trans_history inn = new tbl_admsn_ntf_trans_history();
                        inn.Admsn_notif_trans_Id = rs.Admsn_notif_trans_Id;
                        inn.Admsn_notif_Id = rs.Admsn_notif_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.StatusId = rs.StatusId;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_admsn_ntf_trans_history.Add(inn);

                        tbl_admsn_ntf_trans iss = new tbl_admsn_ntf_trans();
                        iss.Admsn_notif_Id = rs.Admsn_notif_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.StatusId = 106;
                        iss.Remarks = "published";
                        iss.FlowId = 8;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        _db.tbl_admsn_ntf_trans.Add(iss);

                        res.StatusId = 106;
                        res.Remarks = "published";
                        res.FlowId = 8;
                        tbl_admission_comments_transaction comments = new tbl_admission_comments_transaction();
                        comments.notification_id = rs.Admsn_notif_Id;
                        comments.module_id = 2;
                        comments.status_id = 106;
                        comments.login_id = loginRole;
                        comments.comments_transaction_desc = "published";
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        comments.is_published = 1;
                        _db.tbl_admission_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();


                        _commonDLL.NotificationPublish(4);
                        


                        return res.AdmsnNtfNum;
                    }
                    return "failed";
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }

        }
    }
}
