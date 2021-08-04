<%@ Page Title="Ergebnis Lernsitzung" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<LearningSessionResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <%= Scripts.Render("~/bundles/js/LearningSessionResult") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionResultInner.ascx", Model); %>

</asp:Content>