$(document).ready(function () {
    ValidateSession();
    $("#btnReset").click(function () {
        var username = $("#txtUsername").val();
        var email = $("#txtEmail").val();
        var role = $("#ddlRole :selected").val();
        var url = "/User/UpdatePassword";
        var data = { "username": username, "email": email, "role": role }
        $.post(url, data,
                function (result) {
                    $.alert({
                        title: 'Alert',
                        content: result,
                        animation: 'none',
                        type: 'dark'
                    });

                });
    });
    $("#btnCancel").click(function () {
        $("#txtUsername").val('');
        $("#txtEmail").val('');
        $("#ddlRole :selected").val(0);
    });
});