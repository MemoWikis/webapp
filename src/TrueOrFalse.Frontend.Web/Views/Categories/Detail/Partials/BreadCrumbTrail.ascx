<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>


<ul id="Path" class="path dropdown-menu" aria-labelledby="BreadCrumbDropdown"></ul>

<% if (!Model.TopNavMenu.IsWelcomePage)
   { %>
    <% if ((Model.TopNavMenu.IsCategoryBreadCrumb || Model.TopNavMenu.QuestionBreadCrumb) && Model.TopNavMenu.BreadCrumbCategories != null)
       { %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model.TopNavMenu) %>
    <% } %>

    <% var i = 0;
       foreach (var breadCrumbItem in Model.TopNavMenu.BreadCrumb)
       {
           i++;
    %>
        <div>

            <% if (breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last()))
               { %>
                <span>
                    <a class="show-tooltip" id="<%= i %>BreadCrumb" href="<%= breadCrumbItem.Url %>" title="<%= breadCrumbItem.ToolTipText %>" data-placement="bottom">
                        <%= breadCrumbItem.Text %>
                    </a>
                </span>
            <% }
               else
               { %>
                <span>
                    <a id="<%= i %>BreadCrumb" class="show-tooltip" href="<%= breadCrumbItem.Url %>" title="<%= breadCrumbItem.ToolTipText %>" data-placement="bottom"><%= breadCrumbItem.Text %></a>
                    <% if (!breadCrumbItem.Equals(Model.TopNavMenu.BreadCrumb.Last()))
                       { %>
                        <i style="display: inline;" class="fa fa-chevron-right"></i>
                    <% } %>
                </span>
            <% } %>
        </div>
    <% } %>
<% } %>