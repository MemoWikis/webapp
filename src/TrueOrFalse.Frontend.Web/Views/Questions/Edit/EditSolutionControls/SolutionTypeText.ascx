<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSoulutionExact>" %>

<div class="control-group">
    <%= Html.LabelFor(m => m.Text, new { @class = "control-label" })%>
    <div class="controls">
        <%= Html.TextAreaFor(m => m.Text, new { @id = "Answer", @style = "height:18px; width:210px;", placeholder = "Antwort eingeben." })%>
        
        <a href="#" class="btn" style="padding: 3px 4px; display: none"><img src="/Images/textfield-16.png" alt="Text"/></a>
        
        <div  style="display: inline-block; border: 5px solid white; border-top: none; position: absolute; border-radius: 6px">
            <div class="btn-group">
                <a class="btn active" style="padding: 3px 4px; border-bottom-left-radius: 0" id="btnMenuItemText"><img src="/Images/textfield-16.png" /></a>
                <a class="btn" style="padding: 3px 4px" id="btnMenuItemNumber"><img src="/Images/numeric_stepper-16.png" /></a>
                <a class="btn" style="padding: 3px 4px; border-bottom-right-radius: 0" id="btnMenuItemDate"><img src="/Images/date-16.png" /></a>    
            </div>
            
            <%-- MenuItemText --%>
            <div class="well contextMenu hide" id="divMenuItemText">
                <%--<a class="close" href="#" style="margin: -5px -4px 0 0">&times;</a>--%>
                <div style="margin-bottom: 5px">Großschreibung:</div>
                <div class="btn-group">
                    <a class="btn active">Ignorieren</a>
                    <a class="btn">Beachten</a>
                </div>
                <div style="margin-top:10px; height: 20px;">
                    <label class="checkbox" style="width: auto">
                        <input type="checkbox">Exakte Schreibweise
                    </label>
                    <i class="icon-question-sign cursor-hand" id="help"></i>
                </div>
                <div style="clear: both"></div>
            </div>
            
            <%-- MenuItemNumber --%>
            <div class="well contextMenu hide" id="divMenuItemNumber" style="position: relative; left: 25px;">
                <div style="margin-bottom: 5px">Abweichung:</div>
                <div style="width: 150px;">
                    <input id="numberAccuracy" value="0" style="width: 10px;" />%    
                </div>
                
            </div>
            
            <%-- MenuItemDate --%>
            <div class="well contextMenu hide" id="divMenuItemDate" style="position: relative; left: 50px;">
                <%--<a class="close" href="#" style="margin: -5px -4px 0 0">&times;</a>--%>
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

<% /* MODAL-TAB-INFO****************************************************************/ %>
    
<div id="modalHelpSolutionType" class="modal hide fade">
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

<script src="/Views/Questions/Edit/EditSolutionControls/SolutionTypeText.min.js" type="text/javascript"></script>
