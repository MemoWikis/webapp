<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" style="margin-top: 15px">

<% foreach (var defaultCategory in Model.DefaultCategoriesList)
   { %>
    <a class="list-group-item cat" id="default-category-<%= defaultCategory.Name %>" href="<%= Links.CategoryDetail(defaultCategory.Name, defaultCategory.Id) %>">
        <i class="fa fa-caret-right"></i> <%= defaultCategory.Name %>
    </a>
<% } %>
</div>

<script>
    <% if(Model.ActuallCategory != null) { %>
    <% if (Model.RootCategory == Model.ActuallCategory)
    { %>
    $("#default-category-<%= Model.ActuallCategory.Name %>").addClass("active");
    <% }
       else
       { %>
           
    var rootCategory = $("#default-category-<%= Model.RootCategory.Name %>");

    if (rootCategory.length === 0)
        var rootCategory = $("#default-category-Allgemeinwissen");

    var actualCategory = $('<a class= "cat sub list-group-item" style="font-weight: bold" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a>');
    rootCategory.after(actualCategory);

    <% if (Model.CategoryTrail.Count > 0)
       {
           for (int i = 0; i < 2; i++)
           {
               if (i > Model.CategoryTrail.Count - 1)
                   break; %>
    var upperCategory = $('<a class="cat sub list-group-item" href="<%= Links.CategoryDetail(Model.CategoryTrail[i].Name, Model.CategoryTrail[i].Id) %>"><%= Model.CategoryTrail[i].Name %></a>');
    rootCategory.after(upperCategory);
    <% } %>
    <% } %>
    <% }  %>
    <% }  %>
</script>