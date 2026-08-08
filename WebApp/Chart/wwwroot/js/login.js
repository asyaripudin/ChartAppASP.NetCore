function Login(username, password) {
    var username = $("#username").val();
    var password = $("#pass").val();

    $.ajax({
        type: 'POST',
        url: '/Temperature/Login?userName=' + username + '&password=' + password,

        contentType: "application/json;charset=UTF-8",
        dataType: 'json',
        error: function (data) {

            if (data.responseText == "Admin") {
                window.location.href = "/Admin/AdminPage";
            }
            else if (data.responseText == "Staff") {
                window.location.href = "/Staff/StaffPage";
            }
            else if (data.responseText == "You are Suspend") {
                swal({
                    title: data.responseText,
                    text: "Please Contact Your Administrator for Activate Your Account...",
                    type: "warning",
                    timer: 5000
                },
                    function () {
                        location.reload();
                    }
                );
                $("#username").css("background-color", "#fff5e6");
                $("#pass").css("background-color", "#fff5e6");
            }
            else {
                swal({
                    title: data.responseText,
                    text: "Username or Password Empty or Invalid...",
                    type: "warning",
                    timer: 5000
                },
                );
                $("#username").css("background-color", "#fff5e6");
                $("#pass").css("background-color", "#fff5e6");
            }
        }

    })
}
