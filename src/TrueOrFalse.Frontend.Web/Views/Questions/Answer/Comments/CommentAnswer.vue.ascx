<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<comment-answer-component inline-template id-string="<%= Model.Id %>" is-settled-string="<%= Model.IsSettled %>">
    <div class="panel-body commentRelativeContainer">
        <div class="col-xs-2">
            <img class="pull-right answerUserImage" src="<%= Model.ImageUrl %>">
        </div>
        <div class="col-xs-10 commentUserDetails">
            <div>
                <span>
                    <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a>
                    <span class="commentUserDetails">vor <%= Model.CreationDateNiceText%></span>
                </span>
                <% if (Model.IsSettled) { %>
                    <br/><span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
                <% } %>
            </div>
            <div class="answerTextContainer">
                <%= Model.Text.LineBreaksToBRs() %>    
            </div>
            <div>
<%--                <% if (Model.IsInstallationAdmin) { %>
                    <a v-if="isSettled" class="btnMarkAsUnsettled btn btn-sm btn-link answerFooter" data-type="btn-markAsUnsettled" style="<%= Html.CssHide(!Model.IsSettled) %>">Als nicht erledigt markieren</a>
                    <a v-else class="btnMarkAsSettled btn btn-sm btn-link answerFooter" @click="markAsSettled()">Als erledigt markieren</a>
                <% } %>--%>
            </div>
        </div>
    </div>
</comment-answer-component>
