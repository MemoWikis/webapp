<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<LearningSessionResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Ergebnis</title>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

     <style type="text/css">
         
        
          
     </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Hallo</h2>
    <div class="barWrapper">
        <div id="stepsWrong" class="barPart" style="width: <%= Model.NumberWrongAnswersPercentage%>%">
            <%= Model.NumberWrongAnswers %>
        </div>
        <div id="stepsCorrect" class="barPart" style="width: <%= Model.NumberCorrectPercentage%>%">
            <%= Model.NumberCorrectAnswers %>
        </div>
        <div id="stepsUnanswered" class="barPart" style="width: <%= Model.NumberSkippedPercentage%>%">
            <%= Model.NumberSkipped %>
        </div>
    </div>
</asp:Content>