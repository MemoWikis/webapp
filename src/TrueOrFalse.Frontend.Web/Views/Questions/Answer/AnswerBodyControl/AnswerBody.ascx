<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>

<input type="hidden" id="questionId" value="<%= Model.QuestionId %>" />
<input type="hidden" id="ajaxUrl_SendAnswer" value="<%= Model.AjaxUrl_SendAnswer(Url) %>" />
<input type="hidden" id="ajaxUrl_GetSolution" value="<%= Model.AjaxUrl_GetSolution(Url) %>" />
<input type="hidden" id="ajaxUrl_CountLastAnswerAsCorrect" value="<%= Model.AjaxUrl_CountLastAnswerAsCorrect(Url) %>" />

<div class="well">
                                
    <div style="float: right; margin-left: 10px;">
 
        <span id="brainWaveConnected" style="margin-right: 5px; position:relative; top: -6px;">
            <span class="label label-primary" id="concentrationLevel" title="Konzentration"></span>
            <span class="label label-info" id="mellowLevel" title="Entspanntheit"></span>
        </span>
                    
        <a id="HeartToAdd" href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
            <i class="fa fa-heart show-tooltip <%= Model.IsInWishknowledge ? "" : "hide2" %>" id="iAdded" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
            <i class="fa fa-heart-o show-tooltip <%= Model.IsInWishknowledge ? "hide2" : "" %>" id="iAddedNot" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
            <i class="fa fa-spinner fa-spin hide2" id="iAddSpinner" style="color:#b13a48;"></i>
        </a>
    </div>    
    <div style="font-size: 22px; padding-bottom: 20px;">
        <%= Model.QuestionText %>
    </div>
                
    <div class="RenderedMarkdown"><%= Model.QuestionTextMarkdown %></div>
            
    <% if (Model.HasSound){ Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
    
    <div class="alert alert-info" id="divWrongAnswerPlay" style="display: none; background-color: white; color:#2E487B;">
        <span style="color: #B13A48"><b>Deine Antwort war falsch</b></span>
        <div>Dein Eingabe:</div>
        <div style="margin-top:7px;" id="divWrongEnteredAnswer">
        </div>
    </div>

    <div class="alert alert-info" id="divWrongAnswer" style="display: none; background-color: white; color:#2E487B;">
        <span id="spnWrongAnswer" style="color: #B13A48"><b>Falsche Antwort </b></span>
        <a href="#" id="CountWrongAnswers" style="float: right; margin-right: -5px;">(zwei Versuche)</a><br/>
                
        <div style="margin-top:5px;" id="answerFeedback">Du könntest es wenigstens probieren!</div>
                
        <div style="margin-top:7px; display: none;" id="divWrongAnswers" >
            <span class="WrongAnswersHeading">Deine bisherigen Antwortversuche:</span>
            <ul style="padding-top:5px;" id="ulAnswerHistory">
            </ul>
        </div>
    </div>
        
    <div id="AnswerInputSection">
        <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>"/>
        <input type="hidden" id="hddSolutionTypeNum" value="<%: Model.SolutionTypeInt %>" />
        <%
            string userControl = "SolutionType" + Model.SolutionType + ".ascx";
            if (Model.SolutionMetadata.IsDate)
                userControl = "SolutionTypeDate.ascx";
                        
            Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel); 
        %>
    </div>
                
    <div id="SolutionDetailsSpinner" style="display: none;">
        <i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i>
    </div>
    <div id="SolutionDetails" class="alert alert-info" style="display: none; background-color: white; color:#2E487B;">
                    
            <div class="" id="divAnsweredCorrect" style="display: none; margin-top:5px;">
            <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
        </div>

        <div id="Solution" class="Detail" style="display: none;">
            <div class="Label">Richtige Antwort:</div>
            <div class="Content"></div>
        </div>
        <div id="Description" class="Detail" style="display: none;">
            <div class="Label">Ergänzungen zur Antwort:</div>
            <div class="Content"></div>
        </div>
            <div id="References" class="Detail" style="display: none;">
            <div class="Label">Quellen:</div>
            <div class="Content"></div>
        </div>
    </div>
            
    <div id="Buttons" style="margin-bottom: 10px; margin-top: 10px;">
        <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
        <div id="buttons-first-try" class="pull-right">
            <a href="#" class="selectorShowAnswer btn btn-info">Antwort anzeigen</a>
            <a href="#" id="btnCheck" class="btn btn-primary" style="padding-right: 10px">Antworten</a>
        </div>
                    
        <% if(Model.NextUrl != null){ %>
            <div id="buttons-next-answer" class="pull-right" style="display: none;">
                <a href="#" id="btnCountAsCorrect" class="btn btn-info show-tooltip" title="Drücke hier und deine letzte Antwort wird als richtig gewertet (bei anderer Schreibweise, Formulierung ect). Aber nicht schummeln!" style="display: none;">Hab ich gewusst!</a>
                <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-success pull-right">N&auml;chste Frage</a>
            </div>
        <% } %>

        <div id="buttons-edit-answer" class="pull-right" style="display: none;">
            <a href="#" class="selectorShowAnswer btn btn-info">Antwort anzeigen</a>
            <a href="#" id="btnEditAnswer" class="btn btn-warning">Antwort überarbeiten</a>
        </div>
        <div id="buttons-answer-again" class="pull-right" style="display: none">
            <a href="#" class="selectorShowAnswer btn btn-info">Antwort anzeigen</a>
            <a href="#" id="btnCheckAgain" class="btn btn-warning pull-right">Nochmal Antworten</a>
        </div>
                    
        <div style="clear: both"></div>
    </div>
</div>