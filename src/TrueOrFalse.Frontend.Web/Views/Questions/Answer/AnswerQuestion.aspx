<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Core.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="<%= Url.Content("~/Views/Questions/Answer/AnswerQuestion.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        
        <div class="span6">
            <h2>
                Frage beantworten
                <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>"><i class="icon-chevron-left"></i>&nbsp;zu den Fragen</a>
            </h2>
            
            <br/>
            <h4><%= Model.QuestionText %></h4>
        
            <textarea id="txtAnswer" style="height: 30px; width: 430px;"></textarea>

            <div>
                <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
                <div id="buttons-first-try">
                    <div style="float: right; margin-right: 20px; ">
                        <a href="<%= Model.AnswerQuestionLink(Url) %>" id="btnCheck" class="btn btn-primary">Antworten</a>
                    </div>
                </div>
                <div id="buttons-correct-answer" style="display: none">
                    <div style="float: right; margin-right: 20px; ">
                        <a href="#" id="btnNext" class="btn btn-success">N&auml;chste Frage</a>
                    </div>
                </div>
                <div id="buttons-edit-answer" style="display: none">
                    <a href="#" id="btnShowAnswer">Antwort anzeigen</a>
                    <div style="float: right; margin-right: 20px; ">
                    
                    <a href="#" id="btnEditAnswer" class="btn btn-warning">Antwort &Uuml;berarbeiten</a>
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
        
        <div class="span3">
            Erstellt von: <br />
            am: <br />
            Über die Antwort
            <%= Model.TimesAnswered %>x beantwortet<br />
            <%= Model.TimesAnsweredCorrect %>x richtig<br />
            <%= Model.TimesAnsweredWrong %>x falsche<br />
            <%= Model.TimesJumpedOver %>x übersprungen<br />
            druchschn. Antwortzeit <%= Model.AverageAnswerTime %><br />            
        </div>
        
    </div>

</asp:Content>
