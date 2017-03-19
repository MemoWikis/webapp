<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ShareSetModalModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="modalShareSet" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h4><i class="fa fa fa-code" style="padding-right: 5px;"></i> Fragesatz einbetten.</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-8">
                        Kopiere den Code, um den Fragesatz einzubinden.
                    </div>
                    <div class="col-md-4">
<%--                        <a href="#">
                            <span style="font-size: 10px; float: right; margin-top: 4px;"><i class="fa fa-clipboard" aria-hidden="true"></i> 
                                In Zwischenablage kopieren
                            </span>
                        </a>--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <input id="inputSetEmbedCode" type="text" class="form-control"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>