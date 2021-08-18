<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<comment-component inline-template  comment-Id-String="<%= Model.Id %>" is-Admin-String="<%= Model.IsInstallationAdmin %>">
    <div class="commentPanel">
        <div class="panel-heading">
            <% if (Model.IsSettled)
                { %>
                <br/>
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
            <% } %>
        </div>
        <div class="panel-body">
            <div class="col-xs-2">
                <img class="commentUserImg" src="<%= Model.ImageUrl %>">
            </div>
            <div class="col-xs-10">
                <div class="commentUserDetails">
                    <a href="<%= Links.UserDetail(Model.Creator) %>" class="commentUserName"><%= Model.CreatorName %></a>
                    <span class="greyed commentDate">
                        vor <span class="cursor-hand show-tooltip" title="erstellt am <%= Model.CreationDate %>"><%= Model.CreationDateNiceText %></span>
                    </span>
                </div>
                <% if (Model.ShouldBeImproved)
                    { %>
                    <div class='ReasonList'>
                        Ich bitte darum, dass diese Frage verbessert wird, weil:
                        <ul class="fa-ul commentModalImproveText">
                            <% foreach (var shouldReason in Model.ShouldReasons)
                                { %>
                                <li>
                                    <i class="fa-li fa fa-repeat commentShouldReasonImprove"></i><%= shouldReason %>
                                </li>
                            <% } %>
                        </ul>
                    </div>
                <% } %>

                <% if (Model.ShouldBeDeleted)
                    { %>
                    <div class="ReasonList">
                        Ich bitte darum, dass diese Frage gelöscht wird, weil:
                        <ul class="fa-ul commentShouldReasonDelete">
                            <% foreach (var shouldReason in Model.ShouldReasons)
                                { %>
                                <li>
                                    <i class="fa-li fa fa-fire commentImproveFire"></i><%= shouldReason %>
                                </li>
                            <% } %>
                        </ul>
                    </div>
                <% } %>
                <% if (!Model.Text.LineBreaksToBRs().Contains("<br>") || Model.Text.Length < 200)
                    { %>
                    <p class="commentText"><%= Model.Text.LineBreaksToBRs() %></p>
                <% } %>
                <%
                    else
                    { %>
                    <span v-if="readMore"><p class="commentText"><%= Model.Text.LineBreaksToBRs()%></p>
                        <a class="cursor-hand" @click="readMore=false">
                            ...Weniger
                        </a>
                    </span>
                    <span v-else><p class="commentText"><%= Model.Text.LineBreaksToBRs().Substring(0, 200) %></p>
                    <a class="cursor-hand" @click="readMore=true">
                        ...Mehr
                    </a>
                    </span>
                <% } %>
            </div>
        </div>

        <% if (!Model.ShowSettledAnswers && (Model.AnswersSettledCount > 0))
            { %>
            <div class="panel-body commentSettledInfo settledCommentsDescription">
                Dieser Kommentar hat <%= Model.AnswersSettledCount %>
                <% if (Model.Answers.Any()) Response.Write("weitere "); %>
                als erledigt markierte Antwort<%= StringUtils.PluralSuffix(Model.AnswersSettledCount, "en") %> (<a href="#" class="showAllAnswersInclSettled" data-comment-id="<%= Model.Id %>">alle anzeigen</a>).
            </div>
        <% } %>
        <div class="panel-body commentRelativeContainer">
            <div class="commentIconsContainer">
                <a v-if="false" class="commentThumbsIcon">
                    <i class="fa fa-thumbs-up" aria-hidden="true"></i> &nbsp 0
                </a>
                <span class="commentSpeechBubbleIcon">
                <i class="fa fa-comments-o" aria-hidden="true"></i> &nbsp <%= Model.Answers.Count() %>
                </span>
            </div>
            <% if (Model.IsLoggedIn)
                { %>
                <div class="commentMarkAsSettledContainer">

                    <a v-if="isInstallationAdmin && !settled" @click="markAsSettled(<%= Model.Id %>)" href="#" class="btnAnswerComment btn btn-link commentMarkAsSettled commentFooterText" data-comment-id="<%= Model.Id %>">
                        <i class="fa fa-check" aria-hidden="true"></i>
                        Als Erledigt Markieren
                    </a>
                    <a v-if="isInstallationAdmin && settled" @click="markAsUnsettled(<%= Model.Id %>)" href="#" class="btnAnswerComment btn btn-link commentMarkAsSettled commentFooterText" data-comment-id="<%= Model.Id %>">
                        <i class="fa fa-check" aria-hidden="true"></i>
                        Als nicht Erledigt Markieren
                    </a>
                    <a @click="showAnsweringPanel = true" class="btnAnswerComment btn btn-link commentFooterText" >Antworten</a>
                </div>

            <% } %>
        </div>
        <div class="commentAnswers">

            <% foreach (var answer in Model.Answers)
               { %>
                <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswer.vue.ascx", answer); %>
            <% } %>

            <div v-for="answer in addedAnswers">
                <div v-html="answer"></div>
            </div>
        </div>
        <% if (Model.IsLoggedIn)
           { %>
        <div v-if="showAnsweringPanel" >
            <% var answerAddModel = new CommentAnswerAddModel();
               answerAddModel.AuthorImageUrl = Model.ImageUrl;
               answerAddModel.ParentCommentId = Model.Id;
               Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswerAdd.vue.ascx", answerAddModel); %>
        </div>
        <% } %>
    </div>
</comment-component>
