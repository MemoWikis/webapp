<%@ Import Namespace="System.Web.Optimization" %>

<div id="DefaultModalLoader">
    <% Html.RenderPartial("/Views/Shared/Modals/DefaultModal/DefaultModalComponent.vue.ascx") %>
</div>

<%= Scripts.Render("~/bundles/js/defaultModal") %>