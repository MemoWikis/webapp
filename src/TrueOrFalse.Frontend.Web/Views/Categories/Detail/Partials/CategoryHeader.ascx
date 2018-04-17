<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var isLongMenu = true; %>

<div id="CategoryHeader">   
    <div style="display:flex">
        <div class="BreadcrumbsMobile">
            <% Html.RenderPartial("/Views/Categories/Detail/Partials/BreadCrumbMobile.ascx", Model); %>
        </div>     
        <div id="mainMenuThemeCenteredMobile">
            <div class="menu-section">
                <div class="menu-container">
                    <div class="menu-item" style="height: 21px;">
                        <a id="mainMenuBtnKnowledge" style="margin-right: 3px; padding-right: 12px; border-radius: 0px" class="list-group-item menu-section primary-point <%: Model.MenuLeftModel.Active(MenuEntry.Knowledge)%>" href="<%= Links.Knowledge() %>">
                            <i id="mainMenuKnowledgeHeart" class="fa fa-heart fa-2x" style="color: #b13a48;"></i>
                            <span class="primary-point-text">Wissenszentrale</span>
                        </a>
                    </div>
                    <div class="menu-item" style="height: 21px;"><a id="MenuButton"><i class="fa fa-bars"></i></a></div>
                </div>
            </div>
            <div id="LongMenu" class="LongMenu">       
                <div class="menu-section-mobile" style="width:100%">
                    <a id="mainMenuBtnKnowledgeMobile" class="list-group-item menu-section primary-point " href="/Wissenszentrale" style="width:100%">
                        <i id="mainMenuKnowledgeHeartMobile" class="fa fa-heart fa-2x" style="color: #b13a48;"></i>
                        <span class="primary-point-text">Wissenszentrale
                        </span>
                    </a>
                </div>
                <% Html.RenderPartial("/Views/Categories/Navigation/CategoryNavigation.ascx", new CategoryNavigationModel()); %>
                <% if (isLongMenu)
                    { %>
                <div id="mainMenuQuestionsSetsCategories" class="menu-section secondary-section" style="width: 100%">
                    <a id="mainMenuBtnCategories" class="list-group-item cat <%= Model.MenuLeftModel.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>">
                        <i class="fa fa-search" aria-hidden="true"></i>Themen                
                        <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new"
                            onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; "
                            title="Neues Thema erstellen"></i>
                    </a>

                    <a id="mainMenuBtnSets" class="list-group-item set <%= Model.MenuLeftModel.Active(MenuEntry.QuestionSet) %>" href="<%= Links.SetsAll() %>">
                        <i class="fa fa-search" aria-hidden="true"></i>Lernsets                
                        <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 set-color add-new"
                            onclick="window.location = '<%= Url.Action("Create", "EditSet") %>'; return false; "
                            title="Neues Lernset erstellen"></i>
                    </a>

                    <a id="mainMenuBtnQuestions" class="list-group-item quest <%= Model.MenuLeftModel.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>">
                        <i class="fa fa-search" aria-hidden="true"></i>Fragen
                        <i id="mainMenuBtnQuestionCreate" class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new"
                            onclick="window.location = '<%= Links.CreateQuestion() %>'; return false; "
                            title="Frage erstellen"></i>
                    </a>
                </div>

                <div id="mainMenuGamesUsersMessages" class="menu-section secondary-section" style="width: 100%">
                    <a id="mainMenuBtnGames" class="<%= Model.MenuLeftModel.Active(MenuEntry.Play) %> list-group-item play" href="<%= Links.Games(Url) %>">Spielen
                        <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new"
                            onclick="window.location = '<%= Links.GameCreate() %>'; return false; "
                            title="Spiel erstellen"></i>
                    </a>

                    <a id="mainMenuBtnUsers" class="list-group-item users <%= Model.MenuLeftModel.Active(MenuEntry.Users) %>" href="<%= Links.Users() %>">Nutzer
                    </a>

                    <a id="mainMenuBtnMessages" class="list-group-item messages <%= Model.MenuLeftModel.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">Nachrichten
                        <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display: inline-block; position: relative; top: 1px;"><%= Model.MenuLeftModel.UnreadMessageCount %></span>
                    </a>

                    <% if (Model.IsInstallationAdmin)
                        { %>
                    <a class="list-group-item cat <%= Model.MenuLeftModel.Active(MenuEntry.Maintenance) %>" href="<%= Url.Action("Maintenance", "Maintenance") %>">Administrativ
                    </a>
                    <% } %>
                </div>
                <% } %>
            </div>
        </div>     
    </div>
      <div id="ManagementMobile">
        <div class="KnowledgeBarWrapper">
            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
        </div>
        <div class="Buttons">
            <div class="Button Pin" data-category-id="<%= Model.Id %>">
                <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                    <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                </a>
            </div>
            <div class="Button dropdown">
                <% var buttonId = Guid.NewGuid(); %>
                <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                    <i class="fa fa-ellipsis-v"></i>
                </a>
                <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>">
                    <% if (Model.AggregatedSetCount > 0)
                       { %>
                        <li><a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"><i class="fa fa-calendar"></i>&nbsp;Thema zum Termin lernen</a></li>
                        
                        <li><a href="<%= Links.GameCreateFromCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="game"><i class="fa fa-gamepad"></i>&nbsp;Spiel starten</a></li>
                    <% }
                    if(Model.IsOwnerOrAdmin){ %>
                        <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>
                    <% }
                    if (Model.IsInstallationAdmin){ %>
                        <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                    <% } %>
                </ul>
            </div>
        </div>
    </div>    
    <div id="HeadingSection">    
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category)) %>
        </div>
        <div id="HeadingContainer">
            <h1 style="margin-bottom: 0"><%= Model.Name %></h1>
            <div>
                <div class="greyed">
                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <%= Model.AggregatedSetCount %> Lernset<%= StringUtils.PluralSuffix(Model.AggregatedSetCount, "s") %> und <%= Model.AggregatedQuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.AggregatedQuestionCount, "n") %>
                    <% if(Model.IsInstallationAdmin) { %>
                        <a href="#" id="jsAdminStatistics">
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fa fa-user-secret" data-details="<%= Model.GetViewsPerDay() %>">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>
                        </a>
                    
                        <div id="last60DaysViews" style="display: none"></div>
                        
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div id="TabsBar">
        <div class="Tabs">
            <div id="TopicTab" class="Tab active">
                <a href="#">
                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" :  "Übersicht"%>
                </a>
            </div>
            <div id="LearningTab" class="Tab LoggedInOnly">
                <a href="#">
                    Lernen
                </a>
            </div>
            <div id="AnalyticsTab" class="Tab">
                <a href="#">
                    Analytics
                </a>
            </div>
        </div>
        <div id="Management">
            <div class="Border">
                
            </div>
            <div class="KnowledgeBarWrapper">
                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                <%--<div class="KnowledgeBarLegend">Dein Wissensstand</div>--%>
            </div>
            <div class="Buttons">
                <div class="Button Pin" data-category-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </a>
                </div>
                <div class="Button dropdown">
                    <% buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                        <% if (Model.AggregatedSetCount > 0)
                           { %>
                        <li><a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"><i class="fa fa-calendar"></i>&nbsp;Thema zum Termin lernen</a></li>
                        
                        <li><a href="<%= Links.GameCreateFromCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="game"><i class="fa fa-gamepad"></i>&nbsp;Spiel starten</a></li>
                        <% }
                           if (Model.IsOwnerOrAdmin)
                           { %>
                        <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a></li>
                        <% }
                           if (Model.IsInstallationAdmin)
                           { %>
                            <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                        <% } %>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>