<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h3>Anmelden</h3>
<div style="width:155px;">
Benutzername:<br />
<input type="text" />
Passwort:<br />
<input type="password" />
<input value="Anmelden" type="submit" style="float:right; margin-top:4px; font-size:13px;" />
</div>

<div style="width:155px; margin-top:50px;" >
<h3>Registrieren</h3>
Noch kein Benutzer?<br />
<%= Html.ActionLink("Hier anmelden", Links.Register, Links.WelcomeController)%>
</div>