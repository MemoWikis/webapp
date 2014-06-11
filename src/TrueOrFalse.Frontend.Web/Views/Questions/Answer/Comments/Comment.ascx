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
        <div class="col-xs-2">
            <img style="border-radius:5px;" src="<%= Model.ImageUrl %>">
        </div>
        <div class="col-xs-10" style="height: 100%; padding-bottom: 25px; ">
            <% if(Model.ShouldBeImproved){ %>
                <div>
                    <i class="fa fa-repeat show-tooltip" style="float:left" title="Die Frage sollte verbessert werden"></i>&nbsp;
                    <ul style="float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;">
                        <% foreach (var shouldReason in Model.ShouldReasons){ %>
                            <li><%= shouldReason %></li>       
                        <% } %>
                    </ul>
                    <div class="clearfix"></div>
                </div>
            <% } %>
            <%= Model.Text.LineBreaksToBRs() %>
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