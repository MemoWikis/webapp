<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameReadyModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-sm-6" style="font-size: 17px; vertical-align: bottom; line-height: 48px;">
        <% if(Model.IsCreatorOfGame){ %>
            Du bist der Ersteller <i class="fa fa-smile-o"></i>
        <% }else if(Model.IsInGame){ %>
            Du nimmst an diesem Spiel teil <i class="fa fa-smile-o"></i>
        <% } else { %>
            <a href="<%= Links.GamePlay(Url, Model.GameId) %>"
                data-joinGameId="<%= Model.GameId %>" style=""
                class="btn btn-success btn-sm margin-bottom-sm linkJoin">
                <i class="fa fa-play-circle"></i>&nbsp; Mitspielen
            </a>
        <% } %>
    </div>
    <div class="col-sm-6 text-right text-left-sm" style="text-align: right; font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <i class="fa fa-user show-tooltip" data-original-title="<%= player.User.Name %>"></i>
        <% } %>
    </div>
</div>

<div class="row">
    <div class="col-xs-12" style="padding-top: 30px; font-size: 50px;">
        <span style="font-size: 22px; margin-right: 10px;">Start in </span>
        <span style="font-weight: bolder"
            data-remainingSeconds="<%= Model.RemainingSeconds %>"></span>
    </div>            
</div>