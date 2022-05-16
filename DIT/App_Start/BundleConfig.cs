using System.Web;
using System.Web.Optimization;

namespace DIT
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region script Js

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.6.0.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryDataTable").Include(
            //            "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Bootbox").Include(
                        "~/Scripts/bootbox.js",
                        "~/Scripts/bootbox.min.js",
                        "~/Scripts/bootbox.locales.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/MasterDataUpload").Include(
                        "~/Scripts/Admission/MasterDataUpload.js"));

            //Seat Allocation Rules
            bundles.Add(new ScriptBundle("~/bundles/SeatAllocationReviewView").
                Include("~/Scripts/Admission/SeatAllocationReviewView.js",
                                         "~/Scripts/Admission/global.js"));
            bundles.Add(new ScriptBundle("~/bundles/SeatAllocationInsertUpdateRules").
                Include("~/Scripts/Admission/SeatAllocationInsertUpdateRules.js", "~/Scripts/Admission/global.js"));
            bundles.Add(new ScriptBundle("~/bundles/SeatAllocationApproveView").
                Include("~/Scripts/Admission/SeatAllocationApproveView.js",
                                        "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/TradeSeatTransJD").Include(
                        "~/Scripts/Admission/TradeSeatTransJD.js"));

            bundles.Add(new ScriptBundle("~/bundles/SeatAllocation").Include(
                       "~/Scripts/Admission/SeatAllocation.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Content/frontend/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUi").Include(
                      "~/Content/frontend/js/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryFlex").Include(
                      "~/Content/frontend/js/jquery.flexslider.js"));
            bundles.Add(new ScriptBundle("~/bundles/steller").Include(
                      "~/Content/frontend/header/stellarnav.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/carousel").Include(
                      "~/Content/frontend/js/owl.carousel.js"));
            //bundles.Add(new ScriptBundle("~/bundles/SmoothScroll").Include(
            //          "~/Content/frontend/js/SmoothScroll.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/move").Include(
                      "~/Content/frontend/js/move-top.js"));
            bundles.Add(new ScriptBundle("~/bundles/easing").Include(
                      "~/Content/frontend/js/easing.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdmissionNotification").Include(
                "~/Scripts/Admission/AdmissionNotification.js", "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdmissionCalendar").Include(
                "~/Scripts/Admission/AdmissionCalendar.js", "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/VerificationOfficer").Include(
                "~/Scripts/Admission/VerificationOfficer.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicantOnlineApplicationForm").Include(
                "~/Scripts/Admission/ApplicantOnlineApplicationForm.js",
                "~/Scripts/Admission/ApplicantOnlineApplicationFormCommon.js",
                 "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicantVerificationOfficer").Include(
                "~/Scripts/Admission/ApplicantVerificationOfficer.js",
                   "~/Scripts/Admission/ApplicantOnlineApplicationFormCommon.js",
                "~/Scripts/Admission/global.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/AdmissionMeritList").Include(
                "~/Scripts/Admission/AdmissionMeritList.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicationDetails").Include(
               "~/Scripts/Admission/ApplicationDetails.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicantAdmissionITIInstitute").Include(
                "~/Scripts/Admission/ApplicantAdmissionITIInstitute.js",
                "~/Scripts/Admission/ApplicantOnlineApplicationFormCommon.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicantAdmissionITIInstituteRVP").Include(
                "~/Scripts/Admission/ApplicantAdmissionITIInstituteRVP.js",
                "~/Scripts/Admission/ApplicantOnlineApplicationFormCommon.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/GrievanceTentativeList").Include(
                "~/Scripts/Admission/GrievanceAgainstTentativeList.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/affiliationMainJs").Include(
                    "~/Scripts/Affiliation/affiliation.main.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTable").Include(
                     "~/Content/css/jquery.dataTables.min.css",
                     "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Calendar").Include(
                    "~/Content/css/jquery-ui.min.css",
                    "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/SeatAvailability").Include(
                "~/Scripts/Admission/SeatAvailability.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/SeatAutoAllotment").Include(
                "~/Scripts/Admission/SeatAutoAllotment.js",
                "~/Scripts/Admission/global.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/AdmissionSeatMatrix").Include(
                "~/Scripts/Admission/AdmissionSeatMatrix.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/TransferAdmissionSeat").Include(
                "~/Scripts/Admission/TransferAdmissionSeat.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/ApplicantAdmissionAgainstVacancy").Include(
                "~/Scripts/Admission/ApplicantAdmissionAgainstVacancy.js",
                "~/Scripts/Admission/global.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminIndex").Include(
                "~/Scripts/Admin/AdminIndex.js",
                "~/Scripts/Admission/global.js"));

            //Download Excel
            bundles.Add(new ScriptBundle("~/bundles/DataTableExcelButtons").Include(
                        "~/Scripts/DataTableButtons/dataTables.buttons.min.js",
                        "~/Scripts/DataTableButtons/jszip.min.js",
                        "~/Scripts/DataTableButtons/buttons.html5.min.js"));

            //Download PDF
            //bundles.Add(new ScriptBundle("~/bundles/DataTablePDFButtons").Include(
            //           "~/Scripts/DataTableButtons/pdfmake.min.js",
            //               "~/Scripts/DataTableButtons/vfs_fonts.js"
            //));

            bundles.Add(new ScriptBundle("~/bundles/Staffdetails").Include(
                    "~/Scripts/Affiliation/Staffdetails.js"));

            bundles.Add(new ScriptBundle("~/bundles/StaffApprovalJs").Include(
                    "~/Scripts/Affiliation/StaffApproval.js"));

            bundles.Add(new ScriptBundle("~/bundles/StaffReportJs").Include(
                    "~/Scripts/Affiliation/StaffReport.js"));

            bundles.Add(new ScriptBundle("~/bundles/InstituteAffiliationJs").Include(
                  "~/Scripts/Affiliation/InstituteAffiliation.js"));
            #endregion

            #region Style css

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/frontend/css/bootstrap.css",            
                      "~/Content/frontend/css/font-awesome.min.css",            
                     "~/Content/frontend/css/nav-style.css",            
                     "~/Content/frontend/css/style.css",            
                     "~/Content/frontend/css/flexslider.css",            
                     "~/Content/frontend/css/simplelightbox.min.css",                                 
                     "~/Content/frontend/css/owl.carousel.css",
                     "~/Content/css/jquery-ui.min.css",
                     "~/Content/css/jquery.dataTables.min.css",
                     "~/Content/frontend/header/stellarnav.css"));

            bundles.Add(new StyleBundle("~/Styles/DataTableButtons").Include(
                      "~/Scripts/DataTableButtons/jquery.dataTables.min.css",
                      "~/Scripts/DataTableButtons/buttons.dataTables.min.css"));

            #endregion
        }
    }
}
