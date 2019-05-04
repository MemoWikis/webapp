<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumbCategories = Model.BreadCrumbCategories;
   var breadCrumbCategoriesCount = breadCrumbCategories.Count;%>   

 <% if (breadCrumbCategories.Count > 0) { 
         foreach (var rootCategory in Model.RootCategoriesList)
         {
             if (breadCrumbCategories.First() == rootCategory)
             {%>
                  <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" data-placement="bottom" title="<%= rootCategory.Name %>">                                              
                         <span style="display: inline-table; margin-left: 10px;"><a  <%if(breadCrumbCategories.Count == 1){ %> style="color:#003264;" <%}%> href="<%= Links.CategoryDetail(rootCategory) %>"><%= rootCategory.Name %></a>                         
                           <%if(breadCrumbCategories.Count != 1){ %> <i style="display: inline;" class="fa fa-chevron-right"></i><%} %>
                         </span>                      
                  </div>
          <% }
         }
    }

    breadCrumbCategoriesCount--;
    for (var i = 1; i <= breadCrumbCategoriesCount; i++)
    { %>
        <div id="<%=i %>BreadCrumbContainer" style="display: flex; height: auto; margin-bottom: 8px;" class="show-tooltip" data-placement="bottom" title="Zur Themenseite"> 
            <% if (true) {
                    if (i == breadCrumbCategoriesCount && !Model.IsAnswerQuestionOrSetBreadCrumb && !Model.IsWidget) { %> 
                     <span style="margin-left:10px;"><a  id="<%=  i%>BreadCrumb" style="color:#003264;" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a></span>              
                  <%}
                    else if(i != breadCrumbCategoriesCount){ %>
                     <span  style="display:inline-table; margin-left:10px;"><a id="<%=i %>BreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a>
                                 <i style="display: inline;" class="fa fa-chevron-right"></i>
                     </span> 
                  <%}
                    else if(i == breadCrumbCategoriesCount && Model.IsAnswerQuestionOrSetBreadCrumb || Model.IsWidget)
                    { %>
                    <span  style="display:inline-table; margin-left:10px;"><a id="<%=i %>BreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a>
                        <i style="display: inline;" class="fa fa-chevron-right"></i>
                    </span> 
                 <% } %>
             <%}%>
        </div>
    <% } %>
