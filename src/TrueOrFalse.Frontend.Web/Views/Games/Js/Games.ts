class Games {

    private _hub: any;
    private _divGameRowsReady: JQuery;
    private _divGameRowsReadyNone: JQuery;

    constructor() {

        var me = this;

        this.InitializeCountdownAll();
        this.InitializeButtonsAll();

        this._hub = $.connection.gameHub;
        this._divGameRowsReady = $("#divGameRowsReady");
        this._divGameRowsReadyNone = $("#divGameRowsReadyNone");

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = (player: Player) => {
            me.GetRow(player.GameId).AddPlayer(player.Name, player.Id);
        };

        this._hub.client.NextRound = (game: Play) => {
            window.console.log(game);
        };

        this._hub.client.Created = (game: Game) => {

            this._divGameRowsReady.show();
            this._divGameRowsReadyNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId,
                htmlResult => {
                    this._divGameRowsReady.append(
                        $(htmlResult)
                            .animate({ opacity: 0.00 }, 0)
                            .animate({ opacity: 1.00 }, 700)
                    );
                    this.InitializeRow(game.GameId);
                }
            );
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });
    }

    InitializeRow(gameId : number) {
        this.InitializeButtons("[data-gameId=" + gameId + "] [data-joinGameId]");
        this.InitializeCountdown("[data-gameId=" + gameId + "] [data-countdown]");
    }

    InitializeButtonsAll(){ this.InitializeButtons("[data-joinGameId]"); }
    InitializeCountdownAll(){ this.InitializeButtons("[data-countdown]"); }

    InitializeButtons(selector : string) {
        var me = this;

        $(selector).click(function(e){
            e.preventDefault();

            var gameId = +$(this).attr("data-joinGameId");
            window.console.log(gameId);
            me._hub.server.joinGame(gameId).done(() => {}).fail(error => {
                window.alert(error);
            });
        });
    }

    InitializeCountdown(selector: string) {
        $(selector).each(function () {
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