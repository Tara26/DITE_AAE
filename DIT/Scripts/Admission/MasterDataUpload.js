$(document).ready(function () {

    GetCourse();
    function GetCourse() {
        GetCourseTypes('CourseTypes');
        GetCourseTypes('CourseTypes1');
        GetCourseTypes('CourseTypes2');
    }

    function GetCourseTypes(contrl) {
        $("#" + contrl).empty();
        $("#" + contrl).append('<option value="choose">choose</option>');
        $.ajax({
            url: "/Admission/GetCourseTypes",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + contrl).append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                    });
                }

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
    GetOnLoadGridDetails();
    function GetOnLoadGridDetails() {   
        $.ajax({
            type: "Get",
            url: "/Admission/GetOnLoadGridDetails",
            success: function (data) {                
                var t = $('#GridViewSeatAvailability').DataTable({
                    data: data,
                    destroy: true,
                    columns: [
                        { 'data': 'MISCode', 'title': 'Slno', 'className': 'text-center' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        { 'data': 'Division_id', 'title': 'Division_id', 'className': 'text-left' },
                        { 'data': 'Division_name', 'title': 'Division_name', 'className': 'text-left' },
                        { 'data': 'district_id', 'title': 'DistrictCode', 'className': 'text-left' },
                        { 'data': 'dist_name', 'title': 'DistrictName', 'className': 'text-left' },
                        { 'data': 'iti_college_id', 'title': 'InstituteId', 'className': 'text-left' },
                        { 'data': 'iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                        { 'data': 'trade_id', 'title': 'TradeId', 'className': 'text-left' },
                        { 'data': 'trade_name', 'title': 'TradeName', 'className': 'text-left' },
                        { 'data': 'SeatsPerUnit', 'title': 'SeatsPerUnit', 'className': 'text-center' },
                        { 'data': 'units', 'title': 'Unit', 'className': 'text-left' },
                        { 'data': 'SeatType', 'title': 'GovtGIASeats', 'className': 'text-left' },
                        { 'data': 'PPP_seats', 'title': 'PPPSeats', 'className': 'text-left' },
                        { 'data': 'IMCPrivateManageMent', 'title': 'IMCPrivateManageMent', 'className': 'text-left' },
                        { 'data': 'IsPPP', 'title': 'IsPPP', 'className': 'text-center' },
                        { 'data': 'Shift', 'title': 'Shift', 'className': 'text-center' },
                        { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-center' }
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

    GetOnLOadUpdateGridDetails();
    function GetOnLOadUpdateGridDetails() {
        var CourseTypes = $('#CourseTypes1 :selected').val();
        var Divisions = $('#Divisions1 :selected').val();
        var Districts = $('#Districts1 :selected').val();
        var Talukas = $('#Talukas1 :selected').val();

        $.ajax({
            type: "Post",
            url: "/Admission/GetOnLoadGridDetails",
            data: {
                CourseTypes: CourseTypes,
                Divisions: Divisions,
                Districts: Districts,
                Talukas: Talukas,
            },
            success: function (data) {                
                var t = $('#GridUpdateSeatAvailability').DataTable({
                    data: data,
                    destroy: true,
                    columns: [
                        { 'data': 'MISCode', 'title': 'Slno', 'className': 'text-center' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        { 'data': 'Division_id', 'title': 'Division_id', 'className': 'text-left' },
                        { 'data': 'Division_name', 'title': 'Division_name', 'className': 'text-left' },
                        { 'data': 'district_id', 'title': 'DistrictCode', 'className': 'text-left' },
                        { 'data': 'dist_name', 'title': 'DistrictName', 'className': 'text-left' },
                        { 'data': 'iti_college_id', 'title': 'InstituteId', 'className': 'text-left' },
                        { 'data': 'iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                        { 'data': 'trade_id', 'title': 'TradeId', 'className': 'text-left' },
                        { 'data': 'trade_name', 'title': 'TradeName', 'className': 'text-left' },
                        { 'data': 'SeatsPerUnit', 'title': 'SeatsPerUnit', 'className': 'text-center' },
                        { 'data': 'units', 'title': 'Unit', 'className': 'text-left' },
                        { 'data': 'SeatType', 'title': 'GovtGIASeats', 'className': 'text-left' },
                        { 'data': 'PPP_seats', 'title': 'PPPSeats', 'className': 'text-left' },
                        { 'data': 'IMCPrivateManageMent', 'title': 'IMCPrivateManageMent', 'className': 'text-left' },
                        { 'data': 'IsPPP', 'title': 'IsPPP', 'className': 'text-center' },
                        { 'data': 'Shift', 'title': 'Shift', 'className': 'text-center' },
                        { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-center' },
                        {
                            'data': 'MISCode',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetSeatId(" + oData.MISCode + ")' class='btn btn-success btn-xs' data-toggle='modal' data-target='#myModal' value='Edit' id='edit'/>");
                            }

                        }]
                });
                t.on('order.dt search.dt', function () {
                    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                        cell.innerHTML = i + 1;
                    });
                }).draw();
            }
        });
    }

    GetGridDetails();
    function GetGridDetails() {
        
        var CourseTypes = $('#CourseTypes1 :selected').val();
        var Divisions = $('#Divisions :selected').val();
        var Districts = $('#Districts :selected').val();
        var Talukas = $('#Talukas :selected').val();        
        $.ajax({
            type: "Post",
            url: "/Admission/GetGridDetails",
            data: {
                CourseTypes: CourseTypes,
                Divisions: Divisions,
                Districts: Districts,
                Talukas: Talukas,
            },
            success: function (data) {                
                var t = $('#GridViewSeatAvailability').DataTable({
                    data: data,
                    destroy: true,
                    columns: [
                        { 'data': 'MISCode', 'title': 'Slno', 'className': 'text-center' },
                        { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                        { 'data': 'Division_id', 'title': 'Division_id', 'className': 'text-left' },
                        { 'data': 'Division_name', 'title': 'Division_name', 'className': 'text-left' },
                        { 'data': 'district_id', 'title': 'DistrictCode', 'className': 'text-left' },
                        { 'data': 'dist_name', 'title': 'DistrictName', 'className': 'text-left' },                       
                        { 'data': 'iti_college_id', 'title': 'InstituteId', 'className': 'text-left' },
                        { 'data': 'iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                        { 'data': 'trade_id', 'title': 'TradeId', 'className': 'text-left' },
                        { 'data': 'trade_name', 'title': 'TradeName', 'className': 'text-left' },
                        { 'data': 'SeatsPerUnit', 'title': 'SeatsPerUnit', 'className': 'text-center' },
                        { 'data': 'units', 'title': 'Unit', 'className': 'text-left' },
                        { 'data': 'SeatType', 'title': 'GovtGIASeats', 'className': 'text-left' },
                        { 'data': 'PPP_seats', 'title': 'PPPSeats', 'className': 'text-left' },
                        { 'data': 'IMCPrivateManageMent', 'title': 'IMCPrivateManageMent', 'className': 'text-left' },
                        { 'data': 'IsPPP', 'title': 'IsPPP', 'className': 'text-center' },
                        { 'data': 'Shift', 'title': 'Shift', 'className': 'text-center' },
                        { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-center' }
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
    GetDivisions();
    function GetDivisions() {        
        GetDivision('Divisions');
        GetDivision('Divisions1');
    } 
    
    function GetDivision(divi) {
        $("#" + divi).empty();       
        $("#" + divi).append('<option value="Select">Select</option>');        
        $.ajax({
            url: "/Admission/GetDivisions",
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data != null || data != '') {
                    $.each(data, function () {
                        $("#" + divi).append($("<option/>").val(this.Division_id).text(this.Division_name));                        
                    });
                }

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }
            
});

function GetDistricts(division,dist) {

    var divis = $('#' + division).val();
    GetDistrictList(divis, dist);
}
function GetTaluks(district, taluk) {
    var dist = $('#' + district).val();
    GetTaluk(dist, taluk);
}
function GetDistrictList(divis, dist) {
    $('#' + dist).empty();
    $("#" + dist).append('<option value="Select">Select</option>'); 
    //var Divisions = $('#Divisions :selected').val();
    $.ajax({
        type: 'Get',
        url: '/Admission/GetDistrictList',
        data: { Divisions: divis },
        success: function (data) {
            if (data != null || data != '') {  
                $.each(data, function () {
                    $("#" + dist).append($("<option/>").val(this.district_id).text(this.District_dist_name));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetTaluk(district, taluk) {
    $("#" + taluk).empty();
    $("#" + taluk).append('<option value="Select">Select</option>'); 
    //var Districts = $('#Districts :selected').val();
    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukList',
        data: { Districts: district },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + taluk).append($("<option/>").val(this.taluk_id).text(this.taluk_name));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetGridDetails() {    
    var CourseTypes = $('#CourseTypes1 :selected').val();
    var Divisions = $('#Divisions :selected').val();
    var Districts = $('#Districts :selected').val();
    var Talukas = $('#Talukas :selected').val();
    $.ajax({
        type: "Post",
        url: "/Admission/GetGridDetails",
        data: {
            CourseTypes: CourseTypes,
            Divisions: Divisions,
            Districts: Districts,
            Talukas: Talukas,
        },
        success: function (data) {            
            var t = $('#GridViewSeatAvailability').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'MISCode', 'title': 'Slno', 'className': 'text-center' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'Division_id', 'title': 'Division_id', 'className': 'text-left' },
                    { 'data': 'Division_name', 'title': 'Division_name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'DistrictCode', 'className': 'text-left' },
                    { 'data': 'dist_name', 'title': 'DistrictName', 'className': 'text-left' },
                    { 'data': 'iti_college_id', 'title': 'InstituteId', 'className': 'text-left' },
                    { 'data': 'iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                    { 'data': 'trade_id', 'title': 'TradeId', 'className': 'text-left' },
                    { 'data': 'trade_name', 'title': 'TradeName', 'className': 'text-left' },
                    { 'data': 'SeatsPerUnit', 'title': 'SeatsPerUnit', 'className': 'text-center' },
                    { 'data': 'units', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'SeatType', 'title': 'GovtGIASeats', 'className': 'text-left' },
                    { 'data': 'PPP_seats', 'title': 'PPPSeats', 'className': 'text-left' },
                    { 'data': 'IMCPrivateManageMent', 'title': 'IMCPrivateManageMent', 'className': 'text-left' },
                    { 'data': 'IsPPP', 'title': 'IsPPP', 'className': 'text-center' },
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-center' },
                    { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-center' },


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

function GetUpdateGridDetails() {
    var CourseTypes = $('#CourseTypes1 :selected').val();
    var Divisions = $('#Divisions1 :selected').val();
    var Districts = $('#Districts1 :selected').val();
    var Talukas = $('#Talukas1 :selected').val();

    $.ajax({
        type: "Post",
        url: "/Admission/GetGridDetails",
        data: {
            CourseTypes: CourseTypes,
            Divisions: Divisions,
            Districts: Districts,
            Talukas: Talukas,
        },
        success: function (data) {
            
            var t = $('#GridUpdateSeatAvailability').DataTable({
                data: data,
                destroy: true,
                columns: [
                    { 'data': 'MISCode', 'title': 'Slno', 'className': 'text-center' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'Division_id', 'title': 'Division_id', 'className': 'text-left' },
                    { 'data': 'Division_name', 'title': 'Division_name', 'className': 'text-left' },
                    { 'data': 'district_id', 'title': 'DistrictCode', 'className': 'text-left' },
                    { 'data': 'dist_name', 'title': 'DistrictName', 'className': 'text-left' },
                    { 'data': 'iti_college_id', 'title': 'InstituteId', 'className': 'text-left' },
                    { 'data': 'iti_college_name', 'title': 'InstituteName', 'className': 'text-left' },
                    { 'data': 'trade_id', 'title': 'TradeId', 'className': 'text-left' },
                    { 'data': 'trade_name', 'title': 'TradeName', 'className': 'text-left' },
                    { 'data': 'SeatsPerUnit', 'title': 'SeatsPerUnit', 'className': 'text-center' },
                    { 'data': 'units', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'SeatType', 'title': 'GovtGIASeats', 'className': 'text-left' },
                    { 'data': 'PPP_seats', 'title': 'PPPSeats', 'className': 'text-left' },
                    { 'data': 'IMCPrivateManageMent', 'title': 'IMCPrivateManageMent', 'className': 'text-left' },
                    { 'data': 'IsPPP', 'title': 'IsPPP', 'className': 'text-center' },
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-center' },
                    { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-center' },
                    {
                        'data': 'MISCode',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetSeatId(" + oData.MISCode + ")' class='btn btn-success btn-xs' data-toggle='modal' data-target='#myModal' value='Edit' id='edit'/>");
                        }

                    }]
            });
            t.on('order.dt search.dt', function () {
                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }
    });
}