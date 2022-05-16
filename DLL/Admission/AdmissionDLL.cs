using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.Admission;
using Models.Admin;
using Models.ExamNotification;
using DLL.DBConnection;
using Models.Master;
using Models.AdmissionModel;
using System.Globalization;
using System.Web;
using System.IO;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;

namespace DLL.Admission
{
    public class AdmissionDLL : IAdmissionDLL
    {
        private readonly DbConnection _db = new DbConnection();

        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        string SPNameFetchGrievanceDocDetails = ConfigurationManager.AppSettings["SPNameFetchGrievanceDocDetails"];

        //private readonly Random _random = new Random();
        private static Random random = new Random();

        //Get Course List
        public List<SeatAvailability_DD> GetCourseListDLL()
        {
            var res = (from a in _db.tbl_course_type_mast
                       where a.is_active == true
                       select new SeatAvailability_DD
                       {
                           Course_id = a.course_id,
                           Course_type_name = a.course_type_name
                       }).ToList();
            return res;
        }
        public List<SeatAvailability_DD> GetDivisionListDLL()
        {
            var res = (from a in _db.tbl_division_mast
                       where a.is_active == true
                       select new SeatAvailability_DD
                       {
                           Division_id = a.division_id,
                           Division_name = a.division_name
                       }).ToList();
            return res;
        }
        public List<SeatAvailability_DD> GetDistrictListDLL(int divId)
        {
            var res = (from a in _db.tbl_district_mast
                       where a.is_active == true && a.division_id == divId
                       select new SeatAvailability_DD
                       {
                           district_id = a.dist_id,
                           District_dist_name = a.dist_name
                       }).ToList();
            return res;
        }
        public List<SeatAvailability_DD> GetTalukListDLL(int distId)
        {
            var res = (from a in _db.tbl_taluk_mast
                       where a.is_active == true && a.district_id == distId
                       select new SeatAvailability_DD
                       {
                           taluk_id = a.taluk_id,
                           taluk_name = a.taluk_name
                       }).ToList();
            return res;
        }


        public List<SeatAvailability_DD> GetUpdateSeatAvailablityDLL(int courseId, int DivisionId, int DistrictId, int TalukaId)
        {
            var res = (from a in _db.tbl_taluk_mast
                       where a.is_active == true && a.district_id == DistrictId
                       select new SeatAvailability_DD
                       {
                           taluk_id = a.taluk_id,
                           taluk_name = a.taluk_name
                       }).ToList();
            return res;
        }


        public List<SeatAvailability_DD> GetRoleListDLL()
        {
            var res = (from a in _db.tbl_user_roles
                       where a.is_active == true
                       select new SeatAvailability_DD
                       {
                           role_id = a.user_id,
                           role_name = a.user_role
                       }).ToList();
            return res;
        }

        public List<SeatAvailabilityMaster> GetGridDetails(int courseId, int DivisionId, int DistrictId, int TalukId)
        {
            if (courseId == 0)
            {
                var result = (from s in _db.tbl_ITI_trade_seat_master
                              join ts in _db.tbl_ITI_trade_seat_trans on s.Trade_ITI_seat_Id equals ts.Trade_ITI_seat_Id
                              join t in _db.tbl_ITI_Trade on s.Trade_ITI_Id equals t.Trade_ITI_id
                              join c in _db.tbl_iti_college_details on t.ITICode equals c.iti_college_id
                              join div in _db.tbl_division_mast on c.division_id equals div.division_id // && div.is_active equals 1 
                              join dist in _db.tbl_district_mast on div.division_id equals dist.division_id
                              join sh in _db.tbl_shifts on s.ShiftId equals sh.s_id
                              join tm in _db.tbl_trade_mast on t.Trade_ITI_id equals tm.trade_id
                              join u in _db.tbl_units on s.UnitId equals u.u_id
                              join st in _db.tbl_seat_type on s.SeatsTypeId equals st.Seat_type_id
                              select new SeatAvailabilityMaster
                              {
                                  MISCode = c.MISCode,
                                  Division_id = div.division_id,
                                  Division_name = div.division_name,
                                  district_id = dist.dist_id,
                                  dist_name = dist.dist_name,
                                  // Type = c.Insitute_TypeId,
                                  iti_college_id = c.iti_college_id,
                                  iti_college_name = c.iti_college_name,
                                  trade_id = tm.trade_id,
                                  trade_name = tm.trade_name,
                                  SeatsPerUnit = s.SeatsPerUnit,
                                  units = u.units,
                                  SeatType = st.SeatType,
                                  PPP_seats = s.PPP_seats,
                                  IMCPrivateManageMent = s.Management_seats,
                                  IsPPP = s.IsPPP,
                                  Shift = sh.shifts,
                                  DualSystemTraining = s.DualSystemTraining
                              }).ToList();
                return result;
            }
            else
            {
                var result = (from s in _db.tbl_ITI_trade_seat_master
                              join ts in _db.tbl_ITI_trade_seat_trans on s.Trade_ITI_seat_Id equals ts.Trade_ITI_seat_Id
                              join t in _db.tbl_ITI_Trade on s.Trade_ITI_Id equals t.Trade_ITI_id
                              join c in _db.tbl_iti_college_details on t.ITICode equals c.iti_college_id
                              join div in _db.tbl_division_mast on c.division_id equals div.division_id // && div.is_active equals 1 
                              join dist in _db.tbl_district_mast on div.division_id equals dist.division_id
                              join sh in _db.tbl_shifts on s.ShiftId equals sh.s_id
                              join tm in _db.tbl_trade_mast on t.Trade_ITI_id equals tm.trade_id
                              join u in _db.tbl_units on s.UnitId equals u.u_id
                              join st in _db.tbl_seat_type on s.SeatsTypeId equals st.Seat_type_id
                              where div.division_id == DivisionId && tm.trade_course_id == courseId && dist.dist_id == DistrictId && c.taluk_id == TalukId
                              select new SeatAvailabilityMaster
                              {
                                  MISCode = c.MISCode,
                                  Division_id = div.division_id,
                                  Division_name = div.division_name,
                                  district_id = dist.dist_id,
                                  dist_name = dist.dist_name,
                                  // Type = c.Insitute_TypeId,
                                  iti_college_id = c.iti_college_id,
                                  iti_college_name = c.iti_college_name,
                                  trade_id = tm.trade_id,
                                  trade_name = tm.trade_name,
                                  SeatsPerUnit = s.SeatsPerUnit,
                                  units = u.units,
                                  SeatType = st.SeatType,
                                  PPP_seats = s.PPP_seats,
                                  IMCPrivateManageMent = s.Management_seats,
                                  IsPPP = s.IsPPP,
                                  Shift = sh.shifts,
                                  DualSystemTraining = s.DualSystemTraining
                              }).ToList();
                return result;
            }

        }

        public List<Notification> GetNotificationListDLL()
        {
            List<Notification> Notifs = null;

            //Notifs = (from n in _db.tbl_exam_cal_notif_mast 
            //          where n.ecn_is_published == 1 && n.exam_cal_status_id == 105
            //          select new Notification
            //          {
            //              Exam_Notif_Number = n.ecn_number,
            //              exam_notif_status_desc = n.ecn_desc,
            //              Exam_notif_date = n.ecn_Date,
            //              SavePath = n.exam_notif_file_path,
            //          }).ToList();

            return Notifs;
        }

        public List<SeatAvailabilityMaster> GetAdmissionSeatAvailListDLL(SeatAvailabilityMaster modal)
        {
            var res = (from t1 in _db.tbl_ITI_trade_seat_master
                       join t2 in _db.tbl_ITI_Trade on t1.Trade_ITI_Id equals t2.Trade_ITI_id
                       join t3 in _db.tbl_seat_type on t1.SeatsTypeId equals t3.Seat_type_id
                       join t4 in _db.tbl_shifts on t1.ShiftId equals t4.s_id
                       join t5 in _db.tbl_units on t1.UnitId equals t5.u_id
                       join t6 in _db.tbl_iti_college_details on t2.ITICode equals t6.iti_college_id
                       join t7 in _db.tbl_district_mast on t6.district_id equals t7.dist_id
                       join t8 in _db.tbl_division_mast on t7.division_id equals t8.division_id
                       join t9 in _db.tbl_trade_mast on t2.Trade_ITI_id equals t9.trade_id

                       select new SeatAvailabilityMaster
                       {
                           MISCode = t6.MISCode,
                           Division_id = t8.division_id,
                           Division_name = t8.division_name,
                           district_id = t7.dist_id,
                           dist_name = t7.dist_name,
                           // Type = c.Insitute_TypeId,
                           iti_college_id = t6.iti_college_id,
                           iti_college_name = t6.iti_college_name,
                           trade_id = t9.trade_id,
                           trade_name = t9.trade_name,
                           SeatsPerUnit = t1.SeatsPerUnit,
                           units = t5.units,
                           SeatType = t3.SeatType,
                           PPP_seats = t1.PPP_seats,
                           IMCPrivateManageMent = t1.Management_seats,
                           IsPPP = t1.IsPPP,
                           Shift = t4.shifts,
                           DualSystemTraining = t1.DualSystemTraining
                       }).ToList();
            return res;
        }


        // Trade Name List
        public List<SeatAvailabilityMaster> GetTradeNameListDLL()
        {
            var res = (from a in _db.tbl_trade_mast
                       select new SeatAvailabilityMaster
                       {
                           trade_id = a.trade_id,
                           trade_name = a.trade_name

                       }).ToList();
            return res;
        }
        //Unit Trade List
        public List<SeatAvailabilityMaster> GetUnitTradeListDLL()
        {
            var res = (from a in _db.tbl_units
                       select new SeatAvailabilityMaster
                       {
                           u_id = a.u_id,
                           units = a.units

                       }).ToList();
            return res;
        }
        // Trade IsPPP List
        public List<SeatAvailabilityMaster> GetTradeIsPPPListDLL()
        {
            var res = (from a in _db.tbl_ITI_trade_seat_master
                       select new SeatAvailabilityMaster
                       {
                           //Trade_ITI_seat_id = a.Trade_ITI_seat_id,
                           IsPPP = a.IsPPP

                       }).ToList();
            return res;
        }
        // Trade IsPPP List
        public List<SeatAvailabilityMaster> GetSysTrainingListDLL()
        {
            var res = (from a in _db.tbl_ITI_trade_seat_master
                       select new SeatAvailabilityMaster
                       {
                           //Trade_ITI_seat_id = a.Trade_ITI_seat_id,
                           //Trade_ITI_Id = a.Trade_ITI_Id,
                           //Trade_ITI_seat_id = a.Trade_ITI_seat_id,
                           DualSystemTraining = a.DualSystemTraining
                       }).ToList();
            return res;
        }

        public string InsertTradeseatMasterDLL(insertRecordsForTrade model)
        {
            tbl_ITI_trade_seat_master tbltrademaster = new tbl_ITI_trade_seat_master();
            tbltrademaster.Trade_ITI_Id = 21;
            tbltrademaster.UnitId = model.UnitId;
            tbltrademaster.IsPPP = model.IsPPP;
            tbltrademaster.DualSystemTraining = model.DualSystemTraining;
            tbltrademaster.ShiftId = model.ShiftId;
            tbltrademaster.SeatsPerUnit = model.SeatsPerUnit;
            tbltrademaster.SeatsTypeId = model.SeatsTypeId;
            tbltrademaster.CreatedBy = model.CreatedBy;
            tbltrademaster.CreatedOn = DateTime.Now;
            _db.tbl_ITI_trade_seat_master.Add(tbltrademaster);
            _db.SaveChanges();

            return "Success";
        }

        public string InsertTradeseatTranseDLL(insertRecordsForTrade model)
        {
            tbl_ITI_trade_seat_trans tbltradeTrans = new tbl_ITI_trade_seat_trans();

            tbltradeTrans.Trade_ITI_seat_Id = model.Trade_ITI_seat_Id;
            tbltradeTrans.Trans_Date = DateTime.Now;
            tbltradeTrans.StatusId = 1;
            tbltradeTrans.Remarks = model.Remarks;
            tbltradeTrans.FlowId = model.FlowId;
            tbltradeTrans.CreatedBy = model.CreatedBy;
            tbltradeTrans.CreatedOn = DateTime.Now;
            _db.tbl_ITI_trade_seat_trans.Add(tbltradeTrans);
            _db.SaveChanges();

            return "Success";
        }

        public List<SeatAvailabilityMaster> GetSeatListDLL(int courseId, int DivisionId, int DistrictId, int TalukId)
        {
            var res = (from t1 in _db.tbl_ITI_trade_seat_master
                       join t2 in _db.tbl_ITI_Trade on t1.Trade_ITI_Id equals t2.Trade_ITI_id
                       join t3 in _db.tbl_seat_type on t1.SeatsTypeId equals t3.Seat_type_id
                       join t4 in _db.tbl_shifts on t1.ShiftId equals t4.s_id
                       join t5 in _db.tbl_units on t1.UnitId equals t5.u_id
                       join t6 in _db.tbl_iti_college_details on t2.ITICode equals t6.iti_college_id
                       join t7 in _db.tbl_district_mast on t6.district_id equals t7.dist_id
                       join t9 in _db.tbl_trade_mast on t2.Trade_ITI_id equals t9.trade_id
                       join t8 in _db.tbl_division_mast on t7.division_id equals t8.division_id
                       where t8.division_id == DivisionId && t9.trade_course_id == courseId && t7.dist_id == DistrictId && t6.taluk_id == TalukId
                       select new SeatAvailabilityMaster
                       {
                           MISCode = t6.MISCode,
                           Division_id = t8.division_id,
                           Division_name = t8.division_name,
                           district_id = t7.dist_id,
                           dist_name = t7.dist_name,
                           // Type = c.Insitute_TypeId,
                           iti_college_id = t6.iti_college_id,
                           iti_college_name = t6.iti_college_name,
                           trade_id = t9.trade_id,
                           trade_name = t9.trade_name,
                           SeatsPerUnit = t1.SeatsPerUnit,
                           units = t5.units,
                           SeatType = t3.SeatType,
                           PPP_seats = t1.PPP_seats,
                           IMCPrivateManageMent = t1.Management_seats,
                           IsPPP = t1.IsPPP,
                           Shift = t4.shifts,
                           DualSystemTraining = t1.DualSystemTraining

                       }).ToList();
            return res;
        }

        public List<SeatAvailabilityMaster> GetSeatListDLL()
        {
            var res = (from t1 in _db.tbl_ITI_trade_seat_master
                       join t2 in _db.tbl_ITI_Trade on t1.Trade_ITI_Id equals t2.Trade_ITI_id
                       join t3 in _db.tbl_seat_type on t1.SeatsTypeId equals t3.Seat_type_id
                       join t4 in _db.tbl_shifts on t1.ShiftId equals t4.s_id
                       join t5 in _db.tbl_units on t1.UnitId equals t5.u_id
                       join t6 in _db.tbl_iti_college_details on t2.ITICode equals t6.iti_college_id
                       join t7 in _db.tbl_district_mast on t6.district_id equals t7.dist_id
                       join t8 in _db.tbl_division_mast on t7.division_id equals t8.division_id
                       join t9 in _db.tbl_trade_mast on t2.Trade_ITI_id equals t9.trade_id
                       select new SeatAvailabilityMaster
                       {
                           MISCode = t6.MISCode,
                           Division_id = t8.division_id,
                           Division_name = t8.division_name,
                           district_id = t7.dist_id,
                           dist_name = t7.dist_name,
                           // Type = c.Insitute_TypeId,
                           iti_college_id = t6.iti_college_id,
                           iti_college_name = t6.iti_college_name,
                           trade_id = t9.trade_id,
                           trade_name = t9.trade_name,
                           SeatsPerUnit = t1.SeatsPerUnit,
                           units = t5.units,
                           SeatType = t3.SeatType,
                           PPP_seats = t1.PPP_seats,
                           IMCPrivateManageMent = t1.Management_seats,
                           IsPPP = t1.IsPPP,
                           Shift = t4.shifts,
                           DualSystemTraining = t1.DualSystemTraining
                       }).ToList();
            return res;
        }

        public List<SeatAvailabilityMaster> GetSeatListByIdDLL(string ListId)
        {
            var res = (from t1 in _db.tbl_ITI_trade_seat_master
                       join t2 in _db.tbl_ITI_Trade on t1.Trade_ITI_Id equals t2.Trade_ITI_id
                       join t3 in _db.tbl_seat_type on t1.SeatsTypeId equals t3.Seat_type_id
                       join t4 in _db.tbl_shifts on t1.ShiftId equals t4.s_id
                       join t5 in _db.tbl_units on t1.UnitId equals t5.u_id
                       join t6 in _db.tbl_iti_college_details on t2.ITICode equals t6.iti_college_id
                       join t7 in _db.tbl_district_mast on t6.district_id equals t7.dist_id
                       join t8 in _db.tbl_division_mast on t7.division_id equals t8.division_id
                       join t9 in _db.tbl_trade_mast on t2.Trade_ITI_id equals t9.trade_id
                       where t6.MISCode == ListId
                       select new SeatAvailabilityMaster
                       {
                           MISCode = t6.MISCode,
                           Division_id = t8.division_id,
                           Division_name = t8.division_name,
                           district_id = t7.dist_id,
                           dist_name = t7.dist_name,
                           // Type = c.Insitute_TypeId,
                           iti_college_id = t6.iti_college_id,
                           iti_college_name = t6.iti_college_name,
                           trade_id = t9.trade_id,
                           trade_name = t9.trade_name,
                           SeatsPerUnit = t1.SeatsPerUnit,
                           units = t5.units,
                           SeatType = t3.SeatType,
                           PPP_seats = t1.PPP_seats,
                           IMCPrivateManageMent = t1.Management_seats,
                           IsPPP = t1.IsPPP,
                           Shift = t4.shifts,
                           DualSystemTraining = t1.DualSystemTraining
                       }).ToList();
            return res;
        }

        public List<SelectListItem> GetAllStatusBasedOnUserDLL()
        {
            try
            {
                var list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true)
                            select new SelectListItem
                            {
                                Text = a.StatusName,
                                Value = a.StatusId.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SelectList GetStatusListDLL()
        {
            List<SelectListItem> StatusList = _db.tbl_status_master.Select(x => new SelectListItem
            {
                Text = x.StatusName.ToString(),
                Value = x.StatusId.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {
                Value = null,
                Text = "Select Status List",
            };
            StatusList.Insert(0, ProposalList);
            return new SelectList(StatusList, "Value", "Text");
        }

        public bool InsertRemarksDetailsForSeatDLL(string Remarks, int Status, int TradeITIseatidID)
        {
            try
            {
                Models.Master.tbl_ITI_trade_seat_trans_history tbl_ITI_trade_seat_trans_history = new Models.Master.tbl_ITI_trade_seat_trans_history();
                tbl_ITI_trade_seat_trans_history.Trade_ITI_seat_trans_id = TradeITIseatidID;
                tbl_ITI_trade_seat_trans_history.Trade_ITI_seat_Id = TradeITIseatidID;
                tbl_ITI_trade_seat_trans_history.Trans_Date = DateTime.Now;
                tbl_ITI_trade_seat_trans_history.StatusId = Status;
                tbl_ITI_trade_seat_trans_history.Remarks = Remarks;
                tbl_ITI_trade_seat_trans_history.CreatedOn = DateTime.Now;
                tbl_ITI_trade_seat_trans_history.CreatedBy = 1;
                tbl_ITI_trade_seat_trans_history.FlowId = 2;
                tbl_ITI_trade_seat_trans_history.ModifiedRole = 1;
                _db.tbl_ITI_trade_seat_trans_history.Add(tbl_ITI_trade_seat_trans_history);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<SeatAvailabilityMaster> UpdateRemarksDetailsForSeatDLL(string Remarks, int Status, int TradeITIseatidID)
        {
            bool returnValue = InsertRemarksDetailsForSeatDLL1(Remarks, Status, TradeITIseatidID);
            if (returnValue)
            {
                try
                {
                    Models.Master.tbl_ITI_trade_seat_trans tbl_ITI_trade_seat_trans = new Models.Master.tbl_ITI_trade_seat_trans();
                    var update_query = _db.tbl_ITI_trade_seat_trans.Where(s => s.Trade_ITI_seat_trans_id == TradeITIseatidID).FirstOrDefault();
                    update_query.Remarks = Remarks;
                    update_query.StatusId = Status;
                    update_query.FlowId = 2;
                    update_query.Trans_Date = DateTime.Now;
                    update_query.CreatedOn = DateTime.Now;
                    _db.SaveChanges();

                    var res = GetSeatListDLL();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var res = GetSeatListDLL();
                return res;
            }
        }

        public List<SeatAvailabilityUpdate> GetStatusListByIdDLL(int TradeITISeatid)
        {
            var res = (from a in _db.tbl_ITI_trade_seat_trans
                       where a.Trade_ITI_seat_Id == TradeITISeatid
                       select new SeatAvailabilityUpdate
                       {
                           Trade_ITI_seat_trans_id = a.Trade_ITI_seat_trans_id,
                           StatusId = a.StatusId,
                           Remarks = a.Remarks
                       }).ToList();
            return res;
        }

        public bool InsertRemarksDetailsForSeatDLL1(string Remarks, int Status, int TradeITIseatidID)
        {
            try
            {
                //tbl_ITI_trade_seat_trans_history tbl_ITI_trade_seat_trans_history = new tbl_ITI_trade_seat_trans_history();
                //tbl_ITI_trade_seat_trans_history.Trade_ITI_seat_trans_id = TradeITIseatidID;
                //tbl_ITI_trade_seat_trans_history.Trade_ITI_seat_Id = TradeITIseatidID;
                //tbl_ITI_trade_seat_trans_history.Trans_Date = DateTime.Now;
                //tbl_ITI_trade_seat_trans_history.StatusId = Status;
                //tbl_ITI_trade_seat_trans_history.Remarks = Remarks;
                //tbl_ITI_trade_seat_trans_history.CreatedOn = DateTime.Now;
                //tbl_ITI_trade_seat_trans_history.CreatedBy = 1;
                //tbl_ITI_trade_seat_trans_history.FlowId = 2;
                //tbl_ITI_trade_seat_trans_history.ModifiedRole = 1;
                //_db.tbl_ITI_trade_seat_trans_history.Add(tbl_ITI_trade_seat_trans_history);
                //_db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<SeatAvailabilityMaster> UpdateRemarksDetailsForSeatDLL1(string Remarks, int Status, int TradeITIseatidID)
        {
            bool returnValue = InsertRemarksDetailsForSeatDLL(Remarks, Status, TradeITIseatidID);
            if (returnValue)
            {
                try
                {
                    Models.Master.tbl_ITI_trade_seat_trans tbl_ITI_trade_seat_trans = new Models.Master.tbl_ITI_trade_seat_trans();
                    var update_query = _db.tbl_ITI_trade_seat_trans.Where(s => s.Trade_ITI_seat_trans_id == TradeITIseatidID).FirstOrDefault();
                    update_query.Remarks = Remarks;
                    update_query.StatusId = Status;
                    update_query.FlowId = 2;
                    update_query.Trans_Date = DateTime.Now;
                    update_query.CreatedOn = DateTime.Now;
                    _db.SaveChanges();

                    var res = GetSeatListDLL();
                    return res;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var res = GetSeatListDLL();
                return res;
            }
        }


        #region calendar
        //GetCalendar
        public List<AdmissionNotification> GetCalendarNotificationDLL(int id, int? calendarId)
        {
            List<AdmissionNotification> Notifs = null;

            Notifs = (from n in _db.tbl_tentative_calendar_of_events
                      join d in _db.tbl_tentative_calendar_of_events_trans on n.Tentative_admsn_evnt_clndr_Id equals d.Tentative_admsn_evnt_clndr_Id
                      join p in _db.tbl_admission_notif_status_mast on n.StatusId equals p.statusId
                      join c in _db.tbl_course_type_mast on n.Course_Id equals c.course_id
                      join m in _db.tbl_department_master on n.DeptId equals m.dept_id
                      join rl in _db.tbl_role_master on d.FlowId equals rl.role_id
                      join Tae in _db.tbl_Tentative_admsn_eventDetails on n.Tentative_admsn_evnt_clndr_Id equals Tae.Tentative_admsn_evnt_clndr_Id
                      join ad in _db.tbl_admsn_ntf_details on n.admissionNotificationId equals ad.Admsn_notif_Id
                      join ty in _db.tbl_ApplicantType on n.applicantType equals ty.ApplicantTypeId
                      where d.Tentative_admsn_evnt_clndr_Id == calendarId

                      select new AdmissionNotification
                      {
                          Admsn_tentative_calndr_transId = d.Admsn_tentative_calndr_trans_Id,
                          Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                          Exam_Notif_Number = n.Admsn_ntf_num,
                          NotifDesc = n.Notif_desc,
                          Notification_Date = (DateTime)n.Notification_Date,
                          //exam_notif_status_desc = p.statusDesc,
                          StatusId = n.StatusId,
                          CourseTypeId = n.Course_Id,
                          CourseTypeName = c.course_type_name,
                          applicantTypeId = n.applicantType,
                          applicantTypeName = ty.ApplicantType,
                          DeptId = n.DeptId,
                          DeptName = m.dept_description,
                          session = n.session,
                          NtfEmailId = n.EmailId,
                          //sessionYear = ye.Year,
                          Admsn_notif_Id = n.admissionNotificationId,
                          admissionNotificationNumber = ad.AdmsnNtfNum,
                          Admsn_notif_doc = n.Admsn_notif_doc,
                          FromDt_ApplyingOnlineApplicationForm = Tae.FromDt_ApplyingOnlineApplicationForm,
                          ToDt_ApplyingOnlineApplicationForm = Tae.ToDt_ApplyingOnlineApplicationForm,
                          FromDt_DocVerificationPeriod = Tae.FromDt_DocVerificationPeriod,
                          ToDt_DocVerificationPeriod = Tae.ToDt_DocVerificationPeriod,
                          Dt_DisplayEigibleVerifiedlist = Tae.Dt_DisplayEigibleVerifiedlist,
                          FromDt_Fnlseatmtrx = Tae.Dt_DBBackupSeatMatrixFInalByDept,
                          Dt_DisplayTentativeGradation = Tae.Dt_DisplayTentativeGradation,
                          Publishgrdlist1Rnd = Tae.Dt_DisplayFinalGradationList,
                          Dt_1stListSeatAllotment = Tae.Dt_1stListSeatAllotment,
                          FromDt_1stRoundAdmissionProcess = Tae.FromDt_1stRoundAdmissionProcess,
                          ToDt_1stRoundAdmissionProcess = Tae.ToDt_1stRoundAdmissionProcess,
                          Dt_1stListAdmittedCand = Tae.Dt_1stListAdmittedCand,
                          FromDt_2ndRoundEntryChoiceTrade = Tae.FromDt_2ndRoundEntryChoiceTrade,
                          ToDt_2ndRoundEntryChoiceTrade = Tae.ToDt_2ndRoundEntryChoiceTrade,
                          FromDt_DbBkp2ndRoundOnlineSeat = Tae.FromDt_DbBkp2ndRoundOnlineSeat,
                          ToDt_DbBkp2ndRoundOnlineSeat = Tae.ToDt_DbBkp2ndRoundOnlineSeat,
                          FromDt_2ndRoundAdmissionProcess = Tae.FromDt_2ndRoundAdmissionProcess,
                          ToFDt_2ndRoundAdmissionProcess = Tae.ToFDt_2ndRoundAdmissionProcess,
                          Dt_2ndAdmittedCand = Tae.Dt_2ndAdmittedCand,
                          FromDt_3rdRoundEntryChoiceTrade = Tae.FromDt_3rdRoundEntryChoiceTrade,
                          ToDt_3rdRoundEntryChoiceTrade = Tae.ToDt_3rdRoundEntryChoiceTrade,
                          Dt_DbBkp3rdRoundOnlineSeat = Tae.Dt_DbBkp3rdRoundOnlineSeat,
                          FromDt_3rdRoundAdmissionProcess = Tae.FromDt_3rdRoundAdmissionProcess,
                          ToDt_3rdRoundAdmissionProcess = Tae.ToDt_3rdRoundAdmissionProcess,
                          Dt_3rdAdmittedCand = Tae.Dt_3rdAdmittedCand,
                          FromDt_FinalRoundEntryChoiceTrade = Tae.FromDt_FinalRoundEntryChoiceTrade,
                          ToDt_FinalRoundEntryChoiceTrade = Tae.ToDt_FinalRoundEntryChoiceTrade,
                          Dt_FinalRoundSeatAllotment = Tae.Dt_FinalRoundSeatAllotment,
                          FromDt_AdmissionFinalRound = Tae.FromDt_AdmissionFinalRound,
                          ToDt_AdmissionFinalRound = Tae.ToDt_AdmissionFinalRound,
                          Dt_CommencementOfTraining = Tae.Dt_CommencementOfTraining,
                          //Newly added Column Mapping 
                          FromDt_TntGrlistbyAppl = Tae.FromDt_TntGrlistbyAppl,
                          ToDt_TntGrlistbyAppl = Tae.ToDt_TntGrlistbyAppl,
                          FromDt_ApplDocVerif = Tae.FromDt_ApplDocVerif,
                          ToDt_ApplDocVerif = Tae.ToDt_ApplDocVerif,
                          FromDt_AplGrVRoffcr = Tae.FromDt_AplGrVRoffcr,
                          ToDt_AplGrVRoffcr = Tae.ToDt_AplGrVRoffcr,
                          Publishgrdlist2Rnd = Tae.Dt_GradationList2round,
                          Publishgrdlist3Rnd = Tae.Publishgrdlist3Rnd,
                          FromDt_Tentatviveseat = Tae.FromDt_Tentatviveseat,
                          Dt_Notification = Tae.Dt_Notification


                      }).ToList();


            return Notifs;
        }
        public string GetCalendarNotificationTransDLL(AdmissionNotification model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    tbl_tentative_calendar_of_events CalenderNotification = _db.tbl_tentative_calendar_of_events.Where(x => x.Admsn_ntf_num == model.Exam_Notif_Number).FirstOrDefault();
                    //tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = _db.tbl_Tentative_admsn_eventDetails.Where(x => x.Tentative_admsn_evnt_clndr_Id == model.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                    //tbl_tentative_calendar_of_events_trans cal_Notification_Trans = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == model.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                    //if (model.DeptId == (int)CmnClass.Dept.Admission)
                    //{
                    //if (CalenderNotification != null)
                    //{
                    //	return "exist";
                    //}
                    //}
                    if (CalenderNotification == null)
                    {
                        CalenderNotification = new tbl_tentative_calendar_of_events();
                        //CalenderNotification.Tentative_admsn_evnt_clndr_Id = model.Tentative_admsn_evnt_clndrId;
                        CalenderNotification.Admsn_ntf_num = model.Exam_Notif_Number;
                        CalenderNotification.Admsn_notif_doc = model.SavePath;
                        CalenderNotification.DeptId = model.DeptId;
                        CalenderNotification.Notif_desc = model.NotifDesc;
                        CalenderNotification.EmailId = model.NtfEmailId;
                        CalenderNotification.Course_Id = model.CourseTypeId;
                        CalenderNotification.applicantType = model.applicantTypeId.Value;
                        CalenderNotification.Notification_Date = model.Notification_Date;
                        CalenderNotification.session = model.YearID;
                        CalenderNotification.admissionNotificationId = model.Admsn_notif_Id;
                        CalenderNotification.CreatedOn = DateTime.Now;
                        CalenderNotification.CreatedBy = model.user_id;
                        if (model.DeptId == (int)CmnClass.Dept.Admission)
                            CalenderNotification.StatusId = 101;
                        else
                            CalenderNotification.StatusId = 110;

                        _db.tbl_tentative_calendar_of_events.Add(CalenderNotification);
                        _db.SaveChanges();
                        //Start
                        tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = new tbl_Tentative_admsn_eventDetails();
                        cal_Notification_EventDetails.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                        cal_Notification_EventDetails.FromDt_ApplyingOnlineApplicationForm = model.FromDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.ToDt_ApplyingOnlineApplicationForm = model.ToDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.FromDt_DocVerificationPeriod = model.FromDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.ToDt_DocVerificationPeriod = model.ToDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.Dt_DisplayEigibleVerifiedlist = model.Dt_DisplayEigibleVerifiedlist;
                        cal_Notification_EventDetails.Dt_DBBackupSeatMatrixFInalByDept = model.FromDt_Fnlseatmtrx;//model.Dt_DBBackupSeatMatrixFInalByDept;
                        cal_Notification_EventDetails.Dt_DisplayTentativeGradation = model.Dt_DisplayTentativeGradation;
                        cal_Notification_EventDetails.Dt_DisplayFinalGradationList = model.Publishgrdlist1Rnd;//model.Dt_DisplayFinalGradationList;
                        cal_Notification_EventDetails.Dt_1stListSeatAllotment = model.Dt_1stListSeatAllotment;
                        cal_Notification_EventDetails.FromDt_1stRoundAdmissionProcess = model.FromDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
                        cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_2ndRoundAdmissionProcess = model.FromDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToFDt_2ndRoundAdmissionProcess = model.ToFDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_2ndAdmittedCand = model.Dt_2ndAdmittedCand;
                        cal_Notification_EventDetails.FromDt_3rdRoundEntryChoiceTrade = model.FromDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_3rdRoundEntryChoiceTrade = model.ToDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_DbBkp3rdRoundOnlineSeat = model.Dt_DbBkp3rdRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_3rdRoundAdmissionProcess = model.FromDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_3rdRoundAdmissionProcess = model.ToDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_3rdAdmittedCand = model.Dt_3rdAdmittedCand;
                        cal_Notification_EventDetails.FromDt_FinalRoundEntryChoiceTrade = model.FromDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_FinalRoundEntryChoiceTrade = model.ToDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_FinalRoundSeatAllotment = model.Dt_FinalRoundSeatAllotment;
                        cal_Notification_EventDetails.FromDt_AdmissionFinalRound = model.FromDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.ToDt_AdmissionFinalRound = model.ToDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.Dt_CommencementOfTraining = model.Dt_CommencementOfTraining;
                        cal_Notification_EventDetails.CreatedDate = DateTime.Now;
                        cal_Notification_EventDetails.CreatedBy = model.user_id;
                        //New Columns Saving part 
                        cal_Notification_EventDetails.FromDt_TntGrlistbyAppl = model.FromDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.ToDt_TntGrlistbyAppl = model.ToDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.FromDt_ApplDocVerif = model.FromDt_ApplDocVerif;
                        cal_Notification_EventDetails.ToDt_ApplDocVerif = model.ToDt_ApplDocVerif;
                        cal_Notification_EventDetails.FromDt_AplGrVRoffcr = model.FromDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.ToDt_AplGrVRoffcr = model.ToDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.Dt_GradationList2round = model.Publishgrdlist2Rnd;
                        cal_Notification_EventDetails.Publishgrdlist3Rnd = model.Publishgrdlist3Rnd;
                        cal_Notification_EventDetails.FromDt_Tentatviveseat = model.FromDt_Tentatviveseat;
                        cal_Notification_EventDetails.Dt_Notification = model.Dt_Notification;

                        _db.tbl_Tentative_admsn_eventDetails.Add(cal_Notification_EventDetails);
                        _db.SaveChanges();

                        tbl_tentative_calendar_of_events_trans cal_Notification_Trans = new tbl_tentative_calendar_of_events_trans();
                        cal_Notification_Trans = new tbl_tentative_calendar_of_events_trans();
                        cal_Notification_Trans.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                        if (model.DeptId == 1)
                            cal_Notification_Trans.Status_id = 101;
                        else
                            cal_Notification_Trans.Status_id = 110;
                        cal_Notification_Trans.Trans_date = DateTime.Now;
                        cal_Notification_Trans.FlowId = model.user_id;
                        cal_Notification_Trans.CreatedOn = DateTime.Now;
                        cal_Notification_Trans.CreatedBy = model.user_id;
                        cal_Notification_Trans.Remarks = "Inserted successfully";
                        _db.tbl_tentative_calendar_of_events_trans.Add(cal_Notification_Trans);
                        _db.SaveChanges();
                    }
                    else if (model.DeptId == 3)
                    {
                        CalenderNotification.Admsn_notif_doc = model.SavePath;
                        CalenderNotification.Notif_desc = model.NotifDesc;
                        CalenderNotification.Notification_Date = model.Notification_Date;
                        CalenderNotification.admissionNotificationId = model.Admsn_notif_Id;
                        CalenderNotification.EmailId = model.NtfEmailId;

                        if (CalenderNotification.StatusId == 110)
                            CalenderNotification.StatusId = 110;
                        else
                            CalenderNotification.StatusId = 101;
                        //CalenderNotification.Appli_Subm_LastDate = DateTime.Now;
                        _db.SaveChanges();

                        //tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = new tbl_Tentative_admsn_eventDetails();
                        tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = _db.tbl_Tentative_admsn_eventDetails.Where(x => x.Tentative_admsn_evnt_clndr_Id == CalenderNotification.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                        //cal_Notification_EventDetails.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                        cal_Notification_EventDetails.FromDt_ApplyingOnlineApplicationForm = model.FromDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.ToDt_ApplyingOnlineApplicationForm = model.ToDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.FromDt_DocVerificationPeriod = model.FromDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.ToDt_DocVerificationPeriod = model.ToDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.Dt_DisplayEigibleVerifiedlist = model.Dt_DisplayEigibleVerifiedlist;
                        cal_Notification_EventDetails.Dt_DBBackupSeatMatrixFInalByDept = model.FromDt_Fnlseatmtrx;
                        cal_Notification_EventDetails.Dt_DisplayTentativeGradation = model.Dt_DisplayTentativeGradation;
                        cal_Notification_EventDetails.Dt_DisplayFinalGradationList = model.Publishgrdlist1Rnd;
                        cal_Notification_EventDetails.Dt_1stListSeatAllotment = model.Dt_1stListSeatAllotment;
                        cal_Notification_EventDetails.FromDt_1stRoundAdmissionProcess = model.FromDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
                        cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_2ndRoundAdmissionProcess = model.FromDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToFDt_2ndRoundAdmissionProcess = model.ToFDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_2ndAdmittedCand = model.Dt_2ndAdmittedCand;
                        cal_Notification_EventDetails.FromDt_3rdRoundEntryChoiceTrade = model.FromDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_3rdRoundEntryChoiceTrade = model.ToDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_DbBkp3rdRoundOnlineSeat = model.Dt_DbBkp3rdRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_3rdRoundAdmissionProcess = model.FromDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_3rdRoundAdmissionProcess = model.ToDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_3rdAdmittedCand = model.Dt_3rdAdmittedCand;
                        cal_Notification_EventDetails.FromDt_FinalRoundEntryChoiceTrade = model.FromDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_FinalRoundEntryChoiceTrade = model.ToDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_FinalRoundSeatAllotment = model.Dt_FinalRoundSeatAllotment;
                        cal_Notification_EventDetails.FromDt_AdmissionFinalRound = model.FromDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.ToDt_AdmissionFinalRound = model.ToDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.Dt_CommencementOfTraining = model.Dt_CommencementOfTraining;
                        cal_Notification_EventDetails.CreatedDate = DateTime.Now;
                        cal_Notification_EventDetails.CreatedBy = model.user_id;
                        //New Columns Saving part 
                        cal_Notification_EventDetails.FromDt_TntGrlistbyAppl = model.FromDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.ToDt_TntGrlistbyAppl = model.ToDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.FromDt_ApplDocVerif = model.FromDt_ApplDocVerif;
                        cal_Notification_EventDetails.ToDt_ApplDocVerif = model.ToDt_ApplDocVerif;
                        cal_Notification_EventDetails.FromDt_AplGrVRoffcr = model.FromDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.ToDt_AplGrVRoffcr = model.ToDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.Dt_GradationList2round = model.Publishgrdlist2Rnd;
                        cal_Notification_EventDetails.Publishgrdlist3Rnd = model.Publishgrdlist3Rnd;
                        cal_Notification_EventDetails.FromDt_Tentatviveseat = model.FromDt_Tentatviveseat;
                        cal_Notification_EventDetails.Dt_Notification = model.Dt_Notification;
                        //_db.tbl_Tentative_admsn_eventDetails.Add(cal_Notification_EventDetails);
                        _db.SaveChanges();

                        //tbl_tentative_calendar_of_events_trans cal_Notification_Trans = new tbl_tentative_calendar_of_events_trans();
                        tbl_tentative_calendar_of_events_trans cal_Notification_Trans = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == CalenderNotification.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                        //cal_Notification_Trans.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                        //if (model.DeptId == 1)
                        //    cal_Notification_Trans.Status_id = 101;
                        //else
                        //    cal_Notification_Trans.Status_id = 110;

                        if (cal_Notification_Trans.Status_id == 110)
                            cal_Notification_Trans.Status_id = 110;
                        else
                            cal_Notification_Trans.Status_id = 101;

                        cal_Notification_Trans.Trans_date = DateTime.Now;
                        cal_Notification_Trans.FlowId = model.user_id;
                        cal_Notification_Trans.CreatedOn = DateTime.Now;
                        cal_Notification_Trans.CreatedBy = model.user_id;
                        cal_Notification_Trans.Remarks = "Updated successfully";
                        //_db.tbl_tentative_calendar_of_events_trans.Add(cal_Notification_Trans);

                        _db.SaveChanges();
                        transaction.Commit();
                        return "Updated";
                    }
                    else if (CalenderNotification.StatusId == 110 && model.DeptId == 1)
                    {
                        //CalenderNotification.Admsn_notif_doc = model.SavePath;
                        //CalenderNotification.Notif_desc = model.NotifDesc;
                        //CalenderNotification.Notification_Date = model.Notification_Date;
                        //CalenderNotification.StatusId = 101;
                        CalenderNotification.Admsn_notif_doc = model.SavePath;
                        CalenderNotification.Notif_desc = model.NotifDesc;
                        CalenderNotification.EmailId = model.NtfEmailId;
                        CalenderNotification.Notification_Date = model.Notification_Date;
                        CalenderNotification.admissionNotificationId = model.Admsn_notif_Id;
                        CalenderNotification.StatusId = 101;
                        //CalenderNotification.Appli_Subm_LastDate = DateTime.Now;
                        _db.SaveChanges();

                        tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = _db.tbl_Tentative_admsn_eventDetails.Where(x => x.Tentative_admsn_evnt_clndr_Id == CalenderNotification.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                        //cal_Notification_EventDetails.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                        cal_Notification_EventDetails.FromDt_ApplyingOnlineApplicationForm = model.FromDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.ToDt_ApplyingOnlineApplicationForm = model.ToDt_ApplyingOnlineApplicationForm;
                        cal_Notification_EventDetails.FromDt_DocVerificationPeriod = model.FromDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.ToDt_DocVerificationPeriod = model.ToDt_DocVerificationPeriod;
                        cal_Notification_EventDetails.Dt_DisplayEigibleVerifiedlist = model.Dt_DisplayEigibleVerifiedlist;
                        cal_Notification_EventDetails.Dt_DBBackupSeatMatrixFInalByDept = model.FromDt_Fnlseatmtrx;
                        cal_Notification_EventDetails.Dt_DisplayTentativeGradation = model.Dt_DisplayTentativeGradation;
                        cal_Notification_EventDetails.Dt_DisplayFinalGradationList = model.Publishgrdlist1Rnd;
                        cal_Notification_EventDetails.Dt_1stListSeatAllotment = model.Dt_1stListSeatAllotment;
                        cal_Notification_EventDetails.FromDt_1stRoundAdmissionProcess = model.FromDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
                        cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_2ndRoundAdmissionProcess = model.FromDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToFDt_2ndRoundAdmissionProcess = model.ToFDt_2ndRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_2ndAdmittedCand = model.Dt_2ndAdmittedCand;
                        cal_Notification_EventDetails.FromDt_3rdRoundEntryChoiceTrade = model.FromDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_3rdRoundEntryChoiceTrade = model.ToDt_3rdRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_DbBkp3rdRoundOnlineSeat = model.Dt_DbBkp3rdRoundOnlineSeat;
                        cal_Notification_EventDetails.FromDt_3rdRoundAdmissionProcess = model.FromDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.ToDt_3rdRoundAdmissionProcess = model.ToDt_3rdRoundAdmissionProcess;
                        cal_Notification_EventDetails.Dt_3rdAdmittedCand = model.Dt_3rdAdmittedCand;
                        cal_Notification_EventDetails.FromDt_FinalRoundEntryChoiceTrade = model.FromDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.ToDt_FinalRoundEntryChoiceTrade = model.ToDt_FinalRoundEntryChoiceTrade;
                        cal_Notification_EventDetails.Dt_FinalRoundSeatAllotment = model.Dt_FinalRoundSeatAllotment;
                        cal_Notification_EventDetails.FromDt_AdmissionFinalRound = model.FromDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.ToDt_AdmissionFinalRound = model.ToDt_AdmissionFinalRound;
                        cal_Notification_EventDetails.Dt_CommencementOfTraining = model.Dt_CommencementOfTraining;
                        cal_Notification_EventDetails.CreatedDate = DateTime.Now;
                        cal_Notification_EventDetails.CreatedBy = model.user_id;
                        //New Columns Saving part 
                        cal_Notification_EventDetails.FromDt_TntGrlistbyAppl = model.FromDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.ToDt_TntGrlistbyAppl = model.ToDt_TntGrlistbyAppl;
                        cal_Notification_EventDetails.FromDt_ApplDocVerif = model.FromDt_ApplDocVerif;
                        cal_Notification_EventDetails.ToDt_ApplDocVerif = model.ToDt_ApplDocVerif;
                        cal_Notification_EventDetails.FromDt_AplGrVRoffcr = model.FromDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.ToDt_AplGrVRoffcr = model.ToDt_AplGrVRoffcr;
                        cal_Notification_EventDetails.Dt_GradationList2round = model.Publishgrdlist2Rnd;
                        cal_Notification_EventDetails.Publishgrdlist3Rnd = model.Publishgrdlist3Rnd;
                        cal_Notification_EventDetails.FromDt_Tentatviveseat = model.FromDt_Tentatviveseat;
                        cal_Notification_EventDetails.Dt_Notification = model.Dt_Notification;


                        _db.SaveChanges();

                        tbl_tentative_calendar_of_events_trans cal_Notification_Trans = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == CalenderNotification.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                        cal_Notification_Trans.Status_id = 101;
                        cal_Notification_Trans.Trans_date = DateTime.Now;
                        cal_Notification_Trans.FlowId = model.user_id;
                        cal_Notification_Trans.CreatedOn = DateTime.Now;
                        cal_Notification_Trans.CreatedBy = model.user_id;
                        cal_Notification_Trans.Remarks = "Inserted successfully";
                        //_db.tbl_tentative_calendar_of_events_trans.Add(cal_Notification_Trans);

                        _db.SaveChanges();
                    }
                    else
                    {
                        return "exist";
                    }
                    //_db.SaveChanges();
                    //tbl_tentative_calendar_of_events_trans cal_Notification_Trans = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == model.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                    //cal_Notification_Trans = new tbl_tentative_calendar_of_events_trans();
                    //cal_Notification_Trans.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
                    //if (model.DeptId == 1)
                    //    cal_Notification_Trans.Status_id = 101;
                    //else
                    //    cal_Notification_Trans.Status_id = 110;
                    //cal_Notification_Trans.Trans_date = DateTime.Now;
                    //cal_Notification_Trans.FlowId = model.user_id;
                    //cal_Notification_Trans.CreatedOn = DateTime.Now;
                    //cal_Notification_Trans.CreatedBy = model.user_id;
                    //cal_Notification_Trans.Remarks = "Inserted successfully";
                    //_db.tbl_tentative_calendar_of_events_trans.Add(cal_Notification_Trans);
                    //_db.SaveChanges();

                    transaction.Commit();
                    return "Saved";
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    return "Failed";
                }
            }
        }

        public int GetNotificationCalEventStatus(int? calendarId)
        {
            try
            {
                int res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == calendarId).Select(y => y.StatusId).FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetApplicantTypeAdmission(int Id)
        {
            try
            {
                int res = _db.tbl_admsn_ntf_details.Where(x => x.Admsn_notif_Id == Id).Select(y => y.applicantType).FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AdmissionNotification> GetAdmissionNtfNumber(int Id)
        {

            try
            {
                //List<AdmissionNotification> Notifs = null;
                //int res = _db.tbl_admsn_ntf_details.Where(x => x.CourseId == Id).Select(y => y.Admsn_notif_Id).FirstOrDefault();
                var res = (from n in _db.tbl_admsn_ntf_details.Where(x => x.CourseId == Id)
                           select new AdmissionNotification
                           {
                               // CourseTypeId=n.CourseId
                               //applicantTypeId = n.applicantType,
                               AdmsnNtfNum = n.AdmsnNtfNum,
                               Admsn_notif_Id = n.Admsn_notif_Id
                           }).ToList();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Extra source

        //public string GetCalendarNotificationTransDLL(AdmissionNotification model)
        //{
        //    using (var transaction = _db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            tbl_tentative_calendar_of_events CalenderNotification = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == model.Admsn_tentative_calndr_transId).FirstOrDefault();

        //            if (CalenderNotification == null)
        //            {
        //                CalenderNotification = new tbl_tentative_calendar_of_events();
        //                //CalenderNotification.Tentative_admsn_evnt_clndr_Id = model.Tentative_admsn_evnt_clndrId;
        //                CalenderNotification.Admsn_notif_doc = model.SavePath;
        //                CalenderNotification.DeptId = 2;
        //                CalenderNotification.Notif_desc = model.NotifDesc;
        //                CalenderNotification.Admsn_ntf_num = model.Exam_Notif_Number;
        //                CalenderNotification.Course_Id = model.CourseTypeId;
        //                CalenderNotification.Notification_Date = model.Notification_Date;
        //                CalenderNotification.CreatedOn = DateTime.Now;
        //                CalenderNotification.CreatedBy = model.user_id;
        //                CalenderNotification.StatusId = 101;
        //                CalenderNotification.session = model.YearID;
        //                CalenderNotification.admissionNotificationId = model.Admsn_notif_Id;

        //                //CalenderNotification.admissionNotificationId = model.Tentative_admsn_evnt_clndr_Id;
        //                _db.tbl_tentative_calendar_of_events.Add(CalenderNotification);
        //                _db.SaveChanges();

        //                //Start
        //                tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = new tbl_Tentative_admsn_eventDetails();
        //                cal_Notification_EventDetails.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
        //                cal_Notification_EventDetails.FromDt_ApplyingOnlineApplicationForm = model.FromDt_ApplyingOnlineApplicationForm;
        //                cal_Notification_EventDetails.ToDt_ApplyingOnlineApplicationForm = model.ToDt_ApplyingOnlineApplicationForm;
        //                cal_Notification_EventDetails.FromDt_DocVerificationPeriod = model.FromDt_DocVerificationPeriod;
        //                cal_Notification_EventDetails.ToDt_DocVerificationPeriod = model.ToDt_DocVerificationPeriod;
        //                cal_Notification_EventDetails.Dt_DisplayEigibleVerifiedlist = model.Dt_DisplayEigibleVerifiedlist;
        //                cal_Notification_EventDetails.Dt_DBBackupSeatMatrixFInalByDept = model.Dt_DBBackupSeatMatrixFInalByDept;
        //                cal_Notification_EventDetails.Dt_DisplayTentativeGradation = model.Dt_DisplayTentativeGradation;
        //                cal_Notification_EventDetails.Dt_DisplayFinalGradationList = model.Dt_DisplayFinalGradationList;
        //                cal_Notification_EventDetails.Dt_1stListSeatAllotment = model.Dt_1stListSeatAllotment;
        //                cal_Notification_EventDetails.FromDt_1stRoundAdmissionProcess = model.FromDt_1stRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
        //                cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;


        //                cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
        //                cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
        //                //cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
        //                //cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
        //                //cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
        //                //cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;
        //                //cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
        //                //cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
        //                cal_Notification_EventDetails.FromDt_2ndRoundAdmissionProcess = model.FromDt_2ndRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToFDt_2ndRoundAdmissionProcess = model.ToFDt_2ndRoundAdmissionProcess;

        //                cal_Notification_EventDetails.Dt_2ndAdmittedCand = model.Dt_2ndAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_3rdRoundEntryChoiceTrade = model.FromDt_3rdRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_3rdRoundEntryChoiceTrade = model.ToDt_3rdRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.Dt_DbBkp3rdRoundOnlineSeat = model.Dt_DbBkp3rdRoundOnlineSeat;
        //                cal_Notification_EventDetails.FromDt_3rdRoundAdmissionProcess = model.FromDt_3rdRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToDt_3rdRoundAdmissionProcess = model.ToDt_3rdRoundAdmissionProcess;
        //                cal_Notification_EventDetails.Dt_3rdAdmittedCand = model.Dt_3rdAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_FinalRoundEntryChoiceTrade = model.FromDt_FinalRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_FinalRoundEntryChoiceTrade = model.ToDt_FinalRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.Dt_FinalRoundSeatAllotment = model.Dt_FinalRoundSeatAllotment;
        //                cal_Notification_EventDetails.FromDt_AdmissionFinalRound = model.FromDt_AdmissionFinalRound;
        //                cal_Notification_EventDetails.ToDt_AdmissionFinalRound = model.ToDt_AdmissionFinalRound;
        //                cal_Notification_EventDetails.Dt_CommencementOfTraining = model.Dt_CommencementOfTraining;
        //                cal_Notification_EventDetails.CreatedDate = DateTime.Now;
        //                cal_Notification_EventDetails.CreatedBy = model.user_id;
        //                _db.tbl_Tentative_admsn_eventDetails.Add(cal_Notification_EventDetails);
        //                _db.SaveChanges();
        //            }
        //            else
        //            {
        //                CalenderNotification.Admsn_notif_doc = model.SavePath;
        //                CalenderNotification.Notif_desc = model.NotifDesc;
        //                CalenderNotification.Notification_Date = model.Notification_Date;
        //                _db.SaveChanges();

        //                tbl_Tentative_admsn_eventDetails cal_Notification_EventDetails = new tbl_Tentative_admsn_eventDetails();
        //                cal_Notification_EventDetails.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
        //                cal_Notification_EventDetails.FromDt_ApplyingOnlineApplicationForm = model.FromDt_ApplyingOnlineApplicationForm;
        //                cal_Notification_EventDetails.ToDt_ApplyingOnlineApplicationForm = model.ToDt_ApplyingOnlineApplicationForm;
        //                cal_Notification_EventDetails.FromDt_DocVerificationPeriod = model.FromDt_DocVerificationPeriod;
        //                cal_Notification_EventDetails.ToDt_DocVerificationPeriod = model.ToDt_DocVerificationPeriod;
        //                cal_Notification_EventDetails.Dt_DisplayEigibleVerifiedlist = model.Dt_DisplayEigibleVerifiedlist;
        //                cal_Notification_EventDetails.Dt_DBBackupSeatMatrixFInalByDept = model.Dt_DBBackupSeatMatrixFInalByDept;
        //                cal_Notification_EventDetails.Dt_DisplayTentativeGradation = model.Dt_DisplayTentativeGradation;
        //                cal_Notification_EventDetails.Dt_DisplayFinalGradationList = model.Dt_DisplayFinalGradationList;
        //                cal_Notification_EventDetails.Dt_1stListSeatAllotment = model.Dt_1stListSeatAllotment;
        //                cal_Notification_EventDetails.FromDt_1stRoundAdmissionProcess = model.FromDt_1stRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToDt_1stRoundAdmissionProcess = model.ToDt_1stRoundAdmissionProcess;
        //                cal_Notification_EventDetails.Dt_1stListAdmittedCand = model.Dt_1stListAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_2ndRoundEntryChoiceTrade = model.FromDt_2ndRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_2ndRoundEntryChoiceTrade = model.ToDt_2ndRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.FromDt_DbBkp2ndRoundOnlineSeat = model.FromDt_DbBkp2ndRoundOnlineSeat;
        //                cal_Notification_EventDetails.ToDt_DbBkp2ndRoundOnlineSeat = model.ToDt_DbBkp2ndRoundOnlineSeat;
        //                cal_Notification_EventDetails.FromDt_2ndRoundAdmissionProcess = model.FromDt_2ndRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToFDt_2ndRoundAdmissionProcess = model.ToFDt_2ndRoundAdmissionProcess;
        //                cal_Notification_EventDetails.Dt_2ndAdmittedCand = model.Dt_2ndAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_3rdRoundEntryChoiceTrade = model.FromDt_3rdRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_3rdRoundEntryChoiceTrade = model.ToDt_3rdRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.Dt_DbBkp3rdRoundOnlineSeat = model.Dt_DbBkp3rdRoundOnlineSeat;
        //                cal_Notification_EventDetails.FromDt_3rdRoundAdmissionProcess = model.FromDt_3rdRoundAdmissionProcess;
        //                cal_Notification_EventDetails.ToDt_3rdRoundAdmissionProcess = model.ToDt_3rdRoundAdmissionProcess;
        //                cal_Notification_EventDetails.Dt_3rdAdmittedCand = model.Dt_3rdAdmittedCand;
        //                cal_Notification_EventDetails.FromDt_FinalRoundEntryChoiceTrade = model.FromDt_FinalRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.ToDt_FinalRoundEntryChoiceTrade = model.ToDt_FinalRoundEntryChoiceTrade;
        //                cal_Notification_EventDetails.Dt_FinalRoundSeatAllotment = model.Dt_FinalRoundSeatAllotment;
        //                cal_Notification_EventDetails.FromDt_AdmissionFinalRound = model.FromDt_AdmissionFinalRound;
        //                cal_Notification_EventDetails.ToDt_AdmissionFinalRound = model.ToDt_AdmissionFinalRound;
        //                cal_Notification_EventDetails.Dt_CommencementOfTraining = model.Dt_CommencementOfTraining;
        //                cal_Notification_EventDetails.CreatedDate = DateTime.Now;
        //                cal_Notification_EventDetails.CreatedBy = model.user_id;
        //                _db.tbl_Tentative_admsn_eventDetails.Add(cal_Notification_EventDetails);
        //                _db.SaveChanges();
        //            }

        //            tbl_tentative_calendar_of_events_trans cal_Notification_Trans = new tbl_tentative_calendar_of_events_trans();
        //            cal_Notification_Trans.Tentative_admsn_evnt_clndr_Id = CalenderNotification.Tentative_admsn_evnt_clndr_Id;
        //            cal_Notification_Trans.Status_id = 101;
        //            cal_Notification_Trans.Trans_date = DateTime.Now;
        //            cal_Notification_Trans.FlowId = model.user_id;
        //            cal_Notification_Trans.CreatedOn = DateTime.Now;
        //            cal_Notification_Trans.CreatedBy = model.user_id;
        //            cal_Notification_Trans.Remarks = "inserted successfully";
        //            _db.tbl_tentative_calendar_of_events_trans.Add(cal_Notification_Trans);
        //            _db.SaveChanges();

        //            transaction.Commit();
        //            return "Saved";
        //        }

        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return "Failed";
        //        }
        //    }
        //}






        public List<AdmissionNotification> GetUpdateCalendarNtfDLL(int loginId, int? calendarId)
        {
            List<AdmissionNotification> Notifs = null;
            if (calendarId == 0)
            {
                if (loginId == 8)
                {
                    Notifs = (from n in _db.tbl_tentative_calendar_of_events
                              from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                              from c in _db.tbl_course_type_mast.Where(y => y.course_id == n.Course_Id).DefaultIfEmpty()
                              from m in _db.tbl_department_master.Where(z => z.dept_id == n.DeptId).DefaultIfEmpty()
                              select new Models.Admission.AdmissionNotification
                              {
                                  Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                                  Admsn_ntf_num = n.Admsn_ntf_num,
                                  Exam_Notif_Number = n.Admsn_ntf_num,
                                  Exam_Notif_Desc = n.Notif_desc,
                                  Notification_Date = (DateTime)n.Notification_Date,
                                  exam_notif_status_desc = p.statusDesc,
                                  exam_notif_status_id = n.StatusId,
                                  CourseTypeId = n.Course_Id,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = n.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = n.Admsn_notif_doc,
                                  login_id = loginId

                              }).OrderByDescending(x => x.Tentative_admsn_evnt_clndrId).ToList();
                }
                else
                {
                    Notifs = (from n in _db.tbl_tentative_calendar_of_events.Where(t => t.StatusId != 101)
                              from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                              from c in _db.tbl_course_type_mast.Where(y => y.course_id == n.Course_Id).DefaultIfEmpty()
                              from m in _db.tbl_department_master.Where(z => z.dept_id == n.DeptId).DefaultIfEmpty()
                              select new Models.Admission.AdmissionNotification
                              {
                                  Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                                  Admsn_ntf_num = n.Admsn_ntf_num,
                                  Exam_Notif_Number = n.Admsn_ntf_num,
                                  Exam_Notif_Desc = n.Notif_desc,
                                  Notification_Date = (DateTime)n.Notification_Date,
                                  exam_notif_status_desc = p.statusDesc,
                                  exam_notif_status_id = n.StatusId,
                                  CourseTypeId = n.Course_Id,
                                  CourseTypeName = c.course_type_name,
                                  DeptId = n.DeptId,
                                  DeptName = m.dept_description,
                                  SavePath = n.Admsn_notif_doc,
                                  login_id = loginId

                              }).OrderByDescending(x => x.Tentative_admsn_evnt_clndrId).ToList();
                }

            }
            else
            {
                Notifs = (from n in _db.tbl_tentative_calendar_of_events_trans
                          join d in _db.tbl_tentative_calendar_of_events on n.Admsn_tentative_calndr_trans_Id equals d.Tentative_admsn_evnt_clndr_Id
                          join p in _db.tbl_admission_notif_status_mast on n.Status_id equals p.statusId
                          join c in _db.tbl_course_type_mast on d.Course_Id equals c.course_id
                          join m in _db.tbl_department_master on d.DeptId equals m.dept_id
                          //join comnt in _db.tbl_comments_transaction on d.exam_notif_id equals comnt.notification_id into _comnt
                          //from CT in _comnt.DefaultIfEmpty()
                          where d.Tentative_admsn_evnt_clndr_Id == calendarId
                          select new Models.Admission.AdmissionNotification
                          {
                              Admsn_tentative_calndr_transId = n.Admsn_tentative_calndr_trans_Id,
                              Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,

                              Exam_Notif_Desc = d.Notif_desc,
                              Notification_Date = (DateTime)d.Notification_Date,
                              exam_notif_status_desc = p.statusDesc,
                              exam_notif_status_id = n.Status_id,
                              CourseTypeId = d.Course_Id,
                              CourseTypeName = c.course_type_name,
                              DeptId = d.DeptId,
                              DeptName = m.dept_description

                          }).Distinct().ToList();
            }

            return Notifs;
        }
        #endregion
        public List<AdmissionNotification> GetCommentsCalendarFileDLL(int id)
        {
            try
            {
                var notify = (from n in _db.tbl_admission_cal_comments_transaction
                              join e in _db.tbl_tentative_calendar_of_events on n.notification_id equals e.Tentative_admsn_evnt_clndr_Id
                              //join d in _db.tbl_tentative_calendar_of_events_trans on e.Tentative_admsn_evnt_clndr_Id equals d.Tentative_admsn_evnt_clndr_Id
                              join u in _db.tbl_role_master on n.login_id equals u.role_id
                              join bb in _db.tbl_admission_notif_status_mast on n.status_id equals bb.statusId
                              where n.notification_id == id && n.module_id == 2
                              select new AdmissionNotification
                              {
                                  comments = n.comments_transaction_desc,
                                  login_id = n.login_id,
                                  To = u.role_description,
                                  //user_name = u.role_description,
                                  Exam_Notif_Number = e.Admsn_ntf_num,
                                  //created_by = n.ct_created_by,
                                  created_by = n.ct_created_by,
                                  Status = bb.statusDesc,
                                  createdatetime = n.ct_created_on.ToString(),
                                  //FromUser=d.FlowId,
                              }
                 ).ToList();
                var rss = (from aa in notify
                           join uu in _db.tbl_role_master on aa.created_by equals uu.role_id
                           select new AdmissionNotification
                           {
                               comments = aa.comments,
                               Status = aa.Status,
                               To = aa.To,
                               Exam_Notif_Number = aa.Exam_Notif_Number,
                               FromUser = uu.role_description,
                               createdatetime = aa.createdatetime
                           }).ToList();


                return rss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public AdmissionNotification GetAdmissionCalendarDetailsDLL(int id, int roleId)
        {
            try
            {
                var res = (from n in _db.tbl_tentative_calendar_of_events
                           join p in _db.tbl_admission_notif_status_mast on n.StatusId equals p.statusId
                           join c in _db.tbl_course_type_mast on n.Course_Id equals c.course_id
                           join m in _db.tbl_department_master on n.DeptId equals m.dept_id
                           join aaa in _db.tbl_tentative_calendar_of_events_trans on n.Tentative_admsn_evnt_clndr_Id equals aaa.Tentative_admsn_evnt_clndr_Id
                           join bbb in _db.tbl_role_master on aaa.FlowId equals bbb.role_id
                           join Tae in _db.tbl_Tentative_admsn_eventDetails on n.Tentative_admsn_evnt_clndr_Id equals Tae.Tentative_admsn_evnt_clndr_Id
                           join app in _db.tbl_ApplicantType on n.applicantType equals app.ApplicantTypeId into cc
                           from app in cc.DefaultIfEmpty()
                           where n.Tentative_admsn_evnt_clndr_Id == id
                           select new AdmissionNotification
                           {
                               Admsn_tentative_calndr_transId = aaa.Admsn_tentative_calndr_trans_Id,
                               Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                               CourseTypeName = c.course_type_name,
                               DeptName = m.dept_description,
                               Exam_Notif_Number = n.Admsn_ntf_num,
                               NotifDesc = (n.Notif_desc == "" || n.Notif_desc == null ? "NA" : n.Notif_desc),
                               Notification_Date = (DateTime)n.Notification_Date,
                               exam_notif_status_desc = p.statusDesc,
                               StatusId = n.StatusId,
                               ForwardId = aaa.FlowId,
                               Role = bbb.role_description,
                               applicantTypeId = n.applicantType,
                               //applicantTypeName=app.ApplicantType,
                               applicantTypeName = (app.ApplicantType == "" || app.ApplicantType == null ? "NA" : app.ApplicantType),
                               //session = model.YearID,
                               //admissionNotificationId = model.Admsn_notif_Id,
                               NtfEmailId = n.EmailId,

                               FromDt_ApplyingOnlineApplicationForm = Tae.FromDt_ApplyingOnlineApplicationForm,
                               ToDt_ApplyingOnlineApplicationForm = Tae.ToDt_ApplyingOnlineApplicationForm,
                               FromDt_DocVerificationPeriod = Tae.FromDt_DocVerificationPeriod,
                               ToDt_DocVerificationPeriod = Tae.ToDt_DocVerificationPeriod,
                               Dt_DisplayEigibleVerifiedlist = Tae.Dt_DisplayEigibleVerifiedlist,
                               Dt_DBBackupSeatMatrixFInalByDept = Tae.Dt_DBBackupSeatMatrixFInalByDept,
                               Dt_DisplayTentativeGradation = Tae.Dt_DisplayTentativeGradation,
                               Dt_DisplayFinalGradationList = Tae.Dt_DisplayFinalGradationList,
                               Dt_1stListSeatAllotment = Tae.Dt_1stListSeatAllotment,
                               FromDt_1stRoundAdmissionProcess = Tae.FromDt_1stRoundAdmissionProcess,
                               ToDt_1stRoundAdmissionProcess = Tae.ToDt_1stRoundAdmissionProcess,
                               Dt_1stListAdmittedCand = Tae.Dt_1stListAdmittedCand,
                               FromDt_2ndRoundEntryChoiceTrade = Tae.FromDt_2ndRoundEntryChoiceTrade,
                               ToDt_2ndRoundEntryChoiceTrade = Tae.ToDt_2ndRoundEntryChoiceTrade,
                               FromDt_DbBkp2ndRoundOnlineSeat = Tae.FromDt_DbBkp2ndRoundOnlineSeat,
                               ToDt_DbBkp2ndRoundOnlineSeat = Tae.ToDt_DbBkp2ndRoundOnlineSeat,
                               FromDt_2ndRoundAdmissionProcess = Tae.FromDt_2ndRoundAdmissionProcess,
                               ToFDt_2ndRoundAdmissionProcess = Tae.ToFDt_2ndRoundAdmissionProcess,
                               Dt_2ndAdmittedCand = Tae.Dt_2ndAdmittedCand,
                               FromDt_3rdRoundEntryChoiceTrade = Tae.FromDt_3rdRoundEntryChoiceTrade,
                               ToDt_3rdRoundEntryChoiceTrade = Tae.ToDt_3rdRoundEntryChoiceTrade,
                               Dt_DbBkp3rdRoundOnlineSeat = Tae.Dt_DbBkp3rdRoundOnlineSeat,
                               FromDt_3rdRoundAdmissionProcess = Tae.FromDt_3rdRoundAdmissionProcess,
                               ToDt_3rdRoundAdmissionProcess = Tae.ToDt_3rdRoundAdmissionProcess,
                               Dt_3rdAdmittedCand = Tae.Dt_3rdAdmittedCand,
                               FromDt_FinalRoundEntryChoiceTrade = Tae.FromDt_FinalRoundEntryChoiceTrade,
                               ToDt_FinalRoundEntryChoiceTrade = Tae.ToDt_FinalRoundEntryChoiceTrade,
                               Dt_FinalRoundSeatAllotment = Tae.Dt_FinalRoundSeatAllotment,
                               FromDt_AdmissionFinalRound = Tae.FromDt_AdmissionFinalRound,
                               ToDt_AdmissionFinalRound = Tae.ToDt_AdmissionFinalRound,
                               Dt_CommencementOfTraining = Tae.Dt_CommencementOfTraining,
                               // New events  data fetch after changes in db column
                               FromDt_TntGrlistbyAppl = Tae.FromDt_TntGrlistbyAppl,
                               ToDt_TntGrlistbyAppl = Tae.ToDt_TntGrlistbyAppl,
                               FromDt_ApplDocVerif = Tae.FromDt_ApplDocVerif,
                               ToDt_ApplDocVerif = Tae.ToDt_ApplDocVerif,
                               FromDt_AplGrVRoffcr = Tae.FromDt_AplGrVRoffcr,
                               ToDt_AplGrVRoffcr = Tae.ToDt_AplGrVRoffcr,
                               Publishgrdlist2Rnd = Tae.Dt_GradationList2round,
                               Publishgrdlist3Rnd = Tae.Publishgrdlist3Rnd,
                               FromDt_Tentatviveseat = Tae.FromDt_Tentatviveseat,
                               Dt_Notification = Tae.Dt_Notification,
                           }
                        ).FirstOrDefault();
                if (roleId <= 7)
                {
                    if (res.ForwardId < 7)
                    {
                        if (res.StatusId == 102)
                            res.Status = "Reviewed and recommended";
                    }

                }

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AdmissionNotification> GetAdmissionCalendarViewDLL(int id)
        {
            try
            {
                if (id == 8)
                {
                    var notify = (from n in _db.tbl_tentative_calendar_of_events
                                  from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                                  from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.Course_Id).DefaultIfEmpty()
                                  from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                                  from t in _db.tbl_tentative_calendar_of_events_trans.Where(y => y.Tentative_admsn_evnt_clndr_Id == n.Tentative_admsn_evnt_clndr_Id).DefaultIfEmpty()
                                  from v in _db.tbl_role_master.Where(f => f.role_id == t.FlowId).DefaultIfEmpty()
                                  from app in _db.tbl_ApplicantType.Where(a => a.ApplicantTypeId == n.applicantType).DefaultIfEmpty()
                                      // from Tae in _db.tbl_Tentative_admsn_eventDetails.Where(y => y.Tentative_admsn_evnt_clndr_Id == n.Tentative_admsn_evnt_clndr_Id).DefaultIfEmpty()
                                  select new AdmissionNotification
                                  {
                                      Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                                      Exam_Notif_Number = n.Admsn_ntf_num,
                                      Exam_Notif_Desc = n.Notif_desc,
                                      Notification_Date = (DateTime)n.Notification_Date,
                                      //exam_notif_status_desc = p.statusDesc + " - " + v.role_DescShortForm,
                                      exam_notif_status_desc = n.StatusId == 106 ? p.statusDesc : p.statusDesc + " - " + v.role_DescShortForm,
                                      statusRoleDescription = p.statusDesc,
                                      StatusId = n.StatusId,
                                      CourseTypeId = n.Course_Id,
                                      CourseTypeName = c.course_type_name,
                                      DeptId = n.DeptId,
                                      DeptName = m.dept_description,
                                      SavePath = n.Admsn_notif_doc,
                                      FlowId = t.FlowId,
                                      RoleDesc = v.role_DescShortForm,
                                      role_id = id,
                                      applicantTypeId = n.applicantType,
                                      applicantTypeName = (app.ApplicantType == "" || app.ApplicantType == null ? "NA" : app.ApplicantType)
                                      //login_id = id,
                                      // FromDt_ApplyingOnlineApplicationForm=(DateTime)Tae.FromDt_ApplyingOnlineApplicationForm                                     
                                  }).OrderByDescending(x => x.Tentative_admsn_evnt_clndrId).ToList();
                    foreach (var res in notify)
                    {
                        if (res.StatusId == 109)
                            res.exam_notif_status_desc = "Approved - Pending for Publish -" + res.RoleDesc;

                    }
                    return notify;
                }
                else
                {
                    var notify = (from n in _db.tbl_tentative_calendar_of_events
                                  from p in _db.tbl_admission_notif_status_mast.Where(x => x.statusId == n.StatusId).DefaultIfEmpty()
                                  from c in _db.tbl_course_type_mast.Where(t => t.course_id == n.Course_Id).DefaultIfEmpty()
                                  from m in _db.tbl_department_master.Where(y => y.dept_id == n.DeptId).DefaultIfEmpty()
                                  from t in _db.tbl_tentative_calendar_of_events_trans.Where(y => y.Tentative_admsn_evnt_clndr_Id == n.Tentative_admsn_evnt_clndr_Id).DefaultIfEmpty()
                                  from v in _db.tbl_role_master.Where(f => f.role_id == t.FlowId).DefaultIfEmpty()
                                  from app in _db.tbl_ApplicantType.Where(a => a.ApplicantTypeId == n.applicantType).DefaultIfEmpty()
                                  where (n.StatusId != 101)
                                  select new AdmissionNotification
                                  {
                                      Tentative_admsn_evnt_clndrId = n.Tentative_admsn_evnt_clndr_Id,
                                      Exam_Notif_Number = n.Admsn_ntf_num,
                                      Exam_Notif_Desc = n.Notif_desc,
                                      Notification_Date = (DateTime)n.Notification_Date,
                                      //exam_notif_status_desc = p.statusDesc + " - " + v.role_DescShortForm,
                                      exam_notif_status_desc = n.StatusId == 106 ? p.statusDesc : p.statusDesc + " - " + v.role_DescShortForm,
                                      statusRoleDescription = p.statusDesc,
                                      StatusId = n.StatusId,
                                      CourseTypeId = n.Course_Id,
                                      CourseTypeName = c.course_type_name,
                                      DeptId = n.DeptId,
                                      DeptName = m.dept_description,
                                      SavePath = n.Admsn_notif_doc,
                                      FlowId = t.FlowId,
                                      RoleDesc = v.role_description,
                                      role_id = id,
                                      applicantTypeId = n.applicantType,
                                      applicantTypeName = (app.ApplicantType == "" || app.ApplicantType == null ? "NA" : app.ApplicantType)
                                  }
             ).OrderByDescending(x => x.Tentative_admsn_evnt_clndrId).ToList();

                    return notify;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ForwardAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).OrderByDescending(y => y.Admsn_tentative_calndr_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();

                    if (rs != null)
                    {
                        tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                        inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.Status_id = rs.Status_id;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);

                        tbl_tentative_calendar_of_events_trans iss = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == rs.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();
                        //tbl_tentative_calendar_of_events_trans iss = new tbl_tentative_calendar_of_events_trans();
                        iss.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        iss.Trans_date = rs.Trans_date;
                        if (loginRole < 8)
                        {
                            iss.Status_id = 111;
                        }
                        else
                        {
                            iss.Status_id = 102;
                        }
                        iss.Remarks = remarks;
                        iss.FlowId = roleId;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        //_db.tbl_tentative_calendar_of_events_trans.Add(iss);
                        //res.FlowId = roleId;
                        if (loginRole < 8)
                            res.StatusId = 111;
                        else
                            res.StatusId = 102;

                        res.Remarks = remarks;
                        //res.StatusId = 102;
                        //res.Remarks = remarks;
                        //if (loginRole < 8)
                        //    res.StatusId = 111;
                        //else
                        //    res.StatusId = 102;

                        //res.Remarks = remarks;
                        _db.SaveChanges();

                        tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                        comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                        comments.module_id = 2;
                        if (loginRole < 8)
                            comments.status_id = 111;
                        else
                            comments.status_id = 102;
                        comments.login_id = roleId;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_cal_comments_transaction.Add(comments);
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

        public bool ApproveAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).OrderByDescending(y => y.Admsn_tentative_calndr_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();

                    //var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                    //var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                    if (rs != null)
                    {
                        tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                        inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.Status_id = rs.Status_id;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);

                        tbl_tentative_calendar_of_events_trans iss = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == rs.Tentative_admsn_evnt_clndr_Id).FirstOrDefault(); /*new tbl_tentative_calendar_of_events_trans();*/
                        iss.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.Status_id = 109;
                        iss.Remarks = remarks;
                        iss.FlowId = 8;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        //_db.tbl_tentative_calendar_of_events_trans.Add(iss);
                        _db.SaveChanges();
                        res.StatusId = 109;
                        res.Remarks = remarks;
                        //res.FlowId = 8;

                        tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                        comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                        comments.module_id = 2;
                        comments.status_id = 109;
                        comments.login_id = 8;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_cal_comments_transaction.Add(comments);
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

        //Extracode **
        public bool ChangesAdmissionCalendarNotificationDLL(int loginId, int roleId, int admiNotifId, string remarks)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).OrderByDescending(y => y.Admsn_tentative_calndr_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                    if (rs != null)
                    {
                        tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                        inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.Status_id = rs.Status_id;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);

                        tbl_tentative_calendar_of_events_trans iss = new tbl_tentative_calendar_of_events_trans();
                        iss.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.Status_id = 103;
                        iss.Remarks = remarks;
                        iss.FlowId = roleId;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        _db.tbl_tentative_calendar_of_events_trans.Add(iss);
                        res.StatusId = 103;
                        res.Remarks = remarks;

                        tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                        comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                        comments.module_id = 2;
                        comments.status_id = 103;
                        comments.login_id = loginId;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginId;
                        _db.tbl_admission_cal_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }



        }
        //***

        public string PublishAdmissionCalNotification(int notificationId, int loginId, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == notificationId).FirstOrDefault();
                    var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == notificationId).OrderByDescending(y => y.Admsn_tentative_calndr_trans_Id).FirstOrDefault();
                    if (res != null)
                    {
                        tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                        inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.Status_id = rs.Status_id;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);

                        tbl_tentative_calendar_of_events_trans iss = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == rs.Tentative_admsn_evnt_clndr_Id).FirstOrDefault();   // new tbl_tentative_calendar_of_events_trans();
                        iss.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.Status_id = 106;
                        iss.Remarks = "published";
                        iss.FlowId = 8;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        // _db.tbl_tentative_calendar_of_events_trans.Add(iss);
                        _db.SaveChanges();
                        res.StatusId = 106;
                        res.Remarks = "published";

                        tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                        comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                        comments.module_id = 2;
                        comments.status_id = 106;
                        comments.login_id = loginRole;
                        comments.comments_transaction_desc = "published";
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        comments.is_published = 1;
                        _db.tbl_admission_cal_comments_transaction.Add(comments);
                        _db.SaveChanges();
                        transaction.Commit();

                        return res.Admsn_ntf_num;
                        //return true;
                    }
                    return "failed";
                    //return false;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public bool SendbackAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            try
            {
                var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                var sendbackid = _db.tbl_tentative_calendar_of_events_trans_Hist.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).OrderByDescending(a => a.Admsn_tentative_calndr_transHis_Id).Select(y => y.FlowId).First();
                if (rs != null)
                {
                    tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                    inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                    inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                    inn.Trans_date = rs.Trans_date;
                    inn.Status_id = rs.Status_id;
                    inn.Remarks = rs.Remarks;
                    inn.FlowId = rs.FlowId;
                    inn.CreatedOn = rs.CreatedOn;
                    inn.CreatedBy = rs.CreatedBy;
                    _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);
                    rs.FlowId = roleId;
                    rs.Status_id = 108;
                    res.StatusId = 108;
                    tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                    comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                    comments.module_id = 2;
                    comments.status_id = 108;
                    comments.login_id = roleId;
                    comments.comments_transaction_desc = remarks;
                    comments.ct_created_on = DateTime.Now;
                    comments.ct_created_by = loginRole;
                    _db.tbl_admission_cal_comments_transaction.Add(comments);
                    _db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ChangesAdmissionCalNotification(int loginId, int roleId, int admiNotifId, string remarks, int loginRole)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = _db.tbl_tentative_calendar_of_events_trans.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).OrderByDescending(y => y.Admsn_tentative_calndr_trans_Id).Take(1).FirstOrDefault();
                    var res = _db.tbl_tentative_calendar_of_events.Where(x => x.Tentative_admsn_evnt_clndr_Id == admiNotifId).FirstOrDefault();
                    if (rs != null)
                    {
                        tbl_tentative_calendar_of_events_trans_Hist inn = new tbl_tentative_calendar_of_events_trans_Hist();
                        inn.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        inn.Tentative_admsn_evnt_clndr_Id = rs.Tentative_admsn_evnt_clndr_Id;
                        inn.Trans_date = rs.Trans_date;
                        inn.Status_id = rs.Status_id;
                        inn.Remarks = rs.Remarks;
                        inn.FlowId = rs.FlowId;
                        inn.CreatedOn = rs.CreatedOn;
                        inn.CreatedBy = rs.CreatedBy;
                        _db.tbl_tentative_calendar_of_events_trans_Hist.Add(inn);

                        tbl_tentative_calendar_of_events_trans iss = new tbl_tentative_calendar_of_events_trans();
                        iss.Admsn_tentative_calndr_trans_Id = rs.Admsn_tentative_calndr_trans_Id;
                        iss.Trans_date = rs.Trans_date;
                        iss.Status_id = 103;
                        iss.Remarks = remarks;
                        iss.FlowId = roleId;
                        iss.CreatedOn = DateTime.Now;
                        iss.CreatedBy = loginId;
                        _db.tbl_tentative_calendar_of_events_trans.Add(iss);

                        if (res.StatusId == 106)
                            res.StatusId = 107;
                        else
                            res.StatusId = 103;

                        res.Remarks = remarks;
                        //res.FlowId = roleId;

                        tbl_admission_cal_comments_transaction comments = new tbl_admission_cal_comments_transaction();
                        comments.notification_id = rs.Tentative_admsn_evnt_clndr_Id;
                        comments.module_id = 2;
                        comments.status_id = 103;
                        comments.login_id = roleId;
                        comments.comments_transaction_desc = remarks;
                        comments.ct_created_on = DateTime.Now;
                        comments.ct_created_by = loginRole;
                        _db.tbl_admission_cal_comments_transaction.Add(comments);
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

        //Calendar CourseList
        public SelectList GetCourseListCalendarDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_course_type_mast.Select(x => new SelectListItem
            {
                Text = x.course_type_name.ToString(),
                Value = x.course_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Course Type",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }
        public SelectList GetSessionListCalendarDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_Year.Select(x => new SelectListItem
            {
                Text = x.Year.ToString(),
                Value = x.YearID.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Year",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }
        public SelectList GetAdmissionNotifNoListCalendarDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_admsn_ntf_details.Select(x => new SelectListItem
            {
                Text = x.AdmsnNtfNum.ToString(),
                Value = x.Admsn_notif_Id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Notification Number",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }
        //Get Calendar DepartmentList
        public SelectList GetDepartmentListCalendarDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_department_master.Select(x => new SelectListItem
            {
                Text = x.dept_description.ToString(),
                Value = x.dept_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Department Type",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        public SelectList GetCalendarNotfyDescListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_notification_description.Select(x => new SelectListItem
            {
                Text = x.notification_description.ToString(),
                Value = x.notif_decr_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Notification Description",

            };
            CoursetList.Insert(0, ProposalList);
            return new SelectList(CoursetList, "Value", "Text");
        }

        //Get DepartmentList
        public SelectList GetDepartmentListDLL()
        {
            List<SelectListItem> CoursetList = _db.tbl_department_master.Where(y => y.dept_id == (int)CmnClass.Dept.Admission).Select(x => new SelectListItem
            {
                Text = x.dept_description.ToString(),
                Value = x.dept_id.ToString()
            }).ToList();
            foreach (var item in CoursetList)
            {
                item.Selected = true;
            }

            return new SelectList(CoursetList, "Value", "Text");
        }
        public List<AdmissionNotification> AdmissionCalendarNotificationBox()
        {
            try
            {
                var Notifs = (
                                            from d in _db.tbl_tentative_calendar_of_events
                                            join p in _db.tbl_admission_notif_status_mast on d.StatusId equals p.statusId
                                            join c in _db.tbl_course_type_mast on d.Course_Id equals c.course_id
                                            join m in _db.tbl_department_master on d.DeptId equals m.dept_id
                                            where d.StatusId == 106
                                            select new Models.Admission.AdmissionNotification
                                            {
                                                Tentative_admsn_evnt_clndrId = d.Tentative_admsn_evnt_clndr_Id,
                                                Exam_Notif_Number = d.Admsn_ntf_num,
                                                Exam_Notif_Desc = d.Notif_desc,
                                                Exam_notif_date = (DateTime)d.Notification_Date,
                                                exam_notif_status_desc = p.statusDesc,
                                                exam_notif_status_id = d.StatusId,
                                                CourseTypeId = d.Course_Id,
                                                CourseTypeName = c.course_type_name,
                                                DeptId = d.DeptId,
                                                DeptName = m.dept_description,
                                                SavePath = d.Admsn_notif_doc

                                            }
                                            ).OrderByDescending(x => x.Tentative_admsn_evnt_clndrId).ToList();
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion calendar

        #region allocation of seat

        public string InsertRulesAllocationMasterDLL(SeatAllocation objSeatAllocation)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    if (objSeatAllocation.Exam_Year.ToString().Length > 3)
                        objSeatAllocation.Exam_Year = _db.tbl_Year.Where(s => s.Year.Contains(objSeatAllocation.Exam_Year.ToString())).Select(m => m.YearID).FirstOrDefault();
                    int ExistingRecordForUpdate = 0;
                    int Rules_allocation_master_id = objSeatAllocation.Rules_allocation_masterid;
                    if (Rules_allocation_master_id != 0)
                    {
                        ExistingRecordForUpdate = (from ram in _db.Tbl_rules_allocation_master
                                                   where ram.Rules_allocation_master_id == objSeatAllocation.Rules_allocation_masterid &&
                                                   ram.Exam_Year == objSeatAllocation.Exam_Year && ram.CourseId == objSeatAllocation.CourseId
                                                   && ram.IsActive == true
                                                   select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();
                    }
                    else
                    {
                        ExistingRecordForUpdate = (from ram in _db.Tbl_rules_allocation_master
                                                   where ram.Exam_Year == objSeatAllocation.Exam_Year && ram.CourseId == objSeatAllocation.CourseId
                                                   && ram.IsActive == true
                                                   select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();
                    }
                    if (ExistingRecordForUpdate == 0)
                    {
                        #region First Time Insert Tbl_rules_allocation_master

                        Tbl_rules_allocation_master objtbl_rules_allocation_master = new Tbl_rules_allocation_master();
                        objtbl_rules_allocation_master.Exam_Year = objSeatAllocation.Exam_Year;
                        objtbl_rules_allocation_master.Status_Id = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                        objtbl_rules_allocation_master.Trans_Date = DateTime.Now;
                        objtbl_rules_allocation_master.CreatedOn = DateTime.Now;
                        objtbl_rules_allocation_master.ModifiedOn = DateTime.Now;
                        objtbl_rules_allocation_master.CourseId = objSeatAllocation.CourseId;
                        objtbl_rules_allocation_master.FlowId = objSeatAllocation.FlowId != 0 ? objSeatAllocation.FlowId : objSeatAllocation.ModifiedBy;
                        objtbl_rules_allocation_master.CreatedBy = objSeatAllocation.ModifiedBy;
                        objtbl_rules_allocation_master.IsActive = true;
                        objtbl_rules_allocation_master.ModifiedBy = objSeatAllocation.ModifiedBy;
                        objtbl_rules_allocation_master.Remarks = objSeatAllocation.Remarks;
                        _db.Tbl_rules_allocation_master.Add(objtbl_rules_allocation_master);
                        _db.SaveChanges();

                        #endregion

                        Rules_allocation_master_id = (from ram in _db.Tbl_rules_allocation_master
                                                      orderby ram.CreatedOn descending
                                                      select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();

                        Tbl_rules_allocation_master_history objTbl_rules_allocation_master_history = new Tbl_rules_allocation_master_history();
                        objTbl_rules_allocation_master_history.Rules_allocation_master_id = Rules_allocation_master_id;
                        objTbl_rules_allocation_master_history.Exam_Year = objSeatAllocation.Exam_Year;
                        objTbl_rules_allocation_master_history.Status_Id = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                        objTbl_rules_allocation_master_history.Trans_Date = DateTime.Now;
                        objTbl_rules_allocation_master_history.CreatedOn = DateTime.Now;
                        objTbl_rules_allocation_master_history.ModifiedOn = DateTime.Now;
                        objTbl_rules_allocation_master_history.CourseId = objSeatAllocation.CourseId;
                        objTbl_rules_allocation_master_history.FlowId = objSeatAllocation.FlowId != 0 ? objSeatAllocation.FlowId : objSeatAllocation.ModifiedBy;
                        objTbl_rules_allocation_master_history.CreatedBy = objSeatAllocation.FlowId;
                        objTbl_rules_allocation_master_history.ModifiedBy = objSeatAllocation.ModifiedBy;
                        objTbl_rules_allocation_master_history.Remarks = objSeatAllocation.Remarks;
                        _db.Tbl_rules_allocation_master_history.Add(objTbl_rules_allocation_master_history);
                        _db.SaveChanges();
                    }
                    else
                    {
                        Rules_allocation_master_id = (from ram in _db.Tbl_rules_allocation_master
                                                      orderby ram.CreatedOn descending
                                                      select ram.Rules_allocation_master_id).FirstOrDefault();
                        Tbl_rules_allocation_master objtbl_rules_allocation_master = new Tbl_rules_allocation_master();
                        var update_query = _db.Tbl_rules_allocation_master.Where(s => s.Rules_allocation_master_id == Rules_allocation_master_id).FirstOrDefault();
                        update_query.Exam_Year = objSeatAllocation.Exam_Year;
                        update_query.Status_Id = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                        update_query.Trans_Date = DateTime.Now;
                        update_query.CreatedOn = DateTime.Now;
                        update_query.ModifiedOn = DateTime.Now;
                        update_query.CourseId = objSeatAllocation.CourseId;
                        update_query.FlowId = objSeatAllocation.FlowId != 0 ? objSeatAllocation.FlowId : objSeatAllocation.ModifiedBy;
                        update_query.Remarks = objSeatAllocation.Remarks;
                        update_query.IsActive = true;
                        _db.SaveChanges();

                        Tbl_rules_allocation_master_history objTbl_rules_allocation_master_history = new Tbl_rules_allocation_master_history();
                        objTbl_rules_allocation_master_history.Rules_allocation_master_id = objSeatAllocation.Rules_allocation_masterid;
                        objTbl_rules_allocation_master_history.Exam_Year = objSeatAllocation.Exam_Year;
                        objTbl_rules_allocation_master_history.Status_Id = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                        objTbl_rules_allocation_master_history.Trans_Date = DateTime.Now;
                        objTbl_rules_allocation_master_history.CreatedOn = DateTime.Now;
                        objTbl_rules_allocation_master_history.ModifiedOn = DateTime.Now;
                        objTbl_rules_allocation_master_history.CourseId = objSeatAllocation.CourseId;
                        objTbl_rules_allocation_master_history.FlowId = objSeatAllocation.FlowId != 0 ? objSeatAllocation.FlowId : objSeatAllocation.ModifiedBy;
                        objTbl_rules_allocation_master_history.ModifiedBy = objSeatAllocation.ModifiedBy;
                        objTbl_rules_allocation_master_history.Remarks = objSeatAllocation.Remarks;
                        _db.Tbl_rules_allocation_master_history.Add(objTbl_rules_allocation_master_history);
                        _db.SaveChanges();
                    }

                    #region .. Vertical Rules ..
                    int ExistingRecordForUpdateVertical = 0;
                    ExistingRecordForUpdateVertical = (from ram in _db.tbl_Vertical_rule_value
                                                       where ram.Rules_allocation_master_id == Rules_allocation_master_id
                                                       select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();

                    int verticalCount = _db.tbl_Vertical_rules.Count();
                    if (ExistingRecordForUpdateVertical == 0)
                    {
                        for (int verticalCountLoop = 0; verticalCountLoop < verticalCount; verticalCountLoop++)
                        {
                            tbl_Vertical_rule_value objtbl_Vertical_rule_value = new tbl_Vertical_rule_value();
                            objtbl_Vertical_rule_value.Vertical_rules_id = (verticalCountLoop + 1);
                            objtbl_Vertical_rule_value.RuleValue = objSeatAllocation.ConsolidatedVerticalRules[verticalCountLoop];
                            objtbl_Vertical_rule_value.Rules_allocation_master_id = Rules_allocation_master_id;
                            objtbl_Vertical_rule_value.StatusId = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                            objtbl_Vertical_rule_value.Remarks = objSeatAllocation.Remarks;
                            objtbl_Vertical_rule_value.CreatedBy = 1;
                            objtbl_Vertical_rule_value.CreatedOn = DateTime.Now;
                            _db.tbl_Vertical_rule_value.Add(objtbl_Vertical_rule_value);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        List<tbl_Vertical_rule_value> UpdateVertical = (from c in _db.tbl_Vertical_rule_value
                                                                        orderby c.Vertical_rules_value_id ascending
                                                                        where c.Rules_allocation_master_id == Rules_allocation_master_id
                                                                        select c).ToList();
                        if (UpdateVertical != null)
                        {
                            for (int i = 0; i < UpdateVertical.Count; i++)
                            {
                                UpdateVertical[i].RuleValue = objSeatAllocation.ConsolidatedVerticalRules[i];
                                _db.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    #region .. Horizontal Rules ..
                    int horizontalCount = _db.Tbl_horizontal_rules.Count();
                    int HorizontalRulCnt = 3; //Remove before deployment

                    int ExistingRecordForUpdateHorizontal = 0;
                    ExistingRecordForUpdateHorizontal = (from ram in _db.tbl_horizontal_rules_values
                                                         where ram.Rules_allocation_master_id == objSeatAllocation.Rules_allocation_masterid
                                                         select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();


                    int pkvalueforHorizontal = 0;
                    if (ExistingRecordForUpdateHorizontal == 0)
                    {
                        for (int horizontalCountLoop = 0; horizontalCountLoop < horizontalCount; horizontalCountLoop++)
                        {
                            if (horizontalCountLoop == 0)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.Women;
                            else if (horizontalCountLoop == 1)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.PersonsDisabilities;
                            else if (horizontalCountLoop == 2)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.ExService;
                            else if (horizontalCountLoop == 3)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.KannadaMedium;
                            else if (horizontalCountLoop == 4)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.LandLoser;
                            else if (horizontalCountLoop == 5)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.KashmiriMigrants;
                            else if (horizontalCountLoop == 6)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.EconomicWeaker;
                            else if (horizontalCountLoop == 7)
                                pkvalueforHorizontal = (int)CmnClass.pkvalueForHorizontal.GeneralPool;

                            if (horizontalCountLoop != 4 && horizontalCountLoop != 5)
                            {
                                tbl_horizontal_rules_values objtbl_horizontal_rules_values = new tbl_horizontal_rules_values();
                                objtbl_horizontal_rules_values.Horizontal_rules_id = pkvalueforHorizontal;
                                objtbl_horizontal_rules_values.RuleValue = objSeatAllocation.ConsolidatedHorizontalRules[horizontalCountLoop];
                                objtbl_horizontal_rules_values.Rules_allocation_master_id = Rules_allocation_master_id;
                                objtbl_horizontal_rules_values.StatusId = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                                objtbl_horizontal_rules_values.Remarks = objSeatAllocation.Remarks;
                                objtbl_horizontal_rules_values.CreatedOn = DateTime.Now;
                                objtbl_horizontal_rules_values.CreatedBy = 1;
                                _db.tbl_horizontal_rules_values.Add(objtbl_horizontal_rules_values);
                                _db.SaveChanges();
                                HorizontalRulCnt++;
                            }
                        }
                    }
                    else
                    {
                        List<tbl_horizontal_rules_values> UpdateHorizontal = (from c in _db.tbl_horizontal_rules_values
                                                                              orderby c.Horizontal_rules_value_id ascending
                                                                              where c.Rules_allocation_master_id == Rules_allocation_master_id
                                                                              select c).ToList();
                        if (UpdateHorizontal != null)
                        {
                            for (int i = 0, j = 0; i < horizontalCount; i++)
                            {
                                if (i != 4 && i != 5)
                                {
                                    UpdateHorizontal[j].RuleValue = objSeatAllocation.ConsolidatedHorizontalRules[i];
                                    j++;
                                    _db.SaveChanges();
                                }
                            }
                        }
                    }

                    #endregion

                    //#region .. Grades Rules ..
                    //int GradesCount = _db.tbl_Grades.Count();

                    //int ExistingRecordForUpdateGrades = 0;
                    //ExistingRecordForUpdateGrades = (from ram in _db.tbl_Grade_percentage_Value
                    //                                 where ram.Rules_allocation_master_id == objSeatAllocation.Rules_allocation_masterid
                    //                                 select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();

                    //if (ExistingRecordForUpdateGrades == 0)
                    //{
                    //    for (int GradesCountLoop = 0; GradesCountLoop < GradesCount; GradesCountLoop++)
                    //    {
                    //        #region .. update syllabus in grade ..

                    //        tbl_Grade_percentage objtbl_Grade_percentage = new tbl_Grade_percentage();
                    //        objtbl_Grade_percentage.Grade_Id = (GradesCountLoop + 1);
                    //        objtbl_Grade_percentage.Syllabus_type_id = objSeatAllocation.Syllabus_type_id;
                    //        objtbl_Grade_percentage.IsActive = 1;
                    //        objtbl_Grade_percentage.CreatedOn = DateTime.Now;
                    //        objtbl_Grade_percentage.CreatedBy = 1;
                    //        _db.tbl_Grade_percentage.Add(objtbl_Grade_percentage);
                    //        _db.SaveChanges();

                    //        #endregion

                    //        #region .. tbl_Grade_percentage ..

                    //        int Grade_percentage_id = (from gp in _db.tbl_Grade_percentage
                    //                                   orderby gp.CreatedOn descending
                    //                                   select gp.Grade_percentage_id).Take(1).FirstOrDefault();

                    //        tbl_Grade_percentage_Value objtbl_Grade_percentage_Value = new tbl_Grade_percentage_Value();
                    //        objtbl_Grade_percentage_Value.Grade_percentage_id = Grade_percentage_id;
                    //        objtbl_Grade_percentage_Value.Rules_allocation_master_id = Rules_allocation_master_id;
                    //        objtbl_Grade_percentage_Value.RuleValue = objSeatAllocation.ConsolidatedGradeRules[GradesCountLoop];
                    //        objtbl_Grade_percentage_Value.StatusId = 5;
                    //        objtbl_Grade_percentage_Value.Remarks = "Submitted";
                    //        objtbl_Grade_percentage_Value.CreatedOn = DateTime.Now;
                    //        objtbl_Grade_percentage_Value.CreatedBy = 1;
                    //        _db.tbl_Grade_percentage_Value.Add(objtbl_Grade_percentage_Value);
                    //        _db.SaveChanges();

                    //        #endregion
                    //    }
                    //}
                    //else
                    //{

                    //}

                    //#endregion

                    #region .. HyderbadNonHyderabad Rules ..
                    int HydKarRulCnt = 1; //Remove before deployment

                    int ExistingRecordForUpdateRegion = 0;
                    ExistingRecordForUpdateRegion = (from ram in _db.tbl_HYD_kar_rules_value
                                                     where ram.Rules_allocation_master_id == objSeatAllocation.Rules_allocation_masterid
                                                     select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();

                    if (ExistingRecordForUpdateRegion == 0)
                    {
                        for (int GradesCountLoop = 0; GradesCountLoop < 4; GradesCountLoop++)
                        {
                            tbl_HYD_kar_rules_value objtbl_HYD_kar_rules_value = new tbl_HYD_kar_rules_value();
                            objtbl_HYD_kar_rules_value.Hyd_NonHyd_rules_id = (HydKarRulCnt + 1);
                            objtbl_HYD_kar_rules_value.Rules_allocation_master_id = Rules_allocation_master_id;
                            objtbl_HYD_kar_rules_value.RuleValue = objSeatAllocation.ConsolidatedHyderabadRegion[GradesCountLoop];
                            objtbl_HYD_kar_rules_value.StatusId = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                            objtbl_HYD_kar_rules_value.Remarks = objSeatAllocation.Remarks;
                            objtbl_HYD_kar_rules_value.CreatedOn = DateTime.Now;
                            objtbl_HYD_kar_rules_value.CreatedBy = 1;
                            _db.tbl_HYD_kar_rules_value.Add(objtbl_HYD_kar_rules_value);
                            _db.SaveChanges();
                            HydKarRulCnt++;
                        }
                    }
                    else
                    {
                        List<tbl_HYD_kar_rules_value> UpdateRegion = (from c in _db.tbl_HYD_kar_rules_value
                                                                      orderby c.Hyd_NonHyd_rules_val_id ascending
                                                                      where c.Rules_allocation_master_id == Rules_allocation_master_id
                                                                      select c).ToList();
                        if (UpdateRegion != null)
                        {
                            for (int i = 0; i < UpdateRegion.Count; i++)
                            {
                                UpdateRegion[i].RuleValue = objSeatAllocation.ConsolidatedHyderabadRegion[i];
                                _db.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region .. Other Rules ..
                    int OtherRulesCount = _db.Tbl_other_rules.Count();

                    int ExistingRecordForUpdateOtherRes = 0;
                    ExistingRecordForUpdateOtherRes = (from ram in _db.tbl_other_rules_value
                                                       where ram.Rules_allocation_master_id == objSeatAllocation.Rules_allocation_masterid
                                                       select ram.Rules_allocation_master_id).Take(1).FirstOrDefault();

                    if (ExistingRecordForUpdateOtherRes == 0)
                    {
                        for (int GradesCountLoop = 0; GradesCountLoop < OtherRulesCount; GradesCountLoop++)
                        {
                            tbl_other_rules_value objtbl_other_rules_value = new tbl_other_rules_value();
                            objtbl_other_rules_value.Other_rules_id = (GradesCountLoop + 1);
                            objtbl_other_rules_value.Rules_allocation_master_id = Rules_allocation_master_id;
                            objtbl_other_rules_value.RuleValue = objSeatAllocation.ConsolidatedOtherRules[GradesCountLoop];
                            objtbl_other_rules_value.StatusId = objSeatAllocation.StatusId != 0 ? objSeatAllocation.StatusId : 5;
                            objtbl_other_rules_value.Remarks = objSeatAllocation.Remarks;
                            objtbl_other_rules_value.CreatedOn = DateTime.Now;
                            objtbl_other_rules_value.CreatedBy = 1;
                            _db.tbl_other_rules_value.Add(objtbl_other_rules_value);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        List<tbl_other_rules_value> UpdateRegion = (from c in _db.tbl_other_rules_value
                                                                    orderby c.Other_rules_value_id ascending
                                                                    where c.Rules_allocation_master_id == Rules_allocation_master_id
                                                                    select c).ToList();
                        if (UpdateRegion != null)
                        {
                            for (int i = 0; i < UpdateRegion.Count; i++)
                            {
                                UpdateRegion[i].RuleValue = objSeatAllocation.ConsolidatedOtherRules[i];
                                _db.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region .. SeatType ..
                    var chkExistingRec = _db.tbl_seattype_master.Where(a => a.seattype_Id == objSeatAllocation.Seat_typeId).FirstOrDefault();
                    tbl_seattype_master seattype_master = new tbl_seattype_master();
                    if (chkExistingRec == null)
                    {

                        //seattype_master.Id = (GradesCountLoop + 1);
                        seattype_master.seattype_Id = objSeatAllocation.Seat_typeId;
                        seattype_master.Govt_seats = objSeatAllocation.Govtseats;
                        seattype_master.Management_seats = objSeatAllocation.Managementseats;
                        seattype_master.IsActive = true;
                        seattype_master.CreatedOn = DateTime.Now;
                        seattype_master.CreatedBy = 5;
                        _db.tbl_seattype_master.Add(seattype_master);
                        _db.SaveChanges();
                    }
                    else
                    {

                        seattype_master.Govt_seats = objSeatAllocation.Govtseats;
                        seattype_master.Management_seats = objSeatAllocation.Managementseats;
                        seattype_master.CreatedOn = DateTime.Now;
                        _db.SaveChanges();
                    }
                    #endregion
                    transaction.Complete();
                }

                return "Success";
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return "failed";
            }
        }

        public List<SeatAllocation> GetSyllabusDLL()
        {
            var res = (from a in _db.tbl_syllabus_type
                       where a.IsActive == 1
                       select new SeatAllocation
                       {
                           Syllabus_type_id = a.Syllabus_type_id,
                           Syllabus_type = a.Syllabus_type
                       }).ToList();
            return res;
        }

        public List<InstituteWiseAdmission> GetAdmissionRoundsDLL()
        {
            var res = (from a in _db.tbl_ApplicantAdmissionRounds
                       where a.IsActive == true
                       select new InstituteWiseAdmission
                       {
                           ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                           RoundList = a.RoundList
                       }).ToList();
            return res;
        }

        //public List<SeatAllocation> GetYearTypeDLL()
        //{
        //    try
        //    {
        //        var res = (from a in _db.tbl_Year
        //                       // where a.IsActive == 1
        //                   select new SeatAllocation
        //                   {
        //                       YearID = a.YearID,
        //                       Year = a.Year,
        //                       IsActive = a.IsActive
        //                   }).ToList();
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SeatAllocation> GetYearTypeDLL(bool isActiveCheck = true)
        {
            try
            {
                var res = (from a in _db.tbl_Year
                               //where !isActiveCheck || a.IsActive == true
                           select new SeatAllocation
                           {
                               YearID = a.YearID,
                               Year = a.Year,
                               IsActive = a.IsActive
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region .. ExamCourse ..

        public List<SeatAllocation> GetSeatAllocationExamCourseDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId
                       }).ToList();

            return res;
        }
        #endregion

        #region .. Vertical ..

        public List<SeatAllocation> GetSeatAllocationVerticalDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join tvrv in _db.tbl_Vertical_rule_value on tram.Rules_allocation_master_id equals tvrv.Rules_allocation_master_id
                       join tvr in _db.tbl_Vertical_rules on tvrv.Vertical_rules_id equals tvr.Vertical_rules_id
                       join tsm in _db.tbl_status_master on tvrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Vertical_rulesid = tvrv.Vertical_rules_id,
                           RuleValue = tvrv.RuleValue,
                           Vertical_Rules = tvr.Vertical_Rules,
                           CourseId = tram.CourseId,
                           Exam_Year = tram.Exam_Year,
                           Remarks = tvrv.Remarks,
                           StatusName = tsm.StatusName,
                           Rules_allocation_masterid = tram.Rules_allocation_master_id
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Horizontal ..

        public List<SeatAllocation> GetSeatAllocationHorizontalDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
                           Horizontal_rules = thr.Horizontal_rules,
                           RuleValue = thrv.RuleValue,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }
        #endregion

        #region .. HydRegion  ..

        public List<SeatAllocation> GetseatallocationHydDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                       join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                       join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
                           Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
                           RuleValue = thrv.RuleValue,
                           Candidates_type = thnc.Candidates_type,
                           Region_type = thnr.Region_type,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName

                       }).ToList();

            return res;
        }
        #endregion

        #region .. GradeList  ..

        public List<SeatAllocation> GetSeatAllocationGradeDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
                       join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
                       join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
                       join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
                       join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           RuleValue = tgpv.RuleValue,
                           Grade_percentageid = tgpv.Grade_percentage_id,
                           Syllabus_type_id = tgp.Syllabus_type_id,
                           Grade_Id = tgp.Grade_Id,
                           GradGrades = tg.GradGrades,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = tgpv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }
        #endregion

        #region .. Other Rules  ..

        public List<SeatAllocation> GetSeatAllocationOtherRulesDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
                       join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
                       join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == 20 && tram.IsActive == true
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Other_rulesid = tor.Other_rules_id,
                           RuleValue = torv.RuleValue,
                           OtherRules = tor.OtherRules,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = torv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Vertical ..

        public List<SeatAllocation> GetSeatAllocationVerticalDLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            if (tabNameValue == "tab1" || tabNameValue == "tab2" || tabNameValue == "tab4")
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join tvrv in _db.tbl_Vertical_rule_value on tram.Rules_allocation_master_id equals tvrv.Rules_allocation_master_id
                           join tvr in _db.tbl_Vertical_rules on tvrv.Vertical_rules_id equals tvr.Vertical_rules_id
                           join tsm in _db.tbl_status_master on tvrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && tvrv.StatusId != 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Vertical_rulesid = tvrv.Vertical_rules_id,
                               RuleValue = tvrv.RuleValue,
                               Vertical_Rules = tvr.Vertical_Rules,
                               CourseId = tram.CourseId,
                               Exam_Year = tram.Exam_Year,
                               Remarks = tvrv.Remarks,
                               StatusName = tsm.StatusName,
                               Rules_allocation_masterid = tram.Rules_allocation_master_id
                           }).ToList();

                return res;
            }
            else
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join tvrv in _db.tbl_Vertical_rule_value on tram.Rules_allocation_master_id equals tvrv.Rules_allocation_master_id
                           join tvr in _db.tbl_Vertical_rules on tvrv.Vertical_rules_id equals tvr.Vertical_rules_id
                           join tsm in _db.tbl_status_master on tvrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && tvrv.StatusId == 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Vertical_rulesid = tvrv.Vertical_rules_id,
                               RuleValue = tvrv.RuleValue,
                               Vertical_Rules = tvr.Vertical_Rules,
                               CourseId = tram.CourseId,
                               Exam_Year = tram.Exam_Year,
                               Remarks = tvrv.Remarks,
                               StatusName = tsm.StatusName,
                               Rules_allocation_masterid = tram.Rules_allocation_master_id
                           }).ToList();

                return res;
            }

        }

        #endregion

        #region .. Horizontal ..

        public List<SeatAllocation> GetSeatAllocationHorizontalDLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            if (tabNameValue == "tab1" || tabNameValue == "tab2" || tabNameValue == "tab4")
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                           join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
                           join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId != 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
                               Horizontal_rules = thr.Horizontal_rules,
                               RuleValue = thrv.RuleValue,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = thrv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();
                return res;
            }
            /*else if (tabNameValue == "tab2")
			{
					var res = (from tram in _db.Tbl_rules_allocation_master
										 join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
										 join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
										 join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
										 where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId != 2 && thrv.StatusId != 5
										 select new SeatAllocation
										 {
												 Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
												 Horizontal_rules = thr.Horizontal_rules,
												 RuleValue = thrv.RuleValue,
												 Exam_Year = tram.Exam_Year,
												 CourseId = tram.CourseId,
												 Remarks = thrv.Remarks,
												 StatusName = tsm.StatusName
										 }).ToList();
					return res;
			}*/
            else
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                           join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
                           join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId == 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
                               Horizontal_rules = thr.Horizontal_rules,
                               RuleValue = thrv.RuleValue,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = thrv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();

                return res;
            }

        }
        #endregion

        #region .. HydRegion  ..

        public List<SeatAllocation> GetseatallocationHydDLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            if (tabNameValue == "tab1" || tabNameValue == "tab2" || tabNameValue == "tab4")
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                           join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                           join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                           join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
                           join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId != 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
                               Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
                               RuleValue = thrv.RuleValue,
                               Candidates_type = thnc.Candidates_type,
                               Region_type = thnr.Region_type,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = thrv.Remarks,
                               StatusName = tsm.StatusName

                           }).ToList();

                return res;
            }
            /*else if (tabNameValue == "tab2")
			{
					var res = (from tram in _db.Tbl_rules_allocation_master
										 join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
										 join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
										 join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
										 join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
										 join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
										 where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId != 2 && thrv.StatusId != 5 && tram.IsActive == true
										 select new SeatAllocation
										 {
												 Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
												 Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
												 RuleValue = thrv.RuleValue,
												 Candidates_type = thnc.Candidates_type,
												 Region_type = thnr.Region_type,
												 Exam_Year = tram.Exam_Year,
												 CourseId = tram.CourseId,
												 Remarks = thrv.Remarks,
												 StatusName = tsm.StatusName

										 }).ToList();

					return res;
			}*/
            else
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                           join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                           join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                           join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
                           join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && thrv.StatusId == 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
                               Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
                               RuleValue = thrv.RuleValue,
                               Candidates_type = thnc.Candidates_type,
                               Region_type = thnr.Region_type,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = thrv.Remarks,
                               StatusName = tsm.StatusName

                           }).ToList();

                return res;
            }
        }
        #endregion

        #region .. GradeList  ..

        public List<SeatAllocation> GetSeatAllocationGradeDLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            if (tabNameValue == "tab1" || tabNameValue == "tab2" || tabNameValue == "tab4")
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
                           join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
                           join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
                           join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
                           join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && tgpv.StatusId != 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               RuleValue = tgpv.RuleValue,
                               Grade_percentageid = tgpv.Grade_percentage_id,
                               Syllabus_type_id = tgp.Syllabus_type_id,
                               Grade_Id = tgp.Grade_Id,
                               GradGrades = tg.GradGrades,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = tgpv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();

                return res;
            }
            /*else if (tabNameValue == "tab2")
			{
					var res = (from tram in _db.Tbl_rules_allocation_master
										 join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
										 join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
										 join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
										 join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
										 join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
										 where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && tgpv.StatusId != 2 && tgpv.StatusId != 5 && tram.IsActive == true
										 select new SeatAllocation
										 {
												 RuleValue = tgpv.RuleValue,
												 Grade_percentageid = tgpv.Grade_percentage_id,
												 Syllabus_type_id = tgp.Syllabus_type_id,
												 Grade_Id = tgp.Grade_Id,
												 GradGrades = tg.GradGrades,
												 Exam_Year = tram.Exam_Year,
												 CourseId = tram.CourseId,
												 Remarks = tgpv.Remarks,
												 StatusName = tsm.StatusName
										 }).ToList();

					return res;
			}*/
            else
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
                           join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
                           join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
                           join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
                           join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && tgpv.StatusId == 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               RuleValue = tgpv.RuleValue,
                               Grade_percentageid = tgpv.Grade_percentage_id,
                               Syllabus_type_id = tgp.Syllabus_type_id,
                               Grade_Id = tgp.Grade_Id,
                               GradGrades = tg.GradGrades,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = tgpv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();

                return res;
            }
        }
        #endregion

        #region .. Other Rules  ..

        public List<SeatAllocation> GetSeatAllocationOtherRulesDLL(int Exam_Year, int CourseTypes, string tabNameValue)
        {
            if (tabNameValue == "tab1" || tabNameValue == "tab2" || tabNameValue == "tab4")
            {
                var res = (from tram in _db.Tbl_rules_allocation_master
                           join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
                           join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
                           join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && torv.StatusId != 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Other_rulesid = tor.Other_rules_id,
                               RuleValue = torv.RuleValue,
                               OtherRules = tor.OtherRules,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = torv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();

                return res;
            }
            /*else if (tabNameValue == "tab2")
			{
					var res = (from tram in _db.Tbl_rules_allocation_master
										 join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
										 join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
										 join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
										 where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && torv.StatusId != 2 && torv.StatusId != 5
										 select new SeatAllocation
										 {
												 Other_rulesid = tor.Other_rules_id,
												 RuleValue = torv.RuleValue,
												 OtherRules = tor.OtherRules,
												 Exam_Year = tram.Exam_Year,
												 CourseId = tram.CourseId,
												 Remarks = torv.Remarks,
												 StatusName = tsm.StatusName
										 }).ToList();

					return res;
			}*/
            else
            {

                var res = (from tram in _db.Tbl_rules_allocation_master
                           join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
                           join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
                           join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
                           where tram.Exam_Year == Exam_Year && tram.CourseId == CourseTypes && torv.StatusId == 2 && tram.IsActive == true
                           select new SeatAllocation
                           {
                               Other_rulesid = tor.Other_rules_id,
                               RuleValue = torv.RuleValue,
                               OtherRules = tor.OtherRules,
                               Exam_Year = tram.Exam_Year,
                               CourseId = tram.CourseId,
                               Remarks = torv.Remarks,
                               StatusName = tsm.StatusName
                           }).ToList();

                return res;
            }
        }

        #endregion

        #region .. GetseatallocationListById ..

        public List<SeatAllocation> GetseatallocationListByIdDLL(string seatAllocationPopUp)
        {
            if (seatAllocationPopUp == "Vertical")
            {
                var res = (from a in _db.tbl_Vertical_rule_value
                               //where a.Rules_allocation_master_id == 20
                           select new SeatAllocation
                           {
                               Rules_allocation_masterid = a.Rules_allocation_master_id,
                               StatusId = a.StatusId,
                               Remarks = a.Remarks
                           }).ToList();
                return res;
            }
            else if (seatAllocationPopUp == "Horizontal")
            {
                var res = (from a in _db.tbl_horizontal_rules_values
                               //where a.Rules_allocation_master_id == 20
                           select new SeatAllocation
                           {
                               Rules_allocation_masterid = a.Rules_allocation_master_id,
                               StatusId = a.StatusId,
                               Remarks = a.Remarks
                           }).ToList();
                return res;
            }
            else if (seatAllocationPopUp == "Region")
            {
                var res = (from a in _db.tbl_HYD_kar_rules_value
                               //where a.Rules_allocation_master_id == 20
                           select new SeatAllocation
                           {
                               Rules_allocation_masterid = a.Rules_allocation_master_id,
                               StatusId = a.StatusId,
                               Remarks = a.Remarks
                           }).ToList();
                return res;
            }
            else if (seatAllocationPopUp == "Grades")
            {
                var res = (from a in _db.tbl_Grade_percentage_Value
                               //where a.Rules_allocation_master_id == 20
                           select new SeatAllocation
                           {
                               Rules_allocation_masterid = a.Rules_allocation_master_id,
                               StatusId = a.StatusId,
                               Remarks = a.Remarks
                           }).ToList();
                return res;
            }
            else
            {
                var res = (from a in _db.tbl_other_rules_value
                               //where a.Rules_allocation_master_id == 20
                           select new SeatAllocation
                           {
                               Rules_allocation_masterid = a.Rules_allocation_master_id,
                               StatusId = a.StatusId,
                               Remarks = a.Remarks
                           }).ToList();
                return res;
            }
        }

        #endregion

        #region .. GetExamYear ..

        public List<SeatAllocation> GetExamYearDLL(int ExamYearId)
        {
            var res = (from a in _db.tbl_Year
                           //where a.YearID == true && a.YearID == ExamYearId
                       select new SeatAllocation
                       {
                           YearID = a.YearID,
                           Year = a.Year
                       }).ToList();
            return res;
        }
        #endregion

        public string GetSeatupdateVerticalDLL(SeatAllocation modal)
        {
            try
            {
                int updateApprovedVerticalFlag = 0;
                int updateApprovedHorizontalFlag = 0;
                int updateApprovedRegionFlag = 0;
                int updateApprovedGradesFlag = 0;
                int updateApprovedOtherRulesFlag = 0;
                //if (modal.SeatAllocationPartId == "Vertical")
                {
                    List<tbl_Vertical_rule_value> UpdateVertical = (from c in _db.tbl_Vertical_rule_value
                                                                    where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                    select c).ToList();
                    if (UpdateVertical != null)
                    {
                        for (int i = 0; i < UpdateVertical.Count; i++)
                        {
                            UpdateVertical[i].StatusId = modal.StatusId;
                            UpdateVertical[i].Remarks = modal.Remarks;
                            if (UpdateVertical[i].StatusId == 2)
                            {
                                updateApprovedVerticalFlag = 1;
                            }
                            _db.SaveChanges();
                        }
                    }
                }
                //else if (modal.SeatAllocationPartId == "Horizontal")
                {
                    List<tbl_horizontal_rules_values> UpdateHorizontal = (from c in _db.tbl_horizontal_rules_values
                                                                          where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                          select c).ToList();
                    if (UpdateHorizontal != null)
                    {
                        for (int i = 0; i < UpdateHorizontal.Count; i++)
                        {
                            UpdateHorizontal[i].StatusId = modal.StatusId;
                            UpdateHorizontal[i].Remarks = modal.Remarks;
                            if (UpdateHorizontal[i].StatusId == 2)
                            {
                                updateApprovedHorizontalFlag = 1;
                            }
                            _db.SaveChanges();
                        }
                    }
                }
                //else if (modal.SeatAllocationPartId == "Region")
                {
                    List<tbl_HYD_kar_rules_value> UpdateRegion = (from c in _db.tbl_HYD_kar_rules_value
                                                                  where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                  select c).ToList();
                    if (UpdateRegion != null)
                    {
                        for (int i = 0; i < UpdateRegion.Count; i++)
                        {
                            UpdateRegion[i].StatusId = modal.StatusId;
                            UpdateRegion[i].Remarks = modal.Remarks;
                            if (UpdateRegion[i].StatusId == 2)
                            {
                                updateApprovedRegionFlag = 1;
                            }
                            _db.SaveChanges();
                        }
                    }
                }
                //else if (modal.SeatAllocationPartId == "Grades")
                {
                    List<tbl_Grade_percentage_Value> UpdateGrade = (from c in _db.tbl_Grade_percentage_Value
                                                                    where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                    select c).ToList();
                    if (UpdateGrade != null)
                    {
                        for (int i = 0; i < UpdateGrade.Count; i++)
                        {
                            UpdateGrade[i].StatusId = modal.StatusId;
                            UpdateGrade[i].Remarks = modal.Remarks;
                            if (UpdateGrade[i].StatusId == 2)
                            {
                                updateApprovedGradesFlag = 1;
                            }
                            _db.SaveChanges();
                        }
                    }
                }
                //else if (modal.SeatAllocationPartId == "OtherRules")
                {
                    List<tbl_other_rules_value> UpdateOtherRules = (from c in _db.tbl_other_rules_value
                                                                    where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                    select c).ToList();
                    if (UpdateOtherRules != null)
                    {
                        for (int i = 0; i < UpdateOtherRules.Count; i++)
                        {
                            UpdateOtherRules[i].StatusId = modal.StatusId;
                            UpdateOtherRules[i].Remarks = modal.Remarks;
                            if (UpdateOtherRules[i].StatusId == 2)
                            {
                                updateApprovedOtherRulesFlag = 1;
                            }
                            _db.SaveChanges();
                        }
                    }
                }

                #region .. Update in master table Tbl_rules_allocation_master Status and Remarks ..


                List<Tbl_rules_allocation_master> UpdateAllocationMaster = (from c in _db.Tbl_rules_allocation_master
                                                                            where c.Rules_allocation_master_id == modal.Rules_allocation_masterid
                                                                            select c).ToList();

                if (UpdateAllocationMaster != null)
                {
                    for (int i = 0; i < UpdateAllocationMaster.Count; i++)
                    {
                        UpdateAllocationMaster[i].Status_Id = modal.StatusId;
                        UpdateAllocationMaster[i].FlowId = modal.ForwardTo;
                        UpdateAllocationMaster[i].ModifiedBy = modal.ModifiedBy;

                        if (updateApprovedVerticalFlag == 1 && updateApprovedHorizontalFlag == 1 && (updateApprovedRegionFlag == 1 || updateApprovedGradesFlag == 1) && updateApprovedOtherRulesFlag == 1)
                        {
                            UpdateAllocationMaster[i].Remarks = "Approved";
                        }
                        else
                        {
                            UpdateAllocationMaster[i].Remarks = modal.Remarks;
                        }
                        UpdateAllocationMaster[i].IsActive = true;

                        _db.SaveChanges();
                    }
                }


                Tbl_rules_allocation_master_history objTbl_rules_allocation_master_history = new Tbl_rules_allocation_master_history();
                objTbl_rules_allocation_master_history.Rules_allocation_master_id = modal.Rules_allocation_masterid;
                objTbl_rules_allocation_master_history.Exam_Year = modal.Exam_Year;
                objTbl_rules_allocation_master_history.Status_Id = modal.StatusId;
                objTbl_rules_allocation_master_history.Trans_Date = DateTime.Now;
                objTbl_rules_allocation_master_history.CreatedOn = DateTime.Now;
                objTbl_rules_allocation_master_history.ModifiedOn = DateTime.Now;
                objTbl_rules_allocation_master_history.CourseId = modal.CourseId;
                objTbl_rules_allocation_master_history.FlowId = modal.ForwardTo;
                objTbl_rules_allocation_master_history.CreatedBy = 1;
                objTbl_rules_allocation_master_history.ModifiedBy = modal.ModifiedBy;
                objTbl_rules_allocation_master_history.Remarks = modal.Remarks;
                _db.Tbl_rules_allocation_master_history.Add(objTbl_rules_allocation_master_history);
                _db.SaveChanges();

                #region Updating Tbl_rules_allocation_master

                Tbl_rules_allocation_master objtbl_rules_allocation_master = new Tbl_rules_allocation_master();
                var update_query = _db.Tbl_rules_allocation_master.Where(s => s.Rules_allocation_master_id == modal.Rules_allocation_masterid && s.IsActive == true).FirstOrDefault();

                update_query.Status_Id = modal.StatusId;
                update_query.ModifiedOn = DateTime.Now;
                update_query.FlowId = modal.ForwardTo;
                update_query.ModifiedBy = modal.ModifiedBy;
                update_query.Remarks = modal.Remarks;
                _db.SaveChanges();

                #endregion

                #endregion

                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }

        public List<SeatAllocation> GetApprovedAllocationSeatofRuleDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Status_Id == 2 && a.IsActive == true)
                       join ty in _db.tbl_Year on tram.Exam_Year equals ty.YearID
                       join tctm in _db.tbl_course_type_mast on tram.CourseId equals tctm.course_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       join tur in _db.tbl_user_roles on tram.FlowId equals tur.user_id
                       join trm in _db.tbl_role_master on tram.ModifiedBy equals trm.role_id
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           ExamYear = ty.Year,
                           CourseName = tctm.course_type_name,
                           StatusName = (tsm.StatusId == 2 ? "Approved" : tsm.StatusName),
                           Remarks = tram.Remarks,
                           userRole = tur.user_role,
                           role_description = trm.role_description,
                           YearID = ty.YearID,
                           CourseId = tctm.course_id,
                       }).ToList();

            return res;
        }


        public List<SeatAllocation> GetAllocationSeatofRuletoUpdateDLL()
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Status_Id != 2 && a.IsActive == true)
                       join ty in _db.tbl_Year on tram.Exam_Year equals ty.YearID
                       join tctm in _db.tbl_course_type_mast on tram.CourseId equals tctm.course_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       join tur in _db.tbl_role_master on tram.FlowId equals tur.role_id

                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           ExamYear = ty.Year,
                           CourseName = tctm.course_type_name,
                           StatusId = tsm.StatusId,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           userRole = tur.role_DescShortForm
                       }).ToList();

            return res;
        }

        public List<SeatAllocation> GetSeatAllocationDLL(SeatAllocation modal)
        {
            //var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id > 57)
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join ty in _db.tbl_Year on tram.Exam_Year equals ty.YearID
                       join tctm in _db.tbl_course_type_mast on tram.CourseId equals tctm.course_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       //join tur in _db.tbl_user_roles on tram.FlowId equals tur.user_id
                       //join tur in _db.tbl_user_login on tram.FlowId equals tur.id
                       join tur in _db.tbl_user_master on tram.FlowId equals tur.um_id
                       where tram.IsActive == true
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           ExamYear = ty.Year,
                           CourseName = tctm.course_type_name,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           //FormwardFrom = tur.user_name
                           FormwardFrom = tur.um_name
                       }).ToList();

            return res;
        }

        public List<SeatAllocation> GetSeatAllocationByFlowIdDLL(SeatAllocation modal)
        {
            //var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.FlowId == modal.FlowId && a.IsActive == true)
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.IsActive == true && a.Status_Id != 2)
                       join ty in _db.tbl_Year on tram.Exam_Year equals ty.YearID
                       join tctm in _db.tbl_course_type_mast on tram.CourseId equals tctm.course_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       //join tur in _db.tbl_user_login on tram.FlowId equals tur.id
                       join tur in _db.tbl_role_master on tram.FlowId equals tur.role_id
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           ExamYear = ty.Year,
                           CourseName = tctm.course_type_name,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           //FormwardFrom = tur.user_name
                           FormwardFrom = tur.role_description
                       }).ToList();

            return res;
        }

        #region .. Approved Master ..

        public List<SeatAllocation> GetSeatAllocationApprovedDLL(SeatAllocation modal)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join ty in _db.tbl_Year on tram.Exam_Year equals ty.YearID
                       join tctm in _db.tbl_course_type_mast on tram.CourseId equals tctm.course_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       //join tur in _db.tbl_user_roles on tram.FlowId equals tur.user_id
                       //join tur in _db.tbl_user_login on tram.FlowId equals tur.id
                       join tur in _db.tbl_user_master on tram.FlowId equals tur.um_id
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           ExamYear = ty.Year,
                           CourseName = tctm.course_type_name,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           //FormwardFrom = tur.user_name
                           FormwardFrom = tur.um_name
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Approved Vertical ..

        public List<SeatAllocation> GetSeatAllocationApprovedVerticalDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join tvrv in _db.tbl_Vertical_rule_value on tram.Rules_allocation_master_id equals tvrv.Rules_allocation_master_id
                       join tvr in _db.tbl_Vertical_rules on tvrv.Vertical_rules_id equals tvr.Vertical_rules_id
                       join tsm in _db.tbl_status_master on tvrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           Vertical_rulesid = tvrv.Vertical_rules_id,
                           RuleValue = tvrv.RuleValue,
                           Vertical_Rules = tvr.Vertical_Rules,
                           CourseId = tram.CourseId,
                           Exam_Year = tram.Exam_Year,
                           Remarks = tvrv.Remarks,
                           StatusName = tsm.StatusName,
                           Rules_allocation_masterid = tram.Rules_allocation_master_id
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Approved Horizontal ..

        public List<SeatAllocation> GetSeatAllocationApprovedHorizontalDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
                           Horizontal_rules = thr.Horizontal_rules,
                           RuleValue = thrv.RuleValue,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();
            return res;
        }
        #endregion

        #region .. Approved HydRegion  ..

        public List<SeatAllocation> GetSeatAllocationApprovedHydDLL(int Rules_allocation_masterid)
        {

            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                       join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                       join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
                           Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
                           RuleValue = thrv.RuleValue,
                           Candidates_type = thnc.Candidates_type,
                           Region_type = thnr.Region_type,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName

                       }).ToList();

            return res;
        }
        #endregion

        #region .. Approved GradeList  ..

        public List<SeatAllocation> GetSeatAllocationApprovedGradeDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
                       join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
                       join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
                       join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
                       join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           RuleValue = tgpv.RuleValue,
                           Grade_percentageid = tgpv.Grade_percentage_id,
                           Syllabus_type_id = tgp.Syllabus_type_id,
                           Grade_Id = tgp.Grade_Id,
                           GradGrades = tg.GradGrades,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = tgpv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }
        #endregion

        #region .. Approved Other Rules  ..

        public List<SeatAllocation> GetSeatAllocationApprovedOtherRulesDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master.Where(a => a.Rules_allocation_master_id == 2 && a.IsActive == true)
                       join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
                       join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
                       join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           Other_rulesid = tor.Other_rules_id,
                           RuleValue = torv.RuleValue,
                           OtherRules = tor.OtherRules,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = torv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Vertical ..

        public List<SeatAllocation> GetSeatAllocationVerticalDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join tvrv in _db.tbl_Vertical_rule_value on tram.Rules_allocation_master_id equals tvrv.Rules_allocation_master_id
                       join tvr in _db.tbl_Vertical_rules on tvrv.Vertical_rules_id equals tvr.Vertical_rules_id
                       join tsm in _db.tbl_status_master on tvrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid
                       select new SeatAllocation
                       {
                           FlowId = tram.FlowId,
                           Vertical_rulesid = tvrv.Vertical_rules_id,
                           RuleValue = tvrv.RuleValue,
                           Vertical_Rules = tvr.Vertical_Rules,
                           CourseId = tram.CourseId,
                           Exam_Year = tram.Exam_Year,
                           Remarks = tvrv.Remarks,
                           StatusName = tsm.StatusName,
                           Rules_allocation_masterid = tram.Rules_allocation_master_id
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Horizontal ..

        public List<SeatAllocation> GetSeatAllocationHorizontalDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join thrv in _db.tbl_horizontal_rules_values on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_horizontal_rules on thrv.Horizontal_rules_id equals thr.Horizontal_rules_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid && tram.IsActive == true
                       select new SeatAllocation
                       {
                           Horizontal_rules_valueid = thrv.Horizontal_rules_value_id,
                           Horizontal_rules = thr.Horizontal_rules,
                           RuleValue = thrv.RuleValue,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();
            return res;
        }
        #endregion

        #region .. HydRegion  ..

        public List<SeatAllocation> GetseatallocationHydDLL(int Rules_allocation_masterid)
        {

            var res = (from tram in _db.Tbl_rules_allocation_master
                       join thrv in _db.tbl_HYD_kar_rules_value on tram.Rules_allocation_master_id equals thrv.Rules_allocation_master_id
                       join thr in _db.Tbl_Hyd_kar_rules on thrv.Hyd_NonHyd_rules_id equals thr.Hyd_NonHyd_rules_id
                       join thnc in _db.tbl_HYD_NonHYD_candidates on thr.Hyd_NonHyd_candidates_id equals thnc.Hyd_NonHyd_candidates_id
                       join thnr in _db.tbl_HYD_NonHYD_regions on thr.Hyd_NonHyd_region_id equals thnr.Hyd_NonHyd_region_id
                       join tsm in _db.tbl_status_master on thrv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid && tram.IsActive == true
                       select new SeatAllocation
                       {
                           Hyd_NonHyd_rules_valid = thrv.Hyd_NonHyd_rules_val_id,
                           Hyd_NonHyd_rulesid = thrv.Hyd_NonHyd_rules_id,
                           RuleValue = thrv.RuleValue,
                           Candidates_type = thnc.Candidates_type,
                           Region_type = thnr.Region_type,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = thrv.Remarks,
                           StatusName = tsm.StatusName

                       }).ToList();

            return res;
        }
        #endregion

        #region .. GradeList  ..

        public List<SeatAllocation> GetSeatAllocationGradeDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join tgpv in _db.tbl_Grade_percentage_Value on tram.Rules_allocation_master_id equals tgpv.Rules_allocation_master_id
                       join tgp in _db.tbl_Grade_percentage on tgpv.Grade_percentage_id equals tgp.Grade_percentage_id
                       join tg in _db.tbl_Grades on tgp.Grade_Id equals tg.GradeId
                       join tst in _db.tbl_syllabus_type on tgp.Syllabus_type_id equals tst.Syllabus_type_id
                       join tsm in _db.tbl_status_master on tgpv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid && tram.IsActive == true
                       select new SeatAllocation
                       {
                           RuleValue = tgpv.RuleValue,
                           Grade_percentageid = tgpv.Grade_percentage_id,
                           Syllabus_type_id = tgp.Syllabus_type_id,
                           Grade_Id = tgp.Grade_Id,
                           GradGrades = tg.GradGrades,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = tgpv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }
        #endregion

        #region .. Other Rules  ..

        public List<SeatAllocation> GetSeatAllocationOtherRulesDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       join torv in _db.tbl_other_rules_value on tram.Rules_allocation_master_id equals torv.Rules_allocation_master_id
                       join tor in _db.Tbl_other_rules on torv.Other_rules_id equals tor.Other_rules_id
                       join tsm in _db.tbl_status_master on torv.StatusId equals tsm.StatusId
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid && tram.IsActive == true
                       select new SeatAllocation
                       {
                           Other_rulesid = tor.Other_rules_id,
                           RuleValue = torv.RuleValue,
                           OtherRules = tor.OtherRules,
                           Exam_Year = tram.Exam_Year,
                           CourseId = tram.CourseId,
                           Remarks = torv.Remarks,
                           StatusName = tsm.StatusName
                       }).ToList();

            return res;
        }

        #endregion

        #region .. Other Rules  ..

        public List<SeatAllocation> GetSeatAllocationRemarksStatusDLL(int Rules_allocation_masterid)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master
                       where tram.Rules_allocation_master_id == Rules_allocation_masterid && tram.IsActive == true
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           Status_Id = tram.Status_Id,
                           Remarks = tram.Remarks,
                           FlowId = tram.FlowId
                       }).ToList();

            return res;
        }


        #endregion

        #region 

        public List<SeatAllocation> GetCommentsListDLL(int SeatAllocationId)
        {
            var res = (from tram in _db.Tbl_rules_allocation_master_history
                       join tur in _db.tbl_role_master on tram.ModifiedBy equals tur.role_id
                       join turs in _db.tbl_role_master on tram.FlowId equals turs.role_id
                       join tsm in _db.tbl_status_master on tram.Status_Id equals tsm.StatusId
                       where tram.Rules_allocation_master_id == SeatAllocationId
                       orderby tram.CreatedOn descending
                       select new SeatAllocation
                       {
                           Rules_allocation_masterid = tram.Rules_allocation_master_id,
                           userRole = tur.role_description,
                           ForwardedTo = turs.role_description,
                           StatusName = tsm.StatusName,
                           Remarks = tram.Remarks,
                           CommentsCreatedOn = tram.CreatedOn.ToString(),
                           StatusId = tsm.StatusId
                       }).ToList();

            return res;
        }

        public List<ApplicantApplicationForm> GetCommentDetailsApplicantDLL(int SeatAllocationId)
        {
            var res = (from tram in _db.tbl_ApplicantTrans
                       join tat in _db.tbl_Applicant_Detail on tram.ApplicantId equals tat.ApplicationId
                       join tur in _db.tbl_user_master on tram.CreatedBy equals tur.um_id
                       join turs in _db.tbl_user_master on tram.FlowId equals turs.um_id
                       join tafds in _db.tbl_ApplicationFormDescStatus on tram.ApplDescStatus equals tafds.ApplicationFormDescStatus_id
                       where tram.ApplicantId == SeatAllocationId && tram.FinalSubmitInd == 1
                       orderby tram.CreatedOn descending
                       select new ApplicantApplicationForm
                       {
                           ApplicantId = tram.ApplicantId,
                           CommentsCreatedOn = tram.CreatedOn.ToString(),
                           userRole = tur.um_name,
                           ForwardedTo = turs.um_name,
                           ApplDescription = tafds.ApplDescription,
                           Remark = (tram.Remark == "" || tram.Remark == null ? "NA" : tram.Remark)
                       }).ToList();

            return res;
        }

        #endregion

        #endregion

        #region dhanraj joined in rajgopal task
        public List<VerificationOfficer> GetApplicantsStatus(int loginId, int roleId)
        {
            try
            {
                List<VerificationOfficer> res = new List<VerificationOfficer>();

                if (roleId == 12)
                {
                    res = (from tad in _db.tbl_Applicant_Detail
                           join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                           join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                           join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                           join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                           join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                           join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                           where tvom.UserMasterId == loginId && tad.IsActive == true
                           select new VerificationOfficer
                           {
                               ApplicationId = tad.ApplicationId,
                               CredatedBy = tad.CredatedBy,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               StatusName =
                                              (
                                                      tad.ApplStatus == 5 ? "Document verification pending" :
                                                      tad.ApplStatus == 10 ? "Documents Verified" :
                                                      tad.ApplStatus == 10 && tat.ReVerficationStatus == true ? "Documents Re-Verified" :
                                                      tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                                      "Re-Submitted"
                                              ),
                               Remarks = tad.ApplRemarks,
                               CourseType = bb.course_type_name,
                               Year = tad.ApplyYear
                           }
                                         ).Distinct().ToList();
                }
                else
                {
                    res = (from tad in _db.tbl_Applicant_Detail
                           join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                           join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                           join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                           join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                           join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                           join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                           join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id
                           where tat.CreatedBy == loginId && tad.IsActive == true
                           select new VerificationOfficer
                           {
                               ApplicationId = tad.ApplicationId,
                               CredatedBy = tad.CredatedBy,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               StatusName = afds.ApplDescription,
                               Remarks = tad.ApplRemarks,
                               CourseType = bb.course_type_name,
                               Year = tad.ApplyYear,
                               OfficerName = tvom.Name,
                               SubmitDate = tad.CreatedOn,
                               AssignedVO = tad.AssignedVO,
                               ReVerficationStatus = tad.ReVerficationStatus,
                               DocumentFeeReceiptDetails = tad.DocumentFeeReceiptDetails,
                               ApplStatus = tad.ApplStatus
                           }
                                 ).Distinct().ToList();

                    if (res.Count == 0)
                    {
                        res = (from tad in _db.tbl_Applicant_Detail
                               join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                               join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                               join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id
                               where tad.CredatedBy == loginId && tad.IsActive == true
                               select new VerificationOfficer
                               {
                                   ApplicationId = tad.ApplicationId,
                                   CredatedBy = tad.CredatedBy,
                                   ApplicantNumber = tad.ApplicantNumber,
                                   ApplicantName = tad.ApplicantName,
                                   Remarks = tad.ApplRemarks,
                                   PaymentOptionval = tad.PaymentOptionval,
                                   SubmitDate = tad.CreatedOn,
                                   Year = tad.ApplyYear,
                                   ApplStatus = tad.ApplStatus,
                                   StatusName = afds.ApplDescription,
                                   ReVerficationStatus = tad.ReVerficationStatus,
                                   AssignedVO = tad.AssignedVO,
                                   DocumentFeeReceiptDetails = tad.DocumentFeeReceiptDetails
                               }
                                     ).ToList();
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VerificationOfficer> GetApplicantsStatusFilter(int loginId, int roleId, int year, int courseType, int applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId)
        {
            try
            {
                if (year == 0)
                {
                    //changes have been done for use case 22 iti principal  login Document verification status tab role id in where conditiom
                    List<VerificationOfficer> res = new List<VerificationOfficer>();
                    bool ExistingRecordForUpdate = false;

                    if (roleId == 12 || roleId == (int)CmnClass.Role.ITIAdmin)
                    {
                        res = (from tad in _db.tbl_Applicant_Detail
                               join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                               join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                               join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                               join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                               join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                               join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                               //join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id
                               join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                               from afds in gj.DefaultIfEmpty()
                               join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               join DivM in _db.tbl_division_master on aa.division_id equals DivM.division_id into divisionDetails
                               from DivMDetails in divisionDetails.DefaultIfEmpty()
                               join DistM in _db.tbl_district_master on aa.district_id equals DistM.district_lgd_code into districtDetails
                               from DistDetails in districtDetails.DefaultIfEmpty()
                               join TM in _db.tbl_taluk_master on aa.taluk_id equals TM.taluk_lgd_code into talukDetails
                               from TMDetails in talukDetails.DefaultIfEmpty()

                               where (roleId != (int)CmnClass.Role.ITIAdmin ? tvom.UserMasterId != 0 ? tvom.UserMasterId == loginId : true : true) && tad.IsActive == true
                               && (division_id != 0 ? aa.division_id == division_id : true) && (district_lgd_code != 0 ? aa.district_id == district_lgd_code : true)
                        && (taluk_lgd_code != 0 ? aa.taluk_id == taluk_lgd_code : true) && (InstituteId != 0 ? aa.iti_college_id == InstituteId : true)
                               select new VerificationOfficer
                               {
                                   ApplicationId = tad.ApplicationId,
                                   CredatedBy = tad.CredatedBy,
                                   ApplicantNumber = tad.ApplicantNumber,
                                   ApplicantName = tad.ApplicantName,
                                   StatusName = afds.ApplDescription,
                                   ApplDescStatus = tad.ApplDescStatus,
                                   Remarks = tad.ApplRemarks,
                                   CourseType = bb.course_type_name,
                                   Year = tad.ApplyYear,
                                   //change to display year name 23 UC
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   MisCode = aa.MISCode,
                                   InstituteName = aa.iti_college_name,
                                   PaymentOptionval = tad.PaymentOptionval,
                                   OfficerName = tvom.Name,
                                   MobileNumber = tad.PhoneNumber,
                                   DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber,

                                   ApplicationMode = AMDetails.ApplicationMode,
                                   Divisionname = DivMDetails.division_name,
                                   Districtname = DistDetails.district_ename,
                                   Talukname = TMDetails.taluk_ename
                               }
                                             ).Distinct().ToList();
                        return res;
                    }
                    else if (roleId == (int)CmnClass.Role.DeputyDirector || roleId == (int)CmnClass.Role.JDDiv)
                    {
                        int? divid = _db.tbl_user_master.Where(x => x.um_id == loginId).Select(y => y.um_div_id).FirstOrDefault();
                        res = (from tad in _db.tbl_Applicant_Detail
                               join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                               join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                               join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                               join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                               join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                               join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                               join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                              // join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id
                               join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                               from afds in gj.DefaultIfEmpty()
                               join DivM in _db.tbl_division_master on aa.division_id equals DivM.division_id into divisionDetails
                               from DivMDetails in divisionDetails.DefaultIfEmpty()
                               join DistM in _db.tbl_district_master on aa.district_id equals DistM.district_lgd_code into districtDetails
                               from DistDetails in districtDetails.DefaultIfEmpty()
                               join TM in _db.tbl_taluk_master on aa.taluk_id equals TM.taluk_lgd_code into talukDetails
                               from TMDetails in talukDetails.DefaultIfEmpty()
                               where (roleId != (int)CmnClass.Role.ITIAdmin ? tvom.UserMasterId != 0 ? tvom.UserMasterId == loginId : true : true) && tad.IsActive == true
                               && (division_id != 0 ? aa.division_id == division_id : true) && (district_lgd_code != 0 ? aa.district_id == district_lgd_code : true)
                               && (taluk_lgd_code != 0 ? aa.taluk_id == taluk_lgd_code : true) && (InstituteId != 0 ? aa.iti_college_id == InstituteId : true)
                               select new VerificationOfficer
                               {
                                   ApplicationId = tad.ApplicationId,
                                   CredatedBy = tad.CredatedBy,
                                   ApplicantNumber = tad.ApplicantNumber,
                                   ApplicantName = tad.ApplicantName,
                                   StatusName = afds.ApplDescription,
                                   ApplDescStatus = tad.ApplDescStatus,
                                   Remarks = tad.ApplRemarks,
                                   CourseType = bb.course_type_name,
                                   Year = tad.ApplyYear,
                                   //change to display year name 23 UC
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   MisCode = aa.MISCode,
                                   InstituteName = aa.iti_college_name,
                                   PaymentOptionval = tad.PaymentOptionval,
                                   OfficerName = tvom.Name,
                                   MobileNumber = tad.PhoneNumber,
                                   DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber,
                                   ApplicationMode = AMDetails.ApplicationMode,
                                   Divisionname = DivMDetails.division_name,
                                   Districtname = DistDetails.district_ename,
                                   Talukname = TMDetails.taluk_ename
                               }
                                             ).Distinct().ToList();
                        return res;
                    }
                    else
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tat.ReVerficationStatus == true && tad.IsActive == true
                                                   select tat.ReVerficationStatus).Take(1).FirstOrDefault();

                        if (ExistingRecordForUpdate)
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                                   join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                                   join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                                   from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       Remarks = tad.ApplRemarks,
                                       SubmitDate = tad.CreatedOn,
                                       OfficerName = tvom.Name,
                                       CourseType = bb.course_type_name,
                                       Year = tad.ApplyYear,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 ? "Document verification pending" :
                                       //               tad.ApplStatus == 10 ? "Documents Re-Verified" :
                                       //               tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                       //                "Unknown"
                                       //       ),
                                       StatusName = afds.ApplDescription,
                                     

                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       MobileNumber = tad.PhoneNumber,
                                       DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber,

                                       ApplicationMode = AMDetails.ApplicationMode

                                   }
                                         ).ToList();
                        }
                        else
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                                   join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                                   join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                                   from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       OfficerName = tvom.Name,
                                       Remarks = tad.ApplRemarks,
                                       SubmitDate = tad.CreatedOn,
                                       CourseType = bb.course_type_name,
                                       Year = tad.ApplyYear,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 ? "Document verification pending" :
                                       //               tad.ApplStatus == 10 ? "Documents Verified" :
                                       //               tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                       //                "Unknown"
                                       //       ),
                                       StatusName = afds.ApplDescription,

                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       MobileNumber = tad.PhoneNumber,
                                       DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber,
                                       ApplicationMode = AMDetails.ApplicationMode
                                   }
                                         ).ToList();
                        }

                        if (res.Count == 0)
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id


                                   join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                                   from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                       //join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       Remarks = tad.ApplRemarks,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       SubmitDate = tad.CreatedOn,
                                       Year = tad.ApplyYear,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == true ? "Application Submitted" :
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == false && (tad.DocumentFeeReceiptDetails != null && tad.DocumentFeeReceiptDetails != "") ? "Document verification Pending" :
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == false && (tad.DocumentFeeReceiptDetails == null || tad.DocumentFeeReceiptDetails == "")
                                       //               ? "Document verification fee Pending" : "Submitted"
                                       //       ),
                                       StatusName = afds.ApplDescription,
                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       MobileNumber = tad.PhoneNumber,

                                       ApplicationMode = AMDetails.ApplicationMode

                                   }
                                         ).ToList();
                        }

                    }
                    return res;
                }
                else
                {
                    List<VerificationOfficer> res = new List<VerificationOfficer>();
                    bool ExistingRecordForUpdate = false;

                    if (roleId == 12 || roleId == (int)CmnClass.Role.ITIAdmin)
                    {
                        res = (from tad in _db.tbl_Applicant_Detail
                               join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                               join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                               join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                               join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                               join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                               join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id


                               join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                               from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                               join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                               from afds in gj.DefaultIfEmpty()
                               where (roleId != (int)CmnClass.Role.ITIAdmin ? tvom.UserMasterId != 0 ? tvom.UserMasterId == loginId : true : true) && tad.IsActive == true && tad.ApplyYear == year && aa.CourseCode == courseType && tad.ApplicationMode == applicanType
                               select new VerificationOfficer
                               {
                                   ApplicationId = tad.ApplicationId,
                                   CredatedBy = tad.CredatedBy,
                                   ApplicantNumber = tad.ApplicantNumber,
                                   ApplicantName = tad.ApplicantName,
                                   //StatusName =
                                   //               (
                                   //                       tad.ApplStatus == 5 ? "Document verification pending" :
                                   //                       tad.ApplStatus == 10 ? "Documents Verified" :
                                   //                       tad.ApplStatus == 10 && tat.ReVerficationStatus == true ? "Documents Re-Verified" :
                                   //                       tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                   //                       "Re-Submitted"
                                   //               ),
                                   ApplDescStatus = tad.ApplDescStatus,
                                   StatusName = afds.ApplDescription,
                                   Remarks = tad.ApplRemarks,
                                   CourseType = bb.course_type_name,
                                   Year = tad.ApplyYear,
                                   //change to display year name 23 UC
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   //changes for UC 23
                                   MisCode = aa.MISCode,
                                   InstituteName = aa.iti_college_name,
                                   PaymentOptionval = tad.PaymentOptionval,
                                   OfficerName = tvom.Name,
                                   DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber,

                                   ApplicationMode = AMDetails.ApplicationMode

                               }
                                             ).Distinct().ToList();
                        return res;
                    }
                    else if (roleId == (int)CmnClass.Role.DeputyDirector || roleId == (int)CmnClass.Role.JDDiv)
                    {
                        int? divid = _db.tbl_user_master.Where(x => x.um_id == loginId).Select(y => y.um_div_id).FirstOrDefault();
                        res = (from tad in _db.tbl_Applicant_Detail
                               join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                               join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                               join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                               join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                               join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                               join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                               join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                               from afds in gj.DefaultIfEmpty()
                               where (roleId != (int)CmnClass.Role.ITIAdmin ? tvom.UserMasterId != 0 ? tvom.UserMasterId == loginId : true : true) && tad.IsActive == true && tad.ApplyYear == year && aa.CourseCode == courseType && tad.ApplicantType == applicanType
                               && (division_id != 0 ? aa.division_id == division_id : true) && (district_lgd_code != 0 ? aa.district_id == district_lgd_code : true)
                        && (taluk_lgd_code != 0 ? aa.taluk_id == taluk_lgd_code : true) && (InstituteId != 0 ? aa.iti_college_id == InstituteId : true)

                               select new VerificationOfficer
                               {
                                   ApplicationId = tad.ApplicationId,
                                   CredatedBy = tad.CredatedBy,
                                   ApplicantNumber = tad.ApplicantNumber,
                                   ApplicantName = tad.ApplicantName,
                                   //StatusName =
                                   //               (
                                   //                       tad.ApplStatus == 5 ? "Document verification pending" :
                                   //                       tad.ApplStatus == 10 ? "Documents Verified" :
                                   //                       tad.ApplStatus == 10 && tat.ReVerficationStatus == true ? "Documents Re-Verified" :
                                   //                       tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                   //                       "Re-Submitted"
                                   //               ),
                                   ApplDescStatus = tad.ApplDescStatus,
                                   StatusName = afds.ApplDescription,
                                   Remarks = tad.ApplRemarks,
                                   CourseType = bb.course_type_name,
                                   Year = tad.ApplyYear,
                                   //change to display year name 23 UC
                                   Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                   //changes for UC 23
                                   MisCode = aa.MISCode,
                                   InstituteName = aa.iti_college_name,
                                   PaymentOptionval = tad.PaymentOptionval,
                                   OfficerName = tvom.Name,
                                   DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber

                               }
                                             ).Distinct().ToList();
                        return res;
                    }
                    else
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tat.ReVerficationStatus == true && tad.IsActive == true
                                                   select tat.ReVerficationStatus).Take(1).FirstOrDefault();

                        if (ExistingRecordForUpdate)
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                                   join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                                   join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true && tad.ApplyYear == year && aa.CourseCode == courseType && tad.ApplicantType == applicanType
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       Remarks = tad.ApplRemarks,
                                       SubmitDate = tad.CreatedOn,
                                       OfficerName = tvom.Name,
                                       CourseType = bb.course_type_name,
                                       Year = tad.ApplyYear,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       StatusName = afds.ApplDescription,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 ? "Document verification pending" :
                                       //               tad.ApplStatus == 10 ? "Documents Re-Verified" :
                                       //               tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                       //                "Unknown"
                                       //       ),
                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       MobileNumber = tad.PhoneNumber,
                                       DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber
                                   }
                                         ).ToList();
                        }
                        else
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                                   join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                                   join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true && tad.ApplyYear == year && aa.CourseCode == courseType && tad.ApplicantType == applicanType
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       OfficerName = tvom.Name,
                                       Remarks = tad.ApplRemarks,
                                       SubmitDate = tad.CreatedOn,
                                       CourseType = bb.course_type_name,
                                       Year = tad.ApplyYear,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       StatusName = afds.ApplDescription,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 ? "Document verification pending" :
                                       //               tad.ApplStatus == 10 ? "Documents Verified" :
                                       //               tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                       //                "Unknown"
                                       //       ),
                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       MobileNumber = tad.PhoneNumber,
                                       DocumentFeeReceiptDetails = tvam.DocVeriFeeReceiptNumber
                                   }
                                         ).ToList();
                        }

                        if (res.Count == 0)
                        {
                            res = (from tad in _db.tbl_Applicant_Detail
                                   join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                                   join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                                   join afds in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals afds.ApplicationFormDescStatus_id into gj
                                   from afds in gj.DefaultIfEmpty()
                                       //join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                                   where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.CredatedBy != 0 ? tad.CredatedBy == loginId : true : true) && tad.IsActive == true && tad.ApplyYear == year && aa.CourseCode == courseType && tad.ApplicantType == applicanType
                                   select new VerificationOfficer
                                   {
                                       ApplicationId = tad.ApplicationId,
                                       CredatedBy = tad.CredatedBy,
                                       ApplicantNumber = tad.ApplicantNumber,
                                       ApplicantName = tad.ApplicantName,
                                       Remarks = tad.ApplRemarks,
                                       PaymentOptionval = tad.PaymentOptionval,
                                       SubmitDate = tad.CreatedOn,
                                       Year = tad.ApplyYear,
                                       //change to display year name 23 UC
                                       Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                       ApplStatus = tad.ApplStatus,
                                       ApplDescStatus = tad.ApplDescStatus,
                                       StatusName = afds.ApplDescription,
                                       //StatusName =
                                       //       (
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == true ? "Application Submitted" :
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == false && (tad.DocumentFeeReceiptDetails != null && tad.DocumentFeeReceiptDetails != "") ? "Document verification Pending" :
                                       //               tad.ApplStatus == 5 && tad.PaymentOptionval == false && (tad.DocumentFeeReceiptDetails == null || tad.DocumentFeeReceiptDetails == "")
                                       //               ? "Document verification fee Pending" : "Submitted"
                                       //       ),
                                       //changes for UC 23
                                       MisCode = aa.MISCode,
                                       InstituteName = aa.iti_college_name,
                                       MobileNumber = tad.PhoneNumber,

                                   }
                                         ).ToList();
                        }

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VerificationOfficer> GetApplicantsStatusDLL(int loginId, int roleId, int ApplicantType)
        {
            try
            {
                List<VerificationOfficer> res = new List<VerificationOfficer>();

                if (roleId == 12)
                {
                    res = (from tad in _db.tbl_Applicant_Detail
                           join tat in _db.tbl_ApplicantTrans on tad.ApplicationId equals tat.ApplicantId
                           join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                           join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                           join tsm in _db.tbl_status_master on tad.ApplStatus equals tsm.StatusId
                           where tvom.UserMasterId == loginId && tad.ApplicantType == ApplicantType && tad.IsActive == true
                           select new VerificationOfficer
                           {
                               ApplicationId = tad.ApplicationId,
                               CredatedBy = tad.CredatedBy,
                               ApplicantNumber = tad.ApplicantNumber,
                               ApplicantName = tad.ApplicantName,
                               StatusName =
                                              (
                                                      tad.ApplStatus == 5 ? "Document verification pending" :
                                                      tad.ApplStatus == 10 ? "Documents Verified" :
                                                      tad.ApplStatus == 10 && tat.ReVerficationStatus == true ? "Documents Re-Verified" :
                                                      tad.ApplStatus == 9 ? "More Information required (Incomplete/Incorrect)" :
                                                      "Re-Submitted"
                                              ),
                               Remarks = tad.ApplRemarks
                           }
                                         ).Distinct().ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VerificationOfficer> GetDataDocumentsVerificationFee(int loginId, int roleId, int? year, int? courseType, int? applicanType, int division_id, int district_lgd_code, int taluk_lgd_code, int InstituteId)
        {
            string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
            if (yr != null)
            {
                yr = yr.Split('-')[1];
                year = Convert.ToInt32(yr);
            }
            var res = (from tad in _db.tbl_Applicant_Detail
                       join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                       join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                       join tsm in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals tsm.ApplicationFormDescStatus_id into gj
                       from tsm in gj.DefaultIfEmpty()
                       join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                       join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                       join ty in _db.tbl_ApplicantType on tad.ApplicantType equals ty.ApplicantTypeId
                       join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                       from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                       join DivM in _db.tbl_division_master on aa.division_id equals DivM.division_id into divisionDetails
                       from DivMDetails in divisionDetails.DefaultIfEmpty()
                       join DistM in _db.tbl_district_master on aa.district_id equals DistM.district_lgd_code into districtDetails
                       from DistDetails in districtDetails.DefaultIfEmpty()
                       join TM in _db.tbl_taluk_master on aa.taluk_id equals TM.taluk_lgd_code into talukDetails
                       from TMDetails in talukDetails.DefaultIfEmpty()
                       where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.AssignedVO != 0 ? tad.AssignedVO == loginId : true : true) && tad.IsActive == true && (courseType != null ? aa.CourseCode == courseType : true) && (year != null ? tad.ApplyYear == year : true) && (applicanType != null ? tad.ApplicantType == applicanType : true) && tvam.DocVeriFeePymtStatus != null
                       && (division_id != 0 ? aa.division_id == division_id : true) && (district_lgd_code != 0 ? aa.district_id == district_lgd_code : true)
                        && (taluk_lgd_code != 0 ? aa.taluk_id == taluk_lgd_code : true) && (InstituteId != 0 ? aa.iti_college_id == InstituteId : true)
                       select new VerificationOfficer
                       {
                           ApplicationId = tad.ApplicationId,
                           CredatedBy = tad.CredatedBy,
                           ApplicantNumber = tad.ApplicantNumber,
                           ApplicantName = tad.ApplicantName,
                           Remarks = tad.ApplRemarks,
                           SubmitDate = tad.CreatedOn,
                           OfficerName = tvom.Name,
                           CourseType = bb.course_type_name,
                           Year = tad.ApplyYear,
                           ApplStatus = tad.ApplStatus,
                           DocVeriFees = tvam.DocVeriFee,
                           DocVeriFeeReceiptNumbers = tvam.DocVeriFeeReceiptNumber,
                           DocumentVeriFeePymtDate = (DateTime)tvam.DocVeriFeePymtDate,
                           Treasury_Receipt_No = tvam.Treasury_Receipt_No,
                           StatusName = tsm.ApplDescription,
                           ApplyYear = tad.ApplyYear,
                           //change to display year name 23 UC
                           Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                           ApplicantType = tad.ApplicantType,
                           InstituteName = aa.iti_college_name,
                           MisCode = aa.MISCode,
                           PaymentOptionval = tad.PaymentOptionval,
                           ApplicationMode = AMDetails.ApplicationMode,
                           Divisionname = DivMDetails.division_name,
                           Districtname = DistDetails.district_ename,
                           Talukname = TMDetails.taluk_ename
                       }).Distinct().ToList();
            //foreach (var p in res)
            //{						               
            //    if (applicanType != 0 && applicanType != null)
            //	{
            //		res = res.Where(a => a.ApplicantType == applicanType).ToList();
            //	}
            //}
            return res;
        }

        public List<VerificationOfficer> GetDataDocumentsVerificationFeepayment(int loginId, int roleId, int? year, int? courseType, int? applicanType)
        {
            string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
            if (yr != null)
            {
                yr = yr.Split('-')[1];
                year = Convert.ToInt32(yr);
            }
            var res = (from tad in _db.tbl_Applicant_Detail
                       join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId
                       join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                       join tsm in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals tsm.ApplicationFormDescStatus_id into gj
                       from tsm in gj.DefaultIfEmpty()
                       join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id
                       join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                       join ty in _db.tbl_ApplicantType on tad.ApplicantType equals ty.ApplicantTypeId
                       where (roleId != (int)CmnClass.Role.ITIAdmin ? tad.AssignedVO != 0 ? tad.AssignedVO == loginId : true : true) && tad.IsActive == true && (courseType != null ? aa.CourseCode == courseType : true) && (year != null ? tad.ApplyYear == year : true) && (applicanType != null ? tad.ApplicantType == applicanType : true) && tvam.DocVeriFeePymtStatus == null
                       select new VerificationOfficer
                       {
                           ApplicationId = tad.ApplicationId,
                           CredatedBy = tad.CredatedBy,
                           ApplicantNumber = tad.ApplicantNumber,
                           ApplicantName = tad.ApplicantName,
                           Remarks = tad.ApplRemarks,
                           SubmitDate = tad.CreatedOn,
                           OfficerName = tvom.Name,
                           CourseType = bb.course_type_name,
                           Year = tad.ApplyYear,
                           ApplStatus = tad.ApplStatus,
                           DocVeriFees = tvam.DocVeriFee,
                           DocVeriFeeReceiptNumbers = tvam.DocVeriFeeReceiptNumber,
                           DocumentVeriFeePymtDate = (DateTime)tvam.DocVeriFeePymtDate,
                           StatusName = tsm.ApplDescription,
                           ApplyYear = tad.ApplyYear,
                           //change to display year name 23 UC
                           Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                           ApplicantType = tad.ApplicantType,
                           InstituteName = aa.iti_college_name,
                           MisCode = aa.MISCode,
                           PaymentOptionval = tad.PaymentOptionval,


                       }).ToList();
            //foreach (var p in res)
            //{						               
            //    if (applicanType != 0 && applicanType != null)
            //	{
            //		res = res.Where(a => a.ApplicantType == applicanType).ToList();
            //	}
            //}
            return res;
        }

        public List<VerificationOfficer> GetDataDocumentsVerificationFeeNotPaid(int loginId, int roleId, int? year, int? courseType, int? applicanType, string ApplNo)
        {
            string yr = _db.tbl_Year.Where(x => x.YearID == year).Select(y => y.Year).FirstOrDefault();
            if (yr != null)
            {
                yr = yr.Split('-')[1];
                year = Convert.ToInt32(yr);
            }
            var res = (from tad in _db.tbl_Applicant_Detail
                       join tvam in _db.tbl_VerOfficer_Applicant_Mapping on tad.ApplicationId equals tvam.ApplicantId into tvr
                       from tvam in tvr.DefaultIfEmpty()
                           //join tvom in _db.tbl_VerificationOfficer_Master on tvam.VerifiedOfficer equals tvom.Officer_Id
                       join tsm in _db.tbl_ApplicationFormDescStatus on tad.ApplDescStatus equals tsm.ApplicationFormDescStatus_id into gj
                       from tsm in gj.DefaultIfEmpty()
                       join aa in _db.tbl_iti_college_details on tad.DocVerificationCentre equals aa.iti_college_id into itc
                       from aa in itc.DefaultIfEmpty()
                       join bb in _db.tbl_course_type_mast on aa.CourseCode equals bb.course_id
                       join ty in _db.tbl_ApplicantType on tad.ApplicantType equals ty.ApplicantTypeId


                       join AM in _db.tbl_ApplicationMode on tad.ApplicationMode equals AM.ApplicationModeId into ApplicationModeDetails
                       from AMDetails in ApplicationModeDetails.DefaultIfEmpty()
                       where tad.IsActive == true && (ApplNo != null ? tad.ApplicantNumber == ApplNo : false) && tad.AgainstVacancyInd == 1 && tad.ApplDescStatus != 4
                       && (courseType != null ? aa.CourseCode == courseType : true) && (year != null ? tad.ApplyYear == year : true) && (applicanType != null ? tad.ApplicantType == applicanType : true) /*&& tvam.DocVeriFeePymtStatus == null */

                       select new VerificationOfficer
                       {
                           ApplicationId = tad.ApplicationId,
                           CredatedBy = tad.CredatedBy,
                           ApplicantNumber = tad.ApplicantNumber,
                           ApplicantName = tad.ApplicantName,
                           Remarks = tad.ApplRemarks,
                           SubmitDate = tad.CreatedOn,
                           //OfficerName = tvom.Name,
                           CourseType = bb.course_type_name,
                           Year = tad.ApplyYear,
                           ApplStatus = tad.ApplStatus,
                           DocVeriFees = tvam.DocVeriFee,
                           DocVeriFeeReceiptNumbers = tvam.DocVeriFeeReceiptNumber,
                           DocumentVeriFeePymtDate = (DateTime)tvam.DocVeriFeePymtDate,
                           StatusName = tsm.ApplDescription,
                           ApplyYear = tad.ApplyYear,
                           //change to display year name 23 UC
                           Session = _db.tbl_Year.Where(a => a.Year.Contains(tad.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                           ApplicantType = tad.ApplicantType,
                           InstituteName = aa.iti_college_name,
                           MisCode = aa.MISCode,
                           PaymentOptionval = tad.PaymentOptionval,
                           MobileNumber = tad.PhoneNumber,

                           ApplicationMode = AMDetails.ApplicationMode


                       }).Distinct().ToList();
            //foreach (var p in res)
            //{						               
            //    if (applicanType != 0 && applicanType != null)
            //	{
            //		res = res.Where(a => a.ApplicantType == applicanType).ToList();
            //	}
            //}
            return res;
        }
        public List<VerificationOfficer> GetVerificationFeeCmntDetailsByIdDLL(int ApplicationId)
        {
            var res = (from t in _db.tbl_Applicant_Detail.Where(x => x.ApplicationId == ApplicationId)
                       join tum in _db.tbl_user_master on t.AssignedVO equals tum.um_id
                       select new VerificationOfficer
                       {
                           //App = t.ApplicantITIInstituteId,
                           Apdate = (t.CreatedOn == null ? "NA" : t.CreatedOn.ToString()),
                           ApplicationId = t.ApplicationId,
                           Remarks = (t.ApplRemarks == "" || t.ApplRemarks == null ? "NA" : t.ApplRemarks),
                           FlowId = t.FlowId,
                           userRole = tum.um_name
                       }).Distinct().ToList();
            return res;
        }
        #endregion

        #region .. Application Form for seat allocation ..

        #region  Get ITI College Details Based on Pin code

        public List<ApplicationForm> GetITICollegeDetailsDLL(int PinCode)
        {
            var res = (dynamic)null;
            string pincodeLen = Convert.ToString(PinCode);
            if (pincodeLen.Length > 3)
            {
                int PStartingCode = PinCode - 50;
                int PEndingCode = PinCode + 50;

                res = (from ticd in _db.tbl_iti_college_details
                           //where ticd.PinCode == PinCode
                       where (ticd.PinCode >= PStartingCode && ticd.PinCode <= PEndingCode)
                       && ticd.is_active == true && ticd.Insitute_TypeId == 1
                       orderby ticd.iti_college_name
                       select new ApplicationForm
                       {
                           iti_college_code = ticd.iti_college_id,
                           iti_college_name = ticd.iti_college_name
                       }).ToList();
            }
            else
            {
                res = (from ticd in _db.tbl_iti_college_details
                       where ticd.district_id == PinCode && ticd.is_active == true && ticd.Insitute_TypeId == 1
                       orderby ticd.iti_college_name
                       select new ApplicationForm
                       {
                           iti_college_code = ticd.iti_college_id,
                           iti_college_name = ticd.iti_college_name
                       }).ToList();
            }

            return res;
        }

        public List<ApplicationForm> GetITICollegeDetailsByDistrictTalukaDLL(int District, int Taluka)
        {
            var res = (dynamic)null;
            res = (from ticd in _db.tbl_iti_college_details
                       //where ticd.district_id == District && ticd.taluk_id == Taluka && ticd.is_active == true
                   where ticd.district_id == District && ticd.taluk_id == Taluka
                   select new ApplicationForm
                   {
                       iti_college_code = ticd.iti_college_id,
                       iti_college_name = ticd.iti_college_name
                   }).ToList();

            return res;
        }

        #endregion

        #region  Get ITI Trade Details Based on District, Institute

        public List<ApplicationForm> GetITICollegeTradeDetailsDLL(int TradeCode, string qual)
        {
            qual = (qual != null && qual == "1") ? "8th" : "10th";
            var qualString = (qual != null && qual.Contains("10th")) ? "10th" : qual;
            var res = (from tIT in _db.tbl_ITI_Trade
                       join ttm in _db.tbl_trade_mast on tIT.TradeCode equals ttm.trade_id

                       where tIT.ITICode == TradeCode && tIT.ActiveDeActive == true
                       && ttm.trade_Mini_Qualification.Contains(qualString)
                       select new ApplicationForm
                       {
                           trade_id = ttm.trade_id,
                           trade_name = ttm.trade_name
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Religion Details

        public List<ApplicationForm> GetReligionDetailsDLL()
        {
            var res = (from tbr in _db.tbl_Religion
                       where tbr.IsActive == true
                       select new ApplicationForm
                       {
                           Religion_Id = tbr.Religion_Id,
                           Religion = tbr.Religion
                       }).ToList();

            return res;
        }
        public List<ApplicationForm> GetcoursetypeListbycalendar()
        {
            var res = (from ten in _db.tbl_tentative_calendar_of_events
                       join cou in _db.tbl_course_type_mast on ten.Course_Id equals cou.course_id
                       where cou.is_active == true /*&& ten.Course_Id== (int)CmnClass.courseType.NCVT*/
                       select new ApplicationForm
                       {
                           course_type_name = cou.course_type_name.ToString(),
                           course_Id = cou.course_id
                       }).Distinct().OrderByDescending(x => x.course_Id).ToList();

            return res;
        }

        #endregion

        #region  Get Gender Details

        public List<ApplicationForm> GetGenderDetailsDLL()
        {
            var res = (from tbr in _db.tbl_Gender
                       where tbr.IsActive == true
                       select new ApplicationForm
                       {
                           Gender_Id = tbr.Gender_Id,
                           Gender = tbr.Gender
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Document Application Status Details

        public List<ApplicationForm> GetDocumentApplicationStatusDLL()
        {
            var res = (from tbr in _db.tbl_status_master
                       where tbr.IsActive == true && (tbr.StatusId == 3 || tbr.StatusId == 14 || tbr.StatusId == 15)
                       orderby tbr.StatusId descending
                       select new ApplicationForm
                       {
                           ApplDocVerifiID = tbr.StatusId,
                           VerificationStatus = tbr.StatusName
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Category Details

        public List<ApplicationForm> GetCategoryListDLL()
        {
            //var res = (from tbr in _db.tbl_Category
            //           where tbr.IsActive == true
            //           select new ApplicationForm
            //           {
            //               CategoryId = tbr.CategoryId,
            //               Category = tbr.Category
            //           }).ToList();
            //select a.Category,a.CategoryId from tbl_Category a left outer join
            //tbl_Category b on a.CategoryId = b.ParentCategory
            //group by a.Category,a.CategoryId having COUNT(a.Category) = 1 order by a.CategoryId
            //var res = (from tbr in _db.tbl_Category
            //          join tbr1 in _db.tbl_Category on tbr.CategoryId equals tbr1.ParentCategory
            //          where tbr.IsActive == true
            //          group tbr by new { tbr.Category, tbr.CategoryId } into CateLeft
            //          from CL in CateLeft.DefaultIfEmpty()
            //          where CateLeft.Count() == 1
            //          orderby CL.CategoryId
            //          select new ApplicationForm
            //          {
            //              CategoryId = CL.CategoryId,
            //              Category = CL.Category
            //          }).ToList();

            var list1 = (from a in _db.tbl_Category select new { a.CategoryId, a.Category, a.ParentCategory }).ToList();

            var list2 = list1.Where(x => x.ParentCategory != null).ToList().Select(s => s.ParentCategory).Distinct();
            list1.RemoveAll(l => list2.Contains(l.CategoryId));
            var res = (from CL in list1
                       select new ApplicationForm
                       {
                           CategoryId = CL.CategoryId,
                           Category = CL.Category
                       }).ToList();
            list1 = null; list2 = null;
            return res;
        }

        #endregion

        #region  Get Applicant Type

        public List<ApplicationForm> GetApplicantTypeListDLL(int roleId)
        {
            var res = (from tbr in _db.tbl_ApplicantType
                       where tbr.IsActive == true
                       select new ApplicationForm
                       {
                           ApplicantTypeId = tbr.ApplicantTypeId,
                           ApplicantType = tbr.ApplicantType
                       }).ToList();
            if (roleId != (int)CmnClass.Role.ITIAdmin || roleId != (int)CmnClass.Role.ITIAdmin)
            {
                res = res.Where(a => a.ApplicantTypeId != (int)CmnClass.ApplicantTypes.Direct).ToList();
            }

            return res;
        }

        #endregion

        #region  Get Applicant Type

        public List<ApplicationForm> GetCasteListDLL()
        {
            var res = (from tbr in _db.tbl_Caste
                       where tbr.IsActive == true
                       select new ApplicationForm
                       {
                           CasteId = tbr.CasteId,
                           Caste = tbr.Caste
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get PersonWithDisabilityCategory

        public List<PersonWithDisabilityCategory> PersonWithDisabilityCategoryDLL()
        {
            var res = (from tbr in _db.tbl_PersonWithDisabilityCategory
                       where tbr.IsActive == true
                       select new PersonWithDisabilityCategory
                       {
                           PersonWithDisabilityCategoryId = tbr.PersonWithDisabilityCategoryId,
                           DisabilityName = tbr.DisabilityName
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Reservation Type

        public List<ApplicationForm> GetReservationsListDLL()
        {
            var res = (from tbr in _db.tbl_reservation
                       where tbr.IsActive == 1
                       select new ApplicationForm
                       {
                           ReservationId = tbr.ReservationId,
                           Reservations = tbr.Reservations
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Syllabus List

        public List<ApplicationForm> GetSyllabusListDLL()
        {
            var res = (from tbr in _db.tbl_syllabus_type
                       where tbr.IsActive == 1
                       select new ApplicationForm
                       {
                           Syllabus_type_id = tbr.Syllabus_type_id,
                           Syllabus_type = tbr.Syllabus_type
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Qualification List

        public List<ApplicationForm> GetQualificationListDLL()
        {
            var res = (from tbr in _db.tbl_qualification
                       where tbr.IsActive == true
                       select new ApplicationForm
                       {
                           QualificationId = tbr.QualificationId,
                           Qualification = tbr.Qualification
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Location List

        public List<ApplicationForm> GetLocationListDLL()
        {
            var res = (from tbr in _db.tbl_location_type
                       where tbr.is_active == true
                       select new ApplicationForm
                       {
                           location_id = tbr.location_id,
                           location_name = tbr.location_name
                       }).ToList();

            return res;
        }

        #endregion

        #region  Get Qualification List

        public List<ApplicationForm> GetDistrictMasterListDLL()
        {
            var res = (from tbr in _db.tbl_district_master
                       where tbr.dis_is_active == true
                       select new ApplicationForm
                       {
                           district_lgd_code = tbr.district_lgd_code,
                           district_ename = tbr.district_ename
                       }).ToList();

            return res;
        }

        public List<ApplicationForm> GetOtherBoardsListDLL()
        {
            var res = (from tbod in _db.tbl_OtherBoards_Details
                       where tbod.IsActive == true
                       select new ApplicationForm
                       {
                           BoardId = tbod.BoardId,
                           BoardName = tbod.NameOfBoard
                       }).ToList();

            return res;
        }

        #endregion

        #region  .. Get District with Division List ..

        public List<ApplicationForm> GetDistrictMasterDivListDLL(int DivisionId)
        {
            var res = (from tbr in _db.tbl_district_master
                       where tbr.dis_is_active == true && tbr.division_id == DivisionId
                       select new ApplicationForm
                       {
                           district_lgd_code = tbr.district_lgd_code,
                           district_ename = tbr.district_ename
                       }).ToList();

            return res;
        }

        #endregion

        public List<ApplicationForm> GetTalukMasterListDLL(int distId)
        {
            var res = (from a in _db.tbl_taluk_master
                       where a.taluk_is_active == true && a.district_lgd_code == distId
                       select new ApplicationForm
                       {
                           taluk_lgd_code = a.taluk_lgd_code,
                           taluk_ename = a.taluk_ename
                       }).ToList();
            return res;
        }

        public List<ApplicantApplicationForm> GetEligibleDateFrmCalenderEventsDLL()
        {
            List<ApplicantApplicationForm> res = new List<ApplicantApplicationForm>();

            res = (from aa in _db.tbl_tentative_calendar_of_events
                   join bb in _db.tbl_Tentative_admsn_eventDetails on aa.Tentative_admsn_evnt_clndr_Id equals bb.Tentative_admsn_evnt_clndr_Id
                   where aa.StatusId == 106 && aa.Course_Id == 101 // After publish calendar events form displayed.
                                                                   //where aa.StatusId == 109 && aa.Course_Id == 101 --Approved after form displayed.
                   select new ApplicantApplicationForm
                   {
                       FromDt_ApplyingOnlineApplicationForm = bb.FromDt_ApplyingOnlineApplicationForm,
                       ToDt_ApplyingOnlineApplicationForm = bb.ToDt_ApplyingOnlineApplicationForm,
                       FromDt_DocVerificationPeriod = bb.FromDt_DocVerificationPeriod,
                       ToDt_DocVerificationPeriod = bb.ToDt_DocVerificationPeriod,
                       FromDt_2ndRoundEntryChoiceTrade = bb.FromDt_2ndRoundEntryChoiceTrade,
                       ToDt_2ndRoundEntryChoiceTrade = bb.ToDt_2ndRoundEntryChoiceTrade,
                       FromDt_1stRoundAdmissionProcess = (DateTime)bb.FromDt_1stRoundAdmissionProcess,
                       ToDt_1stRoundAdmissionProcess = (DateTime)bb.ToDt_1stRoundAdmissionProcess,
                       FromDt_2ndRoundAdmissionProcess = (DateTime)bb.FromDt_2ndRoundAdmissionProcess,
                       ToFDt_2ndRoundAdmissionProcess = (DateTime)bb.ToFDt_2ndRoundAdmissionProcess,
                   }).ToList();

            if (res != null)
            {
                DateTime from_date1_applicationForm; DateTime to_date1_applicationForm;

                foreach (var item in res)
                {
                    from_date1_applicationForm = (DateTime)item.FromDt_ApplyingOnlineApplicationForm;
                    to_date1_applicationForm = (DateTime)item.ToDt_ApplyingOnlineApplicationForm;
                    item.From_date_applicationForm = from_date1_applicationForm.ToString("yyyy,MM,d");
                    item.To_date_applicationForm = to_date1_applicationForm.ToString("yyyy,MM,d");
                }

                DateTime from_date1_VerificationOfficer; DateTime to_date1_VerificationOfficer;
                foreach (var item in res)
                {
                    from_date1_VerificationOfficer = (DateTime)item.FromDt_DocVerificationPeriod;
                    to_date1_VerificationOfficer = (DateTime)item.ToDt_DocVerificationPeriod;
                    item.From_date_VerificationOfficer = from_date1_VerificationOfficer.ToString("yyyy,MM,d");
                    item.To_date_VerificationOfficer = to_date1_VerificationOfficer.ToString("yyyy,MM,d");
                }

                DateTime from_date1_2ndRoundEntryChoiceTrade; DateTime to_date1_2ndRoundEntryChoiceTrade;
                foreach (var item in res)
                {
                    from_date1_2ndRoundEntryChoiceTrade = (DateTime)item.FromDt_2ndRoundEntryChoiceTrade;
                    to_date1_2ndRoundEntryChoiceTrade = (DateTime)item.ToDt_2ndRoundEntryChoiceTrade;
                    item.From_date_2ndRoundEntryChoiceTrade = from_date1_2ndRoundEntryChoiceTrade.ToString("yyyy,MM,d");
                    item.To_date_2ndRoundEntryChoiceTrade = to_date1_2ndRoundEntryChoiceTrade.ToString("yyyy,MM,d");
                }
            }

            return res;
        }

		public ApplicantApplicationForm GetMasterApplicantDataDLL(int loginId, string DataFrom)
		{
			if (DataFrom == "AP")
			{
				var res = (from a in _db.tbl_Applicant_Detail
						   //join bb in _db.tbl_VerOfficer_Applicant_Mapping on a.ApplicationId equals bb.ApplicantId
						   where a.CredatedBy == loginId && a.IsActive == true
									 select new ApplicantApplicationForm
									 {
										 RStateBoardType = a.RStateBoardType,
										 RAppBasics = a.RAppBasics,
										 RollNumber = a.RollNumber,
										 ApplyMonth = a.ApplyMonth,
										 ApplyYear = a.ApplyYear,
										 ApplicationId = a.ApplicationId,
										 ApplicantName = a.ApplicantName,
										 ApplicantNumber = a.ApplicantNumber,
										 CredatedBy = a.CredatedBy,
										 FathersName = a.FathersName,
										 ParentsOccupation = a.ParentsOccupation,
										 PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
										 PhysicallyHanidcapType = a.PhysicallyHanidcapType,
										 HoraNadu_GadiNadu_Kannidagas = a.HoraNadu_GadiNadu_Kannidagas,
										 ExemptedFromStudyCertificate = a.ExemptedFromStudyCertificate,
										 HyderabadKarnatakaRegion = a.HyderabadKarnatakaRegion,
										 KanndaMedium = a.KanndaMedium,
										 RationCard = a.RationCard,
										 AadhaarNumber = a.AadhaarNumber,
										 AccountNumber = a.AccountNumber,
										 BankName = a.BankName,
										 IFSCCode = a.IFSCCode,
										 Photo = a.Photo,
										 DOB = a.DOB,
										 Gender = a.Gender,
										 MothersName = a.MothersName,
										 Religion = a.Religion,
										 Category = a.Category,
										 MinorityCategory = a.MinorityCategory,
										 Caste = a.Caste,
										 CasteDetail = a.CasteDetail,
										 Percentage = a.Percentage,
										 FamilyAnnIncome = a.FamilyAnnIncome,
										 Qualification = a.Qualification,
										 ApplicantBelongTo = a.ApplicantBelongTo,
										 ApplicantType = a.ApplicantType,
										 AppliedBasic = a.AppliedBasic,
										 TenthBoard = a.TenthBoard,
										 studiedMathsScience = a.studiedMathsScience,
										 InstituteStudiedQual = a.InstituteStudiedQual,
										 MaxMarks = a.MaxMarks,
										 MinMarks = a.MinMarks,
										 MarksObtained = a.MarksObtained,
										 ResultQual = a.ResultQual,
										 CommunicationAddress = a.CommunicationAddress,
										 DistrictId = a.DistrictId,
										 TalukaId = a.TalukaId,
										 Pincode = a.Pincode,
										 SameAdd = a.SameAdd,
										 PermanentAddress = a.PermanentAddress,
										 PDistrict = a.PDistrict,
										 PTaluk = a.PTaluk,
										 PPinCode = a.PPinCode,
										 PhoneNumber = a.PhoneNumber,
										 FatherPhoneNumber = a.FatherPhoneNumber,
										 EmailId = a.EmailId,
										 DocVeriInstituteFilter = a.DocVeriInstituteFilter,
										 DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
										 DocVerificationCentre = a.DocVerificationCentre,
										 ApplRemarks = a.ApplRemarks,
										 PaymentOptionval = a.PaymentOptionval,
										 DocumentFeeReceiptDetails = a.DocumentFeeReceiptDetails,
										 ApplStatus = a.ApplStatus,
										 TenthCOBSEBoard = a.TenthCOBSEBoard,
										 EducationCGPA = a.EducationCGPA,
										 EducationGrade = a.EducationGrade,
										 ParticipateNextRound = a.ParticipateNextRound,
										 FlowId = a.FlowId,
										 AssignedVO = a.AssignedVO,
										 ReVerficationStatus = a.ReVerficationStatus,
										 ExServiceMan = a.ExServiceMan,
										 EconomyWeakerSection = a.EconomyWeakerSection,
										 ApplicationMode=a.ApplicationMode,
										 Caste_RD_No=a.Caste_Categaory_Income_RD,
										 EconomicWeaker_RD_No=a.Economic_Weaker_Section_RD,
										 HYD_Karnataka_RD_No=a.Hyderabada_Karnataka_Region_RD,
										 UDID_No=a.UDID_Number
									 }).FirstOrDefault();
				return res;
			}
			else
			{
				var res = (from a in _db.tbl_Applicant_Detail
									 where a.ApplicationId == loginId && a.IsActive == true
									 select new ApplicantApplicationForm
									 {
										 RStateBoardType = a.RStateBoardType,
										 RAppBasics = a.RAppBasics,
										 RollNumber = a.RollNumber,
										 ApplyMonth = a.ApplyMonth,
										 ApplyYear = a.ApplyYear,
										 ApplicationId = a.ApplicationId,
										 ApplicantName = a.ApplicantName,
										 ApplicantNumber = a.ApplicantNumber,
										 CredatedBy = a.CredatedBy,
										 FathersName = a.FathersName,
										 ParentsOccupation = a.ParentsOccupation,
										 PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
										 PhysicallyHanidcapType = a.PhysicallyHanidcapType,
										 RationCard = a.RationCard,
										 AadhaarNumber = a.AadhaarNumber,
										 AccountNumber = a.AccountNumber,
										 BankName = a.BankName,
										 IFSCCode = a.IFSCCode,
										 Photo = a.Photo,
										 DOB = a.DOB,
										 Gender = a.Gender,
										 MothersName = a.MothersName,
										 Religion = a.Religion,
										 Category = a.Category,
										 MinorityCategory = a.MinorityCategory,
										 Caste = a.Caste,
										 Percentage = a.Percentage,
										 FamilyAnnIncome = a.FamilyAnnIncome,
										 studiedMathsScience = a.studiedMathsScience,
										 Qualification = a.Qualification,
										 ApplicantBelongTo = a.ApplicantBelongTo,
										 ApplicantType = a.ApplicantType,
										 AppliedBasic = a.AppliedBasic,
										 TenthBoard = a.TenthBoard,
										 InstituteStudiedQual = a.InstituteStudiedQual,
										 MaxMarks = a.MaxMarks,
										 MinMarks = a.MinMarks,
										 MarksObtained = a.MarksObtained,
										 ResultQual = a.ResultQual,
										 CommunicationAddress = a.CommunicationAddress,
										 DistrictId = a.DistrictId,
										 TalukaId = a.TalukaId,
										 Pincode = a.Pincode,
										 SameAdd = a.SameAdd,
										 PermanentAddress = a.PermanentAddress,
										 PDistrict = a.PDistrict,
										 PTaluk = a.PTaluk,
										 PPinCode = a.PPinCode,
										 PhoneNumber = a.PhoneNumber,
										 FatherPhoneNumber = a.FatherPhoneNumber,
										 EmailId = a.EmailId,
										 DocVeriInstituteFilter = a.DocVeriInstituteFilter,
										 DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
										 DocVerificationCentre = a.DocVerificationCentre,
										 ApplRemarks = a.ApplRemarks,
										 PaymentOptionval = a.PaymentOptionval,
										 DocumentFeeReceiptDetails = a.DocumentFeeReceiptDetails,
										 ApplStatus = a.ApplStatus,
										 TenthCOBSEBoard = a.TenthCOBSEBoard,
										 EducationCGPA = a.EducationCGPA,
										 BoardId = a.BoardId,
										 EducationGrade = a.EducationGrade,
										 ParticipateNextRound = a.ParticipateNextRound,
										 FlowId = a.FlowId,
										 AssignedVO = a.AssignedVO,
										 HoraNadu_GadiNadu_Kannidagas = a.HoraNadu_GadiNadu_Kannidagas,
										 ExemptedFromStudyCertificate = a.ExemptedFromStudyCertificate,
										 HyderabadKarnatakaRegion = a.HyderabadKarnatakaRegion,
										 KanndaMedium = a.KanndaMedium,
										 ExServiceMan = a.ExServiceMan,
										 EconomyWeakerSection = a.EconomyWeakerSection,
										 ApplDescription=a.ApplDescStatus.ToString(),

										 ApplicationMode = a.ApplicationMode,
										 Caste_RD_No = a.Caste_Categaory_Income_RD,
										 EconomicWeaker_RD_No = a.Economic_Weaker_Section_RD,
										 HYD_Karnataka_RD_No = a.Hyderabada_Karnataka_Region_RD,
										 UDID_No = a.UDID_Number
									 }).FirstOrDefault();
				return res;
			}
		}

		public ApplicantApplicationForm GetUserMasterDataDLL(int loginId, string DataFrom)
		{
			ApplicantApplicationForm result = new ApplicantApplicationForm();
			if (DataFrom == "AP")
			{
				var res = (from a in _db.tbl_user_master where a.um_id == loginId && a.um_is_active == true select a).FirstOrDefault();
				result.ApplicantName = res.um_name;
				result.ApplicantNumber = res.um_mobile_no;
				result.EmailId = res.um_email_id;
			}
			return result;

		}

        public List<ApplicantDocumentsDetail> GetDocumentDetailsDLL(int loginId)
        {
            List<ApplicantDocumentsDetail> res = new List<ApplicantDocumentsDetail>();
            using (SqlConnection con = new SqlConnection(SQLConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(SPNameFetchGrievanceDocDetails, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApplicationID", loginId).SqlDbType = SqlDbType.Int;
                con.Open();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                res = ds.Tables[0].AsEnumerable().Select(row => new ApplicantDocumentsDetail
                {
                    DocAppId = row.Field<int>("DocAppId"),
                    ApplicantId = row.Field<int>("ApplicantId"),
                    DocumentTypeId = row.Field<int>("DocumentTypeId"),
                    FileName = row.Field<string>("FileName"),
                    FilePath = row.Field<string>("FilePath"),
                    Verified = row.Field<int>("Verified"),
                    CreatedBy = row.Field<int>("CreatedBy"),
                    Remarks = row.Field<string>("Remarks"),
                    UpdatedBy = row.Field<int>("UpdatedBy"),
                    DocumentSetInd = row.Field<int?>("DocumentSet")
                }).ToList();
            }
            return res;
        }

        public List<ApplicantInstitutePreference> GetApplicantInstitutePreferenceDLL(int loginId)
        {
            var res = (from a in _db.tbl_Applicant_InstitutePreference
                       join tsm in _db.tbl_iti_college_details on a.InstituteId equals tsm.iti_college_id
                       join ttm in _db.tbl_trade_mast on a.TradeId equals ttm.trade_id

                       //where a.CreatedBy == loginId
                       where a.ApplicantId == loginId
                       select new ApplicantInstitutePreference
                       {
                           InstitutePreferenceId = a.InstitutePreferenceId,
                           ApplicantId = a.ApplicantId,
                           PreferenceId = a.PreferenceId,
                           InstituteId = a.InstituteId,
                           PreferenceType = a.PreferenceType,
                           TradeId = a.TradeId,
                           DistrictId = a.DistrictId,
                           TalukaId = a.TalukaId,
                           InstituteName = tsm.iti_college_name,
                           TradeName = ttm.trade_name

                       }).ToList();
            return res;
        }

        public ApplicantApplicationForm GetAdmissionDocumentDetailsDLL(int ApplicationId)
        {
            var res = (from a in _db.tbl_Applicant_Detail
                       where a.ApplicationId == ApplicationId && a.IsActive == true
                       select new ApplicantApplicationForm
                       {
                           ApplicantName = a.ApplicantName,
                           FathersName = a.FathersName,
                           ParentsOccupation = a.ParentsOccupation,
                           Photo = a.Photo,
                           DOB = a.DOB,
                           Gender = a.Gender,
                           MothersName = a.MothersName,
                           Religion = a.Religion,
                           Category = a.Category,
                           MinorityCategory = a.MinorityCategory,
                           Caste = a.Caste,
                           FamilyAnnIncome = a.FamilyAnnIncome,
                           ApplicantType = a.ApplicantType

                       }).FirstOrDefault();
            return res;
        }

        #region .. Add Institute Preference ..

        public List<ApplicantInstitutePreference> AddInstituePreferenceDetailsDLL(ApplicantInstitutePreference objApplicantInstitutePreference)
        {
            List<ApplicantInstitutePreference> resData = new List<ApplicantInstitutePreference>();
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantInstitutePreference.ApplicantId;
                    int InstitutePreferenceId = objApplicantInstitutePreference.PreferenceId;
                    if (InstitutePreferenceId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_InstitutePreference
                                                   where tad.ApplicantId == ApplicationId && tad.PreferenceId == objApplicantInstitutePreference.PreferenceId &&
                                                   tad.InstitutePreferenceId == objApplicantInstitutePreference.InstitutePreferenceId
                                                   select tad.InstitutePreferenceId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate != 0)
                    {
                        //var remove_query = _db.tbl_Applicant_InstitutePreference.Where(s => s.ApplicantId == ApplicationId && s.PreferenceId == objApplicantInstitutePreference.PreferenceId
                        //&& s.InstitutePreferenceId == objApplicantInstitutePreference.InstitutePreferenceId).ToList();
                        var remove_query = _db.tbl_Applicant_InstitutePreference.Where(s => s.ApplicantId == ApplicationId).ToList();
                        foreach (var item in remove_query)
                        {
                            _db.tbl_Applicant_InstitutePreference.Remove(item);
                        }
                        _db.SaveChanges();
                    }

                    tbl_Applicant_InstitutePreference objtbl_Applicant_InstitutePreference = new tbl_Applicant_InstitutePreference();
                    objtbl_Applicant_InstitutePreference.ApplicantId = objApplicantInstitutePreference.ApplicantId;
                    objtbl_Applicant_InstitutePreference.PreferenceId = objApplicantInstitutePreference.PreferenceId;
                    objtbl_Applicant_InstitutePreference.InstituteId = objApplicantInstitutePreference.InstituteId;
                    objtbl_Applicant_InstitutePreference.PreferenceType = objApplicantInstitutePreference.PreferenceType;
                    objtbl_Applicant_InstitutePreference.TradeId = objApplicantInstitutePreference.TradeId;
                    objtbl_Applicant_InstitutePreference.DistrictId = objApplicantInstitutePreference.DistrictId;
                    objtbl_Applicant_InstitutePreference.TalukaId = objApplicantInstitutePreference.TalukaId;
                    objtbl_Applicant_InstitutePreference.CreatedBy = objApplicantInstitutePreference.CreatedBy;
                    objtbl_Applicant_InstitutePreference.CreatedOn = DateTime.Now;
                    objtbl_Applicant_InstitutePreference.IsActive = true;

                    _db.tbl_Applicant_InstitutePreference.Add(objtbl_Applicant_InstitutePreference);
                    _db.SaveChanges();

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantInstitutePreference.ApplicantId;
                    //objtbl_ApplicantTrans.CreatedBy = objApplicantInstitutePreference.CredatedBy;
                    //objtbl_ApplicantTrans.FlowId = objApplicantInstitutePreference.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    //objtbl_ApplicantTrans.Status = objApplicantInstitutePreference.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantInstitutePreference.VerfOfficer;
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    //if (objApplicantInstitutePreference.ReVerficationStatus == 1)
                    //    objtbl_ApplicantTrans.ReVerficationStatus = true;
                    //else
                    //    objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Institue Preference Basic Details by Applicant";
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantInstitutePreference.ApplicantId && s.IsActive == true).FirstOrDefault();
                    update_query.ParticipateNextRound = objApplicantInstitutePreference.ParticipateNextRound;
                    _db.SaveChanges();

                    resData = (from a in _db.tbl_Applicant_InstitutePreference
                               where a.ApplicantId == ApplicationId //&& a.PreferenceId == objApplicantInstitutePreference.PreferenceId
                               select new ApplicantInstitutePreference
                               {
                                   InstitutePreferenceId = a.InstitutePreferenceId,
                                   ApplicantId = a.ApplicantId,
                                   PreferenceId = a.PreferenceId,
                                   InstituteId = a.InstituteId,
                                   PreferenceType = a.PreferenceType,
                                   TradeId = a.TradeId,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId

                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
            }

            return resData;
        }

        public ApplicantApplicationForm UpdateNextRoundDetailsWithTransDLL(ApplicantApplicationForm formCollection)
        {
            ApplicantApplicationForm resData = new ApplicantApplicationForm();
            using (var transaction = new TransactionScope())
            {
                tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == formCollection.ApplicantId && s.IsActive == true).FirstOrDefault();

                update_query.ParticipateNextRound = formCollection.ParticipateNextRound;
                _db.SaveChanges();

                tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                objtbl_ApplicantTrans.ApplicantId = formCollection.ApplicantId;
                objtbl_ApplicantTrans.VerfOfficer = formCollection.UpdatedBy;
                objtbl_ApplicantTrans.Status = 5;
                objtbl_ApplicantTrans.TransDate = DateTime.Now;
                objtbl_ApplicantTrans.IsActive = 1;
                objtbl_ApplicantTrans.Remark = "Selecting Institute Preference Details Updated";
                objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                objtbl_ApplicantTrans.CreatedBy = formCollection.UpdatedBy;
                _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);

                _db.SaveChanges();
                transaction.Complete();
            }

            return resData;
        }

        #endregion

        #region .. Document Upload ..

        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsDLL(ApplicantDocumentsDetail objApplicantApplicationForm)
        {
            //var resData = (dynamic)null;
            UploadPreferenceType output = new UploadPreferenceType();
            List<ApplicantDocumentsDetail> resData = new List<ApplicantDocumentsDetail>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicantId;
                    int DocAppId = objApplicantApplicationForm.DocAppId;

                    if (DocAppId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Document_Applicant
                                                   where tad.ApplicantId == ApplicationId && tad.DocumentTypeId == objApplicantApplicationForm.DocumentTypeId &&
                                                   tad.DocAppId == objApplicantApplicationForm.DocAppId
                                                   select tad.DocAppId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                        objtbl_Document_Applicant.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        objtbl_Document_Applicant.DocumentTypeId = objApplicantApplicationForm.DocumentTypeId;
                        if (objApplicantApplicationForm.FileName != null)
                        {
                            objtbl_Document_Applicant.FileName = objApplicantApplicationForm.FileName;
                            objtbl_Document_Applicant.FilePath = objApplicantApplicationForm.FilePath;
                        }
                        if (objApplicantApplicationForm.Verified != 0)
                        {
                            objtbl_Document_Applicant.Verified = objApplicantApplicationForm.Verified;
                        }
                        objtbl_Document_Applicant.CreatedOn = DateTime.Now;
                        objtbl_Document_Applicant.CreatedBy = objApplicantApplicationForm.CreatedBy;
                        objtbl_Document_Applicant.UpdatedBy = objApplicantApplicationForm.UpdatedBy;
                        if (objApplicantApplicationForm.DocumentRemarks != null)
                        {
                            objtbl_Document_Applicant.Remarks = objApplicantApplicationForm.DocumentRemarks;
                        }
                        objtbl_Document_Applicant.DocumentSet = 1;
                        _db.tbl_Document_Applicant.Add(objtbl_Document_Applicant);
                        _db.SaveChanges();

                        DocAppId = (from tad in _db.tbl_Document_Applicant
                                    orderby tad.CreatedOn descending
                                    select tad.DocAppId).Take(1).FirstOrDefault();

                    }
                    else
                    {
                        tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                        var update_query = _db.tbl_Document_Applicant.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId &&
                                        s.DocumentTypeId == objApplicantApplicationForm.DocumentTypeId && s.DocAppId == objApplicantApplicationForm.DocAppId).FirstOrDefault();

                        update_query.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        update_query.DocumentTypeId = objApplicantApplicationForm.DocumentTypeId;
                        if (objApplicantApplicationForm.FileName != null)
                        {
                            update_query.FileName = objApplicantApplicationForm.FileName;
                            update_query.FilePath = objApplicantApplicationForm.FilePath;
                        }

                        if (objApplicantApplicationForm.Verified != 0)
                        {
                            update_query.Verified = objApplicantApplicationForm.Verified;
                        }
                        update_query.CreatedBy = objApplicantApplicationForm.CreatedBy;
                        update_query.UpdatedBy = objApplicantApplicationForm.UpdatedBy;
                        update_query.DocumentSet = 1;
                        if (objApplicantApplicationForm.DocumentRemarks != null)
                        {
                            update_query.Remarks = objApplicantApplicationForm.DocumentRemarks;
                        }
                        _db.SaveChanges();
                    }

                    if (objApplicantApplicationForm.FileName != null)
                    {
                        int? ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.AssignedVO).Take(1).FirstOrDefault();

                        tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                        objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.UpdatedBy;
                        objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.Remarks;
                        if (objApplicantApplicationForm.Verified == 1)
                        {
                            objtbl_ApplicantTrans.Status = objApplicantApplicationForm.Verified;
                            objtbl_ApplicantTrans.DocumentWiseStatus = objApplicantApplicationForm.Verified;
                            objtbl_ApplicantTrans.ReVerficationStatus = true;
                        }
                        else
                        {
                            objtbl_ApplicantTrans.Status = 5;
                            objtbl_ApplicantTrans.DocumentWiseStatus = 5;
                            objtbl_ApplicantTrans.ReVerficationStatus = false;
                        }
                        objtbl_ApplicantTrans.TransDate = DateTime.Now;
                        objtbl_ApplicantTrans.IsActive = 1;
                        objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.DocumentRemarks;
                        objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                        objtbl_ApplicantTrans.DocumentName = objApplicantApplicationForm.DocumentTypeId;
                        objtbl_ApplicantTrans.DocumentWiseRemarks = objApplicantApplicationForm.DocumentRemarks;
                        objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.UpdatedBy;
                        objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.UpdatedBy;

                        _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                        _db.SaveChanges();
                    }

                    resData = (from a in _db.tbl_Document_Applicant
                               where a.ApplicantId == ApplicationId
                               select new ApplicantDocumentsDetail
                               {
                                   FileName = a.FileName,
                                   FilePath = a.FilePath,
                                   DocumentTypeId = a.DocumentTypeId,
                                   ApplicantId = a.ApplicantId,
                                   Verified = a.Verified,
                                   DocumentRemarks = a.Remarks

                               }).ToList();

                    transaction.Complete();
                    resData.Add(new ApplicantDocumentsDetail { ErrorOccuredMsg = "Proper Data Format", ErrorOccuredInd = 0, UpdateMsg = "success" });
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                resData.Clear();
                resData.Add(new ApplicantDocumentsDetail { ErrorOccuredMsg = "Error in Data Format", ErrorOccuredInd = 1, UpdateMsg = "failed" });
            }
            return resData;
        }

        public List<GrievanceDocApplData> ApplicantGrievanceDocumentDetailsDLL(GrievanceDocApplData objGrievanceDocApplData)
        {
            //var resData = (dynamic)null;
            UploadPreferenceType output = new UploadPreferenceType();
            List<GrievanceDocApplData> resData = new List<GrievanceDocApplData>();

            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objGrievanceDocApplData.ApplicantId;
                    int DocAppId = objGrievanceDocApplData.DocAppId;

                    if (objGrievanceDocApplData.FileName != null)
                    {
                        if (DocAppId != 0)
                        {
                            ExistingRecordForUpdate = (from tad in _db.tbl_Document_Applicant
                                                       where tad.ApplicantId == ApplicationId && tad.DocumentTypeId == objGrievanceDocApplData.DocumentTypeId &&
                                                       tad.DocAppId == objGrievanceDocApplData.DocAppId && tad.DocumentSet == 2
                                                       select tad.DocAppId).Take(1).FirstOrDefault();
                        }

                        if (ExistingRecordForUpdate == 0)
                        {
                            tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                            objtbl_Document_Applicant.ApplicantId = objGrievanceDocApplData.ApplicantId;
                            objtbl_Document_Applicant.DocumentTypeId = objGrievanceDocApplData.DocumentTypeId;
                            objtbl_Document_Applicant.FileName = objGrievanceDocApplData.FileName;
                            objtbl_Document_Applicant.FilePath = objGrievanceDocApplData.FilePath;
                            objtbl_Document_Applicant.Verified = objGrievanceDocApplData.Verified;
                            objtbl_Document_Applicant.CreatedOn = DateTime.Now;
                            objtbl_Document_Applicant.CreatedBy = objGrievanceDocApplData.CreatedBy;
                            objtbl_Document_Applicant.UpdatedBy = objGrievanceDocApplData.UpdatedBy;
                            objtbl_Document_Applicant.DocumentSet = 2;
                            if (objGrievanceDocApplData.DocumentRemarks != null)
                            {
                                objtbl_Document_Applicant.Remarks = objGrievanceDocApplData.DocumentRemarks;
                            }
                            _db.tbl_Document_Applicant.Add(objtbl_Document_Applicant);
                            _db.SaveChanges();

                            DocAppId = (from tad in _db.tbl_Document_Applicant
                                        orderby tad.CreatedOn descending
                                        select tad.DocAppId).Take(1).FirstOrDefault();

                        }
                        else
                        {
                            tbl_Document_Applicant objtbl_Document_Applicant = new tbl_Document_Applicant();
                            var update_query = _db.tbl_Document_Applicant.Where(s => s.ApplicantId == objGrievanceDocApplData.ApplicantId &&
                                            s.DocumentTypeId == objGrievanceDocApplData.DocumentTypeId && s.DocAppId == objGrievanceDocApplData.DocAppId).FirstOrDefault();

                            update_query.ApplicantId = objGrievanceDocApplData.ApplicantId;
                            update_query.DocumentTypeId = objGrievanceDocApplData.DocumentTypeId;
                            update_query.FileName = objGrievanceDocApplData.FileName;
                            update_query.FilePath = objGrievanceDocApplData.FilePath;
                            update_query.Verified = objGrievanceDocApplData.Verified;
                            update_query.CreatedBy = objGrievanceDocApplData.CreatedBy;
                            update_query.UpdatedBy = objGrievanceDocApplData.UpdatedBy;
                            update_query.DocumentSet = 2;
                            if (objGrievanceDocApplData.DocumentRemarks != null)
                            {
                                update_query.Remarks = objGrievanceDocApplData.DocumentRemarks;
                            }
                            _db.SaveChanges();
                        }
                        transaction.Complete();
                    }
                }

                resData = (from a in _db.tbl_Document_Applicant
                           where a.ApplicantId == objGrievanceDocApplData.ApplicantId
                           select new GrievanceDocApplData
                           {
                               ApplicantId = a.ApplicantId

                           }).ToList();

                resData.Add(new GrievanceDocApplData { ErrorOccuredMsg = "Proper Data Format", ErrorOccuredInd = 0, UpdateMsg = "success" });
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                resData.Clear();
                resData.Add(new GrievanceDocApplData { ErrorOccuredMsg = "Error in Data Format", ErrorOccuredInd = 1, UpdateMsg = "failed" });
            }
            return resData;
        }

        #endregion

        public List<ApplicantDocumentsDetail> ApplicantDocumentDetailsTransDLL(ApplicantDocumentsDetail objApplicantApplicationForm)
        {
            List<ApplicantDocumentsDetail> resData = new List<ApplicantDocumentsDetail>();

            if (objApplicantApplicationForm.FileName != null)
            {
                tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.UpdatedBy;
                if (objApplicantApplicationForm.Verified == 1)
                {
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.Verified;
                    objtbl_ApplicantTrans.DocumentWiseStatus = objApplicantApplicationForm.Verified;
                }
                else
                {
                    objtbl_ApplicantTrans.Status = 5;
                    objtbl_ApplicantTrans.DocumentWiseStatus = 5;
                }
                objtbl_ApplicantTrans.TransDate = DateTime.Now;
                objtbl_ApplicantTrans.IsActive = 1;
                objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.Remarks;
                objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.UpdatedBy;
                _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);

                _db.SaveChanges();
            }

            resData = (from a in _db.tbl_Document_Applicant
                       where a.ApplicantId == objApplicantApplicationForm.ApplicantId
                       select new ApplicantDocumentsDetail
                       {
                           FileName = a.FileName,
                           FilePath = a.FilePath,
                           DocumentTypeId = a.DocumentTypeId,
                           ApplicantId = a.ApplicantId,
                           Verified = a.Verified,
                           DocumentRemarks = a.Remarks

                       }).ToList();

            resData.Add(new ApplicantDocumentsDetail { ErrorOccuredMsg = "Proper Data Format", ErrorOccuredInd = 0, UpdateMsg = "success" });

            return resData;
        }

        public List<ApplicantDocumentsDetail> UpdateApplicantFormDetailsDLL(ApplicantDocumentsDetail objApplicantDocumentsDetail)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantDocumentsDetail.ApplicantId && s.IsActive == true).FirstOrDefault();
                    update_query.PhysicallyHanidcapInd = objApplicantDocumentsDetail.PhysicallyHanidcapInd;
                    update_query.PhysicallyHanidcapType = objApplicantDocumentsDetail.PhysicallyHanidcapType;
                    update_query.HoraNadu_GadiNadu_Kannidagas = objApplicantDocumentsDetail.HoraNadu_GadiNadu_Kannidagas;
                    update_query.HyderabadKarnatakaRegion = objApplicantDocumentsDetail.HyderabadKarnatakaRegion;
                    update_query.ExemptedFromStudyCertificate = objApplicantDocumentsDetail.ExemptedFromStudyCertificate;
                    update_query.KanndaMedium = objApplicantDocumentsDetail.KanndaMedium;
                    update_query.ApplicantBelongTo = objApplicantDocumentsDetail.ApplicantBelongTo;
                    _db.SaveChanges();

                    tbl_Applicant_Reservation objtbl_Applicant_Reservation = new tbl_Applicant_Reservation();
                    if (objApplicantDocumentsDetail.ApplicableReservations != null)
                    {
                        var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantDocumentsDetail.ApplicantId && s.IsActive == true).ToList();
                        foreach (var item in remove_query)
                        {
                            _db.tbl_Applicant_Reservation.Remove(item);
                        }
                        foreach (int ReservationApp in objApplicantDocumentsDetail.ApplicableReservations)
                        {
                            objtbl_Applicant_Reservation.ApplicantId = objApplicantDocumentsDetail.ApplicantId;
                            objtbl_Applicant_Reservation.ReservationId = ReservationApp;
                            objtbl_Applicant_Reservation.CreatedBy = objApplicantDocumentsDetail.CreatedBy;
                            objtbl_Applicant_Reservation.IsActive = true;
                            objtbl_Applicant_Reservation.CreatedOn = DateTime.Now;
                            _db.tbl_Applicant_Reservation.Add(objtbl_Applicant_Reservation);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (objApplicantDocumentsDetail.ExserDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantDocumentsDetail.ApplicantId && s.ReservationId == 2 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        if (objApplicantDocumentsDetail.EWSDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantDocumentsDetail.ApplicantId && s.ReservationId == 5 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        _db.SaveChanges();
                    }
                    transaction.Complete();
                }

                resData = (from a in _db.tbl_Applicant_Detail
                           where a.ApplicationId == objApplicantDocumentsDetail.ApplicantId && a.IsActive == true
                           select new ApplicantDocumentsDetail
                           {
                               ApplicantId = a.ApplicationId
                           }).ToList();
            }
            catch (Exception ex)
            {

            }
            return resData;
        }

        public List<ApplicantApplicationForm> InsertApplicantFormDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicationId;
                    if (ApplicationId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.ApplicationId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Appicant Details
                        objtbl_Applicant_Detail.RStateBoardType = objApplicantApplicationForm.RStateBoardType;
                        objtbl_Applicant_Detail.RAppBasics = objApplicantApplicationForm.RAppBasics;
                        objtbl_Applicant_Detail.RollNumber = objApplicantApplicationForm.RollNumber;
                        objtbl_Applicant_Detail.ApplyMonth = objApplicantApplicationForm.ApplyMonth;
                        objtbl_Applicant_Detail.ApplyYear = objApplicantApplicationForm.ApplyYear;
                        objtbl_Applicant_Detail.ApplicantName = objApplicantApplicationForm.ApplicantName;
                        objtbl_Applicant_Detail.FathersName = objApplicantApplicationForm.FathersName;
                        objtbl_Applicant_Detail.ParentsOccupation = objApplicantApplicationForm.ParentsOccupation;
                        objtbl_Applicant_Detail.DOB = objApplicantApplicationForm.DOB;
                        if (objApplicantApplicationForm.PhotoFile != null)
                            objtbl_Applicant_Detail.Photo = objApplicantApplicationForm.FilePath;
                        objtbl_Applicant_Detail.Gender = objApplicantApplicationForm.Gender;
                        objtbl_Applicant_Detail.MothersName = objApplicantApplicationForm.MothersName;
                        objtbl_Applicant_Detail.Religion = objApplicantApplicationForm.Religion;
                        objtbl_Applicant_Detail.Category = objApplicantApplicationForm.Category;
                        objtbl_Applicant_Detail.MinorityCategory = objApplicantApplicationForm.MinorityCategory;
                        objtbl_Applicant_Detail.Caste = objApplicantApplicationForm.Caste;
                        objtbl_Applicant_Detail.CasteDetail = objApplicantApplicationForm.CasteDetail;
                        objtbl_Applicant_Detail.FamilyAnnIncome = Convert.ToDecimal(objApplicantApplicationForm.FamilyAnnIncome);
                        objtbl_Applicant_Detail.ApplicantType = objApplicantApplicationForm.ApplicantType;
                        objtbl_Applicant_Detail.RationCard = objApplicantApplicationForm.RationCard;
                        objtbl_Applicant_Detail.AadhaarNumber = objApplicantApplicationForm.AadhaarNumber;
                        objtbl_Applicant_Detail.AccountNumber = objApplicantApplicationForm.AccountNumber;
                        objtbl_Applicant_Detail.BankName = objApplicantApplicationForm.BankName;
                        objtbl_Applicant_Detail.IFSCCode = objApplicantApplicationForm.IFSCCode;
                        objtbl_Applicant_Detail.PhysicallyHanidcapInd = objApplicantApplicationForm.PhysicallyHanidcapInd;
                        objtbl_Applicant_Detail.PhysicallyHanidcapType = objApplicantApplicationForm.PhysicallyHanidcapType;
                        objtbl_Applicant_Detail.HoraNadu_GadiNadu_Kannidagas = objApplicantApplicationForm.HoraNadu_GadiNadu_Kannidagas;
                        objtbl_Applicant_Detail.HyderabadKarnatakaRegion = objApplicantApplicationForm.HyderabadKarnatakaRegion;
                        objtbl_Applicant_Detail.ExemptedFromStudyCertificate = objApplicantApplicationForm.ExemptedFromStudyCertificate;
                        objtbl_Applicant_Detail.KanndaMedium = objApplicantApplicationForm.KanndaMedium;
                        objtbl_Applicant_Detail.CreatedOn = DateTime.Now;
                        objtbl_Applicant_Detail.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.FlowId = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.ApplStatus = objApplicantApplicationForm.ApplStatus;
                        objtbl_Applicant_Detail.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        objtbl_Applicant_Detail.IsActive = objApplicantApplicationForm.IsActive;
                        objtbl_Applicant_Detail.EconomyWeakerSection = objApplicantApplicationForm.EconomyWeakerSection;
                        objtbl_Applicant_Detail.ExServiceMan = objApplicantApplicationForm.ExServiceMan;
                        objtbl_Applicant_Detail.AgainstVacancyInd = objApplicantApplicationForm.AgainstVacancyInd;
                        objtbl_Applicant_Detail.CategoryName = objApplicantApplicationForm.CategoryName;
                        //objtbl_Applicant_Detail.EconomyWeakerSection = objApplicantApplicationForm.Ewsection;

						objtbl_Applicant_Detail.ApplicationMode = objApplicantApplicationForm.ApplicationMode;
						objtbl_Applicant_Detail.Caste_Categaory_Income_RD = objApplicantApplicationForm.Caste_RD_No;
						objtbl_Applicant_Detail.Economic_Weaker_Section_RD = objApplicantApplicationForm.EconomicWeaker_RD_No;
						objtbl_Applicant_Detail.Hyderabada_Karnataka_Region_RD = objApplicantApplicationForm.HYD_Karnataka_RD_No;
						objtbl_Applicant_Detail.UDID_Number = objApplicantApplicationForm.UDID_No;
						objtbl_Applicant_Detail.ApplDescStatus = 1; //Application Submitted

						_db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
						_db.SaveChanges();

                        ApplicationId = (from tad in _db.tbl_Applicant_Detail
                                         where tad.IsActive == true
                                         orderby tad.CreatedOn descending
                                         select tad.ApplicationId).Take(1).FirstOrDefault();

                        objApplicantApplicationForm.ApplicationId = ApplicationId;
                    }
                    else
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicationId && s.IsActive == true).FirstOrDefault();

						//Appicant Details
						update_query.ApplyMonth = objApplicantApplicationForm.ApplyMonth;
						update_query.ApplyYear = objApplicantApplicationForm.ApplyYear;
						update_query.RStateBoardType = objApplicantApplicationForm.RStateBoardType;
						update_query.RAppBasics = objApplicantApplicationForm.RAppBasics;
						update_query.RollNumber = objApplicantApplicationForm.RollNumber;
						update_query.ApplicantName = objApplicantApplicationForm.ApplicantName;
						update_query.FathersName = objApplicantApplicationForm.FathersName;
						update_query.ParentsOccupation = objApplicantApplicationForm.ParentsOccupation;
						if (objApplicantApplicationForm.PhotoFile != null)
							update_query.Photo = objApplicantApplicationForm.FilePath;
						update_query.DOB = Convert.ToDateTime(objApplicantApplicationForm.DOB);
						update_query.Gender = objApplicantApplicationForm.Gender;
						update_query.MothersName = objApplicantApplicationForm.MothersName;
						update_query.Religion = objApplicantApplicationForm.Religion;
						update_query.Category = objApplicantApplicationForm.Category;
						update_query.MinorityCategory = objApplicantApplicationForm.MinorityCategory;
						update_query.Caste = objApplicantApplicationForm.Caste;
						update_query.CasteDetail = objApplicantApplicationForm.CasteDetail;
						update_query.FamilyAnnIncome = Convert.ToDecimal(objApplicantApplicationForm.FamilyAnnIncome);
						update_query.ApplicantType = objApplicantApplicationForm.ApplicantType;
						//update_query.RationCard = objApplicantApplicationForm.RationCard;
						//update_query.AadhaarNumber = objApplicantApplicationForm.AadhaarNumber;
						update_query.AccountNumber = objApplicantApplicationForm.AccountNumber;
						update_query.BankName = objApplicantApplicationForm.BankName;
						update_query.IFSCCode = objApplicantApplicationForm.IFSCCode;
						update_query.PhysicallyHanidcapInd = objApplicantApplicationForm.PhysicallyHanidcapInd;
						update_query.PhysicallyHanidcapType = objApplicantApplicationForm.PhysicallyHanidcapType;
						update_query.HoraNadu_GadiNadu_Kannidagas = objApplicantApplicationForm.HoraNadu_GadiNadu_Kannidagas;
						update_query.HyderabadKarnatakaRegion = objApplicantApplicationForm.HyderabadKarnatakaRegion;
						update_query.ExemptedFromStudyCertificate = objApplicantApplicationForm.ExemptedFromStudyCertificate;
						update_query.KanndaMedium = objApplicantApplicationForm.KanndaMedium;
						update_query.ExServiceMan = objApplicantApplicationForm.ExServiceMan;
						update_query.EconomyWeakerSection = objApplicantApplicationForm.EconomyWeakerSection;
						update_query.AgainstVacancyInd = objApplicantApplicationForm.AgainstVacancyInd;
						update_query.CategoryName = objApplicantApplicationForm.CategoryName;
						//update_query.EconomyWeakerSection = objApplicantApplicationForm.Ewsection;

						update_query.ApplicationMode = objApplicantApplicationForm.ApplicationMode;
                        update_query.Caste_Categaory_Income_RD = objApplicantApplicationForm.Caste_RD_No;
                        update_query.Economic_Weaker_Section_RD = objApplicantApplicationForm.EconomicWeaker_RD_No;
                        update_query.Hyderabada_Karnataka_Region_RD = objApplicantApplicationForm.HYD_Karnataka_RD_No;
                        update_query.UDID_Number = objApplicantApplicationForm.UDID_No;

                        _db.SaveChanges();

                        //var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicationId && s.IsActive == true).ToList();
                        //foreach (var item in remove_query)
                        //{
                        //    _db.tbl_Applicant_Reservation.Remove(item);
                        //}
                        //_db.SaveChanges();
                    }

                    //tbl_Applicant_Reservation objtbl_Applicant_Reservation = new tbl_Applicant_Reservation();
                    //if (objApplicantApplicationForm.ApplicableReservations != null)
                    //{
                    //    foreach (int ReservationApp in objApplicantApplicationForm.ApplicableReservations)
                    //    {
                    //        objtbl_Applicant_Reservation.ApplicantId = objApplicantApplicationForm.ApplicationId;
                    //        objtbl_Applicant_Reservation.ReservationId = ReservationApp;
                    //        objtbl_Applicant_Reservation.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    //        objtbl_Applicant_Reservation.IsActive = true;
                    //        objtbl_Applicant_Reservation.CreatedOn = DateTime.Now;
                    //        _db.tbl_Applicant_Reservation.Add(objtbl_Applicant_Reservation);
                    //        _db.SaveChanges();
                    //    }
                    //}

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicationId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Application Basic Details by Applicant";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new ApplicantApplicationForm
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplyMonth = a.ApplyMonth,
                                   ApplyYear = a.ApplyYear,
                                   ApplicantName = a.ApplicantName,
                                   RationCard = a.RationCard,
                                   CredatedBy = a.CredatedBy,
                                   AadhaarNumber = a.AadhaarNumber,
                                   AccountNumber = a.AccountNumber,
                                   BankName = a.BankName,
                                   IFSCCode = a.IFSCCode,
                                   PhysicallyHanidcapType = a.PhysicallyHanidcapType,
                                   PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
                                   FathersName = a.FathersName,
                                   ParentsOccupation = a.ParentsOccupation,
                                   Photo = a.Photo,
                                   DOB = a.DOB,
                                   Gender = a.Gender,
                                   Percentage = a.Percentage,
                                   MothersName = a.MothersName,
                                   Religion = a.Religion,
                                   Category = a.Category,
                                   MinorityCategory = a.MinorityCategory,
                                   Caste = a.Caste,
                                   FamilyAnnIncome = a.FamilyAnnIncome,
                                   studiedMathsScience = a.studiedMathsScience,
                                   ApplicantType = a.ApplicantType,
                                   Qualification = a.Qualification,
                                   ApplicantBelongTo = a.ApplicantBelongTo,
                                   AppliedBasic = a.AppliedBasic,
                                   TenthBoard = a.TenthBoard,
                                   InstituteStudiedQual = a.InstituteStudiedQual,
                                   MaxMarks = a.MaxMarks,
                                   MinMarks = a.MinMarks,
                                   ResultQual = a.ResultQual,
                                   CommunicationAddress = a.CommunicationAddress,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId,
                                   Pincode = a.Pincode,
                                   SameAdd = a.SameAdd,
                                   PermanentAddress = a.PermanentAddress,
                                   PDistrict = a.PDistrict,
                                   PTaluk = a.PTaluk,
                                   PPinCode = a.PPinCode,
                                   PhoneNumber = a.PhoneNumber,
                                   FatherPhoneNumber = a.FatherPhoneNumber,
                                   EmailId = a.EmailId,
                                   DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                                   DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                                   DocVerificationCentre = a.DocVerificationCentre,
                                   ParticipateNextRound = a.ParticipateNextRound,
                                   FlowId = a.FlowId,
                                   AssignedVO = a.AssignedVO,
                                   EconomyWeakerSection = a.EconomyWeakerSection,
                                   ExServiceMan = a.ExServiceMan

                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return resData;
            }

            return resData;
        }

        public List<ApplicantApplicationForm> SaveEducationDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicationId;
                    if (ApplicationId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.ApplicationId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Education
                        objtbl_Applicant_Detail.Qualification = objApplicantApplicationForm.Qualification;
                        objtbl_Applicant_Detail.ApplicantBelongTo = objApplicantApplicationForm.ApplicantBelongTo;
                        objtbl_Applicant_Detail.AppliedBasic = objApplicantApplicationForm.AppliedBasic;
                        objtbl_Applicant_Detail.TenthBoard = objApplicantApplicationForm.TenthBoard;
                        objtbl_Applicant_Detail.InstituteStudiedQual = objApplicantApplicationForm.InstituteStudiedQual;
                        objtbl_Applicant_Detail.MaxMarks = Convert.ToDecimal(objApplicantApplicationForm.MaxMarks);
                        objtbl_Applicant_Detail.MinMarks = Convert.ToDecimal(objApplicantApplicationForm.MinMarks);
                        objtbl_Applicant_Detail.MarksObtained = Convert.ToDecimal(objApplicantApplicationForm.MarksObtained);
                        objtbl_Applicant_Detail.Percentage = Convert.ToDecimal(objApplicantApplicationForm.Percentage);
                        objtbl_Applicant_Detail.ResultQual = objApplicantApplicationForm.ResultQual;
                        objtbl_Applicant_Detail.TenthCOBSEBoard = objApplicantApplicationForm.TenthCOBSEBoard;
                        objtbl_Applicant_Detail.EducationGrade = objApplicantApplicationForm.EducationGrade;
                        objtbl_Applicant_Detail.studiedMathsScience = objApplicantApplicationForm.studiedMathsScience;
                        objtbl_Applicant_Detail.CreatedOn = DateTime.Now;
                        objtbl_Applicant_Detail.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.FlowId = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.ApplStatus = objApplicantApplicationForm.ApplStatus;
                        objtbl_Applicant_Detail.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        objtbl_Applicant_Detail.IsActive = objApplicantApplicationForm.IsActive;
                        _db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
                        _db.SaveChanges();

                        ApplicationId = (from tad in _db.tbl_Applicant_Detail
                                         where tad.IsActive == true
                                         orderby tad.CreatedOn descending
                                         select tad.ApplicationId).Take(1).FirstOrDefault();

                        objApplicantApplicationForm.ApplicationId = ApplicationId;
                    }
                    else
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicationId && s.IsActive == true).FirstOrDefault();

                        //Education
                        update_query.Qualification = objApplicantApplicationForm.Qualification;
                        update_query.ApplicantBelongTo = objApplicantApplicationForm.ApplicantBelongTo;
                        update_query.AppliedBasic = objApplicantApplicationForm.AppliedBasic;
                        update_query.TenthBoard = objApplicantApplicationForm.TenthBoard;
                        update_query.BoardId = (int)objApplicantApplicationForm.BoardId;
                        update_query.InstituteStudiedQual = objApplicantApplicationForm.InstituteStudiedQual;
                        update_query.MaxMarks = objApplicantApplicationForm.MaxMarks;
                        update_query.MinMarks = objApplicantApplicationForm.MinMarks;
                        update_query.MarksObtained = Convert.ToDecimal(objApplicantApplicationForm.MarksObtained);
                        update_query.Percentage = Convert.ToDecimal(objApplicantApplicationForm.Percentage);
                        update_query.ResultQual = objApplicantApplicationForm.ResultQual;
                        update_query.TenthCOBSEBoard = objApplicantApplicationForm.TenthCOBSEBoard;
                        update_query.studiedMathsScience = objApplicantApplicationForm.studiedMathsScience;
                        update_query.EducationCGPA = Convert.ToDecimal(objApplicantApplicationForm.EducationCGPA);
                        update_query.EducationGrade = objApplicantApplicationForm.EducationGrade;

                        _db.SaveChanges();
                    }

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicationId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Education Details by Applicant";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new ApplicantApplicationForm
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplyMonth = a.ApplyMonth,
                                   ApplyYear = a.ApplyYear,
                                   ApplicantName = a.ApplicantName,
                                   RationCard = a.RationCard,
                                   AadhaarNumber = a.AadhaarNumber,
                                   AccountNumber = a.AccountNumber,
                                   BankName = a.BankName,
                                   IFSCCode = a.IFSCCode,
                                   PhysicallyHanidcapType = a.PhysicallyHanidcapType,
                                   PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
                                   FathersName = a.FathersName,
                                   ParentsOccupation = a.ParentsOccupation,
                                   Photo = a.Photo,
                                   DOB = a.DOB,
                                   Gender = a.Gender,
                                   Percentage = a.Percentage,
                                   MothersName = a.MothersName,
                                   Religion = a.Religion,
                                   Category = a.Category,
                                   MinorityCategory = a.MinorityCategory,
                                   Caste = a.Caste,
                                   FamilyAnnIncome = a.FamilyAnnIncome,
                                   ApplicantType = a.ApplicantType,
                                   Qualification = a.Qualification,
                                   ApplicantBelongTo = a.ApplicantBelongTo,
                                   AppliedBasic = a.AppliedBasic,
                                   TenthBoard = a.TenthBoard,
                                   InstituteStudiedQual = a.InstituteStudiedQual,
                                   MaxMarks = a.MaxMarks,
                                   MinMarks = a.MinMarks,
                                   studiedMathsScience = a.studiedMathsScience,
                                   ResultQual = a.ResultQual,
                                   CommunicationAddress = a.CommunicationAddress,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId,
                                   Pincode = a.Pincode,
                                   SameAdd = a.SameAdd,
                                   PermanentAddress = a.PermanentAddress,
                                   PDistrict = a.PDistrict,
                                   PTaluk = a.PTaluk,
                                   PPinCode = a.PPinCode,
                                   PhoneNumber = a.PhoneNumber,
                                   FatherPhoneNumber = a.FatherPhoneNumber,
                                   EmailId = a.EmailId,
                                   DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                                   DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                                   DocVerificationCentre = a.DocVerificationCentre,
                                   ParticipateNextRound = a.ParticipateNextRound,
                                   FlowId = a.FlowId,
                                   AssignedVO = a.AssignedVO

                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return resData;
            }

            return resData;
        }

        public List<ApplicantApplicationForm> SaveAddressDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicationId;
                    if (ApplicationId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.ApplicationId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Address
                        objtbl_Applicant_Detail.CommunicationAddress = objApplicantApplicationForm.CommunicationAddress;
                        objtbl_Applicant_Detail.DistrictId = objApplicantApplicationForm.DistrictId;
                        objtbl_Applicant_Detail.TalukaId = objApplicantApplicationForm.TalukaId;
                        objtbl_Applicant_Detail.Pincode = objApplicantApplicationForm.Pincode;
                        objtbl_Applicant_Detail.PermanentAddress = objApplicantApplicationForm.PermanentAddress;
                        objtbl_Applicant_Detail.SameAdd = objApplicantApplicationForm.SameAdd;
                        objtbl_Applicant_Detail.PDistrict = objApplicantApplicationForm.PermanentDistricts;
                        objtbl_Applicant_Detail.PTaluk = objApplicantApplicationForm.PTaluk;
                        objtbl_Applicant_Detail.PPinCode = objApplicantApplicationForm.PPinCode;
                        objtbl_Applicant_Detail.PhoneNumber = objApplicantApplicationForm.ApplicantPhoneNumber;
                        objtbl_Applicant_Detail.FatherPhoneNumber = objApplicantApplicationForm.FathersPhoneNumber;
                        objtbl_Applicant_Detail.EmailId = objApplicantApplicationForm.EmailId;
                        objtbl_Applicant_Detail.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.FlowId = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.ApplStatus = objApplicantApplicationForm.ApplStatus;
                        objtbl_Applicant_Detail.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        objtbl_Applicant_Detail.IsActive = objApplicantApplicationForm.IsActive;
                        _db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
                        _db.SaveChanges();

                        ApplicationId = (from tad in _db.tbl_Applicant_Detail
                                         where tad.IsActive == true
                                         orderby tad.CreatedOn descending
                                         select tad.ApplicationId).Take(1).FirstOrDefault();

                        objApplicantApplicationForm.ApplicationId = ApplicationId;
                    }
                    else
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicationId && s.IsActive == true).FirstOrDefault();

                        //Address
                        update_query.CommunicationAddress = objApplicantApplicationForm.CommunicationAddress;
                        update_query.DistrictId = objApplicantApplicationForm.DistrictId;
                        update_query.TalukaId = objApplicantApplicationForm.TalukaId;
                        update_query.Pincode = objApplicantApplicationForm.Pincode;
                        update_query.PermanentAddress = objApplicantApplicationForm.PermanentAddress;
                        update_query.SameAdd = objApplicantApplicationForm.SameAdd;
                        update_query.PDistrict = objApplicantApplicationForm.PermanentDistricts;
                        update_query.PTaluk = objApplicantApplicationForm.PTaluk;
                        update_query.PPinCode = objApplicantApplicationForm.PPinCode;
                        update_query.PhoneNumber = objApplicantApplicationForm.ApplicantPhoneNumber;
                        update_query.FatherPhoneNumber = objApplicantApplicationForm.FathersPhoneNumber;
                        update_query.EmailId = objApplicantApplicationForm.EmailId;

                        _db.SaveChanges();
                    }

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicationId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Address Details by Applicant";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new ApplicantApplicationForm
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplyMonth = a.ApplyMonth,
                                   ApplyYear = a.ApplyYear,
                                   ApplicantName = a.ApplicantName,
                                   RationCard = a.RationCard,
                                   AadhaarNumber = a.AadhaarNumber,
                                   AccountNumber = a.AccountNumber,
                                   BankName = a.BankName,
                                   IFSCCode = a.IFSCCode,
                                   PhysicallyHanidcapType = a.PhysicallyHanidcapType,
                                   PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
                                   FathersName = a.FathersName,
                                   ParentsOccupation = a.ParentsOccupation,
                                   Photo = a.Photo,
                                   DOB = a.DOB,
                                   Gender = a.Gender,
                                   Percentage = a.Percentage,
                                   MothersName = a.MothersName,
                                   Religion = a.Religion,
                                   Category = a.Category,
                                   studiedMathsScience = a.studiedMathsScience,
                                   MinorityCategory = a.MinorityCategory,
                                   Caste = a.Caste,
                                   FamilyAnnIncome = a.FamilyAnnIncome,
                                   ApplicantType = a.ApplicantType,
                                   Qualification = a.Qualification,
                                   ApplicantBelongTo = a.ApplicantBelongTo,
                                   AppliedBasic = a.AppliedBasic,
                                   TenthBoard = a.TenthBoard,
                                   InstituteStudiedQual = a.InstituteStudiedQual,
                                   MaxMarks = a.MaxMarks,
                                   MinMarks = a.MinMarks,
                                   ResultQual = a.ResultQual,
                                   CommunicationAddress = a.CommunicationAddress,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId,
                                   Pincode = a.Pincode,
                                   SameAdd = a.SameAdd,
                                   PermanentAddress = a.PermanentAddress,
                                   PDistrict = a.PDistrict,
                                   PTaluk = a.PTaluk,
                                   PPinCode = a.PPinCode,
                                   PhoneNumber = a.PhoneNumber,
                                   FatherPhoneNumber = a.FatherPhoneNumber,
                                   EmailId = a.EmailId,
                                   DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                                   DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                                   DocVerificationCentre = a.DocVerificationCentre,
                                   ParticipateNextRound = a.ParticipateNextRound,
                                   FlowId = a.FlowId,
                                   AssignedVO = a.AssignedVO

                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return resData;
            }

            return resData;
        }

        public List<ApplicantApplicationForm> SaveInstitueDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ExistingRecordForUpdate = 0;
                    int ApplicationId = objApplicantApplicationForm.ApplicationId;
                    if (ApplicationId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.ApplicationId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Institute Preference Centre
                        objtbl_Applicant_Detail.DocVeriInstituteFilter = objApplicantApplicationForm.DocVeriInstituteFilter;
                        objtbl_Applicant_Detail.DocVeriInstituteDistrict = objApplicantApplicationForm.DocVeriInstituteDistrict;
                        objtbl_Applicant_Detail.DocVerificationCentre = objApplicantApplicationForm.DocVerificationCentre;
                        objtbl_Applicant_Detail.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.FlowId = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.ApplStatus = objApplicantApplicationForm.ApplStatus;
                        objtbl_Applicant_Detail.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        objtbl_Applicant_Detail.IsActive = objApplicantApplicationForm.IsActive;
                        _db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
                        _db.SaveChanges();

                        ApplicationId = (from tad in _db.tbl_Applicant_Detail
                                         where tad.IsActive == true
                                         orderby tad.CreatedOn descending
                                         select tad.ApplicationId).Take(1).FirstOrDefault();

                        objApplicantApplicationForm.ApplicationId = ApplicationId;
                    }
                    else
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicationId && s.IsActive == true).FirstOrDefault();

                        //Institute Preference Centre
                        update_query.DocVeriInstituteFilter = objApplicantApplicationForm.DocVeriInstituteFilter;
                        update_query.DocVeriInstituteDistrict = objApplicantApplicationForm.DocVeriInstituteDistrict;
                        update_query.DocVerificationCentre = objApplicantApplicationForm.DocVerificationCentre;
                        update_query.ParticipateNextRound = objApplicantApplicationForm.ParticipateNextRound;

                        _db.SaveChanges();
                    }

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicationId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Document Verification Center Details by Applicant";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new ApplicantApplicationForm
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplyMonth = a.ApplyMonth,
                                   ApplyYear = a.ApplyYear,
                                   ApplicantName = a.ApplicantName,
                                   RationCard = a.RationCard,
                                   AadhaarNumber = a.AadhaarNumber,
                                   AccountNumber = a.AccountNumber,
                                   BankName = a.BankName,
                                   IFSCCode = a.IFSCCode,
                                   PhysicallyHanidcapType = a.PhysicallyHanidcapType,
                                   PhysicallyHanidcapInd = a.PhysicallyHanidcapInd,
                                   FathersName = a.FathersName,
                                   ParentsOccupation = a.ParentsOccupation,
                                   Photo = a.Photo,
                                   DOB = a.DOB,
                                   Gender = a.Gender,
                                   Percentage = a.Percentage,
                                   MothersName = a.MothersName,
                                   Religion = a.Religion,
                                   Category = a.Category,
                                   MinorityCategory = a.MinorityCategory,
                                   Caste = a.Caste,
                                   FamilyAnnIncome = a.FamilyAnnIncome,
                                   ApplicantType = a.ApplicantType,
                                   Qualification = a.Qualification,
                                   ApplicantBelongTo = a.ApplicantBelongTo,
                                   AppliedBasic = a.AppliedBasic,
                                   studiedMathsScience = a.studiedMathsScience,
                                   TenthBoard = a.TenthBoard,
                                   InstituteStudiedQual = a.InstituteStudiedQual,
                                   MaxMarks = a.MaxMarks,
                                   MinMarks = a.MinMarks,
                                   ResultQual = a.ResultQual,
                                   CommunicationAddress = a.CommunicationAddress,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId,
                                   Pincode = a.Pincode,
                                   SameAdd = a.SameAdd,
                                   PermanentAddress = a.PermanentAddress,
                                   PDistrict = a.PDistrict,
                                   PTaluk = a.PTaluk,
                                   PPinCode = a.PPinCode,
                                   PhoneNumber = a.PhoneNumber,
                                   FatherPhoneNumber = a.FatherPhoneNumber,
                                   EmailId = a.EmailId,
                                   DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                                   DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                                   DocVerificationCentre = a.DocVerificationCentre,
                                   ParticipateNextRound = a.ParticipateNextRound,
                                   FlowId = a.FlowId,
                                   AssignedVO = a.AssignedVO

                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return resData;
            }

            return resData;
        }

        public List<ApplicantApplicationForm> InsertPaymentDetailsDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            var resData = (dynamic)null;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var rcpt = "";
                    objApplicantApplicationForm.ApplicationId = _db.tbl_Applicant_Detail.Where(a => a.ApplicantNumber == objApplicantApplicationForm.ApplicantNumber).Select(c => c.ApplicationId).FirstOrDefault();
                    //objApplicantApplicationForm.ApplicantNumber=
                    int ExistingRecordForUpdate = 0;
                    var dat = DateTime.Now;
                    int ApplicationId = objApplicantApplicationForm.ApplicantId;
                    if (ApplicationId != 0)
                    {
                        ExistingRecordForUpdate = (from tad in _db.tbl_Applicant_Detail
                                                   where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                                   select tad.ApplicationId).Take(1).FirstOrDefault();
                    }

                    if (ExistingRecordForUpdate == 0)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Document Fees Payment details
                        objtbl_Applicant_Detail.IsActive = objApplicantApplicationForm.IsActive;
                        objtbl_Applicant_Detail.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.FlowId = objApplicantApplicationForm.CredatedBy;
                        objtbl_Applicant_Detail.ApplStatus = 3;//objApplicantApplicationForm.ApplStatus;
                        objtbl_Applicant_Detail.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        objtbl_Applicant_Detail.PaymentOptionval = objApplicantApplicationForm.PaymentOptionval;

                        InstituteWiseAdmission ins = new InstituteWiseAdmission();
                        ins.CreatedBy = objApplicantApplicationForm.CredatedBy;
                        InstituteAdmission.InstituteAdmission ia = new InstituteAdmission.InstituteAdmission();
                        objtbl_Applicant_Detail.DocumentFeeReceiptDetails = ia.GetReceiptNumberGen(ins, 0, 0);//objApplicantApplicationForm.DocumentFeeReceiptDetails;

                        if (objApplicantApplicationForm.PaymentOptionval == true)
                            objtbl_Applicant_Detail.ApplDescStatus = 1;
                        else if (objApplicantApplicationForm.PaymentOptionval == false && (objApplicantApplicationForm.DocumentFeeReceiptDetails == null || objApplicantApplicationForm.DocumentFeeReceiptDetails == ""))
                            objtbl_Applicant_Detail.ApplDescStatus = 2;
                        else if (objApplicantApplicationForm.PaymentOptionval == false && (objApplicantApplicationForm.DocumentFeeReceiptDetails != null || objApplicantApplicationForm.DocumentFeeReceiptDetails != ""))
                            objtbl_Applicant_Detail.ApplDescStatus = 3;

                        _db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
                        _db.SaveChanges();

                        ApplicationId = (from tad in _db.tbl_Applicant_Detail
                                         where tad.IsActive == true
                                         orderby tad.CreatedOn descending
                                         select tad.ApplicationId).Take(1).FirstOrDefault();

                        objApplicantApplicationForm.ApplicationId = ApplicationId;
                    }
                    else
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                        var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicantId && s.IsActive == true).FirstOrDefault();

                        //Document Fees Payment details
                        update_query.IsActive = objApplicantApplicationForm.IsActive;
                        update_query.CredatedBy = objApplicantApplicationForm.CredatedBy;
                        update_query.ApplStatus = 3;//objApplicantApplicationForm.ApplStatus;
                        update_query.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        update_query.PaymentOptionval = objApplicantApplicationForm.PaymentOptionval;

                        InstituteWiseAdmission ins = new InstituteWiseAdmission();
                        ins.CreatedBy = objApplicantApplicationForm.CredatedBy;
                        InstituteAdmission.InstituteAdmission ia = new InstituteAdmission.InstituteAdmission();
                        update_query.DocumentFeeReceiptDetails = ia.GetReceiptNumberGen(ins, 0, 0); //objApplicantApplicationForm.DocumentFeeReceiptDetails;

                        if (objApplicantApplicationForm.PaymentOptionval == true)
                            update_query.ApplDescStatus = 1;
                        else if (objApplicantApplicationForm.PaymentOptionval == false && (update_query.DocumentFeeReceiptDetails == null || update_query.DocumentFeeReceiptDetails == ""))
                            update_query.ApplDescStatus = 2;
                        else if (objApplicantApplicationForm.PaymentOptionval == false && (update_query.DocumentFeeReceiptDetails != null || update_query.DocumentFeeReceiptDetails != ""))
                            update_query.ApplDescStatus = 3;
                        rcpt = update_query.DocumentFeeReceiptDetails;
                        _db.SaveChanges();
                    }

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == ApplicationId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.FlowId = objApplicantApplicationForm.CredatedBy;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = "Updated Payment of Document Verification fee Details by Applicant";
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    tbl_VerOfficer_Applicant_Mapping objtbl_Fee_Detail = new tbl_VerOfficer_Applicant_Mapping();
                    var update_feedetails = _db.tbl_VerOfficer_Applicant_Mapping.Where(s => s.ApplicantId == objApplicantApplicationForm.ApplicantId).FirstOrDefault();
                    if (update_feedetails != null)
                    {
                        //Applicant Fees Payment details
                        update_feedetails.DocVeriFee = objApplicantApplicationForm.DocVeriFee;
                        update_feedetails.DocVeriFeePymtDate = DateTime.Now;
                        update_feedetails.DocVeriFeeReceiptNumber = rcpt;//objApplicantApplicationForm.DocumentFeeReceiptDetails;
                        update_feedetails.Treasury_Receipt_No = objApplicantApplicationForm.TreasuryReceiptNo;
                        update_feedetails.DocVeriFeePymtStatus = 1;//objApplicantApplicationForm.TreasuryReceiptNo;

                        _db.SaveChanges();
                    }
                    else
                    {
                        tbl_VerOfficer_Applicant_Mapping objtbl_vrfmap = new tbl_VerOfficer_Applicant_Mapping();
                        objtbl_vrfmap.ApplicantId = objApplicantApplicationForm.ApplicantId;
                        objtbl_vrfmap.DocVeriFee = objApplicantApplicationForm.DocVeriFee;
                        objtbl_vrfmap.DocVeriFeePymtDate = dat;
                        objtbl_vrfmap.DocVeriFeeReceiptNumber = rcpt;
                        objtbl_vrfmap.Treasury_Receipt_No = objApplicantApplicationForm.TreasuryReceiptNo;
                        objtbl_vrfmap.DocVeriFeePymtStatus = 1;
                        objtbl_vrfmap.CreatedOn = dat;
                        _db.tbl_VerOfficer_Applicant_Mapping.Add(objtbl_vrfmap);
                        _db.SaveChanges();
                    }

                    resData = (from a in _db.tbl_Applicant_Detail
                               join ts in _db.tbl_VerOfficer_Applicant_Mapping on a.ApplicationId equals ts.ApplicantId
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new ApplicantApplicationForm
                               {
                                   ApplicationId = a.ApplicationId,
                                   ApplyMonth = a.ApplyMonth,
                                   ApplyYear = a.ApplyYear,
                                   ApplicantName = a.ApplicantName,
                                   FathersName = a.FathersName,
                                   ParentsOccupation = a.ParentsOccupation,
                                   Photo = a.Photo,
                                   DOB = a.DOB,
                                   Gender = a.Gender,
                                   Percentage = a.Percentage,
                                   MothersName = a.MothersName,
                                   Religion = a.Religion,
                                   Category = a.Category,
                                   MinorityCategory = a.MinorityCategory,
                                   Caste = a.Caste,
                                   studiedMathsScience = a.studiedMathsScience,
                                   FamilyAnnIncome = a.FamilyAnnIncome,
                                   ApplicantType = a.ApplicantType,
                                   Qualification = a.Qualification,
                                   ApplicantBelongTo = a.ApplicantBelongTo,
                                   AppliedBasic = a.AppliedBasic,
                                   TenthBoard = a.TenthBoard,
                                   InstituteStudiedQual = a.InstituteStudiedQual,
                                   MaxMarks = a.MaxMarks,
                                   MinMarks = a.MinMarks,
                                   ResultQual = a.ResultQual,
                                   CommunicationAddress = a.CommunicationAddress,
                                   DistrictId = a.DistrictId,
                                   TalukaId = a.TalukaId,
                                   Pincode = a.Pincode,
                                   SameAdd = a.SameAdd,
                                   PermanentAddress = a.PermanentAddress,
                                   PDistrict = a.PDistrict,
                                   PTaluk = a.PTaluk,
                                   PPinCode = a.PPinCode,
                                   PhoneNumber = a.PhoneNumber,
                                   FatherPhoneNumber = a.FatherPhoneNumber,
                                   EmailId = a.EmailId,
                                   DocVeriInstituteFilter = a.DocVeriInstituteFilter,
                                   DocVeriInstituteDistrict = a.DocVeriInstituteDistrict,
                                   DocVerificationCentre = a.DocVerificationCentre,
                                   PaymentOptionval = a.PaymentOptionval,
                                   //DocumentFeeReceiptDetails = a.DocumentFeeReceiptDetails,
                                   DocumentFeeReceiptDetails = ts.DocVeriFeeReceiptNumber,
                                   FlowId = a.FlowId,
                                   AssignedVO = a.AssignedVO,
                                   DocVeriFeePymtDate = ts.DocVeriFeePymtDate,
                                   ApplicantNumber = a.ApplicantNumber,
                                   DocVeriFee = ts.DocVeriFee,
                                   TreasuryReceiptNo = ts.Treasury_Receipt_No


                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                return resData;
            }

            return resData;
        }

        public List<ApplicationForm> GetApplicantReservationListDLL(int loginId)
        {
            var res = (from tbr in _db.tbl_Applicant_Reservation
                       where tbr.IsActive == true && tbr.ApplicantId == loginId
                       select new ApplicationForm
                       {
                           ReservationId = tbr.ReservationId
                       }).ToList();

            return res;
        }

        public ApplicationStatusUpdate UpdateApplicationDetailsByIdDLL(ApplicationStatusUpdate objApplicationStatusUpdate)
        {
            var resData = (dynamic)null;
            int ExistingTransRecord = 0;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    int ApplicationId = objApplicationStatusUpdate.ApplicantId;

                    ExistingTransRecord = (from tad in _db.tbl_ApplicantTrans
                                           where tad.ApplicantId == objApplicationStatusUpdate.ApplicantId && tad.Status == 9
                                           select tad.ApplicantId).FirstOrDefault();


                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();

                    objtbl_ApplicantTrans.ApplicantId = objApplicationStatusUpdate.ApplicantId;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicationStatusUpdate.VerfOfficer;
                    objtbl_ApplicantTrans.Status = objApplicationStatusUpdate.ApplStatus;
                    objtbl_ApplicantTrans.Remark = objApplicationStatusUpdate.Remarks;
                    objtbl_ApplicantTrans.DocumentName = 18;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.CreatedBy = objApplicationStatusUpdate.VerfOfficer;
                    if (objApplicationStatusUpdate.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;

                    if (ExistingTransRecord == 0 && objApplicationStatusUpdate.ApplStatus == 10)        //First Time itself Approved by VO
                        objtbl_ApplicantTrans.ApplDescStatus = 4;
                    else if (ExistingTransRecord == 0 && objApplicationStatusUpdate.ApplStatus == 9)     //Sending back for Correction first time
                        objtbl_ApplicantTrans.ApplDescStatus = 5;
                    else if (ExistingTransRecord != 0 && objApplicationStatusUpdate.ApplStatus == 9)     //Resubmitting the documents by applicant
                        objtbl_ApplicantTrans.ApplDescStatus = 5;
                    else if (ExistingTransRecord != 0 && objApplicationStatusUpdate.ApplStatus == 10)     //After Correction, Documents verified
                        objtbl_ApplicantTrans.ApplDescStatus = 7;
                    objtbl_ApplicantTrans.FlowId = objApplicationStatusUpdate.CredatedBy;

                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicationStatusUpdate.ApplicantId && s.IsActive == true).FirstOrDefault();

                    update_query.ApplStatus = objApplicationStatusUpdate.ApplStatus;
                    update_query.ApplRemarks = objApplicationStatusUpdate.Remarks;
                    update_query.ReVerficationStatus = objApplicationStatusUpdate.ReVerficationStatus;

                    if (ExistingTransRecord == 0 && objApplicationStatusUpdate.ApplStatus == 10)        //First Time itself Approved by VO
                        update_query.ApplDescStatus = 4;
                    else if (ExistingTransRecord == 0 && objApplicationStatusUpdate.ApplStatus == 9)     //Sending back for Correction first time
                        update_query.ApplDescStatus = 5;
                    else if (ExistingTransRecord != 0 && objApplicationStatusUpdate.ApplStatus == 9)     //Resubmitting the documents by applicant
                        update_query.ApplDescStatus = 5;
                    else if (ExistingTransRecord != 0 && objApplicationStatusUpdate.ApplStatus == 10)     //After Correction, Documents verified
                        update_query.ApplDescStatus = 7;

                    update_query.FlowId = objApplicationStatusUpdate.CredatedBy;

                    _db.SaveChanges();

                    transaction.Complete();
                    objApplicationStatusUpdate.UpdateMsg = "success";
                    resData = objApplicationStatusUpdate;
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                objApplicationStatusUpdate.UpdateMsg = "failed";
                resData = objApplicationStatusUpdate;
            }

            return resData;
        }

        public ApplicantDocumentsDetail UpdateGrievanceApplicationDetailsByIdDLL(ApplicantDocumentsDetail objApplicationStatusUpdate)
        {
            var resData = (dynamic)null;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    #region Updating in tbl_Grievance

                    //var update_query = _db.tbl_Grievance.Where(s => s.GrievanceId == objApplicationStatusUpdate.GrievanceId && s.ApplicationId == objApplicationStatusUpdate.ApplicantId).FirstOrDefault();
                    //update_query.Status = objApplicationStatusUpdate.Status;
                    //update_query.Remarks = objApplicationStatusUpdate.Remarks;
                    //update_query.CreatedBy = objApplicationStatusUpdate.CreatedBy;
                    //_db.SaveChanges();

                    #endregion

                    #region Updating in tbl_Applicant_Details

                    if (objApplicationStatusUpdate.Status != 3)
                    {
                        var update_query_Appl = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicationStatusUpdate.ApplicantId && s.IsActive == true).FirstOrDefault();
                        update_query_Appl.FathersName = objApplicationStatusUpdate.FathersName;
                        update_query_Appl.Gender = objApplicationStatusUpdate.Gender;
                        update_query_Appl.DOB = objApplicationStatusUpdate.DOB;
                        update_query_Appl.Category = objApplicationStatusUpdate.Category;
                        update_query_Appl.ApplicantBelongTo = objApplicationStatusUpdate.ApplicantBelongTo;
                        update_query_Appl.MaxMarks = objApplicationStatusUpdate.MaxMarks;
                        update_query_Appl.MinMarks = objApplicationStatusUpdate.MinMarks;
                        update_query_Appl.MarksObtained = objApplicationStatusUpdate.MarksObtained;
                        update_query_Appl.Percentage = objApplicationStatusUpdate.Percentage;
                        update_query_Appl.ResultQual = objApplicationStatusUpdate.ResultQual;
                        _db.SaveChanges();
                    }

                    #endregion

                    transaction.Complete();
                    objApplicationStatusUpdate.UpdateMsg = "success";
                    resData = objApplicationStatusUpdate;
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                objApplicationStatusUpdate.UpdateMsg = "failed";
                resData = objApplicationStatusUpdate;
            }

            return resData;
        }

        public List<GrievanceDocApplData> ApplicantGrievanceDetailsDLL(GrievanceDocApplData objGrievanceDocApplData)
        {
            var resData = (dynamic)null;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    #region .. Updating in tbl_Applicant_Details ..

                    int ApplicationId = objGrievanceDocApplData.ApplicantId;

                    var update_query_Appl = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objGrievanceDocApplData.ApplicantId && s.IsActive == true).FirstOrDefault();
                    update_query_Appl.FathersName = objGrievanceDocApplData.FathersName;
                    update_query_Appl.Gender = objGrievanceDocApplData.Gender;
                    update_query_Appl.DOB = objGrievanceDocApplData.DOB;
                    update_query_Appl.Category = objGrievanceDocApplData.Category;
                    update_query_Appl.ApplicantBelongTo = objGrievanceDocApplData.ApplicantBelongTo;
                    update_query_Appl.Caste = objGrievanceDocApplData.Caste;
                    update_query_Appl.MaxMarks = objGrievanceDocApplData.MaxMarks;
                    update_query_Appl.MinMarks = objGrievanceDocApplData.MinMarks;
                    update_query_Appl.MarksObtained = objGrievanceDocApplData.MarksObtained;
                    update_query_Appl.Percentage = objGrievanceDocApplData.Percentage;
                    update_query_Appl.ResultQual = objGrievanceDocApplData.ResultQual;
                    update_query_Appl.PhysicallyHanidcapInd = objGrievanceDocApplData.PhysicallyHanidcapInd;
                    update_query_Appl.PhysicallyHanidcapType = objGrievanceDocApplData.PhysicallyHanidcapType;
                    update_query_Appl.HyderabadKarnatakaRegion = objGrievanceDocApplData.HyderabadKarnatakaRegion;
                    update_query_Appl.ApplRemarks = objGrievanceDocApplData.Remarks;

                    _db.SaveChanges();

                    #endregion

                    tbl_Applicant_Reservation objtbl_Applicant_Reservation = new tbl_Applicant_Reservation();
                    if (objGrievanceDocApplData.ApplicableReservations != null)
                    {
                        var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objGrievanceDocApplData.ApplicantId && s.IsActive == true).ToList();
                        foreach (var item in remove_query)
                        {
                            _db.tbl_Applicant_Reservation.Remove(item);
                        }
                        foreach (int ReservationApp in objGrievanceDocApplData.ApplicableReservations)
                        {
                            objtbl_Applicant_Reservation.ApplicantId = objGrievanceDocApplData.ApplicantId;
                            objtbl_Applicant_Reservation.ReservationId = ReservationApp;
                            objtbl_Applicant_Reservation.IsActive = true;
                            objtbl_Applicant_Reservation.CreatedOn = DateTime.Now;
                            _db.tbl_Applicant_Reservation.Add(objtbl_Applicant_Reservation);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        if (objGrievanceDocApplData.ExserDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objGrievanceDocApplData.ApplicantId && s.ReservationId == 2 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        if (objGrievanceDocApplData.EWSDocStatus == 3)
                        {
                            var remove_query = _db.tbl_Applicant_Reservation.Where(s => s.ApplicantId == objGrievanceDocApplData.ApplicantId && s.ReservationId == 5 && s.IsActive == true).ToList();
                            foreach (var item in remove_query)
                            {
                                _db.tbl_Applicant_Reservation.Remove(item);
                            }
                        }
                        _db.SaveChanges();
                    }


                    #region Updating in tbl_ApplicantTrans

                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == objGrievanceDocApplData.ApplicantId && tad.IsActive == true
                                              select new ApplicationStatusUpdate
                                              {
                                                  FlowId = tad.FlowId,
                                                  AssignedVO = tad.AssignedVO,
                                                  ApplDescStatus = tad.ApplDescStatus,
                                                  ApplStatusS = tad.ApplStatus
                                              }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objGrievanceDocApplData.ApplicantId;
                    objtbl_ApplicantTrans.CreatedBy = objGrievanceDocApplData.LoginId;
                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                    objtbl_ApplicantTrans.ReVerficationStatus = false;
                    objtbl_ApplicantTrans.Remark = objGrievanceDocApplData.Remarks;
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.Status = ExistingAssignedVOval.ApplStatusS;
                            objtbl_ApplicantTrans.VerfOfficer = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                            objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();

                    #endregion

                    objGrievanceDocApplData.UpdateMsg = "success";

                    resData = (from a in _db.tbl_Applicant_Detail
                               where a.ApplicationId == ApplicationId && a.IsActive == true
                               select new GrievanceDocApplData
                               {
                                   ApplicantId = a.ApplicationId
                               }).ToList();

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string exMessage = ex.Message.ToString();
                objGrievanceDocApplData.UpdateMsg = "failed";
                resData = objGrievanceDocApplData;
            }

            return resData;
        }
        public ApplicantApplicationForm GenerateApplicationNumberDLL(ApplicantApplicationForm objApplicantApplicationForm)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {

                    if (objApplicantApplicationForm.ApplicantId != 0 && objApplicantApplicationForm.ApplicantNumber == null)
                    {
                        objApplicantApplicationForm.ApplicantNumber = RandomString(objApplicantApplicationForm.ApplyYear,
                                objApplicantApplicationForm.ApplicantType, objApplicantApplicationForm.DistrictId);
                    }

                    #region .. Updating ApplicantNumber ..

                    tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();
                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicantId && s.IsActive == true && (s.ReVerficationStatus == 0 || s.ReVerficationStatus == null)).FirstOrDefault();
                    if (update_query != null)
                    {
                        update_query.ApplicantNumber = objApplicantApplicationForm.ApplicantNumber;
                        update_query.ReVerficationStatus = 0;
                        _db.SaveChanges();
                    }
                    #endregion

                    #region .. Updating Flow ID , ReVerficationStatus,  ApplDescStatus, AssignedVO, ApplStatus ..
                    int? TransFlowId = 0;

                    string ExistingRecordDocumentFeeReceiptDetails = (from tad in _db.tbl_Applicant_Detail
                                                                      where tad.ApplicationId == objApplicantApplicationForm.ApplicantId && tad.IsActive == true
                                                                      select tad.DocumentFeeReceiptDetails).Take(1).FirstOrDefault();

                    int? AssignedVO = (from tad in _db.tbl_Applicant_Detail
                                       where tad.ApplicationId == objApplicantApplicationForm.ApplicantId && tad.IsActive == true
                                       select tad.AssignedVO).Take(1).FirstOrDefault();

                    int? VerfStatusForMsg = (from tad in _db.tbl_Applicant_Detail
                                             where tad.ApplicationId == objApplicantApplicationForm.ApplicantId && tad.IsActive == true
                                             select tad.ReVerficationStatus).Take(1).FirstOrDefault();

                    var update_query_flow = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicantId && s.IsActive == true).FirstOrDefault();
                    if (update_query_flow != null)
                    {
                        if (ExistingRecordDocumentFeeReceiptDetails != "" && ExistingRecordDocumentFeeReceiptDetails != null)
                        {
                            if (AssignedVO != null && AssignedVO != 0)
                            {
                                update_query_flow.FlowId = AssignedVO;
                                update_query_flow.ApplDescStatus = 6;           //Document Re-verification pending
                                TransFlowId = AssignedVO;

                            }
                            else
                            {
                                update_query_flow.ReVerficationStatus = 0;
                                update_query_flow.ApplDescStatus = 3;           //Document verification pending
                            }
                        }
                        else
                        {
                            if (AssignedVO != null && AssignedVO != 0)
                            {
                                update_query_flow.FlowId = AssignedVO;
                                TransFlowId = AssignedVO;
                            }
                            update_query_flow.ReVerficationStatus = 0;
                            update_query_flow.ApplDescStatus = 2;               //Document verification fee Pending
                        }
                    }
                    update_query_flow.CredatedBy = objApplicantApplicationForm.CredatedBy;
                    _db.SaveChanges();

                    #endregion

                    #region .. Updating Remarks after rejecting by VO for Document verificaiton .. 

                    var update_query_remarks = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == objApplicantApplicationForm.ApplicantId && s.IsActive == true && s.ReVerficationStatus == 1).FirstOrDefault();
                    if (update_query_remarks != null)
                    {
                        update_query_remarks.ApplRemarks = objApplicantApplicationForm.ApplRemarks;
                        _db.SaveChanges();
                    }
                    #endregion


                    var ExistingAssignedVO = (from tad in _db.tbl_Applicant_Detail
                                              where tad.ApplicationId == objApplicantApplicationForm.ApplicantId && tad.IsActive == true
                                              select new ApplicationStatusUpdate { FlowId = tad.FlowId, AssignedVO = tad.AssignedVO, ApplDescStatus = tad.ApplDescStatus }).ToList();

                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                    objtbl_ApplicantTrans.ApplicantId = objApplicantApplicationForm.ApplicantId;
                    objtbl_ApplicantTrans.CreatedBy = objApplicantApplicationForm.CredatedBy;

                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                    objtbl_ApplicantTrans.Status = objApplicantApplicationForm.ApplStatus;
                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                    objtbl_ApplicantTrans.IsActive = 1;
                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                    objtbl_ApplicantTrans.VerfOfficer = objApplicantApplicationForm.VerfOfficer;
                    if (objApplicantApplicationForm.ReVerficationStatus == 1)
                        objtbl_ApplicantTrans.ReVerficationStatus = true;
                    else
                        objtbl_ApplicantTrans.ReVerficationStatus = false;
                    if (objApplicantApplicationForm.ApplRemarks == "" || objApplicantApplicationForm.ApplRemarks == null)
                        objtbl_ApplicantTrans.Remark = "Applicant submitted their application by clicking on Final Submit";
                    else
                        objtbl_ApplicantTrans.Remark = objApplicantApplicationForm.ApplRemarks;
                    if (ExistingAssignedVO.Count > 0)
                    {
                        foreach (var ExistingAssignedVOval in ExistingAssignedVO)
                        {
                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                            objtbl_ApplicantTrans.ApplDescStatus = ExistingAssignedVOval.ApplDescStatus;
                            if (TransFlowId == 0)
                                objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
                            else
                                objtbl_ApplicantTrans.FlowId = TransFlowId;
                        }
                    }
                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                    _db.SaveChanges();
                    transaction.Complete();

                    if (VerfStatusForMsg == 1)
                        objApplicantApplicationForm.UpdateMsg = "Your Application Number :: <b>" + objApplicantApplicationForm.ApplicantNumber + "</b>. Your Application re-submitted successfully to the Verification officer to recheck your updated documents";
                    else
                        objApplicantApplicationForm.UpdateMsg = "Your Application Number :: <b>" + objApplicantApplicationForm.ApplicantNumber + "</b> has been Generated. Your Application submitted successfully";
                }
            }
            catch (Exception ex)
            {
                objApplicantApplicationForm.UpdateMsg = "There is error in Generating Application Number";
            }

            return objApplicantApplicationForm;
        }


        public string RandomString(int? SelectedApplyYear, int? SelectedApplicantType, int? DistrictId)
        {
            string ApplicationRandomNumber = "";
            try
            {
                int AppRefValStartingValue = 1; string AppRefVal = ""; string dist = Convert.ToString(DistrictId);
                List<ApplicantApplicationForm> res = new List<ApplicantApplicationForm>();
                res = (from tad in _db.tbl_Applicant_Detail
                       where tad.IsActive == true && !string.IsNullOrEmpty(tad.ApplicantNumber) && tad.ApplicantNumber.Substring(3, 3) == dist

                       select new ApplicantApplicationForm
                       {
                           ApplicantNumber = (tad.ApplicantNumber.Substring(7, 5))
                       }).Distinct().ToList();

                var MaxApplicationNumber = res.Max(r => r.ApplicantNumber);
                if (MaxApplicationNumber == "" || MaxApplicationNumber == null)
                {
                    AppRefVal = AppRefValStartingValue.ToString().PadLeft(5, '0');
                    ApplicationRandomNumber = SelectedApplyYear.ToString().Substring(2, 2) + SelectedApplicantType + DistrictId + AppRefVal;
                }
                else
                {
                    int MaxApplicationNumber1 = Int32.Parse(MaxApplicationNumber);
                    MaxApplicationNumber1++;
                    AppRefVal = MaxApplicationNumber1.ToString().PadLeft(5, '0');
                    ApplicationRandomNumber = SelectedApplyYear.ToString().Substring(2, 2) + SelectedApplicantType + DistrictId + AppRefVal;
                }
            }
            catch (Exception ex)
            {

            }
            return ApplicationRandomNumber;
        }

        public List<ApplicantApplicationForm> GetApplicantRemarksListDLL(int ApplicantTransId)
        {
            var res = (from tat in _db.tbl_ApplicantTrans
                       join tsm in _db.tbl_status_master on tat.Status equals tsm.StatusId
                       join tum in _db.tbl_user_master on tat.VerfOfficer equals tum.um_id
                       join tdt in _db.tbl_DocumentType on tat.DocumentName equals tdt.DocumentTypeId
                       where tat.ApplicantId == ApplicantTransId
                       orderby tat.CreatedOn descending
                       select new ApplicantApplicationForm
                       {
                           ApplicationId = tat.ApplicantId,
                           DocumentTypeName = tdt.DocumentType,
                           DocumentWiseRemarks = tat.DocumentWiseRemarks,
                           UserName = tum.um_name,
                           StatusName = tsm.StatusName,
                           Remark = tat.Remark,
                           CreatedOn = tat.CreatedOn
                       }).ToList();

            return res;
        }

        #endregion

        #region .. MeritList ..
        public List<AdmissionMeritList> GetCommentsMeritListFileDLL(int id)
        {
            try
            {
                var meritlist = (from n in _db.tbl_GradationRank_TransHistory
                                 join e in _db.tbl_GradationRank_Trans on n.ApplicantId equals e.ApplicantId
                                 join u in _db.tbl_role_master on n.CreatedBy equals u.role_id
                                 join bb in _db.tbl_status_master on n.Status equals bb.StatusId
                                 where n.ApplicantId == id && n.Status != 5

                                 select new AdmissionMeritList
                                 {
                                     comments = n.Remarks,
                                     login_id = n.CreatedBy,
                                     FromUser = u.role_description,
                                     //Status = bb.StatusId,
                                     StatusName = bb.StatusName,
                                     createdatetime = n.CreatedOn.ToString(),
                                     FlowId = n.FlowId
                                 }
                 ).ToList();
                var rss = (from aa in meritlist
                           join uu in _db.tbl_role_master on aa.FlowId equals uu.role_id
                           select new AdmissionMeritList
                           {
                               comments = aa.comments,
                               //StatusName = aa.StatusName,
                               FromUser = aa.FromUser,
                               StatusName = aa.StatusName,
                               To = uu.role_description,
                               createdatetime = aa.createdatetime
                           }).OrderByDescending(x => x.createdatetime.Trim()).ToList();

                return rss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserDetails> GetRoles(int id, int level)
        {
            int? cur_role_seniority_no = 0;
            try
            {
                cur_role_seniority_no = (from aa in _db.tbl_role_master
                                         where aa.role_id == id && aa.role_is_active == true && aa.role_Level == 1
                                         select aa.role_seniority_no).FirstOrDefault();

                if (level == 1)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no < cur_role_seniority_no
                               orderby aa.role_seniority_no descending
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
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no < cur_role_seniority_no
                               orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                 ).ToList();
                    return res;
                }
                else if (level == 4)
                {
                    //admission send back
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no > cur_role_seniority_no && aa.role_id != 10 && aa.role_id != 11 && aa.role_id != 12 && aa.role_id != 9 && aa.role_id != 3 && aa.role_id != 4 && aa.role_id != 7 && aa.role_id != 8
                               //orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                 ).ToList();
                    return res;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UserDetails> GetMeritRoles(int id, int level)
        {
            int? cur_role_seniority_no = 0;
            try
            {
                cur_role_seniority_no = (from aa in _db.tbl_role_master
                                         where aa.role_id == id && aa.role_is_active == true && aa.role_Level == 1
                                         select aa.role_seniority_no).FirstOrDefault();

                if (level == 1)
                {
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no < cur_role_seniority_no
                               orderby aa.role_seniority_no descending
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
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no < cur_role_seniority_no
                               orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                 ).ToList();
                    return res;
                }
                else if (level == 4)
                {
                    //admission send back
                    var res = (from aa in _db.tbl_role_master
                               where aa.role_id != id && aa.role_is_active == true && aa.role_Level == 1 && aa.role_seniority_no > cur_role_seniority_no && aa.role_id == 5 /*&& aa.role_id != 11 && aa.role_id != 12 && aa.role_id != 9 && aa.role_id != 3 && aa.role_id != 4 && aa.role_id != 7 && aa.role_id != 8 && aa.role_id != 6 && aa.role_id != 2*/
                               //orderby aa.role_id descending
                               select new UserDetails
                               {
                                   RoleID = aa.role_id,
                                   RoleName = aa.role_description
                               }
                 ).ToList();
                    return res;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetMeritStatusListDLL(int id)
        {
            try
            {
                List<AdmissionMeritList> list = null;
                if (id == 1)
                {
                    list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true && a.StatusId == 2 || a.StatusId == 9)
                            select new AdmissionMeritList
                            {
                                StatusName = a.StatusName,
                                Status = a.StatusId

                            }).ToList();
                }
                else if (id == 2)
                {
                    list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true && a.StatusId == 2 || a.StatusId == 9 || a.StatusId == 7)
                            select new AdmissionMeritList
                            {
                                StatusName = a.StatusName,
                                Status = a.StatusId

                            }).ToList();
                }
                else if (id == 6)
                {
                    list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true && a.StatusId == 7 || a.StatusId == 4)
                            select new AdmissionMeritList
                            {
                                StatusName = a.StatusName,
                                Status = a.StatusId

                            }).ToList();
                }
                else if (id == 4)
                {
                    list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true && a.StatusId == 7 || a.StatusId == 4 /*|| a.StatusId==8*/)
                            select new AdmissionMeritList
                            {
                                StatusName = a.StatusName,
                                Status = a.StatusId

                            }).ToList();
                }
                else
                {
                    list = (from a in _db.tbl_status_master.Where(a => a.IsActive == true)
                            select new AdmissionMeritList
                            {
                                StatusName = a.StatusName,
                                Status = a.StatusId

                            }).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetDistricts(int divId)
        {
            if (divId != 0)
            {
                var res = (from a in _db.tbl_district_master.Where(a => a.dis_is_active == true)
                           where a.dis_is_active == true && a.division_id == divId
                           select new AdmissionMeritList
                           {
                               district_id = a.district_lgd_code,
                               district_ename = a.district_ename
                           }).ToList();
                return res;
            }
            else
            {
                var res = (from a in _db.tbl_district_master.Where(a => a.dis_is_active == true)
                           where a.dis_is_active == true
                           select new AdmissionMeritList
                           {
                               district_id = a.district_lgd_code,
                               district_ename = a.district_ename
                           }).ToList();
                return res;
            }
        }
        public List<AdmissionMeritList> GetDivision()
        {
            var res = (from a in _db.tbl_division_master
                       select new AdmissionMeritList
                       {
                           division_id = a.division_id,
                           division_name = a.division_name
                       }).ToList();
            return res;
        }
        public List<AdmissionMeritList> GetApplicantType()
        {
            var res = (from a in _db.tbl_ApplicantType
                       select new AdmissionMeritList
                       {
                           ApplicantTypeId = a.ApplicantTypeId,
                           ApplicantTypeDdl = a.ApplicantType
                       }).ToList();
            return res;
        }

        public List<AdmissionMeritList> GetApplicationMode_Dll()
        {
            var res = (from a in _db.tbl_ApplicationMode
                       where a.IsActive == true
                       select new AdmissionMeritList
                       {
                           ApplicationModeId = a.ApplicationModeId,
                           ApplicationMode = a.ApplicationMode
                       }).ToList();
            return res;
        }

        public List<AdmissionMeritList> GetTraineeType()
        {
            var res = (from a in _db.tbl_TraineeType
                       where a.IsActive == true
                       select new AdmissionMeritList
                       {
                           ApplicantTypeId = a.TraineeTypeId,
                           ApplicantTypeDdl = a.TraineeType
                       }).ToList();
            return res;
        }

        public List<AdmissionMeritList> GetGradationType()
        {
            var res = (from a in _db.tbl_GradationType
                       select new AdmissionMeritList
                       {
                           GradationTypeId = a.GradationTypeId,
                           GradationTypes = a.GradationType
                       }).ToList();
            return res;
        }

        public List<AdmissionMeritList> GetGradationMeritListDLL(int generateId, int YearId, int ApplicantTypeId, int round, int id)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null; int age = 0;
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYear = Convert.ToInt32(yr);
                if (generateId == 1)
                {
                    if (id == 5)  //Deputy Director 
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.ApplStatus == 10 && x.IsActive == true)
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId into p
                                  where p.Count() == 0
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join h in _db.tbl_qualification on n.Qualification equals h.QualificationId

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      locationId = l.location_id,
                                      locationName = l.location_name,
                                      ApplicantTypes = at.ApplicantType,
                                      ApplyYear = n.ApplyYear,
                                      MarksObtained_1 = n.MarksObtained,
                                      GenderId = n.Gender,
                                      Qualification = h.QualificationId,
                                      Qual = h.Qualification,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      ExServiceMan = n.ExServiceMan,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      roundId = round
                                  }).Distinct().ToList();
                        //n.HyderabadKarnatakaRegion.Value
                        foreach (var p in Notifs)
                        {
                            age = 0;
                            #region commented code
                            //if (p.DOB !=null)
                            //{
                            //    DateTime dateFB = (DateTime)p.DOB;
                            //    p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                            //}

                            //if (p.DiffAbled == true)
                            //    p.DiffrentAbled = "Yes";
                            //else
                            //    p.DiffrentAbled = "No";

                            //var resid = (from qq in _db.tbl_Applicant_Reservation
                            //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                            //             select xx.ReservationId).FirstOrDefault();

                            //if (resid != 0)
                            //    p.ExService = "Yes";
                            //else
                            //    p.ExService = "No";

                            //var ewsid = (from qq in _db.tbl_Applicant_Reservation
                            //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                            //             select xx.ReservationId).FirstOrDefault();

                            //if (ewsid != 0)
                            //    p.EconomicWeekerSec = "Yes";
                            //else
                            //    p.EconomicWeekerSec = "No";

                            //var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                            //               join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //               where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                            //               select xx.ReservationId).FirstOrDefault();

                            //if (KanndaM != 0)
                            //    p.KanndaMedium = "Yes";
                            //else
                            //    p.KanndaMedium = "No";

                            //var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                            //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                            //                     select xx.ReservationId).FirstOrDefault();

                            //if (HydKarnRegion != 0)
                            //    p.HyderabadKarnatakaRegion = "Yes";
                            //else
                            //    p.HyderabadKarnatakaRegion = "No";
                            #endregion
                            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                            p.age = age;
                            if (p.locationId == 1)
                            {
                                //p.Weitage = Math.Round(p.MarksObtained * 10 / 100, 2);
                                p.Weitage = p.MarksObtained * 10 / 100;
                                p.Total = p.MarksObtained + p.Weitage;
                                if (p.Total > p.MaxMarks)
                                {
                                    p.Total = p.MaxMarks;
                                }
                                //p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                                p.Percentage = (Convert.ToInt32(p.Total) / p.MaxMarks) * 100;
                            }
                            else
                            {
                                //p.Weitage = Math.Round(p.MarksObtained * 0 / 100, 2);
                                p.Weitage = p.MarksObtained * 0 / 100;

                                p.Total = p.MarksObtained + p.Weitage;
                                //p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                                p.Percentage = (p.MarksObtained / p.MaxMarks) * 100;
                            }
                        }
                    }
                }
                else if (generateId == 2)
                {
                    if (id == 5 || id == 2 || id == 1)
                    {
                        if (round > 1)
                        {
                            Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
                                      join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                      join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                      join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                      join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                      join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                      join c in _db.tbl_Category on n.Category equals c.CategoryId
                                      join h in _db.tbl_qualification on n.Qualification equals h.QualificationId
                                      join i in _db.tbl_SeatAllocationDetail_Seatmatrix on n.ApplicationId equals i.ApplicantId


                                      where i.Status == (int)CmnClass.Status.SeatNOTAlloted
                                      select new AdmissionMeritList
                                      {
                                          ApplicationId = n.ApplicationId,
                                          ApplicantNumber = n.ApplicantNumber,
                                          ApplicantName = n.ApplicantName,
                                          FathersName = n.FathersName,
                                          Gender = g.Gender,
                                          DOB = n.DOB,
                                          Category = n.Category,
                                          CategoryName = c.Category,
                                          MaxMarks = n.MaxMarks,
                                          MarksObtained = n.MarksObtained,
                                          locationId = l.location_id,
                                          Result = r.Result,
                                          locationName = l.location_name,
                                          ApplicantTypes = at.ApplicantType,
                                          MarksObtained_1 = n.MarksObtained,
                                          GenderId = n.Gender,
                                          ApplyYear = n.ApplyYear,
                                          Qualification = h.QualificationId,
                                          Qual = h.Qualification,
                                          FlowId = t.FlowId,
                                          Percentage = t.Percentage,
                                          KMedium = n.KanndaMedium,
                                          HydKarRegion = n.HyderabadKarnatakaRegion,
                                          DiffAbled = n.PhysicallyHanidcapInd,
                                          EcoWeakSection = n.EconomyWeakerSection,
                                          ExServiceMan = n.ExServiceMan
                                      }).Distinct().ToList();

                            var Notifs1 = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
                                           join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                           join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                           join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                           join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                           join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                           join c in _db.tbl_Category on n.Category equals c.CategoryId
                                           join h in _db.tbl_qualification on n.Qualification equals h.QualificationId
                                           join j in _db.tbl_Applicant_ITI_Institute_Detail on n.ApplicationId equals j.ApplicationId


                                           where j.AdmittedStatus != (int)CmnClass.AdmissionStatus.Admitted && n.ParticipateNextRound == true
                                           //|| (j.AdmittedStatus == (int)CmnClass.AdmissionStatus.Pending && n.ParticipateNextRound == true)
                                           select new AdmissionMeritList
                                           {
                                               ApplicationId = n.ApplicationId,
                                               ApplicantNumber = n.ApplicantNumber,
                                               ApplicantName = n.ApplicantName,
                                               FathersName = n.FathersName,
                                               Gender = g.Gender,
                                               DOB = n.DOB,
                                               Category = n.Category,
                                               CategoryName = c.Category,
                                               MaxMarks = n.MaxMarks,
                                               MarksObtained = n.MarksObtained,
                                               locationId = l.location_id,
                                               Result = r.Result,
                                               locationName = l.location_name,
                                               ApplicantTypes = at.ApplicantType,
                                               MarksObtained_1 = n.MarksObtained,
                                               ParticipateNextRound = n.ParticipateNextRound,
                                               GenderId = n.Gender,
                                               ApplyYear = n.ApplyYear,
                                               Qualification = h.QualificationId,
                                               Qual = h.Qualification,
                                               FlowId = t.FlowId,
                                               Percentage = t.Percentage,
                                               KMedium = n.KanndaMedium,
                                               HydKarRegion = n.HyderabadKarnatakaRegion,
                                               DiffAbled = n.PhysicallyHanidcapInd,
                                               EcoWeakSection = n.EconomyWeakerSection,
                                               ExServiceMan = n.ExServiceMan
                                           }).Distinct().ToList();
                            //Notifs1 = Notifs1.Where(a => a.ParticipateNextRound == true).ToList();
                            Notifs.AddRange(Notifs1);

                        }
                        else
                        {
                            Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
                                      join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                      join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                      join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                      join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                      join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                      join c in _db.tbl_Category on n.Category equals c.CategoryId
                                      join h in _db.tbl_qualification on n.Qualification equals h.QualificationId
                                      where t.FlowId == 5
                                      select new AdmissionMeritList
                                      {
                                          ApplicationId = n.ApplicationId,
                                          ApplicantNumber = n.ApplicantNumber,
                                          ApplicantName = n.ApplicantName,
                                          FathersName = n.FathersName,
                                          Gender = g.Gender,
                                          DOB = n.DOB,
                                          Category = n.Category,
                                          CategoryName = c.Category,
                                          MaxMarks = n.MaxMarks,
                                          MarksObtained = n.MarksObtained,
                                          locationId = l.location_id,
                                          Result = r.Result,
                                          locationName = l.location_name,
                                          ApplicantTypes = at.ApplicantType,
                                          MarksObtained_1 = n.MarksObtained,
                                          GenderId = n.Gender,
                                          ApplyYear = n.ApplyYear,
                                          Qualification = h.QualificationId,
                                          Qual = h.Qualification,
                                          FlowId = t.FlowId,
                                          Percentage = t.Percentage,
                                          KMedium = n.KanndaMedium,
                                          HydKarRegion = n.HyderabadKarnatakaRegion,
                                          DiffAbled = n.PhysicallyHanidcapInd,
                                          EcoWeakSection = n.EconomyWeakerSection,
                                          ExServiceMan = n.ExServiceMan
                                      }).Distinct().ToList();
                        }
                        foreach (var p in Notifs)
                        {
                            age = 0;
                            #region commented code
                            //if (p.DOB != null)
                            //{
                            //    DateTime dateFB = (DateTime)p.DOB;
                            //    p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                            //}

                            //if (p.DiffAbled == true)
                            //{
                            //    p.DiffrentAbled = "Yes";
                            //}

                            //else
                            //{
                            //    p.DiffrentAbled = "No";
                            //}
                            //var resid = (from qq in _db.tbl_Applicant_Reservation
                            //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                            //             select xx.ReservationId).FirstOrDefault();
                            //if (resid != 0)
                            //    p.ExService = "Yes";
                            //else
                            //    p.ExService = "No";

                            //var ewsid = (from qq in _db.tbl_Applicant_Reservation
                            //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                            //             select xx.ReservationId).FirstOrDefault();
                            //if (ewsid != 0)
                            //    p.EconomicWeekerSec = "Yes";
                            //else
                            //    p.EconomicWeekerSec = "No";

                            //var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                            //               join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //               where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                            //               select xx.ReservationId).FirstOrDefault();
                            //if (KanndaM != 0)
                            //    p.KanndaMedium = "Yes";
                            //else
                            //    p.KanndaMedium = "No";
                            //var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                            //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                            //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                            //                     select xx.ReservationId).FirstOrDefault();
                            //if (HydKarnRegion != 0)
                            //    p.HyderabadKarnatakaRegion = "Yes";
                            //else
                            //    p.HyderabadKarnatakaRegion = "No";
                            #endregion
                            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                            p.age = age;
                            if (p.locationId == 1)
                            {
                                //p.Weitage = Math.Round(p.MarksObtained * 10 / 100, 2);
                                p.Weitage = p.MarksObtained * 10 / 100;
                                p.Total = p.MarksObtained + p.Weitage;
                                if (p.Total > p.MaxMarks)
                                {
                                    p.Total = p.MaxMarks;
                                }
                                //p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                                p.Percentage = (Convert.ToInt32(p.Total) / p.MaxMarks) * 100;
                            }
                            else
                            {
                                //p.Weitage = Math.Round(p.MarksObtained * 0 / 100, 2);
                                p.Weitage = p.MarksObtained * 0 / 100;
                                p.Total = p.MarksObtained + p.Weitage;
                                //p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                                p.Percentage = (p.MarksObtained / p.MaxMarks) * 100;
                            }
                        }
                    }
                }
                updateApplicant(ref Notifs);
                //return Notifs.Where(x => x.Qualification != 1 ).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                List<AdmissionMeritList> listResult = new List<AdmissionMeritList>();
                if (Notifs.Count > 0)
                {
                    var list10 = Notifs.Where(x => x.Qualification == 2).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                    var list8 = Notifs.Where(x => x.Qualification == 1).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                    listResult.AddRange(list10);
                    listResult.AddRange(list8);
                }
                return listResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetceckGradationTransTable(int generateId, int AcademicYear, int ApplicantTypeId, int id)
        {
            try
            {
                List<AdmissionMeritList> res = null;
                if (generateId == 1)
                {
                    res = (from a in _db.tbl_GradationRank_Trans
                           join n in _db.tbl_Applicant_Detail on a.ApplicantId equals n.ApplicationId
                           where n.ApplicantType == ApplicantTypeId && n.ApplyYear == AcademicYear
                           select new AdmissionMeritList
                           {
                               Status = a.Status,
                               //Applicanttypecheck = n.ApplicantType
                           }).ToList();
                    res = res.GroupBy(a => a.ApplyYear).Select(a => new AdmissionMeritList
                    {
                        Status = a.Select(z => z.Status).FirstOrDefault(),
                        // Applicanttypecheck = a.Select(z => z.ApplicantType).FirstOrDefault()
                    }).ToList();
                }
                else
                {
                    res = (from a in _db.tbl_GradationRank_Trans
                           join n in _db.tbl_Applicant_Detail on a.ApplicantId equals n.ApplicationId
                           where n.ApplicantType == ApplicantTypeId && n.ApplyYear == AcademicYear
                           select new AdmissionMeritList
                           {
                               Status = a.Status,
                           }).ToList();
                    res = res.GroupBy(a => a.ApplyYear).Select(a => new AdmissionMeritList
                    {
                        Status = a.Select(z => z.Status).FirstOrDefault(),
                    }).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Approve by DD Tentative Approval
        public string GetGenerateMeritListDLL(nestedMeritList model, int loginId, string remarks, int round)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 5)
                    {
                        foreach (var p in model.lists)
                        {
                            if (model != null)
                            {
                                tbl_GradationRank_Trans MeritListTrans = new tbl_GradationRank_Trans();
                                MeritListTrans.ApplicantId = p.ApplicationId;
                                MeritListTrans.Status = 5;
                                MeritListTrans.TransDate = DateTime.Now;
                                MeritListTrans.FlowId = loginId;
                                MeritListTrans.Tentative = true;
                                MeritListTrans.Final = false;
                                MeritListTrans.Rank = p.Rank;
                                MeritListTrans.Weitage = p.Weitage;
                                MeritListTrans.Total = p.Total;
                                MeritListTrans.Percentage = p.Percentage;
                                MeritListTrans.Remarks = remarks;//"Submitted";
                                MeritListTrans.CreatedOn = DateTime.Now;
                                MeritListTrans.CreatedBy = loginId;
                                MeritListTrans.RoundId = round;
                                _db.tbl_GradationRank_Trans.Add(MeritListTrans);
                                _db.SaveChanges();

                                int ExistingRecordForUpdate = 0;
                                ExistingRecordForUpdate = (from tad in _db.tbl_GradationRank_Trans
                                                           orderby tad.CreatedOn descending
                                                           select tad.Gradation_trans_Id).FirstOrDefault();

                                #region .. Updating in tbl_Applicant_Detail table .. 

                                var ApplicantLIst = (from tgt in _db.tbl_GradationRank_Trans
                                                     where tgt.Tentative == true && tgt.ApplicantId == p.ApplicationId
                                                     select new ApplicantApplicationForm
                                                     {
                                                         ApplicantId = tgt.ApplicantId
                                                     }).ToList();

                                foreach (var applID in ApplicantLIst)
                                {
                                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == p.ApplicationId).FirstOrDefault();
                                    update_query.ApplDescStatus = 8;        //Gradation tentative Generated
                                    update_query.ApplRemarks = "Gradation Tentative List Generated";
                                    _db.SaveChanges();

                                    var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
                                                          where tad.ApplicationId == applID.ApplicantId && tad.IsActive == true
                                                          select new ApplicationStatusUpdate
                                                          {
                                                              CredatedBy = tad.CredatedBy,
                                                              FlowId = tad.FlowId,
                                                              AssignedVO = tad.AssignedVO,
                                                              ApplStatusTrans = tad.ApplStatus
                                                          }).ToList();

                                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                                    objtbl_ApplicantTrans.ApplicantId = applID.ApplicantId;
                                    objtbl_ApplicantTrans.Remark = "Gradation Tentative List Generated";
                                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                                    objtbl_ApplicantTrans.ApplDescStatus = 8;
                                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                                    objtbl_ApplicantTrans.IsActive = 1;
                                    if (FlowIDExisting.Count > 0)
                                    {
                                        foreach (var ExistingAssignedVOval in FlowIDExisting)
                                        {
                                            objtbl_ApplicantTrans.CreatedBy = ExistingAssignedVOval.CredatedBy;   //Mapping VO to Applicant trans table 
                                            objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
                                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                            objtbl_ApplicantTrans.Status = ExistingAssignedVOval.ApplStatusTrans;
                                        }
                                    }
                                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                                    _db.SaveChanges();
                                }

                                #endregion

                                //History
                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = ExistingRecordForUpdate;
                                MeritHistory.ApplicantId = p.ApplicationId;
                                MeritHistory.TransDate = DateTime.Now;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = true;
                                MeritHistory.Status = 5;
                                MeritHistory.Remarks = remarks;//"Submitted";
                                MeritHistory.FlowId = loginId;
                                MeritHistory.Final = false;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                _db.SaveChanges();
                            }
                            else
                            {
                                return "No Records Founds";
                            }
                        }
                        transaction.Complete();
                    }

                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }

        //Gradation List Radio button selection
        public List<AdmissionMeritList> GetGradationListDLL(string rbTentative, int id, int loginId)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;
                if (rbTentative == "Tentative")
                {
                    if (id == 12 || id == 5 || id == 6 || id == 1 || id == 2)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId //into p
                                                                                                                //where p.Count() == 0 && p.st
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                                  //join da in _db.tbl_Document_Applicant on n.ApplicationId equals da.ApplicantId
                                  //join dt in _db.tbl_DocumentType on da.DocAppId equals dt.DocumentTypeId
                                  where his.Tentative == true && his.Final == false && his.Status == 5 && n.IsActive == true

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      locationName = l.location_name,
                                      ApplicantTypes = at.ApplicantType,
                                      MarksObtained_1 = n.MarksObtained,
                                      Qualification = q.QualificationId,
                                      Qual = q.Qualification,
                                      locationId = l.location_id,
                                      Rank = his.Rank,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();
                        #region commented code
                        //foreach (var p in Notifs)
                        //{
                        //    if (p.DiffAbled == true)
                        //    {
                        //        p.DiffrentAbled = "Yes";
                        //    }

                        //    else
                        //    {
                        //        p.DiffrentAbled = "No";
                        //    }
                        //    var resid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (resid != 0)
                        //        p.ExService = "Yes";
                        //    else
                        //        p.ExService = "No";

                        //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (ewsid != 0)
                        //        p.EconomicWeekerSec = "Yes";
                        //    else
                        //        p.EconomicWeekerSec = "No";

                        //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                   select xx.ReservationId).FirstOrDefault();
                        //    if (KanndaM != 0)
                        //        p.KanndaMedium = "Yes";
                        //    else
                        //        p.KanndaMedium = "No";
                        //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                         select xx.ReservationId).FirstOrDefault();
                        //    if (HydKarnRegion != 0)
                        //        p.HyderabadKarnatakaRegion = "Yes";
                        //    else
                        //        p.HyderabadKarnatakaRegion = "No";

                        //    if (p.locationId == 1)
                        //    {
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        if (p.Total > p.MaxMarks)
                        //        {
                        //            p.Total = p.MaxMarks;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //    }
                        //}
                        #endregion
                    }
                    //Applicant login
                    else if (id == 10)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                                  where his.Tentative == true && his.Status == 5 && n.CredatedBy == loginId && n.IsActive == true

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      locationName = l.location_name,
                                      ApplicantTypes = at.ApplicantType,
                                      MarksObtained_1 = n.MarksObtained,
                                      Qualification = q.QualificationId,
                                      Qual = q.Qualification,
                                      Rank = his.Rank,
                                      locationId = l.location_id,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();
                        #region commented code
                        //foreach (var p in Notifs)
                        //{
                        //    if (p.DiffAbled == true)
                        //    {
                        //        p.DiffrentAbled = "Yes";
                        //    }

                        //    else
                        //    {
                        //        p.DiffrentAbled = "No";
                        //    }
                        //    var resid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (resid != 0)
                        //        p.ExService = "Yes";
                        //    else
                        //        p.ExService = "No";

                        //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (ewsid != 0)
                        //        p.EconomicWeekerSec = "Yes";
                        //    else
                        //        p.EconomicWeekerSec = "No";

                        //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                   select xx.ReservationId).FirstOrDefault();
                        //    if (KanndaM != 0)
                        //        p.KanndaMedium = "Yes";
                        //    else
                        //        p.KanndaMedium = "No";
                        //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                         select xx.ReservationId).FirstOrDefault();
                        //    if (HydKarnRegion != 0)
                        //        p.HyderabadKarnatakaRegion = "Yes";
                        //    else
                        //        p.HyderabadKarnatakaRegion = "No";

                        //    if (p.locationId == 1)
                        //    {
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        if (p.Total > p.MaxMarks)
                        //        {
                        //            p.Total = p.MaxMarks;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //    }
                        //}
                        #endregion
                    }
                }
                else
                {
                    if (id == 10)
                    {
                        Notifs = (from t in _db.tbl_GradationRank_Trans
                                  join n in _db.tbl_Applicant_Detail on t.ApplicantId equals n.ApplicationId
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  //join da in _db.tbl_Document_Applicant on n.ApplicationId equals da.ApplicantId
                                  //join dt in _db.tbl_DocumentType on da.DocAppId equals dt.DocumentTypeId
                                  //where t.Status==7
                                  where t.Tentative == false && t.Final == true && t.Status == 2 && n.CredatedBy == loginId && n.IsActive == true
                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      locationId = l.location_id,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      locationName = l.location_name,
                                      MarksObtained_1 = n.MarksObtained,
                                      Rank = t.Rank,
                                      Qualification = q.QualificationId,
                                      Qual = q.Qualification,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();

                        #region commented code
                        //foreach (var p in Notifs)
                        //{
                        //    if (p.DiffAbled == true)
                        //    {
                        //        p.DiffrentAbled = "Yes";
                        //    }

                        //    else
                        //    {
                        //        p.DiffrentAbled = "No";
                        //    }
                        //    var resid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (resid != 0)
                        //        p.ExService = "Yes";
                        //    else
                        //        p.ExService = "No";

                        //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (ewsid != 0)
                        //        p.EconomicWeekerSec = "Yes";
                        //    else
                        //        p.EconomicWeekerSec = "No";

                        //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                   select xx.ReservationId).FirstOrDefault();
                        //    if (KanndaM != 0)
                        //        p.KanndaMedium = "Yes";
                        //    else
                        //        p.KanndaMedium = "No";
                        //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                         select xx.ReservationId).FirstOrDefault();
                        //    if (HydKarnRegion != 0)
                        //        p.HyderabadKarnatakaRegion = "Yes";
                        //    else
                        //        p.HyderabadKarnatakaRegion = "No";


                        //    if (p.locationId == 1)
                        //    {
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        if (p.Total > p.MaxMarks)
                        //        {
                        //            p.Total = p.MaxMarks;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //    }
                        //}
                        #endregion
                    }
                    else
                    {
                        Notifs = (from t in _db.tbl_GradationRank_Trans
                                  join n in _db.tbl_Applicant_Detail on t.ApplicantId equals n.ApplicationId
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  //join da in _db.tbl_Document_Applicant on n.ApplicationId equals da.ApplicantId
                                  //join dt in _db.tbl_DocumentType on da.DocAppId equals dt.DocumentTypeId
                                  //where t.Status==7
                                  where t.Tentative == false && t.Final == true && n.IsActive == true
                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      locationId = l.location_id,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      locationName = l.location_name,
                                      MarksObtained_1 = n.MarksObtained,
                                      Rank = t.Rank,
                                      Qualification = q.QualificationId,
                                      Qual = q.Qualification,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();

                        #region commented code
                        //foreach (var p in Notifs)
                        //{
                        //    if (p.DiffAbled == true)
                        //    {
                        //        p.DiffrentAbled = "Yes";
                        //    }

                        //    else
                        //    {
                        //        p.DiffrentAbled = "No";
                        //    }
                        //    var resid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (resid != 0)
                        //        p.ExService = "Yes";
                        //    else
                        //        p.ExService = "No";

                        //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (ewsid != 0)
                        //        p.EconomicWeekerSec = "Yes";
                        //    else
                        //        p.EconomicWeekerSec = "No";

                        //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                   select xx.ReservationId).FirstOrDefault();
                        //    if (KanndaM != 0)
                        //        p.KanndaMedium = "Yes";
                        //    else
                        //        p.KanndaMedium = "No";
                        //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                         select xx.ReservationId).FirstOrDefault();
                        //    if (HydKarnRegion != 0)
                        //        p.HyderabadKarnatakaRegion = "Yes";
                        //    else
                        //        p.HyderabadKarnatakaRegion = "No";

                        //    if (p.locationId == 1)
                        //    {
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        if (p.Total > p.MaxMarks)
                        //        {
                        //            p.Total = p.MaxMarks;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        ////age = age / 365;
                        //        p.age = age;
                        //    }
                        //}
                        #endregion
                    }
                }
                updateApplicant(ref Notifs);
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Index Gradation List Radio button selection
        public List<AdmissionMeritList> GetIndexGradationMeritListDLL(string rbTentative)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;
                if (rbTentative == "Tentative")
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId //into p
                                                                                                            //where p.Count() == 0 && p.st
                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                              join c in _db.tbl_Category on n.Category equals c.CategoryId
                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                              join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                              //join da in _db.tbl_Document_Applicant on n.ApplicationId equals da.ApplicantId
                              //join dt in _db.tbl_DocumentType on da.DocAppId equals dt.DocumentTypeId
                              where his.Tentative == true && his.Final == false && his.Status == 5

                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  FathersName = n.FathersName,
                                  Gender = g.Gender,
                                  DOB = n.DOB,
                                  Category = n.Category,
                                  CategoryName = c.Category,
                                  MaxMarks = n.MaxMarks,
                                  MarksObtained = n.MarksObtained,
                                  Result = r.Result,
                                  locationName = l.location_name,
                                  ApplicantTypes = at.ApplicantType,
                                  MarksObtained_1 = n.MarksObtained,
                                  Qualification = q.QualificationId,
                                  Qual = q.Qualification,
                                  locationId = l.location_id,
                                  Rank = his.Rank,
                                  Percentage = t.Percentage,
                                  Weitage = t.Weitage,
                                  Total = t.Total,
                                  KMedium = n.KanndaMedium,
                                  HydKarRegion = n.HyderabadKarnatakaRegion,
                                  DiffAbled = n.PhysicallyHanidcapInd,
                                  EcoWeakSection = n.EconomyWeakerSection,
                                  ExServiceMan = n.ExServiceMan
                              }).Distinct().ToList();

                    #region commented code
                    //foreach (var p in Notifs)
                    //{
                    //    if (p.DiffAbled == true)
                    //    {
                    //        p.DiffrentAbled = "Yes";
                    //    }

                    //    else
                    //    {
                    //        p.DiffrentAbled = "No";
                    //    }
                    //    var resid = (from qq in _db.tbl_Applicant_Reservation
                    //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                    //                 select xx.ReservationId).FirstOrDefault();
                    //    if (resid != 0)
                    //        p.ExService = "Yes";
                    //    else
                    //        p.ExService = "No";

                    //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                    //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                    //                 select xx.ReservationId).FirstOrDefault();
                    //    if (ewsid != 0)
                    //        p.EconomicWeekerSec = "Yes";
                    //    else
                    //        p.EconomicWeekerSec = "No";

                    //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                    //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                   select xx.ReservationId).FirstOrDefault();
                    //    if (KanndaM != 0)
                    //        p.KanndaMedium = "Yes";
                    //    else
                    //        p.KanndaMedium = "No";
                    //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                    //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                         select xx.ReservationId).FirstOrDefault();
                    //    if (HydKarnRegion != 0)
                    //        p.HyderabadKarnatakaRegion = "Yes";
                    //    else
                    //        p.HyderabadKarnatakaRegion = "No";

                    //    if (p.locationId == 1)
                    //    {
                    //        int age = 0;
                    //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //        ////age = age / 365;
                    //        p.age = age;
                    //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                    //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                    //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //        if (p.Total > p.MaxMarks)
                    //        {
                    //            p.Total = p.MaxMarks;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                    //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                    //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //        int age = 0;
                    //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //        ////age = age / 365;
                    //        p.age = age;
                    //    }
                    //}
                    #endregion
                }
                else
                {
                    Notifs = (from t in _db.tbl_GradationRank_Trans
                              join n in _db.tbl_Applicant_Detail on t.ApplicantId equals n.ApplicationId
                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                              join c in _db.tbl_Category on n.Category equals c.CategoryId
                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                              //join da in _db.tbl_Document_Applicant on n.ApplicationId equals da.ApplicantId
                              //join dt in _db.tbl_DocumentType on da.DocAppId equals dt.DocumentTypeId
                              //where t.Status==7
                              where t.Tentative == false && t.Final == true
                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  FathersName = n.FathersName,
                                  Gender = g.Gender,
                                  DOB = n.DOB,
                                  Category = n.Category,
                                  CategoryName = c.Category,
                                  MaxMarks = n.MaxMarks,
                                  locationId = l.location_id,
                                  MarksObtained = n.MarksObtained,
                                  Result = r.Result,
                                  locationName = l.location_name,
                                  MarksObtained_1 = n.MarksObtained,
                                  Rank = t.Rank,
                                  Qualification = q.QualificationId,
                                  Qual = q.Qualification,
                                  Percentage = t.Percentage,
                                  Weitage = t.Weitage,
                                  Total = t.Total,
                                  KMedium = n.KanndaMedium,
                                  HydKarRegion = n.HyderabadKarnatakaRegion,
                                  DiffAbled = n.PhysicallyHanidcapInd,
                                  EcoWeakSection = n.EconomyWeakerSection,
                                  ExServiceMan = n.ExServiceMan
                              }).Distinct().ToList();

                    #region commented code
                    //    foreach (var p in Notifs)
                    //    {
                    //        if (p.DiffAbled == true)
                    //        {
                    //            p.DiffrentAbled = "Yes";
                    //        }

                    //        else
                    //        {
                    //            p.DiffrentAbled = "No";
                    //        }
                    //        var resid = (from qq in _db.tbl_Applicant_Reservation
                    //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                    //                     select xx.ReservationId).FirstOrDefault();
                    //        if (resid != 0)
                    //            p.ExService = "Yes";
                    //        else
                    //            p.ExService = "No";

                    //        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                    //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                    //                     select xx.ReservationId).FirstOrDefault();
                    //        if (ewsid != 0)
                    //            p.EconomicWeekerSec = "Yes";
                    //        else
                    //            p.EconomicWeekerSec = "No";

                    //        var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                    //                       join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                       where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                       select xx.ReservationId).FirstOrDefault();
                    //        if (KanndaM != 0)
                    //            p.KanndaMedium = "Yes";
                    //        else
                    //            p.KanndaMedium = "No";
                    //        var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                    //                             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                             select xx.ReservationId).FirstOrDefault();
                    //        if (HydKarnRegion != 0)
                    //            p.HyderabadKarnatakaRegion = "Yes";
                    //        else
                    //            p.HyderabadKarnatakaRegion = "No";

                    //        if (p.locationId == 1)
                    //        {
                    //            int age = 0;
                    //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //            ////age = age / 365;
                    //            p.age = age;
                    //            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                    //            p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                    //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //            if (p.Total > p.MaxMarks)
                    //            {
                    //                p.Total = p.MaxMarks;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                    //            p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                    //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //            int age = 0;
                    //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //            ////age = age / 365;
                    //            p.age = age;
                    //        }
                    //    }

                    #endregion
                }
                updateApplicant(ref Notifs);
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Only Tentative List Display in Index
        public List<AdmissionMeritList> GetIndexTentativeGradationMeritListDLL()
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;

                Notifs = (from n in _db.tbl_Applicant_Detail
                          join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                          join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                          join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                          join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                          join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                          join c in _db.tbl_Category on n.Category equals c.CategoryId
                          join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                          join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                          where his.Tentative == true && his.Final == false && his.Status == 5 && t.Status != 2 && n.IsActive == true

                          select new AdmissionMeritList
                          {
                              ApplicationId = n.ApplicationId,
                              ApplicantNumber = n.ApplicantNumber,
                              ApplicantName = n.ApplicantName,
                              FathersName = n.FathersName,
                              Gender = g.Gender,
                              DOB = n.DOB,
                              Category = n.Category,
                              CategoryName = c.Category,
                              MaxMarks = n.MaxMarks,
                              MarksObtained = n.MarksObtained,
                              Result = r.Result,
                              locationName = l.location_name,
                              ApplicantTypes = at.ApplicantType,
                              MarksObtained_1 = n.MarksObtained,
                              Qualification = q.QualificationId,
                              Qual = q.Qualification,
                              locationId = l.location_id,
                              Rank = t.Rank,
                              Percentage = t.Percentage,
                              Weitage = t.Weitage,
                              Total = t.Total,
                              KMedium = n.KanndaMedium,
                              HydKarRegion = n.HyderabadKarnatakaRegion,
                              DiffAbled = n.PhysicallyHanidcapInd,
                              EcoWeakSection = n.EconomyWeakerSection,
                              ExServiceMan = n.ExServiceMan
                          }).Distinct().ToList();
                var tentativeDateDetail = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.Dt_DisplayTentativeGradation).FirstOrDefault();
                //var to_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.ToDt_DocVerificationPeriod).FirstOrDefault();
                if (tentativeDateDetail != null)
                {
                    DateTime from_date1 = (DateTime)tentativeDateDetail;
                    // DateTime to_date1 = (DateTime)to_date;
                    foreach (var item in Notifs)
                    {
                        item.tentativeDate = from_date1.ToString("dd/MM/yyyy");
                        //item.To = to_date1.ToString("yyyy,MM,d");
                    }
                }
                updateApplicant(ref Notifs);
                #region commented code
                //foreach (var p in Notifs)
                //    {
                //    if (p.DOB != null)
                //    {
                //        DateTime dateFB = (DateTime)p.DOB;
                //        p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                //    }

                //    if (p.DiffAbled == true)
                //        {
                //            p.DiffrentAbled = "Yes";
                //        }

                //        else
                //        {
                //            p.DiffrentAbled = "No";
                //        }
                //        var resid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (resid != 0)
                //            p.ExService = "Yes";
                //        else
                //            p.ExService = "No";

                //        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (ewsid != 0)
                //            p.EconomicWeekerSec = "Yes";
                //        else
                //            p.EconomicWeekerSec = "No";

                //        var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                //                       join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                       where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                       select xx.ReservationId).FirstOrDefault();
                //        if (KanndaM != 0)
                //            p.KanndaMedium = "Yes";
                //        else
                //            p.KanndaMedium = "No";
                //        var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                //                             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                             select xx.ReservationId).FirstOrDefault();
                //        if (HydKarnRegion != 0)
                //            p.HyderabadKarnatakaRegion = "Yes";
                //        else
                //            p.HyderabadKarnatakaRegion = "No";

                //        if (p.locationId == 1)
                //        {
                //            int age = 0;
                //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //            ////age = age / 365;
                //            p.age = age;
                //            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //            p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //            if (p.Total > p.MaxMarks)
                //            {
                //                p.Total = p.MaxMarks;
                //            }
                //        }
                //        else
                //        {
                //            p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                //            p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //            int age = 0;
                //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //            ////age = age / 365;
                //            p.age = age;
                //        }
                //    }
                #endregion
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Only Final List Display in Index
        public List<AdmissionMeritList> GetIndexFinalGradationMeritListDLL()
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;

                Notifs = (from t in _db.tbl_GradationRank_Trans
                          join n in _db.tbl_Applicant_Detail on t.ApplicantId equals n.ApplicationId
                          join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                          join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                          join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                          join c in _db.tbl_Category on n.Category equals c.CategoryId
                          join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                          where t.Tentative == false && t.Final == true && n.IsActive == true
                          select new AdmissionMeritList
                          {
                              ApplicationId = n.ApplicationId,
                              ApplicantNumber = n.ApplicantNumber,
                              ApplicantName = n.ApplicantName,
                              FathersName = n.FathersName,
                              Gender = g.Gender,
                              DOB = n.DOB,
                              Category = n.Category,
                              CategoryName = c.Category,
                              MaxMarks = n.MaxMarks,
                              locationId = l.location_id,
                              MarksObtained = n.MarksObtained,
                              Result = r.Result,
                              locationName = l.location_name,
                              MarksObtained_1 = n.MarksObtained,
                              Rank = t.Rank,
                              Qualification = q.QualificationId,
                              Qual = q.Qualification,
                              Percentage = t.Percentage,
                              Weitage = t.Weitage,
                              Total = t.Total,
                              KMedium = n.KanndaMedium,
                              HydKarRegion = n.HyderabadKarnatakaRegion,
                              DiffAbled = n.PhysicallyHanidcapInd,
                              EcoWeakSection = n.EconomyWeakerSection,
                              ExServiceMan = n.ExServiceMan
                          }).Distinct().ToList();
                var eventFinalDateDetail = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.Dt_DisplayFinalGradationList).FirstOrDefault();
                //var to_date = _db.tbl_Tentative_admsn_eventDetails.OrderByDescending(y => y.Calendar_EventId).Select(x => x.ToDt_DocVerificationPeriod).FirstOrDefault();
                if (eventFinalDateDetail != null)
                {
                    DateTime from_date1 = (DateTime)eventFinalDateDetail;
                    // DateTime to_date1 = (DateTime)to_date;
                    foreach (var item in Notifs)
                    {
                        item.finalDate = from_date1.ToString("dd/MM/yyyy");
                        //item.To = to_date1.ToString("yyyy,MM,d");
                    }
                }
                updateApplicant(ref Notifs);
                #region commented code
                //foreach (var p in Notifs)
                //{
                //    if (p.DOB != null)
                //    {
                //        DateTime dateFB = (DateTime)p.DOB;
                //        p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                //    }

                //    if (p.DiffAbled == true)
                //    {
                //        p.DiffrentAbled = "Yes";
                //    }

                //    else
                //    {
                //        p.DiffrentAbled = "No";
                //    }
                //    var resid = (from qq in _db.tbl_Applicant_Reservation
                //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                //                 select xx.ReservationId).FirstOrDefault();
                //    if (resid != 0)
                //        p.ExService = "Yes";
                //    else
                //        p.ExService = "No";

                //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                //                 select xx.ReservationId).FirstOrDefault();
                //    if (ewsid != 0)
                //        p.EconomicWeekerSec = "Yes";
                //    else
                //        p.EconomicWeekerSec = "No";

                //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                   select xx.ReservationId).FirstOrDefault();
                //    if (KanndaM != 0)
                //        p.KanndaMedium = "Yes";
                //    else
                //        p.KanndaMedium = "No";
                //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                         select xx.ReservationId).FirstOrDefault();
                //    if (HydKarnRegion != 0)
                //        p.HyderabadKarnatakaRegion = "Yes";
                //    else
                //        p.HyderabadKarnatakaRegion = "No";

                //    if (p.locationId == 1)
                //    {
                //        int age = 0;
                //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //        ////age = age / 365;
                //        p.age = age;
                //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //        if (p.Total > p.MaxMarks)
                //        {
                //            p.Total = p.MaxMarks;
                //        }
                //    }
                //    else
                //    {
                //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //        int age = 0;
                //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //        //age = age / 365;
                //        p.age = age;
                //    }
                //}
                #endregion
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //ForwardMeritList to AD
        public string ForwardMeritListDDDLL(AdmissionMeritList model, int loginId, int roleId, string remarks, int round)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 5)
                    {
                        if (roleId == 6)
                        {
                            var Merits = (from t in _db.tbl_GradationRank_Trans
                                          where (t.Status == 5 || t.Status == 9 || t.Status == 4) && t.RoundId == round
                                          select new AdmissionMeritList
                                          {
                                              Gradation_trans_Id = t.Gradation_trans_Id,
                                              ApplicantIdTrans = t.ApplicantId,
                                              TransDate = t.TransDate,
                                              Rank = t.Rank,
                                              Tentative = t.Tentative,
                                              Status = t.Status,
                                              Remarks = t.Remarks,
                                              FlowId = t.FlowId,
                                              Final = t.Final,
                                              Weitage = t.Weitage,
                                              Total = t.Total,
                                              Percentage = t.Percentage
                                          }).Distinct().ToList();
                            foreach (var p in Merits)
                            {
                                if (Merits != null)
                                {
                                    tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                    MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                    MeritListTrans.TransDate = DateTime.Now;
                                    MeritListTrans.Status = 8;
                                    MeritListTrans.Remarks = remarks;//"Review and Recommend";
                                    MeritListTrans.CreatedOn = DateTime.Now;
                                    MeritListTrans.CreatedBy = loginId;
                                    MeritListTrans.Weitage = p.Weitage;
                                    MeritListTrans.Total = p.Total;
                                    MeritListTrans.Percentage = p.Percentage;
                                    MeritListTrans.Rank = p.Rank;
                                    MeritListTrans.FlowId = roleId;
                                    MeritListTrans.RoundId = round;
                                    _db.SaveChanges();

                                    tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                    MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                    MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                    MeritHistory.TransDate = p.TransDate;
                                    MeritHistory.Rank = p.Rank;
                                    MeritHistory.Tentative = p.Tentative;
                                    MeritHistory.Status = 8;
                                    MeritHistory.Remarks = remarks;//"Review and Recommend";
                                    MeritHistory.FlowId = roleId;//p.FlowId;
                                    MeritHistory.Final = p.Final;
                                    MeritHistory.CreatedOn = DateTime.Now;
                                    MeritHistory.CreatedBy = loginId;
                                    _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                    _db.SaveChanges();
                                }
                                else
                                {
                                    return "No Records Founds";
                                }
                            }
                        }
                        else if (roleId == 4)
                        {
                            var Merits = (from t in _db.tbl_GradationRank_Trans
                                          where t.Status == 5 || t.Status == 9 || t.Status == 4
                                          select new AdmissionMeritList
                                          {
                                              Gradation_trans_Id = t.Gradation_trans_Id,
                                              ApplicantIdTrans = t.ApplicantId,
                                              TransDate = t.TransDate,
                                              Rank = t.Rank,
                                              Tentative = t.Tentative,
                                              Status = t.Status,
                                              Remarks = t.Remarks,
                                              FlowId = t.FlowId,
                                              Final = t.Final,
                                              Weitage = t.Weitage,
                                              Total = t.Total,
                                              Percentage = t.Percentage
                                          }).Distinct().ToList();
                            foreach (var p in Merits)
                            {
                                if (Merits != null)
                                {
                                    tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                    MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                    MeritListTrans.TransDate = DateTime.Now;
                                    MeritListTrans.Status = 8;
                                    MeritListTrans.Remarks = remarks;//"Review and Recommend";
                                    MeritListTrans.CreatedOn = DateTime.Now;
                                    MeritListTrans.CreatedBy = loginId;
                                    MeritListTrans.Weitage = p.Weitage;
                                    MeritListTrans.Total = p.Total;
                                    MeritListTrans.Percentage = p.Percentage;
                                    MeritListTrans.Rank = p.Rank;
                                    MeritListTrans.FlowId = roleId;
                                    _db.SaveChanges();

                                    tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                    MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                    MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                    MeritHistory.TransDate = p.TransDate;
                                    MeritHistory.Rank = p.Rank;
                                    MeritHistory.Tentative = p.Tentative;
                                    MeritHistory.Status = 8;
                                    MeritHistory.Remarks = remarks;//"Review and Recommend";
                                    MeritHistory.FlowId = roleId;//p.FlowId;
                                    MeritHistory.Final = p.Final;
                                    MeritHistory.CreatedOn = DateTime.Now;
                                    MeritHistory.CreatedBy = loginId;
                                    _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                    _db.SaveChanges();
                                }
                                else
                                {
                                    return "No Records Founds";
                                }
                            }
                        }
                        transaction.Complete();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        //Send from AD to Director/Commissioner
        public List<AdmissionMeritList> SendforDirectorDLL(AdmissionMeritList model, int loginId, int roleId, string remarks)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    List<AdmissionMeritList> Merits = null;
                    if (loginId == 6)
                    {
                        Merits = (from t in _db.tbl_GradationRank_Trans
                                  where t.Status == 8 || t.Status == 7 || t.Status == 9 && t.FlowId == 6
                                  select new AdmissionMeritList
                                  {
                                      Gradation_trans_Id = t.Gradation_trans_Id,
                                      ApplicantIdTrans = t.ApplicantId,
                                      TransDate = t.TransDate,
                                      Rank = t.Rank,
                                      Tentative = t.Tentative,
                                      Status = t.Status,
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      Final = t.Final,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      Percentage = t.Percentage
                                  }).Distinct().ToList();
                        if (Merits != null)
                        {
                            foreach (var p in Merits)
                            {

                                tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                MeritListTrans.TransDate = DateTime.Now;
                                MeritListTrans.Status = 7;
                                MeritListTrans.Remarks = remarks;//"Sent for Review";
                                MeritListTrans.CreatedOn = DateTime.Now;
                                MeritListTrans.CreatedBy = loginId;
                                MeritListTrans.Weitage = p.Weitage;
                                MeritListTrans.Total = p.Total;
                                MeritListTrans.Percentage = p.Percentage;
                                MeritListTrans.FlowId = roleId;

                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                MeritHistory.Status = 7;
                                MeritHistory.Remarks = remarks;//"Sent for Review";
                                MeritHistory.FlowId = roleId; //p.FlowId;
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                _db.SaveChanges();

                                //else
                                //{
                                //    return "No Records Founds";
                                //}
                            }
                        }
                        Merits = (from t in _db.tbl_GradationRank_Trans
                                  join h in _db.tbl_role_master on t.CreatedBy equals h.role_id
                                  //where t.FlowId == 4 && t.Status == 8 || t.Status == 9
                                  select new AdmissionMeritList
                                  {
                                      Gradation_trans_Id = t.Gradation_trans_Id,
                                      ApplicantIdTrans = t.ApplicantId,
                                      TransDate = t.TransDate,
                                      Rank = t.Rank,
                                      Tentative = t.Tentative,
                                      Status = t.Status,
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      Final = t.Final,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      Percentage = t.Percentage,
                                      roleName = h.role_description
                                  }).Distinct().ToList();

                        transaction.Complete();
                    }

                    else if (loginId == 4)
                    {
                        Merits = (from t in _db.tbl_GradationRank_Trans
                                  join h in _db.tbl_role_master on t.CreatedBy equals h.role_id
                                  where t.FlowId == 4 && t.Status == 8 || t.Status == 9
                                  select new AdmissionMeritList
                                  {
                                      Gradation_trans_Id = t.Gradation_trans_Id,
                                      ApplicantIdTrans = t.ApplicantId,
                                      TransDate = t.TransDate,
                                      Rank = t.Rank,
                                      Tentative = t.Tentative,
                                      Status = t.Status,
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      Final = t.Final,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      Percentage = t.Percentage,
                                      roleName = h.role_description
                                  }).Distinct().ToList();

                        foreach (var p in Merits)
                        {
                            if (Merits != null)
                            {
                                tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                MeritListTrans.TransDate = DateTime.Now;
                                //if (roleId == 6)                                    
                                //    MeritListTrans.Status = 8;                                   
                                //else                                    
                                MeritListTrans.Status = 7;

                                MeritListTrans.Remarks = remarks;//"Sent for Review";
                                MeritListTrans.CreatedOn = DateTime.Now;
                                MeritListTrans.CreatedBy = loginId;
                                MeritListTrans.Weitage = p.Weitage;
                                MeritListTrans.Total = p.Total;
                                MeritListTrans.Percentage = p.Percentage;
                                MeritListTrans.FlowId = roleId;

                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                //if (roleId == 6)
                                //    MeritHistory.Status = 8;
                                //else
                                MeritHistory.Status = 7;
                                MeritHistory.Remarks = remarks;//"Sent for Review";
                                MeritHistory.FlowId = roleId; //p.FlowId;
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                _db.SaveChanges();
                            }
                        }

                        Merits = (from t in _db.tbl_GradationRank_Trans
                                  join h in _db.tbl_role_master on t.CreatedBy equals h.role_id
                                  //where t.FlowId == 4 && t.Status == 8 || t.Status == 9
                                  select new AdmissionMeritList
                                  {
                                      Gradation_trans_Id = t.Gradation_trans_Id,
                                      ApplicantIdTrans = t.ApplicantId,
                                      TransDate = t.TransDate,
                                      Rank = t.Rank,
                                      Tentative = t.Tentative,
                                      Status = t.Status,
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      Final = t.Final,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      Percentage = t.Percentage,
                                      roleName = h.role_description
                                  }).Distinct().ToList();

                        transaction.Complete();
                    }
                    else if (loginId == 2)
                    {
                        Merits = (from t in _db.tbl_GradationRank_Trans
                                  where t.Status == 7 || t.Status == 9 && t.FlowId == 2
                                  select new AdmissionMeritList
                                  {
                                      Gradation_trans_Id = t.Gradation_trans_Id,
                                      ApplicantIdTrans = t.ApplicantId,
                                      TransDate = t.TransDate,
                                      Rank = t.Rank,
                                      Tentative = t.Tentative,
                                      Status = t.Status,
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      Final = t.Final,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      Percentage = t.Percentage
                                  }).Distinct().ToList();
                        foreach (var p in Merits)
                        {
                            if (Merits != null)
                            {

                                tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                MeritListTrans.TransDate = DateTime.Now;
                                MeritListTrans.Status = 7;
                                MeritListTrans.Remarks = remarks;//"Sent for Review";
                                MeritListTrans.CreatedOn = DateTime.Now;
                                MeritListTrans.CreatedBy = loginId;
                                MeritListTrans.Weitage = p.Weitage;
                                MeritListTrans.Total = p.Total;
                                MeritListTrans.Percentage = p.Percentage;
                                MeritListTrans.FlowId = roleId;

                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                MeritHistory.Status = 7;
                                MeritHistory.Remarks = remarks;//"Sent for Review";
                                MeritHistory.FlowId = roleId;/*p.FlowId;*/
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                _db.SaveChanges();
                            }
                            //else
                            //{
                            //    return "No Records Founds";
                            //}
                        }
                        transaction.Complete();
                    }
                    return Merits;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public string ApproveMeritListDDDLL(AdmissionMeritList model, int loginId, int roleId, string remarks)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 6)
                    {

                        var Merits = (from t in _db.tbl_GradationRank_Trans
                                          //join h in _db.tbl_GradationRank_TransHistory on t.ApplicantId equals h.ApplicantId
                                      select new AdmissionMeritList
                                      {
                                          Gradation_trans_Id = t.Gradation_trans_Id,
                                          ApplicantIdTrans = t.ApplicantId,
                                          TransDate = t.TransDate,
                                          Rank = t.Rank,
                                          Tentative = t.Tentative,
                                          Status = t.Status,
                                          Remarks = t.Remarks,
                                          FlowId = t.FlowId,
                                          Final = t.Final
                                      }).Distinct().ToList();
                        foreach (var p in Merits)
                        {
                            if (Merits != null)
                            {
                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                MeritHistory.Status = p.Status;
                                MeritHistory.Remarks = p.Remarks;
                                MeritHistory.FlowId = p.FlowId;
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);

                                tbl_GradationRank_Trans iss = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                MeritHistory.Status = 2;
                                MeritHistory.Remarks = remarks;
                                //_db.tbl_GradationRank_Trans.Add(iss);
                                //MeritHistory.Status = 2;
                                //MeritHistory.Remarks = remarks;

                                _db.SaveChanges();
                            }

                        }
                        transaction.Complete();
                    }

                    if (loginId == 7)
                    {

                        var Merits = (from t in _db.tbl_GradationRank_Trans

                                      select new AdmissionMeritList
                                      {
                                          Gradation_trans_Id = t.Gradation_trans_Id,
                                          ApplicantIdTrans = t.ApplicantId,
                                          TransDate = t.TransDate,
                                          Rank = t.Rank,
                                          Tentative = t.Tentative,
                                          Status = t.Status,
                                          Remarks = t.Remarks,
                                          FlowId = t.FlowId,
                                          Final = t.Final
                                      }).Distinct().ToList();
                        foreach (var p in Merits)
                        {
                            if (Merits != null)
                            {
                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                MeritHistory.Status = p.Status;
                                MeritHistory.Remarks = p.Remarks;
                                MeritHistory.FlowId = p.FlowId;
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);

                                tbl_GradationRank_Trans iss = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                iss.Status = 2;
                                iss.Remarks = remarks;

                                //_db.tbl_GradationRank_Trans.Add(iss);

                                //iss.Status = 2;
                                //iss.Remarks = remarks;

                                _db.SaveChanges();
                            }

                        }
                        transaction.Complete();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //Additional Director View
        public List<AdmissionMeritList> GetReviewGradationMeritListDLL(int generateId, int YearId, int ApplicantTypeId, int id)
        {
            try
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYearReviewId = Convert.ToInt32(yr);
                List<AdmissionMeritList> Notifs = null;
                if (generateId == 2)
                {
                    if (id == 6)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYearReviewId && x.IsActive == true)
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  where t.Status == 7
                                  //join y in _db.tbl_Year on n.ApplyYear equals y.YearID

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      Qualification = q.QualificationId,
                                      locationId = l.location_id,
                                      locationName = l.location_name,
                                      MarksObtained_1 = n.MarksObtained,
                                      Rank = t.Rank,
                                      ApplyYear = n.ApplyYear,
                                      Qual = q.Qualification,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();
                        #region commented code
                        //foreach (var p in Notifs)
                        //{
                        //    if (p.DiffAbled == true)
                        //    {
                        //        p.DiffrentAbled = "Yes";
                        //    }
                        //    else
                        //    {
                        //        p.DiffrentAbled = "No";
                        //    }
                        //    var resid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (resid != 0)
                        //        p.ExService = "Yes";
                        //    else
                        //        p.ExService = "No";

                        //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //                 select xx.ReservationId).FirstOrDefault();
                        //    if (ewsid != 0)
                        //        p.EconomicWeekerSec = "Yes";
                        //    else
                        //        p.EconomicWeekerSec = "No";

                        //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                   select xx.ReservationId).FirstOrDefault();
                        //    if (KanndaM != 0)
                        //        p.KanndaMedium = "Yes";
                        //    else
                        //        p.KanndaMedium = "No";
                        //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                         select xx.ReservationId).FirstOrDefault();
                        //    if (HydKarnRegion != 0)
                        //        p.HyderabadKarnatakaRegion = "Yes";
                        //    else
                        //        p.HyderabadKarnatakaRegion = "No";

                        //    if (p.locationId == 1)
                        //    {
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        //age = age / 365;
                        //        p.age = age;
                        //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        if (p.Total > p.MaxMarks)
                        //        {
                        //            p.Total = p.MaxMarks;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //        int age = 0;
                        //        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //        //age = age / 365;
                        //        p.age = age;
                        //    }
                        //}
                        #endregion
                        updateApplicant(ref Notifs);
                    }
                }
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Use Case 25 - AD (Review)-06052021
        public List<AdmissionMeritList> GetGrdationListReviewADNewDLL(int id, int YearId, int ApplicantTypeId)
        {
            try
            {
                int AcademicYearAD = 0;
                if (YearId != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                    AcademicYearAD = Convert.ToInt32(yr);
                }
                List<AdmissionMeritList> Notifs = null;
                if (id == 6 || id == 4)
                {
                    if (AcademicYearAD != 0 && ApplicantTypeId != 0)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                                  join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId

                                  where n.IsActive == true && n.ApplyYear == AcademicYearAD && n.ApplicantType == ApplicantTypeId
                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      ApplyYear = n.ApplyYear,
                                      Year = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                      roundId = (int)t.RoundId,
                                      roundName = taar.RoundList,
                                      Status = s.StatusId,
                                      //StatusName = s.StatusName + " - " + v.role_DescShortForm,
                                      StatusName = (s.StatusId != (int)CmnClass.Status.SubmittedForReview && s.StatusId != (int)CmnClass.Status.Approve ? s.StatusName + " - " + v.role_DescShortForm : s.StatusId == (int)CmnClass.Status.SubmittedForReview ? CmnClass.statusName.tentativePublish : s.StatusId == (int)CmnClass.Status.Approve ? CmnClass.statusName.approved : s.StatusName),
                                      Remarks = t.Remarks,
                                      FlowId = t.FlowId,
                                      ApplicantTypeId = at.ApplicantTypeId,
                                      ApplicantTypes = at.ApplicantType,
                                      RoleId = id,
                                      //TentativeDescription =t.Tentative
                                      TentativeDescription =
                                       (
                                              t.Tentative == true ? "Final" : "Published"
                                      )
                                  }).Distinct().ToList();
                    }
                    else
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                                  join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId
                                  //where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      ApplyYear = n.ApplyYear,
                                      Year = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                      roundId = (int)t.RoundId,
                                      roundName = taar.RoundList,
                                      Status = s.StatusId,
                                      // GradationTypes=g.GradationType,
                                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                                      Remarks = t.Remarks,
                                      ApplicantTypeId = at.ApplicantTypeId,
                                      ApplicantTypes = at.ApplicantType,
                                      Final = t.Final,
                                      FlowId = t.FlowId,
                                      RoleId = id
                                  }).Distinct().ToList();
                    }
                    Notifs = Notifs.GroupBy(a => new { a.ApplyYear, a.ApplicantTypeId, a.roundId }).Select(a => new AdmissionMeritList
                    {
                        ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                        ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                        ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                        Year = a.Select(z => z.Year).FirstOrDefault(),
                        roundName = a.Select(z => z.roundName).FirstOrDefault(),
                        Status = a.Select(z => z.Status).FirstOrDefault(),
                        StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                        Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                        ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                        FlowId = a.Select(z => z.FlowId).FirstOrDefault(),
                        RoleId = a.Select(z => z.RoleId).FirstOrDefault(),
                        TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),

                    }).ToList();
                }
                #region comment
                //else if (id==4)
                //{
                //    if (AcademicYearAD != 0 && ApplicantTypeId != 0)
                //    {
                //        Notifs = (from n in _db.tbl_Applicant_Detail
                //                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                //                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                //                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                //                  where n.IsActive == true && n.ApplyYear == AcademicYearAD && n.ApplicantType == ApplicantTypeId && t.FlowId == 4 /*&& t.FlowId != 4 */&& t.FlowId != 5 && t.FlowId != 6 /*&& t.Status == 8 || t.Status == 9 || t.Status==4 */
                //                  select new AdmissionMeritList
                //                  {
                //                      ApplicationId = n.ApplicationId,
                //                      ApplicantNumber = n.ApplicantNumber,
                //                      ApplicantName = n.ApplicantName,
                //                      ApplyYear = n.ApplyYear,
                //                      Status = s.StatusId,
                //                      // GradationTypes=g.GradationType,
                //                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                //                      Remarks = t.Remarks,
                //                      FlowId = t.FlowId,
                //                      ApplicantTypeId = at.ApplicantTypeId,
                //                      ApplicantTypes = at.ApplicantType,
                //                      //TentativeDescription =t.Tentative
                //                      TentativeDescription =
                //                     (
                //                        t.Tentative == true ? "Final" : "Published"
                //                    )
                //                  }).Distinct().ToList();

                //        Notifs = Notifs.GroupBy(a => a.ApplyYear).Select(a => new AdmissionMeritList
                //        {
                //            ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                //            ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                //            ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                //            ApplyYear = a.Select(z => z.ApplyYear).FirstOrDefault(),
                //            Status = a.Select(z => z.Status).FirstOrDefault(),
                //            StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                //            Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                //            ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                //            TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),

                //        }).ToList();

                //        Notifs = Notifs.GroupBy(a => a.ApplicantType).Select(a => new AdmissionMeritList
                //        {
                //            ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                //            ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                //            ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                //            ApplyYear = a.Select(z => z.ApplyYear).FirstOrDefault(),
                //            Status = a.Select(z => z.Status).FirstOrDefault(),
                //            StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                //            Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                //            ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                //            TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),
                //        }).ToList();
                //    }
                //    else
                //    {
                //        Notifs = (from n in _db.tbl_Applicant_Detail
                //                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                //                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                //                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                //                  //where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId

                //                  select new AdmissionMeritList
                //                  {
                //                      ApplicationId = n.ApplicationId,
                //                      ApplicantNumber = n.ApplicantNumber,
                //                      ApplicantName = n.ApplicantName,
                //                      ApplyYear = n.ApplyYear,
                //                      Status = s.StatusId,
                //                      // GradationTypes=g.GradationType,
                //                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                //                      Remarks = t.Remarks,
                //                      ApplicantTypeId = at.ApplicantTypeId,
                //                      ApplicantTypes = at.ApplicantType,
                //                      Final = t.Final
                //                  }).Distinct().ToList();
                //    }
                //}
                #endregion
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetGradationMeritAppListADNewIdDLL(int YearId, int ApplicantTypeId, int ApplicationId, int id)
        {
            try
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYearAD = Convert.ToInt32(yr);
                List<AdmissionMeritList> Notifs = null; //int age = 0;
                if (id == 6 || id == 4)
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => ApplicantTypeId != 0 ? x.ApplicantType == ApplicantTypeId : true && x.ApplyYear == AcademicYearAD && x.IsActive == true)
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                              join c in _db.tbl_Category on n.Category equals c.CategoryId
                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                              where (id == 6) ? (t.FlowId == 6) : (t.Status == 8 || t.Status == 7 && t.FlowId == 4)
                              // where t.FlowId==6/*n.ApplicationId == ApplicationId && t.Status == 7*/

                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  FathersName = n.FathersName,
                                  Gender = g.Gender,
                                  DOB = n.DOB,
                                  Category = n.Category,
                                  CategoryName = c.Category,
                                  MaxMarks = n.MaxMarks,
                                  MarksObtained = n.MarksObtained,
                                  Result = r.Result,
                                  Qualification = q.QualificationId,
                                  locationId = l.location_id,
                                  locationName = l.location_name,
                                  MarksObtained_1 = n.MarksObtained,
                                  Rank = t.Rank,
                                  ApplyYear = n.ApplyYear,
                                  Qual = q.Qualification,
                                  Percentage = t.Percentage,
                                  Weitage = t.Weitage,
                                  Total = t.Total,
                                  KMedium = n.KanndaMedium,
                                  HydKarRegion = n.HyderabadKarnatakaRegion,
                                  DiffAbled = n.PhysicallyHanidcapInd,
                                  EcoWeakSection = n.EconomyWeakerSection,
                                  ExServiceMan = n.ExServiceMan
                              }).Distinct().ToList();
                    #region conmmented code
                    //foreach (var p in Notifs)
                    //{
                    //    age = 0;

                    //if (p.DOB != null)
                    //{
                    //    DateTime dateFB = (DateTime)p.DOB;
                    //    p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                    //}
                    //if (p.DiffAbled == true)
                    //{
                    //    p.DiffrentAbled = "Yes";
                    //}
                    //else
                    //{
                    //    p.DiffrentAbled = "No";
                    //}
                    //var resid = (from qq in _db.tbl_Applicant_Reservation
                    //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                    //             select xx.ReservationId).FirstOrDefault();
                    //if (resid != 0)
                    //    p.ExService = "Yes";
                    //else
                    //    p.ExService = "No";

                    //var ewsid = (from qq in _db.tbl_Applicant_Reservation
                    //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                    //             select xx.ReservationId).FirstOrDefault();
                    //if (ewsid != 0)
                    //    p.EconomicWeekerSec = "Yes";
                    //else
                    //    p.EconomicWeekerSec = "No";

                    //var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                    //               join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //               where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //               select xx.ReservationId).FirstOrDefault();
                    //if (KanndaM != 0)
                    //    p.KanndaMedium = "Yes";
                    //else
                    //    p.KanndaMedium = "No";
                    //var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                    //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                     select xx.ReservationId).FirstOrDefault();
                    //if (HydKarnRegion != 0)
                    //    p.HyderabadKarnatakaRegion = "Yes";
                    //else
                    //    p.HyderabadKarnatakaRegion = "No";

                    //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //        p.age = age;
                    //    if (p.locationId == 1)
                    //    {
                    //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                    //        p.Total = p.MarksObtained + p.Weitage;
                    //        if (p.Total > p.MaxMarks)
                    //        {
                    //            p.Total = p.MaxMarks;
                    //        }
                    //        p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                    //    }
                    //    else
                    //    {
                    //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                    //        p.Total = p.MarksObtained + p.Weitage;
                    //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //    }
                    //}
                    #endregion
                    updateApplicant(ref Notifs);
                }
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
                //var list10 = Notifs.Where(x => x.Qualification == 2).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                //var list8 = Notifs.Where(x => x.Qualification == 1).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                //List<AdmissionMeritList> listResult = new List<AdmissionMeritList>();
                //listResult.AddRange(list10);
                //listResult.AddRange(list8);
                return Notifs.OrderBy(x => x.Rank).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Director and Commissioner View
        public List<AdmissionMeritList> GetGradationMeritListDirDLL(int generateId, int YearId, int ApplicantTypeId, int id)
        {
            try
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYearDC = Convert.ToInt32(yr);
                List<AdmissionMeritList> Notifs = null;
                if (generateId == 2)
                {
                    if (id == 2 || id == 1)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYearDC && x.IsActive == true)
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                                  join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                                  join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                                  join c in _db.tbl_Category on n.Category equals c.CategoryId
                                  join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                                  where t.Status == 8
                                  //join y in _db.tbl_Year on n.ApplyYear equals y.YearID

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      FathersName = n.FathersName,
                                      Gender = g.Gender,
                                      DOB = n.DOB,
                                      Category = n.Category,
                                      CategoryName = c.Category,
                                      MaxMarks = n.MaxMarks,
                                      MarksObtained = n.MarksObtained,
                                      Result = r.Result,
                                      Qualification = q.QualificationId,
                                      locationId = l.location_id,
                                      locationName = l.location_name,
                                      MarksObtained_1 = n.MarksObtained,
                                      Rank = t.Rank,
                                      ApplyYear = n.ApplyYear,
                                      Qual = q.Qualification,
                                      Percentage = t.Percentage,
                                      Weitage = t.Weitage,
                                      Total = t.Total,
                                      KMedium = n.KanndaMedium,
                                      HydKarRegion = n.HyderabadKarnatakaRegion,
                                      DiffAbled = n.PhysicallyHanidcapInd,
                                      EcoWeakSection = n.EconomyWeakerSection,
                                      ExServiceMan = n.ExServiceMan
                                  }).Distinct().ToList();

                        //foreach (var p in Notifs)
                        //{

                        #region commented code
                        //if (p.DiffAbled == true)
                        //{
                        //    p.DiffrentAbled = "Yes";
                        //}
                        //else
                        //{
                        //    p.DiffrentAbled = "No";
                        //}
                        //var resid = (from qq in _db.tbl_Applicant_Reservation
                        //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                        //             select xx.ReservationId).FirstOrDefault();
                        //if (resid != 0)
                        //    p.ExService = "Yes";
                        //else
                        //    p.ExService = "No";

                        //var ewsid = (from qq in _db.tbl_Applicant_Reservation
                        //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                        //             select xx.ReservationId).FirstOrDefault();
                        //if (ewsid != 0)
                        //    p.EconomicWeekerSec = "Yes";
                        //else
                        //    p.EconomicWeekerSec = "No";

                        //var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                        //               join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //               where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //               select xx.ReservationId).FirstOrDefault();
                        //if (KanndaM != 0)
                        //    p.KanndaMedium = "Yes";
                        //else
                        //    p.KanndaMedium = "No";
                        //var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                        //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                        //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                        //                     select xx.ReservationId).FirstOrDefault();
                        //if (HydKarnRegion != 0)
                        //    p.HyderabadKarnatakaRegion = "Yes";
                        //else
                        //    p.HyderabadKarnatakaRegion = "No";

                        //if (p.locationId == 1)
                        //{
                        //    int age = 0;
                        //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //    //age = age / 365;
                        //    p.age = age;
                        //    p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                        //    p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                        //    p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //    if (p.Total > p.MaxMarks)
                        //    {
                        //        p.Total = p.MaxMarks;
                        //    }
                        //}
                        //else
                        //{
                        //    p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                        //    p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
                        //    p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                        //    int age = 0;
                        //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                        //    //age = age / 365;
                        //    p.age = age;
                        //}
                        #endregion
                        //}
                        updateApplicant(ref Notifs);
                    }
                }
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateApplicant(ref List<AdmissionMeritList> Notifs)
        {
            foreach (var p in Notifs)
            {
                if (p.DOB != null)
                {
                    DateTime dateFB = (DateTime)p.DOB;
                    p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                }
                p.DiffrentAbled = (p.DiffAbled != null && p.DiffAbled.Value) ? "Yes" : "No";
                //get entire list by application id
                //var reservations = (from qq in _db.tbl_Applicant_Reservation
                //                    join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                    where qq.ApplicantId == p.ApplicationId
                //                    select xx.ReservationId).ToList();
                p.EconomicWeekerSec = (p.EcoWeakSection != null && p.EcoWeakSection.Value) ? "Yes" : "No";
                p.ExService = (p.ExServiceMan != null && p.ExServiceMan.Value) ? "Yes" : "No";
                //p.ExService = (reservations.IndexOf(2) != -1) ? "Yes" : "No";
                //p.EconomicWeekerSec = (reservations.IndexOf(5) != -1) ? "Yes" : "No";
                p.KanndaMedium = (p.KMedium != null && p.KMedium.Value) ? "Yes" : "No";
                p.HyderabadKarnatakaRegion = (p.HydKarRegion != null && p.HydKarRegion.Value) ? "Yes" : "No";
                //p.KanndaMedium = (reservations.IndexOf(3) != -1) ? "Yes" : "No";
                //p.HyderabadKarnatakaRegion = (reservations.IndexOf(3) != -1) ? "Yes" : "No";
                //clean after use
                //reservations = null;
            }
        }
        public List<AdmissionMeritList> GetGrdationListReviewDirComDLL(int id, int YearId, int ApplicantTypeId)
        {
            try
            {
                int AcademicYearDC = 0;
                if (YearId != 0)
                {
                    string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                    yr = yr.Split('-')[1];
                    AcademicYearDC = Convert.ToInt32(yr);
                }
                List<AdmissionMeritList> Notifs = null;
                if (id == 2 || id == 1)
                {
                    if (AcademicYearDC != 0 && ApplicantTypeId != 0)
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                                  join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId

                                  where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId /*&& t.FlowId == 2 */&& n.IsActive == true/*t.Status==7 || t.Status==9 &&*/
                                  //join g in _db.tbl_GradationType on t.Final equals g.GradationTypeId   

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      ApplyYear = n.ApplyYear,
                                      Year = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                      roundId = (int)t.RoundId,
                                      roundName = taar.RoundList,
                                      Status = s.StatusId,
                                      // GradationTypes=g.GradationType,
                                      //StatusName = s.StatusName + " - " + v.role_DescShortForm,
                                      StatusName = (s.StatusId != (int)CmnClass.Status.SubmittedForReview && s.StatusId != (int)CmnClass.Status.Approve ? s.StatusName + " - " + v.role_DescShortForm : s.StatusId == (int)CmnClass.Status.SubmittedForReview ? CmnClass.statusName.tentativePublish : s.StatusId == (int)CmnClass.Status.Approve ? CmnClass.statusName.approved : s.StatusName),
                                      Remarks = t.Remarks,
                                      ApplicantTypeId = at.ApplicantTypeId,
                                      ApplicantTypes = at.ApplicantType,
                                      FlowId = t.FlowId,
                                      RoleId = id,
                                      TentativeDescription =
                                       (
                                              t.Tentative == true ? "Final" : "Published"
                                      )
                                  }).Distinct().ToList();
                    }

                    else
                    {
                        Notifs = (from n in _db.tbl_Applicant_Detail
                                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                                  join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId
                                  //where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId

                                  select new AdmissionMeritList
                                  {
                                      ApplicationId = n.ApplicationId,
                                      ApplicantNumber = n.ApplicantNumber,
                                      ApplicantName = n.ApplicantName,
                                      ApplyYear = n.ApplyYear,
                                      Year = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                      roundName = taar.RoundList,
                                      Status = s.StatusId,
                                      roundId = (int)t.RoundId,
                                      // GradationTypes=g.GradationType,
                                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                                      Remarks = t.Remarks,
                                      ApplicantTypeId = at.ApplicantTypeId,
                                      ApplicantTypes = at.ApplicantType,
                                      FlowId = t.FlowId,
                                      RoleId = id,
                                  }).Distinct().ToList();
                    }
                    Notifs = Notifs.GroupBy(a => new { a.ApplyYear, a.ApplicantTypeId, a.roundId }).Select(a => new AdmissionMeritList
                    {
                        ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                        ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                        ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                        Year = a.Select(z => z.Year).FirstOrDefault(),
                        roundName = a.Select(z => z.roundName).FirstOrDefault(),
                        Status = a.Select(z => z.Status).FirstOrDefault(),
                        StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                        Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                        ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                        FlowId = a.Select(z => z.FlowId).FirstOrDefault(),
                        RoleId = a.Select(z => z.RoleId).FirstOrDefault(),
                        TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),

                    }).ToList();
                }
                #region commented code
                //else if (id == 1)
                //{
                //    if (AcademicYearDC != 0 && ApplicantTypeId != 0)
                //    {
                //        Notifs = (from n in _db.tbl_Applicant_Detail
                //                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                //                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                //                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                //                  where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId && t.FlowId == 1 && t.Final != true && n.IsActive==true/*|| t.Status==9*/
                //                  //join g in _db.tbl_GradationType on t.Final equals g.GradationTypeId      

                //                  select new AdmissionMeritList
                //                  {
                //                      ApplicationId = n.ApplicationId,
                //                      ApplicantNumber = n.ApplicantNumber,
                //                      ApplicantName = n.ApplicantName,
                //                      ApplyYear = n.ApplyYear,
                //                      Status = s.StatusId,
                //                      // GradationTypes=g.GradationType,
                //                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                //                      Remarks = t.Remarks,
                //                      FlowId=t.FlowId,
                //                      ApplicantTypeId = at.ApplicantTypeId,
                //                      ApplicantTypes = at.ApplicantType,
                //                      TentativeDescription =
                //                     (
                //                        t.Tentative == true ? "Final" : "Published"
                //                    )
                //                  }).Distinct().ToList();

                //        Notifs = Notifs.GroupBy(a => a.ApplyYear).Select(a => new AdmissionMeritList
                //        {
                //            ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                //            ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                //            ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                //            ApplyYear = a.Select(z => z.ApplyYear).FirstOrDefault(),
                //            Status = a.Select(z => z.Status).FirstOrDefault(),
                //            StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                //            Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                //            ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                //            TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),

                //        }).ToList();

                //        Notifs = Notifs.GroupBy(a => a.ApplicantType).Select(a => new AdmissionMeritList
                //        {
                //            ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                //            ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                //            ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                //            ApplyYear = a.Select(z => z.ApplyYear).FirstOrDefault(),
                //            Status = a.Select(z => z.Status).FirstOrDefault(),
                //            StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                //            Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                //            ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                //            TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),
                //        }).ToList();
                //    }

                //    else
                //    {
                //        Notifs = (from n in _db.tbl_Applicant_Detail
                //                  join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //                  join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                //                  join s in _db.tbl_status_master on t.Status equals s.StatusId
                //                  join v in _db.tbl_role_master on t.FlowId equals v.role_id
                //                  //where n.ApplyYear == AcademicYearDC && n.ApplicantType == ApplicantTypeId

                //                  select new AdmissionMeritList
                //                  {
                //                      ApplicationId = n.ApplicationId,
                //                      ApplicantNumber = n.ApplicantNumber,
                //                      ApplicantName = n.ApplicantName,
                //                      ApplyYear = n.ApplyYear,
                //                      Status = s.StatusId,
                //                      // GradationTypes=g.GradationType,
                //                      StatusName = s.StatusName + " - " + v.role_DescShortForm,
                //                      Remarks = t.Remarks,
                //                      ApplicantTypeId = at.ApplicantTypeId,
                //                      ApplicantTypes = at.ApplicantType
                //                  }).Distinct().ToList();
                //    }
                //}
                #endregion
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetGradationMeritListDirNewDLL(int YearId, int ApplicantTypeId, int ApplicationId, int id)
        {
            try
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYearDC = Convert.ToInt32(yr);
                List<AdmissionMeritList> Notifs = null;//int age = 0;
                if (id == 1 || id == 2)
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => ApplicantTypeId != 0 ? x.ApplicantType == ApplicantTypeId : true && x.ApplyYear == AcademicYearDC && x.IsActive == true)
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                              join c in _db.tbl_Category on n.Category equals c.CategoryId
                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                              where (id == 2) ? (t.FlowId == 2) : (t.Status == 7 && t.FlowId == 1)
                              //where n.ApplicationId == ApplicationId
                              select new AdmissionMeritList
                              {
                                  Gradation_trans_Id = t.Gradation_trans_Id,
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  FathersName = n.FathersName,
                                  Gender = g.Gender,
                                  DOB = n.DOB,
                                  Category = n.Category,
                                  CategoryName = c.Category,
                                  MaxMarks = n.MaxMarks,
                                  MarksObtained = n.MarksObtained,
                                  Result = r.Result,
                                  Qualification = q.QualificationId,
                                  locationId = l.location_id,
                                  locationName = l.location_name,
                                  MarksObtained_1 = n.MarksObtained,
                                  Rank = t.Rank,
                                  ApplyYear = n.ApplyYear,
                                  Qual = q.Qualification,
                                  Percentage = t.Percentage,
                                  Weitage = t.Weitage,
                                  Total = t.Total,
                                  KMedium = n.KanndaMedium,
                                  HydKarRegion = n.HyderabadKarnatakaRegion,
                                  DiffAbled = n.PhysicallyHanidcapInd,
                                  EcoWeakSection = n.EconomyWeakerSection,
                                  ExServiceMan = n.ExServiceMan
                              }).Distinct().ToList();
                    #region commented code
                    //foreach (var p in Notifs)
                    //{
                    //    age = 0;
                    //    if (p.DOB != null)
                    //    {
                    //        DateTime dateFB = (DateTime)p.DOB;
                    //        p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                    //    }


                    //if (p.DiffAbled == true)
                    //{
                    //    p.DiffrentAbled = "Yes";
                    //}
                    //else
                    //{
                    //    p.DiffrentAbled = "No";
                    //}
                    //var resid = (from qq in _db.tbl_Applicant_Reservation
                    //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                    //             select xx.ReservationId).FirstOrDefault();
                    //if (resid != 0)
                    //    p.ExService = "Yes";
                    //else
                    //    p.ExService = "No";

                    //var ewsid = (from qq in _db.tbl_Applicant_Reservation
                    //             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                    //             select xx.ReservationId).FirstOrDefault();
                    //if (ewsid != 0)
                    //    p.EconomicWeekerSec = "Yes";
                    //else
                    //    p.EconomicWeekerSec = "No";

                    //var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                    //               join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //               where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //               select xx.ReservationId).FirstOrDefault();
                    //if (KanndaM != 0)
                    //    p.KanndaMedium = "Yes";
                    //else
                    //    p.KanndaMedium = "No";
                    //var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                    //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                    //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                    //                     select xx.ReservationId).FirstOrDefault();
                    //if (HydKarnRegion != 0)
                    //    p.HyderabadKarnatakaRegion = "Yes";
                    //else
                    //    p.HyderabadKarnatakaRegion = "No";

                    //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                    //    p.age = age;

                    //    if (p.locationId == 1)
                    //    {
                    //        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                    //        p.Total = p.MarksObtained + p.Weitage;
                    //        if (p.Total > p.MaxMarks)
                    //        {
                    //            p.Total = p.MaxMarks;
                    //        }
                    //        p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                    //    }
                    //    else
                    //    {
                    //        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                    //        p.Total = p.MarksObtained + p.Weitage;
                    //        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                    //    }
                    //}
                    #endregion
                    updateApplicant(ref Notifs);
                }
                #region commented by ravi sirigiri -- code reviewed

                //if (id == 2)
                //{
                //    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYearDC && x.IsActive==true)
                //              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                //              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                //              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                //              join c in _db.tbl_Category on n.Category equals c.CategoryId
                //              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                //              where  t.FlowId==2
                //              //where n.ApplicationId == ApplicationId


                //              select new AdmissionMeritList
                //              {
                //                  Gradation_trans_Id = t.Gradation_trans_Id,
                //                  ApplicationId = n.ApplicationId,
                //                  ApplicantNumber = n.ApplicantNumber,
                //                  ApplicantName = n.ApplicantName,
                //                  FathersName = n.FathersName,
                //                  Gender = g.Gender,
                //                  DOB = n.DOB,
                //                  Category = n.Category,
                //                  CategoryName = c.Category,
                //                  MaxMarks = n.MaxMarks,
                //                  MarksObtained = n.MarksObtained,
                //                  Result = r.Result,
                //                  Qualification = q.QualificationId,
                //                  locationId = l.location_id,
                //                  locationName = l.location_name,
                //                  MarksObtained_1 = n.MarksObtained,
                //                  //Rank = t.Rank,
                //                  ApplyYear = n.ApplyYear,
                //                  Qual = q.Qualification
                //              }).Distinct().ToList();

                //    foreach (var p in Notifs)
                //    {
                //        age = 0;
                //        if (p.DOB != null)
                //        {
                //            DateTime dateFB = (DateTime)p.DOB;
                //            p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                //        }

                //        if (p.DiffAbled == true)
                //        {
                //            p.DiffrentAbled = "Yes";
                //        }
                //        else
                //        {
                //            p.DiffrentAbled = "No";
                //        }
                //        var resid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (resid != 0)
                //            p.ExService = "Yes";
                //        else
                //            p.ExService = "No";

                //        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (ewsid != 0)
                //            p.EconomicWeekerSec = "Yes";
                //        else
                //            p.EconomicWeekerSec = "No";

                //        var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                //                       join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                       where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                       select xx.ReservationId).FirstOrDefault();
                //        if (KanndaM != 0)
                //            p.KanndaMedium = "Yes";
                //        else
                //            p.KanndaMedium = "No";
                //        var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                //                             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                             select xx.ReservationId).FirstOrDefault();
                //        if (HydKarnRegion != 0)
                //            p.HyderabadKarnatakaRegion = "Yes";
                //        else
                //            p.HyderabadKarnatakaRegion = "No";

                //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //            p.age = age;
                //        if (p.locationId == 1)
                //        {
                //            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //            p.Total = p.MarksObtained + p.Weitage;
                //            if (p.Total > p.MaxMarks)
                //            {
                //                p.Total = p.MaxMarks;
                //            }
                //            p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                //        }
                //        else
                //        {
                //            p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                //            p.Total = p.MarksObtained + p.Weitage;
                //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //        }
                //    }
                //}
                //else if (id == 1)
                //{
                //    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYearDC && x.IsActive==true)
                //              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                //              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                //              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                //              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                //              join c in _db.tbl_Category on n.Category equals c.CategoryId
                //              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                //              where t.Status == 7 && t.FlowId == 1
                //              //where n.ApplicationId == ApplicationId


                //              select new AdmissionMeritList
                //              {
                //                  Gradation_trans_Id = t.Gradation_trans_Id,
                //                  FlowId = t.FlowId,
                //                  ApplicantId = t.ApplicantId,
                //                  ApplicationId = n.ApplicationId,
                //                  ApplicantNumber = n.ApplicantNumber,
                //                  ApplicantName = n.ApplicantName,
                //                  FathersName = n.FathersName,
                //                  Gender = g.Gender,
                //                  DOB = n.DOB,
                //                  Category = n.Category,
                //                  CategoryName = c.Category,
                //                  MaxMarks = n.MaxMarks,
                //                  MarksObtained = n.MarksObtained,
                //                  Result = r.Result,
                //                  Qualification = q.QualificationId,
                //                  locationId = l.location_id,
                //                  locationName = l.location_name,
                //                  MarksObtained_1 = n.MarksObtained,
                //                  //Rank = t.Rank,
                //                  ApplyYear = n.ApplyYear,
                //                  Qual = q.Qualification
                //              }).Distinct().ToList();

                //    foreach (var p in Notifs)
                //    {
                //        age = 0;
                //        if (p.DOB != null)
                //        {
                //            DateTime dateFB = (DateTime)p.DOB;
                //            p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                //        }

                //        if (p.DiffAbled == true)
                //        {
                //            p.DiffrentAbled = "Yes";
                //        }
                //        else
                //        {
                //            p.DiffrentAbled = "No";
                //        }
                //        var resid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (resid != 0)
                //            p.ExService = "Yes";
                //        else
                //            p.ExService = "No";

                //        var ewsid = (from qq in _db.tbl_Applicant_Reservation
                //                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                //                     select xx.ReservationId).FirstOrDefault();
                //        if (ewsid != 0)
                //            p.EconomicWeekerSec = "Yes";
                //        else
                //            p.EconomicWeekerSec = "No";

                //        var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                //                       join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                       where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                       select xx.ReservationId).FirstOrDefault();
                //        if (KanndaM != 0)
                //            p.KanndaMedium = "Yes";
                //        else
                //            p.KanndaMedium = "No";
                //        var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                //                             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                             select xx.ReservationId).FirstOrDefault();
                //        if (HydKarnRegion != 0)
                //            p.HyderabadKarnatakaRegion = "Yes";
                //        else
                //            p.HyderabadKarnatakaRegion = "No";

                //            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //            p.age = age;
                //        if (p.locationId == 1)
                //        {
                //            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //            p.Total = p.MarksObtained + p.Weitage;
                //            if (p.Total > p.MaxMarks)
                //            {
                //                p.Total = p.MaxMarks;
                //            }
                //            p.Percentage = Math.Round((Convert.ToInt32(p.Total) / p.MaxMarks) * 100, 2);
                //        }
                //        else
                //        {
                //            p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
                //            p.Total = p.MarksObtained + p.Weitage;
                //            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //        }
                //    }
                //}
                #endregion
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
                //var list10 = Notifs.Where(x => x.Qualification == 2).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                //var list8 = Notifs.Where(x => x.Qualification == 1).OrderByDescending(x => x.Result).ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).ThenByDescending(x => x.GenderId).ToList();
                //List<AdmissionMeritList> listResult = new List<AdmissionMeritList>();
                //listResult.AddRange(list10);
                //listResult.AddRange(list8);
                return Notifs.OrderBy(x => x.Rank).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Gradation List Status
        public List<AdmissionMeritList> GetGradationListStatusDLL(int id)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;
                if (id == 12 || id == 5 || id == 6 || id == 1 || id == 2 || id == 4)
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.IsActive == true)
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                              join s in _db.tbl_status_master on t.Status equals s.StatusId
                              join v in _db.tbl_role_master on t.FlowId equals v.role_id
                              join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId

                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  ApplyYear = n.ApplyYear,
                                  YearID = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.YearID).FirstOrDefault(),
                                  Year = _db.tbl_Year.Where(a => a.Year.Contains(n.ApplyYear.ToString())).Select(a => a.Year).FirstOrDefault().ToString(),
                                  roundId = taar.ApplicantAdmissionRoundsId,
                                  roundName = taar.RoundList,
                                  Status = s.StatusId,
                                  //StatusName = s.StatusName + " - " + v.role_DescShortForm,
                                  StatusName = (s.StatusId != (int)CmnClass.Status.SubmittedForReview && s.StatusId != (int)CmnClass.Status.Approve ? s.StatusName + " - " + v.role_DescShortForm : s.StatusId == (int)CmnClass.Status.SubmittedForReview ? CmnClass.statusName.tentativePublish : s.StatusId == (int)CmnClass.Status.Approve ? CmnClass.statusName.approved : s.StatusName),
                                  Remarks = t.Remarks,
                                  ApplicantTypeId = at.ApplicantTypeId,
                                  ApplicantTypes = at.ApplicantType,
                                  FlowId = t.FlowId
                              }).Distinct().ToList();

                    Notifs = Notifs.GroupBy(a => new { a.ApplyYear, a.ApplicantTypeId, a.roundId }).Select(a => new AdmissionMeritList
                    {
                        ApplicationId = a.Select(z => z.ApplicationId).FirstOrDefault(),
                        ApplicantNumber = a.Select(z => z.ApplicantNumber).FirstOrDefault(),
                        ApplicantName = a.Select(z => z.ApplicantName).FirstOrDefault(),
                        Year = a.Select(z => z.Year).FirstOrDefault(),
                        roundName = a.Select(z => z.roundName).FirstOrDefault(),
                        Status = a.Select(z => z.Status).FirstOrDefault(),
                        StatusName = a.Select(z => z.StatusName).FirstOrDefault(),
                        Remarks = a.Select(z => z.Remarks).FirstOrDefault(),
                        ApplicantTypes = a.Select(z => z.ApplicantTypes).FirstOrDefault(),
                        FlowId = a.Select(z => z.FlowId).FirstOrDefault(),
                        YearID = a.Select(z => z.YearID).FirstOrDefault(),
                        ApplicantTypeId = a.Select(z => z.ApplicantTypeId).FirstOrDefault(),
                        roundId = a.Select(z => z.roundId).FirstOrDefault(),

                        //TentativeDescription = a.Select(z => z.TentativeDescription).FirstOrDefault(),
                    }).ToList();
                }
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //View Merit List
        public List<AdmissionMeritList> viewMeritListDll(int generateId, int YearId, int ApplicantTypeId, int round, int DivisionId, int DistrictId, int id)
        {
            try
            {
                string yr = _db.tbl_Year.Where(x => x.YearID == YearId).Select(y => y.Year).FirstOrDefault();
                yr = yr.Split('-')[1];
                int AcademicYear = Convert.ToInt32(yr);
                List<AdmissionMeritList> Notifs = null;
                //role ids should be in enumartor
                if (id == 12 || id == 4 || id == 5 || id == 6 || id == 1 || id == 2)
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                              join c in _db.tbl_Category on n.Category equals c.CategoryId
                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                              join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                              join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId
                              //join y in _db.tbl_Year on n.ApplyYear equals y.YearID
                              where (generateId == 1) ?
                                        (his.Status == 5 && his.Tentative == true) :
                                        (t.Status == 2 && his.Status == 2 && t.Final == true && t.RoundId == round)
                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  FathersName = n.FathersName,
                                  Gender = g.Gender,
                                  DOB = n.DOB,
                                  Category = n.Category,
                                  CategoryName = c.Category,
                                  MaxMarks = n.MaxMarks,
                                  MarksObtained = n.MarksObtained,
                                  Result = r.Result,
                                  locationName = l.location_name,
                                  ApplicantTypes = at.ApplicantType,
                                  MarksObtained_1 = n.MarksObtained,
                                  ApplyYear = n.ApplyYear,
                                  Rank = t.Rank,
                                  Qualification = q.QualificationId,
                                  Qual = q.Qualification,
                                  locationId = l.location_id,
                                  Weitage = t.Weitage,
                                  Percentage = t.Percentage,
                                  Total = t.Total,
                                  district_id = n.DistrictId.Value,
                                  KMedium = n.KanndaMedium,
                                  HydKarRegion = n.HyderabadKarnatakaRegion,
                                  DiffAbled = n.PhysicallyHanidcapInd,
                                  EcoWeakSection = n.EconomyWeakerSection,
                                  ExServiceMan = n.ExServiceMan,
                                  roundName = taar.RoundList
                              }).Distinct().ToList();
                }
                if (DistrictId != 0 && DivisionId != 0)
                    Notifs = Notifs.Where(x => x.district_id == DistrictId).ToList();

                updateApplicant(ref Notifs);
                return Notifs.OrderBy(x => x.Rank).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region commented by ravi sirigiri -- logic optimized on top method
        ////View Merit List
        //public List<AdmissionMeritList> viewMeritListDll(int generateId, int AcademicYear, int ApplicantTypeId, int DivisionId, int DistrictId, int id)
        //{
        //    try
        //    {
        //        List<AdmissionMeritList> Notifs = null;

        //        if (generateId == 1)
        //        {
        //            if (id == 12 || id == 5 || id == 6 || id == 1 || id == 2)
        //            {
        //                if (DistrictId != 0 && DivisionId != 0)
        //                {
        //                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.DistrictId == DistrictId && x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
        //                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
        //                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
        //                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
        //                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
        //                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
        //                              join c in _db.tbl_Category on n.Category equals c.CategoryId
        //                              join d in _db.tbl_district_master on n.DistrictId equals d.district_lgd_code
        //                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
        //                              join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
        //                              //join y in _db.tbl_Year on n.ApplyYear equals y.YearID
        //                              where his.Status == 5 && his.Tentative == true

        //                              select new AdmissionMeritList
        //                              {
        //                                  ApplicationId = n.ApplicationId,
        //                                  ApplicantNumber = n.ApplicantNumber,
        //                                  ApplicantName = n.ApplicantName,
        //                                  FathersName = n.FathersName,
        //                                  Gender = g.Gender,
        //                                  DOB = n.DOB,
        //                                  Category = n.Category,
        //                                  CategoryName = c.Category,
        //                                  MaxMarks = n.MaxMarks,
        //                                  MarksObtained = n.MarksObtained,
        //                                  Result = r.Result,
        //                                  locationName = l.location_name,
        //                                  ApplicantTypes = at.ApplicantType,
        //                                  MarksObtained_1 = n.MarksObtained,
        //                                  ApplyYear = n.ApplyYear,
        //                                  // Rank = t.Rank,
        //                                  Qualification = q.QualificationId,
        //                                  Qual = q.Qualification,
        //                                  locationId = l.location_id
        //                              }).Distinct().ToList();
        //                    foreach (var p in Notifs)
        //                    {
        //                        if (p.DiffAbled == true)
        //                        {
        //                            p.DiffrentAbled = "Yes";
        //                        }
        //                        else
        //                        {
        //                            p.DiffrentAbled = "No";
        //                        }
        //                        var resid = (from qq in _db.tbl_Applicant_Reservation
        //                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
        //                                     select xx.ReservationId).FirstOrDefault();
        //                        if (resid != 0)
        //                            p.ExService = "Yes";
        //                        else
        //                            p.ExService = "No";

        //                        var ewsid = (from qq in _db.tbl_Applicant_Reservation
        //                                     join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                     where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
        //                                     select xx.ReservationId).FirstOrDefault();
        //                        if (ewsid != 0)
        //                            p.EconomicWeekerSec = "Yes";
        //                        else
        //                            p.EconomicWeekerSec = "No";

        //                        var KanndaM = (from qq in _db.tbl_Applicant_Reservation
        //                                       join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                       where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                       select xx.ReservationId).FirstOrDefault();
        //                        if (KanndaM != 0)
        //                            p.KanndaMedium = "Yes";
        //                        else
        //                            p.KanndaMedium = "No";
        //                        var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
        //                                             join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                             where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                             select xx.ReservationId).FirstOrDefault();
        //                        if (HydKarnRegion != 0)
        //                            p.HyderabadKarnatakaRegion = "Yes";
        //                        else
        //                            p.HyderabadKarnatakaRegion = "No";


        //                        if (p.locationId == 1)
        //                        {
        //                            int age = 0;
        //                            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                            ////age = age / 365;
        //                            p.age = age;
        //                            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
        //                            p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
        //                            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                            if (p.Total > p.MaxMarks)
        //                            {
        //                                p.Total = p.MaxMarks;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
        //                            p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
        //                            p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                            int age = 0;
        //                            age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                            ////age = age / 365;
        //                            p.age = age;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
        //                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
        //                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
        //                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
        //                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
        //                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
        //                              join c in _db.tbl_Category on n.Category equals c.CategoryId
        //                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
        //                              join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
        //                              //join y in _db.tbl_Year on n.ApplyYear equals y.YearID
        //                              where his.Status == 5 && his.Tentative == true

        //                              select new AdmissionMeritList
        //                              {
        //                                  ApplicationId = n.ApplicationId,
        //                                  ApplicantNumber = n.ApplicantNumber,
        //                                  ApplicantName = n.ApplicantName,
        //                                  FathersName = n.FathersName,
        //                                  Gender = g.Gender,
        //                                  DOB = n.DOB,
        //                                  Category = n.Category,
        //                                  CategoryName = c.Category,
        //                                  MaxMarks = n.MaxMarks,
        //                                  MarksObtained = n.MarksObtained,
        //                                  Result = r.Result,
        //                                  locationName = l.location_name,
        //                                  ApplicantTypes = at.ApplicantType,
        //                                  MarksObtained_1 = n.MarksObtained,
        //                                  ApplyYear = n.ApplyYear,
        //                                  Rank = t.Rank,
        //                                  Qualification = q.QualificationId,
        //                                  Qual = q.Qualification,
        //                                  locationId = l.location_id
        //                              }).Distinct().ToList();
        //                }
        //                foreach (var p in Notifs)
        //                {
        //                    if (p.DiffAbled == true)
        //                    {
        //                        p.DiffrentAbled = "Yes";
        //                    }
        //                    else
        //                    {
        //                        p.DiffrentAbled = "No";
        //                    }
        //                    var resid = (from qq in _db.tbl_Applicant_Reservation
        //                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
        //                                 select xx.ReservationId).FirstOrDefault();
        //                    if (resid != 0)
        //                        p.ExService = "Yes";
        //                    else
        //                        p.ExService = "No";

        //                    var ewsid = (from qq in _db.tbl_Applicant_Reservation
        //                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
        //                                 select xx.ReservationId).FirstOrDefault();
        //                    if (ewsid != 0)
        //                        p.EconomicWeekerSec = "Yes";
        //                    else
        //                        p.EconomicWeekerSec = "No";

        //                    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
        //                                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                   select xx.ReservationId).FirstOrDefault();

        //                    if (KanndaM != 0)
        //                        p.KanndaMedium = "Yes";
        //                    else
        //                        p.KanndaMedium = "No";
        //                    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
        //                                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                         select xx.ReservationId).FirstOrDefault();
        //                    if (HydKarnRegion != 0)
        //                        p.HyderabadKarnatakaRegion = "Yes";
        //                    else
        //                        p.HyderabadKarnatakaRegion = "No";


        //                    if (p.locationId == 1)
        //                    {
        //                        int age = 0;
        //                        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                        ////age = age / 365;
        //                        p.age = age;
        //                        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
        //                        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
        //                        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                        if (p.Total > p.MaxMarks)
        //                        {
        //                            p.Total = p.MaxMarks;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
        //                        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
        //                        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                        int age = 0;
        //                        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                        ////age = age / 365;
        //                        p.age = age;
        //                    }
        //                }
        //            }
        //        }
        //        else if (generateId == 2)
        //        {
        //            if (id == 12 || id == 5 || id == 6 || id == 1 || id == 2)
        //            {
        //                if (DistrictId != 0 && DivisionId != 0)
        //                {
        //                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.DistrictId == DistrictId && x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
        //                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
        //                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
        //                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
        //                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
        //                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
        //                              join c in _db.tbl_Category on n.Category equals c.CategoryId
        //                              join d in _db.tbl_district_master on n.DistrictId equals d.district_lgd_code
        //                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId

        //                              where t.Status == 2 && t.Final == true

        //                              select new AdmissionMeritList
        //                              {
        //                                  ApplicationId = n.ApplicationId,
        //                                  ApplicantNumber = n.ApplicantNumber,
        //                                  ApplicantName = n.ApplicantName,
        //                                  FathersName = n.FathersName,
        //                                  Gender = g.Gender,
        //                                  DOB = n.DOB,
        //                                  Category = n.Category,
        //                                  CategoryName = c.Category,
        //                                  MaxMarks = n.MaxMarks,
        //                                  MarksObtained = n.MarksObtained,
        //                                  Result = r.Result,
        //                                  locationName = l.location_name,
        //                                  ApplicantTypes = at.ApplicantType,
        //                                  MarksObtained_1 = n.MarksObtained,
        //                                  ApplyYear = n.ApplyYear,
        //                                  Rank = t.Rank,
        //                                  locationId = l.location_id,
        //                                  Qualification = q.QualificationId,
        //                                  Qual = q.Qualification
        //                              }).Distinct().ToList();
        //                }
        //                else
        //                {
        //                    Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.ApplicantType == ApplicantTypeId && x.ApplyYear == AcademicYear && x.IsActive == true)
        //                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
        //                              join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
        //                              join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
        //                              join r in _db.tbl_Result on n.ResultQual equals r.ResultId
        //                              join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
        //                              join c in _db.tbl_Category on n.Category equals c.CategoryId
        //                              join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
        //                              //join d in _db.tbl_district_master on n.DistrictId equals d.district_lgd_code
        //                              //join y in _db.tbl_Year on n.ApplyYear equals y.YearID
        //                              where t.Status == 2 && t.Final == true

        //                              select new AdmissionMeritList
        //                              {
        //                                  ApplicationId = n.ApplicationId,
        //                                  ApplicantNumber = n.ApplicantNumber,
        //                                  ApplicantName = n.ApplicantName,
        //                                  FathersName = n.FathersName,
        //                                  Gender = g.Gender,
        //                                  DOB = n.DOB,
        //                                  Category = n.Category,
        //                                  CategoryName = c.Category,
        //                                  MaxMarks = n.MaxMarks,
        //                                  MarksObtained = n.MarksObtained,
        //                                  Result = r.Result,
        //                                  locationName = l.location_name,
        //                                  ApplicantTypes = at.ApplicantType,
        //                                  MarksObtained_1 = n.MarksObtained,
        //                                  ApplyYear = n.ApplyYear,
        //                                  Rank = t.Rank,
        //                                  Qualification = q.QualificationId,
        //                                  Qual = q.Qualification,
        //                                  locationId = l.location_id
        //                              }).Distinct().ToList();
        //                }
        //                foreach (var p in Notifs)
        //                {
        //                    if (p.DiffAbled == true)
        //                    {
        //                        p.DiffrentAbled = "Yes";
        //                    }
        //                    else
        //                    {
        //                        p.DiffrentAbled = "No";
        //                    }
        //                    var resid = (from qq in _db.tbl_Applicant_Reservation
        //                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
        //                                 select xx.ReservationId).FirstOrDefault();
        //                    if (resid != 0)
        //                        p.ExService = "Yes";
        //                    else
        //                        p.ExService = "No";

        //                    var ewsid = (from qq in _db.tbl_Applicant_Reservation
        //                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
        //                                 select xx.ReservationId).FirstOrDefault();
        //                    if (ewsid != 0)
        //                        p.EconomicWeekerSec = "Yes";
        //                    else
        //                        p.EconomicWeekerSec = "No";

        //                    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
        //                                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                   select xx.ReservationId).FirstOrDefault();

        //                    if (KanndaM != 0)
        //                        p.KanndaMedium = "Yes";
        //                    else
        //                        p.KanndaMedium = "No";
        //                    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
        //                                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
        //                                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
        //                                         select xx.ReservationId).FirstOrDefault();
        //                    if (HydKarnRegion != 0)
        //                        p.HyderabadKarnatakaRegion = "Yes";
        //                    else
        //                        p.HyderabadKarnatakaRegion = "No";


        //                    if (p.locationId == 1)
        //                    {
        //                        int age = 0;
        //                        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                        ////age = age / 365;
        //                        p.age = age;
        //                        p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
        //                        p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
        //                        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                        if (p.Total > p.MaxMarks)
        //                        {
        //                            p.Total = p.MaxMarks;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        p.Weitage = Math.Round(p.MarksObtained * 0 / 100);
        //                        p.Total = p.MarksObtained + (p.MarksObtained * 0 / 100);
        //                        p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
        //                        int age = 0;
        //                        age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
        //                        ////age = age / 365;
        //                        p.age = age;
        //                    }
        //                }
        //            }
        //        }
        //        //here qualification desc is arranged based on 10th class (2), 8th class (1) -- make enumarator
        //        return Notifs.OrderByDescending(x => x.Qualification).OrderByDescending(x => x.Result).
        //            ThenByDescending(x => x.Percentage).ThenByDescending(x => x.age).
        //            ThenByDescending(x => x.GenderId).ToList();
        //        //return Notifs.OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
        //        //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
        public string PublishMeritList(AdmissionMeritList model, int loginId, int Status, string remarks)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 1 || loginId == 2)
                    {
                        var Merits = (from t in _db.tbl_GradationRank_Trans.Where(x => (x.Status == 7 || x.Status == 9) && (x.FlowId == 1 || x.FlowId == 2))

                                      select new AdmissionMeritList
                                      {
                                          //ApplicationId=t.ApplicantId,
                                          Gradation_trans_Id = t.Gradation_trans_Id,
                                          ApplicantIdTrans = t.ApplicantId,
                                          TransDate = t.TransDate,
                                          Rank = t.Rank,
                                          Tentative = t.Tentative,
                                          Status = t.Status,
                                          Remarks = t.Remarks,
                                          FlowId = t.FlowId,
                                          Final = t.Final
                                      }).Distinct().ToList();
                        //if (id == 2)
                        //{
                        foreach (var p in Merits)
                        {
                            if (model != null && p.Status == 7 || p.Status == 9)
                            {


                                tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                MeritListTrans.TransDate = DateTime.Now;
                                MeritListTrans.Status = 2;
                                MeritListTrans.Tentative = false;
                                MeritListTrans.Final = true;
                                MeritListTrans.Rank = p.Rank;
                                MeritListTrans.Remarks = remarks;//"Approve and Publish";
                                MeritListTrans.CreatedOn = DateTime.Now;
                                MeritListTrans.CreatedBy = loginId;
                                MeritListTrans.FlowId = loginId;
                                _db.SaveChanges();
                                //_db.tbl_GradationRank_Trans.Add(MeritListTrans);

                                #region .. Updating in tbl_Applicant_Detail table .. 

                                var ApplicantLIst = (from tgt in _db.tbl_GradationRank_Trans
                                                     where tgt.Tentative == true && tgt.ApplicantId == p.ApplicantIdTrans
                                                     select new ApplicantApplicationForm
                                                     {
                                                         ApplicantId = tgt.ApplicantId
                                                     }).ToList();

                                foreach (var applID in ApplicantLIst)
                                {
                                    var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == p.ApplicantIdTrans).FirstOrDefault();
                                    update_query.ApplDescStatus = 12;        //Gradation Final List approved
                                    _db.SaveChanges();

                                    var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
                                                          where tad.ApplicationId == applID.ApplicantId && tad.IsActive == true
                                                          select new ApplicationStatusUpdate
                                                          {
                                                              CredatedBy = tad.CredatedBy,
                                                              FlowId = tad.FlowId,
                                                              AssignedVO = tad.AssignedVO,
                                                              ApplStatusTrans = tad.ApplStatus
                                                          }).ToList();

                                    tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
                                    objtbl_ApplicantTrans.ApplicantId = applID.ApplicantId;
                                    objtbl_ApplicantTrans.Remark = "Gradation Final List approved";
                                    objtbl_ApplicantTrans.FinalSubmitInd = 1;
                                    objtbl_ApplicantTrans.ApplDescStatus = 8;
                                    objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
                                    objtbl_ApplicantTrans.TransDate = DateTime.Now;
                                    objtbl_ApplicantTrans.IsActive = 1;
                                    if (FlowIDExisting.Count > 0)
                                    {
                                        foreach (var ExistingAssignedVOval in FlowIDExisting)
                                        {
                                            objtbl_ApplicantTrans.CreatedBy = ExistingAssignedVOval.CredatedBy;   //Mapping VO to Applicant trans table 
                                            objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
                                            objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
                                            objtbl_ApplicantTrans.Status = ExistingAssignedVOval.ApplStatusTrans;
                                        }
                                    }
                                    _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
                                    _db.SaveChanges();
                                }

                                #endregion

                                tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                MeritHistory.TransDate = p.TransDate;
                                MeritHistory.Rank = p.Rank;
                                MeritHistory.Tentative = p.Tentative;
                                MeritHistory.Status = 2;
                                MeritHistory.Remarks = remarks;//"Approve and Publish";
                                MeritHistory.FlowId = loginId;//p.FlowId;
                                MeritHistory.Final = p.Final;
                                MeritHistory.CreatedOn = DateTime.Now;
                                MeritHistory.CreatedBy = loginId;
                                _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                _db.SaveChanges();
                            }
                            else
                            {
                                return "No Records Founds";
                            }
                        }
                        transaction.Complete();
                        //}                       

                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }

        //public string PublishMeritList(nestedMeritList model, int loginId, int Status, string remarks)
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {
        //            if (loginId == 1 || loginId == 2)
        //            {                      
        //                foreach (var p in model.lists)
        //                {
        //                    if (model != null /*&& p.Status == 8 || p.Status == 10*/)
        //                    {


        //                        tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
        //                        MeritListTrans.ApplicantId = p.ApplicantIdTrans;
        //                        MeritListTrans.TransDate = DateTime.Now;
        //                        MeritListTrans.Status = 2;
        //                        MeritListTrans.Tentative = false;
        //                        MeritListTrans.Final = true;
        //                        MeritListTrans.Rank = p.Rank;
        //                        MeritListTrans.Remarks = remarks;//"Approve and Publish";
        //                        MeritListTrans.CreatedOn = DateTime.Now;
        //                        MeritListTrans.CreatedBy = loginId;
        //                        MeritListTrans.FlowId = loginId;
        //                        _db.SaveChanges();
        //                        //_db.tbl_GradationRank_Trans.Add(MeritListTrans);

        //                        #region .. Updating in tbl_Applicant_Detail table .. 

        //                        var ApplicantLIst = (from tgt in _db.tbl_GradationRank_Trans
        //                                             where tgt.Tentative == true && tgt.ApplicantId == p.ApplicantIdTrans
        //                                             select new ApplicantApplicationForm
        //                                             {
        //                                                 ApplicantId = tgt.ApplicantId
        //                                             }).ToList();

        //                        foreach (var applID in ApplicantLIst)
        //                        {
        //                            var update_query = _db.tbl_Applicant_Detail.Where(s => s.ApplicationId == p.ApplicantIdTrans).FirstOrDefault();
        //                            update_query.ApplDescStatus = 12;        //Gradation Final List approved
        //                            _db.SaveChanges();

        //                            var FlowIDExisting = (from tad in _db.tbl_Applicant_Detail
        //                                                  where tad.ApplicationId == applID.ApplicantId && tad.IsActive == true
        //                                                  select new ApplicationStatusUpdate
        //                                                  {
        //                                                      CredatedBy = tad.CredatedBy,
        //                                                      FlowId = tad.FlowId,
        //                                                      AssignedVO = tad.AssignedVO,
        //                                                      ApplStatusTrans = tad.ApplStatus
        //                                                  }).ToList();

        //                            tbl_ApplicantTrans objtbl_ApplicantTrans = new tbl_ApplicantTrans();
        //                            objtbl_ApplicantTrans.ApplicantId = applID.ApplicantId;
        //                            objtbl_ApplicantTrans.Remark = "Gradation Final List approved";
        //                            objtbl_ApplicantTrans.FinalSubmitInd = 1;
        //                            objtbl_ApplicantTrans.ApplDescStatus = 8;
        //                            objtbl_ApplicantTrans.CreatedOn = DateTime.Now;
        //                            objtbl_ApplicantTrans.TransDate = DateTime.Now;
        //                            objtbl_ApplicantTrans.IsActive = 1;
        //                            if (FlowIDExisting.Count > 0)
        //                            {
        //                                foreach (var ExistingAssignedVOval in FlowIDExisting)
        //                                {
        //                                    objtbl_ApplicantTrans.CreatedBy = ExistingAssignedVOval.CredatedBy;   //Mapping VO to Applicant trans table 
        //                                    objtbl_ApplicantTrans.FlowId = ExistingAssignedVOval.FlowId;
        //                                    objtbl_ApplicantTrans.AssignedVO = ExistingAssignedVOval.AssignedVO;
        //                                    objtbl_ApplicantTrans.Status = ExistingAssignedVOval.ApplStatusTrans;
        //                                }
        //                            }
        //                            _db.tbl_ApplicantTrans.Add(objtbl_ApplicantTrans);
        //                            _db.SaveChanges();
        //                        }

        //                        #endregion

        //                        tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
        //                        MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
        //                        MeritHistory.ApplicantId = p.ApplicantIdTrans;
        //                        MeritHistory.TransDate = p.TransDate;
        //                        MeritHistory.Rank = p.Rank;
        //                        MeritHistory.Tentative = p.Tentative;
        //                        MeritHistory.Status = 2;
        //                        MeritHistory.Remarks = remarks;//"Approve and Publish";
        //                        MeritHistory.FlowId = loginId;//p.FlowId;
        //                        MeritHistory.Final = p.Final;
        //                        MeritHistory.CreatedOn = DateTime.Now;
        //                        MeritHistory.CreatedBy = loginId;
        //                        _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
        //                        _db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        return "No Records Founds";
        //                    }
        //                }
        //                transaction.Complete();
        //                //}                       

        //            }
        //            return "success";
        //        }
        //        catch (Exception ex)
        //        {
        //            //transaction.Rollback();
        //            throw ex;
        //        }
        //    }
        //}

        public List<AdmissionMeritList> GetGradationMeritListDirNewRank(List<AdmissionMeritList> MaritList)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (var p in MaritList)
                    {
                        tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                        MeritListTrans.ApplicantId = p.ApplicantId;
                        //MeritListTrans.TransDate = DateTime.Now;
                        //MeritListTrans.Status = 7;//p.Status;
                        //MeritListTrans.Tentative = true;//p.Tentative;
                        //MeritListTrans.Final = false;//p.Final;
                        MeritListTrans.Rank = p.Rank;
                        //MeritListTrans.Remarks = p.Remarks;
                        //MeritListTrans.CreatedOn = DateTime.Now;
                        //MeritListTrans.CreatedBy = p.CredatedBy;
                        //MeritListTrans.FlowId = p.FlowId;
                        MeritListTrans.Weitage = p.Weitage;
                        MeritListTrans.Total = p.Total;
                        MeritListTrans.Percentage = p.Percentage;
                        _db.SaveChanges();


                        //tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                        //MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                        //MeritHistory.ApplicantId = p.ApplicantIdTrans;
                        //MeritHistory.TransDate = p.TransDate;
                        //MeritHistory.Rank = p.Rank;
                        //MeritHistory.Tentative = p.Tentative;
                        //MeritHistory.Status = 8;
                        //MeritHistory.Remarks = p.Remarks;//"Sent for Review";
                        //MeritHistory.FlowId = p.FlowId; //p.FlowId;
                        //MeritHistory.Final = p.Final;
                        //MeritHistory.CreatedOn = DateTime.Now;
                        //MeritHistory.CreatedBy = p.CredatedBy;
                        //_db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                        //_db.SaveChanges();
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return MaritList;
                    throw ex;
                }

                return MaritList;
            }
        }

        public string ChangesMeritListDLL(AdmissionMeritList model, int backId, int loginId, int Status, string remarks)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 1 || loginId == 2)
                    {
                        var Merits = (from t in _db.tbl_GradationRank_Trans/*.Where(x => x.Status != 5 && Status != 2)*/
                                      where t.Status == 7 && t.FlowId == 1 || t.FlowId == 2
                                      //join h in _db.tbl_GradationRank_TransHistory on t.ApplicantId equals h.ApplicantId
                                      select new AdmissionMeritList
                                      {
                                          //ApplicationId=t.ApplicantId,
                                          Gradation_trans_Id = t.Gradation_trans_Id,
                                          ApplicantIdTrans = t.ApplicantId,
                                          TransDate = t.TransDate,
                                          Rank = t.Rank,
                                          Tentative = t.Tentative,
                                          Status = t.Status,
                                          Remarks = t.Remarks,
                                          FlowId = t.FlowId,
                                          Final = t.Final
                                      }).Distinct().ToList();

                        foreach (var p in Merits)
                        {
                            if (p.Status == 7 && p.FlowId == 1 || p.FlowId == 2)
                            {
                                if (model != null && p.Status == 7 && p.FlowId == 1 || p.FlowId == 2)
                                {
                                    tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                    MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                    MeritListTrans.TransDate = DateTime.Now;
                                    MeritListTrans.Status = 9;
                                    MeritListTrans.Remarks = remarks;//"Sent for Correction";
                                    MeritListTrans.CreatedOn = DateTime.Now;
                                    MeritListTrans.CreatedBy = loginId;
                                    MeritListTrans.FlowId = backId;//5;
                                    _db.SaveChanges();

                                    tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                    MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                    MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                    MeritHistory.TransDate = p.TransDate;
                                    MeritHistory.Rank = p.Rank;
                                    MeritHistory.Tentative = p.Tentative;
                                    MeritHistory.Status = 9;
                                    MeritHistory.Remarks = remarks;//"Sent for Correction";
                                    MeritHistory.FlowId = backId;//5; /*p.FlowId;*/
                                    MeritHistory.Final = p.Final;
                                    MeritHistory.CreatedOn = DateTime.Now;
                                    MeritHistory.CreatedBy = loginId;
                                    _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                    _db.SaveChanges();
                                }
                                else
                                {
                                    return "No Records Founds";
                                }
                            }
                        }
                        transaction.Complete();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }
        public string SentBacktoDDMeritListDLL(AdmissionMeritList model, int loginId, int Status, int sentId, string remarks)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (loginId == 6 || sentId == 5)
                    {
                        var Merits = (from t in _db.tbl_GradationRank_Trans
                                      where t.Status == 8

                                      select new AdmissionMeritList
                                      {
                                          //ApplicationId=t.ApplicantId,
                                          Gradation_trans_Id = t.Gradation_trans_Id,
                                          ApplicantIdTrans = t.ApplicantId,
                                          TransDate = t.TransDate,
                                          Rank = t.Rank,
                                          Tentative = t.Tentative,
                                          Status = t.Status,
                                          Remarks = t.Remarks,
                                          FlowId = t.FlowId,
                                          Final = t.Final
                                      }).Distinct().ToList();

                        foreach (var p in Merits)
                        {
                            if (p.Status == 8)
                            {
                                if (model != null && p.Status == 8)
                                {


                                    tbl_GradationRank_Trans MeritListTrans = _db.tbl_GradationRank_Trans.Where(x => x.Gradation_trans_Id == p.Gradation_trans_Id).FirstOrDefault();
                                    MeritListTrans.ApplicantId = p.ApplicantIdTrans;
                                    MeritListTrans.TransDate = DateTime.Now;
                                    MeritListTrans.Status = 4;
                                    MeritListTrans.Remarks = remarks;//"Sent Back";
                                    MeritListTrans.CreatedOn = DateTime.Now;
                                    MeritListTrans.CreatedBy = loginId;
                                    MeritListTrans.FlowId = sentId;
                                    _db.SaveChanges();

                                    tbl_GradationRank_TransHistory MeritHistory = new tbl_GradationRank_TransHistory();
                                    MeritHistory.Gradation_trans_Id = p.Gradation_trans_Id;
                                    MeritHistory.ApplicantId = p.ApplicantIdTrans;
                                    MeritHistory.TransDate = p.TransDate;
                                    MeritHistory.Rank = p.Rank;
                                    MeritHistory.Tentative = p.Tentative;
                                    MeritHistory.Status = 4;
                                    MeritHistory.Remarks = remarks;//"Sent Back";
                                    MeritHistory.FlowId = sentId;/*p.FlowId;*/
                                    MeritHistory.Final = p.Final;
                                    MeritHistory.CreatedOn = DateTime.Now;
                                    MeritHistory.CreatedBy = loginId;
                                    _db.tbl_GradationRank_TransHistory.Add(MeritHistory);
                                    _db.SaveChanges();
                                }
                                else
                                {
                                    return "No Records Founds";
                                }
                            }
                        }
                        transaction.Complete();
                    }
                    return "success";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    throw ex;
                }
            }
        }

        //Applicant login
        public List<AdmissionMeritList> GetApplicantResultMarqDLL(int id, int loginId)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;
                if (id == 10)
                {
                    Notifs = (from n in _db.tbl_Applicant_Detail
                              join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                              where n.IsActive == true && n.CredatedBy == loginId

                              select new AdmissionMeritList
                              {
                                  ApplicationId = n.ApplicationId,
                                  ApplicantNumber = n.ApplicantNumber,
                                  ApplicantName = n.ApplicantName,
                                  ApplyYear = n.ApplyYear,
                                  FathersName = n.FathersName,
                                  //Gender=n.Gender
                              }).Distinct().ToList();
                }
                return Notifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AdmissionMeritList> GetMeritListstatusPopupDLL(int id, int loginId, int ApplicationId)
        {
            try
            {
                List<AdmissionMeritList> Notifs = null;
                Notifs = (from n in _db.tbl_Applicant_Detail.Where(x => x.IsActive == true)
                          join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
                          join g in _db.tbl_Gender on n.Gender equals g.Gender_Id
                          join l in _db.tbl_location_type on n.ApplicantBelongTo equals l.location_id
                          join r in _db.tbl_Result on n.ResultQual equals r.ResultId
                          join at in _db.tbl_ApplicantType on n.ApplicantType equals at.ApplicantTypeId
                          join c in _db.tbl_Category on n.Category equals c.CategoryId
                          //join d in _db.tbl_district_master on n.DistrictId equals d.district_lgd_code
                          join q in _db.tbl_qualification on n.Qualification equals q.QualificationId
                          join his in _db.tbl_GradationRank_TransHistory on n.ApplicationId equals his.ApplicantId
                          join taar in _db.tbl_ApplicantAdmissionRounds on t.RoundId equals taar.ApplicantAdmissionRoundsId

                          select new AdmissionMeritList
                          {
                              ApplicationId = n.ApplicationId,
                              ApplicantNumber = n.ApplicantNumber,
                              ApplicantName = n.ApplicantName,
                              FathersName = n.FathersName,
                              Gender = g.Gender,
                              DOB = n.DOB,
                              Category = n.Category,
                              CategoryName = c.Category,
                              MaxMarks = n.MaxMarks,
                              MarksObtained = n.MarksObtained,
                              Result = r.Result,
                              locationName = l.location_name,
                              ApplicantTypes = at.ApplicantType,
                              MarksObtained_1 = n.MarksObtained,
                              ApplyYear = n.ApplyYear,
                              Rank = t.Rank,
                              Qualification = q.QualificationId,
                              Qual = q.Qualification,
                              locationId = l.location_id,
                              Weitage = t.Weitage,
                              Percentage = t.Percentage,
                              Total = t.Total,
                              KMedium = n.KanndaMedium,
                              HydKarRegion = n.HyderabadKarnatakaRegion,
                              DiffAbled = n.PhysicallyHanidcapInd,
                              EcoWeakSection = n.EconomyWeakerSection,
                              ExServiceMan = n.ExServiceMan,
                              roundName = taar.RoundList,
                              RoleId = t.FlowId
                          }).Distinct().ToList();
                #region commented by ravi sirigiri -- code review done
                //foreach (var p in Notifs)
                //{
                //    if (p.DOB != null)
                //    {
                //        DateTime dateFB = (DateTime)p.DOB;
                //        p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                //    }

                //    if (p.DiffAbled == true)
                //    {
                //        p.DiffrentAbled = "Yes";
                //    }
                //    else
                //    {
                //        p.DiffrentAbled = "No";
                //    }
                //    var resid = (from qq in _db.tbl_Applicant_Reservation
                //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                //                 select xx.ReservationId).FirstOrDefault();
                //    if (resid != 0)
                //        p.ExService = "Yes";
                //    else
                //        p.ExService = "No";

                //    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                //                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                //                 select xx.ReservationId).FirstOrDefault();
                //    if (ewsid != 0)
                //        p.EconomicWeekerSec = "Yes";
                //    else
                //        p.EconomicWeekerSec = "No";

                //    var KanndaM = (from qq in _db.tbl_Applicant_Reservation
                //                   join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                   where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                   select xx.ReservationId).FirstOrDefault();
                //    if (KanndaM != 0)
                //        p.KanndaMedium = "Yes";
                //    else
                //        p.KanndaMedium = "No";
                //    var HydKarnRegion = (from qq in _db.tbl_Applicant_Reservation
                //                         join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                //                         where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 3
                //                         select xx.ReservationId).FirstOrDefault();
                //    if (HydKarnRegion != 0)
                //        p.HyderabadKarnatakaRegion = "Yes";
                //    else
                //        p.HyderabadKarnatakaRegion = "No";


                //if (p.locationId == 1)
                //{
                //    int age = 0;
                //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //    ////age = age / 365;
                //    p.age = age;
                //    p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //    p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                //    p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //    if (p.Total > p.MaxMarks)
                //    {
                //        p.Total = p.MaxMarks;
                //    }
                //}
                //else
                //{
                //    p.Weitage = Math.Round(p.MarksObtained * 10 / 100);
                //    p.Total = p.MarksObtained + (p.MarksObtained * 10 / 100);
                //    p.Percentage = Math.Round((p.MarksObtained / p.MaxMarks) * 100, 2);
                //    int age = 0;
                //    age = DateTime.Now.Subtract(Convert.ToDateTime(p.DOB)).Days;
                //    ////age = age / 365;
                //    p.age = age;
                //}

                //}
                #endregion
                updateApplicant(ref Notifs);
                //already rank generated.
                return Notifs.OrderBy(x => x.Rank).ToList();
                //return Notifs.Where(x => x.Qualification != 1).OrderByDescending(x => x.MarksObtained_1).ThenByDescending(x => x.age).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AdmissionMeritList> GetPublishcalendarEvents(int id)
        {
            List<AdmissionMeritList> res = new List<AdmissionMeritList>();

            res = (from aa in _db.tbl_tentative_calendar_of_events
                   join bb in _db.tbl_Tentative_admsn_eventDetails on aa.Tentative_admsn_evnt_clndr_Id equals bb.Tentative_admsn_evnt_clndr_Id
                   where aa.StatusId == 106 && aa.Course_Id == 101
                   //where aa.StatusId == 109 && aa.Course_Id == 101 --Approved after form displayed.
                   select new AdmissionMeritList
                   {
                       Dt_DisplayTentativeGradation = bb.Dt_DisplayTentativeGradation,
                   }).ToList();
            if (res != null)
            {
                DateTime Dt_DisplayTentativeGradation;
                foreach (var item in res)
                {
                    Dt_DisplayTentativeGradation = (DateTime)item.Dt_DisplayTentativeGradation;
                    item.Dt_DisplayTentativeGradationRE1 = Dt_DisplayTentativeGradation.ToString("yyyy,MM,d");
                }
            }
            return res;
        }
        #endregion


        #region Seat Allocation Review & Recommand Of Seat Matrix

        public SeatMatrixAllocationDetail GetAdmissionCalendarDetailsDLL1(int id)
        {
            try
            {
                //List<SeatMatrixAllocationDetail> res = null;
                var res = (from a in _db.tbl_SeatAllocationDetail_Seatmatrix
                           join b in _db.tbl_trade_mast on a.TradeId equals b.trade_id
                           join c in _db.tbl_iti_college_details on a.InstituteId equals c.Insitute_TypeId
                           join d in _db.tbl_district_master on c.district_id equals d.district_lgd_code
                           join e in _db.tbl_division_master on c.division_id equals e.division_id
                           join f in _db.tbl_Institute_type on c.Insitute_TypeId equals f.Institute_type_id
                           where a.AllocationId == id
                           select new SeatMatrixAllocationDetail
                           {
                               AllocationId = a.AllocationId,
                               RankNumber = a.RankNumber,
                               division_id = e.division_id,
                               division_name = e.division_name,
                               district_id = d.district_lgd_code,
                               district_ename = d.district_ename,
                               MISCode = c.MISCode,
                               InstituteType = f.Institute_type,
                               InstituteName = c.iti_college_name,
                               TradeCode = b.trade_id,
                               TradeName = b.trade_name,
                           }).OrderByDescending(x => x.AllocationId).Take(1).ToList();

                return res[0];


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public string RuleAllocationChkExistenceDLL(SeatAllocation objSeatAllocation)
        {
            int ExistingRecordForUpdate = 0; string IsExistingMsg = "";
            try
            {
                objSeatAllocation.Exam_Year = _db.tbl_Year.Where(a => a.Year.Contains(objSeatAllocation.Exam_Year.ToString())).Select(a => a.YearID).FirstOrDefault();
                ExistingRecordForUpdate = (from ram in _db.Tbl_rules_allocation_master
                                           where ram.CourseId == objSeatAllocation.CourseId && ram.Exam_Year == objSeatAllocation.Exam_Year
                                           //&& ram.CourseId == objSeatAllocation.CourseId 
                                           && ram.IsActive == true
                                           select ram.Rules_allocation_master_id).FirstOrDefault();

                if (ExistingRecordForUpdate != 0)
                {
                    IsExistingMsg = "Record Already Exists";
                }
                else
                {
                    IsExistingMsg = "Record Not Exists";
                }
            }
            catch (Exception ex)
            {
                IsExistingMsg = "Enter Exception Block" + ex.Message.ToString();
            }

            return IsExistingMsg;
        }

        public bool CheckNameAvailabilityDLL(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            if (AadhaarRollNumber == 1)
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.RollNumber == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
            else
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.AadhaarNumber == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
        }
        public bool CheckPhoneNumberAvailabilityDLL(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            if (AadhaarRollNumber == 1)
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.RollNumber == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
            else
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.PhoneNumber == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
        }
        public bool CheckEmailIdAvailabilityDll(string strName, int ApplicationId, int AadhaarRollNumber)
        {
            if (AadhaarRollNumber == 1)
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.RollNumber == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
            else
            {
                var SeachData = _db.tbl_Applicant_Detail.Where(x => x.EmailId == strName && x.IsActive == true && x.ApplicationId != ApplicationId).SingleOrDefault();
                if (SeachData != null)
                    return true;
                else
                    return false;
            }
        }

        #region AdmissionRegister
        public List<AdmissionRegister> GetDistrictsJD()
        {

            var res = (from a in _db.tbl_district_master.Where(a => a.dis_is_active == true)
                       where a.dis_is_active == true
                       select new AdmissionRegister
                       {
                           district_id = a.district_lgd_code,
                           district_ename = a.district_ename
                       }).ToList();
            return res;

        }
        public List<AdmissionRegister> GetAppRound(int roundType)
        {
            try
            {
                if (roundType == 1)
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true && a.ApplicantAdmissionRoundsId <= 3
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
                else if (roundType == 2)
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true && a.ApplicantAdmissionRoundsId > 3
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
                else
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<AdmissionRegister> GetAppRoundReg(int applicantType, int? roundType)
        {
            try
            {
                if (applicantType == 1)
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true && a.ApplicantAdmissionRoundsId <= 3
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
                else if (applicantType == 2)
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true && a.ApplicantAdmissionRoundsId > 3
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
                else
                {
                    var res = (from a in _db.tbl_ApplicantAdmissionRounds
                               where a.IsActive == true
                               select new AdmissionRegister
                               {
                                   ApplicantAdmissionRoundsId = a.ApplicantAdmissionRoundsId,
                                   RoundList = a.RoundList
                               }).ToList();
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<DistrictTalukDetails> GetInstitutesReg(int District)
        {
            try
            {
                var res = (from aa in _db.tbl_iti_college_details
                           where aa.is_active == true && aa.district_id == District
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
		public List<AdmissionRegister> GetViewAdmissionRegisterDemo(int id, int? coursetype, int? session, int? division, int? district, int? Institute, int? InstType)
		{
			try
			{
				string yr = _db.tbl_Year.Where(x => x.YearID == session).Select(y => y.Year).FirstOrDefault();
				yr = yr.Split('-')[1];
				int AcademicYear = Convert.ToInt32(yr);

				var res = (from n in _db.tbl_Applicant_Detail
									 join t in _db.tbl_GradationRank_Trans on n.ApplicationId equals t.ApplicantId
									 join Aiti in _db.tbl_Applicant_ITI_Institute_Detail on n.ApplicationId equals Aiti.ApplicationId
									 join seat in _db.tbl_SeatAllocationDetail_Seatmatrix on n.ApplicationId equals seat.ApplicantId
									 join matrix in _db.tbl_SeatAllocation_SeatMatrix on seat.AllocationId equals matrix.AllocationId
									 join iti in _db.tbl_iti_college_details on seat.InstituteId equals iti.iti_college_id
									 join d in _db.tbl_district_master on iti.district_id equals d.district_lgd_code
									 join div in _db.tbl_division_master on iti.division_id equals div.division_id
									 join tal in _db.tbl_taluk_master on iti.taluk_id equals tal.taluk_lgd_code
									 join Inst in _db.tbl_Institute_type on iti.Insitute_TypeId equals Inst.Institute_type_id
									 join gen in _db.tbl_Gender on n.Gender equals gen.Gender_Id
									 join cat in _db.tbl_Category on n.Category equals cat.CategoryId
									 join qual in _db.tbl_qualification on n.Qualification equals qual.QualificationId
									 join trade in _db.tbl_trade_mast on seat.TradeId equals trade.trade_id
									 join shi in _db.tbl_shifts on seat.ShiftId equals shi.s_id
									 join un in _db.tbl_units on seat.UnitId equals un.u_id
									 //join shi in _db.tbl_shifts on Aiti.Shiftid equals shi.s_id
									 //join un in _db.tbl_units on Aiti.Unitid equals un.u_id
									 join cst in _db.tbl_Caste on n.Caste equals cst.CasteId
									 where n.IsActive == true && Aiti.AdmittedStatus == 6 && matrix.CourseTypeId == coursetype && n.ApplyYear == AcademicYear

									 select new AdmissionRegister
									 {
										 ApplicationId = n.ApplicationId,
										 ApplicantNumber = n.ApplicantNumber,
										 ApplicantName = n.ApplicantName,
										 ApplyYear = n.ApplyYear,
										 FathersName = n.FathersName,
										 MISCode = iti.MISCode,
										 division_id = div.division_id,
										 divisionname = div.division_name,
										 district_id = d.district_lgd_code,
										 districtename = d.district_ename,
										 talukename = tal.taluk_ename,
										 Institutetype = Inst.Institute_type,
										 Institute_type_id = iti.iti_college_id,
										 Institutetypeid = iti.Insitute_TypeId,
										 iticollegename = iti.iti_college_name,
										 EmailId = n.EmailId,
										 PhoneNumber = n.PhoneNumber,
										 DOB = n.DOB,
										 Gendername = gen.Gender,
										 CategoryName = cat.Category,
										 FamilyAnnIncome = n.FamilyAnnIncome,
										 MothersName = n.MothersName,
										 AadhaarNumber = n.AadhaarNumber,
										 Qualify = qual.Qualification,
										 MaxMarks = n.MaxMarks,
										 TraineePhoto = n.Photo,
										 TraineeName = n.ApplicantName,
										 StateRegistrationNumber = Aiti.StateRegistrationNumber,
										 AdmisionTime = Aiti.AdmisionTime,
										 AdmisionFee = Aiti.AdmisionFee,
										 PaymentDate = Aiti.PaymentDate,
										 ReceiptNumber = Aiti.TreasuryReceiptNumber,
										 ITIUnderPPP = Aiti.ITIUnderPPP,
										 TraineeType = Aiti.TraineeType,
										 tradename = trade.trade_name,
										 shifts = shi.shifts,
										 units = un.units,
										 DualType = Aiti.DualType,
										 RollNumber = n.RollNumber,
										 MinorityCategory = n.MinorityCategory,
										 AdmFeePaidStatus = Aiti.AdmFeePaidStatus,
										 //CasteCategory = cst.Caste,
										 IFSCCode = n.IFSCCode,
										 BankName = n.BankName,
										 AccountNumber = n.AccountNumber,
										 MinMarks = n.MinMarks,
										 Trainee = _db.tbl_TraineeType.Where(x => x.TraineeTypeId == Aiti.TraineeType).Select(y => y.TraineeType).FirstOrDefault(),
										 CasteCategory = (n.CategoryName == "" || n.CategoryName == null ? "NA" : n.CategoryName)
									 }).Distinct().ToList();

				foreach (var p in res)
				{
					if (p.PhysicallyHanidcapInd == true)
					{
						p.DisabilityName = "Yes";
					}
					else
					{
						p.DisabilityName = "No";
					}
					if (p.DualType == 1)
					{
						p.Traineeisindual = "Yes";
					}
					else
					{
						p.Traineeisindual = "No";
					}
					//if (p.TraineeType == 1)
					//{
					//	p.Trainee = "Yes";
					//}
					//else
					//{
					//	p.Trainee = "No";
					//}
					if (p.MinorityCategory == 1)
					{
						p.Minority = "Yes";
					}
					else
					{
						p.Minority = "No";
					}
					if (p.AdmFeePaidStatus == 1)
					{
						p.Feestatus = "Yes";
					}

                    else
                    {
                        p.Feestatus = "No";
                    }

                    if (p.AdmisionTime != null)
                    {
                        DateTime dateAdmi = (DateTime)p.AdmisionTime;
                        p.AdmisionDateTime = dateAdmi.ToString("dd/MM/yyyy");
                    }
                    if (p.PaymentDate != null)
                    {
                        DateTime datereceipt = (DateTime)p.PaymentDate;
                        p.PaymentDateReceipt = datereceipt.ToString("dd/MM/yyyy");
                    }

                    if (p.DOB != null)
                    {
                        DateTime dateFB = (DateTime)p.DOB;
                        p.dateofbirth = dateFB.ToString("dd/MM/yyyy");
                    }
                    if (p.DiffAbled == true)
                    {
                        p.DiffrentAbled = "Yes";
                    }

                    else
                    {
                        p.DiffrentAbled = "No";
                    }
                    var docId = (from da in _db.tbl_Document_Applicant
                                 join dt in _db.tbl_DocumentType on da.DocumentTypeId equals dt.DocumentTypeId
                                 where da.ApplicantId == p.ApplicationId && da.DocumentTypeId == 4
                                 select dt.DocumentTypeId).FirstOrDefault();
                    if (docId != 0)
                        p.DocTypeIncome = "Yes";
                    else
                        p.DocTypeIncome = "No";

                    var resid = (from qq in _db.tbl_Applicant_Reservation
                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 2
                                 select xx.ReservationId).FirstOrDefault();
                    if (resid != 0)
                        p.ExService = "Yes";
                    else
                        p.ExService = "No";

                    var ewsid = (from qq in _db.tbl_Applicant_Reservation
                                 join xx in _db.tbl_reservation on qq.ReservationId equals xx.ReservationId
                                 where qq.ApplicantId == p.ApplicationId && xx.ReservationId == 5
                                 select xx.ReservationId).FirstOrDefault();
                    if (ewsid != 0)
                        p.EconomicWeekerSec = "Yes";
                    else
                        p.EconomicWeekerSec = "No";
                    p.AcademicYearString = p.AcademicYear.ToString("yyyy");
                }

                if (division != null)
                {
                    res = res.Where(a => a.division_id == division).ToList();
                }
                if (district != null)
                {
                    res = res.Where(a => a.district_id == district).ToList();
                }
                if (Institute != null)
                {
                    res = res.Where(a => a.Institute_type_id == Institute).ToList();
                }
                if (InstType != null)
                {
                    res = res.Where(a => a.Institutetypeid == InstType).ToList();
                }

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AdmissionRegister> GetInstTypeDetails()
        {
            var res = (from a in _db.tbl_Institute_type
                       where a.IsActive == true
                       select new AdmissionRegister
                       {
                           Institutetype = a.Institute_type,
                           Institute_type_id = a.Institute_type_id
                       }).ToList();
            return res;
        }
        public List<ApplicationForm> GetStateListDetails()
        {
            var res = (from a in _db.tbl_State
                       where a.IsActive == true
                       select new ApplicationForm
                       {
                           stateId = a.StateId,
                           NameOfState = a.State
                       }).OrderBy(x => x.NameOfState).ToList();
            return res;
        }
        #endregion


        public List<UserDetails> GetsendBack(int id)
        {
            var res = (from aa in _db.tbl_user_master
                           //join trans in _db.tbl_Applicant_ITI_Institute_Detail on aa.um_id equals trans.FlowId
                       where aa.um_id != id && aa.um_is_active == true && aa.um_id == 9
                       //orderby aa.role_seniority_no descending
                       select new UserDetails
                       {
                           RoleID = aa.um_id,
                           RoleName = aa.um_name
                       }
                         ).Distinct().ToList();
            return res;
        }
        public List<UserDetails> GetForward(int id)
        {
            try
            {
                List<UserDetails> res = null;
                if (id == 37)
                {
                    res = (from aa in _db.tbl_user_master
                           where aa.um_id != id && aa.um_is_active == true && aa.um_id == 6
                           //orderby aa.role_seniority_no descending
                           select new UserDetails
                           {
                               RoleID = aa.um_id,
                               RoleName = aa.um_name
                           }
                                ).Distinct().ToList();
                }
                else
                {
                    res = (from aa in _db.tbl_user_master
                           where aa.um_id != id && aa.um_is_active == true && aa.um_id == 37
                           //orderby aa.role_seniority_no descending
                           select new UserDetails
                           {
                               RoleID = aa.um_id,
                               RoleName = aa.um_name
                           }
                             ).Distinct().ToList();
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<AdmissionUnitsShifts> GetAdmissionShiftsDetailsDLL()
        {
            var res = (from a in _db.tbl_shifts
                       where a.s_is_active == true
                       select new AdmissionUnitsShifts
                       {
                           s_id = a.s_id,
                           shifts = a.shifts
                       }).ToList();
            return res;
        }

        public List<AdmissionUnitsShifts> GetAdmissionUnitsDetailsDLL()
        {
            var res = (from a in _db.tbl_units
                       where a.u_is_active == true
                       select new AdmissionUnitsShifts
                       {
                           units = a.units,
                           u_id = a.u_id
                       }).ToList();
            return res;
        }
        public ApplicantApplicationForm GetApplicantApplicationForm()
        {
            var username = HttpContext.Current.Session["UserName"].ToString();
            var res = (from tad in _db.tbl_Applicant_Detail
                       where tad.PhoneNumber == username
                       select new ApplicantApplicationForm
                       {
                           ApplicantId = tad.ApplicationId,
                           ApplicantName = tad.ApplicantName,
                           EmailId = tad.EmailId,
                           PhoneNumber = tad.PhoneNumber
                       }).FirstOrDefault();

            return res;
        }

        public bool InsertNewApplicantRegistrationDetailsDLL(NewApplicant objNewApplicant)
        {
            bool result = false;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    //Appicant Details
                    var getresult = (from tad in _db.tbl_user_master
                                     where tad.um_mobile_no == objNewApplicant.MobileNumber
                                     orderby tad.um_creation_datetime descending
                                     select tad).Take(1).FirstOrDefault();
                    if (getresult == null)
                    {
                        tbl_Applicant_Detail objtbl_Applicant_Detail = new tbl_Applicant_Detail();

                        //Appicant Details
                        objtbl_Applicant_Detail.ApplicantName = objNewApplicant.Name;
                        objtbl_Applicant_Detail.EmailId = objNewApplicant.Email;
                        objtbl_Applicant_Detail.PhoneNumber = objNewApplicant.MobileNumber;
                        objtbl_Applicant_Detail.CreatedOn = DateTime.Now;
                        objtbl_Applicant_Detail.IsActive = true;
                        objtbl_Applicant_Detail.ApplStatus = (int)CmnClass.Status.ApplicantRegistered;
                        _db.tbl_Applicant_Detail.Add(objtbl_Applicant_Detail);
                        _db.SaveChanges();

                        tbl_user_master objtbl_user_master = new tbl_user_master();
                        objtbl_user_master.um_name = objNewApplicant.Name;
                        objtbl_user_master.um_mobile_no = objNewApplicant.MobileNumber;
                        objtbl_user_master.um_email_id = objNewApplicant.Email;
                        objtbl_user_master.um_password = objNewApplicant.NewPassword;
                        objtbl_user_master.um_is_active = true;
                        objtbl_user_master.um_creation_datetime = DateTime.Now;
                        //	objtbl_user_master.um_created_by = 0;
                        _db.tbl_user_master.Add(objtbl_user_master);
                        _db.SaveChanges();
                        var res = (from tad in _db.tbl_user_master
                                   where tad.um_is_active == true
                                   orderby tad.um_creation_datetime descending
                                   select tad).Take(1).FirstOrDefault();
                        tbl_user_mapping objtbl_user_mapping = new tbl_user_mapping();
                        objtbl_user_mapping.role_id = (int)CmnClass.Role.Applicant;
                        objtbl_user_mapping.um_id = res.um_id;
                        objtbl_user_mapping.dept_id = 1;
                        objtbl_user_mapping.sub_dept_id = 1;
                        objtbl_user_mapping.is_active = true;
                        objtbl_user_mapping.created_by = 1;
                        objtbl_user_mapping.creation_datetime = DateTime.Now;
                        _db.tbl_user_mapping.Add(objtbl_user_mapping);

                        _db.SaveChanges();

                        int UserMappingIDForRoles = (from tum in _db.tbl_user_mapping
                                                     orderby tum.creation_datetime descending
                                                     select tum.user_map_id).Take(1).FirstOrDefault();


                        int[] menuIDToInsertMenuRoles = { 8, 10, 37, 40, 36, 61 }; //Adding Menus id for user mapping

                        foreach (var id in menuIDToInsertMenuRoles)
                        {
                            MenuRoles objMenuRoles = new MenuRoles();
                            objMenuRoles.MenuId = id;
                            objMenuRoles.UserMap_Id = UserMappingIDForRoles;
                            objMenuRoles.Created_On = DateTime.Now;
                            //objMenuRoles.Created_By = loginId;
                            objMenuRoles.IsActive = true;
                            _db.MenuRoles.Add(objMenuRoles);
                            _db.SaveChanges();
                        }
                        HttpContext.Current.Session["ApplicantName"] = objNewApplicant.Name;
                    }
                    else
                    {
                        var res = (from tad in _db.tbl_user_master
                                   where tad.um_is_active == true
                                   orderby tad.um_creation_datetime descending
                                   select tad).Take(1).FirstOrDefault();


                        if (res != null)
                        {
                            res.um_name = objNewApplicant.MobileNumber;
                            res.um_password = objNewApplicant.NewPassword;
                            objNewApplicant.RegistrationNumber = GetCandidateID();
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    _db.SaveChanges();

                    result = true;

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                //mobjErrorLog.Err_Handler(ex.Message, "AdimssionDLL", "InsertNewApplicantRegistrationDetailsDLL");
                return result;
            }

            return result;
        }

        public bool InsertNewEmployeeRegistrationDetailsDLL(NewEmployee objNewApplicant)
        {
            bool result = false;
            try
            {
                using (var transaction = new TransactionScope())
                {
                    //Appicant Details
                    var getresult = (from tad in _db.tbl_user_master
                                         //where tad.um_mobile_no == objNewApplicant.MobileNumber && 
                                     where tad.um_kgid_number == objNewApplicant.EmployeeKGIDNumber
                                     select tad).Take(1).FirstOrDefault();
                    if (getresult == null)
                    {

                        tbl_user_master objtbl_user_master = new tbl_user_master();
                        objtbl_user_master.um_name = objNewApplicant.Name;
                        objtbl_user_master.um_father_name = objNewApplicant.EmployeeFatherName;
                        objtbl_user_master.um_password = objNewApplicant.NewPassword;
                        objtbl_user_master.um_mobile_no = objNewApplicant.MobileNumber;
                        objtbl_user_master.um_email_id = objNewApplicant.Email;
                        objtbl_user_master.um_is_active = true;
                        if (PropertyExists(objNewApplicant, "EmployeeKGIDNumber"))
                            objtbl_user_master.um_kgid_number = objNewApplicant.EmployeeKGIDNumber;
                        objtbl_user_master.um_dob = objNewApplicant.EmployeeDOB;
                        objtbl_user_master.um_creation_datetime = DateTime.Now;
                        objtbl_user_master.um_gender = objNewApplicant.EmployeeGender;
                        //	objtbl_user_master.um_created_by = 0;
                        _db.tbl_user_master.Add(objtbl_user_master);
                        _db.SaveChanges();
                        HttpContext.Current.Session["ApplicantName"] = objNewApplicant.Name;
                    }
                    else
                    {
                        var res = (from tad in _db.tbl_user_master
                                   where tad.um_kgid_number == objNewApplicant.EmployeeKGIDNumber || tad.um_mobile_no == objNewApplicant.MobileNumber
                                   select tad).Take(1).FirstOrDefault();


                        if (res != null)
                        {
                            res.um_password = objNewApplicant.NewPassword;
                            res.um_mobile_no = objNewApplicant.MobileNumber;
                            res.um_email_id = objNewApplicant.Email;

                        }
                        else
                        {
                            result = false;
                        }
                    }
                    _db.SaveChanges();

                    result = true;

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                //mobjErrorLog.Err_Handler(ex.Message, "AdimssionDLL", "InsertNewApplicantRegistrationDetailsDLL");
                return result;
            }

            return result;
        }

        public bool CheckApplicantExistorNot(string ApplicantMobileNo, string email)
        {
            bool isExist = false;
            var LastCID = _db.tbl_user_master
                                                 .Where(x => x.um_mobile_no == ApplicantMobileNo || x.um_email_id == email)
                                                 .Select(x => x.um_name)
                                                 .ToList();
            if ((LastCID.Count != 0))
            {
                isExist = true;
            }
            return isExist;

        }
        public string GetCandidateID()
        {
            string LastCID1 = "";
            var LastCID = _db.tbl_user_master
                                                     .OrderByDescending(x => x.um_id)
                                                     .Take(1)
                                                     .Select(x => x.um_name)
                                                     .ToList()
                                                     .FirstOrDefault();
            if (LastCID == null)
            {
                string SeqNumber = "0000000";
                var currentyr = DateTime.Now.ToString("yy");
                int l1 = 1;
                //l1 = l1 + 1;
                //SeqNumber = SeqNumber++;
                // LastCID =Convert.ToInt64(Convert.ToInt32(currentyr) +Convert.ToInt32(SeqNumber));
                LastCID1 = string.Format("{0}{1}{2}", currentyr, SeqNumber, l1).ToString();
            }
            else
            {
                long l = long.Parse(LastCID);
                l = l + 1;
                LastCID1 = l.ToString();
            }

            return LastCID1;
        }
        public bool IsUserLoggedOnElsewhere(string userId, string sid)
        {

            IEnumerable<tbl_user_history> logins = (from i in _db.tbl_user_history_mast
                                                    where i.LoggedIn == true &&
                                i.um_name == userId && i.SessionId != sid
                                                    select i).AsEnumerable();
            return logins.Any();
        }

        public static bool PropertyExists(dynamic obj, string name)
        {
            if (obj == null) return false;
            if (obj is IDictionary<string, object> dict)
            {
                return dict.ContainsKey(name);
            }
            return obj.GetType().GetProperty(name) != null;
        }
    }
}
