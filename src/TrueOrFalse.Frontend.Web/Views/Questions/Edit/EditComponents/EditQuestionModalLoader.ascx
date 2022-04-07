<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var user = SessionUser.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
    var isMyWorld = user != null && UserCache.GetItem(user.Id).IsFiltered;
%>
<div id="EditQuestionLoaderApp">
    <template v-if="tiptapIsReady && modalIsReady">
        <edit-question-modal-component is-admin="<%= isAdmin %>" is-my-world="<%= isMyWorld %>" />
    </template>
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
