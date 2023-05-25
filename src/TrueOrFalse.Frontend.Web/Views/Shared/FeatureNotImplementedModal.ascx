<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="FeatureNotImplementedModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Diese Funktion ist noch nicht umgesetzt</h3>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2" style="text-align: center;">
                        <img src="/Images/Logo/memucho_MEMO_angry_blau_w70.png"/>
                    </div>
                    <div class="col-md-10">
                        <p>
                            memucho ist noch in der Beta-Phase, daher ist noch nicht alles fertig. Das tut uns leid, aber wir erweitern memucho ständig und du kannst uns gerne dabei helfen. 
                            und <a href="#" onclick="_urq.push(['Feedback_Open']);">schicke uns dein Feedback</a>.
                        </p>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-primary" data-dismiss="modal" style="width: 80px; max-width: 100%;">Ok</a>
            </div>
        </div>
    </div>
</div>