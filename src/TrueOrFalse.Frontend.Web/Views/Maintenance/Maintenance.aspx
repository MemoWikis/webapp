<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Maintenance</h2>
    
    <br/><br/>
    
    <% Html.Message(Model.Message); %>
    
    <a class="btn btn-primary" href="<%= Url.Action("RecalculateAllKnowledgeItems", "Maintenance") %>">Alle Antwortwahrscheinlichkeiten neu berechnen</a><br/><br/>
    <a class="btn btn-primary" href="<%= Url.Action("CalcAggregatedValues", "Maintenance") %>">Aggregierte Zahlen für Fragen aktualisieren</a>


</asp:Content>

