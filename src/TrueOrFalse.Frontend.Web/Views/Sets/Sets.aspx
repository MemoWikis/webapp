<%@ Page Title="Fragesätze" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<SetsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Sets.css") %>
    <%= Scripts.Render("~/bundles/Sets") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
     <div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
        <div class=" container">
            <div id="mobilePageHeader" class="">
                <h3 class="">
                    Fragesätze
                </h3>
                <a href="<%= Url.Action("Create", "EditSet") %>" class="btnCreateItem btn btn-success btn-sm">
                    <i class="fa fa-plus-circle"></i>
                    Fragesatz erstellen
                </a>
            </div>
            <nav id="mobilePageHeader2" class="navbar navbar-default" style="display: none;">
                <h4>
                    Fragesätze
                </h4>
            </nav>
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
                        <a  href="<%= Links.Sets() %>" type="button" class="btn btn-default">
                            Alle (<span class="JS-Amount"><%= Model.TotalSets %></span>)
                        </a>
                    </div>
                    <div class="btn-group <%= Model.ActiveTabWish ? "active" : "" %>">
                        <a  href="<%= Links.SetsWish() %>" type="button" class="btn btn-default">
                            Wunsch<span class="hidden-xxs">wissen</span> (<span class="tabWishKnowledgeCount JS-Amount"><%= Model.TotalWish %></span>)
                        </a>
                    </div>
                    <div id="MyQuestions" class="btn-group <%= Model.ActiveTabMine ? "active" : "" %>">
                        <a href="<%= Links.SetsMine() %>" type="button" class="btn btn-default">
                            Meine (<span class="JS-Amount"><%= Model.TotalMine %></span>)
                            <i class="fa fa-question-circle show-tooltip" title="Fragesätze, die von dir erstellt wurden." data-placement="right"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="set-main">
        <% using (Html.BeginForm()) { %>
        
             <div class="boxtainer-outlined-tabs">       
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs">
                        <li class="<%= Model.ActiveTabAll ? "active" : ""  %>">
                            <a href="<%= Links.Sets() %>">Alle Fragesätze (<%= Model.TotalSets %>)</a>
                        </li>
                        <li class="<%= Model.ActiveTabWish ? "active" : ""  %>">
                            <a href="<%= Links.SetsWish() %>">Mein Wunschwissen (<span class="tabWishKnowledgeCount"><%= Model.TotalWish %></span>)</a>
                        </li>
                        <li class="<%= Model.ActiveTabMine ? "active" : ""  %>">
                            <a href="<%= Links.SetsMine() %>">
                                Meine Fragesätze (<%= Model.TotalMine %>)
                                <i class="fa fa-question-circle show-tooltip" title="Fragesätze, die von dir erstellt wurden"></i>
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
                                <div class="pull-left form-group">
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
                            <% 
                                if(Model.AccessNotAllowed){
                                    Html.RenderPartial("RegisterOrLogin_Sets");
                                }else{ 
                                    foreach (var row in Model.Rows){
                                        Html.RenderPartial("SetRow", row);
                                    } 
                                }
                            %>
                        </div>
                        
                    </div>
                    
                    <% Html.RenderPartial("Pager", Model.Pager); %>
                </div>
            </div>
        <% } %>
    </div>
    
    <%
        if (!Model.AccessNotAllowed){
            Html.RenderPartial("Modals/DeleteSet");
        }
    %>
</asp:Content>
