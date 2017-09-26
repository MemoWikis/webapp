<%@ Page Title="Widget-Statistik" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<WidgetViewsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Account/WidgetViews/WidgetViews.css") %>
    <%= Scripts.Render("~/Views/Users/Account/WidgetViews/Js/WidgetViews.js") %>


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
    <div class="col-xs-12">
        <div id="MainWrapper">
            <h1>Widget-Statistik</h1>
            <p>
                Hier siehst du die Zugriffe auf unsere Widgets von allen Hosts, die mit deinem Nutzer-Account verbunden sind.
            </p>
            <% foreach (var host in Model.Hosts) { %>
                <div class="col-xs-12" style="margin-top: 20px;">
                    <h2>Host: <%= host %></h2>
                    <% Html.RenderPartial("~/Views/Users/Account/WidgetViews/Partials/WidgetViewsForHost.ascx", new WidgetViewsForHostModel(host)); %>
                </div>
                <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>
            <% } %>



        </div>
    </div>
</div>

</asp:Content>