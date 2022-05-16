$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');
    
    $("#saveDocumentsRemarksDetDiv1").hide();
    $("#saveDocumentsRemarksDetDiv2").hide();
    $("#saveDocumentsRemarksDetDiv3").hide();
    $("#saveDocumentsRemarksDetDiv4").hide();
    clearallErrorFields1();
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
        }
    });

    $('#dateofbirth1').datepicker({

        yearRange: "-100:-18",
        changeMonth: true,
        changeYear: true,
        onSelect: function (selected) {
            var dob = new Date(selected);
            var today = new Date();
            var diff = today.getFullYear() - dob.getFullYear();
            console.log(diff);
            if (diff < 20) {
                alert("Your Age should be more then 20+");
                $("#dateofbirth1").val('');
            }
        },
        dateFormat: 'dd-mm-yy'
    });

    $("#myModalGri").hide();
    $('#HistoryRemarksCommentsModal').hide();
    $("#CalenderNotificationGriEligibleInd").val(0);
    GetEligibleDateFrmCalenderGriEvents();
    //GetApplicantType();
    GetGrievanceStatus();
    //GetDocumentApplicationStatus();
});

function GetEligibleDateFrmCalenderGriEvents() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetEligibleDateFrmCalenderEvents',
        success: function (datajson) {
            var frmDate = ""; var toDate = ""; var dte = new Date();
            var curDate = dte.getUTCDate();
            var CurMon = dte.getUTCMonth();
            var CurYear = dte.getUTCFullYear();
            $.each(datajson, function () {

                if (this.From_date_VerificationOfficer != null && this.To_date_VerificationOfficer != null) {
                    frmDate = this.From_date_VerificationOfficer.split(',');
                    toDate = this.To_date_VerificationOfficer.split(',');

                    if (new Date(parseInt(frmDate[0]), parseInt(frmDate[1]) - 1, parseInt(frmDate[2])) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))
                        && new Date(CurYear, CurMon, curDate) <= new Date(parseInt(toDate[0]), parseInt(toDate[1]) - 1, parseInt(toDate[2]))) {
                        $("#CalenderNotificationGriEligibleInd").val(1);
                        $("#EligibleGrievanceVerificationForm").hide();
                    }
                    else {
                        $("#CalenderNotificationGriEligibleInd").val(0);
                        $("#saveDocumentsRemarksDetDiv1").hide();
                        $("#saveDocumentsRemarksDetDiv2").hide();
                        $("#saveDocumentsRemarksDetDiv3").hide();
                        $("#saveDocumentsRemarksDetDiv4").hide();
                        $("#EligibleGrievanceVerificationForm").show();
                    }
                }
                return false;
            });
        }
    });
}

function CalculatePercentage(IsValidVal) {

    var IsValidP = IsValidVal;
    $("#CalculationMaximumMarks-Required").hide();
    var MaximumMarks = $("#txtMaximumMarks1").val();
    var MarksObtained = $("#txtMarksObtained1").val();
    var MinMarks = $("#txtMinMarks1").val();
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

    if (MinMarks > MaximumMarks) {
        $("#txtMinMarks1-Required").show();
        IsValidP = false;
    }

    if (PercentageErr) {
        $("#CalculationMaximumMarks1-Required").show();
        IsValidP = false;
    }
    FullPercVal = Percentage.toFixed(2);
    $("#lblPercAsPerMarks1").text(FullPercVal + "%");

    return {
        IsValidP,
        FullPercVal
    };
}

function UpdateStatus(updatedMsg) {

    var ApplicantId = $("#ApplicantId").val();
    var IsValid = true;

    var FathersName = $("#txtFathersName1").val();
    $("#txtFathersName1-Required").hide();
    if ($("#txtFathersName1").val() == "") {
        $("#txtFathersName1-Required").show();
        IsValid = false;
    }

    $("#Gender1-Required").hide();
    var Gender = $("#Gender1 :selected").val();
    if (Gender == 0) {
        $("#Gender1-Required").show();
        IsValid = false;
    }

    var dateofbirth = "";
    $("#dateofbirth1-Required").hide();
    var dateofbirth = $("#dateofbirth1").val();
    if (dateofbirth == "") {
        $("#dateofbirth1-Required").show();
        IsValid = false;
    }
    else {
        var dts = $("#dateofbirth1").val().split("-");
        conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
        dateofbirth = (dts[0] + "-" + dts[1] + "-" + dts[2]);
    }
    
    $("#Category1-Required").hide();
    var Category = $("#Category1 :selected").val();
    if (Category == 0) {
        $("#Category1-Required").show();
        IsValid = false;
    }

    $("#txtMaximumMarks1-Required").hide();
    var MaxMarks = $("#txtMaximumMarks1").val();
    if (MaxMarks == 0) {
        $("#txtMaximumMarks1-Required").show();
        IsValid = false;
    }

    $("#txtMarksObtained1-Required").hide();
    var MarksObtained = $("#txtMarksObtained1").val();
    if (MarksObtained == 0) {
        $("#txtMarksObtained1-Required").show();
        IsValid = false;
    }

    $("#txtMinMarks1-Required").hide();
    var MinMarks = $("#txtMinMarks1").val();
    if (MinMarks == 0) {
        $("#txtMinMarks1-Required").show();
        IsValid = false;
    }

    $("#Results1-Required").hide();
    var ResultQual = $("#Results1 :selected").val();
    if (ResultQual == 0) {
        $("#Results1-Required").show();
        IsValid = false;
    }

    let PercentageReturnValue = CalculatePercentage(IsValid);

    IsValidP = PercentageReturnValue.IsValidP,
        Percentage = PercentageReturnValue.FullPercVal;

    if (IsValidP == false) {
        IsValid = false
    }

    var ApplicantBelongTo = $('input[name=RuralUrban1]:checked').val();

    var IsApplicableReservationValEx = 0;
    var IsApplicableReservationValEWS = 0;
    $('#ApplicableReservations1 :selected').each(function (i, selected) {
        if ($(selected).val() == 2) {
            IsApplicableReservationValEx = 1;
        }
        else if ($(selected).val() == 5) {
            IsApplicableReservationValEWS = 1;
        }
    });

    if (IsValid) {

        var fileData = new FormData();
        var fileUpload = "";

        var EDocAppIdVal = $("#EDocAppId").val();
        var CDocAppIdVal = $("#CDocAppId").val();
        var RDocAppIdVal = $("#RDocAppId").val();
        var IDocAppIdVal = $("#IDocAppId").val();
        var UDocAppIdVal = $("#UDocAppId").val();
        var RUDocAppIdVal = $("#RUDocAppId").val();
        var DDocAppIdVal = $("#DDocAppId").val();
        var ExDocAppIdVal = $("#ExDocAppId").val();
        var HDocAppIdVal = $("#HDocAppId").val();
        var HGKDocAppIdVal = $("#HGKDocAppId").val();
        var ODocAppIdVal = $("#ODocAppId").val();
        var ExSDocAppIdVal = $("#ExSDocAppId").val();
        var EWSDocAppIdVal = $("#EWSDocAppId").val();

        var ECreatedByVal = $("#ECreatedBy").val();
        var CCreatedByVal = $("#CCreatedBy").val();
        var RCreatedByVal = $("#RCreatedBy").val();
        var ICreatedByVal = $("#ICreatedBy").val();
        var UCreatedByVal = $("#UCreatedBy").val();
        var RUCreatedByVal = $("#RUCreatedBy").val();
        var DCreatedByVal = $("#DCreatedBy").val();
        var ExCreatedByVal = $("#ExCreatedBy").val();
        var HCreatedByVal = $("#HCreatedBy").val();
        var HGKCreatedByVal = $("#HGKCreatedBy").val();
        var OCreatedByVal = $("#OCreatedBy").val();
        var ExSCreatedByVal = $("#ExSCreatedBy").val();
        var EWSCreatedByVal = $("#EWSCreatedBy").val();

        var EduCertRemarks = $.trim($("#txtEduCertRemarks1").val());
        var CasteCertRemarks = $.trim($("#txtCasteCertRemarks1").val());
        var UIDRemarks = $.trim($("#txtUIDRemarks1").val());
        var RurCertRemarks = $.trim($("#txtRurCertRemarks1").val());
        var DiffAbledCertRemarks = $.trim($("#txtDiffAbledCertRemarks1").val());
        var HydKarnRemarks = $.trim($("#txtHydKarnRemarks1").val());
        var ExservicemanRemarks = $.trim($("#txtExservicemanRemarks1").val());
        var EWSCertificateRemarks = $.trim($("#txtEWSCertificateRemarks1").val());

        var EduDocStatus = $("#EduDocStatus1 :selected").val();
        var CasDocStatus = $("#CasDocStatus1 :selected").val();
        var UIDDocStatus = $("#UIDDocStatus1 :selected").val();
        var RUcerDocStatus = $("#RcerDocStatus1 :selected").val();
        var DiffAblDocStatus = $("#DiffAblDocStatus1 :selected").val();
        var HyKarDocStatus = $("#HyKarDocStatus1 :selected").val();
        var ExserDocStatus = $("#ExserDocStatus1 :selected").val();
        var EWSDocStatus = $("#EWSDocStatus1 :selected").val();

        //EduCertificate
        $("#WithoutDocEduStatus1-Required").hide();
        $("#WithoutDocEduRemarks1-Required").hide();
        var MaxMarks = $("#txtMaximumMarks1").val();
        var MarksObtained = $("#txtMarksObtained1").val();
        var MinMarks = $("#txtMinMarks1").val();
        var ResultQual = $("#Results1 :selected").val();
        $("#EduCertificate1-Required").hide();
        if (!$('#EduCertificate1').prop('disabled')) {
            if ($("#EduCertificate1").val() != "") {
                fileUpload = $('#EduCertificate1').get(0);
                if (EduDocStatus == 0) {
                    $("#WithoutDocEduStatus1-Required").show();
                    $("#EduDocStatus1").focus();
                    IsValid = false;
                }
                if (EduCertRemarks == "") {
                    $("#WithoutDocEduRemarks1-Required").show();
                    $("#txtRurCertRemarks1").focus();
                    IsValid = false;
                }

                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("EduCertificatePDF", files[i]);
                }
            }
            else if ($("#EduCertificate1").val() == "") {
                IsValid = false;
                $("#EduCertificate1").focus();
                $("#EduCertificate1-Required").show();
            }
        }

        //CasteCertificate
        $("#WithoutDocCasStatus1-Required").hide();
        $("#WithoutDocCasRemarks1-Required").hide();
        var Category = $("#Category1 :selected").val();
        var Caste = $("#txtCaste1 :selected").val();
        $("#CasteCertificate1-Required").hide();
        if (!$('#CasteCertificate1').prop('disabled')) {
            if ($("#CasteCertificate1").val() != "") {
                if (CasDocStatus == 0) {
                    $("#WithoutDocCasStatus1-Required").show();
                    $("#CasDocStatus1").focus();
                    IsValid = false;
                }
                if (CasteCertRemarks == "") {
                    $("#WithoutDocCasRemarks1-Required").show();
                    $("#txtCasteCertRemarks1").focus();
                    IsValid = false;
                }

                fileUpload = $('#CasteCertificate1').get(0);
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("CasteCertificatePDF", files[i]);
                }
            }
            else if ($("#CasteCertificate1").val() == "") {
                IsValid = false;
                $("#CasteCertificate1").focus();
                $("#CasteCertificate1-Required").show();
            }
        }

        //UIDCertificate
        $("#WithoutDocIncStatus1-Required").hide();
        $("#WithoutDocIncRemarks1-Required").hide();
        fileUpload = $('#UIDNumber1').get(0);
        var FathersName = $("#txtFathersName1").val();
        var dateofbirth = $("#dateofbirth1").val();
        var Gender = $("#Gender1 :selected").val();
        $("#UIDNumber1-Required").hide();
        if (!$('#UIDNumber1').prop('disabled')) {
            if ($("#UIDNumber1").val() != "") {
                if (UIDDocStatus == 0) {
                    $("#WithoutDocIncStatus1-Required").show();
                    $("#UIDDocStatus1").focus();
                    IsValid = false;
                }
                if (UIDRemarks == "") {
                    $("#WithoutDocIncRemarks1-Required").show();
                    $("#txtUIDRemarks1").focus();
                    IsValid = false;
                }

                fileUpload = $('#UIDNumber1').get(0);
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("UIDNumberPDF", files[i]);
                }
            }
            else if ($("#UIDNumber1").val() == "") {
                IsValid = false;
                $("#UIDNumber1").focus();
                $("#UIDNumber1-Required").show();
            }
        }

        //Rural Certificate
        var ApplicantBelongTo = $('input[name=RuralUrban1]:checked').val();
        $("#WithoutDocRurStatus1-Required").hide();
        $("#WithoutDocRurRemarks1-Required").hide();
        $("#Ruralcertificate1-Required").hide();
        if (!$('#Ruralcertificate1').prop('disabled')) {
            if ($("#Ruralcertificate1").val() != "") {
                if ($("#RcerDocStatus1 :selected").val() == 0) {
                    $("#WithoutDocRurStatus1-Required").show();
                    $("#RcerDocStatus1").focus();
                    IsValid = false;
                }
                if (RurCertRemarks == "") {
                    $("#WithoutDocRurRemarks1-Required").show();
                    $("#txtRurCertRemarks1").focus();
                    IsValid = false;
                }

                if (RUcerDocStatus == 15) {
                    ApplicantBelongTo = 1;
                }
                else if (RUcerDocStatus == 3) {
                    ApplicantBelongTo = 2;
                }
                fileUpload = $('#Ruralcertificate1').get(0);
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("RuralPDF", files[i]);
                }
            }
            else if ($("#Ruralcertificate1").val() == "") {
                IsValid = false;
                $("#Ruralcertificate1").focus();
                $("#Ruralcertificate1-Required").show();
            }
        }

        //DifferentlyAbledPDF
        var PhysicallyHanidcapInd = $('input[name=PhysicallyHanidcapInd1]:checked').val();
        var PhysicallyHanidcapType = $("#PhysicallyHanidcapType1 :selected").val();
        $("#WithoutDocDAStatus1-Required").hide();
        $("#WithoutDocDARemarks1-Required").hide();
        $("#PersonWithDisability1-Required").hide();
        $("#Differentlyabledcertificate1-Required").hide();
        if (!$('#Differentlyabledcertificate1').prop('disabled')) {
            if ($("#Differentlyabledcertificate1").val() != "") {
                if (DiffAblDocStatus == 0) {
                    $("#WithoutDocDAStatus1-Required").show();
                    $("#RcerDocStatus1").focus();
                    IsValid = false;
                }
                if (DiffAbledCertRemarks == "") {
                    $("#WithoutDocDARemarks1-Required").show();
                    $("#txtDiffAbledCertRemarks1").focus();
                    IsValid = false;
                }

                fileUpload = $('#Differentlyabledcertificate1').get(0);
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("DifferentlyAbledPDF", files[i]);
                }
            }
            else if ($("#Differentlyabledcertificate1").val() == "") {
                IsValid = false;
                $("#Differentlyabledcertificate1").focus();
                $("#Differentlyabledcertificate1-Required").show();
            }
            if (DiffAblDocStatus == 15) {
                PhysicallyHanidcapInd = 1;
                $("input[name=PhysicallyHanidcapInd1][value=1]").prop('checked', true);
                PhysicallyHanidcapEna1(PhysicallyHanidcapInd);
                if ($("#PhysicallyHanidcapType1 :selected").val() == 0) {
                    IsValid = false;
                    $("#PersonWithDisability1-Required").show();
                    $("#PhysicallyHanidcapType1").focus();
                }
            }
            else if (DiffAblDocStatus == 3) {
                PhysicallyHanidcapInd = 0;
            }
        }
        PhysicallyHanidcapInd == "1" ? PhysicallyHanidcapInd = true : PhysicallyHanidcapInd = false;

        //HyderabadKarnatakaRegion        
        $("#WithoutDocHKRStatus1-Required").hide();
        $("#WithoutDocHKRRemarks1-Required").hide();
        $("#HyderabadKarnatakaRegion1-Required").hide();
        var HyderabadKarnatakaRegion = $('input[name=HyderabadKarnatakaRegion1]:checked').val();
        if (!$('#HyderabadKarnatakaRegion1').prop('disabled')) {
            if ($("#HyderabadKarnatakaRegion1").val() != "") {
                if (HyKarDocStatus == 0) {
                    $("#WithoutDocHKRStatus1-Required").show();
                    $("#HyKarDocStatus1").focus();
                    IsValid = false;
                }
                if (HydKarnRemarks == "") {
                    $("#WithoutDocHKRRemarks1-Required").show();
                    $("#txtHydKarnRemarks1").focus();
                    IsValid = false;
                }

                if (HyKarDocStatus == 15) {
                    HyderabadKarnatakaRegion = 1;
                }
                else if (HyKarDocStatus == 3) {
                    HyderabadKarnatakaRegion = 0;
                }
                fileUpload = $('#HyderabadKarnatakaRegion1').get(0);
                var files = fileUpload.files;
                for (var i = 0; i < files.length; i++) {
                    fileData.append("HyderabadKarnatakaRegionPDF", files[i]);
                }
            }
            else if ($("#HyderabadKarnatakaRegion1").val() == "") {
                IsValid = false;
                $("#HyderabadKarnatakaRegion1").focus();
                $("#HyderabadKarnatakaRegion1-Required").show();
            }
        }
        HyderabadKarnatakaRegion == "1" ? HyderabadKarnatakaRegion = true : HyderabadKarnatakaRegion = false;

        //Exserviceman
        $("#WithoutDocExSerStatus1-Required").hide();
        $("#WithoutDocExSerRemarks1-Required").hide();
        $("#Exserviceman1-Required").hide();
        if (!$('#Exserviceman1').prop('disabled')) {
            if ($("#Exserviceman1").val() != "") {
                if (ExserDocStatus == 0) {
                    $("#WithoutDocExSerStatus1-Required").show();
                    $("#ExserDocStatus1").focus();
                    IsValid = false;
                }
                if (ExservicemanRemarks == "") {
                    $("#WithoutDocExSerRemarks1-Required").show();
                    $("#txtExservicemanRemarks1").focus();
                    IsValid = false;
                }
                if (IsValid) {
                    fileUpload = $('#Exserviceman1').get(0);
                    var files = fileUpload.files;
                    for (var i = 0; i < files.length; i++) {
                        fileData.append("ExservicemanPDF", files[i]);
                    }
                }
            }
            else if ($("#Exserviceman1").val() == "") {
                IsValid = false;
                $("#Exserviceman1").focus();
                $("#Exserviceman1-Required").show();
            }
        }

        //EWSCertificate
        $("#WithoutDocEWSStatus1-Required").hide();
        $("#WithoutDocEWSRemarks1-Required").hide();
        $("#EWSCertificate1-Required").hide();
        if (!$('#EWSCertificate1').prop('disabled')) {
            if ($("#EWSCertificate1").val() != "") {
                if (EWSDocStatus == 0) {
                    $("#WithoutDocEWSStatus1-Required").show();
                    $("#EWSDocStatus1").focus();
                    IsValid = false;
                }
                if (EWSCertificateRemarks == "") {
                    $("#WithoutDocEWSRemarks1-Required").show();
                    $("#txtEWSCertificateRemarks1").focus();
                    IsValid = false;
                }
                if (IsValid) {
                    fileUpload = $('#EWSCertificate1').get(0);
                    var files = fileUpload.files;
                    for (var i = 0; i < files.length; i++) {
                        fileData.append("EWSCertificatePDF", files[i]);
                    }
                }
            }
            else if ($("#EWSCertificate1").val() == "") {
                IsValid = false;
                $("#EWSCertificate1").focus();
                $("#EWSCertificate1-Required").show();
            }
        }

        fileData.append(
            "ApplicantId", ApplicantId
        );
        fileData.append(
            "MaxMarks", MaxMarks
        );
        fileData.append(
            "MarksObtained", MarksObtained
        );
        fileData.append(
            "MinMarks", MinMarks
        );
        fileData.append(
            "Percentage", Percentage,
        );
        fileData.append(
            "ResultQual", ResultQual
        );
        fileData.append(
            "Category", Category
        );
        fileData.append(
            "Caste", Caste
        );
        fileData.append(
            "ApplicantBelongTo", ApplicantBelongTo
        );
        fileData.append(
            "FathersName", FathersName
        );
        fileData.append(
            "Gender", Gender
        );
        fileData.append(
            "DOB", dateofbirth
        );
        fileData.append(
            "PhysicallyHanidcapInd", PhysicallyHanidcapInd
        );
        fileData.append(
            "PhysicallyHanidcapType", PhysicallyHanidcapType
        );
        fileData.append(
            "HyderabadKarnatakaRegion", HyderabadKarnatakaRegion
        );
        if ($("#ExserDocStatus1 :selected").val() == 15) {
            fileData.append(
                "ApplicableReservations", 2
            );
        }
        if ($("#EWSDocStatus1 :selected").val() == 15) {
            fileData.append(
                "ApplicableReservations", 5
            );
        }


        fileData.append(
            "EduCertificateRemarks", EduCertRemarks
        );
        fileData.append(
            "CasteCertificateRemarks", CasteCertRemarks
        );
        fileData.append(
            "UIDNumberRemarks", UIDRemarks
        );
        fileData.append(
            "RuralRemarks", RurCertRemarks
        );
        fileData.append(
            "DifferentlyAbledRemarks", DiffAbledCertRemarks
        );
        fileData.append(
            "HyderabadKarnatakaRegionRemarks", HydKarnRemarks
        );
        fileData.append(
            "ExservicemanRemarks", ExservicemanRemarks
        );
        fileData.append(
            "EWSCertificateRemarks", EWSCertificateRemarks
        );


        fileData.append(
            "EduDocStatus", EduDocStatus
        );
        fileData.append(
            "CasDocStatus", CasDocStatus
        );
        fileData.append(
            "UIDDocStatus", UIDDocStatus
        );
        fileData.append(
            "RUcerDocStatus", RUcerDocStatus
        );
        fileData.append(
            "DiffAblDocStatus", DiffAblDocStatus
        );
        fileData.append(
            "HyKarDocStatus", HyKarDocStatus
        );
        fileData.append(
            "ExserDocStatus", ExserDocStatus
        );
        fileData.append(
            "EWSDocStatus", EWSDocStatus
        );


        fileData.append(
            "EDocAppIdVal", EDocAppIdVal
        );
        fileData.append(
            "CDocAppIdVal", CDocAppIdVal
        );
        fileData.append(
            "UDocAppIdVal", UDocAppIdVal
        );
        fileData.append(
            "RUDocAppIdVal", RUDocAppIdVal
        );
        fileData.append(
            "DDocAppId", DDocAppIdVal
        );
        fileData.append(
            "HDocAppIdVal", HDocAppIdVal
        );
        fileData.append(
            "EWSDocAppId", EWSDocAppIdVal
        );
        fileData.append(
            "ExSDocAppId", ExSDocAppIdVal
        );


        fileData.append(
            "ECreatedByVal", ECreatedByVal
        );
        fileData.append(
            "CCreatedByVal", CCreatedByVal
        );
        fileData.append(
            "UCreatedByVal", UCreatedByVal
        );
        fileData.append(
            "RUCreatedByVal", RUCreatedByVal
        );
        fileData.append(
            "HCreatedByVal", HCreatedByVal
        );
        fileData.append(
            "DCreatedBy", DCreatedByVal
        );
        fileData.append(
            "EWSCreatedBy", EWSCreatedByVal
        );
        fileData.append(
            "ExSCreatedBy", ExSCreatedByVal
        );
        fileData.append(
            "Remarks", $("#txtRemarks1").val()
        );


        var objGrivanceAppliedVal = {
            "1": $("#EduGrivApplied").val(),
            "2": $("#CasteGrivApplied").val(),
            "5": $("#UIDGrivApplied").val(),
            "6": $("#RuralGrivApplied").val(),
            "8": $("#DiffAbledGrivApplied").val(),
            "10": $("#HydKarnGrivApplied").val(),
            "14": $("#ExServGrivApplied").val(),
            "16": $("#EWSGrivApplied").val()
        };

        $.each(objGrivanceAppliedVal, function (key, value) {
            fileData.append(
                "DocumentSet", value
            );
        });

        if (IsValid) {
            bootbox.confirm("<br><br>Are you sure to update the documents?", function (confirmed) {
                if (confirmed) {
                    $.ajax({
                        type: 'POST',
                        url: '/Admission/ApplicantGrievanceDetails',
                        contentType: false,
                        processData: false,
                        data: fileData,
                        success: function (datajson) {
                            if (datajson.UpdateMsg == "success") {
                                bootbox.alert("<br><br>Grievance Documents status, Remarks and Data updated Successfully by Verification Officer");

                                GetLoadApplicantGrievanceGrid();
                                GetGrievanceAgainstTentativeList();
                                GetFileTypes();
                                GetGrievanceTentativeStatus();
                                $("#myModalGri").modal("hide");
                            }
                            else {
                                bootbox.alert("<br><br>Documents status and Remarks failed to Updated");
                            }

                        }, error: function (result) {
                            bootbox.alert("Error", "something went wrong");
                        }
                    });
                }
            });
        }
    }
}

function GetGrievanceFileType(grivanceId) {
    $.ajax({
        type: "GET",
        url: "/Admission/EditApplicantGrievance",
        contentType: "application/json",
        data: { 'grivanceId': grivanceId },
        success: function (data) {
            if (data != null) {
                for (var i = 0; i < data.FileNames.length; i++) {

                    if (data.FileTypes[i] == 1) {
                        $("#txtFathersName1").prop('disabled', false);
                        $("#UIDNumber1").prop('disabled', false);
                        $("#UIDDocStatus1").prop('disabled', false);
                        $("#txtUIDRemarks1").prop('disabled', false);
                        $("#UIDGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 2) {
                        $("#Gender1").prop('disabled', false);
                        $("#UIDNumber1").prop('disabled', false);
                        $("#UIDDocStatus1").prop('disabled', false);
                        $("#txtUIDRemarks1").prop('disabled', false);
                        $("#UIDGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 3) {
                        $("#dateofbirth1").prop('disabled', false);
                        $("#UIDNumber1").prop('disabled', false);
                        $("#UIDDocStatus1").prop('disabled', false);
                        $("#txtUIDRemarks1").prop('disabled', false);
                        $("#UIDGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 4) {
                        $("#Category1").prop('disabled', false);
                        $("#txtCaste1").prop('disabled', false);
                        $("#CasteCertificate1").prop('disabled', false);
                        $("#CasDocStatus1").prop('disabled', false);
                        $("#txtCasteCertRemarks1").prop('disabled', false);
                        $("#CasteGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 5) {
                        $("#Exserviceman1").prop('disabled', false);
                        $("#ExserDocStatus1").prop('disabled', false);
                        $("#txtExservicemanRemarks1").prop('disabled', false);
                        $(".ApplicableReservations1").prop('disabled', false);
                        $("#ExServGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 6) {
                        $("#Differentlyabledcertificate1").prop('disabled', false);
                        $("#DiffAblDocStatus1").prop('disabled', false);
                        $("#txtDiffAbledCertRemarks1").prop('disabled', false);
                        $(".PhysicallyHanidcapInd1").prop('disabled', false);
                        $(".PhysicallyHanidcapDivGri1").prop('disabled', false);
                        $("#DiffAbledGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 7) {
                        $("#EWSCertificate1").prop('disabled', false);
                        $("#EWSDocStatus1").prop('disabled', false);
                        $("#txtEWSCertificateRemarks1").prop('disabled', false);
                        $(".RuralUrban1").prop('disabled', false);
                        $(".ApplicableReservations1").prop('disabled', false);
                        $("#EWSGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 8) {
                        $("#txtMarksObtained1").prop('disabled', false);
                        $("#EduCertificate1").prop('disabled', false);
                        $("#EduDocStatus1").prop('disabled', false);
                        $("#txtEduCertRemarks1").prop('disabled', false);
                        $("#EduGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 9) {
                        $(".RuralUrban1").prop('disabled', false);
                        $("#Ruralcertificate1").prop('disabled', false);
                        $("#RcerDocStatus1").prop('disabled', false);
                        $("#txtRurCertRemarks1").prop('disabled', false);
                        $("#RuralGrivApplied").val(1);
                    }
                    else if (data.FileTypes[i] == 10) {
                        $("#HyderabadKarnatakaRegion1").prop('disabled', false);
                        $("#HyderabadKarnatakaRegion1").prop('disabled', false);
                        $("#HyKarDocStatus1").prop('disabled', false);
                        $("#txtHydKarnRemarks1").prop('disabled', false);
                        $("#HydKarnGrivApplied").val(1);
                        $(".HyderabadKarnatakaRegion1").prop('disabled', false);
                    }
                }
            }
            else {
                alert('faied to load');
            }
        },
        error: function (result) {
            alert("Error", "something went wrong");
        }
    })
}

function GetGrievanceStatus() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetApplicantsStatus",
        contentType: "application/json",
        success: function (data) {
            $('#GrievanceStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                    { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                    {
                        'data': 'CredatedBy',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            //if (oData.GrievanceRefNumber != null)
                            $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",2)' class='btn btn-primary' value='Edit' id='edit'/>");

                        }
                    }
                ]
            });
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function RuralUrbanLocation() {

    $("input:radio[name=RuralUrban1]").prop('checked', true);
    $("input[name='RuralUrban1']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedWhichBasics() {

    $("input:radio[name=AppBasics1]").prop('checked', true);
    $("input[name='AppBasics1']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
        }
    });
}

function AppliedForSyallbus() {

    $("input:radio[name=TenthBoard1]").prop('checked', true);
    $("input[name='TenthBoard1']").each(function () {
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
                                $("#PermanentTalukas1").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                            });
                        });
                    }
                }
            }
        });

        $("#PermanentTalukas1").val($("#Talukas :selected").val());
        $("#txtPermanentPincode1").val($("#txtPincode").val());
        $("#txtPermanentAddress1").attr('readonly', 'readonly');
        $('#PermanentDistricts1').attr("disabled", true);
        $('#PermanentTalukas1').attr("disabled", true);
        $('#txtPermanentPincode1').attr('readonly', true);
    }
    else {
        $("#txtPermanentAddress1").val('');
        $("#PermanentDistricts1").val(0);
        $("#PermanentTalukas1").val(0);
        $("#txtPermanentPincode1").val('');
        $("#txtPermanentAddress1").attr('readonly', false);
        $('#PermanentDistricts1').attr("disabled", false);
        $('#PermanentTalukas1').attr("disabled", false);
        $('#txtPermanentPincode1').attr('readonly', false);
    }
}

function GetApplicationRemarks(ApplicationId) {
    $('#HistoryRemarksCommentsModal').modal('show');
    $.ajax({
        type: 'Post',
        data: { ApplicantTransId: ApplicationId },
        url: '/Admission/GetApplicantRemarksList',
        success: function (data) {
            var t = $('#GetCommentRemarksDetails').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'DocumentTypeName', 'title': 'Document Type', 'className': 'text-left' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-left' },
                    { 'data': 'DocumentWiseRemarks', 'title': 'Document Wise Remarks', 'className': 'text-left' },
                    {
                        'data': 'CreatedOn', 'title': 'Created', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.CreatedOn, 0);
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'UserName', 'title': 'Verification Officer', 'className': 'text-left' },

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

function GetApplicationDetailsGriById(ApplicationId, GrievanceId) {
    $('#myModalGri').modal('show');
    $('input[type="text"], textarea').attr('readonly', false);
    $("#GrievanceId").val(GrievanceId);
    GetGrievanceFileType(GrievanceId);
    clearallErrorFields1();

    RuralUrbanLocation();
    AppliedWhichBasics();
    AppliedForSyallbus();

    $("#Category1").empty();
    $("#Category1").append('<option value="0">Select Category</option>');

    $("#Religion1").empty();
    $("#Religion1").append('<option value="0">Select Religion</option>');

    $("#Gender1").empty();
    $("#Gender1").append('<option value="0">Select Gender</option>');

    $("#ApplicantType1").empty();
    $("#ApplicantType1").append('<option value="0">Select Applicant</option>');

    $("#Qualification1").empty();
    $("#Qualification1").append('<option value="0">Select Qualification</option>');

    $("#Districts1").empty();
    $("#Districts1").append('<option value="0">Select</option>');

    $("#PermanentDistricts1").empty();
    $("#PermanentDistricts1").append('<option value="0">Select</option>');
    var DisAppliReservationInd = "0";

    $.ajax({
        type: 'Get',
        data: { CredatedBy: ApplicationId },
        url: '/Admission/GetApplicationDetailsById',
        success: function (datajson) {

            $(".EduFileAttach1").hide(); $(".CasteFileAttach1").hide(); $(".RationFileAttach1").hide(); $(".IncomeCertificateAttach1").hide();
            $(".UIDFileAttach1").hide(); $(".RuralcertificateAttach1").hide(); $(".KannadamediumCertificateAttach1").hide(); $(".DifferentlyabledcertificateAttach1").hide();
            $(".ExemptedCertificateAttach1").hide(); $(".HyderabadKarnatakaRegionAttach1").hide(); $(".HoranaaduKannadigaAttach1").hide(); $(".OtherCertificatesAttach1").hide();
            $(".KashmirMigrantsAttach1").hide(); $(".ExservicemanAttach1").hide(); $(".EWSCertificateAttach1").hide();

            $("#txtEduCertRemarks1").val('');
            $("#txtCasteCertRemarks1").val('');
            $("#txtRationCardRemarks1").val('');
            $("#txtIncCertRemarks1").val('');
            $("#txtUIDRemarks1").val('');
            $("#txtRurCertRemarks1").val('');
            $("#txtKanMedRemarks1").val('');
            $("#txtDiffAbledCertRemarks1").val('');
            $("#txtExeCertRemarks1").val('');
            $("#txtHydKarnRemarks1").val('');
            $("#txtHorGadKannadigaRemarks1").val('');
            $("#txtOtherCertRemarks1").val('');
            $("#txtKashmirMigrantsRemarks1").val('');
            $("#txtExservicemanRemarks1").val('');
            $("#txtEWSCertificateRemarks1").val('');

            if (datajson.Resultlist != null || datajson.Resultlist != '') {

                $("#ApplicantId").val(datajson.Resultlist.ApplicationId);
                //ApplicantDetails
                //Get the roll number
                $("input[name=RStateBoardType1][value=" + datajson.Resultlist.RStateBoardType + "]").prop('checked', true);
                $("input[name=RAppBasics1][value=" + datajson.Resultlist.RAppBasics + "]").prop('checked', true);
                $("#txtRollNumber1").val(datajson.Resultlist.RollNumber);
                OnChangeStateBoardType();
                OnChangeGetAppliedBasicsForBoardType();

                if (datajson.Resultlist.GetReligionList.length > 0) {
                    $.each(datajson.Resultlist.GetReligionList, function (index, item) {
                        $("#Religion1").append($("<option/>").val(this.Religion_Id).text(this.Religion));
                    });
                }

                if (datajson.Resultlist.GetGenderList.length > 0) {
                    $.each(datajson.Resultlist.GetGenderList, function (index, item) {
                        $("#Gender1").append($("<option/>").val(this.Gender_Id).text(this.Gender));
                    });
                }

                if (datajson.Resultlist.GetCategoryList.length > 0) {
                    $.each(datajson.Resultlist.GetCategoryList, function (index, item) {
                        $("#Category1").append($("<option/>").val(this.CategoryId).text(this.Category));
                    });
                }

                if (datajson.Resultlist.GetApplicantTypeList.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantTypeList, function (index, item) {
                        $("#ApplicantType1").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                    });
                }

                $("#txtCaste1").empty();
                if (datajson.Resultlist.GetCasteList.length > 0) {
                    $.each(datajson.Resultlist.GetCasteList, function (index, item) {
                        $("#txtCaste1").append($("<option/>").val(this.CasteId).text(this.Caste));
                    });
                }

                $("#EduDocStatus1").empty();
                $("#EduDocStatus1").append('<option value="0">Select Status</option>');

                $("#CasDocStatus1").empty();
                $("#CasDocStatus1").append('<option value="0">Select Status</option>');

                $("#RationDocStatus1").empty();
                $("#RationDocStatus1").append('<option value="0">Select Status</option>');

                $("#IncCerDocStatus1").empty();
                $("#IncCerDocStatus1").append('<option value="0">Select Status</option>');

                $("#UIDDocStatus1").empty();
                $("#UIDDocStatus1").append('<option value="0">Select Status</option>');

                $("#RcerDocStatus1").empty();
                $("#RcerDocStatus1").append('<option value="0">Select Status</option>');

                $("#KanMedCerDocStatus1").empty();
                $("#KanMedCerDocStatus1").append('<option value="0">Select Status</option>');

                $("#DiffAblDocStatus1").empty();
                $("#DiffAblDocStatus1").append('<option value="0">Select Status</option>');

                $("#ExCerDocStatus1").empty();
                $("#ExCerDocStatus1").append('<option value="0">Select Status</option>');

                $("#HyKarDocStatus1").empty();
                $("#HyKarDocStatus1").append('<option value="0">Select Status</option>');

                $("#HorKanDocStatus1").empty();
                $("#HorKanDocStatus1").append('<option value="0">Select Status</option>');

                $("#OtherCerDocStatus1").empty();
                $("#OtherCerDocStatus1").append('<option value="0">Select Status</option>');

                $("#KasMigDocStatus1").empty();
                $("#KasMigDocStatus1").append('<option value="0">Select Status</option>');

                $("#ExserDocStatus1").empty();
                $("#ExserDocStatus1").append('<option value="0">Select Status</option>');

                $("#EWSDocStatus1").empty();
                $("#EWSDocStatus1").append('<option value="0">Select Status</option>');

                if (datajson.Resultlist.GetDocumentApplicationStatus.length > 0) {
                    $.each(datajson.Resultlist.GetDocumentApplicationStatus, function (index, item) {
                        $("#EduDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#CasDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#RationDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#IncCerDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#UIDDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#RcerDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#KanMedCerDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#DiffAblDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#ExCerDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#HyKarDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#HorKanDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#OtherCerDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#KasMigDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#ExserDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                        $("#EWSDocStatus1").append($("<option/>").val(this.ApplDocVerifiID).text(this.VerificationStatus));
                    });
                }

                if (datajson.Resultlist.PhysicallyHanidcapInd == true) {
                    $("input[name=PhysicallyHanidcapInd1][value=1]").prop('checked', true);
                    $("#DiffAbledCer").val(1);
                }
                else
                    $("input[name=PhysicallyHanidcapInd1][value=0]").prop('checked', true);

                PhysicallyHanidcapEna1(datajson.Resultlist.PhysicallyHanidcapInd);

                $("#PhysicallyHanidcapType1").empty();
                $("#PhysicallyHanidcapType1").append($("<option/>").val(0).text("Select"));
                if (datajson.Resultlist.PersonWithDisabilityCategory.length > 0) {
                    $.each(datajson.Resultlist.PersonWithDisabilityCategory, function (index, item) {
                        $("#PhysicallyHanidcapType1").append($("<option/>").val(this.PersonWithDisabilityCategoryId).text(this.DisabilityName));
                    });
                }

                if (datajson.Resultlist.PhysicallyHanidcapType == "" || datajson.Resultlist.PhysicallyHanidcapType == null)
                    $("#PhysicallyHanidcapType1").val(0);
                else
                    $("#PhysicallyHanidcapType1").val(datajson.Resultlist.PhysicallyHanidcapType);

                $.each(datajson.Resultlist.GetReservationList, function (index, item) {
                    $("#ApplicableReservations1").append($("<option/>").val(this.ReservationId).text(this.Reservations));
                });

                var MultiselectSelectedValue = datajson.Resultlist.SelectedReservationId;
                if (MultiselectSelectedValue != null) {
                    $.each(MultiselectSelectedValue.split(","), function (i, e) {
                        $("#ApplicableReservations1 option[value='" + e + "']").prop("selected", true);
                    });
                }
                $('#ApplicableReservations1').multiselect({});

                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#Districts1").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                        $("#PermanentDistricts1").append($("<option/>").val(this.district_lgd_code).text(this.district_ename));
                    });
                }

                if (datajson.Resultlist.GetApplicantDocumentsDetail.length > 0) {
                    $.each(datajson.Resultlist.GetApplicantDocumentsDetail, function (index, item) {
                        if (this.DocumentTypeId == 1) {
                            if (this.FilePath != null) {
                                $(".EduFileAttach1").show();
                                $('#aEduCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtEduCertRemarks1").val(this.Remarks);
                                $("#EduDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#EduCertificate1").prop('disabled', true);
                                    $("#EduDocStatus1").prop('disabled', true);
                                    $("#txtEduCertRemarks1").prop('disabled', true);
                                    $("#txtMarksObtained1").prop('disabled', true);
                                    $("#txtMinMarks1").prop('disabled', true);
                                    $("#DocEduCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocEduCerRejectedImg1").show();
                            }
                            $("#EDocAppId").val(this.DocAppId);
                            $("#ECreatedBy").val(this.CreatedBy);
                            $("#EduDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 2) {
                            if (this.FilePath != null) {
                                $(".CasteFileAttach1").show();
                                $('#aCasteCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtCasteCertRemarks1").val(this.Remarks);
                                $("#CasDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Category1").prop('disabled', true);
                                    $("#txtCaste1").prop('disabled', true);
                                    $("#CasteCertificate1").prop('disabled', true);
                                    $("#CasDocStatus1").prop('disabled', true);
                                    $("#txtCasteCertRemarks1").prop('disabled', true);
                                    $("#DocCasCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocCasCerRejectedImg1").show();
                            }
                            $("#CDocAppId").val(this.DocAppId);
                            $("#CCreatedBy").val(this.CreatedBy);
                            $("#CasDocStatus").val(this.Verified);

                        }
                        else if (this.DocumentTypeId == 3) {
                            if (this.FilePath != null) {
                                $(".RationFileAttach1").show();
                                $('#aRationCard1').attr('href', '' + this.FilePath + '');
                                $("#txtRationCardRemarks1").val(this.Remarks);
                                $("#RationDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Rationcard1").prop('disabled', true);
                                    $("#RationDocStatus1").prop('disabled', true);
                                    $("#txtRationCardRemarks1").prop('disabled', true);
                                    $("#DocRatCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocRatCerRejectedImg1").show();
                            }
                            $("#RDocAppId").val(this.DocAppId);
                            $("#RCreatedBy").val(this.CreatedBy);
                            $("#RationDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 4) {
                            if (this.FilePath != null) {
                                $(".IncomeCertificateAttach1").show();
                                $('#aIncomeCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtIncCertRemarks1").val(this.Remarks);
                                $("#IncCerDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Incomecertificate1").prop('disabled', true);
                                    $("#IncCerDocStatus1").prop('disabled', true);
                                    $("#txtIncCertRemarks1").prop('disabled', true);
                                    $("#DocIncCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocIncCerRejectedImg1").show();
                            }
                            $("#IDocAppId").val(this.DocAppId);
                            $("#ICreatedBy").val(this.CreatedBy);
                            $("#IncCerDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 5) {
                            if (this.FilePath != null) {
                                $(".UIDFileAttach1").show();
                                $('#aUIDNumber1').attr('href', '' + this.FilePath + '');
                                $("#txtUIDRemarks1").val(this.Remarks);
                                $("#UIDDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#UIDNumber1").prop('disabled', true);
                                    $("#UIDDocStatus1").prop('disabled', true);
                                    $("#txtUIDRemarks1").prop('disabled', true);
                                    $("#DocUIDCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocUIDCerRejectedImg1").show();
                            }
                            $("#UDocAppId").val(this.DocAppId);
                            $("#UCreatedBy").val(this.CreatedBy);
                            $("#UIDDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 6) {
                            if (this.FilePath != null) {
                                $(".RuralcertificateAttach1").show();
                                $('#aRuralcertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtRurCertRemarks1").val(this.Remarks);
                                $("#RcerDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Ruralcertificate1").prop('disabled', true);
                                    $("#RcerDocStatus1").prop('disabled', true);
                                    $("#txtRurCertRemarks1").prop('disabled', true);
                                    $(".RuralUrban1").prop('disabled', true);
                                    $("#DocRurCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocRurCerRejectedImg1").show();
                            }
                            $("#RUDocAppId").val(this.DocAppId);
                            $("#RUCreatedBy").val(this.CreatedBy);
                            $("#RcerDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 7) {
                            if (this.FilePath != null) {
                                $(".KannadamediumCertificateAttach1").show();
                                $('#aKannadamediumCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtKanMedRemarks1").val(this.Remarks);
                                $("#KanMedCerDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#KannadamediumCertificate1").prop('disabled', true);
                                    $("#KanMedCerDocStatus1").prop('disabled', true);
                                    $("#txtKanMedRemarks1").prop('disabled', true);
                                    $("#DocKanMedCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocKanMedCerRejectedImg1").show();
                            }
                            $("#KDocAppId").val(this.DocAppId);
                            $("#KCreatedBy").val(this.CreatedBy);
                            $("#KanMedCerDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 8) {
                            if (this.FilePath != null) {
                                $(".DifferentlyabledcertificateAttach1").show();
                                $('#aDifferentlyabledcertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtDiffAbledCertRemarks1").val(this.Remarks);
                                $("#DiffAblDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Differentlyabledcertificate1").prop('disabled', true);
                                    $("#DiffAblDocStatus1").prop('disabled', true);
                                    $("#txtDiffAbledCertRemarks1").prop('disabled', true);
                                    $("#DocDidAblCerAcceptedImg1").show();
                                    $("#PhysicallyHanidcapType1").prop('disabled', true);
                                    $(".PhysicallyHanidcapInd1").prop('disabled', true);
                                }
                                else if (this.Verified == 3)
                                    $("#DocDidAblCerRejectedImg1").show();
                            }
                            $("#DDocAppId").val(this.DocAppId);
                            $("#DCreatedBy").val(this.CreatedBy);
                            $("#DiffAblDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 9) {
                            if (this.FilePath != null) {
                                $(".ExemptedCertificateAttach1").show();
                                $('#aExemptedCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtExeCertRemarks1").val(this.Remarks);
                                $("#ExCerDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#ExemptedCertificate1").prop('disabled', true);
                                    $("#ExCerDocStatus1").prop('disabled', true);
                                    $("#txtExeCertRemarks1").prop('disabled', true);
                                    $("#DocExStuCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocExStuCerRejectedImg1").show();
                            }
                            $("#ExDocAppId").val(this.DocAppId);
                            $("#ExCreatedBy").val(this.CreatedBy);
                            $("#ExCerDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 10) {
                            if (this.FilePath != null) {
                                $(".HyderabadKarnatakaRegionAttach1").show();
                                $('#aHyderabadKarnatakaRegion1').attr('href', '' + this.FilePath + '');
                                $("#txtHydKarnRemarks1").val(this.Remarks);
                                $("#HyKarDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#HyderabadKarnatakaRegion1").prop('disabled', true);
                                    $("#HyKarDocStatus1").prop('disabled', true);
                                    $("#txtHydKarnRemarks1").prop('disabled', true);
                                    $("#HyderabadKarnatakaRegion1").prop('disabled', true);
                                    $("#DocHyKarRegAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocHyKarRegRejectedImg1").show();
                            }
                            $("#HDocAppId").val(this.DocAppId);
                            $("#HCreatedBy").val(this.CreatedBy);
                            $("#HyKarDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 11) {
                            if (this.FilePath != null) {
                                $(".HoranaaduKannadigaAttach1").show();
                                $('#aHoranaaduKannadiga1').attr('href', '' + this.FilePath + '');
                                $("#txtHorGadKannadigaRemarks1").val(this.Remarks);
                                $("#HorKanDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#HoranaaduKannadiga1").prop('disabled', true);
                                    $("#HorKanDocStatus1").prop('disabled', true);
                                    $("#txtHorGadKannadigaRemarks1").prop('disabled', true);
                                    $("#DocHorGadKanAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocHorGadKanRejectedImg1").show();
                            }
                            $("#HGKDocAppId").val(this.DocAppId);
                            $("#HGKCreatedBy").val(this.CreatedBy);
                            $("#HorKanDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 12) {
                            if (this.FilePath != null) {
                                $(".OtherCertificatesAttach1").show();
                                $('#aOtherCertificates1').attr('href', '' + this.FilePath + '');
                                $("#txtOtherCertRemarks1").val(this.Remarks);
                                $("#OtherCerDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#OtherCertificates1").prop('disabled', true);
                                    $("#OtherCerDocStatus1").prop('disabled', true);
                                    $("#txtOtherCertRemarks1").prop('disabled', true);
                                    $("#DocOthCerAcceptedImg1").show();
                                }
                                else if (this.Verified == 3)
                                    $("#DocOthCerRejectedImg1").show();
                            }
                            $("#ODocAppId").val(this.DocAppId);
                            $("#OCreatedBy").val(this.CreatedBy);
                            $("#OtherCerDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 14) {
                            if (this.FilePath != null) {
                                $(".ExservicemanAttach1").show();
                                $('#aExserviceman1').attr('href', '' + this.FilePath + '');
                                $("#txtExservicemanRemarks1").val(this.Remarks);
                                $("#ExserDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#Exserviceman1").prop('disabled', true);
                                    $("#ExserDocStatus1").prop('disabled', true);
                                    $("#txtExservicemanRemarks1").prop('disabled', true);
                                    $("#DocExSerAcceptedImg1").show();
                                    DisAppliReservationInd = parseFloat(DisAppliReservationInd) + 1;
                                }
                                else if (this.Verified == 3)
                                    $("#DocExSerRejectedImg1").show();
                            }
                            $("#ExSDocAppId").val(this.DocAppId);
                            $("#ExSCreatedBy").val(this.ExSCreatedBy);
                            $("#ExserDocStatus").val(this.Verified);
                        }
                        else if (this.DocumentTypeId == 16) {
                            if (this.FilePath != null) {
                                $(".EWSCertificateAttach1").show();
                                $('#aEWSCertificate1').attr('href', '' + this.FilePath + '');
                                $("#txtEWSCertificateRemarks1").val(this.Remarks);
                                $("#EWSDocStatus1").val(this.Verified);
                                if (this.Verified == 15) {
                                    $("#EWSCertificate1").prop('disabled', true);
                                    $("#EWSDocStatus1").prop('disabled', true);
                                    $("#txtEWSCertificateRemarks1").prop('disabled', true);
                                    $("#DocEWSAcceptedImg1").show();
                                    DisAppliReservationInd = parseFloat(DisAppliReservationInd) + 1;
                                }
                                else if (this.Verified == 3)
                                    $("#DocEWSRejectedImg1").show();
                            }
                            $("#EWSDocAppId").val(this.DocAppId);
                            $("#EWSCreatedBy").val(this.EWSCreatedBy);
                            $("#EWSDocStatus").val(this.Verified);
                        }

                        if (DisAppliReservationInd == "2")
                            $("#ApplicableReservations1").prop('disabled', true);
                    });
                }
                $("#ApplStatus").val(datajson.Resultlist.ApplStatus);

                $("#academicyear2").datepicker('setDate', new Date(datajson.Resultlist.ApplyYear, datajson.Resultlist.ApplyMonth, 1));

                $("#txtAadhaarNumber1").val(datajson.Resultlist.AadhaarNumber);
                $("#AccountNumber1").val(datajson.Resultlist.AccountNumber);
                $("#BankName1").val(datajson.Resultlist.BankName);
                $("#IFSCCode1").val(datajson.Resultlist.IFSCCode);
                $("#txtRationCard1").val(datajson.Resultlist.RationCard);
                
                if (datajson.Resultlist.HoraNadu_GadiNadu_Kannidagas == true) {
                    $("input[name=HoraNadu1][value=1]").prop('checked', true);
                    $("#HoraNaduCer").val(1);
                }
                else
                    $("input[name=HoraNadu1][value=0]").prop('checked', true);

                if (datajson.Resultlist.ExemptedFromStudyCertificate == true) {
                    $("input[name=ExemptedFromStudyCertificate1][value=1]").prop('checked', true);
                    $("#ExeStuCer").val(1);
                }
                else
                    $("input[name=ExemptedFromStudyCertificate1][value=0]").prop('checked', true);

                if (datajson.Resultlist.HyderabadKarnatakaRegion == true) {
                    $("input[name=HyderabadKarnatakaRegion1][value=1]").prop('checked', true);
                    $("#HydKarCer").val(1);
                }
                else
                    $("input[name=HyderabadKarnatakaRegion1][value=0]").prop('checked', true);

                if (datajson.Resultlist.KanndaMedium == true) {
                    $("input[name=KanndaMedium1][value=1]").prop('checked', true);
                    $("#KanMedCer").val(1);
                }
                else
                    $("input[name=KanndaMedium1][value=0]").prop('checked', true);

                $("#txtApplicantName1").val(datajson.Resultlist.ApplicantName);
                $("#txtFathersName1").val(datajson.Resultlist.FathersName);
                $("#txtParentOccupation1").val(datajson.Resultlist.ParentsOccupation);
                $('#ImgPhotoUpload1').attr("src", datajson.Resultlist.Photo);
                $("#IsUploaded1").val(datajson.Resultlist.Photo);

                var finalDOB = daterangeformate2(datajson.Resultlist.DOB, 1);
                $("#dateofbirth1").val(finalDOB);
                $("#txtMothersName1").val(datajson.Resultlist.MothersName);
                $("#Religion1").val(datajson.Resultlist.Religion);
                $('#Gender1').val(datajson.Resultlist.Gender);
                $("#Category1").val(datajson.Resultlist.Category);
                $("#MinorityCategory1").val(datajson.Resultlist.MinorityCategory);
                if (datajson.Resultlist.Caste != null)
                    $("#txtCaste1").val(datajson.Resultlist.Caste);
                $("#txtFamilyAnnualIncome1").val(datajson.Resultlist.FamilyAnnIncome);
                $("#ApplicantType1").val(datajson.Resultlist.ApplicantType);

                //EducationDetails
                if (datajson.Resultlist.GetQualificationList.length > 0) {
                    $.each(datajson.Resultlist.GetQualificationList, function (index, item) {
                        $("#Qualification1").append($("<option/>").val(this.QualificationId).text(this.Qualification));
                    });
                }
                $('#Qualification1').val(datajson.Resultlist.Qualification);
                $("input[name=RuralUrban1][value=" + datajson.Resultlist.ApplicantBelongTo + "]").prop('checked', true);
                $("input[name=AppBasics1][value=" + datajson.Resultlist.AppliedBasic + "]").prop('checked', true);
                $("input[name=TenthBoard1][value=" + datajson.Resultlist.TenthBoard + "]").prop('checked', true);
                //$("#txtEduGrade").val(datajson.Resultlist.EducationGrade);
                $("input[name=TenthCOBSEBoard1][value=" + datajson.Resultlist.TenthCOBSEBoard + "]").prop('checked', true);
                TenthBoardStateType();
                $("#txtInstituteStudied1").val(datajson.Resultlist.InstituteStudiedQual);
                $("#txtMaximumMarks1").val(datajson.Resultlist.MaxMarks);
                $("#txtMarksObtained1").val(datajson.Resultlist.MarksObtained);
                $("#txtMinMarks1").val(datajson.Resultlist.MinMarks);
                $("#lblPercAsPerMarks1").text(datajson.Resultlist.Percentage + "%");
                $("#Results1").val(datajson.Resultlist.ResultQual);
                if (datajson.Resultlist.studiedMathsScience == true)
                    $("input[name=studiedMathsScience1][value=1]").prop('checked', true);
                else
                    $("input[name=studiedMathsScience1][value=0]").prop('checked', true);
                //AddressDetails
                $("#txtCommunicationAddress1").val(datajson.Resultlist.CommunicationAddress);
                $("#Districts1").val(datajson.Resultlist.DistrictId);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#Talukas1").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }

                $('#Talukas1').val(datajson.Resultlist.TalukaId);
                $("#txtPincode1").val(datajson.Resultlist.Pincode);
                $("input[name=chkSameAsCommunicationAddress1][value=" + datajson.Resultlist.SameAdd + "]").prop('checked', true);
                if (datajson.Resultlist.SameAdd == 1 || datajson.Resultlist.SameAdd == true)
                    $("#chkSameAsCommunicationAddress1").prop('checked', true);
                else
                    $("#chkSameAsCommunicationAddress1").prop('checked', false);

                OnchangechkSameAsCommunicationAddress();
                $("#txtPermanentAddress1").val(datajson.Resultlist.PermanentAddress);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function (index, item) {
                        $("#PermanentDistricts1").append($("<option/>").val(item.district_lgd_code).text(item.district_ename));
                    });
                }
                $("#PermanentTalukas1").empty();
                $("#PermanentTalukas1").append('<option value="0">Select</option>');
                $("#PermanentDistricts1").val(datajson.Resultlist.PDistrict);
                if (datajson.Resultlist.GetDistrictList.length > 0) {
                    $.each(datajson.Resultlist.GetDistrictList, function () {
                        $.each($(this.TalukListDet), function (index, item) {
                            $("#PermanentTalukas1").append($("<option/>").val(item.taluk_lgd_code).text(item.taluk_ename));
                        });
                    });
                }
                $('#PermanentTalukas1').val(datajson.Resultlist.PTaluk);
                $("#txtPermanentPincode1").val(datajson.Resultlist.PPinCode);
                $("#txtApplicantPhoneNumber1").val(datajson.Resultlist.PhoneNumber);
                $("#txtFathersPhoneNumber1").val(datajson.Resultlist.FatherPhoneNumber);
                $("#txtEmailId1").val(datajson.Resultlist.EmailId);
                $("#txtRemarks1").val('');

                if ($("#CalenderNotificationGriEligibleInd").val() == 1) {
                    if (datajson.Resultlist.ApplStatus == 13) { //Approved
                        $("#saveDocumentsRemarksDetDiv1").show();
                        //$("#saveDocumentsRemarksDetDiv2").show();
                        $("#saveDocumentsRemarksDetDiv2").hide();
                        $("#saveDocumentsRemarksDetDiv3").hide();
                        $("#saveDocumentsRemarksDetDiv4").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 12) {    //Sent for correction
                        $("#saveDocumentsRemarksDetDiv1").show();
                        $("#saveDocumentsRemarksDetDiv2").hide();
                        //$("#saveDocumentsRemarksDetDiv3").show();
                        $("#saveDocumentsRemarksDetDiv3").hide();
                        $("#saveDocumentsRemarksDetDiv4").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 11) {    //Raised
                        $("#saveDocumentsRemarksDetDiv1").show();
                        $("#saveDocumentsRemarksDetDiv2").hide();
                        $("#saveDocumentsRemarksDetDiv3").hide();
                        $("#saveDocumentsRemarksDetDiv4").hide();
                    }
                    else if (datajson.Resultlist.ApplStatus == 3) { //Rejected
                        $("#saveDocumentsRemarksDetDiv1").show();
                        $("#saveDocumentsRemarksDetDiv2").hide();
                        $("#saveDocumentsRemarksDetDiv3").hide();
                        //$("#saveDocumentsRemarksDetDiv4").show();
                        $("#saveDocumentsRemarksDetDiv4").hide();
                    }
                    $("#EligibleGrievanceVerificationForm").hide();
                }
                else {
                    $("#saveDocumentsRemarksDetDiv1").hide();
                    $("#saveDocumentsRemarksDetDiv2").hide();
                    $("#saveDocumentsRemarksDetDiv3").hide();
                    $("#saveDocumentsRemarksDetDiv4").hide();
                    $("#EligibleGrievanceVerificationForm").show();
                }
            }
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

//To Get the standard type based on the board type
function OnChangeStateBoardType() {

    $("input[name='RStateBoardType1']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();

            $("#GetAppliedBasicsForBoardType").hide();
            if (value == 1) {
                $("#GetAppliedBasicsForBoardType").show();
            }
            else {
                $("input[name=RAppBasics1][value=1]").prop('checked', true);
            }
            OnChangeGetAppliedBasicsForBoardType();
        }
    });
}

//To Get the roll number based on the standard type
function OnChangeGetAppliedBasicsForBoardType() {
    $("input[name='RAppBasics1']").each(function () {
        if ($(this).is(":checked")) {
            var value = $(this).val();
            $("#GetRollNumberFor10thStd").hide();
            if (value == 2) {
                $("#GetRollNumberFor10thStd").show();
            }
        }
    });
}

function PhysicallyHanidcapEna1(id) {
    var value = $('input[name=PhysicallyHanidcapInd1]:checked').val();
    if (value == 1) {
        $("#PhysicallyHanidcapType1").prop("disabled", false);
        $(".PhysicallyHanidcapDivGri1").show();
    }
    else {
        $(".PhysicallyHanidcapDivGri1").hide();
    }
}

function TenthBoardStateType() {
    var selectedTenthBoardStateType = $('input[name="TenthBoard1"]:checked').val();
    if (selectedTenthBoardStateType == 1) {
        $("#TenthCOBSEType").hide();
        //$("#txtEduGrade").val('');
        $("input[name=TenthCOBSEBoard1][value=7]").prop('checked', true)
    }
    else {
        $("#TenthCOBSEType").show();
        TenthCOBSEBoardType();
    }
}

function TenthCOBSEBoardType() {
    var selectedTenthCOBSEBoard = $('input[name="TenthCOBSEBoard1"]:checked').val();
    if (selectedTenthCOBSEBoard == 2 || selectedTenthCOBSEBoard == 3) {
        $("#TenthCBSEICSEType").show();
    }
    else {
        $("#TenthCBSEICSEType").hide();
    }
}

function GetApplicantType() {

    $.ajax({
        type: 'Get',
        url: '/Admission/GetApplicantTypeList',
        success: function (datajson) {

            if (datajson != null || datajson != '') {
                $.each(datajson, function () {
                    $("#ApplicantType1").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantType));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

$('select[name="ApplicantTypeSelect"]').on('change', function () {

    var provinceId = $(this).val();
    if (provinceId != 0) {
        $.ajax({
            type: "GET",

            url: "/Admission/GetApplicantsStatusApp",
            data: { ApplicantType: provinceId },
            contentType: "application/json",
            success: function (data) {
                var table = $('#VerifyDocumentsTable').DataTable();
                table.destroy();
                $('#VerifyDocumentsTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                        { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center' },
                        { 'data': 'ApplicantName', 'title': 'Applicant Name', 'className': 'text-left' },
                        { 'data': 'StatusName', 'title': 'Application Status', 'className': 'text-left' },
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
                                $(nTd).html("<input type='button' onclick='GetApplicationDetailsById(" + oData.ApplicationId + ",2)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>");

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
        GetGrievanceStatus();
    }
});

function clearallErrorFields1() {

    $("#EduCertificate1-Required").hide();
    $("#CasteCertificate1-Required").hide();
    $("#Rationcard1-Required").hide();
    $("#Incomecertificate1-Required").hide();
    $("#UIDNumber1-Required").hide();
    $("#Ruralcertificate1-Required").hide();
    $("#KannadamediumCertificate1-Required").hide();
    $("#Differentlyabledcertificate1-Required").hide();
    $("#ExemptedCertificate1-Required").hide();
    $("#HyderabadKarnatakaRegion1-Required").hide();
    $("#HoranaaduKannadiga1-Required").hide();
    $("#OtherCertificates1-Required").hide();
    $("#Exserviceman1-Required").hide();
    $("#EWSCertificate1-Required").hide();
    $("#EduCertificate1").val('');
    $("#CasteCertificate1").val('');
    $("#Incomecertificate1").val('');
    $("#UIDNumber1").val('');
    $("#Ruralcertificate1").val('');
    $("#KannadamediumCertificate1").val('');
    $("#Differentlyabledcertificate1").val('');
    $("#ExemptedCertificate1").val('');
    $("#HyderabadKarnatakaRegion1").val('');
    $("#HoranaaduKannadiga1").val('');
    $("#OtherCertificates1").val('');
    $("#Exserviceman1").val('');
    $("#EWSCertificate1").val('');

    $("#WithoutDocEduStatus1-Required").hide();
    $("#WithoutDocEduRemarks1-Required").hide();
    $("#WithoutDocCasStatus1-Required").hide();
    $("#WithoutDocCasRemarks1-Required").hide();
    $("#WithoutDocRatStatus1-Required").hide();
    $("#WithoutDocRatRemarks1-Required").hide();
    $("#WithoutDocIncStatus1-Required").hide();
    $("#WithoutDocIncRemarks1-Required").hide();
    $("#WithoutDocUIDStatus1-Required").hide();
    $("#WithoutDocUIDRemarks1-Required").hide();
    $("#WithoutDocRurStatus1-Required").hide();
    $("#WithoutDocRurRemarks1-Required").hide();
    $("#WithoutDocKMCStatus1-Required").hide();
    $("#WithoutDocKMCRemarks1-Required").hide();
    $("#WithoutDocDAStatus1-Required").hide();
    $("#WithoutDocDARemarks1-Required").hide();
    $("#WithoutDocECStatus1-Required").hide();
    $("#WithoutDocECRemarks1-Required").hide();
    $("#WithoutDocHKRStatus1-Required").hide();
    $("#WithoutDocHKRRemarks1-Required").hide();
    $("#WithoutDocHorKanStatus1-Required").hide();
    $("#WithoutDocHorKanRemarks1-Required").hide();
    $("#WithoutDocOCStatus1-Required").hide();
    $("#WithoutDocOCRemarks1-Required").hide();
    $("#WithoutDocKMStatus1-Required").hide();
    $("#WithoutDocKMRemarks1-Required").hide();
    $("#WithoutDocExSerStatus1-Required").hide();
    $("#WithoutDocExSerRemarks1-Required").hide();
    $("#WithoutDocLLCStatus1-Required").hide();
    $("#WithoutDocLLCRemarks1-Required").hide();
    $("#WithoutDocEWSStatus1-Required").hide();
    $("#WithoutDocEWSRemarks1-Required").hide();
    $("#txtRemarks1-Required").hide();

    $("#DocEduCerAcceptedImg1").hide();
    $("#DocCasCerAcceptedImg1").hide();
    $("#DocRatCerAcceptedImg1").hide();
    $("#DocIncCerAcceptedImg1").hide();
    $("#DocUIDCerAcceptedImg1").hide();
    $("#DocRurCerAcceptedImg1").hide();
    $("#DocKanMedCerAcceptedImg1").hide();
    $("#DocDidAblCerAcceptedImg1").hide();
    $("#DocExStuCerAcceptedImg1").hide();
    $("#DocHyKarRegAcceptedImg1").hide();
    $("#DocHorGadKanAcceptedImg1").hide();
    $("#DocOthCerAcceptedImg1").hide();
    $("#DocKasMigAcceptedImg1").hide();
    $("#DocExSerAcceptedImg1").hide();
    $("#DocEWSAcceptedImg1").hide();

    $("#DocEduCerRejectedImg1").hide()
    $("#DocCasCerRejectedImg1").hide()
    $("#DocRatCerRejectedImg1").hide()
    $("#DocIncCerRejectedImg1").hide()
    $("#DocUIDCerRejectedImg1").hide()
    $("#DocRurCerRejectedImg1").hide()
    $("#DocKanMedCerRejectedImg1").hide()
    $("#DocDidAblCerRejectedImg1").hide()
    $("#DocExStuCerRejectedImg1").hide()
    $("#DocHyKarRegRejectedImg1").hide()
    $("#DocHorGadKanRejectedImg1").hide()
    $("#DocOthCerRejectedImg1").hide()
    $("#DocExSerRejectedImg1").hide()
    $("#DocEWSRejectedImg1").hide()
}