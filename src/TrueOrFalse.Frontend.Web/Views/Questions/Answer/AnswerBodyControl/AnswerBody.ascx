<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerBodyModel>" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="AnswerBody">

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
    
    <% if (Model.IsLearningSession) { %>
        <input type="hidden" id="ajaxUrl_LearningSessionAmendAfterShowSolution" value="<%= Model.AjaxUrl_LearningSessionAmendAfterShowSolution(Url) %>" />
    <% } %>

    <input type="hidden" id="disableAddKnowledgeButton"  value="<%= Model.DisableAddKnowledgeButton %>"/>
    
    <input type="hidden" id="hddTimeRecords" />

    <div class="AnswerQuestionBodyMenu">

        <% if (!Model.IsInWidget)
           { %>
            <% if (!Model.DisableAddKnowledgeButton)
               { %>
                <span class="Pin" data-question-id="<%= Model.QuestionId %>">
                    <%= Html.Partial("AddToWishknowledgeButtonQuestionDetail", new AddToWishknowledge(Model.IsInWishknowledge, isShortVersion: true)) %>
                </span>
            <% } %>
            <% if (Model.IsLoggedIn && Model.IsCreator || Model.IsInstallationAdmin)
               { %>
            <span class="edit-question">
                <a href="<%= Links.EditQuestion(Url, Model.QuestionText, Model.QuestionId) %>" class="TextLinkWithIcon"><i class="fa fa-pencil"></i></a>
            </span>
            <% }  %>
             <div class="Button dropdown">
                <span class="margin-top-4">
                    <a href="#" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" style="font-size: 14px;">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right">
                        <% if (Model.IsLearningSession || Model.IsTestSession) { %>
                            <li><a target="_blank"href="<%= Links.GetUrl(Model.Question) %>">Frageseite anzeigen </a></li>
                        <% } %>
                        <% if (Model.IsCreator || Model.IsInstallationAdmin)
                           { %>
                            <li><a href="<%= Links.EditQuestion(Url, Model.QuestionText, Model.QuestionId) %>" class="TextLinkWithIcon">Frage bearbeiten</a></li>
                        <% }  %>
                        <li><a target="_blank"href="<%= Model.ShareFacebook %>">Frage teilen </a></li>     
                        <li><a style="white-space: nowrap" href="#" data-action="embed-question">Frage einbetten</a></li>
                        <% if (Model.IsCreator || Model.IsInstallationAdmin)
                           { %>
                            <li id="DeleteQuestion">
                                <a class="TextLinkWithIcon" data-toggle="modal" data-questionid="808" href="#modalDeleteQuestion">
                                  <span>Frage löschen</span>
                                </a>
                            </li>
                        <% } %>
                        <li><a href="<%= Links.QuestionHistory(Model.QuestionId) %>">Versionen anzeigen</a></li>
                    </ul>
                </span>
            </div>
            
         <% } %>
    </div>

    
    <% if (Model.SolutionType != SolutionType.FlashCard.ToString()) { %>
    <h1 class="QuestionText" style="font-size: 22px; font-family: Open Sans, Arial, sans-serif; line-height: 31px; margin: 0;">
        <%= Model.QuestionText %>
    </h1>
    <% } %>

    <div class="row">
        <% if (Model.SolutionType != SolutionType.FlashCard.ToString()){
               if (!string.IsNullOrEmpty(Model.QuestionTextMarkdown)){ %>
                    <div id="MarkdownCol">
                        <div class="RenderedMarkdown"><%= Model.QuestionTextMarkdown %></div>
                    </div>
                <% }
           } %>

        <div id="AnswerAndSolutionCol">
            <div id="AnswerAndSolution">

                <% if (Model.HasSound) { Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>

                <div class="row">
                    <div id="AnswerInputSection" class="">
                        <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>" />
                        <input type="hidden" id="hddSolutionTypeNum" value="<%: Model.SolutionTypeInt %>" />
                        <%
                            string userControl = "SolutionType" + Model.SolutionType + ".ascx";

                            if (Model.SolutionMetadata.IsDate)
                                userControl = "SolutionTypeDate.ascx";
                            if (Model.SolutionType == "MatchList")
                            {
                                if (Request.Browser.IsMobileDevice)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if (Model.IsMobileRequest == true)
                                    userControl = "SolutionTypeMatchList_LayoutMobile.ascx";
                                if (Model.IsMobileRequest == false)
                                    userControl = "SolutionTypeMatchList.ascx";
                            }

                            Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel);

                            if (Model.SolutionType == SolutionType.FlashCard.ToString()){ %>
                                <script type="text/javascript">
                                    var questionText = '<h1 class="QuestionText" style="text-align: center; font-size: 22px; font-family: Open Sans, Arial, sans-serif; line-height: 31px; margin: 0;"><%= Model.QuestionText %></h1>';
                                    var flashCardFrontHTML = questionText + '<%= Model.QuestionTextMarkdown %>';
                                    $("#flashCard-front").append($('<div id="flashCard-frontContent">').append(flashCardFrontHTML));
                                    $('#flashCard-frontContent img').load(function() {
                                        var frontHeight = $('#flashCard-frontContent').outerHeight();
                                        var backHeight = $('.back.flashCardContentSite').outerHeight();
                                        $('#flashCardContent').height(
                                            frontHeight > backHeight
                                                ? frontHeight
                                                : backHeight
                                        );
                                    });
                                </script>
                            <% }

                            if (Model.SolutionType != SolutionType.FlashCard.ToString()) { %>
                                <div class="answerFeedback answerFeedbackCorrect" style="display: none;">
                                    <i class="fa fa-check-circle">&nbsp;</i>Richtig!
                                </div>
                                <div class="answerFeedback answerFeedbackWrong" style="display: none;">
                                    <i class="fa fa-minus-circle">&nbsp;</i>Leider falsch
                                </div>
                            <% } %>
                        </div>
                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <% if (Model.SolutionType == SolutionType.FlashCard.ToString()) { %>
                                        <a href="#" id="btnFlipCard" class="btn btn-warning" rel="nofollow">Umdrehen</a>
                                    <% } %>
                             

                                    <% if (Model.SolutionType != SolutionType.FlashCard.ToString()){ %>
                                        <div id="buttons-first-try" class="ButtonGroup">
                                        <a href="#" id="btnCheck" class="btn btn-primary" rel="nofollow" style="padding-right: 10px">Antworten</a>
                                        <a href="#" class="selectorShowSolution SecAction btn btn-link"><i class="fa fa-lightbulb-o">&nbsp;</i>Lösung anzeigen</a>
                                        <% if (!Model.IsInWidget)
                                           { %>
                                            <span id="activityPointsDispaly">
                                                <small>Punkte</small>
                                                <span id="activityPoints"><%= Model.TotalActivityPoints %></span>
                                                <span style="display: inline-block; white-space: nowrap;" class="show-tooltip" data-placement="bottom" title="Du bekommst Lernpunkte für das Beantworten von Fragen">
                                                    <i class="fa fa-info-circle"></i>
                                                </span>
                                            </span>
                                        <% } %>
                                    <% } else { %>
                                        <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons" style="display: none">
                                            <a href="#" id="btnRightAnswer" class="btn btn-warning" rel="nofollow">Wusste ich!</a>
                                            <a href="#" id="btnWrongAnswer" class="btn btn-warning" rel="nofollow">Wusste ich nicht!</a>
                                            <a href="#" id="flashCard-dontCountAnswer" class="selectorShowSolution SecAction btn btn-link">Nicht werten!</a>
                                            </div>
                                            <div>
                                    <% } %><!-- ??????----->
                                    <% if (Model.IsLearningSession && Model.NextUrl != null) { %>
                                        <a id="aSkipStep" href="<%= Model.NextUrl(Url) %>" class="SecAction btn btn-link"><i class="fa fa-step-forward">&nbsp;</i>Frage überspringen</a>
                                    <% } %>
                                </div>

                                <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                    <% if (Model.NextUrl != null && !Model.IsLastQuestion) { %>
                                        <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-primary" rel="nofollow">Nächste Frage</a>
                                    <% }else if(Model.NextUrl == null && Model.IsForVideo){ %> 
                                        <button id="continue"  class="btn btn-primary clickToContinue" style="display: none">Weiter</button>
                                    <% }else if (Model.PrimarySetMini != null && !Model.IsInWidget && !Model.IsForVideo && !Model.IsInGame) { %>
                                        <a href="<%= Links.TestSessionStartForSet(Model.PrimarySetMini.Name, Model.PrimarySetMini.Id) %>" id="btnStartTestSession" class="btn btn-primary show-tooltip" rel="nofollow" data-original-title="Teste dein Wissen mit <%= Settings.TestSessionQuestionCount  %> zufällig ausgewählten Fragen aus dem Lernset '<%= Model.PrimarySetMini.Name %>'">
                                            <i class="fa fa-play-circle"></i>&nbsp;&nbsp;<b>Weitermachen</b><br/>
                                            <small>Wissen testen: <%= Model.PrimarySetMini.Name.TruncateAtWordWithEllipsisText(30,"...") %></small>
                                        </a>
                                    <% } %>

                                    <% if (Model.SolutionType != SolutionType.FlashCard.ToString()) { %>
                                        <a href="#" id="aCountAsCorrect" class="SecAction btn btn-link show-tooltip" title="Drücke hier und die Frage wird als richtig beantwortet gewertet" rel="nofollow" style="display: none;">Hab ich gewusst!</a>
                                    <% } %>
                                </div>

                                <% if (Model.SolutionType != SolutionType.FlashCard.ToString())
                                    { %>
                                <div id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                    <a href="#" id="btnCheckAgain" class="btn btn-warning" rel="nofollow">Nochmal Antworten</a>
                                    <a href="#" class="selectorShowSolution SecAction btn btn-link">Lösung anzeigen</a>
                                    
                                </div>
                                <% } %>
                                          
                                            

                                <div style="clear: both"></div>
                            </div>
                                <div id="AnswerFeedbackAndSolutionDetails">
                                <% if (Model.SolutionType != SolutionType.FlashCard.ToString())
                                    { %>
                                <div id="AnswerFeedback">
                                    <div class="" id="divAnsweredCorrect" style="display: none; margin-top: 5px;">
                                        <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                                    </div>

                                    <div id="Solution" class="Detail" style="display: none;">
                                        <div class="Label">Richtige Antwort:</div>
                                        <div class="Content"></div>
                                    </div>
                                    <div id="divWrongAnswerPlay" class="Detail" style="display: none; background-color: white;">
                                        <span style="color: #B13A48"><b>Deine Antwort war falsch</b></span>
                                        <div>Deine Eingabe:</div>
                                        <div style="margin-top: 7px;" id="divWrongEnteredAnswer">
                                        </div>
                                    </div>

                                    <div id="divWrongAnswer" class="Detail" style="display: none; background-color: white;">
                                        <span id="spnWrongAnswer" style="color: #B13A48"><b>Falsch beantwortet </b></span>
                                        <a href="#" id="CountWrongAnswers" style="float: right; margin-right: -5px;">(zwei Versuche)</a><br />

                                        <div style="margin-top: 5px;" id="answerFeedbackTry">Du könntest es wenigstens probieren!</div>

                                        <div id="divWrongAnswers" style="margin-top: 7px; display: none;">
                                            <span class="WrongAnswersHeading">Deine bisherigen Antwortversuche:</span>
                                            <ul style="padding-top: 5px;" id="ulAnswerHistory">
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <% } %>

                                <div id="SolutionDetailsSpinner" style="display: none;">
                                    <i class="fa fa-spinner fa-spin" style="color: #b13a48;"></i>
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

<div class="FooterQuestionDetails row" style=" <%if(Model.IsInWidget){%> padding-bottom: 0; <%}
                                         else{ %> padding-top: 80px;<% } %> " >
    <div id="LicenseQuestion" class=" col-md-2">
        <% if (Model.LicenseQuestion.IsDefault()) { %>
            <a class="TextLinkWithIcon" rel="license" href="http://creativecommons.org/licenses/by/4.0/" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz <%= LicenseQuestionRepo.GetDefaultLicense().NameShort %>" data-placement="auto top"
                data-content="Autor: <a href='<%= Links.UserDetail(Model.Creator) %>' <%= Model.IsInWidget ? "target='_blank'" : "" %>><%= Model.Creator.Name %></a><%= Model.IsInWidget ? " (Nutzer auf <a href='/' target='_blank'>memucho.de</a>)" : " " %><br/><%= LicenseQuestionRepo.GetDefaultLicense().DisplayTextFull %>">
                <div> <img src="/Images/Licenses/cc-by 88x31.png" width="60" style="margin-top: 4px; opacity: 0.6; padding-bottom: 2px;" />&nbsp;</div>
                <div  class="TextDiv"> <span class="TextSpan"><%= LicenseQuestionRepo.GetDefaultLicense().NameShort %></span></div>
            </a><%--target blank to open outside the iframe of widget--%>

        <% } else { %>
            <a class="TextLinkWithIcon" href="#" data-toggle="popover" data-trigger="focus" title="Infos zur Lizenz" data-placement="auto top" data-content="<%= Model.LicenseQuestion.DisplayTextFull %>">
                <div class="TextDiv"><span class="TextSpan"><%= Model.LicenseQuestion.DisplayTextShort %></span>&nbsp;&nbsp;<i class="fa fa-info-circle">&nbsp;</i></div>
            </a>
        <% } %>
    </div>
    <% if (!Model.IsInWidget)
       { %>
        <div class="col-md-10">
        <div class="created"> Erstellt von: <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.Creator.Name %></a> vor <%= Model.CreationDateNiceText %></div>
        <div class="processed"> Diese Frage wurde zuletzt bearbeitet von:  <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.Creator.Name %></a> vor  <%= Model.QuestionLastEditedOn %> </div>
            <% if (Model.ShowCommentLink)
               { %>
                <div class="comment-link">
                    <% if (Model.IsLoggedIn)
                       { %>
                        <a href="#comments"><div class="fa fa-comment-o"></div></a>
                      <% }
                       else
                       { %>
                        <a href="#comments"><div class="fas fa-comment"></div></a>
                    <% } %>
                </div>
            <% } %>
        </div>
    <% } %>
</div>


<% Html.RenderPartial("~/Views/Questions/Answer/ShareQuestionModal.ascx", new ShareQuestionModalModel(Model.QuestionId)); %>