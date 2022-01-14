﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% 
    var userSession = new SessionUser();
    var user = userSession.User;
    if (userSession.IsLoggedIn)
    {
        var activityLevel = user.ActivityLevel;
        var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
        var activityPointsTillNextLevel = activityPointsAtNextLevel - user.ActivityPoints;
        var activityPointsPercentageOfNextLevel = user.ActivityPoints == 0 ? 0 : 100 * user.ActivityPoints / activityPointsAtNextLevel;

%>
    <div id="activity-popover-content">
        <div id="activity-popover-content-inner">
            Mit <b><span id="UserActivityPoints"><%= userSession.User.ActivityPoints.ToString("N0") %></span> Lernpunkten</b> <br/> 
            <span style="white-space: nowrap;  display: block;">bist du in <b>Level <%= userSession.User.ActivityLevel %></b>.</span>
            <div class="NextLevelContainer">
                <div class="ProgressBarContainer">
                    <div id="NextLevelProgressPercentageDone" <%if(activityPointsPercentageOfNextLevel == 0){%> style="border-right: none;" <%} %> class="ProgressBarSegment ProgressBarDone" style="width: <%= activityPointsPercentageOfNextLevel %>%;">
                        <div class="ProgressBarSegment ProgressBarLegend">
                            <span id="NextLevelProgressSpanPercentageDone"><%= activityPointsPercentageOfNextLevel %> %</span>
                        </div>
                    </div>
                    <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
                </div>
            </div>
            <div class="ProgressInfoText">Noch <span id="ProgressToNextLevel"><%= activityPointsTillNextLevel.ToString("N0") %> </span>Punkte <br/> bis Level <span id="NextActivityLevel"><%= userSession.User.ActivityLevel + 1 %></span></div>
        </div>
    </div>
<% } %> 