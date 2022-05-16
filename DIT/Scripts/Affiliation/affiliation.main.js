$(document).ready(function () {
    //$('.nav-tabs li:eq(0) a').tab('show')dd-status
    let misArray = {
        results: [],
        "pagination": {
            "more": false
        }
    }
    if ($("#hdnFlag").data('value') == 1) {
        $('.tab-2').tab('show');
    }
    else {
        $('.tab-1').tab('show');
    }
    $("#btnUpload").prop("disabled", true);
    $(".submit-button").attr("disabled", true);
    //$('.collaps-history').click();
    let paging = 1;
    let keyWord = "";
    $('#MIS_CODE').select2({
        placeholder: "Search MIS Code",
        //delay: 250 ,
        minimumInputLength: 2,
        dropdownParent: $('#MIS_CODE').parent(),
        ajax: {
            url: '/Affiliation/GetAffiliatedInstituteMISCode',
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (params) {
                if (keyWord != params.term) {
                    paging = 1;
                    keyWord = params.term;
                }

                return {
                    parm: params.term,
                    page: paging
                };
            },
            processResults: function (data, params) {

                paging++;
                var arr = [];
                $.each(data.list, function () {
                    var obj = {
                        id: this.Value,
                        text: this.Text
                    };
                    arr.push(obj);
                });

                return {
                    results: arr,
                    "pagination": {
                        "more": data.Ismore
                    }
                };

            },
            cache: true
        }
    });

    $('#NewTrade-Insti').select2({
        placeholder: "Select from 0 Institutes",
    });

    $('#FileUpload').change(function () {
        
        var ext = $(this).val().split('.').pop().toLowerCase();
        if (ext != "") {
            if ($.inArray(ext, ['xls', 'xlsx', 'csv']) == -1) {
                bootbox.alert("Pls upload valid Excel file");
                $(this).val("");
                $("#FilePath").text("");
                $("#BtnUpload").val("Browse");
                $("#btnUpload").prop("disabled", false);
                $("#btnUpload").val("Preview");
                $(".submit-button").attr("disabled", true);
            }
            else {
                var filePath = $(this).val();
                var text = filePath;
                text = text.substring(text.lastIndexOf("\\") + 1, text.length);
                $('#FilePath').html(text);

                $("#BtnUpload").val("Change");
                $("#btnUpload").prop("disabled", false);
                $("#btnUpload").val("Preview");
                $(".submit-button").attr("disabled", true);

            }
        } else {
            $("#BtnUpload").val("Browse");
            $("#FilePath").text("");
            $(this).val("");

            $("#btnUpload").prop("disabled", false);
            $("#btnUpload").val("Preview");
            $(".submit-button").attr("disabled", true);
        }


    });

    $('.update-trade-file').change(function () {

        $(this).closest("tr").find(".update-trade-isupload").val(true);

    });

    $("#btnUpload").click(function () {
        
        var fileUpload = $('#FileUpload').get(0);
        var files = fileUpload.files;
        if (files.length > 0) {
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        
            
            $.ajax({
                type: 'POST',
                url: '/Affiliation/UploadAffiliationFile',
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result) {

                    if (result.aff.flag == 1) {
                        
                        var index = 1;
                        if (result.upl_list.length > 0) {

                            var t = $('#tblGridUploadAffiliation').DataTable({
                                data: result.upl_list,
                                "destroy": true,
                                "bSort": true,
                                columns: [
                                    { 'data': 'Slno', 'title': 'Sl. No.', 'className': 'widt' },
                                    { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': 'text-left mis_code' },
                                    { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': 'Iti_Name' },
                                    { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                                    { 'data': 'no_units', 'title': 'Units', 'className': 'text-center' },
                                    { 'data': 'NoofTrades', 'title': 'Total No. Of Trades', 'className': 'text-left notrade' },
                                    { 'data': 'district', 'title': 'District', 'className': 'text-left dist' },
                                    { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left taluk' },
                                    { 'data': 'date', 'title': 'Date', 'className': 'text-left' },
                                ],
                                "columnDefs": [{
                                    "render": function (data, type, full, meta) {

                                        return index++;
                                    },
                                    "targets": 0
                                }],
                            });
                            t.on('order.dt search.dt', function () {
                                t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                                    cell.innerHTML = i + 1;
                                });
                            }).draw();


                            //$('#tblGridUploadAffiliation').DataTable({
                            //    "fnDrawCallback": updateTable
                            //});
                            var index1 = 1;
                            //function updateTable() {
                            $('#tblGridUploadAffiliation').DataTable().destroy();
                                var x = '';
                            $('#tblGridUploadAffiliation tbody').find("tr").each(function (i) {
                                
                              //  if (i > 0) {
                                   
                                    var thisId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").text();
                                    //if (nextId != "") {
                                    //    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text(index1);
                                    //    index1++;
                                    //}
                                    //else {
                                    //    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text("");
                                    //}

                                if (thisId == x) {
                                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".Iti_Name").css('visibility', 'hidden'); 
                                       $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").css('visibility', 'hidden');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".taluk").css('visibility', 'hidden');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".dist").css('visibility', 'hidden');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".notrade").css('visibility', 'hidden');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".widt").css('width', '5%');
                                    }
                                    else {
                                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".Iti_Name").css('visibility', 'visible'); 
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").css('visibility', 'visible');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".taluk").css('visibility', 'visible');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".dist").css('visibility', 'visible');

                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".notrade").css('visibility', 'visible');
                                    $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".widt").css('width', '5%');
                                    //var nextId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i + 1).find(".mis_code").text();
                                   
                                }
                                x = thisId;
                                //}
                                
                            });
                            $("#tblGridUploadAffiliation").dataTable({
                                // ... skipped ...
                            });
                            /*}*/
                        }
                        else {
                            // bootbox.alert("No data found!")
                            $('#tblGridUploadAffiliation').DataTable().clear().draw();
                        }

                        $("#btnUpload").val("Previewed");
                        $("#btnUpload").prop("disabled", true);
                        $(".submit-button").attr("disabled", false);

                    }
                    else {
                        bootbox.alert(result.aff.status);
                    }
                },
                error: function () {
                    bootbox.alert("File is in use. Refreshing the page!")
                    location.reload();
                }
            });
        }
        else {
            //bootbox.alert("Pls select file!");
            $("#BtnUpload").click();
        }
    });

    //$("#BtnUpload").click(function () {

    //       $("#btnUpload").prop("disabled", false);
    //       $("#btnUpload").val("Preview");

    //});

    $(".submit-button").click(function () {
        var fileUpload = $('#FileUpload').get(0);
        var files = fileUpload.files;
        if (files.length > 0) {


            $.ajax({
                type: 'GET',
                url: '/Affiliation/SaveUploadFileData',
                contentType: false,
                processData: false,
                success: function (result) {

                    if (result.flag == 1) {

                        bootbox.alert(result.status);
                        $("#btnUpload").val("Preview");
                        $("#btnUpload").prop("disabled", false);
                        $("#FilePath").text("");
                        $("#BtnUpload").val("Browse");
                        $('input[type=file]').val(null);
                        $(".submit-button").attr("disabled", true);
                        GetAllUploadedAffiliationColleges();
                        GetAllAffiliationColleges(0, 0, 0);
                        fnRefreshScreenData();
                    }
                    else {
                        
                        bootbox.alert('<br><br>'+result.status);
                        //$("#btnUpload").val("Preview");
                        //$("#btnUpload").prop("disabled", false);
                        //$("#FilePath").text("");
                        //$("#BtnUpload").val("Browse");
                        //$('input[type=file]').val(null);
                    }
                },
                error: function (err) {
                    bootbox.alert(err.statusText);
                }
            });
        }
        else {
            bootbox.alert("Pls select File");
            $("#BtnUpload").click();
        }
    });

    var tblGridUploadAvailability = $('#tblGridUploadAffiliation').DataTable();
    tblGridUploadAvailability.on('order.dt arch.dt', function () {
        tblGridUploadAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {

            cell.innerHTML = i + 1;
        });
    }).draw();

    var tblGridSeatAvailability = $('#tblGridUpdateAffiliation').DataTable();
    //tblGridSeatAvailability.on('order.dt arch.dt', function () {
    //	tblGridSeatAvailability.column(0, { search: 'applied', order: 'applied' }).
    //}).draw();

    var tblGridPublishAvailability = $('#tblGridPublishAffiliation').DataTable();
    tblGridPublishAvailability.on('order.dt arch.dt', function () {
        tblGridPublishAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    var tblGridPublishedAvailability = $('#tblGridAffiliatedColleges').DataTable();
    tblGridPublishedAvailability.on('order.dt arch.dt', function () {
        tblGridPublishedAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    var tblGridSeatAvailability = $('#tblGridAffiliationCollege').DataTable();
    //tblGridSeatAvailability.on('order.dt arch.dt', function () {
    //    tblGridSeatAvailability.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();

    cancelAffiliatedInstitute();
    GetCourseTypes();
    GetDivisions();
    GetTrades();
    GetTypesOfITI();
    GetTypesOfITI1();
    //GetDistricts();
    GetConstituencies();
    GetLocationTypes();
    GetTrades_Add();
    // GetCss();
    GetUsers();
    GetStatus();
    GetAffiliationSchemes();
    GetNewTradeDistricts();

    $(".sendBtn3").click(function () {
        
        var Course_type = $("#CourseTypes").val();
        var Division = $("#Divisions").val();
        var District = $("#Districts").val();
        var Trade = $("#Trades").val();

        GetAllAffiliationColleges(Course_type, Division, District, Trade);
    });

    $(".search-publish").click(function () {

        var Course_type = $("#CourseTypes_P").val();
        var Division = $("#Divisions_P").val();
        var District = $("#Districts_P").val();
        var Trade = $("#Trades_P").val();

        GetAllApprovedColleges(Course_type, Division, District, Trade);
    });

    $(".search-affiliated").click(function () {
        
        var Course_type = $("#CourseTypes_Af").val();
        var Division = $("#Divisions_Af").val();
        var District = $("#Districts_Af").val();
        var Trade = $("#Trades_Af").val();

        GetAllPublishedColleges(Course_type, Division, District, Trade);
    });


    $("#Course-Add").change(function () {
        
        var value = $(this).val();
        $("#MISCode-Add").val('');
        $("#MISCode-Add").attr('maxlength', '10');
        if (value == 100) {
            $("#MISCode-Add").attr('maxlength', '11');
        }
        //if (value.toString().length >= maxLength) {
        //    event.preventDefault();
        //    bootbox.alert("Maximum Characters allowed is " + maxLength)
        //}
    });
    /// Update form
    $("#District").change(function () {

        var value = $(this).val();
        $("#Taluka").empty();
        $("#Taluka").append('<option value="">Select</option>');
        $.ajax({
            url: "/Affiliation/GetTaluk",
            type: 'Get',
            data: { DistId: value },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (data != null || data != '') {

                    $.each(data, function () {
                        $("#Taluka").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    });

    //$("#Taluka").change(function () {

    //	var value = $(this).val();
    //	$("#Panchayat").empty();
    //	$("#Panchayat").append('<option value="">Select</option>');
    //	$.ajax({
    //		url: "/Affiliation/GetPanchayat",
    //		type: 'Get',
    //		data: { TalukId: value },
    //		contentType: 'application/json; charset=utf-8',
    //		success: function (data) {

    //			if (data != null || data != '') {

    //				$.each(data, function () {
    //					$("#Panchayat").append($("<option/>").val(this.Value).text(this.Text));
    //				});
    //			}

    //		}, error: function (result) {
    //			 bootbox.alert("Error", "something went wrong");
    //		}
    //       });


    //       //Get all villages by taluk
    //       $("#Village").empty();
    //       $("#Village").append('<option value="">Select</option>');
    //       $.ajax({
    //           url: "/Affiliation/GetVillage",
    //           type: 'Get',
    //           data: { PanchaId: value },
    //           contentType: 'application/json; charset=utf-8',
    //           success: function (data) {

    //               if (data != null || data != '') {

    //                   $.each(data, function () {
    //                       $("#Village").append($("<option/>").val(this.Value).text(this.Text));
    //                   });
    //               }

    //           }, error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //           }
    //       });
    //});

    //$("#Panchayat").change(function () {
    //	
    //	var value = $(this).val();
    //	$("#Village").empty();
    //	$("#Village").append('<option value="">Select</option>');
    //	$.ajax({
    //		url: "/Affiliation/GetVillage",
    //		type: 'Get',
    //		data: { PanchaId: value },
    //		contentType: 'application/json; charset=utf-8',
    //		success: function (data) {
    //			
    //			if (data != null || data != '') {

    //				$.each(data, function () {
    //					$("#Village").append($("<option/>").val(this.Value).text(this.Text));
    //				});
    //			}

    //		}, error: function (result) {
    //			 bootbox.alert("Error", "something went wrong");
    //		}
    //	});
    //});

    ///Add form

    $("#District-Add").change(function () {

        var value = $(this).val();
        if (value != "") {
            $("#Taluka-Add").empty();
            $("#Taluka-Add").append('<option value="">Select</option>');
            $.ajax({
                url: "/Affiliation/GetTaluk",
                type: 'Get',
                data: { DistId: value },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    if (data != null || data != '') {

                        $.each(data, function () {
                            $("#Taluka-Add").append($("<option/>").val(this.Value).text(this.Text));
                        });
                    }

                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }
            });
        }
        else {
            $("#Taluka-Add").empty();
            $("#Taluka-Add").append('<option value="">Select</option>');
        }

    });

    //$("#Taluka-Add").change(function () {

    //	var value = $(this).val();
    //	$("#Panchayat-Add").empty();
    //	$("#Panchayat-Add").append('<option value="">Select</option>');
    //	$.ajax({
    //		url: "/Affiliation/GetPanchayat",
    //		type: 'Get',
    //		data: { TalukId: value },
    //		contentType: 'application/json; charset=utf-8',
    //		success: function (data) {

    //			if (data != null || data != '') {

    //				$.each(data, function () {
    //					$("#Panchayat-Add").append($("<option/>").val(this.Value).text(this.Text));
    //				});
    //			}

    //		}, error: function (result) {
    //			 bootbox.alert("Error", "something went wrong");
    //		}
    //       });

    //       //Get Village by taluk
    //       $("#Village-Add").empty();
    //       $("#Village-Add").append('<option value="">Select</option>');
    //       $.ajax({
    //           url: "/Affiliation/GetVillage",
    //           type: 'Get',
    //           data: { PanchaId: value },
    //           contentType: 'application/json; charset=utf-8',
    //           success: function (data) {

    //               if (data != null || data != '') {

    //                   $.each(data, function () {
    //                       $("#Village-Add").append($("<option/>").val(this.Value).text(this.Text));
    //                   });
    //               }

    //           }, error: function (result) {
    //                bootbox.alert("Error", "something went wrong");
    //           }
    //       });
    //});

    //$("#Panchayat-Add").change(function () {
    //	
    //	var value = $(this).val();
    //	$("#Village-Add").empty();
    //	$("#Village-Add").append('<option value="">Select</option>');
    //	$.ajax({
    //		url: "/Affiliation/GetVillage",
    //		type: 'Get',
    //		data: { PanchaId: value },
    //		contentType: 'application/json; charset=utf-8',
    //		success: function (data) {
    //			
    //			if (data != null || data != '') {

    //				$.each(data, function () {
    //					$("#Village-Add").append($("<option/>").val(this.Value).text(this.Text));
    //				});
    //			}

    //		}, error: function (result) {
    //			 bootbox.alert("Error", "something went wrong");
    //		}
    //	});
    //});

    $(".submit-files").click(function () {

        var files = $(".pdffile");
        var fileData = new FormData();
        var isUploaded = false;
        if (files.length > 0) {
            files.each(function () {

                var fileUpload = $(this).get(0);
                var file = fileUpload.files;
                if (file.length > 0) {

                    fileData.append(file[0].name, file[0]);

                    var CollegeId = $(this).closest("tr").find(".collegeId").val()
                    fileData.append("CollegeId", CollegeId);
                    isUploaded = true;
                }

            });

            if (isUploaded) {
                $.ajax({
                    type: 'POST',
                    url: '/Affiliation/UploadMultipleAffiliationFiles',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {

                        if (result != null) {
                            if (result.length > 0) {

                                $.each(result, function (index, item) {

                                    if (item.flag == 0) {
                                        bootbox.alert(item.status);
                                    }
                                });
                            }
                            else {
                                bootbox.alert("Uplaod Successful!")
                            }
                        }
                    }
                });
            }
            else {
                bootbox.alert("Pls select files to upload")
            }

        }
        else {
            bootbox.alert("No colleges found!")
        }

    });

    // Add new trade table
    $(".add-row-new").click(function () {

        var $tableBody = $('#Table-Trade').find("tbody");
        var $trLast = $tableBody.find("tr:last");
        var $trNew = $trLast.clone();
        //$trLast.after($trNew);
        //After clone
        //$tableBody = $('#Table-Trade').find("tbody");
        //$tableBody.find("tr:last").find(".trade-multi-select").val("");
        //$tableBody.find("tr:last").find(".multi-units").val(1);
        //$tableBody.find("tr:last").find(".add-trade-file").val("");
        //$tableBody.find("tr:last").find(".add-trade-file").change(function () {
        //    
        //    var filePath = $(this).val();
        //    $(this).closest("tr").find(".add-trade-isupload").val(true);
        //    console.log(filePath);
        //});
        $trNew.find(".trade-multi-select").val("").attr("disabled", false);

        // $trNew.find(".text-multi-units").text("0");
        $trNew.find(".update-sessionkey").val(0);
        $trNew.find(".trade-code").text("");
        $trNew.find(".trade-sector").text("");
        $trNew.find(".trade-type").text("");
        $trNew.find(".trade-duration").text("");
        $trNew.find(".trade-size").text("");
        
        $trNew.find(".AidedUnaidedTrade").text("");
        $trNew.find(".btn-add-shift").click(function () {

            GetTradeSession(this);

        }).text("Add Units & Shifts");

        $trNew.find(".add-trade-remove").click(function () {

            var lenght = $('#Table-Trade tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();

                var total = 0;

                $("#Table-Trade").find(".text-multi-units").each(function () {
                    total += parseFloat($(this).text())

                });
                $("#Units-Add").text(total);
            }
            else {
                bootbox.alert("Atleast one row required")
            }
        });

        $tableBody.append($trNew);
    });

    $(".add-trade-remove").click(function () {

        var lenght = $('#Table-Trade tbody tr').length;
        if (lenght > 1) {
            $(this).closest("tr").remove();

            var total = 0;

            $("#Table-Trade").find(".text-multi-units").each(function () {
                total += parseFloat($(this).text())

            });
            $("#Units-Add").text(total);
        }
        else {
            bootbox.alert("Atleast one row required")
        }
    });

    $(".btn-add-shift").click(function () {

        GetTradeSession(this);

    });
    var storeThis = "";
    //Update new trade table
    $(".update-row-new").click(function () {

        var $tableBody = $('#Table-Trade-update').find("tbody");
        var $trLast = $tableBody.find("tr:last");
        var $trNew = $trLast.clone();
        $trNew.find(".trade-multi-select").val("").change(function () {

            GetTradeCode(this);
        }).attr("disabled", false);
        $trNew.find(".btn-add-shift").attr("disabled", false);
        $trNew.find(".text-multi-units").text("0");
        $trNew.find(".trade_shift_id").text("0");
        $trNew.find(".trade-code").text("0");

        $trNew.find(".btn-add-shift").click(function () {
            GetTradeSession(this);
        }).text("Add Shifts");

        $trNew.find(".update-trade_iti_id").val(0);
        $trNew.find(".update-sessionkey").val(0);

        $trNew.find(".update-trade-remove").click(function () {

            var lenght = $('#Table-Trade-update tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();
                var total = 0;
                $(".text-multi-units").each(function () {
                    total += parseFloat($(this).text())

                });
                $("#Units").text(total);
            }
            else {
                bootbox.alert("Atleast one row required")
            }
        }).attr("disabled", false);

        $tableBody.append($trNew);
    });

    $(".update-trade-remove").click(function () {

        var lenght = $('#Table-Trade-update tbody tr').length;
        if (lenght > 1) {
            $(this).closest("tr").remove();
            var total = 0;
            $(".text-multi-units").each(function () {
                total += parseFloat($(this).text())

            });
            $("#Units").text(total);
        }
        else {
            bootbox.alert("Atleast one row required")
        }
    });

    //Shift multiple table in Update Affiliation Trade Page -caseworker login
    $(".update-row-new-units").click(function () {

        var _shift_table = $('#tblShits').find("tbody");
        var len = $('#tblShits').find("tbody tr").length;
        var _tr_shift = $("<tr/>");
        var _td_shift = $("<td/>");
        // _td_shift.addClass("update-shift-units")
        _td_shift.append("<input class='form-control text-center  update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<input class='form-control text-center update-shift' value='1' type='number' min='1' max='3' /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + len + "' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + len + "' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_" + len + "' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_" + len + "' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _btn_rem = $("<button/>");
        _btn_rem.addClass("btn btn-danger remove-update-shift");
        _btn_rem.html("X");
        _btn_rem.click(function () {

            var lenght = $('#tblShits tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();

                $(".update-shift-units").each(function (i) {
                    $(this).text(i + 1);
                });
            }
            else {
                bootbox.alert("Atleast one row required")
            }
        });
        _td_shift.append(_btn_rem);
        _tr_shift.append(_td_shift);
        _shift_table.append(_tr_shift);
        $('#Affiliationordernodate').show();
    });

    //for default load trade
    $(".add-trade-file").change(function () {

        var filePath = $(this).val();
        $(this).closest("tr").find(".add-trade-isupload").val(true);

    });
   
    // Save for Os And AD/DD
    $("#btn-send").click(function () {
        
        var fileUpload = $('#AffilicationDocument').get(0);
        var Fileval = $('#AffilicationDocument').val();
        if (fileUpload != null) {
            var files = fileUpload.files;
        }
       
        var fileData = new FormData();
        
        var isValid = true;
        var status = $("#dd-status").val();
        var flow = $("#dd-flow").val();
        var logid = $("#uservalue").data('value');
        var CTrade_Id = $("#Trade_Id").val();
        if (Fileval != null) {
        for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        }
        //if (!(logid == 5 && (flow == null || flow == "") && status==2 ||status==8)) {
        //if (logid == 5 && (status != 2 || status != 8 || status != 7)) {
        //if (logid == 5 && (status == 6 || status == "")) {
        //    for (var i = 0; i < files.length; i++) {
        //        fileData.append(files[i].name, files[i]);
        //    }
        //    if (Fileval == "") {
        //        return bootbox.alert("pls upload document");

        //    }
        //}
        var selectedStatus = $("#dd-status").find('option:selected').text();
        var selectedFlow = $("#dd-flow").find('option:selected').text();
        var neworderno = $("#New-OrderNo-Update").val();
        var neworderdate = $("#New-OrderNoDate-Update").val();
        if (status == "" || status == null) {
           // $("#Status-Required").show();
            if ($("#dd-status").is(':enabled')) {
                isValid = false;
                return bootbox.alert('Pls select status ');
            }
        }
   
        
            
        if (status != 2) {
            if (flow == "" || flow == null) {
                //$("#Send-Required").show();
                if ($("#dd-flow").is(':enabled') && $("#dd-flow").is(':visible')) {
                    isValid = false;
                    return bootbox.alert('Pls select Send To ');
                }
            }
        }
        else {

            $("#Send-Required").hide();
        }

           // $("#Status-Required").hide();

     
        var remarks = $("#Remarks").val();
        if (remarks == "" || remarks == undefined)
        {
            if (status !="" /*|| status != null*/)
            {
                return bootbox.alert("Pls enter Remarks");
            }
           
        }
        
             
        var clname = $("#ITIName").val();
        if (clname == undefined)
        {
            clname = $(".text-name").text();
        }
        var iti_trade_id = $("#Trade_ITI_Id").val();
        var type = $("#TypeOfITI").val();
        if (type == "" || type == "0" || type == null) {
            $("#TypeOfITI-update-Required").show()
            IsValid = false;
        }
        else {
            $("#TypeOfITI-update-Required").hide()

        }
        var loca = $("#LocationType").val();
        if (loca == "" || loca == "0" || loca == null) {
            $("#LocationType-update-Required").show();
            IsValid = false;
        }
        else {
            $("#LocationType-update-Required").hide();

        }
        var mis = $("#MISCode").val();
        if (mis == "" || mis == "0" || mis == null) {
            $("#MISCode-update-Required").show();
            IsValid = false;
        }
        else {
            $("#MISCode-update-Required").hide();

        }
        var div = $("#Division-Update").val();
        if (div == "" || div == "0" || div == null) {
            $("#Divisions-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Divisions-update-Required").hide();

        }

        var dist = $("#District").val();
        if (dist == "" || dist == "0" || dist == null) {
            $("#District-update-Required").show();
            IsValid = false;
        }
        else {
            $("#District-update-Required").hide();

        }
        var taluk = $("#Taluka").val();
        if (taluk == "" || taluk == "0" || taluk == null) {
            $("#Taluka-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Taluka-update-Required").hide();

        }
        var consti = $("#Constiteuncy").val();
        if (consti == "" || consti == "0" || consti == null) {
            $("#Constiteuncy-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Constiteuncy-update-Required").hide();

        }

        var buildup = $("#BuildUpArea").val();
        if (buildup == "") {
            $("#BuildUpArea-update-Required").show();
            IsValid = false;
        }
        else {
            $("#BuildUpArea-update-Required").hide();

        }
        var address = $("#Address").val();
        if (address == "") {
            $("#Address-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Address-update-Required").hide();

        }
        var geo = $("#GeoLocation").val();
        if (geo == "") {
            $("#GeoLocation-update-Required").show();
            IsValid = false;
        }
        else {
            $("#GeoLocation-update-Required").hide();

        }
        var email = $("#Email").val();
        if (email == "") {
            $("#Email-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Email-update-Required").hide();

        }
        var pincode = $("#Pincode").val();
        if (pincode == "") {
            $("#Pincode-update-Required").show();
            IsValid = false;
        }
        else {
            $("#Pincode-update-Required").hide();
        }
        var phone = $("#PhoneNumber").val();
        if (phone == "") {
            $("#PhoneNumber-update-Required").show();
            IsValid = false;
        }
        else {
            $("#PhoneNumber-update-Required").hide();

        }
        
        var affidate = "";
        var date = "";
        if ($("#AffiliationDate").val() != "" && $("#AffiliationDate").val() != null) {
            var dts = $("#AffiliationDate").val().split("/");
            affidate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
            if (affidate == "Invalid Date") {
                return bootbox.alert("Pls Select Valid EstablishmentDate");
            }
            date = (dts[0] + "/" + dts[1] + "/" + dts[2]);
            $("#AffiliationDate-update-Required").hide();
        }
        else {
            IsValid = false;
            $("#AffiliationDate-update-Required").show();
        }
        var units = $("#Units").text();

        
        var course = $("#Course-Update").val();
        if (course == "") {
            $("#Course-Update-Required").show();
            IsValid = false;
        }
        else {
            $("#Course-Update-Required").hide();

        }
        var orderno = $("#OrderNo-Update").val();
        if (orderno == "") {
            $("#OrderNo-Update-Required").show();
            IsValid = false;
        }
        else {
            $("#OrderNo-Update-Required").hide();
        }

        var orderdate = "";
        if ($("#OrderNoDate-Update").val() != "" && $("#OrderNoDate-Update").val() != null) {
            var dts = $("#OrderNoDate-Update").val().split("/");
            orderdate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
            if (orderdate == "Invalid Date") {
                return bootbox.alert("Please Select Valid Affiliation Date");
            }
            orderdate = (dts[0] + "/" + dts[1] + "/" + dts[2]);
            $("#OrderNoDate-Update-Required").hide();
        }
        else {
            IsValid = false;
            $("#OrderNoDate-Update-Required").show();
        }

        var scheme = $("#Scheme-Update").val();
        if (scheme == "") {
            $("#Scheme-Update-Required").show();
            IsValid = false;
        }
        else {
            $("#Scheme-Update-Required").hide();
        }
        var website = $("#WebSite-Update").val();
        var collegeId = $("#CollegeId").val();
        var aidedunaided = $("#AidedUnaidedTrade").val();


       // var fileData = new FormData();
        
        fileData.append(
            "status_id", status
        );
        fileData.append(
            "flow_id", flow
        );
        fileData.append(
            "remarks", remarks
        );
        fileData.append(
            "trade_iti_id", iti_trade_id
        );
        fileData.append(
            "NewAffiliationOrderNo", neworderno
        );
        fileData.append(
            "NewAffiliationOrderNoDate", neworderdate
        );
        fileData.append(
            "name_of_iti", clname
        );
        fileData.append(
            "type_of_iti_id", type
        );
        fileData.append(
            "location_type_id", loca
        );
        fileData.append(
            "mis_code", mis
        );
        fileData.append(
            "division_id", div
        );
        fileData.append(
            "dist_id", dist
        );
        fileData.append(
            "taluk_id", taluk
        );
        fileData.append(
            "consti_id", consti
        );
        //fileData.append(
        //	"pancha_id", pancha
        //);
        //fileData.append(
        //	"village_id", village
        //);
        fileData.append(
            "build_up_area", buildup
        );
        fileData.append(
            "address", address
        );
        fileData.append(
            "geo_location", geo
        );
        fileData.append(
            "email", email
        );
        fileData.append(
            "affiliation_date", affidate
        );
        fileData.append(
            "phone_number", phone
        );
        fileData.append(
            "no_units", units
        );
        fileData.append(
            "AidedUnaidedTrade", aidedunaided
        );

        fileData.append(
            "iti_college_id", collegeId
        );
        fileData.append(
            "date", date
        );
        fileData.append(
            "Pincode", pincode
        );
        fileData.append(
            "course_code", course
        );
        fileData.append(
            "Website", website
        );
        fileData.append(
            "AffiliationOrderNo", orderno
        );

        fileData.append(
            "Scheme", scheme
        );
        fileData.append(
            "order_no_date", orderdate
        );
        fileData.append(
            "trade_id", CTrade_Id
        );
        //fileData.append(
        //    "list[]", JSON.stringify(sendObj)
        //);

        var _shift_table = $("#tblViewShift tbody");

        _shift_table.find("tr").each(function (len) {

            let $tr = $(this);
            var trade_shift_id = $tr.find(".shiftId").val();
            var ActiveStatus = $tr.find(".active_status").val();


            fileData.append(
                "list[" + len + "].ITI_Trade_Shift_Id", trade_shift_id
            );

            fileData.append(
                "list[" + len + "].IsActive", ActiveStatus
            );
        });
        
        if (isValid) {
            if (flow == 5) {
                var name = "Deputy Director";
            }
            else if (flow == 3) {
                name = "Assistant Director";
            }
            else if (flow == 8) {
                name = "Case Worker";
            }
            else if (flow == 7) {
                name = "Office Superintendent";
            }
            if (status == 4 || status == 9) {
                var stname = "Correction/Clarification";

            }
            else if (status == 7) {
                stname = "Reviewed and Recommend";
            }

            

            if (stname == "Correction/Clarification" && status != 9) {

                bootbox.confirm("<br><br> Are you sure you want to send " + clname + " details to " + name +" for " + stname + "  Please Confirm ?", (confirma) => {
                    if (confirma) {
                        $.ajax({
                            type: 'POST',
                            url: '/Affiliation/AddTradeTransaction',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (result) {

                                if (result.flag == 1) {
                                    //  if (selectedFlow != null && selectedFlow != "" && selectedFlow != "Select") {
                                    ///   bootbox.alert("Affiliation ITI Institute Trades details sent to " + selectedFlow + "  Successfully")
                                    //  } else {
                                    if (status == 2) {
                                        bootbox.alert("<br><br>" + clname + "affiliation  approved successfully")
                                    }
                                    else {
                                        bootbox.alert("<br><br>" + clname + "details sent to " + name +" for " + stname + " successfully")
                                    }

                                    // }

                                    $("#MyTaskModal").modal("hide");
                                    GetAllColleges();
                                    if (status == 2) {
                                        GetAllPublishedColleges();
                                    }
                                    ClearViewAffiliation();
                                }
                                else {
                                    bootbox.alert("Error occured!")
                                }
                            }
                        });
                    }
                });
            }

            else if (status == 9) {

                bootbox.confirm("<br><br> Are you sure you want to sent " + clname + " details   to  Case Worker   for  correction/clarification   Please Confirm ?", (confirma) => {
                    if (confirma) {
                        $.ajax({
                            type: 'POST',
                            url: '/Affiliation/AddTradeTransaction',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (result) {

                                if (result.flag == 1) {
                                    //  if (selectedFlow != null && selectedFlow != "" && selectedFlow != "Select") {
                                    ///   bootbox.alert("Affiliation ITI Institute Trades details sent to " + selectedFlow + "  Successfully")
                                    //  } else {
                                    if (status == 2) {
                                        bootbox.alert("<br><br>" + clname + "affiliation  approved successfully")
                                    }
                                    else {
                                        bootbox.alert("<br><br>" + clname + "details sent to case worker   for  correction/clarification successfully")
                                    }

                                    // }

                                    $("#MyTaskModal").modal("hide");
                                    GetAllColleges();
                                    if (status == 2) {
                                        GetAllPublishedColleges();
                                    }
                                    ClearViewAffiliation();
                                }
                                else {
                                    bootbox.alert("Error occured!")
                                }
                            }
                        });
                    }
                });
            }
            else if (stname == "Reviewed and Recommend") {
                bootbox.confirm("<br><br> Are you sure you want to submit " + clname + " details  to " + name + " for review Please Confirm?", (confirma) => {
                    if (confirma) {
                        $.ajax({
                            type: 'POST',
                            url: '/Affiliation/AddTradeTransaction',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (result) {

                                if (result.flag == 1) {
                                    //  if (selectedFlow != null && selectedFlow != "" && selectedFlow != "Select") {
                                    ///   bootbox.alert("Affiliation ITI Institute Trades details sent to " + selectedFlow + "  Successfully")
                                    //  } else {
                                    if (status == 2) {
                                        bootbox.alert("<br><br>" + clname + " affiliation  approved successfully")
                                    }
                                    else {
                                        bootbox.alert("<br><br>" + clname+ " details sent to " + name + " for review successfully")
                                    }

                                    // }

                                    $("#MyTaskModal").modal("hide");
                                    GetAllColleges();
                                    if (status == 2) {
                                        GetAllPublishedColleges();
                                    }
                                    ClearViewAffiliation();
                                }
                                else {
                                    bootbox.alert("Error occured!")
                                }
                            }
                        });
                    }
                });
            }
            else {
                var txtAppSave = "Save ";
                if ($("#dd-status").val() == 2) {
                    txtAppSave = "Approve "
                }
                bootbox.confirm("<br><br> Are you sure you want to " + txtAppSave + clname + " details  ?", (confirma) => {
                    if (confirma) {
                        $.ajax({
                            type: 'POST',
                            url: '/Affiliation/AddTradeTransaction',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (result) {
                                
                                if (result.flag == 1) {
                                    //  if (selectedFlow != null && selectedFlow != "" && selectedFlow != "Select") {
                                    ///   bootbox.alert("Affiliation ITI Institute Trades details sent to " + selectedFlow + "  Successfully")
                                    //  } else {
                                    if (status == 2) {
                                        bootbox.alert("<br><br>" + clname + " affiliation details  approved successfully")
                                    }
                                    else {
                                        bootbox.alert("<br><br>" + clname + " details saved successfully")
                                    }

                                    // }

                                    $("#MyTaskModal").modal("hide");
                                    GetAllColleges();
                                    if (status == 2) {
                                        GetAllPublishedColleges();
                                    }
                                    ClearViewAffiliation();
                                }
                                else {
                                    bootbox.alert("Error occured!")
                                }
                            }
                        });
                    }
                });
            }
            //fnRefreshScreenData();
        }
    });

    $("#btn-send-insti").click(function () {

        var isValid = true;
        var status = $("#dd-status").val();

        var selectedStatus = $("#dd-status").find('option:selected').text();

        if (status == "" || status == null) {
            $("#Status-Required").show();
            isValid = false;
        }

        var remarks = $("#Remarks").val();

        var iti_trade_id = $("#Trade_ITI_Id").val();
        var fileData = new FormData();
        fileData.append(
            "status_id", status
        );

        fileData.append(
            "remarks", remarks
        );
        fileData.append(
            "trade_iti_id", iti_trade_id
        );
        if (isValid) {
            $.ajax({
                type: 'POST',
                url: '/Affiliation/AddTradeTransaction',
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result) {

                    if (result.flag == 1) {
                        if (status == 2) {
                            bootbox.alert("Affiliation trade approved and published successfully")
                        }
                        else {
                            bootbox.alert("Affiliation trade " + selectedStatus + " successfully")
                        }


                        $("#MyTaskModal").modal("hide");
                        GetAllColleges();

                        ClearViewAffiliation();
                    }
                    else {
                        bootbox.alert("Error occured!!")
                    }
                }
            });
        }
    });

    $("#dd-status").change(function () {
        var value = $(this).val();
        if (value == 2 || value == 3) {
            $("#dd-flow").val("");
            $("#dd-flow").attr("disabled", true);
        }
        else {
            $("#dd-flow").attr("disabled", false);
        }
    });

    $("#dd-status").change(function () {
        
        $("#dd-flow").empty();
        $("#dd-flow").append('<option value="">Select</option>');
        var value = $(this).val();
        //if (value == 4) {
        if (value == 4 || value == 5 || value == 7) {
            $.ajax({
                url: "/Affiliation/GetAllUsers1",
                type: 'Get',
                data: { statusValue: value },
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    if (data != null || data != '') {
                        $.each(data, function () {
                            
                            $("#dd-flow").append($("<option/>").val(this.Value).text(this.Text));
                        });
                    }
                    else {
                        $("#dd-flow").attr("disabled", true);
                    }



                }, error: function (result) {
                    bootbox.alert("Error", "something went wrong");
                }

            });
        }
        //else
        //{
        //    $("#dd-flow").attr("disabled", true);
        //}
        //}
        //var value = $(this).val();
        //if (value == 2 || value == 4) {
        // $("#dd-flow").attr("disabled", true);
        //}
        //else {
        // $("#dd-flow").attr("disabled", false);
        //}
    });

    $(".add-trade-row-new").click(function () {

        var $tableBody = $('#Table-Trade-New').find("tbody");
        var $trLast = $tableBody.find("tr:last");
        var $trNew = $trLast.clone();
        //$trLast.after($trNew);
        //After clone
        //$tableBody = $('#Table-Trade').find("tbody");
        //$tableBody.find("tr:last").find(".trade-multi-select").val("");
        //$tableBody.find("tr:last").find(".multi-units").val(1);
        //$tableBody.find("tr:last").find(".add-trade-file").val("");
        //$tableBody.find("tr:last").find(".add-trade-file").change(function () {
        //    
        //    var filePath = $(this).val();
        //    $(this).closest("tr").find(".add-trade-isupload").val(true);
        //    console.log(filePath);
        //});
        $trNew.find(".trade-multi-select").val("").attr("disabled", false);

        // $trNew.find(".text-multi-units").text("0");
        $trNew.find(".update-sessionkey").val(0);
        $trNew.find(".trade-code").text("");
        $trNew.find(".trade-sector").text("");
        $trNew.find(".trade-type").text("");
        $trNew.find(".trade-duration").text("");
        $trNew.find(".trade-size").text("");
        $trNew.find(".btn-add-shift").click(function () {

            GetTradeSession(this);

        }).text("Add Units & Shifts");

        $trNew.find(".add-new-trade-remove").click(function () {

            var lenght = $('#Table-Trade-New tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();

                var total = 0;

                $("#Table-Trade-New").find(".text-multi-units").each(function () {
                    total += parseFloat($(this).text())

                });
                $("#Units-Add").text(total);
            }
            else {
                bootbox.alert("Atleast one row required")
            }
        });

        $tableBody.append($trNew);
    });

    $(".add-new-trade-remove").click(function () {

        var lenght = $('#Table-Trade-New tbody tr').length;
        if (lenght > 1) {
            $(this).closest("tr").remove();

            var total = 0;

            $("#Table-Trade-New").find(".text-multi-units").each(function () {
                total += parseFloat($(this).text())

            });

        }
        else {
            bootbox.alert("Atleast one row required")
        }
    });

    $(".multi-units").change(function () {

        var val = $(this).val()
        var _table = $(this).closest("tr");
        // _table.empty();

        $.each(parseFloat(val), function (i) {
            var _tr = $("</tr>");
            var _td = "<td>" + (i + 1) + "</td>";
            _tr.append(_td);
            var _input = $("</input>");
            _input.val(1);
            _td = $("</td>");
            _td.append(_input);
            _tr.append(_td);
            _td = "<td><input type='button' class='btn btn-danger' value='X' /></td>";
            _tr.append(_td);
            _table.append(_tr);
        })
    });

    $("#Update_Add_Unit").click(function () {

        var _shift_table = $('#Table-Shift').find("tbody");
        var len = $('#Table-Shift').find("tbody tr").length;
        var _tr_shift = $("<tr/>");
        var _td_shift = $("<td/>");
        // _td_shift.addClass("update-shift-units")
        _td_shift.append("<input class='form-control update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<input class='form-control update-shift' value='1' type='number' min=0 /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + len + "' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + len + "' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_" + len + "' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_" + len + "' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
        _tr_shift.append(_td_shift);
        _td_shift = $("<td/>");
        _btn_rem = $("<button/>");
        _btn_rem.addClass("btn btn-danger remove-update-shift");
        _btn_rem.html("X");
        _btn_rem.click(function () {

            var lenght = $('#Table-Shift tbody tr').length;
            if (lenght > 1) {
                $(this).closest("tr").remove();

                $(".update-shift-units").each(function (i) {
                    $(this).text(i + 1);
                });
            }
            else {
                bootbox.alert("Atleast one row required")
            }
        });
        _td_shift.append(_btn_rem);
        _tr_shift.append(_td_shift);
        _shift_table.append(_tr_shift);
    });

    $("body").on('click', ".btn-shift-cancel", function () {
        storeThis.closest("tr").find('.update-popover').click();
    });

    $("body").on('click', ".btn-shift", function () {

        var pop_div = storeThis.closest("tr").find(".update-popover-div");
        pop_div.empty();
        _div_pop_row = $("<div/>");
        _div_pop_row.addClass("row");

        _div_pop_row_col = $("<div/>");
        _div_pop_row_col.addClass("col-md-1");
        _div_pop_row_col.append("Unit")
        _div_pop_row.append(_div_pop_row_col);

        _div_pop_row_col = $("<div/>");
        _div_pop_row_col.addClass("col-md-1");
        _div_pop_row_col.append("Shift")
        _div_pop_row.append(_div_pop_row_col);


        _div_pop_row_col = $("<div/>");
        _div_pop_row_col.addClass("col-md-4");
        _div_pop_row_col.append("Is PPP")
        _div_pop_row.append(_div_pop_row_col);


        _div_pop_row_col = $("<div/>");
        _div_pop_row_col.addClass("col-md-4");
        _div_pop_row_col.append("Dual Sys. Training")
        _div_pop_row.append(_div_pop_row_col);
        pop_div.append(_div_pop_row);

        $(".popover-content").find(".shift").each(function (i) {

            var val = $(this).val()

            _div_pop_row = $("<div/>");
            _div_pop_row.addClass("row");

            _div_pop_row_col = $("<div/>");
            _div_pop_row_col.addClass("col-md-1");
            _div_pop_row_col.append(i + 1)
            _div_pop_row.append(_div_pop_row_col);

            _div_pop_row_col = $("<div/>");
            _div_pop_row_col.addClass("col-md-2");
            _div_pop_row_col.append("<input type='number' class='form-control shift' value='" + val + "' />")
            _div_pop_row.append(_div_pop_row_col);

            _div_pop_row_col = $("<div/>");
            _div_pop_row_col.addClass("col-md-4");
            _div_pop_row_col.append("<div class='row'><div class='col-md-6'><input class='form-check-input radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no' checked><label class='form-check-label' checked>No</label></div></div>")
            var isPPP_yes = $(this).closest("tr").find(".radio-yes").is(":checked");
            var isPPP_no = $(this).closest("tr").find(".radio-no").is(":checked");
            if (isPPP_yes) {
                _div_pop_row_col.find(".radio-yes").attr("checked", "checked");
            }
            else {
                _div_pop_row_col.find(".radio-no").attr("checked", "checked");
            }
            _div_pop_row.append(_div_pop_row_col);

            _div_pop_row_col = $("<div/>");
            _div_pop_row_col.addClass("col-md-4");
            _div_pop_row_col.append("<div class='row'><div class='col-md-6'><input class='form-check-input radio-dual-yes' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input radio-dual-no' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='no' checked><label class='form-check-label' checked>No</label></div></div>")
            var daul_yes = $(this).closest("tr").find(".radio-dual-yes").is(":checked");
            var dual_no = $(this).closest("tr").find(".radio-dual-no").is(":checked");

            if (daul_yes) {
                _div_pop_row_col.find(".radio-dual").attr("checked", "checked");
            }
            else {
                _div_pop_row_col.find(".radio-dual").attr("checked", "checked");
            }
            _div_pop_row.append(_div_pop_row_col);

            pop_div.append(_div_pop_row); // second row

        });
        pop_div.append("<div style='text-align:center' ><div class='form-group' ><input type='button' class='btn-danger btn-shift-cancel' value='Cancel' /><input type='button' class='btn-primary btn-shift' value='Save' /></div></div>");
        storeThis.closest("tr").find('.update-popover').click();

    });


});

function cancelAffiliatedInstitute() {
    $('#CourseTypes_Af').val('choose');
    $('#Divisions_Af').val('choose');
    $('#Districts_Af').val('choose');
    $('#Trades_Af').val('choose');

}


function GetCourseTypes() {
    //Add 
    $("#Course-Add").empty();
    $("#Course-Add").append('<option value="">Select</option>');

    //Update List
    $("#CourseTypes").empty();
    $("#CourseTypes").append('<option value="">Select</option>');
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
    $("#Divisions").empty();
    $("#Divisions").append('<option value="">Select</option>');
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
                $("#Divisions").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Divisions_P").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Divisions_Af").append($("<option/>").val(this.division_id).text(this.division_name));
                $("#Division-Add").append($("<option/>").val(this.division_id).text(this.division_name));
            });


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

function GetDistrict_Add() {

    var Divisions = $('#Division-Add :selected').val();
    if (Divisions != "" && Divisions != null) {
        $("#District-Add").empty();
        $("#District-Add").append('<option value="">Select</option>');
        $("#Taluka-Add").empty();
        $("#Taluka-Add").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#District-Add").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#District-Add").empty();
        $("#District-Add").append('<option value="">Select</option>');
        $("#Taluka-Add").empty();
        $("#Taluka-Add").append('<option value="">Select</option>');
    }

}

function GetDistrictList() {
    $("#Districts").empty();
    var Divisions = $('#Divisions :selected').val();
    if (Divisions != "" && Divisions != null) {
       /* $("#Districts").empty();*/
        $("#Districts").append('<option value="">Select</option>');
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

//Get districts for publish page
function GetDistrict_Pub() {

    var Divisions = $('#Divisions_P :selected').val();
    if (Divisions != null && Divisions != "") {
        $("#Districts_P").empty();
        $("#Districts_P").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#Districts_P").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }

}

//Get districts for Affiliated View
function GetDistrict_Aff() {

    var Divisions = $('#Divisions_Af :selected').val();
    if (Divisions != null && Divisions != "") {
        $("#Districts_Af").empty();
        $("#Districts_Af").append('<option value="choose">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetDistrictList',
            data: { Divisions: Divisions },
            success: function (data) {

                $.each(data, function () {
                    $("#Districts_Af").append($("<option/>").val(this.district_id).text(this.district));
                });


            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }

}


function GetTrades() {
    //Update
    $("#Trades").empty();
    $("#Trades").append('<option value="">Select</option>');
    //Publish
    $("#Trades_P").empty();
    $("#Trades_P").append('<option value="">Select</option>');
    //Affiliated View
    $("#Trades_Af").empty();
    $("#Trades_Af").append('<option value="">Select</option>');
    //default for dynamic trades table
    $(".trade-multi-select").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradesList',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Trades").append($("<option/>").val(this.trade_id).text(this.trade_code));
                    $("#Trades_P").append($("<option/>").val(this.trade_id).text(this.trade_code));
                    $("#Trades_Af").append($("<option/>").val(this.trade_id).text(this.trade_code));
                    $(".trade-multi-select").append($("<option/>").val(this.trade_id).text(this.trade_code));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

///Add form

function GetTypesOfITI1() {

    $("#TypeOfITI-Add").on("change", function () {
        if ($("#TypeOfITI-Add option:selected").val() == 1) {
            $("#controlId").hide();
        } else {
            $("#controlId").show();
        }
    });
}


function GetTrades_Add() {
    // var Course = $('#CourseTypes :selected').val();
    $("#ListOfTrades-Add").empty();
    $("#ListOfTrades-Add").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradesList',
        //data: { CourseId: Course },
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#ListOfTrades-Add").append($("<option/>").val(this.trade_id).text(this.trade_code));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}
function GetTypesOfITI() {
    // var Course = $('#CourseTypes :selected').val();
    $("#TypeOfITI-Add").empty();
    $("#TypeOfITI-Add").append('<option value="">Select</option>');
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
    $("#LocationType-Add").append('<option value="">Select</option>');
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

function GetConstituencies() {
    // var Course = $('#CourseTypes :selected').val();
    $("#Constiteuncy-Add").empty();
    $("#Constiteuncy-Add").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetConstituency',
        //data: { CourseId: Course },
        success: function (data) {

            if (data != null || data != '') {
                $.each(data, function () {
                    $("#Constiteuncy-Add").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetDistricts() {


    $("#District-Add").empty();
    $("#District-Add").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetDistricts',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#District-Add").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetCss() {


    $("#CssCode-Add").empty();
    $("#CssCode-Add").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetCss',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#CssCode-Add").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function Update(collegeId) {
    

    $("#CollegeId").val(collegeId);
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetCollegeDetails',
        data: { collegId: collegeId },
        success: function (data) {
            if (data != null) {

                $("#DataGridDiv").hide();
                $("#UpdateDiv").show();
                if (data.inst_list.length > 0) {
                    $.each(data.inst_list, function (index, values) {
                        $("#TypeOfITI").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.loca_type_list.length > 0) {
                    $.each(data.loca_type_list, function (index, values) {
                        $("#LocationType").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.css_code_list.length > 0) {
                //	$.each(data.css_code_list, function (index, values) {
                //		$("#CssCode").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                if (data.trades_list.length > 0) {
                    $.each(data.trades_list, function (index, values) {
                        $("#Update_Trade").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.div_list.length > 0) {
                    $.each(data.div_list, function (index, values) {
                        $("#Division-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.dist_list.length > 0) {
                    $.each(data.dist_list, function (index, values) {
                        $("#District").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.taluk_list.length > 0) {
                    $.each(data.taluk_list, function (index, values) {
                        $("#Taluka").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.consti_list.length > 0) {
                    $.each(data.consti_list, function (index, values) {
                        $("#Constiteuncy").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.cou_list.length > 0) {
                    $.each(data.cou_list, function (index, values) {
                        $("#Course-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.schem_list.length > 0) {
                    $.each(data.schem_list, function (index, values) {
                        $("#Scheme-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.pancha_list.length > 0) {
                //	$.each(data.pancha_list, function (index, values) {
                //		$("#Panchayat").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                //if (data.village_list.length > 0) {
                //	$.each(data.village_list, function (index, values) {
                //		$("#Village").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}

                $("#ITIName").val(data.aff.name_of_iti);
                $("#TypeOfITI").val(data.aff.type_of_iti_id);
                $("#LocationType").val(data.aff.location_type_id);
                $("#MISCode").val(data.aff.mis_code);
                $("#Division-Update").val(data.aff.division_id);
                // $("#CssCode").val(data.aff.css_code);
                $("#District").val(data.aff.dist_id);
                $("#Taluka").val(data.aff.taluk_id);
                $("#Constiteuncy").val(data.aff.consti_id);
                //$("#Panchayat").val(data.aff.pancha_id);
                //$("#Village").val(data.aff.village_id);
                $("#BuildUpArea").val(data.aff.build_up_area);
                $("#Address").val(data.aff.address);
                $("#GeoLocation").val(data.aff.geo_location);
                $("#Email").val(data.aff.email);
                $("#Pincode").val(data.aff.Pincode);
                $("#PhoneNumber").val(data.aff.phone_number);
                $("#AffiliationDate").val(data.aff.date);
                $("#Units").text(data.aff.no_units);
                $("#Course-Update").val(data.aff.course_code);
                $("#OrderNo-Update").val(data.aff.AffiliationOrderNo);
                $("#OrderNoDate-Update").val(data.aff.order_no_date);
                $("#Scheme-Update").val(data.aff.Scheme);
                $("#WebSite-Update").val(data.aff.Website);
                $("#FileAttach").empty();
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId + "' >Download File</a>"
                    $("#FileAttach").append(html);
                    $("#IsUploaded").val(true);
                }
                else {
                    $("#IsUploaded").val(false);
                }

                if (data.aff.trades_list != null) {
                    if (data.aff.trades_list.length > 0) {
                        $("#Table-Trade-update tbody").empty()
                        // $("#Table-Trade-update").append("<tbody></tbody>");

                        $.each(data.aff.trades_list, function () {

                            var _select = $("<select/>");
                            _select.addClass("form-control add-ddl trade-multi-select");
                            _select.append($("<option/>").val("").text("Select"));
                            _select.change(function () {

                                GetTradeCode(this);
                            });
                            //var units = $("<select/>");
                            //units.addClass("form-control multi-units");

                            if (data.trades_list.length > 0) {
                                $.each(data.trades_list, function (index, values) {
                                    _select.append($("<option/>").val(values.Value).text(values.Text));
                                });
                            }
                            //for (var i = 0; i < 10; i++) {
                            //    units.append($("<option/>").val(i + 1).text(i + 1));
                            //}

                            if (this.sessionKey > 0) {
                                _btn_pop = $("<button/>");
                                _btn_pop.addClass("btn btn-primary btn-add-shift");
                                _btn_pop.html("Update Shifts");
                            }
                            else {
                                _btn_pop = $("<button/>");
                                _btn_pop.addClass("btn btn-primary btn-add-shift");
                                _btn_pop.html("Add Shifts");
                            }

                            _btn_pop.click(function () {
                                GetTradeSession(this);
                            });

                            if (this.is_published) {
                                _btn_pop.attr("disabled", true);
                            }
                            else {
                                if (!this.en_edit) {
                                    _btn_pop.attr("disabled", true);
                                }
                            }


                            var _tr = $("<tr/>");
                            var _td = $("<td/>"); // first td

                            if (this.is_published) {
                                _td.append(_select.val(this.trade_id).attr("disabled", true));
                            }
                            else {
                                if (this.en_edit) {
                                    _td.append(_select.val(this.trade_id));
                                }
                                else {
                                    _td.append(_select.val(this.trade_id).attr("disabled", true));
                                }
                            }

                            _tr.append(_td);

                            _td = $("<td/>"); //second td
                            //if (this.en_edit) {
                            //    _td.append(units.val(this.units));
                            //}
                            //else {
                            //    _td.append(units.val(this.units).attr("disabled", true));
                            //}
                            _td.append("<label class='form-control trade-code' >" + this.trade_code + "</label>")
                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append("<label class='form-control text-multi-units' >" + this.units + "</label>")
                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append(_btn_pop);

                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append("<button class='btn btn-link'  onclick='ViewShifts(this);'>View Shifts</button>");

                            _tr.append(_td);
                            _td = $("<td/>"); //fourth td
                            _btn = "";
                            if (this.is_published) {
                                _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove' disabled='disabled' >X</button>");
                            }
                            else {
                                if (this.en_edit) {
                                    _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove'>X</button>");
                                    _btn.click(function () {

                                        var lenght = $('#Table-Trade-update tbody tr').length;
                                        if (lenght > 1) {
                                            $(this).closest("tr").remove();
                                            var total = 0;
                                            $(".text-multi-units").each(function () {
                                                total += parseFloat($(this).text())

                                            });
                                            $("#Units").text(total);
                                        }
                                        else {
                                            bootbox.alert("Atleast one row required")
                                        }
                                    });
                                }
                                else {
                                    _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove' disabled='disabled' >X</button>");
                                }
                            }



                            _td.append(_btn);
                            _tr.append(_td);

                            $("#Table-Trade-update tbody").append(_tr);
                        });

                        var isPub = true;
                        var isEdit = false;
                        $.each(data.aff.trades_list, function () {

                            if (!this.is_published) {
                                isPub = false;

                            }

                        });

                        $.each(data.aff.trades_list, function () {

                            if (this.en_edit) {
                                isEdit = true;

                            }
                        });

                        //$(".update-row-new").attr("disabled", (!isPub));
                        //$(".update-button").attr("disabled", (!isPub));
                        //
                        if (isPub) {
                            $(".update-row-new").attr("disabled", false);
                            $(".update-button").attr("disabled", false);
                        }
                        else {
                            if (isEdit) {
                                $(".update-row-new").attr("disabled", false);
                                $(".update-button").attr("disabled", false);
                            } else {
                                $(".update-row-new").attr("disabled", true);
                                $(".update-button").attr("disabled", true);
                            }

                        }
                    }
                }

                if (data.his_list.length > 0) {
                    $("#tblUpdateHistory tbody").empty();
                    $.each(data.his_list, function (i) {
                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.TradeName + "</td");
                        _tr.append("<td>" + this.date + "</td");

                        _tr.append("<td>" + this.created_by + "</td");
                        _tr.append("<td>" + this.Flow_user + "</td>");
                        _tr.append("<td>" + this.Status + "</td>");
                        _tr.append("<td>" + this.Remarks + "</td>");

                        //if (this.FileUploadPath != null && this.FileUploadPath != "") {
                        //    _tr.append("<td><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + this.Trade_ITI_id + "' >Download File</a></td>");
                        //}
                        //else {
                        //    _tr.append("<td>File not uploaded</td>");
                        //}

                        $("#tblUpdateHistory tbody").append(_tr);
                    });
                }

            }

        }, error: function () {
            bootbox.alert("Error", "something went wrong");
        }
    });

}

function GetUpdateTrade(tradeId) {
    
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetUpdateTradeDetails',
        data: { trade_id: tradeId },
        success: function (data) {
            
            if (data.aff.flag == 1) {

                $('#Affiliationordernodate').hide();

                $("#DataGridDiv").hide();               
                $("#UpdateDiv").show();

                if (data.inst_list.length > 0) {
                    $("#TypeOfITI").empty();
                    $("#TypeOfITI").append($("<option/>").val("").text("Select"));
                    $.each(data.inst_list, function (index, values) {
                        $("#TypeOfITI").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.loca_type_list.length > 0) {
                    $("#LocationType").empty();
                    $("#LocationType").append($("<option/>").val("").text("Select"));
                    $.each(data.loca_type_list, function (index, values) {
                        $("#LocationType").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.css_code_list.length > 0) {
                //	$.each(data.css_code_list, function (index, values) {
                //		$("#CssCode").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                if (data.trades_list.length > 0) {
                    $("#Update_Trade").empty();
                    $("#Update_Trade").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#Update_Trade").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.div_list.length > 0) {
                    $("#Division-Update").empty();
                    $("#Division-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.div_list, function (index, values) {

                        $("#Division-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.dist_list.length > 0) {
                    $("#District").empty();
                    $("#District").append($("<option/>").val("").text("Select"));
                    $.each(data.dist_list, function (index, values) {

                        $("#District").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.taluk_list.length > 0) {
                    $("#Taluka").empty();
                    $("#Taluka").append($("<option/>").val("").text("Select"));
                    $.each(data.taluk_list, function (index, values) {

                        $("#Taluka").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.consti_list.length > 0) {
                    $("#Constiteuncy").empty();
                    $("#Constiteuncy").append($("<option/>").val("").text("Select"));
                    $.each(data.consti_list, function (index, values) {

                        $("#Constiteuncy").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.cou_list.length > 0) {
                    $("#Course-Update").empty();
                    $("#Course-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.cou_list, function (index, values) {

                        $("#Course-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.schem_list.length > 0) {
                    $("#Scheme-Update").empty();
                    $("#Scheme-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.schem_list, function (index, values) {

                        $("#Scheme-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.trades_list.length > 0) {
                    $("#ListOfTrades").empty();
                    $("#ListOfTrades").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#ListOfTrades").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.pancha_list.length > 0) {
                //	$.each(data.pancha_list, function (index, values) {
                //		$("#Panchayat").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                //if (data.village_list.length > 0) {
                //	$.each(data.village_list, function (index, values) {
                //		$("#Village").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}

                $("#ITIName").val(data.aff.name_of_iti);
                $("#TypeOfITI").val(data.aff.type_of_iti_id);
                $("#LocationType").val(data.aff.location_type_id);
                $("#MISCode").val(data.aff.mis_code);
                $("#Division-Update").val(data.aff.division_id);
                // $("#CssCode").val(data.aff.css_code);
                $("#District").val(data.aff.dist_id);
                $("#Taluka").val(data.aff.taluk_id);
                $("#Constiteuncy").val(data.aff.consti_id);
                //$("#Panchayat").val(data.aff.pancha_id);
                //$("#Village").val(data.aff.village_id);
                $("#BuildUpArea").val(data.aff.build_up_area);
                $("#Address").val(data.aff.address);
                $("#GeoLocation").val(data.aff.geo_location);
                $("#Email").val(data.aff.email);
                $("#Pincode").val(data.aff.Pincode);
                $("#PhoneNumber").val(data.aff.phone_number);
                $("#AffiliationDate").val(data.aff.date);
                $("#Units").text(data.aff.no_units);
                $("#Course-Update").val(data.aff.course_code);
                $("#OrderNo-Update").val(data.aff.AffiliationOrderNo);
                $("#OrderNoDate-Update").val(data.aff.order_no_date);
                $("#Scheme-Update").val(data.aff.Scheme);
                $("#WebSite-Update").val(data.aff.Website);
                $("#TradeName").text(data.aff.trade);
                $("#SectorName").text(data.aff.sector);
                $("#TradeCode").text(data.aff.trade_code);
                $("#TradeType").text(data.aff.trade_type);
                $("#Duration").text(data.aff.duration);
                $("#BatchSize").text(data.aff.batch_size);
                $("#AidedUnaidedTrade").text(data.aff.AidedUnaidedTrade);
                $("#Trade_ITI_Id").val(tradeId);
                $("#CollegeId").val(data.aff.iti_college_id);
                $("#Trade_Id").val(data.aff.trade_id);
                $("#ListOfTrades").val(data.aff.trade_id);

                $("#New-OrderNo-Update").val(data.aff.NewAffiliationOrderNo);
                $("#New-OrderNoDate-Update").val(data.aff.NewAffiliationOrderNoDate);

                $("#FileAttach").empty();
                $("#TradeFileAttach").empty();
                
                if (data.aff.Filename != null)
                {
                    $(".AfffiliationName").text(data.aff.Filename);
                }
                
                $("#FileAttach").empty();
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    if (data.aff.isSelect) {
                        var html = "<a title='Download Pdf' class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        $("#FileAttach").append(html);
                        $("#IsUploaded").val(true);
                    }
                    else {
                        var html = "<p>File not found</p>"
                        $("#FileAttach").append(html);
                        $("#IsUploaded").val(false);
                    }

                }
                else {
                    $("#IsUploaded").val(false);
                }
                
                $("#TradeFileAttach").empty();
                if (data.aff.UploadTradeAffiliationDoc != null && data.aff.UploadTradeAffiliationDoc != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationTradeDoc?CollegeId=" + data.aff.tradeId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                    $("#TradeFileAttach").append(html);
                    // $("#IsUploaded").val(true);
                }
                else {
                    $("#tradedoc").hide();
                    $("#tradedoc1").hide();
                }
                $(".update-button").attr("disabled", (!data.aff.en_edit));
                $(".update-row-new-units").attr("disabled", (!data.aff.en_edit));
                $(".update-button").attr("disabled", (!data.aff.en_edit));

                var _table = $("#tblShits tbody");
                _table.empty();
                //$.each(data.shifts, function (i) {

                //    var _tr = $("<tr/>");
                //    _tr.append("<td>" + (i + 1) + "</td>");
                //    _tr.append("<td>" + this.Units + "</td");
                //    _tr.append("<td>" + this.Shift + "</td>");
                //    _tr.append("<td>" + this.IsPPP + "</td>");
                //    _tr.append("<td>" + this.Dual_System + "</td>");
                //    _table.append(_tr);
                //});

                if (data.aff.shifts.length > 0) {
                    $.each(data.aff.shifts, function (i) {

                        var _tr_shift = $("<tr/>");
                        var _td_shift = $("<td/>");
                        //_td_shift.addClass("update-shift-units")
                        _td_shift.append("<input class='form-control update-shift-units' value='" + this.Units + "' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<input class='form-control update-shift' value='" + this.Shift + "' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='" + this.ITI_Trade_Shift_Id + "' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        var _checked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' checked>";
                        var _unchecked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' >";
                        var _unchecked_no = "<input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no'>";
                        var _checked_no = "<input class='form-check-input  isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no' checked>"
                        if (this.IsPPP == 'yes') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _unchecked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _checked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        _tr_shift.append(_td_shift);
                        var _checked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' checked>";
                        var _unchecked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' >"
                        var _unchecked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' >";
                        var _checked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' checked>"

                        _td_shift = $("<td/>");
                        if (this.Dual_System == 'regular') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _unchecked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _checked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        if (this.IsActive) {
                            _td_shift.text("Active")
                        }
                        else {
                            if (this.Status == 5) {
                                _td_shift.text("Pending for Approval");
                            }
                            else {
                                _td_shift.text("DeActive");
                            }

                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        _btn_rem = $("<button/>");
                        _btn_rem.addClass("btn btn-danger");
                        _btn_rem.html("X").attr("disabled", true);
                        _btn_rem.click(function () {

                            var lenght = $('#tblShits tbody tr').length;
                            if (lenght > 1) {
                                $(this).closest("tr").remove();
                                $(".update-shift-units").each(function (i) {
                                    $(this).text(i + 1);
                                });
                            }
                            else {
                                bootbox.alert("Atleast one row required")
                            }
                        });
                        _td_shift.append(_btn_rem);
                        _tr_shift.append(_td_shift);

                        _table.append(_tr_shift);
                    });
                }
                else {

                    var _tr_shift = $("<tr/>");
                    var _td_shift = $("<td/>");
                    // _td_shift.addClass("update-shift-units")
                    _td_shift.append("<input class='form-control update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<input class='form-control update-shift' value='1' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_0' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_0' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input isppp radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _btn_rem = $("<button/>");
                    _btn_rem.addClass("btn btn-danger remove-update-shift");
                    _btn_rem.html("X").attr("disabled", (!data.aff.en_edit));
                    _btn_rem.click(function () {

                        var lenght = $('#tblShits tbody tr').length;
                        if (lenght > 1) {
                            $(this).closest("tr").remove();
                            $(".update-shift-units").each(function (i) {
                                $(this).text(i + 1);
                            });
                        }
                        else {
                            bootbox.alert("Atleast one row required")
                        }
                    });
                    _td_shift.append(_btn_rem);
                    _tr_shift.append(_td_shift);
                    _table.append(_tr_shift);

                }

                if (data.his_list.length > 0) {
                    $("#UpdateTabHistory").show();
                    $("#tblUpdateHistory tbody").empty();
                    $.each(data.his_list, function (i) {
                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        //_tr.append("<td>" + this.TradeName + "</td");
                        _tr.append("<td>" + this.date + "</td");

                        _tr.append("<td>" + this.sent_by + "</td");
                        _tr.append("<td>" + this.Flow_user + "</td>");
                        _tr.append("<td>" + this.Status + "</td>");
                        _tr.append("<td>" + this.Remarks + "</td>");

                        //if (this.FileUploadPath != null && this.FileUploadPath != "") {
                        //    _tr.append("<td><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + this.Trade_ITI_id + "' >Download File</a></td>");
                        //}
                        //else {
                        //    _tr.append("<td>File not uploaded</td>");
                        //}

                        $("#tblUpdateHistory tbody").append(_tr);
                    });
                }
                else {
                    $("#UpdateTabHistory").hide();
                }
                $("#tblAffiliationDoc").hide();
                
                if (data.aff.AffiliationDocs.length > 0) {
                    $("#tblAffiliationDoc").show();
                    var _table = $("#tblAffiliationDoc tbody");
                    _table.empty();
                    $.each(data.aff.AffiliationDocs, function (i) {

                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.AffiliationOrder_Number + "</td>");
                        _tr.append("<td>" + this.Affiliation_date.split(' ')[0] + "</td>");
                        _tr.append("<td >" + this.FileName + "</td>");
                        _tr.append("<td ><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "&Trade_Id=" + data.aff.trade_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>" + "" + "</td>");
                        //_tr.append("<td >" + this.Status + "</td>");
                        _table.append(_tr);
                    });
                }
               

                $("#Adress_Update_Count").text(document.getElementById("Address").value.length);
                $("#Name_Update_Count").text(document.getElementById("ITIName").value.length);
                $("#MIS_Update_Count").text(document.getElementById("MISCode").value.length);
                $("#Website_Update_Count").text(document.getElementById("WebSite-Update").value.length);
                //$("#BuildUp_Count").text(document.getElementById("BuildUpArea").value.length);
                $("#GeoLocation_Update_Count").text(document.getElementById("GeoLocation").value.length);
                $("#OrderNo_Update_Count").text(document.getElementById("OrderNo-Update").value.length);

            } else {
                bootbox.alert("Error occured while loading data")
            }


        }
    });
}

//Ram added edit all login 09-07-2021

function GetUpdateTradeEditLogin(tradeId) {
    
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetUpdateTradeDetails',
        data: { trade_id: tradeId },
        success: function (data) {
            
            if (data.aff.flag == 1) {

                $("#DataGridDiv").hide();
                $("#Affiliation_div").hide();
                $("#UpdateDivEditLogin").show();
                var logid = $("#uservalue").data('value');
               
                $(".dd-docupload").hide();
                if (data.inst_list.length > 0) {
                    $("#TypeOfITI").empty();
                    $("#TypeOfITI").append($("<option/>").val("").text("Select"));
                    $.each(data.inst_list, function (index, values) {
                        $("#TypeOfITI").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.loca_type_list.length > 0) {
                    $("#LocationType").empty();
                    $("#LocationType").append($("<option/>").val("").text("Select"));
                    $.each(data.loca_type_list, function (index, values) {
                        $("#LocationType").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.trades_list.length > 0) {
                    $("#Update_Trade").empty();
                    $("#Update_Trade").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#Update_Trade").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.div_list.length > 0) {
                    $("#Division-Update").empty();
                    $("#Division-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.div_list, function (index, values) {

                        $("#Division-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.dist_list.length > 0) {
                    $("#District").empty();
                    $("#District").append($("<option/>").val("").text("Select"));
                    $.each(data.dist_list, function (index, values) {

                        $("#District").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.taluk_list.length > 0) {
                    $("#Taluka").empty();
                    $("#Taluka").append($("<option/>").val("").text("Select"));
                    $.each(data.taluk_list, function (index, values) {

                        $("#Taluka").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.consti_list.length > 0) {
                    $("#Constiteuncy").empty();
                    $("#Constiteuncy").append($("<option/>").val("").text("Select"));
                    $.each(data.consti_list, function (index, values) {

                        $("#Constiteuncy").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.cou_list.length > 0) {
                    $("#Course-Update").empty();
                    $("#Course-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.cou_list, function (index, values) {

                        $("#Course-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.schem_list.length > 0) {
                    $("#Scheme-Update").empty();
                    $("#Scheme-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.schem_list, function (index, values) {

                        $("#Scheme-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.trades_list.length > 0) {
                    $("#ListOfTrades").empty();
                    $("#ListOfTrades").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#ListOfTrades").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
               
                $("#ITIName").val(data.aff.name_of_iti);
                $("#TypeOfITI").val(data.aff.type_of_iti_id);
                $("#LocationType").val(data.aff.location_type_id);
                $("#MISCode").val(data.aff.mis_code);
                $("#Division-Update").val(data.aff.division_id);
                $("#District").val(data.aff.dist_id);
                $("#Taluka").val(data.aff.taluk_id);
                $("#Constiteuncy").val(data.aff.consti_id);
                $("#BuildUpArea").val(data.aff.build_up_area);
                $("#Address").val(data.aff.address);
                $("#GeoLocation").val(data.aff.geo_location);
                $("#Email").val(data.aff.email);
                $("#Pincode").val(data.aff.Pincode);
                $("#PhoneNumber").val(data.aff.phone_number);
                $("#AffiliationDate").val(data.aff.date);
                $("#Units").text(data.aff.no_units);
                $("#Course-Update").val(data.aff.course_code);
                $("#OrderNo-Update").val(data.aff.AffiliationOrderNo);
               
                $("#OrderNoDate-Update").val(data.aff.order_no_date);
                $("#Scheme-Update").val(data.aff.Scheme);
                $("#WebSite-Update").val(data.aff.Website);
                $("#TradeName").text(data.aff.trade);
                $("#SectorName").text(data.aff.sector);
                $("#TradeCode").text(data.aff.trade_code);
                $("#TradeType").text(data.aff.trade_type);
                $("#Duration").text(data.aff.duration);
                $("#BatchSize").text(data.aff.batch_size);
                $("#AidedUnaidedTrade").text(data.aff.AidedUnaidedTrade);
                $("#Trade_ITI_Id").val(tradeId);
                $("#CollegeId").val(data.aff.iti_college_id);
                $("#Trade_Id").val(data.aff.trade_id);
                $("#ListOfTrades").val(data.aff.trade_id);
                if (data.AffiliationDocs != null) {
                    $("#New-OrderNo-Update").val(data.aff.NewAffiliationOrderNo);
                    $("#New-OrderNoDate-Update").val(data.aff.NewAffiliationOrderNoDate);
                }
                else
                {
                    $(".newaffiliationdiv").hide();
                }
                
                if (data.aff.Filename!=null) {
                    $(".AfffiliationName").text(data.aff.Filename);
                }
               
                $("#FileAttach").empty();
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    if (data.aff.isSelect) {
                        var html = "<a title='Download Pdf' class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "&Trade_Id=" + data.aff.trade_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        $("#FileAttach").append(html);
                        $("#IsUploaded").val(true);
                    }
                    else {
                        var html = "<p>File not found</p>"
                        $("#FileAttach").append(html);
                        $("#IsUploaded").val(false);
                    }

                }
                else {
                    $("#IsUploaded").val(false);
                }
                if (data.aff.UploadTradeAffiliationDoc != null && data.aff.UploadTradeAffiliationDoc != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                    $("#TradeFileAttach").append(html);
                    // $("#IsUploaded").val(true);
                }
                else {
                    $("#tradedoc").hide();
                    $("#tradedoc1").hide();
                }

              
                if (!data.aff.en_edit) {
                    $(".myoption").attr("disabled", true)
                }
                else {
                    $(".myoption").attr("disabled", false)
                }
                $("#btn-send").prop('value', 'Submit');
                
                if (logid == 5) {
                    if (logid == 5 && data.aff.AffiliationDocs.length == 0 /*data.aff.FileUploadPath == null*/ && data.aff.en_edit && (data.aff.status_id == 6 ||data.aff.status_id == 19)) {
                        $(".dd-docupload").show();
                        $("#btn-send").attr("disabled", false)
                    }
                    else if (logid == 5 && data.aff.en_edit == false && data.aff.AffiliationDocs.length == 0/*data.aff.FileUploadPath == null*/ && (data.aff.status_id == 6 ||data.aff.status_id == 19)) {
                        $(".dd-docupload").show();
                        $("#btn-send").attr("disabled", false)
                        $("#btn-send").prop('value', 'Save');
                    }
                    //else
                    //{
                    //    $(".dd-docupload").hide();
                    //    $("#btn-send").attr("disabled", true)
                    //}
                }
                //$(".update-button").attr("disabled", (!data.aff.en_edit));
                //$(".update-row-new-units").attr("disabled", (!data.aff.en_edit));


                var _table = $("#tblShits tbody");
                _table.empty();
                //$.each(data.shifts, function (i) {

                //    var _tr = $("<tr/>");
                //    _tr.append("<td>" + (i + 1) + "</td>");
                //    _tr.append("<td>" + this.Units + "</td");
                //    _tr.append("<td>" + this.Shift + "</td>");
                //    _tr.append("<td>" + this.IsPPP + "</td>");
                //    _tr.append("<td>" + this.Dual_System + "</td>");
                //    _table.append(_tr);
                //});

                if (data.aff.shifts.length > 0) {
                    $.each(data.aff.shifts, function (i) {

                        var _tr_shift = $("<tr/>");
                        var _td_shift = $("<td/>");
                        //_td_shift.addClass("update-shift-units")
                        _td_shift.append("<input class='form-control update-shift-units' value='" + this.Units + "' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<input class='form-control update-shift' value='" + this.Shift + "' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='" + this.ITI_Trade_Shift_Id + "' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        var _checked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' checked>";
                        var _unchecked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' >";
                        var _unchecked_no = "<input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no'>";
                        var _checked_no = "<input class='form-check-input  isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no' checked>"
                        if (this.IsPPP == 'yes') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _unchecked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _checked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        _tr_shift.append(_td_shift);
                        var _checked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' checked>";
                        var _unchecked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' >"
                        var _unchecked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' >";
                        var _checked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' checked>"

                        _td_shift = $("<td/>");
                        if (this.Dual_System == 'regular') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _unchecked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _checked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        if (this.IsActive) {
                            _td_shift.text("Active")
                        }
                        else {
                            if (this.Status == 5) {
                                _td_shift.text("Pending for Approval");
                            }
                            else {
                                _td_shift.text("DeActive");
                            }

                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        _btn_rem = $("<button/>");
                        _btn_rem.addClass("btn btn-danger");
                        _btn_rem.html("X").attr("disabled", true);
                        _btn_rem.click(function () {

                            var lenght = $('#tblShits tbody tr').length;
                            if (lenght > 1) {
                                $(this).closest("tr").remove();
                                $(".update-shift-units").each(function (i) {
                                    $(this).text(i + 1);
                                });
                            }
                            else {
                                bootbox.alert("Atleast one row required")
                            }
                        });
                        _td_shift.append(_btn_rem);
                        _tr_shift.append(_td_shift);

                        _table.append(_tr_shift);
                    });
                }
                else {

                    var _tr_shift = $("<tr/>");
                    var _td_shift = $("<td/>");
                    // _td_shift.addClass("update-shift-units")
                    _td_shift.append("<input class='form-control update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<input class='form-control update-shift' value='1' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_0' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_0' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input isppp radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _btn_rem = $("<button/>");
                    _btn_rem.addClass("btn btn-danger remove-update-shift");
                    _btn_rem.html("X").attr("disabled", (!data.aff.en_edit));
                    _btn_rem.click(function () {

                        var lenght = $('#tblShits tbody tr').length;
                        if (lenght > 1) {
                            $(this).closest("tr").remove();
                            $(".update-shift-units").each(function (i) {
                                $(this).text(i + 1);
                            });
                        }
                        else {
                            bootbox.alert("Atleast one row required")
                        }
                    });
                    _td_shift.append(_btn_rem);
                    _tr_shift.append(_td_shift);
                    _table.append(_tr_shift);

                }

                if (data.his_list.length > 0) {
                    $("#UpdateTabHistory").show();
                    $("#tblUpdateHistory tbody").empty();
                    $.each(data.his_list, function (i) {
                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        //_tr.append("<td>" + this.TradeName + "</td");
                        _tr.append("<td>" + this.date + "</td");

                        _tr.append("<td>" + this.sent_by + "</td");
                        _tr.append("<td>" + this.Flow_user + "</td>");
                        _tr.append("<td>" + this.Status + "</td>");
                        _tr.append("<td>" + this.Remarks + "</td>");

                        //if (this.FileUploadPath != null && this.FileUploadPath != "") {
                            //_tr.append("<td><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + this.Trade_ITI_id + "' >Download File</a></td>");
                        //}
                        //else {
                        //    _tr.append("<td>File not uploaded</td>");
                        //}

                        $("#tblUpdateHistory tbody").append(_tr);
                    });
                }
                else {
                    $("#UpdateTabHistory").hide();
                }
                $("#tblAffiliationDoc").hide();
                if (data.aff.AffiliationDocs.length > 0) {
                    $("#tblAffiliationDoc").show();
                    var _table = $("#tblAffiliationDoc tbody");
                    _table.empty();
                    $.each(data.aff.AffiliationDocs, function (i) {

                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.AffiliationOrder_Number + "</td>");
                        _tr.append("<td>" + this.Affiliation_date.split(' ')[0] + "</td>");
                        _tr.append("<td >" + this.FileName + "</td>");
                        _tr.append("<td ><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "&Trade_Id=" + data.aff.trade_id +"' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>" + "" + "</td>");
                        //_tr.append("<td >" + this.Status + "</td>");
                        _table.append(_tr);
                    });
                }

                $("#Adress_Update_Count").text(document.getElementById("Address").value.length);
                $("#Name_Update_Count").text(document.getElementById("ITIName").value.length);
                $("#MIS_Update_Count").text(document.getElementById("MISCode").value.length);
                $("#Website_Update_Count").text(document.getElementById("WebSite-Update").value.length);
                //$("#BuildUp_Count").text(document.getElementById("BuildUpArea").value.length);
                $("#GeoLocation_Update_Count").text(document.getElementById("GeoLocation").value.length);
                $("#OrderNo_Update_Count").text(document.getElementById("OrderNo-Update").value.length);

            } else {
                bootbox.alert("Error occured while loading data")
            }


        }
    });
}



//Send back for correction save
function SaveUpdates() {
    
    var fileUpload = $('#AffilicationDoc').get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    var UnitfileUpload = $('#AffiliationUnitDoc').get(0);
    var unitfiles = UnitfileUpload.files;
    for (var i = 0; i < unitfiles.length; i++) {
        fileData.append("UploadTradeAffiliationDoc", unitfiles[i]);
    }
    var neworderno = $('#New-OrderNo-Update').val();
    var IsValid = true;
    var name = $("#ITIName").val();
    var flow_id = $("#dd-flow").val();
    var remarks = $("#Remarks").val();
    if (remarks == null || remarks == "")
    {
        IsValid = false;
        return bootbox.alert("Pls enter remarks");
    }
    if (flow_id == "") {
        IsValid = false;
        return bootbox.alert("Pls select send to");
    }
    if (name == "") {
        $("#ITIName-update-Required").show();
        IsValid = false;
    }
    else {
        $("#ITIName-update-Required").hide();

    }
    var type = $("#TypeOfITI").val();
    if (type == "" || type == "0" || type == null) {
        $("#TypeOfITI-update-Required").show()
        IsValid = false;
    }
    else {
        $("#TypeOfITI-update-Required").hide()

    }
    var loca = $("#LocationType").val();
    if (loca == "" || loca == "0" || loca == null) {
        $("#LocationType-update-Required").show();
        IsValid = false;
    }
    else {
        $("#LocationType-update-Required").hide();

    }
    var mis = $("#MISCode").val();
    if (mis == "" || mis == "0" || mis == null) {
        $("#MISCode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#MISCode-update-Required").hide();

    }
    var div = $("#Division-Update").val();
    if (div == "" || div == "0" || div == null) {
        $("#Divisions-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Divisions-update-Required").hide();

    }

    var dist = $("#District").val();
    if (dist == "" || dist == "0" || dist == null) {
        $("#District-update-Required").show();
        IsValid = false;
    }
    else {
        $("#District-update-Required").hide();

    }
    var taluk = $("#Taluka").val();
    if (taluk == "" || taluk == "0" || taluk == null) {
        $("#Taluka-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Taluka-update-Required").hide();

    }
    var consti = $("#Constiteuncy").val();
    if (consti == "" || consti == "0" || consti == null) {
        return bootbox.alert("Pls select constituency");
        $("#Constiteuncy-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Constiteuncy-update-Required").hide();

    }
    var buildup = $("#BuildUpArea").val();
    if (buildup == "") {
        $("#BuildUpArea-update-Required").show();
        IsValid = false;
    }
    else {
        $("#BuildUpArea-update-Required").hide();

    }
    var address = $("#Address").val();
    if (address == "") {
        $("#Address-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Address-update-Required").hide();

    }
    var geo = $("#GeoLocation").val();
    if (geo == "") {
        $("#GeoLocation-update-Required").show();
        IsValid = false;
    }
    else {
        $("#GeoLocation-update-Required").hide();

    }
    var email = $("#Email").val();
    if (email == "") {
        $("#Email-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Email-update-Required").hide();

    }
    var pincode = $("#Pincode").val();
    if (pincode == "") {
        $("#Pincode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Pincode-update-Required").hide();
    }
    var phone = $("#PhoneNumber").val();
    if (phone == "") {
        $("#PhoneNumber-update-Required").show();
        IsValid = false;
    }
    else {
        $("#PhoneNumber-update-Required").hide();

    }
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    var affidate = "";
    var date = "";
    if ($("#AffiliationDate").val() != "" && $("#AffiliationDate").val() != null) {
        var dts = $("#AffiliationDate").val().split("/");
        affidate = new Date(dts[0] + "/" + dts[1] + "/" + dts[2]);
        
        // date = (dts[0] + "-" + dts[1] + "-" + dts[2]);
        date = (dts[2] + "/" + dts[1] + "/" + dts[0]);
        $("#AffiliationDate-update-Required").hide();
    }
    else {
        IsValid = false;
        $("#AffiliationDate-update-Required").show();
    }
    var units = $("#Units").text();


    //var File = $("#AffilicationDoc").val();
    //if (File == "") {
    //    var isUpload = $("#IsUploaded").val()
    //    if (isUpload == "false") {

    //        $("#file-update-Required").show();
    //        IsValid = false;
    //    }

    //}
    //else {
    //    $("#file-update-Required").hide();
    //}
    var course = $("#Course-Update").val();
    if (course == "") {
        $("#Course-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Course-update-Required").hide();

    }
    var orderno = $("#OrderNo-Update").val();
    if (orderno == "") {
        $("#OrderNo-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#OrderNo-Update-Required").hide();
    }

    //var orderdate = "";
    //if ($("#OrderNoDate-Update").val() != "") {
    //    var dts = $("#OrderNoDate-Update").val().split("-");
    //    orderdate = (dts[2] + "/" + dts[1] + "/" + dts[0]);
    //    dts = orderdate
    //    $("#OrderNoDate-Update-Required").hide();
    //}
    //else {
    //    IsValid = false;
    //    $("#OrderNoDate-Update-Required").show();
    //}
    var orderdate = "";
    if ($("#OrderNoDate-Update").val() != "" && $("#OrderNoDate-Update").val() != null) {
        var dts = $("#OrderNoDate-Update").val().split("/");
        orderdate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
        if (orderdate == "Invalid Date") {
            return bootbox.alert("Please Select Valid Affiliation Date");
        }

        orderdate = (dts[0] + "/" + dts[1] + "/" + dts[2]);
        //if (orderdate > today) {
        //    return bootbox.alert("Affiliation date is greater than current date, pls select date less than or equal to current date  ")
        //}
        $("#OrderNoDate-Update-Required").hide();
    }
    else {
        IsValid = false;
        $("#OrderNoDate-Update-Required").show();
    }
    var neworderdate = "";
    if ($("#New-OrderNoDate-Update").val() != "") {
        var dts = $("#New-OrderNoDate-Update").val().split("/");
        neworderdate = (dts[0] + "/" + dts[1] + "/" + dts[2]);
        //if (neworderdate > today) {
        //    return bootbox.alert("New Affiliation Date is greater than current date, pls select date less than or equal to current date  ")
        //}
        $("#New-OrderNoDate-Update-Required").hide();
    }
    
    //else {
    //    IsValid = false;
    //    $("#New-OrderNoDate-Update-Required").show();
    //}
    var scheme = $("#Scheme-Update").val();
    if (scheme == "") {
        $("#Scheme-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#Scheme-Update-Required").hide();
    }
    var website = $("#WebSite-Update").val();
    var collegeId = $("#CollegeId").val();
    var trade_iti_id = $("#Trade_ITI_Id").val();
    var trade_id = $("#ListOfTrades").val();
    if (trade_id == "") {
        $(".update-trade-multi-select-required").show();
        IsValid = false;
    }
    else {
        $(".update-trade-multi-select-required").hide();
    }

    //$("#Table-Trade-update").each(function () {

    //    var $tr = $(this);
    //    var trade = $tr.find(".trade-multi-select").val()
    //    if (trade == "" || trade == null || trade == 0) {
    //        $tr.find(".update-trade-multi-select-required").show();
    //        IsValid = false;
    //    }
    //    else {
    //        $tr.find(".update-trade-multi-select-required").hide();
    //    }
    //});
    // IsValid = true;
    let isValidShift = true;

    var _shift_table = $("#tblShits tbody");
    var sendObj = [];

    _shift_table.find("tr").each(function (len) {

        var $tr = $(this);
        var unit = $tr.find(".update-shift-units").val();
        if (unit == "" || unit == null || unit == "0") {
            isValid = false;
            $tr.find(".update-shift-units-required").show();
        }
        else {
            $tr.find(".update-shift-units-required").hide();
        }
        var shift = $tr.find(".update-shift").val();
        if (shift == "" || shift == null || shift == "0") {
            isValid = false;
            $tr.find(".update-shift-required").show();
        }
        else {
            $tr.find(".update-shift-required").hide();
        }
        var isppp = $tr.find("input[name='IsPPP_" + len + "']:checked").val();
        var dual = $tr.find("input[name='Dual_System_" + len + "']:checked").val();
        var trade_shift_id = $tr.find(".trade_shift_id").val();

        var obj = {
            Units: unit,
            Shift: shift,
            IsPPP: isppp,
            Dual_System: dual,
            ITI_Trade_Shift_Id: trade_shift_id
        };

        fileData.append(
            "shifts[" + len + "].Units", unit
        );
        fileData.append(
            "shifts[" + len + "].Shift", shift
        );
        fileData.append(
            "shifts[" + len + "].IsPPP", isppp
        );
        fileData.append(
            "shifts[" + len + "].Dual_System", dual
        );
        fileData.append(
            "shifts[" + len + "].ITI_Trade_Shift_Id", trade_shift_id
        );

        sendObj.push(obj);
    });


    const unique = []
    sendObj.filter(o => {

        if (unique.find(i => i.Units === o.Units && i.Shift === o.Shift)) {

            isValidShift = false;

        }
        else {
            unique.push(o)

        }

    });

    if (!isValidShift) {
        $("#validate-shift-update").show();
    } else {
        $("#validate-shift-update").hide();
    }

    if (IsValid && isValidShift) {
        $(".update-button").attr("disabled", true);

        fileData.append(
            "name_of_iti", name
        );
        fileData.append(
            "type_of_iti_id", type
        );
        fileData.append(
            "location_type_id", loca
        );
        fileData.append(
            "mis_code", mis
        );
        fileData.append(
            "division_id", div
        );
        fileData.append(
            "dist_id", dist
        );
        fileData.append(
            "taluk_id", taluk
        );
        fileData.append(
            "consti_id", consti
        );
        fileData.append(
            "flow_id", flow_id
        );            
        fileData.append(
            "remarks", remarks
        );
        fileData.append(
            "build_up_area", buildup
        );
        fileData.append(
            "address", address
        );
        fileData.append(
            "geo_location", geo
        );
        fileData.append(
            "email", email
        );
        fileData.append(
            "affiliation_date", affidate
        );
        fileData.append(
            "phone_number", phone
        );
        fileData.append(
            "no_units", units
        );
        //fileData.append(
        //	"no_shifts", shifts
        //);

        fileData.append(
            "iti_college_id", collegeId
        );
        fileData.append(
            "date", date
        );
        fileData.append(
            "Pincode", pincode
        );
        fileData.append(
            "course_code", course
        );
        fileData.append(
            "Website", website
        );
        fileData.append(
            "AffiliationOrderNo", orderno
        );

        fileData.append(
            "Scheme", scheme
        );
        fileData.append(
            "order_no_date", orderdate
        );
        //fileData.append(
        //    "list[]", JSON.stringify(sendObj)
        //);
        fileData.append(
            "trade_iti_id", trade_iti_id
        );
        fileData.append(
            "trade_id", trade_id
        );
        fileData.append(
            "NewAffiliationOrderNoDate", neworderdate
        );
        fileData.append(
            "NewAffiliationOrderNo", neworderno
        );

        if (flow_id == 5) {
            var sendto = "Deputy Director";
        }
        else if (flow_id == 3) {
            sendto = "Assistant Director";
        }
        else if (flow_id == 8) {
            sendto = "Case Worker";
        }
        else if (flow_id == 7) {
            sendto = "Office Superintendent";
        }

        bootbox.confirm('<br><br>Are you sure you want Send ' + name + ' Updated  Details to ' + sendto +' ?', (confirma) => {
            if (confirma) {
                $.ajax({
                    type: 'POST',
                    url: '/Affiliation/UpdateCollegeAffilationDetails',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {

                        if (result.flag == 1) {

                            ClearFields();
                            bootbox.alert("<br/><br/> " + name + " trade updated  details sent to " + sendto +"  for review")
                            //bootbox.alert("Affiliation ITI Institute Trade Details Updated and sent to Office Superintendent")
                            fnRefreshScreenData();
                        }
                        else {
                            bootbox.alert(result.status)
                        }

                        $(".update-button").attr("disabled", false);
                    }
                });

            }
        });
    }

}

    function ExcelExportTentative() {
	    
	    var today = new Date();
	    var dd = String(today.getDate()).padStart(2, '0');
	    var mm = String(today.getMonth() + 1).padStart(2, '0');
	    var yyyy = today.getFullYear();
	    today = dd + '/' + mm + '/' + yyyy;
        $('#tblGridAffiliatedColleges').DataTable().destroy();
        $('#tblGridAffiliatedColleges').DataTable({ "paging": false }).draw(false);
        $("#tblGridAffiliatedColleges").table2excel({
	        filename: "ViewAffiliatedInstitute" + today + ".xls",
        });
        $('#tblGridAffiliatedColleges').DataTable().destroy();
        $('#tblGridAffiliatedColleges').DataTable({ "paging": true }).draw(false);
	}
function PublishInstitute() {
    
    var IsValid = true;
    $('#tblGridAffiliatedColleges').DataTable().destroy();
    $('#tblGridAffiliatedColleges').DataTable({ "paging": false }).draw(false);
    var _Publish_table = $("#tblGridAffiliatedColleges tbody");
    var sendObj = [];


    _Publish_table.find("tr").each(function (len) {
        //t.destroy();
        var $tr = $(this);
        var trade_iti_id = $tr.find(".trade_iti_id").val();
        var status = $tr.find(".status").val();


        var obj = {

            trade_iti_id_publish: trade_iti_id,
            status: status,


        };
        sendObj.push(obj);
    });
    var finalObj = JSON.stringify({ pubs_list_topublish: sendObj });

    $('#tblGridAffiliatedColleges').DataTable().destroy();
    $('#tblGridAffiliatedColleges').DataTable({ "paging": true }).draw(false);
    //if (IsValid) {

    bootbox.confirm("<br><br>Are you sure you want  to publish the Institute(S) details ?", function (confirmed) {
        if (confirmed) {
            
            console.log(finalObj);
            // if (IsValid) {
            $.ajax({
                type: 'POST',
                url: '/Affiliation/PublishITIInstitute',
                //contentType: false, // Not to set any content header

                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                // dataType: 'json',

                data: finalObj,
                success: function (result) {

                    if (result) {

                        bootbox.alert("ITI Institutes details published successfully")
                        
                        $("#Remarks").val('');
                        $("#uploadfile").val('');
                        //GetAllActiveDeactiveRequestDetails();
                        //$("#tblGridUpdateAffiliation_wrapper").show();
                        //$("#Affiliation_view").hide();
                        success: callback
                    }
                    else {
                        bootbox.alert("failed")
                    }
                }
            });
            // }
        }
    });
    //}
}

//CW Save After upload
function SaveUploadedAffiliationTrade() {
    
    var AidedUnaidedTrade = $("#AidedUnaidedTrade").text();
    var fileUpload = $('#AffilicationDoc').get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    var flow_id = $("#dd-flow").val();
    if (flow_id == null || flow_id == "") {
        return bootbox.alert("Pls select Send to")
    }
    var remarks = $("#Remarks").val();
    if (remarks == null || remarks == "") {
        return bootbox.alert("pls enter Remarks");
    }
    var IsValid = true;
    var name = $("#ITIName").val();
    var neworderno = $("#New-OrderNo-Update").val();
    var newdate = $("#New-OrderNoDate-Update").val();
    if (name == "") {
        $("#ITIName-update-Required").show();
        IsValid = false;
    }
    else {
        $("#ITIName-update-Required").hide();

    }
    var type = $("#TypeOfITI").val();
    if (type == "" || type == "0" || type == null) {
        return bootbox.alert("Select type of ITI");
        $("#TypeOfITI-update-Required").show()
        IsValid = false;
    }
    else {
        $("#TypeOfITI-update-Required").hide()

    }
    var loca = $("#LocationType").val();
    if (loca == "" || loca == "0" || loca == null) {
        return bootbox.alert("Pls Select Location type ");
        $("#LocationType-update-Required").show();
        IsValid = false;
    }
    else {
        $("#LocationType-update-Required").hide();

    }
    var mis = $("#MISCode").val();
    if (mis == "" || mis == "0" || mis == null) {
        return bootbox.alert("Pls select Miscode ");
        $("#MISCode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#MISCode-update-Required").hide();

    }
    var div = $("#Division-Update").val();
    if (div == "" || div == "0" || div == null) {
        return bootbox.alert("Pls select Division ");
        $("#Divisions-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Divisions-update-Required").hide();

    }

    var dist = $("#District").val();
    if (dist == "" || dist == "0" || dist == null) {
        return bootbox.alert("Pls select District ");
        $("#District-update-Required").show();
        IsValid = false;
    }
    else {
        $("#District-update-Required").hide();

    }
    var taluk = $("#Taluka").val();
    if (taluk == "" || taluk == "0" || taluk == null) {
        return bootbox.alert("Pls select Taluk ");
        $("#Taluka-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Taluka-update-Required").hide();

    }
    var consti = $("#Constiteuncy").val();
    if (consti == "" || consti == "0" || consti == null) {
        return bootbox.alert("Pls select Constituency ");
        $("#Constiteuncy-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Constiteuncy-update-Required").hide();

    }

    var buildup = $("#BuildUpArea").val();
    if (buildup == "") {
        $("#BuildUpArea-update-Required").show();
        IsValid = false;
    }
    else {
        $("#BuildUpArea-update-Required").hide();

    }
    var address = $("#Address").val();
    if (address == "") {
        return bootbox.alert("Pls select Address ");
        $("#Address-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Address-update-Required").hide();

    }
    var geo = $("#GeoLocation").val();
    if (geo == "") {
        return bootbox.alert("Pls enter GeoLocation ");
        $("#GeoLocation-update-Required").show();
        IsValid = false;
    }
    else {
        $("#GeoLocation-update-Required").hide();

    }
    var email = $("#Email").val();
    if (email == "") {
        return bootbox.alert("Pls enter Email ");
        $("#Email-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Email-update-Required").hide();

    }
    var pincode = $("#Pincode").val();
    if (pincode == "") {
        return bootbox.alert("Pls enter PinCode ");
        $("#Pincode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Pincode-update-Required").hide();
    }
    var phone = $("#PhoneNumber").val();
    if (phone == "") {
        return bootbox.alert("Pls Enter Phone No. ");
        $("#PhoneNumber-update-Required").show();
        IsValid = false;
    }
    else {
        $("#PhoneNumber-update-Required").hide();

    }

    //var today = new Date();
    //var dd = String(today.getDate()).padStart(2, '0');
    //var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    //var yyyy = today.getFullYear();

    //today = dd + '-' + mm + '-' + yyyy;
    var affidate = "";
    var date = "";
    if ($("#AffiliationDate").val() != "" && $("#AffiliationDate").val() != null) {
        var dts = $("#AffiliationDate").val().split("/");
        affidate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
        if (affidate == "Invalid Date") {
            return bootbox.alert("Pls Select Valid EstablishmentDate");
        }
        date = (dts[0] + "/" + dts[1] + "/" + dts[2]);

        //if (new Date(date) > new Date(today)) {
        //    return bootbox.alert("Date of Establishment is greater than current date, pls select date less than or equal to current date  ")
        //}
        $("#AffiliationDate-update-Required").hide();
    }
    else {
        IsValid = false;
        $("#AffiliationDate-update-Required").show();
    }
    var units = $("#Units").text();

    //var File = $("#AffilicationDoc").val();
    //if (File == "") {
    //    var isUpload = $("#IsUploaded").val()
    //    if (isUpload == "false") {
    //        return bootbox.alert("Pls Select  Affiliation Pdf Doc ");
    //    }
    //    else {
    //            IsValid = false;
    //            $("#file-update-Required").show();
    //        }
    //    }          

    //else {
    //    $("#file-update-Required").hide();
    //}
    var course = $("#Course-Update").val();
    if (course == "") {
        return bootbox.alert("Pls select Course ");
        $("#Course-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#Course-Update-Required").hide();

    }
    var orderno = $("#OrderNo-Update").val();
    if (orderno == "") {
        return bootbox.alert("Pls select Affiliation OrderNo ");
        $("#OrderNo-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#OrderNo-Update-Required").hide();
    }

    //
    //var orderdate = "";
    //var date = "";
    //if ($("#OrderNoDate-Update").val() != "" && $("#OrderNoDate-Update").val() != null) {
    //    var dts = $("#OrderNoDate-Update").val().split("-");
    //    orderdate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
    //    if (orderdate == "Invalid Date") {
    //        return bootbox.alert("Pls Select Valid AffiliationDate");
    //    }
    //    date = (dts[0] + "-" + dts[1] + "-" + dts[2]);
    //    $("#OrderNoDate-Update-Required").hide();
    //}
    //else {
    //    IsValid = false;
    //    $("#OrderNoDate-Update-Required").show();
    //}

    var orderdate = "";
    if ($("#OrderNoDate-Update").val() != "" && $("#OrderNoDate-Update").val() != null) {
        var dts = $("#OrderNoDate-Update").val().split("/");
        orderdate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
        if (orderdate == "Invalid Date") {
            return bootbox.alert("Please Select Valid Affiliation Date");
        }

        orderdate = (dts[0] + "/" + dts[1] + "/" + dts[2]);
        //if (orderdate > today) {
        //    return bootbox.alert("Affiliation date is greater than current date, pls select date less than or equal to current date  ")
        //}
        $("#OrderNoDate-Update-Required").hide();
    }
    else {
        IsValid = false;
        $("#OrderNoDate-Update-Required").show();
    }

    var scheme = $("#Scheme-Update").val();
    if (scheme == "") {
        return bootbox.alert("Pls select Scheme ");
        $("#Scheme-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#Scheme-Update-Required").hide();
    }
    var website = $("#WebSite-Update").val();
    var collegeId = $("#CollegeId").val();
    var trade_iti_id = $("#Trade_ITI_Id").val();
    var trade_id = $("#ListOfTrades").val();
    if (trade_id == "") {
        return bootbox.alert("Pls select Trade ");
        $(".update-trade-multi-select-required").show();
        IsValid = false;
    }
    else {
        $(".update-trade-multi-select-required").hide();
    }

    // IsValid = true;
    let isValidShift = true;

    var _shift_table = $("#tblShits tbody");
    var sendObj = [];

    _shift_table.find("tr").each(function (len) {

        var $tr = $(this);
        var unit = $tr.find(".update-shift-units").val();
        if (unit == "" || unit == null || unit == "0") {
            isValid = false;
            return bootbox.alert("Pls Add units ");
            $tr.find(".update-shift-units-required").show();
        }
        else {
            $tr.find(".update-shift-units-required").hide();
        }
        var shift = $tr.find(".update-shift").val();
        if (shift == "" || shift == null || shift == "0") {
            return bootbox.alert("Pls Add Shift ");
            isValid = false;
            $tr.find(".update-shift-required").show();
        }
        else {
            $tr.find(".update-shift-required").hide();
        }
        var isppp = $tr.find("input[name='IsPPP_" + len + "']:checked").val();
        var dual = $tr.find("input[name='Dual_System_" + len + "']:checked").val();
        var trade_shift_id = $tr.find(".trade_shift_id").val();
        var ActiveStatus = $tr.find(".active_status").val();

        var add = 0;
        //checking for new added units for existing units
        if (typeof ActiveStatus === "undefined") {
            add = 1;
        }
        var obj = {
            Units: unit,
            Shift: shift,
            IsPPP: isppp,
            Dual_System: dual,
            ITI_Trade_Shift_Id: trade_shift_id,
            IsActive: ActiveStatus,
            new_add: add
        };

        fileData.append(
            "shifts[" + len + "].Units", unit
        );
        fileData.append(
            "shifts[" + len + "].Shift", shift
        );
        fileData.append(
            "shifts[" + len + "].IsPPP", isppp
        );
        fileData.append(
            "shifts[" + len + "].Dual_System", dual
        );
        fileData.append(
            "shifts[" + len + "].ITI_Trade_Shift_Id", trade_shift_id
        );

        fileData.append(
            "shifts[" + len + "].IsActive", ActiveStatus
        );

        fileData.append(
            "shifts[" + len + "].new_add", add
        );

        sendObj.push(obj);
    });


    const unique = []
    sendObj.filter(o => {

        if (unique.find(i => i.Units === o.Units && i.Shift === o.Shift)) {

            isValidShift = false;

        }
        else {
            unique.push(o)

        }

    });

    if (!isValidShift) {
        $("#validate-shift-update").show();
    } else {
        $("#validate-shift-update").hide();
    }

    if (IsValid && isValidShift) {
        $(".update-button").attr("disabled", true);

        fileData.append(
            "name_of_iti", name
        );
        fileData.append(
            "type_of_iti_id", type
        );
        fileData.append(
            "location_type_id", loca
        );
        fileData.append(
            "mis_code", mis
        );
        fileData.append(
            "division_id", div
        );
        fileData.append(
            "dist_id", dist
        );
        fileData.append(
            "taluk_id", taluk
        );
        fileData.append(
            "consti_id", consti
        );
        fileData.append(
            "flow_id", flow_id
        );
        fileData.append(
            "remarks", remarks
        );
        fileData.append(
            "build_up_area", buildup
        );
        fileData.append(
            "address", address
        );
        fileData.append(
            "geo_location", geo
        );
        fileData.append(
            "email", email
        );
        fileData.append(
            "affiliation_date", affidate
        );
        fileData.append(
            "phone_number", phone
        );
        fileData.append(
            "no_units", units
        );
        //fileData.append(
        //	"no_shifts", shifts
        //);

        fileData.append(
            "iti_college_id", collegeId
        );
        fileData.append(
            "date", date
        );
        fileData.append(
            "Pincode", pincode
        );
        fileData.append(
            "course_code", course
        );
        fileData.append(
            "Website", website
        );
        fileData.append(
            "AffiliationOrderNo", orderno
        );

        fileData.append(
            "Scheme", scheme
        );
        fileData.append(
            "order_no_date", orderdate
        );
        //fileData.append(
        //    "list[]", JSON.stringify(sendObj)
        //);
        fileData.append(
            "trade_iti_id", trade_iti_id
        );
        fileData.append(
            "trade_id", trade_id
        );

        fileData.append(
            "NewAffiliationOrderNo", neworderno
        );
        fileData.append(
            "NewAffiliationOrderNoDate", newdate
        );
        fileData.append(
            "AidedUnaidedTrade", AidedUnaidedTrade
        );
        //$("#Table-Trade-update tbody tr").each(function () {

        //    var $tr = $(this);
        //    var trade = $tr.find(".trade-multi-select").val();
        //    var unit = $tr.find(".text-multi-units").text();
        //    var key = $tr.find(".update-sessionkey").val();
        //    //var isupload = $tr.find(".update-trade-isupload").val();
        //    var trade_iti_id = $tr.find(".update-trade_iti_id").val();

        //    fileData.append(
        //        "list_trades", trade
        //    );
        //    fileData.append(
        //        "list_units", unit
        //    );
        //    //fileData.append(
        //    //    "is_uploads", isupload
        //    //);
        //    fileData.append(
        //        "list_it_trade_id", trade_iti_id
        //    );
        //    fileData.append(
        //        "list_keys", key
        //    );

        //});
        
        if (flow_id == 5) {
            var name = "Deputy Director";
        }
        else if (flow_id == 3) {
            name = "Assistant Director";
        }
        else if (flow_id == 8) {
            name = "Case Worker";
        }
        else if (flow_id == 7) {
            name = "Office Superintendent";
        }




        bootbox.confirm("<br><br> Are you sure you want to submit  " + $("#ITIName").val() + ' details For review to ' + name + ' ?', (confirma) => {
            if (confirma) {
                $.ajax({
                    type: 'POST',
                    url: '/Affiliation/SaveUploadedAffiliationCollegeDetails',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {

                        if (result.flag == 1) {
                            
                            $(".update-button").show();
                            $(".save-button").hide();
                            bootbox.alert("<br><br>" + $("#ITIName").val() + " details submitted to " + name + " for review Successfully.")
                            ClearFields();
                            GetAllUploadedAffiliationColleges();
                            fnRefreshScreenData();





                        }
                        else {
                            bootbox.alert(result.status)
                        }

                        $(".update-button").attr("disabled", false);
                    }
                });

            }
        });
    }
}

function fnRefreshScreenData() {
    var url = '/Affiliation/AffiliationCollegeDetails1';
    window.location.replace(url);
}

function GetUploadedInstitute(collegeId) {


    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetUploadedCollegeDetails',
        data: { collegId: collegeId },
        success: function (data) {
            if (data != null) {

                $("#DataGridDiv").hide();
                $("#UpdateDiv").show();
                //button 
                $(".update-button").hide();
                $(".save-button").show();

                if (data.inst_list.length > 0) {
                    $.each(data.inst_list, function (index, values) {
                        $("#TypeOfITI").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.loca_type_list.length > 0) {
                    $.each(data.loca_type_list, function (index, values) {
                        $("#LocationType").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.css_code_list.length > 0) {
                //	$.each(data.css_code_list, function (index, values) {
                //		$("#CssCode").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                if (data.trades_list.length > 0) {
                    $.each(data.trades_list, function (index, values) {
                        $("#Update_Trade").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.dist_list.length > 0) {
                    $.each(data.dist_list, function (index, values) {
                        $("#District").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.taluk_list.length > 0) {
                    $.each(data.taluk_list, function (index, values) {
                        $("#Taluka").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.consti_list.length > 0) {
                    $.each(data.consti_list, function (index, values) {
                        $("#Constiteuncy").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.cou_list.length > 0) {
                    $.each(data.cou_list, function (index, values) {
                        $("#Course-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.schem_list.length > 0) {
                    $.each(data.schem_list, function (index, values) {
                        $("#Scheme-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.pancha_list.length > 0) {
                //	$.each(data.pancha_list, function (index, values) {
                //		$("#Panchayat").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                //if (data.village_list.length > 0) {
                //	$.each(data.village_list, function (index, values) {
                //		$("#Village").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}

                $("#ITIName").val(data.aff.name_of_iti);
                $("#TypeOfITI").val(data.aff.type_of_iti_id);
                $("#LocationType").val(data.aff.location_type_id);
                $("#MISCode").val(data.aff.mis_code);
                // $("#CssCode").val(data.aff.css_code);
                $("#District").val(data.aff.dist_id);
                $("#Taluka").val(data.aff.taluk_id);
                $("#Constiteuncy").val(data.aff.consti_id);
                //$("#Panchayat").val(data.aff.pancha_id);
                //$("#Village").val(data.aff.village_id);
                $("#BuildUpArea").val(data.aff.build_up_area);
                $("#Address").val(data.aff.address);
                $("#GeoLocation").val(data.aff.geo_location);
                $("#Email").val(data.aff.email);
                $("#Pincode").val(data.aff.Pincode);
                $("#PhoneNumber").val(data.aff.phone_number);
                $("#AffiliationDate").val(data.aff.date);
                $("#Units").text(data.aff.no_units);
                $("#Course-Update").val(data.aff.course_code);
                $("#OrderNo-Update").val(data.aff.AffiliationOrderNo);
                $("#OrderNoDate-Update").val(data.aff.order_no_date);
                $("#Scheme-Update").val(data.aff.Scheme);
                $("#WebSite-Update").val(data.aff.Website);
                $("#CollegeId").val(collegeId);

                $("#FileAttach").empty();
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId + "' >Download File</a>"
                    $("#FileAttach").append(html);
                    $("#IsUploaded").val(true);
                }
                else {
                    $("#IsUploaded").val(false);
                }
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId + "' >Download File</a>"
                    $("#FileAttach").append(html);
                    $("#IsUploaded").val(true);
                }
                else {
                    $("#IsUploaded").val(false);
                }

                if (data.aff.trades_list != null) {
                    if (data.aff.trades_list.length > 0) {
                        $("#Table-Trade-update tbody").empty()
                        // $("#Table-Trade-update").append("<tbody></tbody>");

                        $.each(data.aff.trades_list, function () {

                            var _select = $("<select/>");
                            _select.addClass("form-control add-ddl trade-multi-select");
                            _select.append($("<option/>").val("").text("Select"));
                            _select.change(function () {

                                GetTradeCode(this);
                            });
                            //var units = $("<select/>");
                            //units.addClass("form-control multi-units");

                            if (data.trades_list.length > 0) {
                                $.each(data.trades_list, function (index, values) {
                                    _select.append($("<option/>").val(values.Value).text(values.Text));
                                });
                            }
                            //for (var i = 0; i < 10; i++) {
                            //    units.append($("<option/>").val(i + 1).text(i + 1));
                            //}

                            if (this.sessionKey > 0) {
                                _btn_pop = $("<button/>");
                                _btn_pop.addClass("btn btn-primary btn-add-shift");
                                _btn_pop.html("Update Shifts");
                            }
                            else {
                                _btn_pop = $("<button/>");
                                _btn_pop.addClass("btn btn-primary btn-add-shift");
                                _btn_pop.html("Add Shifts");
                            }

                            _btn_pop.click(function () {
                                GetTradeSession(this);
                            });

                            if (this.is_published) {
                                _btn_pop.attr("disabled", true);
                            }
                            else {
                                if (!this.en_edit) {
                                    _btn_pop.attr("disabled", true);
                                }
                            }


                            var _tr = $("<tr/>");
                            var _td = $("<td/>"); // first td

                            if (this.is_published) {
                                _td.append(_select.val(this.trade_id).attr("disabled", true));
                            }
                            else {
                                if (this.en_edit) {
                                    _td.append(_select.val(this.trade_id));
                                }
                                else {
                                    _td.append(_select.val(this.trade_id).attr("disabled", true));
                                }
                            }

                            _tr.append(_td);

                            _td = $("<td/>"); //second td
                            //if (this.en_edit) {
                            //    _td.append(units.val(this.units));
                            //}
                            //else {
                            //    _td.append(units.val(this.units).attr("disabled", true));
                            //}
                            _td.append("<label class='form-control trade-code' >" + this.trade_code + "</label>")
                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append("<label class='form-control text-multi-units' >" + this.units + "</label>")
                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append(_btn_pop);

                            _tr.append(_td);

                            _td = $("<td/>");
                            _td.append("<button class='btn btn-link'  onclick='ViewShifts(this);'>View Shifts</button>");

                            _tr.append(_td);
                            _td = $("<td/>"); //fourth td
                            _btn = "";
                            if (this.is_published) {
                                _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove' disabled='disabled' >X</button>");
                            }
                            else {
                                if (this.en_edit) {
                                    _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove'>X</button>");
                                    _btn.click(function () {

                                        var lenght = $('#Table-Trade-update tbody tr').length;
                                        if (lenght > 1) {
                                            $(this).closest("tr").remove();
                                            var total = 0;
                                            $(".text-multi-units").each(function () {
                                                total += parseFloat($(this).text())

                                            });
                                            $("#Units").text(total);
                                        }
                                        else {
                                            bootbox.alert("Atleast one row required")
                                        }
                                    });
                                }
                                else {
                                    _btn = $("<input type='hidden' value='" + this.trade_iti_id + "' class='update-trade_iti_id' /><input type='hidden' value='" + this.sessionKey + "' class='update-sessionkey' /><button class='btn btn-danger update-trade-remove' disabled='disabled' >X</button>");
                                }
                            }



                            _td.append(_btn);
                            _tr.append(_td);

                            $("#Table-Trade-update tbody").append(_tr);
                        });

                        var isPub = true;
                        var isEdit = false;
                        $.each(data.aff.trades_list, function () {

                            if (!this.is_published) {
                                isPub = false;

                            }

                        });

                        $.each(data.aff.trades_list, function () {

                            if (this.en_edit) {
                                isEdit = true;

                            }
                        });

                        //$(".update-row-new").attr("disabled", (!isPub));
                        //$(".update-button").attr("disabled", (!isPub));
                        //
                        if (isPub) {
                            $(".update-row-new").attr("disabled", false);
                            $(".update-button").attr("disabled", false);
                        }
                        else {
                            if (isEdit) {
                                $(".update-row-new").attr("disabled", false);
                                $(".update-button").attr("disabled", false);
                            } else {
                                $(".update-row-new").attr("disabled", true);
                                $(".update-button").attr("disabled", true);
                            }

                        }
                    }
                }



            }

        }, error: function () {


            bootbox.alert("Error", "something went wrong");
        }
    });

}

function SaveUploadedInstitute() {

    var fileUpload = $('#AffilicationDoc').get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    // var fileData = new FormData();
    //var fileUpload = $('.update-trade-file');
    //$.each(fileUpload, function () {
    //    
    //    var file = $(this).get(0);
    //    var files = file.files;

    //    for (var i = 0; i < files.length; i++) {
    //        fileData.append(files[i].name, files[i]);
    //    }
    //});

    var IsValid = true;
    var name = $("#ITIName").val();
    if (name == "") {
        $("#ITIName-update-Required").show();
        IsValid = false;
    }
    else {
        $("#ITIName-update-Required").hide();

    }
    var type = $("#TypeOfITI").val();
    if (type == "" || type == "0" || type == null) {
        $("#TypeOfITI-update-Required").show()
        IsValid = false;
    }
    else {
        $("#TypeOfITI-update-Required").hide()

    }
    var loca = $("#LocationType").val();
    if (loca == "" || loca == "0" || loca == null) {
        $("#LocationType-update-Required").show();
        IsValid = false;
    }
    else {
        $("#LocationType-update-Required").hide();

    }
    var mis = $("#MISCode").val();
    if (mis == "" || mis == "0" || mis == null) {
        $("#MISCode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#MISCode-update-Required").hide();

    }
    //var css = $("#CssCode").val();
    //if (css == "" || css == "0" || css == null) {
    //	$("#CssCode-update-Required").show();
    //	IsValid = false;
    //}
    //else {
    //	$("#CssCode-update-Required").hide();

    //}

    var dist = $("#District").val();
    if (dist == "" || dist == "0" || dist == null) {
        $("#District-update-Required").show();
        IsValid = false;
    }
    else {
        $("#District-update-Required").hide();

    }
    var taluk = $("#Taluka").val();
    if (taluk == "" || taluk == "0" || taluk == null) {
        $("#Taluka-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Taluka-update-Required").hide();

    }
    var consti = $("#Constiteuncy").val();
    if (consti == "" || consti == "0" || consti == null) {
        $("#Constiteuncy-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Constiteuncy-update-Required").hide();

    }
    //var pancha = $("#Panchayat").val();
    //if (pancha == "" || pancha == "0" || pancha == null) {
    //	$("#Panchayat-update-Required").show();
    //	IsValid = false;
    //}
    //else {
    //	$("#Panchayat-update-Required").hide();

    //}
    //var village = $("#Village").val();
    //if (village == "" || village == "0" || village == null) {
    //	$("#Village-update-Required").show();
    //	IsValid = false;
    //}
    //else {
    //	$("#Village-update-Required").hide();

    //}
    var buildup = $("#BuildUpArea").val();
    if (buildup == "") {
        $("#BuildUpArea-update-Required").show();
        IsValid = false;
    }
    else {
        $("#BuildUpArea-update-Required").hide();

    }
    var address = $("#Address").val();
    if (address == "") {
        $("#Address-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Address-update-Required").hide();

    }
    var geo = $("#GeoLocation").val();
    if (geo == "") {
        $("#GeoLocation-update-Required").show();
        IsValid = false;
    }
    else {
        $("#GeoLocation-update-Required").hide();

    }
    var email = $("#Email").val();
    if (email == "") {
        $("#Email-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Email-update-Required").hide();

    }
    var pincode = $("#Pincode").val();
    if (pincode == "") {
        $("#Pincode-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Pincode-update-Required").hide();
    }
    var phone = $("#PhoneNumber").val();
    if (phone == "") {
        $("#PhoneNumber-update-Required").show();
        IsValid = false;
    }
    else {
        $("#PhoneNumber-update-Required").hide();

    }
    var affidate = "";
    var date = "";
    if ($("#AffiliationDate").val() != "" && $("#AffiliationDate").val() != null) {
        var dts = $("#AffiliationDate").val().split("-");
        affidate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
        date = (dts[0] + "-" + dts[1] + "-" + dts[2]);
        $("#AffiliationDate-update-Required").hide();
    }
    else {
        IsValid = false;
        $("#AffiliationDate-update-Required").show();
    }
    var units = $("#Units").text();
    //if (units == "") {
    //	$("#Units-update-Required").show();
    //	IsValid = false;
    //}
    //else {
    //	$("#Units-update-Required").hide();
    //}
    //var shifts = $("#Shifts").val();
    //if (shifts == "") {
    //	$("#Shifts-update-Required").show();
    //	IsValid = false;
    //}
    //else {
    //	$("#Shifts-update-Required").hide();
    //   }

    var File = $("#AffilicationDoc").val();
    if (File == "") {
        var isUpload = $("#IsUploaded").val()
        if (isUpload == "false") {

            $("#file-update-Required").show();
            IsValid = false;
        }

    }
    else {
        $("#file-update-Required").hide();
    }
    var course = $("#Course-Update").val();
    if (course == "") {
        $("#Course-update-Required").show();
        IsValid = false;
    }
    else {
        $("#Course-update-Required").hide();

    }
    var orderno = $("#OrderNo-Update").val();
    if (orderno == "") {
        $("#OrderNo-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#OrderNo-Update-Required").hide();
    }

    var orderdate = "";
    if ($("#OrderNoDate-Update").val() != "") {
        var dts = $("#OrderNoDate-Update").val().split("-");
        orderdate = (dts[0] + "-" + dts[1] + "-" + dts[2]);

        $("#OrderNoDate-Update-Required").hide();
    }
    else {
        IsValid = false;
        $("#OrderNoDate-Update-Required").show();
    }

    var scheme = $("#Scheme-Update").val();
    if (scheme == "") {
        $("#Scheme-Update-Required").show();
        IsValid = false;
    }
    else {
        $("#Scheme-Update-Required").hide();
    }
    var website = $("#WebSite-Update").val();
    var collegeId = $("#CollegeId").val();


    $("#Table-Trade-update").each(function () {

        var $tr = $(this);
        var trade = $tr.find(".trade-multi-select").val()
        if (trade == "" || trade == null || trade == 0) {
            $tr.find(".update-trade-multi-select-required").show();
            IsValid = false;
        }
        else {
            $tr.find(".update-trade-multi-select-required").hide();
        }
    });
    // IsValid = true;
    if (IsValid) {
        $(".update-button").attr("disabled", true);

        fileData.append(
            "name_of_iti", name
        );
        fileData.append(
            "type_of_iti_id", type
        );
        fileData.append(
            "location_type_id", loca
        );
        fileData.append(
            "mis_code", mis
        );
        //fileData.append(
        //	"css_code_id", css
        //);
        fileData.append(
            "dist_id", dist
        );
        fileData.append(
            "taluk_id", taluk
        );
        fileData.append(
            "consti_id", consti
        );
        //fileData.append(
        //	"pancha_id", pancha
        //);
        //fileData.append(
        //	"village_id", village
        //);
        fileData.append(
            "build_up_area", buildup
        );
        fileData.append(
            "address", address
        );
        fileData.append(
            "geo_location", geo
        );
        fileData.append(
            "email", email
        );
        fileData.append(
            "affiliation_date", affidate
        );
        fileData.append(
            "phone_number", phone
        );
        fileData.append(
            "no_units", units
        );
        //fileData.append(
        //	"no_shifts", shifts
        //);

        fileData.append(
            "iti_college_id", collegeId
        );
        fileData.append(
            "date", date
        );
        fileData.append(
            "Pincode", pincode
        );
        fileData.append(
            "course_code", course
        );
        fileData.append(
            "Website", website
        );
        fileData.append(
            "AffiliationOrderNo", orderno
        );

        fileData.append(
            "Scheme", scheme
        );
        fileData.append(
            "order_no_date", orderdate
        );

        $("#Table-Trade-update tbody tr").each(function () {

            var $tr = $(this);
            var trade = $tr.find(".trade-multi-select").val();
            var unit = $tr.find(".text-multi-units").text();
            var key = $tr.find(".update-sessionkey").val();
            //var isupload = $tr.find(".update-trade-isupload").val();
            var trade_iti_id = $tr.find(".update-trade_iti_id").val();

            fileData.append(
                "list_trades", trade
            );
            fileData.append(
                "list_units", unit
            );
            //fileData.append(
            //    "is_uploads", isupload
            //);
            fileData.append(
                "list_it_trade_id", trade_iti_id
            );
            fileData.append(
                "list_keys", key
            );

        });


        $.ajax({
            type: 'POST',
            url: '/Affiliation/SaveUploadedAffiliationCollegeDetails',
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {

                if (result.flag == 1) {
                    $(".update-button").show();
                    $(".save-button").hide();

                    ClearFields();
                    GetAllUploadedAffiliationColleges();
                    bootbox.alert("Data Saved Successfully")
                    // bootbox.alert("Affiliation Institute Updated and sent to OS")



                }
                else {
                    bootbox.alert(result.status)
                }

                $(".update-button").attr("disabled", false);
            }
        });
    }

}

function ClearFields() {
    $(".update").val("");
    $(".update-dll").empty();
    $("#DataGridDiv").show();
    $("#UpdateDiv").hide();
    $(".update-trade-file").val("")

    GetAllAffiliationColleges(0, 0, 0);
}

function ValidateTextBox(e) {

    var code = ('charCode' in e) ? e.charCode : e.keyCode;
    if (!(code == 32) && // space
        !(code == 46) && // full stop.
        !(code == 44) && // coma,
        !(code > 47 && code < 58) && // numeric (0-9)
        !(code > 64 && code < 91) && // upper alpha (A-Z)
        !(code > 96 && code < 123)) { // lower alpha (a-z)
        bootbox.alert("Invalid input")
        e.preventDefault();
    }
}

function ValidateEmail(val) {

    if (/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(val.value)) {
        return (true)
    }
    bootbox.alert("You have entered an invalid email address!")
    val.value = "";
    return (false)
}

function phonenumber(inputtxt) {

    var phoneno = /^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/;
    if (inputtxt.value.match(phoneno)) {
        return true;
    }
    else {
        bootbox.alert("Invalid Phone Number");
        inputtxt.value = "";
        return false;
    }
}

function pincodevalidate(inputtxt) {
    var pattren = /^\d{6}$/;
    if (!pattren.test(inputtxt.value)) {
        bootbox.alert("Pin code should be 6 digits ");
        inputtxt.value = "";
        return false;
    }
}
function limitKeypressAddressField(e, value, maxLength) {
    var code = ('charCode' in e) ? e.charCode : e.keyCode;
    if (!(code == 32) && // space
        !(code == 46) && // full stop.
        !(code == 44) && // coma,
        !(code > 47 && code < 58) && // numeric (0-9)
        !(code > 64 && code < 91) && // upper alpha (A-Z)
        !(code > 96 && code < 123)) { // lower alpha (a-z)
        bootbox.alert("Invalid input")
        e.preventDefault();
    }
    if (value != undefined && value.toString().length >= maxLength) {
        event.preventDefault();
        bootbox.alert("Maximum Characters allowed is " + maxLength)
    }

}

function isUrlValid(userInput) {

    let isvalid = /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(userInput.value);
    if (!isvalid) {
        userInput.value = "";
        bootbox.alert("Invalid URL");
    }
}

function SaveCollege() {
    

    var fileUpload = $('#AffilicationDoc-Add').get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    
    var Ittype = $("#Aided").val();
    var flow_id = $("#dd-flowinst").val();
    if (flow_id == null || flow_id=="")
    {
        return bootbox.alert("pls select send to ");
    }
    var remarks = $("#Remarks1").val();
    if (remarks == null || remarks == "")
    {
        return bootbox.alert("Pls select Remarks");
    }
    var IsValid = true;
    var name = $("#ITIName-Add").val();
    if (name == "") {
        return bootbox.alert("Pls enter Institute name");
        $("#ITIName-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#ITIName-Add-Required").hide();

    }
    var type = $("#TypeOfITI-Add").val();
    if (type == "" || type == "0") {
        return bootbox.alert("Pls Select type of ITI");
        $("#TypeOfITI-Add-Required").show()
        IsValid = false;
    }
    else {
        $("#TypeOfITI-Add-Required").hide()

    }
    var loca = $("#LocationType-Add").val();
    if (loca == "" || loca == "0") {
        return bootbox.alert("Pls enter Location Type");
        $("#LocationType-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#LocationType-Add-Required").hide();

    }
    var mis = $("#MISCode-Add").val();
    if (mis == "" || mis == "0") {
        return bootbox.alert("Pls enter Miscode");
        $("#MISCode-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#MISCode-Add-Required").hide();

    }
    var div = $("#Division-Add").val();
    if (div == "" || div == "0") {
        return bootbox.alert("Pls Select Division");
        $("#Division-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Division-Add-Required").hide();

    }
    var dist = $("#District-Add").val();
    if (dist == "" || dist == "0") {
        return bootbox.alert("Pls Select Ditrict");
        $("#District-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#District-Add-Required").hide();

    }
    var taluk = $("#Taluka-Add").val();
    if (taluk == "" || taluk == "0") {
        return bootbox.alert("Pls Select Taluk");
        $("#Taluka-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Taluka-Add-Required").hide();

    }
    var consti = $("#Constiteuncy-Add").val();
    if (consti == "" || consti == "0") {
        return bootbox.alert("Pls Select Constituency");
        $("#Constiteuncy-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Constiteuncy-Add-Required").hide();

    }

    var buildup = $("#BuildUpArea-Add").val();
    if (buildup == "") {
        return bootbox.alert("Pls Enter buildup area");
        $("#BuildUpArea-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#BuildUpArea-Add-Required").hide();

    }
    var address = $("#Address-Add").val();
    if (address == "") {
        return bootbox.alert("Pls Enter address");
        $("#Address-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Address-Add-Required").hide();

    }
    var geo = $("#GeoLocation-Add").val();
    if (geo == "") {
        return bootbox.alert("Pls enter Geolocation");
        $("#GeoLocation-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#GeoLocation-Add-Required").hide();

    }
    var email = $("#Email-Add").val();
    if (email == "") {
        return bootbox.alert("Pls enter Email");
        $("#Email-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Email-Add-Required").hide();

    }
    var pincode = $("#Pincode-Add").val();
    if (pincode == "") {
        return bootbox.alert("Pls enter Pincode");
        $("#Pincode-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Pincode-Add-Required").hide();
    }
    var phone = $("#PhoneNumber-Add").val();
    if (phone == "") {
        return bootbox.alert("Pls enter Phone number");
        $("#PhoneNumber-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#PhoneNumber-Add-Required").hide();

    }

    var file = $("#AffilicationDoc-Add").val();
    if (file == "" || file == null) {
        IsValid = false;
        return bootbox.alert("Pls Select Document");
        $("#file-add-Required").show();

    }
    else {
        $("#file-add-Required").hide();
    }


    $("#Table-Trade tbody tr").each(function () {

        var $tr = $(this);
        var trade = $tr.find(".trade-multi-select").val()
        var key = $tr.find(".update-sessionkey").val();
        // var file = $tr.find(".add-trade-file").val()
        if (trade == "" || trade == null || trade == 0) {
            IsValid = false;
            return bootbox.alert("Pls Select trade");
            $tr.find(".trade-multi-select-required").show();
          
        }
        else {
            $tr.find(".trade-multi-select-required").hide();
        }
        if (key == 0) {
            IsValid = false;
            return bootbox.alert("Pls add shift");
            $tr.find(".add_shift_required").show();
           
        }
        else {
            $tr.find(".add_shift_required").hide();
        }


    });
    //var today = new Date();
    //var dd = String(today.getDate()).padStart(2, '0');
    //var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    //var yyyy = today.getFullYear();

    //today = yyyy + '-' + mm + '-' + dd;
   
    var affidate = "";
    var date = "";
    
    if ($("#AffiliationDate-Add").val() != "") {
        var dts = $("#AffiliationDate-Add").val().split("/");
        affidate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
        if (affidate == "Invalid Date") {
            return bootbox.alert("Please Select Valid AffiliationDate");
        }

        date = (dts[2] + "/" + dts[1] + "/" + dts[0]);
        $("#AffiliationDate-Add-Required").hide();
    }
    else {
        IsValid = false;
        $("#AffiliationDate-Add-Required").show();
    }
    //if (date != null) {
    //    if (date > today) {
    //        return bootbox.alert("Date of Establishment is greater than current date, pls select date less than or equal to current date  ")
    //    }
    //}

    var units = $("#Units-Add").text();


    
    var course = $("#Course-Add").val();
    if (course == "") {
        return bootbox.alert("Pls Select Course");
        $("#Course-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Course-Add-Required").hide();
    }
    var orderno = $("#OrderNo-Add").val();
    if (orderno == "") {
        return bootbox.alert("Pls enter Order Number");
        $("#OrderNo-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#OrderNo-Add-Required").hide();
    }


    //var orderdate = "";
    //var date = "";
    //if ($("#OrderNoDate-Add").val() != "") {
    //    var dts = $("#OrderNoDate-Add").val().split("-");
    //    orderdate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
    //    if (orderdate == "Invalid Date") {
    //        return bootbox.alert("Please Select Valid OrderNoDate");
    //    }

    //    date = (dts[2] + "-" + dts[1] + "-" + dts[0]);
    //    $("#OrderNoDate-Add-Required").hide();
    //}
    //else {
    //    IsValid = false;
    //    $("#OrderNoDate-Add-Required").show();
    //}
    
    var orderdate = "";
    if ($("#OrderNoDate-Add").val() != "") {
        var dts = $("#OrderNoDate-Add").val().split("/");
        orderdate = new Date(dts[1] + "/" + dts[0] + "/" + dts[2]);
        if (orderdate == "Invalid Date") {
            return bootbox.alert("Please Select Valid Affiliation Date");
        }
        orderdate = (dts[2] + "/" + dts[1] + "/" + dts[0]);

        // orderdate = (dts[1] + "-" + dts[0] + "-" + dts[2]);
        $("#OrderNoDate-Add-Required").hide();
    }
    else {
        IsValid = false;
        return bootbox.alert("Pls enter Affiliation Order Number date");
        $("#OrderNoDate-Add-Required").show();
    }
    //if (orderdate != null) {
    //    if (orderdate > today) {
    //        return bootbox.alert("Affiliation Date date is greater than current date, pls select date less than or equal to current date  ")
    //    }
    //}

    var scheme = $("#Scheme-Add").val();
    if (scheme == "") {
        return bootbox.alert("Pls Select Scheme");
        $("#Scheme-Add-Required").show();
        IsValid = false;
    }
    else {
        $("#Scheme-Add-Required").hide();
    }
    var website = $("#WebSite-Add").val();
    // IsValid = true;
    if (IsValid) {
        fileData.append(
            "name_of_iti", name
        );
        fileData.append(
            "type_of_iti_id", type
        );
        fileData.append(
            "location_type_id", loca
        );
        fileData.append(
            "mis_code", mis
        );
        fileData.append(
            "division_id", div
        );
        fileData.append(
            "dist_id", dist
        );
        fileData.append(
            "taluk_id", taluk
        );
        fileData.append(
            "consti_id", consti
        );
        //fileData.append(
        //	"pancha_id", pancha
        //);
        //fileData.append(
        //	"village_id", village
        //);
        fileData.append(
            "build_up_area", buildup
        );
        fileData.append(
            "address", address
        );
        fileData.append(
            "geo_location", geo
        );
        fileData.append(
            "email", email
        );
        fileData.append(
            "affiliation_date", affidate
        );
        fileData.append(
            "phone_number", phone
        );
        fileData.append(
            "no_units", units
        );
        fileData.append(
            "flow_id", flow_id
        );
        fileData.append(
            "remarks", remarks
        );
        fileData.append(
            "date", date
        );
        fileData.append(
            "Pincode", pincode
        );
        fileData.append(
            "course_code", course
        );
        fileData.append(
            "Website", website
        );
        fileData.append(
            "AffiliationOrderNo", orderno
        );

        fileData.append(
            "AidedUnaidedTrade", Ittype
        );


        fileData.append(
            "Scheme", scheme
        );
        fileData.append(
            "order_no_date", orderdate
        );
        //var array = [];
        $("#Table-Trade tbody tr").each(function () {
            
            var $tr = $(this);
            var trade = $tr.find(".trade-multi-select").val();
            var unit = $tr.find(".text-multi-units").text();
            var key = $tr.find(".update-sessionkey").val();
            var ititype = $tr.find(".Aided").val();
            // var isupload = $tr.find(".add-trade-isupload").val();

            fileData.append(
                "list_trades", trade
            );
            fileData.append(
                "list_units", unit
            );
            fileData.append(
                "list_type", ititype
            );
            //fileData.append(
            //    "is_uploads", isupload
            //);
            fileData.append(
                "list_keys", key
            );
        });

        
        if (flow_id == 5) {
            var desig = "Deputy Director";
        }
        else if (flow_id == 3) {
            desig = "Assistant Director";
        }
        else if (flow_id == 8) {
            desig = "Case Worker";
        }
        else if (flow_id == 7) {
            desig = "Office Superintendent";
        }

        // bootbox.confirm(" " + name + 'details will be sent to Office Superintendent for review. Please Confirm ?', (confirma) => {
        bootbox.confirm('<br><br>Are you sure you want to send the newly added ' + name + ' Institute details to ' + desig +' for review ?', (confirma) => {
            if (confirma) {

                $.ajax({
                    type: 'POST',
                    url: '/Affiliation/SaveCollegeAffilationDetailses',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {

                        if (result.flag == 1) {

                            ClearAddFields();
                            bootbox.alert("<br><br>" + name + "  Institute details is sent to "  + desig +" for review Successfully")
                            $("#remarks").empty(); 
                            $("#flow_id").empty();
                            //bootbox.alert("New Affiliation ITI Institute Details Added and Sent to Office Superintendent");
                            ///GetAllAffiliationColleges(0, 0, 0);
                            fnRefreshScreenData();
                        }
                        else {
                            bootbox.alert(result.status)
                        }
                    }
                });
            }
        });
    }

}

function ClearAddFields() {
    $(".add").val("");
    $(".add-ddl").val("");
    $(".trade-code").text("");
    $(".trade-sector").text("");
    $(".trade-type").text("");
    $(".trade-duration").text("");
    $(".trade-size").text("");
    $(".btn-add-shift").text("Add Units & Shifts");
    $(".add-dll-clear").empty();
    $(".add-dll-clear").append('<option value="">Select</option>');
    $('#Table-Trade > tbody:last > tr:not(:first)').remove();
    $(".multi-units").val(1);
    $('#AffilicationDoc-Add').val("");
    GetAllAffiliationColleges(0, 0, 0);
}

function GetAllAffiliationColleges(courseId, divisionId, districtId, tradeId) {
    $.ajax({
        type: 'Get',
        url: '/Affiliation/FilterCollegeDetails',
        data: { courseId: courseId, divisionId: divisionId, districtId: districtId, tradeId: tradeId },
        success: function (data) {

            var index = 1;
            if (data.length > 0) {

                $('#tblGridUpdateAffiliation').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    //"ordering": false,
                    columns: [
                        { 'data': 'name_of_iti', 'title': 'Sl. No.', 'className': 'text-center index' },
                        { 'data': 'course_name', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': 'text-left' },
                        { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': '' },
                        { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                        //{ 'data': 'NoofTrades', 'title': 'Total No Of Trades', 'className': 'text-left' },
                        { 'data': 'no_units', 'title': 'Units', 'className': 'text-center' },
                        { 'data': 'division', 'title': 'Division', 'className': 'text-left' },
                        { 'data': 'district', 'title': 'District', 'className': 'text-center' },
                        { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                        { 'data': 'status', 'title': 'Status - Currently with', 'className': 'text-left' },
                        {
                            'data': 'FileUploadPath',
                            render: function (data, type, row) {
                                if (row.FileUploadPath != null && row.FileUploadPath != "") {
                                    if (row.isSelect) {
                                        return "<a class='btn btn-link' href=/Affiliation/DownloadAffiliationDoc?CollegeId=" + row.iti_college_id + "><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a><input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"
                                    }
                                    else {
                                        return "<p>File not found</p>"
                                    }
                                }
                                else {
                                    return "<p>File not uploaded</p>"
                                }

                            }
                        },
                        {
                            'date': 'iti_college_id',
                            render: function (data, type, row) {
                                if (row.isUploaded) {
                                    return "<input type='button' class='btn btn-primary' value='View' onclick='GetUploadedUpdateTrade(" + row.trade_iti_id + ")'/> <input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"
                                }
                                else {
                                    if (row.color_flag == 0) {
                                        return "<input type='button' class='btn btn-primary' value='View' onclick='GetUpdateTrade(" + row.trade_iti_id + ")' disabled/> <input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"
                                    }
                                    else {
                                        return "<input type='button' class='btn btn-primary' value='View' onclick='GetUpdateTrade(" + row.trade_iti_id + ")'/> <input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"
                                    }

                                }

                            }
                        }

                    ],
                    "columnDefs": [{
                        "render": function (data, type, full, meta) {

                            return index++;
                        },
                        "targets": 0
                    }],
                    "createdRow": function (row, data, dataIndex) {
                        if (data["color_flag"] == "1") {
                            $(row).css("background-color", "#e6ffee");
                        }
                        else if (data["color_flag"] == "0") {
                            $(row).css("background-color", "#f2f2f2");
                        }
                    }
                });

                var index = 1;
                $('#tblGridUpdateAffiliation tbody').find("tr").each(function (i) {
                    $('#tblGridUpdateAffiliation tbody').find("tr").eq(i).find(".index").text(index);
                    var nextId = $('#tblGridUpdateAffiliation tbody').find("tr").eq(i + 1).find(".collegeId").val();
                    var thisId = $('#tblGridUpdateAffiliation tbody').find("tr").eq(i).find(".collegeId").val();

                    if (thisId != nextId) {
                        index++;
                    }
                });

            }
            else {
                bootbox.alert("No data found!")
                $('#tblGridUpdateAffiliation').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllUploadedAffiliationColleges() {
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetUploadedAffiliationInstitutes',
        success: function (data) {

            var index = 1;
            if (data.length > 0) {

                var t = $('#tblGridUploadAffiliation').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'name_of_iti', 'title': 'Sl. No.', 'className': 'index' },
                        { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': 'text-left' },
                        { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': '' },
                        { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                        { 'data': 'no_units', 'title': 'Units', 'className': 'text-center' },
                        { 'data': 'NoofTrades', 'title': 'Total No. Of Trades', 'className': 'text-left' },
                        { 'data': 'district', 'title': 'District', 'className': 'text-left' },
                        { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                        { 'data': 'date', 'title': 'Date', 'className': 'text-left' },
                    ],
                    "columnDefs": [{
                        "render": function (data, type, full, meta) {

                            return index++;
                        },
                        "targets": 0
                    }],
                });
                t.on('order.dt search.dt', function () {
                    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                        cell.innerHTML = i + 1;
                    });
                }).draw();

                var index1 = 1;

                $('#tblGridUploadAffiliation tbody').find("tr").each(function (i) {
                    

                    var nextId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").text();
                    //var thisId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").val();
                    if (nextId != "") {
                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text(index1);
                        index1++;
                    }
                    else {
                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text("");
                    }

                    //if (thisId != nextId) {
                    //    index++;
                    //}
                });

                var index1 = 1;
                $('#tblGridUploadAffiliation tbody').find("tr").each(function (i) {

                    var nextId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").val();
                    //var thisId = $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".mis_code").val();
                    if (nextId != "") {
                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text(index1);
                        index1++;
                    }
                    else {
                        $('#tblGridUploadAffiliation tbody').find("tr").eq(i).find(".sorting_1").text("");
                    }

                    //if (thisId != nextId) {
                    //    index++;
                    //}
                });
            }


            else {
                // bootbox.alert("No data found!")
                $('#tblGridUploadAffiliation').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }




    });
}

function GetAllApprovedColleges(courseId, divisionId, districtId, tradeId) {

    $.ajax({
        type: 'Get',
        url: '/Affiliation/FilterApprovedCollegeDetails',
        data: { courseId: courseId, divisionId: divisionId, districtId: districtId, tradeId: tradeId },
        success: function (data) {

            if (data.length > 0) {

                var t = $('#tblGridPublishAffiliation').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'name_of_iti', 'title': 'Sl. No.', 'className': '' },
                        { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': 'text-center' },
                        { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': '' },

                        { 'data': 'status', 'title': 'Status -Currently with', 'className': 'text-center' },
                        {
                            'data': 'iti_college_id', 'title': 'Action', 'className': 'text-center',
                            render: function (data, type, row) {

                                return "<input type='button' class='btn btn-primary' value='View' onclick='ViewCollege(" + row.iti_college_id + ")'/> <input type='hidden' class='approved-collegeId' value=" + row.iti_college_id + " />"
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
                bootbox.alert("No data found!")
                $('#tblGridPublishAffiliation').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetAllPublishedColleges(courseId, divisionId, districtId, tradeId) {
    
    $.ajax({
        type: 'Get',
        url: '/Affiliation/FilterPublishedCollegeDetails',
        data: { courseId: courseId, divisionId: divisionId, districtId: districtId, tradeId: tradeId },
        success: function (data) {
            var index = 1;
            if (data.length > 0) {

                var t = $('#tblGridAffiliatedColleges').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'name_of_iti', 'title': 'Sl. No.', 'className': '' },
                        { 'data': 'course_name', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': '' },
                        { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': 'text-left' },
                        { 'data': 'address', 'title': 'Address of ITI', 'className': 'text-left' },
                        { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                        { 'data': 'division', 'title': 'Division', 'className': 'text-center' },
                        { 'data': 'district', 'title': 'District', 'className': 'text-left' },
                        { 'data': 'state', 'title': 'State', 'className': 'text-center' },
                        { 'data': 'Pincode', 'title': 'Pincode', 'className': 'text-left' },
                        //{ 'data': 'panchayat', 'title': 'Panchayat', 'className': 'text-left' },
                        //{ 'data': 'village', 'title': 'Village', 'className': 'text-left' },
                        { 'data': 'constituency', 'title': 'Constituency', 'className': 'text-left' },
                       
                        { 'data': 'date', 'title': 'Date of Establishment', 'Geo Location': 'text-left' },
                        { 'data': 'geo_location', 'title': 'Geo Location', 'className': 'text-left' },
                        { 'data': 'type_of_iti', 'title': 'Type of ITI( Govt/Aided/Pvt.)', 'className': 'text-left' },
                        { 'data': 'location_type', 'title': 'Location Type', 'className': 'text-left' },
                        { 'data': 'sector', 'title': 'Sector Name    ', 'className': 'text-center' },
                        { 'data': 'trade', 'title': 'Name of Trades', 'className': 'text-center' },
                        { 'data': 'trade_code', 'title': 'Trade Code', 'className': 'text-center' },

                        { 'data': 'trade_type', 'title': 'Trade Type', 'Geo Location': 'text-left' },
                        { 'data': 'tshift', 'title': 'No. of Shifts', 'className': 'text-left' },
                        { 'data': 'units', 'title': 'No. of Units', 'className': 'text-left' },
                       
                        { 'data': 'duration', 'title': 'Trade Duration', 'className': 'text-center' },
                        { 'data': 'batch_size', 'title': 'Batch Size', 'className': 'text-center' },

                        { 'data': 'AidedUnaidedTrade', 'title': 'Aided/Unaided', 'className': 'text-center' },

                        { 'data': 'AffiliationOrderNo', 'title': 'Affiliation File/Order Number', 'className': 'text-center' },

                        { 'data': 'order_no_date', 'title': 'Affiliation Date', 'className': 'text-center' },
                        { 'data': 'phone_number', 'title': 'Principal Phone Number', 'className': 'text-center' },
                        { 'data': 'Website', 'title': 'Web Site', 'className': 'text-center' },
                        { 'data': 'email', 'title': 'Email Id', 'className': 'text-center' },


                        {
                            'data': 'FileUploadPath',
                            render: function (data, type, row) {

                                if (row.FileUploadPath != null && row.FileUploadPath != "") {
                                    if (row.isSelect) {
                                        return "<a class='btn btn-link' href=/Affiliation/DownloadAffiliationDoc?CollegeId=" + row.iti_college_id + "><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a><input type='hidden' class='collegeId' value=" + row.iti_college_id + " />"
                                    }
                                    else {
                                        return "<p>File not found!</p>"
                                    }
                                }
                                else {
                                    return "<p>File not uploaded!</p>"
                                }

                            }
                        },
                        { 'data': 'scheme_name', 'title': 'Schemes', 'className': 'text-center' }

                    ],
                    "columnDefs": [{
                        "render": function (data, type, full, meta) {

                            return index++;
                        },
                        "targets": 0
                    }],
                });

            }
            else {
                bootbox.alert("No data found!")
                $('#tblGridAffiliatedColleges').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function fileExists(url) {
    $.get(url)
        .done(function () {
            // exists code 
            return true;
        }).fail(function () {
            // not exists code
            return false;
        })
}
function ViewCollege(collegeId) {

    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetCollegeDetailsNew',
        data: { collegeId: collegeId },
        success: function (data) {

            if (data != null) {
                $("#DataGridDiv_Publish").hide();
                $("#ViewDiv").show();

                $("#ITIName_V").text(data.name_of_iti);
                $("#TypeOfITI_V").text(data.type_of_iti);
                $("#LocationType_V").text(data.location_type);
                $("#MISCode_V").text(data.mis_code);
                $("#CssCode_V").text(data.css_code);
                // $("#ListOfTrades_V").text(data.list_trades);
                $("#District_V").text(data.district);
                $("#Taluka_V").text(data.taluka);
                $("#Constiteuncy_V").text(data.constituency);
                $("#Panchayat_V").text(data.panchayat);
                $("#Village_V").text(data.village);
                $("#BuildUpArea_V").text(data.build_up_area);
                $("#Address_V").text(data.address);
                $("#GeoLocation_V").text(data.geo_location);
                $("#Email_V").text(data.email);
                //$("#Pincode").text(data.geo_location);
                $("#PhoneNumber_V").text(data.phone_number);
                $("#AffiliationDate_V").text(data.date);
                $("#Units_V").text(data.no_units);
                $("#Shifts_V").text(data.no_shifts);
                $("#ViewFile").empty();
                if (data.FileUploadPath != null && data.FileUploadPath != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId + "' >Download File</a>"
                    $("#ViewFile").append(html);
                }
                if (data.trades != null) {

                    if (data.trades.length > 0) {
                        $.each(data.trades, function (indx, val) {

                            var html = "<li class='list-group-item'>" + val.Text + "</li>"
                            $("#ListOfTrades_V").append(html);
                        });
                    }
                }
            }
            else {
                bootbox.alert("Error occured try again")
            }
        }
    });
}

function HidePublishView() {
    $("#DataGridDiv_Publish").show();
    $("#ViewDiv").hide();

    $("#ITIName_V").text("");
    $("#TypeOfITI_V").text("");
    $("#LocationType_V").text("");
    $("#MISCode_V").text("");
    $("#CssCode_V").text("");
    // $("#ListOfTrades_V").text(data.list_trades);
    $("#District_V").text("");
    $("#Taluka_V").text("");
    $("#Constiteuncy_V").text("");
    $("#Panchayat_V").text("");
    $("#Village_V").text("");
    $("#BuildUpArea_V").text("");
    $("#Address_V").text("");
    $("#GeoLocation_V").text("");
    $("#Email_V").text("");
    //$("#Pincode").text(data.geo_location);
    $("#PhoneNumber_V").text("");
    $("#AffiliationDate_V").text("");
    $("#Units_V").text("");
    $("#Shifts_V").text("");
    $("#ListOfTrades_V").empty();
}

function PublishColleges() {

    var Colleges = $(".approved-collegeId");
    var fileData = new FormData();
    if (Colleges.length > 0) {

        $.each(Colleges, function () {

            fileData.append(
                "colleges", $(this).val()
            );
        });
        $.ajax({
            type: 'POST',
            url: '/Affiliation/PublishAffiliatedColleges',
            data: fileData,
            contentType: false,
            processData: false,
            success: function (data) {
                $('#tblGridPublishAffiliation').DataTable().clear().draw();
                bootbox.alert("Published Successful")

                GetAllPublishedColleges(0, 0, 0, 0);
            }
        });

    }
    else {
        bootbox.alert("No Colleges found for publish")
    }
}

$(function () {
    
    $(".date").datepicker({
        dateFormat: 'dd/mm/yy',
        yearRange: "-60:+0",
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
    });
});
  //$('#AdmisionTime').datepicker({
  //      dateFormat: 'dd-mm-yy',
  //      minDate: '-30d',
  //      changeMonth: true,
  //      changeYear: true,
        
  //      dateFormat: 'dd-mm-yy'
  //  });
//$('#').datepicker({
//    dateFormat: 'dd-mm-yy',
//    minDate: 0.,
//    changeMonth: true,
//    changeYear: true
//});

function ViewTrade(tradeId) {
    

    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradeDetails',
        data: { trade_id: tradeId },
       
        success: function (data) {

            if (data.flag == 1) {

                $("#Affiliation_view").show();
                $("#UpdateDiv").show();
                $("#Affiliation_div").hide();
                $("#UpdateDivEditLogin").show();

                $(".text-name").text(data.name_of_iti);
                $(".text-type").text(data.type_of_iti);
                $(".text-location").text(data.location_type);
                $(".text-mis").text(data.mis_code);
                // $(".text-css").text(data.css_code);
                $(".text-trade").text(data.trade);
                $(".text-district").text(data.district);
                $(".text-taluk").text(data.taluka);
                $(".text-constituency").text(data.constituency);
                $(".text-Panchayat").text(data.panchayat);
                $(".text-village").text(data.village);
                $(".text-build").text(data.build_up_area);
                $(".text-address").text(data.address);
                $(".text-geo").text(data.geo_location);
                $(".text-email").text(data.email);
                $(".text-pincode").text(data.Pincode);
                $(".text-number").text(data.phone_number);
                $(".text-date").text(data.date);
                $(".text-unit").text(data.no_units);
                // $(".text-shift").text(data.no_shifts);
                $(".text-course").text(data.course_name);
                $(".text-order-no").text(data.AffiliationOrderNo);
                $(".text-order-no-date").text(data.order_no_date);
                $(".text-scheme").text(data.scheme_name);
                $(".text-web").append("<a href='" + data.Website + "'  target='_blank' class='btn-link' >" + data.Website + "</a>");
                $(".text-division").text(data.division);
                $(".text-trade-code").text(data.trade_code);
                $(".text-batch").text(data.batch_size);
                $(".text-trade-type").text(data.trade_type);
                $(".text-duration").text(data.duration);
                $(".text-trade-sector").text(data.sector);

                $("#Trade_ITI_Id").val(data.trade_iti_id);
                $("#viewAttachment").empty();
                $("#TradeFileAttach").empty();
                if (data.FileUploadPath != "" && data.FileUploadPath != null) {
                    if (data.isSelect) {
                        $("#viewAttachment").append("<a title='Download Pdf' class='btn btn-link' href=/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.iti_college_id + "><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>")
                    }
                    else {
                        $("#viewAttachment").append("File not found!");
                    }
                }
                if (data.UploadTradeAffiliationDoc != null && data.UploadTradeAffiliationDoc != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationTradeDoc?CollegeId=" + data.iti_college_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                    $("#TradeFileAttach").append(html);
                    // $("#IsUploaded").val(true);
                }
                else {
                    $("#tradedoc").hide();
                    $("#tradedoc1").hide();
                }
                
                if (!data.en_edit) {
                    $(".myoption").attr("disabled", true)
                }
                else {
                    $(".myoption").attr("disabled", false)
                }
                var _table = $("#tblViewShift tbody");
                _table.empty();
                $.each(data.shifts, function (i) {

                    var _tr = $("<tr/>");
                    _tr.append("<td>" + (i + 1) + "</td>");
                    _tr.append("<td>" + this.Units + "</td");
                    _tr.append("<td>" + this.Shift + "</td>");
                    _tr.append("<td>" + this.IsPPP + "</td>");
                    _tr.append("<td>" + this.Dual_System + "</td>");
                    if (this.IsActive) {
                        _tr.append("<td>Active <input type='hidden' class='active_status' value='true' /><input type='hidden' class='shiftId' value='" + this.ITI_Trade_Shift_Id + "' /></td>");

                    }
                    else {
                        if (this.Status == 5) {
                            _tr.append("<td>Pending for Approval <input type='hidden' class='active_status' value='false' /><input type='hidden' class='shiftId' value='" + this.ITI_Trade_Shift_Id + "' /></td>");
                        }
                        else {
                            _tr.append("<td>DeActive <input type='hidden' class='active_status' value='false' /><input type='hidden' class='shiftId' value='" + this.ITI_Trade_Shift_Id + "' /></td>");
                        }

                    }
                    _table.append(_tr);
                });

                $("#tblAffiliationDoc").hide();
                
                if (data.AffiliationDocs.length > 0) {
                    $("#tblAffiliationDoc").show();
                    var _table = $("#tblAffiliationDoc tbody");
                    _table.empty();
                    $.each(data.AffiliationDocs, function (i) {

                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.AffiliationOrder_Number + "</td>");
                        _tr.append("<td>" + this.Affiliation_date.split(' ')[0] + "</td>");
                        _tr.append("<td >" + this.FileName + "</td>");
                        _tr.append("<td ><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.iti_college_id + "&Trade_Id=" + data.trade_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>" + "" + "</td>");
                        //_tr.append("<td >" + this.Status + "</td>");
                        _table.append(_tr);
                    });
                }


                GetTradeHistory(data.trade_iti_id);

            } else {
                bootbox.alert("Error occured while loading data")
            }


        }
    });
}


function GetTradeHistory(tradeId) {

    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradeHistory',
        data: { trade_id: tradeId },
        success: function (data) {

            if (data.flag == 1) {

                if (data.list.length > 0) {
                    $.each(data.list, function (i) {
                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.date + "</td");
                        _tr.append("<td>" + this.sent_by + "</td>");
                        _tr.append("<td>" + this.created_by + "</td>");
                        _tr.append("<td>" + this.Status + "</td>");
                        _tr.append("<td>" + this.Remarks + "</td>");
                        //if (this.FileUploadPath != null && this.FileUploadPath != "") {
                        //    _tr.append("<td><a class='btn btn-link' href='/Affiliation/DownloadAffiliationTradeDoc?TradeId=" + this.Trade_ITI_id + "' >Download File</a></td>");
                        //}
                        //else {
                        //    _tr.append("<td>File not uploaded</td>");
                        //}

                        $("#tblHistory tbody").append(_tr);
                    });
                }
            } else {
                bootbox.alert("Error occured while loading history")
            }
        }
    });
}

function GetStatus() {
    
    $("#dd-status").append('<option value="">Select</option>');

    $.ajax({
        url: "/Affiliation/GetAllStatus",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.flag == 1) {
                $.each(data.list, function () {
                    $("#dd-status").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function GetUsers() {

    $("#dd-flow").append('<option value="">Select</option>');
    $("#dd-flowtr").append('<option value="">Select</option>');
    $("#dd-flowinst").append('<option value="">Select</option>');
    $.ajax({
        url: "/Affiliation/GetAllUsers",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#dd-flow").append($("<option/>").val(this.Value).text(this.Text));
                    $("#dd-flowtr").append($("<option/>").val(this.Value).text(this.Text));
                    $("#dd-flowinst").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}

function ClearViewAffiliation() {

   
    $("#UpdateDivEditLogin").hide();
    $("#Affiliation_div").show();
    $(".clear").text("");
    $("#dd-status").val("");
    $("#dd-flow").val("");
    $("#tblHistory tbody").empty();
    $("#Remarks").val("");
    $("#Affiliation_view").hide();
    $("#tblGridAffiliationCollege").show();
}

// lukhman code
function GetAllColleges() {
    
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetAffiliatedCollegesByRole',
        success: function (data) {

            var index = 1;
            if (data.length > 0) {

                var t = $('#tblGridAffiliationCollege').DataTable({
                    data: data,
                    "destroy": true,
                    "bSort": true,
                    columns: [
                        { 'data': 'name_of_iti', 'title': 'Sl. No.', 'className': 'widt' },
                        { 'data': 'course_name', 'title': 'Course Type', 'className': 'text-left' },
                        { 'data': 'mis_code', 'title': 'MIS ITI Code', 'className': 'text-left' },
                        { 'data': 'name_of_iti', 'title': 'Name of ITI', 'className': '' },
                        { 'data': 'trade', 'title': 'Trades', 'className': 'text-left' },
                        { 'data': 'no_units', 'title': 'Units', 'className': 'text-center' },
                        { 'data': 'division', 'title': 'Division', 'className': 'text-center' },
                        { 'data': 'district', 'title': 'District', 'className': 'text-center' },
                        { 'data': 'taluka', 'title': 'Taluka', 'className': 'text-left' },
                        { 'data': 'status', 'title': 'Status - Currently with', 'className': 'text-left' },
                        {
                            'data': 'mis_code',
                            render: function (data, type, row) {
                                if (row.FileUploadPath != null && row.FileUploadPath != "") {
                                    return "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + row.iti_college_id + "&Trade_Id=" + data.trade_id + "' ><img src ='/Content/img/pdf_logo.png' height ='40px' width ='40px' ></a>"
                                        
                                }
                                else {
                                    return "<p>File not uploaded</p>"
                                }

                            }
                        },

                        {
                            'data': 'trade_iti_id',
                            render: function (data, type, row) {

                                return "<input type='button' value='View' class='btn btn-primary' onclick='GetUpdateTradeEditLogin(" + row.trade_iti_id + ")' />"

                            }
                        }
                    ],
                    "columnDefs": [{
                        "render": function (data, type, full, meta) {
                           
                            return index++;
                        },
                        "targets": 0
                    }],
                    "createdRow": function (row, data, dataIndex) {
                        if (data["color_flag"] == "1") {
                            $(row).css("background-color", "#e6ffee");
                        }
                        else if (data["color_flag"] == "0") {
                            $(row).css("background-color", "#f2f2f2");
                        }
                    }

                    

                });

                $('#tblGridAffiliationCollege tbody').find("tr").each(function (i) {
                    

                    $('#tblGridAffiliationCollege tbody').find("tr").eq(i).find(".widt").css('width', '5%');

                });

                   

               

            }
            else {
                bootbox.alert("No data found!")
                $('#tblGridAffiliationCollege').DataTable().clear().draw();
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });
}


function GetTradeSession(this_row) {

    var key = $(this_row).closest("tr").find(".update-sessionkey").val();
    var trade = $(this_row).closest("tr").find(".trade-multi-select").find('option:selected').text();

    storeThis = $(this_row);
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradeShiftSession',
        data: { sessionKey: key },
        success: function (data) {
            if (data.flag == 1) {

                $("#Text_Trade_Shift").text(trade);
                $("#SessionId").val(key);
                var _shift_table = $("#Table-Shift tbody");
                _shift_table.empty();
                if (data.session.list.length > 0) {
                    $.each(data.session.list, function (i) {

                        var _tr_shift = $("<tr/>");
                        var _td_shift = $("<td/>");
                        //_td_shift.addClass("update-shift-units")
                        _td_shift.append("<input class='form-control update-shift-units' value='" + this.Units + "' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<input class='form-control update-shift' value='" + this.Shift + "' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='" + this.ITI_Trade_Shift_Id + "' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        var _checked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' checked>";
                        var _unchecked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' >";
                        var _unchecked_no = "<input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no'>";
                        var _checked_no = "<input class='form-check-input  isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no' checked>"
                        if (this.IsPPP == 'yes') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _unchecked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _checked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        _tr_shift.append(_td_shift);
                        var _checked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' checked>";
                        var _unchecked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' >"
                        var _unchecked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' >";
                        var _checked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' checked>"

                        _td_shift = $("<td/>");
                        if (this.Dual_System == 'regular') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _unchecked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _checked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        _btn_rem = $("<button/>");
                        _btn_rem.addClass("btn btn-danger");
                        _btn_rem.html("X");
                        _btn_rem.click(function () {

                            var lenght = $('#Table-Shift tbody tr').length;
                            if (lenght > 1) {
                                $(this).closest("tr").remove();
                                $(".update-shift-units").each(function (i) {
                                    $(this).text(i + 1);
                                });
                            }
                            else {
                                bootbox.alert("Atleast one row required")
                            }
                        });
                        _td_shift.append(_btn_rem);
                        _tr_shift.append(_td_shift);

                        _shift_table.append(_tr_shift);
                    });
                }
                else {

                    var _tr_shift = $("<tr/>");
                    var _td_shift = $("<td/>");
                    // _td_shift.addClass("update-shift-units")
                    _td_shift.append("<input class='form-control update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<input class='form-control update-shift' value='1' type='number' min=1 /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_0' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_0' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input isppp radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _btn_rem = $("<button/>");
                    _btn_rem.addClass("btn btn-danger remove-update-shift");
                    _btn_rem.html("X");
                    _btn_rem.click(function () {

                        var lenght = $('#Table-Shift tbody tr').length;
                        if (lenght > 1) {
                            $(this).closest("tr").remove();
                            $(".update-shift-units").each(function (i) {
                                $(this).text(i + 1);
                            });
                        }
                        else {
                            bootbox.alert("Atleast one row required")
                        }
                    });
                    _td_shift.append(_btn_rem);
                    _tr_shift.append(_td_shift);
                    _shift_table.append(_tr_shift);

                }

                $("#TradeShiftModal").modal("show");
            }
            else {
                bootbox.alert("failed");

            }

        }
    });
}

function SaveTradeSession() {
    let isValidShift = true;
    let isValid = true;

    var _shift_table = $("#Table-Shift tbody");
    var sendObj = [];
    var sessionKey = $("#SessionId").val();
    _shift_table.find("tr").each(function (len) {

        var $tr = $(this);
        var unit = $tr.find(".update-shift-units").val();
        if (unit == "" || unit == null || unit == "0") {
            isValid = false;
            $tr.find(".update-shift-units-required").show();
        }
        else {
            $tr.find(".update-shift-units-required").hide();
        }
        var shift = $tr.find(".update-shift").val();
        if (shift == "" || shift == null || shift == "0") {
            isValid = false;
            $tr.find(".update-shift-required").show();
        }
        else {
            $tr.find(".update-shift-required").hide();
        }
        var isppp = $tr.find("input[name='IsPPP_" + len + "']:checked").val();
        var dual = $tr.find("input[name='Dual_System_" + len + "']:checked").val();
        var trade_shift_id = $tr.find(".trade_shift_id").val();

        var obj = {
            Units: unit,
            Shift: shift,
            IsPPP: isppp,
            Dual_System: dual,
            ITI_Trade_Shift_Id: trade_shift_id
        };

        sendObj.push(obj);
    });


    const unique = []
    sendObj.filter(o => {

        if (unique.find(i => i.Units === o.Units && i.Shift === o.Shift)) {

            isValidShift = false;

        }
        else {
            unique.push(o)

        }

    });

    if (!isValidShift) {
        $("#validate-shift").show();
    } else {
        $("#validate-shift").hide();
    }

    var finalObj = JSON.stringify({ list: sendObj, sessionKey: sessionKey });
    if (isValid && isValidShift) {
        $.ajax({
            url: '/Affiliation/TradeShiftSession',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            data: finalObj,
            success: function (data) {

                if (data.falg == 1) {

                    _shift_table.empty();
                    $("#SessionId").val(0);
                    storeThis.closest("tr").find(".text-multi-units").text(sendObj.length);
                    storeThis.closest("tr").find(".update-sessionkey").val(data.sessionkey);
                    $("#Text_Trade_Shift").val("");
                    $("#TradeShiftModal").modal("hide");
                    storeThis.closest("tr").find(".btn-add-shift").text("Update Units & Shifts");

                    var total = 0;
                    $("#" + storeThis.closest("table").attr("id")).find(".text-multi-units").each(function () {
                        total += parseFloat($(this).text())

                    });
                    $("#Units").text(total);
                    $("#Units-Add").text(total);
                }
                else {
                    bootbox.alert("failed")

                }
            }
        });
    }

}

function ClearAddform() {
    $('.add').val(''); $('.add-ddl').val(""); $("#Table-Trade").find("tr:gt(1)").remove(); $('.update-sessionkey').val("0"); $(".text-multi-units").text("0"); $("#AffilicationDoc-Add").val(""); $('.validation_add').hide(); $(".trade-code").text(""); $(".trade-sector").text(""); $(".trade-type").text(""); $(".trade-duration").text(""); $(".trade-size").text(""); $(".btn-add-shift").text("Add Units & Shifts"); $("#District-Add").empty(); $("#District-Add").append('<option value="">Select</option>'); $("#Taluka-Add").empty(); $("#Taluka-Add").append('<option value="">Select</option>');
}

function ExitUpdateform() {
    $(".update").val(''); $(".update-dll").empty(); $("#DataGridDiv").show(); $("#UpdateDiv").hide(); $('.validation').hide(); $('.update-button').show(); $('.save-button').hide(); $(".update-text").text('');
}

function ViewShifts(this_row) {

    var key = $(this_row).closest("tr").find(".update-sessionkey").val();
    var trade = $(this_row).closest("tr").find(".trade-multi-select").find('option:selected').text();

    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetTradeShiftSession',
        data: { sessionKey: key },
        success: function (data) {
            if (data.flag == 1) {

                $("#Text_Trade_Shift-View").text(trade);

                var _shift_table = $("#TradeShiftModalView tbody");
                _shift_table.empty();
                if (data.session.list.length > 0) {
                    $.each(data.session.list, function (i) {

                        var _tr_shift = $("<tr/>");
                        var _td_shift = $("<td/>");
                        _td_shift.addClass("update-shift-units")
                        _td_shift.append(this.Units);
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<label class='form-control' >" + this.Shift + "</label>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<label class='form-control'>" + this.IsPPP + "</label>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<label class='form-control'>" + this.Dual_System + "</label>");
                        _tr_shift.append(_td_shift);

                        _shift_table.append(_tr_shift);
                    });
                }

                $("#TradeShiftModalView").modal("show");
            }
        }
    });
}

function checkIfDuplicateExists(arr) {
    return new Set(arr).size !== arr.length
}

function GetTradeCode(row) {
    
    $.ajax({
        url: "/Affiliation/GetAffiliationTradeCode",
        data: { trade_id: $(row).val() },
        type: "Get",
        success: function (code) {
            $(row).closest("tr").find(".trade-code").text(code.trade_code);
            $(row).closest("tr").find(".trade-sector").text(code.sector);
            $(row).closest("tr").find(".trade-type").text(code.trade_type);
            $(row).closest("tr").find(".trade-duration").text(code.trade_duration);
            $(row).closest("tr").find(".trade-size").text(code.trade_seating);
            
            $(row).closest("tr").find(".AidedUnaidedTrade").text(code.AidedUnaidedTrade);
        }
    });
}

function FindDublicateTradeSelected(id, row) {
    
    let arr = [];
    $("#" + id).find("tbody").find("tr").each(function () {
        let row = $(this);

        arr.push(row.find(".trade-multi-select").val());
    });

    let result = false;
    for (let i = 0; i < arr.length; i++) {
        // compare the first and last index of an element
        if (arr.indexOf(arr[i]) !== arr.lastIndexOf(arr[i])) {
            result = true;
            // terminate the loop
            break;
        }

    }

    if (result) {
        bootbox.alert("Trade already selected!");
        $(row).closest("tr").find(".trade-multi-select").val("")
        $(row).closest("tr").find(".trade-code").text("");
        $(row).closest("tr").find(".trade-sector").text("");
        $(row).closest("tr").find(".trade-type").text("");
        $(row).closest("tr").find(".trade-duration").text("");
        $(row).closest("tr").find(".trade-size").text("");
        $(row).closest("tr").find(".AidedUnaidedTrade").text("");
    }
    else {
        GetTradeCode(row);
    }
}

function GetTradeDetails(row) {
    
    $.ajax({
        url: "/Affiliation/GetAffiliationTradeCode",
        data: { trade_id: $(row).val() },
        type: "Get",
        success: function (code) {
            $("#TradeCode").text(code.trade_code);
            $("#SectorName").text(code.sector);
            $("#TradeType").text(code.trade_type);
            $("#Duration").text(code.trade_duration);
            $("#BatchSize").text(code.trade_seating);
            
            $("#AidedUnaidedTrade").text(code.AidedUnaidedTrade);

        }
    });
}

function GetAffiliationSchemes() {

    $("#Scheme-Add").empty();
    $("#Scheme-Add").append('<option value="">Select</option>');

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

function GetAffiliatedInstituteByName() {
    let collegeId = $("#NewTrade-Insti").val();
    if (collegeId == "" || collegeId == null || collegeId == 0) {
        bootbox.alert("Pls select Institute");
    }
    else {
        $('#MIS_CODE').val(null).trigger('change');
        $(".validation").hide();
        GetAffiliatedInstitute(collegeId);
    }
}

function GetAffiliatedInstituteByMisCode() {
    
    let collegeId = $("#MIS_CODE").val();
    if (collegeId == "" || collegeId == null || collegeId == 0) {
        bootbox.alert("Pls select Mis Code");
    }
    else {

        $("#NewTrade-District").val("");
        $("#NewTrade-Taluk").empty();
        $("#NewTrade-Taluk").append('<option value="">Select</option>');
        $("#NewTrade-Insti").empty();
        $("#NewTrade-Insti").append('<option value="">Select</option>');
        $(".validation").hide();

        GetAffiliatedInstitute(collegeId);
    }
}

function GetAffiliatedInstitute(collegeId) {
    
    $.ajax({
        url: "/Affiliation/GetAAffiliatedInstitute",
        type: "Get",
        contentType: 'application/json; charset=utf-8',
        data: { CollegeId: collegeId },
        success: function (data) {
            if (data.flag == 1) {

                clearNewTrade();

                $("#label-iti-name").text(data.name_of_iti)
                $("#label-iti-type").text(data.type_of_iti)
                $("#label-iti-location").text(data.location_type)
                $("#label-iti-mis").text(data.mis_code)
                $("#label-iti-course").text(data.course_name)
                $("#label-iti-division").text(data.division)
                $("#label-iti-district").text(data.district)
                $("#label-iti-taluk").text(data.taluka)
                $("#label-iti-constituency").text(data.constituency)
                $("#label-iti-built").text(data.build_up_area)
                $("#label-iti-address").text(data.address)
                $("#label-iti-geo").text(data.geo_location)
                $("#label-iti-email").text(data.email)
                $("#label-iti-pincode").text(data.Pincode)
                $("#label-iti-phone").text(data.phone_number)
                $("#label-iti-date").text(data.date)
                $("#label-iti-orderno").text(data.AffiliationOrderNo)
                $("#label-iti-orderdate").text(data.order_no_date)
                $("#label-iti-scheme").text(data.scheme_name)
                $("#label-iti-website").text(data.Website)
                $("#College_Id_New").val(collegeId);

                if (data.FileUploadPath != null && data.FileUploadPath != "") {
                    if (data.isSelect) {
                        //var html = "<a title='Download Pdf' class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId +"&Trade_Id=" + data.aff.trade_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        var html = "<a title='Download Pdf' class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + collegeId + "&Trade_Id=" + data.trade_id + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                        $("#NewTrade-Attachment").append(html);
                        $("#IsNewUploaded").val(true);
                    }
                    else {
                        var html = "<p>File not found</p>"
                        $("#NewTrade-Attachment").append(html);
                        $("#IsNewUploaded").val(false);
                    }

                }
                else {
                    $("#IsNewUploaded").val(false);
                }
                
                $("#TradeFileAttach1").empty();
                //if (data.UploadTradeAffiliationDoc != null && data.UploadTradeAffiliationDoc != "") {
                //    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationTradeDoc?CollegeId=" + collegeId + "' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>"
                //    $("#TradeFileAttach1").append(html);
                //    // $("#IsUploaded").val(true);
                //}
                //else {
                //    $("#tradepdf").hide();
                //    $("#tradepdf1").hide();
                //}

                $("#NewTradeDiv").show();
            }
            else {
                bootbox.alert("Error occured!!!");

            }
        },
        error: function () {
            bootbox.alert("Error", "something went wrong");
        }
    })
}

function clearNewTrade() {
    $(".newtrade-clear").text("");
    $("#NewTrade-Attachment").empty();
    $("#Table-Trade-New").find("tr:gt(1)").remove();
    $('.update-sessionkey').val("0");
    $(".text-multi-units").text("0");
    $("#AffilicationDoc-Add-New").val("");
    $(".trade-code").text("");
    $(".trade-sector").text("");
    $(".trade-type").text("");
    $(".trade-duration").text("");
    $(".trade-size").text("");
    $(".btn-add-shift").text("Add Units & Shifts");
    $(".trade-multi-select").val("");


}

function clearAllNewTrade() {
    $("#NewTradeDiv").hide();
    $(".newtrade-clear").text("");
    $("#NewTrade-Attachment").empty();
    $("#Table-Trade-New").find("tr:gt(1)").remove();
    $('.update-sessionkey').val("0");
    $(".text-multi-units").text("0");
    $("#AffilicationDoc-Add-New").val("");
    $(".trade-code").text("");
    $(".trade-sector").text("");
    $(".trade-type").text("");
    $(".trade-duration").text("");
    $(".trade-size").text("");
    $(".btn-add-shift").text("Add Units & Shifts");
    $(".trade-multi-select").val("");

    $("#NewTrade-District").val("");
    $("#NewTrade-Taluk").empty();
    $("#NewTrade-Taluk").append('<option value="">Select</option>');
    $("#NewTrade-Insti").empty();
    $("#NewTrade-Insti").append('<option value="">Select</option>');
    $('#MIS_CODE').val(null).trigger('change');


}
function PdfValidation(id) {
        
    var ext = $('#' + id).val().split('.').pop().toLowerCase();
        if (ext != "") {
            if ($.inArray(ext, ['pdf', 'PDF']) == -1) {
                bootbox.alert("<br><br>Kindly upload valid PDF file");
                $('#' + id).val("");
            }
        } else {
            $('#' + id).val("");
        }
    //var fileUpload = $("#uploadfile").get(0)
   // var files = fileUpload.files;
    if ($('#' + id).get(0).files[0].size > 500 *1024) {
            bootbox.alert("<br><br>File size should be less than 500kb");
            $('#' + id).val("");
        }

    }

function SaveAffiliatedInstituteTrade() {

    
    var fileUpload = $('#AffilicationDoc-Add-New').get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    
    //var size = parseFloat(fileUpload.files[0].size / 200);
    if (files.length > 0)
    {
        if (files[0].size > 200 * 1024) {
            return bootbox.alert('Pls select file less than 200 kb');

        }
    }
    var flow_id = $("#dd-flowtr").val();
    if (flow_id == null || flow_id =="") {
        return bootbox.alert("Pls select Send To");

    }
    var remarks = $("#RemarksTr").val();
    if (remarks == null || remarks =="")
    {
        return bootbox.alert("Pls enter remarks");
    }
    var neworderno = $("#New-OrderNo-Update1").val();
    var neworderdate = $("#New-OrderNoDate-Update1").val();
    var collegeId = $("#College_Id_New").val();
   
    //var today = new Date();
    //var dd = String(today.getDate()).padStart(2, '0');
    //var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    //var yyyy = today.getFullYear();

    //today = dd + '-' + mm + '-' + yyyy;
    //if (neworderdate != null)
    //{
    //    if (neworderdate > today) {
    //        return bootbox.alert("Affiliation order date is greater than current date, pls select date less than or equal to current date  ")
    //    }
    //}
   
    
    fileData.append(
        "iti_college_id", collegeId
    );

    var Ittype = $("#Aided").val();
    fileData.append(
        "AidedUnaidedTrade", Ittype
    );
    var IsValid = true;
    

    var File = $("#AffilicationDoc-Add-New").val();
    if (File == "") {
        //if ($("#IsNewUploaded").val()) {

        //}
        //else {
        //}
        return bootbox.alert("Pls Upload PDF Document");
        $("#file-add-new-Required").show();
        IsValid = false;
    }
    else {
        $("#file-add-new-Required").hide();
    }


    $("#Table-Trade-New tbody tr").each(function () {

        var $tr = $(this);
        var trade = $tr.find(".trade-multi-select").val()
        var key = $tr.find(".update-sessionkey").val();
        // var file = $tr.find(".add-trade-file").val()
        if (trade == "" || trade == null || trade == 0) {
            IsValid = false;
            return bootbox.alert("Pls Select trade");
            $tr.find(".trade-multi-select-required").show();
          
          
        }
        else {
            $tr.find(".trade-multi-select-required").hide();
        }
        if (key == 0) {
            IsValid = false;
            return bootbox.alert("Pls add units and shifts");
            $tr.find(".add_shift_required").show();
          
        }
        else {
            $tr.find(".add_shift_required").hide();
        }
    });


    if (IsValid) {

        $("#Table-Trade-New tbody tr").each(function () {

            var $tr = $(this);
            var trade = $tr.find(".trade-multi-select").val();
            var unit = $tr.find(".text-multi-units").text();
            var key = $tr.find(".update-sessionkey").val();
            var ititype = $tr.find(".Aided").val();
            // var isupload = $tr.find(".add-trade-isupload").val();

            fileData.append(
                "list_trades", trade
            );
            fileData.append(
                "list_units", unit
            );

            fileData.append(
                "list_type", ititype
            );

            fileData.append(
                "list_keys", key
            );
            fileData.append(
                "NewAffiliationOrderNo", neworderno
            );
            fileData.append(
                "NewAffiliationOrderNoDate", neworderdate
            );
        });
        fileData.append(
            "flow_id", flow_id
        );
        fileData.append(
            "remarks", remarks
        );

        if (flow_id == 5) {
            var name = "Deputy Director";
        }
        else if (flow_id == 3) {
            name = "Assistant Director";
        }
        else if (flow_id == 8) {
            name = "Case Worker";
        }
        else if (flow_id == 7) {
            name = "Office Superintendent";
        }


        

        //bootbox.confirm(" " + trade + 'details will be sent to Office Superintendent for review. Please Confirm ?', (confirma) => {
        bootbox.confirm('<br><br>Are you sure you want send the newly added trade(s) Details to ' + name +' for review ?', (confirma) => {
            if (confirma) {
            /*+ name +*/
                
                $.ajax({
                    type: 'POST',
                    url: '/Affiliation/SaveNewAffiliatedTrade',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (result) {

                        if (result.flag == 1) {

                            $("#NewTradeDiv").hide();
                            clearNewTrade();

                            $("#NewTrade-District").val("");
                            $("#NewTrade-Taluk").empty();
                            $("#NewTrade-Taluk").append('<option value="">Select</option>');
                            $("#NewTrade-Insti").empty();
                            $("#NewTrade-Insti").append('<option value="">Select</option>');
                            $('#MIS_CODE').val(null).trigger('change');

                            //bootbox.alert(" " + trade + "Institute Trade(s) added details is sent to Office Superintendent for review")
                            bootbox.alert("<br><br> Trade(s) added successfully and sent to "+ name +" for review");
                            fnRefreshScreenData();
                            GetAllAffiliationColleges(0, 0, 0);

                        }
                        else {
                            bootbox.alert(result.status)
                        }
                    }
                });

            }
        });



    }

}


function GetNewTradeDistricts() {
    $("#NewTrade-District").empty();
    $("#NewTrade-District").append('<option value="">Select</option>');
    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetDistricts',
        success: function (data) {
            if (data != null || data != '') {

                $.each(data, function () {
                    $("#NewTrade-District").append($("<option/>").val(this.Value).text(this.Text));
                });
            }

        }, error: function (result) {
            bootbox.alert("Error", "something went wrong");
        }
    });


}

function GetNewTradeTaluk(dist_id) {
    if (dist_id != 0 && dist_id != "" && dist_id != null) {
        $("#NewTrade-Taluk").empty();
        $("#NewTrade-Taluk").append('<option value="">Select</option>');
        $.ajax({
            url: "/Affiliation/GetTaluk",
            type: 'Get',
            data: { DistId: dist_id },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (data != null || data != '') {

                    $.each(data, function () {
                        $("#NewTrade-Taluk").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });

        $("#NewTrade-Insti").empty();
        $("#NewTrade-Insti").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetAllAffiliatedInstituteByDistrict',
            data: { dist_id: dist_id },
            success: function (data) {
                if (data != null || data != '') {

                    $.each(data, function () {
                        $("#NewTrade-Insti").append($("<option/>").val(this.Value).text(this.Text));
                    });

                    $("#NewTrade-Insti").select2("destroy").select2({
                        placeholder: "Select from " + data.length + " Institutes",
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        $("#NewTrade-Taluk").empty();
        $("#NewTrade-Taluk").append('<option value="">Select</option>');
        $("#NewTrade-Insti").empty();
        $("#NewTrade-Insti").append('<option value="">Select</option>');
    }

}

function GetAllAffiliatedInstituteByTaluk(taluk_id) {
    if (taluk_id != 0 && taluk_id != "" && taluk_id != null) {
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetAllAffiliatedInstituteByTaluk',
            data: { taluk_id: taluk_id },
            success: function (data) {
                $("#NewTrade-Insti").empty();
                $("#NewTrade-Insti").append('<option value="">Select</option>');

                if (data != null || data != '') {

                    $.each(data, function () {
                        $("#NewTrade-Insti").append($("<option/>").val(this.Value).text(this.Text));
                    });

                    $("#NewTrade-Insti").select2("destroy").select2({
                        placeholder: "Select from " + data.length + " Institutes",
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
    else {
        var dist_id = $("#NewTrade-District").val();
        $("#NewTrade-Insti").empty();
        $("#NewTrade-Insti").append('<option value="">Select</option>');
        $.ajax({
            type: 'Get',
            url: '/Affiliation/GetAllAffiliatedInstituteByDistrict',
            data: { dist_id: dist_id },
            success: function (data) {
                if (data != null || data != '') {

                    $.each(data, function () {
                        $("#NewTrade-Insti").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

            }, error: function (result) {
                bootbox.alert("Error", "something went wrong");
            }
        });
    }
}

function GetUploadedUpdateTrade(tradeId) {

    $.ajax({
        type: 'Get',
        url: '/Affiliation/GetUploadedTradeDetails',
        data: { iti_trade_id: tradeId },
        success: function (data) {

            if (data.aff.flag == 1) {

                $("#DataGridDiv").hide();
                $("#UpdateDiv").show();
                $('#Affiliationordernodate').hide();
                //button 
                $(".update-button").hide();
                $(".save-button").show();

                if (data.inst_list.length > 0) {
                    $("#TypeOfITI").empty();
                    $("#TypeOfITI").append($("<option/>").val("").text("Select"));
                    $.each(data.inst_list, function (index, values) {

                        $("#TypeOfITI").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.loca_type_list.length > 0) {
                    $("#LocationType").empty();
                    $("#LocationType").append($("<option/>").val("").text("Select"));
                    $.each(data.loca_type_list, function (index, values) {

                        $("#LocationType").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.css_code_list.length > 0) {
                //	$.each(data.css_code_list, function (index, values) {
                //		$("#CssCode").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                if (data.trades_list.length > 0) {
                    $("#Update_Trade").empty();
                    $("#Update_Trade").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#Update_Trade").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.div_list.length > 0) {
                    $("#Division-Update").empty();
                    $("#Division-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.div_list, function (index, values) {

                        $("#Division-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.dist_list.length > 0) {
                    $("#District").empty();
                    $("#District").append($("<option/>").val("").text("Select"));
                    $.each(data.dist_list, function (index, values) {

                        $("#District").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }

                if (data.taluk_list.length > 0) {
                    $("#Taluka").empty();
                    $("#Taluka").append($("<option/>").val("").text("Select"));
                    $.each(data.taluk_list, function (index, values) {

                        $("#Taluka").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.consti_list.length > 0) {
                    $("#Constiteuncy").empty();
                    $("#Constiteuncy").append($("<option/>").val("").text("Select"));
                    $.each(data.consti_list, function (index, values) {

                        $("#Constiteuncy").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.cou_list.length > 0) {
                    $("#Course-Update").empty();
                    $("#Course-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.cou_list, function (index, values) {

                        $("#Course-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.schem_list.length > 0) {
                    $("#Scheme-Update").empty();
                    $("#Scheme-Update").append($("<option/>").val("").text("Select"));
                    $.each(data.schem_list, function (index, values) {

                        $("#Scheme-Update").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                if (data.trades_list.length > 0) {
                    $("#ListOfTrades").empty();
                    $("#ListOfTrades").append($("<option/>").val("").text("Select"));
                    $.each(data.trades_list, function (index, values) {

                        $("#ListOfTrades").append($("<option/>").val(values.Value).text(values.Text));
                    });
                }
                //if (data.pancha_list.length > 0) {
                //	$.each(data.pancha_list, function (index, values) {
                //		$("#Panchayat").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                //if (data.village_list.length > 0) {
                //	$.each(data.village_list, function (index, values) {
                //		$("#Village").append($("<option/>").val(values.Value).text(values.Text));
                //	});
                //}
                
                $("#ITIName").val(data.aff.name_of_iti);
                $("#TypeOfITI").val(data.aff.type_of_iti_id);
                $("#LocationType").val(data.aff.location_type_id);
                $("#MISCode").val(data.aff.mis_code);
                $("#Division-Update").val(data.aff.division_id);
                // $("#CssCode").val(data.aff.css_code);
                $("#District").val(data.aff.dist_id);
                $("#Taluka").val(data.aff.taluk_id);
                $("#Constiteuncy").val(data.aff.consti_id);
                //$("#Panchayat").val(data.aff.pancha_id);
                //$("#Village").val(data.aff.village_id);
                $("#BuildUpArea").val(data.aff.build_up_area);
                $("#Address").val(data.aff.address);
                $("#GeoLocation").val(data.aff.geo_location);
                $("#Email").val(data.aff.email);
                $("#Pincode").val(data.aff.Pincode);
                $("#PhoneNumber").val(data.aff.phone_number);
                $("#AffiliationDate").val(data.aff.date.replaceAll('-', '/'));
                $("#Units").text(data.aff.no_units);
                $("#Course-Update").val(data.aff.course_code);
                $("#OrderNo-Update").val(data.aff.AffiliationOrderNo);
                $("#OrderNoDate-Update").val(data.aff.order_no_date.replaceAll('-', '/'));
                $("#Scheme-Update").val(data.aff.Scheme);
                $("#WebSite-Update").val(data.aff.Website);
                $("#TradeName").text(data.aff.trade);
                $("#SectorName").text(data.aff.sector);
                $("#TradeCode").text(data.aff.trade_code);
                $("#TradeType").text(data.aff.trade_type);
                $("#Duration").text(data.aff.duration);
                $("#BatchSize").text(data.aff.batch_size);
                $("#AidedUnaidedTrade").text(data.aff.AidedUnaidedTrade);
                $("#Trade_ITI_Id").val(tradeId);
                $("#CollegeId").val(data.aff.iti_college_id);
                $("#Trade_Id").val(data.aff.trade_id);
                $("#ListOfTrades").val(data.aff.trade_id);
                if (data.aff.Filename!=null) {
                    $(".AfffiliationName").val(data.aff.Filename);
                }
                
                $("#FileAttach").empty();
                $("#TradeFileAttach").empty();
                if (data.aff.FileUploadPath != null && data.aff.FileUploadPath != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "' >Download File</a>"
                    $("#FileAttach").append(html);
                    $("#IsUploaded").val(true);
                }
                else {
                    $("#IsUploaded").val(false);
                }
                
                if (data.aff.UploadTradeAffiliationDoc != null && data.aff.UploadTradeAffiliationDoc != "") {
                    var html = "<a class='btn btn-link' href='/Affiliation/DownloadAffiliationTradeDoc?CollegeId=" + data.aff.iti_college_id + "' >Download File</a>"
                    $("#TradeFileAttach").append(html);
                   // $("#IsUploaded").val(true);
                }
                else {
                    $("#tradedoc").hide();
                    $("#tradedoc1").hide();
                }
                $(".update-button").attr("disabled", (!data.aff.en_edit));
                $(".update-row-new-units").attr("disabled", (!data.aff.en_edit));


                var _table = $("#tblShits tbody");
                _table.empty();

                if (data.aff.shifts.length > 0) {
                    $.each(data.aff.shifts, function (i) {

                        var _tr_shift = $("<tr/>");
                        var _td_shift = $("<td/>");
                        //_td_shift.addClass("update-shift-units")
                        _td_shift.append("<input class='form-control update-shift-units' value='" + this.Units + "' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        _td_shift.append("<input class='form-control update-shift' value='" + this.Shift + "' type='number' min='1'  max='3' /><input type='hidden' class='trade_shift_id' value='" + this.ITI_Trade_Shift_Id + "' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                        _tr_shift.append(_td_shift);
                        _td_shift = $("<td/>");
                        var _checked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' checked>";
                        var _unchecked_yes = "<input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='yes' >";
                        var _unchecked_no = "<input class='form-check-input isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no'>";
                        var _checked_no = "<input class='form-check-input  isppp radio-no' type='radio' name='IsPPP_" + i + "' id='exampleRadios1' value='no' checked>"
                        if (this.IsPPP == 'yes') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _unchecked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_yes + "<label class='form-check-label'>Yes</label></div><div class='col-md-6'>" + _checked_no + "<label class='form-check-label'>No</label></div></div>");
                        }
                        _tr_shift.append(_td_shift);
                        var _checked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' checked>";
                        var _unchecked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' >"
                        var _unchecked_daul_yes = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='regular' >";
                        var _checked_dual_no = "<input class='form-check-input radio-dual' type='radio' name='Dual_System_" + i + "' id='exampleRadios1' value='dual' checked>"

                        _td_shift = $("<td/>");
                        if (this.Dual_System == 'regular') {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _checked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _unchecked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }
                        else {
                            _td_shift.append("<div class=''><div class='col-md-6'>" + _unchecked_daul_yes + "<label class='form-check-label'>Regular</label></div><div class='col-md-6'>" + _checked_dual_no + "<label class='form-check-label'>Dual</label></div></div>");
                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        if (this.IsActive) {
                            _td_shift.text("Active");
                            _td_shift.append("<input type='hidden' class='active_status' value='true' />");
                        }
                        else {
                            _td_shift.text("DeActive");
                            _td_shift.append("<input type='hidden' class='active_status' value='false' />");
                        }

                        _tr_shift.append(_td_shift);

                        _td_shift = $("<td/>");
                        _btn_rem = $("<button/>");
                        _btn_rem.addClass("btn btn-danger");
                        _btn_rem.html("X").attr("disabled", (!data.aff.en_edit));
                        _btn_rem.click(function () {

                            var lenght = $('#tblShits tbody tr').length;
                            if (lenght > 1) {
                                $(this).closest("tr").remove();
                                $(".update-shift-units").each(function (i) {
                                    $(this).text(i + 1);
                                });
                            }
                            else {
                                bootbox.alert("Atleast one row required")
                            }
                        });
                        _td_shift.append(_btn_rem);
                        _tr_shift.append(_td_shift);

                        _table.append(_tr_shift);
                    });
                }
                else {

                    var _tr_shift = $("<tr/>");
                    var _td_shift = $("<td/>");
                    // _td_shift.addClass("update-shift-units")
                    _td_shift.append("<input class='form-control update-shift-units' value='1' type='number' min=1 /><small class='text-danger update-shift-units-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<input class='form-control update-shift' value='1' type='number' min='1' max='3' /><input type='hidden' class='trade_shift_id' value='0' /><small class='text-danger update-shift-required' style='display:none'>*required</small>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input isppp radio-yes' type='radio' name='IsPPP_0' id='exampleRadios1' value='yes'><label class='form-check-label'>Yes</label></div><div class='col-md-6'><input class='form-check-input isppp radio-no' type='radio' name='IsPPP_0' id='exampleRadios1' value='no' checked><label class='form-check-label'>No</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("<div class=''><div class='col-md-6'><input class='form-check-input radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='regular'><label class='form-check-label'>Regular</label></div><div class='col-md-6'><input class='form-check-input isppp radio-dual' type='radio' name='Dual_System_0' id='exampleRadios1' value='dual' checked><label class='form-check-label'>Dual</label></div></div>");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _td_shift.append("");
                    _tr_shift.append(_td_shift);
                    _td_shift = $("<td/>");
                    _btn_rem = $("<button/>");
                    _btn_rem.addClass("btn btn-danger remove-update-shift");
                    _btn_rem.html("X").attr("disabled", (!data.aff.en_edit));
                    _btn_rem.click(function () {

                        var lenght = $('#tblShits tbody tr').length;
                        if (lenght > 1) {
                            $(this).closest("tr").remove();
                            $(".update-shift-units").each(function (i) {
                                $(this).text(i + 1);
                            });
                        }
                        else {
                            bootbox.alert("Atleast one row required")
                        }
                    });
                    _td_shift.append(_btn_rem);
                    _tr_shift.append(_td_shift);
                    _table.append(_tr_shift);

                }

                $("#tblUpdateHistory tbody").empty();
                if (data.his_list.length > 0) {
                    $("#UpdateTabHistory").show();
                    $.each(data.his_list, function (i) {
                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        //_tr.append("<td>" + this.TradeName + "</td");
                        _tr.append("<td>" + this.date + "</td");

                        _tr.append("<td>" + this.sent_by + "</td");
                        _tr.append("<td>" + this.Flow_user + "</td>");
                        _tr.append("<td>" + this.Status + "</td>");
                        _tr.append("<td>" + this.Remarks + "</td>");

                        //if (this.FileUploadPath != null && this.FileUploadPath != "") {
                        //    _tr.append("<td><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + this.Trade_ITI_id + "' >Download File</a></td>");
                        //}
                        //else {
                        //    _tr.append("<td>File not uploaded</td>");
                        //}

                        $("#tblUpdateHistory tbody").append(_tr);
                    });
                }
                else {
                    $("#UpdateTabHistory").hide();
                }
                $("#tblAffiliationDoc").hide();
                if (data.aff.AffiliationDocs != null) {
                if (data.aff.AffiliationDocs.length > 0) {
                    $("#tblAffiliationDoc").show();
                    var _table = $("#tblAffiliationDoc tbody");
                    _table.empty();
                    $.each(data.aff.AffiliationDocs, function (i) {

                        var _tr = $("<tr/>");
                        _tr.append("<td>" + (i + 1) + "</td>");
                        _tr.append("<td>" + this.AffiliationOrder_Number + "</td>");
                        _tr.append("<td>" + this.Affiliation_date.split(' ')[0] + "</td>");
                        _tr.append("<td >" + this.FileName + "</td>");
                        _tr.append("<td ><a class='btn btn-link' href='/Affiliation/DownloadAffiliationDoc?CollegeId=" + data.aff.iti_college_id + "&Trade_Id=" + data.aff.trade_id +"' ><img src='/Content/img/pdf_logo.png' height='40px' width='40px' /></a>" + "" + "</td>");
                        //_tr.append("<td >" + this.Status + "</td>");
                        _table.append(_tr);
                    });
                }
                }
                $("#Adress_Update_Count").text(document.getElementById("Address").value.length);
                $("#Name_Update_Count").text(document.getElementById("ITIName").value.length);
                $("#MIS_Update_Count").text(document.getElementById("MISCode").value.length);
                $("#Website_Update_Count").text(document.getElementById("WebSite-Update").value.length);
                //$("#BuildUp_Count").text(document.getElementById("BuildUpArea").value.length);
                $("#GeoLocation_Update_Count").text(document.getElementById("GeoLocation").value.length);
                $("#OrderNo_Update_Count").text(document.getElementById("OrderNo-Update").value.length);

            } else {
                bootbox.alert("Error occured while loading data")
            }


        }
    });
}

function checkDublicateTrade(id, row) {

    let tradeId = $(row).val();
    let collegeId = $("#College_Id_New").val()
    if (tradeId != "") {
        let arr = [];
        $("#" + id).find("tbody").find("tr").each(function () {
            let row = $(this);

            arr.push(row.find(".trade-multi-select").val());
        });

        let result = false;
        for (let i = 0; i < arr.length; i++) {
            // compare the first and last index of an element
            if (arr.indexOf(arr[i]) !== arr.lastIndexOf(arr[i])) {
                result = true;
                // terminate the loop
                break;
            }

        }

        if (result) {
            bootbox.alert("Trade already selected!");
            $(row).closest("tr").find(".trade-multi-select").val("")
            $(row).closest("tr").find(".trade-code").text("");
            $(row).closest("tr").find(".trade-sector").text("");
            $(row).closest("tr").find(".trade-type").text("");
            $(row).closest("tr").find(".trade-duration").text("");
            $(row).closest("tr").find(".trade-size").text("");
            $(row).closest("tr").find(".AidedUnaidedTrade").text("");
        }
        else {

            $.ajax({
                url: "/Affiliation/IsAffiliatedTradeAlreadyExists",
                data: { trade_code: tradeId, college_id: collegeId },
                type: "Get",
                success: function (exists) {

                    if (exists) {
                        $(row).val("");
                        bootbox.alert("Trade Already Exists")
                    }
                    else {

                        GetTradeCode(row);
                    }
                }
            })
        }
    }
}


function ClearMasterUpload() {
    $(this).val("");
    $("#FilePath").text("");
    $("#BtnUpload").val("Browse");
    $("#btnUpload").prop("disabled", false);
    $("#btnUpload").val("Preview");
    $(".submit-button").attr("disabled", true);
    $("#FileUpload").val("")

    GetAllUploadedAffiliationColleges();
}

function IsInstituteMiscoceExists(control) {

    $.ajax({
        url: "/Affiliation/IsMisCodeExists",
        data: { miscode: $(control).val() },
        type: "Get",
        success: function (exists) {

            if (exists) {
                $(control).val("");
                bootbox.alert("MIS Code Already Exists");
            }
        }
    });
}

function IsInstituteiticollegeExists(control) {

    $.ajax({
        url: "/Affiliation/IsITIColllegeNameExists",
        data: { iticollegename: $(control).val() },
        type: "Get",
        success: function (exists) {

            if (exists) {
                $(control).val("");
                bootbox.alert("ITI College Name Already Exists");
            }
        }
    });
}


function cancelAffiliatedInstitute() {
    $('#CourseTypes_Af').val('');
    $('#Divisions_Af').val('');
    $('#Districts_Af').val('choose');
    $('#Trades_Af').val('');

}

function cancelUpdateITIInstitute() {
    $('#CourseTypes').val('');
    $('#Divisions').val('');
    $('#Districts').val('');
    $('#Trades').val('');

}

function cancelAddNewAffiliatedInstitute() {
    $('#NewTrade-District').val('');
    $('#NewTrade-Taluk').val('');
    $('#NewTrade-Insti').val('');
    $('#select2-NewTrade-Insti-container').text('');

}


//Add
$("#Address-Add").on("input", function () {
    $("#Adress_Count").text(this.value.length);
});
$("#ITIName-Add").on("input", function () {
    $("#Name_Count").text(this.value.length);
});
$("#MISCode-Add").on("input", function () {
    $("#MIS_Count").text(this.value.length);
});
$("#WebSite-Add").on("input", function () {
    $("#Website_Count").text(this.value.length);
});
$("#BuildUpArea-Add").on("input", function () {
    $("#BuildUp_Count").text(this.value.length);
});
$("#GeoLocation-Add").on("input", function () {
    $("#GeoLocation_Count").text(this.value.length);
});
$("#OrderNo-Add").on("input", function () {
    $("#OrderNo_Count").text(this.value.length);
});

//Update
$("#Address").on("input", function () {
    $("#Adress_Update_Count").text(this.value.length);
});
$("#ITIName").on("input", function () {
    $("#Name_Update_Count").text(this.value.length);
});
$("#MISCode").on("input", function () {
    $("#MIS_Update_Count").text(this.value.length);
});
$("#WebSite-Update").on("input", function () {
    $("#Website_Update_Count").text(this.value.length);
});
$("#BuildUpArea").on("input", function () {
    $("#BuildUp_Count").text(this.value.length);
});
$("#GeoLocation").on("input", function () {
    $("#GeoLocation_Update_Count").text(this.value.length);
});
$("#OrderNo-Update").on("input", function () {
    $("#OrderNo_Update_Count").text(this.value.length);
});

function GetDownloadFileLinkForDocument() {
    
    var DownloadFileLinkId1 = $('#DownloadFileLink2').val();
    var urlPath = "../../Affiliation/" + DownloadFileLinkId1;
    window.open(urlPath, '_blank');

}

