$(document).ready(function () {
    ValidateSession();

    $("#btnSave").click(function () {

        if (Validation()) {
            var data = {
                weekId: document.getElementById('hidWeekId').value,
                projId: document.getElementById('hidProjId').value,
                internId: document.getElementById('hidInternId').value,
                Comment: tinyMCE.activeEditor.getContent(),
                status: document.getElementById('hidStatus').value,
            }
            $.ajax({
                url: "/Comment/SaveComment",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (result) {
                   
                    $.alert({
                        title: 'Alert',
                        content: 'Comment Posted!',
                        animation: 'none',
                        type: 'dark',
                      

                    });
                
                    var chat = $.connection.CommentHub;
                    $.connection.hub.start().done(function () {
                        //getMentorId(document.getElementById('hidInternId').value)
                       debugger
                       
                        chat.server.send(getMentorId());


                    });
                },
                error: function (errormessage) {
                   // alert(errormessage.responseText);
                }
            });
   
        }
    
    });
    $("#btnViewFeedback").click(function () {
        $("#mdlFeedback").modal("show");

    });
    $("#btnClose").click(function () {
        $("#mdlFeedback").modal("hide");

    });
    

    var Validation = function () {
      
        return true;
    }
});