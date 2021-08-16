<%@ Control Language="C#" Inherits="ViewUserControl<CommentAnswerAddModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<comment-answer-add-component inline-template>
    <div class="panel panel-default" style="margin-top: 7px; border: none; box-shadow: none;">
        <div class="panel-body">
            <div style="border-top: 1px solid #DDDDDD;">
                <div class="panel-body" style="padding-top: 40px;">
                    <div class="col-xs-2">
                        <img style="border-radius: 50%; max-height:96px;" src="<%= Model.AuthorImageUrl%>">
                    </div>
                    <div class="col-xs-10">
                        <textarea style="resize: none; width: 100%; background-color: #EFEFEF; min-height: 96px; border: none; border-radius: 0;" class="form-control" v-model="commentAnswerText" placeholder="Neuen Kommentar hinzufügen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a class="btn btn-secondary memo-button pull-right saveAnswer" style="border: 1px solid #0065CA; color: #0065ca" @click="saveCommentAnswer(<%= Model.ParentCommentId%>)">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</comment-answer-add-component>