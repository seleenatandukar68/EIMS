$(document).ready(function () {
    ValidateSession();
    $(function () {
        $("ul.listTasks").sortable({
            connectWith: "ul"

        });
    });

    var internId = $("#username").val();
    var weekId = $("#weekid").val();
    $("#btnSave").click(function () {
        var objArray = Array();
        var outputList = $("#listToDo li").each(function () {
            var text = $(this).html();
            var id = $(this).attr('id');

            var obj = {
                internId: internId,
                TaskId: id,
                WeekId: weekId,
                Status: 1
            }
            objArray.push(obj);
        });
        var outputList = $("#listProgress li").each(function () {
            var text = $(this).html();
            var id = $(this).attr('id');
            console.log(text, id);
            var obj = {
                internId: internId,
                TaskId: id,
                WeekId: weekId,
                Status: 2
            }

            objArray.push(obj);
        });
        var outputList = $("#listDone li").each(function () {
            var text = $(this).html();
            var id = $(this).attr('id');
            console.log(text, id);
            var obj = {
                internId: internId,
                TaskId: id,
                WeekId: weekId,
                Status: 3
            }
            objArray.push(obj);
        });


        var url = "/ProjTask/UpdateTaskStatus";
        var data = JSON.stringify(objArray );

        $.ajax({
            contentType: 'application/json',
            type: "POST",
            url: url,
            dataType: "json",
            data: data,
            async: false,
            success: function (data) {
            }
        });

       
    });


});
