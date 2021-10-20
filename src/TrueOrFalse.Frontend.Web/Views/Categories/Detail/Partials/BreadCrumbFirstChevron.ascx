<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>

<% if (!Model.TopNavMenu.IsWelcomePage && Model.TopNavMenu.BreadCrumbCategories != null && Model.TopNavMenu.BreadCrumbCategories.Items.Count > 0)
   {
       if (Sl.SessionUser.IsInOwnWiki())
       { %>
        <span id="FirstChevron">
            <i class="fa fa-chevron-right"></i>
        </span>
<% }
       }
   if (!Sl.SessionUser.IsInOwnWiki())
   { %>
    <span id="FirstChevron">
        <div id="BreadcrumbDivider"></div>
    </span><% } %>