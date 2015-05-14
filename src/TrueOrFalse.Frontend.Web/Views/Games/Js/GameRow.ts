class GameRow {

    GameId: number;
    Div : JQuery;
    DivPlayers : JQuery;

    constructor(gameId: number) {
        this.GameId = gameId;
        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");
    }

    AddPlayer(name: string, id: number) {
        this.DivPlayers.append(
            $("<i class='fa fa-user'></i> " +
              "<a href='/Nutzer/" + name + "/"+ id +"'>"+ name +"</a> ")
        );
    }
 }