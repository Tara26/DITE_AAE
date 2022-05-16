using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Affiliation
{
    public class ActiveandDeactiveDeatils
    {
        public string iti_college { get; set; }
        public string trades { get; set; }
        public int? units { get; set; }
        public int? status_id { get; set; }
        public int? iti_college_id { get; set; }
        public string FileUploadPath { get; set; }
        public int? Trade_ITI_id { get; set; }
        public bool IsActive { get; set; }
        public bool ActiveDeActive { get; set; }
    }

    public class GetAllActiveandDeactiveDeatilsforpopup
    {
        public string iti_college { get; set; }
        public string trades { get; set; }
        public int? units { get; set; }
        public int? status_id { get; set; }
        public int? iti_college_id { get; set; }
        public string FileUploadPath { get; set; }
        public int? Trade_ITI_id { get; set; }
        public bool IsActive { get; set; }
    }

    public class TradeActiveDeactiveUpdate
    {
        public int? Trade_ITI_id { get; set; }
        public string ActiveFileName { get; set; }
        public string ActiveFilePath { get; set; }
        public bool ActiveDeActive { get; set; }
    }

    public class TradeActiveDeactiveInsertintoHistory
    {
        public int? Trade_ITI_id { get; set; }
        public string FilPath { get; set; }
        public string FileName { get; set; }
        public int? ITI_Trade_Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? createdBy { get; set; }

    }

    public class Tradeactivedeactiveuintwsie
    {
        public int ITI_Trade_Shift_Active_Id { get; set; }
        public int ITI_Trade_Shift_Id { get; set; }
        public int ITI_Trade_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }

        public string IsPPP { get; set; }

        public string Dual_System { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovalFlowId { get; set; }
        public string ActDeActRemarks { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public int itiID { get; set; }

        public string status { get; set; }

        public string active { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
        // public string ImageFile { get; set; } 

        //public string fileName { get; set; }
        //public string filePath { get; set; }
        public HttpPostedFileBase ActiveImageFile { get; set; }
        public int userID { get; set; }

        public int Flowid { get; set; }

        public int StatusId { get; set; }

        public int? clgId { get; set; }

        public string status1 { get; set; }
        public string DeactivateOrderNo { get; set; }
        public Nullable<DateTime> DeactivateDate { get; set; }

        public Nullable<DateTime> ActivateDate { get; set; }
        public string ActivateOrderNo { get; set; }
        public string ActivateFileName { get; set; }
        public string ActivateFilePath { get; set; }
    }

    public class Tradeactivedeactiveuintwsiehistory
    {
        public int ITI_Trade_Shift_Active_Id { get; set; }
        public int ITI_Trade_Shift_Id { get; set; }
        public int ITI_Trade_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string IsPPP { get; set; }
        public string Dual_System { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovalFlowId { get; set; }
        public string ActDeActRemarks { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int itiID { get; set; }
        public string status { get; set; }
        public string active { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
        //public string ImageFile { get; set; }

        //public string fileName { get; set; }
        //public string filePath { get; set; }
        public int userID { get; set; }

        public int? Flowid { get; set; }
        public int? StatusId { get; set; }
        public int? clgId { get; set; }

        public string status1 { get; set; }

    }

    public class TradeActiveDeactiveUpdateandinsertUnitwise
    {
        public int? activeITIid { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int ITI_Trade_Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovalFlowId { get; set; }
        public string ActDeActRemarks { get; set; }
        public int ITI_Trade_Shift_Active_Id { get; set; }
        public int ITI_Trade_Shift_Id { get; set; }
        public int Units { get; set; }
        public int Shift { get; set; }
        public string DeactivateOrderNo { get; set; }
        public Nullable<DateTime> DeactivateDate { get; set; }

        public Nullable<DateTime> ActivateDate { get; set; }
        public string ActivateOrderNo { get; set; }
        public string ActivateFileName { get; set; }
        public string ActivateFilePath { get; set; }


    }

    public class DeAffiliateInstitute
    {
        public string iti_college { get; set; }
        public string trades { get; set; }
        public int? units { get; set; }
        public int? status_id { get; set; }
        public int? iti_college_id { get; set; }
        public string FileUploadPath { get; set; }
        public int? Trade_ITI_id { get; set; }
        public bool? IsActive { get; set; }
        public bool ActiveDeActive { get; set; }
        public bool RequestedAction { get; set; }
        public string StatusName { get; set; }
        public int? ApprovalFlowid { get; set; }
        public int? userId { get; set; }
        public string ActiveFilepath { get; set; }
        public int? ApprovalStatus { get; set; }
        public DateTime? createdon { get; set; }
        public bool? IsActive1 { get; set; }
        public string MisCode { get; set; }
        public int? Course_id { get; set; }
        public int? Division_id { get; set; }
        public int? District_id { get; set; }
        public string CourseType { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public int slno { get; set; }
        public string Clg_Adderss { get; set; }
        public string date { get; set; }
    }


    public class ActiveandDeactiveUnitsDeatils
    {
        public string iti_college { get; set; }
        public string trades { get; set; }
        public int? units { get; set; }
        public int? status_id { get; set; }
        public int? iti_college_id { get; set; }
        public string FileUploadPath { get; set; }
        public int? Trade_ITI_id { get; set; }
        public bool? IsActive { get; set; }
        public bool ActiveDeActive { get; set; }
        public bool RequestedAction { get; set; }
        public string StatusName { get; set; }
        public int? ApprovalFlowid { get; set; }
        public int? userId { get; set; }
        public string ActiveFilepath { get; set; }
        public int? ApprovalStatus { get; set; }
        public DateTime createdon { get; set; }
        public int? ITI_trade_id { get; set; }
        public int Shifts { get; set; }
        public int ITI_Trade_Shift_Id { get; set; }
        public int? StatusId { get; set; }
        public bool IsActive1 { get; set; }
        public string miscode { get; set; }
        public int? Courseid { get; set; }
        public int? Division_id { get; set; }
        public int? District_id { get; set; }
        public string CourseType { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public bool IsActiveTr { get; set; } 
        public string college_Address { get; set; }
        public string date { get; set; }
        public int slno { get; set; }
        public string LTdate { get; set; }
        public string Taluk { get; set; }
        public string Year { get; set; }
        public string InstituteType { get; set; }
        public string LocationType { get; set; }
        public string Scheme { get; set; }
        public string ModeofTraining { get; set; }
        public int? Taluk_id { get; set; }
        public int? Insttype_Id { get; set; }
        public int? location_Id { get; set; }
        public int? scheme_Id { get; set; }
        public string ReportType { get; set; }

        public DateTime? CreatedonOrderby { get; set; }

        public int New_Trade_Shift_Id { get; set; }

        public bool? ReqIsActive { get; set; }
        public int? InstCount { get; set; }
        public string NoOfUnits { get; set; }
    }

}
