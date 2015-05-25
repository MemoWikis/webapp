var GameInProgressPlayer = (function () {
    function GameInProgressPlayer(play) {
        var _this = this;
        this._play = play;
        this._solutionEntry = new SolutionEntry();
        this._pinQuestion = new PinQuestion();

        this.StartCountDown();

        if ($("#hddRound").length !== 0) {
            var initialGame = new Game();
            initialGame.QuestionId = $("#hddQuestionId").val();
            initialGame.Round = $("#hddRound").val();
            initialGame.RoundLength = $("#hddRoundLength").val();

            var dateEnd = new Date($("#hddRoundEnd").val());
            var dateNow = new Date();
            var secondsLeft = Math.round(Math.abs(dateEnd.getTime() - dateNow.getTime()) / 1000);

            this.InitGame(initialGame, secondsLeft);
        }

        this._play.Hub.client.NextRound = function (game) {
            _this.InitGame(game);
        };
    }
    GameInProgressPlayer.prototype.InitGame = function (game, secondsRemaining) {
        var _this = this;
        if (typeof secondsRemaining === "undefined") { secondsRemaining = -1; }
        Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());

        $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId, function (htmlResult) {
            _this._play.ChangeContent("#divBodyAnswer", htmlResult);
            _this._solutionEntry.Init();
            _this._pinQuestion.Init();
        });

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
