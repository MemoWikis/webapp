class GameInProgressPlayer {

    _play: Play;

    private thisRoundSecTotal; /*in seconds*/
    private thisRoundSecLeft; /*in seconds*/

    private _answerEntry: AnswerEntry;
    private _pinQuestion: PinQuestion;

    constructor(play : Play) {

        this._play = play;
        this._answerEntry = new AnswerEntry(/*isGameMode*/true);
        this._pinQuestion = new PinQuestion();

        this.StartCountDown();

        if ($("#hddRound").length !== 0) {
            this.InitFromHtml();
        }

        this._play.Hub.client.NextRound = (game: Game) => {
            $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId + "&gameId=" + game.GameId +
                    "&playerId=" + $("#hddPlayerId").val() + "&roundId=" + game.RoundId,
                htmlResult => {
                    this._play.ChangeContent("#divBodyAnswer", htmlResult);
                    this.InitGame(game);
                });

            $("[data-type=answeredCorrectly]").parent().css("background-color", "transparent");
        };

        this._play.Hub.client.Answered = (
            gameId: number,
            playerId: number,
            correct: boolean,
            totalCorrect: number) => {
            window.console.log(gameId + " " + playerId + " " + correct);

            var container = $("[data-player-mini=" + playerId + "]");

            var spanAnswerCount = container.find("[data-type=answeredCorrectly]");
            Utils.SetElementValue2(spanAnswerCount, totalCorrect.toString());

            if(!correct)
                spanAnswerCount.parent().css("background-color", "lightsalmon");
            else
                spanAnswerCount.parent().css("background-color", "lightgreen");
        }
    }

    public InitFromHtml() {
        var initialGame = new Game();
        initialGame.QuestionId = $("#hddQuestionId").val();
        initialGame.RoundNumber = $("#hddRound").val();
        initialGame.RoundLength = $("#hddRoundLength").val();

        var dateEnd = new Date($("#hddRoundEnd").val()); var dateNow = new Date();
        var secondsLeft = Math.round(Math.abs(dateEnd.getTime() - dateNow.getTime()) / 1000);

        this.InitGame(initialGame, secondsLeft);        
    }

    public InitGame(game: Game, secondsRemaining : number = -1) {
        Utils.SetElementValue("#CurrentRoundNum", game.RoundNumber.toString());

        this._answerEntry.Init();
        this._pinQuestion.Init();

        this.thisRoundSecTotal = game.RoundLength;
        if (secondsRemaining != -1) {
            this.thisRoundSecLeft = secondsRemaining;
        } else {
            this.thisRoundSecLeft = game.RoundLength;
        }
    }

    public StartCountDown() {

        var self = this;

        window.setInterval(() => {

            $("#divRemainingTime").each(function() {

                var $this = $(this);
                var $progressRound = $("#progressRound");

                $progressRound.css("width",
                    ((self.thisRoundSecLeft / self.thisRoundSecTotal) * 100) + "%");

                $this.find("#spanRemainingTime").html(self.thisRoundSecLeft + "sec");
            });

            this.thisRoundSecLeft--;

        }, 1000);
    }
}