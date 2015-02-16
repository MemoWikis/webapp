<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetsSearchResultModel>" %>

<div class="box-content">
    <% 
        if(Model.NotAllowed){
            Html.RenderPartial("RegisterOrLogin_Sets");
        }else{ 
            foreach (var row in Model.SetRows){
                    Html.RenderPartial("SetRow", row);
            } 
        }
    %>
</div>
<% if(!Model.NotAllowed){ 
    Html.RenderPartial("Pager", Model.Pager);
} %>
