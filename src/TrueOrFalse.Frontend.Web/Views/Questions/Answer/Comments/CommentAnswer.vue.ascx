<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<comment-answer-component inline-template id-string="<%= Model.Id %>" is-settled-string="<%= Model.IsSettled %>">
    <div class="panel-body" style="position: relative">
        <div class="col-xs-2">
            <img class="pull-right" style="max-height: 48px; border-radius:50%;" src="<%= Model.ImageUrl %>">
        </div>
        <div class="col-xs-10" style="height: 100%; padding-bottom: 15px; ">
            <div>
                <span style="color:darkgray">
                    <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a>
                    <span style="padding-left: 5px;">vor <%= Model.CreationDateNiceText%></span>
                </span>
                <% if (Model.IsSettled) { %>
                    <br/><span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
                <% } %>
            </div>
            <div style="margin-top: 10px;">
                <%= Model.Text.LineBreaksToBRs() %>    
            </div>
            <div>
                <% if (Model.IsInstallationAdmin) { %>
                    <a v-if="isSettled" class="btnMarkAsUnsettled btn btn-sm btn-link" data-type="btn-markAsUnsettled" style="padding-left: 0; margin-left: 0; <%= Html.CssHide(!Model.IsSettled) %>">Als nicht erledigt markieren</a>
                    <a v-else class="btnMarkAsSettled btn btn-sm btn-link" style="padding-left: 0; margin-left: 0;" @click="markAsSettled()">Als erledigt markieren</a>
                <% } %>
            </div>
        </div>
    </div>
</comment-answer-component>