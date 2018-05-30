<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<SetsModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.PageTitle; %>
    <% if (Model.HasFiltersOrChangedOrder) { %>
        <meta name="robots" content="noindex">
    <% } else { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Model.CanonicalUrl %>">
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Sets.css") %>
    <%= Scripts.Render("~/bundles/Sets") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Lernsets", Url = "/Fragesaetze", ImageUrl = "fa-search", ToolTipText = "Lernsets"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
     <div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
        <div class=" container">
            <div id="mobilePageHeader" class="">
                <h3 class="">
                    Lernsets
                </h3>
                <a href="<%= Url.Action("Create", "EditSet") %>" class="btnCreateItem btn btn-success btn-sm">
                    <i class="fa fa-plus-circle"></i>
                    Lernset erstellen
                </a>
            </div>
            <nav id="mobilePageHeader2" class="navbar navbar-default" style="display: none;">
                <h4>
                    Lernsets
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
                
                    <div class="btn-group <%= Model.ActiveTabAll ? "active" : "" %> JS-All">
                        <a  href="<%= Links.SetsAll() %>" type="button" class="btn btn-default">
                            Alle (<span class="JS-Amount"><%= Model.TotalSetsInSystem %></span>)
                        </a>
                    </div>
                    <div class="btn-group <%= Model.ActiveTabWish ? "active" : "" %> JS-Wish">
                        <a  href="<%= Links.SetsWish() %>" type="button" class="btn btn-default">
                            Wunsch<span class="hidden-xxs">wissen</span> (<span class="tabWishKnowledgeCount JS-Amount"><%= Model.TotalWish %></span>)
                            <i class="fa fa-question-circle show-tooltip" title="Lernsets, die du dir merken möchtest." data-placement="right"></i>
                        </a>
                    </div>
                    <div id="MyQuestions" class="btn-group <%= Model.ActiveTabMine ? "active" : "" %> JS-Mine">
                        <a href="<%= Links.SetsMine() %>" type="button" class="btn btn-default">
                            Meine (<span class="JS-Amount"><%= Model.TotalMine %></span>)
                            <i class="fa fa-question-circle show-tooltip" title="Lernsets, die von dir zusammengestellt wurden" data-placement="right"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <% using (Html.BeginForm()) { %>
        
           <script runat="server" type="text/C#">
                public string GetTabText(bool getText, int totalInSystem, int totalInResult)
                {
                    if (!getText || totalInSystem == totalInResult)
                        return "";

                    return totalInResult + " von ";
                }
            </script>
        
        <div id="set-main">
             <div class="boxtainer-outlined-tabs">       
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs JS-Tabs">
                        <li class="<%= Model.ActiveTabAll ? "active" : ""  %> JS-All">
                            <a href="<%= Links.SetsAll() %>">
                                <% string von = GetTabText(Model.ActiveTabAll, Model.TotalSetsInSystem, Model.TotalSetsInResult); %> 
                                Alle Lernsets (<span class="JS-Amount"><%= von + Model.TotalSetsInSystem %></span>)
                            </a>
                        </li>
                        <li class="<%= Model.ActiveTabWish ? "active" : ""  %> JS-Wish">
                            <a href="<%= Links.SetsWish() %>">
                                <% von = GetTabText(Model.ActiveTabWish, Model.TotalWish, Model.TotalSetsInResult); %>
                                <i class="fa fa-heart" style="color:#b13a48;"></i>
                                Mein Wunschwissen (<span class="tabWishKnowledgeCount JS-Amount"><%= von + Model.TotalWish %></span>)
                                <i class="fa fa-question-circle show-tooltip" title="Lernsets, die du dir merken möchtest." data-placement="right"></i>
                            </a>
                        </li>
                        <li class="<%= Model.ActiveTabMine ? "active" : ""  %> JS-Mine">
                            <a href="<%= Links.SetsMine() %>">
                                <% von = GetTabText(Model.ActiveTabMine, Model.TotalMine, Model.TotalSetsInResult); %>
                                Meine Lernsets (<span class="JS-Amount"><%= von + Model.TotalMine %></span>)
                                <i class="fa fa-question-circle show-tooltip" title="Lernsets, die von dir zusammengestellt wurden"></i>
                            </a>
                        </li>
                    </ul>
                    <div style="float: right; position: absolute; right: 0; top: 5px;">
                        <a href="<%= Url.Action("Create", "EditSet") %>" class="btn btn-success btn-sm">
                            <i class="fa fa-plus-circle"></i> Lernset erstellen
                        </a>
                    </div>
                </div>
                         
                <div class="boxtainer-content">
                    <div class="search-section">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
<%--                                <% if(!String.IsNullOrEmpty(Model.Suggestion)){ %> 
                                        <div style="padding-bottom: 10px; font-size: large">
                                            Oder suchst du: 
                                            <a href="<%= Model.SearchUrl + Model.Suggestion %>">
                                                <%= Model.Suggestion %>
                                            </a> ?
                                        </div>
                                    <% } %>--%>
                                
                                    <div class="input-group">
                                        <%: Html.TextBoxFor(model => model.SearchTerm, new {@class="form-control", placeholder="Beginne zu tippen, um Lernsets zu finden", id="txtSearch", formUrl=Model.SearchUrl }) %>
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
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byBestMatch" %>">
                                                    <% if (Model.OrderBy.BestMatch.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Beste Treffer
                                                </a>
                                            </li>
                                            <li>
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byCreationDate" %>">
                                                    <% if (Model.OrderBy.CreationDate.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Datum erstellt
                                                </a>
                                            </li>
                                            <li>
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byValuationsCount" %>">
                                                    <% if (Model.OrderBy.ValuationsCount.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Anzahl Gemerkt
                                                </a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                    
                        </div>
                    </div>
                            
                    <div id="JS-SearchResult">
                        <% Html.RenderPartial("SetsSearchResult", Model.SearchResultModel); %>
                    </div>
              
                </div>
            <% } %>
        </div>
    </div>
    
    <%
        if (!Model.AccessNotAllowed){
            Html.RenderPartial("Modals/DeleteSet");
        }
    %>
</asp:Content>
