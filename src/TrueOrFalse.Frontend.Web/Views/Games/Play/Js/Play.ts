class Play {

    Hub: any;

    private _gameReady: GameReady;
    private _gameInProgressPlayer: GameInProgressPlayer;

    constructor() {

        this.Hub = $.connection.gameHub;

        this._gameReady = new GameReady();
        this._gameInProgressPlayer = new GameInProgressPlayer(this);

        if (this.Hub == null)
            return;

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });        
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