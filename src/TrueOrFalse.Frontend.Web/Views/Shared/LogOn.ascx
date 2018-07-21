<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<% 
    var userSession = new SessionUser();
    var user = userSession.User;
%>
<div style="display: flex;">
    <div class="header-item" style="margin-top:-5px;">
        <div class="input-group" id="SmallHeaderSearchBoxDiv">
            <input type="text" class="form-control" placeholder="Suche" id="SmallHeaderSearchBox">
            <div class="input-group-btn" style="height:34px;">
              <button class="btn btn-default" id="SearchButton" style="padding-top:0px; font-size:29px;" onclick="SearchButtonClick()" type="submit"><i class="fa fa-search" style="color: white;" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
    <div class="header-item">
        <a href="/Wissenszentrale" class="TextLinkWithIcon KnowledgeLink"><i style="font-size:32px;" class="fa fa-dot-circle"></i>
            <span class="primary-point-text TextSpan">Wissenszentrale</span>
        </a>
    </div>
    <div class="header-item" id="Login">
  <%if (userSession.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
%>
      <div style="display:flex; flex-direction:column;">
        <div class="dropdown" style="display: inline-block;">

          <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
              <div style="display: flex;">
                  <span id="badgeNewMessages" class="badge show-tooltip"  data-placement="bottom" title="<%= Model.SidebarModel.UnreadMessageCount%> Ungelesene Nachrichten" <%if(Model.SidebarModel.UnreadMessageCount != 0){ %>style="background-color:#FF001F"<%}%>> <%= Model.SidebarModel.UnreadMessageCount %></span>
                  <img class="userImage" src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>" />
                  <span id="header-level-display" class="level-display">
                      <span style="display: inline-block; white-space: nowrap; margin-top: -8px;" class="" data-placement="bottom">
                          <span class="half-circle"><span style="margin-top: -5px;" class="level-count"><%= userSession.User.ActivityLevel %></span></span>
                      </span>
                  </span>
              </div>
              <span class="userName TextSpan" style="font-weight:normal;">Hallo <b><%= userSession.User.Name%></b><b class="caret"></b></span>
          </a> 
            <ul class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel">
                <li>
                   <a style="white-space:unset; padding:0px;" href="<%= Links.Knowledge()%>">
                       <div id="activity-popover-title">Dein erreichtes Level</div>
                       <div style="padding:3px 20px;">
                        <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                       </div>
                   </a>
                </li>
                <li style="border: solid #707070 1px; margin-left:-1px; width:101%;">
                    <a style="padding:0px;" href="<%= Links.Messages(Url)%>">
                        <div style="white-space:normal; display:flex; padding:22px 0px 24px 22px;">
                            <i class="far fa-bell"></i>
                            <span style="display:block;" class="badge dropdown-badge show-tooltip" title="<%= Model.SidebarModel.UnreadMessageCount%> ungelesene Nachrichten" <%if(Model.SidebarModel.UnreadMessageCount != 0){%> style="background-color:#FF001F;" <%}%>><%= Model.SidebarModel.UnreadMessageCount %></span>
                            <span style="display:block;">Du hast <%if(Model.SidebarModel.UnreadMessageCount != 0){ %> <b><%= Model.SidebarModel.UnreadMessageCount %> neue Nachrichten.</b><%}else{ %>keine neuen Benachrichtigungen<%} %></span>
                        </div>
                    </a>
                </li>
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
        <a class="TextLinkWithIcon" href="#" data-btn-login="true" title="Einloggen"><i style="font-size:32px;" class="fa fa-sign-in"></i>
        <span class="TextSpan">Einloggen</span></a>
<%
    }
%>    
    </div>
    <div class="header-item" style="margin-right:0px;">
        <a id="MenuButton" class="TextLinkWithIcon"><i class="fa fa-bars"></i>
        <span class="TextSpan">Menü</span></a>
        <%Html.RenderPartial("/Views/Shared/MainMenuThemeCentered.ascx", Model.SidebarModel); %>
    </div>
  </div>
