<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalCopySet" class="modal fade" data-set-id="<%= Model.Id %>">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Lernset kopieren</h3>
            </div>
            <div class="modal-body">
                <div style="margin-bottom: 20px;">
                    Möchtest du wirklich eine Kopie des Lernsets <b>'<span id="spanCopySetInfo"><%= Model.Name %></span>'</b> von <span id="spanCopySetOwner"><%= Model.CreatorName %></span> erstellen?
                </div>
                <div id="modalCopySetHint">
                    <b>Hinweis:</b> Du solltest das Lernset nur kopieren, wenn du zum Beispiel einzelne Fragen entfernen oder neue Fragen hinzufügen möchtest. 
                    Um das Lernset zu lernen brauchst du es nicht zu kopieren. Füge es einfach deinem <i class="fa fa-heart-o" style="color: #b13a48;">&nbsp;</i>Wunschwissen hinzu.
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-default" data-dismiss="modal" id="btnAbortSetCopy">Abbrechen</button>
                <a href="#" class="btn btn-primary" id="btnConfirmSetCopy">Lernset kopieren</a>
            </div>
        </div>
    </div>
</div>