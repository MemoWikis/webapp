<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<%--<div class="" style="margin-bottom: 20px;">
    <div class="progress" style="position: relative;" id="divPercentageDone">
        <div class="progress-bar" role="progressbar" id="progressPercentageDone"
            aria-valuemin="0" aria-valuemax="100" style="min-width: 2em; width: <%= Model.TestSessionCurrentStepPercentage%>%">
            <span id="spanPercentageDone" style="margin-left: 2px;"><%= Model.TestSessionCurrentStepPercentage%>%</span>
        </div>
    </div>
</div>--%>

<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <%= Model.TestSessionCurrentStep %> von <%= Model.TestSessionNumberOfSteps %></div>
    <div class="SessionType">
       <%-- <span class="show-tooltip"
            data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>kannst du dir die Lösung anzeigen lassen</li>
                    <li>kannst du Fragen überspringen</li>
                    <li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt</li>
                </ul>
            </div>"%>"
            data-html="true" style="float: left;">--%>
        <span>
            Testen 
            <%--<i class="fa fa-info-circle"></i>--%>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.TestSessionCurrentStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <%= Model.TestSessionCurrentStepPercentage %>%
            </div>
        </div>
        <% if (Model.CurrentLearningStepPercentage<100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: <%= 100-Model.TestSessionCurrentStepPercentage %>%;"></div>
        <% } %>
            
    </div>
</div>