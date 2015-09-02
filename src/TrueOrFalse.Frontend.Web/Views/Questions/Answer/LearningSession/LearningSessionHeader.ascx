<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="well">
<h3>Lernen</h3>
    <div>
        Du lernst 
            <% if (Model.LearningSession.Steps.Count() == Model.LearningSession.SetToLearn.Questions().Count())
                { %>
                alle
            <% }
                else { %>
                <%= Model.LearningSession.Steps.Count() %> von <% } %>

        <%= Model.LearningSession.SetToLearn.Questions().Count() %> Fragen 
        aus dem Fragesatz 
        <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
        </a>

        <br/>Frage <%= Model.LearningSessionCurrentStepNo %> von <%= Model.LearningSession.Steps.Count() %>
        <br/>Frage-Id:  <%= Model.LearningSessionStep.Question.Id %>
    </div>
   
</div>