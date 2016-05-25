<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modalTraining" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="padding-top: 5px; padding-bottom: 5px;">
                <button class="close" data-dismiss="modal">×</button>
                <div class="row">
                    <div class="col-md-12">
                        <h3 style="font-size: 22px; margin-top: 0px; display: inline-block;">
                            Übungsplan
                            <select style="font-size: 15px; padding-left: 3px;" id="SelectTrainingDates">
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
                            <a href="#" data-action="showAdvancedSettings">erweiterte Einstellungen zeigen <i class="fa fa-caret-down"></i></a>
                            <a href="#" data-action="hideAdvancedSettings" style="display: none">erweiterte Einstellungen verbergen <i class="fa fa-caret-up"></i></a>
                        </div>
                    </div>

                    <div id="divAdvancedSettings" style="display: none">
                        <div class="row" style="margin-top: 12px;">
                            <div class="col-md-4">
                                <div class="pull-right">Fragen pro Termin Ideal:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtQuestionsPerDateIdealAmount" style="width: 30px; height: 20px; font-size: 13px;" value="10"/>
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right">Vergessensschwelle:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtAnswerProbabilityThreshold" style="width: 30px; height: 20px; font-size: 13px;" value="92"/>%
                            </div>
                        </div>
                        <div class="row" style="margin-top: 4px;">
                            <div class="col-md-4">
                                <div class="pull-right">Mind. pro Termin:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtQuestionsPerDateMinimum" style="width: 30px; height: 20px; font-size: 13px;" value="7"/>
                            </div>
                            <div class="col-md-4">
                                <div class="pull-right">mind. Zeit zw. Terminen:</div>
                            </div>
                            <div class="col-md-2" style="padding-left: 0px">
                                <input type="text" id="txtSpacingBetweenSessionsInMinutes" style="width: 30px; height: 20px; font-size: 13px;" value="180"/>min
                            </div>
                        </div>                        
                    </div>

                </div>
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Übersicht</h4></div>
                </div>
                <div class="row">
                    <div class="col-md-6" style="font-size: 16px;">
                        <p><span id="QuestionCount"></span> Fragen sind zu lernen<p/>
                        Verbleibender Lernaufwand:<br/>
                        <i class="fa fa-calendar-o"></i> ca. <span id="RemainingDates">7</span> Übungssitzungen<br/>
                        <i class="fa fa-clock-o"></i> ca. <span id="RemainingTime">1:20h</span> Übungszeit
                    </div>
                    <div class="col-md-6" id="chartTrainingTimeWrapper">
                        <div id="chartTrainingTime" data-trainingplaneffort=""></div>
                    </div>
                </div>
                
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-12"><h4 class="ColoredUnderline Date">Terminvorschläge</h4></div>
                </div>
                
                <div id="divTrainingPlanDetailsSpinner" class="row" style="display: none">
                    <div class="col-md-12" style="height: 200px">
                        <div style="display: block; text-align: center; margin-top: 80px;">
                            <i class="fa fa-refresh fa-spin" style="font-size: 30px;"></i>
                        </div>
                    </div>
                </div>
                
                <div id="divTrainingPlanDetails">
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
            </div>
            <div class="modal-footer" style="padding-top: 7px; padding-bottom: 7px;">
                <a href="#" class="btn btn-default" data-dismiss="modal">Schließen</a>
            </div>
        </div>
    </div>
</div>