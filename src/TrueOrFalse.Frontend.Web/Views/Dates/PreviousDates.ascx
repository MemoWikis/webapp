<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DateRowModel>>" %>

<div style="padding-left: 7px; margin-top: 20px; margin-bottom: 15px;" id="captionPreviousDate" class="date-row">
    <h4><span class="ColoredUnderline Date">Termine aus der Vergangenheit</span></h4>
</div>

<% foreach(var dateRowModel in Model){ %>
    <% Html.RenderPartial("DateRow", dateRowModel); %>
<% } %>


<script type="text/javascript">
    drawKnowledgeCharts();
</script>