<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div style="padding-bottom: 15px;">
    <div class="btn-group btn-breadcrumb">
        <a href="/" class="btn btn-sm btn-default"><i class="fa fa-home"></i></a>
        <a href="<%= Links.Categories() %>" class="btn btn-sm btn-default">
            <% if (Model.Type != "Standard"){ %>
                <%= Model.Type %>
            <% } else { %> 
                Kategorien
            <% }  %>
        </a>
        
        <% foreach (var item in Model.BreadCrumb){%>
            <a href="<%= Links.CategoryDetail(item) %>" class="btn btn-sm btn-default"><%= item.Name %></a> 
        <%}%>
        
        <a href="#" class="btn btn-sm btn-default current"><%= Model.Category.Name %></a>

    </div>
</div>

<div id="ItemMainInfo" class="Category Box">
    <div class="">
        <div class="row">
            <div class="col-xs-12">
                <header>
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
                
                <div>
                        <% if (Model.Type != "Standard") {
                        Html.RenderPartial("Reference", Model.Category);
                    }
                    
                    if (!String.IsNullOrEmpty(Model.Description)) {%>
                        <div class="Description"><span><%= Model.Description %></span></div>
                    <% } %>
                    
                    <% if (!String.IsNullOrEmpty(Model.WikiUrl)){ %>
                        <div class="WikiLink" style="margin-top: 10px;">
                            <a href="<%= Model.WikiUrl %>" target="_blank" class="show-tooltip" title="<div style='white-space: normal; word-wrap: break-word; text-align:left; '>Link&nbsp;auf&nbsp;Wikipedia:&nbsp;<%= Model.WikiUrl %></div>" data-placement="left" data-html="true">
                                <img src="/Images/wiki-24.png" style="margin-top: -1px;" /><%= Model.WikiUrl %>
                            </a>
                        </div>
                    <% } %>
                </div>
                            
                <% if(Model.AnswersTotal > 0) { %>
                    <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                    <div style="margin-top: 6px; font-size: 11px;">
                        In dieser Kategorie wurden
                        <%= Model.AnswersTotal + "x Frage" + StringUtils.PluralSuffix(Model.AnswersTotal, "n") %> beantwortet, 
                        davon <%= Model.CorrectnesProbability %>% richtig.
                    </div>
                <% } %>                

                <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                <div class="BottomBar">
                    <div style="float: left; padding-top: 3px;">
                        <div class="fb-share-button" style="width: 100%" data-href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>                
                    </div>
                   <%-- <a class="btn btn-primary show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                    </a>--%>
                </div>
            </div>
        </div>
    </div>    
</div>
<%--<div class="row">
    <div class="col-xs-12 col-sm-6" style="color: darkgray">
        Für Fragesätze in dieser Kategorie
    </div>
    <div class="col-xs-12 col-sm-6" style="color: darkgray">
        Für alle Inhalte in dieser Kategorie
    </div>
</div>--%>
<div class="row BoxButtonBar">
    <div class="BoxButtonColumn">
        <% var tooltipGame = "Tritt in dieser Kategorie gegen andere Nutzer im Echtzeit-Quizspiel an.";
           if (Model.CountSets == 0)
               tooltipGame = "Noch keine Fragesätze zum Spielen in dieser Kategorie vorhanden";%>

        <div class="BoxButton show-tooltip 
            <%= !Model.IsLoggedIn ? "LookDisabled" : ""%> 
            <%= Model.CountSets == 0 ? "LookNotClickable" : ""%>"
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
        <% var tooltipDate = "Gib an, bis wann du alle Fragesätze in dieser Kategorie lernen musst und erhalte deinen persönlichen Übungsplan.";
           if (Model.CountSets == 0)
               tooltipDate = "Noch keine Fragesätze in dieser Kategorie vorhanden";%>
        <div class="BoxButton show-tooltip 
            <%= !Model.IsLoggedIn ? "LookDisabled" : ""%>
            <%= Model.CountSets == 0 ? "LookNotClickable" : ""%>"
            data-original-title="<%= tooltipDate%>">
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
        <% var tooltipLearn = "Lerne aus dieser Kategorie genau die Fakten, die du am dringendsten wiederholen solltest.";
           if (Model.CountSets == 0 && Model.CountQuestions == 0)
               tooltipLearn = "Noch keine Fragesätze oder Fragen zum Lernen in dieser Kategorie vorhanden";%>
         <div class="BoxButton show-tooltip 
            <%= !Model.IsLoggedIn ? "LookDisabled" : ""%>
            <%= Model.CountSets == 0 && Model.CountQuestions == 0 ? "LookNotClickable" : ""%>"
            data-original-title="<%= tooltipLearn%>">
            <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
            <div class="BoxButtonText">
                <span>Üben</span>
            </div>
            <% if (Model.CountSets > 0 || Model.CountQuestions > 0) { %>
                <a href="<%= Links.StartCategoryLearningSession(Model.Id) %>" rel="nofollow" data-allowed="logged-in" data-allowed-type="date-create"></a>
            <% } %>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <% var tooltipTest = "Teste dein Wissen mit " + Settings.TestSessionQuestionCount + " zufällig ausgewählten Fragen aus dieser Kategorie und jeweils nur einem Antwortversuch.";
           if (Model.CountSets == 0 && Model.CountQuestions == 0)
               tooltipTest = "Noch keine Fragesätze oder Fragen zum Testen in dieser Kategorie vorhanden";%>
        <div class="BoxButton show-tooltip 
            <%= Model.CountSets == 0 && Model.CountQuestions == 0 ? "LookNotClickable" : ""%>"
            data-original-title="<%= tooltipTest %>">
            <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
            <div class="BoxButtonText">
                <span>Wissen testen</span>
            </div>
            <% if (Model.CountSets > 0 || Model.CountQuestions > 0) { %>
                <a href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" rel="nofollow"></a>
            <% } %>
        </div>
    </div>
</div>