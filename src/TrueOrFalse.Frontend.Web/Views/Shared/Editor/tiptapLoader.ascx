<script type="text/x-template" id="editor-menu-bar-template">
    <%: Html.Partial("~/Views/Shared/Editor/EditorMenuBarComponent.vue.ascx") %>
</script>
<script type="text/javascript">
    eventBus.$emit('tiptap-is-ready', true);
</script>