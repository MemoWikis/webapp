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

        $("#txtQuestionsPerDateIdealAmount," +
          "#txtAnswerProbabilityThreshold," +
          "#txtQuestionsPerDateMinimum," +
          "#txtMinSpacingBetweenSessionsInMinutes," +
          "#txtEqualizedSpacingMaxMultiplier," + 
          "#txtEqualizedSpacingDelayerDays").keyup(() => {
              this.UpdateTrainingPlanAfterSettingsChange(self._dateId);
        });

        $("#chkbxEqualizeSpacingBetweenSessions").change(() => {
            this.ToggleEqualizeSpacingOptions();
            this.UpdateTrainingPlanAfterSettingsChange(self._dateId);
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
        $("#chartTrainingTime").empty();
        this.RenderDetails(data.Html);
        $("#modalTraining #RemainingDates").html(data.RemainingDates);
        $("#modalTraining #RemainingTime").html(data.RemainingTime);
        $("#modalTraining #QuestionCount").html(data.QuestionCount);
        $("#modalTraining #DateOfDate").html(data.DateOfDate);

        $("#modalTraining #txtQuestionsPerDateIdealAmount").val(data.QuestionsPerDateIdealAmount); 
        $("#modalTraining #txtAnswerProbabilityThreshold").val(data.AnswerProbabilityThreshold);
        $("#modalTraining #txtQuestionsPerDateMinimum").val(data.QuestionsPerDateMinimum);
        $("#modalTraining #txtMinSpacingBetweenSessionsInMinutes").val(data.MinSpacingBetweenSessionsInMinutes);
        $("#modalTraining #chkbxEqualizeSpacingBetweenSessions").prop("checked", data.EqualizeSpacingBetweenSessions);
        $("#modalTraining #txtEqualizedSpacingMaxMultiplier").val(data.EqualizedSpacingMaxMultiplier);
        $("#modalTraining #txtEqualizedSpacingDelayerDays").val(data.EqualizedSpacingDelayerDays);
        this.ToggleEqualizeSpacingOptions();
        
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

    UpdateTrainingPlanAfterSettingsChange(dateId: number) {
        var self = this;
        self._dateId = dateId;
        var delay = (() => {
            var timer = 0;
            return (callback, ms) => {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

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
    }

    GetSettingsFromUi(): TrainingPlanSettings {
        var result = new TrainingPlanSettings();
        result.AnswerProbabilityThreshold = $("#txtAnswerProbabilityThreshold").val();
        result.QuestionsPerDate_IdealAmount = $("#txtQuestionsPerDateIdealAmount").val();
        result.QuestionsPerDate_Minimum = $("#txtQuestionsPerDateMinimum").val();
        result.MinSpacingBetweenSessionsInMinutes = $("#txtMinSpacingBetweenSessionsInMinutes").val();
        result.EqualizeSpacingBetweenSessions = $("#chkbxEqualizeSpacingBetweenSessions").prop("checked");
        result.EqualizedSpacingMaxMultiplier = $("#txtEqualizedSpacingMaxMultiplier").val();
        result.EqualizedSpacingDelayerDays = $("#txtEqualizedSpacingDelayerDays").val();
        return result;
    }

    ToggleEqualizeSpacingOptions() {
        //text inputs are not disabled; when greyed out, values can still be changed and trainingplan is newly generated, even though changes made in greyed out textboxes won't affect trainingplan
        if ($("#chkbxEqualizeSpacingBetweenSessions").prop("checked")) {
            $(".EqualizeSpacingOptions").css("opacity", "1");
            $("#txtEqualizedSpacingMaxMultiplier").prop("disabled", false);
            $("#txtEqualizedSpacingDelayerDays").prop("disabled", false);
        } else {
            $(".EqualizeSpacingOptions").css("opacity", "0.5");
            $("#txtEqualizedSpacingMaxMultiplier").prop("disabled", true);
            $("#txtEqualizedSpacingDelayerDays").prop("disabled", true);
        }
    }

    RenderDetails(html) {
        $("#dateRows").children().remove();
        $("#dateRows").append(
            $(html)
                .animate({ opacity: 1.00 }, 700)
        ).after(() => {
            this.DrawKnowledgeCharts();
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

    DrawKnowledgeCharts() {
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
                0: { color: '#3e7700' },
                1: { color: '#fdd648' },
                2: { color: 'B13A48' },
                3: { color: 'EFEFEF' }
            },
            pieStartAngle: 0
        };

        var chart = new google.visualization.PieChart(div.get()[0]);
        chart.draw(data, options2);
    }


    DrawChartTrainingTime(rowsToDraw) {
        var rowsAsArray = JSON.parse(rowsToDraw.toString());
        var data = new google.visualization.DataTable();
        data.addColumn('date', 'Datum');
        if (rowsAsArray.length > 0) {
            for (var i = 1; i <= rowsAsArray[0].length - 1; i++) {
                data.addColumn('number', 'Übungssitzung ' + i);
            }

            for (var i = 0; i < rowsAsArray.length; i++) {
                //need to create date from first column in every row; alternative: handle it as string to be displayed as received here
                rowsAsArray[i][0] = new Date(rowsAsArray[i][0]);
                for (var j = 2; j < rowsAsArray[i].length; j++) { //change 0 to null to avoid barchart entry with "0". if no dates at a date (== 0 is the first entry), don't remove it, otherwise whole row is ignored
                    if (rowsAsArray[i][j] == 0)
                        rowsAsArray[i][j] = null;
                }
            }
        }

        data.addRows(rowsAsArray);
        
        var view = new google.visualization.DataView(data);

        var options = {
            title: "Übungssitzungen bis zum Termin",
            hAxis: {
                title: "", 
                format: "d.M." //alternative: format as string
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