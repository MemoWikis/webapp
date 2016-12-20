<%@ Page Title="Maintenance: ContentReport" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<ContentReportModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <link href="/Style/site.css" rel="stylesheet" />
    <link href="/Views/Maintenance/ContentReport.css" rel="stylesheet" />
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
                <li><a href="/Maintenance/CMS">CMS</a></li>
                <li class="active"><a href="/Maintenance/ContentReport">Content</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
        
    <div class="row">
        <div class="col-xs-12">
            <h1 class="" style="margin-top: 0;">Erstellte Inhalte</h1>
            <ul>
                <li><a href="#CategoriesAdded">Kategorien</a></li>
                <li><a href="#SetsAdded">Fragesätze</a></li>
                <li><a href="#RecentQuestionsAddedNotMemucho">Fragen erstellt ohne memucho</a></li>
                <li><a href="#RecentQuestionsAddedMemucho">Fragen erstellt memucho</a></li>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h4 id="CategoriesAdded">Kategorien, die seit <%= Model.Since %> erstellt wurden: <%= Model.CategoriesAdded.Count %></h4>
            <span style="font-size: 10px; color: silver"><a href="#Top">(nach oben)</a></span>

            <% foreach (var category in Model.CategoriesAdded) {%>
                <div>
                    <span style="font-size: 10px; color: silver"><%= category.DateCreated %></span> 
                    <a href="<%= Links.UserDetail(category.Creator) %>" class="linkUser"><%= category.Creator.Name %></a>: 
                    <a href="<%= Links.CategoryDetail(category) %>"><%: category.Name %></a> 
                </div>
            <%} %>
        </div>

        <div class="col-xs-12">
            <h4 id="SetsAdded">Fragesätze, die seit <%= Model.Since %> erstellt wurden: <%= Model.SetsAdded.Count %></h4>
            <span style="font-size: 10px; color: silver"><a href="#Top">(nach oben)</a></span>

            <% foreach (var set in Model.SetsAdded) {%>
                <div>
                    <span style="font-size: 10px; color: silver"><%= set.DateCreated %></span> 
                    <a href="<%= Links.UserDetail(set.Creator) %>" class="linkUser"><%= set.Creator.Name %></a>: 
                    <a href="<%= Links.SetDetail(set) %>"><%: set.Name %></a> 
                </div>
            <%} %>
        </div>

        <div class="col-xs-12">
            <h4 id="RecentQuestionsAddedNotMemucho">Alle nicht von memucho seit <%= Model.Since %> erstellten Fragen: <%= Model.RecentQuestionsAddedNotMemucho.Count %></h4>
            <span style="font-size: 10px; color: silver"><a href="#Top">(nach oben)</a></span>

            <% foreach (var question in Model.RecentQuestionsAddedNotMemucho) {%>
                <div class="LabelItem LabelItem-Question">
                    <span style="font-size: 10px; color: silver"><%= question.DateCreated %></span> 
                    <a href="<%= Links.UserDetail(question.Creator) %>" class="linkUser"><%= question.Creator.Name %></a>: 
                    <a href="<%= Links.AnswerQuestion(question) %>"><%: question.Text %></a> 
                    <% if (question.SetTop5Minis.Any()) { %>
                        (
                        <% foreach (var set in question.SetTop5Minis) { %>
                            <a href="<%= Links.SetDetail(set.Name, set.Id) %>" class="linkSet"><%= set.Name %></a> |
                        <% } %>
                        )
                    <% } %>
                </div>
            <%} %>
        </div>

        <div class="col-xs-12">
            <h4 id="RecentQuestionsAddedMemucho">Alle von memucho seit <%= Model.Since %> erstellten Fragen: <%= Model.RecentQuestionsAddedMemucho.Count %></h4>
            <span style="font-size: 10px; color: silver"><a href="#Top">(nach oben)</a></span>

            <% foreach (var question in Model.RecentQuestionsAddedMemucho) {%>
                <div class="LabelItem LabelItem-Question">
                    <span style="font-size: 10px; color: silver"><%= question.DateCreated %></span> <a href="<%= Links.AnswerQuestion(question) %>"><%: question.Text %></a> 
                    <% if (question.SetTop5Minis.Any()) { %>
                        (
                        <% foreach (var set in question.SetTop5Minis) { %>
                            <a href="<%= Links.SetDetail(set.Name, set.Id) %>" class="linkSet"><%= set.Name %></a> |
                        <% } %>
                        )
                    <% } %>
                </div>
            <%} %>
        </div>
    </div>

</asp:Content>