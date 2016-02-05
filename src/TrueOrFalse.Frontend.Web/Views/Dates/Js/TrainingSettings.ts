class TrainingSettings {

    private _dateId: number;
    private _isDateDropDownInitialized: boolean;
    private _ddlDates: JQuery; 

    constructor() {
        var self = this;

        self._ddlDates = $("#modalTraining #SelectTrainingDates");
        self._ddlDates.change(function () {
            self.Populate(this.value);
        });

        $('a[href*=#modalTraining]').click(function () {
            self._dateId = parseInt($(this).attr("data-dateId"));

            if (!self._isDateDropDownInitialized) {
                self._isDateDropDownInitialized = true;
                self.PopulateDropDown();
            }

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

                  $.post("/Dates/TrainingPlanUpdate/",
                      { dateId: self._dateId, planSettings: self.GetSettingsFromUi() },
                      (result) => {
                          self.RenderTrainingPlan(result);
                          $("#divTrainingPlanDetailsSpinner").hide();
                          $("#divTrainingPlanDetails").show();
                      });
              }, 800);
            
        });

        self.ShowSettings();
    }

    PopulateDropDown() {
        var self = this;
        $.post("/Dates/GetUpcomingDatesJson/", (result) => {
            jQuery.each(result.AllUpcomingDates, (index, item) => {
                var option = $("<option value='" + item.DateId + "'>" + item.Title + "</option>");
                self._ddlDates.append(option);
            });

            self._ddlDates.val(self._dateId.toString());
        });        
    }

    RenderTrainingPlan(data) {
        this.RenderDetails(data.Html);
        $("#modalTraining #RemainingDates").html(data.RemainingDates);
        $("#modalTraining #RemainingTime").html(data.RemainingTime);
        $("#modalTraining #QuestionCount").html(data.QuestionCount); 
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
        self._dateId = dateId;

        $.post("/Dates/TrainingPlanGet", {dateId : dateId}, 
            result => {
                self.RenderTrainingPlan(result);
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