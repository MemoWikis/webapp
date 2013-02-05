<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PagerModel>" %>
<%@ Import Namespace="System.Globalization" %>

<% const int size = 3; %>

<div class="row">
    <div class="pagination" style="text-align:center;" >
        <ul>
            <% if (Model.HasPreviousPage) { %>
                <li>
                    <%= Html.ActionLink("«",
                        ViewContext.RouteData.Values["action"].ToString(), 
                        ViewContext.RouteData.Values["controller"].ToString(),
                        new { page = Model.CurrentPage - 1}, null)%>
                </li>
            <% } else { %>
                <li class="disabled"><a href="#">«</a></li>
            <% } %>
            
            <% if (Model.LastPage > 1 ){ %>
                <li <% if (1 == Model.CurrentPage) { %> class="active" <% } %>>
                    <%= Html.ActionLink(1.ToString(CultureInfo.InvariantCulture),
                        ViewContext.RouteData.Values["action"].ToString(), 
                        ViewContext.RouteData.Values["controller"].ToString(), 
                        new { page = 1 }, null)%>
                </li>
            <%} %>
            
            <% if (Model.SkippedLeft) { %>
                <li class="disabled"><a href="#">...</a></li>
            <% } %>
            
            <% if (Model.PageCountWithoutLastAndFirst > 0)  foreach (var i in Enumerable.Range(Model.Start, Model.PageCountWithoutLastAndFirst)) { %>
                <li <% if (i == Model.CurrentPage) { %> class="active" <% } %>>
                    <%= Html.ActionLink(i.ToString(CultureInfo.InvariantCulture),
                        ViewContext.RouteData.Values["action"].ToString(), 
                        ViewContext.RouteData.Values["controller"].ToString(),
                        new { page = i }, null)%>
                </li>   
            <% } %>
            
            <% if (Model.SkippedRight) { %>
                <li class="disabled"><a href="#">...</a></li>
            <% } %>

            <li <% if (Model.LastPage == Model.CurrentPage) { %> class="active" <% } %>>
                <%= Html.ActionLink(Model.LastPage.ToString(CultureInfo.InvariantCulture),
                    ViewContext.RouteData.Values["action"].ToString(), 
                    ViewContext.RouteData.Values["controller"].ToString(), 
                    new { page = Model.LastPage }, null)%>
            </li>

            <% if (Model.HasNextPage) { %>
                <li>
                    <%= Html.ActionLink("»",
                        ViewContext.RouteData.Values["action"].ToString(), 
                        ViewContext.RouteData.Values["controller"].ToString(),
                        new { page = Model.CurrentPage + 1}, null)%>
                </li>
            <% } else { %>
                <li class="disabled"><a href="#">»</a></li>
            <% } %>
        </ul>
    </div>        
</div>
