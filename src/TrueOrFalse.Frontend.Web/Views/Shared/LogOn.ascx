<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Core.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%
    var userSession = new SessionUser();
    if (userSession.IsLoggedIn)
    {
%>
        Hallo <b><%= Html.Encode(userSession.User.UserName) %></b>!
        [ <%= Html.ActionLink("Abmelden", Links.Logout, Links.AccountController) %> ]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Anmelden", "LogOn", "Account")%> ]
<%
    }
%>
