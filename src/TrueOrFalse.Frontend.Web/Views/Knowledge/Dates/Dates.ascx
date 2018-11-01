<%@  Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DatesModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
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
                ['Noch nicht gelernt', notLearned]
            ]);

            var options = {
                pieHole: 0.6,
                tooltip: { isHtml: true },
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
                    3: { color: '#EFEFEF' }
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


 

    <div class="row" id="addDates">
        <div class="PageHeader col-md-9">
            <h1 style="margin-bottom: 15px; margin-top: 0px;" class="pull-left">
                <span class="ColoredUnderline Date">Deine Termine</span>
            </h1>
        </div>
        <div class="col-md-3">
            <div class="headerControls pull-right">
                <div style="padding-top: 5px;">
                    <a href="#" class="btn btn-sm pull-right addDates" v-on:click="getEditDate()">
                        <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                    </a>
                </div>
            </div>
        </div>
    </div>
        
    <div class="row">
        <div class="col-md-12" id="allDateRows">

            <% if(!Model.IsLoggedIn){ %>

                <div class="bs-callout bs-callout-info" style="margin-top: 10px">
                    <h3 style="margin-top: 0px">Optimal auf Prüfungen vorbereiten mit memucho</h3>
                    <p style="margin-top: 25px;">
                        Mit memucho kannst du dich optimal auf Prüfungen vorbereiten. Das geht ganz einfach:
                    </p>
                    <div class="row explanationRow">
                        <div class="col-xs-3 explanationDiv" style="margin-left: 2%;">
                            <p><i class="fa fa-3x fa-calendar-check-o"></i></p>
                            <p>Wähle dein Prüfungsdatum</p>
                        </div>
                        <div class="col-xs-1 arrowDiv">
                            <i class="fa fa-lg fa-arrow-right"></i>
                        </div>
                        <div class="col-xs-3 explanationDiv">
                            <p><i class="fa fa-3x fa-book"></i></p>
                            <p>Wähle die Lerninhalte</p>
                        </div>
                        <div class="col-xs-1 arrowDiv">
                            <i class="fa fa-lg fa-arrow-right"></i>
                        </div>
                        <div class="col-xs-3 explanationDiv">
                            <p><i class="fa fa-3x fa-bar-chart"></i></p>
                            <p>memucho erstellt deinen persönlichen Lernplan</p>
                        </div>
                    </div>
                    <h3>Deine 5 Vorteile:</h3>
                    <ul>
                        <li><b>Mehr Freizeit:</b> Durch das optimierte Lernen sparst du Zeit.</li>
                        <li><b>Planbarkeit:</b> Du siehst sofort, wieviel Zeit du zum Lernen einplanen musst.</li>
                        <li><b>Effizienz:</b> Du lernst immer nur das, was du wirklich lernen musst.</li>
                        <li><b>Sicherheit:</b> Optimierte Algorithmen erinnern dich daran zu lernen, bevor du vergisst.</li>
                        <li><b>Übersicht:</b> Du hast deinen aktuellen Wissensstand immer im Blick.</li>
                        <%--<li><b>Erinnerung:</b> memucho benachrichtigt dich, wenn deine nächste Lernsitzung ansteht.</li>--%>
                    </ul>
                    
                    <h3 style="margin-top: 25px;">Registriere dich und probiere es gleich aus!</h3>
                    <p>Memucho ist kostenlos. Die Registrierung dauert nur 20 Sekunden.</p>
                    <p class="registrationParagraph">                        
                        <a href="<%= Links.Register() %>" class="btn btn-success" style="margin-top: 0; margin-right: 10px;" role="button"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>
                        <a href="#" data-btn-login="true">Ich bin schon Nutzer!</a>
                    </p>
                </div>


            <% }else{ %>
        
                <div id="noOwnCurrentDatesInfo" class="bs-callout bs-callout-info" style="margin-top: 0; margin-bottom: 10px;  <% if (Model.Dates.Any()) Response.Write("display: none;"); %>;">
                    <h4>Du hast keine aktuellen Termine</h4>
                    <p>
                        <a href="<%= Url.Action("Create", "EditDate") %>" class="btn btn-primary btn-sm" style="margin-top: 10px;">
                            <i class="fa fa-plus-circle"></i> &nbsp; Termin erstellen
                        </a>
                    </p>
                    <p style="padding-top: 5px;">
                        Termine helfen dir dabei, dich optimal auf eine Prüfung vorzubereiten. Das geht ganz einfach:
                    </p>
                    <div class="row explanationRow">
                        <div class="col-xs-3 explanationDiv" style="margin-left: 2%;">
                            <p><a href="<%= Links.DateCreate() %>"><i class="fa fa-3x fa-calendar-check-o"></i></a></p>
                            <p>Wähle dein Prüfungsdatum</p>
                        </div>
                        <div class="col-xs-1 arrowDiv">
                            <i class="fa fa-lg fa-arrow-right"></i>
                        </div>
                        <div class="col-xs-3 explanationDiv">
                            <p><a href="<%= Links.DateCreate() %>"><i class="fa fa-3x fa-book"></i></a></p>
                            <p>Wähle die Lerninhalte</p>
                        </div>
                        <div class="col-xs-1 arrowDiv">
                            <i class="fa fa-lg fa-arrow-right"></i>
                        </div>
                        <div class="col-xs-3 explanationDiv">
                            <p><a href="<%= Links.DateCreate() %>"><i class="fa fa-3x fa-bar-chart"></i></a></p>
                            <p>memucho erstellt deinen persönlichen Lernplan</p>
                        </div>
                    </div>
                    <p><b>Deine 5 Vorteile:</b></p>
                    <ul>
                        <li><b>Mehr Freizeit:</b> Durch das optimierte Lernen sparst du Zeit.</li>
                        <li><b>Planbarkeit:</b> Du siehst sofort, wieviel Zeit du zum Lernen einplanen musst.</li>
                        <li><b>Effizienz:</b> Du lernst immer nur das, was du wirklich lernen musst.</li>
                        <li><b>Sicherheit:</b> Optimierte Algorithmen erinnern dich daran zu lernen, bevor du vergisst.</li>
                        <li><b>Übersicht:</b> Du hast deinen aktuellen Wissensstand immer im Blick.</li>
                        <%--<li><b>Erinnerung:</b> memucho benachrichtigt dich, wenn deine nächste Lernsitzung ansteht.</li>--%>
                    </ul>
                </div>

                <% foreach(var date in Model.Dates){ %>
                    <% Html.RenderPartial("~/Views/Knowledge/Dates/DateRow.ascx", new DateRowModel(date)); %>
                <% } %>
                <div id="endOfFutureDates"></div>

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
                        <p style="padding-top: 5px;">
                        <% if (!Model.IsFollowingAnyUsers) { %>
                            Du folgst noch keinem anderen Nutzer. Erweitere dein Netzwerk und folge deinen Freunden. Dann kannst du Termine von ihnen übernehmen und ihr könnt gemeinsam lernen.
                        <% } else { %>
                            Erweitere dein Netzwerk und folge weiteren Freunden. Dann kannst du Termine von ihnen übernehmen und ihr könnt gemeinsam lernen.
                        <% } %>
                        </p>                        
                        <p>
                            <a href="<%= Url.Action("Users", "Users") %>" class="btn btn-primary btn-sm" style="margin-top: 10px;">
                                <i class="fa fa-plus-circle"></i> &nbsp; Netzwerk erweitern
                            </a>
                        </p>
                    </div>
                <% } else { %>
                    <% foreach(var date in Model.DatesInNetwork){ %>
                        <% Html.RenderPartial("~/Views/Knowledge/Dates/DateRow.ascx", new DateRowModel(date, isNetworkDate:true)); %>
                    <% } %>            
                <% } %>

            <% } %>

        </div>
    </div>
  
    <% Html.RenderPartial("~/Views/Knowledge/Dates/Modals/DeleteDate.ascx"); %>
    <% Html.RenderPartial("~/Views/Knowledge/Dates/Modals/TrainingSettings.ascx"); %>
    <% Html.RenderPartial("~/Views/Knowledge/Dates/Modals/CopyDate.ascx"); %>
    <%= Scripts.Render("~/bundles/js/KnowledgeDates") %>
            
