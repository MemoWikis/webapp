<%@ Import Namespace="System.Web.Optimization" %>

<%= Scripts.Render("~/bundles/js/searchTemplate") %>

<script type="text/x-template" id="search-component">
        <%: Html.Partial("~/Views/Shared/Search/SearchComponent.vue.ascx") %>
</script>

