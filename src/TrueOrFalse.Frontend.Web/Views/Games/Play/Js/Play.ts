class Play {

    private _hub: any;

    constructor() {

        this._hub = $.connection.gameHub;
        this.InitCountDown();

        if (this._hub == null)
            return;

        this._hub.client.NextRound = (game: Game) => {
            window.console.log(game);
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });        
    }

    public InitCountDown() {
        $('[data-willStartIn]').each(function() {
            var $this = $(this), finalDate = $(this).data('willStartIn');

            $this.countdown(finalDate, event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
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