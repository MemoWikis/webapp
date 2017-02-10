<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeSummary>" %>

<script>
        
    google.load("visualization", "1", { packages: ["corechart"] });

    google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });

    function drawKnowledgeChart(chartElementId) {

        if ($("#" + chartElementId).length === 0) {
            return;
        }

        var data = google.visualization.arrayToDataTable([
            ['Wissenslevel', 'link', 'Anteil in %'],
            ['Sicheres Wissen', '/Fragen/Wunschwissen/?filter=solid', <%= Model.Solid %>],
            ['Solltest du festigen', '/Fragen/Wunschwissen/?filter=consolidate', <%= Model.NeedsConsolidation %>],
            ['Solltest du lernen', '/Fragen/Wunschwissen/?filter=learn', <%= Model.NeedsLearning %>],
            ['Noch nicht gelernt', '/Fragen/Wunschwissen/?filter=notLearned', <%= Model.NotLearned %>],
            ['Nicht im Wunschwissen', '', <%= Model.NotInWishknowledge %>]
        ]);

        var options = {
            pieHole: 0.6,
            tooltip: { isHtml: true },
            legend: { position: 'labeled' },
            pieSliceText: 'none',
            chartArea: { 'width': '100%', height: '100%', top: 10},
            slices: {
                0: { color: '#afd534' },
                1: { color: '#fdd648' },
                2: { color: 'lightsalmon' },
                3: { color: 'silver' },
                4: { color: '#dddddd' }
            },
            pieStartAngle: 0
        };

        var view = new google.visualization.DataView(data);
        view.setColumns([0, 2]);

        var chart = new google.visualization.PieChart(document.getElementById(chartElementId));
        chart.draw(view, options);

        google.visualization.events.addListener(chart, 'select', selectHandler);

        function selectHandler(e) {
            var urlPart = data.getValue(chart.getSelection()[0].row, 1);
            location.href = urlPart;
        }
    }
</script>

<div id="chartKnowledge" style="height: 150px; text-align: left;"></div>