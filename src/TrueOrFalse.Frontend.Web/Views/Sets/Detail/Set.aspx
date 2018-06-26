<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<SetModel>" %>
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
    <% Model.TopNavMenu.IsAnswerQuestionOrSetBreadCrumb = true; 
       Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem {Text = Model.Name});
       Model.TopNavMenu.IsCategoryBreadCrumb = false;%>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <% Model.SidebarModel.CardFooterText = Model.CreatorName; 
       Model.SidebarModel.AutorImageUrl = Model.ImageUrl_250; %>    
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

    <div id="ItemMainInfo" class="Set">
        <div class="row">
            <div class="col-xs-12">
                <header>
                    <div id="AboveMainHeading" class="greyed">
                        Lernset mit <%= Model.QuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>
                        <% if(Model.IsInstallationAdmin) { %>
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>    
                                        <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                            (<i class="fa fa-user-secret">&nbsp;</i><%= Model.Set.CopiedInstances.Count %> copies)
                                        </span>    
                        <% } %>
                    </div>
                    <div id="MainHeading">
                        <h1 style="margin-top: 5px;">
                            <%= Model.Name %>
                        </h1>
                    </div>
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
                            <% Html.RenderPartial("CategoryLabel", category); %>
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
                        <div class="fb-share-button" style="display: inline-block;" data-href="<%= Settings.CanonicalHost + Links.SetDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true">
                            <a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a>
                        </div> 
                                    
                        <div class="editLinks">
                            <a class="embedSetLink" data-action="embed-set" href="#"><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</a>
                                        <a data-toggle="modal" href="#modalCopySet" data-url="toSecurePost"><i class="fa fa-files-o"></i> Lernset kopieren</a>
                            <% if(Model.IsOwner || Model.IsInstallationAdmin){ %>
                                <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil">&nbsp;</i>Bearbeiten</a> 
                                <a href="<%= Links.CreateQuestion(setId: Model.Id) %>"><i class="fa fa-plus-circle">&nbsp;</i>Frage hinzufügen</a> 
                            <% } %>
                        </div>

                    </div>

                                
                    <div style="float: right">
                        <span style="display: inline-block; font-size: 16px; font-weight: normal;" class="Pin" data-set-id="<%= Model.Id %>">
                            <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </span>
                    </div>

                </div>
            </div>
        </div>

    </div>
    <div class="row BoxButtonBar">
        <div class="BoxButtonColumn">
            <% var tooltipGame = "Tritt gegen andere Nutzer im Echtzeit-Quizspiel an.";
            if (Model.QuestionCount == 0)
                tooltipGame = "Noch keine Fragen zum Spielen in diesem Lernset vorhanden";%>
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
            <% var tooltipDate = "Gib an, bis wann du alle Fragen in diesem Lernset lernen musst und erhalte deinen persönlichen Lernplan.";
            if (Model.QuestionCount == 0)
                tooltipDate = "Noch keine Fragen zum Lernen in diesem Lernset vorhanden";%>
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
                tooltipTest = "Noch keine Fragen zum Testen in diesem Lernset vorhanden";%>
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
                tooltipLearn = "Noch keine Fragen zum Lernen in diesem Lernset vorhanden";%>
            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : ""%>
                <%= Model.QuestionCount == 0 ? "LookNotClickable" : ""%>" 
                data-original-title="<%= tooltipLearn %>">
                <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
                <div class="BoxButtonText">
                    <span>Lernen</span>
                </div>
                <% if (Model.QuestionCount > 0) { %>
                    <a class="btn" data-btn="startLearningSession" data-allowed="logged-in" data-allowed-type="learning-session" href="<%= Links.StartLearningSessionForSet(Model.Id) %>" rel="nofollow">
                    </a>
                <% } %>
            </div>
        </div>
    </div> 
            
    <% if (Model.Set.HasVideo) {
            Html.RenderPartial("/Views/Sets/Detail/Video/SetVideo.ascx", new SetVideoModel(Model.Set));
        } else { %>
        <h3>Teste dein Wissen in diesem Lernset:</h3>
        <div class="setTestSessionNoStartScreen">
            <script src="https://memucho.de/views/widgets/w.js" data-t="templateset" data-id="<%= Model.Set.Id %>" data-width="100%" data-maxwidth="100%" data-logoon="false" data-questioncount="5"></script>
        </div>
    <% } %>

                

    <% if(Model.QuestionCount > 0) { %>
        <h4 style="margin-top: 80px;">
            Übersicht über alle enthaltene Fragen
            <% if(Model.IsOwner || Model.IsInstallationAdmin){ %>
                <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>" style="font-size: smaller;"><i class="fa fa-pencil">&nbsp;</i></a> 
            <% } %>
        </h4>

        <p style="margin-bottom: 30px; max-width: 100%" class="greyed">
            Dieses Lernset enthält <%= Model.QuestionCount %> einzelne Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>. 
            Du kannst in der Übersicht auf eine Frage klicken, um alle Fragen durchzublättern. 
            Um eine begrenzte Zahl an Fragen zu beantworten und eine Auswertung zu erhalten, nutze bitte die LERNEN- oder WISSEN TESTEN-Funktion oben.
        </p>
    <% } else { %>
        <% if (Model.IsOwner) { %>
            <div class="alert alert-info">
                <p>
                    <b>Dein Lernset enthält noch keine Fragen</b>
                </p>
                <p>
                    Bitte bearbeite dein Lernset, um vorhandene oder neue Fragen zu ergänzen. Den <i class="fa fa-pencil">&nbsp;</i>Bearbeiten-Link findest du auch oben.
                </p>
                <p style="margin-top: 15px;">
                    <a href="<%= Links.QuestionSetEdit(Model.Name, Model.Id) %>" class="btn btn-default">
                        <i class="fa fa-pencil"></i>
                        Lernset bearbeiten
                    </a>
                </p>


            </div>
            
        <% } else { %>
            <div style="margin-top: 30px; margin-bottom: 20px;">
                Dieses Lernset enthält noch keine Fragen.
            </div>
        <% } %>
    <% } %>

    <div id="rowContainer">
        <%  foreach(var questionRow in Model.QuestionsInSet){ %>
            <% Html.RenderPartial("/Views/Sets/Detail/SetQuestionRowResult.ascx", questionRow); %>
        <% } %>
    </div>
                
    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>


    <% if (Model.ContentRecommendationResult != null) { %>
        <h4 style="margin-top: 40px;">Das könnte dich auch interessieren:</h4>
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


    <% Html.RenderPartial("~/Views/Shared/LinkToTop.ascx");  %>


    <% Html.RenderPartial("~/Views/Sets/Detail/Modals/CopySetModal.ascx"); %>

</asp:Content>