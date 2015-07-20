var GameInProgressPlayer = (function () {
    function GameInProgressPlayer(play) {
        var _this = this;
        this._play = play;
        this._solutionEntry = new SolutionEntry(true);
        this._pinQuestion = new PinQuestion();

        this.StartCountDown();

        if ($("#hddRound").length !== 0) {
            this.InitFromHtml();
        }

        this._play.Hub.client.NextRound = function (game) {
            $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId + "&gameId=" + game.GameId + "&playerId=" + $("#hddPlayerId").val() + "&roundId=" + game.RoundId, function (htmlResult) {
                _this._play.ChangeContent("#divBodyAnswer", htmlResult);
                _this.InitGame(game);
            });

            $("[data-type=answeredCorrectly]").parent().css("background-color", "transparent");
        };

        this._play.Hub.client.Answered = function (gameId, playerId, correct, totalCorrect) {
            window.console.log(gameId + " " + playerId + " " + correct);

            var container = $("[data-player-mini=" + playerId + "]");

            var spanAnswerCount = container.find("[data-type=answeredCorrectly]");
            Utils.SetElementValue2(spanAnswerCount, totalCorrect.toString());

            if (!correct)
                spanAnswerCount.parent().css("background-color", "lightsalmon");
            else
                spanAnswerCount.parent().css("background-color", "lightgreen");
        };
    }
    GameInProgressPlayer.prototype.InitFromHtml = function () {
        var initialGame = new Game();
        initialGame.QuestionId = $("#hddQuestionId").val();
        initialGame.RoundNumber = $("#hddRound").val();
        initialGame.RoundLength = $("#hddRoundLength").val();

        var dateEnd = new Date($("#hddRoundEnd").val());
        var dateNow = new Date();
        var secondsLeft = Math.round(Math.abs(dateEnd.getTime() - dateNow.getTime()) / 1000);

        this.InitGame(initialGame, secondsLeft);
    };

    GameInProgressPlayer.prototype.InitGame = function (game, secondsRemaining) {
        if (typeof secondsRemaining === "undefined") { secondsRemaining = -1; }
        Utils.SetElementValue("#CurrentRoundNum", game.RoundNumber.toString());

        this._solutionEntry.Init();
        this._pinQuestion.Init();

        this.thisRoundSecTotal = game.RoundLength;
        if (secondsRemaining != -1) {
            this.thisRoundSecLeft = secondsRemaining;
        } else {
            this.thisRoundSecLeft = game.RoundLength;
        }
    };

    GameInProgressPlayer.prototype.StartCountDown = function () {
        var _this = this;
        var self = this;

        window.setInterval(function () {
            $("#divRemainingTime").each(function () {
                var $this = $(this);
                var $progressRound = $("#progressRound");

                $progressRound.css("width", ((self.thisRoundSecLeft / self.thisRoundSecTotal) * 100) + "%");

                $this.find("#spanRemainingTime").html(self.thisRoundSecLeft + "sec");
            });

            _this.thisRoundSecLeft--;
        }, 1000);
    };
    return GameInProgressPlayer;
})();
//# sourceMappingURL=GameInProgressPlayer.js.map
