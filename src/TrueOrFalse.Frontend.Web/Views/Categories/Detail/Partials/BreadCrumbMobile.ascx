<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var breadCrumb = Model.BreadCrumb; %>   
<a href="/" class="category-icon">
    <i class="PathMenuHomeImg fa fa-home show-tooltip" title="Startseite"></i>
    <span style="margin-left: 10px">Home</span>
</a>                
       <%  for (var i = 1; i < breadCrumb.Count; i++)
        { %>
            <span> <i class="fa fa-chevron-right"></i> </span>                                                       
           
                <i class="PathMenuHomeImg fa fa-home show-tooltip" title="Startseite"></i> 
            <a href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>

        <%= Model.ImageFrontendData.RenderHtmlImageBasis(21, true, ImageType.Category, linkToItem: Links.CategoryDetail(breadCrumb[i]), additionalCssClasses:"PathMenuImg") %>
    <% } %>

