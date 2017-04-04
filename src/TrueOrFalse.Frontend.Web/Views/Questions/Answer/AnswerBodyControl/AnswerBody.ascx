﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="AnswerBody" class="well">
                                
    <input type="hidden" id="hddQuestionViewGuid" value="<%= Model.QuestionViewGuid.ToString() %>" />
    <input type="hidden" id="hddInteractionNumber" value="1" />
    <input type="hidden" id="questionId" value="<%= Model.QuestionId %>" />
    <input type="hidden" id="isLastQuestion" value="<%= Model.IsLastQuestion %>" />
    <input type="hidden" id="ajaxUrl_SendAnswer" value="<%= Model.AjaxUrl_SendAnswer(Url) %>" />
    <input type="hidden" id="ajaxUrl_GetSolution" value="<%= Model.AjaxUrl_GetSolution(Url) %>" />
    <input type="hidden" id="ajaxUrl_CountLastAnswerAsCorrect" value="<%= Model.AjaxUrl_CountLastAnswerAsCorrect(Url) %>" />
    <input type="hidden" id="ajaxUrl_CountUnansweredAsCorrect" value="<%= Model.AjaxUrl_CountUnansweredAsCorrect(Url) %>" />
    <% if (Model.IsTestSession) { %>
        <input type="hidden" id="ajaxUrl_TestSessionRegisterAnsweredQuestion" value="<%= Model.AjaxUrl_TestSessionRegisterAnsweredQuestion(Url) %>" />
        <input type="hidden" id="TestSessionProgessAfterAnswering" value="<%= Model.TestSessionProgessAfterAnswering %>" />
    <% } %>
    <% if (Model.IsLearningSession) {%>
        <input type="hidden" id="ajaxUrl_LearningSessionAmendAfterShowSolution" value="<%= Model.AjaxUrl_LearningSessionAmendAfterShowSolution(Url) %>" />
    <% } %>
    <input type="hidden" id="hddTimeRecords" />

    <div style="float: right; margin-left: 10px;">
 
        <span id="brainWaveConnected" style="margin-right: 5px; position:relative; top: -6px;">
            <span class="label label-primary" id="concentrationLevel" title="Konzentration"></span>
            <span class="label label-info" id="mellowLevel" title="Entspanntheit"></span>
        </span>
        
    </div>    
    <h1 class="QuestionText" style="font-size: 22px; font-family: Open Sans, Arial, sans-serif; line-height: 31px; margin: 0;">
        <%= Model.QuestionText %>
    </h1>
    <div class="row">
        <% if (!string.IsNullOrEmpty(Model.QuestionTextMarkdown))
           { %>
            <div id="MarkdownCol">
                <div class="RenderedMarkdown"><%= Model.QuestionTextMarkdown %></div>
            </div>
        <% } %>
        <div id="AnswerAndSolutionCol">
            <div id="AnswerAndSolution">
                
                <% if (Model.HasSound){ Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
    
                <div class="row" >
                    <div id="AnswerInputSection" class="">
                        <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>"/>
                        <input type="hidden" id="hddSolutionTypeNum" value="<%: Model.SolutionTypeInt %>" />
                        <%
                            string userControl = "SolutionType" + Model.SolutionType + ".ascx";

                            if (Model.SolutionMetadata.IsDate)
                                userControl = "SolutionTypeDate.ascx";
                            if (Model.SolutionType == "MatchList")
                            {
                                if(Request.Browser.IsMobileDevice)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if(Model.isMobileRequest == true)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if(Model.isMobileRequest == false)
                                    userControl = "SolutionTypeMatchList.ascx";
                            }

                            Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel);
                        %>

                        <div class="answerFeedback answerFeedbackCorrect" style="display: none;">
                            <i class="fa fa-check-circle">&nbsp;</i>Richtig!
                        </div>
                        <div class="answerFeedback answerFeedbackWrong" style="display: none;">
                            <i class="fa fa-minus-circle">&nbsp;</i>Leider falsch
                        </div>
                    </div>
                    <div id="ButtonsAndSolutionCol">
                        <div id="ButtonsAndSolution" class="Clearfix">
                            <div id="Buttons">
                                
                                <% if(!Model.DisableAddKnowledgeButton) { %>
                                    <span class="Pin" data-question-id="<%= Model.QuestionId%>">
                                        <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge, isShortVersion: true)) %>
                                    </span>
                                <% } %>

                                <div id="buttons-first-try" class="ButtonGroup">
                                    <a href="#" id="btnCheck" class="btn btn-primary" rel="nofollow" style="padding-right: 10px">Antworten</a>
                                    <a href="#" class="selectorShowSolution SecAction btn btn-link"><i class="fa fa-lightbulb-o">&nbsp;</i>Lösung anzeigen</a>
                                    <% if (Model.IsLearningSession && Model.NextUrl != null){%>
                                        <a id="aSkipStep" href="<%= Model.NextUrl(Url) %>" class="SecAction btn btn-link"><i class="fa fa-step-forward">&nbsp;</i>Frage überspringen</a>
                                    <% } %>
                                </div>
                    
                                <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                    <% if (Model.NextUrl != null){ %>
                                        <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-primary" rel="nofollow">Nächste Frage</a>
                                    <% } %>
                                    <a href="#" id="aCountAsCorrect" class="SecAction show-tooltip" title="Drücke hier und die Frage wird als richtig beantwortet gewertet" rel="nofollow" style="display: none;">Hab ich gewusst!</a>
                                </div>
                                <div id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                    <a href="#" id="btnCheckAgain" class="btn btn-warning" rel="nofollow">Nochmal Antworten</a>
                                    <a href="#" class="selectorShowSolution SecAction">Lösung anzeigen</a>
                                </div>
                    
                                <div style="clear: both"></div>
                            </div>
                            <div id="AnswerFeedbackAndSolutionDetails">
                                <div id="AnswerFeedback">
                                    <div class="" id="divAnsweredCorrect" style="display: none; margin-top:5px;">
                                        <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                                    </div>

                                    <div id="Solution" class="Detail" style="display: none;">
                                        <div class="Label">Richtige Antwort:</div>
                                        <div class="Content"></div>
                                    </div>
                                    <div id="divWrongAnswerPlay" class="Detail" style="display: none; background-color: white;">
                                        <span style="color: #B13A48"><b>Deine Antwort war falsch</b></span>
                                        <div>Deine Eingabe:</div>
                                            <div style="margin-top:7px;" id="divWrongEnteredAnswer">
                                        </div>
                                    </div>

                                    <div id="divWrongAnswer" class="Detail" style="display: none; background-color: white;">
                                        <span id="spnWrongAnswer" style="color: #B13A48"><b>Falsch beantwortet </b></span>
                                        <a href="#" id="CountWrongAnswers" style="float: right; margin-right: -5px;">(zwei Versuche)</a><br/>
                
                                        <div style="margin-top:5px;" id="answerFeedbackTry">Du könntest es wenigstens probieren!</div>
                
                                        <div id="divWrongAnswers" style="margin-top:7px; display: none;"  >
                                            <span class="WrongAnswersHeading">Deine bisherigen Antwortversuche:</span>
                                            <ul style="padding-top:5px;" id="ulAnswerHistory">
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div id="SolutionDetailsSpinner" style="display: none;">
                                    <i class="fa fa-spinner fa-spin" style="color:#b13a48;"></i>
                                </div>
                                <div id="SolutionDetails" style="display: none; background-color: white;">
                                    

                                    <div id="Description" class="Detail" style="display: none;">
                                        <div class="Label">Ergänzungen zur Antwort:</div>
                                        <div class="Content"></div>
                                    </div>
                                    <div id="References" class="Detail" style="display: none;">
                                        <div class="Label">Quellen:</div>
                                        <div class="Content"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="LicenseQuestion" class="Clearfix">
    <% if (Model.LicenseQuestion.IsDefault()){ %>
        <a class="TextLinkWithIcon" href="#" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz <%= LicenseQuestionRepo.GetDefaultLicense().NameShort %>" data-placement="auto left"
            data-content="Autor: <a href='<%= Links.UserDetail(Model.Creator) %>'><%= Model.Creator.Name %></a><br/><%= LicenseQuestionRepo.GetDefaultLicense().DisplayTextFull %>">
        <img src="/Images/Licenses/cc-by 88x31.png" width="60"  style="margin-top: 4px; opacity: 0.6;"/>&nbsp;
        <span class="TextSpan"><%= LicenseQuestionRepo.GetDefaultLicense().NameShort %></span>&nbsp;<i class="fa fa-info-circle"></i>
    </a>
    <% } else { %>
        <a class="TextLinkWithIcon" href="#" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz" data-placement="auto left" data-content="<%= Model.LicenseQuestion.DisplayTextFull %>">
            <span class="TextSpan"><%= Model.LicenseQuestion.DisplayTextShort %></span>&nbsp;&nbsp;<i class="fa fa-info-circle">&nbsp;</i>
        </a>
    <% } %>
    
    <%if (Model.ShowCommentLink) { %>
        <div style="float: right; position: relative; top: 4px;">
            <a href="#comments"><i class="fa fa-comment-o"></i> 
                <% if(Model.CommentCount == 0) { %>
                    Jetzt kommentieren
                <% } else if(Model.CommentCount == 1) { %>
                    1 Kommentar
                <% } else if (Model.CommentCount > 1) { %>
                    <%= Model.CommentCount %> Kommentare
                <% } %>
            </a>
        </div>
    <% } %>
</div>
