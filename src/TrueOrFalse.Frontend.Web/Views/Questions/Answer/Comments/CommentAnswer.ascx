<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="panel-body" style="position: relative">
    <div class="col-xs-2">
        <img class="pull-right" style="width:50%; border-radius:5px;" src="<%= Model.ImageUrl %>">
    </div>
    <div class="col-xs-10" style="height: 100%; padding-bottom: 15px; ">
        <div>
            <span style="color:silver">
                <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a>
                <span style="font-size: 11px; padding-left: 5px;">vor <%= Model.CreationDateNiceText%></span>
            </span>
        </div>
        <div>
            <%= Model.Text.LineBreaksToBRs() %>    
        </div>
        <div>
            <% if (Model.IsInstallationAdmin) { %>
                <a href="#" class="btnMarkAsSettled btn btn-sm btn-link" data-type="btn-markAsSettled" style="padding-left: 0; margin-left: 0; <%= Html.CssHide(Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als erledigt markieren</a>
                <a href="#" class="btnMarkAsUnsettled btn btn-sm btn-link" data-type="btn-markAsUnsettled" style="padding-left: 0; margin-left: 0; <%= Html.CssHide(!Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als nicht erledigt markieren</a>
            <% } %>
        </div>
    </div>
</div>