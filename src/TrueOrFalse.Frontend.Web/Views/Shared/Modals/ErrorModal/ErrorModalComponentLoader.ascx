<%@ Import Namespace="System.Web.Optimization" %>

<div id="ErrorModalLoader">
    <% Html.RenderPartial("/Views/Shared/Modals/ErrorModal/ErrorModalComponent.vue.ascx") %>
</div>

<%= Scripts.Render("~/bundles/js/errorModal") %>