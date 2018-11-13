﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SidebarModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%  var isLongMenu = true; %>

<div style="display: flex;">
    <div id="mainMenuThemeCenteredMobile">
        <div id="RightMainMenu" class="RightMainMenu">
            <% Html.RenderPartial("/Views/Categories/Navigation/CategoryNavigation.ascx", Model.categoryNavigationModel); %>
            <% if (isLongMenu)
                { %>
                <div id="mainMenuQuestionsSetsCategories" class="menu-section secondary-section" style="width: 100%">
                  <a id="mainMenuBtnCategories" class="list-group-item cat <%= Model.Active(MainMenuEntry.Categories) %>" href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>">
                        Themen<i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new" onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; " title="Neues Thema erstellen"></i>
                  </a>
                  <a id="mainMenuBtnSets" class="list-group-item set <%= Model.Active(MainMenuEntry.QuestionSet) %>" href="<%= Links.SetsAll() %>">
                    Lernsets<i class="fa fa-plus-circle show-tooltip show-on-hover hide2 set-color add-new" onclick="window.location = '<%= Url.Action("Create", "EditSet") %>'; return false; " title="Neues Lernset erstellen"></i>
                  </a>
                  <a id="mainMenuBtnQuestions" class="list-group-item quest <%= Model.Active(MainMenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>">
                    Fragen<i id="mainMenuBtnQuestionCreate" class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" onclick="window.location = '<%= Links.CreateQuestion() %>'; return false; " title="Frage erstellen"></i>
                  </a>
                </div>
                <div id="mainMenuAboutUsersAdmin" class="menu-section secondary-section" style="width: 100%">
                    <a id="mainMenuBtnAboutMemucho" class="list-group-item cat  <%= Model.Active(MainMenuEntry.About) %>" href="<%= Links.AboutMemucho() %>">Über memucho </a>
                    <a id="mainMenuBtnUsers" class="list-group-item users <%= Model.Active(MainMenuEntry.Users) %>" href="<%= Links.Users() %>">Nutzer </a>
                <% if (Model.IsInstallationAdmin) { %>
                    <a class="list-group-item cat <%= Model.Active(MainMenuEntry.Maintenance) %>" href="<%= Url.Action("Maintenance", "Maintenance") %>">Administrativ </a>
                <% } %>
            </div>
            <% } %>
        </div>
    </div>
</div>