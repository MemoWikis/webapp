<%@ Control Language="C#" Inherits="ViewUserControl<CommentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<comment-component inline-template>
    <div style="margin-top: 7px; border-top: 1px solid #DDDDDD;">
        <div class="panel-heading">

            <% if (Model.IsSettled)
                { %>
                <br/>
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
            <% } %>
        </div>
        <div class="panel-body" style="position: relative">
            <div class="col-xs-2" style="">
                <img style="border-radius: 50%; max-height: 96px;" src="<%= Model.ImageUrl %>">
            </div>
            <div class="col-xs-10" style="height: 100%;">
                <div style="padding-bottom: 12px;">
                    <a href="<%= Links.UserDetail(Model.Creator) %>" style="font-size: 18px;"><%= Model.CreatorName %></a>
                    <span class="greyed" style="font-size: 12px;">
                        vor <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>"><%= Model.CreationDateNiceText %></span>
                    </span>
                </div>
                <% if (Model.ShouldBeImproved)
                    { %>
                    <div class='ReasonList'>
                        Ich bitte darum, dass diese Frage verbessert wird, weil:
                        <ul class="fa-ul" style="float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;">
                            <% foreach (var shouldReason in Model.ShouldReasons)
                                { %>
                                <li>
                                    <i class="fa-li fa fa-repeat" style="float: left;"></i><%= shouldReason %>
                                </li>
                            <% } %>
                        </ul>
                    </div>
                <% } %>

                <% if (Model.ShouldBeDeleted)
                    { %>
                    <div class="ReasonList">
                        Ich bitte darum, dass diese Frage gelöscht wird, weil:
                        <ul class="fa-ul" style="float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;">
                            <% foreach (var shouldReason in Model.ShouldReasons)
                                { %>
                                <li>
                                    <i class="fa-li fa fa-fire" style="float: left; color: tomato;"></i><%= shouldReason %>
                                </li>
                            <% } %>
                        </ul>
                    </div>
                <% } %>
                <% if (!Model.Text.LineBreaksToBRs().Contains("<br>") || Model.Text.Length < 200)
                    { %>
                    <p style="text-overflow: ellipsis; overflow: hidden; width: 634px; max-height: 60px; white-space: nowrap; -webkit-line-clamp: 4;"><%= Model.Text.LineBreaksToBRs() %></p>
                <% } %>
                <%
                    else
                    { %>
                    <span v-if="readMore"><p style="width: 634px;"><%= Model.Text.LineBreaksToBRs()%></p>
                        <a class="" @click="readMore=false" style="cursor: pointer;">
                            ...Weniger
                        </a>
                    </span>
                    <span v-else><p><%= Model.Text.LineBreaksToBRs().Substring(0, 200) %></p>
                    <a class="" @click="readMore=true" style="cursor: pointer;">
                        ...Mehr
                    </a>
                    </span>
                <% } %>
            </div>
        </div>
        <div class="commentAnswers" style="margin-top: 40px;">

            <% foreach (var answer in Model.Answers)
                { %>
                <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswer.vue.ascx", answer); %>
            <% } %>
        </div>
        <% if (!Model.ShowSettledAnswers && (Model.AnswersSettledCount > 0))
            { %>
            <div class="panel-body commentSettledInfo" style="text-align: right;">
                Dieser Kommentar hat <%= Model.AnswersSettledCount %>
                <% if (Model.Answers.Any()) Response.Write("weitere "); %>
                als erledigt markierte Antwort<%= StringUtils.PluralSuffix(Model.AnswersSettledCount, "en") %> (<a href="#" class="showAllAnswersInclSettled" data-comment-id="<%= Model.Id %>">alle anzeigen</a>).
            </div>
        <% } %>
        <div class="panel-body" style="position: relative">
            <div style="position: absolute; bottom: 8px; left: 20px;">
                <a v-if="false" style="font-size: 18px; color: #999999; padding-right: 24px; padding-left: 156px;">
                    <i class="fa fa-thumbs-up" aria-hidden="true"></i> &nbsp 0
                </a>
                <span style="font-size: 18px; color: #999999;">
                <i class="fa fa-comments-o" aria-hidden="true"></i> &nbsp <%= Model.Answers.Count() %>
                </span>
            </div>
            <% if (Model.IsLoggedIn)
                { %>
                <div style="position: absolute; bottom: 8px; right: 20px;">
                    <% if (Model.IsInstallationAdmin)
                        { %>
                        <a href="#" class="btnMarkAsSettled btn btn-link" data-type="btn-markAsSettled" style="<%= Html.CssHide(Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als erledigt markieren</a>
                        <a href="#" class="btnMarkAsUnsettled btn btn-link" data-type="btn-markAsUnsettled" style="<%= Html.CssHide(!Model.IsSettled) %>" data-comment-id="<%= Model.Id %>">Als nicht erledigt markieren</a>
                    <% } %>
                    <a href="#" class="btnAnswerComment btn btn-link" style="font-size: 14px; font-weight: 400;" data-comment-id="<%= Model.Id %>">
                        <i class="fa fa-check" aria-hidden="true"></i>
                        Als Erledigt Markieren
                    </a>
                    <a @click="showAnsweringPanel=true" class="btnAnswerComment btn btn-link" style="font-size: 14px; font-weight: 400;" data-comment-id="<%= Model.Id %>">Antworten</a>
                </div>

            <% } %>
        </div>
        <div v-if="showAnsweringPanel" class="comment-answer-add">
            <% Html.RenderPartial("~/Views/Questions/Answer/Comments/CommentAnswerAdd.ascx", Model); %>
        </div>   
        </div>

</comment-component>
