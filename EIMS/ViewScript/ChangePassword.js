$(document).ready(function () {
    ValidateSession();
    $("#btnChange").click(function () {
        debugger;
        if (Validation()) {
            var obj = {
                Password: $("#txtNewPass").val(),
                UserId: $("#userid").val()
            }

            var url = "/User/UpdateUser";
            var data = obj;
            $.post(url, data,
                            function (result) {
                                $.alert({
                                    title: 'Alert',
                                    content: "Password Updated",
                                    animation: 'none',
                                    type: 'dark'


                                });

                            });
        }

    });

    var Validation = function () {
        var newPass = $("#txtNewPass").val();
        var conPass = $("txtConfirmPass").val();
        var errMsg = "";
        if (newPass == "") {
            errMsg += "Enter New Passsword <br>";
        }
        if(conPass == "")
        {
            errMsg += "Enter Confirm Password <br>";
        }
        if (newPass != "" && conPass != "") {
            if (newPass != conPass) {
                errMsg += "Passwords doesn't match<br>";
            }
        }

        if (errMsg == "") {
            return true;
        }
        else {
            $.alert({
                title: 'Alert',
                content: errMsg,
                animation: 'none',
                type: 'dark'
              

            });
            return false;
        }
    }
});