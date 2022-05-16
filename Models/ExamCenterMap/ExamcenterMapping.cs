using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.ExamCenterMap
{
    public class ExamcenterMapping
    {
        public int Course_id { get; set; }
        public string Course_type_name { get; set; }
        public byte Course_is_active { get; set; }
        public DateTime Course_creation_datetime { get; set; }
        public DateTime Course_updation_datetime { get; set; }
        public int Course_type_created_by { get; set; }
        public int Course_type_updated_by { get; set; }
        public SelectList Course_Type_List { get; set; }

        public int Division_id { get; set; }
        public int Division_name { get; set; }
        public bool? Division_is_active { get; set; }
        public int? Division_created_by { get; set; }
        public DateTime? Division_creation_datetime { get; set; }
        public int Division_updated_by { get; set; }
        public SelectList DivisionList { get; set; }

        public int District_dist_id { get; set; }
        public string District_dist_name { get; set; }
        public int Dist_division_id { get; set; }
        public bool Dist_is_active { get; set; }
        public int Dist_created_by { get; set; }
        public int Dist_id_creation_datetime { get; set; }
        public int Dist_updated_by { get; set; }
        public int Dist_updation_datetime { get; set; }
        public SelectList DistrictList { get; set; }

       
        public int Iti_college_id { get; set; }
        public string Iti_college_code { get; set; }
        public int Iti_district_id { get; set; }
        public int Iti_taluk_id { get; set; }
        public string Iti_village_or_town { get; set; }
        public string Iti_college_address { get; set; }
        public int Iti_location_id { get; set; }
        public int Iti_css_code { get; set; }
        public string Iti_college_name { get; set; }
        public int Iti_grading { get; set; }
        public string Iti_geo { get; set; }
        public string Iti_file_ref_no { get; set; }
        public bool Iti_is_active { get; set; }
        public int Iti_created_by { get; set; }
        public bool IsSelected { get; set; }

        public DateTime Iti_creation_datetime { get; set; }
        public int Iti_updated_by { get; set; }
        public DateTime Iti_updation_datetime { get; set; }
        public SelectList CollegeLists { get; set; }
    }
}
