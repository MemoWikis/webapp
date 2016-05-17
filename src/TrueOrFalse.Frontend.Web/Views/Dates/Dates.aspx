<%@ Page Title="Termine" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<DatesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    
    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawKnowledgeCharts);

        function drawKnowledgeChartDate(chartElementId, notLearned, needsLearning, needsConsolidation, solid) {

            var chartElement = $("#" + chartElementId);
            if (chartElement.length == 0)
                return;

            //'Sicheres Wissen', 'Sollte gefestigt werden', 'Sollte dringend gelernt werden', 'Noch nie gelernt'
            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'Anteil in %'],
                ['Sicheres Wissen', solid],
                ['Solltest du festigen', needsConsolidation],
                ['Solltest du lernen', needsLearning],
                ['Noch nicht gelernt', notLearned],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 120,
                backgroundColor: 'transparent',
                chartArea: {
                    width: '90%', height: '90%', top: 6
                },
                slices: {
                    0: { color: '#3e7700' },
                    1: { color: '#fdd648' },
                    2: { color: '#B13A48' },
                    3: { color: '#EFEFEF' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(chartElement.get()[0]);
            chart.draw(data, options);
        }

        function drawKnowledgeCharts() {
            $("[data-date-id]").each(function () {
                var $this = $(this);
                var dateId = $this.attr("data-date-id");

                drawKnowledgeChartDate(
                    "chartKnowledgeDate" + dateId,
                    parseInt($this.attr("data-notLearned")),
                    parseInt($this.attr("data-needsLearning")),
                    parseInt($this.attr("data-needsConsolidation")),
                    parseInt($this.attr("data-solid")));
            });
        }

    </script>
    
    <%= Styles.Render("~/bundles/Dates") %>
    <%= Scripts.Render("~/bundles/js/Dates") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    

    <div class="row">
        <div class="PageHeader col-md-9">
            <h2 style="margin-bottom: 15px; margin-top: 0px;" class="pull-left">
                <span class="ColoredUnderline Date">Deine Termine</span>
            </h2>
        </div>
        <div class="col-md-3">
            <div class="headerControls pull-right">
                <div style="padding-top: 5px;">
                    <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm pull-right">
                        <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                    </a>
                </div>
            </div>
        </div>
    </div>
        
    <div class="row">
        <div class="col-md-12" id="allDateRows">

            <% if(!Model.IsLoggedIn){ %>

                <div class="bs-callout bs-callout-danger">
                    <h4>Anmelden oder registrieren</h4>
                    <p>Um Termine zu erstellen, musst du dich <a href="/Anmelden">anmelden</a> 
                       oder <a href="/Registrieren">registrieren</a>.</p>
                </div>

            <% }else{ %>
        
                <% if (!Model.Dates.Any()){ %>
                    <div id="noOwnCurrentDatesInfo" class="bs-callout bs-callout-info" style="margin-top: 0; margin-bottom: 10px;">
                        <h4>Du hast keine aktuellen Termine</h4>
                        <p style="padding-top: 5px;">
                            Termine helfen dir dabei, dich optimal auf eine Prüfung vorzubereiten.
                        </p>
                        <p>
                            <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm" style="margin-top: 10px;">
                                <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                            </a>
                        </p>
                    </div>
                <% } else { %>
                    <% foreach(var date in Model.Dates){ %>
                        <% Html.RenderPartial("DateRow", new DateRowModel(date)); %>
                    <% } %>
                <% } %>
            
                <% if(Model.HasPreviousDates){ %>
                    <div style="margin-top: -2px; text-align: right;" id="divShowPreviousDates">
                        <a href="#" id="btnShowPreviousDates" style="font-size: 11px; color: darkgray"><i class="fa fa-caret-down"></i> Zeige Termine aus der Vergangenheit</a>
                    </div>
            
                    <div id="previousDates"></div>
                <% } %>

                <h3 style="margin-bottom: 10px;">
                    <span class="ColoredUnderline Date" style="padding-right: 3px;">Termine im Netzwerk</span>
                </h3>
    
                <% if (!Model.DatesInNetwork.Any()){ %>
                    <div class="bs-callout bs-callout-info"  style="margin-top: 0; <%= Html.CssHide(Model.DatesInNetwork.Any()) %>">
                        <h4>Keine kommenden Termine im Netzwerk</h4>
                        <p>
                            <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-sm" style="margin-top: 10px;">
                                <i class="fa fa-plus-circle"></i> &nbsp; Netzwerk erweitern
                            </a>
                        </p>
                    </div>
                <% } else { %>
                    <% foreach(var date in Model.DatesInNetwork){ %>
                        <% Html.RenderPartial("DateRow", new DateRowModel(date, isNetworkDate:true)); %>
                    <% } %>            
                <% } %>

            <% } %>

        </div>
    </div>
    
    <% Html.RenderPartial("Modals/DeleteDate"); %>
    <% Html.RenderPartial("Modals/TrainingSettings"); %>
    <% Html.RenderPartial("Modals/CopyDate"); %>
            
</asp:Content>