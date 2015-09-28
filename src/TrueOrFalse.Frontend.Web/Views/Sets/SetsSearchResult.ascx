<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetsSearchResultModel>" %>

<div class="box-content">
    <% 
        if(Model.NotAllowed){
            Html.RenderPartial("RegisterOrLogin_Sets");
        }else{ 
            foreach (var row in Model.SetRows){
                    Html.RenderPartial("SetRow", row);
            } 
        %>
            <div class="rowBase" style="padding:10px; <%= Html.CssHide(Model.SetRows.Any()) %>" id="rowNoResults">
                Keine Treffer. <br/> 
                Bitte weitertippen oder anderen Suchbegriff verwenden.
            </div>
        <%

        }
    %>
</div>
<% if(!Model.NotAllowed){ 
    Html.RenderPartial("Pager", Model.Pager);
} %>