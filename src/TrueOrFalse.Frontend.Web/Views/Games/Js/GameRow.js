var GameRow = (function () {
    function GameRow(gameId, hub) {
        var _this = this;
        this.GameId = gameId;
        this._hub = hub;

        this.CurrentUserId = $("#hddCurrentUserId").val();

        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");

        this.ButtonStartGame = this.Div.find("[data-elem=startGame]");
        this.ButtonCancelGame = this.Div.find("[data-elem=cancelGame]");
        this.ButtonJoinGame = this.Div.find("[data-elem=joinGame]");
        this.ButtonLeaveGame = this.Div.find("[data-elem=leaveGame]");

        this.ButtonStartGame.click(function (e) {
            e.preventDefault();
            _this.StartGame();
        });
        this.ButtonCancelGame.click(function (e) {
            e.preventDefault();
            _this.CancelGame();
        });
        this.ButtonJoinGame.click(function (e) {
            e.preventDefault();
            _this.JoinGame();
        });
        this.ButtonLeaveGame.click(function (e) {
            e.preventDefault();
            _this.LeaveGame();
        });

        this.IsCreator = this.Div.attr("data-isCreator") === "True";
        this.IsPlayer = this.Div.attr("data-isPlayer") === "True";
        this.IsCreatorOrPlayer = this.IsCreator || this.IsPlayer;

        window.console.log("isPlayer -> " + this.IsPlayer);

        this.SpanYouArePlayer = this.Div.find(".spanYouArePlayer");
    }
    GameRow.prototype.UiAddPlayer = function (player) {
        if (player.UserId == this.CurrentUserId) {
            this.ButtonJoinGame.hide();
            this.ButtonLeaveGame.show();
            this.Div.find(".spanYouArePlayer").show();
            this.IsPlayer = true;
        }

        if (this.IsCreator) {
            if (player.TotalPlayers > 1) {
                this.ButtonStartGame.show();
                this.ButtonCancelGame.hide();
            } else {
                this.ButtonStartGame.hide();
                this.ButtonCancelGame.show();
            }
        }

        this.DivPlayers.append($("<i class='fa fa-user' data-playerUserId='" + player.UserId + "'></i> " + "<a href='/Nutzer/" + player.Name + "/" + player.UserId + "' data-playerUserId='" + player.UserId + "'>" + player.Name + "</a> "));
    };

    GameRow.prototype.UiRemovePlayer = function (playerUserId) {
        $("[data-playerUserId=" + playerUserId + "]").hide();

        if (this.IsPlayer) {
            this.ButtonJoinGame.show();
            this.ButtonLeaveGame.hide();
            this.SpanYouArePlayer.hide();

            this.IsPlayer = false;
        }

        if (this.IsCreator) {
            this.ButtonStartGame.hide();
            this.ButtonCancelGame.show();
        }
    };

    GameRow.prototype.StartGame = function () {
        var gameId = this.GameId;
        $.post("/Games/StartGame", { gameId: gameId.toString() });

        window.location.href = this.Div.find("[data-elem=urlGame]").attr("href");
    };

    GameRow.prototype.JoinGame = function () {
        this._hub.server.joinGame(this.GameId).done(function () {
        }).fail(function (error) {
            window.alert(error);
        });
    };

    GameRow.prototype.LeaveGame = function () {
        this._hub.server.leaveGame(this.GameId).done(function () {
        }).fail(function (error) {
            window.alert(error);
        });
    };

    GameRow.prototype.CancelGame = function () {
        var gameId = this.GameId;
        $.post("/Games/CancelGame", { gameId: gameId.toString() });
    };

    GameRow.prototype.ChangeTime = function (newTime) {
        window.console.log("newTime: " + newTime);
        $("[data-countdown]").attr("data-countdown", newTime);
    };
    return GameRow;
})();
//# sourceMappingURL=GameRow.js.map
