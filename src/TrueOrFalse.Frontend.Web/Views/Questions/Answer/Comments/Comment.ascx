<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>

<div class="panel panel-default" style="margin-top: 7px;">
    <div class="panel-heading">
        <%= Model.CreatorName %>
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
        
        <div style="position: absolute; bottom: 8px; right: 20px;">
            <a href="#">Antworten</a>
        </div>
    </div>
</div>