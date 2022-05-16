using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_admsn_ntf_trans_history
    {
        [Key]
        public int Admsn_notif_trans_his_Id { get; set; }
        public int Admsn_notif_trans_Id { get; set; }
        public int Admsn_notif_Id { get; set; }
        public DateTime Trans_date { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public int FlowId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }        
    }
}
