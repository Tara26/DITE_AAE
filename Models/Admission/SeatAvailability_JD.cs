using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Admission
{
    public class SeatAvailability_JD
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
        public string Division_name { get; set; }
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

        public int taluk_id { get; set; }
        public string taluk_name { get; set; }
        public Nullable<int> district_id { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> creation_datetime { get; set; }
        public Nullable<int> updated_by { get; set; }
        public Nullable<System.DateTime> updation_datetime { get; set; }
        public SelectList TalukList { get; set; }


        public int role_id { get; set; }
        public string role_name { get; set; }
    }
}
