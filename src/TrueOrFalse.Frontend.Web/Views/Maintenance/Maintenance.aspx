<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-9">
        <h3>Maintenance</h3>
        
        <% Html.Message(Model.Message); %>
    
        <h4>Fragen</h4>
        <a class="btn btn-default" href="<%= Url.Action("RecalculateAllKnowledgeItems", "Maintenance") %>">
            <i class="fa fa-retweet"></i>
            Alle Antwortwahrscheinlichkeiten neu berechnen
        </a><br/><br/>
        <a class="btn btn-default" href="<%= Url.Action("CalcAggregatedValuesQuestions", "Maintenance") %>">
            <i class="fa fa-retweet"></i>
            Aggregierte Zahlen aktualisieren
        </a>
        
        <h4>Fragesätze</h4>
        
        <a class="btn btn-default" href="<%= Url.Action("CalcAggregatedValuesSets", "Maintenance") %>">
            <i class="fa fa-retweet"></i>
            Aggregierte Zahlen aktualisieren
        </a>
    
        <h4>Kategorien</h4>
        <a class="btn btn-default" href="<%= Url.Action("UpdateFieldQuestionCountForCategories", "Maintenance") %>">
            <i class="fa fa-retweet"></i>
            Feld: AnzahlFragen pro Kategorie aktualisieren
        </a>
        
        
  
        <h4>Suche</h4>
        Alle
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllQuestions", "Maintenance") %>">
            <i class="fa fa-retweet"></i>Fragen 
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllSets", "Maintenance") %>">
            <i class="fa fa-retweet"></i>Fragesätze
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllCategories", "Maintenance") %>">
            <i class="fa fa-retweet"></i>Kategorien
        </a> /
        <a class="btn btn-default" href="<%= Url.Action("ReIndexAllUsers", "Maintenance") %>">
            <i class="fa fa-retweet"></i>Nutzer
        </a>        
        für Suche neu indizieren
        <br/><br/>
    </div>

</asp:Content>

