using DLL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BLL
{
	public class MarksBLL : IMarksBLL
	{
		private readonly IMarksDLL _marksDll;
		public MarksBLL()
		{
			this._marksDll = new MarksDLL();
		}

		public string LegacyMarksMasterUploadBLL(MarksUpload model)
		{
			return _marksDll.LegacyMarksMasterUploadDLL(model);
		}

		public SelectList SubjectListBLL()
		{
			return _marksDll.SubjectListDLL();
		}

		public SelectList TradeSectorBLL()
		{
			return _marksDll.TradeSectorDLL();
		}

		public SelectList TradeSchemeBLL()
		{
			return _marksDll.TradeSchemeDLL();
		}

		public SelectList TradeTypeBLL()
		{
			return _marksDll.TradeTypeDLL();
		}

		public SelectList SemesterListBLL()
		{
			return _marksDll.SemesterListDLL();
		}

		public SelectList ExamTypeListBLL()
		{
			return _marksDll.ExamTypeListDLL();
		}

		public string UploadDataBLL(MarksUpload model)
		{
			return _marksDll.UploadDataDLL(model);
		}

		public List<MarksUpload> GetTraineesBLL(MarksUpload model)
		{
			return _marksDll.GetTraineesDLL(model);
		}
	}
}
