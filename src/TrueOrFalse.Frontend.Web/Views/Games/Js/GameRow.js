var GameRow = (function () {
    function GameRow(gameId) {
        var _this = this;
        this.CurrentUserId = $("#hddCurrentUserId").val();

        this.GameId = gameId;
        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");

        this.ButtonStartGame = this.Div.find("[data-elem=startGame]");
        this.ButtonCancelGame = this.Div.find("[data-elem=cancelGame]");

        window.console.log(this.ButtonCancelGame);

        this.ButtonStartGame.click(function (e) {
            e.preventDefault();
            _this.StartGame();
        });
        this.ButtonCancelGame.click(function (e) {
            e.preventDefault();
            _this.CancelGame();
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
        window.alert("start game");
    };

    GameRow.prototype.CancelGame = function () {
        var gameId = this.GameId;
        $.post("/Games/CancelGame/21", { gameId: gameId.toString() });
    };
    return GameRow;
})();
//# sourceMappingURL=GameRow.js.map
