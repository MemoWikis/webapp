<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">

    <div class="main">Neues</div>
    
    <div class="main"><%= Html.ActionLink("Wissen", Links.Knowledge, Links.KnowledgeController)%></div>
    <div>Fragen</div>
    <div>Fragensätze</div>
    <div>Kurse</div>
    <div>-------------</div>
    <div>Kategorien</div>

    <div class="main">Netzwerk <img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></div>

</div>