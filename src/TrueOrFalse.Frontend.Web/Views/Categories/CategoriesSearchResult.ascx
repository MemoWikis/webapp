<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoriesSearchResultModel>" %>

<div class="box-content">
    <% 
        foreach (var row in Model.Rows){
            Html.RenderPartial("CategoryRow", row);
        }
    %>
</div>
<% Html.RenderPartial("Pager", Model.Pager); %>