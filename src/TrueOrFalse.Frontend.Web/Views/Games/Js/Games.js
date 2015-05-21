var Games = (function () {
    function Games() {
        var _this = this;
        var me = this;

        this.InitializeCountdownAll();
        this.InitializeButtonsAll();

        this._hub = $.connection.gameHub;
        this._divGameRowsReady = $("#divGameRowsReady");
        this._divGameRowsReadyNone = $("#divGameRowsReadyNone");

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = function (player) {
            me.GetRow(player.GameId).AddPlayer(player.Name, player.Id);
        };

        this._hub.client.NextRound = function (game) {
            window.console.log(game);
        };

        this._hub.client.Created = function (game) {
            _this._divGameRowsReady.show();
            _this._divGameRowsReadyNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId, function (htmlResult) {
                _this._divGameRowsReady.append($(htmlResult).animate({ opacity: 0.00 }, 0).animate({ opacity: 1.00 }, 700));
                _this.InitializeRow(game.GameId);
            });
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Games.prototype.InitializeRow = function (gameId) {
        this.InitializeButtons("[data-gameId=" + gameId + "] [data-joinGameId]");
        this.InitializeCountdown("[data-gameId=" + gameId + "] [data-countdown]");
    };

    Games.prototype.InitializeButtonsAll = function () {
        this.InitializeButtons("[data-joinGameId]");
    };
    Games.prototype.InitializeCountdownAll = function () {
        this.InitializeButtons("[data-countdown]");
    };

    Games.prototype.InitializeButtons = function (selector) {
        var me = this;

        $(selector).click(function (e) {
            e.preventDefault();

            var gameId = +$(this).attr("data-joinGameId");
            window.console.log(gameId);
            me._hub.server.joinGame(gameId).done(function () {
            }).fail(function (error) {
                window.alert(error);
            });
        });
    };

    Games.prototype.InitializeCountdown = function (selector) {
        $(selector).each(function () {
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
