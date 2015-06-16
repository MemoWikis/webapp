<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<NetworkModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Users/Users.css") %>
    <%= Scripts.Render("~/bundles/Users") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
     <div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
        <div class=" container">
            <div id="mobilePageHeader" class="">
                <h3 class="">
                    Nutzer
                </h3>
            </div>
        </div>
        <div class="MainFilterBarWrapper">
            <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
                <div class="btn-group">
                    <a class="btn btn-default disabled">.</a>
                </div>
            </div>
            <div class="container">
                <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">
                
                    <div class="btn-group">
                        <a href="<%= Url.Action("Users", "Users") %>" type="button" class="btn btn-default">
                            Alle (<span class="JS-Amount"><%= Model.TotalUsers %></span>)
                        </a>
                    </div>
                    <div class="btn-group active">
                        <a href="<%= Url.Action("Network", "Users") %>" type="button" class="btn btn-default">
                            Mein Netzwerk<span class="hidden-xxs"></span> 
                            (<span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span>/<%= Model.TotalFollowingMe %>)
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<div id="user-main">
    <% using (Html.BeginForm()) { %>

        <div class="boxtainer-outlined-tabs">
            <div class="boxtainer-header MobileHide">
                <ul class="nav nav-tabs">
                    <li>
                        <a href="<%= Url.Action("Users", "Users") %>" >Alle Nutzer (<%= Model.TotalUsers %>)</a>
                    </li>
                    <li class="active">
                        <a href="<%= Url.Action("Network", "Users") %>">
                            Mein Netzwerk 
                            (<span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span>/<%= Model.TotalFollowingMe %>)
                            <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                        </a>
                    </li>
                </ul>
            </div>
        
            <div class="boxtainer-content">
                <div class="search-section">
    
                    <h4 style="margin-bottom: 15px; margin-top: 0px;">
                        <span class="ColoredUnderline User">Du folgst <span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span> Nutzern</span>
                    </h4>
                    
                    <% if(!Model.UserIFollow.Any()){ %>
                        <div class="bs-callout bs-callout-info"  
                            style="margin-top: 0; margin-bottom: 10px;">
                            <h4>Noch folgst du niemanden</h4>
                            <p style="padding-top: 5px;">
                                Um Nutzern zu folgen, gehe zur
                                <a href="<%= Url.Action("Users", "Users") %>">"Alle Nutzer"</a> 
                                Seite und verwenden den "Folgen"-Button. <br/><br/>
                            </p>
                        </div>
                    <% } else { %>

                        <div style="clear: both;"> 
                            <% foreach(var row in Model.UserIFollow){
                                Html.RenderPartial("UserRow", row);
                            } %>
                        </div>
                    
                    <% } %>
                    
                    <h4 style="margin-bottom: 15px; margin-top: 0px;">
                        <span class="ColoredUnderline User">Dir folgen <%= Model.TotalFollowingMe %> Nutzer<%= Html.Plural(Model.TotalFollowingMe, "n") %></span>
                    </h4>
                    
                    <% if (!Model.UsersFollowingMe.Any()){ %>
                    
                        <div class="bs-callout bs-callout-info"  
                            style="margin-top: 0; margin-bottom: 10px;">
                            <h4>Noch folgt dir niemand</h4>
                            <p style="padding-top: 5px;">
                                Nutzer folgend dir, wenn du interessante Inhalte erstellst.<br/>
                                Folgen dir viele Nutzer, erhältst du Badges.
                            </p>
                        </div>
                    
                    <% } else { %>
                        <div style="clear: both;">
                            <% foreach(var row in Model.UsersFollowingMe){
                                Html.RenderPartial("UserRow", row);
                            } %>
                        </div>                    
                    <% } %>

                </div>
            </div>
        </div>
    <% } %>
</div>
</asp:Content>
