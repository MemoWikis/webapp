﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalDelete" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Kategorie löschen</h3>
            </div>
            <div class="modal-body">
                <div class="alert alert-error">
                    Die Kategorie <b>'<span id="spanCategoryTitle"></span>'</b> wird unwiederbringlich gelöscht. 
                    Alle damit verknüpften Daten werden entfernt! 
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn" id="btnCloseDelete">Schliessen</a>
                <a href="#" class="btn btn-primary btn-danger" id="confirmDelete">Löschen</a>
            </div>
        </div>
    </div>
</div>