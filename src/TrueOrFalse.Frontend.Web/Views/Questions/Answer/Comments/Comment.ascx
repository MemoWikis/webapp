<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="panel panel-default" style="margin-top: 7px;">
    <div class="panel-heading">
        <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a>
        <span style="color: darkgray">
            vor <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></span>
        </span>
    </div>
    <div class="panel-body" style="position: relative">
        <div class="col-xs-2">
            <img style="border-radius:5px;" src="<%= Model.ImageUrl %>">
        </div>
        <div class="col-xs-10" style="height: 100%; padding-bottom: 15px; ">
            <% if(Model.ShouldBeImproved){ %>
                <div class='ReasonList'>
                    Ich bitte darum, dass diese Frage verbessert wird, weil:
                    <ul class="fa-ul" style="float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;">
                        <% foreach (var shouldReason in Model.ShouldReasons){ %>
                            <li><i class="fa-li fa fa-repeat" style="float:left;"></i> <%= shouldReason %></li>       
                        <% } %>
                    </ul>
                </div>
            <% } %>

            <% if(Model.ShouldBeDeleted){ %>
                <div class="ReasonList">
                    Ich bitte darum, dass diese Frage gelöscht wird, weil:
                    <ul class="fa-ul" style="float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;">
                        <% foreach (var shouldReason in Model.ShouldReasons){ %>
                            <li><i class="fa-li fa fa-fire" style="float:left; color: tomato;"></i> <%= shouldReason %></li>       
                        <% } %>
                    </ul>
                </div>
            <% } %>            

            <p><%= Model.Text.LineBreaksToBRs() %></p>
        </div>
    </div>
    
    <% foreach(var answer in Model.Answers){ %>
        <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswer.ascx", answer); %>
    <% } %>
    
    <div class="panel-body" style="position: relative">
        <% if(Model.IsLoggedIn){ %>
            <div style="position: absolute; bottom: 8px; right: 20px;">
                <% if (Model.IsInstallationAdmin) { %>
                    <a href="#" class="btnMarkAsSettled btn btn-link" data-type="btn-markAsSettled" style="<%= Html.CssHide(Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als erledigt markieren</a>
                    <a href="#" class="btnMarkAsUnsettled btn btn-link" data-type="btn-markAsUnsettled" style="<%= Html.CssHide(!Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als nicht erledigt markieren</a>
                <% } %>
                <a href="#" class="btnAnswerComment btn btn-link" data-comment-id="<%= Model.Id %>">Antworten</a>
            </div>
        <% } %>    
    </div>
</div>