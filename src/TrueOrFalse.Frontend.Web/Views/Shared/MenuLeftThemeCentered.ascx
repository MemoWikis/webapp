<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var isLongMenu = true; %>

<div class="mainMenuContainer">
    <nav id="mainMenuThemeCentered" style="display: none;">
        <div class="list-group">
            <div class="menu-section">
                <a id="mainMenuBtnKnowledge" class="list-group-item menu-section primary-point <%: Model.Active(MenuEntry.Knowledge)%>" href="<%= Links.Knowledge() %>">
                    <i id="mainMenuKnowledgeHeart" class="fa fa-heart fa-2x" style="color: #b13a48;"></i>
                    <span class="primary-point-text">
                        Wissenszentrale
                    </span>

                </a>
            </div>
            
            <% Html.RenderPartial("~/Views/Categories/Navigation/CategoryNavigation.ascx", new CategoryNavigationModel(Model.ActualCategory)); %>
            
            <% if (isLongMenu)
               { %>
                    <div id="mainMenuQuestionsSetsCategories" class="menu-section secondary-section">
                        <a id="mainMenuBtnCategories" class="list-group-item cat <%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>">
                            Themen
                
<%--                            <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new" 
                                onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; "
                                title="Neues Thema erstellen"></i>--%>
                        </a>
       
                        <a id="mainMenuBtnSets" class="list-group-item set <%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Links.SetsAll() %>">
                            Lernsets
                
<%--                            <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 set-color add-new" 
                                onclick="window.location = '<%= Url.Action("Create", "EditSet") %>'; return false; "
                                title="Neues Lernset erstellen"></i>--%>
                        </a>    

                        <a id="mainMenuBtnQuestions" class="list-group-item quest <%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>">
                            Fragen
<%--                            <i id="mainMenuBtnQuestionCreate" class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" 
                                onclick="window.location = '<%= Links.CreateQuestion() %>'; return false; "
                                title="Frage erstellen"></i>--%>
                        </a>
                    </div>

                    <div id="mainMenuGamesUsersMessages" class="menu-section secondary-section">
                        <a id="mainMenuBtnGames" class="<%= Model.Active(MenuEntry.Play) %> list-group-item play" href="<%= Links.Games(Url) %>">
                            Spielen
<%--                        <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new"
                            onclick="window.location = '<%= Links.GameCreate() %>'; return false; "
                            title="Spiel erstellen"></i>--%>
                        </a>

                        <a id="mainMenuBtnUsers" class="list-group-item users <%= Model.Active(MenuEntry.Users) %>" href="<%= Links.Users() %>">
                            Nutzer
                        </a>

                        <a id="mainMenuBtnMessages" class="list-group-item messages <%= Model.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">
                            Nachrichten
                        <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display: inline-block; position: relative; top: 1px;"><%= Model.UnreadMessageCount %></span>
                        </a>

                        <% if (Model.IsInstallationAdmin)
                            { %>
                                <a class="list-group-item cat <%= Model.Active(MenuEntry.Maintenance) %>" href="<%= Url.Action("Maintenance", "Maintenance") %>">
                                    Administrativ
                                </a>
                        <% } %>
                    </div>
            <% } %>
        </div>
    </nav>
</div>