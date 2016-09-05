<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>

<div id="modalEmbedQuestion" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Einbetten</h3>
            </div>
            <div class="modal-body">
                <div>
                    <input type="text" class="form-control" 
                        value="<%: Html.Raw("&lt;script src=&quot;https://memucho.de/views/widgets/question.js&quot; questionId=&quot;" + Model.QuestionId + "&quot; width=&quot;560&quot; height=&quot;315&quot;/&gt;") %>"/>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>
        </div>
    </div>
</div>