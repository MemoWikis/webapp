﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameInProgressPlayerModel>" %>

<input type="hidden" id="hddQuestionId" value="<%= Model.Question.Id %>"/>
<input type="hidden" id="hddRoundEnd" value="<%= Model.RoundEndTime.ToString("yyyy-MM-dd HH:mm:ss") %>"/>
<input type="hidden" id="hddRoundLength" value="<%= Model.RoundLength %>"/>
<input type="hidden" id="hddRound" value="<%= Model.RoundNum %>"/>
<input type="hidden" id="hddPlayerId" value="<%= Model.Player.Id %>"/>


<div class="row">
    <div class="col-sm-3" style="font-size: 17px; line-height: 33px;">
        Runde
        <span id="CurrentRoundNum"><%= Model.RoundNum %></span> von 
        <span id="RoundCount"><%= Model.RoundCount %></span>
    </div>
    <div class="col-sm-3">
        <div class="progress" style="position: relative; top: 7px;" id="divRemainingTime">
            <div class="progress-bar progress-bar-striped active" role="progressbar" id="progressRound"
                aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                <span id="spanRemainingTime" style="margin-left: 2px; color:white"></span>
            </div>
        </div>
    </div>
    <div class="col-sm-6 text-right text-left-sm" style="font-size: 30px;">
        <% foreach(var player in Model.Players){ %>
            <div style="display: inline-block; text-align: left; ">
                <div style="float: right;">
                    
                    <div style="font-size: 10px;">
                        <%: player.User.Name %>
                    </div>                
                    <div style="font-size: 10px;">
                        21 Punkte
                    </div>
                </div>
                <div style="float:right">
                    <i style="margint-top: 0px;" class="fa fa-user show-tooltip" data-original-title="<%= player.User.Name %>"></i>    
                </div>
            </div>
        <% } %>
    </div>
</div>

<div id="divBodyAnswer">
    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", 
            new AnswerBodyModel(Model.Question, Model.Game, Model.Player, Model.Round)); %>
</div>