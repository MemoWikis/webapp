<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-10">
        <h3>Maintenance</h3>
        
        <% Html.Message(Model.Message); %>
    
        <h4>Fragen</h4>
        <a class="btn btn-default" href="<%= Url.Action("RecalculateAllKnowledgeItems", "Maintenance") %>">
            <i class="icon-retweet"></i>
            Alle Antwortwahrscheinlichkeiten neu berechnen
        </a><br/><br/>
        <a class="btn btn-default" href="<%= Url.Action("CalcAggregatedValues", "Maintenance") %>">
            <i class="icon-retweet"></i>
            Aggregierte Zahlen für Fragen aktualisieren
        </a>
    
        <h4>Kategorien</h4>
        <a class="btn btn-default" href="<%= Url.Action("UpdateFieldQuestionCountForCategories", "Maintenance") %>">
            <i class="icon-retweet"></i>
            Feld: AnzahlFragen pro Kategorie aktualisieren
        </a>
    
        <h4>Suche</h4>
        Alle
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllQuestions", "Maintenance") %>">
            <i class="icon-retweet"></i>Fragen 
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllSets", "Maintenance") %>">
            <i class="icon-retweet"></i>Fragesätze
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllCategories", "Maintenance") %>">
            <i class="icon-retweet"></i>Kategorien
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllUsers", "Maintenance") %>">
            <i class="icon-retweet"></i>Nutzer
        </a>        
        für Suche neu indizieren
        <br/><br/>
    </div>

</asp:Content>

