using DLL.DBConnection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Web.Mvc;
using System.Transactions;
using System.Data.Entity.SqlServer;
using Models.Master;
using Models.Affiliation;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using DLL.Common;

namespace DLL
{
    public class AffiliationDLL : IAffiliationDLL
    {
        private readonly DbConnection _db = new DbConnection();

        private readonly CommonDLL _CommonDLL = new CommonDLL();

        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        string SPListOfSubjectTradeStaffWise = ConfigurationManager.AppSettings["SPListOfSubjectTradeStaffWise"];

        #region Upload Affiliation Details DLL
        /// <summary>
        /// UploadAffiliationDetailsDLL
        /// </summary>
        /// <returns>String</returns>
        public UploadAffiliation UploadAffiliationDetailsDLL(DataTable dt, int UserId)
        {

            bool isSaved = false;
            bool isUpdated = false;
            UploadAffiliation output = new UploadAffiliation();
            int total = 0;
            int saved = 0;
            int failed = 0;
            int updated = 0;
            try
            {

                using (var transaction = new TransactionScope(
                    TransactionScopeOption.Required, new System.TimeSpan(0, 5, 0)))
                {
                    List<int> Iti_trades = new List<int>();
                    List<int> Iti_insti = new List<int>();

                    foreach (DataRow row in dt.Rows)
                    {
                        var _item = row.ItemArray;

                        string mis_code = _item[0].ToString().ToUpper();
                        temp_tbl_iti_college_details exists = _db.temp_Tbl_Iti_College_Details.Where(a => a.MISCode.ToUpper() == mis_code).FirstOrDefault();
                        //if (exists == "" || exists == null)
                        //if (exists !== null )
                        if (exists == null)
                        {
                            #region For New Institute
                            temp_tbl_iti_college_details add_insti = new temp_tbl_iti_college_details();
                            add_insti.MISCode = _item[0].ToString();
                            add_insti.iti_college_name = _item[1].ToString();
                            add_insti.Address = _item[2].ToString();
                            add_insti.college_address = _item[2].ToString();
                            add_insti.taluk = _item[3].ToString();
                            add_insti.district = _item[4].ToString();
                            add_insti.PinCode = _item[6].ToString();
                            add_insti.Constituency = _item[7].ToString();
                            add_insti.division = _item[8].ToString();
                            add_insti.AffiliationDate = _item[9].ToString();
                            add_insti.geo = _item[10].ToString();
                            add_insti.Insitute_Type = _item[11].ToString();
                            add_insti.location = _item[12].ToString();
                            add_insti.AidedUnaidedTrade = _item[15].ToString();
                            //trade code
                            //Trade Type
                            add_insti.CourseCode = _item[18].ToString();
                            add_insti.Units = _item[19].ToString();
                            add_insti.NoOfShifts = _item[20].ToString();
                            // add_insti.Duration = _item[21].ToString();
                            add_insti.NoofTrades = _item[22].ToString();
                            //batch size
                            add_insti.AffiliationOrderNo = _item[24].ToString();
                            add_insti.AffiliationOrderNoDate = _item[25].ToString();
                            add_insti.phone_num = _item[28].ToString();
                            add_insti.Website = _item[29].ToString();
                            add_insti.email_id = _item[30].ToString();
                            add_insti.Scheme = _item[32].ToString();
                            add_insti.creation_datetime = DateTime.Now;
                            add_insti.created_by = UserId;
                            add_insti.StatusId = (int)CsystemType.getCommon.Upload;

                            _db.temp_Tbl_Iti_College_Details.Add(add_insti);
                            _db.SaveChanges();

                            int ITICode_temp = add_insti.iti_college_id_temp;
                            Iti_insti.Add(ITICode_temp);

                            temp_tbl_ITI_Trade add_trade = new temp_tbl_ITI_Trade();
                            add_trade.CreatedBy = UserId;
                            add_trade.CreatedOn = DateTime.Now;
                            if (_item[27].ToString() != "")
                            {
                                if (_item[27].ToString().ToUpper().Trim() == "ACTIVE")
                                {
                                    add_trade.IsActive = true;
                                }
                                else
                                {
                                    add_trade.IsActive = false;
                                }
                            }

                            add_trade.ITICode = ITICode_temp;
                            add_trade.StatusId = (int)CsystemType.getCommon.Upload;
                            add_trade.TradeCode = _item[14].ToString();
                            add_trade.NoofTrades = _item[22].ToString();
                            add_trade.IsUploaded = false;

                            _db.temp_Tbl_ITI_Trades.Add(add_trade);
                            _db.SaveChanges();

                            int ITI_Trade_Id_temp = add_trade.Trade_ITI_id_temp;
                            Iti_trades.Add(ITI_Trade_Id_temp);

                            temp_tbl_ITI_Trade_Shift add_shift = new temp_tbl_ITI_Trade_Shift();
                            add_shift.CreatedBy = UserId;
                            add_shift.CreatedOn = DateTime.Now;
                            if (_item[26].ToString() != "")
                            {
                                if (_item[26].ToString().ToUpper() == "REGULAR")
                                {
                                    add_shift.Dual_System = "regular";
                                }
                                else
                                {
                                    add_shift.Dual_System = "dual";
                                }
                            }

                            add_shift.IsPPP = "no";
                            add_shift.ITI_Trade_Id_temp = ITI_Trade_Id_temp;
                            add_shift.Shift = _item[20].ToString();
                            add_shift.Units = _item[19].ToString();
                            if (_item[27].ToString() != "")
                            {
                                if (_item[27].ToString().ToUpper().Trim() == "ACTIVE")
                                {
                                    add_shift.IsActive = true;
                                }
                                else
                                {
                                    add_shift.IsActive = false;
                                }
                            }

                            _db.temp_Tbl_ITI_Trade_Shifts.Add(add_shift);
                            _db.SaveChanges();

                            #endregion

                            isSaved = true;

                        }


                        else
                        {
                            #region For Existing Institute
                            string trade_code = _item[14].ToString().ToUpper();
                            temp_tbl_ITI_Trade isTradeExist = _db.temp_Tbl_ITI_Trades.Where(a => a.ITICode == exists.iti_college_id_temp && a.TradeCode.ToUpper() == trade_code).FirstOrDefault();
                            if (isTradeExist == null)
                            {
                                temp_tbl_ITI_Trade add_trade = new temp_tbl_ITI_Trade();
                                add_trade.CreatedBy = UserId;
                                add_trade.CreatedOn = DateTime.Now;
                                if (_item[27].ToString() != "")
                                {
                                    if (_item[27].ToString().ToUpper().Trim() == "ACTIVE")
                                    {
                                        add_trade.IsActive = true;
                                    }
                                    else
                                    {
                                        add_trade.IsActive = false;
                                    }
                                }
                                add_trade.ITICode = exists.iti_college_id_temp;
                                add_trade.StatusId = (int)CsystemType.getCommon.Upload;
                                add_trade.TradeCode = _item[14].ToString();
                                //add_trade.NoofTrades = _item[22].ToString();

                                _db.temp_Tbl_ITI_Trades.Add(add_trade);
                                _db.SaveChanges();

                                int ITI_Trade_Id_temp = add_trade.Trade_ITI_id_temp;
                                Iti_trades.Add(ITI_Trade_Id_temp);

                                temp_tbl_ITI_Trade_Shift add_shift = new temp_tbl_ITI_Trade_Shift();
                                add_shift.CreatedBy = UserId;
                                add_shift.CreatedOn = DateTime.Now;
                                if (_item[26].ToString() != "")
                                {
                                    if (_item[26].ToString().ToUpper() == "REGULAR")
                                    {
                                        add_shift.Dual_System = "regular";
                                    }
                                    else
                                    {
                                        add_shift.Dual_System = "dual";
                                    }
                                }

                                add_shift.IsPPP = "no";
                                add_shift.ITI_Trade_Id_temp = ITI_Trade_Id_temp;
                                add_shift.Shift = _item[20].ToString();
                                add_shift.Units = _item[19].ToString();
                                if (_item[27].ToString() != "")
                                {
                                    if (_item[27].ToString().ToUpper().Trim() == "ACTIVE")
                                    {
                                        add_shift.IsActive = true;
                                    }
                                    else
                                    {
                                        add_shift.IsActive = false;
                                    }
                                }

                                _db.temp_Tbl_ITI_Trade_Shifts.Add(add_shift);
                                _db.SaveChanges();
                                #endregion

                                isUpdated = true;


                            }
                            else
                            {
                                string units = _item[19].ToString();
                                string shifts = _item[20].ToString();

                                temp_tbl_ITI_Trade_Shift isShiftExists = _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == isTradeExist.Trade_ITI_id_temp && a.Units == units && a.Shift == shifts).FirstOrDefault();
                                if (isShiftExists == null)
                                {
                                    temp_tbl_ITI_Trade_Shift add_shift = new temp_tbl_ITI_Trade_Shift();
                                    add_shift.CreatedBy = UserId;
                                    add_shift.CreatedOn = DateTime.Now;
                                    if (_item[26].ToString() != "")
                                    {
                                        if (_item[26].ToString().ToUpper() == "REGULAR")
                                        {
                                            add_shift.Dual_System = "regular";
                                        }
                                        else
                                        {
                                            add_shift.Dual_System = "dual";
                                        }
                                    }

                                    add_shift.IsPPP = "no";
                                    add_shift.ITI_Trade_Id_temp = isTradeExist.Trade_ITI_id_temp;
                                    add_shift.Shift = _item[20].ToString();
                                    add_shift.Units = _item[19].ToString();
                                    if (_item[27].ToString() != "")
                                    {
                                        if (_item[27].ToString().ToUpper().Trim() == "ACTIVE")
                                        {
                                            add_shift.IsActive = true;
                                        }
                                        else
                                        {
                                            add_shift.IsActive = false;
                                        }
                                    }


                                    _db.temp_Tbl_ITI_Trade_Shifts.Add(add_shift);
                                    _db.SaveChanges();


                                    isUpdated = true;
                                }
                            }
                        }

                        if (isSaved)
                        {
                            saved++;
                        }
                        else if (isUpdated)
                        {
                            updated++;
                        }
                        else
                        {
                            failed++;
                        }

                    }

                    total = saved + updated + failed;

                    foreach (var check in Iti_trades)
                    {
                        bool IsAnyShiftActive = _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == check).Any(a => a.IsActive);
                        if (!IsAnyShiftActive)
                        {
                            var deActive = _db.temp_Tbl_ITI_Trades.Where(a => a.Trade_ITI_id_temp == check).FirstOrDefault();
                            deActive.IsActive = false;

                            _db.SaveChanges();
                        }
                    }

                    foreach (var InstActive in Iti_insti)
                    {
                        bool IsTradeActive = _db.temp_Tbl_ITI_Trades.Where(a => a.ITICode == InstActive).Any(a => a.IsActive);
                        if (IsTradeActive)
                        {
                            var ActiveIns = _db.temp_Tbl_Iti_College_Details.Where(a => a.iti_college_id_temp == InstActive).FirstOrDefault();
                            ActiveIns.is_active = true;

                            _db.SaveChanges();
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            output.flag = 1;
            output.status ="<br><br> Out of " + total + " " + "Records" + " " + saved + " New Records Saved " /*+ updated + " Existing Records Updated and "*/ + failed + " Records already exists";
            return output;

        }
        #endregion

        #region  Get All Course
        /// <summary>
        /// GetCourseListDLL
        /// </summary>
        /// <returns></returns>
        public List<AffiliationCollegeCourseType> GetCourseListDLL()
        {

            var res = (from a in _db.tbl_course_type_mast.Where(a => a.is_active == true)
                       where a.is_active == true
                       select new AffiliationCollegeCourseType
                       {
                           course_id = a.course_id,
                           course_name = a.course_type_name
                       }).ToList();
            return res;
        }
        #endregion

        #region Get All Divisions

        /// <summary>
        /// GetDivisionListDLL
        /// </summary>
        /// <returns></returns>
        public List<AffiliationCollegeDivision> GetDivisionListDLL()
        {
            var res = (from a in _db.tbl_division_master.Where(a => a.division_is_active == true)
                       where a.division_is_active == true
                       select new AffiliationCollegeDivision
                       {
                           division_id = a.division_id,
                           division_name = a.division_name
                       }).ToList();
            return res;
        }
        #endregion

        #region Get All Districts by Division

        /// <summary>
        /// GetDistrictListDLL
        /// </summary>
        /// <param name="divId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDistrict> GetDistrictListDLL(int divId)
        {
            var res = (from a in _db.tbl_district_master.Where(a => a.dis_is_active == true)
                       where a.dis_is_active == true && a.division_id == divId
                       select new AffiliationCollegeDistrict
                       {
                           district_id = a.district_lgd_code,
                           district = a.district_ename
                       }).ToList();
            return res;
        }
        #endregion

        #region Get Trades
        /// <summary>
        /// GetTradesDLL
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeTrades> GetTradesDLL()
        {
            var res = (from a in _db.tbl_trade_mast.Where(a => a.trdae_is_active == true)

                       select new AffiliationCollegeTrades
                       {
                           trade_id = a.trade_id,
                           trade_code = a.trade_name

                       }).OrderBy(a=>a.trade_code).ToList();
            return res;
        }
        #endregion

        #region Get Institution Types
        /// <summary>
        /// GetInstitutionTypesDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListItem> GetInstitutionTypesDLL()
        {
            var res = (from a in _db.tbl_Institute_type.Where(a => a.IsActive == true)

                       select new SelectListItem
                       {
                           Text = a.Institute_type,
                           Value = a.Institute_type_id.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Location Types
        /// <summary>
        /// GetLocationTypesDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListItem> GetLocationTypesDLL()
        {
            var res = (from a in _db.tbl_location_type.Where(a => a.is_active == true)

                       select new SelectListItem
                       {
                           Text = a.location_name,
                           Value = a.location_id.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Taluk
        /// <summary>
        /// GetTalukDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListItem> GetTalukDLL(int DistId)
        {
            var res = (from a in _db.tbl_taluk_master.Where(a => a.district_lgd_code == DistId && a.taluk_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.taluk_ename,
                           Value = a.taluk_lgd_code.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Constiteuncy
        /// <summary>
        /// GetConstiteuncyDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListItem> GetConstiteuncyDLL()
        {
            var res = (from a in _db.tbl_Constituencies.Where(a => a.IsActive == true)

                       select new SelectListItem
                       {
                           Text = a.Constituencies,
                           Value = a.ConstituencyId.ToString()

                       }).OrderBy(a=>a.Text).ToList();
            return res;
        }
        #endregion

        #region Get Panchayat
        /// <summary>
        /// GetPanchayatDLL
        /// </summary>
        /// <param name="">TalukCode</param>
        /// <returns>List<SelectListItem></returns>
        public List<SelectListItem> GetPanchayatDLL(int TalukCode)
        {
            var res = (from a in _db.tbl_Grama_Panchayat_Masters.Where(a => a.taluk_lgd_code == TalukCode && a.gp_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.grama_panchayat_name,
                           Value = a.gp_lgd_code.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Village
        /// <summary>
        /// GetVillageDLL
        /// </summary>
        /// <param name="">panchaId</param>
        /// <returns></returns>
        public List<SelectListItem> GetVillageDLL(int panchaId)
        {
            var res = (from a in _db.tbl_Village_Masters.Where(a => a.taluk_code == panchaId && a.village_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.village_ename,
                           Value = a.village_lgd_code.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get District
        /// <summary>
        /// GetVillageDLL
        /// </summary>
        /// <param name="">panchaId</param>
        /// <returns></returns>
        public List<SelectListItem> GetDistrictsDLL()
        {
            var res = (from a in _db.tbl_district_master.Where(a => a.dis_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.district_ename,
                           Value = a.district_lgd_code.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Css Code
        /// <summary>
        /// GetVillageDLL
        /// </summary>
        /// <param name="">panchaId</param>
        /// <returns></returns>
        public List<SelectListItem> GetCssCodeDLL()
        {
            var res = (from a in _db.tbl_CSS.Where(a => a.IsActive == true)

                       select new SelectListItem
                       {
                           Text = a.CSS_Scheme,
                           Value = a.CSSCode.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get Affiliation Schemes
        /// <summary>
        /// Get Affiliation Schemes
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SelectListItem> GetAffiliationSchemesDLL()
        {
            var res = (from a in _db.tbl_trade_scheme.Where(a => a.ts_is_active == true)

                       select new SelectListItem
                       {
                           Text = a.trade_scheme,
                           Value = a.ts_id.ToString()

                       }).ToList();
            return res;
        }
        #endregion

        #region Get A Affiliation College Details
        /// <summary>
        /// GetAAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetAAffiliationCollegeDetailsDLL(int CollegeId)
        {
            try
            {
                var data = (from a in _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId)
                                ////join b in _db.tbl_ITI_Trades on a.iti_college_id equals b.ITICode into gj
                                //from subpet in gj.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                type_of_iti_id = a.Insitute_TypeId,
                                location_type_id = a.location_id,
                                mis_code = a.MISCode,
                                css_code_id = a.css_code,
                                dist_id = a.district_id,
                                taluk_id = a.taluk_id,
                                consti_id = a.Constituency,
                                pancha_id = a.Panchayat,
                                village_id = a.village_or_town,
                                build_up_area = a.BuildUpArea,
                                address = a.college_address,
                                geo_location = a.geo,
                                email = a.email_id,
                                affiliation_date = a.AffiliationDate,
                                phone_number = a.phone_num,
                                no_units = a.Units,
                                no_shifts = a.NoOfShifts,
                                Pincode = a.PinCode,
                                division_id = a.division_id,
                                iti_college_id = a.iti_college_id,
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                FileUploadPath = a.UploadAffiliationDoc,
                                status_id = a.StatusId,
                                Website = a.Website,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Scheme = a.Scheme,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                course_code = a.CourseCode,
                                UploadTradeAffiliationDoc=a.UploadTradeAffiliationDoc,

                            }).FirstOrDefault();

                if (data != null)
                {
                    data.trades_list = (from a in _db.tbl_ITI_Trade.Where(a => a.ITICode == data.iti_college_id)
                                        join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_id
                                        select new AffiliationTrade
                                        {
                                            trade_id = a.TradeCode,
                                            units = a.Unit,
                                            file_upload_path = a.FileUploadPath,
                                            trade_iti_id = a.Trade_ITI_id,
                                            status_id = a.StatusId,
                                            flow_id = a.FlowId,
                                            en_edit = a.FlowId == (int)CsystemType.getCommon.CaseWorker ? true : false,
                                            is_published = a.StatusId == (int)CsystemType.getCommon.Published ? true : false,
                                            trade_code = b.trade_code,


                                        }).ToList();

                    if (data.trades_list.Count() > 0)
                    {
                        foreach (var item in data.trades_list)
                        {
                            item.list = new List<TradeShift>();
                            item.list = GetAllTradeShiftsDLL(item.trade_iti_id);


                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get A Uploaded Affiliation College Details
        /// <summary>
        /// Here we are getting uploaded affiliation data from temprory tables to AffiliationCollegeDetails Model
        /// We have inner joined tables with name instead of id's bcoz we want ids of masters
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetAAffiliationUploadedCollegeDetailsDLL(int CollegeId)
        {
            try
            {
                var data = (from a in _db.temp_Tbl_Iti_College_Details.Where(a => a.iti_college_id_temp == CollegeId)
                            join c in _db.tbl_district_master on a.district equals c.district_ename into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk equals d.taluk_ename into dd
                            from d in dd.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.Constituencies into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location equals h.location_name into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_Type equals i.Institute_type into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_division_master on a.division equals j.division_name into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_trade_scheme on a.Scheme equals k.trade_scheme into kk
                            from k in kk.DefaultIfEmpty()
                            join l in _db.tbl_course_type_mast on a.CourseCode equals l.course_type_name into ll
                            from l in ll.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                type_of_iti_id = i.Institute_type_id,
                                location_type_id = h.location_id,
                                mis_code = a.MISCode,
                                dist_id = c.district_lgd_code,
                                taluk_id = d.taluk_lgd_code,
                                consti_id = g.ConstituencyId,
                                build_up_area = a.BuildUpArea,
                                address = a.college_address,
                                geo_location = a.geo,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                zipcode = a.PinCode,
                                division_id = j.division_id,
                                iti_college_id = a.iti_college_id_temp,
                                date = a.AffiliationDate,
                                status_id = a.StatusId,
                                Website = a.Website,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                Scheme = k.ts_id,
                                order_no_date = a.AffiliationOrderNoDate,
                                course_code = l.course_id,

                            }).FirstOrDefault();

                if (data.date != null)
                {
                    try
                    {
                        data.date = new string(data.date.TakeWhile(x => x != ' ').ToArray());
                    }
                    catch { }

                }
                if (data.order_no_date != null)
                {
                    try
                    {
                        data.order_no_date = new string(data.order_no_date.TakeWhile(x => x != ' ').ToArray());
                    }
                    catch { }

                }

                /// <summary>
                //taking back pincode from zipcode
                /// </summary>
                if (data.zipcode != null && data.zipcode != "")
                {
                    try
                    {
                        data.Pincode = Convert.ToInt32(data.zipcode);
                    }
                    catch { }

                }

                if (data != null)
                {
                    data.trades_list = (from a in _db.temp_Tbl_ITI_Trades.Where(a => a.ITICode == data.iti_college_id && a.IsActive == true)
                                        join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_name into bb
                                        from b in bb.DefaultIfEmpty()
                                        select new AffiliationTrade
                                        {
                                            trade_id = b.trade_id,
                                            trade_iti_id = a.Trade_ITI_id_temp,
                                            status_id = a.StatusId,
                                            flow_id = (int)CsystemType.getCommon.CaseWorker,
                                            en_edit = true,
                                            is_published = false,
                                            trade_code = b.trade_code


                                        }).ToList();

                    if (data.trades_list.Count() > 0)
                    {
                        foreach (var item in data.trades_list)
                        {
                            item.list = new List<TradeShift>();

                            item.list = (from a in _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == item.trade_iti_id && a.IsActive == true)
                                         select new TradeShift
                                         {
                                             CreatedBy = a.CreatedBy,
                                             ITI_Trade_Id = a.ITI_Trade_Id_temp,
                                             CreatedOn = a.CreatedOn,
                                             Dual_System = a.Dual_System,
                                             IsActive = a.IsActive,
                                             IsPPP = a.IsPPP,
                                             ITI_Trade_Shift_Id = a.ITI_Trade_Shift_Id_temp,
                                             st_shift = a.Shift,
                                             st_unit = a.Units

                                         }).ToList();
                            if (item.list.Count() > 0)
                            {
                                //taking back shift and unit from temp variables st_shift, st_unit
                                foreach (var value in item.list)
                                {
                                    try
                                    {
                                        value.Shift = Convert.ToInt32(value.st_shift);

                                    }
                                    catch { }

                                    try
                                    {

                                        value.Units = Convert.ToInt32(value.st_unit);
                                    }
                                    catch { }
                                }
                            }
                            try
                            {
                                item.units = item.list.Sum(a => Convert.ToInt32(a.st_unit));
                            }
                            catch { }
                        }
                        try
                        {
                            data.no_units = data.trades_list.Sum(a => Convert.ToInt32(a.units));
                        }
                        catch { }
                    }

                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get All Affiliation College Details

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL()
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            join rl in _db.tbl_role_master on b.FlowId equals rl.role_id into rm
                            from rl in rm.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                //FileUploadPath = a.UploadAffiliationDoc,
                                //status = k.StatusName + " - " + rl.role_DescShortForm,
                                status = (k.StatusId != 6 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                //x > y ? "x is greater than y" : x < y ? "x is less than y" :x == y ? "x is equal to y" : "No result";
                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                NewAffiliationOrderNo=a.NewAffiliationOrderNo,
                                NewAffiliationOrderNoDate=a.NewAffiliationOrderNoDate.ToString(),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,

                            }).Distinct().ToList();
                list = list.GroupBy(a => a.trade_iti_id).Select(a => new AffiliationCollegeDetails { name_of_iti = a.Select(z => z.name_of_iti).FirstOrDefault(), mis_code = a.Select(z => z.mis_code).FirstOrDefault(), trade = a.Select(z => z.trade).FirstOrDefault(), no_units = a.Select(z => z.no_units).FirstOrDefault(), district = a.Select(z => z.district).FirstOrDefault(), taluka = a.Select(z => z.taluka).FirstOrDefault(), status = a.Select(z => z.status).FirstOrDefault(), FileUploadPath = a.Select(z => z.FileUploadPath).FirstOrDefault(), trade_iti_id = a.Select(z => z.trade_iti_id).FirstOrDefault(), status_id = a.Select(z => z.status_id).FirstOrDefault(), CreatedOn = a.Select(z => z.CreatedOn).FirstOrDefault(), iti_college_id = a.Select(z => z.iti_college_id).FirstOrDefault(), color_flag = a.Select(z => z.color_flag).FirstOrDefault(), course_name = a.Select(z => z.course_name).FirstOrDefault(), division = a.Select(z => z.division).FirstOrDefault(), trade_id = a.Select(z => z.trade_id).FirstOrDefault() }).ToList();
                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }
                foreach (var itemdocs in list)
                {
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == itemdocs.iti_college_id && a.Trade_Id == itemdocs.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { itemdocs.FileUploadPath = docs.FileName; }
                }
                    return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL(int statusId)
        {
            try
            {
                List<AffiliationCollegeDetails> list = (from a in _db.tbl_iti_college_details
                                                        join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                                                        from b in bb.DefaultIfEmpty()
                                                        join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id 
                                                        join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                                                        from c in cc.DefaultIfEmpty()
                                                        join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                                                        from d in dd.DefaultIfEmpty()
                                                        join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                                                        from e in ee.DefaultIfEmpty()
                                                        join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                                                        from f in ff.DefaultIfEmpty()
                                                        join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                                                        from g in gg.DefaultIfEmpty()
                                                        join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                                                        from h in hh.DefaultIfEmpty()
                                                        join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                                                        from i in ii.DefaultIfEmpty()
                                                        join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                                                        from j in jj.DefaultIfEmpty()
                                                        join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                                                        from l in ll.DefaultIfEmpty()
                                                        join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                                                        from m in mm.DefaultIfEmpty()
                                                        join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                                                        from n in nn.DefaultIfEmpty()
                                                        join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                                                        from o in oo.DefaultIfEmpty()
                                                        join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                                                        from p in pp.DefaultIfEmpty()
                                                        join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                                                        where b.StatusId == statusId || b.StatusId == 19
                                                        select new AffiliationCollegeDetails
                                                        {
                                                            name_of_iti = a.iti_college_name,
                                                            mis_code = a.MISCode,
                                                            type_of_iti = i.Institute_type,
                                                            trade = j.trade_name,
                                                            district = c.district_ename,
                                                            taluka = d.taluk_ename,
                                                            panchayat = e.grama_panchayat_name,
                                                            village_id = a.village_or_town,
                                                            consti_id = a.Constituency,
                                                            build_up_area = a.BuildUpArea,
                                                            css_code_id = a.css_code,
                                                            geo_location = a.geo,
                                                            address = a.college_address,
                                                            location_type = h.location_name,
                                                            email = a.email_id,
                                                            phone_number = a.phone_num,
                                                            affiliation_date = a.AffiliationDate,
                                                            //no_trades = column missing
                                                            NoofTrades = a.NoofTrades,
                                                            no_shifts = a.NoOfShifts,
                                                            iti_college_id = a.iti_college_id,
                                                            no_units = b.Unit,
                                                            state = "Karnataka",
                                                            date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                                            trade_id = j.trade_id,
                                                            FileUploadPath = a.UploadAffiliationDoc,
                                                            status = k.StatusName,
                                                            constituency = g.Constituencies,
                                                            village = f.village_ename,
                                                            trade_iti_id = b.Trade_ITI_id,
                                                            CreatedOn = b.CreatedOn,
                                                            status_id = b.StatusId,
                                                            flow_id = b.FlowId,
                                                            color_flag = b.color_flag,
                                                            Pincode = a.PinCode,
                                                            division = l.division_name,
                                                            sector = m.trade_sector,
                                                            trade_code = j.trade_code,
                                                            course_name = n.course_type_name,
                                                            duration = j.trade_duration,
                                                            batch_size = j.trade_seating,
                                                            AffiliationOrderNo = a.AffiliationOrderNo,
                                                            AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                                            Website = a.Website,
                                                            scheme_name = o.trade_scheme,
                                                            trade_type = p.trade_type_name,
                                                            order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                                            units=trshift.Units.ToString(),
                                                            tshift=trshift.Shift,
                                                            division_id=l.division_id,
                                                            dist_id=c.district_lgd_code,
                                                            course_code=n.course_id,
                                                            taluk_id=d.taluk_lgd_code
                                                            
                                                        }).OrderByDescending(a => a.CreatedOn).ToList();
                foreach (var itemdocs in list)
                {
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == itemdocs.iti_college_id && a.Trade_Id == itemdocs.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { itemdocs.FileUploadPath = docs.FileName; }
                }
                //foreach (var item in list)
                //{
                //    List<TradeShift> shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                //    item.no_units = shifts.Count();
                //    item.no_shifts = shifts.Count();
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting 20 random records
        /// </summary>
        /// <param name="noOfRec"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLL(int noOfRec, int statusId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            where b.StatusId == statusId
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),


                            }).OrderByDescending(a => a.CreatedOn).Take(noOfRec).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseDLL(int courseId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.CourseCode == courseId)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                //foreach (var item in list)
                //{
                //    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                //    item.no_shifts = shifts.Count;
                //    item.no_units = shifts.Count;
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Getting all records by division
        /// </summary>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionDLL(int divisionId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                division_id = c.division_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).Where(a => a.division_id == divisionId).OrderByDescending(a => a.CreatedOn).ToList();

                //foreach (var item in list)
                //{
                //    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                //    item.no_shifts = shifts.Count;
                //    item.no_units = shifts.Count;
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by division & course
        /// </summary>
        /// <param name="divisionId courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseDLL(int courseId, int divisionId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.CourseCode == courseId)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                division_id = c.division_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).Where(a => a.division_id == divisionId).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by district & course
        /// </summary>
        /// <param name="districtid courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseDLL(int courseId, int districtid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.CourseCode == courseId && a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                //foreach (var item in list)
                //{
                //    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                //    item.no_shifts = shifts.Count;
                //    item.no_units = shifts.Count;
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by district
        /// </summary>
        /// <param name="districtid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictDLL(int districtid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by trade & district
        /// </summary>
        /// <param name="districtid tradeid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictDLL(int districtid, int tradeid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift


                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.trade_id == tradeid).ToList();

                //foreach (var item in list)
                //{
                //    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                //    item.no_shifts = shifts.Count;
                //    item.no_units = shifts.Count;
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by trade  district & course
        /// </summary>
        /// <param name="districtid courseId tradeid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLL(int courseid, int districtid, int tradeid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.trade_id == tradeid).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDLL(int tradeId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = b.TradeCode,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                // en_edit = (int)CsystemType.getCommon.CaseWorker ? true : false
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift

                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.trade_id == tradeId).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Ram- Get All Affiliation College Details Filter Generic login

        /// <summary>
        /// Getting all records by trade & district
        /// </summary>
        /// <param name="districtid tradeid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeAndDistrictDLLFilter(int districtid, int constituencyid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code=a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.consti_id == constituencyid).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by district & course
        /// </summary>
        /// <param name="districtid courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictAndCourseDLLFilter(int talukid, int districtid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.taluk_id == talukid && a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by trade  district & course
        /// </summary>
        /// <param name="districtid courseId tradeid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDistrictAndCourseDLLFilter(int talukid, int districtid, int constituencyid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.consti_id == constituencyid).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by division & course
        /// </summary>
        /// <param name="divisionId courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionAndCourseDLLFilter(int talukid, int divisionId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.taluk_id == talukid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                division_id = c.division_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).Where(a => a.division_id == divisionId).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by district
        /// </summary>
        /// <param name="districtid"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDistrictDLLFilter(int districtid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == districtid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByCourseDLLFilter(int talukid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => a.taluk_id == talukid)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by division
        /// </summary>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByDivisionDLLFilter(int divisionId)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                division_id = c.division_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).Where(a => a.division_id == divisionId).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsByTradeDLLFilter(int constituencyid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = b.TradeCode,
                                FileUploadPath = a.UploadAffiliationDoc,
                                status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                // en_edit = (int)CsystemType.getCommon.CaseWorker ? true : false
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).OrderByDescending(a => a.CreatedOn).Where(a => a.consti_id == constituencyid).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLLFilter(int statusId)
        {
            try
            {
                List<AffiliationCollegeDetails> list = (from a in _db.tbl_iti_college_details
                                                        join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                                                        from b in bb.DefaultIfEmpty()
                                                        join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                                                        from c in cc.DefaultIfEmpty()
                                                        join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                                                        from d in dd.DefaultIfEmpty()
                                                        join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                                                        from e in ee.DefaultIfEmpty()
                                                        join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                                                        from f in ff.DefaultIfEmpty()
                                                        join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                                                        from g in gg.DefaultIfEmpty()
                                                        join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                                                        from h in hh.DefaultIfEmpty()
                                                        join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                                                        from i in ii.DefaultIfEmpty()
                                                        join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                                                        from j in jj.DefaultIfEmpty()
                                                        join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                                                        from l in ll.DefaultIfEmpty()
                                                        join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                                                        from m in mm.DefaultIfEmpty()
                                                        join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                                                        from n in nn.DefaultIfEmpty()
                                                        join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                                                        from o in oo.DefaultIfEmpty()
                                                        join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                                                        from p in pp.DefaultIfEmpty()
                                                        join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                                                        where b.StatusId == statusId || b.StatusId == 19
                                                        select new AffiliationCollegeDetails
                                                        {
                                                            name_of_iti = a.iti_college_name,
                                                            mis_code = a.MISCode,
                                                            type_of_iti = i.Institute_type,
                                                            trade = j.trade_name,
                                                            district = c.district_ename,
                                                            taluka = d.taluk_ename,
                                                            panchayat = e.grama_panchayat_name,
                                                            village_id = a.village_or_town,
                                                            consti_id = a.Constituency,
                                                            build_up_area = a.BuildUpArea,
                                                            css_code_id = a.css_code,
                                                            geo_location = a.geo,
                                                            address = a.college_address,
                                                            location_type = h.location_name,
                                                            email = a.email_id,
                                                            phone_number = a.phone_num,
                                                            affiliation_date = a.AffiliationDate,
                                                            //no_trades = column missing
                                                            NoofTrades = a.NoofTrades,
                                                            no_shifts = a.NoOfShifts,
                                                            iti_college_id = a.iti_college_id,
                                                            no_units = b.Unit,
                                                            state = "Karnataka",
                                                            date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                                            trade_id = j.trade_id,
                                                            FileUploadPath = a.UploadAffiliationDoc,
                                                            status = k.StatusName,
                                                            constituency = g.Constituencies,
                                                            village = f.village_ename,
                                                            trade_iti_id = b.Trade_ITI_id,
                                                            CreatedOn = b.CreatedOn,
                                                            status_id = b.StatusId,
                                                            flow_id = b.FlowId,
                                                            color_flag = b.color_flag,
                                                            Pincode = a.PinCode,
                                                            division = l.division_name,
                                                            sector = m.trade_sector,
                                                            trade_code = j.trade_code,
                                                            course_name = n.course_type_name,
                                                            duration = j.trade_duration,
                                                            batch_size = j.trade_seating,
                                                            AffiliationOrderNo = a.AffiliationOrderNo,
                                                            AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                                            Website = a.Website,
                                                            scheme_name = o.trade_scheme,
                                                            trade_type = p.trade_type_name,
                                                            order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),

                                                        }).OrderByDescending(a => a.CreatedOn).ToList();

                foreach (var item in list)
                {
                    List<TradeShift> shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_units = shifts.Count();
                    item.no_shifts = shifts.Count();
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllAffiliationCollegeDetailsDLLFilter()
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join trshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals trshift.ITI_Trade_Id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                            from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                            join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                            from p in pp.DefaultIfEmpty()
                            join rl in _db.tbl_role_master on b.FlowId equals rl.role_id into rm
                            from rl in rm.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = a.UploadAffiliationDoc,
                                //status = k.StatusName + " - " + rl.role_DescShortForm,
                                status = (k.StatusId != 6 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                status_id = b.StatusId,
                                flow_id = b.FlowId,
                                color_flag = b.color_flag,
                                Pincode = a.PinCode,
                                division = l.division_name,
                                sector = m.trade_sector,
                                trade_code = j.trade_code,
                                course_name = n.course_type_name,
                                duration = j.trade_duration,
                                batch_size = j.trade_seating,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Website = a.Website,
                                scheme_name = o.trade_scheme,
                                trade_type = p.trade_type_name,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                units=trshift.Units.ToString(),
                                tshift=trshift.Shift,
                                course_code = a.CourseCode

                            }).ToList();

                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update Affiliation College Details
        /// <summary>
        /// UpdateAffiliationCollegeDetails
        /// </summary>
        /// <param name="Affi"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails UpdateAffiliationCollegeDetailsDLL(AffiliationCollegeDetails Affi)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    NestedTradeSession session = (NestedTradeSession)HttpContext.Current.Session["TradeShift"];

                    var update = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Affi.iti_college_id).FirstOrDefault();

                    update.iti_college_name = Affi.name_of_iti;
                    update.Insitute_TypeId = Affi.type_of_iti_id;
                    update.location_id = Affi.location_type_id;
                    update.MISCode = Affi.mis_code;
                    // update.css_code = Affi.css_code_id;
                    update.district_id = Affi.dist_id;
                    update.taluk_id = Affi.taluk_id;
                    update.Constituency = Affi.consti_id;
                    update.Panchayat = Affi.pancha_id;
                    update.village_or_town = Affi.village_id;
                    update.BuildUpArea = Affi.build_up_area;
                    update.college_address = Affi.address;
                    update.geo = Affi.geo_location;
                    update.email_id = Affi.email;
                    update.phone_num = Affi.phone_number;
                    update.AffiliationDate = Affi.affiliation_date;
                    update.Units = Affi.no_units;
                    update.NoOfShifts = Affi.no_shifts;
                    update.updated_by = Affi.CreatedBy;
                    update.updation_datetime = DateTime.Now;
                    update.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    update.PinCode = Affi.Pincode;
                    update.Website = Affi.Website;
                    update.AffiliationOrderNo = Affi.AffiliationOrderNo;
                    update.AffiliationOrderNoDate = Affi.AffiliationOrderNoDate;
                    update.Scheme = Affi.Scheme;
                    update.CourseCode = Affi.course_code;
                    update.division_id = Affi.division_id;

                    if (Affi.FileUploadPath != null)
                    {
                        update.UploadAffiliationDoc = Affi.FileUploadPath;
                    }

                    _db.SaveChanges();

                    if (Affi.trades_list != null)
                    {
                        if (Affi.trades_list.Count() > 0)
                        {

                            List<int> ids = new List<int>(); //Except these ids we have to delete other records
                            int Status = (int)CsystemType.getCommon.Sent_for_Review;

                            for (var i = 0; i < Affi.trades_list.Count(); i++)
                            {
                                int iti_trade_id = 0;
                                TradeShiftSessions shifts = session.sessions.Find(a => a.sessionKey == Affi.trades_list[i].sessionKey);

                                if (Affi.trades_list[i].trade_iti_id != 0)
                                {
                                    int Trade_ITI_id = Affi.trades_list[i].trade_iti_id;

                                    var Update_Trade = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == Trade_ITI_id).FirstOrDefault();
                                    if (Update_Trade.FlowId == Affi.CreatedBy)
                                    {
                                        Update_Trade.TradeCode = Affi.trades_list[i].trade_id;
                                        Update_Trade.StatusId = Status;
                                        Update_Trade.Unit = Affi.trades_list[i].units;
                                        Update_Trade.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                        Update_Trade.CreatedOn = DateTime.Now;

                                        _db.SaveChanges();

                                        var Add_His = new tbl_ITI_Trade_History();
                                        Add_His.ITICode = Affi.iti_college_id;
                                        Add_His.TradeCode = Convert.ToInt32(Affi.trades_list[i].trade_id);
                                        Add_His.CreatedOn = DateTime.Now;
                                        Add_His.CreatedBy = Affi.CreatedBy;
                                        Add_His.StatusId = Status;
                                        Add_His.Unit = Affi.trades_list[i].units;
                                        Add_His.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                        Add_His.IsActive = true;
                                        Add_His.FileUploadPath = Affi.FileUploadPath;
                                        Add_His.Remarks = "Trade Updated";
                                        Add_His.Trade_ITI_id = Trade_ITI_id;

                                        _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                        _db.SaveChanges();



                                    }
                                    iti_trade_id = Update_Trade.Trade_ITI_id;
                                    ids.Add(Update_Trade.Trade_ITI_id);

                                }
                                else
                                {
                                    var Add_trade = new tbl_ITI_Trade();
                                    Add_trade.ITICode = Affi.iti_college_id;
                                    Add_trade.CreatedBy = Affi.CreatedBy;
                                    Add_trade.CreatedOn = DateTime.Now;
                                    Add_trade.TradeCode = Affi.trades_list[i].trade_id;
                                    Add_trade.StatusId = Status;
                                    Add_trade.Unit = Affi.trades_list[i].units;
                                    Add_trade.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                    Add_trade.IsActive = true;
                                    Add_trade.color_flag = Affi.color_flag;

                                    _db.tbl_ITI_Trade.Add(Add_trade);
                                    _db.SaveChanges();

                                    iti_trade_id = Add_trade.Trade_ITI_id;
                                    ids.Add(Add_trade.Trade_ITI_id);


                                    var Add_His = new tbl_ITI_Trade_History();
                                    Add_His.ITICode = Affi.iti_college_id;
                                    Add_His.TradeCode = Convert.ToInt32(Affi.trades_list[i].trade_id);
                                    Add_His.CreatedOn = DateTime.Now;
                                    Add_His.CreatedBy = Affi.CreatedBy;
                                    Add_His.StatusId = Status;
                                    Add_His.Unit = Affi.trades_list[i].units;
                                    Add_His.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                    Add_His.IsActive = true;
                                    Add_His.FileUploadPath = Affi.FileUploadPath;
                                    Add_His.Remarks = "New Trade Added";
                                    Add_His.Trade_ITI_id = iti_trade_id;

                                    _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                    _db.SaveChanges();
                                }

                                if (shifts != null)
                                {
                                    if (shifts.list != null)
                                    {
                                        List<int> shiftIds = new List<int>();
                                        foreach (var item in shifts.list)
                                        {
                                            if (item.ITI_Trade_Shift_Id != 0)
                                            {
                                                var updateShift = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == item.ITI_Trade_Shift_Id).FirstOrDefault();

                                                updateShift.Dual_System = item.Dual_System;
                                                updateShift.IsPPP = item.IsPPP;
                                                updateShift.Shift = item.Shift;
                                                updateShift.Units = item.Units;

                                                _db.SaveChanges();

                                                shiftIds.Add(updateShift.ITI_Trade_Shift_Id);
                                            }
                                            else
                                            {
                                                var insertShift = new tbl_ITI_Trade_Shift();
                                                insertShift.CreatedBy = Affi.CreatedBy;
                                                insertShift.CreatedOn = DateTime.Now;
                                                insertShift.Dual_System = item.Dual_System;
                                                insertShift.IsActive = true;
                                                insertShift.IsPPP = item.IsPPP;
                                                insertShift.ITI_Trade_Id = iti_trade_id;
                                                insertShift.Shift = item.Shift;
                                                insertShift.Units = item.Units;

                                                _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                                _db.SaveChanges();

                                                shiftIds.Add(insertShift.ITI_Trade_Shift_Id);
                                            }

                                        }

                                        List<tbl_ITI_Trade_Shift> de_shift = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == iti_trade_id).ToList();
                                        foreach (var item in de_shift)
                                        {
                                            if (!shiftIds.Any(a => a == item.ITI_Trade_Shift_Id))
                                            {
                                                item.IsActive = false;
                                                _db.SaveChanges();
                                            }
                                        }
                                    }

                                }
                            }

                            //Delete operation
                            List<tbl_ITI_Trade> de_trades = _db.tbl_ITI_Trade.Where(a => a.ITICode == Affi.iti_college_id && a.IsActive == true).ToList();
                            foreach (var item in de_trades)
                            {
                                if (!ids.Any(a => a == item.Trade_ITI_id)) // deleting records except saved/updated above records
                                {
                                    item.IsActive = false;
                                    _db.SaveChanges();

                                    var Add_His = new tbl_ITI_Trade_History();
                                    Add_His.ITICode = Affi.iti_college_id;
                                    Add_His.TradeCode = item.TradeCode ?? 0;
                                    Add_His.CreatedOn = DateTime.Now;
                                    Add_His.CreatedBy = Affi.CreatedBy;
                                    Add_His.StatusId = item.StatusId;
                                    Add_His.Unit = item.Unit;
                                    Add_His.FlowId = item.FlowId;
                                    Add_His.IsActive = false;
                                    Add_His.FileUploadPath = item.FileUploadPath;
                                    Add_His.Remarks = "Trade deleted";
                                    Add_His.Trade_ITI_id = item.Trade_ITI_id;

                                    _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                    _db.SaveChanges();
                                }
                            }


                        }
                    }
                    transaction.Complete();
                }

                return Affi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Affiliation Trade Details
        /// <summary>
        /// Trade wise update affiliation 
        /// </summary>
        /// <param name="Affi"></param>
        /// <returns></returns>
        public AffiliationCollegeDetailsTest UpdateAffiliationTradeDetails(AffiliationCollegeDetailsTest Affi)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    NestedTradeSession session = (NestedTradeSession)HttpContext.Current.Session["TradeShift"];

                    var update = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Affi.iti_college_id).FirstOrDefault();

                    update.iti_college_name = Affi.name_of_iti;
                    update.Insitute_TypeId = Affi.type_of_iti_id;
                    update.location_id = Affi.location_type_id;
                    update.MISCode = Affi.mis_code;
                    // update.css_code = Affi.css_code_id;
                    update.district_id = Affi.dist_id;
                    update.taluk_id = Affi.taluk_id;
                    update.Constituency = Affi.consti_id;
                    update.Panchayat = Affi.pancha_id;
                    update.village_or_town = Affi.village_id;
                    //update.BuildUpArea = Affi.build_up_area;
                    update.college_address = Affi.address;
                    update.geo = Affi.geo_location;
                    update.email_id = Affi.email;
                    update.phone_num = Affi.phone_number;
                    update.AffiliationDate = Affi.affiliation_date;
                    update.Units = Affi.no_units;
                    update.NoOfShifts = Affi.no_shifts;
                    update.updated_by = Affi.CreatedBy;
                    update.updation_datetime = DateTime.Now;
                    update.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    update.PinCode = Affi.Pincode;
                    update.Website = Affi.Website;
                    update.AffiliationOrderNo = Affi.AffiliationOrderNo;
                    update.AffiliationOrderNoDate = Affi.AffiliationOrderNoDate;
                    update.Scheme = Affi.Scheme;
                    update.CourseCode = Affi.course_code;
                    update.division_id = Affi.division_id;

                    //if (Affi.FileUploadPath != null)
                    //{
                    //    update.UploadAffiliationDoc = Affi.FileUploadPath;
                    //}

                    _db.SaveChanges();

                    var chkExistRec = _db.tbl_Affiliation_documents.Where(a => a.Institute_id == update.iti_college_id && a.Trade_Id == Affi.trade_id).FirstOrDefault();

                    if (chkExistRec == null)
                    {
                        
                        if (Affi.FileUploadPath != null)
                        {
                            tbl_Affiliation_documents DocUpload = new tbl_Affiliation_documents();
                            DocUpload.FileName = Affi.FileUploadPath;
                            DocUpload.Institute_id = update.iti_college_id;
                            DocUpload.Trade_Id = Affi.trade_id;
                            DocUpload.AffiliationOrder_Number = Affi.AffiliationOrderNo;
                            DocUpload.Affiliation_date = Affi.AffiliationOrderNoDate;
                            _db.tbl_Affiliation_documents.Add(DocUpload);
                        }
                        
                    }
                    else
                    {
                        if (Affi.FileUploadPath != null)
                        {
                            chkExistRec.FileName = Affi.FileUploadPath;
                        }
                    }
                    _db.SaveChanges();


                    tbl_ITI_Trade update_trade = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == Affi.trade_iti_id).FirstOrDefault();
                    update_trade.FlowId = Affi.flow_id;
                    update_trade.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    update_trade.CreatedOn = DateTime.Now;
                    update_trade.CreatedBy = Affi.CreatedBy;

                    _db.SaveChanges();


                    var Add_His = new tbl_ITI_Trade_History();
                    Add_His.ITICode = Affi.iti_college_id;
                    Add_His.TradeCode = (int)Affi.trade_id;
                    Add_His.CreatedOn = DateTime.Now;
                    Add_His.CreatedBy = Affi.CreatedBy;
                    Add_His.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    Add_His.Unit = Affi.no_units;
                    Add_His.FlowId = Affi.flow_id;
                    Add_His.IsActive = true;
                    Add_His.FileUploadPath = Affi.FileUploadPath;
                    Add_His.Remarks = Affi.remarks;
                    Add_His.Trade_ITI_id = Affi.trade_iti_id;

                    _db.tbl_ITI_Trade_Histories.Add(Add_His);
                    _db.SaveChanges();


                    if (Affi.shifts != null)
                    {
                        if (Affi.shifts != null)
                        {
                            List<int> shiftIds = new List<int>();
                            foreach (var item in Affi.shifts)
                            {
                                if (item.ITI_Trade_Shift_Id != 0)
                                {
                                    var updateShift = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == item.ITI_Trade_Shift_Id).FirstOrDefault();

                                    updateShift.Dual_System = item.Dual_System;
                                    updateShift.IsPPP = item.IsPPP;
                                    updateShift.Shift = item.Shift;
                                    updateShift.Units = item.Units;

                                    _db.SaveChanges();

                                    shiftIds.Add(updateShift.ITI_Trade_Shift_Id);
                                }
                                else
                                {
                                    var insertShift = new tbl_ITI_Trade_Shift();
                                    insertShift.CreatedBy = Affi.CreatedBy;
                                    insertShift.CreatedOn = DateTime.Now;
                                    insertShift.Dual_System = item.Dual_System;
                                    insertShift.IsActive = false;
                                    insertShift.IsPPP = item.IsPPP;
                                    insertShift.ITI_Trade_Id = Affi.trade_iti_id;
                                    insertShift.Shift = item.Shift;
                                    insertShift.Units = item.Units;
                                    insertShift.Status = (int)CsystemType.getCommon.Submitted;

                                    _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                    _db.SaveChanges();

                                    shiftIds.Add(insertShift.ITI_Trade_Shift_Id);
                                }

                                

                            }
                            if(Affi.UploadTradeAffiliationDoc!=null)
                            {
                                var Trade_Doc = new tbl_Affiliation_documents();
                                Trade_Doc.Institute_id = Affi.iti_college_id;
                                Trade_Doc.Trade_Id = Convert.ToInt32(Affi.trade_id);
                                Trade_Doc.FileName = Affi.UploadTradeAffiliationDoc; 
                                Trade_Doc.IsActive = true;
                                Trade_Doc.Status = "New Unit ";
                                Trade_Doc.Flag = (int)CsystemType.getCommon.New_Units;
                                Trade_Doc.AffiliationOrder_Number = Affi.NewAffiliationOrderNo;
                                Trade_Doc.Affiliation_date = Affi.NewAffiliationOrderNoDate; 
                                _db.tbl_Affiliation_documents.Add(Trade_Doc);
                                _db.SaveChanges();
                            }
                            
                            List<tbl_ITI_Trade_Shift> de_shift = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == Affi.trade_iti_id).ToList();
                            foreach (var item in de_shift)
                            {
                                if (!shiftIds.Any(a => a == item.ITI_Trade_Shift_Id))
                                {
                                    item.IsActive = false;
                                    _db.SaveChanges();
                                }
                            }
                        }

                    }

                    transaction.Complete();
                }

                return Affi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Affiliation College Details
        /// <summary>
        /// AddAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="Affi"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails AddAffiliationCollegeDetailsDLL(AffiliationCollegeDetails Affi)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    NestedTradeSession session = (NestedTradeSession)HttpContext.Current.Session["TradeShift"];

                    var anEntry = new tbl_iti_college_details();
                    anEntry.iti_college_name = Affi.name_of_iti;
                    anEntry.Insitute_TypeId = Affi.type_of_iti_id;
                    anEntry.location_id = Affi.location_type_id;
                    anEntry.MISCode = Affi.mis_code;
                    //anEntry.css_code = Affi.css_code_id;
                    anEntry.district_id = Affi.dist_id;
                    anEntry.taluk_id = Affi.taluk_id;
                    anEntry.Constituency = Affi.consti_id;
                    anEntry.Panchayat = Affi.pancha_id;
                    anEntry.village_or_town = Affi.village_id;
                    //anEntry.BuildUpArea = Affi.build_up_area;
                    anEntry.college_address = Affi.address;
                    anEntry.geo = Affi.geo_location;
                    anEntry.email_id = Affi.email;
                    anEntry.phone_num = Affi.phone_number;
                    anEntry.AffiliationDate = Affi.affiliation_date;
                    anEntry.Units = Affi.no_units;
                    anEntry.NoOfShifts = Affi.no_shifts;
                    if (Affi.FileUploadPath != null)
                    {
                        anEntry.UploadAffiliationDoc = Affi.FileUploadPath;
                    }
                    anEntry.iti_college_code = "0";
                    anEntry.created_by = Affi.CreatedBy;
                    anEntry.creation_datetime = DateTime.Now;
                    anEntry.PinCode = Affi.Pincode;
                    anEntry.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    anEntry.ActiveDeActive = true;
                    anEntry.Website = Affi.Website;
                    anEntry.AffiliationOrderNo = Affi.AffiliationOrderNo;
                    anEntry.AffiliationOrderNoDate = Affi.AffiliationOrderNoDate;
                    anEntry.Scheme = Affi.Scheme;
                    anEntry.CourseCode = Affi.course_code;
                    anEntry.division_id = Affi.division_id;

                    _db.tbl_iti_college_details.Add(anEntry);
                    _db.SaveChanges();

                    Affi.iti_college_id = anEntry.iti_college_id;

                    if (Affi.list_trades != null)
                    {
                        if (Affi.list_trades.Count() > 0)
                        {
                            int Status = (int)CsystemType.getCommon.Sent_for_Review;

                            foreach (var item in Affi.trades_list)
                            {
                                TradeShiftSessions shifts = session.sessions.Find(a => a.sessionKey == item.sessionKey);

                                var Add = new tbl_ITI_Trade();
                                Add.ITICode = Affi.iti_college_id;
                                Add.TradeCode = item.trade_id;
                                Add.CreatedOn = DateTime.Now;
                                Add.CreatedBy = Affi.CreatedBy;
                                Add.StatusId = Status;
                                Add.Unit = item.units;
                                Add.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                Add.IsActive = false;
                                Add.FileUploadPath = item.file_upload_path;
                                Add.ActiveDeActive = true;
                                Add.color_flag = Affi.color_flag;

                                _db.tbl_ITI_Trade.Add(Add);
                                _db.SaveChanges();

                                var Add_His = new tbl_ITI_Trade_History();
                                Add_His.ITICode = Affi.iti_college_id;
                                Add_His.TradeCode = Convert.ToInt32(item.trade_id);
                                Add_His.CreatedOn = DateTime.Now;
                                Add_His.CreatedBy = Affi.CreatedBy;
                                Add_His.StatusId = Status;
                                Add_His.Unit = item.units;
                                Add_His.FlowId = (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                Add_His.IsActive = false;
                                Add_His.Remarks = "New Affiliation Institute Trade Added";
                                Add_His.FileUploadPath = item.file_upload_path;
                                Add_His.Trade_ITI_id = Add.Trade_ITI_id;


                                _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                _db.SaveChanges();


                                if (shifts != null)
                                {
                                    if (shifts.list != null)
                                    {

                                        foreach (var item2 in shifts.list)
                                        {

                                            var insertShift = new tbl_ITI_Trade_Shift();
                                            insertShift.CreatedBy = Affi.CreatedBy;
                                            insertShift.CreatedOn = DateTime.Now;
                                            insertShift.Dual_System = item2.Dual_System;
                                            insertShift.IsActive = false;
                                            insertShift.IsPPP = item2.IsPPP;
                                            insertShift.ITI_Trade_Id = Add.Trade_ITI_id;
                                            insertShift.Shift = item2.Shift;
                                            insertShift.Units = item2.Units;
                                            insertShift.Status = (int)CsystemType.getCommon.Submitted;

                                            _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                            _db.SaveChanges();

                                        }


                                    }

                                }
                            }
                        }
                    }
                    transaction.Complete();
                }

                return Affi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Upload Multiple Affiliation Files
        /// <summary>
        /// UploadMultipleAffiliationFiles
        /// </summary>
        /// <param name="CollegeIds"></param>
        /// <param name="fileNames"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public UploadAffiliation UploadMultipleAffiliationFilesDLL(int CollegeId, string fileName, string filePath)
        {
            UploadAffiliation output = new UploadAffiliation();
            try
            {
                var updateFile = _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId).FirstOrDefault();
                updateFile.UploadAffiliationDoc = filePath;
                //updateFile.file_ref_no = fileName;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                output.status = ex.Message;
                output.flag = 0;
            }
            return output;
        }
        #endregion

        #region GetAffiliationCollegeDetails
        /// <summary>
        /// GetAffiliationCollegeDetails
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetAffiliationCollegeDetailsDLL(int CollegeId, int roleid)
        {
            try
            {
                var data = (from a in _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId)
                            join tr in _db.tbl_ITI_Trade on a.iti_college_id equals tr.ITICode into it
                            from tr in it.DefaultIfEmpty()
                            join his in _db.tbl_ITI_Institute_ActiveStatus on a.iti_college_id equals his.ITI_Institute_Id into AC
                            from his in AC.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join ts in _db.tbl_trade_scheme on a.Insitute_TypeId equals ts.ts_id into tsi
                            from ts in tsi.DefaultIfEmpty()
                            join cst in _db.tbl_course_type_mast on a.CourseCode equals cst.course_id into co
                            from cst in co.DefaultIfEmpty()
                            join div in _db.tbl_division_master on a.division_id equals div.division_id into ds
                            from div in ds.DefaultIfEmpty()
                            join master in _db.tbl_trade_mast on tr.TradeCode equals master.trade_id
                            into tm
                            from master in tm.DefaultIfEmpty()
                            join k in _db.tbl_status_master on his.ApprovalStatus equals k.StatusId into sm
                            from k in sm.DefaultIfEmpty()


                            select new AffiliationCollegeDetails
                            {

                                name_of_iti = a.iti_college_name,
                                type_of_iti_id = a.Insitute_TypeId,
                                location_type_id = a.location_id,
                                mis_code = a.MISCode,
                                css_code_id = a.css_code,
                                dist_id = a.district_id,
                                taluk_id = a.taluk_id,
                                consti_id = a.Constituency,
                                pancha_id = a.Panchayat,
                                village_id = a.village_or_town,
                                build_up_area = a.BuildUpArea,
                                address = a.college_address,
                                geo_location = a.geo,
                                email = a.email_id,
                                affiliation_date = a.AffiliationDate,
                                phone_number = a.phone_num,
                                no_units = a.Units,
                                no_shifts = a.NoOfShifts,
                                Pincode = a.PinCode,
                                division_id = a.division_id,
                                iti_college_id = a.iti_college_id,
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                en_edit = his.ApprovalFlowId == roleid ? true : false,
                                panchayat = e.grama_panchayat_name,
                                location_type = h.location_name,
                                type_of_iti = i.Institute_type,
                                FileUploadPath = his.FilePath,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                course_code = a.CourseCode,
                                course_name = cst.course_type_name,
                                scheme_name = ts.trade_scheme,
                                Website = a.Website,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Scheme = a.Scheme,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                IsActive = a.is_active,
                                IsActiveNew = his.IsActive,
                                division = div.division_name,
                                ActiveDeactive = a.is_active,
                                trade = master.trade_name,
                                status = k.StatusName,
                                flow_id = his.ApprovalFlowId,
                                CreatedByChk = his.CreatedBy,
                                AffiliateFilePath = his.AffiliateFilePath,
                                AffiliationFilePath=a.UploadAffiliationDoc
                            }).FirstOrDefault();

                if (data != null)
                {
                    data.Filename=System.IO.Path.GetFileName(data.AffiliationFilePath);
                    // data.trades = new List<SelectListItem>();
                    data.trades_list = new List<AffiliationTrade>();
                    data.trades_list = (from a in _db.tbl_ITI_Trade.Where(a => a.ITICode == data.iti_college_id)
                                        join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_id
                                        select new AffiliationTrade
                                        {
                                            trade_id = a.TradeCode,
                                            trade_name = b.trade_name,
                                            units = a.Unit,
                                            trade_code = b.trade_code,
                                            ISActive = a.IsActive

                                        }).ToList();
                    data.AffiliationDocs = GetAllAffiliationDoc()?.Where(a =>  a.Institute_id == data.iti_college_id && a.Trade_Id==null && a.Unit==null && a.Shift==null).ToList();
                    if (data.AffiliationDocs.Count() != 0)
                    {
                        data.AffiliationDoc = GetAllAffiliationDoc()?.Where(a =>  a.Institute_id == data.iti_college_id && a.Trade_Id == null && a.Unit == null && a.Shift == null).Last();
                    }
                   
                    if (data.AffiliationDocs != null)
                    {
                        foreach (var item in data.AffiliationDocs)
                        {
                            item.FileName = System.IO.Path.GetFileName(item.FileName);
                        }
                    }
                    //if(data.trades_list!=null)
                    //{
                        data.TradeWiseAffiliationDoc = GetAllAffiliationDoc()?.Where(a => a.Institute_id == data.iti_college_id && a.Trade_Id != null && a.Unit == null && a.Shift == null).ToList();
                    //}
                    if (data.TradeWiseAffiliationDoc != null)
                    {
                        foreach (var item in data.TradeWiseAffiliationDoc)
                        {
                            item.FileName = System.IO.Path.GetFileName(item.FileName);
                        }
                    }


                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Publish Affiliated Colleges
        /// <summary>
        /// PublishAffiliatedColleges
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public string PublishAffiliatedCollegesDLL(int CollegeId)
        {
            try
            {
                var publishCollege = _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId).FirstOrDefault();
                publishCollege.StatusId = (int)CsystemType.getCommon.Published;

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "success";
        }
        #endregion

        #region Approve Affiliated Colleges
        /// <summary>
        /// ApproveAffiliatedCollege
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public string ApproveAffiliatedCollegeDLL(int CollegeId)
        {
            try
            {
                var publishCollege = _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId).FirstOrDefault();
                publishCollege.StatusId = (int)CsystemType.getCommon.Approved;

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "success";
        }
        #endregion

        #region Reject Affiliated Colleges
        /// <summary>
        /// RejectAffiliatedCollege
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public string RejectAffiliatedCollegeDLL(int CollegeId)
        {
            try
            {
                var publishCollege = _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId).FirstOrDefault();
                publishCollege.StatusId = (int)CsystemType.getCommon.Rejected;

                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "success";
        }
        #endregion

        #region Get A Trade Details
        /// <summary>
        /// GetATradeDetailsDLL
        /// </summary>
        /// <param name="Trade_Id"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetATradeDetailsDLL(int Trade_Id, int role_id)
        {
            try
            {
                AffiliationCollegeDetails data = (from b in _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == Trade_Id)

                                                  join a in _db.tbl_iti_college_details on b.ITICode equals a.iti_college_id into aa
                                                  from a in aa.DefaultIfEmpty()
                                                  join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                                                  from c in cc.DefaultIfEmpty()
                                                  join p in _db.tbl_division_master on c.division_id equals p.division_id into pp
                                                  from p in pp.DefaultIfEmpty()
                                                  join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                                                  from d in dd.DefaultIfEmpty()
                                                  join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                                                  from e in ee.DefaultIfEmpty()
                                                  join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                                                  from f in ff.DefaultIfEmpty()
                                                  join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                                                  from g in gg.DefaultIfEmpty()
                                                  join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                                                  from h in hh.DefaultIfEmpty()
                                                  join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                                                  from i in ii.DefaultIfEmpty()
                                                  join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                                                  from j in jj.DefaultIfEmpty()
                                                  join k in _db.tbl_status_master on a.StatusId equals k.StatusId
                                                  join l in _db.tbl_CSS on a.css_code equals l.CSSCode into ll
                                                  from l in ll.DefaultIfEmpty()
                                                  join m in _db.tbl_trade_scheme on a.Scheme equals m.ts_id into mm
                                                  from m in mm.DefaultIfEmpty()
                                                  join n in _db.tbl_course_type_mast on a.CourseCode equals n.course_id into nn
                                                  from n in nn.DefaultIfEmpty()
                                                  join o in _db.tbl_trade_sector on j.sector_id equals o.trade_sector_id into oo
                                                  from o in oo.DefaultIfEmpty()
                                                  join q in _db.tbl_trade_type_mast on j.trade_type_id equals q.trade_type_id into qq
                                                  from q in qq.DefaultIfEmpty()

                                                  select new AffiliationCollegeDetails
                                                  {
                                                      name_of_iti = a.iti_college_name,
                                                      mis_code = a.MISCode,
                                                      type_of_iti = i.Institute_type,
                                                      type_of_iti_id = a.Insitute_TypeId,
                                                      trade = j.trade_name,
                                                      district = c.district_ename,
                                                      dist_id = a.district_id,
                                                      taluka = d.taluk_ename,
                                                      taluk_id = a.taluk_id,
                                                      panchayat = e.grama_panchayat_name,
                                                      village_id = a.village_or_town,
                                                      consti_id = a.Constituency,
                                                      build_up_area = a.BuildUpArea,
                                                      css_code_id = a.css_code,
                                                      geo_location = a.geo,
                                                      address = a.college_address,
                                                      location_type = h.location_name,
                                                      location_type_id = a.location_id,
                                                      email = a.email_id,
                                                      phone_number = a.phone_num,
                                                      affiliation_date = a.AffiliationDate,
                                                      //no_trades = column missing
                                                      NoofTrades = a.NoofTrades,
                                                      no_shifts = a.NoOfShifts,
                                                      iti_college_id = a.iti_college_id,
                                                      no_units = b.Unit,
                                                      state = "Karnataka",
                                                      date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                                      trade_id = j.trade_id,
                                                      FileUploadPath = a.UploadAffiliationDoc,
                                                      status = k.StatusName,
                                                      constituency = g.Constituencies,
                                                      village = f.village_ename,
                                                      trade_iti_id = b.Trade_ITI_id,
                                                      css_code = l.CSS_Scheme,
                                                      Pincode = a.PinCode,
                                                      status_id = b.StatusId,
                                                      en_edit = b.FlowId == role_id ? true : false,
                                                      Website = a.Website,
                                                      AffiliationOrderNo = a.AffiliationOrderNo,
                                                      Scheme = a.Scheme,
                                                      order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                                      scheme_name = m.trade_scheme,
                                                      course_code = a.CourseCode,
                                                      course_name = n.course_type_name,
                                                      division = p.division_name,
                                                      sector = o.trade_sector,
                                                      trade_code = j.trade_code,
                                                      batch_size = j.trade_seating,
                                                      duration = j.trade_duration,
                                                      AidedUnaidedTrade = b.AidedUnaidedTrade,
                                                      trade_type = q.trade_type_name,
                                                      division_id = a.division_id,
                                                      NewAffiliationOrderNo=a.NewAffiliationOrderNo,
                                                      NewAffiliationOrderNoDate=a.NewAffiliationOrderNoDate.ToString(),
                                                      UploadTradeAffiliationDoc=a.UploadTradeAffiliationDoc,
                                                      



            }).FirstOrDefault();
                if (data != null)
                {
                    data.shifts = new List<TradeShift>();
                    data.AffiliationDocs = new List<AffiliationDocuments>();

                    data.shifts = GetAllTradeShiftsDLL(data.trade_iti_id);
                    data.AffiliationDocs = GetAllAffiliationDoc()?.Where(a=>a.Trade_Id== data.trade_id && a.Institute_id==data.iti_college_id && a.Unit==null && a.Shift==null ).ToList();
                    if(data.AffiliationDocs!=null)
                    {
                        foreach(var item in data.AffiliationDocs)
                        {
                            item.FileName = System.IO.Path.GetFileName(item.FileName);
                            //item.Affiliation_date=DateTime.ParseExact(dateString, "yyyy/mm/dd", CultureInfo.InvariantCulture)
                        }
                    }
                    if (data.FileUploadPath != "" && data.FileUploadPath != null)
                    {
                        data.isSelect = System.IO.File.Exists(data.FileUploadPath);
                        data.Filename = System.IO.Path.GetFileName(data.FileUploadPath);
                    }

                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Status
        /// <summary>
        /// GetAllStatusDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllStatusDLL()
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
        #endregion

        #region Get All Users/Followers
        /// <summary>
        /// GetAllUsers
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllUsersDLL()
        {
            try
            {
                var list = (from a in _db.tbl_role_master.Where(a => a.role_is_active == true).OrderByDescending(a => a.role_seniority_no)
                            select new SelectListItem
                            {
                                Text = a.role_description,
                                Value = a.role_id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Affiliate Colleges
        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>

        public List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL(int role_id)
        {
            try
            {
                var list = (from aa in _db.tbl_ITI_Trade_Histories.Where(a => a.FlowId == role_id)
                            join b in _db.tbl_ITI_Trade on aa.Trade_ITI_id equals b.Trade_ITI_id
                            join a in _db.tbl_iti_college_details on b.ITICode equals a.iti_college_id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join div in _db.tbl_division_master on a.division_id equals div.division_id into did
                            from div in did.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join ct in _db.tbl_course_type_mast on a.CourseCode equals ct.course_id into cst
                            from ct in cst.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            from rl in _db.tbl_role_master.Where(f => f.role_id == b.FlowId).DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                //FileUploadPath = a.UploadAffiliationDoc,
                                status = (k.StatusId != 6 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                flow_id = b.FlowId,
                                //flow_id=aa.FlowId,
                                status_id = b.StatusId,
                                color_flag = b.color_flag,
                                course_name=ct.course_type_name,
                                division=div.division_name

                            }).ToList();

                list = list.GroupBy(a => a.trade_iti_id).Select(a => new AffiliationCollegeDetails { name_of_iti = a.Select(z => z.name_of_iti).FirstOrDefault(), mis_code = a.Select(z => z.mis_code).FirstOrDefault(), trade = a.Select(z => z.trade).FirstOrDefault(), no_units = a.Select(z => z.no_units).FirstOrDefault(), district = a.Select(z => z.district).FirstOrDefault(), taluka = a.Select(z => z.taluka).FirstOrDefault(), status = a.Select(z => z.status).FirstOrDefault(), FileUploadPath = a.Select(z => z.FileUploadPath).FirstOrDefault(), trade_iti_id = a.Select(z => z.trade_iti_id).FirstOrDefault(), status_id = a.Select(z => z.status_id).FirstOrDefault(), CreatedOn = a.Select(z => z.CreatedOn).FirstOrDefault(), iti_college_id = a.Select(z => z.iti_college_id).FirstOrDefault(), color_flag = a.Select(z => z.color_flag).FirstOrDefault(), course_name = a.Select(z => z.course_name).FirstOrDefault(), division = a.Select(z => z.division).FirstOrDefault(), trade_id = a.Select(z => z.trade_id).FirstOrDefault() }).ToList();
                foreach (var itemdocs in list)
                {
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == itemdocs.iti_college_id && a.Trade_Id == itemdocs.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { itemdocs.FileUploadPath = docs.FileName; }
                }
                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<AffiliationCollegeDetails> GetAllAffiliateCollegeListDLL1()
        {
            try
            {
                var list = (from aa in _db.tbl_ITI_Trade_Histories
                            join b in _db.tbl_ITI_Trade on aa.Trade_ITI_id equals b.Trade_ITI_id
                            join a in _db.tbl_iti_college_details on b.ITICode equals a.iti_college_id
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join div in _db.tbl_division_master on a.division_id equals div.division_id into dsm
                            from div in dsm.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join rl in _db.tbl_role_master on b.FlowId equals rl.role_id into rm
                            from rl in rm.DefaultIfEmpty()
                            join ct in _db.tbl_course_type_mast on a.CourseCode equals ct.course_id into cst
                            from ct in cst.DefaultIfEmpty()
                                //from rl in _db.tbl_role_master.Where(f => f.role_id == b.FlowId).DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                //FileUploadPath = a.UploadAffiliationDoc,
                                status = (k.StatusId != 6 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                CreatedOn = b.CreatedOn,
                                flow_id = b.FlowId,
                                //flow_id=aa.FlowId,
                                status_id = b.StatusId,
                                color_flag = b.color_flag,
                                course_name=ct.course_type_name,
                                division=div.division_name,
                                
                            }).ToList();

                list = list.GroupBy(a => a.trade_iti_id).Select(a => new AffiliationCollegeDetails { name_of_iti = a.Select(z => z.name_of_iti).FirstOrDefault(), mis_code = a.Select(z => z.mis_code).FirstOrDefault(), trade = a.Select(z => z.trade).FirstOrDefault(), no_units = a.Select(z => z.no_units).FirstOrDefault(), district = a.Select(z => z.district).FirstOrDefault(), taluka = a.Select(z => z.taluka).FirstOrDefault(), status = a.Select(z => z.status).FirstOrDefault(), FileUploadPath = a.Select(z => z.FileUploadPath).FirstOrDefault(), trade_iti_id = a.Select(z => z.trade_iti_id).FirstOrDefault(), status_id = a.Select(z => z.status_id).FirstOrDefault(), CreatedOn = a.Select(z => z.CreatedOn).FirstOrDefault(), iti_college_id = a.Select(z => z.iti_college_id).FirstOrDefault(), color_flag = a.Select(z => z.color_flag).FirstOrDefault(), course_name= a.Select(z => z.course_name).FirstOrDefault(), division = a.Select(z => z.division).FirstOrDefault(), trade_id = a.Select(z => z.trade_id).FirstOrDefault() }).ToList();
                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }
                foreach (var itemdocs in list)
                {
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == itemdocs.iti_college_id && a.Trade_Id == itemdocs.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { itemdocs.FileUploadPath = docs.FileName; }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Get All My Affilated Colleges
        /// <summary>
        /// Getting all records by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<AffiliationCollegeDetails> GetAllMyAffiliatedCollegesDLL(int college_id)
        {
            try
            {
                var list = (from aa in _db.tbl_ITI_Trade_Histories.Where(a => a.ITICode == college_id)
                            join a in _db.tbl_iti_college_details on aa.ITICode equals a.iti_college_id
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join did in _db.tbl_division_master on a.division_id equals did.division_id into dnm
                            from did in dnm.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join cm in _db.tbl_course_type_mast on a.CourseCode equals cm.course_id into cmt
                            from cm in cmt.DefaultIfEmpty()
                            join k in _db.tbl_status_master on b.StatusId equals k.StatusId
                            join rl in _db.tbl_role_master on b.FlowId equals rl.role_id into rm
                            from rl in rm.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                village_id = a.village_or_town,
                                consti_id = a.Constituency,
                                build_up_area = a.BuildUpArea,
                                css_code_id = a.css_code,
                                geo_location = a.geo,
                                address = a.college_address,
                                location_type = h.location_name,
                                email = a.email_id,
                                phone_number = a.phone_num,
                                affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = college_id,
                                no_units = b.Unit,
                                state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                //FileUploadPath = a.UploadAffiliationDoc,
                                status = (k.StatusId != 6 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                //status = k.StatusName + " - " + rl.role_DescShortForm,
                                //status = k.StatusName,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                course_name=cm.course_type_name,
                                division=did.division_name

                            }).ToList();
                list = list.GroupBy(a => a.trade_iti_id).Select(a => new AffiliationCollegeDetails { name_of_iti = a.Select(z => z.name_of_iti).FirstOrDefault(), mis_code = a.Select(z => z.mis_code).FirstOrDefault(), trade = a.Select(z => z.trade).FirstOrDefault(), no_units = a.Select(z => z.no_units).FirstOrDefault(), district = a.Select(z => z.district).FirstOrDefault(), taluka = a.Select(z => z.taluka).FirstOrDefault(), status = a.Select(z => z.status).FirstOrDefault(), FileUploadPath = a.Select(z => z.FileUploadPath).FirstOrDefault(), trade_iti_id = a.Select(z => z.trade_iti_id).FirstOrDefault(), status_id = a.Select(z => z.status_id).FirstOrDefault(), CreatedOn = a.Select(z => z.CreatedOn).FirstOrDefault(), iti_college_id = a.Select(z => z.iti_college_id).FirstOrDefault(), course_name = a.Select(z => z.course_name).FirstOrDefault(), division = a.Select(z => z.division).FirstOrDefault(), trade_id = a.Select(z => z.trade_id).FirstOrDefault() }).ToList();
                foreach (var itemdocs in list)
                {
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == itemdocs.iti_college_id && a.Trade_Id == itemdocs.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { itemdocs.FileUploadPath = docs.FileName; }
                }
                foreach (var item in list)
                {
                    var shifts = GetAllTradeShiftsDLL(item.trade_iti_id);
                    item.no_shifts = shifts.Count;
                    item.no_units = shifts.Count;
                }


                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Trade Transaction
        /// <summary>
        /// AddTradeTransaction
        /// </summary>
        /// <param name="aTrade"></param>
        /// <returns></returns>
        public string AddTradeTransaction(AffiliationTrade aTrade)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    var clg = _db.tbl_iti_college_details.Where(a => a.MISCode == aTrade.mis_code).FirstOrDefault();
                    if (clg != null)
                    {
                        //clg = new tbl_iti_college_details();

                        clg.iti_college_name = aTrade.name_of_iti;
                        clg.Insitute_TypeId = aTrade.type_of_iti_id;
                        clg.location_id = aTrade.location_type_id;
                        clg.MISCode = aTrade.mis_code;
                        // update.css_code = Affi.css_code_id;
                        clg.district_id = aTrade.dist_id;
                        clg.taluk_id = aTrade.taluk_id;
                        clg.Constituency = aTrade.consti_id;
                        //clg.Panchayat = aTrade.pancha_id;
                        //clg.village_or_town = aTrade.village_id;
                        //clg.BuildUpArea = aTrade.build_up_area;
                        clg.college_address = aTrade.address;
                        clg.geo = aTrade.geo_location;
                        clg.email_id = aTrade.email;
                        clg.phone_num = aTrade.phone_number;
                        clg.AffiliationDate = aTrade.affiliation_date;

                        clg.updated_by = aTrade.CreatedBy;
                        clg.PinCode = aTrade.Pincode;
                        clg.Website = aTrade.Website;
                        clg.AffiliationOrderNo = aTrade.AffiliationOrderNo;
                        clg.AffiliationOrderNoDate = aTrade.AffiliationOrderNoDate;
                        clg.Scheme = aTrade.Scheme;
                        clg.CourseCode = aTrade.course_code;
                        clg.division_id = aTrade.division_id;
                        clg.NewAffiliationOrderNo = aTrade.NewAffiliationOrderNo;
                        clg.NewAffiliationOrderNoDate = aTrade.NewAffiliationOrderNoDate;
                        clg.AidedUnaidedTrade = aTrade.AidedUnaidedTrade;
                        //Asave pdf doc for dd login after published/affiliated status
                        //if(aTrade.file_upload_path!=null)
                        //{ clg.UploadAffiliationDoc = aTrade.file_upload_path; }
                        
                        _db.SaveChanges();

                    }
                    if(clg!=null/*iti_college_id !=0 && aTrade.trade_id!=null*/)
                    {
                        var chkExistRec = _db.tbl_Affiliation_documents.Where(a => a.Institute_id == clg.iti_college_id && a.Trade_Id == aTrade.trade_id).FirstOrDefault();
                        tbl_Affiliation_documents DocUpload = new tbl_Affiliation_documents();

                        if (chkExistRec == null)
                        {


                            if (aTrade.file_upload_path != null)
                            {
                                DocUpload.AffiliationOrder_Number = aTrade.AffiliationOrderNo;
                                DocUpload.Affiliation_date = aTrade.AffiliationOrderNoDate;
                                DocUpload.FileName = aTrade.file_upload_path;
                                DocUpload.Institute_id = clg.iti_college_id;
                                DocUpload.Trade_Id = aTrade.trade_id;
                                _db.tbl_Affiliation_documents.Add(DocUpload);
                            }

                        }
                        else
                        {
                            if (aTrade.file_upload_path != null)
                            {
                                chkExistRec.FileName = aTrade.file_upload_path;
                            }

                        }
                        _db.SaveChanges();
                    }
                    

                    if (aTrade.status_id == (int)CsystemType.getCommon.Approved)
                    {
                        var Shifts = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == aTrade.trade_iti_id && a.Status == (int)CsystemType.getCommon.Submitted).ToList();
                        foreach (var shft in Shifts)
                        {
                            shft.IsActive = true;
                            shft.Status = (int)CsystemType.getCommon.Published;

                            _db.SaveChanges();
                        }
                    }

                    //Update ITI_Trade Table
                    var Update = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == aTrade.trade_iti_id).FirstOrDefault();

                    if (aTrade.status_id == (int)CsystemType.getCommon.Sent_for_Correction || aTrade.status_id == (int)CsystemType.getCommon.Rejected)
                    {
                        Update.FlowId = (int)CsystemType.getCommon.CaseWorker;
                    }
                    else if (aTrade.status_id == (int)CsystemType.getCommon.Sent_back)
                    {
                        //Update.FlowId = (int)CsystemType.getCommon.CaseWorker;
                        //Update.FlowId = (int)CsystemType.getCommon.CaseWorker;
                        Update.FlowId = aTrade.flow_id;
                    }
                    else 
                    {
                        Update.FlowId = aTrade.flow_id;
                    }


                    if (aTrade.status_id == (int)CsystemType.getCommon.Approved)
                    {
                        Update.StatusId = (int)CsystemType.getCommon.Published;
                        Update.color_flag = null;
                        //Update.FlowId = (int)CsystemType.getCommon.CaseWorker;

                        if (_db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == aTrade.trade_iti_id).Any(a => a.IsActive))
                        {
                            Update.IsActive = true;
                        }
                        else
                        {
                            Update.IsActive = false;
                        }

                    }
                    else if(aTrade.status_id!=null)
                    {
                        Update.StatusId = aTrade.status_id;
                    }
                    if(aTrade.remarks!=null)
                    {
                        Update.Remarks = aTrade.remarks;
                        Update.CreatedOn = DateTime.Now;
                        Update.CreatedBy = aTrade.CreatedBy;
                    }
                    
                   

                    _db.SaveChanges();

                    //Record History
                    //checking remarks null when updating only document for affiliated institute status 
                    if(aTrade.remarks!=null)
                    {
                    var anEntry = new tbl_ITI_Trade_History();

                    anEntry.StatusId = Update.StatusId;
                    anEntry.IsActive = Update.IsActive;
                    anEntry.FlowId = Update.FlowId;
                    anEntry.ITICode = Update.ITICode;
                    anEntry.Remarks = aTrade.remarks;
                    anEntry.TradeCode = Convert.ToInt32(Update.TradeCode);
                    anEntry.Trade_ITI_id = Update.Trade_ITI_id;
                    anEntry.CreatedOn = DateTime.Now;
                    anEntry.Unit = Update.Unit;
                    anEntry.CreatedBy = aTrade.CreatedBy;

                    _db.tbl_ITI_Trade_Histories.Add(anEntry);
                    _db.SaveChanges();

                    if (aTrade.status_id == (int)CsystemType.getCommon.Approved)
                    {

                        if (!_db.tbl_ITI_Trade.Where(a => a.ITICode == Update.ITICode).Any(a => a.IsActive))
                        {
                            var Update_College = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Update.ITICode).FirstOrDefault();
                            Update_College.is_active = false;
                            Update_College.StatusId = (int)CsystemType.getCommon.Published;
                            Update_College.color_flag = null;

                            _db.SaveChanges();
                        }
                        else
                        {
                            var Update_College = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Update.ITICode).FirstOrDefault();
                            Update_College.is_active = true;
                            Update_College.StatusId = (int)CsystemType.getCommon.Published;
                            Update_College.color_flag = null;

                            _db.SaveChanges();
                        }
                        
                    }
                    else if (aTrade.status_id == (int)CsystemType.getCommon.Review_and_Recommend)
                    {
                        var Update_College = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Update.ITICode).FirstOrDefault();
                        if(aTrade.NewAffiliationOrderNo!=null && aTrade.NewAffiliationOrderNoDate!=null)
                        {
                            Update_College.NewAffiliationOrderNo = aTrade.NewAffiliationOrderNo;
                            Update_College.NewAffiliationOrderNoDate = aTrade.NewAffiliationOrderNoDate;
                        }
                      
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
        #endregion

        #region Get All Active and Deactive Trades

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<ActiveandDeactiveDeatils> GetAllActiveandDeactiveDeatilsDLL()
        {
            try
            {

                List<ActiveandDeactiveDeatils> items = (from trade in _db.tbl_ITI_Trade
                                                        join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                        join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                        where trade.IsActive == true
                                                        select new ActiveandDeactiveDeatils
                                                        {
                                                            iti_college = clz.iti_college_name,
                                                            trades = master.trade_name,
                                                            units = clz.Units,
                                                            status_id = clz.StatusId,
                                                            ActiveDeActive = trade.ActiveDeActive,
                                                            Trade_ITI_id = trade.Trade_ITI_id,
                                                        }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        #endregion

        #region Get All Active and Deactive Trades forpopup

        /// <summary>
        /// GetAllAffiliationCollegeDetailsforpopupDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<GetAllActiveandDeactiveDeatilsforpopup> GetAllActiveandDeactiveDeatilsforpopupDLL(int id)
        {
            try
            {
                var list2 = (from a in _db.tbl_iti_college_details.Where(a => a.is_active == true)
                             join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode
                             join c in _db.tbl_trade_mast on b.TradeCode equals c.trade_id
                             where b.Trade_ITI_id == id
                             select new GetAllActiveandDeactiveDeatilsforpopup
                             {
                                 iti_college = a.iti_college_name,
                                 trades = c.trade_name,
                                 units = a.Units,
                                 status_id = b.StatusId,
                                 Trade_ITI_id = b.Trade_ITI_id,
                                 IsActive = b.IsActive
                             }).ToList();

                return list2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateActiveDeactiveHistoryDLL(tbl_ITI_Trade_ActiveStatus_History model)
        {
            try
            {
                _db.tbl_ITI_Trade_ActiveStatus_History.Add(model);

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public bool UpdateTradeActiveDeactiveDLL(tbl_ITI_Trade model)
        {
            try
            {
                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        public tbl_ITI_Trade GetTradeDLL(int id)
        {
            return _db.tbl_ITI_Trade.Where(x => x.Trade_ITI_id == id).FirstOrDefault();
        }


        #endregion

        #region Get Trade Histroy
        /// <summary>
        /// GetTradeHistoriesDLL
        /// </summary>
        /// <param name="Trade_id"></param>
        /// <returns></returns>
        public List<TradeHistory> GetTradeHistoriesDLL(int Trade_id)
        {
            try
            {
                List<TradeHistory> list = (from a in _db.tbl_ITI_Trade_Histories.Where(a => a.Trade_ITI_id == Trade_id)
                                           join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_id
                                           join c in _db.tbl_role_master on a.FlowId equals c.role_id into rlm
                                           from c in rlm.DefaultIfEmpty()
                                           join e in _db.tbl_role_master on a.CreatedBy equals e.role_id into rmt
                                           from e in rmt.DefaultIfEmpty()
                                           join d in _db.tbl_status_master on a.StatusId equals d.StatusId into cmt
                                           from d in cmt.DefaultIfEmpty()

                                           select new TradeHistory
                                           {
                                               CreatedBy = a.CreatedBy,
                                               CreatedOn = a.CreatedOn,
                                               FileUploadPath = a.FileUploadPath,
                                               FlowId = a.FlowId,
                                               IsActive = a.IsActive,
                                               ITICode = a.ITICode,
                                               Remarks = a.Remarks,
                                               StatusId = a.StatusId,
                                               TradeCode = a.TradeCode,
                                               Trade_ITI_His_id = a.Trade_ITI_His_id,
                                               Trade_ITI_id = a.Trade_ITI_id,
                                               Unit = a.Unit,
                                               TradeName = b.trade_name,
                                               Flow_user = c.role_description??"-",
                                               date = a.CreatedOn.ToString(),
                                               Status = d.StatusName,
                                               sent_by = e.role_description,
                                               created_by = c.role_description,

                                           }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Trade Histroy Institute Wise
        /// <summary>
        /// GetTradeHistoriesDLL
        /// </summary>
        /// <param name="Trade_id"></param>
        /// <returns></returns>
        public List<TradeHistory> GetAllTradeHistoriesDLL(int college_id)
        {
            try
            {
                List<TradeHistory> list = (from a in _db.tbl_ITI_Trade_Histories.Where(a => a.ITICode == college_id)
                                           join b in _db.tbl_trade_mast on a.TradeCode equals b.trade_id
                                           join c in _db.tbl_role_master on a.FlowId equals c.role_id into jj
                                           from c in jj.DefaultIfEmpty()
                                           join d in _db.tbl_role_master on a.CreatedBy equals d.role_id
                                           join e in _db.tbl_status_master on a.StatusId equals e.StatusId
                                           select new TradeHistory
                                           {
                                               CreatedBy = a.CreatedBy,
                                               CreatedOn = a.CreatedOn,
                                               FileUploadPath = a.FileUploadPath,
                                               FlowId = a.FlowId,
                                               IsActive = a.IsActive,
                                               ITICode = a.ITICode,
                                               Remarks = a.Remarks,
                                               StatusId = a.StatusId,
                                               TradeCode = a.TradeCode,
                                               Trade_ITI_His_id = a.Trade_ITI_His_id,
                                               Trade_ITI_id = a.Trade_ITI_id,
                                               Unit = a.Unit,
                                               TradeName = b.trade_name,
                                               Flow_user = c.role_description,
                                               date = a.CreatedOn.ToString(),
                                               created_by = d.role_description,
                                               Status = e.StatusName

                                           }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Trade Shifts
        /// <summary>
        /// GetAllTradeShiftsDLL
        /// </summary>
        /// <param name="iti_trade_id"></param>
        /// <returns></returns>
        public List<TradeShift> GetAllTradeShiftsDLL(int iti_trade_id)
        {
            try
            {
                List<TradeShift> list = (from a in _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == iti_trade_id)
                                         select new TradeShift
                                         {
                                             CreatedBy = a.CreatedBy,
                                             ITI_Trade_Id = a.ITI_Trade_Id,
                                             CreatedOn = a.CreatedOn,
                                             Dual_System = a.Dual_System,
                                             IsActive = a.IsActive,
                                             IsPPP = a.IsPPP,
                                             ITI_Trade_Shift_Id = a.ITI_Trade_Shift_Id,
                                             Shift = a.Shift,
                                             Units = a.Units,
                                             Status = a.Status

                                         }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All  Shifts
        /// <summary>
        /// GetAllTradeShiftsDLL
        /// </summary>
        /// <param name="iti_shift_id"></param>
        /// <returns></returns>
        public List<TradeShift> GetAllShiftsDLL(int iti_shift_id)
        {
            try
            {
                List<TradeShift> list = (from a in _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == iti_shift_id)
                                         select new TradeShift
                                         {
                                             CreatedBy = a.CreatedBy,
                                             ITI_Trade_Id = a.ITI_Trade_Id,
                                             CreatedOn = a.CreatedOn,
                                             Dual_System = a.Dual_System,
                                             IsActive = a.IsActive,
                                             IsPPP = a.IsPPP,
                                             ITI_Trade_Shift_Id = a.ITI_Trade_Shift_Id,
                                             Shift = a.Shift,
                                             Units = a.Units,
                                             Status = a.Status

                                         }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Uploaded Affiliation
        /// <summary>
        /// GetAllUploadedAffiliation
        /// </summary>
        /// <returns></returns>
        /// 

        //public List<AffiliationCollegeDetails> GetAllUploadedAffiliationDLL()
        //{
        //    try
        //    {
        //        List<AffiliationCollegeDetails> list = (from b in _db.temp_Tbl_ITI_Trades
        //                                                join a in _db.temp_Tbl_Iti_College_Details on b.ITICode equals a.iti_college_id_temp
        //                                                // join a in _db.temp_Tbl_Iti_College_Details on b.Trade_ITI_id_temp equals a.iti_college_id_temp
        //                                                join c in _db.tbl_district_master on a.district equals c.district_ename into cc
        //                                                from c in cc.DefaultIfEmpty()
        //                                                join d in _db.tbl_trade_mast on b.TradeCode equals d.trade_name into dd
        //                                                from d in dd.DefaultIfEmpty()
        //                                                join j in _db.tbl_division_master on a.division equals j.division_name into jj
        //                                                from j in jj.DefaultIfEmpty()
        //                                                join l in _db.tbl_course_type_mast on a.CourseCode equals l.course_type_name into ll
        //                                                from l in ll.DefaultIfEmpty()
        //                                                where b.IsUploaded == false

        //                                                select new AffiliationCollegeDetails
        //                                                {
        //                                                    name_of_iti = a.iti_college_name,
        //                                                    mis_code = a.MISCode,
        //                                                    trade = b.TradeCode,
        //                                                    district = a.district,
        //                                                    taluka = a.taluk,
        //                                                    //nooftrades=a.NoofTrades,
        //                                                    nooftrades = b.NoofTrades,
        //                                                    status = "Uploaded",
        //                                                    CreatedOn = a.creation_datetime,
        //                                                    date = a.creation_datetime.ToString(),
        //                                                    status_id = b.StatusId,
        //                                                    iti_college_id = a.iti_college_id_temp,
        //                                                    isUploaded = true,
        //                                                    //course_code = l.course_id,
        //                                                    //division_id = j.division_id,
        //                                                    dist_id = c.district_lgd_code,
        //                                                    trade_id = d.trade_id,
        //                                                    trade_iti_id = b.Trade_ITI_id_temp
        //                                                    //}).OrderBy(x => x.mis_code).ToList();
        //                                                    //}).Distinct().OrderBy(x => x.mis_code).ToList();
        //                                                }).ToList();

        //        //list = list.GroupBy(a => a.iti_college_id).Select(z => new AffiliationCollegeDetails
        //        //// list = list.GroupBy(a => new { a.mis_code}).Select(z => new AffiliationCollegeDetails
        //        //{
        //        //    name_of_iti = z.Select(s => s.name_of_iti).FirstOrDefault(),
        //        //    mis_code = z.Select(s => s.mis_code).FirstOrDefault(),
        //        //    trade = z.Select(s => s.trade).FirstOrDefault(),
        //        //    nooftrades = z.Select(s => s.nooftrades).FirstOrDefault(),
        //        //    no_units = Convert.ToInt32(z.Select(s => s.units).FirstOrDefault()),
        //        //    district = z.Select(s => s.district).FirstOrDefault(),
        //        //    taluka = z.Select(s => s.taluka).FirstOrDefault(),
        //        //    status = z.Select(s => s.status).FirstOrDefault(),
        //        //    CreatedOn = z.Select(s => s.CreatedOn).FirstOrDefault(),
        //        //    date = z.Select(s => s.date).FirstOrDefault(),
        //        //    status_id = z.Select(s => s.status_id).FirstOrDefault(),
        //        //    iti_college_id = z.Select(s => s.iti_college_id).FirstOrDefault(),
        //        //    isUploaded = z.Select(s => s.isUploaded).FirstOrDefault(),
        //        //   // course_code = z.Select(s => s.course_code).FirstOrDefault(),
        //        //   // division_id = z.Select(s => s.division_id).FirstOrDefault(),
        //        //    dist_id = z.Select(s => s.dist_id).FirstOrDefault(),
        //        //    trade_id = z.Select(s => s.trade_id).FirstOrDefault(),
        //        //    trade_iti_id = z.Select(s => s.trade_iti_id).FirstOrDefault()
        //        //    //}).OrderBy(x => x.mis_code).ToList();
        //        //    //}).Distinct().OrderBy(x => x.mis_code).ToList();
        //        //}).ToList();
        //        foreach (var item in list)
        //        {
        //            int units = _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == item.trade_iti_id).Count();
        //            item.no_units = units;

        //            int nooftrades = _db.temp_Tbl_Iti_College_Details.Where(a => a.iti_college_id_temp == item.iti_college_id).Count();
        //            item.NoofTrades = nooftrades;

        //            //int nooftrades = _db.temp_Tbl_ITI_Trades.Where(a => a.Trade_ITI_id_temp == item.iti_college_id).Count();
        //            //item.NoofTrades = nooftrades;

        //            //int nooftrades = _db.tbl_trade_mast.Where(a => a.trade_type_id == item.iti_college_id).Count();
        //            //item.NoofTrades = nooftrades;
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public List<AffiliationCollegeDetails> GetAllUploadedAffiliationDLL()
        {
            try
            {
                List<AffiliationCollegeDetails> list = (from a in _db.temp_Tbl_Iti_College_Details
                                                        join b in _db.temp_Tbl_ITI_Trades on a.iti_college_id_temp equals b.ITICode
                                                        join c in _db.tbl_district_master on a.district equals c.district_ename
                                                        join d in _db.tbl_trade_mast on b.TradeCode equals d.trade_name
                                                        join e in _db.tbl_division_master on a.division equals e.division_name
                                                        join f in _db.tbl_course_type_mast on a.CourseCode equals f.course_type_name
                                                        where b.IsUploaded == false && d.trdae_is_active==true

                                                        select new AffiliationCollegeDetails
                                                        {
                                                            name_of_iti = a.iti_college_name,
                                                            mis_code = a.MISCode,
                                                            trade = b.TradeCode,
                                                            district = a.district,
                                                            taluka = a.taluk,
                                                            //nooftrades=a.NoofTrades,
                                                            nooftrades = b.NoofTrades,
                                                            status = "Uploaded",
                                                            CreatedOn = a.creation_datetime,
                                                            date = SqlFunctions.DatePart("day", a.creation_datetime) + "/" + SqlFunctions.DatePart("m", a.creation_datetime) + "/" + SqlFunctions.DatePart("year", a.creation_datetime),//a.creation_datetime.ToString(),
                                                            status_id = b.StatusId,
                                                            iti_college_id = a.iti_college_id_temp,
                                                            isUploaded = true,
                                                            course_code = f.course_id,
                                                            division_id = e.division_id,
                                                            dist_id = c.district_lgd_code,
                                                            trade_id = d.trade_id,
                                                            trade_iti_id = b.Trade_ITI_id_temp,
                                                            course_name=f.course_type_name,
                                                            division=e.division_name
                                                            //}).OrderBy(x => x.mis_code).ToList();
                                                          }).Distinct().OrderBy(x => x.mis_code).ToList();
                                                        //}).ToList();

                //list = list.GroupBy(a => a.iti_college_id).Select(z => new AffiliationCollegeDetails
                //// list = list.GroupBy(a => new { a.mis_code}).Select(z => new AffiliationCollegeDetails
                //{
                //    name_of_iti = z.Select(s => s.name_of_iti).FirstOrDefault(),
                //    mis_code = z.Select(s => s.mis_code).FirstOrDefault(),
                //    trade = z.Select(s => s.trade).FirstOrDefault(),
                //    nooftrades = z.Select(s => s.nooftrades).FirstOrDefault(),
                //    no_units = Convert.ToInt32(z.Select(s => s.units).FirstOrDefault()),
                //    district = z.Select(s => s.district).FirstOrDefault(),
                //    taluka = z.Select(s => s.taluka).FirstOrDefault(),
                //    status = z.Select(s => s.status).FirstOrDefault(),
                //    CreatedOn = z.Select(s => s.CreatedOn).FirstOrDefault(),
                //    date = z.Select(s => s.date).FirstOrDefault(),
                //    status_id = z.Select(s => s.status_id).FirstOrDefault(),
                //    iti_college_id = z.Select(s => s.iti_college_id).FirstOrDefault(),
                //    isUploaded = z.Select(s => s.isUploaded).FirstOrDefault(),
                //    course_code = z.Select(s => s.course_code).FirstOrDefault(),
                //    division_id = z.Select(s => s.division_id).FirstOrDefault(),
                //    dist_id = z.Select(s => s.dist_id).FirstOrDefault(),
                //    trade_id = z.Select(s => s.trade_id).FirstOrDefault(),
                //    trade_iti_id = z.Select(s => s.trade_iti_id).FirstOrDefault()
                //    //}).OrderBy(x => x.mis_code).ToList();
                //    //}).Distinct().OrderBy(x => x.mis_code).ToList();
                //}).ToList();

                foreach (var item in list)
                {
                    var shifts  = GetAllTradeShiftsDLLTemp(item.trade_iti_id);
                    item.no_units = shifts.Count;

                    int nooftrades = _db.temp_Tbl_ITI_Trades.Where(a => a.Trade_ITI_id_temp == item.trade_iti_id).Count();
                    item.NoofTrades = nooftrades;
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

       
        /// <summary>
        /// GetAllTradeShiftsDLL
        /// </summary>
        /// <param name="iti_trade_id"></param>
        /// <returns></returns>
        public List<TradeShift> GetAllTradeShiftsDLLTemp(int iti_trade_id)
        {
            try
            {
                List<TradeShift> list = (from a in _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == iti_trade_id)
                                         select new TradeShift
                                         {
                                             CreatedBy = a.CreatedBy,
                                             ITI_Trade_Id = a.ITI_Trade_Id_temp,
                                             CreatedOn = a.CreatedOn,
                                             Dual_System = a.Dual_System,
                                             IsActive = a.IsActive,
                                             IsPPP = a.IsPPP,
                                             ITI_Trade_Shift_Id = a.ITI_Trade_Shift_Id_temp,
                                             TotalShift = a.Shift,
                                             TotalUnits = a.Units,
                                            // Status = a.Status

                                         }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Affilaition Trade Code
        public Trade GetAffiliationTradeCodeDLL(int trade_id)
        {
            try
            {
                Trade trade = (from a in _db.tbl_trade_mast.Where(a => a.trade_id == trade_id)
                               join b in _db.tbl_trade_type_mast on a.trade_type_id equals b.trade_type_id into bb
                               from b in bb.DefaultIfEmpty()
                               join c in _db.tbl_trade_sector on a.sector_id equals c.trade_sector_id into cc
                               from c in cc.DefaultIfEmpty()
                               select new Trade
                               {
                                   trade_id = a.trade_id,
                                   trade_code = a.trade_code,
                                   trade_duration = a.trade_duration,
                                   trade_name = a.trade_name,
                                   trade_type_id = a.trade_type_id,
                                   trade_unit = a.trade_unit,
                                   trdae_is_active = a.trdae_is_active,
                                   trade_type = b.trade_type_name,
                                   sector = c.trade_sector,
                                   trade_seating = a.trade_seating,
                                   AidedUnaidedTrade = a.AidedUnaidedTrade

                               }).FirstOrDefault();
                return trade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete Uploaded Affiliation Institute
        /// <summary>
        /// Delete Uploaded Affiliation Institute
        /// </summary>
        /// <param name="College_Id_temp"></param>
        /// <returns></returns>
        public string DeleteUploadedAffiliationInstitute(int College_Id_temp)
        {
            try
            {
                temp_tbl_iti_college_details uploaded_insti = _db.temp_Tbl_Iti_College_Details.Where(a => a.iti_college_id_temp == College_Id_temp).FirstOrDefault();
                uploaded_insti.is_active = false;

                _db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Last Previous Role
        /// <summary>
        /// Get Last Previous Role DLL
        /// </summary>
        /// <param name="_seniority"></param>
        /// <returns></returns>
        public int GetLastPreviousRoleDLL(int role_id)
        {
            try
            {
                if (role_id == (int)CsystemType.getCommon.Deputy_Director)
                {
                    return (int)CsystemType.getCommon.Assistant_Director;
                }
                else if (role_id == (int)CsystemType.getCommon.Assistant_Director)
                {
                    return (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                }
                else if (role_id == (int)CsystemType.getCommon.OFFICE_SUPERINTENDENT)
                {
                    return (int)CsystemType.getCommon.CaseWorker;
                }
                else
                {
                    return 0;
                }
                //tbl_role_master _seniority = _db.tbl_role_master.Where(a => a.role_id == role_id).FirstOrDefault();
                //tbl_role_master _lastRole = _db.tbl_role_master.Where(a => a.role_seniority_no > _seniority.role_seniority_no).OrderBy(a => a.role_seniority_no).FirstOrDefault();
                //if(_lastRole != null)
                //{
                //    return _lastRole.role_id;
                //}
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Fetch Affiliation Institute MISCODE
        public MisCodes FetchAffiliatedInstituteMISCodesDLL(string parm, int page)
        {
            MisCodes misCodes = new MisCodes();
            misCodes.list = new List<SelectListItem>();
            bool isChanged = false;
            if (HttpContext.Current.Session["MISKey"] != null)
            {
                isChanged = HttpContext.Current.Session["MISKey"].ToString() != parm;
            }

            try
            {
                misCodes.total_count = _db.tbl_iti_college_details.Where(a => a.MISCode.Contains(parm) && (a.StatusId== (int)CsystemType.getCommon.Published || a.StatusId == (int)CsystemType.getCommon.pub)).Count();
                var pageSize = 10; // set your page size, which is number of records per page

                // var page = 1; // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

                var skip = pageSize * (page - 1);
                if (isChanged)
                {
                    skip = 0;

                }
                var canPage = skip < misCodes.total_count;

                List<SelectListItem> items = new List<SelectListItem>();

                if (!canPage)
                {
                    return misCodes;
                }
                else
                {
                    misCodes.list = (from a in _db.tbl_iti_college_details.Where(a => a.MISCode.Contains(parm) && (a.StatusId == (int)CsystemType.getCommon.Published || a.StatusId == (int)CsystemType.getCommon.pub))
                                     select new SelectListItem
                                     {
                                         Text = a.MISCode,
                                         Value = a.iti_college_id.ToString()

                                     }).OrderBy(a => a.Text).Skip(skip).Take(pageSize).ToList();
                    misCodes.Ismore = true;

                    return misCodes;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Affiliated New Trade Details
        /// <summary>
        ///  Add Affiliated New Trade Details
        /// </summary>
        /// <param name="Affi"></param>
        /// <returns></returns>
        public AffiliationCollegeDetailsTest AddNewAffiliatedInstituteTradeDLL(AffiliationCollegeDetailsTest Affi)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    NestedTradeSession session = (NestedTradeSession)HttpContext.Current.Session["TradeShift"];

                    tbl_iti_college_details anEntry = _db.tbl_iti_college_details.Where(a => a.iti_college_id == Affi.iti_college_id).FirstOrDefault();

                    if (Affi.UploadTradeAffiliationDoc != null)
                    {
                        anEntry.UploadTradeAffiliationDoc = Affi.UploadTradeAffiliationDoc;
                    }
                    if(Affi.NewAffiliationOrderNo !=null)
                    {
                        anEntry.NewAffiliationOrderNo = Affi.NewAffiliationOrderNo;
                       
                    }
                    if (Affi.NewAffiliationOrderNoDate != null)
                    {
                       anEntry.NewAffiliationOrderNoDate = Affi.NewAffiliationOrderNoDate;
                    }
                    _db.SaveChanges();

                    Affi.iti_college_id = anEntry.iti_college_id;

                    if (Affi.list_trades != null)
                    {
                        if (Affi.list_trades.Count() > 0)
                        {
                            int Status = (int)CsystemType.getCommon.Sent_for_Review;

                            foreach (var item in Affi.trades_list)
                            {
                                TradeShiftSessions shifts = session.sessions.Find(a => a.sessionKey == item.sessionKey);

                                var Add = new tbl_ITI_Trade();
                                Add.ITICode = Affi.iti_college_id;
                                Add.TradeCode = item.trade_id;
                                Add.CreatedOn = DateTime.Now;
                                Add.CreatedBy = Affi.CreatedBy;
                                Add.StatusId = Status;
                                Add.Unit = item.units;
                                Add.FlowId = Affi.flow_id;
                                Add.IsActive = false;
                                Add.FileUploadPath = item.file_upload_path;
                                Add.ActiveDeActive = true;
                                Add.color_flag = Affi.color_flag;
                                Add.AidedUnaidedTrade = Affi.AidedUnaidedTrade;

                                _db.tbl_ITI_Trade.Add(Add);
                                _db.SaveChanges();

                                var Add_His = new tbl_ITI_Trade_History();
                                Add_His.ITICode = Affi.iti_college_id;
                                Add_His.TradeCode = Convert.ToInt32(item.trade_id);
                                Add_His.CreatedOn = DateTime.Now;
                                Add_His.CreatedBy = (int)CsystemType.getCommon.CaseWorker;
                                Add_His.StatusId = Status;
                                Add_His.Unit = item.units;
                                Add_His.FlowId = Affi.flow_id;
                                Add_His.IsActive = false;
                                Add_His.Remarks = Affi.remarks;//"New Affiliation Institute Trade Added";
                                Add_His.FileUploadPath = item.file_upload_path;
                                Add_His.Trade_ITI_id = Add.Trade_ITI_id;


                                _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                _db.SaveChanges();


                                if (shifts != null)
                                {
                                    if (shifts.list != null)
                                    {

                                        foreach (var item2 in shifts.list)
                                        {

                                            var insertShift = new tbl_ITI_Trade_Shift();
                                            insertShift.CreatedBy = Affi.CreatedBy;
                                            insertShift.CreatedOn = DateTime.Now;
                                            insertShift.Dual_System = item2.Dual_System;
                                            insertShift.IsActive = false;
                                            insertShift.IsPPP = item2.IsPPP;
                                            insertShift.ITI_Trade_Id = Add.Trade_ITI_id;
                                            insertShift.Shift = item2.Shift;
                                            insertShift.Units = item2.Units;
                                            insertShift.Status = (int)CsystemType.getCommon.Submitted;

                                            _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                            _db.SaveChanges();

                                        }


                                    }

                                }
                                var Trade_Doc = new tbl_Affiliation_documents();
                                Trade_Doc.Institute_id = Affi.iti_college_id;
                                Trade_Doc.Trade_Id = Convert.ToInt32(item.trade_id);
                                Trade_Doc.FileName = Affi.UploadTradeAffiliationDoc; ;
                                Trade_Doc.IsActive = true;
                                Trade_Doc.Status = "New Trade ";
                                Trade_Doc.Flag = (int)CsystemType.getCommon.New_Trade;
                                Trade_Doc.AffiliationOrder_Number = Affi.NewAffiliationOrderNo;
                                Trade_Doc.Affiliation_date = Affi.NewAffiliationOrderNoDate; ;
                                _db.tbl_Affiliation_documents.Add(Trade_Doc);
                                _db.SaveChanges();
                            }
                        }
                    }


                    transaction.Complete();
                }

                return Affi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAffiliatedInstituteDetailsDLL
        /// <summary>
        /// GetAffiliatedInstituteDetailsDLL
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetAffiliatedInstituteDetailsDLL(int CollegeId)
        {
            try
            {
                var data = (from a in _db.tbl_iti_college_details.Where(a => a.iti_college_id == CollegeId)
                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join tm in _db.tbl_trade_mast on b.TradeCode equals tm.trade_id into tms
                            from tm in tms.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                            from e in ee.DefaultIfEmpty()
                            join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                            from f in ff.DefaultIfEmpty()
                            join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                            from g in gg.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join j in _db.tbl_course_type_mast on a.CourseCode equals j.course_id into jj
                            from j in jj.DefaultIfEmpty()
                            join k in _db.tbl_division_master on a.division_id equals k.division_id into kk
                            from k in kk.DefaultIfEmpty()
                            join l in _db.tbl_trade_scheme on a.Scheme equals l.ts_id into ll
                            from l in ll.DefaultIfEmpty()

                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                type_of_iti_id = a.Insitute_TypeId,
                                location_type_id = a.location_id,
                                mis_code = a.MISCode,
                                css_code_id = a.css_code,
                                dist_id = a.district_id,
                                taluk_id = a.taluk_id,
                                consti_id = a.Constituency,
                                pancha_id = a.Panchayat,
                                village_id = a.village_or_town,
                                build_up_area = a.BuildUpArea,
                                address = a.college_address,
                                geo_location = a.geo,
                                email = a.email_id,
                                affiliation_date = a.AffiliationDate,
                                phone_number = a.phone_num,
                                no_units = a.Units,
                                no_shifts = a.NoOfShifts,
                                Pincode = a.PinCode,
                                division_id = a.division_id,
                                iti_college_id = a.iti_college_id,
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                district = c.district_ename,
                                taluka = d.taluk_ename,
                                panchayat = e.grama_panchayat_name,
                                location_type = h.location_name,
                                type_of_iti = i.Institute_type,
                                //FileUploadPath = a.UploadAffiliationDoc,
                                constituency = g.Constituencies,
                                village = f.village_ename,
                                course_code = a.CourseCode,
                                Website = a.Website,
                                AffiliationOrderNo = a.AffiliationOrderNo,
                                AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                Scheme = a.Scheme,
                                order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                course_name = j.course_type_name,
                                division = k.division_name,
                                scheme_name = l.trade_scheme,
                                UploadTradeAffiliationDoc=a.UploadTradeAffiliationDoc,
                                trade_id=tm.trade_id

                            }).FirstOrDefault();
               
                    var docs = GetAllAffiliationDoc()?.Where(a => a.Institute_id == data.iti_college_id && a.Trade_Id == data.trade_id && a.Unit == null && a.Shift == null).FirstOrDefault();
                    if (docs != null)
                    { data.FileUploadPath = docs.FileName; }
                if (data.FileUploadPath != "" && data.FileUploadPath != null)
                {
                    data.isSelect = System.IO.File.Exists(data.FileUploadPath);
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllAffiliatedInstituteByTalukDLL
        public List<SelectListItem> GetAllAffiliatedInstituteByTalukDLL(int taluk)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {

                list = (from a in _db.tbl_iti_college_details.Where(a => a.taluk_id == taluk)
                        select new SelectListItem
                        {
                            Text = a.iti_college_name,
                            Value = a.iti_college_id.ToString()

                        }).OrderBy(a => a.Text).ToList();

                return list;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllAffiliatedInstituteByDistrictDLL
        public List<SelectListItem> GetAllAffiliatedInstituteByDistrictDLL(int district)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {

                list = (from a in _db.tbl_iti_college_details.Where(a => a.district_id == district)
                        select new SelectListItem
                        {
                            Text = a.iti_college_name,
                            Value = a.iti_college_id.ToString()

                        }).OrderBy(a => a.Text).ToList();

                return list;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetA Affiliation Uploaded Trade Details 
        /// <summary>
        /// Here we are getting uploaded affiliation data from temprory tables to AffiliationCollegeDetails Model
        /// We have inner joined tables with name instead of id's bcoz we want ids of masters
        /// </summary>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetAAffiliationUploadedTradeDetailsDLL(int iti_trade_id)
        {
            try
            {
                AffiliationCollegeDetails data = (from b in _db.temp_Tbl_ITI_Trades.Where(a => a.Trade_ITI_id_temp == iti_trade_id)
                                                  join a in _db.temp_Tbl_Iti_College_Details on b.ITICode equals a.iti_college_id_temp
                                                  join c in _db.tbl_district_master on a.district equals c.district_ename into cc
                                                  from c in cc.DefaultIfEmpty()
                                                  join d in _db.tbl_taluk_master on a.taluk equals d.taluk_ename into dd
                                                  from d in dd.DefaultIfEmpty()
                                                  join g in _db.tbl_Constituencies on a.Constituency equals g.Constituencies into gg
                                                  from g in gg.DefaultIfEmpty()
                                                  join h in _db.tbl_location_type on a.location equals h.location_name into hh
                                                  from h in hh.DefaultIfEmpty()
                                                  join i in _db.tbl_Institute_type on a.Insitute_Type equals i.Institute_type into ii
                                                  from i in ii.DefaultIfEmpty()
                                                  join j in _db.tbl_division_master on a.division equals j.division_name into jj
                                                  from j in jj.DefaultIfEmpty()
                                                  join k in _db.tbl_trade_scheme on a.Scheme equals k.trade_scheme into kk
                                                  from k in kk.DefaultIfEmpty()
                                                  join l in _db.tbl_course_type_mast on a.CourseCode equals l.course_type_name into ll
                                                  from l in ll.DefaultIfEmpty()
                                                  join m in _db.tbl_trade_mast on b.TradeCode equals m.trade_name into mm
                                                  from m in mm.DefaultIfEmpty()
                                                  join n in _db.tbl_trade_sector on m.sector_id equals n.trade_sector_id into nn
                                                  from n in nn.DefaultIfEmpty()
                                                  join o in _db.tbl_trade_type_mast on m.trade_type_id equals o.trade_type_id into oo
                                                  from o in oo.DefaultIfEmpty()
                                                  select new AffiliationCollegeDetails
                                                  {
                                                      name_of_iti = a.iti_college_name,
                                                      type_of_iti_id = i.Institute_type_id,
                                                      location_type_id = h.location_id,
                                                      mis_code = a.MISCode,
                                                      dist_id = c.district_lgd_code,
                                                      taluk_id = d.taluk_lgd_code,
                                                      consti_id = g.ConstituencyId,
                                                      build_up_area = a.BuildUpArea,
                                                      address = a.college_address,
                                                      geo_location = a.geo,
                                                      email = a.email_id,
                                                      // affiliation_date = a.AffiliationDate,
                                                      phone_number = a.phone_num,
                                                      // no_units = a.Units,
                                                      // no_shifts = a.NoOfShifts,
                                                      zipcode = a.PinCode,
                                                      division_id = j.division_id,
                                                      iti_college_id = a.iti_college_id_temp,
                                                      date = a.AffiliationDate, //SqlFunctions.DatePart("day", a.AffiliationDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationDate),   // FileUploadPath = a.UploadAffiliationDoc,
                                                     // date =SqlFunctions.DatePart("day", a.AffiliationDate).ToString() + "/" + SqlFunctions.DatePart("m", a.AffiliationDate).ToString() + "/" + SqlFunctions.DatePart("year", a.AffiliationDate).ToString(),        // FileUploadPath = a.UploadAffiliationDoc,
                                                      status_id = a.StatusId,
                                                      Website = a.Website,
                                                      AffiliationOrderNo = a.AffiliationOrderNo,
                                                      //  AffiliationOrderNoDate = a.AffiliationOrderNoDate,
                                                      Scheme = k.ts_id,
                                                      order_no_date = a.AffiliationOrderNoDate, ///SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "-" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                                      //order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate).ToString() + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate).ToString() + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate).ToString(),
                                                      course_code = l.course_id,
                                                      trade_id = m.trade_id,
                                                      //units = int.TryParse(a, out a.uni),
                                                      //file_upload_path = a.FileUploadPath,
                                                      trade_iti_id = b.Trade_ITI_id_temp,
                                                      trade_code = m.trade_code,
                                                      sector = n.trade_sector,
                                                      trade_type = o.trade_type_name,
                                                      duration = m.trade_duration,
                                                      batch_size = m.trade_seating,
                                                      AidedUnaidedTrade = a.AidedUnaidedTrade,
                                                      flow_id = (int)CsystemType.getCommon.CaseWorker,
                                                      en_edit = true

                                                  }).FirstOrDefault();

                if (data.date != null)
                {
                    try
                    {
                        data.date = new string(data.date.TakeWhile(x => x != ' ').ToArray());
                       
                    }
                    catch { }

                }
                if (data.order_no_date != null)
                {
                    try
                    {
                        data.order_no_date = new string(data.order_no_date.TakeWhile(x => x != ' ').ToArray());
                       
                    }
                    catch { }

                }

                /// <summary>
                //taking back pincode from zipcode
                /// </summary>
                if (data.zipcode != null && data.zipcode != "")
                {
                    try
                    {
                        data.Pincode = Convert.ToInt32(data.zipcode);
                    }
                    catch { }

                }

                if (data != null)
                {

                    data.shifts = new List<TradeShift>();

                    data.shifts = (from a in _db.temp_Tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id_temp == data.trade_iti_id)
                                   select new TradeShift
                                   {
                                       CreatedBy = a.CreatedBy,
                                       ITI_Trade_Id = a.ITI_Trade_Id_temp,
                                       CreatedOn = a.CreatedOn,
                                       Dual_System = a.Dual_System,
                                       IsActive = a.IsActive,
                                       IsPPP = a.IsPPP,
                                       ITI_Trade_Shift_Id = a.ITI_Trade_Shift_Id_temp,
                                       st_shift = a.Shift,
                                       st_unit = a.Units

                                   }).ToList();

                    if (data.shifts.Count() > 0)
                    {
                        //taking back shift and unit from temp variables st_shift, st_unit
                        foreach (var value in data.shifts)
                        {
                            try
                            {
                                value.Shift = Convert.ToInt32(value.st_shift);

                            }
                            catch { }

                            try
                            {

                                value.Units = Convert.ToInt32(value.st_unit);
                            }
                            catch { }
                        }
                    }
                    try
                    {
                        data.no_units = data.shifts.Sum(a => Convert.ToInt32(a.st_unit));
                    }
                    catch { }
                }
                try
                {
                    data.no_units = data.trades_list.Sum(a => Convert.ToInt32(a.units));
                }
                catch { }


                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Save Uploaded Affiliation Trade Details
        /// <summary>
        /// Trade wise update affiliation 
        /// </summary>
        /// <param name="Affi"></param>
        /// <returns></returns>
        public AffiliationCollegeDetailsTest SaveUploadedAffiliationTradeDetails(AffiliationCollegeDetailsTest Affi)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {

                    var update = _db.tbl_iti_college_details.Where(a => a.MISCode == Affi.mis_code).FirstOrDefault();



                    if (update == null)
                    {
                        update = new tbl_iti_college_details();

                        update.iti_college_name = Affi.name_of_iti;
                        update.Insitute_TypeId = Affi.type_of_iti_id;
                        update.location_id = Affi.location_type_id;
                        update.MISCode = Affi.mis_code;
                        // update.css_code = Affi.css_code_id;
                        update.district_id = Affi.dist_id;
                        update.taluk_id = Affi.taluk_id;
                        update.Constituency = Affi.consti_id;
                        update.Panchayat = Affi.pancha_id;
                        update.village_or_town = Affi.village_id;
                        update.BuildUpArea = Affi.build_up_area;
                        update.college_address = Affi.address;
                        update.geo = Affi.geo_location;
                        update.email_id = Affi.email;
                        update.phone_num = Affi.phone_number;
                        update.AffiliationDate = Affi.affiliation_date;
                        update.NoofTrades = Affi.NoofTrades;
                        update.Units = Affi.no_units;
                        update.NoOfShifts = Affi.no_shifts;
                        update.updated_by = Affi.CreatedBy;
                        update.updation_datetime = DateTime.Now;
                        update.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                        update.PinCode = Affi.Pincode;
                        update.Website = Affi.Website;
                        update.AffiliationOrderNo = Affi.AffiliationOrderNo;
                        update.AffiliationOrderNoDate = Affi.AffiliationOrderNoDate;
                        update.Scheme = Affi.Scheme;
                        update.CourseCode = Affi.course_code;
                        update.division_id = Affi.division_id;
                        update.NewAffiliationOrderNo = Affi.NewAffiliationOrderNo;
                        update.NewAffiliationOrderNoDate = Affi.NewAffiliationOrderNoDate;
                        update.AidedUnaidedTrade = Affi.AidedUnaidedTrade;
                        update.iti_college_code = "0";
                        //if (Affi.FileUploadPath != null)
                        //{
                        //    update.UploadAffiliationDoc = Affi.FileUploadPath;
                        //}

                        _db.tbl_iti_college_details.Add(update);
                        _db.SaveChanges();

                    }
                    else
                    {
                        update.iti_college_name = Affi.name_of_iti;
                        update.Insitute_TypeId = Affi.type_of_iti_id;
                        update.location_id = Affi.location_type_id;
                        update.MISCode = Affi.mis_code;
                        // update.css_code = Affi.css_code_id;
                        update.district_id = Affi.dist_id;
                        update.taluk_id = Affi.taluk_id;
                        update.Constituency = Affi.consti_id;
                        update.Panchayat = Affi.pancha_id;
                        update.village_or_town = Affi.village_id;
                        update.BuildUpArea = Affi.build_up_area;
                        update.college_address = Affi.address;
                        update.geo = Affi.geo_location;
                        update.email_id = Affi.email;
                        update.phone_num = Affi.phone_number;
                        update.AffiliationDate = Affi.affiliation_date;
                        update.Units = Affi.no_units;
                        update.NoOfShifts = Affi.no_shifts;
                        update.updated_by = Affi.CreatedBy;
                        update.updation_datetime = DateTime.Now;
                        update.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                        update.PinCode = Affi.Pincode;
                        update.Website = Affi.Website;
                        update.AffiliationOrderNo = Affi.AffiliationOrderNo;
                        update.AffiliationOrderNoDate = Affi.AffiliationOrderNoDate;
                        update.Scheme = Affi.Scheme;
                        update.CourseCode = Affi.course_code;
                        update.division_id = Affi.division_id;
                        update.iti_college_code = "0";
                        //if (Affi.FileUploadPath != null)
                        //{
                        //    update.UploadAffiliationDoc = Affi.FileUploadPath;
                        //}

                        _db.SaveChanges();
                    }

                    var chkExistRec = _db.tbl_Affiliation_documents.Where(a => a.Institute_id == update.iti_college_id && a.Trade_Id == Affi.trade_id).FirstOrDefault();
                    tbl_Affiliation_documents DocUpload = new tbl_Affiliation_documents();
                    
                    if (chkExistRec == null)
                    {
                        
                        if (Affi.FileUploadPath != null)
                        {
                            DocUpload.FileName = Affi.FileUploadPath;
                            DocUpload.Institute_id = update.iti_college_id;
                            DocUpload.Trade_Id = Affi.trade_id;
                            DocUpload.AffiliationOrder_Number = update.AffiliationOrderNo;
                            DocUpload.Affiliation_date = Affi.AffiliationOrderNoDate;
                            _db.tbl_Affiliation_documents.Add(DocUpload);
                        }
                        
                    }
                    else
                    {
                        if (Affi.FileUploadPath != null)
                        {
                            chkExistRec.FileName = Affi.FileUploadPath;
                        }
                        
                    }
                    _db.SaveChanges();

                    tbl_ITI_Trade update_trade = new tbl_ITI_Trade();
                    update_trade.FlowId = Affi.flow_id;
                    update_trade.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    update_trade.CreatedOn = DateTime.Now;
                    update_trade.CreatedBy = Affi.CreatedBy;
                    update_trade.TradeCode = Affi.trade_id;
                    update_trade.ITICode = update.iti_college_id;
                    update_trade.Unit = Affi.no_units;
                    update_trade.IsActive = false;
                    update_trade.ActiveDeActive = true;
                    update_trade.AidedUnaidedTrade = Affi.AidedUnaidedTrade;

                    _db.tbl_ITI_Trade.Add(update_trade);
                    _db.SaveChanges();


                    var Add_His = new tbl_ITI_Trade_History();
                    Add_His.ITICode = Affi.iti_college_id;
                    Add_His.TradeCode = (int)Affi.trade_id;
                    Add_His.CreatedOn = DateTime.Now;
                    Add_His.CreatedBy = Affi.CreatedBy;
                    Add_His.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    Add_His.Unit = Affi.no_units;
                    Add_His.FlowId = Affi.flow_id;
                    Add_His.IsActive = false;
                    Add_His.FileUploadPath = Affi.FileUploadPath;
                    Add_His.Remarks = Affi.remarks;
                    Add_His.Trade_ITI_id = update_trade.Trade_ITI_id;

                    _db.tbl_ITI_Trade_Histories.Add(Add_His);
                    _db.SaveChanges();


                    if (Affi.shifts != null)
                    {
                        if (Affi.shifts != null)
                        {
                            foreach (var item in Affi.shifts)
                            {

                                tbl_ITI_Trade_Shift insertShift = new tbl_ITI_Trade_Shift();
                                insertShift.CreatedBy = Affi.CreatedBy;
                                insertShift.CreatedOn = DateTime.Now;
                                insertShift.Dual_System = item.Dual_System;
                                insertShift.IsActive = item.IsActive;
                                insertShift.IsPPP = item.IsPPP;
                                insertShift.ITI_Trade_Id = update_trade.Trade_ITI_id;
                                insertShift.Shift = item.Shift;
                                insertShift.Units = item.Units;
                                if (item.new_add == 1)
                                {
                                    insertShift.Status = (int)CsystemType.getCommon.Submitted;
                                }
                                else
                                {
                                    insertShift.Status = (int)CsystemType.getCommon.Published;
                                }


                                _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                _db.SaveChanges();

                            }
                        }

                    }

                    temp_tbl_ITI_Trade uploaded = _db.temp_Tbl_ITI_Trades.Where(a => a.Trade_ITI_id_temp == Affi.trade_iti_id).FirstOrDefault();
                    uploaded.IsUploaded = true;

                    _db.SaveChanges();


                    transaction.Complete();
                }

                return Affi;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Is Trade Exists in Affiliated Institute
        /// <summary>
        /// Is Trade Exists in Affiliated Institute
        /// </summary>
        /// <param name="tradecode"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        public bool IsAffiliatedTradeExists(int tradecode, int collegeId)
        {
            try
            {
                return _db.tbl_ITI_Trade.Any(a => a.TradeCode == tradecode && a.ITICode == collegeId && a.IsActive == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Is Mis Code Exists
        /// <summary>
        /// IsMisCodeExists
        /// </summary>
        /// <param name="miscode"></param>
        /// <returns></returns>
        public bool IsMisCodeExists(string miscode)
        {
            try
            {
                return _db.tbl_iti_college_details.Any(a => a.MISCode == miscode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Is ITI College Name Exists
        /// <summary>
        /// IsMisCodeExists
        /// </summary>
        /// <param name="iticollegename"></param>
        /// <returns></returns>
        public bool IsITICollegeNameExists(string iticollegename)
        {
            try
            {
                return _db.tbl_iti_college_details.Any(a => a.iti_college_name == iticollegename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region - Sathish

        #region Get A Unitwise Trade Details caseworker
        /// <summary>
        /// GetATradeDetailsDLL
        /// </summary>
        /// <param name="Trade_Id"></param>
        /// <returns></returns>
        public AffiliationCollegeDetails GetATradeUnitwiseDetailsDLL(int ITI_Trade_ShiftId, int flowid)
        {
            try
            {
                //ish
                AffiliationCollegeDetails data = (from b in _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == ITI_Trade_ShiftId)
                                                  join bb in _db.tbl_ITI_Trade_Shift_ActiveStatus on b.ITI_Trade_Shift_Id equals bb.ITI_Trade_Shift_Id into bbb
                                                  from bb in bbb.DefaultIfEmpty()
                                                  join m in _db.tbl_ITI_Trade on b.ITI_Trade_Id equals m.Trade_ITI_id
                                                  join a in _db.tbl_iti_college_details on m.ITICode equals a.iti_college_id into aa
                                                  from a in aa.DefaultIfEmpty()
                                                  join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                                                  from c in cc.DefaultIfEmpty()
                                                  join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                                                  from d in dd.DefaultIfEmpty()
                                                  join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                                                  from e in ee.DefaultIfEmpty()
                                                  join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                                                  from f in ff.DefaultIfEmpty()
                                                  join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                                                  from g in gg.DefaultIfEmpty()
                                                  join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                                                  from h in hh.DefaultIfEmpty()
                                                  join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                                                  from i in ii.DefaultIfEmpty()
                                                  join j in _db.tbl_trade_mast on m.TradeCode equals j.trade_id into jj
                                                  from j in jj.DefaultIfEmpty()
                                                  join k in _db.tbl_status_master on bb.ApprovalStatus equals k.StatusId into ts
                                                  from k in ts.DefaultIfEmpty()
                                                  join l in _db.tbl_CSS on a.css_code equals l.CSSCode into ll
                                                  from l in ll.DefaultIfEmpty()
                                                  join co in _db.tbl_course_type_mast on a.CourseCode equals co.course_id into cs
                                                  from co in cs.DefaultIfEmpty()
                                                  join sc in _db.tbl_trade_scheme on a.Scheme equals sc.ts_id into sch
                                                  from sc in sch.DefaultIfEmpty()
                                                  join div in _db.tbl_division_master on a.division_id equals div.division_id into ds
                                                  from div in ds.DefaultIfEmpty()
                                                  join o in _db.tbl_trade_sector on j.sector_id equals o.trade_sector_id into oo
                                                  from o in oo.DefaultIfEmpty()
                                                  join ty in _db.tbl_trade_type_mast on j.trade_type_id equals ty.trade_type_id into typ
                                                  from ty in typ.DefaultIfEmpty()
                                                      //join st in _db.tbl_status_master on bb.ApprovalStatus equals st.StatusId into us
                                                      //from st in us.DefaultIfEmpty()

                                                  select new AffiliationCollegeDetails
                                                  {
                                                      name_of_iti = a.iti_college_name,
                                                      mis_code = a.MISCode,
                                                      type_of_iti = i.Institute_type,
                                                      course_name = co.course_type_name,
                                                      trade = j.trade_name,
                                                      district = c.district_ename,
                                                      taluka = d.taluk_ename,
                                                      panchayat = e.grama_panchayat_name,
                                                      village_id = a.village_or_town,
                                                      consti_id = a.Constituency,
                                                      build_up_area = a.BuildUpArea,
                                                      css_code_id = a.css_code,
                                                      geo_location = a.geo,
                                                      address = a.college_address,
                                                      location_type = h.location_name,
                                                      email = a.email_id,
                                                      phone_number = a.phone_num,
                                                      affiliation_date = a.AffiliationDate,
                                                      scheme_name = sc.trade_scheme,
                                                      no_shifts = b.Shift,
                                                      iti_college_id = a.iti_college_id,
                                                      no_units = b.Units,
                                                      state = "Karnataka",
                                                      date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                                      order_no_date = SqlFunctions.DatePart("day", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationOrderNoDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationOrderNoDate),
                                                      AffiliationOrderNo = a.AffiliationOrderNo,
                                                      trade_id = j.trade_id,
                                                      Website = a.Website,
                                                      status = k.StatusName,
                                                      constituency = g.Constituencies,
                                                      village = f.village_ename,
                                                      division = div.division_name,
                                                      css_code = l.CSS_Scheme,
                                                      Pincode = a.PinCode,
                                                      Activefilepath = bb.FilePath,
                                                      //  ActiveDeActive = b.ActiveDeActive
                                                      ITI_Trade_ShiftId = b.ITI_Trade_Shift_Id,
                                                      trade_iti_id = m.Trade_ITI_id,
                                                      IsActive = b.IsActive,
                                                      IsActiveNew = bb.IsActive,
                                                      en_edit = bb.ApprovalFlowId == flowid ? true : false,
                                                      trade_code = j.trade_code,
                                                      sector = o.trade_sector,
                                                      duration = j.trade_duration,
                                                      batch_size = j.trade_seating,
                                                      trade_type = ty.trade_type_name,
                                                      flow_id = bb.ApprovalFlowId,
                                                      CreatedByChk = bb.CreatedBy,
                                                      ActivateFilePath = bb.ActivateFilePath,
                                                      ActivateOrderNo = bb.ActivateOrderNo,
                                                      DeactivateOrderNo = bb.DeactivateOrderNo,
                                                      FileUploadPath=a.UploadAffiliationDoc,
                                                      ActivateDate=bb.ActivateDate.ToString(),
                                                      DeactivateDate=bb.DeactivateDate.ToString(),
                                                      ActivateFileName=bb.ActivateFileName,
                                                      Filename=bb.FileName,

                                                      //TStatus=st.StatusName,




                                                  }).FirstOrDefault();
                if (data != null)
                {
                    //data.Filename= System.IO.Path.GetFileName(data.FileUploadPath);
                    var name = GetAllAffiliationDoc()?.Where(a => a.Shift == null && a.Institute_id == data.iti_college_id && a.Trade_Id==data.trade_id && a.Unit==null).Select(a=>a.FileName).FirstOrDefault();
                    data.Filename = System.IO.Path.GetFileName(name);
                    data.shifts = new List<TradeShift>();

                    data.shifts = GetAllShiftsDLL(ITI_Trade_ShiftId);
                    if(data.IsActiveNew==true)
                    {
                        data.flag = (int)CsystemType.getCommon.Activate;
                    }
                    else
                    {
                        data.flag = (int)CsystemType.getCommon.Deactivate;
                    }
                    data.AffiliationDocs = GetAllAffiliationDoc()?.Where(a => a.Shift == data.ITI_Trade_ShiftId &&a.Institute_id==data.iti_college_id).ToList();
                    if(data.AffiliationDocs.Count() !=0)
                    {
                        data.AffiliationDoc = GetAllAffiliationDoc()?.Where(a => a.Shift == data.ITI_Trade_ShiftId && a.Institute_id == data.iti_college_id).Last();
                    }
                 
                    if (data.AffiliationDocs != null)
                    {
                        foreach (var item in data.AffiliationDocs)
                        {
                            item.FileName = System.IO.Path.GetFileName(item.FileName);
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Trade Histroy unitwise
        /// <summary>
        /// GetTradeHistoriesDLL
        /// </summary>
        /// <param name="Trade_id"></param>
        /// <returns></returns>
        public List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesUintwiseDLL(int ITI_Trade_ShiftId)
        {
            try
            {
                List<ActiveDeactiveTradeHistory> list = (from a in _db.tbl_ITI_Trade_Shift_ActiveStatus_History.Where(a => a.ITI_Trade_Shift_Id == ITI_Trade_ShiftId)
                                                         join b in _db.tbl_ITI_Trade on a.ITI_Trade_Id equals b.Trade_ITI_id
                                                         join c in _db.tbl_role_master on a.CreatedBy equals c.role_id into cw
                                                         from c in cw.DefaultIfEmpty()
                                                         join f in _db.tbl_role_master on a.ApprovalFlowId equals f.role_id into fw
                                                         from f in fw.DefaultIfEmpty()
                                                         join s in _db.tbl_status_master on a.ApprovalStatus equals s.StatusId


                                                         select new ActiveDeactiveTradeHistory
                                                         {
                                                             //CreatedOn = a.CreatedOn,
                                                             // CreatedBy = a.CreatedBy,
                                                             FilePath = a.FilePath,
                                                             ActDeActRemarks = a.ActDeActRemarks,
                                                             IsActive = a.IsActive,
                                                             //ITICode = a.ITICode,
                                                             //Remarks = a.Remarks,
                                                             //StatusId = a.StatusId,
                                                             //TradeCode = a.TradeCode,
                                                             //Trade_ITI_His_id = a.Trade_ITI_His_id,
                                                             //Trade_ITI_id = a.Trade_ITI_id,
                                                             //Unit = a.Unit,
                                                             //TradeName = b.trade_name,
                                                             //Flow_user = r.role_description,
                                                             date = a.CreatedOn.ToString(),
                                                             //CreatedUser = c.id.ToString()
                                                             User = c.role_description,
                                                             Status = s.StatusName,
                                                             ActiveFilepath = a.FilePath,
                                                             Flow_user = f.role_description ?? "-",




                                                         }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Active Deactive unit deatils

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsDLL()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             //where tradeshift.IsActive == true || tradeshift.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 //status_id = clz.StatusId,
                                                                 Shifts = tradeshift.Shift,
                                                                 //StatusName = Statusmaster.StatusName + " - " + rl.role_DescShortForm,
                                                                 StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                                                                 // StatusName = ust.ApprovalStatus,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 college_Address=clz.college_address



                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get Active Deactive unit deatils for OS

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsDLL()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             where tradeshift.IsActive == true || tradeshift.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 //status_id = clz.StatusId,
                                                                 Shifts = tradeshift.Shift,
                                                                // StatusName = (Statusmaster.StatusId != 2 && Statusmaster.StatusId != 19 ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusName),
                                                                 StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                                                                 // StatusName = ust.ApprovalStatus,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 ReqIsActive = tradeshift1.IsActive



                                                             }).ToList();
                items = items.GroupBy(a => a.ITI_Trade_Shift_Id).Select(z => new ActiveandDeactiveUnitsDeatils
                {
                    ITI_Trade_Shift_Id = z.Key,
                    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                    trades = z.Select(c => c.trades).FirstOrDefault(),
                    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                    units = z.Select(c => c.units).FirstOrDefault(),
                    miscode = z.Select(c => c.miscode).FirstOrDefault(),
                    Shifts = z.Select(c => c.Shifts).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    CourseType = z.Select(c => c.CourseType).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    IsActive1 = z.Select(c => c.IsActive1).FirstOrDefault(),
                    ReqIsActive = z.Select(c => c.ReqIsActive).FirstOrDefault(),
                    CreatedonOrderby = z.Select(c => c.CreatedonOrderby).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get Active Deactive unit deatils for ADDD

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieADDDDeatilsDLL()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             where tradeshift.IsActive == true || tradeshift.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 //status_id = clz.StatusId,
                                                                 Shifts = tradeshift.Shift,
                                                                 //StatusName = (Statusmaster.StatusId != 2 && Statusmaster.StatusId != 19 ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusName),
                                                                 StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                                                                 // StatusName = ust.ApprovalStatus,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 ReqIsActive = tradeshift1.IsActive,




                                                             }).ToList();
                items = items.GroupBy(a => a.ITI_Trade_Shift_Id).Select(z => new ActiveandDeactiveUnitsDeatils
                {
                    ITI_Trade_Shift_Id = z.Key,
                    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                    trades = z.Select(c => c.trades).FirstOrDefault(),
                    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                    units = z.Select(c => c.units).FirstOrDefault(),
                    miscode = z.Select(c => c.miscode).FirstOrDefault(),
                    Shifts = z.Select(c => c.Shifts).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    CourseType = z.Select(c => c.CourseType).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    IsActive1 = z.Select(c => c.IsActive1).FirstOrDefault(),
                    ReqIsActive = z.Select(c => c.ReqIsActive).FirstOrDefault(),
                    CreatedonOrderby = z.Select(c => c.CreatedonOrderby).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();




                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get Active Deactive unit deatils for view and reject

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewDeatilsDLL()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from trade in _db.tbl_ITI_Trade
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             join tradeshift in _db.tbl_ITI_Trade_Shift_ActiveStatus on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id
                                                             join tradesh in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradesh.ITI_Trade_Id
                                                          into ts
                                                             from tradesh in ts.DefaultIfEmpty()
                                                                 //join trad_his in _db.tbl_ITI_Trade_Shift_ActiveStatus_History on tradesh.ITI_Trade_Shift_Id equals trad_his.ITI_Trade_Shift_Id

                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()

                                                             join Statusmaster in _db.tbl_status_master on tradeshift.ApprovalStatus equals Statusmaster.StatusId
                                                             where tradeshift.ApprovalStatus == 2 || tradeshift.ApprovalStatus == 19
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 createdon = tradeshift.CreatedOn,
                                                                 Shifts = tradeshift.Shift,
                                                                 StatusName = (Statusmaster.StatusId == (int)CsystemType.getCommon.Approved?"Approved":""),
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 miscode = clz.MISCode,
                                                                 IsActive = trade.IsActive,
                                                                 IsActiveTr = tradesh.IsActive,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 college_Address = clz.college_address,
                                                                 date = tradeshift.CreatedOn.ToString(),



                                                             }).ToList();

                items = items.GroupBy(a => a.ITI_Trade_Shift_Id).Select(z => new ActiveandDeactiveUnitsDeatils
                {
                    ITI_Trade_Shift_Id = z.Key,
                    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                    trades = z.Select(c => c.trades).FirstOrDefault(),
                    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                    units = z.Select(c => c.units).FirstOrDefault(),
                    miscode = z.Select(c => c.miscode).FirstOrDefault(),
                    Shifts = z.Select(c => c.Shifts).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    CourseType = z.Select(c => c.CourseType).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    IsActive1 = z.Select(c => c.IsActive1).FirstOrDefault(),
                    IsActiveTr = z.Select(c => c.IsActiveTr).FirstOrDefault(),
                    college_Address = z.Select(c => c.college_Address).FirstOrDefault(),
                    date = z.Select(c => c.date).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        public List<AffiliationCollegeDetails> GetAllActiveandDeactiveUnitsDeatilsAffDLL(int college_id)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => /*a.is_active == true &&*/ a.iti_college_id == college_id)

                            join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                            from b in bb.DefaultIfEmpty()
                            join tradeshift in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals tradeshift.ITI_Trade_Id into ts
                            from tradeshift in ts.DefaultIfEmpty()


                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                            from j in jj.DefaultIfEmpty()
                            join iast in _db.tbl_ITI_Trade_Shift_ActiveStatus on b.Trade_ITI_id equals iast.ITI_Trade_Id into cd
                            from iast in cd.DefaultIfEmpty()
                            join k in _db.tbl_status_master on iast.ApprovalStatus equals k.StatusId
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join cour in _db.tbl_course_type_mast on a.CourseCode equals cour.course_id
                                                            into crs
                            from cour in crs.DefaultIfEmpty()


                                //where b.IsActive == true //&&
                                //b.FlowId == role_id && 
                                //where iast.ApprovalStatus != (int)CsystemType.getCommon.Approved
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                // type_of_iti = i.Institute_type,
                                trade = j.trade_name,
                                district = c.district_ename,
                                //taluka = d.taluk_ename,
                                //panchayat = e.grama_panchayat_name,
                                //village_id = a.village_or_town,
                                //consti_id = a.Constituency,
                                //build_up_area = a.BuildUpArea,
                                //css_code_id = a.css_code,
                                //geo_location = a.geo,
                                address = a.college_address,
                                //location_type = h.location_name,
                                // email = a.email_id,
                                // phone_number = a.phone_num,
                                // affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                //NoofTrades = a.NoofTrades,
                                no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                no_units = b.Unit,
                                //state = "Karnataka",
                                // date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                trade_id = j.trade_id,
                                FileUploadPath = b.FileUploadPath,
                                status = k.StatusName,
                                //constituency = g.Constituencies,
                                //village = f.village_ename,
                                trade_iti_id = b.Trade_ITI_id,
                                IsActive = tradeshift.IsActive,
                                IsActiveNew = b.IsActive,
                                division = l.division_name,
                                course_name = cour.course_type_name,
                                Trade_ITI_id = b.Trade_ITI_id,
                                ITI_Trade_ShiftId = tradeshift.ITI_Trade_Shift_Id,
                                Pubdate = iast.CreatedOn.ToString(),


                                // course_name=

                            }).ToList();

                list = list.GroupBy(a => a.ITI_Trade_ShiftId).Select(z => new AffiliationCollegeDetails
                {
                    ITI_Trade_ShiftId = z.Key,
                    name_of_iti = z.Select(b => b.name_of_iti).FirstOrDefault(),
                    trade = z.Select(c => c.trade).FirstOrDefault(),
                    Pubdate = z.Select(c => c.Pubdate).FirstOrDefault(),
                    no_units = z.Select(c => c.no_units).FirstOrDefault(),
                    mis_code = z.Select(c => c.mis_code).FirstOrDefault(),
                    no_shifts = z.Select(c => c.no_shifts).FirstOrDefault(),
                    status = z.Select(c => c.status).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    course_name = z.Select(c => c.course_name).FirstOrDefault(),
                    division = z.Select(c => c.division).FirstOrDefault(),
                    district = z.Select(c => c.district).FirstOrDefault(),
                    address = z.Select(c => c.address).FirstOrDefault(),
                    IsActiveNew = z.Select(c => c.IsActiveNew).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public bool CreateActiveDeactiveUnitWiseHistoryDLL(tbl_ITI_Trade_Shift_ActiveStatus_History model)
        {
            try
            {
                _db.tbl_ITI_Trade_Shift_ActiveStatus_History.Add(model);

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

        #region update and insert TradeActiveDeactivedetails Unitwise
        /// <summary>
        /// UpdateTradeActiveDeactivedetails
        /// </summary>
        /// <param name="TradeUintId"></param>
        /// <returns></returns>
        /// 
        public TradeActiveDeactiveUpdateandinsertUnitwise TradeActiveDeactiveUpdateandinsertUnitwsieDLL(TradeActiveDeactiveUpdateandinsertUnitwise TradeUintId)
        {
            try
            {
                var update = _db.tbl_ITI_Trade_Shift_ActiveStatus.Where(a => a.ITI_Trade_Shift_Id == TradeUintId.ITI_Trade_Shift_Id).FirstOrDefault();
                if (update != null)
                {
                    if (TradeUintId.FilePath != "null" && TradeUintId.FilePath != null)
                    {
                        update.FilePath = TradeUintId.FilePath;
                        update.FileName = TradeUintId.FileName;
                    }
                    if (TradeUintId.ActivateFilePath != "null" && TradeUintId.ActivateFilePath != null)
                    {
                        update.ActivateFilePath = TradeUintId.ActivateFilePath;
                        update.ActivateFileName = TradeUintId.ActivateFileName;

                        //update.ActivateOrderNo = TradeUintId.ActivateOrderNo;
                    }
                    if (TradeUintId.DeactivateOrderNo != "null" && TradeUintId.DeactivateOrderNo != null)
                    {
                        update.DeactivateOrderNo = TradeUintId.DeactivateOrderNo;
                    }
                    if (TradeUintId.ActivateOrderNo != "null" && TradeUintId.ActivateOrderNo != null)
                    {
                        update.ActivateOrderNo = TradeUintId.ActivateOrderNo;
                    }
                    update.ITI_Trade_Shift_Id = TradeUintId.ITI_Trade_Shift_Id;
                    update.ITI_Trade_Id = TradeUintId.ITI_Trade_Id;
                    update.IsActive = TradeUintId.IsActive;
                    update.CreatedOn = TradeUintId.CreatedOn;
                    update.CreatedBy = TradeUintId.CreatedBy;
                    update.Units = TradeUintId.Units;
                    update.Shift = TradeUintId.Shift;
                    update.ApprovalStatus = TradeUintId.ApprovalStatus;
                    update.ApprovalFlowId = TradeUintId.ApprovalFlowId;
                    update.ActDeActRemarks = TradeUintId.ActDeActRemarks;
                    if(TradeUintId.DeactivateDate != null)
                    {
                        update.DeactivateDate = TradeUintId.DeactivateDate;
                    }
                    if(TradeUintId.ActivateDate != null)
                    {
                        update.ActivateDate = TradeUintId.ActivateDate;
                    }
                    //if (update.FilePath != "null" && update.FilePath != null)
                    //{
                    //    update.FilePath = TradeUintId.FilePath;
                    //}
                    _db.SaveChanges();

                    ////test
                    if (TradeUintId.ApprovalStatus == 2)
                    {
                        if (TradeUintId.IsActive == false)
                        {
                            var update1 = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == TradeUintId.ITI_Trade_Shift_Id).FirstOrDefault();
                            update1.IsActive = false;
                            _db.SaveChanges();

                            var test = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == TradeUintId.ITI_Trade_Id && a.IsActive == true).FirstOrDefault();
                            if (test == null)
                            {
                                var update2 = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == TradeUintId.ITI_Trade_Id).FirstOrDefault();
                                update2.IsActive = false;
                                update2.color_flag = 0;
                                _db.SaveChanges();

                            }
                        }
                        else
                        {
                            var update1 = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Shift_Id == TradeUintId.ITI_Trade_Shift_Id).FirstOrDefault();
                            update1.IsActive = true;
                            _db.SaveChanges();

                            //var test = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == TradeUintId.ITI_Trade_Id && a.IsActive == true).FirstOrDefault();
                            //if (test == null)
                            //{
                            var update2 = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == TradeUintId.ITI_Trade_Id).FirstOrDefault();
                            update2.IsActive = true;
                            update2.color_flag = null;
                            _db.SaveChanges();

                            //}
                        }

                    }

                    ////test


                }

                else
                {
                    var insert = new tbl_ITI_Trade_Shift_ActiveStatus();
                    //if (insert.FilePath != "null" && insert.FilePath != null)
                    //{
                    insert.FilePath = TradeUintId.FilePath;
                    insert.FileName = TradeUintId.FileName;
                    //}
                    insert.ITI_Trade_Shift_Id = TradeUintId.ITI_Trade_Shift_Id;
                    insert.ITI_Trade_Id = TradeUintId.ITI_Trade_Id;
                    insert.IsActive = TradeUintId.IsActive;
                    insert.CreatedOn = TradeUintId.CreatedOn;
                    insert.CreatedBy = TradeUintId.CreatedBy;
                    insert.Units = TradeUintId.Units;
                    insert.Shift = TradeUintId.Shift;
                    insert.ApprovalStatus = TradeUintId.ApprovalStatus;
                    insert.ApprovalFlowId = TradeUintId.ApprovalFlowId;
                    insert.ActDeActRemarks = TradeUintId.ActDeActRemarks;
                    if(TradeUintId.DeactivateOrderNo!=null)
                    {
                        insert.DeactivateOrderNo = TradeUintId.DeactivateOrderNo;
                    }
                    if (TradeUintId.DeactivateDate != null)
                    {
                        insert.DeactivateDate = TradeUintId.DeactivateDate;
                    }
                    insert.ITI_Trade_Shift_Active_Id = TradeUintId.ITI_Trade_Shift_Active_Id;

                    _db.tbl_ITI_Trade_Shift_ActiveStatus.Add(insert);
                    _db.SaveChanges();


                }

                if ((TradeUintId.DeactivateDate != null || TradeUintId.DeactivateOrderNo != null|| TradeUintId.FilePath != null) && TradeUintId.IsActive==false)
                {
                    var updatedoc = _db.tbl_Affiliation_documents.Where(a => a.Shift == TradeUintId.ITI_Trade_Shift_Id && a.Flag == (int)CsystemType.getCommon.Deactivate).OrderByDescending(a => a.Id).Take(1).FirstOrDefault();
                    if(updatedoc != null)
                    {
                        //Take second last row from history table to check the existing document and add new one of approved
                        int updatedoc1 = _db.tbl_ITI_Trade_Shift_ActiveStatus_History.Where(a => a.ITI_Trade_Shift_Id == updatedoc.Shift && a.ITI_Trade_Id == updatedoc.Trade_Id).OrderByDescending(a=> a.CreatedOn).Skip(1).Select(a => a.ApprovalStatus).Take(1).FirstOrDefault();
                        if ((updatedoc.Shift == TradeUintId.ITI_Trade_Shift_Id && updatedoc.Institute_id == TradeUintId.activeITIid && updatedoc.Flag == (int)CsystemType.getCommon.Deactivate) && updatedoc1 != (int)CsystemType.getCommon.Approved)
                        {

                            updatedoc.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                            updatedoc.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                            updatedoc.Institute_id = TradeUintId.activeITIid;
                            if(TradeUintId.FilePath!=null)
                            {
                                updatedoc.FileName = TradeUintId.FilePath;
                            }
                            
                            updatedoc.IsActive = true;
                            updatedoc.Status = "Deactivate";
                            updatedoc.Flag = (int)CsystemType.getCommon.Deactivate;
                             if(updatedoc.AffiliationOrder_Number!=TradeUintId.DeactivateOrderNo )
                            {
                                updatedoc.AffiliationOrder_Number = TradeUintId.DeactivateOrderNo;
                            }

                            if (updatedoc.Affiliation_date != TradeUintId.DeactivateDate)
                            {
                                updatedoc.Affiliation_date = TradeUintId.DeactivateDate;
                            }
                            _db.SaveChanges();
                        }
                        else
                        {
                            var Trade_Doc = new tbl_Affiliation_documents();
                            Trade_Doc.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                            Trade_Doc.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                            Trade_Doc.Institute_id = TradeUintId.activeITIid;
                            Trade_Doc.FileName = TradeUintId.FilePath;
                            Trade_Doc.IsActive = true;
                            Trade_Doc.Status = "Deactivate";
                            Trade_Doc.Flag = (int)CsystemType.getCommon.Deactivate;
                            Trade_Doc.AffiliationOrder_Number = TradeUintId.DeactivateOrderNo;
                            Trade_Doc.Affiliation_date = TradeUintId.DeactivateDate;
                            _db.tbl_Affiliation_documents.Add(Trade_Doc);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        var Trade_Doc = new tbl_Affiliation_documents();
                        Trade_Doc.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                        Trade_Doc.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                        Trade_Doc.Institute_id = TradeUintId.activeITIid;
                        Trade_Doc.FileName = TradeUintId.FilePath;
                        Trade_Doc.IsActive = true;
                        Trade_Doc.Status = "Deactivate";
                        Trade_Doc.Flag = (int)CsystemType.getCommon.Deactivate;
                        Trade_Doc.AffiliationOrder_Number = TradeUintId.DeactivateOrderNo;
                        Trade_Doc.Affiliation_date = TradeUintId.DeactivateDate;
                        _db.tbl_Affiliation_documents.Add(Trade_Doc);
                        _db.SaveChanges();
                    }
                   
                }
                else if((TradeUintId.ActivateDate != null || TradeUintId.ActivateOrderNo != null || TradeUintId.ActivateFilePath != null)&& TradeUintId.IsActive == true)
                {
                    var updateact = _db.tbl_Affiliation_documents.Where(a => a.Shift == TradeUintId.ITI_Trade_Shift_Id && a.Flag== (int)CsystemType.getCommon.Activate).OrderByDescending(a=> a.Id).Take(1).FirstOrDefault(); 

                    if (updateact != null)
                    {
                        //Take second last row from history table to check the existing document and add new one of approved
                        int updatedoc1 = _db.tbl_ITI_Trade_Shift_ActiveStatus_History.Where(a => a.ITI_Trade_Shift_Id == updateact.Shift && a.ITI_Trade_Id == updateact.Trade_Id).OrderByDescending(a => a.CreatedOn).Skip(1).Select(a => a.ApprovalStatus).Take(1).FirstOrDefault();
                        if ((updateact.Shift == TradeUintId.ITI_Trade_Shift_Id && updateact.Institute_id==TradeUintId.activeITIid && updateact.Flag == (int)CsystemType.getCommon.Activate) && updatedoc1 != (int)CsystemType.getCommon.Approved)
                        {

                            updateact.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                            updateact.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                            updateact.Institute_id = TradeUintId.activeITIid;
                            if (TradeUintId.ActivateFilePath!=null)
                            {
                                updateact.FileName = TradeUintId.ActivateFilePath;
                            }
                            updateact.IsActive = true;
                            updateact.Status = "Activate";
                            updateact.Flag = (int)CsystemType.getCommon.Activate;
                            if (updateact.AffiliationOrder_Number != TradeUintId.ActivateOrderNo)
                            {
                                updateact.AffiliationOrder_Number = TradeUintId.ActivateOrderNo;
                            }
                            if (updateact.Affiliation_date != TradeUintId.ActivateDate)
                            {
                                updateact.Affiliation_date = TradeUintId.ActivateDate;
                            }
                            
                            _db.SaveChanges();
                        }
                        else
                        {
                            var Trade_Doc = new tbl_Affiliation_documents();
                            Trade_Doc.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                            Trade_Doc.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                            Trade_Doc.Institute_id = TradeUintId.activeITIid;
                            Trade_Doc.FileName = TradeUintId.ActivateFilePath;
                            Trade_Doc.IsActive = true;
                            Trade_Doc.Status = "Activate";
                            Trade_Doc.Flag = (int)CsystemType.getCommon.Activate;
                            Trade_Doc.AffiliationOrder_Number = TradeUintId.ActivateOrderNo;
                            Trade_Doc.Affiliation_date = TradeUintId.ActivateDate;
                            _db.tbl_Affiliation_documents.Add(Trade_Doc);
                            _db.SaveChanges();
                        }

                    }
                    else
                    {
                        var Trade_Doc = new tbl_Affiliation_documents();
                        Trade_Doc.Trade_Id = Convert.ToInt32(TradeUintId.ITI_Trade_Id);
                        Trade_Doc.Shift = Convert.ToInt32(TradeUintId.ITI_Trade_Shift_Id);
                        Trade_Doc.Institute_id = TradeUintId.activeITIid;
                        Trade_Doc.FileName = TradeUintId.ActivateFilePath;
                        Trade_Doc.IsActive = true;
                        Trade_Doc.Status = "Activate";
                        Trade_Doc.Flag = (int)CsystemType.getCommon.Activate;
                        Trade_Doc.AffiliationOrder_Number = TradeUintId.ActivateOrderNo;
                        Trade_Doc.Affiliation_date = TradeUintId.ActivateDate;
                        _db.tbl_Affiliation_documents.Add(Trade_Doc);
                        _db.SaveChanges();
                    }
                   
                }

                return TradeUintId;
            }

            catch (Exception ex)

            {
                throw ex;
            }
        }


        #endregion


        #region update and insert TradeActiveDeactiveUpdateandinsert InstwsieDLL 
        /// <summary>
        /// UpdateTradeActiveDeactivedetails
        /// </summary>
        /// <param name="TradeUintId"></param>
        /// <returns></returns>
        /// 
        public TradeActiveandDeactiveStatus TradeActiveDeactiveUpdateandinsertInstwsieDLL(TradeActiveandDeactiveStatus TradeInsttId)
        {
            try
            {
                var update = _db.tbl_iti_college_details.Where(a => a.iti_college_id == TradeInsttId.clgId).FirstOrDefault();
                if (update != null)
                {


                    ////test
                    if (TradeInsttId.StatusId == 2)
                    {
                        if (TradeInsttId.IsActive == false)
                        {
                            var update1 = _db.tbl_iti_college_details.Where(a => a.iti_college_id == TradeInsttId.clgId).FirstOrDefault();
                            update1.is_active = false;
                            _db.SaveChanges();


                        }
                        else
                        {
                            var update1 = _db.tbl_iti_college_details.Where(a => a.iti_college_id == TradeInsttId.clgId).FirstOrDefault();
                            update1.is_active = true;
                            _db.SaveChanges();

                            //var test = _db.tbl_ITI_Trade_Shifts.Where(a => a.ITI_Trade_Id == TradeUintId.ITI_Trade_Id && a.IsActive == true).FirstOrDefault();
                            //if (test == null)
                            //{


                            //}
                        }

                    }

                    ////test


                }



                return TradeInsttId;
            }

            catch (Exception ex)

            {
                throw ex;
            }
        }


        #endregion

        //Institute Deaffiliate

        #region Get All Institute Details for De-Affiliate

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteDLL()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details

                                                    join iast in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals iast.ITI_Institute_Id into bb
                                                    from iast in bb.DefaultIfEmpty()
                                                    join stsm in _db.tbl_status_master on iast.ApprovalStatus equals stsm.StatusId into aa
                                                    from stsm in aa.DefaultIfEmpty()
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on iast.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where clg.is_active == true

                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        //StatusName = stsm.StatusName + " - " + rl.role_DescShortForm,
                                                        //StatusName = (stsm.StatusId != 2 && stsm.StatusId != 19 ? stsm.StatusName + " - " + rl.role_DescShortForm : stsm.StatusName),
                                                        StatusName = (stsm.StatusId != (int)CsystemType.getCommon.Approved && stsm.StatusId != (int)CsystemType.getCommon.pub ? stsm.StatusName + " - " + rl.role_DescShortForm : stsm.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : stsm.StatusName),
                                                        //ActiveDeActive = trade.ActiveDeActive,
                                                        // Trade_ITI_id = trade.Trade_ITI_id,
                                                        iti_college_id = clg.iti_college_id,
                                                        IsActive = clg.is_active,
                                                        IsActive1 = iast.IsActive,
                                                        MisCode = clg.MISCode,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                        date = iast.CreatedOn.ToString(),
                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSDLL()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join Statusmaster in _db.tbl_status_master on stsm.ApprovalStatus equals Statusmaster.StatusId
                                                    into bb
                                                    from Statusmaster in bb.DefaultIfEmpty()
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on stsm.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where stsm.ApprovalFlowId== (int)Models.Affiliation.CsystemType.getCommon.OFFICE_SUPERINTENDENT
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        //StatusName = Statusmaster.StatusName + " - " + rl.role_DescShortForm ?? null,
                                                        //StatusName = (Statusmaster.StatusId != 2 && Statusmaster.StatusId != 19 ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusName) ?? null,
                                                        StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                                                        MisCode = clg.MISCode,
                                                        IsActive = clg.is_active,
                                                        iti_college_id = clg.iti_college_id,
                                                        createdon = stsm.CreatedOn,
                                                        IsActive1 = stsm.IsActive,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,

                                                    }).ToList();
                //items = items.GroupBy(a => a.iti_college_id).Select(z => new DeAffiliateInstitute
                //{
                //    iti_college_id = z.Key,
                //    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                //    trades = z.Select(c => c.trades).FirstOrDefault(),
                //    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                //    units = z.Select(c => c.units).FirstOrDefault(),
                //    MisCode = z.Select(c => c.MisCode).FirstOrDefault(),

                //    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                //    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),



                //}).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDDDLL()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join stsm1 in _db.tbl_status_master on stsm.ApprovalStatus equals stsm1.StatusId
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                              into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on stsm.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where stsm.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Assistant_Director || stsm.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Deputy_Director
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        //StatusName = stsm1.StatusName + " - " + rl.role_DescShortForm,
                                                        //StatusName = (stsm1.StatusId != 2 && stsm1.StatusId != 19 ? stsm1.StatusName + " - " + rl.role_DescShortForm : stsm1.StatusName),
                                                        StatusName = (stsm1.StatusId != (int)CsystemType.getCommon.Approved && stsm1.StatusId != (int)CsystemType.getCommon.pub ? stsm1.StatusName + " - " + rl.role_DescShortForm : stsm1.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : stsm1.StatusName),
                                                        MisCode = clg.MISCode,
                                                        IsActive = clg.is_active,
                                                        iti_college_id = clg.iti_college_id,
                                                        createdon = stsm.CreatedOn,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                        IsActive1 = stsm.IsActive,

                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteApprovedRejectedDLL()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join stsm1 in _db.tbl_status_master on stsm.ApprovalStatus equals stsm1.StatusId
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                           into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                        // join userm in tbl_user_roles on userm.
                                                    where stsm.ApprovalStatus == 2 || stsm.ApprovalStatus == 19
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = (stsm1.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : ""),
                                                        MisCode = clg.MISCode,
                                                        //Trade_ITI_id = trade.Trade_ITI_id,
                                                        iti_college_id = clg.iti_college_id,
                                                        IsActive = stsm.IsActive,
                                                        createdon = stsm.CreatedOn,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                        CourseType = cour.course_type_name,
                                                        Clg_Adderss = clg.college_address,
                                                        date = stsm.CreatedOn.ToString(),


                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public List<AffiliationCollegeDetails> GetAllDeaffiliateInstituteAffDLL(int College_id)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => /*a.is_active == true &&*/ a.iti_college_id == College_id)

                                //join b in _db.tbl_ITI_Trade on a.iti_college_id equals b.ITICode into bb
                                //from b in bb.DefaultIfEmpty()
                                //join ts in _db.tbl_ITI_Trade_Shifts on b.Trade_ITI_id equals ts.ITI_Trade_Id into bs
                                //from ts in bs.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                                //join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                                //from d in dd.DefaultIfEmpty()
                                //join e in _db.tbl_Grama_Panchayat_Masters on a.Panchayat equals e.gp_lgd_code into ee
                                //from e in ee.DefaultIfEmpty()
                                //join f in _db.tbl_Village_Masters on a.village_or_town equals f.village_lgd_code into ff
                                //from f in ff.DefaultIfEmpty()
                                //join g in _db.tbl_Constituencies on a.Constituency equals g.ConstituencyId into gg
                                //from g in gg.DefaultIfEmpty()
                                //join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                                //from h in hh.DefaultIfEmpty()
                                //join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                                //from i in ii.DefaultIfEmpty()
                                //join j in _db.tbl_trade_mast on b.TradeCode equals j.trade_id into jj
                                //from j in jj.DefaultIfEmpty()
                            join stsm in _db.tbl_ITI_Institute_ActiveStatus on a.iti_college_id equals stsm.ITI_Institute_Id
                            join k in _db.tbl_status_master on stsm.ApprovalStatus equals k.StatusId
                            join cour in _db.tbl_course_type_mast on a.CourseCode equals cour.course_id
                                                             into crs
                            from cour in crs.DefaultIfEmpty()
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                            join dt in _db.tbl_district_master on a.district_id equals dt.district_lgd_code into dc
                            from dt in dc.DefaultIfEmpty()
                            join rl in _db.tbl_role_master on stsm.ApprovalFlowId equals rl.role_id into rm
                            from rl in rm.DefaultIfEmpty()
                                //where b.IsActive == true &&
                                //b.FlowId == role_id && 
                                // where a.StatusId == (int)CsystemType.getCommon.Approved
                            select new AffiliationCollegeDetails
                            {
                                name_of_iti = a.iti_college_name,
                                mis_code = a.MISCode,
                                //type_of_iti = i.Institute_type,
                                //trade = j.trade_name,
                                district = c.district_ename,
                                //taluka = d.taluk_ename,
                                //panchayat = e.grama_panchayat_name,
                                //village_id = a.village_or_town,
                                //consti_id = a.Constituency,
                                //build_up_area = a.BuildUpArea,
                                //css_code_id = a.css_code,
                                //geo_location = a.geo,
                                address = a.college_address,
                                // location_type = h.location_name,
                                //email = a.email_id,
                                //phone_number = a.phone_num,
                                //affiliation_date = a.AffiliationDate,
                                //no_trades = column missing
                                NoofTrades = a.NoofTrades,
                                //no_shifts = a.NoOfShifts,
                                iti_college_id = a.iti_college_id,
                                //no_units = b.Unit,
                                //state = "Karnataka",
                                date = SqlFunctions.DatePart("day", a.AffiliationDate) + "/" + SqlFunctions.DatePart("m", a.AffiliationDate) + "/" + SqlFunctions.DatePart("year", a.AffiliationDate),
                                //trade_id = j.trade_id,
                                //FileUploadPath = b.FileUploadPath,
                                status = (k.StatusId != 2 && k.StatusId != 19 ? k.StatusName + " - " + rl.role_DescShortForm : k.StatusName),
                                //constituency = g.Constituencies,
                                //village = f.village_ename,
                                //trade_iti_id = b.Trade_ITI_id,
                                IsActive = a.is_active,
                                course_name = cour.course_type_name,
                                // ITI_Trade_ShiftId=ts.ITI_Trade_Shift_Id,
                                Pubdate = stsm.CreatedOn.ToString(),
                                division = l.division_name,



                            }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public List<ActiveDeactiveTradeHistory> ActiveDeactiveGetTradeHistoriesInstDLL(int clg_id)
        {
            try
            {
                List<ActiveDeactiveTradeHistory> list = (from a in _db.tbl_ITI_Institute_His_ActiveStatus.Where(a => a.ITI_Institute_Id == clg_id)
                                                         join b in _db.tbl_ITI_Institute_ActiveStatus on a.ITI_Institute_Id equals b.ITI_Institute_Id
                                                         join c in _db.tbl_role_master on a.CreatedBy equals c.role_id into bb
                                                         from c in bb.DefaultIfEmpty()
                                                         join f in _db.tbl_role_master on a.ApprovalFlowId equals f.role_id into rm
                                                         from f in rm.DefaultIfEmpty()
                                                         join s in _db.tbl_status_master on a.ApprovalStatus equals s.StatusId
                                                         select new ActiveDeactiveTradeHistory
                                                         {
                                                             //CreatedOn = a.CreatedOn,
                                                             // CreatedBy = a.CreatedBy,
                                                             FilePath = a.FilePath,
                                                             ActDeActRemarks = a.ActDeActRemarks,
                                                             //IsActive = a.IsActive,
                                                             //ITICode = a.ITICode,
                                                             //Remarks = a.Remarks,
                                                             //StatusId = a.StatusId,
                                                             //TradeCode = a.TradeCode,
                                                             //Trade_ITI_His_id = a.Trade_ITI_His_id,
                                                             //Trade_ITI_id = a.Trade_ITI_id,
                                                             //Unit = a.Unit,
                                                             CreatedBy = a.CreatedBy,
                                                             //Flow_user = c.user_role,
                                                             date = a.CreatedOn.ToString(),
                                                             //CreatedUser = c.id.ToString()
                                                             User = c.role_description,
                                                             ActiveFilepath = b.FilePath,
                                                             Flow_user = f.role_description??"-",
                                                             Status = s.StatusName,



                                                         }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public tbl_ITI_Institute_ActiveStatus Get_tbl_ITI_Institute_ActiveStatusdll(int ITI_Institute_Id)
        {
            return _db.tbl_ITI_Institute_ActiveStatus.Where(x => x.ITI_Institute_Id == ITI_Institute_Id).FirstOrDefault();
        }
        public List<tbl_Affiliation_documents> Get_tbl_Affiliation_documents(int itiID, int? TradeIds = null,
            int? Units = null, int? Shift = null)
        {
            return _db.tbl_Affiliation_documents.Where(a => a.Institute_id == itiID
            && a.Trade_Id == TradeIds && a.Unit == Units  && a.Shift == Shift).ToList();
        }

        public bool Save_tbl_ITI_Institute_ActiveStatusdll(tbl_ITI_Institute_ActiveStatus model)
        {
            try
            {
                if (model.ActiveITIId > 0)
                {
                    _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    _db.tbl_ITI_Institute_ActiveStatus.Add(model);
                }

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }

        }
        public bool Save_tbl_Affiliation_documents(tbl_Affiliation_documents model,bool isUpdate)
        {
            try
            {
                if (isUpdate)
                {
                    _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    _db.tbl_Affiliation_documents.Add(model);
                }

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool CreateDeaffiliateInstituteDLL(tbl_ITI_Institute_His_ActiveStatus model)
        {
            try
            {
                _db.tbl_ITI_Institute_His_ActiveStatus.Add(model);

                int abc = _db.SaveChanges();
                if (abc == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }



        public List<DeAffiliateInstitute> GetAllInstituteDetails(int DistId)
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join Statusmaster in _db.tbl_status_master on stsm.ApprovalStatus equals Statusmaster.StatusId
                                                    into bb
                                                    from Statusmaster in bb.DefaultIfEmpty()
                                                        //where stsm.ApprovalFlowId== (int)Models.Affiliation.CsystemType.getCommon.OFFICE_SUPERINTENDENT
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = Statusmaster.StatusName ?? null,
                                                        MisCode = clg.MISCode,
                                                        //Trade_ITI_id = trade.Trade_ITI_id,
                                                        iti_college_id = clg.iti_college_id,
                                                        createdon = stsm.CreatedOn

                                                    }).ToList();



                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        #region Get Active Deactive unit deatils On Filter

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitsDeatilsDLLOnFilter()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             where tradeshift.IsActive == true || tradeshift.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 //status_id = clz.StatusId,
                                                                 Shifts = tradeshift.Shift,
                                                                 StatusName = Statusmaster.StatusName,
                                                                 // StatusName = ust.ApprovalStatus,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 Courseid = clz.CourseCode,
                                                                 Division_id = clz.division_id,
                                                                 District_id = clz.district_id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,



                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get Active Deactive unit deatils for OS

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieOSDeatilsDLLOnFilter()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from trade_his in _db.tbl_ITI_Trade_Shift_ActiveStatus_History
                                                             join tradeshift in _db.tbl_ITI_Trade_Shift_ActiveStatus on trade_his.ITI_Trade_Id equals tradeshift.ITI_Trade_Id
                                                             join trade in _db.tbl_ITI_Trade on trade_his.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id

                                                             join Statusmaster in _db.tbl_status_master on tradeshift.ApprovalStatus equals Statusmaster.StatusId
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                                 //where (trade.IsActive == true || trade.IsActive == false) && tradeshift.ApprovalFlowId == 2
                                                             where (trade.IsActive == true || trade.IsActive == false)
                                                             //&& trade_his.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.OFFICE_SUPERINTENDENT
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 miscode = clz.MISCode,
                                                                 createdon = tradeshift.CreatedOn,
                                                                 Shifts = tradeshift.Shift,
                                                                 StatusName = Statusmaster.StatusName,
                                                                 IsActive = tradeshift.IsActive,
                                                                 //ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 Courseid = clz.CourseCode,
                                                                 Division_id = clz.division_id,
                                                                 District_id = clz.district_id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename

                                                             }).ToList();
                items = items.GroupBy(a => a.ITI_Trade_Shift_Id).Select(z => new ActiveandDeactiveUnitsDeatils
                {
                    ITI_Trade_Shift_Id = z.Key,
                    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                    trades = z.Select(c => c.trades).FirstOrDefault(),
                    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                    units = z.Select(c => c.units).FirstOrDefault(),
                    miscode = z.Select(c => c.miscode).FirstOrDefault(),
                    Shifts = z.Select(c => c.Shifts).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    Courseid = z.Select(c => c.Courseid).FirstOrDefault(),
                    Division_id = z.Select(c => c.Division_id).FirstOrDefault(),
                    District_id = z.Select(c => c.District_id).FirstOrDefault(),
                    CourseType = z.Select(c => c.CourseType).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get Active Deactive unit deatils for ADDD

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieADDDDeatilsDLLOnFilter()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from trade in _db.tbl_ITI_Trade
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             join tradeshift in _db.tbl_ITI_Trade_Shift_ActiveStatus on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id
                                                             join Statusmaster in _db.tbl_status_master on tradeshift.ApprovalStatus equals Statusmaster.StatusId
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                                 // join Statusmaster in _db.tbl_status_master on tradeshift.ApprovalStatus equals Statusmaster.StatusId
                                                                 // where tradeshift.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Assistant_Director || tradeshift.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Deputy_Director
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 miscode = clz.MISCode,
                                                                 createdon = tradeshift.CreatedOn,
                                                                 Shifts = tradeshift.Shift,
                                                                 StatusName = Statusmaster.StatusName,
                                                                 IsActive = tradeshift.IsActive,
                                                                 // ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 Courseid = clz.CourseCode,
                                                                 Division_id = clz.division_id,
                                                                 District_id = clz.district_id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename

                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get All Institute Details for De-Affiliate

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteDLLCwFilter()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details

                                                    join iast in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals iast.ITI_Institute_Id into bb
                                                    from iast in bb.DefaultIfEmpty()
                                                    join stsm in _db.tbl_status_master on iast.ApprovalStatus equals stsm.StatusId into aa
                                                    from stsm in aa.DefaultIfEmpty()
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on iast.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where clg.is_active == true

                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = (stsm.StatusId != 2 && stsm.StatusId != 19 ? stsm.StatusName + " - " + rl.role_DescShortForm : stsm.StatusName),
                                                        //StatusName = stsm.StatusName,
                                                        //ActiveDeActive = trade.ActiveDeActive,
                                                        // Trade_ITI_id = trade.Trade_ITI_id,
                                                        iti_college_id = clg.iti_college_id,
                                                        IsActive = clg.is_active,
                                                        IsActive1 = iast.IsActive,
                                                        MisCode = clg.MISCode,
                                                        Course_id = clg.CourseCode,
                                                        District_id = clg.district_id,
                                                        Division_id = clg.division_id,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteOSDLLOnSearch()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join Statusmaster in _db.tbl_status_master on stsm.ApprovalStatus equals Statusmaster.StatusId
                                                    into bb
                                                    from Statusmaster in bb.DefaultIfEmpty()
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on stsm.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where stsm.ApprovalFlowId== (int)Models.Affiliation.CsystemType.getCommon.OFFICE_SUPERINTENDENT
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = (Statusmaster.StatusId != 2 && Statusmaster.StatusId != 19 ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusName),
                                                        // StatusName = Statusmaster.StatusName ?? null,
                                                        MisCode = clg.MISCode,
                                                        IsActive = clg.is_active,
                                                        iti_college_id = clg.iti_college_id,
                                                        createdon = stsm.CreatedOn,
                                                        IsActive1 = stsm.IsActive,
                                                        Course_id = clg.CourseCode,
                                                        Division_id = clg.division_id,
                                                        District_id = clg.district_id,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,

                                                    }).ToList();
                //items = items.GroupBy(a => a.iti_college_id).Select(z => new DeAffiliateInstitute
                //{
                //    iti_college_id = z.Key,
                //    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                //    trades = z.Select(c => c.trades).FirstOrDefault(),
                //    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                //    units = z.Select(c => c.units).FirstOrDefault(),
                //    MisCode = z.Select(c => c.MisCode).FirstOrDefault(),

                //    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                //    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),



                //}).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public List<DeAffiliateInstitute> GetAllDeaffiliateInstituteADDLLOnsearch()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join stsm1 in _db.tbl_status_master on stsm.ApprovalStatus equals stsm1.StatusId
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                    join rl in _db.tbl_role_master on stsm.ApprovalFlowId equals rl.role_id into rm
                                                    from rl in rm.DefaultIfEmpty()
                                                        //where stsm.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Assistant_Director || stsm.ApprovalFlowId == (int)Models.Affiliation.CsystemType.getCommon.Deputy_Director
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = (stsm1.StatusId != 2 && stsm1.StatusId != 19 ? stsm1.StatusName + " - " + rl.role_DescShortForm : stsm1.StatusName),
                                                        //StatusName = stsm1.StatusName,
                                                        MisCode = clg.MISCode,
                                                        IsActive = clg.is_active,
                                                        iti_college_id = clg.iti_college_id,
                                                        createdon = stsm.CreatedOn,
                                                        District_id = clg.district_id,
                                                        Division_id = clg.division_id,
                                                        Course_id = clg.CourseCode,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        #region Get All DeaffiliateInstitute POPUP

        /// <summary>
        /// GetAllAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<DeAffiliateInstitute> GetAllDeaffiliateInstitutePOPUP()
        {
            try
            {

                List<DeAffiliateInstitute> items = (from clg in _db.tbl_iti_college_details
                                                    join stsm in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals stsm.ITI_Institute_Id
                                                    join stsm1 in _db.tbl_status_master on stsm.ApprovalStatus equals stsm1.StatusId
                                                    join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                             into crs
                                                    from cour in crs.DefaultIfEmpty()
                                                    join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                    from l in ll.DefaultIfEmpty()
                                                    join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                    from c in dt.DefaultIfEmpty()
                                                        // join userm in tbl_user_roles on userm.
                                                    where stsm.ApprovalStatus == (int)CsystemType.getCommon.pub /*|| stsm.ApprovalStatus == 19*/
                                                    select new DeAffiliateInstitute
                                                    {

                                                        iti_college = clg.iti_college_name,
                                                        StatusName = stsm1.StatusName,
                                                        MisCode = clg.MISCode,
                                                        //Trade_ITI_id = trade.Trade_ITI_id,
                                                        iti_college_id = clg.iti_college_id,
                                                        IsActive = stsm.IsActive,
                                                        createdon = stsm.CreatedOn,
                                                        CourseType = cour.course_type_name,
                                                        Division = l.division_name,
                                                        District = c.district_ename,
                                                        Clg_Adderss = clg.college_address,
                                                        date = stsm.CreatedOn.ToString(),

                                                    }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion


        #region Add Affiliation College Details Test
        /// <summary>
        /// AddAffiliationCollegeDetailsDLL
        /// </summary>
        /// <param name="Affi1"></param>
        /// <returns></returns>
        public AffiliationCollegeDetailsTest AddAffiliationCollegeDetailsDLL1(AffiliationCollegeDetailsTest Affi1)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    NestedTradeSession session = (NestedTradeSession)HttpContext.Current.Session["TradeShift"];

                    var anEntry = new tbl_iti_college_details();
                    anEntry.iti_college_name = Affi1.name_of_iti;
                    anEntry.Insitute_TypeId = Affi1.type_of_iti_id;
                    anEntry.location_id = Affi1.location_type_id;
                    anEntry.MISCode = Affi1.mis_code;
                    //anEntry.css_code = Affi.css_code_id;
                    anEntry.district_id = Affi1.dist_id;
                    anEntry.taluk_id = Affi1.taluk_id;
                    anEntry.Constituency = Affi1.consti_id;
                    anEntry.Panchayat = Affi1.pancha_id;
                    anEntry.village_or_town = Affi1.village_id;
                    //anEntry.BuildUpArea = Affi.build_up_area;
                    anEntry.college_address = Affi1.address;
                    anEntry.geo = Affi1.geo_location;
                    anEntry.email_id = Affi1.email;
                    anEntry.phone_num = Affi1.phone_number;
                    //anEntry.AffiliationDate = Convert.ToDateTime(Affi1.affiliation_date.ToString("dd/MM/yyyy"));
                    anEntry.AffiliationDate = Affi1.affiliation_date;
                    anEntry.Units = Affi1.no_units;
                    anEntry.NoOfShifts = Affi1.no_shifts;
                    //if (Affi1.FileUploadPath != null)
                    //{
                    //    anEntry.UploadAffiliationDoc = Affi1.FileUploadPath;
                    //}
                    anEntry.iti_college_code = "0";
                    anEntry.created_by = Affi1.CreatedBy;
                    anEntry.creation_datetime = DateTime.Now;
                    anEntry.PinCode = Affi1.Pincode;
                    anEntry.StatusId = (int)CsystemType.getCommon.Sent_for_Review;
                    anEntry.ActiveDeActive = true;
                    anEntry.Website = Affi1.Website;
                    anEntry.AffiliationOrderNo = Affi1.AffiliationOrderNo;
                    // anEntry.AffiliationOrderNoDate = Convert.ToDateTime(Affi1.AffiliationOrderNoDate.ToString("dd/MM/yyyy"));
                    anEntry.AffiliationOrderNoDate = Affi1.AffiliationOrderNoDate;
                    anEntry.Scheme = Affi1.Scheme;
                    anEntry.AidedUnaidedTrade = Affi1.AidedUnaidedTrade;
                    anEntry.CourseCode = Affi1.course_code;
                    anEntry.division_id = Affi1.division_id;

                    _db.tbl_iti_college_details.Add(anEntry);
                    _db.SaveChanges();

                    Affi1.iti_college_id = anEntry.iti_college_id;

                    if (Affi1.list_trades != null)
                    {
                        if (Affi1.list_trades.Count() > 0)
                        {
                            int Status = (int)CsystemType.getCommon.Sent_for_Review;

                            foreach (var item in Affi1.trades_list)

                            {
                                TradeShiftSessions shifts = session.sessions.Find(a => a.sessionKey == item.sessionKey);

                                var Add = new tbl_ITI_Trade();
                                Add.ITICode = Affi1.iti_college_id;
                                Add.TradeCode = item.trade_id;
                                Add.CreatedOn = DateTime.Now;
                                Add.CreatedBy = Affi1.CreatedBy;
                                Add.StatusId = Status;
                                Add.Unit = item.units;
                                Add.FlowId = Affi1.flow_id;//(int)CsystemType.getCommon.OFFICE_SUPERINTENDENT;
                                Add.IsActive = false;
                                Add.FileUploadPath = item.file_upload_path;
                                Add.ActiveDeActive = true;
                                Add.color_flag = Affi1.color_flag;
                                Add.AidedUnaidedTrade = item.type;

                                _db.tbl_ITI_Trade.Add(Add);
                                _db.SaveChanges();

                                var Add_His = new tbl_ITI_Trade_History();
                                Add_His.ITICode = Affi1.iti_college_id;
                                Add_His.TradeCode = Convert.ToInt32(item.trade_id);
                                Add_His.CreatedOn = DateTime.Now;
                                Add_His.CreatedBy = Affi1.CreatedBy;
                                Add_His.StatusId = Status;
                                Add_His.Unit = item.units;
                                Add_His.FlowId = Affi1.flow_id;
                                Add_His.IsActive = false;
                                Add_His.Remarks = Affi1.remarks;//"New Affiliation Institute Added";
                                Add_His.FileUploadPath = item.file_upload_path;
                                Add_His.Trade_ITI_id = Add.Trade_ITI_id;


                                _db.tbl_ITI_Trade_Histories.Add(Add_His);
                                _db.SaveChanges();


                                if (shifts != null)
                                {
                                    if (shifts.list != null)
                                    {

                                        foreach (var item2 in shifts.list)
                                        {

                                            var insertShift = new tbl_ITI_Trade_Shift();
                                            insertShift.CreatedBy = Affi1.CreatedBy;
                                            insertShift.CreatedOn = DateTime.Now;
                                            insertShift.Dual_System = item2.Dual_System;
                                            insertShift.IsActive = false;
                                            insertShift.IsPPP = item2.IsPPP;
                                            insertShift.ITI_Trade_Id = Add.Trade_ITI_id;
                                            insertShift.Shift = item2.Shift;
                                            insertShift.Units = item2.Units;
                                            insertShift.Status = (int)CsystemType.getCommon.Submitted;

                                            _db.tbl_ITI_Trade_Shifts.Add(insertShift);
                                            _db.SaveChanges();

                                        }


                                    }

                                }
                                var Trade_Doc = new tbl_Affiliation_documents();
                                Trade_Doc.Institute_id = Affi1.iti_college_id;
                                Trade_Doc.Trade_Id = Convert.ToInt32(item.trade_id);
                                if(Affi1.FileUploadPath!=null)
                                {
                                    Trade_Doc.FileName = Affi1.FileUploadPath;
                                }
                                Trade_Doc.IsActive = true;
                                Trade_Doc.Status = "New Institute";
                               // Trade_Doc.Flag = (int)CsystemType.getCommon.New_Trade;
                                Trade_Doc.AffiliationOrder_Number = Affi1.AffiliationOrderNo;
                                Trade_Doc.Affiliation_date = Affi1.AffiliationOrderNoDate; ;
                                _db.tbl_Affiliation_documents.Add(Trade_Doc);
                                _db.SaveChanges();
                            }
                        }
                    }
                    transaction.Complete();
                }

                return Affi1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public AffiliationNested PublishActiveDeactiveTradeUnit(AffiliationNested model)
        {

            try
            {

                foreach (var p in model.ViewUnitDetails)
                {
                    if (model != null)
                    {
                        var update = _db.tbl_ITI_Trade_Shift_ActiveStatus.Where(a => a.ITI_Trade_Shift_Id == p.ITI_Trade_Shift_Id).FirstOrDefault();

                        update.ApprovalStatus = 19;


                        _db.SaveChanges();
                    }


                }
                if (model != null) { 
                _CommonDLL.NotificationPublish(1);
                }

                return model;

            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        public AffiliationNested PublishAffiliateDeaffiliateInstitute(AffiliationNested model)
        {

            try
            {

                foreach (var p in model.Approvedinst_list)
                {
                    if (model != null)
                    {
                        var update = _db.tbl_ITI_Institute_ActiveStatus.Where(a => a.ITI_Institute_Id == p.iti_college_id).FirstOrDefault();

                        update.ApprovalStatus = 19;

                        _db.SaveChanges();
                    }

                }

                if (model != null)
                {
                    _CommonDLL.NotificationPublish(2);
                }
                return model;

            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }


        #region Get All ActiveandDeactive UnitwsieView POPUP

        public List<ActiveandDeactiveUnitsDeatils> GetAllActiveandDeactiveUnitwsieViewPOPUP()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from trade in _db.tbl_ITI_Trade
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             join tradeshift in _db.tbl_ITI_Trade_Shift_ActiveStatus on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id
                                                             join tradesh in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradesh.ITI_Trade_Id
                                                          into ts
                                                             from tradesh in ts.DefaultIfEmpty()
                                                                 //join trad_his in _db.tbl_ITI_Trade_Shift_ActiveStatus_History on tradesh.ITI_Trade_Shift_Id equals trad_his.ITI_Trade_Shift_Id

                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()

                                                             join Statusmaster in _db.tbl_status_master on tradeshift.ApprovalStatus equals Statusmaster.StatusId
                                                             where tradeshift.ApprovalStatus == 19
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 createdon = tradeshift.CreatedOn,
                                                                 Shifts = tradeshift.Shift,
                                                                 StatusName = Statusmaster.StatusName,
                                                                 ActiveDeActive = trade.ActiveDeActive,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 miscode = clz.MISCode,
                                                                 IsActive = trade.IsActive,
                                                                 IsActiveTr = tradesh.IsActive,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 college_Address = clz.college_address,
                                                                 date = SqlFunctions.DatePart("day", tradeshift.CreatedOn) + "-" + SqlFunctions.DatePart("m", tradeshift.CreatedOn) + "-" + SqlFunctions.DatePart("year", tradeshift.CreatedOn),
                                                                 //tradeshift.CreatedOn.ToShortDateString()



                                                             }).ToList();
                items = items.GroupBy(a => a.ITI_Trade_Shift_Id).Select(z => new ActiveandDeactiveUnitsDeatils
                {
                    ITI_Trade_Shift_Id = z.Key,
                    iti_college = z.Select(b => b.iti_college).FirstOrDefault(),
                    trades = z.Select(c => c.trades).FirstOrDefault(),
                    createdon = z.Select(c => c.createdon).FirstOrDefault(),
                    units = z.Select(c => c.units).FirstOrDefault(),
                    miscode = z.Select(c => c.miscode).FirstOrDefault(),
                    Shifts = z.Select(c => c.Shifts).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    CourseType = z.Select(c => c.CourseType).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    IsActive1 = z.Select(c => c.IsActive1).FirstOrDefault(),
                    IsActiveTr = z.Select(c => c.IsActiveTr).FirstOrDefault(),
                    college_Address = z.Select(c => c.college_Address).FirstOrDefault(),
                    date = z.Select(c => c.date).FirstOrDefault(),
                    // ITI_Trade_Shift_Id = z.Select(c => c.ITI_Trade_Shift_Id).FirstOrDefault()


                }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        public ToPublishRecords PublishAffiliateInstitutes(ToPublishRecords model)
        {

            try
            {

                foreach (var p in model.pubs_list_topublish)
                {
                    if (model != null)
                    {
                        var update = _db.tbl_ITI_Trade.Where(a => a.Trade_ITI_id == p.trade_iti_id_publish).FirstOrDefault();
                        if (update != null)
                        {
                            update.StatusId = 19;
                            _db.SaveChanges();
                        }
                        else
                        {

                        }




                    }

                }

                if (model != null)
                {
                    _CommonDLL.NotificationPublish(3);
                }
                return model;

            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        

        #region Get All DesignationDLL
        /// <summary>
        /// Get All DesignationDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllDesignationDLL()
        {
            try
            {
                var list = (from a in _db.tbl_designation_master
                            select new SelectListItem
                            {
                                Text = a.Designation,
                                Value = a.Designation_Id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All TeachingSubjectDLL
        /// <summary>
        /// Get All TeachingSubjectDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllTeachingSubjectDLL()
        {
            try
            {
                var list = (from a in _db.tbl_subject
                            select new SelectListItem
                            {
                                Text = a.subject_name,
                                Value = a.subject_id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Get All TradesDLL
        /// <summary>
        /// Get All TradesDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllTradesDLL()
        {
            try
            {
                var list = (from a in _db.tbl_trade_mast
                            select new SelectListItem
                            {
                                Text = a.trade_name,
                                Value = a.trade_id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Gender
        /// <summary>
        /// Get All Gender
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllGender()
        {
            try
            {
                var list = (from a in _db.tbl_Gender
                            select new SelectListItem
                            {
                                Text = a.Gender,
                                Value = a.Gender_Id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Affiliated Institute Report DLL

        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLL(int Year_id, int Course_Id, int Division_Id, int District_Id, int taluk_id, int Insttype_Id, int location_Id, int tradeId, int scheme_Id, string training_Id, int ReportType_Id)
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join it in _db.tbl_Institute_type on clz.Insitute_TypeId equals it.Institute_type_id into ist
                                                             from it in ist.DefaultIfEmpty()
                                                             join lo in _db.tbl_location_type on clz.location_id equals lo.location_id into lct
                                                             from lo in lct.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                             join d in _db.tbl_taluk_master on clz.taluk_id equals d.taluk_lgd_code into tlk
                                                             from d in tlk.DefaultIfEmpty()
                                                             join o in _db.tbl_trade_scheme on clz.Scheme equals o.ts_id into oo
                                                             from o in oo.DefaultIfEmpty()

                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                                 //where tradeshift.IsActive == true || tradeshift.IsActive == false
                                                             where /*(Year_id != 0 ? taid.AdmittedStatus == AdmittedorRejected : true)*/  (Course_Id != 0 ? cour.course_id == Course_Id : true)
                                                             && (Division_Id != 0 ? l.division_id == Division_Id : true) && (District_Id != 0 ? c.district_lgd_code == District_Id : true)
                                                             && (taluk_id != 0 ? d.taluk_lgd_code == taluk_id : true) && (Insttype_Id != 0 ? it.Institute_type_id == Insttype_Id : true)
                                                             && (location_Id != 0 ? lo.location_id == location_Id : true) && (tradeId != 0 ? master.trade_id == tradeId : true)
                                                             && (scheme_Id != 0 ? o.ts_id == scheme_Id : true) && (training_Id != "0" ? tradeshift.Dual_System == training_Id : true) && (Year_id == 2 ? true : false)
                                                             && (ReportType_Id != 0 ? (ReportType_Id == 1 ? clz.is_active == true : false) || (ReportType_Id == 2 ? clz.is_active == false : false)
                                                             || (ReportType_Id == 3 ? trade.IsActive == false : false) || (ReportType_Id == 4 ? tradeshift1.IsActive == false : false) : true)


                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 Shifts = tradeshift.Shift,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = clz.is_active,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 InstituteType = it.Institute_type,
                                                                 LocationType = lo.location_name,
                                                                 ModeofTraining = tradeshift.Dual_System,
                                                                 Courseid = cour.course_id,
                                                                 Division_id = l.division_id,
                                                                 District_id = c.district_lgd_code,
                                                                 location_Id = lo.location_id,
                                                                 Taluk_id = d.taluk_lgd_code,
                                                                 Insttype_Id = it.Institute_type_id,
                                                                 ITI_trade_id = master.trade_id,
                                                                 scheme_Id = o.ts_id,
                                                                 Taluk = d.taluk_ename,
                                                                 Scheme = o.trade_scheme,
                                                                 NoOfUnits= tradeshift.Units.ToString(),
                                                                 ReqIsActive=clz.is_active


                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get All Deaffiliate Institute DLL For Report

        /// <summary>
        /// GetAllDeaffiliateInstituteDLLForReport
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<ActiveandDeactiveUnitsDeatils> GetAllDeaffiliateInstituteDLLForReport()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from clg in _db.tbl_iti_college_details

                                                             join iast in _db.tbl_ITI_Institute_ActiveStatus on clg.iti_college_id equals iast.ITI_Institute_Id into bb
                                                             from iast in bb.DefaultIfEmpty()
                                                             join stsm in _db.tbl_status_master on iast.ApprovalStatus equals stsm.StatusId into aa
                                                             from stsm in aa.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clg.CourseCode equals cour.course_id
                                                                      into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clg.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clg.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on iast.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                             join d in _db.tbl_taluk_master on clg.taluk_id equals d.taluk_lgd_code into dd
                                                             from d in dd.DefaultIfEmpty()
                                                             join h in _db.tbl_location_type on clg.location_id equals h.location_id into hh
                                                             from h in hh.DefaultIfEmpty()
                                                             join i in _db.tbl_Institute_type on clg.Insitute_TypeId equals i.Institute_type_id into ii
                                                             from i in ii.DefaultIfEmpty()
                                                             where clg.is_active == false

                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {

                                                                 iti_college = clg.iti_college_name,
                                                                 StatusName = (stsm.StatusId != 2 && stsm.StatusId != 19 ? stsm.StatusName + " - " + rl.role_DescShortForm : stsm.StatusName),
                                                                 iti_college_id = clg.iti_college_id,
                                                                 IsActive = clg.is_active,
                                                                 miscode = clg.MISCode,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 date = iast.CreatedOn.ToString(),
                                                                 Courseid = cour.course_id,
                                                                 Division_id = l.division_id,
                                                                 District_id = c.district_lgd_code,
                                                                 Taluk_id = d.taluk_lgd_code,
                                                                 Insttype_Id = i.Institute_type_id,
                                                                 location_Id = h.location_id,
                                                                 Taluk = d.taluk_ename,
                                                                 InstituteType = i.Institute_type,
                                                                 LocationType = h.location_name,

                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get All Affiliated Institute Report DLL For Trade

        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForTrade()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join it in _db.tbl_Institute_type on clz.Insitute_TypeId equals it.Institute_type_id into ist
                                                             from it in ist.DefaultIfEmpty()
                                                             join lo in _db.tbl_location_type on clz.location_id equals lo.location_id into lct
                                                             from lo in lct.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                             join d in _db.tbl_taluk_master on clz.taluk_id equals d.taluk_lgd_code into tlk
                                                             from d in tlk.DefaultIfEmpty()
                                                             join o in _db.tbl_trade_scheme on clz.Scheme equals o.ts_id into oo
                                                             from o in oo.DefaultIfEmpty()

                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             where trade.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 Shifts = tradeshift.Shift,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = clz.is_active,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 InstituteType = it.Institute_type,
                                                                 LocationType = lo.location_name,
                                                                 ModeofTraining = tradeshift.Dual_System,
                                                                 Courseid = cour.course_id,
                                                                 Division_id = l.division_id,
                                                                 District_id = c.district_lgd_code,
                                                                 location_Id = lo.location_id,
                                                                 Taluk_id = d.taluk_lgd_code,
                                                                 Insttype_Id = it.Institute_type_id,
                                                                 ITI_trade_id = master.trade_id,
                                                                 scheme_Id = o.ts_id,
                                                                 Taluk = d.taluk_ename,
                                                                 Scheme = o.trade_scheme,
                                                                 NoOfUnits= tradeshift.Units.ToString()







                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region Get All Affiliated Institute Report DLL For Units

        public List<ActiveandDeactiveUnitsDeatils> GetAllAffiliatedInstituteReportDLLForUnits()
        {
            try
            {

                List<ActiveandDeactiveUnitsDeatils> items = (from tradeshift in _db.tbl_ITI_Trade_Shifts
                                                             join trade in _db.tbl_ITI_Trade on tradeshift.ITI_Trade_Id equals trade.Trade_ITI_id
                                                             into aa
                                                             from trade in aa.DefaultIfEmpty()
                                                             join clz in _db.tbl_iti_college_details on trade.ITICode equals clz.iti_college_id
                                                             into bb
                                                             from clz in bb.DefaultIfEmpty()
                                                             join master in _db.tbl_trade_mast on trade.TradeCode equals master.trade_id
                                                             into cc
                                                             from master in cc.DefaultIfEmpty()
                                                                 // join tradeshift in _db.tbl_ITI_Trade_Shifts on trade.Trade_ITI_id equals tradeshift.ITI_Trade_Id 
                                                             join tradeshift1 in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals tradeshift1.ITI_Trade_Shift_Id
                                                             into dd
                                                             from tradeshift1 in dd.DefaultIfEmpty()
                                                             join Statusmaster in _db.tbl_status_master on tradeshift1.ApprovalStatus equals Statusmaster.StatusId
                                                             into ee
                                                             from Statusmaster in ee.DefaultIfEmpty()
                                                             join cour in _db.tbl_course_type_mast on clz.CourseCode equals cour.course_id
                                                             into crs
                                                             from cour in crs.DefaultIfEmpty()
                                                             join l in _db.tbl_division_master on clz.division_id equals l.division_id into ll
                                                             from l in ll.DefaultIfEmpty()
                                                             join c in _db.tbl_district_master on clz.district_id equals c.district_lgd_code into dt
                                                             from c in dt.DefaultIfEmpty()
                                                             join it in _db.tbl_Institute_type on clz.Insitute_TypeId equals it.Institute_type_id into ist
                                                             from it in ist.DefaultIfEmpty()
                                                             join lo in _db.tbl_location_type on clz.location_id equals lo.location_id into lct
                                                             from lo in lct.DefaultIfEmpty()
                                                             join rl in _db.tbl_role_master on tradeshift1.ApprovalFlowId equals rl.role_id into rm
                                                             from rl in rm.DefaultIfEmpty()
                                                             join d in _db.tbl_taluk_master on clz.taluk_id equals d.taluk_lgd_code into tlk
                                                             from d in tlk.DefaultIfEmpty()
                                                             join o in _db.tbl_trade_scheme on clz.Scheme equals o.ts_id into oo
                                                             from o in oo.DefaultIfEmpty()

                                                                 // join  ust in _db.tbl_ITI_Trade_Shift_ActiveStatus on tradeshift.ITI_Trade_Shift_Id equals ust.ITI_Trade_Shift_Id
                                                             where tradeshift.IsActive == false
                                                             select new ActiveandDeactiveUnitsDeatils
                                                             {
                                                                 iti_college = clz.iti_college_name,
                                                                 miscode = clz.MISCode,
                                                                 trades = master.trade_name,
                                                                 units = tradeshift.Units,
                                                                 Shifts = tradeshift.Shift,
                                                                 IsActive = tradeshift.IsActive,
                                                                 IsActive1 = trade.IsActive,
                                                                 ActiveDeActive = clz.is_active,
                                                                 Trade_ITI_id = trade.Trade_ITI_id,
                                                                 ITI_Trade_Shift_Id = tradeshift.ITI_Trade_Shift_Id,
                                                                 CourseType = cour.course_type_name,
                                                                 Division = l.division_name,
                                                                 District = c.district_ename,
                                                                 iti_college_id = clz.iti_college_id,
                                                                 date = tradeshift1.CreatedOn.ToString(),
                                                                 CreatedonOrderby = tradeshift1.CreatedOn,
                                                                 InstituteType = it.Institute_type,
                                                                 LocationType = lo.location_name,
                                                                 ModeofTraining = tradeshift.Dual_System,
                                                                 Courseid = cour.course_id,
                                                                 Division_id = l.division_id,
                                                                 District_id = c.district_lgd_code,
                                                                 location_Id = lo.location_id,
                                                                 Taluk_id = d.taluk_lgd_code,
                                                                 Insttype_Id = it.Institute_type_id,
                                                                 ITI_trade_id = master.trade_id,
                                                                 scheme_Id = o.ts_id,
                                                                 Taluk = d.taluk_ename,
                                                                 Scheme = o.trade_scheme,
                                                                 NoOfUnits=tradeshift.Units.ToString()







                                                             }).ToList();


                return items;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion


        #region Get All YearDLL
        /// <summary>
        /// Get All YeareDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllYearDLL()
        {
            try
            {
                var list = (from a in _db.tbl_Year.Where(a=>a.IsActive==true)
                            select new SelectListItem
                            {
                                Text = a.Year,
                                Value = a.YearID.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All DesignationDLL
        /// <summary>
        /// Get All DesignationDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllInstitute(int Distid)
        {
            try
            {
                var list = (from a in _db.tbl_iti_college_details.Where(a => (a.StatusId == 2|| a.StatusId == 6|| a.StatusId == 19)&& a.district_id==Distid)
                                //(from a in _db.tbl_iti_college_details.Where(a=>a.Insitute_TypeId==2 )
                            select new SelectListItem
                            {
                                Text = a.iti_college_name,
                                Value = a.iti_college_id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Get All Affiliation Doc
        /// <summary>
        /// GetAllTradeShiftsDLL
        /// </summary>
       
        /// <returns></returns>
        public List<AffiliationDocuments> GetAllAffiliationDoc()
        {
            try
            {
                List<AffiliationDocuments> list = (from a in _db.tbl_Affiliation_documents
                                                   //join td  in _db.tbl_ITI_Trade on a.Trade_Id equals td.Trade_ITI_id into cc
                                                   //from td in cc.DefaultIfEmpty()
                                                   join b in _db.tbl_trade_mast on a.Trade_Id equals b.trade_id into tm
                                                   from b in tm.DefaultIfEmpty()

                                                   select new AffiliationDocuments
                                         {
                                             Id=a.Id,
                                             Institute_id=a.Institute_id,
                                             Trade_Id=a.Trade_Id,
                                             Unit=a.Unit,
                                             Shift=a.Shift,
                                             FileName= a.FileName,
                                             AffiliationOrder_Number=a.AffiliationOrder_Number,
                                             //Affiliation_date=a.Affiliation_date.ToString(),
                                             Affiliation_date= SqlFunctions.DatePart("day", a.Affiliation_date) + "/" + SqlFunctions.DatePart("m", a.Affiliation_date) + "/" + SqlFunctions.DatePart("year", a.Affiliation_date),
                                                       
                                             Flag=a.Flag,
                                             Status=a.Status,
                                             Trade_Name=b.trade_name


                                         }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public AffiliationDocuments GetAllAffiliationDocForDownload(int collegeId, int? Trade_Id, int? shift_id, int? flag)
        {
            try
            {
                AffiliationDocuments list = (from a in _db.tbl_Affiliation_documents.Where(a => (collegeId != 0 ? a.Institute_id == collegeId : true) && (Trade_Id!=0? a.Trade_Id == Trade_Id:true) && (shift_id!=0 ? a.Shift== shift_id:true) && (flag != 0 ? a.Flag == flag : true))

                                             select new AffiliationDocuments
                                                   {
                                                       Id = a.Id,
                                                       Institute_id = a.Institute_id,
                                                       Trade_Id = a.Trade_Id,
                                                       Unit = a.Unit,
                                                       Shift = a.Shift,
                                                       FileName = a.FileName,
                                                       AffiliationOrder_Number = a.AffiliationOrder_Number,
                                                       //Affiliation_date = a.Affiliation_date.ToString(),
                                                       Affiliation_date = SqlFunctions.DatePart("day", a.Affiliation_date) + "/" + SqlFunctions.DatePart("m", a.Affiliation_date) + "/" + SqlFunctions.DatePart("year", a.Affiliation_date),
                                                       Flag=a.Flag,
                                                       Status=a.Status

                                                   }).FirstOrDefault();
                return list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region staff methods

        #region public methods
        /// <summary>
        /// GetAffiliationInstituteIdDLL
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetAffiliationInstituteIdDLL(int UserId)
        {
            try
            {
                Staff_Institute_Detail map = _db.Staff_Institute_Detail.Where(a => a.UserId == UserId).FirstOrDefault();

                if (map != null)
                {
                    return map.InstituteId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        /// <summary>
        /// Get All StaffTypeDLL
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllStaffTypeDLL()
        {
            try
            {
                var list = (from a in _db.tbl_staff_type_master.Where(a => a.isactive == true)
                            select new SelectListItem
                            {
                                Text = a.Staff_Type,
                                Value = a.Staff_type_id.ToString()

                            }).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StaffInstituteDetails> GetApprovedstaffInstitute(int loginId, int year, int courseId, int DivisionId,
            int DistrictId, int InstituteId, int quarter)
        {
            try
            {
                int? div_id = _db.tbl_user_master.Where(a => a.um_id == loginId).Select(a => a.um_div_id).FirstOrDefault();
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                           equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                           //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                           //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into styp
                           from st in styp.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into tyr
                           from yr in tyr.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId
                           join rl in _db.tbl_role_master on sy.ApprovalFlowId equals rl.role_id into rm
                           from rl in rm.DefaultIfEmpty()
                           where sy.ApprovalStatus == (int)CsystemType.getCommon.Approved
                           && (year != 0 ? sy.Year_ID == year : true) && (courseId != 0 ? ct.course_id == courseId : true)
                           && (DivisionId != 0 ? bb.division_id == DivisionId : true) && (DistrictId != 0 ? bb.district_id == DistrictId : true)
                           && (InstituteId != 0 ? bb.iti_college_id == InstituteId : true) && (quarter != 0 ? sy.Quarter == quarter : true) && (bb.division_id == div_id)

                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Qualification = aa.Qualification,
                               //Tradename = tm.trade_name,
                               Type = st.Staff_Type,
                               Year = yr.Year,
                               MIScode = bb.MISCode,
                               //StatusName = sm.StatusName + " - " + rl.role_DescShortForm,
                               StatusName = (sm.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : ""),
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               IsActive = aa.IsActive,
                               Courseid = ct.course_id,
                               InstituteId = bb.iti_college_id,
                               YearId=sy.Year_ID,
                               DivisionId=di.division_id,
                               DistrictId=dt.district_lgd_code,
                               Quarter=sy.Quarter


                           }
                           ).ToList();
                res = res.GroupBy(a => a.InstituteId).Select(z => new StaffInstituteDetails
                {
                    InstituteId = z.Key,
                    InstituteName = z.Select(b => b.InstituteName).FirstOrDefault(),
                    Coursetype = z.Select(c => c.Coursetype).FirstOrDefault(),
                    MIScode = z.Select(c => c.MIScode).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    ApprovalFlowId = z.Select(c => c.ApprovalFlowId).FirstOrDefault(),
                    Courseid = z.Select(c => c.Courseid).FirstOrDefault(),
                    DivisionId = z.Select(c => c.DivisionId).FirstOrDefault(),
                    DistrictId = z.Select(c => c.DistrictId).FirstOrDefault(),
                    YearId = z.Select(c => c.YearId).FirstOrDefault(),
                    Quarter = z.Select(c => c.Quarter).FirstOrDefault(),
                    Year = z.Select(c => c.Year).FirstOrDefault(),
                }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<StaffInstituteDetails> GetApprovedstaffInstituteDivisionWise(int UserId)
        {
            try
            {
                int? div_id = _db.tbl_user_master.Where(a => a.um_id == UserId).Select(a => a.um_div_id).FirstOrDefault();
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                           equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                               //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                               //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into styp
                           from st in styp.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrs
                           from yr in yrs.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId
                           join rl in _db.tbl_role_master on sy.ApprovalFlowId equals rl.role_id into rm
                           from rl in rm.DefaultIfEmpty()
                           where sy.ApprovalStatus == (int)CsystemType.getCommon.Approved
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Qualification = aa.Qualification,
                               //Tradename = tm.trade_name,
                               Type = st.Staff_Type,
                               Year = yr.Year,
                               MIScode = bb.MISCode,
                               //StatusName = sm.StatusName + " - " + rl.role_DescShortForm,
                               StatusName = (sm.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : ""),
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               IsActive = aa.IsActive,
                               Courseid = ct.course_id,
                               InstituteId = bb.iti_college_id,
                               YearId = sy.Year_ID,
                               DivisionId = di.division_id,
                               DistrictId = dt.district_lgd_code,
                               Quarter=sy.Quarter


                           }
                           ).ToList();
                res = res.GroupBy(a => new { a.InstituteId, a.Quarter }).Select(z => new StaffInstituteDetails
                {
                    InstituteId = z.Key.InstituteId,
                    Quarter = z.Key.Quarter,
                    InstituteName = z.Select(b => b.InstituteName).FirstOrDefault(),
                    Coursetype = z.Select(c => c.Coursetype).FirstOrDefault(),
                    MIScode = z.Select(c => c.MIScode).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    ApprovalFlowId = z.Select(c => c.ApprovalFlowId).FirstOrDefault(),
                    Courseid = z.Select(c => c.Courseid).FirstOrDefault(),
                    DivisionId = z.Select(c => c.DivisionId).FirstOrDefault(),
                    DistrictId = z.Select(c => c.DistrictId).FirstOrDefault(),
                    YearId = z.Select(c => c.YearId).FirstOrDefault(),
                    Year = z.Select(c => c.Year).FirstOrDefault(),

                }).ToList();
                if (div_id != null)
                {
                    res = res.Where(a => a.DivisionId == div_id).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddStaffDetail(StaffInstituteDetails staff, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.Staff_Institute_Detail.Where(x => x.MobileNum == staff.MobileNum).FirstOrDefault();
                    //var yer = _db.Staff_Institute_Detail.Where(x => x.InstituteId == staff.InstituteId).FirstOrDefault();
                    //var inst = _db.Staff_Institute_Detail.Where(x => x.InstituteId == staff.InstituteId).ToList();
                    ////int collegeId = GetAffiliationInstituteIdDLL(loginId);
                    //if (staff.YearId != yer.Year_id)
                    //{
                    //    foreach (var item in inst)
                    //    {
                    //        var update = _db.Staff_Institute_Detail.Where(a => a.StaffId == item.StaffId).FirstOrDefault();
                    //        if (update != null)
                    //        {
                    //            update.Approvalstatus = 0;
                    //            _db.SaveChanges();
                    //        }
                    //        else
                    //        {

                    //        }
                    //    }

                    //}
                    if (res == null)
                    {

                        Staff_Institute_Detail staf = new Staff_Institute_Detail();
                        staf.Name = staff.Name;
                        staf.Designation = staff.Designation;
                        //staf.Trade = staff.Trade;
                       // staf.TechingSubject = staff.TechingSubject;
                        staf.MobileNum = staff.MobileNum;
                        staf.EmailId = staff.EmailId;
                        staf.InstituteId = staff.InstituteId;
                        staf.Qualification = staff.Qualification;
                        staf.IsActive = true;
                        staf.CreatedBy = loginId;
                        staf.StaffType = staff.StaffType;
                        staf.CreatedOn = DateTime.Now;
                        //staf.Year_id = staff.YearId;
                        staf.Gender_id = staff.GenderId;
                        staf.Coursetype_id = staff.Courseid;
                        staf.Other = staff.Other;
                        staf.CITS = staff.CITS;
                        staf.Photo = staff.Photo;
                        staf.TotalExperience = staff.TotalExperience;
                        _db.Staff_Institute_Detail.Add(staf);
                        _db.SaveChanges();

                        tbl_StaffYearWise_details tsyw = new tbl_StaffYearWise_details();
                        tsyw.Created_By= loginId;
                        tsyw.Created_On = staf.CreatedOn;
                        tsyw.InstituteId = staf.InstituteId;
                        //tsyw.Remarks = "";
                        tsyw.Staff_ID = staf.StaffId;
                        tsyw.Year_ID = staff.YearId;
                        tsyw.Quarter = staff.Quarter;
                        _db.tbl_StaffYearWise_details.Add(tsyw);
                        _db.SaveChanges();

                        if (staff.MultiSelectSubjectList != null)
                        {
                            foreach (var sub in staff.MultiSelectSubjectList)
                            {
                                tbl_staffsubject_details substaf = new tbl_staffsubject_details();
                                substaf.Subject_Id = sub;
                                substaf.Staff_Id = staf.StaffId;
                                substaf.Created_On = DateTime.Now;
                                substaf.IsActive = true;
                                _db.tbl_staffsubject_details.Add(substaf);
                                _db.SaveChanges();
                            }
                        }
                        if (staff.MultiSelectTradeList != null)
                        {
                            foreach (var trade in staff.MultiSelectTradeList)
                            {
                                tbl_stafftrade_details tradstaf = new tbl_stafftrade_details();
                                tradstaf.Trade_Id = trade;
                                tradstaf.Staff_Id = staf.StaffId;
                                tradstaf.Created_On = DateTime.Now;
                                tradstaf.IsActive = true;
                                _db.tbl_stafftrade_details.Add(tradstaf);
                                _db.SaveChanges();
                            }
                        }
                        //tbl_staffdetails_history his = new tbl_staffdetails_history();
                        //his.Institute_id = staf.InstituteId;
                        //his.ApprovalFlowId = 8;
                        //his.ApprovalStatus = 8;
                        //his.Remarks = staf.Remarks;
                        //his.Created_On = DateTime.Now;
                        //his.CreatedBy = staff.UserId;

                        //_db.tbl_staffdetails_history.Add(his);

                        //_db.SaveChanges();


                        transaction.Commit();
                        return "success";
                    }
                    else
                    {
                        return "exist";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                //transaction.Complete();
            }
        }

        public List<StaffInstituteDetails> GetstaffDetails(int loginId = 0)
        {
            #region  code merged into private method
            //try
            //{
            //    List<StaffInstituteDetails> res = new List<StaffInstituteDetails>();
            //    using (SqlConnection con = new SqlConnection(SQLConnection))
            //    {
            //        SqlDataAdapter da = new SqlDataAdapter();
            //        SqlCommand cmd = new SqlCommand(SPListOfSubjectTradeStaffWise, con);
            //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@InstituteId", loginId).SqlDbType = SqlDbType.Int;
            //        con.Open();
            //        da.SelectCommand = cmd;
            //        DataSet ds = new DataSet();
            //        da.Fill(ds);
            //        res = ds.Tables[0].AsEnumerable().Select(row => new StaffInstituteDetails
            //        {
            //            StaffId = row.Field<int>("StaffId"),
            //            Name = row.Field<string>("Name"),
            //            DesignationName = row.Field<string>("Designation"),
            //            MobileNum = row.Field<string>("MobileNum"),
            //            EmailId = row.Field<string>("EmailId"),
            //            Qualification = row.Field<string>("Qualification"),
            //            Tradename = row.Field<string>("trade_name"),
            //            subject = row.Field<string>("subject_name"),
            //            Type = row.Field<string>("Staff_Type"),
            //            ApprovalFlowId = row.Field<int?>("ApprovalFlowId"),
            //            Appeovalstatus = row.Field<int?>("Approvalstatus"),
            //            YearId = row.Field<int?>("Year_id"),
            //            Courseid = row.Field<int?>("Coursetype_id"),
            //            IsActive = row.Field<bool>("IsActive"),
            //            Year = row.Field<string>("year"),

            //        }).ToList();
            //    }
            //    return res;
            //    #region commented code
            //    // var res = (from aa in _db.Staff_Institute_Detail
            //    //            join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
            //    //            join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
            //    //            from di in dv.DefaultIfEmpty()
            //    //            join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
            //    //            from dt in dis.DefaultIfEmpty()
            //    //            join ct in _db.tbl_course_type_mast on aa.Coursetype_id equals ct.course_id into cst
            //    //            from ct in cst.DefaultIfEmpty()
            //    //            join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
            //    //            from cc in ds.DefaultIfEmpty()
            //    //            join sub in _db.tbl_staffsubject_details on aa.StaffId equals sub.Staff_Id into selsub
            //    //            from sub in selsub.DefaultIfEmpty()
            //    //            join trad in _db.tbl_stafftrade_details on aa.StaffId equals trad.Staff_Id into seltrad
            //    //            from trad in seltrad.DefaultIfEmpty()
            //    //            join tm in _db.tbl_trade_mast on trad.Trade_Id equals tm.trade_id into tr
            //    //            from tm in tr.DefaultIfEmpty()
            //    //            join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into stype
            //    //            from st in stype.DefaultIfEmpty()
            //    //            join sb in _db.tbl_subject on sub.Subject_Id equals sb.subject_id into tbsub
            //    //            from sb in tbsub.DefaultIfEmpty()
            //    //            join sm in _db.tbl_status_master on aa.Approvalstatus equals sm.StatusId into stm
            //    //            from sm in stm.DefaultIfEmpty()
            //    //            join rm in _db.tbl_role_master on aa.ApprovalFlowId equals rm.role_id into rl
            //    //            from rm in rl.DefaultIfEmpty()
            //    //            join yr in _db.tbl_Year on aa.Year_id equals yr.YearID into yrs
            //    //            from yr in yrs.DefaultIfEmpty()

            //    //            where aa.InstituteId == loginId
            //    //            select new StaffInstituteDetails
            //    //            {
            //    //                StaffId = aa.StaffId,
            //    //                Name = aa.Name,
            //    //                Designation = aa.Designation,
            //    //                MobileNum = aa.MobileNum,
            //    //                EmailId = aa.EmailId,
            //    //                InstituteName = bb.iti_college_name,
            //    //                Qualification = aa.Qualification,
            //    //                Tradename = tm.trade_name,
            //    //                Type = st.Staff_Type,
            //    //                subject = sb.subject_name,
            //    //                DesignationName = cc.Designation,
            //    //                MIScode = bb.MISCode,
            //    //                StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
            //    //                Division = di.division_name,
            //    //                District = dt.district_ename,
            //    //                Coursetype = ct.course_type_name,
            //    //                Courseid = ct.course_id,
            //    //                ApprovalFlowId = aa.ApprovalFlowId,
            //    //                IsActive = aa.IsActive,
            //    //                YearId=aa.Year_id,
            //    //                Year=yr.Year,
            //    //                Trade=trad.Trade_Id,
            //    //                TechingSubject=sub.Subject_Id,

            //    //            }
            //    //            ).OrderBy(a=>a.StaffId).ThenBy(a => a.TechingSubject).ToList();


            //    // int intialChk = 0;string ConsUb = null; int c  = 0;
            //    // foreach (var item in res)
            //    // {
            //    //     if (intialChk == 0)
            //    //     {
            //    //         ConsUb =  item.subject;
            //    //     }
            //    //     else if (item.StaffId != 0 && item.StaffId == c)
            //    //     {
            //    //         ConsUb += "," + item.subject;
            //    //     }        
            //    //     c = item.StaffId;
            //    //     item.Consoildatesubject = ConsUb;
            //    //     intialChk++;
            //    // }

            //    //var Finalres =  res.Select(a => a.StaffId).Distinct().ToList();
            //    // return res;
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
            return GetFromLinq(userID: loginId);
        }
        //public List<StaffInstituteDetails> StaffDetailsView()
        //{
        //    #region code merged into private method
        //    //try
        //    //{
        //    //    List<StaffInstituteDetails> res = new List<StaffInstituteDetails>();
        //    //    using (SqlConnection con = new SqlConnection(SQLConnection))
        //    //    {
        //    //        SqlDataAdapter da = new SqlDataAdapter();
        //    //        SqlCommand cmd = new SqlCommand(SPListOfSubjectTradeStaffWise, con);
        //    //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    //        cmd.Parameters.AddWithValue("@InstituteId", 0).SqlDbType = SqlDbType.Int;
        //    //        con.Open();
        //    //        da.SelectCommand = cmd;
        //    //        DataSet ds = new DataSet();
        //    //        da.Fill(ds);
        //    //        res = ds.Tables[0].AsEnumerable().Select(row => new StaffInstituteDetails
        //    //        {
        //    //            StaffId = row.Field<int>("StaffId"),
        //    //            Name = row.Field<string>("Name"),
        //    //            DesignationName = row.Field<string>("Designation"),
        //    //            MobileNum = row.Field<string>("MobileNum"),
        //    //            EmailId = row.Field<string>("EmailId"),
        //    //            Qualification = row.Field<string>("Qualification"),
        //    //            Tradename = row.Field<string>("trade_name"),
        //    //            subject = row.Field<string>("subject_name"),
        //    //            Type = row.Field<string>("Staff_Type"),
        //    //            ApprovalFlowId = row.Field<int?>("ApprovalFlowId"),
        //    //            Appeovalstatus = row.Field<int?>("Approvalstatus"),
        //    //            YearId = row.Field<int?>("Year_id"),
        //    //            Courseid = row.Field<int?>("Coursetype_id"),
        //    //            IsActive = row.Field<bool>("IsActive"),


        //    //        }).ToList();
        //    //    }
        //    //    return res;
        //    //    #region commented code
        //    //    //var res = (from aa in _db.Staff_Institute_Detail
        //    //    //           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
        //    //    //           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
        //    //    //           from di in dv.DefaultIfEmpty()
        //    //    //           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
        //    //    //           from dt in dis.DefaultIfEmpty()
        //    //    //           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
        //    //    //           from ct in cst.DefaultIfEmpty()
        //    //    //           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
        //    //    //           from cc in ds.DefaultIfEmpty()
        //    //    //           join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
        //    //    //           from tm in tr.DefaultIfEmpty()
        //    //    //           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id
        //    //    //           join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id into tsb
        //    //    //           from sb in tsb.DefaultIfEmpty()
        //    //    //           join sm in _db.tbl_status_master on aa.Approvalstatus equals sm.StatusId into stm
        //    //    //           from sm in stm.DefaultIfEmpty()
        //    //    //           join rm in _db.tbl_role_master on aa.ApprovalFlowId equals rm.role_id into rl
        //    //    //           from rm in rl.DefaultIfEmpty()

        //    //    //           select new StaffInstituteDetails
        //    //    //           {
        //    //    //               StaffId = aa.StaffId,
        //    //    //               Name = aa.Name,
        //    //    //               Designation = aa.Designation,
        //    //    //               MobileNum = aa.MobileNum,
        //    //    //               EmailId = aa.EmailId,
        //    //    //               InstituteName = bb.iti_college_name,
        //    //    //               Qualification = aa.Qualification,
        //    //    //               Tradename = tm.trade_name,
        //    //    //               Type = st.Staff_Type,
        //    //    //               subject = sb.subject_name,
        //    //    //               DesignationName = cc.Designation,
        //    //    //               MIScode = bb.MISCode,
        //    //    //               StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
        //    //    //               Division = di.division_name,
        //    //    //               District = dt.district_ename,
        //    //    //               Coursetype = ct.course_type_name,
        //    //    //               Courseid = aa.Coursetype_id,
        //    //    //               ApprovalFlowId = aa.ApprovalFlowId,
        //    //    //               IsActive = aa.IsActive,
        //    //    //               InstituteId=aa.InstituteId,
        //    //    //               YearId=aa.Year_id,
        //    //    //           }
        //    //    //           ).ToList();
        //    //    //return res;
        //    //    #endregion
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    #endregion
        //    return GetFromLinq();
        //}

        //public List<StaffInstituteDetails> ListstaffDetails(int loginId)
        //{
        //    try
        //    {
        //        var res = (from aa in _db.Staff_Institute_Detail
        //                   join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
        //                  equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
        //                   join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
        //                   join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
        //                   from di in dv.DefaultIfEmpty()
        //                   join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
        //                   from dt in dis.DefaultIfEmpty()
        //                   join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
        //                   from ct in cst.DefaultIfEmpty()
        //                   join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
        //                   from cc in ds.DefaultIfEmpty()
        //                   //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
        //                   //from tm in tr.DefaultIfEmpty()
        //                   join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id
        //                   //join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id into subt
        //                   //from sb in subt.DefaultIfEmpty()
        //                   join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId into stm
        //                   from sm in stm.DefaultIfEmpty()
        //                   join rm in _db.tbl_role_master on sy.ApprovalFlowId equals rm.role_id into rl
        //                   from rm in rl.DefaultIfEmpty()
        //                   join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
        //                   from yr in yrd.DefaultIfEmpty()
        //                   where aa.InstituteId == loginId
        //                   select new StaffInstituteDetails
        //                   {
        //                       StaffId = aa.StaffId,
        //                       Name = aa.Name,
        //                       Designation = aa.Designation,
        //                       MobileNum = aa.MobileNum,
        //                       EmailId = aa.EmailId,
        //                       InstituteName = bb.iti_college_name,
        //                       Qualification = aa.Qualification,
        //                       //Tradename = tm.trade_name,
        //                       Type = st.Staff_Type,
        //                       //subject = sb.subject_name,
        //                       DesignationName = cc.Designation,
        //                       MIScode = bb.MISCode,
        //                       StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
        //                       Division = di.division_name,
        //                       District = dt.district_ename,
        //                       Coursetype = ct.course_type_name,
        //                       Courseid = ct.course_id,
        //                       ApprovalFlowId = sy.ApprovalFlowId,
        //                       IsActive = aa.IsActive,
        //                       InstituteId = aa.InstituteId,
        //                       Year = yr.Year
        //                   }
        //                   ).Distinct().ToList();
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
       
        public StaffInstituteDetails EditStaff(int id)
        {
            try
            {
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details  on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                          equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           where aa.StaffId == id
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Qualification = aa.Qualification,
                               StaffType = aa.StaffType,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               //TechingSubject = aa.TechingSubject,
                               //Trade = aa.Trade,
                               YearId = sy.Year_ID,
                               GenderId = aa.Gender_id,
                               Courseid = aa.Coursetype_id,
                               CITS=aa.CITS,
                               TotalExperience=aa.TotalExperience,
                               Photo=aa.Photo,
                               Other=aa.Other
                           }
                      ).ToList();

                return res[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Updatestaff(StaffInstituteDetails staff, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var res = _db.Staff_Institute_Detail.Where(x => x.StaffId == staff.StaffId).SingleOrDefault();
                    var phon = _db.Staff_Institute_Detail.Where(x => x.MobileNum == staff.MobileNum).FirstOrDefault();
                    if (res != null)
                    {
                        res.Name = staff.Name;
                        res.Designation = staff.Designation;
                        //res.Trade = staff.Trade;
                        //res.TechingSubject = staff.TechingSubject;
                        if (phon == null)
                            res.MobileNum = staff.MobileNum;
                        res.EmailId = staff.EmailId;
                        res.Qualification = staff.Qualification;
                        res.Gender_id = staff.GenderId;
                        res.CITS = staff.CITS;
                        res.TotalExperience = staff.TotalExperience;
                        if(staff.Photo!=null)
                        {
                            res.Photo = staff.Photo;
                        }
                        res.Other = staff.Other;
                        if(staff.Courseid!=null)
                        {res.Coursetype_id = staff.Courseid;}
                        res.StaffType = staff.StaffType;
                        var staffYearWise = _db.tbl_StaffYearWise_details.Where(x => x.
                                  InstituteId == res.InstituteId && x.Staff_ID == res.StaffId).FirstOrDefault();
                        ////update year wise staff details
                        if (staffYearWise.ApprovalFlowId == 8)
                        {
                            staffYearWise.ApprovalFlowId = 8;
                            staffYearWise.ApprovalStatus = 8;
                        }

                        else
                        {

                        }
                        _db.SaveChanges();

                        if (staff.MultiSelectSubjectList != null)
                        {
                            var sub = _db.tbl_staffsubject_details.Where(x => x.Staff_Id == staff.StaffId).ToList();
                            //List<StaffInstituteDetails> subSelectredList = sub;

                            if (sub != null)
                            {
                                foreach (var item in sub)
                                {
                                    _db.tbl_staffsubject_details.Remove(item);
                                }
                                _db.SaveChanges();

                            }

                            foreach (var adsub in staff.MultiSelectSubjectList)
                            {
                                tbl_staffsubject_details substaf = new tbl_staffsubject_details();
                                substaf.Subject_Id = adsub;
                                substaf.Staff_Id = res.StaffId;
                                substaf.Created_On = DateTime.Now;
                                substaf.IsActive = true;
                                _db.tbl_staffsubject_details.Add(substaf);
                                _db.SaveChanges();
                            }
                        }
                        if (staff.MultiSelectTradeList != null)
                        {
                            var sub = _db.tbl_stafftrade_details.Where(x => x.Staff_Id == staff.StaffId).ToList();
                            //List<StaffInstituteDetails> subSelectredList = sub;

                            if (sub != null)
                            {
                                foreach (var trades in sub)
                                {
                                    _db.tbl_stafftrade_details.Remove(trades);
                                }
                                _db.SaveChanges();

                            }

                            foreach (var adsub in staff.MultiSelectTradeList)
                            {
                                tbl_stafftrade_details tradstaf = new tbl_stafftrade_details();
                                tradstaf.Trade_Id = adsub;
                                tradstaf.Staff_Id = res.StaffId;
                                tradstaf.Created_On = DateTime.Now;
                                tradstaf.IsActive = true;
                                _db.tbl_stafftrade_details.Add(tradstaf);
                                _db.SaveChanges();
                            }
                        }
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

                    throw ex;
                }
            }

        }

        public bool DeleteStaff(int id, string session = "")
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {

                    var res = _db.Staff_Institute_Detail.Where(x => x.StaffId == id).SingleOrDefault();
                    if (res != null)
                    {
                        res.IsActive = false;
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

        public List<StaffInstituteDetails> GetstaffStatus(int loginId, int Year, int courseId, int Quarter1)
        {
            try
            {
                int iniId = 0;
                int college_id = (loginId != 0 && iniId == 0) ? GetAffiliationInstituteIdDLL(loginId) : iniId;
                int? div_id = _db.tbl_user_master.Where(a => a.um_id == loginId).Select(a => a.um_div_id).FirstOrDefault();
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                          equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId into stm
                           from sm in stm.DefaultIfEmpty()
                           join rm in _db.tbl_role_master on sy.ApprovalFlowId equals rm.role_id into rl
                           from rm in rl.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
                           from yr in yrd.DefaultIfEmpty()
                           where college_id == 0 || aa.InstituteId == college_id
                           select new StaffInstituteDetails
                           {
                               InstituteName = bb.iti_college_name,
                               MIScode = bb.MISCode,
                               StatusName = (sm.StatusId != (int)CsystemType.getCommon.Approved && sm.StatusId != (int)CsystemType.getCommon.pub ? sm.StatusName + " - " + rm.role_DescShortForm : sm.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : sm.StatusName),
                               //StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                               //StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               Courseid = ct.course_id,
                               InstituteId = aa.InstituteId,
                               Year = yr.Year,
                               YearId = yr.YearID,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               DivisionId = di.division_id,
                               Quarter = sy.Quarter

                           }
                           ).Distinct().ToList();


                // where cc.UserId == loginId
                //where (Year != 0 ? sy.Year_ID == Year : true) && (courseId != 0 ? bb.CourseCode == courseId : true)
                //&& (Quarter1 != 0 ? sy.Quarter == Quarter1 : true) && bb.division_id == div_id

                res = res?.Where(a => a.StatusName.Length > 4 && a.DivisionId==div_id).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StaffInstituteDetails> GetstaffStatusForCW()
        {
            try
            {
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                          equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                           //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                           //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into styp
                           from st in styp.DefaultIfEmpty()
                           //join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id into tsub
                           //from sb in tsub.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId
                           join rl in _db.tbl_role_master on sy.ApprovalFlowId equals rl.role_id into rm
                           from rl in rm.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
                           from yr in yrd.DefaultIfEmpty()
                               // where cc.UserId == loginId
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Qualification = aa.Qualification,
                               //Tradename = tm.trade_name,
                               Type = st.Staff_Type,
                               //subject = sb.subject_name,
                               MIScode = bb.MISCode,
                               StatusName = sm.StatusName + " - " + rl.role_DescShortForm,
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               IsActive = aa.IsActive,
                               Courseid = ct.course_id,
                               InstituteId = bb.iti_college_id,
                               YearId = sy.Year_ID,
                               Year = yr.Year


                           }
                           ).ToList();
                res = res.GroupBy(a => a.InstituteId).Select(z => new StaffInstituteDetails
                {
                    InstituteId = z.Key,
                    InstituteName = z.Select(b => b.InstituteName).FirstOrDefault(),
                    Coursetype = z.Select(c => c.Coursetype).FirstOrDefault(),
                    MIScode = z.Select(c => c.MIScode).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    ApprovalFlowId = z.Select(c => c.ApprovalFlowId).FirstOrDefault(),
                    Year = z.Select(c => c.Year).FirstOrDefault(),

                }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StaffInstituteDetails> GetstaffStatusOSAD(int usrid, int Year, int courseId, int Quarter1)
        {
            try
            {
                int iniId = 0;
                int college_id = (usrid != 0 && iniId == 0) ? GetAffiliationInstituteIdDLL(usrid) : iniId;
                int? div_id = _db.tbl_user_master.Where(a => a.um_id == usrid).Select(a => a.um_div_id).FirstOrDefault();
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                          equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                           //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                           //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into styp
                           from st in styp.DefaultIfEmpty()
                           //join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id into tsub
                           //from sb in tsub.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId
                           join rl in _db.tbl_role_master on sy.ApprovalFlowId equals rl.role_id into rm
                           from rl in rm.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
                           from yr in yrd.DefaultIfEmpty()

                           where sy.ApprovalStatus != 8 && (Year != 0 ? sy.Year_ID == Year : true) && (courseId != 0 ? ct.course_id == courseId : true)
                           && (Quarter1 != 0 ? sy.Quarter == Quarter1 : true)  && di.division_id==div_id
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Qualification = aa.Qualification,
                               //Tradename = tm.trade_name,
                               Type = st.Staff_Type,
                               //subject = sb.subject_name,
                               MIScode = bb.MISCode,
                               StatusName = sm.StatusName + " - " + rl.role_DescShortForm,
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               DesignationName = cc.Designation,
                               IsActive = aa.IsActive,
                               Courseid = ct.course_id,
                               InstituteId = bb.iti_college_id,
                               YearId = sy.Year_ID,
                               Year = yr.Year,
                               CreatedOn=aa.CreatedOn,
                               Quarter=sy.Quarter


                           }
                           ).ToList();
                res = res.GroupBy(a => a.InstituteId).Select(z => new StaffInstituteDetails
                {
                    InstituteId = z.Key,
                    InstituteName = z.Select(b => b.InstituteName).FirstOrDefault(),
                    Coursetype = z.Select(c => c.Coursetype).FirstOrDefault(),
                    MIScode = z.Select(c => c.MIScode).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                    IsActive = z.Select(c => c.IsActive).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    ApprovalFlowId = z.Select(c => c.ApprovalFlowId).FirstOrDefault(),
                    Year = z.Select(c => c.Year).FirstOrDefault(),
                    YearId = z.Select(c => c.YearId).FirstOrDefault(),
                    Courseid = z.Select(c => c.Courseid).FirstOrDefault(),
                    CreatedOn = z.Select(c => c.CreatedOn).FirstOrDefault(),
                    Quarter = z.Select(c => c.Quarter).FirstOrDefault(),
                }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StaffInstituteDetails ViewStaff(int id)
        {
            try
            {
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                         equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                           //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                           //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id
                           //join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id
                           where aa.StaffId == id
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Qualification = aa.Qualification,
                               StaffType = aa.StaffType,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               //TechingSubject = aa.TechingSubject,
                               //Trade = aa.Trade,
                               DesignationName = cc.Designation,
                               //Tradename = tm.trade_name,
                               //subject = sb.subject_name,
                               Type = st.Staff_Type,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               Appeovalstatus = sy.ApprovalStatus
                           }
                      ).ToList();

                return res[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ApproveStaff(List<StaffInstituteDetails> staff, int loginId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int? yearos = 0;
                    int? qtros = 0;
                    if (staff.Count != 0)
                    {
                        //int collegeId = GetAffiliationInstituteIdDLL(loginId);
                        string remarkhis = "";
                        int? flowid = 0;
                        int? statusid = 0;
                        int College_id = 0;
                        var years = _db.tbl_Year.ToList();
                        foreach (var item in staff)
                        {


                            var res = _db.Staff_Institute_Detail.Where(x => x.MobileNum == item.MobileNum).FirstOrDefault();
                            College_id = res.InstituteId;
                            if (res != null)
                            {
                                var year = years?.Where(x => x.Year == item.Year).Select(i => i.YearID).FirstOrDefault();
                                var staffYearWise = _db.tbl_StaffYearWise_details.Where(x => x.
                                  InstituteId == College_id && x.Staff_ID == res.StaffId && x.Year_ID==year &&x.Quarter==item.Quarter).FirstOrDefault();
                                yearos = year;
                                qtros = item.Quarter;
                                if (item.Appeovalstatus == 4)
                                {
                                    staffYearWise.ApprovalFlowId = 9;
                                    staffYearWise.ApprovalStatus = item.Appeovalstatus;
                                    flowid = staffYearWise.ApprovalFlowId;
                                }
                                else
                                {
                                    staffYearWise.ApprovalFlowId = item.ApprovalFlowId;
                                    staffYearWise.ApprovalStatus = item.Appeovalstatus;
                                    flowid = item.ApprovalFlowId;

                                }
                                staffYearWise.Remarks = item.Remarks;
                                remarkhis = item.Remarks;

                                statusid = item.Appeovalstatus;
                                _db.SaveChanges();
                            }
                            else
                            {
                                return "exists";
                            }
                        }


                        tbl_staffdetails_history seattrans = new tbl_staffdetails_history();
                        seattrans.Institute_id = College_id;

                        seattrans.Remarks = remarkhis;
                        seattrans.Created_On = DateTime.Now;
                        seattrans.CreatedBy = loginId;
                        seattrans.ApprovalFlowId = flowid;
                        seattrans.ApprovalStatus = statusid;
                        seattrans.Yearid = yearos;
                        seattrans.Quarter = qtros;
                        _db.tbl_staffdetails_history.Add(seattrans);
                        _db.SaveChanges();

                        transaction.Commit();
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

        }

        public List<StaffInstituteDetails> GetAllstaffInstituteReport(int Year, int courseId, int divisionId,
            int districtId, int taluk, int Insttype, int location, int stafftype, int tradeId,
            int gender, int scheme, string training, int quarter)
        {
            try
            {
                var list = (from sd in _db.Staff_Institute_Detail
                            join sy in _db.tbl_StaffYearWise_details on new { x1 = sd.InstituteId, x2 = sd.StaffId }
                        equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                            join a in _db.tbl_iti_college_details on sd.InstituteId equals a.iti_college_id //into std
                            //from a in std.DefaultIfEmpty()
                            join dg in _db.tbl_designation_master on sd.Designation equals dg.Designation_Id into dsg
                            from dg in dsg.DefaultIfEmpty()
                            join st in _db.tbl_staff_type_master on sd.StaffType equals st.Staff_type_id into stf
                            from st in stf.DefaultIfEmpty()
                                //join sb in _db.tbl_subject on sd.TechingSubject equals sb.subject_id
                            //join j in _db.tbl_trade_mast on sd.Trade equals j.trade_id into jj
                            //from j in jj.DefaultIfEmpty()
                            join c in _db.tbl_district_master on a.district_id equals c.district_lgd_code into cc
                            from c in cc.DefaultIfEmpty()
                            join d in _db.tbl_taluk_master on a.taluk_id equals d.taluk_lgd_code into dd
                            from d in dd.DefaultIfEmpty()
                            join h in _db.tbl_location_type on a.location_id equals h.location_id into hh
                            from h in hh.DefaultIfEmpty()
                            join i in _db.tbl_Institute_type on a.Insitute_TypeId equals i.Institute_type_id into ii
                            from i in ii.DefaultIfEmpty()
                            join l in _db.tbl_division_master on a.division_id equals l.division_id into ll
                            from l in ll.DefaultIfEmpty()
                                //join m in _db.tbl_trade_sector on j.sector_id equals m.trade_sector_id into mm
                                //from m in mm.DefaultIfEmpty()
                            join n in _db.tbl_course_type_mast on sd.Coursetype_id equals n.course_id into nn
                            from n in nn.DefaultIfEmpty()
                            join o in _db.tbl_trade_scheme on a.Scheme equals o.ts_id into oo
                            from o in oo.DefaultIfEmpty()
                                //join p in _db.tbl_trade_type_mast on j.trade_type_id equals p.trade_type_id into pp
                                //from p in pp.DefaultIfEmpty()
                            join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
                            from yr in yrd.DefaultIfEmpty()
                            join gn in _db.tbl_Gender on sd.Gender_id equals gn.Gender_Id into gnd
                            from gn in gnd.DefaultIfEmpty()
                                //join p in products on c.Category equals p.Category into ps
                                //from p in ps.DefaultIfEmpty()
                                //select new { Category = c, ProductName = p == null ? "(No products)" : p.ProductName };
                            join tit in _db.tbl_ITI_Trade on sy.InstituteId equals tit.ITICode
                            join ts in _db.tbl_ITI_Trade_Shifts on tit.Trade_ITI_id equals ts.ITI_Trade_Id into trm
                            from ts in trm.DefaultIfEmpty()


                            where (Year != 0 ? yr.YearID == Year : true) && (courseId != 0 ? n.course_id == courseId : true)
                            && (divisionId != 0 ? l.division_id == divisionId : true) && (districtId != 0 ? c.district_lgd_code == districtId : true)
                            && (taluk != 0 ? d.taluk_lgd_code == taluk : true) && (Insttype != 0 ? i.Institute_type_id == Insttype : true)
                            && (location != 0 ? h.location_id == location : true) && (stafftype != 0 ? st.Staff_type_id == stafftype : true) /*&& (tradeId != 0 ? trade.Trade_ITI_id == tradeId : true)*/
                            && (gender != 0 ? gn.Gender_Id == gender : true) && (scheme != 0 ? o.ts_id == scheme : true)
                            && (training != "0" ? ts.Dual_System == training : true) && (quarter != 0 ? sy.Quarter == quarter : true)

                            select new StaffInstituteDetails
                            {
                                Coursetype = n.course_type_name,
                                MIScode = a.MISCode,
                                InstituteName = a.iti_college_name,
                                Division = l.division_name,
                                District = c.district_ename,
                                Taluk = d.taluk_ename,
                                Name = sd.Name,
                                DesignationName = dg.Designation,
                                Qualification = sd.Qualification,
                                //subject = sb.subject_name,
                                //Tradename = j.trade_name,
                                MobileNum = sd.MobileNum,
                                EmailId = sd.EmailId,
                                Courseid = sd.Coursetype_id,
                                DivisionId = l.division_id,
                                DistrictId = c.district_lgd_code,
                                scheme_Id = o.ts_id,
                                taluk_id = d.taluk_lgd_code,
                                StaffType = st.Staff_type_id,
                                Insttype_Id = i.Institute_type_id,
                                location_Id = h.location_id,
                                //Trade = j.trade_id,
                                YearId = sy.Year_ID ?? 0,
                                Year = yr.Year,
                                GenderId = gn.Gender_Id,
                                StaffId = sd.StaffId,
                                location_type = h.location_name,
                                Type = st.Staff_Type,
                                GenderName=gn.Gender,
                                Scheme=o.trade_scheme,
                                Quarter=sy.Quarter,
                                Photo=sd.Photo,
                                InstitutionType=i.Institute_type,
                                TrainingMode=ts.Dual_System





                            }).Distinct().ToList();
                list?.ForEach(x =>
                {
                    x.subject = string.Join(",",
                        (from ssd in _db.tbl_staffsubject_details
                         join sub in _db.tbl_subject on ssd.Subject_Id equals sub.subject_id
                         where ssd.Staff_Id == x.StaffId
                         select new { sub.subject_name }).ToList().Select(s => s.subject_name).ToArray() ?? new string[0] { });
                    x.Tradename = string.Join(",",
                        (from std in _db.tbl_stafftrade_details
                         join trd in _db.tbl_trade_mast on std.Trade_Id equals trd.trade_id
                         where std.Staff_Id == x.StaffId
                         select new { trd.trade_name }).ToList().Select(t => t.trade_name).ToArray() ?? new string[0] { });
                });
                //foreach (StaffInstituteDetails sfd in list ?? new List<StaffInstituteDetails>())
                //{
                //    var subject = (from ssd in _db.tbl_staffsubject_details
                //                   join sub in _db.tbl_subject on ssd.Subject_Id equals sub.subject_id
                //                   where ssd.Staff_Id == sfd.StaffId
                //                   select new { sub.subject_name }).ToList().Select(x => x.subject_name).ToArray();
                //    sfd.subject = string.Join(",", subject ?? new string[0] { });
                //    var trade = (from std in _db.tbl_stafftrade_details
                //                 join trd in _db.tbl_trade_mast on std.Trade_Id equals trd.trade_id
                //                 where std.Staff_Id == sfd.StaffId
                //                 select new { trd.trade_name }).ToList().Select(t => t.trade_name).ToArray();
                //    sfd.Tradename = string.Join(",", trade ?? new string[0] { });
                //}



                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StaffInstituteDetails> ViewStaffhistory(int id,int session,int quarter)
        {
            try
            {
                var res = (from aa in _db.tbl_staffdetails_history
                           join tm in _db.tbl_role_master on aa.ApprovalFlowId equals tm.role_id into tr
                           from tm in tr.DefaultIfEmpty()
                           join rm in _db.tbl_role_master on aa.CreatedBy equals rm.role_id into trm
                           from rm in trm.DefaultIfEmpty()
                           where aa.Institute_id == id  && aa.Quarter==quarter && aa.Yearid==session
                           select new StaffInstituteDetails
                           {
                               InstituteId = aa.Institute_id,

                               Remarks = aa.Remarks,
                               From = rm.role_description,
                               To = tm.role_description??"-",
                               CreatedOn = aa.Created_On,
                               ApprovalFlowId = aa.ApprovalFlowId,
                               Appeovalstatus = aa.ApprovalStatus,
                               Date = aa.Created_On.ToString(),
                           }
                      ).ToList();
                //if (session != "")
                //    res = res?.Where(x => x.CreatedOn.Value.Year.ToString() == session).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SubmitStaffDetails(List<StaffInstituteDetails> staff, int loginId, int roleId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (staff.Count != 0)
                    {
                        int collegeId = GetAffiliationInstituteIdDLL(loginId);
                        var years = _db.tbl_Year.ToList();
                        string remarkhis = "";
                        int? yearhst = 0;
                        int? qtrhst = 0;
                        foreach (var item in staff)
                        {

                            var year = years?.Where(x => x.Year == item.Year).Select(i => i.YearID).FirstOrDefault();
                            var res = _db.Staff_Institute_Detail.Where(x => x.MobileNum == item.MobileNum &&
                            x.InstituteId == collegeId).FirstOrDefault();
                            yearhst = year;
                            qtrhst = item.Quarter;
                            if (res != null)
                            {
                                var staffYearWise = _db.tbl_StaffYearWise_details.Where(x => 
                                x.InstituteId == res.InstituteId && x.Staff_ID == res.StaffId &&
                                 x.Year_ID == year.Value && x.Quarter == item.Quarter).FirstOrDefault();
                                if (staffYearWise != null)
                                {
                                    staffYearWise.ApprovalFlowId = 17;
                                    staffYearWise.ApprovalStatus = 8;
                                    staffYearWise.Remarks = item.Remarks;
                                    remarkhis = item.Remarks;

                                }
                                else
                                {
                                    tbl_StaffYearWise_details tbl_StaffYearWise_detail = new tbl_StaffYearWise_details
                                    {
                                        InstituteId = collegeId,
                                        Year_ID = year,
                                        Staff_ID = res.StaffId,
                                        Created_By = loginId
                                    };
                                    _db.tbl_StaffYearWise_details.Add(tbl_StaffYearWise_detail);
                                }
                                
                            }
                            else
                            {
                                return "exists";
                            }
                        }
                        //_db.SaveChanges();

                        tbl_staffdetails_history seattrans = new tbl_staffdetails_history();
                        seattrans.Institute_id = collegeId;

                        seattrans.Remarks = remarkhis;
                        seattrans.Created_On = DateTime.Now;
                        seattrans.CreatedBy = roleId;
                        seattrans.ApprovalFlowId = 17;
                        seattrans.ApprovalStatus = 8;
                        seattrans.Yearid = yearhst;
                        seattrans.Quarter = qtrhst;

                        _db.tbl_staffdetails_history.Add(seattrans);
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

        public List<int> GetStaffSubjectList(int id)
        {
            try
            {
                var result = _db.tbl_staffsubject_details.Where(aa => aa.Staff_Id == id && aa.IsActive == true).Select(bb => bb.Subject_Id).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StaffInstituteDetails> GetstaffSubjects(int loginId)
        {
            try
            {
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                       equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join cc in _db.tbl_designation_master on aa.Designation equals cc.Designation_Id into ds
                           from cc in ds.DefaultIfEmpty()
                           //join tm in _db.tbl_trade_mast on aa.Trade equals tm.trade_id into tr
                           //from tm in tr.DefaultIfEmpty()
                           join st in _db.tbl_staff_type_master on aa.StaffType equals st.Staff_type_id into stype
                           from st in stype.DefaultIfEmpty()
                           //join sb in _db.tbl_subject on aa.TechingSubject equals sb.subject_id into tbsub
                           //from sb in tbsub.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId into stm
                           from sm in stm.DefaultIfEmpty()
                           join rm in _db.tbl_role_master on sy.ApprovalFlowId equals rm.role_id into rl
                           from rm in rl.DefaultIfEmpty()
                           where aa.InstituteId == loginId
                           select new StaffInstituteDetails
                           {
                               StaffId = aa.StaffId,
                               Name = aa.Name,
                               Designation = aa.Designation,
                               MobileNum = aa.MobileNum,
                               EmailId = aa.EmailId,
                               InstituteName = bb.iti_college_name,
                               Qualification = aa.Qualification,
                               //Tradename = tm.trade_name,
                               Type = st.Staff_Type,
                               //subject = sb.subject_name,
                               DesignationName = cc.Designation,
                               MIScode = bb.MISCode,
                               StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               Courseid = ct.course_id,
                               ApprovalFlowId = sy.ApprovalFlowId,
                               IsActive = aa.IsActive,
                           }
                           ).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetStafTradeList(int id)
        {
            try
            {
                var result = _db.tbl_stafftrade_details.Where(aa => aa.Staff_Id == id && aa.IsActive == true).
                    Select(bb => bb.Trade_Id).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region code refactor
        public List<StaffInstituteDetails> GetStaffDetailsSessionWise(int userId = 0, string session = "", int? iniId = 0)
        {
            return GetFromLinq(userID:userId,session: session, insId:iniId);
        }
        public List<StaffInstituteDetails> ListstaffDetails(int? loginId=0,int? iniId=0)
        {
            try
            {
                int college_id =(loginId != 0 && iniId == 0) ? GetAffiliationInstituteIdDLL(loginId.Value) : iniId.Value;
                int? div_id = _db.tbl_user_master.Where(a => a.um_id == loginId).Select(a=>a.um_div_id).FirstOrDefault();
                var res = (from aa in _db.Staff_Institute_Detail
                           join sy in _db.tbl_StaffYearWise_details on new { x1 = aa.InstituteId, x2 = aa.StaffId }
                          equals new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                           join bb in _db.tbl_iti_college_details on aa.InstituteId equals bb.iti_college_id
                           join di in _db.tbl_division_master on bb.division_id equals di.division_id into dv
                           from di in dv.DefaultIfEmpty()
                           join dt in _db.tbl_district_master on bb.district_id equals dt.district_lgd_code into dis
                           from dt in dis.DefaultIfEmpty()
                           join ct in _db.tbl_course_type_mast on bb.CourseCode equals ct.course_id into cst
                           from ct in cst.DefaultIfEmpty()
                           join sm in _db.tbl_status_master on sy.ApprovalStatus equals sm.StatusId into stm
                           from sm in stm.DefaultIfEmpty()
                           join rm in _db.tbl_role_master on sy.ApprovalFlowId equals rm.role_id into rl
                           from rm in rl.DefaultIfEmpty()
                           join yr in _db.tbl_Year on sy.Year_ID equals yr.YearID into yrd
                           from yr in yrd.DefaultIfEmpty()
                           where college_id==0|| aa.InstituteId == college_id
                           select new StaffInstituteDetails
                           {
                               InstituteName = bb.iti_college_name,
                               MIScode = bb.MISCode,
                               StatusName = (sm.StatusId != (int)CsystemType.getCommon.Approved /*&& sm.StatusId != (int)CsystemType.getCommon.pub */? sm.StatusName + " - " + rm.role_DescShortForm : sm.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : sm.StatusName),
                               //StatusName = (Statusmaster.StatusId != (int)CsystemType.getCommon.Approved && Statusmaster.StatusId != (int)CsystemType.getCommon.pub ? Statusmaster.StatusName + " - " + rl.role_DescShortForm : Statusmaster.StatusId == (int)CsystemType.getCommon.Approved ? "Approved" : Statusmaster.StatusName),
                               //StatusName = sm.StatusName + "-" + rm.role_DescShortForm,
                               Division = di.division_name,
                               District = dt.district_ename,
                               Coursetype = ct.course_type_name,
                               Courseid = ct.course_id,
                               InstituteId = aa.InstituteId,
                               Year = yr.Year,
                               YearId=yr.YearID,
                               ApprovalFlowId=sy.ApprovalFlowId,
                               DivisionId=di.division_id,
                               Quarter=sy.Quarter,
                               Photo=aa.Photo,
                               CreatedOn=sy.Created_On,
                               Appeovalstatus=sy.ApprovalStatus
                               
                           }
                           ).Distinct().ToList();
                res = res.GroupBy(a => new { a.InstituteId, a.Quarter }).Select(z => new StaffInstituteDetails
                {
                    InstituteId = z.Key.InstituteId,
                    Quarter = z.Key.Quarter,
                    InstituteName = z.Select(b => b.InstituteName).FirstOrDefault(),
                    Coursetype = z.Select(c => c.Coursetype).FirstOrDefault(),
                    MIScode = z.Select(c => c.MIScode).FirstOrDefault(),
                    StatusName = z.Select(c => c.StatusName).FirstOrDefault(),
                   CreatedOn= z.Select(c => c.CreatedOn).FirstOrDefault(),
                    Division = z.Select(c => c.Division).FirstOrDefault(),
                    District = z.Select(c => c.District).FirstOrDefault(),
                    ApprovalFlowId = z.Select(c => c.ApprovalFlowId).FirstOrDefault(),
                    Courseid = z.Select(c => c.Courseid).FirstOrDefault(),
                    DivisionId = z.Select(c => c.DivisionId).FirstOrDefault(),
                   
                    YearId = z.Select(c => c.YearId).FirstOrDefault(),
                    Year = z.Select(c => c.Year).FirstOrDefault(),
                    Appeovalstatus = z.Select(c => c.Appeovalstatus).FirstOrDefault(),

                }).ToList();
                if (div_id!=null)
                {
                    res = res.Where(a => a.DivisionId == div_id).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion
        #region private methods
        private List<StaffInstituteDetails> GetFromStoredProcedure(int insId = 0)
        {
            try
            {
                List<StaffInstituteDetails> res = new List<StaffInstituteDetails>();
                using (SqlConnection con = new SqlConnection(SQLConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand(SPListOfSubjectTradeStaffWise, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InstituteId", insId).SqlDbType = SqlDbType.Int;
                    con.Open();
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    res = ds.Tables[0].AsEnumerable().Select(row => new StaffInstituteDetails
                    {
                        StaffId = row.Field<int>("StaffId"),
                        Name = row.Field<string>("Name"),
                        DesignationName = row.Field<string>("Designation"),
                        MobileNum = row.Field<string>("MobileNum"),
                        EmailId = row.Field<string>("EmailId"),
                        Qualification = row.Field<string>("Qualification"),
                        Tradename = row.Field<string>("trade_name"),
                        subject = row.Field<string>("subject_name"),
                        Type = row.Field<string>("Staff_Type"),
                        ApprovalFlowId = row.Field<int?>("ApprovalFlowId"),
                        Appeovalstatus = row.Field<int?>("Approvalstatus"),
                        YearId = row.Field<int?>("Year_id"),
                        Courseid = row.Field<int?>("Coursetype_id"),
                        IsActive = row.Field<bool>("IsActive"),
                        Year = row.Field<string>("year"),

                    }).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<StaffInstituteDetails> GetFromLinq(int userID = 0, string session = "", int? insId = 0)
        {
            try
            {
                int college_id = insId == 0 ? GetAffiliationInstituteIdDLL(userID) : insId.Value;
                List<StaffInstituteDetails> res = new List<StaffInstituteDetails>();
                res = (from s in _db.Staff_Institute_Detail where userID == 0 || s.InstituteId== college_id
                       join d in _db.tbl_designation_master on s.Designation equals d.Designation_Id
                       join st in _db.tbl_staff_type_master on s.StaffType equals st.Staff_type_id
                       join sy in _db.tbl_StaffYearWise_details on new { x1 = s.InstituteId, x2 = s.StaffId } equals
                       new { x1 = sy.InstituteId, x2 = sy.Staff_ID }
                       join y in _db.tbl_Year on sy.Year_ID equals y.YearID 
                       select new StaffInstituteDetails
                       {
                           StaffId = s.StaffId,
                           Name = s.Name,
                           DesignationName = d.Designation,
                           MobileNum = s.MobileNum,
                           EmailId = s.EmailId,
                           Qualification = s.Qualification,
                           //Tradename = row.Field<string>("trade_name"),
                           //subject = row.Field<string>("subject_name"),
                           Type = st.Staff_Type != "Others" ? st.Staff_Type : s.Other,
                           ApprovalFlowId = sy.ApprovalFlowId,
                           Appeovalstatus = sy.ApprovalStatus,
                           YearId = sy.Year_ID,
                           Courseid = s.Coursetype_id,
                           IsActive = s.IsActive,
                           Year = y.Year,
                           Quarter=sy.Quarter,
                           Photo=s.Photo,
                           //Other=s.Other


                       }).ToList();
                //if (insId != 0)
                //    res = res?.Where(i => i.InstituteId == insId).ToList();
                if (session != "")
                {
                    //check current session is null, get previous session active staff
                    var sessionStaff= res?.Where(i => i.Quarter == Convert.ToInt32(session)).ToList();
                    res = (sessionStaff == null || sessionStaff.Count == 0) ? 
                        res?.Where(i => i.Quarter == Int32.Parse(session)-1 && i.IsActive == true).ToList()
                        : sessionStaff;
                    //if session staff null then take previous year data and make status null
                    if (sessionStaff == null || sessionStaff.Count == 0)
                    {
                        res?.ForEach(x =>
                        {
                            x.ApprovalFlowId = null;
                            x.Appeovalstatus = null;
                            //  x.YearId = ++x.YearId; //hardcode way, need to check session id in tbl_year table
                            x.YearId = x.YearId;
                            //x.Year = session;
                            x.Year = x.Year;
                            x.Quarter = ++x.Quarter;//hardcode way, need to check quarter  in tbl_StaffYearWise_detail table
                            //insert into tbl_StaffYearWise_details table
                            var tbl_StaffYearWise_detail = new tbl_StaffYearWise_details
                            {
                                InstituteId = college_id,//Convert.ToInt32(insId),
                                Staff_ID = x.StaffId.Value,
                                Year_ID = x.YearId,
                                Created_On = DateTime.Now,
                                Created_By = userID,
                                Quarter=x.Quarter
                            };
                            _db.tbl_StaffYearWise_details.Add(tbl_StaffYearWise_detail);
                        });
                        _db.SaveChanges();
                    }
                }
              
                res?.ForEach(x =>
                {
                    x.subject = string.Join(",",
                        (from ssd in _db.tbl_staffsubject_details
                         join sub in _db.tbl_subject on ssd.Subject_Id equals sub.subject_id
                         where ssd.Staff_Id == x.StaffId
                         select new { sub.subject_name }).ToList().Select(s => s.subject_name).ToArray() ?? new string[0] { });
                    x.Tradename = string.Join(",",
                        (from std in _db.tbl_stafftrade_details
                         join trd in _db.tbl_trade_mast on std.Trade_Id equals trd.trade_id
                         where std.Staff_Id == x.StaffId
                         select new { trd.trade_name }).ToList().Select(t => t.trade_name).ToArray() ?? new string[0] { });
                });
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public int GetUserDivionIdDLL(int LoginId)
        {
            int res = 0;
            try
            {
                res = _db.tbl_user_master.Where(a => a.um_id == LoginId).Select(b => (int)b.um_div_id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        #endregion
    }
}
