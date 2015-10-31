/// <reference path="../../scripts/typescript.defs/google.visualization.d.ts" />

google.load("visualization", "1", { packages: ["corechart"] });

class ForgettingCurves {

    constructor() {

        $("#ddlInterval").change((e) => { this.LoadExploreGraph(); });
        $("#txtIntervalCount").change((e) => {
            if (!isInt($("#txtIntervalCount").val()))
                $("#txtIntervalCount").val("30");

            this.LoadExploreGraph();
        });

        $("#ddlAnswerFeature0, #ddlAnswerFeature1, #ddlAnswerFeature2, #ddlAnswerFeature3").change(() => { this.LoadExploreGraph(); });
        $("#ddlQuestionFeature0, #ddlQuestionFeature1, #ddlQuestionFeature2, #ddlQuestionFeature3").change(() => { this.LoadExploreGraph(); });

        this.LoadExploreGraph();
        this.DrawSuggested1And2();

        function isInt(value) {
            if (isNaN(value)) {return false;}
            var x = parseFloat(value);
            return (x | 0) === x;
        }

    }

    LoadExploreGraph() {
        $.post("/AlgoInsight/ForgettingCurvesJson",
            {
                Interval: $("#ddlInterval option:selected").val(),
                IntervalCount: $("#txtIntervalCount").val(),
                Curves: [
                    { Show: true, AnswerFeatureId: $("#ddlAnswerFeature0").val(), QuestionFeatureId: $("#ddlQuestionFeature0").val() },
                    { Show: true, AnswerFeatureId: $("#ddlAnswerFeature1").val(), QuestionFeatureId: $("#ddlQuestionFeature1").val() },
                    { Show: true, AnswerFeatureId: $("#ddlAnswerFeature2").val(), QuestionFeatureId: $("#ddlQuestionFeature2").val() },
                    { Show: true, AnswerFeatureId: $("#ddlAnswerFeature3").val(), QuestionFeatureId: $("#ddlQuestionFeature3").val() }
                ]
            },
            (result) => { window.console.log(result); this.DrawExplore(result.Data); });
    }

    DrawExplore(jsonData) {

        var dt = new google.visualization.DataTable(jsonData);

        var options = this.GetOptions();
        options.legend.position = 'bottom';
        options.chartArea.width = '90%';
        options.chartArea.height = '80%';
        options.chartArea.left = "30";
        options.chartArea.top = "10";

        var chartExplore = new google.visualization.ScatterChart(window.document.getElementById('chartExplore'));
        chartExplore.draw(dt, options);
    }

    DrawSuggested1And2() {

        var options = this.GetOptions();

        var chartSuggested1 = new google.visualization.ScatterChart(window.document.getElementById('chartSuggested1'));
        chartSuggested1.draw(this.GetSampleData(), options);

        var chartSuggested2 = new google.visualization.ScatterChart(window.document.getElementById('chartSuggested2'));
        chartSuggested2.draw(this.GetSampleData(), options);
    }

    GetOptions(): google.visualization.ScatterChartOptions {
        return {
            hAxis: { minValue: 0},
            vAxis: { minValue: 0, maxValue: 100 },
            chartArea: { width: '90%' },
            legend: { position: 'top' },
            trendlines: {
                0: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: false
                },
                1: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: false
                },
                2: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: false
                },
                3: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: false
                },
            }
        }; 
    }

    GetSampleData() {
        return google.visualization.arrayToDataTable([
            ['Age', 'Leichte', 'Schwer', 'Mittel', 'Nobrainer'],
            [8, 3, 2, 4, 2],
            [4, 12, 2, 1, 3],
            [11, 12, 1, 2, 2],
            [4, 12, 2, 4, 5],
            [3, 12, 3, 1, 2],
            [6.5, 3, 5, 9, 3]
        ]);
    }
}

$(() => {
    google.setOnLoadCallback(() => { new ForgettingCurves() });
})