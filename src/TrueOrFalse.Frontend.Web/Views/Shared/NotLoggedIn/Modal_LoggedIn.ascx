<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalNotLoggedIn" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h4><i class="fa fa-power-off"></i> Du bist nicht eingeloggt.</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">Um diese Funktion zu nutzen, musst du eingeloggt sein. <br />
                        Jetzt <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>!
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn btn-default" id="btnCloseDateDelete">Schließen</a>
            </div>
        </div>
    </div>
</div>