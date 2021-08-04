<%@ Import Namespace="System.Web.Optimization" %>

<div id="AlertModalLoader">
    <% Html.RenderPartial("/Views/Shared/Modals/AlertModal/AlertModalComponent.vue.ascx") %>
</div>

<%= Scripts.Render("~/bundles/js/alertModal") %>