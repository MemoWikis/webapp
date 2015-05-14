var Games = (function () {
    function Games() {
        var me = this;

        this.InitializeCountdown();
        this.InitializeButtons();

        this._hub = $.connection.playHub;

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = function (player) {
            me.GetRow(player.GameId).AddPlayer(player.Name, player.Id);
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Games.prototype.InitializeButtons = function () {
        var me = this;

        $("[data-joinGameId]").click(function (e) {
            e.preventDefault();

            var gameId = +$(this).attr("data-joinGameId");
            me._hub.server.joinGame(gameId).done(function () {
            }).fail(function (error) {
                window.alert(error);
            });
        });
    };

    Games.prototype.InitializeCountdown = function () {
        $('[data-countdown]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown');
            $this.countdown(finalDate, function (event) {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    };

    Games.prototype.GetRow = function (gameId) {
        return new GameRow(gameId);
    };
    return Games;
})();

var Player = (function () {
    function Player() {
    }
    return Player;
})();

$(function () {
    new Games();
});
//# sourceMappingURL=Games.js.map
