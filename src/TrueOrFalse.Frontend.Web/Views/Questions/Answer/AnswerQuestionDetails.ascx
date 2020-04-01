<%@ Import Namespace="System.Web.Optimization" %>

<%= Scripts.Render("~/bundles/js/d3") %>
<script type="text/x-template" id="question-details-component">
    <%: Html.Partial("~/Views/Questions/Answer/AnswerQuestionDetailsComponent.vue.ascx") %>
</script>

<%= Scripts.Render("~/bundles/js/QuestionDetailsComponent") %>


<div id="QuestionDetailsApp">
    <question-details-component/>
</div>

<%= Scripts.Render("~/bundles/js/QuestionDetailsApp") %>

