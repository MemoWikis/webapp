var GameInProgressPlayer = (function () {
    function GameInProgressPlayer(play) {
        this._play = play;

        this._play.Hub.client.NextRound = function (game) {
            Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());
        };
    }
    return GameInProgressPlayer;
})();
//# sourceMappingURL=GameInProgressPlayer.js.map
