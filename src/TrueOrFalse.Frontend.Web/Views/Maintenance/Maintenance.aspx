<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<MaintenanceModel>"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
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
    <a class="btn btn-default" href="<%= Url.Action("DeleteValuationsForRemovedSets", "Maintenance") %>">
        cleanup set valuations
    </a>
    
    <h4>Kategorien</h4>
    <a class="btn btn-default" href="<%= Url.Action("UpdateFieldQuestionCountForCategories", "Maintenance") %>">
        <i class="fa fa-retweet"></i>
        Feld: AnzahlFragen pro Kategorie aktualisieren
    </a>
        
    <h4>Nutzer</h4>
    <a class="btn btn-default" href="<%= Url.Action("UpdateUserReputationAndRankings", "Maintenance") %>">
        <i class="fa fa-retweet"></i>
        Rankings und Reputation + Aggregates
    </a>&nbsp;
    <a class="btn btn-default" href="<%= Url.Action("UpdateUserWishCount", "Maintenance") %>">
        <i class="fa fa-retweet"></i>
        Aggregates
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
        
    <h4 style="margin-top: 20px;">Nachricht senden</h4>
    <div class="form-horizontal">
        <% using (Html.BeginForm("SendMessage", "Maintenance"))
            {%>
        
            <div class="form-group">
                <%= Html.LabelFor(m => m.TestMsgReceiverId, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-2">
                    <%= Html.TextBoxFor(m => m.TestMsgReceiverId, new {@class="form-control"} ) %>    
                </div>
            </div>
            <div class="form-group">
                <%= Html.LabelFor(m => m.TestMsgSubject, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-6">
                    <%= Html.TextBoxFor(m => m.TestMsgSubject, new {@class="form-control"} ) %>    
                </div>
            </div>
            <div class="form-group">
                <%= Html.LabelFor(m => m.TestMsgBody, new {@class="col-sm-2 control-label"} ) %>
                <div class="col-xs-6">
                    <%= Html.TextAreaFor(m => m.TestMsgBody, new {@class="form-control", rows = 4} ) %>
                </div>
            </div>

            <div class="form-group" style="">
                <div class="col-sm-offset-2 col-sm-9">
                    <input type="submit" value="Senden" class="btn btn-primary" name="btnSave" />
                </div>
            </div>

        <% } %>
    </div>

    <br/><br/>

</asp:Content>