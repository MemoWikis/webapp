<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameInProgressPlayer>" %>


<div class="row">
    <div class="col-sm-3" style="font-size: 17px; line-height: 33px;">
        Runde
        <span id="CurrentRoundNum"><%= Model.CurrentRoundNum %></span> von 
        <span id="RoundCount"><%= Model.RoundCount %></span>
    </div>
    <div class="col-sm-3">
        <div class="progress" style="position: relative; top: 7px;">
            <div class="progress-bar progress-bar-striped active" role="progressbar" id="progressRound"
                    aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: 1%">
                <span id="remainingTime" style="margin-left: 2px; color:white">14sec</span>
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