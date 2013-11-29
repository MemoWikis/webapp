<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="modalToQuestionSet" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h3 id="tqsTitle"></h3>
            </div>
            <div class="modal-body hide">
                <p>Bitte wähle einen Fragesatz</p>
        
                <div>
                    Sie haben noch keine Fragesätze erstellt.
                </div>
            </div>
            <div class="modal-body hide" id="tqsBody">
                <div id="tqsTextSelectSet" class="alert-info" style="padding: 5px;">
                  <strong>Bitte wähle einen Fragesatz</strong> 
                </div>
                <div style="padding-top: 7px;" id="tsqRowContainer">
                    <div id="tsqRowTemplate" class="tsqRow hide">{Name}</div>
                </div>
            </div>
            <div class="modal-body hide" id="tqsSuccess">
                <div class="alert-success" style="padding: 5px;">
                  <strong>2 Fragen wurden zum Fragesatz "asdfasdf" hinzugefügt</strong>
                </div>
            </div> 
            <div class="modal-footer hide" id="tqsSuccessFooter">
                <a href="#" class="btn" data-dismiss="modal">Schließen</a>
            </div>    

            <div class="modal-body hide" id="tqsNoSetsBody">
                <div class="alert">
                  <strong>Noch keine Fragesätze angelegt.</strong> 
                    Um Fragen zu Fragesätzen hinzufügen zu können, erstelle jetzt Deinen ersten Fragesatz: 
                </div>        
            </div>
            <div class="modal-footer hide" id="tqsNoSetsFooter">
                <a href="#" class="btn" data-dismiss="modal">Schließen</a>
                <a href="/QuestionSet/Create" class="btn btn-primary">Jetzt Fragesatz erstellen</a>
            </div>
        </div>
    </div>
</div>
