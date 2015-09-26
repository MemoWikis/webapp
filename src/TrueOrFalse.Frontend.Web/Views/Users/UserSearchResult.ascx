<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserSearchResultModel>" %>

<div class="box-content">
    <% foreach(var row in Model.Rows){
        Html.RenderPartial("UserRow", row);
    } %>
</div>