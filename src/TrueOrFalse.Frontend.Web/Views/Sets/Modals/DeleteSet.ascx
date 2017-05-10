<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalDelete" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">    
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Lernset löschen</h3>
            </div>
            <div class="modal-body">
                <div class="alert alert-danger" id="setDeleteCanDelete">
                    Das Lernset <b>'<span id="spanSetTitle"></span>'</b> wird unwiederbringlich gelöscht. 
                    Alle damit verknüpften Daten werden entfernt, die enthaltenen Fragen werden aber nicht gelöscht. 
                </div>
                <div class="alert alert-danger" id="setDeleteCanNotDelete">
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-default" id="btnCloseSetDelete">Abbrechen</a>
                <a href="#" class="btn btn-primary btn-danger" id="confirmSetDelete">Löschen</a>
            </div>
         </div>
    </div>
</div>