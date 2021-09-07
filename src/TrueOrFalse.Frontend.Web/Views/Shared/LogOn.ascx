<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="FluentNHibernate.Utils" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    var userSession = new SessionUser();
    var user = userSession.User;
%>
<% if (userSession.IsLoggedIn)
   {
       var imageSetttings = new UserImageSettings(userSession.User.Id);
%>
    <div class="dropdown" id="HeaderUserDropdown">
        <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
            <div id="UserBtn">
                <div class="main-user-icon-container">
                    <img src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>"/>
                    <%if (Model.SidebarModel.UnreadMessageCount != 0)
                      { %>
                        <div class="badge-counter"><%= Model.SidebarModel.UnreadMessageCount %></div>
                    <%}%>
                </div>
                <div class="user-name">
                    <%= userSession.User.Name %>
                </div>
                <div class="arrow-down">
                    <i class="fas fa-chevron-down"></i>
                </div>
            </div>
        </a>
        <ul class="dropdown-menu pull-right" id="userDropdown" role="menu" aria-labelledby="dLabel" style="right: 0px;">
            <li>
                <div style="white-space: unset; padding: 0px;">
                    <div id="activity-popover-title">Deine Lernpunkte</div>
                    <div style="padding: 3px 20px 0px 20px;">
                        <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                    </div>
                </div>
            </li>
            <li class="divider"></li>
            <li>
                <a class="<%= Model.UserMenuActive(UserMenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>" style="display: flex;">
                    Deine Nachrichten
                    <% if (Model.SidebarModel.UnreadMessageCount != 0)
                       { %>
                        <svg class="badge" height="100" width="100">
                            <g>
                                <circle cx="16" cy="10" r="8" fill="#FF001F"/>
                                <text class="level-count" x="60%" text-anchor="middle" font-size="10" y="55%" dy=".34em" fill="white"><%= Model.SidebarModel.UnreadMessageCount %></text>
                            </g>
                        </svg>
                    <% } %>
                </a>
            </li>
            <li>
                <a class="<%= Model.UserMenuActive(UserMenuEntry.Network) %>" href="<%= Links.Network() %>">Dein Netzwerk</a>
            </li>
            <li>
                <a class="<%= Model.UserMenuActive(UserMenuEntry.UserDetail) %>" href="<%= Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>">Deine Profilseite</a>
            </li>
            <li class="divider"></li>
            <li>
                <a class="<%= Model.UserMenuActive(UserMenuEntry.UserSettings) %>" href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>">Konto-Einstellungen</a>
            </li>
            <% if (userSession.IsInstallationAdmin)
               { %>
                <li>
                    <a style="padding-bottom: 15px;" href="<%= Url.Action("Maintenance", "Maintenance") %>">Administrativ</a>
                </li>
                <li>
                    <a style="padding-bottom: 15px;" href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>">Adminrechte abgeben</a>
                </li>
            <% } %>
            <li>
                <a <% if (!userSession.IsInstallationAdmin)
                      { %> style="padding-bottom: 15px;" <% } %> href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser ? "true" : "" %>">Ausloggen</a>
            </li>

        </ul>
    </div>


<%
            }
  else
  {
%>
    <div class="login-register-container">
        <div class="btn memo-button link-btn login-btn" data-btn-login="true">            
            <i class="fa fa-sign-in"></i>
            Anmelden
        </div>
        <a href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>"><div class="btn memo-button register-btn">Kostenlos registrieren!</div></a>
    </div>

<%
            }
%>