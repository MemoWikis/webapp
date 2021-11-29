<%@ Import Namespace="System.Web.Optimization" %>
<script type="x-template" id="login-modal-component-loader">
    <%: Html.Partial("/Views/Welcome/Login/LoginModalComponentLoader.vue.ascx") %>
</script>

<%: Html.Partial("/Views/Shared/Modals/DefaultModal/DefaultModalComponentLoader.ascx") %>

<%= Scripts.Render("~/bundles/RegistrationJs") %>    
<%= Styles.Render("~/bundles/Registration") %>
