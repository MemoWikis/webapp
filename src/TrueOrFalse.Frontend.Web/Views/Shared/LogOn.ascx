﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="LoginApp">
    <login-modal-component-loader v-if="loaded"/>
</div>
<div class="login-register-container">
    <div class="btn memo-button link-btn login-btn" data-btn-login="true" onclick="eventBus.$emit('show-login-modal')">
        <i class="fa fa-sign-in"></i>
        Anmelden
    </div>
    <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>">
        <div class="btn memo-button register-btn">Kostenlos registrieren!</div>
    </a>
</div>