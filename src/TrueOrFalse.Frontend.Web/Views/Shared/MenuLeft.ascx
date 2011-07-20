<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="menu">

    <div class="main"><%= Html.ActionLink("Neues", Links.News, Links.NewsController)%></div>
    
    <div class="main" style="padding-top:12px;"><%= Html.ActionLink("Wissen", Links.Knowledge, Links.KnowledgeController)%></div>
    <div><%= Html.ActionLink("Fragen", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Fragensätze", Links.Questions, Links.QuestionsController)%></div>
    <div><%= Html.ActionLink("Kurse", Links.Questions, Links.QuestionsController)%></div>
    <div>-------------</div>
    <div><%= Html.ActionLink("Kategorien", Links.Categories, Links.CategoriesController)%></div>

    <div class="main" style="padding-top:12px;"><a href="#">Netzwerk</a> <img src="/images/menu-icon-person.png" style="vertical-align: text-top;" ></div>

</div>