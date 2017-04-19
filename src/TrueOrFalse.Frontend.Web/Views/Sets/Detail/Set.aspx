<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<SetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.MetaTitle; %>

    <%var canonicalUrl = Settings.CanonicalHost + Links.SetDetail(Model.Name, Model.Id); %>    
    <link rel="canonical" href="<%= canonicalUrl %>">
    <meta name="description" content="<%= Model.MetaDescription %>">
    
    <meta property="og:title" content="<%: Model.Name %>" />
    <meta property="og:url" content="<%= canonicalUrl %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.QuestionSet).Url %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/Set") %>
    <%= Scripts.Render("~/bundles/js/Set") %>
    
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hhdSetId" value="<%= Model.Set.Id %>"/>
    <input type="hidden" id="hhdHasVideo" value="<%= Model.Set.HasVideo %>"/>

    <% if(Model.HasPreviousCategoy) { %>
        <div style="padding-bottom: 15px;">
            <div class="btn-group btn-breadcrumb MobileHide">
                <a href="<%= Model.PreviousCategoryUrl %>" class="btn btn-sm btn-default">
                    <i class="fa fa-angle-double-left" style="padding-right: 5px;"></i>zurück zum Thema "<span style="text-decoration: underline;"><%= Model.PreviousCategoryName %></span>"
                </a>
            </div>
        </div>
    <% } %>

    <div class="row">
        
        <%--<div class="col-xs-3 col-md-2 xxs-stack col-xs-push-9 col-md-push-10">--%>
        <div class="col-xs-12 col-md-2 col-md-push-10">
            <div class="navLinks">
                <a href="<%= Links.SetsAll() %>"><i class="fa fa-list">&nbsp;</i>Zur Übersicht</a>
                <% if(Model.IsOwner || Model.IsInstallationAdmin){ %>
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil">&nbsp;</i>Bearbeiten</a> 
                <% } %>
                                
                <% if(Model.IsInstallationAdmin) { %>
                    <a href="#" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                        <i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views
                    </a>    
                <% } %>
            </div>
        </div>
        

       <%-- <div class="xxs-stack col-xs-9 col-md-10 col-xs-pull-3 col-md-pull-2">--%>
        <div class="col-xs-12 col-md-10 col-md-pull-2">
            <div id="ItemMainInfo" class="Set Box">
                <div class="">
                    <div class="row">
                        <div class="col-xs-12">
                            <header>
                                <div>
                                    Fragesatz mit <%= Model.QuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>
                                </div>
                                <h1 style="margin-top: 5px; font-size: 26px;">
                                    <%= Model.Name %>
                                </h1>
                            </header>
                        </div>
                        <div class="xxs-stack col-xs-4 col-sm-3">
                            <div class="ImageContainer">
                                <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.QuestionSet, "ImageContainer") %>
                            </div>
                        </div>
                        <div class="xxs-stack col-xs-8 col-sm-9">
                            <div>
                                <%= Model.Text %>
                            </div>
                            
                            <div class="row" >
                                <div class="col-md-5" style="display: inline-block; margin-top: 20px;">
                                    <% foreach (var category in Model.Set.Categories){ %>
                                        <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>    
                                    <% } %>
                                </div>        
                    
                                <div class="col-md-7" style="margin-top:6px;">                                
                                    <div style="font-weight: lighter; color: darkgrey;">Dein Wissensstand:</div>
                                    <div style="padding: 10px" id="knowledgeWheelContainer">
                                        <% Html.RenderPartial("/Views/Knowledge/Wheel/KnowledgeWheel.ascx", Model.KnowledgeSummary);  %>
                                    </div>
                                </div>
                            </div>

                            <div class="greyed" style="margin-top: 10px; margin-bottom: 10px; font-size: 11px;">
                                erstellt von <a href="<%= Links.UserDetail(Model.Creator) %>" ><%= Model.CreatorName %></a> 
                                <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>">vor <%= Model.CreationDateNiceText%> <i class="fa fa-info-circle"></i></span>
                                <div id="WuWiStat" class="show-tooltip" title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen" style="float: right;">
                                    <i class="fa fa-heart"></i>
                                    <span id="totalPins"><%= Model.TotalPins %>x</span>
                                </div>

                            </div>
                            <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                            <div class="BottomBar">
                                <div style="float: left; padding-top: 7px;">
                                    <div class="fb-share-button" style="width: 100%; float: left;" data-href="<%= Settings.CanonicalHost + Links.SetDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div> 
                                </div>
                                <a data-action="embed-set" style="display: inline-block; margin-top: 7px; margin-left: 7px;" href="#"><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</a>
                                
                                <div style="float: right">
                                    <span style="display: inline-block; font-size: 16px; font-weight: normal;" class="Pin" data-set-id="<%= Model.Id %>">
                                        <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                                    </span>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>    
            </div>
            <div class="row BoxButtonBar">
                <div class="BoxButtonColumn">
                    <% var tooltipGame = "Tritt gegen andere Nutzer im Echtzeit-Quizspiel an.";
                    if (Model.QuestionCount == 0)
                        tooltipGame = "Noch keine Fragen zum Spielen in diesem Fragesatz vorhanden";%>
                    <div class="BoxButton show-tooltip 
                        <%= !Model.IsLoggedIn ? "LookDisabled" : ""%> 
                        <%= Model.QuestionCount == 0 ? "LookNotClickable" : ""%>"
                        data-original-title="<%= tooltipGame %>">
                        <div class="BoxButtonIcon"><i class="fa fa-gamepad"></i></div>
                        <div class="BoxButtonText">
                            <span>Spiel starten</span>
                        </div>
                        <% if (Model.QuestionCount > 0) { %>
                            <a href="<%= Links.GameCreateFromSet(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="game">
                            </a>
                        <% } %>
                    </div>
                </div>
                <div class="BoxButtonColumn">
                    <% var tooltipDate = "Gib an, bis wann du alle Fragen in diesem Fragesatz lernen musst und erhalte deinen persönlichen Lernplan.";
                    if (Model.QuestionCount == 0)
                       tooltipDate = "Noch keine Fragen zum Lernen in diesem Fragesatz vorhanden";%>
                    <div class="BoxButton show-tooltip 
                        <%= !Model.IsLoggedIn ? "LookDisabled" : ""%>
                        <%= Model.QuestionCount == 0 ? "LookNotClickable" : ""%>"
                        data-original-title="<%= tooltipDate%>">
                        <div class="BoxButtonIcon"><i class="fa fa-calendar"></i></div>
                        <div class="BoxButtonText">
                            <span>Prüfungstermin anlegen</span> 
                        </div>
                        <% if (Model.QuestionCount > 0) { %>
                            <a href="<%= Links.DateCreateForSet(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create">
                            </a>
                        <% } %>
                    </div>
                </div>
                <div class="BoxButtonColumn">
                <% var tooltipTest = "Teste dein Wissen mit " + Settings.TestSessionQuestionCount + " zufällig ausgewählten Fragen und jeweils nur einem Antwortversuch.";
                    if (Model.QuestionCount == 0)
                        tooltipTest = "Noch keine Fragen zum Testen in diesem Fragesatz vorhanden";%>
                    <div class="BoxButton show-tooltip
                        <%= Model.QuestionCount == 0 ? "LookNotClickable" : ""%>" 
                        data-original-title="<%= tooltipTest %>">
                        <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
                        <div class="BoxButtonText">
                            <span>Wissen testen</span>
                        </div>
                        <% if (Model.QuestionCount > 0) { %>
                         <a href="<%= Links.TestSessionStartForSet(Model.Name, Model.Id) %>" rel="nofollow"></a>
                        <% } %>
                    </div>
                </div>
                <div class="BoxButtonColumn">
                    <% var tooltipLearn = "Lerne personalisiert genau die Fragen, die du am dringendsten wiederholen solltest.";
                    if (Model.QuestionCount == 0)
                       tooltipLearn = "Noch keine Fragen zum Lernen in diesem Fragesatz vorhanden";%>
                    <div class="BoxButton show-tooltip 
                        <%= !Model.IsLoggedIn ? "LookDisabled" : ""%>
                        <%= Model.QuestionCount == 0 ? "LookNotClickable" : ""%>" 
                        data-original-title="<%= tooltipLearn %>">
                        <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
                        <div class="BoxButtonText">
                            <span>Lernen</span>
                        </div>
                        <% if (Model.QuestionCount > 0) { %>
                            <a class="btn" data-btn="startLearningSession" data-allowed="logged-in" data-allowed-type="learning-session" href="<%= Links.StartLearningSesssionForSet(Model.Id) %>" rel="nofollow">
                            </a>
                        <% } %>
                    </div>
                </div>
            </div> 
            
            <% if (Model.Set.HasVideo) { 
                Html.RenderPartial("/Views/Sets/Detail/Video/SetVideo.ascx", new SetVideoModel(Model.Set));
            } %>

            <% if(Model.QuestionCount > 0) { %>
                <h4 style="margin-top: 30px; margin-bottom: 8px;">
                    Dieser Fragesatz enthält <%= Model.QuestionCount %> einzelne Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>
                </h4>
                <p class="greyed" style="margin-bottom: 10px;">
                    Wenn du auf eine Frage klickst, kannst du alle Fragen des Fragesatzes durchblättern. 
                    Um eine begrenzte Zahl an Fragen zu beantworten und eine Auswertung zu erhalten, nutze bitte die Schaltflächen LERNEN oder WISSEN TESTEN oben.
                </p>
            <% } else { %>
                <div style="margin-top: 30px; margin-bottom: 20px;">
                    Dieser Fragesatz enthält noch keine Fragen.
                </div>
            <% } %>

            <div id="rowContainer">
                <%  foreach(var questionRow in Model.QuestionsInSet){ %>
                    <% Html.RenderPartial("/Views/Sets/Detail/SetQuestionRowResult.ascx", questionRow); %>
                <% } %>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <% if (Model.QuestionsInSet.Any()){ %>
                        <div class="pull-right">
                            <a href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="display: inline-block; padding-right: 15px; margin-top: 29px;">
                                <i class="fa fa-gamepad" style="font-size: 18px;">&nbsp;&nbsp;</i>SPIEL STARTEN
                            </a>
                            <a class="btn <%= Model.IsLoggedIn ? "btn-primary" : "btn-link" %>" data-btn="startLearningSession" data-allowed="logged-in" data-allowed-type="learning-session" href="<%= Links.StartLearningSesssionForSet(Model.Id) %>" rel="nofollow">
                                <i class="fa fa-line-chart">&nbsp;&nbsp;</i>JETZT LERNEN
                            </a>
                            <a class="btn btn-primary" href="<%= Links.StartLearningSesssionForSet(Model.Id) %>" rel="nofollow">
                                <i class="fa fa-play-circle">&nbsp;&nbsp;</i>WISSEN TESTEN
                            </a>
                        </div>
                    <% } %>
                </div>
            </div> 

            <% if (Model.ContentRecommendationResult != null) { %>
                <h4 style="margin-top: 100px;">Das könnte dich auch interessieren:</h4>
                <div class="row CardsLandscape" id="contentRecommendation">
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

        </div>
        
        
        <div class="col-lg-10 col-xs-9 xxs-stack">
            
        </div>
    
    </div>
</asp:Content>