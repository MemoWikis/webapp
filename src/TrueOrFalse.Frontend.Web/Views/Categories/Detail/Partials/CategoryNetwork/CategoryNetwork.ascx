<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <h4>Über- und untergeordnete Themen</h4>
    
    <% if (Model.Category.IsHistoric) { %>
        <div class="alert alert-info" role="alert">
            Aus technischen Gründen können <b>keine Archivdaten für <i>Über- und untergeordnete 
            Themen</i></b> angezeigt werden. Es werden die aktuellen Themenbeziehungen dargestellt.
        </div>
    <% } %>
    
    <div class="CategoryRelations Box">
        <% if (Model.CategoriesParent.Count > 0) { %>
            <div>
                <% foreach (var category in Model.CategoriesParent)
                    { %>
                    <% Html.RenderPartial("CategoryLabel", category); %>
                <% } %>
            </div>
        <% }
            else { %>
            <div>keine übergeordneten Themen</div>
        <%  } %>
    
        <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
        <div class="MainCategory"><span class="label label-category"><%= Model.Name %></span></div>
        <div class="RelationArrow"><i class="fa fa-arrow-down"></i></div>
    
        <% if(Model.CategoriesChildren.Count > 0){ %>
            <div>
                <% foreach(var category in Model.CategoriesChildren){ %>
                    <% Html.RenderPartial("CategoryLabel", category); %>
                <% } %>
                <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                    style="font-size: 14px; cursor: pointer"
                    onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                    data-original-title="Neues untergeordnetes Thema erstellen"></i>
            </div>
        <% } else { %>
            <div style="margin-top: 0;">keine untergeordneten Themen
                <i class="fa fa-plus-circle show-tooltip color-category add-new" 
                    style="font-size: 14px; cursor: pointer"
                    onclick="window.location = '/Kategorien/Erstelle?parent=<%= Model.Category.Id%>'; return false; " 
                    data-original-title="Neues untergeordnetes Thema erstellen"></i>
            </div>
        <%  } %>
    </div>


<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>


