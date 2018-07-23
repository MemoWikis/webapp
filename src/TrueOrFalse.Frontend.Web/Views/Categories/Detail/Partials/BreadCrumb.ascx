<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%  var userSession = new SessionUser();
    var user = userSession.User;
    var imageSetttings = new UserImageSettings(userSession.User.Id); %>

<div id="BreadcrumbContainer" class="container" style="display:flex; flex-wrap: nowrap;">                      
    <div id="BreadcrumbLogoSmall" style="display:none;">
        <img src="/Images/Logo/LogoSmall.png">
    </div>
    <div style="height: auto;" class="show-tooltip" data-placement="bottom"  title="Zur Startseite">
     <%if(!(Model.TopNavMenu.IsWelcomePage)){ %> 
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
     <%}%>
     </div>

<%if(!(Model.TopNavMenu.IsWelcomePage)){ %>  
    <%if(Model.TopNavMenu.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
    <% }else{
            if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb) { %>
             <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
          <%}
              
        var last = Model.TopNavMenu.BreadCrumb.Last();
       foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb) { %>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom" <% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >                                                                                          
           <%if (breadCrumbItem.Equals(last)){%>
              <span style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;"><a href="<%= breadCrumbItem.Url %>"><% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>Lernset: <%} %><%= breadCrumbItem.Text %></a></span>
            <%} else {%>
               <span style="display: inline-table; margin-left: 10px;"><a href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a>
                <i style="display: inline;" class="fa fa-chevron-right"></i>
               </span>  
            <%} %>
        </div>
    <% } %>        
    <%}%>
<%} %>
    <div id="StickyHeaderContainer">    
       
        <div class="input-group" id="StickyHeaderSearchBoxDiv" style="margin-right:25px">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="StickySearchButton" onclick="SearchButtonClick()" style="height:34px;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0px;margin:0px; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <div style="margin-right:25px"><i class="fa fa-dot-circle"></i></div>
        <div style="margin-right:25px">
           <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
            <img class="userImage" style="margin-top:13px; border:none;" src="<%= imageSetttings.GetUrl_30px_square(userSession.User).Url %>" />
           </a>   
           <ul class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel" style="right:auto;">
                <li>
                   <a style="white-space:unset; padding:0px;" href="<%= Links.Knowledge()%>">
                       <div id="activity-popover-title">Dein erreichtes Level</div>
                       <div style="padding:3px 20px; margin:0px;">
                        <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                       </div>
                   </a>
                </li>
                <li style="border: solid #707070 1px; margin-left:-1px; width:101%;">
                    <a style="padding:0px;" href="<%= Links.Messages(Url)%>">
                        <div style="white-space:normal; display:flex; padding:22px 0px 24px 22px;">
                            <i class="far fa-bell"></i>
                            <span class="badge dropdown-badge show-tooltip" title="<%= Model.SidebarModel.UnreadMessageCount%> ungelesene Nachrichten" <%if(Model.SidebarModel.UnreadMessageCount != 0){%> style="background-color:#FF001F;" <%}%>><%= Model.SidebarModel.UnreadMessageCount %></span>
                            <span >Du hast <%if(Model.SidebarModel.UnreadMessageCount != 0){ %> <b><%= Model.SidebarModel.UnreadMessageCount %> neue Nachrichten.</b><%}else{ %>keine neuen Benachrichtigungen<%} %></span>
                        </div>
                    </a>
                </li>
                <li><a  href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>"><i class="fa fa-user"></i> Deine Profilseite</a></li>
                <li><a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>"><i class="fa fa-wrench" title="Einstellungen"></i> Konto-Einstellungen</a></li>
                <li class="divider"></li>                 
                <li><a href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser() ? "true" : ""  %>"><i class="fa fa-power-off" title="Ausloggen"></i> Ausloggen</a>  </li>
                <% if (userSession.IsInstallationAdmin)
                    { %>
                    <li><a href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>"><i class="fa fa-power-off" title="Ausloggen"></i> Adminrechte abgeben</a>  </li>
                <% } %>
            </ul>
        </div>
        <div><a id="StickyMenuButton" style="margin-top:0px;"><i class="fa fa-bars" style="font-size:inherit; margin-right:0px;"></i></a></div>
    </div>

</div> 