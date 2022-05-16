
$(document).ready(function () {
    $('.nav-tabs li:eq(0) a').tab('show');
    $('#deptId').attr('disabled', 'disabled');
    $('#deptId').val(DeptConst.Admission);
    //$('#appliGetApplicantTypeAdmissioncantType').attr('disabled', 'disabled');
    $('#GridListDiv').show();
    $('#ViewDiv').hide();
    $('#sendBtn3').hide();
    $('#sendBtnchanges').hide();
    GetApplicantTypechange();
    //disable special characters for nitification number
    $('#notifNumber').on('keypress', function (event) {
        //var regex = new RegExp("^[a-zA-Z0-9_@./#&+-]*$");
        //var regex = new RegExp("^[a-zA-Z0-9!@#\$%\^\&*\)\(+=._-]{6,}$");
        var regex = new RegExp("^[0-9-!@#$%&*?]"); 
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
    Getusers("users", "1");
    Getusers("users1", "3");
    GetAdmissionCalendarDetails();

    //22Events checkbox
    $('#divDateofnotifcationfrm').hide();
    $('#divDateofnotifcationto').hide();
    $('#divDateapplonlineappfrm').hide();
    $('#divDateapplonlineappfrmTo').hide();
    $('#divDocumentvrffrm').hide();
    $('#divDocumentvrfTo').hide();
    $('#divDisplayevList').hide();
    $('#divDisplayTentative').hide();
    $('#divDatabasebackup').hide();
    $('#divfstlistseatAllot').hide();
    $('#divfstlistAdmitcan').hide();
    $('#divDisplayfinal').hide();
    $('#divfstroundadmProFrom').hide();
    $('#divfstroundadmProTo').hide();
    $('#div2ndrndEntryFrom').hide();
    $('#div2ndrndEntryTo').hide();
    $('#divDatabasebackup2ndFrom').hide();
    $('#divDatabasebackup2ndTo').hide();
    $('#div2ndroundAdmProFrom').hide();
    $('#div2ndroundAdmProTo').hide();
    $('#div2ndelistofadmitCan').hide();
    $('#div3rdroundEntryFrom').hide();
    $('#div3rdroundEntryTo').hide();
    $('#divDatabasebackup3rdRound').hide();
    $('#div3rdRoundAdmProFrom').hide();
    $('#div3rdRoundAdmProTo').hide();
    $('#divListcanadm3rdRound').hide();
    $('#divFinalRoundEntryFrom').hide();
    $('#divFinalRoundEntryTo').hide();
    $('#divFinalseatallot').hide();
    $('#divAdmFinalRoundFrom').hide();
    $('#divAdmFinalRoundTo').hide();
    $('#divCommencementTra').hide();
    CalendarNextDate();
    $('#txtRemarks-Required').hide();
    $('#users1').click(function () {
        $("#users1 option[value ='" + loginUserRole.DD + "']").hide();
    });
});
//22 Events checkbox script
$('#chkDateofnotiffrm').change(function () {

    if ($('#chkDateofnotiffrm').prop('checked')) {
        $('#divDateofnotifcationfrm').show();
        $('#divDateofnotifcationto').show();
    }
    else {
        $('#divDateofnotifcationfrm').hide();
        $('#divDateofnotifcationto').hide();
        $('#date_ofnotifFrom').val('');
        $('#date_ofnotifTo').val('');
    }
});

$('#chktentativeseatfrm').change(function () {

    if ($('#chktentativeseatfrm').prop('checked')) {
        $('#divtentativesetafrm').show();
        $('#divtentativesetato').show();
    }
    else {
        $('#divtentativesetafrm').hide();
        $('#divtentativesetato').hide();
        $('#date_Tentativefrm').val('');
        $('#date_TentativeTo').val('');
    }
});

$('#chkDateapplonlineappfrm').change(function () {
    
    if ($('#chkDateapplonlineappfrm').prop('checked')) {
        $('#divDateapplonlineappfrm').show();
        $('#divDateapplonlineappfrmTo').show();
    }
    else {
        $('#divDateapplonlineappfrm').hide();
        $('#divDateapplonlineappfrmTo').hide();        
        $('#date_notifApplyFrom').val('');
        $('#date_notifApplyTo').val('');
    }  
});

$('#chkDocumentvrf').change(function () {
   
    if ($('#chkDocumentvrf').prop('checked')) {
        $('#divDocumentvrffrm').show();
        $('#divDocumentvrfTo').show();
    }
    else {
        $('#divDocumentvrffrm').hide();
        $('#divDocumentvrfTo').hide();
        $('#date_VerifyFrom').val('');
        $('#date_VerifyTo').val('');
    }    
});

$('#chkApplicantvrf').change(function () {

    if ($('#chkApplicantvrf').prop('checked')) {
        $('#divApplicantDocvrffrm').show();
        $('#divApplicantDocvrfTo').show();
    }
    else {
        $('#divApplicantDocvrffrm').hide();
        $('#divApplicantDocvrfTo').hide();
        $('#date_AppDocVeriffrm').val('');
        $('#date_AppDocVerifto').val('');
    }
});

$('#chkfinalseatmtrx').change(function () {

    if ($('#chkfinalseatmtrx').prop('checked')) {
        $('#divfnlseatmtrxfrm').show();
        $('#divfnlseatmtrxto').show();
    }
    else {
        $('#divfnlseatmtrxfrm').hide();
        $('#divfnlseatmtrxto').hide();
        $('#date_fnlseatmtrxfrm').val('');
        $('#date_fnlseatmtrxto').val('');
    }
});

$('#chktntgrdbyapplicant').change(function () {

    if ($('#chktntgrdbyapplicant').prop('checked')) {
        $('#divtntgrdlistbyapplfrm').show();
        $('#divtntgrdlistbyapplto').show();
    }
    else {
        $('#divtntgrdlistbyapplfrm').hide();
        $('#divtntgrdlistbyapplto').hide();
        $('#date_tntgrlstapplfrm').val('');
        $('#date_tntgrlstapplto').val('');
    }
});

$('#chkApplDocvro').change(function () {

    if ($('#chkApplDocvro').prop('checked')) {
        $('#divApplgrbyDocvrofcrfrm').show();
        $('#divApplgrbyDocvrofcrto').show();
    }
    else {
        $('#divApplgrbyDocvrofcrfrm').hide();
        $('#divApplgrbyDocvrofcrto').hide();
        $('#date_ApplgrbyDocvrofcrFrm').val('');
        $('#date_ApplgrbyDocvrofcrTo').val('');
    }
});



$('#chkDisplayevList').change(function () {
    
    if ($('#chkDisplayevList').prop('checked')) {
        $('#divDisplayevList').show();       
    }
    else {
        $('#divDisplayevList').hide(); 
        $('#date_eligible').val('');
    }
    
});

$('#chkDisplayTentative').change(function () {
    
    if ($('#chkDisplayTentative').prop('checked')) {
        $('#divDisplayTentative').show();       
    }
    else {
        $('#divDisplayTentative').hide(); 
        $('#date_tentative').val('');
    }
    
});

$('#chkDisplayfinal').change(function () {
    
    if ($('#chkDisplayfinal').prop('checked')) {
        $('#divDisplayfinal').show();       
    }
    else {
        $('#divDisplayfinal').hide();   
        $('#date_final').val(''); 
    }
    
});

$('#chkDatabasebackup').change(function () {
   
    if ($('#chkDatabasebackup').prop('checked')) {
        $('#divDatabasebackup').show();       
    }
    else {
        $('#divDatabasebackup').hide();     
        $('#date_dueDate').val('');
    }    
});

$('#chkfstlistseatAllot').change(function () {
    
    if ($('#chkfstlistseatAllot').prop('checked')) {
        $('#divfstlistseatAllot').show();       
    }
    else {
        $('#divfstlistseatAllot').hide();     
        $('#date_seatallotment').val('');
    }
    
});

$('#chkfstlistAdmitcan').change(function () {

    if ($('#chkfstlistAdmitcan').prop('checked')) {
        $('#divfstlistAdmitcan').show();       
    }
    else {
        $('#divfstlistAdmitcan').hide();     
        $('#date_admittedCan').val('');
    }  
});

$('#chkfstroundadmPro').change(function () {
  
    if ($('#chkfstroundadmPro').prop('checked')) {
        $('#divfstroundadmProFrom').show();
        $('#divfstroundadmProTo').show();   
    }
    else {
        $('#divfstroundadmProFrom').hide();
        $('#divfstroundadmProTo').hide();
        $('#date_1stRoundFrom').val('');
        $('#date_1stRoundTo').val('');
    }    
});

$('#chk2ndrndEntry').change(function () {
    if ($('#chk2ndrndEntry').prop('checked')) {
        $('#div2ndrndEntryFrom').show();
        $('#div2ndrndEntryTo').show(); 
    }
    else {
        $('#div2ndrndEntryFrom').hide();
        $('#div2ndrndEntryTo').hide();
        $('#date_2ndRoundFrom').val('');
        $('#date_2ndRoundTo').val('');
    }   
});

$('#chkDatabasebackup2nd').change(function () {
    if ($('#chkDatabasebackup2nd').prop('checked')) {
        $('#divDatabasebackup2ndFrom').show();
        $('#divDatabasebackup2ndTo').show();
    }
    else {
        $('#divDatabasebackup2ndFrom').hide();
        $('#divDatabasebackup2ndTo').hide();
        $('#date_2ndSeatFrom').val('');
        $('#date_2ndSeatTo').val('');
    }   
});

$('#chk2ndroundAdmPro').change(function () {
    if ($('#chk2ndroundAdmPro').prop('checked')) {
        $('#div2ndroundAdmProFrom').show();
        $('#div2ndroundAdmProTo').show();
    }
    else {
        $('#div2ndroundAdmProFrom').hide();
        $('#div2ndroundAdmProTo').hide();
        $('#date_2ndadmissionFrom').val('');
        $('#date_2ndadmissionTo').val('');
    }  
});

$('#chk2ndelistofadmitCan').change(function () {
    if ($('#chk2ndelistofadmitCan').prop('checked')) {
        $('#div2ndelistofadmitCan').show();
       
    }
    else {
        $('#div2ndelistofadmitCan').hide();
        $('#date_canVacDate').val('');
    }    
});

$('#chk3rdroundEntry').change(function () {
    if ($('#chk3rdroundEntry').prop('checked')) {
        $('#div3rdroundEntryFrom').show();
        $('#div3rdroundEntryTo').show();
       
    }
    else {
        $('#div3rdroundEntryFrom').hide();
        $('#div3rdroundEntryTo').hide();
        $('#date_3rdroundDueFrom').val('');
        $('#date_3rdroundDueTo').val('');
    }    
});

$('#chkDatabasebackup3rdRound').change(function () {
    if ($('#chkDatabasebackup3rdRound').prop('checked')) {
        $('#divDatabasebackup3rdRound').show();

    }
    else {
        $('#divDatabasebackup3rdRound').hide();
        $('#date_3rdseatDate').val('');
    }
});

$('#chk3rdRoundAdmPro').change(function () {
    if ($('#chk3rdRoundAdmPro').prop('checked')) {
        $('#div3rdRoundAdmProFrom').show();
        $('#div3rdRoundAdmProTo').show();

    }
    else {
        $('#div3rdRoundAdmProFrom').hide();
        $('#div3rdRoundAdmProTo').hide();
        $('#date_3rdroundAdmisFrom').val('');
        $('#date_3rdroundAdmisTo').val('');
    }
});

$('#chkListcanadm3rdRound').change(function () {
    if ($('#chkListcanadm3rdRound').prop('checked')) {
        $('#divListcanadm3rdRound').show();
    }
    else {
        $('#divListcanadm3rdRound').hide();
        $('#date_3rdAdminCabVac').val('');
    }
});

$('#chkFinalRoundEntry').change(function () {
    if ($('#chkFinalRoundEntry').prop('checked')) {
        $('#divFinalRoundEntryFrom').show();
        $('#divFinalRoundEntryTo').show();
    }
    else {
        $('#divFinalRoundEntryFrom').hide();
        $('#divFinalRoundEntryTo').hide();
        $('#date_finalTradeFrom').val('');
        $('#date_finalTradeTo').val('');
    }
});

$('#chkFinalseatallot').change(function () {
    if ($('#chkFinalseatallot').prop('checked')) {
        $('#divFinalseatallot').show();
    }
    else {
        $('#divFinalseatallot').hide();   
        $('#date_finalSeatall').val('');
    }
});

$('#chkAdmFinalRound').change(function () {
    if ($('#chkAdmFinalRound').prop('checked')) {
        $('#divAdmFinalRoundFrom').show();
        $('#divAdmFinalRoundTo').show();
    }
    else {
        $('#divAdmFinalRoundFrom').hide();
        $('#divAdmFinalRoundTo').hide();
        $('#date_FinalAdminFrom').val('');
        $('#date_FinalAdminTo').val('');
    }
});

$('#chkCommencementTra').change(function () {
    if ($('#chkCommencementTra').prop('checked')) {
        $('#divCommencementTra').show();
    }
    else {
        $('#divCommencementTra').hide();
        $('#date_CommTrain').val('');
    }
});
$('#chkSeatVacancynxtrnd').change(function () {
    if ($('#chkSeatVacancynxtrnd').prop('checked')) {
        $('#divSeatVacancynxtrnd').show();
    }
    else {
        $('#divSeatVacancynxtrnd').hide();
        $('#date_seatvacancy').val('');
    }
});

$('#chkPub_GrdList').change(function () {
    if ($('#chkPub_GrdList').prop('checked')) {
        $('#divPub_GrdList').show();
    }
    else {
        $('#divPub_GrdList').hide();
        $('#Pub_GrdList').val('');
    }
});

$('#chkPub_GrdListrnd2').change(function () {
    if ($('#chkPub_GrdListrnd2').prop('checked')) {
        $('#divPub_GrdListrnd2').show();
    }
    else {
        $('#divPub_GrdListrnd2').hide();
        $('#Pub_GrdListrnd2').val('');
    }
});

$('#chkRnd2seatAllotment').change(function () {
    if ($('#chkRnd2seatAllotment').prop('checked')) {
        $('#divRnd2seatAllotment').show();
    }
    else {
        $('#divRnd2seatAllotment').hide();
        $('#Rnd2seatAllotment').val('');
    }
});

$('#chkPub_GrdListrnd3').change(function () {
    if ($('#chkPub_GrdListrnd3').prop('checked')) {
        $('#divPub_GrdListrnd3').show();
    }
    else {
        $('#divPub_GrdListrnd3').hide();
        $('#Pub_GrdListrnd3').val('');
    }
});
//**
function CalendarNextDate() {
    
    if ($('#date_notifApplyFrom').val() != '') {
        $('#chkDateapplonlineappfrm').prop('checked', true);
        $('#divDateapplonlineappfrm').show(); 
        $('#divDateapplonlineappfrmTo').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    
    else {
        $('#chkDateapplonlineappfrm').prop('checked', false);
        $('#divDateapplonlineappfrm').hide();
        $('#divDateapplonlineappfrmTo').hide();
    }
    if ($('#date_ofnotifFrom').val() != '') {
        $('#chkDateofnotiffrm').prop('checked', true);
        $('#divDateofnotifcationfrm').show();
        $('#divDateofnotifcationto').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chkDateofnotiffrm').prop('checked', false);
        $('#divDateofnotifcationfrm').hide();
        $('#divDateofnotifcationto').hide();
    }

    if ($('#date_Tentativefrm').val() != '') {
        $('#chktentativeseatfrm').prop('checked', true);
        $('#divtentativesetafrm').show();
        $('#divtentativesetato').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chktentativeseatfrm').prop('checked', false);
        $('#divtentativesetafrm').hide();
        $('#divtentativesetato').hide();
    }

    if ($('#date_VerifyFrom').val() != '') {
        $('#chkDocumentvrf').prop('checked', true);
        $('#divDocumentvrffrm').show();
        $('#divDocumentvrfTo').show();
    }
    else {
        $('#chkDocumentvrf').prop('checked', false);
        $('#divDocumentvrffrm').hide();
        $('#divDocumentvrfTo').hide();;
    }

    if ($('#date_AppDocVeriffrm').val() != '') {
        $('#chkApplicantvrf').prop('checked', true);
        $('#divApplicantDocvrffrm').show();
        $('#divApplicantDocvrfTo').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chkApplicantvrf').prop('checked', false);
        $('#divApplicantDocvrffrm').hide();
        $('#divApplicantDocvrfTo').hide();
    }

    if ($('#date_fnlseatmtrxfrm').val() != '') {
        $('#chkfinalseatmtrx').prop('checked', true);
        $('#divfnlseatmtrxfrm').show();
        $('#divfnlseatmtrxto').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chkfinalseatmtrx').prop('checked', false);
        $('#divfnlseatmtrxfrm').hide();
        $('#divfnlseatmtrxto').hide();
    }

    if ($('#date_eligible').val() != '') {
        $('#chkDisplayevList').prop('checked', true);
        $('#divDisplayevList').show();     
    }
    else {
        $('#chkDisplayevList').prop('checked', false);
        $('#divDisplayevList').hide();     
    }

    if ($('#date_dueDate').val() != '') {
        $('#chkDatabasebackup').prop('checked', true);
        $('#divDatabasebackup').show();

    }
    else {
        $('#chkDatabasebackup').prop('checked', false);
        $('#divDatabasebackup').hide();
    }

    if ($('#date_tentative').val() != '') {
        $('#chkDisplayTentative').prop('checked', true);
        $('#divDisplayTentative').show();

    }
    else {
        $('#chkDisplayTentative').prop('checked', false);
        $('#divDisplayTentative').hide();
    }
    if ($('#date_tntgrlstapplfrm').val() != '') {
        $('#chktntgrdbyapplicant').prop('checked', true);
        $('#divtntgrdlistbyapplfrm').show();
        $('#divtntgrdlistbyapplto').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chktntgrdbyapplicant').prop('checked', false);
        $('#divtntgrdlistbyapplfrm').hide();
        $('#divtntgrdlistbyapplto').hide();
    }

    if ($('#date_ApplgrbyDocvrofcrFrm').val() != '') {
        $('#chkApplDocvro').prop('checked', true);
        $('#divApplgrbyDocvrofcrfrm').show();
        $('#divApplgrbyDocvrofcrto').show();
        //$("#date_notifApplyFrom").datepicker("option", "disabled", true);
        //$('#date_notifApplyFrom').datepicker('enable');
    }
    else {
        $('#chkApplDocvro').prop('checked', false);
        $('#divApplgrbyDocvrofcrfrm').hide();
        $('#divApplgrbyDocvrofcrto').hide();
    }
    
    if ($('#date_final').val() != '') {
        $('#chkDisplayfinal').prop('checked', true);
        $('#divDisplayfinal').show();

    }
    else {
        $('#chkDisplayfinal').prop('checked', false);
        $('#divDisplayfinal').hide();
    }

    if ($('#date_seatallotment').val() != '') {
        $('#chkfstlistseatAllot').prop('checked', true);
        $('#divfstlistseatAllot').show();
    }
    else {
        $('#chkfstlistseatAllot').prop('checked', false);
        $('#divfstlistseatAllot').hide();
    }

    if ($('#date_admittedCan').val() != '') {
        $('#chkfstlistAdmitcan').prop('checked', true);
        $('#divfstlistAdmitcan').show();
    }
    else {
        $('#chkfstlistAdmitcan').prop('checked', false);
        $('#divfstlistAdmitcan').hide();
    }

    if ($('#date_1stRoundFrom').val() != '') {
        $('#chkfstroundadmPro').prop('checked', true);
        $('#divfstroundadmProFrom').show();
        $('#divfstroundadmProTo').show();
    }
    else {
        $('#chkfstroundadmPro').prop('checked', false);
        $('#divfstroundadmProFrom').hide();
        $('#divfstroundadmProTo').hide();
    }

    if ($('#date_2ndRoundFrom').val() != '') {
        $('#chk2ndrndEntry').prop('checked', true);
        $('#div2ndrndEntryFrom').show();
        $('#div2ndrndEntryTo').show();
    }
    else {
        $('#chk2ndrndEntry').prop('checked', false);
        $('#div2ndrndEntryFrom').hide();
        $('#div2ndrndEntryTo').hide();
    }

    if ($('#date_2ndSeatFrom').val() != '') {
        $('#chkDatabasebackup2nd').prop('checked', true);
        $('#divDatabasebackup2ndFrom').show();
        $('#divDatabasebackup2ndTo').show();
    }
    else {
        $('#chkDatabasebackup2nd').prop('checked', false);
        $('#divDatabasebackup2ndFrom').hide();
        $('#divDatabasebackup2ndTo').hide();
    }

    if ($('#date_2ndadmissionFrom').val() != '') {
        $('#chk2ndroundAdmPro').prop('checked', true);
        $('#div2ndroundAdmProFrom').show();
        $('#div2ndroundAdmProTo').show();
    }
    else {
        $('#chk2ndroundAdmPro').prop('checked', false);
        $('#div2ndroundAdmProFrom').hide();
        $('#div2ndroundAdmProTo').hide();
    }

    if ($('#date_canVacDate').val() != '') {
        $('#chk2ndelistofadmitCan').prop('checked', true);
        $('#div2ndelistofadmitCan').show();       
    }
    else {
        $('#chk2ndelistofadmitCan').prop('checked', false);
        $('#div2ndelistofadmitCan').hide();     
    }

    if ($('#date_3rdroundDueFrom').val() != '') {
        $('#chk3rdroundEntry').prop('checked', true);
        $('#div3rdroundEntryFrom').show();
        $('#div3rdroundEntryTo').show();
    }
    else {
        $('#chk3rdroundEntry').prop('checked', false);
        $('#div3rdroundEntryFrom').hide();
        $('#div3rdroundEntryTo').hide();
    }

    if ($('#date_3rdseatDate').val() != '') {
        $('#chkDatabasebackup3rdRound').prop('checked', true);
        $('#divDatabasebackup3rdRound').show();
    }
    else {
        $('#chkDatabasebackup3rdRound').prop('checked', false);
        $('#divDatabasebackup3rdRound').hide();
    }

    if ($('#date_3rdroundAdmisFrom').val() != '') {
        $('#chk3rdRoundAdmPro').prop('checked', true);
        $('#div3rdRoundAdmProFrom').show();
        $('#div3rdRoundAdmProTo').show();
    }
    else {
        $('#chk3rdRoundAdmPro').prop('checked', false);
        $('#div3rdRoundAdmProFrom').hide();
        $('#div3rdRoundAdmProTo').hide();
    }

    if ($('#date_3rdAdminCabVac').val() != '') {
        $('#chkListcanadm3rdRound').prop('checked', true);
        $('#divListcanadm3rdRound').show();
    }
    else {
        $('#chkListcanadm3rdRound').prop('checked', false);
        $('#divListcanadm3rdRound').hide();
    }

    if ($('#date_finalTradeFrom').val() != '') {
        $('#chkFinalRoundEntry').prop('checked', true);
        $('#divFinalRoundEntryFrom').show();
        $('#divFinalRoundEntryTo').show();
    }
    else {
        $('#chkFinalRoundEntry').prop('checked', false);
        $('#divFinalRoundEntryFrom').hide();
        $('#divFinalRoundEntryTo').hide();
    }

    if ($('#date_finalSeatall').val() != '') {
        $('#chkFinalseatallot').prop('checked', true);
        $('#divFinalseatallot').show();
    }
    else {
        $('#chkFinalseatallot').prop('checked', false);
        $('#divFinalseatallot').hide();
    }
    if ($('#date_FinalAdminFrom').val() != '') {
        $('#chkAdmFinalRound').prop('checked', true);
        $('#divAdmFinalRoundFrom').show();
        $('#divAdmFinalRoundTo').show();
    }
    else {
        $('#chkAdmFinalRound').prop('checked', false);
        $('#divAdmFinalRoundFrom').hide();
        $('#divAdmFinalRoundTo').hide();
    }

    if ($('#date_CommTrain').val() != '') {
        $('#chkCommencementTra').prop('checked', true);
        $('#divCommencementTra').show();
    }
    else {
        $('#chkCommencementTra').prop('checked', false);
        $('#divCommencementTra').hide();
    }

    if ($('#date_seatvacancy').val() != '') {
        $('#chkSeatVacancynxtrnd').prop('checked', true);
        $('#divSeatVacancynxtrnd').show();
    }
    else {
        $('#chkSeatVacancynxtrnd').prop('checked', false);
        $('#divSeatVacancynxtrnd').hide();
    }

    if ($('#Pub_GrdList').val() != '') {
        $('#chkPub_GrdList').prop('checked', true);
        $('#divPub_GrdList').show();
    }
    else {
        $('#chkPub_GrdList').prop('checked', false);
        $('#divPub_GrdList').hide();
    }
    if ($('#Pub_GrdListrnd2').val() != '') {
        $('#chkPub_GrdListrnd2').prop('checked', true);
        $('#divPub_GrdListrnd2').show();
    }
    else {
        $('#chkPub_GrdListrnd2').prop('checked', false);
        $('#divPub_GrdListrnd2').hide();
    }

    if ($('#Rnd2seatAllotment').val() != '') {
        $('#chkRnd2seatAllotment').prop('checked', true);
        $('#divRnd2seatAllotment').show();
    }
    else {
        $('#chkRnd2seatAllotment').prop('checked', false);
        $('#divRnd2seatAllotment').hide();
    }
    if ($('#Pub_GrdListrnd3').val() != '') {
        $('#chkPub_GrdListrnd3').prop('checked', true);
        $('#divPub_GrdListrnd3').show();
    }
    else {
        $('#chkPub_GrdListrnd3').prop('checked', false);
        $('#divPub_GrdListrnd3').hide();
    }
}

function GetAdmissionCalendarDetails() {
    $.ajax({
        type: "GET",
        url: "/Admission/GetAdmissionCalendarView",
        contentType: "application/json",
        success: function (data) {
            $('#AdmissionCalendarNtfTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type Name', 'className': 'text-center' },
                /* { 'data': 'DeptName', 'title': 'Name of Section', 'className': 'text-left' },*/
                    { 'data': 'applicantTypeName', 'title': 'Notification Type', 'className': 'text-left' },
                    { 'data': 'Exam_Notif_Number', 'title': 'Notification Number', 'className': 'text-left' },
                    { 'data': 'Exam_Notif_Desc', 'title': 'Notification Description', 'className': 'text-left' },
                    {
                        'data': 'Notification_Date', 'title': 'Notification Date', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.Notification_Date).split(' ')[0];
                            $(nTd).html(date);
                        }
                    },
                    { 'data': 'exam_notif_status_desc', 'title': 'Status ' + '- Currently with', 'className': 'text-left' },
                    {
                        'data': 'Tentative_admsn_evnt_clndrId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {      
                            if (oData.StatusId == 101 || oData.StatusId == 110)
                                $(nTd).html("<input type='button' disabled='disabled' onclick='GetCommentTable(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");
                            else
                                $(nTd).html("<input type='button' onclick='GetCommentTable(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='Remarks1'/>");

                        }
                    },

                    {
                        'data': 'Tentative_admsn_evnt_clndrId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.StatusId == 109) {
                                if (oData.role_id == 8) {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='View'/><input type='button' onclick='PublishCalendarNotification(" + oData.Tentative_admsn_evnt_clndrId + ",\"" + oData.Exam_Notif_Number + "\")' class='btn btn-success btn-xs' value='Publish' id='PublishAd' />");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                }
                            }
                            else if (oData.StatusId == 101 || oData.StatusId == 103 || oData.StatusId == 107 || oData.StatusId == 108) {
                                if (oData.role_id == 8) {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='edit'/><a href='/Admission/AdmissionCalendar?calendarId=" + oData.Tentative_admsn_evnt_clndrId + "' class='btn btn-primary btn-xs' value='Edit'>Edit</a>");
                                }
                                else {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='edit'/>");
                                }
                            }
                            else if (oData.StatusId == 110) {
                                    if (oData.role_id == 8) {
                                        $(nTd).html("<input type='button' disabled='disabled' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='View'/><a href='/Admission/AdmissionCalendar?calendarId=" + oData.Tentative_admsn_evnt_clndrId + "' class='btn btn-primary btn-xs' value='Edit'>Edit</a>");
                                    } else {
                                        $(nTd).html("<input type='button' disabled='disabled' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                                    }

                                }
                             else {
                                    $(nTd).html("<input type='button' onclick='viewAdmissionCalendarDetails(" + oData.Tentative_admsn_evnt_clndrId + ")' class='btn btn-primary btn-xs' value='View' id='edit'/>");

                            }
                        }
                        
                    }
                ]
            });
        }
    });
}

function PublishAdmissionNotification(notificationId) {
    $.ajax({
        type: "Get",
        url: "/Admission/PublishAdmissionCalNotification",
        data: { notificationId: notificationId },
        success: function (data) {
            if (data == true) {
                bootbox.alert("</br> published successfully");
                GetAdmissionCalendarDetails();
            }
            else
                bootbox.alert("</br> failed");
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
    
   // var id = $('#id').html();
    $('#RemarksCommentsModal').modal('show');
    $.ajax({
        type: "Get",
        url: "/Admission/GetCommentsCalendarFile/" + id,
        data: { id: id },
        success: function (data) {
            var t = $('#CommentsTable').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'createdatetime', 'title': 'Date', 'className': 'text-left' },
                    //{ 'data': 'user_name', 'title': 'Designation', 'className': 'text-left' },
                    { 'data': 'FromUser', 'title': 'From', 'className': 'text-left' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-left' },
                    { 'data': 'Status', 'title': 'Status', 'className': 'text-left' },                                     
                    { 'data': 'comments', 'title': 'Remarks Description', 'className': 'text-left' },                  
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

$("#CancelBtn").click(function () {
    $('#ViewDiv').hide();
    $('#GridListDiv').show();
    $('#remarks').val("");
    $('#users').val(0);
    $('#users1').val(0);
    $("#users").prop("disabled", false);
    $("#users1").prop("disabled", false);
    //$('#tableGrid').show();
    //$('#ViewDetailsDiv').hide();
});

function viewAdmissionCalendarDetails(id) {
    $('#GridListDiv').hide();
    $('#ViewDiv').show();
    $("#remarks").text('');
    ExamNotifyId = id
    $.ajax({
        type: "POST",
        url: "/Admission/GetAdmissionCalendarDetails",
        dataType: 'json',
        data: { id: id },
        success: function (response) {

            $("#id").html(response.Tentative_admsn_evnt_clndrId);
            $("#course").html(response.CourseTypeName);
            //$("#dept").html(response.DeptName);
            $("#ntfType").html(response.applicantTypeName);
            $("#notifynum").html(response.Exam_Notif_Number);
            $("#desc1").html(response.NotifDesc);
            var dat = daterangeformate2(response.Dt_Notification).split(' ')[0];
            $('#datenotifycal').html(dat);
            $('#statusdesc').html(response.exam_notif_status_desc);
            $('#RecordLevel').html(response.Role);
            StatusDesc = response.exam_notif_status_desc;
            $('#emailIdCOE').html(response.NtfEmailId);

            //var today = new Date();
            //var yyyy = today.getFullYear();
            //var nextyyyy = String(today.getFullYear() + 1).padStart(2,'0');
            //today = yyyy + '-' + nextyyyy;      
            var parts = dat.split("-")[2];
            var vv = parts.split(" ")[0];
            //conversiondate = new Date(vv);
            Acyear = (vv);
            $("#academicyear").html(Acyear);

            //New Requirment
            var from = daterangeformate2(response.FromDt_ApplyingOnlineApplicationForm).split(' ')[0];
            $('#idDaopF').html(from);
            var To = daterangeformate2(response.ToDt_ApplyingOnlineApplicationForm).split(' ')[0];
            $('#idDaopT').html(To);

            var from1 = daterangeformate2(response.FromDt_DocVerificationPeriod).split(' ')[0];
            $('#IdDvpF').html(from1);
            var To1 = daterangeformate2(response.ToDt_DocVerificationPeriod).split(' ')[0];
            $('#IdDvpT').html(To1);

            var IdDevld = daterangeformate2(response.Dt_DisplayEigibleVerifiedlist).split(' ')[0];
            $('#IdDevld').html(IdDevld);
            var IdOavddTime = daterangeformate2(response.Dt_DBBackupSeatMatrixFInalByDept).split(' ')[0];
            $('#IdOavddTime').html(IdOavddTime);

            var IdTentative = daterangeformate2(response.Dt_DisplayTentativeGradation).split(' ')[0];
            $('#IdTentative').html(IdTentative);
            var IdFinal = daterangeformate2(response.Dt_DisplayFinalGradationList).split(' ')[0];
            $('#IdFinal').html(IdFinal);

            var IdLasd = daterangeformate2(response.Dt_1stListSeatAllotment).split(' ')[0];
            $('#IdLasd').html(IdLasd);
            var IdLacd = daterangeformate2(response.Dt_1stListAdmittedCand).split(' ')[0];
            $('#IdLacd').html(IdLacd);

            var IdRapdF = daterangeformate2(response.FromDt_1stRoundAdmissionProcess).split(' ')[0];
            $('#IdRapdF').html(IdRapdF);
            var IdRapdT = daterangeformate2(response.ToDt_1stRoundAdmissionProcess).split(' ')[0];
            $('#IdRapdT').html(IdRapdT);

            var IdRed2F = daterangeformate2(response.FromDt_2ndRoundEntryChoiceTrade).split(' ')[0];
            $('#IdRed2F').html(IdRed2F);
            var IdRed2T = daterangeformate2(response.ToDt_2ndRoundEntryChoiceTrade).split(' ')[0];
            $('#IdRed2T').html(IdRed2T);

            var IdLsad2F = daterangeformate2(response.FromDt_DbBkp2ndRoundOnlineSeat).split(' ')[0];
            $('#IdLsad2F').html(IdLsad2F);
            var IdLsad2T = daterangeformate2(response.ToDt_DbBkp2ndRoundOnlineSeat).split(' ')[0];
            $('#IdLsad2T').html(IdLsad2T);

            var IdRapd2F = daterangeformate2(response.FromDt_2ndRoundAdmissionProcess).split(' ')[0];
            $('#IdRapd2F').html(IdRapd2F);
            var IdRapd2T = daterangeformate2(response.ToFDt_2ndRoundAdmissionProcess).split(' ')[0];
            $('#IdRapd2T').html(IdRapd2T);

            var IdLoacavld2 = daterangeformate2(response.Dt_2ndAdmittedCand).split(' ')[0];
            $('#IdLoacavld2').html(IdLoacavld2);

            var IdRecotddF = daterangeformate2(response.FromDt_3rdRoundEntryChoiceTrade).split(' ')[0];
            $('#IdRecotddF').html(IdRecotddF);
            var IdRecotddT = daterangeformate2(response.ToDt_3rdRoundEntryChoiceTrade).split(' ')[0];
            $('#IdRecotddT').html(IdRecotddT);

            var Id3Lsad = daterangeformate2(response.Dt_DbBkp3rdRoundOnlineSeat).split(' ')[0];
            $('#Id3Lsad').html(Id3Lsad);

            var Id3RappdF = daterangeformate2(response.FromDt_3rdRoundAdmissionProcess).split(' ')[0];
            $('#Id3RappdF').html(Id3RappdF);
            var Id3RappdT = daterangeformate2(response.ToDt_3rdRoundAdmissionProcess).split(' ')[0];
            $('#Id3RappdT').html(Id3RappdT);

            var IdLoacavld3 = daterangeformate2(response.Dt_3rdAdmittedCand).split(' ')[0];
            $('#IdLoacavld3').html(IdLoacavld3);

            var IdFrecotddF = daterangeformate2(response.FromDt_FinalRoundEntryChoiceTrade).split(' ')[0];
            $('#IdFrecotddF').html(IdFrecotddF);
            var IdFrecotddT = daterangeformate2(response.ToDt_FinalRoundEntryChoiceTrade).split(' ')[0];
            $('#IdFrecotddT').html(IdFrecotddT);

            var IdFRsaddd = daterangeformate2(response.Dt_FinalRoundSeatAllotment).split(' ')[0];
            $('#IdFRsaddd').html(IdFRsaddd);

            var IdAFrdF = daterangeformate2(response.FromDt_AdmissionFinalRound).split(' ')[0];
            $('#IdAFrdF').html(IdAFrdF);
            var IdAFrdT = daterangeformate2(response.ToDt_AdmissionFinalRound).split(' ')[0];
            $('#IdAFrdT').html(IdAFrdT);

            var IdCotD = daterangeformate2(response.Dt_CommencementOfTraining).split(' ')[0];
            $('#IdCotD').html(IdCotD);

            //New calendar events data fetch
            var FromDt_TntGrlist = daterangeformate2(response.FromDt_TntGrlistbyAppl).split(' ')[0];
            $('#FromDt_TntGrlist').html(FromDt_TntGrlist);

            var ToDt_TntGrlist = daterangeformate2(response.ToDt_TntGrlistbyAppl).split(' ')[0];
            $('#ToDt_TntGrlist').html(ToDt_TntGrlist);

            var FromDt_ApplDocVer = daterangeformate2(response.FromDt_ApplDocVerif).split(' ')[0];
            $('#FromDt_ApplDocVer').html(FromDt_ApplDocVer);

            var ToDt_ApplDocVer = daterangeformate2(response.ToDt_ApplDocVerif).split(' ')[0];
            $('#ToDt_ApplDocVer').html(ToDt_ApplDocVer);

            var FromDt_AplGrVR = daterangeformate2(response.FromDt_AplGrVRoffcr).split(' ')[0];
            $('#FromDt_AplGrVR').html(FromDt_AplGrVR);

            var ToDt_AplGrVR = daterangeformate2(response.ToDt_AplGrVRoffcr).split(' ')[0];
            $('#ToDt_AplGrVR').html(ToDt_AplGrVR);

            var Publishgrdlist3 = daterangeformate2(response.Publishgrdlist3Rnd).split(' ')[0];
            $('#Publishgrdlist3').html(Publishgrdlist3);

            var FromDt_Tentatvive = daterangeformate2(response.FromDt_Tentatviveseat).split(' ')[0];
            $('#FromDt_Tentatvive').html(FromDt_Tentatvive);

            var Publishgrdlist2 = daterangeformate2(response.Publishgrdlist2Rnd).split(' ')[0];
            $('#Publishgrdlist2').html(Publishgrdlist2);
            //$('#idDaopf').html(response.ToDt_ApplyingOnlineApplicationForm)
            //***//
            //New Concept
            var pdfFrom = daterangeformate2(response.FromDt_ApplyingOnlineApplicationForm).split(' ')[0];
            $('#dateAoaf').html(pdfFrom);
            var pdfTo = daterangeformate2(response.ToDt_ApplyingOnlineApplicationForm).split(' ')[0];
          $('#dateAoat').html(pdfTo);
          if (response.login_id == 2) {
            $('#forwardtolbl').hide();           
            $('#users').hide();           
            $('#sendBtn1').hide();            
          }
            $("#lblDateofnotiffrmPDF").html("Date Of Notification 1");
            $("#lbltentativeseatfrmPDF").html("Announce the Tentative Seat matrix data to the public");
            //$(".divfinalseatmtrx").show();
            $("#lblPub_GrdListPDF").html("Publish of gradation list for the 1st Round");
            $("#lblfstlistseatAllotPDF").html("1st round of online seat allotment");
            $("#lblfstroundadmProPDF").html("1st round admission process at ITI's");
            $("#lblfstlistAdmitcanPDF").html("vacancy of seats (Seat Matrix) available for the 2nd round seat allocation");
            $("#lbl2ndrndEntryPDF").html("2nd round Entry choice of trades / ITI in the Portal is compulsory for remaining Seats for those who have applied & not got the seats in First Round & those" +
                       "who have applied & allotted but not availed the admission in the first round");
            $(".hideShowNot2SectionPDF").show();
            if (response.applicantTypeId == 2) {
                $("#lbl2ndrndEntryPDF").html("Casual round seat allocation at each ITI institutes");
                $("#lblDateofnotiffrmPDF").html($("#lblDateofnotiffrm").text().replace('1', '2'));
                $("#lbltentativeseatfrmPDF").html("Announce the seat availability data/ Seat matrix data to the public");
                //$(".divfinalseatmtrx").hide();
                $("#lblPub_GrdListPDF").html("Publish of final gradation list");
                $("#lblfstlistseatAllotPDF").html($("#lblfstlistseatAllotPDF").text().replace('1st', 'Final'));
                $("#lblfstroundadmProPDF").html($("#lblfstroundadmProPDF").text().replace('1st round ', '').replace(" at ITI's", ""));
                $("#lblfstlistAdmitcanPDF").html($("#lblfstlistAdmitcanPDF").text().replace('(', '(Final').replace("2nd", "casual"));
                $(".hideShowNot2SectionPDF").hide();
            }

            if (response.forwardStatus == true) {
                //$('#sendBtn1').removeAttr('disabled');
                //$('#sendBackBtn').removeAttr('disabled'); 
                $('#sendBtn1').removeAttr('disabled');
                $('#sendBackBtn').removeAttr('disabled');
                $('#sendBtn3').removeAttr('disabled'); 
            }
            else {
                //$('#sendBtn1').attr('disabled', 'disabled');
                //$('#sendBackBtn').attr('disabled', 'disabled'); 
                $('#sendBtn1').attr('disabled', 'disabled');
                $('#sendBackBtn').attr('disabled', 'disabled');
                $('#sendBtnchanges').attr('disabled', 'disabled');
                $('#sendBtn3').attr('disabled', 'disabled'); 
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

                //$('#sendBtn3').show();
                //$('#sendBtn3').removeAttr('disabled');
            }
            else {
                //$('#sendBtn3').show();
                $('#sendBtn3').hide();
                $('#sendBtnchanges').attr('disabled', 'disabled');
            }

            //if (response.ChangesStatus == true) {
            //    $('#sendBtnchanges').show();
            //}

          if (response.StatusId == 109) {
            $('#users').attr('disabled', 'disabled');
            $('#users1').attr('disabled', 'disabled');
                $('#sendBtn1').attr('disabled', 'disabled');
                $('#sendBackBtn').attr('disabled', 'disabled');
            }
            //else if (response.StatusId == 108) {
            //    $('#sendBtn1').show();
            //    $('#sendBtn1').removeAttr('disabled');
            //}
            else if (response.StatusId == 106) {
                if (response.RoleId == 1 || response.RoleId == 2 || response.RoleId == 4 || response.RoleId == 6) {                   
                    $('#sendBtnchanges').show();
                    $('#sendBtnchanges').removeAttr('disabled');                  
                }
                else {
                    $('#sendBtnchanges').hide();
                    $('#sendBtnchanges').attr('disabled', 'disabled');
            }
            $('#users').attr('disabled', 'disabled');
            $('#users1').attr('disabled', 'disabled');
            $('#sendBtn1').attr('disabled', 'disabled');
            $('#sendBackBtn').attr('disabled', 'disabled');
                $('#sendBtn3').hide();                                
            }
            if (response.RoleId == 1) {
                $('#sendBtn1').attr('disabled', 'disabled');
            }
            if (response.RoleId == 8)
                $('#sedbackdiv').hide();
            else
                $('#sedbackdiv').show();

            if (response.StatusId == 106||response.StatusId == 107) {                
                $('#sendBackBtn').attr('disabled', 'disabled'); 
            }
            if (response.RoleId < 7) {
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
        }, error: function (e) {
            var _msg = "</br> Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });
}

//Getusers("users");
//Getusers("users1");
//function Getusers(user,level) {
//    $("#" + user).empty();
//    $("#" + user).append('<option value="choose">choose</option>');
//    $.ajax({
//        //url: "/Admission/GetRolesCal",
//		url: "/Admission/GetRoles",
//        type: 'Get',
//        data: { 'level': level },
//        contentType: 'application/json; charset=utf-8',
//        success: function (data) {
//            if (data != null || data != '') {
//                $.each(data, function () {
//                   // $("#users").append($("<option/>").val(this.RoleID).text(this.RoleName));
//                    $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
//                });
//            }

//        }, error: function (result) {
//            bootbox.alert("Error", "</br> something went wrong");
//        }
//    });
//}

function ForwardAdmissionCalendarNotification() {
   
    var notificatinNumber = $('#notifynum').html();
    //var RoleSelected = $('#users :selected').text();
    var RoleSelected = $('#users :selected').val();

    var roleId = $('#users').val();
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();

    var IsValid = true;

    if (remarks == 0 || remarks == "" || remarks == null) {
        IsValid = false;
        $('#txtRemarks-Required').show();
    }
    else if (IsValid == true) {
        $('#txtRemarks-Required').hide();
        if (roleId == 0) {
            bootbox.alert('</br> please select role');
        }
        else {
            bootbox.confirm({
                message: "</br> Do you want to Submit the calendar of event, Notification No. <b> " + $('#notifynum').text() + "</b> to <b> " + $('#users :selected').text() + "</b> for review" + ($('#users :selected').val() == loginUserRole.JD || $('#users :selected').val() == loginUserRole.ADL || $('#users :selected').val() == loginUserRole.Director || $('#users :selected').val() == loginUserRole.Commissioner ? " and Approve" : "") + " ?",
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
                            url: "/Admission/ForwardAdmissionCalendarNotification",
                            type: 'Get',
                            data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {

                                if (data == true) {
                                    //alert('forwaded successfully' + $('#notifNumber').val());
                                    //bootbox.alert("Admission notification No : <b>" + notificatinNumber + "</b> Forwaded To <b>" + RoleSelected + "</b> Successfully");
                                    var msg = "</br> <b>" + $('#RecordLevel').html() + "</b> has Submitted Calendar of Event, Notification No: <b>" + notificatinNumber + "</b> To <b>" + $('#users :selected').text() + "</b> for review " + ($('#users :selected').val() == loginUserRole.JD || $('#users :selected').val() == loginUserRole.ADL || $('#users :selected').val() == loginUserRole.Director || $('#users :selected').val() ==loginUserRole.Commissioner ? " and Approve" : "") + " Successfully"
                                    bootbox.alert(msg);
                                    GetAdmissionCalendarDetails();
                                    $("#ViewDiv").hide();
                                    $("#GridListDiv").show();
                                    $("#remarks").val("");
                                    $('#users').val("choose");
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
/*code commented by sujit*/
    //else {
    //    bootbox.confirm('Do you want to forward the notification?', (confirma) => {
    //        if (confirma) {
    //            $.ajax({
    //                url: "/Admission/ForwardAdmissionCalendarNotification",
    //                type: 'Get',
    //                data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
    //                contentType: 'application/json; charset=utf-8',
    //                success: function (data) {
                       
    //                    if (data == true) {
    //                        //alert('forwaded successfully' + $('#notifNumber').val());
    //                        //bootbox.alert("Admission notification No : <b>" + notificatinNumber + "</b> Forwaded To <b>" + RoleSelected + "</b> Successfully");
    //                        bootbox.alert("<b>" + $('#RecordLevel').html() + "</b> has Forwaded Calendar Event notification No: <b>" + notificatinNumber + "</b> To <b>" + RoleSelected + "</b> Successfully");
    //                        GetAdmissionCalendarDetails();
    //                        $("#ViewDiv").hide();
    //                        $("#GridListDiv").show();
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

//New_05052021
function SenDBackAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html();
    var RoleSelected = $('#users1 :selected').text();    
    var roleId = $('#users1').val();
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    var IsValid = true;

    if (remarks == 0 || remarks == "" || remarks == null) {
        IsValid = false;
        $('#txtRemarks-Required').show();
    }
    else if (IsValid == true) {
        $('#txtRemarks-Required').hide();
        if (roleId == 0) {
            bootbox.alert('</br> please select the role');
        }
        else {
            bootbox.confirm({
                message: "</br> Do you want to send back the calendar of event, Notification No.<b>" + $('#notifynum').text() + "</b> to <b>" + $('#users1 :selected').text() + "</b> ?",
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
                            url: "/Admission/SendbackAdmissionCalNotification",
                            type: 'Get',
                            data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                            contentType: 'application/json; charset=utf-8',
                            success: function (data) {
                                if (data == true) {
                                    //alert("sendback successfully");
                                    //bootbox.alert("Calendar notification No <b>" + notificatinNumber + "</b> is successfully send back to <b>" + RoleSelected + "</b>");
                                    bootbox.alert("</br> <b>" + $('#RecordLevel').html() + "</b> has Sent Back Calendar of Event, Notification No <b>" + notificatinNumber + "</b>  successfully to <b>" + RoleSelected + "</b> for more clarification");
                                    $("#GridListDiv").show();
                                    $("#ViewDiv").hide();
                                    GetAdmissionCalendarDetails();
                                    $("#remarks").val("");
                                    $('#users1').val("choose");
                                }
                                else {
                                    //alert("failed");
                                    bootbox.alert("</br> send back failed");
                                }
                            }, error: function (result) {
                                bootbox.alert("Error", "</br> something went wrong");
                            }
                        });
                    }
                }
            });
        }
    }   
}

//ExtraCode 
function SenDBackAdmissionCalendar() {
    var roleId = $('#users1').val();
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    if (roleId == 'choose') {
        bootbox.alert('</br> please select the role');
    } else {
        bootbox.confirm('</br> Do you want to send back the notification?', (confirma) => {
            if (confirma) {
                $.ajax({
                    url: "/Admission/SendbackAdmissionCalNotification",
                    type: 'Get',
                    data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data == true) {
                            bootbox.alert("</br> sendback successfully");
                            $('#ViewDiv').hide();
                            $('#GridListDiv').show();
                            GetAdmissionCalendarDetails();
                        }
                        else
                            bootbox.alert("</br> send back failed");

                    }, error: function (result) {
                        bootbox.alert("Error", "</br> something went wrong");
                    }
                });
            }
        });
    }
}
//**


function ApproveAdmissionCalendarNotification() {
    //var roleId = 8;
    //var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    if (remarks == "") {
        return bootbox.alert("Pls enter Remarks");

    }
    var notificatinNumber = $('#notifynum').html();

    bootbox.confirm({
        message: "</br> Do you want to approve the calendar of event, Notification No :<b>" + notificatinNumber +"</b> ? ",
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
            var remarks = $("#remarks").val();
            $.ajax({
                url: "/Admission/ApproveAdmissionCalendarNotification",
                type: 'Get',
                data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    
                    if (data == true) {
                        //alert("approved successfully");
                       // bootbox.alert("Calendar notification No <b>" + notificatinNumber + "</b> is successfully approved");
                        bootbox.alert("</br> <b>" + $('#RecordLevel').html() + "</b> has approved Calendar of Event, Notification No : <b>" + notificatinNumber + "</b> successfully ");
                        $("#ViewDiv").hide();
                        $("#GridListDiv").show();
                        $("#remarks").val("");
                        GetAdmissionCalendarDetails();

                    }
                    else {
                        //alert("failed");
                        bootbox.alert("</br> failed");
                    }
                }, error: function (result) {
                    bootbox.alert("Error", "</br> something went wrong");
                }
            });
        }
        }
    });

}

function ChangesCalendarAdmissionNotification() {
    var notificatinNumber = $('#notifynum').html();
    var roleId = 8;
    var admsnNotId = $("#id").html();
    var remarks = $("#remarks").val();
    if (remarks == "") {
      return  bootbox.alert("Pls enter remarks");
    }
    var record = $('#RecordLevel').text();
    //var loginId = $('#hdnSession').val(); 
    var loginId = $('#hdnSession').data('value');
    var loginName = $("#hdnSessionRole").data('value');

    bootbox.confirm({
        message: "</br> Do you want to request changes to calendar events,Notification No.<b>" + $('#notifynum').text() + "</b> to <b>" + record + "</b> ?",
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
                url: "/Admission/ChangesAdmissionCalNotification",
                type: 'Get',
                data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    if (data == true) {
                        //alert("Changes request sent successfully");
                        //bootbox.alert("Calendar notification No <b>" + notificatinNumber + "</b> Changes request sent to Case Worker successfully");
                        bootbox.alert("</br> <b>" + loginName + "</b> has requested  Changes to Calendar Event, Notification No: <b>" + notificatinNumber + "</b>  To <b> Case Worker </b> successfully");
                        $('#ViewDiv').hide();
                        $('#GridListDiv').show();
                        $("#remarks").val("");
                        GetAdmissionCalendarDetails();
                    }
                    else {                       
                        bootbox.alert("</br> failed");
                    }


                }, error: function (result) {
                    bootbox.alert("Error", "</br> something went wrong");
                }
            });
        }
        }
    });   
    /*code commented by sujit*/
    //bootbox.confirm('Do you want to request for changes of notification?', (confirma) => {
    //    if (confirma) {
    //        $.ajax({
    //            url: "/Admission/ChangesAdmissionCalNotification",
    //            type: 'Get',
    //            data: { id: roleId, admiNotifId: admsnNotId, remarks: remarks },
    //            contentType: 'application/json; charset=utf-8',
    //            success: function (data) {
                    
    //                if (data == true) {
    //                    //alert("Changes request sent successfully");
    //                    //bootbox.alert("Calendar notification No <b>" + notificatinNumber + "</b> Changes request sent to Case Worker successfully");
    //                    bootbox.alert("<b>" + $('#RecordLevel').html() + "</b> has Changes request sent Calendar Event notification No <b>" + notificatinNumber + "</b>  to Case Worker successfully");
    //                    $('#ViewDiv').hide();
    //                    $('#GridListDiv').show();
    //                    $("#remarks").text("");
    //                    GetAdmissionCalendarDetails();
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

function PublishCalendarNotification(id,num) {

    //var pub_table = $("#AdmissionCalendarNtfTable tbody");
    //var noti_num = '';
    //pub_table.find('tr').each(function (len) {
    //    var $tr = $(this);

    //    noti_num = $tr.find("td:eq(3)").text();
    //    if () {

    //    }


    //});

    bootbox.confirm({
        message: "</br> Do you want to publish the calendar of event, Notification No.<b>" + num + "</b> details ?",
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
                url: "/Admission/PublishAdmissionCalNotification",
                dataType: 'json',
                data: { notificationId: id },
                success: function (response) {
                    if (response == "failed") {
                        bootbox.alert("Fail to publish");
                    } else {
                        //bootbox.alert("Calendar notification No <b>" + response + "</b> is Published Successfully");
                        bootbox.alert("</br> <b> Case worker </b> has Published Calendar of Event, Notification No : <b>" + response + "</b> Successfully");
                        GetAdmissionCalendarDetails();
                    }                    
                }, error: function (e) {
                    var _msg = "</br> Something went wrong.";
                    bootbox.alert(_msg);
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
    //            url: "/Admission/PublishAdmissionCalNotification",
    //            dataType: 'json',
    //            data: { notificationId: id },
    //            success: function (response) {
    //                if (response == "failed") {
    //                    bootbox.alert("Fail to publish");
    //                } else {
    //                    //bootbox.alert("Calendar notification No <b>" + response + "</b> is Published Successfully");
    //                    bootbox.alert("<b> Case worker </b> has Published Calendar Event notification No <b>" + response + "</b> Successfully");
    //                    GetAdmissionCalendarDetails();
    //                }                    
    //            }, error: function (e) {
    //                var _msg = "Something went wrong.";
    //                bootbox.alert(_msg);
    //                $("#preloder, .loader").hide();
    //            }
    //        });
    //    }
    //});
   
}


function GetClientReport() {
    var NotifId = $('#notifynum').html();
    NotifId = NotifId.replace(/\//g, '_');
    NotifId = NotifId.replaceAll('-', '_');
    var urlPath = "../../Content/Template/Notification_" + NotifId + ".pdf";
    window.open(urlPath, '_blank');

}


function fnClearAdmsnFields() {
    $('#notifNumber').val('');
    $('#NotifDesc').val('');
    //$('#sessionId').val('');
    $('#AdmissionNotifId').val(0);
    $('#applicantType').val('');

}

function GetAdmissionCalendarNum(id) {
    fnClearAdmsnFields();
    var apptype = $('#applicantType').val();
    var type = $('#courseType').val();
    var year = new Date().getFullYear()
    var acayear = year + "-" + (year + 1);

        $.ajax({
            type: "Get",
            url: "/Admission/GetAdmissionNtfNumber",
            dataType: 'json',
            data: { Id: id },
            success: function (response) {
           
            if (type != '' || type != 0) {
                if (type == 100) {              
                    type = "TRG-2";
                }
                else {                                    
                    type = "TRG-1";
                }             
                var notificationNum = "DITE/TRG/" + type + "/CR2/" + acayear 
                $('#notifNumber').val(notificationNum);   


                if (response != null || response != '') {
                    $("#AdmissionNotifId").empty();
                    $("#AdmissionNotifId").append('<option value="0">Select Notification Number</option>');
                    $.each(response, function () {
                        $("#AdmissionNotifId").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }
            }

            }, error: function (e) {
                var _msg = "Something went wrong.";
                bootbox.alert(_msg);
                $("#preloder, .loader").hide();
            }
        });
}
function GetApplicantTypeAdmission(id) {

    $('#applicantType').val(null);
    //$("#applicantType option[value !='" + response + "']").hide();
    var apptype = $('#applicantType').val();
    var type = $('#courseType').val();
    var year = new Date().getFullYear()
    var acayear = year + "-" + (year + 1);
    var ntfNum = $('#notifNumber').val();  

    $.ajax({
        type: "Get",
        url: "/Admission/GetApplicantTypeAdmission",
        dataType: 'json',
        data: { Id: id },
        success: function (response) {
            
    if (type == 100)
        type = "TRG-2";
    else
        type = "TRG-1";
            if (response != 0) {

            if (response == 1)
                response = "1";
                if (response == 2)
                response = "2";
            $('#applicantType').val(response);
                response = "/" + response
            }
            else {
                response = "";
            }
            var notificationNum = "DITE/TRG/" + type + "/CR2/" + acayear + response;
            $('#notifNumber').val(notificationNum);
            GetApplicantTypechange();
        }, error: function (e) {
            var _msg = "Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });   
}


function GetApplicantTypechange() {
    resetcheckbox();
    var applType = $("#applicantType").val();
    $("#lblDateofnotiffrm").html("Date Of Notification 1 <span style='color: red'>*</span>");
    $("#lbltentativeseatfrm").html("Announce the Tentative Seat matrix data to the public <span style='color: red'>*</span>");
    $(".divfinalseatmtrx").show();
    $("#lblPub_GrdList").html("Publish of gradation list for the 1st Round <span style='color: red'>*</span>");
    $("#lblfstlistseatAllot").html("1st round of online seat allotment <span style='color: red'>*</span>");
    
    $("#lblfstroundadmPro").html("1st round admission process at ITI's <span style='color: red'>*</span>");
    $("#lblfstlistAdmitcan").html("vacancy of seats (Seat Matrix) available for the 2nd round seat allocation <span style='color:red'>*</span>");
    $("#lbl2ndrndEntry").html("2nd round Entry choice of trades / ITI in the Portal is compulsory for remaining Seats for those who have applied & not got the seats in First Round & those" +
               "who have applied & allotted but not availed the admission in the first round <span style = 'color:red'>*</span >");
    $(".hideShowNot2Section").show();
    if (applType == 2) {
        $("#lbl2ndrndEntry").html("Casual round seat allocation at each ITI institutes<span style='color: red'>*</span>");
        $("#lblDateofnotiffrm").html($("#lblDateofnotiffrm").text().replace('1', '2'));
        $("#lbltentativeseatfrm").html("Announce the seat availability data/ Seat matrix data to the public <span style='color: red'>*</span>");
        $(".divfinalseatmtrx").hide();
        $("#lblPub_GrdList").html("Publish of final gradation list <span style='color: red'>*</span>");
        $("#lblfstlistseatAllot").html($("#lblfstlistseatAllot").text().replace('1st', 'Final'));
        $("#lblfstroundadmPro").html($("#lblfstroundadmPro").text().replace('1st round ', '').replace(" at ITI's", ""));
        $("#lblfstlistAdmitcan").html($("#lblfstlistAdmitcan").text().replace('(', '(Final').replace("2nd", "casual"));
        $(".hideShowNot2Section").hide();
    }
}


function ClearFields() {
   
    $('#notifNumber').val('');
    $('#NotifDesc').val('');
    //$('#sessionId').val('');

    //Newly commented
    $('#AdmissionNotifId option:selected').text('Select Notification Number');
    $('#courseType option:selected').text('Select Course Type');
    //End of comment

    //newly added fields
    $('#date_ofnotifFrom').val('');
    $('#date_Tentativefrm').val('');
    $('#date_AppDocVeriffrm').val('');
    $('#date_AppDocVerifto').val('');
    $('#date_tntgrlstapplfrm').val('');
    $('#date_tntgrlstapplto').val('');
    $('#date_ApplgrbyDocvrofcrFrm').val('');
    $('#date_ApplgrbyDocvrofcrTo').val('');
    $('#Pub_GrdList').val('');
    $('#Pub_GrdListrnd2').val('');
    $('#Pub_GrdListrnd3').val('');
    $('#date_fnlseatmtrxfrm').val('');
    //End

    $('#NtfEmailId').val('');
    $('#date_notifApplyFrom').val('');
    $('#date_notifApplyTo').val('');
    $('#date_VerifyFrom').val('');
    $('#date_VerifyTo').val('');
    $('#date_eligible').val('');
    $('#date_dueDate').val('');
    $('#date_tentative').val('');
    $('#date_final').val('');
    $('#date_seatallotment').val('');
    $('#date_admittedCan').val('');
    $('#date_1stRoundFrom').val('');
    $('#date_1stRoundTo').val('');
    $('#date_2ndRoundFrom').val('');
    $('#date_2ndRoundTo').val('');
    $('#date_2ndSeatFrom').val('');
    $('#date_2ndSeatTo').val('');
    $('#date_2ndadmissionFrom').val('');
    $('#date_2ndadmissionTo').val('');
    $('#date_canVacDate').val('');
    $('#date_3rdroundDueFrom').val('');
    $('#date_3rdroundDueTo').val('');
    $('#date_3rdseatDate').val('');
    $('#date_3rdroundAdmisFrom').val('');
    $('#date_3rdroundAdmisTo').val('');
    $('#date_3rdAdminCabVac').val('');
    $('#date_finalTradeFrom').val('');
    $('#date_finalTradeTo').val('');
    $('#date_finalSeatall').val('');
    $('#date_FinalAdminFrom').val('');
    $('#date_FinalAdminTo').val('');
    $('#date_CommTrain').val('');

    

    var selectedText = $("ddlLang").text();
    var content = "";
    if (selectedText == "English") {
        content = "<p style=\"text-align: center; \">Government of Karnataka</p><p>Notification Date: &lt;Notication Date&gt;</p><p style=\"text-align: right;\">Notification Number : &lt;Notication Number&gt;</p><p>&nbsp;</p><p style=\"text-align: center;\">NOTIFICATION</p><br>";
    }
    else {
        content = "<p style=\"text-align: center; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಕರ್ನಾಟಕ ಸರ್ಕಾರ </p><p style=\"font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ದಿನಾಂಕ : &lt;ಅಧಿಸೂಚನೆ ದಿನಾಂಕ&gt;</p><p style=\"text-align: right; font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ ಸಂ.: &lt;ಅಧಿಸೂಚನೆ ಸಂ.&gt;</p><p>&nbsp;</p><p style=\"text-align: center;font-size: 12.0pt; mso-bidi-font-size: 11.0pt; line-height: 107%; font-family: 'Tunga',sans-serif;\">ಅಧಿಸೂಚನೆ</p><br>";
    }
    tinymce.get("content").setContent(content);
}

function resetcheckbox()
{
    if ($('#chkDateapplonlineappfrm').is(':checked'))
        $('#chkDateapplonlineappfrm').trigger('click');

    if ($('#chkDocumentvrf').is(':checked'))
        $('#chkDocumentvrf').trigger('click');

    if ($('#chkDisplayevList').is(':checked'))
        $('#chkDisplayevList').trigger('click');

    if ($('#chkDisplayTentative').is(':checked'))
        $('#chkDisplayTentative').trigger('click');

    if ($('#chkDisplayfinal').is(':checked'))
        $('#chkDisplayfinal').trigger('click');

    if ($('#chkDatabasebackup').is(':checked'))
        $('#chkDatabasebackup').trigger('click');

    if ($('#chkfstlistseatAllot').is(':checked'))
        $('#chkfstlistseatAllot').trigger('click');

    if ($('#chkfstlistAdmitcan').is(':checked'))
        $('#chkfstlistAdmitcan').trigger('click');

    if ($('#chkfstroundadmPro').is(':checked'))
        $('#chkfstroundadmPro').trigger('click');

    if ($('#chk2ndrndEntry').is(':checked'))
        $('#chk2ndrndEntry').trigger('click');

    if ($('#chkDatabasebackup2nd').is(':checked'))
        $('#chkDatabasebackup2nd').trigger('click');

    if ($('#chk2ndroundAdmPro').is(':checked'))
        $('#chk2ndroundAdmPro').trigger('click');

    if ($('#chk3rdroundEntry').is(':checked'))
        $('#chk3rdroundEntry').trigger('click');

    if ($('#chkDatabasebackup3rdRound').is(':checked'))
        $('#chkDatabasebackup3rdRound').trigger('click');

    if ($('#chk3rdRoundAdmPro').is(':checked'))
        $('#chk3rdRoundAdmPro').trigger('click');

    if ($('#chkListcanadm3rdRound').is(':checked'))
        $('#chkListcanadm3rdRound').trigger('click');

    if ($('#chkFinalRoundEntry').is(':checked'))
        $('#chkFinalRoundEntry').trigger('click');

    if ($('#chkFinalseatallot').is(':checked'))
        $('#chkFinalseatallot').trigger('click');

    if ($('#chkAdmFinalRound').is(':checked'))
        $('#chkAdmFinalRound').trigger('click');

    if ($('#chkCommencementTra').is(':checked'))
        $('#chkCommencementTra').trigger('click');

    if ($('#chktentativeseatfrm').is(':checked'))
        $('#chktentativeseatfrm').trigger('click');

    if ($('#chkApplicantvrf').is(':checked'))
        $('#chkApplicantvrf').trigger('click');

    if ($('#chkfinalseatmtrx').is(':checked'))
        $('#chkfinalseatmtrx').trigger('click');

    if ($('#chktntgrdbyapplicant').is(':checked'))
        $('#chktntgrdbyapplicant').trigger('click');

    if ($('#chkApplDocvro').is(':checked'))
        $('#chkApplDocvro').trigger('click');

    if ($('#chkPub_GrdList').is(':checked'))
        $('#chkPub_GrdList').trigger('click');

    if ($('#chkPub_GrdListrnd2').is(':checked'))
        $('#chkPub_GrdListrnd2').trigger('click');

    if ($('#chkPub_GrdListrnd3').is(':checked'))
        $('#chkPub_GrdListrnd3').trigger('click');

   
    //$('#chkDateapplonlineappfrm').prop('checked', false);
    //$('#chkDocumentvrf').prop('checked', false);
    //$('#chkDisplayevList').prop('checked', false);

    //$('#chkDisplayTentative').prop('checked', false);

    //$('#chkDisplayfinal').prop('checked', false);

    //$('#chkDatabasebackup').prop('checked', false);

    //$('#chkfstlistseatAllot').prop('checked', false);

    //$('#chkfstlistAdmitcan').prop('checked', false);

    //$('#chkfstroundadmPro').prop('checked', false);

    //$('#chk2ndrndEntry').prop('checked', false);

    //$('#chkDatabasebackup2nd').prop('checked', false);

    //$('#chk2ndroundAdmPro').prop('checked', false);

    //$('#chk2ndelistofadmitCan').prop('checked', false);

    //$('#chk3rdroundEntry').prop('checked', false);

    //$('#chkDatabasebackup3rdRound').prop('checked', false);

    //$('#chk3rdRoundAdmPro').prop('checked', false);

    //$('#chkListcanadm3rdRound').prop('checked', false);

    //$('#chkFinalRoundEntry').prop('checked', false);

    //$('#chkFinalseatallot').prop('checked', false);

    //$('#chkAdmFinalRound').prop('checked', false);

    //$('#chkCommencementTra').prop('checked', false);

    if ($('#chkDateofnotiffrm').is(':checked')) 
        $('#chkDateofnotiffrm').trigger('click');
    //$('#chkDateofnotiffrm').prop('checked', false);

    //$('#chktentativeseatfrm').prop('checked', false);

    //$('#chkApplicantvrf').prop('checked', false);

    //$('#chkfinalseatmtrx').prop('checked', false);

    //$('#chktntgrdbyapplicant').prop('checked', false);

    //$('#chkApplDocvro').prop('checked', false);

    //$('#chkPub_GrdList').prop('checked', false);

    //$('#chkPub_GrdListrnd2').prop('checked', false);

    //$('#chkPub_GrdListrnd3').prop('checked', false);
    
    
}

function disablesendToRoles(user, user1, btn, apprbtn) {

    if ($('#' + user).val() == 0) {
        $('#' + user1).removeAttr('disabled');
        $('#' + btn).removeAttr('disabled');
        $('#' + apprbtn).removeAttr('disabled');

    }
    else {
        $('#' + user1).attr('disabled', 'disabled');
        $('#' + btn).attr('disabled', 'disabled');
        $('#' + apprbtn).attr('disabled', 'disabled');
        $('#' + apprbtn).attr('disabled', 'disabled');
    }
}



