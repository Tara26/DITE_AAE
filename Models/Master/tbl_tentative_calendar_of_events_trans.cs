using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_tentative_calendar_of_events_trans
    {
        [Key]
        public int Admsn_tentative_calndr_trans_Id { get; set; }
        public int Tentative_admsn_evnt_clndr_Id { get; set; }
        public DateTime Trans_date { get; set; }
        public int Status_id { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
       
    }
}
