<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalDeleteCategory" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Thema löschen</h3>
            </div>
            <div class="modal-body">
                <div class="alert alert-danger">
                    Das Thema <b>'<span id="spanCategoryTitle"></span>'</b> wird unwiederbringlich gelöscht. 
                    Alle damit verknüpften Daten werden entfernt (Fragen werden nicht gelöscht)! 
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" id="btnCloseDelete">Schließen</a>
                <a href="#" class="btn btn-primary btn-danger" id="confirmDelete">Löschen</a>
            </div>
        </div>
    </div>
</div>