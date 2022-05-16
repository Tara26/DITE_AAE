$(document).ready(function () {
    $('#ddremarksErr').hide();
    $("#statusDDErr").hide();
 
  $('#academicyear').mouseup(function () {
    $('#academicYearErr').text('');
  });
  $('#users').change(function () {
    $('#roleIdErr').text('');
  });
  $('#status').change(function () {
    $('#statusErr').text('');
  });
  $('#usersEdit').change(function () {
    $('#usersErr').text('');
  });
  $('#statusEdit').change(function () {
    $('#statusErrs').text('');
  });
 
  $(function () {
    $("#remarksEdit").tooltip();
  });
 
    $('#forwardJd').hide();
    $('#approveDD').hide();
    $('#hideSentoDD').hide();
    var selectedTab = "Itiadmin";
    if (selectedTab != null) {
        if (selectedTab == "Itiadmin") {
            $('.nav-tabs li:eq(0) a').tab('show');
        }
        else {
            $('.nav-tabs li:eq(2) a').tab('show');
        }
    }
    if (RoleId != null) {
        if (RoleId == 9) {
            $('#tab_1').show();
        }
        else {
            $('#tab_1').hide();
            $('#tab_2').attr("class", "in active");

        }
        if (RoleId == 16) {
            $('#forwardJd').show();
        }
        if (RoleId == 5) {
            $('#approveDD').show();
        }
    }
    GetSessionYear('Session');
    GetSessionYear('Session3');
    GetCourses("CourseTypes");
    GetCourses("CourseTypesEdit");
    GetCourses("CourseType2");
    GetCourses("CourseType3");
    GetCourses("CourseTypesTab3");
    GetRoles("users", 16);
    GetRoles("usersEdit", 16);
    GetRoles("sentTo", 9);
    GetRoles1("DepartmentUsers", 5);
    GetDivisionsDDp("divisionDdp");//filter     
    GetDivisionsDDp("divisionDdp3");    
    GetSeatAvailabilityStatus("status");
    GetSeatAvailabilityStatus("statusdp");
    GetSeatAvailabilityStatus("statusDD");
    GetSeatAvailabilityStatus("statusEdit");
    GetSeatAvailability();

    $('#academicyear').datepicker({
        changeMonth: true,
      changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
      dateFormat: 'MM yy',        
        selectedMonth: 8,
        defaultDate: '6m',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear').val(inst.selectedYear);
            $('#AcademicMonths').val(inst.selectedMonth);
            GetSeatAvailability();
        },
        beforeShow: function (dateText) {
            console.log("Selected date: " + dateText + "; input's current value: " + this.value);
        }
    }).focus(function () {
      $(".ui-datepicker-calendar").hide();
      $(".ui-datepicker-current").hide();
        var year = new Date().getFullYear();
      $("#ui-datepicker-div").position({
        my: "left top",
        at: "left bottom",
        of: $(this)
      });
    }).attr("readonly", false).datepicker('setDate', new Date(new Date().getFullYear(), 7));
    $("#academicyear").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    });
    $('#academicyearEdit').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',

        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicEditYear').val(inst.selectedYear);
            $('#AcademicEditMonths').val(inst.selectedMonth);
        },
        beforeShow: function (dateText) {
            console.log("Selected date: " + dateText + "; input's current value: " + this.value);
        }
    });
    $("#academicyearEdit").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    });

    $('#academicyearTab3').datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-1:+1",
        showButtonPanel: true,
        dateFormat: 'MM yy',
        selectedMonth: 8,
        defaultDate: '7m',
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            $('#AcademicYear').val(inst.selectedYear);
            $('#AcademicMonths').val(inst.selectedMonth);
        },
        beforeShow: function (dateText) {
            console.log("Selected date: " + dateText + "; input's current value: " + this.value);
        }
    }).focus(function () {
        $(".ui-datepicker-calendar").hide();
        $(".ui-datepicker-current").hide();
        var year = new Date().getFullYear();
        $("#ui-datepicker-div").position({
            my: "left top",
            at: "left bottom",
            of: $(this)
        });
    }).attr("readonly", false).datepicker('setDate', new Date(new Date().getFullYear(), 7));
    $("#academicyearTab3").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $("#ui-datepicker-div").position({
            my: "center top",
            at: "center bottom",
            of: $(this)
        });
    });
    GetFilterData("districtDdp");
    GetFilterData("districtDdp3");
    $('#submitbtn').hide();
    $('#approveDD').hide();
    $('#forwardJd').hide();

    $('#DepartmentUsers').attr('disabled', 'disabled');
    //$("#CourseTypes").val(0);
   // $("#CourseTypes option:selected").val('NCVT');
    //$("#CourseTypes option[value='101']").attr("selected", "selected");
   // $('#CourseTypes').val("NCVT");
    //var element = document.getElementById('#CourseTypes.CourseTypeName');
    //element.value = 'NCVT';
    //$("#CourseTypes").val($("#CourseTypes option:first").val());
    btnsearchFiltertable();
});

$('a[href="#tab_2"]').click(function () {
    $('#tab_3').hide();
    $('#tab_2').show();
});

$('a[href="#tab_3"]').click(function () {
    $('#tab_2').hide();
    $('#tab_3').show();
    seatAvailabilityDataNew();
});
//=============Satrt Tab 1==================
function GetSeatAvailabilityStatus(user) {
    $("#" + user).empty();
    $("#" + user).append('<option value="choose">choose</option>');
    $.ajax({
        url: "/Admission/SeatAvailabilityStatus",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + user).append($("<option/>").val(this.Status).text(this.StatusName));
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetRoles1(user, id) {
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
                    if (data[0].RoleID == 5) {
                        $("#" + user).append($("<option/>").val(this.RoleID).text(loginUserRole.DDAdmCellText));
                    }
                    else {
                        $("#" + user).append($("<option/>").val(this.RoleID).text(this.RoleName));
                    }                    
                });
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetSeatAvailability() {
    var courseCode = $("#CourseTypes").val();
    var AcademicYear = $('#academicyear').val().split(" ")[1];
    if (AcademicYear == null) {
        AcademicYear = new Date().getFullYear();
    }
    if (courseCode == null) {
        courseCode = 101;
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAvailabilityList",
        contentType: "application/json",
        data: { 'courseCode': courseCode, 'AcademicYear': AcademicYear },
        success: function (data) {
            var id = 1;
            var idd = 1;
            var iddd = 1;
            var tradid = 1;
            var idddBB = 1;
            $('#seatAvailabilityTable').DataTable({                
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center miscode' },
                  /*  { 'data': 'AcademicYearString', 'title': 'Academic Year', 'className': 'text-left' },*/
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center miscode' },
                   /* { 'data': 'DivisionId', 'title': 'Division Code', 'className': 'text-left' },*/
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                    /*{ 'data': 'DistrictId', 'title': 'District Code', 'className': 'text-left' },*/
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                    //{ 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                    {
                        'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var trade = "tradeId" + tradid;
                            $(nTd).html("<span id='" + trade + "'>" + oData.TradeId + "</span>");
                            tradid++;
                        }
                    },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },
                    //{
                    //    'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-left',

                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(new Date().getFullYear(),7, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return (month + "/" + year + "-" + month + "/" + (year + parseInt(oData.Duration)));
                    //    }
                    //},
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    {
                        'data': 'Unit', 'title': 'Eligible For Current Admission', 'className': 'text-center',
                        "sTitle": "<span>Select</span><input type='checkbox' id=cbxCheckAllData onclick='SelectAllData(this.id)'></input>", "bSortable": false,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var stat = oData.IsActive == true ? 'Active' : 'DeActive';
                            if (oData.IsActive == true)
                                $(nTd).html("<input type='checkbox' name='seatCheck' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")'  id='EligAdmission'/>   <img src='/content/frontend/images/i1.jpg' width='60%' height='60%' data-toggle='tooltip' onmouseover='ActiveStatusiNFO()' id='imginformSeat' border='0' title = " + stat + " />"); //title= " + oData.IsActive == true ? 'Active' : 'InActive' + "
                            else
                                $(nTd).html("<input type='checkbox' name='seatCheck' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")'  id='EligAdmission' disabled/>   <img src='/content/frontend/images/i1.jpg' width='60%' height='60%' data-toggle='tooltip' onmouseover='ActiveStatusiNFO()' id='imginformSeat' border='0' title = " + stat + " />"); 
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Seat Type', 'className': 'text-left seatType', 'width': '10%',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var sea = "seatType" + id;
                            if (oData.IsActive == true) {
                            $(nTd).html("<select style='width:100px;' class='form-control' id='" + sea + "' onchange='assignSeats(\"" + sea + "\",\"" + id + "\")'><option value='choose'>choose</option></select>");
                            GetSeatTypes(sea);
                            id++;
                        }
                            else {
                                $(nTd).html("<select style='width:100px;' class='form-control' id='" + sea + "' onchange='assignSeats(\"" + sea + "\",\"" + id + "\")' disabled><option value='choose'>choose</option></select>");
                                GetSeatTypes(sea);
                                id++;
                            }
                        }
                    },
                    {
                        'data': 'batchsize', 'title': 'Batch Size', 'className': 'text-left',
                        //"fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        //    var mngsea = "totalseat" + idddBB;
                        //    $(nTd).html("<span id='" + mngsea + "'></span>");
                        //    idddBB++;
                        //}
                    },
                    {
                        'data': 'Unit', 'title': 'Govt Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var govsea = "govSeat" + idd;
                            $(nTd).html("<span id='" + govsea + "'></span>");
                            idd++;
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Management Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var mngsea = "mngSeat" + iddd;
                            $(nTd).html("<span id='" + mngsea + "'></span>");
                            iddd++;
                        }
                    },
                    

                    { 'data': 'DualSystemTraining', 'title': 'Dual System Training', 'className': 'text-left' }
                ]
            });
        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function SelectAllData(id) {
    // Get all rows with search applied
    var rows = $('#seatAvailabilityTable').DataTable().rows({ 'search': 'applied' }).nodes();
    // Check/uncheck checkboxes for all rows in the table
    if ($('#' + id).is(":checked")) {
        $('input[type="checkbox"]:enabled', rows).prop('checked', true);
    } else {
        $('input[type="checkbox"]:enabled', rows).prop('checked', false);
    }
}
function ActiveStatusiNFO(x) {   
     $('[data-toggle="tooltip"]').tooltip();
}

//function GetSeatTypes(seatType) {
//    $("#" + seatType).empty();
//    $("#" + seatType).append('<option value="choose">choose</option>');
//    $.ajax({
//        url: "/Admission/GetSeatTypes",
//        type: 'Get',
//        contentType: 'application/json; charset=utf-8',
//        success: function (data) {
//            if (data != null || data != '') {
//                $.each(data, function () {
//                    $("#" + seatType).append($("<option/>").val(this.SeatTypeId).text(this.SeatTypeName));
//                });
//            }

//        }, error: function (result) {
//            //alert("Error", "something went wrong");
//            bootbox.alert("Error", "something went wrong");
//        }
//    });
//}

//function assignSeats(seat, id) {    
//    var selectedId = $('#' + seat).val();
//    var GSeat = "govSeat" + id;
//    var MSeat = "mngSeat" + id;
//    var tradeId = "tradeId" + id;
//    var tradenum = $('#' + tradeId).text();
//    var totalseat = "totalseat" + id;
//    if (selectedId == 'choose') {
//        $('#' + GSeat).text('0');
//        $('#' + MSeat).text('0');
//       // alert('select seat type');
//        bootbox.alert('please select seat type');
//    } else {
//        $.ajax({
//            type: "GET",
//            url: "/Admission/GetSeatsByTradeIdSeatType",
//            data: { 'tradeId': tradenum },
//            contentType: "application/json",
//            success: function (data) {
//                //totalseat = data;
//                if (data != null) {
//                    if (selectedId == 1 || selectedId == 3) {
//                        var govts = data - 4;
//                        $('#' + GSeat).text(govts);
//                        $('#' + MSeat).text('4');
//                        $('#' + totalseat).text(data);
//                    }
//                    else if (selectedId == 2) {
//                        $('#' + GSeat).text(data);
//                        $('#' + MSeat).text('0');
//                        $('#' + totalseat).text(data);
//                    }
//                    else if (selectedId == 5 || selectedId == 4) {
//                        $('#' + GSeat).text(0);
//                        $('#' + MSeat).text(data);
//                        $('#' + totalseat).text(data);
//                    }
//                }
//            },
//            error: function () {
//                //alert("failed");
//                bootbox.alert("failed");             
//            }
//        });
//    }
//}


function assignSeats(seat, id) {
    var selectedId = $('#' + seat).val();
    var GSeat = "govSeat" + id;
    var MSeat = "mngSeat" + id;
    var tradeId = "tradeId" + id;
    var tradenum = $('#' + tradeId).text();
    var totalseat = "totalseat" + id;
    if (selectedId == 'choose') {
        $('#' + GSeat).text('0');
        $('#' + MSeat).text('0');
        $('#' + totalseat).text('0');
        // alert('select seat type');
        bootbox.alert('please select seat type');
    }
    else {
        $.ajax({
            type: "GET",
            url: "/Admission/GetSeatsBySeatTypeRules",
            data: { 'seattypeId': selectedId, 'tradeId': tradenum },
            contentType: "application/json",
            success: function (data) {
                if (data != null) {                 
                      
                  $('#' + GSeat).text(data[0].Govt_seats);
                  $('#' + MSeat).text(data[0].Management_seats);
                  $('#' + totalseat).text(data[0].batchsize);                 
                }
            },
            error: function () {
                //alert("failed");
                bootbox.alert("failed");
            }
        });
    }
}






function SubmitSeatAvailability() {
  var isValid = true;
  var isValid1 = true;
    var courseType = $('#CourseTypes').val();
    var academicYear = $('#academicyear').val();
    var academicYear1 = $('#AcademicYear').val();
    var roleId = $('#users').val();
    var status = $('#status').val();
    var remarks = $('#remarks').val();
    $('#coursetypeErr').text('');
    $('#academicYearErr').text('');
    $('#roleIdErr').text('');
    $('#statusErr').text('');
    $('#remarksErr').text('');
  if (courseType == 'choose') {
    isValid1 = false;
        $('#coursetypeErr').text('select the course type');
        //bootbox.alert('Please select the course type');
  }
  if (academicYear == "") {
    isValid1 = false;
        $('#academicYearErr').text('select the session');
       // bootbox.alert('Please select the year');
  }
    if (remarks == "") {
        isValid1 = false;
        $('#remarksErr').text('Enter the remarks');        
    }
  if (isValid1 == true) {
  
  var list = null;
            var listItem = [];
            var shift_table = $("#seatAvailabilityTable tbody");

            shift_table.find('tr').each(function (len) {
                var $tr = $(this);
                var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
                var misccode = $tr.find("td:eq(2)").text();
                var tradeId = $tr.find("td:eq(8)").text();
                var shift = $tr.find("td:eq(11)").text();
                var unit = $tr.find("td:eq(12)").text();
                var seatTypeId = $tr.find("td select").val()
                var govtSeats = $tr.find("td:eq(16)").text();
                var mngSeats = $tr.find("td:eq(17)").text();
                //var batchsize = $tr.find("td:eq(21)").text();
                var dualSystem = $tr.find("td:eq(18)").text();
                 list = {
                    MISCode: misccode,
                    TradeId: tradeId,
                    Shift: shift,
                    Unit: unit,
                    SeatType: seatTypeId,
                    GovtSeatAvailability: govtSeats,
                    ManagementSeatAvailability: mngSeats,
                    DualSystemTraining: dualSystem,
                    Status: status,
                    Remarks: remarks,
                    RoleId: roleId,
                    CourseType: courseType,
                    AcademicYear: academicYear,
                    //batchsize: batchsize
              }
            
              if (chkbx == true) {
                    if (seatTypeId != "choose")
                        listItem.push(list);
                    else
                        bootbox.alert('please select the seat type');
              }
             
            });
 
    if (list.MISCode == "") {
      bootbox.alert('No data available');
      return false;
    }
    else if (listItem.length == 0) {
      bootbox.alert('please select the seat type and check the checkbox');
      return false;
    } else {
      if (roleId == "choose") {
        isValid = false;
        $('#roleIdErr').text('select the role');
        //  bootbox.alert('Please select the role');
      }

      if (status == "choose") {
        isValid = false;
        $('#statusErr').text('select the status');
        //  bootbox.alert('Please select the status');
      }

      if (isValid == true) {
        bootbox.confirm({
            message: "<br/> Are you sure you want to submit seat availability data for review to <b>" + $('#users option:selected').text() + "</b> ?",
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
                type: "POST",
                url: "/Admission/SaveSeatAvailability",
                contentType: "application/json",
                data: JSON.stringify(listItem),
                success: function (data) {
                  if (data == 'success') {
                      bootbox.alert("<br/> <b>" + $("#hdRoleName").data('value') + "</b> has Submitted Seat availabilty data of <b>" + $('#CourseTypes option:selected').text() + "</b> for <b>" + $('#academicyear').val() + "</b> session to <b>" + $('#users option:selected').text() + "</b> successfully");
                    //$('#CourseTypes').val('choose');
                    //$('#academicyear').val('');
                    $('#status').val('choose');
                    $('#remarks').val('');
                    $('#users').val('choose');
                    GetSeatAvailability();
                    FilterTable();
                      //location.reload();
                  } else if (data == 'exists') {
                    bootbox.alert("Seat availabilty data already exists");
                  }
                  else {
                    bootbox.alert("failed");
                  }
                },
                error: function (result) {
                  bootbox.alert("failed");
                }
              });
            }
          }
        });
      }
      else {
      //  bootbox.alert('Please check the input fields');
        return false;
      }
    }
    
  }
  else {
   // bootbox.alert('Please check the input fields');
    return false;
  }
        
}

function cancelSeatAvailability() {
    window.location.reload();
}

function getfilter(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        //table.search(valu).draw();
    }


    if (id == 1) {
        GetDistrictData('divisionDdp', 'districtDdp1', '1');
        if ($('#divisionDdp option:selected').text() == 'Choose')
            valu = '';
       // table.search(valu).draw();
    }
    else if (id == 2) {
        GetTaluksData('districtDdp1', 'talukDdp', '1');
        if ($('#districtDdp1 option:selected').text() == 'choose')
            valu = $('#divisionDdp option:selected').text();
        //table.search(valu).draw();
    }
    else if (id == 3) {
        GetTaluksData('talukDdp', 'instituteDdp', '2');
        if ($('#talukDdp option:selected').text() == 'choose')
            valu = $('#districtDdp1 option:selected').text();
       // table.search(valu).draw();
    }
    else {
        if ($('#instituteDdp option:selected').text() == 'choose')
            valu = $('#talukDdp option:selected').text();
       // table.search(valu).draw();
    }

}
function getfilterNew(se, id) {
    let valu = $('#' + se + ' option:selected').text();
    if (valu == 'choose') {
        valu = '';
        table.search(valu).draw();
    }
    if (id == 1) {
        GetDistrictData('divisionDdp3', 'districtDdp3', '1');
        if ($('#divisionDdp option:selected').text() == 'Choose')
            valu = '';
    }
    else if (id == 2) {
        GetTaluksData('districtDdp3', 'talukDdp3', '1');
        if ($('#districtDdp option:selected').text() == 'choose')
            valu = $('#divisionDdp option:selected').text();
    }
    else if (id == 3) {
        GetTaluksData('talukDdp3', 'instituteDdp3', '2');
        if ($('#talukDdp3 option:selected').text() == 'choose')
            valu = $('#districtDdp3 option:selected').text();
    }
    else {
        if ($('#instituteDdp3 option:selected').text() == 'choose')
            valu = $('#talukDdp3 option:selected').text();

    }

}
//function getfilter(se, id) {
//    let valu = $('#' + se + ' option:selected').text();
//    if (valu == 'choose') {
//        valu = '';
//        table.search(valu).draw();
//    }


//    if (id == 1) {
//        GetDistrictData('divisionDdp', 'districtDdp1', '1');
//        if ($('#divisionDdp option:selected').text() == 'Choose')
//            valu = '';
//        table.search(valu).draw();
//    }
//    else if (id == 2) {
//        GetTaluksData('districtDdp1', 'talukDdp', '1');
//        if ($('#districtDdp1 option:selected').text() == 'choose')
//            valu = $('#divisionDdp option:selected').text();
//        table.search(valu).draw();
//    }
//    else if (id == 3) {
//        GetTaluksData('talukDdp', 'instituteDdp', '2');
//        if ($('#talukDdp option:selected').text() == 'choose')
//            valu = $('#districtDdp1 option:selected').text();
//        table.search(valu).draw();
//    }
//    else {
//        if ($('#instituteDdp option:selected').text() == 'choose')
//            valu = $('#talukDdp option:selected').text();
//        table.search(valu).draw();
//    }

//}
//=============End Tab 1==================

//=============Satrt Tab 2==================

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
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetDistrictData(dividdp, distddp) {
    var divi = $("#" + dividdp).val();
    $('#' + distddp).empty();
    $('#' + distddp).append('<option value="choose">choose</option>');
    if (divi == 'choose') {
        bootbox.alert('select division');
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
                //alert("Error", "something went wrong");
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}
function GetTaluksData(ddpId, tlk, cityId) {

    var lgdCode = $("#" + ddpId).val();
    if (lgdCode == 'choose') {
        bootbox.alert('select district');
    }
    else {
        $("#" + tlk).empty();
        if (tlk != "instituteDdp") {
            $("#" + tlk).append('<option value="choose">choose</option>');
        }
        else {
            $("#instituteDdp").empty();
            $("#instituteDdp").append('<option value="choose">choose</option>');
        }
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
                //alert("Error", "something went wrong");
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}
function FilterTable() {
    var TabId = 0;
    var district = $('#districtDdp :selected').val();
    var taluk = $('#talukDdp :selected').val();
    var Institute = $('#instituteDdp :selected').val();
    var year = $("#Session").val();
    var courseTyp = $("#CourseType2").val();
    var division = $("#divisionDdp").val();

    if (RoleId == loginUserRole.DD) {
        district = $('#districtDdp1 :selected').val();
    }
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAvailabilityListStatusFilter",
        data: { 'TabId': TabId, 'courseType': courseTyp, 'academicYear': year, 'division': division, 'district': district, 'taluk': taluk, 'Institute': Institute },
        contentType: "application/json",
        success: function (data) {
            table = $('#seatAvailabilityStatusTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        filename: "SeatAvailabilityStatus",
                        title: "Seat Availability Status",
                        exportOptions: {
                            columns: [0, 1, 2, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17]
                        }
                    },
                    {
                        extend: 'pdf',
                        text: 'Download as Pdf',
                        orientation: 'landscape',
                        pageSize: 'LEGAL',
                        filename: "SeatAvailabilityStatus",
                        title: "Seat Availability Status",
                        exportOptions: {
                            columns: [0, 1, 2, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center' },
                    {
                        'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-center',
                        "render": function (nTd, sData, oData, iRow, iCol) {
                            const date = new Date(new Date().getFullYear(), 7, 1);
                            const month = date.toLocaleString('default', { month: 'short' });
                            const year = date.getFullYear();
                            return (month + "-" + year);
                        }
                    },
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-center taluk' },
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-center taluk' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-center taluk' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },
                    //{
                    //    'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-left',

                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(new Date().getFullYear(), 7, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return (month + "/" + year + "-" + month + "/" + (year + parseInt(oData.Duration)));
                    //    }
                    //},
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'SeatName', 'title': 'Seat Type', 'className': 'text-left' },
                    { 'data': 'batchsize', 'title': 'Batch Size', 'className': 'text-left' },
                    { 'data': 'GovtSeatAvailability', 'title': 'Govt Seat Availability', 'className': 'text-left' },
                    { 'data': 'ManagementSeatAvailability', 'title': 'Management Seat Availability', 'className': 'text-left' },
                    { 'data': 'Status', 'title': 'Status' + '- Currently with', 'className': 'text-left' },

                    {
                        'data': 'Id', 'className': 'text-center', 'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.RoleId == 9) {

                                if (oData.StatusId == 5 || oData.StatusId == 4)
                                    $(nTd).html("<button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>");
                                else if (oData.StatusId == 3)
                                    $(nTd).html("<button class='btn btn-secondary btn-xs' onclick='EditSeatDetails(" + oData.Id + ")' >Edit</button><button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>  <button class='btn btn-danger btn-xs' onclick='deleteUnitShiftDetails(" + oData.Id + ")' >Delete</button>");
                                else
                                    /*$(nTd).html("<button class='btn btn-secondary btn-xs' onclick='EditSeatDetails(" + oData.Id + ")' >Edit</button><button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>");*/
                                    $(nTd).html("<button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>");
                            }
                            else {

                                $(nTd).html("<button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>");
                            }
                        }
                    },
                    {
                        'data': 'Id', 'title': 'Remarks', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<button class='btn btn-primary btn-xs' onclick='GetRemarks(" + oData.Id + ")'>View</button>");
                        }
                    },
                    {
                        'data': 'Id', 'title': 'Select', 'className': 'text-center',
                        "sTitle": "<span>Select</span><input type='checkbox' id=cbxCheckAll onclick='SelectAllChkBox(this.id)'></input>", "bSortable": false,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.RoleId == 9) {
                                $(nTd).html("<input type='checkbox' disabled='disabled' name='VerifyCheck' id='VerifyCheckbx'/><input type='hidden' value='" + oData.Id + "'>");
                            }
                            else {
                                if (oData.FlowId == oData.RoleId) {

                                    if (oData.StatusId == 4)
                                        $(nTd).html("<input type='checkbox' disabled='disabled' name='VerifyCheck' id='VerifyCheckbx'/><input type='hidden' value='" + oData.Id + "'>");
                                    else
                                        $(nTd).html("<input type='checkbox' name='VerifyCheck' id='VerifyCheckbx'/><input type='hidden' value='" + oData.Id + "'>");
                                }
                                else {
                                    $(nTd).html("<input type='checkbox' disabled='disabled' name='VerifyCheck' id='VerifyCheckbx'/><input type='hidden' value='" + oData.Id + "'>");
                                }

                            }
                        }
                    },
                ],
            });
            //table.column(21).visible(false);
            if (data != null || data.length > 0) {

                for (let i = 0; i <= data.length - 1; i++) {
                    if (data[i].RoleId == 9)
                        table.column(20).visible(false);

                    if (data[i].FlowId == loginUserRole.DD && RoleId == loginUserRole.DD) {
                        $('#approveDD').show();
                        break;
                    }
                    if (data[i].FlowId == loginUserRole.JDDiv && RoleId == loginUserRole.JDDiv) {
                        $('#forwardJd').show();
                        break;
                    }
                }
                //$('#seatAvailabilityStatusTable tr:eq(0) th:eq(16)').text("Action");
                //else
                // $('#seatAvailabilityStatusTable tr:eq(0) th:eq(19)').text("Details");
            }
            else {
                // $('#seatAvailabilityStatusTable tr:eq(0) th:eq(17)').remove();
            }

        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });

}

function btnsearchFiltertable(id) {
    FilterTable();
}



function SelectAllChkBox(id) {
    //Checkboxes select all
    // Get all rows with search applied
    var rows = $('#seatAvailabilityStatusTable').DataTable().rows({ 'search': 'applied' }).nodes();
    // Check/uncheck checkboxes for all rows in the table
    if ($('#' + id).is(":checked")) {
        $('input[type="checkbox"]:enabled', rows).prop('checked', true);
    } else {
        $('input[type="checkbox"]:enabled', rows).prop('checked', false);
    }
}
function ForwardToDD(id) {
    var isValid = true;
    var roleId = $('#DepartmentUsers').val();
    var remark = $('#jdremarks').val();
    var listItem = [];
    var table = $("#seatAvailabilityStatusTable tbody");

    table.find('tr').each(function (len) {
        var $tr = $(this);
        var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
        var id = $tr.find("td input[type=hidden]").val();
        var status = $('#statusdp').val();
        var list = {
            TradeId: id,
            RoleId: roleId,
            Remarks: remark,
            Status: status
        }
        if (chkbx == true) {
            listItem.push(list);
        }
    });

    if (listItem.length == 0) {
        isValid1 = false;
        bootbox.alert("<br/> Please select the record");
    }
    else if (remark == "") {
        isValid = false;
        $('#jdremarksErr').text('Enter the remarks');
    }
    else if (isValid == true) {
        $('#jdremarksErr').hide();
        if (id == 1) {
            if (roleId == 'choose') {
                bootbox.alert("<br/> Select the role whom do you want to send");
            }
            else {
                var msg = "";
                if ($('#statusdp').val() == 2)
                    msg = "<br/> Are you sure you want to submit seat availability data for review and approve to <b>" + $('#DepartmentUsers option:selected').text() + "</b> ?";
                else
                    msg = "<br/> Are you sure you want to send back seat availability data to <b>" + $('#DepartmentUsers option:selected').text() + "</b> for more clarification ?";
                bootbox.confirm({
                    message: msg,
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
                                type: "POST",
                                url: "/Admission/ForwardSeatAvailability",
                                contentType: "application/json",
                                data: JSON.stringify(listItem),
                                success: function (data) {
                                    if (data == true) {
                                        if ($('#statusdp').val() == 2) {
                                            bootbox.alert("<br/><b>Joint Director</b> has submitted seat availability data successfully to <b>" + $('#DepartmentUsers option:selected').text() + "</b> for review");
                                        }
                                        else if ($('#statusdp').val() == 5) {
                                            bootbox.alert("<br/><b>Joint Director</b> has rejected seat availability data successfully");
                                        }
                                        else if ($('#statusdp').val() == 3) {
                                            bootbox.alert("<br/><b>Joint Director</b> successfully requested for more clarification on seat availability data from <b>" + $('#DepartmentUsers option:selected').text() + "</b>");
                                        }
                                        GetSeatAvailability();
                                        FilterTable();
                                        $('#statusdp').val('choose');
                                        $('#jdremarks').val('');
                                        $('#DepartmentUsers').val('choose');
                                    }
                                    else {
                                        bootbox.alert($('#statusdp option:selected').text() + " failed");
                                    }
                                },
                                error: function (result) {
                                    bootbox.alert("failed");
                                }
                            });
                        }
                    }
                });
            }
        }
        else {
            if (roleId == 'choose') {
                bootbox.alert("<br/> Select the role");
            }
            else {
                var msg = "";
                if ($('#statusdp').val() == 2)
                    msg = "<br/> Are you sure you want to submit seat availability data for review and approve to <b>" + $('#DepartmentUsers option:selected').text() + "</b> ?";
                else
                    msg = "<br/> Are you sure you want to send back seat availability data to <b>" + $('#DepartmentUsers option:selected').text() + "</b> for more clarification ?";

                bootbox.confirm({
                    message: msg,
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
                                type: "POST",
                                url: "/Admission/ForwardSeatAvailability",
                                contentType: "application/json",
                                data: JSON.stringify(listItem),
                                success: function (data) {
                                    if (data == true) {
                                        if ($('#statusdp option:selected').text() == "Reviewed and Recommend") {
                                            bootbox.alert("<br/><b> Joint Director </b> has submitted seat availability data successfully and " + $('#statusdp option:selected').text() + " To <b> " + $('#DepartmentUsers option:selected').text()) + "</b>";
                                        }
                                        else if ($('#statusdp option:selected').text() == "Rejected") {
                                            bootbox.alert("<br/><b>Joint Director</b> has rejected seat availability data successfully");
                                        }
                                        else if ("Send Back for corrections") {
                                            bootbox.alert("<br/><b>Joint Director</b> successfully requested for more clarification on seat availability data from <b>" + $('#DepartmentUsers option:selected').text() + "</b>");
                                        }
                                        GetSeatAvailability();
                                        FilterTable();
                                        $('#statusdp').val('choose');
                                        $('#jdremarks').val('');
                                        $('#DepartmentUsers').val('choose');
                                    }
                                    else {
                                        bootbox.alert($('#statusdp option:selected').text() + " failed");
                                    }
                                },
                                error: function (result) {
                                    bootbox.alert("failed");
                                }
                            });
                        }
                    }
                });
            }
        }

    }
    if (isValid == true) {
        $('#jdremarksErr').hide();
    }
    else {
        $('#jdremarksErr').show();
    }
}

function GetRemarks(seatId) {
    $.ajax({
        type: "GET",
        url: "/Admission/GetRemarks",
        contentType: "application/json",
        data: { 'seatId': seatId },
        success: function (data) {
            $('#RemarksTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'RoleId', 'title': 'Sl.No.', 'className': 'text-center' },
                    //{
                    //    'data': 'Date',
                    //    'title': 'Date',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        var date = daterangeformate2(oData.Date, 1);
                    //        $(nTd).html(date);
                    //    }
                    //},
                    { 'data': 'Date', 'title': 'Date', 'className': 'text-center' },
                    {
                        'data': 'From', 'title': 'From', 'className': 'text-center',
                        "render": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.From.includes(loginUserRole.DDText))
                                return loginUserRole.DDAdmCellText;
                            else
                                return oData.From;
                        }
                    },
                    {
                        'data': 'To', 'title': 'To', 'className': 'text-center',
                        "render": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.From.includes(loginUserRole.DDText))
                                return loginUserRole.DDAdmCellText;
                            else
                                return oData.To;
                        }
                    },
                    { 'data': 'StatusName', 'title': 'Status', 'className': 'text-center' },
                    { 'data': 'Remarks', 'title': 'Remarks', 'className': 'text-left' },
                ]
            });
            $('#RemarksModal').modal('show');
        },
        error: function () {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function ApproveSeatAvailability() {
    var isValid = true;
    var remark = $('#ddremarks').val();
    var listItem = [];
    var table = $("#seatAvailabilityStatusTable tbody");
    var roleId = $('#sentTo').val();
    var statusId = $('#statusDD').val();
    $('#ddremarksErr').hide();
    $("#statusDDErr").hide();

    table.find('tr').each(function (len) {
        var $tr = $(this);
        var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
        var id = $tr.find("td input[type=hidden]").val();
        var status = $('#statusDD').val();
        var list = {
            TradeId: id,
            Remarks: remark,
            Status: status
        }
        if (chkbx == true) {
            listItem.push(list);
        }
    });

    if (listItem.length == 0) {
        isValid = false;
        bootbox.alert("Please select the record");
    }
    if (remark=="") {
        isValid = false;      
        $("#ddremarksErr").show();
    }
    if (statusId == "choose") {
        isValid = false;
        $("#statusDDErr").show();
    }
    else if (isValid == true) {
        if (roleId == 'choose' && statusId==3) {
            bootbox.alert("<br/> Select the role");
        }
        else {
            var msg = "";
            if ($('#statusDD').val() == 4)
                msg = "<br/> Are you sure you want to approve seat availability data ?";
            else
                msg = "<br/> Are you sure you want to send back seat availability data to <b>" + $('#sentTo option:selected').text() + "</b> for more clarification ?";
            bootbox.confirm({
                message: msg,
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
                        type: "POST",
                        url: "/Admission/ApproveSeatAvailability",
                        contentType: "application/json",
                        data: JSON.stringify(listItem),
                        success: function (data) {
                            if (data == true) {
                                if ($('#statusDD').val() == 4) {
                                    bootbox.alert("<br/><b>Deputy Director</b> Approved seat availability data Successfuly ");
                                } else if ($('#statusDD').val() == 3) {
                                    bootbox.alert("<br/><b>Deputy Director</b> Successfuly requested for more clarification on seat availability data from <b>" + $('#sentTo option:selected').text() + "</b>");
                                }
                                else {
                                    bootbox.alert("<br/><b>Deputy Director</b> Rejected the seat availability data Successfuly");
                                        
                                }

                                $('#statusDD').val('choose');
                                $('#ddremarks').val('');
                                GetSeatAvailability();
                                FilterTable();
                            }
                            else {
                                bootbox.alert($('#statusDD option:selected').text() + " failed");
                            }
                        },
                        error: function (result) {
                            bootbox.alert("failed");
                        }
                    });
                }
            }
        });
    }
    }
}

function GetSeatViewDetails(seatId) {
    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatViewDetails",
        contentType: "application/json",
        data: { 'seatId': seatId },
        success: function (data) {
            $('#SeatViewTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                "paging": false,
                "ordering": false,
                "info": false,
                bFilter: false,
                columns: [
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center' },
                    //{ 'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-center' },
                   // { 'data': 'DivisionId', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                   // { 'data': 'DistrictId', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                    //{ 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                   // { 'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'DualSystemTraining', 'title': 'DualSystemTraining', 'className': 'text-left' },
                    { 'data': 'batchsize', 'title': 'Batch Size', 'className': 'text-left' },
                    { 'data': 'SeatName', 'title': 'Seat Type', 'className': 'text-left' },
                    { 'data': 'GovtSeatAvailability', 'title': 'Govt Seat Availability', 'className': 'text-left' },
                    { 'data': 'ManagementSeatAvailability', 'title': 'Management Seat Availability', 'className': 'text-left' },
                ]
            });
            $('#SeatViewModal').modal('show');
        },
        error: function () {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function EditSeatDetails(seatId) {
    $("#remarksEdit").val('');

    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatViewDetails",
        contentType: "application/json",
        data: { 'seatId': seatId },
        success: function (data) {
            var id = 1;
            var tradid = 1;
            $('#SeatEditTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,

                "paging": false,
                "ordering": false,
                "info": false,
                bFilter: false,
                columns: [
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center miscode' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center miscode' },
                   // { 'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-left' },
                   // { 'data': 'DivisionId', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                    //{ 'data': 'DistrictId', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                   // { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                    {
                        'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var trade = "trade" + tradid;
                            $(nTd).html("<span id='" + trade + "'>" + oData.TradeId + "</span><input type='hidden' id='ITISeatId' value='" + oData.Id + "'/>");
                            tradid++;
                        }
                    },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },                    
                    {
                        'data': 'Unit', 'title': 'Eligible For Current Admission', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='checkbox' checked name='seatCheckbx' id='EligAdmissions1'/>");

                        }
                    },
                    {
                        'data': 'Unit', 'title': 'SeatType', 'className': 'text-left', 'width': '200%',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var sear = "seatTy" + id;
                            $(nTd).html("<select  style='width:80px;' class='form-control' id='seatTypeEdit' onchange='EditassignSeats(\"" + sear + "\",\"" + id + "\")'><option value='choose'>choose</option></select>");
                            GetSeatTypes("seatTypeEdit");
                            id++;
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Govt Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<span id='govmtSeat1'></span>");
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Management Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<span id='mngmtSeat1'></span>");
                        }
                    },
                    { 'data': 'DualSystemTraining', 'title': 'Dual System Training', 'className': 'text-left' }
                ]
            });
            if (data != null) {
                $('#govmtSeat1').text(data[0].GovtSeatAvailability);
                $('#mngmtSeat1').text(data[0].ManagementSeatAvailability);
                $('#CourseTypesEdit').val(data[0].CourseType);
                $('#academicyearEdit').val(data[0].AcademicYearString);
                $('#usersEdit').val('choose');
                if (data[0].FlowId == 5 || data[0].FlowId == 16)
                    $("#usersEdit").attr("disabled", true);    

                if (data[0].FlowId == 5 || data[0].FlowId == 16)
                    $("#statusEdit").attr("disabled", true);
                $('#seatTypeEdit option:selected').val(data[0].SeatTypeId).text(data[0].SeatName);
                //$('#remarksEdit').val(data[0].Remarks);
                $('#SeatEditModal').modal('show');

            }
            else {
                //alert("failed");
                bootbox.alert("failed");
            }
        },
        error: function () {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
$('a[href="#tab_1"]').click(function () {
    GetSeatAvailability();
});

function deleteUnitShiftDetails(seatId) {
    var roleId = $('#DepartmentUsers').val();
    var remark = $('#jdremarks').val();
    var listItem = [];
    var table = $("#seatAvailabilityStatusTable tbody");
    table.find('tr').each(function (len) {
        var $tr = $(this);
        //var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
        var id = $tr.find("td input[type=hidden]").val();
        var status = $('#statusdp').val();
        var list = {
            TradeId: id,
            RoleId: roleId,
            Remarks: remark,
            Status: status,
            seatId: seatId
        }
        if (seatId != null) {
            listItem.push(list);
        }
    });
    bootbox.confirm({
        message: "<br/> Are you sure you want to delete record ?",
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
                    type: "GET",
                        url: "/Admission/delUnitShiftDetails",
                    contentType: "application/json",
                    data: { 'seatId': seatId },
                        //type: "POST",
                        //url: "/Admission/delUnitShiftDetails",
                        //contentType: "application/json",
                        //data: { seatId: seatId },/* JSON.stringify(listItem),*/
                    success: function (data) {
                        if (data == true) {
                            FilterTable();
                            bootbox.alert("<br/><b>" + $("#hdRoleName").data('value') +"</b> has deleted seat availability data successfully.");
                            $('#SeatEditModal').modal('hide');
                        }
                        else {
                            bootbox.alert("failed");
                        }
                        },
                        error: function (result) {
                            bootbox.alert("failed");
                    }
                });
            }
        }
    });  
    
}

//function EditassignSeats1(seat, id) {
//    var selectedId = $('#seatTypeEdit').val();
//    var GSeat = "govmtSeat1";
//    var MSeat = "mngmtSeat1";
//    var tradeId = "trade" + id;
//    var tradenum = $('#' + tradeId).text();
//    if (selectedId == 'choose') {
//        $('#' + GSeat).text('0');
//        $('#' + MSeat).text('0');
//       // alert('select seat type');
//        bootbox.alert("Please select seat type");
//    } else {

//        $.ajax({
//            type: "GET",
//            url: "/Admission/GetSeatsByTradeIdSeatType",
//            data: { 'tradeId': tradenum },
//            contentType: "application/json",
//            success: function (data) {
//                if (data != null) {
//                    if (selectedId == 1 || selectedId == 3) {
//                        var govts = data - 4;
//                        $('#' + GSeat).text(govts);
//                        $('#' + MSeat).text('4');
//                    }
//                    else if (selectedId == 2) {
//                        $('#' + GSeat).text(data);
//                        $('#' + MSeat).text('0');
//                    }
//                    else if (selectedId == 5 || selectedId == 4) {
//                        $('#' + GSeat).text(0);
//                        $('#' + MSeat).text(data);
//                    }
//                }
//            },
//            error: function () {
//                //alert("failed");
//                bootbox.alert("failed");
//            }
//        });
//    }
//}

function EditassignSeats(seat, id) {
    var selectedId = $('#seatTypeEdit').val();
    var GSeat = "govmtSeat1";
    var MSeat = "mngmtSeat1";
    var tradeId = "trade" + id;
    var totalseat = "totalseat" + id;
    var tradenum = $('#' + tradeId).text();
    if (selectedId == 'choose') {
        $('#' + GSeat).text('0');
        $('#' + MSeat).text('0');
        $('#' + totalseat).text('0');
        bootbox.alert("Please select seat type");
    } else {

        $.ajax({
            type: "GET",
            url: "/Admission/GetSeatsBySeatTypeRules",
            data: { 'seattypeId': selectedId, 'tradeId': tradenum},
            contentType: "application/json",
            success: function (data) {
                if (data != null) {
                    $('#' + GSeat).text(data[0].Govt_seats);
                    $('#' + MSeat).text(data[0].Management_seats);
                    //$('#' + totalseat).text(data[0].batchsize);
                }
            },
            error: function () {
                bootbox.alert("failed");
            }
        });
    }
}

function UpdateSeatAvailability() {
  var isvalid = true;
    var chk = $('#EligAdmissions1').is(":checked");
    if (chk == true) {
        var courseType = $('#CourseTypesEdit').val();
        var academicYear = $('#academicyearEdit').val();
        var roleId = $('#usersEdit').val();
        var remarks = $('#remarksEdit').val();
      var statusEdit = $('#statusEdit').val();
        $('#CourseTypesEditErr').text('');
        $('#academicyearEditErr').text('');
        $('#usersErr').text('');
      if (courseType == 'choose') {
        isvalid = false;
            $('#CourseTypesEditErr').text('select the course type');
           // bootbox.alert('select the course type');
        }
      else if (academicYear == "") {
        isvalid = false;
            $('#academicyearEditErr').text('select the year');
          //  bootbox.alert('select the year');
        }
      if (statusEdit == "choose") {
        isvalid = false;
        $('#statusErrs').text('select the status');
           // bootbox.alert('select the status');
        }
       
        if (remarks == "") {
            isvalid = false;
            $('#remarksEditErrs').text('Enter the remarks');
            //bootbox.alert('Enter the remarks');
        }
        if (roleId == "choose") {
          isvalid = false;
                $('#usersErr').text('select the role');
               // bootbox.alert('select the role');
            }
    
      if (isvalid == true) {
        var seatId = $('#ITISeatId').val();
        var status = $('#statusEdit').val();
        var remarks = $('#remarksEdit').val();
        var listItem = [];
        var shift_table = $("#SeatEditTable tbody");

        shift_table.find('tr').each(function (len) {
          var $tr = $(this);
          var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
          var misccode = $tr.find("td:eq(0)").text();
          var tradeId = $tr.find("td:eq(7)").text();
          var unit = $tr.find("td:eq(11)").text();
          var shift = $tr.find("td:eq(10)").text();
          var seatTypeId = $tr.find("td select").val();
          var govtSeats = $tr.find("td:eq(14)").text();
          var mngSeats = $tr.find("td:eq(15)").text();
          var dualSystem = $tr.find("td:eq(16)").text();
          var list = {
            MISCode: misccode,
            TradeId: seatId,
            Shift: shift,
            Unit: unit,
            SeatType: seatTypeId,
            GovtSeatAvailability: govtSeats,
            ManagementSeatAvailability: mngSeats,
            DualSystemTraining: dualSystem,
            Status: status,
            Remarks: remarks,
            RoleId: roleId,
            CourseType: courseType,
            AcademicYear: academicYear,
            IsChecked: true
          }
          if (seatTypeId != "choose" && chkbx == true) {
            listItem.push(list);
          }
        });

        $.ajax({
          type: "POST",
          url: "/Admission/UpdateSeatAvailability",
          contentType: "application/json",
          data: JSON.stringify(listItem),
          success: function (data) {
            if (data == true) {
              FilterTable();
                bootbox.alert("<br/><b>" + $("#hdRoleName").data('value') +"</b> has updated Seat availabilty data of <b>" + $('#CourseTypesEdit option:selected').text() + "</b> for <b>" + $('#academicyearEdit').val() + "</b> session and submitted for review to <b>" + $('#usersEdit option:selected').text() + "</b> successfully");
              $('#SeatEditModal').modal('hide');
            }
            else {
              bootbox.alert("failed");
            }
          },
          error: function (result) {
            bootbox.alert("failed");
          }
        });
      }
      else {
       // bootbox.alert('Please check input fields');
        return false;
      }
    }
    else {
        var seatId = $('#ITISeatId').val();
        var listItem = [];
        var shift_table = $("#SeatEditTable tbody");
        shift_table.find('tr').each(function (len) {
            var $tr = $(this);
            var misccode = $tr.find("td:eq(0)").text();
            var tradeId = $tr.find("td:eq(7)").text();
            var shift = $tr.find("td:eq(10)").text();
            var unit = $tr.find("td:eq(11)").text();
            var seatTypeId = $tr.find("td select").val();
            var govtSeats = $tr.find("td:eq(14)").text();
            var mngSeats = $tr.find("td:eq(15)").text();
            var dualSystem = $tr.find("td:eq(16)").text();
            var list = {
                MISCode: misccode,
                TradeId: seatId,
                Shift: shift,
                Unit: unit,
                SeatType: seatTypeId,
                GovtSeatAvailability: govtSeats,
                ManagementSeatAvailability: mngSeats,
                DualSystemTraining: dualSystem,
                Status: status,
                Remarks: remarks,
                RoleId: roleId,
                CourseType: courseType,
                AcademicYear: academicYear,
                IsChecked: false
            }
            listItem.push(list);
        });

        $.ajax({
            type: "POST",
            url: "/Admission/UpdateSeatAvailability",
            contentType: "application/json",
            data: JSON.stringify(listItem),
            success: function (data) {
                if (data == true) {
                    FilterTable();
                    bootbox.alert("<br/><b>" + $("#hdRoleName").data('value') +"</b> has cancelled seat availabilty data successfully");
                    $('#SeatEditModal').modal('hide');
                }
                else {
                    bootbox.alert("failed");
                }
            },
            error: function (result) {
                bootbox.alert("failed");
            }
        });
    }
}
function ApprovedUpdateSeatAvailability() {
  var isvalid = true;
    var chk = $('#EligAdmissions1').is(":checked");
    if (chk == true) {
        var courseType = $('#CourseTypesEdit').val();
        var academicYear = $('#academicyearEdit').val();
        var remarks = $('#remarksEdit').val();
        $('#CourseTypesEditErr').text('');
        $('#academicyearEditErr').text('');
       
      if (courseType == 'choose') {
        isvalid = false;
            $('#CourseTypesEditErr').text('select the course type');
            bootbox.alert('select the course type');
        }
      if (academicYear == "") {
        isvalid = false;
            $('#academicyearEditErr').text('select the year');
            bootbox.alert('select the year');
        }     
      if (remarks == "") {
            isvalid = false;
            $('#remarksEditErrs').text('Enter the remarks');
            bootbox.alert('Enter the remarks');
        }
           
      if (isvalid == true) {
        var seatId = $('#ITISeatId').val();
        var status = $('#statusEdit').val();
        var remarks = $('#remarksEdit').val();
        var listItem = [];
        var shift_table = $("#SeatEditTable tbody");

        shift_table.find('tr').each(function (len) {
          var $tr = $(this);
          var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
            var misccode = $tr.find("td:eq(0)").text();
            var tradeId = $tr.find("td:eq(7)").text();
            var unit = $tr.find("td:eq(11)").text();
            var shift = $tr.find("td:eq(10)").text();
            var seatTypeId = $tr.find("td select").val();
            var govtSeats = $tr.find("td:eq(14)").text();
            var mngSeats = $tr.find("td:eq(15)").text();
            var dualSystem = $tr.find("td:eq(16)").text();
          var list = {
            MISCode: misccode,
            TradeId: seatId,
            Shift: shift,
            Unit: unit,
            SeatType: seatTypeId,
            GovtSeatAvailability: govtSeats,
            ManagementSeatAvailability: mngSeats,
            DualSystemTraining: dualSystem,
            Status: status,
            Remarks: remarks,
            //RoleId: roleId,
            CourseType: courseType,
            AcademicYear: academicYear,
            IsChecked: true
          }
          if (seatTypeId != "choose" && chkbx == true) {
            listItem.push(list);
          }
        });

        $.ajax({
          type: "POST",
          url: "/Admission/UpdateSeatAvailability",
          contentType: "application/json",
          data: JSON.stringify(listItem),
          success: function (data) {
              if (data == true) {
                
              FilterTable();
                  bootbox.alert("<br/><b>" + $("#hdRoleName").data('value') +"</b> has updated seat availabilty data of <b>" + $('#CourseTypesEdit option:selected').text() + "</b> for <b>" + $('#academicyearEdit').val() + "</b> session successfully");
              $('#SeatEditModal').modal('hide');
                  seatAvailabilityDataNew();
            }
            else {
              bootbox.alert("failed");
            }
          },
          error: function (result) {
            bootbox.alert("failed");
          }
        });
      }
      else {
        bootbox.alert('Please check input fields');
        return false;
      }
    }
    else {
        var seatId = $('#ITISeatId').val();
        var listItem = [];
        var shift_table = $("#SeatEditTable tbody");
        shift_table.find('tr').each(function (len) {
            var $tr = $(this);
            var misccode = $tr.find("td:eq(0)").text();
            var tradeId = $tr.find("td:eq(13)").text();
            var shift = $tr.find("td:eq(15)").text();
            var unit = $tr.find("td:eq(14)").text();
            var seatTypeId = $tr.find("td select").val();
            var govtSeats = $tr.find("td:eq(18)").text();
            var mngSeats = $tr.find("td:eq(19)").text();
            var dualSystem = $tr.find("td:eq(20)").text();
            var list = {
                MISCode: misccode,
                TradeId: seatId,
                Shift: shift,
                Unit: unit,
                SeatType: seatTypeId,
                GovtSeatAvailability: govtSeats,
                ManagementSeatAvailability: mngSeats,
                DualSystemTraining: dualSystem,
                Status: status,
                Remarks: remarks,
                RoleId: roleId,
                CourseType: courseType,
                AcademicYear: academicYear,
                IsChecked: false
            }
            listItem.push(list);
        });

        $.ajax({
            type: "POST",
            url: "/Admission/UpdateSeatAvailability",
            contentType: "application/json",
            data: JSON.stringify(listItem),
            success: function (data) {
                if (data == true) {
                    FilterTable();
                    bootbox.alert("<br/><b>" + $("#hdRoleName").data('value') +"</b> has cancelled seat availabilty data successfully");
                    $('#SeatEditModal').modal('hide');
                }
                else {
                    bootbox.alert("failed");
                }
            },
            error: function (result) {
                bootbox.alert("failed");
            }
        });
    }
}

$("#ExcelExport").click(function () {
    $("#seatAvailabilityStatusTable").table2excel({
        filename: "Seatavailability.xls"
    });
});

function disableRole() {

    var ss = $('#statusdp option:selected').val();
    //if (ss == 'Send for correction/clarification' || ss == 'Rejected') {
    if (ss == 3) {
        $('#DepartmentUsers').removeAttr('disabled');
        $('#DepartmentUsers').val('choose');
        GetRoles("DepartmentUsers", 9);
        //$('#DepartmentUsers').attr('disabled', 'disabled');
        $('#submitbtn').show();
        $('#Forwadbtn').hide();

    }
    else if (ss == 2) {
        $('#DepartmentUsers').removeAttr('disabled');
        $('#DepartmentUsers').val('choose');
        GetRoles1("DepartmentUsers", 5);
        $('#submitbtn').hide();
        $('#Forwadbtn').show();
    }
    else {
        $('#DepartmentUsers').attr('disabled', 'disabled');
        //$('#DepartmentUsers').val('choose');
    }
}
function onchangeStatusSentTo() {

    var status = $('#statusDD option:selected').val();
    if (status == 3) {
        $('#hideSentoDD').show();
    }
    else {
        /*$('#sentTo').val(0);*/
        $('#hideSentoDD').hide();
        $('#sentTo').val('choose');
    }
}
/*=============End Tab 2==================*/
/*=============Start Tab 3==================*/

function seatAvailabilityDataNew() {

    var TabId = 1;

    var year = $('#Session3 :selected').val();
    var courseTyp = $('#CourseType3 :selected').val();
    var division = $('#divisionDdp3 :selected').val();
    var district = $('#districtDdp3 :selected').val();
    var taluk = $('#talukDdp3 :selected').val();
    var Institute = $('#instituteDdp3 :selected').val();

    $.ajax({
        type: "GET",
        url: "/Admission/GetSeatAvailabilityListStatusFilter",
        //data: { 'TabId': TabId, 'courseType': courseTyp, 'academicYear': year },
        data: { 'TabId': TabId, 'courseType': courseTyp, 'academicYear': year, 'division': division, 'district': district, 'taluk': taluk, 'Institute': Institute },
        contentType: "application/json",
        success: function (data) {
       
            table = $('#seatAvailabilityStatusTableTab3').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        text: 'Download as Excel',
                        filename: "SeatAvailabilityStatus",
                        title: "Seat Availability Status",
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
                        }
                    },
                    {
                        extend: 'pdf',
                        text: 'Download as Pdf',
                        orientation: 'landscape',
                        filename: "SeatAvailabilityStatus",
                        title: "Seat Availability Status",
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]
                        }
                    }
                ],
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center' },
                    { 'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-center' },
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-center taluk' },
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-center taluk' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-center taluk' },
                    { 'data': 'MISCode', 'title': 'MIS ITI Code', 'className': 'text-center' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },                   
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    { 'data': 'SeatName', 'title': 'Seat Type', 'className': 'text-left' },
                    { 'data': 'batchsize', 'title': 'Batch Size', 'className': 'text-left' },
                    { 'data': 'GovtSeatAvailability', 'title': 'Govt Seat Availability', 'className': 'text-left' },
                    { 'data': 'ManagementSeatAvailability', 'title': 'Management Seat Availability', 'className': 'text-left' },
                    { 'data': 'Status', 'title': 'Status' + '- Currently with', 'className': 'text-left' },
                    {
                        'data': 'Id', 'className': 'text-center', 'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            /*if (oData.RoleId == loginUserRole.DD) {*/
           
                            if (oData.IsActive == true)
                                $(nTd).html("<button class='btn btn-secondary btn-xs' hidden " + (oData.RoleId != loginUserRole.DD ? 'style="display: none";' : '') + " onclick='EditSeatDetails(" + oData.Id + ")' >Edit</button><button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>  <button class='btn btn-danger btn-xs'  "+ (oData.RoleId != loginUserRole.DD ? 'style="display: none";' : '') + " onclick='deActiveSeatDetails(" + oData.Id + "," + oData.TradeItiId + " )' >DeActive</button>");
                            else
                                $(nTd).html("<button class='btn btn-secondary btn-xs' onclick='EditSeatDetails(" + oData.Id + ")' disabled>Edit</button><button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'disabled>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>  <button class='btn btn-danger btn-xs' onclick='deActiveSeatDetails(" + oData.Id + "," + oData.TradeItiId + " )' disabled>DeActive</button>");
                           /* }*/
                            //else {
                            //    $(nTd).html("<button class='btn btn-primary btn-xs' id='viewbtn' onclick='GetSeatViewDetails(" + oData.Id + ")'>View<span class='tooltiptext'> Review the seat availability data <br/> in concurrence with MIS portal</span></button>");
                            //}
                        }
                    },
                    {
                        'data': 'Id', 'title': 'Remarks', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<button class='btn btn-primary btn-xs' onclick='GetRemarks(" + oData.Id + ")'>View</button>");
                        }
                    },                    
                ]
            });
            //table.column(21).visible(false);
            if (data[0] != undefined) {
               // if (data[0].RoleId == 9)
                   // $('#seatAvailabilityStatusTableTab3 tr:eq(0) th:eq(16)').text("Action");
               // else
                 //   $('#seatAvailabilityStatusTableTab3 tr:eq(0) th:eq(18)').text("Details");
            }
            else {
               // $('#seatAvailabilityStatusTableTab3 tr:eq(0) th:eq(16)').remove();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function AddSeatAvailabilityData() {
    $("#txtMIScodeAdd").val('');
    $("#divcourseyear").hide();
    $("#AddfooterId").hide(); $("#scrSeatAddTable").hide();
    $("#remarksAdd").val('');
    $('#SeatADDModal').modal('show');
}

function btnMISSearch() {
    var miscode = $("#txtMIScodeAdd").val();
    $("#AddfooterId").show(); $("#scrSeatAddTable").show();
    $("#divcourseyear").show();
    $.ajax({
        type: "GET",
        data: { 'miscode': miscode },
        url: "/Admission/GetSeatAvailabilityListAdd",
        contentType: "application/json",
        success: function (data) {
            var id = 1;
            var idd = 1;
            var iddd = 1;
            var tradid = 1;
            var idddBB = 1;
            $('#SeatAddTable').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'Slno', 'title': 'Sl.No.', 'className': 'text-center' },
                    { 'data': 'CourseTypeName', 'title': 'Course Type', 'className': 'text-center miscode' },
                 //   { 'data': 'AcademicYearString', 'title': 'Academic Year', 'className': 'text-left' },
                    { 'data': 'MISCode', 'title': 'MISCode', 'className': 'text-center miscode' },
                  //  { 'data': 'DivisionId', 'title': 'Division Code', 'className': 'text-left' },
                    { 'data': 'DivisionName', 'title': 'Division Name', 'className': 'text-left' },
                   // { 'data': 'DistrictId', 'title': 'District Code', 'className': 'text-left' },
                    { 'data': 'DistrictName', 'title': 'District Name', 'className': 'text-left' },
                   // { 'data': 'TalukId', 'title': 'Taluk Code', 'className': 'text-left' },
                    { 'data': 'TalukName', 'title': 'Taluk Name', 'className': 'text-left' },
                    { 'data': 'ITITypeName', 'title': 'ITI Type', 'className': 'text-left' },
                    { 'data': 'ITIName', 'title': 'ITI Institute Name', 'className': 'text-left' },
                    {
                        'data': 'TradeId', 'title': 'Trade Code', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var trade = "tradeId" + tradid;
                            $(nTd).html("<span id='" + trade + "'>" + oData.TradeId + "</span>");
                            tradid++;
                        }
                    },
                    { 'data': 'TradeName', 'title': 'Trade Name', 'className': 'text-left' },
                    { 'data': 'Duration', 'title': 'Trade Duration', 'className': 'text-left' },
                    //{
                    //    'data': 'AcademicYearString', 'title': 'Session', 'className': 'text-left',

                    //    "render": function (nTd, sData, oData, iRow, iCol) {
                    //        const date = new Date(new Date().getFullYear(), 7, 1);
                    //        const month = date.toLocaleString('default', { month: 'short' });
                    //        const year = date.getFullYear();
                    //        return (month + "/" + year + "-" + month + "/" + (year + parseInt(oData.Duration)));
                    //    }
                    //},
                    { 'data': 'Shift', 'title': 'Shift', 'className': 'text-left' },
                    { 'data': 'Unit', 'title': 'Unit', 'className': 'text-left' },
                    {
                        'data': 'Unit', 'title': 'Eligible For Current Admission', 'className': 'text-center',
                        "sTitle": "<span>Select</span><input type='checkbox' id=cbxCheckAddAllData onclick='SelectADDAllData(this.id)'></input>", "bSortable": false,
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var stat = oData.IsActive == true ? 'Active' : 'DeActive';
                            if (oData.IsActive == true)
                                $(nTd).html("<input type='checkbox' name='seatCheck' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")'  id='EligAdmission'/>   <img src='/content/frontend/images/i1.jpg' width='60%' height='60%' data-toggle='tooltip' onmouseover='ActiveStatusiNFO()' id='imginformSeat' border='0' title = " + stat + " />"); //title= " + oData.IsActive == true ? 'Active' : 'InActive' + "
                            else
                                $(nTd).html("<input type='checkbox' name='seatCheck' onclick='viewAdmissionNotificationDetails(" + oData.Admission_Notif_Id + ")'  id='EligAdmission' disabled/>   <img src='/content/frontend/images/i1.jpg' width='60%' height='60%' data-toggle='tooltip' onmouseover='ActiveStatusiNFO()' id='imginformSeat' border='0' title = " + stat + " />");
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Seat Type', 'className': 'text-left seatType', 'width': '10%',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var sea = "seatType" + id;
                            if (oData.IsActive == true) {
                                $(nTd).html("<select style='width:100px;' class='form-control' id='" + sea + "' onchange='assignSeats(\"" + sea + "\",\"" + id + "\")'><option value='choose'>choose</option></select>");
                                GetSeatTypes(sea);
                                id++;
                            }
                            else {
                                $(nTd).html("<select style='width:100px;' class='form-control' id='" + sea + "' onchange='assignSeats(\"" + sea + "\",\"" + id + "\")' disabled><option value='choose'>choose</option></select>");
                                GetSeatTypes(sea);
                                id++;
                            }
                        }
                    },
                    {
                        'data': 'batchsize', 'title': 'Batch Size', 'className': 'text-left'
                        //"fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                        //    var mngsea = "totalseat" + idddBB;
                        //    $(nTd).html("<span id='" + mngsea + "'></span>");
                        //    idddBB++;
                        //}
                    },
                    {
                        'data': 'Unit', 'title': 'Govt Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var govsea = "govSeat" + idd;
                            $(nTd).html("<span id='" + govsea + "'></span>");
                            idd++;
                        }
                    },
                    {
                        'data': 'Unit', 'title': 'Management Seat Availability', 'className': 'text-left',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var mngsea = "mngSeat" + iddd;
                            $(nTd).html("<span id='" + mngsea + "'></span>");
                            iddd++;
                        }
                    },


                    { 'data': 'DualSystemTraining', 'title': 'Dual System Training', 'className': 'text-left' }
                ]
            });
        }, error: function (result) {
            //alert("Error", "something went wrong");
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function deActiveSeatDetails(seatId, TradeItiId) {
    var listItem = [];
    var table = $("#seatAvailabilityStatusTable tbody");
    table.find('tr').each(function (len) {
        var $tr = $(this);
        var id = $tr.find("td input[type=hidden]").val();
       
        var list = {
            //TradeId: id,
            TradeItiId: TradeItiId,
            seatId: seatId
        }
        if (seatId != null) {
            listItem.push(list);
        }
    });
    bootbox.confirm({
        message: "<br/> Are you sure you want to deactive record ?",
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
                    type: "GET",
                    url: "/Admission/GetdeActiveSeatDetails",
                    contentType: "application/json",
                    data: { 'seatId': seatId, 'TradeItiId': TradeItiId},                   
                    success: function (data) {
                    
                        if (data == true) {
                            FilterTable();
                            bootbox.alert("<br/><b> DD </b> has deactivated seat availability data successfully.");
                            $('#SeatEditModal').modal('hide');
                            seatAvailabilityDataNew();
                        }
                        else {
                            bootbox.alert("failed");
                        }
                    },
                    error: function (result) {
                        bootbox.alert("failed");
                    }
                });
            }
        }
    });
}

function ADDSeatData() {
    var isValid = true;
    var isValid1 = true;
    var courseType = $('#CourseTypesTab3').val();
    var academicYear = $('#academicyearTab3').val();
    var remarks = $('#remarksAdd').val();
    //var MISCode = $("#txtMIScodeAdd").val();
    $('#CourseTypesTab3Err').text('');
    $('#academicYearTab3Err').text('');
    $('#remarksAddErr').text('');

    if (courseType == 'choose') {
        isValid1 = false;
        $('#CourseTypesTab3Err').text('select the course type');
    }
    if (academicYear == "") {
        isValid1 = false;
        $('#academicYearTab3Err').text('select the session');      
    }
    if (remarks == "") {
        isValid1 = false;
        $('#remarksAddErr').text('Enter the remarks');
    }
    if (isValid1 == true) {

        var list = null;
        var listItem = [];
        var shift_table = $("#SeatAddTable tbody");

        shift_table.find('tr').each(function (len) {
            var $tr = $(this);
            var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
            var misccode = $tr.find("td:eq(2)").text();
            var tradeId = $tr.find("td:eq(8)").text();
            var unit = $tr.find("td:eq(12)").text();
            var shift = $tr.find("td:eq(11)").text();
            var seatTypeId = $tr.find("td select").val()
            var govtSeats = $tr.find("td:eq(16)").text();
            var mngSeats = $tr.find("td:eq(17)").text();           
            var dualSystem = $tr.find("td:eq(18)").text();
            list = {
                MISCode: misccode,
                //MISCode: MISCode,
                TradeId: tradeId,
                Shift: shift,
                Unit: unit,
                SeatType: seatTypeId,
                GovtSeatAvailability: govtSeats,
                ManagementSeatAvailability: mngSeats,
                DualSystemTraining: dualSystem,
                Status: status,
                Remarks: remarks,
                RoleId: 5,
                CourseType: courseType,
                AcademicYear: academicYear,
                 
            }

            if (chkbx == true) {
                if (seatTypeId != "choose")
                    listItem.push(list);
                else
                    bootbox.alert('please select the seat type');
            }

        });

        if (list.MISCode == "") {
            bootbox.alert('No data available');
            return false;
        }
        else if (listItem.length == 0) {
            bootbox.alert('please select the seat type and check the checkbox');
            return false;
        } else {

            if (isValid == true) {
                bootbox.confirm({
                    message: "<br/> Are you sure you want to add seat availability data ? ",
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
                                type: "POST",
                                url: "/Admission/SaveSeatAvailability",
                                contentType: "application/json",
                                data: JSON.stringify(listItem),
                                success: function (data) {
                                    if (data == 'success') {
                                        bootbox.alert("<br/> <b> Deputy Director </b> has added seat availabilty data of <b>" + $('#CourseTypesTab3 option:selected').text() + "</b> for <b>" + $('#academicyearTab3').val() + "</b> session successfully");
                                        //$('#status').val('choose');
                                        $('#SeatADDModal').modal('toggle');                                      
                                        $('#remarksAdd').val('');                                       
                                        seatAvailabilityDataNew();
                                        //GetSeatAvailability();
                                        //FilterTable();
                                    } else if (data == 'exists') {
                                        bootbox.alert("Seat availabilty data already exists");
                                    }
                                    else {
                                        bootbox.alert("failed");
                                    }
                                },
                                error: function (result) {
                                    bootbox.alert("failed");
                                }
                            });
                        }
                    }
                });
            }
            else {
                bootbox.alert('Please check the input fields');
                return false;
            }
        }

    }
    else {
       // bootbox.alert('Please check the input fields');
        return false;
    }
}

function SelectADDAllData(id) {
    var rows = $('#SeatAddTable').DataTable().rows({ 'search': 'applied' }).nodes();
    if ($('#' + id).is(":checked")) {
        $('input[type="checkbox"]:enabled', rows).prop('checked', true);
    } else {
        $('input[type="checkbox"]:enabled', rows).prop('checked', false);
    }
}

function btnsearchTab() {
    var year = $('#Session3 :selected').val();
    var courseTyp = $('#CourseType3 :selected').val();
    var division = $('#divisionDdp3 :selected').val();
    var district = $('#districtDdp3 :selected').val();
    var taluk = $('#talukDdp3 :selected').val();
    var Institute = $('#instituteDdp3 :selected').val();

    var IsValid = true;
    if (year == "" || year == "0" || year == null) {
        IsValid = false;
    }
    if (courseTyp == "" || courseTyp == "0" || courseTyp == null) {
        IsValid = false;
    }

    if (IsValid == true) {
        seatAvailabilityDataNew();      
    }
}

function fnClearFields() {
    $("#CourseType2").val(101);
    $("#divisionDdp").val('choose');
    $("#districtDdp").val('choose');
    $("#talukDdp").val('choose');
    $("#instituteDdp").val('choose');
    $("#districtDdp1").val('choose');
    $("#CourseType3").val(101);
    $("#divisionDdp3").val('choose');
    $("#districtDdp3").val('choose');
    $("#talukDdp3").val('choose');
    $("#instituteDdp3").val('choose');
}