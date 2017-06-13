<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LevelPopupModel>" ContentType="text/xml" %>

<div id="levelPopupModal" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Fortschritt: Du bist jetzt Level <b><%= Model.UserLevel %></b></h4>
            </div>
            <div class="modal-body">
                <% if (Model.IsLoggedIn)
                   { %>
                Super! Du wirst immer schlauer!
                <% }
                   else
                   { %>
                Wenn du jetzt einfach die Seite, verlässt verlierst du deine ActivityPoints und das eben errungene Level. <br/>
                Melde dich jetzt an und werde immer schlauer mit memucho.de!
                 <% } %>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Okay</button>
            </div>
        </div>
    </div>
</div>