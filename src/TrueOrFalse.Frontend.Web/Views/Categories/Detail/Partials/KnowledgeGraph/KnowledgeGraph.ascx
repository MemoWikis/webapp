<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeGraphModel>" %>

<script type="text/javascript">

    var graphJsonString = "<%= @Model.GraphDataString %>";
    var graphData = JSON.parse(graphJsonString);

    KnowledgeGraph.loadForceGraph();

    //KnowledgeGraph.loadTreeGraph("<%= @Model.GraphDataString %>");

</script>
