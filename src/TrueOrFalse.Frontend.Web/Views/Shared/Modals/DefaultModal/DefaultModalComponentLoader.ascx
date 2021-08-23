<%@ Import Namespace="System.Web.Optimization" %>

<script type="x-template" id="modal-component-template">
    <%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponent.vue.ascx") %>
</script>

<%= Scripts.Render("~/bundles/js/defaultModal") %>
