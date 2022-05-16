using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.User
{
	public class Audittraillog
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string IPAddress { get; set; }
		public string DateAndTime { get; set; }
		public string ActionPerformed { get; set; }
		public string Status { get; set; }
	}
}
