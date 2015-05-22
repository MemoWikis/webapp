class GameInProgressPlayer {

    _play : Play;

    constructor(play : Play) {
        this._play = play;

        this._play.Hub.client.NextRound = (game: Game) => {
            Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());
            this.InitCountDownRound(game.RoundLength);

            $.get("/Play/RenderAnswerBody/?questionId=" + game.QuestionId,
                htmlResult => { this._play.ChangeContent("#divBodyAnswer", htmlResult) }
            );
        };
    }

    public InitCountdown() {
        this.InitCountDownRound(20);
    }

    public InitCountDownRound(roundLength : number) {
        $("#divRemainingTime").each(function () {

            var $this = $(this);
            var $progressRound = $("#progressRound");

            var finalDate = new Date();
            finalDate.setSeconds(finalDate.getSeconds() + roundLength);
            
            $this.find("#spanRemainingTime").each(function() {
                $(this).countdown(finalDate, function (event) {

                    $progressRound.css("width", ((event.offset.seconds / roundLength) * 100) + "%");
                    $(this).html(event.strftime('%Ssec'));
                });
            });
        });
    }
}