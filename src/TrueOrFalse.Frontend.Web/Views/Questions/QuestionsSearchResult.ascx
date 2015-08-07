<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionsSearchResultModel>" %>

<div class="box-content">
    <% 
        if(Model.AccessNotAllowed){
            Html.RenderPartial("RegisterOrLogin_Questions");
        }else{ 
            foreach (var row in Model.QuestionRows){
                Html.RenderPartial("QuestionRow", row);
            } 
        }
    %>
</div>
<% if(!Model.AccessNotAllowed){ 
    Html.RenderPartial("Pager", Model.Pager);
} %>
