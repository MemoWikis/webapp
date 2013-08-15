<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<a href="#" style="vertical-align: middle; margin-right: 10px; "><i class="icon-question-sign" id="tabInfoMyKnowledge"></i> Hilfe </a> 
<%
    var userSession = new SessionUser();
    if (userSession.IsLoggedIn)
    {
%>
        
        
        <div class="dropdown" style="display: inline-block; padding-left: 5px;">
          <a class="dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
              <img src="/Images/Users/1_20.jpg" /> <span style="vertical-align: middle;">Hallo <b><%= userSession.User.Name%></b>!</span>
            <b class="caret"></b>
          </a>
          <ul class="dropdown-menu" role="menu" aria-labelledby="dLabel">
            <li><a href="<%= Url.Action(Links.Logout, Links.AccountController) %>"><i class="icon-off" title="Abmelden"></i> Abemelden</a>  </li>
            <li><a href="<%=Url.Action(Links.User, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>">Dein Profil</a></li>
            <li><a href="#">Einstellungen</a></li>
          </ul>
        </div>
        <%
    }
    else {
%> 
        <%= Html.ActionLink("Anmelden", "LogOn", "Account")%>
<%
    }
%>
