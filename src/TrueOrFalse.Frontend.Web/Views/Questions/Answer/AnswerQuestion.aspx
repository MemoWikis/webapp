<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    
    <%= Scripts.Render("~/bundles/AnswerQuestion") %>

    <style type="text/css">
         .selectorShowAnswer{/* marker class */}
       
        div.headerLinks {}
        div.headerLinks i { margin-top: 2px;}
        
        .questionBlockWidth { }
        
        #sparklineTrueOrFalseTotals{ position: relative;top: 1px; }
        #sparklineTrueOrFalseUser{ position: relative;top: 1px; }

        .valRow .valColumn2 .imgDelete{position: relative; left: 10px;top: -3px;  }
        .valRow .valColumn2 .valMine{margin-top: -2px; padding-top: 0px;padding-left: 5px; float: left; }
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

    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-6">
        <p class="questionBlockWidth" style="padding-bottom:12px; margin-top:0px; font-size: 22px;">
            <%= Model.QuestionText %>
        </p>
            
        <p><%= Model.QuestionTextMarkdown %></p>
            
        <% if (Model.HasSound){ Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>

        <div class="col-md-10 alert alert-info" id="divCorrectAnswer" style="display: none; background-color: white; color:#2E487B;">
            <b>Antwort:</b>
            <span id="spanCorrectAnswer"></span>
            <p style="padding-top:10px;">
                <b>Erklärung:</b>
                <span id="spanAnswerDescription"></span>
            </p>
        </div>
        
        <div class="alert alert-danger col-md-10" id="divWrongAnswer" style="display: none">
            <b>Falsche Antwort </b>
            <a href="#" id="errorTryCount" style="float: right; margin-right: -5px;">(zwei Versuche)</a><br/>
                
            <div style="margin-top:5px;" id="answerFeedback">Du könntest es wenigstens probieren!</div>
                
            <div style="margin-top:7px; display: none;" id="divAnswerHistory" >
                Historie:
                <ul style="padding-top:5px;" id="ulAnswerHistory">
                </ul>
            </div>
        </div>
        
        <div class="row ">
            <div class="col-md-10">
                <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>"/>
                <%
                    string userControl = "SolutionType" + Model.SolutionType + ".ascx";
                    if (Model.SolutionMetadata.IsDate)
                        userControl = "SolutionTypeDate.ascx";
                        
                    Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel); 
                %>
            </div>
        </div>
            
        <div class="row" style="margin-bottom: 10px; margin-top: 10px;">
            <div class="col-md-10">
                <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
                <div id="buttons-first-try">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnCheck" class="btn btn-primary pull-right">Antworten</a>
                </div>
                <div id="buttons-next-answer" style="display: none;">
                    
                <div class="" id="divAnsweredCorrect" style="display: none; float:left; margin-top:5px; width: 250px;">
                    <b style="color: green;">Richtig!</b> <span id="wellDoneMsg"></span>
                </div>
                <a href="<%= Model.NextUrl(Url) %>" id="btnNext" class="btn btn-success pull-right">N&auml;chste Frage</a>
                </div>
                <div id="buttons-edit-answer" style="display: none;">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnEditAnswer" class="btn btn-warning pull-right">Antwort &Uuml;berarbeiten</a>
                </div>
                <div id="buttons-answer-again" style="display: none">
                    <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                    <a href="#" id="btnCheckAgain" class="btn btn-warning pull-right">Nochmal Antworten</a>
                </div>
            </div>
        </div>
                        
        <div class="row valRow" style="margin-top: 25px;">
            <div class="valColumn1 col-md-5">
                <h4>Allgemeine Einschätzung</h4>
            </div>
            <div class="valColumn2 col-md-5">
                <h4>Meine Einschätzung</h4>
            </div>
        </div>
        
        <div class="col-md-10" style="border-bottom: 1px solid silver; margin-bottom: 15px;">
            
        </div>
            
        <% foreach (var row in Model.FeedbackRows){ %>
            <div class="row valRow">
                <div class="valColumn1 col-md-5">
                    <%= row.Title %>: <i class="fa fa-user"></i><span id="span<%= row.Key%>Count">&nbsp;<%= row.FeedbackCount %></span> Ø <span id="span<%= row.Key%>Average"><%= row.FeedbackAverage %></span>
                </div>
                        
                <div id="div<%= row.Key%>Slider" class="valColumn2 col-md-5" <% if(!row.HasUserValue){ %> style="display:none"  <% } %> >
                    <div id="slider<%= row.Key %>" class="col-md-7 ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all">
                        <div class="ui-slider-range ui-widget-header ui-slider-range-min"></div>
                        <a class="ui-slider-handle ui-state-default ui-corner-all" href="#"></a>
                    </div>
                    <a href="#" id="remove<%= row.Key %>Value"><img src="/Images/delete.png" class="imgDelete"></a>
                    <span id="slider<%= row.Key %>Value" class="valMine"><%= row.UserValue%></span>
                </div>
                <div id="div<%= row.Key %>Add" class="valColumn2 col-md-5" <% if(row.HasUserValue){ %> style="display:none"  <% } %>>
                    <a href="#" id="select<%= row.Key %>Value">- Einschätzung hinzfügen <i class="fa fa-plus"></i> ---</a>
                </div>
            </div>
        <%} %>
    
        <div class="" style="margin-top: 20px; width: 400px;">
            Die Frage bitte: &nbsp;
            <a href="#modalImprove" data-toggle="modal"><i class="fa fa-repeat"></i> verbessern!</a>&nbsp; / 
            <a href="#modalDelete" data-toggle="modal"><i class="fa fa-fire"></i> entfernen!</a>
        </div>

    </div>

    <div class="col-md-3">
        <div class="headerLinks" style="margin-top:8px; line-height: 25px;">
            <% if(Model.HasPreviousPage){ %>
                <a href="<%= Model.PreviousUrl(Url) %>"><i class="fa fa-arrow-left"></i></a>
            <% } %>
            <span><%= Model.PageCurrent %> von <%= Model.PagesTotal %></span>
            <% if (Model.HasNextPage) { %>
                <a href="<%= Model.NextUrl(Url) %>"><i class="fa fa-arrow-right"></i> </a>
            <% } %>
            
            <% if (Model.SourceIsSet){ %>
                <br/>                
                Fragesatz:
                <a href="<%= Links.SetDetail(Url, Model.Set) %>">
                    <span class="label label-set"><%= Model.Set.Name %></a>
                </a>            
            <% } %>
        </div>
        
        <div style="padding-top:12px;">
            <% if (Model.SourceIsTabWish || Model.SourceIsTabMine || Model.SourceIsTabAll){ %>
                <a href="<%= QuestionSearchSpecSession.GetUrl(Url, Model.PagerKeyOverviewPage) %>">
                    <i class="fa fa-list"></i> 
                    <% if(Model.SourceIsTabWish){ %> mein Wunschwissen <%} %>
                    <% if(Model.SourceIsTabMine){ %> meine Fragen <%} %>
                    <% if(Model.SourceIsTabAll){ %> alle Fragen <%} %>
                </a>
                <br style="line-height: 10px;"/>
            <% } %>            
            
            <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}, null) %>"><i class="fa fa-pencil"></i> bearbeiten</a>
        </div>            

        <p style="padding-top: 12px;">
            von: <a href="<%= Links.UserDetail(Url, Model.Creator) %>"><%= Model.CreatorName %></a><br />
            vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />
        </p>
        
        <% if(Model.Categories.Count > 0){ %>
            <p style="padding-top: 10px;">
                <% foreach (var category in Model.Categories){ %>
                    <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category" style="margin-top: 3px;"><%= category.Name %></span></a>    
                <% } %>
            </p>
        <% } %>
        
        <% if(Model.SetMinis.Count > 0){ %>
            <% foreach (var setMini in Model.SetMinis){ %>
                <a href="<%= Links.SetDetail(Url, setMini) %>" style="margin-top: 3px; display: inline-block;"><span class="label label-set"><%: setMini.Name %></span></a>
            <% } %>
        
            <% if (Model.SetCount > 5){ %>
                <div style="margin-top: 3px;">
                    <a href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount -5 %> weitere </a>
                </div>
            <% } %>

        <% } %>
    
        <p style="padding-top: 10px;">
            <span class="show-tooltip" title="Insgesamt <%=Model.TimesAnsweredTotal%>x beantwortet."><%=Model.TimesAnsweredTotal%>x </span>
            <span id="sparklineTrueOrFalseUser" data-answersTrue="<%= Model.TimesAnsweredUserTrue  %>" data-answersFalse="<%= Model.TimesAnsweredUserWrong %>"></span>
            <span class="show-tooltip" title="Von Dir <%=Model.TimesAnsweredUser%>x beantwortet.">(ich <%= Model.TimesAnsweredUser%>x </span>
            <span id="sparklineTrueOrFalseTotals" data-answersTrue="<%= Model.TimesAnsweredCorrect %>" data-answersFalse="<%= Model.TimesAnsweredWrongTotal %>"></span>
            
            <br />
            <span class="show-tooltip" data-html="true"
                title="
                    <div style='text-align:left;'>
                        Wahrscheinlichkeit, dass Du die Frage korrekt beantwortest: <b><%: Model.CorrectnessProbability %>%</b><br /><br />
                        <b><%: Model.CorrectnessProbabilityDerivation %>%</b> besser als die korrekte Anwortwahrscheinlichkeit für alle Nutzer 
                        (<%: Model.CorrectnessProbability + Model.CorrectnessProbabilityDerivation %>%). 
                    </div>">
                <i class="fa fa-tachometer" style="color:green;"></i> <%: Model.CorrectnessProbability %>% + 8    
            </span>
            
        </p>
        
        <p>
            <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                <i class="fa fa-eye"></i> <%= Model.TotalViews %>x<br />
            </span>
        </p>
            
        <p style="padding-top:12px;">
            Feedback: 
            <a href="#">4x <i class="fa fa-repeat"></i></a>
            <a href="#">2x <i class="fa fa-fire"></i></a>
        </p>
            
        <p style="width: 150px;">
            <div class="fb-like" data-send="false" data-layout="button_count" data-width="100" data-show-faces="false" data-action="recommend" data-font="arial"></div>
        </p>
    </div>
    
    <%--MODAL IMPROVE--%>
    <div id="modalImprove" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
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
        </div>
    </div>
    
    <%--MODAL DELETE--%>
    <div id="modalDelete" class="modal fade">
         <div class="modal-dialog">
            <div class="modal-content">
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
                    <a href="#" class="btn btn-default" data-dismiss="modal" id="A1">Schliessen</a>
                    <a href="#" class="btn btn-primary btn-danger" id="A2">Absenden</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>