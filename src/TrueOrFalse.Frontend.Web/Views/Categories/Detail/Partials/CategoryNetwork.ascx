<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h4>Über- und untergeordnete Kategorien</h4>
<div class="CategoryRelations Box">
    <% if (Model.CategoriesParent.Count > 0) { %>
        <div>
            <% foreach (var category in Model.CategoriesParent)
                { %>
                <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
            <% } %>
        </div>
    <% }
        else { %>
        <div>keine übergeordneten Kategorien</div>
    <%  } %>

    <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
    <div class="MainCategory"><span class="label label-category"><%= Model.Name %></span></div>
    <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>

    <% if(Model.CategoriesChildren.Count > 0){ %>
        <div>
            <% foreach(var category in Model.CategoriesChildren){ %>
                <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>
            <% } %>
            <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                style="font-size: 14px; cursor: pointer"
                onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                data-original-title="Neue untergeordnete Kategorie erstellen"></i>
        </div>
    <% } else { %>
        <div style="margin-top: 0;">keine untergeordneten Kategorien
            <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                style="font-size: 14px; cursor: pointer"
                onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                data-original-title="Neue untergeordnete Kategorie erstellen"></i>
        </div>
    <%  } %>
</div>
