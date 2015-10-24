/// <reference path="../../scripts/typescript.defs/google.visualization.d.ts" />

google.load("visualization", "1", { packages: ["corechart"] });

class ForgettingCurves {

    constructor() {
        this.DrawExplore();    
        this.DrawSuggested1And2();
    }

    DrawExplore() {
        var data = google.visualization.arrayToDataTable([
            ['Age', 'Leichte', 'Schwer', 'Mittel', 'Nobrainer'],
            [8, 3, 2, 4, 2],
            [4, 12, 2, 1, 3],
            [11, 12, 1, 2, 2],
            [4, 12, 2, 4, 5],
            [3, 12, 3, 1, 2],
            [6.5, 3, 5, 9, 3]
        ]);

        var options = this.GetOptions();
        options.legend.position = 'right';

        var chartExplore = new google.visualization.ScatterChart(window.document.getElementById('chartExplore'));
        chartExplore.draw(data, options);        
    }

    DrawSuggested1And2() {

        var options = this.GetOptions();

        var chartSuggested1 = new google.visualization.ScatterChart(window.document.getElementById('chartSuggested1'));
        chartSuggested1.draw(this.GetSampleData(), options);

        var chartSuggested2 = new google.visualization.ScatterChart(window.document.getElementById('chartSuggested2'));
        chartSuggested2.draw(this.GetSampleData(), options);
    }

    GetOptions() {
        return {
            hAxis: { minValue: 0, maxValue: 15 },
            vAxis: { minValue: 0, maxValue: 15 },
            chartArea: { width: '90%' },
            legend: { position: 'top' },
            trendlines: {
                0: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: true
                },
                1: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: true
                },
                2: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: true
                },
                3: {
                    type: 'exponential',
                    showR2: true,
                    visibleInLegend: true
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