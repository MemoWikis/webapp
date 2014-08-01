<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionsSearchResultModel>" %>

<div class="box-content">
    <% 
        if(Model.NotAllowed){
            Html.RenderPartial("RegisterOrLogin_Questions");
        }else{ 
            foreach (var row in Model.QuestionRows){
                    Html.RenderPartial("QuestionRow", row);
            } 
        }
    %>
</div>
<% if(!Model.NotAllowed){ 
    Html.RenderPartial("Pager", Model.Pager);
} %>
