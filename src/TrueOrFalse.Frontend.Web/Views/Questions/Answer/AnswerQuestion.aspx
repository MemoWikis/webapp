<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Web" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Frage - <%= Model.QuestionText %></title>    
    <%= Scripts.Render("~/bundles/AnswerQuestion") %>

    <style type="text/css">
         .selectorShowAnswer{/* marker class */}
       
        div.headerLinks {}
        div.headerLinks i { margin-top: 2px;}
        
        .questionBlockWidth { }
        
        .sparklineTotals{ position: relative;top: 1px; }
        .sparklineTotalsUser{ position: relative;top: 1px; }

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
    <div class="row">
        <div class="col-lg-9 col-xs-9 xxs-stack">
            <ul class="pager" style="margin-top: 0;">
                <li class="previous <%= Model.HasPreviousPage ? "" : "disabled" %>">
                    <a href="<%= Model.PreviousUrl(Url) %>"><i class="fa fa-arrow-left"></i></a>
                </li>
                <li>
                    <% if (Model.SourceIsSet){ %>
                        <a href="<%= Links.SetDetail(Url, Model.Set) %>">
                            Fragesatz:
                            <span class="label label-set"><%= Model.Set.Name %></span>
                        </a>            
                    <% } %>
                    
                    <% if (Model.SourceIsTabWish || Model.SourceIsTabMine || Model.SourceIsTabAll){ %>
                        <a href="<%= QuestionSearchSpecSession.GetUrl(Url, Model.PagerKeyOverviewPage) %>">
                            
                            <span >
                                <i class="fa fa-list"></i> 
                                <% if(Model.SourceIsTabWish){ %> mein Wunschwissen <%} %>
                                <% if(Model.SourceIsTabMine){ %> meine Fragen <%} %>
                                <% if(Model.SourceIsTabAll){ %> alle Fragen <%} %>
                            </span>
                        </a>
                    <% } %>                    
                </li>
                <li>
                    <span><%= Model.PageCurrent %> von <%= Model.PagesTotal %></span>                
                </li>
                <li class="next">
                    <% if (Model.HasNextPage) { %>
                        <a href="<%= Model.NextUrl(Url) %>"><i class="fa fa-arrow-right"></i> </a>
                    <% } %>                
                </li>
            </ul>            
        </div>
        
        <div class="col-md-3">
            <% if(Model.IsOwner){ %>
                <div>            
                    <a href="<%= Links.EditQuestion(Url, Model.QuestionId) %>" style="">
                        <i class="fa fa-pencil pull-right" style="font-size: 19px; position: relative; top: 5px; opacity: 0.4"></i>
                    </a>
                </div>
            <% } %>
        </div>
    </div>
    <div class="row">

        <div class="col-lg-9 col-xs-9 xxs-stack">
            <div class="well">
                                
                <div style="float: right; margin-left: 10px;">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <% if (Model.IsInWishknowledge){ %>
                            <i class="fa fa-heart" id="iAdd" style="color:#b13a48;"></i>
                        <% } else {  %>
                            <i class="fa fa-heart-o" id="iAdd" style="color:#b13a48;"></i>
                        <% } %>
                        <i class="fa fa-spinner fa-spin hide2" id="iAddSpinner" style="color:#b13a48;"></i>
                    </a>
                </div>    
                <span style="font-size: 22px; padding-bottom: 20px;">
                    <%= Model.QuestionText %>
                </span>
                

                <p><%= Model.QuestionTextMarkdown %></p>
            
                <% if (Model.HasSound){ Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>

                <div class="alert alert-info" id="divCorrectAnswer" style="display: none; background-color: white; color:#2E487B;">
                    <b>Antwort:</b>
                    <span id="spanCorrectAnswer"></span>
                    <p style="padding-top:10px;">
                        <b>Erklärung:</b>
                        <span id="spanAnswerDescription"></span>
                    </p>
                </div>
        
                <div class="alert alert-danger" id="divWrongAnswer" style="display: none">
                    <b>Falsche Antwort </b>
                    <a href="#" id="errorTryCount" style="float: right; margin-right: -5px;">(zwei Versuche)</a><br/>
                
                    <div style="margin-top:5px;" id="answerFeedback">Du könntest es wenigstens probieren!</div>
                
                    <div style="margin-top:7px; display: none;" id="divAnswerHistory" >
                        Historie:
                        <ul style="padding-top:5px;" id="ulAnswerHistory">
                        </ul>
                    </div>
                </div>
        
                <div>
                    <input type="hidden" id="hddSolutionMetaDataJson" value="<%: Model.SolutionMetaDataJson %>"/>
                    <%
                        string userControl = "SolutionType" + Model.SolutionType + ".ascx";
                        if (Model.SolutionMetadata.IsDate)
                            userControl = "SolutionTypeDate.ascx";
                        
                        Html.RenderPartial("~/Views/Questions/Answer/AnswerControls/" + userControl, Model.SolutionModel); 
                    %>
                </div>
            
                <div style="margin-bottom: 10px; margin-top: 10px;">
                    <%--<%= Buttons.Submit("Überspringen", inline:true)%>--%>
                    <div id="buttons-first-try">
                        <a href="#" class="selectorShowAnswer">Antwort anzeigen</a>
                        <a href="#" id="btnCheck" class="btn btn-primary pull-right">Antworten</a>
                    </div>
                    
                    <div id="buttons-next-answer" style="display: none; padding-top: 20px; ">
                    
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
                    
                    <div style="clear: both"></div>
                </div>
            </div>
            
            <div style="margin-top: 30px; color: darkgray; font-weight: bold;" class="row">
                <div class="col-lg-6">
                    <h4 style="padding:0; margin:0;">Kommentare</h4>    
                </div>
                
                <div class="col-lg-6" style="vertical-align: text-bottom; vertical-align: bottom; margin-top: 3px;">
                    
                    Die Frage bitte: &nbsp;
                    <a href="#modalImprove" data-toggle="modal"><i class="fa fa-repeat"></i> verbessern!</a>&nbsp; / 
                    <a href="#modalDelete" data-toggle="modal"><i class="fa fa-fire"></i> entfernen!</a>
                </div>
            </div>
  
            <div class="panel panel-default" style="margin-top: 7px;">
                <div class="panel-heading">
                    Robert Mischke
                    <span style="color: darkgray">vor 3 Tagen</span>
                </div>
                <div class="panel-body">
                    <div class="col-lg-2">
                        <img style="width:100%; border-radius:5px;" src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=250&amp;d=http%3A%2F%2Ftrueorfalse%2FImages%2Fno-profile-picture-250.png">
                    </div>
                    <div class="col-lg-10">
                        "Die Frage sollte verbessert werden"    
                    </div>
                </div>
            </div>
                        
            <div class="panel panel-default" style="margin-top: 7px;">
                <div class="panel-heading">Neuen Kommentar hinzufügen</div>
                <div class="panel-body">
                    <div class="col-lg-2">
                        <img style="width:100%; border-radius:5px;" src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=250&amp;d=http%3A%2F%2Ftrueorfalse%2FImages%2Fno-profile-picture-250.png">
                    </div>
                    <div class="col-lg-10">
                        <textarea style="width: 100%; min-height: 82px;" class="form-control" placeholder="Bitte höflich, freundlich und sachlich schreiben :-)"></textarea>
                    </div>
                    
                    <div class="col-lg-12" style="padding-top: 7px;">
                        <a href="#" class="btn btn-default pull-right">Speichern</a>
                    </div>
                </div>                

            </div>

        </div>
        
        
        <div class="col-md-3 well" style="background-color: white;">
            
            <p>
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
                <% Html.RenderPartial("AnswerHistory", Model.AnswerHistory); %> <br/>
                <% Html.RenderPartial("CorrectnessProbability", Model.CorrectnessProbability); %>
            </p>
        
            <p>
                <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                    <i class="fa fa-heart" style="color:silver;"></i> 
                    <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span><br />
                </span>                
                <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                    <i class="fa fa-eye" style="color:darkslategray;"></i> <%= Model.TotalViews %>x
                </span><br />
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
                                <li><a href="#">Die Frage ist beleidigend, abwertend oder rassistisch.</a></li>
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
    </div>
</asp:Content>