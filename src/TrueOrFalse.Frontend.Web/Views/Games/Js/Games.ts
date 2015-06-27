class Games {

    private _hub: any;
    private _divGamesReady: JQuery;
    private _divGamesReadyNone: JQuery;

    private _divGamesInProgress: JQuery;
    private _divGamesInProgressNone: JQuery;

    private _gameRows : GameRow[] = [];

    constructor() {

        var me = this;

        this._hub = $.connection.gameHub;

        this._divGamesReady = $("#divGamesReady");
        this._divGamesReadyNone = $("#divGamesReadyNone");

        this._divGamesInProgress = $("#divGamesInProgress");
        this._divGamesInProgressNone = $("#divGamesInProgressNone");

        this.InitializeCountdownAll();
        this.InitializeButtonsAll();
        this.InitRows();

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = (player: Player) => {
            me.GetRow(player.GameId).AddPlayer(player);
        };

        this._hub.client.NextRound = (game: Game) => {
            var currentRowSelector = "[data-gameId=" + game.GameId + "] [data-elem = currentRound]";
            Utils.SetElementValue(
                currentRowSelector,
                game.RoundNumber.toString());

            $("[data-gameId=" + game.GameId + "] [data-elem=currentRoundContainer]")
                .attr("data-original-title", game.RoundNumber + " Runden von " + game.GameRoundCount + " gespielt");

            $(currentRowSelector + " .show-tooltip").tooltip();
        };

        this._hub.client.Created = (game: Game) => {

            this._divGamesReady.show();
            this._divGamesReadyNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId,
                htmlResult => {
                    this._divGamesReady.append(
                        $(htmlResult)
                            .animate({ opacity: 0.00 }, 0)
                            .animate({ opacity: 1.00 }, 700)
                    );
                    this.InitializeRow(game.GameId);
                }
            );
        };

        this._hub.client.Started = (game: Game) => {

            this._divGamesInProgress.show();
            this._divGamesInProgressNone.hide();

            $.get("/Games/RenderGameRow/?gameId=" + game.GameId,
                htmlResult => {

                    $("[data-gameId=" + game.GameId + "]")
                        .hide(700)
                        .remove();

                    this._divGamesInProgress.append(
                        $(htmlResult)
                            .animate({ opacity: 0.00 }, 0)
                            .animate({ opacity: 1.00 }, 700));

                    this.InitializeRow(game.GameId);

                    if (this._divGamesReady.find("[data-gameId]").length == 0) {
                        this._divGamesReadyNone.show();
                    }
                }
            );
        };

        this._hub.client.Completed = (game: Game) => {
            $("[data-gameId=" + game.GameId + "]")
                .hide(700)
                .remove();

            this.IfNeeded_ShowNoGamesInProgressInfo();
        }

        this._hub.client.NeverStarted = (game: Game) => {
            $("[data-gameId=" + game.GameId + "]")
                .hide(700)
                .remove();

            this.IfNeeded_ShowNoGamesReadyInfo();
        }

        this._hub.client.Canceled = (cancel) => {
            $("[data-gameId=" + cancel.GameId + "]")
                .hide(700)
                .remove();

            this.IfNeeded_ShowNoGamesReadyInfo();
        };

        this._hub.client.ChangeStartTime = (changeStartTime) => {
            var row = me.GetRow(changeStartTime.GameId);
            row.ChangeTime(changeStartTime.WillStartAt);
            this.InitializeCountdown("[data-gameId=" + changeStartTime.GameId + "] [data-countdown]");
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });
    }

    IfNeeded_ShowNoGamesInProgressInfo() {
        if (this._divGamesInProgress.find("[data-gameId]").length == 0) {
            this._divGamesInProgressNone.show();
        }        
    }

    IfNeeded_ShowNoGamesReadyInfo() {
        if (this._divGamesReady.find("[data-gameId]").length === 0) {
            this._divGamesReadyNone.show();
        }        
    }

    InitializeRow(gameId : number) {
        this.InitializeButtons("[data-gameId=" + gameId + "] [data-joinGameId]");
        this.InitializeCountdown("[data-gameId=" + gameId + "] [data-countdown]");
        $(".show-tooltip").tooltip();

        this._gameRows.push(new GameRow(gameId));
    }

    InitializeButtonsAll(){ this.InitializeButtons("[data-joinGameId]"); }
    InitializeCountdownAll() { this.InitializeCountdown("[data-countdown]"); }

    InitializeButtons(selector : string) {
        var me = this;

        $(selector).click(function(e){
            e.preventDefault();

            var gameId = +$(this).attr("data-joinGameId");

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

    InitRows() {
        var self = this;
        $("[data-gameId]").each(function() {
            self._gameRows.push(
                new GameRow(parseInt($(this).attr("data-gameId")))
            );
        });

        window.console.log(self._gameRows);
    }

    GetRow(gameId: number): GameRow{
        return $.grep(this._gameRows, function (row : GameRow) {
            return row.GameId === gameId;
        })[0];
    }
}

class Player {
    Id: number;
    Name: string;
    GameId: number;
    TotalPlayers : number;
} 

$(() => {
    new Games();
}); 