<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoriesSearchResultModel>" %>

<div class="box-content" style="clear: all;">
    <div class="row">
    <% 
        foreach (var row in Model.Rows){
            Html.RenderPartial("CategoryRow", row);
        }
    %>
    </div>
    
    <div class="rowBase" style="padding:10px; <%= Html.CssHide(Model.Rows.Any()) %>" id="rowNoResults">
        Keine Treffer. <br/> 
        Bitte weitertippen oder anderen Suchbegriff verwenden.
    </div>

</div>
<div class="clearfix"></div>
<% Html.RenderPartial("Pager", Model.Pager); %>