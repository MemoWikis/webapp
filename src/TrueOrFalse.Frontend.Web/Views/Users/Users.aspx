<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<UsersModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

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
                
                    <div class="btn-group  <%= Model.ActiveTabAll ? "active" : ""  %>">
                        <a  href="#" type="button" class="btn btn-default">
                            Alle (<span class="JS-Amount"><%= Model.TotalUsers %></span>)
                        </a>
                    </div>
                    <div class="btn-group <%= Model.ActiveTabFollowed ? "active" : "" %>">
                        <a  href="#" type="button" class="btn btn-default">
                            Mein Netzwerk<span class="hidden-xxs"></span> (<span class="JS-Amount"> ? </span>)
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
                    <li class="active"><a href="#home" >Alle Nutzer (<%= Model.TotalUsers %>)</a></li>
                    <li>
                        <a href="#profile">
                            Mein Netzwerk <span id="tabWishKnowledgeCount">(<%= Model.TotalMine %>)</span> <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
                        </a>
                    </li>
                </ul>
            </div>
        
            <div class="boxtainer-content">
                <div class="search-section">
                    <div class="row">
                        <div class="col-md-6">                    
                            <div class="pull-left form-group search-container">
                                <% if(!String.IsNullOrEmpty(Model.Suggestion)){ %> 
                                    <div style="padding-bottom: 10px; font-size: large">
                                        Oder suchst du: 
                                        <a href="<%= "/Nutzer/Suche/" + Model.Suggestion %>">
                                            <%= Model.Suggestion %>
                                        </a> ?
                                    </div>
                                <% } %>
                                
                                <div class="input-group">
                                    <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch"}) %>
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" id="btnSearch"><i class="fa fa-search"></i></button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                                
                            <ul class="nav pull-right">
                                <li class="dropdown" id="menu1">
                                    <button class="dropdown-toggle btn btn-default btn-xs" data-toggle="dropdown" href="#menu1">
                                        Sortieren nach: <%= Model.OrderByLabel %>
                                        <b class="caret"></b>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byReputation" %>">
                                                <% if (Model.OrderBy.Reputation.IsCurrent()){ %><i class="icon-ok"></i> <% } %> Reputation
                                            </a>
                                        </li>
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byWishCount" %>">
                                                <% if (Model.OrderBy.WishCount.IsCurrent()){ %><i class="icon-ok"></i> <% } %>  Wunschwissen
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>

                        </div>
                    </div>
                        
                    <% Html.Message(Model.Message); %>

                    <div style="clear: both;">
                        <% foreach(var row in Model.Rows){
                            Html.RenderPartial("UserRow", row);
                        } %>
                    </div>

                </div>

                <% Html.RenderPartial("Pager", Model.Pager); %>
            </div>
        </div>
    <% } %>
</div>
</asp:Content>
