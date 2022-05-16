using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Models;
using Models.Affiliation;
using Models.Master;

namespace DLL
{
    public interface IAffiliationDLL
    {
        UploadAffiliation UploadAffiliationDetailsDLL(DataTable dt, int UserId);

        List<AffiliationCollegeCourseType> GetCourseListDLL();

        List<AffiliationCollegeDivision> GetDivisionListDLL();

        List<AffiliationCollegeDistrict> GetDistrictListDLL(int divId);

        List<AffiliationCollegeTrades> GetTradesDLL();

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL(int noOfRec, int statusId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseDLL(int courseId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionDLL(int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseDLL(int courseId, int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseDLL(int courseId, int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictDLL(int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictDLL(int districtid, int tradeid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLL(int courseid, int districtid, int tradeid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDLL(int tradeId);

        //ram add division district taluk consutancy and iti Institution filters for generic login
        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictDLLFilter(int districtid, int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseDLLFilter(int talukid, int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLLFilter(int talukid, int districtid, int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseDLLFilter(int talukid, int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictDLLFilter(int districtid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseDLLFilter(int talukid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionDLLFilter(int divisionId);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDLLFilter(int constituencyid);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLLFilter();

        //end


        List<SelectListItem> GetInstitutionTypesDLL();

        List<SelectListItem> GetLocationTypesDLL();

        List<SelectListItem> GetTalukDLL(int DistId);

        List<SelectListItem> GetConstiteuncyDLL();

        List<SelectListItem> GetPanchayatDLL(int TalukCode);

        List<SelectListItem> GetVillageDLL(int panchaId);

        AffiliationCollegeDetails GetAAffiliationCollegeDetailsDLL(int CollegeId);

        List<SelectListItem> GetDistrictsDLL();

        List<SelectListItem> GetCssCodeDLL();

        AffiliationCollegeDetails UpdateAffiliationCollegeDetailsDLL(AffiliationCollegeDetails Affi);

        AffiliationCollegeDetails AddAffiliationCollegeDetailsDLL(AffiliationCollegeDetails Affi);

        UploadAffiliation UploadMultipleAffiliationFilesDLL(int CollegeId, string fileName, string filePath);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL(int statusId);

        AffiliationCollegeDetails GetAffiliationCollegeDetailsDLL(int CollegeId,int flowid);

        string PublishAffiliatedCollegesDLL(int CollegeId);

        string ApproveAffiliatedCollegeDLL(int CollegeId);

        string RejectAffiliatedCollegeDLL(int CollegeId);

        AffiliationCollegeDetails GetATradeDetailsDLL(int Trade_Id, int role_id);

        List<SelectListItem> GetAllStatusDLL();

        List<SelectListItem> GetAllUsersDLL();

        List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL(int role_id);
        List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL1();

        List<AffiliationCollegeDetails> GetAllMyAffiliatedCollegesDLL(int college_id);

        string AddTradeTransaction(AffiliationTrade aTrade);

        List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL();

        List<TradeHistory> GetAllTradeHistoriesDLL(int college_id);

        List<ActiveandDeactiveDeatils> GetAllActiveandDeactiveDeatilsDLL();

        List<GetAllActiveandDeactiveDeatilsforpopup> GetAllActiveandDeactiveDeatilsforpopupDLL(int id);
        TradeActiveandDeactiveStatus TradeActiveDeactiveUpdateandinsertInstwsieDLL(TradeActiveandDeactiveStatus TradeInsttId);

        bool CreateActiveDeactiveHistoryDLL(tbl_ITI_Trade_ActiveStatus_History model);
        bool UpdateTradeActiveDeactiveDLL(tbl_ITI_Trade model);
        tbl_ITI_Trade GetTradeDLL(int id);

        List<TradeHistory> GetTradeHistoriesDLL(int Trade_id);

        List<AffiliationCollegeDetails> GetAllUploadedAffiliationDLL();

        Trade GetAffiliationTradeCodeDLL(int trade_id);

        List<SelectListItem> GetAffiliationSchemesDLL();

        AffiliationCollegeDetails GetAAffiliationUploadedCollegeDetailsDLL(int CollegeId);

        string DeleteUploadedAffiliationInstitute(int College_Id_temp);

        int GetAffiliationInstituteIdDLL(int UserId);

        AffiliationCollegeDetailsTest UpdateAffiliationTradeDetails(AffiliationCollegeDetailsTest Affi);

        MisCodes FetchAffiliatedInstituteMISCodesDLL(string parm, int page);

        AffiliationCollegeDetailsTest AddNewAffiliatedInstituteTradeDLL(AffiliationCollegeDetailsTest Affi);

        AffiliationCollegeDetails GetAffiliatedInstituteDetailsDLL(int CollegeId);

        List<SelectListItem> GetAllAffiliatedInstituteByTalukDLL(int taluk);

        List<SelectListItem> GetAllAffiliatedInstituteByDistrictDLL(int district);

        AffiliationCollegeDetails GetAAffiliationUploadedTradeDetailsDLL(int iti_trade_id);

        AffiliationCollegeDetailsTest SaveUploadedAffiliationTradeDetails(AffiliationCollegeDetailsTest Affi);

        bool IsAffiliatedTradeExists(int tradecode, int collegeId);

        AffiliationCollegeDetails GetATradeUnitwiseDetailsDLL(int ITI_Trade_ShiftId, int flowid);

        List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesUintwiseDLL(int ITI_Trade_ShiftId);

        bool CreateActiveDeactiveUnitWiseHistoryDLL(tbl_ITI_Trade_Shift_ActiveStatus_History model);

        TradeActiveDeactiveUpdateandinsertUnitwise TradeActiveDeactiveUpdateandinsertUnitwsieDLL(TradeActiveDeactiveUpdateandinsertUnitwise TradeUintId);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsDLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsDLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieADDDDeatilsDLL();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewDeatilsDLL();

        List<AffiliationCollegeDetails> GetAllActiveandDeactiveUnitsDeatilsAffDLL(int college_id);

        //institute Deaffiliate

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteDLL();
         
        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSDLL();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDDDLL();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteApprovedRejectedDLL();

        List<AffiliationCollegeDetails> GetAllDeaffiliateInstituteAffDLL(int College_id);

        List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesInstDLL(int clg_id);

        tbl_ITI_Institute_ActiveStatus Get_tbl_ITI_Institute_ActiveStatusdll(int ITI_Institute_Id);
        List<tbl_Affiliation_documents> Get_tbl_Affiliation_documents(int itiID, int? TradeIds = null,
             int? Units = null, int? Shift = null);
        bool Save_tbl_ITI_Institute_ActiveStatusdll(tbl_ITI_Institute_ActiveStatus model);
        bool Save_tbl_Affiliation_documents(tbl_Affiliation_documents model, bool isUpdate);
        bool CreateDeaffiliateInstituteDLL(tbl_ITI_Institute_His_ActiveStatus model);

        bool IsMisCodeExists(string miscode);

        bool IsITICollegeNameExists(string miscode);

        List<DeAffiliateInstitute> GetAllInstituteDetails(int DistId);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsDLLOnFilter();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsDLLOnFilter();

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieADDDDeatilsDLLOnFilter();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteDLLCwFilter();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSDLLOnSearch();

        List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDLLOnsearch();
        List<DeAffiliateInstitute> GetAllDeaffiliateInstitutePOPUP();

        AffiliationCollegeDetailsTest AddAffiliationCollegeDetailsDLL1(AffiliationCollegeDetailsTest Affi1);

        List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewPOPUP();

        AffiliationNested PublishActiveDeactiveTradeUnit(AffiliationNested model);
        AffiliationNested PublishAffiliateDeaffiliateInstitute(AffiliationNested model);

        ToPublishRecords PublishAffiliateInstitutes(ToPublishRecords model);

       
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
        #region staff module methods
        List<SelectListItem> GetAllStaffTypeDLL();

        string AddStaffDetail(StaffInstituteDetails staff, int loginId);
        List<StaffInstituteDetails> GetstaffDetails(int loginId = 0);
        List<StaffInstituteDetails> ListstaffDetails(int? userid,int? iniId);

        StaffInstituteDetails EditStaff(int id);
        StaffInstituteDetails ViewStaff(int id);
        bool Updatestaff(StaffInstituteDetails staff, int loginId);
        string ApproveStaff(List<StaffInstituteDetails> staff, int loginId);

        bool DeleteStaff(int id, string session = "");

        List<StaffInstituteDetails> GetstaffStatus(int loginId, int Year, int courseId, int Quarter1);
        List<StaffInstituteDetails> GetstaffStatusOSAD(int usrid, int Year, int courseId, int quarter);
        List<StaffInstituteDetails> GetAllstaffInstituteReport(int Year, int courseId, int divisionId,
            int districtId, int taluk, int Insttype, int location, int stafftype, int tradeId,
            int gender, int scheme, string training, int quarter);
        List<StaffInstituteDetails> ViewStaffhistory(int id, int session,int quarter);

        string SubmitStaffDetails(List<StaffInstituteDetails> staff, int loginId, int roleId);
        List<StaffInstituteDetails> GetstaffStatusForCW();
        //List<StaffInstituteDetails> StaffDetailsView();
        List<int> GetStaffSubjectList(int id);
        List<int> GetStafTradeList(int id);
        List<StaffInstituteDetails> GetApprovedstaffInstitute(int loginId, int year, int courseId, int DivisionId,
            int DistrictId, int InstituteId, int quarter);
        List<StaffInstituteDetails> GetApprovedstaffInstituteDivisionWise(int UserId);
        int GetUserDivionIdDLL(int LoginId);
        AffiliationDocuments GetAllAffiliationDocForDownload(int collegeId, int? Trade_Id, int? shift_id, int? flag);
        #region staff details session wise
        List<StaffInstituteDetails> GetStaffDetailsSessionWise(int userId = 0, string session = "", int? iniId = 0);
        #endregion
        #endregion
    }
}
