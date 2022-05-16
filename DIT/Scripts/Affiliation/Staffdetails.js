$(document).ready(function () {

    
    GetOfficers();
    GetStaffstatus();
    GetStaffType();
    GetAllDesignation();
    GetAllteachingSubject();
    GetAllTrades();
    GetCourseTypes();
    GetSeatAvailabilityStatus("status");
    GetSeatAvailabilityStatussubmit("statussub");
    StaffDetailsView();
    GetAllYear();
    GetAllGender();

    $('#PhoneNoEdit').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#PhoneNo').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    
    //$('#dd-Subject').multiselect();
});

$('a[href="#tab_1"]').click(function () {
    $("#tab1").show();
});

$('a[href="#tab_2"]').click(function () {
    $("#tab2").show();
});

$('a[href="#tab_3"]').click(function () {
    $("#tab3").show();
});

//================ Dropdown script ==============//

function GetStaffType() {
    
    $("#dd-stafftype").empty();
    $("#dd-stafftype").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllStaffType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#dd-stafftype").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllDesignation() {
    $("#dd-designation").empty();
    $("#dd-designation").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllDesignation",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#dd-designation").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllteachingSubject() {
    //$("#dd-Subject").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllTeachingSubject",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#dd-Subject").append($("<option/>").val(this.Value).text(this.Text));
            });
            $('#dd-Subject').multiselect({});

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllTrades() {
    //$("#dd-trade").append('<option value="">Select</option>');
    
    $.ajax({
        url: "/Affiliation/GetAllTrades",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#dd-trade").append($("<option/>").val(this.Value).text(this.Text));
            });
            $('#dd-trade').multiselect({});


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetCourseTypes() {
    //Add 
    $("#Course-Add").empty();
    $("#Course-Edit").empty();
    $("#Course-staff").empty();
    //$("#Course-Add").append('<option value="">Select</option>');

    //Update List
    $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="">Select</option>');
    $("#CourseTypeview").empty();
    $("#CourseTypeview").append('<option value="">Select</option>');
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
                    $("#Course-Edit").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#CourseTypeview").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#Course-staff").append($("<option/>").val(this.course_id).text(this.course_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function GetSeatAvailabilityStatus(user) {
    
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/SeatAvailabilityStatus",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.Status).text(this.StatusName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetSeatAvailabilityStatussubmit(user) {
    
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/SeatAvailabilityStatus",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.Status).text(this.StatusName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetAllYear() {
    //$("#Session12").append('<option value="">Select</option>');
    
    $("#year").empty();
    $("#year").append('<option value="">Select</option>');
    $("#Academicyear").empty();
    $("#Academicyear").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#Session12").append($("<option/>").val(this.Value).text(this.Text));
                $("#year").append($("<option/>").val(this.Value).text(this.Text));
                $("#Academicyear").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllGender() {
    $("#Gender").empty();
    $("#Genderid").empty();
    $("#Gender").append('<option value="">Select</option>');
    $("#Genderid").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllGender",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#Gender").append($("<option/>").val(this.Value).text(this.Text));
                $("#Genderid").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
//=============== End DropDown Script ===========//


//======== START Staff=========//
function GetOfficers() {
    var roleid = $('#uservalue').data('value');
    $('#showhidediv').hide();
 
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffDetailsSessionWise",
        contentType: "application/json",
        success: function (data) {
            
            $('#OfficerTable1').DataTable({
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
                    { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
                    { 'data': 'Quarter', 'title': 'Quarter', 'className': 'text-left' },
                    {
                        'title': 'Photo',
                        render: function (data, type, row) {
                            return "<img src='" + row.Photo + "' class='zoom'/>"
                        }
                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            
                            if ((oData.ApprovalFlowId == 0 || oData.ApprovalFlowId == null) &&
                                (oData.Appeovalstatus != 2 && oData.IsActive)) {
                                $(nTd).html("<input type='button' onclick='EditStaff(" + oData.StaffId +
                                    ")' class='btn btn-primary btn-xs' data-target='#EditOfficerModal' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" +
                                    oData.StaffId /*+ "," + oData.YearId*/ + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete' />");
                           }
                            else {
                                $(nTd).html("<input type='button' onclick='EditStaff(" + oData.StaffId +
                                    ")' class='btn btn-primary btn-xs' data-target='#EditOfficerModal' value='Edit' id='edit' disabled='disabled'/>&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" +
                                    oData.StaffId /*+",'" + oData.year*/ + "')' class='btn btn-danger btn-xs' value='Delete' id='Delete' disabled='disabled'/>");
                            }

                        }
                    }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                    if (/*aData.ApprovalFlowId == 9 ||*/ aData.ApprovalFlowId == 0 || aData.ApprovalFlowId == null) {
                        $('#showhidediv').show();
                    }
                    else {
                        $('#Addstaffbtn').attr("disabled", true);
                        $('#showhidediv').hide();
                    }
                    if (aData.Appeovalstatus == 2) {
                        $('#Addstaffbtn').attr("disabled", true);
                        $('#showhidediv').hide();
                    }
                   

                }
            });
        }, error: function (result) {
            alert("No staff details found.");
        }
    });
}

function CheckColors() {
    
    var type = $("#dd-stafftype").val();
    var textboxval = $("#others").val();

    if (type == 6 || $("#stafftypeEdit").val()==6) {
        $("#others").show();
        $("#edit_others").show();
    }
    else {
        $("#others").hide();
        $("#edit_others").hide();
    }

}
function readURL(input) {
    
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#previewimage').attr('src', e.target.result).width(50).height(50);
            $('#Edit_previewimage').attr('src', e.target.result).width(50).height(50);
            $('#Edit_previewimage_2').attr('src', e.target.result).width(50).height(50);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function Addstaff() {
    
    var courseid = $("#Course-Add").val();
    var yearid = $("#Session12").val();
    if (courseid == "") {
        $('#Course-Add').text('select the course type');
        bootbox.alert('select the course type');
    } else if (yearid == "") {
        $('#Session12').text('select the year');
        bootbox.alert('select the year');
    }
    var quarter = $('#Quarter_ID').val();
    var StaffName = $('#StaffName').val();
    var designation = $('#dd-designation').val();
    var Qualification = $('#Qualification').val();
    var stafftype = $('#dd-stafftype').val();
    var gender = $('#Gender').val();
    var trade = $('#dd-trade').val();
    var phoneNo = $('#PhoneNo').val();
    var emailId = $('#EmailId').val();
    var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
    var Experience = $('#dd-Experience').val();
    
    var cits = $("input[name='cits']:checked").val();
    var Other = $('#others').val();
    var stafftype = $('#dd-stafftype').val();
    var fileUpload = $("#PhotoUpload").get(0);
    var files = fileUpload.files;
    
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("ImageFile", files[i]);
    }
    //$('#dd-Subject :selected').each(function (i, selected) {
    //    fileData.append(
    //        "SubjectList", $(selected).val()
    //    );
    //});
    
    if (StaffName == '' && designation == '' && phoneNo == '' && emailId == '' && stafftype == ''   && Qualification == '') {
        $('#StaffNameErr').text('enter the officer name');
        $('#designationmsg').text('select the designation');
        $('#PhoneNoErr').text('enter the phone no');
        $('#EmailIdErr').text('enter the email');
        $('#QualificationErr').text('enter Qualification');
        $('#dd-stafftype_Required').text('Select staff type');
        $('#dd-Subject_Required').text('Select Subject');
        $('#dd-trade_Required').text('Select Trade');
        $('#Gender-Required').text('Select Gender');
        $('#dd-Experience').text('Enter Experience');

    }
    else
        if (StaffName == '') {
            $('#StaffNameErr').text('enter the officer name');
        } else if (designation == '') {
            $('#dd-designation').text('select the designation');
        } else if (phoneNo == '') {
            $('#PhoneNoErr').text('enter the phone no');
        } else if (phoneNo.length > 10 || phoneNo.length < 10) {
            $('#PhoneNoErr').text('enter the valid phone no');
        } else if (Qualification == '') {
            $('#QualificationErr').text('enter qualification');
        }
        else if (stafftype == '') {
            $('#dd-stafftype_Required').text('Select Staff Type');
        }
            
        else if (gender == '') {
            $('#Gender-Required').text('Select Gender');
        }
        else if ($('#dd-Subject').val() == '') {
            $('#dd-Subject_Required').text('Select Subject');
        }
        else if ($('#dd-trade').val() == '') {
            $('#dd-trade_Required').text('Select Trade');
        }
        else if (emailId == '') {
            $('#EmailIdErr').text('enter the email');
        } else
            if (!testEmail.test(emailId)) {
                $('#EmailIdErr').text('enter the valid email');
            } else {
               
               
                fileData.append(
                    "Name", StaffName
                );
                fileData.append(
                    "Designation", designation
                );
                fileData.append(
                    "Qualification", Qualification
                );
                fileData.append(
                    "StaffType", stafftype
                );
                fileData.append(
                    "MobileNum", phoneNo
                );
                fileData.append(
                    "EmailId", emailId
                );
                fileData.append(
                    "Trade", trade
                );
                fileData.append(
                    "Courseid", courseid
                );
                fileData.append(
                    "YearId", yearid
                );
                fileData.append(
                    "GenderId", gender
                );
                fileData.append(
                    "CITS", cits
                );
                fileData.append(
                    "Other", Other
                );
                fileData.append(
                    "Quarter", quarter
                );
                fileData.append(
                    "TotalExperience", Experience
                );
                $('#dd-Subject :selected').each(function (i, selected) {
                    fileData.append(
                        "MultiSelectSubjectList", $(selected).val()
                    );
                });

                $('#dd-trade :selected').each(function (i, selected) {
                    fileData.append(
                        "MultiSelectTradeList", $(selected).val()
                    );
                });
                
                //var Data = {
                //    Name: StaffName,
                //    Designation: designation,
                //    Qualification: Qualification,
                //    StaffType: stafftype,
                //    MobileNum: phoneNo,
                //    EmailId: emailId,
                //    TechingSubject: subject,
                //    Trade: trade
                //};
                
                bootbox.confirm('<br><br> Do you want to add Staff details?', (confirma) => {
                //var confirma = true;

                    if (confirma) {
                        for (var value of fileData.values()) {
                            console.log(value);
                        }
                        
                        $.ajax({
                            url: "/Affiliation/AddStaffDetails",
                            type: 'POST',
                            data: fileData,
                            contentType: false,
                            processData: false,
                            success: function (data) {
                                if (data == "success") {
                                    bootbox.alert("<br><br> Staff details added successfully");
                                    GetOfficers();
                                    $('#OfficerModal').modal('hide');
                                    //$('#OfficerModal').modal('toggle'); 
                                }
                                else if (data == "exist") {
                                    bootbox.alert("<br><br>Staff details already exist");
                                }
                                else {
                                    bootbox.alert("failed");
                                }
                            }, error: function (result) {
                                bootbox.alert("Error", "something went wrong");
                            }
                        });
                    }
                });

            }
    //$('#StaffName').text('');
    //$('#PhoneNoErr').text('');
    //$('#EmailIdErr').text('');
    //$('#Qualification').text('');
    //$('#dd-stafftype').text('');
    //$('#dd-Subject').text('');
    //$('#dd-trade').text('');
    //$('#dd-designation').text('');
    GetStaffType();
    GetAllDesignation();
    GetAllteachingSubject();
    GetAllTrades();
    //GetSeatAvailabilityStatus(user);
    GetCourseTypes();
}

function EditStaff(offid) {
    
    ClearOfficerFields("EditOfficerModal");
    $('#OfficerNameEdit').text('');
    $('#DesignationEdit').text('');
    $('#QualificationEdit').text('');
    $('#stafftypeEdit').text('');
    $('#SubjectEdit').empty();
    $('#PhoneNoEdit').text('');
    $('#EmailIdEdit').text('');
    $('#tradeEdit').empty();
    $('#Genderid').text('');
    $('#SubjectEdit').val('');
    $('#tradeEdit').val('');
    GetAllDesignation();
    //  GetStaffType();
    $.ajax({
        url: "/Affiliation/EditStaff",
        type: 'Get',
        data: { id: offid },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
           
            if (data != null) {
                if (data.Resultlist.DesignationList.length > 0) {
                    $.each(data.Resultlist.DesignationList, function (index, item) {
                      
                        $("#DesignationEdit").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                if (data.Resultlist.StafftypeList.length > 0) {
                    $.each(data.Resultlist.StafftypeList, function (index, item) {
                    
                        $("#stafftypeEdit").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                if (data.Resultlist.GenderList.length > 0) {
                    $.each(data.Resultlist.GenderList, function (index, item) {
                        
                        $("#Genderid").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }


                if (data.Resultlist.SubjectList.length > 0) {
                    $.each(data.Resultlist.SubjectList, function (index, item) {
                        $("#SubjectEdit").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

                var MultiselectSelectedValue = data.Resultlist.selectstaffsub;

                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                       
                        $("#SubjectEdit option[value='" + e + "']").prop("selected", true);
                    });
                }

                if (data.Resultlist.TradeList.length > 0) {
                    $.each(data.Resultlist.TradeList, function (index, item) {
                        $("#tradeEdit").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                var MultiselectTradedValue = data.Resultlist.selectstafftrade;

                if (MultiselectTradedValue != null) {
                    $.each(MultiselectTradedValue.split(","), function (i, e) {
                      
                        $("#tradeEdit option[value='" + e + "']").prop("selected", true);
                    });
                }



                $('#SubjectEdit').multiselect({});
                $('#tradeEdit').multiselect({});

                //if (data.Resultlist.SubjectList.length > 0) {
                //    $.each(data.Resultlist.selectstaffsub, function (index, item) {
                //        
                //        $("#SubjectEdit").append($("<option/>").val(this.Value).text(this.Text));
                //    });
                //}
                //
                //var MultiselectSelectedValue = data.Resultlist.selectstaffsub;

                //if (MultiselectSelectedValue != null) {
                //    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        

                //        $("#SubjectEdit option[value='" + e + "']").prop("selected", true);
                //    });
                //}

                //if (data.Resultlist.TradeList.length > 0) {
                //    $.each(data.Resultlist.TradeList, function (index, item) {
                //        
                //        $("#tradeEdit").append($("<option/>").val(this.Value).text(this.Text));
                //    });
                //}

                $('#OfficerIdEdit').val(data.Resultlist.StaffId);
                $('#OfficerNameEdit').val(data.Resultlist.Name);
                $('#DesignationEdit').val(data.Resultlist.Designation);
                $('#PhoneNoEdit').val(data.Resultlist.MobileNum);
                $('#EmailIdEdit').val(data.Resultlist.EmailId);
                $('#QualificationEdit').val(data.Resultlist.Qualification);
                $('#stafftypeEdit').val(data.Resultlist.StaffType);
                $('#edit-Experience').val(data.Resultlist.TotalExperience);
                //$('#Edit_previewimage').val(data.Resultlist.Photo);
                if (data.Resultlist.Photo != null && data.Resultlist.Photo != "")
                    $('#Edit_previewimage').attr("src", data.Resultlist.Photo);
                else
                    $('#Edit_previewimage').attr("src", "/Images/ProfilePic.png");
                
                $("input[name=cits][value=" + data.Resultlist.CITS + "]").prop('checked', true);
                //$(".cits:checked").val(data.Resultlist.CITS);
                //$('#edit-Experience').val(data.Resultlist.TotalExperience);
                
                $('#Genderid').val(data.Resultlist.GenderId);
                //$('#tradeEdit').val(data.Resultlist.Trade);

            } else
                alert('failed');

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function UpdateStaff() {
    
    $('#OfficerNameEditErr').text('');
    $('#DesignationEditErr').text('');
    $('#PhoneNoEditErr').text('');
    $('#EmailIdEditErr').text('');
    $('#ItiInstituteEditErr').text('');
    $('#OfficerLoginUserNameEditErr').text('');
    $('#OfficerLoginPwdEditErr').text('');
    var officerid = $('#OfficerIdEdit').val();
    var StaffName = $('#OfficerNameEdit').val();
    var designation = $('#DesignationEdit').val();
    var Qualification = $('#QualificationEdit').val();
    var stafftype = $('#stafftypeEdit').val();
    var phoneNo = $('#PhoneNoEdit').val();
    var emailId = $('#EmailIdEdit').val();
    var Trade = $('#tradeEdit').val();
    var subject = $('#SubjectEdit').val();
    var Gender = $('#Genderid').val();
    // New fields
    var Experience = $('#edit-Experience').val();
     var cits = $("input[name='cits']:checked").val();
    var Other = $('#edit_others').val();
   
    var fileUpload = $("#Edit_PhotoUpload").get(0);
    var files = fileUpload.files;

    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("ImageFile", files[i]);
    }
   // var fileData = new FormData();
    if ($('#Status').val() == 0) {
        var status = false;
    }
    else {
        var status = true;
    }
    

    if (StaffName == '') {
        $('#OfficerNameEditErr').text('enter the staff name');
    } else if (designation == '') {
        $('#dd-designation').text('select the designation');
    } else if (stafftype == '') {
        $('#dd-stafftype').text('select staff type');
    } else if (Qualification == '') {
        $('#QualificationEdit').text('enter staff qualification');
    }
    else if (Trade == '') {
        $('#dd-trade').text('Select trade');
    }
    else if (subject == '') {
        $('#dd-Subject').text('select subject');
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
            
            fileData.append(
                "StaffId", officerid
            );
            fileData.append(
                "Name", StaffName
            );
            fileData.append(
                "Designation", designation
            );
            fileData.append(
                "Qualification", Qualification
            );
            fileData.append(
                "StaffType", stafftype
            );
            fileData.append(
                "MobileNum", phoneNo
            );
            fileData.append(
                "EmailId", emailId
            );
            fileData.append(
                "GenderId", Gender
            );
            fileData.append(
                "CITS", cits
            );
            fileData.append(
                "Other", Other
            );
            fileData.append(
                "TotalExperience", Experience
            );

            $('#SubjectEdit :selected').each(function (i, selected) {
                fileData.append(
                    "MultiSelectSubjectList", $(selected).val()
                );
            });
            $('#tradeEdit :selected').each(function (i, selected) {
                fileData.append(
                    "MultiSelectTradeList", $(selected).val()
                );
            });

            //var Data = {
            //    StaffId: officerid,
            //    Name: StaffName,
            //    Designation: designation,
            //    Qualification: Qualification,
            //    StaffType: stafftype,
            //    MobileNum: phoneNo,
            //    EmailId: emailId,
            //    TechingSubject: subject,
            //    Trade: Trade
            //};
            bootbox.confirm('<br><br> Do you want to update Staff details?', (confirma) => {
                if (confirma) {
                    $.ajax({
                        url: "/Affiliation/Updatestaff",
                        type: 'POST',
                        data: fileData,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            if (data == true) {
                                bootbox.alert("<br><br> staff details updated successfully");
                                GetOfficers();
                                GetStaffstatus();
                                $('#EditOfficerModal').modal('hide');

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

function DeleteStaff(id) {
    bootbox.confirm(' <br><br> Do you want to delete staff details?', (confirma) => {
        if (confirma) {
            $.ajax({
                url: "/Affiliation/DeleteStaff",
                type: 'Get',
                data: { id: id },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == true) {
                        bootbox.alert('<br><br> Deleted successfully');
                        GetOfficers();
                        GetStaffstatus();
                        $('#EditStaffModal').modal('hide');
                        $('#EditStaffViewModal').modal('hide');
                    } else
                        bootbox.alert('failed');

                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
    });
}

function ClearOfficerFields(id) {
    
    //$(".clearfield").find("*").removeData();
    //$(".clearfield").find("input[type='text']").val('');
    ////$("#Course-Add").val($("#Course-Add option:first").val());
    //$(".clearfield").find("select").val($(".clearfield option:first").val());

    //$('input:checkbox').each(function () { this.checked = false; });
    //$(".clearfield option:selected").attr("checked", false);
    //$('.checkbox').prop('checked', true);
    //$(".clearfield option:selected").attr("checked", false);
    //$(".clearfield option").remove(); 
    //$(".clearfield input:checkbox").removeAttr("checked");
    //$('#usersDropDown').multipleSelect('refresh');
    //$(".clearfield").find("input[type=checkbox], input[type=radio]").prop("checked", "");
    //$(".clearfield").find("*").val('');
    //$('.clearfield')[0].trigger("reset");
    //$(".clearfield")[0].reset();
    $('#OfficerNameErr').text('');
    $('#DesignationErr').text('');
    $('#PhoneNoErr').text('');
    $('#EmailIdErr').text('');
    $('#ItiInstituteErr').text('');
    $('#OfficerName').val('');
    $('#Designation').val('');
    $('#OfficerName').val('');
    $('#Designation').val('');
    $('#PhoneNo').val('');
    $('#EmailId').val('');
    $('#ItiInstitute').val('');
    $('#OfficerLoginUserName').val('');
    $('#OfficerLoginPwd').val('');
    $('#OfficerLoginPwdErr').text('');
    $('#OfficerLoginUserNameErr').text('');
    $('#StaffName').val('');
    $('#dd-stafftype').val('');
    $('#dd-designation').val('');
    $('#Qualification').val('');
    $('#Gender').val('');
    $('#PhotoUpload').val('');
    $('#dd-Experience').val('');
    $('#previewimage').val('');
        
    $('#dd-Subject').val('').multiselect('refresh');
    $('#dd-trade').val('').multiselect('refresh');
    $('#SubjectEdit').attr('selected', 'selected');;
    $('input[name="cits"]').prop('checked', false);
    $('#PhotoUpload').val(''); 
    //$('#previewimage').val(''); 
    //$('img').remove('src');​
    $("#previewimage").attr("src"," /Images/ProfilePic.png");
    
    fnShowModalWindow(id);
}
function fnShowModalWindow(id) {
    $("#" + id).modal('show');
}

function fnCloseModalWindow(id) {
    $("#" + id).modal('hide');
}

function GetStaffOnsearch() {
    
    var courseid = $("#Course-Add").val();;
    var yearid = $("#Session12").val();
    $.ajax({
        type: "GET",
        url: "/Affiliation/StaffDeatilsonSearch",
        contentType: "application/json",

        data: { Year: yearid, courseId: courseid },
        success: function (data) {
            
            $('#OfficerTable1').DataTable({
                data: data.list,
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
                    { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
                    {

                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.ApprovalFlowId == 9 && oData.IsActive) {
                                $(nTd).html("<input type='button' onclick='EditStaff(" + oData.StaffId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditOfficerModal' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" + oData.StaffId
                                    + "," + oData.Year + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete' diasbled='disbled'/>");
                            }
                            else {
                                $(nTd).html("<input type='button' onclick='EditStaff(" + oData.StaffId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditOfficerModal' value='Edit' id='edit' disabled='disabled'/>&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" +
                                    oData.StaffId + "," + oData.Year + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete' disabled='disabled'/>");
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

function SubmitstaffDetails() {

    
    var courseid = $("#Course-Add").val();
    var yearid = $("#Session12").val();
    var Approvalstatusid = $('#status').val();
    var remarks = $('#remarks').val();
    //if (remarks == "") {
    //  return  bootbox.alert('Please Enter remarks');
    //}
    if (Approvalstatusid == "choose")
    {
        return bootbox.alert('Please select status dropdown');
    }
 
    //else {
    
        $('#OfficerTable1').DataTable().destroy();
        $('#OfficerTable1').DataTable({ "paging": false }).draw(false);
       
    if (remarks=="")
    {
        
           return bootbox.alert("Pls enter remarks")
       
            //remarks = $('#remark').val();
        }
       
                var listItem = [];
        var shift_table = $("#OfficerTable1 tbody");
       
                shift_table.find('tr').each(function (len) {
                    var $tr = $(this);
                   
                    var phoneNo = $tr.find("td:eq(8)").text();
                    var emailId = $tr.find("td:eq(9)").text();
                    var year = $tr.find("td:eq(1)").text();
                    var quarter = $tr.find("td:eq(10)").text();
                    var list = {                       
                        
                        MobileNum: phoneNo,
                        EmailId: emailId,
                        Remarks: remarks,
                        Year: year,
                      Quarter: quarter
                    }
                    listItem.push(list);

                });
             
    bootbox.confirm('<br><br> Do you want to Submit Staff details for review To case Worker?', (confirma) => {
                    if (confirma) {
                        $.ajax({
                            url: "/Affiliation/SaveStaffDetails",
                            type: "POST",
                            data: JSON.stringify(listItem),
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                
                                if (data == "success") {
                                    bootbox.alert("<br><br> Staff details Sent for review to case worker for review successfully");
                                    GetOfficers();
                                    GetStaffstatus();
                                    $("#Submit-btn").attr("disabled", true);
                                    $('#OfficerModal').modal('hide');
                                    $('#EditStaffViewModal').modal('hide');
                                    $('#EditStaffModal').modal('hide');
                                    $('#statussub').val('');
                                    $('#remark').val('');
                                    $("#savebtn").attr("disabled", true);
                                   
                                }
                                //else if (data == "exist") {
                                //    bootbox.alert("Staff details already exist");
                                //}
                                else {
                                    bootbox.alert("failed");
                                }
                            }, error: function (result) {
                                bootbox.alert("Error", "something went wrong");
                            }
                        });
                    }
                });

            //}
    $('#StaffName').text('');
    $('#PhoneNoErr').text('');
    $('#EmailIdErr').text('');
    $('#Qualification').text('');
    $('#dd-stafftype').text('');
    $('#dd-Subject').text('');
    $('#dd-trade').text('');
    $('#dd-designation').text('');
    GetStaffType();
    GetAllDesignation();
    GetAllteachingSubject();
    GetAllTrades();
}

//Replica method
function SubmitstaffDetails1() {

    
    var courseid = $("#Course-Add").val();
    var yearid = $("#Session12").val();
    var Approvalstatusid = $('#statussub').val();
    var remarks = $('#remark').val();
    //if (remarks == "") {
    //  return  bootbox.alert('Please Enter remarks');
    //}
    //else if (Approvalstatusid == "")
    //{
    //    bootbox.alert('Please select status dropdown');
    //}

    //else {
    
    $('#OfficerTable1').DataTable().destroy();
    $('#OfficerTable1').DataTable({ "paging": false }).draw(false);

    if (remarks == "") {
        return bootbox.alert("Pls enter remarks")
    }

    var listItem = [];
    var shift_table = $("#OfficerTable1 tbody");

    shift_table.find('tr').each(function (len) {
        var $tr = $(this);

        var phoneNo = $tr.find("td:eq(8)").text();
        var emailId = $tr.find("td:eq(9)").text();
        var year = $tr.find("td:eq(1)").text();
        var quarter = $tr.find("td:eq(10)").text();
        var list = {

            MobileNum: phoneNo,
            EmailId: emailId,
            Remarks: remarks,
            Year: year,
            Quarter: quarter
        }
        listItem.push(list);

    });

    bootbox.confirm('<br><br> Do you want to Submit Staff details for review To case Worker?', (confirma) => {
        if (confirma) {
            $.ajax({
                url: "/Affiliation/SaveStaffDetails",
                type: "POST",
                data: JSON.stringify(listItem),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    
                    if (data == "success") {
                        bootbox.alert("<br><br> Staff details Sent for review to case worker for review successfully");
                        GetOfficers();
                        GetStaffstatus();
                        $("#Submit-btn").attr("disabled", true);
                        $('#OfficerModal').modal('hide');
                        $('#EditStaffViewModal').modal('hide');
                        $('#EditStaffModal').modal('hide');
                        $('#statussub').val('');
                        $('#remark').val('');
                        $("#savebtn").attr("disabled", true);

                    }
                    //else if (data == "exist") {
                    //    bootbox.alert("Staff details already exist");
                    //}
                    else {
                        bootbox.alert("failed");
                    }
                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
    });

    //}
    $('#StaffName').text('');
    $('#PhoneNoErr').text('');
    $('#EmailIdErr').text('');
    $('#Qualification').text('');
    $('#dd-stafftype').text('');
    $('#dd-Subject').text('');
    $('#dd-trade').text('');
    $('#dd-designation').text('');
    GetStaffType();
    GetAllDesignation();
    GetAllteachingSubject();
    GetAllTrades();
}

//=======END Staff==============//


//============ Start Staff Status=============/
function GetStaffstatus(year, course, quarter) {
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffstatus",
        data: { year: year == "Select" ? "" : year, course: course, quarter: quarter,tabId:0},
        contentType: "application/json",
        success: function (data) {
            
            $('#StaffStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    {
                        'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html(++iRow);
                        }
                    },
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
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }
                        // &nbsp;& nbsp;< input type = 'button' onclick = 'EditStaffStatus(" + oData.StaffId + ")' class= 'btn btn-danger btn-xs' value = 'Delete' id = 'Delete' />
                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var ye = new Date().getFullYear();
                            
                            //if (oData.ApprovalFlowId == 9 && oData.IsActive) {
                            //if (parseInt(oData.Year) < ye ) {
                            //    
                            //    $(nTd).html("<input type='button' onclick='StaffviewMode(" + oData.InstituteId + "," +
                            //        oData.Quarter + ")' class='btn btn-primary btn-xs' " +
                            //        "data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;" +
                            //        "<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                            //        ")' class='btn btn-primary btn-xs' data-target='#EditStaffViewModal' " +
                            //        "value='Edit' id='edit'  disabled='disabled'/>");
                            //}
                           // else {
                               
                                $(nTd).html("<input type='button' onclick='StaffviewMode(" + oData.InstituteId + "," +
                                    oData.Quarter + ")' class='btn btn-primary btn-xs' " +
                                    "data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;" +
                                    "<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                    ")' class='btn btn-primary btn-xs' data-target='#EditStaffViewModal' " +
                                    "value='Edit' id='edit'" + (oData.IsAction ? "" : " disabled='disabled'") + " />");
                           // }

                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("No staff details found");
        }
    });
}

function ViewStaffStatus(offid,year,quarter) {
    
    $('#StaffNameErr').text('');
    $('#dd-designation').text('');
    $('#PhoneNoErr').text('');
    $('#EmailIdErr').text('');
    $('#QualificationErr').text('');
    $('#dd-stafftype_Required').text('');
    $('#dd-Subject_Required').text('');
    $('#dd-trade_Required').text('');
    fnShowModalWindow('ViewStaffModal');
    $.ajax({
        url: "/Affiliation/Viewstaffhistory",
        type: 'Get',
        data: { id: offid, session:year,quarter:quarter },
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

function StaffviewMode(insId,year) {
    
    fnShowModalWindow('StaffViewModal');
   // let testdata = "testdata,TESTRDARA"; 
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffDetailsSessionWise",
        data: {session:year},
        contentType: "application/json",
        success: function (data) {
            
            $('#tblStaffViewMode').DataTable({
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
                    { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
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

function StaffEditviewMode(insID, year) {
    //$("#savebtn").attr('disabled', true);
    fnShowModalWindow('EditStaffViewModal');
    $("#remarksDiv").hide();
    
    $.ajax({
        type: "GET",
        data: { session: year },
        url: "/Affiliation/GetStaffDetailsSessionWise",
        contentType: "application/json",
        success: function (data) {
            //if (data.result != null && data.result.length > 0) {
            //    // $("#savebtn").removeAttr('disabled');
            //    $("#remarksDiv").show();
            //}
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
                    { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
                    { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
                    {

                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            
                            if (oData.ApprovalFlowId == 9 && oData.IsActive) {
                                $(nTd).html("<input type='button' onclick='EditStaffStatus(" + oData.StaffId + ")' class='btn btn-primary btn-xs'  data-target='#EditStaffModal' value='Edit' id='edit' />&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" +
                                    oData.StaffId /*+ "," + oData.Year*/ + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete' diasbled='disbled'/>");
                            }
                            else {
                                $(nTd).html("<input type='button' onclick='EditStaffStatus(" + oData.StaffId + ")' class='btn btn-primary btn-xs'  data-target='#EditStaffModal' value='Edit' id='edit' disabled='disabled' />&nbsp;&nbsp;<input type='button' onclick='DeleteStaff(" +
                                    oData.StaffId /*+ "," + oData.Year*/ + ")' class='btn btn-danger btn-xs' value='Delete' id='Delete' disabled='disabled'/>");
                            }

                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (aData.IsActive == false) {
                        $('td', nRow).css('background-color', '#5691f0');
                    } 
                    if (aData.ApprovalFlowId == 9) {
                        $("#remarksDiv").show();
                    }
                }
           
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function EditStaffStatus(offid) {
    
    $('#StaffNameErr').text('');
    $('#DesignationStatus').text('');
    $('#PhoneNostatus').text('');
    $('#EmailIdstatus').text('');
    $('#Qualificationstatus').text('');
    $('#stafftypestatus').text('');
    $('#Subjectstatus').text('');
    $('#tradestatus').text('');
    $('#Subjectstatus').val('');
    $('#tradestatus').val('');
    fnShowModalWindow('EditStaffModal');
    //$('#Subjectstatus').val('').multiselect('clearSelection');
    //$('#tradestatus').val('').multiselect('clearSelection');
    $.ajax({
        url: "/Affiliation/EditStaff",
        type: 'Get',
        data: { id: offid },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
            if (data != null) {
                if (data.Resultlist.DesignationList.length > 0) {
                    $.each(data.Resultlist.DesignationList, function (index, item) {
                        
                        $("#DesignationStatus").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                if (data.Resultlist.StafftypeList.length > 0) {
                    $.each(data.Resultlist.StafftypeList, function (index, item) {
                        
                        $("#stafftypestatus").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
                
                if (data.Resultlist.SubjectList.length > 0) {
                    $.each(data.Resultlist.SubjectList, function (index, item) {                        
                        $("#Subjectstatus").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

                var MultiselectSelectedValue = data.Resultlist.selectstaffsub;

                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        
                        $("#Subjectstatus option[value='" + e + "']").prop("selected", true);
                    });
                }

                if (data.Resultlist.TradeList.length > 0) {
                    $.each(data.Resultlist.TradeList, function (index, item) {
                        $("#tradestatus").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

                var MultiselectTradedValue = data.Resultlist.selectstafftrade;

                if (MultiselectTradedValue != null) {
                    $.each(MultiselectTradedValue.split(","), function (i, e) {
                        
                        $("#tradestatus option[value='" + e + "']").prop("selected", true);
                    });
                }

               

                $('#Subjectstatus').multiselect({});
                $('#tradestatus').multiselect({});

                $('#OfficerIdEdit').val(data.Resultlist.StaffId);
                $('#OfficerNamestatus').val(data.Resultlist.Name);
                $('#DesignationStatus').val(data.Resultlist.Designation);
                $('#PhoneNostatus').val(data.Resultlist.MobileNum);
                $('#EmailIdstatus').val(data.Resultlist.EmailId);
                $('#Qualificationstatus').val(data.Resultlist.Qualification);
                $('#stafftypestatus').val(data.Resultlist.StaffType);
                $('#Course-staff').val(data.Resultlist.Courseid);
                //$('#tradestatus').val(data.Resultlist.Trade);
                $('#edit-Experience_2').val(data.Resultlist.TotalExperience);
                //$('#Edit_previewimage').val(data.Resultlist.Photo);
                if (data.Resultlist.Photo != null && data.Resultlist.Photo != "")
                    $('#Edit_previewimage_2').attr("src", data.Resultlist.Photo);
                else
                    $('#Edit_previewimage').attr("src", "/Images/ProfilePic.png");
                
                $("input[name=cits][value=" + data.Resultlist.CITS + "]").prop('checked', true);


            } else
                alert('failed');

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function UpdateStaffstatus() {
    
    $('#OfficerNameEditErr').text('');
    $('#DesignationEditErr').text('');
    $('#PhoneNoEditErr').text('');
    $('#EmailIdEditErr').text('');
    $('#ItiInstituteEditErr').text('');
    $('#OfficerLoginUserNameEditErr').text('');
    $('#OfficerLoginPwdEditErr').text('');
    var officerid = $('#OfficerIdEdit').val();
    var StaffName = $('#OfficerNamestatus').val();
    var designation = $('#DesignationStatus').val();
    var Qualification = $('#Qualificationstatus').val();
    var stafftype = $('#stafftypestatus').val();
    var phoneNo = $('#PhoneNostatus').val();
    var emailId = $('#EmailIdstatus').val();
    var Trade = $('#tradestatus').val();
    var subject = $('#Subjectstatus').val();
    var course_id = $('#Course-staff').val();
    var Experience = $('#edit-Experience_2').val();
    var cits = $("input[name='cits']:checked").val();
    var Other = $('#edit_others').val();
    var fileUpload = $("#Edit_PhotoUpload_2").get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("ImageFile", files[i]);
    }
  
    if ($('#Status').val() == 0) {
        var status = false;
    }
    else {
        var status = true;
    }
    

    if (StaffName == '') {
        $('#OfficerNameEditErr').text('enter the staff name');
    } else if (designation == '') {
        $('#dd-designation').text('select the designation');
    } else if (stafftype == '') {
        $('#dd-stafftype').text('select staff type');
    } else if (Qualification == '') {
        $('#QualificationEdit').text('enter staff qualification');
    }
    else if (Trade == '') {
        $('#dd-trade').text('Select trade');
    }
    //else if (subject == '') {
    //    $('#dd-Subject').text('select subject');
    //}
    else if (phoneNo == '') {
        $('#PhoneNoEditErr').text('enter the phone no');
    } else if (phoneNo.length > 10 || phoneNo.length < 10) {
        $('#PhoneNoEditErr').text('enter the valid phone no');
    }
    else
        if (emailId == '') {
            $('#EmailIdEditErr').text('enter the email');
        }
        else {
            
            fileData.append(
                "StaffId", officerid
            );
            fileData.append(
                "Name", StaffName
            );
            fileData.append(
                "Designation", designation
            );
            fileData.append(
                "Qualification", Qualification
            );
            fileData.append(
                "StaffType", stafftype
            );
            fileData.append(
                "MobileNum", phoneNo
            );
            fileData.append(
                "EmailId", emailId
            );
            fileData.append(
                "Trade", Trade
            );
            fileData.append(
                "TotalExperience", Experience
            );

            fileData.append(
                "CITS", cits
            );
            fileData.append(
                "Other", Other
            );
            fileData.append(
                "Courseid", course_id
            );

            $('#Subjectstatus :selected').each(function (i, selected) {
                fileData.append(
                    "MultiSelectSubjectList", $(selected).val()
                );
            });
            $('#tradestatus :selected').each(function (i, selected) {
                fileData.append(
                    "MultiSelectTradeList", $(selected).val()
                );
            });
            //var Data = {
            //    StaffId: officerid,
            //    Name: StaffName,
            //    Designation: designation,
            //    Qualification: Qualification,
            //    StaffType: stafftype,
            //    MobileNum: phoneNo,
            //    EmailId: emailId,
            //    TechingSubject: subject,
            //    Trade: Trade
            //};
            bootbox.confirm('<br><br> Do you want to update officer details?', (confirma) => {
                if (confirma) {
                    for (var value of fileData.values()) {
                        console.log(value);
                    }
                    
                    $.ajax({
                        url: "/Affiliation/Updatestaff",
                        type: 'POST',
                        data: fileData,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            if (data == true) {
                                bootbox.alert("<br><br> staff details updated successfully");
                                GetOfficers();
                                GetStaffstatus();
                                $('#EditStaffModal').modal('hide');
                                $('#EditStaffViewModal').modal('hide');
                                //GetInActiveOfficernMapping();
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

}

function GetStaffstatusOnSearch() {
    
    var courseid = $("#CourseTypes").val();;
    var yearid = $("#year option:selected").text();
    var quarter = $("#Quarter_sear").val();
    GetStaffstatus(yearid, courseid, quarter);
    //$.ajax({
    //    type: "GET",
    //    url: "/Affiliation/StaffDeatilsonSearch",
    //    contentType: "application/json",
    //    data: { Year: yearid, courseId: courseid },
    //    success: function (data) {
    //        $('#StaffStatusTable').DataTable({
    //            data: data.list,
    //            "destroy": true,
    //            "bSort": true,
    //            columns: [
    //                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
    //                { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
    //                { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-center' },
    //                { 'data': 'MIScode', 'title': 'ITI Institute MIS Code', 'className': 'text-left' },
    //                { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
    //                { 'data': 'Name', 'title': 'Staff Name', 'className': 'text-left' },
    //                { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
    //                { 'data': 'District', 'title': 'District', 'className': 'text-left' },
    //                { 'data': 'StatusName', 'title': 'Status- Currently with', 'className': 'text-left' },
    //                {
    //                    'data': 'StaffId',
    //                    'title': 'View',
    //                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
    //                        $(nTd).html("<input type='button' onclick='ViewStaffStatus(" + oData.StaffId +
    //                            "," + oData.Year + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#ViewStaffModal' value='View' id='view'/>");
    //                    }
    //                },
    //                {
    //                    'data': 'StaffId',
    //                    'title': 'View/Edit',
    //                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
    //                        if (oData.ApprovalFlowId == 9 && oData.IsActive) {
    //                            $(nTd).html("<input type='button' onclick='EditStaffStatus(" + oData.StaffId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffModal' value='Edit' id='edit'/>");
    //                        }
    //                        else {
    //                            $(nTd).html("<input type='button' onclick='EditStaffStatus(" + oData.StaffId + ")' class='btn btn-primary btn-xs' data-toggle='modal' data-target='#EditStaffModal' value='Edit' id='edit'/ disabled='disabled'>");
    //                        }

    //                    }
    //                }
    //            ]
    //        });
    //    }, error: function (result) {
    //        alert("Error", "something went wrong");
    //    }
    //});
}

function StaffDetailsView(year, coursetype, quarter) {
    
    $.ajax({
        type: "GET",
        url: "/Affiliation/GetStaffstatus",
        data: { session: year == "Select" ? "" : year,coursetype, quarter, tabId:3 },
        contentType: "application/json",
        success: function (data) {
            
            $('#tblViewStaffDetails').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    {
                        'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center'
                        ,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html(++iRow);
                        }
                    },
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
                                oData.Quarter + ")' class='btn btn-primary btn-xs' data-target='#ViewStaffModal' value='View' id='view'/>");
                        }
                        // &nbsp;& nbsp;< input type = 'button' onclick = 'EditStaffStatus(" + oData.StaffId + ")' class= 'btn btn-danger btn-xs' value = 'Delete' id = 'Delete' />
                    },
                    {
                        'data': 'StaffId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var ye = new Date().getFullYear();
                            
                            //if (oData.ApprovalFlowId == 9 && oData.IsActive) {
                            if (parseInt(oData.Year) < ye) {
                                
                                $(nTd).html("<input type='button' onclick='StaffviewMode(" + oData.InstituteId + "," +
                                    oData.Quarter + ")' class='btn btn-primary btn-xs' " +
                                    "data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;" +
                                    "<input type='button' onclick='StaffEditviewMode(" + oData.InstituteId +
                                    ")' class='btn btn-primary btn-xs' data-target='#EditStaffViewModal' " +
                                    "value='Edit' id='edit'  disabled='disabled'/>");
                            }
                            else {

                                $(nTd).html("<input type='button' onclick='StaffviewMode(" + oData.InstituteId + "," +
                                    oData.Quarter + ")' class='btn btn-primary btn-xs' " +
                                    "data-target='#StaffViewModal' value='View' id='view'/>&nbsp;&nbsp;");
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

function GetApprovedstaffOnSearch() {
    
    //var courseid = $("#CourseTypeview").val();;
    var yearid = $("#Academicyear option:selected").text();
    var coursetype = $("#CourseTypeview").val();
    var quarter = $("#Quarter_view").val();
    StaffDetailsView(yearid, coursetype, quarter);
    // let testdata = "testdata,TESTRDARA";
    //$.ajax({
    //    type: "GET",
    //    url: "/Affiliation/StaffDeatilsonSearch",
    //    contentType: "application/json",
    //    data: { Year: yearid, courseId: courseid },
    //    success: function (data) {
    //        
    //        $('#tblViewStaffDetails').DataTable({
    //            data: data.Approvedlist,
    //            "destroy": true,
    //            "bSort": true,
    //            columns: [
    //                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
    //                { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
    //                { 'data': 'Name', 'title': 'Staff Name', 'className': 'text-center' },
    //                { 'data': 'DesignationName', 'title': 'Designation', 'className': 'text-left' },
    //                { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left' },
    //                { 'data': 'Type', 'title': 'Staff Type', 'className': 'text-left' },
    //                { 'data': 'subject', 'title': 'Teaching Subject', 'className': 'text-left' },
    //                { 'data': 'Tradename', 'title': 'Trade', 'className': 'text-left' },
    //                { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
    //                { 'data': 'EmailId', 'title': 'EmailId', 'className': 'text-left' },
    //            ]
    //        });
    //    }, error: function (result) {
    //        alert("Error", "something went wrong");
    //    }
    //});
}
//============ End Staff Status






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

//=====End validations======

function Validate(e, id) {
    var keyCode = e.keyCode || e.which;

    var lblError = document.getElementById(id + "Err");
    lblError.innerHTML = "";

    //Regex for Valid Characters i.e. Alphabets.
    var regex = /^[A-Za-z ]+$/;

    //Validate TextBox value against the Regex.
    var isValid = regex.test(String.fromCharCode(keyCode));
    if (!isValid) {
        lblError.innerHTML = "Only Alphabets allowed.";
    }

    return isValid;
}

$('#PhotoUpload').change(function () {
    
    var ext = $(this).val().split('.').pop().toLowerCase();
    if (ext != "") {
        if ($.inArray(ext, ['JPEG', 'jpeg', 'png', 'PNG', 'JPG', 'jpg']) == -1) {
            bootbox.alert("<br><br>Kindly upload valid PNG (or) JPEG file");
            $(this).val("");
        }
    } else {
        $(this).val("");
    }
});

function cancelselectfields() {
    $('#CourseTypes').val('');
    $('#year').val('');
    $('#Districts').val('');
    $('#Trades').val('');
    $('#CourseTypeview').val('');
    $('#Academicyear').val('');


}
