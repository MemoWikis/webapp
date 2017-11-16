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


    
<% if (Model.Category.CountQuestionsAggregated > 0)
   {
       var dummyQuestion = Sl.QuestionRepo.GetById(Model.Category.GetAggregatedQuestionIdsFromMemoryCache().FirstOrDefault());

       Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", new AnswerQuestionModel(dummyQuestion.Id));

   }
   else
   { %>
        <div class="NoQuestions" style="margin-top: 40px;">
            Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten.
        </div>
      
  <% } %>

<div id="AnswerBody">
    <input type="hidden" id="hddSolutionTypeNum" value="1" />
</div>



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
