const status = {
    Approved: 2,
    SendBackForCorrection: 4,
    SubmittedForReview: 5,
    ReviewedAndRecommend: 7,
    SentBackForCorrection: 9
}

const TraineeType = {
    Regular: 1,
    Private: 2
}

const ApplicantTypes = {
    Direct : 3
}

const DeptConst = {
    Admission : 1
}

const loginUserRole = {
    DD: 5,
    JD: 4,
    AD: 3, // Assistant Director
    ADL: 6, // Additional Director
    OS: 7,
    Director: 2,
    Commissioner: 1,
    CW: 8,
    ITIAdmin: 9,
    VO: 12,
    JDDiv: 16,
    AO: 18,
    DDText: 'Deputy Director',
    DDAdmCellText: 'Admission cell, commissionrate',
}

const AdmittedOrRejected = {
    Admitted: 6,
    Rejected: 3
}

const AdmFeePaidStatusConst = {
    Paid: 1,
    PaidText: 'Paid',
    UnPaidText: 'UnPaid'
}

const CategoryConst = {
    General: 1,
}

function GetVerticalCategory(VerticalCategory) {

    $("#" + VerticalCategory).empty();
    $.ajax({
        url: "/Admission/GetHorizontalVerticalCategory",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + VerticalCategory).append($("<option/>").val(this.YearID).text(this.Year));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetHorizontalCategory(HorizontalCategory) {

    $("#" + HorizontalCategory).empty();
    $("#" + HorizontalCategory).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetHorizontalVerticalCategory",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + HorizontalCategory).append($("<option/>").val(this.Horizontal_rules_id).text(this.Horizontal_rules));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetVerticalCategory(VerticaCategory) {

    $("#" + VerticaCategory).empty();
    $("#" + VerticaCategory).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetVerticalCategory",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + VerticaCategory).append($("<option/>").val(this.Vertical_rules_id).text(this.Vertical_Rules));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetSessionYear(SessionYear) {

    var currYear = (new Date).getFullYear();
    $("#" + SessionYear).empty();
    $("#" + SessionYear).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetYearType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + SessionYear).append($("<option/>").val(this.YearID).text(this.Year));
                    $("select option:contains(" + currYear + ")").attr('selected', true);
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetCourses(course) {
    $("#" + course).empty();
    //$('#CourseTypes').val("NCVT");
    //$("#" + course).append('<option value="choose">Choose</option>');   
    
    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {         
                    $("#" + course).append($("<option/>").val(this.CourseId).text(this.CourseTypeName));                   
                });               
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetApplicantType(type) {

    $("#" + type).empty();
    var urlPath = "";
    if (type == "TraineeType") {
        urlPath = "/Admission/GetTraineeType";
    }
    else {
        urlPath = "/Admission/GetApplicantType";
        $("#" + type).append('<option value="choose">Choose</option>');
    }

    $.ajax({
        url: urlPath,
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (this.ApplicantTypeId != 3 || type.startsWith("ApplicantType")) {
                        $("#" + type).append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    }
                });
                if (type == "TraineeType") {
                    $('#' + type).val(TraineeType.Regular);
                }
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetApplicationMode(type) {
    
    $("#" + type).empty();
    var urlPath = "";
    urlPath = "/Admission/GetApplicationMode";
        $("#" + type).append('<option value="choose">Choose</option>');
    $.ajax({
        url: urlPath,
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    /*if (this.ApplicantTypeId != 3 || type.startsWith("ApplicantType")) {*/
                    $("#" + type).append($("<option/>").val(this.ApplicationModeId).text(this.ApplicationMode));
                   // }
                });
                //if (type == "TraineeType") {
                //    $('#' + type).val(TraineeType.Regular);
                //}
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}


function GetAdmissionRounds(AdmissionRounds) {
    var leadChars = "", trialChars = "";
    var ApplType = "ApplicantType";
    var roundString = "Round";
    if (String(AdmissionRounds) == ApplType || String(AdmissionRounds).includes(ApplType + "A") || String(AdmissionRounds).includes(ApplType + "R")) {
        roundString = roundString + "Option";
    }

    var ApplicantType = $("#" + AdmissionRounds).val();
    if (!String(AdmissionRounds).includes(roundString)) {
        trialChars = AdmissionRounds.substring(0, AdmissionRounds.indexOf(ApplType));
        leadChars = AdmissionRounds.substring(trialChars.length + ApplType.length, AdmissionRounds.length);
        AdmissionRounds = trialChars + roundString + leadChars
    }

    $("#" + AdmissionRounds).prop('disabled', false);
    if (ApplicantType == ApplicantTypes.Direct) {
        $("#" + AdmissionRounds).attr('disabled', true);
    }

    $("#" + AdmissionRounds).empty();
    $("#" + AdmissionRounds).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetAdmissionRounds",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    if (ApplicantType == "1") {
                        if (this.ApplicantAdmissionRoundsId <= 3) {
                            $("#" + AdmissionRounds).append($("<option/>").val(this.ApplicantAdmissionRoundsId).text(this.RoundList));
                        }
                    }
                    else if (ApplicantType == "2") {
                        if (this.ApplicantAdmissionRoundsId > 3) {
                            $("#" + AdmissionRounds).append($("<option/>").val(this.ApplicantAdmissionRoundsId).text(this.RoundList));
                        }
                    }
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function Getusers(user, id) {
    $("#" + user).empty();
    $("#" + user).append('<option value="0">Choose</option>');
    $.ajax({
        url: "/Admission/GetRoles",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'level': id },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }
            if (user == 'Reviewusers' && id == 4) {
                $("#" + user +" option[value !='" + loginUserRole.DD + "']").hide();
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
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

function GetDivisionsDDp(division) {
    $("#" + division).empty();
    $("#" + division).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetDivision",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + division).append($("<option/>").val(this.division_id).text(this.division_name));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetDistrictsDDp(dist, divisionId) {
    
    $("#" + dist).empty();
    $("#" + dist).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetDistrict",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'Divisions': divisionId},
        success: function (data) {
            if (data != null || data != '') {
              $.each(data, function () {
                if (divisionId == 0) {
                  $("#" + dist).empty();
                  $("#" + dist).append('<option value="choose">Choose</option>');
                }
                else {
                  $("#" + dist).append($("<option/>").val(this.district_id).text(this.district_ename));
                
                }
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetTaluksDDp(taluk, distId) {
    $("#" + taluk).empty();
    $("#" + taluk).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetCitiTaluks",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'distilgdCOde': distId },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + taluk).append($("<option/>").val(this.CityId).text(this.CityName));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetTrades(trades) {
    $("#" + trades).empty();
    $("#" + trades).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetTrades",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',       
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + trades).append($("<option/>").val(this.TradeId).text(this.TradeName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetForwardRoles(role,level) {
    $("#" + role).empty();
    $("#" + role).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetRoles",
        type: 'Get',
        data: { 'level': level},
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + role).append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetStatus(status) {
    $("#" + status).empty();
    $("#" + status).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetStatus",
        type: 'Get',        
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + status).append($("<option/>").val(this.StatusId).text(this.StatusName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetColleges(city, lgdCode) {
    
    $("#" + city).empty();
    $("#" + city).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetInstitutes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'distilgdCOde': lgdCode},
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + city).append($("<option/>").val(this.CityId).text(this.CityName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetShiftsDetails(ShiftsId) {
    $("#" + ShiftsId).empty();
    $("#" + ShiftsId).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetShiftsDetails",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + ShiftsId).append($("<option/>").val(this.s_id).text(this.shifts));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetUnitsDetails(UnitsId) {
    $("#" + UnitsId).empty();
    $("#" + UnitsId).append('<option value="choose">Choose</option>');
    $.ajax({
        url: "/Admission/GetUnitsDetails",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + UnitsId).append($("<option/>").val(this.u_id).text(this.units));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetRoles(user, id) {
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetUserRoles",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        data: { 'level': id },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetInstType(type) {
    $("#" + type).empty();
    $("#" + type).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetInstTypeDetails",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + type).append($("<option/>").val(this.Institute_type_id).text(this.Institutetype));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function fnSetDTExcelBtnPos() {
    return "<'row'<'col-sm-4'l><'col-sm-3 text-center'B><'col-sm-5'f>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-5'i><'col-sm-7'p>>";
}

function fnGetYesNoForVal(data) {
    $.each(data, function () {
        this.AdmFeePaidStatus = this.AdmFeePaidStatus == 1 ? "Yes" : "No";
        this.MinorityCategory = this.MinorityCategory == 1 ? "Yes" : "No";
    });
}

function GetStateList(state) {
    $("#" + state).empty();
    $("#" + state).append('<option value="select">Select</option>');

    $.ajax({
        url: "/Admission/GetStateListDetails",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + state).append($("<option/>").val(this.stateId).text(this.NameOfState));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}


function GetSeatTypes(seatType) {
    $("#" + seatType).empty();
    $("#" + seatType).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetSeatTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + seatType).append($("<option/>").val(this.SeatTypeId).text(this.SeatTypeName));
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}