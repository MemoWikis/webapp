<%@  Control Language="C#" Inherits="ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<add-comment-component inline-template>
<% if (Model.IsLoggedIn)
    { %>
    <div class="panel panel-default commentAddContainer">
        <div class="panel-body">
            <div class="commentAnswerAddTopBorder">
                <div class="panel-body commentAnswerAddTopSpace">
                    <div class="col-xs-2">
                        <img class="commentUserImg" src="<%= Model.ImageUrlAddComment %>">
                    </div>
                    <div class="col-xs-10">
                        <i class="fa fa-spinner fa-spin hide2" id="saveCommentSpinner"></i>
                        <textarea class="commentAnswerAddTextArea form-control" id="txtNewComment" placeholder="Neuen Kommentar hinzufügen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a class="btn btn-secondary memo-button pull-right commentAnswerAddSaveBtn" @click="saveComment(<%= Model.QuestionId %>)">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% }
    else
    { %>
    <div class="row commentLoginContainer">
        <div class="col-xs-12 commentLoginText">
            Um zu kommentieren, musst du eingeloggt sein. <a href="#" data-btn-login="true">Jetzt anmelden</a>
        </div>
    </div>
<% } %>
</add-comment-component>
