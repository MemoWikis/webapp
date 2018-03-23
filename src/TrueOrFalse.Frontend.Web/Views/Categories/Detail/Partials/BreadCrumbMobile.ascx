<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumb = Model.BreadCrumb; %>   

  <div style="display:flex; height:37px">   
      <div>
          <a href="/" class="category-icon">
              <i class="PathMenuHomeImg fa fa-home show-tooltip" title="Startseite"></i>
              <span style="margin-left: 10px">Home</span>
               </a>
              <span><i class="fa fa-chevron-right"></i></span>       
      </div>
       <%  for (var i = 1; i < breadCrumb.Count -1; i++)
        { %>                                                                  
            <div class="PathMenuLicense" style="width: 21px; height: 21px; float: none; margin-right: 0px; margin-left: 12px; top: -37px">
             <%= Model.GetCategoryImage(breadCrumb[i]).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(breadCrumb[i])) %>
            </div>
            <a href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>
            <span><i class="fa fa-chevron-right"></i></span>
  
    <% } %>

  </div> 
