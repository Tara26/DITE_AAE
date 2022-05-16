using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
	public class tbl_audittrail
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string IPAddress { get; set; }
		public string DateAndTime { get; set; }
		public string ActionPerformed { get; set; }
		public string Status { get; set; }
	}
}
