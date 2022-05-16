var AdmVerOfficerMenu = "Admission";
$(document).ready(function () {
  //GetInstitutes();
  GetCourses("CourseType");
  GetCourses("CourseType1");
  GetCourses('ApplicantType');
  GetOfficers();//officer tab
  GetApplicants("");//applicant tab
  GetverificationMapping();//verification mapping tab
  GetTotalApplicantOfficer();
  GetInActiveOfficernMapping();
  GetActiveOfficers();
  $('#PhoneNoEdit').bind("cut copy paste", function (e) {
    e.preventDefault();
  });
  $('#PhoneNo').bind("cut copy paste", function (e) {
    e.preventDefault();
  });

  //GetSessionCurrentYear();
  GetSessionYear('Session');
  GetSessionYear('Session1');
    GetSessionYear('Session12')
    //Reconcile Tab
    GetSessionYear('ddlSessionDvF')
    GetCourses("ddlCourseTypeDvF");
    //GetApplicantType("ddlApplicantTypeDvF");

    GetApplicationMode("ddlApplicantTypeDvF");

    GetDataDocumentsVerificationFee();

    //Document Verification status tab 
    GetDataToVerifyDocuments();
    GetSessionYear('Session22')
    GetCourses("CourseType22");
   // GetApplicantType("ApplicantTypeSelect");
    GetApplicationMode("ApplicantTypeSelect");
    GetDivisionsDDp("divisionDdp");
    GetDivisionsDDp("divisionverificationstatus");
    if ($("#hdnDiv_ID").data('value') != null && $("#hdnDiv_ID").data('value') != "") {
        GetDistrictsDDp('districtDdp1', $("#hdnDiv_ID").data('value'))
        GetDistrictsDDp('districtverificationstatus', $("#hdnDiv_ID").data('value'))

    }
});

//======== START OFFICER=========
function GetOfficers() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetOfficers",
        contentType: "application/json",
        data: { roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
        success: function (data) {
            $('#OfficerTable1').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                /*"autocomplete": false,*/
                preDrawCallback: function () {
                    let el = $('#OfficerTable1_filter label');
                    if (!el.parent('form').length) {
                        el.wrapAll('<form></form>').parent()
                            .attr('autocomplete', false)
                            .attr('autofill', false);
                    }
                },
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'KGIDNo', 'title': 'KGID No.', 'className': 'text-center' },
                    { 'data': 'OfficerName', 'title': 'Verification Officer Name', 'className': 'text-center' },
                    { 'data': 'Designation', 'title': 'Designation', 'className': 'text-left' },
                    { 'data': 'PhoneNo', 'title': 'Phone Number', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
                    {
                        'data': 'OfficerId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='EditOfficer(" + oData.OfficerId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditOfficerModal' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='DeleteOfficer(" + oData.OfficerId + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function AddOfficer() {
    
  $('#OfficerNameErr').text('');
  $('#DesignationErr').text('');
  $('#PhoneNoErr').text('');
  $('#EmailIdErr').text('');
  $('#OfficerLoginPwdErr').text('');
  $('#OfficerLoginUserNameErr').text('');
  var officerName = $('#OfficerName').val();
  var designation = $('#Designation').val();
  //var OfficerLoginUserName = $('#OfficerLoginUserName').val();
  //var OfficerLoginPwd = $('#OfficerLoginPwd').val();
  var phoneNo = $('#PhoneNo').val();
    var emailId = $('#EmailId').val();
    var kgidnoa = $('#kgidno').val();
  var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;

  if (officerName == '' && designation == '' && phoneNo == '' && emailId == '' /*&& OfficerLoginUserName == '' && OfficerLoginPwd == ''*/) {
    $('#OfficerNameErr').text('enter the officer name');
    $('#DesignationErr').text('select the designation');
    $('#PhoneNoErr').text('enter the phone no');
    $('#EmailIdErr').text('enter the email');
    //$('#OfficerLoginUserNameErr').text('enter officer login user name');
    //$('#OfficerLoginPwdErr').text('enter officer login password');
  }
  else
    if (officerName == '') {
      $('#OfficerNameErr').text('enter the officer name');
    } else if (designation == '') {
      $('#DesignationErr').text('select the designation');
    } else if (phoneNo == '') {
      $('#PhoneNoErr').text('enter the phone no');
    } else if (phoneNo.length > 10 || phoneNo.length < 10) {
      $('#PhoneNoErr').text('enter the valid phone no');
    } //else if (OfficerLoginUserName == '') {
    //  $('#OfficerLoginUserNameErr').text('enter officer login user name');
    //} else if (OfficerLoginPwd == '') {
    //  $('#OfficerLoginPwdErr').text('enter officer login password');
    //}
    else if (emailId == '') {
      $('#EmailIdErr').text('enter the email');
    } else
      if (!testEmail.test(emailId)) {
        $('#EmailIdErr').text('enter the valid email');
      }
      else {
        //$('#OfficerLoginPwd').val(SHAEncryption(OfficerLoginPwd));
        //OfficerLoginPwd = $('#OfficerLoginPwd').val();
        var Data = {
            OfficerName: officerName,
          //OfficerLoginPwd: OfficerLoginPwd.toString(),
          //OfficerLoginUserName: OfficerLoginUserName,
          Designation: designation,
          PhoneNo: phoneNo,
            EmailId: emailId,
            KGIDNo: kgidnoa,
          RoleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? RoleId = loginUserRole.AO : RoleId = loginUserRole.VO)
        };
        bootbox.confirm({
          message: "Do you want to add verification officer details ?",
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
                url: "/Admission/AddOfficer",
                type: 'Get',
                data: Data,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == "success") {
                        //bootbox.alert("Officer details added successfully", function (confirmed) {
                        //    if (confirmed) {
                        //      $('#OfficerModal').modal('toggle');

                        //        location.reload();
                        //    }
                        //});

                        $("#OfficerModal").removeClass("in");
                        $(".modal-backdrop").remove();
                        $('body').removeClass('modal-open');
                        $('body').css('padding-right', '');
                        $("#OfficerModal").hide();
                    bootbox.alert("Verification officer details added successfully!!!");

                    //  $('#OfficerModal').modal('hide');
                    ////    $("#OfficerModal .close").click();
                        GetOfficers();

                    GetTotalApplicantOfficer();
                      GetActiveOfficers();
                  }
                  else if (data == "exist") {
                    bootbox.alert("Officer details already exist");
                  }
                  else {
                    bootbox.alert("failed");
                  }
                }, error: function (result) {
                  bootbox.alert("Error", "something went wrong");
                }
              });
            }
          }
        });

        /*code commented by sujit*/
        //bootbox.confirm('Do you want to add officer details?', (confirma) => {
        //    if (confirma) {
        //        $.ajax({
        //            url: "/Admission/AddOfficer",
        //            type: 'Get',
        //            data: Data,
        //            contentType: 'application/json; charset=utf-8',
        //            success: function (data) {
        //                if (data == "success") {
        //                    bootbox.alert("Officer details added successfully");
        //                    GetOfficers();
        //                    $('#OfficerModal').modal('hide');
        //                    GetTotalApplicantOfficer();
        //                    GetActiveOfficers();
        //                }
        //                else if (data == "exist") {
        //                    bootbox.alert("Officer details already exist");
        //                }
        //                else {
        //                    bootbox.alert("failed");
        //                }
        //            }, error: function (result) {
        //                bootbox.alert("Error", "something went wrong");
        //            }
        //        });
        //    }
        //});

      }

}

function EditOfficer(offid) {
  $('#OfficerNameEditErr').text('');
  $('#DesignationEditErr').text('');
  $('#PhoneNoEditErr').text('');
  $('#EmailIdEditErr').text('');
  $('#ItiInstituteEditErr').text('');
  $('#OfficerLoginUserNameErr').text('');
  $('#OfficerLoginPwdErr').text('');
  $.ajax({
    url: "/Admission/EditOfficer",
    type: 'Get',
    data: { id: offid },
    contentType: 'application/json; charset=utf-8',
    success: function (data) {
      if (data != null) {
        $('#OfficerIdEdit').val(data.OfficerId);
        $('#OfficerNameEdit').val(data.OfficerName);
        $('#DesignationEdit').val(data.Designation);
        $('#PhoneNoEdit').val(data.PhoneNo);
          $('#EmailIdEdit').val(data.EmailId);
          $('#kgidnoe').val(data.KGIDNo);
        $('#ItiInstituteEdit').val(data.InstituteName);
        $('#OfficerLoginUserNameEdit').val(data.OfficerLoginUserName);
        $('#OfficerLoginPwdEdit').val(data.OfficerLoginPwd);
        if (data.Status == false)
          $('#Status option[value="0"]').attr("selected", true);
        else
          $('#Status option[value="1"]').attr("selected", true);
      } else
        alert('failed');

    }, error: function (result) {
      alert("Error", "something went wrong");
    }
  });
}

function UpdateOfficer() {
    $('#OfficerNameEditErr').text('');
    $('#DesignationEditErr').text('');
    $('#PhoneNoEditErr').text('');
    $('#EmailIdEditErr').text('');
    $('#ItiInstituteEditErr').text('');
    $('#OfficerLoginUserNameEditErr').text('');
    $('#OfficerLoginPwdEditErr').text('');
    var officerName = $('#OfficerNameEdit').val();
    var designation = $('#DesignationEdit').val();
    var OfficerLoginPwd = $('#OfficerLoginPwdEdit').val();
    var OfficerLoginUserName = $('#OfficerLoginUserNameEdit').val();
    var phoneNo = $('#PhoneNoEdit').val();
    var emailId = $('#EmailIdEdit').val();
    var kgidedit = $('#kgidnoe').val();
    if ($('#Status').val() == 0) {
        var status = false;
    }
    else {
        var status = true;
    }
    var officerid = $('#OfficerIdEdit').val();
    if (officerName == '') {
        $('#OfficerNameEditErr').text('enter the officer name');
    } else if (designation == '') {
        $('#DesignationEditErr').text('select the designation');
    } else if (OfficerLoginUserName == '') {
        $('#OfficerLoginUserNameEditErr').text('enter officer login user name');
    } else if (OfficerLoginPwd == '') {
        $('#OfficerLoginPwdEditErr').text('enter officer login password');
    } else if (phoneNo == '') {
        $('#PhoneNoEditErr').text('enter the phone no');
    } else if (phoneNo.length > 10 || phoneNo.length < 10) {
        $('#PhoneNoEditErr').text('enter the valid phone no');
    }
    else
        if (emailId == '') {
            $('#EmailIdEditErr').text('enter the email');
        }
        else {
            var Data = {
                OfficerId: officerid,
                OfficerName: officerName,
                OfficerLoginUserName: OfficerLoginUserName,
                OfficerLoginPwd: OfficerLoginPwd,
                Designation: designation,
                PhoneNo: phoneNo,
                EmailId: emailId,
                Status: status,
                KGIDNo: kgidedit,
                RoleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? RoleId = loginUserRole.AO : RoleId = loginUserRole.VO)
            };
            bootbox.confirm({
                message: "Do you want to update verification officer details ?",
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
                            url: "/Admission/UpdateOfficer",
                            type: 'Get',
                            data: Data,
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                if (data == true) {

                                    $("#EditOfficerModal").removeClass("in");
                                    $(".modal-backdrop").remove();
                                    $('body').removeClass('modal-open');
                                    $('body').css('padding-right', '');
                                    $("#EditOfficerModal").hide();
                                    //bootbox.alert("Verification officer details updated successfully!!!");
                                    //GetOfficers();
                                    ////$('#EditOfficerModal').modal('hide');
                                    //GetInActiveOfficernMapping();
                                    //GetTotalApplicantOfficer();
                                    //GetActiveOfficers();

                                    var _msg = "Verification officer details updated successfully!!!";
                                    bootbox.alert(_msg, function () {
                                        GetOfficers();
                                        //$('#EditOfficerModal').modal('hide');
                                        GetInActiveOfficernMapping();
                                        GetTotalApplicantOfficer();
                                        GetActiveOfficers();

                                    });
                                }
                                else
                                    bootbox.alert("failed");

                            }, error: function (result) {
                                bootbox.alert("Error", "something went wrong");
                            }
                        });
                    }
                }
            });
            /*code commented by sujit*/
            //bootbox.confirm('Do you want to update officer details?', (confirma) => {
            //    if (confirma) {
            //        $.ajax({
            //            url: "/Admission/UpdateOfficer",
            //            type: 'Get',
            //            data: Data,
            //            contentType: 'application/json; charset=utf-8',
            //            success: function (data) {
            //                if (data == true) {
            //                    bootbox.alert("Officer details updated successfully");
            //                    GetOfficers();
            //                    $('#EditOfficerModal').modal('hide');
            //                    GetInActiveOfficernMapping();
            //                    GetTotalApplicantOfficer();
            //                    GetActiveOfficers();
            //                }
            //                else
            //                    bootbox.alert("failed");

            //            }, error: function (result) {
            //                bootbox.alert("Error", "something went wrong");
            //            }
            //        });
            //    }
            //});
        }

}

function DeleteOfficer(id) {
  bootbox.confirm({
    message: "Do you want to delete verification officer details ?",
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
          url: "/Admission/DeleteOfficer",
          type: 'Get',
          data: { id: id },
          contentType: 'application/json; charset=utf-8',
          success: function (data) {
            if (data == true) {
              bootbox.alert('Verification officer details deleted successfully!!!');
              GetOfficers();
            } else
              bootbox.alert('failed');

          }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
          }
        });
      }
    }
  });
  //bootbox.confirm('Do you want to delete officer details?', (confirma) => {
  //    if (confirma) {
  //        $.ajax({
  //            url: "/Admission/DeleteOfficer",
  //            type: 'Get',
  //            data: { id: id },
  //            contentType: 'application/json; charset=utf-8',
  //            success: function (data) {
  //                if (data == true) {
  //                    bootbox.alert('deleted successfully');
  //                    GetOfficers();
  //                } else
  //                    bootbox.alert('failed');

  //            }, error: function (result) {
  //                bootbox.alert("Error", "something went wrong");
  //            }
  //        });
  //    }
  //});
}

function ClearOfficerFields() {
  $('#OfficerNameErr').text('');
  $('#DesignationErr').text('');
  $('#PhoneNoErr').text('');
  $('#EmailIdErr').text('');
  $('#ItiInstituteErr').text('');
  $('#OfficerName').val('');
  $('#Designation').val('');
  $('#PhoneNo').val('');
  $('#EmailId').val('');
  $('#ItiInstitute').val('');
  $('#OfficerLoginUserName').val('');
  $('#OfficerLoginPwd').val('');
  $('#OfficerLoginPwdErr').text('');
  $('#OfficerLoginUserNameErr').text('');

}
//=======END OFFICER==============


//=======START APPLICANTS==========
function GetApplicants(id) {
  //if ($('#Session12').val() == 'choose' && $('#ApplicantType').val() == 'choose') {
  
  var ApplYear = 0;
  var ApplType = 0;
  var IsValid = false;
  if (id != 0) {
    ApplYear = $('#Session12').val();
    ApplType = $('#ApplicantType').val();
    if (ApplYear == 'choose') {
      bootbox.alert("select the Session");
      IsValid = true;
    }
    else if (ApplType == 'choose') {
      bootbox.alert("select the course type");
      IsValid = true;
    }
  }
  if (!IsValid) {
    $.ajax({
      type: "GET",
      url: "/Admission/GetApplicants",
      contentType: "application/json",
      data: { 'applicantId': ApplType, 'year': ApplYear, roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
      success: function (data) {
        $('#ApplicantTable').DataTable({
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
              //        else
              //        {
              //            $(nTd).html("Offline");
              //        }
              //    }
              //},

            { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left' },
            { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left' },
            { 'data': 'Apdate', 'title': 'Applied Date', 'className': 'text-left' },
          //  { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
          ]
        });
      }, error: function (result) {
        bootbox.alert("Error", "something went wrong");
      }
    });
  }  
}



//function GetSessionCurrentYear() {
//  $("#Session12").empty();
//  $("#Session12").append('<option value="0">choose</option>');
//  /*$("#Session").empty();*/
//  /* $("#Session").append('<option value="0">choose</option>');*/

//  var year = (new Date()).getFullYear();
//  var current = year;
//  year -= 4;
//  $.ajax({
//    url: "/Admission/GetExamYear",
//    type: 'Get',
//    contentType: 'application/json; charset=utf-8',
//    success: function (data) {
//      for (var i = 0; i < 5; i++) {
//        if ((year + i) == current) {
//          $("#Session12").append('<option selected value="' + (year + i) + '">' + (year + i) + '</option>');
//          /*  $("#Session").append('<option selected value="' + (year + i) + '">' + (year + i) + '</option>');*/
//        }
//        else {
//          $("#Session12").append('<option value="' + (year + i) + '">' + (year + i) + '</option>');
//          /* $("#Session").append('<option value="' + (year + i) + '">' + (year + i) + '</option>');*/
//        }
//      }
//    }, error: function (result) {
//      bootbox.alert("Error", "something went wrong");
//    }
//  });
//}

//=======END APPLICANTS==========


//=======START VERIFICATION MAPPING==========
function GetverificationMapping() {
  //if ($('#Session').val() == "choose" && $('#CourseType').val() == "choose") {
  if ($('#Session').val() == "choose" && ($('#CourseType').val() == "101" || $('#CourseType').val() == null)) {
    $.ajax({
      type: "GET",
      url: "/Admission/GetVerificationOfficerDetails",
      contentType: "application/json",
      data: { 'year': 0, 'courseType': 0, roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
        success: function (data) {
            GetApplicants("");
            if (data.length > 0) {
                $('#btnverificationTableExportData').show();
            } else {
                $('#btnverificationTableExportData').hide();
}
        $('#verificationTable').DataTable({
          data: data,
            //searching: true,    
          "destroy": true,
            //"bFilter": false,
          "bSort": true,
            //dom: fnSetDTExcelBtnPos(),
          //  buttons: [
          //      {
          //          extend: 'excel',
          //          text: 'Download Excel',
          //          exportOptions: {
          //              columns: [0, 1, 2, 3, 4, 5, 6, 7
          //              ]
          //          }
          //      }
          //  ],
          columns: [
              { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
              { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
              { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
              { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

            { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
              { 'data': 'PhoneNo', 'title': 'Mobile Number', 'className': 'text-left' },
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
            { 'data': 'Designation', 'title': 'Designation', 'className': 'text-left' },
            { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' }
          ]
        });
      }, error: function (result) {
        alert("Error", "something went wrong");
      }
    });
  }
  else {
    var year = $('#Session').val();
    var courseType = $('#CourseType').val();
    if (year == 'choose') {
      bootbox.alert("select the session");
    } else
      if (courseType == 'choose') {
        bootbox.alert("select the course type");
      }
      else {
        $.ajax({
          type: "GET",
          url: "/Admission/GetVerificationOfficerDetails",
          contentType: "application/json",
          data: { 'year': year, 'courseType': courseType, roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
            success: function (data) {
                GetApplicants("");

                if (data.length > 0) {
                    $('#btnverificationTableExportData').show();
                } else {
                    $('#btnverificationTableExportData').hide();
                }
            $('#verificationTable').DataTable({
              data: data,
                searching: true,
              "destroy": true,
                "bFilter": false,
              "bSort": true,
                //dom: fnSetDTExcelBtnPos(),
              //  buttons: [
              //      {
              //          extend: 'excel',
              //          text: 'Download Excel'                       
              //      }
              //  ],
              columns: [
                  { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                  { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                  { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                  { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                  { 'data': 'PhoneNo', 'title': 'Mobile Number', 'className': 'text-left' },
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
                { 'data': 'Designation', 'title': 'Designation', 'className': 'text-left' },
                { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' }
              ]
            });
          }, error: function (result) {
            alert("Error", "something went wrong");
          }
        });
      }

  }

}

function GetTotalApplicantOfficer() {
  $.ajax({
    type: "GET",
    url: "/Admission/GetTotalApplicantOfficer",
    contentType: "application/json",
    data: { roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
    success: function (data) {
      if (data != null) {
        $('#officers').text(data.TotalOfficers);
        $('#Applicants').text(data.TotalApplicants);
        $('#ReassignOfficers').text(data.TotalOfficers);
        $('#ReassignApplicants').text(data.InactiveOfficerApplicants);
        $('#ReassignApplicants1').text(data.InactiveOfficerApplicants);
      }
    }, error: function (result) {
      alert("Error", "something went wrong");
    }
  });
}

function MapApplicantToOfficer() {
  if ($('#Applicants').text() == 0) {
    bootbox.alert("No new applicants are available!!!");
  }
  else {
    bootbox.confirm({
      message: "Do you want to assign the applicants to verification officer?",
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
            url: "/Admission/MapApplicantToOfficer",
            data: { roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
            contentType: "application/json",
            success: function (data) {
              if (data == true) {

                  var _msg = "Verification officer assigned successfully!!!";
                  bootbox.alert(_msg, function () {
                      GetTotalApplicantOfficer();
                      GetverificationMapping();

                  });
              //  bootbox.alert("assigned successfully");
              }
              else
                bootbox.alert("failed");
            }, error: function (result) {
              bootbox.alert("Error", "something went wrong");
            }
          });
        }
      }
    });
    /*code commented by sujit*/
    //bootbox.confirm('Do you want to assign the applicants to verification officer?', (confirma) => {
    //    if (confirma) {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Admission/MapApplicantToOfficer",
    //            contentType: "application/json",
    //            success: function (data) {
    //                if (data == true) {
    //                    GetTotalApplicantOfficer();
    //                    GetverificationMapping();
    //                    bootbox.alert("assigned successfully");
    //                }
    //                else
    //                    bootbox.alert("failed");
    //            }, error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //            }
    //        });
    //    }
    //});
  }
}

//=======END VERIFICATION MAPPING==========

//==========START Re-assign In-active Officer List====
function GetInActiveOfficernMapping() {
  //if ($('#Session1').val() == 'choose' && $('#CourseType1').val()=='choose') {        
  if ($('#Session1').val() == 'choose' && ($('#CourseType1').val() == '101' || $('#CourseType1').val() == null)) {
    $.ajax({
      type: "GET",
      url: "/Admission/GetInactiveOfficerApplicants",
      contentType: "application/json",
      data: { 'year': 0, 'courseType': 0 },
      success: function (data) {
        $('#ReassignTable').DataTable({
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
            { 'data': 'Designation', 'title': 'Designation', 'className': 'text-left' }
          ]
        });
      }, error: function (result) {
        alert("Error", "something went wrong");
      }
    });
  } else {
    var year = $('#Session1').val();
    var courseType = $('#CourseType1').val();
    if (year == 'choose') {
      bootbox.alert("select the session");
    } else
      if (courseType == 'choose') {
        bootbox.alert("select the course type");
      }
      else {
        $.ajax({
          type: "GET",
          url: "/Admission/GetInactiveOfficerApplicants",
          contentType: "application/json",
          data: { 'year': year, 'courseType': courseType },
          success: function (data) {
            $('#ReassignTable').DataTable({
              data: data,
              "destroy": true,
              "bSort": true,
              columns: [
                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                  { 'data': 'OfficerName', 'title': 'Verification Officer Name', 'className': 'text-left' },
                  { 'data': 'Designation', 'title': 'Designation', 'className': 'text-left' },
                  { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' }

              ]
            });
          }, error: function (result) {
            alert("Error", "something went wrong");
          }
        });
      }
  }
}
function ReMapApplicantToOfficer() {
    if ($('#ReassignApplicants').text() == 0) {
        bootbox.alert("No new applicants available!!!");
    }
    else {
        bootbox.confirm({
            message: "Do you want to Re-map the applicants to verification officers?",
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
                        url: "/Admission/ReMapApplicantToOfficer",
                        data: { roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
                        contentType: "application/json",
                        success: function (data) {
                            if (data == true) {
                                //bootbox.alert("applicants has Re-assigned to verification officers successfully");
                                var _msg = "Applicants has Re-assigned to verification officers successfully!!!";
                                bootbox.alert(_msg, function () {
                                    GetInActiveOfficernMapping();
                                    GetverificationMapping();
                                    GetTotalApplicantOfficer();

                                });
                            }
                            else
                                bootbox.alert("failed");
                        }, error: function (result) {
                            bootbox.alert("Error", "something went wrong");
                        }
                    });
                }
            }
        });
        /*code commented by sujit*/
        //bootbox.confirm('Do you want to Re-map the applicants to verifiaion officers?', (confirma) => {
        //    if (confirma) {
        //        $.ajax({
        //            type: "GET",
        //            url: "/Admission/ReMapApplicantToOfficer",
        //            contentType: "application/json",
        //            success: function (data) {
        //                if (data == true) {
        //                    GetInActiveOfficernMapping();
        //                    GetverificationMapping();
        //                    GetTotalApplicantOfficer();
        //                    bootbox.alert("applicants has Re-assigned to verification officers successfully");
        //                }
        //                else
        //                    bootbox.alert("failed");
        //            }, error: function (result) {
        //                bootbox.alert("Error", "something went wrong");
        //            }
        //        });
        //    }
        //});
    }
}
function GetActiveOfficers() {
    $("#officerNames").empty();
    $("#officerNames").append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetActiveOfficers",
        type: 'Get',
        data: { roleId: ($("#hdnAdmVerOfficerMenu").data('value') == AdmVerOfficerMenu ? roleId = loginUserRole.AO : roleId = loginUserRole.VO) },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#officerNames").append($("<option/>").val(this.OfficerId).text(this.OfficerName));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}
$('#NoApplicants').on('keypress', function (event) {
    var regex = new RegExp("^[0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});


$("#add-row-new").click(function () {
  var $tableBody = $('#Table-Trade').find("tbody");
  var $trLast = $tableBody.find("tr:last");
  var $trNew = $trLast.clone();
  $trNew.find("#tabApplicants").val(0);
  $trNew.find(".add-trade-remove").click(function () {
    var lenght = $('#Table-Trade tbody tr').length;
    if (lenght > 1) {
      $(this).closest("tr").remove();
      var total = 0;
      $("#Table-Trade").find(".text-multi-units").each(function () {
        total += parseFloat($(this).text())
      });
      $("#Units-Add").text(total);
    }
    else {
      alert("Atleast one row required")
    }
  });
  $tableBody.append($trNew);
});

$(".add-trade-remove").click(function () {
  var lenght = $('#Table-Trade tbody tr').length;
  if (lenght > 1) {
    $(this).closest("tr").remove();
  }
  else {
    alert("Atleast one row required");
  }
});

function ReMapApplicantIndividualOff() {
  var _shift_table = $("#Table-Trade tbody");
  var sendObj = [];
  _shift_table.find("tr").each(function (len) {
    var $tr = $(this);
    var officer = $tr.find("#officerNames").val();
    var applicant = $tr.find("#tabApplicants").val();

    var obj = {
      officer: officer,
      applicant: applicant
    };
    sendObj.push(obj);
  });

  var count = [];
  $.each(sendObj, function (key, value) {
    $.each(value, function (k, v) {
      if (k == 'applicant')
        count.push(v);
    })
  });
  var total = 0;
  $.each(count, function (i, v) {
    total += +v;
  });

  var Availno = $('#ReassignApplicants1').text();
  var Addno = $('#tabApplicants').val();
  if ($('#officerNames').val() == 'choose') {
    bootbox.alert("Select the officer");
  } else if (Addno == '') {
    bootbox.alert("Enter the number of applicants");
  } else
    if (total > Availno) {
      bootbox.alert("Applicant count should be a max total No. available applicants");
    }
    else {
      var finalObj = JSON.stringify({ list: sendObj });
      bootbox.confirm({
        message: "Do you want to Re-map the applicants to verifiaion officers ?",
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
              url: '/Admission/ReMapApplicantIndividualOff',
              dataType: 'json',
              contentType: 'application/json; charset=utf-8',
              type: 'POST',
              data: finalObj,
              success: function (data) {
                if (data == true) {
                  bootbox.alert("Re-assigned successfully");
                  GetInActiveOfficernMapping();
                  GetverificationMapping();
                  GetTotalApplicantOfficer();
                  $('#ReassignOfficerModal').modal("hide");
                }
                else {
                  bootbox.alert("failed")
                  console.log(data.status);
                }
              }
            });
          }
        }
      });

      /*code commented by sujit*/
      //bootbox.confirm('Do you want to Re-map the applicants to verifiaion officers?', (confirma) => {
      //    if (confirma) {
      //        $.ajax({
      //            url: '/Admission/ReMapApplicantIndividualOff',
      //            dataType: 'json',
      //            contentType: 'application/json; charset=utf-8',
      //            type: 'POST',
      //            data: finalObj,
      //            success: function (data) {
      //                if (data == true) {
      //                    bootbox.alert("Re-assigned successfully");
      //                    GetInActiveOfficernMapping();
      //                    GetverificationMapping();
      //                    GetTotalApplicantOfficer();
      //                    $('#ReassignOfficerModal').modal("hide");
      //                }
      //                else {
      //                    bootbox.alert("failed")
      //                    console.log(data.status);
      //                }
      //            }
      //        });
      //    }
      //});

    }
}

function ResetTable() {
  var lenght = $('#Table-Trade tbody tr').length;
  if (lenght > 1) {
    for (i = 1; i < lenght; i++) {
      $('#Table-Trade tbody tr:last-child').remove();
    }
  }
  if ($('#ReassignApplicants').text() == 0) {
      bootbox.alert('No new applicants available!!!');
  }
  else {
    $('#ReassignOfficerModal').modal("show");
  }
}
//==========END Re-assign In-active Officer List====




//=====Start validations======
$('#PhoneNo').keypress(function (event) {
  var keycode = event.which;
  if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
    event.preventDefault();
  }
});

$('#PhoneNoEdit').keypress(function (event) {
  var keycode = event.which;
  if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
    event.preventDefault();
  }
});

function fnGetUserDataFromKGIDNumber() {
    
    $.ajax({
        type: "GET",
        data: { KGIDNumber: $("#kgidno").val() },
        url: "/Admin/GetUserDataByKGIDNumber",
        contentType: "application/json",
        success: function (data) {
            if (data[0].KGIDUserID != null && data.length > 0 && data[0].message == "0") {
                if (data.KGIDUserID != 0) { $("#kgidno").val(data[0].KGIDUserID);}
             
                $("#OfficerName").val(data[0].EEName);
                $("#Designation").val('Verification Officer');
                $("#PhoneNo").val(data[0].phoneNum);
                $("#EmailId").val(data[0].email);
                //$("#txtPwd").val(data[0].pwd);
            }
        else if(data[0].message != "1" ) {
        bootbox.alert("User doesn't exist in our database.");
    }
                else if (data[0].message != "2") {
        bootbox.alert("User is deactivated,please activate for user mapping!!");

    }
        }
    });
}

//=====End validations======

//============Document Re-Concile Fee ===================//
function GetDataDocumentsVerificationFee() {
    var session = $('#ddlSessionDvF :selected').val();
    var course = $('#ddlCourseTypeDvF :selected').val();
    var ApplicantType = $('#ddlApplicantTypeDvF :selected').val();
    var Division = $('#divisionDdp :selected').val();
    var District = $('#districtDdp1 :selected').val();
    var Taluk = $('#talukDdp :selected').val();
    var Institute = $('#instituteDdp :selected').val();
    if (ApplicantType == "choose" || ApplicantType == undefined) {
        ApplicantType = null;
    }
    if (District == "choose" || District == undefined) {
        District = 0;
    }
    if (Taluk == "choose" || Taluk == undefined) {
        Taluk = 0;
    }
    if (Institute == "choose" || Institute == undefined) {
        Institute = 0;
    }
    if (Division == "choose" || Division == undefined || Division == "") {
        Division = 0;
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetDataDocumentsVerificationFee",
        contentType: "application/json",
        data: { year: session, courseType: course, applicanType: ApplicantType, division_id: Division ,district_lgd_code: District, taluk_lgd_code: Taluk ,InstituteId: Institute},
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

                    { 'data': 'MisCode', 'title': 'Mis Code', 'className': 'text-left' },
                    { 'data': 'Divisionname', 'title': 'Division Name', 'className': 'text-left' },
                    { 'data': 'Districtname', 'title': 'District Name', 'className': 'text-left' },
                    { 'data': 'Talukname', 'title': 'Taluk Name', 'className': 'text-left' },

                    { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                    
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
                        'data': 'DocumentVeriFeePymtDate', 'title': 'Amount Paid Date', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DocumentVeriFeePymtDate);
                            $(nTd).html(date);
                        }
                    },
                    
                    { 'data': 'DocVeriFeeReceiptNumbers', 'title': 'Receipt Number', 'className': 'text-left' },
                    { 'data': 'Treasury_Receipt_No', 'title': 'Treasury Receipt No.', 'className': 'text-left' },

                    {
                        'title': 'Receipt (PDF)',
                        render: function (data, type, row) {
                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        }
                    },

                    //{
                    //    'title': 'Receipt (Pdf)',
                    //    render: function (data, type, row) {
                    //        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
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

//==========End Document Re-Concile Fee ==============//


//========== Document Verification status ============//
function GetDataToVerifyDocuments() {
    var year = $('#Session22').val();
    var courseType = $('#CourseType22').val();
    var appType = $('#ApplicantTypeSelect').val();
    var Divisionst = $('#divisionverificationstatus').val();
    var Districtst = $('#districtverificationstatus').val();
    var Talukst = $('#talukverificationstatus').val();
    var Institutest = $('#instituteverificationstatus').val();

    if (year == 'choose' || year == null) {
        year = 0;
    }
    if (courseType == 'choose' || courseType == null) {
        courseType = 0;
    }
    if (appType == 'choose' || appType == null) {
            appType = 0;
        }
    if (Divisionst = "choose" || Divisionst == undefined || Divisionst == "" ) {
        Divisionst = 0;
    }
    if (Districtst == "choose" || Districtst == undefined ) {
        Districtst = 0;
    }
    if (Talukst == "choose" || Talukst == undefined ) {
        Talukst = 0;
    }
    if (Institutest == "choose" || Institutest == undefined) {
        Institutest = 0;
    }
    
    //if ($('#CourseType22').val() == "choose") {
    if ($('#CourseType22').val() == "101" || $('#CourseType22').val() == null) {
        $.ajax({
            type: "GET",
            url: "/Admission/GetApplicantsStatusFilter",
            contentType: "application/json",
            data: { year: year, courseType: courseType, applicanType: appType, division_id: Divisionst, district_lgd_code: Districtst, taluk_lgd_code: Talukst, InstituteId: Institutest },

            //data: { 'year': 0, 'courseType': 0, 'applicanType': 0 },
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

                        { 'data': 'MisCode', 'title': 'Mis Code', 'className': 'text-left' },
                        { 'data': 'Divisionname', 'title': 'Division Name', 'className': 'text-left' },
                        { 'data': 'Districtname', 'title': 'District Name', 'className': 'text-left' },
                        { 'data': 'Talukname', 'title': 'Taluk Name', 'className': 'text-left' },

                        { 'data': 'InstituteName', 'title': 'ITI Name', 'className': 'text-left' },
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
                            'title': 'Application Acknowledgement',
                            render: function (data, type, row) {
                                if (row.ApplDescStatus == 4) {
                                    return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                }
                                else { return "" }
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
     
       
        if (year == 'choose') {
            bootbox.alert("select the session");
        } else if (courseType == 'choose') {
            bootbox.alert("select the course type");
        } else
            if (appType == 'choose') {
                bootbox.alert("select the applicant type");
            }
            else {
                $.ajax({
                    type: "GET",
                    url: "/Admission/GetApplicantsStatusFilter",
                    contentType: "application/json",
                    //data: { 'year': year, 'courseType': courseType, 'applicanType': appType },
                    data: { year: year, courseType: courseType, applicanType: appType, division_id: Divisionst, district_lgd_code: Districtst, taluk_lgd_code: Talukst, InstituteId: Institutest },

                    success: function (data) {
                        debugger
                        $('#VerifyDocumentsTable').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                                { 'data': 'CourseType', 'title': 'Course Type', 'className': 'text-left' },
                                { 'data': 'Session', 'title': 'Session', 'className': 'text-left' },
                                { 'data': 'ApplicationMode', 'title': 'Application Mode', 'className': 'text-left' },

                                { 'data': 'MisCode', 'title': 'MisCode', 'className': 'text-left' },
                                { 'data': 'InstituteName', 'title': 'ITI Name', 'className': 'text-left' },
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
                                    'title': 'Application Acknowledgement',
                                    render: function (data, type, row) {
                                        alert(row.ApplDescStatus);
                                        if (row.ApplDescStatus == 4) {
                                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "&DocAdmFeeFlag=" + 1 + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                                        }
                                        else { return "" }
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

function GetApplicationDetailsById(ApplicationId, clickFrom) {
    $('#myModal').modal('show');
    $('input[type="text"], textarea').attr('readonly', false);
    //$("#UpdateDiv").show();
    //clearallErrorFields();

    if (clickFrom == "1") {
        //$('input[type="text"], textarea').attr('readonly', 'readonly');
        //$("#UpdateDiv").hide();
    }

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
                //OnChangeStateBoardType();
                //OnChangeGetAppliedBasicsForBoardType();

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
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function fnCloseModalWindow(id) {
    $("#" + id).modal('hide');
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

function ExcelExportTentative() {
    
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;
    $('#verificationTable').DataTable().destroy();
    $('#verificationTable').DataTable({ "paging": false }).draw(false);
    $("#verificationTable").table2excel({
        filename: "Verifcation Officer Mapping " + today + ".xls",
    });
    $('#verificationTable').DataTable().destroy();
    $('#verificationTable').DataTable({ "paging": true }).draw(false);
}

function ExcelExportDocVerification() {
    
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;
    $('#VerifyDocumentsTable').DataTable().destroy();
    $('#VerifyDocumentsTable').DataTable({ "paging": false }).draw(false);
    $("#VerifyDocumentsTable").table2excel({
        filename: "Document Verification Status " + today + ".xls",
    });
    $('#VerifyDocumentsTable').DataTable().destroy();
    $('#VerifyDocumentsTable').DataTable({ "paging": true }).draw(false);
}



