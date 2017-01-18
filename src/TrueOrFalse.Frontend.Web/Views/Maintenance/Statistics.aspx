<%@ Page Title="Maintenance: Statistics" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<StatisticsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChartNewUsers);
        google.setOnLoadCallback(drawChartUsers);
        google.setOnLoadCallback(drawChartQuestionCountStats);
        google.setOnLoadCallback(drawChartNewQuestions);
        google.setOnLoadCallback(drawChartUsageStatsPt1);
        google.setOnLoadCallback(drawChartUsageStatsPt2);

        function drawChartNewUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Registrierungen am Tag', { role: 'annotation' }
                ],
                <%  var lastDay = Model.Since;
                    foreach (var date in Model.NewUsersGroupedByRegistrationDate)
                    {
                        while (lastDay < date.First().DateCreated.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.") + "', 0, ''],");
                            lastDay = lastDay.AddDays(1);
                        } %>
                        <%= "['" + date.First().DateCreated.ToString("dd.MM.") + "', " + date.Count() + ", '']," %> 
                        <% lastDay = lastDay.AddDays(1);
                    }
                    while (lastDay <= DateTime.Now.Date)
                    {
                        Response.Write("['" + lastDay.ToString("dd.MM.") + "', 0, ''],");
                        lastDay = lastDay.AddDays(1);
                    }
                %>
            ]);

            var view = new google.visualization.DataView(data);
            view.setColumns([0, 1,
                {
                    calc: "stringify",
                    sourceColumn: 1,
                    type: "string",
                    role: "annotation"
                }
                ]);

            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    showTextEvery: 3
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534']
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewUsers"));
            chart.draw(view, options);
        }

        function drawChartUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Registrierte Nutzer'
                ],
                <%  
                    var curentDay = Model.SinceGoLive;
                    while (curentDay <= DateTime.Now.Date)
                    {
                        Response.Write("['" + curentDay.ToString("dd.MM.yyyy") + "', " + Model.Users.Count(u => u.DateCreated.Date <= curentDay) + "],");
                        curentDay = curentDay.AddDays(1);
                    }
                 %>
            ]);

            var view = new google.visualization.DataView(data);
            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 10 },
                colors: ['#afd534'],
                lineWidth: 6,
            };

            var chart = new google.visualization.AreaChart(document.getElementById("chartUsers"));
            chart.draw(view, options);
        }



        function drawChartNewQuestions() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von anderen Nutzern neu erstellte Fragen', 'Von memucho neu erstellte Fragen', { role: 'annotation' }
                ],
                <%  lastDay = Model.Since;
                    foreach (var day in Model.QuestionsCreatedPerDayResults)
                    {
                        while (lastDay < day.DateTime.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.") + "', 0, 0, ''],");
                            lastDay = lastDay.AddDays(1);
                        } %>
                        <%= "['" + day.DateTime.ToString("dd.MM.") + "', " + day.CountByOthers + ", " + day.CountByMemucho + ", " + day.TotalCount + "]," %> 
                        <% lastDay = lastDay.AddDays(1);
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                annotations: { alwaysOutside: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    showTextEvery: 3
                },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#0000ff', '#afd534'],
                isStacked: true
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewQuestions"));
            chart.draw(view, options);
        }
        

        function drawChartQuestionCountStats() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von Nutzern erstellte Fragen', 'Von memucho erstellte Fragen'
                ],
                <%  lastDay = Model.SinceGoLive;
                    var questionCountSoFarMemucho = 0;
                    var questionCountSoFarOthers = 0;
                    foreach (var day in Model.QuestionsExistingPerDayResults)
                    {
                        while (lastDay < day.DateTime.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.") + "', " + questionCountSoFarOthers + ", " + questionCountSoFarMemucho + "],");
                            lastDay = lastDay.AddDays(1);
                        }
                        questionCountSoFarMemucho = day.CountByMemucho;
                        questionCountSoFarOthers = day.CountByOthers;
                        Response.Write("['" + day.DateTime.ToString("dd.MM.") + "', " + questionCountSoFarOthers + ", " + questionCountSoFarMemucho + "],"); 
                        lastDay = lastDay.AddDays(1);
                    } %>
            ]);

            var view = new google.visualization.DataView(data);
            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 10 },
                colors: ['#0000ff', '#afd534'],
                lineWidth: 6,
                isStacked: true
            };

            var chart = new google.visualization.AreaChart(document.getElementById("chartQuestionCountStats"));
            chart.draw(view, options);
        }



        function drawChartUsageStatsPt1() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Fragen beantwortet', '# Fragen gesehen', '# Übungssitzungen gestartet', '# Termine angelegt'
                ],
                <%  foreach (var day in Model.UsageStats)
                    {
                        Response.Write("['" + day.DateTime.ToString("dd.MM.") + "', " + 
                            day.QuestionsAnsweredCount + ", " + 
                            day.QuestionsViewedCount + ", " + 
                            day.LearningSessionsStartedCount + ", " + 
                            day.DatesCreatedCount + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                hAxis: {
                    showTextEvery: 3
                },
                bar: { groupWidth: '80%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534', '#003264', '#b13a48', '#1964c8']
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsageStats1"));
            chart.draw(view, options);
        }

        function drawChartUsageStatsPt2() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', '# Nutzer haben Fragen beantwortet', '# Nutzer haben Fragen gesehen', '# Nutzer haben Übungssitzung gestartet', '# Nutzer haben Termine angelegt'
                ],
                <%  foreach (var day in Model.UsageStats)
                    {
                        Response.Write("['" + day.DateTime.ToString("dd.MM.") + "', " + 
                            day.UsersThatAnsweredQuestionCount + ", " + 
                            day.UsersThatViewedQuestionCount + ", " + 
                            day.UsersThatStartedLearningSessionCount + ", " + 
                            day.UsersThatCreatedDateCount + "],");
                    } %>
            ]);

            var view = new google.visualization.DataView(data);

            var options = {
                tooltip: { isHtml: true },
                legend: {
                    position: 'top',
                    maxLines: 30,
                    textStyle: { fontSize: 11 }
                },
                hAxis: {
                    showTextEvery: 3
                },
                bar: { groupWidth: '80%' },
                chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
                colors: ['#afd534', '#003264', '#b13a48', '#1964c8']
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsageStats2"));
            chart.draw(view, options);
        }

    </script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentReport">Content</a></li>
                <li class="active"><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
        
    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 0;">Nutzungsstatistiken</h2>
            <ul>
                <li><a href="#UsersRegistered">Registrierte Nutzer</a></li>
                <li><a href="#QuestionsCreated">Erstellte Fragen</a></li>
                <li><a href="#UsageStats">Nutzung durch eingeloggte Nutzer</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 40px;" id="UsersRegistered">Registrierte Nutzer</h2>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Neu registrierte Nutzer der letzten 31 Tage und Nutzer insgesamt seit <%= Model.SinceGoLive.ToString("dd.MM.yyyy") %>.
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewUsers" style="height: 250px; margin-right: 20px; text-align: left;"></div>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartUsers" style="height: 300px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h2 class="" style="margin-top: 60px" id="QuestionsCreated">Erstellte Fragen</h2>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Neu erstellte und existierende Fragen seit GoLive (<%= Model.SinceGoLive.ToString("dd.MM.yyyy") %>).
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewQuestions" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartQuestionCountStats" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 60px" id="UsageStats">Nutzung durch eingeloggte Nutzer (ohne Admins)</h1>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <p>
                Wie oft wurden pro Tag durch eingeloggte (!) Nutzer (ohne Admins)...
            </p>
            <div id="chartUsageStats1" style="height: 300px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 20px;">
            <p>
                Wie viele unterschiedliche eingeloggte Nutzer (ohne Admins) haben an diesem Tag...
            </p>
            <div id="chartUsageStats2" style="height: 300px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

</asp:Content>