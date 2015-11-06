<%@ Page Title="Mein Wissensstand" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<KnowledgeModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        $(function () {
            var titles = ['Gewusst', 'Nicht gewusst', 'Unbekannt'];
            $("#totalKnowledgeSpark")
                .sparkline(
                    [<%= Model.KnowledgeSummary.Secure %>, <%= Model.KnowledgeSummary.Weak %>, <%= Model.KnowledgeSummary.Unknown %>],
                    {
                        type: 'pie',
                        sliceColors: ['#3e7700', '#B13A48', '#EFEFEF'],
                        tooltipFormat: '{{offset:slice}} {{value}} ({{percent.1}}%)',
                        tooltipValueLookups: {'slice': titles},
                    }
                );

            $("#totalKnowledgeOverTimeSpark").sparkline([5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5, 3, 2, 2, 4, 6, 7, 5, 6, 7, 9, 9, 5], {
                type: 'line',
                witdh: '250'
            });

            $("#inCategoeryOverTime-1").sparkline([1, 4, 4, 2, 1, 8, 7, 9], { type: 'line', sliceColors: ['#3e7700', '#B13A48'] });
            $("#question-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
            $("#inCategory-1").sparkline([5, 5], { type: 'pie', sliceColors: ['#3e7700', '#B13A48'] });
        });
    </script>
    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate1", 9, 2, 1) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate2", 4, 3, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate("chartKnowledgeDate3", 1, 12, 4) });
        google.setOnLoadCallback(drawActivityChart);

        //chartKnowledgeDate
        function drawKnowledgeChart(chartElementId) {

            if ($("#" + chartElementId).length === 0) {
                return;
            }

            var data = google.visualization.arrayToDataTable([
                ['Task', 'Hours per Day'],
                ['Gewusst', <%= Model.KnowledgeSummary.Secure %>],
                ['Nicht gewusst', <%= Model.KnowledgeSummary.Weak %>],
                ['Unbekannt', <%= Model.KnowledgeSummary.Unknown %>],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'labeled' },
                pieSliceText: 'none',
                chartArea: { 'width': '100%', height: '100%', top: 10},
                slices: {
                    0: { color: 'lightgreen' },
                    1: { color: 'lightsalmon' },
                    2: { color: 'silver' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
            chart.draw(data, options);
        }

        function drawKnowledgeChartDate(chartElementId, amountGood, amountBad, amountUnknown ) {

            var chartElement = $("#" + chartElementId);

            var data = google.visualization.arrayToDataTable([
                ['Task', 'Hours per Day'],
                ['Gewusst', amountGood],
                ['Nicht gewusst', amountBad],
                ['Unbekannt', amountUnknown],
            ]);

            var options = {
                pieHole: 0.5,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 80,
                chartArea: { width: '90%', height: '90%', top: 0 },
                slices: {
                    0: { color: 'lightgreen' },
                    1: { color: 'lightsalmon' },
                    2: { color: 'silver' },
                },
                pieStartAngle: 180
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
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['lightgreen', 'lightsalmon'],
                isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartActivityLastDays"));
            chart.draw(view, options);
            <% if (!Model.HasLearnedInLast30Days) { %>
                var infoDivNotLearned = document.createElement('div');
                infoDivNotLearned.innerHTML = '<p>Du hast in den letzten 30 Tagen keine Fragen beantwortet, daher kann hier keine Übersicht angezeigt werden.</p>'
                infoDivNotLearned.setAttribute('style', 'position: absolute; background-color: white; top: 145px;')
                document.getElementById("chartActivityLastDays").appendChild(infoDivNotLearned);
                document.getElementById("chartActivityLastDays").style.opacity = 0.5;
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
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.Message != null) { %>
        <div class="row">
            <div class="col-xs-12 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>        
    <% } %>

    <h2 style="color: black; margin-bottom: 5px; margin-top: 0px;"><span class="ColoredUnderline Knowledge">Hallo <%= Model.UserName %>!</span></h2>
        
    <p style="margin-bottom: 0px;">Hier erhältst du eine Übersicht über dein Wunschwissen und deinen Wissensstand.</p>

    <% if(!Model.IsLoggedIn){ %>

        <div class="bs-callout bs-callout-danger">
            <h4>Anmelden oder registrieren</h4>
            <p>Um Wunschwissen zu verwenden, musst du dich <a href="/Anmelden">anmelden</a> oder <a href="/Registrieren">registrieren</a>.</p>
        </div>

    <% }else{  %>
        <p style="margin-bottom: 20px;">
            Du hast momentan <%= Model.ReputationTotal %> Reputationspunkte <i class="fa fa-info-circle show-tooltip" title="Du gewinnst Reputationspunkte z.B., indem du gute Fragen, Fragesätze etc. erstellst. In der Hilfe erfährst du mehr."></i>
            (Rang <%= Model.ReputationRank %>) 
            (<a href="<%= Links.UserDetail(Model.User) %>">Details auf deiner Profilseite</a>).
        </p>
    
        <div class="row">
            <div class="col-xs-12 col-md-6">
                <div class="col-xs-12" style="padding: 10px;">

                    <h3 style="margin-top: 0;">Training <span style="font-size: 12px; color: silver; padding-left: 15px;">letzte 30 Tage</span></h3>
                
                    <div id="chartActivityLastDays" style="margin-right: 20px; text-align: left;"></div>
                
                    <div class="row" style="font-size: 12px">
                        <div class="col-md-12">
                            <% var streak = Model.StreakDays; %>
                            <!-- -->
                            <span style="display: inline-block; width: 40%">Lerntage gesamt: 
                                <b><%= streak.TotalLearningDays %></b></span> <span style="color: silver; font-weight: bold;">
                                seit <%= Model.User.DateCreated.ToString("dd.MM.yyyy") %>
                            </span><br />
                        
                            <!-- LongestStreak -->
                            <span style="display: inline-block; width: 40%">
                                Längste Folge: <b><%= streak.LongestLength %></b>
                            </span>
                            <% if (streak.LongestLength == 0){ %>
                                <span style="color: silver; font-weight: bold;">zuletzt nicht gelernt</span>
                            <% } else { %>
                                <span style="color: silver; font-weight: bold;">
                                    <%= streak.LongestStart.ToString("dd.MM") %> - <%= streak.LongestEnd.ToString("dd.MM.yyyy") %>
                                </span><br />
                            <% } %>
                        
                            <!-- CurrentStreak -->
                            <span style="display: inline-block; width: 40%">
                                Aktuelle Folge: <b><%= streak.LastLength %></b>
                            </span>
                            <% if (streak.LastLength == 0){ %>
                                <span style="color: silver; font-weight: bold;">zuletzt nicht gelernt</span>
                            <% } else { %>
                                <%= streak.LastStart.ToString("dd.MM") %> - <%= streak.LastEnd.ToString("dd.MM.yyyy") %>
                            <% } %>
                        </div>
                    </div>

                </div>
            </div>
            <div class="col-xs-12 col-md-6">
                <div class="col-xs-12" style="padding: 10px;">
                    <h3 style="margin-bottom: 3px; margin-top: 0;">Dein Wissensstand</h3>
                    <p style="margin-bottom: 1px; font-size: 12px; color: silver;">Berücksichtigt nur dein Wunschwissen</p>
                    <p>
                        In deinem Wunschwissen sind <%= Model.QuestionsCount %> Frage<%= StringUtils.Plural(Model.QuestionsCount,"n","","n") %> und <%= Model.SetCount %> Frage<%= StringUtils.Plural(Model.SetCount,"sätze","satz","sätze") %>. 
                        <i class="fa fa-info-circle show-tooltip" title="Erweitere dein Wunschwissen, indem du auf das Herz-Symbol neben einer Frage oder einem Fragesatz klickst."></i>
                    </p>
<%--                        <div class="col-xs-6">
                            <a href="<%= Links.QuestionsWish() %>">
                                <div class="number-box-questions2 rowBase" style="padding: 8px; margin: 0px;">
                                    <span style="font-weight: 900; font-size: 14px; padding-left: 9px;"><i class="fa fa-heart" style="color:#b13a48;"></i>&nbsp;<%= Model.QuestionsCount %></span>
                                    <span style="font-size: 14px">Fragen</span>
                                </div>
                            </a>
                        </div>                    
                        <div class="col-xs-6">
                            <a href="<%= Links.SetsWish() %>">
                                <div class="number-box-sets2 rowBase" style="padding: 8px; margin: 0px;">
                                    <span style="font-weight: 900; font-size: 14px; padding-left: 15px;"><i class="fa fa-heart" style="color:#b13a48;"></i>&nbsp;<%= Model.SetCount %></span>
                                    <span style="font-size: 14px">Fragesätze</span>
                                </div>
                            </a>
                        </div>--%>
                    <% if(Model.KnowledgeSummary.Total == 0) { %> <%--Warum nicht "if sets == 0 & questions == 0 then"?--%>
                            <p>
                                MEMuchO kann deinen Wissensstand nicht zeigen, da du noch kein Wunschwissen hast.
                            </p>
                            <p>
                                Um dein Wunschwissen zu erweitern, suche dir interessante <a href="<%= Links.QuestionsAll() %>">Fragen</a>  
                                oder <a href="<%= Links.Sets() %>">Fragesätze</a> aus und klicke dort auf das Herzsymbol:
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
                    <% }else { %>
                        <div id="chartKnowledge" style="margin-right: 20px; text-align: left;"></div>
                    <% } %>
                </div>
            </div>
<%--            <div class="col-xs-12 col-md-3">
                <div class="row">
                    <div class="col-cs-12">
                        <h3>Im Wunschwissen</h3>        
                    </div>
                    <div class="col-cs-12 number-box-questions">
                        <a href="<%= Links.QuestionsWish() %>">
                            <div>
                                <span style="font-weight: 900; font-size: 44px; padding-left: 9px;"><%= Model.QuestionsCount %></span>
                                <span style="font-size: 22px">Fragen</span>
                            </div>
                        </a>
                    </div>                    
                    <div class="col-cs-12 number-box-sets">
                        <a href="<%= Links.SetsWish() %>">
                            <div>
                                <span style="font-weight: 900; font-size: 44px; padding-left: 15px;"><%= Model.SetCount %></span>
                                &nbsp;<span style="font-size: 22px">Fragesätze</span>
                            </div>
                        </a>
                    </div>
                    <div class="col-cs-12 number-box-reputation">
                        <a href="<%= Links.UserDetail(Model.UserName, Model.UserId) %>">
                            <div style="padding-left: 14px; padding: 8px;">                        
                                <span>Reputation <b><%= Model.ReputationTotal %></b></span><br />
                                <span>Platz <b><%= Model.ReputationRank %></b></span>
                            </div>
                        </a>
                    </div>
                </div>
            </div>   --%>         
        </div>
    
        <div class="row" style="margin-top: 25px;">
            <div class="col-xs-12 col-md-4">
                    <div class="col-xs-12 rowBase" style="padding: 10px;">
                        <h3 style="margin-top: 0;">Termine</h3>
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
                                    <div class="col-xs-6" style="text-align: center; clear: left;">
                                        <a href="<%= Links.GameCreateFromDate(date.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="margin-top: 17px; display: inline-block;">
                                            <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                                            Spiel starten
                                        </a>
                                    </div>
                                    <div class="col-xs-6" style="text-align: center;">
                                        <a class="btn btn-sm btn-primary" data-btn="startLearningSession" href="/Termin/Lernen/<%=date.Id %>" style="margin-top: 10px; display: inline-block;">
                                            <i class="fa fa-line-chart"></i> 
                                            Jetzt üben
                                        </a>
                                    </div>
                                </div>  
                                <hr style="margin: 8px 0;"/>  
                            <% } %>
                            <% if (Model.Dates.Count > 3) { %>
                                <a href="<%= Links.Dates() %>">Du hast <%= (Model.Dates.Count - 3) %> <%= StringUtils.Plural(Model.Dates.Count - 3,"weitere Termine","weiteren Termin") %></a>
                                <hr style="margin: 8px 0px;"/>
                            <% } %>
                        <% } %>
                        <p>
                            <% if (Model.DatesInNetwork.Count > 0) { %>
                                <a href="<%= Links.Dates() %>"><%= Model.DatesInNetwork.Count %> Termin<%= StringUtils.Plural(Model.DatesInNetwork.Count,"e") %> in deinem Netzwerk</a>
                                &nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>
                            <% } else {  %>
                                Kein Termin in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>&nbsp;<i class="fa fa-info-circle show-tooltip" title="Termine aus deinem Netzwerk kannst du einfach übernehmen. So kannst du leicht mit Freunden lernen."></i>.
                                Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                            <% } %>
                            
                        </p>
                        <hr style="margin: 8px 0px;"/>
                        <p><a href="<%= Links.Dates() %>">(Zur Terminübersicht)</a></p>

                    </div>

            </div>         
               
            <div class="col-xs-12 col-md-4">
                    <div class="col-xs-12 rowBase" style="padding: 10px;">
                        <h3 style="margin-top: 0;">Zuletzt gelernt</h3>
                        <% foreach(var answer in Model.AnswerRecent){ 
                            var question = answer.Question;
                        %>
                            <div class="row" style="margin-bottom: 10px;">
                                <div class="col-xs-3">
                                    <%= ImageFrontendData.Create(question).RenderHtmlImageBasis(50, true, ImageType.Question) %>
                                </div>
                                <div class="col-xs-9" style="">
                                    <a href="<%= Links.AnswerQuestion(Url, question) %>"><%= question.Text %></a>
                                </div>
                            </div>
                        <% } %>
                
                        <div class="row" style="opacity: 0.4;">
                            <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                        </div>
                    </div>
            </div>

            <div class="col-xs-12 col-md-4">
                    <div class="col-xs-12 rowBase" style="padding: 10px;">
                        <h3 style="margin-top: 0;">Im Netzwerk</h3>
                        <% if (Model.NetworkActivities.Count == 0) { %>
                                Keine Aktivitäten in deinem <a href="<%= Url.Action("Network", "Users") %>">Netzwerk</a>. 
                                Erweitere dein Netzwerk, indem du anderen <a href="<%= Url.Action("Users", "Users") %>">Nutzern folgst</a>.
                        <% } else { %>
                            <% foreach(var activity in Model.NetworkActivities){ %>
                                <div class="row" style="margin-bottom: 10px;">
                                    <div class="col-xs-3">
                                        <img src="<%= new UserImageSettings(activity.UserCauser.Id).GetUrl_128px_square(activity.UserCauser.EmailAddress).Url %>" />
                                    </div>
                                    <div class="col-xs-9" style="">
                                        <div style="color: silver; font-size: 10px; margin: -4px 0;"><%= DateTimeUtils.TimeElapsedAsText(activity.At) %></div>
                                        <div style="clear: left;">
                                            <a href="<%= Links.UserDetail(activity.UserCauser) %>"><%= activity.UserCauser.Name %></a> <%= UserActivityTools.GetActionDescription(activity) %>
                                            <%= UserActivityTools.GetActionObject(activity) %>
                                        </div>
                                    </div>
                                </div>
                            <% } %>
                            <div class="row" style="opacity: 0.4;">
                                <div class="col-xs-12"><a href="#" class="">mehr...</a></div>
                            </div>
                        <% } %>
                    </div>
            </div>

        </div>

<%--        <div class="column">
            <h3>Wunschwissen</h3>
            <div class="answerHistoryRow">
                <div>
                    <a href="<%= Links.QuestionsWish() %>">
                        Fragen: <span><%= Model.QuestionsCount %> <span id="totalKnowledgeSpark"></span></span>
                    </a>
                </div>    
            </div>
            <div class="answerHistoryRow">
                <div>
                    <a href="<%= Links.SetsWish() %>">
                        Frageseätze: <span><%= Model.SetCount %> </span><br/>
                    </a>
                </div>
                
            </div>
            <span id="totalKnowledgeOverTime">Entwicklung über Zeit:<br /> 
            <span id="totalKnowledgeOverTimeSpark"></span></span>
        </div>
        
        <div class="column">
            <h3>Wissenstand</h3>
            <div class="show-tooltip" title="Die Menge der gewussten Fragen" style="padding-bottom: 10px;">Dein Wissensstand entspricht ca. <%= Model.KnowledgeSummary.SecurePercentage %>% deines Wunschwissens.</div>
            <div class="show-tooltip" title="Als 'gewusst' gilt eine Frage dann, wenn die Wahrscheinlichkeit, dass du die Frage richtig beantworten wirst, bei über 90% liegt.">Gewusst: <%= Model.KnowledgeSummary.SecurePercentage %>% (<%= Model.KnowledgeSummary.Secure %> Fragen) </div>
            <div class="show-tooltip" title="Als 'nicht gewusst' gilt eine Frage dann, wenn die Wahrscheinlichkeit, dass du die Frage richtig beantworten wirst, bei unter 90% liegt.">Nicht gewusst: <%= Model.KnowledgeSummary.WeakPercentage %>% (<%= Model.KnowledgeSummary.Weak %> Fragen)  </div>
            <div class="show-tooltip" title="Wenn du die Frage seltener als 4x beantwortet hast, ist unbekannt, ob du sie weißt.">Unbekannt: <%= Model.KnowledgeSummary.UnknownPercentage %>% (<%= Model.KnowledgeSummary.Unknown %> Fragen)</div>
        </div>

        <div class="column">
            <h3>Training</h3>
            <div class="answerHistoryRow" style="margin-bottom: 5px;">
                <div style="color:black;">
                    Antworten ges.: <%= Model.AnswersEver.TotalAnswers %>
                </div> 
            </div>
                
            <div class="answerHistoryRow">    
                <div>
                    Diese Woche 
                    <span class="answerAmount"><%= Model.AnswersThisWeek.TotalAnswers %></span> 
                    <span id="answeredThisWeekSparkle"></span>
                    
                    &nbsp;
                    Vorwoche
                    <%= Model.AnswersLastWeek.TotalTrueAnswers %>
                </div> 
            </div>
            <div class="answerHistoryRow">
                <div>
                    Diesen Monat 
                    <span class="answerAmount"><%= Model.AnswersThisMonth.TotalAnswers %></span> 
                    <span id="answeredThisMonthSparkle"></span>
                    
                    &nbsp;
                    Vormonat
                    <%= Model.AnswersLastMonth.TotalAnswers %>
                </div>
            </div>
            <div class="answerHistoryRow">
                <div>
                    Dieses Jahr 
                    <span class="answerAmount"><%= Model.AnswersThisYear.TotalAnswers %></span> 
                    <span id="answeredThisYearSparkle"></span>
                    
                    &nbsp;
                    Vorjahr
                    <%= Model.AnswersLastYear.TotalAnswers %>
                </div> 
            </div>
        </div>
        <div style="clear:both;"></div>--%>
        
<%--        <div style="padding-top:20px; height: 200px; ">
        
            <div class="column">
                <h3>Fragen (175)</h3>
                <table>
                    <tr>
                        <td>Wann wurde xy geboren </td> 
                        <td>14x <span id="question-1"></span> </td>
                    </tr>
                </table>
            </div>
        
            <div class="column">
                <h3>Fragesätze</h3>
                <div>
                    <span>Noch keine Fragesätze</span>
                </div>
            </div>
 
            <div class="column">
                <h3>Kategorien (34)</h3>
                <table>
                    <tr>
                        <td>Musik</td> 
                        <td>72</td> 
                        <td><span id="inCategory-1"></span></td>
                        <td><span id="inCategoeryOverTime-1"></span></td>
                    </tr>
                </table>
            </div>
        </div>--%>
    <% } %>
</asp:Content>