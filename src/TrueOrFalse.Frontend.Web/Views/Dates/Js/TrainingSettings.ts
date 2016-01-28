class TrainingSettings {
    constructor() {
        var self = this;

        $('a[href*=#modalTraining]').click(function () {
            var dateId = $(this).attr("data-dateId");
            self.Populate(dateId);
        });
    }

    Populate(dateId: string) {

        $.get("/Dates/RenderTrainingDates/?dateId=" + dateId,
            htmlResult => {
                $("#dateRows").children().remove();
                $("#dateRows").append(
                    $(htmlResult)
                    .animate({ opacity: 1.00 }, 700)
                ).after(() => {
                    this.drawCharts();
                    $('#modalTraining').modal();
                });
            }
        );
        
        $(() => {
            $("[data-action=closeSettings]").click(() => {
                $("#settings").hide(300);
                $("#showSettings").show();
                $("#closeSettings").hide();
            });
        });

        $(() => {
            $("[data-action=showSettings]").click(() => {
                $("#settings").show(300);
                $("#showSettings").hide();
                $("#closeSettings").show();
            });
        });
    }

    drawCharts() {

        var self = this;

        $("#modalTraining div[data-trainingDateId]").each(function () {

            $(this).find("[data-knowledgeSummary]").each(function() {
                self.drawKnowledgeChartDate2($(this));
            });

        });
    }

    drawKnowledgeChartDate2(div: JQuery) {

        var json = div.attr("data-knowledgeSummary");
        var summary = <KnowledgeSummary>JSON.parse(json);

        var data = google.visualization.arrayToDataTable([
            ['Wissenslevel', 'Anteil in %'],
            ['Sicheres Wissen', summary.Solid],
            ['Solltest du festigen', summary.NeedsConsolidation],
            ['Solltest du lernen', summary.NeedsLearning],
            ['Noch nicht gelernt', summary.NotLearned],
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

        var chart = new google.visualization.PieChart(div.get()[0]);
        chart.draw(data, options2);
    }
}