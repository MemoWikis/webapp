var Play = (function () {
    function Play() {
        this.Hub = $.connection.gameHub;

        this._gameReady = new GameReady();
        this._gameInProgressPlayer = new GameInProgressPlayer(this);

        if (this.Hub == null)
            return;

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
