<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumb = Model.BreadCrumb; %>   

  <div style="display:flex; flex-wrap:wrap; width:100%">   
      <div style="height:auto; margin-bottom:5px">
          <a href="/" class="category-icon">
              <i class="PathMenuHomeImg fa fa-home show-tooltip" title="Startseite"></i>
              <span style="margin-left: 10px">Home</span>
               </a>
              <span><i class="fa fa-chevron-right"></i></span>       
      </div>
       <%  for (var i = 1; i < breadCrumb.Count -1; i++)
        { %>                                                                  
            <div style="display:flex; height:auto; margin-bottom:5px">               
            <div class="PathMenuImage">
             <%= Model.GetCategoryImage(breadCrumb[i]).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(breadCrumb[i])) %>
            </div>
            
            <span><a style="margin-left:10px;  -webkit-box-decoration-break: clone; box-decoration-break: clone;" href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>              
            <span><i class="fa fa-chevron-right"></i></span>
            </span>          
           </div>
    <% } %>
        </div> 
