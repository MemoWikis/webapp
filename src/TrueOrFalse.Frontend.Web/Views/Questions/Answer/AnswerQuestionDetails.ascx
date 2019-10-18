<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="QuestionDetails">
    <div class="row question-details">
        <div class="col-lg-6">
            <span class=" category-set">
                <span id="Category">
                    <% if (Model.Categories.Count > 0)
                        { %>
                            <span>Thema:&nbsp;</span> <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>
                    <%  } %>
                </span>
            </span>
        </div>
        <div class="col-lg-6 second-row">
            <div id="QuestionDetailsStatistic">
                <div class="personal-answer-probability question-details-row" style="display:flex">
                    <div class="detail-icon-container">
                        <% Html.RenderPartial("~/Views/Shared/CorrectnessProbability.ascx", Model.HistoryAndProbability.CorrectnessProbability); %>             
                    </div>
                    <span> Wahrscheinlichkeit, dass du diese Frage richtig beantwortest.</span>
                </div>

                <div class="allAnswerCount question-details-row" style="display:flex">
                    <div class="detail-icon-container pie-chart">
                        <span class="sparklineTotals" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredWrongTotal %>"></span>
                    </div>
                    <% if (Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal > 0)
                       { %>
                        <span>Insgesamt <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%>x beantwortet, davon <%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %> richtig.</span>
                    <% }
                       else
                       { %>
                        <span>Diese Frage wurde noch nie beantwortet.</span>
                    <% } %>
                </div>
                
                <div class="personalAnswerCount question-details-row" style="display:flex">
                    <div class="detail-icon-container pie-chart">
                        <span class="sparklineTotalsUser" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserWrong %>"></span>
                    </div>
                    <% if (Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser > 0)
                       { %>
                        <span>Von dir <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%>x beantwortet, davon <%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue %> richtig</span>
                    <% }
                       else
                       { %>
                        <span>Du hast diese Frage noch nie beantwortet.</span>
                    <% } %>
                </div>
                
                <div class="viewCount question-details-row" style="display:flex">
                    <div class="detail-icon-container">
                        <i class="fa fa-eye greyed"></i>
                    </div>
                    <span>Diese Frage wurde <%= Model.TotalViews %>x gesehen</span>
                </div>
                
                <div class="inWishKnowledgeCount question-details-row" style="display:flex">
                    <div class="detail-icon-container">
                        <i class="fa fa-heart"></i>
                    </div>
                    <span><%= Model.TotalRelevancePersonalEntries %>x im Wunschwissen</span>
                </div>


                <% if (Model.HistoryAndProbability.QuestionValuation.IsInWishKnowledge()) {
                    var status = Model.HistoryAndProbability.QuestionValuation.KnowledgeStatus; %>
                    
                    <div class="question-details-row" style="display:flex">
                        <div class="detail-icon-container"style="color: <%= status.GetColor() %>">
                            <i class="fas fa-check-circle"></i>
                        </div>
                        <span>Dein Wissensstand: <%= status.GetText() %></span>
                    </div>

                <% } %>

            </div>

        </div>
    </div>
</div>