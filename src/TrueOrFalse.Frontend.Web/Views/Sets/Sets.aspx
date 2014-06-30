<%@ Page Title="Fragesätze" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<SetsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Sets.css") %>
    <%= Scripts.Render("~/bundles/Sets") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="set-main">
        <% using (Html.BeginForm()) { %>
        
             <div class="boxtainer-outlined-tabs">         
                <div class="boxtainer-header">
                    <ul class="nav nav-tabs">
                        <li class="<%= Model.ActiveTabAll ? "active" : ""  %>">
                            <a href="<%= Links.Sets(Url) %>" >Alle Fragesätze (<%= Model.TotalSets %>)</a>
                        </li>
                        <li class="<%= Model.ActiveTabWish ? "active" : ""  %>">
                            <a href="<%= Links.SetsWish(Url) %>">Mein Wunschwissen (<span class="tabWishKnowledgeCount"><%= Model.TotalWish %></span>)</a>
                        </li>
                        <li class="<%= Model.ActiveTabMine ? "active" : ""  %>">
                            <a href="<%= Links.SetsMine(Url) %>">
                                Meine Fragesätze (<%= Model.TotalMine %>)
                                <i class="fa fa-question-circle show-tooltip" title="Fragesätze die von Dir erstellt wurden"></i>
                            </a>
                        </li>
                    </ul>
                    <div style="float: right; position: absolute; right: 0; top: 5px;">
                        <a href="<%= Url.Action("Create", "EditSet") %>" class="btn btn-default">
                            <i class="fa fa-plus-circle"></i> Fragesatz erstellen
                        </a>
                    </div>
                </div>
                         
                <div class="boxtainer-content">
                    <div class="search-section">
                        <div class="row">
                            <div class="col-md-6">
                                <% if(!String.IsNullOrEmpty(Model.Suggestion)){ %> 
                                    <div style="padding-bottom: 10px; font-size: large">
                                        Oder suchst du: 
                                        <a href="<%= Model.SearchUrl + Model.Suggestion %>">
                                            <%= Model.Suggestion %>
                                        </a> ?
                                    </div>
                                <% } %>                                
                                
                                <div class="input-group">
                                    <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", id="txtSearch", formUrl=Model.SearchUrl}) %>
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" id="btnSearch"><i class="fa fa-search"></i></button>
                                    </span>
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
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byValuationsCount" %>">
                                                    <% if (Model.OrderBy.ValuationsCount.IsCurrent()){ %><i class="icon-ok"></i> <% } %> Anzahl Gemerkt
                                                </a>
                                            </li>
                                            <li>
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byValuationsAvg" %>">
                                                    <% if (Model.OrderBy.ValuationsAvg.IsCurrent())
                                                       { %><i class="icon-ok"></i> <% } %>  Gemerkt &#216; Wichtigkeit
                                                </a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>

                            </div>

                        </div>
        
                        <div style="clear:both;">
                            <% foreach(var row in Model.Rows){
                                Html.RenderPartial("SetRow", row);
                            } %>
                        </div>
                        <% Html.RenderPartial("Pager", Model.Pager); %>
                    </div>
                </div>
            </div>
    <% } %>
    </div>
    
    <% Html.RenderPartial("Modals/DeleteSet"); %>
</asp:Content>
