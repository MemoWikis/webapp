<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="padding-bottom: 15px;">
    <div class="BreadcrumbsMobile DesktopHide">
        <% Html.RenderPartial("/Views/Categories/Detail/Partials/BreadCrumbMobile.ascx", Model); %>
    </div>
</div>

<div id="ItemMainInfo" class="Category Box">
    
    <div>
        <div class="row">
            <div class="col-xs-12">
                <header>
                    <div id="AboveMainHeading" class="greyed">
                        Thema mit und <%= Model.CountAggregatedQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountAggregatedQuestions, "n") %>
                        <% if(Model.IsInstallationAdmin) { %>
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fa fa-user-secret">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>    
                        <% } %>
                    </div>
                    <div id="MainHeading">
                        <h1>
                           <%= Model.Name %>
                        </h1>
                        <%--<% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>--%>
                    </div>
                </header>
            </div>
            <div class="xxs-stack col-xs-4 col-sm-3">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                </div>
            </div>
            <div class="xxs-stack col-xs-8 col-sm-9">
                
                <% if (Model.Type != "Standard") { %>
                    <div class="categoryDetailMainInfoReference">                    
                        <% Html.RenderPartial("Reference", Model.Category); %>
                    </div>
                <% } %>
                
                <div class="row">
                    <div class="col-md-12">
                        
                        <div  style="float: right; width: 300px;">
                            <div style="padding-left: 20px; font-weight: lighter; color: darkgrey;">Dein Wissensstand:</div>
                            <div style="padding-left: 20px; padding-bottom: 15px; padding-top: 7px;" id="knowledgeWheelContainer">
                                <% Html.RenderPartial("/Views/Knowledge/Wheel/KnowledgeWheel.ascx", Model.KnowledgeSummary);  %>
                            </div>
                        </div>

                        <div class="Description"><span><%= Model.Description %></span></div>
                    </div>
                    
                </div>
                
                <% if (!String.IsNullOrEmpty(Model.Url)){ %>
                    <div>
                        <div class="WikiLink">
                            <a href="<%= Model.Url %>" target="_blank" class="" title="" data-placement="left" data-html="true">
                                <i class='fa fa-external-link'>&nbsp;&nbsp;</i><%= string.IsNullOrEmpty(Model.Category.UrlLinkText) ? Model.Url : Model.Category.UrlLinkText %>
                            </a>
                        </div>
                    </div>
                <% } %>
                <% if (!String.IsNullOrEmpty(Model.WikipediaURL)){ %>
                    <div>
                        <div class="WikiLink">
                            <a href="<%= Model.WikipediaURL %>" target="_blank" class="show-tooltip" title="<%= Links.IsLinkToWikipedia(Model.WikipediaURL) ? "Link&nbsp;auf&nbsp;Wikipedia" : "" %>" data-placement="left" data-html="true">
                                <% if(Links.IsLinkToWikipedia(Model.WikipediaURL)){ %>
                                    <i class="fa fa-wikipedia-w">&nbsp;</i><% } %><%= Model.WikipediaURL %>
                            </a>
                        </div>
                    </div>
                <% } %>
            
                <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                <div class="BottomBar">
                    <div style="float: left; padding-top: 3px;">
                        <div class="navLinks">  
                            <% if(Model.IsOwnerOrAdmin){ %>
                                <a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" style="font-size: 12px;"><i class="fa fa-pencil"></i>&nbsp;<span class="visible-lg">bearbeiten</span></a> 
                            <% } %>
                            <% if(Model.IsInstallationAdmin){ %>
                                <a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" style="font-size: 12px;"><i class="fa fa-plus-circle"></i>&nbsp;<span class="visible-lg">Frage hinzufügen</span></a>
                            <% } %>
                        </div>                        
                            
                    </div>
                   
                    <div style="float: right">
                        <span style="display: inline-block; font-size: 16px; font-weight: normal;" class="Pin" data-category-id="<%= Model.Id %>">
                            <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                        </span>
                    </div>

                </div>
            </div>
        </div>
    </div>    
</div>

<% if (!Model.Category.DisableLearningFunctions) { %>

    <div class="row BoxButtonBar">
        <%--<div class="BoxButtonColumn">
            <% var tooltipGame = "Tritt zu diesem Thema gegen andere Nutzer im Echtzeit-Quizspiel an.";
               if (Model.CountSets == 0)
                   tooltipGame = "Noch keine Lernsets zum Spielen zu diesem Thema vorhanden"; %>

            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %> 
                <%= Model.CountSets == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipGame %>">
                <div class="BoxButtonIcon"><i class="fa fa-gamepad"></i></div>
                <div class="BoxButtonText">
                    <span>Spiel starten</span>
                </div>
                <% if (Model.CountSets > 0)
                   { %>
                    <a href="<%= Links.GameCreateFromCategory(Model.Id) %>" rel="nofollow"
                    data-allowed="logged-in" data-allowed-type="game">
                    </a>
                <% } %>
            </div>
        </div>--%>
        <div class="BoxButtonColumn">
            <% var tooltipDate = "Gib an, bis wann du alle Lernsets zu diesem Thema lernen musst und erhalte deinen persönlichen Lernplan.";
               if (Model.CountSets == 0)
                   tooltipDate = "Noch keine Lernsets zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %>
                <%= Model.CountSets == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipDate %>">
                <div class="BoxButtonIcon"><i class="fa fa-calendar"></i></div>
                <div class="BoxButtonText">
                    <span>Prüfungstermin anlegen</span> 
                </div>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipTest = "Teste dein Wissen mit " + Settings.TestSessionQuestionCount + " zufällig ausgewählten Fragen zu diesem Thema und jeweils nur einem Antwortversuch.";
               if (Model.CountSets == 0 && Model.CountAggregatedQuestions == 0)
                   tooltipTest = "Noch keine Lernsets oder Fragen zum Testen zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= Model.CountSets == 0 && Model.CountAggregatedQuestions == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipTest %>">
                <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
                <div class="BoxButtonText">
                    <span>Wissen testen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountAggregatedQuestions > 0)
                   { %>
                    <a href="<%= Links.TestSessionStartForCategory(Model.Name, Model.Id) %>" rel="nofollow"></a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipLearn = "Lerne zu diesem Thema genau die Fragen, die du am dringendsten wiederholen solltest.";
               if (Model.CountSets == 0 && Model.CountAggregatedQuestions == 0)
                   tooltipLearn = "Noch keine Lernsets oder Fragen zum Lernen zu diesem Thema vorhanden"; %>
             <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %>
                <%= Model.CountSets == 0 && Model.CountAggregatedQuestions == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipLearn %>">
                <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
                <div class="BoxButtonText">
                    <span>Lernen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountAggregatedQuestions > 0)
                   { %>
                    <a href="<%= Links.StartCategoryLearningSession(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
                <% } %>
            </div>
        </div>
    </div>

<% } %>

