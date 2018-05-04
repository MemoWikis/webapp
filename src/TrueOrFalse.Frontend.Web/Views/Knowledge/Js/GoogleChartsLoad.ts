class GoogleChartsLoad {
    private Tabledata: any;
    Google:any;


    constructor() {
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function() { drawKnowledgeChart("test") });


    }

    public drawKnowledgeChart(chartElementId) {
      var options = {
            pieHole: 0.6,
            tooltip: { isHtml: true },
            legend: { position: "labeled" },
            pieSliceText: 'none',
            chartArea: { 'width': '100%', height: '100%', top: 10 },
            slices: {
                0: { color: '#afd534' },
                1: { color: '#fdd648' },
                2: { color: 'lightsalmon' },
                3: { color: 'silver' },
                4: { color: '#dddddd' }
            },
            pieStartAngle: 0
        }


        if ($("#" + chartElementId).length === 0) {
            return;
        }

        var data = google.visualization.arrayToDataTable([
            ['Wissenslevel', 'link', 'Anteil in %'],
            ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', "14"],
            ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', "10"],
            ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', "15"],
            ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', "20"],
            ['Nicht im Wunschwissen', '', "22"]
        ]);

    

        var view = new google.visualization.DataView(data);
        view.setColumns([0, 2]);

        var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
        chart.draw(view, options);

        this.Google.visualization.events.addListener(chart, 'select', selectHandler);

        function selectHandler(e) {
            var urlPart = data.getValue(chart.getSelection()[0].row, 1);
            location.href = urlPart;
        }
    }


    
   

}

class OptionsTable {
    public PieHole: number;
    public Tooltip: boolean;
    public Legend: string; 
    public PieSliceText: string;
    public chartArea: any[];
    public slices: string[];
    public PieStartAngle: number;

    constructor(pieHole: number, tooltip: boolean, legend: string, pieSlicetext: string, chartArea: any[], slices: string[], pieStartAngle: number) {
        this.PieHole = pieHole;
        this.Tooltip = tooltip;
        this.Legend = legend;
        this.PieSliceText = pieSlicetext;
        this.chartArea = chartArea;
        this.slices = slices;
        this.PieStartAngle = pieStartAngle;

    }


}