class Games {

    private _hub : any;

    constructor() {
        this.InitializeCountdown();
        this.InitializeButtons();

        this._hub = $.connection.playHub;

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = (customer: Player) => {
            window.console.log(customer);
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });
    }

    InitializeButtons() {
        var me = this;

        $("[data-joinGameId]").click(function(e){
            e.preventDefault();
            var gameId = $(this).attr("data-joinGameId");

            window.alert(gameId);
            me._hub.server.joinGame(gameId).done(() => {
                //window.alert("success");
            }).fail(error => {
                //window.alert(error);
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
}

class Player {
    Id: number;
    Name: string;
    GameId: number;
} 

$(() => {
    new Games();
}); 