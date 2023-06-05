<%@ Import Namespace="System.Web.Optimization" %>
<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var user = SessionUserLegacy.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
%>
<div id="EditQuestionLoaderApp">
    <edit-question-modal-component v-if="tiptapIsReady && modalIsReady && editQuestionComponentLoaded" is-admin="<%= isAdmin %>" />
</div>
<%= Scripts.Render("~/bundles/js/EditQuestionLoader") %>
