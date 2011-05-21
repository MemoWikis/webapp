<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="text-align:right; padding:10px;">
    <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
</div>