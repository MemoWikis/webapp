<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="" style="margin-bottom: 20px;">
    
    <div class="progress" style="position: relative;" id="divPercentageDone">
        <div class="progress-bar" role="progressbar" id="progressPercentageDone"
            aria-valuemin="0" aria-valuemax="100" style="min-width: 2em; width: <%= Model.TestSessionCurrentStepPercentage%>%">
            <span id="spanPercentageDone" style="margin-left: 2px;"><%= Model.TestSessionCurrentStepPercentage%>%</span>
        </div>
    </div>

</div>