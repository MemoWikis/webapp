<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%= Scripts.Render("~/bundles/js/EditQuestion") %>
<div id="EditQuestionLoaderApp">
    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditQuestionModal.vue.ascx") %>
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
