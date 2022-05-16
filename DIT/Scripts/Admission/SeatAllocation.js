$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');
});

GetExamType();
GetCourseTypes();
GetSyllabusType();
GetStatusList();
GetSeatAllocationData();

function ExamYearOnChange() {

    ClearAllFields();
    var Exam_Year = $('#ExamYear :selected').val();
    var CourseId = $('#CourseTypes :selected').val();
    
    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationBasedOnExamCourse',
        data: { Exam_Year: Exam_Year, CourseId: CourseId },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#GMPerc").val(this.RuleValue);
                        $("#UpdateGMPerc").val(this.RuleValue);
                        $("#ViewGMPerc").val(this.RuleValue);
                        $("#ReviewGMPerc").text(this.RuleValue);
                        $("#ApproveGMPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#SCPerc").val(this.RuleValue);
                        $("#UpdateSCPerc").val(this.RuleValue);
                        $("#ViewSCPerc").val(this.RuleValue);
                        $("#ReviewSCPerc").text(this.RuleValue);
                        $("#ApproveSCPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#STPerc").val(this.RuleValue);
                        $("#UpdateSTPerc").val(this.RuleValue);
                        $("#ViewSTPerc").val(this.RuleValue);
                        $("#ReviewSTPerc").text(this.RuleValue);
                        $("#ApproveSTPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#C1Perc").val(this.RuleValue);
                        $("#UpdateC1Perc").val(this.RuleValue);
                        $("#ViewC1Perc").val(this.RuleValue);
                        $("#ReviewC1Perc").text(this.RuleValue);
                        $("#ApproveC1Perc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#IIAPerc").val(this.RuleValue);
                        $("#UpdateIIAPerc").val(this.RuleValue);
                        $("#ViewIIAPerc").val(this.RuleValue);
                        $("#ReviewIIAPerc").text(this.RuleValue);
                        $("#ApproveIIAPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#IIBPerc").val(this.RuleValue);
                        $("#UpdateIIBPerc").val(this.RuleValue);
                        $("#ViewIIBPerc").val(this.RuleValue);
                        $("#ReviewIIBPerc").text(this.RuleValue);
                        $("#ApproveIIBPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#IIIAPerc").val(this.RuleValue);
                        $("#UpdateIIIAPerc").val(this.RuleValue);
                        $("#ViewIIIAPerc").val(this.RuleValue);
                        $("#ReviewIIIAPerc").text(this.RuleValue);
                        $("#ApproveIIIAPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#IIIBPerc").val(this.RuleValue);
                        $("#UpdateIIIBPerc").val(this.RuleValue);
                        $("#ViewIIIBPerc").val(this.RuleValue);
                        $("#ReviewIIIBPerc").text(this.RuleValue);
                        $("#ApproveIIIBPerc").text(this.RuleValue);
                    }

                    $("#ReviewVerticalStatus").text(this.StatusName);
                    $("#ReviewVerticalRemarks").text(this.Remarks);

                    $("#ApproveVerticalStatus").text(this.StatusName);
                    $("#ApproveVerticalRemarks").text(this.Remarks);

                    $("#UpdateVerticalStatus").text(this.StatusName);
                    $("#UpdateVerticalRemarks").text(this.Remarks);

                    $("#ExamYear").val(this.Exam_Year);
                    $("#UpdateExamYear").val(this.Exam_Year);
                    $("#ViewExamYear").val(this.Exam_Year);
                    $("#ReviewExamYear").val(this.Exam_Year);
                    $("#ApproveExamYear").val(this.Exam_Year);

                    $("#CourseTypes").val(this.CourseId);
                    $("#UpdateCourseTypes").val(this.CourseId);
                    $("#ViewCourseTypes").val(this.CourseId);
                    $("#ReviewCourseTypes").val(this.CourseId);
                    $("#ApproveCourseTypes").val(this.CourseId);

                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);

                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#WomenPerc").val(this.RuleValue);
                        $("#UpdateWomenPerc").val(this.RuleValue);
                        $("#ViewWomenPerc").val(this.RuleValue);
                        $("#ReviewWomenPerc").text(this.RuleValue);
                        $("#ApproveWomenPerc").text(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#PerDisabilitiesperc").val(this.RuleValue);
                        $("#UpdatePerDisabilitiesperc").val(this.RuleValue);
                        $("#ViewPerDisabilitiesperc").val(this.RuleValue);
                        $("#ReviewPerDisabilitiesperc").text(this.RuleValue);
                        $("#ApprovePerDisabilitiesperc").text(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ExServicePerc").val(this.RuleValue);
                        $("#UpdateExServicePerc").val(this.RuleValue);
                        $("#ViewExServicePerc").val(this.RuleValue);
                        $("#ReviewExServicePerc").text(this.RuleValue);
                        $("#ApproveExServicePerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#KanMediumPerc").val(this.RuleValue);
                        $("#UpdateKanMediumPerc").val(this.RuleValue);
                        $("#ViewKanMediumPerc").val(this.RuleValue);
                        $("#ReviewKanMediumPerc").text(this.RuleValue);
                        $("#ApproveKanMediumPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#LandLoserPerc").val(this.RuleValue);
                        $("#UpdateLandLoserPerc").val(this.RuleValue);
                        $("#ViewLandLoserPerc").val(this.RuleValue);
                        $("#ReviewLandLoserPerc").text(this.RuleValue);
                        $("#ApproveLandLoserPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#KashmiriMigrPerc").val(this.RuleValue);
                        $("#UpdateKashmiriMigrPerc").val(this.RuleValue);
                        $("#ViewKashmiriMigrPerc").val(this.RuleValue);
                        $("#ReviewKashmiriMigrPerc").text(this.RuleValue);
                        $("#ApproveKashmiriMigrPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#EconomicWeakerPerc").val(this.RuleValue);
                        $("#UpdateEconomicWeakerPerc").val(this.RuleValue);
                        $("#ViewEconomicWeakerPerc").val(this.RuleValue);
                        $("#ReviewEconomicWeakerPerc").text(this.RuleValue);
                        $("#ApproveEconomicWeakerPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#GPPerc").val(this.RuleValue);
                        $("#UpdateGPPerc").val(this.RuleValue);
                        $("#ViewGPPerc").val(this.RuleValue);
                        $("#ReviewGPPerc").text(this.RuleValue);
                        $("#ApproveGPPerc").text(this.RuleValue);
                    }
                    $("#ReviewHorizontalStatus").text(this.StatusName);
                    $("#ApproveHorizontalStatus").text(this.StatusName);
                    $("#ReviewHorizontalRemarks").text(this.Remarks);
                    $("#ApproveHorizontalRemarks").text(this.Remarks);

                    $("#UpdateHorizontalStatus").text(this.StatusName);
                    $("#UpdateRemarksStatus").text(this.Remarks);
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#HyderabadRegion").val(this.RuleValue);
                        $("#UpdateHyderabadRegion").val(this.RuleValue);
                        $("#ViewHyderabadRegion").val(this.RuleValue);
                        $("#ReviewHyderabadRegion").text(this.RuleValue);
                        $("#ApproveHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NonHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNonHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNonHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNonHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#NHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNHyderabadRegion").val(this.RuleValue);
                        $("#ViewNHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NNonHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNNonHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNNonHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNNonHyderabadRegion").text(this.RuleValue);
                    }
                    $("#UpdateHyderabadStatus").text(this.StatusName);
                    $("#UpdateHyderabadRemarks").text(this.Remarks);

                    $("#UpdateHyderabadRegionStatus").text(this.StatusName);
                    $("#UpdateHyderabadRegionRemarks").text(this.Remarks);
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#GradespercA1").val(this.RuleValue);
                        $("#ViewGradespercA1").val(this.RuleValue);
                        $("#UpdateGradespercA1").val(this.RuleValue);
                        $("#ReviewGradespercA1").text(this.RuleValue);
                        $("#ApproveGradespercA1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#GradespercA2").val(this.RuleValue);
                        $("#ViewGradespercA2").val(this.RuleValue);
                        $("#UpdateGradespercA2").val(this.RuleValue);
                        $("#ReviewGradespercA2").text(this.RuleValue);
                        $("#ApproveGradespercA2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#GradespercB1").val(this.RuleValue);
                        $("#ViewGradespercB1").val(this.RuleValue);
                        $("#UpdateGradespercB1").val(this.RuleValue);
                        $("#ReviewGradespercB1").text(this.RuleValue);
                        $("#ApproveGradespercB1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#GradespercB2").val(this.RuleValue);
                        $("#ViewGradespercB2").val(this.RuleValue);
                        $("#UpdateGradespercB2").val(this.RuleValue);
                        $("#ReviewGradespercB2").text(this.RuleValue);
                        $("#ApproveGradespercB2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#GradespercC1").val(this.RuleValue);
                        $("#ViewGradespercC1").val(this.RuleValue);
                        $("#UpdateGradespercC1").val(this.RuleValue);
                        $("#ReviewGradespercC1").text(this.RuleValue);
                        $("#ApproveGradespercC1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#GradespercC2").val(this.RuleValue);
                        $("#ViewGradespercC2").val(this.RuleValue);
                        $("#UpdateGradespercC2").val(this.RuleValue);
                        $("#ReviewGradespercC2").text(this.RuleValue);
                        $("#ApproveGradespercC2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#GradespercD").val(this.RuleValue);
                        $("#ViewGradespercD").val(this.RuleValue);
                        $("#UpdateGradespercD").val(this.RuleValue);
                        $("#ReviewGradespercD").text(this.RuleValue);
                        $("#ApproveGradespercD").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#GradespercE1").val(this.RuleValue);
                        $("#ViewGradespercE1").val(this.RuleValue);
                        $("#UpdateGradespercE1").val(this.RuleValue);
                        $("#ReviewGradespercE1").text(this.RuleValue);
                        $("#ApproveGradespercE1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#GradespercE2").val(this.RuleValue);
                        $("#ViewGradespercE2").val(this.RuleValue);
                        $("#UpdateGradespercE2").val(this.RuleValue);
                        $("#ReviewGradespercE2").text(this.RuleValue);
                        $("#ApproveGradespercE2").text(this.RuleValue);
                    }

                    $("#ReviewGradeStatusId").text(this.StatusName);
                    $("#ApproveGradeStatusId").text(this.StatusName);
                    $("#ReviewGradeRemarks").text(this.Remarks);
                    $("#ApproveGradeRemarks").text(this.Remarks);

                    $("#UpdateGradeStatus").text(this.StatusName);
                    $("#UpdateGradeRemarks").text(this.Remarks);

                    $("#Syllabus").val(this.Syllabus_type_id);
                    $("#UpdateSyllabus").val(this.Syllabus_type_id);
                    $("#ViewSyllabus").val(this.Syllabus_type_id);
                    $("#ReviewSyllabus").val(this.Syllabus_type_id);
                    $("#ApproveSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ReservationRural").val(this.RuleValue);
                        $("#ViewReservationRural").val(this.RuleValue);
                        $("#UpateReservationRural").val(this.RuleValue);
                        $("#ReviewReservationRural").text(this.RuleValue);
                        $("#ApproveReservationRural").text(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ReservationTransgender").val(this.RuleValue);
                        $("#ViewReservationTransgender").val(this.RuleValue);
                        $("#UpdateReservationTransgender").val(this.RuleValue);
                        $("#ReviewReservationTransgender").text(this.RuleValue);
                        $("#ApproveReservationTransgender").text(this.RuleValue);
                    }

                    $("#ReviewReservationsStatus").text(this.StatusName);
                    $("#ApproveReservationsStatus").text(this.StatusName);
                    $("#ReviewReservationRemarks").text(this.Remarks);
                    $("#ApproveReservationRemarks").text(this.Remarks);

                    $("#UpdateOtherStatus").text(this.StatusName);
                    $("#UpdateOtherRemarks").text(this.Remarks);
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function InsertSeatAllocationData() {
    var seatAllocationObj = {

        Exam_Year: $('#ExamYear :selected').val(),
        CourseId: $('#CourseTypes :selected').val(),
        Rules_allocation_masterid: $("#SeatAllocationReviewId").val(),

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

        Syllabus_type_id: $('#Syllabus :selected').val(),
        ConsolidatedGradeRules: [
            $('#GradespercA1').val(),
            $('#GradespercA2').val(),
            $('#GradespercB1').val(),
            $('#GradespercB2').val(),
            $('#GradespercC1').val(),
            $("#GradespercC2").val(),
            $("#GradespercD").val(),
            $("#GradespercE1").val(),
            $("#GradespercE2").val()
        ],

        ConsolidatedOtherRules: [
            $("#ReservationRural").val(),
            $("#ReservationTransgender").val()
        ]
    };
    console.log(JSON.stringify(seatAllocationObj));
    $.ajax({
        type: "POST",
        url: "/Admission/InsertSeatAllocationData",
        data: JSON.stringify(seatAllocationObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == "failed")
                alert("There is error in your data");
        }
    });
}

function GetSeatAllocationData() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAllocationData",
        success: function (data) {
            if (data != null || data != '') {
                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#GMPerc").val(this.RuleValue);
                        $("#UpdateGMPerc").val(this.RuleValue);
                        $("#ViewGMPerc").val(this.RuleValue);
                        $("#ReviewGMPerc").text(this.RuleValue);
                        $("#ApproveGMPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#SCPerc").val(this.RuleValue);
                        $("#UpdateSCPerc").val(this.RuleValue);
                        $("#ViewSCPerc").val(this.RuleValue);
                        $("#ReviewSCPerc").text(this.RuleValue);
                        $("#ApproveSCPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#STPerc").val(this.RuleValue);
                        $("#UpdateSTPerc").val(this.RuleValue);
                        $("#ViewSTPerc").val(this.RuleValue);
                        $("#ReviewSTPerc").text(this.RuleValue);
                        $("#ApproveSTPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#C1Perc").val(this.RuleValue);
                        $("#UpdateC1Perc").val(this.RuleValue);
                        $("#ViewC1Perc").val(this.RuleValue);
                        $("#ReviewC1Perc").text(this.RuleValue);
                        $("#ApproveC1Perc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#IIAPerc").val(this.RuleValue);
                        $("#UpdateIIAPerc").val(this.RuleValue);
                        $("#ViewIIAPerc").val(this.RuleValue);
                        $("#ReviewIIAPerc").text(this.RuleValue);
                        $("#ApproveIIAPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#IIBPerc").val(this.RuleValue);
                        $("#UpdateIIBPerc").val(this.RuleValue);
                        $("#ViewIIBPerc").val(this.RuleValue);
                        $("#ReviewIIBPerc").text(this.RuleValue);
                        $("#ApproveIIBPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#IIIAPerc").val(this.RuleValue);
                        $("#UpdateIIIAPerc").val(this.RuleValue);
                        $("#ViewIIIAPerc").val(this.RuleValue);
                        $("#ReviewIIIAPerc").text(this.RuleValue);
                        $("#ApproveIIIAPerc").text(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#IIIBPerc").val(this.RuleValue);
                        $("#UpdateIIIBPerc").val(this.RuleValue);
                        $("#ViewIIIBPerc").val(this.RuleValue);
                        $("#ReviewIIIBPerc").text(this.RuleValue);
                        $("#ApproveIIIBPerc").text(this.RuleValue);
                    }

                    $("#ReviewVerticalStatus").text(this.StatusName);
                    $("#ApproveVerticalStatus").text(this.StatusName);
                    $("#ReviewVerticalRemarks").text(this.Remarks);
                    $("#ApproveVerticalRemarks").text(this.Remarks);

                    $("#UpdateVerticalStatus").text(this.StatusName);
                    $("#UpdateVerticalRemarks").text(this.Remarks);

                    $("#ExamYear").val(this.Exam_Year);
                    $("#UpdateExamYear").val(this.Exam_Year);
                    $("#ViewExamYear").val(this.Exam_Year);
                    $("#ReviewExamYear").val(this.Exam_Year);

                    $("#CourseTypes").val(this.CourseId);
                    $("#UpdateCourseTypes").val(this.CourseId);
                    $("#ViewCourseTypes").val(this.CourseId);
                    $("#ReviewCourseTypes").val(this.CourseId);
                    $("#ApproveCourseTypes").val(this.CourseId);

                    $("#SeatAllocationReviewId").val(this.Rules_allocation_masterid);

                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#WomenPerc").val(this.RuleValue);
                        $("#UpdateWomenPerc").val(this.RuleValue);
                        $("#ViewWomenPerc").val(this.RuleValue);
                        $("#ReviewWomenPerc").text(this.RuleValue);
                        $("#ApproveWomenPerc").text(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#PerDisabilitiesperc").val(this.RuleValue);
                        $("#UpdatePerDisabilitiesperc").val(this.RuleValue);
                        $("#ViewPerDisabilitiesperc").val(this.RuleValue);
                        $("#ReviewPerDisabilitiesperc").text(this.RuleValue);
                        $("#ApprovePerDisabilitiesperc").text(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ExServicePerc").val(this.RuleValue);
                        $("#UpdateExServicePerc").val(this.RuleValue);
                        $("#ViewExServicePerc").val(this.RuleValue);
                        $("#ReviewExServicePerc").text(this.RuleValue);
                        $("#ApproveExServicePerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#KanMediumPerc").val(this.RuleValue);
                        $("#UpdateKanMediumPerc").val(this.RuleValue);
                        $("#ViewKanMediumPerc").val(this.RuleValue);
                        $("#ReviewKanMediumPerc").text(this.RuleValue);
                        $("#ApproveKanMediumPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#LandLoserPerc").val(this.RuleValue);
                        $("#UpdateLandLoserPerc").val(this.RuleValue);
                        $("#ViewLandLoserPerc").val(this.RuleValue);
                        $("#ReviewLandLoserPerc").text(this.RuleValue);
                        $("#ApproveLandLoserPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#KashmiriMigrPerc").val(this.RuleValue);
                        $("#UpdateKashmiriMigrPerc").val(this.RuleValue);
                        $("#ViewKashmiriMigrPerc").val(this.RuleValue);
                        $("#ReviewKashmiriMigrPerc").text(this.RuleValue);
                        $("#ApproveKashmiriMigrPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#EconomicWeakerPerc").val(this.RuleValue);
                        $("#UpdateEconomicWeakerPerc").val(this.RuleValue);
                        $("#ViewEconomicWeakerPerc").val(this.RuleValue);
                        $("#ReviewEconomicWeakerPerc").text(this.RuleValue);
                        $("#ApproveEconomicWeakerPerc").text(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#GPPerc").val(this.RuleValue);
                        $("#UpdateGPPerc").val(this.RuleValue);
                        $("#ViewGPPerc").val(this.RuleValue);
                        $("#ReviewGPPerc").text(this.RuleValue);
                        $("#ApproveGPPerc").text(this.RuleValue);
                    }
                    $("#ReviewHorizontalStatus").text(this.StatusName);
                    $("#ApproveHorizontalStatus").text(this.StatusName);
                    $("#ReviewHorizontalRemarks").text(this.Remarks);
                    $("#ApproveHorizontalRemarks").text(this.Remarks);

                    $("#UpdateHorizontalStatus").text(this.StatusName);
                    $("#UpdateRemarksStatus").text(this.Remarks);
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#HyderabadRegion").val(this.RuleValue);
                        $("#UpdateHyderabadRegion").val(this.RuleValue);
                        $("#ViewHyderabadRegion").val(this.RuleValue);
                        $("#ReviewHyderabadRegion").text(this.RuleValue);
                        $("#ApproveHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NonHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNonHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNonHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNonHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#NHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNHyderabadRegion").val(this.RuleValue);
                        $("#ViewNHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNHyderabadRegion").text(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#NNonHyderabadRegion").val(this.RuleValue);
                        $("#UpdateNNonHyderabadRegion").val(this.RuleValue);
                        $("#ViewNNonHyderabadRegion").val(this.RuleValue);
                        $("#ReviewNNonHyderabadRegion").text(this.RuleValue);
                        $("#ApproveNNonHyderabadRegion").text(this.RuleValue);
                    }
                    $("#UpdateHyderabadStatus").text(this.StatusName);
                    $("#UpdateHyderabadRemarks").text(this.Remarks);

                    $("#UpdateHyderabadRegionStatus").text(this.StatusName);
                    $("#UpdateHyderabadRegionRemarks").text(this.Remarks);
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#GradespercA1").val(this.RuleValue);
                        $("#ViewGradespercA1").val(this.RuleValue);
                        $("#UpdateGradespercA1").val(this.RuleValue);
                        $("#ReviewGradespercA1").text(this.RuleValue);
                        $("#ApproveGradespercA1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#GradespercA2").val(this.RuleValue);
                        $("#ViewGradespercA2").val(this.RuleValue);
                        $("#UpdateGradespercA2").val(this.RuleValue);
                        $("#ReviewGradespercA2").text(this.RuleValue);
                        $("#ApproveGradespercA2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#GradespercB1").val(this.RuleValue);
                        $("#ViewGradespercB1").val(this.RuleValue);
                        $("#UpdateGradespercB1").val(this.RuleValue);
                        $("#ReviewGradespercB1").text(this.RuleValue);
                        $("#ApproveGradespercB1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#GradespercB2").val(this.RuleValue);
                        $("#ViewGradespercB2").val(this.RuleValue);
                        $("#UpdateGradespercB2").val(this.RuleValue);
                        $("#ReviewGradespercB2").text(this.RuleValue);
                        $("#ApproveGradespercB2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#GradespercC1").val(this.RuleValue);
                        $("#ViewGradespercC1").val(this.RuleValue);
                        $("#UpdateGradespercC1").val(this.RuleValue);
                        $("#ReviewGradespercC1").text(this.RuleValue);
                        $("#ApproveGradespercC1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#GradespercC2").val(this.RuleValue);
                        $("#ViewGradespercC2").val(this.RuleValue);
                        $("#UpdateGradespercC2").val(this.RuleValue);
                        $("#ReviewGradespercC2").text(this.RuleValue);
                        $("#ApproveGradespercC2").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#GradespercD").val(this.RuleValue);
                        $("#ViewGradespercD").val(this.RuleValue);
                        $("#UpdateGradespercD").val(this.RuleValue);
                        $("#ReviewGradespercD").text(this.RuleValue);
                        $("#ApproveGradespercD").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#GradespercE1").val(this.RuleValue);
                        $("#ViewGradespercE1").val(this.RuleValue);
                        $("#UpdateGradespercE1").val(this.RuleValue);
                        $("#ReviewGradespercE1").text(this.RuleValue);
                        $("#ApproveGradespercE1").text(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#GradespercE2").val(this.RuleValue);
                        $("#ViewGradespercE2").val(this.RuleValue);
                        $("#UpdateGradespercE2").val(this.RuleValue);
                        $("#ReviewGradespercE2").text(this.RuleValue);
                        $("#ApproveGradespercE2").text(this.RuleValue);
                    }

                    $("#ReviewGradeStatusId").text(this.StatusName);
                    $("#ApproveGradeStatusId").text(this.StatusName);
                    $("#ReviewGradeRemarks").text(this.Remarks);
                    $("#ApproveGradeRemarks").text(this.Remarks);

                    $("#UpdateGradeStatus").text(this.StatusName);
                    $("#UpdateGradeRemarks").text(this.Remarks);

                    $("#Syllabus").val(this.Syllabus_type_id);
                    $("#UpdateSyllabus").val(this.Syllabus_type_id);
                    $("#ViewSyllabus").val(this.Syllabus_type_id);
                    $("#ReviewSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ReservationRural").val(this.RuleValue);
                        $("#ViewReservationRural").val(this.RuleValue);
                        $("#UpateReservationRural").val(this.RuleValue);
                        $("#ReviewReservationRural").text(this.RuleValue);
                        $("#ApproveReservationRural").text(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ReservationTransgender").val(this.RuleValue);
                        $("#ViewReservationTransgender").val(this.RuleValue);
                        $("#UpdateReservationTransgender").val(this.RuleValue);
                        $("#ApproveReservationTransgender").text(this.RuleValue);
                    }

                    $("#ReviewReservationsStatus").text(this.StatusName);
                    $("#ApproveReservationsStatus").text(this.StatusName);
                    $("#ReviewReservationRemarks").text(this.Remarks);
                    $("#ApproveReservationRemarks").text(this.Remarks);

                    $("#UpdateOtherStatus").text(this.StatusName);
                    $("#UpdateOtherRemarks").text(this.Remarks);
                });
            }
        }
    });
}

function GetExamType() {
    $("#ExamYear").empty();
    $("#ExamYear").append('<option value="0">choose</option>');

    $("#ReviewExamYear").empty();
    $("#ReviewExamYear").append('<option value="0">choose</option>');

    $("#ViewExamYear").empty();
    $("#ViewExamYear").append('<option value="0">choose</option>');

    $("#ReviewExamYear").empty();
    $("#ReviewExamYear").append('<option value="0">choose</option>');

    $("#ApproveExamYear").empty();
    $("#ApproveExamYear").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetExamYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ReviewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ViewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#UpdateExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ApproveExamYear").append($("<option/>").val(this.YearID).text(this.Year));
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

    $("#ReviewCourseTypes").empty();
    $("#ReviewCourseTypes").append('<option value="0">choose</option>');

    $("#ApproveCourseTypes").empty();
    $("#ApproveCourseTypes").append('<option value="0">choose</option>');

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
                    $("#ReviewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ApproveCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
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

    $("#ReviewSyllabus").empty();
    $("#ReviewSyllabus").append('<option value="0">choose</option>');

    $("#ApproveSyllabus").empty();
    $("#ApproveSyllabus").append('<option value="0">choose</option>');

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
                    $("#ReviewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                    $("#ApproveSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetStatusList() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetStatusList',
        success: function (data) {
            $("#StatusId").empty();
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#StatusId").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function getDataVertical() {
    var seatAllocationPopUp = "Vertical";
    $.ajax({
        type: 'GET',
        url: '/Admission/getDataseatallocationById',
        data: { seatAllocationPopUp: seatAllocationPopUp },
        success: function (result) {

            $("#StatusId").val(result[0].StatusId);
            $("#RemarksTxt").val(result[0].Remarks);
            $('#SeatAllocationPartId').val("Vertical");
            $('#myModal').modal('show');

        }, error: function (errormessage) {
            alert("Error", "something went wrong");
        }
    });
}

function getDataHorizontal() {

    var seatAllocationPopUp = "Horizontal";
    $.ajax({
        type: 'GET',
        url: '/Admission/getDataseatallocationById',
        data: { seatAllocationPopUp: seatAllocationPopUp },
        success: function (result) {

            $("#StatusId").val(result[0].StatusId);
            $("#RemarksTxt").val(result[0].Remarks);
            $('#SeatAllocationPartId').val("Vertical");
            $('#myModal').modal('show');

        }, error: function (errormessage) {
            alert("Error", "something went wrong");
        }
    });
}

function getDataRegion() {

    var seatAllocationPopUp = "Region";
    $.ajax({
        type: 'GET',
        url: '/Admission/getDataseatallocationById',
        data: { seatAllocationPopUp: seatAllocationPopUp },
        success: function (result) {

            $("#StatusId").val(result[0].StatusId);
            $("#RemarksTxt").val(result[0].Remarks);
            $('#SeatAllocationPartId').val("Vertical");
            $('#myModal').modal('show');

        }, error: function (errormessage) {
            alert("Error", "something went wrong");
        }
    });
}

function getDataGrades() {

    var seatAllocationPopUp = "Grades";
    $.ajax({
        type: 'GET',
        url: '/Admission/getDataseatallocationById',
        data: { seatAllocationPopUp: seatAllocationPopUp },
        success: function (result) {

            $("#StatusId").val(result[0].StatusId);
            $("#RemarksTxt").val(result[0].Remarks);
            $('#SeatAllocationPartId').val("Vertical");
            $('#myModal').modal('show');

        }, error: function (errormessage) {
            alert("Error", "something went wrong");
        }
    });
}

function getDataOtherReservation() {
    var seatAllocationPopUp = "Others";
    $.ajax({
        type: 'GET',
        url: '/Admission/getDataseatallocationById',
        data: { seatAllocationPopUp: seatAllocationPopUp },
        success: function (result) {

            $("#StatusId").val(result[0].StatusId);
            $("#RemarksTxt").val(result[0].Remarks);
            $('#SeatAllocationPartId').val("Vertical");
            $('#myModal').modal('show');

        }, error: function (errormessage) {
            alert("Error", "something went wrong");
        }
    });
}

function UpdateRemarksDetailsForSeat() {
    var seatObj = {
        StatusId: $('#StatusId').val(),
        Remarks: $('#RemarksTxt').val(),
        SeatAllocationPartId: $('#SeatAllocationPartId').val()
    };
    $.ajax({
        type: "POST",
        url: "/Admission/UpdateRemarksForSeats",
        data: JSON.stringify(seatObj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('.modal-backdrop').hide();
            GetSeatAllocationData();
        }
    });
}

function ClearAllFields() {
    $("#GMPerc").val("");
    $("#UpdateGMPerc").val("");
    $("#ViewGMPerc").val("");
    $("#ReviewGMPerc").text("");
    $("#ApproveGMPerc").text("");
    $("#SCPerc").val("");
    $("#UpdateSCPerc").val("");
    $("#ViewSCPerc").val("");
    $("#ReviewSCPerc").text("");
    $("#ApproveSCPerc").text("");
    $("#STPerc").val("");
    $("#UpdateSTPerc").val("");
    $("#ViewSTPerc").val("");
    $("#ReviewSTPerc").text("");
    $("#ApproveSTPerc").text("");
    $("#C1Perc").val("");
    $("#UpdateC1Perc").val("");
    $("#ViewC1Perc").val("");
    $("#ReviewC1Perc").text("");
    $("#ApproveC1Perc").text("");
    $("#IIAPerc").val("");
    $("#UpdateIIAPerc").val("");
    $("#ViewIIAPerc").val("");
    $("#ReviewIIAPerc").text("");
    $("#ApproveIIAPerc").text("");
    $("#IIBPerc").val("");
    $("#UpdateIIBPerc").val("");
    $("#ViewIIBPerc").val("");
    $("#ReviewIIBPerc").text("");
    $("#ApproveIIBPerc").text("");
    $("#IIIAPerc").val("");
    $("#UpdateIIIAPerc").val("");
    $("#ViewIIIAPerc").val("");
    $("#ReviewIIIAPerc").text("");
    $("#ApproveIIIAPerc").text("");
    $("#IIIBPerc").val("");
    $("#UpdateIIIBPerc").val("");
    $("#ViewIIIBPerc").val("");
    $("#ReviewIIIBPerc").text("");
    $("#ApproveIIIBPerc").text("");
    $("#ReviewVerticalStatus").text("");
    $("#ApproveVerticalStatus").text("");
    $("#ReviewVerticalRemarks").text("");
    $("#ApproveVerticalRemarks").text("");
    $("#UpdateVerticalStatus").text("");
    $("#UpdateVerticalRemarks").text("");    
    $("#SeatAllocationReviewId").val("");
    $("#WomenPerc").val("");
    $("#UpdateWomenPerc").val("");
    $("#ViewWomenPerc").val("");
    $("#ReviewWomenPerc").text("");
    $("#ApproveWomenPerc").text("");
    $("#PerDisabilitiesperc").val("");
    $("#UpdatePerDisabilitiesperc").val("");
    $("#ViewPerDisabilitiesperc").val("");
    $("#ReviewPerDisabilitiesperc").text("");
    $("#ApprovePerDisabilitiesperc").text("");
    $("#ExServicePerc").val("");
    $("#UpdateExServicePerc").val("");
    $("#ViewExServicePerc").val("");
    $("#ReviewExServicePerc").text("");
    $("#ApproveExServicePerc").text("");
    $("#KanMediumPerc").val("");
    $("#UpdateKanMediumPerc").val("");
    $("#ViewKanMediumPerc").val("");
    $("#ReviewKanMediumPerc").text("");
    $("#ApproveKanMediumPerc").text("");
    $("#LandLoserPerc").val("");
    $("#UpdateLandLoserPerc").val("");
    $("#ViewLandLoserPerc").val("");
    $("#ReviewLandLoserPerc").text("");
    $("#ApproveLandLoserPerc").text("");
    $("#KashmiriMigrPerc").val("");
    $("#UpdateKashmiriMigrPerc").val("");
    $("#ViewKashmiriMigrPerc").val("");
    $("#ReviewKashmiriMigrPerc").text("");
    $("#ApproveKashmiriMigrPerc").text("");
    $("#EconomicWeakerPerc").val("");
    $("#UpdateEconomicWeakerPerc").val("");
    $("#ViewEconomicWeakerPerc").val("");
    $("#ReviewEconomicWeakerPerc").text("");
    $("#ApproveEconomicWeakerPerc").text("");
    $("#GPPerc").val("");
    $("#UpdateGPPerc").val("");
    $("#ViewGPPerc").val("");
    $("#ReviewGPPerc").text("");
    $("#ApproveGPPerc").text("");
    $("#ReviewHorizontalStatus").text("");
    $("#ApproveHorizontalStatus").text("");
    $("#ReviewHorizontalRemarks").text("");
    $("#ApproveHorizontalRemarks").text("");
    $("#UpdateHorizontalStatus").text("");
    $("#UpdateRemarksStatus").text("");
    $("#HyderabadRegion").val("");
    $("#UpdateHyderabadRegion").val("");
    $("#ViewHyderabadRegion").val("");
    $("#ReviewHyderabadRegion").text("");
    $("#ApproveHyderabadRegion").text("");
    $("#NonHyderabadRegion").val("");
    $("#UpdateNonHyderabadRegion").val("");
    $("#ViewNonHyderabadRegion").val("");
    $("#ReviewNonHyderabadRegion").text("");
    $("#ApproveNonHyderabadRegion").text("");
    $("#NHyderabadRegion").val("");
    $("#UpdateNHyderabadRegion").val("");
    $("#ViewNHyderabadRegion").val("");
    $("#ReviewNHyderabadRegion").text("");
    $("#ApproveNHyderabadRegion").text("");
    $("#NNonHyderabadRegion").val("");
    $("#UpdateNNonHyderabadRegion").val("");
    $("#ViewNNonHyderabadRegion").val("");
    $("#ReviewNNonHyderabadRegion").text("");
    $("#ApproveNNonHyderabadRegion").text("");
    $("#UpdateHyderabadStatus").text("");
    $("#UpdateHyderabadRemarks").text("");
    $("#UpdateHyderabadRegionStatus").text("");
    $("#UpdateHyderabadRegionRemarks").text("");
    $("#GradespercA1").val("");
    $("#ViewGradespercA1").val("");
    $("#UpdateGradespercA1").val("");
    $("#ReviewGradespercA1").text("");
    $("#ApproveGradespercA1").text("");
    $("#GradespercA2").val("");
    $("#ViewGradespercA2").val("");
    $("#UpdateGradespercA2").val("");
    $("#ReviewGradespercA2").text("");
    $("#ApproveGradespercA2").text("");
    $("#GradespercB1").val("");
    $("#ViewGradespercB1").val("");
    $("#UpdateGradespercB1").val("");
    $("#ReviewGradespercB1").text("");
    $("#ApproveGradespercB1").text("");
    $("#GradespercB2").val("");
    $("#ViewGradespercB2").val("");
    $("#UpdateGradespercB2").val("");
    $("#ReviewGradespercB2").text("");
    $("#ApproveGradespercB2").text("");
    $("#GradespercC1").val("");
    $("#ViewGradespercC1").val("");
    $("#UpdateGradespercC1").val("");
    $("#ReviewGradespercC1").text("");
    $("#ApproveGradespercC1").text("");
    $("#GradespercC2").val("");
    $("#ViewGradespercC2").val("");
    $("#UpdateGradespercC2").val("");
    $("#ReviewGradespercC2").text("");
    $("#ApproveGradespercC2").text("");
    $("#GradespercD").val("");
    $("#ViewGradespercD").val("");
    $("#UpdateGradespercD").val("");
    $("#ReviewGradespercD").text("");
    $("#ApproveGradespercD").text("");
    $("#GradespercE1").val("");
    $("#ViewGradespercE1").val("");
    $("#UpdateGradespercE1").val("");
    $("#ReviewGradespercE1").text("");
    $("#ApproveGradespercE1").text("");
    $("#GradespercE2").val("");
    $("#ViewGradespercE2").val("");
    $("#UpdateGradespercE2").val("");
    $("#ReviewGradespercE2").text("");
    $("#ApproveGradespercE2").text("");
    $("#ReviewGradeStatusId").text("");
    $("#ApproveGradeStatusId").text("");
    $("#ReviewGradeRemarks").text("");
    $("#ApproveGradeRemarks").text("");
    $("#UpdateGradeStatus").text("");
    $("#UpdateGradeRemarks").text("");
    $("#Syllabus").val("");
    $("#UpdateSyllabus").val("");
    $("#ViewSyllabus").val("");
    $("#ReviewSyllabus").val("");
    $("#ApproveSyllabus").val("");
    $("#ReservationRural").val("");
    $("#ViewReservationRural").val("");
    $("#UpateReservationRural").val("");
    $("#ReviewReservationRural").text("");
    $("#ApproveReservationRural").text("");
    $("#ReservationTransgender").val("");
    $("#ViewReservationTransgender").val("");
    $("#UpdateReservationTransgender").val("");
    $("#ReviewReservationTransgender").text("");
    $("#ApproveReservationTransgender").text("");
    $("#ReviewReservationsStatus").text("");
    $("#ApproveReservationsStatus").text("");
    $("#ReviewReservationRemarks").text("");
    $("#ApproveReservationRemarks").text("");
    $("#UpdateOtherStatus").text("");
    $("#UpdateOtherRemarks").text("");
}