<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
    
    <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>

    <div class="SessionTitle">
        <div class="CollectionType TypeCategory">Thema</div>
            <% var category = EntityCache.GetCategory(Model.CategoryModel.Id); 
                Html.RenderPartial("CategoryLabel", category); %>

        <% if(Model.LearningSession != null && Model.LearningSession.Config.OnlyWuwi) { %>
            <a href="<%= Links.QuestionsWish() %>">Dein Wunschwissen</a>
        <% } %>
    </div>
</div>

<div class="SessionBar">
    <div class="QuestionCount">Frage <span id="CurrentStepNumber"><%= Model.CurrentLearningStepIdx + 1 %></span> von <span id="StepCount"><%= Model.CategoryModel.AggregatedQuestionCount %></span></div>
    <div class="ProgressBarContainer">
        <span id="spanPercentageDone"><%= Model.CurrentLearningStepPercentage %>%</span>
        <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.CurrentLearningStepPercentage== 0 ? "0" : Model.CurrentLearningStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend"></div>
        </div>
        <% if (Model.CurrentLearningStepPercentage < 100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
        <% } %>
            
    </div>
</div>
