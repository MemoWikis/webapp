﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% int index; %>

<div class="mainMenuContainer">
    <nav id="mainMenu" style="display: none;">
        <div class="list-group">
            <a id="mainMenuBtnKnowledge" class="list-group-item know <%: Model.Active(MenuEntry.Knowledge)%>" href="<%= Links.Knowledge() %>">
                <i class="fa fa-caret-right"></i> 
                Wissenszentrale <span style="float:right"><i class="fa fa-heart" style="color:#b13a48;"></i> <span id="menuWishKnowledgeCount"><%= Model.WishKnowledgeCount %></span></span>
            </a>
            <a id="mainMenuBtnDates" class="list-group-item dues <%= Model.Active(MenuEntry.Dates) %>" href="<%= Links.Dates() %>">
                <i class="fa fa-caret-right"></i> Termine
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 date-color add-new" 
                    onclick="window.location = '<%= Url.Action("Create", "EditDate") %>'; return false; "
                    title="Neuen Termin erstellen"></i>
            </a>            
            <%
                var visitedD = new SessionUiData().VisitedDatePages;
                index = 0;
                foreach (var date in visitedD){
                    index++;
                    var activeClass = "";
                    if (index == 1) { activeClass = Model.Active(MenuEntry.DateDetail); 
                    } %>
                    <a href="<%= Links.DateEdit(date.Id) %>" class="list-group-item dues sub <%= activeClass + visitedD.CssFirst(index) + visitedD.CssLast(index) %>">
                        <i class="fa fa-caret-right"></i> <%=date.Name%>
                        <i class="fa fa-pencil" style="position: relative; left: 3px; top: -1px;"></i>
                    </a>
            <% } %>
            
            <div id="mainMenuQuestionsSetsCategories">
            <a id="mainMenuBtnQuestions" class="list-group-item quest <%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Fragen
                <i id="mainMenuBtnQuestionCreate" class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" 
                    onclick="window.location = '<%= Links.CreateQuestion(Url) %>'; return false; "
                    title="Frage erstellen"></i>
            </a>

            <%  index = 0;
                var visitedQ = new SessionUiData().VisitedQuestions;
                foreach (var question in visitedQ) {
                    index++;
                    string activeClass = (index == 1) ? Model.Active(MenuEntry.QuestionDetail) : "";
            
                    string url = "";
                    if(question.Set != null)
                        url = Links.AnswerQuestion(Url, question.Question, question.Set);
                    else if (question.SearchSpec != null)
                        url = Links.AnswerQuestion(Url, question.SearchSpec);
                    else
                        url = Links.AnswerQuestion(question.Question);

                    string tooltip = "";
                    if (!String.IsNullOrEmpty(question.Text))
                    {
                        tooltip = "Frage: " + question.Text.Replace("\"", "'");
                        if ((index != 1 || activeClass != "active") && question.Solution != null)
                            tooltip += " <br><br> Antwort: " + question.Solution.Replace("\"", "'");                           
                    }
                    %>
                    <a href="<%= url %>" class="list-group-item quest show-tooltip sub <%=activeClass + " " + visitedQ.CssFirst(index) + visitedQ.CssLast(index)%>" title="<%= tooltip %>" data-placement="right" data-html="true">
                        <i class="fa fa-caret-right"></i> <%=question.Text.Truncate(100)%>
                    </a>
            <% } %>
            
            <a id="mainMenuBtnSets" class="list-group-item set <%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Links.SetsAll() %>">
                <i class="fa fa-caret-right"></i> Fragesätze
                
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 set-color add-new" 
                    onclick="window.location = '<%= Url.Action("Create", "EditSet") %>'; return false; "
                    title="Neuen Fragesatz erstellen"></i>
            </a>    
            <%
                var visitedS = new SessionUiData().VisitedSets;
                index = 0; 
                foreach (var set in visitedS){ index++; %>
                    <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.QuestionSetDetail); } %>
            
                    <a href="<%= Links.SetDetail(Url, set.Name, set.Id) %>" class="show-tooltip list-group-item set sub <%= activeClass + " " + visitedS.CssFirst(index) + visitedS.CssLast(index) %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                        <i class="fa fa-caret-right"></i> <%=set.Name%>
                    </a>
            <% } %>

            <a id="mainMenuBtnCategories" class="list-group-item cat <%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.CategoriesAction, Links.CategoriesController) %>">
                <i class="fa fa-caret-right"></i> Themen
                
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new" 
                    onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; "
                    title="Neues Thema erstellen"></i>             
            </a>
       
            <% var visitedC = new SessionUiData().VisitedCategories;
               index = 0; 
               foreach (var categoryHistoryItem in visitedC){ index++; %>
                 <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.CategoryDetail); } %>

                <a href="<%= Links.CategoryDetail(categoryHistoryItem.Name, categoryHistoryItem.Id) %>" class="show-tooltip cat sub <%= activeClass + visitedC.CssFirst(index) + visitedC.CssLast(index) %> list-group-item" title="Thema: <%=categoryHistoryItem.Name%>" data-placement="right">
                    <i class="fa fa-caret-right"></i> <%=categoryHistoryItem.Name%>
                </a>
            <% } %>
            </div>

            <a id="mainMenuBtnUsers" class="list-group-item users <%= Model.Active(MenuEntry.Users) %>" href="<%= Links.Users() %>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Nutzer
            </a>
            <%
                var visitedU = new SessionUiData().VisitedUserDetails;
                index = 0; 
                foreach (var user in visitedU){ index++;  %>
                <% var activeClass = ""; if (index == 1) { activeClass = Model.Active(MenuEntry.UserDetail); } %>
                <a href="<%= Links.UserDetail(user.Name, user.Id) %>" class="list-group-item users sub <%= activeClass + visitedU.CssFirst(index) + visitedU.CssLast(index) %>">
                    <i class="fa fa-caret-right"></i> <%=user.Name%>
                </a>
            <% } %>
        
            <a id="mainMenuBtnMessages" class="list-group-item messages <%= Model.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">
                <i class="fa fa-caret-right"></i> Nachrichten
                <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display:inline-block; position: relative; top: 1px;"><%= Model.UnreadMessageCount %></span>
            </a>

            <a id="mainMenuBtnGames" class="<%= Model.Active(MenuEntry.Play) %> list-group-item play" href="<%= Links.Games(Url) %>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Spielen
                
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" 
                    onclick="window.location = '<%= Links.GameCreate() %>'; return false; "
                    title="Spiel erstellen"></i>
            </a>
                            
            <% if (Model.IsInstallationAdmin){ %>
                <a class="list-group-item cat <%= Model.Active(MenuEntry.Maintenance) %>" style="margin-top: 10px;" href="<%= Url.Action("Maintenance", "Maintenance") %>">
                    <i class="fa fa-caret-right"></i> Administrativ
                </a>
            <% } %>

        </div>
    </nav>
</div>