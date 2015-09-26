<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserSearchResultModel>" %>

<div class="box-content">
    <% foreach(var row in Model.Rows){
        Html.RenderPartial("UserRow", row);
    } %>
    
    <div class="rowBase" style="padding:10px; <%= Html.CssHide(Model.Rows.Any()) %>" id="rowNoResults">
        Keine Treffer. <br/> 
        Bitte weitertippen oder anderen Suchbegriff verwenden.
    </div>
</div>