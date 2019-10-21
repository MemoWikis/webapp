<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="QuestionDetails">
    <div class="row">
        <div class="separationBorderTop" style="min-height: 20px;"></div>
    </div>
    <div class="row question-details">
        <div class="col-lg-6">
            <span class=" category-set">
                <span id="Category">
                    <% if (Model.Categories.Count > 0)
                        { %>
                            <div id="ChipHeader"><%= Model.Categories.Count > 1 ? "Zu diesen Themen zugeordnet":"Zu diesem Thema zugeordnet" %>:</div> 
                            <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>
                    <%  } %>
                </span>
            </span>
        </div>
        <div class="col-lg-6 second-row">
            <div id="QuestionDetailsStatistic">
                <div id="StatsHeader">Statistik:</div> 
                <div class="personal-answer-probability question-details-row" style="display: flex;">
                    <div class="detail-icon-container" style="padding-top: 2px;">
                        <% Html.RenderPartial("~/Views/Shared/CorrectnessProbability.ascx", Model.HistoryAndProbability.CorrectnessProbability); %>             
                    </div>
                    <div class="question-details-label-double">
                        <span> Wahrscheinlichkeit, dass du diese Frage richtig beantwortest. 
                            <% if (Model.IsInWishknowledge){
                                   var status = Model.HistoryAndProbability.QuestionValuation.KnowledgeStatus;%>
                                Dein Wissensstand: <%= status.GetText() %> 
                            <% } %>

                        </span>
                    </div>

                </div>

                <div class="allAnswerCount question-details-row" style="display:flex">
                    <div class="detail-icon-container pie-chart">
                        <div>
                            <span class="sparklineTotals" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredWrongTotal %>"></span>
                        </div>
                    </div>
                    <% if (Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal > 0)
                       { %>
                        <span>Insgesamt <strong><%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%></strong>x beantwortet, davon <strong><%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %></strong> richtig.</span>
                    <% }
                       else
                       { %>
                        <span>Diese Frage wurde noch nie beantwortet.</span>
                    <% } %>
                </div>
                
                <div class="personalAnswerCount question-details-row" style="display:flex">
                    <div class="detail-icon-container pie-chart">
                        <div>
                            <span class="sparklineTotalsUser" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserWrong %>"></span>
                        </div>
                    </div>
                    <% if (Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser > 0)
                       { %>
                        <span>Von dir <strong><%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%></strong>x beantwortet, davon <strong><%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue %></strong> richtig.</span>
                    <% }
                       else
                       { %>
                        <span>Du hast diese Frage noch nie beantwortet.</span>
                    <% } %>
                </div>
                
                <div class="viewCount question-details-row" style="display:flex">
                    <div class="detail-icon-container">
                        <div>
                            <i class="fa fa-eye greyed"></i>
                        </div>
                    </div>
                    <span>Diese Frage wurde <strong><%= Model.TotalViews %></strong>x gesehen</span>
                </div>
                
                <div class="inWishKnowledgeCount question-details-row" style="display:flex">
                    <div class="detail-icon-container">
                        <div>
                            <i class="fa fa-heart"></i>
                        </div>
                    </div>
                    <span><strong><%= Model.TotalRelevancePersonalEntries %></strong>x im Wunschwissen</span>
                </div>

            </div>

        </div>
    </div>
</div>