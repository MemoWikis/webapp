<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">
    <ul >
        <li><%= Html.ActionLink("Übersicht", Links.Summary, Links.SummaryController)%></li>
        <li>Neues</li>
        <ul>
            <li>Wissen</li>
            <li>Netzwerk</li>
        </ul> 
        <li>Wissen</li>
        <ul>
            <li>Fragen</li>
            <li>Fragensätze</li>
            <li>Kurse</li>
        </ul>               
    </ul>
</div>

