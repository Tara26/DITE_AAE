using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DLL;
using Models;
using Models.Affiliation;
using Models.Master;

namespace BLL
{
    public class AffiliationBLL : IAffiliationBLL
    {
        private readonly IAffiliationDLL _affiliationDll;

        public AffiliationBLL()
        {
            _affiliationDll = new AffiliationDLL();
        }

        public UploadAffiliation UploadAffiliationDetailsBLL(DataTable dt, int UserId)
        {
            return _affiliationDll.UploadAffiliationDetailsDLL(dt, UserId);
        }

        public List<AffiliationCollegeCourseType> GetCourseListBLL()
        {
            return _affiliationDll.GetCourseListDLL();
        }

        public List<AffiliationCollegeDistrict> GetDistrictListBLL(int divId)
        {
            return _affiliationDll.GetDistrictListDLL(divId);
        }

        public List<AffiliationCollegeDivision> GetDivisionListBLL()
        {
            return _affiliationDll.GetDivisionListDLL();
        }

        public List<AffiliationCollegeTrades> GetTradesBLL()
        {
            return _affiliationDll.GetTradesDLL();
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL(int noOfRec, int statusId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsDLL(noOfRec, statusId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseBLL(int courseId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByCourseDLL(courseId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionBLL(int divisionId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDivisionDLL(divisionId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseBLL(int courseId, int divisionId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDivisionAndCourseDLL(courseId, divisionId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseBLL(int courseId, int districtid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDistrictAndCourseDLL(courseId, districtid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictBLL(int districtid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDistrictDLL(districtid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictBLL(int districtid, int tradeid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeAndDistrictDLL(districtid, tradeid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLL(int courseid, int districtid, int tradeid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLL(courseid, districtid, tradeid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeBLL(int tradeId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeDLL(tradeId);
        }

        //ram add division district taluk consutancy and iti Institution filters for generic login
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictBLLFilter(int districtid, int constituencyid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeAndDistrictDLLFilter(districtid, constituencyid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseBLLFilter(int talukid, int districtid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDistrictAndCourseDLLFilter(talukid, districtid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseBLLFilter(int talukid, int districtid, int constituencyid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLLFilter(talukid, districtid, constituencyid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseBLLFilter(int talukid, int divisionId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDivisionAndCourseDLLFilter(talukid, divisionId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictBLLFilter(int districtid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDistrictDLLFilter(districtid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseBLLFilter(int talukid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByCourseDLLFilter(talukid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionBLLFilter(int divisionId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByDivisionDLLFilter(divisionId);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeBLLFilter(int constituencyid)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsByTradeDLLFilter(constituencyid);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLLFilter()
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsDLLFilter();
        }

        //end


        public List<SelectListItem> GetInstitutionTypesBLL()
        {
            return _affiliationDll.GetInstitutionTypesDLL();
        }

        public List<SelectListItem> GetLocationTypesBLL()
        {
            return _affiliationDll.GetLocationTypesDLL();
        }

        public List<SelectListItem> GetTalukBLL(int DistId)
        {
            return _affiliationDll.GetTalukDLL(DistId);
        }

        public List<SelectListItem> GetConstiteuncyBLL()
        {
            return _affiliationDll.GetConstiteuncyDLL();
        }

        public List<SelectListItem> GetPanchayatBLL(int TalukCode)
        {
            return _affiliationDll.GetPanchayatDLL(TalukCode);
        }

        public List<SelectListItem> GetVillageBLL(int panchaId)
        {
            return _affiliationDll.GetVillageDLL(panchaId);
        }

        public AffiliationCollegeDetails GetAAffiliationCollegeDetailsBLL(int CollegeId)
        {
            return _affiliationDll.GetAAffiliationCollegeDetailsDLL(CollegeId);
        }

        public List<SelectListItem> GetDistrictsBLL()
        {
            return _affiliationDll.GetDistrictsDLL();
        }

        public List<SelectListItem> GetCssCodeBLL()
        {
            return _affiliationDll.GetCssCodeDLL();
        }

        public AffiliationCollegeDetails UpdateAffiliationCollegeDetailsBLL(AffiliationCollegeDetails Affi)
        {
            return _affiliationDll.UpdateAffiliationCollegeDetailsDLL(Affi);
        }

        public AffiliationCollegeDetails AddAffiliationCollegeDetailsBLL(AffiliationCollegeDetails Affi)
        {
            return _affiliationDll.AddAffiliationCollegeDetailsDLL(Affi);
        }

        public UploadAffiliation UploadMultipleAffiliationFilesBLL(int CollegeIds, string fileName, string filePath)
        {
            return _affiliationDll.UploadMultipleAffiliationFilesDLL(CollegeIds, fileName, filePath);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL(int statusId)
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsDLL(statusId);
        }

        public AffiliationCollegeDetails GetAffiliationCollegeDetailsBLL(int CollegeId,int flowid)
        {
            return _affiliationDll.GetAffiliationCollegeDetailsDLL(CollegeId, flowid);
        }

        public string PublishAffiliatedCollegesBLL(int CollegeId)
        {
            return _affiliationDll.PublishAffiliatedCollegesDLL(CollegeId);
        }

        public string ApproveAffiliatedCollegeBLL(int CollegeId)
        {
            return _affiliationDll.ApproveAffiliatedCollegeDLL(CollegeId);
        }

        public string RejectAffiliatedCollegeBLL(int CollegeId)
        {
            return _affiliationDll.RejectAffiliatedCollegeDLL(CollegeId);
        }

        public AffiliationCollegeDetails GetATradeDetailsBLL(int Trade_Id, int role_id)
        {
            return _affiliationDll.GetATradeDetailsDLL(Trade_Id, role_id);
        }

        public List<SelectListItem> GetAllStatusBLL()
        {
            return _affiliationDll.GetAllStatusDLL();
        }

        public List<SelectListItem> GetAllUsersBLL()
        {
            return _affiliationDll.GetAllUsersDLL();
        }

        public List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL(int role_id)
        {
            return _affiliationDll.GetAllAffiliateCollegeListDLL(role_id);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL1()
        {
            return _affiliationDll.GetAllAffiliateCollegeListDLL1();
        }

        public List<AffiliationCollegeDetails> GetAllMyAffiliatedCollegesBLL(int college_id)
        {
            return _affiliationDll.GetAllMyAffiliatedCollegesDLL(college_id);
        }

        public string AddTradeTransactionBLL(AffiliationTrade aTrade)
        {
            return _affiliationDll.AddTradeTransaction(aTrade);
        }

        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsBLL()
        {
            return _affiliationDll.GetAllAffiliationCollegeDetailsDLL();
        }

        public List<TradeHistory> GetAllTradeHistoriesBLL(int college_id)
        {
            return _affiliationDll.GetAllTradeHistoriesDLL(college_id);
        }

        public List<AffiliationCollegeDetails> GetAllUploadedAffiliationBLL()
        {
            return _affiliationDll.GetAllUploadedAffiliationDLL();
        }

        public List<ActiveandDeactiveDeatils> GetAllActiveandDeactiveDeatilsBLL()
        {
            return _affiliationDll.GetAllActiveandDeactiveDeatilsDLL();
        }

        public List<GetAllActiveandDeactiveDeatilsforpopup> GetAllActiveandDeactiveDeatilsforpopupBLL(int id)
        {
            return _affiliationDll.GetAllActiveandDeactiveDeatilsforpopupDLL(id);
        }

        public bool CreateActiveDeactiveBLL(TradeActiveandDeactiveStatus model)
        {
            tbl_ITI_Trade_ActiveStatus_History tbl_ITI_Trade_ActiveStatus_Histor = new tbl_ITI_Trade_ActiveStatus_History()
            {
                FilePath = model.filePath,
                FileName = model.fileName,
                ITI_Trade_Id = model.itiID,
                IsActive = true,
                CreatedOn = DateTime.Now,
                CreatedBy = model.userID,
            };
            bool status = _affiliationDll.CreateActiveDeactiveHistoryDLL(tbl_ITI_Trade_ActiveStatus_Histor);

            tbl_ITI_Trade tbl_ITI_Trade = _affiliationDll.GetTradeDLL(model.itiID);

            tbl_ITI_Trade.ActiveDeActive = model.status == "Active" ? true : false;
            tbl_ITI_Trade.ActivrFilePath = model.filePath;
            tbl_ITI_Trade.ActiveFileName = model.fileName;

            status = _affiliationDll.UpdateTradeActiveDeactiveDLL(tbl_ITI_Trade);

            return status;
        }

        public List<TradeHistory> GetTradeHistoriesBLL(int Trade_id)
        {
            return _affiliationDll.GetTradeHistoriesDLL(Trade_id);
        }

        public Trade GetAffiliationTradeCodeBLL(int trade_id)
        {
            return _affiliationDll.GetAffiliationTradeCodeDLL(trade_id);
        }

        public List<SelectListItem> GetAffiliationSchemesBLL()
        {
            return _affiliationDll.GetAffiliationSchemesDLL();
        }

        public AffiliationCollegeDetails GetAAffiliationUploadedCollegeDetailsBLL(int CollegeId)
        {
            return _affiliationDll.GetAAffiliationUploadedCollegeDetailsDLL(CollegeId);
        }

        public string DeleteUploadedAffiliationInstituteBLL(int College_Id_temp)
        {
            return _affiliationDll.DeleteUploadedAffiliationInstitute(College_Id_temp);
        }

        public int GetAffiliationInstituteIdBLL(int UserId)
        {
            return _affiliationDll.GetAffiliationInstituteIdDLL(UserId);
        }

        public AffiliationCollegeDetailsTest UpdateAffiliationTradeDetailsBLL(AffiliationCollegeDetailsTest Affi)
        {
            return _affiliationDll.UpdateAffiliationTradeDetails(Affi);
        }

        public MisCodes FetchAffiliatedInstituteMISCodesBLL(string parm, int page)
        {
            return _affiliationDll.FetchAffiliatedInstituteMISCodesDLL(parm, page);
        }

        public AffiliationCollegeDetailsTest AddNewAffiliatedInstituteTradeBLL(AffiliationCollegeDetailsTest Affi)
        {
            return _affiliationDll.AddNewAffiliatedInstituteTradeDLL(Affi);
        }

        public AffiliationCollegeDetails GetAffiliatedInstituteDetailsBLL(int CollegeId)
        {
            return _affiliationDll.GetAffiliatedInstituteDetailsDLL(CollegeId);
        }

        public List<SelectListItem> GetAllAffiliatedInstituteByTalukBLL(int taluk)
        {
            return _affiliationDll.GetAllAffiliatedInstituteByTalukDLL(taluk);
        }

        public List<SelectListItem> GetAllAffiliatedInstituteByDistrictBLL(int district)
        {
            return _affiliationDll.GetAllAffiliatedInstituteByDistrictDLL(district);
        }

        public AffiliationCollegeDetails GetAAffiliationUploadedTradeDetailsBLL(int iti_trade_id)
        {
            return _affiliationDll.GetAAffiliationUploadedTradeDetailsDLL(iti_trade_id);
        }

        public AffiliationCollegeDetailsTest SaveUploadedAffiliationTradeDetailsBLL(AffiliationCollegeDetailsTest Affi)
        {
            return _affiliationDll.SaveUploadedAffiliationTradeDetails(Affi);
        }

        public bool IsAffiliatedTradeExistsBLL(int tradecode, int collegeId)
        {
            return _affiliationDll.IsAffiliatedTradeExists(tradecode, collegeId);
        }
        public AffiliationCollegeDetails GetATradeUnitwiseDetailsBLL(int ITI_Trade_ShiftId,int flowid)
        {
            return _affiliationDll.GetATradeUnitwiseDetailsDLL(ITI_Trade_ShiftId, flowid);
        }

        public List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesUnitwiseBLL(int ITI_Trade_ShiftId)
        {
            return _affiliationDll.ActiveDeactiveGetTradeHistoriesUintwiseDLL(ITI_Trade_ShiftId);
        }

        public bool CreateActiveDeactiveUnitwiseBLL(Tradeactivedeactiveuintwsie model)
        {
            if (model.userID == (int)CsystemType.getCommon.CaseWorker )
            {
                //model.ApprovalFlowId = 7;
                model.ApprovalStatus = 8;
            }
            if(model.userID == (int)CsystemType.getCommon.ITIAdmin)
            {
                model.ApprovalFlowId = 8;
                model.ApprovalStatus = 4;
            }
                tbl_ITI_Trade_Shift_ActiveStatus_History tbl_ITI_Trade_Shift_ActiveStatus_History = new tbl_ITI_Trade_Shift_ActiveStatus_History()
            {
                //   if (model.FileName != "null" && model.FilePath != null)
                // {
                // FileName = model.FileName,
                // FilePath = model.FilePath,
                //}
                // if (model.FileName != "null" && model.FileName != null)
                //{
                //FilePath = model.FilePath,
                //FileName = model.FileName,

                ITI_Trade_Shift_Active_Id = model.ITI_Trade_Shift_Active_Id,
                ITI_Trade_Shift_Id = model.ITI_Trade_Shift_Id,
                ITI_Trade_Id = model.ITI_Trade_Id,
                // ITI_Trade_Id = model.ITI_Trade_Id,
                Units = model.Units,
                Shift = model.Shift,
                IsActive = model.IsActive,
                //  IsActive = false,
                // IsActive =model.status,
                CreatedOn = DateTime.Now,
                // CreatedBy = model.userID,
                CreatedBy = model.userID,
              
                ApprovalFlowId = model.ApprovalFlowId,
                ActDeActRemarks = model.ActDeActRemarks,
                // ApprovalStatus = model.StatusId
                ApprovalStatus = model.ApprovalStatus
            };
            if (model.FileName != "null" && model.FilePath != null)
            {
                tbl_ITI_Trade_Shift_ActiveStatus_History.FileName = model.FileName;
                tbl_ITI_Trade_Shift_ActiveStatus_History.FilePath = model.FilePath;
            }

            bool status = _affiliationDll.CreateActiveDeactiveUnitWiseHistoryDLL(tbl_ITI_Trade_Shift_ActiveStatus_History);

            //tbl_ITI_Trade tbl_ITI_Trade = _affiliationDll.GetTradeDLL(model.itiID);

            //tbl_ITI_Trade.ActiveDeActive = model.status == "Active" ? false : true;
            //if (model.filePath != "null" && model.filePath != null)
            //{
            //    tbl_ITI_Trade.ActivrFilePath = model.filePath;
            //    tbl_ITI_Trade.ActiveFileName = model.fileName;
            //}
            //tbl_ITI_Trade.ActDeActRemarks = model.ActDeActRemarks;

            //status = _affiliationDll.UpdateTradeActiveDeactiveDLL(tbl_ITI_Trade);

            TradeActiveDeactiveUpdateandinsertUnitwise tradeactive = new TradeActiveDeactiveUpdateandinsertUnitwise();
            if (model.FileName != "null" && model.FilePath != null)
            {
                tradeactive.FileName = model.FileName;
                tradeactive.FilePath = model.FilePath;
            }
            if (model.ActivateFileName != "null" && model.ActivateFilePath != null)
            {
                tradeactive.ActivateFileName = model.ActivateFileName;
                tradeactive.ActivateFilePath = model.ActivateFilePath;
                //tradeactive.ActivateOrderNo = model.ActivateOrderNo;
            }
            if (model.ActivateOrderNo != null )
            {
                tradeactive.ActivateOrderNo = model.ActivateOrderNo;
            }
            if (model.DeactivateOrderNo != null )
            {
                tradeactive.DeactivateOrderNo = model.DeactivateOrderNo;
            }
            if (model.ActivateDate != null)
            {
                tradeactive.ActivateDate = model.ActivateDate;
            }
            if (model.DeactivateDate != null)
            {
                tradeactive.DeactivateDate = model.DeactivateDate;
            }
            tradeactive.ITI_Trade_Shift_Id = model.ITI_Trade_Shift_Id;
            // tradeactive.ITI_Trade_Id = model.itiID;
            tradeactive.ITI_Trade_Id = model.ITI_Trade_Id;
            tradeactive.Units = model.Units;
            tradeactive.Shift = model.Shift;
            //tradeactive.CreatedBy = model.userID;
            tradeactive.CreatedBy = model.userID;
            tradeactive.CreatedOn = DateTime.Now;
            tradeactive.IsActive = model.IsActive;
            tradeactive.activeITIid = model.itiID;
            if (model.userID == (int)CsystemType.getCommon.CaseWorker && model.ApprovalFlowId == 7)
            //if (model.userID == 13)
            // if (model.CreatedBy == 13)
            {
                // tradeactive.ApprovalStatus = model.status == "Active" ? 0 : 1;
                tradeactive.ApprovalStatus = 8;
                tradeactive.ApprovalFlowId = 7;
            }
            else if (model.userID == (int)CsystemType.getCommon.CaseWorker && model.ApprovalFlowId == 3)
            {
                tradeactive.ApprovalStatus = 8;
                tradeactive.ApprovalFlowId = 3;
            }
            else if(model.userID == (int)CsystemType.getCommon.CaseWorker && model.ApprovalFlowId == 5)
            {
                tradeactive.ApprovalStatus = 8;
                tradeactive.ApprovalFlowId = 5;
            }
            else if(model.userID == (int)CsystemType.getCommon.ITIAdmin)
            {
                tradeactive.ApprovalStatus = 4;
                tradeactive.ApprovalFlowId = 8;
            }
            else
            {
                //tradeactive.ApprovalStatus = model.StatusId;
                //tradeactive.ApprovalFlowId = model.Flowid;
                tradeactive.ApprovalStatus = model.ApprovalStatus;
                tradeactive.ApprovalFlowId = model.ApprovalFlowId;

            }
            tradeactive.ActDeActRemarks = model.ActDeActRemarks;

            _affiliationDll.TradeActiveDeactiveUpdateandinsertUnitwsieDLL(tradeactive);

            return status;
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLL()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitsDeatilsDLL();
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsBLL()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieOSDeatilsDLL();
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwiseADDDDeatilsBLL()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieADDDDeatilsDLL();
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwiseViewDeatilsBLL()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieViewDeatilsDLL();
        }

        public List<AffiliationCollegeDetails> GetAllActiveandDeactiveUnitsDeatilsAffBLL(int college_id)
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitsDeatilsAffDLL(college_id);
        }

        //Institute Deaffiliate

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteBLL() 
        {
            return _affiliationDll.GetAllDeaffiliateInstituteDLL();
        }

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSBLL()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteOSDLL();
        }

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDDBLL()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteADDDDLL();
        }

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteApprovedRejectedBLL()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteApprovedRejectedDLL();
        }

        public List<AffiliationCollegeDetails> GetAllDeaffiliateInstituteAffBLL(int College_id) 
        {
            return _affiliationDll.GetAllDeaffiliateInstituteAffDLL(College_id);
        }

        public List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesInstBLL(int clg_id)
        {
            return _affiliationDll.ActiveDeactiveGetTradeHistoriesInstDLL(clg_id);
        }

        public bool InstituteDeaffiliateDeatilsBLL(TradeActiveandDeactiveStatus model)
        {
            bool status = false;
            #region commented code
            //tbl_ITI_Institute_His_ActiveStatus tbl_ITI_Institute_His_ActiveStatus = new tbl_ITI_Institute_His_ActiveStatus()
            //{
            //    FilePath = model.filePath,
            //    FileName = model.fileName,
            //    ITI_Trade_Id = model.itiID,
            //    IsActive = true,
            //    CreatedOn = DateTime.Now,
            //    CreatedBy = model.userID,
            //    ApprovalFlowId = model.Flowid,
            //    ActDeActRemarks = model.ActDeActRemarks,
            //    ApprovalStatus = model.StatusId,
            //    ITI_Institute_Id = model.clgId


            //};
            //bool status = _affiliationDll.CreateDeaffiliateInstituteDLL(tbl_ITI_Institute_His_ActiveStatus);
            //sat dont need
            //tbl_ITI_Trade tbl_ITI_Trade = _affiliationDll.GetTradeDLL(model.itiID);

            //tbl_ITI_Trade.ActiveDeActive = model.status == "Active" ? false : true;
            //if (model.filePath != "null" && model.filePath != null)
            //{
            //    tbl_ITI_Trade.ActivrFilePath = model.filePath;
            //    tbl_ITI_Trade.ActiveFileName = model.fileName;
            //}
            //tbl_ITI_Trade.ActDeActRemarks = model.ActDeActRemarks;

            //status = _affiliationDll.UpdateTradeActiveDeactiveDLL(tbl_ITI_Trade);
            #endregion
            int clgID = model.clgId != null ? int.Parse(model.clgId.ToString()) : 0;

            tbl_ITI_Institute_ActiveStatus tbl_ITI_Institute_ActiveStatus = _affiliationDll.Get_tbl_ITI_Institute_ActiveStatusdll(clgID);

            if(model.userID==8)
            {
                model.StatusId =8; 
            } 
            else if(model.userID == 9)
            {
                model.StatusId = 4;
                model.Flowid = 8;
            }
            else
            {
                model.StatusId = model.StatusId;
            }
            if (tbl_ITI_Institute_ActiveStatus == null)
            {
                tbl_ITI_Institute_ActiveStatus _ITI_Institute_ActiveStatus = new tbl_ITI_Institute_ActiveStatus()
                {
                    FilePath = model.filePath,
                    FileName = model.fileName,
                    ITI_Institute_Id = clgID,
                    //IsActive = true,
                    IsActive = model.IsActive,
                    ActDeActRemarks = model.ActDeActRemarks,
                    CreatedBy = model.userID,
                    CreatedOn = DateTime.Now,
                    ApprovalStatus = model.StatusId != null ? int.Parse(model.StatusId.ToString()) : 0,
                    ApprovalFlowId = model.Flowid != null ? int.Parse(model.Flowid.ToString()) : 0,
                    AffiliateFileName=model.AffiliateFileName,
                    AffiliateFilePath=model.AffiliateFilePath,

                };

                status = _affiliationDll.Save_tbl_ITI_Institute_ActiveStatusdll(_ITI_Institute_ActiveStatus);
            }
            else
            {
                if (model.filePath != null && model.filePath != "")
                {
                    tbl_ITI_Institute_ActiveStatus.FilePath = model.filePath;
                    tbl_ITI_Institute_ActiveStatus.FileName = model.fileName;
                }
                
                if (model.AffiliateFilePath != null && model.AffiliateFilePath != "")
                {
                    tbl_ITI_Institute_ActiveStatus.AffiliateFilePath = model.AffiliateFilePath;
                    tbl_ITI_Institute_ActiveStatus.AffiliateFileName = model.AffiliateFileName;
                }
               
                tbl_ITI_Institute_ActiveStatus.ITI_Institute_Id = clgID;
                //tbl_ITI_Institute_ActiveStatus.IsActive = true;
                tbl_ITI_Institute_ActiveStatus.IsActive = model.IsActive;
                tbl_ITI_Institute_ActiveStatus.ActDeActRemarks = model.ActDeActRemarks;
                tbl_ITI_Institute_ActiveStatus.CreatedBy = model.userID;
                tbl_ITI_Institute_ActiveStatus.CreatedOn = DateTime.Now;//model.CreatedOn;
                tbl_ITI_Institute_ActiveStatus.ApprovalStatus = model.StatusId != null ? int.Parse(model.StatusId.ToString()) : 0;
                tbl_ITI_Institute_ActiveStatus.ApprovalFlowId = model.Flowid != null ? int.Parse(model.Flowid.ToString()) : 0;



                status = _affiliationDll.Save_tbl_ITI_Institute_ActiveStatusdll(tbl_ITI_Institute_ActiveStatus);
                _affiliationDll.TradeActiveDeactiveUpdateandinsertInstwsieDLL(model);

            }

            tbl_ITI_Institute_ActiveStatus tbl_ITI_Institute_ActiveStatus1 = _affiliationDll.Get_tbl_ITI_Institute_ActiveStatusdll(clgID);

            tbl_ITI_Institute_His_ActiveStatus tbl_ITI_ = new tbl_ITI_Institute_His_ActiveStatus()
            {
                ActiveITIId = tbl_ITI_Institute_ActiveStatus1.ActiveITIId,
                FilePath = model.filePath,
                FileName = model.fileName,
                ITI_Institute_Id = clgID,
                //IsActive = true,
                IsActive = model.IsActive,
                ApprovalStatus = model.StatusId != null ? int.Parse(model.StatusId.ToString()) : 0,
                ApprovalFlowId = model.Flowid != null ? int.Parse(model.Flowid.ToString()) : 0,
                ActDeActRemarks = model.ActDeActRemarks,
                CreatedOn = DateTime.Now,
                CreatedBy = model.userID,
            };
            status = _affiliationDll.CreateDeaffiliateInstituteDLL(tbl_ITI_);
            // Insert or update only for case worker
            if(model.userID==8)
            { 
            bool isAffiliate = false;
            bool isUpdate = false;
            //if we want go with only institute level, trade, unit, shift must be null
            var updatedoc = _affiliationDll.Get_tbl_Affiliation_documents(itiID: Convert.ToInt32(model.clgId)).LastOrDefault();          
           
            if ((model.DeffiliateDate != null || model.DeaffiliateOrderNo != null || model.filePath != null) 
                && model.IsActive == false)
            {
                if (updatedoc != null && updatedoc.Institute_id == model.clgId && updatedoc.Flag == (int)CsystemType.getCommon.Deaffiliate)
                {
                    isUpdate = true;
                }
            }
            else if ((model.AffiliateDate != null || model.AffiliateOrderNo != null || model.AffiliateFilePath != null) 
                && model.IsActive == true)
            {
                isAffiliate = true;
                if (updatedoc != null && updatedoc.Institute_id == model.clgId && updatedoc.Flag == (int)CsystemType.getCommon.Affiliate)
                {
                    isUpdate = true;
                }
            }
            tbl_Affiliation_documents Trade_Doc;
            if (isUpdate)
                Trade_Doc = updatedoc;            
            else
                Trade_Doc = new tbl_Affiliation_documents();
            if (Trade_Doc != null /*&& isUpdate==true*/)
            {
                AffiliateorDeaffiliate(model, ref Trade_Doc, isAffiliate, isUpdate);
                    if ((model.DeffiliateDate != null || model.DeaffiliateOrderNo != null || model.filePath != null)
                && model.IsActive == false)
                    {
                        _affiliationDll.Save_tbl_Affiliation_documents(Trade_Doc, isUpdate);
                    }
                    else if ((model.AffiliateDate != null || model.AffiliateOrderNo != null || model.AffiliateFilePath != null)
               && model.IsActive == true)
                    {
                        _affiliationDll.Save_tbl_Affiliation_documents(Trade_Doc, isUpdate);
                    }
            }
            }
            #region commented code
            //if ((model.DeffiliateDate != null || model.DeaffiliateOrderNo != null || model.filePath != null) && model.IsActive == false)
            //{
            //    var updatedoc = _db.tbl_Affiliation_documents.Where(a => a.Institute_id == model.itiID).FirstOrDefault();
            //    if (updatedoc != null)
            //    {
            //        if (updatedoc.Institute_id == model.itiID && updatedoc.Flag == (int)CsystemType.getCommon.Deaffiliate)
            //        {
            //            isUpdate = true;
           
            //            //updatedoc.Institute_id = model.itiID;
            //            //if (model.filePath != null)
            //            //{
            //            //    updatedoc.FileName = model.filePath;
            //            //}
           
            //            //updatedoc.IsActive = true;
            //            //updatedoc.Status = "Deaffiliate";
            //            //updatedoc.Flag = (int)CsystemType.getCommon.Deactivate;
            //            //if (updatedoc.AffiliationOrder_Number != model.DeaffiliateOrderNo)
            //            //{
            //            //    updatedoc.AffiliationOrder_Number = model.DeaffiliateOrderNo;
            //            //}

            //            //if (updatedoc.Affiliation_date != model.DeffiliateDate)
            //            //{
            //            //    updatedoc.Affiliation_date = model.DeffiliateDate;
            //            //}
            //            //_db.SaveChanges();
            //        }
            //        else
            //        {
            //            //var Trade_Doc = new tbl_Affiliation_documents();

            //            //Trade_Doc.Institute_id = model.itiID;
            //            //Trade_Doc.FileName = model.filePath;
            //            //Trade_Doc.IsActive = true;
            //            //Trade_Doc.Status = "Deaffiliate";
            //            //Trade_Doc.Flag = (int)CsystemType.getCommon.Deaffiliate;
            //            //Trade_Doc.AffiliationOrder_Number = model.DeaffiliateOrderNo;
            //            //Trade_Doc.Affiliation_date = model.DeffiliateDate;
            //            //_db.tbl_Affiliation_documents.Add(Trade_Doc);
            //            //_db.SaveChanges();
            //        }
            //    }
            //    else
            //    {
            //        //var Trade_Doc = new tbl_Affiliation_documents();

            //        //Trade_Doc.Institute_id = model.itiID;
            //        //Trade_Doc.FileName = model.filePath;
            //        //Trade_Doc.IsActive = true;
            //        //Trade_Doc.Status = "Deaffiliate";
            //        //Trade_Doc.Flag = (int)CsystemType.getCommon.Deaffiliate;
            //        //Trade_Doc.AffiliationOrder_Number = model.DeaffiliateOrderNo;
            //        //Trade_Doc.Affiliation_date = model.DeffiliateDate;
            //        //_db.tbl_Affiliation_documents.Add(Trade_Doc);
            //        //_db.SaveChanges();
            //    }

            //}
            //else
            //if ((model.AffiliateDate != null || model.AffiliateOrderNo != null || model.AffiliateFilePath != null) && model.IsActive == true)
            //{
            //    isAffiliate = true;
            //    var updateact = _db.tbl_Affiliation_documents.Where(a => a.Institute_id == model.itiID).ToList().Last();
            //    if (updateact != null)
            //    {
            //        if (updateact.Institute_id == model.itiID && updateact.Flag == (int)CsystemType.getCommon.Affiliate)
            //        {

            //            isUpdate = true;
            //            //updateact.Institute_id = model.itiID;
            //            //if (model.AffiliateFilePath != null)
            //            //{
            //            //    updateact.FileName = model.AffiliateFilePath;
            //            //}
            //            //updateact.IsActive = true;
            //            //updateact.Status = "Affiliate";
            //            //updateact.Flag = (int)CsystemType.getCommon.Affiliate;
            //            //if (updateact.AffiliationOrder_Number != model.AffiliateOrderNo)
            //            //{
            //            //    updateact.AffiliationOrder_Number = model.AffiliateOrderNo;
            //            //}
            //            //if (updateact.Affiliation_date != model.AffiliateDate)
            //            //{
            //            //    updateact.Affiliation_date = model.AffiliateDate;
            //            //}

            //            //_db.SaveChanges();
            //        }
            //        else
            //        {
            //            //var Trade_Doc = new tbl_Affiliation_documents();

            //            //Trade_Doc.Institute_id = model.itiID;
            //            //Trade_Doc.FileName = model.AffiliateFilePath;
            //            //Trade_Doc.IsActive = true;
            //            //Trade_Doc.Status = "Affiliate";
            //            //Trade_Doc.Flag = (int)CsystemType.getCommon.Affiliate;
            //            //Trade_Doc.AffiliationOrder_Number = model.AffiliateOrderNo;
            //            //Trade_Doc.Affiliation_date = model.AffiliateDate;
            //            //_db.tbl_Affiliation_documents.Add(Trade_Doc);
            //            //_db.SaveChanges();
            //        }

            //    }
            //    else
            //    {
            //        //var Trade_Doc = new tbl_Affiliation_documents();

            //        //Trade_Doc.FileName = model.AffiliateFilePath;
            //        //Trade_Doc.IsActive = true;
            //        //Trade_Doc.Status = "Activate";
            //        //Trade_Doc.Flag = (int)CsystemType.getCommon.Activate;
            //        //Trade_Doc.AffiliationOrder_Number = model.AffiliateOrderNo;
            //        //Trade_Doc.Affiliation_date = model.AffiliateDate;
            //        //_db.tbl_Affiliation_documents.Add(Trade_Doc);
            //        //_db.SaveChanges();
            //    }

            //}
            ////TradeActiveDeactiveUpdateandinsert tradeactive = new TradeActiveDeactiveUpdateandinsert();
            ////if (model.fileName != "null" && model.filePath != null)
            ////{
            ////    tradeactive.FileName = model.fileName;
            ////    tradeactive.FilePath = model.filePath;
            ////}
            ////tradeactive.ITI_Trade_Id = model.itiID;
            ////tradeactive.CreatedBy = model.userID;
            ////tradeactive.CreatedOn = DateTime.Now;
            ////tradeactive.IsActive = true;
            ////if (model.userID == 13)
            ////{
            ////    tradeactive.ApprovalStatus = model.status == "Active" ? 0 : 1;
            ////    tradeactive.ApprovalFlowId = 2;
            ////}
            ////else
            ////{
            ////    tradeactive.ApprovalStatus = model.StatusId;
            ////    tradeactive.ApprovalFlowId = model.Flowid;

            ////}
            ////tradeactive.ActDeActRemarks = model.ActDeActRemarks;

            ////_affiliationDll.TradeActiveDeactiveUpdateandinsertDLL(tradeactive);

            #endregion
            return status;
        }
        private void AffiliateorDeaffiliate(TradeActiveandDeactiveStatus model,
            ref tbl_Affiliation_documents _tbl_Affiliation_documents, bool isAffilate,bool isUpdate)
        {
            _tbl_Affiliation_documents.Institute_id = model.clgId;
            _tbl_Affiliation_documents.IsActive = true;
            _tbl_Affiliation_documents.Status = isAffilate ? "Affiliate" : "Deaffiliate";
            _tbl_Affiliation_documents.Flag = isAffilate ? (int)CsystemType.getCommon.Affiliate :
                (int)CsystemType.getCommon.Deaffiliate;

            if (isUpdate)
            {    
                if (isAffilate)
                {
                    if (model.AffiliateFilePath != null) //affiliation
                    {
                        _tbl_Affiliation_documents.FileName = model.AffiliateFilePath;
                    }
                    if (_tbl_Affiliation_documents.AffiliationOrder_Number != model.AffiliateOrderNo && model.AffiliateOrderNo!=null)
                    {
                        _tbl_Affiliation_documents.AffiliationOrder_Number = model.AffiliateOrderNo;
                    }
                    if (_tbl_Affiliation_documents.Affiliation_date != model.AffiliateDate && model.AffiliateDate != null)
                    {
                        _tbl_Affiliation_documents.Affiliation_date = model.AffiliateDate;
                    }
                }
                else
                {
                    if (model.filePath != null) //deaffiliation
                    {
                        _tbl_Affiliation_documents.FileName = model.filePath;
                    }
                    if (_tbl_Affiliation_documents.AffiliationOrder_Number != model.DeaffiliateOrderNo && model.DeaffiliateOrderNo != null)
                    {
                        _tbl_Affiliation_documents.AffiliationOrder_Number = model.DeaffiliateOrderNo;
                    }
                    if (_tbl_Affiliation_documents.Affiliation_date != model.DeffiliateDate && model.DeffiliateDate != null)
                    {
                        _tbl_Affiliation_documents.Affiliation_date = model.DeffiliateDate;
                    }
                }
            }
            else
            {
                if (model.filePath != null|| model.AffiliateFilePath != null) //deaffiliation
                {
                    _tbl_Affiliation_documents.FileName = isAffilate ? model.AffiliateFilePath : model.filePath;
                }
                if (model.DeaffiliateOrderNo != null || model.AffiliateOrderNo != null)
                {
                    _tbl_Affiliation_documents.AffiliationOrder_Number = isAffilate ? model.AffiliateOrderNo : model.DeaffiliateOrderNo;
                }
                if (model.DeffiliateDate != null || model.AffiliateDate != null)
                {
                    _tbl_Affiliation_documents.Affiliation_date = isAffilate ? model.AffiliateDate : model.DeffiliateDate;
                }
            }
        }

        public bool IsMisCodeExistsBLL(string miscode)
        {
            return _affiliationDll.IsMisCodeExists(miscode);
        }

        public bool IsITICollegeNameExistsBLL(string iticollegename)
        {
            return _affiliationDll.IsITICollegeNameExists(iticollegename);
        }


        public List<DeAffiliateInstitute> GetAffiliateInstituteDetails(int DistId)
        {
            return _affiliationDll.GetAllInstituteDetails(DistId);
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilter()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitsDeatilsDLLOnFilter();
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForOs()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieOSDeatilsDLLOnFilter();
        }
        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsBLLOnFilterForAdDD()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieADDDDeatilsDLLOnFilter();
        }

        public List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLOnFilter()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteDLLCwFilter();
        }
        public List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLOSOnFilter()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteOSDLLOnSearch();
        }

        public List<DeAffiliateInstitute> GetAllAffiliateInsDeatilsBLLADOnFilter()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteADDLLOnsearch();
        }
        public List<DeAffiliateInstitute> GetAllDeaffiliateInstitutePOPUP()
        {
            return _affiliationDll.GetAllDeaffiliateInstitutePOPUP();
        }
        public AffiliationCollegeDetailsTest AddAffiliationCollegeDetailsBLL1(AffiliationCollegeDetailsTest Affi1)
        {
            return _affiliationDll.AddAffiliationCollegeDetailsDLL1(Affi1);
        }

        public AffiliationNested PublishActiveDeactiveTradeUnit(AffiliationNested model)
        {
            return _affiliationDll.PublishActiveDeactiveTradeUnit(model);
        }

        public AffiliationNested PublishAffiliateDeaffiliateInstitute(AffiliationNested model)
        {
            return _affiliationDll.PublishAffiliateDeaffiliateInstitute(model);
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewPOPUP()
        {
            return _affiliationDll.GetAllActiveandDeactiveUnitwsieViewPOPUP();
        }

        public ToPublishRecords PublishAffiliateInstitutesBLL(ToPublishRecords model)
        {
            return _affiliationDll.PublishAffiliateInstitutes(model);
        }


        public List<SelectListItem> GetAllDesignationDLL()
        {
            return _affiliationDll.GetAllDesignationDLL();
        }

        public List<SelectListItem> GetAllTeachingSubjectDLL()
        {
            return _affiliationDll.GetAllTeachingSubjectDLL();
        }

        public List<SelectListItem> GetAllTradesDLL()
        {
            return _affiliationDll.GetAllTradesDLL();
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLL(int Year_id, int Course_Id, int Division_Id, int District_Id, int taluk_id, int Insttype_Id, int location_Id, int tradeId, int scheme_Id, string training_Id, int ReportType_Id)
        {
            return _affiliationDll.GetAllAffiliatedInstituteReportDLL(Year_id, Course_Id, Division_Id, District_Id, taluk_id, Insttype_Id, location_Id, tradeId, scheme_Id, training_Id, ReportType_Id);
        }

        public List<ActiveandDeactiveUnitsDeatils> GetAllDeaffiliateInstituteDLLForReport()
        {
            return _affiliationDll.GetAllDeaffiliateInstituteDLLForReport();
        }
        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForTrade()
        {
            return _affiliationDll.GetAllAffiliatedInstituteReportDLLForTrade();
        }
        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForUnits()
        {
            return _affiliationDll.GetAllAffiliatedInstituteReportDLLForUnits();
        }

        public List<int> GetStafTradeList(int id)
        {
            return _affiliationDll.GetStafTradeList(id);
        }
        public List<SelectListItem> GetAllYearDLL()
        {
            return _affiliationDll.GetAllYearDLL();
        }

        public List<SelectListItem> GetAllInstitute(int DistId)
        {
            return _affiliationDll.GetAllInstitute(DistId);
        }

        public List<SelectListItem> GetAllGender()
        {
            return _affiliationDll.GetAllGender();
        }
        public AffiliationDocuments GetAllAffiliationDocForDownload(int collegeId, int? Trade_Id=0, int? shift_id = 0, int? flag = 0)
        {
            return _affiliationDll.GetAllAffiliationDocForDownload(collegeId, Trade_Id, shift_id, flag);
        }
        #region staff module methods

        public List<SelectListItem> GetAllStaffTypeDLL()
        {
            return _affiliationDll.GetAllStaffTypeDLL();
        }
        public string AddStaffDetail(StaffInstituteDetails staff, int loginId)
        {
            return _affiliationDll.AddStaffDetail(staff, loginId);
        }
        public List<StaffInstituteDetails> GetstaffDetails(int loginId = 0)
        {
            return _affiliationDll.GetstaffDetails(loginId);
        }
        public List<StaffInstituteDetails> ListstaffDetails(int? userid,int? iniId)
        {
            return _affiliationDll.ListstaffDetails(userid, iniId);
        }
        public StaffInstituteDetails EditStaff(int id)
        {
            return _affiliationDll.EditStaff(id);
        }
        public StaffInstituteDetails ViewStaff(int id)
        {
            return _affiliationDll.ViewStaff(id);
        }

        public bool UpdateStaff(StaffInstituteDetails staff, int loginId)
        {
            return _affiliationDll.Updatestaff(staff, loginId);
        }
        public string ApproveStaff(List<StaffInstituteDetails> staff, int loginId)
        {
            return _affiliationDll.ApproveStaff(staff, loginId);
        }

        public bool DeleteStaff(int id, string session = "")
        {
            return _affiliationDll.DeleteStaff(id, session);
        }

        public List<StaffInstituteDetails> GetstaffStatus(int loginId, int Year, int courseId, int Quarter1)
        {
            return _affiliationDll.GetstaffStatus(loginId, Year, courseId, Quarter1);
        }
        public List<StaffInstituteDetails> GetstaffStatusOSAD(int usrid, int Year, int courseId, int quarter)
        {
            return _affiliationDll.GetstaffStatusOSAD(usrid, Year, courseId, quarter);
        }

        public List<StaffInstituteDetails> GetAllstaffInstituteReport(int Year, int courseId, int divisionId,
            int districtId, int taluk, int Insttype, int location, int stafftype, int tradeId,
            int gender, int scheme, string training, int quarter)
        {
            return _affiliationDll.GetAllstaffInstituteReport(Year, courseId, divisionId,
             districtId, taluk, Insttype, location, stafftype, tradeId,
             gender, scheme, training, quarter);
        }
        public List<StaffInstituteDetails> ViewStaffhistory(int id, int session ,int quarter)
        {
            return _affiliationDll.ViewStaffhistory(id,session, quarter);
        }

        public string SubmitStaffDetails(List<StaffInstituteDetails> staff, int loginId, int roleId)
        {
            return _affiliationDll.SubmitStaffDetails(staff, loginId, roleId);
        }
        public List<StaffInstituteDetails> GetstaffStatusForCW()
        {
            return _affiliationDll.GetstaffStatusForCW();
        }
        //public List<StaffInstituteDetails> StaffDetailsView()
        //{
        //    return _affiliationDll.StaffDetailsView();
        //}
        public List<int> GetStaffSubjectList(int id)
        {
            return _affiliationDll.GetStaffSubjectList(id);
        }
        public List<StaffInstituteDetails> GetApprovedstaffInstitute(int loginId, int year, int courseId, int DivisionId,
            int DistrictId, int InstituteId, int quarter)
        {
            return _affiliationDll.GetApprovedstaffInstitute(loginId, year,  courseId,  DivisionId,
             DistrictId,  InstituteId,  quarter);
        }
        public List<StaffInstituteDetails> GetApprovedstaffInstituteDivisionWise(int UserId)
        {
            return _affiliationDll.GetApprovedstaffInstituteDivisionWise(UserId);
        }

        public int GetUserDivionIdBLL(int LoginId)
        {
            return _affiliationDll.GetUserDivionIdDLL(LoginId);
        }

        #region staff details session wise
        public List<StaffInstituteDetails> GetStaffDetailsSessionWise(int userId = 0, string session = "", int? iniId = 0)
        {// less than aug
         //int startMonth = 8;
         //session = session == "" ?
         ////DateTime.Now.Month < 8 ? DateTime.Now.AddYears(-1).Year.ToString() : DateTime.Now.Year.ToString()
         ////: session;
         // DateTime.Now.Month < startMonth ? (startMonth - 1)/3 + 1 .ToString() : ((startMonth - 1) / 3).ToString()
         //: session;
         DateTime dt = DateTime.Now;
            if(session=="")
            session = GetQuarter(dt).ToString();
            //session = GetQuarter(DateTime.Now).ToString();
           


            return _affiliationDll.GetStaffDetailsSessionWise(userId, session, iniId);
        }
        public int GetQuarter(DateTime date)
        {
            if (date.Month >= 8 && date.Month <= 10)
                return 1;
            else if (date.Month >= 5 && date.Month <= 7)
                return 4;
            else if (date.Month >= 2 && date.Month <= 4)
                return 3;
            else
                return 2;
        }
        #endregion


        #endregion
    }
}
