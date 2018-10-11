<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeSummary>" %>

<script>


    google.load("visualization", "1", { packages: ["corechart"] });

    google.setOnLoadCallback(function () { drawKnowledgeChart("chartKnowledge") });

    function drawKnowledgeChart(chartElementId) {
        var options = null;
        var option = "<%= Model.Options%>";

        switch (option) {
            case "standard":
                options = {
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
                $("#chartKnowledge").css({ "height": "150px" });
                break;
            case "withoutLegend":
                options = {
                    pieHole: 0.6,
                    tooltip: { isHtml: true },
                    legend: { position: "none" },
                    pieSliceText: 'none',
                    chartArea: { 'width': '100%', height: '100px', top: 10 },
                    slices: {
                        0: { color: '#afd534' },
                        1: { color: '#fdd648' },
                        2: { color: 'lightsalmon' },
                        3: { color: 'silver' },
                        4: { color: '#dddddd' }
                    },
                    pieStartAngle: 0
                }
               // $("#chartKnowledge").addClass("col col-md-6");
                $("#chartKnowledge").css({ "height": "80px", "text-align": "center" });
                break;
        }



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

<div id="chartKnowledge" style="text-align: left;"></div>

