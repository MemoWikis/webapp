<%--<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LearningSessionModel>" %>--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>

<div class="well">
<h2>Lernsession-Header</h2>
    <div>
        Frage <%= Model.LearningSessionCurrentStepNo %> von <%= Model.LearningSession.Steps.Count %><br />
        Frage-Id:  <%= Model.LearningSessionStep.Question.Id %>
    </div>                                    
</div>