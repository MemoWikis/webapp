class TrainingSettings {

    private _dateId : number;

    constructor() {
        var self = this;

        $('a[href*=#modalTraining]').click(function () {
            self._dateId = +$(this).attr("data-dateId");
            self.Populate(self._dateId);
        });

        var delay = (() => {
            var timer = 0;
            return (callback, ms) => {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        $("#txtQuestionsPerDateIdealAmount," +
          "#txtAnswerProbabilityTreshhold," +
          "#txtQuestionsPerDateMinimum," +
          "#txtSpacingBetweenSessionsInMinutes").keyup(() => {

              $("#divTrainingPlanDetailsSpinner").show();
              $("#divTrainingPlanDetails").hide();

              delay(() => {
                  self.GetSettingsFromUi();
                  console.log(JSON.stringify(self.GetSettingsFromUi()));
                  console.log(self.GetSettingsFromUi());

                  $.post("/Dates/RenderTrainingDates/",
                      { dateId: self._dateId, planSettings: self.GetSettingsFromUi() },
                      (result) => {
                          console.log(result);
                      });

                  $("#divTrainingPlanDetailsSpinner").hide();
                  $("#divTrainingPlanDetails").show();
              }, 800);
            
        });

        self.ShowSettings();
    }

    GetSettingsFromUi(): TrainingPlanSettings {
        var result = new TrainingPlanSettings();
        result.AnswerProbabilityTreshhold = $("#txtAnswerProbabilityTreshhold").val();
        result.QuestionsPerDate_IdealAmount = $("#txtQuestionsPerDateIdealAmount").val();
        result.QuestionsPerDate_Minimum = $("#txtQuestionsPerDateMinimum").val();
        result.SpacingBetweenSessionsInMinutes = $("#txtSpacingBetweenSessionsInMinutes").val();
        return result;
    }

    Populate(dateId: number) {

        var self = this;

        $.get("/Dates/RenderTrainingDates/?dateId=" + dateId,
            htmlResult => {
                self.RenderDetails(htmlResult);
            }
        );

        $("[data-action=closeSettings]").click(() => {
            self.HideSettings();
        });

        $("[data-action=showSettings]").click(() => {
            self.ShowSettings();
        });
    }

    RenderDetails(html) {
        $("#dateRows").children().remove();
        $("#dateRows").append(
            $(html)
                .animate({ opacity: 1.00 }, 700)
        ).after(() => {
            this.DrawCharts();
            $('#modalTraining').modal();
        });        
    }

    HideSettings() {
        $("#settings").hide(300);
        $("#showSettings").show();
        $("#closeSettings").hide();
    }

    ShowSettings() {
        $("#settings").show(300);
        $("#showSettings").hide();
        $("#closeSettings").show();
    }

    DrawCharts() {

        var self = this;

        $("#modalTraining div[data-trainingDateId]").each(function () {

            $(this).find("[data-knowledgeSummary]").each(function() {
                self.DrawKnowledgeChartDate2($(this));
            });

        });
    }

    DrawKnowledgeChartDate2(div: JQuery) {

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