<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="" style="margin-bottom: 20px;">
    <h2 style="margin-top: 0;">Lernen</h2>

    <div class="well" style="background-color: transparent;">
        Du lernst 
        <% if (Model.LearningSession.Steps.Count() == Model.LearningSession.TotalPossibleQuestions){ %>
            alle
        <% }else { %>
            <%= Model.LearningSession.Steps.Count() %> von 
        <% } %>
        
        <% if(Model.LearningSession.IsSetSession) { %>
            <%= Model.LearningSession.TotalPossibleQuestions %> Fragen 
            aus dem Fragesatz 
            <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="margin-top: 3px; display: inline-block;">
                <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
            </a>
        <% } %>
        
        <% if(Model.LearningSession.IsDateSession) { %>
            <%= Model.LearningSession.TotalPossibleQuestions %> Fragen 
            aus dem Termin <a href="<%= Links.Dates() %>"><%= Model.LearningSession.DateToLearn.GetTitle() %></a>
        <% } %>
        <br/>Frage <%= Model.LearningSessionCurrentStepNo %> von <%= Model.LearningSession.Steps.Count() %>        
    </div>
</div>