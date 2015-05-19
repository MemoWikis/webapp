var Play = (function () {
    function Play() {
        this._hub = $.connection.gameHub;
        this.InitCountDown();

        if (this._hub == null)
            return;

        this._hub.client.NextRound = function (game) {
            window.console.log(game);
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Play.prototype.InitCountDown = function () {
        $('[data-willStartIn]').each(function () {
            var $this = $(this), finalDate = $(this).data('willStartIn');

            $this.countdown(finalDate, function (event) {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
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
