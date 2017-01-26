<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="" style="margin-bottom: 20px;">
    <h2 style="margin-top: 0; position: relative;">
        <span class="<% if (Model.LearningSession.IsDateSession) Response.Write("ColoredUnderline Date");
                        if (Model.LearningSession.IsSetSession) Response.Write("ColoredUnderline Set");
                        %>">Lernen</span>
    
    <span class="h2-additional-info">
        Du lernst 
        <% if (Model.LearningSession.Questions().Count() == Model.LearningSession.TotalPossibleQuestions){ %>
            alle
        <% }else { %>
            <%= Model.LearningSession.Questions().Count() %> 
        <% } %>
        
        <% if(Model.LearningSession.IsSetSession) { %>
            Fragen aus dem Fragesatz 
            <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="margin-top: 3px; display: inline-block;">
                <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
            </a>
        <% } %>
        
        <% if(Model.LearningSession.IsCategorySession) { %>
            Fragen aus der Kategorie 
            <a href="<%= Links.CategoryDetail(Model.LearningSession.CategoryToLearn.Name, Model.LearningSession.CategoryToLearn.Id) %>" style="margin-top: 3px; display: inline-block;">
                <span class="label label-category"><%: Model.LearningSession.CategoryToLearn.Name %></span>
            </a>
        <% } %>
        
        <% if(Model.LearningSession.IsDateSession) { %>
            Fragen aus dem Termin 
            <a href="<%= Links.Dates() %>"><%= Model.LearningSession.DateToLearn.GetTitle() %></a>
        <% } %>

        <% if(Model.LearningSession.IsWishSession) { %>
            Fragen aus deinem  
            <a href="<%= Links.QuestionsWish() %>">Wunschwissen</a>
        <% } %>

        <% if (Model.LearningSession.Questions().Count() < Model.LearningSession.TotalPossibleQuestions){ %>
            mit <%= Model.LearningSession.TotalPossibleQuestions %> Fragen.
        <% } %>
    </span>
    </h2>

    <div class="progressBarContainer">
        <% if (Model.CurrentLearningStepPercentage>0) {%>
            <div class="progressBar progressBarDone" style="width: <%= Model.CurrentLearningStepPercentage %>%;">
                &nbsp;
            </div>
        <% } %>                
        <% if (Model.CurrentLearningStepPercentage<100) {%>
            <div class="progressBar progressBarLeft" style="width: <%= 100-Model.CurrentLearningStepPercentage %>%;">
                &nbsp; 
            </div>
        <% } %>   
        <div class="progressBar progressBarLegend">
            <%= Model.CurrentLearningStepPercentage %>%
        </div>
    </div>

</div>