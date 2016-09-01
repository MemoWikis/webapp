<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h3>Einloggen</h3>
<div style="width:155px;">
    <% using (Html.BeginForm("Login", "Account")) { %>
        E-Mail:<br />
        <input type="text" name="EmailAddress" />
        Passwort:<br />
        <input type="password" name="Password" />
        <input value="Einloggen" type="submit" style="float:right; margin-top:4px; font-size:13px;" />
    <% } %>
</div>

<div style="width:155px; margin-top:50px; " >
<h3>Registrieren</h3>
<span style="font-size:1.2em">Noch kein Benutzer?</span><br />
<%= Html.ActionLink("Hier registrieren", Links.Register, Links.WelcomeController)%>
</div>