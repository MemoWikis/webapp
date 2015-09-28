<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionsSearchResultModel>" %>

<div class="box-content">
    <% 
        if(Model.AccessNotAllowed){
            Html.RenderPartial("RegisterOrLogin_Questions");
        }else{ 
            foreach (var row in Model.QuestionRows){
                Html.RenderPartial("QuestionRow", row);
            } 
    %>
        <div class="rowBase" style="padding:10px; <%= Html.CssHide(Model.QuestionRows.Any()) %>" id="rowNoResults">
            Keine Treffer. <br/> 
            Bitte weitertippen oder anderen Suchbegriff verwenden.
        </div>    
    <%
        }
    %>
</div>
<% if(!Model.AccessNotAllowed){ 
    Html.RenderPartial("Pager", Model.Pager);
} %>
