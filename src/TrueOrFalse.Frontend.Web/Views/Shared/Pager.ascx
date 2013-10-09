<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PagerModel>" %>
<%@ Import Namespace="System.Globalization" %>

<div class="row">
    <div class="pagination" style="text-align:center;" >
        <ul>
            <% if (Model.HasPreviousPage) { %>
                <li>
                    <a href="<%= Url.Action(Model.Action, Model.Controller, new { page = Model.CurrentPage - 1 }) %>">«</a>
                </li>
            <% } else { %>
                <li class="disabled"><a href="#">«</a></li>
            <% } %>
            
            <% if (Model.LastPage > 1 ){ %>
                <li <% if (1 == Model.CurrentPage) { %> class="active" <% } %>>
                    <a href="<%= Url.Action(Model.Action, Model.Controller, new { page = 1 }) %>"><%= 1 %></a>
                </li>
            <%} %>
            
            <% if (Model.SkippedLeft) { %>
                <li class="disabled"><a href="#">...</a></li>
            <% } %>
            
            <% if (Model.PageCountWithoutLastAndFirst > 0)  foreach (var i in Enumerable.Range(Model.Start, Model.PageCountWithoutLastAndFirst)) { %>
                <li <% if (i == Model.CurrentPage) { %> class="active" <% } %>>
                    <a href="<%= Url.Action(Model.Action, Model.Controller, new { page = i }) %>"><%= i %></a>
                </li>   
            <% } %>
            
            <% if (Model.SkippedRight) { %>
                <li class="disabled"><a href="#">...</a></li>
            <% } %>

            <li <% if (Model.LastPage == Model.CurrentPage) { %> class="active" <% } %>>
                <a href="<%= Url.Action(Model.Action, Model.Controller, new { page = Model.LastPage }) %>"><%= Model.LastPage %></a>
            </li>

            <% if (Model.HasNextPage) { %>
                <li>
                    <a href="<%= Url.Action(Model.Action, Model.Controller, new { page = Model.CurrentPage + 1}) %>">»</a>
                </li>
            <% } else { %>
                <li class="disabled"><a href="#">»</a></li>
            <% } %>
        </ul>
    </div>        
</div>
