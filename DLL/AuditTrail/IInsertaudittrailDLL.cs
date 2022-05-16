using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.AuditTrail
{
	public interface IInsertaudittrailDLL
	{
		void InsertAuditLog(Audittraillog auditTrail);
	}
}
