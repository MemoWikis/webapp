<%@ Control Language="C#" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var breadCrumb = Model.BreadCrumb;
    if (breadCrumb.Count == 1 && Model.RootCategoriesList.Contains(breadCrumb.First()))
    { %>
        <a href="/" class="category-icon">
            <i class="fa fa-home show-tooltip" title="Startseite"></i>
        </a>
        <span> <i class="fa fa-chevron-right"></i> </span>
        <a href="<%= Links.CategoryDetail(breadCrumb.First()) %>" class=""><%= breadCrumb.First().Name %></a>
 <% }
    else
    {
        foreach (var rootCategory in Model.RootCategoriesList)
        {
            if (breadCrumb.First() == rootCategory)
            {
                switch (Model.RootCategoriesList.IndexOf(rootCategory))
                {
                    case 0:
                    %>
                    <a href="<%= Links.CategoryDetail(rootCategory) %>" class="category-icon">
                        <i class="fa fa-child show-tooltip" title="Schule"></i>
                    </a>
                    <%
                    break;

                    case 1:
                    %> 
                    <a href="<%= Links.CategoryDetail(rootCategory) %>" class="category-icon">
                        <i class="fa fa-graduation-cap show-tooltip" title="Studium"></i>
                    </a>
                    <%
                    break;

                    case 2:
                    %>
                    <a href="<%= Links.CategoryDetail(rootCategory) %>" class="category-icon">
                        <i class="fa fa-file-text show-tooltip" title="Zertifikate"></i>
                    </a>
                    <%
                    break;

                    case 3:
                    %>
                    <a href="<%= Links.CategoryDetail(rootCategory) %>" class="category-icon">
                        <i class="fa fa-lightbulb-o show-tooltip" title="Allgemeinwissen"></i>
                    </a>
                    <%
                    break;
                            
                    //default:
                    //throw new Exception("This should not happen");
                }
                break;
            }
        }
            
        for (var i = 1; i < breadCrumb.Count; i++)
        { %>
            <span> <i class="fa fa-chevron-right"></i> </span>
            <a href="<%= Links.CategoryDetail(breadCrumb[i]) %>" class=""><%= breadCrumb[i].Name %></a>
    <% } %>
<% } %>
