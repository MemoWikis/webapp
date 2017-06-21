<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LevelPopupModel>" ContentType="text/xml" %>


<div id="levelPopupModal" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                        <span class="title-text"><b>Fortschritt:</b> Du bist jetzt Level </span>
                        <span class="level-display">
                            <svg>
                                <circle cx="50%" cy="50%" r="15" />
                                <text class="level-count" x="50%" y="50%" dy = ".34em" ><%= Model.UserLevel %></text>
                            </svg>
                        </span>
                    </h4>
                <% if (Model.IsLoggedIn) { %>
                    <h4 class="title-text">Super! Du wirst immer schlauer.</h4>
                <% } %>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-xs-8">
                <% if (Model.IsLoggedIn)
                   { %>
                        Das Nächste Level erreichst du bei <b><%= Model.PointsToNextLevel %></b> Punkten.
                <% }
                   else
                   { %>
                    Wenn Du jetzt die Seite verlässt, verlierst du deine ActivityPoints und
                    das eben errungene Level. <br/>
                    <b>Melde dich jetzt an und werde immer schlauer mit memucho.de!</b>
                 <% } %>
                        </div>
                    <div class="col-xs-4">
                        <object data="~/Images/memucho_MEMO_happy_blau.svg" type="image/svg+xml">
                            Ihr Browser kann leider kein svg darstellen!
                        </object>
                    </div>
                        </div>
            </div>
            <div class="modal-footer">
                <% if (Model.IsLoggedIn)
                   { %>
                        <button type="button" class="btn" data-dismiss="modal">WEITER LERNEN</button>
                <% }
                   else
                   { %>
                        <button type="button" class="btn" data-dismiss="modal">WEITER TESTEN</button>
                        <button type="button" class="btn btn-primary" data-dismiss="modal">REGISTRIEREN</button>
                <% } %>
            </div>
        </div>
    </div>
</div>