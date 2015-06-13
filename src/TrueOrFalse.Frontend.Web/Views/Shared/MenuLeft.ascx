<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% int index; %>

<%--<% if (Url.RequestContext.RouteData.Values["controller"].ToString() == Links.HelpController) { %>
    <div class="box" style="padding-left: 0px; padding-right: 0;">
        <div class="menu">
            <a href="<%= Url.Action(Links.HelpWillkommen, Links.HelpController) %>">
                <div class="main no-link"><i class="fa fa-caret-right"></i>  Hilfe</div>
            </a>
            
            <% var idx = 0; foreach (var helpPage in new SessionUiData().VisitedHelpPages) { idx++ ;%>
               <div class="sub">
                   <% var activeClass = ""; if (idx == 1) { activeClass = Model.Active(MenuEntry.Help); } %>
                   <a href="<%= Url.Action(helpPage.Text, Links.HelpController) %>" class="show-tooltip <%=activeClass %>" title="" data-placement="right">
                       <i class="fa fa-caret-right"></i> <%=helpPage.Text.Truncate(100)%>
                   </a>
               </div>
            <% } %>
        </div>
    </div>
<% } %>--%>

<div class="mainMenuContainer">
    <nav id="mainMenu" style="display: none;">
        <div class="list-group">
            <a class="list-group-item know <%: Model.Active(MenuEntry.Knowledge)%>" href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>">
                <i class="fa fa-caret-right"></i> 
                Wunschwissen  <span style="float:right"><i class="fa fa-heart-o"></i> <span id="menuWishKnowledgeCount"><%= Model.WishKnowledgeCount %></span></span>
            </a>
            <a class="list-group-item dues <%= Model.Active(MenuEntry.Dates) %>" href="<%= Links.Dates(Url) %>">
                <i class="fa fa-caret-right"></i> Termine
            </a>            

            <a class="list-group-item quest <%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Fragen
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" 
                    onclick="window.location = '<%= Links.CreateQuestion(Url) %>'; return false; "
                    title="Frage erstellen"></i>
            </a>

            <%  index = 0;
                var visitedQ = new SessionUiData().VisitedQuestions;
                foreach (var question in visitedQ) {
                    index++;
                    string activeClass = (index == 1) ? Model.Active(MenuEntry.QuestionDetail) : "";
            
                    if(question.Type == HistoryItemType.Edit){ %>
                        <a href="<%= Links.EditQuestion(Url, question.Id) %>" class="list-group-item quest sub <%=activeClass + " " + visitedQ.CssFirst(index) + visitedQ.CssLast(index)%>" data-placement="right" data-html="true">
                            <i class="fa fa-caret-right"></i>
                            <%=question.Text.Truncate(100)%>
                            <i class="fa fa-pencil" style="position: relative; left: 3px; top: -1px;"></i>
                        </a>
                    <% } else { 
                           
                        string url = "";
                        if(question.Set != null)
                            url = Links.AnswerQuestion(Url, question.Question, question.Set);
                        else
                            url = Links.AnswerQuestion(Url, question.SearchSpec);

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
            <% } %>
            
            <a class="list-group-item set <%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Url.Action("Sets", "Sets")%>">
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
            
                    <% if(set.Type == HistoryItemType.Edit){ %>
                        <a href="<%= Links.QuestionSetEdit(Url, set.Id) %>" class="show-tooltip list-group-item set sub <%= activeClass + " " + visitedS.CssFirst(index) + visitedS.CssLast(index) %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                            <i class="fa fa-caret-right"></i> 
                            <%=set.Name%>
                            <i class="fa fa-pencil" style="position: relative; left: 3px; top: -1px;"></i>
                        </a>
                    <% }else{ %>
                        <a href="<%= Links.SetDetail(Url, set.Name, set.Id) %>" class="show-tooltip list-group-item set sub <%= activeClass + " " + visitedS.CssFirst(index) + visitedS.CssLast(index) %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                            <i class="fa fa-caret-right"></i> <%=set.Name%>
                        </a>
                    <% } %>
            <% } %>

            <a class="list-group-item cat <%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>">
                <i class="fa fa-caret-right"></i> Kategorien
                
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 cat-color add-new" 
                    onclick="window.location = '<%= Url.Action("Create", "EditCategory") %>'; return false; "
                    title="Neue Kategorie erstellen"></i>             
            </a>
       
            <% var visitedC = new SessionUiData().VisitedCategories;
               index = 0; 
               foreach (var categoryHistoryItem in visitedC){ index++; %>
                 <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.CategoryDetail); } %>

                 <% if(categoryHistoryItem.Type == HistoryItemType.Edit){ %>
                    <a href="<%= Links.CategoryEdit( Url, categoryHistoryItem.Id) %>" class="show-tooltip cat sub <%= activeClass + visitedC.CssFirst(index) + visitedC.CssLast(index) %> list-group-item" title="Kategorie bearbeiten: <%=categoryHistoryItem.Name%>" data-placement="right">
                        <i class="fa fa-caret-right"></i> 
                        <%=categoryHistoryItem.Name%>
                        <i class="fa fa-pencil" style="position: relative; left: 3px; top: -1px;"></i> 
                    </a>
                 <% }else{ %>
                    <a href="<%= Links.CategoryDetail(categoryHistoryItem.Name, categoryHistoryItem.Id) %>" class="show-tooltip cat sub <%= activeClass + visitedC.CssFirst(index) + visitedC.CssLast(index) %> list-group-item" title="Kategorie: <%=categoryHistoryItem.Name%>" data-placement="right">
                        <i class="fa fa-caret-right"></i> <%=categoryHistoryItem.Name%>
                    </a>
                <% } %>
            <% } %>
        
            <a class="list-group-item users <%= Model.Active(MenuEntry.Users) %>" href="<%= Url.Action("Users", "Users")%>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Nutzer<img src="/images/menu-icon-person.png" style="position: relative; top: -1px; left: 4px;" >
            </a>
            <%
                var visitedU = new SessionUiData().VisitedUserDetails;
                index = 0; 
                foreach (var user in visitedU){ index++;  %>
                <% var activeClass = ""; if (index == 1) { activeClass = Model.Active(MenuEntry.UserDetail); } %>
                <a href="<%= Links.UserDetail(Url, user.Name, user.Id) %>" class="list-group-item users sub <%= activeClass + visitedU.CssFirst(index) + visitedU.CssLast(index) %>">
                    <i class="fa fa-caret-right"></i> <%=user.Name%>
                </a>
            <% } %>
        
            <a class="list-group-item messages <%= Model.Active(MenuEntry.Messages) %>" href="<%= Links.Messages(Url) %>">
                Nachrichten
                <span id="badgeNewMessages" class="badge show-tooltip" title="Ungelesene Nachrichten" style="display:inline-block; position: relative; top: 1px;"><%= Model.UnreadMessageCount %></span>
            </a>

            <a class="<%= Model.Active(MenuEntry.Play) %> list-group-item play" href="<%= Links.Games(Url) %>" style="margin-top: 10px;">
                <i class="fa fa-caret-right"></i> Spielen
                
                <i class="fa fa-plus-circle show-tooltip show-on-hover hide2 quest-color add-new" 
                    onclick="window.location = '<%= Links.GameCreate(Url) %>'; return false; "
                    title="Spiel erstellen"></i>
            </a>
                            
            <% if (Model.IsInstallationAdmin){ %>
                <a class="list-group-item cat" style="margin-top: 10px;" href="<%= Url.Action("Maintenance", "Maintenance") %>">
                    <i class="fa fa-caret-right"></i> Administrativ
                </a>
            <% } %>

        </div>
    </nav>
</div>

<% if(Model.Categories.Any()){ %>
    <div class="menuCategories">
        <h4><span class="ColoredUnderline">Kategorien</span></h4>
        <% foreach(var catMenuItem in Model.Categories){ %>
            <a href="<%= Links.QuestionWithCategoryFilter(Url, catMenuItem) %>"><span class="label label-category"> <%=catMenuItem.Category.Name %> (<span><%=catMenuItem.OnPageCount %>x) </span> </span></a>
        <% } %>
    </div>
<% } %>