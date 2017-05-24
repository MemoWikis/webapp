<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="padding-bottom: 15px;">
    <div class="btn-group btn-breadcrumb MobileHide">
        <a href="/" class="btn btn-sm btn-default"><i class="fa fa-home"></i></a>
        <a href="<%= Links.CategoriesAll() %>" class="btn btn-sm btn-default">
            <% if (Model.Type != "Standard"){ %>
                <%= Model.Type %>
            <% } else { %> 
                Themen
            <% }  %>
        </a>
        
        <% foreach (var item in Model.BreadCrumb){%>
            <a href="<%= Links.CategoryDetail(item) %>" class="btn btn-sm btn-default"><%= item.Name %></a> 
        <%}%>
        
        <a href="#" class="btn btn-sm btn-default current"><%= Model.Category.Name %></a>

    </div>
    <div class="BreadcrumbsMobile DesktopHide">
        <a href="/" class=""><i class="fa fa-home"></i></a>
        <span> <i class="fa fa-chevron-right"></i> </span>
        <a href="<%= Links.CategoriesAll() %>" class="">
            <% if (Model.Type != "Standard"){ %>
                <%= Model.Type %>
            <% } else { %> 
                Themen
            <% }  %>
        </a>
        <span> <i class="fa fa-chevron-right"></i> </span>
        <% foreach (var item in Model.BreadCrumb){%>
            <a href="<%= Links.CategoryDetail(item) %>" class=""><%= item.Name %></a>
            <span> <i class="fa fa-chevron-right"></i> </span>
        <%}%>
        
        <a href="#" class="current"><%= Model.Category.Name %></a>

    </div>
</div>

<div id="ItemMainInfo" class="Category Box">
    <div class="">
        <div class="row">
            <div class="col-xs-12">
                <header>
                    <div class="greyed">
                        <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <%= Model.CountQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountQuestions, "n") %> und <%= Model.CountSets %> Lernset<%= StringUtils.PluralSuffix(Model.CountSets, "s") %>
                    </div>
                    <h1 style="margin-top: 5px; font-size: 26px;">
                       <%= Model.Name %>
                    </h1>
                </header>
            </div>
            <div class="xxs-stack col-xs-4 col-sm-3">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                </div>
            </div>
            <div class="xxs-stack col-xs-8 col-sm-9">
                
                <% if (Model.Type != "Standard") { %>
                    <div>                    
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
                
                <% if (!String.IsNullOrEmpty(Model.InfoUrl)){ %>
                    <div>
                        <div class="WikiLink" style="margin-top: 10px;">
                            <a href="<%= Model.InfoUrl %>" target="_blank" class="show-tooltip" title="<div style='white-space: normal; word-wrap: break-word; text-align:left; '>Link&nbsp;auf&nbsp;Wikipedia:&nbsp;<%= Model.InfoUrl %></div>" data-placement="left" data-html="true">
                                <% if(Links.IsLinkToWikipedia(Model.InfoUrl)){ %>
                                    <img src="/Images/wiki-24.png" style="margin-top: -1px;" />
                                <% } %>
                                <%= Model.InfoUrl %>
                            </a>
                        </div>
                    </div>
                <% } %>
            
                <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                <div class="BottomBar">
                    <div style="float: left; padding-top: 3px;">
                        <div class="fb-share-button" style="width: 100%" data-href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>                
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
        <div class="BoxButtonColumn">
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
                    <a href="<%= Links.GameCreateFromSets(Model.Category.GetSets().Select(s => s.Id).ToList()) %>" rel="nofollow"
                    data-allowed="logged-in" data-allowed-type="game">
                    </a>
                <% } %>
            </div>
        </div>
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
                <% if (Model.CountSets > 0)
                   { %>
                    <a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipTest = "Teste dein Wissen mit " + Settings.TestSessionQuestionCount + " zufällig ausgewählten Fragen zu diesem Thema und jeweils nur einem Antwortversuch.";
               if (Model.CountSets == 0 && Model.CountQuestions == 0)
                   tooltipTest = "Noch keine Lernsets oder Fragen zum Testen zu diesem Thema vorhanden"; %>
            <div class="BoxButton show-tooltip 
                <%= Model.CountSets == 0 && Model.CountQuestions == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipTest %>">
                <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
                <div class="BoxButtonText">
                    <span>Wissen testen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountQuestions > 0)
                   { %>
                    <a href="<%= Links.TestSessionStartForCategory(Model.Name, Model.Id) %>" rel="nofollow"></a>
                <% } %>
            </div>
        </div>
        <div class="BoxButtonColumn">
            <% var tooltipLearn = "Lerne zu diesem Thema genau die Fragen, die du am dringendsten wiederholen solltest.";
               if (Model.CountSets == 0 && Model.CountQuestions == 0)
                   tooltipLearn = "Noch keine Lernsets oder Fragen zum Lernen zu diesem Thema vorhanden"; %>
             <div class="BoxButton show-tooltip 
                <%= !Model.IsLoggedIn ? "LookDisabled" : "" %>
                <%= Model.CountSets == 0 && Model.CountQuestions == 0 ? "LookNotClickable" : "" %>"
                data-original-title="<%= tooltipLearn %>">
                <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
                <div class="BoxButtonText">
                    <span>Lernen</span>
                </div>
                <% if (Model.CountSets > 0 || Model.CountQuestions > 0)
                   { %>
                    <a href="<%= Links.StartCategoryLearningSession(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
                <% } %>
            </div>
        </div>
    </div>

<% } %>