<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="modalToQuestionSet" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h3 class="modal-title" id="tqsTitle"></h3>
            </div>
            <div class="modal-body hide2">
                <p>Bitte wähle einen Fragesatz</p>
        
                <div>
                    Sie haben noch keine Fragesätze erstellt.
                </div>
            </div>
            <div class="modal-body hide2" id="tqsBody">
                <div id="tqsTextSelectSet" class="alert-info" style="padding: 5px;">
                  <strong>Bitte wähle einen Deiner Fragesätze</strong> 
                </div>
                <div style="margin-top: 11px;">
                    <b>Filter:</b> &nbsp;<input type="text" style="width: 210px;" id="txtTqsSetFilter" />
                    <span id="tqsSetCount"></span>
                </div>
                <div style="padding-top: 7px;" id="tsqRowContainer">
                    <div id="tsqRowTemplate" class="tsqRow hide2"><b>{Name}</b> ({AnzahlFragen} Fragen)</div>
                </div>
            </div>
            

            <div class="modal-body hide2" id="tqsSuccess">
                <div class="alert-success" style="padding: 5px;">
                    <strong id="tqsSuccessMsg">
                        {Amount} Fragen wurden zum Fragesatz "{SetName}" hinzugefügt. {NonAdded}
                    </strong>
                </div>
            </div> 
            <div class="modal-footer hide2" id="tqsSuccessFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>    

            <div class="modal-body hide2" id="tqsNoSetsBody">
                <div class="alert">
                  <strong>Noch keine Fragesätze angelegt.</strong> 
                    Um Fragen zu Fragesätzen hinzufügen zu können, erstelle jetzt Deinen ersten Fragesatz: 
                </div>        
            </div>
            <div class="modal-footer hide2" id="tqsNoSetsFooter">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
                <a href="/QuestionSet/Create" class="btn btn-primary">Jetzt Fragesatz erstellen</a>
            </div>
        </div>
    </div>
</div>
