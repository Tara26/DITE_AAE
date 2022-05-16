var receiptNum;
$(document).ready(function () {
    
    $("#ToShowMsgToVO").hide();
    $("#ToShowMsgToVOGriApplied").hide();
    $('.nav-tabs li:eq(0) a').tab('show');
    
    $('.date-picker').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear').val(inst.selectedYear);
            $('#AcademicMonths').val(inst.selectedMonth);
        }
    });
    clearallErrorFields(); 
    //$("#myModal").hide();
    $('#HistoryRemarksCommentsModal').hide();
    GetCourses("CourseType22");
    //GetApplicantType("ApplicantTypeSelect");
    GetApplicationMode("ApplicantTypeSelect");

    $("#CalenderNotificationEligibleInd").val(0);
    GetEligibleDateFrmCalenderEvents();
    GetDataToVerifyDocuments();
    GetGrievanceStatus();
    GetDocumentApplicationStatus();
    GetSessionYear('ddlSessionDvF');
    GetCourses("ddlCourseTypeDvF");
    //GetApplicantType("ddlApplicantTypeDvF");
    GetApplicationMode("ddlApplicantTypeDvF");

    //GetSessionYear('ddlSessionDvF');
   // GetCourses("ddlCourseTypeDvF");
    //GetApplicantType("ddlApplicantTypeDvF");
    //GetApplicationMode("ddlApplicantTypeDvF");

    GetDataDocumentsVerificationFee();
    DocumentPaymentOption(0);
    //GetDocumentVerificationFeeToPay();
    //GetDocumentVerificationFeeToPay1();

        //$('#tblDocumentFeePay').DataTable({
        //    pageLength: 10,

        //});

    //Doc verification fee tab
    GetDataToVerifyDocumentsDivision();
     GetDataDocumentsVerificationFeeDivision();
    GetSessionYear('ddlSessionfee');
    GetCourses("ddlCourseTypefee");
    //GetApplicantType("ApplicantTypeffee");
    GetApplicationMode("ApplicantTypeffee");

    //GetDistrictList();
    GetDistrictsDDp('districtDoc', $('#hdnDiv_ID').data('value'));
    GetDistrictsDDp('districtreconcile', $('#hdnDiv_ID').data('value'));
    GetTaluksDDp('talukDoc', $('#hdnDiv_ID').data('value'));
    GetTaluksDDp('talukreconcile', $('#hdnDiv_ID').data('value'));
    //GetColleges(Instnamereconcile, $('#hdnDiv_ID').data('value'));
    //GetColleges(Instname, $('#hdnDiv_ID').data('value'));
    GetSessionYear("Session22");

});
function GetEligibleDateFrmCalenderEvents() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetEligibleDateFrmCalenderEvents',
        success: function (datajson) {
            var frmDate = ""; var toDate = ""; var dte = new Date();
            var curDate = dte.getUTCDate();
            var CurMon = dte.getUTCMonth();
            var CurYear = dte.getUTCFullYear();
            $.each(datajson, function () {

                if (this.From_date_VerificationOfficer != null && this.To_date_VerificationOfficer != null) {
                    frmDate = this.From_date_VerificationOfficer.split(',');
                    toDate = this.To_date_VerificationOfficer.split(',');

                    if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))
                        && new Date(CurYear, CurMon, curDate) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
                        $("#CalenderNotificationEligibleInd").val(1);
                        $("#EligibleVerificationForm").hide();
                    }
                    else {
                        $("#CalenderNotificationEligibleInd").val(0);
                        $("#EligibleVerificationForm").show();
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").hide();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                }
                return false;
            });
        }
    });
}

function GetDataToVerifyDocuments() {
    //if ($('#CourseType22').val() == "choose") {

   

    if ($('#CourseType22').val() == "101" || $('#CourseType22').val() == null) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetApplicantsStatusFilter",
            contentType: "application/json",
            //data: { 'year': 0, 'courseType': 0, 'applicanType': 0 },
            data: { year: 0, courseType: 0, applicanType: 0, division_id: 0, district_lgd_code: 0, taluk_lgd_code: 0, InstituteId: 0 },

            success: function (data) {
                $('#VerifyDocumentsTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                        //{
                        //    'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                        //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        //        if (oData.PaymentOptionval == true) {
                        //            $(nTd).html("Online");
                        //        }
                        //        else {
                        //            $(nTd).html("Offline");
                        //        }
                        //    }
                        //},

                        { 'data': 'OfficerName', 'title': 'Verification Officer', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                        {
                            'title': 'Application Acknowledgment',
                            render: function (data, type, row) {
                                
                                if (row.ApplDescStatus == 4) {

                                    return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                } else { return "" }
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetCommentDetailsApplicant(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'CredatedBy',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",1)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

                            }
                        }
                    ]
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        var year = $('#Session22').val();
        var courseType = $('#CourseType22').val();
        var appType = $('#ApplicantTypeSelect').val();
        if (year == 'choose') {
            bootbox.alert("select the session");
        } else if (courseType == 'choose') {
            bootbox.alert("select the course type");
        } else
            if (appType == 'choose') {
                bootbox.alert("select the applicant type");
            } else {
                $.ajax({
                    type: "GET",
                    url: "/Admission/GetApplicantsStatusFilter",
                    contentType: "application/json",
                    //data: { 'year': year, 'courseType': courseType, 'applicanType': appType },
                    data: { year: year, courseType: courseType, applicanType: appType, division_id: 0, district_lgd_code: 0, taluk_lgd_code: 0, InstituteId: 0 },

                    success: function (data) {
                        $('#VerifyDocumentsTable').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                                { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                                { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                                { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                                { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                                { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                                { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                                //{
                                //    'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                                //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                //        if (oData.PaymentOptionval == true) {
                                //            $(nTd).html("Online");
                                //        }
                                //        else {
                                //            $(nTd).html("Offline");
                                //        }
                                //    }
                                //},

                                { 'data': 'OfficerName', 'title': 'Verification Officer Name', 'className': 'text-left' },
                                { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                                {
                                    'title': 'Application Acknowledgment',
                                    render: function (data, type, row) {
                                        
                                        if (row.ApplDescStatus == 4) { 
                                        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                        }
                                        { return "" }}
                                },
                                {
                                    'data': 'ApplicationId',
                                    'title': 'Remarks',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationRemarks(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                                    }
                                },
                                {
                                    'data': 'CredatedBy',
                                    'title': 'Action',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",1)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

                                    }
                                }
                            ]
                        });
                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }

    }
}

function GetGrievanceStatus() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantsStatus",
        contentType: "application/json",
        success: function (data) {
            $('#GrievanceStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                    {
                        'data': 'CredatedBy',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            //if (oData.GrievanceRefNumber != null)
                            $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",2)' class='btn btn-primary' value='Edit' id='edit'/>");

                        }
                    }
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br> There is error in your data");
        }
    });
}

function RuralUrbanLocation() {

    $("input:radio[name=RuralUrban]").prop('checked', true);
    $("input[name='RuralUrban']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}


function AppliedWhichBasics() {

    $("input:radio[name=AppBasics]").prop('checked', true);
    $("input[name='AppBasics']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedForSyallbus() {

    $("input:radio[name=TenthBoard]").prop('checked', true);
    $("input[name='TenthBoard']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function tConvert(time) {
    // Check correct time format and split into components
    time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

    if (time.length > 1) { // If time format correct
        time = time.slice(1);  // Remove full string match value
        time[5] = +time[0] < 12 ? 'AM' : 'PM'; // Set AM/PM
        time[0] = +time[0] % 12 || 12; // Adjust hours
    }
    return time.join(''); // return adjusted time or original string
}

function daterangeformate2(datetime, dobvalue) {
    var openingsd = "";
    if (datetime == "" || datetime == null) {
        var openingsd = "";
        return openingsd;
    }
    else {
        var StartDateOpening = new Date(parseInt(datetime.replace('/Date(', '')))
        var StartDateOpeningmonth = StartDateOpening.toString().slice(4, 7);
        var StartDateOpeningdate = StartDateOpening.toString().slice(8, 10);
        var StartDateOpeningyear = StartDateOpening.toString().slice(11, 15);
        var FindExactTime = StartDateOpening.toString().slice(16, 24);
        var exacttimetoshow = tConvert(FindExactTime);

        var statedatemonth1 = "";
        if (StartDateOpeningmonth == "Jan") { statedatemonth1 = "01"; }
        if (StartDateOpeningmonth == "Feb") { statedatemonth1 = "02"; }
        if (StartDateOpeningmonth == "Mar") { statedatemonth1 = "03"; }
        if (StartDateOpeningmonth == "Apr") { statedatemonth1 = "04"; }
        if (StartDateOpeningmonth == "May") { statedatemonth1 = "05"; }
        if (StartDateOpeningmonth == "Jun") { statedatemonth1 = "06"; }
        if (StartDateOpeningmonth == "Jul") { statedatemonth1 = "07"; }
        if (StartDateOpeningmonth == "Aug") { statedatemonth1 = "08"; }
        if (StartDateOpeningmonth == "Sep") { statedatemonth1 = "09"; }
        if (StartDateOpeningmonth == "Oct") { statedatemonth1 = "10"; }
        if (StartDateOpeningmonth == "Nov") { statedatemonth1 = "11"; }
        if (StartDateOpeningmonth == "Dec") { statedatemonth1 = "12"; }

        if (dobvalue == 1)
            openingsd = StartDateOpeningdate + "-" + statedatemonth1 + "-" + StartDateOpeningyear;
        else
            openingsd = StartDateOpeningdate + "-" + statedatemonth1 + "-" + StartDateOpeningyear + " " + exacttimetoshow;


        return openingsd;
    }
}

function OnchangechkSameAsCommunicationAddress() {
    if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
        $("#txtPermanentAddress").val($("#txtCommunicationAddress").val());
        $("#PermanentDistricts").val($("#Districts :selected").val());

        $.ajax({
            type: 'Get',
            url: '/Admission/GetMasterApplicantData',
            success: function (datajson) {
                if (datajson.Resultlist != null || datajson.Resultlist != '') {
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $.each(datajson.Resultlist.GetDistrictList, function () {
                            $.each($(this.TalukListDet), function (index, item) {
                                $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                            });
                        });
                    }
                }
            }
        });

        $("#PermanentTalukas").val($("#Talukas :selected").val());
        $("#txtPermanentPincode").val($("#txtPincode").val());
        $("#txtPermanentAddress").attr('readonly', 'readonly');
        $('#PermanentDistricts').attr("disabled", true);
        $('#PermanentTalukas').attr("disabled", true);
        $('#txtPermanentPincode').attr('readonly', true);
    }
    else {
        $("#txtPermanentAddress").val('');
        $("#PermanentDistricts").val(0);
        $("#PermanentTalukas").val(0);
        $("#txtPermanentPincode").val('');
        $("#txtPermanentAddress").attr('readonly', false);
        $('#PermanentDistricts').attr("disabled", false);
        $('#PermanentTalukas').attr("disabled", false);
        $('#txtPermanentPincode').attr('readonly', false);
    }
}

function GetApplicationRemarks(ApplicationId) {
    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: 'Post',
        data: { ApplicantTransId: ApplicationId },
        url: '/Admission/GetApplicantRemarksList',
        success: function (data) {

            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CreatedOn', 'title': 'Date', 'className': 'text-left' },
                    { 'data': 'userRole', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'ForwardedTo', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'ApplDescription', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Description', 'className': 'text-left' },
                ]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    });
}

function GetCommentDetailsApplicant(ApplicationId) {
    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: 'Post',
        data: { SeatAllocationId: ApplicationId },
        url: '/Admission/GetCommentDetailsApplicant',
        success: function (data) {

            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CommentsCreatedOn', 'title': 'Date', 'className': 'text-left' },
                    { 'data': 'userRole', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'ForwardedTo', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'ApplDescription', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'Remark', 'title': 'Description', 'className': 'text-left' },
                ]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    });
}

function GetDocumentApplicationStatus() {

    $("#EduDocStatus").empty();
    $("#EduDocStatus").append('<option value="0">Select Status</option>');

    $("#CasDocStatus").empty();
    $("#CasDocStatus").append('<option value="0">Select Status</option>');

    $("#RationDocStatus").empty();
    $("#RationDocStatus").append('<option value="0">Select Status</option>');

    $("#IncCerDocStatus").empty();
    $("#IncCerDocStatus").append('<option value="0">Select Status</option>');

    $("#UIDDocStatus").empty();
    $("#UIDDocStatus").append('<option value="0">Select Status</option>');

    $("#RcerDocStatus").empty();
    $("#RcerDocStatus").append('<option value="0">Select Status</option>');

    $("#KanMedCerDocStatus").empty();
    $("#KanMedCerDocStatus").append('<option value="0">Select Status</option>');

    $("#DiffAblDocStatus").empty();
    $("#DiffAblDocStatus").append('<option value="0">Select Status</option>');

    $("#ExCerDocStatus").empty();
    $("#ExCerDocStatus").append('<option value="0">Select Status</option>');

    $("#HyKarDocStatus").empty();
    $("#HyKarDocStatus").append('<option value="0">Select Status</option>');

    $("#HorKanDocStatus").empty();
    $("#HorKanDocStatus").append('<option value="0">Select Status</option>');

    $("#OtherCerDocStatus").empty();
    $("#OtherCerDocStatus").append('<option value="0">Select Status</option>');

    $("#KasMigDocStatus").empty();
    $("#KasMigDocStatus").append('<option value="0">Select Status</option>');

    $("#ExserDocStatus").empty();
    $("#ExserDocStatus").append('<option value="0">Select Status</option>');

    $("#LLCerDocStatus").empty();
    $("#LLCerDocStatus").append('<option value="0">Select Status</option>');

    $("#EWSDocStatus").empty();
    $("#EWSDocStatus").append('<option value="0">Select Status</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetDocumentApplicationStatus',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#EduDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#CasDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#RationDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#IncCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#UIDDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#RcerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#KanMedCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#DiffAblDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#ExCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#HyKarDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#HorKanDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#OtherCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#KasMigDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#ExserDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#LLCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    $("#EWSDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });


}

function GetApplicationDetailsById(ApplicationId, clickFrom) {
  
    //$('#myModal').modal('show');
    $('input[type="text"], textarea').attr('readonly', false);
    //$(".EditOptionShowGrid").find("*").prop('disabled', true);
    fnDisplayApplicantDetailScreen(ApplicationId);

    $(".EditOptionShowGrid").show();
    $("#EditSubmitbtn").show();
    if ($(".tab-pane.active").attr("id") == "tab_1") {
        $(".clsVerifyDocAppl").hide();
    }

    //$("#UpdateDiv").show();
    clearallErrorFields();
    //$("#PaymentGenerationGridCls").hide();
    if (clickFrom == "1") {
        //$('input[type="text"], textarea').attr('readonly', 'readonly');
        //$("#UpdateDiv").hide();
    }
    
    //GetApplicantFeeDetailsById(ApplicationId);
    RuralUrbanLocation();
    AppliedWhichBasics();
    AppliedForSyallbus();

    $("#Category").empty();
    $("#Category").append('<option value="0">Select Category</option>');

    $("#Religion").empty();
    $("#Religion").append('<option value="0">Select Religion</option>');

    $("#Gender").empty();
    $("#Gender").append('<option value="0">Select Gender</option>');

    $("#ApplicantType").empty();
    $("#ApplicantType").append('<option value="0">Select Applicant</option>');

    $("#Qualification").empty();
    $("#Qualification").append('<option value="0">Select Qualification</option>');

    $("#Districts").empty();
    $("#Districts").append('<option value="0">Select</option>');

    $("#PermanentDistricts").empty();
    $("#PermanentDistricts").append('<option value="0">Select</option>');

    $("#PhysicallyHanidcapType").empty();
    $("#PhysicallyHanidcapType").append('<option value="0">Select Disability</option>');
    $("#ErrorWithoutStatusMsg").hide();
    $.ajax({
        type: 'Get',
        data: { CredatedBy: ApplicationId },
        url: '/Admission/GetApplicationDetailsById',
        success: function (datajson) {

            var sessionValue = $("#hdnSession").data('value');

            $(".EduFileAttach").hide(); $(".CasteFileAttach").hide(); $(".RationFileAttach").hide(); $(".IncomeCertificateAttach").hide();
            $(".UIDFileAttach").hide(); $(".RuralcertificateAttach").hide(); $(".KannadamediumCertificateAttach").hide(); $(".DifferentlyabledcertificateAttach").hide();
            $(".ExemptedCertificateAttach").hide(); $(".HyderabadKarnatakaRegionAttach").hide(); $(".HoranaaduKannadigaAttach").hide(); $(".OtherCertificatesAttach").hide();
            $(".KashmirMigrantsAttach").hide(); $(".ExservicemanAttach").hide(); $(".LLCertificateAttach").hide(); $(".EWSCertificateAttach").hide();

            $("#txtEduCertRemarks").val('');
            $("#txtCasteCertRemarks").val('');
            $("#txtRationCardRemarks").val('');
            $("#txtIncCertRemarks").val('');
            $("#txtUIDRemarks").val('');
            $("#txtRurCertRemarks").val('');
            $("#txtKanMedRemarks").val('');
            $("#txtDiffAbledCertRemarks").val('');
            $("#txtExeCertRemarks").val('');
            $("#txtHydKarnRemarks").val('');
            $("#txtHorGadKannadigaRemarks").val('');
            $("#txtOtherCertRemarks").val('');
            $("#txtKashmirMigrantsRemarks").val('');
            $("#txtExservicemanRemarks").val('');
            $("#txtLLCertificateRemarks").val('');
            $("#txtEWSCertificateRemarks").val('');
            
            //if (datajson.Resultlist.DocumentFeeReceiptDetails != null) {

            //}
            //else {

            //}
            if (datajson.Resultlist != null || datajson.Resultlist != '') {
                if (datajson.Resultlist.ApplDescription == 3 || datajson.Resultlist.ApplDescription == 4) {
                    $("#pymntdiv").hide();
                } else { $("#pymntdiv").show(); }
                $("#ApplicantId").val(datajson.Resultlist.ApplicationId);
                $("#ApplCredatedBy").val(datajson.Resultlist.CredatedBy);
                $("#ApplFlowId").val(datajson.Resultlist.FlowId);
                //ApplicantDetails
                //Get the roll number
                $("input[name=RStateBoardType][value=" + datajson.Resultlist.RStateBoardType + "]").prop('checked', true);
                $("input[name=RAppBasics][value=" + datajson.Resultlist.RAppBasics + "]").prop('checked', true);
                $("#txtRollNumber").val(datajson.Resultlist.RollNumber);
                OnChangeStateBoardType();
                OnChangeGetAppliedBasicsForBoardType();

                if (datajson.Resultlist.GetReligionList.length > 0) {
                    $.each(datajson.Resultlist.GetReligionList, function (index, item) {
                        $("#Religion").append($("<option/>").val(this.Religion_Id).text(this.Religion));
                    });
                }

                if (datajson.Resultlist.GetGenderList.length > 0) {
                    $.each(datajson.Resultlist.GetGenderList, function (index, item) {
                        $("#Gender").append($("<option/>").val(this.Gender_Id).text(this.Gender));
                    });
                }

                if (datajson.Resultlist.GetCategoryList.length > 0) {
                    $.each(datajson.Resultlist.GetCategoryList, function (index, item) {
                        $("#Category").append($("<option/>").val(this.CategoryId).text(this.Category));
                    });
                }

                if (datajson.Resultlist.GetApplicantTypeList.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantTypeList, function (index, item) {
                        $("#ApplicantType").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                    });
                }

                if (datajson.Resultlist.GetCasteList.length > 0) {
                    $.each(datajson.Resultlist.GetCasteList, function (index, item) {
                        $("#txtCaste").append($("<option/>").val(this.CasteId).text(this.Caste));
                    });
                }

                $("#EduDocStatus").empty();
                $("#EduDocStatus").append('<option value="0">Select Status</option>');

                $("#CasDocStatus").empty();
                $("#CasDocStatus").append('<option value="0">Select Status</option>');

                $("#RationDocStatus").empty();
                $("#RationDocStatus").append('<option value="0">Select Status</option>');

                $("#IncCerDocStatus").empty();
                $("#IncCerDocStatus").append('<option value="0">Select Status</option>');

                $("#UIDDocStatus").empty();
                $("#UIDDocStatus").append('<option value="0">Select Status</option>');

                $("#RcerDocStatus").empty();
                $("#RcerDocStatus").append('<option value="0">Select Status</option>');

                $("#KanMedCerDocStatus").empty();
                $("#KanMedCerDocStatus").append('<option value="0">Select Status</option>');

                $("#DiffAblDocStatus").empty();
                $("#DiffAblDocStatus").append('<option value="0">Select Status</option>');

                $("#ExCerDocStatus").empty();
                $("#ExCerDocStatus").append('<option value="0">Select Status</option>');

                $("#HyKarDocStatus").empty();
                $("#HyKarDocStatus").append('<option value="0">Select Status</option>');

                $("#HorKanDocStatus").empty();
                $("#HorKanDocStatus").append('<option value="0">Select Status</option>');

                $("#OtherCerDocStatus").empty();
                $("#OtherCerDocStatus").append('<option value="0">Select Status</option>');

                $("#KasMigDocStatus").empty();
                $("#KasMigDocStatus").append('<option value="0">Select Status</option>');

                $("#ExserDocStatus").empty();
                $("#ExserDocStatus").append('<option value="0">Select Status</option>');

                $("#LLCerDocStatus").empty();
                $("#LLCerDocStatus").append('<option value="0">Select Status</option>');

                $("#EWSDocStatus").empty();
                $("#EWSDocStatus").append('<option value="0">Select Status</option>');

                if (datajson.Resultlist.GetDocumentApplicationStatus.length > 0) {
                    $.each(datajson.Resultlist.GetDocumentApplicationStatus, function (index, item) {
                        $("#EduDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#CasDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#RationDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#IncCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#UIDDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#RcerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#KanMedCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#DiffAblDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#ExCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#HyKarDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#HorKanDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#OtherCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#KasMigDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#ExserDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#LLCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#EWSDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    });
                }

                $.each(datajson.Resultlist.GetReservationList, function (index, item) {
                    $("#ApplicableReservations").append($("<option/>").val(this.ReservationId).text(this.Reservations));
                });

                var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;
                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        $("#ApplicableReservations option[value='" + e + "']").prop("selected", true);
                    });
                }
                $('#ApplicableReservations').multiselect({});

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#Districts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        $("#PermanentDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    });
                }

                $("#CasteCertificate").prop('disabled', true);
                $("#CasDocStatus").prop('disabled', true);
                $("#txtCasteCertRemarks").prop('disabled', true);

                $("#Rationcard").prop('disabled', true);
                $("#RationDocStatus").prop('disabled', true);
                $("#txtRationCardRemarks").prop('disabled', true);

                $("#Incomecertificate").prop('disabled', true);
                $("#IncCerDocStatus").prop('disabled', true);
                $("#txtIncCertRemarks").prop('disabled', true);

                $("#UIDNumber").prop('disabled', true);
                $("#UIDDocStatus").prop('disabled', true);
                $("#txtUIDRemarks").prop('disabled', true);

                $("#Ruralcertificate").prop('disabled', true);
                $("#RcerDocStatus").prop('disabled', true);
                $("#txtRurCertRemarks").prop('disabled', true);

                $("#KannadamediumCertificate").prop('disabled', true);
                $("#KanMedCerDocStatus").prop('disabled', true);
                $("#txtKanMedRemarks").prop('disabled', true);

                $("#Differentlyabledcertificate").prop('disabled', true);
                $("#DiffAblDocStatus").prop('disabled', true);
                $("#txtDiffAbledCertRemarks").prop('disabled', true);

                $("#ExemptedCertificate").prop('disabled', true);
                $("#ExCerDocStatus").prop('disabled', true);
                $("#txtExeCertRemarks").prop('disabled', true);

                $("#HyderabadKarnatakaRegion").prop('disabled', true);
                $("#HyKarDocStatus").prop('disabled', true);
                $("#txtHydKarnRemarks").prop('disabled', true);

                $("#HoranaaduKannadiga").prop('disabled', true);
                $("#HorKanDocStatus").prop('disabled', true);
                $("#txtHorGadKannadigaRemarks").prop('disabled', true);

                $("#Exserviceman").prop('disabled', true);
                $("#ExserDocStatus").prop('disabled', true);
                $("#txtExservicemanRemarks").prop('disabled', true);

                $("#EWSCertificate").prop('disabled', true);
                $("#EWSDocStatus").prop('disabled', true);
                $("#txtEWSCertificateRemarks").prop('disabled', true);              

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {
                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach").show();
                                $('#aEduCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#EduCertificate").prop('disabled', true);
                                    $("#EduDocStatus").prop('disabled', true);
                                    $("#txtEduCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#EduCertificate").prop('disabled', false);
                                    $("#EduDocStatus").prop('disabled', false);
                                    $("#txtEduCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#EDocAppId").val(this.DocAppId);
                            $("#ECreatedBy").val(this.CreatedBy);
                            $("#EduDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocEduCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocEduCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach").show();
                                $('#aCasteCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#CasteCertificate").prop('disabled', true);
                                    $("#CasDocStatus").prop('disabled', true);
                                    $("#txtCasteCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#CasteCertificate").prop('disabled', false);
                                    $("#CasDocStatus").prop('disabled', false);
                                    $("#txtCasteCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#CDocAppId").val(this.DocAppId);
                            $("#CCreatedBy").val(this.CreatedBy);
                            $("#CasDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocCasCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocCasCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach").show();
                                $('#aRationCard').attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#Rationcard").prop('disabled', true);
                                    $("#RationDocStatus").prop('disabled', true);
                                    $("#txtRationCardRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#Rationcard").prop('disabled', false);
                                    $("#RationDocStatus").prop('disabled', false);
                                    $("#txtRationCardRemarks").prop('disabled', false);
                                }
                            }
                            $("#RDocAppId").val(this.DocAppId);
                            $("#RCreatedBy").val(this.CreatedBy);
                            $("#RationDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocRatCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocRatCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach").show();
                                $('#aIncomeCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#Incomecertificate").prop('disabled', true);
                                    $("#IncCerDocStatus").prop('disabled', true);
                                    $("#txtIncCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#Incomecertificate").prop('disabled', false);
                                    $("#IncCerDocStatus").prop('disabled', false);
                                    $("#txtIncCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#IDocAppId").val(this.DocAppId);
                            $("#ICreatedBy").val(this.CreatedBy);
                            $("#IncCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocIncCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocIncCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach").show();
                                $('#aUIDNumber').attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#UIDNumber").prop('disabled', true);
                                    $("#UIDDocStatus").prop('disabled', true);
                                    $("#txtUIDRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#UIDNumber").prop('disabled', false);
                                    $("#UIDDocStatus").prop('disabled', false);
                                    $("#txtUIDRemarks").prop('disabled', false);
                                }
                            }
                            $("#UDocAppId").val(this.DocAppId);
                            $("#UCreatedBy").val(this.CreatedBy);
                            $("#UIDDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocUIDCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocUIDCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 6) {
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach").show();
                                $('#aRuralcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#Ruralcertificate").prop('disabled', true);
                                    $("#RcerDocStatus").prop('disabled', true);
                                    $("#txtRurCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#Ruralcertificate").prop('disabled', false);
                                    $("#RcerDocStatus").prop('disabled', false);
                                    $("#txtRurCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#RUDocAppId").val(this.DocAppId);
                            $("#RUCreatedBy").val(this.CreatedBy);
                            $("#RcerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocRurCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocRurCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 7) {
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach").show();
                                $('#aKannadamediumCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#KannadamediumCertificate").prop('disabled', true);
                                    $("#KanMedCerDocStatus").prop('disabled', true);
                                    $("#txtKanMedRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#KannadamediumCertificate").prop('disabled', false);
                                    $("#KanMedCerDocStatus").prop('disabled', false);
                                    $("#txtKanMedRemarks").prop('disabled', false);
                                }
                            }
                            $("#KDocAppId").val(this.DocAppId);
                            $("#KCreatedBy").val(this.CreatedBy);
                            $("#KanMedCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocKanMedCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocKanMedCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach").show();
                                $('#aDifferentlyabledcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#Differentlyabledcertificate").prop('disabled', true);
                                    $("#DiffAblDocStatus").prop('disabled', true);
                                    $("#txtDiffAbledCertRemarks").prop('disabled', true);
                                    $(".PhysicallyHanidcapInd").prop('disabled', true);
                                }
                                else {
                                    $("#Differentlyabledcertificate").prop('disabled', false);
                                    $("#DiffAblDocStatus").prop('disabled', false);
                                    $("#txtDiffAbledCertRemarks").prop('disabled', false);
                                    $(".PhysicallyHanidcapInd").prop('disabled', false);
                                }
                            }
                            $("#DDocAppId").val(this.DocAppId);
                            $("#DCreatedBy").val(this.CreatedBy);
                            $("#DiffAblDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocDidAblCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocDidAblCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 9) {
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach").show();
                                $('#aExemptedCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#ExemptedCertificate").prop('disabled', true);
                                    $("#ExCerDocStatus").prop('disabled', true);
                                    $("#txtExeCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#ExemptedCertificate").prop('disabled', false);
                                    $("#ExCerDocStatus").prop('disabled', false);
                                    $("#txtExeCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#ExDocAppId").val(this.DocAppId);
                            $("#ExCreatedBy").val(this.CreatedBy);
                            $("#ExCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocExStuCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocExStuCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 10) {
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach").show();
                                $('#aHyderabadKarnatakaRegion').attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#HyderabadKarnatakaRegion").prop('disabled', true);
                                    $("#HyKarDocStatus").prop('disabled', true);
                                    $("#txtHydKarnRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#HyderabadKarnatakaRegion").prop('disabled', false);
                                    $("#HyKarDocStatus").prop('disabled', false);
                                    $("#txtHydKarnRemarks").prop('disabled', false);
                                }
                            }
                            $("#HDocAppId").val(this.DocAppId);
                            $("#HCreatedBy").val(this.CreatedBy);
                            $("#HyKarDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocHyKarRegAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocHyKarRegRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 11) {
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach").show();
                                $('#aHoranaaduKannadiga').attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#HoranaaduKannadiga").prop('disabled', true);
                                    $("#HorKanDocStatus").prop('disabled', true);
                                    $("#txtHorGadKannadigaRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#HoranaaduKannadiga").prop('disabled', false);
                                    $("#HorKanDocStatus").prop('disabled', false);
                                    $("#txtHorGadKannadigaRemarks").prop('disabled', false);
                                }
                            }
                            $("#HGKDocAppId").val(this.DocAppId);
                            $("#HGKCreatedBy").val(this.CreatedBy);
                            $("#HorKanDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocHorGadKanAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocHorGadKanRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 12) {
                            if (this.FilePath != null) {
                                $(".OtherCertificatesAttach").show();
                                $('#aOtherCertificates').attr('href', '' + this.FilePath + '');
                                $("#txtOtherCertRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#OtherCertificates").prop('disabled', true);
                                    $("#OtherCerDocStatus").prop('disabled', true);
                                    $("#txtOtherCertRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#OtherCertificates").prop('disabled', false);
                                    $("#OtherCerDocStatus").prop('disabled', false);
                                    $("#txtOtherCertRemarks").prop('disabled', false);
                                }
                            }
                            $("#ODocAppId").val(this.DocAppId);
                            $("#OCreatedBy").val(this.CreatedBy);
                            $("#OtherCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocOthCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocOthCerRejectedImg").show();
                        }                        
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach").show();
                                $('#aExserviceman').attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#Exserviceman").prop('disabled', true);
                                    $("#ExserDocStatus").prop('disabled', true);
                                    $("#txtExservicemanRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#Exserviceman").prop('disabled', false);
                                    $("#ExserDocStatus").prop('disabled', false);
                                    $("#txtExservicemanRemarks").prop('disabled', false);
                                }
                            }
                            $("#ExSDocAppId").val(this.DocAppId);
                            $("#ExSCreatedBy").val(this.ExSCreatedBy);
                            $("#ExserDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocExSerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocExSerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach").show();
                                $('#aEWSCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks").val(this.Remarks);
                                if (this.Verified == 15) {
                                    $("#EWSCertificate").prop('disabled', true);
                                    $("#EWSDocStatus").prop('disabled', true);
                                    $("#txtEWSCertificateRemarks").prop('disabled', true);
                                }
                                else {
                                    $("#EWSCertificate").prop('disabled', false);
                                    $("#EWSDocStatus").prop('disabled', false);
                                    $("#txtEWSCertificateRemarks").prop('disabled', false);
                                }
                            }
                            $("#EWSDocAppId").val(this.DocAppId);
                            $("#EWSCreatedBy").val(this.EWSCreatedBy);
                            $("#EWSDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocEWSAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocEWSRejectedImg").show();
                        }
                    });
                }
                $("#ApplStatus").val(datajson.Resultlist.ApplStatus);
                $("#academicyear1").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                $("#txtAadhaarNumber").val(datajson.Resultlist.AadhaarNumber);
                $("#AccountNumber").val(datajson.Resultlist.AccountNumber);
                $("#BankName").val(datajson.Resultlist.BankName);
                $("#IFSCCode").val(datajson.Resultlist.IFSCCode);
                $("#txtRationCard").val(datajson.Resultlist.RationCard);
                if (datajson.Resultlist.PhysicallyHanidcapInd == true) {
                    $("input[name=PhysicallyHanidcapInd][value=1]").prop('checked', true);
                    $("#DiffAbledCer").val(1);
                }
                else
                    $("input[name=PhysicallyHanidcapInd][value=0]").prop('checked', true);
                PhysicallyHanidcapEna(PhysicallyHanidcapYes);

                if (datajson.Resultlist.PersonWithDisabilityCategory.length > 0) {
                    $.each(datajson.Resultlist.PersonWithDisabilityCategory, function (index, item) {
                        $("#PhysicallyHanidcapType").append($("<option/>").val(this.PersonWithDisabilityCategoryId).text(this.DisabilityName));
                    });
                }

                if (datajson.Resultlist.PhysicallyHanidcapType == "" || datajson.Resultlist.PhysicallyHanidcapType == null)
                    $("#PhysicallyHanidcapType").val(0);
                else
                    $("#PhysicallyHanidcapType").val(datajson.Resultlist.PhysicallyHanidcapType);

                if (datajson.Resultlist.HoraNadu_GadiNadu_Kannidagas == true) {
                    $("input[name=HoraNadu][value=1]").prop('checked', true);
                    $("#HoraNaduCer").val(1);
                }
                else {
                    $("input[name=HoraNadu][value=0]").prop('checked', true);
                }
                    

                if (datajson.Resultlist.ExemptedFromStudyCertificate == true) {
                    $("input[name=ExemptedFromStudyCertificate][value=1]").prop('checked', true);
                    $("#ExeStuCer").val(1);
                }
                else
                    $("input[name=ExemptedFromStudyCertificate][value=0]").prop('checked', true);

                if (datajson.Resultlist.HyderabadKarnatakaRegion == true) {
                    $("input[name=HyderabadKarnatakaRegion][value=1]").prop('checked', true);
                    $("#HydKarCer").val(1);
                }
                else
                    $("input[name=HyderabadKarnatakaRegion][value=0]").prop('checked', true);

                if (datajson.Resultlist.KanndaMedium == true) {
                    $("input[name=KanndaMedium][value=1]").prop('checked', true);
                    $("#KanMedCer").val(1);
                }
                else
                    $("input[name=KanndaMedium][value=0]").prop('checked', true);

                if (datajson.Resultlist.EconomyWeakerSection == true) {
                    $("input[name=EconomicallyWeakerSections][value=1]").prop('checked', true);
                }
                else
                    $("input[name=EconomicallyWeakerSections][value=0]").prop('checked', true);

                if (datajson.Resultlist.ExServiceMan == true) {
                    $("input[name=ExService][value=1]").prop('checked', true);
                }
                else
                    $("input[name=ExService][value=0]").prop('checked', true);

                $("#txtApplicantName").val(datajson.Resultlist.ApplicantName);
                $("#txtFathersName").val(datajson.Resultlist.FathersName);
                $("#txtParentOccupation").val(datajson.Resultlist.ParentsOccupation);
                $('#ImgPhotoUpload').attr("src", datajson.Resultlist.Photo);
                $("#IsUploaded").val(datajson.Resultlist.Photo);

                var finalDOB = daterangeformate2(datajson.Resultlist.DOB, 1);
                $("#dateofbirth").val(finalDOB);
                $("#txtMothersName").val(datajson.Resultlist.MothersName);
                $("#Religion").val(datajson.Resultlist.Religion);
                $('#Gender').val(datajson.Resultlist.Gender);
                $("#Category").val(datajson.Resultlist.Category);
                $("#MinorityCategory").val(datajson.Resultlist.MinorityCategory);
                if (datajson.Resultlist.Caste != null)
                    $("#txtCaste").val(datajson.Resultlist.Caste);
                $("#txtFamilyAnnualIncome").val(datajson.Resultlist.FamilyAnnIncome);
                $("#ApplicantType").val(datajson.Resultlist.ApplicantType);

                //EducationDetails
                if (datajson.Resultlist.GetQualificationList.length > 0) {
                    $.each(datajson.Resultlist.GetQualificationList, function (index, item) {
                        $("#Qualification").append($("<option/>").val(this.QualificationId).text(this.Qualification));
                    });
                }
                $('#Qualification').val(datajson.Resultlist.Qualification);
                $("input[name=RuralUrban][value=" + datajson.Resultlist.ApplicantBelongTo + "]").prop('checked', true);
                $("input[name=AppBasics][value=" + datajson.Resultlist.AppliedBasic + "]").prop('checked', true);
                $("input[name=TenthBoard][value=" + datajson.Resultlist.TenthBoard + "]").prop('checked', true);
                //$("#txtEduGrade").val(datajson.Resultlist.EducationGrade);
                $("input[name=TenthCOBSEBoard][value=" + datajson.Resultlist.TenthCOBSEBoard + "]").prop('checked', true);
                TenthBoardStateType();
                $("#txtInstituteStudied").val(datajson.Resultlist.InstituteStudiedQual);
                $("#txtMaximumMarks").val(datajson.Resultlist.MaxMarks);
                $("#txtMinMarks").val(datajson.Resultlist.MinMarks);
                $("#txtMarksObtained").val(datajson.Resultlist.MarksObtained);
                if (datajson.Resultlist.Percentage != null && datajson.Resultlist.Percentage != "")
                    $("#lblPercAsPerMarks").text(datajson.Resultlist.Percentage + "%");
                $("#Results").val(datajson.Resultlist.ResultQual);
                if (datajson.Resultlist.studiedMathsScience == true)
                    $("input[name=studiedMathsScience][value=1]").prop('checked', true);
                else
                    $("input[name=studiedMathsScience][value=0]").prop('checked', true);
                //AddressDetails
                $("#txtCommunicationAddress").val(datajson.Resultlist.CommunicationAddress);
                $("#Districts").val(datajson.Resultlist.DistrictId);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#Talukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }

                $('#Talukas').val(datajson.Resultlist.TalukaId);
                $("#txtPincode").val(datajson.Resultlist.Pincode);
                $("input[name=chkSameAsCommunicationAddress][value=" + datajson.Resultlist.SameAdd + "]").prop('checked', true);
                if (datajson.Resultlist.SameAdd == 1 || datajson.Resultlist.SameAdd == true)
                    $("#chkSameAsCommunicationAddress").prop('checked', true);
                else
                    $("#chkSameAsCommunicationAddress").prop('checked', false);

                OnchangechkSameAsCommunicationAddress();
                $("#txtPermanentAddress").val(datajson.Resultlist.PermanentAddress);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#PermanentDistricts").append($("<option/>").val(item.district_lgd_code).text(item.district_ename));
                    });
                }
                $("#PermanentTalukas").empty();
                $("#PermanentTalukas").append('<option value="0">Select</option>');
                $("#PermanentDistricts").val(datajson.Resultlist.PDistrict);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }
                $('#PermanentTalukas').val(datajson.Resultlist.PTaluk);
                $("#txtPermanentPincode").val(datajson.Resultlist.PPinCode);
                $("#txtApplicantPhoneNumber").val(datajson.Resultlist.PhoneNumber);
                $("#txtFathersPhoneNumber").val(datajson.Resultlist.FatherPhoneNumber);
                $("#txtEmailId").val(datajson.Resultlist.EmailId);
                $("#txtRemarks").val('');

                if ($("#CalenderNotificationEligibleInd").val() == 1) {

                    $("#EligibleVerificationForm").hide();
                    if (datajson.Resultlist.ApplStatus == 10) {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                        $("#ToShowMsgToVOApproved").show();
                        $("#ToShowMsgToVOGriApplied").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 11) {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").hide();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").show();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 12) {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").show();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 13) {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").hide();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").show();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 3) {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").hide();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").show();
                    }
                    else if (sessionValue == datajson.Resultlist.FlowId) {
                        $("#saveDocumentsRemarksDetDiv").show();
                        $("#ToShowMsgToVOGriApplToShowMsgToVOied").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                    else {
                        $("#saveDocumentsRemarksDetDiv").hide();
                        $("#ToShowMsgToVO").show();
                        $("#ToShowMsgToVOApproved").hide();
                        $("#ToShowMsgToVOGriApplied").hide();
                        $("#ToShowMsgToVOGriAppliedApproved").hide();
                        $("#ToShowMsgToVOGriAppliedRejected").hide();
                    }
                }
                else {
                    $("#EligibleVerificationForm").show();
                    $("#saveDocumentsRemarksDetDiv").hide();
                    $("#ToShowMsgToVO").hide();
                    $("#ToShowMsgToVOApproved").hide();
                    $("#ToShowMsgToVOGriApplied").hide();
                    $("#ToShowMsgToVOGriAppliedApproved").hide();
                    $("#ToShowMsgToVOGriAppliedRejected").hide();
                }
            }

            $.ajax({
                type: "GET",
                url: "/Admission/GetApplicantDocumentFeeDetails",
                data: { ApplicationId: ApplicationId },
               // contentType: "application/json",
                success: function (datajsonDetails) {
                    
                    $.each(datajsonDetails, function (index, item) {

                       
                        //$(".PaymentGenerationGridCls").hide();
                        //$("#AllocationFeePay").show();
                        //$(".AdmFeePaidStatus").prop('disabled', false);
                        //$("#PaymentDate").prop('disabled', false);
                        if (this.DocumentFeeReceiptDetails != null) {
                            //  $(".AdmFeePaidStatus").prop('disabled', true);
                            // $("#PaymentDate").prop('disabled', true);
                            $(".PaymentGenerationGridCls").show();
                            $("#pymntdiv").hide();
                            // $("#AllocationFeePay").hide();
                            /// $("input[name=AdmittedStatus][value=6]").prop('checked', true);
                            //fnShowHidePaymentInfo();

                            $('#PaymentGenerationGrid').DataTable({
                                data: datajsonDetails,
                                "destroy": true,
                                "bSort": false,
                                sDom: 'lrtip',
                                "bPaginate": false,
                                "bInfo": false,
                                searching: false,
                                columns: [

                                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },

                                    //{
                                    //    'data': 'DocVeriFeePymtDate', 'title': 'Payment Date', 'className': 'text - left DOB',
                                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    //        var date = daterangeformate2(oData.DocVeriFeePymtDate, 2);
                                    //        $(nTd).html(date);
                                    //    }
                                    //},
                                    { 'data': 'DocVeriFeePymtDatestring', 'title': 'Payment Date', 'className': 'text - left DOB' },

                                    { 'data': 'DocumentFeeReceiptDetails', 'title': 'Receipt Number', 'className': 'text-center' },
                                    { 'data': 'TreasuryReceiptNo', 'title': 'Treasury Receipt No.', 'className': 'text-center' },
                                    {
                                        'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                            if (oData.PaymentOptionval == true) {
                                                $(nTd).html("Online");
                                            }
                                            else {
                                                $(nTd).html("Offline");
                                            }
                                        }
                                    },
                                    { 'data': 'DocVeriFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
                                    {
                                        'title': 'Receipt (PDF)',
                                        render: function (data, type, row) {
                                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                        }
                                    },
                                    //{ 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
                                ]
                            });
                        }
                        
                    });
                }
            });
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

//function GetApplicantFeeDetailsById(ApplicationId)
//{
//   // $(".PaymentGenerationGridCls").hide();
//    
//    $.ajax({
//        type: "GET",
//        url: "/Admission/GetApplicantDocumentFeeDetails",
//        data: { ApplicationId: ApplicationId },
//        contentType: "application/json",
//        success: function (datajsonDetails) {
//            
//            $.each(datajsonDetails.objInstituteWiseAdmission, function (index, item) {

//                //Allocation and Fee Details
//                $("#ApplicationNumber").text(this.ApplicantNumber);
//                $("#ApplicationName").text(this.ApplicantName);
//                $("#RankNumber").text(this.ApplicantRank);
//                $("#DivisionId").text(this.DivisionId);
//                $("#DivisionName").text(this.DivisionName);
//                $("#DistrictCd").text(this.DistrictId);
//                $("#DistrictName").text(this.DistrictName);
//                $("#TalukId").text(this.TalukId);
//                $("#TalukName").text(this.TalukName);
//                $("#MISCode").text(this.MISCode);
//                $("#InstituteName").text(this.InstituteName);
//                $("#InstituteType").text(this.InstituteType);
//                $("#VerticalCategory").text(this.VerticalCategory);
//                $("#HorizontalCategory").text(this.HorizontalCategory);
//                $("#TradeId").text(this.TradeCode);
//                $("#TradeName").text(this.TradeName);
//                $("#Units").text(this.Units);
//                $("#Shifts").text(this.Shifts);
//                $("#AllocationId").val(this.AllocationId);
//                $("#AdmissionFee").text(this.AdmisionFee);

//                if (this.DualType == 1)
//                    $("input[name=DualType][value=1]").prop('checked', true);
//                else
//                    $("input[name=DualType][value=0]").prop('checked', true);

//                //$(".PaymentGenerationGridCls").hide();
//                //$("#AllocationFeePay").show();
//                //$(".AdmFeePaidStatus").prop('disabled', false);
//                //$("#PaymentDate").prop('disabled', false);
//                if (this.DocumentFeeReceiptDetails == 1) {
//                  //  $(".AdmFeePaidStatus").prop('disabled', true);
//                   // $("#PaymentDate").prop('disabled', true);
//                    //$(".PaymentGenerationGridCls").show();
//                   // $("#AllocationFeePay").hide();
//                   /// $("input[name=AdmittedStatus][value=6]").prop('checked', true);
//                    //fnShowHidePaymentInfo();

//                    $('#PaymentGenerationGrid').DataTable({
//                        data: datajsonDetails.objInstituteWiseAdmission,
//                        "destroy": true,
//                        "bSort": false,
//                        sDom: 'lrtip',
//                        "bPaginate": false,
//                        "bInfo": false,
//                        searching: false,
//                        columns: [
//                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-center' },
//                            { 'data': 'AdmisionFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
//                            {
//                                'data': 'PaymentDate', 'title': 'Payment Date', 'className': 'text - left DOB',
//                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                                    var date = daterangeformate2(oData.PaymentDate, 2);
//                                    $(nTd).html(date);
//                                }
//                            },
//                            { 'data': 'ReceiptNumber', 'title': 'Receipt Number', 'className': 'text-center' },
//                            { 'data': 'PaymentMode', 'title': 'Payment Mode', 'className': 'text-center' },
//                            { 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
//                        ]
//                    });
//                }

//                //if (this.PaymentDate != null)
//                //    $("#SaveSeatAllocationFeePay").prop("disabled", true);
//                //else
//                //    $("#SaveSeatAllocationFeePay").prop("disabled", false);

//                var AdmisionTime = daterangeformate2(this.AdmisionTime, 1);
//                
//                if (AdmisionTime == "") {
//                    AdmisionTime = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
//                }
//                $("#AdmisionTime").val(AdmisionTime);

//                if (this.ITIUnderPPP == 1)
//                    $("input[name=ITIUnderPPP][value=1]").prop('checked', true);
//                else
//                    $("input[name=ITIUnderPPP][value=0]").prop('checked', true);

//                if (this.AdmFeePaidStatus == 1) {
//                    $("input[name=AdmFeePaidStatus][value=1]").prop('checked', true);
//                    $("input[name=AdmittedStatus][value=3]").prop('disabled', true);
//                }
//                else
//                    $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);

//                var PaymentDate = daterangeformate2(this.PaymentDate, 1);
//                if (PaymentDate == "") {
//                    PaymentDate = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
//                }
//                $("#PaymentDate").val(PaymentDate);

//                if (this.ReceiptNumber == "" || this.ReceiptNumber == null)
//                    $("#ReceiptNumber").text("NIL");
//                else
//                    $("#ReceiptNumber").text(this.ReceiptNumber);

//                $("#lblToShowPendingStatus").hide();
//                if (this.AdmittedStatus == 2)
//                    $("input[name=AdmittedStatus][value=2]").prop('checked', true);
//                else if (this.AdmittedStatus == 1 || this.AdmittedStatus == 3) {
//                    if (this.AdmittedStatus == 1) {
//                        $("#lblToShowPendingStatus").show();
//                    }

//                }
//                $("#txtRemarks").val(this.Remarks);

//                if (this.AdmissionRegistrationNumber == "" || this.AdmissionRegistrationNumber == null)
//                    $("#AdmissionRegistrationNumber").text("NIL");
//                else
//                    $("#AdmissionRegistrationNumber").text(this.AdmissionRegistrationNumber);

//                if (this.RollNumber == "" || this.RollNumber == null)
//                    $("#RollNumber").text("NIL");
//                else
//                    $("#RollNumber").text(this.RollNumber);

//                if (this.StateRegistrationNumber == "" || this.StateRegistrationNumber == null)
//                    $("#StateRegistrationNumber").text("NIL");
//                else
//                    $("#StateRegistrationNumber").text(this.StateRegistrationNumber);
//            });
//        }
//    });
//}

function SaveDocumentsRemarksDet(updatedMsg) {

    var ApplicantId = $("#ApplicantId").val();
    if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null) {
        bootbox.alert("<br><br>Kindly Give the Applicant,Education, Address, Document Verification Center Information and Save the Details");
    }
    else {

        $("#ErrorWithoutStatusMsg").hide();
        var ApplicantId = $("#ApplicantId").val();
        var fileData = new FormData();
        var IsValid = true;
        FlowId = $("#ApplFlowId").val();
        CredatedBy = $("#ApplCredatedBy").val();

        var SaveAsDraft = 0;
       
        if (updatedMsg == 1) {
            ApplStatus = 10;    //Verified
            ReVerficationStatus = 0;
        }
        else if (updatedMsg == 2) {
            ApplStatus = 9;     //Sent for correction
            FlowId = $("#ApplCredatedBy").val();
            ReVerficationStatus = 1;
        }
        else {
            SaveAsDraft = 1;
            ApplStatus = $("#ApplStatus").val();     //Submitted
            ReVerficationStatus = 1;
        }

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
        $("#WithoutDocEduStatus-Required").hide();
        $("#WithoutDocEduRemarks-Required").hide();
        $("#WithoutDocEduStatusHold-Required").hide();
        var fileUpload = $('#EduCertificate').get(0);
        if ($('.aEduCertificate').is(':visible')) {
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
            if ($("#EduDocStatus :selected").val() == 0 || $("#EduDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#EduDocStatus :selected").val() == 14) {
                    $("#WithoutDocEduStatusHold-Required").show();
                   /* $("#ErrorWithoutStatusMsg").show();*/
                } else {
                  /*  $("#ErrorWithoutStatusMsg").show();*/
                    $("#WithoutDocEduStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#EduCertificate-Required").hide();
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
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
        $("#WithoutDocCasStatusHold-Required").hide();
        fileUpload = $('#CasteCertificate').get(0);
        if ($('.aCasteCertificate').is(':visible')) {
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
            if ($("#CasDocStatus :selected").val() == 0 || $("#CasDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#CasDocStatus :selected").val() == 14) {
                    $("#WithoutDocCasStatusHold-Required").show();
                } else {
                  /*   $("#ErrorWithoutStatusMsg").show();*/
                     $("#WithoutDocCasStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#CasteCertificate-Required").hide();
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
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
        $("#WithoutDocRatStatusHold-Required").hide();
        fileUpload = $('#Rationcard').get(0);
        if ($('.aRationCard').is(':visible')) {
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
            }
            if ($("#RationDocStatus :selected").val() == 0 || $("#RationDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#RationDocStatus :selected").val() == 14) {
                    $("#WithoutDocRatStatusHold-Required").show();
                   // $("#ErrorWithoutStatusMsg").show();
                } else {
                //$("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocRatStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#Rationcard-Required").hide();
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
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
        $("#WithoutDocIncStatusHold-Required").hide();
        fileUpload = $('#Incomecertificate').get(0);
        if ($('.aIncomeCertificate').is(':visible')) {
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
            if ($("#IncCerDocStatus :selected").val() == 0 || $("#IncCerDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#IncCerDocStatus :selected").val() == 14) {
                    $("#WithoutDocIncStatusHold-Required").show();
                   // $("#ErrorWithoutStatusMsg").show();
                } else {
                    $("#WithoutDocIncStatusHold-Required").hide();
                    //$("#ErrorWithoutStatusMsg").show();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#Incomecertificate-Required").hide();
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
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
        $("#WithoutDocUIDStatusHold-Required").hide();
        fileUpload = $('#UIDNumber').get(0);
        $("#UIDNumber-Required").hide();
        if ($('.aUIDNumber').is(':visible')) {
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
            if ($("#UIDDocStatus :selected").val() == 0 || $("#UIDDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#UIDDocStatus :selected").val() == 14) {
                    $("#WithoutDocUIDStatusHold-Required").show();
                   // $("#ErrorWithoutStatusMsg").show();
                } else {
                    $("#WithoutDocUIDStatusHold-Required").hide();
                    //$("#ErrorWithoutStatusMsg").show();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#UIDNumber-Required").hide();
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
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
        $("#WithoutDocRurStatus-Required").hide();
        $("#WithoutDocRurRemarks-Required").hide();
        $("#WithoutDocRurStatusHold-Required").hide();
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
            if ($("#RcerDocStatus :selected").val() == 0 || $("#RcerDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#RcerDocStatus :selected").val() == 14) {
                    $("#WithoutDocRurStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                   // $("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocRurStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#Ruralcertificate-Required").hide();
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
            else {
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
        $("#WithoutDocKMCStatusHold-Required").hide();
        if ($('.aKannadamediumCertificate').is(':visible')) {
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
            if ($("#KanMedCerDocStatus :selected").val() == 0 || $("#KanMedCerDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#KanMedCerDocStatus :selected").val() == 14) {
                    $("#WithoutDocKMCStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                   // $("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocKMCStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#KannadamediumCertificate-Required").hide();
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
            else {
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

        $(".PhysicallyHanidcapInd").prop('disabled', true);
        $("#WithoutDocDAStatus-Required").hide();
        $("#WithoutDocDAStatusHold-Required").hide();
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
                $(".PhysicallyHanidcapInd").prop('disabled', false);
                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
            }
            if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                $(".PhysicallyHanidcapInd").prop('disabled', false);
                $("#PersonWithDisability-Required").show();
                $("#PersonWithDisabilityUploadDoc-Required").show();
                $(".PhysicallyHanidcapInd").focus();
                IsValid = false;
            }
            if ($("#DiffAblDocStatus :selected").val() == 0 || $("#DiffAblDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#DiffAblDocStatus :selected").val() == 14) {
                    $("#WithoutDocDAStatusHold-Required").show();
                   // $("#ErrorWithoutStatusMsg").show();
                } else {
                   // $("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocDAStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#Differentlyabledcertificate-Required").hide();
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
                $(".PhysicallyHanidcapInd").prop('disabled', false);
                if ($("#DiffAblDocStatus :selected").val() != 3 && ($("#PhysicallyHanidcapType :selected").val() == 0 || $("#PhysicallyHanidcapType :selected").val() == undefined)) {
                    $("#PersonWithDisability-Required").show();
                    $("#PersonWithDisabilityUploadDoc-Required").show();
                    $(".PhysicallyHanidcapInd").focus();
                    IsValid = false;
                }
            }
            else {
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
        $("#WithoutDocECStatusHold-Required").hide();
        $("#WithoutDocECRemarks-Required").hide();
        $("#ExemptedCertificate-Required").hide();
        if ($('.aExemptedCertificate').is(':visible')) {
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
            if ($("#ExCerDocStatus :selected").val() == 0 || $("#ExCerDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#ExCerDocStatus :selected").val() == 14) {
                    $("#WithoutDocECStatusHold-Required").show();
                   // $("#ErrorWithoutStatusMsg").show();
                } else {
                    $("#WithoutDocECStatusHold-Required").hide();
                    //$("#ErrorWithoutStatusMsg").show();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#ExemptedCertificate-Required").hide();
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
            else {
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
        $("#WithoutDocHKRStatusHold-Required").hide();
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
            if ($("#HyKarDocStatus :selected").val() == 0 || $("#HyKarDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#HyKarDocStatus :selected").val() == 14) {
                    $("#WithoutDocHKRStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                    $("#WithoutDocHKRStatusHold-Required").hide();
                    //$("#ErrorWithoutStatusMsg").show();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#HyderabadKarnatakaRegion-Required").hide();
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
            else {
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
        $("#WithoutDocHorKanStatusHold-Required").hide();
        $("#WithoutDocHorKanRemarks-Required").hide();
        $("#HoranaaduKannadiga-Required").hide();
        if ($('.aHoranaaduKannadiga').is(':visible')) {
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
            if ($("#HorKanDocStatus :selected").val() == 0 || $("#HorKanDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#HorKanDocStatus :selected").val() == 14) {
                    $("#WithoutDocHorKanStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                    //$("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocHorKanStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#HoranaaduKannadiga-Required").hide();
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
            else {
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
        $("#WithoutDocOCStatusHold-Required").hide();
        $("#OtherCertificates-Required").hide();
        $("#WithoutDocOCRemarks-Required").hide();
        if ($('.aOtherCertificates').is(':visible')) {
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
            }
            if ($("#OtherCerDocStatus :selected").val() == 0 || $("#OtherCerDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#OtherCerDocStatus :selected").val() == 14) {
                    $("#WithoutDocOCStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                   // $("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocOCStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#OtherCertificates-Required").hide();
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
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
        $("#WithoutDocExSerStatusHold-Required").hide();
        $("#WithoutDocExSerRemarks-Required").hide();
        if ($('.aExserviceman').is(':visible')) {
            if ($("#Exserviceman").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
            if ($("#ExserDocStatus :selected").val() == 0 || $("#ExserDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#ExserDocStatus :selected").val() == 14) {
                    $("#WithoutDocExSerStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                    //$("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocExSerStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
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
        $("#WithoutDocEWSStatusHold-Required").hide();
        $("#WithoutDocEWSRemarks-Required").hide();
        if ($('.aEWSCertificate').is(':visible')) {
            if ($("#EWSCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
            if ($("#EWSDocStatus :selected").val() == 0 || $("#EWSDocStatus :selected").val() == 14 && updatedMsg != 2 && updatedMsg != 0) {
                if ($("#EWSDocStatus :selected").val() == 14) {
                    $("#WithoutDocEWSStatusHold-Required").show();
                    //$("#ErrorWithoutStatusMsg").show();
                } else {
                    //$("#ErrorWithoutStatusMsg").show();
                    $("#WithoutDocEWSStatusHold-Required").hide();
                }
                $("#ErrorWithoutStatusMsg").show();
                IsValid = false;
            }
        }
        else {
            $("#EWSCertificate-Required").hide();
            if ($("#EWSCertificate").val() == "") {
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

        fileData.append(
            "DocumentLength", fileData.append.length
        );

        fileData.append(
            "ApplicantId", ApplicantId
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
        fileData.append(
            "ReVerficationStatus", ReVerficationStatus
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

        var txtRemarks1 = $("#txtRemarks").val();
       
        if (txtRemarks1 == "" && jQuery.trim(txtRemarks1).length == 0) {
            $("#txtRemarks-Required").show();
            $("#txtRemarks").focus();
            IsValid = false;
        }
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
            "VORemarks", $("#txtRemarks").val()
        );
        fileData.append(
            "VOApplStatus", ApplStatus
        );
        fileData.append(
            "FlowId", FlowId
        );
        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
            "SaveAsDraft", SaveAsDraft
        );
        alert(IsValid)
        if (IsValid == true) {
            bootbox.confirm({
                message: " <br><br>Are you sure to submit documents verification status ? ",
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
                callback: function (result) {
                    if (result == true) {
                    $.ajax({
                        type: "POST",
                        url: "/Admission/ApplicantDocumentDetails",
                        contentType: false,
                        processData: false,
                        data: fileData,
                        success: function (data) {
                            if (data) {

                                if (updatedMsg == 1) {
                                    bootbox.alert("<br><br>Verification Offier updated the applicant documents successfully");
                                }
                                else if (updatedMsg == 2) {
                                    bootbox.alert("<br><br>Application sent back to the applicant for correction");
                                }
                                GetDataToVerifyDocuments();
                                GetGrievanceStatus();
                                GetDocumentApplicationStatus();
                                //$("#myModal").modal("hide");

                            } else {
                                bootbox.alert("<br><br> There is error in your data, Cant able to update it!!");
                            }
                        }
                    });
                }
                }
            });
        /*code commented by sujit*/
            //bootbox.confirm("<br><br>Are you sure to submit documents verification status ?", function (confirmed) {
            //    if (confirmed) {
            //        $.ajax({
            //            type: "POST",
            //            url: "/Admission/ApplicantDocumentDetails",
            //            contentType: false,
            //            processData: false,
            //            data: fileData,
            //            success: function (data) {
            //                if (data) {

            //                    if (updatedMsg == 1) {
            //                        bootbox.alert("<br><br>Verification Offier updated the applicant documents successfully");
            //                    }
            //                    else if (updatedMsg == 2) {
            //                        bootbox.alert("<br><br>Application sent back to the applicant for correction");
            //                    }
            //                    GetDataToVerifyDocuments();
            //                    GetGrievanceStatus();
            //                    GetDocumentApplicationStatus();
            //                    $("#myModal").modal("hide");

            //                } else {
            //                    bootbox.alert("<br><br> There is error in your data, Cant able to update it!!");
            //                }
            //            }
            //        });
            //    }
            //});
        }
    }
}

$('.link').click(function (e) {
    e.preventDefault();
    var id = $(this).attr('id');
    var linktoRedirect = $("#" + id).attr('href');
    window.open(linktoRedirect, '_blank');;
});

//To Get the standard type based on the board type
function OnChangeStateBoardType() {

    $("input[name='RStateBoardType']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();

            $("#GetAppliedBasicsForBoardType").hide();
            if (value == 1) {
                $("#GetAppliedBasicsForBoardType").show();
            }
            else {
                $("input[name=RAppBasics][value=1]").prop('checked', true);
            }
            OnChangeGetAppliedBasicsForBoardType();
        }
    });
}

//To Get the roll number based on the standard type
function OnChangeGetAppliedBasicsForBoardType() {
    $("input[name='RAppBasics']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
            $("#GetRollNumberFor10thStd").hide();
            if (value == 2) {
                $("#GetRollNumberFor10thStd").show();
            }
        }
    });
}

function PhysicallyHanidcapEna(id) {

    var value = $('input[name=PhysicallyHanidcapInd]:checked').val();
    if (value == 1) {
        $("#PhysicallyHanidcapDiv1").show();
        $("#PhysicallyHanidcapDiv2").show();
    }
    else {
        $("#PhysicallyHanidcapDiv1").hide();
        $("#PhysicallyHanidcapDiv2").hide();
    }
}

function TenthBoardStateType() {
    var selectedTenthBoardStateType = $('input[name="TenthBoard"]:checked').val();
    if (selectedTenthBoardStateType == 1) {
        $("#TenthCOBSEType").hide();
        //$("#txtEduGrade").val('');
        $("input[name=TenthCOBSEBoard][value=7]").prop('checked', true)
    }
    else {
        $("#TenthCOBSEType").show();
        TenthCOBSEBoardType();
    }
}

function TenthCOBSEBoardType() {
    var selectedTenthCOBSEBoard = $('input[name="TenthCOBSEBoard"]:checked').val();
    if (selectedTenthCOBSEBoard == 2 || selectedTenthCOBSEBoard == 3) {
        $("#TenthCBSEICSEType").show();
    }
    else {
        $("#TenthCBSEICSEType").hide();
        //$("#txtEduGrade").text('');
    }
}
//function GetApplicantType132(type) {
//    $("#" + type).empty();
//    $("#" + type).append('<option value="choose">choose</option>');
//    $.ajax({
//        type: 'Get',
//        url: '/Admission/GetApplicantTypeList',
//        success: function (data) {
//            if (data != null || data != '') {
//                $.each(data, function () {
//                    $("#" + type).append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
//                });
//            }
//        }, error: function (result) {
//            bootbox.alert("Error", "something went wrong");
//        }
//    });
//}
//function GetApplicantType() {

//    $.ajax({
//        type: 'Get',
//        url: '/Admission/GetApplicantTypeList',
//        success: function (datajson) {

//            if (datajson != null || datajson != '') {
//                $.each(datajson, function () {
//                    $("#ApplicantTypeSelect").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
//                });
//            }

//        }, error: function (result) {
//            bootbox.alert("Error", "something went wrong");
//        }
//    });
//}

function clearallErrorFields() {

    $("#DocEduCerAcceptedImg").hide();
    $("#DocCasCerAcceptedImg").hide();
    $("#DocRatCerAcceptedImg").hide();
    $("#DocIncCerAcceptedImg").hide();
    $("#DocUIDCerAcceptedImg").hide();
    $("#DocRurCerAcceptedImg").hide();
    $("#DocKanMedCerAcceptedImg").hide();
    $("#DocDidAblCerAcceptedImg").hide();
    $("#DocExStuCerAcceptedImg").hide();
    $("#DocHyKarRegAcceptedImg").hide();
    $("#DocHorGadKanAcceptedImg").hide();
    $("#DocOthCerAcceptedImg").hide();
    $("#DocKasMigAcceptedImg").hide();
    $("#DocExSerAcceptedImg").hide();
    $("#DocLLCAcceptedImg").hide();
    $("#DocEWSAcceptedImg").hide();

    $("#DocEduCerRejectedImg").hide();
    $("#DocCasCerRejectedImg").hide();
    $("#DocRatCerRejectedImg").hide();
    $("#DocIncCerRejectedImg").hide();
    $("#DocUIDCerRejectedImg").hide();
    $("#DocRurCerRejectedImg").hide();
    $("#DocKanMedCerRejectedImg").hide();
    $("#DocDidAblCerRejectedImg").hide();
    $("#DocExStuCerRejectedImg").hide();
    $("#DocHyKarRegRejectedImg").hide();
    $("#DocHorGadKanRejectedImg").hide();
    $("#DocOthCerRejectedImg").hide();
    $("#DocExSerRejectedImg").hide();
    $("#DocEWSRejectedImg").hide();

    $("#WithoutDocEduStatus-Required").hide();
    $("#WithoutDocEduRemarks-Required").hide();
    $("#WithoutDocCasStatus-Required").hide();
    $("#WithoutDocCasRemarks-Required").hide();
    $("#WithoutDocRatStatus-Required").hide();
    $("#WithoutDocRatRemarks-Required").hide();
    $("#WithoutDocIncStatus-Required").hide();
    $("#WithoutDocIncRemarks-Required").hide();
    $("#WithoutDocUIDStatus-Required").hide();
    $("#WithoutDocUIDRemarks-Required").hide();
    $("#WithoutDocRurStatus-Required").hide();
    $("#WithoutDocRurRemarks-Required").hide();
    $("#WithoutDocKMCStatus-Required").hide();
    $("#WithoutDocKMCRemarks-Required").hide();
    $("#WithoutDocDAStatus-Required").hide();
    $("#WithoutDocDARemarks-Required").hide();
    $("#WithoutDocECStatus-Required").hide();
    $("#WithoutDocECRemarks-Required").hide();
    $("#WithoutDocHKRStatus-Required").hide();
    $("#WithoutDocHKRRemarks-Required").hide();
    $("#WithoutDocHorKanStatus-Required").hide();
    $("#WithoutDocHorKanRemarks-Required").hide();
    $("#WithoutDocOCStatus-Required").hide();
    $("#WithoutDocOCRemarks-Required").hide();
    $("#WithoutDocKMStatus-Required").hide();
    $("#WithoutDocKMRemarks-Required").hide();
    $("#WithoutDocExSerStatus-Required").hide();
    $("#WithoutDocExSerRemarks-Required").hide();
    $("#WithoutDocLLCStatus-Required").hide();
    $("#WithoutDocLLCRemarks-Required").hide();
    $("#WithoutDocEWSStatus-Required").hide();
    $("#WithoutDocEWSRemarks-Required").hide();
    $("#txtRemarks-Required").hide();
}

function GetCommentDetails(SeatAllocationId) {

    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsApplicant",
        data: { SeatAllocationId: SeatAllocationId },
        success: function (data) {

            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CreatedOn', 'title': 'Date', 'className': 'text-left' },
                    { 'data': 'userRole', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'ForwardedTo', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'ApplDescription', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Description', 'className': 'text-left' },
                ]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    });
}
$(".OnChangeofDocumentStatus").on('change', function () {
    //OnChangeofDocumentStatus();
});

function OnChangeofDocumentStatus() {

    var EduDocStatus = $("#EduDocStatus :selected").val();
    var CasDocStatus = $("#CasDocStatus :selected").val();
    var RationDocStatus = $("#RationDocStatus :selected").val();
    var IncCerDocStatus = $("#IncCerDocStatus :selected").val();
    var UIDDocStatus = $("#UIDDocStatus :selected").val();
    var RcerDocStatus = $("#RcerDocStatus :selected").val();
    var KanMedCerDocStatus = $("#KanMedCerDocStatus :selected").val();
    var DiffAblDocStatus = $("#DiffAblDocStatus :selected").val();
    var ExCerDocStatus = $("#ExCerDocStatus :selected").val();
    var HyKarDocStatus = $("#HyKarDocStatus :selected").val();
    var HorKanDocStatus = $("#HorKanDocStatus :selected").val();
    var OtherCerDocStatus = $("#OtherCerDocStatus :selected").val();
    var ExserDocStatus = $("#ExserDocStatus :selected").val();
    var LLCerDocStatus = $("#LLCerDocStatus :selected").val();
    var EWSDocStatus = $("#EWSDocStatus :selected").val();

    $(".EnableDisableDocStatus").show();
    $(".EnableDisDocSentCorrection").show();

    var SendForCorrectionInd = 0;
    var SendForApprovedInd = 0;
    if ($('.aEduCertificate').is(':visible')) {
        if (EduDocStatus != 15)
            SendForCorrectionInd = 1;
        else if (EduDocStatus == 15)
            SendForApprovedInd = 1;
    }
    else {
        if ($("#EduCertificate").val() != "") {
            if (EduDocStatus != 15)
                SendForCorrectionInd = 1;
            else if (EduDocStatus == 15)
                SendForApprovedInd = 1
        }
    }

    if (SendForCorrectionInd == 0) {
        if ($('.aCasteCertificate').is(':visible')) {
            if (CasDocStatus != 15)
                SendForCorrectionInd = 1;
            else if (CasDocStatus == 15)
                SendForApprovedInd = 1;
        }
        else {
            if ($("#CasteCertificate").val() != "") {
                if (CasDocStatus != 15)
                    SendForCorrectionInd = 1;
                else if (CasDocStatus == 15)
                    SendForApprovedInd = 1;
            }
        }

        if (SendForCorrectionInd == 0) {
            if ($('.aRationCard').is(':visible')) {
                if (RationDocStatus != 15)
                    SendForCorrectionInd = 1;
                else if (RationDocStatus == 15)
                    SendForApprovedInd = 1;
            }
            else {
                if ($("#Rationcard").val() != "") {
                    if (RationDocStatus != 15)
                        SendForCorrectionInd = 1;
                    else if (RationDocStatus == 15)
                        SendForApprovedInd = 1;
                }
            }

            if (SendForCorrectionInd == 0) {
                if ($('.aIncomeCertificate').is(':visible')) {
                    if (IncCerDocStatus != 15)
                        SendForCorrectionInd = 1;
                    else if (IncCerDocStatus == 15)
                        SendForApprovedInd = 1;
                }
                else {
                    if ($("#Incomecertificate").val() != "") {
                        if (IncCerDocStatus != 15)
                            SendForCorrectionInd = 1;
                        else if (IncCerDocStatus == 15)
                            SendForApprovedInd = 1;
                    }
                }

                if (SendForCorrectionInd == 0) {
                    if ($('.aUIDNumber').is(':visible')) {
                        if (UIDDocStatus != 15)
                            SendForCorrectionInd = 1;
                        else if (UIDDocStatus == 15)
                            SendForApprovedInd = 1;
                    }
                    else {
                        if ($("#UIDNumber").val() != "") {
                            if (UIDDocStatus != 15)
                                SendForCorrectionInd = 1;
                            else if (UIDDocStatus == 15)
                                SendForApprovedInd = 1;
                        }
                    }

                    if (SendForCorrectionInd == 0) {
                        if ($('.aRuralcertificate').is(':visible')) {
                            if (RcerDocStatus != 15)
                                SendForCorrectionInd = 1;
                            else if (RcerDocStatus == 15)
                                SendForApprovedInd = 1;
                        }
                        else {
                            if ($("#Ruralcertificate").val() != "") {
                                if (RcerDocStatus != 15)
                                    SendForCorrectionInd = 1;
                                else if (RcerDocStatus == 15)
                                    SendForApprovedInd = 1;
                            }
                        }

                        if (SendForCorrectionInd == 0) {
                            if ($('.aKannadamediumCertificate').is(':visible')) {
                                if (KanMedCerDocStatus != 15)
                                    SendForCorrectionInd = 1;
                                else if (KanMedCerDocStatus == 15)
                                    SendForApprovedInd = 1;
                            }
                            else {
                                if ($("#KannadamediumCertificate").val() != "") {
                                    if (KanMedCerDocStatus != 15)
                                        SendForCorrectionInd = 1;
                                    else if (KanMedCerDocStatus == 15)
                                        SendForApprovedInd = 1;
                                }
                            }

                            if (SendForCorrectionInd == 0) {
                                if ($('.aDifferentlyabledcertificate').is(':visible')) {
                                    if (DiffAblDocStatus != 15)
                                        SendForCorrectionInd = 1;
                                    else if (DiffAblDocStatus == 15)
                                        SendForApprovedInd = 1;
                                }
                                else {
                                    if ($("#Differentlyabledcertificate").val() != "") {
                                        if (DiffAblDocStatus != 15)
                                            SendForCorrectionInd = 1;
                                        else if (DiffAblDocStatus == 15)
                                            SendForApprovedInd = 1;
                                    }
                                }

                                if (SendForCorrectionInd == 0) {
                                    if ($('.aExemptedCertificate').is(':visible')) {
                                        if (ExCerDocStatus != 15)
                                            SendForCorrectionInd = 1;
                                        else if (ExCerDocStatus == 15)
                                            SendForApprovedInd = 1;
                                    }
                                    else {
                                        if ($("#ExemptedCertificate").val() != "") {
                                            if (ExCerDocStatus != 15)
                                                SendForCorrectionInd = 1;
                                            else if (ExCerDocStatus == 15)
                                                SendForApprovedInd = 1;
                                        }
                                    }

                                    if (SendForCorrectionInd == 0) {
                                        if ($('.aHyderabadKarnatakaRegion').is(':visible')) {
                                            if (HyKarDocStatus != 15)
                                                SendForCorrectionInd = 1;
                                            else if (HyKarDocStatus == 15)
                                                SendForApprovedInd = 1;
                                        }
                                        else {
                                            if ($("#HyderabadKarnatakaRegion").val() != "") {
                                                if (HyKarDocStatus != 15)
                                                    SendForCorrectionInd = 1;
                                                else if (HyKarDocStatus == 15)
                                                    SendForApprovedInd = 1;
                                            }
                                        }

                                        if (SendForCorrectionInd == 0) {
                                            if ($('.aHoranaaduKannadiga').is(':visible')) {
                                                if (HorKanDocStatus != 15)
                                                    SendForCorrectionInd = 1;
                                                else if (HorKanDocStatus == 15)
                                                    SendForApprovedInd = 1;
                                            }
                                            else {
                                                if ($("#HoranaaduKannadiga").val() != "") {
                                                    if (HorKanDocStatus != 15)
                                                        SendForCorrectionInd = 1;
                                                    else if (HorKanDocStatus == 15)
                                                        SendForApprovedInd = 1;
                                                }
                                            }

                                            if (SendForCorrectionInd == 0) {
                                                if ($('.aOtherCertificates').is(':visible')) {
                                                    if (OtherCerDocStatus != 15)
                                                        SendForCorrectionInd = 1;
                                                    else if (OtherCerDocStatus == 15)
                                                        SendForApprovedInd = 1;
                                                }
                                                else {
                                                    if ($("#OtherCertificates").val() != "") {
                                                        if (OtherCerDocStatus != 15)
                                                            SendForCorrectionInd = 1;
                                                        else if (OtherCerDocStatus == 15)
                                                            SendForApprovedInd = 1;
                                                    }
                                                }

                                                if (SendForCorrectionInd == 0) {
                                                    if ($('.aExserviceman').is(':visible')) {
                                                        if (ExserDocStatus != 15)
                                                            SendForCorrectionInd = 1;
                                                        else if (ExserDocStatus == 15)
                                                            SendForApprovedInd = 1;
                                                    }
                                                    else {
                                                        if ($("#Exserviceman").val() != "") {
                                                            if (ExserDocStatus != 15)
                                                                SendForCorrectionInd = 1;
                                                            else if (ExserDocStatus == 15)
                                                                SendForApprovedInd = 1;
                                                        }
                                                    }
                                                    if (SendForCorrectionInd == 0) {
                                                        if ($('.aLLCertificate').is(':visible')) {
                                                            if (LLCerDocStatus != 15)
                                                                SendForCorrectionInd = 1;
                                                            else if (LLCerDocStatus == 15)
                                                                SendForApprovedInd = 1;
                                                        }
                                                        else {
                                                            if ($("#LLCertificate").val() != "") {
                                                                if (LLCerDocStatus != 15)
                                                                    SendForCorrectionInd = 1;
                                                                else if (LLCerDocStatus == 15)
                                                                    SendForApprovedInd = 1;
                                                            }
                                                        }

                                                        if (SendForCorrectionInd == 0) {
                                                            if ($('.aEWSCertificate').is(':visible')) {
                                                                if (EWSDocStatus != 15)
                                                                    SendForCorrectionInd = 1;
                                                                else if (EWSDocStatus == 15)
                                                                    SendForApprovedInd = 1;
                                                            }
                                                            else {
                                                                if ($("#EWSCertificate").val() != "") {
                                                                    if (EWSDocStatus != 15)
                                                                        SendForCorrectionInd = 1;
                                                                    else if (EWSDocStatus == 15)
                                                                        SendForApprovedInd = 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    if (SendForCorrectionInd == 1) {
        $(".EnableDisDocSentCorrection").show();
        $(".EnableDisableDocStatus").hide();
    } else if (SendForCorrectionInd == 0 && SendForApprovedInd == 1) {
        $(".EnableDisDocSentCorrection").hide();
        $(".EnableDisableDocStatus").show();
    }

    if (EduDocStatus == 0 && CasDocStatus == 0 && RationDocStatus == 0 && IncCerDocStatus == 0 && UIDDocStatus == 0 && RcerDocStatus == 0
        && KanMedCerDocStatus == 0 && DiffAblDocStatus == 0 && ExCerDocStatus == 0 && HyKarDocStatus == 0 && HorKanDocStatus == 0
        && OtherCerDocStatus == 0 && ExserDocStatus == 0 && LLCerDocStatus == 0 && EWSDocStatus == 0) {
        $(".EnableDisableDocStatus").hide();
        $(".EnableDisDocSentCorrection").hide();
    }
}
//************************** Document Verification fee ******************//

function GetDataDocumentsVerificationFee() {
    var session = $('#ddlSessionDvF :selected').val();
    var course = $('#ddlCourseTypeDvF').val();
    var ApplicantType = $('#ddlApplicantTypeDvF :selected').val();
    if (session == "choose") {
        session =null;
    }
    if (course == null) {
        course = null;
    }
    if (ApplicantType == "choose") {
        ApplicantType = null;
    }
        $.ajax({
            type: "GET",
            url: "/Admission/GetDataDocumentsVerificationFee",
            contentType: "application/json",
            data: { year: session, courseType: course, applicanType: ApplicantType, division_id: 0, district_lgd_code: 0, taluk_lgd_code: 0, InstituteId: 0},
            success: function (data) {

                $('#tblDocumentVerifyFee').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                        { 'data': 'MisCode', 'title': 'MISCODE', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'ITI Name', 'className': 'text-left' },
                        
                        { 'data': 'ApplicantNumber', 'title': 'Applicantion Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        //{
                        //    'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                        //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                        //        if (oData.PaymentOptionval == true) {
                        //            $(nTd).html("Online");
                        //        }
                        //        else {
                        //            $(nTd).html("Offline");
                        //        }
                        //    }
                        //},

                        { 'data': 'OfficerName', 'title': 'Verification Officer Name', 'className': 'text-left' },
                        {
                            'data': 'DocumentVeriFeePymtDate', 'title': 'Date', 'className': 'text-left',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var date = daterangeformate2(oData.DocumentVeriFeePymtDate);
                                $(nTd).html(date);
                            }
                        },
                        { 'data': 'DocVeriFeeReceiptNumbers', 'title': 'Receipt Number', 'className': 'text-left' },
                        //{
                        //    'title': 'Receipt (Pdf)',
                        //    render: function (data, type, row) {
                        //        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 +"' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        //    }
                        //},
                        {
                            'title': 'Receipt (PDF)',
                            render: function (data, type, row) {
                                return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                            }
                        },

                        { 'data': 'Treasury_Receipt_No', 'title': 'Treasury Receipt No.', 'className': 'text-left' },

                        //{
                        //    'title': 'Treasury Receipt No. ',
                        //    render: function (data, type, row) {
                        //        return "RCPT5685"
                        //    }
                        //},
                        //{ 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                        
                        
                        
                        //{
                        //    'data': 'ApplicationId',
                        //    'title': 'Remarks',
                        //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        //        $(nTd).html("<input type='button' onclick='GetVerificationFeeCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                        //    }
                        //},
                        { 'data': 'DocVeriFees', 'title': 'Document Verification Fee Amount (In ₹)', 'className': 'text-left' }
                    ],
                    "footerCallback": function (row, data, start, end, display) {
                        
                        var api = this.api();
                        var totalAmount = 0;
                        for (var i = 0; i < data.length; i++) {
                            totalAmount += parseInt(data[i]["DocVeriFees"]);
                        }
                        var numOfColumns = $('#tblDocumentVerifyFee').DataTable().columns().nodes().length; // Get total number of columns
                        $(this).children("th").remove();
                        var footer = $(this).append('<tfoot><tr></tr></tfoot>');
                        this.api().columns().every(function (index) {
                            if (index == numOfColumns - 1) {
                                $(footer).append('<th><input type="label" style="text-align:center;color:red; width:100%;"' +
                                    'value = ₹' + totalAmount + ' readonly></th>');
                            } else if (index == numOfColumns - 3 || index == numOfColumns - 2) {
                                if (index == numOfColumns - 3) {
                                    $(footer).append('<th colspan="2"><label style="text-align:center; width:100%;" >Document Verification Fee Amount (In ₹) </label></th>');
                                }
                            } else {
                                $(footer).append('<th><label style="width:100%;" /></th> ');
                            }
                        });
                    },
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
}

//function GetDocumentVerificationFeeToPay() {

//    var session = $('#ddlSessionfee :selected').val();
//    var course = $('#ddlCourseTypefee :selected').val();
//    var ApplicantType = $('#ddlApplicantTypeffee :selected').val();

//    $.ajax({
//        type: "GET",
//        url: "/Admission/GetDataDocumentsVerificationFeepayment",
//        contentType: "application/json",
//        data: { year: session, courseType: course, applicanType: ApplicantType },
//        success: function (data) {
//            $('#tblDocumentFeePay').DataTable({
//                data: data,
//                "destroy": true,
//                "bSort": true,
//                columns: [
//                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
//                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
//                    { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                    

//                    { 'data': 'ApplicantNumber', 'title': 'Applicantion Number', 'className': 'text-center' },
//                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
//                    {
//                        'data': 'PaymentOptionval', 'title': 'Application Mode', 'className': 'text-left',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

//                            if (oData.PaymentOptionval == true) {
//                                $(nTd).html("Online");
//                            }
//                            else {
//                                $(nTd).html("Offline");
//                            }
//                        }
//                    },
                    
//                    {
//                        'data': 'slno',
//                        'title': 'Document Verification Fee Amount ',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            $(nTd).html(50);
//                        }
//                    },
//                    {
//                        'data': 'slno',
//                        'title': 'Status ',
//                       // "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            //var sea = "seatType" + id;
//                            

//                            $(nTd).html("<select style='width:100px;' class='rcptnum' id='myselectlist'  onchange='ReceiptNoGeneration(this.id, " + oData.ApplicationId + ")'><option value='choose' >choose</option> <option value='1'>Received</option> <option value='2'>Not Received</option></select>");
                               
//                        }
//                    },
//                    {
//                        'data': 'DocVeriFeeReceiptNumbers', 'title': 'Receipt Number', 'className': 'text-left recptnum',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            
//                             if (oData.DocVeriFeeReceiptNumbers != null) {
//                            return receiptNum;
//                                //$(nTd).html(receiptNum);
//                            }
//                            //else {
//                            //    $(nTd).html("");
//                            //}
//                        }
//                    },
//                    {
//                        'title': 'Receipt (Pdf)',
//                        render: function (data, type, row) {
//                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
//                        }
//                    },
//                    {
//                        'data': 'slno',
//                        'title': 'Treasury Receipt No. ',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            $(nTd).html("<input type='textbox' />");
//                        }
//                    },
//                    {
//                        'data': 'slno',
//                        'title': 'Date ',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            $(nTd).html("<input type='textbox' width='100px'/>");
//                        }
//                    },
//                    {
//                        'data': 'slno',
//                        'title': 'Remarks ',
//                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                            $(nTd).html("<input type='textarea' />");
//                        }
//                    },
//                    //{ 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },



//                    //{
//                    //    'data': 'ApplicationId',
//                    //    'title': 'Remarks',
//                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                    //        $(nTd).html("<input type='button' onclick='GetVerificationFeeCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

//                    //    }
//                    //},
                    
//                ],
               
//            });


//        }, error: function (result) {
//            bootbox.alert("Error", "something went wrong");
//        }
//    });
//}

function GetDocumentVerificationFeeToPay1() {



    var session = $('#ddlSessionfee :selected').val();
    var course = $('#ddlCourseTypefee :selected').val();
    var ApplicantType = $('#ddlApplicantTypeffee :selected').val();
    var ApplicationNo = $("#ApplNo").val();
    var applicationId = $("#ApplicationId").text();
    
    $.ajax({
        type: "GET",
        url: "/Admission/GetDataDocumentsVerificationFeepayment",
        contentType: "application/json",
        data: { year: session, courseType: course, applicanType: ApplicantType, ApplNo: ApplicationNo },
        success: function (data) {
            
            $('#tblDocumentFeePay').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                    { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                    { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                    //{
                    //    'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                    //        if (oData.PaymentOptionval == true) {
                    //            $(nTd).html("Online");
                    //        }
                    //        else {
                    //            $(nTd).html("Offline");
                    //        }
                    //    }
                    //},

                    //{ 'data': 'OfficerName', 'title': 'Verification Officer', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                    {
                        'title': 'Application Acknowledgment',
                        render: function (data, type, row) {
                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        }
                    },
                    {
                        'data': 'ApplicationId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetCommentDetailsApplicant(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'slno',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",1)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

                        }
                    }
                ]
            });
            //$("#ApplicationId").val(data.ApplicationId);
            //paging: true,
            //$("#studentMarks").empty();
            //$("#tblDocumentFeePay").show();
            //if (data.length > 0) {
            //    $("#savefee").show();
            //    
               
                //$.each(data,function (i) {
                //    
                //    var name="";
                //    //if (data.PaymentOptionval == true) {
                //    //return   name=  "Online"
                //    //}
                //    //else {
                //    //    return name = "Offline"
                //    //}
                //    var markup = "<tr><td>" + (i + 1) + "</td>" +
                //        "<td>" + this.CourseType + "</td>" +
                //        "<td>" + this.Year + "</td>" +
                //        "<td id='applno'>" + this.ApplicantNumber + "</td>" +
                //        "<td>" + this.ApplicantName + "</td>" +
                //        "<td>Offline</td>" +
                //        "<td>50</td>" +
                //        "<td><select style='width:100px;' class='GetData " + this.ApplicationId + "' id='myselectlist'  onchange='ReceiptNoGeneration(this.id, " + this.ApplicationId + ")'><option value='choose' >choose</option> <option value='1'>Received</option> <option value='2'>Not Received</option></select></td>" +
                //        "<td id='receiptno'>Not-generated</td>" +
                //        "<td><a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + this.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a></td>" +
                //        "<td  ><input type='textbox' id='TrRcptNo'></td>" +
                //        "<td id='datercpt'><input type='textbox'></td>" +
                //        "<td id='rmrks'><input type='textbox'></td></tr>";
                //    $("#studentMarks").append(markup);

                //});
                //style = "color:red;"
            //}


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}




$("body").on("change", "#studentMarks .GetData", function () {
    
    var row = $(this).closest("tr");
    var $tdMISCode = row.find("td:nth-child(3)");
    var $tdInstitute = row.find("td:nth-child(4)");
    var $tdStudName = row.find("td:nth-child(5)");
    var $tdRollNum1 = row.find("td:nth-child(6)");
    var $tdfeeAmt = row.find("td:nth-child(10)");
    var treasReceiptNo = $("#TreasuryReceipt").val();
    var payment = $('#pay :selected').text();
    var feeAmtcheck = $tdfeeAmt.text();
    if (treasReceiptNo == 0) {
        bootbox.alert({
            message: "Please Enter Treasury number",
            closeButton: false
        });
    }

    //function ReceiptNoGeneration(val, id) {
        
    var value = $("#myselectlist :selected").val()
    var id = $('.GetData').val();

        $.ajax({
            type: "GET",
            url: "/Admission/GetReceiptNumber",
            contentType: "application/json",
            data: { val: value, id: id },
            success: function (data) {
                
                $('#receiptno').html(data);
                //receiptNum = data;

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });


   /* }*/




    //if (parseInt(feeAmtcheck) != 0) {
    //    var $tdTradeName = row.find("td:nth-child(7)");
    //    var $tdTradeTypeID1 = row.find("td:nth-child(9)");
    //    tradeName = $tdTradeName.text();
    //    tradeType1 = $tdTradeTypeID1.text();
    //    rollNum1 = $tdRollNum1.text();
    //    StudName = $tdStudName.text();
    //    Institute = $tdInstitute.text();
    //    miscode = $tdMISCode.text();
    //    feeAmt = $tdfeeAmt.text();

    //    
    //    var model = {}
    //    model.payment = payment;
    //    model.MISCode = miscode;
    //    model.ApplicantName = StudName;
    //    model.traineeRollNumber = rollNum1;
    //    model.InstituteName = Institute;
    //    model.TradeName = tradeName;
    //    model.tradeType = tradeType1;
    //    
    //    bootbox.confirm({
    //        title: "Confirm",
    //        message: "Are you sure want to Generate the Receipt? ",
    //        closeButton: false,
    //        buttons: {
    //            confirm: {
    //                label: 'Yes',
    //                className: 'btn-success'
    //            },
    //            cancel: {
    //                label: 'No',
    //                className: 'btn-danger'
    //            }
    //        },
    //        callback: function (result) {
    //            if (result == true) {
    //                $.ajax({
    //                    type: 'POST',
    //                    url: '/Result/RetotallingFeePay',
    //                    data: {

    //                        payment: payment, MISCode: miscode, ApplicantName: StudName, traineeRollNumber: rollNum1,
    //                        InstituteName: Institute, TradeName: tradeName, tradeType: tradeType1, FeeAmt: feeAmt,
    //                        treasuryReceiptNo: treasReceiptNo
    //                    },
    //                    success: function (data) {
    //                        // $("#HiddenApplicationNumner").text(applicationnumber);
    //                        
    //                        if (data != null) {
    //                            $("#HiddenApplicationNumner").text(applicationNumber);

    //                            bootbox.alert({
    //                                message: "Payment received, receipt generated successfully.....",
    //                                closeButton: false
    //                            });
    //                            //$('#data').find('#receiptno').html(data.num);
    //                            $('#receiptno').html(data.ReceiptNo);
    //                            $('#Appno').html(data.ApplicationNumner);
    //                            $('#pay').attr('disabled', 'disabled');
    //                            $('#btnGravienceSubmission').removeAttr('disabled');
    //                            var applnNum = data.ApplicationNumner;
    //                            var receiptNumebr = data.ReceiptNo;
    //                            $("#sub").attr('disabled', true);
    //                            var table = document.getElementById('SubjectDetailsGrid');
    //                            var rowLength = table.rows.length;
    //                            
    //                            for (var j = 1; j < rowLength; j++) {
    //                                var row = table.rows[j];

    //                                var hh = document.getElementById('sub' + j);

    //                                // $('sub' + j).attr('disabled', true);
    //                                $(hh).attr('disabled', 'disabled');

    //                            }
    //                            //for (j = 1; j <= blkArr.length; j++) {
    //                            //    
    //                            //    var hh = document.getElementById('sub' + j);
    //                            //    $(hh).attr('disabled', 'disabled');

    //                            //}
    //                        }

    //                        else {
    //                            bootbox.alert("Error while Saving Data");
    //                        }
    //                    }


    //                });
    //            }
    //            else {
    //                document.getElementById("pay").selectedIndex = "0";
    //            }

    //        }
    //    })
    //}
    //else {
    //    bootbox.alert({
    //        message: "Please select the subjects",
    //        closeButton: false
    //    });
    //    document.getElementById("pay").selectedIndex = "0";
    //}

});


//function ReceiptNoGeneration(val,id)
//{
//    
//    var value = $("#" + val+" :selected").val()
//    $.ajax({
//        type: "GET",
//        url: "/Admission/GetReceiptNumber",
//        contentType: "application/json",
//        data: { val: value, id: id},
//        success: function (data) {
//            
//            receiptNum = data;

//        }, error: function (result) {
//            bootbox.alert("Error", "something went wrong");
//        }
//    });

   
//}



function GetVerificationFeeCommentDetails(ApplicationId) {
    $('#HisVerificatiobnFeeRemarksModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetVerificationFeeCmntDetailsById",
        data: { ApplicationId: ApplicationId },
        success: function (data) {

            var t = $('#tblVerificationFeeCommentDetails').DataTable({
                data: data,
                destroy: true,
                searching: false,
                info: false,
                paging: false,
                columns: [
                    { 'data': 'Apdate', 'title': 'Date', 'className': 'text-center' },
                    { 'data': 'userRole', 'title': 'Verification By', 'className': 'text-center' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-center' },
                ]
            });
        }
    });
}

//Enable disable the online offline payment details
function DocumentPaymentOption(calledFrm) {

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    if (PaymentOptionval == 1) {
        $("#PaymentThroughOffline").hide();
        $("#PaymentThroughOnline").show();
        $("#txtDocumentFeeReceiptDetails").val('');

    } else {
        $("#PaymentThroughOnline").hide();
        $("#PaymentThroughOffline").show();
        if ($("#InstituteList :selected").text() != "Select") {
            if (calledFrm == 1)
                bootbox.alert("<br><br>You have to pay the verification fee at <b>" + $("#InstituteList :selected").text() + " </b> on or before due date otherwise applicant will not be considered");
        }
    }
}


//To save Payment details
function SavePaymentsDetails() {
    var IsValid = true;
    var ApplicantId = $("#ApplicantId").val();
    var rcptno = $("#receiptno").text();
    var trcptno = $("#TrRcptNo").val();
    var Applno = $("#applno").text();
    var docfee = $("#verifee").text();
    var applid = $(".GetData ").val();
    //var docfee = "50";
    
    var fileData = new FormData();

    var DocumentSubmittedID = $("#DocumentSubmittedID").val();
    var PreferenceSubmittedID = $("#PreferenceSubmittedID").val();
    //if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null || DocumentSubmittedID == 0 || PreferenceSubmittedID == 0) {
    //    bootbox.alert("<br><br>Kindly Give the Documents,Institute Preference Details and Save the Details");
    //}
   // else {
        var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
        var DocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

        //Document Verification Fee Details
    //Offline

        if (PaymentOptionval == 0) {
            $("#txtDocumentFeeReceiptDetails-Required").hide();
            var txtDocumentFeeReceiptDetailsValue = $("#txtDocumentFeeReceiptDetails").val();
            if (txtDocumentFeeReceiptDetailsValue == null || txtDocumentFeeReceiptDetailsValue == "") {
                $("#txtDocumentFeeReceiptDetails-Required").show();
                IsValid = false;
            }
        }
        //else {
        //    IsValid = false;
        //    bootbox.alert("<br><br>As of now we are not providing online option for the payment details!!");
    //}
    if (IsValid == true) {

            //Document Fees Details
            fileData.append(
                "ApplicantId", $("#ApplicantId").val()
            );
            fileData.append(
                "PaymentOptionval","Offline" //PaymentOptionval
            );
            //fileData.append(
            //    "DocumentFeeReceiptDetails", rcptno
            //);
            fileData.append(
                "DocVeriFee", docfee
            );
            fileData.append(
                "TreasuryReceiptNo", $("#txtDocumentFeeReceiptDetails").val()
            );
            fileData.append(
                "ApplicantNumber", Applno
            );
           
            $.ajax({
                type: "POST",
                url: "/Admission/InsertPaymentDetails",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (data) {
                    bootbox.alert("<br><br>" + data.pref.status);
                    if (data.objReturnApplicationForm != null) {
                        //$("#SaveSeatAllocationFeePay").attr("disabled", true);
                        $(".PaymentGenerationGridCls").show();
                        
                        //$(".AdmisionFee").prop('disabled', true);
                       
                        $('#PaymentGenerationGrid').DataTable({
                            data: data.objReturnApplicationForm,
                            "destroy": true,
                            "bSort": false,
                            sDom: 'lrtip',
                            "bPaginate": false,
                            "bInfo": false,
                            searching: false,
                            columns: [
                               
                                { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                                
                                {
                                    'data': 'CreatedOn', 'title': 'Payment Date', 'className': 'text - left DOB',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        var date = daterangeformate2(oData.CreatedOn, 2);
                                        $(nTd).html(date);
                                    }
                                },
                                { 'data': 'DocumentFeeReceiptDetails', 'title': 'Receipt Number', 'className': 'text-center' },
                                { 'data': 'TreasuryReceiptNo', 'title': 'Treasury Receipt No.', 'className': 'text-center' },
                                { 'data': 'DocVeriFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
                                {
                                    'title': 'Receipt (PDF)',
                                    render: function (data, type, row) {
                                        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                    }
                                },
                                //{ 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
                            ]
                        });
                    }
                    if ($("#ReVerficationStatus").val() == 0 || $("#ReVerficationStatus").val() == null) {
                        $("#EnableDisableSubmit").show();
                        $("#FinalSubmitRemarks").hide();
                        $("#FinalSubmit").show();
                        $("#FinalSubmit").show();
                        $("#pymntdiv").hide();
                    }
                    GetApplicantsStatus();
                    GetDataDocumentsVerificationFee();
                }
            });
        }
    //}
}






//function getPagination(table) {
//    var lastPage = 1;

//    $('#maxRows')
//        .on('change', function (evt) {
//            //$('.paginationprev').html('');						// reset pagination

//            lastPage = 1;
//            $('.pagination')
//                .find('li')
//                .slice(1, -1)
//                .remove();
//            var trnum = 0; // reset tr counter
//            var maxRows = parseInt($(this).val()); // get Max Rows from select option

//            if (maxRows == 5000) {
//                $('.pagination').hide();
//            } else {
//                $('.pagination').show();
//            }

//            var totalRows = $(table + ' tbody tr').length; // numbers of rows
//            $(table + ' tr:gt(0)').each(function () {
//                // each TR in  table and not the header
//                trnum++; // Start Counter
//                if (trnum > maxRows) {
//                    // if tr number gt maxRows

//                    $(this).hide(); // fade it out
//                }
//                if (trnum <= maxRows) {
//                    $(this).show();
//                } // else fade in Important in case if it ..
//            }); //  was fade out to fade it in
//            if (totalRows > maxRows) {
//                // if tr total rows gt max rows option
//                var pagenum = Math.ceil(totalRows / maxRows); // ceil total(rows/maxrows) to get ..
//                //	numbers of pages
//                for (var i = 1; i <= pagenum;) {
//                    // for each page append pagination li
//                    $('.pagination #prev')
//                        .before(
//                            '<li data-page="' +
//                            i +
//                            '">\
//								  <span>' +
//                            i++ +
//                            '<span class="sr-only">(current)</span></span>\
//								</li>'
//                        )
//                        .show();
//                } // end for i
//            } // end if row count > max rows
//            $('.pagination [data-page="1"]').addClass('active'); // add active class to the first li
//            $('.pagination li').on('click', function (evt) {
//                // on click each page
//                evt.stopImmediatePropagation();
//                evt.preventDefault();
//                var pageNum = $(this).attr('data-page'); // get it's number

//                var maxRows = parseInt($('#maxRows').val()); // get Max Rows from select option

//                if (pageNum == 'prev') {
//                    if (lastPage == 1) {
//                        return;
//                    }
//                    pageNum = --lastPage;
//                }
//                if (pageNum == 'next') {
//                    if (lastPage == $('.pagination li').length - 2) {
//                        return;
//                    }
//                    pageNum = ++lastPage;
//                }

//                lastPage = pageNum;
//                var trIndex = 0; // reset tr counter
//                $('.pagination li').removeClass('active'); // remove active class from all li
//                $('.pagination [data-page="' + lastPage + '"]').addClass('active'); // add active class to the clicked
//                // $(this).addClass('active');					// add active class to the clicked
//                limitPagging();
//                $(table + ' tr:gt(0)').each(function () {
//                    // each tr in table not the header
//                    trIndex++; // tr index counter
//                    // if tr index gt maxRows*pageNum or lt maxRows*pageNum-maxRows fade if out
//                    if (
//                        trIndex > maxRows * pageNum ||
//                        trIndex <= maxRows * pageNum - maxRows
//                    ) {
//                        $(this).hide();
//                    } else {
//                        $(this).show();
//                    } //else fade in
//                }); // end of for each tr in table
//            }); // end of on click pagination list
//            limitPagging();
//        })
//        .val(5)
//        .change();

//    // end of on select change

//    // END OF PAGINATION
//}

//function limitPagging() {
//    // alert($('.pagination li').length)

//    if ($('.pagination li').length > 7) {
//        if ($('.pagination li.active').attr('data-page') <= 3) {
//            $('.pagination li:gt(5)').hide();
//            $('.pagination li:lt(5)').show();
//            $('.pagination [data-page="next"]').show();
//        } if ($('.pagination li.active').attr('data-page') > 3) {
//            $('.pagination li:gt(0)').hide();
//            $('.pagination [data-page="next"]').show();
//            for (let i = (parseInt($('.pagination li.active').attr('data-page')) - 2); i <= (parseInt($('.pagination li.active').attr('data-page')) + 2); i++) {
//                $('.pagination [data-page="' + i + '"]').show();

//            }

//        }
//    }
//}

$(function () {
    // Just to append id number for each row
    $('table tr:eq(0)').prepend('<th> ID </th>');

    var id = 0;

    $('table tr:gt(0)').each(function () {
        id++;
        $(this).prepend('<td>' + id + '</td>');
    });
});

function GetDataToVerifyDocumentsDivision() {
    //if ($('#CourseType22').val() == "choose") {
    if ($('#CourseType22').val() == "101" || $('#CourseType22').val() == null) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetApplicantsStatusFilter",
            contentType: "application/json",
            data: { 'year': 0, 'courseType': 0, 'applicanType': 0, 'division_id': 0, 'district_lgd_code': 0, 'taluk_lgd_code': 0, 'InstituteId': 0},
            success: function (data) {
                $('#VerifyDocumentsTableDivision').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                        {
                            'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                if (oData.PaymentOptionval == true) {
                                    $(nTd).html("Online");
                                }
                                else {
                                    $(nTd).html("Offline");
                                }
                            }
                        },
                        { 'data': 'OfficerName', 'title': 'Verification Officer', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                        {
                            'title': 'Application Acknowledgment',
                            render: function (data, type, row) {
                                return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetCommentDetailsApplicant(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'CredatedBy',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",1)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

                            }
                        }
                    ]
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        var year = $('#Session22').val();
        var courseType = $('#CourseType22').val();
        var appType = $('#ApplicantTypeSelect').val();
        if (year == 'choose') {
            bootbox.alert("select the session");
        } else if (courseType == 'choose') {
            bootbox.alert("select the course type");
        } else
            if (appType == 'choose') {
                bootbox.alert("select the applicant type");
            } else {
                $.ajax({
                    type: "GET",
                    url: "/Admission/GetApplicantsStatusFilter",
                    contentType: "application/json",
                    data: { 'year': year, 'courseType': courseType, 'applicanType': appType, 'division_id': 0, 'district_lgd_code': 0, 'taluk_lgd_code': 0, 'InstituteId': 0 },

                    //data: { 'year': year, 'courseType': courseType, 'applicanType': appType },
                    success: function (data) {
                        $('#VerifyDocumentsTable').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                                { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                                { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                                { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                                { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                                { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                                {
                                    'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                        if (oData.PaymentOptionval == true) {
                                            $(nTd).html("Online");
                                        }
                                        else {
                                            $(nTd).html("Offline");
                                        }
                                    }
                                },
                                { 'data': 'OfficerName', 'title': 'Verification Officer', 'className': 'text-left' },
                                { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                                {
                                    'title': 'Application Acknowledgment',
                                    render: function (data, type, row) {
                                        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                    }
                                },
                                {
                                    'data': 'ApplicationId',
                                    'title': 'Remarks',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationRemarks(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                                    }
                                },
                                {
                                    'data': 'CredatedBy',
                                    'title': 'Action',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",1)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

                                    }
                                }
                            ]
                        });
                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }

    }
}

function GetDataDocumentsVerificationFeeDivision() {
    
    var session = $('#ddlSessionDvF :selected').val();
    var course = $('#ddlCourseTypeDvF :selected').val();
    var ApplicantType = $('#ddlApplicantTypeDvF :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetDataDocumentsVerificationFee",
        contentType: "application/json",
        data: { year: session, courseType: course, applicanType: ApplicantType, division_id: 0, district_lgd_code: 0, taluk_lgd_code: 0, InstituteId: 0 },
        success: function (data) {
            $('#tblDocumentVerifyFeeReconcile').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                    { 'data': 'MisCode', 'title': 'MISCODE', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'ITI Name', 'className': 'text-left' },

                    { 'data': 'ApplicantNumber', 'title': 'Applicantion Number', 'className': 'text-center' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    {
                        'data': 'PaymentOptionval', 'title': 'Payment Mode', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.PaymentOptionval == true) {
                                $(nTd).html("Online");
                            }
                            else {
                                $(nTd).html("Offline");
                            }
                        }
                    },
                    { 'data': 'OfficerName', 'title': 'Verification Officer Name', 'className': 'text-left' },
                    {
                        'data': 'DocumentVeriFeePymtDate', 'title': 'Date', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DocumentVeriFeePymtDate);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'DocVeriFeeReceiptNumbers', 'title': 'Receipt Number', 'className': 'text-left' },
                    {
                        'title': 'Receipt (Pdf)',
                        render: function (data, type, row) {
                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        }
                    },
                    { 'data': 'Treasury_Receipt_No', 'title': 'Treasury Receipt No.', 'className': 'text-left' },


                    //{
                    //    'title': 'Treasury Receipt No. ',
                    //    render: function (data, type, row) {
                    //        return "RCPT5685"
                    //    }
                    //},
                    //{ 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },



                    //{
                    //    'data': 'ApplicationId',
                    //    'title': 'Remarks',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        $(nTd).html("<input type='button' onclick='GetVerificationFeeCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='view'/>");

                    //    }
                    //},
                    { 'data': 'DocVeriFees', 'title': 'Document Verification Fee Amount (In ₹)', 'className': 'text-left' }
                ],
                "footerCallback": function (row, data, start, end, display) {
                    
                    var api = this.api();
                    var totalAmount = 0;
                    for (var i = 0; i < data.length; i++) {
                        totalAmount += parseInt(data[i]["DocVeriFees"]);
                    }
                    var numOfColumns = $('#tblDocumentVerifyFee').DataTable().columns().nodes().length; // Get total number of columns
                    $(this).children("th").remove();
                    var footer = $(this).append('<tfoot><tr></tr></tfoot>');
                    this.api().columns().every(function (index) {
                        if (index == numOfColumns - 1) {
                            $(footer).append('<th><input type="label" style="text-align:center;color:red; width:100%;"' +
                                'value = ₹' + totalAmount + ' readonly></th>');
                        } else if (index == numOfColumns - 3 || index == numOfColumns - 2) {
                            if (index == numOfColumns - 3) {
                                $(footer).append('<th colspan="2"><label style="text-align:center; width:100%;" >Document Verification Fee Amount (In ₹) </label></th>');
                            }
                        } else {
                            $(footer).append('<th><label style="width:100%;" /></th> ');
                        }
                    });
                },
            });
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function fnDisplayApplicantDetailScreen(ApplicationId) {
    
    GetApplicantDetailsByIdCmn(ApplicationId);
    $("#AdmisionTime").val('');
    $("#PaymentDate").val('');
    $("input[name=AdmittedStatus][value=6]").prop('checked', true);
    //fnShowHidePaymentInfo();
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

