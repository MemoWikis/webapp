<%@ Page Title="Widget-Statistik" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WidgetStatsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Account/WidgetStats/WidgetStats.css") %>
    <%= Scripts.Render("~/Views/Users/Account/WidgetStats/Js/WidgetStats.js") %>


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Widget-Statistik</h1>
    <p>
        Hier siehst du die Zugriffe auf unsere Widgets von allen Hosts, die mit deinem Nutzer-Account verbunden sind.
    </p>
    <% foreach (var host in Model.Hosts) { %>
        <div class="col-xs-12" style="margin-top: 20px;">
            <h2>Host: <%= host %></h2>
            <% Html.RenderPartial("~/Views/Users/Account/WidgetStats/Partials/WidgetStatsForHost.ascx", new WidgetStatsForHostModel(host)); %>
        </div>
        <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
    <% } %>

</asp:Content>