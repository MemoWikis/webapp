<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>

<div class="panel-body" style="position: relative">
    <div class="col-lg-1 col-lg-offset-1">
        <img style="width:100%; border-radius:5px;" src="<%= Model.ImageUrl %>">
    </div>
    <div class="col-lg-10" style="height: 100%; padding-bottom: 25px; ">
        <%= Model.Text %>
    </div>
        
    <div style="position: absolute; bottom: 8px; right: 20px;">
        <a href="#">Antworten</a>
    </div>
</div>