<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumb = Model.BreadCrumb; %>   

<div  style="display: flex; flex-wrap: wrap; width: 100%;">                      
    <div style="height: auto; margin-bottom: 5px;" class="show-tooltip"  title="zur Startseite">
        <i class="fa fa-home"></i>
        <a href="/" class="category-icon">
            <span style="margin-left: 7px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
    </div>        

 <% 
     if (breadCrumb.Count > 1 && breadCrumb.Count < 3 ) { 
         foreach (var rootCategory in Model.RootCategoriesList)
         {
             if (breadCrumb.First() == rootCategory)
             {
                 switch (Model.RootCategoriesList.IndexOf(rootCategory))
                 {
                     case 0:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Schule">                      
                        <i class="fa fa-child"></i>
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Schule</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 1:
                    %> 
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Studium">                      
                        <i class="fa fa-graduation-cap"></i>
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Studium</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 2:
                    %>
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Zertifikate">                      
                        <i class="fa fa-file-text"></i>
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Zertifikate</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 3:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Allgemeinwissen">                      
                        <i class="fa fa-lightbulb-o"></i>
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Allgemeinwissen</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;
                            
                    //default:
                    //throw new Exception("This should not happen");
                }
                break;
            }
        }
 }
      for (var i = 1; i < breadCrumb.Count -1; i++)
        { %>
    <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" title="zur Themenseite">       
            <%= Model.GetCategoryImage(breadCrumb[i]).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(breadCrumb[i])) %>
        <span style="display: inline-table; margin-left:10px;"><a href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>
            <i style="display: inline;" class="fa fa-chevron-right"></i>
        </span>
    </div>
    <% } %>
</div> 
