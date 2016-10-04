<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Frage - <%= Model.QuestionText %></title>
    <%= Styles.Render("~/bundles/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/AnswerQuestion") %>
    <%= Scripts.Render("~/bundles/js/DeleteQuestion") %>

    <style type="text/css">
         .selectorShowSolution{/* marker class */}
               
        .sparklineTotals{ position: relative;top: 1px; }
        .sparklineTotalsUser{ position: relative;top: 1px; }

        .valRow .valColumn2 .imgDelete{position: relative; left: 10px;top: -3px;  }
        .valRow .valColumn2 .valMine{margin-top: -2px; padding-top: 0;padding-left: 5px; float: left; }
    </style>

    <script type="text/javascript">
        var questionId = "<%= Model.QuestionId %>";
        var qualityAvg = "<%= Model.TotalQualityAvg %>";
        var qualityEntries = "<%= Model.TotalQualityEntries %>";

        var relevancePeronalAvg = "<%= Model.TotalRelevancePersonalAvg %>";
        var relevancePersonalEntries = "<%= Model.TotalRelevancePersonalEntries %>";
        var relevanceForAllAvg = "<%= Model.TotalRelevanceForAllAvg %>";
        var relevanceForAlleEntries = "<%= Model.TotalRelevanceForAllEntries %>";
    </script>

    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <input type="hidden" id="hddIsLearningSession" value="<%= Model.IsLearningSession %>" 
        data-current-step-idx="<%= Model.IsLearningSession ? Model.LearningSessionStep.Idx : -1 %>"
        data-is-last-step="<%= Model.IsLastLearningStep %>"/>

    <div class="row">
        <div class="col-lg-9 col-xs-9 xxs-stack">
            <% if (Model.IsLearningSession)
               { %>
                   <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx",
                   Model); %>
                   <%--new LearningSessionModel(Model)); %>--%>
            <% }else{ %>
            <ul id="AnswerQuestionPager" class="pager" style="margin-top: 0;">
                <li class="previous <%= Model.HasPreviousPage ? "" : "disabled" %>">
                    <a href="<%= Model.PreviousUrl(Url) %>"><i class="fa fa-arrow-left"></i></a>
                </li>
                <li>
                    <% if (Model.SourceIsCategory){ %>
                    
                        <% if(Model.SourceCategory.IsSpoiler(Model.Question)){ %>
                            <a href="#" onclick="location.href='<%= Links.CategoryDetail(Model.SourceCategory) %>'" style="height: 30px">
                                Kategorie:
                                <span class="label label-category" data-isSpolier="true" style="position: relative; top: -3px;">Spoiler</span>
                            </a>                    
                        <% } else { %>
                            <a href="<%= Links.CategoryDetail(Model.SourceCategory) %>" style="height: 30px">
                                Kategorie:
                                <span class="label label-category" style="position: relative; top: -3px;"><%= Model.SourceCategory.Name %></span>
                            </a>
                        <% } %>

                    <% } %>
                    <% if (Model.SourceIsSet)
                       { %>
                        <a href="<%= Links.SetDetail(Url, Model.Set) %>">
                            Fragesatz:
                            <span class="label label-set"><%= Model.Set.Name %></span>
                        </a>            
                    <% } %>
                    
                    <% if (Model.SourceIsTabWish || Model.SourceIsTabMine || Model.SourceIsTabAll)
                       { %>
                        <a href="<%= QuestionSearchSpecSession.GetUrl(Model.SearchTabOverview) %>">                        
                            <span >
                                <i class="fa fa-list"></i> 
                                <% if (Model.SourceIsTabWish)
                                   { %> mein Wunschwissen <% } %>
                                <% if (Model.SourceIsTabMine)
                                   { %> meine Fragen <% } %>
                                <% if (Model.SourceIsTabAll)
                                   { %> alle Fragen <% } %>
                            </span>
                        </a>
                    <% } %>                    
                </li>
                <li>
                    <span><%= Model.PageCurrent %> von <%= Model.PagesTotal %></span>
                </li>
                <li class="next">
                    <% if (Model.HasNextPage)
                       { %>
                        <a href="<%= Model.NextUrl(Url) %>"><i class="fa fa-arrow-right"></i> </a>
                    <% } %>
                </li>
            </ul>
            <% } %>
        </div>

        <div class="col-xs-3 xxs-stack">
            <% if (Model.IsOwner)
               { %>
                <div id="EditQuestion">
                    <a href="<%= Links.EditQuestion(Url, Model.QuestionId) %>" class="TextLinkWithIcon">
                        <i class="fa fa-pencil"></i>
                        <span class="TextSpan">Frage bearbeiten</span>
                    </a>
                </div>
            
                <div id="DeleteQuestion">
                    <a data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDeleteQuestion">
                        <i class="fa fa-trash-o"></i> <span class="TextSpan">Frage löschen</span>
                    </a>
                </div>
            <% } %>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-9 xxs-stack">
            
            <% Html.RenderPartial("~/Views/Questions/Answer/AnswerBodyControl/AnswerBody.ascx",
                   new AnswerBodyModel(Model)); %>
            
            <div style="margin-top: 30px; color: darkgray; font-weight: bold;" class="row">

                <div class="col-xs-4">
                    <h4 style="padding:0; margin:0;">Kommentare</h4>    
                </div>
                
                <div class="col-xs-8 " style="vertical-align: text-bottom; 
                      vertical-align: bottom; margin-top: 3px; text-align: right">
                    <% if (Model.IsLoggedIn)
                       { %>
                        <span style="padding-right: 2px">
                            Die Frage bitte: &nbsp;
                            <a href="#modalQuestionFlagImprove" data-toggle="modal"><i class="fa fa-repeat">&nbsp;</i>verbessern!</a>&nbsp; / 
                            <a href="#modalQuestionFlagDelete" data-toggle="modal"><i class="fa fa-fire">&nbsp;</i>entfernen!</a>
                        </span>
                    <% } %>
                </div>
            </div>  
            
            <div id="comments">
                <% foreach (var comment in Model.Comments){ %>
                    <% Html.RenderPartial("~/Views/Questions/Answer/Comments/Comment.ascx", comment); %>
                <% } %>
            </div>
                        
            <% if (Model.IsLoggedIn)
               { %>
                <div class="panel panel-default" style="margin-top: 7px;">
                    <div class="panel-heading">Neuen Kommentar hinzufügen</div>
                    <div class="panel-body">
                        <div class="col-xs-2">
                            <img style="width:100%; border-radius:5px;" src="<%= Model.ImageUrlAddComment %>">
                        </div>
                        <div class="col-xs-10">
                            <i class="fa fa-spinner fa-spin hide2" id="saveCommentSpinner"></i>
                            <textarea style="width: 100%; min-height: 82px;" class="form-control" id="txtNewComment" placeholder="Bitte höflich, freundlich und sachlich schreiben :-)"></textarea>
                        </div>
                    
                        <div class="col-xs-12" style="padding-top: 7px;">
                            <a href="#" class="btn btn-default pull-right" id="btnSaveComment">Speichern</a>
                        </div>
                    </div>                
                </div>
            <% } else { %>
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-xs-12" style="padding-top: 10px; color: darkgray">
                        Um zu kommentieren, musst du eingeloggt sein.
                    </div>                     
                </div>
            <% } %>

        </div>
        
        <div class="col-sm-3 xxs-stack">
            <div class="well" id="answerQuestionDetails" style="background-color: white;">
                <p>
                    von: <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a><%= Model.Visibility != QuestionVisibility.All ? " <i class='fa fa-lock show-tooltip' title='Private Frage'></i>" : "" %><br />
                    vor <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText %></span> <br />
                </p>
        
                <% if (Model.Categories.Count > 0)
                    { %>
                    <p style="padding-top: 10px;">
                        <% Html.RenderPartial("Category", Model.Question); %>
                    </p>
                <% } %>
        
                <% if (Model.SetMinis.Count > 0)
                    { %>
                    <% foreach (var setMini in Model.SetMinis)
                        { %>
                        <a href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>
                    <% } %>
        
                    <% if (Model.SetCount > 5)
                        { %>
                        <div style="margin-top: 3px;">
                            <a href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount - 5 %> weitere </a>
                        </div>
                    <% } %>

                <% } %>
    
                <div style="padding-top: 20px; padding-bottom: 20px;" id="answerHistory">
                    <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
                </div>
        
                <p>
                    <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                        <i class="fa fa-heart" style="color:silver;"></i> 
                        <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span><br />
                    </span>                
                    <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                        <i class="fa fa-eye"></i> <%= Model.TotalViews %>x
                    </span><br />
                </p>

                <p style="width: 150px;">
                    <div class="fb-like" data-send="false" data-layout="button_count" data-width="100" data-show-faces="false" data-action="recommend" data-font="arial"></div>
                    <div style="margin-top: 5px">
                        <a data-toggle="modal" href="#modalEmbedQuestion"><i class="fa fa-share-alt" aria-hidden="true">&nbsp;</i>Einbetten</a>
                    </div>
                </p>
            </div>
        </div>
    
        <%--MODAL IMPROVE--%>
        <div id="modalQuestionFlagImprove" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button class="close" data-dismiss="modal">×</button>
                        <h3>Diese Frage verbessern</h3>
                    </div>
                    <div class="modal-body">
                        <div >
                            <p>
                                Ich bitte darum, dass diese Frage verbessert wird, weil: 
                            </p>
                            <ul style="list-style-type: none">
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="shouldBePrivate"/> 
                                            <%= ShouldReasons.ByKey("shouldBePrivate") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="sourcesAreWrong"/> 
                                            <%= ShouldReasons.ByKey("sourcesAreWrong") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="answerNotClear"/> 
                                            <%= ShouldReasons.ByKey("answerNotClear") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbImprove" value="improveOtherReason"/>
                                            <%= ShouldReasons.ByKey("improveOtherReason") %>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <p style="padding-top: 10px;">
                            Erläuterung zum Verbesserungsvorschlag:
                        </p>
                        <textarea style="width: 500px;" rows="3" id="txtImproveBecause"></textarea>
                        <p style="padding-top: 15px;">
                            Die Verbesserungsanfrage wird als Kommentar veröffentlicht und 
                            als Nachricht an <%= Model.CreatorName %> gesendet.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <a href="#" class="btn" data-dismiss="modal">Schliessen</a>
                        <a href="#" class="btn btn-primary btn-success" id="btnImprove">Absenden</a>
                    </div>
                </div>
            </div>
        </div>
    
        <%--MODAL QUESTION FLAG DELETE--%>
        <div id="modalQuestionFlagDelete" class="modal fade">
             <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button class="close" data-dismiss="modal">×</button>
                        <h3>Diese Frage bitte löschen</h3>
                    </div>
                    <div class="modal-body">
                        <div >
                            <p>
                                Ich bitte darum, dass diese Frage gelöscht wird, weil: 
                            </p>
                            <ul style="list-style-type: none">
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsOffending"/>
                                            <%= ShouldReasons.ByKey("deleteIsOffending") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsOffending"/>
                                            <%= ShouldReasons.ByKey("deleteCopyright") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteIsSpam"/>
                                            <%= ShouldReasons.ByKey("deleteIsSpam") %>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" name="ckbDelete" value="deleteOther"/>
                                            <%= ShouldReasons.ByKey("deleteOther") %>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <p>
                            Weitere Erläuterung (optional):
                        </p>
                        <textarea style="width: 500px;" rows="3" id="txtDeleteBecause"></textarea>
            
                    </div>
                    <div class="modal-footer">
                        <a href="#" class="btn btn-default" data-dismiss="modal" id="A1">Schließen</a>
                        <a href="#" class="btn btn-primary btn-danger" id="btnShouldDelete">Absenden</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
<% if (Model.IsOwner) Html.RenderPartial("~/Views/Questions/Modals/ModalDeleteQuestion.ascx"); %>
<% Html.RenderPartial("~/Views/Questions/Answer/Modal/ShareQuestion.ascx", Model); %>    

</asp:Content>