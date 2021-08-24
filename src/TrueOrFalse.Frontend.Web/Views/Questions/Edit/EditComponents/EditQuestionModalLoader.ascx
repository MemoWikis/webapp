<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var userSession = new SessionUser();
    var user = userSession.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
    var isMyWorld = user != null && UserCache.GetItem(user.Id).IsFiltered;
%>
<div id="EditQuestionLoaderApp">
    <edit-question-modal-component v-if="tiptapIsReady && modalIsReady" is-admin="<%= isAdmin %>" is-my-world="<%= isMyWorld %>" />
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
