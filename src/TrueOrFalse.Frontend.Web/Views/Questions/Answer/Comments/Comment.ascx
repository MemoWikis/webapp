<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="panel panel-default" style="margin-top: 7px;">
    <div class="panel-heading">
        <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.CreatorName %></a>
        <span style="color: darkgray">
            vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a>
        </span>
    </div>
    <div class="panel-body" style="position: relative">
        <div class="col-lg-2">
            <img style="width:100%; border-radius:5px;" src="<%= Model.ImageUrl %>">
        </div>
        <div class="col-lg-10" style="height: 100%; padding-bottom: 25px; ">
            <%= Model.Text %>
        </div>
    </div>
    
    <% foreach(var answer in Model.Answers){ %>
        <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswer.ascx", answer); %>
    <% } %>
    
    <div class="panel-body" style="position: relative">
        <% if(Model.IsLoggedIn){ %>
            <div style="position: absolute; bottom: 8px; right: 20px;">
                <a href="#" class="btnAnswerComment" data-comment-id="<%= Model.Id %>">Antworten</a>
            </div>
        <% } %>    
    </div>
</div>