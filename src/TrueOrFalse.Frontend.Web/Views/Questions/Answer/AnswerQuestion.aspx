﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="/Views/Questions/Answer/AnswerQuestion.js" type="text/javascript"></script>
    
    <style type="text/css">
        
        .selectorShowAnswer{/* marker class */}
        
        .btnRight {
            float: right;
            position: relative;right: 43px;
        }
        
        div.headerLinks {}
        div.headerLinks i { margin-top: 2px;}
        
        .questionBlockWidth { width: 400px;}
        
        #sparklineTrueOrFalseTotals{ position: relative;top: 1px; }
        #sparklineTrueOrFalseUser{ position: relative;top: 1px; }
    </style>
    
    <script type="text/javascript">
        var questionId = "<%= Model.QuestionId %>";
        var qualityAvg = "<%= Model.TotalQualityAvg %>";
        var qualityEntries = "<%= Model.TotalQualityEntries %>";

        var relevancePeronalAvg = "<%= Model.TotalRelevancePersonalAvg %>";
        var relevancePersonalEntries = "<%= Model.TotalRelevancePersonalEntries %>";
        var relevanceForAllAvg = "<%= Model.TotalRelevanceForAllAvg %>";
        var relevanceForAlleEntries = "<%= Model.TotalRelevanceForAllEntries %>";

        var ajaxUrl_SendAnswer = "<%= Model.AjaxUrl_SendAnswer(Url) %>";
        var ajaxUrl_GetAnswer = "<%= Model.AjaxUrl_GetAnswer(Url) %>";
    </script>

    <script type="text/javascript" src="/Scripts/jquery.jplayer.min.js"></script>
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="span6">
        <p class="questionBlockWidth" style="padding-bottom:12px; margin-top:10px; font-size: 22px;">
            <%= Model.QuestionText %>
        </p>
            
        <p><%= Model.QuestionTextMarkdown %></p>
            
        <% if (Model.HasSound){ Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>

        <div class="alert alert-info" id="divCorrectAnswer" style="display: none; margin-top:5px; width: 360px; background-color: white; color:#2E487B;">
            <b>Antwort:</b>
            <span id="spanCorrectAnswer"></span>
            <p style="padding-top:10px;">
                <b>Erklärung:</b>
                <span id="spanAnswerDescription"></span>
            </p>
        </div>
            
        <div class="alert alert-error" id="divWrongAnswer" style="display: none; margin-top:5px; width: 360px;">
            <b>Falsche Antwort </b>
            <a href="#" id="errorTryCount" style="float: right; margin-right: -30px;">(zwei Versuche)</a><br/>
                
            <div style="margin-top:5px;" id="answerFeedback">Du könntest es wenigstens probieren!</div>
                
            <div style="margin-top:7px; display: none;" id="divAnswerHistory" >
                Historie:
                <ul style="padding-top:5px;" id="ulAnswerHistory">
                </ul>
            </div>
        </div>
        
        <%Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/SolutionType" + Model.SolutionType +".ascx", Model.SolutionModel); %>
            
        <div>
            <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
            <div id="buttons-first-try">
                <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                <a href="#" id="btnCheck" class="btn btn-primary btnRight">Antworten</a>
            </div>
            <div id="buttons-next-answer" style="display: none;">
                    
                <div class="" id="divAnsweredCorrect" style="display: none; float:left; margin-top:5px; width: 250px;">
                    <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                </div>
                <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-success btnRight">N&auml;chste Frage</a>
            </div>
            <div id="buttons-edit-answer" style="display: none;">
                <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                <a href="#" id="btnEditAnswer" class="btn btn-warning btnRight">Antwort &Uuml;berarbeiten</a>
            </div>
            <div id="buttons-answer-again" style="display: none">
                <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                <a href="#" id="btnCheckAgain" class="btn btn-warning btnRight">Nochmal Antworten</a>
            </div>
        </div>
                        
        <style type="text/css">
            .val .valRow{ <%--background-color: blue;--%> width: 410px; margin-top: 1px; }
            .val .valRow .valColumn1{ <%--background-color: yellowgreen;--%> width: 200px; margin-left: 0px;  }
            .val .valRow .valColumn2{ <%--background-color: red;--%> width: 190px; height: 20px;padding-top: 3px; }
            .val .valRow .valColumn2 .ui-slider{ width: 140px;float: left; }
            .val .valRow .valColumn2 .imgDelete{ margin-top: -2px;padding-left: 5px; float: left; }
            .val .valRow .valColumn2 .valMine{margin-top: -2px; padding-top: 0px;padding-left: 5px; float: left; }
        </style>

        <div class=" val" style="padding-top:20px; width: 400px;">
                
            <div class="valRow" style="border-bottom: 1px solid silver; margin-bottom: 5px;">
                <div class="valColumn1 span3">
                    <h4>Allgemeine Einschätzung</h4>
                </div>
                <div class="valColumn2 span2">
                    <h4>Meine Einschätzung</h4>
                </div>
            </div>
            
            <div style="clear: both;"></div>
            
            <% foreach (var row in Model.FeedbackRows){ %>
                <div class="valRow">
                    <div class="valColumn1 span3">
                        <%= row.Title %>: <i class="icon-user"></i><span id="span<%= row.Key%>Count">&nbsp;<%= row.FeedbackCount %></span> Ø <span id="span<%= row.Key%>Average"><%= row.FeedbackAverage %></span>
                    </div>
                        
                    <div id="div<%= row.Key%>Slider" class="valColumn2 span2" <% if(!row.HasUserValue){ %> style="display:none"  <% } %> >
                        <div id="slider<%= row.Key %>" class="ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                            <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                            <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
                        </div>
                        <a href="#" id="remove<%= row.Key %>Value"><img src="/Images/delete.png" class="imgDelete"></a>
                        <span id="slider<%= row.Key %>Value" class="valMine"><%= row.UserValue%></span>
                    </div>
                    <div id="div<%= row.Key %>Add" class="valColumn2 span2" <% if(row.HasUserValue){ %> style="display:none"  <% } %>>
                        <a href="#" id="select<%= row.Key %>Value">- Einschätzung hinzfügen <i class="icon-plus"></i> ---</a>
                    </div>

                </div>
            <%} %>
        </div>
        
        <div style="clear: both;"></div>
            
        <div class="" style="margin-top: 20px; width: 400px;">
            Die Frage bitte: &nbsp;
            <a href="#modalImprove" data-toggle="modal"><i class="icon-repeat"></i> verbessern!</a>&nbsp; / 
            <a href="#modalDelete" data-toggle="modal"><i class="icon-fire"></i> entfernen!</a>
        </div>

    </div>

    <div class="span2">
        <div class="headerLinks" style="margin-top:8px; line-height: 25px;">
            <% if(Model.HasPreviousPage){ %>
                <a href="<%= Model.PreviousUrl(Url) %>"><i class="icon-arrow-left"></i></a>
            <% } %>
            <span><%= Model.PageCurrent %> von <%= Model.PagesTotal %></span>
            <% if (Model.HasNextPage) { %>
                <a href="<%= Model.NextUrl(Url) %>"><i class="icon-arrow-right"></i> </a>
            <% } %>
        </div>

        <p style="padding-top: 0px;">
            von: <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.CreatorName %></a><br />
            vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />
        </p>
            
        <p style="padding-top:12px;">
            Feedback: 
            <a href="#">4x <i class="icon-repeat"></i></a>
            <a href="#">2x <i class="icon-fire"></i></a>
        </p>
            
        <p style="width: 150px;">
            <div class="fb-like" data-send="false" data-layout="button_count" data-width="100" data-show-faces="false" data-action="recommend" data-font="arial"></div>
        </p>

    </div>
        
    <div class="span2" style="height: 400px;">
        <div style="padding-top:12px;">
            <a href="<%= QuestionSearchSpecSession.GetUrl(Url, Model.PagerKeyOverviewPage) %>"><i class="icon-list"></i> zur Übersicht</a><br style="line-height: 10px;"/>
            <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}, null) %>"><i class="icon-pencil"></i> bearbeiten</a>
        </div>            
            
        <p style="padding-top: 43px;">
            <b style="color: darkgray">Ich</b> <br/>
            <%= Model.TimesAnsweredUser %> x beantwortet <span id="sparklineTrueOrFalseUser" data-answersTrue="<%= Model.TimesAnsweredUserTrue  %>" data-answersFalse="<%= Model.TimesAnsweredUserWrong %>"></span>
        </p>            
            
        <p>
            <b style="color: darkgray">Alle</b> <br/>
            <%= Model.TotalViews %> x gesehen<br />
            <%= Model.TimesAnsweredTotal %> x beantwortet <span id="sparklineTrueOrFalseTotals" data-answersTrue="<%= Model.TimesAnsweredCorrect %>" data-answersFalse="<%= Model.TimesAnsweredWrongTotal %>"></span><br/>
        </p>           
    </div>
    
    <%--MODAL IMPROVE--%>
    <div id="modalImprove" class="modal hide fade">
        <div class="modal-header">
            <button class="close" data-dismiss="modal">×</button>
            <h3>Dies Frage verbessern</h3>
        </div>
        <div class="modal-body">
            <div >
                <p>
                    Ich bitte darum, dass diese Frage verbessert wird weil: 
                </p>
                <ul>
                    <li><a href="#">Die Frage sollte privat sein.</a></li>
                    <li><a href="#">Die Quellen sind falsch.</a></li>
                    <li><a href="#">Die Quellen sind online nicht zu erreichen.</a></li>
                    <li><a href="#">Die Antwort ist nicht eindeutig.</a></li>
                    <li><a href="#">... ein anderer Grund.</a></li>
                </ul>
            </div>
            <p>
                Erläuterung zum Verbesserungsvorschlag (optional).
            </p>
            <textarea style="width: 500px;" rows="3"></textarea>
            
        </div>
        <div class="modal-footer">
            <a href="#" class="btn" data-dismiss="modal" id="btnCloseQuestionDelete">Schliessen</a>
            <a href="#" class="btn btn-primary btn-success" id="confirmQuestionDelete">Absenden</a>
        </div>
    </div>
    
    <%--MODAL DELETE--%>
    <div id="modalDelete" class="modal hide fade">
        <div class="modal-header">
            <button class="close" data-dismiss="modal">×</button>
            <h3>Dies Frage bitte löschen</h3>
        </div>
        <div class="modal-body">
            <div >
                <p>
                    Ich bitte darum, dass diese Frage gelöscht wird weil: 
                </p>
                <ul>
                    <li><a href="#">Die Frage ist Beleidigend, abwertend oder rassistisch.</a></li>
                    <li><a href="#">Urheberrechte werden verletzt.</a></li>
                    <li><a href="#">Es handelt sich um Spam.</a></li>
                    <li><a href="#">... ein anderer Grund.</a></li>
                </ul>
            </div>
            <p>
                Weiter Erläuterung (optional).
            </p>
            <textarea style="width: 500px;" rows="3"></textarea>
            
        </div>
        <div class="modal-footer">
            <a href="#" class="btn" data-dismiss="modal" id="A1">Schliessen</a>
            <a href="#" class="btn btn-primary btn-danger" id="A2">Absenden</a>
        </div>
    </div>

</asp:Content>
