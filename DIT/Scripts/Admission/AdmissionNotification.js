
$(document).ready(function () {    
    $('#deptId').attr('disabled', 'disabled');
    $('#deptId').val(DeptConst.Admission);
    $('#GridListDiv').show();
    $('#ViewDiv').hide();
    $('#sendBtn3').hide();    
    $('#sendBtnchanges').hide();   
    //disable special characters for nitification number
    $('#notifNumber').on('keypress', function (event) {
        var regex = new RegExp("^[0-9-!@#$%&*?]"); 
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    Getusers("users", "1");
    Getusers("users1", "3");
    GetAdmissionNotificationDetails();
});
$('#notif_date').datepicker({
    dateFormat: 'dd-mm-yy',
    minDate: 0.,
    changeMonth: true,
    changeYear: true
});
$('a[href="#tab_1"]').click(function () {
    fnClearFields();
});

$('#users1').click(function () {
    $("#users1 option[value ='" + loginUserRole.DD + "']").hide();
});

function fnRefreshScreenData () {
    var url = '/Admission/CreateAdmissionNotification';
    window.location.replace(url);
}

$("#btnDownloadNotifFile").click(function () {
    var url = "../../" + $("#btnDownloadNotifFile").data('url');
    window.open(url);
});

function GetAdmissionNotificationDetails() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetAdmissionNotification",
        contentType: "application/json",
        success: function (data) {
            $('#AdmissionNotificationTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No. ', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center' },
                    //{ 'data': 'DeptName', 'title': 'Section Name', 'className': 'text-left' },
                    { 'data': 'Exam_Notif_Number', 'title': 'Notification Number', 'className': 'text-left' },
                    { 'data': 'Exam_Notif_Desc', 'title': 'Notification Description', 'className': 'text-left' },
                    {
                        'data': 'Exam_notif_date', 'title': 'Notification Date', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.Exam_notif_date);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'exam_notif_status_desc', 'title': 'Status '+'- Currently with', 'className': 'text-left' },
                    {
                        'data': 'Remarks', 'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {                            
                            
                            if (oData.StatusId == 101 || oData.StatusId == 110) 
                                $(nTd).html("<input type='button' disabled='disabled' onclick='GetCommentTable(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");
                            else
                                $(nTd).html("<input type='button' onclick='GetCommentTable(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");
                        }
                    },
                    {
                        'data': 'Admission_Notif_Id',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.StatusId == 109) {
                                if (oData.role_id == 8) {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>&nbsp;<input type='button' onclick='PublishDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-success btn-xs' value='Publish' id='PublishAd' />");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                            else if (oData.StatusId == 101 || oData.StatusId == 103 || oData.StatusId == 107 || oData.StatusId == 108) {
                                    if (oData.role_id == 8) {
                                      $(nTd).html("<input type='button' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='edit'/>&nbsp;<a href='/Admission/CreateAdmissionNotification?NotificationId=" + oData.Admission_Notif_Id + "' class='btn btn-primary btn-xs' value='Edit'>Edit</a>");
                                    }
                                    else {
                                        $(nTd).html("<input type='button' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }
                            }
                            else
                            if (oData.StatusId == 110) {
                                if (oData.role_id == 8) {
                                    $(nTd).html("<input type='button' disabled='disabled' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>&nbsp;<a href='/Admission/CreateAdmissionNotification?NotificationId=" + oData.Admission_Notif_Id + "' class='btn btn-primary btn-xs' value='Edit'>Edit</a>");
                                } else {
                                    $(nTd).html("<input type='button' disabled='disabled' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }

                            }
                            else {
                                $(nTd).html("<input type='button' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                            }
                        }
                    }
                ]
            });
        }
    });
}

function daterangeformate2(datetime) {
    if (datetime == "" || datetime == null) {
        var openingsd = "";
        return openingsd;
    }
    else {
        var StartDateOpening = new Date(parseInt(datetime.replace('/Date(', '')))
        var StartDateOpeningmonth = StartDateOpening.toString().slice(4, 7);
        var StartDateOpeningdate = StartDateOpening.toString().slice(8, 10);
        var StartDateOpeningyear = StartDateOpening.toString().slice(11, 15);
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
        var openingsd = StartDateOpeningdate + "-" + statedatemonth1 + "-" + StartDateOpeningyear;

        return openingsd;
    }
}

function GetCommentTable(id) {
    //var id = $('#id').html();
    $('#RemarksCommentsModal').modal('show');
    $.ajax({
        type: "Get",
        url: "/Admission/GetComments",
        data: { id: id },
        success: function (data) {
            var t = $('#CommentsTable').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'createdatetime', 'title': 'Date', 'className': 'text-left' },
                    //{ 'data': 'Exam_Notif_Number', 'title': 'Notification No.', 'className': 'text-left' },
                    { 'data': 'FromUser', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'Status', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'comments', 'title': 'Remarks Description', 'className': 'text-left' }
                ]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();

            $('#notificationNum').html(data[0].Exam_Notif_Number);
        }
    });
}


function GetViewNotificationTable(id) {
    if ($("#hdnSession").data('value') == loginUserRole.CW)
        $("#btnSave").prop('disabled', true);
    $('#ViewNotificationModal').modal('show');
    var id = 0;
    $.ajax({
        type: "Get",
        url: "/Admission/ViewAdmissionNotification",
        data: { id: ExamNotifyId },
        success: function (data) {
            var t = $('#tblViewNotification').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'createdatetime', 'title': 'Date', 'className': 'text-left' },
                    //{ 'data': 'Exam_Notif_Number', 'title': 'Notification No.', 'className': 'text-left' },
                    { 'data': 'Role', 'title': 'Officer', 'className': 'text-left' },
                    {
                        'title': 'Upload/Download Notification(.doc)',
                        render: function (data, type, row) {
                            id++;
                            if (row.SavePath) {
                                return '<div class="row"><div class="col-sm-10"><input type="file" accept="application/docx" id="fileUpload' + id + '" name="file" ' + (row.RoleId == $("#hdnSession").data('value') ? 'enabled' : 'disabled') + '/><small class="text-black-50">' + (row.SavePath != null ? row.SavePath.split('/').pop() : "") + '</small></div>' +
                                    '<div id="NotificationfileAttach" class="col-sm-2"><a href="../../' + row.SavePath + '" class="link" target="_blank" style="color:green;"><img title="Download Notification(doc)" style="width:25px;height:25px" src="/Images/download.png"></a></div></div>';
                            }
                            else {
                                return '<div class="row"><div class="col-sm-10"><input type="file" accept="application/docx" id="fileUpload' + id + '" name="file" ' + (row.RoleId == $("#hdnSession").data('value') ? 'enabled' : 'disabled') + '/><small class="text-black-50">' + (row.SavePath != null ? row.SavePath.split('/').pop() : "") + '</small></div>'
                            }
                        }
                    },
                    {
                        'title': 'Download Notification(.pdf)',
                        render: function (data, type, row) {
                            if (row.toPDF == true)
                                return "<a class='btn btn-link' href='../../" + row.Admsn_notif_doc + "' target='_blank'><img src='/Content/img/pdf_logo.png' height='40px' width='40px' title='Download Notification(pdf)'/></a>";
                            else
                                return "";
                        }
                    },
                    //{ 'data': 'Status', 'title': 'Status', 'className': 'text-left' },
                    //{ 'data': 'comments', 'title': 'Remarks Description', 'className': 'text-left' }
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


$("#CancelBtn").click(function () {     
    $('#ViewDiv').hide();
    $('#GridListDiv').show();       
});
function viewAdmissionNotificationDetails(id) {    
    $('#GridListDiv').hide();  
    $('#ViewDiv').show();      
    ExamNotifyId = id
    $("#remarks").val('');

    $.ajax({
        type: "POST",
        url: "/Admission/GetAdmissionNotificationDetails",
        dataType: 'json',
        data: { id: id },
        success: function (response) {
            $("#id").html(response.Admsn_notif_Id);
            $("#course").html(response.CourseType);
            $("#dept").html(response.DepartmentName);
            $("#notifynum").html(response.NotificationNumber);
            $("#desc1").html(response.Description);
            $("#notifType").html(response.NotifDescName);
            var dat = daterangeformate2(response.AddDate);            
            $('#datenotify').html(dat);
            $('#statusdesc').html(response.Status);
            $('#RecordLevel').html(response.Role)           
            StatusDesc = response.Status;
            
            if (response.forwardStatus == true) {
                $('#sendBtn1').removeAttr('disabled'); 
                $('#sendBackBtn').removeAttr('disabled');  
                $('#sendBtn3').removeAttr('disabled'); 
            }
            else {
                $('#sendBtn1').attr('disabled', 'disabled'); 
                $('#sendBackBtn').attr('disabled', 'disabled'); 
                $('#sendBtnchanges').attr('disabled', 'disabled');
                $('#sendBtn3').attr('disabled', 'disabled');
                $('#users').attr('disabled', 'disabled');
                $('#users1').attr('disabled', 'disabled');
            }
            if (response.ApprovedStatus == true) {
                if (response.forwardStatus == true) {
                    $('#sendBtn3').show();
                    $('#sendBtn3').removeAttr('disabled');
                }
                else {
                    $('#sendBtn3').hide(); 
                    $('#sendBtn3').attr('disabled', 'disabled'); 
                }
            }
            else {                
                $('#sendBtn3').hide();                 
                $('#sendBtnchanges').attr('disabled', 'disabled');                 
            }

            if (response.StatusId == 109) {
                $('#sendBtn1').attr('disabled', 'disabled');
                $('#sendBackBtn').attr('disabled', 'disabled');
            }
            else if (response.StatusId == 106) {
                if (response.RoleId == 1 || response.RoleId == 2 || response.RoleId == 4 || response.RoleId ==6) {
                    $('#sendBtnchanges').show();
                    $('#sendBtnchanges').removeAttr('disabled');
                }
                else {
                    $('#sendBtnchanges').hide();
                    $('#sendBtnchanges').attr('disabled', 'disabled');    
                }
                $('#sendBtn1').attr('disabled', 'disabled');
                $('#users').attr('disabled', 'disabled');
                $('#sendBtn3').hide();
            }
            if (response.RoleId == 1) {
                $('#sendBtn1').attr('disabled', 'disabled');
            }
            if (response.RoleId == loginUserRole.CW || response.RoleId == loginUserRole.OS || response.RoleId == loginUserRole.AD || response.RoleId == loginUserRole.DD)
                $('#sedbackdiv').hide();
            else
                $('#sedbackdiv').show();

            if ( response.StatusId == 106 || response.StatusId == 107 ) {
                $('#sendBackBtn').attr('disabled', 'disabled');                
            } 
            if (response.RoleId == 1 || response.RoleId == 2 || response.RoleId == 4 || response.RoleId == 6) {
                if (response.ApprovedStatus == true) {
                    if (response.forwardStatus == true) {
                        $('#sendBtn3').show();
                        $('#sendBtn3').removeAttr('disabled');
                    }
                    else {
                        $('#sendBtn3').attr('disabled', 'disabled');
                    }
                }               
            }
            else {                 
                $('#sendBtn3').attr('disabled', 'disabled');
                $('#sendBtn3').hide();
            }
            if (response.RoleId == 1) {
                $('#sendBtn1').hide();
                $('#users').hide();
                $('#sendto').hide();
            }
            
        }, error: function (e) {
            var _msg = "</br> Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });
}

function ForwardAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html();
    var RoleSelected = $('#users :selected').text();
    var path = "";
    var roleId = $('#users').val();
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    if (roleId == '0') {
        bootbox.alert('</br> Please Select Role');
    }
    else if (remarks == '') {
        bootbox.alert('</br> Please Enter Remarks');
    }
    else {
        bootbox.confirm({
            message: "</br> Are you sure you want to submit the Admission Notification to <b>" + RoleSelected + "</b> for Review?",
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
                    url: "/Admission/ForwardAdmissionNotification",
                    type: 'Get',
                    data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks, filePathName: path },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == true) {
                            bootbox.alert("</br> <b>" + $('#RecordLevel').html() +"</b> has Submitted Admission notification No : <b>" + notificatinNumber + "</b> To <b>" + RoleSelected + "</b> for review Successfully");
                            GetAdmissionNotificationDetails();
                            $('#ViewDiv').hide();
                            $('#GridListDiv').show();
                            $("#remarks").text("");
                            $('#users').val("choose");
                        }
                        else
                            bootbox.alert("failed");

                    }, error: function (result) {
                        bootbox.alert("Error", "</br> something went wrong");
                    }
                });
            }
            }
        });       
    }
/*code commented by sujit*/
    //else {
    //    bootbox.confirm('Do you want to forward the notification?', (confirma) => {
    //        if (confirma) {
    //            $.ajax({
    //                url: "/Admission/ForwardAdmissionNotification",
    //                type: 'Get',
    //                data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
    //                contentType: 'application/json; charset=utf-8',
    //                success: function (data) {
    //                    if (data == true) {
    //                        bootbox.alert("<b>" + $('#RecordLevel').html() +"</b> has Forwaded Admission notification No : <b>" + notificatinNumber + "</b> To <b>" + RoleSelected + "</b> Successfully");
    //                        GetAdmissionNotificationDetails();
    //                        $('#ViewDiv').hide();
    //                        $('#GridListDiv').show();
    //                        $("#remarks").text("");
    //                        $('#users').val("choose");
    //                    }
    //                    else
    //                        bootbox.alert("failed");

    //                }, error: function (result) {
    //                    bootbox.alert("Error", "something went wrong");
    //                }
    //            });
    //        }
    //    });       
    //}
}

function SenDBackAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html();
    var RoleSelected = $('#users1 :selected').text();

    var roleId = $('#users1').val();    
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    if (remarks == "") {
        bootbox.alert('</br> Please Enter Remarks');
    }
    else if (RoleSelected == 'Choose') {
        bootbox.alert('</br> Please select the role');
    }
    else {
        bootbox.confirm({
            message: "</br> Are you sure you want to send back the Admission Notification to <b>" + RoleSelected + "</b> for Correction/Clarification ?",
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
                    url: "/Admission/SendbackAdmissionNotification",
                    type: 'Get',
                    data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == true) {
                            bootbox.alert("</br> <b>" + $('#RecordLevel').html()+"</b> has Sent Back Admission notification No <b>" + notificatinNumber + "</b> successfully to <b>" + RoleSelected +"</b> for more Correction/Clarification");
                            $('#ViewDiv').hide();
                            $('#GridListDiv').show();
                            GetAdmissionNotificationDetails();
                        }
                        else
                            bootbox.alert("</br> send back failed");

                    }, error: function (result) {
                        bootbox.alert("Error", "</br> something went wrong");
                    }
                });
            }
            }
        });
    }    
/*code commented by sujit*/
    //else {
    //    bootbox.confirm('Do you want to send back the notification?', (confirma) => {
    //        if (confirma) {
    //            $.ajax({
    //                url: "/Admission/SendbackAdmissionNotification",
    //                type: 'Get',
    //                data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
    //                contentType: 'application/json; charset=utf-8',
    //                success: function (data) {
    //                    if (data == true) {
    //                        bootbox.alert("<b>" + $('#RecordLevel').html()+"</b> has Sent Back Admission notification No <b>" + notificatinNumber + "</b> is successfully to <b>" + RoleSelected +"</b> for more clarification");
    //                        $('#ViewDiv').hide();
    //                        $('#GridListDiv').show();
    //                        GetAdmissionNotificationDetails();
    //                    }
    //                    else
    //                        bootbox.alert("send back failed");

    //                }, error: function (result) {
    //                    bootbox.alert("Error", "something went wrong");
    //                }
    //            });
    //        }
    //    });
    //}    
}

function ApproveAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html();   
    var remarks = $("#remarks").val();
    if (remarks == '') {
        bootbox.alert('</br> Please Enter Remarks');
    }
    else {
        bootbox.confirm({
            message: "</br> Are you sure you want to approve the admission notification no: <b>" + notificatinNumber + "</b> ? ",
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
                    var roleId = 1;
                    var admsnNotId = $("#id").html();
                    $.ajax({
                        url: "/Admission/ApproveAdmissionNotification",
                        type: 'Get',
                        data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data == true) {
                                bootbox.alert("</br> <b>" + $('#RecordLevel').html() + "</b> has approved Admission notification No <b>" + notificatinNumber + "</b> successfully ");
                                $('#ViewDiv').hide();
                                $('#GridListDiv').show();
                                $("#remarks").text("");
                                GetAdmissionNotificationDetails();
                            }
                            else
                                bootbox.alert("</br> failed");

                        }, error: function (result) {
                            bootbox.alert("Error", "</br> something went wrong");
                        }
                    });
                }
            }
        });
    }
}

function ChangesAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html(); 
    var roleId = 8;
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    var loginName = $("#hdnSessionRole").data('value');
    var record = $('#RecordLevel').text();

    if (remarks == '') {
        bootbox.alert('</br> Please Enter Remarks');
    }
    else {
        bootbox.confirm({
            message: "</br> Are you sure you want to request for changes to Published Admission Notification to <b>" + record + "</b> ?",
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
                        url: "/Admission/ChangesAdmissionNotification",
                        type: 'Get',
                        data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data == true) {
                                bootbox.alert("</br> <b>" + loginName + "</b> has sent Admission notification No : <b>" + notificatinNumber + "</b>  To <b> Case Worker</b> for Change/Modification successfully");
                                $('#ViewDiv').hide();
                                $('#GridListDiv').show();
                                $("#remarks").text("");
                                GetAdmissionNotificationDetails();
                            }
                            else
                                bootbox.alert("</br> failed");

                        }, error: function (result) {
                            bootbox.alert("Error", "</br> something went wrong");
                        }
                    });
                }
            }
        });
    }
}

function PublishDetails(id) {
    bootbox.confirm({
        message: "</br> Are you sure you want to publish the Admission Notification details ?",
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
                    type: "Get",
                    url: "/Admission/PublishNotification",
                    dataType: 'json',
                    data: { notificationId: id },
                    success: function (response) {
                        if (response == "failed") {
                            bootbox.alert("</br> Fail to publish");
                        }
                        else {
                            bootbox.alert("</br> <b> Case worker </b>has Published Admission notification No : <b>" + response + "</b> Successfully");
                            GetAdmissionNotificationDetails();
                        }

                    }, error: function (e) {
                        bootbox.alert("</br> Something went wrong.");
                        $("#preloder, .loader").hide();
                    }
                });
            }
        }
    });
/*code commented by sujit*/
    //bootbox.confirm('Do you want to publish the notification details?', (confirma) => {
    //    if (confirma) {
    //        $.ajax({
    //            type: "Get",
    //            url: "/Admission/PublishNotification",
    //            dataType: 'json',
    //            data: { notificationId: id },
    //            success: function (response) {
    //                if (response == "failed") {
    //                    bootbox.alert("Fail to publish");
    //                }
    //                else {
    //                    bootbox.alert("<b> Case worker </b>has Published Admission notification No <b>" + response + "</b> Successfully");
    //                    GetAdmissionNotificationDetails();
    //                }                       
                                            
    //            }, error: function (e) {                    
    //                bootbox.alert("Something went wrong.");
    //                $("#preloder, .loader").hide();
    //            }
    //        });
    //    }
    //});    
}

function GetClientReport1(btnid) {
    var urlPath = "";
    var btnname = $(btnid).attr("id");
    var NotifId = $('#notifynum').html();
    NotifId = NotifId.replace(/\//g, '_');
    NotifId = NotifId.replaceAll('-', '_');
    if (btnname == "ViewNotification") {
        urlPath = "../../Content/Template/Notification_" + NotifId + "_1" + ".pdf";
    }
    //} else if (btnname == "EditableNotification") {
    //  urlPath = "../../Content/Template/Notification_" + NotifId + "_1" + ".docx";
    //}
    //else {
    //  urlPath = "../../Content/Template/Notification_" + NotifId + ".pdf";
    //}
    window.open(urlPath, '_blank');
}


function GetAdmissionNotification() {
    var type = $('#courseType').val();
    var year = new Date().getFullYear()
    var acayear = year + "-" + (year + 1);
    if (type == 100)
        type = "TRG-2";
    else
        type = "TRG-1";

    var notificationNum = "DITE/TRG/" + type + "/CR1/" + acayear
    $('#notifNumber').val(notificationNum);
}



function disablesendToRoles(user, user1, btn, apprbtn) {
    if ($('#' + user).val() == '0') {
        $('#' + user1).removeAttr('disabled');
        $('#' + btn).removeAttr('disabled');
        $('#' + apprbtn).removeAttr('disabled');
        $('#sendBtn3').removeAttr('disabled');
    }
    else {
        $('#' + user1).attr('disabled', 'disabled');
        $('#' + btn).attr('disabled', 'disabled');
        $('#' + apprbtn).attr('disabled', 'disabled');
        $('#sendBtn3').attr('disabled', 'disabled');
    }
}
function GetApplicantTypechange() {
    var apptype = $('#applicantType').val();
    var type = $('#courseType').val();
    var year = new Date().getFullYear()
    var acayear = year + "-" + (year + 1);
    var ntfNum = $('#notifNumber').val();  
    if (type == 100)
        type = "TRG-2";
    else
        type = "TRG-1";

    if (apptype == 1)
        apptype = "1";
    else
        apptype = "2";

    var notificationNum = "DITE/TRG/" + type + "/CR1/" + acayear + "/" + apptype
    $('#notifNumber').val(notificationNum);
}


function fnUploadNotifDoc(e) {
    var id = "fileUpload1";
    //var id = "fileUpload" + $('#tblViewNotification').DataTable().data().count();
    //$("#" + id + "Err").hide();
    var fileData = new FormData();
    var fileUpload = $("#" + id).get(0);
    if ($("#" + id).val() != "") {
        var files = fileUpload.files;
        for (var i = 0; i < files.length; i++) {
            fileData.append("file", files[i]);
        }

        fileData.append(
            "Exam_Notif_Number", $('#notifynum').text()
        );
        fileData.append(
            "Admission_Notif_Id", ExamNotifyId
        );

        $.ajax({
            type: "POST",
            url: "/Admission/UploadNotifDoc",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data == "Failed") {
                    bootbox.alert("<br><br>There is error in while uploading file");
                }
                else {
                    bootbox.alert({
                        message: "<br><br>File Uploaded Successfully",
                        //callback: function () { location.reload(true); }
                    });
                    $('#ViewNotificationModal').modal('hide');
                }
            }
        });
    } else {
        //$("#" + id + "Err").show();
        //$("#" + id).focus();
        if ($("#" + id).is(":enabled")) {
            bootbox.alert("<br><br>No File available to Upload. Please upload a file.");
        } else {
            $('#ViewNotificationModal').modal('toggle');
        }
    }


}