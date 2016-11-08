<%@ Page Title="Dein Ergebnis" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<TestSessionResultModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/TestSessionResult") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="margin-bottom: 15px; margin-top: 0px;">
        <span class="">Dein Ergebnis</span>
    </h2>
    <p>
        Du hast dein Wissen zu 
        <% if (Model.TestSessionTypeIsSet) { %>
            dem Fragesatz 
            <a href="<%= Links.SetDetail(Url, Model.TestedSet) %>" style="display: inline-block;">
                <span class="label label-set"><%: Model.TestedSet.Name %></span>
            </a>
            mit insgesamt <%=Model.TestedSet.Questions().Count %> Fragen
        <% } else if (Model.TestSessionTypeIsCategory) { %>
            der Kategorie
            <a href="<%= Links.CategoryDetail(Model.TestedCategory) %>" style="display: inline-block;">
                <span class="label label-category"><%: Model.TestedCategory.Name %></span>
            </a>
            mit insgesamt <%=Model.TestedCategory.CountQuestions %> Fragen
        <% } %>
        getestet und dabei <%= Model.NumberQuestions %> Fragen beantwortet. 
        (<a href="#detailedAnswerAnalysis">Zur Auswertung deiner Antworten</a>)
    </p>
    

    <div class="row">
        <div class="col-sm-12">
            <div class="stackedBarChartContainer" style="margin-bottom: 0;">
                <% if (Model.NumberCorrectPercentage>0) {%>
                    <div class="stackedBarChart chartCorrectAnswer" style="width: <%=Model.NumberCorrectPercentage %>%;">
                        <%=Model.NumberCorrectPercentage %>% <br/>
                        (<%=Model.NumberCorrectAnswers %> richtig)
                    </div>
                <% } %>                
                <% if (Model.NumberWrongAnswersPercentage>0) {%>
                    <div class="stackedBarChart chartWrongAnswer" style="width: <%=Model.NumberWrongAnswersPercentage %>%;">
                        <%=Model.NumberWrongAnswersPercentage %>% <br />
                        (<%=Model.NumberWrongAnswers %> falsch)
                    </div>
                <% } %>                
                <% if (Model.NumberOnlySolutionViewPercentage>0) {%>
                    <div class="stackedBarChart chartNotAnswered" style="width: <%=Model.NumberOnlySolutionViewPercentage %>%;">
                        <%=Model.NumberOnlySolutionViewPercentage %>% <br/>
                        (<%=Model.NumberOnlySolutionView %> unbeantwortet)
                    </div>
                <% } %>                
            </div>
            <div id="divIndicatorAverageWrapper" style="width: 100%">
                <div id="divIndicatorAverage" style="margin-left: <%= Model.PercentageAverageRightAnswers %>%">
                    <i class="fa fa-caret-up fa-4x show-tooltip" style="margin-left: -16px;" title="Der Durchschnitt beantwortet <%= Model.PercentageAverageRightAnswers %> richtig."></i>
                </div>
                <div id="divIndicatorAverageText">
                    <p style="">
                        Der Durchschnitt aller Nutzer <br />
                        beantwortet <%= Model.MeBetterThanAverage ? "nur " : "" %><span id="avgPercentageCorrect"><%= Model.PercentageAverageRightAnswers %></span>% richtig.
                    </p>
                </div>
            </div>
          
            <div class="buttonRow">
                <a href="<%= Url.Action(Links.KnowledgeAction, Links.KnowledgeController) %>" class="btn btn-link" style="padding-right: 10px">
                    Zur Wissenszentrale
                </a>
                <a href="<%= Model.LinkForRepeatTest %>" class="btn btn-primary show-tooltip" style="padding-right: 10px"
                        title="Neue Fragen aus <% if (Model.TestSessionTypeIsSet) Response.Write("dem gleichen Fragesatz");
                                                  else if (Model.TestSessionTypeIsCategory) Response.Write("der gleichen Kategorie");%>
                    " rel="nofollow">
                    <i class="fa fa-repeat AnswerResultIcon">&nbsp;</i>Noch einmal testen
                </a>
                <% 
                    var userSession = new SessionUser();
                    if (!userSession.IsLoggedIn){ %>
                        <a href="<%= Url.Action("Register", "Welcome") %>" class="btn btn-primary" style="padding-right: 10px">
                            Zur Registrierung
                        </a>
                <% } %>
            </div>
            
            <% if (Model.ContentRecommendationResult != null) { %>
                <h4>Lust auf mehr? Das könnte dich auch interessieren:</h4>
                <div class="row CardsLandscape" id="contentRecommendation">
                    <% foreach (var set in Model.ContentRecommendationResult.Sets)
                       {
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                    <% foreach (var category in Model.ContentRecommendationResult.Categories)
                       {
                            Html.RenderPartial("Cards/CardSingleCategory", CardSingleCategoryModel.GetCardSingleCategoryModel(category.Id));
                       } %>
                    <% foreach (var set in Model.ContentRecommendationResult.PopularSets)
                       { 
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                </div>
            <% } %>


            <div id="detailedAnswerAnalysis">
                <h3>Auswertung deiner Antworten</h3>
                <p style="color: silver; font-size: 11px;">
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
                                                    <%= GetQuestionImageFrontendData.Run(step.Question).RenderHtmlImageBasis(128, true, ImageType.Question, linkToItem: Links.AnswerQuestion(Url, step.Question)) %> 
                                                    </div>
                                                </div>
                                                <div class="col-xs-9 col-sm-10">
                                                    <p class="rightAnswer">Richtige Antwort: <%= GetQuestionSolution.Run(step.Question).CorrectAnswer()%><br/></p>
                                                    <p class="answerTry">Deine Antwort: <%= (step.AnswerState == TestSessionStepAnswerState.OnlyViewedSolution) ? "(unbeantwortet)" : step.AnswerText %></p>
                                                    <p class="averageCorrectness">Wahrscheinlichkeit richtige Antwort (alle Nutzer): <%= step.Question.CorrectnessProbability %>%</p>
                                                    <p class="answerLinkToQ"><a href="<%= Links.AnswerQuestion(step.Question) %>"><i class="fa fa-arrow-right">&nbsp;</i>Diese Frage einzeln üben</a></p>
                                                    
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    <% } %>
            </div>
        </div>

    </div>


</asp:Content>