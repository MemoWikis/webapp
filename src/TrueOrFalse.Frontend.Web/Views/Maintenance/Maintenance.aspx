<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Maintenance</h2>
    
    <br/><br/>
    
    <% Html.Message(Model.Message); %>
    
    <a class="btn" href="<%= Url.Action("RecalculateAllKnowledgeItems", "Maintenance") %>">
        <i class="icon-retweet"></i>
        Alle Antwortwahrscheinlichkeiten neu berechnen
    </a><br/><br/>
    <a class="btn" href="<%= Url.Action("CalcAggregatedValues", "Maintenance") %>">
        <i class="icon-retweet"></i>
        Aggregierte Zahlen für Fragen aktualisieren
    </a><br/><br/>
    <a class="btn" href="<%= Url.Action("ReIndexAllQuestions", "Maintenance") %>">
        <i class="icon-retweet"></i>
        Alle Fragen für Suche neu indizieren
    </a><br/><br/>

</asp:Content>

