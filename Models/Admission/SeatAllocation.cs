using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class SeatAllocation
    {
        public List<SeatAllocation> seatAllocationmodel { get; set; }

        public string role_description { get; set; } 
        public string FormwardFrom { get; set; } 
        public string ForwardedTo { get; set; } 
        public string SeatAllocationPartId { get; set; } 
        public int Rules_allocation_masterid { get; set; } 
        public int Exam_Year { get; set; }
        public int SlNo { get; set; }
        public string ExamYear { get; set; }
        public int Status_Id { get; set; }
        public DateTime Trans_Date { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int FlowId { get; set; }
        public string Remarks { get; set; }
        public string userRole { get; set; }
        public string CommentsCreatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public decimal[] ConsolidatedVerticalRules { get; set; }
        public decimal[] ConsolidatedHorizontalRules { get; set; }
        public decimal[] ConsolidatedGradeRules { get; set; }
        public decimal[] ConsolidatedOtherRules { get; set; }
        public decimal[] ConsolidatedHyderabadRegion { get; set; }
        //public decimal[] seatGovtandMgmnt { get; set; }

        public int Vertical_rulesid { get; set; } 
        public string Vertical_Rules { get; set; }
        public bool IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Horizontal_rules_id { get; set; } Primarykey
        public string Horizontal_rules { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        public int Horizontal_rules_valueid { get; set; } 
        public int Horizontal_rulesid { get; set; }
        public decimal RuleValue { get; set; }
       
        //public int Rules_allocation_master_id { get; set; }
        public int StatusId { get; set; }
        //public string Remarks { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int GradeId { get; set; } Primarykey
        public string GradGrades { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        public int Syllabus_type_id { get; set; } 
        public string Syllabus_type { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Grade_percentage_id { get; set; } Primarykey
        public int Grade_Id { get; set; }
        //public int Syllabus_type_id { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Grade_percentage_value_id { get; set; } Primarykey
        public int Grade_percentageid { get; set; }
        //public int Rules_allocation_master_id { get; set; }
        //public float RuleValue { get; set; }
        //public int StatusId { get; set; }
        //public string Remarks { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Hyd_NonHyd_region_id { get; set; } Primarykey
        public string Region_type { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Hyd_NonHyd_candidates_id { get; set; } Primarykey
        public string Candidates_type { get; set; }
        //public int IsActive { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Hyd_NonHyd_rules_id { get; set; } Primarykey
        //public int Hyd_NonHyd_region_id { get; set; }
        //public int Hyd_NonHyd_candidates_id { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }
        //public int IsActive { get; set; }

        //public int Hyd_NonHyd_rules_val_id { get; set; } Primarykey
        public int Hyd_NonHyd_rules_valid { get; set; }
        //public int Hyd_NonHyd_rules_id { get; set; }
        public int Hyd_NonHyd_rulesid { get; set; }
        //public int Rules_allocation_master_id { get; set; }
        //public float RuleValue { get; set; }
        //public int StatusId { get; set; }
        //public string Remarks { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        //public int Other_rules_id { get; set; } Primarykey
        public string OtherRules { get; set; }
        //public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        //public int CreatedBy { get; set; }
   
        //public int Other_rules_value_id { get; set; } Primarykey
        public int Other_rulesid { get; set; }
        //public int Rules_allocation_master_id { get; set; }
        //public float RuleValue { get; set; }
        //public int StatusId { get; set; }
        //public string Remarks { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public int CreatedBy { get; set; }

        public int YearID { get; set; }
        public string Year { get; set; }
        public string StatusName { get; set; }
        public int ForwardTo { get; set; } 
        public int Seat_typeId { get; set; } 
        public int Govtseats { get; set; }  
        public int Managementseats { get; set; } 

    }
}
