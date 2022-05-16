using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Admission
{
    public class GrievanceDocApplData
    {
        public int ApplicantId { get; set; }
        public decimal MaxMarks { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MarksObtained { get; set; }
        public decimal Percentage { get; set; }
        public int? ResultQual { get; set; }
        public bool? HyderabadKarnatakaRegion { get; set; }
        public List<int> ApplicableReservations { get; set; }
        public string FathersName { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public int? Category { get; set; }
        public int? ApplicantBelongTo { get; set; }
        public int? Caste { get; set; }

        public bool PhysicallyHanidcapInd { get; set; }
        public int? PhysicallyHanidcapType { get; set; }

        public HttpPostedFileBase EduCertificatePDF { get; set; }
        public HttpPostedFileBase CasteCertificatePDF { get; set; }
        public HttpPostedFileBase UIDNumberPDF { get; set; }
        public HttpPostedFileBase RuralPDF { get; set; }
        public HttpPostedFileBase DifferentlyAbledPDF { get; set; }
        public HttpPostedFileBase HyderabadKarnatakaRegionPDF { get; set; }
        public HttpPostedFileBase ExservicemanPDF { get; set; }
        public HttpPostedFileBase EWSCertificatePDF { get; set; }

        public string EduCertificateRemarks { get; set; }
        public string CasteCertificateRemarks { get; set; }
        public string UIDNumberRemarks { get; set; }
        public string RuralRemarks { get; set; }
        public string DifferentlyAbledRemarks { get; set; }
        public string HyderabadKarnatakaRegionRemarks { get; set; }
        public string ExservicemanRemarks { get; set; }
        public string EWSCertificateRemarks { get; set; }

        public int EDocAppId { get; set; }
        public int CDocAppId { get; set; }
        public int RUDocAppId { get; set; }
        public int RDocAppId { get; set; }
        public int IDocAppId { get; set; }
        public int UDocAppId { get; set; }
        public int KDocAppId { get; set; }
        public int DDocAppId { get; set; }
        public int ExDocAppId { get; set; }
        public int HDocAppId { get; set; }
        public int HGKDocAppId { get; set; }
        public int ODocAppId { get; set; }
        public int LLDocAppId { get; set; }
        public int ExSDocAppId { get; set; }
        public int KMDocAppId { get; set; }
        public int EWSDocAppId { get; set; }

        public int ECreatedBy { get; set; }
        public int CCreatedBy { get; set; }
        public int RCreatedBy { get; set; }
        public int ICreatedBy { get; set; }
        public int UCreatedBy { get; set; }
        public int RUCreatedBy { get; set; }
        public int KCreatedBy { get; set; }
        public int DCreatedBy { get; set; }
        public int ExCreatedBy { get; set; }
        public int HCreatedBy { get; set; }
        public int HGKCreatedBy { get; set; }
        public int OCreatedBy { get; set; }
        public int KMCreatedBy { get; set; }
        public int ExSCreatedBy { get; set; }
        public int LLCreatedBy { get; set; }
        public int EWSCreatedBy { get; set; }
        public string Remarks { get; set; }

        public int EduDocStatus { get; set; }
        public int CasDocStatus { get; set; }
        public int UIDDocStatus { get; set; }
        public int RUcerDocStatus { get; set; }
        public int KanMedCerDocStatus { get; set; }
        public int DiffAblDocStatus { get; set; }
        public int ExCerDocStatus { get; set; }
        public int HyKarDocStatus { get; set; }
        public int ExserDocStatus { get; set; }
        public int EWSDocStatus { get; set; }

        public int CreatedBy { get; set; }
        public int Verified { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int DocumentTypeId { get; set; }
        public int DocAppId { get; set; }
        public int UpdatedBy { get; set; }
        public string DocumentRemarks { get; set; }
        public int UploadedByVerfication { get; set; }
        public List<int> DocumentSet { get; set; }
        public int DocumentSetInd { get; set; }
        public string UpdateMsg { get; set; }
        public string ErrorOccuredMsg { get; set; }
        public int ErrorOccuredInd { get; set; }
        public int LoginId { get; set; }

    }
}
