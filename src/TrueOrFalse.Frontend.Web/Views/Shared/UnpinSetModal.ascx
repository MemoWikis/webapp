<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="UnpinSetModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h4>Fragesatz aus Wunschwissen entfernen</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12">
                        Der Fragesatz <span id="SetName"></span> wird aus deinem Wunschwissen entfernt.
                        Die folgenden enthaltenen Fragen werden ebenfalls aus deinem Wunschwissen gelöscht:
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="#" id="JS-DontRemoveQuestions" class="SecAction" data-dismiss="modal">Nein, die Fragen sollen im Wunschwissen bleiben.</a>
                <a href="#" id="JS-RemoveQuestions" class="btn btn-primary" data-dismiss="modal">Ok</a>
            </div>
        </div>
    </div>
</div>