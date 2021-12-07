<%@ Import Namespace="System.Web.Optimization" %>
<script type="x-template" id="login-modal-template">
    <%: Html.Partial("/Views/Welcome/Login/LoginModalComponentTemplate.vue.ascx") %>
</script>

<%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponentLoader.ascx") %>
<%= Scripts.Render("~/bundles/LoginModalComponent") %>

