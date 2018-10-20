<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalCopyDate" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Termin übernehmen</h3>
            </div>
            <div class="modal-body">
                <div>
                    Möchtest du den Termin <b>'<span id="spanCopyDateInfo"></span>'</b> von <span id="spanCopyDateOwner"></span> wirklich übernehmen?
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" id="btnCloseDateCopy">Abbrechen</a>
                <a href="#" class="btn btn-primary" id="btnConfirmDateCopy">Termin übernehmen</a>
            </div>
        </div>
    </div>
</div>