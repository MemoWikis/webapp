<%@ Page Title="Maintenance: ContentStats" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<ContentStatsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Head">
    <%= Styles.Render("~/bundles/MaintenanceContentStats") %>
    <%= Scripts.Render("~/bundles/js/MaintenanceContentStats") %>
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
                <li><a href="/Maintenance/ContentCreatedReport">Cnt-Created</a></li>
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
                Die Statistik zeigt relevanten Views und Nutzungsdaten für jedes Lernset.
            </p>
            
            <% if (!Model.SetStats.Any()) { %>
                <div class="alert alert-warning">
                    <p>
                        <b>Es wurden noch keine Statistiken geladen.</b> Das Laden der Daten zu den Zugriffen auf Lernsets und die dazugehörigen 
                        Fragen nimmt einige Zeit in Anspruch (mind. 30 Sekunden), 
                        da eine Vielzahl von DB-Anfragen ausgeführt werden müssen.
                    </p>
                    <p style="margin: 20px; text-align: center;">
                        <a href="<%= Url.Action("ContentStatsShowStats", "Maintenance") %>" data-url="toSecurePost" class="btn btn-primary">
                            <i class="fa fa-clock-o"></i>
                            Statistik-Daten laden
                        </a><br/>
                    </p>
                </div>
            
            <% } else { %>
                <p>
                    Berücksichtigt werden nur Daten seit GoLive (11.10.2016) und ohne Admins.
                </p>
            
                <table id="tableSetStats" class="table table-striped table-bordered table-hover table-condensed" cellspacing="0" width="100%" style="/*border: 1px solid darkblue; text-align: center;*/ font-size: small;">
                    <thead>
                    <tr style="color: green;">
                        <th><span class="show-tooltip" data-original-title="Name">ID: Name</span></th>
                        <th><span class="show-tooltip" data-original-title="Erstellt vor, 1 = 30 Tage">Age<br/>(/30d)</span></th>
                        <th><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite">SetVws<br/>Total</span></th>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 7 Tagen">SetViews<br/>Last 7d</span></td>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 30 Tagen">SetViews<br/>Last 30d</span></td>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den vorvergangenen 30 Tagen">SetViews<br/>Pre 30d</span></td>
                        <th><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets">Q-View<br/>Total</span></th>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 7 Tagen">Q-View<br/>Last 7d</span></td>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 30 Tagen">Q-View<br/>Last 30d</span></td>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den vorvergangenen 30 Tagen">Q-View<br/>Prec.30d</span></td>
                        <th><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets: Durchschnitt pro Tag seit Fragesatzerstellung">Q-View<br/>DailyAvg</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Antworten zu Fragen in diesem Set">Answers<br/>Total</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Learning-Sessions mit diesem Set">Lrnng</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Terminen mit diesem Set">Dts</span></th>
                    </tr>
                    </thead>
                    <tfoot>
                    <tr style="color: green;">
                        <th><span class="show-tooltip" data-original-title="Name">ID: Name</span></th>
                        <th><span class="show-tooltip" data-original-title="Erstellt vor, 1 = 30 Tage">Age<br/>(/30d)</span></th>
                        <th><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite">SetVws<br/>Total</span></th>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 7 Tagen">SetViews<br/>Last 7d</span></td>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den letzten 30 Tagen">SetViews<br/>Last 30d</span></td>
                        <td><span class="show-tooltip" data-original-title="Views der Set-Detail-Seite in den vorvergangenen 30 Tagen">SetViews<br/>Pre 30d</span></td>
                        <th><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets">Q-View<br/>Total</span></th>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 7 Tagen">Q-View<br/>Last 7d</span></td>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den letzten 30 Tagen">Q-View<br/>Last 30d</span></td>
                        <td><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets in den vorvergangenen 30 Tagen">Q-View<br/>Prec.30d</span></td>
                        <th><span class="show-tooltip" data-original-title="QuestionViews aller Fragen des Sets: Durchschnitt pro Tag seit Fragesatzerstellung">Q-View<br/>DailyAvg</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Antworten zu Fragen in diesem Set">Answers<br/>Total</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Learning-Sessions mit diesem Set">Lrnng</span></th>
                        <th><span class="show-tooltip" data-original-title="Anzahl an Terminen mit diesem Set">Dts</span></th>
                    </tr>
                    </tfoot>

                    <tbody>
                
                    <% foreach (var setStat in Model.SetStats) { %>
                        <tr>
                            <td style="text-align: left; max-width: 150px; line-break: auto; font-size: smaller"><%= setStat.SetId %>: <b><%= setStat.SetName %></b></td>
                            <td style="text-align: right;"><span class="show-tooltip" data-original-title="seit <%= new TimeSpanLabel(DateTime.Now - setStat.Created, true).Full %>"><%= ((double) (DateTime.Now - setStat.Created).Days / 30).ToString("N1") %></span></td>
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
                    <% } %>

                    </tbody>
                </table>
            <% } %>
        </div>
    </div>

</asp:Content>