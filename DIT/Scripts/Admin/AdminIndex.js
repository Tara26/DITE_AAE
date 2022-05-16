var userId = 0;
var userMapId = 0;
var formdata = new FormData();
$(document).ready(function () {
    fnCkDITENonDITE();
    fnGetUnitList('ddlUnitSelect');
    fnGetUserData();
    //fnShowProfileScreen('');
    fnShowHideOnLoad();

    //GetSeatAvailability();
    $("#dateofbirth").datepicker({
        dateFormat: 'dd-mm-yy',
        yearRange: "-60:+0",
        changeMonth: true,
        changeYear: true
    });
});


if (window.File && window.FileList && window.FileReader) {
    $("#files").on("change", function (e) {
        var files = e.target.files;
        fnShowUploadedPhotos("files", files);
    });
    $("#files1").on("change", function (e) {
        var files = e.target.files;
        fnShowUploadedPhotos("files1", files);
    });
} else {
    bootbox.alert("Your browser doesn't support to File API")
}

function fnShowUploadedPhotos(id, files) {
    filesLength = files.length;
    for (var i = 0; i < filesLength; i++) {
        var f = files[i]
        var fileReader = new FileReader();
        fileReader.onload = (function (e) {
            var file = e.target;
            $("<span class=\"pip\">" +
                "<img class=\"imageThumb\" src=\"" + e.target.result + "\" title=\"" + file.name + "\"/>" +
                "<br/><span class=\"remove\">Remove image</span>" +
                "</span>").insertAfter("#" + id);
        });
        fileReader.readAsDataURL(f);
    }
}

function fnDeleteImg(imgsrc) {
//$(".remove").click(function () {
    var msg = "<br />Are you sure you want to delete the selected image?"//$(this).parent(".pip").remove();
    imgsrc = imgsrc.substring(imgsrc.indexOf("Content"));
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
        callback: function (confirmed) {
            if (confirmed) {
                $.ajax({
                    url: "/Admin/DeleteImagesFromFolder",
                    type: 'Get',
                    data: { path: imgsrc },
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        
                        if (data == true) {
                            bootbox.alert("Image Deleted Successfully!!!", function () {
                                location.reload();
                            });
                        }
                        else
                            bootbox.alert("Image does NOT exist!!!");
                    }, error: function (result) {
                        bootbox.alert("something went wrong");
                    }
                });
            }
        }
    });
}


$('a[href="#tab_1"]').click(function () {
    fnGetUnitList('ddlUnitSelect');
    fnGetRolesData();
    fnShowHideOnLoad();
});

$('a[href="#tab_2"]').click(function () {
    
    fnGetUnitList('ddlUnits');
    //GetLocation('ddlLocation');
    fnGetSectionsList('', 'ddlSection');
    fnGetWingsList('ddlWing');
    fnGetRolesList('', '', 'ddlRole');
    fnShowHideOnLoad();
    fnGetRoleMapData('');

});


$('a[href="#tab_3"]').click(function () {
    fnGetUnitList('ddlUnits1');
    //GetLocation('ddlLocation1');
    fnGetSectionsList('', 'ddlSection1');
    fnGetWingsList('ddlWing1');
    fnGetRolesList('', '', 'ddlRole1');
    fnShowHideOnLoad();
    fnGetRoleMapDetailsData(0);
});

$('a[href="#tab_4"]').click(function () {
    fnGetUnitList('ddlUnits2');
    GetDivisionsDDp('ddlLocation2');
    fnGetWingsList('ddlWing2');
    fnGetSectionsList('', 'ddlSection2');
    fnGetRolesList('', '', 'ddlRole2');
    fnShowHideOnLoad();
    fnGetUserMapData();
});


$('a[href="#tab_6"]').click(function () {

    fnShowHolidayList(0, '');
    fnShowPhotos("files", files);
    fnShowPhotos("files1", files);
});

$("#txtRoleName").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/Admin/GetEnteredRoleList",
            type: "POST",
            dataType: "json",
            data: { prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item };
                }))
            }
        })
    },
});


$(".cbxDITE").change(function () {
    $(".cbxDITE").not(this).prop('checked', false);
    fnClearUserMapping();
    fnCkDITENonDITE();
});

$('#txtKGIDUserId').keypress(function () {
    var maxLength = $(this).val().length;
    if (($("#cbxDiteEe").prop('checked') == true && maxLength == 7) || ($("#cbxDiteEe").prop('checked') == false && maxLength == 10)) {
        return false;
    }
});

function fnCkDITENonDITE() {
    if ($("#cbxDiteEe").prop('checked') == true) {
        $("#lblKGIDUserId").text("KGID Number");
        $(".NonDITEPassword").hide();
    }
    else {
        $("#lblKGIDUserId").text("User Id");
        $(".NonDITEPassword").show();
    }
}

function fnGetUnitList(id) {
    $("#" + id).empty();
    $("#" + id).append('<option value="0">Choose</option>');
    $.ajax({
        url: "/Admin/GetUnitsList",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + id).append($("<option/>").val(this.Value).text(this.Text));
                });
                if (id.includes('Select')) {
                    $("#" + id).multiselect({});
                }
            }
        }, error: function (result) {
            bootbox.alert("something went wrong");
        }
    });
}

function fnGetSectionsList(id, id1) {
    var UnitId = 0;
    if (id.includes("ddlUnits2"))
        UnitId = $("#" + id).val();

    $("#" + id1).empty();
    $("#" + id1).append('<option value="0">Choose</option>');
    $.ajax({
        url: "/Admin/GetSectionsList",
        type: 'Get',
        data: { UnitId: UnitId },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + id1).append($("<option/>").val(this.Value).text(this.Text));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}


function fnGetWingsList(id) {
    $("#" + id).empty();
    $("#" + id).append('<option value="0">Choose</option>');
    $.ajax({
        url: "/Admin/GetWingsList",
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + id).append($("<option/>").val(this.Value).text(this.Text));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function fnGetRolesList(id, id1, id2) {
    var unitId = 0;
    if (id != "")
        unitId = $("#" + id).val();
    var sectionId = 0;
    if (id1.includes("ddlSection2") != "")
        sectionId = $("#" + id1).val();
    $("#" + id2).empty();
    $("#" + id2).append('<option value="0">Choose</option>');
    $.ajax({
        url: "/Admin/GetRolesList",
        data: { unitId: unitId, sectionId: sectionId },
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != null || data != '') {
                $.each(data, function () {
                    $("#" + id2).append($("<option/>").val(this.Value).text(this.Text));
                });
            }
        }, error: function (result) {
            alert("Error", "something went wrong");
        }
    });
}

function fnAddRoles() {
    fnClearAllErrMsgs();
    var IsValid = true;
    if ($("#txtRoleName").val() == '') {
        $("#txtRoleName-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        var fileData = new FormData();
        fileData.append(
            "RoleName", $("#txtRoleName").val()
        );

        var ddlUnitSelectId = "ddlUnitSelect";
        if ($('#tblAddRoles').DataTable().search($("#txtRoleName").val().toString()).row({ search: 'applied' }).data() != undefined) {
            ddlUnitSelectId = ddlUnitSelectId + "1";
        }
        if ($("#" + ddlUnitSelectId).val() == null) {
            $("#txtUnit-Required").show();
            IsValid = false;
        }
        if (IsValid) {
            $("#" + ddlUnitSelectId + " :selected").each(function (i, selected) {
                fileData.append(
                    "MultiSelectUnitList", $(selected).val()
                );
            });

            bootbox.confirm({
                message: "<br><br>Are you sure you want to " + (ddlUnitSelectId == "ddlUnitSelect" ? "Add" : "Update") + " Role <b>'" + $("#txtRoleName").val() + "'</b> ?",
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
                callback: function (confirmed) {
                    if (confirmed) {
                        $.ajax({
                            url: "/Admin/GetUnitsList",
                            type: 'POST',
                            data: fileData,
                            contentType: false,
                            processData: false,
                            success: function (data) {
                                if (data == "Success") {
                                    bootbox.alert("Role " + (ddlUnitSelectId == "ddlUnitSelect" ? "Added" : "Updated") + " '<b>" + $("#txtRoleName").val() + "</b>' Successfully!!");
                                }
                                else if (data == "Updated") {
                                    bootbox.alert("Role <b>" + $("#txtRoleName").val() + "</b> Updated Successfully!!");
                                    //$("#ddlUnitSelect1").next().hide();
                                    //$('#ddlUnitSelect1').after('<div class="col-md-8"><select class= "form-control clear" id = "ddlUnitSelect" multiple /><small class="text-danger" id="ddlUnitSelect-Required"></small></div>');
                                    //$("#ddlUnitSelect").next().show();
                                }
                                else if (data == "Exists") {
                                    bootbox.alert("The role <b>" + $("#txtRoleName").val() + "</b> already exists. Please enter a different role.");
                                }
                                else {
                                    bootbox.alert("failed");
                                }
                                fnGetRolesData();
                            }, error: function (result) {
                                bootbox.alert("Error", "something went wrong");
                            }
                        });
                        fnClearAddRoles();
                    }
                }
            });
        }
    }
}

function fnAddUserMapping() {
    fnClearAllErrMsgs();
    var IsValid = true;

    if ($("#txtKGIDUserId").val() == '') {
        $("#txtKGIDUserId-Required").show();
        IsValid = false;
    }
    //if ($("#txtEEName").val() == '') {
    //    $("#txtEEName-Required").show();
    //    IsValid = false;
    //}
    //if ($("#dateofbirth").val() == '') {
    //    $("#dateofbirth-Required").show();
    //    IsValid = false;
    //}
    //if ($("#txtPhoneNum").val() == '') {
    //    $("#txtPhoneNum-Required").show();
    //    IsValid = false;
    //}
    //if ($("#txtEmail").val() == '') {
    //    $("#txtEmail-Required").show();
    //    IsValid = false;
    //}

    if ($('#ddlUnits2').val() == 0) {
        $("#ddlUnits2-Required").show();
        IsValid = false;
    }
    if ($('#ddlWing2').val() == 0 && $("#ddlWing2").is(':visible')) {
        $("#ddlWing2-Required").show();
        IsValid = false;
    }
    if ($('#ddlLocation2').val() == 'choose' && $("#ddlLocation2").is(':visible')) {
        $("#ddlLocation2-Required").show();
        IsValid = false;
    }
    if ($('#ddlDistrict2').val() == 0 && $("#ddlDistrict2").is(':visible')) {
        $("#ddlDistrict2-Required").show();
        IsValid = false;
    }
    if ($('#ddlTaluk2').val() == 0 && $("#ddlTaluk2").is(':visible')) {
        $("#ddlTaluk2-Required").show();
        IsValid = false;
    }
    if ($('#ddlITIName2').val() == 0 && $("#ddlITIName2").is(':visible')) {
        $("#ddlITIName2-Required").show();
        IsValid = false;
    }
    if ($('#ddlSection2').val() == 0) {
        $("#ddlSection2-Required").show();
        IsValid = false;
    }
    if ($('#ddlRole2').val() == 0) {
        $("#ddlRole2-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        var lstRoleMap = [];

        var dateofbirth = "";
        $("#dateofbirth-Required").hide();
        var dateofbirth = $("#dateofbirth").val();
        if (dateofbirth == "") {
            $("#dateofbirth-Required").show();
            IsValid = false;
        }
        else {
            var dts = $("#dateofbirth").val().split("-");
            conversiondate = new Date(dts[1] + "-" + dts[0] + "-" + dts[2]);
            dateofbirth = (dts[0] + "-" + dts[1] + "-" + dts[2]);
        }

        var sendObj = [];
        var list = {
            UnitId: $("#ddlUnits2").val(),
            LocationDivId: $("#ddlLocation2").val(),
            SectionId: $("#ddlSection2").val(),
            RoleId: $("#ddlRole2").val(),
            DistrictId: $("#ddlDistrict2").val(),
            TalukId: $("#ddlTaluk2").val(),
            InstituteId: $("#ddlITIName2").val()
        }
        lstRoleMap.push(list);

        var obj = {
            DiteNonDiteEE: $("input[name='cbxDITE']:checked").val(),
            KGIDUserID: $("#txtKGIDUserId").val(),
            EEName: $("#txtEEName").val(),
            phoneNum: $("#txtPhoneNum").val(),
            email: $("#txtEmail").val(),
            pwd: $("#txtPwd").val(),
            DOB: dateofbirth,
            lstRoleMapping: lstRoleMap,
            btnValue: $("#btnSubmitUserMap").val(),
            userId: userId,
            userMapId: userMapId
        };
        sendObj.push(obj);

        bootbox.confirm({
            message: "<br><br>Are you sure you want to " + (userId == 0 ? "Add" : "Update") + " User Mapping ?",
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
            callback: function (confirmed) {
                if (confirmed) {

                    $.ajax({
                        url: "/Admin/AddUserMapping",
                        type: 'POST',
                        data: JSON.stringify({ lstUserMap: sendObj }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            if (data == "Success") {
                                bootbox.alert("<br><br> User Mapping " + (userId != 0 ? "Added" : "Updated") + " successfully");
                                fnGetUserMapData();
                                fnClearUserMapping();
                                userId = 0;
                                userMapId = 0;
                            }
                            else {
                                bootbox.alert("failed");
                            }
                        }, error: function (result) {
                            bootbox.alert("Error", "something went wrong");
                        }
                    });
                }
            }
        });
    }
}

function fnGetUserData() {
    $.ajax({
        type: "GET",
        url: "/Admin/GetUserData",
        contentType: "application/json",
        success: function (data) {
            $('#tblUserDetails').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'sino.', 'title': 'SI No.', 'className': 'text-center' },
                    {
                        'data': 'KGIDNumber', 'title': 'DITE/Non-DITE', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            if (oData.KGIDNumber != null)
                                $(nTd).html("DITE");
                            else
                                $(nTd).html("Non-DITE");
                        }
                    },
                    { 'data': 'KGIDNumber.', 'title': 'KGID Number', 'className': 'text-center' },
                    { 'data': 'UserName', 'title': 'Name of the User', 'className': 'text-center' },
                    { 'data': 'DOB', 'title': 'Date Of Birth', 'className': 'text-center' },
                    { 'data': 'Gender', 'title': 'Gender', 'className': 'text-center' },
                    { 'data': 'Designation', 'title': 'Designation', 'className': 'text-center' },
                    { 'data': 'FatherName', 'title': 'Father Name', 'className': 'text-center' },
                    { 'data': 'Mobile', 'title': 'Mobile Number', 'className': 'text-center' },
                    { 'data': 'Email', 'title': 'Email Id', 'className': 'text-center' },
                    //{
                    //    'data': 'Photo', 'title': 'Photo', 'className': 'text-center',
                    //    render: function (data, type, row) {
                    //        return "<img src='../../" + row.Photo + "' class='zoom'/>";
                    //    }
                    //},
                    {
                        'data': 'status', 'title': 'Status', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.userId + ", " + 0 + ", " + 0 + ",\"" + oData.UserName + "\"," + oData.status + ")' class='btn btn-xs btn-primary' " + (oData.status == true ? " disabled" : "") + " value='Active' />" +
                                "<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.userId + ", " + 0 + ", " + 0 + ",\"" + oData.UserName + "\"," + oData.status + ")' class = 'btn btn-xs btn-danger'" + (oData.status == false ? " disabled" : "") + " value = 'DeActive' /> ");
                        }
                    },
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}


function fnGetRolesData() {
    $.ajax({
        type: "GET",
        url: "/Admin/GetRolesData",
        contentType: "application/json",
        success: function (data) {
            $('#tblAddRoles').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'RoleName', 'title': 'Role Name', 'className': 'text-center' },
                    {
                        'data': 'RoleLevel', 'title': 'Head Office', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 1 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'Division', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 2 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'ITI Institute', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 3 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'ITOT', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 4 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'STARC', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 5 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'Public', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 6 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleLevel', 'title': 'Exam Centers', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleDataGrid(nTd, oData.lstRoleLevel, iCol);
                            //$(nTd).html("<input type='checkbox' id='cbxHeadOffice' " + (oData.RoleLevel == 7 ? 'checked' : '') + " />");
                        }
                    },
                    {
                        'data': 'RoleId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='fnEditAddRoles(\"" + oData.RoleName.replace(" ", "").trim().toUpperCase() + "\", this.value)' class='btn btn-primary btn-xs' value='Edit' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='fnDeleteBtnClick(\"" + oData.RoleName.replace(" ", "").trim().toUpperCase() + "\", this.value, \"Role\")' class='btn btn-danger btn-xs' value='Delete' id='Delete'/>");
                        }
                    }
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnGetRoleMapData(id) {
    var val = 0;
    if (id != '') {
        val = $("#" + id).val();
    }

    $.ajax({
        type: "GET",
        data: { val: val },
        url: "/Admin/GetRoleMapData",
        contentType: "application/json",
        success: function (data) {
            $('#tblRoleMapping').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'MenuName', 'title': 'Module/Menu', 'className': 'text-center' },
                    { 'data': 'SubMenuName', 'title': 'Submenu', 'className': 'text-center' },
                    { 'data': 'SubSubMenuName', 'title': 'Sub-Submenu', 'className': 'text-center' },
                    {
                        'data': 'Create', 'title': 'Create', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='checkbox' id='cbxCreate' />");
                        }
                    },
                    {
                        'data': 'Edit', 'title': 'Edit', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='checkbox' id='cbxEdit' />");
                        }
                    },
                    {
                        'data': 'Delete', 'title': 'Delete', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='checkbox' id='cbxDelete' />");
                        }
                    },
                    {
                        'data': 'View', 'title': 'View', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='checkbox' id='cbxView' />");
                        }
                    },
                    { 'data': 'MenuId', 'title': '', 'className': 'text-center', 'visible': false },
                    { 'data': 'SubMenuId', 'title': '', 'className': 'text-center', 'visible': false }
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnGetRoleMapDetailsData(id) {

    if (id != 0) {
        var obj = {
            UnitId: $("#ddlUnits1").val(),
            WingId: $("#ddlWing1").val(),
            SectionId: $("#ddlSection1").val(),
            RoleId: $("#ddlRole1").val(),
        };
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify({ clsRole: obj }),
        url: "/Admin/GetRoleMapDetailsData",
        contentType: "application/json",
        success: function (data) {
            $('#tblRoleMappingDetails').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'UnitName', 'title': 'Unit', 'className': 'text-center' },
                    //{ 'data': 'LocationDivName', 'title': 'Location', 'className': 'text-center' },
                    { 'data': 'RoleName', 'title': 'Role', 'className': 'text-center' },
                    { 'data': 'lstMenuList[0].MenuName', 'title': 'Module/Menu', 'className': 'text-center' },
                    { 'data': 'lstMenuList[0].SubMenuName', 'title': 'Submenu', 'className': 'text-center' },
                    { 'data': 'lstMenuList[0].SubSubMenuName', 'title': 'Sub-Submenu', 'className': 'text-center' },
                    {
                        'data': 'lstMenuList[0].Create', 'title': 'Create', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleMappingDataGrid(nTd, oData.lstMenuList[0].Create, iCol)

                        }
                    },
                    {
                        'data': 'lstMenuList[0].Edit', 'title': 'Edit', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleMappingDataGrid(nTd, oData.lstMenuList[0].Edit, iCol)
                        }
                    },
                    {
                        'data': 'lstMenuList[0].Delete', 'title': 'Delete', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleMappingDataGrid(nTd, oData.lstMenuList[0].Delete, iCol)
                        }
                    },
                    {
                        'data': 'lstMenuList[0].View', 'title': 'View', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            fnUpdateRoleMappingDataGrid(nTd, oData.lstMenuList[0].View, iCol)
                        }
                    },
                    //{
                    //    'data': 'IsActive', 'title': 'Status', 'className': 'text-center',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        var stat = "InActive";
                    //        if (oData.IsActive == true) {
                    //            stat = "Active";
                    //        }
                    //        $(nTd).html(stat);
                    //    }
                    //},
                    {
                        'data': 'IsActive', 'title': 'Status', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.rmId + ", " + 0 + ", " + 0 + ",\"" + "Role Mapping" + "\"," + oData.IsActive + ")' class='btn btn-xs btn-primary' " + (oData.IsActive == true ? " disabled" : "") + " value='Active' />" +
                                "<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.rmId + ", " + 0 + ", " + 0 + ",\"" + "Role Mapping" + "\"," + oData.IsActive + ")' class = 'btn btn-xs btn-danger'" + (oData.IsActive == false ? " disabled" : "") + " value = 'DeActive' /> ");
                        }
                    },
                    //{
                    //    'data': 'rmId',
                    //    'title': 'Action',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        $(nTd).html("<input type='button' onclick='fnEditRoleMapping(" + oData.rmId + ", this.value)' class='btn btn-primary btn-xs' value='Active' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='fnDeleteBtnClick(" + oData.rmId + ", this.value, \"Role Mapping\")' class='btn btn-danger btn-xs' value='DeActive' id='Delete'/>");
                    //    }
                    //}
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnGetUserDataFromKGIDNumber() {
    if ($("#txtKGIDUserId").val() != '') {
        $.ajax({
            type: "GET",
            data: { KGIDNumber: $("#txtKGIDUserId").val() },
            url: "/Home/GetUserDataByKGIDNumber",
            contentType: "application/json",
            success: function (data) {
                
                if (data[0].KGIDUserID != null && data.length > 0 && data[0].message == "0") {
                    $("#txtKGIDUserId").val(data[0].KGIDUserID);
                    $("#txtEEName").val(data[0].EEName);
                    $("#dateofbirth").val(data[0].DateOfBirth);
                    $("#txtPhoneNum").val(data[0].phoneNum);
                    $("#txtEmail").val(data[0].email);
                    $("#txtPwd").val(data[0].pwd);
                } else if (data[0].message != "1" ) {
                    bootbox.alert("User doesn't exist in our database.");
                }
                else if (data[0].message != "2") {
                    bootbox.alert("User is deactivated,please activate for user mapping!!");

                }
            }
        });
    }
}

function fnGetUserMapData() {
    $.ajax({
        type: "GET",
        url: "/Admin/GetUserMappingData",
        contentType: "application/json",
        success: function (data) {
            $('#tblUserMapping').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'KGIDUserID', 'title': 'KGID Number/User ID', 'className': 'text-center' },
                    { 'data': 'lstRoleMapping[0].WingName', 'title': 'Wing', 'className': 'text-center' },
                    { 'data': 'EEName', 'title': 'Name of the User', 'className': 'text-center' },
                    { 'data': 'phoneNum', 'title': 'Phone Number', 'className': 'text-center' },
                    { 'data': 'email', 'title': 'E-mail ID', 'className': 'text-center' },
                    { 'data': 'lstRoleMapping[0].UnitName', 'title': 'Unit', 'className': 'text-center' },
                    { 'data': 'lstRoleMapping[0].LocationDivName', 'title': 'Location', 'className': 'text-center' },
                    { 'data': 'lstRoleMapping[0].SectionName', 'title': 'Section', 'className': 'text-center' },
                    { 'data': 'lstRoleMapping[0].RoleName', 'title': 'Role', 'className': 'text-center' },
                    {
                        'data': 'IsActive', 'title': 'Status', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            var stat = "InActive";
                            if (oData.IsActive == true) {
                                stat = "Active";
                            }
                            $(nTd).html(stat);
                        }
                    },
                    {
                        'data': 'IsActive', 'title': 'Status', 'className': 'text-center',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {

                            $(nTd).html("<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.userId + ", " + oData.lstRoleMapping[0].RoleId + ", " + oData.lstRoleMapping[0].SectionId + ",\"" + "User Mapping" + "\"," + oData.IsActive + ")' class='btn btn-xs btn-primary' " + (oData.IsActive == true ? " disabled" : "") + " value='Active' />" +
                                "<input type = 'button' onclick = 'fnSetActiveDeActiveStatus(" + oData.userId + ", " + oData.lstRoleMapping[0].RoleId + ", " + oData.lstRoleMapping[0].SectionId + ",\"" + "User Mapping" + "\"," + oData.IsActive + ")' class = 'btn btn-xs btn-danger'" + (oData.IsActive == false ? " disabled" : "") + " value = 'DeActive' /> ");
                        }
                    },
                    //{
                    //    'data': 'userId',
                    //    'title': 'Action',
                    //    "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                    //        $(nTd).html("<input type='button' onclick='fnEditUserMapping(" + oData.userId + ", " + oData.userMapId + ", this.value)' class='btn btn-primary btn-xs' value='Active' id='edit'/>&nbsp;&nbsp;<input type='button' onclick='fnDeleteBtnClick(" + oData.userId + ", this.value, \"User Mapping\")' class='btn btn-danger btn-xs' value='DeActive' id='Delete'/>");
                    //    }
                    //}
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnUpdateRoleDataGrid(nTd, oData, col) {
    if (oData.length > 0) {
        $.each(oData, function (index, item) {
            if (oData.includes(col)) {
                //if (oData[index] == col || index >= col) {
                $(nTd).html("<input type='checkbox' checked readonly />");
            }
            else {
                $(nTd).html("-");
            }
        });

    }
}


function fnUpdateRoleMappingDataGrid(nTd, oData, col) {
    if (oData == true) {
        //if (oData[index] == col || index >= col) {
        $(nTd).html("<input type='checkbox' checked disabled='disabled' />");
    }
    else {
        $(nTd).html("-");
    }
}



function fnRoleMapping() {

    fnClearAllErrMsgs();
    var IsValid = true;

    if ($('#ddlUnits').val() == 0) {
        $("#ddlUnits-Required").show();
        IsValid = false;
    }
    if ($('#ddlWing').val() == 0 && $("#ddlWing").is(':visible')) {
        $("#ddlWing-Required").show();
        IsValid = false;
    }
    if ($('#ddlSection').val() == 0) {
        $("#ddlSection-Required").show();
        IsValid = false;
    }
    if ($('#ddlRole').val() == 0) {
        $("#ddlRole-Required").show();
        IsValid = false;
    }

    if (IsValid) {
        $('#tblRoleMapping').DataTable().destroy();
        $('#tblRoleMapping').DataTable({ "paging": false }).draw(true);
        $('#tblRoleMapping').DataTable().column(7).visible(false);
        $('#tblRoleMapping').DataTable().column(8).visible(false);
        var listItem = [];
        var sendObj = [];
        var tblRoleMap = $("#tblRoleMapping tbody");
        var i = 0;
        tblRoleMap.find('tr').each(function (len) {
            var $tr = $(this);
            var chkbx = $tr.find("td input[type=checkbox]").is(":checked");
            var cbxCreate = $tr.find("td:eq(3) input[type=checkbox]").is(":checked");
            var cbxEdit = $tr.find("td:eq(4) input[type=checkbox]").is(":checked");
            var cbxDelete = $tr.find("td:eq(5) input[type=checkbox]").is(":checked");
            var cbxView = $tr.find("td:eq(6) input[type=checkbox]").is(":checked");
            lstMenu = {
                MenuId: $('#tblRoleMapping').DataTable().column(7).data()[i],
                SubMenuId: $('#tblRoleMapping').DataTable().column(8).data()[i],
                Create: cbxCreate,
                Edit: cbxEdit,
                Delete: cbxDelete,
                View: cbxView,
            }
            if (chkbx == true) {
                listItem.push(lstMenu);
            }
            i = i + 1
        });
        $('#tblRoleMapping').DataTable().destroy();
        $('#tblRoleMapping').DataTable({ "paging": true }).draw(true);
        $('#tblRoleMapping').DataTable().column(7).visible(false);
        $('#tblRoleMapping').DataTable().column(8).visible(false);
        if (listItem.length == 0) {
            bootbox.alert("<br>No Module has been selected for mapping. Please select Menu/SubMenu with " +
                "Create/Edit/Update/Delete options to proceed.");
        }
        else {
            var obj = {
                UnitId: $("#ddlUnits").val(),
                LocationDivId: $("#ddlLocation").val(),
                WingId: $("#ddlWing").val(),
                SectionId: $("#ddlSection").val(),
                RoleId: $("#ddlRole").val(),
                lstMenuList: listItem
            };
            sendObj.push(obj);

            bootbox.confirm({
                message: "<br><br>Are you sure you want to Add Role Mapping ?",
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
                callback: function (confirmed) {
                    if (confirmed) {
                        $.ajax({
                            type: "POST",
                            data: JSON.stringify({ lstRoleMapping: sendObj }),
                            url: "/Admin/GetRoleMapData",
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                if (data != null || data != '') {
                                    if (data == "Success") {
                                        bootbox.alert("<br><br>Role Mapping Added Successfully!!");
                                        fnGetRoleMapData('');
                                    }
                                    else {
                                        bootbox.alert("<br><br>failed");
                                    }
                                }
                            }, error: function (result) {
                                bootbox.alert("<br><br>Error", "something went wrong");
                            }
                        });
                        fnClearRoleMapping();
                    }
                }
            });
        }
    }
}

function fnEditRoleMapping(id, val) {
    $.ajax({
        type: "GET",
        data: { id: id, value: val },
        url: "/Admin/GetRoleMappingDataById",
        contentType: "application/json",
        success: function (data) {
            if (data != null || data != '') {
                if (data[0].message == "Success") {
                    bootbox.alert("<br><br>Role Mapping Updated Successfully!!");
                    fnGetRoleMapDetailsData(0);
                    return
                }

                //$('.nav a[href="#tab_2"]').tab('show');
                //$('.nav a[href="#tab_2"]').trigger('click');
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}


function fnEditUserMapping(id, mapId, val) {
    userId = id;
    userMapId = mapId;

    $.ajax({
        type: "GET",
        data: { id: id, value: val },
        url: "/Admin/GetUserMappingDataById",
        contentType: "application/json",
        success: function (data) {
            if (data != null || data != '') {
                if (data[0].message == "Success") {
                    bootbox.alert("<br><br>User Mapping Deleted Successfully!!");
                    fnGetUserMapData();
                    fnClearUserMapping();
                    return
                }
                $("#txtKGIDUserId").val(data[0].KGIDUserID);
                $("#txtEEName").val(data[0].EEName);
                $("#dateofbirth").val(data[0].DateOfBirth);
                $("#txtPhoneNum").val(data[0].phoneNum);
                $("#txtEmail").val(data[0].email);
                $("#txtPwd").val(data[0].pwd);
                $("#ddlUnits2").val(data[0].lstRoleMapping[0].UnitId);
                $("#ddlLocation2").val(data[0].lstRoleMapping[0].LocationDivId);
                $("#ddlSection2").val(data[0].lstRoleMapping[0].SectionId);
                $("#ddlRole2").val(data[0].lstRoleMapping[0].RoleId);
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnEditAddRoles(id, val) {
    $.ajax({
        type: "GET",
        data: { userId: id, value: val },
        url: "/Admin/GetRolesDataById",
        contentType: "application/json",
        success: function (data) {
            if (data != null || data != '') {
                if (data.res[0].message == "Success") {
                    bootbox.alert("<br><br>Role Deleted Successfully!!");
                    fnGetRolesData();
                    return
                }
                $("#txtRoleName").val(data.res[0].RoleName);
                $("#ddlUnitSelect").next().hide();
                $('#ddlUnitSelect').after('<div class="col-md-8"><select class= "form-control clear" id = "ddlUnitSelect1" multiple /><small class="text-danger" id="ddlUnitSelect1-Required"></small></div>');
                $("#ddlUnitSelect1").next().show();
                if (data.res1.length > 0) {
                    $.each(data.res1, function (index, item) {
                        $("#ddlUnitSelect1").append($("<option/>").val(this.Value).text(this.Text));
                    });
                }

                $.each(data.res[0].lstRoleLevel, function (i, e) {
                    $("#ddlUnitSelect1 option[value='" + e + "']").prop("selected", true);
                    //$("#ddlUnitSelect option[value='1']").prop("selected", true);
                    //$("#ddlUnitSelect option[value='" + this.val + "']").prop("selected", true);
                    //$("#ddlDistrictJD").append($("<option/>").val(this.district_id).text(this.district_ename));
                });
                $("#ddlUnitSelect1").multiselect({});
                //$.each(data[0].lstRoleLevel, function (i, e) {
                //    $("#ddlUnitSelect option[value='" + e + "']").prop("selected", true);
                //});
                //}
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnEditHolidayList(id, val) {
    if (id == "")
    {
        $("#btn_addholiday").val('');
        $("#btn_addholiday").val('Add');
    }
    else {
        $("#exampleHoliday").text('');
        $("#btn_addholiday").val('');
        $("#exampleHoliday").text('Edit Holiday List');
        $("#btn_addholiday").val('Update');
    }

    $.ajax({
        type: "GET",
        data: { id: id, value: val },
        url: "/Admin/GetHolidayList",
        contentType: "application/json",
        success: function (data) {
            if (data != null || data != '') {
                if (data[0].message == "Success") {
                    bootbox.alert("<br><br>Holiday Deleted Successfully!!");
                    fnShowHolidayList(0, '');
                    return;
                }
                else {
                    var dt = data[0].HolidayDate.split('-');
                    var date = dt[2] + "-" + dt[1].padStart(2, '0') + "-" + dt[0].padStart(2, '0');
                    $("#txtHolidaydate").val(date);
                    $("#hdnHolidayId").val(data[0].HolidayId);
                    $("#txtHolidayDay").val(data[0].HolidayDay);
                    $("#txtHolidayName").val(data[0].HolidayName);
                    //if (data[0].HolidayId == null) { $("#btn_addholiday").text('Add'); } else { $("#btn_addholiday").text('Update');}
                    
                    $('#addHolidayList').modal('toggle');
                    
                }
            }
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}
function AddUserPostData() {
    $("#BtnAddUser").prop('disabled', true);
    var _forms = $("#UserAddform").valid();
    //Serialize the form datas.
    if (_forms == true) {

        var UserProfileData = $("#UserAddform").serialize();
        //to get alert popup
        //$.ajax({
        //    url: '/Admin/AddUserPostsavedata',
        //    type: "POST",
        //    dataType: 'json',
        //    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        //    data: UserProfileData,
        //    success: function (data) {
        //        if (data == "Success") {
        //            $('#addStaffModal').modal('hide');
        //            var _msg = "";
        //            _msg = "User Details Added successfully!!";
        //            bootbox.alert(_msg, function () {
        //                $("#UserAddform").trigger("reset");
        //                $("#BtnAddUser").prop('disabled', false);
        //                window.location.href = "/Admin/AdminIndex";
        //            });
        //        }
        //        else {
        //            bootbox.alert("Failed", function () {
        //            });
        //        }
        //    },
        //    error: function (e) {
        //        var _msg = "Something went wrong.";
        //        bootbox.alert(_msg);
        //    }
        //});

        $.ajax({
            type: "POST",
            data: UserProfileData,
            url: "/Admin/AddUserPostsavedata",
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (data) {
                console.log(data);
                if (data == "Success") {
                    $("#UserAddform").trigger("reset");
                    $('#addStaffModal').modal('hide');
                    var _msg = "";
                    _msg = "User Details Added successfully!!";
                    bootbox.alert(_msg, function () {
                        $("#UserAddform").trigger("reset");
                        $("#BtnAddUser").prop('disabled', false);
                        window.location.href = "/Admin/AdminIndex";
                    });
                }
                else {
                    bootbox.alert("Failed", function () {
                    });
                }
            },
            error: function (e) {
                var _msg = "Something went wrong.";
                bootbox.alert(_msg);
            }
        });

    }
    else {
        $("#BtnAddUser").prop('disabled', false);
        bootbox.alert("Error", "Unble to process data!!");
    }
}



function fnClearAddRoles() {
    $("#txtRoleName").val('');
    $('#ddlUnitSelect').val('').multiselect('refresh');
    $('#ddlUnitSelect1').val('').multiselect('refresh');
    //$('#ddlUnitSelect option').each(function (index, option) {
    //    $(option).remove();
    //});

    //$("#ddlUnitSelect option:selected").removeAttr("selected");
    //$("#ddlUnitSelect option: selected").prop("selected", false);
    //$("#ddlUnitSelect").multiselect("clearSelection");
    //var ddlUnitSelectId = "ddlUnitSelect";
    //if ($('#' + ddlUnitSelectId).is(':hidden')) {
    //    ddlUnitSelectId = ddlUnitSelectId + "";
    //}
    //$('#' + ddlUnitSelectId).val('').multiselect('refresh');
    //$("#ddlUnitSelect").next().show();
}

function fnClearRoleMapping() {
    $("#ddlUnits").val(0);
    $("#ddlWing").val(0);
    $("#ddlSection").val(0);
    $("#ddlRole").val(0);
}

function fnClearRoleMappingDetails() {
    $("#ddlUnits1").val(0);
    $("#ddlWing1").val(0);
    $("#ddlSection1").val(0);
    $("#ddlRole1").val(0);
}

function fnClearUserMapping() {
    $("#txtKGIDUserId").val('');
    $("#txtEEName").val('');
    $("#dateofbirth").val('');
    $("#txtPhoneNum").val('');
    $("#txtEmail").val('');
    $("#txtPwd").val('');
    $("#ddlUnits2").val(0);
    $("#ddlWing2").val(0);
    $("#ddlLocation2").val('choose');
    $("#ddlDistrict2").val('choose');
    $("#ddlTaluk2").val('choose');
    $("#ddlITIName2").val('choose');
    $("#ddlSection2").val(0);
    $("#ddlRole2").val(0);
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#ImgPhotoUpload').attr('src', e.target.result).width(50).height(50);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function fnDeleteBtnClick(id, val, text) {
    bootbox.confirm({
        message: "<br><br>Are you sure you want to Delete '" + text + "' ?",
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
        callback: function (confirmed) {
            if (confirmed) {
                if (text == "Role")
                    fnEditAddRoles(id, val);
                if (text == "Role Mapping")
                    fnEditRoleMapping(id, val);
                if (text == "User Mapping")
                    fnEditUserMapping(id, '', val);
                if (text == "Holiday")
                    fnEditHolidayList(id, val);
            }
        }
    });
};

function UpdateLocation(id, id1, id2) {
    if (id != 0) {
        if (id.includes("ddlUnits")) {
            fnGetRolesList(id, id1, id2);
            if (id.includes("ddlUnits2") && id1.includes("ddlLocation2"))
                fnGetSectionsList(id, "ddlSection2");
            if (!id1.includes("ddlSection2")) {
                $(".clsITIDetails").hide();
                $(".clsWing").hide();
                if ($("#" + id).val() == 1) {
                    $("#" + id1).val(0);
                    $(".clsWing").show();
                }
                if (id.includes("ddlUnits2") && !($("#" + id).val() == 2 || $("#" + id).val() == 3)) {
                    $("#" + id1).val(1);
                    $("#lblLocation").text('Location');
                    $("#" + id1 + " option[value!='1']").hide();
                } else if (id.includes("ddlUnits2")) {
                    $("#" + id1).val('choose');
                    if ($("#" + id).val() == 3) {
                        $(".clsITIDetails").show();
                    }
                    $("#lblLocation").text('Division');
                    $("#" + id1 + " option[value!='1']").show();
                }
            }
        }
        else if (id.includes("ddlWing")) {
            $("#" + id1 + " option[value!='76']").show();
            if ($("#" + id).val() == 1) {
                $("#" + id1 + " option[value!='76']").hide();
            }
        }
        else {
            if ($("#" + id).val().toLowerCase() != "choose") {
                if (id.includes('ddlLocation2')) {
                    GetDistrictsDDp(id1, $("#" + id).val());
                }
                if (id.includes('ddlDistrict2')) {
                    GetTaluksDDp(id1, $("#" + id).val());
                }
                if (id.includes('ddlTaluk2')) {
                    GetColleges(id1, $("#" + id).val());
                }
            }
        }
    }
}

function fnShowHideOnLoad() {
    $(".clsITIDetails").hide();
    $(".clsWing").hide();
}

function fnSetActiveDeActiveStatus(usrId, roleId, sectionId, usrName, stat) {
    bootbox.confirm({
        message: "<br>Are you sure you want to " + (stat == false ? "Activate" : "Deactivate") + " '<b>" + usrName + "</b > ' ?",
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
        callback: function (confirmed) {
            if (confirmed) {
                $.ajax({
                    url: "/Admin/UpdateActiveInActiveStatus",
                    type: 'POST',
                    data: {
                        clsRole: { UnitId: usrId, SectionId: sectionId, RoleId: roleId }, userName: usrName, status: stat
                    },
                    success: function (data) {
                        if (data == "Success") {
                            if (!(usrName == "Role Mapping" || usrName == "User Mapping")) {
                                bootbox.alert("User '<b>" + usrName + "</b>' " + (stat == false ? "Activated" : "Deactivated") + " Successfully!!");
                                fnGetUserData();
                            }
                            else if (usrName == "Role Mapping") {
                                bootbox.alert(usrName + " " + (stat == false ? "Activated" : "Deactivated") + " Successfully!!");
                                fnGetRoleMapDetailsData(0);
                            }
                            else {
                                bootbox.alert(usrName + " " + (stat == false ? "Activated" : "Deactivated") + " Successfully!!");
                                fnGetUserMapData();
                            }
                        }
                        else {
                            bootbox.alert("failed");
                        }
                    }, error: function (result) {
                        bootbox.alert("Error", "something went wrong");
                    }
                });
            }
        }
    });
}


function fnClearAllErrMsgs() {

    //tab2
    $("#txtRoleName-Required").hide();
    $("#txtUnit-Required").hide();

    //tab3

    $("#ddlUnits-Required").hide();
    $("#ddlWing-Required").hide();
    $("#ddlSection-Required").hide();
    $("#ddlRole-Required").hide();


    //tab4

    //tab5
    $("#txtKGIDUserId-Required").hide();
    $("#ddlUnits2-Required").hide();
    $("#ddlWing2-Required").hide();
    $("#ddlLocation2-Required").hide();
    $("#ddlDistrict2-Required").hide();
    $("#ddlTaluk2-Required").hide();
    $("#ddlITIName2-Required").hide();
    $("#ddlSection2-Required").hide();
    $("#ddlRole2-Required").hide();
    //tab6
}


function fnUpdtHolidayList(hId) {
    $("#btn_addholiday").prop('disabled', true);

    
    var IsValid = true;
    if ($("#txtHolidayName").val() == '') {
        $("#txtHolidayName-Required").show();
        IsValid = false;
    }
    if ($("#txtHolidayDay").val() == '') {
        $("#txtHolidayDay-Required").show();
        IsValid = false;
    }

    if ($("#txtHolidaydate").val() == '') {
        $("#txtHolidaydate-Required").show();
        IsValid = false;
    }
    if ($("#hdnHolidayId").val() != '')
        hId = $("#hdnHolidayId").val();
    if (IsValid) {
        var obj = {
            HolidayId: hId,
            HolidayDate: $("#txtHolidaydate").val(),
            HolidayDay: $("#txtHolidayDay").val(),
            HolidayName: $("#txtHolidayName").val(),
        };

        if (hId == 0) {
            $("#btn_addholiday").val('');
            $("#btn_addholiday").text('Add');
        }
        else {
            $("#btn_addholiday").val('');

            $("#btn_addholiday").val('Update');
        }

        bootbox.confirm({
            message: "<br><br>Are you sure you want to " + $("#btn_addholiday").text() + " Date as Holiday?",
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
            callback: function (confirmed) {
                if (confirmed) {
                    $.ajax({
                        url: "/Admin/GetHolidayList",
                        type: 'POST',
                        data: JSON.stringify({ clsHoliday: obj }),
                        contentType: "application/json",
                        success: function (data) {

                            $("#hdnHolidayId").val('');
                            $("#addHolidayList").removeClass("in");
                            $(".modal-backdrop").remove();
                            $('body').removeClass('modal-open');
                            $('body').css('padding-right', '');
                            $("#addHolidayList").hide();
                            if (data == "Success") {
                                $("#btn_addholiday").prop('disabled', false);

                                bootbox.alert("Data added Successfully!!");
                            }
                            else if (data == "Updated") {
                                $("#btn_addholiday").prop('disabled', false);

                                bootbox.alert("Data Updated Successfully!!");
                            }
                            else if (data == "Exists") {
                                $("#txtHolidayName-Required").html('');
                                $("#txtHolidaydate-Required").html('');
                                $("#txtHolidayDay-Required").html('');
                                $("#txtHolidaydate").val('');
                                $("#txtHolidayDay").val('');
                                $("#txtHolidayName").val('');
                                $("#btn_addholiday").prop('disabled', false);

                                bootbox.alert("Date Already Exists!!. Please select a different date.");
                            }
                            else {
                                $("#btn_addholiday").prop('disabled', false);

                                bootbox.alert("failed");
                            }
                            //$('#addHolidayList').modal('toggle');
                            $("#txtHolidaydate").val('');
                            $("#txtHolidayDay").val('');
                            $("#txtHolidayName").val('');
                            $("#txtHolidayName-Required").html('');
                            $("#txtHolidaydate-Required").html('');
                            $("#txtHolidayDay-Required").html('');

                            fnShowHolidayList(0, '');
                        }, error: function (result) {
                            $("#btn_addholiday").prop('disabled', false);

                            bootbox.alert("Error", "something went wrong");
                        }
                    });
                }
            }
        });
    }
    else {
        $("#btn_addholiday").prop('disabled', false);

        bootbox.alert("please fill mandatory data");
    }

}

function fnShowHolidayList(id, val) {
    $.ajax({
        type: "GET",
        url: "/Admin/GetHolidayList",
        data: { id: id, value: val },
        contentType: "application/json",
        success: function (data) {
            $('#tblHolidayList').DataTable({
                data: data,
                "destroy": true,
                "bSort": true,
                columns: [
                    { 'data': 'sino', 'title': 'SI No.', 'className': 'text-center' },
                    { 'data': 'HolidayDate', 'title': 'Date', 'className': 'text-center' },
                    { 'data': 'HolidayDay', 'title': 'Day', 'className': 'text-center' },
                    { 'data': 'HolidayName', 'title': 'Name', 'className': 'text-center' },
                    {
                        'data': 'HolidayId',
                        'title': 'Action',
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $(nTd).html("<input type='button' onclick='fnEditHolidayList(" + oData.HolidayId + ", this.value)' class='btn btn-primary btn-xs' value='Edit'/>&nbsp;&nbsp;<input type='button' onclick='fnDeleteBtnClick(\"" + oData.HolidayId + "\", this.value, \"Holiday\")' class='btn btn-danger btn-xs' value='Delete' />");
                        }
                    }
                ]
            });
        }, error: function (result) {
            bootbox.alert("<br><br>Error", "something went wrong");
        }
    });
}

function fnShowPhotos() {

}