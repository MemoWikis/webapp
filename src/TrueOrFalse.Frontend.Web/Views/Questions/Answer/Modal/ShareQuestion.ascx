<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>

<div id="modalEmbedQuestion" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</h3>
            </div>
            <div class="modal-body">
                <div>
                    <p>
                        Du kannst diese Frage ganz einfach in deine eigene Webseite oder deinen Blog einbetten. 
                        Kopiere einfach die folgende Code-Zeile und füge sie dort ein, wo die Frage erscheinen soll.
                    </p>
                    <p>
                        Beachte, dass du dich in dem Modus befinden musst, in dem HTML erlaubt ist (zum Beispiel bei Wordpress "Text" statt "Visuell").
                    </p>
                    <input type="text" class="form-control" 
                        value="<%: Html.Raw("&lt;script src=&quot;https://memucho.de/views/widgets/question.js&quot; questionId=&quot;" + Model.QuestionId + "&quot; width=&quot;560&quot; height=&quot;315&quot;&gt; &lt;/script&gt;") %>"/>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>
        </div>
    </div>
</div>