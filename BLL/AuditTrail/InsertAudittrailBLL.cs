using DLL.AuditTrail;
using Models.Master;
using Models.User;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AuditTrail
{
	public class InsertAudittrailBLL : IInsertaudittrailBLL
	{
		private readonly IInsertaudittrailDLL _useraudittrail;

		public InsertAudittrailBLL()
		{
			this._useraudittrail = new InsertAudittrailDLL();
		}

		public void InsertAuditLog(Audittraillog auditTrail)
		{
			 _useraudittrail.InsertAuditLog(auditTrail);
		}
	}
}
