<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

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
<%= Styles.Render("~/bundles/AnswerQuestion") %>
<%= Scripts.Render("~/bundles/js/AnswerQuestion") %> 
<%= Scripts.Render("~/bundles/js/jqueryUi") %>
<%= Scripts.Render("~/bundles/js/bootstrapTooltip") %>
<%= Styles.Render("~/bundles/jqueryUi") %>

<script type="text/x-template" id="question-details-component">
    <%: Html.Partial("~/Views/Questions/Answer/AnswerQuestionDetailsComponent.vue.ascx") %>
</script>
<%= Scripts.Render("~/bundles/js/QuestionDetailsComponent") %>

<div id="SessionHeader" class="row">
    <input id="SessionConfigQuestionChecker" type="hidden" data-category-has-no-questions="<%= Model.HasQuestions %>">
    <div v-show="showFilter">
        <session-config-component :is-logged-in="'<%= SessionUserLegacy.IsLoggedIn %>' == 'True'">
            <session-progress-bar-component/>
        </session-config-component>
    </div>

</div>

<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
    <div id="QuestionDetails" data-div-type="questionDetails"></div>
</div>
<%: Html.Partial("~/Views/Questions/SessionConfig/SessionConfigComponentLoader.ascx") %>
<% Html.RenderPartial("~/Views/Questions/Modals/DeleteQuestionModalTemplateLoader.ascx"); %>
<% Html.RenderPartial("~/Views/Questions/QuestionList/QuestionList.ascx", new QuestionListModel(Model.Category.Id, Model.ShowLearningSessionConfigurationMessageForQuestionList)); %>
<%= Scripts.Render("~/bundles/js/d3") %>
