<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Seedworks.Lib.Persistence.IPager>" %>
<%@ Import Namespace="System.Globalization" %>

<% const int size = 3; %>

<div class="row" style="margin-top:-20px;">
    <div class="pagination" style="text-align:center;" >
        <ul>
            <li <% if (!Model.HasPreviousPage()) { %>class="disabled"<% } %>>
                <%= Html.ActionLink("«",
                    ViewContext.RouteData.Values["action"].ToString(), 
                    ViewContext.RouteData.Values["controller"].ToString(),
                    new { page = Model.CurrentPage - 1}, null)%>
            </li>
            <% for (var i =  1;  i <= Model.PageCount; i++)
               { %>
                <li <% if (Model.CurrentPage == i) { %>class="active"<% } %>>
                    <%= Html.ActionLink(i.ToString(CultureInfo.InvariantCulture),
                        ViewContext.RouteData.Values["action"].ToString(), 
                        ViewContext.RouteData.Values["controller"].ToString(), 
                        new { page = i }, null)%>
                </li>
            <% } %>
            <li <% if (!Model.HasNextPage()) { %>class="disabled"<% } %>>
                <%= Html.ActionLink("»",
                    ViewContext.RouteData.Values["action"].ToString(), 
                    ViewContext.RouteData.Values["controller"].ToString(),
                    new { page = Model.CurrentPage + 1}, null)%>
            </li>
        </ul>
    </div>        
</div>
