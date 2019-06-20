<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>

<button onclick="toggleFullscreen()">open Fullscreen</button>
<button onclick="toggleRad()">open Rad</button>
<button onclick="toggleRect()">open Rect</button>


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
