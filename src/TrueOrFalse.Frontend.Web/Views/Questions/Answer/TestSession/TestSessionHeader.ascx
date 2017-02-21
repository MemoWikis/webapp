<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<link href="/Views/Questions/Answer/LearningSession/LearningSessionResult.css" rel="stylesheet" />

<div class="SessionHeading">
    Du machst einen Test mit 
    <% if (Model.TestSessionNumberOfSteps == Model.TestSession.TotalPossibleQuestions){ %>
        allen <%= Model.TestSession.TotalPossibleQuestions %>
    <% }else { %>
        <%= Model.TestSessionNumberOfSteps %> 
    <% } %>
        
    <% if(Model.TestSession.IsSetSession) { %>
        Fragen aus dem Fragesatz 
        <a href="<%= Links.SetDetail(Url, Model.TestSession.SetToTest) %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-set"><%: Model.TestSession.SetToTest.Name %></span>
        </a>
    <% } %>

    <% if(Model.TestSession.IsSetsSession) { %>
        Fragen aus "<%= Model.TestSession.SetListTitle %>" (<%= Model.TestSession.SetsToTest.Count %> Fragesätze)
    <% } %>

    <% if(Model.TestSession.IsCategorySession) { %>
        Fragen zum Thema 
        <a href="<%= Links.CategoryDetail(Model.TestSession.CategoryToTest.Name, Model.TestSession.CategoryToTest.Id) %>" style="margin-top: 3px; display: inline-block;">
            <span class="label label-category"><%: Model.TestSession.CategoryToTest.Name %></span>
        </a>
    <% } %>
        
    <% if (Model.TestSessionNumberOfSteps < Model.TestSession.TotalPossibleQuestions){ %>
        mit <%= Model.TestSession.TotalPossibleQuestions %> Fragen.
    <% } %>
</div>
<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <%= Model.TestSessionCurrentStep %> von <%= Model.TestSessionNumberOfSteps %></div>
    <div class="SessionType">
       <%-- <span class="show-tooltip"
            data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>kannst du dir die Lösung anzeigen lassen</li>
                    <li>kannst du Fragen überspringen</li>
                    <li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt</li>
                </ul>
            </div>"%>"
            data-html="true" style="float: left;">--%>
        <span>
            Testen 
            <%--<i class="fa fa-info-circle"></i>--%>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div class="ProgressBarSegment ProgressBarDone" style="width: <%= Model.TestSessionCurrentStepPercentage + "%" %>;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <%= Model.TestSessionCurrentStepPercentage %>%
            </div>
        </div>
        <% if (Model.TestSessionCurrentStepPercentage<100) {%>
            <div class="ProgressBarSegment ProgressBarLeft" style="width: <%= 100-Model.TestSessionCurrentStepPercentage %>%;"></div>
        <% } %>
            
    </div>
</div>