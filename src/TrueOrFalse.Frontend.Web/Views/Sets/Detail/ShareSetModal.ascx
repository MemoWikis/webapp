<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ShareSetModalModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="modalShareSet" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h4><i class="fa fa fa-code" style="padding-right: 5px;"></i> Fragesatz als Widget einbetten.</h4>
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
                        <input id="inputSetEmbedCode" type="text" class="form-control" style="width: 100%"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="padding-top: 15px;">
                        <a href="#" data-action="showSettings">Einstellungen zeigen <i class="fa fa-caret-down"></i></a>                        
                    </div>
                    <div class="col-md-12 hide2" style="padding-top: 15px;">
                        <a href="#" data-action="hideSettings">Einstellungen verbergen <i class="fa fa-caret-up"></i></a>                        
                    </div>
                </div>

                <div id="divShareSetSettings" class="hide2">
                    <div class="row form-inline" style="margin-bottom: 10px;">
                        <div class="col-sm-6" >
                            <div class="form-group" style="padding-top: 15px;">
                                <label for="widgetWidth">Breite</label>
                                <input type="text" class="form-control" id="widgetWidth" style="width: 62px;" value="100">
                                <select class="form-control" id="widgetWidthUnit">
                                    <option>%</option>
                                    <option>px</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group" style="padding-top: 15px;">
                                <label class="checkbox-inline" for="ckbEnableMaxWidth">
                                    <input type="checkbox" id="ckbEnableMaxWidth" checked="checked"> max Breite
                                </label>                        
                                <input type="text" id="widgetMaxWidth" class="form-control" style="width: 62px;" value="600">
                                <label>px</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="padding-top: 10px;">
                            <label class="checkbox-inline">
                                <input type="checkbox" id="ckbHideKnowledgeBtn"> verberge Wunschwissen Button
                            </label>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="margin-top: 18px; margin-bottom: 7px; border-bottom: 1px solid #e5e5e5;">
                        <h4>Vorschau auf das Fragesatz-Widget:</h4>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" style="padding-top: 10px;" id="divPreviewSetWidget">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>