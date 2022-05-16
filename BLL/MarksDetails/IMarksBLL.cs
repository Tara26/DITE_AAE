using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
	public interface IMarksBLL
	{
		string LegacyMarksMasterUploadBLL(MarksUpload model);
		SelectList SubjectListBLL();
		SelectList SemesterListBLL();
		SelectList ExamTypeListBLL();
		string UploadDataBLL(MarksUpload model);
		List<MarksUpload> GetTraineesBLL(MarksUpload model);
		SelectList TradeSectorBLL();
		SelectList TradeSchemeBLL();
		SelectList TradeTypeBLL();
	}
}

