<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.PureContent.Master" 
    Inherits="ViewPage<WidgetQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/js/Widget") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(Model.AnswerQuestionModel)); %>
</asp:Content>