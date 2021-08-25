
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script type="x-template" id="add-comment-component">
    <% Html.RenderPartial("~/Views/Questions/Answer/Comments/AddCommentComponent.vue.ascx"); %>
</script>
<script type="x-template" id="comment-component">
    <%: Html.Partial("~/Views/Questions/Answer/Comments/CommentComponent.vue.ascx") %>
</script>
<script type="x-template" id="comment-section-component">
    <%: Html.Partial("~/Views/Questions/Answer/Comments/CommentsSectionComponent.vue.ascx") %>
</script>


<div v-if="showCommentSectionModal">
    <default-modal-component>
        <div v-slot:header>
            <h2>Diskussionen</h2>
        </div>
        <div v-slot:body>
            <comment-section-component/>
        </div>
        <div v-slot:footer></div>
    </default-modal-component>
</div>
<%= Scripts.Render("~/bundles/js/CommentsSection") %>
