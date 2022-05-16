using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Models
{
	public class MarksUpload
	{
		public int lm_id { get; set; }
		public string trainee_roll_num { get; set; }
		public int subject_id { get; set; }
		public int lm_obtained_omr_marks { get; set; }
		public int lm_obtained_sessional_marks { get; set; }
		public int lm_obtained_offline_exam_marks { get; set; }
		public int lm_semester { get; set; }
		public int lm_exam_type { get; set; }
		public bool lm_is_active { get; set; }
		public int lm_created_by { get; set; }
		public DateTime? lm_creation_datetime { get; set; }
		public int lm_updated_by { get; set; }
		public DateTime? lm_updation_datetime { get; set; }
		public HttpPostedFileBase UploadDocs { get; set; }
		public int user_id { get; set; }
		public SelectList SubjectList { get; set; }
		public SelectList Tradesector { get; set; }
		public SelectList Tradescheme { get; set; }
		public SelectList Tradetype { get; set; }
		public SelectList SemesterList { get; set; }
		public SelectList ExamTypeList { get; set; }
		public string code_marks_sheet_num { get; set; }

		public int tradescheme_id { get; set; }
		public int tradesector_id { get; set; }
		public int tradetype_id { get; set; }
		public string exam_marks { get; set; }
		public string unique_identification_code { get; set; }
		public List<MarksUpload> marksUploads { get; set; }
	}
}
