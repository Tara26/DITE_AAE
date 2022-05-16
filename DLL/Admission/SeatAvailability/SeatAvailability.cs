using DLL.DBConnection;
using Models.Admission;
using Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DLL.Admission.SeatAvailability
{
    public class SeatAvailability : ISeatAvailability
    {
        private readonly DbConnection _db = new DbConnection();

        public List<SeatDetails> SeatAvailabilityStatus(int roleId)
        {
            try
            {
                if(roleId==16)
                {
                    var res = (from aa in _db.tbl_SeatAvail_status_master
                               where aa.IsActive == true && aa.StatusId != 1 && aa.StatusId!=4 && aa.StatusId!=5
                               select new SeatDetails
                               {
                                   Status = aa.StatusId,
                                   StatusName = aa.StatusName
                               }).ToList();
                    return res;
                }
                else if(roleId <= 5)
                {
                    var res = (from aa in _db.tbl_SeatAvail_status_master
                               where aa.IsActive == true && aa.StatusId != 1 && aa.StatusId != 2 && aa.StatusId != 5
                               select new SeatDetails
                               {
                                   Status = aa.StatusId,
                                   StatusName = aa.StatusName
                               }).ToList();
                    return res;
                }
                else
                {
                    var res = (from aa in _db.tbl_SeatAvail_status_master
                               where aa.IsActive == true && aa.StatusId == 1
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
        public List<UserDetails> GetUserRoles(int id, int level,int roleId)
        {
            try
            {
                if (level == 1)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1
                               orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                    ).ToList();
                    return res;
                }
                else if (level == 3)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no < 60
                               orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                   ).ToList();
                    var ad = new UserDetails();
                    ad.RoleID = 9;
                    ad.RoleName = "ITI Admin";
                    res.Add(ad);
                    return res;
                }
                else if(level == 16)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id == 16 && aa.role_is_active == true && aa.role_Level == 2
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                    ).ToList();
                    //var ad = new UserDetails();
                    //ad.RoleID = 5;
                    //ad.RoleName = "Deputy Director";
                    //res.Add(ad);
                    return res;
                }
                else if (level == 5)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id == 5 && aa.role_is_active == true && aa.role_Level == 1
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                    ).ToList();
                    return res;
                }
                else if (level==9)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id == 9 && aa.role_is_active == true && aa.role_Level == 3
                               orderby aa.role_id ascending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                    ).ToList();
                    return res;
                }
                else if (level == 100)
                {
                    if (roleId == 16)
                    {
                        var res = (from aa in _db.tbl_role_master
                                   where aa.role_id != 4 && aa.role_seniority_no < 60 && aa.role_is_active == true && aa.role_Level == 1
                                   orderby aa.role_seniority_no descending
                                   select new UserDetails
                                   {
                                       RoleID = aa.role_id,
                                       RoleName = aa.role_description
                                   }).ToList();
                        return res;
                    }
                    else
                    {
                        var senior = _db.tbl_role_master.Where(x => x.role_id == roleId).Select(y => y.role_seniority_no).FirstOrDefault();
                        var res = (from aa in _db.tbl_role_master
                                   where aa.role_seniority_no < senior && aa.role_is_active == true && aa.role_Level == 1
                                   select new UserDetails
                                   {
                                       RoleID = aa.role_id,
                                       RoleName = aa.role_description
                                   }).ToList();
                        return res;
                    }
                   
                    
                }
                else
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 2
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                    ).ToList();
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SeatAvailabilty> GetSeatAvailabilityList(int loginId, int courseCode, string AcademicYear)
        {
            try
            {
                int collegeId = GetCollegeId(loginId);  
                var res = (from aa in _db.tbl_iti_college_details
                           join bb in _db.tbl_division_master on aa.division_id equals bb.division_id
                           join cc in _db.tbl_district_master on aa.district_id equals cc.district_lgd_code
                           join dd in _db.tbl_taluk_master on cc.district_lgd_code equals dd.district_lgd_code
                           join ee in _db.tbl_Institute_type on aa.Insitute_TypeId equals ee.Institute_type_id
                           join ff in _db.tbl_ITI_Trade on aa.iti_college_id equals ff.ITICode
                           join gg in _db.tbl_trade_mast on ff.TradeCode equals gg.trade_id
                           join hh in _db.tbl_ITI_Trade_Shifts on ff.Trade_ITI_id equals hh.ITI_Trade_Id
                           join v in _db.tbl_course_type_mast on aa.CourseCode equals v.course_id
                           where aa.iti_college_id == collegeId && aa.CourseCode == courseCode && aa.taluk_id == dd.taluk_lgd_code
                           && (ff.StatusId == (int)CmnClass.Status.Affiliated || ff.StatusId == (int)CmnClass.Status.Published)


                           select new SeatAvailabilty
                           {
                               CourseTypeName=v.course_type_name,
                               AffilationDate = aa.AffiliationDate,
                               MISCode = aa.MISCode,
                               DivisionId = aa.division_id,
                               DivisionName = bb.division_name,
                               DistrictId = aa.district_id,
                               DistrictName = cc.district_ename,
                               TalukId = aa.taluk_id,
                               TalukName = dd.taluk_ename,
                               ITITypeName = ee.Institute_type,
                               ITIName = aa.iti_college_name,
                               TradeItiId = ff.Trade_ITI_id,
                               TradeId = gg.trade_id,
                               TradeName = gg.trade_name,
                               Shift = hh.Shift,
                               Unit = hh.Units,
                               batchsize = gg.trade_seating,
                               DualSystemTraining = hh.Dual_System,
                               Duration=gg.trade_duration,
                               IsActive=hh.IsActive,
                           }).ToList();

                int Year = Convert.ToInt32(AcademicYear);
                var seatMaster = _db.tbl_ITI_trade_seat_master.Where(a=> a.AcademicYear.Year == Year).ToList();
                if (seatMaster.Count > 0)
                {
                    List<Tuple<int, int, int>> cnt = new List<Tuple<int, int, int>>();
                    foreach (var itm in res)
                    {
                        if (seatMaster.Where(a => a.Trade_ITI_Id == itm.TradeItiId && a.ShiftId == itm.Shift && a.UnitId == itm.Unit && a.Status != 5).Count() > 0) // Rejected Status
                        {
                            cnt.Add(new Tuple<int, int, int>(itm.TradeItiId, itm.Unit, itm.Shift));
                        }
                        var yearmonth = (DateTime)itm.AffilationDate;
                        itm.AcademicYearString = yearmonth.ToString("MMM yyyy");
                    }
                    res = res.Where(a => !cnt.Any(b=> b.Item1 == a.TradeItiId && b.Item2 == a.Unit && b.Item3 == a.Shift)).ToList();
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SeatAvailabilty> GetSeatAvailabilityListAdd(int loginId,string miscode)
        {
            try
            {
                int collegeId = _db.Staff_Institute_Detail.Where(x => x.Name == miscode).Select(y => y.InstituteId).FirstOrDefault();
                
                var res = (from aa in _db.tbl_iti_college_details
                           join bb in _db.tbl_division_master on aa.division_id equals bb.division_id
                           join cc in _db.tbl_district_master on aa.district_id equals cc.district_lgd_code
                           join dd in _db.tbl_taluk_master on cc.district_lgd_code equals dd.district_lgd_code
                           join ee in _db.tbl_Institute_type on aa.Insitute_TypeId equals ee.Institute_type_id
                           join ff in _db.tbl_ITI_Trade on aa.iti_college_id equals ff.ITICode
                           join gg in _db.tbl_trade_mast on ff.TradeCode equals gg.trade_id
                           join hh in _db.tbl_ITI_Trade_Shifts on ff.Trade_ITI_id equals hh.ITI_Trade_Id
                           join v in _db.tbl_course_type_mast on aa.CourseCode equals v.course_id
                           where aa.iti_college_id == collegeId && aa.taluk_id == dd.taluk_lgd_code && !_db.tbl_ITI_trade_seat_master.Any(sp =>sp.Trade_ITI_Id== ff.Trade_ITI_id && sp.ShiftId==hh.Shift && sp.UnitId==hh.Units && sp.Status!=5)
                           select new SeatAvailabilty
                           {
                               CourseTypeName=v.course_type_name,
                               AffilationDate = aa.AffiliationDate,
                               MISCode = aa.MISCode,
                               DivisionId = aa.division_id,
                               DivisionName = bb.division_name,
                               DistrictId = aa.district_id,
                               DistrictName = cc.district_ename,
                               TalukId = aa.taluk_id,
                               TalukName = dd.taluk_ename,
                               ITITypeName = ee.Institute_type,
                               ITIName = aa.iti_college_name,
                               TradeId = gg.trade_id,
                               TradeName = gg.trade_name,
                               Shift = hh.Shift,
                               Unit = hh.Units,
                               batchsize = gg.trade_seating,
                               DualSystemTraining = hh.Dual_System,
                               Duration=gg.trade_duration,
                               IsActive=hh.IsActive,
                           }).ToList();
               
                foreach (var itm in res)
                {
                    var yearmonth = (DateTime)itm.AffilationDate;
                    itm.AcademicYearString = yearmonth.ToString("MMM yyyy");
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SeatAvailabilty> GetSeatTypes()
        {
            try
            {
                var res = (from aa in _db.tbl_seat_type
                           select new SeatAvailabilty
                           {
                               SeatTypeId = aa.Seat_type_id,
                               SeatTypeName = aa.SeatType,
                           }).ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetSeatsByTradeIdSeatType(int tradeId)
        {
            try
            {
                var res = _db.tbl_trade_mast.Where(x => x.trade_id == tradeId).Select(y => y.trade_seating).FirstOrDefault();
                if (res != null)
                {
                    return Convert.ToInt32(res);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SeatAvailabilty> GetSeatsBySeatTypeRules(int seattypeId, int tradeId)
        {
            try
            {
                int MgmtSeats = 4;
                int ruleId = _db.Tbl_other_rules.Where(a => a.OtherRules.Contains("Management")).Select(a => a.Other_rules_id).FirstOrDefault();
                MgmtSeats = Convert.ToInt32(_db.tbl_other_rules_value.Where(a => a.Other_rules_id == ruleId).Select(a => a.RuleValue).FirstOrDefault());
                //var res = _db.tbl_seattype_master.Where(x => x.seattype_Id == seattypeId).Select(y => y.seattype_Id).FirstOrDefault();

                //var res = (from aa in _db.tbl_seattype_master.Where(x => x.seattype_Id == seattypeId)
                //           select new SeatAvailabilty
                //           {
                //               SeatTypeId = aa.seattype_Id,
                //               Govt_seats = aa.Govt_seats,
                //               Management_seats = aa.Management_seats,
                //               batchsize = aa.Govt_seats + aa.Management_seats
                //           }).ToList();
                int batchSize = 24;
                if (tradeId != 0)
                 batchSize = (int)_db.tbl_trade_mast.Where(x => x.trade_id == tradeId).Select(a => a.trade_seating).FirstOrDefault();
                var res = new List<Models.Admission.SeatAvailabilty>();
                switch (seattypeId)
                {
                    case 1: //GIA
                        res.Add(
                            new SeatAvailabilty()
                            {
                                batchsize = batchSize,
                                Govt_seats = batchSize - MgmtSeats,
                                Management_seats = MgmtSeats
                            });
                        break;
                    case 2: //Govt
                        res.Add(
                            new SeatAvailabilty()
                            {
                                batchsize = batchSize,
                                Govt_seats = batchSize,
                                Management_seats = 0
                            });
                        break;
                    case 3: //Partial PPP
                        res.Add(
                            new SeatAvailabilty()
                            {
                                batchsize = batchSize,
                                Govt_seats = batchSize - MgmtSeats,
                                Management_seats = MgmtSeats
                            });
                        break;
                    case 4: //Fully PPP
                        res.Add(
                        new SeatAvailabilty()
                        {
                            batchsize = batchSize,
                            Govt_seats = 0,
                            Management_seats = batchSize
                        });
                        break;
                    case 5: //Unaided
                        res.Add(
                            new SeatAvailabilty()
                            {
                                batchsize = batchSize,
                                Govt_seats = 0,
                                Management_seats = batchSize
                            });
                        break;
                }

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string SaveSeatAvailability(List<SeatDetails> seat, int loginId,int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (seat.Count != 0)
                    {
                        if (roleId == 9)
                        {
                        foreach (var item in seat)
                        {
                            int collegeId = GetCollegeId(loginId);
                            var itiid = _db.tbl_ITI_Trade.Where(x => x.TradeCode == item.TradeId && x.ITICode == collegeId && x.IsActive == true && (x.StatusId == (int)CmnClass.Status.Affiliated || x.StatusId == (int)CmnClass.Status.Published)).Select(y => y.Trade_ITI_id).FirstOrDefault();
                            var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_Id == itiid && x.UnitId == item.Unit && x.ShiftId == item.Shift /*&& x.SeatsTypeId == item.SeatType*/ && x.CourseType==item.CourseType).FirstOrDefault();
                            if (res == null)
                            {
                                tbl_ITI_trade_seat_master seatmast = new tbl_ITI_trade_seat_master();
                                seatmast.Trade_ITI_Id = itiid;
                                seatmast.UnitId = item.Unit;
                                seatmast.ShiftId = item.Shift;
                                seatmast.SeatsPerUnit = item.GovtSeatAvailability + item.ManagementSeatAvailability;
                                seatmast.SeatsTypeId = item.SeatType;
                                seatmast.IsPPP = false;
                                if (item.DualSystemTraining == "dual")
                                    seatmast.DualSystemTraining = true;
                                else
                                    seatmast.DualSystemTraining = false;
                                seatmast.CreatedOn = DateTime.Now;
                                seatmast.CreatedBy = loginId;
                                seatmast.Govt_Gia_seats = item.GovtSeatAvailability;
                                seatmast.PPP_seats = 0;
                                seatmast.Management_seats = item.ManagementSeatAvailability;
                                seatmast.Status = item.Status;
                                seatmast.Remarks = item.Remarks;
                                seatmast.FlowId = item.RoleId;
                                seatmast.CourseType = item.CourseType;
                                seatmast.AcademicYear = item.AcademicYear;
                                    seatmast.IsActive = true;
                                _db.tbl_ITI_trade_seat_master.Add(seatmast);
                                _db.SaveChanges();

                                int rr = _db.tbl_ITI_trade_seat_master.OrderByDescending(y => y.Trade_ITI_seat_Id).Select(x => x.Trade_ITI_seat_Id).FirstOrDefault();

                                tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                                seattrans.Trade_ITI_seat_Id = rr;
                                seattrans.Trans_Date = DateTime.Now;
                                seattrans.StatusId = item.Status;
                                seattrans.Remarks = item.Remarks;
                                seattrans.CreatedOn = DateTime.Now;
                                seattrans.CreatedBy = roleId;
                                seattrans.FlowId = item.RoleId;
                                _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                                _db.SaveChanges();
                            }
                            else
                            {
                                 //var res1 = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == item.seatId).FirstOrDefault();
                                 res.FlowId = item.RoleId;
                                 res.Status = 1;
                                 res.SeatsTypeId = item.SeatType;
                                 res.Govt_Gia_seats = item.GovtSeatAvailability;                                 
                                 res.Management_seats = item.ManagementSeatAvailability;
                                //res.UnitId = item.Unit;
                                //res.ShiftId = item.Shift;
                                //res.SeatsTypeId = item.SeatType;
                                //int rr = _db.tbl_ITI_trade_seat_master.OrderByDescending(y => y.Trade_ITI_seat_Id).Select(x => x.Trade_ITI_seat_Id).FirstOrDefault();
                                //tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                                //seattrans.Trade_ITI_seat_Id = rr;
                                //seattrans.Trans_Date = DateTime.Now;
                                //seattrans.StatusId = item.Status;
                                //seattrans.Remarks = item.Remarks;
                                //seattrans.CreatedOn = DateTime.Now;
                                //seattrans.CreatedBy = roleId;
                                //seattrans.FlowId = item.RoleId;
                                //_db.tbl_ITI_trade_seat_trans.Add(seattrans);
                                tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                                seattrans.Trade_ITI_seat_Id = res.Trade_ITI_seat_Id;
                                seattrans.Trans_Date = DateTime.Now;
                                seattrans.StatusId = item.Status;
                                seattrans.Remarks = item.Remarks;
                                seattrans.CreatedOn = DateTime.Now;
                                seattrans.CreatedBy = roleId;
                                seattrans.FlowId = item.RoleId;
                                _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                                _db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in seat)
                            {
                                //int collegeId = GetCollegeId(loginId);
                                int collegeId = _db.Staff_Institute_Detail.Where(x => x.Name == item.MISCode).Select(y => y.InstituteId).FirstOrDefault();
                                var itiid = _db.tbl_ITI_Trade.Where(x => x.TradeCode == item.TradeId && x.ITICode == collegeId).Select(y => y.Trade_ITI_id).FirstOrDefault();
                                var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_Id == itiid && x.UnitId == item.Unit && x.ShiftId == item.Shift /*&& x.SeatsTypeId == item.SeatType*/ && x.CourseType == item.CourseType).FirstOrDefault();
                                if (res == null)
                                {
                                    tbl_ITI_trade_seat_master seatmast = new tbl_ITI_trade_seat_master();
                                    seatmast.Trade_ITI_Id = itiid;
                                    seatmast.UnitId = item.Unit;
                                    seatmast.ShiftId = item.Shift;
                                    seatmast.SeatsPerUnit = item.GovtSeatAvailability + item.ManagementSeatAvailability;
                                    seatmast.SeatsTypeId = item.SeatType;
                                    seatmast.IsPPP = false;
                                    if (item.DualSystemTraining == "dual")
                                        seatmast.DualSystemTraining = true;
                                    else
                                        seatmast.DualSystemTraining = false;
                                    seatmast.CreatedOn = DateTime.Now;
                                    seatmast.CreatedBy = loginId;
                                    seatmast.Govt_Gia_seats = item.GovtSeatAvailability;
                                    seatmast.PPP_seats = 0;
                                    seatmast.Management_seats = item.ManagementSeatAvailability;
                                    seatmast.Status = 4;
                                    seatmast.Remarks = item.Remarks;
                                    //seatmast.FlowId = item.RoleId;
                                    seatmast.FlowId = roleId;
                                    seatmast.CourseType = item.CourseType;
                                    seatmast.AcademicYear = item.AcademicYear;
                                    seatmast.IsActive = true;
                                    _db.tbl_ITI_trade_seat_master.Add(seatmast);
                                    _db.SaveChanges();

                                    int rr = _db.tbl_ITI_trade_seat_master.OrderByDescending(y => y.Trade_ITI_seat_Id).Select(x => x.Trade_ITI_seat_Id).FirstOrDefault();

                                    tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                                    seattrans.Trade_ITI_seat_Id = rr;
                                    seattrans.Trans_Date = DateTime.Now;
                                    seattrans.StatusId = 4;
                                    seattrans.Remarks = item.Remarks;
                                    seattrans.CreatedOn = DateTime.Now;
                                    seattrans.CreatedBy = roleId;
                                    //seattrans.FlowId = item.RoleId;
                                    seattrans.FlowId = roleId;
                                    _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                                    _db.SaveChanges();
                                }
                                else
                                {
                                    res.FlowId = item.RoleId;
                                    res.Status = 1;
                                    res.SeatsTypeId = item.SeatType;
                                    res.Govt_Gia_seats = item.GovtSeatAvailability;
                                    res.Management_seats = item.ManagementSeatAvailability;
                                    tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                                    seattrans.Trade_ITI_seat_Id = res.Trade_ITI_seat_Id;
                                    seattrans.Trans_Date = DateTime.Now;
                                    seattrans.StatusId = item.Status;
                                    seattrans.Remarks = item.Remarks;
                                    seattrans.CreatedOn = DateTime.Now;
                                    seattrans.CreatedBy = roleId;
                                    seattrans.FlowId = item.RoleId;
                                    _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                                    _db.SaveChanges();
                                }
                            }
                        }
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

        public bool ForwardSeatAvailability(List<SeatDetails> seat, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var itm in seat)
                    {
                        var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == itm.TradeId).FirstOrDefault();
                        if (itm.RoleId == 0)
                            res.FlowId = 9;
                        else
                            res.FlowId = itm.RoleId;
                        res.Status = itm.Status;

                        tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                        seattrans.Trade_ITI_seat_Id = itm.TradeId;
                        seattrans.Trans_Date = DateTime.Now;
                        seattrans.StatusId = itm.Status;
                        if (itm.Remarks != null)
                            seattrans.Remarks = itm.Remarks;
                        seattrans.CreatedOn = DateTime.Now;
                        seattrans.CreatedBy = roleId;
                        if(itm.Status==5 ||itm.Status==3)
                            seattrans.FlowId = 9;
                        else
                            seattrans.FlowId = itm.RoleId;
                        _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                        _db.SaveChanges();
                    }
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
        public List<SeatDetails> GetRemarks(int seatId)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_trade_seat_trans
                           join bb in _db.tbl_SeatAvail_status_master on aa.StatusId equals bb.StatusId
                           join cc in _db.tbl_role_master on aa.CreatedBy equals cc.role_id
                           join dd in _db.tbl_role_master on aa.FlowId equals dd.role_id
                           where aa.Trade_ITI_seat_Id == seatId 
                           orderby aa.CreatedOn descending
                           select new SeatDetails
                           {
                               StatusName = bb.StatusName,
                               Remarks = aa.Remarks,
                               From = cc.role_description,                               
                               Date = aa.CreatedOn.ToString(),
                               FlowId=aa.FlowId
                           }).ToList();

                var rss=(from a in res
                        join b in _db.tbl_role_master on a.FlowId equals b.role_id
                        select new SeatDetails
                        {
                            StatusName = a.StatusName,
                            Remarks = a.Remarks,
                            From = a.From,
                            Date = a.Date,
                            To = b.role_description
                        }).ToList();

                return rss;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool ApproveSeatAvailability(List<SeatDetails> seat, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var itm in seat)
                    {
                        var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == itm.TradeId).FirstOrDefault();
                        //itm.RoleId;
                        if (itm.Status == 4)
                            res.FlowId = roleId;                      
                        else
                            res.FlowId = 9;

                        res.Status = itm.Status;

                        tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                        seattrans.Trade_ITI_seat_Id = itm.TradeId;
                        seattrans.Trans_Date = DateTime.Now;
                        seattrans.StatusId = itm.Status;
                        if (itm.Remarks != null)
                            seattrans.Remarks = itm.Remarks;

                        seattrans.CreatedOn = DateTime.Now;
                        seattrans.CreatedBy = roleId;
                        //seattrans.FlowId = 9;
                        if (itm.Status == 4)
                            seattrans.FlowId = roleId;
                        else
                        seattrans.FlowId = 9;

                        _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                        _db.SaveChanges();
                    }
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
        //public bool RejectSeatAvailability(List<SeatDetails> seat, int loginId)
        //{
        //    using (var transaction = _db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            foreach (var itm in seat)
        //            {
        //                var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == itm.TradeId).FirstOrDefault();
        //                res.Status = 5;
        //                res.FlowId = itm.RoleId;

        //                tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
        //                seattrans.Trade_ITI_seat_Id = itm.TradeId;
        //                seattrans.Trans_Date = DateTime.Now;
        //                seattrans.StatusId = 5;
        //                seattrans.Remarks = itm.Remarks;
        //                seattrans.CreatedOn = DateTime.Now;
        //                seattrans.CreatedBy = loginId;
        //                seattrans.FlowId = itm.RoleId;
        //                _db.tbl_ITI_trade_seat_trans.Add(seattrans);
        //                _db.SaveChanges();
        //            }
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            transaction.Rollback();
        //            throw e;
        //        }
        //    }
        //}
        public List<SeatAvailabilty> GetSeatViewDetails(int seatId)
        {
            try
            {
                var res = (from aa in _db.tbl_ITI_trade_seat_master
                           join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                           join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                           join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                           join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                           join gg in _db.tbl_location_type on ee.location_id equals gg.location_id
                           join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                           join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                           join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                           join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                           join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                           join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                           where aa.Trade_ITI_seat_Id == seatId && ee.taluk_id == ii.taluk_lgd_code
                           select new SeatAvailabilty
                           {
                               Id = aa.Trade_ITI_seat_Id,
                               MISCode = ee.MISCode,
                               DivisionId = ee.location_id,
                               DivisionName = gg.location_name,
                               DistrictId = ee.district_id,
                               DistrictName = hh.district_ename,
                               TalukId = ee.taluk_id,
                               TalukName = ii.taluk_ename,
                               ITITypeName = ff.Institute_type,
                               ITIName = ee.iti_college_name,
                               TradeId = dd.trade_id,
                               TradeName = dd.trade_name,
                               Shift = aa.ShiftId,
                               Unit = aa.UnitId,
                               DualSystem = aa.DualSystemTraining,
                               GovtSeatAvailability = aa.Govt_Gia_seats,
                               ManagementSeatAvailability = aa.Management_seats,
                               Status = kk.StatusName,
                               Remarks = aa.Remarks,
                               FlowId = aa.FlowId,
                               CourseType = aa.CourseType,
                               AcademicYear = aa.AcademicYear,
                               SeatTypeId = aa.SeatsTypeId,
                               SeatName = cd.SeatType,
                               CourseTypeName = jj.course_type_name,
                               Duration=dd.trade_duration,
                               batchsize= aa.Govt_Gia_seats + aa.Management_seats,

                           }).Take(1).ToList();

                foreach (var item in res)
                {
                    item.AcademicYearString = item.AcademicYear.ToString("MMM yyyy");
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool GetdelUnitShiftDetails(int seatId, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //foreach (var itm in seat)
                    //{
                        //var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == seatId).FirstOrDefault();
                    var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == seatId).FirstOrDefault();
                        //if (roleId == 0)
                            res.FlowId = 9;
                        //else
                        //    res.FlowId = roleId;
                        res.Status = 5;

                    tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                    seattrans.Trade_ITI_seat_Id = res.Trade_ITI_seat_Id;
                    seattrans.Trans_Date = DateTime.Now;
                    seattrans.StatusId = 5;
                    seattrans.Remarks = "Rejected";
                    seattrans.CreatedOn = DateTime.Now;
                    seattrans.CreatedBy = roleId;
                    seattrans.FlowId = roleId;
                    _db.tbl_ITI_trade_seat_trans.Add(seattrans);                  
                        //_db.tbl_ITI_trade_seat_master.Remove(res);
                    _db.SaveChanges();
                   
                    //}
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
                
        }
        public bool GetdeActiveSeatDetails(int seatId,int TradeItiId, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //var res = _db.tbl_ITI_Trade.Where(x => x.Trade_ITI_id == TradeItiId).FirstOrDefault();
                    //res.IsActive = false;

                    var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == seatId).FirstOrDefault();
                    res.FlowId = roleId;
                    res.Status = 4;
                    res.IsActive = false;

                    tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                    seattrans.Trade_ITI_seat_Id = res.Trade_ITI_seat_Id;
                    seattrans.Trans_Date = DateTime.Now;
                    seattrans.StatusId = 4;
                    seattrans.Remarks = "DeActivated";
                    seattrans.CreatedOn = DateTime.Now;
                    seattrans.CreatedBy = roleId;
                    seattrans.FlowId = roleId;
                    _db.tbl_ITI_trade_seat_trans.Add(seattrans);

                    _db.SaveChanges();

                    
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public bool UpdateSeatAvailability(SeatDetails seat, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (seat != null)
                    {
                        var res = _db.tbl_ITI_trade_seat_master.Where(x => x.Trade_ITI_seat_Id == seat.TradeId).FirstOrDefault();
                        if (seat.IsChecked == true)
                        {
                            res.SeatsPerUnit = seat.GovtSeatAvailability + seat.ManagementSeatAvailability;
                            res.SeatsTypeId = seat.SeatType;
                            res.ModifiedOn = DateTime.Now;
                            res.ModifedBy = loginId;
                            res.Govt_Gia_seats = seat.GovtSeatAvailability;
                            res.Management_seats = seat.ManagementSeatAvailability;
                            if (roleId == 9)
                            res.Status = seat.Status;
                            else
                                res.Status = 4;
                            if(seat.Remarks!=null)
                                res.Remarks = seat.Remarks;
                            if (roleId == 9)
                            res.FlowId = seat.RoleId;
                            else
                                res.FlowId = roleId;
                            res.CourseType = seat.CourseType;
                            res.AcademicYear = seat.AcademicYear;

                            tbl_ITI_trade_seat_trans seattrans = new tbl_ITI_trade_seat_trans();
                            seattrans.Trade_ITI_seat_Id = res.Trade_ITI_seat_Id;
                            seattrans.Trans_Date = DateTime.Now;
                            if (roleId == 9)
                            seattrans.StatusId = seat.Status;
                            else
                                seattrans.StatusId = 4;
                            if (seat.Remarks != null)
                                seattrans.Remarks = seat.Remarks;
                            seattrans.CreatedOn = DateTime.Now;
                            seattrans.CreatedBy = roleId;
                            if (roleId == 9)
                            seattrans.FlowId = seat.RoleId;
                            else
                                seattrans.FlowId = roleId;
                            _db.tbl_ITI_trade_seat_trans.Add(seattrans);
                        }
                        else
                        {
                            _db.tbl_ITI_trade_seat_master.Remove(res);
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
        public List<DistrictTalukDetails> GetRegionDistrictCities(int loginId)
        {
            try
            {
                int div_id = Convert.ToInt32(_db.tbl_user_master.Where(a => a.um_id == loginId).Select(a => a.um_div_id).FirstOrDefault());

                var res = (from aa in _db.tbl_district_master
                           where aa.dis_is_active == true && div_id != 0 ? aa.division_id == div_id : true
                           select new DistrictTalukDetails
                           {
                               CityId = aa.district_lgd_code,
                               CityName = aa.district_ename
                           }).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DistrictTalukDetails> GetTaluks(int distilgdCOde)
        {
            try
            {
                var res = (from aa in _db.tbl_taluk_master
                           where aa.taluk_is_active == true && aa.district_lgd_code == distilgdCOde
                           select new DistrictTalukDetails
                           {
                               CityId = aa.taluk_lgd_code,
                               CityName = aa.taluk_ename
                           }).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DistrictTalukDetails> GetInstitutes(int distilgdCOde)
        {
            try
            {
                var res = (from aa in _db.tbl_iti_college_details
                           where aa.is_active == true && aa.taluk_id == distilgdCOde
                           select new DistrictTalukDetails
                           {
                               CityId = aa.iti_college_id,
                               CityName = aa.iti_college_name
                           }).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SeatAvailabilty> GetSeatAvailabilityListStatusFilter(int TabId, int Course_Id, int Year_id, int roleId, int loginId, int Division_Id, int District_Id, int taluk_id, int Insttype_Id)
        {
            try
            {
                //int year = 0;
                if (Year_id != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == Year_id).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                    Year_id = Convert.ToInt32(yr);
                }

                if (TabId == 0)
                {
                    //if (courseType == 0)
                    //{
                    if (roleId == 9)
                    {
                        int collegeId = GetCollegeId(loginId);
                        var res = (from aa in _db.tbl_ITI_trade_seat_master
                                   join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                                   join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                                   join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                                   join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                                   join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
                                   join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                                   join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                                   join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                                   join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                                   join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                                   join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id                                                                      
                                   where ee.iti_college_id == collegeId && aa.CreatedBy == loginId && ee.taluk_id == ii.taluk_lgd_code
                                   && aa.Status != 5 && (Year_id != 0 ? aa.AcademicYear.Year == Year_id : true)
                                   && (Course_Id != 0 ? aa.CourseType == Course_Id : true)
                                   && (Division_Id != 0 ? gg.division_id == Division_Id : true) && (District_Id != 0 ? hh.district_lgd_code == District_Id : true)
                                   && (taluk_id != 0 ? ii.taluk_id == taluk_id : true) && (Insttype_Id != 0 ? ee.iti_college_id == Insttype_Id : true)
                                   select new SeatAvailabilty
                                   {
                                       Id = aa.Trade_ITI_seat_Id,
                                       MISCode = ee.MISCode,
                                       DivisionId = ee.division_id,
                                       DivisionName = gg.division_name,
                                       DistrictId = ee.district_id,
                                       DistrictName = hh.district_ename,
                                       TalukId = ee.taluk_id,
                                       TalukName = ii.taluk_ename,
                                       ITIType = ee.Insitute_TypeId,
                                       ITITypeName = ff.Institute_type,
                                       ITIName = ee.iti_college_name,
                                       TradeId = dd.trade_id,
                                       TradeName = dd.trade_name,
                                       Shift = aa.ShiftId,
                                       Unit = aa.UnitId,
                                       DualSystem = aa.DualSystemTraining,
                                       GovtSeatAvailability = aa.Govt_Gia_seats,
                                       ManagementSeatAvailability = aa.Management_seats,
                                       Status = kk.StatusName ,
                                       StatusId =aa.Status,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       FlowId = aa.FlowId,
                                       AcademicYear = aa.AcademicYear,
                                       CourseTypeName = jj.course_type_name,
                                       SeatName = cd.SeatType,
                                       Duration = dd.trade_duration,
                                       batchsize= aa.Management_seats + aa.Govt_Gia_seats
                                   }).Distinct().ToList();

                        foreach (var itm in res)
                        {
                            var desc = _db.tbl_role_master.Where(x => x.role_id == itm.FlowId).Select(y => y.role_DescShortForm).FirstOrDefault();
                            if (itm.StatusId == 1)
                                itm.Status = "Submitted for review"+"-"+desc;

                            if (itm.StatusId == 2)
                                itm.Status = "Reviewed and Recommended" + "-" + desc;

                            if (itm.StatusId == 3)
                                itm.Status = "Send for correction/clarification " + "-" + desc;

                            if (itm.DualSystem == true)
                                itm.DualSystemTraining = "dual";
                            else
                                itm.DualSystemTraining = "regular";

                            //itm.batchsize = itm.GovtSeatAvailability + itm.ManagementSeatAvailability;

                            itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
                        }
                        return res;
                    }
                    else if (roleId == 5)
                    {
                        var res = (from aa in _db.tbl_ITI_trade_seat_master
                                   join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                                   join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                                   join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                                   join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                                   join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
                                   join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                                   join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                                   join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                                   join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                                   join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                                   join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                                   where ee.taluk_id == ii.taluk_lgd_code && aa.Status != 4 && (Year_id != 0 ? aa.AcademicYear.Year == Year_id : true)   /*&& (TabId == 1 ? aa.Status == 4 : true)*/
                                   && (Course_Id != 0 ? aa.CourseType == Course_Id : true)
                                   && (Division_Id != 0 ? ee.division_id == Division_Id : true) && (District_Id != 0 ? hh.district_lgd_code == District_Id : true)
                                   && (taluk_id != 0 ? ee.taluk_id == taluk_id : true) && (Insttype_Id != 0 ? ee.iti_college_id == Insttype_Id : true)                                               
                                   select new SeatAvailabilty
                                   {
                                       Id = aa.Trade_ITI_seat_Id,
                                       MISCode = ee.MISCode,
                                       DivisionId = ee.division_id,
                                       DivisionName = gg.division_name,
                                       DistrictId = ee.district_id,
                                       DistrictName = hh.district_ename,
                                       TalukId = ee.taluk_id,
                                       TalukName = ii.taluk_ename,
                                       ITIType = ee.Insitute_TypeId,
                                       ITITypeName = ff.Institute_type,
                                       ITIName = ee.iti_college_name,
                                       TradeId = dd.trade_id,
                                       TradeName = dd.trade_name,
                                       Shift = aa.ShiftId,
                                       Unit = aa.UnitId,
                                       DualSystem = aa.DualSystemTraining,
                                       GovtSeatAvailability = aa.Govt_Gia_seats,
                                       ManagementSeatAvailability = aa.Management_seats,
                                       Status = kk.StatusName,
                                       StatusId = aa.Status,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       FlowId = aa.FlowId,
                                       AcademicYear = aa.AcademicYear,
                                       CourseTypeName = jj.course_type_name,
                                       SeatName = cd.SeatType,
                                       Duration = dd.trade_duration,
                                       batchsize = aa.Management_seats + aa.Govt_Gia_seats
                                   }).Distinct().ToList();

                        foreach (var itm in res)
                        {
                            var desc = _db.tbl_role_master.Where(x => x.role_id == itm.FlowId).Select(y => y.role_DescShortForm).FirstOrDefault();

                            if (itm.StatusId == 1)
                                itm.Status = "Submitted for review" + "-" + desc;

                            if (itm.StatusId == 2)
                                itm.Status = "Reviewed and Recommended" + "-" + desc;

                            if (itm.StatusId == 3)
                                itm.Status = "Send for correction/clarification " + "-" + desc;

                            if (itm.DualSystem == true)
                                itm.DualSystemTraining = "dual";
                            else
                                itm.DualSystemTraining = "regular";

                            itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
                        }
                        return res;
                    }
                    else
                    {
                        if (Division_Id == 0)
                        {
                            Division_Id = Convert.ToInt32(_db.tbl_user_master.Where(a=> a.um_id == loginId).Select(a=> a.um_div_id).First());
                        }
                        var res = (from aa in _db.tbl_ITI_trade_seat_master
                                   join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                                   join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                                   join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                                   join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                                   join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
                                   join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                                   join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                                   join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                                   join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                                   join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                                   join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                                   where ee.taluk_id == ii.taluk_lgd_code && aa.Status != 4 && (Year_id != 0 ? aa.AcademicYear.Year == Year_id : true)
                                   && (Course_Id != 0 ? aa.CourseType == Course_Id : true)
                                   && (Division_Id != 0 ? gg.division_id == Division_Id : true) && (District_Id != 0 ? hh.district_lgd_code == District_Id : true)
                                   && (taluk_id != 0 ? ee.taluk_id == taluk_id : true) && (Insttype_Id != 0 ? ee.iti_college_id == Insttype_Id : true)
                                   select new SeatAvailabilty
                                   {
                                       Id = aa.Trade_ITI_seat_Id,
                                       MISCode = ee.MISCode,
                                       DivisionId = ee.division_id,
                                       DivisionName = gg.division_name,
                                       DistrictId = ee.district_id,
                                       DistrictName = hh.district_ename,
                                       TalukId = ee.taluk_id,
                                       TalukName = ii.taluk_ename,
                                       ITIType = ee.Insitute_TypeId,
                                       ITITypeName = ff.Institute_type,
                                       ITIName = ee.iti_college_name,
                                       TradeId = dd.trade_id,
                                       TradeName = dd.trade_name,
                                       Shift = aa.ShiftId,
                                       Unit = aa.UnitId,
                                       DualSystem = aa.DualSystemTraining,
                                       GovtSeatAvailability = aa.Govt_Gia_seats,
                                       ManagementSeatAvailability = aa.Management_seats,
                                       Status = kk.StatusName,
                                       StatusId = aa.Status,
                                       Remarks = aa.Remarks,
                                       RoleId = roleId,
                                       FlowId = aa.FlowId,
                                       AcademicYear = aa.AcademicYear,
                                       CourseTypeName = jj.course_type_name,
                                       SeatName = cd.SeatType,
                                       Duration = dd.trade_duration,
                                       batchsize = aa.Management_seats + aa.Govt_Gia_seats
                                   }).Distinct().ToList();
                        foreach (var itm in res)
                        {
                            var desc = _db.tbl_role_master.Where(x => x.role_id == itm.FlowId).Select(y => y.role_DescShortForm).FirstOrDefault();
                            if (itm.StatusId == 1)
                                itm.Status = "Submitted for review" + "-" + desc;

                            if (itm.StatusId == 2)
                                itm.Status = "Reviewed and Recommended" + "-" + desc;

                            if (itm.StatusId == 3)
                                itm.Status = "Send for correction/clarification " + "-" + desc;

                            if (itm.DualSystem == true)
                                itm.DualSystemTraining = "dual";
                            else
                                itm.DualSystemTraining = "regular";
                            itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
                        }
                        if (loginId == 0 && roleId == 0)
                        {
                            res = res.Where(a => a.StatusId == (int)Models.CmnClass.Status.seatavailabilityApprovePublish).ToList();
                        }
                        return res;
                    }
                    //}
                    //else
                    //{
                    //    var res = (from aa in _db.tbl_ITI_trade_seat_master
                    //               join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                    //               join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                    //               join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                    //               join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                    //               join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
                    //               join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                    //               join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                    //               join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                    //               join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                    //               join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                    //               join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                    //               where ee.taluk_id == ii.taluk_lgd_code && aa.CourseType == courseType
                    //               select new SeatAvailabilty
                    //               {
                    //                   Id = aa.Trade_ITI_seat_Id,
                    //                   MISCode = ee.MISCode,
                    //                   DivisionId = ee.division_id,
                    //                   DivisionName = gg.division_name,
                    //                   DistrictId = ee.district_id,
                    //                   DistrictName = hh.district_ename,
                    //                   TalukId = ee.taluk_id,
                    //                   TalukName = ii.taluk_ename,
                    //                   ITIType = ee.Insitute_TypeId,
                    //                   ITITypeName = ff.Institute_type,
                    //                   ITIName = ee.iti_college_name,
                    //                   TradeId = dd.trade_id,
                    //                   TradeName = dd.trade_name,
                    //                   Shift = aa.ShiftId,
                    //                   Unit = aa.UnitId,
                    //                   DualSystem = aa.DualSystemTraining,
                    //                   GovtSeatAvailability = aa.Govt_Gia_seats,
                    //                   ManagementSeatAvailability = aa.Management_seats,
                    //                   Status = kk.StatusName,
                    //                   StatusId = aa.Status,
                    //                   Remarks = aa.Remarks,
                    //                   RoleId = roleId,
                    //                   FlowId = aa.FlowId,
                    //                   AcademicYear = aa.AcademicYear,
                    //                   CourseTypeName = jj.course_type_name,
                    //                   SeatName = cd.SeatType,
                    //                   Duration = dd.trade_duration,
                    //                   batchsize = aa.Management_seats + aa.Govt_Gia_seats
                    //               }).Distinct().ToList();

                    //    foreach (var itm in res)
                    //    {
                    //        var desc = _db.tbl_role_master.Where(x => x.role_id == itm.FlowId).Select(y => y.role_DescShortForm).FirstOrDefault();
                    //        if (itm.StatusId == 1)
                    //            itm.Status = "Submitted for review" + "-" + desc;

                    //        if (itm.StatusId == 2)
                    //            itm.Status = "Reviewed and Recommended" + "-" + desc;

                    //        if (itm.StatusId == 3)
                    //            itm.Status = "Send for correction/clarification " + "-" + desc;

                    //        if (itm.DualSystem == true)
                    //            itm.DualSystemTraining = "dual";
                    //        else
                    //            itm.DualSystemTraining = "regular";

                    //        itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
                    //    }
                    //    string yr = _db.tbl_Year.Where(x => x.YearID == academicYear).Select(y => y.Year).FirstOrDefault();
                    //    var res1 = res.Where(c => Convert.ToInt32(c.AcademicYearString) == Convert.ToInt32(yr)).ToList();
                    //    return res1;
                    //}
                }
                else
                {
                    if (roleId == (int)CmnClass.Role.JDDiv && Division_Id == 0)
                    {
                        Division_Id = Convert.ToInt32(_db.tbl_user_master.Where(a => a.um_id == loginId).Select(a => a.um_div_id).First());
                    }
                    var res = (from aa in _db.tbl_ITI_trade_seat_master
                               join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
                               join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
                               join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
                               join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
                               join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
                               join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
                               join ii in _db.tbl_taluk_master on ee.district_id equals ii.district_lgd_code
                               join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
                               join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
                               join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
                               join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
                               where ee.taluk_id == ii.taluk_lgd_code && aa.Status == 4 && (Year_id != 0 ? aa.AcademicYear.Year == Year_id : true)/*&& aa.Status == 6*/
                                   && (Course_Id != 0 ? aa.CourseType == Course_Id : true)
                                   && (Division_Id != 0 ? gg.division_id == Division_Id : true) && (District_Id != 0 ? hh.district_lgd_code == District_Id : true)
                                   && (taluk_id != 0 ? ee.taluk_id == taluk_id : true) && (Insttype_Id != 0 ? ee.iti_college_id == Insttype_Id : true)
                               select new SeatAvailabilty
                               {
                                   Id = aa.Trade_ITI_seat_Id,
                                   TradeItiId = aa.Trade_ITI_Id,
                                   MISCode = ee.MISCode,
                                   DivisionId = ee.division_id,
                                   DivisionName = gg.division_name,
                                   DistrictId = ee.district_id,
                                   DistrictName = hh.district_ename,
                                   TalukId = ee.taluk_id,
                                   TalukName = ii.taluk_ename,
                                   ITIType = ee.Insitute_TypeId,
                                   ITITypeName=ff.Institute_type,
                                   ITIName = ee.iti_college_name,
                                   TradeId = dd.trade_id,
                                   TradeName = dd.trade_name,
                                   Shift = aa.ShiftId,
                                   Unit = aa.UnitId,
                                   DualSystem = aa.DualSystemTraining,
                                   GovtSeatAvailability = aa.Govt_Gia_seats,
                                   ManagementSeatAvailability = aa.Management_seats,
                                   Status = kk.StatusName,
                                   StatusId = aa.Status,
                                   Remarks = aa.Remarks,
                                   RoleId = roleId,
                                   FlowId = aa.FlowId,
                                   AcademicYear = aa.AcademicYear,
                                   CourseTypeName = jj.course_type_name,
                                   SeatName = cd.SeatType,
                                   Duration = dd.trade_duration,
                                   batchsize = aa.Management_seats + aa.Govt_Gia_seats,
                                   IsActive = aa.IsActive
                               }).Distinct().ToList();

                    foreach (var itm in res)
                    {
                        var desc = _db.tbl_role_master.Where(x => x.role_id == itm.FlowId).Select(y => y.role_DescShortForm).FirstOrDefault();
                        if (itm.StatusId == 1)
                            itm.Status = "Submitted for review" + "-" + desc;

                        if (itm.StatusId == 2)
                            itm.Status = "Reviewed and Recommended" + "-" + desc;

                        if (itm.StatusId == 3)
                            itm.Status = "Send for correction/clarification " + "-" + desc;

                        if (itm.DualSystem == true)
                            itm.DualSystemTraining = "dual";
                        else
                            itm.DualSystemTraining = "regular";

                        itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
                    }
                    //string yr = _db.tbl_Year.Where(x => x.YearID == Year_id).Select(y => y.Year).FirstOrDefault();
                    //var res1 = res.Where(c => Convert.ToInt32(c.AcademicYearString) == Convert.ToInt32(yr)).ToList();
 
                    //var res1 = res.Where(c => Convert.ToInt32(c.Origyear) == Convert.ToInt32(yr)).ToList();
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

        //public List<SeatAvailabilty> GetIndexseatavailabilityList()
        //{
        //    try
        //    {
        //        var res = (from aa in _db.tbl_ITI_trade_seat_master
        //                   join kk in _db.tbl_SeatAvail_status_master on aa.Status equals kk.StatusId
        //                   join bb in _db.tbl_ITI_Trade_Shifts on aa.Trade_ITI_Id equals bb.ITI_Trade_Id
        //                   join cc in _db.tbl_ITI_Trade on bb.ITI_Trade_Id equals cc.Trade_ITI_id
        //                   join dd in _db.tbl_trade_mast on cc.TradeCode equals dd.trade_id
        //                   join ee in _db.tbl_iti_college_details on cc.ITICode equals ee.iti_college_id
        //                   join gg in _db.tbl_division_master on ee.division_id equals gg.division_id
        //                   join hh in _db.tbl_district_master on ee.district_id equals hh.district_lgd_code
        //                   join ii in _db.tbl_taluk_master on ee.taluk_id equals ii.taluk_lgd_code
        //                   join ff in _db.tbl_Institute_type on ee.Insitute_TypeId equals ff.Institute_type_id
        //                   join jj in _db.tbl_course_type_mast on aa.CourseType equals jj.course_id
        //                   join cd in _db.tbl_seat_type on aa.SeatsTypeId equals cd.Seat_type_id
        //                   where aa.Status== (int)Models.CmnClass.Status.seatavailabilityApprovePublish
        //                   select new SeatAvailabilty
        //                   {
        //                       Id = aa.Trade_ITI_seat_Id,
        //                       MISCode = ee.MISCode,
        //                       DivisionId = ee.division_id,
        //                       DivisionName = gg.division_name,
        //                       DistrictId = ee.district_id,
        //                       DistrictName = hh.district_ename,
        //                       TalukId = ee.taluk_id,
        //                       TalukName = ii.taluk_ename,
        //                       ITIType = ee.Insitute_TypeId,
        //                       ITITypeName = ff.Institute_type,
        //                       ITIName = ee.iti_college_name,
        //                       TradeId = dd.trade_id,
        //                       TradeName = dd.trade_name,
        //                       Shift = aa.ShiftId,
        //                       Unit = aa.UnitId,
        //                       DualSystem = aa.DualSystemTraining,
        //                       GovtSeatAvailability = aa.Govt_Gia_seats,
        //                       ManagementSeatAvailability = aa.Management_seats,
        //                       Status = kk.StatusName,
        //                       StatusId = aa.Status,
        //                       Remarks = aa.Remarks,
        //                       //RoleId = roleId,
        //                       FlowId = aa.FlowId,
        //                       AcademicYear = aa.AcademicYear,
        //                       CourseTypeName = jj.course_type_name,
        //                       SeatName = cd.SeatType,
        //                       Duration = dd.trade_duration,
        //                       batchsize = aa.Management_seats + aa.Govt_Gia_seats
        //                   }).Distinct().ToList();

        //        foreach (var itm in res)
        //        {                   
        //            itm.AcademicYearString = itm.AcademicYear.ToString("yyyy");
        //        }               
        //        return res;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
