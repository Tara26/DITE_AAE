$(document).ready(function () {

    $('.nav-tabs li:eq(0) a').tab('show');
    $(".EditOptionShowGrid").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide();
    GetSessionYear("SessionYear");
    GetCourses("CourseType");
    GetApplicantType("ApplicantType");
    GetApplicantType("TraineeType");
    GetAdmissionRounds("RoundOption");
    GetGenderList();
    GetSessionYear("SessionYearAdmitted");
    GetCourses("CourseTypeAdmitted");
    GetApplicantType("ApplicantTypeAdmitted");
    GetAdmissionRounds("RoundOptionAdmitted");
    GetInstituteType();
    GetApplicationMode("applicationmode");
    GetApplicationMode("admittedapplicationmode");
    GetApplicationMode("rejectedapplicationmode");
    GetApplicationMode("reconcileapplicationmode");
    GetSessionYear("SessionYearRejected");
    GetCourses("CourseTypeRejected");
    GetApplicantType("ApplicantTypeRejected");
    GetAdmissionRounds("RoundOptionRejected");

    GetOnChangeSendTo("SendTo");
    GetOnChangeSendTo("usersPrin");
    $('.date-picker').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear1').val(inst.selectedYear);
            $('#AcademicMonths1').val(inst.selectedMonth);
        },
        beforeShow: function (dateText) {

        }
    });

    $('#AdmisionTime').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: '-30d',
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
    });

    $('#PaymentDate').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: 0.,
        maxDate: '0',
    });

    $(".toggle-accordion").on("click", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;

        $(this).toggleClass("active");

        if (numPanelOpen == 0) {
            openAllPanels(accordionId);
        } else {
            closeAllPanels(accordionId);
        }
    })

    openAllPanels = function (aId) {
        console.log("setAllPanelOpen");
        $(aId + ' .panel-collapse:not(".in")').collapse('show');
    }
    closeAllPanels = function (aId) {
        console.log("setAllPanelclose");
        $(aId + ' .panel-collapse.in').collapse('hide');
    }

    $('#Admitteddateofbirth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "-50:-13",
        dateFormat: 'dd-mm-yy',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });

    clearallErrorFields();
});

function GetInstituteType() {
    $("#InstituteType").empty();
    $("#InstituteType").append('<option value="0">Select Institute</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetApplicantTypeList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#InstituteType").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetGenderList() {

    $("#AdmittedGender").empty();
    $("#AdmittedGender").append('<option value="0">Select Gender</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetGenderList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#AdmittedGender").append($("<option/>").val(this.Gender_Id).text(this.Gender));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

$("#DownloadReceipt").click(function () {
    var url = "/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=";
    url = url + $("#ApplicantId").val();
    window.open(url);
});

$('a[href="#tab_1"]').click(function () {
    $("#tab_1").show();
    $("#tab_2").hide();
    $("#tab_3").hide();
    $("#tab_4").hide();
    $("#tab_2").removeClass("tab-pane fade show active in");
    $("#tab_2").addClass("tab-pane fade");
    $("#tab_3").removeClass("tab-pane fade show active in");
    $("#tab_3").addClass("tab-pane fade");
    $("#tab_4").removeClass("tab-pane fade show active in");
    $("#tab_4").addClass("tab-pane fade");
    $(".SearchOptionShowGrid").show();
    $(".SearchOptionAdmittedShowGrid").hide();
    $(".SearchOptionRejectedShowGrid").hide();
    $(".EditOptionShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".ApplicantAdmittedSeatDetails").hide();
    $(".ApplicantRejectedSeatDetails").hide();
    $('#EnableSendTo').hide();

    $("#divsubmitbtn").hide();
    $("#disSendToAdm").hide();

});

$('a[href="#tab_2"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").show();
    $("#tab_3").hide();
    $("#tab_4").hide();
    $("#tab_1").removeClass("tab-pane fade show active in");
    $("#tab_1").addClass("tab-pane fade");
    $("#tab_3").removeClass("tab-pane fade show active in");
    $("#tab_3").addClass("tab-pane fade");
    $("#tab_4").removeClass("tab-pane fade show active in");
    $("#tab_4").addClass("tab-pane fade");
    $(".SearchOptionShowGrid").hide();
    $(".SearchOptionShowGrid").show();
    $(".SearchOptionAdmittedShowGrid").show();
    $("#EnableSendTo").hide();
    $(".SearchOptionRejectedShowGrid").hide();
    $(".EditOptionShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".ApplicantAdmittedSeatDetails").hide();
    $(".ApplicantRejectedSeatDetails").hide();
    $("#divsubmitbtn").hide();
    $("#disSendToAdm").hide();
});

$('a[href="#tab_3"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").hide();
    $("#tab_3").show();
    $("#tab_4").hide();
    $("#tab_2").removeClass("tab-pane fade show active in");
    $("#tab_2").addClass("tab-pane fade");
    $("#tab_1").removeClass("tab-pane fade show active in");
    $("#tab_1").addClass("tab-pane fade");
    $("#tab_4").removeClass("tab-pane fade show active in");
    $("#tab_4").addClass("tab-pane fade");
    $(".SearchOptionShowGrid").hide();
    $(".SearchOptionAdmittedShowGrid").hide();
    $(".SearchOptionRejectedShowGrid").show();
    $(".EditOptionShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".ApplicantAdmittedSeatDetails").hide();
    $(".ApplicantRejectedSeatDetails").hide();
    $("#EnableSendTo").hide();
    $("#divsubmitbtn").hide();
    $("#disSendToAdm").hide();
    $("#disSentToJD").hide();
    $("#divbtnsentToJD").hide();
});

$('a[href="#tab_4"]').click(function () {
    GetSessionYear("SessionYearReconcile");
    GetCourses("CourseTypeReconcile");
    GetApplicantType("ApplicantTypeReconcile");
    GetAdmissionRounds("RoundOptionReconcile");
    $("#tab_1").hide();
    $("#tab_2").hide();
    $("#tab_3").hide();
    $("#tab_4").show();
    $("#tab_1").removeClass("tab-pane fade show active in");
    $("#tab_1").addClass("tab-pane fade");
    $("#tab_2").removeClass("tab-pane fade show active in");
    $("#tab_2").addClass("tab-pane fade");
    $("#tab_3").removeClass("tab-pane fade show active in");
    $("#tab_3").addClass("tab-pane fade");
    $(".SearchOptionShowGrid").show();
    $(".SearchOptionReconcileShowGrid").show();
    $("#EnableSendTo").hide();
    $(".SearchOptionRejectedShowGrid").hide();
    $(".EditOptionShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".ApplicantAdmittedSeatDetails").hide();
    $(".ApplicantRejectedSeatDetails").hide();

    $('.ApplicantReconcileSeatDetails').hide();
});

//region .. Tab 1

$('.AadhaarNumberText').keyup(function () {
    var foo = $(this).val().split("-").join(""); // remove hyphens
    if (foo.length > 0) {
        foo = foo.match(new RegExp('.{1,4}', 'g')).join("-");
    }
    $(this).val(foo);
});

function IncomeMoneyPhoneValidation(eveObj, id) {

    var eligibleLen = 10;
    if (id == "txtFamilyAnnualIncome") {
        eligibleLen = 8;
    }
    else if (id == "txtPermanentPincode" || id == "txtPincode") {
        eligibleLen = 5;
    }
    else if (id == "txtMaximumMarks" || id == "txtMarksObtained" || id == "txtMinMarks") {
        eligibleLen = 2;
    }
    else if (id == "AccountNumber") {
        eligibleLen = 15;
    }

    var len = $("#" + id).val().length;
    if (eveObj.charCode == 45 || eveObj.charCode == 43 || eveObj.charCode == 101 || len > eligibleLen ||
        (eveObj.charCode == 46 && id == "AccountNumber")) {
        event.preventDefault();
    }
}

function ValidateEmail(val) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(val)) {
        $("#txtEmailId-Required").show();
        return false;
    } else {
        return true;
    }
}

function getSelectedOptions(sel) {

    $("#DiffAbledCer").val(0);
    $("#ExServiceCer").val(0);
    $("#LandLoserCer").val(0);
    $("#EcoWeakCer").val(0);
    $("#KMCer").val(0);
    var len = sel.options.length;
    for (var i = 0; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected) {
            if (opt.value == "2")
                $("#ExServiceCer").val(1);
            else if (opt.value == "5")
                $("#EcoWeakCer").val(1);
        }
    }
}

function ValidateDOB(dateString) {
    var parts = dateString.split("-");
    var dtDOB = new Date(parts[1] + "-" + parts[0] + "-" + parts[2]);
    var dtCurrent = new Date();
    $("#dobError").val(0);
    $("#dateofbirth-Required").hide();
    if (dtCurrent.getFullYear() - dtDOB.getFullYear() < 13) {
        $("#dobError").val(1);
        $("#dateofbirth-Required").show();
        bootbox.alert("<br><br>Age should not be less than 13 years")
        return false;
    }

    if (dtCurrent.getFullYear() - dtDOB.getFullYear() == 13) {

        if (dtCurrent.getMonth() < dtDOB.getMonth()) {
            $("#dobError").val(1);
            $("#dateofbirth-Required").show();
            bootbox.alert("<br><br>Age should not be less than 13 years")
            return false;
        }
        if (dtCurrent.getMonth() == dtDOB.getMonth()) {
            if (dtCurrent.getDate() < dtDOB.getDate()) {
                $("#dateofbirth-Required").show();
                $("#dobError").val(1);
                bootbox.alert("<br><br>Age should not be less than 13 years")
                return false;
            }
        }
    }
    return true;
}

function OnChangeFilters() {
    $('.ApplicantSeatDetails').hide();
    $('#EnableSendTo').hide();
}

function GetDataAdmissionApplicants() {
    var IsValid = true;
 
    var SessionYear = $("#SessionYear :selected").val();
    var CourseType = $("#CourseType :selected").val();
    var ApplicantType = $("#ApplicantType :selected").val();
    var RoundOption = $("#RoundOption :selected").val();
    var ApplicatoinMode = $("#applicationmode :selected").val();

    $("#SessionYear-Required").hide();
    $("#CourseTypes-Required").hide();
    $("#ApplicantType-Required").hide();
    $("#RoundOption-Required").hide();
    if ($("#SessionYear :selected").val() == "choose") {
        $("#SessionYear-Required").show();
        IsValid = false;
    }
    if ($("#CourseType :selected").val() == "choose") {
        $("#CourseTypes-Required").show();
        IsValid = false;
    }
    if ($("#ApplicantType :selected").val() == "choose") {
        $("#ApplicantType-Required").show();
        IsValid = false;
    }
    if ($("#RoundOption :selected").val() == "choose" && ApplicantType != ApplicantTypes.Direct) {
        $("#RoundOption-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataAdmissionApplicants",
            data: { SessionYear: SessionYear, CourseType: CourseType, ApplicantType: ApplicantType, RoundOption: RoundOption, AdmittedorRejected: 1, ApplicatoinMode: ApplicatoinMode},
            contentType: "application/json",
            success: function (data) {

                if (data != null || data != '') {
                    fnGetYesNoForVal(data);
                }
                $('.ApplicantSeatDetails').show();
                $('#ApplicantSeatDetails').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download as Excel',
                            exportOptions: {
                                columns: [0, 11, 12, 13, 1, 14, 15, 2, 16, 17, 4, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                                    29, 30, 31, 32, 33, 5, 6, 34, 35, 36, 37, 38, 39, 40]
                            }
                        }
                    ],
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' }, 
                        { 'data': 'CourseTypeName', 'title': 'CourseType', 'className': 'text-left' },
                        { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' }, 
                        //{ 'data': 'ApplicantRank', 'title': 'Rank Number', 'className': 'text-left' },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                        { 'data': 'ApplicationMode', 'title': 'ApplicationMode', 'className': 'text-left' },
                        { 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                        { 'data': 'AdmittedStatusEx', 'title': 'Admission Status', 'className': 'text-left' },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");

                            }
                        },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmissionRegistrationNumber', 'title': 'Admission Registration Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible': false },
                        { 'data': 'GenderName', 'title': 'Gender', 'className': 'text-left', 'visible': false },
                        { 'data': 'Email', 'title': 'E-Mail Id', 'className': 'text-left', 'visible': false },
                        { 'data': 'AadharNumber', 'title': 'UID Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'ReligionName', 'title': 'Religion Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MinorityCategory', 'title': 'Minority Category', 'className': 'text-left', 'visible': false},
                        { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'HorizontalCategory', 'title': 'Admission Given in Horizontal Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'VerticalCategory', 'title': 'Admission Given in Vertical Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'TraineeType', 'title': 'Trainee Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left', 'visible': false },
                        { 'data': 'RationCardNo', 'title': 'Ration Card No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'IncomeCertificateNo', 'title': 'Income Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'AccountNumber', 'title': 'Bank Account No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'CasteCertNum', 'title': 'Caste Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'TradeName', 'title': 'Trade', 'className': 'text-left', 'visible': false },
                        { 'data': 'Units', 'title': 'Unit', 'className': 'text-left', 'visible': false },
                        { 'data': 'Shifts', 'title': 'Shift', 'className': 'text-left', 'visible': false },
                        { 'data': 'DualType', 'title': 'Dual System', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmTime', 'title': 'Admission Date', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmisionFee', 'title': 'Admission Fee Amount (In ₹)', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmFeePaidStatus', 'title': 'Admission Fee Status', 'className': 'text-left', 'visible': false },
                        { 'data': 'ReceiptNumber', 'title': 'Receipt No.', 'className': 'text-left', 'visible': false },
                    ]
                });
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}

$('.link').click(function (e) {
    e.preventDefault();
    var id = $(this).attr('id');
    var linktoRedirect = $("#" + id).attr('href');
    window.open(linktoRedirect, '_blank');;
});

function OnChangeAdmFeePaidStatus() {
    var AdmFeePaidStatus = $("input[name='AdmFeePaidStatus']:checked").val();
    if (AdmFeePaidStatus == 1) {
        SaveSeatAllocationFeePay(0);
    }
}

function GetApplicationDetailsById(ApplicationId) {
    
    $(".EditOptionShowGrid").find("*").prop('disabled', true);
    fnDisplayApplicantDetailScreen(ApplicationId);
    
    $(".EditOptionShowGrid").show();
    $("#EditSubmitbtn").show();

    if ($(".tab-pane.active").attr("id") == "tab_1") {
        $(".SearchOptionShowGrid").hide();
        $(".RemovebkEditClick").hide();
        $(".EditOptionShowGrid").find("*").prop('disabled', false);
        $("#SaveSeatAllocationFeePay").attr("disabled", true);
        $("#txtApplicantName").prop('disabled', true);
        $("#txtFathersName").prop('disabled', true);
        $("#academicyear1").prop('disabled', true);
    }
    if ($(".tab-pane.active").attr("id") == "tab_2") {
        $(".SearchOptionAdmittedShowGrid").hide();
        $(".RemovebkEditAdmittedClick").hide();
        $(".EditOptionShowGrid").find("*").prop('disabled', true);
        $("#EditSubmitbtn").hide();
    }
    if ($(".tab-pane.active").attr("id") == "tab_3") {
        $(".SearchOptionRejectedShowGrid").hide();
        $(".RemovebkEditRejectedClick").hide();
        $(".EditOptionShowGrid").find("*").prop('disabled', true);
        $("#EditSubmitbtn").hide();
    }
        $(".close").prop('disabled', false);
}
function SaveDocumentDetails() {

    $("#WithoutDocEduStatus-Required").hide();
    $("#WithDocEduStatus-Required").hide();
    $("#WithoutDocEduRemarks-Required").hide();
    $("#WithDocEduRemarks-Required").hide();
    $("#WithoutDocCasStatus-Required").hide();
    $("#WithDocCasStatus-Required").hide();
    $("#WithoutDocCasRemarks-Required").hide();
    $("#WithDocCasRemarks-Required").hide();
    $("#WithoutDocRatStatus-Required").hide();
    $("#WithDocRatStatus-Required").hide();
    $("#WithoutDocRatRemarks-Required").hide();
    $("#WithDocRatRemarks-Required").hide();
    $("#WithoutDocIncStatus-Required").hide();
    $("#WithDocIncStatus-Required").hide();
    $("#WithoutDocIncRemarks-Required").hide();
    $("#WithDocIncRemarks-Required").hide();
    $("#WithoutDocUIDStatus-Required").hide();
    $("#WithDocUIDStatus-Required").hide();
    $("#WithoutDocUIDRemarks-Required").hide();
    $("#WithDocUIDRemarks-Required").hide();
    $("#WithoutDocRurStatus-Required").hide();
    $("#WithDocRurStatus-Required").hide();
    $("#WithoutDocRurRemarks-Required").hide();
    $("#WithDocRurRemarks-Required").hide();
    $("#WithoutDocKMCStatus-Required").hide();
    $("#WithDocKMCStatus-Required").hide();
    $("#WithoutDocKMCRemarks-Required").hide();
    $("#WithDocKMCRemarks-Required").hide();
    $("#WithoutDocDAStatus-Required").hide();
    $("#WithDocDAStatus-Required").hide();
    $("#WithoutDocDARemarks-Required").hide();
    $("#WithDocDARemarks-Required").hide();
    $("#WithoutDocECStatus-Required").hide();
    $("#WithDocECStatus-Required").hide();
    $("#WithoutDocECRemarks-Required").hide();
    $("#WithDocECRemarks-Required").hide();
    $("#WithoutDocHKRStatus-Required").hide();
    $("#WithDocHKRStatus-Required").hide();
    $("#WithoutDocHKRRemarks-Required").hide();
    $("#WithDocHKRRemarks-Required").hide();
    $("#WithoutDocHorKanStatus-Required").hide();
    $("#WithDocHorKanStatus-Required").hide();
    $("#WithoutDocHorKanRemarks-Required").hide();
    $("#WithDocHorKanRemarks-Required").hide();
    $("#WithoutDocOCStatus-Required").hide();
    $("#WithDocOCStatus-Required").hide();
    $("#WithoutDocOCRemarks-Required").hide();
    $("#WithDocOCRemarks-Required").hide();
    $("#WithoutDocKMStatus-Required").hide();
    $("#WithDocExSerStatus-Required").hide();
    $("#WithoutDocKMRemarks-Required").hide();
    $("#WithDocExSerRemarks-Required").hide();
    $("#WithoutDocExSerStatus-Required").hide();
    $("#WithDocEWSStatus-Required").hide();
    $("#WithoutDocExSerRemarks-Required").hide();
    $("#WithDocEWSRemarks-Required").hide();
    $("#WithoutDocEWSStatus-Required").hide();
    $("#WithoutDocEWSRemarks-Required").hide();

    var fileData = new FormData();
    var IsValid = true;
    var EduCertRemarks = $.trim($("#txtEduCertRemarks").val());
    var CasteCertRemarks = $.trim($("#txtCasteCertRemarks").val());
    var RationCardRemarks = $.trim($("#txtRationCardRemarks").val());
    var IncCertRemarks = $.trim($("#txtIncCertRemarks").val());
    var UIDRemarks = $.trim($("#txtUIDRemarks").val());
    var RurCertRemarks = $.trim($("#txtRurCertRemarks").val());
    var KanMedRemarks = $.trim($("#txtKanMedRemarks").val());
    var DiffAbledCertRemarks = $.trim($("#txtDiffAbledCertRemarks").val());
    var ExeCertRemarks = $.trim($("#txtExeCertRemarks").val());
    var HydKarnRemarks = $.trim($("#txtHydKarnRemarks").val());
    var HorGadKannadigaRemarks = $.trim($("#txtHorGadKannadigaRemarks").val());
    var OtherCertRemarks = $.trim($("#txtOtherCertRemarks").val());
    var KashmirMigrantsRemarks = $.trim($("#txtKashmirMigrantsRemarks").val());
    var ExservicemanRemarks = $.trim($("#txtExservicemanRemarks").val());
    var LLCertificateRemarks = $.trim($("#txtLLCertificateRemarks").val());
    var EWSCertificateRemarks = $.trim($("#txtEWSCertificateRemarks").val());

    //EduCertificate
    if (!$('#collapseFour').attr("aria-expanded")) {
        $("#WithoutDocEduStatus-Required").hide();
        $("#WithoutDocEduRemarks-Required").hide();
        var fileUpload = $('#EduCertificate').get(0);
        if ($('.aEduCertificate').is(':visible')) {
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
            if ($("#EduDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#EduCertificate-Required").hide();
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }

                if ($("#EduDocStatus :selected").val() == 0) {
                    $("#WithDocEduStatus-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
                if (EduCertRemarks == "") {
                    $("#WithDocEduRemarks-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#EduDocStatus :selected").val() != 0) {
                    $("#WithoutDocEduStatus-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
                if (EduCertRemarks != "") {
                    $("#WithoutDocEduRemarks-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //CasteCertificate
        $("#WithoutDocCasStatus-Required").hide();
        $("#WithoutDocCasRemarks-Required").hide();
        fileUpload = $('#CasteCertificate').get(0);
        if ($('.aCasteCertificate').is(':visible')) {
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
            if ($("#CasDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#CasteCertificate-Required").hide();
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
                if ($("#CasDocStatus :selected").val() != 0) {
                    $("#WithDocCasStatus-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
                if (CasteCertRemarks != "") {
                    $("#WithDocCasRemarks-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#CasDocStatus :selected").val() != 0) {
                    $("#WithoutDocCasStatus-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
                if (CasteCertRemarks != "") {
                    $("#WithoutDocCasRemarks-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Rationcard
        $("#WithoutDocRatStatus-Required").hide();
        $("#WithoutDocRatRemarks-Required").hide();
        fileUpload = $('#Rationcard').get(0);
        if ($('.aRationCard').is(':visible')) {
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
            }
            if ($("#RationDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Rationcard-Required").hide();
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
                if ($("#RationDocStatus :selected").val() != 0) {
                    $("#WithDocRatStatus-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
                if (RationCardRemarks != "") {
                    $("#WithDocRatRemarks-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#RationDocStatus :selected").val() != 0) {
                    $("#WithoutDocRatStatus-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
                if (RationCardRemarks != "") {
                    $("#WithoutDocRatRemarks-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Incomecertificate 
        $("#WithoutDocIncStatus-Required").hide();
        $("#WithoutDocIncRemarks-Required").hide();
        fileUpload = $('#Incomecertificate').get(0);
        if ($('.aIncomeCertificate').is(':visible')) {
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
            if ($("#IncCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Incomecertificate-Required").hide();
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
                if ($("#IncCerDocStatus :selected").val() != 0) {
                    $("#WithDocIncStatus-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
                if (IncCertRemarks != "") {
                    $("#WithDocIncRemarks-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#IncCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocIncStatus-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
                if (IncCertRemarks != "") {
                    $("#WithoutDocIncRemarks-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //UID Number
        $("#WithoutDocIncStatus-Required").hide();
        $("#WithoutDocIncRemarks-Required").hide();
        fileUpload = $('#UIDNumber').get(0);
        $("#UIDNumber-Required").hide();
        if ($('.aUIDNumber').is(':visible')) {
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
            if ($("#UIDDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#UIDNumber-Required").hide();
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
                if ($("#UIDDocStatus :selected").val() != 0) {
                    $("#WithDocIncStatus-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
                if (UIDRemarks != "") {
                    $("#WithDocIncRemarks-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#UIDDocStatus :selected").val() != 0) {
                    $("#WithoutDocIncStatus-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
                if (UIDRemarks != "") {
                    $("#WithoutDocIncRemarks-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Rural Certificate
        var RuralUrban = $('input[name=RuralUrban]:checked').val();
        $("#WithoutDocRurStatus-Required").hide();
        $("#WithoutDocRurRemarks-Required").hide();
        fileUpload = $('#Ruralcertificate').get(0);
        $("#Ruralcertificate-Required").hide();
        $("#WithoutDocRurStatus-Required").hide();
        $("#WithoutDocRurRemarks-Required").hide();
        if ($('.aRuralcertificate').is(':visible')) {
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
            if ($("#RcerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Ruralcertificate-Required").hide();
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
                if ($("#RcerDocStatus :selected").val() == 0) {
                    $("#WithDocRurStatus-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
                if (RurCertRemarks == "") {
                    $("#WithDocRurRemarks-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#Ruralcertificate").val() == "" && RuralUrban == 1) {
                    $("#Ruralcertificate-Required").show();
                    IsValid = false;
                }
                if ($("#RcerDocStatus :selected").val() != 0) {
                    $("#WithoutDocRurStatus-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
                if (RurCertRemarks != "") {
                    $("#WithoutDocRurRemarks-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //KannadamediumCertificate
        fileUpload = $('#KannadamediumCertificate').get(0);
        $("#WithoutDocKMCStatus-Required").hide();
        $("#WithoutDocKMCRemarks-Required").hide();
        $("#KannadamediumCertificate-Required").hide();
        if ($('.aKannadamediumCertificate').is(':visible')) {
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
            if ($("#KanMedCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#KannadamediumCertificate-Required").hide();
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
                if ($("#KanMedCerDocStatus :selected").val() != 0) {
                    $("#WithDocKMCStatus-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
                if (KanMedRemarks != "") {
                    $("#WithDocKMCRemarks-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                $("#KannadamediumCertificate-Required").hide();
                if (($("#KannadamediumCertificate").val() == "") && ($("#KanMedCer").val() == 1)) {
                    $("#KannadamediumCertificate-Required").show();
                    IsValid = false;
                }
                if ($("#KanMedCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocKMCStatus-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
                if (KanMedRemarks != "") {
                    $("#WithoutDocKMCRemarks-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //DifferentlyAbledPDF
        fileUpload = $('#Differentlyabledcertificate').get(0);

        $("#WithoutDocDAStatus-Required").hide();
        $("#WithoutDocDARemarks-Required").hide();
        $("#Differentlyabledcertificate-Required").hide();
        $("#PersonWithDisability-Required").hide();
        $("#PersonWithDisabilityUploadDoc-Required").hide();
        if ($('.aDifferentlyabledcertificate').is(':visible')) {
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
            }
            if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                $("#PersonWithDisability-Required").show();
                $("#PersonWithDisabilityUploadDoc-Required").show();
                $(".PhysicallyHanidcapInd").focus();
                IsValid = false;
            }
            if ($("#DiffAblDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Differentlyabledcertificate-Required").hide();
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
                if ($("#DiffAblDocStatus :selected").val() == 0) {
                    $("#WithDocDAStatus-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
                if (DiffAbledCertRemarks == "") {
                    $("#WithDocDARemarks-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#Differentlyabledcertificate").val() == "") && ($("#DiffAbledCer").val() == 1)) {
                    $("#Differentlyabledcertificate-Required").show();
                    IsValid = false;
                }
                if ($("#DiffAblDocStatus :selected").val() != 0) {
                    $("#WithoutDocDAStatus-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
                if (DiffAbledCertRemarks != "") {
                    $("#WithoutDocDARemarks-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //ExemptedCertificate
        fileUpload = $('#ExemptedCertificate').get(0);
        $("#WithoutDocECStatus-Required").hide();
        $("#WithoutDocECRemarks-Required").hide();
        $("#ExemptedCertificate-Required").hide();
        if ($('.aExemptedCertificate').is(':visible')) {
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
            if ($("#ExCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#ExemptedCertificate-Required").hide();
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
                if ($("#ExCerDocStatus :selected").val() == 0) {
                    $("#WithDocECStatus-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
                if (ExeCertRemarks == "") {
                    $("#WithDocECRemarks-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#ExemptedCertificate").val() == "") && ($("#ExeStuCer").val() == 1)) {
                    $("#ExemptedCertificate-Required").show();
                    IsValid = false;
                }
                if ($("#ExCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocECStatus-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
                if (ExeCertRemarks != "") {
                    $("#WithoutDocECRemarks-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //HyderabadKarnatakaRegion
        $("#WithoutDocHKRStatus-Required").hide();
        $("#WithoutDocHKRRemarks-Required").hide();
        fileUpload = $('#HyderabadKarnatakaRegion').get(0);
        $("#HyderabadKarnatakaRegion-Required").hide();
        if ($('.aHyderabadKarnatakaRegion').is(':visible')) {
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
            if ($("#HyKarDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#HyderabadKarnatakaRegion-Required").hide();
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
                if ($("#HyKarDocStatus :selected").val() == 0) {
                    $("#WithDocHKRStatus-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
                if (HydKarnRemarks == "") {
                    $("#WithDocHKRRemarks-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#HyderabadKarnatakaRegion").val() == "") && ($("#HydKarCer").val() == 1)) {
                    $("#HyderabadKarnatakaRegion-Required").show();
                    IsValid = false;
                }
                if ($("#HyKarDocStatus :selected").val() != 0) {
                    $("#WithoutDocHKRStatus-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
                if (HydKarnRemarks != "") {
                    $("#WithoutDocHKRRemarks-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //HoranaaduKannadiga
        fileUpload = $('#HoranaaduKannadiga').get(0);
        $("#WithoutDocHorKanStatus-Required").hide();
        $("#WithoutDocHorKanRemarks-Required").hide();
        $("#HoranaaduKannadiga-Required").hide();
        if ($('.aHoranaaduKannadiga').is(':visible')) {
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
            if ($("#HorKanDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#HoranaaduKannadiga-Required").hide();
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
                if ($("#HorKanDocStatus :selected").val() == 0) {
                    $("#WithDocHorKanStatus-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
                if (HorGadKannadigaRemarks == "") {
                    $("#WithDocHorKanRemarks-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#HoranaaduKannadiga").val() == "") && ($("#HoraNaduCer").val() == 1)) {
                    $("#HoranaaduKannadiga-Required").show();
                    IsValid = false;
                }
                if ($("#HorKanDocStatus :selected").val() != 0) {
                    $("#WithoutDocHorKanStatus-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
                if (HorGadKannadigaRemarks != "") {
                    $("#WithoutDocHorKanRemarks-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //OtherCertificates
        fileUpload = $('#OtherCertificates').get(0);
        $("#WithoutDocOCStatus-Required").hide();
        $("#OtherCertificates-Required").hide();
        $("#WithoutDocOCRemarks-Required").hide();
        if ($('.aOtherCertificates').is(':visible')) {
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
            }
            if ($("#OtherCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#OtherCertificates-Required").hide();
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
                if ($("#OtherCerDocStatus :selected").val() == 0) {
                    $("#WithDocOCStatus-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
                if (OtherCertRemarks == "") {
                    $("#WithDocOCRemarks-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#OtherCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocOCStatus-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
                if (OtherCertRemarks != "") {
                    $("#WithoutDocOCRemarks-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }
        //Exserviceman
        fileUpload = $('#Exserviceman').get(0);
        $("#Exserviceman-Required").hide();
        $("#WithoutDocExSerStatus-Required").hide();
        $("#WithoutDocExSerRemarks-Required").hide();
        if ($('.aExserviceman').is(':visible')) {
            if ($('input[name=ExServiceManRDB]:checked').val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
            if ($("#ExserDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Exserviceman-Required").hide();
            if (($('input[name=ExServiceManRDB]:checked').val() == "") && ($("#ExServiceCer").val() == 1)) {
                $("#Exserviceman-Required").show();
                IsValid = false;
            }
            else if ($('input[name=ExServiceManRDB]:checked').val() == "") {
                if ($("#ExserDocStatus :selected").val() != 0) {
                    $("#WithoutDocExSerStatus-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }
                if (ExservicemanRemarks != "") {
                    $("#WithoutDocExSerRemarks-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }
            }
            else {
                if ($('input[name=ExServiceManRDB]:checked').val() != 0 && $("#ExserDocStatus :selected").val() == 0) {
                    $("#WithoutDocExSerStatus-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }
                if ($('input[name=ExServiceManRDB]:checked').val() != 0 && ExservicemanRemarks == "") {
                    $("#WithoutDocExSerRemarks-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }

                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
        }

        //EWSCertificate
        fileUpload = $('#EWSCertificate').get(0);
        $("#EWSCertificate-Required").hide();
        $("#WithoutDocEWSStatus-Required").hide();
        $("#WithoutDocEWSRemarks-Required").hide();
        if ($('.aEWSCertificate').is(':visible')) {
            if ($('input[name=EWSCertificateRDB]:checked').val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
            if ($("#EWSDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#EWSCertificate-Required").hide();
            $("#EWSCertificate-Required").hide();
            if (($('input[name=EWSCertificateRDB]:checked').val() == "") && ($("#EcoWeakCer").val() == 1)) {
                $("#EWSCertificate-Required").show();
                IsValid = false;
            }
            else if ($('input[name=EWSCertificateRDB]:checked').val() == "") {
                if ($("#EWSDocStatus :selected").val() != 0) {
                    $("#WithoutDocEWSStatus-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }
                if (EWSCertificateRemarks != "") {
                    $("#WithoutDocEWSRemarks-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }
            }
            else {
                if ($('input[name=EWSCertificateRDB]:checked').val() != 0 && $("#EWSDocStatus :selected").val() == 0) {
                    $("#WithoutDocEWSStatus-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }
                if ($('input[name=EWSCertificateRDB]:checked').val() != 0 && EWSCertificateRemarks == "") {
                    $("#WithoutDocEWSRemarks-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }

                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
        }
    }

    if (IsValid) {

        var RuralUrban = $('input[name=RuralUrban]:checked').val();
        if ($("#RcerDocStatus :selected").val() == 15) {
            RuralUrban = 1;
        }
        else if ($("#RcerDocStatus :selected").val() == 3) {
            RuralUrban = 2;
        }

        fileData.append(
            "ApplicantBelongTo", RuralUrban
        );

        var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
        if ($("#KanMedCerDocStatus :selected").val() == 15) {
            KanndaMedium = 1;
        }
        else if ($("#KanMedCerDocStatus :selected").val() == 3) {
            KanndaMedium = 0;
        }

        KanndaMedium == "1" ? KanndaMedium = true : KanndaMedium = false;
        fileData.append(
            "KanndaMedium", KanndaMedium
        );

        var PhysicallyHanidcapType = $("#PhysicallyHanidcapType :selected").val();
        var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val();
        if ($("#DiffAblDocStatus :selected").val() == 15) {
            PhysicallyHanidcapInd = 1;
        }
        else if ($("#DiffAblDocStatus :selected").val() == 3) {
            PhysicallyHanidcapInd = 0;
        }

        PhysicallyHanidcapInd == "1" ? PhysicallyHanidcapInd = true : PhysicallyHanidcapInd = false;
        fileData.append(
            "PhysicallyHanidcapInd", PhysicallyHanidcapInd
        );
        fileData.append(
            "PhysicallyHanidcapType", PhysicallyHanidcapType
        );

        var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
        if ($("#ExCerDocStatus :selected").val() == 15) {
            ExemptedFromStudyCertificate = 1;
        }
        else if ($("#ExCerDocStatus :selected").val() == 3) {
            ExemptedFromStudyCertificate = 0;
        }

        ExemptedFromStudyCertificate == "1" ? ExemptedFromStudyCertificate = true : ExemptedFromStudyCertificate = false;
        fileData.append(
            "ExemptedFromStudyCertificate", ExemptedFromStudyCertificate
        );

        var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
        if ($("#HyKarDocStatus :selected").val() == 15) {
            HyderabadKarnatakaRegion = 1;
        }
        else if ($("#HyKarDocStatus :selected").val() == 3) {
            HyderabadKarnatakaRegion = 0;
        }

        HyderabadKarnatakaRegion == "1" ? HyderabadKarnatakaRegion = true : HyderabadKarnatakaRegion = false;
        fileData.append(
            "HyderabadKarnatakaRegion", HyderabadKarnatakaRegion
        );

        var HoraNadu = $('input[name=HoraNadu]:checked').val();
        if ($("#HorKanDocStatus :selected").val() == 15) {
            HoraNadu = 1;
        }
        else if ($("#HorKanDocStatus :selected").val() == 3) {
            HoraNadu = 0;
        }

        HoraNadu == "1" ? HoraNadu = true : HoraNadu = false;
        fileData.append(
            "HoraNadu_GadiNadu_Kannidagas", HoraNadu
        );

        if ($("#ExserDocStatus :selected").val() == 15) {
            fileData.append(
                "ApplicableReservations", 2
            );
        }
        if ($("#EWSDocStatus :selected").val() == 15) {
            fileData.append(
                "ApplicableReservations", 5
            );
        }

        //V3 Review validation
        $("#DifferentlyabledcertificateYes-Required").hide();
        if ((($("#Differentlyabledcertificate").val() != "") && ($("#DiffAbledCer").val() != 1))
            || ($('.aDifferentlyabledcertificate').is(':visible') && ($("#DiffAbledCer").val() != 1))) {
            $("#DifferentlyabledcertificateYes-Required").show();
            $("#PhysicallyHanidcapYes").focus();
            IsValid = false;
        }

        $("#HoranaaduKannadigaYes-Required").hide();
        if ((($("#HoranaaduKannadiga").val() != "") && ($("#HoraNaduCer").val() != 1))
            || ($('.aHoranaaduKannadiga').is(':visible') && ($("#HoraNaduCer").val() != 1))) {
            $("#HoranaaduKannadigaYes-Required").show();
            $("#HoraNaduNo").focus();
            IsValid = false;
        }

        $("#ExemptedFromStudyCertificateYes-Required").hide();
        if ((($("#ExemptedCertificate").val() != "") && ($("#ExeStuCer").val() != 1))
            || ($('.aExemptedCertificate').is(':visible') && ($("#ExeStuCer").val() != 1))) {
            $("#ExemptedFromStudyCertificateYes-Required").show();
            $("#ExemptedFromStudyCertificateYes").focus();
            IsValid = false;
        }

        $("#HyderabadKarnatakaRegionYes-Required").hide();
        if ((($("#HyderabadKarnatakaRegion").val() != "") && ($("#HydKarCer").val() != 1))
            || ($('.aHyderabadKarnatakaRegion').is(':visible') && ($("#HydKarCer").val() != 1))) {
            $("#HyderabadKarnatakaRegionYes-Required").show();
            $("#HyderabadKarnatakaRegionYes").focus();
            IsValid = false;
        }

        $("#KannadamediumCertificateYes-Required").hide();
        if ((($("#KannadamediumCertificate").val() != "") && ($("#KanMedCer").val() != 1))
            || ($('.aKannadamediumCertificate').is(':visible') && ($("#KanMedCer").val() != 1))) {
            $("#KannadamediumCertificateYes-Required").show();
            $("#KanndaMediumYes").focus();
            IsValid = false;
        }

        $("#RuralcertificateYes-Required").hide();
        if ((($("#Ruralcertificate").val() != "") && (RuralUrban != 1))
            || ($('.aRuralcertificate').is(':visible') && (RuralUrban != 1))) {
            $("#RuralcertificateYes-Required").show();
            $(".RuralUrbanID").focus();
            IsValid = false;
        }

        $("#ExservicemanYes-Required").hide();
        if ((($("#Exserviceman").val() != "") && ($("#ExServiceCer").val() != 1))
            || ($('.aExserviceman').is(':visible') && ($("#ExServiceCer").val() != 1))) {
            $("#ExservicemanYes-Required").show();
            $("#ApplicableReservations").focus();
            IsValid = false;
        }

        $("#EWSCertificateYes-Required").hide();
        if ((($("#EWSCertificate").val() != "") && ($("#EcoWeakCer").val() != 1))
            || ($('.aEWSCertificate').is(':visible') && ($("#EcoWeakCer").val() != 1))) {
            $("#EWSCertificateYes-Required").show();
            $("#ApplicableReservations").focus();
            IsValid = false;
        }

        if (IsValid) {
            fileData.append(
                "DocumentLength", fileData.append.length
            );

            fileData.append(
                "ApplicantId", $("#ApplicantId").val()
            );

            //Adding Remarks
            fileData.append(
                "EduCertificateRemarks", EduCertRemarks
            );
            fileData.append(
                "CasteCertificateRemarks", CasteCertRemarks
            );
            fileData.append(
                "RationcardRemarks", RationCardRemarks
            );
            fileData.append(
                "IncomeRemarks", IncCertRemarks
            );
            fileData.append(
                "UIDNumberRemarks", UIDRemarks
            );
            fileData.append(
                "RuralRemarks", RurCertRemarks
            );
            fileData.append(
                "KannadaMediumRemarks", KanMedRemarks
            );
            fileData.append(
                "DifferentlyAbledRemarks", DiffAbledCertRemarks
            );
            fileData.append(
                "StudyExemptedRemarks", ExeCertRemarks
            );
            fileData.append(
                "HyderabadKarnatakaRegionRemarks", HydKarnRemarks
            );
            fileData.append(
                "HoranaaduGadinaaduKannadigaRemarks", HorGadKannadigaRemarks
            );
            fileData.append(
                "OtherCertificatesRemarks", OtherCertRemarks
            );

            fileData.append(
                "KashmirMigrantsRemarks", KashmirMigrantsRemarks
            );
            fileData.append(
                "LLCertificateRemarks", LLCertificateRemarks
            );
            fileData.append(
                "EWSCertificateRemarks", EWSCertificateRemarks
            );
            fileData.append(
                "ExservicemanRemarks", ExservicemanRemarks
            );

            var EDocAppIdVal = $("#EDocAppId").val();
            var CDocAppIdVal = $("#CDocAppId").val();
            var RDocAppIdVal = $("#RDocAppId").val();
            var IDocAppIdVal = $("#IDocAppId").val();
            var UDocAppIdVal = $("#UDocAppId").val();
            var RUDocAppIdVal = $("#RUDocAppId").val();
            var KDocAppIdVal = $("#KDocAppId").val();
            var DDocAppIdVal = $("#DDocAppId").val();
            var ExDocAppIdVal = $("#ExDocAppId").val();
            var HDocAppIdVal = $("#HDocAppId").val();
            var HGKDocAppIdVal = $("#HGKDocAppId").val();
            var ODocAppIdVal = $("#ODocAppId").val();
            var KMDocAppIdVal = $("#KMDocAppId").val();
            var ExSDocAppIdVal = $("#ExSDocAppId").val();
            var LLDocAppIdVal = $("#LLDocAppId").val();
            var EWSDocAppIdVal = $("#EWSDocAppId").val();

            var ECreatedByVal = $("#ECreatedBy").val();
            var CCreatedByVal = $("#CCreatedBy").val();
            var RCreatedByVal = $("#RCreatedBy").val();
            var ICreatedByVal = $("#ICreatedBy").val();
            var UCreatedByVal = $("#UCreatedBy").val();
            var RUCreatedByVal = $("#RUCreatedBy").val();
            var KCreatedByVal = $("#KCreatedBy").val();
            var DCreatedByVal = $("#DCreatedBy").val();
            var ExCreatedByVal = $("#ExCreatedBy").val();
            var HCreatedByVal = $("#HCreatedBy").val();
            var HGKCreatedByVal = $("#HGKCreatedBy").val();
            var OCreatedByVal = $("#OCreatedBy").val();
            var KMCreatedByVal = $("#KMCreatedBy").val();
            var ExSCreatedByVal = $("#ExSCreatedBy").val();
            var LLCreatedByVal = $("#LLCreatedBy").val();
            var EWSCreatedByVal = $("#EWSCreatedBy").val();

            var EduDocStatus = $("#EduDocStatus :selected").val();
            var CasDocStatus = $("#CasDocStatus :selected").val();
            var RationDocStatus = $("#RationDocStatus :selected").val();
            var IncCerDocStatus = $("#IncCerDocStatus :selected").val();
            var UIDDocStatus = $("#UIDDocStatus :selected").val();
            var RUcerDocStatus = $("#RcerDocStatus :selected").val();
            var KanMedCerDocStatus = $("#KanMedCerDocStatus :selected").val();
            var DiffAblDocStatus = $("#DiffAblDocStatus :selected").val();
            var ExCerDocStatus = $("#ExCerDocStatus :selected").val();
            var HyKarDocStatus = $("#HyKarDocStatus :selected").val();
            var HorKanDocStatus = $("#HorKanDocStatus :selected").val();
            var OtherCerDocStatus = $("#OtherCerDocStatus :selected").val();
            var KasMigDocStatus = $("#KasMigDocStatus :selected").val();
            var ExserDocStatus = $("#ExserDocStatus :selected").val();
            var LLCerDocStatus = $("#LLCerDocStatus :selected").val();
            var EWSDocStatus = $("#EWSDocStatus :selected").val();

            //Adding ID
            fileData.append(
                "EDocAppId", EDocAppIdVal
            );
            fileData.append(
                "CDocAppId", CDocAppIdVal
            );
            fileData.append(
                "RDocAppId", RDocAppIdVal
            );
            fileData.append(
                "IDocAppId", IDocAppIdVal
            );
            fileData.append(
                "UDocAppId", UDocAppIdVal
            );
            fileData.append(
                "RUDocAppId", RUDocAppIdVal
            );
            fileData.append(
                "KDocAppId", KDocAppIdVal
            );
            fileData.append(
                "DDocAppId", DDocAppIdVal
            );
            fileData.append(
                "ExDocAppId", ExDocAppIdVal
            );
            fileData.append(
                "HDocAppId", HDocAppIdVal
            );
            fileData.append(
                "HGKDocAppId", HGKDocAppIdVal
            );
            fileData.append(
                "ODocAppId", ODocAppIdVal
            );
            fileData.append(
                "KMDocAppId", KMDocAppIdVal
            );
            fileData.append(
                "ExSDocAppId", ExSDocAppIdVal
            );
            fileData.append(
                "LLDocAppId", LLDocAppIdVal
            );
            fileData.append(
                "EWSDocAppId", EWSDocAppIdVal
            );

            fileData.append(
                "UploadedByVerfication", 1
            );
            fileData.append(
                "ECreatedBy", ECreatedByVal
            );
            fileData.append(
                "CCreatedBy", CCreatedByVal
            );
            fileData.append(
                "RCreatedBy", RCreatedByVal
            );
            fileData.append(
                "ICreatedBy", ICreatedByVal
            );
            fileData.append(
                "UCreatedBy", UCreatedByVal
            );
            fileData.append(
                "RUCreatedBy", RUCreatedByVal
            );
            fileData.append(
                "KCreatedBy", KCreatedByVal
            );
            fileData.append(
                "DCreatedBy", DCreatedByVal
            );
            fileData.append(
                "ExCreatedBy", ExCreatedByVal
            );
            fileData.append(
                "HCreatedBy", HCreatedByVal
            );
            fileData.append(
                "HGKCreatedBy", HGKCreatedByVal
            );
            fileData.append(
                "OCreatedBy", OCreatedByVal
            );
            fileData.append(
                "KMCreatedBy", KMCreatedByVal
            );
            fileData.append(
                "ExSCreatedBy", ExSCreatedByVal
            );
            fileData.append(
                "LLCreatedBy", LLCreatedByVal
            );
            fileData.append(
                "EWSCreatedBy", EWSCreatedByVal
            );

            fileData.append(
                "EduDocStatus", EduDocStatus
            );
            fileData.append(
                "CasDocStatus", CasDocStatus
            );
            fileData.append(
                "RationDocStatus", RationDocStatus
            );
            fileData.append(
                "IncCerDocStatus", IncCerDocStatus
            );
            fileData.append(
                "UIDDocStatus", UIDDocStatus
            );
            fileData.append(
                "RUcerDocStatus", RUcerDocStatus
            );
            fileData.append(
                "KanMedCerDocStatus", KanMedCerDocStatus
            );
            fileData.append(
                "DiffAblDocStatus", DiffAblDocStatus
            );
            fileData.append(
                "ExCerDocStatus", ExCerDocStatus
            );
            fileData.append(
                "HyKarDocStatus", HyKarDocStatus
            );
            fileData.append(
                "HorKanDocStatus", HorKanDocStatus
            );
            fileData.append(
                "OtherCerDocStatus", OtherCerDocStatus
            );
            fileData.append(
                "KasMigDocStatus", KasMigDocStatus
            );
            fileData.append(
                "ExserDocStatus", ExserDocStatus
            );
            fileData.append(
                "LLCerDocStatus", LLCerDocStatus
            );
            fileData.append(
                "EWSDocStatus", EWSDocStatus
            );

            fileData.append(
                "CredatedBy", CredatedBy
            );
            fileData.append(
                "ApplicantId", ApplicantId
            );

            bootbox.confirm({
                message: "<br><br>Are you sure you want to Update the Documents?",
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-success btn-alert-yes'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger btn-alert-no'
                    }
                },
                callback : function(confirmed) {
                    if (confirmed) {
                        $.ajax({
                            type: "POST",
                            url: "/PaymentPDFGeneration/ApplicantDocumentDetails",
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (data) {
                                if (data[0].UpdateMsg == "success") {
                                    bootbox.alert("<br><br>Application required documents Added to the List");
                                    GetApplicationDetailsById($("#ApplicantId").val());
                                } else {
                                    alert("Cannot add to list !");
                                }
                            }
                        });
                    }
                }
            });
        }
        else {
            bootbox.alert("<br><br>There is error in your data!! Kindly check the documents section");
        }

    }
    else {
        bootbox.alert("<br><br>There is error in your data!!");
    }
}

function SaveSeatAllocationFeePay(PayMode) {


    var IsValid = true;
    $("#TraineeType-Required").hide();
    var TraineeType = $("#TraineeType :selected").val();
    if (TraineeType == "choose") {
        $("#TraineeType-Required").show();
        $("#TraineeType").focus();
        IsValid = false;
    }

    $("#AdmisionTime-Required").hide();
    if ($("#AdmisionTime").val() == "") {
        $("#AdmisionTime-Required").show();
        $("#AdmisionTime").focus();
        IsValid = false;
    }
    //$("#SaveSeatAllocationFeePay").attr("disabled", true);
    if (IsValid) {
        bootbox.confirm({
            message: "<br><br>Have you collected the amount of <b>'₹" + $("#AdmissionFee").text() + "'</b> from Applicant <b>'" + $("#ApplicationName").text() +"'</b> ?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success btn-alert-yes'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger btn-alert-no'
                }
            },
            callback : function(confirmed) {
                if (confirmed) {

                    var fileData = new FormData();
                    var AdmisionFeeVal = $("#AdmissionFee").text()
                    var AdmisionTime = "";
                    var dts = $("#AdmisionTime").val().split("-");
                    conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                    AdmisionTime = (dts[0] + "-" + dts[1] + "-" + dts[2]);

                    var DualType = $("input[name='DualType']:checked").val();
                    var ITIUnderPPP = $("input[name='ITIUnderPPP']:checked").val();

                    fileData.append(
                        "ApplicationId", $("#ApplicantId").val()
                    );
                    fileData.append(
                        "ApplicantNumber", $("#ApplicationNumber").text()
                    );
                    fileData.append(
                        "DualType", DualType
                    );
                    fileData.append(
                        "ITIUnderPPP", ITIUnderPPP
                    );
                    fileData.append(
                        "PaymentAmount", AdmisionFeeVal
                    );
                    fileData.append(
                        "TraineeTypeId", TraineeType
                    );
                    fileData.append(
                        "AdmisionTime", AdmisionTime
                    );
                    fileData.append(
                        "ClickToPay", PayMode
                    );
                    fileData.append(
                        "TradeName", $("#TradeName").text()
                    );
                    fileData.append(
                        "AdmFeePaidStatus", $("#AdmFeePaidStatus").val()
                    );
                    fileData.append(
                        "MISCode", $("#MISCode").text()
                    );
                    fileData.append(
                        "CourseType", $("#CourseType").val()
                    )

                    $.ajax({
                        type: "POST",
                        url: "/PaymentPDFGeneration/GeneratePaymentReceiptPDFData",
                        data: fileData,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            if (data != null) {
                                //$("#SaveSeatAllocationFeePay").attr("disabled", true);
                                $(".PaymentGenerationGridCls").show();

                                //$(".AdmisionFee").prop('disabled', true);
                                $(".AdmFeePaidStatus").prop('disabled', true);
                                $("#PaymentDate").prop('disabled', true);
                                $("input[name=AdmittedStatus][value=3]").prop('disabled', true);
                                $(".PaymentGenerationGridCls").show();
                                $("#AllocationFeePay").hide();

                                var AdmisionTime = daterangeformate2(data[0].AdmisionTime, 1);
                                $("#AdmisionTime").val(AdmisionTime);

                                if (data[0].ITIUnderPPP == 1)
                                    $("input[name=ITIUnderPPP][value=1]").prop('checked', true);
                                else
                                    $("input[name=ITIUnderPPP][value=0]").prop('checked', true);

                                if (data[0].AdmFeePaidStatus == 1)
                                    $("input[name=AdmFeePaidStatus][value=1]").prop('checked', true);
                                else
                                    $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);

                                var PaymentDate = daterangeformate2(data[0].PaymentDate, 1);

                                $("#PaymentDate").val(PaymentDate);

                                if (data[0].AdmittedStatus == 2)
                                    $("input[name=AdmittedStatus][value=2]").prop('checked', true);
                                else
                                    $("input[name=AdmittedStatus][value=6]").prop('checked', true);

                                $("#ReceiptNumber").text(data[0].ReceiptNumber);
                                $("#AdmissionRegistrationNumber").text(data[0].AdmissionRegistrationNumber);
                                $("#StateRegistrationNumber").text(data[0].StateRegistrationNumber);
                                $('#PaymentGenerationGrid').DataTable({
                                    data: data,
                                    "destroy": true,
                                    "bSort": false,
                                    sDom: 'lrtip',
                                    "bPaginate": false,
                                    "bInfo": false,
                                    searching: false,
                                    columns: [
                                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-center' },
                                        { 'data': 'AdmisionFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
                                        {
                                            'data': 'PaymentDate', 'title': 'Payment Date', 'className': 'text - left DOB',
                                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                                var date = daterangeformate2(oData.PaymentDate, 2);
                                                $(nTd).html(date);
                                            }
                                        },
                                        { 'data': 'ReceiptNumber', 'title': 'Receipt Number', 'className': 'text-center' },
                                        { 'data': 'PaymentMode', 'title': 'Payment Mode', 'className': 'text-center' },
                                        { 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
                                    ]
                                });
                            }
                        }
                    });
                }
                else {
                    $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);
                }
            }
        });
    }
    else {
        $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);
        bootbox.alert("<br><br>Please check all the required fields before proceeding with payment.!!");
    }
}

function OnClickEditSubmit() {

    var IsValid = true;
    if ($("input[name=AdmittedStatus]:checked").val() == 6) {

        if ($("input[name=AdmFeePaidStatus]:checked").val() != 1) {
            bootbox.alert("<br><br> The <b>'" + $("#TuitionFeePdStat").text() + "'</b> has NOT been selected. Please select <b>'" + $("#TuitionFeePdStat").text() + "'</b> as <b>'Received'<b> if Payment was offline.");
            $("#AdmFeePaidStatus").focus();
            return false;
        }

            $("#TraineeType-Required").hide();
            var TraineeType = $("#TraineeType :selected").val();
            if (TraineeType == "choose") {
                $("#TraineeType-Required").show();
                $("#TraineeType").focus();
                IsValid = false;
            }

            $("#AdmisionTime-Required").hide();
            if ($("#AdmisionTime").val() == "") {
                $("#AdmisionTime-Required").show();
                $("#AdmisionTime").focus();
                IsValid = false;
            }
            else {
                var dts = $("#AdmisionTime").val().split("-");
                conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                AdmisionTime = (dts[0] + "-" + dts[1] + "-" + dts[2]);
            }

            $("#PaymentDate-Required").hide();
            if ($("#PaymentDate").val() == "") {
                $("#PaymentDate-Required").show();
                $("#PaymentDate").focus();
                IsValid = false;
            }
            else {
                var dts = $("#PaymentDate").val().split("-");
                conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                PaymentDate = (dts[0] + "-" + dts[1] + "-" + dts[2]);
            }
        }
    if (IsValid) {

        let PercentageReturnValue = ToChkDocValidation(IsValid);
        IsValid = PercentageReturnValue.IsValid;
    }

    if (IsValid) {
        var CredatedBy = $("#ApplCredatedBy").val();
        $.ajax({
            type: 'GET',
            url: '/Admission/GetDocumentIndicatorOptionData',
            data: { ApplicationIdFromUI: CredatedBy },
            contentType: "application/json",
            success: function (datajsonDocInd) {
                if (datajsonDocInd != null && datajsonDocInd != "") {

                    ////V3 Review validation
                    //$("#DifferentlyabledcertificateYes-Required").hide();
                    //if ((($("#Differentlyabledcertificate").val() != "") && (!datajsonDocInd.Resultlist.PhysicallyHanidcapInd))
                    //    || ($('.aDifferentlyabledcertificate').is(':visible') && (!datajsonDocInd.Resultlist.PhysicallyHanidcapInd))) {
                    //    $("#DifferentlyabledcertificateYes-Required").show();
                    //    $("#PhysicallyHanidcapYes").focus();
                    //    IsValid = false;
                    //}

                    //$("#HoranaaduKannadigaYes-Required").hide();
                    //if ((($("#HoranaaduKannadiga").val() != "") && (!datajsonDocInd.Resultlist.HoraNadu_GadiNadu_Kannidagas))
                    //    || ($('.aHoranaaduKannadiga').is(':visible') && (!datajsonDocInd.Resultlist.HoraNadu_GadiNadu_Kannidagas))) {
                    //    $("#HoranaaduKannadigaYes-Required").show();
                    //    $("#HoraNaduNo").focus();
                    //    IsValid = false;
                    //}

                    //$("#ExemptedFromStudyCertificateYes-Required").hide();
                    //if ((($("#ExemptedCertificate").val() != "") && (!datajsonDocInd.Resultlist.ExemptedFromStudyCertificate))
                    //    || ($('.aExemptedCertificate').is(':visible') && (!datajsonDocInd.Resultlist.ExemptedFromStudyCertificate))) {
                    //    $("#ExemptedFromStudyCertificateYes-Required").show();
                    //    $("#ExemptedFromStudyCertificateYes").focus();
                    //    IsValid = false;
                    //}

                    //$("#HyderabadKarnatakaRegionYes-Required").hide();
                    //if ((($("#HyderabadKarnatakaRegion").val() != "") && (!datajsonDocInd.Resultlist.HyderabadKarnatakaRegion))
                    //    || ($('.aHyderabadKarnatakaRegion').is(':visible') && (!datajsonDocInd.Resultlist.HyderabadKarnatakaRegion))) {
                    //    $("#HyderabadKarnatakaRegionYes-Required").show();
                    //    $("#HyderabadKarnatakaRegionYes").focus();
                    //    IsValid = false;
                    //}

                    //$("#KannadamediumCertificateYes-Required").hide();
                    //if ((($("#KannadamediumCertificate").val() != "") && (!datajsonDocInd.Resultlist.KanndaMedium))
                    //    || ($('.aKannadamediumCertificate').is(':visible') && (!datajsonDocInd.Resultlist.KanndaMedium))) {
                    //    $("#KannadamediumCertificateYes-Required").show();
                    //    $("#KanndaMediumYes").focus();
                    //    IsValid = false;
                    //}

                    //$("#RuralcertificateYes-Required").hide();
                    //if ((($("#Ruralcertificate").val() != "") && (datajsonDocInd.Resultlist.ApplicantBelongTo != 1))
                    //    || ($('.aRuralcertificate').is(':visible') && (datajsonDocInd.Resultlist.ApplicantBelongTo != 1))) {
                    //    $("#RuralcertificateYes-Required").show();
                    //    $(".RuralUrbanID").focus();
                    //    IsValid = false;
                    //}

                    //$("#ExservicemanYes-Required").hide(); $("#EWSCertificateYes-Required").hide();
                    //var MultiselectSelectedValue = datajsonDocInd.Resultlist.SelectedReservationId;
                    //var ExservicemanYesInd = 0; var EWSCertificateYesInd = 0;
                    //if (MultiselectSelectedValue != null) {
                    //    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                    //        if (e == "2") {
                    //            ExservicemanYesInd = 1;
                    //        }
                    //        else if (e == "5") {
                    //            EWSCertificateYesInd = 1;
                    //        }
                    //    });
                    //}

                    //$("#ExservicemanYes-Required").hide();
                    //if ((($("#Exserviceman").val() != "") && (ExservicemanYesInd != 1))
                    //    || ($('.aExserviceman').is(':visible') && (ExservicemanYesInd != 1))) {
                    //    $("#ExservicemanYes-Required").show();
                    //    $("#ApplicableReservations").focus();
                    //    IsValid = false;
                    //}

                    //$("#EWSCertificateYes-Required").hide();
                    //if ((($("#EWSCertificate").val() != "") && (EWSCertificateYesInd != 1))
                    //    || ($('.aEWSCertificate').is(':visible') && (EWSCertificateYesInd != 1))) {
                    //    $("#EWSCertificateYes-Required").show();
                    //    $("#ApplicableReservations").focus();
                    //    IsValid = false;
                    //}

                    if (IsValid) {
  
                        bootbox.confirm({
                            message: "<br><br>Are you sure you want to Update the Applicant <b>" + $("#ApplicationName").text() +"</b> 's Admission Status as : <b>'" + $('input[name="AdmittedStatus"]:checked + label').text() + "' </b> ?",
                            buttons: {
                                confirm: {
                                    label: 'Yes',
                                    className: 'btn-success btn-alert-yes'
                                },
                                cancel: {
                                    label: 'No',
                                    className: 'btn-danger btn-alert-no'
                                }
                            },
                            callback: function (confirmed) {
                                if (confirmed) {
                                    var fileData = new FormData();
                                    var AdmisionFeeVal = $("#AdmissionFee").text()
                                    var AdmisionTime = "";
                                    var dts = $("#AdmisionTime").val().split("-");
                                    conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                                    AdmisionTime = (dts[0] + "-" + dts[1] + "-" + dts[2]);

                                    var DualType = $("input[name='DualType']:checked").val();
                                    var ITIUnderPPP = $("input[name='ITIUnderPPP']:checked").val();
                                    var AdmFeePaidStatus = $("input[name='AdmFeePaidStatus']:checked").val();
                                    var AdmittedStatus = $("input[name='AdmittedStatus']:checked").val();

                                    fileData.append(
                                        "ApplicantNumber", $("#ApplicationNumber").text()
                                    );

                                    fileData.append(
                                        "ApplicationId", $("#ApplicantId").val()
                                    );
                                    fileData.append(
                                        "DualType", DualType
                                    );
                                    fileData.append(
                                        "ITIUnderPPP", ITIUnderPPP
                                    );
                                    fileData.append(
                                        "PaymentAmount", AdmisionFeeVal = (AdmittedStatus == AdmittedOrRejected.Admitted ? AdmisionFeeVal : 0)
                                    );
                                    fileData.append(
                                        "TraineeTypeId", TraineeType
                                    );
                                    fileData.append(
                                        "AdmisionTime", AdmisionTime
                                    );
                                    fileData.append(
                                        "ClickToPay", 0
                                    );
                                    fileData.append(
                                        "AdmFeePaidStatus", AdmFeePaidStatus
                                    );
                                    fileData.append(
                                        "PaymentDate", PaymentDate
                                    );
                                    fileData.append(
                                        "AdmittedStatus", AdmittedStatus
                                    );
                                    fileData.append(
                                        "Remarks", $("#txtRemarks").val(),
                                    );
                                    fileData.append(
                                        "ApplInstiStatus", AdmittedStatus,
                                    );
                                    fileData.append(
                                        "AllocationId", $("#AllocationId").val(),
                                    );

                                    $.ajax({
                                        type: "POST",
                                        url: "/Admission/UpdateAdmittedDetails",
                                        data: fileData,
                                        contentType: false,
                                        processData: false,
                                        success: function (data) {
                                            bootbox.alert("<br><br>" + data[0].UpdateMsg.replace('InstName', $("#InstituteName").text()));
                                            OnClickEditCancel();
                                            GetDataAdmissionApplicants();
                                        }
                                    });

                                }
                            }
                        });
                    }

                }
            }
        });
    }
    else {
        bootbox.alert("<br><br> There is error in your page, Kindly check all section !!");
    }

}

function ToChkDocValidation(IsValidVal) {

    var IsValid = IsValidVal;
    var fileData = new FormData();
    var EduCertRemarks = $.trim($("#txtEduCertRemarks").val());
    var CasteCertRemarks = $.trim($("#txtCasteCertRemarks").val());
    var RationCardRemarks = $.trim($("#txtRationCardRemarks").val());
    var IncCertRemarks = $.trim($("#txtIncCertRemarks").val());
    var UIDRemarks = $.trim($("#txtUIDRemarks").val());
    var RurCertRemarks = $.trim($("#txtRurCertRemarks").val());
    var KanMedRemarks = $.trim($("#txtKanMedRemarks").val());
    var DiffAbledCertRemarks = $.trim($("#txtDiffAbledCertRemarks").val());
    var ExeCertRemarks = $.trim($("#txtExeCertRemarks").val());
    var HydKarnRemarks = $.trim($("#txtHydKarnRemarks").val());
    var HorGadKannadigaRemarks = $.trim($("#txtHorGadKannadigaRemarks").val());
    var OtherCertRemarks = $.trim($("#txtOtherCertRemarks").val());
    var ExservicemanRemarks = $.trim($("#txtExservicemanRemarks").val());
    var EWSCertificateRemarks = $.trim($("#txtEWSCertificateRemarks").val());

    //EduCertificate
    if (!$('#collapseFour').attr("aria-expanded")) {

        $("#WithoutDocEduStatus-Required").hide();
        $("#WithoutDocEduRemarks-Required").hide();
        var fileUpload = $('#EduCertificate').get(0);
        if ($('.aEduCertificate').is(':visible')) {
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
            if ($("#EduDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#EduCertificate-Required").hide();
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }

                if ($("#EduDocStatus :selected").val() == 0) {
                    $("#WithDocEduStatus-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
                if (EduCertRemarks == "") {
                    $("#WithDocEduRemarks-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#EduDocStatus :selected").val() != 0) {
                    $("#WithoutDocEduStatus-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
                if (EduCertRemarks != "") {
                    $("#WithoutDocEduRemarks-Required").show();
                    $("#EduDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //CasteCertificate
        $("#WithoutDocCasStatus-Required").hide();
        $("#WithoutDocCasRemarks-Required").hide();
        fileUpload = $('#CasteCertificate').get(0);
        if ($('.aCasteCertificate').is(':visible')) {
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
            if ($("#CasDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#CasteCertificate-Required").hide();
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
                if ($("#CasDocStatus :selected").val() != 0) {
                    $("#WithDocCasStatus-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
                if (CasteCertRemarks != "") {
                    $("#WithDocCasRemarks-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#CasDocStatus :selected").val() != 0) {
                    $("#WithoutDocCasStatus-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
                if (CasteCertRemarks != "") {
                    $("#WithoutDocCasRemarks-Required").show();
                    $("#CasDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Rationcard
        $("#WithoutDocRatStatus-Required").hide();
        $("#WithoutDocRatRemarks-Required").hide();
        fileUpload = $('#Rationcard').get(0);
        if ($('.aRationCard').is(':visible')) {
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
            }
            if ($("#RationDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Rationcard-Required").hide();
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
                if ($("#RationDocStatus :selected").val() != 0) {
                    $("#WithDocRatStatus-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
                if (RationCardRemarks != "") {
                    $("#WithDocRatRemarks-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#RationDocStatus :selected").val() != 0) {
                    $("#WithoutDocRatStatus-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
                if (RationCardRemarks != "") {
                    $("#WithoutDocRatRemarks-Required").show();
                    $("#RationDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Incomecertificate 
        $("#WithoutDocIncStatus-Required").hide();
        $("#WithoutDocIncRemarks-Required").hide();
        fileUpload = $('#Incomecertificate').get(0);
        if ($('.aIncomeCertificate').is(':visible')) {
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
            if ($("#IncCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Incomecertificate-Required").hide();
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
                if ($("#IncCerDocStatus :selected").val() != 0) {
                    $("#WithDocIncStatus-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
                if (IncCertRemarks != "") {
                    $("#WithDocIncRemarks-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#IncCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocIncStatus-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
                if (IncCertRemarks != "") {
                    $("#WithoutDocIncRemarks-Required").show();
                    $("#IncCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //UID Number
        $("#WithoutDocIncStatus-Required").hide();
        $("#WithoutDocIncRemarks-Required").hide();
        fileUpload = $('#UIDNumber').get(0);
        $("#UIDNumber-Required").hide();
        if ($('.aUIDNumber').is(':visible')) {
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
            if ($("#UIDDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#UIDNumber-Required").hide();
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
                if ($("#UIDDocStatus :selected").val() != 0) {
                    $("#WithDocIncStatus-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
                if (UIDRemarks != "") {
                    $("#WithDocIncRemarks-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#UIDDocStatus :selected").val() != 0) {
                    $("#WithoutDocIncStatus-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
                if (UIDRemarks != "") {
                    $("#WithoutDocIncRemarks-Required").show();
                    $("#UIDDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //Rural Certificate
        var RuralUrban = $('input[name=RuralUrban]:checked').val();
        $("#WithoutDocRurStatus-Required").hide();
        $("#WithoutDocRurRemarks-Required").hide();
        fileUpload = $('#Ruralcertificate').get(0);
        $("#Ruralcertificate-Required").hide();
        $("#WithoutDocRurStatus-Required").hide();
        $("#WithoutDocRurRemarks-Required").hide();
        if ($('.aRuralcertificate').is(':visible')) {
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
            if ($("#RcerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Ruralcertificate-Required").hide();
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
                if ($("#RcerDocStatus :selected").val() != 0) {
                    $("#WithDocRurStatus-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
                if (RurCertRemarks != "") {
                    $("#WithDocRurRemarks-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#Ruralcertificate").val() == "" && RuralUrban == 1) {
                    $("#Ruralcertificate-Required").show();
                    IsValid = false;
                }
                if ($("#RcerDocStatus :selected").val() != 0) {
                    $("#WithoutDocRurStatus-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
                if (RurCertRemarks != "") {
                    $("#WithoutDocRurRemarks-Required").show();
                    $("#RcerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //KannadamediumCertificate
        fileUpload = $('#KannadamediumCertificate').get(0);
        $("#WithoutDocKMCStatus-Required").hide();
        $("#WithoutDocKMCRemarks-Required").hide();
        $("#KannadamediumCertificate-Required").hide();
        if ($('.aKannadamediumCertificate').is(':visible')) {
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
            if ($("#KanMedCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#KannadamediumCertificate-Required").hide();
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
                if ($("#KanMedCerDocStatus :selected").val() != 0) {
                    $("#WithDocKMCStatus-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
                if (KanMedRemarks != "") {
                    $("#WithDocKMCRemarks-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                $("#KannadamediumCertificate-Required").hide();
                if (($("#KannadamediumCertificate").val() == "") && ($("#KanMedCer").val() == 1)) {
                    $("#KannadamediumCertificate-Required").show();
                    IsValid = false;
                }
                if ($("#KanMedCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocKMCStatus-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
                if (KanMedRemarks != "") {
                    $("#WithoutDocKMCRemarks-Required").show();
                    $("#KanMedCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //DifferentlyAbledPDF
        fileUpload = $('#Differentlyabledcertificate').get(0);

        $("#WithoutDocDAStatus-Required").hide();
        $("#WithoutDocDARemarks-Required").hide();
        $("#Differentlyabledcertificate-Required").hide();
        $("#PersonWithDisability-Required").hide();
        $("#PersonWithDisabilityUploadDoc-Required").hide();
        if ($('.aDifferentlyabledcertificate').is(':visible')) {
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }

                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
            }
            if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                $("#PersonWithDisability-Required").show();
                $("#PersonWithDisabilityUploadDoc-Required").show();
                $(".PhysicallyHanidcapInd").focus();
                IsValid = false;
            }
            if ($("#DiffAblDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Differentlyabledcertificate-Required").hide();
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }

                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
                if ($("#DiffAblDocStatus :selected").val() != 0) {
                    $("#WithDocDAStatus-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
                if (DiffAbledCertRemarks != "") {
                    $("#WithDocDARemarks-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#Differentlyabledcertificate").val() == "") && ($("#DiffAbledCer").val() == 1)) {
                    $("#Differentlyabledcertificate-Required").show();
                    IsValid = false;
                }
                if ($("#DiffAblDocStatus :selected").val() != 0) {
                    $("#WithoutDocDAStatus-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
                if (DiffAbledCertRemarks != "") {
                    $("#WithoutDocDARemarks-Required").show();
                    $("#DiffAblDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //ExemptedCertificate
        fileUpload = $('#ExemptedCertificate').get(0);
        $("#WithoutDocECStatus-Required").hide();
        $("#WithoutDocECRemarks-Required").hide();
        $("#ExemptedCertificate-Required").hide();
        if ($('.aExemptedCertificate').is(':visible')) {
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
            if ($("#ExCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#ExemptedCertificate-Required").hide();
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
                if ($("#ExCerDocStatus :selected").val() != 0) {
                    $("#WithDocECStatus-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
                if (ExeCertRemarks != "") {
                    $("#WithDocECRemarks-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#ExemptedCertificate").val() == "") && ($("#ExeStuCer").val() == 1)) {
                    $("#ExemptedCertificate-Required").show();
                    IsValid = false;
                }
                if ($("#ExCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocECStatus-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
                if (ExeCertRemarks != "") {
                    $("#WithoutDocECRemarks-Required").show();
                    $("#ExCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //HyderabadKarnatakaRegion
        $("#WithoutDocHKRStatus-Required").hide();
        $("#WithoutDocHKRRemarks-Required").hide();
        fileUpload = $('#HyderabadKarnatakaRegion').get(0);
        $("#HyderabadKarnatakaRegion-Required").hide();
        if ($('.aHyderabadKarnatakaRegion').is(':visible')) {
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
            if ($("#HyKarDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#HyderabadKarnatakaRegion-Required").hide();
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
                if ($("#HyKarDocStatus :selected").val() != 0) {
                    $("#WithDocHKRStatus-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
                if (HydKarnRemarks != "") {
                    $("#WithDocHKRRemarks-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#HyderabadKarnatakaRegion").val() == "") && ($("#HydKarCer").val() == 1)) {
                    $("#HyderabadKarnatakaRegion-Required").show();
                    IsValid = false;
                }
                if ($("#HyKarDocStatus :selected").val() != 0) {
                    $("#WithoutDocHKRStatus-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
                if (HydKarnRemarks != "") {
                    $("#WithoutDocHKRRemarks-Required").show();
                    $("#HyKarDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //HoranaaduKannadiga
        fileUpload = $('#HoranaaduKannadiga').get(0);
        $("#WithoutDocHorKanStatus-Required").hide();
        $("#WithoutDocHorKanRemarks-Required").hide();
        $("#HoranaaduKannadiga-Required").hide();
        if ($('.aHoranaaduKannadiga').is(':visible')) {
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
            if ($("#HorKanDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#HoranaaduKannadiga-Required").hide();
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
                if ($("#HorKanDocStatus :selected").val() != 0) {
                    $("#WithDocHorKanStatus-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
                if (HorGadKannadigaRemarks != "") {
                    $("#WithDocHorKanRemarks-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if (($("#HoranaaduKannadiga").val() == "") && ($("#HoraNaduCer").val() == 1)) {
                    $("#HoranaaduKannadiga-Required").show();
                    IsValid = false;
                }
                if ($("#HorKanDocStatus :selected").val() != 0) {
                    $("#WithoutDocHorKanStatus-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
                if (HorGadKannadigaRemarks != "") {
                    $("#WithoutDocHorKanRemarks-Required").show();
                    $("#HorKanDocStatus").focus();
                    IsValid = false;
                }
            }
        }

        //OtherCertificates
        fileUpload = $('#OtherCertificates').get(0);
        $("#WithoutDocOCStatus-Required").hide();
        $("#OtherCertificates-Required").hide();
        $("#WithoutDocOCRemarks-Required").hide();
        if ($('.aOtherCertificates').is(':visible')) {
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
            }
            if ($("#OtherCerDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#OtherCertificates-Required").hide();
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
                if ($("#OtherCerDocStatus :selected").val() == 0) {
                    $("#WithDocOCStatus-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
                if (OtherCertRemarks == "") {
                    $("#WithDocOCRemarks-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
            }
            else {
                if ($("#OtherCerDocStatus :selected").val() != 0) {
                    $("#WithoutDocOCStatus-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
                if (OtherCertRemarks != "") {
                    $("#WithoutDocOCRemarks-Required").show();
                    $("#OtherCerDocStatus").focus();
                    IsValid = false;
                }
            }
        }
        //Exserviceman
        fileUpload = $('#Exserviceman').get(0);
        $("#Exserviceman-Required").hide();
        $("#WithoutDocExSerStatus-Required").hide();
        $("#WithoutDocExSerRemarks-Required").hide();
        if ($('.aExserviceman').is(':visible')) {
            if ($("#Exserviceman").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
            if ($("#ExserDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#Exserviceman-Required").hide();
            if (($("#Exserviceman").val() == "") && ($("#ExServiceCer").val() == 1)) {
                $("#Exserviceman-Required").show();
                IsValid = false;
            }
            else if ($("#Exserviceman").val() == "") {

                if ($("#ExserDocStatus :selected").val() != 0) {
                    $("#WithoutDocExSerStatus-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }
                if (ExservicemanRemarks != "") {
                    $("#WithoutDocExSerRemarks-Required").show();
                    $("#Exserviceman").focus();
                    IsValid = false;
                }
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
        }

        //EWSCertificate
        fileUpload = $('#EWSCertificate').get(0);
        $("#EWSCertificate-Required").hide();
        $("#WithoutDocEWSStatus-Required").hide();
        $("#WithoutDocEWSRemarks-Required").hide();
        if ($('.aEWSCertificate').is(':visible')) {
            if ($("#EWSCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
            if ($("#EWSDocStatus :selected").val() == 0) {
                IsValid = false;
                $("#ErrorWithoutStatusMsg").show();
            }
        }
        else {
            $("#EWSCertificate-Required").hide();
            $("#EWSCertificate-Required").hide();
            if (($("#EWSCertificate").val() == "") && ($("#EcoWeakCer").val() == 1)) {
                $("#EWSCertificate-Required").show();
                IsValid = false;
            }
            else if ($("#EWSCertificate").val() == "") {
                if ($("#EWSDocStatus :selected").val() != 0) {
                    $("#WithoutDocEWSStatus-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }
                if (EWSCertificateRemarks != "") {
                    $("#WithoutDocEWSRemarks-Required").show();
                    $("#EWSCertificate").focus();
                    IsValid = false;
                }
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
        }
    }

    //var RuralUrban = $('input[name=RuralUrban]:checked').val();
    //if ($("#RcerDocStatus :selected").val() == 15) {
    //    RuralUrban = 1;
    //}
    //else if ($("#RcerDocStatus :selected").val() == 3) {
    //    RuralUrban = 2;
    //}

    //fileData.append(
    //    "ApplicantBelongTo", RuralUrban
    //);

    //var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
    //if ($("#KanMedCerDocStatus :selected").val() == 15) {
    //    KanndaMedium = 1;
    //}
    //else if ($("#KanMedCerDocStatus :selected").val() == 3) {
    //    KanndaMedium = 0;
    //}

    //KanndaMedium == "1" ? KanndaMedium = true : KanndaMedium = false;
    //fileData.append(
    //    "KanndaMedium", KanndaMedium
    //);

    //var PhysicallyHanidcapType = $("#PhysicallyHanidcapType :selected").val();
    //var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val();
    //if ($("#DiffAblDocStatus :selected").val() == 15) {
    //    PhysicallyHanidcapInd = 1;
    //}
    //else if ($("#DiffAblDocStatus :selected").val() == 3) {
    //    PhysicallyHanidcapInd = 0;
    //}

    //PhysicallyHanidcapInd == "1" ? PhysicallyHanidcapInd = true : PhysicallyHanidcapInd = false;
    //fileData.append(
    //    "PhysicallyHanidcapInd", PhysicallyHanidcapInd
    //);
    //fileData.append(
    //    "PhysicallyHanidcapType", PhysicallyHanidcapType
    //);

    //var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
    //if ($("#ExCerDocStatus :selected").val() == 15) {
    //    ExemptedFromStudyCertificate = 1;
    //}
    //else if ($("#ExCerDocStatus :selected").val() == 3) {
    //    ExemptedFromStudyCertificate = 0;
    //}

    //ExemptedFromStudyCertificate == "1" ? ExemptedFromStudyCertificate = true : ExemptedFromStudyCertificate = false;
    //fileData.append(
    //    "ExemptedFromStudyCertificate", ExemptedFromStudyCertificate
    //);

    //var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
    //if ($("#HyKarDocStatus :selected").val() == 15) {
    //    HyderabadKarnatakaRegion = 1;
    //}
    //else if ($("#HyKarDocStatus :selected").val() == 3) {
    //    HyderabadKarnatakaRegion = 0;
    //}

    //HyderabadKarnatakaRegion == "1" ? HyderabadKarnatakaRegion = true : HyderabadKarnatakaRegion = false;
    //fileData.append(
    //    "HyderabadKarnatakaRegion", HyderabadKarnatakaRegion
    //);

    //var HoraNadu = $('input[name=HoraNadu]:checked').val();
    //if ($("#HorKanDocStatus :selected").val() == 15) {
    //    HoraNadu = 1;
    //}
    //else if ($("#HorKanDocStatus :selected").val() == 3) {
    //    HoraNadu = 0;
    //}

    //HoraNadu == "1" ? HoraNadu = true : HoraNadu = false;
    //fileData.append(
    //    "HoraNadu_GadiNadu_Kannidagas", HoraNadu
    //);

    //if ($("#ExserDocStatus :selected").val() == 15) {
    //    fileData.append(
    //        "ApplicableReservations", 2
    //    );
    //}
    //if ($("#EWSDocStatus :selected").val() == 15) {
    //    fileData.append(
    //        "ApplicableReservations", 5
    //    );
    //}

    ////V3 Review validation
    //$("#DifferentlyabledcertificateYes-Required").hide();
    //if ((($("#Differentlyabledcertificate").val() != "") && ($("#DiffAbledCer").val() != 1))
    //    || ($('.aDifferentlyabledcertificate').is(':visible') && ($("#DiffAbledCer").val() != 1))) {
    //    $("#DifferentlyabledcertificateYes-Required").show();
    //    $("#PhysicallyHanidcapYes").focus();
    //    IsValid = false;
    //}

    //$("#HoranaaduKannadigaYes-Required").hide();
    //if ((($("#HoranaaduKannadiga").val() != "") && ($("#HoraNaduCer").val() != 1))
    //    || ($('.aHoranaaduKannadiga').is(':visible') && ($("#HoraNaduCer").val() != 1))) {
    //    $("#HoranaaduKannadigaYes-Required").show();
    //    $("#HoraNaduNo").focus();
    //    IsValid = false;
    //}

    //$("#ExemptedFromStudyCertificateYes-Required").hide();
    //if ((($("#ExemptedCertificate").val() != "") && ($("#ExeStuCer").val() != 1))
    //    || ($('.aExemptedCertificate').is(':visible') && ($("#ExeStuCer").val() != 1))) {
    //    $("#ExemptedFromStudyCertificateYes-Required").show();
    //    $("#ExemptedFromStudyCertificateYes").focus();
    //    IsValid = false;
    //}

    //$("#HyderabadKarnatakaRegionYes-Required").hide();
    //if ((($("#HyderabadKarnatakaRegion").val() != "") && ($("#HydKarCer").val() != 1))
    //    || ($('.aHyderabadKarnatakaRegion').is(':visible') && ($("#HydKarCer").val() != 1))) {
    //    $("#HyderabadKarnatakaRegionYes-Required").show();
    //    $("#HyderabadKarnatakaRegionYes").focus();
    //    IsValid = false;
    //}

    //$("#KannadamediumCertificateYes-Required").hide();
    //if ((($("#KannadamediumCertificate").val() != "") && ($("#KanMedCer").val() != 1))
    //    || ($('.aKannadamediumCertificate').is(':visible') && ($("#KanMedCer").val() != 1))) {
    //    $("#KannadamediumCertificateYes-Required").show();
    //    $("#KanndaMediumYes").focus();
    //    IsValid = false;
    //}

    //$("#RuralcertificateYes-Required").hide();
    //if ((($("#Ruralcertificate").val() != "") && (RuralUrban != 1))
    //    || ($('.aRuralcertificate').is(':visible') && (RuralUrban != 1))) {
    //    $("#RuralcertificateYes-Required").show();
    //    $(".RuralUrbanID").focus();
    //    IsValid = false;
    //}

    //$("#ExservicemanYes-Required").hide();
    //if ((($("#Exserviceman").val() != "") && ($("#ExServiceCer").val() != 1))
    //    || ($('.aExserviceman').is(':visible') && ($("#ExServiceCer").val() != 1))) {
    //    $("#ExservicemanYes-Required").show();
    //    $("#ApplicableReservations").focus();
    //    IsValid = false;
    //}

    //$("#EWSCertificateYes-Required").hide();
    //if ((($("#EWSCertificate").val() != "") && ($("#EcoWeakCer").val() != 1))
    //    || ($('.aEWSCertificate').is(':visible') && ($("#EcoWeakCer").val() != 1))) {
    //    $("#EWSCertificateYes-Required").show();
    //    $("#ApplicableReservations").focus();
    //    IsValid = false;
    //}

    return {
        IsValid
    };
}

//End region .. Tab 1

//region .. Tab 2
function GetDataAdmittedApplicants() {
    $(".EditOptionAdmittedShowGrid").hide();
    var IsValid = true;

    var SessionYear = $("#SessionYearAdmitted :selected").val();
    var CourseType = $("#CourseTypeAdmitted :selected").val();
    var ApplicantType = $("#ApplicantTypeAdmitted :selected").val();
    var RoundOption = $("#RoundOptionAdmitted :selected").val();
    var ApplicatoinMode = $("#admittedapplicationmode :selected").val();


    $("#CourseTypesAdmitted-Required").hide();
    $("#ApplicantTypeAdmitted-Required").hide();
    $("#RoundOptionAdmitted-Required").hide();
    if ($("#CourseTypeAdmitted :selected").val() == "choose") {
        $("#CourseTypesAdmitted-Required").show();
        IsValid = false;
    }
    if ($("#ApplicantTypeAdmitted :selected").val() == "choose") {
        $("#ApplicantTypeAdmitted-Required").show();
        IsValid = false;
    }
    if ($("#RoundOptionAdmitted :selected").val() == "choose") {
        $("#RoundOptionAdmitted-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataAdmissionApplicants",
            data: { SessionYear: SessionYear, CourseType: CourseType, ApplicantType: ApplicantType, RoundOption: RoundOption, AdmittedorRejected: 6, ApplicatoinMode: ApplicatoinMode },
            contentType: "application/json",
            success: function (data) {

                if (data != null || data != '') {
                    fnGetYesNoForVal(data);
                }
                $('.ApplicantAdmittedSeatDetails').show();
                $("#EnableSendTo").show();
                $('#ApplicantAdmittedSeatDetails').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download as Excel',
                            exportOptions: {
                                columns: [0, 6, 10, 11, 1, 2, 11, 3, 12, 13, 14, 5, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
                                    25, 26, 27, 28, 29, 7, 8, 30, 31, 32, 33, 34, 35, 36, 37]
                            }
                        }
                    ],
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Applicant Number', 'className': 'text-left' },
                        { 'data': 'AdmissionRegistrationNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        //{ 'data': 'ApplicantRank', 'title': 'Rank Number', 'className': 'text-left' },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MISCode', 'title': 'MISITI Code', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                        { 'data': 'OfficerName', 'title': 'Admission Officer Name', 'className': 'text-left' },
                        { 'data': 'AdmittedStatusEx', 'title': 'Admitted Status', 'className': 'text-left' },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible': false },
                        { 'data': 'GenderName', 'title': 'Gender', 'className': 'text-left', 'visible': false },
                        { 'data': 'Email', 'title': 'E-Mail Id', 'className': 'text-left', 'visible': false },
                        { 'data': 'AadharNumber', 'title': 'UID Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'ReligionName', 'title': 'Religion Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MinorityCategory', 'title': 'Minority Category', 'className': 'text-left', 'visible': false},
                        { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'HorizontalCategory', 'title': 'Admission Given in Horizontal Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'VerticalCategory', 'title': 'Admission Given in Vertical Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'TraineeType', 'title': 'Trainee Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left', 'visible': false },
                        { 'data': 'RationCardNo', 'title': 'Ration Card No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'IncomeCertificateNo', 'title': 'Income Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'AccountNumber', 'title': 'Bank Account No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'CasteCertNum', 'title': 'Caste Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'TradeName', 'title': 'Trade', 'className': 'text-left', 'visible': false },
                        { 'data': 'Units', 'title': 'Unit', 'className': 'text-left', 'visible': false },
                        { 'data': 'Shifts', 'title': 'Shift', 'className': 'text-left', 'visible': false },
                        { 'data': 'DualType', 'title': 'Dual System', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmTime', 'title': 'Admission Date', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmisionFee', 'title': 'Admission Fee Amount (In ₹)', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmFeePaidStatus', 'title': 'Admission Fee Status', 'className': 'text-left', 'visible': false},
                        { 'data': 'ReceiptNumber', 'title': 'Receipt No.', 'className': 'text-left', 'visible': false },
                        {
                            'title': 'Admission Fee Payment Receipt',
                            render: function (data, type, row) {
                                return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");
                            }
                        },
                    ],
                });
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}


//End region .. Tab 2

//region .. Tab 3
function GetDataRejectedApplicants() {
    $(".EditOptionRejectedShowGrid").hide();
    var IsValid = true;
    var SessionYear = $("#SessionYearRejected :selected").val();
    var CourseType = $("#CourseTypeRejected :selected").val();
    var ApplicantType = $("#ApplicantTypeRejected :selected").val();
    var RoundOption = $("#RoundOptionRejected :selected").val();
    var ApplicatoinMode = $("#rejectedapplicationmode :selected").val();

    $("#SessionYearRejected-Required").hide();
    $("#CourseTypesRejected-Required").hide();
    $("#ApplicantTypeRejected-Required").hide();
    $("#RoundOptionRejected-Required").hide();
    if (SessionYear == "choose") {
        $("#SessionYearRejected-Required").show();
        IsValid = false;
    }
    if ($("#CourseTypeRejected :selected").val() == "choose") {
        $("#CourseTypesRejected-Required").show();
        IsValid = false;
    }
    if ($("#ApplicantTypeRejected :selected").val() == "choose") {
        $("#ApplicantTypeRejected-Required").show();
        IsValid = false;
    }
    if ($("#RoundOptionRejected :selected").val() == "choose"  && ApplicantType != ApplicantTypes.Direct) {
        $("#RoundOptionRejected-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataAdmissionApplicants",
            data: { SessionYear: SessionYear, CourseType: CourseType, ApplicantType: ApplicantType, RoundOption: RoundOption, AdmittedorRejected: 3, ApplicatoinMode: ApplicatoinMode },
            contentType: "application/json",
            success: function (data) {

                if (data != null || data != '') {
                    fnGetYesNoForVal(data);
                }
                $('.ApplicantRejectedSeatDetails').show();
                    $('#ApplicantRejectedSeatDetails').DataTable({
                        data: data,
                        "destroy": true,
                        "bSort": true,
                        dom: fnSetDTExcelBtnPos(),
                        buttons: [
                            {
                                extend: 'excel',
                                text: 'Download as Excel',
                                exportOptions: {
                                    columns: [0, 10, 11, 12, 1, 13, 14, 2, 15, 4, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                                        29, 30, 31, 32, 5, 6, 33, 34, 35, 36, 37, 38, 39]
                                }
                            }
                        ],
                        columns: [
                            { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                            { 'data': 'ApplicantNumber', 'title': 'Applicant Number', 'className': 'text-left' },
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                            //{ 'data': 'ApplicantRank', 'title': 'Rank Number', 'className': 'text-left' },
                            { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                            { 'data': 'MISCode', 'title': 'MISITI Code', 'className': 'text-left' },
                            { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                            { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                            { 'data': 'OfficerName', 'title': 'Admission Officer Name', 'className': 'text-left' },
                            { 'data': 'AdmittedStatusEx', 'title': 'Admission Status', 'className': 'text-left' },
                            {
                                'title': 'Rejected Acknowledgement',
                                render: function (data, type, row) {
                                    return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                }
                            },
                            {
                                'data': 'ApplicationId',
                                'title': 'Remarks',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");

                                }
                            },
                            {
                                'data': 'ApplicationId',
                                'title': 'Action',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");
                                }
                            },
                            { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'AdmissionRegistrationNumber', 'title': 'Admission Registration Number', 'className': 'text-left', 'visible': false },
                            { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-left', 'visible': false },
                            { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible': false },
                            { 'data': 'GenderName', 'title': 'Gender', 'className': 'text-left', 'visible': false },
                            { 'data': 'Email', 'title': 'E-Mail Id', 'className': 'text-left', 'visible': false },
                            { 'data': 'AadharNumber', 'title': 'UID Number', 'className': 'text-left', 'visible': false },
                            { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'ReligionName', 'title': 'Religion Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'MinorityCategory', 'title': 'Minority Category', 'className': 'text-left', 'visible': false },
                            { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left', 'visible': false },
                            { 'data': 'HorizontalCategory', 'title': 'Admission Given in Horizontal Category', 'className': 'text-left', 'visible': false },
                            { 'data': 'VerticalCategory', 'title': 'Admission Given in Vertical Category', 'className': 'text-left', 'visible': false },
                            { 'data': 'TraineeType', 'title': 'Trainee Type', 'className': 'text-left', 'visible': false },
                            { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left', 'visible': false },
                            { 'data': 'RationCardNo', 'title': 'Ration Card No.', 'className': 'text-left', 'visible': false },
                            { 'data': 'IncomeCertificateNo', 'title': 'Income Certificate', 'className': 'text-left', 'visible': false },
                            { 'data': 'AccountNumber', 'title': 'Bank Account No.', 'className': 'text-left', 'visible': false },
                            { 'data': 'CasteCertNum', 'title': 'Caste Certificate', 'className': 'text-left', 'visible': false },
                            { 'data': 'TradeName', 'title': 'Trade', 'className': 'text-left', 'visible': false },
                            { 'data': 'Units', 'title': 'Unit', 'className': 'text-left', 'visible': false },
                            { 'data': 'Shifts', 'title': 'Shift', 'className': 'text-left', 'visible': false },
                            { 'data': 'DualType', 'title': 'Dual System', 'className': 'text-left', 'visible': false },
                            { 'data': 'AdmTime', 'title': 'Admission/Rejection Date', 'className': 'text-left', 'visible': false },
                            { 'data': 'AdmisionFee', 'title': 'Admission Fee Amount (In ₹)', 'className': 'text-left', 'visible': false },
                            { 'data': 'AdmFeePaidStatus', 'title': 'Admission Fee Status', 'className': 'text-left', 'visible': false },
                            { 'data': 'ReceiptNumber', 'title': 'Receipt No.', 'className': 'text-left', 'visible': false },
                        ]
                    });
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}

function GetRejectedDetailsById(ApplicationId, clickFrom) {
    //$(".SearchOptionRejectedShowGrid").hide();
    //$(".EditOptionRejectedShowGrid").show();

    //$('input[type="text"], textarea').attr('readonly', false);

    //clearallErrorFields();
    //if (clickFrom == "1") {
    //    GetAdmittedDetailsById(ApplicationId);
    //}
    if (clickFrom == "1") {
        $(".SearchOptionRejectedShowGrid").hide();
        $(".EditOptionRejectedShowGrid").show();
        $(".RemovebkEditRejectedClick").hide();
        $("#disSentToJD").hide();
        $("#divbtnsentToJD").hide();

        clearallErrorFields();
        $("#AdmittedApplicantId").val(ApplicationId);
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataAllocationFeeDetails",
            data: { ApplicationId: ApplicationId },
            contentType: "application/json",
            success: function (datajsonDetails) {

                $.each(datajsonDetails.Resultlist, function (index, item) {

                    $("#AdmittedApplicantId").text(this.ApplicationId);
                    $("#AdmittedApplicantNumber").text(this.ApplicantNumber);
                    $("#AdmittedApplicationName").text(this.ApplicantName);
                    $("#AdmittedRegistrationNumber").text(this.AdmissionRegistrationNumber);
                    $("#RollNumber").text(this.RollNumber);
                    $("#AdmittedStateRegistrationNumber").text(this.StateRegistrationNumber);
                    $("#AdmittedDivisonId").text(this.DivisionId);
                    $("#AdmittedDivisonName").text(this.DivisionName);
                    $("#AdmittedDistrictId").text(this.DistrictId);
                    $("#AdmittedDistrictName").text(this.DistrictName);
                    $("#AdmittedTalukId").text(this.TalukId);
                    $("#AdmittedTalukName").text(this.TalukName);
                    $("#AdmittedFatherName").text(this.FathersName);
                    $("#AdmittedMotherName").text(this.MothersName);
                    var finalDOB = daterangeformate2(this.DOB, 1);
                    $("#Admitteddateofbirth").val(finalDOB);
                    $("#AdmittedGender").val(this.Gender);
                    $("#AdmittedMISCode").text(this.MISCode);
                    $("#AdmittedInstituteType").text(this.InstituteType);
                    $("#AdmittedInstituteName").text(this.InstituteName);
                    $("#AdmittedTradeName").text(this.TradeName);
                    $("#AdmittedUnits").text(this.Units);
                    $("#AdmittedShifts").text(this.Shifts);

                    if (this.DualType == 1)
                        $("input[name=AdmittedDualType][value=1]").prop('checked', true);
                    else
                        $("input[name=AdmittedDualType][value=0]").prop('checked', true);

                    if (this.AdmisionFee == 1200)
                        $("input[name=AdmittedAdmisionFee][value=1200]").prop('checked', true);
                    else
                        $("input[name=AdmittedAdmisionFee][value=2400]").prop('checked', true);

                    if (this.AdmFeePaidStatus == 1)
                        $("input[name=AdmittedAdmFeePaidStatus][value=1]").prop('checked', true);
                    else
                        $("input[name=AdmittedAdmFeePaidStatus][value=0]").prop('checked', true);

                    if (this.AdmittedStatus == 3)
                        $("input[name=AdmittedAdmissionStatus][value=3]").prop('checked', true);
                    else
                        $("input[name=AdmittedAdmissionStatus][value=6]").prop('checked', true);

                });
            }
        });
        $("#txtRemarks").val(this.Remarks);
    }
}

//End region .. Tab 3
// Tab 4
function GetDataReconcileFees() {

    var IsValid = true;

    var SessionYear = $("#SessionYearReconcile :selected").val();
    var CourseType = $("#CourseTypeReconcile :selected").val();
    var ApplicantType = $("#ApplicantTypeReconcile :selected").val();
    var RoundOption = $("#RoundOptionReconcile :selected").val();
    var ApplicatoinMode = $("#reconcileapplicationmode :selected").val();

    $("#ApplicantTypeReconcile-Required").hide();
    $("#RoundOptionReconcile-Required").hide();

    if ($("#ApplicantTypeReconcile :selected").val() == "choose") {
        $("#ApplicantTypeReconcile-Required").show();
        IsValid = false;
    }
    if ($("#RoundOptionReconcile :selected").val() == "choose" && ApplicantType != ApplicantTypes.Direct) {
        $("#RoundOptionReconcile-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataReconcile",
            data: { SessionYear: SessionYear, CourseType: CourseType, ApplicantType: ApplicantType, RoundOption: RoundOption, AdmittedorRejected: AdmittedOrRejected.Admitted, ApplicatoinMode: ApplicatoinMode },
            contentType: "application/json",
            success: function (data) {

                $('.ApplicantReconcileSeatDetails').show();
                    $('#ApplicantReconcileSeatDetails').DataTable({
                        data: data,
                        "destroy": true,
                        columns: [
                            { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                            { 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-left' },
                            { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                            { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                            { 'data': 'TradeName', 'title': 'Trade', 'className': 'text-left' },
                            { 'data': 'TradeDuration', 'title': 'Trade Year', 'className': 'text-left' },
                            {
                                'data': 'ApplyYear', 'title': 'Session', 'className': 'text-left',

                                "render": function (nTd, sData, oData, iRow, iCol) {
                                    const date = new Date(oData.ApplyYear, oData.ApplyMonth, 1);
                                    const month = date.toLocaleString('default', { month: 'short' });
                                    const year = date.getFullYear();
                                    return (month + "/" + year + "-" + month + "/" + (year + parseInt(oData.TradeDuration)));
                                }
                            },
                            {
                                'data': 'PaymentDate', 'title': 'Payment Date', 'className': 'text - left DOB',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    var date = daterangeformate2(oData.AdmisionTime, 1);
                                    $(nTd).html(date);
                                }
                            },
                            { 'data': 'ReceiptNumber', 'title': 'Receipt Number', 'className': 'text-left' },
                            { 'data': 'AdmisionFee', 'title': 'Tution Fee Amount (In ₹)', 'className': 'text-center' }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api();
                            var totalAmount = 0;
                            for (var i = 0; i < data.length; i++) {
                                totalAmount += parseInt(data[i]["AdmisionFee"]);
                            }
                            var numOfColumns = $('#ApplicantReconcileSeatDetails').DataTable().columns().nodes().length; // Get total number of columns
                            $(this).children("th").remove();
                            var footer = $(this).append('<tfoot><tr></tr></tfoot>');
                            this.api().columns().every(function (index) {
                                if (index == numOfColumns - 1) {
                                    $(footer).append('<th><input type="label" style="text-align:center; width:100%;"' +
                                        'value = ₹' + totalAmount + ' ></th>');
                                } else if (index == numOfColumns - 3 || index == numOfColumns - 2) {
                                    if (index == numOfColumns - 3) {
                                        $(footer).append('<th colspan="2"><label style="text-align:center; width:100%;" >Admission Fee Amount </label></th>');
                                    }
                                } else {
                                    $(footer).append('<th><label style="width:100%;" /></th> ');
                                }
                            });
                        },
                    });
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}



function GetCommentDetails(ApplicationId) {
    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsRemarksById",
        data: { ApplicationId: ApplicationId },
        success: function (data) {

            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                searching: false,
                info: false,
                paging: false,
                columns: [
                    { 'data': 'AdmTime', 'title': 'Date', 'className': 'text-center' },
                    { 'data': 'userRole', 'title': 'Approved/Rejected By', 'className': 'text-center' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-center' },
                ]
            });
        }
    });
}

function GetOnChangeSendTo(user) {
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetForward",
        type: 'Get',
        //data: { 'level': level },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}




function OnClickSendToHierarchy() {

    var ForId = $('#SendTo :selected').val();
    var RoleSelected = $('#SendTo :selected').text();
    var TabName = "Rejected";

    if (ForId == 'choose') {
        bootbox.alert('please select the sent role');
    } else {
        bootbox.confirm('Do you want to Submit the Admitted applicant details?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetOnClickSendToHierarchy",
                    type: 'Get',
                    data: { ForId: ForId, TabName: TabName },
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data == "success") {
                            var _msg = "Submit Admitted applicant details to <b>" + RoleSelected + "</b> successfully.";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
}
function onsentToRegJD() {

    var ForId = $('#usersPrin :selected').val();
    var RoleSelected = $('#usersPrin :selected').text();
    var TabName = "Rejected";

    if (ForId == 'choose') {
        bootbox.alert('please select the role');
    } else {
        bootbox.confirm('Do you want to Submit the Rejected applicant details?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetOnClickSendToHierarchy",
                    type: 'Get',
                    data: { ForId: ForId, TabName: TabName },
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data == "success") {
                            var _msg = "Submit Rejected applicant details to <b>" + RoleSelected + "</b> successfully.";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
}
function OnChangeFiltersRejected() {

}

function funSentTo() {

}

function fnShowHidePaymentInfo() {
    $('#TraineeType option[value="' + TraineeType.Private + '"]').remove();
    if ($("input[name=AdmittedStatus]:checked").val() == 6) {
        $("#hideDataForRDBSelection").show();
    } else {
        $("#hideDataForRDBSelection").hide();
    }
}
function fnDisplayApplicantDetailScreen(ApplicationId) {
    
    GetApplicantDetailsByIdCmn(ApplicationId);
    $("#AdmisionTime").val('');
    $("#PaymentDate").val('');
    $("input[name=AdmittedStatus][value=6]").prop('checked', true);
    fnShowHidePaymentInfo();
    var date = new Date();
    var param = $("#ApplicantId").val();
    var base = $("#DownloadReceipt").data('url');
    $("#DownloadReceipt").attr('href', base + '?ApplicationId=' + param);

    $.ajax({
        type: "GET",
        url: "/Admission/GetDataAllocationFeeDetails",
        data: { ApplicationId: ApplicationId },
        contentType: "application/json",
        success: function (datajsonDetails) {

            $.each(datajsonDetails.Resultlist, function (index, item) {

                //Allocation and Fee Details
                $("#ApplicationNumber").text(this.ApplicantNumber);
                $("#ApplicationName").text(this.ApplicantName);
                $("#RankNumber").text(this.ApplicantRank);
                $("#DivisionId").text(this.DivisionId);
                $("#DivisionName").text(this.DivisionName);
                $("#DistrictCd").text(this.DistrictId);
                $("#DistrictName").text(this.DistrictName);
                $("#TalukId").text(this.TalukId);
                $("#TalukName").text(this.TalukName);
                $("#MISCode").text(this.MISCode);
                $("#InstituteName").text(this.InstituteName);
                $("#InstituteType").text(this.InstituteType);
                $("#VerticalCategory").text(this.VerticalCategory);
                $("#HorizontalCategory").text(this.HorizontalCategory);
                $("#TradeId").text(this.TradeCode);
                $("#TradeName").text(this.TradeName);
                $("#Units").text(this.Units);
                $("#Shifts").text(this.Shifts);
                $("#AllocationId").val(this.AllocationId);
                $("#AdmissionFee").text(this.AdmisionFee);
        
                if (this.DualType == 1)
                    $("input[name=DualType][value=1]").prop('checked', true);
                else
                    $("input[name=DualType][value=0]").prop('checked', true);

                $(".PaymentGenerationGridCls").hide();
                $("#AllocationFeePay").show();
                $(".AdmFeePaidStatus").prop('disabled', false);
                $("#PaymentDate").prop('disabled', false);
                if (this.AdmFeePaidStatus == 1) {
                    $(".AdmFeePaidStatus").prop('disabled', true);
                    $("#PaymentDate").prop('disabled', true);
                    $(".PaymentGenerationGridCls").show();
                    $("#AllocationFeePay").hide();
                    $("input[name=AdmittedStatus][value=6]").prop('checked', true);
                    fnShowHidePaymentInfo();

                    $('#PaymentGenerationGrid').DataTable({
                        data: datajsonDetails.Resultlist,
                        "destroy": true,
                        "bSort": false,
                        sDom: 'lrtip',
                        "bPaginate": false,
                        "bInfo": false,
                        searching: false,
                        columns: [
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-center' },
                            { 'data': 'AdmisionFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
                            {
                                'data': 'PaymentDate', 'title': 'Payment Date', 'className': 'text - left DOB',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    var date = daterangeformate2(oData.PaymentDate, 2);
                                    $(nTd).html(date);
                                }
                            },
                            { 'data': 'ReceiptNumber', 'title': 'Receipt Number', 'className': 'text-center' },
                            { 'data': 'PaymentMode', 'title': 'Payment Mode', 'className': 'text-center' },
                            { 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
                        ]
                    });
                }

                //if (this.PaymentDate != null)
                //    $("#SaveSeatAllocationFeePay").prop("disabled", true);
                //else
                //    $("#SaveSeatAllocationFeePay").prop("disabled", false);

                var AdmisionTime = daterangeformate2(this.AdmisionTime, 1);
                
                if (AdmisionTime == "") {
                    AdmisionTime = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                }
                $("#AdmisionTime").val(AdmisionTime);

                if (this.ITIUnderPPP == 1)
                    $("input[name=ITIUnderPPP][value=1]").prop('checked', true);
                else
                    $("input[name=ITIUnderPPP][value=0]").prop('checked', true);

                if (this.AdmFeePaidStatus == 1) {
                    $("input[name=AdmFeePaidStatus][value=1]").prop('checked', true);
                    $("input[name=AdmittedStatus][value=3]").prop('disabled', true);
                }
                else
                    $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);

                var PaymentDate = daterangeformate2(this.PaymentDate, 1);
                if (PaymentDate == "") {
                    PaymentDate = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                }
                $("#PaymentDate").val(PaymentDate);

                if (this.ReceiptNumber == "" || this.ReceiptNumber == null)
                    $("#ReceiptNumber").text("NIL");
                else
                    $("#ReceiptNumber").text(this.ReceiptNumber);

                $("#lblToShowPendingStatus").hide();
                if (this.AdmittedStatus == 2)
                    $("input[name=AdmittedStatus][value=2]").prop('checked', true);
                else if (this.AdmittedStatus == 1 || this.AdmittedStatus == 3) {
                    if (this.AdmittedStatus == 1) {
                        $("#lblToShowPendingStatus").show();
                    }

                }
                $("#txtRemarks").val(this.Remarks);

                if (this.AdmissionRegistrationNumber == "" || this.AdmissionRegistrationNumber == null)
                    $("#AdmissionRegistrationNumber").text("NIL");
                else
                    $("#AdmissionRegistrationNumber").text(this.AdmissionRegistrationNumber);

                if (this.RollNumber == "" || this.RollNumber == null)
                    $("#RollNumber").text("NIL");
                else
                    $("#RollNumber").text(this.RollNumber);

                if (this.StateRegistrationNumber == "" || this.StateRegistrationNumber == null)
                    $("#StateRegistrationNumber").text("NIL");
                else
                    $("#StateRegistrationNumber").text(this.StateRegistrationNumber);
            });
        }
    });
}