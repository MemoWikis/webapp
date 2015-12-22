<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<script>
    
    $.get("/Dates/RenderTrainingDates/?dateId=10",
        htmlResult => {
            $("#dateRows").append(
                $(htmlResult)
                    .animate({ opacity: 1.00 }, 700)
            );
            drawCharts();
        }
    );

    function drawCharts() {
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate1Before", 9, 2, 1, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate1After", 4, 3, 2, 3) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate2Before", 9, 2, 1, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate2After", 4, 3, 2, 3) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate3Before", 9, 2, 1, 2) });
        google.setOnLoadCallback(function () { drawKnowledgeChartDate2("chartKnowledgeDate3After", 4, 3, 2, 3) });
    }

    function drawKnowledgeChartDate2(chartElementId, amountSolid, amountToConsolidate, amountToLearn, amountNotLearned) {

        var chartElement = $("#" + chartElementId);

        var data = google.visualization.arrayToDataTable([
            ['Wissenslevel', 'Anteil in %'],
            ['Sicheres Wissen', amountSolid],
            ['Solltest du festigen', amountToConsolidate],
            ['Solltest du lernen', amountToLearn],
            ['Noch nicht gelernt', amountNotLearned],
        ]);

        var options2 = {
            pieHole: 0.5,
            legend: { position: 'none' },
            pieSliceText: 'none',
            height: 36,
            width: 42,
            chartArea: { width: 42, height: 36, left: 0 },
            slices: {
                0: { color: 'lightgreen' },
                1: { color: '#fdd648' },
                2: { color: 'lightsalmon' },
                3: { color: 'silver' }
            },
            pieStartAngle: 0
        };

        var chart = new google.visualization.PieChart(chartElement.get()[0]);
        chart.draw(data, options2);
    }

    $(function () {
        $("[data-action=closeSettings]").click(function () {
            $("#settings").hide(300);
            $("#showSettings").show();
            $("#closeSettings").hide();
        });
    });

    $(function () {
        $("[data-action=showSettings]").click(function () {
            $("#settings").show(300);
            $("#showSettings").hide();
            $("#closeSettings").show();
        });
    });

</script>

<div id="modalTraining" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                <button class="close" data-dismiss="modal">×</button>
                <div class="row">
                    <div class="col-md-6">
                        <h3>Übungsplan</h3>
                    </div>
                    <div class="col-md-6">
                        <div class="pull-right" style="font-size: 16px; margin-top: 5px;">
                            <a href="#" id="showSettings" data-action="showSettings">zeige Einstellungen <i class="fa fa-cogs"></i></a>
                            <a href="#" style="display:none" id="closeSettings" data-action="closeSettings">verberge Einstellungen <i class="fa fa-cogs"></i></a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-body" style="padding-top: 0px;">
                <div id="settings" style="display:none">
                    <div class="row" style="margin-top: 0px;">
                        <div class="col-md-12">
                            <h4 class="ColoredUnderline Date">
                                Einstellungen
                                <a href="#" class="pull-right" style="font-size: 11px;" data-action="closeSettings">x schließen</a>
                            </h4>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Lernstrategie:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <select>
                                <option>Nachhaltig lernen</option>
                                <option>Minimaler Aufwand</option>
                            </select>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Ruhezeiten:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            jeden Tag 22:00-8:00 u. an Sonntagen <i class="fa fa-plus-square-o"></i>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4"></div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <a href="#">erweiterte Einstellungen verbergen <i class="fa fa-caret-down"></i></a>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 12px;">
                        <div class="col-md-4">
                            <div class="pull-right">Fragen pro Termin Ideal:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="10"/>
                        </div>
                        <div class="col-md-4">
                            <div class="pull-right">Vergessensschwelle:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="92"/>%
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Mind. pro Termin:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="7"/>
                        </div>
                        <div class="col-md-4">
                            <div class="pull-right">mind. Zeit zw. Terminen:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="92"/>min
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Übersicht</h4></div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="font-size: 16px;">
                        <span style="padding-right: 12px">Verbleibend: ca. 7 Übungssitzungen </span>
                        <span style="padding-right: 12px"><i class="fa fa-clock-o"></i> ca. 1:20h Übungszeit</span>
                        <br/><span>Zu lernen: 31 Fragen</span>
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Terminvorschläge</h4></div>
                </div>
                <div class="row" style="margin-top: 4px">
                    <div class="col-md-8" style="">
                        Bis Benachrichtigung
                    </div>
                    <div class="col-md-2">Vorher</div>
                    <div class="col-md-2">Nachher</div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div style="height: 1px; background-color: rgb(219, 219, 219);"></div>
                    </div>
                </div>
                <div id="dateRows"></div>
            </div>
            <div class="modal-footer" style="padding-top: 7px; padding-bottom: 7px;">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schliessen</a>
            </div>
        </div>
    </div>
</div>