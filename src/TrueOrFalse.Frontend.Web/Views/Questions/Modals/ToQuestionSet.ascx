<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="modalToQuestionSet" class="modal show fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3>4 Fragen zu Fragesatz hinzufügen</h3>
    </div>
    <div class="modal-body hide">
        <p>Bitte wähle einen Fragesatz</p>
        
        <div>
            Sie haben noch keine Fragesätze erstellt.
        </div>
    </div>
    <div class="modal-body hide" id="tqsBody">
        <div class="alert-info">
          <strong>Bitte wählen Sie einen Fragesatz</strong> 
        </div>
        <a href="#" class="btn btn-primary hide ">4 Fragen zu "X Fragen zur Tageschau, alles was Sie wissen wollten" hinzufügen</a>

        <div style="padding-top: 7px;" id="tsqRowContainer">
            <div id="tsqRowTemplate" class="tsqRow hide">{Name}</div>
        </div>
    </div>
    <div class="modal-footer hide" id="tqsFooter">
        <a href="#" class="btn" data-dismiss="modal">Schließen</a>
        <a href="#" class="btn btn-primary">Jetzt Fragesatz erstellen</a>
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
