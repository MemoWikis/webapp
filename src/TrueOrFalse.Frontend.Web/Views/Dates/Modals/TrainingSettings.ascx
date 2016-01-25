﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalTraining" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                <button class="close" data-dismiss="modal">×</button>
                <div class="row">
                    <div class="col-md-12">
                        <h3 style="font-size: 22px; margin-top: 0px; display: inline-block;">
                            Übungsplan
                            <select style="font-size: 15px; padding-left: 3px;">
                                <option>Klassenarbeit am 24.12.2015</option>
                                <option>Mündliche Prüfung am Fr.</option>
                            </select>
                        </h3>
                        <div class="pull-right" style="font-size: 15px; margin-top: 5px;">
                            <a href="#" id="showSettings" data-action="showSettings">zeige Einstellungen <i class="fa fa-cogs"></i></a>
                            <a href="#" style="display:none" id="closeSettings" data-action="closeSettings">verberge Einstellungen <i class="fa fa-cogs"></i></a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-body" style="padding-top: 0px;">
                <div id="settings" style="display:none">
                    <div class="row" style="margin-top: 0px;">
                        <div class="col-md-12">
                            <h4 class="ColoredUnderline Date">
                                Einstellungen
                                <a href="#" class="pull-right" style="font-size: 11px;" data-action="closeSettings">x schließen</a>
                            </h4>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Lernstrategie:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <select>
                                <option>Nachhaltig lernen</option>
                                <option>Minimaler Aufwand</option>
                            </select>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Ruhezeiten:</div>
                        </div>
                        <div class="col-md-8" style="padding-left: 0px">
                            jeden Tag 22:00-8:00 u. an Sonntagen <i class="fa fa-plus-square-o"></i>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4"></div>
                        <div class="col-md-8" style="padding-left: 0px">
                            <a href="#">erweiterte Einstellungen verbergen <i class="fa fa-caret-down"></i></a>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 12px;">
                        <div class="col-md-4">
                            <div class="pull-right">Fragen pro Termin Ideal:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="10"/>
                        </div>
                        <div class="col-md-4">
                            <div class="pull-right">Vergessensschwelle:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="92"/>%
                        </div>
                    </div>
                    <div class="row" style="margin-top: 4px;">
                        <div class="col-md-4">
                            <div class="pull-right">Mind. pro Termin:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="7"/>
                        </div>
                        <div class="col-md-4">
                            <div class="pull-right">mind. Zeit zw. Terminen:</div>
                        </div>
                        <div class="col-md-2" style="padding-left: 0px">
                            <input type="text" style="width: 30px; height: 20px; font-size: 13px;" value="180"/>min
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Übersicht</h4></div>
                </div>
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
                <div id="dateRows"></div>
            </div>
            <div class="modal-footer" style="padding-top: 7px; padding-bottom: 7px;">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schliessen</a>
            </div>
        </div>
    </div>
</div>