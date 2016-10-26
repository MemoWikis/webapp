<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="alert alert-success" role="alert" 
    id="divMsgPartOfGame" style="margin-top: 2px; margin-bottom: 10px; padding: 7px;">
    In <span style="font-weight: bold" 
        data-remainingSeconds="<%= Model.UpcomingGame.RemainingSeconds() %>"
        data-game-url="<%= Links.GamePlay(Url, Model.UpcomingGame.Id) %>"
        ></span> 
    startet ein <a style="font-weight: bold" href="<%= Links.GamePlay(Url, Model.UpcomingGame.Id) %>">Spiel</a>, 
    <% if(Model.IsCreatorOfGame){ %>
        das du erstellt hast.
    <% } else { %>
        an dem Du teilnimmst.
    <% } %>
</div>