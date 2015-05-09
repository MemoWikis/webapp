<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase game-row" style="position: relative;">
    <div class="col-xs-12 header">
        <h4>
            Quiz (1 / 27) (Startet in spät. 30 sec)
        </h4>
    </div>
    <div class="col-xs-12">
        Gespielt wird mit:
        <div style="display: inline; position: relative; top: -2px;" >
            <%  foreach(var set in Model.Sets){ %>
                <a href="<%= Links.SetDetail(Url, set) %>">
                    <span class="label label-set"><%= set.Name %></span>
                </a>
            <% } %>
        </div>
    </div>
    <div class="col-xs-12">
        Spieler:
        <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.Creator.Name %></a>
        <%  foreach(var player in Model.Players){ %>
            <a href="<%= Links.UserDetail(Url, player) %>"><%= player.Name %></a>
        <% } %>                             
    </div>
    <div class="col-xs-12">
        <% if(Model.Status == GameStatus.InProgress){ %>
            <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-info btn-xs" style="float:right">
                Zusehen
            </a>
        <% }else{ %>
            <a href="<%= Links.GamePlay(Url, Model.GameId) %>" class="btn btn-success btn-xs" style="float:right">
                Mitspielen
            </a>        
        <% } %>
    </div>
</div>