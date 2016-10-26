class Play {

    Hub: any;

    private _gameReady: GameReady;
    private _gameInProgressPlayer: GameInProgressPlayer;
    private _gameId: number;

    constructor() {

        this.Hub = $.connection.gameHub;

        this._gameReady = new GameReady();
        this._gameInProgressPlayer = new GameInProgressPlayer(this);
        this._gameId = $("#GameId").val();

        this.Hub.client.Started = (game: Game) => {

            if (game.GameId != this._gameId) { return; }

            $.get("/Play/RenderGameInProgressPlayer/?gameId=" + game.GameId,
                htmlResult => {
                    this.ChangeBody(htmlResult);
                    this._gameInProgressPlayer.InitFromHtml();
                }
            );
        };

        this.Hub.client.Completed = (game: Game) => {

            if (game.GameId != this._gameId) { return; }

            AnswerQuestion.LogTimeForQuestionView();

            $.get("/Play/RenderGameCompleted/?gameId=" + game.GameId,
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        };

        this.Hub.client.NeverStarted = (game: Game) => {

            if (game.GameId != this._gameId) { return; }

            $.get("/Play/RenderGameNeverStarted/?gameId=" + game.GameId,
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        }

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });        
    }

    ChangeContent(selector: string, html: string) {

        Play.HideSolutionDetails();

        $(selector)
            .empty()
            .animate({ opacity: 0.00 }, 0)
            .append(html)
            .animate({ opacity: 1.00 }, 600);

        $(".show-tooltip").tooltip();
    }

    ChangeBody(html : string) {
        this.ChangeContent("#divGameBody", html);
    }

    static HideSolutionDetails() {
        $("#SolutionDetailsSpinner").hide();
        $("#SolutionDetails").hide();
    }
}

class Game {
    GameId: number;
    GameRoundCount: number;
    QuestionId: number;
    RoundId: number;
    RoundNumber: number;
    RoundLength: number;
}

$(() => {
    new Play();
}); 