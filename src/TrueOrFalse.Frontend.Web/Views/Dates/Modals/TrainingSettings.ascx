<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalTraining" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                <button class="close" data-dismiss="modal">×</button>
                <div class="row">
                    <div class="col-md-6">
                        <h3>Übungsplan</h3>
                    </div>
                    <div class="col-md-6">
                        <div class="pull-right" style="font-size: 16px; margin-top: 5px;">
                            <a href="#">zeige Einstellungen <i class="fa fa-cogs"></i></a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-12" style="font-size: 16px;">
                        <span style="padding-right: 12px">Verbleibend: ca. 7 Übungssitzungen </span>
                        <span style="padding-right: 12px"><i class="fa fa-clock-o"></i> ca. 1:20h Übungszeit</span>
                        <br/><span>Zu lernen: 31 Fragen</span>
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Terminvorschläge</h4></div>
                </div>
                <div class="row" style="margin-top: 4px">
                    <div class="col-md-8" style="">
                        Bis Benachrichtigung
                    </div>
                    <div class="col-md-2">Vorher</div>
                    <div class="col-md-2">Nachher</div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div style="height: 1px; background-color: rgb(219, 219, 219);"></div>
                    </div>
                </div>
            </div>
            <div class="modal-footer" style="padding-top: 7px; padding-bottom: 7px;">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schliessen</a>
            </div>
        </div>
    </div>
</div>