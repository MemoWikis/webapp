<%@ Control Language="C#" AutoEventWireup="true"  Inherits="System.Web.Mvc.ViewUserControl<TestSetWidgetModel>" %>

<script src="https://memucho.de/views/widgets/w.js" data-t="templateset" data-id="<%= Model.SetId %>" data-width="100%" data-maxWidth="100%" data-setTitle = "<%: Model.Title %>" data-setText = "<%: Model.Text %>" data-questionCount="5"></script>