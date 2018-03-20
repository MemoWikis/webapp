<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>


    data-learning-session-id="-1"
    data-current-step-guid=""
    data-current-step-idx=""
    data-is-last-step=""
    data-skip-step-index="" />
 

<%--<input type="hidden" id="hddIsTestSession" value="<%= Model.IsTestSession %>" 
       data-test-session-id="<%= Model.IsTestSession ? Model.TestSessionId : -1 %>"
       data-current-step-idx="<%= Model.IsTestSession ? Model.TestSessionCurrentStep : -1 %>"
       data-is-last-step="<%= Model.TestSessionIsLastStep %>"/--%>>


<input type="hidden" id="hddQuestionId" value="1" />
<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>" />
<input type="hidden" id="hddLearningSessionStarted" value="False" />
<input type="hidden" id="hddIsLearningSessionOnCategoryPage" value="true" />

       data-current-step-guid=""
       data-current-step-idx=""
       data-is-last-step="4"
/>
 
<%--<input type="hidden" id="hddIsLearningSession" value="<%= Model.IsLearningSession %>" 
       data-learning-session-id="<%= Model.IsLearningSession ? Model.LearningSession.Id : -1 %>"
       data-current-step-guid="<%= Model.IsLearningSession ? Model.LearningSessionStep.Guid.ToString() : "" %>"
       data-current-step-idx="<%= Model.IsLearningSession ? Model.LearningSessionStep.Idx : -1 %>"
       data-is-last-step="<%= Model.IsLastLearningStep %>"
       data-skip-step-index = "-1" />--%>


<% 
    if (Model.Category.CountQuestionsAggregated > 0)
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
        else
        {
            var learningTabModel = new LearningTabModel();
            Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx",learningTabModel.answerQuestionModel);
            Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx", new AnswerBodyModel(learningTabModel.answerQuestionModel));
        }
    }
    else
    { %>
        <div class="NoQuestions" style="margin-top: 40px;">
            Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten.
        </div>
    <% } %>

<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
</div>
