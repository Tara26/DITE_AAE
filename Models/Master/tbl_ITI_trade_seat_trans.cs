using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_ITI_trade_seat_trans
    {
        [Key]
        public long Trade_ITI_seat_trans_id { get; set; }
        public int Trade_ITI_seat_Id { get; set; }
        public DateTime Trans_Date { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? FlowId { get; set; }
    }
}
