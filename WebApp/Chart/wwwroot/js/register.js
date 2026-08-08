function Register() {
    var user_name = $("#username").val();
    var pwd = $("#pass").val();
    if (user_name == '' && pwd == '') {
        swal({
            title: "Warning!",
            text: "Username and password is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    }
    else if (user_name == '') {
        swal({
            title: "Warning!",
            text: "Username is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    } else if (pwd == '') {
        swal({
            title: "Warning!",
            text: "Paassword is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#pass").css("background-color", "#fff5e6");
    } else {
      var UserObj = {
            username: $("#username").val(),
            password: $("#pass").val()
        };
        $.ajax({
            url: "/Home/Register/",
            data: JSON.stringify(UserObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            error: function (data) {
                if (data.responseText == "Register Successfully...") {
                    swal({
                        title: data.responseText,
                        text: "Your data has been saved.",
                        type: "success",
                        timer: 5000
                    },
                        function () {
                            location.reload();
                        }
                    );
                }
                else if (data.responseText == "UserName Allready Exist...") {
                    swal({
                        title: data.responseText,
                        text: "Please Change Username, your data has not been saved.",
                        type: "warning",
                        timer: 5000
                    },
                       
                    );
                }
                               
            },
          
        });
    }
}
function Update()
{
    var username = $("#txtUserName").val();
    var newpassword = $("#txtNewPassword").val();

    if (username == '' && password == '' && newpassword == '') {
        swal({
            title: "Warning!",
            text: "Username and New Password is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#newpassword").css("background-color", "#fff5e6");
    }
    else if (username == '') {
        swal({
            title: "Warning!",
            text: "Username is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#newpassword").css("background-color", "#fff5e6");
    } else if (newpassword == '') {
        swal({
            title: "Warning!",
            text: "New Password is empty.",
            type: "warning",
            timer: 3000
        });
        $("#username").css("background-color", "#fff5e6");
        $("#newpassword").css("background-color", "#fff5e6");
    }
    else
    {
        var UserObj = {
            username: $("#txtUserName").val(),
            newpassword: $("#txtNewPassword").val()
        };
        $.ajax({
            url: "/Home/ChangePassword/",
            data: JSON.stringify(UserObj),
            type: "PUT",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (UserObj) {
                if (UserObj != null) {
                    swal({
                        title: "Change Password success!",
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
                        title: "Change Password Failed!",
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
}
