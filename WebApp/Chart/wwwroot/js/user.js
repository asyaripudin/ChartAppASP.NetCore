function getUser() {
    var row = '';
    var counter = 1;

    $.ajax({
        url: '/Admin/GetUser/',
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, val) {
                row += '<tr><td style="text-align:center; font-size:12px">' + counter +
                    '<td style="text-align:center; font-size:12px;">' + val.userName + '</td>' +
                    '<td style="text-align:center; font-size:12px;">' + val.password + '</td>' +
                    '<td style="text-align:center;font-size:12px;">' + val.userType + '</td>' +
                    '<td style="text-align:center;font-size:12px;">' + val.isActive + '</td>' +
                    '<td style="text-align:center; font-size:12px;" class="noExl"><a href="#" onclick=getbyUserId("' + val.userID + '") data-bs-toggle="modal"' +
                    'data-bs-target="#UpdateModal"> Edit</a> | <a href="#" onclick=Delete("' + val.userID + '") > Delete</a ></td >' +
                    '</td></tr>';
                counter++;
            });

            $("#tableUser").append(row);
            $(document).ready(function () {
                $.noConflict();
                $('#tableUser').DataTable({

                    pageLength: 5,
                    paging: true,
                    filter: true,
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    dom: "fltip",
                    lengthMenu: [[5, 10, 100, -1], [5, 10, 100, 'All']]
                });
            })
        }
    });
}

function getUserID() {
    $('#txtUserID').css('border-color', 'lightgrey');
    $('#txtUserName').css('border-color', 'lightgrey');
    $('#txtPassword').css('border-color', 'lightgrey');
    
    $(document).ready(function () {
        $.getJSON("/Admin/GetUserID/",
            function (data) {
                $('#txtUserID').val(data);
                $('#myModal').modal('show');
                //$('#btnUpdate').hide();
                $('#btnAdd').show();
            });
    })
    return false;
}
function getRadio() {
    $(document).ready(function () {
        var radioValue = $("input[name='usertype']:checked").val();
        if (radioValue) {
            $("#hRadioBtn").val(radioValue);
        }
    });
}

function Add()
{
    var user_name = $("#txtUserName").val();
    var pwd = $("#txtPassword").val();
    var radioValue = $("input[name='usertype']:checked").val();;
    if (user_name == '' && pwd == '' && radioValue == '') {
        swal({
            title: "Warning!",
            text: "Username, Password and User Type is required.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    }
    else if (user_name == '') {
        swal({
            title: "Warning!",
            text: "Username is required.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    } else if (pwd == '') {
        swal({
            title: "Warning!",
            text: "Status is required.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    } else if (radioValue == '') {
        swal({
            title: "Warning!",
            text: "Paassword is required.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    } else {
        var UsersObj = {
            userName: $("#txtUserName").val(),
            password: $("#txtPassword").val(),
            userType: $("#hRadioBtn").val(),
            isActive: '1'           
           
        };
        $.ajax({
            url: "/Admin/Add/",
            data: JSON.stringify(UsersObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (UsersObj) {
                if (UsersObj != null) {
                    swal({
                        title: "Save success!",
                        text: "Your data has been saved.",
                        type: "success",
                        timer: 3000
                    },
                        function () {
                            location.reload();
                        }
                    );
                }
                else {
                    swal({
                        title: "Save Failed!",
                        text: "Your data has not been saved.",
                        type: "success",
                        timer: 3000
                    },
                        function () {
                            location.reload();
                        }
                    );
                }

            },

        });
    }
}

function getRadioUpdate() {
    $('input[id="rdoAdmin"]').on('change', function () {
        value = "Admin";
        $('#hRadioBtnUpdate').val(value);
    });
    $('input[id="rdoStaff"]').on('change', function () {
        value = "Staff";
        $('#hRadioBtnUpdate').val(value);
    });
}

function getRadioStatusUpdate() {
    $('input[id="rdoActive"]').on('change', function () {
        value = "Active";
        $('#hRadioBtnStatusUpdate').val(value);
    });
    $('input[id="rdoNonActive"]').on('change', function () {
        value = "NonActive";
        $('#hRadioBtnStatusUpdate').val(value);
    });
}
function getbyUserId(userID)
{
     $("document").ready(function () {
        $.ajax({
            url: "/Admin/Get/" + userID,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (data) {
                $('#txtUserIDUpdate').val(data.userID);
                $('#txtUserNameUpdate').val(data.userName);
                $('#txtPasswordUpdate').val(data.password);
                $('#hRadioBtnUpdate').val(data.userType);
                if (data.userType == 'Admin') {
                    $("input[name='usertype'][value='Admin']").attr("checked", true);
                }
                else
                {
                    $("input[name='usertype'][value='Staff']").attr("checked", true);
                }
                $('#hRadioBtnStatusUpdate').val(data.isActive);
                if (data.isActive == 'True') {
                    $("input[name='status'][value='True']").attr("checked", true);
                }
                else {
                    $("input[name='status'][value='False']").attr("checked", true);
                }           
                $('#UpdateModal').modal('show');
                $('#btnUpdate').show();
                $('#btnAdd').hide();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
        return false;
    })
}

function Update(userID) {
    userID = $('#txtUserIDUpdate').val();
    var userObj = {
        userID: $('#txtUserIDUpdate').val(),
        userName: $('#txtUserNameUpdate').val(),
        password: $('#txtPasswordUpdate').val(),
        userType: $("#hRadioBtnUpdate").val(),
        isActive: $("#hRadioBtnStatusUpdate").val()
    };
    $.ajax({
        url: "/Admin/Update/" + userID,
        data: JSON.stringify(userObj),
        type: "PUT",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (userObj) {
            if (userObj != null) {
                swal({
                    title: "Update success!",
                    text: "Your data has been updated.",
                    type: "success",
                    timer: 3000
                },
                    function () {
                        location.reload();
                    }
                );
            }
            else {
                swal({
                    title: "Update Failed!",
                    text: "Your data has not been updated.",
                    type: "warning",
                    timer: 3000
                },
                    function () {
                        location.reload();
                    }
                );
            }
            $('#UpdateModal').modal('hide');

        },
    });

}

function Delete(userID) {
    var cfm = confirm("Are you sure you want to delete this Record?");
    if (cfm) {
        $.ajax({
            url: "/Admin/Delete/" + userID,
            type: "PUT",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            error: function (data) {
                if (data.responseText != "") {
                    swal({
                        title: data.responseText,
                        text: "Your data has been deleted.",
                        type: "success",
                        timer: 3000
                    },
                        function () {
                            location.reload();
                        }
                    );

                }
            }
        });
    }
}
