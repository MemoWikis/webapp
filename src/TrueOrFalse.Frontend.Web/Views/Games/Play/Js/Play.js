var Play = (function () {
    function Play() {
        var _this = this;
        this.Hub = $.connection.gameHub;

        this._gameReady = new GameReady();
        this._gameInProgressPlayer = new GameInProgressPlayer(this);

        this.Hub.client.Started = function (game) {
            $.get("/Play/RenderGameInProgressPlayer/?gameId=" + game.GameId, function (htmlResult) {
                _this.ChangeBody(htmlResult);
            });
        };

        this.Hub.client.Completed = function (game) {
            $.get("/Play/RenderGameCompleted/?gameId=" + game.GameId, function (htmlResult) {
                _this.ChangeBody(htmlResult);
            });
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Play.prototype.ChangeBody = function (html) {
        $("#divGameBody").animate({ opacity: 0.00 }, 200).empty().append(html).animate({ opacity: 1.00 }, 600);

        $(".show-tooltip").tooltip();
    };
    return Play;
})();

var Game = (function () {
    function Game() {
    }
    return Game;
})();

$(function () {
    new Play();
});
//# sourceMappingURL=Play.js.map
