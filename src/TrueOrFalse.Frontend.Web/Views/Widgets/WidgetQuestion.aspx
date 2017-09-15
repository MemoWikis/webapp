<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Widget.Master" 
    Inherits="ViewPage<WidgetQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.QuestionText; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/WidgetQuestion") %>

    <% if(Model.IncludeCustomCss){ %>
        <link href="<%= Model.CustomCss %>" rel="stylesheet" />
    <% } %>

    <style type="text/css">
        html { height: auto;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(Model.AnswerQuestionModel)); %>
</asp:Content>