<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TestSetWidgetModel>" %>

<div class="testSetWidget">
    <h2><%: Model.Title %></h2>
    <h5><%: Model.Text %></h5>
    <script src="https://memucho.de/views/widgets/w.js" data-t="templateset" data-id="<%= Model.Set.Id %>" data-width="100%" data-maxwidth="100%" data-logoon="false" data-questioncount="5"></script>
</div>
