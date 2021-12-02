﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="bs-callout bs-callout-danger" style="margin-top: 35px;">
    <h4>Einloggen oder registrieren</h4>
    <p>
        Um Wunschwissen oder eigene Fragen zu verwenden, <br/>
        musst du dich <a href="#" data-btn-login="true" onclick="eventBus.$emit('show-login-modal')">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
    </p>
</div>