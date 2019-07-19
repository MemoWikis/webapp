<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>


<div class="btn btn-primary" onclick="toggleRad()">Ansicht 1</div>       
<div class="btn btn-primary" onclick="toggleRect()">Ansicht 2 (experimentell)</div>       

<div class="btn btn-primary" onclick="toggleFullscreen()">Vollbild</div>       


<script type="text/javascript">

    var graphJsonString = "<%= @Model.GraphDataString %>";
    var graphData = JSON.parse(graphJsonString);

    KnowledgeGraph.loadForceGraph();

    function toggleRad() {
        $('svg').empty();
        KnowledgeGraph.loadForceGraph();
    }
    function toggleRect() {
        $('svg').empty();
        KnowledgeGraph.loadDwarfGraph();
    }

    function toggleFullscreen() {
        var svg = document.querySelector('svg');
        var rfs = svg.requestFullscreen || svg.webkitRequestFullScreen || svg.mozRequestFullScreen || svg.msRequestFullscreen;
        rfs.call(svg);
    }

</script>
