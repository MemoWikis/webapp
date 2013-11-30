<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSoulutionExact>" %>

<div class="form-group">
    <%= Html.LabelFor(m => m.Text, new { @class = "col-sm-2 control-label" })%>
    <div class="col-sm-10">
        <%= Html.TextBoxFor(m => m.Text, new { @class="form-control", @id = "Answer", @style = "width:210px; float: left;", placeholder = "Antwort eingeben." })%>
        
        <a href="#" class="btn btn-default" style="padding: 3px 4px; display: none"><img src="/Images/textfield-16.png" alt="Text"/></a>
        
        <div  style="display: inline-block; position: absolute;;">
            <div class="btn-group" style="position: relative; top: 3px; left: 10px; z-index: 5001;">
                <a class="btn btn-default active" style="padding: 3px 4px; border-bottom-left-radius: 0" id="btnMenuItemText"><img src="/Images/textfield-16.png" /></a>
                <a class="btn btn-default" style="padding: 3px 4px" id="btnMenuItemNumber"><img src="/Images/numeric_stepper-16.png" /></a>
                <a class="btn btn-default" style="padding: 3px 4px; border-bottom-right-radius: 0" id="btnMenuItemDate"><img src="/Images/date-16.png" /></a>    
            </div>
            
            <%-- MenuItemText --%>
            <div class="contextMenuOuter" id="divMenuItemText" style="z-index: 5000; position: relative; left: 5px; display: none">
                <div class="well contextMenu">
                    <div style="margin-bottom: 5px">Großschreibung:</div>
                    <div class="btn-group">
                        <a class="btn active">Ignorieren</a>
                        <a class="btn btn-default">Beachten</a>
                    </div>
                    <div style="margin-top:10px; height: 20px;">
                        <label class="checkbox" style="width: auto">
                            <input type="checkbox">Exakte Schreibweise
                        </label>
                        <i class="fa fa-question-circle cursor-hand" id="help"></i>
                    </div>
                    <div style="clear: both"></div>
                </div>
            </div>
            
            <%-- MenuItemNumber --%>
            <div class="contextMenuOuter" id="divMenuItemNumber" style="z-index: 5000; position: relative; left: 30px; width: 160px; display: none">
                <div class="well contextMenu">
                    <div style="margin-bottom: 5px">
                        Abweichung:
                        <input id="numberAccuracy" value="0" style="width: 20px;" />%    
                    </div>
                    <div>
                        Einheit: 
                        <input type="text" style="width: 100px;" />
                    </div>
                </div>
            </div>
            
            <%-- MenuItemDate --%>
            <div class="contextMenuOuter" id="divMenuItemDate" style="z-index: 5000; position: relative; left: 55px; display: none">
                <div class="well contextMenu">
                    <div style="margin-bottom: 5px">Genau auf:</div>   
          
                    <span id="spanSliderValue"></span>      
                    <div id="sliderDate" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all" style="width: 120px; margin-left:5px;"> 
                        <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                        <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
                    </div>
                    <div style="clear: both"></div>
                </div>
            </div>
        </div>
    </div>

</div>

<% /* MODAL-TAB-INFO****************************************************************/ %>
    
<div id="modalHelpSolutionType" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button class="close" data-dismiss="modal">×</button>
                <h3>Erklärung Lösungseigenschaften</h3>
            </div>
            <div class="modal-body">
                <h2>Groß- und Kleinschreibung</h2>
                <p>
                    Wenn "ignorieren" gewählt, dann wird bei der Eingabe die Groß- und Kleinschreibung ignoriert.
                </p>
                <h2>Exakte Schreibweise</h2>
                <p>
                    Ist "Exakte Schreibweise" gewählt, dann muss für eine korrekte Beantwortung die Eingabe exakt der Antwort entsprechen.
                </p>
            </div>
            <div class="modal-footer">
                <a href="#" class="btn btn-warning" data-dismiss="modal">Mmh ok, nun gut.</a>
                <a href="#" class="btn btn-info" data-dismiss="modal">Danke, ich habe verstanden!</a>
            </div>
        </div>
    </div>
</div>

<script src="/Views/Questions/Edit/EditSolutionControls/SolutionTypeText.min.js" type="text/javascript"></script>