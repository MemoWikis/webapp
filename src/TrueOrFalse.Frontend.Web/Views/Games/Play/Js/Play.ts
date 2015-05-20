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
            .animate({ opacity: 0.00 }, 30)
            .empty()
            .append(html)
            .animate({ opacity: 1.00 }, 300);        
    }
}

class Game
{
    GameId: number;
    Round : number;
}

$(() => {
    new Play();
}); 