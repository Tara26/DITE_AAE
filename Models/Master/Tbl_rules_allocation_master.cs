using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class Tbl_rules_allocation_master
    {
        [Key]
        public int Rules_allocation_master_id { get; set; }
        public int Exam_Year { get; set; }
        public int Status_Id { get; set; }
        public DateTime Trans_Date { get; set; }
        public int CourseId { get; set; }
        public int FlowId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class tbl_Vertical_rules
    {
        [Key]
        public int Vertical_rules_id { get; set; }
        public string Vertical_Rules { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_Vertical_rule_value
    {
        [Key]
        public int Vertical_rules_value_id { get; set; }
        public int Vertical_rules_id { get; set; }
        public decimal RuleValue { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class Tbl_horizontal_rules
    {
        [Key]
        public int Horizontal_rules_id { get; set; }
        public string Horizontal_rules { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string Horizontal_rules_desc { get; set; }
    }

    public class tbl_horizontal_rules_values
    {
        [Key]
        public int Horizontal_rules_value_id { get; set; }
        public int Horizontal_rules_id { get; set; }
        public decimal RuleValue { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_Grades
    {
        [Key]
        public int GradeId { get; set; }
        public string GradGrades { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_syllabus_type
    {
        [Key]
        public int Syllabus_type_id { get; set; }
        public string Syllabus_type { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_Grade_percentage
    {
        [Key]
        public int Grade_percentage_id { get; set; }
        public int Grade_Id { get; set; }
        public int Syllabus_type_id { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_Grade_percentage_Value
    {
        [Key]
        public int Grade_percentage_value_id { get; set; }
        public int Grade_percentage_id { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public decimal RuleValue { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_HYD_NonHYD_regions
    {
        [Key]
        public int Hyd_NonHyd_region_id { get; set; }
        public string Region_type { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_HYD_NonHYD_candidates
    {
        [Key]
        public int Hyd_NonHyd_candidates_id { get; set; }
        public string Candidates_type { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class Tbl_Hyd_kar_rules
    {
        [Key]
        public int Hyd_NonHyd_rules_id { get; set; }
        public int Hyd_NonHyd_region_id { get; set; }
        public int Hyd_NonHyd_candidates_id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int IsActive { get; set; }
    }

    public class tbl_HYD_kar_rules_value
    {
        [Key]
        public int Hyd_NonHyd_rules_val_id { get; set; }
        public int Hyd_NonHyd_rules_id { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public decimal RuleValue { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class Tbl_other_rules
    {
        [Key]
        public int Other_rules_id { get; set; }
        public string OtherRules { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_other_rules_value
    {
        [Key]
        public int Other_rules_value_id { get; set; }
        public int Other_rules_id { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public decimal RuleValue { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
    }

    public class tbl_Year
    {
        [Key]
        public int YearID { get; set; }
        public string Year { get; set; }
        public bool IsActive { get; set; }
    }

    public class Tbl_rules_allocation_master_history
    {
        [Key]
        public int Rules_allocation_master_His_id { get; set; }
        public int Rules_allocation_master_id { get; set; }
        public int Exam_Year { get; set; }
        public int Status_Id { get; set; }
        public DateTime Trans_Date { get; set; }
        public int CourseId { get; set; }
        public int FlowId { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}
