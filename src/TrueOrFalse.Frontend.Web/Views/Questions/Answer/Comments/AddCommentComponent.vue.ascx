<%@  Control Language="C#" Inherits="ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<add-comment-component inline-template>
<% if (Model.IsLoggedIn)
    { %>
    <div class="panel panel-default" style="margin-top: 7px; border: none;">
        <div class="panel-body">
            <div style="border-top: 1px solid #DDDDDD;">
                <div class="panel-body" style="padding-top: 40px;">
                    <div class="col-xs-2">
                        <img style="border-radius: 50%; max-height:96px;" src="<%= Model.ImageUrlAddComment %>">
                    </div>
                    <div class="col-xs-10">
                        <i class="fa fa-spinner fa-spin hide2" id="saveCommentSpinner"></i>
                        <textarea style="resize: none; width: 100%; background-color: #EFEFEF; min-height: 96px; border: none; border-radius: 0;" class="form-control" id="txtNewComment" placeholder="Neuen Kommentar hinzufügen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a href="#" class="btn btn-secondary memo-button pull-right" style="border: 1px solid #0065CA; color: #0065ca" id="btnSaveComment">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% }
    else
    { %>
    <div class="row" style="margin-bottom: 18px;">
        <div class="col-xs-12" style="padding-top: 10px; color: darkgray">
            Um zu kommentieren, musst du eingeloggt sein. <a @click="showLoginModal">Jetzt anmelden</a>
        </div>
    </div>
<% } %>
</add-comment-component>
