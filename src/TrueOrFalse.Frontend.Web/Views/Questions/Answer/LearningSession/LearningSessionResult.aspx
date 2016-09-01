<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<LearningSessionResultModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Ergebnis</title>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawKnowledgeCharts);

        function drawKnowledgeChartDate(chartElementId, notLearned, needsLearning, needsConsolidation, solid) {

            var chartElement = $("#" + chartElementId);
            if (chartElement.length == 0)
                return;

            //'Sicheres Wissen', 'Sollte gefestigt werden', 'Sollte dringend gelernt werden', 'Noch nie gelernt'
            var data = google.visualization.arrayToDataTable([
                ['Wissenslevel', 'Anteil in %'],
                ['Sicheres Wissen', solid],
                ['Solltest du festigen', needsConsolidation],
                ['Solltest du lernen', needsLearning],
                ['Noch nicht gelernt', notLearned],
            ]);

            var options = {
                pieHole: 0.6,
                legend: { position: 'none' },
                pieSliceText: 'none',
                height: 120,
                backgroundColor: 'transparent',
                chartArea: {
                    width: '90%', height: '90%', top: 6
                },
                slices: {
                    0: { color: '#3e7700' },
                    1: { color: '#fdd648' },
                    2: { color: '#B13A48' },
                    3: { color: '#EFEFEF' },
                },
                pieStartAngle: 180
            };

            var chart = new google.visualization.PieChart(chartElement.get()[0]);
            chart.draw(data, options);
        }

        function drawKnowledgeCharts() {
            $("[data-date-id]").each(function () {
                var $this = $(this);
                var dateId = $this.attr("data-date-id");

                drawKnowledgeChartDate(
                    "chartKnowledgeDate" + dateId,
                    parseInt($this.attr("data-notLearned")),
                    parseInt($this.attr("data-needsLearning")),
                    parseInt($this.attr("data-needsConsolidation")),
                    parseInt($this.attr("data-solid")));
            });
        }

    </script>

    <%= Scripts.Render("~/bundles/js/LearningSessionResult") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Deine Übungssitzung: Ergebnis</h2>
    

    <div class="row">
        <div class="col-sm-9 xxs-stack">
            <div class="stackedBarChartContainer">
                <div class="stackedBarChart chartCorrectAnswer" style="width: <%=Model.NumberCorrectPercentage %>%;">
                    <% if (Model.NumberCorrectPercentage>0) {%> <%=Model.NumberCorrectPercentage %>% <%} %>
                </div>
                <div class="stackedBarChart chartCorrectAfterRepetitionAnswer" style="width: <%=Model.NumberCorrectAfterRepetitionPercentage %>%;">
                    <% if (Model.NumberCorrectAfterRepetitionPercentage>0) {%> <%=Model.NumberCorrectAfterRepetitionPercentage %>% <%} %>
                </div>
                <div class="stackedBarChart chartWrongAnswer" style="width: <%=Model.NumberWrongAnswersPercentage %>%;">
                    <% if (Model.NumberWrongAnswersPercentage>0) {%> <%=Model.NumberWrongAnswersPercentage %>% <%} %>
                </div>
                <div class="stackedBarChart chartNotAnswered" style="width: <%=Model.NumberNotAnsweredPercentage %>%;">
                    <% if (Model.NumberNotAnsweredPercentage>0) {%> <%=Model.NumberNotAnsweredPercentage %>% <%} %>
                </div>
            </div>

            <div class="SummaryText" style="clear: left;">
                <p>In dieser Übungssitzung hast du <%= Model.NumberQuestions %> Fragen gelernt und dabei</p>
                <div class="row">
                    <div class="col-xs-2 sumPct sumPctRight"><%=Model.NumberCorrectPercentage %>%</div>
                    <div class="col-xs-10 sumExpl">beim 1. Versuch gewusst (<%=Model.NumberCorrectAnswers %> Fragen)</div>
                    <div class="col-xs-2 sumPct sumPctRightAfterRep"><%=Model.NumberCorrectAfterRepetitionPercentage %>%</div>
                    <div class="col-xs-10 sumExpl">beim 2. oder 3. Versuch gewusst (<%=Model.NumberCorrectAfterRepetitionAnswers %> Fragen)</div>
                    <div class="col-xs-2 sumPct sumPctWrong"><%=Model.NumberWrongAnswersPercentage %>%</div>
                    <div class="col-xs-10 sumExpl">nicht gewusst (<%=Model.NumberWrongAnswers %> Fragen)</div>
                    <div class="col-xs-2 sumPct sumPctNotAnswered"><%=Model.NumberNotAnsweredPercentage %>%</div>
                    <div class="col-xs-10 sumExpl">übersprungen (<%=Model.NumberNotAnswered %> Fragen)</div>
                </div>
            </div>
            <div class="buttonRow">
                <% if (Model.LearningSession.IsDateSession) { %>
                    <a href="/Termin/Lernen/<%=Model.LearningSession.DateToLearn.Id %>" class="btn btn-link show-tooltip" style="padding-right: 10px" title="Eine neue Übungssitzung zu diesem Termin/Fragesatz beginnen">
                        Neue Übungssitzung
                    </a>
                    <a href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>" class="btn btn-primary" style="padding-right: 10px">
                        Zum Überblick
                    </a>
                <% } else if (Model.LearningSession.IsSetSession) { %>
                    <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" class="btn btn-link" style="padding-right: 10px">Zum Fragesatz (Übersicht)</a>
                    <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                        Neue Übungssitzung
                    </a>
                <% } else {
                    throw new Exception("buttons for this type of learning session not specified");
                } %>
            </div>
            
            <div class="stackedVBarChartContainer">
                <div class="stackedVBarChart chartCorrectAnswer" style="height: <%=Model.NumberCorrectPercentage %>%;">
                    <% if (Model.NumberCorrectPercentage>0) {%> <%=Model.NumberCorrectPercentage %>% <%} %>
                </div>
                <div class="stackedVBarChart chartCorrectAfterRepetitionAnswer" style="height: <%=Model.NumberCorrectAfterRepetitionPercentage %>%;">
                    <% if (Model.NumberCorrectAfterRepetitionPercentage>0) {%> <%=Model.NumberCorrectAfterRepetitionPercentage %>% <%} %>
                </div>
                <div class="stackedVBarChart chartWrongAnswer" style="height: <%=Model.NumberWrongAnswersPercentage %>%;">
                    <% if (Model.NumberWrongAnswersPercentage>0) {%> <%=Model.NumberWrongAnswersPercentage %>% <%} %>
                </div>
                <div class="stackedVBarChart chartNotAnswered" style="height: <%=Model.NumberNotAnsweredPercentage %>%;">
                    <% if (Model.NumberNotAnsweredPercentage>0) {%> <%=Model.NumberNotAnsweredPercentage %>% <%} %>
                </div>
            </div>


            <div id="detailedAnswerAnalysis">
                <h3>Auswertung der Antworten</h3>
                <p style="color: silver; font-size: 11px;">
                    <a href="#" data-action="showAllDetails">Alle Details einblenden</a> | <a href="#" data-action="hideAllDetails">Alle Details ausblenden</a> | <a href="#" data-action="showDetailsExceptRightAnswer">Details zu allen nicht korrekten Fragen einblenden</a>
                </p>
                <% foreach (var uniqueQuestion in Model.AnsweredStepsGrouped) // not accounted for: if answered wrong and then skipped, it counts as skipped, but maybe should count as wrong.
                    {
                        if (uniqueQuestion.First().AnswerState != StepAnswerState.Answered)
                        { %>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned Unanswered">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-circle AnswerResultIcon show-tooltip" title="Nicht beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% }
                        else if ((uniqueQuestion.First().AnswerState == StepAnswerState.Answered) && uniqueQuestion.First().Answer.AnsweredCorrectly())
                        { %> 
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned AnsweredRight">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 1. Versuch richtig beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% }
                        else if ((uniqueQuestion.Count() > 1) && (uniqueQuestion.Last().AnswerState == StepAnswerState.Answered) && uniqueQuestion.Last().Answer.AnsweredCorrectly())
                        { %> 
                            <div class="row">
                                <div class="col-xs-12">
                                        <a href="#" data-action="showAnswerDetails">
                                    <div class="QuestionLearned AnsweredRightAfterRepetition">
                                        <i class="fa fa-check-circle AnswerResultIcon show-tooltip" title="Beim 2. oder 3. Versuch richtig beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>

                        <% }
                        else if (((uniqueQuestion.Last().AnswerState == StepAnswerState.Answered) && (uniqueQuestion.Last().Answer.AnswerredCorrectly == AnswerCorrectness.False)) ||
                                 ((uniqueQuestion.Last().AnswerState != StepAnswerState.Answered) && (uniqueQuestion.Count() > 1)))
                        { %>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="QuestionLearned AnsweredWrong">
                                        <a href="#" data-action="showAnswerDetails">
                                        <i class="fa fa-minus-circle AnswerResultIcon show-tooltip" title="Falsch beantwortet">
                                            &nbsp;&nbsp;
                                        </i><%= uniqueQuestion.First().Question.GetShortTitle(150) %> 
                                        (Details)</a><br/>
                        <% } %>
                                        <div class="answerDetails" data-questionId=<%= uniqueQuestion.First().QuestionId %>>
                                            <div class="row">
                                                <div class="col-xs-3 col-sm-2 answerDetailImage">
                                                    <%= GetQuestionImageFrontendData.Run(uniqueQuestion.First().Question).RenderHtmlImageBasis(128, true, ImageType.Question) %> 
                                                </div>
                                                <div class="col-xs-9 col-sm-10">
                                                    <p class="rightAnswer">Richtige Antwort: <%=new GetQuestionSolution().Run(uniqueQuestion.First().Question).CorrectAnswer()%><br/></p>
                                                    <%
                                                    int counter = 1;
                                                    foreach (var step in uniqueQuestion)
                                                    {
                                                        if (step.AnswerState == StepAnswerState.NotViewedOrAborted)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (abgebrochen)</p><%
                                                        }
                                                        else if (step.AnswerState == StepAnswerState.Skipped)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (übersprungen)</p><%
                                                        }
                                                        else if (step.AnswerState == StepAnswerState.Uncompleted)
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: (noch nicht gesehen)</p><%
                                                        }
                                                        else
                                                        {
                                                            %> <p class="answerTry">Dein <%= counter %>. Versuch: <%= step.Answer.AnswerText %></p><%
                                                        }
                                                        counter++;
                                                    } %>
                                                    <p class="answerLinkToQ"><a href="<%= Links.AnswerQuestion(Url, uniqueQuestion.First().Question) %>"><i class="fa fa-arrow-right">&nbsp;</i>Diese Frage einzeln üben</a></p>
                                                    
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    <% } %>
            </div>
        </div>


        <div class="col-sm-3 xxs-stack">
            <% if(Model.LearningSession.IsSetSession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Fragesatz-Info
                    </div>
                    <div class="boxInfoContent">
                        <p>
                            Du hast diesen Fragesatz gelernt:<br />
                            <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="display: inline-block;">
                                <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
                            </a> (insgesamt <%=Model.LearningSession.TotalPossibleQuestions %> Fragen)
                        </p>
                    </div>
                </div>

            <% } %>

            <% if(Model.LearningSession.IsDateSession) { %>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Dein Termin
                    </div>
                    <div class="boxInfoContent">
                        <h3><%= Model.DateToLearn.GetTitle() %></h3>
                        <div class="tableLayout">
                            <div class="tableCellLayout"><i class="fa fa-calendar-o">&nbsp;</i></div>
                            <div class="tableCellLayout">
                                <%= Model.DateToLearn.DateTime.ToString("dd.MM.yyyy 'um' HH:mm") %>
                                (<% if(Model.DateIsInPast){
                                        Response.Write("vorbei seit ");
                                    }else { 
                                        Response.Write("noch ");
                                    }%><%= Model.DateRemainingTimeLabel.Value %> <%= Model.DateRemainingTimeLabel.Label %>)
                            </div>
                        </div>
                        <div class="tableLayout">
                            <div class="tableCellLayout"><i class="fa fa-list">&nbsp;</i></div>
                            <div class="tableCellLayout">
                                Insgesamt <%= Model.DateToLearn.CountQuestions() %> Fragen aus<br /> 
                                <a href="#" data-action="toggleDateSets">
                                    <i class="fa fa-caret-right dateSets">&nbsp;</i><i class="fa fa-caret-down dateSets" style="display: none;">&nbsp;</i><%= Model.DateToLearn.Sets.Count %> 
                                    <%= StringUtils.Plural(Model.DateToLearn.Sets.Count, "Fragesätzen","Fragesatz") %>
                                </a>
                                <div class="dateSets" style="display: inline; position: relative; display: none;" >
                                    <%  foreach(var set in Model.DateToLearn.Sets){ %>
                                        <a href="<%= Links.SetDetail(Url, set) %>">
                                            <span class="label label-set"><%= set.Name %></span>
                                        </a>
                                    <% } %>
                                </div>
                            </div>
                        </div>
                                
                        <p style="font-weight: bold;">
                            Dein aktueller Wissensstand:<br/>
                            <div id="chartKnowledgeDate<%=Model.DateToLearn.Id %>" 
                                    data-date-id="<%= Model.DateToLearn.Id %>"
                                    data-notLearned="<%= Model.KnowledgeNotLearned %>"
                                    data-needsLearning="<%= Model.KnowledgeNeedsLearning %>"
                                    data-needsConsolidation="<%= Model.KnowledgeNeedsConsolidation %>"
                                    data-solid="<%= Model.KnowledgeSolid %>">
                            </div>
                        </p>
                        <p style="text-align: right; margin-top: 20px;">
                            <a href="<%= Links.Dates() %>"><i class="fa fa-arrow-right">&nbsp;</i>Zur Terminübersicht</a>
                        </p>
                    </div>
                </div>
                <div class="boxInfo">
                    <div class="boxInfoHeader">
                        Dein Übungsplan
                    </div>
                    <div class="boxInfoContent">
                        <% if (Model.TrainingDateCount > 0) { %>
                            <p>
                                Vor dir liegen noch: <br/>
                                ca. <%= Model.TrainingDateCount %> Übungssitzung<%= StringUtils.Plural(Model.DateToLearn.Sets.Count, "en") %><br />
                                ca. <%= Model.RemainingTrainingTime%> Übungszeit
                            </p>
                            <p>
                                Deine nächste Übungssitzung: <br/><i class="fa fa-bell"></i>&nbsp;
                                <% if(Model.TrainingPlan.HasOpenDates) {
                                    var timeSpanLabel = new TimeSpanLabel(Model.TrainingPlan.TimeToNextDate, showTimeUnit: true);
                                    if (timeSpanLabel.TimeSpanIsNegative) { %>
                                        <a style="display: inline-block;" data-btn="startLearningSession" href="/Termin/Lernen/<%=Model.DateToLearn.Id %>">startet jetzt!</a>
                                    <% } else { %>
                                        in <span class="TPTimeToNextTrainingDate"><%= timeSpanLabel.Full %></span> 
                                    <% } %>
                                    (<span class="TPQuestionsInNextTrainingDate"><%= Model.TrainingPlan.QuestionCountInNextDate %></span> Fragen)<br/>
                                <% } %>
                            </p>
                        <% } else { %>
                            <p>Für diesen Termin sind keine weiteren Übungssitzungen geplant.</p>
                        <% } %>
                    </div>
                </div>
                
            <% } %>
        </div>
    </div>


</asp:Content>