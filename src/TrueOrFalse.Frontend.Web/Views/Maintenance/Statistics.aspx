<%@ Page Title="Maintenance: Statistics" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<StatisticsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChartNewUsers);
        google.setOnLoadCallback(drawChartUsers);
        //google.setOnLoadCallback(drawChartNewQuestions);

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
                        Response.Write("['" + curentDay.ToString("dd.MM.yyyy") + "', " + Model.Users.Count(u => u.DateCreated <= curentDay) + ", ''],");
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
                legend: { position: 'top', maxLines: 30 },
                bar: { groupWidth: '89%' },
                chartArea: { 'width': '98%', 'height': '60%', top: 30, bottom:-10 },
                colors: ['#afd534'],
                //isStacked: true,
            };

            var chart = new google.visualization.ColumnChart(document.getElementById("chartUsers"));
            chart.draw(view, options);
        }


        function drawChartNewQuestions() {
            var data = google.visualization.arrayToDataTable([
                [
                    'Datum', 'Fragen memucho', 'Fragen != memucho', { role: 'annotation' }
                ],
                <%  lastDay = Model.Since;
                    foreach (var day in Model.QuestionsCreatedResult)
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
                <li><a href="/Maintenance/Images">Bilder</a></li>
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
            <h1 class="" style="margin-top: 0;">Registrierte Nutzer</h1>
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
            <h1 class="">Fragen</h1>
        </div>

        <div class="col-xs-12" style="margin-top: 20px;">
            <div id="chartNewQuestions" style="margin-right: 20px; text-align: left;"></div>
        </div>
    </div>

</asp:Content>