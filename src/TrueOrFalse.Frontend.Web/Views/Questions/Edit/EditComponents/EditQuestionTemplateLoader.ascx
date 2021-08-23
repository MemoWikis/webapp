<%@ Import Namespace="System.Web.Optimization" %>
<%= Scripts.Render("~/bundles/js/EditQuestion") %>

<script type="x-template" id="edit-question-modal-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditQuestionModal.vue.ascx") %>
</script>
<script type="text/javascript">
    eventBus.$emit('edit-question-is-ready')
</script>