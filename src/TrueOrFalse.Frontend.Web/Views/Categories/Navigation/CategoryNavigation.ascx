<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" style="margin-top: 15px">
    <% if (Model.CategoryTrail.Count > 0)
       { %>
            <div class="list-group-item"><a href="<%= Links.CategoryDetail(Model.CategoryTrail.First().Name, Model.CategoryTrail.First().Id) %>"><%= Model.CategoryTrail.First().Name %></a></div>
    <% } %>
    <div class="list-group-item"><a style="font-weight: bold" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a></div>
</div>