<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<LearningSessionResultModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Ergebnis</title>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Lernsitzung Ergebnis</h2>

    <div>
        
        <div id="SummaryAll">
            <div class="DescrLeft">Vorgelegt:</div>
            <div class="DescrRight">
                <%= Model.TotalNumberSteps %>
                von
                <%= Model.LearningSession.SetToLearn.Questions().Count() %>
                <%= Language.SingularPlural(Model.TotalNumberSteps, "Frage", "Fragen") %>
                aus dem Fragesatz
                <%= Model.LearningSession.SetToLearn.Name %>
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
        <% if(Model.NumberSkipped != 0) { %>
            <div id="SummaryUnanswered">
                <div class="BarDescr">unbeantwortet:</div>
                <div class="ChartBarWrapper" style="width: <%= (Model.NumberSkippedPercentage * 0.5).ToString(CultureInfo.InvariantCulture) %>%">
                    <div class="ChartBar"></div>
                </div>
                <div class="ChartNumbers"> <%=Model.NumberSkipped %> 
                    (<%=Model.NumberSkippedPercentage %>%)
                </div>
            </div>
        <% } %>
    </div>
    
    <div style="margin-top: 30px;">
    <% foreach (var step in Model.LearningSession.Steps.Where(s => s.AnswerState != StepAnswerState.Uncompleted))
        {
            if (step.AnswerState == StepAnswerState.Answered && step.AnswerHistory != null)
            {
                if (step.AnswerHistory.AnsweredCorrectly())
                { %>
                    <div class="QuestionLearned AnsweredRight" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-check show-tooltip" title="Richtig beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>"><%= step.Question.GetShortTitle(150) %></a>
                    </div>
                <% }
                else
                {  %>
                    <div class="QuestionLearned AnsweredWrong" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-minus-circle show-tooltip" title="Falsch beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>"><%= step.Question.GetShortTitle(150) %></a>
                    </div>
                <%
                       
                }
            }
            else if (step.AnswerState == StepAnswerState.Skipped)
            { %>
                    <div  class="QuestionLearned Unanswered" style="white-space: nowrap; overflow: hidden; -moz-text-overflow:ellipsis; text-overflow:ellipsis;">
                        <i class="fa fa-circle-o show-tooltip" title="Nicht beantwortet"></i>
                        <a href="<%= Links.AnswerQuestion(Url, step.Question) %>"><%= step.Question.GetShortTitle(150) %></a>
                    </div>
                <%
            }
        } %>
    </div>
</asp:Content>