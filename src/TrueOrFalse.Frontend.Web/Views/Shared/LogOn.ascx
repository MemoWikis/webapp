<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Core.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%
    var userSession = new SessionUser();
    if (userSession.IsLoggedIn)
    {
%>
        Hallo  <b><%= Html.ActionLink(userSession.User.Name, "Profile", "UserProfile", new {name = userSession.User.Name, id = userSession.User.Id} , null)%></b>!
        [ <%= Html.ActionLink("Abmelden", Links.Logout, Links.AccountController) %> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Anmelden", "LogOn", "Account")%> ]
<%
    }
%>
