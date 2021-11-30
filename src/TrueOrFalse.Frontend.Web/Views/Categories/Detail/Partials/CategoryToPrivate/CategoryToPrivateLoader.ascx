<%@ Import Namespace="System.Web.Optimization" %>

<%= Scripts.Render("~/bundles/js/CategoryToPrivate") %>
<script type="text/x-template" id="category-to-private">
        <%: Html.Partial("~/Views/Categories/Detail/Partials/CategoryToPrivate/CategoryToPrivate.vue.ascx") %>
</script>
