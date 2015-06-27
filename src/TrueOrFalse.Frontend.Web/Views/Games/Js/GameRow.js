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
        this.ButtonJoinGame = this.Div.find("[data-elem=cancelGame]");

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
    }
    GameRow.prototype.AddPlayer = function (player) {
        if (player.Id === this.CurrentUserId) {
            $(".linkJoin").hide();
            this.Div.find(".spanYouArePlayer").show();
        }

        if (player.TotalPlayers > 1) {
            this.ButtonStartGame.show();
            this.ButtonCancelGame.hide();
        } else {
            this.ButtonStartGame.hide();
            this.ButtonCancelGame.show();
        }

        this.DivPlayers.append($("<i class='fa fa-user'></i> " + "<a href='/Nutzer/" + player.Name + "/" + player.Id + "'>" + player.Name + "</a> "));
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
