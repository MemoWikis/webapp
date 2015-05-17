var Play = (function () {
    function Play() {
        this._hub = $.connection.gameHub;

        if (this._hub == null)
            return;

        this._hub.client.NextRound = function (game) {
            window.console.log(game);
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
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
