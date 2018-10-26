<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeModel>"%>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Styles.Render("~/bundles/_dashboard") %>

<script>
    $(function () {
        $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#3e7700', '#B13A48'] });
        $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
        $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
    });
</script>
<script>
       
         if (isGoogleApiInitialized)
             Initialize();
         else
             google.setOnLoadCallback(Initialize);

         function Initialize() {
             drawKnowledgeChart("chartKnowledgeP");
             drawActivityChart();
             <% for (var i = 0; i < Model.Dates.Count; i++){ %>
 
                 drawKnowledgeChartDate("chartKnowledgeDate" + <%= (i+1) %>,
                     <%= Model.DateRowModelList[i].KnowledgeSolid %>,
                     <%= Model.DateRowModelList[i].KnowledgeNeedsConsolidation %>,
                     <%= Model.DateRowModelList[i].KnowledgeNeedsLearning %>,
                     <%= Model.DateRowModelList[i].KnowledgeNotLearned %>);
            <%}%>
         }


         function drawKnowledgeChart(chartElementId) {
             if ($("#" + chartElementId).length === 0) {
                 return;
             }

             var data = google.visualization.arrayToDataTable([
                 ['Wissenslevel', 'link', 'Anteil in %'],
                 ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', <%= Model.KnowledgeSummary.Solid %>],
                 ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', <%= Model.KnowledgeSummary.NeedsConsolidation %>],
                 ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', <%= Model.KnowledgeSummary.NeedsLearning %>],
                 ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', <%= Model.KnowledgeSummary.NotLearned %>],
             ]);

             var options = {
                 pieHole: 0.6,
                 tooltip: { isHtml: true },
                 legend: { position: 'labeled' },
                 pieSliceText: 'none',
                 chartArea: { 'width': '100%', height: '100%', top: 10 },
                 slices: {
                     0: { color: '#afd534' },
                     1: { color: '#fdd648' },
                     2: { color: 'lightsalmon' },
                     3: { color: 'silver' }
                 },
                 pieStartAngle: 0
             };

             var view = new google.visualization.DataView(data);
             view.setColumns([0, 2]);

             var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
             chart.draw(view, options);

             google.visualization.events.addListener(chart, 'select', selectHandler);

             function selectHandler(e) {
                 var urlPart = data.getValue(chart.getSelection()[0].row, 1);
                 location.href = urlPart;
             }
         }

         function drawKnowledgeChartDate(chartElementId, amountSolid, amountToConsolidate, amountToLearn, amountNotLearned) {

             var chartElement = $("#" + chartElementId);

             var data = google.visualization.arrayToDataTable([
                 ['Wissenslevel', 'Anteil in %'],
                 ['Sicheres Wissen', amountSolid],
                 ['Solltest du festigen', amountToConsolidate],
                 ['Solltest du lernen', amountToLearn],
                 ['Noch nicht gelernt', amountNotLearned],
             ]);

             var options = {
                 pieHole: 0.5,
                 legend: { position: 'none' },
                 pieSliceText: 'none',
                 height: 80,
                 chartArea: { width: '100%', height: '100%', top: 10 },
                 slices: {
                     0: { color: '#afd534' },
                     1: { color: '#FFD603' },
                     2: { color: '#FF001F' },
                     3: { color: 'silver' }
                 },
                 pieStartAngle: 0
             };

             var chart = new google.visualization.PieChart(chartElement.get()[0]);
             chart.draw(data, options);
         }

         function drawActivityChart() {
             var data = google.visualization.arrayToDataTable([
                 [
                     'Datum', 'Richtig beantwortet', 'Falsch beantwortet', { role: 'annotation' }
                 ],
                 <% foreach (var stats in Model.Last30Days)
         { %>
                 <%= "['" + stats.DateTime.ToString("dd.MM") + "', " + stats.TotalTrueAnswers + ", "+ stats.TotalFalseAnswers +", '']," %> 
                 <% } %>
             ]);

             var view = new google.visualization.DataView(data);
             view.setColumns([0, 1,
                 {
                     calc: "stringify",
                     sourceColumn: 1,
                     type: "string",
                     role: "annotation"
                 },
                 2]);

             var options = {
                 legend: { position: 'top', maxLines: 30 },
                 tooltip: { isHtml: true },
                 bar: { groupWidth: '89%' },
                 chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom: -10 },
                 colors: ['#afd534', 'lightsalmon'],
                 isStacked: true
             };

             
             <% if (!Model.HasLearnedInLast30Days)
         { %>
             var infoDivNotLearned = document.createElement('div');
             infoDivNotLearned.setAttribute('style', ' margin-top: 50px; left: 20px; right: 20px; height: 150px; padding-top: 40px');
             infoDivNotLearned.setAttribute('class', 'alert alert-info');
             infoDivNotLearned.innerHTML = '<p>Du hast in den letzten 30 Tagen keine Fragen beantwortet, daher kann hier keine Übersicht angezeigt werden.</p>';
             document.getElementById("chartActivityLastDays").appendChild(infoDivNotLearned);
             <% }
                else
                {%>
                    var chart = new google.visualization.ColumnChart(document.getElementById("chartActivityLastDays"));
                    chart.draw(view, options);
               <% } %>
         }
     </script>
<link rel="stylesheet" type="text/css" href="//fonts.googleapis.com/css?family=Open+Sans" />
<input type="hidden" id="hddCountDates" value="<%=Model.Dates.Count %>"/>
<input type="hidden" id="hddUserId" value="<%=Model.UserId %>"/>

<div class="row first-row">
<div class="col-xs-3 " >
    <h3>Dein Wissenstand</h3>
    <div id="chartKnowledgeP" ></div>
</div>
<!-- Dein Training -->
<div class ="col-xs-5">
    <h3> Dein Training</h3>
    <div id="chartActivityLastDays"></div>
</div>
<div class="col-xs-4">
    <h3> Dein Wunschwissen</h3>
    <div><i class="bold"><%=Model.TopicCount %></i> Themen <i class="bold"><%=Model.User.WishCountSets %></i> Lernsets <i class="bold"><%=Model.User.WishCountQuestions %></i> Fragen</div> 
</div>
</div>

<div class="row second-row">
<div class="col-xs-3">
    <span>
        <h3>Deine Reputation</h3>
        <p>
            <a href="<%= Links.UserDetail(Model.User) %>">Details auf deiner Profilseite</a>
        </p>
        <p>
            <b class="reputation-number"><%= Model.TopicCreatedCount %></b><span>erstellte Themen</span><br/>
            <b class="reputation-number"><%= Model.SetsCreatedCount %></b><span>erstellte Lernsets</span><br/>
            <b class="reputation-number"><%= Model.QuestionsCreatedCount %></b><span>erstellte Fragen</span> <br/>
            <h3 id="rang">Rang <%=Model.ReputationRank %></h3>
            <b>(<%= Model.ReputationTotal %> ReputationsPunkte)</b>
            </p>
     <%--   <p>
            <i class="fa fa-question-circle show-tooltip" data-original-title="Reputationspunkte erhältst du, wenn du gute Lerninhalte erstellst und andere damit lernen."></i>
            <br/>
        </p>--%>
    </span>
</div>

<div class="col-xs-5">
    <h3 >Dein erreichtes Level</h3>
    <div  class="learn-points">
        <span class="level-display">
            <span style="display: inline-block; white-space: nowrap;">
                <svg class="">
                    <circle cx="50%" cy="50%" r="50%" />
                    <text class="level-count" x="50%" y="50%" dy = ".34em" ><%= Model.ActivityLevel %></text>
                </svg>
            </span>
        </span>
        <p class="textPointsAndLevel">
            Mit <b><%= Model.ActivityPoints.ToString("N0") %> Lernpunkten</b> bist du in <span style="white-space: nowrap"><b>Level <%= Model.ActivityLevel %></b>.</span>
        </p>
    </div>
    <div class="row">
        <div class="NextLevelContainer">
            <div class="ProgressBarContainer">
                 <div id="NextLevelProgressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.ActivityPointsPercentageOfNextLevel %>%;">
                    <div class="ProgressBarSegment ProgressBarLegend">
                        <span id="NextLevelProgressSpanPercentageDone"><%= Model.ActivityPointsPercentageOfNextLevel %> %</span>
                    </div>
                </div>
                <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div> 
            </div>
        </div>
    </div>
</div>
<div class="col-xs-4">
    <h3>Im Netzwerk</h3>
        <p class="greyed" style="font-size: 12px;"><a href="<%= Url.Action("Network", "Users") %>">Zu deinem Netzwerk</a></p>
        <% if (Model.NetworkActivities.Count == 0)
           { %>
                Keine Aktivitäten in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>. 
                Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
        <% }
           else
           { %>
            <% foreach (var activity in Model.NetworkActivities)
               { %>
                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-xs-3">
                        <a href="<%= Links.UserDetail(activity.UserCauser) %>">
                        <img class="ItemImage" src="<%= new UserImageSettings(activity.UserCauser.Id).GetUrl_128px_square(activity.UserCauser).Url %>" />
                        </a>
                    </div>
                    <div class="col-xs-9" id="textNetzwerk" >
                        <div class="greyed" style="font-size: 10px; margin: -4px 0;">vor <%= DateTimeUtils.TimeElapsedAsText(activity.At) %></div>
                        <div  style="clear: left;" >
                            <a href="<%= Links.UserDetail(activity.UserCauser) %>"><%= activity.UserCauser.Name %></a> <%= UserActivityTools.GetActionDescription(activity) %>
                            <%= UserActivityTools.GetActionObject(activity) %>
                        </div>
                    </div>
                </div>
            <% } %>
            <div class="row" style="opacity: 0.4;">
                <div class="col-xs-12"><a class="featureNotImplemented">mehr...</a></div>
            </div>
        <% } %>
</div>
</div>

<%= Scripts.Render("~/bundles/js/_dashboard") %>


