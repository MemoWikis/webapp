<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">

    <div class="main" ><%= Html.ActionLink("Wissen (175)", Links.Knowledge, Links.KnowledgeController)%></div>
    <div><%= Html.ActionLink("Fragen", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Fragensätze", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Kurse", Links.Questions, Links.QuestionsController)%></div>
    
    <div style="margin-top: 13px;"><%= Html.ActionLink("Kategorisierung", Links.Categories, Links.CategoriesController)%></div>
    
    <div class="main" style="margin-top:12px;"><a href="#">Neues <span class="badge badge-info" style="display:inline-block; position: relative; top: -2px;">21</span></a>
        
    </div>

    <div class="main" style="margin-top:12px;"><a href="#" >Netzwerk<img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></a> </div>

    <% foreach (var user in new SessionUiData().LastVisitedProfiles)
       { %>
       <div><a href="<%= Url.Action(Links.UserProfile, Links.UserProfileController, new {name= user.UrlName, id = user.Id}, null) %>"><%=user.Name%></a></div>
     <%  } %>
         
     <% if(Model.Categories.Any()){ %>
         <p class="categories">Kategorien</p>
         <div class="category">
             <% foreach(var category in Model.Categories){ %>
                <a href="#" style="margin-bottom: 3px;"><%=category.Category.Name %> (<span><%=category.OnPageCount %>x) </span></a>
            <% } %>
         </div>
     <% } %>
</div>