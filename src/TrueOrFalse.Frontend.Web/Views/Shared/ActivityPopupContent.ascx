<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

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
            Mit <b><%= userSession.User.ActivityPoints.ToString("N0") %> Lernpunkten</b> 
            <span style="white-space: nowrap">bist du <b>Level <%= userSession.User.ActivityLevel %></b>.</span>
            <div class="NextLevelContainer">
                <div class="ProgressBarContainer">
                    <div id="NextLevelProgressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= activityPointsPercentageOfNextLevel %>%;">
                        <div class="ProgressBarSegment ProgressBarLegend">
                            <span id="NextLevelProgressSpanPercentageDone"><%= activityPointsPercentageOfNextLevel %> %</span>
                        </div>
                    </div>
                    <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>

                </div>
            </div>
            <div class="ProgressInfoText">Noch <%= activityPointsTillNextLevel.ToString("N0") %> Punkte bis Level <%= userSession.User.ActivityLevel + 1 %></div>
        </div>
    </div>
<% } %> 