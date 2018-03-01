$(document).ready(function () {
    var boardId = parseInt($("#board-id").val());
    getFavoriteStatus();

    $("#favorite-container")
        .click(function () {
            changeFavorite();
        });


    function getFavoriteStatus() {
        $.ajax({
            url: "/Favorites/IsFavorite",
            type: "GET",
            data: { boardId: boardId },
            dataType: 'JSON',
            success: function (data) {
                if (data.favourite) {
                    $("#fav-true").show();
                    $("#fav-false").hide();
                } else {
                    $("#fav-true").hide();
                    $("#fav-false").show();
                }
                return data;
            }
        });
    }
    function changeFavorite() {
        $.ajax({
            url: "/Favorites/ChangeFavorite",
            type: "POST",
            data: {
                boardId: boardId
            }
        }).done(function () {
            getFavoriteStatus();
        });
    }
});