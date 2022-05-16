$(document).ready(function () {
    GetViewSeatAvailability();
    $('.nav-tabs li:eq(0) a').tab('show');

    $("#ApplicantTypeUpdateErr").hide();
    $("#AcademicyearUpdateErr").hide();
    $("#RoundUpdateErr").hide();


    $("#divisionviewcheckErr").hide();
    $("#talukaviewcheckErr").hide();
    $("#divuserSubmit").hide();


    $("#tblCheckSummary").hide();
    $("#AcademicyearUpdateErr").hide();
    $("#AcademicyearReviewErr").hide();
    $("#AcademicyearApproveErr").hide();
    $("#tblWebgridviewSeatM").hide();

    $('#Collegepartialview').hide();//hiding the update seatmatrix
    $('#reviewGrid').hide();//hiding the review seatmatrix

    GetInstituteType();
    Getusers2();
    GetStatus();
    Getusers('Reviewusers', 1);
    GetStatus('UpStatus');
    GetStatus('ReviewStatus');
    GetStatus('PubStatus');


    //filter start
    GetCourses("CourseType2");
    GetCourses("courseType");
    GetSessionYear("Session2");
    GetDivisionsDDp("divisionDdp");
    GetFilterData("districtDdp");
    GetApplicantType("ddlApplicantType");
    GetApplicantType("ddlApplicantTypeGen");
    GetApplicantType("ddlApplicantTypecheck");
    GetApplicantType("ddlApplicantTypeView");
    GetSessionYear("ddlAcademicYear");
    GetSessionYear("ddlAcademicYearGen");
    GetSessionYear("ddlAcademicYearcheckSum");
    GetSessionYear("ddlAcademicYearUpdate");
    GetSessionYear("ddlAcademicYearReview");
    GetSessionYear("ddlAcademicYearApprove");
    GetSessionYear("ddlAcademicYearView");
    GetDivisionsDDp("ddlDivision");
    GetDivisionsDDp("ddlDivisionView");
    //filter end

    $('#seatGenerate').hide();
    var table;
    var table1;
});
function GetFilterData(ddpId) {
    $("#" + ddpId).empty();
    $("#" + ddpId).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetRegionDistrictCities",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + ddpId).append($("<option/>").val(this.CityId).text(this.CityName));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetInstituteType() {
    $("#ddlInstitute").empty();
    $("#ddlInstitute").append('<option value="0">choose</option>');
    $.ajax({
        url: "/Admission/GetInstituteType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlInstitute").append($("<option/>").val(this.Institute_typeId).text(this.InstituteType));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDistrict() {
    var Divisions = $('#ddlDivision :selected').val();
    if (Divisions != "" && Divisions != null) {
        $("#ddlDistrict").empty();
        $("#ddlDistrict").append('<option value="choose">choose</option>');
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
        $("#ddlDistrict").append('<option value="choose">choose</option>');
    }
}

function GetTaluk() {

    var DistId = $('#ddlDistrict :selected').val();
    if (DistId != "" && DistId != null) {
        $("#ddlTaluka").empty();
        $("#ddlTaluka").append('<option value="choose">choose</option>');
        $.ajax({
            type: 'Get',
            url: "/Admission/GetTaluk",
            data: { DistId: DistId },
            success: function (data) {
                $.each(data, function () {
                    $("#ddlTaluka").append($("<option/>").val(this.Value).text(this.Text));
                });

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#ddlTaluka").empty();
        $("#ddlTaluka").append('<option value="choose">choose</option>');
    }
}

//View Dropdowns
function GetDistrictView() {
    var Divisions = $('#ddlDivisionView :selected').val();
    if (Divisions != "" && Divisions != "Select") {
        $("#ddlDistrictView").empty();
        $("#ddlDistrictView").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: "/Admission/GetDistrict",
            data: { Divisions: Divisions },
            success: function (data) {
                $.each(data, function () {
                    $("#ddlDistrictView").append($("<option/>").val(this.district_id).text(this.district_ename));
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#ddlDistrictView").empty();
        $("#ddlDistrictView").append('<option value="">Select</option>');
    }
}

function GetTalukView() {
    var DistId = $('#ddlDistrictView :selected').val();
    if (DistId != "" && DistId != null) {
        $("#ddlTalukaView").empty();
        $("#ddlTalukaView").append('<option value="0">Select</option>');
        $.ajax({
            type: 'Get',
            url: "/Admission/GetTaluk",
            data: { DistId: DistId },
            success: function (data) {
                $.each(data, function () {
                    $("#ddlTalukaView").append($("<option/>").val(this.Value).text(this.Text));
                });

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#ddlTalukaView").empty();
        $("#ddlTalukaView").append('<option value="">Select</option>');
    }
}

function Getusers2() {
    $("#Upusers").empty();
    $("#Upusers").append('<option value="choose">choose</option>');
    $("#Reusers").empty();
    $("#Reusers").append('<option value="choose">choose</option>');
    $("#Publishusers").empty();
    $("#Publishusers").append('<option value="choose">choose</option>');
    $("#users").empty();
    $("#users").append('<option value="choose">choose</option>');

    $.ajax({
        url: "/Admission/GetRolesCal",
        type: 'Get',
        data: { Level: 1 },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Upusers").append($("<option/>").val(this.RoleID).text(this.RoleName));
                    $("#Reusers").append($("<option/>").val(this.RoleID).text(this.RoleName));
                    $("#Publishusers").append($("<option/>").val(this.RoleID).text(this.RoleName));
                    $("#users").append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetviewSeatmatrix() {
    var ApplicantTypeId = $('#ddlApplicantType :selected').val();
    var AcademicYear = $('#ddlAcademicYear :selected').val();
    var Institute = $('#ddlInstitute :selected').val();

    $.ajax({
        type: "GET",
        //url: "/Admission/GetviewSeatmatrix",
        data: { ApplicantTypeId: ApplicantTypeId, AcademicYear: AcademicYear, Institute: Institute },
        contentType: "application/json",
        success: function (data) {
            //
            $('#tblviewseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'ApplicantNumber', 'title': 'Application Number', 'className': 'text-center ApplicantNumber' },
                    { 'data': 'ApplicantName', 'title': 'ApplicantName', 'className': 'text-left ApplicantName' },
                    { 'data': 'FathersName', 'title': 'Father Name', 'className': 'text-left FathersName' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-left Gender' },
                    {
                        'data': 'DOB', 'title': 'DOB', 'className': 'text-left DOB',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var date = daterangeformate2(oData.DOB);
                            var hide = "<input type='hidden' class='ApplicationId' value='" + oData.ApplicationId + "' />"
                            $(nTd).html(date + hide);

                        }
                    },
                    { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left Category' },
                    { 'data': 'MaxMarks', 'title': 'Max Marks in 10th', 'className': 'text-left MaxMarks' },
                    { 'data': 'MarksObtained', 'title': 'Obtained Marks in 10th', 'className': 'text-left MarksObtained' },
                    { 'data': 'Result', 'title': '10thPass/Fail', 'className': 'text-left Result' },
                    { 'data': 'locationName', 'title': 'Rural/Urban', 'className': 'text-left locationName' },
                    { 'data': 'Rank', 'title': 'Rank', 'className': 'text-left Rank' },
                ],

            });
        }
    });
}

//AD Usecase-31
function ReviewSeatMatrixId(SeatMaxId) {

    var SeatMaxId = SeatMaxId;
    $.ajax({
        type: "GET",
        url: "/Admission/GetReviewTradeSeatMatrixId/" + SeatMaxId,
        data: { SeatMaxId: SeatMaxId },
        contentType: "application/json",
        success: function (data) {

            $('#tblReviewTradeseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center ApplicantTypeDdl' },
                    { 'data': 'iti_college_name', 'title': 'Institute Name', 'className': 'text-left Round' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left StatusName' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left StatusName' },
                    { 'data': 'taluk_ename', 'title': 'Taluk Name', 'className': 'text-left StatusName' },
                    { 'data': 'iti_college_id', 'title': 'Institute Id', 'className': 'text-left StatusName' },

                    {
                        'data': 'iti_college_id',
                        'title': 'Seat allocation view',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type='button' onclick='ShowViewTreadSeatMatrix(" + oData.iti_college_id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ],
            });
        }
    });
}

//Tab-5 check summary seat matrix
function btnSeatmatrixcheckSummary() {
    $("#ApplicantTypecheckErr").text('');
    $("#AcademicyearcheckSumErr").text('');
    $("#RoundcheckErr").text('');
    $("#divisionviewcheckSumErr").text('');
    $("#districtviewcheckErr").text('');
    $("#talukaviewcheckSumErr").text('');
    $("#InstitutecheckErr").text('');

    var ApplicantTypeId = $('#ddlApplicantTypecheck :selected').val();
    var AcademicYearId = $('#ddlAcademicYearcheckSum :selected').val();
    var Round = $('#ddlRoundcheck :selected').val();
    var DivisionId = $('#ddlDivision :selected').val();
    var DistrictId = $('#ddlDistrict :selected').val();
    var TalukId = $('#ddlTaluka :selected').val();
    var InstituteId = $('#ddlInstitutecheck :selected').val();

    if (ApplicantTypeId == 'choose' && AcademicYearId == 'choose' && Round == 'choose' && DivisionId == 'choose' && DistrictId == 'choose' && TalukId == 'choose' && InstituteId == 'choose') {
        $("#ApplicantTypecheckErr").text('select the applicant type');
        $("#AcademicyearcheckSumErr").text('select the academic year');
        $("#RoundcheckErr").text('select the round');
        $("#divisionviewcheckSumErr").text('select the division');
        $("#districtviewcheckErr").text('select the district');
        $("#talukaviewcheckSumErr").text('select the taluku');
        $("#InstitutecheckErr").text('select the institute');

    }
    else if (ApplicantTypeId == 'choose') {
        $("#ApplicantTypecheckErr").text('select the applicant type');
    }
    else if (AcademicYearId == 'choose') {
        $("#AcademicyearcheckSumErr").text('select the academic year');
    }
    else if (Round == 0) {
        $("#RoundcheckErr").text('select the round');
    }
    else if (DivisionId == 'choose') {
        $("#divisionviewcheckSumErr").text('select the division');
    }
    else if (DistrictId == 'choose') {
        $("#districtviewcheckErr").text('select the district');
    }
    else if (TalukId == 'choose') {
        $("#talukaviewcheckSumErr").text('select the taluku');
    }
    else if (InstituteId == 'choose') {
        $("#InstitutecheckErr").text('select the institute');
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetCheckSummary",
            data: { AcademicYearId: AcademicYearId, ApplicantTypeId: ApplicantTypeId, InstituteId: InstituteId, Round: Round, DistrictId: DistrictId, DivisionId: DivisionId, TalukId: TalukId },
            contentType: "application/json",
            success: function (data) {
                if (data != null || data != '' || data != 0) {
                    $("#tblCheckSummary").show();
                    $.each(data, function () {
                        //if (this.Vertical_Rules == "GM") {
                        $("#checkGeneralPoolGM").text(this.GMGPH);
                        $("#checkWomenGM").text(this.GMWH);
                        $("#checkPhysicallyHandiGM").text(this.GMPDH);
                        $("#checkExServiceGM").text(this.GMEXSH);
                        $("#checkKanMediumGM").text(this.GMKMH);
                        $("#checkEwsGM").text(this.GMEWSH);
                        //}
                        //else if (this.Vertical_Rules == "SC") {
                        $("#checkGeneralPoolSC").text(this.SCGPH);
                        $("#checkWomenSC").text(this.SCWH);
                        $("#checkPhysicallyHandiSC").text(this.SCPDH);
                        $("#checkExServiceSC").text(this.SCEXSH);
                        $("#checkKanMediumSC").text(this.SCKMH);
                        $("#checkEwsSC").text(this.SCEWSH);
                        //}
                        //else if (this.Vertical_Rules == "ST") {
                        $("#checkGeneralPoolST").text(this.STGPH);
                        $("#checkWomenST").text(this.STWH);
                        $("#checkPhysicallyHandiST").text(this.STPDH);
                        $("#checkExServiceST").text(this.STEXSH);
                        $("#checkKanMediumST").text(this.STKMH);
                        $("#checkEwsST").text(this.STEWSH);
                        //}
                        //else if (this.Vertical_Rules == "C1") {
                        $("#checkGeneralPoolC1").text(this.C1GPH);
                        $("#checkWomenC1").text(this.C1WH);
                        $("#checkPhysicallyHandiC1").text(this.C1PDH);
                        $("#checkExServiceC1").text(this.C1EXSH);
                        $("#checkKanMediumC1").text(this.C1KMH);
                        $("#checkEwsC1").text(this.C1EWSH);
                        //}
                        //else if (this.Vertical_Rules == "IIA") {
                        $("#checkGeneralPool2A").text(this.TWOAGPH);
                        $("#checkWomen2A").text(this.TWOAWH);
                        $("#checkPhysicallyHandi2A").text(this.TWOAPDH);
                        $("#checkExService2A").text(this.TWOAEXSH);
                        $("#checkKanMedium2A").text(this.TWOAKMH);
                        $("#checkEws2A").text(this.TWOAEWSH);
                        //}
                        //else if (this.Vertical_Rules == "IIB") {
                        $("#checkGeneralPool2B").text(this.TWOBGPH);
                        $("#checkWomen2B").text(this.TWOBWH);
                        $("#checkPhysicallyHandi2B").text(this.TWOBPDH);
                        $("#checkExService2B").text(this.TWOBEXSH);
                        $("#checkKanMedium2B").text(this.TWOBKMH);
                        $("#checkEws2B").text(this.TWOBEWSH);
                        //}
                        //else if (this.Vertical_Rules == "IIIA") {
                        $("#checkGeneralPool3A").text(this.THREEAGPH);
                        $("#checkWomen3A").text(this.THREEAWH);
                        $("#checkPhysicallyHandi3A").text(this.THREEAPDH);
                        $("#checkExService3A").text(this.THREEAEXSH);
                        $("#checkKanMedium3A").text(this.THREEAKMH);
                        $("#checkEws3A").text(this.THREEAEWSH);
                        //}
                        //else if (this.Vertical_Rules == "IIIB") {
                        $("#checkGeneralPool3B").text(this.THREEBGPH);
                        $("#checkWomen3B").text(this.THREEBWH);
                        $("#checkPhysicallyHandi3B").text(this.THREEBPDH);
                        $("#checkExService3B").text(this.THREEBEXSH);
                        $("#checkKanMedium3B").text(this.THREEBKMH);
                        $("#checkEws3B").text(this.THREEBEWSH);
                        //}
                    });
                    if (data.length == 0) {
                        bootbox.alert('Data Not Available');
                        $("#tblCheckSummary").hide();
                    }
                }
            }
        });
    }
}

//Tab-8-Approve & Publish Seat Matrix
function ApproveSeatMatrix() {
    var AcademicYearId = $('#ddlAcademicYearApprove :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetApproveSeatMatrix",
        data: { AcademicYearId: AcademicYearId },
        contentType: "application/json",
        success: function (data) {
            $('#tblApproveseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'ApplicantTypeDdl', 'title': 'Applicant Type', 'className': 'text-center ApplicantTypeDdl' },
                    { 'data': 'Round', 'title': 'Round', 'className': 'text-left Round' },
                    { 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left StatusName' },
                    {
                        'data': 'SeatMaxId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type='button' data-toggle='modal' data-target='#PublishListModel' onclick='ApproveSeatMatrixId(" + oData.SeatMaxId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ],
            });
        }
    });
}

function ApproveSeatMatrixId(SeatMaxId) {

    var SeatMaxId = SeatMaxId;
    $.ajax({
        type: "GET",
        url: "/Admission/GetAproveTradeSeatMatrixId/" + SeatMaxId,
        data: { SeatMaxId: SeatMaxId },
        contentType: "application/json",
        success: function (data) {

            $('#tblPublishTradeseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center ApplicantTypeDdl' },
                    { 'data': 'iti_college_name', 'title': 'Institute Name', 'className': 'text-left Round' },
                    { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left StatusName' },
                    { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left StatusName' },
                    { 'data': 'taluk_ename', 'title': 'Taluk Name', 'className': 'text-left StatusName' },
                    { 'data': 'iti_college_id', 'title': 'Institute Id', 'className': 'text-left StatusName' },

                    {
                        'data': 'iti_college_id',
                        'title': 'Seat allocation view',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type='button' onclick='ShowViewTreadSeatMatrix(" + oData.iti_college_id + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ],
            });
        }
    });
}

//Tab-6 View SeatMatrix
function btnviewSeatmatrix() {
    $("#ApplicantTypeViewErr").text('');
    $("#AcademicyearViewErr").text('');
    $("#RoundViewErr").text('');
    //$('#divisionviewViewErr').text('');
    //$('#spandistrictViewErr').text('');
    //$('#spantalukaViewErr').text('');
    //$('#InstituteViewErr').text('');

    var ApplicantTypeId = $('#ddlApplicantTypeView :selected').val();
    var AcademicYearId = $('#ddlAcademicYearView :selected').val();
    var Round = $('#ddlRoundView :selected').val();
    //var InstituteId = $('#ddlInstituteView :selected').val();
    //var DistrictId = $('#ddlDistrictView :selected').val();
    //var DivisionId = $('#ddlDivisionView :selected').val();
    //var TalukId = $('#ddlTalukaView :selected').val();
    if (ApplicantTypeId == 'choose' && AcademicYearId == 'choose' && Round == 'choose' /*&& DivisionId == 'choose' && DistrictId == 'choose' && TalukId == 'choose' && InstituteId == 'choose'*/) {
        $("#ApplicantTypeViewErr").text('select the applicant type');
        $("#AcademicyearViewErr").text('select the year');
        $("#RoundViewErr").text('select the round');
        //$('#divisionviewViewErr').text('select the division');
        //$('#spandistrictViewErr').text('select the district');
        //$('#spantalukaViewErr').text('select the taluku');
        //$('#InstituteViewErr').text('select the institute');
    }
    else if (ApplicantTypeId == 'choose') {
        $("#ApplicantTypeViewErr").text('select the applicant type');
    }
    else if (AcademicYearId == 'choose') {
        $("#AcademicyearViewErr").text('select the year');
    }
    else if (Round == 0) {
        $("#RoundViewErr").text('select the round');
    }
    //else if (DivisionId == 'choose') {
    //    $('#divisionviewViewErr').text('select the division');
    //}
    //else if (DistrictId == 'choose') {
    //    $('#spandistrictViewErr').text('select the district');
    //}
    //else if (TalukId == 'choose') {
    //    $('#spantalukaViewErr').text('select the taluku');
    //}
    //else if (InstituteId == 'choose') {
    //    $('#InstituteViewErr').text('select the institute');
    //}
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetViewSeatMatrixGrid",
            //data: { ApplicantTypeId: ApplicantTypeId, AcademicYearId: AcademicYearId, Round: Round, InstituteId: InstituteId, DistrictId: DistrictId, DivisionId: DivisionId, TalukId: TalukId },
            data: { ApplicantTypeId: ApplicantTypeId, AcademicYearId: AcademicYearId, Round: Round, InstituteId: 0, DistrictId: 0, DivisionId: 0, TalukId: 0 },
            contentType: "application/json",
            success: function (data) {
                $('#tblWebgridviewSeatM').show();
                table1=$('#tblWebgridviewSeatM').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                        { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left StatusName' },
                        { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left StatusName' },
                        { 'data': 'taluk_ename', 'title': 'Taluk Name', 'className': 'text-left taluk' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center ApplicantTypeDdl' },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left StatusName' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left StatusName' },
                        { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left Round' },
                        {
                            'data': 'SeatMaxId',
                            'title': 'Seat allocation view',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                $(nTd).html("<input type='button' onclick='ShowViewTreadSeatMatrix(" + oData.Round + "," + oData.InstituteId + "," + oData.AcademicYear + "," + oData.TradeId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                            }
                        }
                    ],
                });
            }
        });
    }
}
function ViewITISeatMatrix() {
    $("#ApplicantTypeViewErr").text('');
    $("#AcademicyearViewErr").text('');
    $("#RoundViewErr").text('');
    var ApplicantTypeId = $('#ddlApplicantTypeView :selected').val();
    var AcademicYearId = $('#ddlAcademicYearView :selected').val();
    var Round = $('#ddlRoundView :selected').val();
    if (ApplicantTypeId == 'choose' && AcademicYearId == 'choose' && Round == 'choose') {
        $("#ApplicantTypeViewErr").text('select the applicant type');
        $("#AcademicyearViewErr").text('select the year');
        $("#RoundViewErr").text('select the round');
    }
    else if (ApplicantTypeId == 'choose') {
        $("#ApplicantTypeViewErr").text('select the applicant type');
    }
    else if (AcademicYearId == 'choose') {
        $("#AcademicyearViewErr").text('select the year');
    }
    else if (Round == 0) {
        $("#RoundViewErr").text('select the round');
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetViewSeatMatrixData",
            data: { 'ApplicantTypeId': ApplicantTypeId, 'AcademicYearId': AcademicYearId, 'Round': Round },
            contentType: "application/json",
            success: function (data) {
                $('#tblWebgridviewSeatM').show();
                $('#tblWebgridviewSeatM').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                        { 'data': 'division_name', 'title': 'Division Name', 'className': 'text-left StatusName' },
                        { 'data': 'district_ename', 'title': 'District Name', 'className': 'text-left StatusName' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center ApplicantTypeDdl' },
                        { 'data': 'InstituteType', 'title': 'Institute Type', 'className': 'text-left StatusName' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left StatusName' },
                        { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left Round' },
                        {
                            'data': 'SeatMaxId',
                            'title': 'Seat allocation view',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                                $(nTd).html("<input type='button' onclick='ShowViewTreadSeatMatrix(" + oData.Round + "," + oData.InstituteId + "," + oData.AcademicYear + "," + oData.TradeId + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                            }
                        }
                    ],
                });
            }
        });
    }
}
function ShowViewTreadSeatMatrix(round, instiId, year, tradeId) {
    $.ajax({
        type: "GET",
        url: "/Admission/AdmissionSeatMatrixNested",
        contentType: "application/json",
        data: { 'round': round, 'instiId': instiId, 'year': year, 'tradeId': tradeId },
        success: function (data) {
            if (data != null) {
                $('#ViewListModel').modal('show');
                if (round == 1 || round == 2) {
                    $('#tblViewListModel').DataTable({
                        data: data,
                        "destroy": true,
                        "bSort": true,
                        "paging": false,
                        "ordering": false,
                        "info": false,
                        bFilter: false,
                        columns: [
                            { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                            { 'data': 'Units', 'title': 'Number of Units', 'className': 'text-left' },
                            { 'data': 'Seats', 'title': 'Number of Seats', 'className': 'text-left' },
                            { 'data': 'GMWH', 'title': 'GMW-H', 'className': 'text - left' },
                            { 'data': 'GMWNH', 'title': 'GMW-NH', 'className': 'text - left' },
                            { 'data': 'GMPDH', 'title': 'GMPD-H', 'className': 'text - left' },
                            { 'data': 'GMPDNH', 'title': 'GMPD-NH', 'className': 'text - left' },
                            { 'data': 'GMEXSH', 'title': 'GMEXS-H', 'className': 'text - left' },
                            { 'data': 'GMEXSNH', 'title': 'GMEXS-NH', 'className': 'text - left' },
                            { 'data': 'GMKMH', 'title': 'GMKM-H', 'className': 'text - left' },
                            { 'data': 'GMKMNH', 'title': 'GMKM-NH', 'className': 'text - left' },
                            { 'data': 'GMEWSH', 'title': 'GMEWS-H', 'className': 'text - left' },
                            { 'data': 'GMEWSNH', 'title': 'GMEWS-NH', 'className': 'text - left' },
                            { 'data': 'GMGPH', 'title': 'GMGP-H', 'className': 'text - left' },
                            { 'data': 'GMGPNH', 'title': 'GMGP-NH', 'className': 'text - left' },
                            { 'data': 'SCWH', 'title': 'SCW-H', 'className': 'text - left' },
                            { 'data': 'SCWNH', 'title': 'SCW-NH', 'className': 'text - left' },
                            { 'data': 'SCPDH', 'title': 'SCPD-H', 'className': 'text - left' },
                            { 'data': 'SCPDNH', 'title': 'SCPD-NH', 'className': 'text - left' },
                            { 'data': 'SCEXSH', 'title': 'SCEXS-H', 'className': 'text - left' },
                            { 'data': 'SCEXSNH', 'title': 'SCEXS-NH', 'className': 'text - left' },
                            { 'data': 'SCKMH', 'title': 'SCKM-H', 'className': 'text - left' },
                            { 'data': 'SCKMNH', 'title': 'SCKM-NH', 'className': 'text - left' },
                            { 'data': 'SCEWSH', 'title': 'SCEWS-H', 'className': 'text - left' },
                            { 'data': 'SCEWSNH', 'title': 'SCEWS-NH', 'className': 'text - left' },
                            { 'data': 'SCGPH', 'title': 'SCGP-H', 'className': 'text - left' },
                            { 'data': 'SCGPNH', 'title': 'SCGP-NH', 'className': 'text - left' },
                            { 'data': 'STWH', 'title': 'STW-H', 'className': 'text - left' },
                            { 'data': 'STWNH', 'title': 'STW-NH', 'className': 'text - left' },
                            { 'data': 'STPDH', 'title': 'STPD-H', 'className': 'text - left' },
                            { 'data': 'STPDNH', 'title': 'STPD-NH', 'className': 'text - left' },
                            { 'data': 'STEXSH', 'title': 'STEXS-H', 'className': 'text - left' },
                            { 'data': 'STEXSNH', 'title': 'STEXS-NH', 'className': 'text - left' },
                            { 'data': 'STKMH', 'title': 'STKM-H', 'className': 'text - left' },
                            { 'data': 'STKMNH', 'title': 'STKM-NH', 'className': 'text - left' },
                            { 'data': 'STEWSH', 'title': 'STEWS-H', 'className': 'text - left' },
                            { 'data': 'STEWSNH', 'title': 'STEWS-NH', 'className': 'text - left' },
                            { 'data': 'STGPH', 'title': 'STGP-H', 'className': 'text - left' },
                            { 'data': 'STGPNH', 'title': 'STGP-NH', 'className': 'text - left' },
                            { 'data': 'C1WH', 'title': 'C1W-H', 'className': 'text - left' },
                            { 'data': 'C1WNH', 'title': 'C1W-NH', 'className': 'text - left' },
                            { 'data': 'C1PDH', 'title': 'C1PD-H', 'className': 'text - left' },
                            { 'data': 'C1PDNH', 'title': 'C1PD-NH', 'className': 'text - left' },
                            { 'data': 'C1EXSH', 'title': 'C1EXS-H', 'className': 'text - left' },
                            { 'data': 'C1EXSNH', 'title': 'C1EXS-NH', 'className': 'text - left' },
                            { 'data': 'C1KMH', 'title': 'C1KM-H', 'className': 'text - left' },
                            { 'data': 'C1KMNH', 'title': 'C1KM-NH', 'className': 'text - left' },
                            { 'data': 'C1EWSH', 'title': 'C1EWS-H', 'className': 'text - left' },
                            { 'data': 'C1EWSNH', 'title': 'C1EWS-NH', 'className': 'text - left' },
                            { 'data': 'C1GPH', 'title': 'C1GP-H', 'className': 'text - left' },
                            { 'data': 'C1GPNH', 'title': 'C1GP-NH', 'className': 'text - left' },
                            { 'data': 'TWOAWH', 'title': 'TWOAW-H', 'className': 'text - left' },
                            { 'data': 'TWOAWNH', 'title': 'TWOAW-NH', 'className': 'text - left' },
                            { 'data': 'TWOAPDH', 'title': 'TWOAPD-H', 'className': 'text - left' },
                            { 'data': 'TWOAPDNH', 'title': 'TWOAPD-NH', 'className': 'text - left' },
                            { 'data': 'TWOAEXSH', 'title': 'TWOAEXS-H', 'className': 'text - left' },
                            { 'data': 'TWOAEXSNH', 'title': 'TWOAEXS-NH', 'className': 'text - left' },
                            { 'data': 'TWOAKMH', 'title': 'TWOAKM-H', 'className': 'text - left' },
                            { 'data': 'TWOAKMNH', 'title': 'TWOAKM-NH', 'className': 'text - left' },
                            { 'data': 'TWOAEWSH', 'title': 'TWOAEWS-H', 'className': 'text - left' },
                            { 'data': 'TWOAEWSNH', 'title': 'TWOAEWS-NH', 'className': 'text - left' },
                            { 'data': 'TWOAGPH', 'title': 'TWOAGP-H', 'className': 'text - left' },
                            { 'data': 'TWOAGPNH', 'title': 'TWOAGP-NH', 'className': 'text - left' },
                            { 'data': 'TWOBWH', 'title': 'TWOBW-H', 'className': 'text - left' },
                            { 'data': 'TWOBWNH', 'title': 'TWOBW-NH', 'className': 'text - left' },
                            { 'data': 'TWOBPDH', 'title': 'TWOBPD-H', 'className': 'text - left' },
                            { 'data': 'TWOBPDNH', 'title': 'TWOBPD-NH', 'className': 'text - left' },
                            { 'data': 'TWOBEXSH', 'title': 'TWOBEXS-H', 'className': 'text - left' },
                            { 'data': 'TWOBEXSNH', 'title': 'TWOBEXS-NH', 'className': 'text - left' },
                            { 'data': 'TWOBKMH', 'title': 'TWOBKM-H', 'className': 'text - left' },
                            { 'data': 'TWOBKMNH', 'title': 'TWOBKM-NH', 'className': 'text - left' },
                            { 'data': 'TWOBEWSH', 'title': 'TWOBEWS-H', 'className': 'text - left' },
                            { 'data': 'TWOBEWSNH', 'title': 'TWOBEWS-NH', 'className': 'text - left' },
                            { 'data': 'TWOBGPH', 'title': 'TWOBGP-H', 'className': 'text - left' },
                            { 'data': 'TWOBGPNH', 'title': 'TWOBGP-NH', 'className': 'text - left' },
                            { 'data': 'THREEAWH', 'title': 'THREEAW-H', 'className': 'text - left' },
                            { 'data': 'THREEAWNH', 'title': 'THREEAW-NH', 'className': 'text - left' },
                            { 'data': 'THREEAPDH', 'title': 'THREEAPD-H', 'className': 'text - left' },
                            { 'data': 'THREEAPDNH', 'title': 'THREEAPD-NH', 'className': 'text - left' },
                            { 'data': 'THREEAEXSH', 'title': 'THREEAEXS-H', 'className': 'text - left' },
                            { 'data': 'THREEAEXSNH', 'title': 'THREEAEXS-NH', 'className': 'text - left' },
                            { 'data': 'THREEAKMH', 'title': 'THREEAKM-H', 'className': 'text - left' },
                            { 'data': 'THREEAKMNH', 'title': 'THREEAKM-NH', 'className': 'text - left' },
                            { 'data': 'THREEAEWSH', 'title': 'THREEAEWS-H', 'className': 'text - left' },
                            { 'data': 'THREEAEWSNH', 'title': 'THREEAEWS-NH', 'className': 'text - left' },
                            { 'data': 'THREEAGPH', 'title': 'THREEAGP-H', 'className': 'text - left' },
                            { 'data': 'THREEAGPNH', 'title': 'THREEAGP-NH', 'className': 'text - left' },
                            { 'data': 'THREEBWH', 'title': 'THREEBW-H', 'className': 'text - left' },
                            { 'data': 'THREEBWNH', 'title': 'THREEBW-NH', 'className': 'text - left' },
                            { 'data': 'THREEBPDH', 'title': 'THREEBPD-H', 'className': 'text - left' },
                            { 'data': 'THREEBPDNH', 'title': 'THREEBPD-NH', 'className': 'text - left' },
                            { 'data': 'THREEBEXSH', 'title': 'THREEBEXS-H', 'className': 'text - left' },
                            { 'data': 'THREEBEXSNH', 'title': 'THREEEXS-NH', 'className': 'text - left' },
                            { 'data': 'THREEBKMH', 'title': 'THREEBKM-H', 'className': 'text - left' },
                            { 'data': 'THREEBKMNH', 'title': 'THREEBKM-NH', 'className': 'text - left' },
                            { 'data': 'THREEBEWSH', 'title': 'THREEBEWS-H', 'className': 'text - left' },
                            { 'data': 'THREEBEWSNH', 'title': 'THREEBEWS-NH', 'className': 'text - left' },
                            { 'data': 'THREEBGPH', 'title': 'THREEBGP-H', 'className': 'text - left' },
                            { 'data': 'THREEBGPNH', 'title': 'THREEBGP-NH', 'className': 'text - left' },

                        ]
                    });
                }
                else
                    if (round == 6) {
                        $('#tblViewListModel').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'TradeName', 'title': 'Sl.no', 'className': 'text-center' },
                                { 'data': 'GMGP', 'title': 'GM-GP', 'className': 'text-left' }
                            ]
                        });
                    }
                    else {

                        $('#tblViewListModel').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-center' },
                                { 'data': 'GMGP', 'title': 'GM-GP', 'className': 'text-left' },
                                { 'data': 'SCGP', 'title': 'SC-GP', 'className': 'text-left' },
                                { 'data': 'STGP', 'title': 'ST-GP', 'className': 'text-left' },
                                { 'data': 'C1GP', 'title': 'C1-GP', 'className': 'text-left' },
                                { 'data': 'TWOAGP', 'title': 'TWOA-GP', 'className': 'text-center' },
                                { 'data': 'TWOBGP', 'title': 'TWOB-GP', 'className': 'text-left' },
                                { 'data': 'THREEAGP', 'title': 'THREEA-GP', 'className': 'text-left' },
                                { 'data': 'THREEBGP', 'title': 'THREEB-GP', 'className': 'text-left' }
                            ]
                        });
                    }
            }
            else {
                bootbox.alert('No data available');
            }
        }, error: function (e) {
            var _msg = "Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });
}

function GetDistrictData(dividdp, distddp) {
    var divi = $("#" + dividdp).val();
    $('#' + distddp).empty();
    $('#' + distddp).append('<option value="choose">choose</option>');
    if (divi == 'choose') {
        console.log('select division');
    }
    else {
        $.ajax({
            url: "/Admission/GetDistrict",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { "Divisions": divi },
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + distddp).append($("<option/>").val(this.district_id).text(this.district_ename));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}
function GetTaluksData(ddpId, tlk, cityId) {
    var lgdCode = $("#" + ddpId).val();
    $("#" + tlk).empty();
    $("#" + tlk).append('<option value="choose">choose</option>');
    if (lgdCode != 'choose') {
        $("#instituteDdp").empty();
        if (tlk != "instituteDdp") {
            //$("#talukDdp").append('<option value="choose">choose</option>');
            $("#instituteDdp").append('<option value="choose">choose</option>');
        }
        //else
        //    $("#" + tlk).append('<option value="choose">choose</option>');

        var Url = "/Admission/GetCitiTaluks";
        if (cityId == 1) {
            Url = "/Admission/GetCitiTaluks";
        }
        else {
            Url = "/Admission/GetInstitutes";
        }
        $.ajax({
            url: Url,
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { "distilgdCOde": lgdCode },
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + tlk).append($("<option/>").val(this.CityId).text(this.CityName));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}

function GetViewSeatAvailability() {
    if (($('#CourseType2').val() == 'choose' || $('#CourseType2').val() == undefined) && ($('#Session2').val() == 'choose' || $('#Session2').val() == undefined)) {
        var collegeId = 0;
        var courseTyp = 0;
        var year = 0;
        $.ajax({
            type: "GET",
            url: "/Admission/GetSeatAvailabilityListStatusFilter",
            data: { TabId: 0, 'collegeId': collegeId, 'courseType': courseTyp, 'academicYear': year },
            contentType: "application/json",
            success: function (data) {
                table = $('#seatAvailabilityTable').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'Slno', 'title': 'Sl.no', 'className': 'text-center' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                        { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                        { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                        { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                        { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left taluk' },
                        { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                        { 'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left' },
                        { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                        { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                        { 'data': 'SeatName', 'title': 'Seat Type', 'className': 'text-left' },
                        { 'data': 'GovtSeatAvailability', 'title': 'Govt Seat Availability', 'className': 'text-left' },
                        { 'data': 'ManagementSeatAvailability', 'title': 'Management Seat Availability', 'className': 'text-left' },

                    ]
                });
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        if ($('#CourseType2').val() == 'choose') {
            bootbox.alert('Please select course Type');
        } else
            if ($('#Session2').val() == 'choose') {
                bootbox.alert('Please select Session');
            }
            else {
                //var collegeId = $('#instituteDdp').val();
                //if (collegeId == undefined)
                //    collegeId = 0;

                var courseTyp = $('#CourseType2').val();
                if (courseTyp == undefined)
                    courseTyp = 0;

                var year = $('#Session2').val();
                if (year == undefined)
                    year = 0;
                $.ajax({
                    type: "GET",
                    url: "/Admission/GetSeatAvailabilityListStatusFilter",
                    data: { 'collegeId': 0, 'courseType': courseTyp, 'academicYear': year },
                    contentType: "application/json",
                    success: function (data) {
                        table = $('#seatAvailabilityTable').DataTable({
                            data: data,
                            "destroy": true,
                            "bSort": true,
                            columns: [
                                { 'data': 'Slno', 'title': 'Sl.no', 'className': 'text-center' },
                                { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                                { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                                { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                                { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                                { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                                { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left taluk' },
                                { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                                { 'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left' },
                                { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                                { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                                { 'data': 'SeatName', 'title': 'Seat Type', 'className': 'text-left' },
                                { 'data': 'GovtSeatAvailability', 'title': 'Govt Seat Availability', 'className': 'text-left' },
                                { 'data': 'ManagementSeatAvailability', 'title': 'Management Seat Availability', 'className': 'text-left' },
                            ]
                        });
                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }
    }
}


function getfilter(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.columns().search(valu).draw();
    }


    if (id == 1) {
        GetDistrictData('divisionDdp', 'districtDdp1', '1');
        if ($('#divisionDdp option:selected').text() == 'Choose') {
            valu = '';
        }
        table.columns(id).search(valu).draw();
    }
    else if (id == 2) {
        GetTaluksData('districtDdp1', 'talukDdp', '1');
        if ($('#districtDdp1 option:selected').text() == 'choose') {
            valu = $('#divisionDdp option:selected').text();
            id = id - 1;
        }
        table.columns(id).search(valu).draw();
    }
    else if (id == 3) {
        GetTaluksData('talukDdp', 'instituteDdp', '2');
        if ($('#talukDdp option:selected').text() == 'choose') {
            valu = $('#districtDdp1 option:selected').text();
            $('#instituteDdp option:selected').val(0)
            id = id - 1;
        }
        table.columns(id).search(valu).draw();
    }
    else {
        if ($('#instituteDdp option:selected').text() == 'choose')
            valu = $('#talukDdp option:selected').text();
        table.search(valu).draw();
    }

}

function ViewSeatsfilter(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table1.search(valu).draw();
    }


    if (id == 1) {
        GetDistrictData('ddlDivisionView', 'ddlDistrictView', '1');
        if ($('#ddlDivisionView option:selected').text() == 'Choose')
            valu = '';
        table1.search(valu).draw();
    }
    else if (id == 2) {
        GetTaluksData('ddlDistrictView', 'ddlTalukaView', '1');
        if ($('#ddlDistrictView option:selected').text() == 'choose')
            valu = $('#ddlDivisionView option:selected').text();
        table1.search(valu).draw();
    }
    else if (id == 3) {
        GetTaluksData('ddlTalukaView', 'ddlInstituteView', '2');
        if ($('#ddlTalukaView option:selected').text() == 'choose')
            valu = $('#ddlDistrictView option:selected').text();
        table1.search(valu).draw();
    }
    else {
        if ($('#ddlInstituteView option:selected').text() == 'choose')
            valu = $('#ddlTalukaView option:selected').text();
        table1.search(valu).draw();
    }

}

function ShowTreadSeatMatrix(id) {

    $("#TradeListModel").modal('show')

    $('#tblTradeListModel').show();
    ExamNotifyId = id
    $.ajax({
        type: "GET",
        url: "/Admission/AdmissionSeatMatrixNested",
        dataType: 'json',
        data: { id: id },
        success: function (data) {
            
            if (data.SelectListMatrix) {
                $('#tblTradeListModel').DataTable({
                    "bFilter": false,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'SelectListMatrix.slno', 'title': 'Slno', 'className': 'text-center slno' },
                        ////{ 'data': 'division_name', 'width': '100px', 'title': 'Division Name', 'className': 'text-center' },
                        ////{ 'data': 'district_ename', 'title': 'DistrictName', 'className': 'text-center' },
                        ////{ 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        //{ 'data': 'iti_college_id', 'title': 'InstituteType', 'className': 'text-left' },
                        { 'data': 'SelectListMatrix.iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                        { 'data': 'TradeName', 'title': 'TradeName', 'className': 'text-left' },

                        //Added Category GM
                        {
                            'data': 'GMWomenH', 'title': 'GMWH', 'className': 'text-center GMWH',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                var input = "<input type='text' class='GMWH' value='" + oData.GMWomenH + "' />"
                                $(nTd).html(input);
                            }
                        },
                    ],

                });
                $("#TradeListModel").modal('show')
            }
            else {
                $("#TradeListModel").modal('hide')
            }
        },
        error: function (e) {
            var _msg = "Something went wrong.";
            bootbox.alert(_msg);
            $("#preloder, .loader").hide();
        }
    });
}



// start generate seat matrix dhanraj================================
//tab 1 generate seat matrix
function changeRounds(sel, id) {
    $("#" + id).empty();
    $("#" + id).append('<option value="0">choose</option>');
    var round = $('#' + sel).val();
    $.ajax({
        url: "/Admission/GetAppRound",
        type: 'Get',
        data: { 'Roundtype': round },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $.each(data, function () {
                $("#" + id).append($("<option/>").val(this.ApplicantAdmissionRoundsId).text(this.RoundList));
            });
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GenerateSeatMatrixRoundwise() {
    $("#ApplicantTypeGenErr").text('');
    $("#AcademicyearGenErr").text('');
    $("#RoundGenErr").text('');
    //$('#courseTypeErr').text('');
    if ($("#ddlApplicantTypeGen").val() == 'choose' && $("#ddlAcademicYearGen").val() == 'choose' && $("#ddlRoundGen").val() == 'choose') {
        $("#ApplicantTypeGenErr").text('choose applicant type');
        $("#AcademicyearGenErr").text('choose academic year');
        $("#RoundGenErr").text('choose round');
        //$('#courseTypeErr').text('choose course type');
    }
    else if ($("#ddlApplicantTypeGen").val() == 'choose') {
        $("#ApplicantTypeGenErr").text('choose applicant type');
    }
    else if ($("#ddlAcademicYearGen").val() == 'choose') {
        $("#AcademicyearGenErr").text('choose academic year');
    }
    else if ($("#ddlRoundGen").val() == 0 || $("#ddlRoundGen").val() == 'choose') {
        $("#RoundGenErr").text('choose round');
    }
    else
        if ($("#courseType").val() == 'choose') {
            $('#courseTypeErr').text('choose course type');
        }
        else {
            let year = $("#ddlAcademicYearGen").val();
            let apptyp = $("#ddlApplicantTypeGen").val();
            let course = $("#courseType").val();
            let round = $("#ddlRoundGen").val();
            $.ajax({
                type: "GET",
                url: "/Admission/GetDivisionsInstituteTrades",
                data: { 'year': year, 'appType': apptyp, 'courseType': course, 'round': round },
                contentType: "application/json",
                success: function (data) {
                    
                    try {
                        if (data != null && data !="" && data!="\r\n") {
                            $('#SeatGenerateDiv').html(data);

                            $('#seatGenerate').show();
                            $('#divuserSubmit').show();
                        }
                        else
                        {
                            alert("No trades are bounded from institutes");
                        }
                    }
                    catch {
                        alert("Something went wrong, please generate again.");
                    }
                }
            });
        }
}
function SubmitSeatMatrixCollegeWise(tableId, instituteId) {
    
    let year = $("#ddlAcademicYearGen").val();
    let apptyp = $("#ddlApplicantTypeGen").val();
    let course = $("#courseType").val();
    let round = $("#ddlRoundGen").val();
    var valid = true;
    var listItem = [];
    var table = $("#" + tableId + " tbody");

    table.find('tr').each(function (len) {
        var $tr = $(this);
        let tardename = $tr.find("td:eq(0)").text();
        let tardeId = $tr.find("td:eq(0) input").val();
        let seats = parseInt($tr.find("td:eq(3)").text());
        if (round == 1 || round == 2) {
            let GMWH = $tr.find("td:eq(5) input").val();
            let GMWNH = $tr.find("td:eq(6) input").val();
            let GMPDH = $tr.find("td:eq(7) input").val();
            let GMPDNH = $tr.find("td:eq(8) input").val();
            let GMEXSH = $tr.find("td:eq(9) input").val();
            let GMEXSNH = $tr.find("td:eq(10) input").val();
            let GMKMH = $tr.find("td:eq(11) input").val();
            let GMKMNH = $tr.find("td:eq(12) input").val();
            let GMEWSH = $tr.find("td:eq(13) input").val();
            let GMEWSNH = $tr.find("td:eq(14) input").val();
            let GMGPH = $tr.find("td:eq(15) input").val();
            let GMGPNH = $tr.find("td:eq(16) input").val();
            let SCWH = $tr.find("td:eq(17) input").val();
            let SCWNH = $tr.find("td:eq(18) input").val();
            let SCPDH = $tr.find("td:eq(19) input").val();
            let SCPDNH = $tr.find("td:eq(20) input").val();
            let SCEXSH = $tr.find("td:eq(21) input").val();
            let SCEXSNH = $tr.find("td:eq(22) input").val();
            let SCKMH = $tr.find("td:eq(23) input").val();
            let SCKMNH = $tr.find("td:eq(24) input").val();
            let SCEWSH = $tr.find("td:eq(25) input").val();
            let SCEWSNH = $tr.find("td:eq(26) input").val();
            let SCGPH = $tr.find("td:eq(27) input").val();
            let SCGPNH = $tr.find("td:eq(28) input").val();
            let STWH = $tr.find("td:eq(29) input").val();
            let STWNH = $tr.find("td:eq(30) input").val();
            let STPDH = $tr.find("td:eq(31) input").val();
            let STPDNH = $tr.find("td:eq(32) input").val();
            let STEXSH = $tr.find("td:eq(33) input").val();
            let STEXSNH = $tr.find("td:eq(34) input").val();
            let STKMH = $tr.find("td:eq(35) input").val();
            let STKMNH = $tr.find("td:eq(36) input").val();
            let STEWSH = $tr.find("td:eq(37) input").val();
            let STEWSNH = $tr.find("td:eq(38) input").val();
            let STGPH = $tr.find("td:eq(39) input").val();
            let STGPNH = $tr.find("td:eq(40) input").val();
            let C1WH = $tr.find("td:eq(41) input").val();
            let C1WNH = $tr.find("td:eq(42) input").val();
            let C1PDH = $tr.find("td:eq(43) input").val();
            let C1PDNH = $tr.find("td:eq(44) input").val();
            let C1EXSH = $tr.find("td:eq(45) input").val();
            let C1EXSNH = $tr.find("td:eq(46) input").val();
            let C1KMH = $tr.find("td:eq(47) input").val();
            let C1KMNH = $tr.find("td:eq(48) input").val();
            let C1EWSH = $tr.find("td:eq(49) input").val();
            let C1EWSNH = $tr.find("td:eq(50) input").val();
            let C1GPH = $tr.find("td:eq(51) input").val();
            let C1GPNH = $tr.find("td:eq(52) input").val();
            let TWOAWH = $tr.find("td:eq(53) input").val();
            let TWOAWNH = $tr.find("td:eq(54) input").val();
            let TWOAPDH = $tr.find("td:eq(55) input").val();
            let TWOAPDNH = $tr.find("td:eq(56) input").val();
            let TWOAEXSH = $tr.find("td:eq(57) input").val();
            let TWOAEXSNH = $tr.find("td:eq(58) input").val();
            let TWOAKMH = $tr.find("td:eq(59) input").val();
            let TWOAKMNH = $tr.find("td:eq(60) input").val();
            let TWOAEWSH = $tr.find("td:eq(61) input").val();
            let TWOAEWSNH = $tr.find("td:eq(62) input").val();
            let TWOAGPH = $tr.find("td:eq(63) input").val();
            let TWOAGPNH = $tr.find("td:eq(64) input").val();
            let TWOBWH = $tr.find("td:eq(65) input").val();
            let TWOBWNH = $tr.find("td:eq(66) input").val();
            let TWOBPDH = $tr.find("td:eq(67) input").val();
            let TWOBPDNH = $tr.find("td:eq(68) input").val();
            let TWOBEXSH = $tr.find("td:eq(69) input").val();
            let TWOBEXSNH = $tr.find("td:eq(70) input").val();
            let TWOBKMH = $tr.find("td:eq(71) input").val();
            let TWOBKMNH = $tr.find("td:eq(72) input").val();
            let TWOBEWSH = $tr.find("td:eq(73) input").val();
            let TWOBEWSNH = $tr.find("td:eq(74) input").val();
            let TWOBGPH = $tr.find("td:eq(75) input").val();
            let TWOBGPNH = $tr.find("td:eq(76) input").val();
            let THREEAWH = $tr.find("td:eq(77) input").val();
            let THREEAWNH = $tr.find("td:eq(78) input").val();
            let THREEAPDH = $tr.find("td:eq(79) input").val();
            let THREEAPDNH = $tr.find("td:eq(80) input").val();
            let THREEAEXSH = $tr.find("td:eq(81) input").val();
            let THREEAEXSNH = $tr.find("td:eq(82) input").val();
            let THREEAKMH = $tr.find("td:eq(83) input").val();
            let THREEAKMNH = $tr.find("td:eq(84) input").val();
            let THREEAEWSH = $tr.find("td:eq(85) input").val();
            let THREEAEWSNH = $tr.find("td:eq(86) input").val();
            let THREEAGPH = $tr.find("td:eq(87) input").val();
            let THREEAGPNH = $tr.find("td:eq(88) input").val();
            let THREEBWH = $tr.find("td:eq(89) input").val();
            let THREEBWNH = $tr.find("td:eq(90) input").val();
            let THREEBPDH = $tr.find("td:eq(91) input").val();
            let THREEBPDNH = $tr.find("td:eq(92) input").val();
            let THREEBEXSH = $tr.find("td:eq(93) input").val();
            let THREEBEXSNH = $tr.find("td:eq(94) input").val();
            let THREEBKMH = $tr.find("td:eq(95) input").val();
            let THREEBKMNH = $tr.find("td:eq(96) input").val();
            let THREEBEWSH = $tr.find("td:eq(97) input").val();
            let THREEBEWSNH = $tr.find("td:eq(98) input").val();
            let THREEBGPH = $tr.find("td:eq(99) input").val();
            let THREEBGPNH = $tr.find("td:eq(100) input").val();
            var allseats = parseInt(GMWH) + parseInt(GMWNH) + parseInt(GMPDH) + parseInt(GMPDNH) + parseInt(GMEXSH) + parseInt(GMEXSNH) + parseInt(GMKMH) + parseInt(GMKMNH) + parseInt(GMEWSH)
                + parseInt(GMEWSNH) + parseInt(GMGPH) + parseInt(GMGPNH) + parseInt(SCWH) + parseInt(SCWNH) + parseInt(SCPDH) + parseInt(SCPDNH) + parseInt(SCEXSH) + parseInt(SCEXSNH) + parseInt(SCKMH)
                + parseInt(SCKMNH) + parseInt(SCEWSH) + parseInt(SCEWSNH) + parseInt(SCGPH) + parseInt(SCGPNH) + parseInt(STWH) + parseInt(STWNH) + parseInt(STPDH) + parseInt(STPDNH) + parseInt(STEXSH)
                + parseInt(STEXSNH) + parseInt(STKMH) + parseInt(STKMNH) + parseInt(STEWSH) + parseInt(STEWSNH) + parseInt(STGPH) + parseInt(STGPNH) + parseInt(C1WH) + parseInt(C1WNH) + parseInt(C1PDH)
                + parseInt(C1PDNH) + parseInt(C1EXSH) + parseInt(C1EXSNH) + parseInt(C1KMH) + parseInt(C1KMNH) + parseInt(C1EWSH) + parseInt(C1EWSNH) + parseInt(C1GPH) + parseInt(C1GPNH) + parseInt(TWOAWH)
                + parseInt(TWOAWNH) + parseInt(TWOAPDH) + parseInt(TWOAPDNH) + parseInt(TWOAEXSH) + parseInt(TWOAEXSNH) + parseInt(TWOAKMH) + parseInt(TWOAKMNH) + parseInt(TWOAEWSH) + parseInt(TWOAEWSNH)
                + parseInt(TWOAGPH) + parseInt(TWOAGPNH) + parseInt(TWOBWH) + parseInt(TWOBWNH) + parseInt(TWOBPDH) + parseInt(TWOBPDNH) + parseInt(TWOBEXSH) + parseInt(TWOBEXSNH) + parseInt(TWOBKMH)
                + parseInt(TWOBKMNH) + parseInt(TWOBEWSH) + parseInt(TWOBEWSNH) + parseInt(TWOBGPH) + parseInt(TWOBGPNH) + parseInt(THREEAWH) + parseInt(THREEAWNH) + parseInt(THREEAPDH) + parseInt(THREEAPDNH)
                + parseInt(THREEAEXSH) + parseInt(THREEAEXSNH) + parseInt(THREEAKMH) + parseInt(THREEAKMNH) + parseInt(THREEAEWSH) + parseInt(THREEAEWSNH) + parseInt(THREEAGPH) + parseInt(THREEAGPNH)
                + parseInt(THREEBWH) + parseInt(THREEBWNH) + parseInt(THREEBPDH) + parseInt(THREEBPDNH) + parseInt(THREEBEXSH) + parseInt(THREEBEXSNH) + parseInt(THREEBKMH) + parseInt(THREEBKMNH)
                + parseInt(THREEBEWSH) + parseInt(THREEBEWSNH) + parseInt(THREEBGPH) + parseInt(THREEBGPNH);

            var list = {
                TradeId: tardeId,
                GMWH: GMWH,
                GMWNH: GMWNH,
                GMPDH: GMPDH,
                GMPDNH: GMPDNH,
                GMEXSH: GMEXSH,
                GMEXSNH: GMEXSNH,
                GMKMH: GMKMH,
                GMKMNH: GMKMNH,
                GMEWSH: GMEWSH,
                GMEWSNH: GMEWSNH,
                GMGPH: GMGPH,
                GMGPNH: GMGPNH,
                SCWH: SCWH,
                SCWNH: SCWNH,
                SCPDH: SCPDH,
                SCPDNH: SCPDNH,
                SCEXSH: SCEXSH,
                SCEXSNH: SCEXSNH,
                SCKMH: SCKMH,
                SCKMNH: SCKMNH,
                SCEWSH: SCEWSH,
                SCEWSNH: SCEWSNH,
                SCGPH: SCGPH,
                SCGPNH: SCGPNH,
                STWH: STWH,
                STWNH: STWNH,
                STPDH: STPDH,
                STPDNH: STPDNH,
                STEXSH: STEXSH,
                STEXSNH: STEXSNH,
                STKMH: STKMH,
                STKMNH: STKMNH,
                STEWSH: STEWSH,
                STEWSNH: STEWSNH,
                STGPH: STGPH,
                STGPNH: STGPNH,
                C1WH: C1WH,
                C1WNH: C1WNH,
                C1PDH: C1PDH,
                C1PDNH: C1PDNH,
                C1EXSH: C1EXSH,
                C1EXSNH: C1EXSNH,
                C1KMH: C1KMH,
                C1KMNH: C1KMNH,
                C1EWSH: C1EWSH,
                C1EWSNH: C1EWSNH,
                C1GPH: C1GPH,
                C1GPNH: C1GPNH,
                TWOAWH: TWOAWH,
                TWOAWNH: TWOAWNH,
                TWOAPDH: TWOAPDH,
                TWOAPDNH: TWOAPDNH,
                TWOAEXSH: TWOAEXSH,
                TWOAEXSNH: TWOAEXSNH,
                TWOAKMH: TWOAKMH,
                TWOAKMNH: TWOAKMNH,
                TWOAEWSH: TWOAEWSH,
                TWOAEWSNH: TWOAEWSNH,
                TWOAGPH: TWOAGPH,
                TWOAGPNH: TWOAGPNH,
                TWOBWH: TWOBWH,
                TWOBWNH: TWOBWNH,
                TWOBPDH: TWOBPDH,
                TWOBPDNH: TWOBPDNH,
                TWOBEXSH: TWOBEXSH,
                TWOBEXSNH: TWOBEXSNH,
                TWOBKMH: TWOBKMH,
                TWOBKMNH: TWOBKMNH,
                TWOBEWSH: TWOBEWSH,
                TWOBEWSNH: TWOBEWSNH,
                TWOBGPH: TWOBGPH,
                TWOBGPNH: TWOBGPNH,
                THREEAWH: THREEAWH,
                THREEAWNH: THREEAWNH,
                THREEAPDH: THREEAPDH,
                THREEAPDNH: THREEAPDNH,
                THREEAEXSH: THREEAEXSH,
                THREEAEXSNH: THREEAEXSNH,
                THREEAKMH: THREEAKMH,
                THREEAKMNH: THREEAKMNH,
                THREEAEWSH: THREEAEWSH,
                THREEAEWSNH: THREEAEWSNH,
                THREEAGPH: THREEAGPH,
                THREEAGPNH: THREEAGPNH,
                THREEBWH: THREEBWH,
                THREEBWNH: THREEBWNH,
                THREEBPDH: THREEBPDH,
                THREEBPDNH: THREEBPDNH,
                THREEBEXSH: THREEBEXSH,
                THREEBEXSNH: THREEBEXSNH,
                THREEBKMH: THREEBKMH,
                THREEBKMNH: THREEBKMNH,
                THREEBEWSH: THREEBEWSH,
                THREEBEWSNH: THREEBEWSNH,
                THREEBGPH: THREEBGPH,
                THREEBGPNH: THREEBGPNH,
            }
        }
        else
            if (round == 6) {
                let GMGP = $tr.find("td:eq(5) input").val();
                var list = {
                    TradeId: tardeId,
                    GMGP: GMGP
                }

                var allseats = parseInt(GMGP);
            }
            else {
                let GMGP = $tr.find("td:eq(5) input").val();
                let SCGP = $tr.find("td:eq(6) input").val();
                let STGP = $tr.find("td:eq(7) input").val();
                let C1GP = $tr.find("td:eq(8) input").val();
                let TWOAGP = $tr.find("td:eq(9) input").val();
                let TWOBGP = $tr.find("td:eq(10) input").val();
                let THREEAGP = $tr.find("td:eq(11) input").val();
                let THREEBGP = $tr.find("td:eq(12) input").val();

                var list = {
                    TradeId: tardeId,
                    GMGP: GMGP,
                    SCGP: SCGP,
                    STGP: STGP,
                    C1GP: C1GP,
                    TWOAGP: TWOAGP,
                    TWOBGP: TWOBGP,
                    THREEAGP: THREEAGP,
                    THREEBGP: THREEBGP
                }

                var allseats = parseInt(GMGP) + parseInt(SCGP) + parseInt(STGP) + parseInt(C1GP) + parseInt(TWOAGP) + parseInt(TWOBGP) + parseInt(THREEAGP) + parseInt(THREEBGP);
            }

        if (seats != allseats) {
            valid = false;
        }
        else {
            listItem.push(list);
        }
    });
    var sss = JSON.stringify(listItem);
    if (valid == true) {
        $.ajax({
            type: "POST",
            url: "/Admission/SubmitSeatMatrixCollegeWise",
            data: '{ "listItem":' + sss + ',"collegeId":' + instituteId + ',"year":' + year + ',"appType":' + apptyp + ',"courseType":' + course + ',"round":' + round + '}',
            contentType: "application/json",
            success: function (data) {
                if (data == true)
                    bootbox.alert('successfully allocated');
                else if (data == false)
                    bootbox.alert('institute seat matrix already generated for this round failed');
                else
                    bootbox.alert('seat matrix generate failed');
            },
            error: function (res) {
                bootbox.alert('seat matrix generate failed');
            }
        });
    }
    else {
        bootbox.alert('No of modified seats across all reservation should match with no of available seats');
        return false;
    }
}
function SubmitMainseatMatrix() {
    $('#spnselectdropdown').text('');
    let year = $("#ddlAcademicYearGen").val();
    let apptyp = $("#ddlApplicantTypeGen").val();
    let course = $("#courseType").val();
    let round = $("#ddlRoundGen").val();
    var remark = $('#SubmitRemarks').val();
    let roleId = $("#users").val();
    if (roleId == 'choose') {
        $('#spnselectdropdown').text('select the role');
    }
    else {
        bootbox.confirm('Do you want to submit seat metrix to ' + $("#users option:selected").text() + '?', (confirma) => {
            if (confirma) {
                $.ajax({
                    type: "GET",
                    url: "/Admission/SubmitSeatMatrix",
                    data: { 'year': year, 'appType': apptyp, 'courseType': course, 'round': round, 'role': roleId, 'remark': remark },
                    contentType: "application/json",
                    success: function (data) {
                        if (data == true) {
                            bootbox.alert('seat matrix submitted successfully');
                            $('#btnsubmit').attr('disabled', true);
                        }
                        else
                            bootbox.alert('failed');
                    }
                });
            }
        });
    }

}

//Tab-3-Update Seat Matrix
function UpdateSeatMatrix() {
    var AcademicYearId = $('#ddlAcademicYearUpdate :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetUpdateSeatMatrix",
        data: { AcademicYearId: AcademicYearId },
        contentType: "application/json",
        success: function (data) {
            $('#tblUpdateseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'ApplicantTypeDdl', 'title': 'Applicant Type', 'className': 'text-center ApplicantTypeDdl' },
                    { 'data': 'coursetypename', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'Round', 'title': 'Round', 'className': 'text-center Round' },
                    { 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left StatusName' },
                    {
                        'data': 'SeatMaxId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewRremarks(" + oData.ApplicantType + "," + oData.Round +
                                "," + oData.courseid + ", " + 2 + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    },
                    {
                        'data': 'SeatMaxId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetCollegeWiseSeatmatrixData(" + oData.SeatMaxId +
                                "," + oData.ApplicantType + "," + oData.Round + "," + oData.courseid + "," + oData.StatusId +
                                ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                    
                ],
            });
        }
    });
}
function GetCollegeWiseSeatmatrixData(SeatMaxId, applId, round, courseID, StatusId) {
    $('#listTable').hide();
    $('#courseIdUpdate').val(courseID);
    $('#apptypeIDUpdate').val(applId);
    $('#roundIDUpdate').val(round);
    $('#Collegepartialview').show();
    $('#updateDiv').hide();
    var year = $('#ddlAcademicYearUpdate').val();
    $.ajax({
        type: "GET",
        url: "/Admission/GetDivisionsInstituteTradesUpdate",
        data: { 'year': year, 'round': round, 'applicantType': applId, 'courseId': courseID },
        contentType: "application/json",
        success: function (data) {
            if (data != null)
                $('#MyPrivateDiv').html(data);

            $('#Collegepartialview').show();
            if (StatusId == 9) {
                $('.update').show();
                $('#BackBtn').hide();
                $('#updateDiv').show();                  
            }
            else {
                $('.update').hide();
                $('#updateDiv').hide();
                $('#BackBtn').show();
                
            }               
        }
    });
}


function UpdateSeatMatrixCollegeWise(tableId, instituteId) {
    let year = $("#ddlAcademicYearUpdate").val();
    //let apptyp = $("#ddlApplicantTypeGen").val();
    //let course = $("#courseType").val();
    let course=$('#courseIdUpdate').val();
    let apptyp=$('#apptypeIDUpdate').val();
    var round = $('#roundIDUpdate').val();
    var valid = true;
    var listItem = [];
    var table = $("#" + tableId + " tbody");

    table.find('tr').each(function (len) {
        var $tr = $(this);
        let tardename = $tr.find("td:eq(0)").text();
        let tardeId = $tr.find("td:eq(0) input").val();
        let seats = parseInt($tr.find("td:eq(3)").text());
        round = parseInt($tr.find("td:eq(3) input").val());
        if (round == 1 || round == 2) {
            let GMWH = $tr.find("td:eq(5) input").val();
            let GMWNH = $tr.find("td:eq(6) input").val();
            let GMPDH = $tr.find("td:eq(7) input").val();
            let GMPDNH = $tr.find("td:eq(8) input").val();
            let GMEXSH = $tr.find("td:eq(9) input").val();
            let GMEXSNH = $tr.find("td:eq(10) input").val();
            let GMKMH = $tr.find("td:eq(11) input").val();
            let GMKMNH = $tr.find("td:eq(12) input").val();
            let GMEWSH = $tr.find("td:eq(13) input").val();
            let GMEWSNH = $tr.find("td:eq(14) input").val();
            let GMGPH = $tr.find("td:eq(15) input").val();
            let GMGPNH = $tr.find("td:eq(16) input").val();
            let SCWH = $tr.find("td:eq(17) input").val();
            let SCWNH = $tr.find("td:eq(18) input").val();
            let SCPDH = $tr.find("td:eq(19) input").val();
            let SCPDNH = $tr.find("td:eq(20) input").val();
            let SCEXSH = $tr.find("td:eq(21) input").val();
            let SCEXSNH = $tr.find("td:eq(22) input").val();
            let SCKMH = $tr.find("td:eq(23) input").val();
            let SCKMNH = $tr.find("td:eq(24) input").val();
            let SCEWSH = $tr.find("td:eq(25) input").val();
            let SCEWSNH = $tr.find("td:eq(26) input").val();
            let SCGPH = $tr.find("td:eq(27) input").val();
            let SCGPNH = $tr.find("td:eq(28) input").val();
            let STWH = $tr.find("td:eq(29) input").val();
            let STWNH = $tr.find("td:eq(30) input").val();
            let STPDH = $tr.find("td:eq(31) input").val();
            let STPDNH = $tr.find("td:eq(32) input").val();
            let STEXSH = $tr.find("td:eq(33) input").val();
            let STEXSNH = $tr.find("td:eq(34) input").val();
            let STKMH = $tr.find("td:eq(35) input").val();
            let STKMNH = $tr.find("td:eq(36) input").val();
            let STEWSH = $tr.find("td:eq(37) input").val();
            let STEWSNH = $tr.find("td:eq(38) input").val();
            let STGPH = $tr.find("td:eq(39) input").val();
            let STGPNH = $tr.find("td:eq(40) input").val();
            let C1WH = $tr.find("td:eq(41) input").val();
            let C1WNH = $tr.find("td:eq(42) input").val();
            let C1PDH = $tr.find("td:eq(43) input").val();
            let C1PDNH = $tr.find("td:eq(44) input").val();
            let C1EXSH = $tr.find("td:eq(45) input").val();
            let C1EXSNH = $tr.find("td:eq(46) input").val();
            let C1KMH = $tr.find("td:eq(47) input").val();
            let C1KMNH = $tr.find("td:eq(48) input").val();
            let C1EWSH = $tr.find("td:eq(49) input").val();
            let C1EWSNH = $tr.find("td:eq(50) input").val();
            let C1GPH = $tr.find("td:eq(51) input").val();
            let C1GPNH = $tr.find("td:eq(52) input").val();
            let TWOAWH = $tr.find("td:eq(53) input").val();
            let TWOAWNH = $tr.find("td:eq(54) input").val();
            let TWOAPDH = $tr.find("td:eq(55) input").val();
            let TWOAPDNH = $tr.find("td:eq(56) input").val();
            let TWOAEXSH = $tr.find("td:eq(57) input").val();
            let TWOAEXSNH = $tr.find("td:eq(58) input").val();
            let TWOAKMH = $tr.find("td:eq(59) input").val();
            let TWOAKMNH = $tr.find("td:eq(60) input").val();
            let TWOAEWSH = $tr.find("td:eq(61) input").val();
            let TWOAEWSNH = $tr.find("td:eq(62) input").val();
            let TWOAGPH = $tr.find("td:eq(63) input").val();
            let TWOAGPNH = $tr.find("td:eq(64) input").val();
            let TWOBWH = $tr.find("td:eq(65) input").val();
            let TWOBWNH = $tr.find("td:eq(66) input").val();
            let TWOBPDH = $tr.find("td:eq(67) input").val();
            let TWOBPDNH = $tr.find("td:eq(68) input").val();
            let TWOBEXSH = $tr.find("td:eq(69) input").val();
            let TWOBEXSNH = $tr.find("td:eq(70) input").val();
            let TWOBKMH = $tr.find("td:eq(71) input").val();
            let TWOBKMNH = $tr.find("td:eq(72) input").val();
            let TWOBEWSH = $tr.find("td:eq(73) input").val();
            let TWOBEWSNH = $tr.find("td:eq(74) input").val();
            let TWOBGPH = $tr.find("td:eq(75) input").val();
            let TWOBGPNH = $tr.find("td:eq(76) input").val();
            let THREEAWH = $tr.find("td:eq(77) input").val();
            let THREEAWNH = $tr.find("td:eq(78) input").val();
            let THREEAPDH = $tr.find("td:eq(79) input").val();
            let THREEAPDNH = $tr.find("td:eq(80) input").val();
            let THREEAEXSH = $tr.find("td:eq(81) input").val();
            let THREEAEXSNH = $tr.find("td:eq(82) input").val();
            let THREEAKMH = $tr.find("td:eq(83) input").val();
            let THREEAKMNH = $tr.find("td:eq(84) input").val();
            let THREEAEWSH = $tr.find("td:eq(85) input").val();
            let THREEAEWSNH = $tr.find("td:eq(86) input").val();
            let THREEAGPH = $tr.find("td:eq(87) input").val();
            let THREEAGPNH = $tr.find("td:eq(88) input").val();
            let THREEBWH = $tr.find("td:eq(89) input").val();
            let THREEBWNH = $tr.find("td:eq(90) input").val();
            let THREEBPDH = $tr.find("td:eq(91) input").val();
            let THREEBPDNH = $tr.find("td:eq(92) input").val();
            let THREEBEXSH = $tr.find("td:eq(93) input").val();
            let THREEBEXSNH = $tr.find("td:eq(94) input").val();
            let THREEBKMH = $tr.find("td:eq(95) input").val();
            let THREEBKMNH = $tr.find("td:eq(96) input").val();
            let THREEBEWSH = $tr.find("td:eq(97) input").val();
            let THREEBEWSNH = $tr.find("td:eq(98) input").val();
            let THREEBGPH = $tr.find("td:eq(99) input").val();
            let THREEBGPNH = $tr.find("td:eq(100) input").val();
            var allseats = parseInt(GMWH) + parseInt(GMWNH) + parseInt(GMPDH) + parseInt(GMPDNH) + parseInt(GMEXSH) + parseInt(GMEXSNH) + parseInt(GMKMH) + parseInt(GMKMNH) + parseInt(GMEWSH)
                + parseInt(GMEWSNH) + parseInt(GMGPH) + parseInt(GMGPNH) + parseInt(SCWH) + parseInt(SCWNH) + parseInt(SCPDH) + parseInt(SCPDNH) + parseInt(SCEXSH) + parseInt(SCEXSNH) + parseInt(SCKMH)
                + parseInt(SCKMNH) + parseInt(SCEWSH) + parseInt(SCEWSNH) + parseInt(SCGPH) + parseInt(SCGPNH) + parseInt(STWH) + parseInt(STWNH) + parseInt(STPDH) + parseInt(STPDNH) + parseInt(STEXSH)
                + parseInt(STEXSNH) + parseInt(STKMH) + parseInt(STKMNH) + parseInt(STEWSH) + parseInt(STEWSNH) + parseInt(STGPH) + parseInt(STGPNH) + parseInt(C1WH) + parseInt(C1WNH) + parseInt(C1PDH)
                + parseInt(C1PDNH) + parseInt(C1EXSH) + parseInt(C1EXSNH) + parseInt(C1KMH) + parseInt(C1KMNH) + parseInt(C1EWSH) + parseInt(C1EWSNH) + parseInt(C1GPH) + parseInt(C1GPNH) + parseInt(TWOAWH)
                + parseInt(TWOAWNH) + parseInt(TWOAPDH) + parseInt(TWOAPDNH) + parseInt(TWOAEXSH) + parseInt(TWOAEXSNH) + parseInt(TWOAKMH) + parseInt(TWOAKMNH) + parseInt(TWOAEWSH) + parseInt(TWOAEWSNH)
                + parseInt(TWOAGPH) + parseInt(TWOAGPNH) + parseInt(TWOBWH) + parseInt(TWOBWNH) + parseInt(TWOBPDH) + parseInt(TWOBPDNH) + parseInt(TWOBEXSH) + parseInt(TWOBEXSNH) + parseInt(TWOBKMH)
                + parseInt(TWOBKMNH) + parseInt(TWOBEWSH) + parseInt(TWOBEWSNH) + parseInt(TWOBGPH) + parseInt(TWOBGPNH) + parseInt(THREEAWH) + parseInt(THREEAWNH) + parseInt(THREEAPDH) + parseInt(THREEAPDNH)
                + parseInt(THREEAEXSH) + parseInt(THREEAEXSNH) + parseInt(THREEAKMH) + parseInt(THREEAKMNH) + parseInt(THREEAEWSH) + parseInt(THREEAEWSNH) + parseInt(THREEAGPH) + parseInt(THREEAGPNH)
                + parseInt(THREEBWH) + parseInt(THREEBWNH) + parseInt(THREEBPDH) + parseInt(THREEBPDNH) + parseInt(THREEBEXSH) + parseInt(THREEBEXSNH) + parseInt(THREEBKMH) + parseInt(THREEBKMNH)
                + parseInt(THREEBEWSH) + parseInt(THREEBEWSNH) + parseInt(THREEBGPH) + parseInt(THREEBGPNH);
            console.log(allseats);
            var list = {
                TradeId: tardeId,
                GMWH: GMWH,
                GMWNH: GMWNH,
                GMPDH: GMPDH,
                GMPDNH: GMPDNH,
                GMEXSH: GMEXSH,
                GMEXSNH: GMEXSNH,
                GMKMH: GMKMH,
                GMKMNH: GMKMNH,
                GMEWSH: GMEWSH,
                GMEWSNH: GMEWSNH,
                GMGPH: GMGPH,
                GMGPNH: GMGPNH,
                SCWH: SCWH,
                SCWNH: SCWNH,
                SCPDH: SCPDH,
                SCPDNH: SCPDNH,
                SCEXSH: SCEXSH,
                SCEXSNH: SCEXSNH,
                SCKMH: SCKMH,
                SCKMNH: SCKMNH,
                SCEWSH: SCEWSH,
                SCEWSNH: SCEWSNH,
                SCGPH: SCGPH,
                SCGPNH: SCGPNH,
                STWH: STWH,
                STWNH: STWNH,
                STPDH: STPDH,
                STPDNH: STPDNH,
                STEXSH: STEXSH,
                STEXSNH: STEXSNH,
                STKMH: STKMH,
                STKMNH: STKMNH,
                STEWSH: STEWSH,
                STEWSNH: STEWSNH,
                STGPH: STGPH,
                STGPNH: STGPNH,
                C1WH: C1WH,
                C1WNH: C1WNH,
                C1PDH: C1PDH,
                C1PDNH: C1PDNH,
                C1EXSH: C1EXSH,
                C1EXSNH: C1EXSNH,
                C1KMH: C1KMH,
                C1KMNH: C1KMNH,
                C1EWSH: C1EWSH,
                C1EWSNH: C1EWSNH,
                C1GPH: C1GPH,
                C1GPNH: C1GPNH,
                TWOAWH: TWOAWH,
                TWOAWNH: TWOAWNH,
                TWOAPDH: TWOAPDH,
                TWOAPDNH: TWOAPDNH,
                TWOAEXSH: TWOAEXSH,
                TWOAEXSNH: TWOAEXSNH,
                TWOAKMH: TWOAKMH,
                TWOAKMNH: TWOAKMNH,
                TWOAEWSH: TWOAEWSH,
                TWOAEWSNH: TWOAEWSNH,
                TWOAGPH: TWOAGPH,
                TWOAGPNH: TWOAGPNH,
                TWOBWH: TWOBWH,
                TWOBWNH: TWOBWNH,
                TWOBPDH: TWOBPDH,
                TWOBPDNH: TWOBPDNH,
                TWOBEXSH: TWOBEXSH,
                TWOBEXSNH: TWOBEXSNH,
                TWOBKMH: TWOBKMH,
                TWOBKMNH: TWOBKMNH,
                TWOBEWSH: TWOBEWSH,
                TWOBEWSNH: TWOBEWSNH,
                TWOBGPH: TWOBGPH,
                TWOBGPNH: TWOBGPNH,
                THREEAWH: THREEAWH,
                THREEAWNH: THREEAWNH,
                THREEAPDH: THREEAPDH,
                THREEAPDNH: THREEAPDNH,
                THREEAEXSH: THREEAEXSH,
                THREEAEXSNH: THREEAEXSNH,
                THREEAKMH: THREEAKMH,
                THREEAKMNH: THREEAKMNH,
                THREEAEWSH: THREEAEWSH,
                THREEAEWSNH: THREEAEWSNH,
                THREEAGPH: THREEAGPH,
                THREEAGPNH: THREEAGPNH,
                THREEBWH: THREEBWH,
                THREEBWNH: THREEBWNH,
                THREEBPDH: THREEBPDH,
                THREEBPDNH: THREEBPDNH,
                THREEBEXSH: THREEBEXSH,
                THREEBEXSNH: THREEBEXSNH,
                THREEBKMH: THREEBKMH,
                THREEBKMNH: THREEBKMNH,
                THREEBEWSH: THREEBEWSH,
                THREEBEWSNH: THREEBEWSNH,
                THREEBGPH: THREEBGPH,
                THREEBGPNH: THREEBGPNH,
            }
        }
        else
            if (round == 6) {
                let GMGP = $tr.find("td:eq(5) input").val();
                var list = {
                    TradeId: tardeId,
                    GMGP: GMGP
                }

                var allseats = parseInt(GMGP);
            }
            else {
                let GMGP = $tr.find("td:eq(5) input").val();
                let SCGP = $tr.find("td:eq(6) input").val();
                let STGP = $tr.find("td:eq(7) input").val();
                let C1GP = $tr.find("td:eq(8) input").val();
                let TWOAGP = $tr.find("td:eq(9) input").val();
                let TWOBGP = $tr.find("td:eq(10) input").val();
                let THREEAGP = $tr.find("td:eq(11) input").val();
                let THREEBGP = $tr.find("td:eq(12) input").val();

                var list = {
                    TradeId: tardeId,
                    GMGP: GMGP,
                    SCGP: SCGP,
                    STGP: STGP,
                    C1GP: C1GP,
                    TWOAGP: TWOAGP,
                    TWOBGP: TWOBGP,
                    THREEAGP: THREEAGP,
                    THREEBGP: THREEBGP
                }

                var allseats = parseInt(GMGP) + parseInt(SCGP) + parseInt(STGP) + parseInt(C1GP) + parseInt(TWOAGP) + parseInt(TWOBGP) + parseInt(THREEAGP) + parseInt(THREEBGP);
            }

        if (seats != allseats) {
            valid = false;
        }
        else {
            listItem.push(list);
        }
    });
    var sss = JSON.stringify(listItem);
    if (valid == true) {
        $.ajax({
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            url: "/Admission/UpdateSeatMatrixCollegeWise",
            data: '{ "listItem":' + sss + ',"collegeId":' + instituteId + ',"year":' + year +
                ',"appType":' + apptyp + ',"courseType":' + course + ',"round":' + round + '}',
            contentType: "application/json",
            success: function (data) {
                if (data == true)
                    bootbox.alert('successfully allocated');
                else
                    bootbox.alert('failed');
            }
        });
    }
    else {
        bootbox.alert('No of modified seats across all reservation should match with no of available seats');
        return false;
    }
}
function UpdateMainSeatMatrix() {
    $('#spnselectdropdown').text('');
    let year = $("#ddlAcademicYearUpdate").val();
    //let apptyp = $("#ddlApplicantTypeGen").val();
    //let course = $("#courseType").val();
    //let round = $("#ddlRoundGen").val();
    let course = $('#courseIdUpdate').val();
    let apptyp = $('#apptypeIDUpdate').val();
    let round = $('#roundIDUpdate').val();
    let roleId = $("#Upusers").val();
    if (roleId == 'choose') {
        $('#spnselectdropdown').text('select the role');
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Admission/UpdateSeatMatrix",
            data: '{ "year":' + year + ',"appType":' + apptyp + ',"courseType":' + course + ',"round":' + round + ',"role":' + roleId + '}',
            contentType: "application/json",
            success: function (data) {
                if (data == true) {
                    bootbox.alert('seat matrix updated and submitted successfully');
                    $('#updateBtn').attr('disabled', true);
                }
                else
                    bootbox.alert('failed');
            }
        });
    }
}
function cancelUpdate() {
    $('#Collegepartialview').hide();
    $('#listTable').show();
}

function ForwardSendBackApproveSeatMatrix() {
    $('#ReviewStatusErr').text('');
    $('#ReviewusersErr').text('');
    let year = $("#ddlAcademicYearReview").val();
    let course = $("#courseId").val().toString();
    let round = $("#roundId").val();
    let Status = $("#ReviewStatus").val();
    let remarks = $("#reviewRemarks").val();
    let role = $("#Reviewusers").val();
    if (Status == 'choose') {
        $('#ReviewStatusErr').text('select the status');
    } else if (Status == 2) {
        $.ajax({
            type: 'GET',
            url: "/Admission/ForwardSendBackApproveSeatMatrix",
            data: { 'round': round, 'year': year, 'remarks': remarks, 'courseType': course, 'Status': Status, 'roleId': 0 },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == true) {
                    bootbox.alert('seat matrix Approved and published successfully');
                    cancelReview();
                    ReviewSeatMatrix();
                }
                else {
                    bootbox.alert("Error", "something went wrong");
                }
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else if (role == 'choose') {
        $('#ReviewusersErr').text('select the role');
    }
    else {
        $.ajax({
            type: 'GET',
            url: "/Admission/ForwardSendBackApproveSeatMatrix",
            data: { 'round': round, 'year': year, 'remarks': remarks, 'courseType': course, 'Status': Status, 'roleId': role },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == true) {
                    if (Status == 7) {
                        bootbox.alert('seat matrix data forwarded successfully');
                        cancelReview();
                        ReviewSeatMatrix();
                    }
                    else if (Status == 3) {
                        bootbox.alert('seat matrix data is Rejected successfully');
                        cancelReview();
                        ReviewSeatMatrix();
                    }
                }
                else {
                    bootbox.alert("Error", "something went wrong");
                }
            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}

function ReviewSeatMatrix() {
    var AcademicYearId = $('#ddlAcademicYearReview :selected').val();
    $.ajax({
        type: "GET",
        url: "/Admission/GetUpdateSeatMatrix",
        data: { AcademicYearId: AcademicYearId },
        contentType: "application/json",
        success: function (data) {
            $('#tblReviewseatmatrix').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'slno', 'title': 'Slno', 'className': 'text-center slno' },
                    { 'data': 'coursetypename', 'title': 'Course Type' },
                    { 'data': 'ApplicantTypeDdl', 'title': 'Applicant Type', 'className': 'text-center ApplicantTypeDdl' },
                    { 'data': 'Round', 'title': 'Round', 'className': 'text-center Round' },
                    { 'data': 'StatusName', 'title': 'Status - Currently with', 'className': 'text-left StatusName' },
                    {
                        'data': 'SeatMaxId',
                        'title': 'Remarks',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewRremarks(" + oData.ApplicantType + "," +
                                oData.Round + "," + oData.courseid + "," + 1 + ")' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    },
                    {
                        'data': 'SeatMaxId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='ViewCollegeWiseSeatmatrixData(" +
                                oData.ApplicantType + "," + oData.Round + "," + oData.FlowId + "," +
                                oData.RoleId + "," + oData.courseid + "," + oData.StatusId + " )' class='btn btn-primary btn-xs' value='View' id='View'/>");
                        }
                    }
                ],
            });
        }
    });
}

function ViewCollegeWiseSeatmatrixData(applId, round, flowId, roleId, courseid, StatusId) {
    $('#TablereviewGrid').hide();
    $('#ReviewStatus').val('choose');
    var year = $('#ddlAcademicYearReview').val();
    $.ajax({
        type: "GET",
        url: "/Admission/GetDivisionsInstituteTradesUpdate",
        data: { 'year': year, 'round': round, 'applicantType': applId, 'courseId': courseid },
        contentType: "application/json",
        success: function (data) {
            if (data != null)
                $('#ReviewDiv').html(data);

            $('#reviewGrid').show();
            $('#courseId').val(courseid);
            $("#roundId").val(round);
            if (roleId == flowId && StatusId!=2) {
                $('#SendDiv').show();
                $('#btnBack').hide();
            }
            else {
                $('#SendDiv').hide();
                $('#btnBack').show();
            }

        }
    });
}
function ViewRremarks(applId, round, courseid,crudId) {
    $('#RemarksCommentsModal').modal('show');
    if (crudId==1)
        var year = $('#ddlAcademicYearReview').val();
    else
        var year = $('#ddlAcademicYearUpdate').val();
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatmatrixRemarks",
        data: { 'year': year, 'round': round, 'applId': applId, 'courseType': courseid },
        contentType: "application/json",
        success: function (data) {
            $('#CommentsTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.no', 'className': 'text-center' },
                    { 'data': 'Date', 'title': 'Date', 'className': 'text-center' },
                    { 'data': 'From', 'title': 'From', 'className': 'text-center' },
                    { 'data': 'To', 'title': 'To', 'className': 'text-center' },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-center' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' }
                ],
            });
        }

    });
}
function cancelReview() {
    $('#reviewGrid').hide();
    $('#TablereviewGrid').show();
}
//end generate seat matrix dhanraj ===================================

function GetInstitutes(city, distid) {
    var lgdId = $('#' + distid).val();
    GetColleges(city, lgdId);
}
function Approvebtn(statuId) {
    var id = $('#' + statuId).val();
    //$('#Reviewusers').empty();
    
    if (id == 2) {
        $('#Reviewusers').val('choose');
        $('#Reviewusers').attr('disabled', true);
    }
    else if (id == 9) {
        $('#Reviewusers').empty();
        //$('#Reviewusers').attr('disabled', false);
        $('#Reviewusers').removeAttr('disabled');
        $('#Reviewusers').append('<option value="choose">choose</option>');
        $('#Reviewusers').append('<option value="5">Deputy Director</option>');
    }
    else {
        $('#Reviewusers').removeAttr('disabled');
        Getusers('Reviewusers', 1);
    }
}

function EachInputCount(instituteId) {
    var $tr = $('#' + 'tr_' + instituteId);
    let round = parseInt($tr.find("td:eq(3) input").val());
    var vall = parseFloat(0);
    if (round == 1 || round == 2) {
        for (i = 5; i <= 100; i++) {
            vall = vall + parseFloat($tr.find("td:eq(" + i + ") input").val());
        }
    }
    else {
        for (i = 5; i <= 12; i++) {
            vall = vall + parseFloat($tr.find("td:eq(" + i + ") input").val());
        }
    }
    $tr.find("td:eq(4)").text(vall.toFixed(2));

}