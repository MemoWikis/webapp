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
 
    <a href="/" id="BreadcrumbLogoSmall" class="show-tooltip" data-placement="bottom" title="Zur Startseite" >
        
        <i class="fas fa-home"></i> <% if (!Model.TopNavMenu.IsWelcomePage && Model.TopNavMenu.BreadCrumbCategories.Count > 0){ %><i class="fa fa-chevron-right"></i><% } %>
        
    </a>

    <div id="BreadCrumbTrail" style="display:flex;">
        <ul id="Path" class="path">
        </ul>

    <%if(!Model.TopNavMenu.IsWelcomePage){ %>  
        <%if(false){ %>
            <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
        <% }else
           {
                if (true) { %>
                 <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
              <%}

                  var i = 0;
                  foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb) {
                      i++;
              %>
                    
                <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom"
                     <% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >  

                   <%if (breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last())){%>
                      <span style="display: flex; margin-left: 10px;"><a id="<%=i %>BreadCrumb" style="color:#003264;" href="<%= breadCrumbItem.Url %>">
                          <% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>Lernset: <%} %><%= breadCrumbItem.Text %>
                      </a></span>
                    <%} else {%>
                       <span style="display: inline-table; margin-left: 10px;"><a id="<%= i %>BreadCrumb" style="display:inline;"  href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a>
                          <i style="display: inline;" class="fa fa-chevron-right"></i>
                       </span>  
                    <%} %>
                </div>
            <% } %>        
        <%}%>
    <%} %>
    </div>
    <div id="StickyHeaderContainer">    
        <div class="input-group" id="StickyHeaderSearchBoxDiv" style="margin-right:3px">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="StickyHeaderSearchButton" onclick="SearchButtonClick()" style="height:34px; border: none;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0;margin:0; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <div id="KnowledgeImage" style="margin-right:0;"><a href="<%= Links.Knowledge() %>"><i style="margin-top:6px; font-size:32px;" class="fa fa-dot-circle-o"></i></a></div>
        <div id="BreadcrumbUserDropdownImage"  <%if(Model.IsLoggedIn){ %> style="margin-right: 15px; min-width: 29px;" <%} %>>
        <%if(Model.IsLoggedIn){ %>
           <a class="TextLinkWithIcon dropdown-toggle" id="dLabelBreadCrumb" data-toggle="dropdown" href="#">
            <img class="userImage" style="margin-top:21px; border:none; text-align:center;" src="<%= userImage%>" />
           </a>   
            <ul id="BreadcrumbUserDropdown" class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel" style="right:0; position: absolute; padding-right: 20px;">
                <li>
                    <a style="white-space: unset; padding: 0px;" href="<%= Links.Knowledge()%>">
                            <div id="activity-popover-title">Deine Lernpunkte</div>
                            <div style="padding: 3px 20px 0px 20px;">
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
                    <li><a  <% if (!userSession.IsInstallationAdmin)
                            {%>
                        style="padding-bottom: 15px;" <%}%> href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser() ? "true" : ""  %>">Ausloggen</a>  </li>
                    <% if (userSession.IsInstallationAdmin)
                        { %>
                    <li><a style="padding-bottom: 15px;" href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>">Adminrechte abgeben</a>  </li>
                    <% } %>
            </ul>
        <%}else{%>
             <a class="TextLinkWithIcon" href="#" data-btn-login="true"><i style="font-size:32px; color:grey; padding-top:19px;" class="fa fa-sign-in"></i></a>
        <%} %>
        </div>
        <div><a id="StickyMenuButton"><i class="fa fa-bars" style="font-size:inherit; margin-right:0px; color:grey"></i></a></div>
    </div>
</div> 