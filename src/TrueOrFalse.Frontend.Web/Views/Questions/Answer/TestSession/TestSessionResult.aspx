<%@ Page Title="Ergebnis Übungssitzung" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<TestSessionResultModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Ergebnis</title>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

/asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2 style="margin-bottom: 15px; margin-top: 0px;">
        <span class="">Ergebnis</span>
    </h2>
    

    <div class="row">
        <div class="col-sm-9 xxs-stack">
            <div class="stackedBarChartContainer">
                <% if (Model.NumberCorrectPercentage>0) {%>
                    <div class="stackedBarChart chartCorrectAnswer" style="width: <%=Model.NumberCorrectPercentage %>%;">
                        <%=Model.NumberCorrectPercentage %>% 
                    </div>
                <% } %>                
                <% if (Model.NumberWrongAnswersPercentage>0) {%>
                    <div class="stackedBarChart chartWrongAnswer" style="width: <%=Model.NumberWrongAnswersPercentage %>%;">
                        <%=Model.NumberWrongAnswersPercentage %>% 
                    </div>
                <% } %>                
                <% if (Model.NumberNotAnsweredPercentage>0) {%>
                    <div class="stackedBarChart chartNotAnswered" style="width: <%=Model.NumberNotAnsweredPercentage %>%;">
                        <%=Model.NumberNotAnsweredPercentage %>% 
                    </div>
                <% } %>                
            </div>

            <div class="SummaryText" style="clear: left;">
                <p>In dieser Übungssitzung hast du <%= Model.NumberQuestions %> Fragen gelernt und dabei</p>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctRight"><span class="sumPctSpan"><%=Model.NumberCorrectPercentage %>%</span></div></div>
                            <div class="col-xs-10 col-sm-9 sumExpl">beim 1. Versuch gewusst (<%=Model.NumberCorrectAnswers %> Fragen)</div>
                        </div>
                        <div class="row">
                            <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctWrong"><span class="sumPctSpan"><%=Model.NumberWrongAnswersPercentage %>%</span></div></div>
                            <div class="col-xs-10 col-sm-9 sumExpl">nicht gewusst (<%=Model.NumberWrongAnswers %> Fragen)</div>
                        </div>
                        <div class="row">
                            <div class="col-xs-2 col-sm-offset-1 sumPctCol"><div class="sumPct sumPctNotAnswered"><span class="sumPctSpan"><%=Model.NumberNotAnsweredPercentage %>%</span></div></div>
                            <div class="col-xs-10 col-sm-9 sumExpl">übersprungen (<%=Model.NumberNotAnswered %> Fragen)</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="buttonRow">
                <a href="<%= Url.Action(Links.KnowledgeAction, Links.KnowledgeController) %>" class="btn btn-primary" style="padding-right: 10px">
                    Zur Wissenszentrale
                </a>
            </div>
            
            <div id="detailedAnswerAnalysis">
                <h3>Auswertung der Antworten</h3>
                <p style="color: silver; font-size: 11px;">
                    <a href="#" data-action="showAllDetails">Alle Details einblenden</a> | <a href="#" data-action="hideAllDetails">Alle Details ausblenden</a> | <a href="#" data-action="showDetailsExceptRightAnswer">Details zu allen nicht korrekten Fragen einblenden</a>
                </p>

            </div>
        </div>


        <div class="col-sm-3 xxs-stack">
            <div class="boxInfo">
                <div class="boxInfoHeader">
                    Fragesatz-Info
                </div>
                <div class="boxInfoContent">
                    <p>
                        Du hast diesen Fragesatz gelernt:<br />
<%--                        <a href="<%= Links.SetDetail(Url, Model.LearningSession.SetToLearn) %>" style="display: inline-block;">
                            <span class="label label-set"><%: Model.LearningSession.SetToLearn.Name %></span>
                        </a> (insgesamt <%=Model.LearningSession.TotalPossibleQuestions %> Fragen)--%>
                    </p>
                </div>
            </div>
        </div>
    </div>


</asp:Content>