<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumb = Model.BreadCrumb; %>   

<div  style="display: flex; flex-wrap: wrap; width: 100%;">                      
    <div style="height: auto; margin-bottom: 5px" class="show-tooltip" title="zur Startseite">
        <i class="PathMenuHomeImg fa fa-home"></i>
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
    </div>
    <%  for (var i = 1; i < breadCrumb.Count -1; i++)
        { %>
    <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" title="zur Themenseite">
        <div class="PathMenuImage">
            <%= Model.GetCategoryImage(breadCrumb[i]).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(breadCrumb[i])) %>
        </div>
        <span style="display: inline-table;"><a style="margin-left: 10px;" href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>
            <i style="display: inline;" class="fa fa-chevron-right"></i>
        </span>
    </div>
    <% } %>
</div> 
