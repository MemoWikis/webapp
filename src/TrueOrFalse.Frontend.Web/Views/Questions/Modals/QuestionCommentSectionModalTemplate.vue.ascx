<%@ Import Namespace="System.Web.Optimization" %>
<script type="x-template" id="add-comment-component">
    <%: Html.Partial("~/Views/Questions/Answer/Comments/AddCommentComponent.vue.ascx") %>
</script>
<script type="x-template" id="comment-component">
    <%: Html.Partial("~/Views/Questions/Answer/Comments/CommentComponent.vue.ascx") %>
</script>
<script type="x-template" id="comment-section-component">
    <%: Html.Partial("~/Views/Questions/Answer/Comments/CommentsSectionComponent.vue.ascx") %>
</script>
<script type="x-template" id="question-comment-section-modal-component">
    <%: Html.Partial("/Views/Questions/Modals/QuestionCommentSectionModalComponentLoader.vue.ascx") %>
</script>
<script type="x-template" id="comment-answer-component">
    <%: Html.Partial("/Views/Questions/Answer/Comments/CommentAnswerComponent.vue.ascx") %>
</script>
<script type="x-template" id="comment-answer-add-component">
    <%: Html.Partial("/Views/Questions/Answer/Comments/CommentAnswerAddComponent.vue.ascx") %>
</script>
<%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponentLoader.ascx") %>

<%= Scripts.Render("~/bundles/js/CommentsSection") %>