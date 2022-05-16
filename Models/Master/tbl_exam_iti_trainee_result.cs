using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class tbl_exam_iti_trainee_result
    {
        [Key]
        public int iti_trainee_result_id { get; set; }
        public int trade_theory { get; set; }
        public int min_trade_theory { get; set; }
        public int max_trade_theory { get; set; }
        public int workshop_calculation_sci { get; set; }
        public int min_workshop_calculation_sci { get; set; }
        public int max_workshop_calculation_sci { get; set; }
        public int engineering_drawing { get; set; }
        public int min_engineering_drawing { get; set; }
        public int max_engineering_drawing { get; set; }
        public int employability_skill { get; set; }
        public int min_employability_skill { get; set; }
        public int max_employability_skill { get; set; }
        public int trade_practical { get; set; }
        public int min_trade_practical { get; set; }
        public int max_trade_practical { get; set; }
        public int english { get; set; }
        public int min_english { get; set; }
        public int max_english { get; set; }
        public int disaster_management { get; set; }
        public int min_disaster_management { get; set; }
        public int max_disaster_management { get; set; }
        public int industrial_security_management { get; set; }
        public int min_industrial_security_management { get; set; }
        public int max_industrial_security_management { get; set; }
        public int industrial_safety_management { get; set; }
        public int min_industrial_safety_management { get; set; }
        public int max_industrial_safety_management { get; set; }
        public int fire_extinction { get; set; }
        public int min_fire_extinction { get; set; }
        public int max_fire_extinction { get; set; }
        public int material_science { get; set; }
        public int min_material_science { get; set; }
        public int max_material_science { get; set; }
        public int rescue_first_aid_tp { get; set; }
        public int min_rescue_first_aid_tp { get; set; }
        public int max_rescue_first_aid_tp { get; set; }
        public int fire_fighting_tp { get; set; }
        public int min_fire_fighting_tp { get; set; }
        public int max_fire_fighting_tp { get; set; }
        public int fundamentals_of_turbine { get; set; }
        public int min_fundamentals_of_turbine { get; set; }
        public int max_fundamentals_of_turbine { get; set; }
        public int commissioning_maintenance_of_turbine { get; set; }
        public int min_commissioning_maintenance_of_turbine { get; set; }
        public int max_commissioning_maintenance_of_turbine { get; set; }
        public int operation_of_steam_turbine { get; set; }
        public int min_operation_of_steam_turbine { get; set; }
        public int max_operation_of_steam_turbine { get; set; }
        public int fundamentals_of_various_instruments { get; set; }
        public int min_fundamentals_of_various_instruments { get; set; }
        public int max_fundamentals_of_various_instruments { get; set; }
        public int instruments_their_calibration { get; set; }
        public int min_instruments_their_calibration { get; set; }
        public int max_instruments_their_calibration { get; set; }
        public int control_field_instrumentation { get; set; }
        public int min_control_field_instrumentation { get; set; }
        public int max_control_field_instrumentation { get; set; }
        public int cane_handling_preparatory_devices { get; set; }
        public int min_cane_handling_preparatory_devices { get; set; }
        public int max_cane_handling_preparatory_devices { get; set; }
        public int operation_aintenance_of_centrifugal_machine_conveyar_cooling_tower { get; set; }
        public int min_operation_aintenance_of_centrifugal_machine_conveyar_cooling_tower { get; set; }
        public int max_operation_aintenance_of_centrifugal_machine_conveyar_cooling_tower { get; set; }
        public int operation_maintenance_of_mills_pumps_gear_box { get; set; }
        public int min_operation_maintenance_of_mills_pumps_gear_box { get; set; }
        public int max_operation_maintenance_of_mills_pumps_gear_box { get; set; }
        public int principles_of_sugar_manufacturing { get; set; }
        public int min_principles_of_sugar_manufacturing { get; set; }
        public int max_principles_of_sugar_manufacturing { get; set; }
        public int theory_of_pan_boiling { get; set; }
        public int min_theory_of_pan_boiling { get; set; }
        public int max_theory_of_pan_boiling { get; set; }
        public int pan_control_operation { get; set; }
        public int min_pan_control_operation { get; set; }
        public int max_pan_control_operation { get; set; }
        public int theory_of_centrifugal_machines { get; set; }
        public int min_theory_of_centrifugal_machines { get; set; }
        public int max_theory_of_centrifugal_machines { get; set; }
        public int centrifugal_control_operation { get; set; }
        public int min_centrifugal_control_operation { get; set; }
        public int max_centrifugal_control_operation { get; set; }
        public int course_type { get; set; }
        public int session_id { get; set; }
        public int trainee_id { get; set; }
        public int roll_number { get; set; }
        public int trade_type_id { get; set; }
        public int trae_id { get; set; }
        public int division_id { get; set; }
        public int dist_id { get; set; }
        public int exam_centre_id { get; set; }
        public int collage_id { get; set; }
        public bool? is_passfail { get; set; }


        public bool? is_active { get; set; }
        public int? created_by { get; set; }
        public Nullable<DateTime> creation_datetime { get; set; }
        public int? updated_by { get; set; }
        public Nullable<DateTime> updation_datetime { get; set; }
    }
}
