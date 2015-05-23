class GameInProgressPlayer {

    _play: Play;

    private thisRoundSecTotal; /*in seconds*/
    private thisRoundSecLeft; /*in seconds*/

    constructor(play : Play) {
        this._play = play;

        var initialGame = new Game();
        initialGame.QuestionId = $("#hddQuestionId").val();
        var dateEnd = new Date($("#hddRoundEnd").val()); var dateNow = new Date();
        var secondsLeft = Math.round(Math.abs(dateEnd.getTime() - dateNow.getTime()) / 1000);
        initialGame.Round = $("#hddRound").val();
        initialGame.RoundLength = $("#hddRoundLength").val();

        this.InitGame(initialGame, secondsLeft);

        this._play.Hub.client.NextRound = (game: Game) => {
            this.InitGame(game);
        };

        this.StartCountDown();
    }

    public InitGame(game: Game, secondsRemaining : number = -1) {
        Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());

        $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId,
            htmlResult => { this._play.ChangeContent("#divBodyAnswer", htmlResult) });

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