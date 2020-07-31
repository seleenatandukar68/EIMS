$(document).ready(function () {
    ValidateSession();
    $("#btnView").click(function () {
        var obj = {
            internId: $("#internId").val(),
            weekId: $("#weekId").val(),
            projId: $("#projectId").val()
        }
        var url = "/Comment/GetCommentById";
        var data = obj;
        $.post(url, data,
            function (result) {
                $("#divComment").empty();
                if (result.Comment == null) {
                    $("#divComment").append("No Comments Made yet");

                }
                else{
                $("#divComment").append(result.Comment).append('Commented On: ').append(result.addedOn);
                UpdateIsRead(result.cmtId);
            }
            });
    });
    var UpdateIsRead = function (cmtId) {
        $.ajax({
            url: "/Comment/UpdateIsRead",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: { 'cmtId': cmtId },
            success: function (result) {
                GetNotification();
            }
        });
    }

    $("#btnFeedback").click(function () {
        $("#mdlFeedback").modal({
            show: true
        });
    });
    $("#btnSave").click(function () {
        debugger
        var obj = {
            internId: $("#internId").val(),
            weekId: $("#weekId").val(),
            projId: $("#projectId").val(),
            feedback: $("#txtFeedback").val()
        }
        $.ajax({
            url: "/Comment/SaveFeedback",
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: obj,
            success: function (result) {
                debugger;
                $.alert({
                    title: 'Alert',
                    content: 'Feedback Posted !!!',
                    animation: 'none'
                });
                $("#mdlFeedback").modal({
                    hide: true
                });
            }
        });
    });
    $("#btnClose").click(function () {
        $("#mdlFeedback").modal("hide");
    });
});