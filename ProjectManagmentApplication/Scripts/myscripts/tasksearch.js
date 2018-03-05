$(document).ready(function () {
    var boardId = parseInt($("#board-id").val());
    var hub = $.connection.boardHub;
    var selectedType = "all-tasks";
    var selectedTime = "";
    hub.client.boardUpdated = function (updatedBoardId) {
        console.log("refresh" + updatedBoardId);
        if (updatedBoardId === boardId) {
            refreshBoardData();
        }
    };

    $('#selector-buttons .btn')
        .click(function () {
            $(this).addClass('active').siblings().removeClass('active');
            var buttonType = $(this).attr('id');
            selectedType = buttonType;
            refreshBoardData();
        });

    function refreshBoardData() {
        var taskNameSearch = $("#task-name-search").val();
        var personName = $("#task-person-name-search").val();

        $.ajax({
            url: "/Boards/DetailsPart",
            type: "GET",
            data: {
                id: boardId,
                type: selectedType,
                taskName: taskNameSearch,
                time: selectedTime,
                personName: personName
            }
        }).done(function (partialViewResult) {
            $("#board-column-container").html(partialViewResult);
        });
    }

    $("#task-name-search, #task-person-name-search").on('input', function (e) {
        refreshBoardData();
    });

    $('#deadline-bt-group .btn')
        .click(function () {
            $(this).addClass('active').siblings().removeClass('active');
            selectedTime = $(this).attr('id');
            refreshBoardData();
        });

    $("#all-tasks").trigger("click");
    $("#deadline-bt-group #all").trigger("click");
});