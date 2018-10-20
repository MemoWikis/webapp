<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DateRowModel>>" %>

<div style="padding-left: 7px; margin-top: 20px; margin-bottom: 15px;" id="captionPreviousDate">
    <h4><span class="ColoredUnderline Date">Termine aus der Vergangenheit</span>
        &nbsp;<a href="#" id="btnHidePreviousDates" id="divHidePreviousDates" style="font-size: 11px; color: darkgray"><i class="fa fa-caret-up"></i> verbergen</a>
    </h4>
</div>

<% foreach(var dateRowModel in Model){ %>
    <% Html.RenderPartial("~/Views/Knowledge/Dates/DateRow.ascx", dateRowModel); %>
<% } %>


<script type="text/javascript">
    drawKnowledgeCharts();
</script>