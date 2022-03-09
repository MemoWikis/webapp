<%@ Import Namespace="System.Web.Optimization" %>

<script type="x-template" id="session-config-template">
    <%: Html.Partial("~/Views/Questions/SessionConfig/SessionConfigComponent.vue.ascx") %>
</script>

<script type="x-template" id="session-progress-bar-template">
    <%: Html.Partial("~/Views/Questions/SessionConfig/SessionProgressBarComponent.vue.ascx") %>
</script>
<%= Styles.Render("~/bundles/SessionConfig") %>
<%= Scripts.Render("~/bundles/js/SessionConfigComponent") %>


