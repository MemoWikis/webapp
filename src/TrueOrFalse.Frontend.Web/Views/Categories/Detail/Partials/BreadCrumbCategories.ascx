<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% var breadCrumbCategories = Model.BreadCrumbCategories;
   var breadCrumbCategoriesCount = breadCrumbCategories.Count;%>   

 <% if (breadCrumbCategories.Count > 0 && breadCrumbCategories.Count < 3 ) { 
         foreach (var rootCategory in Model.RootCategoriesList)
         {
             if (breadCrumbCategories.First() == rootCategory)
             {
                 switch (Model.RootCategoriesList.IndexOf(rootCategory))
                 {
                     case 0:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" data-placement="bottom" title="Schule">                                              
                          <span <%if(breadCrumbCategories.Count == 1){ %> style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;" <%}else{ %>style="display: inline-table; margin-left: 10px;"<%} %>><a href="<%= Links.CategoryDetail(rootCategory) %>">Schule</a>                         
                           <%if(!(breadCrumbCategories.Count == 1)){ %> <i style="display: inline;" class="fa fa-chevron-right"></i><%} %>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 1:
                    %> 
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" data-placement="bottom" title="Studium">                      
                          <span <%if(breadCrumbCategories.Count == 1){ %> style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;" <%}else{ %>style="display: inline-table; margin-left: 10px;"<%} %>><a href="<%= Links.CategoryDetail(rootCategory) %>">Studium</a>                         
                           <%if(!(breadCrumbCategories.Count == 1)){ %> <i style="display: inline;" class="fa fa-chevron-right"></i><%} %>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 2:
                    %>
                     <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" data-placement="bottom" title="Zertifikate">                      
                          <span <%if(breadCrumbCategories.Count == 1){ %> style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;" <%}else{ %>style="display: inline-table; margin-left: 10px;"<%} %>><a href="<%= Links.CategoryDetail(rootCategory) %>">Zertifikate</a>                         
                           <%if(!(breadCrumbCategories.Count == 1)){ %> <i style="display: inline;" class="fa fa-chevron-right"></i><%} %>
                         </span>                      
                    </div>
                    <%
                    break;

                    case 3:
                    %>
                    <div style="display: flex; height: auto; margin-bottom: 5px" class="category-icon show-tooltip" data-placement="bottom" title="Allgemeinwissen">                      
                          <span <%if(breadCrumbCategories.Count == 1){ %> style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;" <%}else{ %>style="display: inline-table; margin-left: 10px;"<%} %>><a href="<%= Links.CategoryDetail(rootCategory) %>">Allgemeinwissen</a>                         
                           <%if(!(breadCrumbCategories.Count == 1)){ %> <i style="display: inline;" class="fa fa-chevron-right"></i><%} %>
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

    breadCrumbCategoriesCount--;
                        
    for (var i = 1; i <= breadCrumbCategoriesCount; i++)
    { %>
        <div id="<%if (i == breadCrumbCategoriesCount && !(Model.IsAnswerQuestionOrSetBreadCrumb)){%>Last<%} else{ %><%=i %><%} %>BreadCrumbContainer" style="display: flex; height: auto; margin-bottom: 5px;" class="show-tooltip" data-placement="bottom" title="Zur Themenseite"> 
            <% if (!(Model.IsAnswerQuestionOrSetBreadCrumb)) {
                    if (i == breadCrumbCategoriesCount) { %> 
                     <div style="margin-left:10px; color:#000000; opacity:0.50;"><div><a style="display:block; text-overflow:ellipsis; overflow:hidden;" id="LastBreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a></div></div>              
                  <%} else { %>
                     <div  style="display: flex; margin-left:10px;"><div><a id="<%=i %>BreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" style="display:block; overflow:hidden; text-overflow:ellipsis;" class=""><%= breadCrumbCategories[i].Name %></a></div>
                      <div><i style="display: inline;" class="fa fa-chevron-right"></i></div>
                     </div> 
                  <%}%>
             <%}else{%>
                <div style="display:flex; margin-left:10px;"><div><a id="<%= i %>BreadCrumb" style="display:block; text-overflow:ellipsis; overflow:hidden;" href="<%= Links.CategoryDetail(breadCrumbCategories[i]) %>" class=""><%= breadCrumbCategories[i].Name %></a></div>
                  <div><i style="display: inline;" class="fa fa-chevron-right"></i></div>
                </div> 
             <%} %>
        </div>
    <% } %>
