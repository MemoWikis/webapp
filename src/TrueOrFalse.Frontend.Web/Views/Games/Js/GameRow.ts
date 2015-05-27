﻿class GameRow {

    CurrentUserId: number;

    GameId: number;
    Div : JQuery;
    DivPlayers : JQuery;

    constructor(gameId: number) {

        this.CurrentUserId = $("#hddCurrentUserId").val();

        this.GameId = gameId;
        this.Div = $("[data-gameId=" + gameId + "]");
        this.DivPlayers = this.Div.find("[data-row-type=players]");
    }

    AddPlayer(name: string, id: number) {

        window.console.log(this.CurrentUserId);
        window.console.log(id);

        if (id == this.CurrentUserId) {

            $(".linkJoin").hide();
            this.Div.find(".spanYouArePlayer").show();
        }

        this.DivPlayers.append(
            $("<i class='fa fa-user'></i> " +
              "<a href='/Nutzer/" + name + "/"+ id +"'>"+ name +"</a> ")
        );
    }
 }