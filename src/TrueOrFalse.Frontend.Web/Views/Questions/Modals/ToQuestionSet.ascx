<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="modalToQuestionSet" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h3 class="modal-title" id="tqsTitle"></h3>
            </div>

            <div class="modal-body hide2" id="tqsBody">
                <div id="tqsTextSelectSet" class="">
                  <strong>Bitte wähle eines deiner Lernsets:</strong> 
                </div>
                <div style="margin-top: 11px;">
                    Filter:&nbsp;<input type="text" style="width: 210px;" id="txtTqsSetFilter" />
                    <span id="tqsSetCount"></span>
                </div>
                <div style="padding-top: 7px;" id="tqsRowContainer">
                    <div id="tqsRowTemplate" class="tqsRow hide2"><b>{Name}</b> ({AnzahlFragen} Fragen)</div>
                </div>
            </div>
            <div class="modal-footer hide2" id="tqsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Abbrechen</a>
            </div>
            
            <div class="modal-body hide2" id="tqsNoQuestionsSelectedBody">
                <div class="row">
                    <div class="col-md-4" style="text-align: center;">
                        <img src="/Images/Screenshots/fragen-zu-fragesatz.png"/>
                    </div>
                    <div class="col-md-8">
                        &nbsp;<br/>
                        <p>Bitte wähle zuerst die Fragen aus, die du zu einem Lernset hinzufügen möchtest.</p>
                        <p>Gehe dazu mit der Maus über die obere Hälfte des Bildes neben der Frage und aktiviere die Checkbox oder benutze die Auswahl-Schaltfläche.</p>
                    </div>       
                </div>                     
            </div>
            <div class="modal-footer hide2" id="tqsNoQuestionsSelectedFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>

            <div class="modal-body hide2" id="tqsSuccess">
                <div class="alert-success" style="padding: 5px;">
                    <strong id="tqsSuccessMsg">
                        {Amount} Fragen wurden zum Lernset "{SetName}" hinzugefügt. {NonAdded}
                    </strong>
                </div>
            </div> 
            <div class="modal-footer hide2" id="tqsSuccessFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>    

            <div class="modal-body hide2" id="tqsNoSetsBody">
                <div class="">
                  <strong>Du hast noch keine Lernsets angelegt.</strong> 
                    Um Fragen zu Lernsets hinzufügen zu können, musst du zuerst ein eigenes Lernset erstellen. 
                </div>        
            </div>
            <div class="modal-footer hide2" id="tqsNoSetsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                <a href="/Fragesaetze/Erstelle" class="btn btn-primary">Jetzt Lernset erstellen</a>
            </div>

        </div>
    </div>
</div>
