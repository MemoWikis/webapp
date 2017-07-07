<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="mainMenuThemeNavigation" class="menu-section">
<% var defaultCategories = Model.DefaultCategoriesList;
    for (int i = 0; i < defaultCategories.Count; i++)
    { %>
       <a class="list-group-item cat default-category" id="default-category-<%= defaultCategories[i].Name %>" href="<%= Links.CategoryDetail(defaultCategories[i].Name, defaultCategories[i].Id) %>">
    <% switch (i)
           {
               case 0: 
        %> <i class="fa fa-child"></i> <%
                break;

            case 1:
                %> <i class="fa fa-graduation-cap"></i> <%
                break;

            case 2:
                %> <i class="fa fa-file-text-o"></i> <%
                break;

            case 3:
                %> <i class="fa fa-lightbulb-o"></i> <%
                break;
        } %>
            <%= defaultCategories[i].Name %>
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

    var actualCategory = $('<a class= "cat sub list-group-item" href="<%= Links.CategoryDetail(Model.ActuallCategory.Name, Model.ActuallCategory.Id) %>"><%= Model.ActuallCategory.Name %></a>')
        .append($('<i class="fa fa-caret-right"></i>'));
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
    <% } %>
    <% } %>
</script>