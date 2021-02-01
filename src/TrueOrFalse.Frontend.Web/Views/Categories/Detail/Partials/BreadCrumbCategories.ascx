<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% 
    var breadCrumbCategories = Model.BreadCrumbCategories.Items;
    breadCrumbCategories.Add(Model.BreadCrumbCategories.Current);
    var breadCrumbCategoriesCount = breadCrumbCategories.Count; %>
<%
for (var i = 1; i <= breadCrumbCategoriesCount - 1; i++)
{ %>
    <div id="<%=i %>BreadCrumbContainer" style="display: flex; height: auto; margin-bottom: 8px;" class="show-tooltip" data-placement="bottom" title="Zur Themenseite"> 
        <% if (true) {
                if (i == breadCrumbCategoriesCount - 1 && !Model.QuestionBreadCrumb) { %> 
                    <span style="margin-left:10px;"><a  id="<%=  i%>BreadCrumb" style="color:#003264;" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>" class=""><%= breadCrumbCategories[i].Category.Name %></a></span>
              <%}
                else if(i != breadCrumbCategoriesCount){ %>
                    <span  style="display:inline-table; margin-left:10px;"><a id="<%=i %>BreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>" class=""><%= breadCrumbCategories[i].Category.Name %></a>
                        <i style="display: inline;" class="fa fa-chevron-right"></i>
                    </span> 
              <%}
                else if(i == breadCrumbCategoriesCount && Model.QuestionBreadCrumb)
                { %>
                    <span  style="display:inline-table; margin-left:10px;"><a id="<%=i %>BreadCrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>" class=""><%= breadCrumbCategories[i].Category.Name %></a>
                        <i style="display: inline;" class="fa fa-chevron-right"></i>
                    </span>
             <% } %>
         <% } %>
    </div>
<% } %>