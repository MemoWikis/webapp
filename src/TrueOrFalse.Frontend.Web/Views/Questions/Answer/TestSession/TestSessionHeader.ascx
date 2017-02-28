<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
    Du machst einen Test mit 
    <% if (Model.TestSessionNumberOfSteps == Model.TestSession.TotalPossibleQuestions){ %>
        allen <%= Model.TestSession.TotalPossibleQuestions %>
    <% }else { %>
        <%= Model.TestSessionNumberOfSteps %> 
    <% } %>
        
    <% if(Model.TestSession.IsSetSession) { %>
        Fragen aus dem Fragesatz 
        <a href="<%= Model.TestSession.SetLink %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-set"><%: Model.TestSession.SetName %></span>
        </a>
    <% } %>

    <% if(Model.TestSession.IsSetsSession) { %>
        Fragen aus "<%= Model.TestSession.SetListTitle %>" (<%= Model.TestSession.SetsToTestIds.Count %> Fragesätze)
    <% } %>

    <% if(Model.TestSession.IsCategorySession) { %>
        Fragen zum Thema 
        <a href="<%= Links.CategoryDetail(Model.TestSession.CategoryToTest.Name, Model.TestSession.CategoryToTest.Id) %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-category"><%: Model.TestSession.CategoryToTest.Name %></span>
        </a>
    <% } %>
        
    <% if (Model.TestSessionNumberOfSteps < Model.TestSession.TotalPossibleQuestions){ %>
        mit <%= Model.TestSession.TotalPossibleQuestions %> Fragen.
    <% } %>
</div>
<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <%= Model.TestSessionCurrentStep %> von <%= Model.TestSessionNumberOfSteps %></div>
    <div class="SessionType">
        <span class="show-tooltip"
            data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>werden die Fragen zufällig ausgewählt</li>
                    <li>hast du jeweils nur einen Antwortversuch</li>
                </ul>
            </div>"%>"
            data-html="true" style="float: left;">
            Testen
            <span class="fa-stack fa-1x" style="font-size: 10px; top: -1px;">
                <i class="fa fa-circle fa-stack-2x" style="color: #e1efb3;"></i>
                <i class="fa fa-info fa-stack-1x" style=""></i>
            </span>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.TestSessionCurrentStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <span id="spanPercentageDone"><%= Model.TestSessionCurrentStepPercentage %>%</span>
            </div>
        </div>
        <% if (Model.TestSessionCurrentStepPercentage<100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
        <% } %>
            
    </div>
</div>