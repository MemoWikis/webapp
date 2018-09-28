<%@ Page Title="Wissenszentrale" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<KnowledgeModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.Knowledge() %>">
</asp:Content>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <%= Styles.Render("~/bundles/Knowledge") %>
    <%= Scripts.Render("~/bundles/js/Knowledge") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body">
        <input type="hidden" id="hddUrlAddTopic" value="<%= Url.Action("Create", "EditCategory") %>" />

        <script type="text/javascript">

            google.load("visualization", "1", { packages: ["corechart"] });

            var isGoogleApiInitialized = false;
            google.setOnLoadCallback(isApiInitialized);

            function isApiInitialized() {
                isGoogleApiInitialized = true;
            }
        </script>

        <div class="container-fluid">
            <div class="row h1Head">
                <div class="col-xs-10">
                    <h1>Wissenszentrale - Überblick und Zahlen </h1>
                </div>
            </div>
            <div class="row  link-set">
                <div class="col-xs-2"><a href="#" id="dashboard">Dashboard</a></div>
                <div class="col-xs-2"><a href="#" id="topics">Themen</a></div>
                <div class="col-xs-2"><a href="#" id="questions">Fragen</a></div>
                <div class="col-xs-3 col-sm-offset-3 "><a href="#" id="LinkIsDirectedToPartialView">Lernsitzung starten</a></div>
            </div>
            <div class="row">
                <% if (Model.IsLoggedIn)
                    { %>
                <div class="content" style="margin-top: 2rem">
                    <% Html.RenderPartial("~/Views/Knowledge/Partials/_Dashboard.ascx"); %>
                </div>
                <% } %>
            </div>
        </div>
    </div>


</asp:Content>
