using DLL.DBConnection;
using Models;
using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Admission.TransferAdmissionSeats
{
    public class TransferAdmissionSeat : ITransferAdmissionSeat
    {
        private readonly DbConnection _db = new DbConnection();

        public List<Trades> GetTrades(int loginId)
        {
            try
            {
                int collgeId = GetCollegeId(loginId);
                if (collgeId != 0)
                {
                    var courseCode = _db.tbl_iti_college_details.Where(x => x.iti_college_id == collgeId).Select(y => y.CourseCode).FirstOrDefault();
                    var res= ( from seat in _db.tbl_ITI_Trade 
                               join iti in _db.tbl_iti_college_details on seat.ITICode  equals iti.iti_college_id
                               join tr in _db.tbl_trade_mast  on seat.TradeCode equals tr.trade_id
                               where seat.IsActive==true && iti.iti_college_id==collgeId
                               select new Trades
                               {
                                   TradeId = tr.trade_id,
                                   TradeName = tr.trade_name
                               }).ToList();
                    
                    return res;
                }
                else
                {
                    var res = new List<Trades>();
                    return res;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetMisCode(int instiId)
        {
            try
            {
                var res = _db.tbl_iti_college_details.Where(x => x.iti_college_id == instiId).Select(y => y.MISCode).FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Trades> GetAvailableseatsTrades(int instiId)
        {
            try
            {
                //var res=_db.tbl_SeatMatrix_Main.Where(x =>x.InstituteId== instiId && x.Status==2).
                var res = new List<Trades>();


                var rev = (from tt in _db.tbl_ITI_Trade
                           where tt.ITICode == instiId && tt.IsActive == true
                           select new TradeCodes
                           {
                               Trade_ITI_Id = tt.Trade_ITI_id,
                               TradeCode = tt.TradeCode
                           }).Distinct().ToList();
                
                foreach (var itm in rev)
                {
                    var tradename = _db.tbl_trade_mast.Where(x => x.trade_id == itm.TradeCode).Select(y => y.trade_name).FirstOrDefault();
                    Trades td = new Trades();
                    td.TradeId = (int)itm.TradeCode;
                    td.TradeName = tradename;
                    var trad_seats = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_Id == itm.Trade_ITI_Id).Sum(y => y.SeatsPerUnit);
                    var admitted_seats = _db.tbl_SeatAllocationDetail_Seatmatrix.Where(x => x.Status == 6 && x.InstituteId == instiId && x.TradeId == itm.TradeCode).Count();
                    if (admitted_seats < trad_seats)                        
                        res.Add(td);
                }

                

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Trades> GetUnits(int instiId, int trade)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_Trade_Shifts
                           join bb in _db.tbl_ITI_Trade on aa.ITI_Trade_Id equals bb.Trade_ITI_id
                           where bb.ITICode == instiId && bb.TradeCode == trade
                           select new Trades { TradeId = aa.Units }).Distinct().ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Trades> GetShifts(int instiId, int trade, int unit)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_Trade_Shifts
                           join bb in _db.tbl_ITI_Trade on aa.ITI_Trade_Id equals bb.Trade_ITI_id
                           where bb.ITICode == instiId && bb.TradeCode == trade && aa.Units == unit
                           select new Trades { TradeId = aa.Shift }).Distinct().ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetDualSystem(int insti, int trade)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_Trade_Shifts
                           join bb in _db.tbl_ITI_Trade on aa.ITI_Trade_Id equals bb.Trade_ITI_id
                           where bb.ITICode == insti && bb.TradeCode == trade
                           select aa.Dual_System).FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Trades> GetTranseferInstitutes(int type, int taluklgd)
        {
            try
            {
                var res = (from aa in _db.tbl_iti_college_details
                           where aa.Insitute_TypeId == type && aa.taluk_id == taluklgd && aa.is_active==true
                           select new Trades
                           {
                               TradeId = aa.iti_college_id,
                               TradeName = aa.iti_college_name
                           }).ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool ApproveAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var res = _db.tbl_transfer_admitted_data.Where(x => x.Admit_Id == transSeatId).FirstOrDefault();
                    res.FlowId = 9;
                    res.Status = status;

                    tbl_transfer_admitted_data_trans seattrans = new tbl_transfer_admitted_data_trans();
                    seattrans.Admit_Id = transSeatId;
                    seattrans.Trans_Date = DateTime.Now;
                    seattrans.Status = status;
                    if (remarks != null)
                        seattrans.Remarks = remarks;
                    seattrans.CreatedOn = DateTime.Now;
                    seattrans.CreatedBy = roleId;
                    seattrans.FlowId = 9;
                    _db.tbl_transfer_admitted_data_trans.Add(seattrans);
                    _db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public bool SendBackAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var res = _db.tbl_transfer_admitted_data.Where(x => x.Admit_Id == transSeatId).FirstOrDefault();
                    res.FlowId = 9;
                    res.Status = status;

                    tbl_transfer_admitted_data_trans seattrans = new tbl_transfer_admitted_data_trans();
                    seattrans.Admit_Id = transSeatId;
                    seattrans.Trans_Date = DateTime.Now;
                    seattrans.Status = status;
                    if (remarks != null)
                        seattrans.Remarks = remarks;
                    seattrans.CreatedOn = DateTime.Now;
                    seattrans.CreatedBy = roleId;
                    seattrans.FlowId = 9;
                    _db.tbl_transfer_admitted_data_trans.Add(seattrans);
                    _db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public bool ForwardAdmittedData(int transSeatId, int status, string remarks, int loginId, int roleId, int flowId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var res = _db.tbl_transfer_admitted_data.Where(x => x.Admit_Id == transSeatId).FirstOrDefault();
                    res.FlowId = flowId;
                    res.Status = status;

                    tbl_transfer_admitted_data_trans seattrans = new tbl_transfer_admitted_data_trans();
                    seattrans.Admit_Id = transSeatId;
                    seattrans.Trans_Date = DateTime.Now;
                    seattrans.Status = status;
                    if (remarks != null)
                        seattrans.Remarks = remarks;
                    seattrans.CreatedOn = DateTime.Now;
                    seattrans.CreatedBy = roleId;
                    seattrans.FlowId = flowId;
                    _db.tbl_transfer_admitted_data_trans.Add(seattrans);
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public List<ApplicantTransferModel> GetAdmittedData(int loginId, int session, int course, int? trade, int round)
        {
            try
            {
                int college_id = GetCollegeId(loginId);
                var rr = _db.tbl_transfer_admitted_data.Select(x => x.ApplicantId).ToList();
                int year = 0;
                var SessYear = "";

                    //int yer = Convert.ToInt32(_db.tbl_Year.Where(x => x.YearID == session).Select(y => y.Year).FirstOrDefault());

                if (session != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == session).Select(y => y.Year).FirstOrDefault();
                    SessYear = yr;
                    yr = yr.Split('-')[1];
                    year = Convert.ToInt32(yr);
                }

                var res = (from aa in _db.tbl_Applicant_ITI_Institute_Detail
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                               join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                               join ff in _db.tbl_ITI_Trade on cc.InstituteId equals ff.ITICode
                               join gg in _db.tbl_trade_mast on cc.TradeId equals gg.trade_id
                               join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                               join jj in _db.tbl_AdmissionatInstituteStatus on aa.ApplInstiStatus equals jj.StatusId
                               where dd.iti_college_id == college_id && !rr.Contains(aa.ApplicationId) && bb.ApplyYear == year && dd.CourseCode == course /*&& gg.trade_id == trade*/ && aa.ApplInstiStatus == 6
                               select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   ApplicantId = bb.ApplicationId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   TradeId = gg.trade_id,
                                   Unit = (int)cc.UnitId,
                                   Shift = (int)cc.ShiftId,
                                   DualSystem = aa.DualType == 1 ? "Yes" : "No",
                                   AdmissionStatus = aa.AdmittedStatus,
                                   AdmissionStatusName = jj.StatusName,
                                   ApplicantInstituId = cc.InstituteId,
                                   TradeDuration = gg.trade_duration,
                                   ApplyMonth = bb.ApplyMonth,
                                   YearSession= SessYear
                               }).Distinct().ToList();
                    foreach (var itm in res)
                    {
                        var se = res.Select(x => x.TradeId == itm.TradeId).Count();
                        if (se <= 5)
                            itm.Select = true;
                        else
                            itm.Select = false;
                    }
                    if (trade != null)
                    {
                        res = res.Where(a => a.TradeId == trade).ToList();
                    }
                    return res;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ApplicantTransferModel> GetRequestedDetails(int loginId, int session, int course, int trade)
        {
            try
            {
                int college_id = GetCollegeId(loginId);
                var rr = _db.tbl_transfer_admitted_data.Select(x => x.ApplicantId).ToList();
                if (session == 0)
                {

                    var res = (from aa in _db.tbl_Applicant_ITI_Institute_Detail
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                               join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                               join ff in _db.tbl_ITI_Trade on dd.iti_college_id equals ff.ITICode
                               join gg in _db.tbl_trade_mast on cc.TradeId equals gg.trade_id
                               join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                               join ii in _db.tbl_ITI_Trade_Shifts on ff.Trade_ITI_id equals ii.ITI_Trade_Id
                               where dd.iti_college_id == college_id && !rr.Contains(aa.ApplicationId)
                               select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   YearSession = _db.tbl_Year.Where(a=> a.Year.Contains(bb.ApplyYear.ToString())).Select(a=> a.Year).FirstOrDefault().ToString(),
                                   ApplicantId = bb.ApplicationId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   TradeId = gg.trade_id,
                                   Unit = ii.Units,
                                   Shift = ii.Shift,
                                   DualSystem = ii.Dual_System,
                                   AdmissionStatus = aa.AdmittedStatus,
                                   ApplicantInstituId = aa.ApplicantITIInstituteId
                               }).Distinct().ToList();
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_Applicant_ITI_Institute_Detail
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicationId equals bb.ApplicationId
                               join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                               join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                               join ff in _db.tbl_ITI_Trade on dd.iti_college_id equals ff.ITICode
                               join gg in _db.tbl_trade_mast on cc.TradeId equals gg.trade_id
                               join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                               join ii in _db.tbl_ITI_Trade_Shifts on ff.Trade_ITI_id equals ii.ITI_Trade_Id
                               where dd.iti_college_id == college_id && bb.ApplyYear == session && 
                               dd.CourseCode == course && gg.trade_id == trade && !rr.Contains(aa.ApplicationId) 
                               select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   YearSession = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   ApplicantId = bb.ApplicationId,
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   TradeId = gg.trade_id,
                                   Unit = ii.Units,
                                   Shift = ii.Shift,
                                   DualSystem = ii.Dual_System,
                                   AdmissionStatus = aa.AdmittedStatus,
                                   ApplicantInstituId = aa.ApplicantITIInstituteId
                               }).Distinct().ToList();
                    return res;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ApplicantTransferModel> GetTransferRemarks(int seatId)
        {
            var res = (from aa in _db.tbl_transfer_admitted_data_trans
                       join bb in _db.tbl_status_master on aa.Status equals bb.StatusId
                       join cc in _db.tbl_role_master on aa.CreatedBy equals cc.role_id
                       //join dd in _db.tbl_role_master on aa.FlowId equals dd.role_id
                       where aa.Admit_Id == seatId && bb.StatusId != (int)CmnClass.Status.SubmitTransferAppl
                       select new ApplicantTransferModel
                       {
                           StatusName = bb.StatusName,
                           Remarks = aa.Remarks,
                           From = cc.role_description,
                           Date = aa.CreatedOn.ToString(),
                           FlowId = aa.FlowId
                       }).ToList();

            var rss = (from a in res
                       join b in _db.tbl_role_master on a.FlowId equals b.role_id
                       select new ApplicantTransferModel
                       {
                           StatusName = a.StatusName,
                           Remarks = a.Remarks,
                           From = a.From,
                           Date = a.Date,
                           To = b.role_description
                       }).ToList();

            return rss;
        }

        public string SubmitAdmittedData(List<ApplicantTransferModel> seat, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (seat.Count != 0)
                    {
                        foreach (var item in seat)
                        {
                            int collegeId = GetCollegeId(loginId);
                            var res = _db.tbl_transfer_admitted_data.Where(x => x.ApplicantId == item.ApplicantId).FirstOrDefault();
                            if (res == null)
                            {
                                tbl_transfer_admitted_data seat_data = new tbl_transfer_admitted_data();
                                seat_data.ApplicantITIInstituteId = item.ApplicantInstituId;
                                seat_data.AdmissionRegistrationNumber = item.AdmisRegiNumber;
                                seat_data.ApplicantId = item.ApplicantId;
                                seat_data.DualSystemTraining = item.DualSystem;
                                seat_data.CreatedOn = DateTime.Now;
                                seat_data.CreatedBy = loginId;
                                seat_data.Status = 20;
                                seat_data.FlowId = 9;
                                seat_data.Trade = item.TradeId;
                                seat_data.Unit = item.Unit;
                                seat_data.Shift = item.Shift;
                                _db.tbl_transfer_admitted_data.Add(seat_data);
                                _db.SaveChanges();

                                int rr = _db.tbl_transfer_admitted_data.OrderByDescending(y => y.Admit_Id).Select(x => x.Admit_Id).FirstOrDefault();

                                tbl_transfer_admitted_data_trans seattrans = new tbl_transfer_admitted_data_trans();
                                seattrans.Admit_Id = rr;
                                seattrans.ApplicantITIInstituteId = item.ApplicantInstituId;
                                seattrans.ApplicantId = item.ApplicantId;
                                seattrans.AdmissionRegistrationNumber = item.AdmisRegiNumber;
                                seattrans.Trans_Date = DateTime.Now;
                                seattrans.Status = 20;
                                seattrans.Remarks = item.Remarks;
                                seattrans.CreatedOn = DateTime.Now;
                                seattrans.CreatedBy = roleId;
                                seattrans.FlowId = 9;
                                seattrans.Trade = item.TradeId;
                                seattrans.Unit = item.Unit;
                                seattrans.Shift = item.Shift;
                                _db.tbl_transfer_admitted_data_trans.Add(seattrans);
                            }
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public List<Trades> GetInstituteTypes()
        {
            try
            {
                var res = (from aa in _db.tbl_Institute_type
                           where aa.IsActive == true
                           select new Trades
                           {
                               TradeId = aa.Institute_type_id,
                               TradeName = aa.Institute_type
                           }).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Trades> GetInstituteNames(int type)
        {
            try
            {
                var res = (from aa in _db.tbl_iti_college_details
                           where aa.Insitute_TypeId == type
                           select new Trades
                           {
                               TradeId = aa.iti_college_id,
                               TradeName = aa.iti_college_name
                           }).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        int GetCollegeId(int loginId)
        {
            try
            {
                int id = _db.Staff_Institute_Detail.Where(x => x.UserId == loginId).Select(y => y.InstituteId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<ApplicantTransferModel> GetAdmittedDataStatus(int loginId, int roleId)
        {
            try
            {
                if (roleId == 9)
                {
                    int collegId = GetCollegeId(loginId);
                    var res = (from aa in _db.tbl_transfer_admitted_data
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                               join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                               join gg in _db.tbl_trade_mast on aa.Trade equals gg.trade_id
                               join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                               join jj in _db.tbl_status_master on aa.Status equals jj.StatusId
                               join dj in _db.tbl_role_master on aa.FlowId equals dj.role_id
                               where aa.ApplicantITIInstituteId == collegId
                               select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   YearSession = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   Unit = aa.Unit,
                                   Shift = aa.Shift,
                                   DualSystem = aa.DualSystemTraining,
                                   //StatusName = aa.Status == 2 ? jj.StatusName : jj.StatusName + " - " + dj.role_DescShortForm,
                                   StatusName = (aa.Status == 2 ? "Approved" : jj.StatusName + " - " + dj.role_DescShortForm),
                                   StatusId = aa.Admit_Id,
                                   TradeDuration = gg.trade_duration,
                                   ApplyMonth = bb.ApplyMonth,
                                   statuswith=aa.Status
                               }).Distinct().ToList();
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_transfer_admitted_data
                               join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                               join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                               join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                               join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                               join gg in _db.tbl_trade_mast on aa.Trade equals gg.trade_id
                               join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                               join jj in _db.tbl_status_master on aa.Status equals jj.StatusId
                               join dj in _db.tbl_role_master on aa.FlowId equals dj.role_id
                               where aa.Status != 20
                               select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   YearSession = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   Unit = aa.Unit,
                                   Shift = aa.Shift,
                                   DualSystem = aa.DualSystemTraining,
                                   //StatusName = aa.Status == 2 ? jj.StatusName : jj.StatusName + " - " + dj.role_DescShortForm,
                                   StatusName = (aa.Status == 2 ? "Approved" : jj.StatusName + " - " + dj.role_DescShortForm),
                                   StatusId = aa.Admit_Id,
                                   TradeDuration = gg.trade_duration,
                                   ApplyMonth = bb.ApplyMonth,
                                   //statuswith = aa.Status
                               }).Distinct().ToList();
                    return res;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ApplicantTransferModel> GetApplicantTransferbyList(int loginId, int roleId)
        {
            try
            {
               int collegId = GetCollegeId(loginId);
               var res = (from aa in _db.tbl_transfer_Institute_Details
                          join td in _db.tbl_transfer_admitted_data on aa.Admit_Id equals td.Admit_Id
                          join bb in _db.tbl_Applicant_Detail on td.ApplicantId equals bb.ApplicationId
                          //join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                          join dd in _db.tbl_iti_college_details on aa.InstituteId equals dd.iti_college_id
                          join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                          join gg in _db.tbl_trade_mast on aa.TradeId equals gg.trade_id
                          join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                          join jj in _db.tbl_status_master on td.Status equals jj.StatusId
                          join dj in _db.tbl_role_master on td.FlowId equals dj.role_id                              
                          where aa.InstituteId == collegId && td.Status==2
                          select new ApplicantTransferModel
                               {
                                   Session = bb.ApplyYear,
                                   YearSession = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   ApplicantNumber = bb.ApplicantNumber,
                                   AdmisRegiNumber = td.AdmissionRegistrationNumber,
                                   ApplicantName = bb.ApplicantName,
                                   MISCode = dd.MISCode,
                                   InstituteType = ee.Institute_type,
                                   InstituteName = dd.iti_college_name,
                                   CourseType = hh.course_type_name,
                                   TradeName = gg.trade_name,
                                   Unit = aa.Unit,
                                   Shift = aa.Shift,
                                   DualSystem = td.DualSystemTraining,
                                   StatusName = (td.Status == 2 ? "Approved" : jj.StatusName + " - " + dj.role_DescShortForm),
                                   StatusId = aa.Admit_Id,
                                   TradeDuration = gg.trade_duration,
                                   ApplyMonth = bb.ApplyMonth,
                                   statuswith = td.Status
                               }).Distinct().ToList();
               return res;               
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //edid this now
        public List<ApplicantTransferModel> GetApprovedTransferbyList(int loginId, int roleId)
        {
            try
            {
                int collegId = GetCollegeId(loginId);
                var res = (from aa in _db.tbl_transfer_Institute_Details
                           join td in _db.tbl_transfer_admitted_data on aa.Admit_Id equals td.Admit_Id
                           join bb in _db.tbl_Applicant_Detail on td.ApplicantId equals bb.ApplicationId
                           //join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                           join dd in _db.tbl_iti_college_details on aa.InstituteId equals dd.iti_college_id
                           join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                           join gg in _db.tbl_trade_mast on aa.TradeId equals gg.trade_id
                           join hh in _db.tbl_course_type_mast on dd.CourseCode equals hh.course_id
                           join jj in _db.tbl_status_master on td.Status equals jj.StatusId
                           join dj in _db.tbl_role_master on td.FlowId equals dj.role_id
                           where td.ApplicantITIInstituteId == collegId && td.Status == 2
                           select new ApplicantTransferModel
                           {
                               Session = bb.ApplyYear,
                               YearSession = _db.tbl_Year.Where(a => a.Year.Contains(bb.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                               ApplicantNumber = bb.ApplicantNumber,
                               AdmisRegiNumber = td.AdmissionRegistrationNumber,
                               ApplicantName = bb.ApplicantName,
                               MISCode = dd.MISCode,
                               InstituteType = ee.Institute_type,
                               InstituteName = dd.iti_college_name,
                               CourseType = hh.course_type_name,
                               TradeName = gg.trade_name,
                               Unit = aa.Unit,
                               Shift = aa.Shift,
                               DualSystem = td.DualSystemTraining,
                               StatusName = (td.Status == 2 ? "Approved" : jj.StatusName + " - " + dj.role_DescShortForm),
                               StatusId = aa.Admit_Id,
                               TradeDuration = gg.trade_duration,
                               ApplyMonth = bb.ApplyMonth,
                               statuswith = td.Status
                           }).Distinct().ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ApplicantTransferModel> GetApplicantInstituteDetails(int id)
        {
            try
            {
                List<ApplicantTransferModel> li = new List<ApplicantTransferModel>();

                var res = (from aa in _db.tbl_transfer_admitted_data
                           join bb in _db.tbl_Applicant_Detail on aa.ApplicantId equals bb.ApplicationId
                           join cc in _db.tbl_SeatAllocationDetail_Seatmatrix on bb.ApplicationId equals cc.ApplicantId
                           join dd in _db.tbl_iti_college_details on cc.InstituteId equals dd.iti_college_id
                           join ee in _db.tbl_Institute_type on dd.Insitute_TypeId equals ee.Institute_type_id
                           join gg in _db.tbl_trade_mast on aa.Trade equals gg.trade_id
                           join jj in _db.tbl_status_master on aa.Status equals jj.StatusId
                           join ii in _db.tbl_division_master on dd.division_id equals ii.division_id
                           join kk in _db.tbl_district_master on dd.district_id equals kk.district_lgd_code
                           join ll in _db.tbl_taluk_master on dd.taluk_id equals ll.taluk_lgd_code
                           where aa.Admit_Id == id
                           select new ApplicantTransferModel
                           {
                               AdmitId = aa.Admit_Id,
                               Session = bb.ApplyYear,
                               ApplicantNumber = bb.ApplicantNumber,
                               AdmisRegiNumber = aa.AdmissionRegistrationNumber,
                               ApplicantName = bb.ApplicantName,
                               MISCode = dd.MISCode,
                               InstituteType = ee.Institute_type,
                               InstituteName = dd.iti_college_name,
                               TradeName = gg.trade_name,
                               Unit = aa.Unit,
                               Shift = aa.Shift,
                               DualSystem = aa.DualSystemTraining,
                               StatusName = jj.StatusName,
                               StatusId = aa.Status,
                               FlowId = aa.FlowId,
                               DivisionName = ii.division_name,
                               DistrictName = kk.district_ename,
                               TalukName = ll.taluk_ename,
                               TalukLgdCode = ll.taluk_lgd_code,
                               DistrictLgdCode = kk.district_lgd_code,
                               DivisionId= ii.division_id
                           }).FirstOrDefault();
                li.Add(res);

                if (res != null)
                {
                    if (res.AdmitId != 0)
                    {
                        var rslt = (from aaa in _db.tbl_transfer_Institute_Details
                                    join bbb in _db.tbl_iti_college_details on aaa.InstituteId equals bbb.iti_college_id
                                    join ccc in _db.tbl_Institute_type on bbb.Insitute_TypeId equals ccc.Institute_type_id
                                    join ddd in _db.tbl_division_master on aaa.DivisionId equals ddd.division_id
                                    join eee in _db.tbl_district_master on aaa.DistrictId equals eee.district_lgd_code
                                    join fff in _db.tbl_taluk_master on aaa.TalukId equals fff.taluk_lgd_code
                                    join ggg in _db.tbl_trade_mast on aaa.TradeId equals ggg.trade_id
                                    where aaa.Admit_Id == res.AdmitId
                                    select new ApplicantTransferModel
                                    {
                                        MISCode = bbb.MISCode,
                                        InstituteTypeId = aaa.InstituteType,
                                        InstituteType = ccc.Institute_type,
                                        ApplicantInstituId = aaa.InstituteId,
                                        InstituteName = bbb.iti_college_name,
                                        DivisionId = aaa.DivisionId,
                                        DivisionName = ddd.division_name,
                                        DistrictId = aaa.DistrictId,
                                        DistrictName = eee.district_ename,
                                        TalukId = aaa.TalukId,
                                        TalukName = fff.taluk_ename,
                                        TalukLgdCode = fff.taluk_lgd_code,
                                        DistrictLgdCode = eee.district_lgd_code,
                                        TradeName = ggg.trade_name,
                                        Unit = aaa.Unit,
                                        Shift = aaa.Shift,
                                        DualSystem = aaa.DualSystem,
                                        DivisionIdEdit = ddd.division_id,
                                        TradeId=ggg.trade_id
                                        
                                    }).FirstOrDefault();
                        if (rslt != null)
                            li.Add(rslt);
                    }
                }

                return li;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateApplicantInstituteDetails(ApplicantTransferModel tran, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (tran != null)
                    {
                        var resp = _db.tbl_transfer_admitted_data.Where(x => x.Admit_Id == tran.AdmitId).FirstOrDefault();
                        resp.Status = 5;
                        resp.FlowId = tran.FlowId;

                        var sss = _db.tbl_transfer_Institute_Details.Where(x => x.Admit_Id == resp.Admit_Id).FirstOrDefault();
                        if (sss != null)
                        {
                            sss.DivisionId = tran.DivisionId;
                            sss.DistrictId = tran.DistrictId;
                            sss.TalukId = tran.TalukId;
                            sss.InstituteId = tran.ApplicantInstituId;
                            sss.InstituteType=tran.InstituteTypeId;
                            sss.TradeId = tran.TradeId;
                            sss.Unit = tran.Unit;
                            sss.Shift = tran.Shift;
                            sss.DualSystem = tran.DualSystem;
                        }
                        else
                        {
                            tbl_transfer_Institute_Details detail = new tbl_transfer_Institute_Details();
                            detail.Admit_Id = tran.AdmitId;
                            detail.InstituteId = tran.ApplicantInstituId;
                            detail.DivisionId = tran.DivisionId;
                            detail.DistrictId = tran.DistrictId;
                            detail.TalukId = tran.TalukId;
                            detail.InstituteType = tran.InstituteTypeId;
                            detail.TradeId = tran.TradeId;
                            detail.Unit = tran.Unit;
                            detail.Shift = tran.Shift;
                            detail.DualSystem = tran.DualSystem;
                            _db.tbl_transfer_Institute_Details.Add(detail);
                        }

                        tbl_transfer_admitted_data_trans seattrans = new tbl_transfer_admitted_data_trans();
                        seattrans.Admit_Id = tran.AdmitId;
                        seattrans.ApplicantITIInstituteId = resp.ApplicantITIInstituteId;
                        seattrans.ApplicantId = resp.ApplicantId;
                        seattrans.AdmissionRegistrationNumber = resp.AdmissionRegistrationNumber;
                        seattrans.Trans_Date = DateTime.Now;
                        seattrans.Status = 5;
                        seattrans.Remarks = tran.Remarks;
                        seattrans.CreatedOn = DateTime.Now;
                        seattrans.CreatedBy = roleId;
                        seattrans.FlowId = tran.FlowId;
                        seattrans.Trade = resp.Trade;
                        seattrans.Unit = resp.Unit;
                        seattrans.Shift = resp.Shift;
                        _db.tbl_transfer_admitted_data_trans.Add(seattrans);                       

                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public List<SeatDetails> SeatTransferStatus(int roleId)
        {
            try
            {
                if (roleId == 16|| roleId == 5)
                {
                    var res = (from aa in _db.tbl_status_master
                               where aa.IsActive == true && (aa.StatusId == 7 || aa.StatusId == 9)
                               select new SeatDetails
                               {
                                   Status = aa.StatusId,
                                   StatusName = aa.StatusName
                               }).ToList();
                    return res;
                }
                else if (roleId == 1)
                {
                    var res = (from aa in _db.tbl_status_master
                               where aa.IsActive == true && (aa.StatusId == 2 || aa.StatusId == 9)
                               select new SeatDetails
                               {
                                   Status = aa.StatusId,
                                   StatusName = aa.StatusName
                               }).ToList();
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_status_master
                               where aa.IsActive == true && (aa.StatusId == 7 || aa.StatusId == 2 || aa.StatusId == 9)
                               select new SeatDetails
                               {
                                   Status = aa.StatusId,
                                   StatusName = aa.StatusName
                               }).ToList();
                    return res;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
