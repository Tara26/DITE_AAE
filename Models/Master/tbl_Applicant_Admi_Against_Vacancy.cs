using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Master
{
    public class tbl_Applicant_Admi_Against_Vacancy
    {
        [Key]
        public int ApplicantAdmiAgainstVacancyId { get;set;}
        public int ApplicantId{get;set;}
        public int AcademicMonths{get;set;}
        public int AcademicYear{get;set;}
        public bool DirectAdmission{get;set;}
        public string ApplicationNumber{get;set;}
        public string StateRegistrationNumber{get;set;}
        public int Division{get;set;}
        public int District{get;set;}
        public int Taluk{get;set;}
        public int InstituteType{get;set;}
        public int ITIInstitute{get;set;}
        public int CourseType{get;set;}
        public int AdmissionVerticalCategory{get;set;}
        public int AdmissionHorizontalCategory{get;set;}
        public int TraineeType{get;set;}
        public int TradeName{get;set;}
        public int Units{get;set;}
        public int Shift{get;set;}
        public bool DualSystem{get;set;}
        public DateTime AdmisionTime{get;set;}
        public int TuitionFee{get;set;}
        public bool TuitionFeePaidStatus{get;set;}
        public DateTime PaymentDate{get;set;}
        public string ReceiptNumber{get;set;}
        public int AdmissionStatus{get;set;}
        public bool IsActive{get;set;}
        public int CreatedBy { get;set;}
    }
}
