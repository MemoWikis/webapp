<%@ Import Namespace="System.Web.Optimization" %>

<script type="x-template" id="default-modal-component">
    <%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponent.vue.ascx") %>
</script>

<%= Scripts.Render("~/bundles/js/defaultModal") %>
