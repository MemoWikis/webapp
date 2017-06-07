<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<LevelPopupModel>" ContentType="text/xml" %>

<div id="levelPopupModal" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Yaay Level errungen: <b><%= Model.UserLevel %></b></h4>
            </div>
            <div class="modal-body">
                <% if (Model.IsLoggedIn)
                   { %>
                Super! Du wirst immer schlauer!
                <% }
                   else
                   { %>
                Hi ich bin Robert und ich wünsche mir, dass du noch auf dieser Seite bleibst und dich anmeldest. <br/>
                Wenn du das nicht tust verlierst du deine ActivityPoints und das eben errungene Level. <br/>
                Außerdem verliere ich dann jegliche Achtung vor dir und du deine menschliche Würde!                  
                 <% } %>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Okay</button>
            </div>
        </div>
    </div>
</div>