<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<TopicOfWeek_2017_30Model>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="category-of-week-container">
    <h3 class="greyed">Thema der Woche</h3>
    <div class="separator-category"></div>

    <div class="category-of-week-container-inner">
        <h1>Psychologie</h1>

        <div class="ImageContainer category-main-image" style="clear: both;">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
        </div>

        <p>
            Die Psychologie ist eine Wissenschaft, die sich mit den seelischen Vorgängen im Menschen befasst. 
            Sie ist nicht nur ein sehr beliebtes Studienfach, sondern auch ein sehr beliebtes populärwissenschaftliches Thema.
            Etwas abfällig oder (selbst-)ironisch spricht man von "Küchenpsychologie", wenn es um eine 
            naive und unreflektierte Verwendung alltagspsychologischer Kenntnisse geht. Für andere ist die Psychologie
            vor allem die Kunst, Menschen einzuschätzen und zu lenken.
        </p>
        <p>
            Dabei hat die Psychologie tatsächlich eine Vielzahl an spannenden Forschungsfeldern und Erkenntnissen zu bieten.
            Wie vielfältig das Fach ist, zeigt unsere <a href="/Kategorien/Psychologie-Studium/264">Themenseite zum Studienfach Psychologie</a>: 
            Von der Lern-, Bio- oder Entwicklungspsychologie über die Arbeits- und Organisations-Psychologie bis zur Klinischen Psychologie. 
            Ein weiterer Schwerpunkt in dem Fach sind die <a href="/Kategorien/Statistik-fuer-PsychologInnen/649">Methoden</a>, 
            denn viele Erkenntnisse werden über quantitative Studien gewonnen.
        </p>

        <div class="category-of-week-footer">
            <a class="btn btn-lg btn-primary" href="/Kategorien/Psychologie-Studium/264">Zur Themenseite Psychologie</a>
            
            <span style="display: inline-block;" class="Pin float-right-sm-up" data-category-id="<%= Model.CategoryId %>">
                <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
            </span>
        </div>

        <div class="category-of-week-quiz">
            <script src="https://memucho.de/views/widgets/w.js" data-t="set" data-id="116" data-width="100%" data-hideKnowledgeBtn="true"></script>
        </div>
    </div>

    <div class="category-of-week-additional-recom">
        <h3>Entdecke weitere Themenbereiche der Psychologie:</h3>
        <div class="row" style="padding-left: -10px; padding-right: -10px;">
            <% foreach (var categoryId in Model.AdditionalCategoriesIds) { %>
                <div class="col-xs-6 col-md-3">
                    <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                </div>
            <% } %>
        </div>
    </div>
    <div class="separator-category"></div>

</div>
