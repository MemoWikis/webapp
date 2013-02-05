<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="text-align:right; padding:10px 0px 10px 0px; margin-left: -20px; margin-right: 20px;">
    <%= Html.ActionLink("Gemeinwohlunternehmen", Links.WelfareCompany, Links.VariousController)%> | 
    <%= Html.ActionLink("Impressum", Links.Impressum, Links.VariousController)%>
</div>