<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameCompletedModel>" %>



<div class="rowBase">
    <div class="col-xs-12">
        
        <h4>Spiel vorbei<% if(Model.IsPlayer){ %>: 
            <% if(Model.PlayerPosition == 1) { %><i class="fa fa-trophy" style="color: gold"></i><% } %>
            Du bist <%=Model.PlayerPosition %>. geworden! <% } %></h4>
    </div>
</div>

<div class="row" style="padding-left: 7px;">
    <div class="col-xs-12">
        <h4>Rangliste:</h4>
    </div>    
</div>

<% foreach(var playerRow in Model.Rows){ %>
    <div class="rowBase">
        <div class="col-xs-12">
            <div class="row">
                <div class="col-xs-12">
                    <h4>Platz <%= playerRow.Position %>: <%= playerRow.IsCurrentUser ? "(Du)" : "" %> <%: playerRow.PlayerName %></h4>
                </div>        
            </div>
            <div class="row">
                <div class="col-sm-3">
                    Von <%= playerRow.TotalQuestions %> Fragen beantwortet:
                </div>
                <div class="col-sm-3" style="background-color: lightgreen; color: darkgreen">
                    <%= playerRow.TotalCorrect %> Richtig
                </div>
                <div class="col-sm-3" style="background-color: lightsalmon; color: darkred">
                    <%= playerRow.TotalWrong %> Falsch
                </div>
                <div class="col-sm-3" style="background-color: silver">
                    <%= playerRow.TotalNotAnswered %> Nicht
                </div>        
            </div>
        </div>
    </div>
<% } %>