<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web.Context" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">

    <div class="main"><%= Html.ActionLink("Neues", Links.News, Links.NewsController)%></div>
    
    <div class="main" style="margin-top:12px;"><%= Html.ActionLink("Wissen", Links.Knowledge, Links.KnowledgeController)%></div>
    <div><%= Html.ActionLink("Fragen", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Fragensätze", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Kurse", Links.Questions, Links.QuestionsController)%></div>
    <div class="no-hover">-------------</div>
    <div><%= Html.ActionLink("Kategorisierung", Links.Categories, Links.CategoriesController)%></div>

    <div class="main" style="margin-top:12px;"><a href="#" >Netzwerk<img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></a> </div>

    <% foreach (var user in new SessionUiData().LastVisitedProfiles)
       { %>
       <div><a href="<%= Url.Action(Links.UserProfile, Links.UserProfileController, new {name= user.UrlName, id = user.Id}, null) %>"><%=user.Name%></a></div>
     <%  } %>

</div>