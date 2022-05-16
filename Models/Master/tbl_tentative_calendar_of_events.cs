using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_tentative_calendar_of_events
    {
        [Key]
        public int Tentative_admsn_evnt_clndr_Id { get; set; }
        public DateTime?  Notification_Date { get; set; }
        public int  Course_Id { get; set; }
        public string  Admsn_ntf_num { get; set; }
        public string  Notif_desc { get; set; }
        public string Admsn_notif_doc { get; set; }
        public int  StatusId { get; set; }
        public string  Remarks { get; set; }
        public int?  CreatedBy { get; set; }
        public DateTime?  CreatedOn { get; set; }
        public int DeptId { get; set; }
        public int session { get; set; }
        public int admissionNotificationId { get; set; }
        public int? applicantType { get; set; }
        //public int FlowId { get; set; }
        
        public string EmailId { get; set; }

    }
}
