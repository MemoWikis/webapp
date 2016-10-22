<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<%  var userSession = new SessionUser(); %>

<a id="SupportUs" class="helpLink TextLinkWithIcon" href="<%= Url.Action(Links.Membership, Links.AccountController) %>">
    <i class="fa fa-thumbs-up"></i>
    <span class="TextSpan">
        <% if (userSession.IsLoggedIn && userSession.User.IsMember()){ %>
            Du bist Unterstützer!
        <% } else { %>
            Werde Unterstützer!
        <% } %>
    </span>
</a>
<a class="helpLink TextLinkWithIcon" href="<%= Links.HelpFAQ() %>">
    <i class="fa fa-question-circle"></i>
    <span class="TextSpan Help">FAQ</span>
</a> 

<%--<a href="#" class="helpLink TextLinkWithIcon" id="startWelcomeTour">
    <i class="fa fa-map-signs"></i> Kurze Tour
</a>--%>

<%
    
    if (userSession.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
%>
        <div class="dropdown" style="display: inline-block;">
            <span>Hallo</span>
            <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
                <span class="userName TextSpan"><b><%= userSession.User.Name%></b></span>
                <b class="caret"></b>
                <img class="userImage" src="<%= imageSetttings.GetUrl_30px_square(userSession.User.EmailAddress).Url %>" /> 
            </a>
            <ul class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel">
                <li><a href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>"><i class="fa fa-user"></i> Deine Profilseite</a></li>
                <li><a href="<%= Url.Action(Links.UserSettings, Links.UserSettingsController) %>"><i class="fa fa-wrench" title="Einstellungen"></i> Konto-Einstellungen</a></li>
                <li class="divider"></li>
                 
                <li><a href="<%= Url.Action(Links.Logout, Links.WelcomeController) %>"><i class="fa fa-power-off" title="Ausloggen"></i> Ausloggen</a>  </li>
                <% if(userSession.IsInstallationAdmin){ %>
                    <li><a href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>"><i class="fa fa-power-off" title="Ausloggen"></i> Adminrechte abgeben</a>  </li>
                <% } %>
            </ul>
        </div>
<%
    }else {
%> 
        <a class="TextLinkWithIcon" href="<%=Url.Action("Login", Links.WelcomeController) %>" title="Einloggen"><i class="fa fa-sign-in"></i> <span class="TextSpan">Einloggen</span></a>
<%
    }
%>    