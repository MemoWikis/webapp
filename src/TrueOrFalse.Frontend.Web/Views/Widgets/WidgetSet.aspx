﻿<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.PureContent.Master" 
    Inherits="ViewPage<WidgetSetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/WidgetSet") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    <input type="hidden" id="hddIsTestSession" value="<%= Model.IsTestSession %>" 
        data-test-session-id="<%= Model.IsTestSession ? Model.TestSessionId : -1 %>"
        data-current-step-idx="<%= Model.IsTestSession ? Model.TestSessionCurrentStep : -1 %>"
        data-is-last-step="<%= Model.TestSessionIsLastStep %>"/>
    
    <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model.AnswerQuestionModel); %>

    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(Model.AnswerQuestionModel)); %>
</asp:Content>