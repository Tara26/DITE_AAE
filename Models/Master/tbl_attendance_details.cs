using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_attendance_details
    {
        [Key]
        public int attendance_id { get; set; }
        public int?  exam_centre_id { get; set; }
        public int? trade_id { get; set; }
        public int? trade_type_id { get; set; }
        public int? subject_id { get; set; }
        public string attendance { get; set; }
        public int? additional_sheet_no { get; set; }
        public int? answer_sheet_no { get; set; }
        public int? exam_duration { get; set; }
        public int? trainee_roll_num { get; set; }
        public int? block_id { get; set; }
        public int? Procedure_or_drawing_sheet_no { get; set; }
       
        public int? status_id { get; set; }
        public bool? is_active { get; set; }
        public int created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
        public string remarks { get; set; }
        public int exam_notif_status_id { get; set; }
        public int login_id { get; set; }




    }

    public class tbl_block_or_room
    {
        [Key]
        public int block_id { get; set; }
        public string room_number { get; set; }
        public string upload_pdf_file { get; set; }
        public bool? is_active { get; set; }
        public int created_by { get; set; }
        public DateTime? creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public DateTime? updation_datetime { get; set; }
        public string upload_excel_file { get; set; }


    }
    public class tbl_attendance_details_trans
    {
        [Key]
        public int? adt_id { get; set; }
        public int? exam_centre_id { get; set; }
        public int? trade_id { get; set; }
        public int? trade_type_id { get; set; }
        public int? subject_id { get; set; }
        public string attendance { get; set; }
        public int? additional_sheet_no { get; set; }
        public int? answer_sheet_no { get; set; }
        public int? exam_duration { get; set; }
        public int? iti_trainees_id { get; set; }
        public int? block_id { get; set; }
        public int? procedure_or_drawing_sheet_no { get; set; }
        public int? status_id { get; set; }
        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
        public string adt_remarks { get; set; }
        public int exam_notif_status_id { get; set; }
        public int? login_id { get; set; }
        public int attendance_id  { get;set;}
    }
}

