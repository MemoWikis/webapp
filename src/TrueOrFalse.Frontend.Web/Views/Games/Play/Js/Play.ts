class Play {

    Hub: any;

    private _gameReady: GameReady;
    private _gameInProgressPlayer: GameInProgressPlayer;

    constructor() {

        this.Hub = $.connection.gameHub;

        this._gameReady = new GameReady();
        this._gameInProgressPlayer = new GameInProgressPlayer(this);

        this.Hub.client.Started = (game: Game) => {
            $.get("/Play/RenderGameInProgressPlayer/?gameId=" + game.GameId,
                htmlResult => {
                    this.ChangeBody(htmlResult);
                    this._gameInProgressPlayer.InitFromHtml();
                }
            );
        };

        this.Hub.client.Completed = (game: Game) => {
            $.get("/Play/RenderGameCompleted/?gameId=" + game.GameId,
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        };

        this.Hub.client.NeverStarted = (game: Game) => {
            $.get("/Play/RenderGameNeverStarted/?gameId=" + game.GameId,
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        }

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });        
    }

    ChangeContent(selector : string, html: string) {
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