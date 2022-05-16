$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');

    $("#GridDivId").hide();
    $("#UpdateGridDivId").hide();
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
    $("#ReviewPanel").hide();
    $("#UpdateEditPanel").hide();

    $('#ExamYear').mouseup(function () {
        $('#ExamYear-Required').text('');
    });

    $('#existExamYear').mouseup(function () {
       // $('#ExamYear-Required').text('');
    });

    GetExamType();
    GetSeatTypes("seatTypeId");
    GetCourseTypes();
    GetRoleList();
    GetSyllabusType();
    ClearAllFields();
    GetAllocationSeatofRuletoUpdate();
    GetApprovedAllocationSeatofRuletoView();
});
$('#ExamYear').datepicker({
    changeMonth: true,
    changeYear: true,
    yearRange: "-1:+1",
    showButtonPanel: true,
    dateFormat: 'MM yy',
    selectedMonth: 8,
    defaultDate: '6m',
    onClose: function (dateText, inst) {
        $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
        $('#AcademicYear').val(inst.selectedYear);
        $('#AcademicMonths').val(inst.selectedMonth);
       // GetSeatAvailability();
    },
    beforeShow: function (dateText) {
        console.log("Selected date: " + dateText + "; input's current value: " + this.value);
    }
}).focus(function () {
    $(".ui-datepicker-calendar").hide();
    $(".ui-datepicker-current").hide();
    var year = new Date().getFullYear();
    $("#ui-datepicker-div").position({
        my: "left top",
        at: "left bottom",
        of: $(this)
    });
}).attr("readonly", false).datepicker('setDate', new Date(new Date().getFullYear(), 7));
$("#ExamYear").focus(function () {
    $(".ui-datepicker-calendar").hide();
    $("#ui-datepicker-div").position({
        my: "center top",
        at: "center bottom",
        of: $(this)
    });
});

$('a[href="#tab_1"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

$('a[href="#tab_2"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

$('a[href="#tab_3"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

function CancelDetailsApprovedView() {
    $("#ApprovedViewPanel").hide();
}

function CancelSeatAllocation() {
    $("#EditPanel").hide();
}

function GetRoleList() {

    $("#ForwardTo").empty();
    $("#ForwardTo").append('<option value="0">choose</option>');

    $("#UpdateForwardTo").empty();
    $("#UpdateForwardTo").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetRoleList",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (this.RoleID == 1 || this.RoleID == 2 || this.RoleID == 5 || this.RoleID == 6 || this.RoleID==4) {
                        $("#ForwardTo").append($("<option/>").val(this.RoleID).text(this.RoleName));
                        $("#UpdateForwardTo").append($("<option/>").val(this.RoleID).text(this.RoleName));
                    }
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function ClearAllFields() {


    $("#GMPerc").val("");
    $("#UpdateGMPerc").val("");
    $("#ViewGMPerc").val("");

    $("#SCPerc").val("");
    $("#UpdateSCPerc").val("");
    $("#ViewSCPerc").val("");

    $("#STPerc").val("");
    $("#UpdateSTPerc").val("");
    $("#ViewSTPerc").val("");

    $("#C1Perc").val("");
    $("#UpdateC1Perc").val("");
    $("#ViewC1Perc").val("");

    $("#IIAPerc").val("");
    $("#UpdateIIAPerc").val("");
    $("#ViewIIAPerc").val("");

    $("#IIBPerc").val("");
    $("#UpdateIIBPerc").val("");
    $("#ViewIIBPerc").val("");

    $("#IIIAPerc").val("");
    $("#UpdateIIIAPerc").val("");
    $("#ViewIIIAPerc").val("");

    $("#IIIBPerc").val("");
    $("#UpdateIIIBPerc").val("");
    $("#ViewIIIBPerc").val("");

    $("#WomenPerc").val("");
    $("#UpdateWomenPerc").val("");
    $("#ViewWomenPerc").val("");

    $("#PerDisabilitiesperc").val("");
    $("#UpdatePerDisabilitiesperc").val("");
    $("#ViewPerDisabilitiesperc").val("");

    $("#ExServicePerc").val("");
    $("#UpdateExServicePerc").val("");
    $("#ViewExServicePerc").val("");

    $("#KanMediumPerc").val("");
    $("#UpdateKanMediumPerc").val("");
    $("#ViewKanMediumPerc").val("");

    $("#LandLoserPerc").val("");
    $("#UpdateLandLoserPerc").val("");
    $("#ViewLandLoserPerc").val("");

    $("#KashmiriMigrPerc").val("");
    $("#UpdateKashmiriMigrPerc").val("");
    $("#ViewKashmiriMigrPerc").val("");

    $("#EconomicWeakerPerc").val("");
    $("#UpdateEconomicWeakerPerc").val("");
    $("#ViewEconomicWeakerPerc").val("");

    $("#GPPerc").val("");
    $("#UpdateGPPerc").val("");
    $("#ViewGPPerc").val("");

    $("#HyderabadRegion").val("");
    $("#UpdateHyderabadRegion").val("");
    $("#ViewHyderabadRegion").val("");

    $("#NonHyderabadRegion").val("");
    $("#UpdateNonHyderabadRegion").val("");
    $("#ViewNonHyderabadRegion").val("");

    $("#NHyderabadRegion").val("");
    $("#UpdateNHyderabadRegion").val("");
    $("#ViewNHyderabadRegion").val("");

    $("#NNonHyderabadRegion").val("");
    $("#UpdateNNonHyderabadRegion").val("");
    $("#ViewNNonHyderabadRegion").val("");

    $("#GradespercA1").val("");
    $("#ViewGradespercA1").val("");
    $("#UpdateGradespercA1").val("");

    $("#GradespercA2").val("");
    $("#ViewGradespercA2").val("");
    $("#UpdateGradespercA2").val("");

    $("#GradespercB1").val("");
    $("#ViewGradespercB1").val("");
    $("#UpdateGradespercB1").val("");

    $("#GradespercB2").val("");
    $("#ViewGradespercB2").val("");
    $("#UpdateGradespercB2").val("");

    $("#GradespercC1").val("");
    $("#ViewGradespercC1").val("");
    $("#UpdateGradespercC1").val("");

    $("#GradespercC2").val("");
    $("#ViewGradespercC2").val("");
    $("#UpdateGradespercC2").val("");

    $("#GradespercD").val("");
    $("#ViewGradespercD").val("");
    $("#UpdateGradespercD").val("");

    $("#GradespercE1").val("");
    $("#ViewGradespercE1").val("");
    $("#UpdateGradespercE1").val("");

    $("#GradespercE2").val("");
    $("#ViewGradespercE2").val("");
    $("#UpdateGradespercE2").val("");

    $("#Syllabus").val("0");
    $("#UpdateSyllabus").val("0");
    $("#ViewSyllabus").val("0");

    $("#ReservationRural").val("");
    $("#ViewReservationRural").val("");
    $("#UpdateReservationRural").val("");

    $("#ReservationTransgender").val("");
    $("#ViewReservationTransgender").val("");
    $("#UpdateReservationTransgender").val("");
    $("#TotalPer").val("");
    $("#hrPercent").val("");
    $("#UpdateTotalPer").val("");
    $("#UpdatehrPercent").val("");
    $("#ViewTotalPer").val("");
    $("#ViewhrPercent").val("");
    $("#hrPercent1-Required").hide();
    $("#TotalPer-Required").hide();
}

function ClearFieldsOnListChange() {
   // $("#existExamYear").val(0);
    $("#existCourseTypes").val(0);
    $("#SeatAllocationReviewId").val(0);
}

function GetExamType() {
    $("#ExamYear").empty();
    $("#ExamYear").append('<option value="0">choose</option>');

    $("#UpdateExamYear").empty();
    $("#UpdateExamYear").append('<option value="0">choose</option>');

    $("#ViewExamYear").empty();
    $("#ViewExamYear").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetExamYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#UpdateExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ViewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetCourseTypes() {

    $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="0">choose</option>');

    $("#UpdateCourseTypes").empty();
    $("#UpdateCourseTypes").append('<option value="0">choose</option>');

    $("#ViewCourseTypes").empty();
    $("#ViewCourseTypes").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#CourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#UpdateCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ViewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetSyllabusType() {
    $("#Syllabus").empty();
    $("#Syllabus").append('<option value="0">choose</option>');
    $("#UpdateSyllabus").empty();
    $("#UpdateSyllabus").append('<option value="0">choose</option>');
    $("#ViewSyllabus").empty();
    $("#ViewSyllabus").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetSyllabusType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Syllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                    $("#UpdateSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                    $("#ViewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GridView() {
    //$("#btnSubmitdivid").hide();
    $("#GMPerc").attr('disabled', true);
    $("#SCPerc").attr('disabled', true);
    $("#STPerc").attr('disabled', true);
    $("#C1Perc").attr('disabled', true);
    $("#IIAPerc").attr('disabled', true);
    $("#IIBPerc").attr('disabled', true);
    $("#IIIAPerc").attr('disabled', true);
    $("#IIIBPerc").attr('disabled', true);

    $("#WomenPerc").attr('disabled', true);
    $("#PerDisabilitiesperc").attr('disabled', true);
    $("#ExServicePerc").attr('disabled', true);
    $("#KanMediumPerc").attr('disabled', true);
    $("#LandLoserPerc").attr('disabled', true);
    $("#KashmiriMigrPerc").attr('disabled', true);
    $("#EconomicWeakerPerc").attr('disabled', true);
    $("#GPPerc").attr('disabled', true);

    $("#HyderabadRegion").attr('disabled', true);
    $("#NonHyderabadRegion").attr('disabled', true);
    $("#NHyderabadRegion").attr('disabled', true);
    $("#NNonHyderabadRegion").attr('disabled', true);

    $("#GradespercA1").attr('disabled', true);
    $("#GradespercA2").attr('disabled', true);
    $("#GradespercB1").attr('disabled', true);
    $("#GradespercB2").attr('disabled', true);
    $("#GradespercC1").attr('disabled', true);
    $("#GradespercC2").attr('disabled', true);
    $("#GradespercD").attr('disabled', true);
    $("#GradespercE1").attr('disabled', true);
    $("#GradespercE2").attr('disabled', true);

    $("#ReservationRural").attr('disabled', true);
    $("#ReservationTransgender").attr('disabled', true);

    $("#Syllabus").attr('disabled', true);
    //$("#ForwardTo").attr('disabled', true);
}

function GridEdit() {

    $("#GM-Add-Required").hide();
    $("#ForwardTo-Required").hide();

    $("#btnSubmitdivid").show();

    $("#seatTypeId").attr("disabled", false);
    $("#numberofSeats").attr("disabled", false);
    $("#numberofSeatMngmt").attr("disabled", false);

    $("#GMPerc").attr('disabled', false);
    $("#SCPerc").attr('disabled', false);
    $("#STPerc").attr('disabled', false);
    $("#C1Perc").attr('disabled', false);
    $("#IIAPerc").attr('disabled', false);
    $("#IIBPerc").attr('disabled', false);
    $("#IIIAPerc").attr('disabled', false);
    $("#IIIBPerc").attr('disabled', false);

    $("#WomenPerc").attr('disabled', false);
    $("#PerDisabilitiesperc").attr('disabled', false);
    $("#ExServicePerc").attr('disabled', false);
    $("#KanMediumPerc").attr('disabled', false);
    $("#LandLoserPerc").attr('disabled', false);
    $("#KashmiriMigrPerc").attr('disabled', false);
    $("#EconomicWeakerPerc").attr('disabled', false);
    $("#GPPerc").attr('disabled', false);

    $("#HyderabadRegion").attr('disabled', false);
    $("#NonHyderabadRegion").attr('disabled', false);
    $("#NHyderabadRegion").attr('disabled', false);
    $("#NNonHyderabadRegion").attr('disabled', false);

    $("#GradespercA1").attr('readonly', false);
    $("#GradespercA2").attr('readonly', false);
    $("#GradespercB1").attr('readonly', false);
    $("#GradespercB2").attr('readonly', false);
    $("#GradespercC1").attr('readonly', false);
    $("#GradespercC2").attr('readonly', false);
    $("#GradespercD").attr('readonly', false);
    $("#GradespercE1").attr('readonly', false);
    $("#GradespercE2").attr('readonly', false);

    $("#ReservationRural").attr('disabled', false);
    $("#ReservationTransgender").attr('disabled', false);

    $("#Syllabus").attr('readonly', false);
    $("#GridSubmit").css('visibility', 'hidden');
}

function DataSubmit() {
    var IsValid = true;
    var ChkExistanceObj = {
        Exam_Year: $('#ExamYear').val().split(" ")[1],//$('#existExamYear :selected').val(),
        CourseId: $('#CourseTypes :selected').val(),
        Syllabus: $('#Syllabus :selected').val()
    }
    if ($('#WithExistingData').is(':checked')) {
        if ($("#cbxModifyData").prop('checked') == false) {
            var Exam_Year = $('#ExamYear').val().split(" ")[1];
            var exist_Year = $('#existExamYear :selected').val();

            if (Exam_Year != exist_Year) {
                $("#ForwardTo").attr("disabled", true);
                $("#ForwardTo-Required").hide();
                IsValid = true;
            }
        }
        else
            $("#ForwardTo").removeAttr("disabled");
    }
    else {
        $("#ForwardTo").removeAttr("disabled");
    }
    
    $.ajax({
        type: "POST",
        url: "/Admission/RuleAllocationChkExistence",
        data: JSON.stringify(ChkExistanceObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.toLowerCase() == "record already exists") {
                $("#RecordAlreadyExist").val(1);
                bootbox.alert("<br><br> Rules of seat allocation for the Session <b>:" + $('#ExamYear').val() + "</b> for the Course Type : <b>" + $('#CourseTypes :selected').text() + "</b> already Exists");
            }
            else {
                var ErrorValidationRetValue = ErrorValidation(IsValid);
                if (ErrorValidationRetValue) {

                    $("#RecordAlreadyExist").val(0);
                    $("#btnSubmitdivid").hide();
                    $("#GridDivId").show();
                    var ExamYearValue = $('#ExamYear').val();
                    $("#lblExamYear").text(ExamYearValue);
                    $("#GridSubmit").css('visibility', 'visible');
                    var CourseTypesValue = $('#CourseTypes :selected').text();
                    $("#lblCourseDetails").text(CourseTypesValue);
                    $("#seatTypeId").attr("disabled", true);
                    $("#numberofSeats").attr("disabled", true);
                    $("#numberofSeatMngmt").attr("disabled", true);

                    $("#GMPerc").attr("disabled", true);
                    $("#SCPerc").attr("disabled", true);
                    $("#STPerc").attr("disabled", true);
                    $("#C1Perc").attr("disabled", true);
                    $("#IIAPerc").attr("disabled", true);
                    $("#IIBPerc").attr("disabled", true);
                    $("#IIIAPerc").attr("disabled", true);
                    $("#IIIBPerc").attr("disabled", true);


                    $("#WomenPerc").attr("disabled", true);
                    $("#PerDisabilitiesperc").attr("disabled", true);
                    $("#ExServicePerc").attr("disabled", true);
                    $("#KanMediumPerc").attr("disabled", true);
                    $("#EconomicWeakerPerc").attr("disabled", true);
                    $("#GPPerc").attr("disabled", true);
                    $("#HyderabadRegion").attr("disabled", true);
                    $("#HyderabadRegion").attr("disabled", true);
                    $("#NonHyderabadRegion").attr("disabled", true);
                    $("#NHyderabadRegion").attr("disabled", true);
                    $("#NNonHyderabadRegion").attr("disabled", true);
                    $("#ReservationRural").attr("disabled", true);
                    $("#ReservationTransgender").attr("disabled", true);
                }
            }
        }
    });
}

function ErrorValidation(IsValid) {

    ErrorValidationHide();

    //Year Validation
    if ($("#ExamYear").val() == 0) {
        IsValid = false;
        $("#ExamYear-Required").show();
    }
    if ($("#CourseTypes").val() == 0) {
        IsValid = false;
        $("#CourseTypes-Required").show();
    }
    if ($("#Syllabus").val() == 0) {
        IsValid = false;
        $("#Syllabus-Required").show();
    }

    //Vertical Error Validation
    if ($("#GMPerc").val() == "") {
        IsValid = false;
        $("#GM-Required").show();
    }
    if ($("#SCPerc").val() == "") {
        IsValid = false;
        $("#SC-Required").show();
    }
    if ($("#STPerc").val() == "") {
        IsValid = false;
        $("#ST-Required").show();
    }
    if ($("#C1Perc").val() == "") {
        IsValid = false;
        $("#C1-Required").show();
    }
    if ($("#IIAPerc").val() == "") {
        IsValid = false;
        $("#IIAPerc-Required").show();
    }
    if ($("#IIBPerc").val() == "") {
        IsValid = false;
        $("#IIBPerc-Required").show();
    }
    if ($("#IIIAPerc").val() == "") {
        IsValid = false;
        $("#IIIAPerc-Required").show();
    }
    if ($("#IIIBPerc").val() == "") {
        IsValid = false;
        $("#IIIBPerc-Required").show();
    }

    //Horizontal Error Validation
    if ($("#WomenPerc").val() == "") {
        IsValid = false;
        $("#Women-Required").show();
    }
    if ($("#PerDisabilitiesperc").val() == "") {
        IsValid = false;
        $("#Disabilities-Required").show();
    }
    if ($("#ExServicePerc").val() == "") {
        IsValid = false;
        $("#Service-Required").show();
    }
    if ($("#KanMediumPerc").val() == "") {
        IsValid = false;
        $("#KanMediumPerc-Required").show();
    }
    //if ($("#LandLoserPerc").val() == "") {
    //    IsValid = false;
    //    $("#LandLoserPerc-Required").show();
    //}
    //if ($("#KashmiriMigrPerc").val() == "") {
    //    IsValid = false;
    //    $("#KashmiriMigrPerc-Required").show();
    //}
    if ($("#EconomicWeakerPerc").val() == "") {
        IsValid = false;
        $("#EconomicWeakerPerc-Required").show();
    }
    if ($("#GPPerc").val() == "") {
        IsValid = false;
        $("#GPPerc-Required").show();
    }

    //Hyderabad Region Regionwise Error Validation
    if ($("#HyderabadRegion").val() == "") {
        IsValid = false;
        $("#HyderabadRegion-Required").show();
    }
    if ($("#NonHyderabadRegion").val() == "") {
        IsValid = false;
        $("#NonHyderabadRegion-Required").show();
    }

    //Non - Hyderabad Region Regionwise Error Validation
    if ($("#NHyderabadRegion").val() == "") {
        IsValid = false;
        $("#NHyderabadRegion-Required").show();
    }
    if ($("#NNonHyderabadRegion").val() == "") {
        IsValid = false;
        $("#NNonHyderabadRegion-Required").show();
    }

    //Grades Error Validation
    if ($("#GradespercA1").val() == "") {
        IsValid = false;
        $("#GradespercA1-Required").show();
    }
    if ($("#GradespercA2").val() == "") {
        IsValid = false;
        $("#GradespercA2-Required").show();
    }
    if ($("#GradespercB1").val() == "") {
        IsValid = false;
        $("#GradespercB1-Required").show();
    }
    if ($("#GradespercB2").val() == "") {
        IsValid = false;
        $("#GradespercB2-Required").show();
    }
    if ($("#GradespercC1").val() == "") {
        IsValid = false;
        $("#GradespercC1-Required").show();
    }
    if ($("#GradespercC2").val() == "") {
        IsValid = false;
        $("#GradespercC2-Required").show();
    }
    if ($("#GradespercD").val() == "") {
        IsValid = false;
        $("#GradespercD-Required").show();
    }
    if ($("#GradespercE1").val() == "") {
        IsValid = false;
        $("#GradespercE1-Required").show();
    }
    if ($("#GradespercE2").val() == "") {
        IsValid = false;
        $("#GradespercE2-Required").show();
    }
    if ($("#ReservationRural").val() == "") {
        IsValid = false;
        $("#ReservationRural-Required").show();
    }
    if ($("#ReservationTransgender").val() == "") {
        IsValid = false;
        $("#ReservationTransgender-Required").show();
    }

    var toFindTotVerticalPerc = parseInt($("#GMPerc").val()) + parseInt($("#SCPerc").val()) + parseInt($("#STPerc").val()) + parseInt($("#C1Perc").val()) + parseInt($("#IIAPerc").val()) + parseInt($("#IIBPerc").val()) +
        parseInt($("#IIIAPerc").val()) + parseInt($("#IIIBPerc").val());

    var toFindTotHorizontalPerc = parseInt($("#WomenPerc").val()) + parseInt($("#PerDisabilitiesperc").val()) + parseInt($("#ExServicePerc").val()) + parseInt($("#KanMediumPerc").val()) + parseInt($("#GPPerc").val()) + parseInt($("#EconomicWeakerPerc").val());

    //var toFindTotHorizontalPerc = parseInt($("#WomenPerc").val()) + parseInt($("#PerDisabilitiesperc").val()) + parseInt($("#ExServicePerc").val()) + parseInt($("#KanMediumPerc").val()) + parseInt($("#LandLoserPerc").val()) + parseInt($("#GPPerc").val()) +
    //parseInt($("#KashmiriMigrPerc").val()) + parseInt($("#EconomicWeakerPerc").val());

    var toFindTotHyderabadRegionPerc = parseInt($("#HyderabadRegion").val()) + parseInt($("#NonHyderabadRegion").val());
    var toFindTotNonHyderabadRegionPerc = parseInt($("#NHyderabadRegion").val()) + parseInt($("#NNonHyderabadRegion").val());
    //var toFindTotGradesPerc = parseInt($("#GradespercA1").val()) + parseInt($("#GradespercA2").val()) + parseInt($("#GradespercB1").val()) + parseInt($("#GradespercB2").val()) + parseInt($("#GradespercC1").val()) +
    //parseInt($("#GradespercC2").val()) + parseInt($("#GradespercD").val()) + parseInt($("#GradespercE1").val()) + parseInt($("#GradespercE2").val());
    //var toFindTotOtherReservationPerc = parseInt($("#ReservationRural").val()) + parseInt($("#ReservationTransgender").val());
    var ReservationRuralVal = parseInt($("#ReservationRural").val());
    var ReservationTransgenderVal = parseInt($("#ReservationTransgender").val());

    var AlertErrValidationMsg = "";
    if (toFindTotVerticalPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg = "<br><br>*Total Vertical Percentage must be equals to 100 Percentage";
    }

    if (toFindTotHorizontalPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Horizontal Percentage must be equals to 100 Percentage";
    }

    if (toFindTotHyderabadRegionPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Hyderabad region must be equals to 100 Percentage";
    }

    if (toFindTotNonHyderabadRegionPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Non Hyderabad region must be equals to 100 Percentage";
    }

    if (ReservationRuralVal > 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Rural must be between 0 and 100 Percentage";
    }

    if (ReservationTransgenderVal > 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Transgender must be between 0 and 100 Percentage";
    }

    //if (toFindTotGradesPerc != 100) {
    //    IsValid = false;
    //    AlertErrValidationMsg += "<br><br>*Total Grades must be equals to 100 Percentage";
    //}

    //if (toFindTotOtherReservationPerc != 100) {
    //    IsValid = false;
    //    AlertErrValidationMsg += "<br><br>*Total Other Reservation Rule must be equals to 100 Percentage";
    //}

    //if (toFindTotVerticalPerc != 100 || toFindTotHorizontalPerc != 100 || toFindTotOtherReservationPerc != 100 ||
    if (toFindTotVerticalPerc != 100 || toFindTotHorizontalPerc != 100 || ReservationRuralVal > 100 || ReservationTransgenderVal > 100 ||
        toFindTotNonHyderabadRegionPerc != 100 || toFindTotHyderabadRegionPerc != 100) {
        bootbox.alert(AlertErrValidationMsg);
    }

    //if (toFindTotVerticalPerc != 100 || toFindTotHorizontalPerc != 100 || toFindTotOtherReservationPerc != 100 || toFindTotGradesPerc != 100 ||
    //    toFindTotNonHyderabadRegionPerc != 100 || toFindTotHyderabadRegionPerc != 100) {
    //    bootbox.alert(AlertErrValidationMsg);
    //}

    return IsValid;
}

function ErrorValidationHide() {

    $("#GM-Add-Required").hide();
    $("#GM-Required").hide();
    $("#SC-Required").hide();
    $("#ST-Required").hide();
    $("#C1-Required").hide();
    $("#IIAPerc-Required").hide();
    $("#IIBPerc-Required").hide();
    $("#IIIAPerc-Required").hide();
    $("#IIIBPerc-Required").hide();

    $("#Women-Required").hide();
    $("#Disabilities-Required").hide();
    $("#Service-Required").hide();
    $("#KanMediumPerc-Required").hide();
    $("#LandLoserPerc-Required").hide();
    $("#KashmiriMigrPerc-Required").hide();
    $("#EconomicWeakerPerc-Required").hide();
    $("#GPPerc-Required").hide();

    $("#HyderabadRegion-Required").hide();
    $("#NHyderabadRegion-Required").hide();
    $("#NonHyderabadRegion-Required").hide();
    $("#NNonHyderabadRegion-Required").hide();

    $("#Women-Required").hide();
    $("#SC-Required").hide();

    $("#GradespercA1-Required").hide();
    $("#GradespercA2-Required").hide();
    $("#GradespercB1-Required").hide();
    $("#GradespercB2-Required").hide();
    $("#GradespercC1-Required").hide();
    $("#GradespercC2-Required").hide();
    $("#GradespercD-Required").hide();
    $("#GradespercE1-Required").hide();
    $("#GradespercE2-Required").hide();

    $("#ReservationRural-Required").hide();
    $("#ReservationTransgender-Required").hide();

    $("#ExamYear-Required").hide();
    $("#CourseTypes-Required").hide();
    $("#Syllabus-Required").hide();
}

function InsertSeatAllocationData() {
    var IsValid = true; var RecordAlreadyExistCnt = 0;
    var ErrorValidationRetValue = ErrorValidation(IsValid);
    var remarks = $('#RemarksTxt').val().trim();
    if (ErrorValidationRetValue) {
       
        $("#ForwardTo-Required").hide();       
        if ($("#ForwardTo").is(":disabled") == false && $("#ForwardTo").val() == 0) {
            IsValid = false;
            $("#ForwardTo-Required").show();
        } else {
            $("#ForwardTo-Required").hide();
        }
      
        if (remarks == "") {
            IsValid = false;
            $("#Remarks-Required").show();
        } else {
            $("#Remarks-Required").hide();
        }
        RecordAlreadyExistCnt = $("#RecordAlreadyExist").val();
        if (IsValid && RecordAlreadyExistCnt == 0) {


            bootbox.confirm({
                message: "<br><br>Are you sure, you want to submit the rules of seat allocation ?",
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
                    ToSuccessfulSaveYesDataSave();
                }
                }
            });
        }
        else {
            bootbox.alert("<br><br>Kindly clear the error in that page");
        }

    }
}
function ToSuccessfulSaveYesDataSave() {

    var StatusId = 0;
    if ($("#ForwardTo").is(":disabled") == true) {
            StatusId = status.Approved;
    }
    var seatAllocationObj = {
        Exam_Year: $('#ExamYear').val().split(" ")[1],
        CourseId: $('#CourseTypes :selected').val(),
        Rules_allocation_masterid: $("#SeatAllocationReviewId").val(),
        FlowId: $("#ForwardTo").val(),
        Remarks: $("#RemarksTxt").val(),
        Seat_typeId: $('#seatTypeId :selected').val(),
        //Govtseats: $("#numberofSeats").val(),
        //Managementseats: $("#numberofSeatMngmt").val(),
        StatusId: StatusId,

        ConsolidatedVerticalRules: [
            $('#GMPerc').val(),
            $('#SCPerc').val(),
            $('#STPerc').val(),
            $('#C1Perc').val(),
            $('#IIAPerc').val(),
            $("#IIBPerc").val(),
            $("#IIIAPerc").val(),
            $("#IIIBPerc").val()
        ],

        ConsolidatedHorizontalRules: [
            $('#WomenPerc').val(),
            $('#PerDisabilitiesperc').val(),
            $('#ExServicePerc').val(),
            $('#KanMediumPerc').val(),
            $('#LandLoserPerc').val(),
            $("#KashmiriMigrPerc").val(),
            $("#EconomicWeakerPerc").val(),
            $("#GPPerc").val()
        ],

        ConsolidatedHyderabadRegion: [
            $("#HyderabadRegion").val(),
            $("#NonHyderabadRegion").val(),
            $("#NHyderabadRegion").val(),
            $("#NNonHyderabadRegion").val()
        ],
       
        //Syllabus_type_id: $('#Syllabus :selected').val(),
        //ConsolidatedGradeRules: [
        //    $('#GradespercA1').val(),
        //    $('#GradespercA2').val(),
        //    $('#GradespercB1').val(),
        //    $('#GradespercB2').val(),
        //    $('#GradespercC1').val(),
        //    $("#GradespercC2").val(),
        //    $("#GradespercD").val(),
        //    $("#GradespercE1").val(),
        //    $("#GradespercE2").val()
        //],

        ConsolidatedOtherRules: [
            $("#ReservationRural").val(),
            $("#ReservationTransgender").val(),
            $("#numberofSeats").val()
        ]
        //seatGovtandMgmnt: [
        //    $("#numberofSeats").val(),
        //    //$("#numberofSeatMngmt").val(),
        //],
    };

    $.ajax({
        type: "POST",
        url: "/Admission/AllocationDataInsertUpdateRules",
        data: JSON.stringify(seatAllocationObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == "failed") {
                bootbox.alert("<br><br>There is error in your Rules of seat allocation data");
            }
            else {
                var msg = "";
                if ($("#ForwardTo").is(":disabled") == true) {
                    msg = "<br><br>Rules of Seat Allocation Approved Successfully";
                }
                else
                    msg = "<br><br>Rules of seat allocation created for the Session : <b>" + $('#ExamYear').val() + "</b> for the Course Type : <b>" + $('#CourseTypes :selected').text() + " </b> and forwarded to <b>" + $('#ForwardTo :selected').text() + "</b> for review successfully";

                bootbox.alert({
                    message: msg,  
                    callback: function () { location.reload(true); }
                });
               // $("#ExamYear").val(0);
                $("#CourseTypes").val(0);
                $("#GridDivId").hide();
                ClearAllFields();
            }
        }
    });
}

function ExamYearOnChange() {
    ClearAllFields();
    ErrorValidationHide();
    var Exam_Year = $('#ExamYear').val().split(" ")[1];
    var CourseId = $('#CourseTypes :selected').val();
    var tabNameValue = "tab1";

    if ($('#WithExistingData').is(':checked')) {

        $("#hideDataForRDBSelection").show();
        var Exam_Year = $('#existExamYear :selected').val();
        var CourseId = $('#existCourseTypes :selected').val();
        var tabNameValue = "tab3";       
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAllocationDataInsertUpdateRules",
        data: { Exam_Year: Exam_Year, CourseId: CourseId, tabNameValue: tabNameValue },
        success: function (data) {
            if (data != null || data != '') {
                var RuleVal = 0;
                var Vrtotal = 0;  
                var Hrtotal = 0;    
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#GMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#SCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#STPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#C1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#IIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#IIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#IIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#IIIBPerc").val(this.RuleValue);
                    }

                    RuleVal = this.RuleValue;                   
                    Vrtotal += RuleVal;                  
                    if ($('#WithExistingData').is(':checked') &&
                        $(event.target).is('#ExamYear') ||
                        $(event.target).is('#CourseTypes')) {

                        $("#ExamYear").val(this.Exam_Year);
                        $("#CourseTypes").val(this.CourseId);
                        $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                    }
                });
                $("#TotalPer").val(Vrtotal);
                var HRuleVal = 0;
                var HVrtotal = 0;
                var HHrtotal = 0;
                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#WomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#PerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#KanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#LandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#KashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#EconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#GPPerc").val(this.RuleValue);
                    }
                    HRuleVal = this.RuleValue;
                    HHrtotal += HRuleVal;
                });

                $("#hrPercent").val(HHrtotal);
                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#HyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#NHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#GradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#GradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#GradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#GradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#GradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#GradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#GradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#GradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#GradespercE2").val(this.RuleValue);
                    }
                    $("#Syllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ReservationTransgender").val(this.RuleValue);
                    }
                });
            }
        }
    });
}

function UpdateExamYearOnChange() {

    ClearAllFields();
    var Exam_Year = $('#UpdateExamYear :selected').val();
    var CourseId = $('#UpdateCourseTypes :selected').val();
    var tabNameValue = "tab2";
    $("#btnSubmitdivid").hide();
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAllocationDataInsertUpdateRules",
        data: { Exam_Year: Exam_Year, CourseId: CourseId, tabNameValue: tabNameValue },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#UpdateGMPerc").val(this.RuleValue);
                        //$("#ViewGMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#UpdateSCPerc").val(this.RuleValue);
                        //$("#ViewSCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#UpdateSTPerc").val(this.RuleValue);
                        //$("#ViewSTPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#UpdateC1Perc").val(this.RuleValue);
                        //$("#ViewC1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#UpdateIIAPerc").val(this.RuleValue);
                        //$("#ViewIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#UpdateIIBPerc").val(this.RuleValue);
                        //$("#ViewIIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#UpdateIIIAPerc").val(this.RuleValue);
                        //$("#ViewIIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#UpdateIIIBPerc").val(this.RuleValue);
                        //$("#ViewIIIBPerc").val(this.RuleValue);
                    }

                    $("#UpdateExamYear").val(this.Exam_Year);
                    //$("#ViewExamYear").val(this.Exam_Year);

                    $("#UpdateCourseTypes").val(this.CourseId);
                    //$("#ViewCourseTypes").val(this.CourseId);

                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#UpdateWomenPerc").val(this.RuleValue);
                        //$("#ViewWomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#UpdatePerDisabilitiesperc").val(this.RuleValue);
                        //$("#ViewPerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#UpdateExServicePerc").val(this.RuleValue);
                        //$("#ViewExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#UpdateKanMediumPerc").val(this.RuleValue);
                        //$("#ViewKanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#UpdateLandLoserPerc").val(this.RuleValue);
                        //$("#ViewLandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#UpdateKashmiriMigrPerc").val(this.RuleValue);
                        //$("#ViewKashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#UpdateEconomicWeakerPerc").val(this.RuleValue);
                        //$("#ViewEconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#UpdateGPPerc").val(this.RuleValue);
                        //$("#ViewGPPerc").val(this.RuleValue);
                    }
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateHyderabadRegion").val(this.RuleValue);
                        //$("#ViewHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNonHyderabadRegion").val(this.RuleValue);
                        //$("#ViewNonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateNHyderabadRegion").val(this.RuleValue);
                        //$("#ViewNHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNNonHyderabadRegion").val(this.RuleValue);
                        //$("#ViewNNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        //$("#ViewGradespercA1").val(this.RuleValue);
                        $("#UpdateGradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        //$("#ViewGradespercA2").val(this.RuleValue);
                        $("#UpdateGradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        //$("#ViewGradespercB1").val(this.RuleValue);
                        $("#UpdateGradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        //$("#ViewGradespercB2").val(this.RuleValue);
                        $("#UpdateGradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        //$("#ViewGradespercC1").val(this.RuleValue);
                        $("#UpdateGradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        //$("#ViewGradespercC2").val(this.RuleValue);
                        $("#UpdateGradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        //$("#ViewGradespercD").val(this.RuleValue);
                        $("#UpdateGradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        //$("#ViewGradespercE1").val(this.RuleValue);
                        $("#UpdateGradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        //$("#ViewGradespercE2").val(this.RuleValue);
                        $("#UpdateGradespercE2").val(this.RuleValue);
                    }

                    $("#UpdateSyllabus").val(this.Syllabus_type_id);
                    //$("#ViewSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#UpateReservationRural").val(this.RuleValue);
                        //$("#ViewReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#UpdateReservationTransgender").val(this.RuleValue);
                        //$("#ViewReservationTransgender").val(this.RuleValue);
                    }
                });
            }
        }
    });
}

function ViewExamYearOnChange() {

    ClearAllFields();
    var Exam_Year = $('#ViewExamYear :selected').val();
    var CourseId = $('#ViewCourseTypes :selected').val();
    var tabNameValue = "tab3";
    $("#btnSubmitdivid").hide();
    $("#GridDivId").hide();

    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAllocationDataInsertUpdateRules",
        data: { Exam_Year: Exam_Year, CourseId: CourseId, tabNameValue: tabNameValue },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#ViewGMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#ViewSCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#ViewSTPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#ViewC1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#ViewIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#ViewIIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#ViewIIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#ViewIIIBPerc").val(this.RuleValue);
                    }

                    $("#ViewExamYear").val(this.Exam_Year);
                    $("#ViewCourseTypes").val(this.CourseId);
                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#ViewWomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#ViewPerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ViewExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#ViewKanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#ViewLandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#ViewKashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#ViewEconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#ViewGPPerc").val(this.RuleValue);
                    }
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ViewHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ViewNonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ViewNHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ViewNNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#ViewGradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#ViewGradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#ViewGradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#ViewGradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#ViewGradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#ViewGradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#ViewGradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#ViewGradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#ViewGradespercE2").val(this.RuleValue);
                    }
                    $("#ViewSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ViewReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ViewReservationTransgender").val(this.RuleValue);
                    }
                });
            }
        }
    });
}

function PercentvalidationGM(eveObj, id) {
    var idvalueoftxt = (id);
    var text = $("#" + idvalueoftxt).val();
    var index = text.indexOf('.');
    var len = $("#" + idvalueoftxt).val().length;
    var text_lenght = $("#" + idvalueoftxt).toString().length;
    var total_dec = parseFloat(len) - parseFloat(index)
    if (total_dec > 3 || (total_dec <= 3 && (eveObj.charCode == 101 || eveObj.charCode==45))) {            
        event.preventDefault();
    }
}
$(document).on("change", ".percent", function () {
    var sum = 0;
    $(".percent").each(function () {
        sum += +$(this).val();     
        if (sum > 100) {
            $(".percent1").val(sum).css('color', 'red');
            $("#TotalPer-Required").show();
            $("#UpdateTotalPer-Required").show();
            $("#ViewTotalPer-Required").show();
        }
        else {
            $(".percent1").val(sum).css('color', 'black');
            $("#TotalPer-Required").hide();
            $("#UpdateTotalPer-Required").hide();
            $("#ViewTotalPer-Required").hide();
        }
    });
    
});
$(document).on("change", ".hrPercent", function () {
    var sum = 0;
    $(".hrPercent").each(function () {
        sum += +$(this).val();     
        if (sum > 100) {
            $(".hrPercent1").val(sum).css('color', 'red');
            $("#hrPercent1-Required").show();
            $("#UpdatehrPercent1-Required").show();
            $("#ViewhrPercent1-Required").show();
        }
        else {
            $(".hrPercent1").val(sum).css('color', 'black');
            $("#hrPercent1-Required").hide();
            $("#UpdatehrPercent1-Required").hide();
            $("#ViewhrPercent1-Required").hide();
        }
    });

});
function GetAllocationSeatofRuletoUpdate() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetAllocationSeatofRuletoUpdate',
        success: function (data) {
            $('#tblGetAllocationSeatofRuletoUpdate').DataTable({
                data: data,
                "destroy": true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.' },
                    { 'data': 'ExamYear', 'title': 'Session' },
                    { 'data': 'CourseName', 'title': 'Course Type', 'className': 'text-left' },
                    {
                        'data': 'StatusName', "render": function (data, type, row) {
                            return data + ' - ' + row['userRole'];
                        }
                        , 'title': 'Status - Currently With', 'className': 'text-left'
                    },                    
                    {
                        'data': 'Rules_allocation_masterid',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'Rules_allocation_masterid',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (data[0].StatusId == status.SendBackForCorrection || data[0].StatusId == status.SentBackForCorrection)
                                $(nTd).html("<input type='button' onclick='GetSeatAllocationById(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' data-toggle='modal' data-target='#myModal' value='Edit' id='edit'/>");
                            else
                                $(nTd).html("<input type='button' onclick='GetSeatAllocationById(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' data-toggle='modal' data-target='#myModal' value='View' id='edit'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetApprovedAllocationSeatofRuletoView() {

   // $("#existExamYear").empty();
   // $("#existExamYear").append('<option value="0">choose</option>');

    $("#existCourseTypes").empty();
    $("#existCourseTypes").append('<option value="0">choose</option>');


    $.ajax({
        type: 'Get',
        url: '/Admission/GetApprovedAllocationSeat',
        success: function (data) {
            $('#tblGetApprovedAllocationSeatofRuletoView').DataTable({
                data: data,
                "destroy": true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.' },
                    { 'data': 'ExamYear', 'title': 'Session' },
                    { 'data': 'CourseName', 'title': 'Course Type', 'className': 'text-left' },
                    {
                        'data': 'StatusName', "render": function (data, type, row) {
                            return data + ' - ' + row['role_description'];
                        }
                        , 'title': 'Status - Currently With', 'className': 'text-left'
                    },  
                    {
                        'data': 'Rules_allocation_masterid',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' data-toggle='modal' data-target='#myModal' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'Rules_allocation_masterid',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetSeatAllocationForViewById(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' data-toggle='modal' data-target='#myModal' value='View' id='view'/>");

                        }
                    }
                ]
            });

            if (data != null || data != '') {
                $.each(data, function () {
                    if (!$("#existExamYear option[value='" + this.YearID + "']").length) {
                        $("#existExamYear").append($("<option/>").val(this.YearID).text(this.ExamYear));
                    }
                    if (!$("#existCourseTypes option[value='" + this.CourseId + "']").length) {
                        $("#existCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseName));
                    }
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetApprovedSeatAllocationForViewById(Rules_allocation_masterid) {
    ClearAllFields();
    $("#UpdateGridDivId").hide();
    $("#EditPanel").show();
    $("#UpdatebtnSubmitdivid").hide();
    $("#UpdateExamYear").val(0);
    $("#UpdateCourseTypes").val(0);
    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationApproved',
        data: { Rules_allocation_masterid: Rules_allocation_masterid },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#UpdateGMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#UpdateSCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#UpdateSTPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#UpdateC1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#UpdateIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#UpdateIIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#UpdateIIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#UpdateIIIBPerc").val(this.RuleValue);
                    }

                    $("#UpdateExamYear").val(this.Exam_Year);
                    $("#UpdateCourseTypes").val(this.CourseId);
                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#UpdateWomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#UpdatePerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#UpdateExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#UpdateKanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#UpdateLandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#UpdateKashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#UpdateEconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#UpdateGPPerc").val(this.RuleValue);
                    }
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateNHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#UpdateGradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#UpdateGradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#UpdateGradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#UpdateGradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#UpdateGradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#UpdateGradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#UpdateGradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#UpdateGradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#UpdateGradespercE2").val(this.RuleValue);
                    }
                    $("#UpdateSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#UpdateReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#UpdateReservationTransgender").val(this.RuleValue);
                    }
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetSeatAllocationById(Rules_allocation_masterid) {
    ClearAllFields();
    $("#UpdateGridDivId").hide();
    $("#EditPanel").show();
    $("#UpdatebtnSubmitdivid").show();
    $("#CancelbtnSubmitdivid").hide();
    $("#UpdateExamYear").val(0);
    $("#UpdateCourseTypes").val(0);
    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationById',
        data: { Rules_allocation_masterid: Rules_allocation_masterid },
        success: function (data) {
            if (data != null || data != '') {
                var sessionValue = $("#hdnSession").data('value');

                $.each(data.list7, function () {
                    $("#UpdateExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                });

                $.each(data.list8, function () {
                    $("#UpdateCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });

                $.each(data.list9, function () {
                    $("#UpdateSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });
                var RuleVal = 0;
                var Vrtotal = 0;
                var Hrtotal = 0;
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#UpdateGMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#UpdateSCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#UpdateSTPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#UpdateC1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#UpdateIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#UpdateIIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#UpdateIIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#UpdateIIIBPerc").val(this.RuleValue);
                    }

                    RuleVal = this.RuleValue;
                    Vrtotal += RuleVal;

                    $("#UpdateExamYear").val(this.Exam_Year);
                    $("#UpdateCourseTypes").val(this.CourseId);
                    $("#UpdateSyllabus").val(this.Rules_allocation_masterid);
                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                });
                $("#UpdateTotalPer").val(Vrtotal);
                var HRuleVal = 0;
                var HVrtotal = 0;
                var HHrtotal = 0;
                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#UpdateWomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#UpdatePerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#UpdateExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#UpdateKanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#UpdateLandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#UpdateKashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#UpdateEconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#UpdateGPPerc").val(this.RuleValue);
                    }
                    HRuleVal = this.RuleValue;
                    HHrtotal += HRuleVal;
                });
                $("#UpdatehrPercent").val(HHrtotal);
                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#UpdateNHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#UpdateNNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#UpdateGradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#UpdateGradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#UpdateGradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#UpdateGradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#UpdateGradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#UpdateGradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#UpdateGradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#UpdateGradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#UpdateGradespercE2").val(this.RuleValue);
                    }
                    $("#UpdateSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#UpdateReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#UpdateReservationTransgender").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Management Quota Seats") {
                        $("#numberofSeatsEdit").val(this.RuleValue);
                    }
                });

                $.each(data.list6, function () {
             
                    $("#UpdateRemarksTxt").val();
                    $("#StatusId").val(0);

                    if (sessionValue == this.FlowId)
                        $("#UpdateSubmit").show();
                    else
                        $("#UpdateSubmit").hide();
                });

                $("#EditPanel").show();
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}


function UpdateGridView() {
    $("#UpdatebtnSubmitdivid").hide();
    $("#UpdateGMPerc").attr('readonly', true);
    $("#UpdateSCPerc").attr('readonly', true);
    $("#UpdateSTPerc").attr('readonly', true);
    $("#UpdateC1Perc").attr('readonly', true);
    $("#UpdateIIAPerc").attr('readonly', true);
    $("#UpdateIIBPerc").attr('readonly', true);
    $("#UpdateIIIAPerc").attr('readonly', true);
    $("#UpdateIIIBPerc").attr('readonly', true);

    $("#UpdateWomenPerc").attr('readonly', true);
    $("#UpdatePerDisabilitiesperc").attr('readonly', true);
    $("#UpdateExServicePerc").attr('readonly', true);
    $("#UpdateKanMediumPerc").attr('readonly', true);
    $("#UpdateLandLoserPerc").attr('readonly', true);
    $("#UpdateKashmiriMigrPerc").attr('readonly', true);
    $("#UpdateEconomicWeakerPerc").attr('readonly', true);
    $("#UpdateGPPerc").attr('readonly', true);

    $("#UpdateHyderabadRegion").attr('readonly', true);
    $("#UpdateNonHyderabadRegion").attr('readonly', true);
    $("#UpdateNHyderabadRegion").attr('readonly', true);
    $("#UpdateNNonHyderabadRegion").attr('readonly', true);

    $("#UpdateGradespercA1").attr('readonly', true);
    $("#UpdateGradespercA2").attr('readonly', true);
    $("#UpdateGradespercB1").attr('readonly', true);
    $("#UpdateGradespercB2").attr('readonly', true);
    $("#UpdateGradespercC1").attr('readonly', true);
    $("#UpdateGradespercC2").attr('readonly', true);
    $("#UpdateGradespercD").attr('readonly', true);
    $("#UpdateGradespercE1").attr('readonly', true);
    $("#UpdateGradespercE2").attr('readonly', true);

    $("#UpdateReservationRural").attr('readonly', true);
    $("#UpdateReservationTransgender").attr('readonly', true);

    $("#UpdateSyllabus").attr('readonly', true);
}

function UpdateGridEdit() {

    $("#UpdateGM-Add-Required").hide();
    $("#UpdateForwardTo-Required").hide();

    $("#UpdatebtnSubmitdivid").show();

    $("#UpdateGMPerc").attr('readonly', false);
    $("#UpdateSCPerc").attr('readonly', false);
    $("#UpdateSTPerc").attr('readonly', false);
    $("#UpdateC1Perc").attr('readonly', false);
    $("#UpdateIIAPerc").attr('readonly', false);
    $("#UpdateIIBPerc").attr('readonly', false);
    $("#UpdateIIIAPerc").attr('readonly', false);
    $("#UpdateIIIBPerc").attr('readonly', false);

    $("#UpdateWomenPerc").attr('readonly', false);
    $("#UpdatePerDisabilitiesperc").attr('readonly', false);
    $("#UpdateExServicePerc").attr('readonly', false);
    $("#UpdateKanMediumPerc").attr('readonly', false);
    $("#UpdateLandLoserPerc").attr('readonly', false);
    $("#UpdateKashmiriMigrPerc").attr('readonly', false);
    $("#UpdateEconomicWeakerPerc").attr('readonly', false);
    $("#UpdateGPPerc").attr('readonly', false);

    $("#UpdateHyderabadRegion").attr('readonly', false);
    $("#UpdateNonHyderabadRegion").attr('readonly', false);
    $("#UpdateNHyderabadRegion").attr('readonly', false);
    $("#UpdateNNonHyderabadRegion").attr('readonly', false);

    $("#UpdateGradespercA1").attr('readonly', false);
    $("#UpdateGradespercA2").attr('readonly', false);
    $("#UpdateGradespercB1").attr('readonly', false);
    $("#UpdateGradespercB2").attr('readonly', false);
    $("#UpdateGradespercC1").attr('readonly', false);
    $("#UpdateGradespercC2").attr('readonly', false);
    $("#UpdateGradespercD").attr('readonly', false);
    $("#UpdateGradespercE1").attr('readonly', false);
    $("#UpdateGradespercE2").attr('readonly', false);

    $("#UpdateReservationRural").attr('readonly', false);
    $("#UpdateReservationTransgender").attr('readonly', false);

    $("#UpdateSyllabus").attr('readonly', false);
    $("#UpdateGridSubmit").css('visibility', 'hidden');
}

function UpdateDataSubmit() {
    var IsValid = true;
    $("#UpdateGM-Required").hide();
    $("#UpdateSC-Required").hide();
    $("#UpdateST-Required").hide();
    $("#UpdateC1-Required").hide();
    $("#UpdateIIAPerc-Required").hide();
    $("#UpdateIIBPerc-Required").hide();
    $("#UpdateIIIAPerc-Required").hide();
    $("#UpdateIIIBPerc-Required").hide();

    $("#UpdateWomen-Required").hide();
    $("#UpdateDisabilities-Required").hide();
    $("#UpdateService-Required").hide();
    $("#UpdateKanMediumPerc-Required").hide();
    $("#UpdateLandLoserPerc-Required").hide();
    $("#UpdateKashmiriMigrPerc-Required").hide();
    $("#UpdateEconomicWeakerPerc-Required").hide();
    $("#UpdateGPPerc-Required").hide();

    $("#UpdateHyderabadRegion-Required").hide();
    $("#UpdateNHyderabadRegion-Required").hide();
    $("#UpdateNonHyderabadRegion-Required").hide();
    $("#UpdateNNonHyderabadRegion-Required").hide();

    $("#UpdateWomen-Required").hide();
    $("#UpdateSC-Required").hide();

    $("#UpdateReservationRural-Required").hide();
    $("#UpdateReservationTransgender-Required").hide();

    $("#UpdateExamYear-Required").hide();
    $("#UpdateCourseTypes-Required").hide();
    $("#UpdateSyllabus-Required").hide();
    //Year Validation
    if ($("#UpdateExamYear").val() == 0) {
        IsValid = false;
        $("#UpdateExamYear-Required").show();
    }
    if ($("#UpdateCourseTypes").val() == 0) {
        IsValid = false;
        $("#UpdateCourseTypes-Required").show();
    }

    //Vertical Error Validation
    if ($("#UpdateGMPerc").val() == "") {
        IsValid = false;
        $("#UpdateGM-Required").show();
    }
    if ($("#UpdateSCPerc").val() == "") {
        IsValid = false;
        $("#UpdateSC-Required").show();
    }
    if ($("#UpdateSTPerc").val() == "") {
        IsValid = false;
        $("#UpdateST-Required").show();
    }
    if ($("#UpdateC1Perc").val() == "") {
        IsValid = false;
        $("#UpdateC1-Required").show();
    }
    if ($("#UpdateIIAPerc").val() == "") {
        IsValid = false;
        $("#UpdateIIAPerc-Required").show();
    }
    if ($("#UpdateIIBPerc").val() == "") {
        IsValid = false;
        $("#UpdateIIBPerc-Required").show();
    }
    if ($("#UpdateIIIAPerc").val() == "") {
        IsValid = false;
        $("#UpdateIIIAPerc-Required").show();
    }
    if ($("#UpdateIIIBPerc").val() == "") {
        IsValid = false;
        $("#UpdateIIIBPerc-Required").show();
    }

    //Horizontal Error Validation
    if ($("#UpdateWomenPerc").val() == "") {
        IsValid = false;
        $("#UpdateWomen-Required").show();
    }
    if ($("#UpdatePerDisabilitiesperc").val() == "") {
        IsValid = false;
        $("#UpdateDisabilities-Required").show();
    }
    if ($("#UpdateExServicePerc").val() == "") {
        IsValid = false;
        $("#UpdateService-Required").show();
    }
    if ($("#UpdateKanMediumPerc").val() == "") {
        IsValid = false;
        $("#UpdateKanMediumPerc-Required").show();
    }
    if ($("#UpdateEconomicWeakerPerc").val() == "") {
        IsValid = false;
        $("#UpdateEconomicWeakerPerc-Required").show();
    }
    if ($("#UpdateGPPerc").val() == "") {
        IsValid = false;
        $("#UpdateGPPerc-Required").show();
    }

    //Hyderabad Region Regionwise Error Validation
    if ($("#UpdateHyderabadRegion").val() == "") {
        IsValid = false;
        $("#UpdateHyderabadRegion-Required").show();
    }
    if ($("#UpdateNonHyderabadRegion").val() == "") {
        IsValid = false;
        $("#UpdateNonHyderabadRegion-Required").show();
    }

    //Non - Hyderabad Region Regionwise Error Validation
    if ($("#UpdateNHyderabadRegion").val() == "") {
        IsValid = false;
        $("#UpdateNHyderabadRegion-Required").show();
    }
    if ($("#UpdateNNonHyderabadRegion").val() == "") {
        IsValid = false;
        $("#UpdateNNonHyderabadRegion-Required").show();
    }

    if ($("#UpdateReservationRural").val() == "") {
        IsValid = false;
        $("#UpdateReservationRural-Required").show();
    }
    if ($("#UpdateReservationTransgender").val() == "") {
        IsValid = false;
        $("#UpdateReservationTransgender-Required").show();
    }

    var toFindTotVerticalPerc = parseInt($("#UpdateGMPerc").val()) + parseInt($("#UpdateSCPerc").val()) + parseInt($("#UpdateSTPerc").val()) + parseInt($("#UpdateC1Perc").val()) + parseInt($("#UpdateIIAPerc").val()) + parseInt($("#UpdateIIBPerc").val()) +
        parseInt($("#UpdateIIIAPerc").val()) + parseInt($("#UpdateIIIBPerc").val());

    var toFindTotHorizontalPerc = parseInt($("#UpdateWomenPerc").val()) + parseInt($("#UpdatePerDisabilitiesperc").val()) + parseInt($("#UpdateExServicePerc").val()) + parseInt($("#UpdateKanMediumPerc").val()) + parseInt($("#UpdateGPPerc").val()) + parseInt($("#UpdateEconomicWeakerPerc").val());

    var toFindTotHyderabadRegionPerc = parseInt($("#UpdateHyderabadRegion").val()) + parseInt($("#UpdateNonHyderabadRegion").val());
    var toFindTotNonHyderabadRegionPerc = parseInt($("#UpdateNHyderabadRegion").val()) + parseInt($("#UpdateNNonHyderabadRegion").val());
    var ReservationRuralVal = parseInt($("#UpdateReservationRural").val());
    var ReservationTransgenderVal = parseInt($("#UpdateReservationTransgender").val());

    var AlertErrValidationMsg = "";
    if (toFindTotVerticalPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg = "<br><br>*Total Vertical Percentage must be equals to 100 Percentage";
    }

    if (toFindTotHorizontalPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Horizontal Percentage must be equals to 100 Percentage";
    }

    if (toFindTotHyderabadRegionPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Hyderabad region must be equals to 100 Percentage";
    }

    if (toFindTotNonHyderabadRegionPerc != 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Non Hyderabad region must be equals to 100 Percentage";
    }

    if (ReservationRuralVal > 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Rural must be between 0 and 100 Percentage";
    }

    if (ReservationTransgenderVal > 100) {
        IsValid = false;
        AlertErrValidationMsg += "<br><br>*Total Transgender must be between 0 and 100 Percentage";
    }

    if (toFindTotVerticalPerc != 100 || toFindTotHorizontalPerc != 100 || ReservationRuralVal > 100 || ReservationTransgenderVal > 100 ||
        toFindTotNonHyderabadRegionPerc != 100 || toFindTotHyderabadRegionPerc != 100) {
        bootbox.alert(AlertErrValidationMsg);
    }

    if (IsValid) {
        $("#UpdatebtnSubmitdivid").hide();
        $("#UpdateGridDivId").show();
        var ExamYearValue = $('#UpdateExamYear :selected').text();
        $("#UpdatelblExamYear").text(ExamYearValue);
        $("#UpdateGridSubmit").css('visibility', 'visible');
        var CourseTypesValue = $('#UpdateCourseTypes :selected').text();
        $("#UpdatelblCourseDetails").text(CourseTypesValue);
    }
}

function UpdateInsertSeatAllocationData() {
    var IsValid = true;
    var remarks = $('#UpdateRemarksTxt').val();
    $("#UpdateForwardTo-Required").hide();
    $("#UpdateGM-Add-Required").hide();
    $("#UpdateRemarkstxt-Required").hide();

    if ($("#UpdateGMPerc").val() == "") {
        IsValid = false;
        $("#UpdateGM-Add-Required").show();
    }

    if ($("#UpdateForwardTo").val() == 0) {
        IsValid = false;
        $("#UpdateForwardTo-Required").show();
    }
    if (remarks == " ") {
        IsValid = false;
        $("#UpdateRemarkstxt-Required").show();
    } 

    if (IsValid) {
        bootbox.confirm({
            message: "<br><br>Are you sure, you want to submit the updated Rules of seat allocation?",
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
                UpdateDataForwardingAfterConfirm();
            }
            }
        });

        //bootbox.confirm("<br><br>Are you sure to submit the rule of allocation?", function (confirmed) {
        //    if (confirmed) {
        //        UpdateDataForwardingAfterConfirm();
        //    }
        //});
    }
    else {
        /* bootbox.alert("<br><br>Kindly clear the error in that page");*/
        bootbox.alert("<br> Please select role ");
    }
}

function UpdateDataForwardingAfterConfirm() {
    var seatAllocationObj = {

        Exam_Year: $('#UpdateExamYear :selected').val(),
        CourseId: $('#UpdateCourseTypes :selected').val(),
        Rules_allocation_masterid: $("#SeatAllocationReviewId").val(),
        FlowId: $("#UpdateForwardTo").val(),
        Remarks: $('textarea#UpdateRemarksTxt').val(),

        ConsolidatedVerticalRules: [
            $('#UpdateGMPerc').val(),
            $('#UpdateSCPerc').val(),
            $('#UpdateSTPerc').val(),
            $('#UpdateC1Perc').val(),
            $('#UpdateIIAPerc').val(),
            $("#UpdateIIBPerc").val(),
            $("#UpdateIIIAPerc").val(),
            $("#UpdateIIIBPerc").val()
        ],

        ConsolidatedHorizontalRules: [
            $('#UpdateWomenPerc').val(),
            $('#UpdatePerDisabilitiesperc').val(),
            $('#UpdateExServicePerc').val(),
            $('#UpdateKanMediumPerc').val(),
            $('#UpdateLandLoserPerc').val(),
            $("#UpdateKashmiriMigrPerc").val(),
            $("#UpdateEconomicWeakerPerc").val(),
            $("#UpdateGPPerc").val()
        ],

        ConsolidatedHyderabadRegion: [
            $("#UpdateHyderabadRegion").val(),
            $("#UpdateNonHyderabadRegion").val(),
            $("#UpdateNHyderabadRegion").val(),
            $("#UpdateNNonHyderabadRegion").val()
        ],

        Syllabus_type_id: $('#UpdateSyllabus :selected').val(),
        ConsolidatedGradeRules: [
            $('#UpdateGradespercA1').val(),
            $('#UpdateGradespercA2').val(),
            $('#UpdateGradespercB1').val(),
            $('#UpdateGradespercB2').val(),
            $('#UpdateGradespercC1').val(),
            $("#UpdateGradespercC2").val(),
            $("#UpdateGradespercD").val(),
            $("#UpdateGradespercE1").val(),
            $("#UpdateGradespercE2").val()
        ],

        ConsolidatedOtherRules: [
            $("#UpdateReservationRural").val(),
            $("#UpdateReservationTransgender").val(),
            $("#numberofSeatsEdit").val()
        ]
    };

    alertmsg = "<br><br>Rules of seat allocation Updated for the Session : <b>" + $('#UpdateExamYear :selected').text() + "</b> for the Course Type : <b>" + $('#UpdateCourseTypes :selected').text() + "</b> forwarded to <b>" + $('#UpdateForwardTo :selected').text() + "</b> for Review";

    $.ajax({
        type: "POST",
        url: "/Admission/AllocationDataInsertUpdateRules",
        data: JSON.stringify(seatAllocationObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == "failed") {
                bootbox.alert("<br><br>There is error in your Rules of seat allocation data");
            }
            else {
                bootbox.alert({
                    message: alertmsg,
                    callback: function () { location.reload(true); }
                });
                $("#EditPanel").hide();
                ClearAllFields();
            }
        }
    });
}

function GetSeatAllocationForViewById(Rules_allocation_masterid) {
    ClearAllFields();
    $("#UpdateGridDivId").hide();
    $("#EditPanel").hide();
    $("#ApprovedViewPanel").show();
    $("#UpdatebtnSubmitdivid").hide();
    $("#UpdateExamYear").val(0);
    $("#UpdateCourseTypes").val(0);
    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationById',
        data: { Rules_allocation_masterid: Rules_allocation_masterid },
        success: function (data) {
            var RuleVal = 0;
            var Vrtotal = 0;
            var Hrtotal = 0;
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#ViewGMPerc").val(this.RuleValue);
                        $("#ViewGMPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#ViewSCPerc").val(this.RuleValue);
                        $("#ViewSCPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#ViewSTPerc").val(this.RuleValue);
                        $("#ViewSTPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#ViewC1Perc").val(this.RuleValue);
                        $("#ViewC1Perc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#ViewIIAPerc").val(this.RuleValue);
                        $("#ViewIIAPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#ViewIIBPerc").val(this.RuleValue);
                        $("#ViewIIBPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#ViewIIIAPerc").val(this.RuleValue);
                        $("#ViewIIIAPerc").attr('readonly', true);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#ViewIIIBPerc").val(this.RuleValue);
                        $("#ViewIIIBPerc").attr('readonly', true);
                    }
                    RuleVal = this.RuleValue;
                    Vrtotal += RuleVal;
                    $("#ViewExamYear").val(this.Exam_Year);
                    $("#ViewExamYear").attr('readonly', true);

                    $("#ViewCourseTypes").val(this.CourseId);
                    $("#ViewCourseTypes").attr('readonly', true);

                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);
                });
                $("#ViewTotalPer").val(Vrtotal);
                var HRuleVal = 0;
                var HVrtotal = 0;
                var HHrtotal = 0;
                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#ViewWomenPerc").val(this.RuleValue);
                        $("#ViewWomenPerc").attr('readonly', true);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#ViewPerDisabilitiesperc").val(this.RuleValue);
                        $("#ViewPerDisabilitiesperc").attr('readonly', true);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ViewExServicePerc").val(this.RuleValue);
                        $("#ViewExServicePerc").attr('readonly', true);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#ViewKanMediumPerc").val(this.RuleValue);
                        $("#ViewKanMediumPerc").attr('readonly', true);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#ViewLandLoserPerc").val(this.RuleValue);
                        $("#ViewLandLoserPerc").attr('readonly', true);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#ViewKashmiriMigrPerc").val(this.RuleValue);
                        $("#ViewKashmiriMigrPerc").attr('readonly', true);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#ViewEconomicWeakerPerc").val(this.RuleValue);
                        $("#ViewEconomicWeakerPerc").attr('readonly', true);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#ViewGPPerc").val(this.RuleValue);
                        $("#ViewGPPerc").attr('readonly', true);
                    }
                    HRuleVal = this.RuleValue;
                    HHrtotal += HRuleVal;
                });
                $("#ViewhrPercent").val(HHrtotal);
                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ViewHyderabadRegion").val(this.RuleValue);
                        $("#ViewHyderabadRegion").attr('readonly', true);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ViewNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNonHyderabadRegion").attr('readonly', true);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ViewNHyderabadRegion").val(this.RuleValue);
                        $("#ViewNHyderabadRegion").attr('readonly', true);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ViewNNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNNonHyderabadRegion").attr('readonly', true);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#ViewGradespercA1").val(this.RuleValue);
                        $("#ViewGradespercA1").attr('readonly', true);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#ViewGradespercA2").val(this.RuleValue);
                        $("#ViewGradespercA2").attr('readonly', true);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#ViewGradespercB1").val(this.RuleValue);
                        $("#ViewGradespercB1").attr('readonly', true);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#ViewGradespercB2").val(this.RuleValue);
                        $("#ViewGradespercB2").attr('readonly', true);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#ViewGradespercC1").val(this.RuleValue);
                        $("#ViewGradespercC1").attr('readonly', true);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#ViewGradespercC2").val(this.RuleValue);
                        $("#ViewGradespercC2").attr('readonly', true);
                    }
                    else if (this.GradGrades == "D") {
                        $("#ViewGradespercD").val(this.RuleValue);
                        $("#ViewGradespercD").attr('readonly', true);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#ViewGradespercE1").val(this.RuleValue);
                        $("#ViewGradespercE1").attr('readonly', true);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#ViewGradespercE2").val(this.RuleValue);
                        $("#ViewGradespercE2").attr('readonly', true);
                    }
                    $("#ViewSyllabus").val(this.Syllabus_type_id);
                    $("#ViewSyllabus").attr('readonly', true);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ViewReservationRural").val(this.RuleValue);
                        $("#ViewReservationRural").attr('readonly', true);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ViewReservationTransgender").val(this.RuleValue);
                        $("#ViewReservationTransgender").attr('readonly', true);
                    }
                });
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetCommentDetails(SeatAllocationId) {

    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsRuleofAllocation",
        data: { SeatAllocationId: SeatAllocationId },
        success: function (data) {
            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CommentsCreatedOn', 'title': 'Date', 'className': 'text-left' },
                    { 'data': 'userRole', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'ForwardedTo', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
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



function ModifyExistingDataCB() {
    if ($("#cbxModifyData").prop('checked') == true) {
        GridEdit();
    }
    else {
        ExamYearOnChange();
        GridView();
        $("#btnSubmitdivid").show();
        $("#GridSubmit").css('visibility', 'hidden');
    }
}

function WithExistingData() {
    ClearAllFields();
    GridView();
    ErrorValidationHide();
    ClearFieldsOnListChange();
    $("#existingSeatAllocationData").show();
    $("#hideDataForRDBSelection").hide();
    $('label[for="cbxModifyData"]').show();
}

function WithoutExistingData() {
    ClearAllFields();
    GridEdit();
    ErrorValidationHide();
    ClearFieldsOnListChange();
    $("#existingSeatAllocationData").hide();
    $("#hideDataForRDBSelection").show();
    
    $('label[for="cbxModifyData"]').hide();
}

window.addEventListener("keydown", function (e) {
    if (["Space", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].indexOf(e.code) > -1) {
        e.preventDefault();
    }
}, false);


$("#seatTypeId").change(function () {
    var seattypeId = $('#seatTypeId :selected').val();
    //var seattypeText= $('#seatTypeId :selected').text();   
    $("#spnSeattype").text($('#seatTypeId :selected').text());
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatsBySeatTypeRules",
        data: { 'seattypeId': seattypeId, 'tradeId': 0 },
        contentType: "application/json",
        success: function (data) {
            if (data != null) {
                $("#numberofSeats").val(data[0].Management_seats);
                //$("#numberofSeatMngmt").val(data[0].Management_seats);
            }
            else {
                $("#numberofSeats").val(4);
                //$("#numberofSeatMngmt").val(0);
            }
        },
        error: function () {
            bootbox.alert("failed");
        }
    });
});