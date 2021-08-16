<%@ Import Namespace="System.Web.Optimization" %>

<%= Scripts.Render("~/bundles/js/lazy") %>
<script type="text/x-template" id="lazy-component">
        <%: Html.Partial("~/Views/Shared/Lazy/LazyComponent.vue.ascx") %>
</script>