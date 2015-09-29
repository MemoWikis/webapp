<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabForecastModel>" %>
<%@ Import Namespace="NHibernate.Util" %>


<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script>
        
    google.load("visualization", "1", { packages: ["corechart", "bar"] });
    google.setOnLoadCallback(drawCharts);

    function drawCharts() 
    {
        drawBubbleChart(<%= Model.GetBubbleChartRows(Model.FeatureModelsRepetition) %>, "repetitionFeatureBubbleChart");
        drawColumnChart();
    }

    function drawBubbleChart(dataArray, chartElement) {

        var dataBubbleChart = google.visualization.arrayToDataTable(dataArray);

        var options = {
            title: 'Beziehung Anzahl Wiederholungen (x-Axchse) zu Algorithmus-Vorhersagegenauigkeit (y-Achse) und Anzahl Testdaten (Bubble Größe).',
            hAxis: { title: 'Anzahl Wiederholungen' },
            vAxis: { title: 'Algorithmus-Vorhersagegenauigkeit %' },
            bubble: { repetitionFeatureBubbleChart: { fontSize: 12 } },
            animation: {duration:1000 }
        };

        var chart = new google.visualization.BubbleChart(document.getElementById(chartElement));
        chart.draw(dataBubbleChart, options);
    }

    function drawColumnChart() {
        var data = google.visualization.arrayToDataTable([
            ['Anzahl Wiederholungen',' <%= Model.FeatureModelsRepetition
                    .First()
                    .Summaries
                    .Select(x => x.Algo.Name)
                    .Aggregate((a, b) => a + "','" + b ) %>'],
            <%
                Model.FeatureModelsRepetition
                    .Where(x => x.Summaries.Any())
                    .OrderByDescending(x => x.Feature.Name)
                    .ForEach(x =>
                    {
                        var algoValues = x.Summaries
                            .Select(y => y.SuccessRateInPercent.ToString())
                            .Aggregate((a, b) => a + "," + b);

                        Response.Write(
                            String.Format("[{0}, {1}],",
                                Model.GetRepetitionCount(x.Feature.Name),
                                algoValues));
                    });
            %>
        ]);

        var options = {
            title: 'Beziehung Anzahl Wiederholungen (x-Axchse) zu Algorithmus-Vorhersagegenauigkeit (y-Achse)',
            bar: { groupWidth: "90%" },
            legend: { position: "bottom" },
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('repetitionFeatureColumnChart'));

        chart.draw(data, options);
    }

</script>

<div class="alert alert-info col-md-12" style="margin-top:9px">
    <p>
        Hier erhältst du Einblick in die Algorithmen, die die <b>Antwortwahrscheinlichkeit</b> 
        und den <b>optimalen Wiedervorlage-Zeitpunkt</b> berechnen.
        MEMuchO ist Open Source<a href="https://github.com/TrueOrFalse/TrueOrFalse"> (auf Github)</a>. 
        Wir freuen uns über Verbesserungsvorschläge.
    </p>        
</div>

<div class="row" >
    <div class="col-md-12" style="margin-top: -20px; margin-bottom: 10px;">
        <h3>Vergleich Algorithmen</h3>
        <p>
            Gezeigt wird, wie gut unterschiedliche Algorithmen
            bisher gegebene Antworten korrekt vorhergesagt hätten.
            Getestet wird jede bisher gegebene Antwort, jeweils mit
            allen vorherigen Antworten. 
        </p> 
        <p>
            Die berechnete Antwortwahrscheinlichkeit wird 
            mit der tatsächlich gegebenen Antwort verglichen. 
            Hieraus ergibt sich die durchschnittliche Erfolgsrate (% Erfolg). 
        </p>
    </div>
</div>

<div class="row">
    <div class="col-md-6">
        <div class="row">
            <div class="col-md-12">
                <h3>Alle Antworten</h3>
            </div>
        </div>
        <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(Model.Summaries.ToList())); %>
    </div>
    <div class="col-md-6">
        <div class="row">
            <div class="col-md-12">
                <h3>Top 5 Features (mehr als 50 Daten)</h3>
            </div>
        </div>
            
        <div class="row">
            <div class="col-md-12">
                      
                <table class="table table-hover">
                    <tr>
                        <th>FeatureName</th>
                        <th>AlgoName</th>
                        <th>%&nbsp;Erfolg</th>
                        <th>Total</th>
                    </tr>
	            
                    <% foreach(var summary in Model.TopFeatures.Take(5)) { %>
                        <tr>
	                        <td><%= summary.FeatureName %></td>
                            <td><%= summary.Algo.Name %></td>
	                        <td><%= summary.SuccessRate %></td>
                            <td><%= summary.TestCount %></td>
                        </tr>
                    <% } %>
                </table>
            </div>
        </div>
    </div>
</div>
    
<div class="row">
    <div class="col-md-12">
        <h3>Feature: Vorherige Wiederholungen</h3>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
    <% foreach(var repetitionFeature in Model.FeatureModelsRepetition) { %>
        <% if (!repetitionFeature.Summaries.Any()){continue;} %>
            <div class="row">
                <div class="col-md-12">
                    <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(repetitionFeature)); %>
                </div>
            </div>
    <% } %>
    </div>
    <div class="col-md-6" style="vertical-align: top">
        <div id="repetitionFeatureBubbleChart" style="width: 100%; height: 300px;"></div>                
        <div id="repetitionFeatureColumnChart" style="width: 100%; height: 250px;"></div>
    </div>
</div>
    
<div class="row">
    <div class="col-md-12">
        <h3>Feature: Tageszeit</h3>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
    <% foreach(var repetitionFeature in Model.FeatureModelsTime) { %>
        <% if (!repetitionFeature.Summaries.Any()){continue;} %>
            <div class="row">
                <div class="col-md-12">
                    <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(repetitionFeature)); %>
                </div>
            </div>
    <% } %>
    </div>
    <div class="col-md-6" style="vertical-align: top"></div>
</div>

<div class="row">
    <div class="col-md-12">
        <h3>Feature: Trainingstyp</h3>
    </div>
</div>
<div class="row">
    <div class="col-md-6">
    <% foreach(var repetitionFeature in Model.FeatureModelsTrainingType) { %>
        <% if (!repetitionFeature.Summaries.Any()){continue;} %>
            <div class="row">
                <div class="col-md-12">
                    <% Html.RenderPartial("ComparisonTable", new  ComparisonTableModel(repetitionFeature)); %>
                </div>
            </div>
    <% } %>
    </div>
    <div class="col-md-6" style="vertical-align: top"></div>
</div>