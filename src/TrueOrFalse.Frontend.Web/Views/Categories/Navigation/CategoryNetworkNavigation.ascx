<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryNetworkNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CategoryRelations Box" style="text-align: center;">
    <% if (Model.CategoriesParent.Count > 0) { %>
        <div>
            <% foreach (var category in Model.CategoriesParent){ %>
                <% Html.RenderPartial("~/Views/Categories/Navigation/CategoryNetworkNavigationCatLabel.ascx", category); %>
<%--                <a class="networkNavigationUpdate" href="#" data-category-id="<%=category.Id %>"><i class="fa fa-bullseye"></i></a>--%>
            <% } %>
        </div>
    <% }
        else { %>
        <div>keine übergeordneten Themen</div>
    <%  } %>

    <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
    <div class="MainCategory"><span class="label label-category"><%= Model.CategoryName %></span></div>
    <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>

    <% if(Model.CategoriesChildren.Count > 0){ %>
        <div>
            <% foreach(var category in Model.CategoriesChildren){ %>
                <% Html.RenderPartial("~/Views/Categories/Navigation/CategoryNetworkNavigationCatLabel.ascx", category); %>
<%--                <a class="networkNavigationUpdate" href="#" data-category-id="<%=category.Id %>"><i class="fa fa-bullseye"></i></a>--%>
            <% } %>
            <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                style="font-size: 14px; cursor: pointer"
                onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.CategoryId%>'; return false; " 
                data-original-title="Neues untergeordnetes Thema erstellen"></i>
        </div>
    <% } else { %>
        <div style="margin-top: 0;">keine untergeordneten Themen
            <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                style="font-size: 14px; cursor: pointer"
                onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.CategoryId%>'; return false; " 
                data-original-title="Neues untergeordnetes Thema erstellen"></i>
        </div>
    <%  } %>
</div>
