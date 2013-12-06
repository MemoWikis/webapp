<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="Seedworks.Lib" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% if (Url.RequestContext.RouteData.Values["controller"].ToString() == Links.HelpController) { %>
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
<% } %>

<div class="box box-menu" style="padding-left: 0px; padding-right: 0;">
    <div class="menu" >

        <div class="main">
            <a class="<%= Model.Active(MenuEntry.Knowledge) %>" href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>"> <i class="fa fa-caret-right"></i>
                Wissen (<span id="menuWishKnowledgeCount"><%= Model.WishKnowledgeCount %></span>)
            </a>
        </div>
        <div><a class="<%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>"><i class="fa fa-caret-right"></i> Fragen</a></div>
        <% var index = 0; foreach (var question in new SessionUiData().VisitedQuestions) { index++ ;%>
               <div class="sub">
                   <% 
                       var activeClass = "";  
                       if (index == 1)
                           activeClass = Model.Active(MenuEntry.QuestionDetail);
                           
                       string url = "";
                       if(question.Set != null)
                           url = Links.AnswerQuestion(Url, question.Question, question.Set);
                       else
                           url = Links.AnswerQuestion(Url, question.SearchSpec);

                       string tooltip = "";
                       if (!String.IsNullOrEmpty(question.Text))
                       {
                           tooltip = "Frage: " + question.Text.Replace("\"", "'");
                           if (index != 1 || activeClass != "active")
                               tooltip += " <br><br> Antwort: " + question.Solution.Replace("\"", "'");                           
                       }
                   %>
                   <a href="<%= url %>" class="show-tooltip <%=activeClass %>" title="<%= tooltip %>" data-placement="right" data-html="true">
                       <i class="fa fa-caret-right"></i> <%=question.Text.Truncate(100)%>
                   </a>
               </div>
        <% } %>

        <div><a class="<%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Url.Action("Sets", "Sets")%>"><i class="fa fa-caret-right"></i> Fragesätze</a></div>
        <% index = 0; foreach (var set in new SessionUiData().VisitedQuestionSets){ index++; %>
               <div class="sub">
                   <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.QuestionSetDetail); } %>
                   <a href="<%= Links.SetDetail(Url, set.Name, set.Id) %>" class="show-tooltip <%= activeClass %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                       <i class="fa fa-caret-right"></i> <%=set.Name%>
                   </a>
               </div>
        <% } %>
        
        <div><a href="<%= Links.Dates(Url) %>" class="<%= Model.Active(MenuEntry.Dates) %>"><i class="fa fa-caret-right"></i> Termine</a></div>
        <%--<div><a href="#"><i class="fa fa-caret-right"></i> Lerngruppen</a></div>--%>
    
        <div style="margin-top: 13px;">
            <a class="<%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>"><i class="fa fa-caret-right"></i> Kategorisierung </a>
        </div>
        <% index = 0; foreach (var set in new SessionUiData().VisitedCategories){ index++; %>
               <div class="sub">
                   <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.CategoryDetail); } %>
                   <a href="<%= Links.CategoryDetail( Url, set.Name, set.Id) %>" class="show-tooltip <%= activeClass %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                       <i class="fa fa-caret-right"></i> <%=set.Name%>
                   </a>
               </div>
        <% } %>
    
        <div class="main" style="margin-top:12px;">
            <a href="<%= Links.News(Url) %>" class="<%= Model.Active(MenuEntry.News) %>"><i class="fa fa-caret-right"></i> Neues <span class="badge badge-info" style="display:inline-block; position: relative; top: -2px;">21</span></a>
        </div>

        <div class="main" style="margin-top:12px;">
            <a class="<%= Model.Active(MenuEntry.Users) %>" href="<%= Url.Action("Users", "Users")%>"><i class="fa fa-caret-right"></i> Nutzer<img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></a> 
        </div>
        <div><a class="<%= Model.Active(MenuEntry.Play) %>" href="#"><i class="fa fa-caret-right"></i> Spielen</a></div>
    
        <% index = 0; foreach (var user in new SessionUiData().VisitedUserDetails){ index++;  %>
               <div class="sub">
                   <% var activeClass = ""; if (index == 1) { activeClass = Model.Active(MenuEntry.UserDetail); } %>
                   <a href="<%= Links.UserDetail(Url, user.Name, user.Id) %>" class="<%= activeClass %>">
                       <i class="fa fa-caret-right"></i> <%=user.Name%>
                   </a>
               </div>
        <% } %>
    
        <% if (Model.IsInstallationAdmin){ %>

            <div class="main" style="margin-top:12px;">
                <a href="<%= Url.Action("Maintenance", "Maintenance") %>"><i class="fa fa-caret-right"></i> Adminstrativ</a> 
            </div>
        <% } %>
        
    </div>
    
</div>

<% if(Model.Categories.Any()){ %>
    <div class="box" style="padding-left: 0px; padding-right: 0;">
        <div class="menu">
            <div class="main no-link"><i class="fa fa-caret-right"></i> Kategorien</div>
            
            <% foreach(var category in Model.Categories){ %>
                <div class="sub"><a href="<%= Links.CategoryDetail(Url, category.Category) %>"><i class="fa fa-caret-right"></i> <%=category.Category.Name %> (<span><%=category.OnPageCount %>x) </span></a></div>
            <% } %>
        </div>
    </div>
<% } %>