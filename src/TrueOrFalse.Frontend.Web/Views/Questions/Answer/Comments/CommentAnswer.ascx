<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="panel-body" style="position: relative">
    <div class="col-xs-2">
        <img class="pull-right" style="width:50%; border-radius:5px;" src="<%= Model.ImageUrl %>">
    </div>
    <div class="col-xs-10" style="height: 100%; padding-bottom: 25px; ">
        <div>
            <span style="color:silver">
                <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.CreatorName %></a>
                <span style="font-size: 11px; padding-left: 5px;">vor <%= Model.CreationDateNiceText%></span>
            </span>
        </div>
        <div>
            <%= Model.Text.LineBreaksToBRs() %>    
        </div>
    </div>
</div>