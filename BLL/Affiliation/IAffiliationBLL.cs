using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.Affiliation;

namespace BLL
{
    public interface IAffiliationBLL
    {
        UploadAffiliation UploadAffiliationDetailsBLL(DataTable dt, int UserId);

        List<AffiliationCollegeCourseType> GetCourseListBLL();

        List<AffiliationCollegeDistrict> GetDistrictListBLL(int divId);

        List<AffiliationCollegeDivision> GetDivisionListBLL();

        List<AffiliationCollegeTrades> GetTradesBLL();

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL(int noOfRec, int statusId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseBLL(int courseId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionBLL(int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(int courseId, int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(int courseId, int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictBLL(int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(int districtid, int tradeid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(int courseid, int districtid, int tradeid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeBLL(int tradeId);

        //ram add division district taluk consutancy and iti Institution filters for generic login

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictBLLFilter(int districtid, int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(int talukid, int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(int talukid, int districtid, int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseBLLFilter(int talukid, int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictBLLFilter(int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseBLLFilter(int talukid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionBLLFilter(int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeBLLFilter(int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLLFilter();

        //end



        List<SelectListItem> GetInstitutionTypesBLL();

        List<SelectListItem> GetLocationTypesBLL();

        List<SelectListItem> GetTalukBLL(int DistId);

        List<SelectListItem> GetConstiteuncyBLL();

        List<SelectListItem> GetPanchayatBLL(int TalukCode);

        List<SelectListItem> GetVillageBLL(int panchaId);

        AffiliationCollegeDetails GetAAffiliationCollegeDetailsBLL(int CollegeId);

        List<SelectListItem> GetDistrictsBLL();

        List<SelectListItem> GetCssCodeBLL();

        AffiliationCollegeDetails UpdateAffiliationCollegeDetailsBLL(AffiliationCollegeDetails Affi);

        AffiliationCollegeDetails AddAffiliationCollegeDetailsBLL(AffiliationCollegeDetails Affi);

        UploadAffiliation UploadMultipleAffiliationFilesBLL(int CollegeId, string fileName, string filePath);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL(int statusId);

        AffiliationCollegeDetails GetAffiliationCollegeDetailsBLL(int CollegeId,int flowid);

        string PublishAffiliatedCollegesBLL(int CollegeId);

        string ApproveAffiliatedCollegeBLL(int CollegeId);

        string RejectAffiliatedCollegeBLL(int CollegeId);

        AffiliationCollegeDetails GetATradeDetailsBLL(int Trade_Id, int role_id);

        List<SelectListItem> GetAllStatusBLL();

        List<SelectListItem> GetAllUsersBLL();

        List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL(int role_id);

        List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL1();       

        List<AffiliationCollegeDetails> GetAllMyAffiliatedCollegesBLL(int college_id);

        string AddTradeTransactionBLL(AffiliationTrade aTrade);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL();

        List<TradeHistory> GetAllTradeHistoriesBLL(int college_id);

        List<AffiliationCollegeDetails> GetAllUploadedAffiliationBLL();

        List<ActiveandDeactiveDeatils> GetAllActiveandDeactiveDeatilsBLL();

        List<GetAllActiveandDeactiveDeatilsforpopup> GetAllActiveandDeactiveDeatilsforpopupBLL(int id);

        bool CreateActiveDeactiveBLL(TradeActiveandDeactiveStatus model);

        List<TradeHistory> GetTradeHistoriesBLL(int Trade_id);

        Trade GetAffiliationTradeCodeBLL(int trade_id);

        List<SelectListItem> GetAffiliationSchemesBLL();

        AffiliationCollegeDetails GetAAffiliationUploadedCollegeDetailsBLL(int CollegeId);

        string DeleteUploadedAffiliationInstituteBLL(int College_Id_temp);

        int GetAffiliationInstituteIdBLL(int UserId);

        AffiliationCollegeDetailsTest UpdateAffiliationTradeDetailsBLL(AffiliationCollegeDetailsTest Affi);

        MisCodes FetchAffiliatedInstituteMISCodesBLL(string parm, int page);

        AffiliationCollegeDetailsTest AddNewAffiliatedInstituteTradeBLL(AffiliationCollegeDetailsTest Affi);

        AffiliationCollegeDetails GetAffiliatedInstituteDetailsBLL(int CollegeId);

        List<SelectListItem> GetAllAffiliatedInstituteByTalukBLL(int taluk);

        List<SelectListItem> GetAllAffiliatedInstituteByDistrictBLL(int dist);

        AffiliationCollegeDetails GetAAffiliationUploadedTradeDetailsBLL(int iti_trade_id);

        AffiliationCollegeDetailsTest SaveUploadedAffiliationTradeDetailsBLL(AffiliationCollegeDetailsTest Affi);

        bool IsAffiliatedTradeExistsBLL(int tradecode, int collegeId);

      
        AffiliationCollegeDetails GetATradeUnitwiseDetailsBLL(int ITI_Trade_ShiftId,int flowid);

        //Unitewise Trades Active 

        List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesUnitwiseBLL(int ITI_Trade_ShiftId);

        bool CreateActiveDeactiveUnitwiseBLL(Tradeactivedeactiveuintwsie model);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsBLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwiseADDDDeatilsBLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwiseViewDeatilsBLL();

        List<AffiliationCollegeDetails> GetAllActiveandDeactiveUnitsDeatilsAffBLL(int college_id);
         
        //Institute DeAffiliate

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteBLL();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSBLL();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDDBLL();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteApprovedRejectedBLL();

        List<AffiliationCollegeDetails> GetAllDeaffiliateInstituteAffBLL(int College_id);

        List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesInstBLL(int clg_id);

        bool InstituteDeaffiliateDeatilsBLL(TradeActiveandDeactiveStatus model);

        bool IsMisCodeExistsBLL(string miscode);

        bool IsITICollegeNameExistsBLL(string iticollegename);

        List<DeAffiliateInstitute> GetAffiliateInstituteDetails(int Dist);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD();

        List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLOnFilter();
        List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLOSOnFilter();
        List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLADOnFilter();
        List<DeAffiliateInstitute> GetAllDeaffiliateInstitutePOPUP();

        AffiliationCollegeDetailsTest AddAffiliationCollegeDetailsBLL1(AffiliationCollegeDetailsTest Affi1);

        AffiliationNested PublishActiveDeactiveTradeUnit(AffiliationNested model);
        AffiliationNested PublishAffiliateDeaffiliateInstitute(AffiliationNested model);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewPOPUP();

        ToPublishRecords PublishAffiliateInstitutesBLL(ToPublishRecords model);
        List<SelectListItem> GetAllDesignationDLL();
        

        List<SelectListItem> GetAllTeachingSubjectDLL();

        List<SelectListItem> GetAllTradesDLL();
        
        List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLL(int Year_id, int Course_Id, int Division_Id, int District_Id, int taluk_id, int Insttype_Id, int location_Id, int tradeId, int scheme_Id, string training_Id, int ReportType_Id);
        List<ActiveandDeactiveUnitsDeatils> GetAllDeaffiliateInstituteDLLForReport();
        List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForTrade();
        List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForUnits();
        List<SelectListItem> GetAllYearDLL();
        List<SelectListItem> GetAllInstitute(int DistId);
        List<SelectListItem> GetAllGender();
        AffiliationDocuments GetAllAffiliationDocForDownload(int collegeId, int? Trade_Id, int? shift_id, int? flag);

        #region staff module methods
        List<SelectListItem> GetAllStaffTypeDLL();
        string AddStaffDetail(StaffInstituteDetails staff, int loginId);

        List<StaffInstituteDetails> GetstaffDetails(int loginId = 0);
        List<StaffInstituteDetails> ListstaffDetails(int? userid=0,int? iniId=0);
        StaffInstituteDetails EditStaff(int id);
        StaffInstituteDetails ViewStaff(int id);
        bool UpdateStaff(StaffInstituteDetails staff, int loginId);
        bool DeleteStaff(int id,string session="");
        List<StaffInstituteDetails> GetstaffStatus(int loginId, int Year, int courseId, int Quarter1);
        List<StaffInstituteDetails> GetstaffStatusOSAD(int usrid, int Year, int courseId, int quarter);
        string ApproveStaff(List<StaffInstituteDetails> staff, int loginId);
        List<StaffInstituteDetails> GetAllstaffInstituteReport(int Year, int courseId, int divisionId,
            int districtId, int taluk, int Insttype, int location, int stafftype, int tradeId,
            int gender, int scheme, string training, int quarter);
        List<StaffInstituteDetails> ViewStaffhistory(int id, int session ,int quarter);
        string SubmitStaffDetails(List<StaffInstituteDetails> seat, int loginId, int roleId);
        List<StaffInstituteDetails> GetstaffStatusForCW();
        //List<StaffInstituteDetails> StaffDetailsView();
        List<int> GetStaffSubjectList(int id);
        List<int> GetStafTradeList(int id);
       
        List<StaffInstituteDetails> GetApprovedstaffInstitute(int loginId, int year, int courseId, int DivisionId,
            int DistrictId, int InstituteId, int quarter);
        List<StaffInstituteDetails> GetApprovedstaffInstituteDivisionWise(int UserId);

        int GetUserDivionIdBLL(int LoginId);
        #region staff details session wise
        List<StaffInstituteDetails> GetStaffDetailsSessionWise(int userId = 0, string session = "", int? iniId = 0);
        #endregion
        #endregion
    }
}
