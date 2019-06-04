<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>

<button type="button" onclick="loadGraph()">
    Test Load Graph
</button>

<script type="text/javascript">
    function loadGraph() {
        KnowledgeGraph.loadKnowledgeGraph("<%= @Model.GraphDataString %>");
    }

</script>
