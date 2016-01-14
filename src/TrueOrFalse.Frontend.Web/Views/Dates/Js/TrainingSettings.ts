class TrainingSettings {
    constructor() {
        var self = this;

        $('a[href*=#modalTraining]').click(function () {
            var dateId = $(this).attr("data-dateId");
            self.Populate(dateId);
        });
    }

    Populate(dateId : string) {
        $.get("/Dates/RenderTrainingDates/?dateId=" + dateId,
            htmlResult => {
                $("#dateRows").children().remove();
                $("#dateRows").append(
                    $(htmlResult)
                    .animate({ opacity: 1.00 }, 700)
                ).after(() => {
                    drawCharts();
                });
                
            }
        );
        
        function drawCharts() {
            drawKnowledgeChartDate2("chartKnowledgeDate1Before", 9, 2, 1, 2);
            drawKnowledgeChartDate2("chartKnowledgeDate1After", 4, 3, 2, 3);
            drawKnowledgeChartDate2("chartKnowledgeDate2Before", 9, 2, 1, 2);
            drawKnowledgeChartDate2("chartKnowledgeDate2After", 4, 3, 2, 3);
            drawKnowledgeChartDate2("chartKnowledgeDate3Before", 9, 2, 1, 2);
            drawKnowledgeChartDate2("chartKnowledgeDate3After", 4, 3, 2, 3);
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
}