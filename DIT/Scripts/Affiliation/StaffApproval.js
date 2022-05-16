$(document).ready(function () {
    $('.tab-3').tab('show');
    $('.tab-2').tab('show');
    $('.tab-1').tab('show');
    // $('.tab-3').tab('show');
    
    
    GetStaffstatus();
    GetStatus();
    GetUsers();
    GetCourseTypes();
    GetDivisions();
    GetDistrictList();
    GetStaffstatusOSAD();
    StaffDetailsView();
    GetInstitutes();
    GetAllYear();
   

    $('#PhoneNoEdit').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#PhoneNo').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#Staffdetails_view').hide();
    $('#Staffdetails_view1').hide();
});

function GetStatus() {
    
    $("#dd-status").append('<option value="">Select</option>');
    $("#status").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllStatus",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.flag == 1) {
                $.each(data.list, function () {
                    $("#dd-status").append($("<option/>").val(this.Value).text(this.Text));
                    $("#status").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetUsers() {
    $("#dd-flow").append('<option value="">Select</option>');
    $("#flow").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllUsers",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#dd-flow").append($("<option/>").val(this.Value).text(this.Text));
                    $("#flow").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetInstitutes() {
    $("#InstituteName").empty();
    var DistId = $('#Districts :selected').val();
    
    $("#InstituteName").append('<option value="">Select</option>');
   
    $.ajax({
        url: "/Affiliation/GetAllInstitute",
        type: 'Get',
        data: { DistId: DistId },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
                $.each(data, function () {
                    $("#InstituteName").append($("<option/>").val(this.Value).text(this.Text));
                   
                });
           

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllYear() {
    $("#Session12").append('<option value="">Select</option>');
    $("#Year").append('<option value="">Select</option>');
    $("#YearOs").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#Session12").append($("<option/>").val(this.Value).text(this.Text));
                $("#Year").append($("<option/>").val(this.Value).text(this.Text));
                $("#YearOs").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


$("#dd-status").change(function () {
    var value = $(this).val();
    if (value == 2 || value == 3 || value == 4) {
        $("#dd-flow").val("");
        $("#dd-flow").attr("disabled", true);
    }
    else {
        $("#dd-flow").attr("disabled", false);
    }
});

$("#dd-status").change(function () {
    
    $("#dd-flow").empty();
    $("#dd-flow").append('<option value="">Select</option>');
    var value = $(this).val();
    //if (value == 4) {
    if (value == 4 || value == 5 || value == 7) {
        $.ajax({
            url: "/Affiliation/GetAllUsersforstaff",
            type: 'Get',
            data: { statusValue: value },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
                if (data != null || data != '') {
                    $.each(data, function () {

                        $("#dd-flow").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                else {
                    $("#dd-flow").attr("disabled", true);
                }



            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }

        });
    }

});

$("#status").change(function () {
    var value = $(this).val();
    if (value == 2 || value == 3 || value == 4) {
        $("#flow").val("");
        $("#flow").attr("disabled", true);
    }
    else {
        $("#flow").attr("disabled", false);
    }
});

$("#status").change(function () {
    
    $("#dd-flow").empty();
    $("#dd-flow").append('<option value="">Select</option>');
    $("#flow").empty();
    $("#flow").append('<option value="">Select</option>');
    var value = $(this).val();
    //if (value == 4) {
    if (value == 4 || value == 5 || value == 7) {
        $.ajax({
            url: "/Affiliation/GetAllUsersforstaff",
            type: 'Get',
            data: { statusValue: value },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
                if (data != null || data != '') {
                    $.each(data, function () {

                        $("#dd-flow").append($("<option/>").val(this.Value).text(this.Text));
                        $("#flow").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                else {
                    $("#dd-flow").attr("disabled", true);
                }



            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }

        });
    }

});

function GetCourseTypes() {
    //Add 
    $("#Course-Add").empty();
    $("#Course-Add").append('<option value="">Select</option>');
    $("#Course-OS").empty();
    $("#Course-OS").append('<option value="">Select</option>');
    //Update List
    $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="">Select</option>');
    // Publish
    $("#CourseTypes_P").empty();
    $("#CourseTypes_P").append('<option value="">Select</option>');
    //Affiliated View
    $("#CourseTypes_Af").empty();
    $("#CourseTypes_Af").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#CourseTypes").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#CourseTypes_P").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#CourseTypes_Af").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#Course-Add").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#Course-OS").append($("<option/>").val(this.course_id).text(this.course_name));

                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDivisions() {
    //Update
    
    $("#Divisions").empty();
    var div = $("#hdnDiv_ID").data('value')
    
    $("#Divisions").append('<option value="">Select</option>');
    //Publish
    $("#Divisions_P").empty();
    $("#Divisions_P").append('<option value="">Select</option>');
    //Affiliated View
    $("#Divisions_Af").empty();
    $("#Divisions_Af").append('<option value="">Select</option>');
    // Add Page
    $("#Division-Add").empty();
    $("#Division-Add").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetDivisions",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {
                if (($("#hdnDiv_ID").data('value') == 0 || $("#hdnDiv_ID").data('value') == this.division_id)) {
                    $("#Divisions").append($("<option/>").val(this.division_id).text(this.division_name));
                }
                //$("#Divisions").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Divisions_P").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Divisions_Af").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Division-Add").append($("<option/>").val(this.division_id).text(this.division_name));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetDistrictList() {

    var Divisions = $('#Divisions :selected').val();

    if ($("#hdnDiv_ID").data('value') != 0) {
        Divisions = $("#hdnDiv_ID").data('value');
    }
    if (Divisions != "" && Divisions != null) {
        $("#Districts").empty();
        $("#Districts").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#Districts").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }

}

//cancelAffiliatedInstitute();

//GetDivisions();

$(document).ready(function () {
    $('.tab-2').tab('show');
    $('.tab-1').tab('show');


    //alert("2");
    ;
    var tblGridSeatAvailability = $('#tblGridUpdateAffiliation1').DataTable();
    tblGridSeatAvailability.on('order.dt arch.dt', function () {
        tblGridSeatAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});
function clearView() {
    $("#tblGridUpdateAffiliation_wrapper").show(); $("#Staffdetails_view").hide(); $("#Remarks").val(""); $("#dd-flow").val(""); $("#dd-status").val(""); $("#Staffdetails_view1").hide(); $("#DistrictWiseFilter").show();

}
//=========== Staff Approval Start ===========//
function GetStaffstatus() {
    
    var roleid = $('#uservalue').data('value');
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffstatusForCW",
        contentType: "application/json",
        success: function (data) {
            
            $('#StaffStatusTable1').DataTable({

                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'wid' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewStaffStatus(" + oData.InstituteId + "," +
                                oData.YearId + "," +
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }

                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' " +
                                "value='View' id='view'" + (oData.IsAction ? "" : " disabled='disabled'") + "/>");
                            //if (roleid == oData.ApprovalFlowId /*&& oData.IsActive*/) {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + "," + oData.Year + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/>");
                            //} //<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view' /> & nbsp;& nbsp;
                            //else {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/ disabled='disabled'>");
                            //}//<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                        }
                    }
                ]
            });
            $('#StaffStatusTable1 tbody').find("tr").each(function (i) {
                

                $('#StaffStatusTable1 tbody').find("tr").eq(i).find(".wid").css('width', '5%');

            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function StaffviewMode(offid) {
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffDetailsForInstitute",
        type: 'Get',
        data: { id: offid },
        contentType: "application/json",
        success: function (data) {
            
            $('#tblStaffViewMode').DataTable({
                data: data.res,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Name', 'title': 'Staff Name', 'className': 'text-center' },
                    { 'data': 'DesignationName', 'title': 'Designation', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Type', 'title': 'Staff Type', 'className': 'text-left' },
                    { 'data': 'subject', 'title': 'Teaching Subject', 'className': 'text-left' },
                    { 'data': 'Tradename', 'title': 'Trade', 'className': 'text-left' },
                    { 'data': 'MobileNum', 'title': 'Mobile No', 'className': 'text-left' },
                    
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function StaffEditviewMode(offid,year) {
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffDetailsSessionWise",
        data: { id: offid, year: year},
        contentType: "application/json",
        success: function (data) {
            
            $('#tblEditStaffViewMode').DataTable({
                data: data.result,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Name', 'title': 'Staff Name', 'className': 'text-center' },
                    { 'data': 'DesignationName', 'title': 'Designation', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Type', 'title': 'Staff Type', 'className': 'text-left' },
                    { 'data': 'subject', 'title': 'Teaching Subject', 'className': 'text-left' },
                    { 'data': 'Tradename', 'title': 'Trade', 'className': 'text-left' },
                    { 'data': 'MobileNum', 'title': 'Mobile No', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'Email', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                   
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.IsActive == false) {
                        $('td', nRow).css('background-color', '#5691f0');
                    }

                }
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetStaffstatusOSAD() {
    var roleid = $('#uservalue').data('value');
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffstatusForCW",
        contentType: "application/json",
        success: function (data) {
            
            $('#StaffStatusTable').DataTable({

                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'widt' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewStaffStatus(" + oData.InstituteId + "," +
                                oData.YearId + "," +
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }

                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' " +
                                "value='View' id='view'" + (oData.IsAction ? "" : " disabled='disabled'") + "/>");
                            //if (roleid == oData.ApprovalFlowId /*&& oData.IsActive*/) {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/>");
                            //}//<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                            //else {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/ disabled='disabled'>");
                            //}  //<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                        }
                    }
                ]
            });

            $('#StaffStatusTable tbody').find("tr").each(function (i) {
                

                $('#StaffStatusTable tbody').find("tr").eq(i).find(".widt").css('width', '5%');

            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function ViewStaffStatus(offid, year, quarter) {
    
    $('#StaffNameErr').text('');
    $('#dd-designation').text('');
    $('#PhoneNoErr').text('');
    $('#EmailIdErr').text('');
    $('#QualificationErr').text('');
    $('#dd-stafftype_Required').text('');
    $('#dd-Subject_Required').text('');
    $('#dd-trade_Required').text('');
    $.ajax({
        url: "/Affiliation/Viewstaffhistory",
        type: 'Get',
        data: { id: offid, session: year, quarter: quarter },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null) {
                var _table = $("#tblViewStaff tbody");
                _table.empty();
                $.each(data, function (i) {
                    
                    var _tr = $("<tr/>");
                    _tr.append("<td>" + (i + 1) + "</td>");
                    _tr.append("<td>" + this.Date + "</td");
                    _tr.append("<td>" + this.From + "</td>");
                    _tr.append("<td>" + this.To + "</td>");
                    _tr.append("<td>" + this.Remarks + "</td>");

                    _table.append(_tr);
                });
                

            } else
                alert('failed');

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function EditStaffStatus(offid) {
    
    $('.OfficerNameEdit').text('');
    $('.designation').text('');
    $('.PhoneNoEdit').text('');
    $('.EmailIdEdit').text('');
    $('.QualificationEdit').text('');
    $('.stafftype').text('');
    $('.Subject').text('');
    $('.trade').text('');
    $('#DistrictWiseFilter').hide('');
    $('#tblGridUpdateAffiliation_wrapper').hide();
    //$('#modal-backdrop in"').modal('show');
    var roleid = $('#uservalue').data('value');


    $.ajax({
        url: "/Affiliation/ViewStaff",
        type: 'Get',
        data: { id: offid },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
            if (data != null) {
                if (roleid == data.ApprovalFlowId) {
                    $("#snd").attr("disabled", false);
                }
                else {
                    $("#snd").attr("disabled", true);
                }
                
                $('#StaffId').val(data.StaffId);
                $('.OfficerNameEdit').text(data.Name);
                $('.designation').text(data.DesignationName);
                $('.PhoneNoEdit').text(data.MobileNum);
                $('.EmailIdEdit').text(data.EmailId);
                $('.QualificationEdit').text(data.Qualification);
                $('.stafftype').text(data.Type);
                $('.Subject').text(data.subject);
                $('.trade').text(data.Tradename);
                $('#Staffdetails_view').show();
            } else
                alert('failed');

        },
        error: function (result) {
            alert("Error", "something went wrong");

        }
    });
}

function EditStaffStatusOSAD(offid) {

    $('.OfficerNameEdit').text('');
    $('.designation').text('');
    $('.PhoneNoEdit').text('');
    $('.EmailIdEdit').text('');
    $('.QualificationEdit').text('');
    $('.stafftype').text('');
    $('.Subject').text('');
    $('.trade').text('');
    $('#DistrictWiseFilter').hide('');
    $('#tblGridUpdateAffiliation_wrapper').hide();
    //$('#modal-backdrop in"').modal('show');
    var roleid = $('#uservalue').data('value');
    
    $.ajax({
        url: "/Affiliation/ViewStaff",
        type: 'Get',
        data: { id: offid },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null) {
                if (roleid == data.ApprovalFlowId) {
                    $("#snd").attr("disabled", false);
                }
                else {
                    $("#snd").attr("disabled", true);
                }
                
                $('#StaffId').val(data.StaffId);
                $('.text-name').text(data.Name);
                $('.text-Designation').text(data.DesignationName);
                $('.text-mobileNum').text(data.MobileNum);
                $('.text-email').text(data.EmailId);
                $('.text-Qualification').text(data.Qualification);
                $('.text-stafftype').text(data.Type);
                $('.text-Teachingsubject').text(data.subject);
                $('.text-Trade').text(data.Tradename);
                $('#Staffdetails_view1').show();
            } else
                alert('failed');

        },
        error: function (result) {
            alert("Error", "something went wrong");

        }
    });
}

function SaveStaffDetails(offid) {
    
    var flowid = $('#dd-flow').val();
    var Approvalstatusid = $('#dd-status').val();
    var Osflow = $('#flow').val();
    var Osstatusid = $('#status').val();
    var Remarks = $('#Remarks').val();
    if (Remarks == "") {

        return bootbox.alert("Pls enter Remarks");
    }
    if (Approvalstatusid == "") {
        return bootbox.alert("Pls select Status");
    }
    if (flowid == "" || flowid == null) {

        if ($("#dd-flow").is(':enabled')) {

            return bootbox.alert('Pls select send to ');
        }
    }
    //if (flowid == "") {
    //    if (Approvalstatusid != 4) {
    //        return bootbox.alert('Please select Send to dropdown');
    //    }

    //}
    //}
    //if (Approvalstatusid == "") {
        
     
    //}
    //if (Remarks == "") {
    //    return bootbox.alert('Please Enter Remarks');
    //}
    
    $('#tblEditStaffViewMode').DataTable().destroy();
    $('#tblEditStaffViewMode').DataTable({ "paging": false }).draw(false);
       var listItem = [];
    var shift_table = $("#tblEditStaffViewMode tbody");
    shift_table.find('tr').each(function (len) {
        var $tr = $(this);

        var phoneNo = $tr.find("td:eq(8)").text();
        var emailId = $tr.find("td:eq(9)").text();
        var instid = $tr.find("td:eq(9)").val();
        var Year = $tr.find("td:eq(1)").text();
        var quarter = $tr.find("td:eq(10)").text();
        var list = {
            MobileNum: phoneNo,
            EmailId: emailId,
            Remarks: Remarks,
            ApprovalFlowId:flowid,
            Appeovalstatus: Approvalstatusid,
            Year: Year,
            Quarter: quarter
        }
        listItem.push(list);

    });

    if (flowid == 13) {
        var name = 'Office Superintendent'
    }
    else if (flowid == 17) {
        name = 'Case Worker'
    }
    else if (flowid == 14) {
        name = 'Assistant Director'
    }
    else if (flowid == 15) {
        name = 'Deputy Director'
    }

    bootbox.confirm('<br><br> Are you sure you want to submit the staff details to ' + name + ' for review please Confirm ?', (confirma) => {
        if (confirma) {
            $.ajax({
                url: "/Affiliation/ApproveStaff",
                type: "POST",
                data: JSON.stringify(listItem),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == "success") {
                        
                        bootbox.alert("<br><br> Staff details Submitted successfully to " + name + " for review");
                        GetStaffstatus();
                        GetStaffstatusOSAD();
                        StaffDetailsView();
                        $('#EditStaffViewModal').modal('hide');
                        //$('#tblGridUpdateAffiliation_wrapper').show();
                        //$("#Staffdetails_view").hide();
                        //$("#Staffdetails_view1").hide();
                        //$("#Remarks").val("");
                        //$("#dd-flow").val("");
                        //$("#dd-status").val("");
                        //$('#EditOfficerModal').modal('hide');
                        //GetStaffstatusOSAD();
                        //GetTotalApplicantOfficer();
                        //GetActiveOfficers();
                    }
                    else
                        bootbox.alert("failed");

                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
    });
}

function SaveStaffDetailsOSAD(offid) {
    
    
    var Osflow = $('#flow').val();
    var Osstatusid = $('#status').val();
    var Remarks = $('#Remark').val();
    if (Remarks == "")
    {
       
      return  bootbox.alert("Pls enter Remarks");
    }
    if (Osstatusid == "") {
        return bootbox.alert("Pls enter Status");
    }
    if (Osflow == "" || Osflow == null) {

        if ($("#flow").is(':enabled')) {
           
            return bootbox.alert('Pls select send to ');
        }
    }
    $('#tblEditStaffViewMode').DataTable().destroy();
    $('#tblEditStaffViewMode').DataTable({ "paging": false }).draw(false);
    var listItem = [];
    var shift_table = $("#tblEditStaffViewMode tbody");
    shift_table.find('tr').each(function (len) {
        var $tr = $(this);

        var phoneNo = $tr.find("td:eq(8)").text();
        var emailId = $tr.find("td:eq(9)").text();
        var instid = $tr.find("td:eq(9)").val();
        var Year = $tr.find("td:eq(1)").text();
        var quarter = $tr.find("td:eq(10)").text();
        var list = {
            MobileNum: phoneNo,
            EmailId: emailId,
            Remarks: Remarks,
            ApprovalFlowId: Osflow,
            Appeovalstatus: Osstatusid,
            Year: Year,
            Quarter: quarter
        }
        listItem.push(list);

    });

    if (Osflow == 13) {
        var name = 'Office Superintendent'
    }
    else if (Osflow == 17) {
        name = 'Case Worker'
    }
    else if (Osflow==14)
    {
        name = 'Assistant Director'
    }
    else if (Osflow == 15) {
        name = 'Deputy Director'
    }
    if (Osstatusid==4)
    {
        name ='ItI Admin'
    }
    
    if (Osstatusid != 2) {
        bootbox.confirm('<br><br> Are you sure you want to Send staff details to ' + name + ' for review please Confirm?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Affiliation/ApproveStaff",
                    type: "POST",
                    data: JSON.stringify(listItem),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                            

                            bootbox.alert('<br><br> Staff Detail records sent to ' + name + ' successfully for review.');
                            GetStaffstatus();
                            GetStaffstatusOSAD();
                            StaffDetailsView();
                            $('#EditStaffViewModal').modal('hide');

                        }
                        else
                            bootbox.alert("failed");

                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }
        });
    }
    else
    {
        bootbox.confirm('<br><br> Are you sure you want to Approve the Staff Details Please Confirm?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Affiliation/ApproveStaff",
                    type: "POST",
                    data: JSON.stringify(listItem),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                            

                            bootbox.alert('<br><br> Staff Details Approved successfully.');
                            GetStaffstatus();
                            GetStaffstatusOSAD();
                            $('#EditStaffViewModal').modal('hide');

                        }
                        else
                            bootbox.alert("failed");

                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }
        });
    }
    
}

function GetStaffOnsearch() {
    
    var courseid = $("#CourseTypes").val();;
    var yearid = $("#Session12").val();
    var Quarter1 = $("#Quarter1").val();
    $.ajax({
        type: "GET",
        url: "/Affiliation/SearchStaff",
        contentType: "application/json",
        data: { Year: yearid, courseId: courseid, Quarter1: Quarter1 },
        success: function (data) {
            
            $('#StaffStatusTable1').DataTable({

                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewStaffStatus(" + oData.InstituteId + "," +
                                oData.YearId + "," +
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }

                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' " +
                                "value='View' id='view'" + (oData.IsAction ? "" : " disabled='disabled'") + "/>");
                            //if (roleid == oData.ApprovalFlowId /*&& oData.IsActive*/) {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + "," + oData.Year + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/>");
                            //} //<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view' /> & nbsp;& nbsp;
                            //else {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/ disabled='disabled'>");
                            //}//<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetStaffOnsearchForOSAD() {
    
    var courseid = $("#Course-OS").val();
    var yearid = $("#YearOs").val();
    var quarter = $("#QuarterOs").val();
    $.ajax({
        type: "GET",
        url: "/Affiliation/SearchStaffForOSAD",
        contentType: "application/json",
        data: { Year: yearid, courseId: courseid, quarter: quarter },
        success: function (data) {
            
            $('#StaffStatusTable').DataTable({

                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewStaffStatus(" + oData.InstituteId + "," +
                                oData.YearId + "," +
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }

                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' " +
                                "value='View' id='view'" + (oData.IsAction ? "" : " disabled='disabled'") + "/>");
                            //if (roleid == oData.ApprovalFlowId /*&& oData.IsActive*/) {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/>");
                            //}//<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                            //else {
                            //    $(nTd).html("<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffViewModal' value='View' id='view'/ disabled='disabled'>");
                            //}  //<input type='button' onclick='StaffviewMode(" + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function StaffDetailsView() { 
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/StaffDetailsView",
        contentType: "application/json",
        success: function (data) {
            
            $('#StaffDetailsViewTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type='button' onclick='StaffApproveviewMode(" + oData.Quarter + "," + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ApproveStaffViewModal' value='View' id='view'/>");
                            //$(nTd).html("<input type='button' onclick='StaffApproveviewMode(" + oData.Year + ","+ oData.InstituteId +   ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ApproveStaffViewModal' value='View' id='view'/>");
                            
                            
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function ViewStaffOnsearch() {
    
    var courseid = $("#Course-Add").val();
    var yearid = $("#Year").val();
    var Division = $("#Divisions").val();
    var District = $("#Districts").val();
    var Institute = $("#InstituteName").val();
    var quarter = $("#Quarter").val();
    $.ajax({
        type: "GET",
        url: "/Affiliation/ViewStaffDetailsView",
        contentType: "application/json",
        data: { Year: yearid, courseId: courseid, DivisionId: Division, DistrictId: District, InstituteId: Institute, quarter: quarter },
        success: function (data) {
            
            $('#StaffDetailsViewTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                    { 'data': 'District', 'title': 'District', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'quarter', 'className': 'text-left' },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='StaffApproveviewMode(" + oData.Quarter + "," + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ApproveStaffViewModal' value='View' id='view'/>");
                            //$(nTd).html("<input type='button' onclick='StaffApproveviewMode(" + oData.Year + "," + oData.InstituteId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ApproveStaffViewModal' value='View' id='view'/>");


                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function StaffApproveviewMode(year,offid) {
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffDetailsSessionWise",
        data: { session:year,id: offid },
        contentType: "application/json",
        success: function (data) {
            
            $('#tblApproveStaffViewMode').DataTable({
                data: data.filt,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'Name', 'title': 'Staff Name', 'className': 'text-center' },
                    { 'data': 'DesignationName', 'title': 'Designation', 'className': 'text-left' },
                    { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
                    { 'data': 'Type', 'title': 'Staff Type', 'className': 'text-left' },
                    { 'data': 'subject', 'title': 'Teaching Subject', 'className': 'text-left' },
                    { 'data': 'Tradename', 'title': 'Trade', 'className': 'text-left' },
                    { 'data': 'MobileNum', 'title': 'Mobile No', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'Email', 'className': 'text-left' },


                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function cancelselectfields() {
    $('#CourseTypes').val('');
    //$('#Quarter1').val('');
    $('#Session12').val('');
    $('#YearOs').val('');
    //$('#QuarterOs').val('');
    $('#Course-OS').val('');

    $('#InstituteName').val('');
    $('#Districts').val('');
    $('#Divisions').val('');
    $('#Course-Add').val('');
    //$('#Quarter').val('');
    $('#Year').val('');


}

//===========Staff Approval end=============//