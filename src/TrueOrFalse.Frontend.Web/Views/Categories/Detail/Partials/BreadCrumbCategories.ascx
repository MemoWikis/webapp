<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<% 
    var breadCrumbCategories = Model.BreadCrumbCategories.Items;
    breadCrumbCategories.Add(Model.BreadCrumbCategories.Current);
    var breadCrumbCategoriesCount = breadCrumbCategories.Count; 
    var startTopicId = Sl.SessionUser.IsLoggedIn ? Sl.SessionUser.User.StartTopicId : RootCategory.RootCategoryId;
%>

<%
for (var i = 0; i <= breadCrumbCategoriesCount; i++)
{ %>
    <div id="<%=i %>BreadCrumbContainer" class="breadcrumb-item" data-placement="bottom"> 
        <% if (true) {
                if (i == breadCrumbCategoriesCount - 1 && !Model.QuestionBreadCrumb && breadCrumbCategories[i].Category.Id != startTopicId) { %> 
                    <span><a id="<%=  i%>BreadCrumb" class="show-tooltip last-in-breadcrumb" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>" title="Zur Themenseite" data-placement="bottom"><%= breadCrumbCategories[i].Category.Name %></a></span>
              <%}
                else if(i != breadCrumbCategoriesCount && (breadCrumbCategories[i].Category.Id != startTopicId || i!= 0)){ %>
                    <span><a id="<%=i %>BreadCrumb" class="show-tooltip" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>"title="Zur Themenseite" data-placement="bottom"><%= breadCrumbCategories[i].Category.Name %></a>
                        <i class="fa fa-chevron-right"></i>
                    </span> 
              <%}
                else if(i == breadCrumbCategoriesCount && Model.QuestionBreadCrumb)
                { %>
                    <span><a id="<%=i %>BreadCrumb" class="show-tooltip" href="<%= Links.CategoryDetail(breadCrumbCategories[i].Category) %>" title="Zur Themenseite" data-placement="bottom"><%= breadCrumbCategories[i].Category.Name %></a>
                        <i class="fa fa-chevron-right"></i>
                    </span>
             <% } %>
         <% } %>
    </div>
<% } %>