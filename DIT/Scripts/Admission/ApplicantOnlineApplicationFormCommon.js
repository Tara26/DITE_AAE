
function ClearErrorFields() {

    //Applicant Details
    $("#txtRollNumber-Required").hide();
    $("#txtRollNumberDuplicate-Required").hide();
    $("#academicyear1-Required").hide();
    $("#txtApplicantName-Required").hide();
    $("#Gender-Required").hide();
    $("#txtFathersName-Required").hide();
    $("#PhotoUpload-Required").hide();
    //$("#txtParentOccupation-Required").hide();
    $("#dateofbirth-Required").hide();
    $("#txtMothersName-Required").hide();
    $("#Religion-Required").hide();
    $("#Category-Required").hide();
    $("#MinorityCategory-Required").hide();
    $("#txtCaste-Required").hide();
    $("#txtFamilyAnnualIncome-Required").hide();
    $("#ApplicantType-Required").hide();
    $("#ApplicableReservations-Required").hide();
    $("#txtAadhaarNumber-Required").hide();
    $("#txtAadhaarNumberDuplicate-Required").hide();
    //$("#txtRationCard-Required").hide();
    $("#txtMothersName-Required").hide();

    //Education
    $("#Qualification-Required").hide();
    $("#txtInstituteStudied-Required").hide();
    $("#txtMaximumMarks-Required").hide();
    $("#txtMarksObtained-Required").hide();
    //$("#txtMinMarks-Required").hide();
    $("#Results-Required").hide();
    $("#CalculationMaximumMarks-Required").hide();

    //Address
    $("#txtCommunicationAddress-Required").hide();
    $("#Districts-Required").hide();
    $("#Talukas-Required").hide();
    $("#txtPincode-Required").hide();
    $("#txtPermanentAddress-Required").hide();
    $("#PermanentDistricts-Required").hide();
    $("#PermanentTalukas-Required").hide();
    $("#txtPermanentPincode-Required").hide();
    $("#txtApplicantPhoneNumber-Required").hide();
    $("#txtFathersPhoneNumber-Required").hide();
    $("#txtEmailId-Required").hide();

    //Institute
    $("#InstituteDistricts-Required").hide();
    $("#InstituteList-Required").hide();

    //Documents
    $("#EduCertificate-Required").hide();
    $("#CasteCertificate-Required").hide();
    $("#Rationcard-Required").hide();
    $("#Incomecertificate-Required").hide();
    $("#UIDNumber-Required").hide();
    $("#Ruralcertificate-Required").hide();
    $("#KannadamediumCertificate-Required").hide();
    $("#Differentlyabledcertificate-Required").hide();
    $("#ExemptedCertificate-Required").hide();
    $("#HyderabadKarnatakaRegion-Required").hide();
    $("#HoranaaduKannadiga-Required").hide();
    //$("#OtherCertificates-Required").hide();
    $("#KashmirMigrants-Required").hide();
    $("#Exserviceman-Required").hide();
    $("#EWSCertificate-Required").hide();
}

function RuralUrbanLocation() {

    $("input:radio[name=RuralUrban]").prop('checked', true);
    $("input[name='RuralUrban']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function GetcoursetypeListbycalendar() {

    $("#coursetypeId").empty();
     //$("#coursetypeId").append('<option value="0">Select CourseType</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetcoursetypeListbycalendar',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#coursetypeId").append($("<option/>").val(this.course_Id).text(this.course_type_name));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
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

    $("input[name=TenthBoard][value='1']").prop('checked', true)
    $("input[name='TenthBoard']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
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
        $(".TenthCOBSEType").hide();
        $("input[name=TenthCOBSEBoard][value=2]").prop('checked', true)
        MarksCGPAType();
    }
    else {
        $(".TenthCOBSEType").show();
        TenthCOBSEBoardType();
        MarksCGPAType();
    }
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
//Generate the Institute Details based on the zip code
$('#txtPincode').focusout(function () {
    searchOptionInstitue();
});

//TO call the API to get the basic details after capturing the Roll number
$('#txtRollNumber').focusout(function () {

    //API Should call to get the auto populate.
});

//To calculate percentage based on given marks
$('#txtMaximumMarks').focusout(function () {
    CalculatePercentage(true);
});

$('#txtMarksObtained').focusout(function () {
    CalculatePercentage(true);
});

$('#txtCGPA').focusout(function () {
    CalculatePercentage(true);
});

function CalculatePercentage(IsValidVal) {
    
    var IsValidP = IsValidVal;
    $("#CalculationMaximumMarks-Required").hide();
    var MaximumMarks = $("#txtMaximumMarks").val();
    var MarksObtained = $("#txtMarksObtained").val();
    //var MinMarks = $("#txtMinMarks").val();
    var CGPA = $("#txtCGPA").val();
    var Percentage = 0; var PercentageErr = false;
    var FullPercVal = 0;
    if (parseInt(MaximumMarks) != 0 && parseInt(MarksObtained) != 0 && !isNaN(parseInt(MaximumMarks)) && !isNaN(parseInt(MarksObtained))) {

        if (parseInt(MaximumMarks) >= parseInt(MarksObtained))
            Percentage = (MarksObtained / MaximumMarks) * 100;
        else
            PercentageErr = true;
    }
    else if (CGPA != 0 && !isNaN(parseInt(CGPA))) {
        Percentage = CGPA * 9.5;
    }

    if (parseInt(MarksObtained) > parseInt(MaximumMarks)) {
        //$("#txtMinMarks-Required").show();
        IsValidP = false;
    }

    if (PercentageErr) {
        $("#CalculationMaximumMarks-Required").show();
        IsValidP = false;
    }
    else {
        $("#CalculationMaximumMarks-Required").hide();
        IsValidP = true;
    }
    FullPercVal = Percentage.toFixed(2);
    $("#lblPercAsPerMarks").text(FullPercVal + "%");

    return {
        IsValidP,
        FullPercVal
    };
}

function TenthCOBSEBoardType() {
    var selectedTenthCOBSEBoard = $('input[name="TenthCOBSEBoard"]:checked').val();
    if (selectedTenthCOBSEBoard == 2 || selectedTenthCOBSEBoard == 3) {
        $("#TenthCBSEICSEType").show();
    }
    else {
        $("#TenthCBSEICSEType").hide();
    }
    MarksCGPAType();
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

//To Get the standard type based on the board type
function OnChangeStateBoardType() {

    $("input[name='RStateBoardType']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();

            $("#GetAppliedBasicsForBoardType").hide();
            if (value == 1) {
                $("#GetRollNumberFor10thStd").show();
                $("#GetAppliedBasicsForBoardType").show();
            }
            else {
                $("#GetRollNumberFor10thStd").hide();
               /* $("input[name=RAppBasics][value=1]").prop('checked', true);*/
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
            if (value == 2 || value == 3) {
                $("#GetRollNumberFor10thStd").show();
            }
        }
    });
}

$("#Category").change(function () {
    
    if ($(this).val() == 3 || $(this).val() == 4 || $(this).val() == 5 || $(this).val() == 1) {
        $(".hideshow").hide();
        $("#Incomecertificate").attr("disabled", true);
        bootbox.alert("please upload the relevent category document under document section.!!");
    }
    else {
        $(".hideshow").show();
        $("#Incomecertificate").attr("disabled", false);
        bootbox.alert("please upload the relevent category and income document under document section.!!");
    }

    if ($(this).val() != 1) {
        $("#CasteCertificate").attr("disabled", false);
    }
    else {
        $("#CasteCertificate").attr("disabled", true);
    }

    if ($(this).val() == 1) {
        //$("#dveconomic1").show();
        //$("#dvHKR1").show();
        $('input[name="chkeconomicsection"]').attr('disabled', false);
    } else {
        $('input[name="chkeconomicsection"]').attr('disabled', true);
        $('#dveconomic').hide();
    }
});

$("#ddlState1").change(function () {
    $('input[name="TenthBoard"]').prop('checked', false)
    $('input[name="TenthCOBSEBoard"]').prop('checked', false)
    $('input[name="HyderabadKarnatakaRegion"]').prop('checked', false)
    $('input[name="HyderabadKarnatakaRegion"]').attr('disabled', true);
    $('input[name="chkHKregion"]').attr('disabled', true);
    $('input[name="RuralUrban"]').attr('disabled', true);
});


function SaveApplicantDetails() {
    
  
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    var RollNumber = ""; var RStateBoardTypeval = 0; var RAppBasicsval = 1;
    $("#Gender-Required").hide();
    $("#txtRollNumberDuplicate-Required").hide();
    var RollNumberDuplicate = $("#RollNumberDuplicate").text();
    RStateBoardTypeval = $("input[name='RStateBoardType']:checked").val();
    if (RStateBoardTypeval == 1) {
        RAppBasicsval = $("input[name='RAppBasics']:checked").val();
        if (RAppBasicsval == 2) {
            RollNumber = $("#txtRollNumber").val();
            if (RollNumber == "" || ($("#txtRollNumber").val().length != 11)) {
                $("#txtRollNumber-Required").show();
                IsValid = false;
                bootbox.alert("Enter SSLC ROll number")
                return false;
            }
            else if (RollNumberDuplicate == 1) {
                $("#txtRollNumberDuplicate-Required").show();
                IsValid = false;
            }
        }
    }

    if (RStateBoardTypeval == 1)
        RStateBoardTypeval = true;
    else
        RStateBoardTypeval = false;

    var SameAsCommunicationAddress = 0;
    if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
        SameAsCommunicationAddress = 1;
    }

    $("#academicyear1-Required").hide();
    var AcademicMonths = $("#AcademicMonths").val();
    var AcademicYear = $("#AcademicYear").val();
    if (AcademicMonths == "" || AcademicYear == "") {
        $("#academicyear1-Required").show();
        IsValid = false;
    }

    $("#txtApplicantName-Required").hide();
    var ApplicantName = $("#txtApplicantName").val();
    ApplicantName = ApplicantName.toUpperCase();
    if (ApplicantName == "") {
        $("#txtApplicantName-Required").show();
        IsValid = false;
    }

    $("#Gender-Required").hide();
    var Gender = $("#Gender :selected").val();
    if (Gender == 0) {
        $("#Gender-Required").show();
        IsValid = false;
    }

    $("#txtFathersName-Required").hide();
    var FathersName = $("#txtFathersName").val();
    if (FathersName == "") {
        $("#txtFathersName-Required").show();
        IsValid = false;
    }

    $("#PhotoUpload-Required").hide();
    var e = $('#FileAttach');
    if (e.is(':disabled')) {
        var PhotoUpload = $("#PhotoUpload").val();
        if (PhotoUpload == "") {
            $("#PhotoUpload-Required").show();
            IsValid = false;
            bootbox.alert("Upload Photo")
            return false;
        }
    }
    else {
        var PhotoUpload = $("#PhotoUpload").val();
        if (PhotoUpload == "") {
            var IsUploaded = $('#IsUploaded').val();
            if (IsUploaded != "" && IsUploaded != undefined) {
                fileData.append(
                    "Photo", IsUploaded
                );
            }
            else {
                $("#PhotoUpload-Required").show();
                IsValid = false;
                bootbox.alert("Upload Photo")
                return false;
            }
        }
        else {
            var fileUpload = $('#PhotoUpload').get(0);
            var files = fileUpload.files;

            //var sizeInKB = files.size/1024;
            //var sizeLimit = 50;
            //var lessThan = 10;
            //if (sizeInKB > sizeLimit) {
            //    bootbox.alert("Max file size 50KB");
            //    return false;
            //}
            //if (sizeInKB < lessThan) {
            //    bootbox.alert("File size minimum 10KB");
            //    return false;
            //}

            for (var i = 0; i < files.length; i++) {
                fileData.append("PhotoFile", files[i]);
            }
        }
    }

    $("#txtParentOccupation-Required").hide();
    var ParentOccupation = $("#txtParentOccupation").val();
    //if (ParentOccupation == "") {
    //    $("#txtParentOccupation-Required").show();
    //    IsValid = false;
    //}

    var dateofbirth = "";
    $("#dateofbirth-Required").hide();
    var dateofbirth = $("#dateofbirth").val();
    var dateofbirthage = $("#dobError").val();
    if (dateofbirth == "" || dateofbirthage == 1) {
        $("#dateofbirth-Required").show();
        IsValid = false;
    }
    else {
        var dts = $("#dateofbirth").val().split("-");
        conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
        dateofbirth = (dts[1] + "-" + dts[0] + "-" + dts[2]);
    }

    $("#txtMothersName-Required").hide();
    var MothersName = $("#txtMothersName").val();
    if (MothersName == "") {
        $("#txtMothersName-Required").show();
        IsValid = false;
    }

    $("#Religion-Required").hide();
    var Religion = $("#Religion :selected").val();
    if (Religion == 0) {
        $("#Religion-Required").show();
        IsValid = false;
    }

  

    $("#MinorityCategory-Required").hide();
    var MinorityCategory = $("#MinorityCategory :selected").val();
    if (MinorityCategory == 0) {
        $("#MinorityCategory-Required").show();
        IsValid = false;
    }

 
    if (($("#Religion :selected").val()) != 2) {
        $("#txtCaste-Required").hide();
        IsValid = true;
    }


    if ($("#chkYes1").is(":checked") || $("#chkYes1").is(":checked")) {

        $("#txtCaste-Required").hide();
        var Caste = $("#txtCaste").val();
        //var CasteDetail = $("#txtCaste").val();
        if (/*CasteDetail == "" ||*/ Caste == 0) {
            $("#txtCaste-Required").show();
            IsValid = false;
        }

        $("#Category-Required").hide();
        var Category = $("#Category :selected").val();
        if (Category == 0) {
            $("#Category-Required").show();
            IsValid = false;
        }
    }

    

    $("#txtFamilyAnnualIncome-Required").hide();
    var FamilyAnnualIncome = $("#txtFamilyAnnualIncome").val();
    //if (FamilyAnnualIncome == "") {
    //    $("#txtFamilyAnnualIncome-Required").show();
    //    IsValid = false;
    //}
    
    $("#ApplicantType-Required").hide();
    var ApplicantType = $("#ApplicantType :selected").val();
    if (ApplicantType == 0) {
        $("#ApplicantType-Required").show();
        IsValid = false;
    }

    var txtAadhaarNumber = $("#txtAadhaarNumber").val();
    var AadhaarNumber = txtAadhaarNumber.replace('-', '');
    AadhaarNumber = AadhaarNumber.replace('-', '');
    var AadhaarNumberDuplicate = $("#AadhaarNumberDuplicate").text();
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#txtAadhaarNumber-Required").hide();
    //if (AadhaarNumber == "" || AadhaarNumber.length != 12) {
    //    $("#txtAadhaarNumber-Required").show();
    //    IsValid = false;
    //}
    //else if (AadhaarNumberDuplicate == 1) {
    //    $("#txtAadhaarNumberDuplicate-Required").show();
    //    IsValid = false;
    //}

    //$("#txtRationCard-Required").hide();
    var RationCard = $("#txtRationCard").val();
    //if (RationCard == "" || ($("#txtRationCard").val().length != 12)) {
    //    $("#txtRationCard-Required").show();
    //    IsValid = false;
    //}

    $("#txtMothersName-Required").hide();
    var MothersName = $("#txtMothersName").val();
    if (MothersName == "") {
        $("#txtMothersName-Required").show();
        IsValid = false;
    }

    //$("#AccountNumber-Required").hide();
    var AccountNumber = $("#AccountNumber").val();
    //if (AccountNumber.length < 11) {
    //    $("#AccountNumber-Required").show();
    //    IsValid = false;
    //}

    //$("#BankName-Required").hide();
    var BankName = $.trim($("#BankName").val());
    //if (BankName == "") {
    //    $("#BankName-Required").show();
    //    IsValid = false;
    //}

    //$("#IFSCCode-Required").hide();
    var IFSCCode = $.trim($("#IFSCCode").val());
    //if (IFSCCode.length < 10) {
    //    $("#IFSCCode-Required").show();
    //    IsValid = false;
    //}

    $("#PersonWithDisability-Required").hide();
    var PhysicallyHanidcapType = 0;
    var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd]:checked').val();
    if (PhysicallyHanidcapInd == 1) {
        PhysicallyHanidcapInd = true;
        PhysicallyHanidcapType = $("#PhysicallyHanidcapType :selected").val();
        if (PhysicallyHanidcapType == 0 || PhysicallyHanidcapType == "") {
            $("#PersonWithDisability-Required").show();
            IsValid = false;
        }
    }
    else {
        PhysicallyHanidcapInd = false;
    }

    var HoraNadu_GadiNadu_Kannidagas = $('input[name=HoraNadu]:checked').val();
    if (HoraNadu_GadiNadu_Kannidagas == 1)
        HoraNadu_GadiNadu_Kannidagas = true;
    else
        HoraNadu_GadiNadu_Kannidagas = false;

    var ExemptedFromStudyCertificate = $('input[name=ExemptedFromStudyCertificate]:checked').val();
    if (ExemptedFromStudyCertificate == 1)
        ExemptedFromStudyCertificate = true;
    else
        ExemptedFromStudyCertificate = false;

    var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion]:checked').val();
    if (HyderabadKarnatakaRegion == 1)
  
        HyderabadKarnatakaRegion = true;
  

    else
        HyderabadKarnatakaRegion = false;

    var KanndaMedium = $('input[name=KanndaMedium]:checked').val();
    if (KanndaMedium == 1)
        KanndaMedium = true;
    else
        KanndaMedium = false;

    var EconomyWeakerSection = $('input[name=EconomicallyWeakerSections]:checked').val();
    if (EconomyWeakerSection == 1)
        EconomyWeakerSection = true;
    else
        EconomyWeakerSection = false;

    var ExServiceMan = $('input[name=ExService]:checked').val();
    if (ExServiceMan == 1)
        ExServiceMan = true;
    else
        ExServiceMan = false;

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    var txtDocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

    //Added by sujit
    var CategoryName = "";
    if (Category == 5 || Category == 6 || Category == 7 || Category == 8 || Category == 9)
        CategoryName = 'OBC';
    else if (Category == 1)
        CategoryName = 'General';
    else if (Category == 3)
        CategoryName = 'Schedule Castes';
    else if (Category == 4)
        CategoryName = 'Schedule Tribes';
    
    $("#ApplicationModeRequired").hide();
    var ddlApplicationMode = $("#ddlApplicationMode").val();
    if (ddlApplicationMode == "0") {
        $("#ApplicationModeRequired").show();
        IsValid = false;
    }
    var Caste_RD_No=$("#txtCasteRd").val();
    var EconomicWeaker_RD_No=$("#txtEconomicWeakerRD").val();
    var HYD_Karnataka_RD_No=$("#txtHYDKarRD").val();
    var UDID_No = $("#txtUDIDRd").val();
    var CategoryId = $("#Category").val();
    if ( CategoryId == 6 || CategoryId == 7 || CategoryId == 8 || CategoryId == 9) {
        if ($("#txtFamilyAnnualIncome").val() == 0 || $("#txtCasteRDValidity").val() == "") {
            bootbox.alert("Enter both Family Annual income and Validity !!");
            return false;
        }
    }

    if (IsValid) {

        fileData.append(
            "RStateBoardType", RStateBoardTypeval
        );
        fileData.append(
            "RAppBasics", RAppBasicsval
        );
        fileData.append(
            "RollNumber", RollNumber
        );
        fileData.append(
            "ApplyMonth", AcademicMonths
        );
        fileData.append(
            "ApplyYear", AcademicYear
        );
        fileData.append(
            "ApplicantName", ApplicantName
        );
        fileData.append(
            "FathersName", FathersName
        );
        fileData.append(
            "ParentsOccupation", ParentOccupation
        );
        fileData.append(
            "DOB", dateofbirth
        );
        fileData.append(
            "MothersName", MothersName
        );
        fileData.append(
            "Religion", Religion
        );
        fileData.append(
            "Gender", Gender
        );
        fileData.append(
            "Category", Category
        );
        fileData.append(
            "MinorityCategory", MinorityCategory
        );
        fileData.append(
            "Caste", Caste
        );
        fileData.append(
            "FamilyAnnIncome", FamilyAnnualIncome
        );
        fileData.append(
            "ApplicantType", ApplicantType
        );
        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
            "RationCard", SHAEncryption(RationCard)
        );
        fileData.append(
            "AadhaarNumber", SHAEncryption(AadhaarNumber)
        );
        fileData.append(
            "AccountNumber", AccountNumber != '' ? SHAEncryption(AccountNumber) : AccountNumber = AccountNumber
        );
        fileData.append(
            "BankName", BankName != '' ? SHAEncryption(BankName) : BankName = BankName
        );
        fileData.append(
            "IFSCCode", IFSCCode != '' ? SHAEncryption(IFSCCode) : IFSCCode = IFSCCode
        );
        fileData.append(
            "PhysicallyHanidcapInd", PhysicallyHanidcapInd
        );

        fileData.append(
            "PhysicallyHanidcapType", PhysicallyHanidcapType
        );
        fileData.append(
            "ApplicationId", ApplicantId
        );

        fileData.append(
            "HoraNadu_GadiNadu_Kannidagas", HoraNadu_GadiNadu_Kannidagas
        );

        fileData.append(
            "ExemptedFromStudyCertificate", ExemptedFromStudyCertificate
        );

        fileData.append(
            "HyderabadKarnatakaRegion", HyderabadKarnatakaRegion
        );

        fileData.append(
            "KanndaMedium", KanndaMedium
        );

        fileData.append(
            "PaymentOptionval", PaymentOptionval
        );

        fileData.append(
            "DocumentFeeReceiptDetails", txtDocumentFeeReceiptDetails
        );
        fileData.append(
            "EconomyWeakerSection", EconomyWeakerSection
        );
        fileData.append(
            "ExServiceMan", ExServiceMan
        );
        fileData.append(
            "CategoryName", CategoryName
        );

        fileData.append(
            "ApplicationMode", ddlApplicationMode
        );
        fileData.append(
            "Caste_RD_No", Caste_RD_No
        );
        fileData.append(
            "EconomicWeaker_RD_No", EconomicWeaker_RD_No
        );
        fileData.append(
            "HYD_Karnataka_RD_No", HYD_Karnataka_RD_No
        );
        fileData.append(
            "UDID_No", UDID_No
        );
        //Applicant
        //$('#ApplicableReservations :selected').each(function (i, selected) {
        //    fileData.append(
        //        "ApplicableReservations", $(selected).val()
        //    );
        //});

        $.ajax({
            type: "POST",
            url: "/Admission/InsertApplicantFormDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {

                if (data.objReturnApplicationForm != null) {
                    //$.each(data.objReturnApplicationForm, function () {
                    //    GetMasterData();
                    //});
                    var alertmsg = "";
                    if (data.pref.status == "Error occured!")
                        alertmsg = "<br><br>Error in data !";
                    else {
                        alertmsg = "<br><br>Applicant Details saved successfully !";
                        bootbox.alert({
                            message: alertmsg,
                            callback: function () {
                                //location.reload(true);
                                GetMasterData();
                            }
                        });
                    }
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function SaveEducationDetails() {
    
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;
    var MaxMarks = 0;
    var MarksObtained = 0;

    //Education
    $("#RuralcertificateYes-Required").hide();
    $("#Qualification-Required").hide();
    var Qualification = $("#Qualification :selected").val();
    if (Qualification == 0) {
        $("#Qualification-Required").show();
        IsValid = false;
    }

    var TenthCOBSEBoard = $('input[name=TenthCOBSEBoard]:checked').val();
    //var EducationGrade = $("#txtEduGrade").val();
    var ApplicantBelongTo = $('input[name=RuralUrban]:checked').val();
    var AppliedBasic = $('input[name=AppBasics]:checked').val();
    var TenthBoard = $('input[name=TenthBoard]:checked').val();

    $("#txtInstituteStudied-Required").hide();
    var InstituteStudiedQual = $("#txtInstituteStudied").val();
    if (InstituteStudiedQual == 0) {
        $("#txtInstituteStudied-Required").show();
        IsValid = false;
    }

    if (TenthCOBSEBoard != 7) {
        $("#OtherBoards").val() == 0;
    }

    $("#OtherBoards-Required").hide();
    var OtherBoardsVal = $("#OtherBoards").val();
    if (OtherBoardsVal == "0" && $("#OtherBoards").is(":visible") == true) {
        $("#OtherBoards-Required").show();
        IsValid = false;
    }
    var selectedMarksCGPAType = $('input[name="CGPAMarks"]:checked').val();
    if (selectedMarksCGPAType == 0) {
        $("#txtMaximumMarks-Required").hide();
         MaxMarks = $("#txtMaximumMarks").val();
        if (MaxMarks == 0 && $("#txtMaximumMarks").is(":disabled") == false) {
            $("#txtMaximumMarks-Required").show();
            IsValid = false;
        }

        $("#txtMarksObtained-Required").hide();
         MarksObtained = $("#txtMarksObtained").val();
        if (MarksObtained == 0) {
            $("#txtMarksObtained-Required").show();
            IsValid = false;
        }
    }
    //$("#txtMinMarks-Required").hide();
    //var MinMarks = $("#txtMinMarks").val();
    //if (MinMarks == 0) {
    //    $("#txtMinMarks-Required").show();
    //    IsValid = false;
    //}
    if (selectedMarksCGPAType == 1) {
        $("#txtCGPA-Required").hide();
        var CGPAGrade = $("#txtCGPA").val();
        if (CGPAGrade == "0") {  //&& $("#txtCGPA").is(":visible") == true
            $("#txtCGPA-Required").show();
            IsValid = false;
        }
    }
    let PercentageReturnValue = CalculatePercentage(IsValid);

    IsValidP = PercentageReturnValue.IsValidP,
        Percentage = PercentageReturnValue.FullPercVal;

    if (IsValidP == false) {
        IsValid = false
    }

    $("#Results-Required").hide();
    var ResultQual = $("#Results :selected").val();
    if (ResultQual == 0) {
        $("#Results-Required").show();
        IsValid = false;
    }

    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    var txtDocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

    //var studiedMathsScience = $('input[name=studiedMathsScience]:checked').val();
    //if (studiedMathsScience == 1)
    //    studiedMathsScience = true;
    //else
    //    studiedMathsScience = false;

    if (IsValid) {
        fileData.append(
            "CredatedBy", CredatedBy
        );
        fileData.append(
            "ApplicationId", ApplicantId
        );

        fileData.append(
            "TenthCOBSEBoard", TenthCOBSEBoard
        );
        //fileData.append(
        //    "EducationGrade", EducationGrade
        //);

        //Education
        fileData.append(
            "Qualification", Qualification
        );
        fileData.append(
            "ApplicantBelongTo", ApplicantBelongTo
        );
        fileData.append(
            "AppliedBasic", AppliedBasic
        );
        fileData.append(
            "TenthBoard", TenthBoard
        );
        fileData.append(
            "BoardId", OtherBoardsVal
        );
        fileData.append(
            "Percentage", Percentage
        );
        fileData.append(
            "InstituteStudiedQual", InstituteStudiedQual
        );
        fileData.append(
            "MaxMarks", $("#txtMaximumMarks").val()
        );
        fileData.append(
            "MinMarks", "300"
        );
        fileData.append(
            "MarksObtained", $("#txtMarksObtained").val()
        );

        fileData.append(
            "EducationCGPA", CGPAGrade
        );

        fileData.append(
            "ResultQual", ResultQual
        );
        fileData.append(
            "ApplicantNumber", ApplicantNumber
        );

        fileData.append(
            "PaymentOptionval", PaymentOptionval
        );

        fileData.append(
            "DocumentFeeReceiptDetails", txtDocumentFeeReceiptDetails
        );

        //fileData.append(
        //    "studiedMathsScience", studiedMathsScience
        //);

        $.ajax({
            type: "POST",
            url: "/Admission/SaveEducationDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    //$.each(data.objReturnApplicationForm, function () {
                    //    GetMasterData(0);
                    //});

                    if (data.pref.status == "Error occured!")
                        bootbox.alert("<br><br>Error in data !");
                    else
                        bootbox.alert("<br><br>Applicant Education Data saved Successfully !");
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function SaveAddressDetails() {


    debugger;
    ClearErrorFields();
    var ApplicantId = $("#ApplicantId").val();
    var CredatedBy = $("#CredatedBy").val();
    var fileData = new FormData();
    var IsValid = true;

    //Address
    $("#txtCommunicationAddress-Required").hide();
    var CommunicationAddress = $("#txtCommunicationAddress").val();
    if (CommunicationAddress == "") {
        $("#txtCommunicationAddress-Required").show();
        IsValid = false;
    }

    $("#Districts-Required").hide();
    var Districts = $("#Districts :selected").val();
    if (Districts == 0) {
        $("#Districts-Required").show();
        IsValid = false;
    }

    $("#Talukas-Required").hide();
    var Talukas = $("#Talukas :selected").val();
    if (Talukas == 0) {
        $("#Talukas-Required").show();
        IsValid = false;
    }

    $("#txtPincode-Required").hide();
    var Pincode = $("#txtPincode").val();
    if (Pincode == "") {
        $("#txtPincode-Required").show();
        IsValid = false;
    }

    $("#txtPermanentAddress-Required").hide();
    var PermanentAddress = $("#txtPermanentAddress").val();
    if (PermanentAddress == "") {
        $("#txtPermanentAddress-Required").show();
        IsValid = false;
    }

    $("#PermanentDistricts-Required").hide();
    var PermanentDistricts = $("#PermanentDistricts :selected").val();
    if (PermanentDistricts == "" || PermanentDistricts == 0) {
        $("#PermanentDistricts-Required").show();
        IsValid = false;
    }

    $("#PermanentTalukas-Required").hide();
    var PermanentTalukas = $("#PermanentTalukas :selected").val();
    if (PermanentTalukas == 0) {
        $("#PermanentTalukas-Required").show();
        IsValid = false;
    }

    $("#txtPermanentPincode-Required").hide();
    var PermanentPincode = $("#txtPermanentPincode").val();
    if (PermanentPincode == "") {
        $("#txtPermanentPincode-Required").show();
        IsValid = false;
    }

    //$("#txtApplicantPhoneNumber-Required").hide();
    //var ApplicantPhoneNumber = $("#txtApplicantPhoneNumber").val();
    //if (ApplicantPhoneNumber == "" || ApplicantPhoneNumber.length != 10) {
    //    $("#txtApplicantPhoneNumber-Required").show();
    //    IsValid = false;
    //}

    var txtApplicantPhoneNumber = $("#txtApplicantPhoneNumber").val();
    var ApplicantPhoneNumber = txtApplicantPhoneNumber.replace('-', '');
    ApplicantPhoneNumber = ApplicantPhoneNumber.replace('-', '');
    var phonenumberDuplicate = $("#ApplicantPhoneNumberDuplicate").text();
    $("#txtApplicantPhoneNumberDuplicate-Required").hide();
    $("#txtApplicantPhoneNumber-Required").hide();
    if (ApplicantPhoneNumber == "" || ApplicantPhoneNumber.length != 10) {
        $("#txtApplicantPhoneNumber-Required").show();
        IsValid = false;
    }
    else if (phonenumberDuplicate == 1) {
        $("#txtApplicantPhoneNumberDuplicate-Required").show();
        bootbox.alert("<br><br>Phone Number Already Exist/Invalid*");
        IsValid = false;
    }


    $("#txtFathersPhoneNumber-Required").hide();
    var FathersPhoneNumber = $("#txtFathersPhoneNumber").val();
    //if (FathersPhoneNumber == "" || FathersPhoneNumber.length != 10) {
    //    $("#txtFathersPhoneNumber-Required").show();
    //    IsValid = false;
    //}

    //$("#txtEmailId-Required").hide();
    //var EmailId = $("#txtEmailId").val();
    //if (EmailId == "") {
    //    $("#txtEmailId-Required").show();
    //    IsValid = false;
    //}
    //var IsValidE = ValidateEmail(EmailId);
    //if (IsValidE == false)
    //    IsValid = false;

    var EmailId = $("#txtEmailId").val();
    var txtEmailId = EmailId.replace('-', '');
    txtEmailId = txtEmailId.replace('-', '');
    var EmailIdDuplicate = $("#EmailIdDuplicate").text();
    $("#txtEmailIdDuplicate-Required").hide();
    $("#txtEmailId-Required").hide();
    if (txtEmailId == "") {
        $("#txtEmailId-Required").show();
        IsValid = false;
    }
    else if (EmailIdDuplicate == 1) {
        $("#txtEmailIdDuplicate-Required").show();
        bootbox.alert("<br><br>EmailId Already Exist/Invalid*");
        IsValid = false;
    }
    var IsValidE = ValidateEmail(EmailId);
    if (IsValidE == false)
        IsValid = false;


    var PaymentOptionval = $("input[name='PaymentOption']:checked").val();
    var txtDocumentFeeReceiptDetails = $("#txtDocumentFeeReceiptDetails").val();

    if (IsValid) {

        fileData.append(
            "ApplicationId", ApplicantId
        );

        //Address
        fileData.append(
            "CommunicationAddress", CommunicationAddress
        );
        fileData.append(
            "DistrictId", Districts
        );
        fileData.append(
            "TalukaId", Talukas
        );
        fileData.append(
            "Pincode", Pincode
        );
        fileData.append(
            "PermanentAddress", PermanentAddress
        );

        var SameAsCommunicationAddress = 0;
        if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
            SameAsCommunicationAddress = 1;
        }

        var SameAdd = false;
        if (SameAsCommunicationAddress == 1)
            SameAdd = true;

        fileData.append(
            "SameAdd", SameAdd
        );

        fileData.append(
            "PermanentDistricts", PermanentDistricts
        );
        fileData.append(
            "PTaluk", PermanentTalukas
        );
        fileData.append(
            "PPinCode", PermanentPincode
        );

        fileData.append(
            "ApplicantPhoneNumber", ApplicantPhoneNumber
        );
        fileData.append(
            "FathersPhoneNumber", FathersPhoneNumber
        );
        fileData.append(
            "EmailId", EmailId
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
            url: "/Admission/SaveAddressDetails",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (data) {
                if (data.objReturnApplicationForm != null) {
                    $.each(data.objReturnApplicationForm, function () {
                        GetMasterData(0);
                    });

                    if (data.pref.status == "Error occured!")
                        bootbox.alert("<br><br>Error in data !");
                    else
                        bootbox.alert("<br><br>Applicant Communication Data saved Successfully !");
                }
                else {
                    bootbox.alert("<br><br>Cannot add to list !");
                }
            }
        });
    }
}

function clearallErrorFields() {

    //Applicant Details   
    $("#txtApplicantName-Required").hide();
    $("#Gender-Required").hide();
    $("#txtFathersName-Required").hide();
    $("#txtParentOccupation-Required").hide();
    $("#dateofbirth-Required").hide();
    $("#txtMothersName-Required").hide();
    $("#Religion-Required").hide();
    $("#Category-Required").hide();
    $("#MinorityCategory-Required").hide();
    $("#txtCaste-Required").hide();
    $("#txtFamilyAnnualIncome-Required").hide();
    $("#ApplicantType-Required").hide();
    $("#ApplicableReservations-Required").hide();
    $("#txtAadhaarNumber-Required").hide();
    $("#txtAadhaarNumberDuplicate-Required").hide();
    $("#txtRationCard-Required").hide();
    $("#txtMothersName-Required").hide();

    //Education
    $("#Qualification-Required").hide();
    $("#txtInstituteStudied-Required").hide();
    $("#txtMaximumMarks-Required").hide();
    $("#txtMarksObtained-Required").hide();
    $("#txtMinMarks-Required").hide();
    $("#Results-Required").hide();
    $("#CalculationMaximumMarks-Required").hide();

    //Address
    $("#txtCommunicationAddress-Required").hide();
    $("#Districts-Required").hide();
    $("#Talukas-Required").hide();
    $("#txtPincode-Required").hide();
    $("#txtPermanentAddress-Required").hide();
    $("#PermanentDistricts-Required").hide();
    $("#PermanentTalukas-Required").hide();
    $("#txtPermanentPincode-Required").hide();
    $("#txtApplicantPhoneNumber-Required").hide();
    $("#txtFathersPhoneNumber-Required").hide();
    $("#txtEmailId-Required").hide();

    //Documents
    $("#EduCertificate-Required").hide();
    $("#CasteCertificate-Required").hide();
    $("#Rationcard-Required").hide();
    $("#Incomecertificate-Required").hide();
    $("#UIDNumber-Required").hide();
    $("#Ruralcertificate-Required").hide();
    $("#KannadamediumCertificate-Required").hide();
    $("#Differentlyabledcertificate-Required").hide();
    $("#ExemptedCertificate-Required").hide();
    $("#HyderabadKarnatakaRegion-Required").hide();
    $("#HoranaaduKannadiga-Required").hide();
    $("#OtherCertificates-Required").hide();
    $("#EWSCertificate-Required").hide();
    $("#Exserviceman-Required").hide();

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
    $("#DocKasMigAcceptedImg").hide();
    $("#DocExSerAcceptedImg").hide();
    $("#DocLLCAcceptedImg").hide();
    $("#DocEWSAcceptedImg").hide();

    $("#DocEduCerRejectedImg").hide();
    $("#DocCasCerRejectedImg").hide();
    $("#DocRatCerRejectedImg").hide();
    $("#DocIncCerRejectedImg").hide();
    $("#DocUIDCerRejectedImg").hide();
    $("#DocRurCerRejectedImg").hide();
    $("#DocKanMedCerRejectedImg").hide();
    $("#DocDidAblCerRejectedImg").hide();
    $("#DocExStuCerRejectedImg").hide();
    $("#DocHyKarRegRejectedImg").hide();
    $("#DocHorGadKanRejectedImg").hide();
    $("#DocOthCerRejectedImg").hide();
    $("#DocExSerRejectedImg").hide();
    $("#DocEWSRejectedImg").hide();

    $("#WithoutDocEduStatus-Required").hide();
    $("#WithoutDocEduRemarks-Required").hide();
    $("#WithoutDocCasStatus-Required").hide();
    $("#WithoutDocCasRemarks-Required").hide();
    $("#WithoutDocRatStatus-Required").hide();
    $("#WithoutDocRatRemarks-Required").hide();
    $("#WithoutDocIncStatus-Required").hide();
    $("#WithoutDocIncRemarks-Required").hide();
    $("#WithoutDocUIDStatus-Required").hide();
    $("#WithoutDocUIDRemarks-Required").hide();
    $("#WithoutDocRurStatus-Required").hide();
    $("#WithoutDocRurRemarks-Required").hide();
    $("#WithoutDocKMCStatus-Required").hide();
    $("#WithoutDocKMCRemarks-Required").hide();
    $("#WithoutDocDAStatus-Required").hide();
    $("#WithoutDocDARemarks-Required").hide();
    $("#WithoutDocECStatus-Required").hide();
    $("#WithoutDocECRemarks-Required").hide();
    $("#WithoutDocHKRStatus-Required").hide();
    $("#WithoutDocHKRRemarks-Required").hide();
    $("#WithoutDocHorKanStatus-Required").hide();
    $("#WithoutDocHorKanRemarks-Required").hide();
    $("#WithoutDocOCStatus-Required").hide();
    $("#WithoutDocOCRemarks-Required").hide();
    $("#WithoutDocKMStatus-Required").hide();
    $("#WithoutDocKMRemarks-Required").hide();
    $("#WithoutDocExSerStatus-Required").hide();
    $("#WithoutDocExSerRemarks-Required").hide();
    $("#WithoutDocLLCStatus-Required").hide();
    $("#WithoutDocLLCRemarks-Required").hide();
    $("#WithoutDocEWSStatus-Required").hide();
    $("#WithoutDocEWSRemarks-Required").hide();
    $("#txtRemarks-Required").hide();

    $("#TraineeType-Required").hide();
    $("#AdmisionTime-Required").hide();
    $("#PaymentDate-Required").hide();

    $("#EduCertificate").prop('disabled', false);
    $("#EduDocStatus").prop('disabled', false);
    $("#txtEduCertRemarks").prop('disabled', false);
    $("#CasteCertificate").prop('disabled', false);
    $("#CasDocStatus").prop('disabled', false);
    $("#txtCasteCertRemarks").prop('disabled', false);
    $("#Rationcard").prop('disabled', false);
    $("#RationDocStatus").prop('disabled', false);
    $("#txtRationCardRemarks").prop('disabled', false);
    $("#Incomecertificate").prop('disabled', false);
    $("#IncCerDocStatus").prop('disabled', false);
    $("#txtIncCertRemarks").prop('disabled', false);
    $("#UIDNumber").prop('disabled', false);
    $("#UIDDocStatus").prop('disabled', false);
    $("#txtUIDRemarks").prop('disabled', false);
    $("#Ruralcertificate").prop('disabled', false);
    $("#RcerDocStatus").prop('disabled', false);
    $("#txtRurCertRemarks").prop('disabled', false);
    $("#KannadamediumCertificate").prop('disabled', false);
    $("#KanMedCerDocStatus").prop('disabled', false);
    $("#txtKanMedRemarks").prop('disabled', false);
    $("#Differentlyabledcertificate").prop('disabled', false);
    $("#DiffAblDocStatus").prop('disabled', false);
    $("#txtDiffAbledCertRemarks").prop('disabled', false);
    $("#ExemptedCertificate").prop('disabled', false);
    $("#ExCerDocStatus").prop('disabled', false);
    $("#txtExeCertRemarks").prop('disabled', false);
    $("#HyderabadKarnatakaRegion").prop('disabled', false);
    $("#HyKarDocStatus").prop('disabled', false);
    $("#txtHydKarnRemarks").prop('disabled', false);
    $("#HoranaaduKannadiga").prop('disabled', false);
    $("#HorKanDocStatus").prop('disabled', false);
    $("#txtHorGadKannadigaRemarks").prop('disabled', false);
    $("#OtherCertificates").prop('disabled', false);
    $("#OtherCerDocStatus").prop('disabled', false);
    $("#txtOtherCertRemarks").prop('disabled', false);
    $("#Exserviceman").prop('disabled', false);
    $("#ExserDocStatus").prop('disabled', false);
    $("#txtExservicemanRemarks").prop('disabled', false);
    $("#EWSCertificate").prop('disabled', false);
    $("#EWSDocStatus").prop('disabled', false);
    $("#txtEWSCertificateRemarks").prop('disabled', false);
}


function OnchangechkSameAsCommunicationAddress() {
    debugger;
    if ($("#chkSameAsCommunicationAddress").prop('checked') == true) {
        $("#txtPermanentAddress").val($("#txtCommunicationAddress").val());
        $("#PermanentDistricts").val($("#Districts :selected").val());

        GetTalukBySameAsCommunication($("#PermanentDistricts").val());
        //$("#PermanentTalukas").val($("#Talukas :selected").val());
        
        

        //$.ajax({
        //    type: 'Get',
        //    url: '/Admission/GetMasterApplicantData',
        //    success: function (datajson) {
        //        if (datajson.Resultlist != null || datajson.Resultlist != '') {
        //            if (datajson.Resultlist.GetDistrictList.length > 0) {
        //                $.each(datajson.Resultlist.GetDistrictList, function () {
        //                    $.each($(this.TalukListDet), function (index, item) {
        //                        $("#PermanentTalukas").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
        //                    });
        //                });

        //                alert(datajson.Resultlist.GetDistrictList);
        //            }
        //        }
        //    }
        //});

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

function IncomeMoneyPhoneValidation(eveObj, id) {

    var eligibleLen = 10;
    if (id == "txtFamilyAnnualIncome") {
        eligibleLen = 8;
    }
    else if (id == "txtPermanentPincode" || id == "txtPincode") {
        eligibleLen = 5;
    }
    //else if (id == "txtMaximumMarks" || id == "txtMarksObtained" || id == "txtMinMarks") {
    //    eligibleLen = 2;
    //}
    else if (id == "AccountNumber") {
        eligibleLen = 15;
    }

    var len = $("#" + id).val().length;
    if (eveObj.charCode == 45 || eveObj.charCode == 43 || eveObj.charCode == 101 || len > eligibleLen ||
        (eveObj.charCode == 46 && id == "AccountNumber")) {
        event.preventDefault();
    }
}

function CheckApplicantPhoneNumberAvailability() {
    var txtPhoneNumber = $("#txtApplicantPhoneNumber").val();
    var PhoneNumber = txtPhoneNumber.replace('-', '');
    PhoneNumber = PhoneNumber.replace('-', '');
    $("#txtApplicantPhoneNumberDuplicate-Required").hide();
    $("#ApplicantPhoneNumberDuplicate").text(0);

    if (!($.isNumeric(PhoneNumber))) {
        $("#txtApplicantPhoneNumberDuplicate-Required").show();
        $("#ApplicantPhoneNumberDuplicate").text(1);
    }
    else {
        var ApplicationId = $("#ApplicantId").val();

        $.ajax({
            type: "GET",
            url: "/Admission/CheckPhoneNumberAvailability",
            data: { strName: PhoneNumber, ApplicationId: ApplicationId, AadhaarRollNumber: 2 },
            success: function (data) {
                if (data == 1) {
                    $("#txtApplicantPhoneNumberDuplicate-Required").show();
                    bootbox.alert("<br><br>Phone Number Already Exist/Invalid*");
                    $("#ApplicantPhoneNumberDuplicate").text(1);
                }
            }
        });
    }
}

function CheckEmailIdAvailability() {

    var txtEmailId = $("#txtEmailId").val();
    var EmailId = txtEmailId.replace('-', '');
    EmailId = EmailId.replace('-', '');
    $("#txtEmailIdDuplicate-Required").hide();
    $("#EmailIdDuplicate").text(0);

    //if (!($.isNumeric(EmailId))) {
    //    $("#txtEmailIdDuplicate-Required").show();
    //    $("#EmailIdDuplicate").text(1);
    //}
    //else {
    var ApplicationId = $("#ApplicantId").val();

    $.ajax({
        type: "GET",
        url: "/Admission/CheckEmailIdAvailability",
        data: { strName: txtEmailId, ApplicationId: ApplicationId, AadhaarRollNumber: 2 },
        success: function (data) {

            if (data == 1) {
                $("#txtEmailIdDuplicate-Required").show();
                bootbox.alert("<br><br>EmailId Already Exist/Invalid*");
                $("#EmailIdDuplicate").text(1);
            } else {
                $("#txtEmailIdDuplicate-Required").hide();
            }
        }
    });
    //}
}

function GetGenderList() {

    $("#Gender").empty();
    $("#Gender").append('<option value="0">Select Gender</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetGenderList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Gender").append($("<option/>").val(this.Gender_Id).text(this.Gender));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetReligionList() {

    $("#Religion").empty();
    $("#Religion").append('<option value="choose">Select Religion</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetReligionList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Religion").append($("<option/>").val(this.Religion_Id).text(this.Religion));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetCategoryList() {

    $("#Category").empty();
    $("#Category").append('<option value="0">Select Category</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetCategoryList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Category").append($("<option/>").val(this.CategoryId).text(this.Category));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetTaluka(valueChange) {

    var Districts = "";
    if (valueChange != "Permanent") {
        Districts = $('#Districts :selected').val();
        $("#Talukas").empty();
        $("#Talukas").append('<option value="0">Select</option>');
    }
    else {
        Districts = $('#PermanentDistricts :selected').val();
        $("#PermanentTalukas").empty();
        $("#PermanentTalukas").append('<option value="0">Select</option>');
    }

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (valueChange != "Permanent")
                        $("#Talukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                    else
                        $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetTalukaOnLoad(TalukaId) {
    debugger;
    Districts = $('#Districts :selected').val();

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    
                        $("#Talukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                        $("#Talukas").val(TalukaId);
                });
            }

        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function GetTalukaForPermanent(valueChange, talukChange) {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: valueChange },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetTalukBySameAsCommunication(valueChange) {
    var Districts = valueChange;
    $("#PermanentTalukas").empty();
    $("#PermanentTalukas").append('<option value="0">Select</option>');

    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukMasterList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                debugger;
                $.each(data, function () {
                    $("#PermanentTalukas").append($("<option/>").val(this.taluk_lgd_code).text(this.taluk_ename));
                });

                $("#PermanentTalukas").val($("#Talukas :selected").val());
            }

            debugger;
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function OnClickEditCancel() {
    //hdnSession
    debugger
    if ($("#hdnSessionRoleId").data('value') == 12) {
        debugger
        $(".clsVerifyDocAppl").show();
    }

    $(".EditOptionShowGrid").hide();
    if ($(".tab-pane.active").attr("id") == "tab_1") {
        $(".SearchOptionShowGrid").show();
        $(".RemovebkEditClick").show();
    }
    if ($(".tab-pane.active").attr("id") == "tab_2") {
        $(".SearchOptionAdmittedShowGrid").show();
        $(".RemovebkEditAdmittedClick").show();
    }
    if ($(".tab-pane.active").attr("id") == "tab_3") {
        $(".SearchOptionRejectedShowGrid").show();
        $(".RemovebkEditRejectedClick").show();
    }
    if ($(".tab-pane.active").attr("id") == "tab_4") {
        $(".SearchOptionReconcileShowGrid").show();
        $(".RemovebkEditReconcileClick").show();
    }
}

function GetApplicantDetailsByIdCmn(ApplicantId) {
 
    $('input[type="text"], textarea').attr('readonly', false);
    $("input[name=AdmittedStatus][value=3]").prop('disabled', false);

    clearallErrorFields();
    RuralUrbanLocation();
    AppliedWhichBasics();
    AppliedForSyallbus();

    GetcoursetypeListbycalendar();

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

    $("#OtherBoards").empty();
    $("#OtherBoards").append('<option value="0">Select Board/Institute</option>');

    $("#KanMedCer").val(0); $("#DiffAbledCer").val(0); $("#HoraNaduCer").val(0); $("#ExeStuCer").val(0); $("#HydKarCer").val(0);
    $("#ExServiceCer").val(0); $("#LandLoserCer").val(0); $("#EcoWeakCer").val(0); $("#KMCer").val(0);
    $("#ErrorWithoutStatusMsg").hide();
    $.ajax({
        type: 'Get',
        data: { CredatedBy: ApplicantId },
        url: '/Admission/GetApplicationDetailsById',
        success: function (datajson) {

            var sessionValue = $("#hdnSession").data('value');

            $(".EduFileAttach").hide(); $(".CasteFileAttach").hide(); $(".RationFileAttach").hide(); $(".IncomeCertificateAttach").hide();
            $(".UIDFileAttach").hide(); $(".RuralcertificateAttach").hide(); $(".KannadamediumCertificateAttach").hide(); $(".DifferentlyabledcertificateAttach").hide();
            $(".ExemptedCertificateAttach").hide(); $(".HyderabadKarnatakaRegionAttach").hide(); $(".HoranaaduKannadigaAttach").hide(); $(".OtherCertificatesAttach").hide();
            $(".KashmirMigrantsAttach").hide(); $(".ExservicemanAttach").hide(); $(".LLCertificateAttach").hide(); $(".EWSCertificateAttach").hide();
            $("#DocRurCerAcceptedImg").hide();
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
            $("#EduCertificate").val('');
            $("#CasteCertificate").val('');
            $("#Rationcard").val('');
            $("#Incomecertificate").val('');
            $("#UIDNumber").val('');
            $("#Ruralcertificate").val('');
            $("#KannadamediumCertificate").val('');
            $("#Differentlyabledcertificate").val('');
            $("#ExemptedCertificate").val('');
            $("#HyderabadKarnatakaRegion").val('');
            $("#HoranaaduKannadiga").val('');
            $("#OtherCertificates").val('');
            $("#Exserviceman").val('');
            $("#EWSCertificate").val('');
            $("#ApplicableReservations option:selected").prop("selected", false);

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

                $("#ApplicantId").val(datajson.Resultlist.ApplicationId);
                $("#ApplCredatedBy").val(datajson.Resultlist.CredatedBy);
                $("#ApplFlowId").val(datajson.Resultlist.FlowId);
                //ApplicantDetails
                //Get the roll number
                $("input[name=RStateBoardType][value=" + datajson.Resultlist.RStateBoardType + "]").prop('checked', true);
                $("input[name=RAppBasics][value=" + datajson.Resultlist.RAppBasics + "]").prop('checked', true);
                $("#txtRollNumber").val(datajson.Resultlist.RollNumber);
                OnChangeStateBoardType();
                OnChangeGetAppliedBasicsForBoardType();

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
                        if (this.ApplDocVerifiID != 14) {
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
                        }
                    });
                }
                //Added by sujit
                if (datajson.Resultlist.ExServiceMan == true) {
                    $("input[name=ExServiceManRDB][value=1]").prop('checked', true);
                }
                else
                    $("input[name=ExServiceManRDB][value=0]").prop('checked', true);

                if (datajson.Resultlist.EconomyWeakerSection == true) {
                    $("input[name=EWSCertificateRDB][value=1]").prop('checked', true);
                }
                else
                    $("input[name=EWSCertificateRDB][value=0]").prop('checked', true);

                //$("input[name=ExServiceManRDB][value=0]").prop('checked', true);
                //$("input[name=EWSCertificateRDB][value=0]").prop('checked', true);
                //var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;
                //if (MultiselectSelectedValue != null) {
                //    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                //        if (e == "2") {
                //            $("input[name=ExServiceManRDB][value=1]").prop('checked', true);
                //            $("#ExServiceCer").val(1);
                //        }
                //        else if (e == "5") {
                //            $("input[name=EWSCertificateRDB][value=1]").prop('checked', true);
                //            $("#EcoWeakCer").val(1);
                //        }

                //    });
                //}

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $('#PermanentDistricts').empty();
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#Districts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        $("#PermanentDistricts").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    });
                }

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {
                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach").show();
                                $('#aEduCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks").val(this.Remarks);
                            }
                            $("#EDocAppId").val(this.DocAppId);
                            $("#ECreatedBy").val(this.CreatedBy);
                            $("#EduDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEduCerAcceptedImg").show();
                                $("#DocEduCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEduCerRejectedImg").show();
                                $("#DocEduCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach").show();
                                $('#aCasteCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks").val(this.Remarks);
                            }
                            $("#CDocAppId").val(this.DocAppId);
                            $("#CCreatedBy").val(this.CreatedBy);
                            $("#CasDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocCasCerAcceptedImg").show();
                                $("#DocCasCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocCasCerRejectedImg").show();
                                $("#DocCasCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach").show();
                                $('#aRationCard').attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks").val(this.Remarks);
                            }
                            $("#RDocAppId").val(this.DocAppId);
                            $("#RCreatedBy").val(this.CreatedBy);
                            $("#RationDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRatCerAcceptedImg").show();
                                $("#DocRatCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRatCerRejectedImg").show();
                                $("#DocRatCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach").show();
                                $('#aIncomeCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks").val(this.Remarks);
                            }
                            $("#IDocAppId").val(this.DocAppId);
                            $("#ICreatedBy").val(this.CreatedBy);
                            $("#IncCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocIncCerAcceptedImg").show();
                                $("#DocIncCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocIncCerRejectedImg").show();
                                $("#DocIncCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach").show();
                                $('#aUIDNumber').attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks").val(this.Remarks);
                            }
                            $("#UDocAppId").val(this.DocAppId);
                            $("#UCreatedBy").val(this.CreatedBy);
                            $("#UIDDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocUIDCerAcceptedImg").show();
                                $("#DocUIDCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocUIDCerRejectedImg").show();
                                $("#DocUIDCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 6) {
                            $(".RuralUrban").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach").show();
                                $('#aRuralcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks").val(this.Remarks);
                            }
                            $("#RUDocAppId").val(this.DocAppId);
                            $("#RUCreatedBy").val(this.CreatedBy);
                            $("#RcerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocRurCerAcceptedImg").show();
                                $("#DocRurCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocRurCerRejectedImg").show();
                                $("#DocRurCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 7) {
                            $(".KanndaMedium").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach").show();
                                $('#aKannadamediumCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks").val(this.Remarks);
                            }
                            $("#KDocAppId").val(this.DocAppId);
                            $("#KCreatedBy").val(this.CreatedBy);
                            $("#KanMedCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocKanMedCerAcceptedImg").show();
                                $("#DocKanMedCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocKanMedCerRejectedImg").show();
                                $("#DocKanMedCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach").show();
                                $('#aDifferentlyabledcertificate').attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks").val(this.Remarks);
                            }
                            $("#DDocAppId").val(this.DocAppId);
                            $("#DCreatedBy").val(this.CreatedBy);
                            $("#DiffAblDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocDidAblCerAcceptedImg").show();
                                $("#DocDidAblCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocDidAblCerRejectedImg").show();
                                $("#DocDidAblCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 9) {
                            $(".ExemptedFromStudyCertificate").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach").show();
                                $('#aExemptedCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks").val(this.Remarks);
                            }
                            $("#ExDocAppId").val(this.DocAppId);
                            $("#ExCreatedBy").val(this.CreatedBy);
                            $("#ExCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExStuCerAcceptedImg").show();
                                $("#DocExStuCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExStuCerRejectedImg").show();
                                $("#DocExStuCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 10) {
                            $(".HyderabadKarnatakaRegion").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach").show();
                                $('#aHyderabadKarnatakaRegion').attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks").val(this.Remarks);
                            }
                            $("#HDocAppId").val(this.DocAppId);
                            $("#HCreatedBy").val(this.CreatedBy);
                            $("#HyKarDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHyKarRegAcceptedImg").show();
                                $("#DocHyKarRegRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHyKarRegRejectedImg").show();
                                $("#DocHyKarRegAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 11) {
                            $(".HoraNadu").prop('disabled', false);
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach").show();
                                $('#aHoranaaduKannadiga').attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks").val(this.Remarks);
                            }
                            $("#HGKDocAppId").val(this.DocAppId);
                            $("#HGKCreatedBy").val(this.CreatedBy);
                            $("#HorKanDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocHorGadKanAcceptedImg").show();
                                $("#DocHorGadKanRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocHorGadKanRejectedImg").show();
                                $("#DocHorGadKanAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 12) {
                            if (this.FilePath != null) {
                                $(".OtherCertificatesAttach").show();
                                $('#aOtherCertificates').attr('href', '' + this.FilePath + '');
                                $("#txtOtherCertRemarks").val(this.Remarks);
                            }
                            $("#ODocAppId").val(this.DocAppId);
                            $("#OCreatedBy").val(this.CreatedBy);
                            $("#OtherCerDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocOthCerAcceptedImg").show();
                                $("#DocOthCerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocOthCerRejectedImg").show();
                                $("#DocOthCerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach").show();
                                $('#aExserviceman').attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks").val(this.Remarks);
                            }
                            $("#ExSDocAppId").val(this.DocAppId);
                            $("#ExSCreatedBy").val(this.ExSCreatedBy);
                            $("#ExserDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocExSerAcceptedImg").show();
                                $("#DocExSerRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocExSerRejectedImg").show();
                                $("#DocExSerAcceptedImg").hide();
                            }
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach").show();
                                $('#aEWSCertificate').attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks").val(this.Remarks);
                            }
                            $("#EWSDocAppId").val(this.DocAppId);
                            $("#EWSCreatedBy").val(this.EWSCreatedBy);
                            $("#EWSDocStatus").val(this.Verified);
                            if (this.Verified == 15) {
                                $("#DocEWSAcceptedImg").show();
                                $("#DocEWSRejectedImg").hide();
                            }
                            else if (this.Verified == 3) {
                                $("#DocEWSRejectedImg").show();
                                $("#DocEWSAcceptedImg").hide();
                            }
                        }
                    });
                }

                $("#ApplicableReservations").prop('disabled', false);
                if ($("#EWSCertificate").prop('disabled') && $("#Exserviceman").prop('disabled')) {
                    $("#ApplicableReservations").prop('disabled', true);
                }

                $("#ApplStatus").val(datajson.Resultlist.ApplStatus);
                $("#academicyear1").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                $("#txtAadhaarNumber").val(datajson.Resultlist.AadhaarNumber);
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
                else {
                    if ($("input[name=StudyKannadaInOtherSt][value=1]").is(':checked')) {
                        $("input[name=HoraNadu][value=1]").prop('checked', true);
                    } else {
                        $("input[name=HoraNadu][value=0]").prop('checked', true);
                    }
                }
                   

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
                //else
                //    $("input[name=HyderabadKarnatakaRegion][value=0]").prop('checked', true);

                if (datajson.Resultlist.KanndaMedium == true) {
                    $("input[name=KanndaMedium][value=1]").prop('checked', true);
                    $("#KanMedCer").val(1);
                }
                else
                    $("input[name=KanndaMedium][value=0]").prop('checked', true);

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
                $("#txtInstituteStudied").val(datajson.Resultlist.InstituteStudiedQual);
                $("#txtMaximumMarks").val(datajson.Resultlist.MaxMarks);
                $("#txtMarksObtained").val(datajson.Resultlist.MarksObtained);
                $("#txtCGPA").val(datajson.Resultlist.EducationCGPA);
                if (datajson.Resultlist.BoardId != null)
                    $("#OtherBoards").val(datajson.Resultlist.BoardId);
                $("#txtMinMarks").val(datajson.Resultlist.MinMarks);
                if (datajson.Resultlist.Percentage != null && datajson.Resultlist.Percentage != "")
                    $("#lblPercAsPerMarks").text(datajson.Resultlist.Percentage + "%");
                $("#Results").val(datajson.Resultlist.ResultQual);
                TenthBoardStateType();
                if (datajson.Resultlist.studiedMathsScience == true)
                    $("input[name=studiedMathsScience][value=1]").prop('checked', true);
                else
                    $("input[name=studiedMathsScience][value=0]").prop('checked', true);
                //AddressDetails
                $("#txtCommunicationAddress").val(datajson.Resultlist.CommunicationAddress);
                $("#Districts").val(datajson.Resultlist.DistrictId);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $("#Talukas").empty();
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
                    $('#PermanentDistricts').empty();
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

                $("#academicyear2").text($("#academicyear1").val());
            }
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

$(document).ready(function () {
    $('#ModalFamilydb').modal('hide');
    $('input[name="rbtyesno"]').on('click', function () {

        if ($(this).val() == 'yes') {
            $('#trKutumba').show();
            $('#trKutumba2').hide();
            $('#sslcreg').hide();
        }
        if ($(this).val() == 'no') {
            $('#trKutumba').hide();
            $('#trKutumba2').show();
            $('#sslcreg').show();
            $('#txtKutumba').val('');
            /*window.open("https://kutumba.karnataka.gov.in/en/Index", "_blank");*/
        }
    });

    $('#btnGetKutumbaDetais').click(function () {

        $('#loading').show();
        $.ajax({
            type: "POST",
            url: "/Admission/GetKutumbaDetails",
            dataType: 'json',
            data: { KutumbaId: $('#txtKutumba').val()},
            success: function (data) {
                if (data != null) {

                    $('#loading').hide();
                    
                    //var _msg = d + " " + "Grievances Number Registered Successfully!!";
                    //bootbox.alert(_msg, function () {
                    //    window.location.href = "../CPGrams/Janaspandana_Grievance_Registration_View";
                    //});

                    $('#ModalFamilydb').modal('show');
                    $('#kutumbaIdModel').modal('hide');
                    //var family_list_data = d.Member_model_details;
                    //var Response_ID = d.Response_ID;
                   
                    $('#tblFamilydb').DataTable({
                        data: data,
                        "destroy": true,
                        "bSort": true,
                        columns: [
                            { 'data': 'MEMBER_NAME_ENG', 'title': 'MEMBER NAME', 'className': 'text-center' },
                            { 'data': 'FAMILY_ID', 'title': 'FAMILY ID', 'className': 'text-center' },
                           /* { 'data': 'MEMBER_ID', 'title': 'MEMBER ID', 'className': 'text-center' },*/
                            /*{ 'data': 'MBR_DOB', 'title': 'DOB', 'className': 'text-center' },*/
                            /*{ 'data': 'MBR_GENDER', 'title': 'GENDER', 'className': 'text-center' },*/
                            { 'data': 'MBR_NPR_FATHER_NAME', 'title': 'Father Name', 'className': 'text-center' },
                            { 'data': 'MBR_NPR_MOTHER_NAME', 'title': 'Mother Name', 'className': 'text-center' },
                            { 'data': 'MBR_EDUCATION_ID', 'title': 'SSLC Reg No.', 'className': 'text-center' },

                            { 'data': 'MBR_Disability_Applicant_No', 'title': 'UDID No.', 'className': 'text-center' },
                            { 'data': 'MBR_CASTE_RD_NO', 'title': 'Caste RD', 'className': 'text-center' },
                            { 'data': 'MBR_INCOME_RD_NO', 'title': 'Income RD No.', 'className': 'text-center' },
                            
                            
                            {
                                'data': 'MEMBER_NAME_ENG', 'title': 'Action', 'className': 'text-center',
                                "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                    //$(nTd).html("<a class='btn btn-primary' onclick='GetFamilyDetails(" + oData.MBR_EDUCATION_ID + "," + oData.MBR_Disability_Applicant_No + "," + oData.MBR_CASTE_RD_NO + "," + oData.MBR_INCOME_RD_NO + ")'> Select </a> ");
                                    $(nTd).html("<a class='btn btn-primary btnSelect'> Select </a> ");
                                }
                            },
                       
                        ]
                    });
                }

            }, error: function (e)
            {
                $('#loading').hide();
                var _msg = " No Record Found!! fill the application form manually..";

                bootbox.alert(_msg);

            }
        })
    });
    $('#btnGetSSLCDetails').click(function () {
        $('#loading').show()
        $.ajax({
            type: "POST",
            url: "/Admission/Get_SSLC_Details",
            dataType: 'json',
            data: { SSLC_Reg_No: $('#txtRollNumber').val() },
            success: function (data) {
                if (data.ApplicantName != null) {
                    $('#loading').hide()
                    
                    $('#txtApplicantName').val(data.ApplicantName);
                    $('#txtFathersName').val(data.ApplicantFatherName);
                    $('#txtMothersName').val(data.ApplicantMotherName);
                    if (data.Gender == "BOY") {
                        $('#Gender').val('1');
                    }
                    if (data.Gender == "GIRL") {
                        $('#Gender').val('2');
                    }

                    var dts = data.DOB.split("/");
                    //conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
                    dateofbirth = (dts[0] + "-" + dts[1] + "-" + dts[2]);

                    $('#dateofbirth').val(dateofbirth);

                    $("#txtMaximumMarks").val(data.SSLC_MaxMarks);
                    $("#txtMarksObtained").val(data.SSLC_Secured_Marks);
                    $("#lblPercAsPerMarks").text(data.SSLC_Percentage + "%");
                    $("#txtInstituteStudied").val(data.Applicant_School_Address);

                    if (data.SSLC_Results == "Pass") {
                        $("#Results").val(1);
                    }
                    if (data.SSLC_Results == "Fail") {
                        $("#Results").val(2);
                    }
                }
                else
                {
                    $('#loading').hide()
                    var _msg = "No Record Found!! fill the application form manually..";
                    bootbox.alert(_msg);
                    
                }

            }, error: function (e)
            {
                $('#loading').hide()
                var _msg = "No Record Found!! fill the application form manually..";
                bootbox.alert(_msg);
            }
        })
    });

    
    $('#btnCastRD').click(function () {

        GetRDNumbersDetails($('#txtCasteRd').val(),1);
        
    });
    
    $('#btnREWSRD').click(function () {

        GetRDNumbersDetails($('#txtEconomicWeakerRD').val(),2);
        
    }); 
    $('#btnHYDKarRD').click(function () {

        GetRDNumbersDetails($('#txtHYDKarRD').val(), 3);

    });
    $('#btnUDIDRd').click(function () {
        
       
        if ($('#txtUDIDRd').val()!="") {
            $('#divDisability1').show();
            $("input[name=PhysicallyHanidcapInd][value=1]").prop('checked', true);
            $('#btnUDIDRd').text('Verified');
            $('#btnUDIDRd').addClass('btn-success').removeClass('btn-primary');
            $('#Differentlyabledcertificate').attr('disabled', false);
        }

    });

    function GetRDNumbersDetails(RDNumbers, RDType) {
        
        $.ajax({
            type: "POST",
            url: "/Admission/Get_RD_Numbers_Details",
            dataType: 'json',
            data: { RDNumber: RDNumbers },
            success: function (data) {
                if (data != null) {
                    
                    if (RDType==1) {
                        $('#dvPassport1').show();
                        $('#txtFamilyAnnualIncome').val(data.AnnualIncome);
                        $('#txtCasteRDValidity').val(data.CertificateValidUpto);
                        $('#btnCastRD').text('Verified');
                        $('#btnCastRD').addClass('btn-success').removeClass('btn-primary ');
                        if (data.RDnumberValidity != null) {
                            bootbox.alert(data.RDnumberValidity);
                            $('#CastCatValidity').show();
                            $('#CastCatValidity').val(data.RDnumberValidity);
                            $('#CastCatValidity').addClass('btn-success').removeClass('btn-primary ');
                        }
                    }

                    if (RDType == 2) {
                        $('#dveconomic1').show();
                        $("input[name=EconomicallyWeakerSections][value=1]").prop('checked', true);
                        
                        $('#btnREWSRD').text('Verified');
                        $('#btnREWSRD').addClass('btn-success').removeClass('btn-primary ');
                        $('#EWSCertificate').attr('disabled', false);
                        $('#Incomecertificate').attr('disabled', false);
                    }
                    if (RDType == 3) {
                        $('#dvHKR1').show();
                        $("input[name=HyderabadKarnatakaRegion][value=1]").prop('checked', true);
                        $('#btnHYDKarRD').text('Verified');
                        $('#btnHYDKarRD').addClass('btn-success').removeClass('btn-primary ');
                        $('#HyderabadKarnatakaRegion').attr('disabled', false);
                    }
                
                  
                }

            }, error: function (e) {

                var _msg = "Something went wrong.";

                bootbox.alert(_msg);
            }
        })
    }

    $('#EduCertificate').attr('disabled', true);
    $('#CasteCertificate').attr('disabled', true);

    $('#Rationcard').attr('disabled', true);
    $('#Incomecertificate').attr('disabled', true);

    $('#UIDNumber').attr('disabled', true);
    $('#Ruralcertificate').attr('disabled', true);

    $('#KannadamediumCertificate').attr('disabled', true);
    $('#Differentlyabledcertificate').attr('disabled', true);

    $('#ExemptedCertificate').attr('disabled', true);
    $('#HyderabadKarnatakaRegion').attr('disabled', true);
    $('#HoranaaduKannadiga').attr('disabled', true);

    $('#Exserviceman').attr('disabled', true);
    $('#EWSCertificate').attr('disabled', true);

    $('#PhotoUpload').change(function () {
    
        fileSize = this.files[0].size;

        var maxSizeKB = 50; //Size in KB
        var minSizeKB = 10; //Size in KB
        var maxSize = maxSizeKB * 1024;
        var minSize = minSizeKB * 1024;
        if (fileSize > maxSize) {
           /* $("#ModalImagePriview").modal('hide');*/
            bootbox.alert("Photo size shoulb be >greater than 10kb, less than 50kb");
            $('#PhotoUpload').val('');
        }
        if (fileSize < minSize) {
           /* $("#ModalImagePriview").modal('hide');*/
            bootbox.alert("Photo size shoulb be >greater than 10kb, less than 50kb");
            $('#PhotoUpload').val('');
        }

        //alert(sizeInMb);
    });

});

$('input[type=radio][name=RAppBasics]').change(function () {
    
    //$('#kutumbaIdModel').modal('show');
    var selectedQualification = $('input[name="RAppBasics"]:checked').val();
    $('#Qualification').val(selectedQualification);
    if (selectedQualification == 2 || selectedQualification == 3) {
        bootbox.alert("Please enter the SSLC registration number to fetch the basic details and SSLC results data.");
    } else {
        $('input[name="TenthBoard"]').attr('disabled', true);
        $('input[name="TenthBoard"]').prop('checked', false)
        bootbox.alert("Please fill the Application form manually !!");
    }
});

//$('input[type=radio][name=ParentGovtEE]').change(function () {
//    
//    $('#RStateBoardType2').prop('checked', true);
    
   
//});

$('input[type=radio][name=RuralUrban]').change(function () {
    

    var selectedRuralUrban = $('input[name="RuralUrban"]:checked').val();
    if (selectedRuralUrban == 1) {
        $('#Ruralcertificate').attr('disabled', false);
        $("#ExemptedCertificate").attr("disabled", true);
        $('#ExemptedCertificate').attr('disabled', true);
        bootbox.alert("Please upload Relavent documents in documents section");
    }
    else {
        $('#Ruralcertificate').attr('disabled', true);
        $("#ExemptedCertificate").attr("disabled", false);
        $('#ExemptedCertificate').attr('disabled', false);
    }
});

$('input[type=radio][name=RStateBoardType]').change(function () {
    
    var selectedQualification = $('input[name="RStateBoardType"]:checked').val();
    if (selectedQualification == 1) {

        $("input[name=TenthBoard][value=1]").prop('checked', true);
    }
    if (selectedQualification == 0) {
        $("input[name=TenthBoard][value=4]").prop('checked', true);
    }
});

$('input[type=radio][name=HyderabadKarnatakaRegion]').change(function () {
    
    var selectedHybKarnataka = $('input[name="HyderabadKarnatakaRegion"]:checked').val();
    if (selectedHybKarnataka == 1) {
        $('#HyderabadKarnatakaRegion').attr('disabled', false);
        bootbox.alert("Please upload Relavent documents in documents section");
        $("#dvHKR").show();
    }
    if (selectedHybKarnataka == 0) {
        $('#HyderabadKarnatakaRegion').attr('disabled', true);
        $("#dvHKR").hide();
    }
});
$('input[type=radio][name=KanndaMedium]').change(function () {
    
    var selectedKannadaMedium = $('input[name="KanndaMedium"]:checked').val();
    if (selectedKannadaMedium == 1) {
        var msg = "Please upload Relavent documents in documents section"
        bootbox.alert(msg);
        $('#KannadamediumCertificate').attr('disabled', false);
    }
    if (selectedKannadaMedium == 0) {
        $('#KannadamediumCertificate').attr('disabled', true);
    }
});
//$('input[type=radio][name=HoraNadu]').change(function () {
//    
//    var selectedHoraNadu = $('input[name="HoraNadu"]:checked').val();
//    if (selectedHoraNadu == 1) {
//        var msg = "Please upload Relavent documents in documents section"
//        bootbox.alert(msg);
//        $('#HoranaaduKannadiga').attr('disabled', false);
//    }
//    if (selectedHoraNadu == 0) {
//        $('#HoranaaduKannadiga').attr('disabled', true);
//    }
//});

$('input[type=radio][name=ExService]').change(function () {
    
    var selectedExService = $('input[name="ExService"]:checked').val();
    if (selectedExService == 1) {
        var msg = "Please upload Relavent documents in documents section"
        bootbox.alert(msg);
        $('#Exserviceman').attr('disabled', false);
    }
    if (selectedExService == 0) {
        $('#Exserviceman').attr('disabled', true);
    }
});

$('input[type=radio][name=EconomicallyWeakerSections]').change(function () {
    
    var selectedWeakerSection = $('input[name="EconomicallyWeakerSections"]:checked').val();
    if (selectedWeakerSection == 1) {
        $('#EWSCertificate').attr('disabled', false);
        $('#dveconomic').show();
        bootbox.alert("Please upload Relavent documents in documents section");
      
    }
    if (selectedWeakerSection == 0) {
        $('#EWSCertificate').attr('disabled', true);
        $('#dveconomic').hide();
    }
});
$('input[type=radio][name=PhysicallyHanidcapInd]').change(function () {
    
    var selectedWeakerSection = $('input[name="PhysicallyHanidcapInd"]:checked').val();
    if (selectedWeakerSection == 1) {
        $('#Differentlyabledcertificate').attr('disabled', false);
        $('#divDisability').show();
    }
    if (selectedWeakerSection == 0) {
        $('#Differentlyabledcertificate').attr('disabled', true);
        $('#divDisability').hide();
    }
});
$('input[type=radio][name=ExemptedFromStudyCertificate]').change(function () {
    
    var selectedWeakerSection = $('input[name="ExemptedFromStudyCertificate"]:checked').val();
    if (selectedWeakerSection == 1) {
        var msg = "Please upload Relavent documents in documents section"
        bootbox.alert(msg);

        $('#ExemptedCertificate').attr('disabled', false);
        
    }
    if (selectedWeakerSection == 0) {
        $('#ExemptedCertificate').attr('disabled', true);
     
    }
});



function GetFamilyDetails(val, val2, val3,val4) {
    
    //$.ajax({
    //    url: "/Admission/Get_Family_Details",
    //    method: "POST",
    //    data: {
    //        Family_Id: val,
    //        Family_Member_Id: val2,
    //        ResponseId: val3
    //    },

    //    success: function (response) {
            
    //        if (response != null) {
    //            $('#ModalFamilydb').modal('hide');
    //            $('#kutumbaIdModel').modal('hide');

    //            $('#txtApplicantName').val('sdfsfs');
    //            $('#txtFathersName').val('sdfsfs');
    //            $('#dateofbirth').val('02-04-1989');
    //            $('#txtMothersName').val('sdfsfs');
    //            $('#Gender').val('1');
    //            $('#Category').val('1');
    //            $('#Religion').val('2');
    //            $('#MinorityCategory').val('1');
    //            $("#txtMaximumMarks").val(600);
    //            $("#txtMarksObtained").val(350);
    //            $("#Results").val(1);
                
    //        }
    //    }
    //})
}

$("#tblFamilydb").on('click', '.btnSelect', function () {
    
    // get the current row
    var currentRow = $(this).closest("tr");
    var SSLCNo = currentRow.find("td:eq(4)").text(); // get current row 1st TD value
    var UDIDNo = currentRow.find("td:eq(5)").text(); // get current row 1st TD value
    var CasteIncomeRD = currentRow.find("td:eq(6)").text(); // get current row 1st TD value
    
    //var action_taken = $(this).closest('tr').find('input[name="response"]').val();
    
    $('#txtRollNumber').val(SSLCNo);
    $('#txtCasteRd').val(CasteIncomeRD);
    $('#txtUDIDRd').val(UDIDNo);
    $('#ModalFamilydb').modal('hide');

    if (CasteIncomeRD != "") {
        $("#dvPassport").show();
        $("#dvPassport1").hide();
        $("#chkYes1").prop('checked', true);
        $('#txtCasteRd').attr('readonly', 'true');
    }
    else {
        $("#dvPassport").hide();
    }
    if (UDIDNo != "") {
        $("#divDisability").show();
        $("#divDisability1").hide();
        $("#chkDyes").prop('checked', true);
        $('#txtUDIDRd').attr('readonly', 'true');
    }
    else {
        $("#divDisability").hide();
    }

    $('#btnCastRD').text('Verify');
    $('#btnCastRD').addClass('btn-primary').removeClass('btn-success');
    $('#btnREWSRD').text('Verify');
    $('#btnREWSRD').addClass('btn-primary').removeClass('btn-success');

    $('#btnHYDKarRD').text('Verify');
    $('#btnHYDKarRD').addClass('btn-primary').removeClass('btn-success');
    $('#btnUDIDRd').text('Verify');
    $('#btnUDIDRd').addClass('btn-primary').removeClass('btn-success');

    if (SSLCNo == "") {
        bootbox.alert("Please enter SSLC Register no. manually and upload the relevant document under documents section!!");
        $("#EduCertificate").attr("disabled", false);
    }
});