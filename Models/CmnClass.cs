using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CmnClass
    {
        public enum pkvalueForHorizontal
        {
            Women = 1, PersonsDisabilities = 2, ExService = 3, KannadaMedium = 4, LandLoser = 5, KashmiriMigrants = 6, EconomicWeaker = 7, GeneralPool = 8
        }

        public enum stRegNumbStDiv
        {
            KA, B, M, H, K, G, P
        }

        //public static int CurrentYear = 21;

        public enum roleLevel
        {
            Department = 1, Division = 2
        }
        
        public enum MenuList
        {
            Modules = 8
        }

        public enum seniorityRole
        {
            Commissioner = 10,
            Director = 20,
            AdditionalDirector = 30,
            JointDirector = 40,
            DeputyDirector = 50,
            AssistantDirector = 60,
            OfficeSuperitendent = 70,
            CaseWorker = 80,
            ITIAdmin = 90,
            Applicant = 100,
            DivisionOfficer = 110,
            VerificationOfficer = 120,
            AdmissionOfficer = 130
        }

        public enum Role
        {
            DeputyDirector = 5,
            CaseWorker = 8,
            ITIAdmin = 9,
            Applicant = 10,
            VerOff = 12,
            JDDiv = 16,
            AdmOff = 18
        }
        public enum courseType
        {
            SCVT = 100, NCVT = 101
        }

        public enum applFormDescStatus
        {
            SeatAllocated = 13,
            SeatNOTAllocated = 18,
            Documents_Verified = 4
        }
        public enum Status
        {
            Approve = 2,
            SubmittedForReview = 5,
            Affiliated = 6,
            Published = 19,
            SubmitTransferAppl = 20,
            SeatAlloted = 22,
            SeatNOTAlloted = 23,
            seatavailabilityApprovePublish=4,
            ApplicantRegistered=24
        }

        public enum AdmissionStatus
        {
            Pending = 1,
            Admitted = 6,
            Rejected = 3
        }

        public enum Dept
        {
            Admission = 1,
        }
        public enum ApplicantTypes
        {
            Direct = 3
        }
        public static class FeePaymentReceiptText
        {
            public static string Admission { get { return "AF"; } }
            public static string DocVerify { get { return "DV"; } }
        }
        public static class statusName
        {           
            public static string tentativePublish { get { return "Tentative Gradation List Published "; } }          
            public static string approved { get { return "Approved "; } }           
        }


        public static class EmailAndMobileMsgs
        {
            // Applicant Messages
            public static string ApplicantOTPMsg { get { return "Welcome to our online ITI Admission Programme. Please use this code {0} to complete the mobile validation."+System.Environment.NewLine+ "From" + System.Environment.NewLine + "CITE Admission Team."; } }
            public static string ApplicantEmailConfirmMsg { get { return "Hello {0}, /n/nThank you for registering to our online ITI Admission Programme. Now you are eligible to fill the application form and participate in the online seat allocation admission./nThe user ID is default your registered mobile number {1} and password as you entered while registration./nWe look forward to seeing you in our Industrial Training Programmes./n/nRegards,/n/nDITE Admission Cell"; } }
            public static string ApplicantMobileConfirmMsg { get { return "Hello {0}, /nYour registration is Successful, Your User ID for login is registered mobile number: {1}/n/nRegards,/n/nDITE Admission Cell"; } }
            public static string EmailOTPSubject { get { return "DITE Verification"; } }
            public static string ApplicantForgotOTPMsg { get { return "Reset the password for your DITE account. Please enter the OTP {0}to validate and enter the new password." + System.Environment.NewLine + "From" + System.Environment.NewLine + "CITE Admission Team."; } }


            //Employee Messages
            public static string EmployeeOTPMsg { get { return "Welcome to our online ITI Admission Programme. Please use this code {0} to complete the mobile validation." + System.Environment.NewLine + "From" + System.Environment.NewLine + "CITE Admission Team."; } }
            public static string EmailGRVSubject { get { return "Dear {0},Grievance raised successfully and Grievance number {1}. Please keep the Generated Grievance No.{1} for future communication. Regards CITE Team"; } }
            public static string MobileGRVApprove { get { return "Dear {0},Grievance against merit list is reviewed and closed.Check the application status in CITE application under Applicant -Application status.Regards"; } }

            //public static string EmployeeOTPMsg { get { return "Dear {0}, /nWelcome to our DITE ESS Application. /nPlease use this code {1} to complete the Registration process./n/nRegards, /n/nDITE."; } }
            public static string EmployeeEmailConfirmMsg { get { return "Hello {0}, /n/nThank you for registering our DITE ESS Application. Now you are eligible to use our application based on your role./nThe user ID is default your KGID number {1} and password as you entered while registration./nWe look forward to seeing you in using our Application./n/nRegards,/n/nDITE"; } }
            public static string EmployeeMobileConfirmMsg { get { return "Hello {0}, /nYour registration is Successful, Your User ID for login is KGID number: {1}/nRegards,/nDITE"; } }

            public static string EmpEmailSubject { get { return "Employee Registration Success"; } }
            public static string EmployeeForgotOTPMsg { get { return "Reset the password for your DITE account. Please enter the OTP {0}to validate and enter the new password." + System.Environment.NewLine + "From" + System.Environment.NewLine + "CITE Admission Team."; } }
            public static string EmailGRVSubjectMOB { get { return "Dear {0}, You have successfully submitted the Grievance agianst Tentative merit list & Generated Grievance number {1},please visit to the selected verificaition center to review the grievance documents for verification. You can check the application status in CITE application under Applicant  Regards CITE Team"; } }
            public static string EmailGRVApprove { get { return "Dear {0},The Grievance agianst Tentativve merit list is reviewed by verification officer and closed the ticket. You can check the application status in CITE application under Applicant -Application status.Regards CITE Team"; } }
        }

        public static List<MenuItem.MenuItems> GetCmnCustomMenuList(List<MenuItem.MenuItems> res)
        {
            //Case Worker - Admission Notification
            res.Add(new MenuItem.MenuItems {Url = "Admission/GetAdmissionNotification" });
            res.Add(new MenuItem.MenuItems {Url = "Admission/ViewAdmissionNotification" });
            return res;
        }

    }
}
