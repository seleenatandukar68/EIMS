$(document).ready(function () {
    ValidateSession();
    function GetDynamicTextBox(result) {
        var div = $("<div class = 'row '/>");
        var div1 = $("<div class='col-md-2'/>");
        var value = result.InternId;
        div1.append(value);
        var div2 = $("<div class='col-md-4'/>");
        div2.append(result.FName+' ')

        var MName = " ";
        if(result.MName != null){
            MName = result.LName+ ' ';
        }
        div2.append(result.MName).append(result.LName);
        var div3 = $("<div class='col-md-4'/>");
        div3.append(result.prefCategory);
        var div4 = $("<div class='col-md-2'/>").append("Select").append("<input type= 'hidden'/>").attr("internID", result.Id).attr("onclick", "GetDetails(this)");

        div.append(div1).append(div2).append(div3).append(div4);

        
        

        return div;
    }
    
    $("#btnView").click(function () {
        var projId = $("#ddlProject option:selected").val();
        var catId = $("#ddlCategory option:selected").val();
        var weekid = $("#ddlWeek option:selected").val();
        if (projId == 0|| weekid == 0) {
            $.alert({
                title: 'Alert',
                content: "Select Project and Week No",
                animation: 'none',
                type: 'dark'
            });
        }
        else{
        var url = "/Intern/GetInternByMentorAndPrefAndProjId";
        var data = { 'mentorId': getUserId(), 'prefId': catId, 'projId': projId }
        $.post(url, data,
            function (result) {
                $("#divDetails").empty();
                if (result.length > 0) {
                    for (i = 0; i < result.length ; i++) {
                        var div = GetDynamicTextBox(result[i]);
                        $("#divDetails").append(div);
                    }

                }
                $("#divIntern").attr("style","display:block")
                console.log(result);
            });
        }
    });
});
function GetDetails(select) {
    var internID = $(select).attr("internID");
    var projId = $("#ddlProject option:selected").val();
    var weekid = $("#ddlWeek option:selected").val();
    if (projId == 0 || weekid == 0) {
        $.alert({
            title: 'Alert',
            content: "Select Project and Week No",
            animation: 'none',
            type: 'dark'
        });
    }
    else {
        window.location.href = "/ProjTask/getWeeklyTaskById?ProjId=" + projId + "&Internid=" + internID + "&WeekId=" + weekid;
    }
}