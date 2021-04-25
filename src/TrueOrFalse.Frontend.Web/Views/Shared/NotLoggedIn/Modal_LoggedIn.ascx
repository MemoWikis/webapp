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
                    <div class="col-md-12">Um diese Funktion zu nutzen, musst du eingeloggt sein. <br/>
                        Wenn du noch kein Nutzer bist, registriere dich jetzt.
                        <span style="font-style: italic;">memucho ist kostenlos.</span>
                        <%--<br /> Jetzt <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>!--%>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn btn-secondary memo-button" id="btnCloseDateDelete">Jetzt nicht, danke</a>
                <a href="#" data-btn-login="true" class="btn btn-secondary memo-button" id=""><i class="fa fa-sign-in">&nbsp;</i>Einloggen</a>
                <a href="/Registrieren" class="btn btn-success memo-button" id="btnCloseDateDelete"><i class="fa fa-chevron-circle-right">&nbsp;&nbsp;</i>Jetzt Registrieren</a>
            </div>
        </div>
    </div>
</div>