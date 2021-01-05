<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Microsoft.VisualBasic.Activities" %>

<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>" />
<input type="hidden" id="hddIsLearningSessionOnCategoryPage" value="true" />
<input type="hidden" id="hddLearningSessionStarted" value="False" />
<%----%>


<input type="hidden" id="hddIsLearningSession" value="<%= Model.IsLearningSession %>"
    data-learning-session-id="-1"
    data-current-step-guid=""
    data-current-step-idx=""
    data-is-last-step=""
    data-skip-step-index="" />

<input type="hidden" id="hddIsTestSession" value="<%= Model.IsTestSession %>"
    data-test-session-id="-1"
    data-current-step-idx=""
    data-is-last-step="" />

<%= Scripts.Render("~/bundles/js/d3") %>
<script type="text/x-template" id="question-details-component">
    <%: Html.Partial("~/Views/Questions/Answer/AnswerQuestionDetailsComponent.vue.ascx") %>
</script>
<%= Scripts.Render("~/bundles/js/QuestionDetailsComponent") %>

<%
    var dummyQuestion = Model.GetDummyQuestion(); 
    if(dummyQuestion.Id != 0)
       Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion, null, true, Model)); 
    else
    { %>
        <div class="NoQuestions" style="margin-top: 40px;">
            Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten.
        </div>
  <% } %>

<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
    <div id="QuestionDetails" data-div-type="questionDetails"></div>
</div>

<% Html.RenderPartial("~/Views/Questions/QuestionList/QuestionList.ascx", new QuestionListModel(Model.Category.Id, Model.IsDisplayNoneSessionConfigNoteQuestionList)); %>
<% Html.RenderPartial("~/Views/Questions/Modals/ModalDeleteQuestion.ascx"); %>
<div id="LearningTabFABApp">
    <%: Html.Partial("~/Views/Categories/Detail/Partials/FloatingActionButton/FloatingActionButton.ascx", new FloatingActionButtonModel(Model.Category, false)) %>
</div>
<%= Scripts.Render("~/bundles/js/FloatingActionButton") %>
<%= Scripts.Render("~/bundles/js/LearningTabFABLoader") %>

