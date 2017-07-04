<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" style="margin-top: 15px">
<%--    <% if (Model.CategoryTrail.Count > 0)
       { 
             for (int i = 0; i < 2; i++)
             {
                 if (i > Model.CategoryTrail.Count - 1)
                     break; %>
                <div class="list-group-item"><a href="<%= Links.CategoryDetail(Model.CategoryTrail[i].Name, Model.CategoryTrail[i].Id) %>"><%= Model.CategoryTrail[i].Name %></a></div>
            <% } %>
    <% } %>
    <div class="list-group-item"><a style="font-weight: bold" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a></div>--%>

<% foreach (var defaultCategory in Model.DefaultCategoriesList)
   { %>
    <a class="list-group-item cat" href="<%= Links.CategoryDetail(defaultCategory.Name, defaultCategory.Id) %>">
        <i class="fa fa-caret-right"></i> <%= defaultCategory.Name %>
    </a>
<% } %>
</div>