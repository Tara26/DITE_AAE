using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_special_trade_types
    {
        [Key]
        public int stt_id { get; set; }
        public string stt_description { get; set; }

        public bool? stt_is_active { get; set; }
        public int? stt_created_by { get; set; }
        public DateTime? stt_created_on { get; set; }
        public int? stt_updated_by { get; set; }
        public DateTime? stt_updated_on { get; set; }
    }

	//public class tbl_mapping_retotaling_officers
	//{
	//	[Key]
	//	public int id { get; set; }
	//	public int kgid_id { get; set; }
	//	public int exam_year { get; set; }
	//	public int trade_id { get; set; }
	//	public bool? is_active { get; set; }
	//	public int? created_by { get; set; }
	//	public DateTime? creation_datetime { get; set; }
	//	public int? updated_by { get; set; }
	//	public DateTime? updation_datetime { get; set; }

	//}
	
}
