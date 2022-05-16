$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');

    $("#EditPanel").hide();
    $("#ApprovedViewPanel").hide();
    //$("#btn1").click(function () {
    //    $("#myModal").toggle();
    //    $("#myModal").show();
    //    $("#ApprovedViewPanel").hide();
    //});

    GetRoleList();
    GetAllocationSeatofRuletoUpdate();
    GetApprovedAllocationSeatofRuletoView();
    GetExamType();
    GetCourseTypes();
    GetSyllabusType();
    GetStatusList();
    OnchangeStatus();
});


function GetExamType() {
    $("#ReviewExamYear").empty();
    $("#ViewExamYear").empty();
    $("#ReviewExamYear").append('<option value="0">choose</option>');
    $("#ViewExamYear").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetExamYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ReviewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ViewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetCourseTypes() {

    $("#ReviewCourseTypes").empty();
    $("#ViewCourseTypes").empty();
    $("#ReviewCourseTypes").append('<option value="0">choose</option>');
    $("#ViewCourseTypes").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ReviewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ViewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetSyllabusType() {
    $("#ReviewSyllabus").empty();
    $("#ViewSyllabus").empty();
    $("#ReviewSyllabus").append('<option value="0">choose</option>');
    $("#ViewSyllabus").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetSyllabusType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ReviewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                    $("#ViewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });
            }

        }, error: function (result) {
           // alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetStatusList() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetStatusList',
        success: function (data) {
            $("#StatusId").empty();
            $("#StatusId").append('<option value="0">choose</option>');
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#StatusId").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetRoleList() {

    $("#ForwardTo").empty();
    $("#ForwardTo").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetRoleList",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (this.RoleID == 1 || this.RoleID == 2 || this.RoleID == 5 || this.RoleID == 6)
                        $("#ForwardTo").append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
           // alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function CancelDetailsApprovedView() {
    $("#ApprovedViewPanel").hide();
}

$('a[href="#tab_3"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

$('a[href="#tab_4"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

function CancelDetails() {
    $("#EditPanel").hide();
}

function GetSeatAllocationById(Rules_allocation_masterid) {
    $("#EditPanel").show();
    $("#ApprovedViewPanel").hide();
    $("#SeatAllocationReviewId").val(Rules_allocation_masterid);

    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationById',
        data: { Rules_allocation_masterid: Rules_allocation_masterid },
        success: function (data) {
            if (data != null || data != '') {
                var sessionValue = $("#hdnSession").data('value');

                $.each(data.list7, function () {
                    $("#ReviewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                });

                $.each(data.list8, function () {
                    $("#ReviewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });

                $.each(data.list9, function () {
                    $("#ReviewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });

                $.each(data.list1, function () {
                    if (this.Vertical_Rules == "GM") {
                        $("#ReviewGMPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "SC") {
                        $("#ReviewSCPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "ST") {
                        $("#ReviewSTPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "C1") {
                        $("#ReviewC1Perc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIA") {
                        $("#ReviewIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIB") {
                        $("#ReviewIIBPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIA") {
                        $("#ReviewIIIAPerc").val(this.RuleValue);
                    }
                    else if (this.Vertical_Rules == "IIIB") {
                        $("#ReviewIIIBPerc").val(this.RuleValue);
                    }

                    $("#ReviewExamYear").val(this.Exam_Year);
                    $("#ReviewCourseTypes").val(this.CourseId);
                    $("#ReviewSyllabus").val(this.Rules_allocation_masterid);
                });

                $.each(data.list2, function () {
                    if (this.Horizontal_rules == "Women") {
                        $("#ReviewWomenPerc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Persons with disabilities") {
                        $("#ReviewPerDisabilitiesperc").val(this.RuleValue);
                    }

                    else if (this.Horizontal_rules == "Ex-Service") {
                        $("#ReviewExServicePerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kannada Medium") {
                        $("#ReviewKanMediumPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Land loser") {
                        $("#ReviewLandLoserPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Kashmiri Migrants") {
                        $("#ReviewKashmiriMigrPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "Economic weaker section") {
                        $("#ReviewEconomicWeakerPerc").val(this.RuleValue);
                    }
                    else if (this.Horizontal_rules == "General Pool") {
                        $("#ReviewGPPerc").val(this.RuleValue);
                    }
                });

                $.each(data.list3, function () {
                    if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ReviewHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ReviewNonHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                        $("#ReviewNHyderabadRegion").val(this.RuleValue);
                    }
                    else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                        $("#ReviewNNonHyderabadRegion").val(this.RuleValue);
                    }
                });

                $.each(data.list4, function () {
                    if (this.GradGrades == "A1") {
                        $("#ReviewGradespercA1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "A2") {
                        $("#ReviewGradespercA2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B1") {
                        $("#ReviewGradespercB1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "B2") {
                        $("#ReviewGradespercB2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C1") {
                        $("#ReviewGradespercC1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "C2") {
                        $("#ReviewGradespercC2").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "D") {
                        $("#ReviewGradespercD").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E1") {
                        $("#ReviewGradespercE1").val(this.RuleValue);
                    }
                    else if (this.GradGrades == "E2") {
                        $("#ReviewGradespercE2").val(this.RuleValue);
                    }
                    $("#ReviewSyllabus").val(this.Syllabus_type_id);
                });

                $.each(data.list5, function () {
                    if (this.OtherRules == "Rural") {
                        $("#ReviewReservationRural").val(this.RuleValue);
                    }
                    else if (this.OtherRules == "Transgender") {
                        $("#ReviewReservationTransgender").val(this.RuleValue);
                    }
                });

                $.each(data.list6, function () {

                    $("#RemarksTxt").val();
                    $("#StatusId").val(0);
                    OnchangeStatus();

                    if (sessionValue == this.FlowId)
                        $("#submitbtn").show();
                    else
                        $("#submitbtn").hide();
                });

                $("#EditPanel").show();
            }
        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
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

$("body").on("click", "#SeatRulesValues .RateAnalysis", function () {

    var row = $(this).closest("tr");
    var xid = row.find("td:nth-child(8)");
    SeatAllocationId = xid.text().trim();
    $("#SeatAllocationReviewId").val(SeatAllocationId);

    $("#ApprovedViewPanel").hide();
    $.ajax({
        type: "Get",
        url: "/Admission/GetSeatAllocationById",
        dataType: 'json',
        data: {
            Rules_allocation_masterid: SeatAllocationId
        },
        success: function (data) {

            var sessionValue = $("#hdnSession").data('value');

            $.each(data.list7, function () {
                $("#ReviewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
            });

            $.each(data.list8, function () {
                $("#ReviewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
            });

            $.each(data.list9, function () {
                $("#ReviewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
            });

            $.each(data.list1, function () {
                if (this.Vertical_Rules == "GM") {
                    $("#ReviewGMPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "SC") {
                    $("#ReviewSCPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "ST") {
                    $("#ReviewSTPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "C1") {
                    $("#ReviewC1Perc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "IIA") {
                    $("#ReviewIIAPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "IIB") {
                    $("#ReviewIIBPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "IIIA") {
                    $("#ReviewIIIAPerc").val(this.RuleValue);
                }
                else if (this.Vertical_Rules == "IIIB") {
                    $("#ReviewIIIBPerc").val(this.RuleValue);
                }

                $("#ReviewExamYear").val(this.Exam_Year);
                $("#ReviewCourseTypes").val(this.CourseId);
                $("#ReviewSyllabus").val(this.Rules_allocation_masterid);
            });

            $.each(data.list2, function () {
                if (this.Horizontal_rules == "Women") {
                    $("#ReviewWomenPerc").val(this.RuleValue);
                }

                else if (this.Horizontal_rules == "Persons with disabilities") {
                    $("#ReviewPerDisabilitiesperc").val(this.RuleValue);
                }

                else if (this.Horizontal_rules == "Ex-Service") {
                    $("#ReviewExServicePerc").val(this.RuleValue);
                }
                else if (this.Horizontal_rules == "Kannada Medium") {
                    $("#ReviewKanMediumPerc").val(this.RuleValue);
                }
                else if (this.Horizontal_rules == "Land loser") {
                    $("#ReviewLandLoserPerc").val(this.RuleValue);
                }
                else if (this.Horizontal_rules == "Kashmiri Migrants") {
                    $("#ReviewKashmiriMigrPerc").val(this.RuleValue);
                }
                else if (this.Horizontal_rules == "Economic weaker section") {
                    $("#ReviewEconomicWeakerPerc").val(this.RuleValue);
                }
                else if (this.Horizontal_rules == "General Pool") {
                    $("#ReviewGPPerc").val(this.RuleValue);
                }
            });

            $.each(data.list3, function () {
                if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                    $("#ReviewHyderabadRegion").val(this.RuleValue);
                }
                else if (this.Region_type == "HyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                    $("#ReviewNonHyderabadRegion").val(this.RuleValue);
                }
                else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "HyderabadCandidates") {
                    $("#ReviewNHyderabadRegion").val(this.RuleValue);
                }
                else if (this.Region_type == "NonHyderabadRegion" && this.Candidates_type == "NonHyderabadCandidates") {
                    $("#ReviewNNonHyderabadRegion").val(this.RuleValue);
                }
            });

            $.each(data.list4, function () {
                if (this.GradGrades == "A1") {
                    $("#ReviewGradespercA1").val(this.RuleValue);
                }
                else if (this.GradGrades == "A2") {
                    $("#ReviewGradespercA2").val(this.RuleValue);
                }
                else if (this.GradGrades == "B1") {
                    $("#ReviewGradespercB1").val(this.RuleValue);
                }
                else if (this.GradGrades == "B2") {
                    $("#ReviewGradespercB2").val(this.RuleValue);
                }
                else if (this.GradGrades == "C1") {
                    $("#ReviewGradespercC1").val(this.RuleValue);
                }
                else if (this.GradGrades == "C2") {
                    $("#ReviewGradespercC2").val(this.RuleValue);
                }
                else if (this.GradGrades == "D") {
                    $("#ReviewGradespercD").val(this.RuleValue);
                }
                else if (this.GradGrades == "E1") {
                    $("#ReviewGradespercE1").val(this.RuleValue);
                }
                else if (this.GradGrades == "E2") {
                    $("#ReviewGradespercE2").val(this.RuleValue);
                }
                $("#ReviewSyllabus").val(this.Syllabus_type_id);
            });

            $.each(data.list5, function () {
                if (this.OtherRules == "Rural") {
                    $("#ReviewReservationRural").val(this.RuleValue);
                }
                else if (this.OtherRules == "Transgender") {
                    $("#ReviewReservationTransgender").val(this.RuleValue);
                }
            });

            $.each(data.list6, function () {

                $("#RemarksTxt").val(this.Remarks);
                $("#StatusId").val(this.Status_Id);
                if (this.Status_Id != 4 && this.Status_Id != 7) {
                    $("#StatusId").val(0);
                }
                OnchangeStatus();
           
                if (sessionValue == this.FlowId)
                    $("#submitbtn").show();
                else
                    $("#submitbtn").hide();
            });



            $("#EditPanel").show();
        }, error: function (e) {
            var _msg = "Something went wrong.";
            //alert(_msg);
            //$("#preloder, .loader").hide();
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    })
});

function OnchangeStatus() {

        $("#EnableForwardTo").hide();
        if ($("#StatusId :selected").val() == status.ReviewedAndRecommend || $("#StatusId :selected").val() == status.SubmittedForReview) {
            $("#EnableForwardTo option[value ='" + loginUserRole.DD + "']").hide();
            $("#EnableForwardTo option[value !='" + loginUserRole.DD + "']").show();
            $("#EnableForwardTo").show();
            GetRoleList();
        }
        else if ($("#StatusId :selected").val() == status.SendBackForCorrection) {
            $("#EnableForwardTo option[value ='" + loginUserRole.DD + "']").show();
            $("#EnableForwardTo option[value !='" + loginUserRole.DD + "']").hide();
            $("#EnableForwardTo").show();
            //GetUsers('ForwardTo',4)
            GetRoles('ForwardTo', 5);
        }
        $("#EnableForwardTo option[value ='0']").show();
        $("#ForwardTo").val(0);
}

function UpdateRemarksDetailsForSeat() {
    var StatusId = $('#StatusId :selected').val();
    var ForwardTo = $('#ForwardTo :selected').val();
    var remarks = $('#RemarksTxt').val();
    var IsValid = true;

    $("#ForwardTo-Required").hide();
    $("#StatusId-Required").hide();
    if (StatusId == 0 || StatusId == "") {
        IsValid = false;
        $("#StatusId-Required").show();
    }
    else if (StatusId == status.ReviewedAndRecommend || StatusId == status.SubmittedForReview || StatusId == status.SendBackForCorrection) {
        if (ForwardTo == 0 || ForwardTo == "" || ForwardTo == "choose") {
            IsValid = false;
            $("#ForwardTo-Required").show();
            bootbox.alert('Kindly clear the error in that page');
        }
    }
    if (remarks == " ") {
        IsValid = false;
        $("#Remarkstxt-Required").show();
    } else {
        $("#Remarkstxt-Required").hide();
    }
    if (IsValid) {
        var msg = "";
        if ($('#StatusId').val() == 7)
            msg = "<br/> Are you sure, you want to send rules of seat allocation to <b>" + $('#ForwardTo option:selected').text() + "</b> for review?";
        else
            msg = "<br/> Are you sure, you want to send back rules of seat allocation for correction/clarification to <b>" + $('#ForwardTo option:selected').text() + "</b> ?";
        bootbox.confirm({
            message: msg,
        //bootbox.confirm({
        //    message: "<br><br>Are you sure to update status for the rules of seat allocation ?",
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
        //bootbox.confirm("<br><br>Are you sure to update status for the rule of allocation?", function (confirmed) {
        //    if (confirmed) {
        //        ToSuccessfulSaveYesDataSave();
        //    }
        //});
    }
}

function ToSuccessfulSaveYesDataSave() {

    var Remarks = $('textarea#RemarksTxt').val();
    var StatusId = $('#StatusId :selected').val();
    var Exam_Year = $('#ReviewExamYear :selected').val();
    var CourseId = $('#ReviewCourseTypes :selected').val();
    var ForwardTo = $('#ForwardTo :selected').val();
    var Rules_allocation_masterid = $('#SeatAllocationReviewId').val();

    if (StatusId == 2) {
        ForwardTo = 5;
        //alertmsg = "<br><br>Allocation Rules Updated for the Session : <b>" + $('#ReviewExamYear :selected').text() + "</b> for the Course Type : <b>" + $('#ReviewCourseTypes :selected').text() + "</b> has been Approved";
        alertmsg = "<br><br>Rules of Seat Allocation Approved Successfully";
    }
    else if (StatusId == status.SubmittedForReview || StatusId == status.ReviewedAndRecommend) {
        ForwardTo = ForwardTo;
        //alertmsg = "<br><br>Allocation Rules Updated for the Session : <b>" + $('#ReviewExamYear :selected').text() + "</b> for the Course Type : <b>" + $('#ReviewCourseTypes :selected').text() + "</b> forwarded to <b>" + $('#ForwardTo :selected').text() + "</b> for Review";
        alertmsg = "<br><br>Rules of Seat Allocation Sent to <b>" + $('#ForwardTo :selected').text() + "</b> for Review Successfully.";
    }
    else {
        ForwardTo = 5;
       // alertmsg = "<br><br>Allocation Rules Updated for the Session : <b>" + $('#ReviewExamYear :selected').text() + "</b> for the Course Type : <b>" + $('#ReviewCourseTypes :selected').text() + "</b> sent back to <b> Deputy Director </b> for Clarification";
        alertmsg = "<br><br>Rules of Seat Allocation Sent Back to <b>" + $('#ForwardTo :selected').text() + "</b> for Correction/Clarification Successfully.";
    }

    $.ajax({
        type: 'POST',
        url: '/Admission/UpdateRemarksForSeats',
        data: {
            Remarks: Remarks,
            StatusId: StatusId,
            Exam_Year: Exam_Year,
            CourseId: CourseId,
            Rules_allocation_masterid: Rules_allocation_masterid,
            ForwardTo: ForwardTo
        },
        success: function (data) {
            if (data == "Success") {
                bootbox.alert({
                    message: alertmsg,
                    callback: function () { location.reload(true); }
                });
                $("#EditPanel").hide();
                GetApprovedAllocationSeatofRuletoView();
            }
            else {
                bootbox.alert("<br><br>There is error in ur data");

            }
        }, error: function (data) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

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
                            $(nTd).html("<input type='button' onclick='GetSeatAllocationById(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' value='View' id='view'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetApprovedAllocationSeatofRuletoView() {

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
                            $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'Rules_allocation_masterid',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetSeatAllocationForViewById(" + oData.Rules_allocation_masterid + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    }
                ]
            });
        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
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
    $("#SeatAllocationReviewId").val(Rules_allocation_masterid);

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
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
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
    $("#SeatAllocationReviewId").val(Rules_allocation_masterid);

    $.ajax({
        type: 'Get',
        url: '/Admission/GetSeatAllocationById',
        data: { Rules_allocation_masterid: Rules_allocation_masterid },
        success: function (data) {

            if (data != null || data != '') {

                $.each(data.list7, function () {
                    $("#ViewExamYear").append($("<option/>").val(this.YearID).text(this.Year));
                });

                $.each(data.list8, function () {
                    $("#ViewCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });

                $.each(data.list9, function () {
                    $("#ViewSyllabus").append($("<option/>").val(this.Syllabus_type_id).text(this.Syllabus_type));
                });

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
                    $("#ViewExamYear").val(this.Exam_Year);
                    $("#ViewExamYear").attr('readonly', true);

                    $("#ViewCourseTypes").val(this.CourseId);
                    $("#ViewCourseTypes").attr('readonly', true);
                });

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
                });

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
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
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

    $("#Syllabus").val("");
    $("#UpdateSyllabus").val("");
    $("#ViewSyllabus").val("");

    $("#ReservationRural").val("");
    $("#ViewReservationRural").val("");
    $("#UpdateReservationRural").val("");

    $("#ReservationTransgender").val("");
    $("#ViewReservationTransgender").val("");
    $("#UpdateReservationTransgender").val("");
}