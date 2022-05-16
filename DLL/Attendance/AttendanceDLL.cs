using System;
using System.Collections.Generic;
using DLL.DBConnection;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Web;
using Spire.Xls;
using Models.AttendanceDetails;

namespace DLL.Attendance
{
    public class AttendanceDLL : IAttendanceDLL
    {
        private readonly DbConnection _db = new DbConnection();
        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

       public  SelectList GetDivisionListDLL()
        {
            List<SelectListItem> DivisinList = _db.tbl_division_mast.Select(x => new SelectListItem
            {
                Text = x.division_name.ToString(),
                Value = x.division_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Division",

            };
            DivisinList.Insert(0, ProposalList);
            return new SelectList(DivisinList, "Value", "Text");
        }
        public SelectList GetCenterListDLL()
        {
            List<SelectListItem> CntrList = _db.tbl_division_mast.Select(x => new SelectListItem
            {
                Text = x.division_name.ToString(),
                Value = x.division_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Course Type",

            };
            CntrList.Insert(0, ProposalList);
            return new SelectList(CntrList, "Value", "Text");
        }

        public string ExamAttendanceUploadDLL(AtendanceDet model)
        {
            if (!string.IsNullOrEmpty(model.UploadXcel.FileName) && (model.UploadXcel.ContentLength > 0))
            {
                DataTable EntryDt_tbl_Attendance_Dtls_Mast = new DataTable("EntryDt_tbl_Attendance_Dtls_Mast");
                DataColumn exam_centre_id = new DataColumn("exam_centre_id", typeof(System.Int16));
                DataColumn trade_id = new DataColumn("trade_id", typeof(System.Int16));
                DataColumn trade_type_id = new DataColumn("trade_type_id", typeof(System.Int16));
                DataColumn subject_id = new DataColumn("subject_id", typeof(System.Int16));
                DataColumn attendance = new DataColumn("attendance", typeof(System.String));
                DataColumn additional_sheet_no = new DataColumn("additional_sheet_no", typeof(System.Int16));
                DataColumn answer_sheet_no = new DataColumn("answer_sheet_no", typeof(System.Int16));
                DataColumn exam_duration = new DataColumn("exam_duration", typeof(System.Int16));
                DataColumn trainee_roll_num = new DataColumn("trainee_roll_num", typeof(System.Int16));
                DataColumn block_id = new DataColumn("block_id", typeof(System.Int16));
                DataColumn Procedure_or_drawing_sheet_no = new DataColumn("Procedure_or_drawing_sheet_no", typeof(System.Int16));
                DataColumn status_id = new DataColumn("status_id", typeof(System.Int16));
                DataColumn is_active = new DataColumn("is_active", typeof(System.Boolean));
                DataColumn created_by = new DataColumn("created_by", typeof(System.Int16));
                DataColumn creation_datetime = new DataColumn("creation_datetime", typeof(System.DateTime));
                DataColumn updated_by = new DataColumn("updated_by", typeof(System.Int16));
                DataColumn updation_datetime = new DataColumn("updation_datetime", typeof(System.DateTime));
                DataColumn remarks = new DataColumn("remarks", typeof(System.String));
                DataColumn Notif_Status_id = new DataColumn("exam_notif_status_id", typeof(System.Int16));
                DataColumn Loginid = new DataColumn("login_id", typeof(System.Int16));
                //  DataColumn remarks = new DataColumn(model.BlockNo, typeof(System.Int16));


                EntryDt_tbl_Attendance_Dtls_Mast.Columns.AddRange(new DataColumn[] { exam_centre_id, trade_id, trade_type_id, subject_id, attendance, additional_sheet_no, answer_sheet_no, exam_duration, trainee_roll_num, block_id, Procedure_or_drawing_sheet_no, status_id, is_active, created_by, creation_datetime, updated_by, updation_datetime, remarks, Notif_Status_id, Loginid });



                string path1 = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/Content/Uploads"), model.UploadXcel.FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Content/Uploads"));
                }
                if (File.Exists(path1))
                {
                    File.Delete(path1);
                }
                model.UploadXcel.SaveAs(path1);
                DataTable dt = ConvertXSLXtoDataTable(path1);

                if (dt.Columns.Count > 0)
                {
                    //if (dt.Columns.Count == 18)
                    //{
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
                        //for Exam Center ID
                        string Exam_centre_id = Convert.ToString(dt.Rows[j][0].ToString().ToUpper());
                        var centreDetails = _db.tbl_exam_centers.Where(x => x.ec_name.ToString()== Exam_centre_id).Select(x => new SelectListItem { Text = x.ec_name, Value = x.ec_id.ToString() }).ToList();
                        string centre_id = "0";
                        if (centreDetails != null && centreDetails.Count > 0)
                        {
                            centre_id = centreDetails[0].Value;
                        }

                        //For tradeID
                        
                        string Trade_id = Convert.ToString(dt.Rows[j][1].ToString().ToUpper());
                        var tradeidDtls = _db.tbl_trade_mast.Where(x => x.trade_name == Trade_id).Select(x => new SelectListItem { Text = x.trade_name, Value = x.trade_id.ToString() }).ToList();
                        string tradeid = "0";
                        if (tradeidDtls != null && tradeidDtls.Count > 0)
                        {
                            tradeid = tradeidDtls[0].Value;
                        }
                        ////For Trade Type ID
                        string Trade_type_id = Convert.ToString(dt.Rows[j][2].ToString().ToUpper());
                        var tradTypeiddtls = _db.tbl_trade_type_mast.Where(x => x.trade_type_name == Trade_type_id).Select(x => new SelectListItem { Text = x.trade_type_name, Value = x.trade_type_id.ToString() }).ToList();
                        string tradTypeid = "0";
                        if (tradTypeiddtls != null && tradTypeiddtls.Count > 0)
                        {
                            tradTypeid = tradTypeiddtls[0].Value;
                        }
                        //For subject Id
                        string Subject_id = Convert.ToString(dt.Rows[j][3].ToString().ToUpper());
                        var subidDtls = _db.tbl_subject.Where(x => x.subject_name == Subject_id).Select(x => new SelectListItem { Text = x.subject_name, Value = x.subject_id.ToString() }).ToList();
                        string subid = "0";
                        if (subidDtls != null && subidDtls.Count > 0)
                        {
                            subid = subidDtls[0].Value;
                        }
                        //For ITI Number

                        string Trainee_roll_num = Convert.ToString(dt.Rows[j][8].ToString().ToUpper());

                        var itiDtls = _db.tbl_iti_trainees_mast.Where(x => x.roll_num.ToString() == Trainee_roll_num).Select(x => new SelectListItem { Text = x.iti_code, Value = x.iti_trainees_id.ToString() }).ToList();
                        string ITIid = "0";
                        if (itiDtls != null && subidDtls.Count > 0)
                        {
                            ITIid = itiDtls[0].Value;
                        }

                        if (model.Trainee_Roll_Num != null && model.Trainee_Roll_Num!=0 && model.Trainee_Roll_Num.ToString()!= Trainee_roll_num)
                        {
                            //if it is roll number traniee based update ,ignore the record if it is diffrent record
                            continue;
                        }
                            
                            string Attendance = Convert.ToString(dt.Rows[j][4].ToString().ToUpper());
                            string Additional_sheet_no = Convert.ToString(dt.Rows[j][5].ToString().ToUpper());
                            string Answer_sheet_no = Convert.ToString(dt.Rows[j][6].ToString().ToUpper());
                            string Exam_duration = Convert.ToString(dt.Rows[j][7].ToString().ToUpper());
                            
                            string Block_id = Convert.ToString(dt.Rows[j][9].ToString().ToUpper());
                            string Procedure_or_drawing_sheet_No = Convert.ToString(dt.Rows[j][10].ToString().ToUpper());
                            string Status_id = Convert.ToString(dt.Rows[j][11].ToString().ToUpper());
                            string Is_active = Convert.ToString(dt.Rows[j][12].ToString().ToUpper());
                            string Created_by = Convert.ToString(dt.Rows[j][13].ToString().ToUpper());
                            string Creation_datetime = Convert.ToString(dt.Rows[j][14].ToString().ToUpper());
                            string Updated_by = Convert.ToString(dt.Rows[j][15].ToString().ToUpper());
                            string Updation_datetime = Convert.ToString(dt.Rows[j][16].ToString().ToUpper());
                            string Remarks = Convert.ToString(dt.Rows[j][17].ToString().ToUpper());
                        string Notif_Status_Id = Convert.ToString(dt.Rows[j][18].ToString().ToUpper());
                        string LoginId = Convert.ToString(dt.Rows[j][19].ToString().ToUpper());

                        tbl_attendance_details tbl_attendance_dtls = _db.tbl_attendance_details.Where(x => x.trainee_roll_num.ToString().ToUpper() == Trainee_roll_num).FirstOrDefault();
                            if (tbl_attendance_dtls != null)
                            {
							    
                                tbl_attendance_dtls.exam_centre_id = Convert.ToInt32(centre_id);
                                tbl_attendance_dtls.trade_id = Convert.ToInt32(tradeid);
                                tbl_attendance_dtls.trade_type_id = Convert.ToInt32(tradTypeid);
                                tbl_attendance_dtls.subject_id = Convert.ToInt32(subid);
                                tbl_attendance_dtls.attendance = Convert.ToString(Attendance);
                                tbl_attendance_dtls.additional_sheet_no = Convert.ToInt32(Additional_sheet_no);
                                tbl_attendance_dtls.answer_sheet_no = Convert.ToInt32(Answer_sheet_no);
                                tbl_attendance_dtls.exam_duration = Convert.ToInt32(Exam_duration);
                                tbl_attendance_dtls.trainee_roll_num = Convert.ToInt32(Trainee_roll_num);
                                tbl_attendance_dtls.block_id = Convert.ToInt32(Block_id);
                                tbl_attendance_dtls.Procedure_or_drawing_sheet_no = Convert.ToInt32(Procedure_or_drawing_sheet_No);
                                tbl_attendance_dtls.status_id = Convert.ToInt32(Status_id);
                                tbl_attendance_dtls.is_active = true;
                                
                                tbl_attendance_dtls.updated_by = Convert.ToInt32(Updated_by); 
                                tbl_attendance_dtls.updation_datetime = DateTime.Now;
                            tbl_attendance_dtls.exam_notif_status_id = Convert.ToInt32(Notif_Status_Id);
                            tbl_attendance_dtls.login_id = Convert.ToInt32(LoginId);

                            _db.SaveChanges();
                            }
                            else
                            {
                                tbl_attendance_dtls = new tbl_attendance_details();
                                tbl_attendance_dtls.exam_centre_id = Convert.ToInt32(centre_id);
                                tbl_attendance_dtls.trade_id = Convert.ToInt32(tradeid);
                                tbl_attendance_dtls.trade_type_id = Convert.ToInt32(tradTypeid);
                                tbl_attendance_dtls.subject_id = Convert.ToInt32(subid);
                                tbl_attendance_dtls.attendance = Convert.ToString(Attendance);
                                tbl_attendance_dtls.additional_sheet_no = Convert.ToInt32(Additional_sheet_no);
                                tbl_attendance_dtls.answer_sheet_no = Convert.ToInt32(Answer_sheet_no);
                                tbl_attendance_dtls.exam_duration = Convert.ToInt32(Exam_duration);
                                tbl_attendance_dtls.trainee_roll_num = Convert.ToInt32(Trainee_roll_num);
                                tbl_attendance_dtls.block_id = Convert.ToInt32(Block_id);
                                tbl_attendance_dtls.Procedure_or_drawing_sheet_no = Convert.ToInt32(Procedure_or_drawing_sheet_No);
                                tbl_attendance_dtls.status_id = Convert.ToInt32(Status_id);
                                tbl_attendance_dtls.is_active = true;
                                tbl_attendance_dtls.created_by = Convert.ToInt32(Created_by); 
                                tbl_attendance_dtls.creation_datetime = DateTime.Now;
                            tbl_attendance_dtls.exam_notif_status_id = Convert.ToInt32(Notif_Status_Id);
                            tbl_attendance_dtls.login_id = Convert.ToInt32(LoginId);



                            _db.tbl_attendance_details.Add(tbl_attendance_dtls);
                                _db.SaveChanges();
                            }


                        tbl_attendance_details_trans tbl_attent_dtls_trans = _db.tbl_attendance_details_trans.Where(x => x.iti_trainees_id.ToString().ToUpper() == Trainee_roll_num).FirstOrDefault();
                        if (tbl_attent_dtls_trans != null)
                        {

                            tbl_attent_dtls_trans.exam_centre_id = Convert.ToInt32(centre_id);
                            tbl_attent_dtls_trans.trade_id = Convert.ToInt32(tradeid);
                            tbl_attent_dtls_trans.trade_type_id = Convert.ToInt32(tradTypeid);
                            tbl_attent_dtls_trans.subject_id = Convert.ToInt32(subid);
                            tbl_attent_dtls_trans.attendance = Convert.ToString(Attendance);
                            tbl_attent_dtls_trans.additional_sheet_no = Convert.ToInt32(Additional_sheet_no);
                            tbl_attent_dtls_trans.answer_sheet_no = Convert.ToInt32(Answer_sheet_no);
                            tbl_attent_dtls_trans.exam_duration = Convert.ToInt32(Exam_duration);
                            tbl_attent_dtls_trans.iti_trainees_id = Convert.ToInt32(ITIid);
                            tbl_attent_dtls_trans.block_id = Convert.ToInt32(Block_id);
                            tbl_attent_dtls_trans.procedure_or_drawing_sheet_no = Convert.ToInt32(Procedure_or_drawing_sheet_No);
                            tbl_attent_dtls_trans.status_id = Convert.ToInt32(Status_id);
                            tbl_attent_dtls_trans.is_active = true;

                            tbl_attent_dtls_trans.updated_by = Convert.ToInt32(Updated_by);
                            tbl_attent_dtls_trans.updation_datetime = DateTime.Now;
                            tbl_attent_dtls_trans.exam_notif_status_id = Convert.ToInt32(Notif_Status_Id);
                            tbl_attent_dtls_trans.login_id = Convert.ToInt32(LoginId);
                            tbl_attent_dtls_trans.attendance_id = tbl_attendance_dtls.attendance_id;

                            _db.SaveChanges();
                        }
                        else
                        {
                            tbl_attent_dtls_trans = new tbl_attendance_details_trans();
                            tbl_attent_dtls_trans.exam_centre_id = Convert.ToInt32(centre_id);
                            tbl_attent_dtls_trans.trade_id = Convert.ToInt32(tradeid);
                            tbl_attent_dtls_trans.trade_type_id = Convert.ToInt32(tradTypeid);
                            tbl_attent_dtls_trans.subject_id = Convert.ToInt32(subid);
                            tbl_attent_dtls_trans.attendance = Convert.ToString(Attendance);
                            tbl_attent_dtls_trans.additional_sheet_no = Convert.ToInt32(Additional_sheet_no);
                            tbl_attent_dtls_trans.answer_sheet_no = Convert.ToInt32(Answer_sheet_no);
                            tbl_attent_dtls_trans.exam_duration = Convert.ToInt32(Exam_duration);
                            tbl_attent_dtls_trans.iti_trainees_id = Convert.ToInt32(ITIid);
                            tbl_attent_dtls_trans.block_id = Convert.ToInt32(Block_id);
                            tbl_attent_dtls_trans.procedure_or_drawing_sheet_no = Convert.ToInt32(Procedure_or_drawing_sheet_No);
                            tbl_attent_dtls_trans.status_id = Convert.ToInt32(Status_id);
                            tbl_attent_dtls_trans.is_active = true;
                            tbl_attent_dtls_trans.created_by = Convert.ToInt32(Created_by);
                            tbl_attent_dtls_trans.creation_datetime = DateTime.Now;
                            tbl_attent_dtls_trans.exam_notif_status_id = Convert.ToInt32(Notif_Status_Id);
                            tbl_attent_dtls_trans.login_id = Convert.ToInt32(LoginId);
                            tbl_attent_dtls_trans.attendance_id = tbl_attendance_dtls.attendance_id;
                            _db.tbl_attendance_details_trans.Add(tbl_attent_dtls_trans);
                            _db.SaveChanges();
                        }







                    }
                    //}
                    //else
                    //    return "Error";
                }
                else
                    return "Error";
            }
            return "Saved";
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


        public List<AtendanceDet> getAttendanceDetDLL(int divId)
        {

            var det = (from n in _db.tbl_exam_subject_trans
                       join d in _db.tbl_exam_subject_type_mast on n.est_exam_subject_type_id equals d.exam_subject_type_id
                       join ty in _db.tbl_trade_year_mast on n.est_trade_year_id equals ty.trade_year_id

                       where n.est_id == 151
                       select new AtendanceDet
                       {
                           Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                          ECT_ExamDate = n.est_exam_date,
                          ECT_Exam_Start_Time=n.est_exam_start_date,
                          ECT_Exam_End_Time=n.exam_end_date,
                          TradeYr = ty.trade_year_name,
                          Subject=d.exam_subject_type_desc,
                          BlockNo = 1.ToString(),
                         
                          
                           
                           
                          
                       }).ToList();
            return det;
        }

        public SelectList GetLoginRoleListDLL()
        {
            List<SelectListItem> UserLoginList = _db.tbl_user_login
                .Where(x => x.id == 2 || x.id == 9)
                .Select(x => new SelectListItem
            {
                Text = x.user_name.ToString(),
                Value = x.id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select next level",

            };
            UserLoginList.Insert(0, ProposalList);
            return new SelectList(UserLoginList, "Value", "Text");
        }

        public string CreateAttendanceDetailsDLL(List<AtendanceDet> models)
        {
            string returnValue = "saved";
            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var model in models)
                {
                    try
                    {
                        UploadAttendanceDocuments(model);
                        bool isActive = false;
                        int blockNo = Convert.ToInt32(model.BlockNo);
                        tbl_block_or_room block_Or_Room = _db.tbl_block_or_room.Where(x => x.block_id == blockNo).FirstOrDefault();
                        if (block_Or_Room == null)
                        {
                            block_Or_Room = new tbl_block_or_room();
                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;
                            _db.tbl_block_or_room.Add(block_Or_Room);
                            _db.SaveChanges();

                        }
                        else
                        {

                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;

                            _db.SaveChanges();
                        }
                        


                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        returnValue = "Failed";
                    }
                }

            }
            return returnValue;
        }

        public string UpdateAttendanceDetailsDLL(List<AtendanceDet> models)
        {
            string returnValue = "saved";
            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var model in models)
                {
                    try
                    {
                        UploadAttendanceDocuments(model);
                        bool isActive = false;
                        int blockNo = Convert.ToInt32(model.BlockNo);
                        tbl_block_or_room block_Or_Room = _db.tbl_block_or_room.Where(x => x.block_id == blockNo && x.upload_excel_file == model.UploadXcel.FileName).FirstOrDefault();
                        if (block_Or_Room == null)
                        {
                            block_Or_Room = new tbl_block_or_room();
                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;
                            _db.tbl_block_or_room.Add(block_Or_Room);
                            _db.SaveChanges();

                        }
                        else
                        {

                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;

                            _db.SaveChanges();
                        }
                        
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        returnValue = "Failed";
                    }
                }

            }
            return returnValue;
        }

        private void UploadAttendanceDocuments(AtendanceDet model)
        {
            string uploadDirectory = HttpContext.Current.Server.MapPath("~/Content/Uploads");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            // upload scanned copy
            if (model != null && !string.IsNullOrEmpty(model.UploadPdf.FileName) && (model.UploadPdf.ContentLength > 0))
            {
                string pdfFile = string.Format("{0}/{1}", uploadDirectory, model.UploadPdf.FileName);
                model.UploadPdf.SaveAs(pdfFile);
            }
            if (model != null && !string.IsNullOrEmpty(model.UploadXcel.FileName) && (model.UploadXcel.ContentLength > 0))
            {   // upload the excel file
                string excelFile = string.Format("{0}/{1}", uploadDirectory, model.UploadXcel.FileName);               
                model.UploadXcel.SaveAs(excelFile);
                ExamAttendanceUploadDLL(model);
            }
        }

        public List<AtendanceDet> getModifyAttendanceDetDLL(int divId)
        {
            var det = (from n in _db.tbl_attendance_details
                       join b in _db.tbl_block_or_room on n.block_id equals b.block_id


                       where n.block_id == 6 && n.block_id == 6
                       select new AtendanceDet
                       {
                           Day = "Monday",
                                         
                                          
                           Subject = n.subject_id.ToString(),
                           BlockNo = b.room_number,
                           attendSavePath = b.upload_pdf_file,
                           attendExcelPath = b.upload_excel_file,
                           ECT_ExamDate = DateTime.Now,
                           ECT_Exam_Start_Time = DateTime.Now,
                           ECT_Exam_End_Time = DateTime.Now,

                       }).ToList();
            return det;
        }
		
		public List<OMRSheetDetails> GetOMREntryDLL(int divId)
        {

            var det = (from n in _db.tbl_exam_subject_trans
                       join d in _db.tbl_exam_subject_type_mast on n.est_exam_subject_type_id equals d.exam_subject_type_id
                       join ty in _db.tbl_trade_year_mast on n.est_trade_year_id equals ty.trade_year_id

                       where n.est_id == 74 || n.est_id == 75
                       select new OMRSheetDetails
                       {
                           Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                           EExam_Date = n.est_exam_date.ToString(),
                           Exam_Start_date = n.est_exam_start_date.ToString(),
                           Exam_End_date = n.exam_end_date.ToString(),
                           TradeYr = ty.trade_year_name,
                           Subject = d.exam_subject_type_desc,
                           RoomNo = 1,
                       }).ToList();
            return det;
        }

        public List<AtendanceDet> getTraineeDtlsByRollNoDLL(int rollNo)
        {
            var det = (from n in _db.tbl_attendance_details
                       join b in _db.tbl_block_or_room on n.block_id equals b.block_id


                       where n.trainee_roll_num==rollNo
                       select new AtendanceDet
                       {
                           Day = "Monday",
                           Subject = n.subject_id.ToString(),
                           BlockNo = b.room_number,
                           attendSavePath = b.upload_pdf_file,
                           attendExcelPath = b.upload_excel_file,
                           ECT_ExamDate = DateTime.Now,
                           ECT_Exam_Start_Time = DateTime.Now,
                           ECT_Exam_End_Time = DateTime.Now,

                       }).ToList();
            return det;
        }

        public List<AtendanceDet> getAttendanceDtlsDLL(AtendanceDet attendanceDet)//AtendanceDet attendanceDet
            {
            var dtls = (from n in _db.tbl_attendance_details_trans
                        join a in _db.tbl_attendance_details on n.attendance_id equals a.attendance_id
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join b in _db.tbl_block_or_room on n.block_id equals b.block_id

                        where n.login_id == attendanceDet.login_id //&& n.exam_notif_status_id == 100
                        select new AtendanceDet
                        {
                            Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                            Trainee_Roll_Num = a.trainee_roll_num,
                            attenId = n.attendance_id,
                           
                            
                            BlockNo = n.block_id.ToString(),
                           
                            login_id = n.login_id,
                            StatusId = n.status_id,
                            status_desc = p.exam_notif_status_desc,
                            status_id = n.exam_notif_status_id,
                            attendSavePath = b.upload_pdf_file,
                            attendExcelPath = b.upload_excel_file
                        }).ToList();
            return dtls;
        }

        public List<AtendanceDet> AttendanceDtlsDLL(AtendanceDet attendanceDet)//AtendanceDet attendanceDet
        {
            var dtls = (from n in _db.tbl_attendance_details
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join b in _db.tbl_block_or_room on n.block_id equals b.block_id

                        where  n.attendance_id == attendanceDet.attenId
                        select new AtendanceDet
                        {
                            Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                            Trainee_Roll_Num = n.trainee_roll_num,
                            attenId = n.attendance_id,


                            BlockNo = n.block_id.ToString(),

                            login_id = n.login_id,
                            StatusId = n.status_id,
                            status_desc = p.exam_notif_status_desc,
                            status_id = n.exam_notif_status_id,
                            attendSavePath = b.upload_pdf_file,
                            attendExcelPath = b.upload_excel_file
                        }).ToList();
            return dtls;
        }

        public tbl_attendance_details_trans GetAttendByID(AtendanceDet model)
        {
            var _mat = (from n in _db.tbl_attendance_details_trans

                            //join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id

                        join u in _db.tbl_user_login on n.login_id equals u.id
                        where n.attendance_id == model.attenId && n.login_id == model.login_id
                        select n).FirstOrDefault();
            return _mat;
        }

        public int? UpdateEmp(tbl_attendance_details dtls)
        {
            
            return _db.SaveChanges();

        }
        public int? UpdateStatus(tbl_attendance_details_trans emp)
        {
            return _db.SaveChanges();

        }

        public List<AtendanceDet> GetAttendanceDtlsByLoginIdDLL(AtendanceDet modal)
        {
            var notify = (from n in _db.tbl_attendance_details_trans
                         
                          join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                          join b in _db.tbl_block_or_room on n.block_id equals b.block_id
                          join a in _db.tbl_attendance_details on n.attendance_id equals a.attendance_id
                          where n.login_id == modal.RoleId
                          select new AtendanceDet
                          {
                              Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                             Trainee_Roll_Num = a.trainee_roll_num,
                              attenId = n.attendance_id,


                              BlockNo = n.block_id.ToString(),

                              login_id = n.login_id,
                              StatusId = n.status_id,
                              status_desc = p.exam_notif_status_desc,
                              status_id = n.exam_notif_status_id,
                              attendSavePath = b.upload_pdf_file,
                              attendExcelPath = b.upload_excel_file
                          }
                ).OrderByDescending(x => x.attenId).ToList();
            return notify;
        }

        public string CreateTransAttendenceDLL(tbl_attendance_details_trans model, AtendanceDet role)
        {
            try
            {
                bool isActive = false;
                tbl_attendance_details_trans exam_Attendance_Trans = _db.tbl_attendance_details_trans.Where(x => x.adt_id == model.attendance_id && x.login_id == role.RoleId).FirstOrDefault();
                if (exam_Attendance_Trans == null)
                {
                    exam_Attendance_Trans = new tbl_attendance_details_trans();
                    exam_Attendance_Trans.exam_centre_id = model.exam_centre_id;
                    exam_Attendance_Trans.trade_id = model.trade_id;
                    exam_Attendance_Trans.trade_type_id = model.trade_type_id;
                    exam_Attendance_Trans.subject_id = model.subject_id;
                    exam_Attendance_Trans.attendance = model.attendance;
                    exam_Attendance_Trans.additional_sheet_no = model.additional_sheet_no;
                    exam_Attendance_Trans.answer_sheet_no = model.answer_sheet_no;
                    exam_Attendance_Trans.exam_duration = model.exam_duration;
                    exam_Attendance_Trans.iti_trainees_id = model.iti_trainees_id;
                    exam_Attendance_Trans.block_id = model.block_id;
                    exam_Attendance_Trans.procedure_or_drawing_sheet_no = model.procedure_or_drawing_sheet_no;
                    exam_Attendance_Trans.status_id = model.status_id;
                    exam_Attendance_Trans.exam_notif_status_id = 104;
                    
                    exam_Attendance_Trans.is_active = isActive;
                    exam_Attendance_Trans.created_by = model.login_id;
                    exam_Attendance_Trans.creation_datetime = DateTime.Now;
                   
                    
                    exam_Attendance_Trans.login_id = role.RoleId;
                    exam_Attendance_Trans.attendance_id = model.attendance_id;
                    _db.tbl_attendance_details_trans.Add(exam_Attendance_Trans);
                    _db.SaveChanges();

                }
                else
                {


                    exam_Attendance_Trans.exam_centre_id = model.exam_centre_id;
                    exam_Attendance_Trans.trade_id = model.trade_id;
                    exam_Attendance_Trans.trade_type_id = model.trade_type_id;
                    exam_Attendance_Trans.subject_id = model.subject_id;
                    exam_Attendance_Trans.attendance = model.attendance;
                    exam_Attendance_Trans.additional_sheet_no = model.additional_sheet_no;
                    exam_Attendance_Trans.answer_sheet_no = model.answer_sheet_no;
                    exam_Attendance_Trans.exam_duration = model.exam_duration;
                    exam_Attendance_Trans.iti_trainees_id = model.iti_trainees_id;
                    exam_Attendance_Trans.block_id = model.block_id;
                    exam_Attendance_Trans.procedure_or_drawing_sheet_no = model.procedure_or_drawing_sheet_no;
                    exam_Attendance_Trans.status_id = model.status_id;
                    exam_Attendance_Trans.exam_notif_status_id = 104;

                    exam_Attendance_Trans.is_active = isActive;
                    exam_Attendance_Trans.updated_by = model.login_id;
                    exam_Attendance_Trans.updation_datetime = DateTime.Now; ;
                    exam_Attendance_Trans.login_id = role.RoleId;
                    exam_Attendance_Trans.attendance_id = model.attendance_id;

                    _db.SaveChanges();
                }

                return "Saved";
            }

            catch (Exception ex)
            {
                return "Failed";
            }

        }

        public AtendanceDet ViewAttendanceDtlsDLL(AtendanceDet attendanceDet)//AtendanceDet attendanceDet
        {
            var dtls = (from n in _db.tbl_attendance_details
                        join p in _db.tbl_exam_notif_status_mast on n.exam_notif_status_id equals p.exam_notif_status_id
                        join b in _db.tbl_block_or_room on n.block_id equals b.block_id

                        where n.attendance_id == attendanceDet.attenId
                        select new AtendanceDet
                        {
                            Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                            Trainee_Roll_Num = n.trainee_roll_num,
                            attenId = n.attendance_id,


                            BlockNo = n.block_id.ToString(),

                            login_id = n.login_id,
                            StatusId = n.status_id,
                            status_desc = p.exam_notif_status_desc,
                            exam_notif_status_id = p.exam_notif_status_id,
                            status_id = n.exam_notif_status_id,
                            attendSavePath = b.upload_pdf_file,
                            attendExcelPath = b.upload_excel_file,
                            
                        }).FirstOrDefault();
            return dtls;
        }

         public tbl_attendance_details_trans Getnotify(AtendanceDet attendanceDtls)
        {
            var details = (from n in _db.tbl_attendance_details_trans
                           where n.attendance_id == attendanceDtls.attenId && n.login_id == attendanceDtls.RoleId
                           select n).FirstOrDefault();
            return details;
        }

        public tbl_attendance_details_trans GetnotifyLoginID(AtendanceDet attendanceDtls)
        {
            var details = (from n in _db.tbl_attendance_details_trans
                           where n.attendance_id == attendanceDtls.attenId && n.login_id == attendanceDtls.login_id
                           select n).FirstOrDefault();
            return details;
        }

        public int UpdateStatusDLL(tbl_attendance_details_trans obj)
        {
            return _db.SaveChanges();
        }

    }
}
