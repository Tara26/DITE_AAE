$(document).ready(function () {
    //GetViewAdmissionRegister();
    $("#SessionYearErr").hide();
    $("#CourseTypeErr").hide();
    $("#divisionErr").hide();
    $("#districtErr").hide();
    $("#ITIInstituteErr").hide();
    $("#ApplicantTypeErr").hide();
    $("#RoundErr").hide();
    $("#SessionYearJDErr").hide();
    $("#CourseTypeJDErr").hide(); 
    $("#districtJDErr").hide();
    $("#ITIInstituteJDErr").hide();
    $("#ApplicantTypeJDErr").hide();
    $("#RoundJDErr").hide();

    $('.nav-tabs li:eq(0) a').tab('show');
    //GetAcademicYear();
    GetCourseTypes();
    //GetApplicantTypeNew();
    GetDivision();
   // GetInstituteType();
    //GetDistrictJD();
     //GetRoundlist();
    //GetAdmissionRegister();
    //GetAdmissionRegisterJD();
    
    //Logic Change-05/06/2021
    ////GetAcademicYear("Session2");
    ///GetAcademicYear("Session");
    GetSessionYear("Session");
    GetSessionYear("Session2");

    GetCourses("CourseType2");
    GetCourses("ddlCourseType");

    GetDivisionsDD("divisionDdp");
    GetFilterData("districtDdp");
    GetFilterData("ddlDistrictJD");
    GetApplicantType("dllApplicantTypeNew");
    GetApplicantType("dllApplicantTypeDD");
    GetInstTypeJD_DD("instTypes");
    GetInstTypeJD_DD("instTypesJd");
    //$("#divtblAdmReg").hide();    
    GetAdmissionRegisterDemo();
});


function GetCourseTypes() {
    $("#ddlCourseTypes").empty();
    $("#ddlCourseTypes").append('<option value="0">choose</option>');
    $("#ddlCourseTypesJD").empty();
    $("#ddlCourseTypesJD").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlCourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    $("#ddlCourseTypesJD").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetApplicantTypeNew() {
    
    $("#dllApplicantType").empty();
    $("#dllApplicantType").append('<option value="0">choose</option>');
    $("#dllApplicantTypeJD").empty();
    $("#dllApplicantTypeJD").append('<option value="0">choose</option>');
    $("#dllApplicantTypeNew").empty();
    $("#dllApplicantTypeNew").append('<option value="0">choose</option>');

    $.ajax({
        url: "/Admission/GetApplicantType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#dllApplicantType").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    $("#dllApplicantTypeJD").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                    $("#dllApplicantTypeNew").append($("<option/>").val(this.ApplicantTypeId).text(this.ApplicantTypeDdl));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
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
}
function GetInstituteType() {
    var District = $('#ddlDistrict :selected').val();
    if (District != "" && District != null) {
        $("#ddlInstitute").empty();
        $("#ddlInstitute").append('<option value="0">choose</option>');
        $("#ddlInstituteJD").empty();
        $("#ddlInstituteJD").append('<option value="0">choose</option>');
        $.ajax({
            url: "/Admission/GetInstitutesReg",
            type: 'Get',
            data: { District: District },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#ddlInstitute").append($("<option/>").val(this.CityId).text(this.CityName));
                        $("#ddlInstituteJD").append($("<option/>").val(this.Institute_typeId).text(this.InstituteType));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }  
}
function ChangeInstituteJD() {
    var District = $('#ddlDistrictJD :selected').val();
    if (District != "" && District != null) {       
        $("#ddlInstituteJD").empty();
        $("#ddlInstituteJD").append('<option value="0">choose</option>');
        $.ajax({
            url: "/Admission/GetInstitutesReg",
            type: 'Get',
            data: { District: District },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#ddlInstituteJD").append($("<option/>").val(this.CityId).text(this.CityName));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }  
}
function GetDistrictJD() {
    $("#ddlDistrictJD").empty();
    $("#ddlDistrictJD").append('<option value="0">choose</option>');  
    $.ajax({
        url: "/Admission/GetDistrictsReg",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ddlDistrictJD").append($("<option/>").val(this.district_id).text(this.district_ename));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

//Logic changed -05/08/2021
function getfilter2(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.search(valu).draw();
    }
    else if (id == 2) {
        GetTaluksData('ddlDistrictJD', 'ddlInstituteJD', '1');
        if ($('#ddlDistrictJD option:selected').text() == 'choose')
            valu = $('#ddlDistrictJD option:selected').text();
        //table.search(valu).draw();
        table.columns(5).search(valu).draw();
    }
    else if (id == 3) {
        GetTaluksData('ddlInstituteJD', '2');
        if ($('#ddlInstituteJD option:selected').text() == 'choose')
            valu = $('#ddlInstituteJD option:selected').text();
        table.search(valu).draw();
        //table.columns(8).search(valu).draw();
    }
    //////else if (id == 3) {
    //////    GetTaluksData('talukDdpJD', 'ddlInstituteJD', '2');
    //////    if ($('#talukDdpJD option:selected').text() == 'choose')
    //////        valu = $('#talukDdpJD option:selected').text();
    //////    table.search(valu).draw();
    //////}
    //////else {
    //////    if ($('#ddlInstituteJD option:selected').text() == 'choose')
    //////        valu = $('#ddlDistrictJD option:selected').text();
    //////    table.search(valu).draw();
    //////}
}
function GetFilterData(ddpId) {
    $("#" + ddpId).empty();
    $("#" + ddpId).append('<option value="choose">ALL</option>');
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
function getfilter(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.search(valu).draw();
    }
    if (id == 1) {
        GetDistrictData('divisionDdp', 'districtDdp1', '1');
        if ($('#divisionDdp option:selected').text() == 'Choose')
            valu = '';
        table.search(valu).draw();
    }
    else if (id == 2) {
        GetTaluksData('districtDdp1', 'instituteDdp', '2');
        if ($('#districtDdp1 option:selected').text() == 'choose')
            //valu = $('#divisionDdp option:selected').text();
            valu = '';
        table.search(valu).draw();
    }
    else if (id == 3) {
       // GetTaluksData('instituteDdp', '2');
        if ($('#instituteDdp option:selected').text() == 'choose')
            //valu = $('#divisionDdp option:selected').text();
            valu = '';
        table.search(valu).draw();
    }
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
function GetTaluksData(ddpId, distddp, cityId) {
    var lgdCode = $("#" + ddpId).val();
    if (lgdCode == 'choose') {
        console.log('select district');
    }
    else {
        //$("#" + tlk).empty();
        //$("#" + tlk).append('<option value="choose">choose</option>');
        $('#' + distddp).empty();
        $('#' + distddp).append('<option value="choose">choose</option>');
        //$("#instituteDdp").empty();
        if (distddp != "instituteDdp") {
           /* $("#talukDdp").append('<option value="choose">choose</option>');*/
            $("#instituteDdp").append('<option value="choose">choose</option>');
        }
        //else
        //    $("#" + tlk).append('<option value="choose">choose</option>');

        var Url = "/Admission/GetCitiTaluks";
        if (cityId == 1) {
        /*Url = "/Admission/GetCitiTaluks";*/
            Url = "/Admission/GetInstitutesReg";
        }
        else {
            Url = "/Admission/GetInstitutesReg";
        }
        $.ajax({
            url: Url,
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { "District": lgdCode },
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + distddp).append($("<option/>").val(this.CityId).text(this.CityName));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}
function GetAdmissionRoundsRegDD() {
    var applicantType = $('#dllApplicantTypeDD :selected').val();
    if (applicantType != "" && applicantType != null) {
        $("#dllRoundDD").empty();
        $("#dllRoundDD").append('<option value="0">choose</option>');

        $.ajax({
            url: "/Admission/GetAppRoundReg",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { applicantType: applicantType },
            success: function (data) {

                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#dllRoundDD").append($("<option/>").val(this.ApplicantAdmissionRoundsId).text(this.RoundList));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#dllRoundDD").empty();
        $("#dllRoundDD").append('<option value="">Select</option>');
    }
}
function GetAcademicYear(SessionYear) {
    $("#" + SessionYear).empty();
    //$("#" + SessionYear).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetYearType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + SessionYear).append($("<option/>").val(this.YearID).text(this.Year));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function getfilter3(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.search(valu).draw();
    }
    if (id == 1) {
        GetAdmissionRoundsRegDD('dllApplicantTypeDD', 'dllRoundDD', '1');
        if ($('#dllApplicantTypeDD option:selected').text() == 'Choose')
            valu = '';
        table.search(valu).draw();
    }
    else {
        if ($('#dllRoundDD option:selected').text() == 'choose')
            valu = $('#dllRoundDD option:selected').text();
        table.search(valu).draw();
    }
}
function getfilter4(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.search(valu).draw();
    }
    if (id == 1) {
        GetRoundlist('dllApplicantTypeNew', 'dllRoundDD', '1');
        if ($('#dllApplicantTypeNew option:selected').text() == 'Choose')
            valu = '';
        table.search(valu).draw();
    }
    else {
        if ($('#dllRound option:selected').text() == 'choose')
            valu = $('#dllRound option:selected').text();
        table.search(valu).draw();
    }
}

function GetAdmissionRegisterDemo() {
    var coursetype = $('#CourseType2 :selected').val();
    var session = $('#Session :selected').val();
    var division = $('#divisionDdp :selected').val();
    var district = $('#districtDdp1 :selected').val();
    var Institute = $('#instituteDdp :selected').val();
    var InstType = $('#instTypes :selected').val();
  
        $.ajax({
            type: "GET",
        url: "/Admission/GetViewAdmissionRegisterDemo",
        dataType: 'json',
        data: { coursetype: coursetype, session: session, division: division, district: district, Institute: Institute, InstType: InstType},
        contentType: 'application/json; charset=utf-8',
            success: function (data) {
            $('#tblviewAdmissionRegister').DataTable({
                    data: data,
                searching: false,
                "bFilter": false,
                    "destroy": true,
                    "bSort": true,                   
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'excel',
                            text: 'Download as Excel',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 26, 27, 28, 30, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41]
                            }
                        }
                    ],

                    columns: [
                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
                        { 'data': 'ApplyYear', 'title': 'Session ', 'className': 'text-left ApplyYear' },
                        { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-center StateRegistrationNumber' },
                        { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left MISCode' },
                        { 'data': 'divisionname', 'title': 'Division Name', 'className': 'text-left divisionname' },
                        { 'data': 'districtename', 'title': 'District Name', 'className': 'text-left districtename' },
                        { 'data': 'talukename', 'title': 'Taluk Name', 'className': 'text-left talukename' },
                        { 'data': 'Institutetype', 'title': 'Institute Type', 'className': 'text-left Institutetype' },
                        { 'data': 'iticollegename', 'title': 'Institute Name', 'className': 'text-left iticollegename' },
                        { 'data': 'AdmisionDateTime', 'title': 'Admission Date', 'className': 'text-left AdmisionDateTime' },
                        { 'data': 'TraineeName', 'title': 'Trainee Name', 'className': 'text-left TraineeName' },
                        { 'data': 'PhoneNumber', 'title': 'Mobile Number', 'className': 'text-left PhoneNumber' },
                        { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left EmailId' },
                        { 'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left dateofbirth' },
                        { 'data': 'Gendername', 'title': 'Gender', 'className': 'text-left Gendername' },
                        { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left CategoryName' },
                        { 'data': 'CasteCategory', 'title': 'Category ', 'className': 'text-left Total' },
                        { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                        { 'data': 'DisabilityName', 'title': 'PWD category', 'className': 'text-left DisabilityName' },
                        { 'data': 'ExService', 'title': 'Whether belong to ex-Servicemen', 'className': 'text-left ExService' },

                        { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                        { 'data': 'FathersName', 'title': 'Father/Guardian Name', 'className': 'text-left FathersName' },
                        //{ 'data': 'FileName', 'title': 'Father/Guardian Income Certificate in case of OBC/General/Minorities)', 'className': 'text-left Rank' },
                        { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left MotherName' },

                        { 'data': 'AccountNumber', 'title': 'Bank Account', 'className': 'text-left AccountNumber' },
                        { 'data': 'IFSCCode', 'title': 'IFSC Code', 'className': 'text-left IFSCCode' },
                        { 'data': 'tradename', 'title': 'Trade', 'className': 'text-left tradename' },
                        { 'data': 'shifts', 'title': 'Shift', 'className': 'text-left shifts' },
                        { 'data': 'units', 'title': 'Unit', 'className': 'text-left units' },
                        //{ 'data': 'AadhaarNumber', 'title': 'UID Number', 'className': 'text-left AadhaarNumber' },
                        { 'data': 'Qualify', 'title': 'Highest Qualification', 'className': 'text-left Qualify' },

                        //{ 'data': 'ITIUnderPPP', 'title': 'Is the ITI under PPP', 'className': 'text-left ITIUnderPPP' },
                        { 'data': 'RollNumber', 'title': 'SSLC Reg Number', 'className': 'text-left RollNumber' },
                        { 'data': 'MaxMarks', 'title': 'Maximum Marks', 'className': 'text-left MaxMarks' },
                        { 'data': 'MinMarks', 'title': 'Obtained Marks', 'className': 'text-left MotherName' },
                        { 'data': 'Minority', 'title': 'Minorities (Y/N)', 'className': 'text-left Minority' },

                        { 'data': 'Feestatus', 'title': 'Fees Paid (Y/N)', 'className': 'text-left Feestatus' },
                        { 'data': 'AdmisionFee', 'title': 'Amount Rs.', 'className': 'text-left AdmisionFee' },
                        //{ 'data': 'ReceiptNumber', 'title': 'Receipt No & Date', 'className': 'text-left ReceiptNumber' },
                        {
                            data: { ReceiptNumber: 'ReceiptNumber', PaymentDateReceipt: 'PaymentDateReceipt' },
                            'title': 'Receipt No & Date',
                            mRender: function (data, type, full) {
                                return data.ReceiptNumber + '- ' + data.PaymentDateReceipt + ' ';
                            }
                        },
                        { 'data': 'Traineeisindual', 'title': 'Is Trainee Dual Mode', 'className': 'text-left DualType' },
                        {
                            'data': 'TraineePhoto',
                            'title': 'Trainee Photo',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<img src='" + oData.TraineePhoto + "' class='pull-center' width='50' height='50'/>");
                            }
                        },

                        { 'data': 'Trainee', 'title': 'Trainee Type', 'className': 'text-left TraineeType' },
                        //{ 'data': 'Institutetype', 'title': 'Seat Type', 'className': 'text-left MotherName' },
                    ],
                });
            }
        });
    }
function btnsearchDD() {
    var session = $('#Session :selected').val();
    var coursetype = $('#CourseType2 :selected').val();
    var division = $('#divisionDdp :selected').val();
    var district = $('#districtDdp1 :selected').val();
    var Institute = $('#instituteDdp :selected').val();

    var IsValid = true;
    if (session == "" || session == "0" || session == null) {
        IsValid = false;
        $("#divtblAdmReg").hide();
    }
    else {
        $("#SessionYearErr").hide();
    }
    if (coursetype == "" || coursetype == "0" || coursetype == null) {
        IsValid = false;
        $("#divtblAdmReg").hide();
            }
            else {
        $("#CourseTypeErr").hide();
    }
    if (IsValid == true) {
        $("#divtblAdmReg").show();
        GetAdmissionRegisterDemo();
        //GetAdmissionRegister();
    }
}

function GetAdmissionRegisterJD() {
    var session = $('#Session2 :selected').val();
    var coursetype = $('#ddlCourseType :selected').val();
    var district = $('#ddlDistrictJD :selected').val();
    var Institute = $('#ddlInstituteJD :selected').val();
    var InstType = $('#instTypesJd :selected').val();

                $.ajax({
                    type: "GET",
        url: "/Admission/GetViewAdmissionRegisterDemo",
        dataType: 'json',
        data: { coursetype: coursetype, session: session,district: district, Institute: Institute, InstType: InstType },
        contentType: 'application/json; charset=utf-8',
                    success: function (data) {
            $('#tblviewAdmissionRegister').DataTable({
                            data: data,
                searching: false,
                "bFilter": false,
                            "destroy": true,
                            "bSort": true,
                            dom: 'Bfrtip',
                            buttons: [
                                {
                                    extend: 'excel',
                                    text: 'Download as Excel',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 26, 27, 28, 30, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41]
                                    }
                                }
                            ],

                            columns: [
                                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
                                { 'data': 'ApplyYear', 'title': 'Session ', 'className': 'text-left ApplyYear' },
                                { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-center StateRegistrationNumber' },
                                { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left MISCode' },
                                { 'data': 'divisionname', 'title': 'Division Name', 'className': 'text-left divisionname' },
                                { 'data': 'districtename', 'title': 'District Name', 'className': 'text-left districtename' },
                                { 'data': 'talukename', 'title': 'Taluk Name', 'className': 'text-left talukename' },
                                { 'data': 'Institutetype', 'title': 'Institute Type', 'className': 'text-left Institutetype' },
                                { 'data': 'iticollegename', 'title': 'Institute Name', 'className': 'text-left iticollegename' },
                                { 'data': 'AdmisionDateTime', 'title': 'Admission Date', 'className': 'text-left AdmisionDateTime' },
                                { 'data': 'TraineeName', 'title': 'Trainee Name', 'className': 'text-left TraineeName' },
                                { 'data': 'PhoneNumber', 'title': 'Mobile Number', 'className': 'text-left PhoneNumber' },
                                { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left EmailId' },
                                { 'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left dateofbirth' },
                                { 'data': 'Gendername', 'title': 'Gender', 'className': 'text-left Gendername' },
                                { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left CategoryName' },
                              { 'data': 'CasteCategory', 'title': 'Category ', 'className': 'text-left Total' },
                                { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                                { 'data': 'DisabilityName', 'title': 'PWD category', 'className': 'text-left DisabilityName' },
                                { 'data': 'ExService', 'title': 'Whether belong to ex-Servicemen', 'className': 'text-left ExService' },

                                { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                                { 'data': 'FathersName', 'title': 'Father/Guardian Name', 'className': 'text-left FathersName' },
                                //{ 'data': 'DocTypeIncome', 'title': 'Father/Guardian Income Certificate in case of OBC/General/Minorities)', 'className': 'text-left Rank' },
                                { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left MotherName' },

                                { 'data': 'AccountNumber', 'title': 'Bank Account', 'className': 'text-left AccountNumber' },
                                { 'data': 'IFSCCode', 'title': 'IFSC Code', 'className': 'text-left IFSCCode' },
                                { 'data': 'tradename', 'title': 'Trade', 'className': 'text-left tradename' },
                                { 'data': 'shifts', 'title': 'Shift', 'className': 'text-left shifts' },
                                { 'data': 'units', 'title': 'Unit', 'className': 'text-left units' },
                                //{ 'data': 'AadhaarNumber', 'title': 'UID Number', 'className': 'text-left AadhaarNumber' },
                                { 'data': 'Qualify', 'title': 'Highest Qualification', 'className': 'text-left Qualify' },

                                //{ 'data': 'ITIUnderPPP', 'title': 'Is the ITI under PPP', 'className': 'text-left ITIUnderPPP' },
                                { 'data': 'RollNumber', 'title': 'SSLC Reg Number', 'className': 'text-left RollNumber' },
                                { 'data': 'MaxMarks', 'title': 'Maximum Marks', 'className': 'text-left MaxMarks' },
                                { 'data': 'MinMarks', 'title': 'Obtained Marks', 'className': 'text-left MotherName' },
                                { 'data': 'Minority', 'title': 'Minorities (Y/N)', 'className': 'text-left Minority' },

                                { 'data': 'Feestatus', 'title': 'Fees Paid (Y/N)', 'className': 'text-left Feestatus' },
                                { 'data': 'AdmisionFee', 'title': 'Amount Rs.', 'className': 'text-left AdmisionFee' },
                                //{ 'data': 'ReceiptNumber', 'title': 'Receipt No & Date', 'className': 'text-left ReceiptNumber' },
                                {
                                    data: { ReceiptNumber: 'ReceiptNumber', PaymentDateReceipt: 'PaymentDateReceipt' },
                                    'title': 'Receipt No & Date',
                                    mRender: function (data, type, full) {
                                        return data.ReceiptNumber + '- ' + data.PaymentDateReceipt + ' ';
                                    }
                                },
                                { 'data': 'Traineeisindual', 'title': 'Is Trainee Dual Mode', 'className': 'text-left DualType' },
                                {
                                    'data': 'TraineePhoto',
                                    'title': 'Trainee Photo',
                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                        $(nTd).html("<img src='" + oData.TraineePhoto + "' class='pull-center' width='50' height='50'/>");
                                    }
                                },

                                { 'data': 'Trainee', 'title': 'Trainee Type', 'className': 'text-left TraineeType' },
                                //{ 'data': 'Institutetype', 'title': 'Seat Type', 'className': 'text-left MotherName' },
                            ],
                        });
                    }
                });
            }
function btnsearchJD() {
    var session = $('#Session2 :selected').val();
    var coursetype = $('#ddlCourseType :selected').val();
    var district = $('#ddlDistrictJD :selected').val();
    var Institute = $('#ddlInstituteJD :selected').val();
    var InstType = $('#instTypesJd :selected').val();

    var IsValid = true;
    if (session == "" || session == "0" || session == null) {
        IsValid = false;
        $("#divtblAdmReg").hide();
    }
    else {
        $("#SessionYearJDErr").hide();
    }
    if (coursetype == "" || coursetype == "0" || coursetype == null) {
        IsValid = false;
        $("#divtblAdmReg").hide();
    }
    else {
        $("#CourseTypeJDErr").hide();
    }

    if (IsValid == true) {
        $("#divtblAdmReg").show();
        GetAdmissionRegisterJD();
    }

}


//**************//

function GetRoundlist() {

    var applicantType = $('#dllApplicantTypeNew :selected').val();
    if (applicantType != "" && applicantType != null) {
        $("#dllRound").empty();
        $("#dllRound").append('<option value="0">choose</option>');

        $.ajax({
            url: "/Admission/GetAppRoundReg",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            data: { applicantType: applicantType },
            success: function (data) {

                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#dllRound").append($("<option/>").val(this.ApplicantAdmissionRoundsId).text(this.RoundList));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#dllRound").empty();
        $("#dllRound").append('<option value="">Select</option>');
    }
}

function GetAdmissionRegister() {
    var coursetype = $('#ddlCourseTypes :selected').val();
    var session = $('#ddlSessionYear :selected').val();
    var division = $('#ddlDivision :selected').val();
    var district = $('#ddlDistrict :selected').val();
    var Institute = $('#ddlInstitute :selected').val();
    var applicantType = $('#dllApplicantType :selected').val();
    var round = $('#dllRound :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetAdmissionRegisterCon",
        dataType: 'json',
        data: { coursetype: coursetype, session: session, division: division, district: district, Institute: Institute, applicantType: applicantType, round: round},
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#tblAdmissionRegister').DataTable({
                data: data,
                searching: false,
                "bFilter": false,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42]
                        }
                    }
                ],

                columns: [
                    { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
                    { 'data': 'ApplyYear', 'title': 'Session ', 'className': 'text-left ApplyYear' },
                    { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-center StateRegistrationNumber' },
                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left MISCode' },
                    { 'data': 'divisionname', 'title': 'Division Name', 'className': 'text-left divisionname' },
                    { 'data': 'districtename', 'title': 'District Name', 'className': 'text-left districtename' },
                    { 'data': 'talukename', 'title': 'Taluk Name', 'className': 'text-left talukename' },
                    { 'data': 'Institutetype', 'title': 'ITI Type', 'className': 'text-left Institutetype' },
                    { 'data': 'iticollegename', 'title': 'ITI Name', 'className': 'text-left iticollegename' },
                    { 'data': 'AdmisionDateTime', 'title': 'Admission Date', 'className': 'text-left AdmisionDateTime' },
                    { 'data': 'TraineeName', 'title': 'Trainee Name', 'className': 'text-left TraineeName' },
                    { 'data': 'PhoneNumber', 'title': 'Mobile Number', 'className': 'text-left PhoneNumber' },
                    { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left EmailId' },
                    { 'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left dateofbirth' },
                    { 'data': 'Gendername', 'title': 'Gender', 'className': 'text-left Gendername' },
                    { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left CategoryName' },
                    { 'data': 'CasteCategory', 'title': 'Category ', 'className': 'text-left Total' },
                    { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
                    { 'data': 'DisabilityName', 'title': 'PWD category', 'className': 'text-left DisabilityName' },
                    { 'data': 'ExService', 'title': 'Whether belong to ex-Servicemen', 'className': 'text-left ExService' },

                    { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
                    { 'data': 'FathersName', 'title': 'Father/Guardian Name', 'className': 'text-left FathersName' },
                    { 'data': 'FileName', 'title': 'Father/Guardian Income Certificate in case of OBC/General/Minorities)', 'className': 'text-left Rank' },
                    { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left MotherName' },

                    { 'data': 'AccountNumber', 'title': 'Bank Account', 'className': 'text-left AccountNumber' },
                    { 'data': 'IFSCCode', 'title': 'IFSC Code', 'className': 'text-left IFSCCode' },
                    { 'data': 'tradename', 'title': 'Trade', 'className': 'text-left tradename' },
                    { 'data': 'shifts', 'title': 'Shift', 'className': 'text-left shifts' },
                    { 'data': 'units', 'title': 'Unit', 'className': 'text-left units' },
                    { 'data': 'AadhaarNumber', 'title': 'UID Number', 'className': 'text-left AadhaarNumber' },
                    { 'data': 'Qualify', 'title': 'Highest Qualification', 'className': 'text-left Qualify' },

                    { 'data': 'ITIUnderPPP', 'title': 'Is the ITI under PPP', 'className': 'text-left ITIUnderPPP' },
                    { 'data': 'RollNumber', 'title': 'SSLC Reg Number', 'className': 'text-left RollNumber' },
                    { 'data': 'MaxMarks', 'title': 'Maximum Marks', 'className': 'text-left MaxMarks' },
                    { 'data': 'MinMarks', 'title': 'Min Marks', 'className': 'text-left MotherName' },
                    { 'data': 'Minority', 'title': 'Minorities (Y/N)', 'className': 'text-left Minority' },

                    { 'data': 'Feestatus', 'title': 'Fees Paid (Y/N)', 'className': 'text-left Feestatus' },
                    { 'data': 'AdmisionFee', 'title': 'Amount Rs.', 'className': 'text-left AdmisionFee' },
                    //{ 'data': 'ReceiptNumber', 'title': 'Receipt No & Date', 'className': 'text-left ReceiptNumber' },
                    {
                        data: { ReceiptNumber: 'ReceiptNumber', PaymentDateReceipt: 'PaymentDateReceipt' },
                        'title': 'Receipt No & Date',
                        mRender: function (data, type, full) {
                            return data.ReceiptNumber + '- ' + data.PaymentDateReceipt + ' ';
                        }
                    },
                    { 'data': 'Traineeisindual', 'title': 'Is Trainee Dual Mode', 'className': 'text-left DualType' },                                   
                    {
                        'data': 'TraineePhoto',
                        'title': 'Trainee Photo',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<img src='" + oData.TraineePhoto + "' class='pull-center' width='50' height='50'/>");
                        }
                    },

                    { 'data': 'Trainee', 'title': 'Trainee Type', 'className': 'text-left TraineeType' },
                    { 'data': 'Institutetype', 'title': 'Seat Type', 'className': 'text-left MotherName' },
                ],
            });
        }
    });
}

  

//function GetAcademicYear() {
//    $("#ddlSessionYear").empty();
//    $("#ddlSessionYear").append('<option value="0">choose</option>');
//    $("#ddlSessionYearJD").empty();
//    $("#ddlSessionYearJD").append('<option value="0">choose</option>');
//    $.ajax({
//        url: "/Admission/GetAcademicYear",
//        type: 'Get',
//        contentType: 'application/json; charset=utf-8',
//        success: function (data) {

//            if (data != null || data != '') {
//                $.each(data, function () {
//                    $("#ddlSessionYear").append($("<option/>").val(this.Year).text(this.Year));
//                    $("#ddlSessionYearJD").append($("<option/>").val(this.Year).text(this.Year));
//                });
//            }
//        }, error: function (result) {
//            alert("Error", "something went wrong");
//        }
//    });
//}

//function GetViewAdmissionRegister() {

//    if (($('#CourseType2').val() == 'choose' || $('#CourseType2').val() == undefined) && ($('#Session2').val() == 'choose' || $('#Session2').val() == undefined)) {
//        var collegeId = 0;
//        var courseTyp = 0;
//        var year = 0;
//        var NotificationId = 0;
//        //var NotificationId = $('#dllApplicantTypeNew :selected').val();
//        $.ajax({
//            type: "GET",
//            url: "/Admission/GetViewAdmissionRegister",
//            data: { 'collegeId': collegeId, 'courseType': courseTyp, 'academicYear': year, 'NotificationId': NotificationId},
//            contentType: "application/json",
//            success: function (data) {
//                table = $('#tblviewAdmissionRegister').DataTable({
//                    data: data,
//                    "destroy": true,
//                    "bSort": true,                   
//                    dom: 'Bfrtip',
//                    buttons: [
//                        {
//                            extend: 'excel',
//                            text: 'Download as Excel',
//                            exportOptions: {
//                                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42]
//                            }
//                        }
//                    ],
//                    columns: [
//                        { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
//                        { 'data': 'ApplyYear', 'title': 'Session ', 'className': 'text-left ApplyYear' },
//                        { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-center StateRegistrationNumber' },
//                        { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left MISCode' },
//                        { 'data': 'divisionname', 'title': 'Division Name', 'className': 'text-left divisionname' },
//                        { 'data': 'districtename', 'title': 'District Name', 'className': 'text-left districtename' },
//                        { 'data': 'talukename', 'title': 'Taluk Name', 'className': 'text-left talukename' },
//                        { 'data': 'Institutetype', 'title': 'ITI Type', 'className': 'text-left Institutetype' },
//                        { 'data': 'iticollegename', 'title': 'ITI Name', 'className': 'text-left iticollegename' },
//                        { 'data': 'AdmisionDateTime', 'title': 'Admission Date', 'className': 'text-left AdmisionDateTime' },
//                        { 'data': 'TraineeName', 'title': 'Trainee Name', 'className': 'text-left TraineeName' },
//                        { 'data': 'PhoneNumber', 'title': 'Mobile Number', 'className': 'text-left PhoneNumber' },
//                        { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left EmailId' },
//                        { 'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left dateofbirth' },
//                        { 'data': 'Gendername', 'title': 'Gender', 'className': 'text-left Gendername' },
//                        { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left CategoryName' },
//                        { 'data': 'CasteCategory', 'title': 'Category ', 'className': 'text-left Total' },
//                        { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
//                        { 'data': 'DisabilityName', 'title': 'PWD category', 'className': 'text-left DisabilityName' },
//                        { 'data': 'ExService', 'title': 'Whether belong to ex-Servicemen', 'className': 'text-left ExService' },

//                        { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
//                        { 'data': 'FathersName', 'title': 'Father/Guardian Name', 'className': 'text-left FathersName' },
//                        { 'data': 'DocTypeIncome', 'title': 'Father/Guardian Income Certificate in case of OBC/General/Minorities)', 'className': 'text-left Rank' },
//                        { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left MotherName' },

//                        { 'data': 'AccountNumber', 'title': 'Bank Account', 'className': 'text-left AccountNumber' },
//                        { 'data': 'IFSCCode', 'title': 'IFSC Code', 'className': 'text-left IFSCCode' },
//                        { 'data': 'tradename', 'title': 'Trade', 'className': 'text-left tradename' },
//                        { 'data': 'shifts', 'title': 'Shift', 'className': 'text-left shifts' },
//                        { 'data': 'units', 'title': 'Unit', 'className': 'text-left units' },
//                        { 'data': 'AadhaarNumber', 'title': 'UID Number', 'className': 'text-left AadhaarNumber' },
//                        { 'data': 'Qualify', 'title': 'Highest Qualification', 'className': 'text-left Qualify' },

//                        { 'data': 'ITIUnderPPP', 'title': 'Is the ITI under PPP', 'className': 'text-left ITIUnderPPP' },
//                        { 'data': 'RollNumber', 'title': 'SSLC Reg Number', 'className': 'text-left RollNumber' },
//                        { 'data': 'MaxMarks', 'title': 'Maximum Marks', 'className': 'text-left MaxMarks' },
//                        { 'data': 'MinMarks', 'title': 'Min Marks', 'className': 'text-left MotherName' },
//                        { 'data': 'Minority', 'title': 'Minorities (Y/N)', 'className': 'text-left Minority' },

//                        { 'data': 'Feestatus', 'title': 'Fees Paid (Y/N)', 'className': 'text-left Feestatus' },
//                        { 'data': 'AdmisionFee', 'title': 'Amount Rs.', 'className': 'text-left AdmisionFee' },
//                        //{ 'data': 'ReceiptNumber', 'title': 'Receipt No & Date', 'className': 'text-left ReceiptNumber' },
//                        {
//                            data: { ReceiptNumber: 'ReceiptNumber', PaymentDateReceipt: 'PaymentDateReceipt' },
//                            'title': 'Receipt No & Date',
//                            mRender: function (data, type, full) {
//                                return data.ReceiptNumber + '- ' + data.PaymentDateReceipt + ' ';
    //}
//                        },
//                        { 'data': 'Traineeisindual', 'title': 'Is Trainee Dual Mode', 'className': 'text-left DualType' },
//                        {
//                            'data': 'TraineePhoto',
//                            'title': 'Trainee Photo',
//                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                                $(nTd).html("<img src='" + oData.TraineePhoto + "' class='pull-center' width='50' height='50'/>");
    //}
//                        },

//                        { 'data': 'Trainee', 'title': 'Trainee Type', 'className': 'text-left TraineeType' },
//                        { 'data': 'Institutetype', 'title': 'Seat Type', 'className': 'text-left MotherName' },
//                    ],
//                });
//            }, error: function (result) {
//                bootbox.alert("Error", "something went wrong");
    //}
//        });
    //}
    //else {
//        if ($('#CourseType2').val() == 'choose') {
//            bootbox.alert('Please select course Type');
//        } else
//            if ($('#Session2').val() == 'choose') {
//                bootbox.alert('Please select Session');
    //}
    //else {
//                //var collegeId = $('#instituteDdp').val();
//                //if (collegeId == undefined)
//                //    collegeId = 0;

//                var courseTyp = $('#CourseType2').val();
//                if (courseTyp == undefined)
//                    courseTyp = 0;

//                var year = $('#Session2').val();
//                if (year == undefined)
//                    year = 0;
//                $.ajax({
//                    type: "GET",
//                    url: "/Admission/GetViewAdmissionRegister",
//                    data: { 'collegeId': 0, 'courseType': courseTyp, 'academicYear': year },
//                    contentType: "application/json",
//                    success: function (data) {
//                        table = $('#tblviewAdmissionRegister').DataTable({
//                            data: data,
//                            "destroy": true,
//                            "bSort": true,
//                            dom: 'Bfrtip',
//                            buttons: [
//                                {
//                                    extend: 'excel',
//                                    text: 'Download as Excel',
//                                    exportOptions: {
//                                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42]
    //}
    //}
//                            ],
//                            columns: [
//                                { 'data': 'slno', 'title': 'Sl.No.', 'className': 'text-center slno' },
//                                { 'data': 'ApplyYear', 'title': 'Session ', 'className': 'text-left ApplyYear' },
//                                { 'data': 'StateRegistrationNumber', 'title': 'State Registration Number', 'className': 'text-center StateRegistrationNumber' },
//                                { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-left MISCode' },
//                                { 'data': 'divisionname', 'title': 'Division Name', 'className': 'text-left divisionname' },
//                                { 'data': 'districtename', 'title': 'District Name', 'className': 'text-left districtename' },
//                                { 'data': 'talukename', 'title': 'Taluk Name', 'className': 'text-left talukename' },
//                                { 'data': 'Institutetype', 'title': 'ITI Type', 'className': 'text-left Institutetype' },
//                                { 'data': 'iticollegename', 'title': 'ITI Name', 'className': 'text-left iticollegename' },
//                                { 'data': 'AdmisionDateTime', 'title': 'Admission Date', 'className': 'text-left AdmisionDateTime' },
//                                { 'data': 'TraineeName', 'title': 'Trainee Name', 'className': 'text-left TraineeName' },
//                                { 'data': 'PhoneNumber', 'title': 'Mobile Number', 'className': 'text-left PhoneNumber' },
//                                { 'data': 'EmailId', 'title': 'Email ID', 'className': 'text-left EmailId' },
//                                { 'data': 'dateofbirth', 'title': 'Date Of Birth', 'className': 'text-left dateofbirth' },
//                                { 'data': 'Gendername', 'title': 'Gender', 'className': 'text-left Gendername' },
//                                { 'data': 'CategoryName', 'title': 'Category', 'className': 'text-left CategoryName' },
//                                { 'data': 'CasteCategory', 'title': 'Category ', 'className': 'text-left Total' },
//                                { 'data': 'DiffrentAbled', 'title': 'Person with Disability', 'className': 'text-left DiffrentAbled' },
//                                { 'data': 'DisabilityName', 'title': 'PWD category', 'className': 'text-left DisabilityName' },
//                                { 'data': 'ExService', 'title': 'Whether belong to ex-Servicemen', 'className': 'text-left ExService' },

//                                { 'data': 'EconomicWeekerSec', 'title': 'Economic Weaker Section', 'className': 'text-left EconomicWeekerSec' },
//                                { 'data': 'FathersName', 'title': 'Father/Guardian Name', 'className': 'text-left FathersName' },
//                                { 'data': 'DocTypeIncome', 'title': 'Father/Guardian Income Certificate in case of OBC/General/Minorities)', 'className': 'text-left Rank' },
//                                { 'data': 'MothersName', 'title': 'Mother Name', 'className': 'text-left MotherName' },
       
//                                { 'data': 'AccountNumber', 'title': 'Bank Account', 'className': 'text-left AccountNumber' },
//                                { 'data': 'IFSCCode', 'title': 'IFSC Code', 'className': 'text-left IFSCCode' },
//                                { 'data': 'tradename', 'title': 'Trade', 'className': 'text-left tradename' },
//                                { 'data': 'shifts', 'title': 'Shift', 'className': 'text-left shifts' },
//                                { 'data': 'units', 'title': 'Unit', 'className': 'text-left units' },
//                                { 'data': 'AadhaarNumber', 'title': 'UID Number', 'className': 'text-left AadhaarNumber' },
//                                { 'data': 'Qualify', 'title': 'Highest Qualification', 'className': 'text-left Qualify' },
    
//                                { 'data': 'ITIUnderPPP', 'title': 'Is the ITI under PPP', 'className': 'text-left ITIUnderPPP' },
//                                { 'data': 'RollNumber', 'title': 'SSLC Reg Number', 'className': 'text-left RollNumber' },
//                                { 'data': 'MaxMarks', 'title': 'Maximum Marks', 'className': 'text-left MaxMarks' },
//                                { 'data': 'MinMarks', 'title': 'Min Marks', 'className': 'text-left MotherName' },
//                                { 'data': 'Minority', 'title': 'Minorities (Y/N)', 'className': 'text-left Minority' },

//                                { 'data': 'Feestatus', 'title': 'Fees Paid (Y/N)', 'className': 'text-left Feestatus' },
//                                { 'data': 'AdmisionFee', 'title': 'Amount Rs.', 'className': 'text-left AdmisionFee' },
//                                //{ 'data': 'ReceiptNumber', 'title': 'Receipt No & Date', 'className': 'text-left ReceiptNumber' },
//                                {
//                                    data: { ReceiptNumber: 'ReceiptNumber', PaymentDateReceipt: 'PaymentDateReceipt' },
//                                    'title': 'Receipt No & Date',
//                                    mRender: function (data, type, full) {
//                                        return data.ReceiptNumber + '- ' + data.PaymentDateReceipt + ' ';
//                                    }
//                                },
//                                { 'data': 'Traineeisindual', 'title': 'Is Trainee Dual Mode', 'className': 'text-left DualType' },
//                                {
//                                    'data': 'TraineePhoto',
//                                    'title': 'Trainee Photo',
//                                    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
//                                        $(nTd).html("<img src='" + oData.TraineePhoto + "' class='pull-center' width='50' height='50'/>");
//                                    }
//                                },

//                                { 'data': 'Trainee', 'title': 'Trainee Type', 'className': 'text-left TraineeType' },
//                                { 'data': 'Institutetype', 'title': 'Seat Type', 'className': 'text-left MotherName' },
//                            ],
//                });
//        }, error: function (result) {
//                        bootbox.alert("Error", "something went wrong");
//        }
//    });
//}
//    }
//}
function GetInstTypeJD_DD(type) {
    $("#" + type).empty();
    $("#" + type).append('<option value="choose">ALL</option>');
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
function GetDivisionsDD(division) {
    $("#" + division).empty();
    $("#" + division).append('<option value="choose">ALL</option>');
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