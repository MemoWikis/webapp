<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="<%= Url.Content("~/Views/Questions/Answer/AnswerQuestion.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Frage beantworten</h2>

    <div class="span-15">

        <p>Kategorien:</p>

        <h4><%= Model.QuestionText %></h4>
        
        <textarea id="txtAnswer" style="height: 30px;"></textarea>

        <div>
            <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
            <div id="buttons-first-try">
                <div style="float: right; margin-right: 175px; ">
                    <%= Buttons.Submit("Antworten", url: Model.AnswerQuestionLink(Url),  id: "btnCheck", inline: true)%>
                </div>
            </div>
            <div id="buttons-correct-answer" style="display: none">
                <div style="float: right; margin-right: 175px; ">
                    <%= Buttons.Submit("N&auml;chste Frage", url: "#",  id: "btnNext", inline: true)%>
                    </div>
            </div>
            <div id="buttons-edit-answer" style="display: none">
                <a href="#" id="btnShowAnswer">Antwort anzeigen</a>
                <div style="float: right; margin-right: 175px; ">
                    
                <%= Buttons.Submit("Antwort &Uuml;berarbeiten", url: "#",  id: "btnEditAnswer", inline: true)%>
                </div>
            </div>
            <div id="buttons-answer-again" style="display: none">
                <div style="float: right; margin-right: 175px; ">
                    <%= Buttons.Submit("Nochmal Antworten", url: Model.AnswerQuestionLink(Url), id: "btnCheckAgain", inline: true)%>
                </div>
            </div>
            <div id="answerFeedback" style="display: none; margin-top:12px; padding-right: 140px;">Du könntest es wenigstens probieren!</div>
            <div style="clear: both"></div>
        </div>
        
        <div id="divCorrectAnswer" style="display: none; margin-top:10px;">
            <b>Richtige Antwort:</b><br />
            <span id="spanCorrectAnswer"></span>
        </div>
        
    </div>

    <div class="span-5 last">

        Erstellt von: <br />
        am: <br />
        Über die Antwort
        <%= Model.TimesAnswered %>x beantwortet<br />
        <%= Model.TimesAnsweredCorrect %>x richtig<br />
        <%= Model.TimesAnsweredWrong %>x falsche<br />
        <%= Model.TimesJumpedOver %>x übersprungen<br />
        druchschn. Antwortzeit <%= Model.AverageAnswerTime %><br />

    </div>

</asp:Content>
