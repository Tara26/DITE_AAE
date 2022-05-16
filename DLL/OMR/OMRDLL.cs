using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.OMR;
using System.Web.Mvc;
using DLL.DBConnection;
using System.Configuration;
using Models;
using Models.Master;

namespace DLL.OMR
{
    public class OMRDLL : IOMRDLL
    {
        private readonly DbConnection _db = new DbConnection();
        string SQLConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public SelectList GetDivisionListDLL()
        {
            List<SelectListItem> DivisinList = _db.tbl_division_mast.Select(x => new SelectListItem
            {
                Text = x.division_name.ToString(),
                Value = x.division_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Division",

            };
            DivisinList.Insert(0, ProposalList);
            return new SelectList(DivisinList, "Value", "Text");
        }
        public SelectList GetCenterListDLL()
        {
            List<SelectListItem> CntrList = _db.tbl_exam_centers.Select(x => new SelectListItem
            {
                Text = x.ec_name.ToString(),
                Value = x.ec_id.ToString()
            }).ToList();
            var ProposalList = new SelectListItem()
            {

                Value = null,
                Text = "Select Course Type",

            };
            CntrList.Insert(0, ProposalList);
            return new SelectList(CntrList, "Value", "Text");
        }

        public SelectList GetSubjectListDLL()
        {
            List<SelectListItem> SubjectList = _db.tbl_exam_subject_mast.Select(x => new SelectListItem
            {
                Text = x.exam_subject_desc.ToString(),
                Value = x.exam_subject_id.ToString()
            }).ToList();
            var ExamTList = new SelectListItem()
            {

                Value = null,
                Text = "Select Subject",

            };
            SubjectList.Insert(0, ExamTList);
            return new SelectList(SubjectList, "Value", "Text");
        }

        public List<OMRdtls> getSubjectDtlsDLL(int subjectId)
        {
            var det = (from n in _db.tbl_exam_subject_trans
                       join d in _db.tbl_exam_subject_type_mast on n.est_exam_subject_type_id equals d.exam_subject_type_id
                       join ty in _db.tbl_trade_year_mast on n.est_trade_year_id equals ty.trade_year_id
                       join s in _db.tbl_exam_subject_mast on n.est_exam_subject_id equals s.exam_subject_id.ToString()

                       where n.est_exam_subject_id == subjectId.ToString()
                       select new OMRdtls
                       {
                           Day = "Monday",// ((DateTime)n.est_exam_date).ToString("dddd"),
                           ECT_ExamDate = n.est_exam_date,
                           ECT_Exam_Start_Time = n.est_exam_start_date,
                           ECT_Exam_End_Time = n.exam_end_date,
                           TradeYr = ty.trade_year_name,
                           Subject = d.exam_subject_type_desc,
                           BlockNo = 1.ToString(),
                       }).ToList();
            return det;
        }

        public SelectList GetCenterListDLL(int? DivId)
        {
            List<SelectListItem> _ListItems = new List<SelectListItem>();
            _ListItems = (from C in _db.tbl_exam_centers.AsEnumerable()
                          join d in _db.tbl_division_mast on C.division_id equals d.division_id
                          where d.division_id == DivId
                          select new SelectListItem
                          {
                              Text = C.ec_name,
                              Value = C.ec_id.ToString()
                              //Value=PD.proj_dtls_id.ToString(),
                          }).ToList();




            return new SelectList(_ListItems, "Value", "Text");
        }


        public string CreateOMRDetailsDLL(List<OMRdtls> models)
        {
            string returnValue = "saved";
            using (var transaction = _db.Database.BeginTransaction())
            {
                foreach (var model in models)
                {
                    try
                    {
                        // UploadAttendanceDocuments(model);
                        bool isActive = false;
                        int blockNo = Convert.ToInt32(model.BlockNo);
                        tbl_block_or_room block_Or_Room = _db.tbl_block_or_room.Where(x => x.block_id == blockNo).FirstOrDefault();
                        if (block_Or_Room == null)
                        {
                            block_Or_Room = new tbl_block_or_room();
                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;
                            _db.tbl_block_or_room.Add(block_Or_Room);
                            _db.SaveChanges();

                        }
                        else
                        {

                            block_Or_Room.room_number = model.BlockNo.ToString();
                            block_Or_Room.upload_pdf_file = model.attendSavePath;
                            block_Or_Room.upload_excel_file = model.attendExcelPath;
                            block_Or_Room.is_active = isActive;
                            block_Or_Room.creation_datetime = DateTime.Now;
                            block_Or_Room.created_by = 1;
                            //block_Or_Room.login_id = model.login_id;

                            _db.SaveChanges();
                        }


                        tbl_omr_details_mast omrDtls = _db.tbl_omr_details_mast.Where(x => x.omr_block_id == blockNo).FirstOrDefault();
                        {
                            if (omrDtls == null)
                            {
                                omrDtls.omr_division_id = model.divId;
                                omrDtls.omr_exam_centre_id = model.ExamCntrId;
                                omrDtls.omr_exam_calendar_id = model.ExamCntrId;
                                omrDtls.omr_is_active = isActive;
                                omrDtls.omr_created_by = 1;
                                omrDtls.omr_creation_datetime = DateTime.Now;
                                //block_Or_Room.login_id = model.login_id;
                                _db.tbl_omr_details_mast.Add(omrDtls);
                                _db.SaveChanges();


                            }
                            else
                            {
                                omrDtls.omr_division_id = model.divId;
                                omrDtls.omr_exam_centre_id = model.ExamCntrId;
                                omrDtls.omr_exam_calendar_id = model.ExamCntrId;
                                omrDtls.omr_is_active = isActive;
                                omrDtls.omr_updated_by = 1;
                                omrDtls.omr_updation_datetime = DateTime.Now;
                                //block_Or_Room.login_id = model.login_id;

                                _db.SaveChanges();
                            }
                        }



                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        returnValue = "Failed";
                    }
                }

            }
            return returnValue;
        }
    }
}

