<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalDelete" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Termin löschen</h3>
            </div>
            <div class="modal-body">
                <div class="alert alert-danger">
                    Der Termin <b>'<span id="spanDeleteDateInfo"></span>'</b> wird gelöscht.
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" id="btnCloseDateDelete">Abbrechen</a>
                <a href="#" class="btn btn-primary btn-danger" id="btnConfirmDateDelete">Löschen</a>
            </div>
        </div>
    </div>
</div>