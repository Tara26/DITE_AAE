using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.Admission
{
    public class ApplicantApplicationForm
    {
        public List<ApplicationForm> GetReligionList { get; set; }
        public List<GrievanceDocApplData> GrievanceDocApplData { get; set; }
        public List<ApplicationForm> GetGenderList { get; set; }
        public List<ApplicationForm> GetCategoryList { get; set; }
        public List<ApplicationForm> GetDocumentApplicationStatus { get; set; }
        public List<ApplicationForm> GetApplicantTypeList { get; set; }
        public List<ApplicationForm> GetReservationList { get; set; }
        public List<ApplicationForm> GetQualificationList { get; set; }
        public List<ApplicationForm> GetDistrictList { get; set; }
        public List<ApplicationForm> GetOtherBoards { get; set; }
        public List<ApplicationForm> GetCasteList { get; set; }
        public List<PersonWithDisabilityCategory> PersonWithDisabilityCategory { get; set; }
        public List<ApplicantDocumentsDetail> GetApplicantDocumentsDetail { get; set; }
        public List<ApplicantInstitutePreference> GetApplicantInstitutePreference { get; set; }
        public List<ApplicationForm> GetApplicantDocVerfiInstitutePreference { get; set; }
        public List<ApplicationForm> GetApplicableReservations { get; set; }
        public List<int> ApplicableReservations { get; set; }

        public List<int> PreferenceDetId { get; set; }
        public List<int> PreferenceDetType { get; set; }
        public List<int> DistrictDetId { get; set; }
        public List<int> TalukaDetId { get; set; }
        public List<int> InstituteDetId { get; set; }
        public List<int> InstituteType { get; set; }
        public List<int> TradeDetId { get; set; }
        public List<int> InstitutePreferenceId { get; set; }

        public decimal? EducationCGPA { get; set; }
        public bool RStateBoardType { get; set; }
        public bool ParticipateNextRound { get; set; }
        public int? RAppBasics { get; set; }
        public int? notificationTypes { get; set; }
        public string RationCard { get; set; }
        public string AadhaarNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string RollNumber { get; set; }
        public int slno { get; set; }
        public int ApplicationId { get; set; }
        public int? ApplyMonth { get; set; }
        public int? ApplyYear { get; set; }
        public string ApplicantNumber { get; set; }
        public string ApplicantName { get; set; }
        public string FathersName { get; set; }
        public string ParentsOccupation { get; set; }
        public string Photo { get; set; }
        public string PhotoUpload { get; set; }
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public string MothersName { get; set; }
        public int? Religion { get; set; }
        public int? Category { get; set; }
        public int? MinorityCategory { get; set; }
        public int? Caste { get; set; }
        public string CasteDetail { get; set; }
        public decimal? FamilyAnnIncome { get; set; }
        public int? ApplicantType { get; set; }

        public bool PhysicallyHanidcapInd { get; set; }
        public int? PhysicallyHanidcapType { get; set; }
        public int? Qualification { get; set; }
        public int? ApplicantBelongTo { get; set; }
        public int? AppliedBasic { get; set; }
        public int? TenthBoard { get; set; }
        public string InstituteStudiedQual { get; set; }
        public decimal? MaxMarks { get; set; }
        public decimal? MinMarks { get; set; }
        public decimal? MarksObtained { get; set; }
        public decimal? Percentage { get; set; }
        public int? ResultQual { get; set; }
        public string CommunicationAddress { get; set; }
        public int? DistrictId { get; set; }
        public int? TalukaId { get; set; }
        public int? Pincode { get; set; }
        public bool? SameAdd { get; set; }
        public string PermanentAddress { get; set; }
        public int? PDistrict { get; set; }
        public int? PTaluk { get; set; }
        public int? PPinCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string EmailId { get; set; }
        public int? DocVerificationCentre { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CredatedBy { get; set; }
        public int PermanentDistricts { get; set; }
        public string ApplicantPhoneNumber { get; set; }
        public string FathersPhoneNumber { get; set; }
        public int? DocVeriInstituteFilter { get; set; }
        public string DocVeriInstituteDistrict { get; set; }
        public int ApplicantTransId { get; set; }
        public int VerfOfficer { get; set; }
        public string UserName { get; set; }
        public int DocumentName { get; set; }
        public string DocumentWiseRemarks { get; set; }
        public int ApplicantId { get; set; }
        public int ReservationId { get; set; }
        public string SelectedReservationId { get; set; }
        public int? ApplStatus { get; set; }
        public string ApplRemarks { get; set; }
        public string UpdateMsg { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public HttpPostedFileBase PhotoFile { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public int? BoardId { get; set; }
        public string StatusName { get; set; }
        public bool PaymentOptionval { get; set; }
        public bool studiedMathsScience { get; set; }
        public string DocumentFeeReceiptDetails { get; set; }
        public int? TenthCOBSEBoard { get; set; }
        public string EducationGrade { get; set; }
        public string DocumentTypeName { get; set; }

        public bool? HoraNadu_GadiNadu_Kannidagas { get; set; }
        public bool? ExemptedFromStudyCertificate { get; set; }
        public bool? HyderabadKarnatakaRegion { get; set; }
        public bool? KanndaMedium { get; set; }
        public int UpdatedBy { get; set; }
        public int? FlowId { get; set; }
        public int? AssignedVO { get; set; }
        public int? ReVerficationStatus { get; set; }
        public string userRole { get; set; }
        public string ForwardedTo { get; set; }
        public string ApplDescription { get; set; }
        public string Remarks { get; set; }
        public string CommentsCreatedOn { get; set; }
        public string From_date_applicationForm { get; set; }
        public string To_date_applicationForm { get; set; }
        public string From_date_VerificationOfficer { get; set; }

        public string From_date_2ndRoundEntryChoiceTrade { get; set; }
        public string To_date_2ndRoundEntryChoiceTrade { get; set; }
        public string To_date_VerificationOfficer { get; set; }

        public DateTime? FromDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime? ToDt_ApplyingOnlineApplicationForm { get; set; }
        public DateTime? FromDt_DocVerificationPeriod { get; set; }
        public DateTime? ToDt_DocVerificationPeriod { get; set; }
        public DateTime? FromDt_2ndRoundEntryChoiceTrade { get; set; }
        public DateTime? ToDt_2ndRoundEntryChoiceTrade { get; set; }
    public DateTime? FromDt_1stRoundAdmissionProcess { get; set; }
    public DateTime? ToDt_1stRoundAdmissionProcess { get; set; }
    public DateTime? FromDt_2ndRoundAdmissionProcess { get; set; }
    public DateTime? ToFDt_2ndRoundAdmissionProcess { get; set; }
    public int ExserDocStatus { get; set; }
        public int EWSDocStatus { get; set; }
        public int? AgainstVacancyInd { get; set; }
        public bool? ExServiceMan { get; set; }
        public bool? EconomyWeakerSection { get; set; }
        public string CategoryName { get; set; }
    public string ApplicantRegNo { get; set; }
        public int? DocVeriFee { get; set; }
        public string TreasuryReceiptNo { get; set; }
        public DateTime? DocVeriFeePymtDate { get; set; }
        public string DocVeriFeePymtDatestring { get; set; }
        public int? ApplicationMode { get; set; }
        public string Caste_RD_No { get; set; }
        public string EconomicWeaker_RD_No { get; set; }
        public string HYD_Karnataka_RD_No { get; set; }
        public string UDID_No { get; set; }
    }

    public class ApplicantDocumentsDetail
    {
        public int Status { get; set; }
        public int DocAppId { get; set; }
        public int ApplicantId { get; set; }
        public int GrievanceId { get; set; }
        public int DocumentLength { get; set; }
        public int DocumentTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UpdateMsg { get; set; }
        public int Verified { get; set; }
        public HttpPostedFileBase EduCertificatePDF { get; set; }
        public HttpPostedFileBase CasteCertificatePDF { get; set; }
        public HttpPostedFileBase RationcardPDF { get; set; }
        public HttpPostedFileBase IncomePDF { get; set; }
        public HttpPostedFileBase UIDNumberPDF { get; set; }
        public HttpPostedFileBase RuralPDF { get; set; }
        public HttpPostedFileBase KannadaMediumPDF { get; set; }
        public HttpPostedFileBase DifferentlyAbledPDF { get; set; }
        public HttpPostedFileBase StudyExemptedPDF { get; set; }
        public HttpPostedFileBase HyderabadKarnatakaRegionPDF { get; set; }
        public HttpPostedFileBase HoranaaduGadinaaduKannadigaPDF { get; set; }
        public HttpPostedFileBase OtherCertificatesPDF { get; set; }
        public HttpPostedFileBase KashmirMigrantsPDF { get; set; }
        public HttpPostedFileBase ExservicemanPDF { get; set; }
        public HttpPostedFileBase LLCertificatePDF { get; set; }
        public HttpPostedFileBase EWSCertificatePDF { get; set; }
        public string EduCertificateRemarks { get; set; }
        public string CasteCertificateRemarks { get; set; }
        public string RationcardRemarks { get; set; }
        public string IncomeRemarks { get; set; }
        public string UIDNumberRemarks { get; set; }
        public string RuralRemarks { get; set; }
        public string KannadaMediumRemarks { get; set; }
        public string DifferentlyAbledRemarks { get; set; }
        public string StudyExemptedRemarks { get; set; }
        public string HyderabadKarnatakaRegionRemarks { get; set; }
        public string HoranaaduGadinaaduKannadigaRemarks { get; set; }
        public string OtherCertificatesRemarks { get; set; }
        public string KashmirMigrantsRemarks { get; set; }
        public string ExservicemanRemarks { get; set; }
        public string LLCertificateRemarks { get; set; }
        public string EWSCertificateRemarks { get; set; }

        public string DocumentRemarks { get; set; }
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
        public int UploadedByVerfication { get; set; }
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
        public int RationDocStatus { get; set; }
        public int IncCerDocStatus { get; set; }
        public int UIDDocStatus { get; set; }
        public int RUcerDocStatus { get; set; }
        public int KanMedCerDocStatus { get; set; }
        public int DiffAblDocStatus { get; set; }
        public int ExCerDocStatus { get; set; }
        public int HyKarDocStatus { get; set; }
        public int HorKanDocStatus { get; set; }
        public int OtherCerDocStatus { get; set; }
        public int KasMigDocStatus { get; set; }
        public int ExserDocStatus { get; set; }
        public int LLCerDocStatus { get; set; }
        public int EWSDocStatus { get; set; }

        public string ErrorOccuredMsg { get; set; }
        public int ErrorOccuredInd { get; set; }
        public int PhysicallyHanidcapType { get; set; }

        public string FathersName { get; set; }
        
        public DateTime? DOB { get; set; }
        public int? Gender { get; set; }
        public int? Category { get; set; }
        public int? ApplicantBelongTo { get; set; }

        public decimal MaxMarks { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MarksObtained { get; set; }
        public decimal Percentage { get; set; }
        public int? ResultQual { get; set; }
        public int? FlowId { get; set; }
        public int? ReVerficationStatus { get; set; }
        public int? CredatedBy { get; set; }

        public int VOApplStatus { get; set; }
        public int SaveAsDraft { get; set; }
        public int? DocumentSetInd { get; set; }
        public string VORemarks { get; set; }

        public bool? HoraNadu_GadiNadu_Kannidagas { get; set; }
        public bool? ExemptedFromStudyCertificate { get; set; }
        public bool? HyderabadKarnatakaRegion { get; set; }
        public bool? KanndaMedium { get; set; }

        public bool PhysicallyHanidcapInd { get; set; }
        public List<int> ApplicableReservations { get; set; }
    }

    public class ApplicantInstitutePreference
    {
        public int InstitutePreferenceId { get; set; }
        public int ApplicantId { get; set; }
        public int PreferenceId { get; set; }
        public int InstituteId { get; set; }
        public int PreferenceType { get; set; }
        public int TalukaId { get; set; }
        public int DistrictId { get; set; }
        public int TradeId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int Verified { get; set; }
        public string Remarks { get; set; }
        public int VerfOfficer { get; set; }
        public bool ParticipateNextRound { get; set; }
        //New fiels
        public string InstituteName { get; set; }
        public string TradeName { get; set; }

        //End Fields
        public List<ApplicationForm> InstituteDet { get; set; }
        public List<ApplicationForm> TalukDet { get; set; }
        public List<ApplicationForm> TradeDet { get; set; }
        public List<ApplicationForm> DocVerfiInstDet { get; set; }
    }

    public class ApplicationStatusUpdate
    {
        public string Remarks { get; set; }
        public int ApplicantId { get; set; }
        public string UpdateMsg { get; set; }
        public int ApplStatus { get; set; }
        public int VerfOfficer { get; set; }
        public int? ReVerficationStatus { get; set; }
        public int? FlowId { get; set; }
        public int DocumentName { get; set; }
        public int GrievanceId { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public int? ApplDescStatus { get; set; }
        public int? AssignedVO { get; set; }
        public int? CredatedBy { get; set; }
        public int? ApplStatusTrans { get; set; }
        public int? ApplStatusS { get; set; }
    }

    public class ApplicantCombinedData
    {
        public ApplicantApplicationForm ApplicantGeneralDetails { get; set; }
        public ApplicantDocumentsDetail ApplicantDocumentDetails { get; set; }
    }
}
