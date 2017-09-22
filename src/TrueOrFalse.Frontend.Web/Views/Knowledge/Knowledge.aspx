<%@ Page Title="Wissenszentrale" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Register Src="~/Views/Knowledge/TrainingDate.ascx" TagPrefix="uc1" TagName="TrainingDate" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.Knowledge() %>">
</asp:Content>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        $(function () {
            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#3e7700', '#B13A48'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#90EE90', '#FFA07A'] });
        });
    </script>
    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate1", 9, 2, 1, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate2", 4, 3, 2, 3) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate3", 1, 12, 4, 12) });
        google.setOnLoadCallback(drawActivityChart);

        //chartKnowledgeDate
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
                chartArea: { 'width': '100%', height: '100%', top: 10},
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
                    3: { color: 'silver'}
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
                chartArea: { width: '90%', height: '90%', top: 0 },
                slices: {
                    0: { color: '#afd534' },
                    1: { color: '#fdd648' },
                    2: { color: 'lightsalmon' },
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
                    <% foreach (var stats in Model.Last30Days){ %>
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
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534', 'lightsalmon'],
                isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartActivityLastDays"));
            chart.draw(view, options);
            <% if (!Model.HasLearnedInLast30Days) { %>
                var infoDivNotLearned = document.createElement('div');
                infoDivNotLearned.setAttribute('style', 'position: absolute; top: 165px; left: 20px; right: 20px;');
                infoDivNotLearned.setAttribute('class', 'alert alert-info');
                infoDivNotLearned.innerHTML = '<p>Du hast in den letzten 30 Tagen keine Fragen beantwortet, daher kann hier keine Übersicht angezeigt werden.</p>';
                document.getElementById("chartActivityLastDays").appendChild(infoDivNotLearned);
            <% } %>

        }
    </script>
    
    <style>
        #totalKnowledgeOverTime{font-size: 18px; line-height:27px ;color: rgb(170, 170, 170);padding-top: 5px;display: inline-block;}
        #totalKnowledgeOverTimeSpark{ display: inline-block;}
        div.answerHistoryRow div{ display: inline-block; height: 22px;}
        div.answerHistoryRow .answerAmount{ color:blue; font-weight: bolder;}
        
        div.column  { width: 260px; float: left;}

        div.percentage{display: inline-block; width: 40px; background-color:beige; height: 22px;vertical-align: top;}
        div.percentage span{ font-size: 22px; color: green; position: relative; top: 2px; left: 4px;}
    </style>

    <%= Styles.Render("~/bundles/Knowledge") %>
    <%= Scripts.Render("~/bundles/js/Knowledge") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.Message != null) { %>
        <div class="row">
            <div class="col-xs-12 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>        
    <% } %>

    <h1 style="margin-bottom: 5px; margin-top: 0px;"><span class="ColoredUnderline Knowledge">Hallo <%= Model.UserName %>!</span></h1>

    <% if(!Model.IsLoggedIn){ %>

        <div class="bs-callout bs-callout-danger">
            <h4>Einloggen oder registrieren</h4>
            <p>
                Um einen Überblick über deine Lernerfolge, deine anstehenden Lernsitzungen und die Lernaktivitäten deiner Freunde zu sehen, 
                musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="<%= Links.Register() %>">registrieren</a>.
            </p>
            <p>
                <b>Registriere dich und probiere es gleich aus!</b>
            </p>
            <p>                        
                <a href="<%= Links.Register() %>" class="btn btn-success" style="margin-top: 0; margin-right: 10px;" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>
                <a href="#" data-btn-login="true">Ich bin schon Nutzer!</a>
                <br/><span style="margin-top: 3px; font-style: italic">memucho ist kostenlos.</span>
            </p>
        </div>
    <% } %>

    <div id="dashboardContent" style="<%= Model.IsLoggedIn ? "" : "pointer-events: none; opacity: 0.3;" %>">
        
        <div class="row">
            <div class="col-sm-6" id="learningPoints">
                <div class="rowBase" style="padding: 10px;">
                    <h3>Deine Lernpunkte</h3>
                    <div style="text-align: center; margin-bottom: 28px; margin-top: 15px;">
                        <span class="level-display" style="float: left; margin-top: -4px;">
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
                    <div class="greyed" style="text-align: center; margin-bottom: 15px;">Noch <%= Model.ActivityPointsTillNextLevel.ToString("N0") %> Punkte bis Level <%= Model.ActivityLevel + 1 %></div>
                </div>
            </div>

            <div class="col-sm-6" id="reputationPoints">
                <div class="rowBase" style="padding: 10px;">
                    <h3>Deine Reputation</h3>
                
                    <p>
                        Reputation: <b><%= Model.ReputationTotal %> Punkte</b>
                        <i class="fa fa-question-circle show-tooltip" data-original-title="Reputationspunkte erhältst du, wenn du gute Lerninhalte erstellst und andere damit lernen."></i>
                        <br/>
                        Position: <%= Model.ReputationRank %><br/>
                        Erstellte Fragen: <%= Model.QuestionsCreatedCount %><br/>
                        Erstellte Lernsets: <%= Model.SetsCreatedCount %>
                    </p>

                    <p class="moreInfoLink">
                        <a href="<%= Links.UserDetail(Model.User) %>">Details auf deiner Profilseite</a>
                    </p>                    
                </div>
            </div>
        </div>


        <div class="row">
                            
            <div class="col-xs-12 col-md-6">
                <div class="rowBase" style="padding: 10px">
                    <h3 style="margin-bottom: 0px; margin-top: 0;">Dein Wissensstand</h3>
                    <p class="greyed" style="font-size: 12px;">Berücksichtigt nur dein Wunschwissen</p>
                    <p style="margin-bottom: 0px;">In deinem Wunschwissen sind:</p>
                    <%--<p>
                        In deinem Wunschwissen sind <%= Model.QuestionsCount %> Frage<%= StringUtils.Plural(Model.QuestionsCount,"n","","n") %> und <%= Model.SetCount %> Lernset<%= StringUtils.Plural(Model.SetCount,"s") %>. 
                        <i class="fa fa-info-circle show-tooltip" title="Erweitere dein Wunschwissen, indem du auf das Herz-Symbol neben einer Frage oder einem Lernset klickst."></i>
                    </p>--%>
                    <div class="row" style="line-height: 30px; margin-bottom: 20px;">
                        <div class="col-md-6">
                            <div class="number-box-questions" style="text-align: center;">
                                <a href="<%= Links.QuestionsWish() %>">
                                    <div>
                                        <span style="font-weight: 900; font-size: 20px;"><%= Model.QuestionsCount %></span>
                                        <span style="font-size: 14px">Frage<%= StringUtils.PluralSuffix(Model.QuestionsCount,"n") %></span>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="number-box-sets" style="text-align: center;">
                                <a href="<%= Links.SetsWish() %>">
                                    <div>
                                        <span style="font-weight: 900; font-size: 20px;"><%= Model.SetsCount %></span>
                                        &nbsp;<span style="font-size: 14px">Lernset<%= StringUtils.PluralSuffix(Model.SetsCount,"s") %></span>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>

                    <% if(Model.KnowledgeSummary.Total == 0) { %>
                        <div class="alert alert-info" style="min-height: 180px; margin-bottom: 54px;">
                            <p>
                                memucho kann deinen Wissensstand nicht zeigen, da du noch kein Wunschwissen hast.
                            </p>
                            <p>
                                Um dein Wunschwissen zu erweitern, suche dir interessante <a href="<%= Links.QuestionsAll() %>">Fragen</a>  
                                oder <a href="<%= Links.SetsAll() %>">Lernsets</a> aus und klicke dort auf das Herzsymbol:
                                <ul style="list-style-type: none">
                                    <li>
                                        <i class="fa fa-heart show-tooltip" style="color:#b13a48;" title="" data-original-title="In deinem Wunschwissen"></i>
                                        In deinem Wunschwissen
                                    </li>                                
                                    <li>
                                        <i class="fa fa-heart-o show-tooltip" style="color:#b13a48;" title="" data-original-title="Nicht Teil deines Wunschwissens."></i>
                                        <i>Nicht</i> in deinem Wunschwissen.
                                    </li>
                                </ul>
                            
                            </p>
                        </div>
                    <% }else { %>
                        <div id="chartKnowledge" style="height: 180px; margin-left: 20px; margin-right: 20px; text-align: left;"></div>
                        <div style="text-align: center; margin-top: 20px;">
                            <a href="<%= Links.StartWishLearningSession() %>" class="btn btn-primary show-tooltip" title="Startet eine persönliche Lernsitzung. Du wiederholst die Fragen aus deinem Wunschwissen, die am dringendsten zu lernen sind.">
                                <i class="fa fa-line-chart">&nbsp;</i>Jetzt Wunschwissen lernen
                            </a>
                        </div>
                    <% } %>
                </div>
            </div>

            <div class="col-xs-12 col-md-6">
                <div class="rowBase" style="padding: 10px; height: 384px;">
                    <h3 style="margin-bottom: 0px; margin-top: 0;">Training</h3>
                    <p class="greyed" style="font-size: 12px;">In den letzten 30 Tagen</p>
                
                    <div id="chartActivityLastDays" style="height: 245px; margin-left: -3px; margin-right: 0px; margin-bottom: 10px; text-align: left;"></div>
                
                    <div class="row" style="font-size: 12px">
                        <div class="col-md-12">
                            <% var streak = Model.StreakDays; %>
                            <!-- -->
                            <span style="display: inline-block; width: 40%">Lerntage gesamt: 
                                <b><%= streak.TotalLearningDays %></b></span> <span class="greyed" style="font-weight: bold;">
                                seit <%= Model.User.DateCreated.ToString("dd.MM.yyyy") %>
                            </span><br />
                        
                            <!-- LongestStreak -->
                            <span style="display: inline-block; width: 40%">
                                Längste Folge: <b><%= streak.LongestLength %></b>
                            </span>
                            <% if (streak.LongestLength == 0){ %>
                                <span class="greyed" style="font-weight: bold;">zuletzt nicht gelernt</span>
                            <% } else { %>
                                <span class="greyed" style="font-weight: bold;">
                                    <%= streak.LongestStart.ToString("dd.MM.") %> - <%= streak.LongestEnd.ToString("dd.MM.yyyy") %>
                                </span><br />
                            <% } %>
                        
                            <!-- CurrentStreak -->
                            <span style="display: inline-block; width: 40%">
                                Aktuelle Folge: <b><%= streak.LastLength %></b>
                            </span>
                            <% if (streak.LastLength == 0){ %>
                                <span class="greyed" style="font-weight: bold;">zuletzt nicht gelernt</span>
                            <% } else { %>
                                <%= streak.LastStart.ToString("dd.MM") %> - <%= streak.LastEnd.ToString("dd.MM.yyyy") %>
                            <% } %>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        
        
        <div id="wishKnowledge" class="rowBase">
            <div class="col-xs-12">
                <h3>Themen und Lernsets in deinem Wunschwissen</h3>
                <div class="row wishKnowledgeNavRow">
                    <% foreach (var catOrSet in Model.CatsAndSetsWish) {
                           if (Model.CatsAndSetsWish.IndexOf(catOrSet) == 6 && Model.CatsAndSetsWish.Count > 8)
                           { %>
                                </div>
                                <div id="wishKnowledgeMore" class="row wishKnowledgeNavRow" style="display: none;">
                           <% } %>
                        <div class="col-xs-6 topic">
                            <% if (catOrSet is Category) {
                                   var category = (Category) catOrSet; %>
                                <div class="row">
                                    <div class="col-xs-3">
                                        <div class="ImageContainer">
                                            <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(category.Name, category.Id)) %>
                                        </div>
                                    </div>
                                    <div class="col-xs-9">
                                        <a class="topic-name" href="<%= Links.GetUrl(category) %>">
                                            <div class="topic-name">
                                                <%: category.Name %>
                                            </div>
                                        </a>
                                        <div class="set-question-count">
                                            <%: Model.GetTotalSetCount(category) %> Lernset<%= StringUtils.PluralSuffix(Model.GetTotalSetCount(category),"s") %>
                                            <%: Model.GetTotalQuestionCount(category) %> Frage<%= StringUtils.PluralSuffix(Model.GetTotalQuestionCount(category),"s") %>
                                        </div>
                                        <div class="KnowledgeBarWrapper">
                                            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                                            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                        </div>
<%--                                        <div class="showSubTopics">
                                            <button data-toggle="collapse" data-target="#agg<%= category.Id %>"><i class="fa fa-caret-down">&nbsp;</i>Zeige aggreg. Unterthemen</button>
                                            <div id="agg<%= category.Id %>" class="collapse">
                                                <% foreach (var aggregatedCategory in category.AggregatedCategories(false))
                                                   {
                                                       Response.Write(aggregatedCategory.Name + " (" + aggregatedCategory.Id + "); ");
                                                   } %>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            <% } else if (catOrSet is Set) {
                                   var set = (Set) catOrSet; %>
                                <div class="row">
                                    <div class="col-xs-3">
                                        <div class="ImageContainer">
                                            <%= Model.GetSetImage(set).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.SetDetail(set)) %>
                                        </div>
                                    </div>
                                    <div class="col-xs-9">            
                                        <a class="topic-name" href="<%= Links.GetUrl(set) %>">
                                            <div class="set-question-count">
                                                Lernset mit <%= set.QuestionCount() /*includes private questions! excluding them would also exclude private questions visible to user*/ %>
                                                Frage<%= StringUtils.PluralSuffix(set.QuestionCount(),"n") %>
                                            </div>
                                            <div class="topic-name">
                                                <%: set.Name %>
                                            </div>
<%--                                            <div class="KnowledgeBarWrapper">
                                                <% Html.RenderPartial("~/Views/QuestionSet/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(set)); %>
                                                <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                            </div>--%>
                                        </a>
                                        
                                    </div>
                                </div>
                            <% } %>
                        </div>
                    <% } %>
                </div>
                <% if (Model.CatsAndSetsWish.Count > 8) { %>
                    <div>
                        <a href="#" id="btnShowAllWishKnowledgeContent" class="btn btn-link btn-lg">Alle anzeigen (<%= Model.CatsAndSetsWish.Count-6 %> weitere) <i class="fa fa-caret-down"></i></a> 
                        <a href="#" id="btnShowLessWishKnowledgeContent" class="btn btn-link btn-lg" style="display: none;"> <i class="fa fa-caret-up"></i> Weniger anzeigen</a>
                    </div>
                <% } %>
            </div>

        </div>

    
        <div class="row" style="margin-top: 20px;">
            <div class="col-xs-12 col-sm-6 col-md-4" style="padding: 5px;">
                <div class="rowBase" id="FutureDatesOverview" style="padding: 10px;">
                    <h3 style="margin-top: 0; margin-bottom: 0;">Termine</h3>
                    <p class="greyed" style="font-size: 12px;"><a href="<%= Links.Dates() %>">Zur Terminübersicht</a></p>
                    <% if (Model.Dates.Count ==0) { %>
                        <p>
                            Du hast momentan keine offenen Termine. Termine helfen dir dabei, dich optimal auf eine Prüfung vorzubereiten.
                        </p>
                        <p>
                            <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm">
                                <i class="fa fa-plus-circle"></i>&nbsp;Termin erstellen
                            </a>
                        </p>
                        <hr style="margin: 5px 0px;"/>
                    <% }else { %>
                        <%
                        var index = 0;    
                        foreach(var date in Model.Dates.Take(3)){
                            index++;
                            %>
                            <div class="row" style="margin-bottom: 3px;">
                                <div class="col-xs-9">
                                    <div style="font-weight: bold; margin-bottom: 3px;"><%= date.GetTitle(true) %></div>
                                    <span style="font-size: 12px;">Noch <%= (date.DateTime - DateTime.Now).Days %> Tage für <%= date.CountQuestions() %> Fragen aus:</span><br />
                                    <% foreach(var set in date.Sets){ %>
                                        <a href="<%= Links.SetDetail(Url, set) %>">
                                            <span class="label label-set" style="font-size: 70%;"><%= set.Name %></span>
                                        </a>                            
                                    <% } %>
                                </div>
                                <div class="col-xs-3" style="opacity: .4;">
                                    <div id="chartKnowledgeDate<%=index %>"></div>
                                </div>
                            </div>  
                            <div class="row">
                                <div class="col-xs-12">
                                    <a href="<%= Links.GameCreateFromDate(date.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="margin-top: 17px; display: inline-block;">
                                        <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                                        Spiel starten
                                    </a>
                                    &nbsp;
                                    <a data-btn="startLearningSession" href="/Termin/Lernen/<%=date.Id %>" style="margin-top: 17px; display: inline-block;">
                                        <i class="fa fa-line-chart"></i> 
                                        Jetzt lernen
                                    </a>
                                </div>                                
                            </div>
                            <hr style="margin: 8px 0;"/>  
                        <% } %>
                        <% if (Model.Dates.Count > 3) { %>
                            <a href="<%= Links.Dates() %>">Du hast <%= (Model.Dates.Count - 3) %> <%= StringUtils.PluralSuffix(Model.Dates.Count - 3,"weitere Termine","weiteren Termin") %></a>
                            <hr style="margin: 8px 0px;"/>
                        <% } %>
                    <% } %>
                    <p>
                        <% if (Model.DatesInNetwork.Count > 0) { %>
                            <a href="<%= Links.Dates() %>"><%= Model.DatesInNetwork.Count %> Termin<%= StringUtils.PluralSuffix(Model.DatesInNetwork.Count,"e") %> in deinem Netzwerk</a>
                            &nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>
                        <% } else {  %>
                            Kein Termin in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>&nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>.
                            Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                        <% } %>
                            
                    </p>
                </div>
                
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0;">Zuletzt gelernt</h3>
                    <% foreach(var answer in Model.AnswerRecent){ 
                        var question = answer.Question;
                    %>
                        <div class="row" style="margin-bottom: 10px;">
                            <div class="col-xs-3">
                                <div class="ImageContainer ShortLicenseLinkText">
                                    <%= ImageFrontendData.Create(question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(question)) %>
                                </div>
                            </div>
                            <div class="col-xs-9" style="">
                                <a href="<%= Links.AnswerQuestion(question) %>"><%= question.Text %></a>
                            </div>
                        </div>
                    <% } %>
                
                    <div class="row" style="opacity: 0.4;">
                        <div class="col-xs-12"><a class="featureNotImplemented">mehr...</a></div>
                    </div>
                </div>
            </div>
            
            <div class="col-xs-12 col-sm-6 col-md-4" style="padding: 5px;">
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0; margin-bottom: 3px;">Lernsitzungen</h3>
                    <% if (Model.TrainingDates.Count ==0) { %>
                        <div class="row" style="margin-bottom: 7px;">
                            <div class="col-md-12">
                                Du hast in den nächsten <b>7 Tagen</b> keine geplanten Lernsitzungen.
                            </div>
                        </div>
                    <% } else { %>
                        <div class="row" style="margin-bottom: 7px;">
                            <div class="col-md-12">
                                in den nächsten <b>7 Tagen</b>
                                <ul>
                                    <li>ca. <%= Model.TrainingDates.Count %> Lernsitzungen</li>
                                    <li>ca. <%= new TimeSpan(0, Model.TrainingDates.Sum(x => x.LearningTimeInMin), 0).ToString(@"hh\:mm") %>h Lernzeit</li>
                                </ul>
                            </div>
                        </div>
                        <% foreach(var trainingDate in Model.TrainingDates) { %>
                            <% Html.RenderPartial("TrainingDate", trainingDate); %>
                        <% } %>
                    <% } %>
                </div>
            </div>
                           
            <div class="col-xs-12 col-sm-6 col-md-4" style="padding: 5px;">
                <div class="rowBase" style="padding: 10px;">
                    <h3 style="margin-top: 0; margin-bottom: 0;">Im Netzwerk</h3>
                    <p class="greyed" style="font-size: 12px;"><a href="<%= Url.Action("Network", "Users") %>">Zu deinem Netzwerk</a></p>

                    <% if (Model.NetworkActivities.Count == 0) { %>
                            Keine Aktivitäten in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>. 
                            Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                    <% } else { %>
                        <% foreach(var activity in Model.NetworkActivities){ %>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <a href="<%= Links.UserDetail(activity.UserCauser) %>">
                                    <img class="ItemImage" src="<%= new UserImageSettings(activity.UserCauser.Id).GetUrl_128px_square(activity.UserCauser).Url %>" />
                                    </a>
                                </div>
                                <div class="col-xs-9" style="">
                                    <div class="greyed" style="font-size: 10px; margin: -4px 0;">vor <%= DateTimeUtils.TimeElapsedAsText(activity.At) %></div>
                                    <div style="clear: left;">
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
        </div>
    
        <div class="row" style="margin-top: 20px;">
            <div class="col-xs-12 col-md-4" style="padding: 5px;">
                
            </div>
        </div>

    </div>

</asp:Content>