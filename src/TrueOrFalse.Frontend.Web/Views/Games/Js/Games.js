var Games = (function () {
    function Games() {
        var _this = this;
        this._gameRows = [];
        var me = this;

        this._hub = $.connection.gameHub;

        this._divGamesReady = $("#divGamesReady");
        this._divGamesReadyNone = $("#divGamesReadyNone");

        this._divGamesInProgress = $("#divGamesInProgress");
        this._divGamesInProgressNone = $("#divGamesInProgressNone");

        this.InitializeCountdownAll();
        this.InitializeButtonsAll();
        this.InitRows();

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = function (player) {
            me.GetRow(player.GameId).AddPlayer(player);
        };

        this._hub.client.NextRound = function (game) {
            var currentRowSelector = "[data-gameId=" + game.GameId + "] [data-elem = currentRound]";
            Utils.SetElementValue(currentRowSelector, game.RoundNumber.toString());

            $("[data-gameId=" + game.GameId + "] [data-elem=currentRoundContainer]").attr("data-original-title", game.RoundNumber + " Runden von " + game.GameRoundCount + " gespielt");

            $(currentRowSelector + " .show-tooltip").tooltip();
        };

        this._hub.client.Created = function (game) {
            _this._divGamesReady.show();
            _this._divGamesReadyNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId, function (htmlResult) {
                _this._divGamesReady.append($(htmlResult).animate({ opacity: 0.00 }, 0).animate({ opacity: 1.00 }, 700));
                _this.InitializeRow(game.GameId);
            });
        };

        this._hub.client.Started = function (game) {
            _this._divGamesInProgress.show();
            _this._divGamesInProgressNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId, function (htmlResult) {
                $("[data-gameId=" + game.GameId + "]").hide(700).remove();

                _this._divGamesInProgress.append($(htmlResult).animate({ opacity: 0.00 }, 0).animate({ opacity: 1.00 }, 700));

                _this.InitializeRow(game.GameId);

                if (_this._divGamesReady.find("[data-gameId]").length == 0) {
                    _this._divGamesReadyNone.show();
                }
            });
        };

        this._hub.client.Completed = function (game) {
            $("[data-gameId=" + game.GameId + "]").hide(700).remove();

            _this.IfNeeded_ShowNoGamesInProgressInfo();
        };

        this._hub.client.NeverStarted = function (game) {
            $("[data-gameId=" + game.GameId + "]").hide(700).remove();

            _this.IfNeeded_ShowNoGamesReadyInfo();
        };

        this._hub.client.Canceled = function (cancel) {
            $("[data-gameId=" + cancel.GameId + "]").hide(700).remove();

            _this.IfNeeded_ShowNoGamesReadyInfo();
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Games.prototype.IfNeeded_ShowNoGamesInProgressInfo = function () {
        if (this._divGamesInProgress.find("[data-gameId]").length == 0) {
            this._divGamesInProgressNone.show();
        }
    };

    Games.prototype.IfNeeded_ShowNoGamesReadyInfo = function () {
        if (this._divGamesReady.find("[data-gameId]").length === 0) {
            this._divGamesReadyNone.show();
        }
    };

    Games.prototype.InitializeRow = function (gameId) {
        this.InitializeButtons("[data-gameId=" + gameId + "] [data-joinGameId]");
        this.InitializeCountdown("[data-gameId=" + gameId + "] [data-countdown]");
        $(".show-tooltip").tooltip();

        this._gameRows.push(new GameRow(gameId));
    };

    Games.prototype.InitializeButtonsAll = function () {
        this.InitializeButtons("[data-joinGameId]");
    };
    Games.prototype.InitializeCountdownAll = function () {
        this.InitializeCountdown("[data-countdown]");
    };

    Games.prototype.InitializeButtons = function (selector) {
        var me = this;

        $(selector).click(function (e) {
            e.preventDefault();

            var gameId = +$(this).attr("data-joinGameId");

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

    Games.prototype.InitRows = function () {
        var self = this;
        $("[data-gameId]").each(function () {
            self._gameRows.push(new GameRow(parseInt($(this).attr("data-gameId"))));
        });

        window.console.log(self._gameRows);
    };

    Games.prototype.GetRow = function (gameId) {
        return $.grep(this._gameRows, function (row) {
            return row.GameId === gameId;
        })[0];
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
