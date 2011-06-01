<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="text-align:right; padding:10px;">
    <% if(Request.IsLocal ){ %>
    <%= Html.ActionLink("Demodaten erzeugen", Links.CreateDemoData, Links.WelcomeController)%> |
    <% } %>
    <%= Html.ActionLink("Gemeinwohlunternehmen", Links.WelfareCompany, Links.VariousController)%> | 
    <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
</div>