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
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        };

        this.Hub.client.Completed = (game: Game) => {
            $.get("/Play/RenderGameCompleted/?gameId=" + game.GameId,
                htmlResult => { this.ChangeBody(htmlResult) }
            );
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });        
    }

    ChangeBody(html : string) {
        $("#divGameBody")
            .animate({ opacity: 0.00 }, 200)
            .empty()
            .append(html)
            .animate({ opacity: 1.00 }, 600);

        $(".show-tooltip").tooltip();
    }
}

class Game {
    GameId: number;
    GameRoundCount: number;
    QuestionId: number;
    Round: number;
    RoundLength: number;
}

$(() => {
    new Play();
}); 