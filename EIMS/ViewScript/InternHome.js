$(document).ready(function () {
    ValidateSession();
    if ($("#txtFlag").val() == false) {

        $("#myModal").modal({
            show: true,
            keyboard: false,
            backdrop: 'static'
        })
    }
    var mentorId = getMentorId();
    var url = "/Mentor/ViewPartialMentor";
    var data = {'id':mentorId}
    $.post(url, data,
        function (result) {
            $("#Mentor").html(result);
        });
    $("#btnSave").click(function Update() {
        var prefId = $("#ddlPref option:selected").val();
        var prefDes = $("#ddlPref option:selected").text();
        if (prefId == "") {
            $.alert({
                title: 'Alert!',
                content: 'Select the Internship Category'
            });
            return false;
        }
        var internId = $("#username").val();

        var url = "/Intern/UpdatePreference";
        var data = { 'prefId': prefId, 'internId' : internId , 'projRole' : prefDes};
        $.post(url, data,
                                             function (result) {
                                                     
                                                 //var data = jQuery.parseJSON(result);
                                                 $("#myModal").modal("hide");
                                                 $.confirm({
                                                     title: 'Confirmation',
                                                     content: result + '<br>Log in again to continue </br>',
                                                     buttons: {
                                                        
                                                         somethingElse: {
                                                             text: 'Log out',
                                                             btnClass: 'btn-blue',
                                                             keys: ['enter', 'shift'],
                                                             action: function () {
                                                                 window.location.href = "/Login/LogOff"
                                                             }
                                                         }
                                                     }
                                                 });
                                            
                                                 

                                                 //window.location = "/Module/Security/ChangePassword.aspx"
                                                 //LogOut();
                                             }
        );
                                                     
});
   
    $("#btnLogOut").click(function () {
        window.location.href = "/Login/LogOff"
    })


    
    var items = $(".liclass");
    var numItems = items.length;
    var perPage = 4;

    items.slice(perPage).hide();

    $('#pagination-container').pagination({
        items: numItems,
        itemsOnPage: perPage,
        prevText: "&laquo;",
        nextText: "&raquo;",
        onPageClick: function (pageNumber) {
            var showFrom = perPage * (pageNumber - 1);
            var showTo = showFrom + perPage;
            items.hide().slice(showFrom, showTo).show();
        }
    });

    //$(".liclass").click(function () {
    //    var weekId = $(this).attr('itemid');
    //    $("#" + weekId).modal("show");
    //});
    //$(".close").click(function () {
    //    var weekId = $(this).attr('itemid');
    //    $("#" + weekId).modal("hide");
    //});
    //$( function() {
    //    $( "ul.listTasks" ).sortable({
    //        connectWith: "ul"
    //    });     
 
       // $("#listToDo, #listProgress, #listDone").disableSelection();
   // } );
    
    
    
});