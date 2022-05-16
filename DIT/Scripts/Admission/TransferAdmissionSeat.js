
$(document).ready(function () {
    if (RoleId != null) {
        if (RoleId == 9) {
            $('#tab_1').show();
        }
        else {
            $('#tab_1').hide();
            $('#tab_2').attr("class", "in active");

        }
    }
    GetSessionYear("session");
    GetSessionYear("Session2");
    GetSessionYear("SessionTby");
    GetSessionYear("SessionAT");
    GetCourses("CourseTypes");
    GetCourses("CourseType2");
    GetCourses("CourseTypeTby");
    GetCourses("CourseTypeAT");
    GetTrades("Trades");
    GetTrades("TradesTby");
    //GetForwardRoles("users", 2);
    GetRoles("rolesfwd", 100);
    GetRoles("rolesback", 9);
    GetDistrictsDDp('districtDdp', 0);
    GetDivisionsDDp('divisionDdp');
    //GetRounds("session");


    GetDivisionsDDp('DivisionName2');
    GetInstituteTypes('InstituteType2');
    GetAdmittedDataStatus();
    GetRoles("users", 16);
    GetStatus1('status');
    $('#approve').hide();
    $('#remarksDiv').hide();
    $('#submitDiv').hide();
   /* $('#divApplicantAdmittedData').hide();*/
    GetApplicantTransferbyList();
    GetApprovedTransferList();
});

function GetAdmittedData() {
    $('#sessionErr').text('');
    $('#coursetypeErr').text('');
    $('#tradeErr').text('');
    $('#remarksDiv').hide();
    $('#submitDiv').hide();
    var year = $('#session').val();
    var course = $('#CourseTypes').val();
    var tarde = $('#Trades').val();
                
        if ($('#session').val() == 'choose') {
        $('#sessionErr').text('select the academic year');
        $('#divApplicantAdmittedData').hide();
                }
                else {
                    $.ajax({
                        type: "GET",
                        url: "/Admission/GetAdmittedData",
                        data: { 'session': year, 'course': course, 'trade': tarde, 'round': 0 },
                        contentType: "application/json",
                        success: function (data) {
                            $('#ApplicantAdmittedDataTable').DataTable({
                                data: data,
                    searching: true,
                    "bFilter": false,
                                "destroy": true,
                                "bSort": true,
                    dom: 'Bfrtip',
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download Excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                                //columns: ':visible',
                            }
                        }
                    ],
                                columns: [
                                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                                    { 'data': 'YearSession', 'title': 'Session ', 'className': 'text-center' },
                                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                                    { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },                                    
                                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },                                    
                                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                                    { 'data': 'DualSystem', 'title': 'DualSystem', 'className': 'text-left' },
                        //{
                        //    'data': 'Session', 'title': 'Session', 'className': 'text-left',
                        //    "render": function (nTd, sData, oData, iRow, iCol) {
                        //        const date = new Date(oData.Session, oData.ApplyMonth, 1);
                        //        const month = date.toLocaleString('default', { month: 'short' });
                        //        const year = date.getFullYear();
                        //        return month + "/" + year + "-" + month + "/" + (year + parseInt(oData.TradeDuration))
                        //    },
                        //},
                                    { 'data': 'AdmissionStatusName', 'title': 'Admission Status', 'className': 'text-left' },
                                    {
                                        'data': 'Slno',
                                        'title': 'Select',
                                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                            if (oData.Select == true) {
                                                $(nTd).html("<input type='hidden' id='applicantId' value='" + oData.ApplicantId + "'/><input type='hidden' id='tradeId' value='" + oData.TradeId + "'/><input type='hidden' id='admitId' value='" + oData.ApplicantInstituId + "'/><input type='checkbox' name='seatcheck' id='seatcheck'/>");
                                            }
                                            else {
                                                $(nTd).html("<input type='hidden' id='applicantId' value='" + oData.ApplicantId + "'/><input type='hidden' id='tradeId' value='" + oData.TradeId + "'/><input type='hidden' id='admitId' value='" + oData.ApplicantInstituId + "'/><input type='checkbox' disabled='disabled' name='seatcheck' id='seatcheck'/>");
                                            }
                                        }
                                    }
                                ]
                            });

                            if (data.length > 0 && data.length < 6) {
                    $('#divApplicantAdmittedData').show();
                                $('#remarksDiv').show();
                                $('#submitDiv').show();
                            }
                else {
                    $('#divApplicantAdmittedData').show();
                    //bootbox.alert("No Data Available.");
                }
                        }, error: function (result) {
                            alert("Error", "something went wrong");
                        }
                    });
                }
    }

function GetRequestedDetails() {
    $('#remarksDiv').hide();
    $('#submitDiv').hide();
    $('#sessionErr').text('');
    $('#coursetypeErr').text('');
    $('#tradeErr').text('');
   
    if ($('#Session3 option:selected').text() == 'choose' && ($('#CourseTypes2').val() == '101' || $('#CourseTypes2').val() == null)
        && $('#Trades1').val() == 'choose') {
        $.ajax({
            type: "GET",
            url: "/Admission/GetRequestedDetails",
            data: { 'session': 0, 'course': 0, 'trade': 0 },
            contentType: "application/json",
            success: function (data) {
                $('#ApplicantAdmittedDataStatusTable').DataTable({
                    data: data,
                    "bDestroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'YearSession', 'title': 'Session', 'className': 'text-center' },
                        { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                        { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                        { 'data': 'Status', 'title': 'Status', 'className': 'text-left' },
                        //{ 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                        //{ 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                        //{ 'data': 'DualSystem', 'title': 'DualSystem', 'className': 'text-left' },
                        //{ 'data': 'AdmissionStatus', 'title': 'Admission Status', 'className': 'text-left' },
                        //{
                        //    'data': 'Slno',
                        //    'title': 'Select',
                        //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        //        $(nTd).html("<input type='hidden' id='applicantId' value='" + oData.ApplicantId + "'/><input type='hidden' id='tradeId' value='" + oData.TradeId + "'/><input type='hidden' id='admitId' value='" + oData.ApplicantInstituId + "'/><input type='checkbox' name='seatcheck' id='seatcheck'/>");
                        //    }
                        //}
                    ]
                });
                //enable if applicants count 5 or less
                if (data.length > 0 && data.length < 6) {
                    $('#remarksDiv').show();
                    $('#submitDiv').show();
                }
                //if (data.length == 0) {
                //    $('#remarksDiv').hide();
                //    $('#submitDiv').hide();
                //}
            }, error: function (result) {
                alert("Please select mandatory fields.");
            }
        });
    }
    else {

        if ($('#Session3 option:selected').text() == 'choose') {
            $('#sessionErr').text('select the session');
        } else
            if ($('#CourseTypes2 option:selected').text() == 'choose') {
                $('#coursetypeErr').text('select the course');
            } else
                if ($('#Trades1 option:selected').text() == 'choose') {
                    $('#tradeErr').html('select the trade');
                }
                else {
                    var year = $('#Session3').val();
                    var course = $('#CourseType2').val();
                    var tarde = $('#Trades1').val();
                    $.ajax({
                        type: "GET",
                        url: "/Admission/GetRequestedDetails",
                        data: { 'session': year, 'course': course, 'trade': tarde },
                        contentType: "application/json",
                        success: function (data) {

                            $('#ApplicantAdmittedDataStatusTable').DataTable({
                                data: data,
                                "bDestroy": true,
                                "bSort": true,
                                columns: [
                                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                                    { 'data': 'YearSession', 'title': 'Session', 'className': 'text-center' },
                                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                                    { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                                    { 'data': 'Status', 'title': 'Status', 'className': 'text-left' },
                                    //{
                                    //    'data': 'Slno',
                                    //    'title': 'Select',
                                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    //        $(nTd).html("<input type='hidden' id='applicantId' value='" + oData.ApplicantId + "'/><input type='hidden' id='tradeId' value='" + oData.TradeId + "'/><input type='hidden' id='admitId' value='" + oData.ApplicantInstituId + "'/><input type='checkbox' name='seatcheck' id='seatcheck'/>");
                                    //    }
                                    //}
                                ]
                            });
                            //enable if applicants count 5 or less
                            if (data.length > 0 && data.length < 6) {
                                $('#remarksDiv').show();
                                $('#submitDiv').show();
                            }
                            //if (data.length == 0) {
                            //    $('#remarksDiv').hide();
                            //    $('#submitDiv').hide();
                            //}
                        },
                        error: function (result) {
                            alert("Error", "something went wrong");
                        }
                    });
                }
    }
}

function SubmitTransferAdmissionSeat() {

    var remarks = $('#remarks').val();
    var listItem = [];
    var shift_table = $("#ApplicantAdmittedDataTable tbody");

    shift_table.find('tr').each(function (len) {
        var $tr = $(this);
        var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
        var applicantInstituteId = $tr.find("td input[id=admitId]").val();
        var tradeId = $tr.find("td input[id=tradeId]").val();
        var applicantId = $tr.find("td input[id=applicantId]").val();
        var shift = $tr.find("td:eq(11)").text();
        var unit = $tr.find("td:eq(10)").text();
        var dualSystem = $tr.find("td:eq(12)").text();
        var reginum = $tr.find("td:eq(3)").text();
        var list = {
            ApplicantId: applicantId,
            ApplicantInstituId: applicantInstituteId,
            AdmisRegiNumber: reginum,
            TradeId: tradeId,
            Shift: shift,
            Unit: unit,
            DualSystem: dualSystem,
            Remarks: remarks,
            RoleId: 0
        }
        if (chkbx == true) {
            listItem.push(list);
        }
    });
    if (listItem.length == 0) {
        bootbox.alert('please select the checkbox');
    }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to submit! ",
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
                    url: "/Admission/SubmitAdmittedData",
                    contentType: "application/json",
                    data: JSON.stringify(listItem),
                    success: function (data) {
                        if (data == 'success') {
                            bootbox.alert("<br><b> ITI Admin </b> has successfully Submitted the Seat transfer Request for ready to send");
                            GetAdmittedData();
                            GetAdmittedDataStatus();
                            $('#remarks').val('');
                        }
                        else {
                            bootbox.alert("failed");
                        }
                    },
                    error: function (result) {
                        bootbox.alert("failed");
                    }
                });
            }
            }
        });

        //bootbox.confirm("Are you sure you want to submit!", function (result) {
        //    if (result == true) {
        //        $.ajax({
        //            type: "POST",
        //            url: "/Admission/SubmitAdmittedData",
        //            contentType: "application/json",
        //            data: JSON.stringify(listItem),
        //            success: function (data) {
        //                if (data == 'success') {
        //                    bootbox.alert(" ITI Admin has successfully Submitted the Seat transfer data for ready to send");
        //                    GetAdmittedData();
        //                    GetAdmittedDataStatus();
        //                    $('#remarks').val('');
        //                }
        //                else {
        //                    bootbox.alert("failed");
        //                }
        //            },
        //            error: function (result) {
        //                bootbox.alert("failed");
        //            }
        //        });
        //    }
        //});
    }
}
function ResetTransferfields() {
    $('#remarks').val('');
    $('#users').val('choose');
}
function GetAdmittedDataStatus(isFilter) {
    var dataContent = { session: "", courseType: "", trade: "" };
    if (isFilter == 1) {      
        if ($("#Session3 option:selected").text() != "choose") {
            dataContent = {
                session: $("#Session3 option:selected").text(),
                courseType: $("#CourseType2 option:selected").text(), trade: $("#Trades1 option:selected").text()
            };
        }
        else
            $("#sessionErr1").text("Select session.");
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetAdmittedDataStatus",
        data: dataContent,
        contentType: "application/json",
        success: function (data) {
            $('#ApplicantAdmittedDataStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    }
                ],                  
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'YearSession', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    //{
                    //    'data': 'Session', 'title': 'Session', 'className': 'text-left',
                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(oData.Session, oData.ApplyMonth, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return month + "/" + year + "-" + month + "/" + (year + parseInt(oData.TradeDuration))
                    //    },
                    //},
                    { 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left' },
                    {
                        'data': 'Slno',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GettransferComments(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    },
                    {
                        'data': 'Slno',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.statuswith == 9)
                                $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-danger btn-xs' value='Edit' id='View'/>");
                            else
                            $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
function changebuttn(btn, sel, my) {
    if ($('#' + my).val() != 'choose') {
        $('#' + btn).attr('disabled', true);
        $('#' + sel).attr('disabled', true);
    }
    else {
        $('#' + btn).attr('disabled', false);
        $('#' + sel).attr('disabled', false);
    }
}

function GetSeatViewDetails(id) {
    $('#DivisionName2').val('choose').attr('disabled', false);
    $('#DistrictName2').val('choose').attr('disabled', false);
    $('#DistrictLgd2').val('');
    $('#TalukName2').val('choose').attr('disabled', false);
    $('#TalukLgd2').val('');
    $('#MISITICode2').val('').attr('disabled', false);
    $('#InstituteType2').val('choose').attr('disabled', false);
    $('#InstituteTypeName2').val('choose').attr('disabled', false);
    $('#TradeName2').val('choose').attr('disabled', false);
    $('#Unit2').val('choose').attr('disabled', false);
    $('#Shift2').val('choose').attr('disabled', false);
    $('#DualSystem2').val('').attr('disabled', false);
    $('#SeatViewModal').modal('show');

    $('#lblPresent').text("Present");
    $('#lblChange').text("Change Requested");
    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantInstituteDetails",
        contentType: "application/json",
        data: { 'id': id },
        success: function (data) {
            if (data != null) {
                if (data[0].RoleId != data[0].FlowId) {                    
                        $('#remarksDiv1').hide();                   
                }
                else {
                    if (data[0].StatusId == 2) {
                        $('#lblPresent').text("Previous");
                        $('#lblChange').text("Current");
                        $('#remarksDiv1').hide();
                        //$('#lblPresent').hide();
                        //$('#lblChangeReq').hide();
                        //$('#lblPrevious').show();
                        //$('#lblCurrentReq').show();      
                    }
                    else {
                        $('#remarksDiv1').show();                    
                        //$('#lblPresent').show();
                        //$('#lblChangeReq').show();             
                    }
                }

                if (data[0].RoleId != data[0].FlowId)
                    $('#currentrole').hide();
                else
                    $('#currentrole').show();

                if (data[0].FlowId == 9 && data[0].StatusId == 20 && data[0].RoleId == 9) {
                    
                    $('#alternateviewForforward').hide();
                    $('#alternateviewForupdate').show();
                    $('#AdmitId').val(data[0].AdmitId);
                    $('#ApplicationNum1').val(data[0].ApplicantNumber);
                    $('#ApplicationName1').val(data[0].ApplicantName); 
                    $('#AdmissionRegNum1').val(data[0].AdmisRegiNumber);
                    $('#DivisionName1').val(data[0].DivisionName);
                    $('#DistrictName1').val(data[0].DistrictName);
                    $('#DistrictLgd1').val(data[0].DistrictLgdCode);
                    $('#TalukName1').val(data[0].TalukName);
                    $('#TalukLgd1').val(data[0].TalukLgdCode);
                    $('#MISITICode1').val(data[0].MISCode);
                    $('#InstituteType1').val(data[0].InstituteType);
                    $('#InstituteName1').val(data[0].InstituteName);
                    $('#TradeName1').val(data[0].TradeName);
                    $('#Unit1').val(data[0].Unit);
                    $('#Shift1').val(data[0].Shift);
                    $('#DualSystem1').val(data[0].DualSystem);

                    $('#ApplicationNum2').val(data[0].ApplicantNumber);
                    $('#ApplicationName2').val(data[0].ApplicantName);
                    $('#AdmissionRegNum2').val(data[0].AdmisRegiNumber);
                }
                else if (data[0].FlowId == 9 && data[0].StatusId == 9 && data[0].RoleId == 9) {                    
                    $('#alternateviewForforward').hide();
                    $('#alternateviewForupdate').show();
                    $('#AdmitId').val(data[0].AdmitId);
                    $('#ApplicationNum1').val(data[0].ApplicantNumber);
                    $('#ApplicationName1').val(data[0].ApplicantName);
                    $('#AdmissionRegNum1').val(data[0].AdmisRegiNumber);
                    $('#DivisionName1').val(data[0].DivisionName);
                    $('#DistrictName1').val(data[0].DistrictName);
                    $('#DistrictLgd1').val(data[0].DistrictLgdCode);
                    $('#TalukName1').val(data[0].TalukName);
                    $('#TalukLgd1').val(data[0].TalukLgdCode);
                    $('#MISITICode1').val(data[0].MISCode);
                    $('#InstituteType1').val(data[0].InstituteType);
                    $('#InstituteName1').val(data[0].InstituteName);
                    $('#TradeName1').val(data[0].TradeName);
                    $('#Unit1').val(data[0].Unit);
                    $('#Shift1').val(data[0].Shift);
                    $('#DualSystem1').val(data[0].DualSystem);
                    $('#ApplicationNum2').val(data[0].ApplicantNumber);
                    $('#ApplicationName2').val(data[0].ApplicantName);
                    $('#AdmissionRegNum2').val(data[0].AdmisRegiNumber);
                    GetDivisionsEdit(data[1].DivisionIdEdit);
                    $('#DivisionName2').val(data[1].DivisionIdEdit);                    
                    GetDistrictsEdit(data[1].DistrictLgdCode, data[1].DivisionIdEdit);
                    $("#DistrictName2").val(data[1].DistrictLgdCode);
                    GetTaluksEdit(data[1].TalukLgdCode, data[1].DistrictLgdCode);
                    $('#TalukName2').val(data[1].TalukLgdCode);
                    GetInstituteTypesEdit(data[1].InstituteTypeId);
                    $('#InstituteType2').val(data[1].InstituteTypeId);
                    GetInstituteNamesEdit(data[1].InstituteTypeId, data[1].TalukLgdCode, data[1].ApplicantInstituId);
                    $('#InstituteTypeName2').val(data[1].ApplicantInstituId);
                    $('#MISITICode2').val(data[1].MISCode);
                    $('#DualSystem2').val(data[1].DualSystem);
                    GetTradeNameEdit(data[1].ApplicantInstituId, data[1].TradeId);
                    $('#TradeName2').val(data[1].TradeId);
                    GetUnitsEdit(data[1].ApplicantInstituId, data[1].TradeId, data[1].Unit);
                    $('#Unit2').val(data[1].Unit);
                    GetShiftsEdit(data[1].TradeId, data[1].ApplicantInstituId, data[1].Unit, data[1].Shift);
                    $('#Shift2').val(data[1].Shift);
                    //$('#DivisionName2').val(data[1].DivisionId).change();
                    //let dist=$('#DistrictName2').val();
                    
                }
                else {
                    
                    $('#alternateviewForupdate').hide();
                    $('#alternateviewForforward').show();
                    $('#AdmitId').val(data[0].AdmitId);
                    $('#ApplicationNum1').val(data[0].ApplicantNumber);
                    $('#ApplicationName1').val(data[0].ApplicantName);
                    $('#AdmissionRegNum1').val(data[0].AdmisRegiNumber);
                    $('#DivisionName1').val(data[0].DivisionName);
                    $('#DistrictName1').val(data[0].DistrictName);
                    $('#DistrictLgd1').val(data[0].DistrictLgdCode);
                    $('#TalukName1').val(data[0].TalukName);
                    $('#TalukLgd1').val(data[0].TalukLgdCode);
                    $('#MISITICode1').val(data[0].MISCode);
                    $('#InstituteType1').val(data[0].InstituteType);
                    $('#InstituteName1').val(data[0].InstituteName);
                    $('#TradeName1').val(data[0].TradeName);
                    $('#Unit1').val(data[0].Unit);
                    $('#Shift1').val(data[0].Shift);
                    $('#DualSystem1').val(data[0].DualSystem);


                    $('#ApplicationNum2').val(data[0].ApplicantNumber).attr('disabled', true);
                    $('#ApplicationName2').val(data[0].ApplicantName).attr('disabled', true);
                    $('#AdmissionRegNum2').val(data[0].AdmisRegiNumber);
                    $('#DivisionName3').append('<option>' + data[1].DivisionName + '<option>').attr('disabled', true);
                    $('#DivisionName3').val(data[1].DivisionName);

                    $('#DistrictName3').append('<option>' + data[1].DistrictName + '<option>').attr('disabled', true);
                    $('#DistrictName3').val(data[1].DistrictName);


                    $('#DistrictLgd3').val(data[1].DistrictLgdCode);

                    $('#TalukName3').append('<option>' + data[1].TalukName + '<option>').attr('disabled', true);
                    $('#TalukName3').val(data[1].TalukName);

                    $('#TalukLgd3').val(data[1].TalukLgdCode);


                    $('#MISITICode3').val(data[1].MISCode).attr('disabled', true);

                    $('#InstituteType3').append('<option>' + data[1].InstituteType + '<option>').attr('disabled', true);
                    $('#InstituteType3').val(data[1].InstituteType);

                    $('#InstituteTypeName3').append('<option>' + data[1].InstituteName + '<option>').attr('disabled', true);
                    $('#InstituteTypeName3').val(data[1].InstituteName);

                    $('#TradeName3').append('<option>' + data[1].TradeName + '<option>').attr('disabled', true);
                    $('#TradeName3').val(data[1].TradeName);

                    $('#Unit3').append('<option>' + data[1].Unit + '<option>').attr('disabled', true);
                    $('#Unit3').val(data[1].Unit);

                    $('#Shift3').append('<option>' + data[1].Shift + '<option>').attr('disabled', true);
                    $('#Shift3').val(data[1].Shift);

                    $('#DualSystem3').val(data[1].DualSystem).attr('disabled', true);
                }
            }
            else {
                bootbox.alert("failed");
            }
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function UpdateApplicantInstituteDetails() {
    let admitId = $('#AdmitId').val();
    let divisionId = $('#DivisionName2').val();
    let distictId = $('#DistrictName2').val();
    let talukId = $('#TalukName2').val();
    let instituteId = $('#InstituteTypeName2').val();
    let instituteType = $('#InstituteType2').val();
    let tradeId = $('#TradeName2').val();
    let unit = $('#Unit2').val();
    let shift = $('#Shift2').val();
    let DualSystem = $('#DualSystem2').val();
    let remark = $('#remarks1').val();
    let flowId = $('#users').val();

    if (divisionId == 'choose') {
        bootbox.alert('select the division');
    }
    else if (distictId == 'choose') {
        bootbox.alert('select the district');
    }
    else if (talukId == 'choose') {
        bootbox.alert('select the taluk');
    }
    else if (instituteType == 'choose') {
        bootbox.alert('select the institute Type');
    }
    else if (instituteId == 'choose') {
        bootbox.alert('select the institute');
    }
    else if (tradeId == 'choose') {
        bootbox.alert('select the trade');
    }
    else if (unit == 'choose') {
        bootbox.alert('select the unit');
    }
    else if (shift == 'choose') {
        bootbox.alert('select the shift');
    }
    else
        if ($('#users').val() == 'choose') {
            bootbox.alert('select the role');
        } else if ($('#remarks1').val() == '') { bootbox.alert('Please enter the Remark'); }
        else {

            var Dta = {
                AdmitId: admitId,
                DivisionId: divisionId,
                DistrictId: distictId,
                TalukId: talukId,
                ApplicantInstituId: instituteId,
                InstituteTypeId: instituteType,
                TradeId: tradeId,
                Unit: unit,
                Shift: shift,
                DualSystem: DualSystem,
                Remarks: remark,
                FlowId: flowId
            }
            bootbox.confirm({
                message: "<br> Are you sure you want to submit the transfer request details to <b>Joint Director</b> for review?",
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
                    type: "GET",
                    url: "/Admission/UpdateApplicantInstituteDetails",
                    contentType: "application/json",
                    data: Dta,
                    success: function (data) {
                        if (data == true) {
                            bootbox.alert("<br> Transfer request details submitted to <b>Joint Director</b> successsfully for review");
                            GetAdmittedDataStatus();
                            $('#SeatViewModal').modal('hide');
                        }
                        else {
                            bootbox.alert("failed");
                        }
                    },
                    error: function (result) {
                        bootbox.alert("failed");
                    }
                });
                    }
                }
            });
            //bootbox.confirm("Are you sure you want to update!", function (result) {
            //    $.ajax({
            //        type: "GET",
            //        url: "/Admission/UpdateApplicantInstituteDetails",
            //        contentType: "application/json",
            //        data: Dta,
            //        success: function (data) {
            //            if (data == true) {
            //                bootbox.alert("transfer seat successsfully submitted");
            //                GetAdmittedDataStatus();
            //                $('#SeatViewModal').modal('hide');
            //            }
            //            else {
            //                bootbox.alert("failed");
            //            }
            //        },
            //        error: function (result) {
            //            bootbox.alert("failed");
            //        }
            //    });
            //});

        }
}

function GettransferComments(id) {

    $.ajax({
        type: "GET",
        url: "/Admission/GetTransferRemarks",
        data: { 'seatId': id },
        contentType: "application/json",
        success: function (data) {
            $('#RemarksSeatTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.no', 'className': 'text-center' },
                    { 'data': 'Date', 'title': 'Date', 'className': 'text-center' },
                    { 'data': 'From', 'title': 'From', 'className': 'text-center' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-center' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-center' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
    $('#RemarkseatModal').modal('show');
}

function GetDistrictData(se, nxt) {
    if ($('#' + se).val() != 'choose')
        GetDistrictsDDp(nxt, $('#' + se).val());
    else {
        $('#' + nxt).empty();
        $('#' + nxt).append('<option value="choose">Choose</option>');
        $('#TalukName2').empty();
        $('#TalukName2').append('<option value="choose">Choose</option>');
        $('#DistrictLgd2').val('');
        $('#TalukLgd2').val('');
    }
}


function GetTalukData(se, nxt) {
    if ($('#' + se).val() != 'choose') {
        GetTaluksDDp(nxt, $('#' + se).val());
        $('#TalukLgd2').val('');
    }
    else {
        $('#' + nxt).empty();
        $('#' + nxt).append('<option value="choose">Choose</option>');
    }
    $('#DistrictLgd2').val('');
    if ($('#' + se).val() != 'choose')
        $('#DistrictLgd2').val($('#' + se).val());
}
function GetTalukLgdCode(se) {
    $('#TalukLgd2').val('');
    if ($('#' + se).val() != 'choose')
        $('#TalukLgd2').val($('#' + se).val());
}
function GetInstituteTypes(taluk) {
    $("#" + taluk).empty();
    $("#" + taluk).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetInstituteTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + taluk).append($("<option/>").val(this.TradeId).text(this.TradeName));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
function GetInstituteNames(insti, vid, taluk) {
    $("#" + insti).empty();
    $("#" + insti).append('<option value="choose">choose</option>');
    var tup = $('#' + vid).val();
    var tal = $('#' + taluk).val();
    if ($('#' + taluk).val() != 'choose') {
        $.ajax({
            url: "/Admission/GetTranseferInstitutes",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { 'type': tup, 'taluklgd': tal },
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + insti).append($("<option/>").val(this.TradeId).text(this.TradeName));
                    });
                }
            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#" + vid).val('choose');
        bootbox.alert('select the taluk');
    }
}
function ForwardtransferSeat() {
    var transId = $('#AdmitId').val();
    var remark = $('#remarks1').val();
    var status = $('#status').val();
    var flowId = $('#rolesfwd').val();
    var RoleSelected = $('#rolesfwd :selected').text();
    if (flowId == 'choose') {
        bootbox.alert('<br> please select role');
    } else if ($('#remarks1').val() == '') { bootbox.alert('Please enter the Remarks'); }
    else {
    bootbox.confirm({
        message: "<br> Are you sure you want to Forward Seat transfer request details to <b> " + RoleSelected +"</b> ?",
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
                type: "GET",
                url: "/Admission/ForwardAdmittedData",
                contentType: "application/json",
                data: { 'transSeatId': transId, 'status': status, 'remarks': remark, 'flowId': flowId },
                success: function (data) {
                    if (data == true) {
                        $('#SeatViewModal').modal('hide');
                        bootbox.alert("<br> You have forwarded the Seat transfer request details for " + $('#status option:selected').text() + " to <b> " + RoleSelected+ "</b> successfully");
                        GetAdmittedDataStatus();

                    }
                    else {
                        bootbox.alert("failed");
                    }
                },
                error: function (result) {
                    bootbox.alert("failed");
                }
            });
        }
        }
    });
}

    //bootbox.confirm("Are you sure you want to Forward!", function (result) {
    //    if (result == true) {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Admission/ForwardAdmittedData",
    //            contentType: "application/json",
    //            data: { 'transSeatId': transId, 'status': status, 'remarks': remark, 'flowId': flowId },
    //            success: function (data) {
    //                if (data == true) {
    //                    $('#SeatViewModal').modal('hide');
    //                    bootbox.alert("you have forwarded the Seat transfer data for " + $('#status option:selected').text() + " to " + $('#rolesfwd option:selected').text() + " successfully");
    //                    GetAdmittedDataStatus();

    //                }
    //                else {
    //                    bootbox.alert("failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("failed");
    //            }
    //        });
    //    }
    //});
}

function ApprovetransferSeat() {
    var transId = $('#AdmitId').val();
    var remark = $('#remarks1').val();
    var status = $('#status').val();

    if ($('#remarks1').val() == '') { bootbox.alert('Please enter the Remark'); }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to Approve the Seat transfer request details?",
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
                        type: "GET",
                        url: "/Admission/ApproveAdmittedData",
                        contentType: "application/json",
                        data: { 'transSeatId': transId, 'status': status, 'remarks': remark },
                        success: function (data) {
                            if (data == true) {
                                $('#SeatViewModal').modal('hide');
                                bootbox.alert("<br> You have approved the Seat transfer request details successfully");
                                GetAdmittedDataStatus();
                            }
                            else {
                                bootbox.alert("failed");
                            }
                        },
                        error: function (result) {
                            bootbox.alert("failed");
                        }
                    });
                }
            }
        });
    }
    
    //bootbox.confirm("<br> Are you sure you want to Approve!", function (result) {
    //    if (result == true) {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Admission/ApproveAdmittedData",
    //            contentType: "application/json",
    //            data: { 'transSeatId': transId, 'status': status, 'remarks': remark },
    //            success: function (data) {
    //                if (data == true) {
    //                    $('#SeatViewModal').modal('hide');
    //                    bootbox.alert("<br> you have approved the Seat transfer data successfully");
    //                    GetAdmittedDataStatus();
    //                }
    //                else {
    //                    bootbox.alert("failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("failed");
    //            }
    //        });
    //    }
    //});
}

function SendbacktransferSeat() {
    var transId = $('#AdmitId').val();
    var remark = $('#remarks1').val();
    var status = $('#status').val();
    var roleback = $('#rolesback').val();
    var RoleSelected = $('#rolesback :selected').text();

    if (roleback == 'choose') {
        bootbox.alert('<br> please select role');
    } else if ($('#remarks1').val() == '') { bootbox.alert('Please enter the Remark'); }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to Sendback Seat transfer request details for correction/clarification to <b>" + RoleSelected + "</b>!",
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
                type: "GET",
                url: "/Admission/SendBackAdmittedData",
                contentType: "application/json",
                data: { 'transSeatId': transId, 'status': status, 'remarks': remark },
                success: function (data) {
                    if (data == true) {
                        $('#SeatViewModal').modal('hide');
                                bootbox.alert(" <br> You have successfully sendback the Seat transfer request details for correction/clarification to <b>" + RoleSelected + "</b>");
                        GetAdmittedDataStatus();
                    }
                    else {
                        bootbox.alert("failed");
                    }
                },
                error: function (result) {
                    bootbox.alert("failed");
                }
            });
        }
            }
    });
}
  

    //bootbox.confirm("Are you sure you want to Sendback for correction/clarification!", function (result) {
    //    if (result == true) {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Admission/SendBackAdmittedData",
    //            contentType: "application/json",
    //            data: { 'transSeatId': transId, 'status': status, 'remarks': remark },
    //            success: function (data) {
    //                if (data == true) {
    //                    $('#SeatViewModal').modal('hide');
    //                    bootbox.alert("you have successfully sendback the Seat transfer data for correction/clarification");
    //                    GetAdmittedDataStatus();
    //                }
    //                else {
    //                    bootbox.alert("failed");
    //                }
    //            },
    //            error: function (result) {
    //                bootbox.alert("failed");
    //            }
    //        });
    //    }
    //});
}
function GetMisCode(instiId, misId) {
    var insti = $('#' + instiId).val();
    $('#' + misId).val('');
    if (insti != 'choose') {
        $.ajax({
            type: "GET",
            url: "/Admission/GetMisCode",
            contentType: "application/json",
            data: { 'instiId': insti },
            success: function (data) {
                $('#' + misId).val(data);
            },
            error: function (result) {
                bootbox.alert("failed");
            }
        });
    }
    $("#TradeName2").empty();
    $("#TradeName2").append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/GetAvailableseatsTrades",
        contentType: "application/json",
        data: { 'instiId': insti },
        success: function (data) {
            $.each(data, function () {
                $("#TradeName2").append($("<option/>").val(this.TradeId).text(this.TradeName));
            });
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function GetUnits(idval, institute, setId) {
    var trad = $('#' + idval).val();
    var insti = $('#' + institute).val();
    $("#" + setId).empty();
    $("#" + setId).append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/GetUnits",
        contentType: "application/json",
        data: { 'insti': insti, 'trade': trad },
        success: function (data) {
            $.each(data, function () {
                $("#" + setId).append($("<option/>").val(this.TradeId).text(this.TradeId));
            });
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });


    $.ajax({
        type: "GET",
        url: "/Admission/GetDualSystem",
        contentType: "application/json",
        data: { 'insti': insti, 'trade': trad },
        success: function (data) {
            $('#DualSystem2').val(data);
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function GetShifts(idval, institute, setId) {
    var trad = $('#' + idval).val();
    var insti = $('#' + institute).val();
    var unit = $('#Unit2').val();
    $("#" + setId).empty();
    $("#" + setId).append('<option value="choose">choose</option>');
    if (unit != 'choose') {
        $.ajax({
            type: "GET",
            url: "/Admission/GetShifts",
            contentType: "application/json",
            data: { 'insti': insti, 'trade': trad, 'unit': unit },
            success: function (data) {
                $.each(data, function () {
                    $("#" + setId).append($("<option/>").val(this.TradeId).text(this.TradeId));
                });
            },
            error: function (result) {
                bootbox.alert("failed");
            }
        });
    }
}
function GetStatus1(status) {
    $("#" + status).empty();
    $("#" + status).append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/SeatTransferStatus",
        contentType: "application/json",
        success: function (data) {
            $.each(data, function () {
                $("#" + status).append($("<option/>").val(this.Status).text(this.StatusName));
            });
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}
function Approvechange() {
    if ($('#status').val() == '2') {
        $('#approve').show();
        $('#rolesfwd').attr('disabled', true);
        $('#forward').attr('disabled', true);
        $('#rolesback').attr('disabled', true);
        $('#sendback').attr('disabled', true);

    }
    else
    if ($('#status').val() == '9') {
        $('#approve').hide();
        $('#rolesfwd').attr('disabled', true);
        $('#forward').attr('disabled', true);
        $('#rolesback').attr('disabled', false);
        $('#sendback').attr('disabled', false);
    }
    else if ($('#status').val() == '7') {
        $('#approve').hide();
        $('#rolesfwd').attr('disabled', false);
        $('#forward').attr('disabled', false);
        $('#rolesback').attr('disabled', true);
        $('#sendback').attr('disabled', true);
    }
    else {
        $('#approve').hide();
        $('#rolesfwd').attr('disabled', false);
        $('#forward').attr('disabled', false);
        $('#rolesback').attr('disabled', false);
        $('#sendback').attr('disabled', false);
    }
}

$('#SeatViewModal').on('hidden.bs.modal', function (event) {
    $('#remarks1').val('');
    $('#status').val('choose');
    $('#rolesfwd').val('choose');
    $('#users').val('choose');
    $('#rolesback').val('choose');
    $('#rolesfwd').attr('disabled', false);
    $('#forward').attr('disabled', false);
    $('#rolesback').attr('disabled', false);
    $('#sendback').attr('disabled', false);
});

function GetApplicantTransferbyList(isFilter) {
    
    var dataContent = { session: "", courseType: "", trade: "" };
    if (isFilter == 1) {
        if ($("#Session3 option:selected").text() != "choose") {
            dataContent = {
                session: $("#SessionTby option:selected").text(),
                courseType: $("#CourseTypeTby option:selected").text(), trade: $("#TradesTby option:selected").text()
            };
        }
        else
            $("#sessionErr1").text("Select session.");
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantTransferbyList",
        data: dataContent,
        contentType: "application/json",
        success: function (data) {
            $('#ApplicantTransferInstSeatTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'YearSession', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    //{
                    //    'data': 'Session', 'title': 'Session', 'className': 'text-left',
                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(oData.Session, oData.ApplyMonth, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return month + "/" + year + "-" + month + "/" + (year + parseInt(oData.TradeDuration))
                    //    },
                    //},
                    { 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left' },      
                    {
                        'data': 'Slno',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GettransferComments(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    },
                    {
                        'data': 'Slno',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.statuswith == 9)
                                $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-danger btn-xs' value='Edit' id='View'/>");
                            else
                                $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetApprovedTransferList(isFilter) {

    var dataContent = { session: "", courseType: "", trade: "" };
    if (isFilter == 1) {
        if ($("#SessionAT option:selected").text() != "choose") {
            dataContent = {
                session: $("#SessionAT option:selected").text(),
                courseType: $("#CourseTypeAT option:selected").text()
                //, trade: $("#TradesTby option:selected").text()
            };
        }
        else
            $("#sessionErr1").text("Select session.");
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetApprovedTransferList",
        data: dataContent,
        contentType: "application/json",
        success: function (data) {
            $('#ApplicantTransferApprovedTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'YearSession', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'AdmisRegiNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    //{ 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left' }
                    //{
                    //    'data': 'Session', 'title': 'Session', 'className': 'text-left',
                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(oData.Session, oData.ApplyMonth, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return month + "/" + year + "-" + month + "/" + (year + parseInt(oData.TradeDuration))
                    //    },
                    //},
                    
                    //{
                    //    'data': 'Slno',
                    //    'title': 'Remarks',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        $(nTd).html("<input type='button' onclick='GettransferComments(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                    //    }
                    //},
                    //{
                    //    'data': 'Slno',
                    //    'title': 'Action',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        if (oData.statuswith == 9)
                    //            $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-danger btn-xs' value='Edit' id='View'/>");
                    //        else
                    //            $(nTd).html("<input type='button' onclick='GetSeatViewDetails(" + oData.StatusId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                    //    }
                    //}
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetDistrictsEdit(dist, divisionId) {
    
    $("#DistrictName2").empty();
    $("#DistrictName2").append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetDistrict",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'Divisions': divisionId },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (divisionId == 0) {
                        $("#DistrictName2").empty();
                        $("#DistrictName2").append('<option value="choose">Choose</option>');
                    }
                    else {
                        $("#DistrictName2").append($("<option/>").val(this.district_id).text(this.district_ename));

                    }
                });
                $("#DistrictName2").val(dist);
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetDivisionsEdit(division) {
    $("#DivisionName2").empty();
    $("#DivisionName2").append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetDivision",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#DivisionName2").append($("<option/>").val(this.division_id).text(this.division_name));
                });
                $("#DivisionName2").val(division);
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetTaluksEdit(taluk, distId) {
    $("#TalukName2").empty();
    $("#TalukName2").append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetCitiTaluks",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'distilgdCOde': distId },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#TalukName2").append($("<option/>").val(this.CityId).text(this.CityName));
                });

                $('#TalukName2').val(taluk);
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetInstituteTypesEdit(InstituteTypeId) {
    $("#InstituteType2").empty();
    $("#InstituteType2").append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetInstituteTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#InstituteType2").append($("<option/>").val(this.TradeId).text(this.TradeName));
                });
                $('#InstituteType2').val(InstituteTypeId);
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
function GetInstituteNamesEdit(institute, taluk, ApplicantInstituId) {
    $("#InstituteTypeName2").empty();
    $("#InstituteTypeName2").append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetTranseferInstitutes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'type': institute, 'taluklgd': taluk },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#InstituteTypeName2").append($("<option/>").val(this.TradeId).text(this.TradeName));
                });
                $('#InstituteTypeName2').val(ApplicantInstituId);
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
function GetUnitsEdit(ApplicantInstituId, TradeIds, Unit) {
    $("#Unit2").empty();
    $("#Unit2").append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/GetUnits",
        contentType: "application/json",
        data: { 'insti': ApplicantInstituId, 'trade': TradeIds },
        success: function (data) {
            $.each(data, function () {
                $("#Unit2").append($("<option/>").val(this.TradeId).text(this.TradeId));
            });
            $('#Unit2').val(Unit);
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function GetShiftsEdit(TradeId, ApplicantInstituId, Unit, Shift) {
    $("#Shift2").empty();
    $("#Shift2").append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/GetShifts",
        contentType: "application/json",
        data: { 'insti': ApplicantInstituId, 'trade': TradeId, 'unit': Unit },
        success: function (data) {
            $.each(data, function () {
                $("#Shift2").append($("<option/>").val(this.TradeId).text(this.TradeId));
            });
            $('#Shift2').val(Shift);
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function GetTradeNameEdit(ApplicantInstituId,TradeId) {
    $("#TradeName2").empty();
    $("#TradeName2").append('<option value="choose">choose</option>');
    $.ajax({
        type: "GET",
        url: "/Admission/GetAvailableseatsTrades",
        contentType: "application/json",
        data: { 'instiId': ApplicantInstituId },
        success: function (data) {
            $.each(data, function () {
                $("#TradeName2").append($("<option/>").val(this.TradeId).text(this.TradeName));
            });
            $('#TradeName2').val(TradeId);
        },
        error: function (result) {
            bootbox.alert("failed");
        }
    });
}

function ForwardOrSendbacktransferSeat() {
    if ($('#status').val() == '7') {
        ForwardtransferSeat();
    }
    else if ($('#status').val() == '9') {
        SendbacktransferSeat();
    }
    else if ($('#status').val() == '2') {
        ApprovetransferSeat();
    }
}
