<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameReadyModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row" style="font-size: 17px; vertical-align: bottom;">
    <div class="col-sm-4">
        <% if(Model.IsCreatorOfGame){ %>
            Du spielst mit und bist der Ersteller.
        <% }else if(Model.IsInGame){ %>
            Du spielst mit.
        <% } else { %>
            <a href="<%= Links.GamePlay(Url, Model.GameId) %>"
                data-joinGameId="<%= Model.GameId %>" style=""
                class="btn btn-success">
                <i class="fa fa-play-circle"></i>&nbsp; Mitspielen
            </a>
        <% } %>
    </div>
    <div class="col-sm-8 text-right text-left-sm" style="margin-top: 10px;">
        Mitspieler: 
        <% foreach(var player in Model.Players){ %>
            <span style="white-space: nowrap">
                <i class="fa fa-user"></i> <%= player.User.Name %>
            </span>
        <% } %>
    </div>
</div>

<div class="row">
    <div class="col-xs-12" style="padding-top: 30px; font-size: 50px;">
        <span style="font-size: 22px; margin-right: 10px;">Start in </span>
        <span style="font-weight: bolder" data-remainingSeconds="<%= Model.RemainingSeconds %>"></span>
    </div>          

    <div class="col-xs-12" style="margin-top: 20px;">
        <div class="alert alert-info">
            <p>
                <strong>Geht nicht weiter?</strong> Falls die Zeit abgelaufen ist und nichts passiert, <a href="<%= Links.GamePlay(Url, Model.GameId) %>">lade diese Seite erneut</a>!
            </p>
        </div>  
    </div>
</div>