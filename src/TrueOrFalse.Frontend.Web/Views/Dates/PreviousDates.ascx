<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DateRowModel>>" %>

<div style="padding-left: 7px;" id="captionPreviousDate" class="rowBase date-row">
    <h4><span class="ColoredUnderline Date">Termine aus der Vergangenheit</span></h4>
</div>

<% foreach(var dateRowModel in Model){ %>
    <% Html.RenderPartial("DateRow", dateRowModel); %>
<% } %>


<script type="text/javascript">
    drawKnowledgeCharts();
</script>