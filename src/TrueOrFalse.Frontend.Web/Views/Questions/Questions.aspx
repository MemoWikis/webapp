<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h2 style="float: left;">Fragen</h2>
        <div style="float: right;">
            <%= Buttons.Link("Frage erstellen", Links.CreateQuestion, Links.CreateQuestionController, ButtonIcon.Add)%>
        </div>

    </div>

    <div>
        <% foreach (var row in Model.QuestionRows)
           {
               Html.RenderPartial("QuestionRow", row);
           } %>
    </div> 

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightMenu" runat="server">
</asp:Content>
