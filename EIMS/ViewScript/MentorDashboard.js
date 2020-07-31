$(document).ready(function () {
    ValidateSession();
    $("#btnEdit").click(function () {
        $("#btnSave").attr("disabled", false);
        $("#enableFields *").attr("disabled", false);
    });
});