<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%  var userSession = new SessionUser();
    var user = userSession.User;
    string userImage = "";

    if (Model.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
        userImage = imageSetttings.GetUrl_30px_square(userSession.User).Url;
    }
%>

<div id="BreadCrumbContainer" class="container">
    <span>
        <a href="/" id="BreadcrumbLogoSmall" class="show-tooltip" data-placement="bottom" title="Zur Startseite" >
            <i class="fas fa-home"></i>
        </a>
    </span>
    <% if (!Model.TopNavMenu.IsWelcomePage && Model.TopNavMenu.BreadCrumbCategories != null && Model.TopNavMenu.BreadCrumbCategories.Items.Count > 0){ %>
        <span>
            <i class="fa fa-chevron-right"></i>
        </span>
    <% } %>


    <div id="BreadCrumbTrail">
        <ul id="Path" class="path dropdown-menu" aria-labelledby="BreadCrumbDropdown"></ul>

    <%if(!Model.TopNavMenu.IsWelcomePage){ %>  
        <% if ((Model.TopNavMenu.IsCategoryBreadCrumb || Model.TopNavMenu.QuestionBreadCrumb) && Model.TopNavMenu.BreadCrumbCategories != null){ %>
            <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
        <% } %>

            <% var i = 0;
               foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb) {
                   i++;
            %> 
                    <div>  

                       <%if (breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last())){%>
                          <span style="display: flex; margin-left: 10px;"><a class="show-tooltip" id="<%=i %>BreadCrumb" href="<%= breadCrumbItem.Url %>" title="<%= breadCrumbItem.ToolTipText%>" data-placement="bottom">
                              <%= breadCrumbItem.Text %>
                          </a></span>
                        <%} else {%>
                           <span style="display: inline-table; margin-left: 10px;"><a id="<%= i %>BreadCrumb" class="show-tooltip"  href="<%= breadCrumbItem.Url %>" title="<%= breadCrumbItem.ToolTipText%>" data-placement="bottom"><%= breadCrumbItem.Text %></a>
                           <%if (!breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last()))
                             { %>
                                 <i style="display: inline;" class="fa fa-chevron-right"></i>
                           <% } %>
                           </span>  
                        <%} %>
                    </div>
            <% } %>        
    <%} %>
    </div>
    <div id="StickyHeaderContainer">    
        <div class="input-group" id="StickyHeaderSearchBoxDiv" style="margin-right:3px">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="StickyHeaderSearchButton" onclick="SearchButtonClick()" style="height:34px; border: none;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0;margin:0; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <div id="BreadcrumbUserDropdownImage"  <%if(Model.IsLoggedIn){ %> style="margin-right: 15px; min-width: 29px;" <%} %>>
        <%if(Model.IsLoggedIn){ %>
           <a class="TextLinkWithIcon dropdown-toggle" id="dLabelBreadCrumb" data-toggle="dropdown" href="#">
            <img class="userImage" style="margin-top:21px; border:none; text-align:center;" src="<%= userImage%>" />
           </a>   
            <ul id="BreadcrumbUserDropdown" class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel" style="right:0; position: absolute; width: 220px;">
                <li>
                    <a id="UserProgressContainer" href="<%= Links.Knowledge()%>">
                            <div id="activity-popover-title">Deine Lernpunkte</div>
                            <div id="activity-popover-container">
                                <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a class="messages,<%= Model.UserMenuActive(UserMenuEntry.Messages) %>" href="<%=Links.Messages(Url) %>"  style="display: flex;">Deine Nachrichten                        
                            <% if (Model.SidebarModel.UnreadMessageCount != 0) { %>
                                <svg class="badge">
                                  <g>
                                    <circle cx="16" cy="11" r="8" fill="#FF001F"/>
                                    <text class="level-count" x="59%" text-anchor="middle" font-size="10" y="59%" dy=".34em" fill="white"><%= Model.SidebarModel.UnreadMessageCount %></text>
                                  </g>
                                </svg>
                            <% } %>
                        </a>
                       
                    </li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.Network) %>" href="<%=Links.Network() %>">Deine Netzwerk</a></li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.UserDetail) %>" href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>">Deine Profilseite</a></li>
                    <li class="divider"></li>
                    <li><a class="<%= Model.UserMenuActive(UserMenuEntry.UserSettings) %>" href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>">Konto-Einstellungen</a></li>
                <% if (userSession.IsInstallationAdmin)  { %>
                    <li><a style="padding-bottom: 15px;" href="<%= Url.Action("Maintenance", "Maintenance") %>">Administrativ</a></li>
                    <li><a style="padding-bottom: 15px;" href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>">Adminrechte abgeben</a></li>
                <% } %>
                <li><a  <% if (!userSession.IsInstallationAdmin) {%> style="padding-bottom: 15px;" <%}%> href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser ? "true" : ""  %>">Ausloggen</a>  </li>

            </ul>
        <%}else{%>
             <a class="TextLinkWithIcon" href="#" data-btn-login="true"><i style="font-size:32px; color:grey; padding-top:19px;" class="fa fa-sign-in"></i></a>
        <%} %>
        </div>
    </div>
</div> 