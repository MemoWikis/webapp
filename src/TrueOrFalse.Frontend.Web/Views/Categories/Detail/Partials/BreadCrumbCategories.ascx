<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumbCategories = Model.BreadCrumbCategories;
   var breadCrumbCategoriesCount = breadCrumbCategories.Count;%>   

 <% if (breadCrumbCategories.Count > 1 && breadCrumbCategories.Count < 3 ) { 
         foreach (var rootCategory in Model.RootCategoriesList)
         {
             if (breadCrumbCategories.First() == rootCategory)
             {
                 switch (Model.RootCategoriesList.IndexOf(rootCategory))
                 {
                     case 0:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Schule">                                              
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Schule</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 1:
                    %> 
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Studium">                      
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Studium</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 2:
                    %>
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Zertifikate">                      
                          <span   style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Zertifikate</a>                         
                            <i style="display: inline;" class="fa fa-chevron-right"></i>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 3:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" title="Allgemeinwissen">                      
                          <span style="display: inline-table; margin-left: 10px;"><a href="<%= Links.CategoryDetail(rootCategory) %>">Allgemeinwissen</a>                         
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

    if (!(Model.IsSetBreadCrumb || Model.IsAnswerQuestionBreadCrumb)) {  
    breadCrumbCategoriesCount = breadCrumbCategoriesCount - 1; 
    }
    for (var i = 1; i <= breadCrumbCategoriesCount; i++)
    { %>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" title="Zur Themenseite">                 
              <%if (i == breadCrumbCategoriesCount){ %> 
                 <span style="display: inline-table; margin-left:10px; color:#000000; opacity:0.50;"><a href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a></span>              
                <%}else{ %>
                   <span style="display: inline-table; margin-left:10px;"><a href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a>
                       <i style="display: inline;" class="fa fa-chevron-right"></i>
                   </span> 
                <%} %>
           
        </div>
    <% } %>
