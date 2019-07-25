<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>


<input type="number" id="graphMaxLevel" onchange="setGraph()" value="3">
<input type="number" id="graphMaxNodeCount" onchange="setGraph()" value="50">
<input type="checkbox" id="graphShowKnowledgeGraph" onclick="setGraph()">

<div class="btn btn-primary" onclick="setGraph()">Aktualisieren</div>   

<div class="btn btn-primary" onclick="toggleRad()">Ansicht 1</div>       
<div class="btn btn-primary" onclick="toggleRect()">Ansicht 2 (experimentell)</div>       

<div class="btn btn-primary" onclick="toggleFullscreen()">Vollbild</div>       


<script type="text/javascript">

    var graphJsonString = "<%= @Model.GraphDataString %>";
    document.getElementById("graphShowKnowledgeGraph").checked = true;

    setGraph();

    function setGraph() {
        var maxLevel = document.getElementById("graphMaxLevel").value;
        var maxNodeCount = document.getElementById("graphMaxNodeCount").value;
        var showKnowledgeGraph = true;
        if (document.getElementById("graphShowKnowledgeGraph").checked)
            showKnowledgeGraph = true;
        else showKnowledgeGraph = false;

        KnowledgeGraph.setGraphData(maxLevel, maxNodeCount, showKnowledgeGraph);
    }

    function toggleRad() {
        $('svg').empty();
        KnowledgeGraph.loadRadialNodeGraph();
    }
    function toggleRect() {
        $('svg').empty();
        KnowledgeGraph.loadRectangleNodeGraph();
    }

    function toggleFullscreen() {
        var svg = document.querySelector('svg');
        var rfs = svg.requestFullscreen || svg.webkitRequestFullScreen || svg.mozRequestFullScreen || svg.msRequestFullscreen;
        rfs.call(svg);
    }

</script>
