<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SubCategoriesModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h1><%: Model.Title %></h1>
<p><%: Model.Text %></p>

<div id="subCategory">
    <% foreach (var subCategory in Model.SubCategoryList)
       { %>
        <div class="row">
            <div class="col-xs-4">
                <%= Model.GetCategoryImage(subCategory).RenderHtmlImageBasis(100, false, ImageType.Category) %>
            </div>
            <div class="col-xs-8">
                <a href="<%= Links.GetUrl(subCategory) %>"><%: subCategory.Name %></a>
                <%-- HIER PROGRESS BAR REIN --%>
            </div>
        </div>
        <div>
            <ul>
            <% var childCategories = Sl.CategoryRepo.GetChildren(subCategory.Id);
               foreach (var childCategory in childCategories)
               { %>
                    <li><%: childCategory.Name %></li>
            <% } %>
            </ul>
        </div>
    <% } %>
</div>