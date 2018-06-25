﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%  var isLongMenu = true; %>

<div style="display: flex;">
    <div id="mainMenuThemeCenteredMobile">
        <div id="LongMenu" class="LongMenu">
            <% Html.RenderPartial("/Views/Categories/Navigation/CategoryNavigation.ascx", Model.categoryNavigationModel); %>
            <% if (isLongMenu)
                { %>
            <div id="mainMenuQuestionsSetsCategories" class="menu-section secondary-section" style="width: 100%">
                <a id="mainMenuBtnCategories" class="list-group-item cat <%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>">
                    <i class="fa fa-search" aria-hidden="true"></i>Themen                
                                            <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new" onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; " title="Neues Thema erstellen"></i>
                </a>
                <a id="mainMenuBtnSets" class="list-group-item set <%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Links.SetsAll() %>">
                    <i class="fa fa-search" aria-hidden="true"></i>Lernsets<i class="fa fa-plus-circle show-tooltip show-on-hover hide2 set-color add-new" onclick="window.location = '<%= Url.Action("Create", "EditSet") %>'; return false; " title="Neues Lernset erstellen"></i>
                </a>
                <a id="mainMenuBtnQuestions" class="list-group-item quest <%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>">
                    <i class="fa fa-search" aria-hidden="true"></i>Fragen
                                            <i id="mainMenuBtnQuestionCreate" class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" onclick="window.location = '<%= Links.CreateQuestion() %>'; return false; " title="Frage erstellen"></i>
                </a>
            </div>
            <div id="mainMenuGamesUsersMessages" class="menu-section secondary-section" style="width: 100%">
                <a id="mainMenuBtnGames" class="<%= Model.Active(MenuEntry.Play) %> list-group-item play" href="<%= Links.Games(Url) %>">Spielen
                                            <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" onclick="window.location = '<%= Links.GameCreate() %>'; return false; " title="Spiel erstellen"></i>
                </a>
                <a id="mainMenuBtnUsers" class="list-group-item users <%= Model.Active(MenuEntry.Users) %>" href="<%= Links.Users() %>">Nutzer </a>
                <a id="mainMenuBtnMessages" class="list-group-item messages <%= Model.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">Nachrichten
                                            <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display: inline-block; position: relative; top: 1px;"><%= Model.UnreadMessageCount %></span>
                </a>

                <% if (Model.IsInstallationAdmin)
                    { %>
                <a class="list-group-item cat <%= Model.Active(MenuEntry.Maintenance) %>" href="<%= Url.Action("Maintenance", "Maintenance") %>">Administrativ </a>
                <% } %>
            </div>
            <% } %>
        </div>
    </div>
</div>