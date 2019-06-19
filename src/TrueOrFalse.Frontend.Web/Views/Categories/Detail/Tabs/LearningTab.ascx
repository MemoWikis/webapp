<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<input  type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>" />
<input type="hidden" id="hddIsLearningSessionOnCategoryPage" value="true" />

<% if (Model.Category.CountQuestionsAggregated > 0)
    {
        var questionId = Model.Category
            .GetAggregatedQuestionsFromMemoryCache()
            .Where(q => q.IsVisibleToCurrentUser())
            .Select(q => q.Id)
            .FirstOrDefault();

        var dummyQuestion = Sl.QuestionRepo.GetById(questionId); // why not take Question from Cache directly?

        if (Model.IsLoggedIn)
        {
            Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion.Id));
        }
    }
    else
    { %>
        <div class="NoQuestions" style="margin-top: 40px;">
            Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten.
        </div>
    <% } %>

<div id="AnswerBody">
    
</div>
