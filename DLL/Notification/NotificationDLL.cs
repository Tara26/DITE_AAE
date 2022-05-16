using DLL.DBConnection;
using System;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models.ExamNotification;

using System.Data;
using System.IO;
using System.Web;
using Spire.Xls;
using System.Data.SqlClient;
using System.Configuration;
using Models.ExamCenterMap;
using Models.Master;


namespace DLL.User
{
    public class NotificationDLL : INotificationDLL
    {
        private readonly DbConnection _db = new DbConnection();
        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        //BNM Question Paper Upload
        private readonly string UploadFolderQP = ConfigurationManager.AppSettings["QPDocumentsPath"];

        //Get Exam CentresEmailID List  
        public SelectList GetExamCentresEmailIDListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_exam_centers.Select(x => new SelectListItem
            {
                Text = x.ec_email_id.ToString(),
                Value = x.ec_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select EC Email",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }
        //Get Course List
        public SelectList GetCourseListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_course_type_mast.Select(x => new SelectListItem
            {
                Text = x.course_type_name.ToString(),
                Value = x.course_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Course",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        public SelectList SessionListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_Year.Select(x => new SelectListItem
            {
                Text = x.Year.ToString(),
                Value = x.YearID.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Year",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }


        public SelectList GetTradeTypeListDLL()
        {
            List<SelectListItem> TradeTypetList = _db.tbl_trade_type_mast.Select(x => new SelectListItem
            {
                Text = x.trade_type_name.ToString(),
                Value = x.trade_type_id.ToString()
            }).ToList();
            var TradeTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Trade Type",

            };
            TradeTypetList.Insert(0, TradeTList);
            return new SelectList(TradeTypetList, "Value", "Text");
        }

        public SelectList GetTradeListDLL()
        {
            List<SelectListItem> TradeList = _db.tbl_trade_mast.Select(x => new SelectListItem
            {
                Text = x.trade_name.ToString(),
                Value = x.trade_id.ToString()
            }).ToList();
            var TradeTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Trade",

            };
            TradeList.Insert(0, TradeTList);
            return new SelectList(TradeList, "Value", "Text");
        }

        public SelectList GetTradeYearListDLL()
        {
            List<SelectListItem> TradeYearList = _db.tbl_trade_year_mast.Select(x => new SelectListItem
            {
                Text = x.trade_year_name.ToString(),
                Value = x.trade_year_id.ToString()
            }).ToList();
            var TradeYList = new SelectListItem()
            {

                Value = null,
                Text = "Select Trade Year",

            };
            TradeYearList.Insert(0, TradeYList);
            return new SelectList(TradeYearList, "Value", "Text");
        }
        public SelectList ExamNotificationListDLL()
        {
            List<SelectListItem> res = new List<SelectListItem>();
            var noti = _db.tbl_exam_cal_notif_mast.ToList();
            //if (noti != null)
            //{


            //   res = (from enm in _db.tbl_exam_notification_mast
            //                                join n in _db.tbl_exam_cal_notif_mast on enm.exam_notif_id equals n.exam_notif_id
            //                                select new SelectListItem
            //                                {
            //                                    Text = enm.exam_notif_number.ToString(),
            //                                    Value = enm.exam_notif_id.ToString()
            //                                }).ToList();
            //}

            List<SelectListItem> NotifList = (from enm in _db.tbl_exam_notification_mast
                                              join dept in _db.tbl_department_master on enm.department_id equals dept.dept_id
                                              where dept.dept_id == 1 && enm.status_id == 105
                                              select new SelectListItem
                                              {
                                                  Text = enm.exam_notif_number.ToString(),
                                                  Value = enm.exam_notif_id.ToString()
                                              }).ToList();

            var groupBNames = new HashSet<string>(res.Select(x => x.Value));
            List<SelectListItem> result = NotifList.Where(x => !groupBNames.Contains(x.Value)).ToList();

            var NotificationList = new SelectListItem()
            {

                Value = null,
                Text = "Select Exam Notification",

            };
            result.Insert(0, NotificationList);
            return new SelectList(result, "Value", "Text");
        }
        public List<SelectListItem> GetTradeListBasedOnIdDLL(int CourseTypeId, int? TradeTypeId)
        {
            List<SelectListItem> TradeList = new List<SelectListItem>();
            if (TradeTypeId != null)
            {
                TradeList = _db.tbl_trade_mast.Where(x => x.trade_course_id == CourseTypeId && x.trade_type_id == TradeTypeId).Select(x => new SelectListItem { Text = x.trade_name, Value = x.trade_id.ToString() }).ToList();
            }
            else
            {
                TradeList = _db.tbl_trade_mast.Where(x => x.trade_course_id == CourseTypeId).Select(x => new SelectListItem { Text = x.trade_name, Value = x.trade_id.ToString() }).ToList();
            }

            var TradeTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Trade"
            };
            TradeList.Insert(0, TradeTList);
            return TradeList;
        }

        public List<SelectListItem> GetSubjectListBasedOnIdDLL(int? CourseTypeId, int? STradeId, int? subjectType)
        {
            List<SelectListItem> SubjectList = new List<SelectListItem>();
            if (subjectType != null && STradeId != null)
            {
                if (subjectType < 36)
                {
                    SubjectList = _db.tbl_exam_subject_mast.Where(x => x.subject_type_id == 1 && x.exam_subject_trade_id != null).Select(x => new SelectListItem { Text = x.exam_subject_desc, Value = x.exam_subject_id.ToString() }).ToList();
                }
                if (subjectType == 36)
                {
                    SubjectList = _db.tbl_exam_subject_mast.Where(x => x.subject_type_id == 2).Select(x => new SelectListItem { Text = x.exam_subject_desc, Value = x.exam_subject_id.ToString() }).ToList();
                }
                //else
                //{
                //    //SubjectList = _db.tbl_exam_subject_mast.Where(x => x.trade_special_type == STradeId && x.subject_type_id == subjectType).Select(x => new SelectListItem { Text = x.exam_subject_desc, Value = x.exam_subject_id.ToString() }).ToList();
                //    SubjectList = _db.tbl_exam_subject_mast.Where(x => x.trade_special_type == STradeId && x.subject_type_id == subjectType).Select(x => new SelectListItem { Text = x.exam_subject_desc, Value = x.exam_subject_id.ToString() }).ToList();
                //}

            }
            else
            {
                SubjectList = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_is_active == true).Select(x => new SelectListItem { Text = x.exam_subject_desc, Value = x.exam_subject_id.ToString() }).ToList();
            }

            var SubjectTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Subject"
            };
            SubjectList.Insert(0, SubjectTList);
            return SubjectList;
        }

        public SelectList GetExamTypeListDLL()
        {
            List<SelectListItem> ExamTypeList = _db.tbl_exam_type_mast.Select(x => new SelectListItem
            {
                Text = x.exam_type_name.ToString(),
                Value = x.exam_type_id.ToString()
            }).ToList();

            for (int i = 0; i < 2; i++)
            {
                if (ExamTypeList[i].Text == "Main")
                {
                    ExamTypeList[i].Text = "Regular";
                    //ExamTypeList[i].Value = ExamTypeList[i].Value;
                }
                else
                {
                    ExamTypeList[i].Text = "Repeater";
                    //ExamTypeList[i].Value = ExamTypeList[i].Value;
                }
            }

            var ExamTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Exam Type",
            };
            ExamTypeList.Insert(0, ExamTList);
            return new SelectList(ExamTypeList, "Value", "Text");
        }

        public SelectList GetExamSemListDLL()
        {
            List<SelectListItem> ExamSemList = _db.tbl_exam_semester_mast.Select(x => new SelectListItem
            {
                Text = x.exam_semester_desc.ToString(),
                Value = x.exam_semester_id.ToString()
            }).ToList();
            var ExamTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Semester",

            };
            ExamSemList.Insert(0, ExamTList);
            return new SelectList(ExamSemList, "Value", "Text");
        }

        public SelectList GetSubjectTypeListDLL()
        {
            //List<SelectListItem> SubjectTypeList = _db.tbl_exam_subject_type_mast.Select(x => new SelectListItem
            //{
            //    Text = x.exam_subject_type_desc.ToString(),
            //    Value = x.exam_subject_type_id.ToString()
            //}).ToList();
            //var ExamTList = new SelectListItem()
            //{

            //    Value = null,
            //    Text = "Select Subject Type",

            //};
            //SubjectTypeList.Insert(0, ExamTList);
            //return new SelectList(SubjectTypeList, "Value", "Text");
            //List<SelectListItem> SubjectTypeList = _db.tbl_exam_subject_type_mast.Select(x => new SelectListItem
            //{
            //    Text = x.exam_subject_type_desc.ToString(),
            //    Value = x.exam_subject_type_id.ToString()
            //}).ToList();
            //var ExamTList = new SelectListItem()
            //{

            //    Value = null,
            //    Text = "Select Subject Type",

            //};
            //SubjectTypeList.Insert(0, ExamTList);
            //return new SelectList(SubjectTypeList, "Value", "Text");

            List<SelectListItem> SubjectTypeList = _db.tbl_exam_subject_type_mast.Select(x => new SelectListItem
            {
                Text = x.exam_subject_type_desc.ToString(),
                Value = x.exam_subject_type_id.ToString()
            }).ToList();
            var ExamTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Subject Type",

            };
            SubjectTypeList.Insert(0, ExamTList);
            return new SelectList(SubjectTypeList, "Value", "Text");
        }

        public SelectList SubjectTypeDLL()
        {
            List<SelectListItem> SubjectType = _db.tbl_subject_type.Select(x => new SelectListItem
            {
                Text = x.subject_type.ToString(),
                Value = x.st_id.ToString()
            }).ToList();
            var ExamTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Subject Type",

            };
            SubjectType.Insert(0, ExamTList);
            return new SelectList(SubjectType, "Value", "Text");
        }

        public SelectList GetSubjectListDLL()
        {
            List<SelectListItem> SubjectList = _db.tbl_exam_subject_mast.Select(x => new SelectListItem
            {
                Text = x.exam_subject_desc.ToString(),
                Value = x.exam_subject_id.ToString()
            }).ToList();
            var ExamTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Subject",

            };
            SubjectList.Insert(0, ExamTList);
            return new SelectList(SubjectList, "Value", "Text");
        }

        //Get DepartmentList
        public SelectList GetDepartmentListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_department_master.Select(x => new SelectListItem
            {
                Text = x.dept_description.ToString(),
                Value = x.dept_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Department Type",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        //Get Notification Description List
        public SelectList GetNotificationDescListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_notification_description.Select(x => new SelectListItem
            {
                Text = x.notification_description.ToString(),
                Value = x.notif_decr_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Notification Description",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        public string CreateNotificationDetailsDLL(Notification model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {

                try
                {
                    bool isActive = true;
                    int exam_notif_status_id = 100;                    
                    if (model.Action == 0)
                    {
                        isActive = false;
                        exam_notif_status_id = 111;
                    }
                   
                    tbl_exam_notification_mast tbl_Exam_Notification = _db.tbl_exam_notification_mast.Where(x => x.exam_notif_number == model.Exam_Notif_Number && x.login_id == model.login_id).FirstOrDefault();
                    if (tbl_Exam_Notification == null)
                    {
                                            
                        tbl_Exam_Notification = new tbl_exam_notification_mast();
                        tbl_Exam_Notification.exam_notif_number = model.Exam_Notif_Number;
                        tbl_Exam_Notification.notif_type_id = model.NotifDescId;
                        tbl_Exam_Notification.course_id = model.CourseTypeId;
                        tbl_Exam_Notification.department_id = model.DeptId;
                        tbl_Exam_Notification.exam_notif_date = model.Exam_notif_date;
                        tbl_Exam_Notification.exam_notif_type = "Regular";
                        tbl_Exam_Notification.appli_charges_fee = 20;
                        tbl_Exam_Notification.exam_regular_fee = 250;
                        tbl_Exam_Notification.exam_repeater_fee = 150;
                        tbl_Exam_Notification.fee_pay_last_date = model.fee_pay_last_date;
                        tbl_Exam_Notification.appli_from_last_date = DateTime.Now;
                        tbl_Exam_Notification.trainee_name_eval_last_date = DateTime.Now;
                        tbl_Exam_Notification.princ_sub_last_date = DateTime.Now;
                        tbl_Exam_Notification.jd_sub_last_date = DateTime.Now;
                        tbl_Exam_Notification.penalty_after10days_fee = 30;
                        tbl_Exam_Notification.penalty_after30days_fee = 20;
                        tbl_Exam_Notification.penalty_after40days_fee = 10;
                        tbl_Exam_Notification.is_active = isActive;
                        tbl_Exam_Notification.creation_datetime = DateTime.Now;
                        tbl_Exam_Notification.created_by = model.user_id;
                        tbl_Exam_Notification.login_id = model.login_id;
                        tbl_Exam_Notification.status_id = exam_notif_status_id;
                       
                        tbl_Exam_Notification.exam_notif_file_path = model.SavePath;
                        tbl_Exam_Notification.notif_description = model.Description;
                        tbl_Exam_Notification.signed_exam_notif_file_path = model.SavePath;
                        _db.tbl_exam_notification_mast.Add(tbl_Exam_Notification);
                        _db.SaveChanges();

                    }
                    else
                    {
                        tbl_Exam_Notification.exam_notif_number = model.Exam_Notif_Number;
                        tbl_Exam_Notification.notif_type_id = model.NotifDescId;
                        tbl_Exam_Notification.exam_notif_date = model.Exam_notif_date;
                        tbl_Exam_Notification.exam_notif_type = "Regular";
                        tbl_Exam_Notification.appli_charges_fee = 68;
                        tbl_Exam_Notification.exam_regular_fee = 78;
                        tbl_Exam_Notification.exam_repeater_fee = 78;
                        tbl_Exam_Notification.fee_pay_last_date = model.fee_pay_last_date;
                        tbl_Exam_Notification.appli_from_last_date = DateTime.Now;
                        tbl_Exam_Notification.trainee_name_eval_last_date = DateTime.Now;
                        tbl_Exam_Notification.princ_sub_last_date = DateTime.Now;
                        tbl_Exam_Notification.jd_sub_last_date = DateTime.Now;
                        tbl_Exam_Notification.penalty_after10days_fee = 30;
                        tbl_Exam_Notification.penalty_after30days_fee = 20;
                        tbl_Exam_Notification.penalty_after40days_fee = 10;
                        tbl_Exam_Notification.is_active = isActive;
                        tbl_Exam_Notification.status_id = exam_notif_status_id;
                        tbl_Exam_Notification.updation_datetime = DateTime.Now;
                        tbl_Exam_Notification.updated_by = 2;
                        tbl_Exam_Notification.notif_description = model.Description;
                        tbl_Exam_Notification.exam_notif_file_path = model.SavePath;
                        tbl_Exam_Notification.signed_exam_notif_file_path = model.SavePath;

                        _db.SaveChanges();
                    }



                    tbl_exam_notification_trans exam_Notification_Trans = _db.tbl_exam_notification_trans.Where((x => x.exam_notif_id == model.Exam_Notif_Id && x.login_id == model.login_id || x.exam_notif_status_id ==111 && x.exam_notif_id == tbl_Exam_Notification.exam_notif_id)).FirstOrDefault();
                    if (exam_Notification_Trans == null)
                    {
                        exam_Notification_Trans = new tbl_exam_notification_trans();
                        exam_Notification_Trans.exam_notif_id = tbl_Exam_Notification.exam_notif_id;
                        exam_Notification_Trans.exam_notif_status_id = exam_notif_status_id;
                        exam_Notification_Trans.trans_date = DateTime.Now;
                        exam_Notification_Trans.is_active = isActive;
                        exam_Notification_Trans.creation_datetime = DateTime.Now;
                        exam_Notification_Trans.created_by = 1;
                        exam_Notification_Trans.login_id = model.login_id;
                        exam_Notification_Trans.exam_notif_doc_file_path = Path.ChangeExtension(model.SavePath, ".doc");
                        _db.tbl_exam_notification_trans.Add(exam_Notification_Trans);
                        _db.SaveChanges();

                    }
                    else
                    {
                        exam_Notification_Trans.exam_notif_id = tbl_Exam_Notification.exam_notif_id;
                        exam_Notification_Trans.exam_notif_status_id = exam_notif_status_id;
                        exam_Notification_Trans.trans_date = DateTime.Now;
                        exam_Notification_Trans.is_active = isActive;
                        exam_Notification_Trans.updation_datetime = DateTime.Now;
                        exam_Notification_Trans.updated_by = model.login_id;
                        exam_Notification_Trans.exam_notif_doc_file_path = Path.ChangeExtension(model.SavePath, ".doc");
                        exam_Notification_Trans.login_id = model.login_id;
                        _db.SaveChanges();
                    }
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

        public List<Notification> GetUpdateNotificationDLL(int? notificationId = null)
        {
            List<Notification> Notifs = null;
            if (notificationId == null)
            {
                Notifs = (from d in _db.tbl_exam_notification_mast

                          join p in _db.tbl_exam_notif_status_mast on d.status_id equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                          join m in _db.tbl_department_master on d.department_id equals m.dept_id
                          join END in _db.tbl_notification_description on d.notif_type_id equals END.notif_decr_id
                          //join comnt in _db.tbl_comments_transaction on d.exam_notif_id  equals comnt.notification_id into _comnt
                          //from CT in _comnt.DefaultIfEmpty()
                          where (d.status_id == 102 || d.status_id == 103 || d.status_id == 105 || d.status_id == 108) && d.login_id == 2 //&& CT.login_id == 2
                          select new Models.ExamNotification.Notification
                          {
                              Exam_Notif_Id = d.exam_notif_id,
                              Exam_Notif_Number = d.exam_notif_number,
                              NotifDescId = d.notif_type_id,
                              Exam_Notif_Desc = END.notification_description,
                              Exam_notif_date = (DateTime)d.exam_notif_date,
                              exam_notif_type = d.exam_notif_type,
                              fee_pay_last_date = (DateTime)d.fee_pay_last_date,
                              appli_from_last_date = (DateTime)d.appli_from_last_date,
                              trainee_name_eval_last_date = (DateTime)d.trainee_name_eval_last_date,
                              princ_sub_last_date = (DateTime)d.princ_sub_last_date,
                              jd_sub_last_date = (DateTime)d.jd_sub_last_date,
                              appli_charges_fee = d.appli_charges_fee,
                              exam_regular_fee = d.exam_regular_fee,
                              exam_repeater_fee = d.exam_repeater_fee,
                              penalty_after10days_fee = d.penalty_after10days_fee,
                              penalty_after30days_fee = d.penalty_after30days_fee,
                              penalty_after40days_fee = d.penalty_after40days_fee,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = d.status_id,
                              CourseTypeId = d.course_id,
                              CourseTypeName = c.course_type_name,
                              DeptId = d.department_id,
                              DeptName = m.dept_description,
                              SavePath = d.exam_notif_file_path,
                              Description = d.notif_description
                              //comments = CT.comments_transaction_desc
                          }
               ).OrderByDescending(x => x.Exam_Notif_Id).ToList();

            }
            else
            {
                Notifs = (from n in _db.tbl_exam_notification_trans
                          join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                          join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                          join m in _db.tbl_department_master on d.department_id equals m.dept_id into _m
                          join END in _db.tbl_notification_description on d.notif_type_id equals END.notif_decr_id
                          //join comnt in _db.tbl_comments_transaction on d.exam_notif_id equals comnt.notification_id into _comnt
                          //from CT in _comnt.DefaultIfEmpty()
                          where d.exam_notif_id == notificationId
                          select new Models.ExamNotification.Notification
                          {
                              Exam_Notif_Id = n.exam_notif_id,
                              Exam_Notif_Number = d.exam_notif_number,
                              NotifDescId = d.notif_type_id,
                              Exam_Notif_Desc = END.notification_description,
                              Exam_notif_date = (DateTime)d.exam_notif_date,
                              exam_notif_type = d.exam_notif_type,
                              fee_pay_last_date = (DateTime)d.fee_pay_last_date,
                              appli_from_last_date = (DateTime)d.appli_from_last_date,
                              trainee_name_eval_last_date = (DateTime)d.trainee_name_eval_last_date,
                              princ_sub_last_date = (DateTime)d.princ_sub_last_date,
                              jd_sub_last_date = (DateTime)d.jd_sub_last_date,
                              appli_charges_fee = d.appli_charges_fee,
                              exam_regular_fee = d.exam_regular_fee,
                              exam_repeater_fee = d.exam_repeater_fee,
                              penalty_after10days_fee = d.penalty_after10days_fee,
                              penalty_after30days_fee = d.penalty_after30days_fee,
                              penalty_after40days_fee = d.penalty_after40days_fee,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = n.exam_notif_status_id,
                              CourseTypeId = d.course_id,
                              CourseTypeName = c.course_type_name,
                              DeptId = d.department_id,
                              DeptName = END.notification_description,
                              //comments = CT.comments_transaction_desc
                              Description = d.notif_description

                          }).Distinct().ToList();



            }
            return Notifs;
        }

        public List<Notification> GetNotificationStatusDLL(Notification notification)
        {
            var notify = (from n in _db.tbl_exam_notification_mast
                          join p in _db.tbl_exam_notif_status_mast on n.status_id equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on n.course_id equals c.course_id
                          join m in _db.tbl_department_master on n.department_id equals m.dept_id
                          join nd in _db.tbl_notification_description on n.notif_type_id equals nd.notif_decr_id
                          join u in _db.tbl_user_master on n.login_id equals u.um_id 
                        // join rm in _db.tbl_role_master on  u.role_id equals rm.role_id
                          select new Models.ExamNotification.Notification
                          {
                              Exam_Notif_Id = n.exam_notif_id,
                              Exam_Notif_Number = n.exam_notif_number,
                              NotifDescId = n.notif_type_id,
                              Exam_Notif_Desc = nd.notification_description,
                              Description = n.notif_description,
                              Exam_notif_date = (DateTime)n.exam_notif_date,
                              exam_notif_type = n.exam_notif_type,
                              fee_pay_last_date = (DateTime)n.fee_pay_last_date,
                              appli_from_last_date = (DateTime)n.appli_from_last_date,
                              trainee_name_eval_last_date = (DateTime)n.trainee_name_eval_last_date,
                              princ_sub_last_date = (DateTime)n.princ_sub_last_date,
                              jd_sub_last_date = (DateTime)n.jd_sub_last_date,
                              appli_charges_fee = n.appli_charges_fee,
                              exam_regular_fee = n.exam_regular_fee,
                              exam_repeater_fee = n.exam_repeater_fee,
                              penalty_after10days_fee = n.penalty_after10days_fee,
                              penalty_after40days_fee = n.penalty_after40days_fee,
                              penalty_after30days_fee = n.penalty_after30days_fee,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = n.status_id,
                              CourseTypeId = n.course_id,
                              CourseTypeName = c.course_type_name,
                              DeptId = n.department_id,
                              DeptName = m.dept_description,
                              login_id = n.login_id,
                              SavePath = n.exam_notif_file_path,
                              creation_datetime = n.creation_datetime,
                              updation_datetime = n.updation_datetime.HasValue ? n.updation_datetime : n.creation_datetime,
                             // By = rm.role_DescShortForm,
                              comcount = 1,
                              //  Description = d.notif_description

                              // comments = com.comments_transaction_desc
                          }
                ).Distinct().OrderByDescending(x => x.updation_datetime).ToList();

            for (int i = 0; i < notify.Count; i++)
            {
                
                var count = notify[i].Exam_Notif_Id;
                var listcont = (from j in _db.tbl_comments_transaction
                                where j.notification_id == count
                                select j).ToList();
                notify[i].comcount = listcont.Count;
            }

            return notify;
        }

        public Notification GetViewDLL(int id, int loginid)
        {
            var a = (from n in _db.tbl_exam_notification_mast
                         //join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                     join p in _db.tbl_exam_notif_status_mast on n.status_id equals p.exam_notif_status_id
                     join c in _db.tbl_course_type_mast on n.course_id equals c.course_id
                     join m in _db.tbl_department_master on n.department_id equals m.dept_id
                     //join u in _db.tbl_user_login on n.login_id equals u.id
                     join um in _db.tbl_user_master on n.login_id equals um.um_id
                     join nd in _db.tbl_notification_description on n.notif_type_id equals nd.notif_decr_id
                     //join com in _db.tbl_comments_transaction on d.exam_notif_id equals com.notification_id into _com
                     //from CT in _com.DefaultIfEmpty()
                     where n.exam_notif_id == id //&& n.login_id == loginid
                     select new Models.ExamNotification.Notification
                     {
                         Exam_Notif_Id = n.exam_notif_id,
                         Exam_Notif_Number = n.exam_notif_number,
                         NotifDescId = n.notif_type_id,
                         Exam_Notif_Desc = nd.notification_description,
                         Exam_notif_date = (DateTime)n.exam_notif_date,
                         exam_notif_type = n.exam_notif_type,
                         Description = n.notif_description,
                         fee_pay_last_date = (DateTime)n.fee_pay_last_date,
                         appli_from_last_date = (DateTime)n.appli_from_last_date,
                         trainee_name_eval_last_date = (DateTime)n.trainee_name_eval_last_date,
                         princ_sub_last_date = (DateTime)n.princ_sub_last_date,
                         jd_sub_last_date = (DateTime)n.jd_sub_last_date,
                         appli_charges_fee = n.appli_charges_fee,
                         exam_regular_fee = n.exam_regular_fee,
                         exam_repeater_fee = n.exam_repeater_fee,
                         penalty_after10days_fee = n.penalty_after10days_fee,
                         penalty_after30days_fee = n.penalty_after30days_fee,
                         penalty_after40days_fee = n.penalty_after40days_fee,
                         exam_notif_status_id = n.status_id,
                         exam_notif_status_desc = p.exam_notif_status_desc,
                         CourseTypeId = n.course_id,
                         CourseTypeName = c.course_type_name,
                         DeptId = n.department_id,
                         DeptName = m.dept_description,
                         SavePath = n.exam_notif_file_path,
                         //comments = CT.comments_transaction_desc,
                         creation_datetime = n.creation_datetime,
                         updation_datetime = n.updation_datetime,
                         //createdbyuser = u.user_name,
                         //updatedbyuser = u.user_name,
                         login_id = n.login_id,
                         //RecordLevel = u.user_name,
						 created_by = n.created_by,
                         NotifDescName = p.exam_notif_status_desc,
                     }).FirstOrDefault();

			//un comment from user fix Me
				//var ById = a.created_by;
				//var UserName = (from j in _db.tbl_user_login
				//				where j.id == ById
				//				select j.user_name).FirstOrDefault();
				//a.By = UserName;
			return a;
        }

        public Notification GetNotificationtransactiondtlsDLL(int id, int loginid)
        {
            var a = (from n in _db.tbl_exam_notification_trans
                         
                     where n.exam_notif_id == id && n.login_id == loginid
                     select new Models.ExamNotification.Notification
                     {
                         Exam_Notif_Id = n.exam_notif_id,
                         DocSavePath = n.exam_notif_doc_file_path
                     }).FirstOrDefault();


            var ById = a.created_by;
            var UserName = (from j in _db.tbl_user_login
                            where j.id == ById
                            select j.user_name).FirstOrDefault();
            a.By = UserName;
            return a;
        }

        public Notification GetViewDLL(int id)
        {
            var a = (from n in _db.tbl_exam_notification_trans
                     join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                     join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                     join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                     join m in _db.tbl_department_master on d.department_id equals m.dept_id
                     join nd in _db.tbl_notification_description on d.notif_type_id equals nd.notif_decr_id
                     join com in _db.tbl_comments_transaction on d.exam_notif_id equals com.notification_id into _com
                     from CT in _com.DefaultIfEmpty()
                     where n.exam_notif_id == id
                     select new Models.ExamNotification.Notification
                     {
                         Exam_Notif_Id = n.exam_notif_id,
                         Exam_Notif_Number = d.exam_notif_number,
                         NotifDescId = d.notif_type_id,
                         Exam_Notif_Desc = nd.notification_description,
                         Exam_notif_date = (DateTime)d.exam_notif_date,
                         exam_notif_type = d.exam_notif_type,
                         fee_pay_last_date = (DateTime)d.fee_pay_last_date,
                         appli_from_last_date = (DateTime)d.appli_from_last_date,
                         trainee_name_eval_last_date = (DateTime)d.trainee_name_eval_last_date,
                         princ_sub_last_date = (DateTime)d.princ_sub_last_date,
                         jd_sub_last_date = (DateTime)d.jd_sub_last_date,
                         appli_charges_fee = d.appli_charges_fee,
                         exam_regular_fee = d.exam_regular_fee,
                         exam_repeater_fee = d.exam_repeater_fee,
                         penalty_after10days_fee = d.penalty_after10days_fee,
                         penalty_after30days_fee = d.penalty_after30days_fee,
                         penalty_after40days_fee = d.penalty_after40days_fee,
                         exam_notif_status_id = n.exam_notif_status_id,
                         exam_notif_status_desc = p.exam_notif_status_desc,
                         CourseTypeId = d.course_id,
                         CourseTypeName = c.course_type_name,
                         DeptId = d.department_id,
                         DeptName = m.dept_description,
                         SavePath = d.exam_notif_file_path,
                         comments = CT.comments_transaction_desc
                     }).FirstOrDefault();
            return a;
        }

        public tbl_exam_notification_trans GetnotifyByID(Notification notification)
        {

            var _mat = (from n in _db.tbl_exam_notification_trans
                        join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                        join m in _db.tbl_department_master on d.department_id equals m.dept_id
                        join nd in _db.tbl_notification_description on d.notif_type_id equals nd.notif_decr_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notification.Exam_Notif_Id && n.login_id == notification.login_id && n.exam_notif_status_id != 111
                        select n).FirstOrDefault();
            return _mat;
        }

        public tbl_exam_notification_mast GetnotifyByIDMast(int notifnum)
        {
            var _mat = (from n in _db.tbl_exam_notification_mast
                        join p in _db.tbl_exam_notif_status_mast on n.status_id equals p.exam_notif_status_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notifnum
                        select n).FirstOrDefault();
            return _mat;
        }

        public tbl_exam_notification_mast Getnotify(Notification notification)
        {
            var _mat = (from n in _db.tbl_exam_notification_mast

                        //join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                        join p in _db.tbl_exam_notif_status_mast on n.status_id equals p.exam_notif_status_id
                        join c in _db.tbl_course_type_mast on n.course_id equals c.course_id
                        join m in _db.tbl_department_master on n.department_id equals m.dept_id
                        join nd in _db.tbl_notification_description on n.notif_type_id equals nd.notif_decr_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notification.Exam_Notif_Id
                        select n).FirstOrDefault();
            return _mat;
        }

        public tbl_exam_notification_trans GetnotifyByIDofCW(Notification notification)
        {
            var _mat = (from n in _db.tbl_exam_notification_trans
                        join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                        join m in _db.tbl_department_master on d.department_id equals m.dept_id
                        join nd in _db.tbl_notification_description on d.notif_type_id equals nd.notif_decr_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notification.Exam_Notif_Id && n.login_id == 2
                        select n).FirstOrDefault();
            return _mat;
        }

        public tbl_exam_notification_trans GetnotifyRoleId(Notification notification)
        {
            var _mat = (from n in _db.tbl_exam_notification_trans
                        join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                        join m in _db.tbl_department_master on d.department_id equals m.dept_id
                        join nd in _db.tbl_notification_description on d.notif_type_id equals nd.notif_decr_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notification.Exam_Notif_Id && n.login_id == notification.RoleId
                        select n).FirstOrDefault();
            return _mat;
        }

        public int? UpdateEmp(tbl_exam_notification_trans emp)
        {
            
            emp.exam_notif_status_id = 113;
            emp.updation_datetime = DateTime.Now;
            return _db.SaveChanges();

        }

        public int? UpdateStatus(tbl_exam_notification_trans emp)
        {
            return _db.SaveChanges();

        }

        public int? UpdateStatusMast(tbl_exam_notification_mast emp)
        {
           
                return _db.SaveChanges();
            
        }

        public int? UpdateNotificationDoc(tbl_exam_notification_trans emp)
        {
            return _db.SaveChanges();

        }

        public int? UpdateStatusforall(tbl_exam_notification_trans emp)
        {
            tbl_exam_notification_trans obj = (from n in _db.tbl_exam_notification_trans
                                               where n.exam_notif_trans_id == emp.exam_notif_trans_id
                                               select n).FirstOrDefault();

            obj.exam_notif_status_id = emp.exam_notif_status_id;
            obj.updated_by = emp.updated_by;
            obj.updation_datetime = DateTime.Now;
            int res = _db.SaveChanges();
            return res;


        }

        public List<Notification> GetNotificationStatus1DLL(Notification modal)
        {
            var notify = (from n in _db.tbl_exam_notification_mast
                          //join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                          join p in _db.tbl_exam_notif_status_mast on n.status_id equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on n.course_id equals c.course_id
                          join m in _db.tbl_department_master on n.department_id equals m.dept_id
                          join nd in _db.tbl_notification_description on n.notif_type_id equals nd.notif_decr_id
                          join u in _db.tbl_user_login on n.login_id equals u.id
                          //join com in _db.tbl_Comments_Transactions on d.exam_notif_id equals com.notification_id
                          //where n.login_id == modal.RoleId
                          where n.status_id != 111 && n.status_id!=100
                          select new Models.ExamNotification.Notification
                          {
                              Exam_Notif_Id = n.exam_notif_id,
                              Exam_Notif_Number = n.exam_notif_number,
                              NotifDescId = n.notif_type_id,
                              Exam_Notif_Desc = nd.notification_description,
                              Description = n.notif_description,
                              Exam_notif_date = (DateTime)n.exam_notif_date,
                              exam_notif_type = n.exam_notif_type,
                              fee_pay_last_date = (DateTime)n.fee_pay_last_date,
                              appli_from_last_date = (DateTime)n.appli_from_last_date,
                              trainee_name_eval_last_date = (DateTime)n.trainee_name_eval_last_date,
                              princ_sub_last_date = (DateTime)n.princ_sub_last_date,
                              jd_sub_last_date = (DateTime)n.jd_sub_last_date,
                              appli_charges_fee = n.appli_charges_fee,
                              exam_regular_fee = n.exam_regular_fee,
                              exam_repeater_fee = n.exam_repeater_fee,
                              penalty_after10days_fee = n.penalty_after10days_fee,
                              penalty_after30days_fee = n.penalty_after30days_fee,
                              penalty_after40days_fee = n.penalty_after40days_fee,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = n.status_id,
                              CourseTypeId = n.course_id,
                              CourseTypeName = c.course_type_name,
                              DeptId = n.department_id,
                              DeptName = m.dept_description,
                              SavePath = n.exam_notif_file_path,
                              created_by = n.created_by,
                              updated_by = n.updated_by,
                              By = u.short_name_designation,
                            //  updatedbyuser = u.user_name,
                              creation_datetime = (DateTime)n.creation_datetime,
                              updation_datetime = n.updation_datetime.HasValue ? n.updation_datetime : n.creation_datetime,
                              login_id = n.login_id,
                              comcount = 1
                              //comments = com.comments_transaction_desc
                          }
                ).OrderByDescending(x => x.updation_datetime).ToList();

            for (int i = 0; i < notify.Count; i++)
            {
              
                var count = notify[i].Exam_Notif_Id;
                var listcont = (from j in _db.tbl_comments_transaction
                                where j.notification_id == count
                                select j).ToList();
                notify[i].comcount = listcont.Count;
                
            }
            return notify;
        }

        public string CreateTransNotificationDLL(Notification model)
        {
            try
            {
                bool isActive = false;
                tbl_exam_notification_trans exam_Notification_Trans = _db.tbl_exam_notification_trans.Where(x => x.exam_notif_id == model.Exam_Notif_Id && x.login_id == model.RoleId).FirstOrDefault();
                if (exam_Notification_Trans == null)
                {
                    exam_Notification_Trans = new tbl_exam_notification_trans();
                    exam_Notification_Trans.exam_notif_id = model.Exam_Notif_Id;
                    if (exam_Notification_Trans.exam_notif_status_id ==111)
                    {
                        exam_Notification_Trans.exam_notif_status_id = 111;
                    }
                    else
                    {
                        exam_Notification_Trans.exam_notif_status_id = 113;
                    }
                        
                    exam_Notification_Trans.trans_date = DateTime.Now;
                    exam_Notification_Trans.is_active = isActive;
                    exam_Notification_Trans.creation_datetime = DateTime.Now;
                    exam_Notification_Trans.created_by = model.login_id;
                    exam_Notification_Trans.login_id = model.RoleId;
                    _db.tbl_exam_notification_trans.Add(exam_Notification_Trans);
                    _db.SaveChanges();

                }
                else
                {
                    exam_Notification_Trans = new tbl_exam_notification_trans();
                    exam_Notification_Trans.exam_notif_id = model.Exam_Notif_Id;
                    exam_Notification_Trans.exam_notif_status_id = 101;
                    exam_Notification_Trans.trans_date = null;
                    exam_Notification_Trans.is_active = isActive;
                    exam_Notification_Trans.updation_datetime = DateTime.Now;
                    exam_Notification_Trans.updated_by = model.login_id;
                    exam_Notification_Trans.login_id = model.RoleId;
                    _db.SaveChanges();
                }

                return "Saved";
            }

            catch (Exception ex)
            {
                return "Failed";
            }

        }

        public string SaveExamCalNotificationDLL(ExamCalendarMaster model)
        {
            //if(model.Course != null)
            //{
            //    tbl_course_type_mast tbl_course_type_mast = _db.tbl_course_type_mast.Where(x => x.course_type_name.ToUpper() == model.Course.Trim().ToUpper()).FirstOrDefault();
            //    if (tbl_course_type_mast != null)
            //    {

            //    }
            //    else
            //    {
            //        tbl_course_type_mast = new tbl_course_type_mast();
            //        tbl_course_type_mast.course_type_name = model.Course;
            //        tbl_course_type_mast.created_by = model.user_id;
            //        tbl_course_type_mast.is_active = true;
            //        tbl_course_type_mast.creation_datetime = DateTime.Now;
            //        _db.tbl_course_type_mast.Add(tbl_course_type_mast);
            //        _db.SaveChanges();
            //    }
            //    model.CourseTypeId = tbl_course_type_mast.course_id;
            //}

            if (model.TradeTypeName != null)
            {
                //tbl_trade_type_mast tbl_trade_type_mast = _db.tbl_trade_type_mast.Where(x => x.trade_type_name.ToUpper() == model.TradeTypeName.Trim().ToUpper() && x.trade_type_course_id == model.Course).FirstOrDefault();
                //if (tbl_trade_type_mast != null)
                //{
                //}
                //else
                //{
                tbl_trade_type_mast tbl_trade_type_mast = new tbl_trade_type_mast();
                tbl_trade_type_mast.trade_type_name = model.TradeTypeName;
                tbl_trade_type_mast.trade_type_created_by = model.user_id;
                tbl_trade_type_mast.trade_type_course_id = model.Course;
                tbl_trade_type_mast.trade_type_is_active = true;
                tbl_trade_type_mast.trade_type_creation_datetime = DateTime.Now;
                _db.tbl_trade_type_mast.Add(tbl_trade_type_mast);
                _db.SaveChanges();
                //}
                model.TradeTypeId = tbl_trade_type_mast.trade_type_id;
            }

            if (model.TradeName != null)
            {
                //tbl_trade_mast tbl_trade_mast = _db.tbl_trade_mast.Where(x => x.trade_name.ToUpper() == model.TradeName.Trim().ToUpper() && x.trade_course_id == model.Course).FirstOrDefault();
                //if (tbl_trade_mast != null)
                //{

                //}
                //else
                //{
                tbl_trade_mast tbl_trade_mast = new tbl_trade_mast();
                tbl_trade_mast.trade_name = model.TradeName;
                tbl_trade_mast.trade_type_id = model.TradeTypeId;
                tbl_trade_mast.trade_course_id = model.Course;
                tbl_trade_mast.trdae_is_active = true;
                tbl_trade_mast.trade_created_by = model.user_id;
                tbl_trade_mast.trade_creation_date = DateTime.Now;
                _db.tbl_trade_mast.Add(tbl_trade_mast);
                _db.SaveChanges();
                //}
                model.TradeId = tbl_trade_mast.trade_id;
            }

            if (model.SubjectTypeName != null)
            {
                //tbl_exam_subject_type_mast tbl_exam_subject_type_mast = _db.tbl_exam_subject_type_mast.Where(x => x.exam_subject_type_desc.ToUpper() == model.SubjectTypeName.Trim().ToUpper() && x.course_id == model.Course).FirstOrDefault();
                //if (tbl_exam_subject_type_mast != null)
                //{
                //}
                //else
                //{
                tbl_exam_subject_type_mast tbl_exam_subject_type_mast = new tbl_exam_subject_type_mast();
                tbl_exam_subject_type_mast.exam_subject_type_desc = model.SubjectTypeName;
                tbl_exam_subject_type_mast.course_id = model.Course;
                tbl_exam_subject_type_mast.exam_subject_type_is_active = true;
                tbl_exam_subject_type_mast.exam_subject_type_created_by = model.user_id;
                tbl_exam_subject_type_mast.exam_subject_type_creation_datetime = DateTime.Now;
                _db.tbl_exam_subject_type_mast.Add(tbl_exam_subject_type_mast);
                _db.SaveChanges();
                //}
                model.SubjectTypeId = tbl_exam_subject_type_mast.exam_subject_type_id;
            }

            if (model.SubjectName != null)
            {
                //tbl_exam_subject_mast tbl_exam_subject_mast = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_desc.ToUpper() == model.SubjectName.Trim().ToUpper() && x.exam_subject_type_id == model.SubjectTypeId && x.exam_course_id == model.Course).FirstOrDefault();
                //if (tbl_exam_subject_mast != null)
                //{

                //}
                //else
                //{
                tbl_exam_subject_mast tbl_exam_subject_mast = new tbl_exam_subject_mast();
                tbl_exam_subject_mast.exam_subject_desc = model.SubjectName;
                tbl_exam_subject_mast.exam_subject_type_id = model.SubjectTypeId;
                tbl_exam_subject_mast.exam_course_id = model.Course;
                tbl_exam_subject_mast.exam_subject_is_active = true;
                tbl_exam_subject_mast.exam_subject_created_by = model.user_id;
                tbl_exam_subject_mast.exam_subject_creation_datetime = DateTime.Now;
                _db.tbl_exam_subject_mast.Add(tbl_exam_subject_mast);
                _db.SaveChanges();
                //}
            }

            if (model.ExamSemName != null)
            {
                //tbl_exam_semester_mast tbl_exam_semester_mast = _db.tbl_exam_semester_mast.Where(x=>x.exam_semester_desc.ToUpper() == model.ExamSemName.Trim().ToUpper()).FirstOrDefault();
                //if (tbl_exam_semester_mast != null)
                //{

                //}
                //else
                //{
                tbl_exam_semester_mast tbl_exam_semester_mast = new tbl_exam_semester_mast();
                tbl_exam_semester_mast.exam_semester_desc = model.ExamSemName;
                tbl_exam_semester_mast.exam_semester_is_active = true;
                tbl_exam_semester_mast.exam_semester_created_by = model.user_id;
                tbl_exam_semester_mast.exam_semester_creation_datetime = DateTime.Now;
                _db.tbl_exam_semester_mast.Add(tbl_exam_semester_mast);
                _db.SaveChanges();
                //}
            }

            if (model.ExamTypeName != null)
            {
                //tbl_exam_type_mast tbl_exam_type_mast = _db.tbl_exam_type_mast.Where(x => x.exam_type_name.ToUpper() == model.ExamTypeName.Trim().ToUpper()).FirstOrDefault();
                //if (tbl_exam_type_mast != null)
                //{

                //}
                //else
                //{
                tbl_exam_type_mast tbl_exam_type_mast = new tbl_exam_type_mast();
                tbl_exam_type_mast.exam_type_name = model.ExamTypeName;
                tbl_exam_type_mast.exam_type_is_active = true;
                tbl_exam_type_mast.exam_type_created_by = model.user_id;
                tbl_exam_type_mast.exam_type_creation_datetime = DateTime.Now;
                _db.tbl_exam_type_mast.Add(tbl_exam_type_mast);
                _db.SaveChanges();
                //}
            }

            if (model.TradeYearName != null)
            {
                tbl_trade_year_mast tbl_trade_year_mast = new tbl_trade_year_mast();
                tbl_trade_year_mast.trade_year_name = model.TradeYearName;
                tbl_trade_year_mast.trade_year_is_active = true;
                tbl_trade_year_mast.trade_id = model.TradeId;
                tbl_trade_year_mast.trade_type_id = model.TradeTypeId;
                tbl_trade_year_mast.trade_year_created_by = model.user_id;
                tbl_trade_year_mast.trade_year_creation_datetime = DateTime.Now;
                _db.tbl_trade_year_mast.Add(tbl_trade_year_mast);
                _db.SaveChanges();
            }

            return "Success";
        }

        public DataTable ConvertXSLXtoDataTable(string strFilePath)
        {
            using (Workbook workbook = new Workbook())
            {
                workbook.LoadFromFile(strFilePath);
                Worksheet sheet = workbook.Worksheets[0];
                DataTable dt = sheet.ExportDataTable(sheet.AllocatedRange, false, true);
                dt = dt.AsEnumerable().Where(row => !row.ItemArray.All(f => f is null || String.IsNullOrWhiteSpace(f.ToString()))).CopyToDataTable();
                return dt;
            }
        }

        public string ExamCalenderMasterUploadDLL(ExamCalendarMaster model)
        {
            if (!string.IsNullOrEmpty(model.UploadDocs.FileName) && (model.UploadDocs.ContentLength > 0))
            {
                DataTable EntryDt_exam_semester_mast = new DataTable("EntryTable_exam_semester_mast");
                DataColumn exam_semester_desc = new DataColumn("exam_semester_desc", typeof(System.String));
                DataColumn exam_semester_is_active = new DataColumn("exam_semester_is_active", typeof(System.Boolean));
                DataColumn exam_semester_created_by = new DataColumn("exam_semester_created_by", typeof(System.Int16));
                DataColumn exam_semester_creation_datetime = new DataColumn("exam_semester_creation_datetime", typeof(System.DateTime));

                EntryDt_exam_semester_mast.Columns.AddRange(new DataColumn[] { exam_semester_desc, exam_semester_is_active, exam_semester_created_by, exam_semester_creation_datetime });

                DataTable EntryDt_exam_subject_mast = new DataTable("EntryTable_exam_subject_mast");
                DataColumn exam_subject_desc = new DataColumn("exam_subject_desc", typeof(System.String));
                DataColumn exam_subject_is_active = new DataColumn("exam_subject_is_active", typeof(System.Boolean));
                DataColumn exam_subject_type_id = new DataColumn("exam_subject_type_id", typeof(System.Int32));
                DataColumn exam_subject_created_by = new DataColumn("exam_subject_created_by", typeof(System.Int16));
                DataColumn exam_subject_creation_datetime = new DataColumn("exam_subject_creation_datetime", typeof(System.DateTime));

                EntryDt_exam_subject_mast.Columns.AddRange(new DataColumn[] { exam_subject_desc, exam_subject_is_active, exam_subject_type_id, exam_subject_created_by, exam_subject_creation_datetime });

                DataTable EntryDt_exam_subject_type_mast = new DataTable("EntryTable_exam_subject_type_mast");
                DataColumn exam_subject_type_desc = new DataColumn("exam_subject_type_desc", typeof(System.String));
                DataColumn exam_subject_type_is_active = new DataColumn("exam_subject_type_is_active", typeof(System.Boolean));
                DataColumn exam_subject_type_created_by = new DataColumn("exam_subject_type_created_by", typeof(System.Int16));
                DataColumn exam_subject_type_creation_datetime = new DataColumn("exam_subject_type_creation_datetime", typeof(System.DateTime));

                EntryDt_exam_subject_type_mast.Columns.AddRange(new DataColumn[] { exam_subject_type_desc, exam_subject_type_is_active, exam_subject_type_created_by, exam_subject_type_creation_datetime });

                DataTable EntryDt_tbl_trade_mast = new DataTable("EntryTable_tbl_trade_mast");
                DataColumn trade_name = new DataColumn("trade_name", typeof(System.String));
                DataColumn trdae_is_active = new DataColumn("trdae_is_active", typeof(System.Boolean));
                DataColumn trade_type_id = new DataColumn("trade_type_id", typeof(System.Int32));
                DataColumn trade_course_id = new DataColumn("trade_course_id", typeof(System.Int32));
                DataColumn trade_created_by = new DataColumn("trade_created_by", typeof(System.Int16));
                DataColumn trade_creation_date = new DataColumn("trade_creation_date", typeof(System.DateTime));

                EntryDt_tbl_trade_mast.Columns.AddRange(new DataColumn[] { trade_name, trdae_is_active, trade_type_id, trade_course_id, trade_created_by, trade_creation_date });

                DataTable EntryDt_trade_type_mast = new DataTable("EntryTable_trade_type_mast");
                DataColumn trade_type_name = new DataColumn("trade_type_name", typeof(System.String));
                DataColumn trade_type_is_active = new DataColumn("trade_type_is_active", typeof(System.Boolean));
                DataColumn trade_type_created_by = new DataColumn("trade_type_created_by", typeof(System.Int16));
                DataColumn trade_type_creation_datetime = new DataColumn("trade_type_creation_datetime", typeof(System.DateTime));

                EntryDt_trade_type_mast.Columns.AddRange(new DataColumn[] { trade_type_name, trade_type_is_active, trade_type_created_by, trade_type_creation_datetime });

                DataTable EntryDt_exam_type_mast = new DataTable("EntryTable_exam_type_mast");
                DataColumn exam_type_name = new DataColumn("exam_type_name", typeof(System.String));
                DataColumn exam_type_is_active = new DataColumn("exam_type_is_active", typeof(System.Boolean));
                DataColumn exam_type_created_by = new DataColumn("exam_type_created_by", typeof(System.Int16));
                DataColumn exam_type_creation_datetime = new DataColumn("exam_type_creation_datetime", typeof(System.DateTime));

                EntryDt_exam_type_mast.Columns.AddRange(new DataColumn[] { exam_type_name, exam_type_is_active, exam_type_created_by, exam_type_creation_datetime });

                DataTable EntryDt_trade_year_mast = new DataTable("EntryDt_trade_year_mast");
                DataColumn trade_year_name = new DataColumn("trade_year_name", typeof(System.String));
                DataColumn trade_id = new DataColumn("trade_id", typeof(System.Int32));
                DataColumn ytrade_type_id = new DataColumn("ytrade_type_id", typeof(System.Int32));
                DataColumn trade_year_is_active = new DataColumn("trade_year_is_active", typeof(System.Boolean));
                DataColumn trade_year_created_by = new DataColumn("trade_year_created_by", typeof(System.Int16));
                DataColumn trade_year_creation_datetime = new DataColumn("trade_year_creation_datetime", typeof(System.DateTime));

                EntryDt_trade_year_mast.Columns.AddRange(new DataColumn[] { trade_year_name, trade_id, ytrade_type_id, trade_year_is_active, trade_year_created_by, trade_year_creation_datetime });

                string path1 = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/Content/Uploads"), model.UploadDocs.FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/Uploads"));
                }
                if (File.Exists(path1))
                {
                    File.Delete(path1);
                }
                model.UploadDocs.SaveAs(path1);
                DataTable dt = ConvertXSLXtoDataTable(path1);
                if (dt.Columns.Count > 0)
                {
                    if (dt.Columns.Count == 8)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = dt.Rows[0][i].ToString();
                        }
                        DataRow row = dt.Rows[0];
                        dt.Rows.Remove(row);

                        foreach (DataRow dr in dt.Rows)
                        {
                            for (int i = 0; i < dr.ItemArray.Length; i++)
                            {
                                if (string.IsNullOrEmpty(dr.ItemArray[i].ToString()))
                                {
                                    return "Error";
                                }
                            }
                        }

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dt.Columns[0].ColumnName == "CourseName")
                            {
                                string CourseName = Convert.ToString(dt.Rows[j][0].ToString().ToUpper());
                                tbl_course_type_mast tbl_course_type_mast = _db.tbl_course_type_mast.Where(x => x.course_type_name.ToString().ToUpper() == CourseName).FirstOrDefault();
                                if (tbl_course_type_mast != null)
                                {

                                }
                                else
                                {
                                    tbl_course_type_mast = new tbl_course_type_mast();
                                    tbl_course_type_mast.course_type_name = CourseName;
                                    tbl_course_type_mast.created_by = model.user_id;
                                    tbl_course_type_mast.is_active = true;
                                    tbl_course_type_mast.creation_datetime = DateTime.Now;
                                    _db.tbl_course_type_mast.Add(tbl_course_type_mast);
                                    _db.SaveChanges();
                                }
                                model.CourseTypeId = tbl_course_type_mast.course_id;
                            }

                            if (dt.Columns[5].ColumnName == "TradeType")
                            {
                                string TradeTypeName = Convert.ToString(dt.Rows[j][5].ToString().ToUpper());
                                //tbl_trade_type_mast tbl_trade_type_mast = _db.tbl_trade_type_mast.Where(x => x.trade_type_name.ToUpper() == TradeTypeName && x.trade_type_course_id == model.CourseTypeId).FirstOrDefault();
                                //if (tbl_trade_type_mast != null)
                                //{
                                //}
                                //else
                                //{
                                tbl_trade_type_mast tbl_trade_type_mast = new tbl_trade_type_mast();
                                tbl_trade_type_mast.trade_type_name = TradeTypeName;
                                tbl_trade_type_mast.trade_type_created_by = model.user_id;
                                tbl_trade_type_mast.trade_type_course_id = model.CourseTypeId;
                                tbl_trade_type_mast.trade_type_is_active = true;
                                tbl_trade_type_mast.trade_type_creation_datetime = DateTime.Now;
                                _db.tbl_trade_type_mast.Add(tbl_trade_type_mast);
                                _db.SaveChanges();
                                // }
                                model.TradeTypeId = tbl_trade_type_mast.trade_type_id;
                            }

                            if (dt.Columns[2].ColumnName == "TradeName")
                            {
                                string TradeName = Convert.ToString(dt.Rows[j][2].ToString().ToUpper());
                                //tbl_trade_mast tbl_trade_mast = _db.tbl_trade_mast.Where(x => x.trade_name.ToUpper() == TradeName && x.trade_course_id == model.CourseTypeId).FirstOrDefault();
                                //if (tbl_trade_mast != null)
                                //{

                                //}
                                //else
                                //{
                                tbl_trade_mast tbl_trade_mast = new tbl_trade_mast();
                                tbl_trade_mast.trade_name = TradeName;
                                tbl_trade_mast.trade_type_id = model.TradeTypeId;
                                tbl_trade_mast.trade_course_id = model.CourseTypeId;
                                tbl_trade_mast.trdae_is_active = true;
                                tbl_trade_mast.trade_created_by = model.user_id;
                                tbl_trade_mast.trade_creation_date = DateTime.Now;
                                _db.tbl_trade_mast.Add(tbl_trade_mast);
                                _db.SaveChanges();
                                //}
                                model.TradeId = tbl_trade_mast.trade_id;

                            }

                            if (dt.Columns[6].ColumnName == "SubjectType")
                            {
                                string SubjectType = Convert.ToString(dt.Rows[j][6].ToString().ToUpper());
                                //tbl_exam_subject_type_mast tbl_exam_subject_type_mast = _db.tbl_exam_subject_type_mast.Where(x => x.exam_subject_type_desc.ToUpper() == SubjectType && x.course_id == model.CourseTypeId).FirstOrDefault();
                                //if (tbl_exam_subject_type_mast != null)
                                //{
                                //}
                                //else
                                //{
                                tbl_exam_subject_type_mast tbl_exam_subject_type_mast = new tbl_exam_subject_type_mast();
                                tbl_exam_subject_type_mast.exam_subject_type_desc = SubjectType;
                                tbl_exam_subject_type_mast.course_id = model.CourseTypeId;
                                tbl_exam_subject_type_mast.exam_subject_type_is_active = true;
                                tbl_exam_subject_type_mast.exam_subject_type_created_by = model.user_id;
                                tbl_exam_subject_type_mast.exam_subject_type_creation_datetime = DateTime.Now;
                                _db.tbl_exam_subject_type_mast.Add(tbl_exam_subject_type_mast);
                                _db.SaveChanges();
                                //}
                                model.SubjectTypeId = tbl_exam_subject_type_mast.exam_subject_type_id;

                            }

                            if (dt.Columns[4].ColumnName == "SubjectName")
                            {
                                string SubjectName = Convert.ToString(dt.Rows[j][4].ToString().ToUpper());
                                //tbl_exam_subject_mast tbl_exam_subject_mast = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_desc.ToUpper() == SubjectName && x.exam_subject_type_id == model.SubjectTypeId && x.exam_course_id == model.CourseTypeId).FirstOrDefault();
                                //if (tbl_exam_subject_mast != null)
                                //{

                                //}
                                //else
                                //{
                                tbl_exam_subject_mast tbl_exam_subject_mast = new tbl_exam_subject_mast();
                                tbl_exam_subject_mast.exam_subject_desc = SubjectName;
                                tbl_exam_subject_mast.exam_subject_type_id = model.SubjectTypeId;
                                tbl_exam_subject_mast.exam_course_id = model.CourseTypeId;
                                tbl_exam_subject_mast.exam_subject_is_active = true;
                                tbl_exam_subject_mast.exam_subject_created_by = model.user_id;
                                tbl_exam_subject_mast.exam_subject_creation_datetime = DateTime.Now;
                                _db.tbl_exam_subject_mast.Add(tbl_exam_subject_mast);
                                _db.SaveChanges();
                                //}
                            }

                            if (dt.Columns[3].ColumnName == "ExamSemester")
                            {
                                string ExamSemester = Convert.ToString(dt.Rows[j][3].ToString().ToUpper());
                                //tbl_exam_semester_mast tbl_exam_semester_mast = _db.tbl_exam_semester_mast.Where(x => x.exam_semester_desc.ToUpper() == ExamSemester).FirstOrDefault();
                                //if (tbl_exam_semester_mast != null)
                                //{

                                //}
                                //else
                                //{
                                tbl_exam_semester_mast tbl_exam_semester_mast = new tbl_exam_semester_mast();
                                tbl_exam_semester_mast.exam_semester_desc = ExamSemester;
                                tbl_exam_semester_mast.exam_semester_is_active = true;
                                tbl_exam_semester_mast.exam_semester_created_by = model.user_id;
                                tbl_exam_semester_mast.exam_semester_creation_datetime = DateTime.Now;
                                _db.tbl_exam_semester_mast.Add(tbl_exam_semester_mast);
                                _db.SaveChanges();
                                //}
                            }

                            if (dt.Columns[1].ColumnName == "ExamType")
                            {
                                string ExamType = Convert.ToString(dt.Rows[j][1].ToString().ToUpper());
                                //tbl_exam_type_mast tbl_exam_type_mast = _db.tbl_exam_type_mast.Where(x => x.exam_type_name.ToUpper() == ExamType).FirstOrDefault();
                                //if (tbl_exam_type_mast != null)
                                //{

                                //}
                                //else
                                //{
                                tbl_exam_type_mast tbl_exam_type_mast = new tbl_exam_type_mast();
                                tbl_exam_type_mast.exam_type_name = ExamType;
                                tbl_exam_type_mast.exam_type_is_active = true;
                                tbl_exam_type_mast.exam_type_created_by = model.user_id;
                                tbl_exam_type_mast.exam_type_creation_datetime = DateTime.Now;
                                _db.tbl_exam_type_mast.Add(tbl_exam_type_mast);
                                _db.SaveChanges();
                                // }
                            }

                            if (dt.Columns[7].ColumnName == "TradeYear")
                            {
                                string TradeYear = Convert.ToString(dt.Rows[j][7].ToString().ToUpper());
                                tbl_trade_year_mast tbl_trade_year_mast = new tbl_trade_year_mast();
                                tbl_trade_year_mast.trade_year_name = TradeYear;
                                tbl_trade_year_mast.trade_year_is_active = true;
                                tbl_trade_year_mast.trade_id = model.TradeId;
                                tbl_trade_year_mast.trade_type_id = model.TradeTypeId;
                                tbl_trade_year_mast.trade_year_created_by = model.user_id;
                                tbl_trade_year_mast.trade_year_creation_datetime = DateTime.Now;
                                _db.tbl_trade_year_mast.Add(tbl_trade_year_mast);
                                _db.SaveChanges();

                            }
                        }
                    }
                    else
                        return "Error";
                }
                else
                    return "Error";
            }
            return "Saved";
        }


        int GetValues(string val, int item)
        {
            Int32 newID = 0;
            string sql = string.Empty;
            if (item == 0)
            {
                sql = "SELECT course_id FROM tbl_course_type_mast WHERE UPPER(course_type_name) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            if (item == 1)
            {
                sql = "SELECT exam_type_id FROM tbl_exam_type_mast WHERE UPPER(exam_type_name) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 2)
            {
                sql = "SELECT trade_id FROM tbl_trade_mast WHERE UPPER(trade_name) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 5)
            {
                sql = "SELECT exam_semester_id FROM tbl_exam_semester_mast WHERE UPPER(exam_semester_desc) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 6)
            {
                sql = "SELECT exam_subject_id FROM tbl_exam_subject_mast WHERE UPPER(exam_subject_desc) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 7)
            {
                sql = "SELECT trade_type_id FROM tbl_trade_type_mast WHERE UPPER(trade_type_name) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 9)
            {
                sql = "SELECT exam_subject_type_id FROM tbl_exam_subject_type_mast WHERE UPPER(exam_subject_type_desc) = '" + val.Trim().ToString().ToUpper() + "'";
            }
            else if (item == 10)
            {
                var trade_id = val.Split('-')[0];
                var trade_type_id = val.Split('-')[1];
                sql = "SELECT trade_year_id FROM tbl_trade_year_mast WHERE trade_id = " + trade_id + " and trade_type_id = " + trade_type_id;
            }
            else if (item == 11)
            {
                var exam_type_id = Convert.ToInt32(val);
                sql = "SELECT exam_calender_id FROM tbl_exam_calendar_mast WHERE exam_type_id = " + exam_type_id;
            }

            using (SqlConnection conn = new SqlConnection(SQLConnection))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    newID = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (Int32)newID;
        }

        public SelectList GetLoginRoleListDLL(int? login)
        {
            List<SelectListItem> UserLoginList = _db.tbl_user_login.Where(n => n.id <= 12 && n.id != login).Select(x => new SelectListItem
            {
                Text = x.user_name.ToString(),
                Value = x.id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select",

            };
            UserLoginList.Insert(0, ProposalList);
            return new SelectList(UserLoginList, "Value", "Text");
        }

        public SelectList GetLoginRoleListDLL()
        {
            List<SelectListItem> UserLoginList = _db.tbl_user_login.Select(x => new SelectListItem
            {
                Text = x.user_name.ToString(),
                Value = x.id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select",

            };
            UserLoginList.Insert(0, ProposalList);
            return new SelectList(UserLoginList, "Value", "Text");
        }
        public string CreateCommentsNotificationDLL(Notification model)
        {
            try
            {
                //char isActive = 'F';
                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = model.Exam_Notif_Id;
                exam_comments_Trans.module_id = 1;
                //exam_comments_Trans.status_id = ;
                //exam_comments_Trans.updation_datetime = DateTime.Now;
                //exam_comments_Trans.created_by = model.login_id;
                //exam_comments_Trans.updated_by = 2;
                exam_comments_Trans.login_id = model.RoleId;
                exam_comments_Trans.ct_created_by = model.login_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();

                return "Saved";
            }

            catch (Exception ex)
            {
                return "Failed";
            }

        }

        public List<Notification> GetCommentsFileDLL(int NotificationId)
        {


            //var notify = (
            //              from n in _db.tbl_comments_transaction
            //              join e in _db.tbl_exam_notification_mast on n.notification_id equals e.exam_notif_id
            //              join u in _db.tbl_user_login on n.login_id equals u.id
            //              where n.notification_id == NotificationId && n.module_id == 1
            //              select new Models.ExamNotification.Notification
            //              {
            //                  comments = n.comments_transaction_desc,
            //                  login_id = n.login_id,
            //                  user_name = u.user_name,
            //                  Exam_Notif_Number = e.exam_notif_number,
            //                  created_by = n.ct_created_by,
            //                  createdatetime = n.ct_created_on.ToString(),
            //              }
            //    ).ToList();
            //return notify;
            var notify = (
                          from n in _db.tbl_comments_transaction
                          join e in _db.tbl_exam_notification_mast on n.notification_id equals e.exam_notif_id
                          join u in _db.tbl_user_login on n.login_id equals u.id
                          where n.notification_id == NotificationId && n.module_id == 1
                          // orderby n.ct_created_on descending
                          orderby n.comments_transaction_id descending
                          select new Models.ExamNotification.Notification
                          { //comments = n.comments_transaction_desc,
                            //login_id = n.login_id,
                            //user_name = u.user_name,
                            //Exam_Notif_Number = e.exam_notif_number,
                            //created_by = n.ct_created_by,
                            //creationdatetime = n.ct_created_on.ToString(),

                              creationdatetime = n.ct_created_on.ToString(),
                              //login_id = n.login_id,
                              user_name = u.id.ToString(),
                              By = n.ct_created_by.ToString(),
                              comments = n.comments_transaction_desc,
                              Status = ("Forwarded To " + u.user_name).ToString(),
                          }
                ).ToList();
            for (int i = 0; i < notify.Count; i++)
            {
                var ById = notify[i].By;
                var UserName = (from j in _db.tbl_user_login
                                where j.id.ToString() == ById
                                select j.user_name).FirstOrDefault();
                notify[i].By = UserName;
            }
            return notify;
        }

        public string PublishNotificationDLL(Notification notification)
        {
            try
            {
                bool isActive = false;
                tbl_exam_notification_trans exam_Notification_Trans = _db.tbl_exam_notification_trans.Where(x => x.exam_notif_id == notification.Exam_Notif_Id).FirstOrDefault();
                if (exam_Notification_Trans == null)
                {
                    exam_Notification_Trans = new tbl_exam_notification_trans();
                    exam_Notification_Trans.exam_notif_id = notification.Exam_Notif_Id;
                    exam_Notification_Trans.exam_notif_status_id = notification.exam_notif_status_id;
                    exam_Notification_Trans.trans_date = DateTime.Now;
                    exam_Notification_Trans.is_active = isActive;
                    exam_Notification_Trans.creation_datetime = DateTime.Now;
                    exam_Notification_Trans.updation_datetime = DateTime.Now;
                    exam_Notification_Trans.created_by = notification.login_id;
                    exam_Notification_Trans.updated_by = 2;
                    exam_Notification_Trans.login_id = notification.RoleId;
                    _db.tbl_exam_notification_trans.Add(exam_Notification_Trans);
                    _db.SaveChanges();

                }
                else
                {
                    exam_Notification_Trans.exam_notif_id = notification.Exam_Notif_Id;
                    exam_Notification_Trans.exam_notif_status_id = notification.exam_notif_status_id;
                    exam_Notification_Trans.trans_date = DateTime.Now;
                    exam_Notification_Trans.is_active = isActive;
                    exam_Notification_Trans.creation_datetime = DateTime.Now;
                    exam_Notification_Trans.updation_datetime = DateTime.Now;
                    exam_Notification_Trans.created_by = notification.login_id;
                    exam_Notification_Trans.updated_by = 2;
                    //    exam_Notification_Trans.login_id = notification.RoleId;

                    _db.SaveChanges();
                }

                return "Saved";
            }

            catch (Exception ex)
            {
                return "Failed";
            }
        }

        public string stringSaveNotifiedSubjectsDLL(ExamCalendarMaster model)
        {
            try
            {
                if (model.ectId != 0)
                {
                    tbl_Exam_Cal_Notif_mast tbl_Exam_Cal_Notif_Mast = _db.tbl_exam_cal_notif_mast.Where(x => x.exam_notif_id.ToString() == model.Exam_CalNotif_Number).FirstOrDefault();
                    if (tbl_Exam_Cal_Notif_Mast != null)
                    {
                        tbl_Exam_Cal_Notif_Mast.exam_cal_status_id = model.IsDraft == 1 ? 100 : 111;
                        tbl_Exam_Cal_Notif_Mast.ecn_updated_by = model.user_id;
                        tbl_Exam_Cal_Notif_Mast.ecn_is_active = model.IsDraft == 1 ? true : false;
                        tbl_Exam_Cal_Notif_Mast.ecn_updation_datetime = DateTime.Now;
                        _db.SaveChanges();

                        tbl_exam_subject_trans tbl_exam_subject_trans = _db.tbl_exam_subject_trans.Where(x => x.est_transactionid == model.ectId).FirstOrDefault();
                        if (tbl_exam_subject_trans != null)
                        {
                            var res = from r in _db.tbl_exam_subject_trans
                                      where r.est_transactionid == model.ectId
                                      select r;

                            foreach (var r in res)
                            {
                                _db.tbl_exam_subject_trans.Remove(r);
                            }
                            _db.SaveChanges();
                        }

                        if (model.NotifiationListList != null)
                        {
                            if (model.NotifiationListList.objSubjectsItem != null)
                            {
                                foreach (var Item in model.NotifiationListList.objSubjectsItem)
                                {
                                    tbl_exam_subject_trans = new tbl_exam_subject_trans();
                                    tbl_exam_subject_trans.est_exam_date = Convert.ToDateTime(Item.Exam_Date);
                                    tbl_exam_subject_trans.est_exam_start_date = Convert.ToDateTime(Item.Exam_Start_Time);
                                    tbl_exam_subject_trans.exam_end_date = Item.Exam_End_Time;
                                    tbl_exam_subject_trans.est_exam_subject_type_id = Item.SubjectTypeID;
                                    tbl_exam_subject_trans.est_exam_subject_id = Item.SubjectId;
                                    tbl_exam_subject_trans.est_transactionid = tbl_Exam_Cal_Notif_Mast.ecn_id;

                                    tbl_exam_subject_trans.est_course_id = Item.CourseTypeId;
                                    tbl_exam_subject_trans.est_trade_type_id = 15;
                                    tbl_exam_subject_trans.est_trade_id = 14;
                                    tbl_exam_subject_trans.est_trade_year_id = 4;
                                    tbl_exam_subject_trans.est_exam_type_id = Item.ExamTypeId;
                                    //tbl_exam_subject_trans.exam_end_date = Item.Exam_End_Time;
                                    tbl_exam_subject_trans.exam_semester_id = 11;
                                    tbl_exam_subject_trans.est_exam_year = DateTime.Now.Year;
                                    tbl_exam_subject_trans.est_subject_type_id = Item.SujectTtypeId;

                                    tbl_exam_subject_trans.est_is_active = true;
                                    tbl_exam_subject_trans.est_created_by = model.user_id;
                                    tbl_exam_subject_trans.est_creation_datetime = DateTime.Now;

                                    _db.tbl_exam_subject_trans.Add(tbl_exam_subject_trans);
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                }
                else
                {
                    //tbl_Exam_Cal_Notif_mast tbl_Exam_Cal_Notif_Mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_number == model.Exam_CalNotif_Number).FirstOrDefault();
                    tbl_Exam_Cal_Notif_mast tbl_Exam_Cal_Notif_Mast = _db.tbl_exam_cal_notif_mast.Where(x => x.exam_notif_id.ToString() == model.Exam_CalNotif_Number).FirstOrDefault();
                    if (tbl_Exam_Cal_Notif_Mast == null)
                    {
                        tbl_exam_notification_mast tbl_exam_notification_mast = _db.tbl_exam_notification_mast.Where(x => x.exam_notif_id.ToString() == model.Exam_CalNotif_Number).FirstOrDefault();
                        tbl_Exam_Cal_Notif_Mast = new tbl_Exam_Cal_Notif_mast();
                        tbl_Exam_Cal_Notif_Mast.exam_notif_id = tbl_exam_notification_mast.exam_notif_id;
                        //tbl_Exam_Cal_Notif_Mast.ecn_number = model.Exam_CalNotif_Number;
                        tbl_Exam_Cal_Notif_Mast.ecn_desc = model.Exam_CalNotif_Description;
                        tbl_Exam_Cal_Notif_Mast.ecn_Date = model.EExam_Date;
                        tbl_Exam_Cal_Notif_Mast.ecn_is_active = model.IsDraft == 1 ? true : false;
                        tbl_Exam_Cal_Notif_Mast.ecn_created_by = model.user_id;
                        tbl_Exam_Cal_Notif_Mast.exam_cal_status_id = model.IsDraft == 1 ? 100 : 111;
                        tbl_Exam_Cal_Notif_Mast.course_type_id = model.CourseTypeId;
                        tbl_Exam_Cal_Notif_Mast.login_id = model.user_id;
                        tbl_Exam_Cal_Notif_Mast.ecn_creation_datetime = DateTime.Now;
                        _db.tbl_exam_cal_notif_mast.Add(tbl_Exam_Cal_Notif_Mast);
                        _db.SaveChanges();
                        model.ectId = tbl_Exam_Cal_Notif_Mast.ecn_id;
                    }
                    else
                        return "Exists";

                    if (model.NotifiationListList != null)
                    {
                        if (model.NotifiationListList.objSubjectsItem != null)
                        {
                            foreach (var Item in model.NotifiationListList.objSubjectsItem)
                            {
                                tbl_exam_subject_trans tbl_exam_subject_trans = new tbl_exam_subject_trans();
                                tbl_exam_subject_trans.est_exam_date = Convert.ToDateTime(Item.Exam_Date);
                                tbl_exam_subject_trans.est_exam_start_date = Convert.ToDateTime(Item.Exam_Start_Time);
                                tbl_exam_subject_trans.est_exam_subject_type_id = Item.SubjectTypeID;
                                tbl_exam_subject_trans.est_exam_subject_id = Item.SubjectId;
                                tbl_exam_subject_trans.est_transactionid = tbl_Exam_Cal_Notif_Mast.ecn_id;

                                tbl_exam_subject_trans.est_course_id = Item.CourseTypeId;
                                tbl_exam_subject_trans.est_trade_type_id = 15;
                                tbl_exam_subject_trans.est_trade_id = 14;
                                tbl_exam_subject_trans.est_trade_year_id = 4;
                                tbl_exam_subject_trans.est_exam_type_id = Item.ExamTypeId;
                                tbl_exam_subject_trans.exam_end_date = Convert.ToDateTime(Item.Exam_End_Time);
                                tbl_exam_subject_trans.exam_semester_id = 11;// Item.ExamSemId;
                                tbl_exam_subject_trans.est_exam_year = DateTime.Now.Year;
                                tbl_exam_subject_trans.est_subject_type_id = Item.SujectTtypeId;

                                tbl_exam_subject_trans.est_is_active = true;
                                tbl_exam_subject_trans.est_created_by = model.user_id;
                                tbl_exam_subject_trans.est_creation_datetime = DateTime.Now;

                                _db.tbl_exam_subject_trans.Add(tbl_exam_subject_trans);
                                _db.SaveChanges();
                            }
                        }
                    }
                }

                return "Success-" + model.ectId + "-" + model.PublishedId;
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<ExamCalendarMaster> GetNotificationForApprovalDLL(ExamCalendarMaster model)
        {
            List<ExamCalendarMaster> notify = null;

            if (model.RoleId == 1)
            {
                notify = (from n in _db.tbl_exam_cal_notif_mast
                              join s in _db.tbl_exam_notif_status_mast on n.exam_cal_status_id equals s.exam_notif_status_id
                              join en in _db.tbl_exam_notification_mast on n.exam_notif_id equals en.exam_notif_id
                              join l in _db.tbl_user_login on n.login_id equals l.id
                              join d in _db.tbl_department_master on en.department_id equals d.dept_id
                              join c in _db.tbl_course_type_mast on n.course_type_id equals c.course_id

                              select new ExamCalendarMaster
                              {
                                  Exam_CalNotif_Number = en.exam_notif_number,// n.ecn_number,
                                  Exam_CalNotif_Description = n.ecn_desc,
                                  status = n.ecn_is_published == 0 ? s.exam_notif_status_desc : s.exam_notif_status_desc,
                                  Notif_Date = n.ecn_Date,
                                  NotificationId = n.ecn_id,
                                  status_id = s.exam_notif_status_id,
                                  PublishedId = n.ecn_is_published,
                                  PdfPath = n.exam_notif_file_path,
                                  CourseName = c.course_type_name,
                                  //   DeptName = "Exam",
                                  Login_Id = n.login_id,
                                  Notif_Present = l.user_name,
                                  Notif_Number_Id = n.exam_notif_id,
                                  DeptName = d.dept_description,
                                  DateTimeUpdate = n.ecn_updation_datetime.HasValue ? n.ecn_updation_datetime : n.ecn_creation_datetime,
                                  By = l.short_name_designation,
                              }).OrderByDescending(x => x.DateTimeUpdate).ToList();

                if (model.user_id != 2 && notify.Count > 0)
                {
                    var itemToRemove = notify.RemoveAll(r => r.status == "Draft Notification");
                }

                for (int i = 0; i < notify.Count; i++)
                {
                    var count = notify[i].NotificationId;
                    var listcont = (from j in _db.tbl_comments_transaction
                                    where j.notification_id == count && j.module_id == 2
                                    select j).ToList();
                    notify[i].comcount = listcont.Count;
                }

            }
            else
            {
                notify = (from n in _db.tbl_exam_cal_notif_mast
                          join s in _db.tbl_exam_notif_status_mast on n.exam_cal_status_id equals s.exam_notif_status_id
                          join en in _db.tbl_exam_notification_mast on n.exam_notif_id equals en.exam_notif_id
                          join l in _db.tbl_user_login on n.login_id equals l.id
                          join d in _db.tbl_department_master on en.department_id equals d.dept_id
                          join c in _db.tbl_course_type_mast on n.course_type_id equals c.course_id
                          where n.exam_cal_status_id != 100
                              select new ExamCalendarMaster
                              {
                                  Exam_CalNotif_Number = en.exam_notif_number,// n.ecn_number,
                                  Exam_CalNotif_Description = n.ecn_desc,
                                  status = n.ecn_is_published == 0 ? s.exam_notif_status_desc : s.exam_notif_status_desc,
                                  Notif_Date = n.ecn_Date,
                                  NotificationId = n.ecn_id,
                                  status_id = s.exam_notif_status_id,
                                  PublishedId = n.ecn_is_published,
                                  PdfPath = n.exam_notif_file_path,
                                  CourseName = c.course_type_name,
                                  //   DeptName = "Exam",
                                  Login_Id = n.login_id,
                                  Notif_Present = l.user_name,
                                  Notif_Number_Id = n.exam_notif_id,
                                  DeptName = d.dept_description,
                                  DateTimeUpdate = n.ecn_updation_datetime.HasValue ? n.ecn_updation_datetime : n.ecn_creation_datetime,
                                  By = l.short_name_designation,
                              }).OrderByDescending(x => x.DateTimeUpdate).ToList();

                if (model.user_id != 2 && notify.Count > 0)
                {
                    var itemToRemove = notify.RemoveAll(r => r.status == "Draft Notification");
                }

                for (int i = 0; i < notify.Count; i++)
                {
                    var count = notify[i].NotificationId;
                    var listcont = (from j in _db.tbl_comments_transaction
                                    where j.notification_id == count && j.module_id == 2
                                    select j).ToList();
                    notify[i].comcount = listcont.Count;
                }

            }

            return notify;
        }

        public List<ExamCalendarMaster> GetPublishedNotificationDLL(ExamCalendarMaster model)
        {
            var notify = (from n in _db.tbl_exam_cal_notif_mast
                          join en in _db.tbl_exam_notification_mast on n.exam_notif_id equals en.exam_notif_id
                          join s in _db.tbl_exam_notif_status_mast on n.exam_cal_status_id equals s.exam_notif_status_id
                          where n.exam_cal_status_id == 105
                          select new ExamCalendarMaster
                          {
                              Exam_CalNotif_Number = en.exam_notif_number,// n.ecn_number,
                              Exam_CalNotif_Description = n.ecn_desc,
                              status = s.exam_notif_status_desc,
                              Notif_Date = n.ecn_Date,
                              NotificationId = n.ecn_id,
                              status_id = s.exam_notif_status_id,
                              PublishedId = n.ecn_is_published,
                              PdfPath = n.exam_notif_file_path,
                          }).ToList();

            return notify;
        }

        public List<subjects> GetSubjectsItemListDLL(int NotificationId)
        {
            List<subjects> SubjectsItemList = (from cal in _db.tbl_exam_cal_notif_mast
                                               join est in _db.tbl_exam_subject_trans on cal.ecn_id equals est.est_transactionid
                                               //join sub in _db.tbl_exam_subject_mast on est.est_exam_subject_type_id equals sub.exam_subject_type_id
                                               join subtype in _db.tbl_exam_subject_type_mast on est.est_exam_subject_type_id equals subtype.exam_subject_type_id
                                               where est.est_transactionid == NotificationId
                                               select new subjects
                                               {
                                                   ExamDate = (est.est_exam_date.Day + "-" + est.est_exam_date.Month + "-" + est.est_exam_date.Year).ToString(),
                                                   ExamDay = (est.est_exam_date.Year + "-" + est.est_exam_date.Month + "-" + est.est_exam_date.Day).ToString(),
                                                   //STime = (est.est_exam_start_date.Hour + ":" + est.est_exam_start_date.Minute) + ((est.est_exam_start_date.Hour >= 12 && est.est_exam_start_date.Minute >= 00) ? " PM" : " AM").ToString(),
                                                   //STime = (est.est_exam_start_date.Hour + ":" + est.est_exam_start_date.Minute) + (est.est_exam_start_date.Hour >= 12 && est.est_exam_start_date.Minute >= 0 ? " PM" : " AM").ToString(),
                                                   //ETime = (est.exam_end_date.Hour + ":" + est.exam_end_date.Minute).ToString(),
                                                   SubjectType = subtype.exam_subject_type_desc,
                                                   //Subject = GetSubjectNamefromId(sub.exam_subject_id.ToString()),
                                                   SubjectId = est.est_exam_subject_id,
                                                   SubjectTypeID = subtype.exam_subject_type_id,
                                                   CourseTypeId = est.est_course_id,
                                                   ExamSemId = est.exam_semester_id,
                                                   ExamTypeId = est.est_exam_type_id,
                                                   Exam_Start_Time = est.est_exam_start_date,
                                                   Exam_End_Time = est.exam_end_date,
                                                   TradeId = est.est_trade_id,
                                                   TradeTypeId = est.est_trade_type_id,
                                                   TradeYearId = est.est_trade_year_id,
                                                   ExamYear = est.est_exam_year,
                                                   Exam_Date = est.est_exam_date,
                                                   SujectTtypeId = est.est_subject_type_id,

                                               }).Distinct().ToList();

            for (int i = 0; i < SubjectsItemList.Count; i++)
            {
                SubjectsItemList[i].Subject = GetSubjectNamefromId(SubjectsItemList[i].SubjectId);
                SubjectsItemList[i].STime = SubjectsItemList[i].Exam_Start_Time.ToString("hh:mm tt");
                SubjectsItemList[i].ETime = SubjectsItemList[i].Exam_End_Time.ToString("hh:mm tt");
            }

            return SubjectsItemList;
        }

        public string GetSubjectNamefromId(string SubjectIds)
        {
            var d = SubjectIds.Split(',');
            var stringBuilder = "";
            for (int i = 0; i < d.Length; i++)
            {
                if (!string.IsNullOrEmpty(d[i]))
                {
                    var subjectId = d[i];
                    var tbl_exam_subject_mast = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_id.ToString() == subjectId).FirstOrDefault();
                    stringBuilder += stringBuilder != "" ? "," + tbl_exam_subject_mast.exam_subject_desc : tbl_exam_subject_mast.exam_subject_desc;
                }
            }
            return stringBuilder.ToString();
        }

        public string SaveRemarkAndForwardToUserDLL(Notification model)
        {
            tbl_Exam_Cal_Notif_mast tbl_exam_cal_notif_mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_id == model.Exam_Notif_Id).FirstOrDefault();
            if (tbl_exam_cal_notif_mast != null)
            {
                if (model.role_id == 1)
                {
                    tbl_exam_cal_notif_mast.exam_cal_status_id = model.IsForward == 1 ? 113 : 113;

                }
                else
                {
                    tbl_exam_cal_notif_mast.exam_cal_status_id = model.IsForward == 1 ? 101 : 110;
                }
                if (model.IsForward == 1)
                    tbl_exam_cal_notif_mast.login_id = model.SelectedRoleId;
                else
                    tbl_exam_cal_notif_mast.login_id = 2;
                tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = model.Exam_Notif_Id;
                exam_comments_Trans.module_id = 2;
                exam_comments_Trans.is_published = tbl_exam_cal_notif_mast.ecn_is_published;


                exam_comments_Trans.ct_is_active = true;
                if (model.IsForward == 1 && model.role_id!= 1)
                {
                    exam_comments_Trans.login_id = model.SelectedRoleId;
                    exam_comments_Trans.status_id = 101;
                }
                else if(model.IsForward == 1 && model.role_id == 1)
                {
                    exam_comments_Trans.login_id = model.SelectedRoleId;
                    exam_comments_Trans.status_id = 113;

                }
                else
                {
                    exam_comments_Trans.status_id = 103;
                    exam_comments_Trans.login_id = 2;
                }
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();
            }

            return "Success";
        }

        public List<WorkflowActionDetails> GetCommentRemarksDetailsDLL(int NotificationId)
        {
            List<WorkflowActionDetails> actions = (from cn in _db.tbl_exam_cal_notif_mast
                                                   join cc in _db.tbl_comments_transaction on cn.ecn_id equals cc.notification_id
                                                   join l in _db.tbl_user_login on cc.login_id equals l.id
                                                   where cn.ecn_id == NotificationId && cc.module_id == 2
                                                   orderby cc.ct_created_on descending
                                                   select new WorkflowActionDetails
                                                   {
                                                       DateInfo = cc.ct_created_on.ToString(),
                                                       UserName = l.id.ToString(),
                                                       By = cc.ct_created_by.ToString(),
                                                       Comment = cc.comments_transaction_desc.ToString(),
                                                       Status =  l.user_name.ToString(),
                                                   }).ToList();


            for (int i = 0; i < actions.Count; i++)
            {
                var ById = actions[i].By;
                var UserName = (from j in _db.tbl_user_login
                                where j.id.ToString() == ById
                                select j.user_name).FirstOrDefault();
                actions[i].By = UserName;
            }

            return actions;
        }
        public string ChangesToModificationDLL(Notification model)
        {
            tbl_Exam_Cal_Notif_mast tbl_exam_cal_notif_mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_id == model.Exam_Notif_Id).FirstOrDefault();

            if (tbl_exam_cal_notif_mast != null)
            {
                tbl_exam_cal_notif_mast.login_id = 2;
                tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                tbl_exam_cal_notif_mast.exam_cal_status_id = 102;
                tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = model.Exam_Notif_Id;
                exam_comments_Trans.module_id = 2;
                exam_comments_Trans.status_id = 102;
                exam_comments_Trans.ct_is_active = true;
                exam_comments_Trans.login_id = 2;
                exam_comments_Trans.is_published = model.PublishedId;
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();

                if(tbl_exam_cal_notif_mast.exam_cal_status_id == 102)
                {
                    try
                    {
                        tbl_exam_cal_notif_trans exam_Cal_Notif_Trans = _db.tbl_exam_cal_notif_trans.Where(x => x.exam_cal_notif_id == tbl_exam_cal_notif_mast.exam_notif_id).FirstOrDefault();
                        if (exam_Cal_Notif_Trans == null)
                        {
                            exam_Cal_Notif_Trans = new tbl_exam_cal_notif_trans();
                            exam_Cal_Notif_Trans.exam_cal_notif_id = tbl_exam_cal_notif_mast.ecn_id;
                            exam_Cal_Notif_Trans.exam_cal_notif_status_id = tbl_exam_cal_notif_mast.exam_cal_status_id;
                            exam_Cal_Notif_Trans.trans_date = DateTime.Now;
                            exam_Cal_Notif_Trans.is_active = true;
                            exam_Cal_Notif_Trans.created_by = tbl_exam_cal_notif_mast.ecn_created_by;
                            exam_Cal_Notif_Trans.creation_datetime = tbl_exam_cal_notif_mast.ecn_creation_datetime;
                            _db.tbl_exam_cal_notif_trans.Add(exam_Cal_Notif_Trans);
                            _db.SaveChanges();
                        }
                        else
                        {
                            exam_Cal_Notif_Trans.exam_cal_notif_id = tbl_exam_cal_notif_mast.ecn_id; 
                            exam_Cal_Notif_Trans.exam_cal_notif_status_id = tbl_exam_cal_notif_mast.exam_cal_status_id;
                            exam_Cal_Notif_Trans.trans_date = DateTime.Now;
                            exam_Cal_Notif_Trans.is_active = true;
                            exam_Cal_Notif_Trans.updated_by = tbl_exam_cal_notif_mast.ecn_updated_by;
                            exam_Cal_Notif_Trans.updation_datetime = tbl_exam_cal_notif_mast.ecn_updation_datetime;

                            _db.SaveChanges();
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                    
                    
                    
                }
            }
            return "Success";
        }

        public string GoBackToModificationDLL(Notification model)
        {
            tbl_Exam_Cal_Notif_mast tbl_exam_cal_notif_mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_id == model.Exam_Notif_Id).FirstOrDefault();

            //var banckLoginId = (from k in _db.tbl_comments_transaction
            //                    where k.notification_id == model.Exam_Notif_Id
            //                    orderby k.ct_created_on descending
            //                    select k.ct_created_by).FirstOrDefault();

            if (model.role_id.ToString() != "" && tbl_exam_cal_notif_mast != null)
            {
                tbl_exam_cal_notif_mast.login_id = model.role_id;
                tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                tbl_exam_cal_notif_mast.exam_cal_status_id = 109;
                tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = model.Exam_Notif_Id;
                exam_comments_Trans.module_id = 2;
                exam_comments_Trans.status_id = model.role_id.ToString() == "2" ? 102 : 109;
                exam_comments_Trans.ct_is_active = true;
                exam_comments_Trans.login_id = model.role_id;
                exam_comments_Trans.is_published = model.PublishedId;
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();
            }
            return "Success";
        }

        public DataTable GetNotificationForSubjectsDLL(int NotificationId, int PublishedId)
        {
            List<subjects> SubjectsItemList = (from cal in _db.tbl_exam_cal_notif_mast
                                               join est in _db.tbl_exam_subject_trans on cal.ecn_id equals est.est_transactionid
                                               //join sub in _db.tbl_exam_subject_mast on est.est_exam_subject_type_id equals sub.exam_subject_type_id
                                               join subtype in _db.tbl_exam_subject_type_mast on est.est_exam_subject_type_id equals subtype.exam_subject_type_id
                                               where est.est_transactionid == NotificationId
                                               select new subjects
                                               {
                                                   ExamDate = (est.est_exam_date.Month + "-" + est.est_exam_date.Day + "-" + est.est_exam_date.Year).ToString(),
                                                   //ExamDate = (est.est_exam_date.Day + "-" + est.est_exam_date.Month + "-" + est.est_exam_date.Year).ToString(),
                                                   //ExamDay = (est.est_exam_date.DayOfWeek).ToString(),
                                                   Exam_Start_Time = est.est_exam_date,
                                                   ETime = (est.est_exam_start_date.Hour + ":" + est.est_exam_start_date.Minute).ToString(),
                                                   SubjectType = subtype.exam_subject_type_desc,
                                                   //Subject = sub.exam_subject_desc,
                                                   SubjectId = est.est_exam_subject_id.ToString(),
                                                   SubjectTypeID = subtype.exam_subject_type_id,
                                                   NotificationNumber = cal.exam_notif_id,// cal.ecn_number,
                                                   CreationDate = cal.ecn_Date.ToString(),
                                                   currentYear = (DateTime.Now.Year).ToString(),
                                                   PTime = est.est_exam_start_date,
                                                   statusID = cal.exam_cal_status_id
                                                   

                                               }).ToList();

            for (int i = 0; i < SubjectsItemList.Count; i++)
            {
                SubjectsItemList[i].Subject = GetSubjectNamefromId(SubjectsItemList[i].SubjectId);
            }

            DataTable boundTable = new DataTable();


            boundTable.Columns.Add("ExamDate", typeof(String));
            boundTable.Columns.Add("ExamDay", typeof(String));
            boundTable.Columns.Add("ETime", typeof(String));
            boundTable.Columns.Add("SubjectType", typeof(String));
            boundTable.Columns.Add("Subject", typeof(String));
            boundTable.Columns.Add("NotificationNumber", typeof(String));
            boundTable.Columns.Add("CreationDate", typeof(String));
            boundTable.Columns.Add("currentYear", typeof(String));
            boundTable.Columns.Add("PTime", typeof(DateTime));

            DateTime dateValue;
            DateTimeOffset dateOffsetValue;
            DateTime CreationDate;
            DateTime currentYear;

            foreach (var sub in SubjectsItemList)
            {
                dateValue = DateTime.Parse(sub.ExamDate, System.Globalization.CultureInfo.InvariantCulture);
                dateOffsetValue = new DateTimeOffset(dateValue,
                                             TimeZoneInfo.Local.GetUtcOffset(dateValue));

                CreationDate = DateTime.Parse(sub.CreationDate, System.Globalization.CultureInfo.InvariantCulture);
                currentYear = DateTime.Parse(sub.CreationDate, System.Globalization.CultureInfo.InvariantCulture);

                DataRow dr = boundTable.NewRow();
                dr["ExamDate"] = dateValue.ToString("dd-MM-yyyy");
                dr["ExamDay"] = dateValue.ToString("dddd");
                dr["ETime"] = sub.ETime;
                dr["SubjectType"] = sub.SubjectType;
                dr["Subject"] = sub.Subject;
                dr["NotificationNumber"] = sub.NotificationNumber;
                dr["CreationDate"] = CreationDate.ToString("dd-MM-yyyy");
                dr["currentYear"] = currentYear.ToString("yyyy");
                dr["PTime"] = sub.PTime;

                boundTable.Rows.Add(dr);
            }
            return boundTable;

        }

        public string UpdateNotificationFileDLL(ExamCalendarMaster model)
        {
            tbl_Exam_Cal_Notif_mast tbl_exam_cal_notif_mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_id == model.NotificationId).FirstOrDefault();
            if (tbl_exam_cal_notif_mast != null)
            {
                if (model.FromReg == 1)
                {
                    tbl_exam_cal_notif_mast.exam_notif_file_path = model.PdfPath;
                    tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                    tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                    _db.SaveChanges();
                }
                else
                {
                   List<ExamCalendarMaster> Notifs = (from n in _db.tbl_exam_cal_notif_trans
                              where n.exam_cal_notif_id == model.NotificationId
                              select new ExamCalendarMaster
                              {
                                 ectId = n.exam_cal_notif_id,
                                 status_id = n.exam_cal_notif_status_id
                              }).ToList();
                          
                    tbl_exam_cal_notif_mast.exam_notif_file_path = model.PdfPath;
                    tbl_exam_cal_notif_mast.ecn_is_published = 1;

                    
                        if(Notifs.Count >0 && Notifs!=null)
                        {
                            foreach(var item in Notifs)
                            {
                                if(item.status_id==102)
                                {
                                    tbl_exam_cal_notif_mast.exam_cal_status_id = 2119;
                                }
                            }
                        }
                        else
                        {
                            tbl_exam_cal_notif_mast.exam_cal_status_id = 105;
                        }
                    
                    
                    
                    tbl_exam_cal_notif_mast.login_id = 2;
                    tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                    tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                    _db.SaveChanges();
                }
            }
            return "Success";
        }

        public List<Notification> GetNotificationListDLL()
        {
            List<Notification> Notifs = null;

            Notifs = (from n in _db.tbl_exam_cal_notif_mast
                      join desc in _db.tbl_exam_notif_status_mast on n.exam_cal_status_id equals desc.exam_notif_status_id
                      where (n.ecn_is_published == 1 && n.exam_cal_status_id == 105) || (n.exam_cal_status_id == 2119)
                      select new Notification
                      {
                          Exam_Cal_Notif_Number = n.exam_notif_id,
                          exam_notif_status_desc = desc.exam_notif_status_desc,
                          Exam_notif_date = n.ecn_Date,
                          SavePath = n.exam_notif_file_path,
                      }).ToList();

            return Notifs;
        }

        public List<SelectListItem> GetTradeYearListBasedOnIdDLL(int TradeId)
        {
            List<SelectListItem> TradeYearList = _db.tbl_trade_year_mast.Where(x => x.trade_id == TradeId).Select(x => new SelectListItem { Text = x.trade_year_name, Value = x.trade_year_id.ToString() }).ToList();

            var TradeYearTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Trade Year"
            };
            TradeYearList.Insert(0, TradeYearTList);
            return TradeYearList;
        }

        public string RejectNotificationDLL(Notification model)
        {
            tbl_Exam_Cal_Notif_mast tbl_exam_cal_notif_mast = _db.tbl_exam_cal_notif_mast.Where(x => x.ecn_id == model.Exam_Notif_Id).FirstOrDefault();
            if (tbl_exam_cal_notif_mast != null)
            {
                tbl_exam_cal_notif_mast.exam_cal_status_id = 101;
                tbl_exam_cal_notif_mast.login_id = 9;
                tbl_exam_cal_notif_mast.ecn_updated_by = model.user_id;
                tbl_exam_cal_notif_mast.ecn_updation_datetime = DateTime.Now;
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = model.Exam_Notif_Id;
                exam_comments_Trans.module_id = 2;
                exam_comments_Trans.status_id = 101;
                exam_comments_Trans.ct_is_active = true;
                exam_comments_Trans.login_id = 9;
                exam_comments_Trans.is_published = model.PublishedId;
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();
            }
            return "Success";
        }

        public List<Notification> GetCommentsListDLL(int NotificationId)
        {
            try
            {
                var notify = (
                                          from n in _db.tbl_comments_transaction
                                          join e in _db.tbl_exam_notification_mast on n.notification_id equals e.exam_notif_id
                                          join u in _db.tbl_user_login on n.login_id equals u.id
                                          where n.notification_id == NotificationId && n.module_id == 1

                                          //orderby n.ct_created_on 
                                          orderby n.comments_transaction_id descending
                                          select new Models.ExamNotification.Notification
                                          { //comments = n.comments_transaction_desc,
                                            //login_id = n.login_id,
                                            //user_name = u.user_name,
                                            //Exam_Notif_Number = e.exam_notif_number,
                                            //created_by = n.ct_created_by,
                                            //creationdatetime = n.ct_created_on.ToString(),
                                              id = n.comments_transaction_id,
                                              creationdatetime = n.ct_created_on.ToString(),
                                              //login_id = n.login_id,
                                              user_name = u.id.ToString(),
                                              By = n.ct_created_by.ToString(),
                                              comments = n.comments_transaction_desc,
                                              Status = (u.user_name).ToString(),
                                          }
                                ).ToList();
                for (int i = 0; i < notify.Count; i++)
                {
                    var ById = notify[i].By;
                    var UserName = (from j in _db.tbl_user_login
                                    where j.id.ToString() == ById
                                    select j.user_name).FirstOrDefault();
                    notify[i].By = UserName;
                }
                return notify;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public List<SelectListItem> GetTradeTypeListBasedOnIdDLL(int CourseTypeId)
        {
            List<SelectListItem> TradeList = _db.tbl_trade_type_mast.Where(x => x.trade_type_course_id == CourseTypeId).Select(x => new SelectListItem { Text = x.trade_type_name, Value = x.trade_type_id.ToString() }).ToList();

            var TradeTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Trade Type"
            };
            TradeList.Insert(0, TradeTList);
            return TradeList;
        }

        public List<SelectListItem> GetSubjectTypeListBasedOnIdDLL(int CourseTypeId)
        {
            List<SelectListItem> TradeList = _db.tbl_exam_subject_type_mast.Where(x => x.course_id == CourseTypeId).Select(x => new SelectListItem { Text = x.exam_subject_type_desc, Value = x.exam_subject_type_id.ToString() }).ToList();

            var TradeTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Trade Type"
            };
            TradeList.Insert(0, TradeTList);
            return TradeList;
        }

        public SelectList GetDivisionListDLL(int? divisionId)
        {
            List<SelectListItem> CoursetList1 = null;
            if (divisionId != null)
            {
                CoursetList1 = _db.tbl_division_master.Where(x => x.division_id == divisionId).Select(x => new SelectListItem
                {
                    Text = x.division_name.ToString(),
                    Value = x.division_id.ToString()
                }).ToList();
            }
            else
            {
                CoursetList1 = _db.tbl_division_master.Select(x => new SelectListItem
                {
                    Text = x.division_name.ToString(),
                    Value = x.division_id.ToString()
                }).ToList();
            }
           
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Division",

            };
            CoursetList1.Insert(0, ProposalList);
            return new SelectList(CoursetList1, "Value", "Text");
        }

        public SelectList GetDistrictDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_district_master.Select(x => new SelectListItem
            {
                Text = x.district_ename.ToString(),
                Value = x.district_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select District",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        public SelectList GetDistrictbasedonDivisionIDDLL(int Div_ID)
        {

            List<SelectListItem> DistrictList = _db.tbl_district_master.Where(x => x.division_id == Div_ID).Select(x => new SelectListItem { Text = x.district_ename, Value = x.district_id.ToString() }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select District",

            };
            DistrictList.Insert(0, ProposalList);
            return new SelectList(DistrictList, "Value", "Text");
            //return new SelectList(DistrictList);/*, "Value", "Text");*/

        }

        public SelectList GetCollegeDLL()
        {

            List<SelectListItem> CollegeList = _db.tbl_iti_college_details.Select(x => new SelectListItem
            {
                Text = x.iti_college_name.ToString(),
                Value = x.iti_college_id.ToString()

            }).ToList();

            //var ProposalList = new SelectListItem()
            //{
            //    Value=null,
            //    Text="Select College",
            //};
            //CollegeList.Insert(0, ProposalList);
            return new SelectList(CollegeList, "Value", "Text");
        }
        public SelectList GetCollegetNamebasedonDivisionIDDLL(int Dist_ID)
        {
            // Dist_ID = 1;
            List<SelectListItem> _districtList = _db.tbl_iti_college_details.Where(x => x.district_id == Dist_ID).Select(y => new SelectListItem { Text = y.iti_college_name + "-" + y.iti_college_code, Value = y.iti_college_id.ToString() }).ToList();
            return new SelectList(_districtList, "Value", "Text");
        }

        public SelectList GetCollegeCodebasedonCollegeIDDLL(string Col_Code_ID)
        {
            List<SelectListItem> Col_Code_Value = _db.tbl_iti_college_details.Where(x => x.iti_college_name.ToString() == Col_Code_ID).Select(x => new SelectListItem { Text = x.iti_college_code, Value = x.iti_college_id.ToString() }).ToList();

            return new SelectList(Col_Code_Value);

        }
        //BNM  for question paper set population

        public SelectList GetQuestionPaperSetListDLL()
        {
            List<SelectListItem> CoursetList1 = _db.tbl_questionpaper_set.Select(x => new SelectListItem
            {
                Text = x.qp_set.ToString(),
                Value = x.qp_id.ToString()
            }).ToList();

            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Question Paper Set",

            };
            CoursetList1.Insert(0, ProposalList);
            return new SelectList(CoursetList1, "Value", "Text");
        }


        public List<QuestionPaper> getGetSearchModifyQuestionPapersDLL(int CourseTypeID, int TradeTypeID, int TradeID,int TradeYearID, int ExamTypeID, int ExamSubTypeID, int ExamSubID)
        {
            List<QuestionPaper> model = new List<QuestionPaper>();

            //CourseTypeID = 100;
            //TradeTypeID = 15;
            //TradeID  = 4;
            //ExamTypeID = 9;
            //ExamSubTypeID  = 9;
            //ExamSubID = 22;

            using (SqlConnection con = new SqlConnection(SQLConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("getGetSearchModifyQuestionPapers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@CourseTypeID", CourseTypeID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@TradeTypeID", TradeTypeID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@TradeID", TradeID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@TradeYearID", TradeYearID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@ExamTypeID", ExamTypeID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@ExamSubTypeID", ExamSubTypeID).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@ExamSubID", 23).SqlDbType = SqlDbType.Int;
               //cmd.Parameters.AddWithValue("@ExamSubID", ExamSubID).SqlDbType = SqlDbType.Int;
                con.Open();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                model = ds.Tables[0].AsEnumerable().Select(row =>
                    new QuestionPaper
                    {
                        QPSTID= row.Field<int>("qpst_id"),
                        CourseType = row.Field<string>("course_type_name"),
                        TradeType = row.Field<string>("trade_type_name"),
                        TradeYear = row.Field<string>("trade_year_name"),
                        ExamType = row.Field<string>("exam_type_name"),
                        SubjectType = row.Field<string>("exam_subject_type_desc"),
                        Subject = row.Field<string>("exam_subject_desc"),
                        QP = row.Field<string>("qpst_file_path"),
                        //  ExamDate= row.Field<string>("qpst_file_path"),

                    }).ToList();
            }
            return model;




        }

        //public string SaveMappedDLL(string[] Check_values, Exam_Center ECModel)
        //{
        //    try
        //    {
        //        //tbl_exam_centers EC = _db.tbl_exam_centers.Where(x => x.ec_id == ECModel.ec_id).FirstOrDefault();return new SelectList(CollegeList, "Value", "Text");
        //        tbl_exam_centers EC = new tbl_exam_centers();

        //        tbl_iti_college_details iti_college = new tbl_iti_college_details();
        //        tbl_iti_trainees_details iti_trainee = new tbl_iti_trainees_details();
        //        tbl_iti_trainees_mast iti_tr_mast = new tbl_iti_trainees_mast();
        //        tbl_taluk_mast it_taluk_data = new tbl_taluk_mast();
        //        List<SelectListItem> taluk_data = _db.tbl_taluk_mast.Select(x => new SelectListItem
        //        {
        //            Text = x.taluk_id.ToString(),
        //            //Value = x.iti_college_id.ToString()

        //        }).ToList();

        //        if (EC != null)
        //        {
        //            EC.ec_name = Check_values[0];
        //            EC.ec_email_id = "d1@gmail.com";
        //            EC.ec_phone_num = "8660578344";
        //            EC.division_id = ECModel.division_id;
        //            EC.district_id = ECModel.district_id;
        //            //EC.taluk_id = Convert.ToInt32(taluk_data[0].Text);
        //            EC.ec_is_active = true;
        //            EC.ec_created_by = 1;
        //            EC.ec_creation_datetime = DateTime.Now;
        //            EC.ec_updated_by = 1;
        //            EC.ec_updation_datetime = DateTime.Now;
        //            EC.ec_status_id = 100;
        //            EC.ec_remarks = "Mapping here";

        //            EC.ec_code = "Code-1";
        //            _db.tbl_exam_centers.Add(EC);
        //            _db.SaveChanges();
        //            tbl_exam_centre_mapping_trns ECMTrans = new tbl_exam_centre_mapping_trns();
        //            List<SelectListItem> iti_trainee_data = _db.tbl_iti_trainees_mast.Select(x => new SelectListItem
        //            {
        //                Text = x.iti_trainees_id.ToString(),
        //                //Value = x.iti_college_id.ToString()

        //            }).ToList();



        //            List<SelectListItem> EC_Data_List = _db.tbl_exam_centers.Select(x => new SelectListItem
        //            {
        //                Text = x.ec_id.ToString(),
        //                //Value = x.iti_college_id.ToString()

        //            }).ToList();

        //            List<SelectListItem> College_Data_List = _db.tbl_iti_college_details.Select(x => new SelectListItem
        //            {
        //                Text = x.iti_college_id.ToString(),
        //                //Value = x.iti_college_id.ToString()

        //            }).ToList();

        //            ECMTrans.exam_centre_id = Convert.ToInt32(EC_Data_List[0].Text);
        //            ECMTrans.iti_college_id = Convert.ToInt32(College_Data_List[0].Text);
        //            ECMTrans.trainee_id = Convert.ToInt32(iti_trainee_data[0].Text);
        //            ECMTrans.is_active = true;
        //            ECMTrans.created_by = 1;
        //            ECMTrans.creation_datetime = DateTime.Now;
        //            ECMTrans.updated_by = 1;
        //            ECMTrans.updation_datetime = DateTime.Now;
        //            ECMTrans.esmt_status_id = 100;
        //            ECMTrans.esmt_remarks = "mapping data here";
        //            _db.tbl_exam_centre_mapping_trns.Add(ECMTrans);
        //            _db.SaveChanges();
        //        }
        //        return "Sucess";

        //    }
        //    catch (Exception ex)
        //    {
        //        return "Failed";
        //    }
        //}

        public string GetExamdateBasedOnIdDLL(int? SubjectID)
        {
            //var res = "";
            //var dateAndTime = DateTime.Now;
            //var date = dateAndTime.Date;
            //res = date.ToString();
            //res = res.Split(' ')[0];
            //return res;
            string model = "";



            using (SqlConnection con = new SqlConnection(SQLConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("usp_GetExamDate", con);
                cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@subjectid", SubjectID).SqlDbType = SqlDbType.Int;
                con.Open();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                //model = ds.Tables[0].
                model =Convert.ToString(ds.Tables[0].Rows[0]["ExamDate"]);//.ToString();
                    //new Subjects
                    //{
                    //    SlNo = row.Field<Int64>("slno"),
                    //    Day = row.Field<string>("Weekday"),
                    //    Date = row.Field<string>("ExamDay"),
                    //    StartTime = row.Field<string>("StartTime"),
                    //    EndTime = row.Field<string>("EndTime"),
                    //    Subject = row.Field<string>("Subject"),

                //}).ToList();
            }
            return model;
        }

        public List<SelectListItem> GetTradeListBasedOnIdDLL(int TradeTypeId)
        {
            List<SelectListItem> TradeList = _db.tbl_trade_mast.Where(x => x.trade_type_id == TradeTypeId).Select(x => new SelectListItem { Text = x.trade_name, Value = x.trade_id.ToString() }).ToList();

            var TradeTList = new SelectListItem()
            {
                Value = null,
                Text = "Select Subject"
            };
            TradeList.Insert(0, TradeTList);
            return TradeList;
        }

        //public List<SelectListItem> GetTradeYearListBasedOnIdDLL(int TradeId)
        //{
        //    List<SelectListItem> TradeYearList = _db.tbl_trade_year_mast.Where(x => x.trade_id == TradeId).Select(x => new SelectListItem { Text = x.trade_year_name, Value = x.trade_year_id.ToString() }).ToList();

        //    var TradeYearTList = new SelectListItem()
        //    {
        //        Value = null,
        //        Text = "Select Subject"
        //    };
        //    TradeYearList.Insert(0, TradeYearTList);
        //    return TradeYearList;
        //}
        //public string GetExamdateBasedOnIdDLL(int SubjectID)
        //{
        //    var res = "";
        //    var dateAndTime = DateTime.Now;
        //    var date = dateAndTime.Date;
        //    res = date.ToString();
        //    res = res.Split(' ')[0];
        //    return res;
        //}
        
        public bool isQPExistsorNot(string @subject_id, int @exam_type_id, int @trade_year_id, int @subject_type_id, int @course_id, int @trade_type_id, int @trade_id, string @qpst_remarks)
        {
            //var result="";
            bool status = false;
            using (SqlConnection con = new SqlConnection(SQLConnection))
            {

                // SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("usp_IsQuestionPaperExistsOrNot", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@subject_id", @subject_id).SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@exam_type_id", @exam_type_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@trade_year_id", @trade_year_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@subject_type_id", @subject_type_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@course_id", @course_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@trade_type_id", @trade_type_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@trade_id", @trade_id).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@qpst_remarks", @qpst_remarks).SqlDbType = SqlDbType.VarChar;

                // var returnParameter = cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                //  returnParameter.Direction = ParameterDirection.ReturnValue;

                con.Open();
                status = Convert.ToBoolean(cmd.ExecuteScalar());
                // SqlDataReader reader = cmd.ExecuteReader();
                con.Close();
            }
            return status;
        }


        public string stringSendQPDLL(QuestionPaperSets model)
        {
            bool abc = false;
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    if (model.QuestionPaperSetsList.objQuestionPaperSetsItem != null)
                    {
                        foreach (var item in model.QuestionPaperSetsList.objQuestionPaperSetsItem)
                        {
                            
                             abc = isQPExistsorNot(item.SubjectId,item.ExamTypeId,item.TradeYearId,item.SubjectTypeId,item.CourseTypeId,item.TradeTypeId,item.TradeId,item.QuestionPaper);
                           // tbl_question_paper_set_trns paper_Set_Trns = _db.tbl_question_paper_set_trns.Where(x => x.subject_id == item.SubjectId).FirstOrDefault();
                            {
                              //  if (paper_Set_Trns != null)
                               // {
                                  
                                    if (abc == true)
                                    {
                                    tbl_question_paper_set_trns paper_Set_Trns = new tbl_question_paper_set_trns();
                                        paper_Set_Trns.subject_id = item.SubjectId;
                                        paper_Set_Trns.exam_calendar_id = 2;
                                        //tbl_question_paper_set_trns.status_id = model.status_id;
                                        paper_Set_Trns.qpst_file_path = item.UploadFiles;
                                        paper_Set_Trns.qpst_remarks = item.QuestionPaper;
                                        //tbl_question_paper_set_trns.qpst_file_path = model.qpst_file_path;
                                        paper_Set_Trns.exam_type_id = Convert.ToInt32(item.ExamTypeId);
                                        paper_Set_Trns.trade_year_id = item.TradeYearId;
                                        paper_Set_Trns.subject_type_id = Convert.ToInt32(item.SubjectTypeId);
                                        paper_Set_Trns.course_id = Convert.ToInt16(item.CourseTypeId);
                                        paper_Set_Trns.trade_type_id = item.TradeTypeId;
                                        paper_Set_Trns.trade_id = item.TradeId;
                                        paper_Set_Trns.qpst_is_active = true;
                                        paper_Set_Trns.qpst_created_by = 2;
                                        paper_Set_Trns.qpst_creation_datetime = DateTime.Today;
                                        _db.tbl_question_paper_set_trns.Add(paper_Set_Trns);

                                        _db.SaveChanges();
                                    }
                                    else
                                    {

                                    }

                               // }
                               // else
                                //{
                                //    paper_Set_Trns.subject_id = item.SubjectId;
                                //    paper_Set_Trns.exam_calendar_id = 2;
                                //    //tbl_question_paper_set_trns.status_id = model.status_id;
                                //    paper_Set_Trns.qpst_file_path = item.UploadFiles;
                                //    paper_Set_Trns.qpst_remarks = item.QuestionPaper;
                                //    //tbl_question_paper_set_trns.qpst_file_path = model.qpst_file_path;
                                //    paper_Set_Trns.exam_type_id = Convert.ToInt32(item.ExamTypeId);
                                //    paper_Set_Trns.trade_year_id = item.TradeYearId;
                                //    paper_Set_Trns.subject_type_id = Convert.ToInt32(item.SubjectTypeId);
                                //    paper_Set_Trns.course_id = Convert.ToInt16(item.CourseTypeId);
                                //    paper_Set_Trns.trade_type_id = item.TradeTypeId;
                                //    paper_Set_Trns.qpst_is_active = true;
                                //    paper_Set_Trns.qpst_updated_by = 2;
                                //    paper_Set_Trns.qpst_updation_datetime = DateTime.Today;

                                //    _db.SaveChanges();
                                //}
                            }

                            //  tbl_question_paper_set_trns tbl_question_paper_set_trns = new tbl_question_paper_set_trns();



                        }



                    }
                    if(abc==false)
                    {
                        return "AlreadyExists";
                    }
                    else
                    {
                        transaction.Commit();
                        return "Success";
                    }
                    
                
                }
            catch (Exception ex)
            {
                    transaction.Rollback();
                    return "Failed";
            }
        }
        }

        public List<QuestionPaper> getQuestionPaperDll()
        {
            List<QuestionPaper> model = new List<QuestionPaper>();

            using (SqlConnection con = new SqlConnection(SQLConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("usp_GetQP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@ScreenId", 1).SqlDbType = SqlDbType.Int;
                con.Open();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                //            var groupedCustomerList = ds.Tables[0].AsEnumerable()
                //.GroupBy(u => u.)
                //.Select(grp => grp.ToList())
                //.ToList();
                //            var model1 = ds.Tables[0].AsEnumerable().GroupBy(d => new { CourseType = d.Field<string>("CourseType"), V = d.Field<String>("supplierArticle") })
                //                    .Select(g => new { _Date = g.Key.Date, Article = g.Key.V, Qty = g.Sum(r => r.Field<int>("quantity")) }).ToList();

                //            var grouped = ds.Tables[0].AsEnumerable()
                //.GroupBy(r => { r., r.isSelected })
                //.Select(g => new {
                //    storedId = g.Key.storedId,
                //    isSelected = g.Key.isSelected,
                //    invoicesList = g.Select(i => new {
                //        storeName = i.storeName,
                //        partyCode = i.partyCode
                //    })
                //));

                model = ds.Tables[0].AsEnumerable().Select(row =>
                    new QuestionPaper
                    {
                        //CourseType, TradeType,SubjectType,Trade, TradeYear , ExamType ,	, Sub, ExamDate , QP, Files

                        CourseType = row.Field<string>("CourseType"),
                        TradeType = row.Field<string>("TradeType"),
                        Trade = row.Field<string>("SubjectType"),
                        TradeYear = row.Field<string>("TradeYear"),
                        ExamType = row.Field<string>("ExamType"),
                        SubjectType = row.Field<string>("Trade"),
                        Subject = row.Field<string>("Subject"),
                        ExamDate = row.Field<DateTime?>("ExamDate"),
                        QP = row.Field<string>("QP")
                    }).ToList();
                //.GroupBy(x => new { x.CourseType, x.ExamType, x.SubjectType, x.Subject }).Select(x => new
                //{
                   

                //}
                // //{
                 //    //x.Key.Id,
                 //  //  x.Key.Name,
                 //    Ingredients = string.Join("," x.Where(y => y.IngrName != null).Select(y => $"{y.Quantity} {y.UOM} {y.Name}").Distinct()),
                 //    Tags = string.Join("," x.Where(y => y.TagName != null).Select(y => y.TagName).Distinct())
                 //}).ToList();
            }
            return model;
        }


        
              public List<QuestionPaperApprovedByHofQC> GetApprovedQPByHofQPCommitteDll()
        {


            List<QuestionPaperApprovedByHofQC> model = new List<QuestionPaperApprovedByHofQC>();
            try
            {


                using (SqlConnection con = new SqlConnection(SQLConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand("usp_GetApprovedQPByHofQPCommitte", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // cmd.Parameters.AddWithValue("@ScreenId", 1).SqlDbType = SqlDbType.Int;
                    con.Open();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    model = ds.Tables[0].AsEnumerable().Select(row =>
                        new QuestionPaperApprovedByHofQC
                        {
                            CourseType = row.Field<string>("CourseType"),
                            TradeType = row.Field<string>("TradeType"),
                            Trade = row.Field<string>("SubjectType"),
                            TradeYear = row.Field<string>("TradeYear"),
                            ExamType = row.Field<string>("ExamType"),
                            SubjectType = row.Field<string>("Trade"),
                            Subject = row.Field<string>("Subject"),
                            ExamDate = row.Field<string>("ExamDate"),
                            QP = row.Field<string>("QP")
                        }).ToList();
                }
            }
            catch(Exception ex)
            {

            }
            return model;
        }

        public SelectList GetQPStatusDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_course_type_mast.Select(x => new SelectListItem
            {
                Text = x.course_type_name.ToString(),
                Value = x.course_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Status",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }


        public string saveApproveQuestionPaperbyHeadofQPCommitteDll(ApproveQuestionPaperbyHeadofQPCommitte obj)
        {
            // List<ApproveQuestionPaperbyHeadofQPCommitte> model = new List<ApproveQuestionPaperbyHeadofQPCommitte>();
            
                using (var transaction = _db.Database.BeginTransaction())
                {
                try
                {
                   // foreach (var obj in model.ApproveQuestionPaperbyHeadofQPCommitteList)
                        //foreach (var obj in model.ApproveQuestionPaperbyHeadofQPCommitteList)
                        //{
                        //  tbl_SaveApproved_QP_by_HofQPC mapping_Trns= _db.tbl_SaveApproved_QP_by_HofQPC.Where(x => x.Id == obj).FirstOrDefault();
                        tbl_SaveApproved_QP_by_HofQPC objSave_Trans = new tbl_SaveApproved_QP_by_HofQPC();
                        objSave_Trans.CourseType = obj.CourseType;
                        objSave_Trans.TradeType = obj.TradeType;
                        objSave_Trans.Trade = obj.Trade;
                        objSave_Trans.TradeYear =obj.TradeYear ;
                        objSave_Trans.ExamType = obj.ExamType;
                        objSave_Trans.SubjectType = obj.SubjectType;
                        objSave_Trans.Subject =obj.Subject;
                        objSave_Trans.ExamDate = obj.ExamDate;
                        objSave_Trans.QP =obj.QP;
                        objSave_Trans.UploadedFile =obj.UploadedFile;
                        objSave_Trans.isSelected = obj.isSelected;
                        objSave_Trans.Remarks =obj.Remarks;
                        objSave_Trans.Status = obj.Status;
                    _db.tbl_SaveApproved_QP_by_HofQPC.Add(objSave_Trans);
                    _db.SaveChanges();

                    tbl_question_paper_set_trns obj1 = new tbl_question_paper_set_trns();
                    obj1.status_id = 0;

                    //}
                    transaction.Commit();
                    return "Success";

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }

                //try
                //{
                //    bool isActive = false;
                //}
            }
        }

        public int? LastNotificationNumberDLL(Notification notification)
        {
            string year = string.Format("{0}-{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            var lastNotificationNumberRecord = (from n in _db.tbl_exam_notification_mast
                                          orderby n.exam_notif_id descending
                                          where n.exam_notif_number.Contains(year)
                                          select n).FirstOrDefault();
            int lastNotificationNumber = 0;
            // extract last nofification number e.g DITE/SCVT/TRG/Exam/301/2021-2022
            if (lastNotificationNumberRecord != null)
            {
                string szNotificationNumber = lastNotificationNumberRecord.exam_notif_number;
                // try find from /2021-2022
                int index = szNotificationNumber.IndexOf("/" + year);
                if(index > 0)
                {
                    // get the string ends with number e.g. DITE/SCVT/TRG/Exam/301
                    szNotificationNumber = szNotificationNumber.Substring(0, index);
                    index = szNotificationNumber.LastIndexOf("/");
                    if(index > 0)
                    {
                        //extract the number e.g. "301"
                        szNotificationNumber = szNotificationNumber.Substring(index + 1);
                        try
                        {
                            lastNotificationNumber = Convert.ToInt32(szNotificationNumber);
                        }
                        catch
                        { }
                    }
                }
            }
         
            return lastNotificationNumber;
        }

        public List<ExamCalendarMaster> GetPublishedNotificationIDDLL(ExamCalendarMaster model)
        {
            List<ExamCalendarMaster> Notifs = (from n in _db.tbl_exam_cal_notif_trans
                                               where n.exam_cal_notif_id == model.NotificationId
                                               select new ExamCalendarMaster
                                               {
                                                   ectId = n.exam_cal_notif_id,
                                                   status_id = n.exam_cal_notif_status_id
                                               }).ToList();
            return Notifs;
        }

        public SelectList GetSpecialTradeTypeListDLL()
        {
            List<SelectListItem> pecialTradeTypeList = _db.tbl_special_trade_types.Select(x => new SelectListItem
            {
                Text = x.stt_description.ToString(),
                Value = x.stt_id.ToString()
            }).ToList();

            var TradeYList = new SelectListItem()
            {

                Value = null,
                Text = "Select Trade Type",

            };
            pecialTradeTypeList.Insert(0, TradeYList);
            return new SelectList(pecialTradeTypeList, "Value", "Text");
        }

        public List<SelectListItem> GetRemainingSubjectListBasedOnIdDLL(string SubjectID, bool? IsEnable)
        {
            var dd = false;
            if (SubjectID != null && SubjectID != "")
            {
                var d = SubjectID.Split(',');
                for (int i = 0; i < d.Length; i++)
                {
                    if (!string.IsNullOrEmpty(d[i]))
                    {
                        var subjectId = d[i];
                        tbl_exam_subject_mast tbl_exam_subject_mast = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_id.ToString() == subjectId).FirstOrDefault();
                        if (tbl_exam_subject_mast != null)
                        {
                            tbl_exam_subject_mast.exam_subject_is_active = IsEnable;
                            if (tbl_exam_subject_mast.subject_type_id == 2)
                            {
                                dd = false;
                            }
                            else
                            {
                                dd = true;
                            }
                        }
                        _db.SaveChanges();
                    }
                }
            }
            else
            {
                List<tbl_exam_subject_mast> tbl_exam_subject_mast = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_is_active == false).ToList();
                for(int i = 0; i < tbl_exam_subject_mast.Count; i++)
                {
                    var subjectId = tbl_exam_subject_mast[i].exam_subject_id;
                    tbl_exam_subject_mast tbl_exam_subject_mastl = _db.tbl_exam_subject_mast.Where(x => x.exam_subject_id == subjectId).FirstOrDefault();
                    if (tbl_exam_subject_mastl != null)
                    {
                        tbl_exam_subject_mastl.exam_subject_is_active = true;
                    }
                }
                _db.SaveChanges();
            }
            if (dd == false)
            {
                return GetSubjectListBasedOnIdDLL(100, 3, 36);
            }
            else
            {
                return GetSubjectListBasedOnIdDLL(100, 3, 18);

            }
        }

        public List<tbl_exam_notification_trans> GetNotificationTranslist(Notification notification)
        {
            var _mat = (from n in _db.tbl_exam_notification_trans
                        join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                        join m in _db.tbl_department_master on d.department_id equals m.dept_id
                        join nd in _db.tbl_notification_description on d.notif_type_id equals nd.notif_decr_id
                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.exam_notif_id == notification.Exam_Notif_Id
                        select n).ToList();
            return _mat;
        }


        public SelectList GetLoginRoleListForwardDLL(int? login)
        {
            List<SelectListItem> UserLoginList = _db.tbl_user_login.Where(n => n.id <= 12 && n.id > login).Select(x => new SelectListItem
            {
                Text = x.user_name.ToString(),
                Value = x.id.ToString()
            }).ToList();

            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select",

            };
            UserLoginList.Insert(0, ProposalList);
            return new SelectList(UserLoginList, "Value", "Text");
        }

        public SelectList GetLoginRoleListSendBackDLL(int? login)
        {
            List<SelectListItem> UserLoginList = _db.tbl_user_login.Where(n => n.id < login && n.id != login).Select(x => new SelectListItem
            {
                Text = x.user_name.ToString(),
                Value = x.id.ToString()
            }).ToList();
            UserLoginList.Reverse();
            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select",

            };
            UserLoginList.Insert(0, ProposalList);
            return new SelectList(UserLoginList, "Value", "Text");
        }




        //DSC Key Mapping BNM 01May2021
        public string LinkDSCDetailsdll(DSCModel model)
        {
            try
            {
                tbl_office_emp_mapping tbl_Office_Emp_Mapping = _db.tbl_office_emp_mapping.Where(x => x.office_id == model.userID && x.oem_is_active == true).FirstOrDefault();

                //if (tbl_Office_Emp_Mapping != null)
                //{

                //tbl_dsc_mapping_dtls IsDSCUser = _db.tbl_dsc_mapping_dtls.Where(x => x.dmd_emp_id == tbl_Office_Emp_Mapping.emp_id && x.dmd_is_active == true).FirstOrDefault();
                tbl_dsc_mapping_dtls IsDSCUser = _db.tbl_dsc_mapping_dtls.Where(x => x.dmd_emp_id == 1 && x.dmd_is_active == true).FirstOrDefault();
                if (IsDSCUser != null)
                {
                    IsDSCUser.dmd_emp_id = model.userID;
                    IsDSCUser.dmd_date_of_expiry = Convert.ToDateTime(model.ExpiryDate);
                    IsDSCUser.dmd_public_key = model.PublicKey;
                    IsDSCUser.dmd_serial_number = model.SerialNo;
                    IsDSCUser.dmd_certifying_authority = model.certifyingAuthority;
                    IsDSCUser.dmd_name = model.DSCName;
                    IsDSCUser.dmd_place = model.Place;
                    IsDSCUser.dmd_is_approved = null;
                    IsDSCUser.dmd_email = model.issuerEmail;
                    IsDSCUser.dmd_phone_no = model.PhoneNumber;
                    IsDSCUser.dmd_is_active = true;
                    IsDSCUser.dmd_updated_on = DateTime.Now;

                    _db.Entry(IsDSCUser).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    tbl_dsc_mapping_dtls dscKey = new tbl_dsc_mapping_dtls();
                    dscKey.dmd_emp_id = model.userID;
                    dscKey.dmd_date_of_expiry = Convert.ToDateTime(model.ExpiryDate);
                    dscKey.dmd_public_key = model.PublicKey;
                    dscKey.dmd_serial_number = model.SerialNo;
                    dscKey.dmd_certifying_authority = model.certifyingAuthority;
                    //dscKey.dmd_emp_id = tbl_Office_Emp_Mapping.emp_id;
                    dscKey.dmd_name = model.DSCName;
                    dscKey.dmd_place = model.Place;
                    dscKey.dmd_is_active = true;
                    dscKey.dmd_is_approved = null;
                    dscKey.dmd_email = model.issuerEmail;
                    dscKey.dmd_phone_no = model.PhoneNumber;
                    dscKey.dmd_created_on = DateTime.Now;
                    _db.tbl_dsc_mapping_dtls.Add(dscKey);
                }
                _db.SaveChanges();
                return "true";
                
            }
            catch (Exception ex)
            {
                return "false";
            }
        }

        public DSCModelList GetOfficerDSCMappingsdll(int userID)
        {
            DSCModelList dSCModelList = new DSCModelList();

            //string userDesignation = _iWDDApprovalFlowDLL.GetUserDesignationdll(userID);
            string userDesignation = "Director";
            //OfficeLocation OfficeLocation = _iWDDApprovalFlowDLL.GetUserOfficeLocationdll(userID);
            //if (OfficeLocation.designationID == 7)
            // {
            //check for any dsc approval by admin for Director,JD, jda ,wdd
            dSCModelList.dSCModels = ( //from dsc in _db.tbl_wsd_dsc_mapping_dtls
                                       //                          join empd in _db.tbl_employee_master on dsc.dmd_emp_id equals empd.emp_id
                                       //                          join emp in _db.tbl_office_emp_mapping on dsc.dmd_emp_id equals emp.emp_id
                                       //                          join office in _db.tbl_office_master on emp.office_id equals office.office_id
                                       //                          join desc in _db.tbl_designation_master on office.designation_id equals desc.designation_id
                                       //                          where dsc.dmd_is_approved == null && office.designation_id == 5 || office.designation_id == 6

                                           from dsc in _db.tbl_dsc_mapping_dtls
                                           join user in _db.tbl_user_login on dsc.dmd_emp_id equals user.id
                                           join desc in _db.tbl_user_roles on user.role_id equals desc.user_id
                                           where dsc.dmd_is_approved == null //&& desc.user_id == 8 || user.role_id == 6 commented by BM 
                                           select new DSCModel
                                           {
                                               DSCName = dsc.dmd_name,
                                               Place = dsc.dmd_name,
                                               SerialNo = dsc.dmd_place,
                                               issuerEmail = dsc.dmd_email,
                                               ExpiryDate = dsc.dmd_date_of_expiry.ToString(),
                                               ValidFromDate = dsc.dmd_created_on.ToString(),
                                               certifyingAuthority = dsc.dmd_certifying_authority,
                                               userID = dsc.dmd_emp_id,
                                               userName = user.user_name,
                                               DSCKeyId = dsc.dmd_regn_no,

                                               designation = desc.user_role,
                                               //district = dsc.dmd_name,
                                               //taluka = dsc.dmd_name,
                                               //hobli = dsc.dmd_name,
                                           }).ToList();
            // }
            //else if (OfficeLocation.designationID == 5)
            //{
            //    //get list of officers under this JDA
            //    int[] usersSubordinates = _iWDDApprovalFlowDLL.GetUsersSubordinateOfficerEmpIDdll(userID);//this will return emp ID's

            //    //check for any dsc approval req
            //    dSCModelList.dSCModels = (from dsc in _db.tbl_wsd_dsc_mapping_dtls
            //                              join empd in _db.tbl_employee_master on dsc.dmd_emp_id equals empd.emp_id
            //                              join emp in _db.tbl_office_emp_mapping on dsc.dmd_emp_id equals emp.emp_id
            //                              join office in _db.tbl_office_master on emp.office_id equals office.office_id
            //                              join desc in _db.tbl_designation_master on office.designation_id equals desc.designation_id
            //                              where usersSubordinates.Contains(dsc.dmd_emp_id) && dsc.dmd_is_approved == null && office.designation_id != 5 && office.designation_id != 6
            //                              select new DSCModel
            //                              {
            //                                  DSCName = dsc.dmd_name,
            //                                  Place = dsc.dmd_name,
            //                                  SerialNo = dsc.dmd_place,
            //                                  issuerEmail = dsc.dmd_email,
            //                                  ExpiryDate = dsc.dmd_date_of_expiry.ToString(),
            //                                  //ValidFromDate = dsc.dmd_name,
            //                                  certifyingAuthority = dsc.dmd_certifying_authority,
            //                                  userID = dsc.dmd_emp_id,
            //                                  userName = empd.name_of_employee,
            //                                  DSCKeyId = dsc.dmd_regn_no,

            //                                  designation = desc.designation_description,
            //                                  //district = dsc.dmd_name,
            //                                  //taluka = dsc.dmd_name,
            //                                  //hobli = dsc.dmd_name,
            //                              }).ToList();

            //    //_db.tbl_wsd_dsc_mapping_dtls.Where(x => dmd_user_id)
            //}
            return dSCModelList;
        }

        public bool UpdateDSCStatusdll(DSCModelList list)
        {
            try
            {
                foreach (DSCModel dSCModel in list.dSCModels.Where(x => x.ischecked == true))
                {
                    tbl_dsc_mapping_dtls tbl = _db.tbl_dsc_mapping_dtls.Where(x => x.dmd_regn_no == dSCModel.DSCKeyId).FirstOrDefault();

                    tbl.dmd_is_approved = dSCModel.status == "Approve" ? true : false;
                    tbl.dmd_remarks = dSCModel.remarks;

                    _db.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                }

                int abc = _db.SaveChanges();
                if (abc >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public bool ValidateDSCKeyWithUserdll(int officeID, string PublicKey)
        {
            int count = (//from ofm in _db.tbl_office_master
            //             join um in _db.tbl_office_emp_mapping on ofm.office_id equals um.office_id
            //             join dsc in _db.tbl_wsd_dsc_mapping_dtls on um.emp_id equals dsc.dmd_emp_id
            //             where um.oem_is_active == true && dsc.dmd_is_active == true && ofm.office_id == officeID && dsc.dmd_public_key == PublicKey && dsc.dmd_is_approved == true
            //             select dsc.dmd_is_approved
            from ul in _db.tbl_user_login
            join dsc in _db.tbl_dsc_mapping_dtls on ul.id equals dsc.dmd_emp_id
            where dsc.dmd_is_active == true && dsc.dmd_is_approved == true && dsc.dmd_public_key == PublicKey
            select dsc.dmd_is_approved
             ).Count();

            return count == 0 ? false : true;
        }

        public string GetLastFeeDateBLLDll(int ExamNotifyId)
        {
            tbl_exam_notification_mast tbl_exam_notification_mast = _db.tbl_exam_notification_mast.Where(x => x.exam_notif_id == ExamNotifyId).FirstOrDefault();
            if(tbl_exam_notification_mast != null)
            {
                return tbl_exam_notification_mast.fee_pay_last_date.ToString();

            }
            return "Error";
        }
        
        public string GrievanceSaveRemarkAndForwardToUserDLL(TabulateGrievance model)
        {
            tbl_grievance_approval tbl_grievance_approval = _db.tbl_grievance_approval.Where(x => x.ga_exam_year == DateTime.Now.Year).FirstOrDefault();
            if (tbl_grievance_approval == null)
            {
                tbl_grievance_approval = new tbl_grievance_approval();
                tbl_grievance_approval.ga_Cource_type = 100;
                tbl_grievance_approval.ga_exam_year = DateTime.Now.Year;
                tbl_grievance_approval.ga_login_id = model.RoleId;
                tbl_grievance_approval.ga_status = 113;
                tbl_grievance_approval.ga_is_active = true;
                tbl_grievance_approval.ga_created_by = model.user_id;
                tbl_grievance_approval.ga_creation_datetime = DateTime.Now;
                _db.tbl_grievance_approval.Add(tbl_grievance_approval);
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = tbl_grievance_approval.ga_id;
                exam_comments_Trans.module_id = 5;
                exam_comments_Trans.status_id = 113;
                exam_comments_Trans.is_published = 0;
                exam_comments_Trans.ct_is_active = true;
                exam_comments_Trans.login_id = model.RoleId;
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();
            }
            else
            {
                tbl_grievance_approval.ga_Cource_type = 100;
                tbl_grievance_approval.ga_exam_year = DateTime.Now.Year;
                tbl_grievance_approval.ga_login_id = model.RoleId;
                tbl_grievance_approval.ga_status = 113;
                tbl_grievance_approval.ga_is_active = true;
                tbl_grievance_approval.ga_updated_by = model.user_id;
                tbl_grievance_approval.ga_updation_datetime = DateTime.Now;
                _db.SaveChanges();

                tbl_comments_transaction exam_comments_Trans = new tbl_comments_transaction();
                exam_comments_Trans.comments_transaction_desc = model.comments;
                exam_comments_Trans.notification_id = tbl_grievance_approval.ga_id;
                exam_comments_Trans.module_id = 5;
                exam_comments_Trans.status_id = 113;
                exam_comments_Trans.is_published = 0;
                exam_comments_Trans.ct_is_active = true;
                exam_comments_Trans.login_id = model.RoleId;
                exam_comments_Trans.ct_created_by = model.user_id;
                exam_comments_Trans.ct_created_on = DateTime.Now;
                _db.tbl_comments_transaction.Add(exam_comments_Trans);
                _db.SaveChanges();
            }

            return "Success";
        }
        public SelectList ExamNotificationsListDLL()
        {
            // List<SelectListItem> res = new List<SelectListItem>();


            List<SelectListItem> NotifList = (from enm in _db.tbl_exam_notification_mast
                                              join dept in _db.tbl_department_master on enm.department_id equals dept.dept_id
                                              where dept.dept_id == 1 && enm.status_id == 105
                                              select new SelectListItem
                                              {
                                                  Text = enm.exam_notif_number.ToString(),
                                                  Value = enm.exam_notif_id.ToString()
                                              }).ToList();


            var NotificationList = new SelectListItem()
            {

                Value = null,
                Text = "Select Exam Notification",

            };
            NotifList.Insert(0, NotificationList);
            return new SelectList(NotifList, "Value", "Text");
        }

        public List<Notification> GetPublishedFilePathDLL(Notification notification)
        {
            var Notifs = (from n in _db.tbl_exam_notification_mast
                          where (n. exam_notif_id== notification.Exam_Notif_Id)
                          select new Models.ExamNotification.Notification
                          {

                              SavePath = n.exam_notif_file_path,

                          }).ToList();



            return Notifs;
        }

        public List<Notification> ViewNotificationFileDLL(int id, int? notificationId)
        {
            List<Notification> Notifs = null;
            
                Notifs = (from n in _db.tbl_exam_notification_trans
                          join d in _db.tbl_exam_notification_mast on n.exam_notif_id equals d.exam_notif_id
                          join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                          join c in _db.tbl_course_type_mast on d.course_id equals c.course_id
                          join m in _db.tbl_department_master on d.department_id equals m.dept_id into _m
                          join END in _db.tbl_notification_description on d.notif_type_id equals END.notif_decr_id
                          //join role in _db.tbl_user_roles on d.login_id equals role.user_id
                          //join comnt in _db.tbl_comments_transaction on d.exam_notif_id equals comnt.notification_id into _comnt
                          //from CT in _comnt.DefaultIfEmpty()
                          where n.exam_notif_id == notificationId && n.exam_notif_status_id!= 111 
                          orderby n.creation_datetime descending
                          //orderby n.login_id descending
                          
                          
                          select new Models.ExamNotification.Notification
                          {
                              Exam_Notif_Id = n.exam_notif_id,
                              Exam_Notif_Number = d.exam_notif_number,
                              NotifDescId = d.notif_type_id,
                              Exam_Notif_Desc = END.notification_description,
                              Exam_notif_date = (DateTime)d.exam_notif_date,
                              exam_notif_type = d.exam_notif_type,
                              fee_pay_last_date = (DateTime)d.fee_pay_last_date,
                              appli_from_last_date = (DateTime)d.appli_from_last_date,
                              trainee_name_eval_last_date = (DateTime)d.trainee_name_eval_last_date,
                              princ_sub_last_date = (DateTime)d.princ_sub_last_date,
                              jd_sub_last_date = (DateTime)d.jd_sub_last_date,
                              appli_charges_fee = d.appli_charges_fee,
                              exam_regular_fee = d.exam_regular_fee,
                              exam_repeater_fee = d.exam_repeater_fee,
                             login_id = n.login_id,
                              exam_notif_status_desc = p.exam_notif_status_desc,
                              exam_notif_status_id = n.exam_notif_status_id,
                              CourseTypeId = d.course_id,
                              CourseTypeName = c.course_type_name,
                              DeptId = d.department_id,
                              DeptName = END.notification_description,
                              //comments = CT.comments_transaction_desc
                              Description = d.notif_description,
                              creation_datetime = n.creation_datetime,
                              SavePath = d.exam_notif_file_path,
                              DocSavePath = n.exam_notif_doc_file_path

                          }).ToList();


            foreach(var Id in Notifs)
            {
                var ById =Id.login_id;
                var UserName = (from j in _db.tbl_user_login
                                where j.id == ById
                               
                                select j.user_name ).FirstOrDefault();
                Id.user_name = UserName;
            }
            
            


            return Notifs;
        }

        public string UpdateNotificationDocFileDetailsDLL(List<Notification> models)
        {
           
            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var model in models)
                {
                    try
                    {
                        
                        bool isActive = false;
                        var NotificationId = _db.tbl_exam_notification_mast.Where(x => x.exam_notif_number == model.Exam_Notif_Number).Select(x => new SelectListItem { Text = x.exam_notif_number, Value = x.exam_notif_id.ToString()}).ToList();
                        string notificationId = "0";
                        if (NotificationId != null && NotificationId.Count > 0)
                        {
                            notificationId = NotificationId[0].Value;
                        }

                        int NotificatioId = Convert.ToInt32(notificationId);
                        tbl_exam_notification_trans exam_Notification_Trans = _db.tbl_exam_notification_trans.Where(x => x.exam_notif_id == NotificatioId).FirstOrDefault();
                        
                        //if (exam_Notification_Trans == null)
                        //{
                        //    exam_Notification_Trans = new tbl_exam_notification_trans();
                        //    exam_Notification_Trans.exam_notif_id = NotificatioId;
                        //    exam_Notification_Trans.exam_notif_status_id = notification.exam_notif_status_id;
                        //    exam_Notification_Trans.trans_date = DateTime.Now;
                        //    exam_Notification_Trans.is_active = isActive;
                        //    exam_Notification_Trans.creation_datetime = DateTime.Now;
                        //    exam_Notification_Trans.updation_datetime = DateTime.Now;
                        //    exam_Notification_Trans.created_by = notification.login_id;
                        //    exam_Notification_Trans.updated_by = 2;
                        //    exam_Notification_Trans.login_id = notification.RoleId;
                        //    _db.tbl_exam_notification_trans.Add(exam_Notification_Trans);
                        //    _db.SaveChanges();

                        //}
                        //else
                        //{
                        //    exam_Notification_Trans.exam_notif_id = notification.Exam_Notif_Id;
                        //    exam_Notification_Trans.exam_notif_status_id = notification.exam_notif_status_id;
                        //    exam_Notification_Trans.trans_date = DateTime.Now;
                        //    exam_Notification_Trans.is_active = isActive;
                        //    exam_Notification_Trans.creation_datetime = DateTime.Now;
                        //    exam_Notification_Trans.updation_datetime = DateTime.Now;
                        //    exam_Notification_Trans.created_by = notification.login_id;
                        //    exam_Notification_Trans.updated_by = 2;
                        //    //    exam_Notification_Trans.login_id = notification.RoleId;

                        //    _db.SaveChanges();
                        //}
                        //UpdateUploadAttendanceDocuments(model);
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                     //   returnValue = "Failed";
                    }
                }

            }
           
            return "Saved";
        }

        public int GetNotificationID(Notification notification)
        {
            var NotificationId = _db.tbl_exam_notification_mast.Where(x => x.exam_notif_number == notification.Exam_Notif_Number).Select(x => new SelectListItem { Text = x.exam_notif_number, Value = x.exam_notif_id.ToString() }).ToList();
            string notificationId = "0";
            if (NotificationId != null && NotificationId.Count > 0)
            {
                notificationId = NotificationId[0].Value;
            }

            int NotificatioId = Convert.ToInt32(notificationId);
            return NotificatioId;
        }

        public int? UpdateNotificationDocFile(tbl_exam_notification_trans obj)
        {

            obj.exam_notif_doc_file_path = "";
            obj.updation_datetime = DateTime.Now;
            return _db.SaveChanges();

        }

        private void UploadDocNotification(Notification model)
        {
            string uploadDirectory = HttpContext.Current.Server.MapPath("~/Content/Template");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            // upload scanned copy
            if (model != null && !string.IsNullOrEmpty(model.UploadPdf.FileName) && (model.UploadPdf.ContentLength > 0))
            {
                string docFile = string.Format("{0}/{1}", uploadDirectory, model.UploadPdf.FileName);
                model.UploadPdf.SaveAs(docFile);
            }
            
        }

        public List<Notification> GetRolesDLL(int? Roleid,int? loginID)
        {
            var res = (from aa in _db.tbl_user_roles
                       join l in _db.tbl_user_login on aa.user_id equals l.role_id
                       where aa.user_id == Roleid && l.id == loginID
                       select new Notification
                       {
                           RoleId = aa.user_id,
                           user_role = aa.user_role,
                           shrotName = l.short_name_designation

                       }
                    ).ToList();
            return res;
        }

        public List<tbl_exam_notification_trans> Get_tbl_exam_notification_trans(Notification notification)
        {
            var list = (from n in _db.tbl_exam_notification_trans
                        where n.exam_notif_id == notification.Exam_Notif_Id && n.login_id == notification.login_id
                        select n).ToList();
            return list;
        }

        public List<tbl_exam_notification_mast> Get_tbl_exam_notification_mast(Notification notification)
        {
            var list = (from n in _db.tbl_exam_notification_mast
                        where n.exam_notif_id == notification.Exam_Notif_Id && n.login_id == notification.login_id
                        select n).ToList();
            return list;
        }

        public int updateTransactionNotificationDoc(tbl_exam_notification_trans obj)
        {
            tbl_exam_notification_trans obj1 = (from n in _db.tbl_exam_notification_trans
                                                where n.exam_notif_id == obj.exam_notif_id && n.login_id == obj.login_id
                                           select n).FirstOrDefault();
            if (obj.exam_notif_status_id == 111)
            {
                obj1.login_id = obj.login_id;
                obj1.updated_by = obj.updated_by;
                obj1.exam_notif_doc_file_path = obj.exam_notif_doc_file_path;
                obj1.updation_datetime = DateTime.Now;
            }
            else
            {
                obj1.login_id = obj.login_id;
                obj1.updated_by = obj.updated_by;
                obj1.exam_notif_doc_file_path = obj.exam_notif_doc_file_path;
                obj1.updation_datetime = DateTime.Now;
            }

            
            var res = _db.SaveChanges();
            return res;
        }

        public int updateMasterNotificationPDF(tbl_exam_notification_mast obj)
        {
            tbl_exam_notification_mast obj1 = (from n in _db.tbl_exam_notification_mast
                                               where n.exam_notif_id == obj.exam_notif_id && n.login_id == obj.login_id
                                                select n).FirstOrDefault();
            if (obj.status_id != 110)
            {
                obj1.login_id = obj.login_id;
            }


            obj1.updated_by = obj.updated_by;
            obj1.exam_notif_file_path = obj.exam_notif_file_path;
            obj1.updation_datetime = DateTime.Now;
            var res = _db.SaveChanges();
            return res;
        }

        public List<string> ExamCenterMailIdDLL()
        {
            List<string> list1 = (from n in _db.tbl_iti_college_details
                                  where n.iti_college_code == "PR29000057" //|| n.iti_college_code =="GU29000214"
                                  select n.email_id).ToList();
            return list1;
        }

        public SelectList GetApplicantTypeListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_ApplicantType.Select(x => new SelectListItem
            {
                Text = x.ApplicantType.ToString(),
                Value = x.ApplicantTypeId.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Year",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }


    }

}
