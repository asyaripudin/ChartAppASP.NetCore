function getLogOut() {
    $.ajax({
        type: 'POST',
        url: '/Temperature/Logout/',
        dataType: 'json',
        error: function (data) {
            if (data.responseText == "success") {
                window.location.href = "/Home/Index";
                preventBack();
            }
        }

    });
}
//new code
function preventBack() { window.history.forward(); }
setTimeout("preventBack()", 0);
window.onunload = function () { null };