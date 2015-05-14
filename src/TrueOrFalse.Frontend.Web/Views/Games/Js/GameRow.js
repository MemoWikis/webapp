var GameRow = (function () {
    function GameRow(gameId) {
        this.GameId = gameId;
        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");
    }
    GameRow.prototype.AddPlayer = function (name, id) {
        this.DivPlayers.append($("<i class='fa fa-user'></i> " + "<a href='/Nutzer/" + name + "/" + id + "'>" + name + "</a> "));
    };
    return GameRow;
})();
//# sourceMappingURL=GameRow.js.map
