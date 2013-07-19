<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="Seedworks.Lib" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="box" style="padding-left: 0px; padding-right: 0;">
    <div class="menu" >

        <div class="main">
            <a class="<%= Model.Active(MenuEntry.Knowledge) %>" href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>"> <i class="icon-caret-right"></i>
                Wissen (<span id="menuWishKnowledgeCount"><%= Model.WishKnowledgeCount %></span>)
            </a>
        </div>
        <div><a class="<%= Model.Active(MenuEntry.Questions) %>" href="<%= Url.Action("Questions", "Questions") %>"><i class="icon-caret-right"></i> Fragen</a></div>
        <% var index = 0; foreach (var question in new SessionUiData().VisitedQuestions) { index++ ;%>
               <div class="sub">
                   <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.QuestionDetail); } %>
                   <a href="<%= Links.AnswerQuestion(Url, question.Text, question.Id) %>" class="show-tooltip <%=activeClass %>" title="Frage: <%=question.Text %>" data-placement="right">
                       <i class="icon-caret-right"></i> <%=question.Text.Truncate(100)%>
                   </a>
               </div>
        <% } %>

        <div><a class="<%= Model.Active(MenuEntry.QuestionSet) %>" href="<%= Url.Action("QuestionSets", "QuestionSets") %>"><i class="icon-caret-right"></i> Fragesätze</a></div>
        <% index = 0; foreach (var set in new SessionUiData().VisitedQuestionSets){ index++; %>
               <div class="sub">
                   <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.QuestionSetDetail); } %>
                   <a href="<%= Links.QuestionSetDetail(Url, set.Name, set.Id) %>" class="show-tooltip <%= activeClass %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                       <i class="icon-caret-right"></i> <%=set.Name%>
                   </a>
               </div>
        <% } %>
        
        <div><a href="#"><i class="icon-caret-right"></i> Termine</a></div>
        <%--<div><a href="#"><i class="icon-caret-right"></i> Lerngruppen</a></div>--%>

    
        <div style="margin-top: 13px;">
            <a class="<%= Model.Active(MenuEntry.Categories) %>" href="<%= Url.Action(Links.Categories, Links.CategoriesController) %>"><i class="icon-caret-right"></i> Kategorisierung </a>
        </div>
        <% index = 0; foreach (var set in new SessionUiData().VisitedCategories){ index++; %>
               <div class="sub">
                   <% var activeClass = "";  if (index == 1) { activeClass = Model.Active(MenuEntry.CategoryDetail); } %>
                   <a href="<%= Links.CategoryDetail( Url, set.Name, set.Id) %>" class="show-tooltip <%= activeClass %>" title="Fragesatz: <%=set.Name%>" data-placement="right">
                       <i class="icon-caret-right"></i> <%=set.Name%>
                   </a>
               </div>
        <% } %>
    
        <div class="main" style="margin-top:12px;">
            <a href="#" class="<%= Model.Active(MenuEntry.News) %>" ><i class="icon-caret-right"></i> Neues <span class="badge badge-info" style="display:inline-block; position: relative; top: -2px;">21</span></a>
        </div>

        <div class="main" style="margin-top:12px;"><a href="#"><i class="icon-caret-right"></i> Nutzer<img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></a> </div>
    
        <% index = 0; foreach (var user in new SessionUiData().VisitedProfiles){ index++;  %>
               <div class="sub">
                   <% var activeClass = ""; if (index == 1) { activeClass = Model.Active(MenuEntry.ProfilDetail); } %>
                   <a href="<%= Links.Profile(Url, user.Name, user.Id) %>" class="<%= activeClass %>">
                       <i class="icon-caret-right"></i> <%=user.Name%>
                   </a>
               </div>
        <% } %>
    
        <% if (Request.IsLocal && Model.IsInstallationAdmin){ %>

            <div class="main" style="margin-top:12px;">
                <a href="<%= Url.Action("Maintenance", "Maintenance") %>"><i class="icon-caret-right"></i> Adminstrativ</a> 
            </div>
        <% } %>
        
    </div>
    
</div>

<% if(Model.Categories.Any()){ %>
    <div class="box" style="padding-left: 0px; padding-right: 0;">
        <div class="menu">
            <div class="main no-link"><i class="icon-caret-right"></i> Kategorien</div>
            
            <% foreach(var category in Model.Categories){ %>
                <div class="sub"><a href="#"><i class="icon-caret-right"></i> <%=category.Category.Name %> (<span><%=category.OnPageCount %>x) </span></a></div>
            <% } %>
        </div>
    </div>
<% } %>