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

<div id="BreadCrumbContainer" class="container" style="display:flex;">
    <a href="/" id="BreadcrumbLogoSmall" class="show-tooltip" data-placement="bottom" title="Zur Startseite" style="display:none;">
        <img src="/Images/Logo/LogoSmall.png">
    </a>
    <div id="BreadCrumbTrail" style="display:flex; flex-wrap: wrap;">
    <div style="height: auto;" id="BreadcrumbHome" class="show-tooltip" data-placement="bottom"  title="Zur Startseite">
     <%if(!Model.TopNavMenu.IsWelcomePage){ %> 
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
     <%}%>
     </div>

<%if(!Model.TopNavMenu.IsWelcomePage){ %>  
    <%if(Model.TopNavMenu.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
    <% }else
      {
            if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb) { %>
             <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
          <%}
        
       var i = 0;
       foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb) {
            i++;%>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom" <% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >                                                                                          
           <%if (breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last())){%>
              <span style="display: flex; margin-left: 10px; color:#003264;"><a id="<%=i %>BreadCrumb" href="<%= breadCrumbItem.Url %>"><% if (Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb){%>Lernset: <%} %><%= breadCrumbItem.Text %></a></span>
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
        <div class="input-group" id="StickyHeaderSearchBoxDiv" style="margin-right:25px">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="StickySearchButton" onclick="SearchButtonClick()" style="height:34px;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0px;margin:0px; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <div id="KnowledgeImage" style="margin-right:0px;"><a href="<%= Links.Knowledge() %>"><i style="margin-top:6px; font-size:32px;" class="fa fa-dot-circle-o"></i></a></div>
        <div id="UserImage"  <%if(Model.IsLoggedIn){ %> style="margin-right:25px" <%} %>>
        <%if(Model.IsLoggedIn){ %>
           <a class="TextLinkWithIcon dropdown-toggle" id="dLabel" role="button" data-toggle="dropdown" data-target="#" href="#">
            <img class="userImage" style="margin-top:21px; border:none; text-align:center;" src="<%= userImage%>" />
           </a>   
            <ul id="BreadcrumbUserDropdown" class="dropdown-menu pull-right" role="menu" aria-labelledby="dLabel" style="right:0px;">
                <li>
                   <a style="white-space:unset; padding:0px;" href="<%= Links.Knowledge()%>">
                       <div id="activity-popover-title">Dein erreichtes Level</div>
                       <div style="padding:3px 20px 26px 20px;">
                        <% Html.RenderPartial("/Views/Shared/ActivityPopupContent.ascx"); %>
                       </div>
                   </a>
                </li>
                <li style="border: solid #707070 1px; margin-left:-1px; width:101%;">
                    <a style="padding:0px;" href="<%= Links.Messages(Url)%>">
                        <div style="white-space:normal; display:flex; padding:22px 0px 25px 22px;">
                            <% if (Model.SidebarModel.UnreadMessageCount != 0) { %> 
                                <i style="font-size:24px;" class="fa fa-bell"></i>
                                <span style="display:block;" class="badge dropdown-badge show-tooltip" title="<%= Model.SidebarModel.UnreadMessageCount%> ungelesene Nachrichten" style="background-color:#FF001F;" ><%= Model.SidebarModel.UnreadMessageCount %></span>
                                <span style="display:block; padding-left: 14px;">Du hast <b><%= Model.SidebarModel.UnreadMessageCount %> neue Nachrichten.</b></span>
                            <% } else { %>
                                <i style="font-size:24px; color: #979797;" class="fa fa-bell"></i>
                                <span style="display:block; color: #979797; padding-left: 14px;">Du hast keine neuen Benachrichtigungen</span>                                
                            <% } %>
                        </div>
                    </a>
                </li>
                <li><a style="padding-top: 14px;" href="<%=Url.Action(Links.UserAction, Links.UserController, new {name = userSession.User.Name, id = userSession.User.Id}) %>"> Deine Profilseite</a></li>
                <li><a style="padding-bottom: 5px;" href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>">Konto-Einstellungen</a></li>
                <li class="divider"></li>                 
                <li><a  <% if (!userSession.IsInstallationAdmin){%> style="padding-bottom: 15px;"<%}%> href="#" id="btn-logout" data-url="<%= Url.Action(Links.Logout, Links.WelcomeController) %>" data-is-facebook="<%= user.IsFacebookUser() ? "true" : ""  %>">Ausloggen</a>  </li>
                <% if (userSession.IsInstallationAdmin)
                    { %>
                    <li><a style="padding-bottom: 15px;" href="<%= Url.Action("RemoveAdminRights", Links.AccountController) %>"> Adminrechte abgeben</a>  </li>
                <% } %>
            </ul>
        <%}else{%>
             <a class="TextLinkWithIcon" href="#" data-btn-login="true"><i style="font-size:32px; color:grey; padding-top:19px;" class="fa fa-sign-in"></i></a>
        <%} %>
        </div>
        <div><a id="StickyMenuButton"><i class="fa fa-bars" style="font-size:inherit; margin-right:0px; color:grey"></i></a></div>
    </div>
</div> 