using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DLL
{
	public interface IMarksDLL
	{
		string LegacyMarksMasterUploadDLL(MarksUpload model);
		DataTable ConvertXSLXtoDataTable(string strFilePath);
		SelectList SubjectListDLL();
		SelectList SemesterListDLL();
		SelectList ExamTypeListDLL();
		string UploadDataDLL(MarksUpload model);
		List<MarksUpload> GetTraineesDLL(MarksUpload model);
		SelectList TradeSectorDLL();
		SelectList TradeSchemeDLL();
		SelectList TradeTypeDLL();

	}
}
