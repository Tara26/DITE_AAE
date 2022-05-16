$(document).ready(function () {
    var selectedTab = "Applicant";
    if (selectedTab != null) {
        if (selectedTab == "Applicant") {
            $('.nav-tabs li:eq(0) a').tab('show');
        }
        else {
            $('.nav-tabs li:eq(2) a').tab('show')
        }
    }

    GetCourses("CourseType");
    GetCourses("Coursetyp");
    GetCourses("CoursetypDiv");
    GetApplicantType123("applicantType");
  GetApplicantType123("ApplicantType123");
  GetSessionYear("Session");
  GetSessionYear("Session123");
    GetDivisionsDDp("division");
    GetDistrictsd(0, "district123");
    GetDistrictsd(0, "district1");
    $('#grivanceapply').hide();
    $('#grivanceapply1').hide();

    $('#EditviewDetails').hide();
    GetGrievanceAgainstTentativeList();
    GetLoadApplicantGrievanceGrid();
    GetFileTypes();
  GetGrievanceTentativeStatus();

    $('#verRemarks').hide();
    $('#verifiedbtn').hide();
    $('#sendbtn').hide();
    $('#rejectbtn').hide();
    if ($("#hdnDiv_ID").data('value') != null && $("#hdnDiv_ID").data('value') != "") {
        var divs = $("#hdnDiv_ID").data('value');
        GetDistrictsDDp("districtverificationstatusGR",divs );
    }
});
//GetColleges('instituteverificationstatus', $('#talukverificationstatusGR').val())
//========= START Grievance Status tab 1================
function GetLoadApplicantGrievanceGrid() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetGrievanceGrid",
        contentType: "application/json",
        success: function (data) {
            if (data.Status == true) {
                $('#grivanceapply').hide();
                $('#grivanceapply1').hide();
            }
            else {
                var frmDate = data.From.split(',');
                var toDate = data.To.split(',');
                var dte = new Date();
                var d = dte.getUTCDate();
                var m = dte.getUTCMonth();
                var y = dte.getUTCFullYear();
                if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2])) && new Date(y, m, d) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
                    $('#grivanceapply').show();
                    $('#grivanceapply1').show();
                }
                else {
                    $('#grivanceapply').hide();
                    $('#grivanceapply1').hide();
                }
            }
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}
function GetGrievanceAgainstTentativeList() {
    $.ajax({
        type: "GET",
        url: "/Admission/ApplicantRankDetails",
        contentType: "application/json",
        success: function (data) {
            $('#ApplicantGrievanceTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                scrollX: true,
                "paging": false,
                "ordering": false,
                "info": false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    {
                        'data': 'DOB', 'title': 'DOB', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'Category', 'title': 'Category', 'className': 'text-left' },
                    { 'data': 'ExService', 'title': 'Ex-Serviceman', 'className': 'text-left' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with disability', 'className': 'text-left' },
                    { 'data': 'EconomicWeekerSec', 'title': 'EWS (Economic Weaker Section)', 'className': 'text-left' },
                    { 'data': 'HydKar', 'title': 'Hydrabad/Karnataka', 'className': 'text-left' },
                    { 'data': 'KannadaMedium', 'title': 'KannadaMedium', 'className': 'text-left' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left' },
                    { 'data': 'MarksObtained', 'title': 'Marks Obtained', 'className': 'text-left' },
                    { 'data': 'RuralUrban', 'title': 'Rural/Urban', 'className': 'text-left' },
                    { 'data': 'Weightage', 'title': 'Weightage', 'className': 'text-left' },
                    { 'data': 'TotalMarks', 'title': 'Total Marks', 'className': 'text-left' },
                    { 'data': 'Percentage', 'title': 'Percentage', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Result', 'title': 'Result', 'className': 'text-left' }
                ]
            });

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
function GetFileTypes() {
    $("#Filetypes").empty();
    $("#Filetypes").append('<option value="choose">choose</option>');
    $("#FiletypesEdit").empty();
    $("#FiletypesEdit").append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetDocumentTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Filetypes").append($("<option/>").val(this.docTypeId).text(this.docType));
                    $("#FiletypesEdit").append($("<option/>").val(this.docTypeId).text(this.docType));
                });
            }
        }, error: function (result) {           
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function AddOnemoreFile() {
    var $tableBody = $('#GrievanceData').find("tbody");
    var $trLast = $tableBody.find("tr:last");
    var $trNew = $trLast.clone();
    $trNew.find("#Filetypes").val('choose');
    $trNew.find("#uploadfile").val('');
    $trNew.find("#remove").click(function () {
        var lenght = $('#GrievanceData tbody tr').length;
        if (lenght > 1) {
            $(this).closest("tr").remove();
            $("#GrievanceData").find(".text-multi-units").each(function () {
                total += parseFloat($(this).text())
            });
        }
        else {
            bootbox.alert("Atleast one row required")
        }
    });
    $tableBody.append($trNew);
}
$("#remove").click(function () {
    var lenght = $('#GrievanceData tbody tr').length;
    if (lenght > 1) {
        $(this).closest("tr").remove();
    }
    else {
        bootbox.alert("Atleast one row required");
    }
});
function SubmitGrievance() {
    if ($("#Filetypes").val() == "choose") {
        bootbox.alert("select the file type");
    }
    else
        if ($('#uploadfile').val() == "") {
           bootbox.alert("please upload the file");
        }
        else {
            bootbox.confirm({
                message: " Do you want to submit the grievance? ",
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
                    $('#submitbtn').attr('disabled', 'disabled');
                    var remarks = $('#remarks').val();
                    var shift_table = $("#GrievanceData tbody");

                    var Data = new FormData();

                    shift_table.find("tr").each(function (len) {
                        var $tr = $(this);
                        var fileType = $tr.find("#Filetypes").val();
                        var file = $tr.find("input[type=file]")[0].files[0];
                        if (fileType != 'choose' && file != undefined) {
                            Data.append('list', file);
                            Data.append('fileType', fileType);

                        }
                    });
                    Data.append('remarks', remarks);
                    $.ajax({
                        url: '/Admission/SubmitGrievanceTentative',
                        dataType: 'json',
                        type: 'POST',
                        data: Data,
                        processData: false,
                        contentType: false,
                        success: function (data) {
                            if (data != "failed") {
                                var toMatch = " - ";
                                bootbox.alert("Applicant has submitted the grievance against tentative gradation list successfully to verification officer and your Grievance number is <b>" + data.split(toMatch)[0] +
                                    "</b> ,Please carry the original documents which you need to correct to the previous verification officer before grievance due date");
                                ClearGrievance();
                                GetLoadApplicantGrievanceGrid();
                                GetGrievanceTentativeStatus();
                            }
                            else {
                                bootbox.alert("Grievance against tentative gradation list submition failed");
                            }
                        },
                        error: function (res) {
                            bootbox.alert("Grievance submition failed");
                        }
                    });
                    }
                }
            });
        }
/*code commented by sujit*/
        //else {
        //    bootbox.confirm('Do you want to submit the grievance?', (confirma) => {
        //        if (confirma) {
        //            $('#submitbtn').attr('disabled', 'disabled');
        //            var remarks = $('#remarks').val();
        //            var shift_table = $("#GrievanceData tbody");

        //            var Data = new FormData();

        //            shift_table.find("tr").each(function (len) {
        //                var $tr = $(this);
        //                var fileType = $tr.find("#Filetypes").val();
        //                var file = $tr.find("input[type=file]")[0].files[0];
        //                if (fileType != 'choose' && file != undefined) {
        //                    Data.append('list', file);
        //                    Data.append('fileType', fileType);

        //                }
        //            });
        //            Data.append('remarks', remarks);
        //            $.ajax({
        //                url: '/Admission/SubmitGrievanceTentative',
        //                dataType: 'json',
        //                type: 'POST',
        //                data: Data,
        //                processData: false,
        //                contentType: false,
        //                success: function (data) {
        //                    if (data != "failed") {
        //                        bootbox.alert("Applicant has submitted the grievance against tentative gradation list successfully to verification officer and your Grievance number is <b>" + data +
        //                            "</b> , Go with original documents  which you need to correct, to the previous verification officer . ");
        //                        ClearGrievance();
        //                        GetLoadApplicantGrievanceGrid();
        //                        GetGrievanceTentativeStatus();
        //                    }
        //                    else {
        //                        bootbox.alert("Grievance against tentative gradation list submition failed");
        //                    }
        //                },
        //                error: function (res) {
        //                    bootbox.alert("Grievance submition failed");
        //                }
        //            });

        //        }
        //    });
        //}
}
function ClearGrievance() {
    $('#gribody tr:not(:first)').remove();
    $('#Filetypes').val('choose');
    $('#remarks').val('');
    $('#uploadfile').val('');
}
//============END Grievance Status tab 1============
//========= START Grievance Status tab 2================
function GetGrievanceTentativeStatus() {
    if ($('#CourseType').val() == 'choose' || $('#CourseType').val() == undefined) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetGrievanceTentativeStatus",
            contentType: "application/json",
            data: { 'course': 0, 'year': 0, 'division': 0, 'district': 0, 'applicantType': 0, 'taluk': 0, 'institute': 0 },
            success: function (data) {
                $('#ApplicantGrievanceStatus').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    "paging": false,
                    "ordering": false,
                    "info": false,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                        
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left ITIName' },
                        { 'data': 'Divisionname', 'title': 'Division', 'className': 'text-left division' },
                        { 'data': 'Districtname', 'title': 'District', 'className': 'text-left district' },
                        { 'data': 'Talukname', 'title': 'Taluk Name', 'className': 'text-left district' },
                        { 'data': 'OfficerName', 'title': 'VO Name', 'className': 'text-left VOName' },
                        { 'data': 'StatusName', 'title': 'Status' },                        
                        {
                            'data': 'GrievanceId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetGrievanceRemarks(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                            }
                        },
                        {
                            'data': 'GrievanceId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.RoleId == 10 && oData.StatusId != 13 && oData.StatusId != 3) {
                                    if (oData.From != null) {
                                        var frmDate = oData.From.split(',');
                                        var toDate = oData.To.split(',');
                                        var dte = new Date();
                                        var d = dte.getUTCDate();
                                        var m = dte.getUTCMonth();
                                        var y = dte.getUTCFullYear();
                                        
                                        if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2])) && new Date(y, m, d) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
                                            if (oData.StatusId != 11) {
                                                $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                            }
                                            else {
                                                $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                            }
                                        }
                                        else {
                                            $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                        }      
                                    }
                                    else {
                                        //$(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                        $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                    }                            
                                }
                                else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 12 || oData.StatusId == 13 || oData.StatusId == 3)) {
                                    //$(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                        }
                    ]
                });
                
                if (data.length > 0) {
                if (data[0].RoleId == 16) {
                    $(".division").css("display", "none");                  
                }
                else if (data[0].RoleId == 12 || data[0].RoleId == 10) {
                    $(".VOName").css("display", "none");
                    $(".ITIName").css("display", "none");   
                    $(".division").css("display", "none");
                    $(".district").css("display", "none");
                }
                else {
                    $(".division").css("display", "");
                    }
                }
            }
        });
    } else {
        var course = $('#CourseType').val();
        var year = $('#Session').val();
        var applicantType = $('#applicantType').val();
        if (year == 'choose') {
           // bootbox.alert('select the session');
        }
        else if (applicantType == 'choose') {
            bootbox.alert('select the applicant type');
        }
        else {
            $.ajax({
                type: "GET",
                url: "/Admission/GetGrievanceTentativeStatus",
                contentType: "application/json",
                data: { 'course': course, 'year': year, 'division': 0, 'district': 0, 'applicantType': applicantType, 'taluk': 0, 'institute': 0 },
                success: function (data) {
                    $('#ApplicantGrievanceStatus').DataTable({
                        data: data,
                        "destroy": true,
                        "bSort": true,
                        columns: [
                            { 'data': 'slno', 'title': 'Sl.no', 'className': 'text-center' },
                            { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                            { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                            { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
                            { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                            { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                            
                            { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                            { 'data': 'Districtname', 'title': 'District', 'className': 'text-left' },
                            { 'data': 'OfficerName', 'title': 'VO Name', 'className': 'text-left' },
                            { 'data': 'StatusName', 'title': 'Status' },
                            {
                                'data': 'GrievanceId',
                                'title': 'Remarks',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    $(nTd).html("<input type='button' onclick='GetGrievanceRemarks(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            },
                            {
                                'data': 'GrievanceId',
                                'title': 'Action',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    if (oData.RoleId == 10 && oData.StatusId != 13 && oData.StatusId != 3) {
                                        if (oData.From != null) {
                                            var frmDate = oData.From.split(',');
                                            var toDate = oData.To.split(',');
                                            var dte = new Date();
                                            var d = dte.getUTCDate();
                                            var m = dte.getUTCMonth();
                                            var y = dte.getUTCFullYear();

                                            if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2])) && new Date(y, m, d) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
                                                $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                            }
                                            else {
                                                $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                            }       
                                        }
                                        else{
                                            $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                        } 
                                    }
                                    else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 13 || oData.StatusId == 3)) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }
                                    else {
                                        $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }
                                }
                            }
                        ]
                    });
                }
            });
        }

    }

}
function GetGrievanceTentativeStatusForDD() { 
  var isValid = true;
  
  var hdnRoleIdsessionValue = $("#hdnRoleId").data('value');
  var SessionVal =0;
  var ApplicantType = 0;
  var district = 0;
  var division = 0;
  if (hdnRoleIdsessionValue == "11") {    
    ApplicantType = $('#ApplicantType123').val();
    district = $('#district123').val();
    SessionVal = $('#Session').val();
    $('#Session1Err').text('');
    if (SessionVal == 'choose') {
      isValid = false;
      $('#Session1Err').text('select the session');
      //bootbox.alert('Please select the course type');
    }    
  }
  else if (hdnRoleIdsessionValue == "5") {
    SessionVal = $('#Session123').val();
   division = $('#division').val();
      district = $('#district').val();
      course = $('#Coursetyp').val();
      taluk = $('#talukverificationstatusdd').val();
      institute = $('#instituteverificationstatusdd').val();
   ApplicantType = $('#ApplicantType123').val();
    $('#Session2Err').text('');
    if (SessionVal == 'choose') {
      isValid = false;
      $('#Session2Err').text('select the session');
      //bootbox.alert('Please select the course type');
    }  
  }
  else if (hdnRoleIdsessionValue == "16") {
    ApplicantType = $('#ApplicantType123').val();    
    district = $('#district1').val();
    SessionVal = $('#Session123').val();
    $('#Session3Err').text('');
    if (SessionVal == 'choose') {
      isValid = false;
      $('#Session3Err').text('select the session');
      //bootbox.alert('Please select the course type');
    }   
  }
  if (isValid==true) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetGrievanceTentativeStatus",
            contentType: "application/json",
            data: { 'course': course, 'year': SessionVal, 'division': division, 'district': district, 'applicantType': ApplicantType, 'taluk': taluk, 'institute': institute },
            success: function (data) {
                $('#ApplicantGrievanceStatus').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.no', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
                            { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Status' },
                        { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                        {
                            'data': 'GrievanceId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.RoleId == 10 && oData.StatusName != "Verified and Updated" && oData.StatusName != "Rejected") {
                                    $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                }
                                else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 13 || oData.StatusId == 3)) {
                                    $(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                        }
                    ]
                });
            }
        });
  } else {
    bootbox.alert('Please check the input fields');
    return false;
        //var applicantType = $('#ApplicantType123').val();
        //var year = $('#Session123').val();
        //var division = $('#division').val();
        //var district = $('#district').val();
        //if (year == 'choose') {
        //    bootbox.alert('select the session');
        //}
        //else if (division == 'choose') {
        //    bootbox.alert('select the division');
        //}
        //else if (district == 'choose') {
        //    bootbox.alert('select the district');
        //}
        //else {
        //    $.ajax({
        //        type: "GET",
        //        url: "/Admission/GetGrievanceTentativeStatus",
        //        contentType: "application/json",
        //        data: { 'course': 0, 'year': year, 'division': division, 'district': district, 'applicantType': applicantType },
        //        success: function (data) {
        //            $('#ApplicantGrievanceStatus').DataTable({
        //                data: data,
        //                "destroy": true,
        //                "bSort": true,
        //                columns: [
        //                    { 'data': 'slno', 'title': 'Sl.no', 'className': 'text-center' },
        //                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
        //                    { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
        //                    { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
        //                    { 'data': 'ApplicantNumber', 'title': 'Applicant Number', 'className': 'text-center' },
        //                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
        //                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
        //                    { 'data': 'StatusName', 'title': 'Status' },
        //                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
        //                    {
        //                        'data': 'GrievanceId',
        //                        'title': 'Action',
        //                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
        //                            if (oData.RoleId == 10 && oData.StatusName != "Verified and Updated" && oData.StatusName != "Rejected") {
        //                                $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
        //                            }
        //                            else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 13 || oData.StatusId == 3)) {
        //                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
        //                            }
        //                            else {
        //                                $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
        //                            }
        //                        }
        //                    }
        //                ]
        //            });
        //        }
        //    });
        //}

    }

}
function GetGrievanceTentativeStatusForDvision() {
    
    if ($('#ApplicantType123').val() == 'choose' || $('#ApplicantType123').val() == undefined) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetGrievanceTentativeStatus",
            contentType: "application/json",
            data: { 'course': 0, 'year': 0, 'division': 0, 'district': 0, 'applicantType': 0, 'taluk': 0, 'institute': 0 },
            success: function (data) {
                $('#ApplicantGrievanceStatus').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.no', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Status' },
                        { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                        {
                            'data': 'GrievanceId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.RoleId == 10 && oData.StatusName != "Verified and Updated" && oData.StatusName != "Rejected") {
                                    $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                }
                                else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 13 || oData.StatusId == 3)) {
                                    $(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                        }
                    ]
                });
            }
        });
    } else {
        var apptype = $('#ApplicantType123').val();
        var year = $('#Session').val();
        var district = $('#districtverificationstatusGR').val();
        var taluk = $('#talukverificationstatusGR').val();
        var clg = $('#instituteverificationstatus').val();
        var div = $("#hdnDiv_ID").data('value')
        if (year == 'choose') {
            bootbox.alert('select the session');
        }
        else if (district == 'choose') {
            bootbox.alert('select the district');
        }
        else {
            $.ajax({
                type: "GET",
                url: "/Admission/GetGrievanceTentativeStatus",
                contentType: "application/json",
                data: { 'course': 0, 'year': year, 'division': div, 'district': district, 'applicantType': apptype, 'taluk': taluk, 'institute': clg },
                success: function (data) {
                    $('#ApplicantGrievanceStatus').DataTable({
                        data: data,
                        "destroy": true,
                        "bSort": true,
                        columns: [
                            { 'data': 'slno', 'title': 'Sl.no', 'className': 'text-center' },
                            { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                            { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                            { 'data': 'GrievanceRefNumber', 'title': 'Grievance Number', 'className': 'text-center' },
                            { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                            { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                            { 'data': 'StatusName', 'title': 'Status' },
                            { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                            {
                                'data': 'GrievanceId',
                                'title': 'Action',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    if (oData.RoleId == 10 && oData.StatusName != "Verified and Updated" && oData.StatusName != "Rejected") {
                                        $(nTd).html("<input type='button' onclick='EditApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-danger btn-xs' value='View' id='View'/>");
                                    }
                                    else if (oData.RoleId == 12 && (oData.StatusId == 11 || oData.StatusId == 13 || oData.StatusId == 3)) {
                                        $(nTd).html("<input type='button' onclick='GetApplicationDetailsGriById(" + oData.ApplicationId + "," + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='Edit' id='edit' />&nbsp;&nbsp;<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }
                                    else {
                                        $(nTd).html("<input type='button' onclick='ViewApplicantGrievance(" + oData.GrievanceId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }
                                }
                            }
                        ]
                    });
                }
            });
        }

    }

}
function EditApplicantGrievance(grivanceId) {
    $('#headtitle').text('Grievance against Tentative gradation Edit');
    $('#add').show();
    $('#GrievanceId').val(grivanceId);
    $('#ApplicantNumber').text('');
    $('#ApplicantName').text('');
    $('#Rank').text('');
    $('#StatusName').text('');
    $('#Remarks').text('');

    $('#remarksEdit').show();
    $('#add').attr('disabled', false);
    $('#removeEdit').attr('disabled', false);

    $('#update').show();
    $('#verRemarks').hide();
    $('#verifiedbtn').hide();
    $('#sendbtn').hide();
    $('#rejectbtn').hide();
    
    $.ajax({
        type: "GET",
        url: "/Admission/EditApplicantGrievance",
        contentType: "application/json",
        data: { 'grivanceId': grivanceId },
        success: function (data) {
            $('#grivancNumber').val('');
            $('#grivancNumber').val(data.GrievanceRefNumber);
            var aa = new Array();
            aa.push(data);
            $('#ApplicantGrievanceEdit').DataTable({
                data: aa,
                "destroy": true,
                "bSort": true,
                "paging": false,
                "ordering": false,
                "info": false,
                bFilter: false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    {
                        'data': 'DOB', 'title': 'DOB', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'Category', 'title': 'Category', 'className': 'text-left' },
                    { 'data': 'ExService', 'title': 'Ex-Serviceman', 'className': 'text-left' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with disability', 'className': 'text-left' },
                    { 'data': 'EconomicWeekerSec', 'title': 'EWS (Economic Weaker Section)', 'className': 'text-left' },
                    { 'data': 'HydKar', 'title': 'Hydrabad/Karnataka', 'className': 'text-left' },
                    { 'data': 'KannadaMedium', 'title': 'KannadaMedium', 'className': 'text-left' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left' },
                    { 'data': 'MarksObtained', 'title': 'Marks Obtained', 'className': 'text-left' },
                    { 'data': 'RuralUrban', 'title': 'Rural/Urban', 'className': 'text-left' },
                    { 'data': 'Weightage', 'title': 'Weightage', 'className': 'text-left' },
                    { 'data': 'TotalMarks', 'title': 'Total Marks', 'className': 'text-left' },
                    { 'data': 'Percentage', 'title': 'Percentage', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Result', 'title': 'Result', 'className': 'text-left' }
                ]
            });
            if (data != null) {                
                $("#tablebody").empty();
                var content = "<tr>"
                var id = 1;
                for (var i = 0; i < data.FileNames.length; i++) {
                    var select = $('<select></select>').attr('id', "FiletypesEdit").attr('class', "form-control selct" + id + "").prop('disabled', false).append('<option value="choose">choose</option >');
                    for (var j = 0; j < data.Doctypes.length; j++) {
                        select.append($("<option></option>").attr("value", data.Doctypes[j].DocTypeId).text(data.Doctypes[j].DoctypeName));
                    }
                    var input = $('<input/>').attr('type', "file").attr('class', "form-control").attr('accept', ".jpg,.pdf").attr('id', "FileName").attr('disabled', false);
                    var anchor = $('<a>').attr('id', "File").attr('value', "File").attr('href', data.Files[i]).attr('target', "new").attr('class', 'anchor').html("<img style='width:40px;height:40px;' src='/Content/img/iconpdf.png'></img>");
                    var select1 = $('<select></select>').attr('id', "Status").attr('class', "form-control");
                    if (data.FileStatus[i] == "Approved") {
                        select1.append($("<option></option>").attr("value", data.FileStatus[i]).text(data.FileStatus[i])).prop('disabled', 'disabled');
                    } else if (data.FileStatus[i] == "Rejected") {
                        select1.append($("<option></option>").attr("value", data.FileStatus[i]).text(data.FileStatus[i]));                        
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                    } else if (data.FileStatus[i] == "Send for correction") {
                        select1.append($("<option></option>").attr("value", data.FileStatus[i]).text(data.FileStatus[i]));                        
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                    }
                    else {
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));                        
                    }
                    var btn = $('<input/>').attr('type', "button").attr('id', "removeEdit").attr('class', "btn btn-danger removeEditBtn" + id + "").attr('value', "X");
                    content += '<td style="padding:5px;" id="grivance' + id + '"></td><td style="padding:5px;" id="msg' + id + '"></td><td style="padding:5px;" id="input' + id + '"></td><td style="padding:5px;" id="link' + id + '"></td><td style="padding:5px;width:120px;" id="status' + id + '"></td><td id="removebtn' + id + '"></td></tr>';
                    $('#tablebody').append(content);
                    $('#msg' + id).html(select);
                    $('#input' + id).html(input);
                    $('#link' + id).html(anchor).append('<style>.anchor{color:green;}</style>');
                    $('#grivance' + id).html('<b>Grievance</b>');
                    $('#removebtn' + id).html(btn);
                    $('.selct' + id).val(data.FileTypes[i]);
                    $('#status' + id).html(select1);
                    id++;
                }
                $('#gridlist').hide();
                $('#EditviewDetails').show();
            }
            else {
                bootbox.alert('faied to load');
            }
        },
        error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    })
}
function ViewApplicantGrievance(grivanceId) {
    $('#headtitle').text('Grievance against Tentative gradation View');
    $('#add').hide();
    $('#GrievanceId').val(grivanceId);
    $('#ApplicantNumber').text('');
    $('#ApplicantName').text('');
    $('#Rank').text('');
    $('#StatusName').text('');
    $('#Remarks').text('');
    $.ajax({
        type: "GET",
        url: "/Admission/EditApplicantGrievance",
        contentType: "application/json",
        data: { 'grivanceId': grivanceId },
        success: function (data) {
            $('#grivancNumber').val('');
            $('#grivancNumber').val(data.GrievanceRefNumber);
            var aa = new Array();
            aa.push(data);
            $('#ApplicantGrievanceEdit').DataTable({
                data: aa,
                "destroy": true,
                "bSort": true,
                "paging": false,
                "ordering": false,
                "info": false,
                bFilter: false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-left' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
                    {
                        'data': 'DOB', 'title': 'DOB', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB, 1);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'Category', 'title': 'Category', 'className': 'text-left' },
                    { 'data': 'ExService', 'title': 'Ex-Serviceman', 'className': 'text-left' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with disability', 'className': 'text-left' },
                    { 'data': 'EconomicWeekerSec', 'title': 'EWS (Economic Weaker Section)', 'className': 'text-left' },
                    { 'data': 'HydKar', 'title': 'Hydrabad/Karnataka', 'className': 'text-left' },
                    { 'data': 'KannadaMedium', 'title': 'KannadaMedium', 'className': 'text-left' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left' },
                    { 'data': 'MarksObtained', 'title': 'Marks Obtained', 'className': 'text-left' },
                    { 'data': 'RuralUrban', 'title': 'Rural/Urban', 'className': 'text-left' },
                    { 'data': 'Weightage', 'title': 'Weightage', 'className': 'text-left' },
                    { 'data': 'TotalMarks', 'title': 'Total Marks', 'className': 'text-left' },
                    { 'data': 'Percentage', 'title': 'Percentage', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Result', 'title': 'Result', 'className': 'text-left' }
                ]
            });
            if (data != null) {
                $("#tablebody").empty();

                var content = "<tr>"
                var id = 1;
                for (var i = 0; i < data.FileNames.length; i++) {
                    var select = $('<select></select>').attr('id', "FiletypesEdit").attr('class', "form-control").prop('disabled', 'disabled');
                    select.append($("<option></option>").attr("value", data.FileTypes[i]).text(data.DocNames[i]));
                    var input = $('<input/>').attr('type', "file").attr('class', "form-control").attr('accept', ".jpg,.pdf").attr('id', "FileName").attr('disabled', true);
                    var anchor = $('<a>').attr('id', "File").attr('value', "File").attr('href', data.Files[i]).attr('target', "new").attr('class', 'anchor').html("<img style='width:40px;height:40px;' src='/Content/img/iconpdf.png'></img>");
                    if (data.RoleId == 12) {
                        if (data.FileStatus[i] == "Approved") {
                            var select1 = $('<select></select>').attr('id', "StatusEdit").attr('class', "form-control").prop('disabled', 'disabled');
                        } else {
                            var select1 = $('<select></select>').attr('id', "StatusEdit").attr('class', "form-control");
                        }
                    } else {
                        var select1 = $('<select></select>').attr('id', "StatusEdit").attr('class', "form-control").prop('disabled', 'disabled');
                    }
                    if (data.FileStatus[i] == "Rejected") {
                        select1.append($("<option></option>").attr("value", "Rejected").text("Rejected"));
                        select1.append($("<option></option>").attr("value", "Approved").text("Approved"));
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                        select1.append($("<option></option>").attr("value", "Send for correction").text("Send for correction"));
                    }
                    else if (data.FileStatus[i] == "Uploaded") {
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                        select1.append($("<option></option>").attr("value", "Send for correction").text("Send for correction"));
                        select1.append($("<option></option>").attr("value", "Approved").text("Approved"));
                        select1.append($("<option></option>").attr("value", "Rejected").text("Rejected"));
                    } else if (data.FileStatus[i] == "Approved") {
                        select1.append($("<option></option>").attr("value", "Approved").text("Approved"));
                        select1.append($("<option></option>").attr("value", "Rejected").text("Rejected"));
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                        select1.append($("<option></option>").attr("value", "Send for correction").text("Send for correction"));
                    } else if (data.FileStatus[i] == "Send for correction") {
                        select1.append($("<option></option>").attr("value", "Send for correction").text("Send for correction"));
                        select1.append($("<option></option>").attr("value", "Rejected").text("Rejected"));
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                        select1.append($("<option></option>").attr("value", "Approved").text("Approved"));
                    }
                    else {
                        select1.append($("<option></option>").attr("value", "Uploaded").text("Uploaded"));
                        select1.append($("<option></option>").attr("value", "Approved").text("Approved"));
                        select1.append($("<option></option>").attr("value", "Rejected").text("Rejected"));
                        //select1.append($("<option></option>").attr("value", "Send for correction").text("Send for correction"));
                    }
                    content += '<td style="padding:5px;" id="grivance' + id + '"></td><td style="padding:5px;" id="msg' + id + '"></td><td style="padding:5px;" id="input' + id + '"></td><td style="padding:5px;" id="link' + id + '"></td><td style="padding:5px;width:120px;" id="statusView' + id + '"></td></tr>';
                    $('#tablebody').append(content);
                    var ss = $('#msg' + id).html(select);
                    var iin = $('#input' + id).html(input);
                    var lin = $('#link' + id).html(anchor).append('<style>.anchor{color:green;}</style>');
                    $('#grivance' + id).html('<b>Grievance</b>');
                    iin.val(data.FileNames[i]);
                    $('#statusView' + id).html(select1);
                    id++;

                }
                //$('#remarksEdit').val(data.Remarks);
                $('#remarksEdit').hide();
                $('#add').attr('disabled', true);
                $('#removeEdit').attr('disabled', true);
                $('#update').hide();
                $('#gridlist').hide();
                $('#EditviewDetails').show();
                if (data.RoleId == 12 && data.StatusId != 13) {
                    $('#verRemarks').show();
                    $('#verifiedbtn').show();
                    //$('#sendbtn').show();
                    $('#rejectbtn').show();
                    if (data.StatusId == 12 || data.StatusId == 3) {
                        $('#verRemarks').attr('disabled', true);
                        $('#verifiedbtn').attr('disabled', true);
                       // $('#sendbtn').attr('disabled', true);
                        $('#rejectbtn').attr('disabled', true);
                    }
                    else {
                        $('#verRemarks').attr('disabled', false);
                        $('#verifiedbtn').attr('disabled', false);
                        //$('#sendbtn').attr('disabled', false);
                        $('#rejectbtn').attr('disabled', false);
                    }
                }
                else {
                    $('#verRemarks').attr('disabled', true);
                    $('#verifiedbtn').attr('disabled', true);
                    //$('#sendbtn').attr('disabled', true);
                    $('#rejectbtn').attr('disabled', true);
                }
            }
            else {
                bootbox.alert('faied to load');
            }
        },
        error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    })
}
function AddOnemoreFileEdit() {
    var $tableBody = $('#GrievanceData1').find("tbody");
    var $trLast = $tableBody.find("tr:last");
    var $trNew = $trLast.clone();
    $trNew.find("#FiletypesEdit").val('choose');
    $trNew.find("#uploadfileEdit").val('');
    $trNew.find("#removeEdit").click(function () {
        var lenght = $('#GrievanceData1 tbody tr').length;
        if (lenght > 1) {
            $(this).closest("tr").remove();
            //$("#FiletypesEdit option[value='1']").remove();
            $("#GrievanceData1").find(".text-multi-units").each(function () {
                total += parseFloat($(this).text())
            });
        }
        else {
            bootbox.alert("Atleast one row required")
        }
    });
    $trNew.find("#File").text('');

    $tableBody.append($trNew);
}
function CancelGrievance() {
    $('#EditviewDetails').hide();
    $('#gridlist').show();
}
function VerifyGrievance() {
    bootbox.confirm({
        message: "Please Update the applicant details and relevant grievance documents in the application form after verify the grievance.",
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
            var remark = $('#verRemarks').val();
            grivanceId = $('#GrievanceId').val();

            var table = $("#GrievanceData1 tbody");

            var Data = new FormData();

            table.find("tr").each(function (len) {
                var $tr = $(this);
                var fileType = $tr.find("#FiletypesEdit").val();
                var status = $tr.find("#StatusEdit").val();
                if (status != undefined && fileType != undefined) {
                    Data.append('status', status);
                    Data.append('fileType', fileType);
                }
            });
            Data.append('grivanceId', grivanceId);
            Data.append('remarks', remark);

            $.ajax({
                type: 'POST',
                url: "/Admission/VerifyGrievance",
                contentType: "json",
                data: Data,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data = true) {
                        GetGrievanceTentativeStatus();
                        bootbox.alert("Verification officer has verified Grivance No " + $('#grivancNumber').val() + " grievance against tentative gradation list successfully");
                        $('#EditviewDetails').hide();
                        $('#gridlist').show();

                    }
                    else {
                        bootbox.alert("Grivance No " + $('#grivancNumber').val() + " verified failed");
                    }
                },
                error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
        }
    });

/*code commented by sujit*/
    //bootbox.confirm('Please Update the applicant details and relevant grievance documents in the application form before verify the grievance if you updated the details then continue with OK', (confirma) => {
    //    if (confirma) {
    //        var remark = $('#verRemarks').val();
    //        grivanceId = $('#GrievanceId').val();

    //        var table = $("#GrievanceData1 tbody");

    //        var Data = new FormData();

    //        table.find("tr").each(function (len) {
    //            var $tr = $(this);
    //            var fileType = $tr.find("#FiletypesEdit").val();
    //            var status = $tr.find("#StatusEdit").val();
    //            if (status != undefined && fileType != undefined) {
    //                Data.append('status', status);
    //                Data.append('fileType', fileType);
    //            }
    //        });
    //        Data.append('grivanceId', grivanceId);
    //        Data.append('remarks', remark);

    //        $.ajax({
    //            type: 'POST',
    //            url: "/Admission/VerifyGrievance",
    //            contentType: "json",
    //            data: Data,
    //            processData: false,
    //            contentType: false,
    //            success: function (data) {
    //                if (data = true) {
    //                    GetGrievanceTentativeStatus();
    //                    bootbox.alert("Verification officer has verified Grivance No " + $('#grivancNumber').val() + " grievance against tentative gradation list successfully");
    //                    $('#EditviewDetails').hide();
    //                    $('#gridlist').show();

    //                }
    //                else {
    //                    bootbox.alert("Grivance No " + $('#grivancNumber').val() + " verified failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //            }
    //        });
    //    }
    //});
}
function SendForCorrection() {
    bootbox.confirm({
        message: "Do you want to send it for correction the grievance ?",
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
            var remark = $('#verRemarks').val();
            grivanceId = $('#GrievanceId').val();

            var table = $("#GrievanceData1 tbody");

            var Data = new FormData();

            table.find("tr").each(function (len) {
                var $tr = $(this);
                var fileType = $tr.find("#FiletypesEdit").val();
                var status = $tr.find("#StatusEdit").val();
                if (status != undefined && fileType != undefined) {
                    Data.append('status', status);
                    Data.append('fileType', fileType);
                }
            });
            Data.append('grivanceId', grivanceId);
            Data.append('remarks', remark);
            $.ajax({
                type: "POST",
                url: "/Admission/SendForCorrection",
                contentType: "json",
                data: Data,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data = true) {
                        GetGrievanceTentativeStatus();
                        bootbox.alert("Verification officer has sent back for more clarification Grievance No " + $('#grivancNumber').val() + " grievance against gradation list successfully");
                        $('#EditviewDetails').hide();
                        $('#gridlist').show();
                    }
                    else {
                        bootbox.alert("Grievance No " + $('#grivancNumber').val() + " Send for correction is failed");
                    }
                },
                error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
        }
    });

/*code commented by sujit*/
    //bootbox.confirm('Do you want to send it for correction the grievance?', (confirma) => {
    //    if (confirma) {
    //        var remark = $('#verRemarks').val();
    //        grivanceId = $('#GrievanceId').val();

    //        var table = $("#GrievanceData1 tbody");

    //        var Data = new FormData();

    //        table.find("tr").each(function (len) {
    //            var $tr = $(this);
    //            var fileType = $tr.find("#FiletypesEdit").val();
    //            var status = $tr.find("#StatusEdit").val();
    //            if (status != undefined && fileType != undefined) {
    //                Data.append('status', status);
    //                Data.append('fileType', fileType);
    //            }
    //        });
    //        Data.append('grivanceId', grivanceId);
    //        Data.append('remarks', remark);
    //        $.ajax({
    //            type: "POST",
    //            url: "/Admission/SendForCorrection",
    //            contentType: "json",
    //            data: Data,
    //            processData: false,
    //            contentType: false,
    //            success: function (data) {
    //                if (data = true) {
    //                    GetGrievanceTentativeStatus();
    //                    bootbox.alert("Verification officer has sent back for more clarification Grievance No " + $('#grivancNumber').val() + " grievance against gradation list successfully");
    //                    $('#EditviewDetails').hide();
    //                    $('#gridlist').show();
    //                }
    //                else {
    //                    bootbox.alert("Grievance No " + $('#grivancNumber').val() + " Send for correction is failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //            }
    //        });
    //    }
    //});
}
function UpdateGrievance() {
    var valid = false;
    var shift_table = $("#GrievanceData1 tbody");
    var hdnRoleIdsessionValue = $("#hdnRoleId").data('value');
    shift_table.find("tr").each(function (len) {
        var $tr = $(this);
        var fileType = $tr.find("#FiletypesEdit").val();
        var status = $tr.find("#Status").val();
        if ($tr.find("#FileName").length != 0) {
            var file = $tr.find("input[type=file]")[0].files[0];
            if (fileType == 'choose' && file == undefined) {
                valid = false; 
                return false;
            }
            else {
                valid = true;
            }
        }
    });

    if ($("#FiletypesEdit").val() == "choose") {
        bootbox.alert("select the file type and upload the file");
    } else if (valid == false) {
        bootbox.alert("select the grievance data and upload the file");
    }
    else {
        bootbox.confirm({
            message: "Do you want to update the grievance data ?",
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
                var remarks = $('#remarksEdit').val();
                var shift_table = $("#GrievanceData1 tbody");

                var Data = new FormData();

                shift_table.find("tr").each(function (len) {
                    var $tr = $(this);
                    var fileType = $tr.find("#FiletypesEdit").val();
                    var status = $tr.find("#Status").val();
                    if ($tr.find("#FileName").length != 0) {
                        var file = $tr.find("input[type=file]")[0].files[0];
                        if (fileType != 'choose' && file != undefined) {
                            Data.append('list', file);
                            Data.append('fileType', fileType);
                            Data.append('status', status);
                        }
                    }
                });

                grivanceId = $('#GrievanceId').val();

                Data.append('remarks', remarks);
                Data.append('grievanceId', grivanceId);
                $.ajax({
                    url: '/Admission/UpdateGrievanceTentative',
                    dataType: 'json',
                    type: 'POST',
                    data: Data,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        if (data == true) {
                            if (hdnRoleIdsessionValue == 10) {
                                bootbox.alert("Applicant has updated Grievance No: " + $('#grivancNumber').val() + " grievance against tentative gradation list and submitted to verification officer successfully");
                            }
                            else
                            {
                                bootbox.alert("Verification Officer has updated Grievance No: " + $('#grivancNumber').val() + " grievance against tentative gradation list");
                            }
                                $('#EditviewDetails').hide();
                            $('#gridlist').show();
                            GetGrievanceTentativeStatus();
                        }
                        else {
                            bootbox.alert("Grievance No: " + $('#grivancNumber').val() + " update failed");
                        }
                    }
                });
            }
            }
        });
    }
/*code commented by sujit*/
    //else {
    //    bootbox.confirm('Do you want to update the grievance?', (confirma) => {
    //        if (confirma) {
    //            var remarks = $('#remarksEdit').val();
    //            var shift_table = $("#GrievanceData1 tbody");

    //            var Data = new FormData();

    //            shift_table.find("tr").each(function (len) {
    //                var $tr = $(this);
    //                var fileType = $tr.find("#FiletypesEdit").val();
    //                var status = $tr.find("#Status").val();
    //                if ($tr.find("#FileName").length != 0) {
    //                    var file = $tr.find("input[type=file]")[0].files[0];
    //                    if (fileType != 'choose' && file != undefined) {
    //                        Data.append('list', file);
    //                        Data.append('fileType', fileType);
    //                        Data.append('status', status);
    //                    }
    //                }
    //            });

    //            grivanceId = $('#GrievanceId').val();

    //            Data.append('remarks', remarks);
    //            Data.append('grievanceId', grivanceId);
    //            $.ajax({
    //                url: '/Admission/UpdateGrievanceTentative',
    //                dataType: 'json',
    //                type: 'POST',
    //                data: Data,
    //                processData: false,
    //                contentType: false,
    //                success: function (data) {
    //                    if (data == true) {
    //                        bootbox.alert("Applicant has updated Grievance No: " + $('#grivancNumber').val() + " grievance against tentative gradation list and submitted to verification officer successfully");
    //                        $('#EditviewDetails').hide();
    //                        $('#gridlist').show();
    //                        GetGrievanceTentativeStatus();
    //                    }
    //                    else {
    //                        bootbox.alert("Grievance No: " + $('#grivancNumber').val() + " update failed");
    //                    }
    //                }
    //            });
    //        }
    //    });
    //}
}
function Reject() {
    bootbox.confirm({
        message: "Do you want to reject the grievance ?",
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
            var remark = $('#verRemarks').val();
            grivanceId = $('#GrievanceId').val();
            $.ajax({
                type: "GET",
                url: "/Admission/RejectGrivance",
                contentType: "application/json",
                data: { 'grivanceId': grivanceId, 'remarks': remark },
                success: function (data) {
                    if (data = true) {
                        bootbox.alert("Verification officer has rejected Grievance No " + $('#grivancNumber').val() + " grievance against tentative gradation list successfuly");
                        $('#EditviewDetails').hide();
                        $('#gridlist').show();
                        GetGrievanceTentativeStatus();
                    }
                    else {
                        bootbox.alert("Grievance No " + $('#grivancNumber').val() + " reject failed");
                    }
                },
                error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
        }
    });
/*code commented by sujit*/
    //bootbox.confirm('Do you want to reject the grievance?', (confirma) => {
    //    if (confirma) {
    //        var remark = $('#verRemarks').val();
    //        grivanceId = $('#GrievanceId').val();
    //        $.ajax({
    //            type: "GET",
    //            url: "/Admission/RejectGrivance",
    //            contentType: "application/json",
    //            data: { 'grivanceId': grivanceId, 'remarks': remark },
    //            success: function (data) {
    //                if (data = true) {
    //                    bootbox.alert("Verification officer has rejected Grievance No " + $('#grivancNumber').val() + " grievance against tentative gradation list successfuly");
    //                    $('#EditviewDetails').hide();
    //                    $('#gridlist').show();
    //                    GetGrievanceTentativeStatus();
    //                }
    //                else {
    //                    bootbox.alert("Grievance No " + $('#grivancNumber').val() + " reject failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //            }
    //        });
    //    }
    //});
}
function GetDistrictsForDd(id, distId) {
    var diviId = $('#' + id).val();
    
  if (diviId != "choose") {
    GetDistrictsDDp(distId, diviId);
  }
  else {
    GetDistrictsDDp(distId, 0);
  }

}
function GetDistrictsd(id, dist) {
    GetDistrictsDDp(dist, id);
}
function GetGrievanceRemarks(id) {
    $('#GrievaceRemarksModal').modal('show');
    $.ajax({
        type: "Get",
        url: "/Admission/GetGrievanceRemarks",
        data: { id: id },
        success: function (data) {
            $('#grievanceNum').html('');
            var t = $('#RemarksTable').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Datestring', 'title': 'Date', 'className': 'text-left' },                    
                    { 'data': 'From', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Remarks Description', 'className': 'text-left' }
                ]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            $('#grievanceNum').html(data[0].GrievanceRefNumber);
        }
    });
}
//filter
function GetApplicantType123(type) {
    $("#" + type).empty();
    $("#" + type).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetApplicantType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + type).append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                });
            }
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
//============END Grievance Status tab 2============