using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AuditTrail
{
	public interface IInsertaudittrailBLL
	{
		void InsertAuditLog(Audittraillog auditTrail);
	}
}
