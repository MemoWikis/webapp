<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<% 
    var userSession = new SessionUser();
    var user = userSession.User;
%>
<div style="display: flex;">
    <div class="header-item">
        <a href="/Wissenszentrale" class="TextLinkWithIcon KnowledgeLink"><i class="fa fa-dot-circle"></i>
            <span class="primary-point-text">Wissenszentrale</span>
        </a>
    </div>
    <div class="header-item">
       <%
    
    if (userSession.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
%>
        <div style="display:flex; flex-direction:column;">
        <div style="display:flex;">         
          <img class="userImage" src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>" /> 
         <span id="header-level-display" class="level-display">
            <a id="header-level-display-popover" class="" href="#" data-toggle="popover" data-trigger="focus" data-placement="auto top"
                title='<div id="activity-popover-title">Dein erreichtes Level</div>'>
                <span style="display: inline-block; white-space: nowrap; margin-top:-11px;" class="" data-placement="bottom">
                    <span>
                        <span class="half-circle"> <span class="level-count"><%= userSession.User.ActivityLevel %></span></span>
                       
                    </span>
                </span>
            </a>
        </span>
        </div>
            <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
                <span class="userName TextSpan">Hallo <b><%= userSession.User.Name%></b><b class="caret"></b></span>
            </a>              
<div class="half-circle"></div>
        <div class="dropdown" style="display: inline-block;">
            <ul class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel">
                <li><a href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>"><i class="fa fa-user"></i> Deine Profilseite</a></li>
                <li><a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>"><i class="fa fa-wrench" title="Einstellungen"></i> Konto-Einstellungen</a></li>
                <li class="divider"></li>
                 
                <li><a href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser() ? "true" : ""  %>"><i class="fa fa-power-off" title="Ausloggen"></i> Ausloggen</a>  </li>
                <% if (userSession.IsInstallationAdmin)
                    { %>
                    <li><a href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>"><i class="fa fa-power-off" title="Ausloggen"></i> Adminrechte abgeben</a>  </li>
                <% } %>
            </ul>
        </div>
</div>
      

<%
    }else {
%> 
        <a class="TextLinkWithIcon" href="#" data-btn-login="true" title="Einloggen"><i class="fa fa-sign-in"></i>
        <span class="TextSpan">Einloggen</span></a>
<%
    }
%>    
    </div>
    <div class="header-item" style="margin-right:0px;">
        <a id="MenuButton" class="TextLinkWithIcon"><i class="fa fa-bars"></i>
        <span class="TextSpan">Menü</span></a>
    </div>
    
</div>
