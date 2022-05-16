$(document).ready(function () {

    $('.nav-tabs li:eq(0) a').tab('show');
    $(".EditOptionShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".EditOptionAdmittedShowGrid").hide();
    $(".EditOptionRejectedShowGrid").hide(); 
    GetSessionYear("SessionYearA");
    GetCourses("CourseTypeA");
    GetApplicantType("ApplicantTypeA");
    GetApplicantType("TraineeTypeA");
    GetAdmissionRounds("RoundOptionA");
    GetDivisionMaster("DivisionA");
    GetGenderList("GenderA");
    searchDivisionWiseDistrict("DivisionA");

    GetSessionYear("SessionYearR");
    GetCourses("CourseTypeR");
    GetApplicantType("ApplicantTypeR");
    GetApplicantType("TraineeTypeR");
    GetAdmissionRounds("RoundOptionR");
    GetDivisionMaster("DivisionR");
    GetGenderList("GenderR");
    searchDivisionWiseDistrict("DivisionR");
    //30-06-2021
    $("#disSentTo").hide();
    $("#btnsendback").hide();
    //$("#btnApprovedJD").hide();
    $("btnApproved").attr("disabled", true);
    $("#btnsendBack").attr("disabled", true);
    $("#users").attr("disabled", true);
    
    

    $('.date-picker').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear1').val(inst.selectedYear);
            $('#AcademicMonths1').val(inst.selectedMonth);
        },
        beforeShow: function (dateText) {

        }
    });
    $('#dateofbirthA').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "-50:-13",
        dateFormat: 'dd-mm-yy',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });
    $('#dateofbirthR').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "-50:-13",
        dateFormat: 'dd-mm-yy',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });

    $('#AdmisionTime').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: '-30d',
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        dateFormat: 'dd-mm-yy'
    });

    $('#PaymentDateA').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: 0,
        maxDate: '30d',
        dateFormat: 'dd-mm-yy'
    });
    $('#PaymentDateR').datepicker({
        dateFormat: 'dd-mm-yy',
        minDate: 0,
        maxDate: '30d',
        dateFormat: 'dd-mm-yy'
    });

    $('#Admitteddateofbirth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "-50:-13",
        dateFormat: 'dd-mm-yy',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });

    //$(".btnSaveSeatAllocationFeePay").prop('disabled', true);
});

function searchDistrictTalukWiseInstitue(id) {

    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);

    var DistrictVal = $("#District" + idlenChk + " :selected").val();
    var TalukVal = $("#Taluk" + idlenChk + " :selected").val();
    fnBuildAllDropDown("#ITIInstitute" + idlenChk);

    
    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetailsByDistrictTaluka',
        data: { District: DistrictVal, Taluka: TalukVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ITIInstitute" + idlenChk).append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDistrictWiseTaluk(id) {

    var DistrictVal = "";
    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);
    DistrictVal = $("#" + id + " :selected").val();

    fnBuildAllDropDown("#Taluk" + idlenChk);
    fnBuildAllDropDown("#ITIInstitute" + idlenChk);

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: DistrictVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Taluk" + idlenChk).append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDivisionWiseDistrict(id) {

    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);

    fnBuildAllDropDown("#District" + idlenChk);
    fnBuildAllDropDown("#Taluk" + idlenChk);
    fnBuildAllDropDown("#ITIInstitute" + idlenChk);

    var DivisionVal = "";
    if ($("#hdnSession").data('value') != 5 && (id == "DivisionA" || id == "DivisionR")) {
        var DivisionVal = $("#UserDivionId").data('value');
    }
    else {
        DivisionVal = $("#Division" + idlenChk + " :selected").val();
    }

    $.ajax({
        type: 'Get',
        url: '/Admission/GetDistrictMasterDivList',
        data: { Division: DivisionVal },
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#District" + idlenChk).append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetDivisionMaster(id) {

    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);

    var sessionValue = $("#hdnSession").data('value');
    if (sessionValue == 5) {
        $("#" + id).prop('disabled', false);
    }

    fnBuildAllDropDown("#District" + idlenChk);
    fnBuildAllDropDown("#Taluk" + idlenChk);
    fnBuildAllDropDown("#ITIInstitute" + idlenChk);

    $.ajax({
        type: 'Get',
        url: '/Admission/GetDivision',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#" + id).append($("<option/>").val(this.division_id).text(this.division_name));
                });
                if (sessionValue != 5) {
                    $("#" + id).val($("#UserDivionId").data('value'))
                }
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetGenderList(id) {

    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);

    $("#" + id).empty();
    $("#" + id).append('<option value="0">Select Gender</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetGenderList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + id).append($("<option/>").val(this.Gender_Id).text(this.Gender));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

$('a[href="#tab_1"]').click(function () {
    $("#tab_1").show();
    $("#tab_2").hide();
    $(".SearchOptionShowGrid").show();
    $(".SearchOptionAdmittedShowGrid").show();
    $("#EnableSendTo").hide();
    $(".EditOptionShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    $(".FinalButton").hide();
    $("#btnApprovedJD").hide(); 
    $("#btnsendback").hide();
    $("#disSentTo").hide();

    $("#divForTo").hide();
    $("#divbtnForward").hide();

    var tab = $('#tab_1').tabs('getSelected');
    tab.panel('refresh');

    //location.reload();
    //$('#ApplicantSeatDetailsA').dataTable().fnDestroy(); //Not workig correctly

   // $('#ApplicantSeatDetailsA').DataTable();
    //$('#ApplicantSeatDetailsR').dataTable().fnClearTable();
    //$(".SearchOptionRejectedShowGrid").hide(); 
    //$(".EditOptionRejectedShowGrid").hide();
    //$(".EditOptionAdmittedShowGrid").hide();
    //$(".ApplicantAdmittedSeatDetails").hide();
    //$(".ApplicantRejectedSeatDetails").hide();
});

$('a[href="#tab_2"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").show();

    $(".FinalButton").hide();
    $(".SearchOptionShowGrid").show();
    $(".SearchOptionAdmittedShowGrid").show();
    $(".EditOptionShowGrid").hide();
    $(".ApplicantSeatDetails").hide();
    //$('#ApplicantSeatDetailsR').dataTable().fnDestroy();
    var tab = $('#tab_2').tabs('getSelected');
    tab.panel('refresh');
    
});

//region .. Tab 1

$('.AadhaarNumberText').keyup(function () {
    var foo = $(this).val().split("-").join(""); // remove hyphens
    if (foo.length > 0) {
        foo = foo.match(new RegExp('.{1,4}', 'g')).join("-");
    }
    $(this).val(foo);
});

function IncomeMoneyPhoneValidation(eveObj, id) {

    var eligibleLen = 10;
    var idlen = id.length;
    var idlenChk = id.substring(idlen, idlen - 1);

    if (idlenChk == "txtFamilyAnnualIncome") {
        eligibleLen = 8;
    }
    else if (idlenChk == "txtPermanentPincode" || id == "txtPincode") {
        eligibleLen = 5;
    }
    else if (idlenChk == "txtMaximumMarks" || id == "txtMarksObtained" || id == "txtMinMarks") {
        eligibleLen = 2;
    }
    else if (idlenChk == "AccountNumber") {
        eligibleLen = 15;
    }

    var len = $("#" + id).val().length;
    if (eveObj.charCode == 45 || eveObj.charCode == 43 || eveObj.charCode == 101 || len > eligibleLen ||
        (eveObj.charCode == 46 && id == "AccountNumber")) {
        event.preventDefault();
    }
}

function ValidateEmail(val) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(val)) {
        return false;
    } else {
        return true;
    }
}

function getSelectedOptions(sel) {

    var idlen = sel.length;
    var idlenChk = sel.substring(idlen - 1);

    $("#DiffAbledCer" + idlenChk).val(0);
    $("#ExServiceCer" + idlenChk).val(0);
    $("#LandLoserCer" + idlenChk).val(0);
    $("#EcoWeakCer" + idlenChk).val(0);
    $("#KMCer" + idlenChk).val(0);
    var len = sel.options.length;
    for (var i = 0; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected) {
            if (opt.value == "2")
                $("#ExServiceCer" + idlenChk).val(1);
            else if (opt.value == "5")
                $("#EcoWeakCer" + idlenChk).val(1);
        }
    }
}

function ValidateDOB(dateString) {
    var parts = dateString.split("-");
    var dtDOB = new Date(parts[1] + "-" + parts[0] + "-" + parts[2]);
    var dtCurrent = new Date();
    $("#dobError").val(0);
    $("#dateofbirth-Required").hide();
    if (dtCurrent.getFullYear() - dtDOB.getFullYear() < 13) {
        $("#dobError").val(1);
        $("#dateofbirth-Required").show();
        bootbox.alert("<br><br>Age should not be less than 13 years")
        return false;
    }

    if (dtCurrent.getFullYear() - dtDOB.getFullYear() == 13) {

        if (dtCurrent.getMonth() < dtDOB.getMonth()) {
            $("#dobError").val(1);
            $("#dateofbirth-Required").show();
            bootbox.alert("<br><br>Age should not be less than 13 years")
            return false;
        }
        if (dtCurrent.getMonth() == dtDOB.getMonth()) {
            if (dtCurrent.getDate() < dtDOB.getDate()) {
                $("#dateofbirth-Required").show();
                $("#dobError").val(1);
                bootbox.alert("<br><br>Age should not be less than 13 years")
                return false;
            }
        }
    }
    return true;
}

function OnChangeFilters() {
    $('.ApplicantSeatDetails').hide();
    //$('#EnableSendTo').hide();
}

function GetDataAdmissionApplicants(id) {
    var IsValid = true;

    $("#SessionValue" + id + "-Required").hide();
    $("#CourseTypes" + id + "-Required").hide();
    $("#ApplicantType" + id + "-Required").hide();
    $("#RoundOption" + id + "-Required").hide();
    if ($("#SessionYear" + id + " :selected").val() == "choose") {
        $("#SessionValue" + id + "-Required").show();
        IsValid = false;
    }
    if ($("#CourseType" + id + " :selected").val() == "choose") {
        $("#CourseType" + id + "-Required").show();
        IsValid = false;
    }
    //if ($("#ApplicantType" + id + " :selected").val() == "choose") {
    //    $("#ApplicantType" + id + "-Required").show();
    //    IsValid = false;
    //}
    //if ($("#RoundOption" + id + " :selected").val() == "choose") {
    //    $("#RoundOption" + id + "-Required").show();
    //    IsValid = false;
    //}

    if (IsValid) {

        var SessionYear = $("#SessionYear" + id + " :selected").val();
        var CourseType = $("#CourseType" + id + " :selected").val();
        var ApplicantType = $("#ApplicantType" + id + " :selected").val();
        ApplicantType = isNaN(ApplicantType) ? 0 : ApplicantType;
        var RoundOption = $("#RoundOption" + id + " :selected").val();
        RoundOption = isNaN(RoundOption) ? 0 : RoundOption;
        var Division = $("#Division" + id + " :selected").val();
        var District = $("#District" + id + " :selected").val();
        var Taluk = $("#Taluk" + id + " :selected").val();
        var ITIInstitute = $("#ITIInstitute" + id + " :selected").val();
       
        if (id == "A") {
            AdmittedorRejected = 6;
            $("#disSentTo").show();
            $("#btnsendback").show();
            $("#divForTo").show();
            $("#divbtnForward").show();
            $("#btnApprovedJD").hide();
            $("#btnPublishDD").show();
                       
        }
        else {
            AdmittedorRejected = 3;
            $("#disSentTo").hide();
            $("#btnsendback").hide();
            $("#divForTo").hide();
            $("#divbtnForward").hide();
            $("#btnApprovedJD").show();
            $("#btnPublishDD").hide();
                      
        }
            
        var objSearchData = {
            Session: SessionYear, CourseType: CourseType, ApplicantType: ApplicantType, RoundOption: RoundOption, Division: Division,
            District: District, Taluk: Taluk, ITIInstitute: ITIInstitute, AdmittedorRejected: AdmittedorRejected
        }
        var sessionValue = $("#hdnSession").data('value');
        var buttonCommon = {
            exportOptions: {
                format: {
                    body: function (data, row, column, node) {
                        return data.includes("myModalPopUp") ? node.innerText : data;
                        return column === 3 ?
                            data.replace(/[$,]/g, '') :
                            data;
                    }
                }
            }
        };

        //$('#ApplicantSeatDetails' + id).DataTable().destroy();
        //$('#ApplicantSeatDetails' + id).DataTable({ "paging": false }).draw(false);

        $.ajax({
            type: "GET",
            url: "/Admission/GetAdmissionApplicantsDistLogin",
            data: objSearchData,
            contentType: "application/json",
            success: function (data) {

                if (data != null || data != '') {
                    fnGetYesNoForVal(data);
                }

                $('.ApplicantSeatDetails').show();
                var table = $('#ApplicantSeatDetails' + id).DataTable({
                    destroy: true,
                    data: data,
                    "bSort": true,
                    dom: fnSetDTExcelBtnPos(),
                    buttons: [
                        {   
                            extend: 'excel',
                            text: 'Download as Excel',
                            exportOptions: {
                                columns: [0, 5, 11, 12, 1, 2, 13, 3, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
                                    29, 30, 31, 5, 6, 7, 40, 41, 42, 43, 32, 33, 34, 35, 36, 37, 38, 39]
                            }
                        }
                    ],                   
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-left' },
                        { 'data': 'AdmissionRegistrationNumber', 'title': 'Admission Registration Number', 'className': 'text-left' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'ApplicantRank', 'title': 'Rank Number', 'className': 'text-left' },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                        { 'data': 'MISCode', 'title': 'MISITI Code', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'AdmittedStatusEx', 'title': 'Admission Status', 'className': 'text-left' },
                        {
                            'data': 'ApplicationId',
                            'title': 'Remarks',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                if (sessionValue == 5) {
                                    $("#FinalButton").show();
                                    $("#OnClickPublish").show();
                                    $("#OnClickSubmit").hide();
                                }
                                else {
                                    $("#FinalButton").show();
                                    $("#OnClickPublish").hide();
                                    $("#OnClickSubmit").show();
                                }


                                if (sessionValue == 5)
                                    $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");
                                else
                                    $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");
                            }
                        },
                        {
                            'data': 'ApplicationId',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var idval = 1; if (id == 'R') idval = 2;
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + "," + idval + ")' class='btn btn-primary' value='View' id='view'/>");
                            }
                        },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'DateOfBirth', 'title': 'DOB', 'className': 'text-left', 'visible': false },
                        { 'data': 'GenderName', 'title': 'Gender', 'className': 'text-left', 'visible': false },
                        { 'data': 'MobileNumber', 'title': 'Mobile Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'Email', 'title': 'E-Mail Id', 'className': 'text-left', 'visible': false },
                        { 'data': 'AadharNumber', 'title': 'UID Number', 'className': 'text-left', 'visible': false },
                        { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'ReligionName', 'title': 'Religion Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'MinorityCategory', 'title': 'Minority Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'CategoryName', 'title': 'Category Name', 'className': 'text-left', 'visible': false },
                        { 'data': 'HorizontalCategory', 'title': 'Admission Given in Horizontal Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'VerticalCategory', 'title': 'Admission Given in Vertical Category', 'className': 'text-left', 'visible': false },
                        { 'data': 'TraineeType', 'title': 'Trainee Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-left', 'visible': false },
                        { 'data': 'RationCardNo', 'title': 'Ration Card No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'IncomeCertificateNo', 'title': 'Income Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'AccountNumber', 'title': 'Bank Account No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'CasteCertNum', 'title': 'Caste Certificate', 'className': 'text-left', 'visible': false },
                        { 'data': 'TradeName', 'title': 'Trade', 'className': 'text-left', 'visible': false },
                        { 'data': 'Units', 'title': 'Unit', 'className': 'text-left', 'visible': false },
                        { 'data': 'Shifts', 'title': 'Shift', 'className': 'text-left', 'visible': false },
                        { 'data': 'DualType', 'title': 'Dual System', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmTime', 'title': 'Admission Date', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmisionFee', 'title': 'Admission Fee Amount (In ₹)', 'className': 'text-left', 'visible': false },
                        { 'data': 'AdmFeePaidStatus', 'title': 'Admission Fee Status', 'className': 'text-left', 'visible': false },
                        { 'data': 'ReceiptNumber', 'title': 'Receipt No.', 'className': 'text-left', 'visible': false },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'ApplicantType', 'title': 'Applicant Type', 'className': 'text-left', 'visible': false },
                        { 'data': 'RoundList', 'title': 'Round', 'className': 'text-left', 'visible': false }
                    ]
                });
                if (id == 'R') {
                    table.columns([2]).visible(false);
                }
                if ($("input[name='checkboxlist']:checked").val() == true) {
                    $("#btnApproved").attr("disabled", false);
                    $("#btnsendBack").attr("disabled", false);
                    $("#users").attr("disabled", false);
                    $("#btnForward").attr("disabled", false);
                    $("#usersFor").attr("disabled", false);
                    $("#btnPublish").attr("disabled", false);
                }
                else {
                    $("#btnApproved").attr("disabled", true);
                    $("#btnsendBack").attr("disabled", true);
                    $("#users").attr("disabled", true);
                    $("#btnForward").attr("disabled", true);
                    $("#usersFor").attr("disabled", true);
                    $("#btnPublish").attr("disabled", true);
                  
                }
            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}

$('.link').click(function (e) {
    e.preventDefault();
    var id = $(this).attr('id');
    var linktoRedirect = $("#" + id).attr('href');
    window.open(linktoRedirect, '_blank');;
});

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
        $(".TenthCOBSEType").hide();
        $("input[name=TenthCOBSEBoard][value=7]").prop('checked', true)
    }
    else {
        $(".TenthCOBSEType").show();
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
    }
}

function RuralUrbanLocation(id) {

    $("input:radio[name=RuralUrban" + id + "]").prop('checked', true);
    $("input[name='RuralUrban" + id + "]").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedWhichBasics(id) {

    $("input:radio[name=AppBasics" + id + "]").prop('checked', true);
    $("input[name=AppBasics" + id + "]").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedForSyallbus(id) {

    $("input:radio[name=TenthBoard" + id + "]").prop('checked', true);
    $("input[name=TenthBoard" + id + "]").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
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

function OnchangechkSameAsCommunicationAddress() {
    if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
        $("#txtPermanentAddress").val($("#txtCommunicationAddress").val());
        $("#PermanentDistricts").val($("#Districts :selected").val());

        $.ajax({
            type: 'Get',
            url: '/Admission/GetMasterApplicantData',
            success: function (datajson) {
                if (datajson.Resultlist != null || datajson.Resultlist != '') {
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $.each(datajson.Resultlist.GetDistrictList, function () {
                            $.each($(this.TalukListDet), function (index, item) {
                                $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                            });
                        });
                    }
                }
            }
        });

        $("#PermanentTalukas").val($("#Talukas :selected").val());
        $("#txtPermanentPincode").val($("#txtPincode").val());
        $("#txtPermanentAddress").attr('readonly', 'readonly');
        $('#PermanentDistricts').attr("disabled", true);
        $('#PermanentTalukas').attr("disabled", true);
        $('#txtPermanentPincode').attr('readonly', true);
    }
    else {
        $("#txtPermanentAddress").val('');
        $("#PermanentDistricts").val(0);
        $("#PermanentTalukas").val(0);
        $("#txtPermanentPincode").val('');
        $("#txtPermanentAddress").attr('readonly', false);
        $('#PermanentDistricts').attr("disabled", false);
        $('#PermanentTalukas').attr("disabled", false);
        $('#txtPermanentPincode').attr('readonly', false);
    }
}

function GetApplicationDetailsById(ApplicationId, idval) {
    var id = 'A'; if (idval == 2) id = 'R';
    $(".EditOptionShowGrid").find("*").prop('disabled', true);

    if (idval == 1) {
        $("#AdmittedStatusA").attr('checked', 'checked');
    }
    
    $(".SearchOptionShowGrid").hide();
    $(".RemovebkEditClick").hide();
    $(".EditOptionShowGrid").show();

    $('.btnSaveSeatAllocationFeePay').attr("disabled", true); 
    $("#btnsendback").hide();
    $("#disSentTo").hide();
    $("#divForTo").hide();
    $("#divbtnForward").hide();
    $("#btnApprovedJD").hide();
    $("#btnPublishDD").hide();


    $('input[type="text"], textarea').attr('readonly', false);

    RuralUrbanLocation(id);
    AppliedWhichBasics(id);
    AppliedForSyallbus(id);

    $("#Category" + id).empty();
    $("#Category" + id).append('<option value="0">Select Category</option>');

    $("#Religion" + id).empty();
    $("#Religion" + id).append('<option value="0">Select Religion</option>');

    $("#Gender" + id).empty();
    $("#Gender" + id).append('<option value="0">Select Gender</option>');

    $("#ApplicantType" + id).empty();
    $("#ApplicantType" + id).append('<option value="0">Select Applicant</option>');

    $("#Qualification" + id).empty();
    $("#Qualification" + id).append('<option value="0">Select Qualification</option>');

    $("#Districts" + id).empty();
    $("#Districts" + id).append('<option value="0">Select</option>');

    $("#PermanentDistricts" + id).empty();
    $("#PermanentDistricts" + id).append('<option value="0">Select</option>');

    $("#PhysicallyHanidcapType" + id).empty();
    $("#PhysicallyHanidcapType" + id).append('<option value="0">Select Disability</option>');

    $("#KanMedCer" + id).val(0); $("#DiffAbledCer" + id).val(0); $("#HoraNaduCer" + id).val(0); $("#ExeStuCer" + id).val(0); $("#HydKarCer" + id).val(0);
    $("#DocEduCerAcceptedImg" + id).hide();
    $("#DocEduCerRejectedImg" + id).hide();
    $("#DocCasCerAcceptedImg" + id).hide();
    $("#DocCasCerRejectedImg" + id).hide();
    $("#DocRatCerAcceptedImg" + id).hide();
    $("#DocRatCerRejectedImg" + id).hide();
    $("#DocIncCerAcceptedImg" + id).hide();
    $("#DocIncCerRejectedImg" + id).hide();
    $("#DocUIDCerAcceptedImg" + id).hide();
    $("#DocUIDCerRejectedImg" + id).hide();
    $("#DocRurCerAcceptedImg" + id).hide();
    $("#DocRurCerRejectedImg" + id).hide();
    $("#DocKanMedCerAcceptedImg" + id).hide();
    $("#DocKanMedCerRejectedImg" + id).hide();
    $("#DocDidAblCerAcceptedImg" + id).hide();
    $("#DocDidAblCerRejectedImg" + id).hide();
    $("#DocExStuCerAcceptedImg" + id).hide();
    $("#DocExStuCerRejectedImg" + id).hide();
    $("#DocHyKarRegAcceptedImg" + id).hide();
    $("#DocHyKarRegRejectedImg" + id).hide();
    $("#DocHorGadKanAcceptedImg" + id).hide();
    $("#DocHorGadKanRejectedImg" + id).hide();
    $("#DocOthCerAcceptedImg" + id).hide();
    $("#DocOthCerRejectedImg" + id).hide();
    $("#DocExSerAcceptedImg" + id).hide();
    $("#DocExSerRejectedImg" + id).hide();
    $("#DocEWSAcceptedImg" + id).hide();
    $("#DocEWSRejectedImg" + id).hide();
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
            $("#DocRurCerAcceptedImg" + id).hide();
            $("#txtEduCertRemarks" + id).val('');
            $("#txtCasteCertRemarks" + id).val('');
            $("#txtRationCardRemarks" + id).val('');
            $("#txtIncCertRemarks" + id).val('');
            $("#txtUIDRemarks" + id).val('');
            $("#txtRurCertRemarks" + id).val('');
            $("#txtKanMedRemarks" + id).val('');
            $("#txtDiffAbledCertRemarks" + id).val('');
            $("#txtExeCertRemarks" + id).val('');
            $("#txtHydKarnRemarks" + id).val('');
            $("#txtHorGadKannadigaRemarks" + id).val('');
            $("#txtOtherCertRemarks" + id).val('');
            $("#txtKashmirMigrantsRemarks" + id).val('');
            $("#txtExservicemanRemarks" + id).val('');
            $("#txtLLCertificateRemarks" + id).val('');
            $("#txtEWSCertificateRemarks" + id).val('');
            $("#EduCertificate" + id).val('');
            $("#CasteCertificate" + id).val('');
            $("#Rationcard" + id).val('');
            $("#Incomecertificate" + id).val('');
            $("#UIDNumber" + id).val('');
            $("#Ruralcertificate" + id).val('');
            $("#KannadamediumCertificate" + id).val('');
            $("#Differentlyabledcertificate" + id).val('');
            $("#ExemptedCertificate" + id).val('');
            $("#HyderabadKarnatakaRegion" + id).val('');
            $("#HoranaaduKannadiga" + id).val('');
            $("#OtherCertificates" + id).val('');
            $("#Exserviceman" + id).val('');
            $("#EWSCertificate" + id).val('');

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

                $("#ApplicantId" + id).val(datajson.Resultlist.ApplicationId);
                $("#ApplCredatedBy" + id).val(datajson.Resultlist.CredatedBy);
                $("#ApplFlowId" + id).val(datajson.Resultlist.FlowId);

                if (datajson.Resultlist.GetReligionList.length > 0) {
                    $.each(datajson.Resultlist.GetReligionList, function (index, item) {
                        $("#Religion" + id).append($("<option/>").val(this.Religion_Id).text(this.Religion));
                    });
                }

                if (datajson.Resultlist.GetGenderList.length > 0) {
                    $.each(datajson.Resultlist.GetGenderList, function (index, item) {
                        $("#Gender" + id).append($("<option/>").val(this.Gender_Id).text(this.Gender));
                    });
                }

                if (datajson.Resultlist.GetCategoryList.length > 0) {
                    $.each(datajson.Resultlist.GetCategoryList, function (index, item) {
                        $("#Category" + id).append($("<option/>").val(this.CategoryId).text(this.Category));
                    });
                }

                if (datajson.Resultlist.GetApplicantTypeList.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantTypeList, function (index, item) {
                        $("#ApplicantTypeSelect" + id).append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                    });
                }

                if (datajson.Resultlist.GetCasteList.length > 0) {
                    $.each(datajson.Resultlist.GetCasteList, function (index, item) {
                        $("#txtCaste" + id).append($("<option/>").val(this.CasteId).text(this.Caste));
                    });
                }

                $("#EduDocStatus" + id).empty();
                $("#EduDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#CasDocStatus" + id).empty();
                $("#CasDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#RationDocStatus" + id).empty();
                $("#RationDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#IncCerDocStatus" + id).empty();
                $("#IncCerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#UIDDocStatus" + id).empty();
                $("#UIDDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#RcerDocStatus" + id).empty();
                $("#RcerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#KanMedCerDocStatus" + id).empty();
                $("#KanMedCerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#DiffAblDocStatus" + id).empty();
                $("#DiffAblDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#ExCerDocStatus" + id).empty();
                $("#ExCerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#HyKarDocStatus" + id).empty();
                $("#HyKarDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#HorKanDocStatus" + id).empty();
                $("#HorKanDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#OtherCerDocStatus" + id).empty();
                $("#OtherCerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#KasMigDocStatus" + id).empty();
                $("#KasMigDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#ExserDocStatus" + id).empty();
                $("#ExserDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#LLCerDocStatus" + id).empty();
                $("#LLCerDocStatus" + id).append('<option value="0">Select Status</option>');

                $("#EWSDocStatus" + id).empty();
                $("#EWSDocStatus" + id).append('<option value="0">Select Status</option>');

                if (datajson.Resultlist.GetDocumentApplicationStatus.length > 0) {
                    $.each(datajson.Resultlist.GetDocumentApplicationStatus, function (index, item) {
                        if (this.ApplDocVerifiID != 14) {
                            $("#EduDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#CasDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#RationDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#IncCerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#UIDDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#RcerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#KanMedCerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#DiffAblDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#ExCerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#HyKarDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#HorKanDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#OtherCerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#KasMigDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#ExserDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#LLCerDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                            $("#EWSDocStatus" + id).append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        }
                    });
                }

                $("#ApplicableReservations" + id).empty();
                $.each(datajson.Resultlist.GetReservationList, function (index, item) {
                    $("#ApplicableReservations" + id).append($("<option/>").val(this.ReservationId).text(this.Reservations));
                });

                var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;
                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        $("#ApplicableReservations" + id + " option[value='" + e + "']").prop("selected", true);
                    });
                }
                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        if (e == "2")
                            $("#ExServiceCer" + id).val(1);
                        else if (e == "5")
                            $("#EcoWeakCer" + id).val(1);
                        $("#ApplicableReservations" + id + " option[value='" + e + "']").prop("selected", true);
                    });
                }

                $('#ApplicableReservations' + id).multiselect({});

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#Districts" + id).append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        $("#PermanentDistricts" + id).append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    });
                }

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {
                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach").show();
                                $('#aEduCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks" + id).val(this.Remarks);
                            }
                            $("#EDocAppId" + id).val(this.DocAppId);
                            $("#ECreatedBy" + id).val(this.CreatedBy);
                            $("#EduDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEduCerAcceptedImg" + id).show();
                                $("#DocEduCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEduCerRejectedImg" + id).show();
                                $("#DocEduCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach").show();
                                $('#aCasteCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks" + id).val(this.Remarks);
                            }
                            $("#CDocAppId" + id).val(this.DocAppId);
                            $("#CCreatedBy" + id).val(this.CreatedBy);
                            $("#CasDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocCasCerAcceptedImg" + id).show();
                                $("#DocCasCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocCasCerRejectedImg" + id).show();
                                $("#DocCasCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach").show();
                                $('#aRationCard ' + id).attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks" + id).val(this.Remarks);
                            }
                            $("#RDocAppId" + id).val(this.DocAppId);
                            $("#RCreatedBy" + id).val(this.CreatedBy);
                            $("#RationDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRatCerAcceptedImg" + id).show();
                                $("#DocRatCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRatCerRejectedImg" + id).show();
                                $("#DocRatCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach").show();
                                $('#aIncomeCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks" + id).val(this.Remarks);
                            }
                            $("#IDocAppId" + id).val(this.DocAppId);
                            $("#ICreatedBy" + id).val(this.CreatedBy);
                            $("#IncCerDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocIncCerAcceptedImg" + id).show();
                                $("#DocIncCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocIncCerRejectedImg" + id).show();
                                $("#DocIncCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach").show();
                                $('#aUIDNumber' + id).attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks" + id).val(this.Remarks);
                            }
                            $("#UDocAppId" + id).val(this.DocAppId);
                            $("#UCreatedBy" + id).val(this.CreatedBy);
                            $("#UIDDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocUIDCerAcceptedImg" + id).show();
                                $("#DocUIDCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocUIDCerRejectedImg" + id).show();
                                $("#DocUIDCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 6) {
                            $(".RuralUrban").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach").show();
                                $('#aRuralcertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks" + id).val(this.Remarks);
                            }
                            $("#RUDocAppId" + id).val(this.DocAppId);
                            $("#RUCreatedBy" + id).val(this.CreatedBy);
                            $("#RcerDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRurCerAcceptedImg" + id).show();
                                $("#DocRurCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRurCerRejectedImg" + id).show();
                                $("#DocRurCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 7) {
                            $(".KanndaMedium").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach").show();
                                $('#aKannadamediumCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks" + id).val(this.Remarks);
                            }
                            $("#KDocAppId" + id).val(this.DocAppId);
                            $("#KCreatedBy" + id).val(this.CreatedBy);
                            $("#KanMedCerDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocKanMedCerAcceptedImg" + id).show();
                                $("#DocKanMedCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocKanMedCerRejectedImg" + id).show();
                                $("#DocKanMedCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach").show();
                                $('#aDifferentlyabledcertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks" + id).val(this.Remarks);
                            }
                            $("#DDocAppId" + id).val(this.DocAppId);
                            $("#DCreatedBy" + id).val(this.CreatedBy);
                            $("#DiffAblDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocDidAblCerAcceptedImg" + id).show();
                                $("#DocDidAblCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocDidAblCerRejectedImg" + id).show();
                                $("#DocDidAblCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 9) {
                            $(".ExemptedFromStudyCertificate").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach").show();
                                $('#aExemptedCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks" + id).val(this.Remarks);
                            }
                            $("#ExDocAppId" + id).val(this.DocAppId);
                            $("#ExCreatedBy" + id).val(this.CreatedBy);
                            $("#ExCerDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExStuCerAcceptedImg" + id).show();
                                $("#DocExStuCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExStuCerRejectedImg" + id).show();
                                $("#DocExStuCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 10) {
                            $(".HyderabadKarnatakaRegion").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach").show();
                                $('#aHyderabadKarnatakaRegion' + id).attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks" + id).val(this.Remarks);
                            }
                            $("#HDocAppId" + id).val(this.DocAppId);
                            $("#HCreatedBy" + id).val(this.CreatedBy);
                            $("#HyKarDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHyKarRegAcceptedImg" + id).show();
                                $("#DocHyKarRegRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHyKarRegRejectedImg" + id).show();
                                $("#DocHyKarRegAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 11) {
                            $(".HoraNadu").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach").show();
                                $('#aHoranaaduKannadiga' + id).attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks" + id).val(this.Remarks);
                            }
                            $("#HGKDocAppId" + id).val(this.DocAppId);
                            $("#HGKCreatedBy" + id).val(this.CreatedBy);
                            $("#HorKanDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHorGadKanAcceptedImg" + id).show();
                                $("#DocHorGadKanRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHorGadKanRejectedImg" + id).show();
                                $("#DocHorGadKanAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 12) {
                            if (this.FilePath != null) {
                                $(".OtherCertificatesAttach").show();
                                $('#aOtherCertificates' + id).attr('href', '' + this.FilePath + '');
                                $("#txtOtherCertRemarks" + id).val(this.Remarks);
                            }
                            $("#ODocAppId" + id).val(this.DocAppId);
                            $("#OCreatedBy" + id).val(this.CreatedBy);
                            $("#OtherCerDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocOthCerAcceptedImg" + id).show();
                                $("#DocOthCerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocOthCerRejectedImg" + id).show();
                                $("#DocOthCerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach").show();
                                $('#aExserviceman' + id).attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks" + id).val(this.Remarks);
                            }
                            $("#ExSDocAppId" + id).val(this.DocAppId);
                            $("#ExSCreatedBy" + id).val(this.ExSCreatedBy);
                            $("#ExserDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExSerAcceptedImg" + id).show();
                                $("#DocExSerRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExSerRejectedImg" + id).show();
                                $("#DocExSerAcceptedImg" + id).hide();
                            }
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach").show();
                                $('#aEWSCertificate' + id).attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks" + id).val(this.Remarks);
                            }
                            $("#EWSDocAppId" + id).val(this.DocAppId);
                            $("#EWSCreatedBy" + id).val(this.EWSCreatedBy);
                            $("#EWSDocStatus" + id).val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEWSAcceptedImg" + id).show();
                                $("#DocEWSRejectedImg" + id).hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEWSRejectedImg" + id).show();
                                $("#DocEWSAcceptedImg" + id).hide();
                            }
                        }
                    });
                }

                $("#ApplicableReservations" + id).prop('disabled', false);
                if ($("#EWSCertificate" + id).prop('disabled') && $("#Exserviceman").prop('disabled')) {
                    $("#ApplicableReservations" + id).prop('disabled', true);
                }

                $("#ApplStatus" + id).val(datajson.Resultlist.ApplStatus);
                $("#academicyear1" + id).datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                $("#txtAadhaarNumber" + id).val(datajson.Resultlist.AadhaarNumber);
                $("#txtRationCard" + id).val(datajson.Resultlist.RationCard);
                $("#AccountNumber" + id).val(datajson.Resultlist.AccountNumber);
                $("#BankName" + id).val(datajson.Resultlist.BankName);
                $("#IFSCCode" + id).val(datajson.Resultlist.IFSCCode);
                if (datajson.Resultlist.PhysicallyHanidcapInd == true) {
                    $("input[name=PhysicallyHanidcapInd" + id + "][value=1]").prop('checked', true);
                    $("#DiffAbledCer").val(1);
                }
                else
                    $("input[name=PhysicallyHanidcapInd" + id + "][value=0]").prop('checked', true);
                PhysicallyHanidcapEna("PhysicallyHanidcapYes");

                if (datajson.Resultlist.PersonWithDisabilityCategory.length > 0) {
                    $.each(datajson.Resultlist.PersonWithDisabilityCategory, function (index, item) {
                        $("#PhysicallyHanidcapType" + id).append($("<option/>").val(this.PersonWithDisabilityCategoryId).text(this.DisabilityName));
                    });
                }

                if (datajson.Resultlist.PhysicallyHanidcapType == "" || datajson.Resultlist.PhysicallyHanidcapType == null)
                    $("#PhysicallyHanidcapType" + id).val(0);
                else
                    $("#PhysicallyHanidcapType" + id).val(datajson.Resultlist.PhysicallyHanidcapType);

                if (datajson.Resultlist.HoraNadu_GadiNadu_Kannidagas == true) {
                    $("input[name=HoraNadu" + id + " ][value=1]").prop('checked', true);
                    $("#HoraNaduCer").val(1);
                }
                else
                    $("input[name=HoraNadu" + id + "][value=0]").prop('checked', true);

                if (datajson.Resultlist.ExemptedFromStudyCertificate == true) {
                    $("input[name=ExemptedFromStudyCertificate" + id + "][value=1]").prop('checked', true);
                    $("#ExeStuCer").val(1);
                }
                else
                    $("input[name=ExemptedFromStudyCertificate" + id + "][value=0]").prop('checked', true);

                if (datajson.Resultlist.HyderabadKarnatakaRegion == true) {
                    $("input[name=HyderabadKarnatakaRegion" + id + "][value=1]").prop('checked', true);
                    $("#HydKarCer").val(1);
                }
                else
                    $("input[name=HyderabadKarnatakaRegion" + id + "][value=0]").prop('checked', true);

                if (datajson.Resultlist.KanndaMedium == true) {
                    $("input[name=KanndaMedium" + id + "][value=1]").prop('checked', true);
                    $("#KanMedCer").val(1);
                }
                else
                    $("input[name=KanndaMedium" + id + "][value=0]").prop('checked', true);

                $("#txtApplicantName" + id).val(datajson.Resultlist.ApplicantName);
                $("#txtFathersName" + id).val(datajson.Resultlist.FathersName);
                $("#txtParentOccupation" + id).val(datajson.Resultlist.ParentsOccupation);
                $('#ImgPhotoUpload' + id).attr("src", datajson.Resultlist.Photo);
                $("#IsUploaded" + id).val(datajson.Resultlist.Photo);

                var finalDOB = daterangeformate2(datajson.Resultlist.DOB, 1);
                $("#dateofbirth" + id).val(finalDOB);
                $("#txtMothersName" + id).val(datajson.Resultlist.MothersName);
                $("#Religion" + id).val(datajson.Resultlist.Religion);
                $('#Gender' + id).val(datajson.Resultlist.Gender);
                $("#Category" + id).val(datajson.Resultlist.Category);
                $("#MinorityCategory" + id).val(datajson.Resultlist.MinorityCategory);
                if (datajson.Resultlist.Caste != null)
                    $("#txtCaste" + id).val(datajson.Resultlist.Caste);
                $("#txtFamilyAnnualIncome" + id).val(datajson.Resultlist.FamilyAnnIncome);
                $("#ApplicantTypeSelect" + id).val(datajson.Resultlist.ApplicantType);

                //EducationDetails
                if (datajson.Resultlist.GetQualificationList.length > 0) {
                    $.each(datajson.Resultlist.GetQualificationList, function (index, item) {
                        $("#Qualification" + id).append($("<option/>").val(this.QualificationId).text(this.Qualification));
                    });
                }
                $('#Qualification' + id).val(datajson.Resultlist.Qualification);
                $("input[name=RuralUrban" + id + "][value=" + datajson.Resultlist.ApplicantBelongTo + "]").prop('checked', true);
                $("input[name=AppBasics" + id + "][value=" + datajson.Resultlist.AppliedBasic + "]").prop('checked', true);
                $("input[name=TenthBoard" + id + "][value=" + datajson.Resultlist.TenthBoard + "]").prop('checked', true);
                //$("#txtEduGrade").val(datajson.Resultlist.EducationGrade);
                $("input[name=TenthCOBSEBoard" + id + "][value=" + datajson.Resultlist.TenthCOBSEBoard + "]").prop('checked', true);
                TenthBoardStateType();
                $("#txtInstituteStudied" + id).val(datajson.Resultlist.InstituteStudiedQual);
                $("#txtMaximumMarks" + id).val(datajson.Resultlist.MaxMarks);
                $("#txtMinMarks" + id).val(datajson.Resultlist.MinMarks);
                $("#txtMarksObtained" + id).val(datajson.Resultlist.MarksObtained);
                if (datajson.Resultlist.Percentage != null && datajson.Resultlist.Percentage != "")
                    $("#lblPercAsPerMarks" + id).text(datajson.Resultlist.Percentage + "%");
                $("#Results" + id).val(datajson.Resultlist.ResultQual);
                if (datajson.Resultlist.studiedMathsScience == true)
                    $("input[name=studiedMathsScience" + id + "][value=1]").prop('checked', true);
                else
                    $("input[name=studiedMathsScience" + id + "][value=0]").prop('checked', true);
                //AddressDetails
                $("#txtCommunicationAddress" + id).val(datajson.Resultlist.CommunicationAddress);
                $("#Districts" + id).val(datajson.Resultlist.DistrictId);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#Talukas" + id).append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }

                $('#Talukas' + id).val(datajson.Resultlist.TalukaId);
                $("#txtPincode" + id).val(datajson.Resultlist.Pincode);
                $("input[name=chkSameAsCommunicationAddress" + id + "][value=" + datajson.Resultlist.SameAdd + "]").prop('checked', true);
                if (datajson.Resultlist.SameAdd == 1 || datajson.Resultlist.SameAdd == true)
                    $("#chkSameAsCommunicationAddress" + id).prop('checked', true);
                else
                    $("#chkSameAsCommunicationAddress" + id).prop('checked', false);

                OnchangechkSameAsCommunicationAddress();
                $("#txtPermanentAddress").val(datajson.Resultlist.PermanentAddress);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#PermanentDistricts" + id).append($("<option/>").val(item.district_lgd_code).text(item.district_ename));
                    });
                }
                $("#PermanentTalukas" + id).empty();
                $("#PermanentTalukas" + id).append('<option value="0">Select</option>');
                $("#PermanentDistricts" + id).val(datajson.Resultlist.PDistrict);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#PermanentTalukas" + id).append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }
                $('#PermanentTalukas' + id).val(datajson.Resultlist.PTaluk);
                $("#txtPermanentPincode" + id).val(datajson.Resultlist.PPinCode);
                $("#txtApplicantPhoneNumber" + id).val(datajson.Resultlist.PhoneNumber);
                $("#txtFathersPhoneNumber" + id).val(datajson.Resultlist.FatherPhoneNumber);
                $("#txtEmailId" + id).val(datajson.Resultlist.EmailId);
                $("#txtRemarks" + id).val('');

                $("#academicyear2" + id).text($("#academicyear1").val());
            }
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });

    $("#AdmisionTime" + id).val('');
    $("#PaymentDate" + id).val('');
    $.ajax({
        type: "GET",
        url: "/Admission/GetDataAllocationFeeDetails",
        data: { ApplicationId: ApplicationId },
        contentType: "application/json",
        success: function (datajsonDetails) {

            $.each(datajsonDetails.Resultlist, function (index, item) {

                //Allocation and Fee Details
                $("#ApplicationNumber" + id).text(this.ApplicantNumber);
                $("#ApplicationName" + id).text(this.ApplicantName);
                $("#RankNumber" + id).text(this.ApplicantRank);
                $("#DivisionName" + id).text(this.DivisionName);
                $("#DistrictName" + id).text(this.DistrictName);
                $("#TalukName" + id).text(this.TalukName);
                $("#MISCode" + id).text(this.MISCode);
                $("#InstituteName" + id).text(this.InstituteName);
                $("#InstituteType" + id).text(this.InstituteType);
                $("#VerticalCategory" + id).text(this.VerticalCategory);
                $("#HorizontalCategory" + id).text(this.HorizontalCategory);
                $("#TradeName" + id).text(this.TradeName);
                $("#Units" + id).text(this.Units);
                $("#Shifts" + id).text(this.Shifts);

                if (this.DualType == 1)
                    $("input[name=DualType" + id + "][value=1]").prop('checked', true);
                else
                    $("input[name=DualType" + id + "][value=0]").prop('checked', true);

                $("#TraineeType" + id).val(this.TraineeType);//Drop down
                if (this.TraineeType == null || this.TraineeType == 0) {
                    $("#TraineeType" + id).val("choose");
                }
               // $(".btnSaveSeatAllocationFeePay").prop('disabled', true);
                $(".PaymentGenerationGridCls").hide();
                $("#AllocationFeePay" + id).show();
                $(".AdmisionFee").prop('disabled', false);
                $(".AdmFeePaidStatus").prop('disabled', false);
                $("#PaymentDate" + id).prop('disabled', false);
                if (this.AdmFeePaidStatus == 1) {
                    $(".AdmisionFee").prop('disabled', true);
                    $(".AdmFeePaidStatus").prop('disabled', true);
                    $("#PaymentDate" + id).prop('disabled', true);
                    $(".PaymentGenerationGridCls").show();
                    $("#AllocationFeePay" + id).hide();
                    $('#PaymentGenerationGrid' + id).DataTable({
                        data: datajsonDetails.Resultlist,
                        "destroy": true,
                        "bSort": false,
                        sDom: 'lrtip',
                        "bPaginate": false,
                        "bInfo": false,
                        searching: false,
                        columns: [
                            { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-center' },
                            { 'data': 'AdmisionFee', 'title': 'Payment Amount (In ₹)', 'className': 'text-center' },
                            {
                                'data': 'PaymentDate', 'title': 'Payment Date', 'className': 'text - left DOB',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                    var date = daterangeformate2(oData.PaymentDate, 2);
                                    $(nTd).html(date);
                                }
                            },
                            { 'data': 'ReceiptNumber', 'title': 'Receipt Number', 'className': 'text-center' },
                            { 'data': 'PaymentMode', 'title': 'Payment Mode', 'className': 'text-center' },
                            { 'data': 'PaymentStatus', 'title': 'Payment Status', 'className': 'text-center' }
                        ]
                    });
                }

                if (this.PaymentDate != null)
                    $("#SaveSeatAllocationFeePay" + id).prop("disabled", true);
                else
                    $("#SaveSeatAllocationFeePay" + id).prop("disabled", false);

                var AdmisionTime = daterangeformate2(this.AdmisionTime, 1);
                $("#AdmisionTime" + id).val(AdmisionTime);

                if (this.ITIUnderPPP == 1)
                    $("input[name=ITIUnderPPP" + id + "][value=1]").prop('checked', true);
                else
                    $("input[name=ITIUnderPPP" + id + "][value=0]").prop('checked', true);

                if (this.AdmisionFee == 1200)
                    $("input[name=AdmisionFee" + id + "][value=1200]").prop('checked', true);
                else
                    $("input[name=AdmisionFee" + id + "][value=2400]").prop('checked', true);

                if (this.AdmFeePaidStatus == 1)
                    $("input[name=AdmFeePaidStatus" + id + "][value=1]").prop('checked', true);
                else
                    $("input[name=AdmFeePaidStatus" + id + "][value=0]").prop('checked', true);

                var PaymentDate = daterangeformate2(this.PaymentDate, 1);

                $("#PaymentDate").val(PaymentDate);

                if (this.ReceiptNumber == "" || this.ReceiptNumber == null)
                    $("#ReceiptNumber" + id).text("NIL");
                else
                    $("#ReceiptNumber" + id).text(this.ReceiptNumber);

                $("#lblToShowPendingStatus" + id).hide();
                if (this.AdmittedStatus == 2)
                    $("input[name=AdmittedStatus" + id + "][value=2]").prop('checked', true);
                else if (this.AdmittedStatus == 1 || this.AdmittedStatus == 3) {
                    if (this.AdmittedStatus == 1) {
                        $("#lblToShowPendingStatus" + id).show();
                    }
                    $("input[name=AdmittedStatus" + id + "][value=3]").prop('checked', true);
                }
                $("#txtRemarks").val('');

                if (this.AdmissionRegistrationNumber == "" || this.AdmissionRegistrationNumber == null)
                    $("#AdmissionRegistrationNumber" + id).text("NIL");
                else
                    $("#AdmissionRegistrationNumber" + id).text(this.AdmissionRegistrationNumber);

                if (this.RollNumber == "" || this.RollNumber == null)
                    $("#RollNumber" + id).text("NIL");
                else
                    $("#RollNumber" + id).text(this.RollNumber);

                if (this.StateRegistrationNumber == "" || this.StateRegistrationNumber == null)
                    $("#StateRegistrationNumber" + id).text("NIL");
                else
                    $("#StateRegistrationNumber" + id).text(this.StateRegistrationNumber);
            });
        }
    });
}

function CheckAadhaarNumberAvailability(id) {

    var idlen = id.length;
    var idlenChk = id.substring(idlen - 1);

    var txtAadhaarNumber = $("#" + id).val();
    var AadhaarNumber = txtAadhaarNumber.replace('-', '');
    AadhaarNumber = AadhaarNumber.replace('-', '');

    var ApplicationId = $("#ApplicantId" + idlenChk).val();
    $("#AadhaarNumberDuplicate" + idlenChk).text(0);
    $.ajax({
        type: "GET",
        url: "/Admission/CheckNameAvailability",
        data: { strName: AadhaarNumber, ApplicationId: ApplicationId, AadhaarRollNumber: 2 },
        success: function (data) {
            if (data == 1) {
                bootbox.alert("<br><br>AadhaarNumber Already Exist*");
                $("#AadhaarNumberDuplicate" + id).text(1);
            }
        }
    });
}

$('.FileUpload').change(function () {
    var ext = $(this).val().split('.').pop().toLowerCase();
    if (ext != "") {
        if ($.inArray(ext, ['pdf', 'PDF']) == -1) {
            bootbox.alert("<br><br>Kindly upload valid PDF file");
            $(this).val("");
        }
    } else {
        $(this).val("");
    }

    if (this.files[0].size > 1000000) {
        bootbox.alert("<br><br>File size should be less than 1MB");
        $(this).val("");
    }
});

function OnClickEditCancel() {
    $(".EditOptionShowGrid").hide();
    $(".SearchOptionShowGrid").show();
    $(".RemovebkEditClick").show();

    $("#btnsendback").show();
    $("#disSentTo").show();
    $("#divForTo").show();
    $("#divbtnForward").show();
    $("#btnApprovedJD").show();
    $("#btnPublishDD").show();
}

//To calculate percentage based on given marks
$('#txtMaximumMarks').focusout(function () {
    CalculatePercentage(true);
});

$('#txtMinMarks').focusout(function () {
    CalculatePercentage(true);
});

$('#txtMarksObtained').focusout(function () {
    CalculatePercentage(true);
});

function CalculatePercentage(IsValidVal) {

    var IsValidP = IsValidVal;
    $("#CalculationMaximumMarks-Required").hide();
    var MaximumMarks = $("#txtMaximumMarks").val();
    var MarksObtained = $("#txtMarksObtained").val();
    var MinMarks = $("#txtMinMarks").val();
    var Percentage = 0; var PercentageErr = false;
    var FullPercVal = 0;
    if (MaximumMarks != 0 && MarksObtained != 0) {

        if (MaximumMarks >= MarksObtained)
            Percentage = (MarksObtained / MaximumMarks) * 100;
        else
            PercentageErr = true;
    }
    else {
        PercentageErr = true;
    }

    if (MinMarks > MaximumMarks)
        PercentageErr = true;

    if (PercentageErr) {
        $("#CalculationMaximumMarks-Required").show();
        IsValidP = false;
    }
    FullPercVal = Percentage.toFixed(2);
    $("#lblPercAsPerMarks").text(FullPercVal + "%");

    return {
        IsValidP,
        FullPercVal
    };
}

function GetCommentDetails(ApplicationId) {
    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsRemarks",
        data: { ApplicationId: ApplicationId },
        success: function (data) {
            
            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CreatedOn', 'title': 'Date', 'className': 'text-left' },                    
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

function AddRemarksnDetailsById(ApplicationId) {
    
    $("#PopUpRemarksID").val(ApplicationId);
    $("#AddRemark").val($.trim($("#AddRemark").val()));
    $('#AddHistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsRemarksById",
        data: { ApplicationId: ApplicationId },
        success: function (data) {
           
            $.each(data, function (index, item) {
                $("#ExistingRemark").val(item.Remarks);
               return false;
            });
        }
    });
}



function OnClickSubmit() {

    var fileData = new FormData(); 
    //Applicant
    $('.editor-active :selected').each(function (i, selected) {
        fileData.append(
            "ApplicableReservationsqq", $(selected).val()
        );
    });
}

function btnApprovedJDList() {

    var checkboxes_value = [];
    var inputval = $(".editor-active").val();
    $('.editor-active').each(function () {       
        if (this.checked) {
            checkboxes_value.push($(this).val());
        }
    });
    checkboxes_value = checkboxes_value.toString(); 
    
    bootbox.confirm('Do you want to approve rejected applicant details?', (confirma) => {
        if (confirma) {
            $.ajax({
                type: "Get",
                url: "/Admission/ApprovedRejectedList",
                dataType: 'json',
                data: { checkboxes_value: checkboxes_value },
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    
                    if (response == "success") {
                        var _msg = "Approved rejected applicant details successfully";
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

function btnsendbackList() {
    var sentId = $('#users :selected').val();
    var RoleSelected = $('#users :selected').text();
    var checkboxes_value = [];
    var inputval = $(".editor-active").val();
    $('.editor-active').each(function () {
        if (this.checked) {
            checkboxes_value.push($(this).val());
        }
    });
    checkboxes_value = checkboxes_value.toString();
    
    if (sentId == 'choose') {
        bootbox.alert('please select the sent role ');
    } else {
        bootbox.confirm('Do you want to send back for correction the admitted applicant details ?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/SentBackAdmittedList",
                    type: 'Get',
                    data: { checkboxes_value:checkboxes_value, sentId: sentId},
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        /*if (data == "success") {*/
                            var _msg = "Send back for correction admitted applicant details to <b>" + RoleSelected +"</b> successfully.";
                            bootbox.alert({
                                message: _msg, callback: function () { window.location.href = window.location.href;
                                    //$('#ApplicantSeatDetails').DataTable().destroy();
                                    //$('#ApplicantSeatDetails').DataTable().ajax.reload();
                                    //$('#ApplicantSeatDetails').data.reload();
                                    //$('#ApplicantSeatDetails').DataTable().draw();
                                    //table.ajax.reload(null, false)
                                    //ajax.reload(callback, resetPaging);
                                    //$('#ApplicantSeatDetails').DataTable().clear().destroy();
                                    //GetDataAdmissionApplicants('A');

                                    //if ($.fn.DataTable.isDataTable('#ApplicantSeatDetails')) {
                                    //    $('#ApplicantSeatDetails').DataTable().destroy();
                                    //}
                                    //$('#ApplicantSeatDetails tbody').empty();
                                    //$('#ApplicantSeatDetails').dataTable({
                                    //    "autoWidth": false,
                                    //    "info": true,
                                    //    "paging": true,
                                    //    "scrollX": true,
                                    //    "bSort": true
                                    //});
                                    
                                    //GetDataAdmissionApplicants("A");
                                }
                            });

                        //}
                        //else {
                        //    var _msg = "Fail to send";
                        //    bootbox.alert(_msg);
                        //}
                    }, error: function (e) {
                        var _msg = "Something went wrong.";
                        bootbox.alert(_msg);
                        $("#preloder, .loader").hide();
                    }
                });
            }
        });  
    }
}

function btnForwardList() {
    
    var ForId = $('#usersFor :selected').val();
    var RoleSelected = $('#usersFor :selected').text();
    var checkboxes_value = [];
    var inputval = $(".editor-active").val();
    $('.editor-active').each(function () {
        if (this.checked) {
            checkboxes_value.push($(this).val());
        }
    });
    checkboxes_value = checkboxes_value.toString();

    if (ForId == 'choose') {
        bootbox.alert('please select the sent role');
    } else {
        bootbox.confirm('Do you want to Forward the Admitted applicant details?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetforwardAdmittedList",
                    type: 'Get',
                    data: { checkboxes_value: checkboxes_value, ForId: ForId},
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                            var _msg = "Admitted applicant details forwarded to <b> " + RoleSelected + " </b> successfully.";
                            bootbox.alert({ message: _msg, callback: function () { 
                                $('#ApplicantSeatDetails').DataTable().ajax.reload();
                            }
                            });
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
}

function btnPublishedList() {
    
    var checkboxes_value = [];
    var inputval = $(".editor-active").val();
    $('.editor-active').each(function () {
        if (this.checked) {
            checkboxes_value.push($(this).val());
        }
    });
    checkboxes_value = checkboxes_value.toString();
     bootbox.confirm('Do you want to Forward the Admitted data?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/GetforwardAdmittedList",
                    type: 'Get',
                    data: { checkboxes_value: checkboxes_value},
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == "success") {
                            var _msg = "Forward to " + RoleSelected + " successfully.";
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

function ColumnCheck() {
    $(".editor-active").each(function () {
        if ($("input[name='checkboxlist']:checked").prop('checked')) {      
            $("#btnApproved").attr("disabled", false);
            $("#btnsendBack").attr("disabled", false);
            $("#btnForward").attr("disabled", false);
            $("#users").attr("disabled", false);
            $("#usersFor").attr("disabled", false);  
            $("#Add").attr("disabled", false);          
            $("#btnPublish").attr("disabled", true);
        }
        else {
            $("#btnApproved").attr("disabled", true);
            $("#btnsendBack").attr("disabled", true);
            $("#btnForward").attr("disabled", true);
            $("#users").attr("disabled", true);
            $("#usersFor").attr("disabled", true);
            $("#btnPublish").attr("disabled", true);    
        }
    });   

    var _dataExchangeColumnList;
    var dataExchangeCheckColumnVM = $('#ApplicantSeatDetailsR').DataTable().row($(finalObj).parents('tr')).data();
    var dataExchangeCheckColumnList = $('#ApplicantSeatDetailsR').DataTable().rows().data();
    for (var i = 0; i < dataExchangeCheckColumnList.length; i++) {
        if (dataExchangeCheckColumnList[i].ApplicationId !== null) {
            if (dataExchangeCheckColumnList[i].ApplicationId === dataExchangeCheckColumnVM.ApplicationId) {
                dataExchangeCheckColumnList[i].ColumnCheck = thisObj.checked;
            }
        }
    }
    _dataExchangeColumnList = dataExchangeCheckColumnList;
}

function onclickAddRemarks() {
    
    var PopUpRemarksID = $("#PopUpRemarksID").val();

    var seatObj = {
        AddRemark: $('#AddRemark').val(),
        ApplicationId : PopUpRemarksID          
    };
    bootbox.confirm('Do you want to add remarks?', (confirma) => {
        if (confirma) {
            $.ajax({
                type: "post",
                url: "/Admission/clickAddRemarksTrans",
                dataType: 'json',
                data: JSON.stringify(seatObj),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    
                    if (response == "success") {
                        var _msg = "Added remarks successfully";
                        bootbox.alert({ message: _msg, callback: function () { 
                            $(this).tab('show');
                        }
                        });
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

function funSentTo() {
    
    if (($("#users :selected").val()) == 9) {
        $("#usersFor").attr("disabled", true);
        $("#btnForward").attr("disabled", true);
    }
    else {
        $("#usersFor").attr("disabled", false);
        $("#btnForward").attr("disabled", false);
    }
}
function funForTo() {
    
    if (($("#usersFor :selected").val()) == 6) {
        $("#users").attr("disabled", true);
        $("#btnsendBack").attr("disabled", true);
    }
    else {
        $("#users").attr("disabled", false);
        $("#btnsendBack").attr("disabled", false);
    }
}
//End region .. Tab 1

function fnBuildAllDropDown(id){
    $(id).empty();
    $(id).append($("<option/>").val(0).text("All"));
}