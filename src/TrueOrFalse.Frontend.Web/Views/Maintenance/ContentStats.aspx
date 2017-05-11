<%@ Page Title="Maintenance: ContentStats" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<ContentStatsModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Maintenance/ContentStats.css" rel="stylesheet" />
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
                <li><a href="/Maintenance/ContentCreatedReport">Content</a></li>
                <li class="active"><a href="/Maintenance/ContentStats">Cnt Stats</a></li>
                <li><a href="/Maintenance/Statistics">Stats</a></li>
            </ul>
        </div>
    </nav>
    <% Html.Message(Model.Message); %>
        
    <div class="row">
        <div class="col-xs-12">
            <h4 class="">Content Usage Statistics</h4>
            
<%--            <p>
                Fragen-Ids für Set 14: 339,340,341,342,343,344,345,346,347,348 <br />
                select Id, QuestionId, UserId, DateCreated  from questionview where QuestionId IN (339,340,341,342,343,344,345,346,347,348); <br/>
                select count(*), Date(DateCreated) from questionview where QuestionId IN (339,340,341,342,343,344,345,346,347,348) GROUP BY DATE(DateCreated);
            </p>--%>
            <p>
                Berücksichtigt werden nur Daten seit GoLive (11.10.2016) und ohne Admins.
            </p>
            
            <table style="border: 1px solid darkblue; text-align: center;">
                <tr>
                    <th>Id</th>
                    <td>Created</td>
                    <th>SetViews<br/>Total</th>
                    <td>SetViews<br/>Last 7d</td>
                    <td>SetViews<br/>Last 30d</td>
                    <td>SetViews<br/>Pre 30d</td>
                    <th>QuestionView<br/>Total</th>
                    <td><span class="show-tooltip" data-original-title="# of questions of this set seen daily (average)">QuestionV<br/>DailyAvg</span></td>
                    <td>QuestionView<br/>Last 7d</td>
                    <td>QuestionView<br/>Last 30d</td>
                    <td>QuestionView<br/>Preced. 30d</td>
                    <th>Answers<br/>Total</th>
                    <th>Lrnng</th>
                    <th>Dts</th>
                </tr>
                <% foreach (var setStat in Model.SetStats)
                   { %>
                        <tr>
                            <th><span class="show-tooltip" data-original-title="<%= setStat.SetName %>"><%= setStat.SetId %></span></th>
                            <td><%= new TimeSpanLabel(DateTime.Now - setStat.Created).Full %> </td>
                            <th><%= setStat.SetDetailViewsTotal %> </th>
                            <td><%= setStat.SetDetailViewsLast7Days %> </td>
                            <td><%= setStat.SetDetailViewsLast30Days %> </td>
                            <td><%= setStat.SetDetailViewsPrec30Days %> </td>
                            <th><%= setStat.QuestionsViewsTotal.ToString("D") %> </th>
                            <td><%= setStat.QuestionViewsDailyAvg.ToString("n2") %> </td>
                            <td><%= setStat.QuestionsViewsLast7Days %> </td>
                            <td><%= setStat.QuestionsViewsLast30Days %> </td>
                            <td><%= setStat.QuestionsViewsPrec30Days %> </td>
                            <th><%= setStat.QuestionsAnswersTotal %> </th>
                            <th><%= setStat.LearningSessionsTotal %> </th>
                            <th><%= setStat.DatesTotal %> </th>
                        </tr>
                       
                   <%} %>
            </table>
        </div>
    </div>

</asp:Content>