
   function ValidateSession () {
        var url = "/Shared/ValidateSession";
        $.ajax({
            
            type: "POST",
            url: url,
            
            
            async: false,
            success: function (result) {
               // alert(result);
                if (result == "False") {
                    window.location.href = "/Login/LogOff";
                }
            }
        });
        

        }
   function getUserId() {
       return $("#username").val();
   }

   function getMentorId() {
       var id = "";
       var url = "/Shared/GetMentortoNotify";
       $.ajax({

           type: "POST",
           url: url,
           data : {'InterId' : getUserId()},

           async: false,
           success: function (result) {
               debugger
               id = result;
           }
       });
       return id;
   }