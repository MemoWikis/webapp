<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<CMSModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

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
                <li><a href="/Maintenance/ContentCreatedReport">Content</a></li>
                <li><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <div class="row">
        <div class="col-md-10">
            <h4 class="">CMS</h4>
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

</asp:Content>