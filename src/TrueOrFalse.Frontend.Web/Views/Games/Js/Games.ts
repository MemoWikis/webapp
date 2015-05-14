class Games {

    private _hub : any;

    constructor() {

        var me = this;

        this.InitializeCountdown();
        this.InitializeButtons();

        this._hub = $.connection.playHub;

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = (player: Player) => {
            window.console.log(player);
            var gameRow = me.GetRow(player.GameId);
            gameRow.AddPlayer(player.Name, player.Id);
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });
    }

    InitializeButtons() {
        var me = this;

        $("[data-joinGameId]").click(function(e){
            e.preventDefault();

            //hide button
            //show is player (?)

            var gameId = $(this).attr("data-joinGameId");
            me._hub.server.joinGame(gameId).done(() => {}).fail(error => {
                window.alert(error);
            });
        });
    }

    InitializeCountdown() {
        $('[data-countdown]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown');
            $this.countdown(finalDate, event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });        
    }

    GetRow(gameId : number) {
        return new GameRow(gameId);
    }
}

class Player {
    Id: number;
    Name: string;
    GameId: number;
} 

$(() => {
    new Games();
}); 