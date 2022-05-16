$(document).ready(function () {
    $("#ShowHideGenerateRankList").hide();
    $('#viewGradlist').click(function () {
        $("#GradationTypeErr").hide();
        $("#ApplicantTypeErr").hide();
        $("#AcademicyearErr").hide();
    });
    $('#Gradliststatus').click(function () {
        $("#ApplicantTypeviewErr").hide();
        $("#ddlRoundview-Required").hide();
        $("#GradationTypeviewErr").hide();
        $("#AcademicyearviewErr").hide();
        $("#GradationTypeErr").hide();
        $("#ApplicantTypeErr").hide();
        $("#AcademicyearErr").hide();
        $("#ShowHideGenerateRankList").hide();
    });
    $('#GradApplist').click(function () {
        $("#ApplicantTypeviewErr").hide();
        $("#ddlRoundview-Required").hide();
        $("#GradationTypeviewErr").hide();
        $("#AcademicyearviewErr").hide();
        $("#ShowHideGenerateRankList").hide();
    });
    $('.nav-tabs li:eq(0) a').tab('show');
    $("#sendBtn3").attr("disabled", true);
    GrdationList();
    $("#disSentTo").hide();
    $("#GradationTypeErr").hide();
    $("#ApplicantTypeErr").hide();
    $("#AcademicyearErr").hide();
    $("#ddlApplicantTypeReviewErr").hide();
    $("#ddlAcademicYearReviewErr").hide();
    $("#ddlGradationTypeReviewErr").hide();
    $("#remarksErr").hide();
    $("#ApplicantTypeDirErr").hide();
    $("#GradationTypeDirErr").hide();
    $("#divRemarks").hide();
    $("#divpublish").hide();
    $("#tbldivview").hide();
    $("#ddlAcademicYearDirErr").hide();
    $("#ddlAcademicYearDirNewErr").hide();
    $("#GradationTypeDirNewErr").hide();
    $("#ApplicantTypeviewErr").hide();
    $("#GradationTypeviewErr").hide();
    $("#divisionviewErr").hide();
    $("#districtviewErr").hide();
    $("#AcademicyearviewErr").hide();
    $("#btnForward").hide();
    $('#divSendToDC').hide();
    $("#ApplicantTypeDirNewErr").hide();
    $("#ddlAcademicYearADNewErr").hide();
    $("#ApplicantTypeADNewErr").hide();
    $("#ApplicantTypeADErr").hide();
    $("#GradationTypeADNewErr").hide();
    $("#ddlAcademicYearADErr").hide();
    $("#GradationTypeADErr").hide();
    $("#tblDivADReviewNewId").hide();

    $("#divSearchboxDD").hide();
    $("#divRemarksDD").hide();
    $("#divSearchboxView").hide();

    $("#usersDirToCom").attr("disabled", true);
    $("#sendBackADBtn").attr("disabled", true);
    $("#btnSendDir").attr("disabled", true);
    $("#btnSendCom").attr("disabled", true);
    $("#backusersDirector").attr("disabled", true);
    $("#sendBtnchangesDirtoDD").attr("disabled", true);
    $('#divRemarksDirCom').hide();
    if ($("#hdnSession").data('value') == loginUserRole.JD || $("#hdnSession").data('value') == loginUserRole.ADL) {
        GetAcademicYearADNew(0);
    }
    if ($("#hdnSession").data('value') == loginUserRole.Director || $("#hdnSession").data('value') == loginUserRole.Commissioner) {
        GetAcademicYearDDc(0);
    }


    //Getusers();
    GetAcademicYear();
    //GetDistrict();
    GetApplicantTypeDDL();
    GetGradationType();
    GetDivision();
    GetStatusDrop();
    GrdationListStatus();

    //var radio = document.getElementById("#tentative").checked;
    radiobtn = document.getElementById("tentative");
    radiobtn.checked = true;

    Getusers("users", "1");
    Getusers("ADReusers", "1");
    Getusers("backusersAd", "3");
    Getusers("ReusersDir", "1");
    Getusers("backusersDir", "1");
    Getusers("usersDirToCom", "1");
    Getusers("backusersDirector", "3");

    $('#backusersAd').click(function () {
        $("#backusersAd option[value ='" + loginUserRole.CW + "']").hide();
    });
    $('#backusersDirector').click(function () {
        $("#backusersDirector option[value ='" + loginUserRole.CW + "']").hide();
    });

    // $('#sendBtnchanges').hide();
});

//function Getusers() {
//    $("#users").empty();
//    $("#users").append('<option value="choose">choose</option>');
//    $("#usersview").empty();
//    $("#usersview").append('<option value="choose">choose</option>');
//    $("#usersAdToDC").empty();
//    $("#usersAdToDC").append('<option value="choose">choose</option>');
//    $("#usersPublish").empty();
//    $("#usersPublish").append('<option value="choose">choose</option>');
//    $("#userCD").empty();
//    $("#userCD").append('<option value="choose">choose</option>');
//    $("#ADReusers").empty();
//    $("#ADReusers").append('<option value="choose">choose</option>');
//    $("#Reusers").empty();
//    $("#Reusers").append('<option value="choose">choose</option>');
//    $.ajax({
//        //url: "/Admission/GetRoles",
//        url: "/Admission/GetRolesForTentativeList",
//        type: 'Get',
//        contentType: 'application/json; charset=utf-8',
//        success: function (data) {
//            if (data != null || data != '') {
//                $.each(data, function () {
//                    $("#users").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#usersview").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#usersAdToDC").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#usersPublish").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#userCD").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#ADReusers").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#Reusers").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                });
//            }

//        }, error: function (result) {
//            alert("Error", "something went wrong");
//        }
//    });
//}

function Getusers(user, level) {
    
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        //url: "/Admission/GetRolesCal",
        //url: "/Admission/GetRolesForTentativeList",
        url:"/Admission/GetMeritRoles",
        type: 'Get',
        data: { 'level': level },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
              
                    console.log(data);
                    if (user == "backusersAd" && this.RoleID == 5)
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    else if (user == "backusersDir" && this.RoleID == 5)
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    else if (user == "users" && (this.RoleID == 1) || (this.RoleID == 2) || (this.RoleID == 6)||(this.RoleID == 4))
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    else if (user == "ADReusers" && (this.RoleID == 1 || this.RoleID == 2))
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    else if (user == "usersDirToCom" && (this.RoleID == 1))
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    else if (user == "backusersDirector")
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));

                    //else if (user == "ReusersDir" && (this.RoleID == 1 || this.RoleID == 2))
                    //    $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function funStatusAD() {
  
    //var ADRevStatus = $("#ADRevStatus :selected").text();
    //$("#ADReviewremarks").val(ADRevStatus);
 
    if (($("#ADRevStatus :selected").val()) == 4 ) {
        if (($("#ADRevStatus :selected").val()) == 4) {
            $("#ADReusers").attr("disabled", true);
            $("#btnSendDir").attr("disabled", true);

            $("#backusersAd").attr("disabled", false);
            $("#sendBackADBtn").attr("disabled", false);
           // var ADRevStatus = $("#ADRevStatus :selected").text();
            //$("#ADReviewremarks").val(ADRevStatus);
        }
        else {
            $("#backusersAd").attr("disabled", true);
            $("#sendBackADBtn").attr("disabled", true);
            $("#ADReusers").attr("disabled", false);
            $("#btnSendDir").attr("disabled", false);
           // var ADRevStatus = $("#ADRevStatus :selected").text();
            //$("#ADReviewremarks").val(ADRevStatus);
        }     
    }
    else if (($("#ADRevStatus :selected").val()) == 7) {
        if (($("#ADRevStatus :selected").val()) == 4 ) {
            $("#ADReusers").attr("disabled", true);
            $("#btnSendDir").attr("disabled", true);

            $("#backusersAd").attr("disabled", false);
            $("#sendBackADBtn").attr("disabled", false);
        }
        else if (($("#ADRevStatus :selected").val()) == 8) {
            $("#backusersAd").attr("disabled", true);
            $("#sendBackADBtn").attr("disabled", true);
            $("#ADReusers").attr("disabled", false);
            $("#btnSendDir").attr("disabled", false);
        /*$("#ADReusers :selected").val() = 8;*/
            
        }     
        else {
            $("#backusersAd").attr("disabled", true);
            $("#sendBackADBtn").attr("disabled", true);
            $("#ADReusers").attr("disabled", false);
            $("#btnSendDir").attr("disabled", false);
        }     
    }
   
}

function GetStatusDrop() {
    $("#ADRevStatus").empty();
    $("#ADRevStatus").append('<option value="choose">choose</option>');
    $("#RevStatusDir").empty();
    $("#RevStatusDir").append('<option value="choose">choose</option>');

    $.ajax({
        url: "/Admission/GetMeritStatusList",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ADRevStatus").append($("<option/>").val(this.Status).text(this.StatusName));
                    $("#RevStatusDir").append($("<option/>").val(this.Status).text(this.StatusName));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetGradationMeritListDetails(yearId, ApplTypeId, roundId) {
    var generateId = $('#ddlGradationType :selected').val();
    if (generateId == 0)
        generateId = 2; // final merit list
    var AcademicYear = $('#ddlAcademicYearDD :selected').val();
    if (yearId != 0)
        AcademicYear = yearId;
    var ApplicantTypeId = $('#ddlApplicantType :selected').val();
    if (ApplTypeId != 0)
        ApplicantTypeId = ApplTypeId;
    var round = $('#ddlRound :selected').val();
    if (roundId != 0)
        round = roundId;

    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationMeritList",
        data: { generateId: generateId, ApplicantTypeId: ApplicantTypeId, AcademicYear: AcademicYear, round: round},
        contentType: "application/json",
        success: function (data) {
            if (data != "" || data != 0 || data.length != 0) {
                var t = $('#tblGradationMeritList').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download Excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]
                            }
                        }
                    ],
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                        { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                        { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                        //{ 'data': 'dateofbirth', 'title': 'DOB', 'className': 'text-left DOB' },
                        {
                            'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                //var date = daterangeformate2(oData.dateofbirth);
                                var date = oData.dateofbirth;
                                var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                                $(nTd).html(date + hide);
                            }
                        },
                        { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                        { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                        { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                        { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                        { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                        { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                        { 'data': 'MaxMarks', 'title': 'Max Marks in 10th', 'className': 'text-left MaxMarks' },
                        { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                        { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                        { 'data': 'Weitage', 'title': 'Weightage', 'className': 'text-left Weitage' },
                        { 'data': 'Total', 'title': 'Total Marks ', 'className': 'text-left Total' },
                        { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                        { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                        { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                        { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                    ],
                });
                //$('#btnSearchMeritDD').click(function () {
                //    t.search($("#txtSearchMeritDD").val()).draw();
                //});
                //$("#ApplicantName").keypress(function (e) {
                //    if (e.which === 13) {
                //        e.preventDefault();
                //        t.search($(this).val()).draw();
                //    }
                //});
                //$("#ApplicantNumber").keypress(function (e) {
                //    if (e.which === 13) {
                //        e.preventDefault();
                //        t.search($(this).val()).draw();
                //    }
                //});
                $("#sendBtn1").attr("disabled", false);
                $("#sendBtn3").attr("disabled", false);
                $("#users").attr("disabled", false);
            }
            else {
                $("#sendBtn1").attr("disabled", true);
                $("#sendBtn3").attr("disabled", true);
                $("#users").attr("disabled", true);
                GetceckGradationTransTable();
                //console.log(tab);
                //if ($('#ddlGradationType :selected').val() == 2) {
                //    $('#disSentTo').hide();
                //    $('#divRemarksDD').hide();
                //    $('#btnForward').hide();
                //    $('#tbldivMerit').hide();
                //    $('#txtSearchMeritDD').hide();
                //    $('#btnSearchMeritDD').hide();
                //    var _msg = "Final merit list  <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b>data is not available.";
                //    bootbox.alert(_msg);
                //} else {
                //    $('#txtSearchMeritDD').hide();
                //    $('#btnSearchMeritDD').hide();
                //    $('#tbldivMerit').hide();
                //    $('#btnapprove').hide();
                //    $('#tbldivMerit').hide();
                //    var _msg = "Tentative list  <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b>data is not available.";
                //    bootbox.alert(_msg);
                //}
            }
        }
    });
}

function GetceckGradationTransTable() {
    var ApplicantTypeText = $('#ddlApplicantType :selected').text();
    var AcademicYearText = $('#ddlAcademicYearDD :selected').text();
    var ApplicantTypeId = $('#ddlApplicantType :selected').val();
    var generateId = $('#ddlGradationType :selected').val();
    var AcademicYear = $('#ddlAcademicYearDD :selected').val();
    //var status = 5;

    $.ajax({
        type: "GET",
        url: "/Admission/GetceckGradationTransTable",        
        data: { generateId: generateId, ApplicantTypeId: ApplicantTypeId, AcademicYear: AcademicYear },
        contentType: "application/json",
        success: function (data) {
            if (data.length!=0) {
                if (generateId == 1 ) {
                    var _msg = "<br> Tentative list for Notification: <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b>data already generated.";
                    bootbox.alert(_msg);
                    $('#txtSearchMeritDD').hide();
                    $('#btnSearchMeritDD').hide();
                    $('#tbldivMerit').hide();
                    $('#btnapprove').hide();
                    $('#tbldivMerit').hide();
                }
                else if (generateId == 2 ) {
                    var _msg = "<br> Final merit list for Notification: <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b> data already sent";
                    bootbox.alert(_msg);
                    $('#disSentTo').hide();
                    $('#divRemarksDD').hide();
                    $('#btnForward').hide();
                    $('#tbldivMerit').hide();
                    $('#txtSearchMeritDD').hide();
                    $('#btnSearchMeritDD').hide();
                   
                }
            } else {
                if ($('#ddlGradationType :selected').val() == 2) {
                    var _msg = "<br> Final merit list for Notification: <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b>data is not available without Tentative Merit List. Please generate Tentative Merit List to proceed.";
                    bootbox.alert(_msg);
                    $('#ShowHideGenerateRankList').hide();
                    $('#disSentTo').hide();
                    $('#divRemarksDD').hide();
                    $('#btnForward').hide();
                    $('#tbldivMerit').hide();
                    $('#txtSearchMeritDD').hide();
                    $('#btnSearchMeritDD').hide();
                    
                }
                else {
                    var _msg = "<br> Tentative list  <b>" + ApplicantTypeText + " </b> for Session:  <b>" + AcademicYearText + " </b>data is not available.";
                    bootbox.alert(_msg);
                    $('#txtSearchMeritDD').hide();
                    $('#btnSearchMeritDD').hide();
                    $('#tbldivMerit').hide();
                    $('#btnapprove').hide();
                    $('#tbldivMerit').hide();
                }
            }
        }, error: function (result) {
           
        }       
    })
    
}


function GetAcademicYear() {
    GetSessionYear('ddlAcademicYear');
    GetSessionYear('ddlAcademicYearDD');
    GetSessionYear('ddlAcademicYearReview');
    GetSessionYear('ddlAcademicYearview');
    GetSessionYear('ddlAcademicYearDir');
    GetSessionYear('ddlAcademicYearDirNew');
    GetSessionYear('ddlAcademicYearADNew');
}
function GetDistrict() {
    var Divisions = $('#ddlDivision :selected').val();
    if (Divisions != "" && Divisions != null) {
        $("#ddlDistrict").empty();
        $("#ddlDistrict").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: "/Admission/GetDistrict",
            data: { Divisions: Divisions },
            success: function (data) {
                $.each(data, function () {
                    $("#ddlDistrict").append($("<option/>").val(this.district_id).text(this.district_ename));
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#ddlDistrict").empty();
        $("#ddlDistrict").append('<option value="">Select</option>');
    }

    //$("#ddlDistrict").empty();
    //$("#ddlDistrict").append('<option value="0">choose</option>');
    //$.ajax({
    //    url: "/Admission/GetDistrict",
    //    type: 'Get',
    //    contentType: 'application/json; charset=utf-8',
    //    success: function (data) {

    //        if (data != null || data != '') {
    //            $.each(data, function () {
    //                $("#ddlDistrict").append($("<option/>").val(this.district_id).text(this.district_ename));
    //            });
    //        }

    //    }, error: function (result) {
    //        alert("Error", "something went wrong");
    //    }
    //});
}
function GetApplicantTypeDDL() {
    GetApplicantType("ddlApplicantType");
    GetApplicantType("ddlApplicantTypeReview");
    GetApplicantType("ddlApplicantTypePublish");
    GetApplicantType("ddlApplicantTypeview");
    GetApplicantType("ddlApplicantTypeDir");
    GetApplicantType("ddlApplicantTypeDirNew");
    GetApplicantType("ddlApplicantTypeADNew");
}
function GetGradationType() {

    $("#ddlGradationType").empty();
    $("#ddlGradationType").append('<option value="0">choose</option>');
    $("#ddlGradationTypeReview").empty();
    $("#ddlGradationTypeReview").append('<option value="0">choose</option>');
    $("#ddlGradationTypeview").empty();
    $("#ddlGradationTypeview").append('<option value="0">choose</option>');
    $("#ddlGradationTypeNewAD").empty();
    $("#ddlGradationTypeNewAD").append('<option value="0">choose</option>');
    $("#ddlGradationTypeDir").empty();
    $("#ddlGradationTypeDir").append('<option value="0">choose</option>');
    $("#ddlGradationTypeNewDir").empty();
    $("#ddlGradationTypeNewDir").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetGradationType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlGradationType").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                    $("#ddlGradationTypeReview").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                    $("#ddlGradationTypeview").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                    if ($("#hdnSession").data('value') != loginUserRole.DD && this.GradationTypeId != 1) {
                        $("#ddlGradationTypeNewAD").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                        $("#ddlGradationTypeDir").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                        $("#ddlGradationTypeNewDir").append($("<option/>").val(this.GradationTypeId).text(this.GradationTypes));
                    }
                });
            }
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function funSentTo() {

    if (($("#users :selected").val()) == 6 || ($("#users :selected").val()) == 4) {
        $("#btnForward").show();
        $("#sendBtn3").attr("disabled", false);
    }
    else {
        $("#sendBtn3").attr("disabled", true);
    }
}
function funSentBackToDD() {

    if (($("#backusersAd :selected").val()) == 5 || ($("#backusersAd :selected").val()) == 4) {
        $("#sendBackADBtn").show();
        $("#sendBackADBtn").attr("disabled", false);
        $("#btnSendDir").attr("disabled", true);
        $("#ADReusers").attr("disabled", true);
    }
    else {
        $("#sendBackADBtn").attr("disabled", true);
        $("#btnSendDir").attr("disabled", false);
        $("#ADReusers").attr("disabled", false);
    }
}
function funSentfrmADToDir() {

    if (($("#ADReusers :selected").val()) == 1 || ($("#ADReusers :selected").val()) == 2 || ($("#ADReusers :selected").val()) == 6 ) {
        $("#btnSendDir").show();
        $("#btnSendDir").attr("disabled", false);
        $("#sendBackADBtn").attr("disabled", true);
        $("#backusersAd").attr("disabled", true);
    }
    else {
        $("#btnSendDir").attr("disabled", true);
        $("#sendBackADBtn").attr("disabled", false);
        $("#backusersAd").attr("disabled", false);
    }
}
function funSentFromADC() {
    if (($("#usersAdToDC :selected").val()) == 1 || ($("#usersAdToDC :selected").val()) == 2) {
        $("#btnSend").show();
        $("#btnSend").attr("disabled", false);
    }
    else {
        $("#btnSend").attr("disabled", true);
    }
}

function GetDivision() {
    $("#ddlDivision").empty();
    $("#ddlDivision").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetDivision",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlDivision").append($("<option/>").val(this.division_id).text(this.division_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
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

function btnForwardMeritListDD() {
    
    var roundId = $("#ddlRound").val();
    if (roundId > 1) {
        btnAproveMeritListDDCal();
    }
        var roleId = $('#users').val();
        var RoleSelected = $('#users :selected').text();
        //var MeritListId = $("#id").html();
        var remarks = $("#RemarksDD").val();

        var applicantType = $('#ddlApplicantType :selected').text();
        var year = $('#ddlAcademicYearDD :selected').text();

        if (roleId == 'choose') {
            bootbox.alert('<br> please select role');
        }
        else {
            bootbox.confirm({//$('#ddlGradationType :selected').text() + " Merit list for Notification: <b>" + $('#ddlApplicantType :selected').text() + " </b> for round <b>" + $('#ddlRound :selected').text() + "</b> for Session:  <b>" + $('#ddlAcademicYearDD :selected').text() + " </b> already generated."
                message: "<br> Are you sure you want to send the " + $('#ddlGradationType :selected').text() + " Merit List for Notification: <b>" + $('#ddlApplicantType :selected').text() + " </b> for round: <b>" + $('#ddlRound :selected').text() + "</b> for the Session:  <b>" + $('#ddlAcademicYearDD :selected').text() + " </b> to <b>" + RoleSelected + "</b> for review ?",
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
                            url: "/Admission/GetForwardMeritListDD",
                            type: 'Get',
                            data: { id: roleId, remarks: remarks, round: roundId },
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {

                                if (data == "success") {
                                    var _msg = "<br> <b>Deputy Director </b> has sent " + $('#ddlGradationType :selected').text() + " Merit List for Notification: <b>  " + applicantType + "</b> for round: <b>" + $('#ddlRound :selected').text() + "</b> for Session: <b>" + year + "</b> to  <b>" + RoleSelected + "</b> for review Successfully";
                                    bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                                }
                                else {
                                    var _msg = "<br> Fail to Forwad";
                                    bootbox.alert(_msg);
                                }
                            }, error: function (e) {
                                var _msg = "<br> Something went wrong.";
                                bootbox.alert(_msg);
                                $("#preloder, .loader").hide();
                            }
                        });
                    }
                    //else {
                    //    $("#preloder, .loader").hide();
                    //}

                }
            });
        }
    
    //**Comment**//
    //else {
    //    bootbox.confirm('Do you want to send final merit list?', (confirma) => {
    //        if (confirma) {
    //            $.ajax({
    //                url: "/Admission/GetForwardMeritListDD",
    //                type: 'Get',
    //                data: { id: roleId, remarks: remarks },
    //                contentType: 'application/json; charset=utf-8',
    //                success: function (data) {

    //                    if (data == "success") {                        
    //                        var _msg = "<b>Deputy Director </b> has send <b>  " + applicantType +"</b> final merit list for Session: <b>" + year + "</b> to  <b>" + RoleSelected + "</b> Successfully";
    //                        bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
    //                    }
    //                    else {
    //                        var _msg = "Fail to Forwad";
    //                        bootbox.alert(_msg);
    //                    }
    //                }, error: function (e) {
    //                    var _msg = "Something went wrong.";
    //                    bootbox.alert(_msg);
    //                    $("#preloder, .loader").hide();
    //                }                  
    //            });
    //        }
    //    });  
    //}

}

function btnCancelBtnDD() {

    $('#tbldivMerit').hide();
    $('#divSearchboxDD').hide();
    $("#divRemarksDD").hide();
    $('#btnapprove').hide();
    $("#ddlApplicantType").val('choose');
    $("#ddlRound").val('choose');
    $("#ddlGradationType").val(0);
    $("#divRemarksDD").hide();
    $('#ShowHideGenerateRankList').hide();
}

function btnCancelBtnDD2() {
    $('#tbldivMerit').hide();
    $('#divSearchboxDD').hide();
    $("#divRemarksDD").hide();
    $('#btnForward').hide();
    $('#disSentTo').hide();
    $("#ddlApplicantType").val(0);
    $("#ddlAcademicYearDD").val(0);
    $("#ddlGradationType").val(0);

}

function CancelBtnAd() {
    $('#tbldivReview').hide();
    $('#divRemarks').hide();
    $('#divpublish').hide();
    $("#ddlApplicantTypeReview").val(0);
    $("#ddlAcademicYearReview").val(0);
    $("#ddlGradationTypeReview").val(0);
    $('#divSendToDC').hide();
}
function btnAproveMeritListDDCal() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetPublishcalendarEvents",
        contentType: "application/json",
        success: function (datajson) {
            var frmDate = ""; var toDate = ""; var dte = new Date();
            var curDate = dte.getUTCDate();
            var CurMon = dte.getUTCMonth();
            var CurYear = dte.getUTCFullYear();
            if (datajson.length > 0) {
                $.each(datajson, function () {
                    if (this.Dt_DisplayTentativeGradationRE1 != null) {
                        frmDate = this.Dt_DisplayTentativeGradationRE1.split(',');
                        if (new Date(CurYear, CurMon, curDate) >= new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2]))) {
                            btnAproveMeritListDD();
                        }
                        else {
                            var _msg = "<br> Tentative calendar event date not reached yet ! <br> This event will reach on :<b> " + new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) + "</b>";
                            bootbox.alert(_msg);
                        }
                    }

                    return false;
                });
            }
            else {
                var _msg = "Calender notification not yet created.";
                bootbox.alert(_msg);
            }
        }       
    });
}

function btnAproveMeritListDD() {
    let isValidShift = true;
    let isValid = true;
    var year = $('#ddlAcademicYearDD :selected').text();
    var applicantType = $('#ddlApplicantType :selected').text();
    var round = $("#ddlRound :selected").text();
    var roundId = $("#ddlRound").val();
    var gradationType = $("#ddlGradationType :selected").text();
    var remarks = $("#RemarksDD").val();
    //var t = $('#tblGradationMeritList').DataTable('destroy');
    $('#tblGradationMeritList').DataTable().destroy();
    $('#tblGradationMeritList').DataTable({ "paging": false }).draw(false);
    var _Merit_table = $("#tblGradationMeritList tbody");
    var sendObj = [];
      
  
    _Merit_table.find("tr").each(function (len) {
        //t.destroy();
        var $tr = $(this);
        var slno = $tr.find(".slno").text();
        var ApplicationId = $tr.find(".ApplicationId").val();
        var ApplicantNumber = $tr.find(".ApplicantNumber").text();
        var ApplicantName = $tr.find(".ApplicantName").text();
        var FathersName = $tr.find(".FathersName").text();
        var Gender = $tr.find(".Gender").text();
        var DOB = $tr.find(".DOB").text();
        var Category = $tr.find(".Category").text();
        var MaxMarks = $tr.find(".MaxMarks").text();
        var MarksObtained = $tr.find(".MarksObtained").text();
        var Result = $tr.find(".Result").text();
        var locationName = $tr.find(".locationName").text();
        var Rank = $tr.find(".Rank").text();
        var Weitage = $tr.find(".Weitage").text();
        var Total = $tr.find(".Total").text();
        var Percentage = $tr.find(".Percentage").text();

        var obj = {
            //slno: slno,
            ApplicationId: ApplicationId,
            //here applicant personal details no need to send
            //method just for send approve the list
            //ApplicantNumber: ApplicantNumber,
            //ApplicantName: ApplicantName,
            //FathersName: FathersName,
           // Gender: Gender,
            //DOB: DOB,
           // Category: Category,
           // MaxMarks: MaxMarks,
            //MarksObtained: MarksObtained,
           // Result: Result,
            // locationName: locationName,
            Rank: Rank,
            Weitage: Weitage,
            Total: Total,
            Percentage: Percentage,
            //remarks: remarks 
        };
        sendObj.push(obj);
    });
    var finalObj = JSON.stringify({ lists: sendObj, remarks: remarks, round: roundId});
    $('#tblGradationMeritList').DataTable().destroy();
    $('#tblGradationMeritList').DataTable({ "paging": true }).draw(false);

    bootbox.confirm({
        message: "<br> Are you sure you want to publish <b>" + gradationType + "</b> merit list for Session: <b>" + year + "</b> Notification Type: <b>" + applicantType + "</b> Round: <b>" + round + "</b> ?",
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
                    //type: "Get",
                    url: "/Admission/GetGeneratbtnMeritList",
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    type: 'POST',
                    data: finalObj,
                    success: function (data) {

                        if (data == "success" && roundId == 1) {
                            //alert("successfully generated.")
                            //GetGradationMeritListDetails();

                            var _msg = "<br> <b>" + gradationType + "</b> merit list for Session: <b>" + year + "</b> Notification Type: <b>" + applicantType + "</b> Round: <b>" + round + "</b>  published successfully ";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else if (roundId == 1){
                            var _msg = "<br> Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }    
        }
    });

    //***Comment***//
    /*  //console.log(finalObj);
   bootbox.confirm('Do you want to publish tentative list?', (confirma) => {
        if (confirma) {
            $.ajax({
                //type: "Get",
                url: "/Admission/GetGeneratbtnMeritList",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                type: 'POST',
                data: finalObj,
                success: function (data) {
  
                    if (data == "success") {
                        //alert("successfully generated.")
                        //GetGradationMeritListDetails();

                        var _msg = "<b>"+ applicantType +"</b> Final merit list for Session: <b>" + year + "</b> published successfully ";
                        bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                    }
                    else {
                        var _msg = "Fail to send";
                        bootbox.alert(_msg);
                    }
                }, error: function (e) {
                    var _msg = "Something went wrong.";
                    bootbox.alert(_msg);
                    $("#preloder, .loader").hide();
                }
            });
        }    
    }); */ 
}

function GenerateMeritList() {
    var AcademicYear = $("#ddlAcademicYearDD").val();
    var ApplicantType = $("#ddlApplicantType").val();
    var round = $("#ddlRound").val();
    var GradationType = $("#ddlGradationType").val();

    if (AcademicYear == "choose") {
        bootbox.alert('<br> please select Session');
    }

    var IsValid = true;
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#AcademicyearErr").show();
    }
    else {
        $("#AcademicyearErr").hide();
    }
    if (IsValid && (ApplicantType == "choose" || ApplicantType == "0" || ApplicantType == null)) {
        IsValid = false;
        $("#ApplicantTypeErr").show();    
    }
    else {
        $("#ApplicantTypeErr").hide();   
    }

    if (IsValid && (round == "choose" || round == "0" || round == null)) {
        IsValid = false;
        $("#ddlRound-Required").show();
    }
    else {
        $("#ddlRound-Required").hide(); 
    }
    if (IsValid && round == 1 && (GradationType == "" || GradationType == "0" || GradationType == null)) {
        IsValid = false;
        $("#GradationTypeErr").show();     
    }
    else {
        $("#GradationTypeErr").hide();
    }

    if (IsValid) {
        $("#ddlApplicantType").show();
        $("#ddlGradationType").show();
        $("#ddlAcademicYearDD").show();
        $("#spnRound").text($('#ddlRound :selected').text());
        $("#lblRoundCnt").show();
        $("#tblGradationMeritList").show();
        $("#tbldivMerit").show();
        $("#divRemarksDD").show();
        $("#divSearchboxDD").show();
        $("#btnapprove").hide();

        //GetGradationMeritListDetails();

        if (($("#ddlGradationType :selected").val()) == 1) {
            $("#btnapprove").show();
            $("#btnForward").hide();
            $("#disSentTo").hide();
            $("#btnGenerate").show();
            $("#divRemarksDD").hide();
            //$("#btnpublishDD").hide();                 
        }
        else {
            $("#btnapprove").hide();
            $("#btnForward").show();
            $("#disSentTo").show();
            $("#btnGenerate").show();
            $("#divRemarksDD").show();
            //$("#btnpublishDD").show();                   
        }
        //$("#tbldivMerit").show();
        $("#ShowHideGenerateRankList").show();
        GetGradationMeritListDetails(0, 0, 0);
    }
}
//AD Login
function GetReviewGradationMeritList() {
    var generateId = $('#ddlGradationTypeReview :selected').val();
    var AcademicYearReviewId = $('#ddlAcademicYearReview :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeReview :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetReviewGradationMeritList",
        data: { generateId: generateId, AcademicYearReviewId: AcademicYearReviewId, ApplicantTypeId: ApplicantTypeId },
        contentType: "application/json",
        success: function (data) {

            $('#tblReviewGradationMeritList').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'DOB', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB);
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ]
            });
      
            if (data.length == 0) {
                $("#btnSend").attr("disabled", true);
                $("#usersAdToDC").attr("disabled", true);
                $("#remarksDC").attr("disabled", true);
                //$("#btnpublish").attr("disabled", true);
                //$("#sendBtnchanges").attr("disabled", true);
            } else {
                $("#btnSend").attr("disabled", false);
                $("#usersAdToDC").attr("disabled", false);
                $("#remarksDC").attr("disabled", false);
                //$("#btnpublish").attr("disabled", false);
                //$("#sendBtnchanges").attr("disabled", false);
            }
        }
    });
}

function btnSubmit() {
    var ApplicantType = $("#ddlApplicantTypeReview").val();
    var AcademicYear = $("#ddlAcademicYearReview").val();
    var GradationType = $("#ddlGradationTypeReview").val();
    var Remarks = $("#remarks").val();
    var IsValid = true;
    if (ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        IsValid = false;
        $("#ddlApplicantTypeReviewErr").show();
        $("#tblReviewGradationMeritList").hide();
        $("#divpublish").hide();
        $("#divRemarks").hide();
        $("#tbldivReview").hide();
    }
    else {
        $("#ddlApplicantTypeReviewErr").hide();
    }
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#ddlAcademicYearReviewErr").show();
        $("#tblReviewGradationMeritList").hide();
        $("#divpublish").hide();
        $("#divRemarks").hide();
        $("#tbldivReview").hide();
    }
    else {
        $("#ddlAcademicYearReviewErr").hide();
    }
    if (GradationType == "" || GradationType == "0" || GradationType == null) {
        IsValid = false;
        $("#ddlGradationTypeReviewErr").show();
        $("#tblReviewGradationMeritList").hide();
        $("#divpublish").hide();
        $("#divRemarks").hide();
        $("#tbldivReview").hide();
    }
    else {
        $("#ddlGradationTypeReviewErr").hide();
    }
    if (IsValid) {
        $("#ddlApplicantTypeReview").show();
        $("#ddlGradationTypeReview").show();
        $("#ddlAcademicYearReview").show();
        $("#tblReviewGradationMeritList").show();
        $("#divRemarks").show();
        $("#divpublish").show();
        $("#tbldivReview").show();
        if (($("#ddlGradationTypeReview :selected").val()) == 1) {
            $("#divRemarks").hide();
            $("#divpublish").hide();
            $("#btnReviewdata").hide();
            $('#divSendToDC').hide();
        }
        else {
            $("#divRemarks").show();
            $("#divpublish").show();
            $("#btnReviewdata").show();
            $('#divSendToDC').show();
        }
        GetReviewGradationMeritList();

    }
}

//GradationList Radio button
function GrdationList() {
    var rbId = $("input[name='merit']:checked").val();
    //var rbFinal = $('#final :selected').val();   
    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationList",
        data: { rbId: rbId },
        contentType: "application/json",
        success: function (data) {
            $('#tblMeritGradeList').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                "paging": false,
                "ordering": false,
                "info": false,
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'DOB', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB);
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ],

            });
        }
    });
}

//GradationListStatus
function GrdationListStatus() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationListStatus",
        contentType: "application/json",
        success: function (data) {
            if (data != null) {
                $('#tblGradationListstatus').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center' },
                        { 'data': 'ApplicantTypes', 'title': 'Notification Type', 'className': 'text-center' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-left' },
                        { 'data': 'roundName', 'title': 'Round', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Status' + '- Currently with', 'className': 'text-left' },
                        //{ 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                
                                if (oData.Status == 5)
                                    $(nTd).html("<input type='button' disabled='disabled' onclick='GetRemarksTable(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");
                                else
                                $(nTd).html("<input type='button' onclick='GetRemarksTable(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");

                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.FlowId == 5)
                                    $(nTd).html("<input type='button' onclick='GetMeritListForwardDD(" + oData.YearID + "," + oData.ApplicantTypeId + "," + oData.roundId + ")' class='btn btn-primary btn-xs' value='Edit' id='View'/>");
                                else
                                    $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                            }
                        }
                    ],
                });
            }
        }
    });
}

function GetMeritListForwardDD(yearId, ApplTypeId, roundId) {
    $("#ShowHideGenerateRankList").show();
    GetGradationMeritListDetails(yearId, ApplTypeId, roundId);
    $("#divRemarksDD").show();
    $("#btnapprove").hide();
    $("#btnForward").show();
    $("#disSentTo").show();
    $("#divRemarksDD").show();
}

function GetMeritstatusview(ApplicationId) {
    //var ApplicationId = ApplicationId;
    $.ajax({
        type: "GET",
        url: "/Admission/GetMeritListstatusPopup/" + ApplicationId,
        //data: { ApplicationId: ApplicationId, AcademicYearDC: AcademicYearDC, ApplicantTypeId: ApplicantTypeId },
        data: { ApplicationId: ApplicationId},
        contentType: "application/json",
        success: function (data) {
            var t = $('#tblMeritStatusViewpopup').DataTable({
                data: data,
                searching: true,
                "bFilter": false,
                "destroy": true,
                "bSort": true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]
                        }
                    }
                ],
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    // { 'data': 'dateofbirth', 'title': 'DOB', 'className': 'text-left DOB' },
                    {
                        'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            //var date = daterangeformate2(oData.DOB);
                            var date = oData.dateofbirth;
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                    //{
                    //    'data': 'RoleId', 'title': 'Rank', 'className': 'text-left Rank', 'visible': false,
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        
                    //        if (oData.RoleId == loginUserRole.DD) {
                    //            
                    //            $("#divRemarksDDView").show();
                    //            $("#divSentToDDView").show();
                    //            $("#divSentToBtnDDView").show();
                    //        }
                    //    }
                    //}
                ],

            });
            $('#btnSearchMeritDC').click(function () {
                t.search($("#txtSearchMeritDC").val()).draw();
            });
            $("#ApplicantName").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            $("#ApplicantNumber").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            if (data.length == 0) {
                // $("#sendBtnchangesDirtoDD").attr("disabled", true);
                //$("#backusersDirector").attr("disabled", true);
                $("#btnpublishByCD").attr("disabled", true);
                $("#RevStatusDir").attr("disabled", true);
                $("#sendBtnchangesDirtoDD").hide();
            }
            else {
                // $("#sendBtnchangesDirtoDD").attr("disabled", false);
                //$("#backusersDirector").attr("disabled", false);
                $("#btnpublishByCD").attr("disabled", false);
                $("#RevStatusDir").attr("disabled", false);
                $("#sendBtnchangesDirtoDD").show();
            }
        }
    });
}


function GetViewGradationMeritList() {
    var generateId = $('#ddlGradationTypeview :selected').val();
    var AcademicYear = $('#ddlAcademicYearview :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeview :selected').val();
    var round = $('#ddlRoundview :selected').val();
    var DivisionId = $('#ddlDivision :selected').val();
    var DistrictId = $('#ddlDistrict :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/viewMeritList",
        data: { generateId: generateId, ApplicantTypeId: ApplicantTypeId, AcademicYear: AcademicYear, DistrictId: DistrictId, DivisionId: DivisionId, round: round },
        contentType: "application/json",
        success: function (data) {
            var t =  $('#tblGradationListView').DataTable({
                data: data,
                searching: true,
                "bFilter": false,
                "destroy": true,
                "bSort": true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]
                        }
                    }
                ],
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'DOB', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB);
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' },
                    { 'data': 'roundName', 'title': 'Round', 'className': 'text-left Round' }
                ],
            });         
            $('#btnSearchView').click(function () {
                t.search($("#txtSearchView").val()).draw();
            });

            // EDIT: Capture enter press as well
            $("#ApplicantName").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            $("#ApplicantNumber").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
        }
    });
}

function btnviewMeritList() {
    var ApplicantType = $("#ddlApplicantTypeview").val();
    var GradationType = $("#ddlGradationTypeview").val();
    var round = $('#ddlRoundview :selected').val();
    //var DivisisionId = $("#ddlDivision").val();
    //var DistrictId = $("#ddlDistrict").val();
    var AcademicYear = $("#ddlAcademicYearview").val();
   
    var IsValid = true;
    if (ApplicantType == "" || ApplicantType == "choose" || ApplicantType == null) {
        IsValid = false;
        $("#ApplicantTypeviewErr").show();
        $("#tblGradationListView").hide();
        $("#tbldivview").hide();
        $("#txtSearchView").hide();
        $("#btnSearchView").hide();
        //$("#divpublish").hide();
        //$("#divRemarks").hide();
        //$("#tbldivReview").hide();

    }
    else {
        $("#ApplicantTypeviewErr").hide();
    }

    if (round == "" || round == "choose" || round == null) {
        IsValid = false;
        $("#ddlRoundview-Required").show();
        $("#tblGradationListView").hide();
        $("#txtSearchView").hide();
        $("#btnSearchView").hide();
        $("#tbldivview").hide();
        //$("#divpublish").hide();
        //$("#divRemarks").hide();
        //$("#tbldivReview").hide();
    }
    else {
        $("#ddlRoundview-Required").hide();
    }

    if (GradationType == "" || GradationType == "0" || GradationType == null) {
        IsValid = false;
        $("#GradationTypeviewErr").show();
        $("#tblGradationListView").hide();
        $("#txtSearchView").hide();
        $("#btnSearchView").hide();
        $("#tbldivview").hide();
        //$("#divpublish").hide();
        //$("#divRemarks").hide();
        //$("#tbldivReview").hide();
    }
    else {
        $("#GradationTypeviewErr").hide();
    }
    //if (DivisisionId == "" || DivisisionId == "0" || DivisisionId == null) {
    //    IsValid = false;
    //    $("#divisionviewErr").show();
    //    $("#tblGradationListView").hide();
    //    //$("#divpublish").hide();
    //    //$("#divRemarks").hide();
    //    //$("#tbldivReview").hide();
    //}
    //else {
    //    $("#divisionviewErr").hide();
    //}
    //if (DistrictId == "" || DistrictId == "0" || DistrictId == null) {
    //    IsValid = false;
    //    $("#districtviewErr").show();
    //    $("#tblGradationListView").hide();
    //    //$("#divpublish").hide();
    //    //$("#divRemarks").hide();
    //    //$("#tbldivReview").hide();
    //}
    //else {
    //    $("#districtviewErr").hide();
    //}
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#AcademicyearviewErr").show();
        $("#tblGradationListView").hide();
        $("#tbldivview").hide();
        $("#txtSearchView").hide();
        $("#btnSearchView").hide();
        //$("#divpublish").hide();
        //$("#divRemarks").hide();
        //$("#tbldivReview").hide();
    }
    else {
        $("#AcademicyearviewErr").hide();
    }
    if (IsValid) {

        $("#ddlApplicantTypeview").show();
        $("#ddlGradationTypeview").show();
        $("#ddlDivision").show();
        $("#ddlDistrict").show();
        $("#ddlAcademicYearview").show();
        $("#tblGradationListView").show();
        $("#tbldivview").show();
        $("#divSearchboxView").show();

        $("#txtSearchView").show();
        $("#btnSearchView").show();

        $("#spnRoundview").text($('#ddlRoundview :selected').text());
        $("#lblRoundCntview").show();

        if (($("#ddlGradationTypeReview :selected").val()) == 1) {
            //$("#divRemarks").hide();
            //$("#divpublish").hide();
            //$("#btnReviewdata").hide();
        }
        else {
            //$("#divRemarks").show();
            //$("#divpublish").show();
            //$("#btnReviewdata").show();
        }
        GetViewGradationMeritList();
    }
}

function ApplicantTypeSelect() {
    $('#tblGradationListView').hide();
}

function SendMeritList() {
    let isValidShift = true;
    let isValid = true;

    var _Merit_table = $("#tblReviewGradationMeritList tbody");
    var sendObj = [];

    _Merit_table.find("tr").each(function (len) {

        var $tr = $(this);
        var slno = $tr.find(".slno").text();
        var ApplicationId = $tr.find(".ApplicationId").val();
        var ApplicantNumber = $tr.find(".ApplicantNumber").text();
        var ApplicantName = $tr.find(".ApplicantName").text();
        var FathersName = $tr.find(".FathersName").text();
        var Gender = $tr.find(".Gender").text();
        var DOB = $tr.find(".DOB").text();
        var Category = $tr.find(".Category").text();
        var MaxMarks = $tr.find(".MaxMarks").text();
        var MarksObtained = $tr.find(".MarksObtained").text();
        var Result = $tr.find(".Result").text();
        var locationName = $tr.find(".locationName").text();
        var Rank = $tr.find(".Rank").text();

        var obj = {
            slno: slno,
            ApplicationId: ApplicationId,
            ApplicantNumber: ApplicantNumber,
            ApplicantName: ApplicantName,
            FathersName: FathersName,
            Gender: Gender,
            DOB: DOB,
            Category: Category,
            MaxMarks: MaxMarks,
            MarksObtained: MarksObtained,
            Result: Result,
            locationName: locationName,
            Rank: Rank
        };
        sendObj.push(obj);
    });
    var finalObj = JSON.stringify({ lists: sendObj });

    $.ajax({
        type: "Get",
        url: "/Admission/GetGeneratbtnMeritList",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: finalObj,
        success: function (data) {

            if (data == "success") {
                //alert("successfully generated.")
                var _msg = "Generated  Successfully";
                bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
            }
            else {
                var _msg = "Fail to Generated";
                bootbox.alert(_msg);
            }
            //alert("failed");
        }, error: function (e) {
            var _msg = "Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });
}

function btnPublishedMeritList() {
    var Status = 8;
    var remarks = $("#ReviewremarksDir").val();

    var applicantType = $('#ddlApplicantTypeDirNew :selected').text();
    var year = $('#ddlAcademicYearDirNew :selected').text();

    var table = $("#tblMeritlistDirectorfirstPage tbody");
    var Session = '';
    var ApplType = '';
    var round = '';
    table.find('tr').each(function (len) {
        var $tr = $(this);
        Session = $tr.find("td:eq(1)").text();
        ApplType = $tr.find("td:eq(2)").text();
        round = $tr.find("td:eq(3)").text();
    });


    bootbox.confirm({
        message: "<br><br>Are you sure you want to Approve and Publish the Final Merit list for Session: <b>" + Session + "</b> for Notification: <b>" + ApplType + "</b> for Round: <b>" + round + "</b>?",
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
                    url: "/Admission/PublishMeritList",
                    dataType: 'json',
                    data: { Status: Status, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {

                        if (response == "success") {
                            //var _msg = "<br> Approved & Published <b>" + applicantType + "</b> final list for Session: <b>" + year + "</b> successfully";
                            var _msg = "<br> Final Merit List for Session <b>" + year + "</b> Approved & Published successfully";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "<br> Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "<br> Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        }
    });
  /*
   bootbox.confirm('Do you want to publish final list?', (confirma) => {
        if (confirma) {
            $.ajax({
                type: "Get",
                url: "/Admission/PublishMeritList",
                dataType: 'json',
                data: { Status: Status, remarks: remarks},
                contentType: 'application/json; charset=utf-8',
                success: function (response) {

                    if (response == "success") {
                        var _msg = "Published <b>" + applicantType +"</b> final list for Session: <b>"+ year +"</b> successfully";
                        bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                    }
                    else {
                        var _msg = "Fail to send";
                        bootbox.alert(_msg);
                    }
                }, error: function (e) {
                    var _msg = "Something went wrong.";
                    bootbox.alert(_msg);
                    $("#preloder, .loader").hide();
                }
            });
        }
    });
   */
    
}

// Director/Commisioner tab-7
function GetGradationMeritAppListDir() {

    var generateId = $('#ddlGradationTypeDir :selected').val();
    var AcademicYearDC = $('#ddlAcademicYearDir :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeDir :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationMeritListDir",
        data: { generateId: generateId, ApplicantTypeId: ApplicantTypeId, AcademicYearDC: AcademicYearDC },
        contentType: "application/json",
        success: function (data) {

            $('#tblMeritlistDirector').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'DOB', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB);
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ],

            });

            if (data.length == 0) {
                $("#btnpublishByCD").attr("disabled", true);


            } else {
                $("#btnpublishByCD").attr("disabled", false);
            }
        }
    });
}
function btnReviewdataDir() {
    var ApplicantType = $("#ddlApplicantTypeDir").val();
    var AcademicYear = $("#ddlAcademicYearDir").val();
    var GradationType = $("#ddlGradationTypeDir").val();
    var Remarks = $("#remarksDir").val();

    var IsValid = true;

    if (ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        IsValid = false;
        $("#ApplicantTypeDirErr").show();
    }
    else {
        $("#ApplicantTypeDirErr").hide();
    }

    if (GradationType == "" || GradationType == "0" || GradationType == null) {
        IsValid = false;
        $("#GradationTypeDirErr").show();
    }
    else {
        $("#GradationTypeDirErr").hide();
    }
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#ddlAcademicYearDirErr").show();
    }
    else {
        $("#ddlAcademicYearDirErr").hide();
    }

    if (IsValid) {
        $("#ddlApplicantTypeDir").show();
        $("#ddlGradationTypeDir").show();
        $('#divDirCom').show();
        $('#divRemarksDirCom').show();
        $('#tblDirCom').show();
        //$("#ddlAcademicYearReview").show();

        GetGradationMeritAppListDir();
    }
}
function btnCancelBtnCD() {
    $('#divDirCom').hide();
    $('#divRemarksDirCom').hide();
    $('#tblDirCom').hide();
    $("#ddlApplicantTypeDir").val(0);
    $("#ddlAcademicYearDir").val(0);
    $("#ddlGradationTypeDir").val(0);
}
function btnChangesMeritList() {
  
    var backId = $('#backusersDirector').val();
    var Status = 5;
    var remarks = $("#ReviewremarksDir").val();   
    var applicantType = $('#ddlApplicantTypeDirNew :selected').text();
    var Sentback = $('#backusersDirector :selected').text();

    var year = $('#ddlAcademicYearDirNew :selected').text();
    var table = $("#tblMeritlistDirectorfirstPage tbody");
    var Session = '';
    var ApplType = '';
    var round = '';
    table.find('tr').each(function (len) {
        var $tr = $(this);
        Session = $tr.find("td:eq(1)").text();
        ApplType = $tr.find("td:eq(2)").text();
        round = $tr.find("td:eq(3)").text();
    });

    if (backId == 'choose') {
        bootbox.alert('<br> please select role');
    }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to send back the Final merit list for Session: <b>" + Session + "</b> Notification:<b>" + ApplType + "</b> round: <b>" + round +"</b> to <b> Deputy Director </b> for more correction/clarification?",
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
                        url: "/Admission/ChangesMeritList",
                        type: 'Get',
                        data: { backId: backId, Status: Status, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data == "success") {
                                // var _msg = "Send back for Correction <b>" + applicantType + "</b> tentative list to <b>" + Sentback + "</b> Successfully.";
                                var _msg = "<br>Final Merit List for Notification: <b>" + applicantType + "</b> for Session: <b>" + year + " </b> sent back to <b>" + Sentback + " </b> successfully for more correction/clarification.";
                                bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                            }
                            else {
                                var _msg = "<br> Fail to send";
                                bootbox.alert(_msg);
                            }
                        }, error: function (e) {
                            var _msg = "<br> Something went wrong.";
                            bootbox.alert(_msg);
                            $("#preloder, .loader").hide();
                        }
                    });
                }
            }
        });
    }
    /*
       else {
        bootbox.confirm('Do you want to Sent for Correction tentative list?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/ChangesMeritList",
                    type: 'Get',
                    data: { backId: backId, Status: Status, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                           // var _msg = "Send back for Correction <b>" + applicantType + "</b> tentative list to <b>" + Sentback + "</b> Successfully.";
                            var _msg = "Send back for correction <b>" + applicantType + "</b> final list for Session: <b>" + year + " </b> to <b>" + Sentback + " </b> successfully.";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
     */
  
  
}

function RevStatusApprove() {
    if (($("#RevStatusDir :selected").val()) == 2 || ($("#RevStatusDir :selected").val()) == 9 || ($("#RevStatusDir :selected").val()) == 7 ) {
        if (($("#RevStatusDir :selected").val()) == 2) {
            $("#sendBtnchangesDirtoDD").attr("disabled", true);
            $("#backusersDirector").attr("disabled", true);                       
            $("#btnpublishByCD").attr("disabled", false);
            //var RevStatusDir = $("#RevStatusDir :selected").text();
            //$("#ReviewremarksDir").val(RevStatusDir);
            $("#usersDirToCom").attr("disabled", true);
            $("#btnSendCom").attr("disabled", true);
        }
        else if (($("#RevStatusDir :selected").val()) == 7) {
            $("#sendBtnchangesDirtoDD").attr("disabled", true);
            $("#backusersDirector").attr("disabled", true);
            $("#btnpublishByCD").attr("disabled", true);
            //var RevStatusDir = $("#RevStatusDir :selected").text();
            //$("#ReviewremarksDir").val(RevStatusDir);
            $("#usersDirToCom").attr("disabled", false);
            $("#btnSendCom").attr("disabled", false);

        }
        else if (($("#RevStatusDir :selected").val()) == 9 )
        {           
            $("#btnpublishByCD").attr("disabled", true);
            $("#sendBtnchangesDirtoDD").attr("disabled", false);
            $("#backusersDirector").attr("disabled", false);
            //var RevStatusDir = $("#RevStatusDir :selected").text();
            //$("#ReviewremarksDir").val(RevStatusDir);
            $("#usersDirToCom").attr("disabled", true);
            $("#btnSendCom").attr("disabled", true);

        }
       
    }
    else {
        $("#btnpublishByCD").attr("disabled", true);
        $("#sendBtnchangesDirtoDD").attr("disabled", true);
        $("#backusersDirector").attr("disabled", true);
        var RevStatusDir = $("#RevStatusDir :selected").text();
        $("#ReviewremarksDir").val(RevStatusDir);
        $("#usersDirToCom").attr("disabled", true);
        $("#btnSendCom").attr("disabled", true);
    }
}
//TAB_5 :Send to AD to Director/Commisioner
function SendforDirector() {
    var roleId = $('#usersAdToDC').val();
    var remarks = $("#remarksDC").val();
    if (roleId == 'choose') {
        bootbox.alert('<br> please select role');
    }
    else {
        $.ajax({
            url: "/Admission/GetSendforDirector",
            type: 'Get',
            data: { id: roleId, remarks: remarks },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (data == "success") {
                    //var _msg = "Forwaded Successfully";
                    var _msg = "<br> Additional Director have been submitted Successfully.";
                    bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                }
                else {
                    var _msg = "<br> Fail to submit";
                    bootbox.alert(_msg);
                }
            }, error: function (e) {
                var _msg = "<br> Something went wrong.";
                bootbox.alert(_msg);
                $("#preloder, .loader").hide();
            }
            //    if (data == "success") {
            //        alert("forwaded successfully");
            //        $("#remarks").text("");
            //        $('#users').val("choose");
            //    }
            //    else
            //        alert("failed");

            //}, error: function (e) {
            //    var _msg = "Something went wrong.";
            //    bootbox.alert(_msg);
            //    $("#preloder, .loader").hide();
            //}
        });
    }
}

//New Modification-01052021
function GetGrdationListReviewDirCom() {
   // var generateId = $('#ddlGradationTypeNewDir :selected').val();
    var AcademicYearDC = $('#ddlAcademicYearDirNew :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeDirNew :selected').val();

    if (AcademicYearDC.toLowerCase() == 'select' || AcademicYearDC.toLowerCase() == 'choose') {
        AcademicYearDC = 0;
    }
    if (ApplicantTypeId = 'choose') {
        ApplicantTypeId = 0;
    }

    $.ajax({
        type: "GET",
        url: "/Admission/GetGrdationListReviewDirCom",
        contentType: "application/json",
        data: { ApplicantTypeId: ApplicantTypeId, AcademicYearDC: AcademicYearDC },
        success: function (data) {

            if (data != null) {
                $('#tblMeritlistDirectorfirstPage').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Slno', 'className': 'text-center' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                        { 'data': 'ApplicantTypes', 'title': 'Notification Type', 'className': 'text-center' },
                        { 'data': 'roundName', 'title': 'Round', 'className': 'text-center' },
                        //{ 'data': 'TentativeDescription', 'title': 'Gradation Type', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Status' + '- Currently with', 'className': 'text-left' },
                        //{ 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetRemarksTable(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.RoleId == 2) {
                                    if (oData.FlowId == 2)
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#ReviewListModel' onclick='GetGradationMeritAppListDirNewId(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    else
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else if (oData.RoleId == 1) {
                                    if (oData.FlowId == 1 && oData.Status!=2)
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#ReviewListModel' onclick='GetGradationMeritAppListDirNewId(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    else
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else {
                                    $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                        }
                    ],
                });
            }
        }
    });
}
function GetGradationMeritAppListDirNewId(ApplicationId) {
    var AcademicYearDC = $('#ddlAcademicYearDirNew :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeDirNew :selected').val();
    if (ApplicantTypeId.toLowerCase() == 'choose') {
        ApplicantTypeId = 0;
    }
    var ApplicationId = ApplicationId;

    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationMeritListDirNew/" + ApplicationId,
        data: { ApplicationId: ApplicationId, AcademicYearDC: AcademicYearDC, ApplicantTypeId: ApplicantTypeId },
        contentType: "application/json",
        success: function (data) {
   
            var t= $('#tblMeritlistDirectorNewDemo').DataTable({
                data: data,
                searching: true,
                "bFilter": false,
                "destroy": true,
                "bSort": true,
                dom: fnSetDTExcelBtnPos(),
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20]
                        }
                    }
                ],
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            //var date = daterangeformate2(oData.DOB);
                            var date = oData.dateofbirth;
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ],

            });
            $('#btnSearchMeritDC').click(function () {
                t.search($("#txtSearchMeritDC").val()).draw();
            });
            $("#ApplicantName").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            $("#ApplicantNumber").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            if (data.length == 0) {
               // $("#sendBtnchangesDirtoDD").attr("disabled", true);
                //$("#backusersDirector").attr("disabled", true);
                $("#btnpublishByCD").attr("disabled", true);
                $("#RevStatusDir").attr("disabled", true);
                $("#sendBtnchangesDirtoDD").hide();
            }
            else {
               // $("#sendBtnchangesDirtoDD").attr("disabled", false);
                //$("#backusersDirector").attr("disabled", false);
                $("#btnpublishByCD").attr("disabled", false);
                $("#RevStatusDir").attr("disabled", false);
                $("#sendBtnchangesDirtoDD").show();
            }
        }
    });
}
function CancelBtnNew() {
  
    $('#tblMeritListDirComNewId').hide();
}
function SendforCommissioner() {

    var roleId = $('#usersDirToCom').val();
    var remarks = $("#ReviewremarksDir").val();
    var RoleSelected = $('#usersDirToCom :selected').text();
    var year = $('#ddlAcademicYearDirNew :selected').text();
    var applicantType = $('#ddlApplicantTypeDirNew :selected').text();

    var table = $("#tblMeritlistDirectorfirstPage tbody");
    var Session = '';
    var ApplType = '';
    var round = '';
    table.find('tr').each(function (len) {
        var $tr = $(this);
        Session = $tr.find("td:eq(1)").text();
        ApplType = $tr.find("td:eq(2)").text();
        round = $tr.find("td:eq(3)").text();
    });

    if (roleId == 'choose') {
        bootbox.alert('<br> please select role');
    }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to send the Final merit list for Session: <b>" + Session + "</b> for Notification: <b>" + ApplType + "</b> for <b>" + round + "</b> to <b> " + RoleSelected + "</b> for review?",
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
                        url: "/Admission/GetSendforDirector",
                        type: 'Get',
                        data: { id: roleId, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {

                            if (data != null) {
                                //var _msg = "<b> Director </b> has Forwaded tentative list to <b>" + RoleSelected + "  </b> successfully";
                                var _msg = "<br> <b> Director </b> has sent Final Merit List for Notification: <b>" + applicantType + " </b> for Session: <b>" + year + "</b> to <b>" + RoleSelected + "  </b> for review successfully";
                                bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                            }
                            else {
                                var _msg = "<br> Fail to submit";
                                bootbox.alert(_msg);
                            }
                        }, error: function (e) {
                            var _msg = "<br> Something went wrong.";
                            bootbox.alert(_msg);
                            $("#preloder, .loader").hide();
                        }
                    });
                }
            }
        });
    }
    /*
      else {
        bootbox.confirm('Do you want to send the final merit list?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetSendforDirector",
                    type: 'Get',
                    data: { id: roleId, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {

                        if (data != null) {
                            //var _msg = "<b> Director </b> has Forwaded tentative list to <b>" + RoleSelected + "  </b> successfully";
                            var _msg = "<b> Director </b> has send <b>" + applicantType + " </b> final merit list for Session: <b>" + year + "</b> to <b>" + RoleSelected + "  </b> successfully";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to submit";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
     */
   
}
//Applicant Rank
GetApplicantRankGradation();
function GetApplicantRankGradation() {

    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantResultMarq",
        contentType: "application/json",
        success: function (data) {

            $('#tblApplicantRank').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'ApplicantName', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    //{ 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    //{
                    //    'data': 'DOB', 'title': 'DOB', 'className': 'text-left DOB',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        var date = daterangeformate2(oData.DOB);
                    //        var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                    //        $(nTd).html(date + hide);
                    //    }
                    //},
                    //{ 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left Category' },
                    //{ 'data': 'MaxMarks', 'title': 'Max Marks in 10th', 'className': 'text-left MaxMarks' },
                    //{ 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    //{ 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    //{ 'data': 'Weitage', 'title': 'Weitage', 'className': 'text-left Weitage' },
                    //{ 'data': 'Total', 'title': 'Total', 'className': 'text-left Total' },
                    //{ 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    //{ 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    //{ 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    //{ 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ]
            });
        }
    });
}

//Use Case 25 - AD(Review)-Tab-10
function GetGrdationListReviewADNew() {

    var generateId = $('#ddlGradationTypeNewAD :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeADNew :selected').val();
    var AcademicYearAD = $('#ddlAcademicYearADNew :selected').val();


    $.ajax({
        type: "GET",
        url: "/Admission/GetGrdationListReviewADNew",
        contentType: "application/json",
        data: { ApplicantTypeId: ApplicantTypeId, AcademicYearAD: AcademicYearAD },
        success: function (data) {

            if (data != null) {
                $('#tblMeritlistADReviewNew').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Slno', 'className': 'text-center' },
                        { 'data': 'Year', 'title': 'Session', 'className': 'text-center' },
                        { 'data': 'ApplicantTypes', 'title': 'Notification Type', 'className': 'text-center' },
                        { 'data': 'roundName', 'title': 'Round', 'className': 'text-center' },
                        //{ 'data': 'TentativeDescription', 'title': 'Gradation Type', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Status' + '- Currently with', 'className': 'text-left' },
                        //{ 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {                               
                                $(nTd).html("<input type='button' onclick='GetRemarksTable(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");                               
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (oData.RoleId == 4) {
                                    if (oData.FlowId==4)
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#ADReviewListModel' onclick='GetGradationMeritAppListADNewId(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    else
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else if (oData.RoleId == 6){
                                    if (oData.FlowId == 6)
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#ADReviewListModel' onclick='GetGradationMeritAppListADNewId(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    else
                                        $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                                else {
                                    $(nTd).html("<input type='button' data-toggle='modal' data-target='#MeritStatusModel' onclick='GetMeritstatusview(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    //$(nTd).html("<input type='button' disabled='disabled' data-toggle='modal' data-target='#ADReviewListModel' onclick='GetGradationMeritAppListADNewId(" + oData.ApplicationId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }                                   
                            }
                        }
                    ],
                });
            }
        }
    });
}
function GetRemarksTable(id) {
    $('#RemarksCommentsModal').modal('show');
    $.ajax({
        type: "Get",
        url: "/Admission/GetCommentsMeritListFile/" + id,
        data: { id: id },
        success: function (data) {
            var t = $('#CommentsTable').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'createdatetime', 'title': 'Date', 'className': 'text-left' },                    
                    { 'data': 'FromUser', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
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

function GetAcademicYearADNew(id) {

    var ApplicantType = $("#ddlApplicantTypeADNew").val();
    var AcademicYear = $("#ddlAcademicYearADNew").val();
    var IsValid = true;
    if (id != 0 && (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null)) {
        IsValid = false;
        $("#ApplicantTypeADNewErr").hide();
        $("#ddlAcademicYearADNewErr").hide();
        $("#tblDivADReviewNewId").hide();
    }
    else {
        $("#ApplicantTypeADNewErr").hide();
    }
    if (id != 0 && (ApplicantType == "" || ApplicantType == "0" || ApplicantType == null)) {
        IsValid = false;
        $("#ApplicantTypeADNewErr").show();
        $("#tblDivADReviewNewId").hide();
    }
    else {
        $("#ApplicantTypeADNewErr").hide();
    }
    if (IsValid) {
        $('#tblDivADReviewNewId').show();
        GetGrdationListReviewADNew();
    }
}
function GetApplicantTypeADNew() {

    var ApplicantType = $("#ddlApplicantTypeADNew").val();
    var AcademicYear = $("#ddlAcademicYearADNew").val();
    var IsValid = true;
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#ddlAcademicYearADNewErr").show();
        $("#tblDivADReviewNewId").hide();
    }
    else if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null || ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        IsValid = false;
        $("#ApplicantTypeADNewErr").hide();
        $("#ddlAcademicYearADNewErr").hide();
        $("#tblDivADReviewNewId").hide();
    }
    else if (AcademicYear != "" || AcademicYear != "0" || AcademicYear != null || ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        //IsValid = false;
        $("#ApplicantTypeADNewErr").hide();
        //$("#ddlAcademicYearADNewErr").hide();
        $("#tblDivADReviewNewId").hide();
    }
    else {
        $("#ddlAcademicYearADNewErr").hide();
    }
    if (IsValid) {
        $('#tblDivADReviewNewId').show();
        GetGrdationListReviewADNew();
    }
}

function GetAcademicYearDDc(id) {

    var ApplicantType = $("#ddlApplicantTypeDirNew").val();
    var AcademicYear = $("#ddlAcademicYearDirNew").val();
    var IsValid = true;
    if (id != 0 && (ApplicantType == "" || ApplicantType == "0" || ApplicantType == null)) {
        IsValid = false;
        $("#ApplicantTypeDirNewErr").show();
        $("#tblMeritListDirComNewId").hide();
    }
    else if (id != 0 && (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null )) {
        IsValid = false;
        $("#ApplicantTypeDirNewErr").hide();
        $("#ddlAcademicYearDirNewErr").hide();
        $("#tblMeritListDirComNewId").hide();
    }
    else {
        $("#ApplicantTypeDirNewErr").hide();
        $('#tblMeritListDirComNewId').hide();
    }
    if (IsValid) {
        $('#tblMeritListDirComNewId').show();
        GetGrdationListReviewDirCom();
    }
}
function GetApplicantTypeDirC() {
    var ApplicantType = $("#ddlApplicantTypeDirNew").val();
    var AcademicYear = $("#ddlAcademicYearDirNew").val();
    var IsValid = true;
    if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null) {
        IsValid = false;
        $("#ddlAcademicYearDirNewErr").show();
        $("#tblMeritListDirComNewId").hide();
    }
    else if (AcademicYear == "" || AcademicYear == "0" || AcademicYear == null || ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        IsValid = false;
        $("#ApplicantTypeDirNewErr").hide();
        $("#ddlAcademicYearDirNewErr").hide();
        $("#tblMeritListDirComNewId").hide();
    }
    else if (AcademicYear != "" || AcademicYear != "0" || AcademicYear != null || ApplicantType == "" || ApplicantType == "0" || ApplicantType == null) {
        $("#ApplicantTypeDirNewErr").hide();
        //$("#ddlAcademicYearADNewErr").hide();
        $("#tblMeritListDirComNewId").hide();
    }
    else {
        $("#ddlAcademicYearDirNewErr").hide();
        $('#tblMeritListDirComNewId').hide();
    }
    if (IsValid) {
        $('#tblMeritListDirComNewId').show();
        GetGrdationListReviewDirCom();
    }
}

function CancelBtnADNew() {
    $('#tblDivADReviewNewId').hide();
    //$('#ddlApplicantTypeADNew option:selected').val(0);
    //$('#ddlAcademicYearADNew option:selected').val(0);
}
function GetGradationMeritAppListADNewId(ApplicationId) {
    //var generateId = $('#ddlGradationTypeNewDir :selected').val();
    var ApplicantTypeId = $('#ddlApplicantTypeADNew :selected').val();
    var AcademicYearAD = $('#ddlAcademicYearADNew :selected').val();
    if (ApplicantTypeId = 'choose') {
        ApplicantTypeId = 0;
    }
    var ApplicationId = ApplicationId;

    $.ajax({
        type: "GET",
        url: "/Admission/GetGradationMeritAppListADNewId/" + ApplicationId,
        data: { ApplicationId: ApplicationId, AcademicYearAD: AcademicYearAD, ApplicantTypeId: ApplicantTypeId },
        contentType: "application/json",
        success: function (data) {
         var t=   $('#tblMeritlistADNewDemoId').DataTable({
                data: data,
                searching: true,
                "bFilter": false,
                "destroy": true,
             "bSort": true,
             
                columns: [
                    { 'data': 'slno', 'title': 'Sl. No.', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                   // { 'data': 'dateofbirth', 'title': 'DOB', 'className': 'text-left DOB' },
                    {
                        'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            //var date = daterangeformate2(oData.DOB);
                            var date = oData.dateofbirth;
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);
                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left Category' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'ExService', 'title': 'Ex-servicemen', 'className': 'text-left ExService' },
                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'KanndaMedium', 'title': 'Kannada Medium', 'className': 'text-left KanndaMedium' },
                    { 'data': 'HyderabadKarnatakaRegion', 'title': 'Hyderabad Karnataka Region', 'className': 'text-left HyderabadKarnatakaRegion' },
                    { 'data': 'MaxMarks', 'title': 'Max. Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Weitage', 'title': 'weightage', 'className': 'text-left Weitage' },
                    { 'data': 'Total', 'title': 'Total Marks', 'className': 'text-left Total' },
                    { 'data': 'Percentage', 'title': 'Percentage %', 'className': 'text-left Percentage' },
                    { 'data': 'Qual', 'title': 'Qualification', 'className': 'text-left Qualification' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' }
                ],
             dom: fnSetDTExcelBtnPos(),
             buttons: [
                 {
                     extend: 'excel',
                     text: 'Download Excel',
                     exportOptions: {
                         columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20]
                     }
                 }
             ],
            });
            $('#btnSearchMeritAD').click(function () {
                t.search($("#txtSearchMeritAD").val()).draw();
            });
            $("#ApplicantName").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
            $("#ApplicantNumber").keypress(function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    t.search($(this).val()).draw();
                }
            });
        }
    });
}
function SendforDirectorNew() {
    
    var roleId = $('#ADReusers').val();
    var remarks = $("#ADReviewremarks").val();
    var RoleSelected = $('#ADReusers :selected').text();
    var applicantType = $('#ddlApplicantTypeADNew :selected').text();
    var year = $('#ddlAcademicYearADNew :selected').text();

    var table = $("#tblMeritlistADReviewNew tbody");
    var Session = '';
    var ApplType = '';
    var round = '';
    table.find('tr').each(function (len) {
        var $tr = $(this);
        Session = $tr.find("td:eq(1)").text();
        ApplType = $tr.find("td:eq(2)").text();
        round = $tr.find("td:eq(3)").text();
    });

    if (roleId == 'choose') {
        bootbox.alert('<br> please select role');
    }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to send the Final merit list for Session: <b>" + Session + "</b> Notification: <b>" + ApplType + "</b> Round: <b>" + round + "</b> to <b> " + RoleSelected + "</b> for review?",
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
                        url: "/Admission/GetSendforDirector",
                        type: 'Get',
                        data: { id: roleId, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {

                            var UserRole = data[0].roleName;
                            if (data != null) {
                                var _msg = "<br> <b> " + UserRole + " </b> has sent final merit list for Notification: <b>" + ApplType + " </b> for Session: <b>" + year + "</b> to <b>" + RoleSelected + "  </b> successfully for review";
                                bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                            }
                            else {
                                var _msg = "<br> Fail to submit";
                                bootbox.alert(_msg);
                            }
                        }, error: function (e) {
                            var _msg = "<br> Something went wrong.";
                            bootbox.alert(_msg);
                            $("#preloder, .loader").hide();
                        }
                    });
                }
            }
        });
    }
    /*  
      else {
        bootbox.confirm('Do you want to send the final merit list?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetSendforDirector",
                    type: 'Get',
                    data: { id: roleId, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {

                        var UserRole = data[0].roleName;
                        if (data != null) {
                            var _msg = "<b> " + UserRole + " </b> has send <b>" + applicantType + " </b>final merit list for the year <b>" + year + "</b> to <b>" + RoleSelected + "  </b> successfully";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to submit";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
     */
   
}
function sendBackADtoDD() {
    var sentId = $('#backusersAd :selected').val();
    var Status = 5;
    var applicantType = $('#ddlApplicantTypeADNew :selected').text();
    var remarks = $("#ADReviewremarks").val();
    var year = $('#ddlAcademicYearADNew :selected').text();

    var table = $("#tblMeritlistADReviewNew tbody");
    var Session = '';
    var ApplType = '';
    var round = '';
    table.find('tr').each(function (len) {
        var $tr = $(this);
        Session = $tr.find("td:eq(1)").text();
        ApplType = $tr.find("td:eq(2)").text();
        round = $tr.find("td:eq(3)").text();
    });

    if (sentId == 'choose') {
        bootbox.alert('<br> please select role');
    }
    else {
        bootbox.confirm({
            message: "<br> Are you sure you want to send back the Final merit list for Session: <b>" + Session + "</b> Notification:<b>" + ApplType + "</b> round: <b>" + round + "</b> to <b> Deputy Director </b> for more correction/clarification?",
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
                        url: "/Admission/SentBacktoDDMeritList",
                        type: 'Get',
                        data: { Status: Status, sentId: sentId, remarks: remarks },
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data == "success") {
                                var _msg = "<br> Final merit list for Notification: <b>" + ApplType + "</b> for round: <b>" + round + "</b> for Session: <b>" + Session + " </b> sent back to <b> Deputy Director </b> for more correction/clarifiction successfully.";
                                bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                            }
                            else {
                                var _msg = "<br> Fail to send";
                                bootbox.alert(_msg);
                            }
                        }, error: function (e) {
                            var _msg = "<br> Something went wrong.";
                            bootbox.alert(_msg);
                            $("#preloder, .loader").hide();
                        }
                    });
                }
            }
        });
    }
    /*
      else {
        bootbox.confirm('Do you want to Send back the tentative list?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/SentBacktoDDMeritList",
                    type: 'Get',
                    data: { Status: Status, sentId: sentId, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                            var _msg = "Send back for correction <b>" + applicantType + "</b> tentative list for the year <b>"+ year +" </b> to <b> Deputy Director </b> successfully.";
                            bootbox.alert({ message: _msg, callback: function () { window.location.href = window.location.href; } });
                        }
                        else {
                            var _msg = "Fail to send";
                            bootbox.alert(_msg);
                        }
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });
    }
     */
   
}

function ExcelExportTentativeADL() {

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;

    $("#tblMeritlistADNewDemoId").table2excel({
        filename: "TentativeMeritList_" + today + ".xls",
    });
}

//********************************************************//

//function GetGradationMeritAppListDirNewId(ApplicationId) {
//    var ApplicationId = ApplicationId;
//    if (ApplicationId != null) {
//        $('#ReviewListModel').modal('show');
//    }

//    //$("#tblMeritlistDirectorNewDemo").hide();
//    //GetGradationMeritAppListDirNew();
//}

function OnChangeFilters(id) {
    var rndId = 'ddlRound';
    var orgId = id;
    var grdId = 'ddlGradationType';
    id = id.substring(rndId.length);
    id = grdId + id;
    $("#" + id).prop('disabled', false);
    if ($("#" + orgId).val() != 1) {
        $("#" + id).val(0);
        $("#" + id).prop('disabled', true);
    }
}
