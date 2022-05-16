using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_admsn_ntf_details
    {
        [Key]
        public int Admsn_notif_Id { get; set; }
        public int CourseId { get; set; }
        public string AdmsnNtfNum { get; set; }
        public string Notif_desc { get; set; }
        public DateTime? Admsn_ntf_Date { get; set; }
        public int DeptId { get; set; }
        public DateTime? Appli_Subm_LastDate { get; set; }
        public string Admsn_notif_Doc { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int FlowId { get; set; }
        public int applicantType { get; set; }
        public string Admsn_notif_Doc_Publish { get; set; }
    }
}
