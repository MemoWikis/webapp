<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase user-row <%= Model.IsCurrentUser ? "loggedInUser"  : "" %>" data-questionSetId="<%= Model.Id %>" style="position: relative;">
    <div class="column-1" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="105"/>
    </div>
    
    <div class="column-2" style="width: 635px; height: 87px;">
        <div style="font-size:large;">
            <a href="<%= Model.UserLink(Url) %>"><%= Model.Name %></a>
            <button class="btn btn-default btn-xs" type="button" style="position: relative; top: -2px; left: 3px;">Folgen</button>
        </div>
        <div style="padding-top: 3px;">
            <span style="width: 53px; display: inline-block">Rang:<%= Model.Rank %></span> 
            Repuation:<%= Model.Reputation %>
        </div>
        <div>
            <%= Model.WishCountQuestions %> öffentliches Wunschwissen (davon <%= Model.WishCountSets %> Fragesätze)
        </div>
        
        <%= Model.DescriptionShort %>
        
        <div style="overflow: no-content; height: 20px; width: 130px; position: absolute; bottom:2px;">
            
            <%--<a data-toggle="modal" data-questionId="<%= Model.Id %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>--%>
            
            <% if(Model.IsCurrentUser){ %>
                <a href="<%= Url.Action(Links.UserSettings, Links.UserSettingsController) %>" class="show-tooltip" title="Einstellungen bearbeiten">
                    <img src="/Images/edit.png"/> 
                </a>
            <% } %>
        
            <% if(!Model.IsCurrentUser && Model.AllowsSupportiveLogin && Model.IsInstallationLogin){ %>
                <a href="<%= Links.UserLoginAs(Url, Model.Id) %>" class="show-tooltip" title="Anmelden als dieser Nutzer"> <i class="fa fa-share"></i> </a>
            <%} %>
            
        </div>        
    </div>
    
    <div class="column-3">
    </div>
</div>