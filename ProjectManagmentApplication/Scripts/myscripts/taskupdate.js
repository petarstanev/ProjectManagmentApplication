$(document).ready(function () {
    var taskId = $("#taskid").val();
    var hub = $.connection.taskHub;

    hub.client.taskUpdated = function (updatedTaskId) {
        //console.log("refresh" + updatedTaskId);
        if (taskId == updatedTaskId) {
            updateDetails();
        }
    };

    $.connection.hub.start();

    function updateDetails() {
        //console.log(taskId);
        $.ajax({
            url: "/Tasks/DetailsGet",
            type: "GET",
            data: {
                id: taskId
            }
        }).done(function (partialViewResult) {
            $("#task-details-container").html(partialViewResult);
        });
    }
    updateDetails();
});