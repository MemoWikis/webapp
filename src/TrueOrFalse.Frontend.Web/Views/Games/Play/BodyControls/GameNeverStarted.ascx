<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GameNeverStarted>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h3>Das Spiel wurde nicht gestartet</h3>

<p>Es haben sich keine zwei Spieler gefunden.</p>

<div style="margin-top: 20px;">
    <a href="<%= Links.Games(Url) %>" style="font-size: 18px; margin: 0;">
        <i class="fa fa-list"></i>&nbsp;zur Übersicht
    </a>
</div>