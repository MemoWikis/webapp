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
            (result) => {
                window.console.log(result);
                for (var i = 0; i < 4; i++) {
                    $("#pairCount" + i).html(result.Data.ChartInfos[i].TotalPairs + "P");
                    $("#regressionValue" + i).html("R" + result.Data.ChartInfos[i].RegressionValue);
                    
                }

                this.DrawExplore(result.Data.ChartData);
            });
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
        chartExplore.draw(dt, options); //        chartExplore.draw(this.GetSampleData() vs dt, options);

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
            ['Age', 'Alle (No-Brainer)', 'X-Days-Exactly-3 (Alle)', 'Max-Strak-Of-4 (Mittelschwer)'],
            [1, 99, 89, 82],
            [1.2, 98, 85, 70],
            [1.3, 98, 86, 72],
            [1.6, 95, 82, 67],
            [1.7, 98, 83, 67],
            [1.9, 97, , 69],
            [2.2, 98, 79, 70],
            [2.3, 94, 75, 68],
            [2.5, 95, 78, 12],
            [2.9, 93, 76, 67],
            [3.3, 92, 67, 66],
            [3.4, 95, , 65],
            [3.9, 98, 62, 67],
            [4.4, 21, , 55],
            [4.5, 95, 60, 54],
            [4.6, 93, , 51],
            [4.9, 91, 40, 56],
            [5.0, 92, 59, 40],
            [5.2, 91, 58, 55],
            [5.4, 90, , 56],
            [5.5, 89, 59, 51],
            [5.9, 81, 56, 50],
            [6.0, 84, , 61],
            [6.1, 83, 57, ,],
            [6.2, 88, 53, ,],
            [6.8, 89, 55, 45],
            [6.9, 90, , 51],
            [7.4, 87, 53, 40],
            [8.3, 86, 50, 44],
            [9.0, 92, 41, 34],
            [9.3, 85, 38, 35],
            [9.9, 84, 40, 30],
            [10.3, 81, 15, 9],
            [10.5, , , 20],
            [10.8, 70, 33, ,]
        ]);
    }
}

$(() => {
    google.setOnLoadCallback(() => { new ForgettingCurves() });
})