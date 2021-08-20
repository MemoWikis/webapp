<%@ Control Language="C#" Inherits="ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<div id="CommentsSection">
    <comments-section-component inline-template>
        <div>

           <% foreach (var comment in Model.Comments)
               { %>
        <div class="comment">
            <% Html.RenderPartial("~/Views/Questions/Answer/Comments/Comment.vue.ascx", comment); %>
        </div>
    <% } %>
            <div v-for="comment in addedComments">
                <div v-html="comment"></div>
            </div>
            <div>
            </div>  


    <% if (Model.CommentsSettledCount > 0)
        { %>
                <div v-if="showSettledComments" class="commentSettledInfo">
                    Diese Frage hat <%= Model.CommentsSettledCount %>
                    <% if (Model.Comments.Any()) Response.Write("weitere "); %>
                    als erledigt markierte<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "", "n") %> Kommentar<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "e") %>
                    (<a class="cursor-hand" @click="showSettledComments = false" data-question-id="<%= Model.QuestionId %>">alle verstecken</a>).
                </div>
                <div v-else class="commentSettledInfo">
                    Diese Frage hat <%= Model.CommentsSettledCount %>
                    <% if (Model.Comments.Any()) Response.Write("weitere "); %>
                    als erledigt markierte<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "", "n") %> Kommentar<%= StringUtils.PluralSuffix(Model.CommentsSettledCount, "e") %>
                    (<a class="cursor-hand" @click="showSettledComments = true" data-question-id="<%= Model.QuestionId %>">alle anzeigen</a>).
                </div>
                <div v-if="showSettledComments">

                    <% foreach (var settledComment in Model.SettledComments)
                        {%>
                        <div class="comment">
                            <% Html.RenderPartial("~/Views/Questions/Answer/Comments/Comment.vue.ascx", settledComment); %>
                        </div>  
                    <% } %>
        </div>
    <% } %>
    <div class="addCommentComponent">
        <% Html.RenderPartial("~/Views/Questions/Answer/Comments/AddCommentComponent.vue.ascx", Model); %>
    </div>
        </div>
    </comments-section-component>

</div>
<%= Scripts.Render("~/bundles/js/CommentsSection") %>
<%= Scripts.Render("~/bundles/js/defaultModal") %>