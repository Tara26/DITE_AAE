using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Models.OMR;
using BLL.OMR;
using Newtonsoft.Json;

namespace DIT.Controllers
{
    public class OMRSheetController : Controller
    {

        private readonly IOMRBLL _omrBll;
        private readonly string UploadFolder = ConfigurationManager.AppSettings["TemplateDocumentsPath"];

        // strings to generate file name
        private readonly string PdfFileNameFormat = "InvigilatorDairy{0}.pdf";
        private readonly string ExcelFileNameFormat = "InvigilatorDairy{0}.pdf";
        // GET: Attendance

        public OMRSheetController()
        {
            _omrBll = new OMRBLL();
        }
        // GET: OMRSheet
        public ActionResult Index()
        {
            return View();
        }

        // GET: OMRSheet
        [HttpGet]
        public ActionResult CreateOMRdtls()
        {
            OMRdtls model = new OMRdtls();
            model.User_Id = Convert.ToInt32(Session["LoginId"]);
            model.divisionList = _omrBll.GetDivisionListBLL();
            model.ExamCentrList = _omrBll.GetCenterListBLL();
            model.SubjectList = _omrBll.GetSubjectListBLL();
            return View(model);
        }

        //[HttpPost]
        //public ActionResult CreateOMRdtls()
        //{
        //    return View();
        //}

        [HttpPost]
        public JsonResult saveExamattendanceDetails(FormCollection formCollection)
        {
            // get the files from view
            HttpFileCollectionBase files = Request.Files;
            List<OMRdtls> models = new List<OMRdtls>();

            //AtendanceDet models = new AtendanceDet();


            // get all the row data
            foreach (var key in formCollection.AllKeys)
            {
                //Deserialize JSON to class
                OMRdtls model = JsonConvert.DeserializeObject<OMRdtls>(formCollection[key]);
                model.User_Id = Convert.ToInt32(Session["LoginId"]);
                for (int index = 0; index < files.Count; index++)
                {
                    // assign file to be uploaded
                    if (model.attendSavePath == files[index].FileName)
                    {
                        model.UploadPdf = files[index];
                    }
                    else if (model.attendExcelPath == files[index].FileName)
                    {
                        model.UploadXcel = files[index];
                    }
                }
                models.Add(model);
            }

            var res = _omrBll.CreateOMRDetailsBLL(models);
            //  var res = _attendBll.ExamAttendanceUploadBLL(models);
            return Json(res, JsonRequestBehavior.AllowGet);

        }


        public JsonResult getSubjectDtls(int subjectId)
        {
            OMRdtls model = new OMRdtls();

            model.User_Id = Convert.ToInt32(Session["LoginId"]);

            var res = _omrBll.getSubjectDtlsBLL(subjectId);
            //  sb.Append("< tr >< td > " ' + slNo + " </ td >< td > " + attentdance.Day + " </ td >< td > " + dateFormat(new Date(parseInt((attentdance.ECT_ExamDate).match(/\d+/)[0]))) + " </ td >< td > " + attentdance.padStart + " </ td >< td > " + attentdance.Day+ " </ td >< td > " + attentdance.Day + " </ td >< td > " + attentdance.Day + " </ td ></ tr > ";")
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CenterListBasedOnDivId(int DivId)
        {
            OMRdtls model = new OMRdtls();
            var res = _omrBll.GetCenterListBLL(DivId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}
