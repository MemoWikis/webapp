<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalDeleteQuestion" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Frage löschen</h3>
            </div>
            <div class="modal-body">
                <div class="alert alert-danger" id="questionDeleteCanDelete">
                    Die Frage <b>'<span id="spanQuestionTitle"></span>'</b> wird unwiederbringlich gelöscht. Alle damit verknüpften Daten werden entfernt! 
                </div>
                <div class="alert alert-danger" id="questionDeleteCanNotDelete">
                </div>
                <div class="alert alert-info" id="questionDeleteResult">
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default memo-button" id="btnCloseQuestionDelete">Abbrechen</a>
                <a href="#" class="btn btn-primary btn-danger memo-button" id="confirmQuestionDelete">Löschen</a>
            </div>
        </div>
    </div>
</div>