<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation">
    <% foreach (var category in Model.CategoryTrail)
       { %>
            <div><a href="<%= Links.CategoryDetail(category.Name, category.Id) %>"><%= category.Name %></a></div>
       <% } %>
    <a style="font-weight: bold" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a>
</div>