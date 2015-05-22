var GameInProgressPlayer = (function () {
    function GameInProgressPlayer(play) {
        var _this = this;
        this._play = play;

        this._play.Hub.client.NextRound = function (game) {
            Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());
            _this.InitCountDownRound(game.RoundLength);

            $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId, function (htmlResult) {
                _this._play.ChangeContent(htmlResult, "#divBodyAnswer");
            });
        };
    }
    GameInProgressPlayer.prototype.InitCountdown = function () {
        this.InitCountDownRound(20);
    };

    GameInProgressPlayer.prototype.InitCountDownRound = function (roundLength) {
        $("#divRemainingTime").each(function () {
            var $this = $(this);
            var $progressRound = $("#progressRound");

            var finalDate = new Date();
            finalDate.setSeconds(finalDate.getSeconds() + roundLength);

            $this.find("#spanRemainingTime").each(function () {
                $(this).countdown(finalDate, function (event) {
                    $progressRound.css("width", ((event.offset.seconds / roundLength) * 100) + "%");
                    $(this).html(event.strftime('%Ssec'));
                });
            });
        });
    };
    return GameInProgressPlayer;
})();
//# sourceMappingURL=GameInProgressPlayer.js.map
