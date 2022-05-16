using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
using System.Web.Mvc;

namespace DLL.Admission.Admission_Notification
{
    public class AdmissionNotificationDLL : IAdmissionNotificationDLL
    {
        private readonly DbConnection _db = new DbConnection();

        public string CreateAdmissionNotificationDetailsDLL(AdmissionNotification model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                try
                {
                    tbl_admsn_ntf_details tbl_Admission_Notification = _db.tbl_admsn_ntf_details.Where(x => x.AdmsnNtfNum == model.Exam_Notif_Number).FirstOrDefault();
                    if (tbl_Admission_Notification == null)
                    {
                        tbl_Admission_Notification = new tbl_admsn_ntf_details();
                        tbl_Admission_Notification.AdmsnNtfNum = model.Exam_Notif_Number;
                        tbl_Admission_Notification.Notif_desc = model.NotifDesc;
                        tbl_Admission_Notification.CourseId = model.CourseTypeId;
                        tbl_Admission_Notification.DeptId = 2;
                        tbl_Admission_Notification.Admsn_ntf_Date = model.Exam_notif_date;
                        tbl_Admission_Notification.Appli_Subm_LastDate = DateTime.Now;
                        tbl_Admission_Notification.Admsn_notif_Doc = model.SavePath;
                        tbl_Admission_Notification.CreatedOn = DateTime.Now;
                        tbl_Admission_Notification.CreatedBy = model.user_id;
                        tbl_Admission_Notification.StatusId = 101;
                        _db.tbl_admsn_ntf_details.Add(tbl_Admission_Notification);
                    }
                    else
                    {
                        tbl_Admission_Notification.Notif_desc = model.NotifDesc;
                        tbl_Admission_Notification.Admsn_ntf_Date = model.Exam_notif_date;
                        tbl_Admission_Notification.StatusId = 101;
                        tbl_Admission_Notification.Appli_Subm_LastDate = DateTime.Now;
                        tbl_Admission_Notification.Admsn_notif_Doc = model.SavePath;                       
                    }

                    tbl_admsn_ntf_trans exam_Notification_Trans = _db.tbl_admsn_ntf_trans.Where(x => x.Admsn_notif_Id == model.Admission_Notif_Id).FirstOrDefault();

                    exam_Notification_Trans = new tbl_admsn_ntf_trans();
                    exam_Notification_Trans.Admsn_notif_Id = tbl_Admission_Notification.Admsn_notif_Id;
                    exam_Notification_Trans.StatusId = 101;
                    exam_Notification_Trans.Trans_date = DateTime.Now;
                    exam_Notification_Trans.FlowId = model.role_id;
                    exam_Notification_Trans.CreatedOn = DateTime.Now;
                    exam_Notification_Trans.CreatedBy = model.user_id;
                    exam_Notification_Trans.Remarks = "inserted successfully";                    
                    _db.tbl_admsn_ntf_trans.Add(exam_Notification_Trans);

                    _db.SaveChanges();
                    transaction.Commit();
                    return "Saved";
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }
            }

        }
        public List<AdmissionNotification> GetUpdateNotificationDLL(int id, int? notificationId)
        {
            List<AdmissionNotification> Notifs = null;

            if (notificationId == 0)
            {
                if (id == 8)
                {
                    Notifs = (from n in _db.tbl_admsn_ntf_details
                              from p in _db.tbl_exam_notif_status_mast.Where(x => x.exam_notif_status_id == n.StatusId).DefaultIfEmpty()
                              from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.CourseId).DefaultIfEmpty()
                              from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                                  //from d in _db.tbl_admsn_ntf_trans.Where(z=>z.Admsn_notif_Id==n.Admsn_notif_Id).DefaultIfEmpty()
                              select new AdmissionNotification
                              {
                                  Admission_Notif_Id = n.Admsn_notif_Id,
                                  Exam_Notif_Number = n.AdmsnNtfNum,
                                  Exam_Notif_Desc = n.Notif_desc,
                                  Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                                  exam_notif_status_desc = p.exam_notif_status_desc,
                                  StatusId = n.StatusId,
                                  CourseTypeId = n.CourseId,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = n.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = n.Admsn_notif_Doc,
                                  //flowId = d.FlowId,
                                  login_id = id
                              }).OrderByDescending(x => x.Admission_Notif_Id).ToList();

                    if (id == 6 || id == 7 || id == 8 || id == 9)
                    {
                        foreach (var item in Notifs)
                        {
                            if (item.FlowId == 6 || item.FlowId == 7 || item.FlowId == 8 || item.FlowId == 9)
                            {
                                item.exam_notif_status_desc = "Pending for Approval";
                            }
                        }
                    }
                    return Notifs;
                }
                else
                {
                    Notifs = (from n in _db.tbl_admsn_ntf_details
                              from p in _db.tbl_exam_notif_status_mast.Where(x => x.exam_notif_status_id == n.StatusId).DefaultIfEmpty()
                              from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.CourseId).DefaultIfEmpty()
                              from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                              where n.StatusId != 101
                              select new Models.Admission.AdmissionNotification
                              {
                                  Admission_Notif_Id = n.Admsn_notif_Id,
                                  Exam_Notif_Number = n.AdmsnNtfNum,
                                  Exam_Notif_Desc = n.Notif_desc,
                                  Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                                  exam_notif_status_desc = p.exam_notif_status_desc,
                                  exam_notif_status_id = n.StatusId,
                                  CourseTypeId = n.CourseId,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = n.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = n.Admsn_notif_Doc

                              }).OrderByDescending(x => x.Admission_Notif_Id).ToList();
                    return Notifs;
                }

            }
            else
            {
                Notifs = (from n in _db.tbl_admsn_ntf_details //.tbl_exam_notification_trans
                          join d in _db.tbl_admsn_ntf_trans on n.Admsn_notif_Id equals d.Admsn_notif_Id
                          join p in _db.tbl_exam_notif_status_mast on n.StatusId equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on n.CourseId equals c.course_id
                          join m in _db.tbl_department_master on n.DeptId equals m.dept_id
                          where d.Admsn_notif_Id == notificationId
                          select new Models.Admission.AdmissionNotification
                          {
                              Admission_Notif_Id = n.Admsn_notif_Id,
                              Exam_Notif_Number = n.AdmsnNtfNum,
                              Exam_Notif_Desc = n.Notif_desc,
                              Exam_notif_date = (DateTime)n.Admsn_ntf_Date,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = n.StatusId,
                              CourseTypeId = n.CourseId,
                              CourseTypeName = c.course_type_name,
                              DeptId = n.DeptId,
                          }).Distinct().ToList();


                return Notifs;
            }
        }
        public List<AdmissionNotification> GetAdmissionNotificationBox()
        {
            try
            {
                var Notifs = (
                              from d in _db.tbl_admsn_ntf_details
                              join p in _db.tbl_exam_notif_status_mast on d.StatusId equals p.exam_notif_status_id
                              join c in _db.tbl_course_type_mast on d.CourseId equals c.course_id
                              join m in _db.tbl_department_master on d.DeptId equals m.dept_id
                              where d.StatusId == 106
                              select new Models.Admission.AdmissionNotification
                              {
                                  Admission_Notif_Id = d.Admsn_notif_Id,
                                  Exam_Notif_Number = d.AdmsnNtfNum,
                                  Exam_Notif_Desc = d.Notif_desc,
                                  Exam_notif_date = (DateTime)d.Admsn_ntf_Date,
                                  exam_notif_status_desc = p.exam_notif_status_desc,
                                  exam_notif_status_id = d.StatusId,
                                  CourseTypeId = d.CourseId,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = d.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = d.Admsn_notif_Doc

                              }
                              ).OrderByDescending(x => x.Admission_Notif_Id).ToList();
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SelectList GetDepartmentListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_department_master.Where(y => y.dept_id == 2).Select(x => new SelectListItem
            {
                Text = x.dept_description.ToString(),
                Value = x.dept_id.ToString()
            }).ToList();
            foreach (var item in CoursetList)
            {
                item.Selected = true;
            }

            return new SelectList(CoursetList, "Value", "Text");
        }

    }
}
