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

        $("#allDateRows").on("click", 'a[href*=#modalTraining]', function () {
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
          "#txtAnswerProbabilityThreshold," +
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

        $("[data-action=showAdvancedSettings]").click(() => {
            $("#divAdvancedSettings").show(300);
            $("[data-action=showAdvancedSettings]").hide();
            $("[data-action=hideAdvancedSettings]").show();
        });
        $("[data-action=hideAdvancedSettings]").click(() => {
            $("#divAdvancedSettings").hide(300);
            $("[data-action=showAdvancedSettings]").show();
            $("[data-action=hideAdvancedSettings]").hide();
        });

        $("[data-action=closeSettings]").click(() => {
            self.HideSettings();
        });

        $("[data-action=showSettings]").click(() => {
            self.ShowSettings();
        });
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

    Populate(dateId: number) {

        var self = this;
        self._dateId = dateId;

        $.post("/Dates/TrainingPlanGet", { dateId: dateId },
            result => {
                self.RenderTrainingPlan(result);
            }
        );
    }

    RenderTrainingPlan(data) {
        this.RenderDetails(data.Html);
        $("#modalTraining #RemainingDates").html(data.RemainingDates);
        $("#modalTraining #RemainingTime").html(data.RemainingTime);
        $("#modalTraining #QuestionCount").html(data.QuestionCount);
        $("#modalTraining #DateOfDate").html(data.DateOfDate);

        $("#modalTraining #txtQuestionsPerDateIdealAmount").val(data.QuestionsPerDateIdealAmount); 
        $("#modalTraining #txtAnswerProbabilityThreshold").val(data.AnswerProbabilityThreshold);
        $("#modalTraining #txtQuestionsPerDateMinimum").val(data.QuestionsPerDateMinimum);
        $("#modalTraining #txtSpacingBetweenSessionsInMinutes").val(data.SpacingBetweenSessionsInMinutes);

        
        var renderChartIfDivHasWidth = () => {
            window.setTimeout(() => {
                if ($("#chartTrainingTime").width() > 1) 
                    this.DrawChartTrainingTime(data.ChartTrainingTimeRows);
                else
                    renderChartIfDivHasWidth();
            }, 20); 
        }
        renderChartIfDivHasWidth();
    }

    GetSettingsFromUi(): TrainingPlanSettings {
        var result = new TrainingPlanSettings();
        result.AnswerProbabilityThreshold = $("#txtAnswerProbabilityThreshold").val();
        result.QuestionsPerDate_IdealAmount = $("#txtQuestionsPerDateIdealAmount").val();
        result.QuestionsPerDate_Minimum = $("#txtQuestionsPerDateMinimum").val();
        result.SpacingBetweenSessionsInMinutes = $("#txtSpacingBetweenSessionsInMinutes").val();
        return result;
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


    DrawChartTrainingTime(rowsToDraw) {
        var rowsAsArray = JSON.parse(rowsToDraw.toString());
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Datum');
        if (rowsAsArray.length > 0) {
            for (var i = 1; i <= rowsAsArray[0].length - 1; i++) {
                data.addColumn('number', 'Übungssitzung ' + i);
            }
        }

        data.addRows(rowsAsArray);
        
        var view = new google.visualization.DataView(data);
        //view.setColumns([0, 1, 2,
        //    {
        //        calc: "stringify",
        //        sourceColumn: 1,
        //        type: "string",
        //        role: "annotation"
        //    },
        //    2]); //this is responsible for putting numbers in columns, but also for duplicating columns

        var options = {
            title: "Übungssitzungen bis zum Termin",
            hAxis: {
                title: "" //format: string //changeto "day" or something like this
            },
            vAxis: {
                title: "Anzahl Fragen"
            },
            legend: { position: 'none'},
            bar: { groupWidth: '80%' },
            chartArea: { 'width': '92%', 'left': 40},
            colors: ['#afd534', '#afd534'],
            isStacked: true
        };

        var chart = new google.visualization.ColumnChart(document.getElementById("chartTrainingTime"));
        chart.draw(view, options);

    }
}