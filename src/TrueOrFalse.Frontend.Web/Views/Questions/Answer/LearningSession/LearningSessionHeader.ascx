<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="" style="margin-bottom: 20px;">
<h2>Lernen</h2>
    <div class="well" style="background-color: transparent;">
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
    </div>
   
</div>