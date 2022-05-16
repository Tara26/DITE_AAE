using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_affiliation_uploadexcel
    {
        [Key]
        public int Affliation_Id { get; set; }
        public string Name_Of_ITI { get; set; }
        public string Mis_Code { get; set; }
        public string Type_Of_ITI { get; set; }
        public string Trade { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Taluk { get; set; }
        public string Panchayat { get; set; }
        public string Village { get; set; }
        public string Constituency { get; set; }
        public string Built_Up_Area { get; set; }
        public string Css_Code { get; set; }
        public string Geo_Location { get; set; }
        public string Address { get; set; }
        public string Location_Type { get; set; }
        public string Email_Id { get; set; }
        public string Phone_Number { get; set; }
        public string Affilation_Date { get; set; }
        public string No_Trades { get; set; }
        public string No_Units { get; set; }
        public string No_Shifts { get; set; }
        public DateTime? Created_On { get; set; }
        public int? Created_By { get; set; }
    }
}
