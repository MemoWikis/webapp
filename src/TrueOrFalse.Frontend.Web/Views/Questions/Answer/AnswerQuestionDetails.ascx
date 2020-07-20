<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>

<div id="QuestionDetailsApp">
    <question-details-component base-question-id="<%= Model.QuestionId %>"/>
</div>

<%= Scripts.Render("~/bundles/js/QuestionDetailsApp") %>

