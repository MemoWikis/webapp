<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SetVideoModel>" %>

<div class="well" style="margin-bottom: -7px;">
    <div class="row">
        <div class="col-xs-12">
            <div style="margin-top: 13px; margin-right: 7px; display: inline-block; font-size: 18px;">Fragen</div>

            <% 
            for (var i = 0; i < Model.Questions.Count; i++)
            {
                var question = Model.Questions[i];
                var classCurrent = question.Id == Model.CurrentQuestion ? "btn-info" : "btn-default";
            %>
                <a href="#" class="btn btn-sm <%= classCurrent %>" data-video-question-id="<%= question.Id %>"><%= i %></a>
            <% } %>
        </div>
    </div>    
</div>