$(document).ready(function () {
    ValidateSession();
    $("#btnCancel").click(function () {
        $("#txtInternId").val("");
        $("#txtStartDate").val("");
        $("#txtEndDate").val("");
        $("#txtAreaRemarks").val("");
    });
    $("#btnPrint").click(function () {
        if (Validation()) {
       
            var obj = {
                InternId: $("#txtInternId").val(),
                internStartDate: $("#txtStartDate").val(),
                internEndDate: $("#txtEndDate").val(),
                Remarks: $("#txtAreaRemarks").val()
            }
          
                var url = "/home/printcertificate";

                var data = obj;
                $.post(url, data,
                                function (result) {
                                    var div = "<div id ='certificate'/>";
                                    div = "To Whom It May Concern <br>";
                                    div +="This is to certify that " + result.FName + " " + result.LName + " has completed his/her internship at ASTS in the " + result.prefCategory;
                                     div += "The internship period was from " + moment(result.internStartDate).format('MMMM Do YYYY') + " to " + moment(result.internStartDate).format('MMMM Do YYYY');
                                     div += "His/Her performance was " + result.Remarks
                                        
                                     $("#ptinnt").append(div);

                                     var doc = new jsPDF();
                                     var specialElementHandlers = {
                                         '#editor': function (element, renderer) {
                                             return true;
                                         }
                                     };

                                   
                                     doc.fromHTML($('#ptinnt').html(), 15, 15, {
                                             'width': 170,
                                             'elementHandlers': specialElementHandlers
                                         });
                                         doc.save('LetterOfCompletion.pdf');
                                    

                                });


            }

        
    })

    var Validation = function () {
        var errMsg = "";
        if ($("#txtInternId").val() == "") {
            errMsg += "Enter Intern Id <br>";
        }
        if ($("#txtStartDate").val() == "") {
            errMsg += "Enter Internship Start date<br>";
        }
        if ($("#txtEndDate").val() == "") {
            errMsg += "Enter Internship End date<br>";
        }
        if ($("#txtAreaRemarks").val() == "") {
            errMsg += "Enter Performance Remarks";
        }
        if (errMsg == "") {
            return true
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