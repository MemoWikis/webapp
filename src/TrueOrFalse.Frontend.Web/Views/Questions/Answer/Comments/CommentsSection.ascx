<%@ Control Language="C#" Inherits="ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<div id="CommentsSection">
    <% foreach (var comment in Model.Comments)
       { %>
        <div class="comment">
            <% Html.RenderPartial("~/Views/Questions/Answer/Comments/Comment.vue.ascx", comment); %>
        </div>
    <% } %>
    <% if (Model.CommentsSettledCount > 0)
       { %>
        <div class="commentSettledInfo" style="margin: 5px 10px 15px;">
            Diese Frage hat <%= Model.CommentsSettledCount %>
            <% if (Model.Comments.Any()) Response.Write("weitere "); %>
            als erledigt markierte<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "", "n") %> Kommentar<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "e") %>
            (<a href="#" id="showAllCommentsInclSettled" data-question-id="<%= Model.QuestionId %>">alle anzeigen</a>).
        </div>
    <% } %>
    <div class="addCommentComponent">
        <% Html.RenderPartial("~/Views/Questions/Answer/Comments/AddCommentComponent.vue.ascx", Model); %>
    </div>

</div>
<%= Scripts.Render("~/bundles/js/CommentsSection") %>