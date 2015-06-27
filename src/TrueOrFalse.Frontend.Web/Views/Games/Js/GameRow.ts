class GameRow {

    CurrentUserId: number;

    GameId: number;
    Div : JQuery;
    DivPlayers: JQuery;

    ButtonStartGame: JQuery;
    ButtonCancelGame: JQuery;

    constructor(gameId: number) {

        this.CurrentUserId = $("#hddCurrentUserId").val();

        this.GameId = gameId;
        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");

        this.ButtonStartGame = this.Div.find("[data-elem=startGame]");
        this.ButtonCancelGame = this.Div.find("[data-elem=cancelGame]");

        window.console.log(this.ButtonCancelGame);

        this.ButtonStartGame.click((e) => { e.preventDefault(); this.StartGame(); });
        this.ButtonCancelGame.click((e) => { e.preventDefault(); this.CancelGame(); });
    }

    AddPlayer(player : Player) {

        if (player.Id === this.CurrentUserId) {
            $(".linkJoin").hide();
            this.Div.find(".spanYouArePlayer").show();
        }

        if (player.TotalPlayers > 1) {
            this.ButtonStartGame.show();
            this.ButtonCancelGame.hide();
        } else {
            this.ButtonStartGame.hide();
            this.ButtonCancelGame.show();            
        }

        this.DivPlayers.append(
            $("<i class='fa fa-user'></i> " +
              "<a href='/Nutzer/" + player.Name + "/"+ player.Id +"'>"+ player.Name +"</a> ")
        );
    }

    StartGame() {
        var gameId = this.GameId;
        $.post("/Games/StartGame", { gameId: gameId.toString() });

        window.location.href = this.Div.find("[data-elem=urlGame]").attr("href");
    }

    CancelGame() {
        var gameId = this.GameId;
        $.post("/Games/CancelGame", { gameId: gameId.toString() });
    }

    ChangeTime(newTime: string) {
        window.console.log("newTime: " + newTime);
        $("[data-countdown]").attr("data-countdown", newTime);
    }
 }