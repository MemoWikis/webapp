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
                Berücksichtigt werden nur Daten seit GoLive (11.10.2016) und ohne Admins.
            </p>
            
            <table style="border: 1px solid darkblue; text-align: center; font-size: small;">
                <% var idx = 0;
                    foreach (var setStat in Model.SetStats)
                    {
                        if (idx == 0)
                        { %>
                        <tr style="color: green;">
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite">SetViews<br/>Total</span></th>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 7 Tagen">SetViews<br/>Last 7d</span></td>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 30 Tagen">SetViews<br/>Last 30d</span></td>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den vorvergangenen 30 Tagen">SetViews<br/>Pre 30d</span></td>
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets">QstnView<br/>Total</span></th>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 7 Tagen">QstnView<br/>Last 7d</span></td>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 30 Tagen">QstnView<br/>Last 30d</span></td>
                            <td style="padding-top: 15px;"><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den vorvergangenen 30 Tagen">QstnView<br/>Preced. 30d</span></td>
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets: Durchschnitt pro Tag seit Fragesatzerstellung">*QstnView<br/>DailyAvg</span></th>
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Anzahl an Antworten zu Fragen in diesem Set">Answers<br/>Total</span></th>
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Anzahl an Learning-Sessions mit diesem Set">Lrnng</span></th>
                            <th style="padding-top: 15px;"><span class="show-tooltip" data-original-title="Anzahl an Terminen mit diesem Set">Dts</span></th>
                        </tr>

                    <% } 
                    if (idx == 15)
                        idx = 0;
                    else
                        idx += 1;
                    %>
                    <tr>
                        <td colspan="12" style="text-align: left; padding-top: 10px; color: black;"><%= setStat.SetId %>: <b><%= setStat.SetName %></b> (seit <%= new TimeSpanLabel(DateTime.Now - setStat.Created, true).Full %>)</td>
                    </tr>
                        <tr>
                            <th style="text-align: right;"><%= setStat.SetDetailViewsTotal.ToString("N0") %> </th>
                            <td style="text-align: right;"><%= setStat.SetDetailViewsLast7Days.ToString("N0") %> </td>
                            <td style="text-align: right;"><%= setStat.SetDetailViewsLast30Days.ToString("N0") %> </td>
                            <td style="text-align: right;"><%= setStat.SetDetailViewsPrec30Days.ToString("N0") %> </td>
                            <th style="text-align: right;"><%= setStat.QuestionsViewsTotal.ToString("N0") %> </th>
                            <td style="text-align: right;"><%= setStat.QuestionsViewsLast7Days.ToString("N0") %> </td>
                            <td style="text-align: right;"><%= setStat.QuestionsViewsLast30Days.ToString("N0") %> </td>
                            <td style="text-align: right;"><%= setStat.QuestionsViewsPrec30Days.ToString("N0") %> </td>
                            <th style="text-align: right;"><%= setStat.QuestionViewsDailyAvg.ToString("N2") %> </th>
                            <th style="text-align: right;"><%= setStat.QuestionsAnswersTotal.ToString("N0") %> </th>
                            <th style="text-align: right;"><%= setStat.LearningSessionsTotal.ToString("N0") %> </th>
                            <th style="text-align: right;"><%= setStat.DatesTotal.ToString("N0") %> </th>
                        </tr>
                       
                <%} %>
            </table>
        </div>
    </div>

</asp:Content>