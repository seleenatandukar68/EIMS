
$(document).ready(function () {
    ValidateSession();
    $("#btnEdit").click(function (e) {
        e.preventDefault();
        $(".form-control*").prop("disabled", false);
        $("#txtId").prop("disabled", true);
        $("#btnSave").prop("disabled", false);
        $("#btnEdit").prop("disabled", true);
    });
    $('#btnFileDet').on('click', function () {
        var $buttonClicked = $(this);  
        var options = {  
            "backdrop": "static",  
            keyboard: true  
        };  
        $.ajax({  
            type: "GET",  
            url: "/Intern/FileDetails",
            data:{'id':parseInt($("#hidID").val())},
            contentType: "application/json; charset=utf-8",  
            datatype: "json",  
            success: function(data)  
            {  
               
                $('#myModalContent').html(data);  
                $('#myModal').modal(options);  
                $('#myModal').modal('show');  
            },  
            error: function() {  
                alert("Content load failed.");  
            }  
        });  
    });  
    $("#btnClose").click(function()  
    {  
          
        $('#myModal').modal('hide');  
    });
    
   
}); 