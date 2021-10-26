<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="Markdig.Helpers" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var userSession = new SessionUser();
   var user = userSession.User;
   string userImage = "";
   string toolTipToHomepage = "Zur Startseite";

   if (Model.IsLoggedIn)
   {
       var imageSetttings = new UserImageSettings(userSession.User.Id);
       userImage = imageSetttings.GetUrl_30px_square(userSession.User).Url;
       toolTipToHomepage = "Zu deinem Wiki";
   }
%>

<div id="BreadCrumbContainer" class="container">
    <span>
        <a href="/" id="BreadcrumbLogoSmall" class="show-tooltip" data-placement="bottom" title="<%= toolTipToHomepage %>">
            <i class="fas fa-home"></i>
        </a>
    </span>
    <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbFirstChevron.ascx", Model) %>

    <div id="BreadCrumbTrail">
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbTrail.ascx", Model) %>
    </div>
    <div id="StickyHeaderContainer">
        <div id="StickySearch">
            <div class="searchButton" :class="{ 'showSearch' : showSearch }" @click="showSearch = !showSearch" v-cloak>
                <i :class="[ showSearch ? 'fas fa-times' : 'fa fa-search']" aria-hidden="true"></i>
            </div>
            <div class="StickySearchContainer" :class="{ 'showSearch' : showSearch }" v-cloak>
                <search-component v-on:select-item="openUrl" :search-type="searchType" id="StickySearchComponent" :show-search="showSearch"/>
            </div>
        </div>
        <div id="BreadcrumbUserDropdownImage">
            <% if (Model.IsLoggedIn)
               { %>
                <a class="TextLinkWithIcon dropdown-toggle" id="dLabelBreadCrumb" data-toggle="dropdown" href="#">
                    <div id="dLabelContainer">
                        <img class="userImage" src="<%= userImage %>"/>
                        <% if (Model.SidebarModel.UnreadMessageCount != 0)
                           { %>
                            <div class="badge-counter"></div>
                        <% } %>
                        <div class="arrow-down">
                            <i class="fas fa-chevron-down"></i>
                        </div>
                    </div>
                </a>
                <ul id="BreadcrumbUserDropdown" class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel" style="right: 0; position: absolute; width: 220px;">
                    <li>
                        <a id="UserProgressContainer" href="<%= Links.Knowledge() %>">
                            <div id="activity-popover-title">Deine Lernpunkte</div>
                            <div id="activity-popover-container">
                                <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a class="messages,<%= Model.UserMenuActive(UserMenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>" style="display: flex;">
                            Deine Nachrichten
                            <% if (Model.SidebarModel.UnreadMessageCount != 0)
                               { %>
                                <svg class="badge">
                                    <g>
                                        <circle cx="16" cy="11" r="8" fill="#FF001F"/>
                                        <text class="level-count" x="59%" text-anchor="middle" font-size="10" y="59%" dy=".34em" fill="white"><%= Model.SidebarModel.UnreadMessageCount %></text>
                                    </g>
                                </svg>
                            <% } %>
                        </a>

                    </li>
                    <li>
                        <a class="<%= Model.UserMenuActive(UserMenuEntry.Network) %>" href="<%= Links.Network() %>">Deine Netzwerk</a>
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
            <% }
              else
              { %>
                <div class="breadcrumb-login-register-container">
                    <a class="TextLinkWithIcon" href="#" data-btn-login="true">
                        <i class="fa fa-sign-in"></i>
                    </a>
                    <a id="StickyRegisterBtn" class="hidden-xs hidden-sm" href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>">
                        <div class="btn memo-button register-btn">Kostenlos registrieren!</div>
                    </a>
                </div>
            <% } %>
        </div>
    </div>
</div>