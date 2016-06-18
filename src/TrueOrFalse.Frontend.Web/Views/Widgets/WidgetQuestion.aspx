<%@ Page Title="Spielen" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.PureContent.Master" 
    Inherits="ViewPage<WidgetQuestionModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
            new AnswerBodyModel(Model.AnswerQuestionModel)); %>
</asp:Content>