<%@ Import Namespace="System.Web.Optimization" %>

<script type="x-template" id="delete-question-modal">
    <%: Html.Partial("/Views/Questions/Modals/DeleteQuestionModalComponent.vue.ascx") %>
</script>

<%= Scripts.Render("~/bundles/js/DeleteQuestion") %>
