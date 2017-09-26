<%@ Page Title="Widget-Statistik" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<WidgetViewsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Account/WidgetViews.css") %>
    <%= Scripts.Render("~/Views/Users/Account/Js/WidgetViews.js") %>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChartAllWidgets);

    function drawChartAllWidgets() {
        var data = google.visualization.arrayToDataTable([
            [
                'Monat',
                <% foreach (var widgetKey in Model.WidgetKeys) {
                        Response.Write("'" + widgetKey + "', ");
                    } %>
                { role: 'annotation' }
            ]
            <% foreach (var month in Model.WidgetViewsPerMonthAndKeyResults)
               {
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
                format: 'MM/yyyy'
            },
            bar: { groupWidth: '89%' },
            chartArea: { top: 50, left: 50, right: 20, bottom: 35 },
            isStacked: true
        };

        var chart = new google.visualization.ColumnChart(document.getElementById("chartAllWidgets"));
        chart.draw(view, options);
    }
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
    <div class="col-xs-12">
        <div id="MainWrapper">
            <h1>Widget-Statistik</h1>

            <div class="col-xs-12" style="margin-top: 20px;">
                <div id="chartAllWidgets" style="height: 600px; margin-right: 20px; text-align: left;"></div>
            </div>

        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>

        </div>
    </div>
</div>

</asp:Content>