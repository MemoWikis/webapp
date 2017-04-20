<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<AnswerQuestionModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.QuestionText; %>
    <% if (Model.IsLearningSession || Model.IsTestSession ) { %>
        <meta name="robots" content="noindex" />
    <%}else { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <% } %>
    
    <meta name="description" content="<%= Model.DescriptionForSearchEngines %>"/>
    
    <meta property="og:title" content="<%: Model.QuestionText %>" />
    <meta property="og:url" content="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= GetQuestionImageFrontendData.Run(Model.Question).GetImageUrl(435, true, imageTypeForDummy: ImageType.Question).Url %>" />
    <meta property="og:description" content="<%= Model.DescriptionForFacebook %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    
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
        data-learning-session-id="<%= Model.IsLearningSession ? Model.LearningSession.Id : -1 %>"
        data-current-step-guid="<%= Model.IsLearningSession ? Model.LearningSessionStep.Guid.ToString() : "" %>"
        data-current-step-idx="<%= Model.IsLearningSession ? Model.LearningSessionStep.Idx : -1 %>"
        data-is-last-step="<%= Model.IsLastLearningStep %>"/>
    <input type="hidden" id="hddIsTestSession" value="<%= Model.IsTestSession %>" 
        data-test-session-id="<%= Model.IsTestSession ? Model.TestSessionId : -1 %>"
        data-current-step-idx="<%= Model.IsTestSession ? Model.TestSessionCurrentStep : -1 %>"
        data-is-last-step="<%= Model.TestSessionIsLastStep %>"/>
    <input type="hidden" id="hddQuestionId" value="<%= Model.QuestionId %>"/>

            <% if (Model.IsLearningSession) { %>
                   <% Html.RenderPartial("~/Views/Questions/Answer/LearningSession/LearningSessionHeader.ascx", Model); %>
            <% }else if (Model.IsTestSession) { %>
                   <% Html.RenderPartial("~/Views/Questions/Answer/TestSession/TestSessionHeader.ascx", Model); %>
            <% }else { %>
                <div class="AnswerQuestionHeader">
                        
                    <% Html.RenderPartial("~/Views/Questions/Answer/Sponsor.ascx", Model); %>

                    <div id="AnswerQuestionPager">
                        <div class="Current">
                        <% if (!String.IsNullOrEmpty(Model.PageCurrent) && !String.IsNullOrEmpty(Model.PagesTotal)){

                                if (Model.SourceIsCategory){ %>
                                        Frage <%= Model.PageCurrent %> von <%= Model.PagesTotal %> im Thema
                                        <% if(Model.SourceCategory.IsSpoiler(Model.Question)){ %>
                                            <a href="#" onclick="location.href='<%= Links.CategoryDetail(Model.SourceCategory) %>'" style="height: 30px">
                                                <span class="label label-category" data-isSpolier="true" style="position: relative; top: -1px;">Spoiler</span>
                                            </a>                    
                                        <% } else { %>
                                            <a href="<%= Links.CategoryDetail(Model.SourceCategory) %>" style="height: 30px">
                                                <span class="label label-category" style="position: relative; top: -1px;"><%= Model.SourceCategory.Name %></span>
                                            </a>
                                        <% } %>
                                <% }

                                else if (Model.SourceIsSet) { %>
                                    Frage <%= Model.PageCurrent %> von <%= Model.PagesTotal %> im Fragesatz 
                                
                                     <a href="<%= Links.SetDetail(Url, Model.Set) %>">
                                        <span class="label label-set"><%= Model.Set.Name %></span>
                                    </a>     
                                  
                                <% }
                            
                                else if (Model.SourceIsTabWish || Model.SourceIsTabMine || Model.SourceIsTabAll){ %>
                                    Suchtreffer: <%= Model.PageCurrent %> von  
                                    <a href="<%= QuestionSearchSpecSession.GetUrl(Model.SearchTabOverview) %>">  
                                        <%= Model.PagesTotal %> in                       
                                        <span>
                                            <% if (Model.SourceIsTabWish)
                                               { %> deinem Wunschwissen <% } %>
                                            <% if (Model.SourceIsTabMine)
                                               { %> deinen Fragen <% } %>
                                            <% if (Model.SourceIsTabAll)
                                               { %> allen Fragen <% } %>
                                        </span>
                                    </a>
                                <% }
                           } %> 
                        </div>
                        <div class="Previous" style="padding-right: 5px;">
                            <% if (Model.HasPreviousPage)
                                { %>
                                <a class="btn btn-sm btn-default" href="<%= Model.PreviousUrl(Url) %>" rel="nofollow"><i class="fa fa-chevron-left"></i><span class="NavButtonText"> vorherige Frage</span></a>
                            <% } %>
                        </div>
                        <div class="Next" style="padding-left: 5px;">
                            <% if (Model.HasNextPage)
                                { %>
                                <a class="btn btn-sm btn-default" href="<%= Model.NextUrl(Url) %>" rel="nofollow"><span class="NavButtonText">nächste Frage </span><i class="fa fa-chevron-right"></i></a>
                            <% } %>
                        </div>
                    </div>
                </div>
           <% } %>

        

    <div class="row">
        <div class="col-xs-12">
            
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
                                Fragesatz mit <a href="<%= Links.SetDetail(Url,primarySet.Name,primarySet.Id) %>"><%= primarySet.Questions().Count %> Fragen</a>
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
                        <div class="well" id="answerQuestionDetails" style="background-color: white; padding-bottom: 10px; min-height: 175px;">
                            <div class="row">
                                <div class="col-xs-6 xxs-stack">
                                    <p>
                                        von: <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a><%= Model.Visibility != QuestionVisibility.All ? " <i class='fa fa-lock show-tooltip' title='Private Frage'></i>" : "" %><br />
                                        vor <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText %></span> <br />
                                    </p>
                                    
                                    <% if (Model.IsOwner)
                                       { %>
                                        <%--<div class="navLinks">--%>
                                            <div id="EditQuestion">
                                                <a href="<%= Links.EditQuestion(Url, Model.QuestionText, Model.QuestionId) %>" class="TextLinkWithIcon">
                                                    <i class="fa fa-pencil"></i>
                                                    <span class="TextSpan">Frage bearbeiten</span>
                                                </a>
                                            </div>
            
                                            <div id="DeleteQuestion">
                                                <a class="TextLinkWithIcon" data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDeleteQuestion">
                                                    <i class="fa fa-trash-o"></i> <span class="TextSpan">Frage löschen</span>
                                                </a>
                                            </div>
                                        <%--</div>--%>
                                    <% } %>
        
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
                                </div>
                                <div class="col-xs-6 xxs-stack">
                                    <div style="padding-bottom: 20px;" id="answerHistory">
                                        <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
                                    </div>
        
                                    <p>
                                        <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                                            <i class="fa fa-heart greyed"></i> 
                                            <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span><br />
                                        </span>                
                                        <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                                            <i class="fa fa-eye"></i> <%= Model.TotalViews %>x
                                        </span><br />
                                    </p>

                                    <p style="width: 150px;">                    
                                        <div class="fb-share-button" style="margin-right: 10px; margin-bottom: 5px; float: left; " data-href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>
                    
                                        <div style="margin-top: 5px">
                                            <a style="white-space: nowrap" href="#" data-action="embed-question"><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</a>
                                        </div>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            <%--</div>--%>
            
            <% if (Model.ContentRecommendationResult != null) { %>
                <h4 style="margin-top: 30px;">Das könnte dich auch interessieren:</h4>
                <div class="row CardsLandscapeNarrow" id="contentRecommendation">
                    <% foreach (var set in Model.ContentRecommendationResult.Sets)
                       {
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                    <% foreach (var category in Model.ContentRecommendationResult.Categories)
                       {
                            Html.RenderPartial("Cards/CardSingleCategory", CardSingleCategoryModel.GetCardSingleCategoryModel(category.Id));
                       } %>
                    <% foreach (var set in Model.ContentRecommendationResult.PopularSets)
                       { 
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                </div>
            <% } %>

            <div class="row" style="margin-top: 30px; color: darkgray; font-weight: bold;">

                <div class="col-xs-4">
                    <h4 style="padding:0; margin:0;">Kommentare<a name="comments"></a></h4>    
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
                    <div class="comment">
                        <% Html.RenderPartial("~/Views/Questions/Answer/Comments/Comment.ascx", comment); %>
                    </div>
                <% } %>
                <% if (Model.CommentsSettledCount > 0) { %>
                    <div class="commentSettledInfo" style="margin: 5px 10px 15px;">
                        Diese Frage hat <%= Model.CommentsSettledCount %> 
                        <% if (Model.Comments.Any()) Response.Write("weitere "); %>
                        als erledigt markierte<%= StringUtils.PluralSuffix(Model.CommentsSettledCount,"","n") %> Kommentar<%= StringUtils.PluralSuffix(Model.CommentsSettledCount,"e") %>
                        (<a href="#" id="showAllCommentsInclSettled" data-question-id="<%= Model.QuestionId %>">alle anzeigen</a>).
                    </div>
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

</asp:Content>