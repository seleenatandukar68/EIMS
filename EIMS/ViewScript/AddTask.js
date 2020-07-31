$(document).ready(function () {
    ValidateSession();
    var count = 0;
    function GetDynamicTextBox(value,id,status) {
        var div = $("<div class='dynamicdiv'/>");

        var textBox = $("<input />").attr("type", "textbox").attr("class", "DynamicTextBox").attr("taskid", id).attr('status',status)
        textBox.val(value);
       
        div.append(textBox);
        // $(".DynamicTextBox").focus();

        var button = $("<input />").attr("type", "button").attr("value", "Remove").attr("onclick", "RemoveTextBox(this)").attr("class", "DynamicButton").attr("taskid", id);
        div.append(button);

        return div;
    }
    $("#btnView").click(function () {
        $("#TextBoxContainer").empty();
        LoadTaskDetails();
    });

    $("#btnAdd").click(function () {
        count++; 
        var div = GetDynamicTextBox("","",'false');
        $("#TextBoxContainer").append(div);

    });

    $("#btnSubmit").click(function () {
        if (Validation) {
            var projId = $("#ddlProject option:selected").val();
            var catId = $("#ddlCategory option:selected").val();
            var weekId = $("#txtWeekId").val();
            var taskList = Array();

            $(".DynamicTextBox").each(function () {
             if ($(this).val() != "" ) {
                    var task = {
                        TaskDes: $(this).val(),
                        TaskId: $(this).attr("taskid")
                    }
                    taskList.push(task);
                }
            });
            var project = {
                ProjId: projId
            }
            var obj = {
                project: project,
                prefId: catId,
                weekId: weekId,
                tasks: taskList,
                AssignedBy: getUserId()
            }
            if (taskList.length == 0) {
                $.alert({
                    title: 'Alert!',
                    content: 'Enter the task details !!!',
                    animation: 'none'
                });
            }
            else {
                var url = "/Mentor/SaveTask";
                var data = obj;
                $.post(url, data,
                                    function (result) {

                                                         //    var data = jQuery.parseJSON(result);

                                        $.confirm({
                                            title: 'Alert',
                                            content: result,
                                            animation: 'none',
                                            buttons: {
                                                OK: {
                                                    action: function () {
                                                        window.location.href = "/Mentor/AddTask";
                                                    }

                                                }


                                            }
                                        });


                                                         //  window.location.href = "/Mentor/AddTask";

                                                         //window.location = "/Module/Security/ChangePassword.aspx"
                                                         //LogOut();
                                                     }
                );

            }

        }

    });
    $("#btnPublish").click(function () {
        if (Validation) {
            var projId = $("#ddlProject option:selected").val();
            var catId = $("#ddlCategory option:selected").val();
            var weekId = $("#txtWeekId").val();
            var taskList = Array();

            $(".DynamicTextBox").each(function () {
                if ($(this).val() != "") {
                    var task = {
                        TaskDes: $(this).val(),
                        TaskId: $(this).attr("taskid"),
                        Status: $(this).attr("status")
                    }
                    taskList.push(task);
                }
            });
            var project = {
                ProjId: projId
            }
            var obj = {
                project: project,
                prefId: catId,
                weekId: weekId,
                tasks: taskList,
                AssignedBy: getUserId()
            }
            if (taskList.length == 0) {
                $.alert({
                    title: 'Alert!',
                    content: 'Enter the task details !!!',
                    animation: 'none'
                });
            }
            else {
                $.confirm({
                    title: 'Confirmation',
                    content: 'Unsaved changes will not be published. Continue anyway?',
                    animation: 'none',
                    buttons: {
                        yes: {
                            action: function () {
                                var url = "/Mentor/PublishTask";
                                var data = obj;
                                $.post(url, data,
                                                                     function (result) {

                                                                         //    var data = jQuery.parseJSON(result);

                                                                         $.confirm({
                                                                             title: 'Alert',
                                                                             content: result,
                                                                             animation: 'none',
                                                                             buttons: {
                                                                                 OK: {
                                                                                     action: function () {
                                                                                         window.location.href = "/Mentor/AddTask";
                                                                                     }

                                                                                 }


                                                                             }
                                                                         });
                                                                    
                                                                         //window.location.href = "/Mentor/AddTask";

                                                                         //window.location = "/Module/Security/ChangePassword.aspx"
                                                                         //LogOut();
                                                                     }
                                );
                            }
                        },
                        no: {
                            action: function () {
                                $.alert({
                                    title: 'Alert',
                                    content: 'Save before publishing ',
                                    animation: 'none',
                                    type: 'dark'


                                });
                            }
                        }
                    }
                });
   

            }

        }

    });
    //============================
    var LoadTaskDetails = function() {
     if (Validation()) {
         $("#taskDetails").attr("style", "display:block");
        var projId = $("#ddlProject option:selected").val();
        var catId = $("#ddlCategory option:selected").val();
        var weekId = $("#txtWeekId").val();
        var project = {
            ProjId: projId
        }
        var obj = {
            project: project,
            prefId: catId,
            weekId: weekId,
               
            AssignedBy: getUserId()
        }
        var url = "/Mentor/GetAssignedTask";
        var data = obj;
        $.post(url, data,
                                             function (result) {
                                                 var data = result;
                                                 tasks = data.tasks;
                                                 if (tasks.length > 0) {
                                                     for (i = 0; i < tasks.length ; i++) {
                                                         var div = GetDynamicTextBox(tasks[i].TaskDes, tasks[i].TaskId, tasks[i].Status);
                                                         $("#TextBoxContainer").append(div);
                                                     }

                                                 }
                                             }
        );
    }
}
    //===========================
    var Validation = function () {
        var errMsg = "";
        var projId = $("#ddlProject option:selected").val();
        var catId = $("#ddlCategory option:selected").val();
        var weekId = $("#txtWeekId").val();
        if (projId == "") {
            errMsg += "Select the project <br>";

        }
        if (catId == "") {
            errMsg += "Select the category <br>";
        }
        if (weekId == "") {
            errMsg += "Enter Week No <br>";
        }
        else {
            if (weekId < 1 || weekId > 12) {
                errMsg += "Enter the week no within range 1-12 \n";
            }
        }
        if (errMsg != "") {
            $.alert({
                title: 'Alert!',
                content: errMsg,
                animation: 'none'
            });
            return false;
        }
        else
            return true;
    }
  
});

function RemoveTextBox(button) {
    if ($(button).attr("taskid") != null || $(button).attr("taskid") != "") {
        $.confirm({
            title: 'Confirmation',
            content: 'Do you want to remove this?',
            animation:'none',
            buttons: {
                yes: {
                    action: function(){
                            $(button).parent().remove();
                            var url = "/Mentor/DeleteTask";
                            var data = {"taskid": $(button).attr("taskid")};
                            $.post(url, data,
                                        function (result) {
                                            $.alert({
                                                title: 'Alert',
                                                content: result,
                                                animation:'none'
                                                 
                                            });

                                        }
                        );
                    }

                },
                no:{
                    
                }

             
            }
        });
    }
    else{
        $(button).parent().remove();
    }
    
}



function reset(){
    $("#ddlProject option:selected").val("");
    $("#ddlCategory option:selected").val("");
    $("#txtWeekId").val("");
    $("#taskDetails").attr("style","display:none");
    

}