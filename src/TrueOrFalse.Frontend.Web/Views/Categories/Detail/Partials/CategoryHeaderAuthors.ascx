<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="Authors">
    <% foreach (var author in Model.Authors.Take(7)) { %>
        <a class="author" href="<%= Links.UserDetail(author.User) %>" data-toggle="tooltip" data-placement="top" title="<%= author.Name %>">
            <div class="author">
                <img class="ItemImage JS-InitImage author-img" alt="" src="<%= author.ImageUrl %>" data-append-image-link-to="ImageContainer" />
                <span class="author-img-label"></span>
            </div>
        </a>
    <% } %>


    <% if (Model.Authors.Count > 7 ){%>
        <div class="dropdown">
            <div class="additional-authors-btn" type="button" id="AuthorDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                +<%= Model.Authors.Count - 7 %>
            </div>
            <ul class="dropdown-menu" aria-labelledby="AuthorDropdown">
                <% foreach (var author in Model.Authors.Skip(7)) { %>
                    <li>                                    
                        <a href="<%= Links.UserDetail(author.User) %>">
                            <div class="author-listitem">
                                <img class="ItemImage JS-InitImage author-img" alt="" src="<%= author.ImageUrl %>" data-append-image-link-to="ImageContainer" />
                                <span class="author-img-label"><%= author.Name %></span>
                            </div>
                        </a>
                    </li>
                <% } %>
            </ul>
        </div>
    <%} %>
</div>
