<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>

<script type="text/javascript">

    var graphJsonString = "<%= @Model.GraphDataString %>";

    KnowledgeGraph.initGraphData();
    initSettings();
    setGraph();

    $('#graphDropdown').on('click', function(e) {
        $('#graphToolbar').toggleClass('open');
    });

    $('body').on('click', function (e) {
        if (!$('#graphToolbar').is(e.target)
            && $('#graphDropdown').has(e.target).length === 0
            && $('.open').has(e.target).length === 0
        ) {
            $('#graphToolbar').removeClass('open');
        }
    });

    function setNodeCountValue(val) {

        var newNodeCount;

        if (val == 'slider') {
            newNodeCount = document.getElementById("graphMaxNodeCount").value;
            document.getElementById("nodeCountValue").value = newNodeCount;
        } else {
            newNodeCount = document.getElementById("nodeCountValue").value;
            document.getElementById("graphMaxNodeCount").value = newNodeCount;
        }

        if (newNodeCount > 100)
            $('#nodeCountWarning').removeClass("hidden");
        else
            $('#nodeCountWarning').addClass("hidden");
    }

    function setNodeLevelValue(val) {
        if (val == 'slider')
            document.getElementById("nodeLevelValue").value = document.getElementById("graphMaxLevel").value;
        else
            document.getElementById("graphMaxLevel").value = document.getElementById("nodeLevelValue").value;
    }

    function initSettings()
    {
        $('#radNodeButton').addClass('selected');

        if (graphDepth == -1) {
            document.getElementById("graphMaxLevel").max = 11;
            document.getElementById("nodeLevelValue").max = 11;
        } else {
            document.getElementById("graphMaxLevel").max = graphDepth;
            document.getElementById("nodeLevelValue").max = graphDepth;
        }

        if (graphDepth < 3 && graphDepth > -1) {
            document.getElementById("graphMaxLevel").value = graphDepth;
            document.getElementById("nodeLevelValue").value = graphDepth;
        }

        document.getElementById("graphMaxNodeCount").max = nodeCount;
        document.getElementById("nodeCountValue").max = nodeCount;
        if (nodeCount < 50) {
            document.getElementById("graphMaxNodeCount").value = nodeCount;
            document.getElementById("nodeCountValue").value = nodeCount;
        }

        document.getElementById("graphShowKnowledgeGraph").checked = true;
    }

    function setGraph() {

        var maxLevel = document.getElementById("graphMaxLevel").value;
        var maxNodeCount = document.getElementById("graphMaxNodeCount").value;
        var showKnowledgeGraph = true;
        if (document.getElementById("graphShowKnowledgeGraph").checked)
            showKnowledgeGraph = true;
        else showKnowledgeGraph = false;

        $('svg').empty();
        KnowledgeGraph.setGraphData(maxLevel, maxNodeCount, showKnowledgeGraph);
    }

    function toggleRad() {
        document.getElementById("graphMaxNodeCount").max = nodeCount;
        document.getElementById("nodeCountValue").max = nodeCount;
        $('#knowledgebarWarning').addClass("hidden");


        $('svg').empty();
        KnowledgeGraph.loadRadialNodeGraph();
        $('#rectNodeButton').removeClass('selected');
        $('#radNodeButton').addClass('selected');
        $('#knowledgeBarCheckBox').addClass('invisible');
    }
    function toggleRect() {
        if (nodeCount > 50)
            $('#knowledgebarWarning').removeClass("hidden");

        if (nodeCount > 50) {
            document.getElementById("graphMaxNodeCount").max = 50;
            document.getElementById("nodeCountValue").max = 50;
        }

        if (document.getElementById("graphMaxNodeCount").value > 50) {
            document.getElementById("graphMaxNodeCount").value = 50;
            document.getElementById("nodeCountValue").value = 50;
        }
        $('svg').empty();
        KnowledgeGraph.loadRectangleNodeGraph();
        setGraph();
        $('#radNodeButton').removeClass('selected');
        $('#rectNodeButton').addClass('selected');
        $('#knowledgeBarCheckBox').removeClass('invisible');
    }

    function toggleFullscreen() {
        var svg = document.querySelector('svg');
        var rfs = svg.requestFullscreen || svg.webkitRequestFullScreen || svg.mozRequestFullScreen || svg.msRequestFullscreen;
        rfs.call(svg);
    }

</script>
