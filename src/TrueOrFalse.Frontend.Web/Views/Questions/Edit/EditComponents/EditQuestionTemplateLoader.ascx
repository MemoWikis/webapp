<%@ Import Namespace="System.Web.Optimization" %>
<%= Scripts.Render("~/bundles/js/EditQuestion") %>

<script type="x-template" id="textsolution-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Text/TextSolutionComponent.vue.ascx") %>
</script>
<script type="x-template" id="multiplechoice-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MultipleChoice/MultipleChoiceComponent.vue.ascx") %>
</script>
<script type="x-template" id="matchlist-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MatchList/MatchListComponent.vue.ascx") %>
</script>
<script type="x-template" id="flashcard-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/FlashCard/FlashCardComponent.vue.ascx") %>
</script>
<script type="x-template" id="categorychip-template">
    <%: Html.Partial("~/Views/Shared/CategoryChip/CategoryChipComponent.vue.ascx") %>
</script>
<script type="x-template" id="edit-question-modal-template">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditQuestionModal.vue.ascx") %>
</script>
<script type="text/javascript">
    eventBus.$emit('edit-question-is-ready')
</script>