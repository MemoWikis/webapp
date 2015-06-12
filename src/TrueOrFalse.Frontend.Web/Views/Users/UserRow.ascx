<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="rowBase user-row <%= Model.IsCurrentUser ? "loggedInUser"  : "" %>" 
    data-UserId="<%= Model.Id %>" style="position: relative;">
    
    <div class="column-Image" style="line-height: 15px; font-size: 90%;">
        <img src="<%= Model.ImageUrl%>" width="105"/>
    </div>
    
    <div class="column-MainContent">
        <div class="MainContentUpper">
            <div style="font-size:large;">
                <a href="<%= Model.UserLink(Url) %>"><%= Model.Name %></a>
                
                <div style="float: right;">
                    
                    <%--<a data-toggle="modal" data-questionId="<%= Model.Id %>" href="#modalDelete"><img src="/Images/delete.png"/> </a>--%>
            
                    <% if(Model.IsCurrentUser){ %>
                        <a href="<%= Url.Action(Links.UserSettings, Links.UserSettingsController) %>" class="show-tooltip" title="Einstellungen bearbeiten">
                            <img src="/Images/edit.png"/> 
                        </a>
                    <% } %>
        
                    <% if(!Model.IsCurrentUser && Model.AllowsSupportiveLogin && Model.IsInstallationLogin){ %>
                        <a href="<%= Links.UserLoginAs(Url, Model.Id) %>" class="show-tooltip" title="Anmelden als dieser Nutzer" style="padding-left: 10px;"> <i class="fa fa-share" style="color: lightsteelblue"></i> </a>
                    <%} %>

                    <% if(!Model.IsCurrentUser){ %>
                        <button class="btn btn-default btn-sm" type="button" data-type="btn-follow"
                            style="min-width: 90px; position: relative; top: -2px; margin-left: 10px; <%= Html.CssHide(Model.DoIFollow) %> ">
                            <i class="fa fa-user-plus"></i>
                            Folgen
                        </button>
                    
                        <i class='fa fa-spinner fa-pulse' data-type="btnFollowSpinner" style="display:none"></i>
                
                        <button class="btn btn-warning btn-sm " type="button" data-type="btn-unfollow"
                            style="min-width: 90px; position: relative; top: -2px; margin-left: 10px; <%= Html.CssHide(!Model.DoIFollow) %>">
                            <i class="fa fa-user-times"></i>
                            Entfolgen
                        </button>
                    <% } %>
                </div>

            </div>
            <div style="padding-top: 3px; font-size: 110%">
                <span style="width: 60px; display: inline-block">Rang: <%= Model.Rank %></span> 
                Repuation: <%= Model.Reputation %>
            </div>
            <div style="padding-top: 5px;">
                <div>
                    Erstellt:
                    <%= Model.CreatedQuestions %> Fragen / <%= Model.CreatedSets %> Fragesätze
                </div>
                
                <div>
                    <% if (!Model.ShowWishKnowlede){ %>
                        <i class="fa fa-lock show-tooltip" data-html="true" style="color: lightslategrey"  title="Privates Wunschwissen. <br> Das Wunschwissen ist nicht einsehbar."></i>
                    <% } %>
                    Wunschwissen: <%= Model.WishCountQuestions %> Fragen / <%= Model.WishCountSets %> Fragesätze
                </div>

            </div>
        
            <%= Model.DescriptionShort %>
        </div>
        
    </div>
</div>