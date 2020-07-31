$(document).ready(function () {
    ValidateSession();
    $("#btnEdit").click(function (e) {
        e.preventDefault();
        $(".form-control*").prop("disabled", false);
        $("#txtId").prop("disabled", true);
        $("#btnSave").prop("disabled", false);
        $("#btnEdit").prop("disabled", true);
    });
});