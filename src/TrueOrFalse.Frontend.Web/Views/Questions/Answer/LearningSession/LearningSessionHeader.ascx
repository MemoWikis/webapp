<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
        
    Du lernst 
    <% if (Model.LearningSession.Questions().Count() == Model.LearningSession.TotalPossibleQuestions){ %>
        alle <%= Model.LearningSession.Questions().Count() %>
    <% }else { %>
        <%= Model.LearningSession.Questions().Count() %> 
    <% } %>
        
    <% if(Model.LearningSession.IsSetSession) { %>
        Fragen aus dem Fragesatz 
        <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
        </a>
    <% } %>

    <% if(Model.LearningSession.IsSetsSession) { %>
        Fragen aus "<%= Model.LearningSession.SetListTitle %>" (<%= Model.LearningSession.SetsToLearn().Count %> Fragesätze)
    <% } %>

    <% if(Model.LearningSession.IsCategorySession) { %>
        Fragen zum Thema 
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
</div>

<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <%= Model.CurrentLearningStepIdx + 1 %> von <%= Model.LearningSession.Steps.Count %></div>
    <div class="SessionType">
        <span class="show-tooltip"
        data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>kannst du dir die Lösung anzeigen lassen</li>
                    <li>kannst du Fragen überspringen</li>
                    <li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt</li>
                </ul>
            </div>"%>" data-html="true" style="float: left;">
            Lernen 
            <i class="fa fa-info-circle"></i>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.CurrentLearningStepPercentage== 0 ? "0" : Model.CurrentLearningStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <%= Model.CurrentLearningStepPercentage %>%
            </div>
        </div>
        <% if (Model.CurrentLearningStepPercentage<100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: <%= 100-Model.CurrentLearningStepPercentage %>%;"></div>
        <% } %>
            
    </div>
</div>
