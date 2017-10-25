<%@ Page Title="CMS" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CMSModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <%= Scripts.Render("~/bundles/js/MaintenanceCMS") %>
    <meta id="blablabla"/>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <nav class="navbar navbar-default" style="" role="navigation">
        <div class="container">
            <a class="navbar-brand" href="#">Maintenance</a>
            <ul class="nav navbar-nav">
                <li><a href="/Maintenance">Allgemein</a></li>
                <li><a href="/MaintenanceImages/Images">Bilder</a></li>
                <li><a href="/Maintenance/Messages">Nachrichten</a></li>
                <li><a href="/Maintenance/Tools">Tools</a></li>
                <li class="active"><a href="/Maintenance/CMS">CMS</a></li>
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
                <li><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <div class="row">
        <div class="col-md-10">
            <h2 class="">CMS</h2>
        </div>
    </div>
    <div>
        <% using (Html.BeginForm("CMS", "Maintenance")){%>
        
            <%= Html.AntiForgeryToken() %>
            <div class="form-group">
                <label class="control-label"><span style="font-weight: bold">Vorgeschlagene Lernsets</span> (Set-Ids kommasepariert)</label>
                <i class="fa fa-info-circle show-tooltip" title="Diese Lernsets werden bei den Inhalteempfehlungen zusätzlich zu allen Lernsets von memucho berücksichtigt.">&nbsp;</i>
                <%= Html.TextBoxFor(m => m.SuggestedSetsIdString, new {@class="form-control"} ) %>
                <% foreach(var set in Model.SuggestedSets) { %>
                    <a href="<%= Links.SetDetail(Url, set) %>"><span class="label label-set"><%: set.Id %>-<%: set.Name %></span></a>
                <% } %>
            </div>
        
            <div class="form-group">
                <label class="control-label"><span style="font-weight: bold">Vorgeschlagene Spiele</span> (Set-Ids kommasepariert)</label>
                <%= Html.TextBoxFor(m => m.SuggestedGames, new {@class="form-control"} ) %>
                <% foreach(var set in Model.SuggestedGameSets) { %>
                    <a href="<%= Links.SetDetail(Url, set) %>"><span class="label label-set"><%: set.Id %>-<%: set.Name %></span></a>
                <% } %>
            </div>
        
            <input type="submit" value="Speichern" class="btn btn-primary" name="btnSave" />

        <% } %>
    </div>
    
    <hr/>
    <div>
        <h3 style="margin-top: 50px;">Tools zur Content-Pflege</h3>
        <div id="showLooseCategories">
            <h4 style="margin-top: 40px;">Lose Themen</h4>
            <p>
                Themen anzeigen, die nicht in eines der vier Oberthemen eingehangen sind: <a href="#" id="btnShowLooseCategories" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showLooseCategoriesResult" style="margin-left: 25px;"></div>
        </div>

        <div id="showCategoriesWithNonAggregatedChildren">
            <h4 style="margin-top: 40px;">Themen mit unbearbeitetem Aggregierungsstatus</h4>
            <p>
                Themen anzeigen, die Unterthemen haben, über deren Aggregierungs-Status noch nicht entschieden ist: <a href="#" id="btnShowCategoriesWithNonAggregatedChildren" class="btn btn-default">Themen anzeigen</a>
            </p>
            <div id="showCategoriesWithNonAggregatedChildrenResult" style="padding: 10px;"></div>
        </div>
    </div>

</asp:Content>