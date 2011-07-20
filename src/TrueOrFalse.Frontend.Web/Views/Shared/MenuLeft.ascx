<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">

    <div>Neues</div>
    
    <div class="main"><%= Html.ActionLink("Wissen", Links.Summary, Links.SummaryController)%></div>
    <div>Fragen</div>
    <div>Fragensätze</div>
    <div>Kurse</div>
    <div>-------------</div>
    <div>Kategorien</div>

    <div>Netzwerk</div>
    <div>Kontakt</div>

</div>

