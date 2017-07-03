<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" style="margin-top: 15px">
    <% foreach (var category in Model.CategoryTrail)
       { %>
            <div class="list-group-item"><a href="<%= Links.CategoryDetail(category.Name, category.Id) %>"><%= category.Name %></a></div>
       <% } %>
    <div class="list-group-item"><a style="font-weight: bold" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a></div>
</div>