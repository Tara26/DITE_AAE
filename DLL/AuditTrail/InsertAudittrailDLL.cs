using Models.Master;
using Models.User;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DBConnection;
namespace DLL.AuditTrail
{
	public class InsertAudittrailDLL : IInsertaudittrailDLL
	{
		private readonly DBConnection.DbConnection _db = new DBConnection.DbConnection();
		//private static readonly log4net.ILog log = log4net.LogManager.GetLogger
		// (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public void InsertAuditLog(Audittraillog audittrail)
		{
			try
			{
				tbl_audittrail tblaudittrail = new tbl_audittrail();
				tblaudittrail.UserId = audittrail.UserId;
				tblaudittrail.IPAddress = audittrail.IPAddress;
				tblaudittrail.DateAndTime = audittrail.DateAndTime;
				tblaudittrail.ActionPerformed = audittrail.ActionPerformed;
				tblaudittrail.Status = audittrail.Status;
				_db.tbl_audittrail_mast.Add(tblaudittrail);
				_db.SaveChanges();
			//	log.Info("Audit Informations Stored Successfully");
			}
			catch (Exception ex)
			{
			//	log.Error(ex.Message.ToString());
			}


		}
	}
}
