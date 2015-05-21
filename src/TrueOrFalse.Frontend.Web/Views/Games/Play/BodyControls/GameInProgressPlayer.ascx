<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameInProgressPlayerModel>" %>


<div class="row">
    <div class="col-sm-3" style="font-size: 17px; line-height: 33px;">
        Runde
        <span id="CurrentRoundNum"><%= Model.CurrentRoundNum %></span> von 
        <span id="RoundCount"><%= Model.RoundCount %></span>
    </div>
    <div class="col-sm-3">
        <div class="progress" style="position: relative; top: 7px;" id="divRemainingTime">
            <div class="progress-bar progress-bar-striped active" role="progressbar" id="progressRound"
                aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                <span id="spanRemainingTime" style="margin-left: 2px; color:white"></span>
            </div>
        </div>
    </div>
    <div class="col-sm-6 text-right text-left-sm" style="font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <i class="fa fa-user show-tooltip" data-original-title="<%= player.Name %>"></i>
        <% } %>
    </div>
</div>

<h2>Das Spiel läuft</h2>

Du bist Mitspieler.