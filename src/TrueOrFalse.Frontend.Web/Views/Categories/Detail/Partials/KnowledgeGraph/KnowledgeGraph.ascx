<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>



<script type="text/javascript">

    var graphJsonString = "<%= @Model.GraphDataString %>";

    KnowledgeGraph.initGraphData();

    initSettings();

    setGraph();

    function initSettings()
    {
        if (graphDepth == -1)
            document.getElementById("graphMaxLevel").max = 11;
        else
            document.getElementById("graphMaxLevel").max = graphDepth;

        if (graphDepth < 3 && graphDepth > -1)
            document.getElementById("graphMaxLevel").value = graphDepth;

        document.getElementById("graphMaxNodeCount").max = nodeCount;
        if (nodeCount < 50)
            document.getElementById("graphMaxNodeCount").value = nodeCount;

        document.getElementById("graphShowKnowledgeGraph").checked = true;
    }

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
