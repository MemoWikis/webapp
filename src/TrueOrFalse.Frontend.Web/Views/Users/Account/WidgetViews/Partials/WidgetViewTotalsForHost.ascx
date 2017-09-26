<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<WidgetViewTotalsForHostModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChartAllWidgets<%= Model.HostOnlyAlphaNumerical %>);

    function drawChartAllWidgets<%= Model.HostOnlyAlphaNumerical %>() {
        var data = google.visualization.arrayToDataTable([
            [
                { label: 'Monat', type: 'date' },
                <% foreach (var widgetKey in Model.WidgetKeys) {
                       Response.Write("'" + widgetKey + "', ");
                   } %>
                { role: 'annotation' }
            ]
            <% foreach (var month in Model.WidgetViewsPerMonthAndKeyResults)
               {
                   //Response.Write(", [{v: new Date(" +month.Month.ToString("yyyy-MM-dd") +"), f:'" + month.Month.ToString("MM/yyyy") + "'}");
                   Response.Write(", [new Date('" + month.Month.ToString("yyyy-MM-dd") + "')");
                   foreach (var widgetKey in Model.WidgetKeys)
                   {
                       int value = 0;
                       month.ViewsPerWidgetKey.TryGetValue(widgetKey, out value);
                       Response.Write(", " + value);
                   }
                   Response.Write(",'Insgesamt: "+ month.ViewsPerWidgetKey.Sum(x => x.Value) + "']");
               } %>
        ]);

        var view = new google.visualization.DataView(data);

        var options = {
            tooltip: { isHtml: true },
            annotations: { alwaysOutside: true },
            legend: { position: 'top', maxLines: 30 },
            hAxis: {
                format: 'MM/yyyy',
                gridlines: { count: 4 }
            },
            bar: { groupWidth: '89%' },
            chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
            isStacked: true
        };

        var chart = new google.visualization.ColumnChart(document.getElementById("chartAllWidgets<%= Model.HostOnlyAlphaNumerical %>"));
        chart.draw(view, options);
    }
</script>

<div id="chartAllWidgets<%= Model.HostOnlyAlphaNumerical %>" style="height: 600px; margin-right: 20px; text-align: left;"></div>

<p>
    <strong>Erläuterung Legende:</strong> 
    Unterschiedliche Widgets werden an dem Widget-Key erkannt, der bei der Konfiguration des Widgets angegeben werden kann. 
    Wird kein eigener Key angegeben, wird die ID des Lernsets (bei einem Lernset-Widget) bzw. der Frage (bei einem Einzelfragen-Widget) als Key verwendet.
</p>

<p>
    <strong>Erläuterung Zählweise:</strong> 
    Jede einzelne Seite im Widget zählt. 
    Ruft ein Nutzer die Seite mit dem Widget auf, beantwortet alle sechs Fragen und sieht auch die Ergebnisseite, dann sind 7 Aufrufe entstanden 
    (Startseite + 5 Steps + Ergebnisseite). Rufen drei weitere Nutzer nur die Webseite mit dem Widget auf, ohne im Widget auf "Wissen Testen" zu klicken, 
    sind drei weitere Aufrufe entstanden (3x Startseite). Tooltips zu jedem Widget zeigen im Balkendiagramm die genaue Anzahl der Aufrufe an.
</p>


<h3>Details zu einzelnen Widgets</h3>

<p></p>

