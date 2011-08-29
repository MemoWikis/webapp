<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<QuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

<style type="text/css">

div.question-row div.stats-2 div.answersTotal{ width: 65px;}
div.question-row div.stats-2 div.truePercentage{ width: 30px;}
div.question-row div.stats-2 div.falsePercentage{ width: 30px;}

</style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h2 style="float: left;">Fragen</h2>
        <div style="float: right;">
            <%= Buttons.Link("Frage erstellen", Links.CreateQuestion, Links.CreateQuestionController, ButtonIcon.Add)%>
        </div>

    </div>

    <div style="clear: both;">
        <% foreach (var row in Model.QuestionRows)
           {
               Html.RenderPartial("QuestionRow", row);
           } %>
    </div> 

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightMenu" runat="server">
</asp:Content>
