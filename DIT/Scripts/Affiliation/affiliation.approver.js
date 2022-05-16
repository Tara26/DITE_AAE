
    $(document).ready(function () {
        $('.nav-tabs li:eq(0) a').tab('show');

    GetCourseTypes();
    GetDivisions();
    GetTrades();

        $(".search-published").click(function () {
            
    var Course_type = $("#CourseTypes_P").val();
    var Division = $("#Divisions_P").val();
    var District = $("#Districts_P").val();
    var Trade = $("#Trades_P").val();

    GetAllPublishedColleges(Course_type, Division, District, Trade);
});

        $(".search-submitted").click(function () {
            
    var Course_type = $("#CourseTypes").val();
    var Division = $("#Divisions").val();
    var District = $("#Districts").val();
    var Trade = $("#Trades").val();

    GetAllAffiliationColleges(Course_type, Division, District, Trade);
});

var tblGridSeatAvailability = $('#tblGridSubmittedAffiliation').DataTable();
        tblGridSeatAvailability.on('order.dt arch.dt', function () {
        tblGridSeatAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    var tblGridPublishAvailability = $('#tblGridAffiliatedColleges').DataTable();
        tblGridPublishAvailability.on('order.dt arch.dt', function () {
        tblGridPublishAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});

        function GetCourseTypes() {
        //tab 1 
        $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="0">Select</option>');
    // tab 2
    $("#CourseTypes_P").empty();
            $("#CourseTypes_P").append('<option value="0">Select</option>');
   
            $.ajax({
        url: "/Affiliation/GetCourseTypes",
    type: 'Get',
    contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data != null || data != '') {

        $.each(data, function () {
            $("#CourseTypes").append($("<option/>").val(this.course_id).text(this.course_name));
            $("#CourseTypes_P").append($("<option/>").val(this.course_id).text(this.course_name));

        });
    }

                }, error: function (result) {
        alert("Error", "something went wrong");
    }
});
}

        function GetDivisions() {
        //Update
        $("#Divisions").empty();
    $("#Divisions").append('<option value="0">Select</option>');
    //Publish
    $("#Divisions_P").empty();
            $("#Divisions_P").append('<option value="0">Select</option>');
  
            $.ajax({
        url: "/Affiliation/GetDivisions",
    type: 'Get',
    contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    
    $.each(data, function () {
        $("#Divisions").append($("<option/>").val(this.division_id).text(this.division_name));
    $("#Divisions_P").append($("<option />").val(this.division_id).text(this.division_name));
  
});


                }, error: function (result) {
        alert("Error", "something went wrong");
    }
});
}

        function GetDistricts_App() {
            
    var Divisions = $('#Divisions :selected').val();

    $("#Districts").empty();
            $("#Districts").append('<option value="0">Select</option>');
            $.ajax({
        type: 'Get',
    url: '/Affiliation/GetDistrictList',
                data: {Divisions: Divisions },
                success: function (data) {
                    
    $.each(data, function () {
        $("#Districts").append($("<option/>").val(this.district_id).text(this.district));
    });


                }, error: function (result) {
        alert("Error", "something went wrong");
    }
});
}

        function GetDistricts() {
            
    var Divisions = $('#Divisions_P :selected').val();

    $("#Districts_P").empty();
            $("#Districts_P").append('<option value="0">Select</option>');
            $.ajax({
        type: 'Get',
    url: '/Affiliation/GetDistrictList',
                data: {Divisions: Divisions },
                success: function (data) {
                    
    $.each(data, function () {
        $("#Districts_P").append($("<option/>").val(this.district_id).text(this.district));
    });


                }, error: function (result) {
        alert("Error", "something went wrong");
    }
});
}

        function GetTrades() {
        //Update
        $("#Trades").empty();
    $("#Trades").append('<option value="0">Select</option>');
    //Publish
    $("#Trades_P").empty();
            $("#Trades_P").append('<option value="0">Select</option>');
  
            $.ajax({
        type: 'Get',
    url: '/Affiliation/GetTradesList',
                success: function (data) {
                    if (data != null || data != '') {
        $.each(data, function () {
            $("#Trades").append($("<option/>").val(this.trade_id).text(this.trade_code));
            $("#Trades_P").append($("<option/>").val(this.trade_id).text(this.trade_code));

        });
    }

                }, error: function (result) {
        alert("Error", "something went wrong");
    }
});
}

        function GetAllPublishedColleges(courseId, divisionId, districtId, tradeId) {
        $.ajax({
            type: 'Get',
            url: '/Affiliation/FilterPublishedCollegeDetails',
            data: { courseId: courseId, divisionId: divisionId, districtId: districtId, tradeId: tradeId },
            success: function (data) {
                
                if (data.length > 0) {
                    
                    var t = $('#tblGridAffiliatedColleges').DataTable({
                        data: data,
                        destroy: true,
                        columns: [
                            { 'data': 'name_of_iti', 'title': 'Slno', 'className': 'text-center' },
                            { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': 'text-center' },
                            { 'data': 'mis_code', 'title': 'MISCode', 'className': 'text-left' },
                            { 'data': 'type_of_iti', 'title': 'Type of ITI', 'className': 'text-left' },
                            { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                            { 'data': 'state', 'title': 'State', 'className': 'text-left' },
                            { 'data': 'district', 'title': 'District', 'className': 'text-center' },
                            { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                            { 'data': 'panchayat', 'title': 'Panchayat', 'className': 'text-left' },
                            { 'data': 'village', 'title': 'Village', 'className': 'text-left' },
                            { 'data': 'constituency', 'title': 'Constituency', 'className': 'text-left' },
                            { 'data': 'build_up_area', 'title': 'Built Up area', 'className': 'text-center' },
                            { 'data': 'geo_location', 'title': 'Unit', 'Geo Location': 'text-left' },
                            { 'data': 'email', 'title': 'Email Id', 'className': 'text-left' },
                            { 'data': 'phone_number', 'title': 'Phone number', 'className': 'text-left' },
                            { 'data': 'date', 'title': 'Affiliation Date', 'className': 'text-left' },
                            { 'data': 'no_trades', 'title': 'No. of Trades', 'className': 'text-center' },
                            { 'data': 'no_units', 'title': 'No. of Units', 'className': 'text-center' },
                            { 'data': 'no_shifts', 'title': 'No. of Shifts', 'className': 'text-center' },
                            {
                                'data': 'FileUploadPath',
                                render: function (data, type, row) {
                                    if (row.FileUploadPath != null && row.FileUploadPath != "") {
                                        return "<a class='btn btn-link' href=/Affiliation/Download?path='" + row.FileUploadPath.replace(/%20/g, " ") + "'>Download File</a>"
                                    }
                                    else {
                                        return "<p>No File !</p>"
                                    }
                                    //else {
                                    //    return "<input type='file' name='files' />"
                                    //}
                                    // return "<input type='file' name='files' />"
                                }
                            }

                        ]
                    });
                    t.on('order.dt search.dt', function () {
                        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                            cell.innerHTML = i + 1;
                        });
                    }).draw();
                }
                else {
                    alert("No data found!")
                    $('#tblGridAffiliatedColleges').DataTable().clear().draw();
                }

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }

        function GetAllAffiliationColleges(courseId, divisionId, districtId, tradeId) {
        $.ajax({
            type: 'Get',
            url: '/Affiliation/FilterSubmittedCollegeDetails',
            data: { courseId: courseId, divisionId: divisionId, districtId: districtId, tradeId: tradeId },
            success: function (data) {
                
                if (data.length > 0) {
                    
                    var t = $('#tblGridSubmittedAffiliation').DataTable({
                        data: data,
                        destroy: true,
                        columns: [
                            { 'data': 'name_of_iti', 'title': 'Slno', 'className': 'text-center' },
                            { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': 'text-center' },
                            { 'data': 'mis_code', 'title': 'MISCode', 'className': 'text-left' },
                            { 'data': 'type_of_iti', 'title': 'Type of ITI', 'className': 'text-left' },
                            { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                            { 'data': 'state', 'title': 'State', 'className': 'text-left' },
                            { 'data': 'district', 'title': 'District', 'className': 'text-center' },
                            { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                            { 'data': 'panchayat', 'title': 'Panchayat', 'className': 'text-left' },
                            { 'data': 'village', 'title': 'Village', 'className': 'text-left' },
                            { 'data': 'constituency', 'title': 'Constituency', 'className': 'text-left' },

                            { 'data': 'geo_location', 'title': 'Unit', 'Geo Location': 'text-left' },
                            { 'data': 'address', 'title': 'Address', 'className': 'text-center' },
                            { 'data': 'location_type', 'title': 'Location Type', 'className': 'text-center' },
                            { 'data': 'email', 'title': 'Email Id', 'className': 'text-left' },
                            { 'data': 'phone_number', 'title': 'Phone number', 'className': 'text-left' },
                            { 'data': 'date', 'title': 'Affiliation Date', 'className': 'text-left' },
                            { 'data': 'no_trades', 'title': 'No. of Trades', 'className': 'text-center' },
                            { 'data': 'no_units', 'title': 'No. of Units', 'className': 'text-center' },
                            { 'data': 'no_shifts', 'title': 'No. of Shifts', 'className': 'text-center' },
                            {
                                'data': 'iti_college_id',
                                render: function (data, type, row) {
                                    if (row.FileUploadPath != null && row.FileUploadPath != "") {
                                        return "<a class='btn btn-link' href=/Affiliation/DownloadAffiliationDoc?CollegeId=" + row.iti_college_id + ">Download File</a><input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"

                                    }
                                    else {
                                        return "<p></p>"
                                    }
                                    // return "<input type='file' name='files' />"
                                }
                            },
                            { 'data': 'status', 'title': 'Status', 'className': 'text-center' },
                            { 'data': 'remarks', 'title': 'Remarks', 'className': 'text-center' },
                            {
                                'date': 'iti_college_id',
                                render: function (data, type, row) {

                                    return "<input type='checkbox' class='form-check-input check-me' />"
                                }
                            }

                        ]
                    });
                    t.on('order.dt search.dt', function () {
                        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                            cell.innerHTML = i + 1;
                        });
                    }).draw();
                }
                else {

                    $('#tblGridSubmittedAffiliation').DataTable().clear().draw();
                    alert("No data found!")
                }

            }, error: function (result) {
                alert("Error", "something went wrong");
            }
        });
    }

        function ApproveColleges() {
            
    var selected = [];
            if ($('.check-me').is(':checked')) {
                
    $("input:checked").each(function () {
        selected.push($(this).closest("tr").find(".collegeId").val());
    });

                $.ajax({
        url: "/Affiliation/ApproveAffiliatedCollege",
                    data: {collegeIds: selected },
    type: 'POST',
                    success: function (data) {
                        
    if (data.flag == 1) {
        alert("Selected College(s) approved successfully")

                            GetAllAffiliationColleges(0, 0, 0, 0);
}
                        else {
        console.log(data.status);
    alert("Failed")
}
}

})

}
            else {
        alert("Pls select atleast one college to approve!")
    }


    }

        function RejectColleges() {
            
    var selected = [];
            if ($('.check-me').is(':checked')) {
                
    $("input:checked").each(function () {
        selected.push($(this).closest("tr").find(".collegeId").val());
    });

                $.ajax({
        url: "/Affiliation/RejectAffiliatedCollege",
                    data: {collegeIds: selected },
    type: 'POST',
                    success: function (data) {
                        
    if (data.flag == 1) {
        alert("Selected College(s) rejected successfully")

                            GetAllAffiliationColleges(0, 0, 0, 0);
}
                        else {
        console.log(data.status);
    alert("Failed")
}
}

})

}
            else {
        alert("Pls select atleast one college to reject!")
    }


}
