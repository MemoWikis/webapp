<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TestSessionResultModel>" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="stackedBarChartContainer" style="margin-bottom: 0;">
    <% if (Model.NumberCorrectPercentage>0) { %>
        <div class="stackedBarChart chartCorrectAnswer" style="width: <%= Model.NumberCorrectPercentage %>%;">
            <%=Model.NumberCorrectPercentage %>% <br/>
            (<%=Model.NumberCorrectAnswers %> richtig)
        </div>
    <% } %>                
    <% if (Model.NumberWrongAnswersPercentage>0) { %>
        <div class="stackedBarChart chartWrongAnswer" style="width: <%= Model.NumberWrongAnswersPercentage %>%;">
            <%=Model.NumberWrongAnswersPercentage %>% <br />
            (<%=Model.NumberWrongAnswers %> falsch)
        </div>
    <% } %>                
    <% if (Model.NumberOnlySolutionViewPercentage>0) { %>
        <div class="stackedBarChart chartNotAnswered" style="width: <%= Model.NumberOnlySolutionViewPercentage %>%;">
            <%=Model.NumberOnlySolutionViewPercentage %>% <br/>
            (<%=Model.NumberOnlySolutionView %> unbeantwortet)
        </div>
    <% } %>                
</div>
            
<% var tooltip = string.Format("Der Durchschnitt aller Nutzer beantwortete {0}% richtig", Model.PercentageAverageRightAnswers); %>
<div id="divIndicatorAverageWrapper" style="width: 100%">
    <div id="divIndicatorAverage" style="margin-left: <%= Model.PercentageAverageRightAnswers %>%">
        <i class="fa fa-caret-up fa-4x show-tooltip" style="margin-left: -16px;" title="<%= tooltip %>"></i>
    </div>
    <div id="divIndicatorAverageText">
        <p class="show-tooltip" title="<%= tooltip %>">
            Nutzerdurchschnitt (<span id="avgPercentageCorrect"><%= Model.PercentageAverageRightAnswers %></span>% richtig)
        </p>
    </div>
</div>


<div id="detailedAnswerAnalysis">
    <h3 style="margin-bottom: 25px">Auswertung deiner Antworten</h3>
    <p class="greyed fontSizeSmall">
        <a href="#" data-action="showAllDetails">Alle Details einblenden</a> | <a href="#" data-action="hideAllDetails">Alle Details ausblenden</a> | <a href="#" data-action="showDetailsExceptRightAnswer">Details zu allen nicht korrekten Fragen einblenden</a>
    </p>
    <% foreach (var step in Model.Steps)
        {
            if (step.AnswerState == TestSessionStepAnswerState.OnlyViewedSolution)
            { %> 
                <div class="row">
                    <div class="col-xs-12">
                        <div class="QuestionLearned Unanswered">
                            <a href="#" data-action="showAnswerDetails">
                            <i class="fa fa-circle AnswerResultIcon show-tooltip" title="Nicht beantwortet.">
                                &nbsp;&nbsp;
                            </i><%= step.Question.GetShortTitle(150) %>
                            (Details)</a><br/>
            <% }
            else if (step.AnswerState == TestSessionStepAnswerState.AnsweredCorrect)
            { %> 
                <div class="row">
                    <div class="col-xs-12">
                        <div class="QuestionLearned AnsweredRight">
                            <a href="#" data-action="showAnswerDetails">
                            <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Richtig beantwortet">
                                &nbsp;&nbsp;
                            </i><%= step.Question.GetShortTitle(150) %> 
                            (Details)</a><br/>
            <% }
            else if (step.AnswerState == TestSessionStepAnswerState.AnsweredWrong)
            { %>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="QuestionLearned AnsweredWrong">
                            <a href="#" data-action="showAnswerDetails">
                            <i class="fa fa-minus-circle AnswerResultIcon show-tooltip" title="Falsch beantwortet">
                                &nbsp;&nbsp;
                            </i><%= step.Question.GetShortTitle(150) %> 
                            (Details)</a><br/>
            <% } %>
                            <div class="answerDetails" data-questionId="<%= step.Question.Id %>">
                                <div class="row">
                                    <div class="col-xs-3 col-sm-2 answerDetailImage">
                                        <div class="ImageContainer ShortLicenseLinkText">
                                        <%= GetQuestionImageFrontendData.Run(step.Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(step.Question)) %> 
                                        </div>
                                    </div>
                                    <div class="col-xs-9 col-sm-10">
                                        <p class="rightAnswer">Richtige Antwort: <%= GetQuestionSolution.Run(step.Question).GetCorrectAnswerAsHtml() %></p>
                                        <% if (step.Question.SolutionType != SolutionType.FlashCard)
                                           { %>
                                                <p class="answerTry">Deine Antwort: <%= (step.AnswerState == TestSessionStepAnswerState.OnlyViewedSolution) ? "(unbeantwortet)" : Question.AnswersAsHtml(step.AnswerText, step.Question.SolutionType) %></p>
                                        <% } %>
                                        <p class="averageCorrectness">Wahrscheinlichkeit richtige Antwort (alle Nutzer): <%= step.Question.CorrectnessProbability %>%</p>
                                        
                                        <% if(!Model.IsInWidget) { %>
                                            <p class="answerLinkToQ"><a href="<%= Links.AnswerQuestion(step.Question) %>"><i class="fa fa-arrow-right">&nbsp;</i>Diese Frage einzeln lernen</a></p>
                                        <% } %>
                                    </div>
                                                
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        <% } %>
</div>
