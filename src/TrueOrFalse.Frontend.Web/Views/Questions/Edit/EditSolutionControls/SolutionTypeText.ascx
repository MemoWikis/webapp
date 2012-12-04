﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSoulutionExact>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>

<script language="javascript" type="text/javascript">
    $(function() {
        $('#Answer').defaultText();
    });
</script>

<div class="control-group">
    <%= Html.LabelFor(m => m.Text, new { @class = "control-label" })%>
    <div class="controls">
        <%= Html.TextAreaFor(m => m.Text, new { @id = "Answer", @style = "height:18px; width:210px;", placeholder = "Antwort eingeben." })%>
        
        <a href="#" class="btn" style="padding: 3px 4px; display: none"><img src="/Images/textfield-16.png" alt="Text"/></a>
        
        <div  style="display: inline-block; border: 5px solid white; border-top: none; position: absolute; border-radius: 6px">
            <div class="btn-group">
                <a class="btn active" style="padding: 3px 4px; border-bottom-left-radius: 0"><img src="/Images/textfield-16.png" /></a>
                <a class="btn" style="padding: 3px 4px"><img src="/Images/numeric_stepper-16.png" /></a>
                <a class="btn" style="padding: 3px 4px; border-bottom-right-radius: 0"><img src="/Images/date-16.png" /></a>
            </div>
            
            <div class="well" style="border-top-left-radius: 0; margin-top: -1px; padding: 10px; border-color: #ccc; margin-bottom: 0;
            background: #ffffff;
            background: -moz-linear-gradient(top, #ffffff 0%, #f8f8f8 100%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ffffff), color-stop(100%,#f8f8f8));
            background: -webkit-linear-gradient(top, #ffffff 0%,#f8f8f8 100%);
            background: -o-linear-gradient(top, #ffffff 0%,#f8f8f8 100%);
            background: -ms-linear-gradient(top, #ffffff 0%,#f8f8f8 100%);
            background: linear-gradient(to bottom, #ffffff 0%,#f8f8f8 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#f8f8f8',GradientType=0 );">
                <a class="close" href="#" style="margin: -5px -4px 0 0">&times;</a>
                
                
                <div style="margin-bottom: 5px">Großschreibung:</div>
                
                <div class="btn-group">
                    <a class="btn active">Ignorieren</a>
                    <a class="btn">Beachten</a>
                </div>
                                
                <div style="margin-top:10px; height: 20px;">
                    <label class="checkbox" style="width: auto">
                        <input type="checkbox">Exakte Schreibweise
                    </label>
                    <i class="icon-question-sign cursor-hand" id="tabInfoMyKnowledge">
                    <script>
                        $(function () {
                            $('#tabInfoMyKnowledge').click(function () {
                                $("#modalTabInfoMyKnowledge").modal('show');
                            });
                        });
                    </script>
                    </i>
                </div>
                
                <div style="clear: both"></div>
            </div>
        </div>
    </div>

</div>

<% /* MODAL-TAB-INFO****************************************************************/ %>
    
<div id="modalTabInfoEx" class="modal hide fade">
    <div class="modal-header">
        <button class="close" data-dismiss="modal">×</button>
        <h3>Hilfe: Tab - Mein Wunschwissen</h3>
    </div>
    <div class="modal-body">
        Es werden nur die Fragen gezeigt, die Sie zu Ihrem (Wunsch-)Wissen hinzugefügt haben. 
    </div>
    <div class="modal-footer">
        <a href="#" class="btn btn-warning" data-dismiss="modal">Mmh ok, nun gut.</a>
        <a href="#" class="btn btn-info" data-dismiss="modal">Danke, ich habe verstanden!</a>
    </div>
</div>