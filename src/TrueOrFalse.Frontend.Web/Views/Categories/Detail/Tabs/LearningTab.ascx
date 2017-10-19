<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<input type="hidden" id="hddIsLearningSession" value="True" 
       data-learning-session-id="-1"
       data-current-step-guid=""
       data-current-step-idx=""
       data-is-last-step=""
       data-skip-step-index="" />

<input type="hidden" id="hddQuestionId" value="1"/>
<input type="hidden" id="hddCategoryId" value="<%= Model.Category.Id %>"/>
<input type="hidden" id="hddLearningSessionStarted" value="False"/>


    
<% if(Model.Category.CountQuestionsAggregated > 0)
    {
        var dummyQuestion = Sl.QuestionRepo.GetById(Model.Category.GetAggregatedQuestionIdsFromMemoryCache().FirstOrDefault());
           
        Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion.Id));

    } %>

<%--<div class="SessionBar">
    <div class="QuestionCount" style="float: right;">Abfrage <span id="CurrentStepNumber"></span> von <span id="StepCount"></span></div>
    <div class="SessionType">
        <span class="show-tooltip"
              data-original-title="<%= @"<div style='text-align: left;'>In diesem Modus
                <ul>
                    <li>wiederholst du personalisiert die Fragen, die du am dringendsten lernen solltest</li>
                    <li>kannst du dir die Lösung anzeigen lassen</li>
                    <li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt</li>
                </ul>
            </div>"%>" data-html="true" style="float: left;">
            Lernen
            <span class="fa-stack fa-1x" style="font-size: 10px; top: -1px;">
                <i class="fa fa-circle fa-stack-2x" style="color: #e1efb3;"></i>
                <i class="fa fa-info fa-stack-1x" style=""></i>
            </span>
        </span>
    </div>
    <div class="ProgressBarContainer">
        <div id="progressPercentageDone" class="ProgressBarSegment ProgressBarDone" style="width: 0%;">
            <div class="ProgressBarSegment ProgressBarLegend">
                <span id="spanPercentageDone">0%</span>
            </div>
        </div>
        <div class="ProgressBarSegment ProgressBarLeft" style="width: 100%;"></div>
    </div>
</div>--%>
<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
</div>

<%--<% if (Model.IsLearningSession) { %>
    <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", Model); %>
<% }else if (Model.IsTestSession) { %>
    <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model); %>
<% }else { %>
    <div class="AnswerQuestionHeader">
        <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionPager.ascx", Model); %>
    </div>
<% } %>


<% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
       new AnswerBodyModel(Model)); %>

<div class="row">
    <% if (!Model.IsLoggedIn && !Model.IsTestSession && !Model.IsLearningSession && Model.SetMinis.Any()) {
           var primarySet = Sl.R<SetRepo>().GetById(Model.SetMinis.First().Id); %>
        <div class="col-sm-6 xxs-stack">
            <div class="well CardContent" style="margin-left: 0; margin-right: 0; padding-top: 10px; min-height: 175px;">
                <h6 class="ItemInfo">
                    <span class="Pin" data-set-id="<%= primarySet.Id %>" style="">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                    </span>&nbsp;
                    Lernset mit <a href="<%= Links.SetDetail(Url,primarySet.Name,primarySet.Id) %>"><%= primarySet.Questions().Count %> Fragen</a>
                </h6>
                <h4 class="ItemTitle"><%: primarySet.Name %></h4>
                <div class="ItemText"><%: primarySet.Text %></div>
                <div style="margin-top: 8px; text-align: right;">
                    <a href="<%= Links.TestSessionStartForSet(primarySet.Name, primarySet.Id) %>" class="btn btn-primary btn-sm" role="button" rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                    </a>
                </div>
            </div>
        </div>
                
    <% } %>
    <div class="col-sm-6 xxs-stack">
        <% Html.RenderPartial("~/Views/Questions/Answer/AnswerQuestionDetails.ascx", Model); %>
    </div>
</div>--%>
<% if (!Model.Category.DisableLearningFunctions) { %>

    <div class="row BoxButtonBar">
        <div class="BoxButtonColumn">
            <% var tooltipGame = "Tritt zu diesem Thema gegen andere Nutzer im Echtzeit-Quizspiel an.";
               if (Model.CountSets == 0)
                   tooltipGame = "Noch keine Lernsets zum Spielen zu diesem Thema vorhanden"; %>

            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %> 
                <%= Model.CountSets == 0 ? "LookNotClickable" : "" %>"
                 data-original-title="<%= tooltipGame %>">
                <div class="BoxButtonIcon"><i class="fa fa-gamepad"></i></div>
                <div class="BoxButtonText">
                    <span>Spiel starten</span>
                </div>
                <% if (Model.CountSets > 0)
                   { %>
                    <a href="<%= Links.GameCreateFromCategory(Model.Id) %>" rel="nofollow"
                       data-allowed="logged-in" data-allowed-type="game">
                    </a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipDate = "Gib an, bis wann du alle Lernsets zu diesem Thema lernen musst und erhalte deinen persönlichen Lernplan.";
               if (Model.CountSets == 0)
                   tooltipDate = "Noch keine Lernsets zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %>
                <%= Model.CountSets == 0 ? "LookNotClickable" : "" %>"
                 data-original-title="<%= tooltipDate %>">
                <div class="BoxButtonIcon"><i class="fa fa-calendar"></i></div>
                <div class="BoxButtonText">
                    <span>Prüfungstermin anlegen</span> 
                </div>
                <% if (Model.CountSets > 0)
                   { %>
                    <a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipTest = "Teste dein Wissen mit " + Settings.TestSessionQuestionCount + " zufällig ausgewählten Fragen zu diesem Thema und jeweils nur einem Antwortversuch.";
               if (Model.CountSets == 0 && Model.CountAggregatedQuestions == 0)
                   tooltipTest = "Noch keine Lernsets oder Fragen zum Testen zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= Model.CountSets == 0 && Model.CountAggregatedQuestions == 0 ? "LookNotClickable" : "" %>"
                 data-original-title="<%= tooltipTest %>">
                <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
                <div class="BoxButtonText">
                    <span>Wissen testen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountAggregatedQuestions > 0)
                   { %>
                    <a href="<%= Links.TestSessionStartForCategory(Model.Name, Model.Id) %>" rel="nofollow"></a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipLearn = "Lerne zu diesem Thema genau die Fragen, die du am dringendsten wiederholen solltest.";
               if (Model.CountSets == 0 && Model.CountAggregatedQuestions == 0)
                   tooltipLearn = "Noch keine Lernsets oder Fragen zum Lernen zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %>
                <%= Model.CountSets == 0 && Model.CountAggregatedQuestions == 0 ? "LookNotClickable" : "" %>"
                 data-original-title="<%= tooltipLearn %>">
                <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
                <div class="BoxButtonText">
                    <span>Lernen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountAggregatedQuestions > 0)
                   { %>
                    <a href="<%= Links.StartCategoryLearningSession(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
                <% } %>
            </div>
        </div>
    </div>

<% } %>
