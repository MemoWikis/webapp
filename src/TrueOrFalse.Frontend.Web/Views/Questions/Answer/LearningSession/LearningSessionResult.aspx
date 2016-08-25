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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Lernen: Dein Ergebnis</h2>
    
    <style>
        .chart-container { 
            display: table;
            width: 100%
        }
        .chart2 {
            display: table-cell; /*inline-block would prevent to show div when 0-width, but vertical-centered only for table-cell. */
            text-align: center;
            overflow: hidden;
            height: 40px;
            vertical-align: middle;
        }
    </style>

    <div class="row">
        <div class="col-sm-9 xxs-stack">
<%--            <div class="chart-container" style="margin-top: 10px;">
                <div class="chart2 chartCorrectAnswer" style="width: 15%; background-color: yellowgreen">15%</div>
                <div class="chart2 chartCorrectAnswer" style="width: 1%; background-color: yellow">1%</div>
                <div class="chart2 chartCorrectAnswer" style="width: 70%; background-color: red">70%</div>
                <div class="chart2 chartCorrectAnswer" style="width: 14%; background-color: silver">14%</div>
            </div>--%>
            <div class="chart-container" style="margin: 10px 0;">
                <div class="chart2 chartCorrectAnswer" style="width: <%=Model.NumberCorrectPercentage %>%; background-color: yellowgreen">
                    <% if (Model.NumberCorrectPercentage>0) {%> <%=Model.NumberCorrectPercentage %> <%} %>
                </div>
                <div class="chart2 chartCorrectAnswer" style="width: <%=Model.NumberCorrectAfterRepetitionPercentage %>%; background-color: yellow">
                    <% if (Model.NumberCorrectAfterRepetitionPercentage>0) {%> <%=Model.NumberCorrectAfterRepetitionPercentage %> <%} %>
                </div>
                <div class="chart2 chartCorrectAnswer" style="width: <%=Model.NumberWrongAnswersPercentage %>%; background-color: red">
                    <% if (Model.NumberWrongAnswersPercentage>0) {%> <%=Model.NumberWrongAnswersPercentage %> <%} %>
                </div>
                <div class="chart2 chartCorrectAnswer" style="width: <%=Model.NumberNotAnsweredPercentage %>%; background-color: silver">
                    <% if (Model.NumberNotAnsweredPercentage>0) {%> <%=Model.NumberNotAnsweredPercentage %> <%} %>
                </div>
            </div>

            <div class="" style="clear: left;">
                In dieser Übungssitzung hast du <%= Model.NumberQuestions %> Fragen gelernt und dabei
                <div class="row">
                    <div class="col-xs-2" style="text-align: right; padding-right: 0;"><%=Model.NumberCorrectPercentage %>%</div>
                    <div class="col-xs-10" style="text-align: left;">beim 1. Versuch gewusst (<%=Model.NumberCorrectAnswers %> Fragen)</div>
                    <div class="col-xs-2" style="text-align: right; padding-right: 0;"><%=Model.NumberCorrectAfterRepetitionPercentage %>%</div>
                    <div class="col-xs-10" style="text-align: left;">beim 2. oder 3. Versuch gewusst (<%=Model.NumberCorrectAfterRepetitionAnswers %> Fragen)</div>
                    <div class="col-xs-2" style="text-align: right; padding-right: 0;"><%=Model.NumberWrongAnswersPercentage %>%</div>
                    <div class="col-xs-10" style="text-align: left;">nicht gewusst (<%=Model.NumberWrongAnswers %> Fragen)</div>
                    <div class="col-xs-2" style="text-align: right; padding-right: 0;"><%=Model.NumberNotAnsweredPercentage %>%</div>
                    <div class="col-xs-10" style="text-align: left;">übersprungen (<%=Model.NumberNotAnswered %> Fragen)</div>
                </div>
            </div>
            <div class="" style="margin-top: 20px; text-align: right;">
                <% if(Model.LearningSession.IsDateSession) { %>
                    <a href="/Termin/Lernen/<%=Model.LearningSession.DateToLearn.Id %>" class="btn btn-link show-tooltip" style="padding-right: 10px" title="Eine neue Lernsitzung zu diesem Termin beginnen">
                        Neue Lernsitzung
                    </a>
                    <a href="<%= Url.Action(Links.Knowledge, Links.KnowledgeController) %>" class="btn btn-primary" style="padding-right: 10px">
                        Zum Überblick
                    </a>
                <% } else if (Model.LearningSession.IsSetSession) { %>
                    <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" class="btn btn-link" style="padding-right: 10px">Zum Fragesatz (Übersicht)</a>
                    <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                        Neue Lernsitzung
                    </a>
                <% } else {
                    throw new Exception("buttons for this type of learning session not specified");
                } %>
            </div>

            <div id="detailedAnswerAnalysis">
                
            </div>

        </div>

        <div class="col-sm-3 xxs-stack">
            Du hast zu diesem Termin/Fragesatz gelernt:
            [Zur Terminübersicht]
            [Knowledge-Chart]
        </div>
    </div>

    
    
    
    

    <div style="margin-top: 200px; margin-bottom: 20px;">
        <% if (Model.LearningSession.IsDateSession)
           { %>
            <% Html.RenderPartial("~/Views/Dates/DateRow.ascx", new DateRowModel(Model.LearningSession.DateToLearn, hideEditPlanButton: true));
           } %>
    </div>
    <div class="well Chart">
        <div id="SummaryAll">
            <div class="TableRow">
                <div class="DescrLeft">Gelernt:</div>
                <div class="DescrRight">
                    <%= Model.NumberQuestions %>
                    von
                    <%= Model.LearningSession.TotalPossibleQuestions %>
                    <%= Language.SingularPlural(Model.NumberQuestions, "Frage", "Fragen") %>
                    <% if(Model.LearningSession.IsSetSession) { %>
                        aus dem Fragesatz
                        <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="display: inline-block;">
                            <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
                        </a>
                    <% } %>
                    <% if(Model.LearningSession.IsDateSession) { %>
                        aus dem Termin <a href="<%= Links.Dates() %>"><%= Model.LearningSession.DateToLearn.GetTitle() %></a>
                    <% } %>
                </div>
            </div>
        </div>
        <div id="SummaryRightAnswers">
            <div class="BarDescr">richtig:</div>
            <div class="ChartBarWrapper" style="width: <%= (Model.NumberCorrectPercentage * 0.5).ToString(CultureInfo.InvariantCulture) %>%">
                <div class="ChartBar"></div>
            </div>
            <div class="ChartNumbers"> <%=Model.NumberCorrectAnswers %> 
                <% if(Model.NumberCorrectAnswers != 0) { %>
                (<%=Model.NumberCorrectPercentage %>%)
                <% } %>
            </div>
        </div>
        <div id="SummaryWrongAnswers">
            <div class="BarDescr">falsch:</div>
            <div class="ChartBarWrapper" style="width: <%= (Model.NumberWrongAnswersPercentage * 0.5).ToString(CultureInfo.InvariantCulture) %>%">
                <div class="ChartBar"></div>
            </div>
            <div class="ChartNumbers"> <%=Model.NumberWrongAnswers %> 
                <% if(Model.NumberWrongAnswers != 0) { %>
                (<%=Model.NumberWrongAnswersPercentage %>%)
                <% } %>
            </div>
        </div>
        <% if(Model.NumberNotAnswered != 0) { %>
            <div id="SummaryUnanswered">
                <div class="BarDescr">unbeantwortet:</div>
                <div class="ChartBarWrapper" style="width: <%= (Model.NumberNotAnsweredPercentage * 0.5).ToString(CultureInfo.InvariantCulture) %>%">
                    <div class="ChartBar"></div>
                </div>
                <div class="ChartNumbers"> <%=Model.NumberNotAnswered %> 
                    (<%=Model.NumberNotAnsweredPercentage %>%)
                </div>
            </div>
        <% } %>
    </div>
    
    <div style="margin-top: 30px;">
    <% foreach (var step in Model.LearningSession.Steps.Where(s => s.AnswerState != StepAnswerState.Uncompleted))
        {
            if (step.AnswerState == StepAnswerState.Answered && step.Answer != null)
            {
                if (step.Answer.AnsweredCorrectly())
                { %>
                    <div class="QuestionLearned AnsweredRight" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-check show-tooltip" title="Richtig beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>">&nbsp;<%= step.Question.GetShortTitle(150) %></a>
                    </div>
                <% }
                else
                {  %>
                    <div class="QuestionLearned AnsweredWrong" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-minus-circle show-tooltip" title="Falsch beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>">&nbsp;<%= step.Question.GetShortTitle(150) %></a>
                    </div>
                <% }
            }
            else if (step.AnswerState == StepAnswerState.Skipped || step.AnswerState == StepAnswerState.NotViewedOrAborted)
            { %>
                    <div  class="QuestionLearned Unanswered" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-circle-o show-tooltip" title="Nicht beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>">&nbsp;<%= step.Question.GetShortTitle(150) %></a>
                    </div>
            <% }
        } %>
    </div>
    
    <div class="pull-right" style="margin-top: 20px;">
        <% if(Model.LearningSession.IsDateSession) { %>
            <a href="<%= Links.Dates() %>" class="btn btn-primary" style="padding-right: 10px">
                Zurück zur Terminübersicht
            </a>
        <% } else if (Model.LearningSession.IsSetSession) { %>
            <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" class="btn btn-link" style="padding-right: 10px">Zum Fragesatz (Übersicht)</a>
            <a href="<%= Links.StartLearningSession(Model.LearningSession) %>" class="btn btn-primary" style="padding-right: 10px">
                Neue Lernsitzung zu diesem Fragesatz
            </a>
        <% } else {
            throw new Exception("buttons for this type of learning session not specified");
        } %>
    </div>

</asp:Content>