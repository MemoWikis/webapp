class GameRow {

    CurrentUserId: number;

    IsCreator: boolean;
    IsPlayer: boolean;
    IsCreatorOrPlayer : boolean;

    GameId: number;
    Div : JQuery;
    DivPlayers: JQuery;

    ButtonStartGame: JQuery;
    ButtonCancelGame: JQuery;
    ButtonJoinGame: JQuery;
    ButtonLeaveGame: JQuery;

    SpanYouArePlayer: JQuery;

    private _hub: any;

    constructor(gameId: number, hub : any) {

        this.GameId = gameId;
        this._hub = hub;

        this.CurrentUserId = $("#hddCurrentUserId").val();

        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");

        this.ButtonStartGame = this.Div.find("[data-elem=startGame]");
        this.ButtonCancelGame = this.Div.find("[data-elem=cancelGame]");
        this.ButtonJoinGame = this.Div.find("[data-elem=joinGame]");
        this.ButtonLeaveGame = this.Div.find("[data-elem=leaveGame]");

        this.ButtonStartGame.click((e) => { e.preventDefault(); this.StartGame(); });
        this.ButtonCancelGame.click((e) => { e.preventDefault(); this.CancelGame(); });
        this.ButtonJoinGame.click((e) => {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg();
                return;
            }
             e.preventDefault(); this.JoinGame();
        });
        this.ButtonLeaveGame.click((e) => { e.preventDefault(); this.LeaveGame(); });

        this.IsCreator = this.Div.attr("data-isCreator") === "True";
        this.IsPlayer = this.Div.attr("data-isPlayer") === "True";
        this.IsCreatorOrPlayer = this.IsCreator || this.IsPlayer;

        window.console.log("isPlayer -> " + this.IsPlayer);

        this.SpanYouArePlayer = this.Div.find(".spanYouArePlayer");
    }

    UiAddPlayer(player : Player) {

        if (player.UserId == this.CurrentUserId) {
            this.ButtonJoinGame.hide();
            this.ButtonLeaveGame.show();
            this.Div.find(".spanYouArePlayer").show();
            this.IsPlayer = true;

            SiteMessages.ShowStartBox();
        }

        if (this.IsCreator) {
            if (player.TotalPlayers > 1) {
                this.ButtonStartGame.show();
                this.ButtonCancelGame.hide();
            } else {
                this.ButtonStartGame.hide();
                this.ButtonCancelGame.show();
            }            
        }

        this.DivPlayers.append(
            $("<i class='fa fa-user' data-playerUserId='" + player.UserId + "'></i> " +
              "<a href='/Nutzer/" + player.Name + "/"+ player.UserId +"' data-playerUserId='" + player.UserId + "'>"
                 + player.Name +
              "</a> ")
        );
    }

    UiRemovePlayer(playerUserId: number) {
        $("[data-playerUserId=" + playerUserId + "]").hide();

        if (this.IsPlayer) {
            this.ButtonJoinGame.show();
            this.ButtonLeaveGame.hide();
            this.SpanYouArePlayer.hide();

            this.IsPlayer = false;
        }

        if (this.IsCreator) {
            this.ButtonStartGame.hide();
            this.ButtonCancelGame.show();
        }

    }

    StartGame() {
        var gameId = this.GameId;
        $.post("/Games/StartGame", { gameId: gameId.toString() });

        window.location.href = this.Div.find("[data-elem=urlGame]").attr("href");
    }

    JoinGame() {
        this._hub.server.joinGame(this.GameId).done(() => {
        }).fail(error => {
            window.alert(error);
        });
    }

    LeaveGame() {
        this._hub.server.leaveGame(this.GameId).done(() => {
            SiteMessages.StopAndHide();
        }).fail(error => {
            window.alert(error);
        });
    }

    CancelGame() {
        var gameId = this.GameId;
        $.post("/Games/CancelGame", { gameId: gameId.toString() });
        SiteMessages.StopAndHide();
    }

    ChangeTime(newTime: string) {
        window.console.log("newTime: " + newTime);
        $("[data-countdown]").attr("data-countdown", newTime);
    }
 }