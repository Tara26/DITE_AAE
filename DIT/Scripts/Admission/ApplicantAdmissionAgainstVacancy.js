$(document).ready(function () {

    $('.nav-tabs li:eq(0) a').tab('show');
    $(".EnableGetApplicationNumber").hide();
    $("#accordion").hide();
    $("#AppFormID").hide();
    $("#tab_2").hide();

    $(".EduFileAttach").hide(); $(".CasteFileAttach").hide(); $(".RationFileAttach").hide(); $(".IncomeCertificateAttach").hide();
    $(".UIDFileAttach").hide(); $(".RuralcertificateAttach").hide(); $(".KannadamediumCertificateAttach").hide(); $(".DifferentlyabledcertificateAttach").hide();
    $(".ExemptedCertificateAttach").hide(); $(".HyderabadKarnatakaRegionAttach").hide(); $(".HoranaaduKannadigaAttach").hide(); $(".OtherCertificatesAttach").hide();
    $(".KashmirMigrantsAttach").hide(); $(".ExservicemanAttach").hide(); $(".LLCertificateAttach").hide(); $(".EWSCertificateAttach").hide();
    $("#DocRurCerAcceptedImg").hide();
    
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
        },
        beforeShow: function (dateText) {

        }
    });
    $(".date-picker").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    }).attr("readonly", false).datepicker('setDate', new Date(new Date().getFullYear(), 7));

    $('#dateofbirth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "-50:-13",
        dateFormat: 'dd-mm-yy',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });

    $('#AdmisionTime').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: '-30d',
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        dateFormat: 'dd-mm-yy'
    });

    $('#PaymentDate').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: 0,
        maxDate: '30d',
        dateFormat: 'dd-mm-yy'
    });

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


$('a[href="#tab_1"]').click(function () {
    $("#tab_1").show();
    $("#tab_2").hide();
    $("#tab_2").removeClass("tab-pane fade show active in");
    $("#tab_2").addClass("tab-pane fade");
    $(".ApplicationFormDetails").find("*").prop('disabled', false);
    $(".ApplicationFormDetails").hide();
});


$('a[href="#tab_2"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").show();
    $("#tab_1").removeClass("tab-pane fade show active in");
    $("#tab_1").addClass("tab-pane fade");
    $(".ApplicationFormDetails").hide();
    GetDataAdmissionApplicants();
});

function GetAdmissionUnitsShifts(UnitsDetails, ShiftsDetails) {
    
    if ($("#TradeDetails").val() != "choose") {
        $("#UnitsDetails").empty();
        $("#UnitsDetails").append('<option value="choose">Choose</option>');
        $("#ShiftsDetails").empty();
        $("#ShiftsDetails").append('<option value="choose">Choose</option>');

        var collegeId = $("#CollegeId").text();
        var tradeId = $("#TradeDetails").val();

        $.ajax({
            url: "/Admission/GetUnitsShiftsDetails",
            type: 'Get',
            data: { CollegeId: collegeId, TradeId: tradeId },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        if (!$("#UnitsDetails option:contains(" + this.UnitsId + ")").length > 0) {
                            $("#UnitsDetails").append($("<option/>").val(this.UnitsId).text(this.UnitsValue));
                        }
                        $("#ShiftsDetails").append($("<option/>").val(this.ShiftId).text(this.ShiftValue));
                    });
                }

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
}





function GetInstituteMaster() {
    $("#TradeDetails").empty();
    $("#TradeDetails").append('<option value="choose">Choose</option>');
    $.ajax({
        type: 'Get',
        url: '/Admission/GetInstituteMaster',
        success: function (data) {
            if (data != null || data != '') {
                $("#DivisionId").text(data.Division);
                $("#DivisionName").text(data.DivisionName);
                $("#DistrictCd").text(data.District);
                $("#DistrictName").text(data.DistrictName);
                $("#TalukId").text(data.Taluk);
                $("#TalukName").text(data.TalukName);
                $("#MISCode").text(data.MISCode);
                $("#InstituteName").text(data.ITIInstituteName);
                $("#InstituteType").text(data.InstituteTypeDet);
                $("#CollegeId").text(data.CollegeId);
                $("#txtApplRemarks").text(data.Remarks);
                $("#AdmissionFee").text(data.TuitionFee);
                $.each(data.TradeDetails, function () {
                    $("#TradeDetails").append($("<option/>").val(this.TradeName).text(this.TradeNameDet));
                });
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetDataAdmissionApplicants() {
    var IsValid = true;
    if (IsValid) {
        $.ajax({
            type: "GET",
            url: "/Admission/DirectAdmissionApplicantDetails",
            contentType: "application/json",
            success: function (data) {

                $('#DirectAdmissionApplicantDetails').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },                       
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
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsViewById(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id ='view'/> ");
                               // <input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit' />


                            },
                            'className' : 'text-center'
                        },
                    ]
                });
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}

function NavigateTabApplicantDetails() {
    $("#accordion").hide();
    $("#AppFormID").hide();
    $(".close").hide();
    $("#tab_2").hide();
    var NewOldApplication = $('input[name=NewOldApplication]:checked').val();
    $(".ApplicationFormDetails").find("*").prop('disabled', false);
    if (NewOldApplication == 1) {
        $(".EnableGetApplicationNumber").show();
        $(".ApplicationFormDetails").find("*").prop('disabled', true);
        $('#collapseFive').find("*").prop('disabled', false);
        $('#EnableDisableSubmit').find("*").prop('disabled', false);
        $(".close").prop('disabled', false);
    }
    else
        $(".EnableGetApplicationNumber").hide();
    $('#ExistChkApplicationNumber').val('');
}

function ToGetApplicationDeatilsForOldApplicant() {
    //$("#accordion").show();
    //$("#AppFormID").show();
    //$("#tab_2").hide();
    var NewOldApplication = $('input[name=NewOldApplication]:checked').val();
    var ExistChkApplicationNumber = $('#ExistChkApplicationNumber').val();
    var ErrFlag = 0;

    $("#ExistChkApplicationNumber-Required").hide();
    if (NewOldApplication == 1 && ExistChkApplicationNumber == '') {
        $("#ExistChkApplicationNumber-Required").show();
        ErrFlag = 1;
    }

    if (ErrFlag == 0) {
        GetVerticalCategory("VerticalCategory");
        GetHorizontalCategory("HorizontalCategory");
        GetApplicantType("TraineeType");
        GetUnitsDetails("UnitsDetails");
        GetShiftsDetails("ShiftsDetails");

        if (NewOldApplication == 0) {
            $("#accordion").show();
            $("#AppFormID").show();
            bootbox.alert("<br><br> Enter the Applicant Details detail in appropriate panel");
            GetApplicationDetailsById(0);
            fnEnableDisableEWS();
            GetInstituteMaster();
        }
        else {
            $(".ApplicationFormDetails").find("*").prop('disabled', true);
            $('#collapseFive').find("*").prop('disabled', false);
            $('#EnableDisableSubmit').find("*").prop('disabled', false);
            $(".close").prop('disabled', false);

            $.ajax({
                type: 'Get',
                data: { ExistChkApplicationNumber: ExistChkApplicationNumber },
                url: '/Admission/GetApplIdByApplicationNumber',
                success: function (datajson) {
                    if (datajson == 0) {
                        $("#accordion").hide();
                        $("#AppFormID").hide();
                        $("#tab_2").hide();
                        ErrFlag = 1;
                        bootbox.alert("<br><br> Application Number : <b>" + ExistChkApplicationNumber + "</b> is Invalid or does NOT exist, Kindly enter a valid Application Number");


                    }
                    else {
                        ErrFlag = 0;
                        $("#ApplicantId").val(datajson);
                        GetInstituteMaster();
                    }
                    if (ErrFlag == 0) {
                        bootbox.confirm("<br><br>Are you sure to get the application details for Application Number : <b>" + ExistChkApplicationNumber + "<b> ?", function (confirmed) {
                            if (confirmed) {
                                //  $('.nav-tabs li:eq(1) a').tab('show');
                                //$("#tab_1").hide();
                                //$("#tab_2").show();
                                $("#tab_1").show();
                                $("#tab_2").hide();
                                GetApplicationDetailsById($("#ApplicantId").val());
                            }
                        });
                    }

                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
    }
}

function OnChangeAdmFeePaidStatus() {
    var AdmFeePaidStatus = $("input[name='AdmFeePaidStatus']:checked").val();
    if (AdmFeePaidStatus == 1) {
        SaveSeatAllocationFeePay(0);
    }
}

function SaveSeatAllocationFeePay(PayMode) {

    var IsValid = true;
    $("#TraineeType-Required").hide();
    var TraineeType = $("#TraineeType :selected").val();
    var UnitsDetail = $("#UnitsDetails :selected").val();
    var ShiftsDetail = $("#ShiftsDetails :selected").val();
    if (TraineeType == "choose") {
        $("#TraineeType-Required").show();
        $("#TraineeType").focus();
        IsValid = false;
    }
    if (UnitsDetail == "choose") {
        $("#UnitsDetails-Required").show();
        $("#UnitsDetails").focus();
        IsValid = false;
    }
    if (ShiftsDetail == "choose") {
        $("#ShiftsDetails-Required").show();
        $("#ShiftsDetails").focus();
        IsValid = false;
    }
    $("#AdmisionTime-Required").hide();
    if ($("#AdmisionTime").val() == "") {
        $("#AdmisionTime-Required").show();
        $("#AdmisionTime").focus();
        IsValid = false;
    }
    //$("#SaveSeatAllocationFeePay").attr("disabled", false);
    if (IsValid) {
        bootbox.confirm("<br><br>Are you sure to Pay the amount?", function (confirmed) {
            if (confirmed) {

                var fileData = new FormData();
                var AdmisionFeeVal = $("#AdmissionFee").text();
                var AdmisionTime = "";
                var dts = $("#AdmisionTime").val().split("-");
                conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                AdmisionTime = (dts[0] + "-" + dts[1] + "-" + dts[2]);

                var DualType = $("input[name='DualType']:checked").val();
                var ITIUnderPPP = $("input[name='ITIUnderPPP']:checked").val();
                var TradeName = $("#TradeDetails :selected").text();
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
                    "TradeName", TradeName
                );
                fileData.append(
                    "MISCode", $("#MISCode").text()
                );

                $.ajax({
                    type: "POST",
                    url: "/PaymentPDFGeneration/GeneratePaymentReceiptPDFData",
                    data: fileData,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        if (data != null) {
                            //$("#SaveSeatAllocationFeePay").attr("disabled", true);

                            $(".AdmisionFee").prop('disabled', true);
                            $(".AdmFeePaidStatus").prop('disabled', true);
                            $("#PaymentDate").prop('disabled', true);
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
                           
                            var PaymentDate = daterangeformate2(data[0].PaymentDate, 1);

                            $("#PaymentDate").val(PaymentDate);

                            if (data[0].AdmittedStatus == 2)
                                $("input[name=AdmittedStatus][value=2]").prop('checked', true);
                            //else
                            //    $("input[name=AdmittedStatus][value=3]").prop('checked', true);

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
        });
    }
    else {
        $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);
        bootbox.alert("<br><br>There is error in your page!!");
    }
}

$('a[href="#tab_1"]').click(function () {
    $("#tab_1").show();
    $("#tab_2").hide();
});

$('a[href="#tab_2"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").show();

    $('#ExistChkApplicationNumber').val('');
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
    var idlen = id.length;
    var idlenChk = id.substring(idlen, idlen - 1);

    if (idlenChk == "txtFamilyAnnualIncome") {
        eligibleLen = 8;
    }
    else if (idlenChk == "txtPermanentPincode" || id == "txtPincode") {
        eligibleLen = 5;
    }
    else if (idlenChk == "txtMaximumMarks" || id == "txtMarksObtained") {
        eligibleLen = 2;
    }
    else if (idlenChk == "AccountNumber") {
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
        return false;
    } else {
        return true;
    }
}

function getSelectedOptions(sel) {

    var idlen = sel.length;
    var idlenChk = sel.substring(idlen - 1);

    $("#DiffAbledCer" + idlenChk).val(0);
    $("#ExServiceCer" + idlenChk).val(0);
    $("#LandLoserCer" + idlenChk).val(0);
    $("#EcoWeakCer" + idlenChk).val(0);
    $("#KMCer" + idlenChk).val(0);
    var len = sel.options.length;
    for (var i = 0; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected) {
            if (opt.value == "2")
                $("#ExServiceCer" + idlenChk).val(1);
            else if (opt.value == "5")
                $("#EcoWeakCer" + idlenChk).val(1);
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
    //$('#EnableSendTo').hide();
}

function OnClickEditSubmit() {

    var IsValid = true;

    $("#academicyear1-Required").hide();
    var AcademicMonths = $("#AcademicMonths").val();
    var AcademicYear = $("#AcademicYear").val();
    if (AcademicMonths == "" || AcademicYear == "") {
        $("#academicyear1-Required").show();
        IsValid = false;
    }

    $("#TraineeType-Required").hide();
    var TraineeType = $("#TraineeType :selected").val();
    var UnitDetail = $("#UnitsDetails :selected").val();
    var ShiftDetail = $("#ShiftsDetails :selected").val();
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

    if (IsValid) {

        let PercentageReturnValue = CalculatePercentage(IsValid);
        IsValid = PercentageReturnValue.IsValidP;
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

                    if (IsValid) {

                        bootbox.confirm("<br><br>Are you sure to Update the Applicant Admitted Details?", function (confirmed) {
                            if (confirmed) {

                                var fileData = new FormData();
                                var AdmisionFeeVal = $("#AdmissionFee").text();
                                var AdmisionTime = "";
                                var dts = $("#AdmisionTime").val().split("-");
                                conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                                AdmisionTime = (dts[0] + "-" + dts[1] + "-" + dts[2]);

                                var DualType = $("input[name='DualType']:checked").val();
                                var ITIUnderPPP = $("input[name='ITIUnderPPP']:checked").val();
                                var AdmFeePaidStatus = $("input[name='AdmFeePaidStatus']:checked").val();
                                var AdmittedStatus = $("input[name='AdmittedStatus']:checked").val();
                                var AdmissionTypeID = 1;
                                if ($('input[name=NewOldApplication]:checked').val() == 0) {
                                    ToSuccessfulSaveYesDataSave();
                                }
                                fileData.append(
                                    "ApplyYear", AcademicYear
                                );
                                fileData.append(
                                    "ApplicationId", $("#ApplicantId").val()
                                );
                                fileData.append(
                                    "ApplicantNumber", $("#ApplicantNumber").val()
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
                                    "Remarks", $("#txtApplRemarks").val(),
                                );
                                fileData.append(
                                    "ApplInstiStatus", AdmittedStatus,
                                );
                                fileData.append(
                                    "AdmissionTypeID", AdmissionTypeID,
                                );
                                fileData.append(
                                    "Unitid", UnitDetail,
                                );
                                fileData.append(
                                    "Shiftid", ShiftDetail,
                                );
                                fileData.append(
                                    "HorizontalCategory", $("#HorizontalCategory").val(),
                                );
                                fileData.append(
                                    "VerticalCategory", $("#VerticalCategory").val(),
                                );
                                fileData.append(
                                    "TradeId", $("#TradeDetails").val(),
                                );
                                $.ajax({
                                    type: "POST",
                                    url: "/Admission/UpdateAdmittedDetails",
                                    data: fileData,
                                    contentType: false,
                                    processData: false,
                                    success: function (data) {
                                        bootbox.alert("<br><br>" + data[0].UpdateMsg);
                                        $(".EditOptionShowGrid").hide();
                                        GetDataAdmissionApplicants();
                                        OnClickEditCancel();
                                    }
                                });

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

function ToSuccessfulSaveYesDataSave() {
    var GenerateApplicationNumberObj = {
        ApplicantId: $("#ApplicantId").val(),
        ApplicantNumber: $("#ApplicantNumber").val(),
        ApplyYear: $("#AcademicYear").val(),
        ApplicantType: $("#ApplicantTypeSelect :selected").val(),
        DistrictId: $("#Districts :selected").val(),
        ApplRemarks: $("#txtApplRemarks").val(),
        FlowId: $("#ApplFlowId").val(),
        CredatedBy: $("#ApplCredatedBy").val()
    }

    $.ajax({
        type: 'POST',
        url: '/Admission/GenerateApplicationNumber',
        data: GenerateApplicationNumberObj,
        success: function (result) {

            if (result != null) {
                GetMasterData();
                GetApplicantsStatus();
                if ($("#txtDocumentFeeReceiptDetails").val() == "" || $("#txtDocumentFeeReceiptDetails").val() == null)
                    bootbox.alert("<br><br>" + result.UpdateMsg + " , but your document verification fee is pending. It has to be paid at <b>" + $("#InstituteList :selected").text() + "</b> before due date. Once you paid that document verification fee, you have to enter the receipt details in the application form then only application will be considered as submitted successfully.");
                else
                    //bootbox.alert("<br><br>" + result.UpdateMsg + " .Your Application submitted successfully.");
                    bootbox.alert("<br><br>" + result.UpdateMsg + " , Applicant should retain the same mobile number and email till the examination ."  );
            }
            else {
                bootbox.alert("<br><br><h3>" + result.UpdateMsg + "<h3>");
            }
        }
    });
}




function OnClickEditCancel() {
 //   window.location = '/Admission/ApplicantAdmissionAgainstVacancy';
   // ("#tab_1").hide();
    $(".close").hide();
    if ($(".tab-pane.active").attr("id") == "tab_1") {
        $("#accordion").hide();
        $("#AppFormID").hide();
        $(".container").show();
    }
    else {
        $("#accordion").hide();
        $("#AppFormID").hide();
        $(".DirectAdmissionApplicantDetails").show();
    }
}


$('.link').click(function (e) {
    e.preventDefault();
    var id = $(this).attr('id');
    var linktoRedirect = $("#" + id).attr('href');
    window.open(linktoRedirect, '_blank');;
});

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
        $(".TenthCOBSEType").hide();
        $("input[name=TenthCOBSEBoard][value=2]").prop('checked', true)
        MarksCGPAType();
    }
    else {
        $(".TenthCOBSEType").show();
        TenthCOBSEBoardType();
        MarksCGPAType();
    }
}

function MarksCGPAType() {
    var selectedMarksCGPAType = $('input[name="CGPAMarks"]:checked').val();
    $(".OtherBoardsType").hide();
    if ($('input[name="TenthCOBSEBoard"]:checked').val() == 7) {
        $(".OtherBoardsType").show();
    }
    if (selectedMarksCGPAType == 1 && $('.TenthCOBSEType').is(':visible')) {
        $(".Marks").find("*").prop('disabled', true);
        $("#txtCGPA").prop('disabled', false);
        $('.Marks').find('input[type="number"]').val('');
    }
    else {
        $(".Marks").find("*").prop('disabled', false);
        $("#txtCGPA").prop('disabled', true);
        $("#txtCGPA").val("");
    }
    CalculatePercentage(true);
}

function TenthCOBSEBoardType() {
    var selectedTenthCOBSEBoard = $('input[name="TenthCOBSEBoard"]:checked').val();
    if (selectedTenthCOBSEBoard == 2 || selectedTenthCOBSEBoard == 3) {
        $("#TenthCBSEICSEType").show();
    }
    else {
        $("#TenthCBSEICSEType").hide();
    }
    MarksCGPAType();
}

function RuralUrbanLocation(id) {

    $("input:radio[name=RuralUrban" + id + "]").prop('checked', true);
    $("input[name='RuralUrban" + id + "]").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}


function GetTaluka(valueChange) {

    var Districts = "";
    if (valueChange != "Permanent") {
        Districts = $('#Districts :selected').val();
        $("#Talukas").empty();
        $("#Talukas").append('<option value="0">Select</option>');
    }
    else {
        Districts = $('#PermanentDistricts :selected').val();
        $("#PermanentTalukas").empty();
        $("#PermanentTalukas").append('<option value="0">Select</option>');
    }

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (valueChange != "Permanent")
                        $("#Talukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                    else
                        $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetTalukaForPermanent(valueChange, talukChange) {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: valueChange },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetTalukBySameAsCommunication(valueChange) {

    var Districts = valueChange;
    $("#PermanentTalukas").empty();
    $("#PermanentTalukas").append('<option value="0">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function AppliedWhichBasics(id) {

    $("input:radio[name=AppBasics" + id + "]").prop('checked', true);
    $("input[name=AppBasics" + id + "]").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedForSyallbus(id) {

    $("input[name=TenthBoard " + id +"][value='1']").prop('checked', true)   
    $("input[name=TenthBoard" + id + "]").each(function () {
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

function GetApplicationDetailsViewById(ApplicationId) {
    $(".DirectAdmissionApplicantDetails").hide();
    GetApplicationDetailsById(ApplicationId);
    $(".ApplicationFormDetails").find("*").prop('disabled', true);
    $(".close").prop('disabled', false);

}

function GetApplicationDetailsById(ApplicationId) {
    var param = $("#ApplicantId").val();
    var base = $("#DownloadReceipt").data('url');
    $("#DownloadReceipt").attr('href', base + '?ApplicationId=' + param);

    $(".ApplicationFormDetails").show();
    $("#EditSubmitbtn").show();
    $('input[type="text"], textarea').attr('readonly', false);

    if (ApplicationId == 0) {
        RuralUrbanLocation('');
        AppliedForSyallbus('');
        AppliedWhichBasics('');
    }

    $("#Category").empty();
    $("#Category").append('<option value="0">Select Category</option>');

    $("#Religion").empty();
    $("#Religion").append('<option value="0">Select Religion</option>');

    $("#Gender").empty();
    $("#Gender").append('<option value="0">Select Gender</option>');

    $("#ApplicantTypeSelect").empty();
    $("#ApplicantTypeSelect").append('<option value="0">Select Applicant Type</option>');

    $("#Qualification").empty();
    $("#Qualification").append('<option value="0">Select Qualification</option>');

    $("#Districts").empty();
    $("#Districts").append('<option value="0">Select</option>');

    $("#PermanentDistricts").empty();
    $("#PermanentDistricts").append('<option value="0">Select</option>');

    $("#PhysicallyHanidcapType").empty();
    $("#PhysicallyHanidcapType").append('<option value="0">Select Disability</option>');

    $("#KanMedCer").val(0); $("#DiffAbledCer").val(0); $("#HoraNaduCer").val(0); $("#ExeStuCer").val(0); $("#HydKarCer").val(0);
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
            $("#DocRurCerAcceptedImg").hide();
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
            $("#EduCertificate").val('');
            $("#CasteCertificate").val('');
            $("#Rationcard").val('');
            $("#Incomecertificate").val('');
            $("#UIDNumber").val('');
            $("#Ruralcertificate").val('');
            $("#KannadamediumCertificate").val('');
            $("#Differentlyabledcertificate").val('');
            $("#ExemptedCertificate").val('');
            $("#HyderabadKarnatakaRegion").val('');
            $("#HoranaaduKannadiga").val('');
            $("#OtherCertificates").val('');
            $("#Exserviceman").val('');
            $("#EWSCertificate").val('');
            $("#AdmissionFee").val('');
            $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

                $("#ApplicantId").val(datajson.Resultlist.ApplicationId);
                $("#ApplicantNumber").val(datajson.Resultlist.ApplicantNumber);
                $("#ApplCredatedBy").val(datajson.Resultlist.CredatedBy);
                $("#ApplFlowId").val(datajson.Resultlist.FlowId);
                //ApplicantDetails
                //Get the roll number
                $("input[name=RStateBoardType][value=" + datajson.Resultlist.RStateBoardType + "]").prop('checked', true);
                $("input[name=RAppBasics][value=" + datajson.Resultlist.RAppBasics + "]").prop('checked', true);
                $("#txtRollNumber").val(datajson.Resultlist.RollNumber);
                $("#AdmissionFee").text(datajson.Resultlist.InstituteFee);


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
                        if (this.ApplicantTypeId == ApplicantTypes.Direct) {
                            $("#ApplicantTypeSelect").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                        }
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

                $("#OtherBoards").empty();
                $("#OtherBoards").append('<option value="0">Select Board/Institute</option>');

                if (datajson.Resultlist.GetDocumentApplicationStatus.length > 0) {
                    $.each(datajson.Resultlist.GetDocumentApplicationStatus, function (index, item) {
                        if (this.ApplDocVerifiID != 14) {
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
                        }
                    });
                }

                $("input[name=ExServiceMan][value=0]").prop('checked', true);
                $("input[name=ExServiceMan][value=0]").prop('checked', true);
                var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;
                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        if (e == "2") {
                            $("input[name=ExServiceManRDB][value=1]").prop('checked', true);
                            $("#ExServiceCer").val(1);
                        }
                        else if (e == "5") {
                            $("input[name=EWSCertificateRDB][value=1]").prop('checked', true);
                            $("#EcoWeakCer").val(1);
                        }

                    });
                }

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#Districts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        $("#PermanentDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    });
                }

                if (datajson.Resultlist.GetOtherBoards.length > 0) {
                    $.each(datajson.Resultlist.GetOtherBoards, function (index, item) {
                        $("#OtherBoards").append($("<option/>").val(this.BoardId).text(this.BoardName));
                    });
                }

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {
                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach").show();
                                $('#aEduCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks").val(this.Remarks);
                            }
                            $("#EDocAppId").val(this.DocAppId);
                            $("#ECreatedBy").val(this.CreatedBy);
                            $("#EduDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEduCerAcceptedImg").show();
                                $("#DocEduCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEduCerRejectedImg").show();
                                $("#DocEduCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach").show();
                                $('#aCasteCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks").val(this.Remarks);
                            }
                            $("#CDocAppId").val(this.DocAppId);
                            $("#CCreatedBy").val(this.CreatedBy);
                            $("#CasDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocCasCerAcceptedImg").show();
                                $("#DocCasCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocCasCerRejectedImg").show();
                                $("#DocCasCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach").show();
                                $('#aRationCard').attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks").val(this.Remarks);
                            }
                            $("#RDocAppId").val(this.DocAppId);
                            $("#RCreatedBy").val(this.CreatedBy);
                            $("#RationDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRatCerAcceptedImg").show();
                                $("#DocRatCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRatCerRejectedImg").show();
                                $("#DocRatCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach").show();
                                $('#aIncomeCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks").val(this.Remarks);
                            }
                            $("#IDocAppId").val(this.DocAppId);
                            $("#ICreatedBy").val(this.CreatedBy);
                            $("#IncCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocIncCerAcceptedImg").show();
                                $("#DocIncCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocIncCerRejectedImg").show();
                                $("#DocIncCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach").show();
                                $('#aUIDNumber').attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks").val(this.Remarks);
                            }
                            $("#UDocAppId").val(this.DocAppId);
                            $("#UCreatedBy").val(this.CreatedBy);
                            $("#UIDDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocUIDCerAcceptedImg").show();
                                $("#DocUIDCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocUIDCerRejectedImg").show();
                                $("#DocUIDCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 6) {
                            $(".RuralUrban").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach").show();
                                $('#aRuralcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks").val(this.Remarks);
                            }
                            $("#RUDocAppId").val(this.DocAppId);
                            $("#RUCreatedBy").val(this.CreatedBy);
                            $("#RcerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRurCerAcceptedImg").show();
                                $("#DocRurCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRurCerRejectedImg").show();
                                $("#DocRurCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 7) {
                            $(".KanndaMedium").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach").show();
                                $('#aKannadamediumCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks").val(this.Remarks);
                            }
                            $("#KDocAppId").val(this.DocAppId);
                            $("#KCreatedBy").val(this.CreatedBy);
                            $("#KanMedCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocKanMedCerAcceptedImg").show();
                                $("#DocKanMedCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocKanMedCerRejectedImg").show();
                                $("#DocKanMedCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach").show();
                                $('#aDifferentlyabledcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks").val(this.Remarks);
                            }
                            $("#DDocAppId").val(this.DocAppId);
                            $("#DCreatedBy").val(this.CreatedBy);
                            $("#DiffAblDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocDidAblCerAcceptedImg").show();
                                $("#DocDidAblCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocDidAblCerRejectedImg").show();
                                $("#DocDidAblCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 9) {
                            $(".ExemptedFromStudyCertificate").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach").show();
                                $('#aExemptedCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks").val(this.Remarks);
                            }
                            $("#ExDocAppId").val(this.DocAppId);
                            $("#ExCreatedBy").val(this.CreatedBy);
                            $("#ExCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExStuCerAcceptedImg").show();
                                $("#DocExStuCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExStuCerRejectedImg").show();
                                $("#DocExStuCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 10) {
                            $(".HyderabadKarnatakaRegion").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach").show();
                                $('#aHyderabadKarnatakaRegion').attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks").val(this.Remarks);
                            }
                            $("#HDocAppId").val(this.DocAppId);
                            $("#HCreatedBy").val(this.CreatedBy);
                            $("#HyKarDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHyKarRegAcceptedImg").show();
                                $("#DocHyKarRegRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHyKarRegRejectedImg").show();
                                $("#DocHyKarRegAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 11) {
                            $(".HoraNadu").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach").show();
                                $('#aHoranaaduKannadiga').attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks").val(this.Remarks);
                            }
                            $("#HGKDocAppId").val(this.DocAppId);
                            $("#HGKCreatedBy").val(this.CreatedBy);
                            $("#HorKanDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHorGadKanAcceptedImg").show();
                                $("#DocHorGadKanRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHorGadKanRejectedImg").show();
                                $("#DocHorGadKanAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 12) {
                            if (this.FilePath != null) {
                                $(".OtherCertificatesAttach").show();
                                $('#aOtherCertificates').attr('href', '' + this.FilePath + '');
                                $("#txtOtherCertRemarks").val(this.Remarks);
                            }
                            $("#ODocAppId").val(this.DocAppId);
                            $("#OCreatedBy").val(this.CreatedBy);
                            $("#OtherCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocOthCerAcceptedImg").show();
                                $("#DocOthCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocOthCerRejectedImg").show();
                                $("#DocOthCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach").show();
                                $('#aExserviceman').attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks").val(this.Remarks);
                            }
                            $("#ExSDocAppId").val(this.DocAppId);
                            $("#ExSCreatedBy").val(this.ExSCreatedBy);
                            $("#ExserDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExSerAcceptedImg").show();
                                $("#DocExSerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExSerRejectedImg").show();
                                $("#DocExSerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach").show();
                                $('#aEWSCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks").val(this.Remarks);
                            }
                            $("#EWSDocAppId").val(this.DocAppId);
                            $("#EWSCreatedBy").val(this.EWSCreatedBy);
                            $("#EWSDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEWSAcceptedImg").show();
                                $("#DocEWSRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEWSRejectedImg").show();
                                $("#DocEWSAcceptedImg").hide();
                            }
                        }
                    });
                }
                if (datajson.Resultlist.ApplyMonth == null)
                    datajson.Resultlist.ApplyMonth = 7;
                if (datajson.Resultlist.ApplyYear == null)
                    datajson.Resultlist.ApplyYear = new Date().getFullYear();
                $("#ApplStatus").val(datajson.Resultlist.ApplStatus);
                $('#AcademicYear').val(datajson.Resultlist.ApplyYear);
                $('#AcademicMonths').val(datajson.Resultlist.ApplyMonth);
                $("#academicyear1").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                $("#txtAadhaarNumber").val(datajson.Resultlist.AadhaarNumber);
                $("#txtRationCard").val(datajson.Resultlist.RationCard);
                $("#AccountNumber").val(datajson.Resultlist.AccountNumber);
                $("#BankName").val(datajson.Resultlist.BankName);
                $("#IFSCCode").val(datajson.Resultlist.IFSCCode);
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
                else
                    $("input[name=HoraNadu][value=0]").prop('checked', true);

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

                if (datajson.Resultlist.ExServiceMan == true) {
                    $("input[name=ExServiceMan][value=1]").prop('checked', true);
                    $("#ExServiceManCer").val(1);
                }
                else
                    $("input[name=ExServiceMan][value=0]").prop('checked', true);

                if (datajson.Resultlist.EconomyWeakerSection == true) {
                    $("input[name=EconomyWeakerSection][value=1]").prop('checked', true);
                    $("#EconomyWeakerSectionCer").val(1);
                }
                else
                    $("input[name=EconomyWeakerSection][value=0]").prop('checked', true);

                $("#txtApplicantName").val(datajson.Resultlist.ApplicantName);
                $("#txtFathersName").val(datajson.Resultlist.FathersName);
                $("#txtParentOccupation").val(datajson.Resultlist.ParentsOccupation);
                $('#ImgPhotoUpload').attr("src", datajson.Resultlist.Photo);
                $("#IsUploaded").val(datajson.Resultlist.Photo);

                var finalDOB = daterangeformate2(datajson.Resultlist.DOB, 1);
                $("#dateofbirth").val(finalDOB);
                $("#txtMothersName").val(datajson.Resultlist.MothersName);
                if (datajson.Resultlist.Religion != null)
                $("#Religion").val(datajson.Resultlist.Religion);
                else
                $("#Religion").val(0);
                if (datajson.Resultlist.Gender != null)
                    $('#Gender').val(datajson.Resultlist.Gender);
                else
                    $('#Gender').val(0);

                if (datajson.Resultlist.Category != null)
                    $("#Category").val(datajson.Resultlist.Category);
                else
                    $("#Category").val(0);

                if (datajson.Resultlist.MinorityCategory != null)
                    $("#MinorityCategory").val(datajson.Resultlist.MinorityCategory);
                else
                    $("#MinorityCategory").val(0);
                $("#txtCaste").val(datajson.Resultlist.Caste);
                $("#txtFamilyAnnualIncome").val(datajson.Resultlist.FamilyAnnIncome);
                if (datajson.Resultlist.ApplicantType != null)
                $("#ApplicantTypeSelect").val(datajson.Resultlist.ApplicantType);
                else
                $("#ApplicantTypeSelect").val(0);

                //EducationDetails
                if (datajson.Resultlist.GetQualificationList.length > 0) {
                    $.each(datajson.Resultlist.GetQualificationList, function (index, item) {
                        $("#Qualification").append($("<option/>").val(this.QualificationId).text(this.Qualification));
                    });
                }
                if (datajson.Resultlist.Qualification != null)
                    $('#Qualification').val(datajson.Resultlist.Qualification);
                else
                    $('#Qualification').val(0);

                $("input[name=RuralUrban][value=" + datajson.Resultlist.ApplicantBelongTo + "]").prop('checked', true);
                $("input[name=AppBasics][value=" + datajson.Resultlist.AppliedBasic + "]").prop('checked', true);
                $("input[name=TenthBoard][value=" + datajson.Resultlist.TenthBoard + "]").prop('checked', true);
                //$("#txtEduGrade").val(datajson.Resultlist.EducationGrade);
                $("input[name=TenthCOBSEBoard][value=" + datajson.Resultlist.TenthCOBSEBoard + "]").prop('checked', true);
                $("#txtInstituteStudied").val(datajson.Resultlist.InstituteStudiedQual);
                $("#txtMaximumMarks").val(datajson.Resultlist.MaxMarks);
                $("#txtMarksObtained").val(datajson.Resultlist.MarksObtained);
                $("#txtCGPA").val(datajson.Resultlist.EducationCGPA);
                TenthBoardStateType();
                if (datajson.Resultlist.BoardId != null)
                    $("#OtherBoards").val(datajson.Resultlist.BoardId);
                if (datajson.Resultlist.Percentage != null && datajson.Resultlist.Percentage != "")
                    $("#lblPercAsPerMarks").text(datajson.Resultlist.Percentage + "%");
                $("#Results").val(datajson.Resultlist.ResultQual);
                if (datajson.Resultlist.studiedMathsScience == true)
                    $("input[name=studiedMathsScience][value=1]").prop('checked', true);
                else
                    $("input[name=studiedMathsScience][value=0]").prop('checked', true);
                //AddressDetails
                $("#txtCommunicationAddress").val(datajson.Resultlist.CommunicationAddress);
                if (datajson.Resultlist.DistrictId != null)
                    $("#Districts").val(datajson.Resultlist.DistrictId);
                else
                    $("#Districts").val(0);

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#Talukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }
                if (datajson.Resultlist.TalukaId != null)
                    $('#Talukas').val(datajson.Resultlist.TalukaId);
                else
                    $('#Talukas').val(0);

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
                if (datajson.Resultlist.PDistrict != null)
                    $("#PermanentDistricts").val(datajson.Resultlist.PDistrict);
                else
                    $("#PermanentDistricts").val(0);

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }

                if (datajson.Resultlist.PTaluk != null)
                    $('#PermanentTalukas').val(datajson.Resultlist.PTaluk);
                else
                    $('#PermanentTalukas').val(0);
                $("#txtPermanentPincode").val(datajson.Resultlist.PPinCode);
                $("#txtApplicantPhoneNumber").val(datajson.Resultlist.PhoneNumber);
                $("#txtFathersPhoneNumber").val(datajson.Resultlist.FatherPhoneNumber);
                $("#txtEmailId").val(datajson.Resultlist.EmailId);
                $("#txtRemarks").val('');

                $("#academicyear2").text($("#academicyear1").val());
                $("#ApplicationNumber").text($("#ExistChkApplicationNumber").val());
                $("#ApplicationName").text(datajson.Resultlist.ApplicantName);

            }

            fnClearAllocatedFeeDetails();
            $.ajax({
                type: "GET",
                url: "/Admission/GetDataAllocationFeeDetails",
                data: { ApplicationId: ApplicationId },
                contentType: "application/json",
                success: function (datajsonDetails) {

                    $.each(datajsonDetails.Resultlist, function (index, item) {
                        if ((this.AdmissionTypeID == 1 || this.AdmittedStatus == 6 )&& $(".tab-pane.active").attr("id") == "tab_1") {
                            $("#accordion").hide();
                            $("#AppFormID").hide();
                            $("#tab_2").hide();
                            ErrFlag = 1;
                            bootbox.alert("<br><br> </b> The Entered Application Number : <b>" + $('#ExistChkApplicationNumber').val() + "</b> has been admitted to the ITI Institute, Kindly enter a different Application Number.");

                        }
                        else {

                            $("#accordion").show();
                            $("#AppFormID").show();
                        }
                        //Allocation and Fee Details
                      //  $("#Units").text(this.Units);
                      //  $("#Shifts").text(this.Shifts);
                        $("#ApplicationNumber").text(this.ApplicantNumber);
                        $("#DivisionId").text(this.DivisionId);
                        $("#DivisionName").text(this.DivisionName);
                        $("#DistrictCd").text(this.DistrictId);
                        $("#DistrictName").text(this.DistrictName);
                        $("#TalukId").text(this.TalukId);
                        $("#TalukName").text(this.TalukName);
                        $("#MISCode").text(this.MISCode);
                        $("#InstituteName").text(this.InstituteName);
                        $("#InstituteType").text(this.InstituteType);
                      //  $("#TradeDetails").text(this.TradeName);
                      //  $("#TradeDetails").hide();

                       //var Trexs = $("#TradeName").val(this.TradeName);//Drop down
                       // if (this.TradeName != null || this.TradeName != 0) {
                       //     $("#TradeName").val(Trexs);
                       // }
                        //$("#TradeName").append($("<option/>").text(this.TradeName));
                        if (this.AdmFeePaidStatus == 1) {
                            $(".AdmisionFee").prop('disabled', true);
                            $(".AdmFeePaidStatus").prop('disabled', true);
                            $("#PaymentDate").prop('disabled', true);
                            $(".PaymentGenerationGridCls").show();
                            $("#AllocationFeePay").hide();
                            $("input[name=AdmittedStatus][value=6]").prop('checked', true);
                          //  fnShowHidePaymentInfo();

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

                        if (this.AdmittedStatus == 2) {
                            $("input[name=AdmittedStatus][value=2]").prop('checked', true);
                    }
                        if (this.AdmFeePaidStatus == 1)
                            $("input[name=AdmFeePaidStatus][value=1]").prop('checked', true);
                        else
                            $("input[name=AdmFeePaidStatus][value=0]").prop('checked', true);

                        $("#TraineeType").val(this.TraineeType);//Drop down
                        if (this.TraineeType == null || this.TraineeType == 0) {
                            $("#TraineeType").val(1);
                        }

                        var AdmisionTime = daterangeformate2(this.AdmisionTime, 1);
                        $("#AdmisionTime").val(AdmisionTime);

                        var PaymentDate = daterangeformate2(this.PaymentDate, 1);

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
                        $("#txtApplRemarks").text(this.Remarks);

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
                        if (this.AdmissionTypeID == 1) 

                        
                            $("#EditSubmitbtn").hide();
                        else 
                            $("#EditSubmitbtn").show();
                        if (this.AdmittedStatus == 3)
                            $("input[name=AdmittedAdmissionStatus][value=3]").prop('checked', true);
                        else
                            $("input[name=AdmittedAdmissionStatus][value=6]").prop('checked', true);

                    });
                }
            });
            $(".close").show();
            $(".EligibleApplicationForm").show();
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function CheckAadhaarNumberAvailability() {

    var txtAadhaarNumber = $("#txtAadhaarNumber").val();
    var AadhaarNumber = txtAadhaarNumber.replace('-', '');
    AadhaarNumber = AadhaarNumber.replace('-', '');
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#AadhaarNumberDuplicate").text(0);

    if (!($.isNumeric(AadhaarNumber))) {
        $("#txtAadhaarNumberDuplicate-Required").show();
        $("#AadhaarNumberDuplicate").text(1);
    }
    else {
        var ApplicationId = $("#ApplicantId").val();

        $.ajax({
            type: "GET",
            url: "/Admission/CheckNameAvailability",
            data: { strName: AadhaarNumber, ApplicationId: ApplicationId, AadhaarRollNumber: 2 },
            success: function (data) {
                if (data == 1) {
                    $("#txtAadhaarNumberDuplicate-Required").show();
                    bootbox.alert("<br><br>Aadhaar Number Already Exist/Invalid*");
                    $("#AadhaarNumberDuplicate").text(1);
                }
            }
        });
    }
}

$('.FileUpload').change(function () {
    var ext = $(this).val().split('.').pop().toLowerCase();
    if (ext != "") {
        if ($.inArray(ext, ['pdf', 'PDF']) == -1) {
            bootbox.alert("<br><br>Kindly upload valid PDF file");
            $(this).val("");
        }
    } else {
        $(this).val("");
    }

    if (this.files[0].size > 1000000) {
        bootbox.alert("<br><br>File size should be less than 1MB");
        $(this).val("");
    }
});

$('#txtApplicantName').focusout(function () {
    $("#ApplicationName").text($("#txtApplicantName").val().toUpperCase());
});
//To calculate percentage based on given marks
$('#txtMaximumMarks').focusout(function () {
    CalculatePercentage(true);
});

$('#txtMarksObtained').focusout(function () {
    CalculatePercentage(true);
});

$('#txtCGPA').focusout(function () {
    CalculatePercentage(true);
});

function CalculatePercentage(IsValidVal) {

    var IsValidP = IsValidVal;
    $("#CalculationMaximumMarks-Required").hide();
    var MaximumMarks = $("#txtMaximumMarks").val();
    var MarksObtained = $("#txtMarksObtained").val();
    var CGPA = $("#txtCGPA").val();
    var Percentage = 0; var PercentageErr = false;
    var FullPercVal = 0;
    if (MaximumMarks != 0 && MarksObtained != 0) {

        if (MaximumMarks >= MarksObtained)
            Percentage = (MarksObtained / MaximumMarks) * 100;
        else
            PercentageErr = true;
    }
    else if (CGPA != 0) {
        Percentage = CGPA * 9.5;
    }
    else {
        PercentageErr = true;
    }
    if (PercentageErr) {
        $("#CalculationMaximumMarks-Required").show();
        IsValidP = false;
    }
    FullPercVal = Percentage.toFixed(2);
    $("#lblPercAsPerMarks").text(FullPercVal + "%");

    return {
        IsValidP,
        FullPercVal
    };
}

function GetCommentDetails(ApplicationId) {

    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Get",
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
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-center' },
                ]
            });
        }
    });
}

function OnClickSubmit() {

    var fileData = new FormData();

    $("#academicyear1-Required").hide();
    var AcademicMonths = $("#AcademicMonths").val();
    var AcademicYear = $("#AcademicYear").val();
    if (AcademicMonths == "" || AcademicYear == "") {
        $("#academicyear1-Required").show();
        IsValid = false;
    }

    $("#ApplicationNumber-Required").hide();
    var ApplicationNumber = $("#ApplicationNumber").val();
    if (ApplicationNumber == "") {
        $("#ApplicationNumber-Required").show();
        IsValid = false;
    }

    $("#StateRegistrationNumber-Required").hide();
    var StateRegistrationNumber = $("#StateRegistrationNumber").val();
    if (StateRegistrationNumber == "") {
        $("#StateRegistrationNumber-Required").show();
        IsValid = false;
    }

    $("#DivisionG-Required").hide();
    var DivisionG = $("#DivisionG :selected").val();
    if (DivisionG == "") {
        $("#DivisionG-Required").show();
        IsValid = false;
    }

    $("#DistrictG-Required").hide();
    var DistrictG = $("#DistrictG :selected").val();
    if (DistrictG == "") {
        $("#DistrictG-Required").show();
        IsValid = false;
    }

    $("#TalukG-Required").hide();
    var TalukG = $("#TalukG :selected").val();
    if (TalukG == "") {
        $("#TalukG-Required").show();
        IsValid = false;
    }

    $("#ITIInstituteG-Required").hide();
    var ITIInstituteG = $("#ITIInstituteG :selected").val();
    if (ITIInstituteG == "") {
        $("#ITIInstituteG-Required").show();
        IsValid = false;
    }

    $("#CourseType-Required").hide();
    var CourseType = $("#CourseType :selected").val();
    if (CourseType == "") {
        $("#CourseType-Required").show();
        IsValid = false;
    }

    $("#CourseType-Required").hide();
    var CourseType = $("#CourseType :selected").val();
    if (CourseType == "") {
        $("#CourseType-Required").show();
        IsValid = false;
    }

    $("#TraineeType-Required").hide();
    var TraineeType = $("#TraineeType :selected").val();
    if (TraineeType == "") {
        $("#TraineeType-Required").show();
        IsValid = false;
    }

    $("#TradeName-Required").hide();
    var TradeName = $("#TradeName :selected").val();
    if (TradeName == "") {
        $("#TradeName-Required").show();
        IsValid = false;
    }

    $("#Shift-Required").hide();
    var Shift = $("#Shift :selected").val();
    if (Shift == "") {
        $("#Shift-Required").show();
        IsValid = false;
    }

    $("#Units-Required").hide();
    var Units = $("#Units :selected").val();
    if (Units == "") {
        $("#Units-Required").show();
        IsValid = false;
    }

    $("#AdmisionTime-Required").hide();
    var AdmisionTime = $("#AdmisionTime").val();
    if (AdmisionTime == "") {
        $("#AdmisionTime-Required").show();
        IsValid = false;
    }

    $("#ReceiptNumber-Required").hide();
    var ReceiptNumber = $("#ReceiptNumber").val();
    if (ReceiptNumber == "") {
        $("#ReceiptNumber-Required").show();
        IsValid = false;
    }

    $("#PaymentDateG-Required").hide();
    var PaymentDateG = $("#PaymentDateG").val();
    if (PaymentDateG == "") {
        $("#PaymentDateG-Required").show();
        IsValid = false;
    }

}

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

//End region .. Tab 1

function SaveApplicantDetails() {
    
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    $("#Gender-Required").hide();
    var SameAsCommunicationAddress = 0;
    if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
        SameAsCommunicationAddress = 1;
    }

    $("#txtApplicantName-Required").hide();
    var ApplicantName = $("#txtApplicantName").val();
    ApplicantName = ApplicantName.toUpperCase();
    if (ApplicantName == "") {
        $("#txtApplicantName-Required").show();
        IsValid = false;
    }

    $("#Gender-Required").hide();
    var Gender = $("#Gender :selected").val();
    if (Gender == 0) {
        $("#Gender-Required").show();
        IsValid = false;
    }

    $("#txtFathersName-Required").hide();
    var FathersName = $("#txtFathersName").val();
    if (FathersName == "") {
        $("#txtFathersName-Required").show();
        IsValid = false;
    }

    $("#PhotoUpload-Required").hide();
    var e = $('#FileAttach');
    if (e.is(':disabled')) {
        var PhotoUpload = $("#PhotoUpload").val();
        if (PhotoUpload == "") {
            $("#PhotoUpload-Required").show();
            IsValid = false;
        }
    }
    else {
        var PhotoUpload = $("#PhotoUpload").val();
        if (PhotoUpload == "") {
            var IsUploaded = $('#IsUploaded').val();
            if (IsUploaded != "") {
                fileData.append(
                    "Photo", IsUploaded
                );
            }
            else {
                $("#PhotoUpload-Required").show();
                IsValid = false;
            }
        }
        else {
            var fileUpload = $('#PhotoUpload').get(0);
            var files = fileUpload.files;

            for (var i = 0; i < files.length; i++) {
                fileData.append("PhotoFile", files[i]);
            }
        }
    }

    $("#txtParentOccupation-Required").hide();
    var ParentOccupation = $("#txtParentOccupation").val();
    if (ParentOccupation == "") {
        $("#txtParentOccupation-Required").show();
        IsValid = false;
    }

    var dateofbirth = "";
    $("#dateofbirth-Required").hide();
    var dateofbirth = $("#dateofbirth").val();
  //  var dateofbirthage = $("#dobError").val();
   // if (dateofbirth == "" || dateofbirthage == 1) {
    if (dateofbirth == "" ) {
        $("#dateofbirth-Required").show();
        IsValid = false;
    }
    else {
        var dts = $("#dateofbirth").val().split("-");
        conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
        dateofbirth = (dts[0] + "-" + dts[1] + "-" + dts[2]);
    }

    $("#txtMothersName-Required").hide();
    var MothersName = $("#txtMothersName").val();
    if (MothersName == "") {
        $("#txtMothersName-Required").show();
        IsValid = false;
    }

    $("#Religion-Required").hide();
    var Religion = $("#Religion :selected").val();
    if (Religion == 0) {
        $("#Religion-Required").show();
        IsValid = false;
    }

    $("#Category-Required").hide();
    var Category = $("#Category :selected").val();
    if (Category == 0) {
        $("#Category-Required").show();
        IsValid = false;
    }

    $("#MinorityCategory-Required").hide();
    var MinorityCategory = $("#MinorityCategory :selected").val();
    if (MinorityCategory == 0) {
        $("#MinorityCategory-Required").show();
        IsValid = false;
    }

    $("#txtCaste-Required").hide();
    var Caste = $("#txtCaste").val();
    //if (Caste == "" || Caste == 0) {
    //    $("#txtCaste-Required").show();
    //    IsValid = false;
    //}

    $("#txtFamilyAnnualIncome-Required").hide();
    var FamilyAnnualIncome = $("#txtFamilyAnnualIncome").val();
    //if (FamilyAnnualIncome == "") {
    //    $("#txtFamilyAnnualIncome-Required").show();
    //    IsValid = false;
    //}

    $("#ApplicantType-Required").hide();
    var ApplicantType = $("#ApplicantTypeSelect :selected").val();
    if (ApplicantType == 0) {
        $("#ApplicantType-Required").show();
        IsValid = false;
    }

    var txtAadhaarNumber = $("#txtAadhaarNumber").val();
    var AadhaarNumber = txtAadhaarNumber.replace('-', '');
    AadhaarNumber = AadhaarNumber.replace('-', '');
    var AadhaarNumberDuplicate = $("#AadhaarNumberDuplicate").text();
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#txtAadhaarNumber-Required").hide();
    if (AadhaarNumber == "" || AadhaarNumber.length != 12) {
        $("#txtAadhaarNumber-Required").show();
        IsValid = false;
    }
    else if (AadhaarNumberDuplicate == 1) {
        $("#txtAadhaarNumberDuplicate-Required").show();
        IsValid = false;
    }

    $("#txtRationCard-Required").hide();
    var RationCard = $("#txtRationCard").val();
    //if (RationCard == "" || ($("#txtRationCard").val().length != 12)) {
    //    $("#txtRationCard-Required").show();
    //    IsValid = false;
    //}

    $("#txtMothersName-Required").hide();
    var MothersName = $("#txtMothersName").val();
    if (MothersName == "") {
        $("#txtMothersName-Required").show();
        IsValid = false;
    }

    $("#AccountNumber-Required").hide();
    var AccountNumber = $("#AccountNumber").val();
    if (AccountNumber.length < 11) {
        $("#AccountNumber-Required").show();
        IsValid = false;
    }

    $("#BankName-Required").hide();
    var BankName = $.trim($("#BankName").val());
    if (BankName == "") {
        $("#BankName-Required").show();
        IsValid = false;
    }

    $("#IFSCCode-Required").hide();
    var IFSCCode = $.trim($("#IFSCCode").val());
    if (IFSCCode.length < 10) {
        $("#IFSCCode-Required").show();
        IsValid = false;
    }

    $("#PersonWithDisability-Required").hide();
    var PhysicallyHanidcapType = 0;
    var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val();
    if (PhysicallyHanidcapInd == 1) {
        PhysicallyHanidcapInd = true;
        PhysicallyHanidcapType = $("#PhysicallyHanidcapType :selected").val();
        if (PhysicallyHanidcapType == 0 || PhysicallyHanidcapType == "") {
            $("#PersonWithDisability-Required").show();
            IsValid = false;
        }
    }
    else {
        PhysicallyHanidcapInd = false;
    }

    var HoraNadu_GadiNadu_Kannidagas = $('input[name=HoraNadu]:checked').val();
    if (HoraNadu_GadiNadu_Kannidagas == 1)
        HoraNadu_GadiNadu_Kannidagas = true;
    else
        HoraNadu_GadiNadu_Kannidagas = false;

    var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
    if (ExemptedFromStudyCertificate == 1)
        ExemptedFromStudyCertificate = true;
    else
        ExemptedFromStudyCertificate = false;

    var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
    if (HyderabadKarnatakaRegion == 1)
        HyderabadKarnatakaRegion = true;
    else
        HyderabadKarnatakaRegion = false;

    var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
    if (KanndaMedium == 1)
        KanndaMedium = true;
    else
        KanndaMedium = false;

    var ExServiceMan = $('input[name=ExServiceMan]:checked').val();
    if (ExServiceMan == 1)
        ExServiceMan = true;
    else
        ExServiceMan = false;

    var EconomyWeakerSection = $('input[name=EconomyWeakerSection]:checked').val();
    if (EconomyWeakerSection == 1)
        EconomyWeakerSection = true;
    else
        EconomyWeakerSection = false;

    if (IsValid) {

        fileData.append(
            "ApplyMonth", AcademicMonths
        );
        fileData.append(
            "ApplyYear", AcademicYear
        );
        fileData.append(
            "ApplicantName", ApplicantName
        );
        fileData.append(
            "FathersName", FathersName
        );
        fileData.append(
            "ParentsOccupation", ParentOccupation
        );
        fileData.append(
            "DOB", dateofbirth
        );
        fileData.append(
            "MothersName", MothersName
        );
        fileData.append(
            "Religion", Religion
        );
        fileData.append(
            "Gender", Gender
        );
        fileData.append(
            "Category", Category
        );
        fileData.append(
            "MinorityCategory", MinorityCategory
        );
        fileData.append(
            "Caste", Caste
        );
        fileData.append(
          "FamilyAnnIncome", SHAEncryption(FamilyAnnualIncome)
        );
        fileData.append(
            "ApplicantType", ApplicantType
        );
        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
          "RationCard", SHAEncryption(RationCard)
        );
        fileData.append(
          "AadhaarNumber", SHAEncryption(AadhaarNumber)
        );
        fileData.append(
          "AccountNumber", SHAEncryption(AccountNumber)
        );
        fileData.append(
          "BankName", SHAEncryption(BankName)
        );
        fileData.append(
          "IFSCCode", SHAEncryption(IFSCCode)
        );
        fileData.append(
            "PhysicallyHanidcapInd", PhysicallyHanidcapInd
        );

        fileData.append(
            "PhysicallyHanidcapType", PhysicallyHanidcapType
        );
        fileData.append(
            "ApplicationId", ApplicantId
        );

        fileData.append(
            "HoraNadu_GadiNadu_Kannidagas", HoraNadu_GadiNadu_Kannidagas
        );

        fileData.append(
            "ExemptedFromStudyCertificate", ExemptedFromStudyCertificate
        );

        fileData.append(
            "HyderabadKarnatakaRegion", HyderabadKarnatakaRegion
        );

        fileData.append(
            "KanndaMedium", KanndaMedium
        );

        fileData.append(
            "EconomyWeakerSection", EconomyWeakerSection
        );

        fileData.append(
            "ExServiceMan", ExServiceMan
        );
        fileData.append(
            "AgainstVacancyInd", 1
        );

        $.ajax({
            type: "POST",
            url: "/Admission/InsertApplicantFormDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    $("#ApplCredatedBy").val(data.objReturnApplicationForm[0].CredatedBy);
                    $("#ApplicantId").val(data.objReturnApplicationForm[0].ApplicationId);
                    var alertmsg = "";
                    if (data.pref.status == "Error occured!")
                        alertmsg = "<br><br>Error in data !";
                    else {
                        alertmsg = "<br><br>Applicant Data Updated Successfully !";
                        //bootbox.alert({
                        //    message: alertmsg,
                        //    callback: function () { location.reload(true); }
                        //});
                        bootbox.alert("<br><br>" + alertmsg);
                    }
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function SaveEducationDetails() {

    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    //Education
    $("#RuralcertificateYes-Required").hide();
    $("#Qualification-Required").hide();
    var Qualification = $("#Qualification :selected").val();
    if (Qualification == 0) {
        $("#Qualification-Required").show();
        IsValid = false;
    }

    var TenthCOBSEBoard = $('input[name=TenthCOBSEBoard]:checked').val();
    var ApplicantBelongTo = $('input[name=RuralUrban]:checked').val();
    var AppliedBasic = $('input[name=AppBasics]:checked').val();
    var TenthBoard = $('input[name=TenthBoard]:checked').val();

    if (TenthCOBSEBoard != 7) {
        $("#OtherBoards").val() == 0;
    }

    $("#txtInstituteStudied-Required").hide();
    var InstituteStudiedQual = $("#txtInstituteStudied").val();
    if (InstituteStudiedQual == 0) {
        $("#txtInstituteStudied-Required").show();
        IsValid = false;
    }

    $("#OtherBoards-Required").hide();
    var OtherBoardsVal = $("#OtherBoards").val();
    if (OtherBoardsVal == 0 && $("#OtherBoards").is(":visible") == true) {
        $("#OtherBoards-Required").show();
        IsValid = false;
    }

    $("#txtMaximumMarks-Required").hide();
    var MaxMarks = $("#txtMaximumMarks").val();
    if (MaxMarks == 0 && $("#txtMaximumMarks").is(":disabled") == false) {
        $("#txtMaximumMarks-Required").show();
        IsValid = false;
    }

    $("#txtMarksObtained-Required").hide();
    var MarksObtained = $("#txtMarksObtained").val();
    if (MarksObtained == 0 && $("#txtMarksObtained").is(":disabled") == false) {
        $("#txtMarksObtained-Required").show();
        IsValid = false;
    }

    $("#txtCGPA-Required").hide();
    var CGPAGrade = $("#txtCGPA").val();
    if (CGPAGrade == 0 && $("#txtCGPA").is(":visible") == true) {
        $("#txtCGPA-Required").show();
        IsValid = false;
    }

    let PercentageReturnValue = CalculatePercentage(IsValid);

    IsValidP = PercentageReturnValue.IsValidP,
        Percentage = PercentageReturnValue.FullPercVal;

    if (IsValidP == false) {
        IsValid = false
    }

    $("#Results-Required").hide();
    var ResultQual = $("#Results :selected").val();
    if (ResultQual == 0) {
        $("#Results-Required").show();
        IsValid = false;
    }

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    var txtDocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

    var studiedMathsScience = $('input[name=studiedMathsScience]:checked').val();
    if (studiedMathsScience == 1)
        studiedMathsScience = true;
    else
        studiedMathsScience = false;

    if (IsValid) {

        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
            "ApplicationId", ApplicantId
        );

        fileData.append(
            "TenthCOBSEBoard", TenthCOBSEBoard
        );

        //Education
        fileData.append(
            "Qualification", Qualification
        );
        fileData.append(
            "ApplicantBelongTo", ApplicantBelongTo
        );
        fileData.append(
            "AppliedBasic", AppliedBasic
        );
        fileData.append(
            "TenthBoard", TenthBoard
        );
        fileData.append(
            "BoardId", OtherBoardsVal
        );
        fileData.append(
            "Percentage", Percentage
        );
        fileData.append(
            "InstituteStudiedQual", InstituteStudiedQual
        );
        fileData.append(
            "MaxMarks", MaxMarks
        );
        fileData.append(
            "MarksObtained", MarksObtained
        );

        fileData.append(
            "ResultQual", ResultQual
        );
                fileData.append(
            "EducationCGPA", CGPAGrade
        );
        fileData.append(
            "ApplicantNumber", ApplicantNumber
        );

        fileData.append(
            "PaymentOptionval", PaymentOptionval
        );

        fileData.append(
            "DocumentFeeReceiptDetails", txtDocumentFeeReceiptDetails
        );

        fileData.append(
            "studiedMathsScience", studiedMathsScience
        );
        fileData.append(
            "AgainstVacancyInd", 1
        );

        $.ajax({
            type: "POST",
            url: "/Admission/SaveEducationDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    $.each(data.objReturnApplicationForm, function () {
                        //GetMasterData();
                    });

                    if (data.pref.status == "Error occured!")
                        bootbox.alert("<br><br>Error in data !");
                    else
                        bootbox.alert("<br><br>Applicant Education Data Updated Successfully !");
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function SaveAddressDetails() {

    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    //Address
    $("#txtCommunicationAddress-Required").hide();
    var CommunicationAddress = $("#txtCommunicationAddress").val();
    if (CommunicationAddress == "") {
        $("#txtCommunicationAddress-Required").show();
        IsValid = false;
    }

    $("#Districts-Required").hide();
    var Districts = $("#Districts :selected").val();
    if (Districts == 0) {
        $("#Districts-Required").show();
        IsValid = false;
    }

    $("#Talukas-Required").hide();
    var Talukas = $("#Talukas :selected").val();
    if (Talukas == 0) {
        $("#Talukas-Required").show();
        IsValid = false;
    }

    $("#txtPincode-Required").hide();
    var Pincode = $("#txtPincode").val();
    if (Pincode == "") {
        $("#txtPincode-Required").show();
        IsValid = false;
    }

    $("#txtPermanentAddress-Required").hide();
    var PermanentAddress = $("#txtPermanentAddress").val();
    if (PermanentAddress == "") {
        $("#txtPermanentAddress-Required").show();
        IsValid = false;
    }

    $("#PermanentDistricts-Required").hide();
    var PermanentDistricts = $("#PermanentDistricts :selected").val();
    if (PermanentDistricts == "" || PermanentDistricts == 0) {
        $("#PermanentDistricts-Required").show();
        IsValid = false;
    }

    $("#PermanentTalukas-Required").hide();
    var PermanentTalukas = $("#PermanentTalukas :selected").val();
    if (PermanentTalukas == 0) {
        $("#PermanentTalukas-Required").show();
        IsValid = false;
    }

    $("#txtPermanentPincode-Required").hide();
    var PermanentPincode = $("#txtPermanentPincode").val();
    if (PermanentPincode == "") {
        $("#txtPermanentPincode-Required").show();
        IsValid = false;
    }

    $("#txtApplicantPhoneNumber-Required").hide();
    var ApplicantPhoneNumber = $("#txtApplicantPhoneNumber").val();
    if (ApplicantPhoneNumber == "" || ApplicantPhoneNumber.length != 10) {
        $("#txtApplicantPhoneNumber-Required").show();
        IsValid = false;
    }

    $("#txtFathersPhoneNumber-Required").hide();
    var FathersPhoneNumber = $("#txtFathersPhoneNumber").val();
    if (FathersPhoneNumber == "" || FathersPhoneNumber.length != 10) {
        $("#txtFathersPhoneNumber-Required").show();
        IsValid = false;
    }

    $("#txtEmailId-Required").hide();
    var EmailId = $("#txtEmailId").val();
    if (EmailId == "") {
        $("#txtEmailId-Required").show();
        IsValid = false;
    }
    var IsValidE = ValidateEmail(EmailId);
    if (IsValidE == false)
        IsValid = false;

    if (IsValid) {

        fileData.append(
            "ApplicationId", ApplicantId
        );

        //Address
        fileData.append(
            "CommunicationAddress", CommunicationAddress
        );
        fileData.append(
            "DistrictId", Districts
        );
        fileData.append(
            "TalukaId", Talukas
        );
        fileData.append(
            "Pincode", Pincode
        );
        fileData.append(
            "PermanentAddress", PermanentAddress
        );

        var SameAsCommunicationAddress = 0;
        if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
            SameAsCommunicationAddress = 1;
        }

        var SameAdd = false;
        if (SameAsCommunicationAddress == 1)
            SameAdd = true;

        fileData.append(
            "SameAdd", SameAdd
        );

        fileData.append(
            "PermanentDistricts", PermanentDistricts
        );
        fileData.append(
            "PTaluk", PermanentTalukas
        );
        fileData.append(
            "PPinCode", PermanentPincode
        );

        fileData.append(
            "ApplicantPhoneNumber", ApplicantPhoneNumber
        );
        fileData.append(
            "FathersPhoneNumber", FathersPhoneNumber
        );
        fileData.append(
            "EmailId", EmailId
        );
        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
            "AgainstVacancyInd", 1
        );

        $.ajax({
            type: "POST",
            url: "/Admission/SaveAddressDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    $.each(data.objReturnApplicationForm, function () {
                        $("#ApplicantId").val(data.objReturnApplicationForm[0].ApplicationId);
                        //GetMasterData();
                    });

                    if (data.pref.status == "Error occured!")
                        bootbox.alert("<br><br>Error in data !");
                    else
                        bootbox.alert("<br><br>Applicant Communication Data Updated Successfully !");
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function SaveDocumentDetails() {

    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null) {
        bootbox.alert("<br><br>Kindly Give the Applicant,Education, Address and Save the Details");
    }
    else {

        var fileData = new FormData();

        var IsValid = true;
        //EduCertificate
        var fileUpload = $('#EduCertificate').get(0);
        if ($('.aEduCertificate').is(':visible')) {
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#EduCertificate-Required").hide();
            if ($("#EduCertificate").val() == "") {
                $("#EduCertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
        }
        
        //CasteCertificate
        fileUpload = $('#CasteCertificate').get(0);
        if ($('.aCasteCertificate').is(':visible')) {
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#CasteCertificate-Required").hide();
            if ($("#CasteCertificate").val() != "") {
            //    $("#CasteCertificate-Required").show();
            //    IsValid = false;
            //}
            //else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
        }

        //Rationcard
        fileUpload = $('#Rationcard').get(0);
        if ($('.aRationCard').is(':visible')) {
            if ($("#Rationcard").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
            }
        }
        else {
            $("#Rationcard-Required").hide();
            if ($("#Rationcard").val() != "") {
                //$("#Rationcard-Required").show();
                //IsValid = false;
            //}
            //else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RationcardPDF", files[i]);
                }
            }
        }

        //Incomecertificate 
        fileUpload = $('#Incomecertificate').get(0);
        if ($('.aIncomeCertificate').is(':visible')) {
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
        }
        else {
            $("#Incomecertificate-Required").hide();
            if ($("#Incomecertificate").val() != "") {
                //$("#Incomecertificate-Required").show();
                //IsValid = false;
            //}
            //else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
        }

        //UID Number
        fileUpload = $('#UIDNumber').get(0);
        $("#UIDNumber-Required").hide();
        if ($('.aUIDNumber').is(':visible')) {
            if ($("#UIDNumber").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
        }
        else {
            $("#UIDNumber-Required").hide();
            if ($("#UIDNumber").val() == "") {
                $("#UIDNumber-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
        }

        var RuralUrban = $('input[name=RuralUrban]:checked').val();
        //Rural Certificate
        fileUpload = $('#Ruralcertificate').get(0);
        $("#Ruralcertificate-Required").hide();
        if ($('.aRuralcertificate').is(':visible')) {
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
        }
        else {
            $("#Ruralcertificate-Required").hide();
            if ($("#Ruralcertificate").val() == "" && RuralUrban == 1) {
                $("#Ruralcertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
        }

        var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
        if (KanndaMedium == 1)
            $("#KanMedCer").val(1);
        else
            $("#KanMedCer").val(0);

        //KannadamediumCertificate
        fileUpload = $('#KannadamediumCertificate').get(0);
        $("#KannadamediumCertificate-Required").hide();
        if ($('.aKannadamediumCertificate').is(':visible')) {
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
        }
        else {
            $("#KannadamediumCertificate-Required").hide();
            if (($("#KannadamediumCertificate").val() == "") && ($("#KanMedCer").val() == 1)) {
                $("#KannadamediumCertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
        }

        var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val();
        if (PhysicallyHanidcapInd == 1)
            $("#DiffAbledCer").val(1);
        else
            $("#DiffAbledCer").val(0);

        //DifferentlyAbledPDF
        fileUpload = $('#Differentlyabledcertificate').get(0);
        $("#Differentlyabledcertificate-Required").hide();
        if ($('.aDifferentlyabledcertificate').is(':visible')) {
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
            }
        }
        else {
            $("#Differentlyabledcertificate-Required").hide();
            if (($("#Differentlyabledcertificate").val() == "") && ($("#DiffAbledCer").val() == 1)) {
                $("#Differentlyabledcertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
            }
        }

        var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
        if (ExemptedFromStudyCertificate == 1)
            $("#ExeStuCer").val(1);
        else
            $("#ExeStuCer").val(0);
        //ExemptedCertificate
        fileUpload = $('#ExemptedCertificate').get(0);
        $("#ExemptedCertificate-Required").hide();
        if ($('.aExemptedCertificate').is(':visible')) {
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
        }
        else {
            $("#ExemptedCertificate-Required").hide();
            if (($("#ExemptedCertificate").val() == "") && ($("#ExeStuCer").val() == 1)) {
                $("#ExemptedCertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
        }

        var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
        if (HyderabadKarnatakaRegion == 1)
            $("#HydKarCer").val(1);
        else
            $("#HydKarCer").val(0);
        //HyderabadKarnatakaRegion
        fileUpload = $('#HyderabadKarnatakaRegion').get(0);
        $("#HyderabadKarnatakaRegion-Required").hide();
        if ($('.aHyderabadKarnatakaRegion').is(':visible')) {
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
        }
        else {
            $("#HyderabadKarnatakaRegion-Required").hide();
            if (($("#HyderabadKarnatakaRegion").val() == "") && ($("#HydKarCer").val() == 1)) {
                $("#HyderabadKarnatakaRegion-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
        }

        var HoraNadu = $('input[name=HoraNadu]:checked').val();
        if (HoraNadu == 1)
            $("#HoraNaduCer").val(1);
        else
            $("#HoraNaduCer").val(0);
        //HoranaaduKannadiga
        fileUpload = $('#HoranaaduKannadiga').get(0);
        $("#HoranaaduKannadiga-Required").hide();
        if ($('.aHoranaaduKannadiga').is(':visible')) {
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
        }
        else {
            $("#HoranaaduKannadiga-Required").hide();
            if (($("#HoranaaduKannadiga").val() == "") && ($("#HoraNaduCer").val() == 1)) {
                $("#HoranaaduKannadiga-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
        }

        //OtherCertificates
        fileUpload = $('#OtherCertificates').get(0);
        $("#OtherCertificates-Required").hide();
        if ($('.aOtherCertificates').is(':visible')) {
            if ($("#OtherCertificates").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
            }
        }
        else {
            $("#OtherCertificates-Required").hide();
            if ($("#OtherCertificates").val() == "") {
                //$("#OtherCertificates-Required").show();
                //IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("OtherCertificatesPDF", files[i]);
                }
            }
        }

        //Exserviceman
        fileUpload = $('#Exserviceman').get(0);
        $("#Exserviceman-Required").hide();
        if ($('.aExserviceman').is(':visible')) {
            if ($("#Exserviceman").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
        }
        else {
            $("#Exserviceman-Required").hide();
            if (($("#Exserviceman").val() == "") && ($("#ExServiceCer").val() == 1)) {
                $("#Exserviceman-Required").show();
                IsValid = false;
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
        if ($('.aEWSCertificate').is(':visible')) {
            if ($("#EWSCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#EWSCertificate-Required").hide();
            if (($("#EWSCertificate").val() == "") && ($("#EcoWeakCer").val() == 1)) {
                $("#EWSCertificate-Required").show();
                IsValid = false;
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
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
        var EWSCreatedByVal = $("#EWSCreatedBy").val();

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
            "EWSDocAppId", EWSDocAppIdVal
        );
        fileData.append(
            "UploadedByVerfication", 0
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
            "EWSCreatedBy", EWSCreatedByVal
        );

        fileData.append(
            "DocumentLength", fileData.append.length
        );

        fileData.append(
            "ApplicantId", ApplicantId
        );

        if (IsValid) {
            $.ajax({
                type: "POST",
                url: "/Admission/ApplicantDocumentDetails",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (data) {
                    if (data) {

                        //GetMasterData();
                        alertmsg = "<br><br>Application required documents Added to the List";
                        bootbox.alert({
                            message: alertmsg,
                            //callback: function () { location.reload(true); }
                        });
                    } else {
                        alert("Cannot add to list !");
                    }
                }
            });
        }
        else {
            //bootbox.alert("<br><br>Error in your data");
        }
    }
}

function ClearErrorFields() {

    //Applicant Details
    $("#txtRollNumber-Required").hide();
    $("#txtRollNumberDuplicate-Required").hide();
    $("#academicyear1-Required").hide();
    $("#txtApplicantName-Required").hide();
    $("#Gender-Required").hide();
    $("#txtFathersName-Required").hide();
    $("#PhotoUpload-Required").hide();
    $("#txtParentOccupation-Required").hide();
    $("#dateofbirth-Required").hide();
    $("#txtMothersName-Required").hide();
    $("#Religion-Required").hide();
    $("#Category-Required").hide();
    $("#MinorityCategory-Required").hide();
    $("#txtCaste-Required").hide();
    $("#txtFamilyAnnualIncome-Required").hide();
    $("#ApplicantType-Required").hide();
    $("#ApplicableReservations-Required").hide();
    $("#txtAadhaarNumber-Required").hide();
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#txtRationCard-Required").hide();
    $("#txtMothersName-Required").hide();

    //Education
    $("#Qualification-Required").hide();
    $("#txtInstituteStudied-Required").hide();
    $("#txtMaximumMarks-Required").hide();
    $("#txtMarksObtained-Required").hide();
    //$("#txtMinMarks-Required").hide();
    $("#Results-Required").hide();
    $("#CalculationMaximumMarks-Required").hide();

    //Address
    $("#txtCommunicationAddress-Required").hide();
    $("#Districts-Required").hide();
    $("#Talukas-Required").hide();
    $("#txtPincode-Required").hide();
    $("#txtPermanentAddress-Required").hide();
    $("#PermanentDistricts-Required").hide();
    $("#PermanentTalukas-Required").hide();
    $("#txtPermanentPincode-Required").hide();
    $("#txtApplicantPhoneNumber-Required").hide();
    $("#txtFathersPhoneNumber-Required").hide();
    $("#txtEmailId-Required").hide();

    //Institute
    $("#InstituteDistricts-Required").hide();
    $("#InstituteList-Required").hide();

    //Documents
    $("#EduCertificate-Required").hide();
    $("#CasteCertificate-Required").hide();
    $("#Rationcard-Required").hide();
    $("#Incomecertificate-Required").hide();
    $("#UIDNumber-Required").hide();
    $("#Ruralcertificate-Required").hide();
    $("#KannadamediumCertificate-Required").hide();
    $("#Differentlyabledcertificate-Required").hide();
    $("#ExemptedCertificate-Required").hide();
    $("#HyderabadKarnatakaRegion-Required").hide();
    $("#HoranaaduKannadiga-Required").hide();
    $("#OtherCertificates-Required").hide();
    $("#KashmirMigrants-Required").hide();
    $("#Exserviceman-Required").hide();
    $("#EWSCertificate-Required").hide();
}

//To show preview the image
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#previewimage').attr('src', e.target.result).width(50).height(50);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function fnEnableDisableEWS() {
    $("#EconomyWeakerSection").prop('disabled', true);
    if ($("#Category :selected").val() == CategoryConst.General) {
        $("#EconomyWeakerSection").prop('disabled', false);
    }
}

function fnClearAllocatedFeeDetails(){
    $("#AdmisionTime").val('');
    $("#PaymentDate").val('');
    $("#txtApplRemarks").val('');
    $("input[name=AdmittedStatus][value=3]").prop('checked', true);
    $("#ReceiptNumber").text('');
    $("#AdmissionRegistrationNumber").text('');
    $("#StateRegistrationNumber").text('');
    $("#RollNumber").text('');

    $("#SaveSeatAllocationFeePay").attr("disabled", true);
    $(".PaymentGenerationGridCls").hide();
    $("#AllocationFeePay").show();
    $(".AdmFeePaidStatus").prop('disabled', false);
    $("#PaymentDate").prop('disabled', false);
}
