<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>

<% if (!Model.TopNavMenu.IsWelcomePage && Model.TopNavMenu.BreadCrumbCategories != null && Model.TopNavMenu.BreadCrumbCategories.Items.Count > 0)
   {
       if (SessionUserLegacy.IsInOwnWiki())
       { %>
        <span id="FirstChevron">
            <i class="fa fa-chevron-right"></i>
        </span>
<% }
       }
   if (!SessionUserLegacy.IsInOwnWiki() || (!Model.TopNavMenu.IsCategoryBreadCrumb && Model.TopNavMenu.BreadCrumb != null ))
   { %>
    <span id="FirstChevron">
        <div id="BreadcrumbDivider"></div>
    </span><% } %>