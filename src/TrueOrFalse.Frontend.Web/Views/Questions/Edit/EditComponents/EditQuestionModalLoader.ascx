<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var user = SessionUser.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
    var isMyWorld = user != null && UserCache.GetItem(user.Id).IsFiltered;
%>
<div id="EditQuestionLoaderApp">
    <edit-question-modal-component v-if="tiptapIsReady && modalIsReady && editQuestionComponentLoaded" is-admin="<%= isAdmin %>" is-my-world="<%= isMyWorld %>" />
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
