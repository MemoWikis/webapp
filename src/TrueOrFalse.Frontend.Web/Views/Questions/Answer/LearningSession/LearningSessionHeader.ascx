<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
<%--    <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>--%>
    <div class="SessionTitle">
        <div class="CollectionType TypeCategory">Thema</div>
            <% var category = EntityCache.GetCategory(Model.CategoryModel.Id); 
                Html.RenderPartial("CategoryLabel", category); %>
        <% if(Model.LearningSession != null && Model.LearningSession.Config.InWishknowledge) { %>
            <a href="<%= Links.QuestionsWish() %>">Dein Wunschwissen</a>
        <% } %>
    </div>
</div>
<div class="SessionBar">
    <div class="QuestionCount">Frage <span id="CurrentStepNumber"><%= Model.CurrentLearningStepIdx + 1 %></span>von<span id="StepCount">10</span></div>
    <div class="ProgressBarContainer">
        <span id="spanPercentageDone"> 0% </span>
        <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: 0;">
            <div class="ProgressBarSegment ProgressBarLegend"></div>
        </div>
        <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
    </div>
</div>
