using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Admission
{
    public class Memeber_Model
    {
        public string StatusCode { get; set; }
        public string StatusText { get; set; }
        public List<ResultDataList> Member_model_details { get; set; }

    }
    public class ResultDataList
    {
        public string MEMBER_NAME_ENG { get; set; }
        public string MBR_DOB { get; set; }
        public string MBR_GENDER { get; set; }
        public string Member_District_details { get; set; }
        public string Member_Taluk_details { get; set; }
        public string Member_Panchayat_details { get; set; }
        public string data_is_null { get; set; }
        public string FAMILY_MEMBER_ID { get; set; }
        public bool IsExistUser { get; set; }
        public List<ResultDataList> Member_model_details { get; set; }
        public string MBR_DOB_SURV { get; set; }
        public string MBR_GENDER_SURV { get; set; }
        public string Mobile_number { get; set; }
        public string FAMILY_ID { get; set; }
        public string MEMBER_ID { get; set; }
        public string Response_ID { get; set; }
        public string LGD_DISTRICT_CODE { get; set; }
        public string LGD_TALUK_CODE { get; set; }
        public string LGD_VILLAGE_CODE { get; set; }
        public string MBR_CASTE_RD_NO { get; set; }
        public string MBR_CASTE { get; set; }
        public string MBR_CASTE_CATEGORY { get; set; }
        public string MBR_NPR_FATHER_NAME { get; set; }
        public string MBR_NPR_MOTHER_NAME { get; set; }
        public string MBR_EDUCATION_ID { get; set; }
        public string MBR_INCOME_RD_NO { get; set; }
        public string MBR_Disability_Applicant_No { get; set; }


        public string ApplicantName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string ApplicantFatherName { get; set; }
        public string ApplicantMotherName { get; set; }
        public string SSLC_MaxMarks { get; set; }
        public string SSLC_Secured_Marks { get; set; }
        public string SSLC_Results { get; set; }
        public string SSLC_Percentage { get; set; }
        public string Applicant_School_Address { get; set; }

        public string FacilityCode { get; set; }
        public string DateOfIssue { get; set; }
        public string CertificateValidUpto { get; set; }
        public string AnnualIncome { get; set; }
        public string RDnumberValidity { get; set; }
        
    }
}
