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
<script type="x-template" id="default-modal-component">
    <%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponentLoader.ascx") %>
</script>

<%= Scripts.Render("~/bundles/js/CommentsSection") %>
<%= Scripts.Render("~/bundles/js/defaultModal") %>