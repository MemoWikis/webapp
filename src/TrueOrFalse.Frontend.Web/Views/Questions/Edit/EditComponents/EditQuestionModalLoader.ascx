<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var userSession = new SessionUser();
    var user = userSession.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
%>
<div id="EditQuestionLoaderApp">
    <edit-question-modal-component v-if="tiptapIsReady && modalIsReady" is-admin="<%= isAdmin %>" />
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
