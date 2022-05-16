var seatAllocDescMsg = "Seat Allocation List";

$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');
    GetExamYear("AcademicYear");
    GetCourses("CourseTypes");
    GetApplicantType("ApplicantTypes");
    GetAdmissionRounds("Rounds");

    GetCourses("CourseTypesEdit");
    GetApplicantType("ApplicantTypesEdit");
    GetExamYear("AcademicYearEdit");
    GetAdmissionRounds("RoundsEdit");

    GetCourses("ddlCourseTypesGen");
    GetApplicantType("ddlApplicantTypeGen");
    GetAdmissionRounds("ddlRoundGen");

    GetExamYear("AcademicYearVAS");
    GetCourses("CourseTypesVAS");
    GetApplicantType("ApplicantTypesVAS");
    GetAdmissionRounds("RoundsVAS");

    GetStatusList();

    Getusers("users", "1");
    Getusers("users1", "1");
    Getusers("Correctionusers", "1");

    $('.footDiv').hide();
    $('#footDiv1').hide();
    $('#footDiv2').hide();

    $('#GridListDiv').show();
    $('#ViewDiv1').hide();
    $("#ApplicantTypeErr").hide();
    $("#AcademicyearErr").hide();
    $("#RoundErr").hide();
    $("#ApplicantTypeGenErr").hide();
    $("#AcademicyearGenErr").hide();
    $("#CourseTypeGenErr").hide();
    $("#RoundGenErr").hide();
    var tblId = '';
    if ($("#hdnSession").data('value') == loginUserRole.DD) {
        tblId = 'UpdateseatAutoAllotmentTable';
    } else {
        tblId = 'seatAutoAllotmentReviewTable';
    }
    GetGeneratedSeatAutoAllotmentList(tblId);
    GetAcademicYear();
    GetCourseTypes();
});

$('a[href="#tab_1"]').click(function () {
    $("#ApprovedViewPanel").hide();
    $("#EditPanel").hide();
});

function GetExamYear(AcademicYear) {
    $.ajax({
        type: "GET",
        url: "/Admission/GetExamYear",
        contentType: "application/json",
        success: function (data) {
          if (data != null || data != '') {
            $("#" + AcademicYear).append('<option value="choose">Choose</option>');
              $.each(data, function () {
                    $("#" + AcademicYear).append($("<option/>").val(this.YearID).text(this.Year));
                    $("select option:contains(" + (new Date).getFullYear() + ")").attr('selected', true);
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GenerateSeatAutoAllotment() {
    var course = $('#CourseTypes').val();
    var applicnttype = $('#ApplicantTypes').val();
    var academicYear = $('#AcademicYear').val();
    var round = $('#Rounds').val();
    $('#coursetypeErr').text('');
    $('#applicantErr').text('');
    $('#academicYearErr').text('');
    $('#roundErr').text('');
    if (course == 'choose') {
        $('#coursetypeErr').text('select the course');
    }
    else if (applicnttype == 'choose') {
        $('#applicantErr').text('select the applicant type');
    }
    else if (academicYear == 'choose') {
        $('#academicYearErr').text('select the Session');
    }
    else if (round == 'choose') {
        $('#roundErr').text('select the round');
    }
    else {
        $('#loading').show();
        $.ajax({
            type: "GET",
            url: "/Admission/GenerateSeatAutoAllotment",
            data: { 'courseType': course, 'applicantType': applicnttype, 'academicYear': academicYear, 'round': round },
            contentType: "application/json",
            success: function (data) {
                $('#loading').hide();
                $('.ShowSendToSubmit').attr('disabled', true);
                if (data.length == 0) {
                    bootbox.alert("<br><br> You have already generated the " + seatAllocDescMsg + " for the Session: <b>" + $("#AcademicYear :selected").text() +
                        "</b>, Course :<b>" + $('#CourseTypes :selected').text() + "</b>, ApplicantType :<b>" + $('#ApplicantTypes :selected').text() + "</b>, Round :<b>" + $('#Rounds :selected').text() + "</b>, <br> For more details, Requesting go to tab <b> " + $('.nav-tabs li:eq(1) a').tab().text() + " </b>");

                    $('.ShowSendToSubmit').attr('disabled', false);
                    $('.footDiv').hide();
                }
                else {
                    $('.footDiv').show();
                    if (data.length != 0) {
                        
                        currApplType = data[0].ApplicantType;
                        currRound = data[0].RoundSm;
                    }
                }
                $('#seatAutoAllotmentTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    scrollX: true,
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download as Excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                    16, 17, 18, 19, 20, 21, 22
                                ]
                            }
                        }
                    ],
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'Rank', 'title': 'Rank Number', 'className': 'text-center' },
                        { 'data': 'Status', 'title': 'Seat Allocation Status - Round', 'className': 'text-center' },
                        { 'data': 'division_id', 'title': 'Division Code', 'className': 'text-left' },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                        { 'data': 'district_id', 'title': 'District Code', 'className': 'text-left' },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                        { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                        { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        { 'data': 'ITIType', 'title': 'Type of Institute', 'className': 'text-left' },
                        { 'data': 'ITIName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'TradeCode', 'title': 'Trade Code', 'className': 'text-left' },
                        { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                        { 'data': 'UnitsDet', 'title': 'Unit', 'className': 'text-left' },
                        { 'data': 'ShiftsDet', 'title': 'Shift', 'className': 'text-left' },
                        { 'data': 'AllottedCategory', 'title': 'Allotted Vertical Category', 'className': 'text-left' },
                        { 'data': 'AllottedGroup', 'title': 'Allotted Horizontal Category', 'className': 'text-left' },
                        { 'data': 'LocalStatus', 'title': 'Local Status (HK/NON-HK)', 'className': 'text-left' },
                        { 'data': 'PreferenceNumber', 'title': 'Preference Order Number', 'className': 'text-left' },
                        { 'data': 'FirstName', 'title': 'Name', 'className': 'text-left' },
                        { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                        { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                        {
                            'data': 'DOB',
                            'title': 'DOB',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var date = daterangeformate2(oData.DOB, 1);
                                $(nTd).html(date + " " + "<input type='hidden' id='allocationId' value='" + oData.seatAllocDetailId + "' />");
                            }
                        },
                        { 'data': 'DateOfBirth', 'title': 'Date Of Birth', 'className': 'text-left', 'visible': false },

                    ]
                });
                var tblId = '';
                if ($("#hdnSession").data('value') == loginUserRole.DD) {
                    tblId = 'UpdateseatAutoAllotmentTable';
                } else {
                    tblId = 'seatAutoAllotmentReviewTable';
                }
                GetGeneratedSeatAutoAllotmentList(tblId);
            }, error: function (result) {
                $('#loading').hide();
                alert("Error", "something went wrong");
            }
        });
    }
}

$("#btnExport").click(function () {
    $("#seatAutoAllotmentTable").table2excel({
        filename: "SeatAllocation.xls"
    });
});

$("#pdfSeatAllocationDownload").click(function () {
    $("#tblShowGeneratedSeatAllocation").table2excel({
        filename: "SeatAllocation.xls"
    });
});

$("#pdfReviewSeatAllocationDownload").click(function () {
    $("#tblShowGeneratedSeatAllocationReview").table2excel({
        filename: "SeatAllocation.xls"
    });
});

$("#pdfApprovedSeatAllocationDownload").click(function () {
    $("#tblApprovedShowGeneratedSeatAllocationReview").table2excel({
        filename: "SeatAllocationApprovedList.xls"
    });
});

function cancelSeatAutoAllotment() {
    $('#CourseTypes').val('choose');
    $('#ApplicantTypes').val('choose');
    $('#AcademicYear').val('choose');
    $('#Rounds').val('choose');
    $('#users').val('choose');
}

function SubmitSeatAutoAllotment() {

    var table = $("#seatAutoAllotmentTable tbody");

    var Data = new FormData();
    var role = $('#users').val();
    var Status = $('#StatusId').val();

    table.find("tr").each(function (len) {
        var $tr = $(this);
        var allocId = $tr.find("#allocationId").val();
        Data.append('seatAllocDetailId', allocId);
    });
    Data.append('roleId', role);
    Data.append('Remarks', $('#SendToRemarks').val());
    Data.append('Status', Status);

    bootbox.confirm({
        message: fnDisplayConfirmMsg('users'),
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
                $.ajax({
                    url: '/Admission/ForwardSeatAutoAllotment',
                    dataType: 'json',
                    type: 'POST',
                    data: Data,
                    processData: false,
                    contentType: false,
                    success: function (data) {

                        if (data == true) {
                            bootbox.alert(fnDisplayAlertMsg('users', Status));
                            if ($.fn.DataTable.isDataTable('#seatAutoAllotmentTable')) {
                                $('#seatAutoAllotmentTable').DataTable().destroy();
                            }

                            $('#seatAutoAllotmentTable tbody').empty();

                            // ... skipped ...

                            $('#seatAutoAllotmentTable').dataTable({
                                "autoWidth": false,
                                "info": true,
                                "paging": true,
                                "scrollX": true,
                                "bSort": true
                            });

                            $("#SendToRemarks").val('');
                            $("#users").val('choose');
                            $("#StatusId").val('0');
                            $(".GenerateSubmitButton").hide();
                            $(".footDiv").hide();
                            $("#EnableForwardTo").hide();
                            var tblId = '';
                            if ($("#hdnSession").data('value') == loginUserRole.DD) {
                                tblId = 'UpdateseatAutoAllotmentTable';
                            } else {
                                tblId = 'seatAutoAllotmentReviewTable';
                            }
                            GetGeneratedSeatAutoAllotmentList(tblId);
                        }
                        else {
                            alert("failed")
                        }
                    }
                });
            }
        }
    });
}

//tab 2 update details
function GetGeneratedSeatAutoAllotmentList(id) {
    var course = 0;
    var applicnttype = 0;
    var academicYear = 0;
    var round = 0;

    var sessionValue = $("#hdnSession").data('value');
    $(".btn-SubmitReviewSeatAutoAllotment").hide();

    $.ajax({
        type: "GET",
        url: "/Admission/GetGeneratedSeatAutoAllotmentList",
        data: { 'courseType': course, 'applicantType': applicnttype, 'academicYear': academicYear, 'round': round },
        contentType: "application/json",
        success: function (data) {
            if (data.length != 0) {
                currApplType = data[0].ApplicantType;
                currRound = data[0].RoundSm;
            }
            $('#' + id).DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                bAutoWidth: false,
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'AcademicYearSm', 'title': 'Session', 'className': 'text-left' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'ApplicantType', 'title': 'Applicant Type', 'className': 'text-left' },
                    { 'data': 'RoundSm', 'title': 'Round', 'className': 'text-center' },
                    {
                        'data': 'Status', "render": function (data, type, row) {
                            return data + ' - ' + row['userRole'];
                        }
                        , 'title': 'Status - Currently With', 'className': 'text-left'
                    },
                    {
                        'data': 'AllocationId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetRemarks(" + oData.AllocationId + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'AllocationId', 'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if ($("#hdnSession").data('value') == loginUserRole.DD) {
                                $(nTd).html("<input type='button' class='btn btn-primary' onclick='ViewSeatAutoAllotment(" + oData.AllocationId + ")' id='View' value='View' />");
                            } else {
                                $(nTd).html("<input type='button' class='btn btn-primary' onclick='ViewSeatAutoAllotmentReview(" + oData.AllocationId + ")' id='View' value='View' /><input type='button' class='btn btn-primary'");
                            }
                            if (oData.FlowId == sessionValue) {
                                $(".btn-SubmitReviewSeatAutoAllotment").show();
                            }
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function FilterGetGeneratedSeatAutoAllotmentList() {
    var course = $('#CourseTypesEdit').val();
    var applicnttype = $('#ApplicantTypesEdit').val();
    var academicYear = $('#AcademicYearEdit').val();
    var round = $('#RoundsEdit').val();
    $('#coursetypeEditErr').text('');
    $('#applicantEditErr').text('');
    $('#academicYearEditErr').text('');
    $('#roundEditErr').text('');
    if (course == 'choose') {
        $('#coursetypeEditErr').text('select the course');
    }
    else if (applicnttype == 'choose') {
        $('#applicantEditErr').text('select the applicant type');
    }
    else if (academicYear == '0') {
        $('#academicYearEditErr').text('select the Session');
    }
    else if (round == 'choose') {
        $('#roundEditErr').text('select the round');
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetGeneratedSeatAutoAllotmentList",
            data: { 'courseType': course, 'applicantType': applicnttype, 'academicYear': academicYear, 'round': round },
            contentType: "application/json",
            success: function (data) {
                $('#UpdateseatAutoAllotmentTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'Rank', 'title': 'Rank Number', 'className': 'text-center' },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' }
                    ]
                });
            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
}

function ViewSeatAutoAllotment(allocationId) {
    $(".btn-SubmitSeatAutoAllotment").hide();
    $(".CorrectionSendTo").hide();
    var sessionValue = $("#hdnSession").data('value');
    $.ajax({
        type: "GET",
        url: "/Admission/ViewSeatAutoAllotment",
        data: { 'allocationId': allocationId },
        contentType: "application/json",
        success: function (data) {

            if (data.length != 0) {
                $("#pdfSeatAllocationDownload").show();
            }

            $('#ShowGeneratedSeatAllocationModel').modal('show');
            var t = $('#tblShowGeneratedSeatAllocation').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                scrollX: true,
                responsive: true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                16, 17, 18, 19, 20, 21, 22
                            ]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'RankNumber', 'title': 'Rank Number', 'className': 'text-center' },
                    { 'data': 'Status', 'title': 'Seat Allocation Status - Round', 'className': 'text-center' },
                    { 'data': 'division_id', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left' },
                    { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-center' },
                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeCode', 'title': 'Trade Code', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'ShiftsDet', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'UnitsDet', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'AllottedCategory', 'title': 'Allotted Vertical Category', 'className': 'text-left' },
                    { 'data': 'AllottedGroup', 'title': 'Allotted Horizontal Category', 'className': 'text-left' },
                    { 'data': 'LocalStatus', 'title': 'Local Status', 'className': 'text-left' },
                    { 'data': 'PreferenceNumber', 'title': 'Preference Number', 'className': 'text-left' },
                    { 'data': 'FirstName', 'title': 'Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                    {
                        'data': 'DOB',
                        'title': 'DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date + " " + "<input type='hidden' id='allocationId' value='" + oData.AllocationId + "' />");
                            if (oData.FlowId == sessionValue) {
                                $(".CorrectionSendTo").show();
                                $(".btn-SubmitSeatAutoAllotment").show();
                            }
                        }
                    },
                    { 'data': 'DateOfBirth', 'title': 'Date Of Birth', 'className': 'text-left', 'visible': false }
                ]

            });

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function EditSeatAutoAllotment(allocationId) {
    //$.ajax({
    //    type: "GET",
    //    url: "/Admission/ViewSeatAutoAllotment",
    //    data: { 'courseType': allocationId },
    //    contentType: "application/json",
    //    success: function (data) {

    //    }, error: function (result) {
    //        alert("Error", "something went wrong");
    //    }
    //});
}
//end tab 2

//ram
function GetApplicantType() {
    $("#ddlApplicantType").empty();
    $("#ddlApplicantType").append('<option value="0">choose</option>');
    $("#ddlApplicantTypeGen").empty();
    $("#ddlApplicantTypeGen").append('<option value="0">choose</option>');
    $("#ddlApplicantTypecheck").empty();
    $("#ddlApplicantTypecheck").append('<option value="0">choose</option>');
    $("#ddlApplicantTypeView").empty();
    $("#ddlApplicantTypeView").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetApplicantType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlApplicantType").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    $("#ddlApplicantTypeGen").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    $("#ddlApplicantTypecheck").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    $("#ddlApplicantTypeView").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetAcademicYear() {
    var currYear = (new Date).getFullYear();
    $("#ddlAcademicYear").empty();
    $("#ddlAcademicYear").append('<option value="0">choose</option>');
    $("#ddlAcademicYearGen").empty();
    $("#ddlAcademicYearGen").append('<option value="0">choose</option>');
    $("#ddlAcademicYearView").empty();
    $("#ddlAcademicYearView").append('<option value="0">choose</option>');
    $("#ddlAcademicYearcheckSum").empty();
    $("#ddlAcademicYearcheckSum").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetAcademicYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlAcademicYear").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ddlAcademicYearGen").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ddlAcademicYearView").append($("<option/>").val(this.YearID).text(this.Year));
                    $("#ddlAcademicYearcheckSum").append($("<option/>").val(this.YearID).text(this.Year));
                    $("select option:contains(" + currYear + ")").attr('selected', true);
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetCourseTypes() {

    $("#ddlCourseTypes").empty();
    $("#ddlCourseTypes").append('<option value="0">choose</option>');

    $("#ddlCourseTypeGen").empty();
    $("#ddlCourseTypeGen").append('<option value="0">choose</option>');

    $("#ddlCourseTypeView").empty();
    $("#ddlCourseTypeView").append('<option value="0">choose</option>');

    $("#ddlCourseTypecheckSum").empty();
    $("#ddlCourseTypecheckSum").append('<option value="0">choose</option>');


    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ddlCourseTypeGen").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ddlCourseTypeView").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ddlCourseTypecheckSum").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

//List Grid
function GetGenerateSeatMatrix1() {
    var ddlCourseTypesGen = $('#ddlCourseTypesGen :selected').val();
    var ddlApplicantTypeGen = $('#ddlApplicantTypeGen :selected').val();
    var ddlAcademicYearGen = $('#ddlAcademicYearGen :selected').val();
    var ddlRoundGen = $('#ddlRoundGen :selected').val();
    //
    $.ajax({
        type: "GET",
        url: "/Admission/GetGenerateSeatMatrix1",
        data: { 'ddlCourseTypesGen': ddlCourseTypesGen, 'ddlApplicantTypeGen': ddlApplicantTypeGen, 'ddlAcademicYearGen': ddlAcademicYearGen, 'ddlRoundGen': ddlRoundGen },
        contentType: "application/json",
        success: function (data) {
            //
            $('#tblGenerateseatmatrix1').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                bAutoWidth: false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
                    { 'data': 'CourseName', 'width': '100px', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'ApplicantTypeDdl', 'title': 'Applicant Type', 'className': 'text-center' },
                    { 'data': 'ExamYear', 'title': 'AcademicYear', 'className': 'text-center' },
                    { 'data': 'Round', 'title': 'Round', 'className': 'text-center' },
                    { 'data': 'StatusName', 'title': 'Status Name', 'className': 'text-left' },
                    {
                        'data': 'AllocationId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='viewSeatMatrixDetails(" + oData.AllocationId + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'AllocationId', 'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<button class='btn btn-primary btn-xs' onclick='GetRemarks(" + oData.AllocationId + ")'>View</button>");
                        }
                    },
                ],
            });
        }
    });
}

function GenerateSeatMatrix1() {
    GetGenerateSeatMatrix1();
}

$("#btnExport1").click(function () {
    $("#seatAutoAllotmentTable1").table2excel({
        filename: "Table.xls"
    });
});

$("#btnExport2").click(function () {
    $("#seatAutoAllotmentViewList").table2excel({
        filename: "Table.xls"
    });
});

function viewSeatMatrixDetails(id) {
    
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatMatrixViewDetails",
        contentType: "application/json",
        data: { 'id': id },
        success: function (data) {
            
            $('#seatAutoAllotmentTable1').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                bAutoWidth: false,
                columns: [
                    { 'data': 'AllocationId', 'title': 'AllocationId', 'className': 'text-center' },
                    { 'data': 'RankNumber', 'title': 'Rank Number', 'className': 'text-left' },
                    { 'data': 'division_id', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-left' },
                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeCode', 'title': 'Trade Code', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'AllottedCategory', 'title': 'Allotted Category', 'className': 'text-left' },
                    { 'data': 'AllottedGroup', 'title': 'Allotted Group', 'className': 'text-left' },
                    { 'data': 'LocalStatus', 'title': 'Local Status', 'className': 'text-left' },
                    { 'data': 'PreferenceNumber', 'title': 'Preference No', 'className': 'text-left' },
                    { 'data': 'FirstName', 'title': 'First Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                ]
            });
            $('#footDiv1').show();
            //$('#SeatViewModal').modal('show');
        },
        error: function () {

        }
    });
}

function CorrectionSeatAutoAllotment() {

    var table = $("#tblShowGeneratedSeatAllocation tbody");

    var Data = new FormData();
    var role = $('#Correctionusers').val();
    var Status = $('#CorrectionStatusId').val();

    table.find("tr").each(function (len) {
        var $tr = $(this);
        var allocId = $tr.find("#allocationId").val();
        Data.append('seatAllocDetailId', allocId);
    });
    Data.append('roleId', role);
    Data.append('Remarks', $('#CorrectionSendToRemarks').val());
    Data.append('Status', Status);

    bootbox.confirm({
        message: fnDisplayConfirmMsg('Correctionusers', Status),
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

                $.ajax({
                    url: '/Admission/ForwardSeatAutoAllotment',
                    dataType: 'json',
                    type: 'POST',
                    data: Data,
                    processData: false,
                    contentType: false,
                    success: function (data) {

                        if (data == true) {
                            bootbox.alert(fnDisplayAlertMsg('Correctionusers', Status));
                            $("#ShowGeneratedSeatAllocationModel").modal('hide');
                            var tblId = '';
                            if ($("#hdnSession").data('value') == loginUserRole.DD) {
                                tblId = 'UpdateseatAutoAllotmentTable';
                            } else {
                                tblId = 'seatAutoAllotmentReviewTable';
                            }
                            GetGeneratedSeatAutoAllotmentList(tblId);
                            $(".CorrectionSendTo").hide();
                        }
                        else {
                            alert("failed")
                        }
                    }
                });
            }
        }
    });
}

//Tab 3 JD / AD

function FilterGetGeneratedSeatAutoAllotmentReviewList() {
    var course = $('#ddlCourseTypesGen').val();
    var applicnttype = $('#ddlApplicantTypeGen').val();
    var academicYear = $('#ddlAcademicYearGen').val();
    var round = $('#ddlRoundGen').val();
    $('#ddlCourseTypesGenErr').text('');
    $('#ddlApplicantTypeGenErr').text('');
    $('#ddlAcademicYearGenErr').text('');
    $('#ddlRoundGenErr').text('');
    if (course == 'choose') {
        $('#ddlCourseTypesGenErr').text('select the course');
    }
    else if (applicnttype == 'choose') {
        $('#ddlApplicantTypeGenErr').text('select the applicant type');
    }
    else if (academicYear == '0') {
        $('#ddlAcademicYearGenErr').text('select the Session');
    }
    else if (round == 'choose') {
        $('#ddlRoundGenErr').text('select the round');
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetGeneratedSeatAutoAllotmentList",
            data: { 'courseType': course, 'applicantType': applicnttype, 'academicYear': academicYear, 'round': round },
            contentType: "application/json",
            success: function (data) {
                if (data.length != 0) {
                    currApplType = data[0].ApplicantType;
                    currRound = data[0].RoundSm;
                }
                $('#seatAutoAllotmentReviewTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'AcademicYearSm', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-center' },
                        { 'data': 'ApplicantType', 'title': 'Applicant Type', 'className': 'text-left' },
                        { 'data': 'RoundSm', 'title': 'Round', 'className': 'text-center' },
                        {
                            'data': 'Status', "render": function (data, type, row) {
                                return data + ' - ' + row['userRole'];
                            }
                            , 'title': 'Status - Currently With', 'className': 'text-left'
                        },
                        {
                            'data': 'AllocationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetRemarks(" + oData.AllocationId + ")' class='btn btn-primary' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'AllocationId', 'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' class='btn btn-primary' onclick='ViewSeatAutoAllotmentReview(" + oData.AllocationId + ")' id='View' value='View' /><input type='button' class='btn btn-primary'");
                            }
                        }
                    ]
                });
            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
}

function SubmitReviewSeatAutoAllotment() {

    var msg = "<br><br> Select a valid #idText before submitting the records.";
    if ($("#ReviewStatusId :selected").val() == 0) {
        bootbox.alert(msg.replace("#idText", "<b>'Status'</b>"));
    } else if ($("#Reviewusers :selected").val() == 0 && $("#ReviewStatusId :selected").val() != status.Approved) {
        bootbox.alert(msg.replace("#idText","<b>'Send To'</b> option"));
    } else {
        var table = $("#tblShowGeneratedSeatAllocationReview tbody");

        var Data = new FormData();
        var Status = $('#ReviewStatusId').val();
        var role = $('#Reviewusers').val();
        if (Status == 2) {
            role = $("#hdnSession").data('value');
        }

        table.find("tr").each(function (len) {
            var $tr = $(this);
            var allocId = $tr.find("#allocationId").val();
            Data.append('seatAllocDetailId', allocId);
        });

        if (Status == 4)    //Sent for Correction
            role = 5;       //Deputy Director

        Data.append('roleId', role);
        Data.append('Status', Status);
        Data.append('Remarks', $('#ReviewSendToRemarks').val());

        for (var value of Data.values()) {
            console.log(value);
        }
        
        id = 'Reviewusers';
        bootbox.confirm({
            message: fnDisplayConfirmMsg(id, Status),
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

                    $.ajax({
                        url: '/Admission/ForwardSeatAutoAllotment',
                        dataType: 'json',
                        type: 'POST',
                        data: Data,
                        processData: false,
                        contentType: false,
                        success: function (data) {

                            if (data == true) {
                                if (Status == 4)
                                    bootbox.alert(fnDisplayAlertMsg('Reviewusers', Status));
                                else if (Status == 2)
                                    bootbox.alert(fnDisplayAlertMsg('Reviewusers', Status));
                                else
                                    bootbox.alert(fnDisplayAlertMsg('Reviewusers', Status));

                                $("#ShowGeneratedSeatAllocationReviewModel").modal('hide');
                                var tblId = '';
                                if ($("#hdnSession").data('value') == loginUserRole.DD) {
                                    tblId = 'UpdateseatAutoAllotmentTable';
                                } else {
                                    tblId = 'seatAutoAllotmentReviewTable';
                                }
                                GetGeneratedSeatAutoAllotmentList(tblId);
                            }
                            else {
                                alert("failed")
                            }
                        }
                    });
                }
            }
        });
    }
}

function ViewSeatAutoAllotmentReview(allocationId) {

    $("#ReviewSendToRemarks").val('');
    $("#ReviewStatusId").val(0);
    $(".ReviewSendTo").hide();
    OnchangeReviewStatus();
    OnchangeStatus();
    OnCorrectionchangeStatus();

    $.ajax({
        type: "GET",
        url: "/Admission/ViewSeatAutoAllotment",
        data: { 'allocationId': allocationId },
        contentType: "application/json",
        success: function (data) {

            var sessionValue = $("#hdnSession").data('value');
            $(".btn-SubmitReviewSeatAutoAllotment").hide();

            $('#ShowGeneratedSeatAllocationReviewModel').modal('show');
            var t = $('#tblShowGeneratedSeatAllocationReview').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                scrollX: true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                16, 17, 18, 19, 20, 21, 22, 23
                            ]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'RankNumber', 'title': 'Rank Number', 'className': 'text-center' },
                    { 'data': 'Status', 'title': 'Seat Allocation Status - Round', 'className': 'text-center' },
                    { 'data': 'division_id', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left' },
                    { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'InstituteType', 'title': 'Type of Institute', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeCode', 'title': 'Trade Code', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'UnitsDet', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'ShiftsDet', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'AllottedCategory', 'title': 'Allotted Vertical Category', 'className': 'text-left' },
                    { 'data': 'AllottedGroup', 'title': 'Allotted Horizontal Category', 'className': 'text-left' },
                    { 'data': 'LocalStatus', 'title': 'Local Status (HK/NON-HK)', 'className': 'text-left' },
                    { 'data': 'PreferenceNumber', 'title': 'Preference Order Number', 'className': 'text-left' },
                    { 'data': 'FirstName', 'title': 'Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                    { 'data': 'DateOfBirth', 'title': 'Date Of Birth', 'className': 'text-left', 'visible': false },
                    {
                        'data': 'DOB',
                        'title': 'DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date + " " + "<input type='hidden' id='allocationId' value='" + oData.AllocationId + "' />");
                            if (oData.FlowId == sessionValue) {
                                $(".ReviewSendTo").show();
                                $(".btn-SubmitReviewSeatAutoAllotment").show();
                            }
                        }
                    },
                    { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible': false }
                ]

            });

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
//Approved view list
function GetApprovedAllocationSeatofRuletoView() {

    var course = $('#CourseTypesVAS').val();
    var applicnttype = $('#ApplicantTypesVAS').val();
    var academicYear = $('#AcademicYearVAS').val();
    var round = $('#RoundsVAS').val();
    $('#CourseTypesVASErr').text('');
    $('#applicantErr').text('');
    $('#AcademicYearVASErr').text('');
    $('#roundErr').text('');
    $("#footDiv2").hide();
    if (course == 'choose') {
        $('#CourseTypesVASErr').text('select the course');
    }
    else if (applicnttype == 'choose') {
        $('#applicantErr').text('select the applicant type');
    }
    else if (academicYear == 'choose') {
        $('#AcademicYearVASErr').text('select the Session');
    }
    else if (round == 'choose') {
        $('#roundErr').text('select the round');
    }
    else {

        $.ajax({
            type: 'Get',
            url: '/Admission/GetSeatMatrixViewList',
            data: { 'courseType': course, 'applicantType': applicnttype, 'academicYear': academicYear, 'round': round },
            success: function (data) {
                if (data.length != 0) {
                    currApplType = data[0].ApplicantType;
                    currRound = data[0].RoundSm;
                }
                $('#seatAutoAllotmentViewList').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    scrollX: true,
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'AcademicYearSm', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-center' },
                        { 'data': 'ApplicantType', 'title': 'Applicant Type', 'className': 'text-left' },
                        { 'data': 'RoundSm', 'title': 'Round', 'className': 'text-center' },
                        {
                            'data': 'Status', 'className': 'text-center', "render": function (data, type, row) {
                                return data;
                            }
                            , 'title': 'Status - Currently With', 'className': 'text-left'
                        },
                        {
                            'data': 'AllocationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetRemarks(" + oData.AllocationId + ")' class='btn btn-primary' value='View' id='view'/>");

                            }
                        },
                        {
                            'data': 'AllocationId', 'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' class='btn btn-primary' onclick='ApprovedViewSeatAutoAllotmentReview(" + oData.AllocationId + ")' id='View' value='View' />");
                            }
                        }
                    ]
                });
                $('.ReviewSendTo').show();

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
}


function ApprovedViewSeatAutoAllotmentReview(allocationId) {
    $("#ReviewSendTo").show();
    $.ajax({
        type: "GET",
        url: "/Admission/ViewSeatAutoAllotment",
        data: { 'allocationId': allocationId },
        contentType: "application/json",
        success: function (data) {

            $('#ApprovedShowGeneratedSeatAllocationReviewModel').modal('show');
            var t = $('#tblApprovedShowGeneratedSeatAllocationReview').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                scrollX: true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                                16, 17, 18, 19, 20, 21
                            ]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'RankNumber', 'title': 'Rank Number', 'className': 'text-center' },
                    { 'data': 'Status', 'title': 'Seat Allocation Status - Round', 'className': 'text-center' },
                    { 'data': 'division_id', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left' },
                    { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'InstituteType', 'title': 'Type of Institute', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeCode', 'title': 'Trade Code', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'UnitsDet', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'ShiftsDet', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'AllottedCategory', 'title': 'Allotted Vertical Category', 'className': 'text-left' },
                    { 'data': 'AllottedGroup', 'title': 'Allotted Horizontal Category', 'className': 'text-left' },
                    { 'data': 'LocalStatus', 'title': 'Local Status (HK/NON-HK)', 'className': 'text-left' },
                    { 'data': 'PreferenceNumber', 'title': 'Preference Order Number', 'className': 'text-left' },
                    { 'data': 'FirstName', 'title': 'First Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left' },
                    {
                        'data': 'DOB',
                        'title': 'DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible' : false }
                ]
            });

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetRemarks(seatId) {
    $.ajax({
        type: "GET",
        url: "/Admission/GetCommentDetailsSeatAllocation",
        contentType: "application/json",
        data: { 'seatId': seatId },
        success: function (data) {
            $('#RemarksTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                bAutoWidth: false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CommentsCreatedOn', 'title': 'Date', 'className': 'text-left' },
                    { 'data': 'userRole', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'ForwardedTo', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Description', 'className': 'text-left' },
                ]
            });
            $('#RemarksModal').modal('show');
        },
        error: function () {

        }
    });
}

function SubmitSeatAutoAllotmentReview() {

    var table = $("#seatAutoAllotmentTable tbody");

    var Data = new FormData();
    var role = $('#users').val();
    table.find("tr").each(function (len) {
        var $tr = $(this);
        var allocId = $tr.find("#allocationId").val();
        Data.append('seatAllocDetailId', allocId);
    });
    Data.append('roleId', role);
    $.ajax({
        url: '/Admission/ForwardSeatAutoAllotmentReview',
        dataType: 'json',
        type: 'POST',
        data: Data,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data == true) {
                alert("Data saved successfully");
            }
            else {
                alert("failed")
            }
        }
    });
}

function GetStatusList() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetStatusList',
        success: function (data) {
            $("#StatusId").empty();
            $("#ReviewStatusId").empty();
            $("#CorrectionStatusId").empty();
            $("#StatusId").append('<option value="0">choose</option>');
            $("#ReviewStatusId").append('<option value="0">choose</option>');
            $("#CorrectionStatusId").append('<option value="0">choose</option>');
            if (data != null || data != '') {
                $.each(data, function () {
                    if (this.Value == status.SubmittedForReview) {
                        $("#StatusId").append($("<option/>").val(this.Value).text(this.Text));
                        $("#CorrectionStatusId").append($("<option/>").val(this.Value).text(this.Text));
                    }
                    $("#ReviewStatusId").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function OnchangeReviewStatus() {

    $("#EnableReviewForwardTo").hide();
    if ($("#ReviewStatusId :selected").val() == status.ReviewedAndRecommend || $("#ReviewStatusId :selected").val() == status.SubmittedForReview) {
        Getusers("Reviewusers", "1");
        $("#EnableReviewForwardTo").show();
    }
    else if ($("#ReviewStatusId :selected").val() == status.SendBackForCorrection) {
        Getusers("Reviewusers", "4");
        $("#EnableReviewForwardTo").show();
    }
    $("#EnableReviewForwardTo option[value ='0']").show();
    $("#Reviewusers").val('0');
}

function OnchangeStatus() {
    if ($("#StatusId :selected").val() == status.SubmittedForReview) {
        $("#EnableForwardTo").show();
    }
    else {
        $("#EnableForwardTo").hide();
    }
}

function OnCorrectionchangeStatus() {
    if ($("#CorrectionStatusId :selected").val() == status.SubmittedForReview) {
        $("#CorrectionEnableForwardTo").show();
    }
    else {
        $("#CorrectionEnableForwardTo").hide();
    }
}

function fnDisplayConfirmMsg(id, stat) {
    var toUser = '';
    var seatAllocDescMsgTo = '';
    var tmpseatAllocPurposeMsg = '';
    var seatAllocPurposeMsgFor = 'for ';
    var sendMsg = 'send';
    var seatAllocPurposeMsg = "Review";
    toUser = $('#' + id + ' :selected').text();
    seatAllocDescMsgTo = " to";
    tmpseatAllocPurposeMsg = seatAllocPurposeMsg;
    if (stat == status.SendBackForCorrection) {
        seatAllocPurposeMsg = "Correction/Clarification";
    } else if (stat == status.Approved) {
        seatAllocDescMsgTo = '';
        seatAllocPurposeMsg = '';
        seatAllocPurposeMsgFor = '';
        sendMsg = "Approve";
        toUser = '';
    }
    var msg = "<br><br> Are you sure you want to " + sendMsg + " the " + seatAllocDescMsg + " for " +
        "Session: <b>" + $("#AcademicYear :selected").text() + "</b>, Course : <b>" + $('#CourseTypes :selected').text() +
        "</b>, ApplicantType : <b>" + currApplType + "</b>, Round : <b>" + currRound + "</b>" +
        seatAllocDescMsgTo + " <b> " + toUser + "</b > " + seatAllocPurposeMsgFor + seatAllocPurposeMsg + "?";
    ((tmpseatAllocPurposeMsg != '' ? seatAllocPurposeMsg = tmpseatAllocPurposeMsg : seatAllocPurposeMsg));

    return msg
}

function fnDisplayAlertMsg(id, stat) {
    var toUser = " Sent from <b>" + $("#hdnSessionRole").data('value') + "</b> to <b>" + $('#' + id + ' :selected').text() + "</b> for ";
    var sentBackInfoMsg = '';
    var seatAllocPurposeMsg = "Review";
    if (stat == status.SendBackForCorrection) {
        seatAllocPurposeMsg = "Correction/Clarification";
    }
    else if (stat == status.Approved) {
        toUser = '';
        seatAllocPurposeMsg = " Approved";
    }
    else {
    }

    var msg = "<br><br> " + seatAllocDescMsg + sentBackInfoMsg + " for " +
        "Session: <b>" + $("#AcademicYear :selected").text() +
        "</b>, Course : <b>" + $('#CourseTypes :selected').text() + "</b>, ApplicantType : <b>" + currApplType +
        "</b>, Round : <b>" + currRound + "</b>" +
        toUser + seatAllocPurposeMsg + " Successfully.";
    return msg;
}