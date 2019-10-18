<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Question>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div style="display: flex; flex-wrap: wrap;">
    <% foreach(var category in Model.Categories){ %>

        <% if(category.IsSpoiler(Model)){ %>
            <a href="#" onclick="location.href='<%= Links.CategoryDetail(category)%>'"><span data-isSpolier="true" class="label label-category">Spoiler</span></a>
        <% }else{ %>
            <% Html.RenderPartial("CategoryLabel", category); %>
        <%} %>

    <% } %>
</div>
