<%@ Import Namespace="System.Web.Optimization" %>
    
<%= Scripts.Render("~/bundles/js/tiptap") %>
<script type="text/x-template" id="editor-menu-bar-template">
    <%: Html.Partial("~/Views/Shared/Editor/EditorMenuBarComponent.vue.ascx") %>
</script>
<script type="text/javascript">
    eventBus.$emit('tiptap-is-ready')
</script>