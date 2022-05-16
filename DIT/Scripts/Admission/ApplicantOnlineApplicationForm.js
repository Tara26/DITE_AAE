var maxInstPref = 20;
$(document).ready(function () {
    //GetCalenderNotificationDate();
    $("#ApplSubmittedRecipt").hide();
    $("#ApplSubmittedReciptVO").hide();
    $("#ApplReSubmittedRecipt").hide();
    $("#ApplVerifiedOfficer").hide();
    $("#WillingToParticipateNextRound").hide();

    $("#EnableDisableSubmit").hide();
    $("#PhysicallyHanidcapDiv1").hide();
    $("#PhysicallyHanidcapDiv2").hide();
    $("#EnableDisableSubmitReason").hide();
    $('.nav-tabs li:eq(0) a').tab('show');

    $('#myModal').modal('hide');
    GetStateList('ddlState');
    GetStateList('ddlState1');

    $('.date-picker').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear').val(inst.selectedYear);
            $('#AcademicMonths').val(inst.selectedMonth);
        },
    });

    $(".date-picker").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    });

    $('#dateofbirth').datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: "1975:-14",
        maxDate: '-14y',
        dateFormat: 'dd-mm-yy',
        //maxDate: '-14y',
        onSelect: function (dateString, dateofbirth) {
            ValidateDOB(dateString);
        }
    });

    function ValidateDOB(dateString) {
        var parts = dateString.split("-");
        var dtDOB = new Date(parts[1] + "-" + parts[0] + "-" + parts[2]);
        var dtCurrent = new Date();
        $("#dobError").val(0);
        $("#dateofbirth-Required").hide();
        if (dtCurrent.getFullYear() - dtDOB.getFullYear() < 14) {
            $("#dobError").val(1);
            $("#dateofbirth-Required").show();
            bootbox.alert("<br><br>Age should not be less than 14 years")
            return false;
        }

        if (dtCurrent.getFullYear() - dtDOB.getFullYear() == 14) {

            if (dtCurrent.getMonth() < dtDOB.getMonth()) {
                $("#dobError").val(1);
                $("#dateofbirth-Required").show();
                bootbox.alert("<br><br>Age should not be less than 14 years")
                return false;
            }
            if (dtCurrent.getMonth() == dtDOB.getMonth()) {
                if (dtCurrent.getDate() < dtDOB.getDate()) {
                    $("#dateofbirth-Required").show();
                    $("#dobError").val(1);
                    bootbox.alert("<br><br>Age should not be less than 14 years")
                    return false;
                }
            }
        }
        $("#dateofbirth-Required").text("Age: " + (dtCurrent.getFullYear() - dtDOB.getFullYear()).toString()).show();
        return true;
    }

    $(".toggle-accordion").on("click", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;

        $(this).toggleClass("active");

        if (numPanelOpen == 0) {
            openAllPanels(accordionId);
        } else {
            closeAllPanels(accordionId);
        }
    })

    openAllPanels = function (aId) {
        console.log("setAllPanelOpen");
        $(aId + ' .panel-collapse:not(".in")').collapse('show');
    }
    closeAllPanels = function (aId) {
        console.log("setAllPanelclose");
        $(aId + ' .panel-collapse.in').collapse('hide');
    }

    $("#EnableDistrictWiseFilter").hide();
    $("#CalenderNotificationEligibleInd").val(0);
    GetEligibleDateFrmCalenderEvents();
    GetApplicantsStatus();
    DocumentPaymentOption(0);
    GetcoursetypeListbycalendar();

});

//$("#AccountNumber").focusout(function () {
//    $("#AccountNumber-Required").hide();
//    var AccountNumber = $("#AccountNumber").val().length;
//    if (AccountNumber < 11) {
//        $("#AccountNumber-Required").show();
//        bootbox.alert("<br><br>Enter Valid 11 to 16 digits Account Number");
//    }
//});
function getParentOccupation(id, newId) {
    $("#" + newId).val($("#" + id).val());
}

$('a[href="#tab_1"]').click(function () {
   // GetEstablishViewofAppliant();
    $("#tab_1").show();
    $("#tab_2").hide();
});

$('a[href="#tab_2"]').click(function () {
    $("#tab_1").hide();
    $("#tab_2").show();
});

//$("input[name='StudyKannadaInOtherSt']:checked").val()
$('input:radio[name="StudyInKarnataka"]').change(function () {
    
    fnHideApplFormFields();
    if ($(this).val() == '1') {
        $(".clsParentGovtEE").hide();
        $(".clsStudyKannadaInOtherSt").hide();
        $('input:radio[name="ParentGovtEE"]').prop("checked", false);
        $('input:radio[name="StudyKannadaInOtherSt"]').prop("checked", false);
        $(".EditOptionShowGrid").show();
        $("#FinalSubmit").hide();
        
        GetMasterData();
        $('#ExemptedCertificate').attr('disabled', false);
       /* $('#EduCertificate').attr('disabled', false);*/
        $("#ExemptedFromStudyCertificateYes").prop('checked', true);
        $('#RStateBoardType1').prop('checked', true);
        $("input[name=TenthBoard][value=1]").prop('checked', true);


        $('input[name="EconomicallyWeakerSections"]').prop('checked', false)
        $('input[name="ExService"]').prop('checked', false)
        $('input[name="KanndaMedium"]').prop('checked', false)
        $('input[name="HoraNadu"]').prop('checked', false)
        $('input[name="RuralUrban"]').prop('checked', false)
        $('input[name="HyderabadKarnatakaRegion"]').prop('checked', false)
        $('input[name="HoraNadu"]').attr('disabled', true);
    }
    else {
        $(".clsParentGovtEE").show();
    }
});

$('input:radio[name="ParentGovtEE"]').change(function () {
    
    fnHideApplFormFields();
    $(".clsOtherState").hide();
    if ($(this).val() == '1') {
        $(".clsOtherState").show();
        $(".clsStudyKannadaInOtherSt").hide();
        $('input:radio[name="StudyKannadaInOtherSt"]').prop("checked", false);
        $(".EditOptionShowGrid").show();
        $("#FinalSubmit").hide();
        $('#RStateBoardType2').prop('checked', true);
        GetMasterData();
        $("input[name=TenthBoard][value=4]").prop('checked', true);
        $("input[name=TenthCOBSEBoard][value=7]").prop('checked', true);
        $(".TenthCOBSEType").show();

        $('#HoranaaduKannadiga').attr('disabled', false);
        $('#EduCertificate').attr('disabled', false);

        $('input[name="HoraNadu"]').prop('checked', false);
        $('input[name="HoraNadu"]').attr('disabled', true);
        $('input[name="ExService"]').prop('checked', false)
        $('input[name="KanndaMedium"]').prop('checked', false)
        $('input[name="RuralUrban"]').prop('checked', false)

        $('input[name="HyderabadKarnatakaRegion"]').prop('checked', false)
        $('input[name="HyderabadKarnatakaRegion"]').attr('disabled', true);
        $('input[name="chkHKregion"]').attr('disabled', true);
        $('input[name="RuralUrban"]').attr('disabled', true);
    }
    else {
        
        $(".clsStudyKannadaInOtherSt").show();
    }
});

$('input:radio[name="StudyKannadaInOtherSt"]').change(function () {
    fnHideApplFormFields();
    $(".clsOtherState1").hide();
    if ($(this).val() == '1') {
        $(".clsOtherState1").show();
        $(".EditOptionShowGrid").show();
        GetMasterData();

        $('#EduCertificate').attr('disabled', false);
        $("#HoraNaduYes").prop('checked', true);
        $("input[name=HoraNadu]").attr('disabled', true);

        $("input[name=HoraNadu][value='1']").prop('checked', true);

        $("input[name=TenthBoard][value='1']").attr('disabled', false);


        $('input[name="EconomicallyWeakerSections"]').prop('checked', false)
        $('input[name="ExService"]').prop('checked', false)
        $('input[name="KanndaMedium"]').prop('checked', false)

        $('input[name="RuralUrban"]').prop('checked', false)
        $('input[name="HyderabadKarnatakaRegion"]').prop('checked', false)
    }
    else {
        $("#NotEligibleApplFormLocalCriteria").show();
    }
});

function fnHideApplFormFields() {
    $(".EditOptionShowGrid").hide();
    $("#NotEligibleApplFormLocalCriteria").hide();
    $("#EnableDisableSubmitReason").hide();
}

function GetEligibleDateFrmCalenderEvents() {
  $.ajax({
    type: 'Get',
    url: '/Admission/GetEligibleDateFrmCalenderEvents',
    success: function (datajson) {
      var frmDate = ""; var toDate = ""; var dte = new Date();
      var curDate = dte.getUTCDate();
      var CurMon = dte.getUTCMonth();
      var CurYear = dte.getUTCFullYear();
      $.each(datajson, function () {
        //Application Eligible
          
        frmDate = this.From_date_applicationForm.split(',');

        if (this.From_date_applicationForm != null && this.To_date_applicationForm != null &&
          new Date(CurYear, CurMon, curDate) >= new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2]))) {

          toDate = this.To_date_applicationForm.split(',');

          if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))
            && new Date(CurYear, CurMon, curDate) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
            $("#CalenderNotificationEligibleInd").val(1);
                        $(".clsStudyInKarnataka").show();
            $(".NotEligibleApplicationForm").hide();
          }
          else {
            $("#CalenderNotificationEligibleInd").val(0);
                        $(".clsStudyInKarnataka").hide();
            $(".NotEligibleApplicationForm").show();
                        $(".clsStudyInKarnataka").hide();
          }
        }

        //For the 2nd round eligibe check

        frmDate = this.From_date_2ndRoundEntryChoiceTrade.split(',');

        if (this.From_date_2ndRoundEntryChoiceTrade != null && this.To_date_2ndRoundEntryChoiceTrade != null &&
          new Date(CurYear, CurMon, curDate) >= new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2]))) {

          toDate = this.To_date_2ndRoundEntryChoiceTrade.split(',');

          if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))
            && new Date(CurYear, CurMon, curDate) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
            $("#CalenderNotificationEligibleInd").val(1);
                        $(".clsStudyInKarnataka").show();
            $(".NotEligibleApplicationForm").hide();
            $("#WillingToParticipateNextRound").show();
          }
          else {
            $("#CalenderNotificationEligibleInd").val(0);
                        $(".clsStudyInKarnataka").hide();
            $(".NotEligibleApplicationForm").show();
          }
        }

        return false;
      });
    }
  });
}

//Update new trade table
$(".update-row-new").click(function () {
    fnAddNewRow();
});

function fnAddNewRow() {
    var PreferenceEligible = true;

    $("#Table-Trade-update").each(function () {
        var $tr = $(this);
        $tr.find(".select-Preference-multi-select-required").hide();
        $tr.find(".select-District-multi-select-required").hide();
        $tr.find(".select-Taluk-multi-select-required").hide();
        $tr.find(".select-InstituteType-multi-select-required").hide();
        $tr.find(".select-Institute-multi-select-required").hide();
        $tr.find(".select-Trade-multi-select-required").hide();

        var PreferenceMaxLength = this.rows.length;
        if (PreferenceMaxLength <= maxInstPref) {
            PreferenceEligible = true;
        }
        else {
            PreferenceEligible = false;
            bootbox.alert("<br><br>Maximum " + maxInstPref + " Institute can be allowed to choose for Preference Type")
        }
    });

    if (PreferenceEligible) {
        var $tableBody = $('#Table-Trade-update').find("tbody");
        var $trLast = $tableBody.find("tr:last");
        var $trNew = $trLast.clone();
        $trLast.after($trNew);
        //After clone
        $tableBody = $('#Table-Trade-update').find("tbody");
        var SINO = $("#Table-Trade-update tbody tr").length;
        $tableBody.find("tr:last").find(".SINO-multi-select").text(SINO);
        $tableBody.find("tr:last").find(".Preference-multi-select").val(0);
        $tableBody.find("tr:last").find(".District-multi-select").val();
        $tableBody.find("tr:last").find(".Taluk-multi-select").val();

        $tableBody.find("tr:last").find(".Institute-multi-select").empty();
        $tableBody.find("tr:last").find(".Institute-multi-select").append('<option value="0">Select</option>');

        $tableBody.find("tr:last").find(".Institute-multi-select").empty();
        $tableBody.find("tr:last").find(".Institute-multi-select").append('<option value="0">Select</option>');

        $tableBody.find("tr:last").find(".Trade-multi-select").empty();
        $tableBody.find("tr:last").find(".Trade-multi-select").append('<option value="0">Select</option>');

        $tableBody.find("tr:last").find(".btn-link").remove();
        $tableBody.find("tr:last").find(".update-trade-remove").click(function () {
            var lenght = $('#Table-Trade-update tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();
            }
            else {
                bootbox.alert("<br><br>Atleast one row required")
            }
        });

        $tableBody.find("tr:last").find(".District-multi-select").change(function () {
            searchDistrictWiseTaluka(this);
        });

        $tableBody.find("tr:last").find(".Taluk-multi-select").change(function () {
            searchDistrictWiseInstitue(this);
        });

        $tableBody.find("tr:last").find(".Institute-multi-select").change(function () {
            PreferenceInstitueWiseTrade(this);
        });
    }
}

$(".update-trade-remove").click(function () {
    var lenght = $('#Table-Trade-update tbody tr').length;
    if (lenght > 1) {
        $(this).closest("tr").remove();
    }
    else {
        bootbox.alert("<br><br>Atleast one row required")
    }
});

function GetApplicantTypeList() {

    $("#ApplicantType").empty();
    $("#ApplicantType").append('<option value="0">Select Applicant</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetApplicantTypeList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ApplicantType").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetReservationList() {

    $("#ApplicableReservations").empty();

    $.ajax({
        type: 'Get',
        url: '/Admission/GetReservationsList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ApplicableReservations").append($("<option/>").val(this.ReservationId).text(this.Reservations));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetQualificationList() {

    $("#Qualification").empty();
    $("#Qualification").append('<option value="0">Select Qualification</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetQualificationList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Qualification").append($("<option/>").val(this.QualificationId).text(this.Qualification));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

$('link').click(function (e) {
    e.preventDefault();
    var id = $(this).attr('id');
    var linktoRedirect = $("#" + id).attr('href');
    window.open(linktoRedirect, '_blank');;
});

function GetInstitutePreferenceList() {
    $("#PreferenceInstituteTypeId").empty();
    $("#PreferenceInstituteTypeId").append('<option value="0">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetLocationList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#PreferenceInstituteTypeId").append($("<option/>").val(this.location_id).text(this.location_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });

}

function searchOptionInstitue() {
    debugger;
    var searchInstitueOptionVal = $("input[name='searchInstitueOption']:checked").val();
    var PincodeVal = "";
    $("#InstituteList").empty();
    $("#InstituteList").append('<option value="Select">Select</option>');
    //if (searchInstitueOptionVal == 0) {
    //    $("#EnableDistrictWiseFilter").hide();
    //    PincodeVal = $("#txtPincode").val();
    //}
    //else {
        $("#EnableDistrictWiseFilter").show();
        GetInstituteDistricts();
/*    }*/

    if (PincodeVal != "") {
        $.ajax({
            type: 'Get',
            url: '/Admission/GetITICollegeDetails',
            data: { Pincode: PincodeVal },
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#InstituteList").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                    });


                }

            }, error: function (result) {
                bootbox.alert("<br><br>Error", "something went wrong");
            }
        });
    }
}

function GetInstituteDistricts() {

   
    $.ajax({
        type: 'Get',
        url: '/Admission/GetMasterDistrictList',
        data: { Divisions: '0' },
        success: function (data) {
            if (data != null || data != '') {
                $("#InstituteDistricts").empty();
                $("#InstituteDistricts").append('<option value="Select">Select</option>');
                $.each(data, function () {
                    $("#InstituteDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDistrictWiseInstitue(row) {
    var PincodeVal = "";
    DistrictVal = $(row).closest("tr").find(".District-multi-select").val(); //$('#InstituteDistricts :selected').val();
    TalukaVal = $(row).closest("tr").find(".Taluk-multi-select").val(); //$('#InstituteDistricts :selected').val();
    //$("#InstituteList").empty();
    $(row).closest("tr").find(".Institute-multi-select").empty();
    $(row).closest("tr").find(".Institute-multi-select").append($("<option/>").val(0).text("select"));
    $(row).closest("tr").find(".Trade-multi-select").empty();
    $(row).closest("tr").find(".Trade-multi-select").append($("<option/>").val(0).text("select"));
    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetailsByDistrictTaluka',
        data: { District: DistrictVal, Taluka: TalukaVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {

                    $(row).closest("tr").find(".Institute-multi-select").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                    //$("#InstituteList").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDocVerfiDistrictWiseInstitue() {

    var PincodeVal = "";
    PincodeVal = $('#InstituteDistricts :selected').val();
    $("#InstituteList").empty();
    $("#InstituteList").append('<option value="">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetails',
        data: { Pincode: PincodeVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {                    
                    $("#InstituteList").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}


function DocVerfiDistrictWiseInstitueOnLoad(districtWiseInstituteid) {
    debugger;
    var PincodeVal = "";
    PincodeVal = $('#InstituteDistricts :selected').val();
   
    $("#InstituteList").append('<option value="">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetails',
        data: { Pincode: PincodeVal },
        success: function (data) {
            if (data != null || data != '') {
                $("#InstituteList").empty();
                $.each(data, function () {
                    $("#InstituteList").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
                $("#InstituteList").val(districtWiseInstituteid);
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}
function GetInstituteDistrictsOnLoad(InstituteDistId) {
    debugger;

    $.ajax({
        type: 'Get',
        url: '/Admission/GetMasterDistrictList',
        data: { Divisions: '0' },
        success: function (data) {
            $("#InstituteDistricts").empty();
            if (data != null || data != '') {
                $("#InstituteDistricts").empty();
                $("#InstituteDistricts").append('<option value="Select">Select</option>');
                $.each(data, function () {
                    $("#InstituteDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                });

                $("#InstituteDistricts").val(InstituteDistId);
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function PreferenceDistrictWiseInstitue(row) {
    debugger;
    var PincodeVal = "";
    PincodeVal = $(row).val();

    $(row).closest("tr").find(".PreferenceInstitute").empty();
    $(row).closest("tr").find(".PreferenceInstitute").append('<option value="0">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetails',
        data: { Pincode: PincodeVal },
        success: function (data) {
            if (data != null || data != '') {
                $(row).closest("tr").find(".PreferenceInstitute").empty();
                $(row).closest("tr").find(".PreferenceInstitute").append('<option value="0">Select</option>');

                $.each(data, function () {
                    $(row).closest("tr").find(".PreferenceInstitute").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function PreferenceInstitueWiseTrade(row) {
    debugger;
    var PreferenceInstitute = "";
    PreferenceInstitute = $(row).val();
    var qualid = $("input[name='RAppBasics']:checked").val();
    var qual = '';
    if (qualid == '1')
        qual = '8th';
    if (qualid == '2')
        qual = '10th Pass';
    if (qualid == '3')
        qual = '10th Fail';
    $(row).closest("tr").find(".PreferenceTrade").empty();
    $(row).closest("tr").find(".PreferenceTrade").append('<option value="0">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeTradeDetails',
        data: { TradeCode: PreferenceInstitute, qual: qual },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $(row).closest("tr").find(".PreferenceTrade").append($("<option/>").val(this.trade_id).text(this.trade_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function SaveDocumentDetails() {
    
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null) {
        bootbox.alert("<br><br>Kindly Give the Applicant,Education, Address, Document Verification Center Information and Save the Details");
    }
    else {

        var fileData = new FormData();

        var IsValid = true;
        //EduCertificate
        var fileUpload = $('#EduCertificate').get(0);
        if ($('.aEduCertificate').is(':visible')) {
            if ($("#EduCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#EduCertificate-Required").hide();
            if ($("#EduCertificate").val() == "") {
                if ($("input[name=ParentGovtEE][value=1]").is(':checked')) {
                    $("#EduCertificate-Required").show();
                    IsValid = false;
                }
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
        }

        //CasteCertificate
        fileUpload = $('#CasteCertificate').get(0);
        if ($('.aCasteCertificate').is(':visible')) {
            if ($("#CasteCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#CasteCertificate-Required").hide();
            
            if ($("#CasteCertificate").val() == "") {
                if (($("#Category :selected").val()) == 1) {
                    $("#CasteCertificate-Required").hide();
                }
                else
                {
                $("#CasteCertificate-Required").show();
                    IsValid = false;
                    $('#CasteCertificate').attr('disabled', false);
                    bootbox.alert("Upload Caste Certificate under document section.!!");
                }
            }
            
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
        }

        //Rationcard
        //fileUpload = $('#Rationcard').get(0);
        //if ($('.aRationCard').is(':visible')) {
        //    if ($("#Rationcard").val() != "") {
        //        var files = fileUpload.files;
        //        for (var i = 0; i < files.length; i++) {
        //            fileData.append("RationcardPDF", files[i]);
        //        }
        //    }
        //}
        //else {
        //    $("#Rationcard-Required").hide();
        //    if ($("#Rationcard").val() == "") {
        //        $("#Rationcard-Required").show();
        //        IsValid = false;
        //    }
        //    else {
        //        var files = fileUpload.files;
        //        for (var i = 0; i < files.length; i++) {
        //            fileData.append("RationcardPDF", files[i]);
        //        }
        //    }
        //}

        //Incomecertificate 
        fileUpload = $('#Incomecertificate').get(0);
        if ($('.aIncomeCertificate').is(':visible')) {
            if ($("#Incomecertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
        }
        else {
            $("#Incomecertificate-Required").hide();
            if ($("#Incomecertificate").val() == "") {                
                if ($("input[name=EconomicallyWeakerSections]:checked").val() == "1"  && ($("#Category :selected").val()) == 1) {
                    $("#Incomecertificate-Required").show();
                    IsValid = false;
                    $('#Incomecertificate').attr('disabled', false);
                    bootbox.alert("Upload Incomee Certificate under document section.!!");
                }
                else if (($("#Category :selected").val()) == 1) {
                    $("#Incomecertificate-Required").hide();
                }
               else {
                    $("#Incomecertificate-Required").show();
                    IsValid = false;
                    $('#Incomecertificate').attr('disabled', false);
                    bootbox.alert("Upload Incomee Certificate under document section.!!");
                }
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("IncomePDF", files[i]);
                }
            }
        }

        //UID Number
        //fileUpload = $('#UIDNumber').get(0);
        //$("#UIDNumber-Required").hide();
        //if ($('.aUIDNumber').is(':visible')) {
        //    if ($("#UIDNumber").val() != "") {
        //        var files = fileUpload.files;
        //        for (var i = 0; i < files.length; i++) {
        //            fileData.append("UIDNumberPDF", files[i]);
        //        }
        //    }
        //}
        //else {
        //    $("#UIDNumber-Required").hide();
        //    if ($("#UIDNumber").val() == "") {
        //        $("#UIDNumber-Required").show();
        //        IsValid = false;
        //    }
        //    else {
        //        var files = fileUpload.files;
        //        for (var i = 0; i < files.length; i++) {
        //            fileData.append("UIDNumberPDF", files[i]);
        //        }
        //    }
        //}

        var RuralUrban = $('input[name=RuralUrban]:checked').val();
        //Rural Certificate
        fileUpload = $('#Ruralcertificate').get(0);
        $("#Ruralcertificate-Required").hide();
        if ($('.aRuralcertificate').is(':visible')) {
            if ($("#Ruralcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
        }
        else {
            $("#Ruralcertificate-Required").hide();
            if ($("#Ruralcertificate").val() == "" && RuralUrban == 1) {
                if ($("input[name=RAppBasics]:checked").val() == "1") {
                    $("#Ruralcertificate").attr("disabled", true);
                } else {
                $("#Ruralcertificate-Required").show();
                    IsValid = false;
                    $('#Ruralcertificate').attr('disabled', false);
                    bootbox.alert("Upload Rural Certificate under document section.!!");
                }
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
        }

        var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
        if (KanndaMedium == 1)
            $("#KanMedCer").val(1);
        else
            $("#KanMedCer").val(0);

        //KannadamediumCertificate
        fileUpload = $('#KannadamediumCertificate').get(0);
        $("#KannadamediumCertificate-Required").hide();
        if ($('.aKannadamediumCertificate').is(':visible')) {
            if ($("#KannadamediumCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
        }
        else {
            $("#KannadamediumCertificate-Required").hide();
            if (($("#KannadamediumCertificate").val() == "") && ($('input[name="KanndaMedium"]:checked').val() == 1)) {
                $("#KannadamediumCertificate-Required").show();
                IsValid = false;
                $('#KannadamediumCertificate').attr('disabled', false);
                bootbox.alert("Upload Kannada Medium Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("KannadaMediumPDF", files[i]);
                }
            }
        }

        var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val(); 
        if (PhysicallyHanidcapInd == 1)
            $("#DiffAbledCer").val(1);
        else
            $("#DiffAbledCer").val(0);

        //DifferentlyAbledPDF
        fileUpload = $('#Differentlyabledcertificate').get(0);
        $("#Differentlyabledcertificate-Required").hide();
        if ($('.aDifferentlyabledcertificate').is(':visible')) {
            if ($("#Differentlyabledcertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
            }
        }
        else {
            $("#Differentlyabledcertificate-Required").hide();
            if (($("#Differentlyabledcertificate").val() == '') && ($('input[name="PhysicallyHanidcapInd"]:checked').val() == 1)) {
                $("#Differentlyabledcertificate-Required").show();
                IsValid = false;
                $('#Differentlyabledcertificate').attr('disabled', false);
                bootbox.alert("Upload Ph Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
            }
        }

        var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
        if (ExemptedFromStudyCertificate == 1)
            $("#ExeStuCer").val(1);
        else
            $("#ExeStuCer").val(0);
        //ExemptedCertificate
        fileUpload = $('#ExemptedCertificate').get(0);
        $("#ExemptedCertificate-Required").hide();
        if ($('.aExemptedCertificate').is(':visible')) {
            if ($("#ExemptedCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
        }
        else {
            $("#ExemptedCertificate-Required").hide();
            if (($("#ExemptedCertificate").val() == "") && ($("#ExeStuCer").val() == 1)) {
                $("#ExemptedCertificate-Required").show();
                IsValid = false;
                $('#ExemptedCertificate').attr('disabled', false);
                bootbox.alert("Upload Study Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("StudyExemptedPDF", files[i]);
                }
            }
        }

        var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
        if (HyderabadKarnatakaRegion == 1)
            $("#HydKarCer").val(1);
        else
            $("#HydKarCer").val(0);
        //HyderabadKarnatakaRegion
        fileUpload = $('#HyderabadKarnatakaRegion').get(0);
        $("#HyderabadKarnatakaRegion-Required").hide();
        if ($('.aHyderabadKarnatakaRegion').is(':visible')) {
            if ($("#HyderabadKarnatakaRegion").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
        }
        else {
            $("#HyderabadKarnatakaRegion-Required").hide();
            if (($("#HyderabadKarnatakaRegion").val() == "") && ($("#HydKarCer").val() == 1)) {
                $("#HyderabadKarnatakaRegion-Required").show();
                IsValid = false;
                $('#HyderabadKarnatakaRegion').attr('disabled', false);
                bootbox.alert("Upload Hyderabad Karnataka Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
        }

        var HoraNadu = $('input[name=HoraNadu]:checked').val();
        if (HoraNadu == 1)
            $("#HoraNaduCer").val(1);
        else
            $("#HoraNaduCer").val(0);
        //HoranaaduKannadiga
        fileUpload = $('#HoranaaduKannadiga').get(0);
        $("#HoranaaduKannadiga-Required").hide();
        if ($('.aHoranaaduKannadiga').is(':visible')) {
            if ($("#HoranaaduKannadiga").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
        }
        else {
            $("#HoranaaduKannadiga-Required").hide();
            if (($("#HoranaaduKannadiga").val() == "") && $("input[name=ParentGovtEE][value=1]").is(':checked')) /*($("#HoraNaduCer").val() == 1)*/
            {
                $("#HoranaaduKannadiga-Required").show();
                IsValid = false;
                $('#HoranaaduKannadiga').attr('disabled', false);
                bootbox.alert("Upload Service Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HoranaaduGadinaaduKannadigaPDF", files[i]);
                }
            }
        }

        //OtherCertificates
        ////fileUpload = $('#OtherCertificates').get(0);
        ////$("#OtherCertificates-Required").hide();
        ////if ($('.aOtherCertificates').is(':visible')) {
        ////    if ($("#OtherCertificates").val() != "") {
        ////        var files = fileUpload.files;
        ////        for (var i = 0; i < files.length; i++) {
        ////            fileData.append("OtherCertificatesPDF", files[i]);
        ////        }
        ////    }
        ////}
        ////else {
        ////    $("#OtherCertificates-Required").hide();
        ////    if ($("#OtherCertificates").val() == "") {
        ////        //$("#OtherCertificates-Required").show();
        ////        //IsValid = false;
        ////    }
        ////    else {
        ////        var files = fileUpload.files;
        ////        for (var i = 0; i < files.length; i++) {
        ////            fileData.append("OtherCertificatesPDF", files[i]);
        ////        }
        ////    }
        ////}

        //Exserviceman
        fileUpload = $('#Exserviceman').get(0);
        $("#Exserviceman-Required").hide();
        if ($('.aExserviceman').is(':visible')) {
            if ($("#Exserviceman").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
        }
        else {
            $("#Exserviceman-Required").hide();
            if (($("#Exserviceman").val() == "") && ($("#ExServiceCer").val() == 1)) {
                $("#Exserviceman-Required").show();
                IsValid = false;
                $('#Exserviceman').attr('disabled', false);
                bootbox.alert("Upload Exserviceman Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("ExservicemanPDF", files[i]);
                }
            }
        }

        //EWSCertificate
        fileUpload = $('#EWSCertificate').get(0);
        $("#EWSCertificate-Required").hide();
        if ($('.aEWSCertificate').is(':visible')) {
            if ($("#EWSCertificate").val() != "") {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
        }
        else {
            $("#EWSCertificate-Required").hide();
            if (($("#EWSCertificate").val() == "") && ($("#EcoWeakCer").val() == 1)) {
                $("#EWSCertificate-Required").show();
                IsValid = false;
                $('#EWSCertificate').attr('disabled', false);
                bootbox.alert("Upload Economic Weaker Section Certificate under document section.!!");
            }
            else {
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EWSCertificatePDF", files[i]);
                }
            }
        }

        //V3 Review validation
        //$("#DifferentlyabledcertificateYes-Required").hide();
        //if ((($("#Differentlyabledcertificate").val() != "") && ($("#DiffAbledCer").val() != 1))
        //    || ($('.aDifferentlyabledcertificate').is(':visible') && ($("#DiffAbledCer").val() != 1))) {
        //    $("#DifferentlyabledcertificateYes-Required").show();
        //    $("#PhysicallyHanidcapYes").focus();
        //    IsValid = false;
        //}

        //$("#HoranaaduKannadigaYes-Required").hide();
        //if (( ($("#HoranaaduKannadiga").val() != "") && ($("#HoraNaduCer").val() != 1) )
        //    || ( $('.aHoranaaduKannadiga').is(':visible') && ($("#HoraNaduCer").val() != 1) ) ) {
        //    $("#HoranaaduKannadigaYes-Required").show();
        //    $("#HoraNaduNo").focus();
        //    IsValid = false;
        //}

        //$("#ExemptedFromStudyCertificateYes-Required").hide();
        //if ((($("#ExemptedCertificate").val() != "") && ($("#ExeStuCer").val() != 1))
        //    || ($('.aExemptedCertificate').is(':visible') && ($("#ExeStuCer").val() != 1))) {
        //    $("#ExemptedFromStudyCertificateYes-Required").show();
        //    $("#ExemptedFromStudyCertificateYes").focus();
        //    IsValid = false;
        //}

        //$("#HyderabadKarnatakaRegionYes-Required").hide();
        //if ((($("#HyderabadKarnatakaRegion").val() != "") && ($("#HydKarCer").val() != 1))
        //    || ($('.aHyderabadKarnatakaRegion').is(':visible') && ($("#HydKarCer").val() != 1))) {
        //    $("#HyderabadKarnatakaRegionYes-Required").show();
        //    $("#HyderabadKarnatakaRegionYes").focus();
        //    IsValid = false;
        //}

        //$("#KannadamediumCertificateYes-Required").hide();
        //if ((($("#KannadamediumCertificate").val() != "") && ($("#KanMedCer").val() != 1))
        //    || ($('.aKannadamediumCertificate').is(':visible') && ($("#KanMedCer").val() != 1))) {
        //    $("#KannadamediumCertificateYes-Required").show();
        //    $("#KanndaMediumYes").focus();
        //    IsValid = false;
        //}

        //$("#RuralcertificateYes-Required").hide();
        //if ((($("#Ruralcertificate").val() != "") && (RuralUrban != 1))
        //    || ($('.aRuralcertificate').is(':visible') && (RuralUrban != 1))) {
        //    $("#RuralcertificateYes-Required").show();
        //    $(".RuralUrbanID").focus();
        //    IsValid = false;
        //}

        //$("#ExservicemanYes-Required").hide();
        //if ((($("#Exserviceman").val() != "") && ($("#ExServiceCer").val() != 1))
        //    || ($('.aExserviceman').is(':visible') && ($("#ExServiceCer").val() != 1))) {
        //    $("#ExservicemanYes-Required").show();
        //    $("#ApplicableReservations").focus();
        //    IsValid = false;
        //}

        //$("#EWSCertificateYes-Required").hide();
        //if ((($("#EWSCertificate").val() != "") && ($("#EcoWeakCer").val() != 1))
        //    || ($('.aEWSCertificate').is(':visible') && ($("#EcoWeakCer").val() != 1))) {
        //    $("#EWSCertificateYes-Required").show();
        //    $("#ApplicableReservations").focus();
        //    IsValid = false;
        //}



        var EDocAppIdVal = $("#EDocAppId").val();
        var CDocAppIdVal = $("#CDocAppId").val();
        var RDocAppIdVal = $("#RDocAppId").val();
        var IDocAppIdVal = $("#IDocAppId").val();
        var UDocAppIdVal = $("#UDocAppId").val();
        var RUDocAppIdVal = $("#RUDocAppId").val();
        var KDocAppIdVal = $("#KDocAppId").val();
        var DDocAppIdVal = $("#DDocAppId").val();
        var ExDocAppIdVal = $("#ExDocAppId").val();
        var HDocAppIdVal = $("#HDocAppId").val();
        var HGKDocAppIdVal = $("#HGKDocAppId").val();
        var ODocAppIdVal = $("#ODocAppId").val();
        var KMDocAppIdVal = $("#KMDocAppId").val();
        var ExSDocAppIdVal = $("#ExSDocAppId").val();
        var EWSDocAppIdVal = $("#EWSDocAppId").val();

        var ECreatedByVal = $("#ECreatedBy").val();
        var CCreatedByVal = $("#CCreatedBy").val();
        var RCreatedByVal = $("#RCreatedBy").val();
        var ICreatedByVal = $("#ICreatedBy").val();
        var UCreatedByVal = $("#UCreatedBy").val();
        var RUCreatedByVal = $("#RUCreatedBy").val();
        var KCreatedByVal = $("#KCreatedBy").val();
        var DCreatedByVal = $("#DCreatedBy").val();
        var ExCreatedByVal = $("#ExCreatedBy").val();
        var HCreatedByVal = $("#HCreatedBy").val();
        var HGKCreatedByVal = $("#HGKCreatedBy").val();
        var OCreatedByVal = $("#OCreatedBy").val();
        var KMCreatedByVal = $("#KMCreatedBy").val();
        var ExSCreatedByVal = $("#ExSCreatedBy").val();
        var EWSCreatedByVal = $("#EWSCreatedBy").val();

        fileData.append(
            "EDocAppId", EDocAppIdVal
        );
        fileData.append(
            "CDocAppId", CDocAppIdVal
        );
        fileData.append(
            "RDocAppId", RDocAppIdVal
        );
        fileData.append(
            "IDocAppId", IDocAppIdVal
        );
        fileData.append(
            "UDocAppId", UDocAppIdVal
        );
        fileData.append(
            "RUDocAppId", RUDocAppIdVal
        );
        fileData.append(
            "KDocAppId", KDocAppIdVal
        );
        fileData.append(
            "DDocAppId", DDocAppIdVal
        );
        fileData.append(
            "ExDocAppId", ExDocAppIdVal
        );
        fileData.append(
            "HDocAppId", HDocAppIdVal
        );
        fileData.append(
            "HGKDocAppId", HGKDocAppIdVal
        );
        fileData.append(
            "ODocAppId", ODocAppIdVal
        );
        fileData.append(
            "KMDocAppId", KMDocAppIdVal
        );
        fileData.append(
            "ExSDocAppId", ExSDocAppIdVal
        );
        fileData.append(
            "EWSDocAppId", EWSDocAppIdVal
        );
        fileData.append(
            "UploadedByVerfication", 0
        );
        fileData.append(
            "ECreatedBy", ECreatedByVal
        );
        fileData.append(
            "CCreatedBy", CCreatedByVal
        );
        fileData.append(
            "RCreatedBy", RCreatedByVal
        );
        fileData.append(
            "ICreatedBy", ICreatedByVal
        );
        fileData.append(
            "UCreatedBy", UCreatedByVal
        );
        fileData.append(
            "RUCreatedBy", RUCreatedByVal
        );
        fileData.append(
            "KCreatedBy", KCreatedByVal
        );
        fileData.append(
            "DCreatedBy", DCreatedByVal
        );
        fileData.append(
            "ExCreatedBy", ExCreatedByVal
        );
        fileData.append(
            "HCreatedBy", HCreatedByVal
        );
        fileData.append(
            "HGKCreatedBy", HGKCreatedByVal
        );
        fileData.append(
            "OCreatedBy", OCreatedByVal
        );
        fileData.append(
            "KMCreatedBy", KMCreatedByVal
        );
        fileData.append(
            "ExSCreatedBy", ExSCreatedByVal
        );
        fileData.append(
            "EWSCreatedBy", EWSCreatedByVal
        );

        fileData.append(
            "DocumentLength", fileData.append.length
        );

        fileData.append(
            "ApplicantId", ApplicantId
        );

        if (IsValid) {
            $.ajax({
                type: "POST",
                url: "/Admission/ApplicantDocumentDetails",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (data) {
                    if (data) {

                        GetMasterData();
                        alertmsg = "<br><br>Application required documents Added to the List";
                        bootbox.alert({
                            message: alertmsg,
                            callback: function () {
                                //location.reload(true);
                            }
                        });
                    } else {
                        alert("Cannot add to list !");
                    }
                }
            });
        }
        else {
            //bootbox.alert("<br><br>Error in your data");
        }
    }
}

function SaveInstitutePreferenceDetails() {
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();

    if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null) {
        bootbox.alert("<br><br>Kindly Give the Applicant,Education, Address, Document Verification Center Information and Save the Details");
    }
    else {
        var fileData = new FormData();
        var IsValid = true;
        var ApplicantId = $("#ApplicantId").val();

        var ParticipateNextRound = $('input[name=ParticipateNextRound]:checked').val();
        if (ParticipateNextRound == 1)
            ParticipateNextRound = true;
        else
            ParticipateNextRound = false;


        $("#Table-Trade-update tbody tr").each(function () {

            var $tr = $(this);
           // var Preference = $tr.find(".Preference-multi-select").val();
            var District = $tr.find(".District-multi-select").val();
            var Taluka = $tr.find(".Taluk-multi-select").val();
            var Institute = $tr.find(".Institute-multi-select").val();
            var Trade = $tr.find(".Trade-multi-select").val();

            //if (Preference == "" || Preference == null || Preference == 0) {
            //    $tr.find(".select-Preference-multi-select-required").show();
            //    IsValid = false;
            //}
            //else {
            //    $tr.find(".select-Preference-multi-select-required").hide();
            //    IsValid = true;
            //}

            if (District == "" || District == null || District == 0) {
                $tr.find(".select-District-multi-select-required").show();
                IsValid = false;
            }
            else {
                $tr.find(".select-District-multi-select-required").hide();
            }

            if (Taluka == "" || Taluka == null || Taluka == 0) {
                $tr.find(".select-Taluk-multi-select-required").show();
                IsValid = false;
            }
            else {
                $tr.find(".select-Taluk-multi-select-required").hide();
            }

            if (Institute == "" || Institute == null || Institute == 0) {
                $tr.find(".select-Institute-multi-select-required").show();
                IsValid = false;
            }
            else {
                $tr.find(".select-Institute-multi-select-required").hide();
            }

            if (Trade == "" || Trade == null || Trade == 0) {
                $tr.find(".select-Trade-multi-select-required").show();
                IsValid = false;
            }
            else {
                $tr.find(".select-Trade-multi-select-required").hide();
            }
        });

        if (IsValid == true) {
            var InstituteCnt = 0;
            $("#Table-Trade-update tbody tr").each(function () {
                var $tr = $(this);
                var Preference = $tr.find(".Preference-multi-select").val();
                var District = $tr.find(".District-multi-select").val();
                var Taluk = $tr.find(".Taluk-multi-select").val();
                var Institute = $tr.find(".Institute-multi-select").val();
                var Trade = $tr.find(".Trade-multi-select").val();
                var TradeIDValue = $tr.find(".update-trade_iti_id").val();

                fileData.append(
                    "PreferenceDetId", (InstituteCnt + 1)
                );
                fileData.append(
                    "PreferenceDetType", Preference
                );
                fileData.append(
                    "DistrictDetId", District
                );
                fileData.append(
                    "TalukaDetId", Taluk
                );
                fileData.append(
                    "InstituteDetId", Institute
                );
                fileData.append(
                    "TradeDetId", Trade
                );
                fileData.append(
                    "InstitutePreferenceId", TradeIDValue
                );
                InstituteCnt++;
            });

            fileData.append(
                "ApplicationId", ApplicantId
            );

            fileData.append(
                "ParticipateNextRound", ParticipateNextRound
            );

            $.ajax({
                type: 'POST',
                url: '/Admission/SaveInstituePreferenceDetails',
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result) {
                    if (result != "" || result != null) {
                        //$("#Table-Trade-update tbody").empty();
                        GetMasterData();
                        bootbox.alert("<br><br>You have selected " + InstituteCnt + " Institute and Trade Preference Details and saved successfully");
                        $("#FinalSubmit").show();
                        $("#divSaveUpdate").hide();
                    }
                    else {
                        bootbox.alert("<br><br>There is Error in Institute Preference Type Data");
                        $("#FinalSubmit").hide();
                        $("#divSaveUpdate").hide();
                    }
                }
            });
        }
    }
}

function daterangeformate2(datetime, submitFrom) {
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
        if (submitFrom == 1)
            openingsd = StartDateOpeningdate + "-" + statedatemonth1 + "-" + StartDateOpeningyear + "-" + exacttimetoshow;
        else
            openingsd = StartDateOpeningdate + "-" + statedatemonth1 + "-" + StartDateOpeningyear;

        return openingsd;
    }
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
//======dhanraj Tab 2 applicant status=====

function GetApplicantsStatus() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantsStatus",
        contentType: "application/json",
        success: function (data) {
            $('#ApplicantStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                "bPaginate": false,
                "bInfo": false,
                searching: false,
                columns: [
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    {
                        'data': 'SubmitDate', 'title': 'Applicant Submitted Date', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.SubmitDate, 1);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                    { 'data': 'OfficerName', 'title': 'Verification Officer', 'className': 'text-left' },
                    //{
                    //    'title': 'Admission Fee Payment Receipt',
                    //    render: function (data, type, row) {
                    //        return "<a class='btn btn-link' href='/PaymentPDFGeneration/GeneratePaymentReceiptPDF?ApplicationId=" + row.ApplicationId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                    //    }
                    //},
                    {
                        'title': 'Applicant Acknowledgement',
                        render: function (data, type, row) {
                            return "<a class='btn btn-link' href='/PaymentPDFGeneration/GenerateAdmissionAcknowledgementPDF?ApplicationId=" + row.ApplicationId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        }
                    },
                    {
                        'data': 'ApplicationId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetCommentDetails(" + oData.ApplicationId + ")' class='btn btn-primary' value='View' id='view'/>");

                        }
                    },
                    {
                        'data': 'ApplicationId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            if (oData.ApplStatus == 10 || oData.ApplStatus == 11 || oData.ApplStatus == 12 || oData.ApplStatus == 13) {
                                $(nTd).html("<input type='button' onclick='GetDocumentWiseDetailByVO()' class='btn btn-primary btn-xs' value='View' id='view'/>  <input type='button' onclick='GetEstablishViewofAppliant()' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");
                            }
                            else if ((oData.AssignedVO == 0 || oData.AssignedVO == "" || oData.AssignedVO == null) && (oData.DocumentFeeReceiptDetails == "" || oData.DocumentFeeReceiptDetails == null)) {
                                $(nTd).html("<input type='button' onclick='GetEstablishViewofAppliant()' class='btn btn-primary' value='Edit' id='edit'/>");
                            }
                            else if ((oData.AssignedVO == 0 || oData.AssignedVO == "" || oData.AssignedVO == null) && oData.DocumentFeeReceiptDetails != "") {
                                $(nTd).html("<input type='button' onclick='GetEstablishViewofAppliant()' class='btn btn-primary' value='View' id='view'/>");
                            }
                            else if ((oData.AssignedVO != 0 && oData.AssignedVO != "" && oData.AssignedVO != null) && oData.ReVerficationStatus == 1) {
                                $(nTd).html("<input type='button' onclick='GetDocumentWiseDetailByVO()' class='btn btn-primary btn-xs' value='View' id='view'/>  <input type='button' onclick='GetEstablishViewofAppliant()' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");
                            }
                            else if ((oData.AssignedVO != 0 && oData.AssignedVO != "" && oData.AssignedVO != null) && (oData.ReVerficationStatus == 0 || oData.ReVerficationStatus == "" || oData.ReVerficationStatus == null)) {
                                $(nTd).html("<input type='button' onclick='GetEstablishViewofAppliant()' class='btn btn-primary' value='View' id='view'/>");
                            }
                        }
                    },
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

//======end applicant status=====

//Function to call on final submit
function SaveUpdates() {
    ;
    var IsValid = true;
    $.ajax({
        type: 'Get',
        url: '/Admission/GetDocumentIndicatorOptionData',
        data: { ApplicationIdFromUI: $("#ApplicantId").val() },
        contentType: "application/json",
        success: function (datajsonDocInd) {
            ;
            if (datajsonDocInd != null && datajsonDocInd != "") {

                //V3 Review validation
                $("#DifferentlyabledcertificateYes-Required").hide();
                if ((($("#Differentlyabledcertificate").val() != "") && (!datajsonDocInd.Resultlist.PhysicallyHanidcapInd))
                    || ($('.aDifferentlyabledcertificate').is(':visible') && (!datajsonDocInd.Resultlist.PhysicallyHanidcapInd))) {
                    $("#DifferentlyabledcertificateYes-Required").show();
                    $("#PhysicallyHanidcapYes").focus();
                    IsValid = false;
                }

                //$("#HoranaaduKannadigaYes-Required").hide();
                //if ((($("#HoranaaduKannadiga").val() != "") && (!datajsonDocInd.Resultlist.HoraNadu_GadiNadu_Kannidagas))
                //    || ($('.aHoranaaduKannadiga').is(':visible') && (!datajsonDocInd.Resultlist.HoraNadu_GadiNadu_Kannidagas))) {
                //    $("#HoranaaduKannadigaYes-Required").show();
                //    $("#HoraNaduNo").focus();
                //    IsValid = false;
                //}

                $("#ExemptedFromStudyCertificateYes-Required").hide();
                if ((($("#ExemptedCertificate").val() != "") && (!datajsonDocInd.Resultlist.ExemptedFromStudyCertificate))
                    || ($('.aExemptedCertificate').is(':visible') && (!datajsonDocInd.Resultlist.ExemptedFromStudyCertificate))) {
                    $("#ExemptedFromStudyCertificateYes-Required").show();
                    $("#ExemptedFromStudyCertificateYes").focus();
                    IsValid = false;
                }

                $("#HyderabadKarnatakaRegionYes-Required").hide();
                if ((($("#HyderabadKarnatakaRegion").val() != "") && (!datajsonDocInd.Resultlist.HyderabadKarnatakaRegion))
                    || ($('.aHyderabadKarnatakaRegion').is(':visible') && (!datajsonDocInd.Resultlist.HyderabadKarnatakaRegion))) {
                    $("#HyderabadKarnatakaRegionYes-Required").show();
                    $("#HyderabadKarnatakaRegionYes").focus();
                    IsValid = false;
                }
                $("#KannadamediumCertificateYes-Required").hide();
                if ((($("#KannadamediumCertificate").val() != "") && (!datajsonDocInd.Resultlist.KanndaMedium))
                    || ($('.aKannadamediumCertificate').is(':visible') && (!datajsonDocInd.Resultlist.KanndaMedium))) {
                    $("#KannadamediumCertificateYes-Required").show();
                    $("#KanndaMediumYes").focus();
                    IsValid = false;
                }

                $("#RuralcertificateYes-Required").hide();
                if ((($("#Ruralcertificate").val() != "") && (datajsonDocInd.Resultlist.ApplicantBelongTo != 1))
                    || ($('.aRuralcertificate').is(':visible') && (datajsonDocInd.Resultlist.ApplicantBelongTo != 1))) {
                    $("#RuralcertificateYes-Required").show();
                    $(".RuralUrbanID").focus();
                    IsValid = false;
                }

                $("#ExservicemanYes-Required").hide();
                if ((($("#Exserviceman").val() != "") && (datajsonDocInd.Resultlist.ExServiceMan != 1))
                    || ($('.aExserviceman').is(':visible') && (datajsonDocInd.Resultlist.ExServiceMan != 1))) {
                    $("#ExservicemanYes-Required").show();
                    $("#ApplicableReservations").focus();
                    IsValid = false;
                }

                $("#EWSCertificateYes-Required").hide();
                if ((($("#EWSCertificate").val() != "") && (datajsonDocInd.Resultlist.EconomyWeakerSection != 1))
                    || ($('.aEWSCertificate').is(':visible') && (datajsonDocInd.Resultlist.EconomyWeakerSection != 1))) {
                    $("#EWSCertificateYes-Required").show();
                    $("#ApplicableReservations").focus();
                    IsValid = false;
                }

                if (IsValid) {
                    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
                    if (PaymentOptionval == 0 || PaymentOptionval == undefined) {
                        bootbox.confirm({
                            message: "<br><br>Are you sure, you want to submit the application ?",
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
                        //bootbox.confirm("<br><br>Are you sure to submit the given data to the verification officer to verify the details?", function (confirmed) {
                        //    if (confirmed) {
                        //        ToSuccessfulSaveYesDataSave();
                        //    }
                        //});
                    }
                    else {
                        bootbox.alert("<br><br>As of now we are not providing online option for the payment details!!");
                    }
                }
                else {
                    bootbox.alert("<br><br>There is error in your documents attached and relavant indicator selection opion, kindly save the indicator data in each panel");
                }
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });    
}

function ToSuccessfulSaveYesDataSave() {

    var GenerateApplicationNumberObj = {
        ApplicantId: $("#ApplicantId").val(),
        ApplicantNumber: $("#ApplicantNumber").val(),
        ApplyYear: $("#AcademicYear").val(),
        ApplicantType: $("#ApplicantType :selected").val(),
        DistrictId: $("#Districts :selected").val(),
        ApplRemarks: $("#txtApplRemarks").val(),
        FlowId: $("#ApplFlowId").val(),
        CredatedBy: $("#ApplCredatedBy").val(),
        ApplicantName: $("#txtApplicantName").val(),
        PhoneNumber: $("#txtApplicantPhoneNumber").val()
    }
    //var institute = $("#InstituteList :selected").text(); 
    //var appNumber = $("#ApplicantNumber").val();
    $.ajax({
        type: 'POST',
        url: '/Admission/GenerateApplicationNumber',
        data: GenerateApplicationNumberObj,
        success: function (result) {
            if (result != null) {
                GetMasterData();
                GetApplicantsStatus();
                //if ($("#txtDocumentFeeReceiptDetails").val() == "" || $("#txtDocumentFeeReceiptDetails").val() == null || $("#txtDocumentFeeReceiptDetails").val() == undefined)
                //    // bootbox.alert("<br><br>" + result.UpdateMsg + " , but your document verification fee is pending. It has to be paid at <b>" + institute + "</b> before due date. Once you paid that document verification fee, you have to enter the receipt details in the application form then only application will be considered as submitted successfully.");
                //    bootbox.alert("<br><br> Your Application Number: " + appNumber + "has been Generated. Your Application submitted successfully, Applicant should retain the same mobile number and email till the examination should be replaced with Your Application Number: " + appNumber +" has been Generated. Your Application submitted successfully, Applicant should retain the same mobile number and email till the completion of AITT examination. ( in the Popup on submit)")
                //    else
                //    //bootbox.alert("<br><br>" + result.UpdateMsg + " .Your Application submitted successfully.");
                   bootbox.alert("<br><br>" + result.UpdateMsg + " , Applicant should retain the same mobile number and email till the completion of AITT examination . Please walk to the selected document verification center to verify your submitted document along with document verification fee Rs.50"  );
            }
            else {
                bootbox.alert("<br><br><h3>" + result.UpdateMsg + "<h3>");
            }
        }
    });
}

//Enable disable the online offline payment details
function DocumentPaymentOption(calledFrm) {

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    if (PaymentOptionval == 1) {
        $("#PaymentThroughOffline").hide();
        $("#PaymentThroughOnline").show();
        $("#txtDocumentFeeReceiptDetails").val('');

    } else {
        $("#PaymentThroughOnline").hide();
        $("#PaymentThroughOffline").show();
        if ($("#InstituteList :selected").text() != "Select") {
            if (calledFrm == 1)
                bootbox.alert("<br><br>You have to pay the verification fee at <b>" + $("#InstituteList :selected").text() + " </b> on or before due date otherwise applicant will not be considered");
        }
    }
}

//To save Payment details
function SavePaymentsDetails() {

    var IsValid = true;
    var ApplicantId = $("#ApplicantId").val();
    var fileData = new FormData();

    var DocumentSubmittedID = $("#DocumentSubmittedID").val();
    var PreferenceSubmittedID = $("#PreferenceSubmittedID").val();
    if (ApplicantId == 0 || ApplicantId == "" || ApplicantId == null || DocumentSubmittedID == 0 || PreferenceSubmittedID == 0) {
        bootbox.alert("<br><br>Kindly Give the Documents,Institute Preference Details and Save the Details");
    }
    else {
        var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
        var DocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

        //Document Verification Fee Details
        //Offline
        if (PaymentOptionval != 1) {
            $("#txtDocumentFeeReceiptDetails-Required").hide();
            var ApplicantNumber = $("#ApplicantNumber").val();
            if (DocumentFeeReceiptDetails == "" && (ApplicantNumber != "")) {
                //$("#txtDocumentFeeReceiptDetails-Required").show();
                //IsValid = false;
            }
        }
        else {
            IsValid = false;
            bootbox.alert("<br><br>As of now we are not providing online option for the payment details!!");
        }

        if (IsValid) {

            //Document Fees Details
            fileData.append(
                "ApplicantId", $("#ApplicantId").val()
            );
            fileData.append(
                "PaymentOptionval", PaymentOptionval
            );
            fileData.append(
                "DocumentFeeReceiptDetails", DocumentFeeReceiptDetails
            );

            $.ajax({
                type: "POST",
                url: "/Admission/InsertPaymentDetails",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (data) {
                    bootbox.alert("<br><br>" + data.pref.status);
                    if ($("#ReVerficationStatus").val() == 0 || $("#ReVerficationStatus").val() == null) {
                        $("#EnableDisableSubmit").show();
                        $("#FinalSubmitRemarks").hide();
                        $("#FinalSubmit").show();
                    }
                    GetApplicantsStatus();
                }
            });
        }
    }
}

//To show preview the image
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#previewimage').attr('src', e.target.result).width(50).height(50);
            $('#imgPriview').attr('src', e.target.result);
            /*$('#ModalImagePriview').modal('show');*/
        };

        reader.readAsDataURL(input.files[0]);
    }
}



//To Get the standard type based on the board type
function OnChangeStateBoardType() {

    $("input[name='RStateBoardType']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();

            $("#GetAppliedBasicsForBoardType").hide();
            if (value == 1) {
                $("#GetAppliedBasicsForBoardType").show();
                //$("input[name=RAppBasics][value=2]").prop('checked', true);
            }
            else {
                $("input[name=RAppBasics][value=1]").prop('checked', true);
            }
            OnChangeGetAppliedBasicsForBoardType();
        }
    });
}

//To Get the roll number based on the standard type
function OnChangeGetAppliedBasicsForBoardType() {
   
    $("input[name='RAppBasics']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
            $("#GetRollNumberFor10thStd").hide();
            if (value == 2) {
                $("#GetRollNumberFor10thStd").show();
            }
        }
        else if ($("input[name=RAppBasics]:checked").val() == "1") {
            $("#Ruralcertificate").attr("disabled", true);
            $("#KannadamediumCertificate").attr("disabled", true);
        }
        else {
            $("#Ruralcertificate").attr("disabled", false);
            $("#KannadamediumCertificate").attr("disabled", false);
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

    //if (this.files[0].size > 1000000) {
    //    bootbox.alert("<br><br>File size should be less than 1MB");
    //    $(this).val("");
    //}

    if (this.files[0].size > 204800) {
        bootbox.alert("<br><br>File size should be less than 200kb");
        $(this).val("");
    }

    //if (ext.length < 50) {
    //    bootbox.alert("<br><br>File Name size should be less than 50 Characters");
    //    $(this).val("");
    //}
});

$('#PhotoUpload').change(function () {
    var ext = $(this).val().split('.').pop().toLowerCase();
    if (ext != "") {
        if ($.inArray(ext, ['gif', 'jpeg', 'jpg', 'png']) == -1) {
            bootbox.alert("<br><br>Pls upload valid image/gif, image/jpeg, image/png file");
            $(this).val("");
            $("#previewimage").attr("src", "\Images/ProfilePic.png");

        }
    } else {
        $(this).val("");
        $("#previewimage").attr("src", "\Images/ProfilePic.png");
    }
});

function ValidateEmail(val) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(val)) {
        $("#txtEmailId-Required").show();
        return false;
    } else {
        return true;
    }
}

function getSelectedOptions(sel) {

    $("#DiffAbledCer").val(0);
    $("#KanMedCer").val(0);
    $("#ExServiceCer").val(0);
    $("#LandLoserCer").val(0);
    $("#EcoWeakCer").val(0);
    $("#KMCer").val(0);
    var len = sel.options.length;
    for (var i = 0; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected) {
            if (opt.value == "1")
                $("#DiffAbledCer").val(1);
            else if (opt.value == "2")
                $("#ExServiceCer").val(1);
            else if (opt.value == "3")
                $("#KanMedCer").val(1);
            else if (opt.value == "4")
                $("#LandLoserCer").val(1);
            else if (opt.value == "5")
                $("#EcoWeakCer").val(1);
            else if (opt.value == "6")
                $("#KMCer").val(1);
        }
    }
}

$('.AadhaarNumberText').keyup(function () {
    var foo = $(this).val().split("-").join(""); // remove hyphens
    if (foo.length > 0) {
        foo = foo.match(new RegExp('.{1,4}', 'g')).join("-");
    }
    $(this).val(foo);
});

function PhysicallyHanidcapEna(id) {
    var value = $('input[name=PhysicallyHanidcapInd]:checked').val();
    //var value = $(id).val();
    if (value == 1) {
        $("#PhysicallyHanidcapDiv1").show();
        $("#PhysicallyHanidcapDiv2").show();
    }
    else {
        $("#PhysicallyHanidcapDiv1").hide();
        $("#PhysicallyHanidcapDiv2").hide();
    }
}

function GetEstablishViewofAppliant() {
    
    //$('#myModal').modal('show');
    //GetMasterData();
    //$("#tab_1").show();
    //$("#tab_1").on('click');
    //$("#tab_1").addClass('active');
    //$("#tabs").tabs("option", "active", 1);
    //$("#tab_2").hide();
    //$(".clsStudyInKarnataka").show();
    window.location.reload();
}

function PreferenceDistrictWiseTaluk(row) {

    Districts = $(row).val();
    $(row).closest("tr").find(".PreferenceTaluk").empty();
    $(row).closest("tr").find(".PreferenceTaluk").append('<option value="0">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {

                $(row).closest("tr").find(".PreferenceTaluk").empty();
                $(row).closest("tr").find(".PreferenceTaluk").append('<option value="0">Select</option>');

                $.each(data, function () {
                    $(row).closest("tr").find(".PreferenceTaluk").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function PreferenceTalukWiseInstitue(row) {

    var TalukaVal = ""; var DistrictVal = "";
    TalukaVal = $(row).val();
    DistrictVal = $(row).closest("tr").find(".PreferenceDistrict").val();

    $(row).closest("tr").find(".PreferenceInstitute").empty();
    $(row).closest("tr").find(".PreferenceInstitute").append('<option value="0">Select</option>');
    $(row).closest("tr").find(".PreferenceTrade").empty();
    $(row).closest("tr").find(".PreferenceTrade").append('<option value="0">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetailsByDistrictTaluka',
        data: { District: DistrictVal, Taluka: TalukaVal },
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $(row).closest("tr").find(".PreferenceInstitute").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDistrictWiseTaluka(row) {

    var DistrictVal = "";
    DistrictVal = $(row).closest("tr").find(".District-multi-select").val();
    $(row).closest("tr").find(".Taluk-multi-select").empty();
    $(row).closest("tr").find(".Taluk-multi-select").append($("<option/>").val(0).text("select"));
    $(row).closest("tr").find(".Institute-multi-select").empty();
    $(row).closest("tr").find(".Institute-multi-select").append($("<option/>").val(0).text("select"));
    $(row).closest("tr").find(".Trade-multi-select").empty();
    $(row).closest("tr").find(".Trade-multi-select").append($("<option/>").val(0).text("select"));
    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: DistrictVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $(row).closest("tr").find(".Taluk-multi-select").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function searchDistrictTalukWiseInstitue(row) {
    var DistrictVal = ""; var TalukVal = "";
    DistrictVal = $(row).closest("tr").find(".District-multi-select").val();
    TalukVal = $(row).closest("tr").find(".Taluk-multi-select").val();

    $(row).closest("tr").find(".Institute-multi-select").empty();
    $(row).closest("tr").find(".Institute-multi-select").append($("<option/>").val(0).text("select"));
    $(row).closest("tr").find(".Trade-multi-select").empty();
    $(row).closest("tr").find(".Trade-multi-select").append($("<option/>").val(0).text("select"));
    $.ajax({
        type: 'Get',
        url: '/Admission/GetITICollegeDetailsByDistrictTaluka',
        data: { District: DistrictVal, Taluka: TalukVal },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $(row).closest("tr").find(".Institute-multi-select").append($("<option/>").val(this.iti_college_code).text(this.iti_college_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function TenthBoardStateType() {
    var selectedTenthBoardStateType = $('input[name="TenthBoard"]:checked').val();
    if (selectedTenthBoardStateType == 1) {
        
        $(".TenthCOBSEType").hide();
        //$("#txtEduGrade").val('');
        $("input[name=TenthCOBSEBoard][value=2]").prop('checked', true)
    }
    else {
        $(".TenthCOBSEType").show();
        TenthCOBSEBoardType();
    }
    MarksCGPAType();
}

function MarksCGPAType() {
    var selectedMarksCGPAType = $('input[name="CGPAMarks"]:checked').val();
    $(".OtherBoardsType").hide();
    if ($('input[name="TenthCOBSEBoard"]:checked').val() == 7) {
        $(".OtherBoardsType").show();
    }
    if (selectedMarksCGPAType == 1 && $('.TenthCOBSEType').is(':visible')) {
        $(".Marks").find("*").prop('disabled', true);
        $("#txtCGPA").prop('disabled', false);
        $('.Marks').find('input[type="number"]').val('');
    }
    else {
        $(".Marks").find("*").prop('disabled', false);
        $("#txtCGPA").prop('disabled', true);
        $("#txtCGPA").val("");
    }
    CalculatePercentage(true);
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
    MarksCGPAType();
}

function CheckRollNumberAvailability() {

    $("#txtRollNumber-Required").hide();
    if ($("#txtRollNumber").val().length != 11) {
        $("#txtRollNumber-Required").show();
    }
    else {
        var txtRollNumberVal = $("#txtRollNumber").val();
        var ApplicationId = $("#ApplicantId").val();
        $("#txtRollNumberDuplicate-Required").hide();
        $("#RollNumberDuplicate").text(0);

        var ApplicationId = $("#ApplicantId").val();
        $("#txtRollNumberDuplicate-Required").hide();
        $("#RollNumberDuplicate").text(0);
        $.ajax({
            type: "GET",
            url: "/Admission/CheckNameAvailability",
            data: { strName: txtRollNumberVal, ApplicationId: ApplicationId, AadhaarRollNumber: 1 },
            success: function (data) {
                if (data == 1) {
                    $("#txtRollNumberDuplicate-Required").show();
                    bootbox.alert("<br><br>RollNumber/Registration Number Already Exist*");
                    $("#RollNumberDuplicate").text(1);
                }
            }
        });
    }
}

function SaveInstitueDetails() {

    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    var InstituteDistricts = "";
    $("#InstituteDistricts-Required").hide();
    InstituteDistricts = $("#InstituteDistricts").val();
    if (InstituteDistricts == "" || InstituteDistricts == "Select") {
        $("#InstituteDistricts-Required").show();
        IsValid = false;
    }

    //Institute    
    $("#InstituteList-Required").hide();
    var InstituteList = $("#InstituteList :selected").val();
    if (InstituteList == "" || InstituteList == "Select" || InstituteList == undefined) {
        $("#InstituteList-Required").show();
        IsValid = false;
    }

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    var txtDocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();


    if (IsValid) {

        //Document Verification Institute
        //fileData.append(
        //    "DocVeriInstituteFilter", searchInstitueOption
        //);

        fileData.append(
            "DocVeriInstituteDistrict", InstituteDistricts
        );

        fileData.append(
            "DocVerificationCentre", InstituteList
        );

        fileData.append(
            "ApplicationId", ApplicantId
        );

        fileData.append(
            "CredatedBy", CredatedBy
        );

        fileData.append(
            "PaymentOptionval", PaymentOptionval
        );

        fileData.append(
            "DocumentFeeReceiptDetails", txtDocumentFeeReceiptDetails
        );

        $.ajax({
            type: "POST",
            url: "/Admission/SaveInstitueDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    $.each(data.objReturnApplicationForm, function () {
                        GetMasterData();
                    });

                    if (data.pref.status == "Error occured!")
                        bootbox.alert("<br><br>Error in data !");
                    else
                        bootbox.alert("<br><br>Applicant Document Verification Center saved Successfully !");
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function CheckAadhaarNumberAvailability() {

    var txtAadhaarNumber = $("#txtAadhaarNumber").val();
    var AadhaarNumber = txtAadhaarNumber.replace('-', '');
    AadhaarNumber = AadhaarNumber.replace('-', '');
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#AadhaarNumberDuplicate").text(0);

    if (!($.isNumeric(AadhaarNumber))) {
        $("#txtAadhaarNumberDuplicate-Required").show();
        $("#AadhaarNumberDuplicate").text(1);
    }
    else {
        var ApplicationId = $("#ApplicantId").val();

    $.ajax({
        type: "GET",
        url: "/Admission/CheckNameAvailability",
        data: { strName: AadhaarNumber, ApplicationId: ApplicationId, AadhaarRollNumber: 2 },
        success: function (data) {
            if (data == 1) {
                $("#txtAadhaarNumberDuplicate-Required").show();
                    bootbox.alert("<br><br>Aadhaar Number Already Exist/Invalid*");
                $("#AadhaarNumberDuplicate").text(1);
            }
        }
    });
}
}

function GetDocumentWiseDetailByVO() {
    ApplicationId = $("#ApplicantId").val();
    $("#DocEduCerAcceptedImg").hide();
    $("#DocCasCerAcceptedImg").hide();
    $("#DocRatCerAcceptedImg").hide();
    $("#DocIncCerAcceptedImg").hide();
    $("#DocUIDCerAcceptedImg").hide();
    $("#DocRurCerAcceptedImg").hide();
    $("#DocKanMedCerAcceptedImg").hide();
    $("#DocDidAblCerAcceptedImg").hide();
    $("#DocExStuCerAcceptedImg").hide();
    $("#DocHyKarRegAcceptedImg").hide();
    $("#DocHorGadKanAcceptedImg").hide();
    $("#DocOthCerAcceptedImg").hide();
    $("#DocExSerAcceptedImg").hide();
    $("#DocLLCAcceptedImg").hide();
    $("#DocEWSAcceptedImg").hide();

    $(".EduFileAttach1").hide();
    $(".CasteFileAttach1").hide();
    $(".RationFileAttach1").hide();
    $(".IncomeCertificateAttach1").hide();
    $(".UIDFileAttach1").hide();
    $(".RuralcertificateAttach1").hide();
    $(".KannadamediumCertificateAttach1").hide();
    $(".DifferentlyabledcertificateAttach1").hide();
    $(".ExemptedCertificateAttach1").hide();
    $(".HyderabadKarnatakaRegionAttach1").hide();
    $(".HoranaaduKannadigaAttach1").hide();
    //$(".OtherCertificatesAttach1").hide();
    $(".ExservicemanAttach1").hide();
    $(".EWSCertificateAttach1").hide();

    $('#myModal').modal('show');
    $.ajax({
        type: 'Get',
        data: { CredatedBy: ApplicationId },
        url: '/Admission/GetApplicationDetailsById',
        success: function (datajson) {

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

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
                        $("#ExserDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#LLCerDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#EWSDocStatus").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    });
                }

                $("#txtRemarks").val(datajson.Resultlist.ApplRemarks);

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {

                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach1").show();
                                $('#aEduCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks").val(this.Remarks);
                                $("#EduCertificate1").prop('disabled', true);
                                $("#EduDocStatus").prop('disabled', true);
                                $("#txtEduCertRemarks").prop('disabled', true);
                            }
                            $("#EduDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocEduCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocEduCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach1").show();
                                $('#aCasteCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks").val(this.Remarks);
                                $("#CasteCertificate1").prop('disabled', true);
                                $("#CasDocStatus").prop('disabled', true);
                                $("#txtCasteCertRemarks").prop('disabled', true);
                            }
                            $("#CasDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocCasCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocCasCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach1").show();
                                $('#aRationCard1').attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks").val(this.Remarks);
                                $("#Rationcard1").prop('disabled', true);
                                $("#RationDocStatus").prop('disabled', true);
                                $("#txtRationCardRemarks").prop('disabled', true);
                            }
                            $("#RationDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocRatCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocRatCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach1").show();
                                $('#aIncomeCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks").val(this.Remarks);
                                $("#Incomecertificate1").prop('disabled', true);
                                $("#IncCerDocStatus").prop('disabled', true);
                                $("#txtIncCertRemarks").prop('disabled', true);
                            }
                            $("#IncCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocIncCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocIncCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach1").show();
                                $('#aUIDNumber1').attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks").val(this.Remarks);
                                $("#UIDNumber1").prop('disabled', true);
                                $("#UIDDocStatus").prop('disabled', true);
                                $("#txtUIDRemarks").prop('disabled', true);
                            }
                            $("#UIDDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocUIDCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocUIDCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 6) {
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach1").show();
                                $('#aRuralcertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks").val(this.Remarks);
                                $("#Ruralcertificate1").prop('disabled', true);
                                $("#RcerDocStatus").prop('disabled', true);
                                $("#txtRurCertRemarks").prop('disabled', true);
                            }
                            $("#RcerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocRurCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocRurCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 7) {
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach1").show();
                                $('#aKannadamediumCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks").val(this.Remarks);
                                $("#KannadamediumCertificate1").prop('disabled', true);
                                $("#KanMedCerDocStatus").prop('disabled', true);
                                $("#txtKanMedRemarks").prop('disabled', true);
                            }
                            $("#KanMedCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocKanMedCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocKanMedCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach1").show();
                                $('#aDifferentlyabledcertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks").val(this.Remarks);
                                $("#Differentlyabledcertificate1").prop('disabled', true);
                                $("#DiffAblDocStatus").prop('disabled', true);
                                $("#txtDiffAbledCertRemarks").prop('disabled', true);
                            }
                            $("#DiffAblDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocDidAblCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocDidAblCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 9) {
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach1").show();
                                $('#aExemptedCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks").val(this.Remarks);
                                $("#ExemptedCertificate1").prop('disabled', true);
                                $("#ExCerDocStatus").prop('disabled', true);
                                $("#txtExeCertRemarks").prop('disabled', true);
                            }
                            $("#ExCerDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocExStuCerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocExStuCerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 10) {
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach1").show();
                                $('#aHyderabadKarnatakaRegion1').attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks").val(this.Remarks);
                                $("#HyderabadKarnatakaRegion1").prop('disabled', true);
                                $("#HyKarDocStatus").prop('disabled', true);
                                $("#txtHydKarnRemarks").prop('disabled', true);
                            }
                            $("#HyKarDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocHyKarRegAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocHyKarRegRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 11) {
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach1").show();
                                $('#aHoranaaduKannadiga1').attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks").val(this.Remarks);
                                $("#HoranaaduKannadiga1").prop('disabled', true);
                                $("#HorKanDocStatus").prop('disabled', true);
                                $("#txtHorGadKannadigaRemarks").prop('disabled', true);
                            }
                            $("#HorKanDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocHorGadKanAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocHorGadKanRejectedImg").show();
                        }
                        //else if (this.DocumentTypeId == 12) {
                        //    if (this.FilePath != null) {
                        //        $(".OtherCertificatesAttach1").show();
                        //        $('#aOtherCertificates1').attr('href', '' + this.FilePath + '');
                        //        $("#txtOtherCertRemarks").val(this.Remarks);
                        //        $("#OtherCertificates1").prop('disabled', true);
                        //        $("#OtherCerDocStatus").prop('disabled', true);
                        //        $("#txtOtherCertRemarks").prop('disabled', true);
                        //    }
                        //    $("#OtherCerDocStatus").val(this.Verified);
                        //    if (this.Verified == 15)
                        //        $("#DocOthCerAcceptedImg").show();
                        //    else if (this.Verified == 3)
                        //        $("#DocOthCerRejectedImg").show();
                        //}
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach1").show();
                                $('#aExserviceman1').attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks").val(this.Remarks);
                                $("#Exserviceman1").prop('disabled', true);
                                $("#ExserDocStatus").prop('disabled', true);
                                $("#txtExservicemanRemarks").prop('disabled', true);
                            }
                            $("#ExserDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocExSerAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocExSerRejectedImg").show();
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach1").show();
                                $('#aEWSCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks").val(this.Remarks);
                                $("#EWSCertificate1").prop('disabled', true);
                                $("#EWSDocStatus").prop('disabled', true);
                                $("#txtEWSCertificateRemarks").prop('disabled', true);
                            }
                            $("#EWSDocStatus").val(this.Verified);
                            if (this.Verified == 15)
                                $("#DocEWSAcceptedImg").show();
                            else if (this.Verified == 3)
                                $("#DocEWSRejectedImg").show();
                        }
                    });
                }
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetCommentDetails(SeatAllocationId) {

    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: "Post",
        url: "/Admission/GetCommentDetailsApplicant",
        data: { SeatAllocationId: SeatAllocationId },
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
function ReshufflePreference() {

    var ParticipateNextRound = $('input[name=ParticipateNextRound]:checked').val();
    $("#SaveInstitutePreferenceDet").hide();
    if (ParticipateNextRound == 1) {
        $("#SaveInstitutePreferenceDet").show();
    }
}

function OnreligionMinority() {
    
    if (($("#Religion :selected").val()) == 2) {
        /* $("#MinorityCategory").attr("disabled", true);*/
        var MinorityCategory = $("#MinorityCategory :selected").text();
        $("#MinorityCategory").val(2);
        $("#txtCaste").attr("disabled", false);
        $("#txtCaste-Required").attr("disabled", true);
    }
    else {
        var MinorityCategory = $("#MinorityCategory :selected").text();
        $("#MinorityCategory").val(1);
        $("#txtCaste").val(0);
        $("#txtCaste").attr("disabled", true);
        $("#txtCaste-Required").attr("disabled", false);
    }
}
$("#KanndaMediumYes").click(function () {
    $("#ExemptedCertificate").attr("disabled", true);
});
$("#KanndaMediumNo").click(function () {
    $("#ExemptedCertificate").attr("disabled", false);
});
//$(".RuralUrbanID").click(function () {
//    if ($("input[name=RuralUrban]:checked").val() == "1") {
//        $("#ExemptedCertificate").attr("disabled", true);
//        bootbox.alert("Please upload Relavent documents in documents section");
//    }

//    else {
//        $("#ExemptedCertificate").attr("disabled", false);
//    }
       
//});
function oncategorycanges() {
    if (($("#Category :selected").val()) == 1) {
        $('input[name=EconomicallyWeakerSections]').attr("disabled", false);
    }
    else {
        /*$("input[name=EconomicallyWeakerSections][value=0]").prop('checked', true);*/
        $('input[name=EconomicallyWeakerSections]').attr("disabled", true);
    }
}
//$("#EwsYes").click(function () {
//    //$("#Incomecertificate").attr("disabled", true);
//    $("#Incomecertificate-Required").show();
//});

function GetDistrictList() {



    $("#PreferenceTalukId").empty();
    $("#PreferenceTalukId").append('<option value="0">Select</option>');

    $("#PreferenceInstituteId").empty();
    $("#PreferenceInstituteId").append('<option value="0">Select</option>');

    $("#PreferenceTradeId").empty();
    $("#PreferenceTradeId").append('<option value="0">Select</option>');

    //$("#PreferenceTypeId").empty();
    //$("#PreferenceTypeId").append('<option value="0">Select</option>');
    //$("#PreferenceTypeId").append('<option value="1">8th</option>');
    //$("#PreferenceTypeId").append('<option value="2">10th</option>');

    $("#PreferenceInstituteTypeId").empty();
    $("#PreferenceInstituteTypeId").append('<option value="0">Select</option>');
    $("#PreferenceInstituteTypeId").append('<option value="1">RURAL</option>');
    $("#PreferenceInstituteTypeId").append('<option value="2">URBAN</option>');

  


    $.ajax({
        type: 'Get',
        url: '/Admission/GetMasterDistrictList',
        success: function (data) {
            if (data != null || data != '') {
              

                $("#PreferenceDistrictId").empty();
                $("#PreferenceDistrictId").append('<option value="0">Select</option>');
                $("#Districts").empty();
                $("#Districts").append('<option value="0">Select</option>');
                $("#PermanentDistricts").empty();
                $("#PermanentDistricts").append('<option value="0">Select</option>');

                $('#Talukas').empty()
                $('#PermanentTalukas').empty()

                $.each(data, function ()
                {
                    $("#PreferenceDistrictId").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    $("#Districts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    $("#PermanentDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                });
            }

            for (var i = $('#Table-Trade-update tr').length; i <= (maxInstPref / 2); i++) {
                fnAddNewRow();
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetMasterData() {
    debugger;
    //$("input[name=RStateBoardType][value=1]").prop('checked', true);
    GetDistrictList();
    //GetInstitutePreferenceList();
    RuralUrbanLocation();
    AppliedWhichBasics();
    AppliedForSyallbus();
    //GetQualificationList();
    //searchOptionInstitue();
    $("#Category").empty();
    $("#Category").append('<option value="0">Select Category</option>');
    $("#Category").val(0);

    $("#Religion").empty();
    $("#Religion").append('<option value="0">Select Religion</option>');
    $("#Religion").val(0);

    $("#Gender").empty();
    $("#Gender").append('<option value="0">Select Gender</option>');
    $("#Gender").val(0);

    $("#ApplicantType").empty();
    $("#ApplicantType").append('<option value="0">Select Applicant</option>');
    $("#ApplicantType").val(0);

    $("#Qualification").empty();
    $("#Qualification").append('<option value="0">Select Qualification</option>');
    $("#Qualification").val(0);

    $("#Districts").empty();
    $("#Districts").append('<option value="0">Select</option>');
    $("#Districts").val(0);

    $("#txtCaste").empty();
    $("#txtCaste").append('<option value="0">Select Caste</option>');
    $("#txtCaste").val(0);

    $("#PermanentDistricts").empty();
    $("#PermanentDistricts").append('<option value="0">Select</option>');
    $("#PermanentDistricts").val(0);

    $("#OtherBoards").empty();
    $("#OtherBoards").append('<option value="0">Select Board/Institute</option>');

    $("#PhysicallyHanidcapType").empty();
    $("#PhysicallyHanidcapType").append('<option value="0">Select Disability Type</option>');

    var GivenGeneralInfoInd = false;
    var GivenDocumentInfoInd = false;
    var GivenInstutitePrefInfoInd = false;
    var MakeButtonDisable = 0;
    $("#DiffAbledCer").val(0);
    $("#KanMedCer").val(0);
    $("#ExServiceCer").val(0);
    $("#LandLoserCer").val(0);
    $("#EcoWeakCer").val(0);
    $("#KMCer").val(0);
    $("#HoraNaduCer").val(0);
    $("#HydKarCer").val(0);
    $("#ExeStuCer").val(0);
    $("#FinalSubmitRemarks").hide();
    $.ajax({
        type: 'Get',
        url: '/Admission/GetMasterApplicantData',
        success: function (datajson) {
            $("#EduFileAttach").hide(); $("#CasteFileAttach").hide(); $("#RationFileAttach").hide(); $("#IncomeCertificateAttach").hide();
            $("#UIDFileAttach").hide(); $("#RuralcertificateAttach").hide(); $("#KannadamediumCertificateAttach").hide(); $("#DifferentlyabledcertificateAttach").hide();
            $("#ExemptedCertificateAttach").hide(); $("#HyderabadKarnatakaRegionAttach").hide(); $("#HoranaaduKannadigaAttach").hide();/* $("#OtherCertificatesAttach").hide();*/
            $("#KashmirMigrantsAttach").hide(); $("#ExservicemanAttach").hide(); $("#EWSCertificateAttach").hide();
            $("#DocumentSubmittedID").val(0);
            $("#PreferenceSubmittedID").val(0);
            $("input[name=RStateBoardType][value=1]").prop('disabled', false);
           
        
            if ($("input[name='StudyKannadaInOtherSt']:checked").val() == "1") {
                $("input[name=RStateBoardType][value=1]").prop('disabled', true);
                $("input[name=RStateBoardType][value=0]").prop('checked', true);
               /* $("input[name=TenthBoard][value='1']").prop('disabled', true);*/
                $("input[name=TenthBoard][value='4']").prop('checked', true);
            }
            
            if ($("input[name='ParentGovtEE']:checked").val() == "1") {
                $("input[name=RStateBoardType][value=1]").prop('disabled', false);
                $("input[name=RStateBoardType][value=0]").prop('checked', true);
                $("#GetAppliedBasicsForBoardType").hide();

                $("input[name=TenthBoard][value='1']").prop('disabled', false);
                $("input[name=TenthBoard][value='4']").prop('checked', true);
                $(".TenthCOBSEType").show();
            }

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

                if ($("#CalenderNotificationEligibleInd").val() == 1) {
                    $(".EligibleApplicationForm").show();
                    $(".NotEligibleApplicationForm").hide();

                    $("#ApplicantId").val(datajson.Resultlist.ApplicationId);
                    $("#ApplFlowId").val(datajson.Resultlist.FlowId);
                    $("#ApplCredatedBy").val(datajson.Resultlist.CredatedBy);
                    $("#ReVerficationStatus").val(datajson.Resultlist.ReVerficationStatus);
                    if (datajson.Resultlist.ReVerficationStatus == 0 || datajson.Resultlist.ReVerficationStatus == null) {
                        $("#FinalSubmitRemarks").hide();
                        $("#txtApplRemarks").val('');
                    }
                    else {
                        $("#FinalSubmitRemarks").show();
                        $("#txtApplRemarks").val('');
                    }

                    if (datajson.Resultlist.ApplicationId != 0)
                        GivenGeneralInfoInd = true;

                    //Get the roll number
                    if (datajson.Resultlist.RStateBoardType == true) {
                        if ($("input[name='ParentGovtEE']:checked").val() == "0") {
                            $("input[name=RStateBoardType][value=1]").prop('checked', true);
                        }
                       
                    }

                    
                    $("input[name=RAppBasics][value=" + datajson.Resultlist.RAppBasics + "]").prop('checked', true);
                    $("#txtRollNumber").val(datajson.Resultlist.RollNumber);
                    OnChangeStateBoardType();
                    OnChangeGetAppliedBasicsForBoardType();
                    //ApplicantDetails
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

                    if (datajson.Resultlist.GetOtherBoards.length > 0) {
                        $.each(datajson.Resultlist.GetOtherBoards, function (index, item) {
                            $("#OtherBoards").append($("<option/>").val(this.BoardId).text(this.BoardName));
                        });
                    }

                    if (datajson.Resultlist.PersonWithDisabilityCategory.length > 0) {
                        $.each(datajson.Resultlist.PersonWithDisabilityCategory, function (index, item) {
                            $("#PhysicallyHanidcapType").append($("<option/>").val(this.PersonWithDisabilityCategoryId).text(this.DisabilityName));
                        });
                    }

                    //$.each(datajson.Resultlist.GetReservationList, function (index, item) {
                    //    $("#ApplicableReservations").append($("<option/>").val(this.ReservationId).text(this.Reservations));
                    //});

                    //var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;

                    //if (MultiselectSelectedValue != null) {
                    //    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                    //        if (e == "1")
                    //            $("#DiffAbledCer").val(1);
                    //        else if (e == "2")
                    //            $("#ExServiceCer").val(1);
                    //        else if (e == "3")
                    //            $("#KanMedCer").val(1);
                    //        else if (e == "4")
                    //            $("#LandLoserCer").val(1);
                    //        else if (e == "5")
                    //            $("#EcoWeakCer").val(1);
                    //        else if (e == "6")
                    //            $("#KMCer").val(1);

                    //        $("#ApplicableReservations option[value='" + e + "']").prop("selected", true);
                    //    });
                    //}

                 //   $('#ApplicableReservations').multiselect({});

                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $('#Districts').empty();
                        $('#PermanentDistricts').empty();
                        $('#InstituteDistricts').empty();
                        $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                            $("#Districts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                            $("#PermanentDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                            $("#InstituteDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        });
                    }

                    //Documents                                
                    if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                        $("#EduCertificate").prop('disabled', true);
                        $("#CasteCertificate").prop('disabled', true);
                        $("#Rationcard").prop('disabled', true);
                        $("#Incomecertificate").prop('disabled', true);
                        $("#UIDNumber").prop('disabled', true);
                        $("#Ruralcertificate").prop('disabled', true);
                        $("#KannadamediumCertificate").prop('disabled', true);
                        $("#Differentlyabledcertificate").prop('disabled', true);
                        $("#ExemptedCertificate").prop('disabled', true);
                        $("#HyderabadKarnatakaRegion").prop('disabled', true);
                        $("#HoranaaduKannadiga").prop('disabled', true);
                        $("#Exserviceman").prop('disabled', true);
                        $("#EWSCertificate").prop('disabled', true);

                        $("#DocumentSubmittedID").val(datajson.Resultlist.GetApplicantDocumentsDetail.length);
                        GivenDocumentInfoInd = true;
                        $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {

                            if (this.DocumentTypeId == 1) {
                                if (this.FilePath != null) {
                                    $("#EduFileAttach").show();
                                    $('#aEduCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtEduCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#EduCertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified ==3) 
                                    //    $("#EduCertificate").prop('disabled', false);
                                    else
                                        $("#EduCertificate").prop('disabled', false);
                                }
                                $("#EDocAppId").val(this.DocAppId);
                                $("#ECreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 2) {
                                if (this.FilePath != null) {
                                    $("#CasteFileAttach").show();
                                    $('#aCasteCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtCasteCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#CasteCertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#CasteCertificate").prop('disabled', false);
                                    else
                                        $("#CasteCertificate").prop('disabled', false);
                                }
                                $("#CDocAppId").val(this.DocAppId);
                                $("#CCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 3) {
                                if (this.FilePath != null) {
                                    $("#RationFileAttach").show();
                                    $('#aRationCard').attr('href', '' + this.FilePath + '');
                                    $("#txtRationCardRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#Rationcard").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#Rationcard").prop('disabled', false);
                                    else
                                        $("#Rationcard").prop('disabled', false);
                                }
                                $("#RDocAppId").val(this.DocAppId);
                                $("#RCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 4) {
                                if (this.FilePath != null) {
                                    $("#IncomeCertificateAttach").show();
                                    $('#aIncomeCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtIncCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#Incomecertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#Incomecertificate").prop('disabled', false);
                                    else
                                        $("#Incomecertificate").prop('disabled', false);
                                }
                                $("#IDocAppId").val(this.DocAppId);
                                $("#ICreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 5) {
                                if (this.FilePath != null) {
                                    $("#UIDFileAttach").show();
                                    $('#aUIDNumber').attr('href', '' + this.FilePath + '');
                                    $("#txtUIDRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#UIDNumber").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#UIDNumber").prop('disabled', false);
                                    else
                                        $("#UIDNumber").prop('disabled', false);
                                }
                                $("#UDocAppId").val(this.DocAppId);
                                $("#UCreatedBy").val(this.CreatedBy);
                            }

                            else if (this.DocumentTypeId == 6) {
                                if (this.FilePath != null) {
                                    $("#RuralcertificateAttach").show();
                                    $('#aRuralcertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtRurCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#Ruralcertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#Ruralcertificate").prop('disabled', false);
                                    else
                                        $("#Ruralcertificate").prop('disabled', false);
                                }

                                else if ($("input[name=RAppBasics]:checked").val() == "1") {
                                    $("#Ruralcertificate").attr("disabled", true);
                                }
                                $("#RDocAppId").val(this.DocAppId);
                                $("#RCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 7) {
                                if (this.FilePath != null) {
                                    $("#KannadamediumCertificateAttach").show();
                                    $('#aKannadamediumCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtKanMedRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#KannadamediumCertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#KannadamediumCertificate").prop('disabled', false);
                                    else
                                        $("#KannadamediumCertificate").prop('disabled', false);
                                }
                                else if ($("input[name=RAppBasics]:checked").val() == "1") {
                                    $("#KannadamediumCertificate").attr("disabled", true);
                                }
                                $("#KDocAppId").val(this.DocAppId);
                                $("#KCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 8) {
                                if (this.FilePath != null) {
                                    $("#DifferentlyabledcertificateAttach").show();
                                    $('#aDifferentlyabledcertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtDiffAbledCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#Differentlyabledcertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#Differentlyabledcertificate").prop('disabled', false);
                                    else
                                        $("#Differentlyabledcertificate").prop('disabled', false);
                                }
                                $("#DDocAppId").val(this.DocAppId);
                                $("#DCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 9) {
                                if (this.FilePath != null) {
                                    $("#ExemptedCertificateAttach").show();
                                    $('#aExemptedCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtExeCertRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#ExemptedCertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#ExemptedCertificate").prop('disabled', false);
                                    else
                                        $("#ExemptedCertificate").prop('disabled', false);
                                }
                                $("#ExDocAppId").val(this.DocAppId);
                                $("#ExCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 10) {
                                if (this.FilePath != null) {
                                    $("#HyderabadKarnatakaRegionAttach").show();
                                    $('#aHyderabadKarnatakaRegion').attr('href', '' + this.FilePath + '');
                                    $("#txtHydKarnRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#HyderabadKarnatakaRegion").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#HyderabadKarnatakaRegion").prop('disabled', false);
                                    else
                                        $("#HyderabadKarnatakaRegion").prop('disabled', false);
                                }
                                $("#HDocAppId").val(this.DocAppId);
                                $("#HCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 11) {
                                if (this.FilePath != null) {
                                    $("#HoranaaduKannadigaAttach").show();
                                    $('#aHoranaaduKannadiga').attr('href', '' + this.FilePath + '');
                                    $("#txtHorGadKannadigaRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#HoranaaduKannadiga").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#HoranaaduKannadiga").prop('disabled', false);
                                    else
                                        $("#HoranaaduKannadiga").prop('disabled', false);
                                }
                                $("#HGKDocAppId").val(this.DocAppId);
                                $("#HGKCreatedBy").val(this.CreatedBy);
                            }
                            //else if (this.DocumentTypeId == 12) {
                            //    if (this.FilePath != null) {
                            //        $("#OtherCertificatesAttach").show();
                            //        $('#aOtherCertificates').attr('href', '' + this.FilePath + '');
                            //        $("#txtOtherCertRemarks").val(this.Remarks);
                            //        if (this.Verified == 15)
                            //            $("#OtherCertificates").prop('disabled', true);
                            //        else
                            //            $("#OtherCertificates").prop('disabled', false);
                            //    }
                            //    $("#ODocAppId").val(this.DocAppId);
                            //    $("#OCreatedBy").val(this.CreatedBy);
                            //}
                            else if (this.DocumentTypeId == 14) {
                                if (this.FilePath != null) {
                                    $("#ExservicemanAttach").show();
                                    $('#aExserviceman').attr('href', '' + this.FilePath + '');
                                    $("#txtExservicemanRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#Exserviceman").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#Exserviceman").prop('disabled', false);
                                    else
                                        $("#Exserviceman").prop('disabled', false);
                                }
                                $("#ExSDocAppId").val(this.DocAppId);
                                $("#ExSCreatedBy").val(this.CreatedBy);
                            }
                            else if (this.DocumentTypeId == 16) {
                                if (this.FilePath != null) {
                                    $("#EWSCertificateAttach").show();
                                    $('#aEWSCertificate').attr('href', '' + this.FilePath + '');
                                    $("#txtEWSCertificateRemarks").val(this.Remarks);
                                    if (this.Verified == 15)
                                        $("#EWSCertificate").prop('disabled', true);
                                    //else if (this.Verified == 14 || this.Verified == 3) 
                                    //    $("#EWSCertificate").prop('disabled', false);
                                    else
                                        $("#EWSCertificate").prop('disabled', false);
                                }
                                $("#EWSDocAppId").val(this.DocAppId);
                                $("#EWSCreatedBy").val(this.CreatedBy);
                            }
                        });
                    }

                    //$("#academicyear1").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1)); 
                    if (datajson.Resultlist.ApplyMonth == null)
                        datajson.Resultlist.ApplyMonth = 7;
                    if (datajson.Resultlist.ApplyYear == null)
                        datajson.Resultlist.ApplyYear = new Date().getFullYear();
                    $("#AcademicMonths").val(datajson.Resultlist.ApplyMonth);
                    $("#AcademicYear").val(datajson.Resultlist.ApplyYear);
                    $("#academicyear1").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                    $("#txtApplicantName").val(datajson.Resultlist.ApplicantName);
                    $("#txtFathersName").val(datajson.Resultlist.FathersName);
                    $("#ApplicantNumber").val(datajson.Resultlist.ApplicantNumber);

                  /*  $("#txtCaste").val(datajson.Resultlist.Caste);*/

                    $("#txtParentOccupation").val(datajson.Resultlist.ParentsOccupation);

                    if (datajson.Resultlist.Photo != null && datajson.Resultlist.Photo != "") {
                        $('#ImgPhotoUpload').attr("src", datajson.Resultlist.Photo);
                        $('#previewimage').attr("src", datajson.Resultlist.Photo).width('50px').height('50px');
                    }
                    else {
                        $('#ImgPhotoUpload').attr("src", "/Images/ProfilePic.png");
                    }
                        

                    $("#IsUploaded").val(datajson.Resultlist.Photo);

                    var finalDOB = daterangeformate2(datajson.Resultlist.DOB, 0);
                    $("#dateofbirth").val(finalDOB);
                    $("#txtMothersName").val(datajson.Resultlist.MothersName);

                    if (datajson.Resultlist.Religion != null)
                        $("#Religion").val(datajson.Resultlist.Religion);

                    if (datajson.Resultlist.Gender != null)
                        $('#Gender').val(datajson.Resultlist.Gender);

                    if (datajson.Resultlist.Category != null)
                        $("#Category").val(datajson.Resultlist.Category);

                    if (datajson.Resultlist.MinorityCategory != null)
                        $("#MinorityCategory").val(datajson.Resultlist.MinorityCategory);

                    
                    if (datajson.Resultlist.Caste != null) {
                        $("#txtCaste").val(datajson.Resultlist.Caste);
                    }
                         

                    $("#txtFamilyAnnualIncome").val(datajson.Resultlist.FamilyAnnIncome);

                    if (datajson.Resultlist.ApplicantType != null) {
                        $("#ApplicantType").val(datajson.Resultlist.ApplicantType);
                        $("#ApplicantType").attr("disabled", true);
                    }
                        


                    if (datajson.Resultlist.AadhaarNumber == 0)
                        datajson.Resultlist.AadhaarNumber = "";
                    $("#txtAadhaarNumber").val(datajson.Resultlist.AadhaarNumber);

                    if (datajson.Resultlist.RationCard == 0)
                        datajson.Resultlist.RationCard = "";
                    $("#txtRationCard").val(datajson.Resultlist.RationCard);
                    $("#AccountNumber").val(datajson.Resultlist.AccountNumber);
                    $("#BankName").val(datajson.Resultlist.BankName);
                    $("#IFSCCode").val(datajson.Resultlist.IFSCCode);

                    if (datajson.Resultlist.PhysicallyHanidcapInd == true) {
                        $("input[name=PhysicallyHanidcapInd][value=1]").prop('checked', true);
                        $("#DiffAbledCer").val(1);
                    }
                    else
                        $("input[name=PhysicallyHanidcapInd][value=0]").prop('checked', true);
                    PhysicallyHanidcapEna(PhysicallyHanidcapYes);

                    if (datajson.Resultlist.PhysicallyHanidcapType == "" || datajson.Resultlist.PhysicallyHanidcapType == null)
                        $("#PhysicallyHanidcapType").val(0);
                    else
                        $("#PhysicallyHanidcapType").val(datajson.Resultlist.PhysicallyHanidcapType);


                    if (datajson.Resultlist.HoraNadu_GadiNadu_Kannidagas == true) {
                        $("input[name=HoraNadu][value=1]").prop('checked', true);
                        $("#HoraNaduCer").val(1);
                    }
                    else {
                        if ($("input[name=StudyKannadaInOtherSt][value=1]").is(':checked')) {
                            $("input[name=HoraNadu][value=1]").prop('checked', true);
                        }
                        //else {
                        //    $("input[name=HoraNadu][value=0]").prop('checked', true);
                        //}
                    }
                        

                    if (datajson.Resultlist.ExemptedFromStudyCertificate == true) {
                        $("input[name=ExemptedFromStudyCertificate][value=1]").prop('checked', true);
                        $("#ExeStuCer").val(1);
                    }
                    else {
                        if ($("input[name=StudyInKarnataka][value=1]").is(':checked')) {
                            $("input[name=ExemptedFromStudyCertificate][value=1]").prop('checked', true);
                        }
                        else {
                            $("input[name=ExemptedFromStudyCertificate][value=0]").prop('checked', true);
                        }
                    }
                        

                    if (datajson.Resultlist.HyderabadKarnatakaRegion == true) {
                        $("input[name=HyderabadKarnatakaRegion][value=1]").prop('checked', true);
                        $("#HydKarCer").val(1);
                    }
                    //else
                    //    $("input[name=HyderabadKarnatakaRegion][value=0]").prop('checked', true);

                    if (datajson.Resultlist.KanndaMedium == true) {
                        $("input[name=KanndaMedium][value=1]").prop('checked', true);
                        $("#KanMedCer").val(1);
                    }
                    //else
                    //    $("input[name=KanndaMedium][value=0]").prop('checked', true);
                    //Added by sujit
                    if (datajson.Resultlist.ExServiceMan == true) {
                        $("input[name=ExService][value=1]").prop('checked', true);
                        $("#ExServiceCer").val(1);
                    }
                    //else
                    //    $("input[name=ExService][value=0]").prop('checked', true);
                    
                    if (datajson.Resultlist.EconomyWeakerSection == true) {
                        $("input[name=EconomicallyWeakerSections][value=1]").prop('checked', true);
                        $("#EcoWeakCer").val(1);
                    }
                    //else
                    //    $("input[name=EconomicallyWeakerSections][value=0]").prop('checked', true);

                    //EducationDetails
                    if (datajson.Resultlist.GetQualificationList.length > 0) {
                        $.each(datajson.Resultlist.GetQualificationList, function (index, item) {
                            $("#Qualification").append($("<option/>").val(this.QualificationId).text(this.Qualification));
                        });
                    }

                    if (datajson.Resultlist.Qualification != null)
                        $('#Qualification').val(datajson.Resultlist.Qualification);
                    if (datajson.Resultlist.BoardId != null)
                        $("#OtherBoards").val(datajson.Resultlist.BoardId);
                    
                    if (datajson.Resultlist.ApplicantBelongTo!=null) {
                        $("input[name=RuralUrban][value=" + datajson.Resultlist.ApplicantBelongTo + "]").prop('checked', true);
                    }
                    

                    $("input[name=AppBasics][value=" + datajson.Resultlist.AppliedBasic + "]").prop('checked', true);
                    
                    if (datajson.Resultlist.TenthBoard != null) {
                        $("input[name=TenthBoard][value=" + datajson.Resultlist.TenthBoard + "]").prop('checked', true);
                    }
                    //$("#txtEduGrade").val(datajson.Resultlist.EducationGrade);
                    $("input[name=TenthCOBSEBoard][value=" + datajson.Resultlist.TenthCOBSEBoard + "]").prop('checked', true);
                    $("#txtInstituteStudied").val(datajson.Resultlist.InstituteStudiedQual);
                    $("#txtMaximumMarks").val(datajson.Resultlist.MaxMarks);
                    $("#txtCGPA").val(datajson.Resultlist.EducationCGPA);
                    //$("#txtMinMarks").val(datajson.Resultlist.MinMarks);
                    $("#txtMarksObtained").val(datajson.Resultlist.MarksObtained);
                    $("#lblPercAsPerMarks").text(datajson.Resultlist.Percentage + "%");
                    TenthBoardStateType();

                    //if (datajson.Resultlist.studiedMathsScience == true)
                    //    $("input[name=studiedMathsScience][value=1]").prop('checked', true);
                    //else
                    //    $("input[name=studiedMathsScience][value=0]").prop('checked', true);

                    if (datajson.Resultlist.ParticipateNextRound == true)
                        $("input[name=ParticipateNextRound][value=1]").prop('checked', true);
                    else
                        $("input[name=ParticipateNextRound][value=0]").prop('checked', true);

                    if (datajson.Resultlist.ResultQual != null)
                        $("#Results").val(datajson.Resultlist.ResultQual);
                    ;
                    //AddressDetails
                    $("#txtCommunicationAddress").val(datajson.Resultlist.CommunicationAddress);
                    if (datajson.Resultlist.DistrictId != null)
                        $("#Districts").val(datajson.Resultlist.DistrictId);
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        debugger;
                        $.each(datajson.Resultlist.GetDistrictList, function () {
                            $('#Talukas').empty();
                            $.each($(this.TalukListDet), function (index, item) {
                                $("#Talukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                            });
                        });
                    }

                    if (datajson.Resultlist.TalukaId != null) {
                        debugger;
                        GetTalukaOnLoad(datajson.Resultlist.TalukaId);
                        debugger;
                        $('#Talukas').val(datajson.Resultlist.TalukaId);
                    }
                       
                    if (datajson.Resultlist.Pincode == 0)
                        $("#txtPincode").val();
                    else
                        $("#txtPincode").val(datajson.Resultlist.Pincode);
                    $("input[name=chkSameAsCommunicationAddress][value=" + datajson.Resultlist.SameAdd + "]").prop('checked', true);
                    if (datajson.Resultlist.SameAdd == 1 || datajson.Resultlist.SameAdd == true)
                        $("#chkSameAsCommunicationAddress").prop('checked', true);
                    else
                        $("#chkSameAsCommunicationAddress").prop('checked', false);

                    OnchangechkSameAsCommunicationAddress();
                    $("#txtPermanentAddress").val(datajson.Resultlist.PermanentAddress);
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $('#PermanentDistricts').empty();
                        $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                            $("#PermanentDistricts").append($("<option/>").val(item.district_lgd_code).text(item.district_ename));
                        });
                    }
                    $("#PermanentTalukas").empty();
                    $("#PermanentTalukas").append('<option value="0">Select</option>');

                    if (datajson.Resultlist.PDistrict != null)
                        $("#PermanentDistricts").val(datajson.Resultlist.PDistrict);
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $.each(datajson.Resultlist.GetDistrictList, function () {
                            $('#PermanentTalukas').empty();
                            $.each($(this.TalukListDet), function (index, item) {
                                $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                            });
                        });
                    }

                    if (datajson.Resultlist.PTaluk != null)
                        $('#PermanentTalukas').val(datajson.Resultlist.PTaluk);
                    if (datajson.Resultlist.PPinCode == 0)
                        $("#txtPermanentPincode").val();
                    else
                        $("#txtPermanentPincode").val(datajson.Resultlist.PPinCode);
                    $("#txtApplicantPhoneNumber").val(datajson.Resultlist.PhoneNumber);
                    $("#txtFathersPhoneNumber").val(datajson.Resultlist.FatherPhoneNumber);
                    $("#txtEmailId").val(datajson.Resultlist.EmailId);

                    //Document Verification Institutue
                    $("input[name=searchInstitueOption][value=" + datajson.Resultlist.DocVeriInstituteFilter + "]").prop('checked', true);
                    var sessionValue = $("#hdnSession").data('value');
                    debugger;
                    searchOptionInstitue();
                    if (datajson.Resultlist.GetDistrictList.length > 0) {
                        $('#InstituteDistricts').empty();
                        $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                            $("#InstituteDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        });
                    }
                   
                    if (datajson.Resultlist.DocVeriInstituteDistrict != null) {
                        GetInstituteDistrictsOnLoad(datajson.Resultlist.DocVeriInstituteDistrict);
                        $("#InstituteDistricts").val(datajson.Resultlist.DocVeriInstituteDistrict);
                    }
                    if (datajson.Resultlist.GetApplicantDocVerfiInstitutePreference.length > 0) {
                        $("#InstituteList").empty();
                        $.each(datajson.Resultlist.GetApplicantDocVerfiInstitutePreference, function (index, item) {
                            $.each(item.DocVerfiInstDet, function (index, values) {
                                $("#InstituteList").append($("<option/>").val(values.iti_college_code).text(values.iti_college_name));
                            });
                        });
                    }

                    if (datajson.Resultlist.DocVerificationCentre != null)
                    {
                        DocVerfiDistrictWiseInstitueOnLoad(datajson.Resultlist.DocVerificationCentre);
                        $("#InstituteList").val(datajson.Resultlist.DocVerificationCentre);
                    }
                        

                    //Institute Preference Type

                    if (datajson.Resultlist.GetApplicantInstitutePreference.length > 0) {
                        $("#PreferenceSubmittedID").val(datajson.Resultlist.GetApplicantInstitutePreference.length);
                        GivenInstutitePrefInfoInd = true;
                        $("#Table-Trade-update tbody").empty();
                        var showSno = 1;
                        $.each(datajson.Resultlist.GetApplicantInstitutePreference, function (index, item) {

                            var _selectsino = $("<label/>");
                            _selectsino.addClass("form-control SINO-multi-select SINO");

                            var _tr = $("<tr/>");
                            var _td = $("<td/>"); // first td
                            _td.append(_selectsino.text(showSno));
                            _tr.append(_td);

                            //////////////////////////////////
                            //var _selectPreference = $("<select/>");
                            //var _selectPreferenceErr = $("<small/>");
                            //_selectPreferenceErr.text("Select Preference Type");
                            //_selectPreferenceErr.hide();
                            //_selectPreference.addClass("form-control Preference-multi-select PreferenceType");
                            //_selectPreferenceErr.addClass("text-danger select-Preference-multi-select-required");
                            //_selectPreference.append($("<option/>").val("0").text("Select"));

                            //if (datajson.pref.length > 0) {
                            //    $.each(datajson.pref, function (index, values) {
                            //        _selectPreference.append($("<option/>").val(values.QualificationId).text(values.Qualification));
                            //    });
                            //}

                            //_td = $("<td/>"); // first td
                            //if (this.PreferenceType != "")
                            //    _td.append(_selectPreference.val(this.PreferenceType));
                            //else
                            //    _td.append(_selectPreference.val(0));

                            //_td.append(_selectPreferenceErr);
                            //_tr.append(_td);

                            /////////////////////////////////////////// District

                            var _selectDistrict = $("<select/>");
                            var _selectDistrictErr = $("<small/>");
                            _selectDistrictErr.text("Select District");
                            _selectDistrictErr.hide();
                            _selectDistrictErr.addClass("text-danger select-District-multi-select-required");
                            _selectDistrict.addClass("form-control District-multi-select PreferenceDistrict");
                            _selectDistrict.append($("<option/>").val("").text("Select"));

                            _selectDistrict.change(function () {
                                searchDistrictWiseTaluka(this);
                            });

                            if (datajson.dist.length > 0) {
                                $.each(datajson.dist, function (index, values) {
                                    _selectDistrict.append($("<option/>").val(values.district_lgd_code).text(values.district_ename));
                                });
                            }

                            _td = $("<td/>"); //second td
                            _td.append(_selectDistrict.val(this.DistrictId));
                            _td.append(_selectDistrictErr);
                            _tr.append(_td);
                            _td = $("<td/>");

                            /////////////////////////////////////////// Taluka

                            var _selectTaluka = $("<select/>");
                            var _selectTalukaErr = $("<small/>");
                            _selectTalukaErr.text("Select Taluka");
                            _selectTalukaErr.hide();
                            _selectTalukaErr.addClass("text-danger select-Taluk-multi-select-required");
                            _selectTaluka.addClass("form-control Taluk-multi-select PreferenceTaluk");
                            _selectTaluka.append($("<option/>").val("").text("Select"));

                            _selectTaluka.change(function () {
                                searchDistrictWiseInstitue(this);
                            });

                            if (item.TalukDet.length > 0) {
                                $.each(item.TalukDet, function (index, values) {
                                    _selectTaluka.append($("<option/>").val(values.taluk_lgd_code).text(values.taluk_ename));
                                });
                            }

                            _td = $("<td/>"); //second td
                            _td.append(_selectTaluka.val(this.TalukaId));
                            _td.append(_selectTalukaErr);
                            _tr.append(_td);
                            _td = $("<td/>");

                            //////////////////////////////////////// Institute

                            var _selectInstituteErr = $("<small/>");
                            _selectInstituteErr.text("Select Institute Type");
                            _selectInstituteErr.hide();
                            _selectInstituteErr.addClass("text-danger select-Institute-multi-select-required");
                            var _selectInstitute = $("<select/>");
                            _selectInstitute.addClass("form-control Institute-multi-select PreferenceInstitute");
                            _selectInstitute.append($("<option/>").val("").text("Select"));
                            _selectInstitute.change(function () {
                                PreferenceInstitueWiseTrade(this);
                            });

                            if (item.InstituteDet.length > 0) {
                                $.each(item.InstituteDet, function (index, values) {
                                    _selectInstitute.append($("<option/>").val(values.iti_college_code).text(values.iti_college_name));
                                });
                            }

                            _td = $("<td/>"); //third td
                            _td.append(_selectInstitute.val(this.InstituteId));
                            _td.append(_selectInstituteErr);
                            _tr.append(_td);
                            _td = $("<td/>");

                            /////////////////////////////////// Trade

                            var _selectTradeErr = $("<small/>");
                            _selectTradeErr.text("Select Trade");
                            _selectTradeErr.hide();
                            _selectTradeErr.addClass("text-danger select-Trade-multi-select-required");
                            var _selectTrade = $("<select/>");
                            _selectTrade.addClass("form-control Trade-multi-select PreferenceTrade");
                            _selectTrade.append($("<option/>").val("").text("Select"));
                            if (item.TradeDet.length > 0) {
                                $.each(item.TradeDet, function (index, values) {
                                    _selectTrade.append($("<option/>").val(values.trade_id).text(values.trade_name));
                                });
                            }

                            _td = $("<td/>"); //fifth td
                            _td.append(_selectTrade.val(this.TradeId));
                            _td.append(_selectTradeErr);
                            _tr.append(_td);
                            _td = $("<td/>");

                            _btn = "";
                            _btn = $("<input type='hidden' value='" + this.InstitutePreferenceId + "' class='update-trade_iti_id' /><button class='btn btn-danger update-trade-remove'>X</button>");
                            _btn.click(function () {
                                var lenght = $('#Table-Trade-update tbody tr').length;
                                if (lenght > 1) {
                                    $(this).closest("tr").remove();
                                }
                                else {
                                    bootbox.alert("<br><br>Atleast one row required")
                                }
                            });
                            _td.append(_btn);
                            _tr.append(_td);

                            $("#Table-Trade-update tbody").append(_tr);
                            showSno++;
                        });
                    }
                    $("#ApplFlowId").val(datajson.Resultlist.FlowId);
                    $("#ApplCredatedBy").val(datajson.Resultlist.CreatedBy);

                    //Document Verification Payment  Details
                    if (datajson.Resultlist.PaymentOptionval == 1)
                        $("input[name=PaymentOption][value=1]").prop('checked', true);
                    else
                        $("input[name=PaymentOption][value=0]").prop('checked', true);
                    DocumentPaymentOption(datajson.Resultlist.PaymentOptionval);
                    $("#txtDocumentFeeReceiptDetails").val(datajson.Resultlist.DocumentFeeReceiptDetails);

                    if (datajson.Resultlist.ApplStatus == 10 || datajson.Resultlist.ApplStatus == 11 || datajson.Resultlist.ApplStatus == 12 || datajson.Resultlist.ApplStatus == 13) {
                        MakeButtonDisable = 1;
                        $("#ApplVerifiedOfficer").show();
                    }
                    else if (datajson.Resultlist.ReVerficationStatus == 0 && (datajson.Resultlist.DocumentFeeReceiptDetails != "" && datajson.Resultlist.DocumentFeeReceiptDetails != null)
                        && (datajson.Resultlist.AssignedVO == null || datajson.Resultlist.AssignedVO == 0)) {
                        MakeButtonDisable = 1;
                        $("#ApplSubmittedRecipt").show();
                    }
                    else if (datajson.Resultlist.ReVerficationStatus == 0 && (datajson.Resultlist.DocumentFeeReceiptDetails != "" && datajson.Resultlist.DocumentFeeReceiptDetails != null)
                        && (datajson.Resultlist.AssignedVO != 0 && datajson.Resultlist.AssignedVO != null)) {
                        MakeButtonDisable = 1;
                        $("#ApplSubmittedReciptVO").show();
                    }
                    else if (datajson.Resultlist.ReVerficationStatus == 1) {
                        if (sessionValue != datajson.Resultlist.FlowId) {
                            MakeButtonDisable = 1;
                            $("#ApplReSubmittedRecipt").show();
                        }
                    }

                    if (GivenGeneralInfoInd && GivenDocumentInfoInd && GivenInstutitePrefInfoInd) {
                        $("#EnableDisableSubmit").show();
                        $("#EnableDisableSubmitReason").hide();
                    }
                    else {
                        if (!$(".EditOptionShowGrid").is(":hidden")) {
                            $("#EnableDisableSubmitReason").show();
                        }
                        $("#EnableDisableSubmit").hide();
                    }

                    if (MakeButtonDisable == 1) {
                        $("#SaveApplicantDetails").hide();
                        $("#SaveEducationDetails").hide();
                        $("#SaveAddressDetails").hide();
                        $("#SaveVerificationCenter").hide();
                        $("#SaveInstitutePreferenceDet").hide();
                        $("#SavePaymentsDetails").hide();
                        $("#FinalSubmit").hide();
                        $("#SaveDocDet").hide();
                    }
                    if ($("#txtApplicantName").val() == "" || $("#txtCommunicationAddress").val() == "") {
                        $("#FinalSubmit").hide();
                        if (!$(".EditOptionShowGrid").is(":hidden")) {
                            $("#EnableDisableSubmitReason").show();
                        }
                    }
                }
                else {
                    $(".EligibleApplicationForm").hide();
                    $(".NotEligibleApplicationForm").show();
                }
            }

            if ($("#WillingToParticipateNextRound").is(":visible")) {
                ReshufflePreference();
            }
            //modified by sujit
            if ($("input[name=KanndaMedium]:checked").val() == "1" || $("input[name=RuralUrban]:checked").val() == "1") {
                $("#ExemptedCertificate").attr("disabled", true);
            }
            //else {
            //    $("#ExemptedCertificate").attr("disabled", false);
            //}
            if ($("#Category :selected").val() == 1) {
                $('input[name=EconomicallyWeakerSections]').attr("disabled", false);
            }
            else {
                $('input[name=EconomicallyWeakerSections]').attr("disabled", true);
            }
            if (datajson.Resultlist.ApplicationMode!=null) {
                $('#ddlApplicationMode').val(datajson.Resultlist.ApplicationMode);
                $("#ddlApplicationMode").attr("disabled", true);
            }
            
            if (datajson.Resultlist.Caste_RD_No!=null) {
                $('#txtCasteRd').val(datajson.Resultlist.Caste_RD_No);
                
                $("#chkYes1").prop('checked', true);
                $('#dvPassport1').show()
                $("#dvPassport").show()
            }
            if (datajson.Resultlist.EconomicWeaker_RD_No != null) {
                $('#txtEconomicWeakerRD').val(datajson.Resultlist.EconomicWeaker_RD_No);
                $("#chkYes").prop('checked', true);
                $('#dveconomic1').show()
                $("#dveconomic").show();
            }
            if (datajson.Resultlist.HYD_Karnataka_RD_No != null) {
                $('#txtHYDKarRD').val(datajson.Resultlist.HYD_Karnataka_RD_No);
                $("#chkYesHK").prop('checked', true);
                $("#dvHKR").show();
                $("#dvHKR1").show();
            }
            if (datajson.Resultlist.UDID_No != null) {
                $('#txtUDIDRd').val(datajson.Resultlist.UDID_No);
                $("#chkDyes").prop('checked', true);
                $("#divDisability").show();
                $("#divDisability1").show();
            }
            ;
            if (datajson.Resultlist.ApplicantNumber != null)
            {
                $('#divEnds *').prop('disabled', true);
            }
            
            
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function ValidateRDNumbers(val) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(val)) {
        return false;
    } else {
        return true;
    }
}