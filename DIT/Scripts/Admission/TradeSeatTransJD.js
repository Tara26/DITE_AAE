GetCourseTypes();
GetDivisions();
GetStatusList();

function GetProjectDetailsList() {
    $("#EnableDataGridTab2").show();
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatList",
        contentType: "application/json",
        success: function (data) {
            $('#tblUpdateSeatAvail').DataTable({
                data: data,
                "destroy": true,
                columns: [
                    { 'data': 'SlNo', 'title': 'Sl No' },
                    //{ 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-left' },
                    { 'data': 'Division_name', 'title': 'Division name' },
                    { 'data': 'dist_name', 'title': 'District Name', 'className': 'text-left' },
                    {
                        'data': 'Trade_ITI_seat_id',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='GetSeatId(" + oData.Trade_ITI_seat_id + ")' class='btn btn-success btn-xs' data-toggle='modal' data-target='#myModal' value='Edit' id='edit'/>");

                        }
                    }
                ]
            });
        }
    });
}

function GetCourseTypes() {
    $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#CourseTypes").append($("<option/>").val(this.CourseId).text(this.CourseTypeName));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetDivisions() {
    $("#Divisions").empty();
    $("#Divisions").append('<option value="Select">Select</option>');
    $.ajax({
        url: "/Admission/GetDivisions",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Divisions").append($("<option/>").val(this.Division_id).text(this.Division_name));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetDistrictList() {
    var Divisions = $('#Divisions :selected').val();
    $.ajax({
        type: 'Get',
        url: '/Admission/GetDistrictList',
        data: { Divisions: Divisions },
        success: function (data) {
            if (data != null || data != '') {
                $("#Districts").empty();
                $("#Talukas").empty();
                $("#Districts").append($("<option/>").val(0).text("--Please Select--"));
                $("#Talukas").append($("<option/>").val(0).text("--Please Select--"));
                $.each(data, function () {
                    $("#Districts").append($("<option/>").val(this.district_id).text(this.District_dist_name));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetTaluka() {
    var Districts = $('#Districts :selected').val();
    $.ajax({
        type: 'Get',
        url: '/Admission/GetTalukList',
        data: { Districts: Districts },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Talukas").append($("<option/>").val(this.taluk_id).text(this.taluk_name));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetSeatId(TradeITISeatid) {
    GetStatusList();
    $('textarea#RemarksTxt').text("");
    $.ajax({
        type: 'Get',
        url: '/Admission/GetStatusListById',
        data: { TradeITISeatid: TradeITISeatid },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $('#TradeITIseatidID').val(this.Trade_ITI_seat_trans_id);
                    $("#TradeITIseatidID").hide();
                    $('textarea#RemarksTxt').text(this.Remarks);
                    $("#StatusId").val(this.StatusId);
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function GetStatusList() {
    $.ajax({
        type: 'Get',
        url: '/Admission/GetStatusList',
        success: function (data) {
            $("#StatusId").empty();
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#StatusId").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function UpdateRemarksDetailsForSeat() {
    var Remarks = $('textarea#RemarksTxt').val();
    var Status = $('#StatusId :selected').val();
    var TradeITIseatidID = $('#TradeITIseatidID').val();
    $.ajax({
        type: 'Get',
        url: '/Admission/UpdateRemarksDetailsForSeat',
        data: {
            Remarks: Remarks,
            Status: Status,
            TradeITIseatidID: TradeITIseatidID
        },
        success: function (data) {
            if (data.length > 0) {
                $('#tblUpdateSeatAvail').DataTable({
                    data: data,
                    "destroy": true,
                    columns: [
                        { 'data': 'SlNo', 'title': 'Sl No' },
                        //{ 'data': 'MISCode', 'title': 'MIS Code', 'className': 'text-left' },
                        { 'data': 'Division_name', 'title': 'Division name' },
                        { 'data': 'dist_name', 'title': 'District Name', 'className': 'text-left' },
                        {
                            'data': 'Trade_ITI_seat_id',
                            'title': 'Action',
                            "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                                $(nTd).html("<input type='button' onclick='GetSeatId(" + oData.Trade_ITI_seat_id + ")' class='btn btn-success btn-xs' data-toggle='modal' data-target='#myModal' value='Edit' id='edit'/>");

                            }
                        }
                    ]
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}