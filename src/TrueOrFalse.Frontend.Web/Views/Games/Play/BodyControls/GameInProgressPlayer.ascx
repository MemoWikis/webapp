<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameInProgressPlayer>" %>


<div class="row">
    <div class="col-sm-6" style="font-size: 17px; vertical-align: bottom; line-height: 48px;">
        Runde <span id="CurrentRoundNum"><%= Model.CurrentRoundNum %></span> von 
              <span id="RoundCount"><%= Model.RoundCount %></span>
    </div>
    <div class="col-sm-6 text-right text-left-sm" style="font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <i class="fa fa-user show-tooltip" data-original-title="<%= player.Name %>"></i>
        <% } %>
    </div>
</div>

<h2>Das Spiel läuft</h2>

Du bist Mitspieler.