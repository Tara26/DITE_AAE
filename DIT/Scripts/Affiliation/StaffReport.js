$(document).ready(function () {
    $('.tab-2').tab('show');
    $('.tab-1').tab('show');
    
    // GetOfficers();
    
    GetCourseTypes();
    GetDivisions();
    GetDistrictList();
    GetDistrict_Update();
    GetTypesOfITI();
    GetLocationTypes();
    GetStaffType();
    GetAllTrades();
    GetAffiliationSchemes();
    GetTalukList();
    GetAllGender();
    GetAllYear();

    $('#PhoneNoEdit').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#PhoneNo').bind("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#Staffdetails_view').hide();
    $('#Staffdetails_view1').hide();
    $('#Staf-Excel').hide();
    $('#staf-count').hide();

});

function GetCourseTypes() {
    //Add 
    $("#Course-Add").empty();
    $("#Course-Add").append('<option value="">Select</option>');

    //Update List
    $("#CourseTypes").empty();
    //$("#CourseTypes").append('<option value="">Select</option>');
    // Publish
    $("#CourseTypes_P").empty();
    $("#CourseTypes_P").append('<option value="">Select</option>');
    //Affiliated View
    $("#CourseTypes_Af").empty();
    $("#CourseTypes_Af").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetCourseTypes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#CourseTypes").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#CourseTypes_P").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#CourseTypes_Af").append($("<option/>").val(this.course_id).text(this.course_name));
                    $("#Course-Add").append($("<option/>").val(this.course_id).text(this.course_name));

                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDivisions() {
    //Update
   var div= $("#hdnDiv_ID").data('value')
    
    $("#Divisions").empty();
    if ($("#hdnDiv_ID").data('value') == 0) {
        $("#Divisions").append('<option value="0">ALL</option>');
    }
    //Publish
    $("#Divisions_P").empty();
    $("#Divisions_P").append('<option value="">Select</option>');
    //Affiliated View
    $("#Divisions_Af").empty();
    $("#Divisions_Af").append('<option value="">Select</option>');
    // Add Page
    $("#Division-Add").empty();
    $("#Division-Add").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetDivisions",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
          
            $.each(data, function () {
                
                if (($("#hdnDiv_ID").data('value') == 0 || $("#hdnDiv_ID").data('value') == this.division_id)) {
                    $("#Divisions").append($("<option/>").val(this.division_id).text(this.division_name));
                }
                $("#Divisions_P").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Divisions_Af").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Division-Add").append($("<option/>").val(this.division_id).text(this.division_name));
            });
        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDistrictList() {
    
    var Divisions = $('#Divisions :selected').val();

    if ($("#hdnDiv_ID").data('value') != 0) {
        Divisions = $("#hdnDiv_ID").data('value');
    }
    if (Divisions != "" && Divisions != null) {
        $("#Districts").empty();
        $("#Districts").append('<option value="0">ALL</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#Districts").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }

}

//$("#Districts").change(function () {
//    
//    var value = $(this).val();
//    $("#Taluka").empty();
//    $("#Taluka").append('<option value="">Select</option>');
//    $.ajax({
//        url: "/Affiliation/GetTaluk",
//        type: 'Get',
//        data: { DistId: value },
//        contentType: 'application/json; charset=utf-8',
//        success: function (data) {


//            $.each(data, function () {
//                $("#Taluka").append($("<option/>").val(this.Value).text(this.Text));
//            });


//        }, error: function (result) {
//            bootbox.alert("Error", "something went wrong");
//        }
//    });
//});



function GetTypesOfITI() {
    // var Course = $('#CourseTypes :selected').val();
    $("#TypeOfITI-Add").empty();
    $("#TypeOfITI-Add").append('<option value="0">ALL</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetInstitutionType',
        //data: { CourseId: Course },
        success: function (data) {
            
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#TypeOfITI-Add").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetLocationTypes() {
    // var Course = $('#CourseTypes :selected').val();
    $("#LocationType-Add").empty();
    $("#LocationType-Add").append('<option value="0">ALL</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetLocationType',
        //data: { CourseId: Course },
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#LocationType-Add").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDistrict_Update() {

    var Divisions = $('#Division-Update :selected').val();
    if (Divisions != "" && Divisions != null) {
        $("#District").empty();
        $("#District").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#District").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#District").empty();
        $("#District").append('<option value="">Select</option>');
        $("#Taluka").empty();
        $("#Taluka").append('<option value="">Select</option>');
    }

}

function GetStaffType() {
    
    $("#dd-stafftype").append('<option value="0">ALL</option>');

    $.ajax({
        url: "/Affiliation/GetAllStaffType",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
            $.each(data, function () {
                
                $("#dd-stafftype").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllTrades() {
    $("#dd-trade").append('<option value="0">ALL</option>');
    
    $.ajax({
        url: "/Affiliation/GetAllTrades",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
            $.each(data, function () {
                
                $("#dd-trade").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function GetAllSchemes() {
    $("#dd-trade").append('<option value="0">ALL</option>');
    
    $.ajax({
        url: "/Affiliation/GetAllTrades",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
            $.each(data, function () {
                
                $("#dd-trade").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAffiliationSchemes() {

    $("#Scheme-Add").empty();
    $("#Scheme-Add").append('<option value="0">ALL</option>');

    $.ajax({
        url: "/Affiliation/GetAffiliationSchemes",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {
                $("#Scheme-Add").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetTalukList() {
    
    var Districts = $('#Districts :selected').val();
    if (Districts != "" && Districts != null) {
        $("#Taluka").empty();
        $("#Taluka").append('<option value="0">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetTaluk',
            data: { DistId: Districts },
            success: function (data) {

                $.each(data, function () {
                    $("#Taluka").append($("<option/>").val(this.Value).text(this.Text));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }

}

function GetAllGender() {
    $("#Gender").append('<option value="0">ALL</option>');
    $("#Genderid").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllGender",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#Gender").append($("<option/>").val(this.Value).text(this.Text));
                $("#Genderid").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function GetAllYear() {
    //$("#Session12").append('<option value="">Select</option>');
    //$("#year").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllYear",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $.each(data, function () {

                $("#Session12").append($("<option/>").val(this.Value).text(this.Text));
                $("#year").append($("<option/>").val(this.Value).text(this.Text));
            });


        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
//======================== End Dropdown =====================

function GetAllstaffInstitute () {
    
    var Session = $("#Session12").val();
    var courseId = $("#CourseTypes").val();
    var divisionId = $("#Divisions").val();
    var districtId = $("#Districts").val();
    var taluk = $("#Taluka").val();
    var Insttype = $("#TypeOfITI-Add").val();
    var location = $("#LocationType-Add").val();
    var stafftype = $("#dd-stafftype").val();
    var tradeId = $("#dd-trade").val();
    var gender = $("#Gender").val();
    var scheme = $("#Scheme-Add").val();
    var training = $("#trainingID").val();
    var quarter = $("#Quarterid").val();

    GetAllstaffInstituteReprt(Session, courseId, divisionId, districtId, taluk, Insttype, location, stafftype, tradeId, gender, scheme, training, quarter);
}


function GetAllstaffInstituteReprt(Session, courseId, divisionId, districtId, taluk, Insttype, location, stafftype, tradeId, gender, scheme, training, quarter)
{
    
    $.ajax({
        type: 'Get',
        url: '/Affiliation/Institutestaffreport',
        data: { Year: Session, courseId: courseId, divisionId: divisionId, districtId: districtId, taluk: taluk, Insttype: Insttype, location: location, stafftype: stafftype, tradeId: tradeId, gender: gender, scheme: scheme, training: training, quarter: quarter },
        success: function (data) {
            
            var index = 1;
            if (data.resultllist !=null)
            {
                $('#staf-count').show();
                $('#StaffCount').text(data.resultllist.StaffCount);
            }
            
            if (data.Stafflist.length > 0) {
                $('#Staf-Excel').show();

                var t = $('#StaffStatusTable').DataTable({
                    data: data.Stafflist,
                    "destroy": true,
                    "bSort": true,
                  
                    columns: [
                        { 'data': 'slno', 'title': 'Sl. No.', 'className': '' },
                        { 'data': 'Year', 'title': 'Session', 'className': '' },
                        { 'data': 'Coursetype', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'MIScode', 'title': 'MIS ITI Code', 'className': 'text-left' },
                        { 'data': 'InstituteName', 'title': 'Institute Name', 'className': 'text-left' },
                        { 'data': 'Division', 'title': 'Division', 'className': 'text-left' },
                        { 'data': 'District', 'title': 'District', 'className': 'text-center' },
                        { 'data': 'Taluk', 'title': 'Taluk', 'className': 'text-center' },
                        { 'data': 'Name', 'title': 'Name of Staff ', 'className': 'text-left' }, 
                        { 'data': 'GenderName', 'title': 'Gender', 'className': 'text-left' },
                        { 'data': 'DesignationName', 'title': 'Designation', 'className': 'text-left' },
                        { 'data': 'Qualification', 'title': 'Qualification', 'className': 'text-center' },
                        { 'data': 'subject', 'title': 'Teaching Subject', 'Geo Location': 'text-left' },
                        { 'data': 'Tradename', 'title': 'Trade', 'className': 'text-left' },
                        { 'data': 'MobileNum', 'title': 'Mobile No.', 'className': 'text-left' },
                        { 'data': 'EmailId', 'title': 'Email', 'className': 'text-left' },
                        { 'data': 'Type', 'title': 'Staff Type', 'className': 'text-left' },
                        { 'data': 'InstitutionType', 'title': 'Institute Type', 'className': 'text-left' },
                        { 'data': 'TrainingMode', 'title': 'Mode Of Training', 'className': 'text-left' },
                        { 'data': 'location_type', 'title': 'Location Type', 'className': 'text-left' },
                        { 'data': 'Scheme', 'title': 'Financial Scheme', 'className': 'text-left' },
                        {
                            'title': 'Photo',
                            render: function (data, type, row) {
                                return "<img src='" + row.Photo + "' class='zoom'/>"
                            }
                        },

                    ],
                    "columnDefs": [{

                        "render": function (data, type, full, meta) {

                            return index++;
                        },
                        "targets": 0
                    }],
                });
                
                if ($('#dd-trade option:selected').text() != 'ALL') {

                    valu = $('#dd-trade option:selected').text();

                    //id = id - 1;
                    t.columns(13).search(valu).draw();
                    $('#StaffCount').text(t.page.info().recordsDisplay);
                }
                //table.columns(id).search(valu).draw();
               
               

            }
            else {
                bootbox.alert("No data found!")
                t.destroy();
                t.clear().draw();
                //$('#StaffStatusTable').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function pdfStaffDownload() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();

    today = dd + '/' + mm + '/' + yyyy;

    
    // Get the element.
    $('#StaffStatusTable').DataTable().destroy();
    $('#StaffStatusTable').DataTable({ "paging": false }).draw(false);
    var element = document.getElementById('StaffStatusTable');

    //// Choose pagebreak options based on mode.
    var mode = document.getElementById('mode').value;
    var pagebreak = (mode === 'specify') ?
        { mode: '', before: '.before', after: '.after', avoid: '.avoid' } :
        { mode: mode };

        html2pdf().from(element).set({
        filename: 'StaffReport List_' + today + '.pdf',
        pagebreak: { mode: ['css', 'legacy'] },
        html2canvas: { scale: 2 },
        jsPDF: { orientation: 'landscape', unit: 'in', format: 'a3', compressPDF: true }
    }).save();
}

function ExcelExportStaff() {
    
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    today = dd + '/' + mm + '/' + yyyy;
    $('#StaffStatusTable').DataTable().destroy();
    $('#StaffStatusTable').DataTable({ "paging": false }).draw(false);

    $("#StaffStatusTable").table2excel({
        filename: "StaffSReportList_" + today + ".xls",
    });
}
