<%@ Page Title="Algorithmus-Einblick" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<AlgoInsightModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    <%= Styles.Render("~/bundles/AlgoInsight") %>
       <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = "Algorithmus-Einblick", Url = "/AlgoInsight/Forecast"});
          Model.TopNavMenu.IsCategoryBreadCrumb = false;%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if(Model.Message != null) { %>
        <div class="row">
            <div class="col-xs-12 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
        </div>        
    <% } %>

    <h2 style="color: black; margin-bottom: 25px; margin-top: 0px;">
        <span class="ColoredUnderline Knowledge">Algorithmus-Einblick</span>
    </h2>
    
    <p>
        Hier erhältst du Einblick in die Algorithmen, die dir beim Lernen mit memucho helfen. 
    </p>
    <p>
        Wir glauben, dass es nicht <em>den</em> einen perfekten Algorithmus beim Lernen gibt.
        Deshalb treten bei memucho immer verschiedene <b>Algorithmen in einem Wettbewerb</b> gegeneinander an. Gewinner ist, wer die beste Vorhersage darüber trifft, 
        wann du etwas vergessen wirst - damit wir dich kurz vorher an's Wiederholen erinnern können! (Wenn du es genau wissen willst: Wir arbeiten mit Vergessensschwellen. 
        Das heißt, wenn die Wahrscheinlichkeit einer korrekten Antwort z.B. unter 90% sinkt, dann sollte diese Frage wiederholt werden.) <br/>
        Wir gehen sogar noch einen Schritt weiter: <b>Je nach Lernsituation</b> können unterschiedliche Algorithmen gewinnen. 
        Lernst du Geschichte für eine Prüfung in zwei Wochen ist möglicherweise ein anderer Algorithmus besser 
        als für das Allgemeinwissen in Literatur, welches du immer parat haben möchtest. Das berücksichtigen wir!
    </p>
    <p>
        Wir haben gerade erst angefangen und sind noch in der <a href="<%= Links.BetaInfo() %>">Beta-Phase</a>, es wird noch viel passieren. memucho ist 
        Open Source, du kannst dir den <a href="https://github.com/TrueOrFalse/TrueOrFalse">Quelltext auf <i class="fa fa-github"></i> Github ansehen</a>. Wir freuen uns über Verbesserungsvorschläge!
    </p>        
            
    <div class="row" style="margin-top: 20px;">
        
        <div id="MobileSubHeader" class="MobileSubHeader DesktopHide" style="margin-top: 0px;">
            <div class="MainFilterBarWrapper">
                <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <a class="btn btn-default disabled">.</a>
                    </div>
                </div>
                <div class="container">
                    <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">
                        <div class="btn-group <%= Model.IsActiveTabForecast ? "active" : "" %>">
                            <a href="<%= Url.Action("Forecast", "AlgoInsight") %>" type="button" class="btn btn-default">
                                Vorhersage
                            </a>
                        </div>                        
                        <div class="btn-group  <%= Model.IsActiveTabForgettingCurve ? "active" : "" %>">
                            <a  href="<%= Url.Action("ForgettingCurve", "AlgoInsight") %>" type="button" class="btn btn-default">
                                Vergessenskurve
                            </a>
                        </div>
                        <div class="btn-group  <%= Model.IsActiveTabRepetition ? "active" : "" %>">
                            <a  href="<%= Url.Action("Repetition", "AlgoInsight") %>" type="button" class="btn btn-default">
                                Wiedervorlage
                            </a>
                        </div>
                        <div class="btn-group  <%= Model.IsActiveTabVarious ? "active" : "" %>">
                            <a  href="<%= Url.Action("Various", "AlgoInsight") %>" type="button" class="btn btn-default">
                                Verschiedenes
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="boxtainer-outlined-tabs" style="margin-top: 0px;">
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs">
                        <li class="<%= Html.IfTrue(Model.IsActiveTabForecast, "active") %>">
                            <a href="<%= Url.Action("Forecast", "AlgoInsight") %>" >
                                Vorhersage
                            </a>
                        </li>
                        <li class="<%= Html.IfTrue(Model.IsActiveTabForgettingCurve, "active") %>">
                            <a href="<%= Url.Action("ForgettingCurve", "AlgoInsight") %>">
                                Vergessenskurve
                            </a>
                        </li>
                        <li class="<%= Html.IfTrue(Model.IsActiveTabRepetition, "active") %>">
                            <a href="<%= Url.Action("Repetition", "AlgoInsight") %>">
                                Wiedervorlage
                            </a>
                        </li>
                        <li class="<%= Html.IfTrue(Model.IsActiveTabVarious, "active") %>">
                            <a href="<%= Url.Action("Various", "AlgoInsight") %>">
                                Verschiedenes
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="boxtainer-content">
                    <% if(Model.IsActiveTabForecast) { %>
                        <% Html.RenderPartial("TabForecast", new TabForecastModel()); %>
                    <% } %>
                    <% if(Model.IsActiveTabForgettingCurve) { %>
                        <% Html.RenderPartial("TabForgettingCurve", new TabForgettingCurveModel()); %>
                    <% } %>
                    <% if(Model.IsActiveTabRepetition) { %>
                        <% Html.RenderPartial("TabRepetition", new TabRepetitionModel()); %>
                    <% } %>
                    <% if(Model.IsActiveTabVarious) { %>
                        <% Html.RenderPartial("TabVarious", new TabVariousModel()); %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
