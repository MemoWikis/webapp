﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
    
<% 
    var userSession = new SessionUser();
    var user = userSession.User;
%>
<div style="display: flex;">
    <div class="header-item">
        <a href="/Wissenszentrale" class="TextLinkWithIcon KnowledgeLink"><i style="font-size:32px;" class="fa fa-dot-circle"></i>
            <span class="primary-point-text">Wissenszentrale</span>
        </a>
    </div>
    <div class="header-item" id="Login">
  <%if (userSession.IsLoggedIn)
    {
        var imageSetttings = new UserImageSettings(userSession.User.Id);
%>
      <div style="display:flex; flex-direction:column;">
        <div style="display:flex;"> 
        <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="height:19px; padding:0px; padding-top:1px; line-height:13px; top:-7px !important; font-size:10px; left:14px; border:#003264 solid 2px; min-width:19px; display: inline-block; position: relative;"><%= Model.SidebarModel.UnreadMessageCount %></span>
         <img class="userImage" src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>" /> 
         <span id="header-level-display" class="level-display">
             <span style="display: inline-block; white-space: nowrap; margin-top:-8px;" class="" data-placement="bottom">
                 <span class="half-circle"><span style="margin-top:-5px;" class="level-count"><%= userSession.User.ActivityLevel %></span></span>
             </span>
        </span>
        </div>
        <div class="dropdown" style="display: inline-block;">
            <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
                <span class="userName TextSpan">Hallo <b><%= userSession.User.Name%></b><b class="caret"></b></span>
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
                            <span  class="badge show-tooltip" title="Ungelesene Nachrichten" style="height:19px; padding:0px; padding-top:1px; line-height:13px; font-size:10px; left:-13px; border:#003264 solid 2px; min-width:19px; display: inline-block; position: relative;"><%= Model.SidebarModel.UnreadMessageCount %></span>
                            <span>Du hast <%if(Model.SidebarModel.UnreadMessageCount != 0){ %> <b><%= Model.SidebarModel.UnreadMessageCount %> neue Nachrichten.</b><%}else{ %>keine neuen Benachrichtigungen<%} %></span>
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
        <a id="MenuButton" class="TextLinkWithIcon"><i style="font-size:32px;" class="fa fa-bars"></i>
        <span class="TextSpan">Menü</span></a>
    </div>
  </div>
