class GameInProgressPlayer {

    _play: Play;

    private thisRoundSecTotal; /*in seconds*/
    private thisRoundSecLeft; /*in seconds*/

    private _solutionEntry: SolutionEntry;
    private _pinQuestion: PinQuestion;

    constructor(play : Play) {

        this._play = play;
        this._solutionEntry = new SolutionEntry(/*isGameMode*/true);
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
        };
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

        this._solutionEntry.Init();
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