using DLL.DBConnection;
using Models;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DLL
{
	public class MarksDLL : IMarksDLL
	{
		private readonly DbConnection _db = new DbConnection();
		string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

		public string LegacyMarksMasterUploadDLL(MarksUpload model)
		{
			if (!string.IsNullOrEmpty(model.UploadDocs.FileName) && (model.UploadDocs.ContentLength > 0))
			{
				DataTable EntryDt_tbl_legacy_marks_mast = new DataTable("EntryTable_tbl_legacy_marks_mast");
				DataColumn trainee_roll_num = new DataColumn("trainee_roll_num", typeof(System.String));
				DataColumn publicsubject_id = new DataColumn("publicsubject_id", typeof(System.Int16));
				DataColumn lm_obtained_omr_marks = new DataColumn("lm_obtained_omr_marks", typeof(System.Int16));
				DataColumn lm_obtained_sessional_marks = new DataColumn("lm_obtained_sessional_marks", typeof(System.Int16));
				DataColumn lm_obtained_offline_exam_marks = new DataColumn("lm_obtained_offline_exam_marks", typeof(System.Int16));
				DataColumn lm_semester = new DataColumn("lm_semester", typeof(System.Int16));
				DataColumn lm_exam_type = new DataColumn("lm_exam_type", typeof(System.Int16));
				DataColumn lm_is_active = new DataColumn("lm_is_active", typeof(System.Boolean));
				DataColumn lm_created_by = new DataColumn("lm_created_by", typeof(System.Int16));
				DataColumn lm_creation_datetime = new DataColumn("lm_creation_datetime", typeof(System.DateTime));

				EntryDt_tbl_legacy_marks_mast.Columns.AddRange(new DataColumn[] { trainee_roll_num, publicsubject_id, lm_obtained_omr_marks, lm_obtained_sessional_marks, lm_obtained_offline_exam_marks, lm_semester, lm_exam_type, lm_is_active, lm_created_by, lm_creation_datetime });

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
					//if (dt.Columns.Count == 12)
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
						string Rollnum = Convert.ToString(dt.Rows[j][0].ToString().ToUpper());
						string Subjectid = Convert.ToString(dt.Rows[j][1].ToString().ToUpper());
						string Omrmarks = Convert.ToString(dt.Rows[j][2].ToString().ToUpper());
						string Sessionalmarks = Convert.ToString(dt.Rows[j][3].ToString().ToUpper());
						string Offlineexammarks = Convert.ToString(dt.Rows[j][4].ToString().ToUpper());
						string Semester = Convert.ToString(dt.Rows[j][5].ToString().ToUpper());
						string Examtype = Convert.ToString(dt.Rows[j][6].ToString().ToUpper());
						tbl_legacy_marks tbl_legacy_marks = _db.tbl_legacy_marks.Where(x => x.trainee_roll_num.ToString().ToUpper() == Rollnum).FirstOrDefault();
						if (tbl_legacy_marks != null)
						{

						}
						else
						{
							//tbl_legacy_marks tbl_legacy_marks = new tbl_legacy_marks();
							tbl_legacy_marks.trainee_roll_num = Rollnum;
							tbl_legacy_marks.subject_id = Convert.ToInt32(Subjectid);
							tbl_legacy_marks.lm_obtained_omr_marks = Convert.ToInt32(Omrmarks);
							tbl_legacy_marks.lm_obtained_sessional_marks = Convert.ToInt32(Sessionalmarks);
							tbl_legacy_marks.lm_obtained_offline_exam_marks = Convert.ToInt32(Offlineexammarks);
							tbl_legacy_marks.lm_semester = Convert.ToInt32(Semester);
							tbl_legacy_marks.lm_exam_type = Convert.ToInt32(Examtype);
							tbl_legacy_marks.lm_is_active = true;
							tbl_legacy_marks.lm_created_by = model.user_id;
							tbl_legacy_marks.lm_creation_datetime = DateTime.Now;
							_db.tbl_legacy_marks.Add(tbl_legacy_marks);
							_db.SaveChanges();
						}


					}
					//}
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


		public string UploadDataDLL(MarksUpload model)
		{
			tbl_legacy_marks tbl_legacy_marks = _db.tbl_legacy_marks.Where(a => a.trainee_roll_num == model.trainee_roll_num).FirstOrDefault();
			if (tbl_legacy_marks == null)
			{
				tbl_legacy_marks = new tbl_legacy_marks();
				tbl_legacy_marks.trainee_roll_num = model.trainee_roll_num;
				tbl_legacy_marks.subject_id = model.subject_id;
				tbl_legacy_marks.lm_obtained_omr_marks = model.lm_obtained_omr_marks;
				tbl_legacy_marks.lm_obtained_sessional_marks = model.lm_obtained_sessional_marks;
				tbl_legacy_marks.lm_obtained_offline_exam_marks = model.lm_obtained_offline_exam_marks;
				tbl_legacy_marks.lm_semester = model.lm_semester;
				tbl_legacy_marks.lm_exam_type = model.lm_exam_type;
				tbl_legacy_marks.lm_is_active = true;
				tbl_legacy_marks.lm_created_by = model.user_id;
				tbl_legacy_marks.lm_creation_datetime = DateTime.Now;
				_db.tbl_legacy_marks.Add(tbl_legacy_marks);
				_db.SaveChanges();
				return "Saved";
			}
			else
			{
				return "Trainee Rollnum already Exist";
			}


		}


		public SelectList SubjectListDLL()
		{
			List<SelectListItem> SubjectList = _db.tbl_subject.Select(x => new SelectListItem
			{
				Text = x.subject_name.ToString(),
				Value = x.subject_id.ToString()
			}).ToList();
			var ExamTList = new SelectListItem()
			{

				Value = null,
				Text = "Select Subject",

			};
			SubjectList.Insert(0, ExamTList);
			return new SelectList(SubjectList, "Value", "Text");
		}

		public SelectList SemesterListDLL()
		{
			List<SelectListItem> SemesterList = _db.tbl_semester.Select(x => new SelectListItem
			{
				Text = x.semester.ToString(),
				Value = x.semester_id.ToString()
			}).ToList();
			var ExamTList = new SelectListItem()
			{

				Value = null,
				Text = "Select Semester",

			};
			SemesterList.Insert(0, ExamTList);
			return new SelectList(SemesterList, "Value", "Text");
		}

		public SelectList ExamTypeListDLL()
		{
			List<SelectListItem> ExamTypeList = _db.tbl_exam_type_mast.Select(x => new SelectListItem
			{
				Text = x.exam_type_name.ToString(),
				Value = x.exam_type_id.ToString()
			}).ToList();
			var ExamTList = new SelectListItem()
			{

				Value = null,
				Text = "Select Exam Type",

			};
			ExamTypeList.Insert(0, ExamTList);
			return new SelectList(ExamTypeList, "Value", "Text");
		}




		public List<MarksUpload> GetTraineesDLL(MarksUpload model)
		{
			var TraineeList = (from n in _db.tbl_code_marks_sheet_master
							   join u in _db.tbl_unique_trainee_mast on n.cms_ut_id equals u.ut_id
							   join f in _db.tbl_trainee_fee_paid on u.ut_tfp_id equals f.tfp_id
							   join t in _db.tbl_iti_trainees_details on f.trainee_id equals t.trainee_id
							   where n.code_marks_sheet_num == model.code_marks_sheet_num
							   select new MarksUpload
							   {
								   unique_identification_code = u.unique_identification_code,
								   trainee_roll_num = t.trainee_roll_num,
							   }).ToList();
			return TraineeList;
		}


		public SelectList TradeSectorDLL()
		{
			List<SelectListItem> Tradesector = _db.tbl_trade_sector.Select(x => new SelectListItem
			{
				Text = x.trade_sector.ToString(),
				Value = x.trade_sector_id.ToString()
			}).ToList();
			var Tradesec = new SelectListItem()
			{

				Value = null,
				Text = "Select Trade Sector",

			};
			Tradesector.Insert(0, Tradesec);
			return new SelectList(Tradesector, "Value", "Text");
		}

		public SelectList TradeSchemeDLL()
		{
			List<SelectListItem> TradeScheme = _db.tbl_trade_scheme.Select(x => new SelectListItem
			{
				Text = x.trade_scheme.ToString(),
				Value = x.ts_id.ToString()
			}).ToList();
			var tradeschm = new SelectListItem()
			{

				Value = null,
				Text = "Select Trade Scheme",

			};
			TradeScheme.Insert(0, tradeschm);
			return new SelectList(TradeScheme, "Value", "Text");
		}


		public SelectList TradeTypeDLL()
		{
			List<SelectListItem> Tradetype = _db.tbl_trade_type_mast.Select(x => new SelectListItem
			{
				Text = x.trade_type_name.ToString(),
				Value = x.trade_type_id.ToString()
			}).ToList();
			var tradetyp = new SelectListItem()
			{

				Value = null,
				Text = "Select Trade Type",

			};
			Tradetype.Insert(0, tradetyp);
			return new SelectList(Tradetype, "Value", "Text");
		}

	}
}
