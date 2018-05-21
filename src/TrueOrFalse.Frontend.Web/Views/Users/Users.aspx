<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<UsersModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.PageTitle; %>
    <% if (Model.HasFiltersOrChangedOrder) { %>
        <meta name="robots" content="noindex">
    <% } else { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Model.CanonicalUrl %>">
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/Users") %>
    <%= Scripts.Render("~/bundles/js/Users") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
    <% Html.RenderPartial("HeaderMobile", Model.HeaderModel); %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<div id="user-main">
    <% using (Html.BeginForm()) { %>

        <div class="boxtainer-outlined-tabs">
            <% Html.RenderPartial("Header", Model.HeaderModel); %>
        
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
                                    <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch", formUrl="/Nutzer/Suche"}) %>
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" id="btnSearch"><i class="fa fa-search"></i></button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                                
                            <ul class="nav pull-right">
                                <li class="dropdown" id="menu1">
                                    <button class="dropdown-toggle btn btn-default btn-xs" data-toggle="dropdown" href="#menu1" rel="nofollow">
                                        Sortieren nach: <%= Model.OrderByLabel %>
                                        <b class="caret"></b>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byReputation" %>">
                                                <% if (Model.OrderBy.Reputation.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Reputation
                                            </a>
                                        </li>
                                        <li>
                                            <a href="<%= Request.Url.AbsolutePath + "?orderBy=byWishCount" %>">
                                                <% if (Model.OrderBy.WishCount.IsCurrent()){ %><i class="fa fa-check"></i> <% } %>  Wunschwissen
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                            </ul>
                            <div class="pull-right" style="font-size: 14px; margin-top: 0px; margin-right: 7px;">
                                <span id="resultCount2"><%= Model.TotalInResult %></span> Treffer
                            </div>
                        </div>
                    </div>
                </div>
                
                <% Html.Message(Model.Message); %>
                    
                <div id="JS-SearchResult">
                    <% Html.RenderPartial("UserSearchResult", Model.SearchResultModel); %>
                </div>

            </div>
        </div>
    <% } %>
</div>
</asp:Content>
