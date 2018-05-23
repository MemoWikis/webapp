<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoriesModel>" %>
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
    <%= Styles.Render("~/Views/Categories/Categories.css") %>
    <%= Scripts.Render("~/bundles/Categories") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Themen", Url = "/Kategorien", ImageUrl = "fa-search"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SubHeader" runat="server">
     <div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
        <div class=" container">
            <div id="mobilePageHeader" class="">
                <h3 class="">
                    Themen
                </h3>
                <a href="<%= Url.Action("Create", "EditCategory") %>" class="btnCreateItem btn btn-success btn-sm">
                    <i class="fa fa-plus-circle"></i>
                    Thema erstellen
                </a>
            </div>
            <nav id="mobilePageHeader2" class="navbar navbar-default" style="display: none;">
                <h4>
                    Themen
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
                    <div class="btn-group  <%= Model.ActiveTabAll ? "active" : "" %> JS-All">
                        <a  href="#" type="button" class="btn btn-default">
                            <% string von = GetTabText(Model.ActiveTabAll, Model.TotalCategoriesInSystem, Model.TotalCategoriesInResult); %> 
                            Alle (<span class="JS-Amount"><%= von + Model.TotalCategoriesInSystem %></span>)
                        </a>
                    </div>
                    <div class="btn-group <%= Model.ActiveTabWish ? "active" : "" %> JS-Mine">
                        <a  href="#" type="button" class="btn btn-default">
                            <% von = GetTabText(Model.ActiveTabWish, Model.TotalWish, Model.TotalCategoriesInResult); %>
                            Wunsch<span class="hidden-xxs">wissen</span> (<span class="JS-Amount tabWishKnowledgeCount"><%= von + Model.TotalWish %></span>)
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="category-main">
       
        <% Html.Message(Model.Message); %>
        
        <% using (Html.BeginForm()) { %>
        
           <script runat="server" type="text/C#">
                public string GetTabText(bool getText, int totalInSystem, int totalInResult)
                {
                    if (!getText || totalInSystem == totalInResult)
                        return "";

                    return totalInResult + " von ";
                }
            </script>

            <div class="boxtainer-outlined-tabs">
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs JS-Tabs">
                        <li class="<%= Model.ActiveTabAll ? "active" : ""  %> JS-All">
                            <a href="<%= Links.CategoriesAll() %>" >
                                <% string von = GetTabText(Model.ActiveTabAll, Model.TotalCategoriesInSystem, Model.TotalCategoriesInResult); %> 
                                Alle Themen (<span class="JS-Amount"><%= von + Model.TotalCategoriesInSystem %></span>)
                            </a>
                        </li>
                        <li class="<%= Model.ActiveTabWish ? "active" : ""  %> JS-Wish">
                            <a href="<%= Links.CategoriesWish() %>">
                                <% von = GetTabText(Model.ActiveTabWish, Model.TotalWish, Model.TotalCategoriesInResult); %>
                                <i class="fa fa-heart" style="color:#b13a48;"></i>
                                Mein Wunsch<span class="hidden-xxs">wissen</span> (<span class="JS-Amount tabWishKnowledgeCount"><%= von + Model.TotalWish %></span>)
                                <i class="fa fa-question-circle show-tooltip" id="tabInfoMyKnowledge" title="Themen, die du dir merken möchtest." data-placement="right"></i>
                            </a>
                        </li>
                    </ul>
                    <div class="" style="float: right; position: absolute; right: 0; top: 5px;">
                        <a href="<%= Url.Action("Create", "EditCategory") %>" class="btn btn-success btn-sm">
                            <i class="fa fa-plus-circle"></i>  Thema erstellen
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
                                            <a href="<%= "/Kategorien/Suche/" + Model.Suggestion %>">
                                                <%= Model.Suggestion %>
                                            </a> ?
                                        </div>
                                    <% } %>--%>

                                    <div class="input-group">
                                        <%: Html.TextBoxFor(model => model.SearchTerm, 
                                                new {@class="form-control", placeholder="Beginne zu tippen, um Themen zu finden", id="txtSearch", formurl = Model.SearchUrl}) %>
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
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byQuestions" %>">
                                                    <% if (Model.OrderBy.QuestionCount.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Anzahl Fragen
                                                </a>
                                            </li>
                                            <li>
                                                <a href="<%= Request.Url.AbsolutePath + "?orderBy=byDate" %>">
                                                    <% if (Model.OrderBy.CreationDate.IsCurrent()){ %><i class="fa fa-check"></i> <% } %> Datum erstellt
                                                </a>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                                <div class="pull-right" style="font-size: 14px; margin-top: 0px; margin-right: 7px;">
                                    <span id="resultCount2"><%= Model.TotalCategoriesInResult %></span> Treffer
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div id="JS-SearchResult">
                        <% Html.RenderPartial("CategoriesSearchResult", Model.SearchResultModel); %>
                    </div>
                </div>
            </div>
            
        <% } %>
        
    </div>
    
    <% Html.RenderPartial("Modals/ModalDeleteCategory"); %>

</asp:Content>
