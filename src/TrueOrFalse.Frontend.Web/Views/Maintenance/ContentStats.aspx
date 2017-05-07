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
            
            <p>
                Ziel: 1) Übersicht über Kerndaten aller Fragesätze (vgl. SetViewStatsResult.cs), 
                die wegen aufwendiger Berechnung in DB abgelegt werden und 1x/Tag neu berechnet werden. <br/>
                2) Dazu per Formular Eingabe von Set Id(s), für die aktuell alle Daten aus DBs geholt werden 
                und wo tagesgenau Zugriffe dargestellt werden (set-vergleichend).
            </p>
            <p>
                Fragen-Ids für Set 14: 339,340,341,342,343,344,345,346,347,348 <br />
                select Id, QuestionId, UserId, DateCreated  from questionview where QuestionId IN (339,340,341,342,343,344,345,346,347,348); <br/>
                select count(*), Date(DateCreated) from questionview where QuestionId IN (339,340,341,342,343,344,345,346,347,348) GROUP BY DATE(DateCreated);
            </p>
            
            <table style="border: 1px solid darkblue; text-align: center;">
                <tr>
                    <th>Id</th>
                    <td>Created</td>
                    <td>WklyAvg</td>
                    <th>VwsTtl</th>
                    <td>Vws7</td>
                    <td>Vws30</td>
                    <td>Vws-30</td>
                    <th>QVwsTtl</th>
                    <td>QVws7</td>
                    <td>QVws30</td>
                    <td>QVws-30</td>
                    <th>QAnswsTtl</th>
                    <th>Lrnng</th>
                    <th>Dts</th>
                </tr>
                <tr>
                    <th class="show-tooltip" data-original-title="<%= Model.SetStats.SetName %>"><%= Model.SetStats.SetId %> </th>
                    <td><%= Model.SetStats.Created.ToString(CultureInfo.InvariantCulture) %> </td>
                    <td><%= Model.SetStats.SetDetailViewsWeeklyAvg %> </td>
                    <th><%= Model.SetStats.SetDetailViewsTotal %> </th>
                    <td><%= Model.SetStats.SetDetailViewsLast7Days %> </td>
                    <td><%= Model.SetStats.SetDetailViewsLast30Days %> </td>
                    <td><%= Model.SetStats.SetDetailViewsPrec30Days %> </td>
                    <th><%= Model.SetStats.QuestionsViewsTotal %> </th>
                    <td><%= Model.SetStats.QuestionsViewsLast7Days %> </td>
                    <td><%= Model.SetStats.QuestionsViewsLast30Days %> </td>
                    <td><%= Model.SetStats.QuestionsViewsPrec30Days %> </td>
                    <th><%= Model.SetStats.QuestionsAnswersTotal %> </th>
                    <th><%= Model.SetStats.LearningSessionsTotal %> </th>
                    <th><%= Model.SetStats.DatesTotal %> </th>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>