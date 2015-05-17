class Play {

    private _hub: any;

    constructor() {
        this._hub = $.connection.gameHub;

        if (this._hub == null)
            return;

        this._hub.client.NextRound = (game: Game) => {
            window.console.log(game);
        };

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