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

        function drawChartNewUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Registrierungen', { role: 'annotation' }
                ],
                <%  var lastDay = Model.Since;
                    foreach (var date in Model.NewUsersGroupedByRegistrationDate)
                    {
                        lastDay = lastDay.AddDays(1);
                        while (lastDay < date.First().DateCreated.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.yyyy") + "', 0, ''],");
                            lastDay = lastDay.AddDays(1);
                        } %>
                        <%= "['" + date.First().DateCreated.ToString("dd.MM.yyyy") + "', " + date.Count() + ", '']," %> 
                <%  }
                    while (lastDay <= DateTime.Now.Date)
                    {
                        Response.Write("['" + lastDay.ToString("dd.MM.yyyy") + "', 0, ''],");
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
                },
                2]);

            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534'],
                //isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewUsers"));
            chart.draw(view, options);
        }

        function drawChartUsers() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Registrierte Nutzer', { role: 'annotation' }
                ],
                <%  
                    var curentDay = Model.SinceGoLive;
                    while (curentDay <= DateTime.Now.Date)
                    {
                        Response.Write("['" + curentDay.ToString("dd.MM.yyyy") + "', " + Model.Users.Count(u => u.DateCreated.Date <= curentDay) + ", ''],");
                        curentDay = curentDay.AddDays(1);
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
                },
                2]);

            var options = {
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534'],
                //isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsers"));
            chart.draw(view, options);
        }


        function drawChartQuestionCountStats() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von memucho erstellte Fragen', 'Von anderen Nutzern erstellte Fragen', { role: 'annotation' }
                ],
                <%  lastDay = Model.SinceGoLive;
                    var questionCountSoFarMemucho = 0;
                    var questionCountSoFarOthers = 0;
                    foreach (var day in Model.QuestionsExistingPerDayResults)
                    {
                        lastDay = lastDay.AddDays(1);
                        while (lastDay < day.DateTime.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.yyyy") + "', " + questionCountSoFarMemucho + ", " + questionCountSoFarOthers + ", ''],");
                            lastDay = lastDay.AddDays(1);
                        }
                        questionCountSoFarMemucho = day.CountByMemucho;
                        questionCountSoFarOthers = day.CountByOthers;
                        Response.Write("['" + day.DateTime.ToString("dd.MM.yyyy") + "', " + questionCountSoFarMemucho + ", " + questionCountSoFarOthers + ", ''],"); 
                    } %>
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
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534', '#0000ff'],
                isStacked: true
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartQuestionCountStats"));
            chart.draw(view, options);
        }

        function drawChartNewQuestions() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Von memucho neu erstellte Fragen', 'Von anderen Nutzern neu erstellte Fragen', { role: 'annotation' }
                ],
                <%  lastDay = Model.SinceGoLive;
                    foreach (var day in Model.QuestionsCreatedPerDayResults)
                    {
                        lastDay = lastDay.AddDays(1);
                        while (lastDay < day.DateTime.Date) { 
                            Response.Write("['" + lastDay.ToString("dd.MM.yyyy") + "', 0, 0, ''],");
                            lastDay = lastDay.AddDays(1);
                        } %>
                        <%= "['" + day.DateTime.ToString("dd.MM.yyyy") + "', " + day.CountByMemucho + ", " + day.CountByOthers + ", '']," %> 
                <%  } %>
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
                tooltip: { isHtml: true },
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534', '#0000ff'],
                isStacked: true
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartNewQuestions"));
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
            <h1 class="" style="margin-top: 0;">Nutzungsstatistiken</h1>
            <ul>
                <li><a href="#UsersRegistered">Registrierte Nutzer</a></li>
                <li><a href="#QuestionsCreated">Erstellte Fragen</a></li>
                <li><a href="#QuestionsAnswered">Fragen beantwortet</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 40px;" id="UsersRegistered">Registrierte Nutzer</h1>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Neu registrierte Nutzer der letzten 31 Tage und Nutzer insgesamt seit <%= Model.SinceGoLive.ToString("dd.MM.YYYY") %>.
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewUsers" style="margin-right: 20px; text-align: left;"></div>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartUsers" style="margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 20px" id="QuestionsCreated">Erstellte Fragen</h1>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Existierende und neu erstellte Fragen seit GoLive (11.10.2016).
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 40px;">
            <div id="chartQuestionCountStats" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewQuestions" style="height: 400px; margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 40px" id="QuestionsAnswered">Fragen beantwortet</h1>
            <span class="greyed" style="font-size: 10px;"><a href="#Top">(nach oben)</a></span>
            <p>
                Anzahl Frage-Beantworten der letzten 31 Tage.<br/>
                Select count(*) as answeredByLogged, Date(answer.DateCreated) as day from answer where UserId != -1 GROUP BY Date(DateCreated) <br/>
                Select count(*) as viewedByLogged, Date(questionview.DateCreated) as day from questionview where UserId != -1 GROUP BY Date(DateCreated) ORDER BY day DESC;
            </p>
            <p>
                <% foreach (var day in Model.Tempi)
                   {
                       Response.Write(day.DateTime.ToString() + " -- " + day.Int + " -- <br/>");
                   } %>
            </p>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartQuestionsAnsweredStats" style="margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

</asp:Content>